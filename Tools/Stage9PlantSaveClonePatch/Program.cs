using Mono.Cecil;
using Mono.Cecil.Cil;
using System.Security.Cryptography;
using System.Text;

if (args.Length != 2)
{
    Console.Error.WriteLine("Usage: GodsPVZ.Stage9PlantSaveClonePatch <input.dll> <output.dll>");
    return 2;
}

const string ExpectedInputSha256 = "99a9dcbf8e677b705ec4df86c2034112264b872b6edce805f89c15b2f438abe6";
const int ExpectedMethodDefCount = 2317;

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
var target = methods.SingleOrDefault(m => m.FullName == "PlantSave PlantSave::Clone()")
    ?? throw new InvalidDataException("PlantSave.Clone target missing or ambiguous");
var targetToken = Raw(target);
Console.WriteLine($"TARGET_METHOD token=0x{targetToken:X8} name={target.FullName}");
if (target.Body.Variables.Count <= 16 || target.Body.Variables[7].VariableType.FullName != "System.Object" || target.Body.Variables[16].VariableType.FullName != "System.IndexOutOfRangeException")
    throw new InvalidDataException("PlantSave.Clone local layout drift");

var changed = new HashSet<uint> { targetToken };
var untouchedBefore = methods.Where(m => !changed.Contains(Raw(m))).ToDictionary(Raw, MethodSemantic);

var initIndex = At(target, 0x0206);
var storeIndex = At(target, 0x020B);
if (initIndex.OpCode.Code != Code.Ldc_I4 || initIndex.Operand is not int init || init != 0 || storeIndex.OpCode.Code != Code.Stloc || storeIndex.Operand is not VariableDefinition v7 || v7.Index != 7)
    throw new InvalidDataException("PlantSave.Clone loop-index initialization drift");
var incrementLoad = At(target, 0x02FF);
var incrementConst = At(target, 0x0303);
var incrementAdd = At(target, 0x0308);
var incrementStore = At(target, 0x0309);
if (incrementLoad.OpCode.Code != Code.Ldloc || incrementLoad.Operand is not VariableDefinition ilv || ilv.Index != 7 ||
    incrementConst.OpCode.Code != Code.Ldc_I4 || incrementConst.Operand is not int one || one != 1 ||
    incrementAdd.OpCode.Code != Code.Add ||
    incrementStore.OpCode.Code != Code.Stloc || incrementStore.Operand is not VariableDefinition isv || isv.Index != 7)
    throw new InvalidDataException("PlantSave.Clone loop-index arithmetic drift");
target.Body.Variables[7].VariableType = module.TypeSystem.Int32;
Console.WriteLine("PATCH_PLANTSAVE_CLONE_INDEX local=V7 type=System.Int32");

var fallbackNew = At(target, 0x036B);
var fallbackStore = At(target, 0x0370);
var fallbackLoad = At(target, 0x0374);
var fallbackRet = At(target, 0x0378);
if (fallbackNew.OpCode.Code != Code.Newobj || fallbackNew.Operand is not MethodReference ctor || ctor.DeclaringType.FullName != "System.IndexOutOfRangeException" ||
    fallbackStore.OpCode.Code != Code.Stloc || fallbackStore.Operand is not VariableDefinition fsv || fsv.Index != 16 ||
    fallbackLoad.OpCode.Code != Code.Ldloc || fallbackLoad.Operand is not VariableDefinition flv || flv.Index != 16 ||
    fallbackRet.OpCode.Code != Code.Ret)
    throw new InvalidDataException("PlantSave.Clone bounds-failure sequence drift");
fallbackRet.OpCode = OpCodes.Throw;
fallbackRet.Operand = null;
Console.WriteLine("PATCH_PLANTSAVE_CLONE_BOUNDS action=throw exception=System.IndexOutOfRangeException");

module.Write(output);
var outputSha = Convert.ToHexString(SHA256.HashData(File.ReadAllBytes(output))).ToLowerInvariant();
Console.WriteLine($"OUTPUT_SHA256 {outputSha}");

using var reopen = ModuleDefinition.ReadModule(output, new ReaderParameters { InMemory = true, ReadingMode = ReadingMode.Immediate });
var after = AllTypes(reopen.Types).SelectMany(t => t.Methods).ToList();
if (after.Count != ExpectedMethodDefCount) throw new InvalidDataException("reopen MethodDef count drift");
var rt = after.SingleOrDefault(m => Raw(m) == targetToken) ?? throw new InvalidDataException("reopen target missing");
if (rt.Body.Variables.Count <= 16 || rt.Body.Variables[7].VariableType.FullName != "System.Int32")
    throw new InvalidDataException("reopen V7 int32 repair missing");
var lastThrow = rt.Body.Instructions.SingleOrDefault(i => i.Offset == 0x0378);
if (lastThrow == null || lastThrow.OpCode.Code != Code.Throw)
    throw new InvalidDataException("reopen bounds throw repair missing");
Console.WriteLine($"REOPEN_PLANTSAVE_CLONE_PASS index_type=System.Int32 bounds=throw token=0x{targetToken:X8}");

var untouchedAfter = after.Where(m => !changed.Contains(Raw(m))).ToDictionary(Raw, MethodSemantic);
if (untouchedAfter.Count != untouchedBefore.Count) throw new InvalidDataException("untouched method count drift");
var drift = untouchedBefore.Where(kv => !untouchedAfter.TryGetValue(kv.Key, out var sem) || sem != kv.Value).Select(kv => kv.Key).ToList();
if (drift.Count != 0) throw new InvalidDataException("semantic drift outside PlantSave.Clone: " + string.Join(',', drift.Select(t => $"0x{t:X8}")));
Console.WriteLine($"SEMANTIC_ISOLATION_PASS untouched_methods={untouchedAfter.Count} changed_methods=1");
return 0;
