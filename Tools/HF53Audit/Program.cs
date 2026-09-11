using System.Security.Cryptography;
using Mono.Cecil;
using Mono.Cecil.Cil;

if (args.Length != 1)
{
    Console.Error.WriteLine("Usage: GodsPVZ.HF53Audit <Assembly-CSharp.dll>");
    return 2;
}

const string ExpectedSha256 = "924a06f86d648d1bc04bb1d73ccc610dbd59daeb040331910dad86541777d9b5";
const uint HF52Token = 0x060003CEu;
const uint CreateAudio3Token = 0x0600013Du;
const uint ZMBGMPauseToken = 0x06000277u;
const uint GameFailToken = 0x060002BCu;
const uint PopupBoardToken = 0x06000705u;

var path = Path.GetFullPath(args[0]);
if (!File.Exists(path)) throw new FileNotFoundException(path);
var sha = Convert.ToHexString(SHA256.HashData(File.ReadAllBytes(path))).ToLowerInvariant();
Console.WriteLine($"SHA256 {sha}");
if (sha != ExpectedSha256)
    throw new InvalidDataException($"HF53 candidate SHA mismatch: expected {ExpectedSha256}, got {sha}");

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
};

static bool HasCall(MethodDefinition m, string declaringType, string name, int parameters)
    => m.Body.Instructions.Select(i => i.Operand).OfType<MethodReference>()
        .Any(x => x.DeclaringType.FullName == declaringType && x.Name == name && x.Parameters.Count == parameters);

static bool HasField(MethodDefinition m, string fullName)
    => m.Body.Instructions.Select(i => i.Operand).OfType<FieldReference>().Any(f => f.FullName == fullName);

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
        Console.WriteLine($"HF53_AUDIT {label} 0x{s.token:X8} {s.type}.{s.method}/{s.parameters} generic={m.GenericParameters.Count} il={m.Body.Instructions.Count} bytes={m.Body.CodeSize} eh={m.Body.ExceptionHandlers.Count}");
    }

    // Retain HF52 exact numerical semantics.
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

    // HF53 target 1: three-argument audio overload must forward the real Vector3 and pitch=1f.
    var a3 = module.LookupToken(new MetadataToken(TokenType.Method, (int)(CreateAudio3Token & 0x00FFFFFF))) as MethodDefinition
        ?? throw new InvalidDataException($"{label}: missing CreateAudioAtPoint/3");
    if (a3.Body.Instructions.Count != 6 || a3.Body.CodeSize != 14 || a3.Body.ExceptionHandlers.Count != 0)
        throw new InvalidDataException($"{label}: CreateAudioAtPoint/3 exact body drift");
    if (!a3.Body.Instructions.Any(i => i.OpCode == OpCodes.Ldc_R4 && i.Operand is float p && p == 1f))
        throw new InvalidDataException($"{label}: CreateAudioAtPoint/3 missing pitch=1f");
    if (!HasCall(a3, "GlobalStaticVars", "CreateAudioAtPoint", 4))
        throw new InvalidDataException($"{label}: CreateAudioAtPoint/3 missing 4-arg forward call");

    // HF53 target 2: manager pause must enumerate zombieList and call Zombie.BGMPasue.
    var zm = module.LookupToken(new MetadataToken(TokenType.Method, (int)(ZMBGMPauseToken & 0x00FFFFFF))) as MethodDefinition
        ?? throw new InvalidDataException($"{label}: missing ZombieManager.BGMPasue");
    if (zm.Body.Instructions.Count != 17 || zm.Body.CodeSize != 52 || zm.Body.ExceptionHandlers.Count != 1)
        throw new InvalidDataException($"{label}: ZombieManager.BGMPasue exact body drift");
    if (!HasField(zm, "System.Collections.Generic.List`1<Zombie> ZombieManager::zombieList") || !HasCall(zm, "Zombie", "BGMPasue", 0))
        throw new InvalidDataException($"{label}: ZombieManager.BGMPasue list/call semantics drift");

    // HF53 target 3: GameFail must use the real camera Vector3 and restored dependency chain.
    var gf = module.LookupToken(new MetadataToken(TokenType.Method, (int)(GameFailToken & 0x00FFFFFF))) as MethodDefinition
        ?? throw new InvalidDataException($"{label}: missing Board.GameFail");
    if (gf.Body.Instructions.Count != 38 || gf.Body.CodeSize != 102 || gf.Body.ExceptionHandlers.Count != 0)
        throw new InvalidDataException($"{label}: Board.GameFail exact body drift");
    if (!gf.Body.Instructions.Any(i => i.OpCode == OpCodes.Ldc_R4 && i.Operand is float f08 && f08 == 0.8f))
        throw new InvalidDataException($"{label}: Board.GameFail missing 0.8 volume multiplier");
    if (!HasCall(gf, "Board", "GamePause", 1) || !HasCall(gf, "GlobalStaticVars", "AudioVolume", 0) ||
        !HasCall(gf, "GlobalStaticVars", "CreateAudioAtPoint", 3) || !HasCall(gf, "ZombieManager", "BGMPasue", 0) ||
        !HasCall(gf, "Window_Q", "PopupNewWindow", 3))
        throw new InvalidDataException($"{label}: Board.GameFail dependency calls drift");
    if (!HasField(gf, "System.Boolean Board::isFailed") || !HasField(gf, "UnityEngine.AudioClip[] Board::audioList1") ||
        !HasField(gf, "UnityEngine.AudioSource[] Board::audioSource") || !HasField(gf, "ZombieManager Board::zombieManager"))
        throw new InvalidDataException($"{label}: Board.GameFail field semantics drift");

    // HF53 target 4: Board popup overload must instantiate, bind board state and parent transform.
    var popup = module.LookupToken(new MetadataToken(TokenType.Method, (int)(PopupBoardToken & 0x00FFFFFF))) as MethodDefinition
        ?? throw new InvalidDataException($"{label}: missing Window_Q.PopupNewWindow(...,Board)");
    if (popup.Body.Instructions.Count != 17 || popup.Body.CodeSize != 45 || popup.Body.ExceptionHandlers.Count != 0)
        throw new InvalidDataException($"{label}: Window_Q.PopupNewWindow exact body drift");
    if (!HasField(popup, "System.Int32 Window_Q::Q") || !HasField(popup, "System.Boolean Window_Q::onBoard") || !HasField(popup, "Board Window_Q::board"))
        throw new InvalidDataException($"{label}: Window_Q popup state stores drift");
    if (!popup.Body.Instructions.Select(i => i.Operand).OfType<MethodReference>().Any(m => m.DeclaringType.FullName == "UnityEngine.Transform" && m.Name == "SetParent" && m.Parameters.Count == 2))
        throw new InvalidDataException($"{label}: Window_Q popup SetParent call missing");
}

AuditOnce("OPEN1");
GC.Collect();
GC.WaitForPendingFinalizers();
AuditOnce("OPEN2");
Console.WriteLine("HF53_AUDIT_OK");
return 0;
