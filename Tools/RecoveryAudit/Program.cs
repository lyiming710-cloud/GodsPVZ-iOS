using System.Security.Cryptography;
using Mono.Cecil;

if (args.Length != 1)
{
    Console.Error.WriteLine("Usage: GodsPVZ.RecoveryAudit <Assembly-CSharp.dll>");
    return 2;
}

var path = Path.GetFullPath(args[0]);
if (!File.Exists(path)) throw new FileNotFoundException(path);

static IEnumerable<TypeDefinition> AllTypes(IEnumerable<TypeDefinition> roots)
{
    foreach (var t in roots)
    {
        yield return t;
        foreach (var n in AllTypes(t.NestedTypes)) yield return n;
    }
}

var sha = Convert.ToHexString(SHA256.HashData(File.ReadAllBytes(path))).ToLowerInvariant();
Console.WriteLine($"SHA256 {sha}");

var specs = new (string type, string method, int parameters)[]
{
    ("Plant", "Awake", 0),
    ("Plant", "Start", 0),
    ("Plant", "Start_Characteristic", 0),
    ("Plant", "LoopAddAnimation", 1),
    ("Plant", "Update", 0),
    ("Plant", "FixedUpdate", 0),
    ("Plant", "Planting", 2),
    ("Plant", "PlantDie", 0),
    ("MouseManager", "Update", 0),
    ("MouseManager", "MouseDownUpdate", 0),
    ("Card", "Update", 0),
    ("Card", "CardOnClick", 0),
};

void AuditOnce(string label)
{
    using var module = ModuleDefinition.ReadModule(path, new ReaderParameters { InMemory = true, ReadSymbols = false });
    var types = AllTypes(module.Types).ToList();
    var methods = types.SelectMany(t => t.Methods).ToList();
    var bodies = methods.Count(m => m.HasBody);
    Console.WriteLine($"{label} module={module.Assembly.Name.Name} types={types.Count} methods={methods.Count} bodies={bodies}");

    foreach (var s in specs)
    {
        var t = types.Single(x => x.Name == s.type || x.FullName == s.type);
        var matches = t.Methods.Where(m => m.Name == s.method && m.Parameters.Count == s.parameters).ToList();
        if (matches.Count != 1) throw new InvalidDataException($"{label}: expected one {s.type}.{s.method}/{s.parameters}, got {matches.Count}");
        var m = matches[0];
        if (!m.HasBody || m.Body.Instructions.Count < 2) throw new InvalidDataException($"{label}: invalid body {s.type}.{s.method}/{s.parameters}");
        Console.WriteLine($"AUDIT {label} {s.type}.{s.method}/{s.parameters} token=0x{m.MetadataToken.ToUInt32():X8} il={m.Body.Instructions.Count} bytes={m.Body.CodeSize}");
    }
}

AuditOnce("OPEN1");
GC.Collect();
GC.WaitForPendingFinalizers();
AuditOnce("OPEN2");
Console.WriteLine("RECOVERY_AUDIT_OK");
return 0;
