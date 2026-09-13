using Mono.Cecil;
using Mono.Cecil.Cil;
using System.Security.Cryptography;
using System.Text;

if (args.Length != 2)
{
    Console.Error.WriteLine("Usage: GodsPVZ.Stage9SetResolutionPatch <input.dll> <output.dll>");
    return 2;
}

const string ExpectedInputSha256 = "cf4c2ffcde7a9ac20d91b89fe5e315b832e85f73082323c3e0db976604d647b9";
const int ExpectedMethodDefCount = 2317;
const uint ExpectedToken = 0x0600014A;

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
static void Require(MethodDefinition m, int offset, Code code, string label)
{
    var i = At(m, offset);
    if (i.OpCode.Code != code) throw new InvalidDataException($"{label}: expected {code} at IL_{offset:X4}, got {i.OpCode.Code}");
}

var input = Path.GetFullPath(args[0]);
var output = Path.GetFullPath(args[1]);
var inputSha = Convert.ToHexString(SHA256.HashData(File.ReadAllBytes(input))).ToLowerInvariant();
Console.WriteLine($"INPUT_SHA256 {inputSha}");
if (inputSha != ExpectedInputSha256) throw new InvalidDataException($"input SHA mismatch: {inputSha}");

using var module = ModuleDefinition.ReadModule(input, new ReaderParameters { InMemory = true, ReadingMode = ReadingMode.Immediate });
var methods = AllTypes(module.Types).SelectMany(t => t.Methods).ToList();
if (methods.Count != ExpectedMethodDefCount) throw new InvalidDataException($"MethodDef count drifted: {methods.Count}");
var target = methods.SingleOrDefault(m => m.DeclaringType.FullName == "GlobalStaticVars" && m.Name == "SetResolution" && m.Parameters.Count == 1 && m.Parameters[0].ParameterType.FullName == "System.Int32" && m.ReturnType.FullName == "System.Void")
    ?? throw new InvalidDataException("GlobalStaticVars.SetResolution(int) target missing or ambiguous");
var token = Raw(target);
Console.WriteLine($"TARGET_METHOD token=0x{token:X8} name={target.FullName}");
if (token != ExpectedToken) throw new InvalidDataException($"target token drifted: 0x{token:X8}");
if (target.Body.Variables.Count != 18) throw new InvalidDataException($"SetResolution local count drift: {target.Body.Variables.Count}");
if (target.Body.Variables[7].VariableType.FullName != "System.Object") throw new InvalidDataException("expected corrupt V7 object local");
Require(target, 0x0062, Code.Ldarg, "resolutionScale load before V7");
Require(target, 0x006B, Code.Sub, "resolutionScale-1 before V7");
Require(target, 0x006C, Code.Stloc, "invalid V7 store");
Require(target, 0x0249, Code.Call, "Screen.SetResolution call");

var operands = target.Body.Instructions.Select(i => i.Operand).ToList();
var gLawnApp = operands.OfType<FieldReference>().FirstOrDefault(f => f.FullName == "GlobalStaticVars/LawnApp GlobalStaticVars::gLawnApp")
    ?? throw new InvalidDataException("gLawnApp FieldRef missing");
var resolutionScaleField = operands.OfType<FieldReference>().FirstOrDefault(f => f.FullName == "System.Int32 GlobalStaticVars/LawnApp::resolutionScale")
    ?? throw new InvalidDataException("LawnApp.resolutionScale FieldRef missing");
var fullscreenField = operands.OfType<FieldReference>().FirstOrDefault(f => f.FullName == "System.Boolean GlobalStaticVars/LawnApp::fullscreen")
    ?? throw new InvalidDataException("LawnApp.fullscreen FieldRef missing");
var screenSetResolution = operands.OfType<MethodReference>().FirstOrDefault(m => m.Name == "SetResolution" && m.DeclaringType.FullName == "UnityEngine.Screen" && m.Parameters.Count == 3)
    ?? throw new InvalidDataException("UnityEngine.Screen.SetResolution(int,int,FullScreenMode) MethodRef missing");
Console.WriteLine($"SCREEN_CALL {screenSetResolution.FullName}");

var changed = new HashSet<uint> { token };
var untouchedBefore = methods.Where(m => !changed.Contains(Raw(m))).ToDictionary(Raw, MethodSemantic);

var body = target.Body;
body.Instructions.Clear();
body.ExceptionHandlers.Clear();
body.Variables.Clear();
body.InitLocals = true;
body.MaxStackSize = 3;
var width = new VariableDefinition(module.TypeSystem.Int32);
var height = new VariableDefinition(module.TypeSystem.Int32);
body.Variables.Add(width);
body.Variables.Add(height);
var il = body.GetILProcessor();

var ret = il.Create(OpCodes.Ret);
var case0 = il.Create(OpCodes.Ldc_I4, 1280);
var case1 = il.Create(OpCodes.Ldc_I4, 1920);
var case2 = il.Create(OpCodes.Ldc_I4, 2160);
var case3 = il.Create(OpCodes.Ldc_I4, 2560);
var case4 = il.Create(OpCodes.Ldc_I4, 2960);
var fallback = il.Create(OpCodes.Ldc_I4, 1280);
var apply = il.Create(OpCodes.Ldloc, width);

il.Append(il.Create(OpCodes.Ldsfld, gLawnApp));
il.Append(il.Create(OpCodes.Ldarg_0));
il.Append(il.Create(OpCodes.Stfld, resolutionScaleField));
il.Append(il.Create(OpCodes.Ldsfld, gLawnApp));
il.Append(il.Create(OpCodes.Ldfld, fullscreenField));
il.Append(il.Create(OpCodes.Brtrue, ret));
il.Append(il.Create(OpCodes.Ldarg_0));
il.Append(il.Create(OpCodes.Switch, new[] { case0, case1, case2, case3, case4 }));
il.Append(il.Create(OpCodes.Br, fallback));

void AddCase(Instruction label, int h)
{
    il.Append(label);
    il.Append(il.Create(OpCodes.Stloc, width));
    il.Append(il.Create(OpCodes.Ldc_I4, h));
    il.Append(il.Create(OpCodes.Stloc, height));
    il.Append(il.Create(OpCodes.Br, apply));
}
AddCase(case0, 720);
AddCase(case1, 1080);
AddCase(case2, 1080);
AddCase(case3, 1440);
AddCase(case4, 1440);
AddCase(fallback, 720);

il.Append(apply);
il.Append(il.Create(OpCodes.Ldloc, height));
il.Append(il.Create(OpCodes.Ldc_I4_3));
il.Append(il.Create(OpCodes.Call, screenSetResolution));
il.Append(ret);

Console.WriteLine("NATIVE_AUTHORITY token=0x0600014A address=0x000000018031BF70 next=0x000000018031C080 gameassembly_sha256=9ebd7ca996a5b03fb4a766f7a2502b660d581ddbbb36af4d7bf2f06a211da39d metadata_sha256=ad992341add498bd1018ac980f42171e0737b0793bf945c8714ad70a445b36b9");
Console.WriteLine("PATCH_SETRESOLUTION table=0:1280x720,1:1920x1080,2:2160x1080,3:2560x1440,4:2960x1440,default:1280x720 fullscreen=early-return mode=3");

module.Write(output);
var outputSha = Convert.ToHexString(SHA256.HashData(File.ReadAllBytes(output))).ToLowerInvariant();
Console.WriteLine($"OUTPUT_SHA256 {outputSha}");

using var reopen = ModuleDefinition.ReadModule(output, new ReaderParameters { InMemory = true, ReadingMode = ReadingMode.Immediate });
var after = AllTypes(reopen.Types).SelectMany(t => t.Methods).ToList();
if (after.Count != ExpectedMethodDefCount) throw new InvalidDataException("reopen MethodDef count drift");
var rt = after.SingleOrDefault(m => Raw(m) == token) ?? throw new InvalidDataException("reopen target missing");
if (rt.Body.Variables.Count != 2 || rt.Body.Variables.Any(v => v.VariableType.FullName != "System.Int32")) throw new InvalidDataException("SetResolution repaired locals drift");
var ints = rt.Body.Instructions.Where(i => i.OpCode.Code == Code.Ldc_I4).Select(i => (int)i.Operand).ToList();
foreach (var n in new[] { 1280, 720, 1920, 1080, 2160, 2560, 1440, 2960 }) if (!ints.Contains(n)) throw new InvalidDataException($"reopen missing native resolution constant {n}");
if (!rt.Body.Instructions.Any(i => i.OpCode.Code == Code.Ldc_I4_3)) throw new InvalidDataException("reopen FullScreenMode 3 constant missing");
if (!rt.Body.Instructions.Any(i => i.OpCode.Code == Code.Call && i.Operand is MethodReference m && m.Name == "SetResolution" && m.DeclaringType.FullName == "UnityEngine.Screen")) throw new InvalidDataException("reopen Screen.SetResolution call missing");
Console.WriteLine($"REOPEN_SETRESOLUTION_PASS locals=2 cases=5 fallback=1280x720 token=0x{token:X8}");

var untouchedAfter = after.Where(m => !changed.Contains(Raw(m))).ToDictionary(Raw, MethodSemantic);
if (untouchedAfter.Count != untouchedBefore.Count) throw new InvalidDataException("untouched method count drift");
var drift = untouchedBefore.Where(kv => !untouchedAfter.TryGetValue(kv.Key, out var sem) || sem != kv.Value).Select(kv => kv.Key).ToList();
if (drift.Count != 0) throw new InvalidDataException("semantic drift outside SetResolution: " + string.Join(',', drift.Select(t => $"0x{t:X8}")));
Console.WriteLine($"SEMANTIC_ISOLATION_PASS untouched_methods={untouchedAfter.Count} changed_methods=1");
return 0;
