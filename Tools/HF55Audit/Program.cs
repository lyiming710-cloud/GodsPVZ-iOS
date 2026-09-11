using System.Security.Cryptography;
using Mono.Cecil;
using Mono.Cecil.Cil;

if (args.Length != 1)
{
    Console.Error.WriteLine("Usage: GodsPVZ.HF55Audit <Assembly-CSharp.dll>");
    return 2;
}

const string ExpectedSha256 = "63266400a3add461c9cb6dacd302e5ba0070c4445f0e299cd491bd9166d51cbc";
const uint HF54DropToken = 0x06000202u;
const uint HF55ZombieDropToken = 0x06000438u;
var path = Path.GetFullPath(args[0]);
if (!File.Exists(path)) throw new FileNotFoundException(path);
var sha = Convert.ToHexString(SHA256.HashData(File.ReadAllBytes(path))).ToLowerInvariant();
Console.WriteLine($"SHA256 {sha}");
if (sha != ExpectedSha256)
    throw new InvalidDataException($"HF55 candidate SHA mismatch: expected {ExpectedSha256}, got {sha}");

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
    (0x06000438u, "Zombie", "DropLootPiece", 0, 0, 19),
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
        Console.WriteLine($"HF55_AUDIT {label} 0x{s.token:X8} {s.type}.{s.method}/{s.parameters} generic={m.GenericParameters.Count} il={m.Body.Instructions.Count} bytes={m.Body.CodeSize} eh={m.Body.ExceptionHandlers.Count}");
    }

    var hf54 = module.LookupToken(new MetadataToken(TokenType.Method, (int)(HF54DropToken & 0x00FFFFFF))) as MethodDefinition
        ?? throw new InvalidDataException($"{label}: missing HF54 target");
    if (hf54.Body.Instructions.Count != 159 || hf54.Body.CodeSize != 341 || hf54.Body.ExceptionHandlers.Count != 0)
        throw new InvalidDataException($"{label}: HF54 exact body drift il={hf54.Body.Instructions.Count} bytes={hf54.Body.CodeSize} eh={hf54.Body.ExceptionHandlers.Count}");
    if (!HasCall(hf54, "ProjectManager", "CreateProject", 3) || !HasCall(hf54, "Board", "TestWinTargetZombie", 0) ||
        !HasCall(hf54, "Board", "GameFinished", 0) || !HasCall(hf54, "Project", "SunSet", 1) || !HasCall(hf54, "UnityEngine.Random", "Range", 2))
        throw new InvalidDataException($"{label}: HF54 core dependency calls drift");

    var drop = module.LookupToken(new MetadataToken(TokenType.Method, (int)(HF55ZombieDropToken & 0x00FFFFFF))) as MethodDefinition
        ?? throw new InvalidDataException($"{label}: missing HF55 target");
    if (drop.Body.Instructions.Count != 19 || drop.Body.CodeSize != 52 || drop.Body.ExceptionHandlers.Count != 0)
        throw new InvalidDataException($"{label}: HF55 exact body drift il={drop.Body.Instructions.Count} bytes={drop.Body.CodeSize} eh={drop.Body.ExceptionHandlers.Count}");
    if (!HasCall(drop, "Zombie", "GetHaedPosition", 0) || !HasCall(drop, "ProjectManager", "DropLootPiece", 2) ||
        !HasCall(drop, "UnityEngine.GameObject", "get_transform", 0) || !HasCall(drop, "UnityEngine.Transform", "get_position", 0))
        throw new InvalidDataException($"{label}: HF55 required calls drift");
    var fields = drop.Body.Instructions.Select(i => i.Operand).OfType<FieldReference>().Select(f => $"{f.DeclaringType.FullName}::{f.Name}").ToList();
    foreach (var f in new[] { "Zombie::shadow", "Zombie::isOnBoard", "Zombie::board", "Board::projectManager" })
        if (!fields.Contains(f)) throw new InvalidDataException($"{label}: HF55 missing field {f}");

    var ins = drop.Body.Instructions.ToList();
    var getHead = ins.FindIndex(i => i.Operand is MethodReference mr && mr.DeclaringType.FullName == "Zombie" && mr.Name == "GetHaedPosition");
    var pmDrop = ins.FindIndex(i => i.Operand is MethodReference mr && mr.DeclaringType.FullName == "ProjectManager" && mr.Name == "DropLootPiece" && mr.Parameters.Count == 2);
    if (getHead < 0 || pmDrop < 0 || getHead >= pmDrop)
        throw new InvalidDataException($"{label}: HF55 call order drift");
    if (!ins.Skip(getHead + 1).Take(pmDrop - getHead - 1).Any(i => i.OpCode.Code is Code.Stloc or Code.Stloc_0 or Code.Stloc_1 or Code.Stloc_2 or Code.Stloc_3 or Code.Stloc_S))
        throw new InvalidDataException($"{label}: HF55 head position is not materialized to a local");
    if (!ins.Take(pmDrop).Reverse().Take(3).Any(i => i.OpCode.Code is Code.Ldloc or Code.Ldloc_0 or Code.Ldloc_1 or Code.Ldloc_2 or Code.Ldloc_3 or Code.Ldloc_S))
        throw new InvalidDataException($"{label}: HF55 ProjectManager.DropLootPiece is not fed from recovered Vector3 local");
}

AuditOnce("OPEN1");
GC.Collect();
GC.WaitForPendingFinalizers();
AuditOnce("OPEN2");
Console.WriteLine("HF55_AUDIT_OK");
return 0;
