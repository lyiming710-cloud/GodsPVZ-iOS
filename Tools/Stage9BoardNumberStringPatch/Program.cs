using Mono.Cecil;
using Mono.Cecil.Cil;
using System.Security.Cryptography;
using System.Text;

if (args.Length != 2)
{
    Console.Error.WriteLine("Usage: GodsPVZ.Stage9BoardNumberStringPatch <input.dll> <output.dll>");
    return 2;
}

const string ExpectedInputSha256 = "d65169e360e2b73a619a0047ec42b6df366344b1172b611ef95d6c6f71765c4b";
const int ExpectedMethodDefCount = 2317;
const uint ExpectedToken = 0x06000174;

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
var target = methods.SingleOrDefault(m => m.DeclaringType.FullName == "BoardManager" && m.Name == "GetBoardNumberString" && m.Parameters.Count == 2 && m.Parameters[0].ParameterType.FullName == "ChallengeType" && m.Parameters[1].ParameterType.FullName == "System.Int32" && m.ReturnType.FullName == "System.String")
    ?? throw new InvalidDataException("BoardManager.GetBoardNumberString(ChallengeType,int) target missing or ambiguous");
var token = Raw(target);
Console.WriteLine($"TARGET_METHOD token=0x{token:X8} name={target.FullName}");
if (token != ExpectedToken) throw new InvalidDataException($"target token drifted: 0x{token:X8}");
if (target.Body.Variables.Count != 14) throw new InvalidDataException($"GetBoardNumberString local count drift: {target.Body.Variables.Count}");
if (target.Body.Variables[2].VariableType.FullName != "System.Object") throw new InvalidDataException("expected corrupt V2 object local");
Require(target, 0x000F, Code.Stloc, "invalid V2 store");
if (!target.Body.Instructions.Any(i => i.OpCode.Code == Code.Ldstr && Convert.ToString(i.Operand) == "Not implemented instruction: \"imul ecx\"")) throw new InvalidDataException("expected imul ecx Cpp2IL placeholder missing");
if (!target.Body.Instructions.Any(i => i.OpCode.Code == Code.Ldstr && Convert.ToString(i.Operand) == "Not implemented instruction: \"imul r8d\"")) throw new InvalidDataException("expected imul r8d Cpp2IL placeholder missing");

var operands = target.Body.Instructions.Select(i => i.Operand).ToList();
var intToString = operands.OfType<MethodReference>().FirstOrDefault(m => m.FullName == "System.String System.Int32::ToString()")
    ?? throw new InvalidDataException("Int32.ToString MethodRef missing");
var concat2 = operands.OfType<MethodReference>().FirstOrDefault(m => m.FullName == "System.String System.String::Concat(System.String,System.String)")
    ?? throw new InvalidDataException("String.Concat(string,string) MethodRef missing");
var concat3 = operands.OfType<MethodReference>().FirstOrDefault(m => m.FullName == "System.String System.String::Concat(System.String,System.String,System.String)")
    ?? throw new InvalidDataException("String.Concat(string,string,string) MethodRef missing");
var emptyField = operands.OfType<FieldReference>().FirstOrDefault(f => f.FullName == "System.String System.String::Empty")
    ?? throw new InvalidDataException("String.Empty FieldRef missing");

var changed = new HashSet<uint> { token };
var untouchedBefore = methods.Where(m => !changed.Contains(Raw(m))).ToDictionary(Raw, MethodSemantic);

var body = target.Body;
body.Instructions.Clear();
body.ExceptionHandlers.Clear();
body.Variables.Clear();
body.InitLocals = true;
body.MaxStackSize = 3;
var page = new VariableDefinition(module.TypeSystem.Int32);
var stage = new VariableDefinition(module.TypeSystem.Int32);
body.Variables.Add(page);
body.Variables.Add(stage);
var il = body.GetILProcessor();

var adventure = il.Create(OpCodes.Ldarg_1);
var rescue = il.Create(OpCodes.Ldstr, "R-");
var hard = il.Create(OpCodes.Ldstr, "H-");
var other = il.Create(OpCodes.Ldsfld, emptyField);

il.Append(il.Create(OpCodes.Ldarg_0));
il.Append(il.Create(OpCodes.Brfalse, adventure));
il.Append(il.Create(OpCodes.Ldarg_0));
il.Append(il.Create(OpCodes.Ldc_I4_1));
il.Append(il.Create(OpCodes.Beq, rescue));
il.Append(il.Create(OpCodes.Ldarg_0));
il.Append(il.Create(OpCodes.Ldc_I4_2));
il.Append(il.Create(OpCodes.Beq, hard));
il.Append(il.Create(OpCodes.Br, other));

// Adventure: page=((level-1)/20)+1, stage=((level-1)%20)+1, return "page-stage".
il.Append(adventure);
il.Append(il.Create(OpCodes.Ldc_I4_1));
il.Append(il.Create(OpCodes.Sub));
il.Append(il.Create(OpCodes.Ldc_I4_S, (sbyte)20));
il.Append(il.Create(OpCodes.Div));
il.Append(il.Create(OpCodes.Ldc_I4_1));
il.Append(il.Create(OpCodes.Add));
il.Append(il.Create(OpCodes.Stloc, page));
il.Append(il.Create(OpCodes.Ldarg_1));
il.Append(il.Create(OpCodes.Ldc_I4_1));
il.Append(il.Create(OpCodes.Sub));
il.Append(il.Create(OpCodes.Ldc_I4_S, (sbyte)20));
il.Append(il.Create(OpCodes.Rem));
il.Append(il.Create(OpCodes.Ldc_I4_1));
il.Append(il.Create(OpCodes.Add));
il.Append(il.Create(OpCodes.Stloc, stage));
il.Append(il.Create(OpCodes.Ldloca, page));
il.Append(il.Create(OpCodes.Call, intToString));
il.Append(il.Create(OpCodes.Ldstr, "-"));
il.Append(il.Create(OpCodes.Ldloca, stage));
il.Append(il.Create(OpCodes.Call, intToString));
il.Append(il.Create(OpCodes.Call, concat3));
il.Append(il.Create(OpCodes.Ret));

// Rescue: "R-" + level.
il.Append(rescue);
il.Append(il.Create(OpCodes.Ldarga, target.Parameters[1]));
il.Append(il.Create(OpCodes.Call, intToString));
il.Append(il.Create(OpCodes.Call, concat2));
il.Append(il.Create(OpCodes.Ret));

// HardAdventure: "H-" + level.
il.Append(hard);
il.Append(il.Create(OpCodes.Ldarga, target.Parameters[1]));
il.Append(il.Create(OpCodes.Call, intToString));
il.Append(il.Create(OpCodes.Call, concat2));
il.Append(il.Create(OpCodes.Ret));

il.Append(other);
il.Append(il.Create(OpCodes.Ret));

Console.WriteLine("NATIVE_AUTHORITY token=0x06000174 address=0x000000018030F6F0 function_end=0x000000018030F82A next_managed=0x000000018030FE40 gameassembly_sha256=9ebd7ca996a5b03fb4a766f7a2502b660d581ddbbb36af4d7bf2f06a211da39d metadata_sha256=ad992341add498bd1018ac980f42171e0737b0793bf945c8714ad70a445b36b9");
Console.WriteLine("PATCH_BOARDNUMBER adventure=((level-1)/20+1)-((level-1)%20+1) rescue=R-+level hard=H-+level other=String.Empty");

module.Write(output);
var outputSha = Convert.ToHexString(SHA256.HashData(File.ReadAllBytes(output))).ToLowerInvariant();
Console.WriteLine($"OUTPUT_SHA256 {outputSha}");

using var reopen = ModuleDefinition.ReadModule(output, new ReaderParameters { InMemory = true, ReadingMode = ReadingMode.Immediate });
var after = AllTypes(reopen.Types).SelectMany(t => t.Methods).ToList();
if (after.Count != ExpectedMethodDefCount) throw new InvalidDataException("reopen MethodDef count drift");
var rt = after.SingleOrDefault(m => Raw(m) == token) ?? throw new InvalidDataException("reopen target missing");
if (rt.Body.Variables.Count != 2 || rt.Body.Variables.Any(v => v.VariableType.FullName != "System.Int32")) throw new InvalidDataException("GetBoardNumberString repaired locals drift");
if (rt.Body.Instructions.Any(i => i.OpCode.Code == Code.Ldstr && Convert.ToString(i.Operand)!.StartsWith("Not implemented instruction:"))) throw new InvalidDataException("Cpp2IL placeholder survived repair");
foreach (var s in new[] { "-", "R-", "H-" }) if (!rt.Body.Instructions.Any(i => i.OpCode.Code == Code.Ldstr && Convert.ToString(i.Operand) == s)) throw new InvalidDataException($"reopen missing string {s}");
if (!rt.Body.Instructions.Any(i => i.OpCode.Code == Code.Div) || !rt.Body.Instructions.Any(i => i.OpCode.Code == Code.Rem)) throw new InvalidDataException("reopen adventure div/rem missing");
if (!rt.Body.Instructions.Any(i => i.OpCode.Code == Code.Ldc_I4_S && Convert.ToInt32(i.Operand) == 20)) throw new InvalidDataException("reopen divisor 20 missing");
if (!rt.Body.Instructions.Any(i => i.OpCode.Code == Code.Ldsfld && i.Operand is FieldReference f && f.FullName == "System.String System.String::Empty")) throw new InvalidDataException("reopen String.Empty missing");
Console.WriteLine($"REOPEN_BOARDNUMBER_PASS locals=2 adventure=20-div-rem rescue=R hard=H other=empty token=0x{token:X8}");

var untouchedAfter = after.Where(m => !changed.Contains(Raw(m))).ToDictionary(Raw, MethodSemantic);
if (untouchedAfter.Count != untouchedBefore.Count) throw new InvalidDataException("untouched method count drift");
var drift = untouchedBefore.Where(kv => !untouchedAfter.TryGetValue(kv.Key, out var sem) || sem != kv.Value).Select(kv => kv.Key).ToList();
if (drift.Count != 0) throw new InvalidDataException("semantic drift outside GetBoardNumberString: " + string.Join(',', drift.Select(t => $"0x{t:X8}")));
Console.WriteLine($"SEMANTIC_ISOLATION_PASS untouched_methods={untouchedAfter.Count} changed_methods=1");
return 0;
