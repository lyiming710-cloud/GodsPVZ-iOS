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

var specs = new (string type, string method, int parameters, int minIl)[]
{
    ("Plant", "Awake", 0, 2),
    ("Plant", "Start", 0, 2),
    ("Plant", "Start_Characteristic", 0, 2),
    ("Plant", "LoopAddAnimation", 1, 2),
    ("Plant", "Update", 0, 2),
    ("Plant", "FixedUpdate", 0, 2),
    ("Plant", "Planting", 2, 2),
    ("Plant", "PlantDie", 0, 2),
    ("MouseManager", "Update", 0, 2),
    ("MouseManager", "MouseDownUpdate", 0, 2),
    ("Card", "Update", 0, 2),
    ("Card", "CardOnClick", 0, 2),
    ("Zombie", "Awake", 0, 100),
    ("Zombie", "Start", 0, 200),
    ("Zombie", "LoopAddAnimation", 1, 35),
    ("Zombie", "Update", 0, 250),
    ("Zombie", "Update_Attack", 0, 400),
    ("Zombie", "Update_Characteristic", 0, 250),
    ("Zombie", "GetMoveDirection", 0, 150),
    ("Zombie", "SetrSpeed", 1, 1000),
    ("Zombie", "InjuryStatusUpdate_Body", 1, 200),
    ("BuffManager", "Update", 1, 100),
    ("Buff", "Update", 3, 160),
    ("Buff", "Start", 2, 40),
    ("Buff", "End", 2, 55),
    ("StatsIncreased", "Start_stats", 1, 35),
    ("StatsIncreased", "End_stats", 1, 35),
    ("Bleed", "Bleeding", 1, 85),
    ("AttackRange", "TestInRange", 1, 220),
    ("Buff", "Awake", 1, 35),
    ("Hide", "Awake_Hide", 0, 30),
    ("Hide", "Updata_Hide", 0, 60),
    ("Hide", "End_Hide", 0, 35),
    ("Buff", "ComputingIncrement", 2, 60),
    ("Buff", ".ctor", 0, 14),
    ("BuffManager", "GetIncrement", 2, 35),
    ("BuffManager", "EndAll", 1, 35),
    ("BuffManager", "FindBuff", 1, 35),
    ("BuffManager", "FindStatsIncreased", 1, 5),
    ("BuffManager", ".ctor", 0, 12),
    ("Zombie", "IsDisabled", 0, 50),
    ("Zombie", "IsNormalZombie", 0, 25),
    ("Zombie", "IsPlantZombie", 0, 35),
    ("AttackRange", "NewCircleRange", 3, 8),
    ("AttackRange", "NewCircleRange", 4, 55),
    ("Element", ".ctor", 4, 15),
    ("Damage", "AddElement", 1, 45),
    ("Plant", "GetATK", 0, 13),
    ("Plant", "GetDamageRange", 4, 170),
    ("Plant", "GetDamage", 3, 420),
    ("ProjectileManager", "Start", 0, 54),
    ("ProjectileManager", "SetFloatScale", 0, 271),
    ("ProjectileManager", "CrateNewProjectile", 1, 36),
    ("Projectile", "ResetData", 0, 101),
    ("Projectile", "BindTrack", 0, 50),
    ("ParticlesManager", "Start", 0, 58),
    ("ParticlesManager", "CreatNewParticle", 1, 20),
    ("ResourceManager", "Load_projectileSprite", 0, 64),
    ("Damage", "AreaDamage", 0, 90),
    ("Damage", "AreaDamage_Device", 1, 75),
    ("Damage", "AreaDamage_Plant", 0, 70),
    ("Damage", "AreaDamage_Zombie", 0, 70),
    ("ElementManager", "ToEffect", 1, 5),
    ("ElementManager", "GetElement", 1, 30),
    ("Device", "CanAttacked", 1, 40),
    ("Device", "TakeDamage", 2, 235),
    ("Plant", "TakeDamage", 2, 95),
    ("Zombie", "CanAttacked", 0, 40),
    ("Zombie", "GetATK", 0, 13),
    ("Zombie", "TakeDamage", 2, 195),
    ("Zombie", "Hurt_Armor1", 2, 241),
    ("Zombie", "Hurt_Armor2", 2, 203),
    ("Zombie", "Hurt_Artillery", 2, 70),
    ("Zombie", "Hurt_Ashes", 2, 97),
    ("Zombie", "Hurt_Body", 2, 143),
    ("Zombie", "Hurt_FinalDamageReduction", 2, 47),
    ("Zombie", "Hurt_Normal", 2, 73),
    ("Zombie", "Hurt_Real", 2, 59),
    ("Zombie", "Hurt_Throughout", 2, 78),
    ("Projectile", "Update", 0, 335),
    ("Projectile", "CollisionDetect", 0, 105),
    ("Projectile", "CollisionDetect_Ground", 0, 49),
    ("Projectile", "Collision_AudioParticle", 0, 392),
    ("Projectile", "Collision_Device", 1, 43),
    ("Projectile", "Collision_Zombie", 1, 55),
    ("Projectile", "CollisionDetect_Device", 1, 311),
    ("Projectile", "CollisionDetect_Plant", 1, 127),
    ("Projectile", "CollisionDetect_Zombie", 1, 252),
    ("Projectile", "Rotating", 0, 81),
    ("Projectile", "Update_Tracking", 0, 55),
    ("Projectile", "Aim", 1, 31),
    ("Projectile", "SetEulerAngles", 2, 118),
    ("GlobalStaticVars", "CreateAudioAtPoint", 4, 55),
    ("Projectile", "Update_Time", 0, 81),
    ("Projectile", "Update_MoveTrack7", 0, 67),
    ("Zombie", "GetPredictedPosition", 1, 36),
    ("Device", "InjuryStatusUpdate", 0, 70),
    ("Zombie", "ZC_ArmoredFlagWakeUpZombies", 0, 50),
    ("Projectile", "Initial", 6, 10),
    ("Projectile", "Initial", 7, 10),
    ("Projectile", "Initial", 10, 70),
    ("Plant", "KillEvent", 1, 65),
    ("Plant", "PC_SSI_KillEvent", 1, 90),
    ("BuffManager", "AddBuff", 1, 8),
    ("Element", "Burst", 0, 240),
    ("Element", "Decay", 1, 75),
    ("ElementManager", "GetEffectBoardEntry", 2, 40),
    ("ElementManager", "Effect", 1, 90),
    ("ElementManager", "Update", 0, 75),
    ("ElementUIController", "UpdateUI", 1, 150),
    ("GlobalStaticVars", "AppearSprite", 2, 10),
    ("GlobalStaticVars", "GetAnimationSprite_Name", 2, 25),
    ("GlobalStaticVars", "HideSprite", 2, 10),
    ("Plant", "ElementLevelUp", 2, 110),
    ("Plant", "GetER", 0, 12),
    ("Plant", "GetElementPreference", 1, 10),
    ("Zombie", "GetER", 0, 12),
    ("ElementUIController", "Start", 0, 41),
    ("ElementManager", "GetElementColor", 0, 35),
    ("Zombie", "Update_Color", 0, 45),
    ("Zombie", "FixedUpdate", 0, 20),
    ("Zombie", "FixedUpdate_BGM", 0, 60),
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
        if (!m.HasBody || m.Body.Instructions.Count < s.minIl) throw new InvalidDataException($"{label}: invalid body {s.type}.{s.method}/{s.parameters}: {m.Body.Instructions.Count} IL < {s.minIl}");
        Console.WriteLine($"AUDIT {label} {s.type}.{s.method}/{s.parameters} token=0x{m.MetadataToken.ToUInt32():X8} il={m.Body.Instructions.Count} bytes={m.Body.CodeSize}");
    }
}

AuditOnce("OPEN1");
GC.Collect();
GC.WaitForPendingFinalizers();
AuditOnce("OPEN2");
Console.WriteLine("RECOVERY_AUDIT_OK");
return 0;