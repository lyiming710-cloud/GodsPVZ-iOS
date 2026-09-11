using System.Security.Cryptography;
using Mono.Cecil;
using Mono.Cecil.Cil;

if (args.Length != 1)
{
    Console.Error.WriteLine("Usage: GodsPVZ.HF48Audit <Assembly-CSharp.dll>");
    return 2;
}

const string ExpectedSha256 = "6c1eb1468f398610cc61ea89b7f3a9409f6baeb451e6df0ae6243b28af50e48e";
var path = Path.GetFullPath(args[0]);
if (!File.Exists(path)) throw new FileNotFoundException(path);
var sha = Convert.ToHexString(SHA256.HashData(File.ReadAllBytes(path))).ToLowerInvariant();
Console.WriteLine($"SHA256 {sha}");
if (sha != ExpectedSha256)
    throw new InvalidDataException($"HF48 candidate SHA mismatch: expected {ExpectedSha256}, got {sha}");

static IEnumerable<TypeDefinition> AllTypes(IEnumerable<TypeDefinition> roots)
{
    foreach (var t in roots)
    {
        yield return t;
        foreach (var n in AllTypes(t.NestedTypes)) yield return n;
    }
}

// HF47 + HF48 late-stage targets. HF1-HF46 are independently covered by the
// retained historical RecoveryAudit in the composite artifact.
var specs = new (uint token, string type, string method, int parameters, int minIl)[]
{
    (0x060001B5u, "EnemyManager", "DispatcheWave", 1, 100),
    (0x060001B6u, "EnemyManager", "DispatcheZombie", 1, 60),
    (0x060001C0u, "EnemyManager", "TimeUpdate", 0, 300),
    (0x060001B3u, "EnemyManager", "DispatcheLadderWave", 0, 60),
    (0x060001B4u, "EnemyManager", "DispatcheSPHWave", 0, 80),
    (0x06000273u, "ZombieManager", "Update", 0, 3),
    (0x06000274u, "ZombieManager", "Update_Ladder", 0, 90),
    (0x0600027Du, "ZombieManager", "TriggerLadder", 0, 40),
    (0x060002A4u, "EnemyPath", ".ctor", 4, 25),
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
        if (m.DeclaringType.FullName != s.type || m.Name != s.method || m.Parameters.Count != s.parameters)
            throw new InvalidDataException($"{label}: token drift 0x{s.token:X8}: {m.FullName}");
        if (!m.HasBody || m.Body.Instructions.Count < s.minIl)
            throw new InvalidDataException($"{label}: invalid body 0x{s.token:X8}: {m.Body.Instructions.Count} IL < {s.minIl}");
        if (m.Body.Instructions.Any(i => i.Operand is MethodReference mr && mr.DeclaringType.FullName.Contains("Cpp2IL", StringComparison.Ordinal)))
            throw new InvalidDataException($"{label}: Cpp2IL helper remained 0x{s.token:X8}");
        Console.WriteLine($"HF48_AUDIT {label} 0x{s.token:X8} {s.type}.{s.method}/{s.parameters} il={m.Body.Instructions.Count} bytes={m.Body.CodeSize} eh={m.Body.ExceptionHandlers.Count}");
    }
}

AuditOnce("OPEN1");
GC.Collect();
GC.WaitForPendingFinalizers();
AuditOnce("OPEN2");
Console.WriteLine("HF48_AUDIT_OK");
return 0;
