using Mono.Cecil;
using Mono.Cecil.Cil;
using System.Security.Cryptography;
using System.Text;

if (args.Length != 2)
{
    Console.Error.WriteLine("Usage: GodsPVZ.Stage9SetScreenPatch <input.dll> <output.dll>");
    return 2;
}

const string ExpectedInputSha256 = "eee0e8f1f919c9038138c10a3d642c69116a768f5ec7651ed2c396fb7df88842";
const int ExpectedMethodDefCount = 2317;
const uint ExpectedToken = 0x06000149;

static IEnumerable<TypeDefinition> AllTypes(IEnumerable<TypeDefinition> roots)
{
    foreach (var t in roots)
    {
        yield return t;
        foreach (var n in AllTypes(t.NestedTypes)) yield return n;
    }
}
static uint Raw(IMetadataTokenProvider p) => p.MetadataToken.ToUInt32();
static string OperandSemantic(object? operand, MethodDefinition owner)
{
    if (operand is null) return "";
    if (operand is Instruction bi) return $"I#{owner.Body.Instructions.IndexOf(bi)}";
    if (operand is Instruction[] sw) return "SW[" + string.Join(',', sw.Select(x => owner.Body.Instructions.IndexOf(x))) + "]";
    if (operand is MethodReference mr) return $"M:{mr.FullName}";
    if (operand is FieldReference fr) return $"F:{fr.FullName}";
    if (operand is TypeReference tr) return $"T:{tr.FullName}";
    if (operand is VariableDefinition vr) return $"V:{vr.Index}:{vr.VariableType.FullName}";
    if (operand is ParameterDefinition pr) return $"P:{pr.Index}:{pr.ParameterType.FullName}";
    if (operand is string s) return $"S:{Convert.ToBase64String(Encoding.UTF8.GetBytes(s))}";
    return $"C:{Convert.ToString(operand, System.Globalization.CultureInfo.InvariantCulture)}";
}
static string MethodSemantic(MethodDefinition m)
{
    var sb = new StringBuilder();
    sb.Append(m.FullName).Append('|').Append(m.Attributes).Append('|').Append(m.ImplAttributes).Append('|');
    if (!m.HasBody) return sb.Append("NOBODY").ToString();
    sb.Append("init=").Append(m.Body.InitLocals).Append(";max=").Append(m.Body.MaxStackSize).Append(';');
    foreach (var v in m.Body.Variables) sb.Append("L:").Append(v.Index).Append(':').Append(v.VariableType.FullName).Append(';');
    foreach (var i in m.Body.Instructions) sb.Append("I:").Append(i.OpCode.Code).Append(':').Append(OperandSemantic(i.Operand, m)).Append(';');
    return sb.ToString();
}
static Instruction At(MethodDefinition m, int offset)
    => m.Body.Instructions.SingleOrDefault(i => i.Offset == offset)
       ?? throw new InvalidDataException($"{m.FullName}: missing IL_{offset:X4}");
static void RequireLocal(MethodDefinition m, int offset, Code code, int local, string label)
{
    var i = At(m, offset);
    if (i.OpCode.Code != code || i.Operand is not VariableDefinition v || v.Index != local)
        throw new InvalidDataException($"{label}: expected {code} V{local} at IL_{offset:X4}, got {i.OpCode.Code} {i.Operand}");
}

var input = Path.GetFullPath(args[0]);
var output = Path.GetFullPath(args[1]);
var inputSha = Convert.ToHexString(SHA256.HashData(File.ReadAllBytes(input))).ToLowerInvariant();
Console.WriteLine($"INPUT_SHA256 {inputSha}");
if (inputSha != ExpectedInputSha256) throw new InvalidDataException($"input SHA mismatch: {inputSha}");

using var module = ModuleDefinition.ReadModule(input, new ReaderParameters { InMemory = true, ReadingMode = ReadingMode.Immediate });
var methods = AllTypes(module.Types).SelectMany(t => t.Methods).ToList();
if (methods.Count != ExpectedMethodDefCount) throw new InvalidDataException($"MethodDef count drifted: {methods.Count}");
var target = methods.SingleOrDefault(m => m.DeclaringType.FullName == "GlobalStaticVars" && m.Name == "SetScreen" && m.Parameters.Count == 1 && m.Parameters[0].ParameterType.FullName == "System.Boolean" && m.ReturnType.FullName == "System.Void")
    ?? throw new InvalidDataException("GlobalStaticVars.SetScreen(bool) target missing or ambiguous");
var token = Raw(target);
Console.WriteLine($"TARGET_METHOD token=0x{token:X8} name={target.FullName}");
if (token != ExpectedToken) throw new InvalidDataException($"target token drifted: 0x{token:X8}");
if (target.Body.Variables.Count != 27) throw new InvalidDataException($"SetScreen local count drift: {target.Body.Variables.Count}");
if (target.Body.Variables[11].VariableType.FullName != "System.Object") throw new InvalidDataException("expected corrupt V11 object local");
RequireLocal(target, 0x00B0, Code.Stloc, 11, "runtime-invalid resolutionScale-1 store");
var oldPlaceholder = At(target, 0x025D);
if (oldPlaceholder.OpCode.Code != Code.Ldstr || (string?)oldPlaceholder.Operand != "Method not found @180D0E340")
    throw new InvalidDataException("expected unresolved Resolution.get_refreshRateRatio placeholder missing");

var operands = target.Body.Instructions.Select(i => i.Operand).ToList();
var gLawnApp = operands.OfType<FieldReference>().FirstOrDefault(f => f.FullName == "GlobalStaticVars/LawnApp GlobalStaticVars::gLawnApp")
    ?? throw new InvalidDataException("gLawnApp FieldRef missing");
var resolutionScaleField = operands.OfType<FieldReference>().FirstOrDefault(f => f.FullName == "System.Int32 GlobalStaticVars/LawnApp::resolutionScale")
    ?? throw new InvalidDataException("LawnApp.resolutionScale FieldRef missing");
var fullscreenField = operands.OfType<FieldReference>().FirstOrDefault(f => f.FullName == "System.Boolean GlobalStaticVars/LawnApp::fullscreen")
    ?? throw new InvalidDataException("LawnApp.fullscreen FieldRef missing");
var screenSet3 = operands.OfType<MethodReference>().FirstOrDefault(m => m.Name == "SetResolution" && m.DeclaringType.FullName == "UnityEngine.Screen" && m.Parameters.Count == 3)
    ?? throw new InvalidDataException("Screen.SetResolution 3-arg MethodRef missing");
var screenSet4 = operands.OfType<MethodReference>().FirstOrDefault(m => m.Name == "SetResolution" && m.DeclaringType.FullName == "UnityEngine.Screen" && m.Parameters.Count == 4)
    ?? throw new InvalidDataException("Screen.SetResolution 4-arg MethodRef missing");
var screenCurrent = operands.OfType<MethodReference>().FirstOrDefault(m => m.Name == "get_currentResolution" && m.DeclaringType.FullName == "UnityEngine.Screen" && m.Parameters.Count == 0)
    ?? throw new InvalidDataException("Screen.get_currentResolution MethodRef missing");
var resolutionType = screenCurrent.ReturnType;
var refreshRateType = screenSet4.Parameters[3].ParameterType;
var getRefreshRateRatio = module.ImportReference(new MethodReference("get_refreshRateRatio", refreshRateType, resolutionType)
{
    HasThis = true,
    ExplicitThis = false,
    CallingConvention = MethodCallingConvention.Default
});
Console.WriteLine($"ENGINE_CALL {getRefreshRateRatio.FullName}");

var changed = new HashSet<uint> { token };
var untouchedBefore = methods.Where(m => !changed.Contains(Raw(m))).ToDictionary(Raw, MethodSemantic);

var body = target.Body;
body.Instructions.Clear();
body.ExceptionHandlers.Clear();
body.Variables.Clear();
body.InitLocals = true;
body.MaxStackSize = 4;
var scale = new VariableDefinition(module.TypeSystem.Int32);
var width = new VariableDefinition(module.TypeSystem.Int32);
var height = new VariableDefinition(module.TypeSystem.Int32);
var currentResolution = new VariableDefinition(resolutionType);
body.Variables.Add(scale);
body.Variables.Add(width);
body.Variables.Add(height);
body.Variables.Add(currentResolution);
var il = body.GetILProcessor();

var afterInitialStore = il.Create(OpCodes.Ldarg_0);
var fullscreenPath = il.Create(OpCodes.Call, screenCurrent);
var windowedPath = il.Create(OpCodes.Ldc_I4, 1440);
var ret = il.Create(OpCodes.Ret);
var case0 = il.Create(OpCodes.Ldc_I4, 1280);
var case1 = il.Create(OpCodes.Ldc_I4, 1920);
var case2 = il.Create(OpCodes.Ldc_I4, 2160);
var case3 = il.Create(OpCodes.Ldc_I4, 2560);
var case4 = il.Create(OpCodes.Ldc_I4, 2960);
var fallback = il.Create(OpCodes.Ldc_I4, 1280);
var applyWindowed = il.Create(OpCodes.Ldloc, width);

// if (gLawnApp != null) gLawnApp.fullscreen = fullscreen;
il.Append(il.Create(OpCodes.Ldsfld, gLawnApp));
il.Append(il.Create(OpCodes.Brfalse, afterInitialStore));
il.Append(il.Create(OpCodes.Ldsfld, gLawnApp));
il.Append(il.Create(OpCodes.Ldarg_0));
il.Append(il.Create(OpCodes.Stfld, fullscreenField));
il.Append(afterInitialStore);
il.Append(il.Create(OpCodes.Brtrue, fullscreenPath));
il.Append(il.Create(OpCodes.Br, windowedPath));

// Windowed preflight proven by PC native: 1440x810, FullScreenMode numeric value 3.
il.Append(windowedPath);
il.Append(il.Create(OpCodes.Ldc_I4, 810));
il.Append(il.Create(OpCodes.Ldc_I4_3));
il.Append(il.Create(OpCodes.Call, screenSet3));

// Preserve the native reload/copy sequence around resolutionScale.
il.Append(il.Create(OpCodes.Ldsfld, gLawnApp));
il.Append(il.Create(OpCodes.Ldfld, resolutionScaleField));
il.Append(il.Create(OpCodes.Stloc, scale));
il.Append(il.Create(OpCodes.Ldsfld, gLawnApp));
il.Append(il.Create(OpCodes.Ldloc, scale));
il.Append(il.Create(OpCodes.Stfld, resolutionScaleField));
il.Append(il.Create(OpCodes.Ldsfld, gLawnApp));
il.Append(il.Create(OpCodes.Ldfld, fullscreenField));
il.Append(il.Create(OpCodes.Brtrue, ret));

il.Append(il.Create(OpCodes.Ldloc, scale));
il.Append(il.Create(OpCodes.Switch, new[] { case0, case1, case2, case3, case4 }));
il.Append(il.Create(OpCodes.Br, fallback));

void AddCase(Instruction label, int h)
{
    il.Append(label);
    il.Append(il.Create(OpCodes.Stloc, width));
    il.Append(il.Create(OpCodes.Ldc_I4, h));
    il.Append(il.Create(OpCodes.Stloc, height));
    il.Append(il.Create(OpCodes.Br, applyWindowed));
}
AddCase(case0, 720);
AddCase(case1, 1080);
AddCase(case2, 1080);
AddCase(case3, 1440);
AddCase(case4, 1440);
AddCase(fallback, 720);

il.Append(applyWindowed);
il.Append(il.Create(OpCodes.Ldloc, height));
il.Append(il.Create(OpCodes.Ldc_I4_3));
il.Append(il.Create(OpCodes.Call, screenSet3));
il.Append(il.Create(OpCodes.Br, ret));

// Fullscreen PC native path: currentResolution.refreshRateRatio and 2560x1440, mode numeric value 1.
il.Append(fullscreenPath);
il.Append(il.Create(OpCodes.Stloc, currentResolution));
il.Append(il.Create(OpCodes.Ldc_I4, 2560));
il.Append(il.Create(OpCodes.Ldc_I4, 1440));
il.Append(il.Create(OpCodes.Ldc_I4_1));
il.Append(il.Create(OpCodes.Ldloca, currentResolution));
il.Append(il.Create(OpCodes.Call, getRefreshRateRatio));
il.Append(il.Create(OpCodes.Call, screenSet4));
il.Append(ret);

Console.WriteLine("NATIVE_AUTHORITY token=0x06000149 address=0x000000018031C080 next=0x000000018031C2B0 gameassembly_sha256=9ebd7ca996a5b03fb4a766f7a2502b660d581ddbbb36af4d7bf2f06a211da39d metadata_sha256=ad992341add498bd1018ac980f42171e0737b0793bf945c8714ad70a445b36b9");
Console.WriteLine("NATIVE_ENGINE_CALL address=0x0000000180D0E340 module=UnityEngine.CoreModule.dll token=0x0600034F method=UnityEngine.Resolution::get_refreshRateRatio/0");
Console.WriteLine("PATCH_SETSCREEN windowed_preflight=1440x810:mode3 table=0:1280x720,1:1920x1080,2:2160x1080,3:2560x1440,4:2960x1440,default:1280x720 fullscreen=2560x1440:mode1 refresh=currentResolution.refreshRateRatio");

module.Write(output);
var outputSha = Convert.ToHexString(SHA256.HashData(File.ReadAllBytes(output))).ToLowerInvariant();
Console.WriteLine($"OUTPUT_SHA256 {outputSha}");

using var reopen = ModuleDefinition.ReadModule(output, new ReaderParameters { InMemory = true, ReadingMode = ReadingMode.Immediate });
var after = AllTypes(reopen.Types).SelectMany(t => t.Methods).ToList();
if (after.Count != ExpectedMethodDefCount) throw new InvalidDataException("reopen MethodDef count drift");
var rt = after.SingleOrDefault(m => Raw(m) == token) ?? throw new InvalidDataException("reopen target missing");
if (rt.Body.Variables.Count != 4) throw new InvalidDataException($"SetScreen repaired local count drift: {rt.Body.Variables.Count}");
if (rt.Body.Variables[0].VariableType.FullName != "System.Int32" || rt.Body.Variables[1].VariableType.FullName != "System.Int32" || rt.Body.Variables[2].VariableType.FullName != "System.Int32" || rt.Body.Variables[3].VariableType.FullName != "UnityEngine.Resolution")
    throw new InvalidDataException("SetScreen repaired local types drift");
var calls = rt.Body.Instructions.Where(i => i.OpCode.Code == Code.Call).Select(i => i.Operand).OfType<MethodReference>().ToList();
if (calls.Count(m => m.DeclaringType.FullName == "UnityEngine.Screen" && m.Name == "SetResolution" && m.Parameters.Count == 3) != 2) throw new InvalidDataException("expected two 3-arg Screen.SetResolution calls");
if (calls.Count(m => m.DeclaringType.FullName == "UnityEngine.Screen" && m.Name == "SetResolution" && m.Parameters.Count == 4) != 1) throw new InvalidDataException("expected one 4-arg Screen.SetResolution call");
if (calls.Count(m => m.DeclaringType.FullName == "UnityEngine.Screen" && m.Name == "get_currentResolution") != 1) throw new InvalidDataException("currentResolution getter missing");
if (calls.Count(m => m.DeclaringType.FullName == "UnityEngine.Resolution" && m.Name == "get_refreshRateRatio") != 1) throw new InvalidDataException("refreshRateRatio getter missing");
if (rt.Body.Instructions.Any(i => i.Operand is string s && s.Contains("Method not found", StringComparison.Ordinal))) throw new InvalidDataException("unresolved native placeholder survived SetScreen repair");
var ints = rt.Body.Instructions.Where(i => i.OpCode.Code == Code.Ldc_I4).Select(i => (int)i.Operand).ToList();
foreach (var n in new[] { 1440, 810, 1280, 720, 1920, 1080, 2160, 2560, 2960 }) if (!ints.Contains(n)) throw new InvalidDataException($"reopen missing native SetScreen constant {n}");
Console.WriteLine($"REOPEN_SETSCREEN_PASS locals=4 windowed_calls=2 fullscreen_calls=1 refresh=get_refreshRateRatio token=0x{token:X8}");

var untouchedAfter = after.Where(m => !changed.Contains(Raw(m))).ToDictionary(Raw, MethodSemantic);
if (untouchedAfter.Count != untouchedBefore.Count) throw new InvalidDataException("untouched method count drift");
var drift = untouchedBefore.Where(kv => !untouchedAfter.TryGetValue(kv.Key, out var sem) || sem != kv.Value).Select(kv => kv.Key).ToList();
if (drift.Count != 0) throw new InvalidDataException("semantic drift outside SetScreen: " + string.Join(',', drift.Select(t => $"0x{t:X8}")));
Console.WriteLine($"SEMANTIC_ISOLATION_PASS untouched_methods={untouchedAfter.Count} changed_methods=1");
return 0;
