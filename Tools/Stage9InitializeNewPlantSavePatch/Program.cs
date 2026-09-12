using Mono.Cecil;
using Mono.Cecil.Cil;
using System.Security.Cryptography;
using System.Text;

if (args.Length != 2)
{
    Console.Error.WriteLine("Usage: GodsPVZ.Stage9InitializeNewPlantSavePatch <input.dll> <output.dll>");
    return 2;
}

const string ExpectedInputSha256 = "bad91c3610837a05e38dbccf5b25730bcdb30b242f213af885a3b31a1c0ed3a0";
const int ExpectedMethodDefCount = 2317;
var NullCheckOffsets = new[] { 0x0013, 0x0046, 0x0070, 0x00F6, 0x0153, 0x01B0, 0x01E5, 0x0218, 0x0261, 0x02A7 };

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
static bool IsLongI4Zero(Instruction i)
    => i.OpCode.Code == Code.Ldc_I4 && i.Operand is int v && v == 0;

var input = Path.GetFullPath(args[0]);
var output = Path.GetFullPath(args[1]);
var inputSha = Convert.ToHexString(SHA256.HashData(File.ReadAllBytes(input))).ToLowerInvariant();
Console.WriteLine($"INPUT_SHA256 {inputSha}");
if (inputSha != ExpectedInputSha256) throw new InvalidDataException($"input SHA mismatch: {inputSha}");

using var module = ModuleDefinition.ReadModule(input, new ReaderParameters { InMemory = true, ReadingMode = ReadingMode.Immediate });
var methods = AllTypes(module.Types).SelectMany(t => t.Methods).ToList();
if (methods.Count != ExpectedMethodDefCount) throw new InvalidDataException($"MethodDef count drifted: {methods.Count}");
var target = methods.SingleOrDefault(m => m.FullName == "PlantSave SavesManager::InitializeNewPlantSave(System.Int32)")
    ?? throw new InvalidDataException("InitializeNewPlantSave(int) target missing or ambiguous");
var targetToken = Raw(target);
Console.WriteLine($"TARGET_METHOD token=0x{targetToken:X8} name={target.FullName}");
if (target.Body.Variables.Count <= 24 || target.Body.Variables[6].VariableType.FullName != "PlantSave" || target.Body.Variables[24].VariableType.FullName != "System.NullReferenceException")
    throw new InvalidDataException("InitializeNewPlantSave local layout drift");

var changed = new HashSet<uint> { targetToken };
var untouchedBefore = methods.Where(m => !changed.Contains(Raw(m))).ToDictionary(Raw, MethodSemantic);
var instructionIndexes = new List<int>();
foreach (var offset in NullCheckOffsets)
{
    var i = At(target, offset);
    var idx = target.Body.Instructions.IndexOf(i);
    if (!IsLongI4Zero(i) || idx + 1 >= target.Body.Instructions.Count || target.Body.Instructions[idx + 1].OpCode.Code != Code.Ceq)
        throw new InvalidDataException($"InitializeNewPlantSave IL_{offset:X4}: expected long ldc.i4 0 followed by ceq");
    instructionIndexes.Add(idx);
    i.OpCode = OpCodes.Ldnull;
    i.Operand = null;
}
Console.WriteLine($"PATCH_PLANTSAVE_NULL_CHECKS sites={instructionIndexes.Count} opcode=ldnull");

var fallbackNew = At(target, 0x023E);
var fallbackIdx = target.Body.Instructions.IndexOf(fallbackNew);
if (fallbackNew.OpCode.Code != Code.Newobj || fallbackNew.Operand is not MethodReference ctor || ctor.DeclaringType.FullName != "System.NullReferenceException")
    throw new InvalidDataException("InitializeNewPlantSave fallback newobj drift");
if (target.Body.Instructions[fallbackIdx + 1].OpCode.Code != Code.Stloc || target.Body.Instructions[fallbackIdx + 1].Operand is not VariableDefinition sv || sv.Index != 24 ||
    target.Body.Instructions[fallbackIdx + 2].OpCode.Code != Code.Ldloc || target.Body.Instructions[fallbackIdx + 2].Operand is not VariableDefinition lv || lv.Index != 24 ||
    target.Body.Instructions[fallbackIdx + 3].OpCode.Code != Code.Ret)
    throw new InvalidDataException("InitializeNewPlantSave fallback sequence drift");
fallbackNew.OpCode = OpCodes.Ldnull; fallbackNew.Operand = null;
target.Body.Instructions[fallbackIdx + 1].OpCode = OpCodes.Stloc; target.Body.Instructions[fallbackIdx + 1].Operand = target.Body.Variables[6];
target.Body.Instructions[fallbackIdx + 2].OpCode = OpCodes.Ldloc; target.Body.Instructions[fallbackIdx + 2].Operand = target.Body.Variables[6];
Console.WriteLine("PATCH_PLANTSAVE_NULL_RETURN local=PlantSave");

module.Write(output);
var outputSha = Convert.ToHexString(SHA256.HashData(File.ReadAllBytes(output))).ToLowerInvariant();
Console.WriteLine($"OUTPUT_SHA256 {outputSha}");

using var reopen = ModuleDefinition.ReadModule(output, new ReaderParameters { InMemory = true, ReadingMode = ReadingMode.Immediate });
var after = AllTypes(reopen.Types).SelectMany(t => t.Methods).ToList();
if (after.Count != ExpectedMethodDefCount) throw new InvalidDataException("reopen MethodDef count drift");
var rt = after.SingleOrDefault(m => Raw(m) == targetToken) ?? throw new InvalidDataException("reopen target missing");
foreach (var idx in instructionIndexes)
{
    if (idx + 1 >= rt.Body.Instructions.Count || rt.Body.Instructions[idx].OpCode.Code != Code.Ldnull || rt.Body.Instructions[idx + 1].OpCode.Code != Code.Ceq)
        throw new InvalidDataException($"reopen null-check repair missing at instruction index {idx}");
}
if (rt.Body.Instructions[fallbackIdx].OpCode.Code != Code.Ldnull ||
    rt.Body.Instructions[fallbackIdx + 1].OpCode.Code != Code.Stloc || rt.Body.Instructions[fallbackIdx + 1].Operand is not VariableDefinition rsv || rsv.Index != 6 ||
    rt.Body.Instructions[fallbackIdx + 2].OpCode.Code != Code.Ldloc || rt.Body.Instructions[fallbackIdx + 2].Operand is not VariableDefinition rlv || rlv.Index != 6 ||
    rt.Body.Instructions[fallbackIdx + 3].OpCode.Code != Code.Ret)
    throw new InvalidDataException("reopen PlantSave null-return repair missing");
Console.WriteLine($"REOPEN_PLANTSAVE_PASS null_checks={instructionIndexes.Count} null_return=1 token=0x{targetToken:X8}");

var untouchedAfter = after.Where(m => !changed.Contains(Raw(m))).ToDictionary(Raw, MethodSemantic);
if (untouchedAfter.Count != untouchedBefore.Count) throw new InvalidDataException("untouched method count drift");
var drift = untouchedBefore.Where(kv => !untouchedAfter.TryGetValue(kv.Key, out var sem) || sem != kv.Value).Select(kv => kv.Key).ToList();
if (drift.Count != 0) throw new InvalidDataException("semantic drift outside InitializeNewPlantSave(int): " + string.Join(',', drift.Select(t => $"0x{t:X8}")));
Console.WriteLine($"SEMANTIC_ISOLATION_PASS untouched_methods={untouchedAfter.Count} changed_methods=1");
return 0;
