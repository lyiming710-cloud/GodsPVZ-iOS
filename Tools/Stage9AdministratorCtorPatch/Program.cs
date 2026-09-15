using Mono.Cecil;
using Mono.Cecil.Cil;
using System.Globalization;
using System.Security.Cryptography;
using System.Text;

if (args.Length != 3)
{
    Console.Error.WriteLine("Usage: Stage9AdministratorCtorPatch <input.dll> <output.dll> <expected-input-sha256>");
    return 2;
}

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

const uint TargetToken = 0x0600000C;
const uint ModeToken = 0x0400000A;
const uint SuppliesIdToken = 0x040000BD;
const int ExpectedCodeSize = 191;
const int ExpectedLocals = 7;
const ulong NativeEntry = 0x181B82DB8;
const ulong NativeVa = 0x1802FEB60;
const ulong NativeEnd = 0x1802FED13;
const string NativeSha = "0fc07b9a066f57d91a828c58f90fca975de1e77bebf89d15b1f404c7b3f51da5";
const int ExpectedMethods = 2317;
const int ExpectedFields = 2802;

var input = Path.GetFullPath(args[0]);
var output = Path.GetFullPath(args[1]);
var expectedInputSha = args[2].Trim().ToLowerInvariant();
if (expectedInputSha.Length != 64 || expectedInputSha.Any(c => !Uri.IsHexDigit(c))) throw new InvalidDataException("expected input SHA invalid");
if (Sha(input) != expectedInputSha) throw new InvalidDataException("input SHA mismatch");
Console.WriteLine($"INPUT_SHA256 {Sha(input)}");

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

    var administrator = types.Single(t => t.FullName == "Administrator");
    var system3 = types.Single(t => t.FullName == "System3");
    var target = methods.Single(m => Raw(m) == TargetToken);
    if (target.DeclaringType != administrator || !target.IsConstructor || target.IsStatic || target.Parameters.Count != 0) throw new InvalidDataException("Administrator ctor identity drift");
    if (!target.HasBody || target.Body.CodeSize != ExpectedCodeSize || target.Body.Variables.Count != ExpectedLocals) throw new InvalidDataException($"target body fingerprint drift size={target.Body.CodeSize} locals={target.Body.Variables.Count}");
    var mode = fields.Single(f => Raw(f) == ModeToken);
    var suppliesId = fields.Single(f => Raw(f) == SuppliesIdToken);
    if (mode.DeclaringType != administrator || mode.Name != "mode" || mode.FieldType.FullName != "System.Int32") throw new InvalidDataException("Administrator::mode identity drift");
    if (suppliesId.DeclaringType != system3 || suppliesId.Name != "suppliesID" || suppliesId.FieldType.FullName != "System.Int32") throw new InvalidDataException("System3::suppliesID identity drift");

    beforeLocalSig = string.Join("|", target.Body.Variables.Select(v => TypeSig(v.VariableType)));
    beforeInit = target.Body.InitLocals;
    beforeHandlers = target.Body.ExceptionHandlers.Count;
    oldInstructionCount = target.Body.Instructions.Count;

    static List<int> Hits(MethodDefinition target, uint fieldToken)
    {
        var hits = new List<int>();
        for (int i = 0; i < target.Body.Instructions.Count - 1; i++)
        {
            var a = target.Body.Instructions[i];
            var b = target.Body.Instructions[i + 1];
            if (a.OpCode == OpCodes.Ldc_I8 && a.Operand is long n && n == 4294967295L &&
                b.OpCode == OpCodes.Stfld && b.Operand is FieldReference fr && Raw(fr.Resolve()) == fieldToken)
                hits.Add(i);
        }
        return hits;
    }

    var modeHits = Hits(target, ModeToken);
    var suppliesHits = Hits(target, SuppliesIdToken);
    if (modeHits.Count != 1 || suppliesHits.Count != 1) throw new InvalidDataException($"damaged pair count mode={modeHits.Count} suppliesID={suppliesHits.Count}");
    var allI8MinusOne = target.Body.Instructions.Count(i => i.OpCode == OpCodes.Ldc_I8 && i.Operand is long n && n == 4294967295L);
    if (allI8MinusOne != 2) throw new InvalidDataException($"Administrator ctor has {allI8MinusOne} I8 -1 constants; target-specific patch refused");

    Console.WriteLine($"NATIVE_AUTHORITY target_token=0x{TargetToken:X8} rid={target.MetadataToken.RID} method_pointer_entry=0x{NativeEntry:X} va=0x{NativeVa:X} end=0x{NativeEnd:X} native_slice_sha256={NativeSha}");
    Console.WriteLine("RECOVERY_STRATEGY type=Administrator own_field=mode own_field_token=0x0400000A child_type=System3 child_field=suppliesID child_field_token=0x040000BD two_width_repairs=1 old=ldc.i8_4294967295 new=ldc.i4.m1 metadata_changes=0 guards=0 exception_swallowing=0");

    var il = target.Body.GetILProcessor();
    foreach (var hit in new[] { modeHits[0], suppliesHits[0] }.OrderByDescending(x => x))
        il.Replace(target.Body.Instructions[hit], il.Create(OpCodes.Ldc_I4_M1));

    Directory.CreateDirectory(Path.GetDirectoryName(output)!);
    module.Write(output);
}

Console.WriteLine($"OUTPUT_SHA256 {Sha(output)}");
using (var module = ModuleDefinition.ReadModule(output, new ReaderParameters { InMemory=true, ReadingMode=ReadingMode.Immediate }))
{
    var types = AllTypes(module.Types).ToList();
    var methods = types.SelectMany(t => t.Methods).ToList();
    var fields = types.SelectMany(t => t.Fields).ToList();
    if (methods.Count != ExpectedMethods || fields.Count != ExpectedFields) throw new InvalidDataException("metadata count drift after reopen");
    var target = methods.Single(m => Raw(m) == TargetToken);
    if (target.Body.Instructions.Count != oldInstructionCount) throw new InvalidDataException("instruction count drift");
    if (target.Body.Variables.Count != ExpectedLocals || target.Body.InitLocals != beforeInit || target.Body.ExceptionHandlers.Count != beforeHandlers) throw new InvalidDataException("body metadata drift");
    var afterLocalSig = string.Join("|", target.Body.Variables.Select(v => TypeSig(v.VariableType)));
    if (afterLocalSig != beforeLocalSig) throw new InvalidDataException("local signature drift");
    if (target.Body.Instructions.Count(i => i.OpCode == OpCodes.Ldc_I8 && i.Operand is long n && n == 4294967295L) != 0) throw new InvalidDataException("reopened target still contains I8 -1");

    foreach (var ft in new[] { ModeToken, SuppliesIdToken })
    {
        var stores = target.Body.Instructions.Select((ins, idx) => (ins, idx))
            .Where(x => x.ins.OpCode == OpCodes.Stfld && x.ins.Operand is FieldReference fr && Raw(fr.Resolve()) == ft).ToList();
        if (stores.Count != 1 || stores[0].idx == 0 || target.Body.Instructions[stores[0].idx - 1].OpCode != OpCodes.Ldc_I4_M1)
            throw new InvalidDataException($"reopened field 0x{ft:X8} is not fed by ldc.i4.m1 exactly once");
    }

    var changedM = methods.Where(m => beforeM[Raw(m)] != MethodSig(m)).Select(Raw).OrderBy(x => x).ToList();
    if (changedM.Count != 1 || changedM[0] != TargetToken) throw new InvalidDataException("semantic isolation failed: " + string.Join(',', changedM.Select(x => $"0x{x:X8}")));
    var changedF = fields.Where(f => beforeF[Raw(f)] != FieldSig(f)).Select(Raw).ToList();
    if (changedF.Count != 0) throw new InvalidDataException("field metadata isolation failed");
    var afterRefs = string.Join("\n", module.AssemblyReferences.Select(a => a.FullName).OrderBy(x => x, StringComparer.Ordinal));
    if (beforeRefs != afterRefs) throw new InvalidDataException("assembly reference set changed");
    if (module.AssemblyReferences.Any(a => a.Name == "System.Private.CoreLib")) throw new InvalidDataException("System.Private.CoreLib pollution");

    Console.WriteLine($"REOPEN_ADMINISTRATOR_CTOR_PASS token=0x{TargetToken:X8} mode_i4_m1=1 system3_suppliesID_i4_m1=1 ldc_i8_minus_one=0 instructions={oldInstructionCount} locals={ExpectedLocals}");
    Console.WriteLine($"SEMANTIC_ISOLATION_PASS untouched_methods={ExpectedMethods - 1} changed_methods=1 target=0x{TargetToken:X8}");
    Console.WriteLine($"FIELD_METADATA_ISOLATION_PASS unchanged_fields={ExpectedFields} changed_fields=0");
    Console.WriteLine("FRAMEWORK_REFERENCE_GATE_PASS system_private_corelib_refs=0 assembly_reference_set_unchanged=1");
}

return 0;
