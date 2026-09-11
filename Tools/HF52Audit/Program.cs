using System.Security.Cryptography;
using Mono.Cecil;
using Mono.Cecil.Cil;

if (args.Length != 1)
{
    Console.Error.WriteLine("Usage: GodsPVZ.HF52Audit <Assembly-CSharp.dll>");
    return 2;
}

const string ExpectedSha256 = "1db686c32f24ea81660f3d18ccfd649ee7aeb5a40902e2042639e47355184b82";
const uint HF52Token = 0x060003CEu;
var path = Path.GetFullPath(args[0]);
if (!File.Exists(path)) throw new FileNotFoundException(path);
var sha = Convert.ToHexString(SHA256.HashData(File.ReadAllBytes(path))).ToLowerInvariant();
Console.WriteLine($"SHA256 {sha}");
if (sha != ExpectedSha256)
    throw new InvalidDataException($"HF52 candidate SHA mismatch: expected {ExpectedSha256}, got {sha}");

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
        Console.WriteLine($"HF52_AUDIT {label} 0x{s.token:X8} {s.type}.{s.method}/{s.parameters} generic={m.GenericParameters.Count} il={m.Body.Instructions.Count} bytes={m.Body.CodeSize} eh={m.Body.ExceptionHandlers.Count}");
    }

    var sun = module.LookupToken(new MetadataToken(TokenType.Method, (int)(HF52Token & 0x00FFFFFF))) as MethodDefinition
        ?? throw new InvalidDataException($"{label}: missing HF52 target");
    if (sun.Body.Instructions.Count != 14 || sun.Body.CodeSize != 38 || sun.Body.ExceptionHandlers.Count != 0)
        throw new InvalidDataException($"{label}: HF52 exact body drift il={sun.Body.Instructions.Count} bytes={sun.Body.CodeSize} eh={sun.Body.ExceptionHandlers.Count}");
    if (!sun.Body.Instructions.Any(i => i.OpCode == OpCodes.Ldc_R4 && i.Operand is float f && f == 50f))
        throw new InvalidDataException($"{label}: HF52 missing 50f divisor");
    if (!sun.Body.Instructions.Any(i => i.OpCode == OpCodes.Ldc_R8 && i.Operand is double d && d == 0.5d))
        throw new InvalidDataException($"{label}: HF52 missing 0.5 exponent");
    if (!sun.Body.Instructions.Any(i => i.OpCode == OpCodes.Conv_R8) || sun.Body.Instructions.Count(i => i.OpCode == OpCodes.Conv_R4) < 2)
        throw new InvalidDataException($"{label}: HF52 conversion chain drift");
    var pow = sun.Body.Instructions.Select(i => i.Operand).OfType<MethodReference>()
        .SingleOrDefault(m => m.DeclaringType.FullName == "System.Math" && m.Name == "Pow" && m.Parameters.Count == 2);
    if (pow == null || pow.ReturnType.FullName != "System.Double" || pow.Parameters.Any(p => p.ParameterType.FullName != "System.Double"))
        throw new InvalidDataException($"{label}: HF52 Math.Pow(double,double) reference missing/drifted");
    var stores = sun.Body.Instructions.Where(i => i.OpCode == OpCodes.Stfld).Select(i => (i.Operand as FieldReference)?.FullName).ToList();
    if (!stores.Any(x => x == "System.Int32 Project::value") || !stores.Any(x => x == "System.Single Project::size"))
        throw new InvalidDataException($"{label}: HF52 Project value/size stores drifted");
}

AuditOnce("OPEN1");
GC.Collect();
GC.WaitForPendingFinalizers();
AuditOnce("OPEN2");
Console.WriteLine("HF52_AUDIT_OK");
return 0;
