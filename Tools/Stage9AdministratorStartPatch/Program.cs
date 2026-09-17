using Mono.Cecil;
using Mono.Cecil.Cil;
using System.Globalization;
using System.Security.Cryptography;
using System.Text;

if (args.Length != 3)
{
    Console.Error.WriteLine("Usage: Stage9AdministratorStartPatch <input.dll> <output.dll> <expected-input-sha256>");
    return 2;
}

const uint TargetToken = 0x06000004;
const int ExpectedRid = 4;
const int ExpectedMethods = 2317;
const int ExpectedFields = 2802;
const int ExpectedOldCodeSize = 635;
const int ExpectedOldLocals = 30;
const int ExpectedOldInstructions = 168;
const string ExpectedPreimageSha = "b6aa91789131594a0b1ac89a83197f026261ea6e9867df34cc2b8b15b46b8089";
const string NativeSha = "3d83a514d37c18b7d91df7ceba17803bb0ac3cbd670dab15e823af4be8b58d02";

const uint MainSystemFieldToken = 0x04000002;
const uint SystemsFieldToken = 0x04000003;
const uint System0FieldToken = 0x04000004;
const uint System1FieldToken = 0x04000005;
const uint System2FieldToken = 0x04000006;
const uint System3FieldToken = 0x04000007;
const uint BoardEdiorFieldToken = 0x04000008;
const uint SkipLevelFieldToken = 0x04000009;
const uint ModeFieldToken = 0x0400000A;
const uint GLawnAppFieldToken = 0x0400015E;
const uint LawnAppSavesManagerFieldToken = 0x04000166;
const uint SavesManagerPlayerSaveFieldToken = 0x04000310;

const uint StartPlantDataToken = 0x06000007;
const uint StartZombiePickerDataToken = 0x06000008;
const uint StartBoardConfigToken = 0x06000009;
const uint StartSuppliesDataToken = 0x0600000A;
const uint System0LoadPlantDataToken = 0x06000073;
const uint System1LoadZombieInfoToken = 0x06000081;
const uint System2Start2Token = 0x06000088;
const uint System3LoadSuppliesInfoToken = 0x0600009E;
const uint SystemSkipLevelStart0Token = 0x060000A4;
const uint BoardManagerGetInstanceToken = 0x06000169;
const uint BoardManagerBoardOnPlayToken = 0x0600016F;
const uint BoardGamePauseToken = 0x060002C1;
const uint MainUIGetInstanceToken = 0x06000636;
const uint MainUISetGloveButtonsToken = 0x06000647;

const uint TransformGetChildRefToken = 0x0A000001;
const uint CameraGetMainRefToken = 0x0A000003;
const uint CanvasSetWorldCameraRefToken = 0x0A000004;
const uint UnityObjectImplicitRefToken = 0x0A000005;
const uint UnityObjectInequalityRefToken = 0x0A000006;
const uint ComponentGetTransformRefToken = 0x0A000008;
const uint ComponentGetGameObjectRefToken = 0x0A000015;
const uint GameObjectSetActiveRefToken = 0x0A000016;
const uint CameraSetOrthographicSizeRefToken = 0x0A000025;
const uint ListGetEnumeratorRefToken = 0x0A0000FB;
const uint EnumeratorGetCurrentRefToken = 0x0A0000FC;
const uint EnumeratorMoveNextRefToken = 0x0A0000FD;
const uint EnumeratorDisposeRefToken = 0x0A000126;
const uint ListGetItemRefToken = 0x0A0001EC;
const uint GetComponentCanvasMethodSpecToken = 0x2B000001;

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
static string HashText(string s) => Convert.ToHexString(SHA256.HashData(Encoding.UTF8.GetBytes(s))).ToLowerInvariant();
static string Scope(IMetadataScope? s) => s switch
{
    AssemblyNameReference a => a.Name,
    ModuleDefinition m => m.Assembly?.Name?.Name ?? m.Name,
    ModuleReference mr => mr.Name,
    _ => s?.ToString() ?? "<null>"
};
static string TypeSig(TypeReference t) => $"{t.FullName}@{Scope(t.Scope)}";
static string MethodRefSig(MethodReference m) => $"{m.FullName}@{Scope(m.DeclaringType.Scope)}#0x{Raw(m):X8}";
static string OperandForPreimage(object? o) => o switch
{
    null => "",
    Instruction i => $"IL_{i.Offset:X4}",
    Instruction[] a => string.Join(',', a.Select(i => $"IL_{i.Offset:X4}")),
    VariableDefinition v => $"V_{v.Index}:{v.VariableType.FullName}",
    ParameterDefinition p => $"P_{p.Index}:{p.ParameterType.FullName}",
    MethodReference m => $"{m.FullName}@{Scope(m.DeclaringType.Scope)}#0x{Raw(m):X8}",
    FieldReference f => $"{f.FullName}@{Scope(f.DeclaringType.Scope)}#0x{Raw(f):X8}",
    TypeReference t => $"{t.FullName}@{Scope(t.Scope)}#0x{Raw(t):X8}",
    _ => o.ToString() ?? ""
};
static string PreimageMethodSig(MethodDefinition m)
{
    var sb = new StringBuilder();
    sb.Append(m.FullName).Append('|').Append(m.Attributes).Append('|').Append(m.ImplAttributes).Append('|');
    if (!m.HasBody) return sb.Append("NOBODY").ToString();
    sb.Append("code=").Append(m.Body.CodeSize).Append(";init=").Append(m.Body.InitLocals).Append(";max=").Append(m.Body.MaxStackSize).Append(';');
    foreach (var v in m.Body.Variables) sb.Append("L:").Append(v.Index).Append(':').Append(v.VariableType.FullName).Append(';');
    foreach (var i in m.Body.Instructions) sb.Append("I:").Append(i.Offset.ToString("X4")).Append(':').Append(i.OpCode.Code).Append(':').Append(OperandForPreimage(i.Operand)).Append(';');
    foreach (var h in m.Body.ExceptionHandlers)
        sb.Append("EH:").Append(h.HandlerType).Append(':').Append(h.TryStart?.Offset.ToString("X4") ?? "").Append(':').Append(h.TryEnd?.Offset.ToString("X4") ?? "").Append(':')
          .Append(h.HandlerStart?.Offset.ToString("X4") ?? "").Append(':').Append(h.HandlerEnd?.Offset.ToString("X4") ?? "").Append(':').Append(h.FilterStart?.Offset.ToString("X4") ?? "").Append(':')
          .Append(h.CatchType?.FullName ?? "").Append(';');
    return sb.ToString();
}
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
static string InstructionSig(Instruction i, MethodDefinition owner) => $"{i.OpCode.Code}:{OpSig(i.Operand, owner)}";
static string MethodBodySig(MethodDefinition m)
{
    if (!m.HasBody) return "NOBODY";
    var sb = new StringBuilder();
    sb.Append("init=").Append(m.Body.InitLocals).Append(";max=").Append(m.Body.MaxStackSize).Append(';');
    foreach (var v in m.Body.Variables) sb.Append("L:").Append(v.Index).Append(':').Append(TypeSig(v.VariableType)).Append(';');
    foreach (var i in m.Body.Instructions) sb.Append("I:").Append(InstructionSig(i, m)).Append(';');
    foreach (var h in m.Body.ExceptionHandlers)
    {
        int Idx(Instruction? x) => x is null ? -1 : m.Body.Instructions.IndexOf(x);
        sb.Append("EH:").Append(h.HandlerType).Append(':').Append(Idx(h.TryStart)).Append(':').Append(Idx(h.TryEnd)).Append(':')
          .Append(Idx(h.HandlerStart)).Append(':').Append(Idx(h.HandlerEnd)).Append(':').Append(Idx(h.FilterStart)).Append(':')
          .Append(h.CatchType is null ? "" : TypeSig(h.CatchType)).Append(';');
    }
    return sb.ToString();
}
static string MethodHeaderSig(MethodDefinition m)
{
    var ca = string.Join(',', m.CustomAttributes.Select(a => a.AttributeType.FullName).OrderBy(x => x, StringComparer.Ordinal));
    return $"{m.FullName}|{m.Attributes}|{m.ImplAttributes}|generic={m.GenericParameters.Count}|ca={ca}";
}
static string FieldSig(FieldDefinition f)
{
    var attrs = string.Join(',', f.CustomAttributes.Select(a => a.AttributeType.FullName).OrderBy(x => x, StringComparer.Ordinal));
    var c = f.HasConstant ? Convert.ToString(f.Constant, CultureInfo.InvariantCulture) ?? "<null>" : "<none>";
    return $"{f.FullName}|{f.Attributes}|{TypeSig(f.FieldType)}|const={c}|ca={attrs}";
}
static T Lookup<T>(ModuleDefinition module, uint token, string label) where T : class, IMetadataTokenProvider
{
    var p = module.LookupToken(new MetadataToken(token));
    if (p is not T t) throw new InvalidDataException($"{label} token 0x{token:X8} resolved to {p?.GetType().Name ?? "null"}");
    return t;
}
static void RequireMethod(MethodReference m, uint token, string declaringType, string name, string returnType, params string[] parameters)
{
    if (Raw(m) != token || m.DeclaringType.FullName != declaringType || m.Name != name || m.ReturnType.FullName != returnType || m.Parameters.Count != parameters.Length ||
        !m.Parameters.Select(p => p.ParameterType.FullName).SequenceEqual(parameters))
        throw new InvalidDataException($"method dependency drift token=0x{token:X8} actual={m.FullName}");
}
static void RequireField(FieldReference f, uint token, string declaringType, string name, string fieldType)
{
    if (Raw(f) != token || f.DeclaringType.FullName != declaringType || f.Name != name || f.FieldType.FullName != fieldType)
        throw new InvalidDataException($"field dependency drift token=0x{token:X8} actual={f.FullName}");
}
static string AssemblyRefs(ModuleDefinition m) => string.Join("\n", m.AssemblyReferences.Select(a => a.FullName).OrderBy(x => x, StringComparer.Ordinal));
static string MemberRefs(ModuleDefinition m) => string.Join("\n", m.GetMemberReferences().Select(r => $"{r.FullName}|{Scope(r.DeclaringType.Scope)}").OrderBy(x => x, StringComparer.Ordinal));
static string TypeRefs(ModuleDefinition m) => string.Join("\n", m.GetTypeReferences().Select(r => $"{r.FullName}|{Scope(r.Scope)}").OrderBy(x => x, StringComparer.Ordinal));

var input = Path.GetFullPath(args[0]);
var output = Path.GetFullPath(args[1]);
var expectedInputSha = args[2].Trim().ToLowerInvariant();
if (expectedInputSha.Length != 64 || expectedInputSha.Any(c => !Uri.IsHexDigit(c))) throw new InvalidDataException("expected input SHA invalid");
var inputSha = Sha(input);
if (inputSha != expectedInputSha) throw new InvalidDataException($"input SHA mismatch actual={inputSha} expected={expectedInputSha}");
if (inputSha != "26064265fdba4e9b501f0c3312260503fdde43b9e111072c19dd39891e5ca022") throw new InvalidDataException("wrong Administrator.Start recovery baseline");
Console.WriteLine($"INPUT_SHA256 {inputSha}");

var beforeBody = new Dictionary<uint, string>();
var beforeHeader = new Dictionary<uint, string>();
var beforeFields = new Dictionary<uint, string>();
string beforeAssemblyRefs;
string beforeMemberRefs;
string beforeTypeRefs;

using (var module = ModuleDefinition.ReadModule(input, new ReaderParameters { InMemory = true, ReadingMode = ReadingMode.Immediate }))
{
    var types = AllTypes(module.Types).ToList();
    var methods = types.SelectMany(t => t.Methods).ToList();
    var fields = types.SelectMany(t => t.Fields).ToList();
    if (methods.Count != ExpectedMethods || fields.Count != ExpectedFields) throw new InvalidDataException($"metadata count drift methods={methods.Count} fields={fields.Count}");
    if (module.AssemblyReferences.Any(a => a.Name == "System.Private.CoreLib")) throw new InvalidDataException("input references System.Private.CoreLib");
    foreach (var m in methods) { beforeBody[Raw(m)] = MethodBodySig(m); beforeHeader[Raw(m)] = MethodHeaderSig(m); }
    foreach (var f in fields) beforeFields[Raw(f)] = FieldSig(f);
    beforeAssemblyRefs = AssemblyRefs(module);
    beforeMemberRefs = MemberRefs(module);
    beforeTypeRefs = TypeRefs(module);

    var administrator = types.Single(t => t.FullName == "Administrator");
    var target = methods.Single(m => Raw(m) == TargetToken);
    if (target.DeclaringType != administrator || target.Name != "Start" || target.IsStatic || target.Parameters.Count != 0 || target.ReturnType.FullName != "System.Void" || !target.HasBody)
        throw new InvalidDataException("target identity drift");
    var preimage = HashText(PreimageMethodSig(target));
    if (target.MetadataToken.RID != ExpectedRid || target.Body.CodeSize != ExpectedOldCodeSize || target.Body.Variables.Count != ExpectedOldLocals || target.Body.ExceptionHandlers.Count != 0 || !target.Body.InitLocals || target.Body.Instructions.Count != ExpectedOldInstructions || preimage != ExpectedPreimageSha)
        throw new InvalidDataException($"target preimage drift rid={target.MetadataToken.RID} size={target.Body.CodeSize} locals={target.Body.Variables.Count} handlers={target.Body.ExceptionHandlers.Count} init={target.Body.InitLocals} instructions={target.Body.Instructions.Count} sha={preimage}");

    FieldReference F(uint tok, string dt, string name, string ft)
    {
        var f = Lookup<FieldReference>(module, tok, name); RequireField(f, tok, dt, name, ft); return f;
    }
    MethodReference M(uint tok, string dt, string name, string rt, params string[] ps)
    {
        var m = Lookup<MethodReference>(module, tok, name); RequireMethod(m, tok, dt, name, rt, ps); return m;
    }

    var mainSystem = F(MainSystemFieldToken, "Administrator", "mainSystem", "UnityEngine.GameObject");
    var systems = F(SystemsFieldToken, "Administrator", "systems", "System.Collections.Generic.List`1<UnityEngine.GameObject>");
    var system0 = F(System0FieldToken, "Administrator", "system0", "System0");
    var system1 = F(System1FieldToken, "Administrator", "system1", "System1");
    var system2 = F(System2FieldToken, "Administrator", "system2", "System2");
    var system3 = F(System3FieldToken, "Administrator", "system3", "System3");
    var boardEdior = F(BoardEdiorFieldToken, "Administrator", "boardEdior", "Admin_system2_boardEdior");
    var skipLevel = F(SkipLevelFieldToken, "Administrator", "skipLevel", "SystemSkipLevel");
    var mode = F(ModeFieldToken, "Administrator", "mode", "System.Int32");
    var gLawnApp = F(GLawnAppFieldToken, "GlobalStaticVars", "gLawnApp", "GlobalStaticVars/LawnApp");
    var savesManager = F(LawnAppSavesManagerFieldToken, "GlobalStaticVars/LawnApp", "savesManager", "SavesManager");
    var playerSave = F(SavesManagerPlayerSaveFieldToken, "SavesManager", "playerSave", "Save");

    var getTransform = M(ComponentGetTransformRefToken, "UnityEngine.Component", "get_transform", "UnityEngine.Transform");
    var getChild = M(TransformGetChildRefToken, "UnityEngine.Transform", "GetChild", "UnityEngine.Transform", "System.Int32");
    var getCanvas = Lookup<MethodReference>(module, GetComponentCanvasMethodSpecToken, "Component.GetComponent<Canvas>");
    if (Raw(getCanvas) != GetComponentCanvasMethodSpecToken || getCanvas.DeclaringType.FullName != "UnityEngine.Component" || getCanvas.Name != "GetComponent" || getCanvas.Parameters.Count != 0 ||
        getCanvas is not GenericInstanceMethod getCanvasGeneric || getCanvasGeneric.GenericArguments.Count != 1 || getCanvasGeneric.GenericArguments[0].FullName != "UnityEngine.Canvas" || getCanvas.ReturnType.FullName != "!!0")
        throw new InvalidDataException($"GetComponent<Canvas> MethodSpec drift actual={getCanvas.FullName}");
    var getCameraMain = M(CameraGetMainRefToken, "UnityEngine.Camera", "get_main", "UnityEngine.Camera");
    var setWorldCamera = M(CanvasSetWorldCameraRefToken, "UnityEngine.Canvas", "set_worldCamera", "System.Void", "UnityEngine.Camera");
    var setActive = M(GameObjectSetActiveRefToken, "UnityEngine.GameObject", "SetActive", "System.Void", "System.Boolean");
    var getGameObject = M(ComponentGetGameObjectRefToken, "UnityEngine.Component", "get_gameObject", "UnityEngine.GameObject");
    var setOrthographicSize = M(CameraSetOrthographicSizeRefToken, "UnityEngine.Camera", "set_orthographicSize", "System.Void", "System.Single");
    var unityImplicit = M(UnityObjectImplicitRefToken, "UnityEngine.Object", "op_Implicit", "System.Boolean", "UnityEngine.Object");
    var unityInequality = M(UnityObjectInequalityRefToken, "UnityEngine.Object", "op_Inequality", "System.Boolean", "UnityEngine.Object", "UnityEngine.Object");
    var getEnumerator = M(ListGetEnumeratorRefToken, "System.Collections.Generic.List`1<UnityEngine.GameObject>", "GetEnumerator", "System.Collections.Generic.List`1/Enumerator<UnityEngine.GameObject>");
    var getCurrent = M(EnumeratorGetCurrentRefToken, "System.Collections.Generic.List`1/Enumerator<UnityEngine.GameObject>", "get_Current", "UnityEngine.GameObject");
    var moveNext = M(EnumeratorMoveNextRefToken, "System.Collections.Generic.List`1/Enumerator<UnityEngine.GameObject>", "MoveNext", "System.Boolean");
    var dispose = M(EnumeratorDisposeRefToken, "System.Collections.Generic.List`1/Enumerator<UnityEngine.GameObject>", "Dispose", "System.Void");
    var getItem = M(ListGetItemRefToken, "System.Collections.Generic.List`1<UnityEngine.GameObject>", "get_Item", "!0", "System.Int32");

    var startPlantData = M(StartPlantDataToken, "Administrator", "Start_植物存档数据", "System.Void");
    var startZombiePickerData = M(StartZombiePickerDataToken, "Administrator", "Start_出怪挑选器数据", "System.Void");
    var startBoardConfig = M(StartBoardConfigToken, "Administrator", "Start_关卡配置文件", "System.Void", "Board");
    var startSuppliesData = M(StartSuppliesDataToken, "Administrator", "Start_材料基础数据", "System.Void");
    var loadPlantData = M(System0LoadPlantDataToken, "System0", "LoadPlantData", "System.Void", "System.Int32");
    var loadZombieInfo = M(System1LoadZombieInfoToken, "System1", "LoadZombieInfo", "System.Void", "System.Int32");
    var start2 = M(System2Start2Token, "System2", "Start2", "System.Void");
    var loadSuppliesInfo = M(System3LoadSuppliesInfoToken, "System3", "LoadSuppliesInfo", "System.Void", "System.Int32");
    var start0 = M(SystemSkipLevelStart0Token, "SystemSkipLevel", "Start0", "System.Void", "Save");
    var boardManagerGetInstance = M(BoardManagerGetInstanceToken, "BoardManager", "get_Instance", "BoardManager");
    var boardOnPlay = M(BoardManagerBoardOnPlayToken, "BoardManager", "Board_OnPlay", "Board");
    var gamePause = M(BoardGamePauseToken, "Board", "GamePause", "System.Void", "System.Boolean");
    var mainUIGetInstance = M(MainUIGetInstanceToken, "MainUIController", "get_instance", "MainUIController");
    var setGloveButtons = M(MainUISetGloveButtonsToken, "MainUIController", "SetGloveButtons", "System.Void");

    Console.WriteLine($"ADMINISTRATOR_START_PREIMAGE_PASS sha256={preimage} code_size={target.Body.CodeSize} locals={target.Body.Variables.Count} instructions={target.Body.Instructions.Count} handlers={target.Body.ExceptionHandlers.Count}");
    Console.WriteLine($"NATIVE_AUTHORITY target_token=0x{TargetToken:X8} rid={ExpectedRid} method_pointer_entry=0x181B82D78 va=0x1802FDD20 end=0x1802FE980 native_slice_sha256={NativeSha}");
    Console.WriteLine("NATIVE_DISPATCH mode_plus_one_unsigned_le_5=1 invalid_mode_skips_system_deactivation=1 valid_modes=-1..4");
    Console.WriteLine("NATIVE_SEMANTICS mode_m1=mainSystem_off,boardEdior_off,camera_540,MainUIController.SetGloveButtons_zero_arg mode0=Start0(playerSave) mode1=plant mode2=zombie mode3=board_accessors mode4=LoadSuppliesInfo_before_StartSupplies tail=GamePause_true");

    var body = target.Body;
    body.Instructions.Clear();
    body.ExceptionHandlers.Clear();
    body.Variables.Clear();
    body.InitLocals = true;
    body.MaxStackSize = 4;

    var enumLocal = new VariableDefinition(getEnumerator.ReturnType);
    var modeIndex = new VariableDefinition(module.TypeSystem.Int32);
    body.Variables.Add(enumLocal);
    body.Variables.Add(modeIndex);
    var il = body.GetILProcessor();

    void A(Instruction i) => il.Append(i);
    Instruction Nop() => Instruction.Create(OpCodes.Nop);

    var validStart = Nop();
    var tryStart = Instruction.Create(OpCodes.Br, Nop());
    var loopBody = Nop();
    var loopCheck = Nop();
    var finallyStart = Instruction.Create(OpCodes.Ldloca, enumLocal);
    var afterFinally = Nop();
    var caseMinus1 = Nop();
    var case0 = Nop();
    var case1 = Nop();
    var case2 = Nop();
    var case3 = Nop();
    var case4 = Nop();
    var mode3NullBoard = Nop();
    var mode3BoardReady = Nop();
    var tail = Nop();
    var ret = Instruction.Create(OpCodes.Ret);
    tryStart.Operand = loopCheck;

    A(Instruction.Create(OpCodes.Ldarg_0));
    A(Instruction.Create(OpCodes.Call, getTransform));
    A(Instruction.Create(OpCodes.Ldc_I4_0));
    A(Instruction.Create(OpCodes.Call, getChild));
    A(Instruction.Create(OpCodes.Call, getCanvas));
    A(Instruction.Create(OpCodes.Call, getCameraMain));
    A(Instruction.Create(OpCodes.Call, setWorldCamera));

    A(Instruction.Create(OpCodes.Ldarg_0));
    A(Instruction.Create(OpCodes.Ldfld, mode));
    A(Instruction.Create(OpCodes.Ldc_I4_1));
    A(Instruction.Create(OpCodes.Add));
    A(Instruction.Create(OpCodes.Stloc, modeIndex));
    A(Instruction.Create(OpCodes.Ldloc, modeIndex));
    A(Instruction.Create(OpCodes.Ldc_I4_5));
    A(Instruction.Create(OpCodes.Ble_Un, validStart));
    A(Instruction.Create(OpCodes.Br, tail));

    A(validStart);
    A(Instruction.Create(OpCodes.Ldarg_0));
    A(Instruction.Create(OpCodes.Ldfld, systems));
    A(Instruction.Create(OpCodes.Callvirt, getEnumerator));
    A(Instruction.Create(OpCodes.Stloc, enumLocal));
    A(tryStart);
    A(loopBody);
    A(Instruction.Create(OpCodes.Ldloca, enumLocal));
    A(Instruction.Create(OpCodes.Call, getCurrent));
    A(Instruction.Create(OpCodes.Ldc_I4_0));
    A(Instruction.Create(OpCodes.Call, setActive));
    A(loopCheck);
    A(Instruction.Create(OpCodes.Ldloca, enumLocal));
    A(Instruction.Create(OpCodes.Call, moveNext));
    A(Instruction.Create(OpCodes.Brtrue, loopBody));
    A(Instruction.Create(OpCodes.Leave, afterFinally));
    A(finallyStart);
    A(Instruction.Create(OpCodes.Call, dispose));
    A(Instruction.Create(OpCodes.Endfinally));
    A(afterFinally);

    A(Instruction.Create(OpCodes.Ldloc, modeIndex));
    A(Instruction.Create(OpCodes.Switch, new[] { caseMinus1, case0, case1, case2, case3, case4 }));
    A(Instruction.Create(OpCodes.Br, tail));

    A(caseMinus1);
    A(Instruction.Create(OpCodes.Ldarg_0)); A(Instruction.Create(OpCodes.Ldfld, mainSystem)); A(Instruction.Create(OpCodes.Ldc_I4_0)); A(Instruction.Create(OpCodes.Call, setActive));
    A(Instruction.Create(OpCodes.Ldarg_0)); A(Instruction.Create(OpCodes.Ldfld, boardEdior)); A(Instruction.Create(OpCodes.Call, getGameObject)); A(Instruction.Create(OpCodes.Ldc_I4_0)); A(Instruction.Create(OpCodes.Call, setActive));
    A(Instruction.Create(OpCodes.Call, getCameraMain)); A(Instruction.Create(OpCodes.Ldc_R4, 540.0f)); A(Instruction.Create(OpCodes.Call, setOrthographicSize));
    A(Instruction.Create(OpCodes.Call, mainUIGetInstance)); A(Instruction.Create(OpCodes.Call, unityImplicit)); A(Instruction.Create(OpCodes.Brfalse, tail));
    A(Instruction.Create(OpCodes.Call, mainUIGetInstance)); A(Instruction.Create(OpCodes.Call, setGloveButtons));
    A(Instruction.Create(OpCodes.Br, tail));

    A(case0);
    A(Instruction.Create(OpCodes.Ldarg_0)); A(Instruction.Create(OpCodes.Ldfld, mainSystem)); A(Instruction.Create(OpCodes.Ldc_I4_1)); A(Instruction.Create(OpCodes.Call, setActive));
    A(Instruction.Create(OpCodes.Ldarg_0)); A(Instruction.Create(OpCodes.Ldfld, skipLevel));
    A(Instruction.Create(OpCodes.Ldsfld, gLawnApp)); A(Instruction.Create(OpCodes.Ldfld, savesManager)); A(Instruction.Create(OpCodes.Ldfld, playerSave));
    A(Instruction.Create(OpCodes.Call, start0));
    A(Instruction.Create(OpCodes.Br, tail));

    A(case1);
    A(Instruction.Create(OpCodes.Ldarg_0)); A(Instruction.Create(OpCodes.Ldfld, systems)); A(Instruction.Create(OpCodes.Ldc_I4_0)); A(Instruction.Create(OpCodes.Call, getItem)); A(Instruction.Create(OpCodes.Ldc_I4_1)); A(Instruction.Create(OpCodes.Call, setActive));
    A(Instruction.Create(OpCodes.Ldarg_0)); A(Instruction.Create(OpCodes.Call, startPlantData));
    A(Instruction.Create(OpCodes.Ldarg_0)); A(Instruction.Create(OpCodes.Ldfld, system0)); A(Instruction.Create(OpCodes.Ldc_I4_M1)); A(Instruction.Create(OpCodes.Call, loadPlantData));
    A(Instruction.Create(OpCodes.Br, tail));

    A(case2);
    A(Instruction.Create(OpCodes.Ldarg_0)); A(Instruction.Create(OpCodes.Ldfld, systems)); A(Instruction.Create(OpCodes.Ldc_I4_1)); A(Instruction.Create(OpCodes.Call, getItem)); A(Instruction.Create(OpCodes.Ldc_I4_1)); A(Instruction.Create(OpCodes.Call, setActive));
    A(Instruction.Create(OpCodes.Ldarg_0)); A(Instruction.Create(OpCodes.Call, startZombiePickerData));
    A(Instruction.Create(OpCodes.Ldarg_0)); A(Instruction.Create(OpCodes.Ldfld, system1)); A(Instruction.Create(OpCodes.Ldc_I4_M1)); A(Instruction.Create(OpCodes.Call, loadZombieInfo));
    A(Instruction.Create(OpCodes.Br, tail));

    A(case3);
    A(Instruction.Create(OpCodes.Ldarg_0)); A(Instruction.Create(OpCodes.Ldfld, systems)); A(Instruction.Create(OpCodes.Ldc_I4_2)); A(Instruction.Create(OpCodes.Call, getItem)); A(Instruction.Create(OpCodes.Ldc_I4_1)); A(Instruction.Create(OpCodes.Call, setActive));
    A(Instruction.Create(OpCodes.Call, boardManagerGetInstance)); A(Instruction.Create(OpCodes.Call, unityImplicit)); A(Instruction.Create(OpCodes.Brfalse, mode3NullBoard));
    A(Instruction.Create(OpCodes.Call, boardManagerGetInstance)); A(Instruction.Create(OpCodes.Call, boardOnPlay)); A(Instruction.Create(OpCodes.Br, mode3BoardReady));
    A(mode3NullBoard); A(Instruction.Create(OpCodes.Ldnull));
    A(mode3BoardReady);
    var boardTemp = new VariableDefinition(startBoardConfig.Parameters[0].ParameterType);
    body.Variables.Add(boardTemp);
    A(Instruction.Create(OpCodes.Stloc, boardTemp));
    A(Instruction.Create(OpCodes.Ldarg_0)); A(Instruction.Create(OpCodes.Ldloc, boardTemp)); A(Instruction.Create(OpCodes.Call, startBoardConfig));
    A(Instruction.Create(OpCodes.Ldarg_0)); A(Instruction.Create(OpCodes.Ldfld, system2)); A(Instruction.Create(OpCodes.Call, start2));
    A(Instruction.Create(OpCodes.Br, tail));

    A(case4);
    A(Instruction.Create(OpCodes.Ldarg_0)); A(Instruction.Create(OpCodes.Ldfld, systems)); A(Instruction.Create(OpCodes.Ldc_I4_3)); A(Instruction.Create(OpCodes.Call, getItem)); A(Instruction.Create(OpCodes.Ldc_I4_1)); A(Instruction.Create(OpCodes.Call, setActive));
    A(Instruction.Create(OpCodes.Ldarg_0)); A(Instruction.Create(OpCodes.Ldfld, system3)); A(Instruction.Create(OpCodes.Ldc_I4_M1)); A(Instruction.Create(OpCodes.Call, loadSuppliesInfo));
    A(Instruction.Create(OpCodes.Ldarg_0)); A(Instruction.Create(OpCodes.Call, startSuppliesData));
    A(Instruction.Create(OpCodes.Br, tail));

    A(tail);
    A(Instruction.Create(OpCodes.Ldarg_0)); A(Instruction.Create(OpCodes.Ldfld, mode)); A(Instruction.Create(OpCodes.Ldc_I4_M1)); A(Instruction.Create(OpCodes.Beq, ret));
    A(Instruction.Create(OpCodes.Call, boardManagerGetInstance)); A(Instruction.Create(OpCodes.Call, unityImplicit)); A(Instruction.Create(OpCodes.Brfalse, ret));
    A(Instruction.Create(OpCodes.Call, boardManagerGetInstance)); A(Instruction.Create(OpCodes.Call, boardOnPlay)); A(Instruction.Create(OpCodes.Ldnull)); A(Instruction.Create(OpCodes.Call, unityInequality)); A(Instruction.Create(OpCodes.Brfalse, ret));
    A(Instruction.Create(OpCodes.Call, boardManagerGetInstance)); A(Instruction.Create(OpCodes.Call, boardOnPlay)); A(Instruction.Create(OpCodes.Ldc_I4_1)); A(Instruction.Create(OpCodes.Call, gamePause));
    A(ret);

    body.ExceptionHandlers.Add(new ExceptionHandler(ExceptionHandlerType.Finally)
    {
        TryStart = tryStart,
        TryEnd = finallyStart,
        HandlerStart = finallyStart,
        HandlerEnd = afterFinally
    });

    Console.WriteLine($"RECOVERY_STRATEGY full_method_native_reconstruction=1 valid_range_gate=1 foreach_enumerator_try_finally=1 switch_cases=6 board_private_field_access=0 SetGloveButtons_args={setGloveButtons.Parameters.Count} locals={body.Variables.Count} handlers={body.ExceptionHandlers.Count}");
    Directory.CreateDirectory(Path.GetDirectoryName(output)!);
    module.Write(output);
}

Console.WriteLine($"OUTPUT_SHA256 {Sha(output)}");
using (var module = ModuleDefinition.ReadModule(output, new ReaderParameters { InMemory = true, ReadingMode = ReadingMode.Immediate }))
{
    var types = AllTypes(module.Types).ToList();
    var methods = types.SelectMany(t => t.Methods).ToList();
    var fields = types.SelectMany(t => t.Fields).ToList();
    if (methods.Count != ExpectedMethods || fields.Count != ExpectedFields) throw new InvalidDataException("metadata count drift after reopen");
    if (AssemblyRefs(module) != beforeAssemblyRefs) throw new InvalidDataException("assembly reference set changed");
    if (MemberRefs(module) != beforeMemberRefs) throw new InvalidDataException("MemberRef metadata set changed");
    if (TypeRefs(module) != beforeTypeRefs) throw new InvalidDataException("TypeRef metadata set changed");
    if (module.AssemblyReferences.Any(a => a.Name == "System.Private.CoreLib")) throw new InvalidDataException("System.Private.CoreLib pollution");

    var changedHeaders = methods.Where(m => beforeHeader[Raw(m)] != MethodHeaderSig(m)).Select(Raw).OrderBy(x => x).ToList();
    if (changedHeaders.Count != 0) throw new InvalidDataException("method signature/accessibility drift: " + string.Join(',', changedHeaders.Select(x => $"0x{x:X8}")));
    var changedBodies = methods.Where(m => beforeBody[Raw(m)] != MethodBodySig(m)).Select(Raw).OrderBy(x => x).ToList();
    if (changedBodies.Count != 1 || changedBodies[0] != TargetToken) throw new InvalidDataException("body isolation failed: " + string.Join(',', changedBodies.Select(x => $"0x{x:X8}")));
    var changedFields = fields.Where(f => beforeFields[Raw(f)] != FieldSig(f)).Select(Raw).OrderBy(x => x).ToList();
    if (changedFields.Count != 0) throw new InvalidDataException("field metadata isolation failed: " + string.Join(',', changedFields.Select(x => $"0x{x:X8}")));

    var target = methods.Single(m => Raw(m) == TargetToken);
    if (!target.HasBody || !target.Body.InitLocals || target.Body.ExceptionHandlers.Count != 1 || target.Body.ExceptionHandlers[0].HandlerType != ExceptionHandlerType.Finally)
        throw new InvalidDataException("target structural recovery did not survive reopen");
    if (target.Body.Instructions.Any(i => i.Operand is FieldReference f && Raw(f) == 0x040001B4)) throw new InvalidDataException("private BoardManager.board access introduced");
    if (target.Body.Instructions.Any(i => i.Operand is FieldReference f && f.DeclaringType.FullName.StartsWith("System.Collections.Generic.List`1", StringComparison.Ordinal) && f.Name is "_items" or "_size" or "_version"))
        throw new InvalidDataException("List<T> private internals introduced");
    if (target.Body.Instructions.Any(i => i.Operand is MemberReference r && r.FullName.Contains("UnityEngine.Vector3::zeroVector", StringComparison.Ordinal))) throw new InvalidDataException("zeroVector scaffolding introduced");
    if (target.Body.Instructions.Any(i => i.OpCode.Code is Code.Calli or Code.Localloc or Code.Cpblk or Code.Initblk or Code.Ldind_I or Code.Ldind_I1 or Code.Ldind_I2 or Code.Ldind_I4 or Code.Ldind_I8 or Code.Ldind_R4 or Code.Ldind_R8 or Code.Ldind_Ref or Code.Ldind_U1 or Code.Ldind_U2 or Code.Ldind_U4 or Code.Stind_I or Code.Stind_I1 or Code.Stind_I2 or Code.Stind_I4 or Code.Stind_I8 or Code.Stind_R4 or Code.Stind_R8 or Code.Stind_Ref)) throw new InvalidDataException("unmanaged scaffolding introduced");

    int Calls(uint tok) => target.Body.Instructions.Count(i => i.Operand is MethodReference m && Raw(m) == tok);
    int Fields(uint tok) => target.Body.Instructions.Count(i => i.Operand is FieldReference f && Raw(f) == tok);
    if (Calls(MainUISetGloveButtonsToken) != 1 || Lookup<MethodReference>(module, MainUISetGloveButtonsToken, "SetGloveButtons").Parameters.Count != 0)
        throw new InvalidDataException("zero-arg SetGloveButtons gate failed");
    if (Calls(BoardManagerGetInstanceToken) != 5 || Calls(BoardManagerBoardOnPlayToken) != 3 || Calls(BoardGamePauseToken) != 1)
        throw new InvalidDataException($"Board accessor tail gate failed getInstance={Calls(BoardManagerGetInstanceToken)} Board_OnPlay={Calls(BoardManagerBoardOnPlayToken)} GamePause={Calls(BoardGamePauseToken)}");
    if (Calls(System3LoadSuppliesInfoToken) != 1 || Calls(StartSuppliesDataToken) != 1) throw new InvalidDataException("mode4 calls missing");
    var mode4LoadIndex = target.Body.Instructions.Select((i, n) => (i, n)).Where(x => x.i.Operand is MethodReference m && Raw(m) == System3LoadSuppliesInfoToken).Select(x => x.n).SingleOrDefault(-1);
    var mode4StartIndex = target.Body.Instructions.Select((i, n) => (i, n)).Where(x => x.i.Operand is MethodReference m && Raw(m) == StartSuppliesDataToken).Select(x => x.n).SingleOrDefault(-1);
    if (mode4LoadIndex < 0 || mode4StartIndex < 0 || mode4LoadIndex >= mode4StartIndex) throw new InvalidDataException($"mode4 native call order drift load={mode4LoadIndex} start={mode4StartIndex}");
    if (Fields(GLawnAppFieldToken) != 1 || Fields(LawnAppSavesManagerFieldToken) != 1 || Fields(SavesManagerPlayerSaveFieldToken) != 1 || Calls(SystemSkipLevelStart0Token) != 1)
        throw new InvalidDataException("mode0 save chain gate failed");
    int CallsExternal(string declaringType, string name) => target.Body.Instructions.Count(i => i.Operand is MethodReference m && m.DeclaringType.FullName == declaringType && m.Name == name);
    if (CallsExternal("System.Collections.Generic.List`1<UnityEngine.GameObject>", "GetEnumerator") != 1 || CallsExternal("System.Collections.Generic.List`1/Enumerator<UnityEngine.GameObject>", "get_Current") != 1 || CallsExternal("System.Collections.Generic.List`1/Enumerator<UnityEngine.GameObject>", "MoveNext") != 1 || CallsExternal("System.Collections.Generic.List`1/Enumerator<UnityEngine.GameObject>", "Dispose") != 1 || CallsExternal("System.Collections.Generic.List`1<UnityEngine.GameObject>", "get_Item") != 4)
        throw new InvalidDataException("List<GameObject> API semantic gate failed");
    var getEnumeratorInstruction = target.Body.Instructions.Single(i => i.Operand is MethodReference m && m.DeclaringType.FullName == "System.Collections.Generic.List`1<UnityEngine.GameObject>" && m.Name == "GetEnumerator");
    if (getEnumeratorInstruction.OpCode.Code != Code.Callvirt) throw new InvalidDataException($"List<GameObject>.GetEnumerator opcode drift actual={getEnumeratorInstruction.OpCode.Code}");
    Console.WriteLine("LIST_GAMEOBJECT_FOREACH_GATE_PASS getenumerator_opcode=callvirt getcurrent_call=1 movenext_call=1 dispose_call=1");

    Console.WriteLine($"REOPEN_ADMINISTRATOR_START_PASS token=0x{TargetToken:X8} code_size={target.Body.CodeSize} locals={target.Body.Variables.Count} instructions={target.Body.Instructions.Count} handlers={target.Body.ExceptionHandlers.Count} initlocals={target.Body.InitLocals}");
    Console.WriteLine($"SEMANTIC_ISOLATION_PASS unchanged_method_headers={ExpectedMethods} changed_method_bodies=1 target=0x{TargetToken:X8} unchanged_fields={ExpectedFields}");
    Console.WriteLine("METADATA_REFERENCE_GATE_PASS assembly_refs_unchanged=1 member_ref_semantics_unchanged=1 type_ref_semantics_unchanged=1 system_private_corelib_refs=0");
    Console.WriteLine("ADMINISTRATOR_START_STRUCTURE_GATE_PASS valid_range_gate=1 foreach_try_finally=1 SetGloveButtons_zero_arg=1 mode0_save_chain=1 mode3_private_board_field=0 mode4_order=load_then_start tail_gamepause=1");
}
return 0;
