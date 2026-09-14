using Mono.Cecil;
using Mono.Cecil.Cil;
using System.Globalization;
using System.Security.Cryptography;
using System.Text;

if (args.Length != 4)
{
    Console.Error.WriteLine("Usage: Stage9PlantDetailUpdatePatch <input.dll> <output.dll> <expected-input-sha256> <UnityEngine.CoreModule.dll>");
    return 2;
}

const string ExpectedCoreModuleSha = "83192116b34abac2bb1797ec0e3943362be33b028b1eefd2e561d5fdc1c3312f";
const uint TargetToken = 0x06000661;
const int ExpectedRid = 1633;
const int ExpectedMethods = 2317;
const int ExpectedFields = 2802;
const int ExpectedOldCodeSize = 355;
const int ExpectedOldLocals = 14;
const string NativeSliceSha = "8d2c51790690cb695fa590e65e09378555174d4f050dba2a716a6530b3052996";

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
var coreModulePath = Path.GetFullPath(args[3]);
if (expectedInputSha.Length != 64 || expectedInputSha.Any(c => !Uri.IsHexDigit(c))) throw new InvalidDataException("expected input SHA is not 64 hex chars");
if (Sha(input) != expectedInputSha) throw new InvalidDataException("input SHA mismatch");
if (Sha(coreModulePath) != ExpectedCoreModuleSha) throw new InvalidDataException("UnityEngine.CoreModule SHA mismatch");
Console.WriteLine($"INPUT_SHA256 {Sha(input)}");
Console.WriteLine($"UNITY_COREMODULE_SHA256 {Sha(coreModulePath)}");

using var coreModule = ModuleDefinition.ReadModule(coreModulePath, new ReaderParameters { InMemory=true, ReadingMode=ReadingMode.Immediate });
var vector3 = coreModule.GetType("UnityEngine.Vector3") ?? throw new InvalidDataException("UnityEngine.Vector3 missing from exact CoreModule");
var getForwardDef = vector3.Methods.Single(m => m.Name == "get_forward" && m.IsStatic && m.Parameters.Count == 0 && m.ReturnType.FullName == "UnityEngine.Vector3");

var beforeM = new Dictionary<uint,string>();
var beforeF = new Dictionary<uint,string>();

using (var module = ModuleDefinition.ReadModule(input, new ReaderParameters { InMemory=true, ReadingMode=ReadingMode.Immediate }))
{
    if (module.AssemblyReferences.Any(a => a.Name == "System.Private.CoreLib")) throw new InvalidDataException("input references System.Private.CoreLib");
    var types = AllTypes(module.Types).ToList();
    var methods = types.SelectMany(t => t.Methods).ToList();
    var fields = types.SelectMany(t => t.Fields).ToList();
    if (methods.Count != ExpectedMethods || fields.Count != ExpectedFields) throw new InvalidDataException("metadata count drift");
    foreach (var m in methods) beforeM[Raw(m)] = MethodSig(m);
    foreach (var f in fields) beforeF[Raw(f)] = FieldSig(f);

    var t = types.Single(x => x.FullName == "Plant_DetaiPage");
    var target = t.Methods.Single(m => m.Name == "Update" && m.Parameters.Count == 0);
    if (Raw(target) != TargetToken || target.MetadataToken.RID != ExpectedRid || target.Body.CodeSize != ExpectedOldCodeSize || target.Body.Variables.Count != ExpectedOldLocals)
        throw new InvalidDataException("Plant_DetaiPage.Update fingerprint drift");
    if (target.Body.Variables[13].VariableType.FullName != "System.Object") throw new InvalidDataException("expected corrupt object local 13 missing");

    var badAxisLoads = target.Body.Instructions.Where(i =>
        i.OpCode == OpCodes.Ldloca && i.Operand is VariableDefinition v && v.Index == 13 && v.VariableType.FullName == "System.Object").ToList();
    if (badAxisLoads.Count != 2 || badAxisLoads[0].Offset != 0x31 || badAxisLoads[1].Offset != 0x79)
        throw new InvalidDataException("expected two corrupt Object& Rotate axis loads at IL_0031/IL_0079");
    var rotateCalls = target.Body.Instructions.Where(i =>
        (i.OpCode == OpCodes.Call || i.OpCode == OpCodes.Callvirt) && i.Operand is MethodReference m && m.DeclaringType.FullName == "UnityEngine.Transform" && m.Name == "Rotate" && m.Parameters.Count == 3).ToList();
    if (rotateCalls.Count != 2 || rotateCalls[0].Offset != 0x3E || rotateCalls[1].Offset != 0x86)
        throw new InvalidDataException("expected two Transform.Rotate calls at IL_003E/IL_0086");
    foreach (var r in rotateCalls)
    {
        var m = (MethodReference)r.Operand;
        if (m.Parameters[0].ParameterType.FullName != "UnityEngine.Vector3" || m.Parameters[1].ParameterType.FullName != "System.Single" || m.Parameters[2].ParameterType.FullName != "UnityEngine.Space")
            throw new InvalidDataException("Rotate signature drift");
    }
    if (!target.Body.Instructions.Any(i => i.OpCode == OpCodes.Ldc_R4 && i.Operand is float f && BitConverter.SingleToInt32Bits(f) == BitConverter.SingleToInt32Bits(-60f)))
        throw new InvalidDataException("-60f native speed missing");
    if (!target.Body.Instructions.Any(i => i.OpCode == OpCodes.Ldc_R4 && i.Operand is float f && BitConverter.SingleToInt32Bits(f) == BitConverter.SingleToInt32Bits(-75f)))
        throw new InvalidDataException("-75f native speed missing");

    var getForward = module.ImportReference(getForwardDef);
    if (getForward.ReturnType.FullName != "UnityEngine.Vector3" || getForward.Parameters.Count != 0 || getForward.HasThis)
        throw new InvalidDataException("Vector3.get_forward imported signature drift");

    Console.WriteLine($"NATIVE_AUTHORITY target_token=0x{TargetToken:X8} rid={ExpectedRid} method_pointer_entry=0x181B86060 va=0x1803A57D0 end=0x1803A5934 native_slice_sha256={NativeSliceSha}");
    Console.WriteLine("NATIVE_SEMANTICS roll_axis_forward=1 roll_speed_neg60_dt=1 skillAuto_axis_forward=1 skillAuto_speed_neg75_dt=1 Space_Self=1 remaining_branch_flow_preserved=1");
    Console.WriteLine("RECOVERY_STRATEGY replace_only_two_corrupt_Object_axis_loads_with_exact_Vector3_get_forward=1 control_flow_changes=0 field_metadata_changes=0 null_guard_changes=0");

    foreach (var ins in badAxisLoads)
    {
        ins.OpCode = OpCodes.Call;
        ins.Operand = getForward;
    }

    Directory.CreateDirectory(Path.GetDirectoryName(output)!);
    module.Write(output);
}

Console.WriteLine($"OUTPUT_SHA256 {Sha(output)}");

var resolver = new DefaultAssemblyResolver();
resolver.AddSearchDirectory(Path.GetDirectoryName(coreModulePath)!);
resolver.AddSearchDirectory(Path.GetDirectoryName(output)!);
using (var module = ModuleDefinition.ReadModule(output, new ReaderParameters { InMemory=true, ReadingMode=ReadingMode.Immediate, AssemblyResolver=resolver }))
{
    if (module.AssemblyReferences.Any(a => a.Name == "System.Private.CoreLib")) throw new InvalidDataException("System.Private.CoreLib reference pollution");
    var types = AllTypes(module.Types).ToList();
    var methods = types.SelectMany(t => t.Methods).ToList();
    var fields = types.SelectMany(t => t.Fields).ToList();
    var target = methods.Single(m => Raw(m) == TargetToken);

    var badAxisRefs = target.Body.Instructions.Count(i => (i.OpCode == OpCodes.Ldloca || i.OpCode == OpCodes.Ldloca_S) && i.Operand is VariableDefinition v && v.Index == 13);
    var forwardCalls = target.Body.Instructions.Where(i => i.OpCode == OpCodes.Call && i.Operand is MethodReference m && m.DeclaringType.FullName == "UnityEngine.Vector3" && m.Name == "get_forward").ToList();
    if (badAxisRefs != 0 || forwardCalls.Count != 2) throw new InvalidDataException("reopen typed Vector3 axis replacement mismatch");
    foreach (var i in forwardCalls)
    {
        var mr = (MethodReference)i.Operand;
        var resolved = mr.Resolve() ?? throw new InvalidDataException("Vector3.get_forward MemberRef did not resolve");
        if (resolved.Name != "get_forward" || resolved.DeclaringType.FullName != "UnityEngine.Vector3" || resolved.ReturnType.FullName != "UnityEngine.Vector3")
            throw new InvalidDataException("Vector3.get_forward resolved to wrong MethodDef");
        if (string.IsNullOrEmpty(resolved.Module.FileName) || Sha(resolved.Module.FileName) != ExpectedCoreModuleSha)
            throw new InvalidDataException("Vector3.get_forward resolved against wrong CoreModule");
    }

    var changedM = methods.Where(m => beforeM[Raw(m)] != MethodSig(m)).Select(m => Raw(m)).OrderBy(x => x).ToList();
    if (changedM.Count != 1 || changedM[0] != TargetToken)
        throw new InvalidDataException("semantic method isolation failed: " + string.Join(',', changedM.Select(x => $"0x{x:X8}")));
    var changedF = fields.Where(f => beforeF[Raw(f)] != FieldSig(f)).Select(f => Raw(f)).ToList();
    if (changedF.Count != 0) throw new InvalidDataException("field metadata drift");

    Console.WriteLine($"PLANT_DETAIL_UPDATE_VECTOR3_RESOLUTION_PASS target_token=0x{TargetToken:X8} corrupt_object_axis_refs=0 Vector3_get_forward_calls=2 coremodule_sha256={ExpectedCoreModuleSha}");
    Console.WriteLine($"SEMANTIC_ISOLATION_PASS untouched_methods={ExpectedMethods-1} changed_methods=1 target=0x{TargetToken:X8}");
    Console.WriteLine($"FIELD_METADATA_ISOLATION_PASS unchanged_fields={ExpectedFields} changed_fields=0");
}

return 0;
