using Mono.Cecil;
using Mono.Cecil.Cil;
using System.Security.Cryptography;
using System.Text;

if (args.Length != 2)
{
    Console.Error.WriteLine("Usage: GodsPVZ.Stage9BoardCtorNativePatch <input.dll> <output.dll>");
    return 2;
}

const string ExpectedInputSha256 = "06be72c9a18079f6b0dea7e6026b1ad1f5cd6f083aedf2ea250e074330aef21d";
const int ExpectedMethodDefCount = 2317;
const uint TargetToken = 0x060002D1u;

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
    if (operand is float f) return $"R4:{BitConverter.SingleToInt32Bits(f):X8}";
    if (operand is double d) return $"R8:{BitConverter.DoubleToInt64Bits(d):X16}";
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
    foreach (var h in m.Body.ExceptionHandlers)
    {
        int Idx(Instruction? x) => x is null ? -1 : m.Body.Instructions.IndexOf(x);
        sb.Append("EH:").Append(h.HandlerType).Append(':')
          .Append(Idx(h.TryStart)).Append(':').Append(Idx(h.TryEnd)).Append(':')
          .Append(Idx(h.HandlerStart)).Append(':').Append(Idx(h.HandlerEnd)).Append(':')
          .Append(Idx(h.FilterStart)).Append(':').Append(h.CatchType?.FullName ?? "").Append(';');
    }
    return sb.ToString();
}

var input = Path.GetFullPath(args[0]);
var output = Path.GetFullPath(args[1]);
var inputSha = Convert.ToHexString(SHA256.HashData(File.ReadAllBytes(input))).ToLowerInvariant();
Console.WriteLine($"INPUT_SHA256 {inputSha}");
if (inputSha != ExpectedInputSha256) throw new InvalidDataException($"input SHA mismatch: {inputSha}");

using var module = ModuleDefinition.ReadModule(input, new ReaderParameters { InMemory = true, ReadingMode = ReadingMode.Immediate });
var allBefore = AllTypes(module.Types).SelectMany(t => t.Methods).ToList();
if (allBefore.Count != ExpectedMethodDefCount) throw new InvalidDataException($"MethodDef count drifted: {allBefore.Count}");
var target = module.LookupToken(new MetadataToken(TokenType.Method, (int)(TargetToken & 0x00FFFFFF))) as MethodDefinition
    ?? throw new InvalidDataException("Board::.ctor missing");
if (target.DeclaringType.FullName != "Board" || target.Name != ".ctor" || target.Parameters.Count != 0 || !target.HasBody)
    throw new InvalidDataException($"target drift: {target.FullName}");
Console.WriteLine($"SOURCE_TARGET token=0x{Raw(target):X8} locals={target.Body.Variables.Count} instructions={target.Body.Instructions.Count} code_size={target.Body.CodeSize}");

var untouchedBefore = allBefore.Where(m => Raw(m) != TargetToken).ToDictionary(Raw, MethodSemantic);

FieldDefinition Field(string name)
    => target.DeclaringType.Fields.SingleOrDefault(f => f.Name == name)
       ?? throw new InvalidDataException($"Board field missing: {name}");

var fChallenge = Field("challengeType");
var fGameSpeed = Field("gameSpeed");
var fCountdown = Field("gameStartCountdown");
var fPopups = Field("popups");
var fCardBankHidden = Field("cardBankHidden");
var fAudio1 = Field("audioList1");
var fAudio2 = Field("audioList2");
var fBgm = Field("BGMList");
var fUiAble = Field("UI_able");
var fZombieUiAble = Field("zombieUI_able");
var fCameraPosition = Field("cameraPosition");
var fDithering = Field("dithering");
var fReward = Field("reward");

if (fChallenge.FieldType.FullName != "ChallengeType" ||
    fGameSpeed.FieldType.FullName != "System.Single" ||
    fCountdown.FieldType.FullName != "System.Single" ||
    fCardBankHidden.FieldType.FullName != "System.Boolean" ||
    fUiAble.FieldType.FullName != "System.Boolean" ||
    fZombieUiAble.FieldType.FullName != "System.Boolean" ||
    fCameraPosition.FieldType.FullName != "UnityEngine.Vector3" ||
    fDithering.FieldType.FullName != "UnityEngine.Vector3" ||
    fReward.FieldType.FullName != "RewardInBoard")
    throw new InvalidDataException("Board ctor field type drift");

if (fAudio1.FieldType is not ArrayType a1 || fAudio2.FieldType is not ArrayType a2 || fBgm.FieldType is not ArrayType a3 ||
    a1.ElementType.FullName != "UnityEngine.AudioClip" || a2.ElementType.FullName != "UnityEngine.AudioClip" || a3.ElementType.FullName != "UnityEngine.AudioClip")
    throw new InvalidDataException("Board audio array type drift");
if (fPopups.FieldType is not GenericInstanceType popupsType || popupsType.ElementType.FullName != "System.Collections.Generic.List`1" || popupsType.GenericArguments.Count != 1 || popupsType.GenericArguments[0].FullName != "Popup")
    throw new InvalidDataException($"Board.popups type drift: {fPopups.FieldType.FullName}");

var oldRefs = target.Body.Instructions.Select(i => i.Operand).OfType<MethodReference>().ToList();
var listCtor = oldRefs.SingleOrDefault(m => m.Name == ".ctor" && m.Parameters.Count == 0 && m.DeclaringType.FullName == fPopups.FieldType.FullName)
    ?? throw new InvalidDataException("List<Popup> ctor reference missing from source body");
var rewardCtor = oldRefs.SingleOrDefault(m => m.Name == ".ctor" && m.Parameters.Count == 0 && m.DeclaringType.FullName == "RewardInBoard")
    ?? throw new InvalidDataException("RewardInBoard ctor reference missing from source body");
var baseCtor = oldRefs.SingleOrDefault(m => m.Name == ".ctor" && m.Parameters.Count == 0 && m.DeclaringType.FullName == "UnityEngine.MonoBehaviour")
    ?? throw new InvalidDataException("MonoBehaviour ctor reference missing from source body");
var vectorZero = allBefore.Where(m => m.HasBody)
    .SelectMany(m => m.Body.Instructions)
    .Select(i => i.Operand)
    .OfType<MethodReference>()
    .FirstOrDefault(m => m.Name == "get_zero" && m.Parameters.Count == 0 && m.DeclaringType.FullName == "UnityEngine.Vector3" && m.ReturnType.FullName == "UnityEngine.Vector3")
    ?? throw new InvalidDataException("Vector3.get_zero reference missing from module");

Console.WriteLine("PC_NATIVE_PROOF pointer=0x180329440 challenge=-1 speed=1 countdown=6 popups=1 cardBankHidden=1 audio=64,128,64 ui_pair=0x0101 vector_zero_copies=2 reward=1 base_ctor=1");

var body = target.Body;
body.ExceptionHandlers.Clear();
body.Instructions.Clear();
body.Variables.Clear();
body.InitLocals = false;
body.MaxStackSize = 3;
var il = body.GetILProcessor();

void StoreI4(FieldDefinition f, int value)
{
    il.Emit(OpCodes.Ldarg_0);
    switch (value)
    {
        case -1: il.Emit(OpCodes.Ldc_I4_M1); break;
        case 0: il.Emit(OpCodes.Ldc_I4_0); break;
        case 1: il.Emit(OpCodes.Ldc_I4_1); break;
        default: il.Emit(OpCodes.Ldc_I4, value); break;
    }
    il.Emit(OpCodes.Stfld, f);
}

StoreI4(fChallenge, -1);
il.Emit(OpCodes.Ldarg_0); il.Emit(OpCodes.Ldc_R4, 1f); il.Emit(OpCodes.Stfld, fGameSpeed);
il.Emit(OpCodes.Ldarg_0); il.Emit(OpCodes.Ldc_R4, 6f); il.Emit(OpCodes.Stfld, fCountdown);
il.Emit(OpCodes.Ldarg_0); il.Emit(OpCodes.Newobj, listCtor); il.Emit(OpCodes.Stfld, fPopups);
StoreI4(fCardBankHidden, 1);
il.Emit(OpCodes.Ldarg_0); il.Emit(OpCodes.Ldc_I4, 64); il.Emit(OpCodes.Newarr, a1.ElementType); il.Emit(OpCodes.Stfld, fAudio1);
il.Emit(OpCodes.Ldarg_0); il.Emit(OpCodes.Ldc_I4, 128); il.Emit(OpCodes.Newarr, a2.ElementType); il.Emit(OpCodes.Stfld, fAudio2);
il.Emit(OpCodes.Ldarg_0); il.Emit(OpCodes.Ldc_I4, 64); il.Emit(OpCodes.Newarr, a3.ElementType); il.Emit(OpCodes.Stfld, fBgm);
StoreI4(fUiAble, 1);
StoreI4(fZombieUiAble, 1);
il.Emit(OpCodes.Ldarg_0); il.Emit(OpCodes.Call, vectorZero); il.Emit(OpCodes.Stfld, fCameraPosition);
il.Emit(OpCodes.Ldarg_0); il.Emit(OpCodes.Call, vectorZero); il.Emit(OpCodes.Stfld, fDithering);
il.Emit(OpCodes.Ldarg_0); il.Emit(OpCodes.Newobj, rewardCtor); il.Emit(OpCodes.Stfld, fReward);
il.Emit(OpCodes.Ldarg_0); il.Emit(OpCodes.Call, baseCtor); il.Emit(OpCodes.Ret);

Console.WriteLine("PATCH Board::.ctor native_backed pc=0x180329440 fields=13 locals=0 cpp2il_synthetic=0");
module.Write(output);
var outputSha = Convert.ToHexString(SHA256.HashData(File.ReadAllBytes(output))).ToLowerInvariant();
Console.WriteLine($"OUTPUT_SHA256 {outputSha}");

using var reopen = ModuleDefinition.ReadModule(output, new ReaderParameters { InMemory = true, ReadingMode = ReadingMode.Immediate });
var allAfter = AllTypes(reopen.Types).SelectMany(t => t.Methods).ToList();
if (allAfter.Count != ExpectedMethodDefCount) throw new InvalidDataException($"reopen MethodDef count drifted: {allAfter.Count}");
var rTarget = reopen.LookupToken(new MetadataToken(TokenType.Method, (int)(TargetToken & 0x00FFFFFF))) as MethodDefinition
    ?? throw new InvalidDataException("reopen Board::.ctor missing");
if (!rTarget.HasBody || rTarget.Body.ExceptionHandlers.Count != 0 || rTarget.Body.Variables.Count != 0)
    throw new InvalidDataException("reopen Board::.ctor body/EH/locals drift");

var requiredFields = new[] { "challengeType", "gameSpeed", "gameStartCountdown", "popups", "cardBankHidden", "audioList1", "audioList2", "BGMList", "UI_able", "zombieUI_able", "cameraPosition", "dithering", "reward" };
foreach (var name in requiredFields)
{
    var count = rTarget.Body.Instructions.Count(i => i.OpCode.Code == Code.Stfld && i.Operand is FieldReference fr && fr.Name == name && fr.DeclaringType.FullName == "Board");
    if (count != 1) throw new InvalidDataException($"reopen field assignment count {name}={count}");
}
var zeroCalls = rTarget.Body.Instructions.Count(i => (i.OpCode.Code == Code.Call || i.OpCode.Code == Code.Callvirt) && i.Operand is MethodReference mr && mr.Name == "get_zero" && mr.DeclaringType.FullName == "UnityEngine.Vector3");
var helperCalls = rTarget.Body.Instructions.Count(i => i.Operand is MethodReference mr && mr.DeclaringType.FullName == "Cpp2ILInjected.Cpp2ILHelpers");
var nativeLocals = rTarget.Body.Variables.Count(v => v.VariableType.MetadataType == MetadataType.IntPtr || v.VariableType.MetadataType == MetadataType.UIntPtr);
if (zeroCalls != 2 || helperCalls != 0 || nativeLocals != 0)
    throw new InvalidDataException($"reopen validation failed zero={zeroCalls} helpers={helperCalls} nativeLocals={nativeLocals}");
Console.WriteLine($"REOPEN_BOARD_CTOR_NATIVE_PASS instructions={rTarget.Body.Instructions.Count} locals=0 zero_calls={zeroCalls} helpers=0 native_locals=0");

var untouchedAfter = allAfter.Where(m => Raw(m) != TargetToken).ToDictionary(Raw, MethodSemantic);
if (untouchedAfter.Count != untouchedBefore.Count) throw new InvalidDataException("untouched method count drift");
var drift = untouchedBefore.Where(kv => !untouchedAfter.TryGetValue(kv.Key, out var a) || a != kv.Value).Select(kv => kv.Key).ToList();
if (drift.Count != 0) throw new InvalidDataException("semantic drift outside Board::.ctor: " + string.Join(',', drift.Select(t => $"0x{t:X8}")));
Console.WriteLine($"SEMANTIC_ISOLATION_PASS untouched_methods={untouchedAfter.Count} changed_methods=1");
return 0;
