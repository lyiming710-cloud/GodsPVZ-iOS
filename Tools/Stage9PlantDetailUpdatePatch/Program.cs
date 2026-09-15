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
static IEnumerable<MethodReference> MethodRefs(IEnumerable<MethodDefinition> methods) => methods
    .Where(m => m.HasBody).SelectMany(m => m.Body.Instructions).Select(i => i.Operand).OfType<MethodReference>();
static MethodReference FindRef(IEnumerable<MethodDefinition> methods, Func<MethodReference,bool> pred, string label)
{
    var hits = MethodRefs(methods).Where(pred)
        .GroupBy(m => m.FullName + "@" + Scope(m.DeclaringType.Scope), StringComparer.Ordinal)
        .Select(g => g.First()).ToList();
    if (hits.Count != 1) throw new InvalidDataException($"{label} refs={hits.Count}: {string.Join(" | ", hits.Select(h => h.FullName))}");
    return hits[0];
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

var exactResolver = new DefaultAssemblyResolver();
exactResolver.AddSearchDirectory(Path.GetDirectoryName(coreModulePath)!);
using var exactCore = ModuleDefinition.ReadModule(coreModulePath, new ReaderParameters
{
    InMemory = true,
    ReadingMode = ReadingMode.Immediate,
    AssemblyResolver = exactResolver
});
var exactVector3 = exactCore.GetType("UnityEngine.Vector3") ?? throw new InvalidDataException("UnityEngine.Vector3 missing from exact CoreModule");
var getForwardDef = exactVector3.Methods.Single(m => m.Name == "get_forward" && m.IsStatic && m.Parameters.Count == 0 && m.ReturnType.FullName == "UnityEngine.Vector3");

var beforeM = new Dictionary<uint,string>();
var beforeF = new Dictionary<uint,string>();
string beforeRefs;

using (var module = ModuleDefinition.ReadModule(input, new ReaderParameters { InMemory=true, ReadingMode=ReadingMode.Immediate }))
{
    if (module.AssemblyReferences.Any(a => a.Name == "System.Private.CoreLib")) throw new InvalidDataException("input references System.Private.CoreLib");
    var types = AllTypes(module.Types).ToList();
    var methods = types.SelectMany(t => t.Methods).ToList();
    var fields = types.SelectMany(t => t.Fields).ToList();
    if (methods.Count != ExpectedMethods || fields.Count != ExpectedFields) throw new InvalidDataException("metadata count drift");
    foreach (var m in methods) beforeM[Raw(m)] = MethodSig(m);
    foreach (var f in fields) beforeF[Raw(f)] = FieldSig(f);
    beforeRefs = string.Join("\n", module.AssemblyReferences.Select(a => a.FullName).OrderBy(x => x, StringComparer.Ordinal));

    var t = types.Single(x => x.FullName == "Plant_DetaiPage");
    var target = t.Methods.Single(m => m.Name == "Update" && m.Parameters.Count == 0);
    if (Raw(target) != TargetToken || target.MetadataToken.RID != ExpectedRid || target.Body.CodeSize != ExpectedOldCodeSize || target.Body.Variables.Count != ExpectedOldLocals)
        throw new InvalidDataException("Plant_DetaiPage.Update fingerprint drift");
    if (target.Body.Variables.Count(v => v.VariableType.FullName == "System.Object") != 1 || target.Body.Variables[13].VariableType.FullName != "System.Object")
        throw new InvalidDataException("expected corrupt System.Object local 13 missing");
    if (!target.Body.Instructions.Any(i => i.Offset == 0x3E && i.OpCode == OpCodes.Call)) throw new InvalidDataException("first Rotate damage marker missing");
    if (!target.Body.Instructions.Any(i => i.Offset == 0x96 && i.OpCode == OpCodes.Ceq)) throw new InvalidDataException("p_skill reference/int ceq damage marker missing");

    FieldDefinition F(string n) => t.Fields.Single(f => f.Name == n);
    var roll = F("roll");
    var skillAuto = F("skillAuto");
    var pSkill = F("p_skill");
    var plant = F("plant");
    var skillLock = F("skillLock");
    var plantT = types.Single(x => x.FullName == "Plant");
    var plantID = plantT.Fields.Single(f => f.Name == "ID");
    var plantState = plantT.Fields.Single(f => f.Name == "state");

    var getTransform = FindRef(methods, m => m.DeclaringType.FullName == "UnityEngine.Component" && m.Name == "get_transform" && m.Parameters.Count == 0, "Component.get_transform");
    var deltaTime = FindRef(methods, m => m.DeclaringType.FullName == "UnityEngine.Time" && m.Name == "get_deltaTime" && m.Parameters.Count == 0, "Time.get_deltaTime");
    var rotate = MethodRefs(methods).Where(m => m.DeclaringType.FullName == "UnityEngine.Transform" && m.Name == "Rotate" && m.Parameters.Count == 3 && m.Parameters[0].ParameterType.FullName == "UnityEngine.Vector3" && m.Parameters[1].ParameterType.FullName == "System.Single" && m.Parameters[2].ParameterType.FullName == "UnityEngine.Space").GroupBy(m => m.FullName).Select(g => g.First()).Single();
    var getGameObject = FindRef(methods, m => m.DeclaringType.FullName == "UnityEngine.Component" && m.Name == "get_gameObject" && m.Parameters.Count == 0, "Component.get_gameObject");
    var setActive = FindRef(methods, m => m.DeclaringType.FullName == "UnityEngine.GameObject" && m.Name == "SetActive" && m.Parameters.Count == 1 && m.Parameters[0].ParameterType.FullName == "System.Boolean", "GameObject.SetActive");
    var updateSkillProgress = t.Methods.Single(m => m.Name == "Update_SkillProgress" && m.Parameters.Count == 0);
    var updateCamera = t.Methods.Single(m => m.Name == "Updata_Camera" && m.Parameters.Count == 0);
    var getForward = module.ImportReference(getForwardDef);

    Console.WriteLine($"NATIVE_AUTHORITY target_token=0x{TargetToken:X8} rid={ExpectedRid} method_pointer_entry=0x181B86060 va=0x1803A57D0 end=0x1803A5934 native_slice_sha256={NativeSliceSha}");
    Console.WriteLine("NATIVE_SEMANTICS roll_forward_neg60dt=1 skillAuto_forward_neg75dt=1 p_skill_null_gate=1 skillLock_ID4_state_nonzero=1 Updata_Camera_tail=1");
    Console.WriteLine("RECOVERY_STRATEGY full_single_MethodDef_rebuild=1 exact_Vector3_forward=1 remove_reference_int_ceq=1 field_metadata_changes=0 null_guard_changes=0 exception_swallowing=0");

    var body = target.Body;
    body.Instructions.Clear();
    body.Variables.Clear();
    body.ExceptionHandlers.Clear();
    body.InitLocals = true;
    body.MaxStackSize = 4;
    var active = new VariableDefinition(module.TypeSystem.Boolean);
    body.Variables.Add(active);
    var il = body.GetILProcessor();

    void EmitRotate(FieldReference image, float speed)
    {
        il.Append(il.Create(OpCodes.Ldarg_0));
        il.Append(il.Create(OpCodes.Ldfld, image));
        il.Append(il.Create(OpCodes.Call, getTransform));
        il.Append(il.Create(OpCodes.Call, getForward));
        il.Append(il.Create(OpCodes.Call, deltaTime));
        il.Append(il.Create(OpCodes.Ldc_R4, speed));
        il.Append(il.Create(OpCodes.Mul));
        il.Append(il.Create(OpCodes.Ldc_I4_0));
        il.Append(il.Create(OpCodes.Call, rotate));
    }

    EmitRotate(roll, -60f);
    EmitRotate(skillAuto, -75f);

    var cameraTail = il.Create(OpCodes.Ldarg_0);
    var activeFalse = il.Create(OpCodes.Ldc_I4_0);
    var activeStore = il.Create(OpCodes.Stloc, active);

    il.Append(il.Create(OpCodes.Ldarg_0));
    il.Append(il.Create(OpCodes.Ldfld, pSkill));
    il.Append(il.Create(OpCodes.Brfalse, cameraTail));

    il.Append(il.Create(OpCodes.Ldarg_0));
    il.Append(il.Create(OpCodes.Call, updateSkillProgress));

    il.Append(il.Create(OpCodes.Ldarg_0));
    il.Append(il.Create(OpCodes.Ldfld, plant));
    il.Append(il.Create(OpCodes.Ldfld, plantID));
    il.Append(il.Create(OpCodes.Ldc_I4_4));
    il.Append(il.Create(OpCodes.Bne_Un, activeFalse));
    il.Append(il.Create(OpCodes.Ldarg_0));
    il.Append(il.Create(OpCodes.Ldfld, plant));
    il.Append(il.Create(OpCodes.Ldfld, plantState));
    il.Append(il.Create(OpCodes.Brfalse, activeFalse));
    il.Append(il.Create(OpCodes.Ldc_I4_1));
    il.Append(il.Create(OpCodes.Br, activeStore));
    il.Append(activeFalse);
    il.Append(activeStore);

    il.Append(il.Create(OpCodes.Ldarg_0));
    il.Append(il.Create(OpCodes.Ldfld, skillLock));
    il.Append(il.Create(OpCodes.Call, getGameObject));
    il.Append(il.Create(OpCodes.Ldloc, active));
    il.Append(il.Create(OpCodes.Call, setActive));

    il.Append(cameraTail);
    il.Append(il.Create(OpCodes.Call, updateCamera));
    il.Append(il.Create(OpCodes.Ret));

    Directory.CreateDirectory(Path.GetDirectoryName(output)!);
    module.Write(output);
}

Console.WriteLine($"OUTPUT_SHA256 {Sha(output)}");

var resolver = new DefaultAssemblyResolver();
resolver.AddSearchDirectory(Path.GetDirectoryName(coreModulePath)!);
resolver.AddSearchDirectory(Path.GetDirectoryName(output)!);
using (var module = ModuleDefinition.ReadModule(output, new ReaderParameters { InMemory=true, ReadingMode=ReadingMode.Immediate, AssemblyResolver=resolver }))
{
    var types = AllTypes(module.Types).ToList();
    var methods = types.SelectMany(t => t.Methods).ToList();
    var fields = types.SelectMany(t => t.Fields).ToList();
    var target = methods.Single(m => Raw(m) == TargetToken);
    var afterRefs = string.Join("\n", module.AssemblyReferences.Select(a => a.FullName).OrderBy(x => x, StringComparer.Ordinal));
    if (afterRefs != beforeRefs) throw new InvalidDataException("assembly reference set changed");
    if (module.AssemblyReferences.Any(a => a.Name == "System.Private.CoreLib")) throw new InvalidDataException("System.Private.CoreLib reference pollution");

    int Calls(string type, string name) => target.Body.Instructions.Count(i =>
        (i.OpCode == OpCodes.Call || i.OpCode == OpCodes.Callvirt || i.OpCode == OpCodes.Newobj) && i.Operand is MethodReference m && m.DeclaringType.FullName == type && m.Name == name);
    var objectLocals = target.Body.Variables.Count(v => v.VariableType.FullName == "System.Object");
    var ceq = target.Body.Instructions.Count(i => i.OpCode == OpCodes.Ceq);
    if (objectLocals != 0 || ceq != 0 || Calls("UnityEngine.Vector3","get_forward") != 2 || Calls("UnityEngine.Transform","Rotate") != 2 || Calls("UnityEngine.Time","get_deltaTime") != 2 || Calls("Plant_DetaiPage","Update_SkillProgress") != 1 || Calls("UnityEngine.GameObject","SetActive") != 1 || Calls("Plant_DetaiPage","Updata_Camera") != 1)
        throw new InvalidDataException("reopen semantic count mismatch");

    foreach (var ins in target.Body.Instructions.Where(i => i.OpCode == OpCodes.Call && i.Operand is MethodReference m && m.DeclaringType.FullName == "UnityEngine.Vector3" && m.Name == "get_forward"))
    {
        var resolved = ((MethodReference)ins.Operand).Resolve() ?? throw new InvalidDataException("Vector3.get_forward did not resolve");
        if (resolved.DeclaringType.FullName != "UnityEngine.Vector3" || resolved.ReturnType.FullName != "UnityEngine.Vector3") throw new InvalidDataException("Vector3.get_forward resolved signature drift");
        if (string.IsNullOrEmpty(resolved.Module.FileName) || Sha(resolved.Module.FileName) != ExpectedCoreModuleSha) throw new InvalidDataException("Vector3.get_forward resolved against wrong CoreModule");
    }

    var changedM = methods.Where(m => beforeM[Raw(m)] != MethodSig(m)).Select(m => Raw(m)).OrderBy(x => x).ToList();
    if (changedM.Count != 1 || changedM[0] != TargetToken) throw new InvalidDataException("semantic method isolation failed: " + string.Join(',', changedM.Select(x => $"0x{x:X8}")));
    var changedF = fields.Where(f => beforeF[Raw(f)] != FieldSig(f)).Select(f => Raw(f)).ToList();
    if (changedF.Count != 0) throw new InvalidDataException("field metadata drift");

    Console.WriteLine($"REOPEN_PLANT_DETAIL_UPDATE_PASS token=0x{TargetToken:X8} code_size={target.Body.CodeSize} locals={target.Body.Variables.Count} object_locals=0 ceq=0 Vector3_forward=2 Rotate=2 deltaTime=2 Update_SkillProgress=1 SetActive=1 Updata_Camera=1");
    Console.WriteLine($"PLANT_DETAIL_UPDATE_VECTOR3_RESOLUTION_PASS target_token=0x{TargetToken:X8} Vector3_get_forward_calls=2 coremodule_sha256={ExpectedCoreModuleSha}");
    Console.WriteLine($"SEMANTIC_ISOLATION_PASS untouched_methods={ExpectedMethods-1} changed_methods=1 target=0x{TargetToken:X8}");
    Console.WriteLine($"FIELD_METADATA_ISOLATION_PASS unchanged_fields={ExpectedFields} changed_fields=0");
    Console.WriteLine("FRAMEWORK_REFERENCE_GATE_PASS system_private_corelib_refs=0 assembly_reference_set_unchanged=1");
}

return 0;
