using System.Security.Cryptography;
using Mono.Cecil;
using Mono.Cecil.Cil;

if (args.Length != 1)
{
    Console.Error.WriteLine("Usage: GodsPVZ.HF51Audit <Assembly-CSharp.dll>");
    return 2;
}

const string ExpectedSha256 = "89e1961cefec6f8c532a0ca7cda4bd4afff2152c14f3856842ee44e5d7d7d90e";
var path = Path.GetFullPath(args[0]);
if (!File.Exists(path)) throw new FileNotFoundException(path);
var sha = Convert.ToHexString(SHA256.HashData(File.ReadAllBytes(path))).ToLowerInvariant();
Console.WriteLine($"SHA256 {sha}");
if (sha != ExpectedSha256)
    throw new InvalidDataException($"HF51 candidate SHA mismatch: expected {ExpectedSha256}, got {sha}");

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
};

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
        Console.WriteLine($"HF51_AUDIT {label} 0x{s.token:X8} {s.type}.{s.method}/{s.parameters} generic={m.GenericParameters.Count} il={m.Body.Instructions.Count} bytes={m.Body.CodeSize} eh={m.Body.ExceptionHandlers.Count}");
    }
}

AuditOnce("OPEN1");
GC.Collect();
GC.WaitForPendingFinalizers();
AuditOnce("OPEN2");
Console.WriteLine("HF51_AUDIT_OK");
return 0;
