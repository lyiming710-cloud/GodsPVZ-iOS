using Mono.Cecil;
using Mono.Cecil.Cil;
using System.Security.Cryptography;
using System.Text;

if (args.Length != 2)
{
    Console.Error.WriteLine("Usage: GodsPVZ.Stage9QualificationTestPatch <input.dll> <output.dll>");
    return 2;
}

const string ExpectedInputSha256 = "eec540dd6c194fa9dc10d035c879c3e045476d873f39a4489f5006c28df77928";
const int ExpectedMethodDefCount = 2317;
const uint ExpectedToken = 0x06000148;

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
    foreach (var eh in m.Body.ExceptionHandlers)
    {
        int Ix(Instruction? i) => i is null ? -1 : m.Body.Instructions.IndexOf(i);
        sb.Append("EH:").Append(eh.HandlerType).Append(':').Append(Ix(eh.TryStart)).Append(':').Append(Ix(eh.TryEnd))
          .Append(':').Append(Ix(eh.HandlerStart)).Append(':').Append(Ix(eh.HandlerEnd)).Append(':')
          .Append(Ix(eh.FilterStart)).Append(':').Append(eh.CatchType?.FullName ?? "").Append(';');
    }
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

var target = methods.SingleOrDefault(m => m.DeclaringType.FullName == "GlobalStaticVars" && m.Name == "QualificationTest" && m.Parameters.Count == 1 && m.Parameters[0].ParameterType.FullName == "System.String" && m.ReturnType.FullName == "System.Boolean")
    ?? throw new InvalidDataException("GlobalStaticVars.QualificationTest(string) target missing or ambiguous");
var token = Raw(target);
Console.WriteLine($"TARGET_METHOD token=0x{token:X8} name={target.FullName}");
if (token != ExpectedToken) throw new InvalidDataException($"target token drifted: 0x{token:X8}");
if (target.Body.Variables.Count != 22) throw new InvalidDataException($"QualificationTest local count drift: {target.Body.Variables.Count}");

// Guard the exact Cpp2IL-corrupted shape observed in eec5 before replacing the method.
Require(target, 0x0016, Code.Ldloc, "broken HashAlgorithm load");
Require(target, 0x001F, Code.Ceq, "broken HashAlgorithm null compare");
Require(target, 0x0033, Code.Ldloc, "broken ComputeHash receiver load");
Require(target, 0x005F, Code.Ceq, "broken string null compare");
Require(target, 0x00D1, Code.Call, "SHA256.Create call");
Require(target, 0x00D6, Code.Stloc, "SHA256 V5 store");
Require(target, 0x00DA, Code.Call, "Encoding.UTF8 call");
Require(target, 0x00EC, Code.Ceq, "broken Encoding null compare");
Require(target, 0x0126, Code.Ceq, "broken dispose null compare");
var missing = At(target, 0x00A1);
if (missing.OpCode.Code != Code.Ldstr || (string?)missing.Operand != "Method not found @180002D90")
    throw new InvalidDataException("expected lost native dispose placeholder at IL_00A1");
if (target.Body.Variables[5].VariableType.FullName != "System.Security.Cryptography.SHA256") throw new InvalidDataException("V5 SHA256 type drift");
if (target.Body.Variables[6].VariableType.FullName != "System.Security.Cryptography.HashAlgorithm") throw new InvalidDataException("V6 HashAlgorithm type drift");

var allRefs = methods.Where(m => m.HasBody).SelectMany(m => m.Body.Instructions).Select(i => i.Operand).OfType<MethodReference>().ToList();
var allFields = methods.Where(m => m.HasBody).SelectMany(m => m.Body.Instructions).Select(i => i.Operand).OfType<FieldReference>().ToList();
MethodReference MR(string full) => allRefs.FirstOrDefault(x => x.FullName == full) ?? throw new InvalidDataException($"missing MethodRef: {full}");
FieldReference FR(string full) => allFields.FirstOrDefault(x => x.FullName == full) ?? throw new InvalidDataException($"missing FieldRef: {full}");

var shaCreate = MR("System.Security.Cryptography.SHA256 System.Security.Cryptography.SHA256::Create()");
var utf8Get = MR("System.Text.Encoding System.Text.Encoding::get_UTF8()");
var getBytes = MR("System.Byte[] System.Text.Encoding::GetBytes(System.String)");
var computeHash = MR("System.Byte[] System.Security.Cryptography.HashAlgorithm::ComputeHash(System.Byte[])");
var bitToString = MR("System.String System.BitConverter::ToString(System.Byte[])");
var replace = MR("System.String System.String::Replace(System.String,System.String)");
var toLower = MR("System.String System.String::ToLower()");
var strEq = MR("System.Boolean System.String::op_Equality(System.String,System.String)");
var dispose = MR("System.Void System.IDisposable::Dispose()");
var storedHash = FR("System.String Administrator::storedHash");

var changed = new HashSet<uint> { token };
var untouchedBefore = methods.Where(m => !changed.Contains(Raw(m))).ToDictionary(Raw, MethodSemantic);

var body = target.Body;
body.Instructions.Clear();
body.ExceptionHandlers.Clear();
body.Variables.Clear();
body.InitLocals = true;
body.MaxStackSize = 3;

var vSha = new VariableDefinition(shaCreate.ReturnType);
var vEncoding = new VariableDefinition(utf8Get.ReturnType);
var vBytes = new VariableDefinition(getBytes.ReturnType);
var vHash = new VariableDefinition(computeHash.ReturnType);
var vText = new VariableDefinition(module.TypeSystem.String);
var vResult = new VariableDefinition(module.TypeSystem.Boolean);
body.Variables.Add(vSha);
body.Variables.Add(vEncoding);
body.Variables.Add(vBytes);
body.Variables.Add(vHash);
body.Variables.Add(vText);
body.Variables.Add(vResult);

var il = body.GetILProcessor();
var callCreate = il.Create(OpCodes.Call, shaCreate);
var stSha = il.Create(OpCodes.Stloc, vSha);
var tryStart = il.Create(OpCodes.Call, utf8Get);
var stEncoding = il.Create(OpCodes.Stloc, vEncoding);
var ldEncoding = il.Create(OpCodes.Ldloc, vEncoding);
var ldKey = il.Create(OpCodes.Ldarg_0);
var callGetBytes = il.Create(OpCodes.Callvirt, getBytes);
var stBytes = il.Create(OpCodes.Stloc, vBytes);
var ldShaHash = il.Create(OpCodes.Ldloc, vSha);
var ldBytes = il.Create(OpCodes.Ldloc, vBytes);
var callComputeHash = il.Create(OpCodes.Callvirt, computeHash);
var stHash = il.Create(OpCodes.Stloc, vHash);
var ldHash = il.Create(OpCodes.Ldloc, vHash);
var callBitToString = il.Create(OpCodes.Call, bitToString);
var stText = il.Create(OpCodes.Stloc, vText);
var ldText = il.Create(OpCodes.Ldloc, vText);
var ldDash = il.Create(OpCodes.Ldstr, "-");
var ldEmpty = il.Create(OpCodes.Ldstr, "");
var callReplace = il.Create(OpCodes.Callvirt, replace);
var callToLower = il.Create(OpCodes.Callvirt, toLower);
var ldStoredHash = il.Create(OpCodes.Ldsfld, storedHash);
var callEq = il.Create(OpCodes.Call, strEq);
var stResult = il.Create(OpCodes.Stloc, vResult);
var finallyStart = il.Create(OpCodes.Ldloc, vSha);
var ldShaDispose = il.Create(OpCodes.Ldloc, vSha);
var callDispose = il.Create(OpCodes.Callvirt, dispose);
var endFinally = il.Create(OpCodes.Endfinally);
var retLoad = il.Create(OpCodes.Ldloc, vResult);
var ret = il.Create(OpCodes.Ret);
var leave = il.Create(OpCodes.Leave, retLoad);
var brNoDispose = il.Create(OpCodes.Brfalse, endFinally);

foreach (var ins in new[] { callCreate, stSha, tryStart, stEncoding, ldEncoding, ldKey, callGetBytes, stBytes, ldShaHash, ldBytes, callComputeHash, stHash, ldHash, callBitToString, stText, ldText, ldDash, ldEmpty, callReplace, callToLower, ldStoredHash, callEq, stResult, leave, finallyStart, brNoDispose, ldShaDispose, callDispose, endFinally, retLoad, ret })
    il.Append(ins);

body.ExceptionHandlers.Add(new ExceptionHandler(ExceptionHandlerType.Finally)
{
    TryStart = tryStart,
    TryEnd = finallyStart,
    HandlerStart = finallyStart,
    HandlerEnd = retLoad
});

Console.WriteLine("NATIVE_AUTHORITY token=0x06000148 address=0x000000018031BDD0 next=0x000000018031BF70 gameassembly_sha256=9ebd7ca996a5b03fb4a766f7a2502b660d581ddbbb36af4d7bf2f06a211da39d metadata_sha256=ad992341add498bd1018ac980f42171e0737b0793bf945c8714ad70a445b36b9");
Console.WriteLine("PATCH_QUALIFICATIONTEST flow=SHA256.Create>UTF8.GetBytes>ComputeHash>BitConverter.ToString>Replace>ToLower>storedHash-equality finally=IDisposable.Dispose");

module.Write(output);
var outputSha = Convert.ToHexString(SHA256.HashData(File.ReadAllBytes(output))).ToLowerInvariant();
Console.WriteLine($"OUTPUT_SHA256 {outputSha}");

using var reopen = ModuleDefinition.ReadModule(output, new ReaderParameters { InMemory = true, ReadingMode = ReadingMode.Immediate });
var after = AllTypes(reopen.Types).SelectMany(t => t.Methods).ToList();
if (after.Count != ExpectedMethodDefCount) throw new InvalidDataException("reopen MethodDef count drift");
var rt = after.SingleOrDefault(m => Raw(m) == token) ?? throw new InvalidDataException("reopen target missing");
if (rt.Body.ExceptionHandlers.Count != 1 || rt.Body.ExceptionHandlers[0].HandlerType != ExceptionHandlerType.Finally)
    throw new InvalidDataException("QualificationTest finally handler missing after reopen");
if (rt.Body.Variables.Count != 6) throw new InvalidDataException($"QualificationTest local count after repair: {rt.Body.Variables.Count}");
var rrefs = rt.Body.Instructions.Select(i => i.Operand).OfType<MethodReference>().Select(x => x.FullName).ToList();
foreach (var req in new[] { shaCreate.FullName, utf8Get.FullName, getBytes.FullName, computeHash.FullName, bitToString.FullName, replace.FullName, toLower.FullName, strEq.FullName, dispose.FullName })
    if (!rrefs.Contains(req)) throw new InvalidDataException($"reopen missing call: {req}");
if (!rt.Body.Instructions.Any(i => i.OpCode.Code == Code.Ldsfld && i.Operand is FieldReference f && f.FullName == storedHash.FullName))
    throw new InvalidDataException("reopen storedHash field load missing");
if (rt.Body.Instructions.Any(i => i.OpCode.Code == Code.Ldstr && (string?)i.Operand == "Method not found @180002D90"))
    throw new InvalidDataException("lost native dispose placeholder survived repair");
Console.WriteLine($"REOPEN_QUALIFICATIONTEST_PASS locals=6 finally=1 token=0x{token:X8}");

var untouchedAfter = after.Where(m => !changed.Contains(Raw(m))).ToDictionary(Raw, MethodSemantic);
if (untouchedAfter.Count != untouchedBefore.Count) throw new InvalidDataException("untouched method count drift");
var drift = untouchedBefore.Where(kv => !untouchedAfter.TryGetValue(kv.Key, out var sem) || sem != kv.Value).Select(kv => kv.Key).ToList();
if (drift.Count != 0) throw new InvalidDataException("semantic drift outside QualificationTest: " + string.Join(',', drift.Select(t => $"0x{t:X8}")));
Console.WriteLine($"SEMANTIC_ISOLATION_PASS untouched_methods={untouchedAfter.Count} changed_methods=1");
return 0;
