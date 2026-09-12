using Mono.Cecil;
using Mono.Cecil.Cil;
using System.Security.Cryptography;
using System.Text;

if (args.Length != 2)
{
    Console.Error.WriteLine("Usage: GodsPVZ.Stage9BoardMechanicalPatch <input.dll> <output.dll>");
    return 2;
}

const string ExpectedInputSha256 = "e49557eeb620c2d3be38aaf9c399a10978740e0a26dd8d0b0f8a190b12b8b568";
const int ExpectedMethodDefCount = 2317;
const uint BoardManagerAwakeToken = 0x0600016Bu;
const uint BoardManagerCtorToken = 0x06000176u;
const uint PropChooseSystemCtorToken = 0x06000699u;
const uint PropChooseCtorToken = 0x0600040Fu;
const uint BoardManagerLevelFieldToken = 0x040001AFu;
const uint BoardManagerConfigFieldToken = 0x040001B0u;
const uint PropChooseSystemCurrentBankFieldToken = 0x0400089Eu;
const uint PropChooseIdFieldToken = 0x0400058Cu;

var targetTokens = new HashSet<uint>
{
    BoardManagerAwakeToken,
    BoardManagerCtorToken,
    PropChooseSystemCtorToken,
    PropChooseCtorToken
};

var input = Path.GetFullPath(args[0]);
var output = Path.GetFullPath(args[1]);
var inputSha = Convert.ToHexString(SHA256.HashData(File.ReadAllBytes(input))).ToLowerInvariant();
Console.WriteLine($"INTEGRATED_INPUT_SHA256 {inputSha}");
if (inputSha != ExpectedInputSha256)
    throw new InvalidDataException($"Stage9 Board mechanical input SHA mismatch: expected {ExpectedInputSha256}, got {inputSha}");

static IEnumerable<TypeDefinition> AllTypes(IEnumerable<TypeDefinition> roots)
{
    foreach (var t in roots)
    {
        yield return t;
        foreach (var n in AllTypes(t.NestedTypes)) yield return n;
    }
}

static uint RawToken(IMetadataTokenProvider p) => p.MetadataToken.ToUInt32();

static string OperandSemantic(object? operand, MethodDefinition owner)
{
    if (operand is null) return "";
    if (operand is Instruction bi)
        return $"I#{owner.Body.Instructions.IndexOf(bi)}";
    if (operand is Instruction[] sw)
        return "SW[" + string.Join(',', sw.Select(x => owner.Body.Instructions.IndexOf(x))) + "]";
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
    foreach (var v in m.Body.Variables)
        sb.Append("L:").Append(v.Index).Append(':').Append(v.VariableType.FullName).Append(';');
    foreach (var i in m.Body.Instructions)
        sb.Append("I:").Append(i.OpCode.Code).Append(':').Append(OperandSemantic(i.Operand, m)).Append(';');
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

using var module = ModuleDefinition.ReadModule(input, new ReaderParameters { InMemory = true });
var allBefore = AllTypes(module.Types).SelectMany(t => t.Methods).ToList();
if (allBefore.Count != ExpectedMethodDefCount)
    throw new InvalidDataException($"Stage9 Board input MethodDef count drifted: {allBefore.Count}");

var untouchedBefore = allBefore
    .Where(m => !targetTokens.Contains(RawToken(m)))
    .ToDictionary(m => RawToken(m), MethodSemantic);

MethodDefinition M(uint token, string type, string name)
{
    var m = module.LookupToken(new MetadataToken(TokenType.Method, (int)(token & 0x00FFFFFF))) as MethodDefinition
        ?? throw new InvalidDataException($"Stage9 Board target missing: 0x{token:X8}");
    if (m.DeclaringType.FullName != type || m.Name != name || m.Parameters.Count != 0 || !m.HasBody)
        throw new InvalidDataException($"Stage9 Board target signature drift: 0x{token:X8} {m.FullName}");
    return m;
}

static bool IsField(Instruction i, uint token, string type, string name, string fieldType)
{
    return i.OpCode.Code is Code.Stfld or Code.Ldfld
        && i.Operand is FieldReference f
        && f.MetadataToken.ToUInt32() == token
        && f.DeclaringType.FullName == type
        && f.Name == name
        && f.FieldType.FullName == fieldType;
}

static bool IsI4Zero(Instruction i) =>
    i.OpCode.Code == Code.Ldc_I4_0 ||
    (i.OpCode.Code == Code.Ldc_I4 && i.Operand is int v && v == 0);

static void ReplaceWithI4MinusOne(Instruction i)
{
    i.OpCode = OpCodes.Ldc_I4_M1;
    i.Operand = null;
}

static void ReplaceWithLdnull(Instruction i)
{
    i.OpCode = OpCodes.Ldnull;
    i.Operand = null;
}

var boardCtor = M(BoardManagerCtorToken, "BoardManager", ".ctor");
var boardAwake = M(BoardManagerAwakeToken, "BoardManager", "Awake");
var propSystemCtor = M(PropChooseSystemCtorToken, "PropChooseSystem", ".ctor");
var propCtor = M(PropChooseCtorToken, "Prop_Choose", ".ctor");

int boardLevelFix = 0;
for (int i = 1; i < boardCtor.Body.Instructions.Count; i++)
{
    if (!IsField(boardCtor.Body.Instructions[i], BoardManagerLevelFieldToken, "BoardManager", "level", "System.Int32")) continue;
    var prev = boardCtor.Body.Instructions[i - 1];
    if (prev.OpCode.Code != Code.Ldc_I8 || prev.Operand is not long v || v != 4294967295L)
        throw new InvalidDataException($"BoardManager..ctor level source drift at {prev}");
    ReplaceWithI4MinusOne(prev);
    boardLevelFix++;
}
if (boardLevelFix != 1)
    throw new InvalidDataException($"Expected exactly one BoardManager.level width fix, got {boardLevelFix}");

int boardConfigNullFix = 0;
for (int i = 2; i < boardAwake.Body.Instructions.Count; i++)
{
    var ceq = boardAwake.Body.Instructions[i];
    if (ceq.OpCode.Code != Code.Ceq) continue;
    var zero = boardAwake.Body.Instructions[i - 1];
    var source = boardAwake.Body.Instructions[i - 2];
    if (IsI4Zero(zero) && IsField(source, BoardManagerConfigFieldToken, "BoardManager", "boardConfig", "BoardConfig"))
    {
        ReplaceWithLdnull(zero);
        boardConfigNullFix++;
    }
}
if (boardConfigNullFix != 1)
    throw new InvalidDataException($"Expected exactly one BoardManager.boardConfig null fix, got {boardConfigNullFix}");

int propSystemFix = 0;
for (int i = 1; i < propSystemCtor.Body.Instructions.Count; i++)
{
    if (!IsField(propSystemCtor.Body.Instructions[i], PropChooseSystemCurrentBankFieldToken, "PropChooseSystem", "currentPropBankID", "System.Int32")) continue;
    var prev = propSystemCtor.Body.Instructions[i - 1];
    if (prev.OpCode.Code != Code.Ldc_I8 || prev.Operand is not long v || v != 4294967295L)
        throw new InvalidDataException($"PropChooseSystem..ctor currentPropBankID source drift at {prev}");
    ReplaceWithI4MinusOne(prev);
    propSystemFix++;
}
if (propSystemFix != 1)
    throw new InvalidDataException($"Expected exactly one PropChooseSystem currentPropBankID width fix, got {propSystemFix}");

int propIdFix = 0;
for (int i = 1; i < propCtor.Body.Instructions.Count; i++)
{
    if (!IsField(propCtor.Body.Instructions[i], PropChooseIdFieldToken, "Prop_Choose", "ID", "System.Int32")) continue;
    var prev = propCtor.Body.Instructions[i - 1];
    if (prev.OpCode.Code != Code.Ldc_I8 || prev.Operand is not long v || v != 4294967295L)
        throw new InvalidDataException($"Prop_Choose..ctor ID source drift at {prev}");
    ReplaceWithI4MinusOne(prev);
    propIdFix++;
}
if (propIdFix != 1)
    throw new InvalidDataException($"Expected exactly one Prop_Choose.ID width fix, got {propIdFix}");

Console.WriteLine("PATCH 0x06000176 BoardManager..ctor: level ldc.i8 0xFFFFFFFF -> ldc.i4.m1");
Console.WriteLine("PATCH 0x0600016B BoardManager.Awake: boardConfig null compare ldc.i4.0 -> ldnull");
Console.WriteLine("PATCH 0x06000699 PropChooseSystem..ctor: currentPropBankID ldc.i8 0xFFFFFFFF -> ldc.i4.m1");
Console.WriteLine("PATCH 0x0600040F Prop_Choose..ctor: ID ldc.i8 0xFFFFFFFF -> ldc.i4.m1");

module.Write(output);
var outputSha = Convert.ToHexString(SHA256.HashData(File.ReadAllBytes(output))).ToLowerInvariant();
Console.WriteLine($"OUTPUT_SHA256 {outputSha}");

using var reopen = ModuleDefinition.ReadModule(output, new ReaderParameters { InMemory = true });
var allAfter = AllTypes(reopen.Types).SelectMany(t => t.Methods).ToList();
if (allAfter.Count != ExpectedMethodDefCount)
    throw new InvalidDataException($"Stage9 Board output MethodDef count drifted: {allAfter.Count}");

MethodDefinition RM(uint token, string type, string name)
{
    var m = reopen.LookupToken(new MetadataToken(TokenType.Method, (int)(token & 0x00FFFFFF))) as MethodDefinition
        ?? throw new InvalidDataException($"Stage9 Board reopen target missing: 0x{token:X8}");
    if (m.DeclaringType.FullName != type || m.Name != name || m.Parameters.Count != 0 || !m.HasBody)
        throw new InvalidDataException($"Stage9 Board reopen target drift: 0x{token:X8} {m.FullName}");
    return m;
}

static int CountI4MinusOneStore(MethodDefinition m, uint fieldToken, string type, string name, string fieldType)
{
    int count = 0;
    for (int i = 1; i < m.Body.Instructions.Count; i++)
        if (IsField(m.Body.Instructions[i], fieldToken, type, name, fieldType) && m.Body.Instructions[i - 1].OpCode.Code == Code.Ldc_I4_M1)
            count++;
    return count;
}

var rbCtor = RM(BoardManagerCtorToken, "BoardManager", ".ctor");
var rbAwake = RM(BoardManagerAwakeToken, "BoardManager", "Awake");
var rpSystem = RM(PropChooseSystemCtorToken, "PropChooseSystem", ".ctor");
var rp = RM(PropChooseCtorToken, "Prop_Choose", ".ctor");

if (CountI4MinusOneStore(rbCtor, BoardManagerLevelFieldToken, "BoardManager", "level", "System.Int32") != 1)
    throw new InvalidDataException("BoardManager..ctor reopen validation failed");
if (CountI4MinusOneStore(rpSystem, PropChooseSystemCurrentBankFieldToken, "PropChooseSystem", "currentPropBankID", "System.Int32") != 1)
    throw new InvalidDataException("PropChooseSystem..ctor reopen validation failed");
if (CountI4MinusOneStore(rp, PropChooseIdFieldToken, "Prop_Choose", "ID", "System.Int32") != 1)
    throw new InvalidDataException("Prop_Choose..ctor reopen validation failed");

int nullGood = 0;
for (int i = 2; i < rbAwake.Body.Instructions.Count; i++)
{
    if (rbAwake.Body.Instructions[i].OpCode.Code == Code.Ceq
        && rbAwake.Body.Instructions[i - 1].OpCode.Code == Code.Ldnull
        && IsField(rbAwake.Body.Instructions[i - 2], BoardManagerConfigFieldToken, "BoardManager", "boardConfig", "BoardConfig"))
        nullGood++;
}
if (nullGood != 1)
    throw new InvalidDataException($"BoardManager.Awake reopen null validation failed: {nullGood}");

var untouchedAfter = allAfter.Where(m => !targetTokens.Contains(RawToken(m))).ToDictionary(m => RawToken(m), MethodSemantic);
if (untouchedAfter.Count != untouchedBefore.Count)
    throw new InvalidDataException($"Untouched MethodDef count drifted: before={untouchedBefore.Count} after={untouchedAfter.Count}");

var semanticDrift = new List<uint>();
foreach (var (token, before) in untouchedBefore)
{
    if (!untouchedAfter.TryGetValue(token, out var after) || before != after)
        semanticDrift.Add(token);
}
if (semanticDrift.Count != 0)
    throw new InvalidDataException("Unexpected semantic drift outside four mechanical targets: " + string.Join(',', semanticDrift.Select(t => $"0x{t:X8}")));

Console.WriteLine($"SEMANTIC_ISOLATION_PASS untouched_methods={untouchedAfter.Count} changed_targets=4");
Console.WriteLine($"REOPEN 0x06000176 il={rbCtor.Body.Instructions.Count} bytes={rbCtor.Body.CodeSize}");
Console.WriteLine($"REOPEN 0x0600016B il={rbAwake.Body.Instructions.Count} bytes={rbAwake.Body.CodeSize}");
Console.WriteLine($"REOPEN 0x06000699 il={rpSystem.Body.Instructions.Count} bytes={rpSystem.Body.CodeSize}");
Console.WriteLine($"REOPEN 0x0600040F il={rp.Body.Instructions.Count} bytes={rp.Body.CodeSize}");

return 0;
