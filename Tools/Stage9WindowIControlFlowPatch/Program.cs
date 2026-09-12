using Mono.Cecil;
using Mono.Cecil.Cil;
using System.Security.Cryptography;
using System.Text;

if (args.Length != 2)
{
    Console.Error.WriteLine("Usage: GodsPVZ.Stage9WindowIControlFlowPatch <input.dll> <output.dll>");
    return 2;
}

const string ExpectedInputSha256 = "dc2dccac079f4db46934283c380817d7b60e2a82ae71e8e18ba0b9f199bb6931";
const int ExpectedMethodDefCount = 2317;
const uint StartToken = 0x060006F3u;
const uint YesToken = 0x060006F6u;

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

static void AssertLdloc(Instruction i, VariableDefinition v, string label)
{
    if (i.OpCode.Code != Code.Ldloc || i.Operand != v)
        throw new InvalidDataException($"{label}: expected ldloc V_{v.Index}, got {i.OpCode} {i.Operand}");
}

static void AssertBranch(Instruction i, Code code, int targetOffset, string label)
{
    if (i.OpCode.Code != code || i.Operand is not Instruction t || t.Offset != targetOffset)
        throw new InvalidDataException($"{label}: expected {code} IL_{targetOffset:X4}, got {i.OpCode} {i.Operand}");
}

var input = Path.GetFullPath(args[0]);
var output = Path.GetFullPath(args[1]);
var inputSha = Convert.ToHexString(SHA256.HashData(File.ReadAllBytes(input))).ToLowerInvariant();
Console.WriteLine($"INPUT_SHA256 {inputSha}");
if (inputSha != ExpectedInputSha256) throw new InvalidDataException($"input SHA mismatch: {inputSha}");

using var module = ModuleDefinition.ReadModule(input, new ReaderParameters { InMemory = true, ReadingMode = ReadingMode.Immediate });
var methods = AllTypes(module.Types).SelectMany(t => t.Methods).ToList();
if (methods.Count != ExpectedMethodDefCount) throw new InvalidDataException($"MethodDef count drifted: {methods.Count}");

var start = module.LookupToken(new MetadataToken(TokenType.Method, (int)(StartToken & 0x00FFFFFF))) as MethodDefinition
    ?? throw new InvalidDataException("Window_I.Start missing");
var yes = module.LookupToken(new MetadataToken(TokenType.Method, (int)(YesToken & 0x00FFFFFF))) as MethodDefinition
    ?? throw new InvalidDataException("Window_I.Yes missing");
if (start.DeclaringType.FullName != "Window_I" || start.Name != "Start" || !start.HasBody || start.Parameters.Count != 0)
    throw new InvalidDataException($"Start target drift: {start.FullName}");
if (yes.DeclaringType.FullName != "Window_I" || yes.Name != "Yes" || !yes.HasBody || yes.Parameters.Count != 0)
    throw new InvalidDataException($"Yes target drift: {yes.FullName}");

var changed = new HashSet<uint> { StartToken, YesToken };
var untouchedBefore = methods.Where(m => !changed.Contains(Raw(m))).ToDictionary(Raw, MethodSemantic);

// Window_I.Start: Cpp2IL lowered the switch discriminator through object locals and
// accidentally re-used the I==0 boolean for the I==1 and I==2 cases.
if (start.Body.Variables.Count != 9) throw new InvalidDataException($"Start local count drift: {start.Body.Variables.Count}");
var sV0 = start.Body.Variables[0];
var sV1 = start.Body.Variables[1];
var sV2 = start.Body.Variables[2];
if (sV0.VariableType.FullName != "System.Boolean" || sV1.VariableType.FullName != "System.Object" || sV2.VariableType.FullName != "System.Object")
    throw new InvalidDataException("Start local-type precondition drift");
var s15 = At(start, 0x0015); var s19 = At(start, 0x0019); var s31 = At(start, 0x0031); var s35 = At(start, 0x0035);
AssertLdloc(s15, sV0, "Start IL_0015"); AssertBranch(s19, Code.Brtrue, 0x015A, "Start IL_0019");
AssertLdloc(s31, sV0, "Start IL_0031"); AssertBranch(s35, Code.Brtrue, 0x00DC, "Start IL_0035");
sV1.VariableType = module.TypeSystem.Int32;
sV2.VariableType = module.TypeSystem.Int32;
s15.Operand = sV1; s19.OpCode = OpCodes.Brfalse;
s31.Operand = sV2; s35.OpCode = OpCodes.Brfalse;
Console.WriteLine("PATCH_START locals=V1,V2:Object->Int32 branches=I1,I2-zero-tests");

// Window_I.Yes has the same corrupted switch lowering. Cases 1 and 2 create a
// save, case 3 renames, case 0/default returns. Preserve every call/target.
if (yes.Body.Variables.Count != 5) throw new InvalidDataException($"Yes local count drift: {yes.Body.Variables.Count}");
var yV1 = yes.Body.Variables[1];
var yV2 = yes.Body.Variables[2];
var yV4 = yes.Body.Variables[4];
if (yV1.VariableType.FullName != "System.Boolean" || yV2.VariableType.FullName != "System.Object" || yV4.VariableType.FullName != "System.Object")
    throw new InvalidDataException("Yes local-type precondition drift");
var y2F = At(yes, 0x002F); var y33 = At(yes, 0x0033); var y4B = At(yes, 0x004B); var y4F = At(yes, 0x004F);
AssertLdloc(y2F, yV1, "Yes IL_002F"); AssertBranch(y33, Code.Brtrue, 0x0080, "Yes IL_0033");
AssertLdloc(y4B, yV1, "Yes IL_004B"); AssertBranch(y4F, Code.Brtrue, 0x0080, "Yes IL_004F");
yV2.VariableType = module.TypeSystem.Int32;
yV4.VariableType = module.TypeSystem.Int32;
y2F.Operand = yV2; y33.OpCode = OpCodes.Brfalse;
y4B.Operand = yV4; y4F.OpCode = OpCodes.Brfalse;
Console.WriteLine("PATCH_YES locals=V2,V4:Object->Int32 branches=I1,I2-zero-tests");

module.Write(output);
var outputSha = Convert.ToHexString(SHA256.HashData(File.ReadAllBytes(output))).ToLowerInvariant();
Console.WriteLine($"OUTPUT_SHA256 {outputSha}");

using var reopen = ModuleDefinition.ReadModule(output, new ReaderParameters { InMemory = true, ReadingMode = ReadingMode.Immediate });
var after = AllTypes(reopen.Types).SelectMany(t => t.Methods).ToList();
if (after.Count != ExpectedMethodDefCount) throw new InvalidDataException($"reopen MethodDef count drifted: {after.Count}");
var rStart = reopen.LookupToken(new MetadataToken(TokenType.Method, (int)(StartToken & 0x00FFFFFF))) as MethodDefinition
    ?? throw new InvalidDataException("reopen Start missing");
var rYes = reopen.LookupToken(new MetadataToken(TokenType.Method, (int)(YesToken & 0x00FFFFFF))) as MethodDefinition
    ?? throw new InvalidDataException("reopen Yes missing");
if (rStart.Body.Variables[1].VariableType.FullName != "System.Int32" || rStart.Body.Variables[2].VariableType.FullName != "System.Int32")
    throw new InvalidDataException("reopen Start local repair missing");
if (rYes.Body.Variables[2].VariableType.FullName != "System.Int32" || rYes.Body.Variables[4].VariableType.FullName != "System.Int32")
    throw new InvalidDataException("reopen Yes local repair missing");

static void CheckReopenStart(MethodDefinition m)
{
    var v1=m.Body.Variables[1]; var v2=m.Body.Variables[2];
    var i15=At(m,0x0015); var i19=At(m,0x0019); var i31=At(m,0x0031); var i35=At(m,0x0035);
    AssertLdloc(i15,v1,"reopen Start IL_0015"); AssertBranch(i19,Code.Brfalse,0x015A,"reopen Start IL_0019");
    AssertLdloc(i31,v2,"reopen Start IL_0031"); AssertBranch(i35,Code.Brfalse,0x00DC,"reopen Start IL_0035");
}
static void CheckReopenYes(MethodDefinition m)
{
    var v2=m.Body.Variables[2]; var v4=m.Body.Variables[4];
    var i2F=At(m,0x002F); var i33=At(m,0x0033); var i4B=At(m,0x004B); var i4F=At(m,0x004F);
    AssertLdloc(i2F,v2,"reopen Yes IL_002F"); AssertBranch(i33,Code.Brfalse,0x0080,"reopen Yes IL_0033");
    AssertLdloc(i4B,v4,"reopen Yes IL_004B"); AssertBranch(i4F,Code.Brfalse,0x0080,"reopen Yes IL_004F");
}
CheckReopenStart(rStart); CheckReopenYes(rYes);
Console.WriteLine("REOPEN_WINDOWI_CONTROLFLOW_PASS start_cases=0,1,2,3 yes_cases=0,1,2,3");

var untouchedAfter = after.Where(m => !changed.Contains(Raw(m))).ToDictionary(Raw, MethodSemantic);
if (untouchedAfter.Count != untouchedBefore.Count) throw new InvalidDataException("untouched method count drift");
var drift = untouchedBefore.Where(kv => !untouchedAfter.TryGetValue(kv.Key, out var s) || s != kv.Value).Select(kv => kv.Key).ToList();
if (drift.Count != 0) throw new InvalidDataException("semantic drift outside Window_I.Start/Yes: " + string.Join(',', drift.Select(t => $"0x{t:X8}")));
Console.WriteLine($"SEMANTIC_ISOLATION_PASS untouched_methods={untouchedAfter.Count} changed_methods=2");
return 0;
