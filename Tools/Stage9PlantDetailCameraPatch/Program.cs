using Mono.Cecil;
using Mono.Cecil.Cil;
using System.Globalization;
using System.Security.Cryptography;
using System.Text;

if (args.Length != 4)
{
    Console.Error.WriteLine("Usage: Stage9PlantDetailCameraPatch <input.dll> <output.dll> <expected-input-sha256> <UnityEngine.CoreModule.dll>");
    return 2;
}

const string ExpectedCoreModuleSha = "83192116b34abac2bb1797ec0e3943362be33b028b1eefd2e561d5fdc1c3312f";
const uint TargetToken = 0x06000662;
const int ExpectedRid = 1634;
const int ExpectedMethods = 2317;
const int ExpectedFields = 2802;
const int ExpectedOldCodeSize = 1210;
const int ExpectedOldLocals = 57;
const string NativeSpanSha = "1cff925d1002b279b599a7a18bc076ad423f89878bc26b3ec04ed665c2e66ee9";

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

var exactResolver = new DefaultAssemblyResolver();
exactResolver.AddSearchDirectory(Path.GetDirectoryName(coreModulePath)!);
using var exactCore = ModuleDefinition.ReadModule(coreModulePath, new ReaderParameters { InMemory=true, ReadingMode=ReadingMode.Immediate, AssemblyResolver=exactResolver });
var exactVector3 = exactCore.GetType("UnityEngine.Vector3") ?? throw new InvalidDataException("Vector3 missing from exact CoreModule");
var vectorCtorDef = exactVector3.Methods.Single(m => m.IsConstructor && !m.IsStatic && m.Parameters.Count == 3 && m.Parameters.All(p => p.ParameterType.FullName == "System.Single"));
var xDef = exactVector3.Fields.Single(f => f.Name == "x" && f.FieldType.FullName == "System.Single");
var yDef = exactVector3.Fields.Single(f => f.Name == "y" && f.FieldType.FullName == "System.Single");
var zDef = exactVector3.Fields.Single(f => f.Name == "z" && f.FieldType.FullName == "System.Single");

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

    var detailT = types.Single(t => t.FullName == "Plant_DetaiPage");
    var boardT = types.Single(t => t.FullName == "Board");
    var mapT = types.Single(t => t.FullName == "Map");
    var mouseT = types.Single(t => t.FullName == "MouseManager");
    var itemT = types.Single(t => t.FullName == "ItemType");
    var target = detailT.Methods.Single(m => m.Name == "Updata_Camera" && m.Parameters.Count == 0);
    if (Raw(target) != TargetToken || target.MetadataToken.RID != ExpectedRid || target.Body.CodeSize != ExpectedOldCodeSize || target.Body.Variables.Count != ExpectedOldLocals)
        throw new InvalidDataException($"Updata_Camera fingerprint drift token=0x{Raw(target):X8} size={target.Body.CodeSize} locals={target.Body.Variables.Count}");
    if (target.Body.Variables.Count(v => v.VariableType.FullName == "System.Object") < 3)
        throw new InvalidDataException("expected damaged object locals missing");
    if (!target.Body.Instructions.Any(i => i.Operand is string s && s.Contains("non empty stack", StringComparison.Ordinal)))
        throw new InvalidDataException("expected damaged non-empty-stack marker missing");
    var crosshair = itemT.Fields.Single(f => f.Name == "PlantSkillCrosshairs" && f.HasConstant);
    if (Convert.ToInt32(crosshair.Constant, CultureInfo.InvariantCulture) != 4) throw new InvalidDataException("PlantSkillCrosshairs enum value drift");

    FieldDefinition DF(string n) => detailT.Fields.Single(f => f.Name == n);
    FieldDefinition BF(string n) => boardT.Fields.Single(f => f.Name == n);
    FieldDefinition MF(string n) => mapT.Fields.Single(f => f.Name == n);
    FieldDefinition MMF(string n) => mouseT.Fields.Single(f => f.Name == n);
    var skill = DF("skill");
    var board = DF("board");
    var boardMouse = BF("mouseManager");
    var boardMap = BF("map");
    var boardCameraPosition = BF("cameraPosition");
    var handItemType = MMF("handItemType");
    var cameraSize = MF("cameraSize");
    if (boardCameraPosition.FieldType.FullName != "UnityEngine.Vector3" || cameraSize.FieldType.FullName != "System.Single") throw new InvalidDataException("camera field type drift");

    var cameraMain = FindRef(methods, m => m.DeclaringType.FullName == "UnityEngine.Camera" && m.Name == "get_main" && m.Parameters.Count == 0, "Camera.get_main");
    var objectImplicit = FindRef(methods, m => m.DeclaringType.FullName == "UnityEngine.Object" && m.Name == "op_Implicit" && m.Parameters.Count == 1, "Object.op_Implicit");
    var activeSelf = FindRef(methods, m => m.DeclaringType.FullName == "UnityEngine.GameObject" && m.Name == "get_activeSelf" && m.Parameters.Count == 0, "GameObject.get_activeSelf");
    var componentTransform = FindRef(methods, m => m.DeclaringType.FullName == "UnityEngine.Component" && m.Name == "get_transform" && m.Parameters.Count == 0, "Component.get_transform");
    var gameObjectTransform = FindRef(methods, m => m.DeclaringType.FullName == "UnityEngine.GameObject" && m.Name == "get_transform" && m.Parameters.Count == 0, "GameObject.get_transform");
    var getPosition = FindRef(methods, m => m.DeclaringType.FullName == "UnityEngine.Transform" && m.Name == "get_position" && m.Parameters.Count == 0, "Transform.get_position");
    var setPosition = FindRef(methods, m => m.DeclaringType.FullName == "UnityEngine.Transform" && m.Name == "set_position" && m.Parameters.Count == 1 && m.Parameters[0].ParameterType.FullName == "UnityEngine.Vector3", "Transform.set_position");
    var getOrtho = FindRef(methods, m => m.DeclaringType.FullName == "UnityEngine.Camera" && m.Name == "get_orthographicSize" && m.Parameters.Count == 0, "Camera.get_orthographicSize");
    var setOrtho = FindRef(methods, m => m.DeclaringType.FullName == "UnityEngine.Camera" && m.Name == "set_orthographicSize" && m.Parameters.Count == 1 && m.Parameters[0].ParameterType.FullName == "System.Single", "Camera.set_orthographicSize");
    var vectorCtor = module.ImportReference(vectorCtorDef);
    var vx = module.ImportReference(xDef); var vy = module.ImportReference(yDef); var vz = module.ImportReference(zDef);

    Console.WriteLine($"NATIVE_AUTHORITY target_token=0x{TargetToken:X8} rid={ExpectedRid} method_pointer_entry=0x181B86068 va=0x1803A4810 end=0x1803A4BAB native_span_sha256={NativeSpanSha}");
    Console.WriteLine("NATIVE_SEMANTICS Camera_main_calls=2 skill_inactive_board_cameraPosition_x=1 crosshair_scale_1_1=1 skill_active_skill_x_scale_0_3=1 ratio_div_880=1 ordered_clamp_0_1_nan_raw=1 ortho_smooth_0_1=1");
    Console.WriteLine("RECOVERY_STRATEGY full_single_MethodDef_rebuild=1 typed_Vector3=1 exact_Vector3_ctor_fields=1 preserve_Unity_liveness=1 metadata_changes=0 null_guard_changes=0 exception_swallowing=0");

    var body = target.Body;
    body.Instructions.Clear(); body.Variables.Clear(); body.ExceptionHandlers.Clear();
    body.InitLocals = true; body.MaxStackSize = 5;
    var camera = new VariableDefinition(cameraMain.ReturnType);
    var current = new VariableDefinition(module.ImportReference(exactVector3));
    var newPos = new VariableDefinition(module.ImportReference(exactVector3));
    var newX = new VariableDefinition(module.TypeSystem.Single);
    var targetSize = new VariableDefinition(module.TypeSystem.Single);
    var ratio = new VariableDefinition(module.TypeSystem.Single);
    var clamped = new VariableDefinition(module.TypeSystem.Single);
    body.Variables.Add(camera); body.Variables.Add(current); body.Variables.Add(newPos); body.Variables.Add(newX); body.Variables.Add(targetSize); body.Variables.Add(ratio); body.Variables.Add(clamped);
    var il = body.GetILProcessor();

    var ret = il.Create(OpCodes.Ret);
    var skillActive = il.Create(OpCodes.Ldloc, camera);
    var crosshairPath = il.Create(OpCodes.Ldarg_0);
    var sizeSmooth = il.Create(OpCodes.Ldloc, camera);
    var upperCheck = il.Create(OpCodes.Ldloc, ratio);
    var rawRatio = il.Create(OpCodes.Ldloc, ratio);
    var afterClamp = il.Create(OpCodes.Ldc_R4, 1.4f);

    // Camera main = Camera.main; if (!main) return; main = Camera.main;
    il.Append(il.Create(OpCodes.Call, cameraMain));
    il.Append(il.Create(OpCodes.Stloc, camera));
    il.Append(il.Create(OpCodes.Ldloc, camera));
    il.Append(il.Create(OpCodes.Call, objectImplicit));
    il.Append(il.Create(OpCodes.Brfalse, ret));
    il.Append(il.Create(OpCodes.Call, cameraMain));
    il.Append(il.Create(OpCodes.Stloc, camera));

    // skill.activeSelf ? skill-active : board-camera path
    il.Append(il.Create(OpCodes.Ldarg_0));
    il.Append(il.Create(OpCodes.Ldfld, skill));
    il.Append(il.Create(OpCodes.Call, activeSelf));
    il.Append(il.Create(OpCodes.Brtrue, skillActive));

    // Inactive skill: current = camera.transform.position
    il.Append(il.Create(OpCodes.Ldloc, camera));
    il.Append(il.Create(OpCodes.Call, componentTransform));
    il.Append(il.Create(OpCodes.Call, getPosition));
    il.Append(il.Create(OpCodes.Stloc, current));
    // newX = current.x + (board.cameraPosition.x - current.x) * 0.1f
    il.Append(il.Create(OpCodes.Ldloca, current)); il.Append(il.Create(OpCodes.Ldfld, vx));
    il.Append(il.Create(OpCodes.Ldarg_0)); il.Append(il.Create(OpCodes.Ldfld, board)); il.Append(il.Create(OpCodes.Ldflda, boardCameraPosition)); il.Append(il.Create(OpCodes.Ldfld, vx));
    il.Append(il.Create(OpCodes.Ldloca, current)); il.Append(il.Create(OpCodes.Ldfld, vx));
    il.Append(il.Create(OpCodes.Sub)); il.Append(il.Create(OpCodes.Ldc_R4, 0.1f)); il.Append(il.Create(OpCodes.Mul)); il.Append(il.Create(OpCodes.Add)); il.Append(il.Create(OpCodes.Stloc, newX));
    // camera.transform.position = new Vector3(newX,current.y,current.z)
    il.Append(il.Create(OpCodes.Ldloc, newX));
    il.Append(il.Create(OpCodes.Ldloca, current)); il.Append(il.Create(OpCodes.Ldfld, vy));
    il.Append(il.Create(OpCodes.Ldloca, current)); il.Append(il.Create(OpCodes.Ldfld, vz));
    il.Append(il.Create(OpCodes.Newobj, vectorCtor)); il.Append(il.Create(OpCodes.Stloc, newPos));
    il.Append(il.Create(OpCodes.Ldloc, camera)); il.Append(il.Create(OpCodes.Call, componentTransform)); il.Append(il.Create(OpCodes.Ldloc, newPos)); il.Append(il.Create(OpCodes.Call, setPosition));
    // hand item: normal cameraSize or crosshair cameraSize*1.1
    il.Append(il.Create(OpCodes.Ldarg_0)); il.Append(il.Create(OpCodes.Ldfld, board)); il.Append(il.Create(OpCodes.Ldfld, boardMouse)); il.Append(il.Create(OpCodes.Ldfld, handItemType));
    il.Append(il.Create(OpCodes.Ldc_I4_4)); il.Append(il.Create(OpCodes.Beq, crosshairPath));
    il.Append(il.Create(OpCodes.Ldarg_0)); il.Append(il.Create(OpCodes.Ldfld, board)); il.Append(il.Create(OpCodes.Ldfld, boardMap)); il.Append(il.Create(OpCodes.Ldfld, cameraSize)); il.Append(il.Create(OpCodes.Stloc, targetSize));
    il.Append(il.Create(OpCodes.Br, sizeSmooth));
    il.Append(crosshairPath); il.Append(il.Create(OpCodes.Ldfld, board)); il.Append(il.Create(OpCodes.Ldfld, boardMap)); il.Append(il.Create(OpCodes.Ldfld, cameraSize)); il.Append(il.Create(OpCodes.Ldc_R4, 1.1f)); il.Append(il.Create(OpCodes.Mul)); il.Append(il.Create(OpCodes.Stloc, targetSize)); il.Append(il.Create(OpCodes.Br, sizeSmooth));

    // Skill-active path
    il.Append(skillActive); il.Append(il.Create(OpCodes.Call, componentTransform)); il.Append(il.Create(OpCodes.Call, getPosition)); il.Append(il.Create(OpCodes.Stloc, current));
    il.Append(il.Create(OpCodes.Ldloca, current)); il.Append(il.Create(OpCodes.Ldfld, vx));
    il.Append(il.Create(OpCodes.Ldarg_0)); il.Append(il.Create(OpCodes.Ldfld, skill)); il.Append(il.Create(OpCodes.Call, gameObjectTransform)); il.Append(il.Create(OpCodes.Call, getPosition)); il.Append(il.Create(OpCodes.Ldfld, vx)); il.Append(il.Create(OpCodes.Ldc_R4, 0.3f)); il.Append(il.Create(OpCodes.Mul));
    il.Append(il.Create(OpCodes.Ldloca, current)); il.Append(il.Create(OpCodes.Ldfld, vx)); il.Append(il.Create(OpCodes.Sub)); il.Append(il.Create(OpCodes.Ldc_R4, 0.1f)); il.Append(il.Create(OpCodes.Mul)); il.Append(il.Create(OpCodes.Add)); il.Append(il.Create(OpCodes.Stloc, newX));
    il.Append(il.Create(OpCodes.Ldloc, newX)); il.Append(il.Create(OpCodes.Ldloca, current)); il.Append(il.Create(OpCodes.Ldfld, vy)); il.Append(il.Create(OpCodes.Ldloca, current)); il.Append(il.Create(OpCodes.Ldfld, vz)); il.Append(il.Create(OpCodes.Newobj, vectorCtor)); il.Append(il.Create(OpCodes.Stloc, newPos));
    il.Append(il.Create(OpCodes.Ldloc, camera)); il.Append(il.Create(OpCodes.Call, componentTransform)); il.Append(il.Create(OpCodes.Ldloc, newPos)); il.Append(il.Create(OpCodes.Call, setPosition));
    // ratio = cameraSize/880
    il.Append(il.Create(OpCodes.Ldarg_0)); il.Append(il.Create(OpCodes.Ldfld, board)); il.Append(il.Create(OpCodes.Ldfld, boardMap)); il.Append(il.Create(OpCodes.Ldfld, cameraSize)); il.Append(il.Create(OpCodes.Ldc_R4, 880f)); il.Append(il.Create(OpCodes.Div)); il.Append(il.Create(OpCodes.Stloc, ratio));
    // ordered clamp: NaN makes both cgt tests false, therefore raw ratio path.
    il.Append(il.Create(OpCodes.Ldc_R4, 0f)); il.Append(il.Create(OpCodes.Ldloc, ratio)); il.Append(il.Create(OpCodes.Cgt)); il.Append(il.Create(OpCodes.Brfalse, upperCheck));
    il.Append(il.Create(OpCodes.Ldc_R4, 0f)); il.Append(il.Create(OpCodes.Stloc, clamped)); il.Append(il.Create(OpCodes.Br, afterClamp));
    il.Append(upperCheck); il.Append(il.Create(OpCodes.Ldc_R4, 1f)); il.Append(il.Create(OpCodes.Cgt)); il.Append(il.Create(OpCodes.Brfalse, rawRatio));
    il.Append(il.Create(OpCodes.Ldc_R4, 1f)); il.Append(il.Create(OpCodes.Stloc, clamped)); il.Append(il.Create(OpCodes.Br, afterClamp));
    il.Append(rawRatio); il.Append(il.Create(OpCodes.Stloc, clamped));
    // target = (1.4 - clamped*0.4) * cameraSize
    il.Append(afterClamp); il.Append(il.Create(OpCodes.Ldloc, clamped)); il.Append(il.Create(OpCodes.Ldc_R4, 0.4f)); il.Append(il.Create(OpCodes.Mul)); il.Append(il.Create(OpCodes.Sub));
    il.Append(il.Create(OpCodes.Ldarg_0)); il.Append(il.Create(OpCodes.Ldfld, board)); il.Append(il.Create(OpCodes.Ldfld, boardMap)); il.Append(il.Create(OpCodes.Ldfld, cameraSize)); il.Append(il.Create(OpCodes.Mul)); il.Append(il.Create(OpCodes.Stloc, targetSize));

    // orthographicSize += (targetSize - orthographicSize) * 0.1f
    il.Append(sizeSmooth);
    il.Append(il.Create(OpCodes.Ldloc, targetSize)); il.Append(il.Create(OpCodes.Ldloc, camera)); il.Append(il.Create(OpCodes.Call, getOrtho)); il.Append(il.Create(OpCodes.Sub)); il.Append(il.Create(OpCodes.Ldc_R4, 0.1f)); il.Append(il.Create(OpCodes.Mul)); il.Append(il.Create(OpCodes.Ldloc, camera)); il.Append(il.Create(OpCodes.Call, getOrtho)); il.Append(il.Create(OpCodes.Add)); il.Append(il.Create(OpCodes.Call, setOrtho));
    il.Append(ret);

    Directory.CreateDirectory(Path.GetDirectoryName(output)!);
    module.Write(output);
}

Console.WriteLine($"OUTPUT_SHA256 {Sha(output)}");

var resolver = new DefaultAssemblyResolver();
resolver.AddSearchDirectory(Path.GetDirectoryName(coreModulePath)!);
resolver.AddSearchDirectory(Path.GetDirectoryName(output)!);
using (var module = ModuleDefinition.ReadModule(output, new ReaderParameters { InMemory=true, ReadingMode=ReadingMode.Immediate, AssemblyResolver=resolver }))
{
    var types = AllTypes(module.Types).ToList(); var methods = types.SelectMany(t => t.Methods).ToList(); var fields = types.SelectMany(t => t.Fields).ToList();
    var target = methods.Single(m => Raw(m) == TargetToken);
    var afterRefs = string.Join("\n", module.AssemblyReferences.Select(a => a.FullName).OrderBy(x => x, StringComparer.Ordinal));
    if (afterRefs != beforeRefs) throw new InvalidDataException("assembly reference set changed");
    if (module.AssemblyReferences.Any(a => a.Name == "System.Private.CoreLib")) throw new InvalidDataException("System.Private.CoreLib pollution");
    int Calls(string type,string name) => target.Body.Instructions.Count(i => (i.OpCode == OpCodes.Call || i.OpCode == OpCodes.Callvirt || i.OpCode == OpCodes.Newobj) && i.Operand is MethodReference m && m.DeclaringType.FullName == type && m.Name == name);
    if (target.Body.Variables.Count != 7 || target.Body.Variables.Any(v => v.VariableType.FullName == "System.Object")) throw new InvalidDataException("typed local gate failed");
    if (Calls("UnityEngine.Camera","get_main") != 2 || Calls("UnityEngine.Object","op_Implicit") != 1 || Calls("UnityEngine.GameObject","get_activeSelf") != 1 || Calls("UnityEngine.Transform","get_position") != 3 || Calls("UnityEngine.Transform","set_position") != 2 || Calls("UnityEngine.Vector3",".ctor") != 2 || Calls("UnityEngine.Camera","get_orthographicSize") != 2 || Calls("UnityEngine.Camera","set_orthographicSize") != 1)
        throw new InvalidDataException("reopen call-count mismatch");
    if (target.Body.Instructions.Count(i => i.OpCode == OpCodes.Cgt) != 2) throw new InvalidDataException("ordered clamp comparison count mismatch");
    if (target.Body.Instructions.Any(i => i.Operand is string s && s.Contains("non empty stack", StringComparison.Ordinal))) throw new InvalidDataException("damaged warning marker survived");
    var changedM = methods.Where(m => beforeM[Raw(m)] != MethodSig(m)).Select(m => Raw(m)).OrderBy(x => x).ToList();
    if (changedM.Count != 1 || changedM[0] != TargetToken) throw new InvalidDataException("semantic isolation failed: " + string.Join(',', changedM.Select(x => $"0x{x:X8}")));
    var changedF = fields.Where(f => beforeF[Raw(f)] != FieldSig(f)).Select(f => Raw(f)).ToList();
    if (changedF.Count != 0) throw new InvalidDataException("field metadata drift");
    Console.WriteLine($"REOPEN_PLANT_DETAIL_CAMERA_PASS token=0x{TargetToken:X8} code_size={target.Body.CodeSize} locals=7 object_locals=0 Camera_main=2 position_get=3 position_set=2 Vector3_ctor=2 ordered_cgt=2 ortho_get=2 ortho_set=1");
    Console.WriteLine($"SEMANTIC_ISOLATION_PASS untouched_methods={ExpectedMethods-1} changed_methods=1 target=0x{TargetToken:X8}");
    Console.WriteLine($"FIELD_METADATA_ISOLATION_PASS unchanged_fields={ExpectedFields} changed_fields=0");
    Console.WriteLine("FRAMEWORK_REFERENCE_GATE_PASS system_private_corelib_refs=0 assembly_reference_set_unchanged=1");
}

return 0;
