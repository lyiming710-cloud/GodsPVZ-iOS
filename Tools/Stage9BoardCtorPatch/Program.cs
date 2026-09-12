using Mono.Cecil;
using Mono.Cecil.Cil;
using System.Security.Cryptography;
using System.Text;

if (args.Length != 2)
{
    Console.Error.WriteLine("Usage: GodsPVZ.Stage9BoardCtorPatch <input.dll> <output.dll>");
    return 2;
}

const string ExpectedInputSha256 = "d9e3da6a7ce72daf505aaa806c0c6a6a5b38ecac2308e70135f5245db4144c03";
const int ExpectedMethodDefCount = 2317;
const uint BoardCtorToken = 0x060002D1u;
const uint ChallengeFieldToken = 0x040003B2u;

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

static HashSet<Instruction> Targets(MethodDefinition m)
{
    var s = new HashSet<Instruction>();
    foreach (var i in m.Body.Instructions)
    {
        if (i.Operand is Instruction q) s.Add(q);
        else if (i.Operand is Instruction[] a) foreach (var q2 in a) s.Add(q2);
    }
    foreach (var h in m.Body.ExceptionHandlers)
        foreach (var q in new[] { h.TryStart, h.TryEnd, h.HandlerStart, h.HandlerEnd, h.FilterStart })
            if (q is not null) s.Add(q);
    return s;
}

var input = Path.GetFullPath(args[0]);
var output = Path.GetFullPath(args[1]);
var inputSha = Convert.ToHexString(SHA256.HashData(File.ReadAllBytes(input))).ToLowerInvariant();
Console.WriteLine($"INPUT_SHA256 {inputSha}");
if (inputSha != ExpectedInputSha256) throw new InvalidDataException($"input SHA mismatch: {inputSha}");

using var module = ModuleDefinition.ReadModule(input, new ReaderParameters { InMemory = true, ReadingMode = ReadingMode.Immediate });
var allBefore = AllTypes(module.Types).SelectMany(t => t.Methods).ToList();
if (allBefore.Count != ExpectedMethodDefCount) throw new InvalidDataException($"MethodDef count drifted: {allBefore.Count}");

var challenge = AllTypes(module.Types).SingleOrDefault(t => t.FullName == "ChallengeType") ?? throw new InvalidDataException("ChallengeType missing");
var valueField = challenge.Fields.SingleOrDefault(f => f.Name == "value__") ?? throw new InvalidDataException("ChallengeType.value__ missing");
var noneField = challenge.Fields.SingleOrDefault(f => f.Name == "None" && f.HasConstant) ?? throw new InvalidDataException("ChallengeType.None missing");
if (!challenge.IsEnum || valueField.FieldType.FullName != "System.Int32" || Convert.ToInt32(noneField.Constant) != -1)
    throw new InvalidDataException("ChallengeType layout/None value does not prove Int32 -1");
Console.WriteLine($"ENUM_PROOF type=ChallengeType underlying={valueField.FieldType.FullName} None={noneField.Constant}");

var ctor = module.LookupToken(new MetadataToken(TokenType.Method, (int)(BoardCtorToken & 0x00FFFFFF))) as MethodDefinition
    ?? throw new InvalidDataException("Board ctor token missing");
if (ctor.DeclaringType.FullName != "Board" || ctor.Name != ".ctor" || ctor.Parameters.Count != 0 || !ctor.HasBody)
    throw new InvalidDataException($"Board ctor drift: {ctor.FullName}");
var challengeField = module.LookupToken(new MetadataToken(TokenType.Field, (int)(ChallengeFieldToken & 0x00FFFFFF))) as FieldDefinition
    ?? throw new InvalidDataException("challengeType field token missing");
if (challengeField.DeclaringType.FullName != "Board" || challengeField.Name != "challengeType" || challengeField.FieldType.FullName != "ChallengeType")
    throw new InvalidDataException($"challengeType field drift: {challengeField.FullName}");

var untouchedBefore = allBefore.Where(m => Raw(m) != BoardCtorToken).ToDictionary(Raw, MethodSemantic);
var targets = Targets(ctor);
var ins = ctor.Body.Instructions;
int repaired = 0;
for (int i = 0; i + 2 < ins.Count; i++)
{
    var a = ins[i]; var b = ins[i + 1]; var c = ins[i + 2];
    if (a.OpCode.Code != Code.Ldarg_0) continue;
    if (b.OpCode.Code != Code.Ldc_I8 || b.Operand is not long lv || lv != 4294967295L) continue;
    if (c.OpCode.Code != Code.Stfld || c.Operand is not FieldReference fr || Raw(fr) != ChallengeFieldToken) continue;
    if (targets.Contains(a) || targets.Contains(b) || targets.Contains(c))
        throw new InvalidDataException("Board ctor repair span contains control-flow/EH target");
    b.OpCode = OpCodes.Ldc_I4_M1;
    b.Operand = null;
    repaired++;
}
if (repaired != 1) throw new InvalidDataException($"Expected one Board ctor width repair, got {repaired}");
Console.WriteLine("PATCH Board::.ctor challengeType init ldc.i8 4294967295 -> ldc.i4.m1");

module.Write(output);
var outputSha = Convert.ToHexString(SHA256.HashData(File.ReadAllBytes(output))).ToLowerInvariant();
Console.WriteLine($"OUTPUT_SHA256 {outputSha}");

using var reopen = ModuleDefinition.ReadModule(output, new ReaderParameters { InMemory = true, ReadingMode = ReadingMode.Immediate });
var allAfter = AllTypes(reopen.Types).SelectMany(t => t.Methods).ToList();
if (allAfter.Count != ExpectedMethodDefCount) throw new InvalidDataException($"reopen MethodDef count drifted: {allAfter.Count}");
var rCtor = reopen.LookupToken(new MetadataToken(TokenType.Method, (int)(BoardCtorToken & 0x00FFFFFF))) as MethodDefinition
    ?? throw new InvalidDataException("reopen Board ctor missing");
int good = 0, oldBad = 0;
for (int i = 0; i + 2 < rCtor.Body.Instructions.Count; i++)
{
    var a = rCtor.Body.Instructions[i]; var b = rCtor.Body.Instructions[i + 1]; var c = rCtor.Body.Instructions[i + 2];
    if (a.OpCode.Code != Code.Ldarg_0 || c.OpCode.Code != Code.Stfld || c.Operand is not FieldReference fr || Raw(fr) != ChallengeFieldToken) continue;
    if (b.OpCode.Code == Code.Ldc_I4_M1) good++;
    if (b.OpCode.Code == Code.Ldc_I8 && b.Operand is long lv && lv == 4294967295L) oldBad++;
}
if (good != 1 || oldBad != 0) throw new InvalidDataException($"reopen validation failed good={good} oldbad={oldBad}");

var untouchedAfter = allAfter.Where(m => Raw(m) != BoardCtorToken).ToDictionary(Raw, MethodSemantic);
if (untouchedAfter.Count != untouchedBefore.Count) throw new InvalidDataException("untouched method count drift");
var drift = untouchedBefore.Where(kv => !untouchedAfter.TryGetValue(kv.Key, out var a) || a != kv.Value).Select(kv => kv.Key).ToList();
if (drift.Count != 0) throw new InvalidDataException("semantic drift outside Board ctor: " + string.Join(',', drift.Select(t => $"0x{t:X8}")));

Console.WriteLine($"REOPEN_BOARD_CTOR_PASS good={good} old_bad={oldBad}");
Console.WriteLine($"SEMANTIC_ISOLATION_PASS untouched_methods={untouchedAfter.Count} changed_methods=1");
return 0;
