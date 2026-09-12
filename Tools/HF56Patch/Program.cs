using Mono.Cecil;
using Mono.Cecil.Cil;
using System.Security.Cryptography;

if (args.Length != 2)
{
    Console.Error.WriteLine("Usage: GodsPVZ.HF56Patch <input.dll> <output.dll>");
    return 2;
}

const string ExpectedInputSha256 = "dc205a40dc2478b3aacbb3a7d6bb1ca96ffb0a964648d34062b4ddc75f3b3655";
const uint BoardAwakeToken = 0x06000524u;
const uint GameStartToken = 0x06000526u;

var input = Path.GetFullPath(args[0]);
var output = Path.GetFullPath(args[1]);
var inputSha = Convert.ToHexString(SHA256.HashData(File.ReadAllBytes(input))).ToLowerInvariant();
Console.WriteLine($"FORMAL_INPUT_SHA256 {inputSha}");
if (inputSha != ExpectedInputSha256)
    throw new InvalidDataException($"HF56 formal HF55 input SHA mismatch: expected {ExpectedInputSha256}, got {inputSha}");

using var module = ModuleDefinition.ReadModule(input, new ReaderParameters { InMemory = true });

static IEnumerable<TypeDefinition> AllTypes(IEnumerable<TypeDefinition> roots)
{
    foreach (var t in roots)
    {
        yield return t;
        foreach (var n in AllTypes(t.NestedTypes)) yield return n;
    }
}

var methodCount = AllTypes(module.Types).SelectMany(t => t.Methods).Count();
if (methodCount != 2317)
    throw new InvalidDataException($"HF56 input MethodDef count drifted: {methodCount}");

MethodDefinition M(uint token, string type, string name)
{
    var m = module.LookupToken(new MetadataToken(TokenType.Method, (int)(token & 0x00FFFFFF))) as MethodDefinition
        ?? throw new InvalidDataException($"HF56 target missing: 0x{token:X8}");
    if (m.DeclaringType.FullName != type || m.Name != name || m.Parameters.Count != 0 || !m.HasBody)
        throw new InvalidDataException($"HF56 target signature drift: 0x{token:X8} {m.FullName}");
    return m;
}

static bool IsModeField(Instruction i) =>
    i.OpCode.Code == Code.Stfld && i.Operand is FieldReference f &&
    f.DeclaringType.FullName == "Administrator" && f.Name == "mode" && f.FieldType.FullName == "System.Int32";

static bool IsNullCheckField(Instruction i, string declaringType, string fieldName) =>
    i.OpCode.Code == Code.Ldfld && i.Operand is FieldReference f &&
    f.DeclaringType.FullName == declaringType && f.Name == fieldName;

static void ReplaceWithLdnull(Instruction i)
{
    i.OpCode = OpCodes.Ldnull;
    i.Operand = null;
}

static void ReplaceWithI4MinusOne(Instruction i)
{
    i.OpCode = OpCodes.Ldc_I4_M1;
    i.Operand = null;
}

var board = M(BoardAwakeToken, "BoardStart", "Awake");
var game = M(GameStartToken, "GameStart", "Start");

int boardModeFix = 0;
for (int i = 1; i < board.Body.Instructions.Count; i++)
{
    if (!IsModeField(board.Body.Instructions[i])) continue;
    var prev = board.Body.Instructions[i - 1];
    if (prev.OpCode.Code != Code.Ldc_I8 || prev.Operand is not long v || v != 4294967295L)
        throw new InvalidDataException($"HF56 BoardStart mode source drift at {prev}");
    ReplaceWithI4MinusOne(prev);
    boardModeFix++;
}
if (boardModeFix != 1)
    throw new InvalidDataException($"HF56 expected exactly one BoardStart mode-width fix, got {boardModeFix}");

int savesNullFix = 0, playerSaveNullFix = 0, gameModeFix = 0;
for (int i = 2; i < game.Body.Instructions.Count; i++)
{
    var cur = game.Body.Instructions[i];
    if (cur.OpCode.Code == Code.Ceq)
    {
        var zero = game.Body.Instructions[i - 1];
        var source = game.Body.Instructions[i - 2];
        if (zero.OpCode.Code == Code.Ldc_I4 && zero.Operand is int z && z == 0)
        {
            if (IsNullCheckField(source, "GlobalStaticVars/LawnApp", "savesManager"))
            {
                ReplaceWithLdnull(zero);
                savesNullFix++;
            }
            else if (IsNullCheckField(source, "SavesManager", "playerSave"))
            {
                ReplaceWithLdnull(zero);
                playerSaveNullFix++;
            }
        }
    }

    if (IsModeField(cur))
    {
        var prev = game.Body.Instructions[i - 1];
        if (prev.OpCode.Code != Code.Ldc_I8 || prev.Operand is not long v || v != 4294967295L)
            throw new InvalidDataException($"HF56 GameStart mode source drift at {prev}");
        ReplaceWithI4MinusOne(prev);
        gameModeFix++;
    }
}

if (savesNullFix != 1 || playerSaveNullFix != 1 || gameModeFix != 1)
    throw new InvalidDataException($"HF56 fix-count drift saves={savesNullFix} playerSave={playerSaveNullFix} gameMode={gameModeFix}");

Console.WriteLine("PATCH 0x06000524 BoardStart.Awake: ldc.i8 0xFFFFFFFF -> ldc.i4.m1");
Console.WriteLine("PATCH 0x06000526 GameStart.Start: savesManager null compare ldc.i4.0 -> ldnull");
Console.WriteLine("PATCH 0x06000526 GameStart.Start: playerSave null compare ldc.i4.0 -> ldnull");
Console.WriteLine("PATCH 0x06000526 GameStart.Start: ldc.i8 0xFFFFFFFF -> ldc.i4.m1");

module.Write(output);
var outputSha = Convert.ToHexString(SHA256.HashData(File.ReadAllBytes(output))).ToLowerInvariant();
Console.WriteLine($"OUTPUT_SHA256 {outputSha}");

using (var reopen = ModuleDefinition.ReadModule(output, new ReaderParameters { InMemory = true }))
{
    var methods = AllTypes(reopen.Types).SelectMany(t => t.Methods).ToList();
    if (methods.Count != 2317) throw new InvalidDataException("HF56 output MethodDef count drifted from 2317");

    MethodDefinition RM(uint token, string type, string name)
    {
        var m = reopen.LookupToken(new MetadataToken(TokenType.Method, (int)(token & 0x00FFFFFF))) as MethodDefinition
            ?? throw new InvalidDataException($"HF56 reopen target missing 0x{token:X8}");
        if (m.DeclaringType.FullName != type || m.Name != name || !m.HasBody)
            throw new InvalidDataException($"HF56 reopen target drift {m.FullName}");
        return m;
    }

    var rb = RM(BoardAwakeToken, "BoardStart", "Awake");
    var rg = RM(GameStartToken, "GameStart", "Start");

    int boardGood = 0;
    for (int i = 1; i < rb.Body.Instructions.Count; i++)
        if (IsModeField(rb.Body.Instructions[i]) && rb.Body.Instructions[i - 1].OpCode.Code == Code.Ldc_I4_M1) boardGood++;

    int savesGood = 0, playerSaveGood = 0, gameModeGood = 0;
    for (int i = 2; i < rg.Body.Instructions.Count; i++)
    {
        var cur = rg.Body.Instructions[i];
        if (cur.OpCode.Code == Code.Ceq && rg.Body.Instructions[i - 1].OpCode.Code == Code.Ldnull)
        {
            var source = rg.Body.Instructions[i - 2];
            if (IsNullCheckField(source, "GlobalStaticVars/LawnApp", "savesManager")) savesGood++;
            if (IsNullCheckField(source, "SavesManager", "playerSave")) playerSaveGood++;
        }
        if (IsModeField(cur) && rg.Body.Instructions[i - 1].OpCode.Code == Code.Ldc_I4_M1) gameModeGood++;
    }

    if (boardGood != 1 || savesGood != 1 || playerSaveGood != 1 || gameModeGood != 1)
        throw new InvalidDataException($"HF56 reopen validation failed board={boardGood} saves={savesGood} playerSave={playerSaveGood} gameMode={gameModeGood}");

    foreach (var m in new[] { rb, rg })
    {
        if (m.Body.Instructions.Any(i => i.Operand is MethodReference mr && mr.DeclaringType.FullName.Contains("Cpp2IL", StringComparison.Ordinal)))
            throw new InvalidDataException($"HF56 Cpp2IL helper remained in {m.FullName}");
        if (m.Body.Instructions.Any(i => i.OpCode.Code == Code.Ldc_I8 && i.Operand is long l && l == 4294967295L))
            throw new InvalidDataException($"HF56 0xFFFFFFFF I8 remained in {m.FullName}");
    }

    Console.WriteLine($"REOPEN BoardStart.Awake il={rb.Body.Instructions.Count} bytes={rb.Body.CodeSize} eh={rb.Body.ExceptionHandlers.Count}");
    Console.WriteLine($"REOPEN GameStart.Start il={rg.Body.Instructions.Count} bytes={rg.Body.CodeSize} eh={rg.Body.ExceptionHandlers.Count}");
}

return 0;
