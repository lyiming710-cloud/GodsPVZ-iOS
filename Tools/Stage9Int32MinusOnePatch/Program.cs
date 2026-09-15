using Mono.Cecil;
using Mono.Cecil.Cil;
using System.Globalization;
using System.Security.Cryptography;
using System.Text;

if (args.Length != 13)
{
    Console.Error.WriteLine("Usage: Stage9Int32MinusOnePatch <input.dll> <output.dll> <expected-input-sha256> <type> <target-token-hex> <field-name> <field-token-hex> <old-code-size> <old-locals> <native-entry> <native-va> <native-end> <native-sha256>");
    return 2;
}

static uint U32(string s) => Convert.ToUInt32(s.StartsWith("0x", StringComparison.OrdinalIgnoreCase) ? s[2..] : s, 16);
static ulong U64(string s) => Convert.ToUInt64(s.StartsWith("0x", StringComparison.OrdinalIgnoreCase) ? s[2..] : s, 16);
static string Sha(string p) => Convert.ToHexString(SHA256.HashData(File.ReadAllBytes(p))).ToLowerInvariant();
static IEnumerable<TypeDefinition> AllTypes(IEnumerable<TypeDefinition> roots)
{
    foreach (var t in roots)
    {
        yield return t;
        foreach (var n in AllTypes(t.NestedTypes)) yield return n;
    }
}
static uint Raw(IMetadataTokenProvider p) => p.MetadataToken.ToUInt32();
static string Scope(IMetadataScope? s) => s switch
{
    AssemblyNameReference a => a.Name,
    ModuleDefinition m => m.Assembly?.Name?.Name ?? m.Name,
    ModuleReference mr => mr.Name,
    _ => s?.ToString() ?? "<null>"
};
static string TypeSig(TypeReference t) => $"{t.FullName}@{Scope(t.Scope)}";
static string OpSig(object? o, MethodDefinition owner)
{
    if (o is null) return "";
    if (o is Instruction i) return $"I#{owner.Body.Instructions.IndexOf(i)}";
    if (o is Instruction[] sw) return "SW[" + string.Join(',', sw.Select(i => owner.Body.Instructions.IndexOf(i))) + "]";
    if (o is MethodReference mr) return $"M:{mr.FullName}@{Scope(mr.DeclaringType.Scope)}";
    if (o is FieldReference fr) return $"F:{fr.FullName}@{Scope(fr.DeclaringType.Scope)}";
    if (o is TypeReference tr) return $"T:{TypeSig(tr)}";
    if (o is VariableDefinition v) return $"V:{v.Index}:{TypeSig(v.VariableType)}";
    if (o is ParameterDefinition p) return $"P:{p.Index}:{TypeSig(p.ParameterType)}";
    if (o is string s) return "S:" + Convert.ToBase64String(Encoding.UTF8.GetBytes(s));
    if (o is float f) return $"R4:{BitConverter.SingleToInt32Bits(f):X8}";
    if (o is double d) return $"R8:{BitConverter.DoubleToInt64Bits(d):X16}";
    return "C:" + Convert.ToString(o, CultureInfo.InvariantCulture);
}
static string MethodSig(MethodDefinition m)
{
    var sb = new StringBuilder();
    sb.Append(m.FullName).Append('|').Append(m.Attributes).Append('|').Append(m.ImplAttributes).Append('|');
    if (!m.HasBody) return sb.Append("NOBODY").ToString();
    sb.Append("init=").Append(m.Body.InitLocals).Append(";max=").Append(m.Body.MaxStackSize).Append(';');
    foreach (var v in m.Body.Variables) sb.Append("L:").Append(v.Index).Append(':').Append(TypeSig(v.VariableType)).Append(';');
    foreach (var i in m.Body.Instructions) sb.Append("I:").Append(i.OpCode.Code).Append(':').Append(OpSig(i.Operand, m)).Append(';');
    foreach (var h in m.Body.ExceptionHandlers)
    {
        int Idx(Instruction? x) => x is null ? -1 : m.Body.Instructions.IndexOf(x);
        sb.Append("EH:").Append(h.HandlerType).Append(':').Append(Idx(h.TryStart)).Append(':').Append(Idx(h.TryEnd)).Append(':')
          .Append(Idx(h.HandlerStart)).Append(':').Append(Idx(h.HandlerEnd)).Append(':').Append(Idx(h.FilterStart)).Append(':')
          .Append(h.CatchType is null ? "" : TypeSig(h.CatchType)).Append(';');
    }
    return sb.ToString();
}
static string FieldSig(FieldDefinition f)
{
    var attrs = string.Join(',', f.CustomAttributes.Select(a => a.AttributeType.FullName).OrderBy(x => x, StringComparer.Ordinal));
    var c = f.HasConstant ? Convert.ToString(f.Constant, CultureInfo.InvariantCulture) ?? "<null>" : "<none>";
    return $"{f.FullName}|{f.Attributes}|{TypeSig(f.FieldType)}|const={c}|ca={attrs}";
}

var input = Path.GetFullPath(args[0]);
var output = Path.GetFullPath(args[1]);
var expectedInputSha = args[2].Trim().ToLowerInvariant();
var typeName = args[3];
var targetToken = U32(args[4]);
var fieldName = args[5];
var fieldToken = U32(args[6]);
var oldCodeSize = int.Parse(args[7], CultureInfo.InvariantCulture);
var oldLocals = int.Parse(args[8], CultureInfo.InvariantCulture);
var nativeEntry = U64(args[9]);
var nativeVa = U64(args[10]);
var nativeEnd = U64(args[11]);
var nativeSha = args[12].Trim().ToLowerInvariant();
if (expectedInputSha.Length != 64 || expectedInputSha.Any(c => !Uri.IsHexDigit(c))) throw new InvalidDataException("expected input SHA invalid");
if (nativeSha.Length != 64 || nativeSha.Any(c => !Uri.IsHexDigit(c))) throw new InvalidDataException("native SHA invalid");
if (Sha(input) != expectedInputSha) throw new InvalidDataException("input SHA mismatch");
Console.WriteLine($"INPUT_SHA256 {Sha(input)}");

const int ExpectedMethods = 2317;
const int ExpectedFields = 2802;
var beforeM = new Dictionary<uint,string>();
var beforeF = new Dictionary<uint,string>();
string beforeRefs;
string beforeLocalSig;
bool beforeInit;
int beforeHandlers;
int oldInstructionCount;
using (var module = ModuleDefinition.ReadModule(input, new ReaderParameters { InMemory=true, ReadingMode=ReadingMode.Immediate }))
{
    var types = AllTypes(module.Types).ToList();
    var methods = types.SelectMany(t => t.Methods).ToList();
    var fields = types.SelectMany(t => t.Fields).ToList();
    if (methods.Count != ExpectedMethods || fields.Count != ExpectedFields) throw new InvalidDataException("metadata count drift");
    if (module.AssemblyReferences.Any(a => a.Name == "System.Private.CoreLib")) throw new InvalidDataException("input references System.Private.CoreLib");
    foreach (var m in methods) beforeM[Raw(m)] = MethodSig(m);
    foreach (var f in fields) beforeF[Raw(f)] = FieldSig(f);
    beforeRefs = string.Join("\n", module.AssemblyReferences.Select(a => a.FullName).OrderBy(x => x, StringComparer.Ordinal));

    var t = types.Single(x => x.FullName == typeName);
    var target = methods.Single(m => Raw(m) == targetToken);
    if (target.DeclaringType != t || !target.IsConstructor || target.IsStatic || target.Parameters.Count != 0) throw new InvalidDataException("target ctor identity drift");
    if (!target.HasBody || target.Body.CodeSize != oldCodeSize || target.Body.Variables.Count != oldLocals) throw new InvalidDataException($"target body fingerprint drift size={target.Body.CodeSize} locals={target.Body.Variables.Count}");
    var field = fields.Single(f => Raw(f) == fieldToken);
    if (field.DeclaringType != t || field.Name != fieldName || field.FieldType.FullName != "System.Int32") throw new InvalidDataException("target field identity drift");

    beforeLocalSig = string.Join("|", target.Body.Variables.Select(v => TypeSig(v.VariableType)));
    beforeInit = target.Body.InitLocals;
    beforeHandlers = target.Body.ExceptionHandlers.Count;
    oldInstructionCount = target.Body.Instructions.Count;

    var hits = new List<int>();
    for (int i=0;i<target.Body.Instructions.Count-1;i++)
    {
        var a=target.Body.Instructions[i]; var b=target.Body.Instructions[i+1];
        if (a.OpCode == OpCodes.Ldc_I8 && a.Operand is long n && n == 4294967295L && b.OpCode == OpCodes.Stfld && b.Operand is FieldReference fr && Raw(fr.Resolve()) == fieldToken)
            hits.Add(i);
    }
    if (hits.Count != 1) throw new InvalidDataException($"target damaged pair count={hits.Count}");
    var allI8MinusOne = target.Body.Instructions.Count(i => i.OpCode == OpCodes.Ldc_I8 && i.Operand is long n && n == 4294967295L);
    if (allI8MinusOne != 1) throw new InvalidDataException($"ctor has {allI8MinusOne} I8 -1 constants; generic single-op patch forbidden");

    Console.WriteLine($"NATIVE_AUTHORITY target_token=0x{targetToken:X8} rid={target.MetadataToken.RID} method_pointer_entry=0x{nativeEntry:X} va=0x{nativeVa:X} end=0x{nativeEnd:X} native_slice_sha256={nativeSha}");
    Console.WriteLine($"RECOVERY_STRATEGY type={typeName} field={fieldName} field_token=0x{fieldToken:X8} single_instruction_replace=1 old=ldc.i8_4294967295 new=ldc.i4.m1 metadata_changes=0 guards=0 exception_swallowing=0");
    var il=target.Body.GetILProcessor();
    il.Replace(target.Body.Instructions[hits[0]], il.Create(OpCodes.Ldc_I4_M1));
    Directory.CreateDirectory(Path.GetDirectoryName(output)!);
    module.Write(output);
}

Console.WriteLine($"OUTPUT_SHA256 {Sha(output)}");
using (var module = ModuleDefinition.ReadModule(output, new ReaderParameters { InMemory=true, ReadingMode=ReadingMode.Immediate }))
{
    var types=AllTypes(module.Types).ToList(); var methods=types.SelectMany(t=>t.Methods).ToList(); var fields=types.SelectMany(t=>t.Fields).ToList();
    if (methods.Count != ExpectedMethods || fields.Count != ExpectedFields) throw new InvalidDataException("metadata count drift after reopen");
    var target=methods.Single(m=>Raw(m)==targetToken); var field=fields.Single(f=>Raw(f)==fieldToken);
    if (target.Body.Instructions.Count != oldInstructionCount) throw new InvalidDataException("instruction count drift");
    if (target.Body.Variables.Count != oldLocals || target.Body.InitLocals != beforeInit || target.Body.ExceptionHandlers.Count != beforeHandlers) throw new InvalidDataException("body metadata drift");
    var afterLocalSig=string.Join("|", target.Body.Variables.Select(v=>TypeSig(v.VariableType)));
    if (afterLocalSig != beforeLocalSig) throw new InvalidDataException("local signature drift");
    var fixedHits=target.Body.Instructions.Count(i=>i.OpCode==OpCodes.Ldc_I4_M1);
    var badHits=target.Body.Instructions.Count(i=>i.OpCode==OpCodes.Ldc_I8 && i.Operand is long n && n==4294967295L);
    var stores=target.Body.Instructions.Count(i=>i.OpCode==OpCodes.Stfld && i.Operand is FieldReference fr && Raw(fr.Resolve())==fieldToken);
    if (fixedHits < 1 || badHits != 0 || stores != 1 || field.FieldType.FullName != "System.Int32") throw new InvalidDataException("reopened target semantic gate failed");
    var changedM=methods.Where(m=>beforeM[Raw(m)]!=MethodSig(m)).Select(Raw).OrderBy(x=>x).ToList();
    if(changedM.Count!=1 || changedM[0]!=targetToken) throw new InvalidDataException("semantic isolation failed: "+string.Join(',',changedM.Select(x=>$"0x{x:X8}")));
    var changedF=fields.Where(f=>beforeF[Raw(f)]!=FieldSig(f)).Select(Raw).ToList();
    if(changedF.Count!=0) throw new InvalidDataException("field metadata isolation failed");
    var afterRefs=string.Join("\n", module.AssemblyReferences.Select(a=>a.FullName).OrderBy(x=>x,StringComparer.Ordinal));
    if(beforeRefs!=afterRefs) throw new InvalidDataException("assembly reference set changed");
    if(module.AssemblyReferences.Any(a=>a.Name=="System.Private.CoreLib")) throw new InvalidDataException("System.Private.CoreLib pollution");
    Console.WriteLine($"REOPEN_INT32_MINUS_ONE_PASS type={typeName} token=0x{targetToken:X8} field={fieldName} field_token=0x{fieldToken:X8} ldc_i4_m1_present=1 ldc_i8_minus_one=0 target_stfld=1 instructions={oldInstructionCount} locals={oldLocals}");
    Console.WriteLine($"SEMANTIC_ISOLATION_PASS untouched_methods={ExpectedMethods-1} changed_methods=1 target=0x{targetToken:X8}");
    Console.WriteLine($"FIELD_METADATA_ISOLATION_PASS unchanged_fields={ExpectedFields} changed_fields=0");
    Console.WriteLine("FRAMEWORK_REFERENCE_GATE_PASS system_private_corelib_refs=0 assembly_reference_set_unchanged=1");
}
return 0;
