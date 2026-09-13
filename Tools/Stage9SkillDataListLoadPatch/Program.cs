using Mono.Cecil;
using Mono.Cecil.Cil;
using System.Security.Cryptography;
using System.Text;

if (args.Length != 2)
{
    Console.Error.WriteLine("Usage: GodsPVZ.Stage9SkillDataListLoadPatch <input.dll> <output.dll>");
    return 2;
}

const string ExpectedInputSha256 = "e2a0ef87b213da46aff086be8b47bce2080a871600bbac2a20fa5977ea5b5904";
const int ExpectedMethodDefCount = 2317;
const uint ExpectedToken = 0x0600051F;

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

var input = Path.GetFullPath(args[0]);
var output = Path.GetFullPath(args[1]);
var inputSha = Convert.ToHexString(SHA256.HashData(File.ReadAllBytes(input))).ToLowerInvariant();
Console.WriteLine($"INPUT_SHA256 {inputSha}");
if (inputSha != ExpectedInputSha256) throw new InvalidDataException($"input SHA mismatch: {inputSha}");

using var module = ModuleDefinition.ReadModule(input, new ReaderParameters { InMemory = true, ReadingMode = ReadingMode.Immediate });
var methods = AllTypes(module.Types).SelectMany(t => t.Methods).ToList();
if (methods.Count != ExpectedMethodDefCount) throw new InvalidDataException($"MethodDef count drifted: {methods.Count}");
var target = methods.SingleOrDefault(m => m.FullName == "SkillDataList SkillDataList::LoadSkillDataList()")
    ?? throw new InvalidDataException("SkillDataList.LoadSkillDataList target missing or ambiguous");
var token = Raw(target);
Console.WriteLine($"TARGET_METHOD token=0x{token:X8} name={target.FullName}");
if (token != ExpectedToken) throw new InvalidDataException($"target token drifted: 0x{token:X8}");

var changed = new HashSet<uint> { token };
var untouchedBefore = methods.Where(m => !changed.Contains(Raw(m))).ToDictionary(Raw, MethodSemantic);

int[] zeroOffsets = { 0x0050, 0x00A7, 0x00C9 };
var zeroIndices = new List<int>();
foreach (var off in zeroOffsets)
{
    var z = At(target, off);
    var idx = target.Body.Instructions.IndexOf(z);
    if (z.OpCode.Code != Code.Ldc_I4 || z.Operand is not int val || val != 0)
        throw new InvalidDataException($"expected ldc.i4 0 at IL_{off:X4}");
    var next = target.Body.Instructions[idx + 1];
    if (next.OpCode.Code != Code.Ceq)
        throw new InvalidDataException($"expected ceq after IL_{off:X4}");
    z.OpCode = OpCodes.Ldnull;
    z.Operand = null;
    zeroIndices.Add(idx);
}
Console.WriteLine("PATCH_SKILLDATALIST_NULL_CHECKS sites=3 opcode=ldnull");

var fallbackNew = At(target, 0x0124);
var fallbackStore = At(target, 0x0129);
var fallbackLoad = At(target, 0x012D);
var fallbackRet = At(target, 0x0131);
var throwIndex = target.Body.Instructions.IndexOf(fallbackRet);
if (fallbackNew.OpCode.Code != Code.Newobj || fallbackNew.Operand is not MethodReference ctor || ctor.DeclaringType.FullName != "System.NullReferenceException" ||
    fallbackStore.OpCode.Code != Code.Stloc || fallbackStore.Operand is not VariableDefinition sv || sv.Index != 13 ||
    fallbackLoad.OpCode.Code != Code.Ldloc || fallbackLoad.Operand is not VariableDefinition lv || lv.Index != 13 ||
    fallbackRet.OpCode.Code != Code.Ret)
    throw new InvalidDataException("SkillDataList null-failure sequence drift");
fallbackRet.OpCode = OpCodes.Throw;
fallbackRet.Operand = null;
Console.WriteLine("PATCH_SKILLDATALIST_NULL_FAILURE action=throw exception=System.NullReferenceException");

module.Write(output);
var outputSha = Convert.ToHexString(SHA256.HashData(File.ReadAllBytes(output))).ToLowerInvariant();
Console.WriteLine($"OUTPUT_SHA256 {outputSha}");

using var reopen = ModuleDefinition.ReadModule(output, new ReaderParameters { InMemory = true, ReadingMode = ReadingMode.Immediate });
var after = AllTypes(reopen.Types).SelectMany(t => t.Methods).ToList();
if (after.Count != ExpectedMethodDefCount) throw new InvalidDataException("reopen MethodDef count drift");
var rt = after.SingleOrDefault(m => Raw(m) == token) ?? throw new InvalidDataException("reopen target missing");
foreach (var idx in zeroIndices)
{
    if (idx < 0 || idx >= rt.Body.Instructions.Count || rt.Body.Instructions[idx].OpCode.Code != Code.Ldnull || rt.Body.Instructions[idx + 1].OpCode.Code != Code.Ceq)
        throw new InvalidDataException($"reopen null-check repair missing at instruction index {idx}");
}
if (throwIndex < 0 || throwIndex >= rt.Body.Instructions.Count || rt.Body.Instructions[throwIndex].OpCode.Code != Code.Throw)
    throw new InvalidDataException("reopen null failure throw missing");
Console.WriteLine($"REOPEN_SKILLDATALIST_PASS null_checks=3 null_failure=throw token=0x{token:X8}");

var untouchedAfter = after.Where(m => !changed.Contains(Raw(m))).ToDictionary(Raw, MethodSemantic);
if (untouchedAfter.Count != untouchedBefore.Count) throw new InvalidDataException("untouched method count drift");
var drift = untouchedBefore.Where(kv => !untouchedAfter.TryGetValue(kv.Key, out var sem) || sem != kv.Value).Select(kv => kv.Key).ToList();
if (drift.Count != 0) throw new InvalidDataException("semantic drift outside SkillDataList.LoadSkillDataList: " + string.Join(',', drift.Select(t => $"0x{t:X8}")));
Console.WriteLine($"SEMANTIC_ISOLATION_PASS untouched_methods={untouchedAfter.Count} changed_methods=1");
return 0;
