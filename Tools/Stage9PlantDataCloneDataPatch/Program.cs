using Mono.Cecil;
using Mono.Cecil.Cil;
using System.Security.Cryptography;
using System.Text;

if (args.Length != 2)
{
    Console.Error.WriteLine("Usage: GodsPVZ.Stage9PlantDataCloneDataPatch <input.dll> <output.dll>");
    return 2;
}

const string ExpectedInputSha256 = "1c8b03824d1de8096fdf79028f69af5b28ab69ca51980dd0fb89ae0889e5a958";
const int ExpectedMethodDefCount = 2317;
const uint ExpectedToken = 0x06000256;
int[] IntLocals = { 12, 13, 15, 16, 17, 18, 19, 20, 21 };

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
static void RequireLocal(Instruction i, Code code, int local, string label)
{
    if (i.OpCode.Code != code || i.Operand is not VariableDefinition v || v.Index != local)
        throw new InvalidDataException($"{label}: expected {code} V{local}, got {i.OpCode.Code} {i.Operand}");
}
static void RequireI4(Instruction i, int value, string label)
{
    if (i.OpCode.Code != Code.Ldc_I4 || i.Operand is not int v || v != value)
        throw new InvalidDataException($"{label}: expected ldc.i4 {value}");
}

var input = Path.GetFullPath(args[0]);
var output = Path.GetFullPath(args[1]);
var inputSha = Convert.ToHexString(SHA256.HashData(File.ReadAllBytes(input))).ToLowerInvariant();
Console.WriteLine($"INPUT_SHA256 {inputSha}");
if (inputSha != ExpectedInputSha256) throw new InvalidDataException($"input SHA mismatch: {inputSha}");

using var module = ModuleDefinition.ReadModule(input, new ReaderParameters { InMemory = true, ReadingMode = ReadingMode.Immediate });
var methods = AllTypes(module.Types).SelectMany(t => t.Methods).ToList();
if (methods.Count != ExpectedMethodDefCount) throw new InvalidDataException($"MethodDef count drifted: {methods.Count}");
var target = methods.SingleOrDefault(m => m.DeclaringType.FullName == "PlantData" && m.Name == "CloneData" && m.Parameters.Count == 1 && m.Parameters[0].ParameterType.FullName == "PlantSave" && m.ReturnType.FullName == "System.Void")
    ?? throw new InvalidDataException("PlantData.CloneData(PlantSave) target missing or ambiguous");
var token = Raw(target);
Console.WriteLine($"TARGET_METHOD token=0x{token:X8} name={target.FullName}");
if (token != ExpectedToken) throw new InvalidDataException($"target token drifted: 0x{token:X8}");
if (target.Body.Variables.Count != 24) throw new InvalidDataException($"PlantData.CloneData local count drift: {target.Body.Variables.Count}");
foreach (var idx in IntLocals)
    if (target.Body.Variables[idx].VariableType.FullName != "System.Object") throw new InvalidDataException($"V{idx} local type drift: {target.Body.Variables[idx].VariableType.FullName}");

// Guard the exact integer-producing data flow. These checks prove the nine locals are I4 values;
// they deliberately do not rewrite the suspicious repeated V1 control-flow branches.
RequireI4(At(target, 0x0038), 1, "order decrement constant");
if (At(target, 0x003D).OpCode.Code != Code.Sub) throw new InvalidDataException("order decrement sub drift");
RequireLocal(At(target, 0x003E), Code.Stloc, 13, "order-1 store");
RequireLocal(At(target, 0x0050), Code.Ldloc, 13, "V13 load");
RequireI4(At(target, 0x0054), 1, "V13 decrement constant");
if (At(target, 0x0059).OpCode.Code != Code.Sub) throw new InvalidDataException("V13 decrement sub drift");
RequireLocal(At(target, 0x005A), Code.Stloc, 16, "V16 store");
RequireLocal(At(target, 0x006C), Code.Ldloc, 16, "V16 load");
RequireI4(At(target, 0x0070), 1, "V16 decrement constant");
if (At(target, 0x0075).OpCode.Code != Code.Sub) throw new InvalidDataException("V16 decrement sub drift");
RequireLocal(At(target, 0x0076), Code.Stloc, 15, "V15 store");
foreach (var (off, value) in new (int,int)[] { (0x00A8,140), (0x00B6,90), (0x00C4,50), (0x00D2,20), (0x00E0,0) })
{
    RequireI4(At(target, off), value, $"V17 constant {value}");
    RequireLocal(At(target, off + 5), Code.Stloc, 17, $"V17 store {value}");
}
RequireLocal(At(target, 0x0138), Code.Ldloc, 12, "V12 load health");
RequireI4(At(target, 0x013C), 1, "health index decrement");
if (At(target, 0x0141).OpCode.Code != Code.Sub) throw new InvalidDataException("health index sub drift");
RequireLocal(At(target, 0x0142), Code.Stloc, 18, "V18 store");
RequireLocal(At(target, 0x0170), Code.Ldloc, 12, "V12 load max health");
if (At(target, 0x0179).OpCode.Code != Code.Sub) throw new InvalidDataException("max health index sub drift");
RequireLocal(At(target, 0x017A), Code.Stloc, 19, "V19 store");
RequireLocal(At(target, 0x01A8), Code.Ldloc, 12, "V12 load attack");
if (At(target, 0x01B1).OpCode.Code != Code.Sub) throw new InvalidDataException("attack index sub drift");
RequireLocal(At(target, 0x01B2), Code.Stloc, 20, "V20 store");
RequireLocal(At(target, 0x0205), Code.Ldloc, 12, "V12 load defense");
if (At(target, 0x020E).OpCode.Code != Code.Sub) throw new InvalidDataException("defense index sub drift");
RequireLocal(At(target, 0x020F), Code.Stloc, 21, "V21 store");
var level = At(target, 0x02A2);
if (level.OpCode.Code != Code.Ldfld || level.Operand is not FieldReference levelField || levelField.FullName != "System.Int32 PlantSave::level") throw new InvalidDataException("PlantSave.level load drift");
RequireLocal(At(target, 0x02A7), Code.Ldloc, 17, "V17 level offset load");
if (At(target, 0x02AB).OpCode.Code != Code.Add) throw new InvalidDataException("level offset add drift");
RequireLocal(At(target, 0x02AC), Code.Stloc, 12, "V12 level index base store");

var changed = new HashSet<uint> { token };
var untouchedBefore = methods.Where(m => !changed.Contains(Raw(m))).ToDictionary(Raw, MethodSemantic);
foreach (var idx in IntLocals) target.Body.Variables[idx].VariableType = module.TypeSystem.Int32;
Console.WriteLine("PATCH_PLANTDATA_CLONEDATA_INT_LOCALS locals=V12,V13,V15,V16,V17,V18,V19,V20,V21 type=System.Int32");
Console.WriteLine("CONTROL_FLOW_UNCHANGED suspicious_order_branches=preserved");

module.Write(output);
var outputSha = Convert.ToHexString(SHA256.HashData(File.ReadAllBytes(output))).ToLowerInvariant();
Console.WriteLine($"OUTPUT_SHA256 {outputSha}");

using var reopen = ModuleDefinition.ReadModule(output, new ReaderParameters { InMemory = true, ReadingMode = ReadingMode.Immediate });
var after = AllTypes(reopen.Types).SelectMany(t => t.Methods).ToList();
if (after.Count != ExpectedMethodDefCount) throw new InvalidDataException("reopen MethodDef count drift");
var rt = after.SingleOrDefault(m => Raw(m) == token) ?? throw new InvalidDataException("reopen target missing");
foreach (var idx in IntLocals)
    if (rt.Body.Variables[idx].VariableType.FullName != "System.Int32") throw new InvalidDataException($"reopen V{idx} int repair missing");
Console.WriteLine($"REOPEN_PLANTDATA_CLONEDATA_PASS int_locals=9 token=0x{token:X8}");

var untouchedAfter = after.Where(m => !changed.Contains(Raw(m))).ToDictionary(Raw, MethodSemantic);
if (untouchedAfter.Count != untouchedBefore.Count) throw new InvalidDataException("untouched method count drift");
var drift = untouchedBefore.Where(kv => !untouchedAfter.TryGetValue(kv.Key, out var sem) || sem != kv.Value).Select(kv => kv.Key).ToList();
if (drift.Count != 0) throw new InvalidDataException("semantic drift outside PlantData.CloneData: " + string.Join(',', drift.Select(t => $"0x{t:X8}")));
Console.WriteLine($"SEMANTIC_ISOLATION_PASS untouched_methods={untouchedAfter.Count} changed_methods=1");
return 0;
