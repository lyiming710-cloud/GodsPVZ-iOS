using System.Security.Cryptography;
using Mono.Cecil;
using Mono.Cecil.Cil;

if (args.Length != 1)
{
    Console.Error.WriteLine("Usage: GodsPVZ.HF54Audit <Assembly-CSharp.dll>");
    return 2;
}

const string ExpectedSha256 = "307dc20c09a6338ecf4a4917b489cc0303d61c79a6aafeb34011e3e67961af2c";
const uint DropLootToken = 0x06000202u;
var path = Path.GetFullPath(args[0]);
if (!File.Exists(path)) throw new FileNotFoundException(path);
var sha = Convert.ToHexString(SHA256.HashData(File.ReadAllBytes(path))).ToLowerInvariant();
Console.WriteLine($"SHA256 {sha}");
if (sha != ExpectedSha256)
    throw new InvalidDataException($"HF54 candidate SHA mismatch: expected {ExpectedSha256}, got {sha}");

static IEnumerable<TypeDefinition> AllTypes(IEnumerable<TypeDefinition> roots)
{
    foreach (var t in roots)
    {
        yield return t;
        foreach (var n in AllTypes(t.NestedTypes)) yield return n;
    }
}

var specs = new (uint token, string type, string method, int parameters, int genericParameters, int minIl)[]
{
    (0x060001B5u, "EnemyManager", "DispatcheWave", 1, 0, 100),
    (0x060001B6u, "EnemyManager", "DispatcheZombie", 1, 0, 60),
    (0x060001C0u, "EnemyManager", "TimeUpdate", 0, 0, 300),
    (0x060001B3u, "EnemyManager", "DispatcheLadderWave", 0, 0, 60),
    (0x060001B4u, "EnemyManager", "DispatcheSPHWave", 0, 0, 80),
    (0x06000273u, "ZombieManager", "Update", 0, 0, 3),
    (0x06000274u, "ZombieManager", "Update_Ladder", 0, 0, 90),
    (0x0600027Du, "ZombieManager", "TriggerLadder", 0, 0, 40),
    (0x060002A4u, "EnemyPath", ".ctor", 4, 0, 25),
    (0x060002A3u, "EnemyPath", "ArrivalTest", 1, 0, 45),
    (0x06000141u, "GlobalStaticVars", "GetAnimationSpritePosition", 2, 0, 12),
    (0x06000201u, "ProjectManager", "CreateProject", 3, 0, 50),
    (0x06000299u, "Grid", "FindDevice_Occupy", 1, 0, 25),
    (0x060002D0u, "Board", "TestWinTargetZombie", 0, 0, 30),
    (0x06000484u, "Zombie", "TranToStant", 1, 0, 85),
    (0x060003CAu, "Project", "SetEndPosition", 1, 1, 80),
    (0x060003CEu, "Project", "SunSet", 1, 0, 14),
    (0x0600013Du, "GlobalStaticVars", "CreateAudioAtPoint", 3, 0, 6),
    (0x06000277u, "ZombieManager", "BGMPasue", 0, 0, 17),
    (0x060002BCu, "Board", "GameFail", 0, 0, 38),
    (0x06000705u, "Window_Q", "PopupNewWindow", 3, 0, 17),
    (0x06000202u, "ProjectManager", "DropLootPiece", 2, 0, 150),
};

static bool HasCall(MethodDefinition m, string declaringType, string name, int parameters)
    => m.Body.Instructions.Select(i => i.Operand).OfType<MethodReference>()
        .Any(x => x.DeclaringType.FullName == declaringType && x.Name == name && x.Parameters.Count == parameters);

void AuditOnce(string label)
{
    using var module = ModuleDefinition.ReadModule(path, new ReaderParameters { InMemory = true, ReadSymbols = false });
    var types = AllTypes(module.Types).ToList();
    var methods = types.SelectMany(t => t.Methods).ToList();
    if (methods.Count != 2317)
        throw new InvalidDataException($"{label}: MethodDef count {methods.Count}, expected 2317");
    Console.WriteLine($"{label} module={module.Assembly.Name.Name} types={types.Count} methods={methods.Count} bodies={methods.Count(m => m.HasBody)}");

    foreach (var s in specs)
    {
        var m = module.LookupToken(new MetadataToken(TokenType.Method, (int)(s.token & 0x00FFFFFF))) as MethodDefinition
            ?? throw new InvalidDataException($"{label}: missing token 0x{s.token:X8}");
        if (m.DeclaringType.FullName != s.type || m.Name != s.method || m.Parameters.Count != s.parameters || m.GenericParameters.Count != s.genericParameters)
            throw new InvalidDataException($"{label}: token/signature drift 0x{s.token:X8}: {m.FullName} generic={m.GenericParameters.Count}");
        if (!m.HasBody || m.Body.Instructions.Count < s.minIl)
            throw new InvalidDataException($"{label}: invalid body 0x{s.token:X8}: {m.Body.Instructions.Count} IL < {s.minIl}");
        if (m.Body.Instructions.Any(i => i.Operand is MethodReference mr && mr.DeclaringType.FullName.Contains("Cpp2IL", StringComparison.Ordinal)))
            throw new InvalidDataException($"{label}: Cpp2IL helper remained 0x{s.token:X8}");
        Console.WriteLine($"HF54_AUDIT {label} 0x{s.token:X8} {s.type}.{s.method}/{s.parameters} generic={m.GenericParameters.Count} il={m.Body.Instructions.Count} bytes={m.Body.CodeSize} eh={m.Body.ExceptionHandlers.Count}");
    }

    var drop = module.LookupToken(new MetadataToken(TokenType.Method, (int)(DropLootToken & 0x00FFFFFF))) as MethodDefinition
        ?? throw new InvalidDataException($"{label}: missing HF54 target");
    if (drop.Body.Instructions.Count != 159 || drop.Body.CodeSize != 341 || drop.Body.ExceptionHandlers.Count != 0)
        throw new InvalidDataException($"{label}: HF54 exact body drift il={drop.Body.Instructions.Count} bytes={drop.Body.CodeSize} eh={drop.Body.ExceptionHandlers.Count}");
    if (!HasCall(drop, "ProjectManager", "CreateProject", 3) || !HasCall(drop, "Board", "TestWinTargetZombie", 0) ||
        !HasCall(drop, "Board", "GameFinished", 0) || !HasCall(drop, "Project", "SunSet", 1) || !HasCall(drop, "UnityEngine.Random", "Range", 2))
        throw new InvalidDataException($"{label}: HF54 core dependency calls drift");
    var genericCalls = drop.Body.Instructions.Select(i => i.Operand).OfType<GenericInstanceMethod>()
        .Where(g => g.ElementMethod.DeclaringType.FullName == "Project" && g.ElementMethod.Name == "SetEndPosition" && g.GenericArguments.Count == 1)
        .ToList();
    if (genericCalls.Count < 4 || genericCalls.Any(g => g.GenericArguments[0].FullName != "Zombie"))
        throw new InvalidDataException($"{label}: HF54 SetEndPosition<Zombie> calls drift count={genericCalls.Count}");
    var ints = drop.Body.Instructions.Where(i => i.OpCode == OpCodes.Ldc_I4 || i.OpCode == OpCodes.Ldc_I4_S)
        .Select(i => Convert.ToInt32(i.Operand)).ToList();
    foreach (var v in new[]{25,50,100,150,800,10000})
        if (!ints.Contains(v)) throw new InvalidDataException($"{label}: HF54 missing threshold/value {v}");
    var floats = drop.Body.Instructions.Where(i => i.OpCode == OpCodes.Ldc_R4).Select(i => (float)i.Operand).ToList();
    if (!floats.Contains(920f) || !floats.Contains(540f))
        throw new InvalidDataException($"{label}: HF54 camera clamp constants drift");
}

AuditOnce("OPEN1");
GC.Collect();
GC.WaitForPendingFinalizers();
AuditOnce("OPEN2");
Console.WriteLine("HF54_AUDIT_OK");
return 0;
