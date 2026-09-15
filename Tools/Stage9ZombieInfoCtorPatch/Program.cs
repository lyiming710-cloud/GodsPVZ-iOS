using Mono.Cecil;
using Mono.Cecil.Cil;
using System.Globalization;
using System.Security.Cryptography;
using System.Text;

if (args.Length != 3)
{
    Console.Error.WriteLine("Usage: Stage9ZombieInfoCtorPatch <input.dll> <output.dll> <expected-input-sha256>");
    return 2;
}

const uint TargetToken = 0x06000152;
const int ExpectedRid = 338;
const int ExpectedMethods = 2317;
const int ExpectedFields = 2802;
const int ExpectedOldCodeSize = 89;
const int ExpectedOldLocals = 5;
const string NativeSpanSha = "4ccabbc3bb2ebdc0f06c97337a2473d0855c91c94c5c753635bf8e167ed90c56";

static IEnumerable<TypeDefinition> AllTypes(IEnumerable<TypeDefinition> roots)
{
    foreach (var t in roots)
    {
        yield return t;
        foreach (var n in AllTypes(t.NestedTypes)) yield return n;
    }
}
static uint Raw(IMetadataTokenProvider p) => p.MetadataToken.ToUInt32();
static string Sha(string p) => Convert.ToHexString(SHA256.HashData(File.ReadAllBytes(p))).ToLowerInvariant();
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
if (expectedInputSha.Length != 64 || expectedInputSha.Any(c => !Uri.IsHexDigit(c))) throw new InvalidDataException("expected input SHA is not 64 hex chars");
if (Sha(input) != expectedInputSha) throw new InvalidDataException("input SHA mismatch");
Console.WriteLine($"INPUT_SHA256 {Sha(input)}");

var beforeM = new Dictionary<uint,string>();
var beforeF = new Dictionary<uint,string>();
string beforeRefs;
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

    var t = types.Single(x => x.FullName == "ZombieInfo");
    var target = t.Methods.Single(m => m.IsConstructor && !m.IsStatic && m.Parameters.Count == 0);
    if (Raw(target) != TargetToken || target.MetadataToken.RID != ExpectedRid || target.Body.CodeSize != ExpectedOldCodeSize || target.Body.Variables.Count != ExpectedOldLocals)
        throw new InvalidDataException($"ZombieInfo ctor fingerprint drift token=0x{Raw(target):X8} rid={target.MetadataToken.RID} size={target.Body.CodeSize} locals={target.Body.Variables.Count}");
    if (!target.Body.Instructions.Any(i => i.Offset == 0x40 && i.OpCode == OpCodes.Ldc_I8))
        throw new InvalidDataException("expected painterID ldc.i8 corruption missing");
    if (!target.Body.Instructions.Any(i => i.Operand is string s && s.Contains("Method not found @180302170", StringComparison.Ordinal)))
        throw new InvalidDataException("expected fake base-ctor diagnostic missing");

    FieldDefinition F(string n) => t.Fields.Single(f => f.Name == n);
    var healthPoint=F("healthPoint"); var attackPoint=F("attackPoint"); var speedRating=F("speedRating");
    var enemyRequirements=F("enemyRequirements"); var painterID=F("painterID");
    var objectCtor = methods.SelectMany(m => m.HasBody ? m.Body.Instructions : Enumerable.Empty<Instruction>())
        .Select(i => i.Operand).OfType<MethodReference>()
        .Where(m => m.Name == ".ctor" && m.DeclaringType.FullName == "System.Object" && m.Parameters.Count == 0)
        .GroupBy(m => m.FullName + "@" + Scope(m.DeclaringType.Scope), StringComparer.Ordinal).Select(g => g.First()).Single();

    Console.WriteLine($"NATIVE_AUTHORITY target_token=0x{TargetToken:X8} rid={ExpectedRid} method_pointer_entry=0x181B837E8 va=0x1803251F0 end=0x180325261 native_span_sha256={NativeSpanSha}");
    Console.WriteLine("NATIVE_SEMANTICS healthPoint=270 attackPoint=100 speedRating=3 enemyRequirements=bool[10] painterID=-1 base_ctor=System.Object");

    var body=target.Body; body.Instructions.Clear(); body.Variables.Clear(); body.ExceptionHandlers.Clear(); body.InitLocals=false; body.MaxStackSize=3;
    var il=body.GetILProcessor();
    void StoreR4(FieldDefinition f,float v){ il.Append(il.Create(OpCodes.Ldarg_0)); il.Append(il.Create(OpCodes.Ldc_R4,v)); il.Append(il.Create(OpCodes.Stfld,f)); }
    void StoreI4(FieldDefinition f,int v){ il.Append(il.Create(OpCodes.Ldarg_0)); il.Append(il.Create(OpCodes.Ldc_I4,v)); il.Append(il.Create(OpCodes.Stfld,f)); }
    StoreR4(healthPoint,270f);
    StoreR4(attackPoint,100f);
    StoreI4(speedRating,3);
    il.Append(il.Create(OpCodes.Ldarg_0)); il.Append(il.Create(OpCodes.Ldc_I4,10)); il.Append(il.Create(OpCodes.Newarr,module.TypeSystem.Boolean)); il.Append(il.Create(OpCodes.Stfld,enemyRequirements));
    StoreI4(painterID,-1);
    il.Append(il.Create(OpCodes.Ldarg_0)); il.Append(il.Create(OpCodes.Call,objectCtor)); il.Append(il.Create(OpCodes.Ret));
    Directory.CreateDirectory(Path.GetDirectoryName(output)!); module.Write(output);
}

Console.WriteLine($"OUTPUT_SHA256 {Sha(output)}");
using (var module = ModuleDefinition.ReadModule(output, new ReaderParameters { InMemory=true, ReadingMode=ReadingMode.Immediate }))
{
    var types=AllTypes(module.Types).ToList(); var methods=types.SelectMany(t=>t.Methods).ToList(); var fields=types.SelectMany(t=>t.Fields).ToList();
    var target=methods.Single(m=>Raw(m)==TargetToken);
    if(target.MetadataToken.RID!=ExpectedRid || target.Body.Variables.Count!=0) throw new InvalidDataException("reopen target/local drift");
    if(target.Body.Instructions.Any(i=>i.OpCode==OpCodes.Ldc_I8)) throw new InvalidDataException("ldc.i8 corruption remains");
    if(target.Body.Instructions.Any(i=>i.Operand is string s && s.Contains("Method not found",StringComparison.Ordinal))) throw new InvalidDataException("fake diagnostic remains");
    if(target.Body.Instructions.Count(i=>i.OpCode==OpCodes.Newarr && i.Operand is TypeReference tr && tr.FullName=="System.Boolean")!=1) throw new InvalidDataException("bool[10] allocation missing");
    var changedM=methods.Where(m=>beforeM[Raw(m)]!=MethodSig(m)).Select(Raw).OrderBy(x=>x).ToList();
    if(changedM.Count!=1 || changedM[0]!=TargetToken) throw new InvalidDataException("semantic isolation failed: "+string.Join(',',changedM.Select(x=>$"0x{x:X8}")));
    var changedF=fields.Where(f=>beforeF[Raw(f)]!=FieldSig(f)).Select(Raw).ToList(); if(changedF.Count!=0) throw new InvalidDataException("field metadata drift");
    var afterRefs=string.Join("\n",module.AssemblyReferences.Select(a=>a.FullName).OrderBy(x=>x,StringComparer.Ordinal)); if(afterRefs!=beforeRefs) throw new InvalidDataException("assembly reference set changed");
    if(module.AssemblyReferences.Any(a=>a.Name=="System.Private.CoreLib")) throw new InvalidDataException("System.Private.CoreLib pollution");
    Console.WriteLine($"REOPEN_ZOMBIEINFO_CTOR_PASS token=0x{TargetToken:X8} code_size={target.Body.CodeSize} locals=0 ldc_i8=0 bool_array_alloc=1 fake_diagnostics=0");
    Console.WriteLine($"SEMANTIC_ISOLATION_PASS untouched_methods={ExpectedMethods-1} changed_methods=1 target=0x{TargetToken:X8}");
    Console.WriteLine($"FIELD_METADATA_ISOLATION_PASS unchanged_fields={ExpectedFields} changed_fields=0");
    Console.WriteLine("FRAMEWORK_REFERENCE_GATE_PASS system_private_corelib_refs=0 assembly_reference_set_unchanged=1");
}
return 0;
