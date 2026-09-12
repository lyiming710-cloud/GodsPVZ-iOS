using Mono.Cecil;
using Mono.Cecil.Cil;
using System.Security.Cryptography;
using System.Text;

if (args.Length != 2)
{
    Console.Error.WriteLine("Usage: GodsPVZ.Stage9WindowIPopupNewPatch <input.dll> <output.dll>");
    return 2;
}

const string ExpectedInputSha256 = "338b70971320ab701db67adb24dc9c2c87de5c301cd64edcbedba6bea9d4ffa6";
const int ExpectedMethodDefCount = 2317;
const uint PopupToken = 0x060006FAu;

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
static bool IsI4Zero(Instruction i)
    => i.OpCode.Code == Code.Ldc_I4_0 || (i.OpCode.Code == Code.Ldc_I4 && Convert.ToInt32(i.Operand) == 0);

var input = Path.GetFullPath(args[0]);
var output = Path.GetFullPath(args[1]);
var inputSha = Convert.ToHexString(SHA256.HashData(File.ReadAllBytes(input))).ToLowerInvariant();
Console.WriteLine($"INPUT_SHA256 {inputSha}");
if (inputSha != ExpectedInputSha256) throw new InvalidDataException($"input SHA mismatch: {inputSha}");

using var module = ModuleDefinition.ReadModule(input, new ReaderParameters { InMemory = true, ReadingMode = ReadingMode.Immediate });
var methods = AllTypes(module.Types).SelectMany(t => t.Methods).ToList();
if (methods.Count != ExpectedMethodDefCount) throw new InvalidDataException($"MethodDef count drifted: {methods.Count}");
MethodDefinition M(uint token) => module.LookupToken(new MetadataToken(TokenType.Method, (int)(token & 0x00FFFFFF))) as MethodDefinition
    ?? throw new InvalidDataException($"method 0x{token:X8} missing");
var popup = M(PopupToken);
if (popup.FullName != "Window_I Window_I::PopupNewWindow(System.Int32,UnityEngine.Transform)" || !popup.IsStatic)
    throw new InvalidDataException($"PopupNewWindow target drift: {popup.FullName}");
if (popup.Body.Variables.Count < 7 || popup.Body.Variables[2].VariableType.FullName != "Window_I")
    throw new InvalidDataException("PopupNewWindow local V2 drift");

var changed = new HashSet<uint> { PopupToken };
var untouchedBefore = methods.Where(m => !changed.Contains(Raw(m))).ToDictionary(Raw, MethodSemantic);

var z1 = At(popup, 0x0017); var ceq1 = At(popup, 0x001C);
var z2 = At(popup, 0x006B); var ceq2 = At(popup, 0x0070);
if (!IsI4Zero(z1) || ceq1.OpCode.Code != Code.Ceq) throw new InvalidDataException("first null-check precondition drift");
if (!IsI4Zero(z2) || ceq2.OpCode.Code != Code.Ceq) throw new InvalidDataException("second null-check precondition drift");
z1.OpCode = OpCodes.Ldnull; z1.Operand = null;
z2.OpCode = OpCodes.Ldnull; z2.Operand = null;
Console.WriteLine("PATCH_POPUPNEW_NULL_CHECKS sites=2 opcode=ldnull");

var n = At(popup, 0x009B); var s = At(popup, 0x00A0); var l = At(popup, 0x00A4); var r = At(popup, 0x00A8);
if (n.OpCode.Code != Code.Newobj || n.Operand is not MethodReference nmr || nmr.DeclaringType.FullName != "System.NullReferenceException")
    throw new InvalidDataException("PopupNewWindow fallback newobj drift");
if (s.OpCode.Code != Code.Stloc || s.Operand is not VariableDefinition sv || sv.Index != 6)
    throw new InvalidDataException("PopupNewWindow fallback stloc drift");
if (l.OpCode.Code != Code.Ldloc || l.Operand is not VariableDefinition lv || lv.Index != 6 || r.OpCode.Code != Code.Ret)
    throw new InvalidDataException("PopupNewWindow fallback ldloc/ret drift");
n.OpCode = OpCodes.Ldnull; n.Operand = null;
s.OpCode = OpCodes.Stloc; s.Operand = popup.Body.Variables[2];
l.OpCode = OpCodes.Ldloc; l.Operand = popup.Body.Variables[2];
Console.WriteLine("PATCH_POPUPNEW_NULL_RETURN local=Window_I");

module.Write(output);
var outputSha = Convert.ToHexString(SHA256.HashData(File.ReadAllBytes(output))).ToLowerInvariant();
Console.WriteLine($"OUTPUT_SHA256 {outputSha}");

using var reopen = ModuleDefinition.ReadModule(output, new ReaderParameters { InMemory = true, ReadingMode = ReadingMode.Immediate });
var after = AllTypes(reopen.Types).SelectMany(t => t.Methods).ToList();
if (after.Count != ExpectedMethodDefCount) throw new InvalidDataException("reopen MethodDef count drift");
MethodDefinition RM(uint token) => reopen.LookupToken(new MetadataToken(TokenType.Method, (int)(token & 0x00FFFFFF))) as MethodDefinition
    ?? throw new InvalidDataException("reopen method missing");
var rp = RM(PopupToken);
var ri = rp.Body.Instructions;
if (ri.Count != 43) throw new InvalidDataException($"reopen instruction count drift: {ri.Count}");
if (ri[5].OpCode.Code != Code.Ldnull || ri[6].OpCode.Code != Code.Ceq || ri[25].OpCode.Code != Code.Ldnull || ri[26].OpCode.Code != Code.Ceq)
    throw new InvalidDataException("reopen null-check repair missing");
if (ri[37].OpCode.Code != Code.Ldnull || ri[38].OpCode.Code != Code.Stloc || ri[38].Operand is not VariableDefinition rsv || rsv.Index != 2 ||
    ri[39].OpCode.Code != Code.Ldloc || ri[39].Operand is not VariableDefinition rlv || rlv.Index != 2 || ri[40].OpCode.Code != Code.Ret)
    throw new InvalidDataException("reopen null-return repair missing");
Console.WriteLine("REOPEN_POPUPNEW_PASS null_checks=2 null_return=1");

var untouchedAfter = after.Where(m => !changed.Contains(Raw(m))).ToDictionary(Raw, MethodSemantic);
if (untouchedAfter.Count != untouchedBefore.Count) throw new InvalidDataException("untouched method count drift");
var drift = untouchedBefore.Where(kv => !untouchedAfter.TryGetValue(kv.Key, out var sem) || sem != kv.Value).Select(kv => kv.Key).ToList();
if (drift.Count != 0) throw new InvalidDataException("semantic drift outside PopupNewWindow: " + string.Join(',', drift.Select(t => $"0x{t:X8}")));
Console.WriteLine($"SEMANTIC_ISOLATION_PASS untouched_methods={untouchedAfter.Count} changed_methods=1");
return 0;
