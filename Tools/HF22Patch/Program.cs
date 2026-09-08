using Mono.Cecil;
using Mono.Cecil.Cil;
using System.Reflection;

if (args.Length != 2)
{
    Console.Error.WriteLine("Usage: GodsPVZ.HF22Patch <input.dll> <output.dll>");
    return 2;
}

var input = Path.GetFullPath(args[0]);
var output = Path.GetFullPath(args[1]);
var self = typeof(Template.Damage).Assembly.Location;
if (string.IsNullOrEmpty(self) || !File.Exists(self))
    throw new InvalidOperationException("HF22 requires a multi-file publish so the template assembly is readable.");

using var target = ModuleDefinition.ReadModule(input, new ReaderParameters { InMemory = true, ReadSymbols = false });
using var template = ModuleDefinition.ReadModule(self, new ReaderParameters { InMemory = true, ReadSymbols = false });

static IEnumerable<TypeDefinition> AllTypes(IEnumerable<TypeDefinition> roots)
{
    foreach (var t in roots)
    {
        yield return t;
        foreach (var n in AllTypes(t.NestedTypes)) yield return n;
    }
}

var targetTypes = AllTypes(target.Types).ToList();
var targetDefs = targetTypes.ToDictionary(t => t.FullName, StringComparer.Ordinal);
var targetTypeRefs = target.GetTypeReferences().GroupBy(t => t.FullName).ToDictionary(g => g.Key, g => g.First(), StringComparer.Ordinal);
var targetMethodRefs = targetTypes.SelectMany(t => t.Methods).Where(m => m.HasBody)
    .SelectMany(m => m.Body.Instructions).Select(i => i.Operand).OfType<MethodReference>().ToList();

string TargetName(string fullName) => fullName.StartsWith("Template.", StringComparison.Ordinal) ? fullName[9..] : fullName;

TypeReference MapType(TypeReference t, MethodDefinition? targetMethod = null)
{
    if (t is GenericParameter gp)
    {
        if (targetMethod != null && gp.Type == GenericParameterType.Method && gp.Position < targetMethod.GenericParameters.Count)
            return targetMethod.GenericParameters[gp.Position];
        throw new InvalidDataException($"Unsupported generic parameter {gp.FullName}");
    }
    if (t is ByReferenceType br) return new ByReferenceType(MapType(br.ElementType, targetMethod));
    if (t is PointerType pt) return new PointerType(MapType(pt.ElementType, targetMethod));
    if (t is ArrayType at) return new ArrayType(MapType(at.ElementType, targetMethod), at.Rank);
    if (t is GenericInstanceType git)
    {
        var x = new GenericInstanceType(MapType(git.ElementType, targetMethod));
        foreach (var a in git.GenericArguments) x.GenericArguments.Add(MapType(a, targetMethod));
        return x;
    }
    if (t is OptionalModifierType omt) return new OptionalModifierType(MapType(omt.ModifierType, targetMethod), MapType(omt.ElementType, targetMethod));
    if (t is RequiredModifierType rmt) return new RequiredModifierType(MapType(rmt.ModifierType, targetMethod), MapType(rmt.ElementType, targetMethod));
    if (t is PinnedType pin) return new PinnedType(MapType(pin.ElementType, targetMethod));

    var name = TargetName(t.FullName);
    if (targetDefs.TryGetValue(name, out var def)) return def;
    if (targetTypeRefs.TryGetValue(name, out var tr)) return tr;
    return name switch
    {
        "System.Void" => target.TypeSystem.Void,
        "System.Boolean" => target.TypeSystem.Boolean,
        "System.Byte" => target.TypeSystem.Byte,
        "System.SByte" => target.TypeSystem.SByte,
        "System.Int16" => target.TypeSystem.Int16,
        "System.UInt16" => target.TypeSystem.UInt16,
        "System.Int32" => target.TypeSystem.Int32,
        "System.UInt32" => target.TypeSystem.UInt32,
        "System.Int64" => target.TypeSystem.Int64,
        "System.UInt64" => target.TypeSystem.UInt64,
        "System.Single" => target.TypeSystem.Single,
        "System.Double" => target.TypeSystem.Double,
        "System.Char" => target.TypeSystem.Char,
        "System.String" => target.TypeSystem.String,
        "System.Object" => target.TypeSystem.Object,
        _ => throw new InvalidDataException($"HF22 cannot map type {t.FullName} -> {name}")
    };
}

string Sig(TypeReference t) => TargetName(t.FullName);

FieldReference MapField(FieldReference f, MethodDefinition targetMethod)
{
    var declName = TargetName(f.DeclaringType.FullName);
    if (!targetDefs.TryGetValue(declName, out var td)) throw new InvalidDataException($"HF22 field declaring type missing {declName}");
    var q = td.Fields.Where(x => x.Name == f.Name).ToList();
    if (q.Count != 1) throw new InvalidDataException($"HF22 field {declName}.{f.Name} count={q.Count}");
    return q[0];
}

MethodReference ConstructMethod(MethodReference m, MethodDefinition targetMethod)
{
    var decl = MapType(m.DeclaringType, targetMethod);
    var ret = MapType(m.ReturnType, targetMethod);
    var x = new MethodReference(m.Name, ret, decl)
    {
        HasThis = m.HasThis,
        ExplicitThis = m.ExplicitThis,
        CallingConvention = m.CallingConvention
    };
    for (int i = 0; i < m.GenericParameters.Count; i++) x.GenericParameters.Add(new GenericParameter(m.GenericParameters[i].Name, x));
    foreach (var p in m.Parameters) x.Parameters.Add(new ParameterDefinition(MapType(p.ParameterType, targetMethod)));
    return x;
}

MethodReference MapMethod(MethodReference m, MethodDefinition targetMethod)
{
    if (m is GenericInstanceMethod gim)
    {
        var e = MapMethod(gim.ElementMethod, targetMethod);
        var g = new GenericInstanceMethod(e);
        foreach (var a in gim.GenericArguments) g.GenericArguments.Add(MapType(a, targetMethod));
        return g;
    }

    var declName = TargetName(m.DeclaringType.FullName);
    var wantParams = m.Parameters.Select(p => Sig(MapType(p.ParameterType, targetMethod))).ToArray();
    if (targetDefs.TryGetValue(declName, out var td))
    {
        var q = td.Methods.Where(x => x.Name == m.Name && x.Parameters.Count == wantParams.Length)
            .Where(x => x.Parameters.Select(p => Sig(p.ParameterType)).SequenceEqual(wantParams)).ToList();
        if (q.Count == 1) return q[0];
        if (q.Count > 1)
        {
            var retName = Sig(MapType(m.ReturnType, targetMethod));
            var r = q.Where(x => Sig(x.ReturnType) == retName).ToList();
            if (r.Count == 1) return r[0];
        }
        throw new InvalidDataException($"HF22 custom MethodRef {declName}::{m.Name}({string.Join(',', wantParams)}) count={q.Count}");
    }

    var ext = targetMethodRefs.Where(x => x.DeclaringType.FullName == declName && x.Name == m.Name && x.Parameters.Count == wantParams.Length)
        .Where(x => x.Parameters.Select(p => Sig(p.ParameterType)).SequenceEqual(wantParams)).ToList();
    if (ext.Count > 0) return ext[0];
    return ConstructMethod(m, targetMethod);
}

Instruction NewInstruction(Instruction old, MethodDefinition targetMethod, Dictionary<VariableDefinition,VariableDefinition> vars)
{
    var op = old.OpCode;
    var v = old.Operand;
    if (v == null) return Instruction.Create(op);
    return v switch
    {
        sbyte x => Instruction.Create(op, x),
        byte x => Instruction.Create(op, (sbyte)x),
        int x => Instruction.Create(op, x),
        long x => Instruction.Create(op, x),
        float x => Instruction.Create(op, x),
        double x => Instruction.Create(op, x),
        string x => Instruction.Create(op, x),
        FieldReference x => Instruction.Create(op, MapField(x, targetMethod)),
        MethodReference x => Instruction.Create(op, MapMethod(x, targetMethod)),
        TypeReference x => Instruction.Create(op, MapType(x, targetMethod)),
        VariableDefinition x => Instruction.Create(op, vars[x]),
        ParameterDefinition x => Instruction.Create(op, targetMethod.Parameters[x.Index]),
        Instruction x => Instruction.Create(op, x),
        Instruction[] x => Instruction.Create(op, x),
        CallSite x => throw new NotSupportedException($"HF22 CallSite operand {x}"),
        _ => throw new NotSupportedException($"HF22 operand {v.GetType().FullName} at {old}")
    };
}

void CloneBody(MethodDefinition src, MethodDefinition dst)
{
    if (!src.HasBody) throw new InvalidDataException($"Template missing body {src.FullName}");
    var sb = src.Body;
    var db = new Mono.Cecil.Cil.MethodBody(dst) { InitLocals = sb.InitLocals, MaxStackSize = Math.Max(sb.MaxStackSize, 8) };
    dst.Body = db;
    var vars = new Dictionary<VariableDefinition,VariableDefinition>();
    foreach (var v in sb.Variables)
    {
        var nv = new VariableDefinition(MapType(v.VariableType, dst));
        db.Variables.Add(nv); vars[v] = nv;
    }
    var imap = new Dictionary<Instruction,Instruction>();
    foreach (var i in sb.Instructions)
    {
        var ni = NewInstruction(i, dst, vars);
        db.Instructions.Add(ni); imap[i] = ni;
    }
    foreach (var i in sb.Instructions)
    {
        var ni = imap[i];
        if (i.Operand is Instruction b) ni.Operand = imap[b];
        else if (i.Operand is Instruction[] sw) ni.Operand = sw.Select(x => imap[x]).ToArray();
    }
    foreach (var eh in sb.ExceptionHandlers)
    {
        db.ExceptionHandlers.Add(new ExceptionHandler(eh.HandlerType)
        {
            CatchType = eh.CatchType == null ? null : MapType(eh.CatchType, dst),
            TryStart = eh.TryStart == null ? null : imap[eh.TryStart],
            TryEnd = eh.TryEnd == null ? null : imap[eh.TryEnd],
            HandlerStart = eh.HandlerStart == null ? null : imap[eh.HandlerStart],
            HandlerEnd = eh.HandlerEnd == null ? null : imap[eh.HandlerEnd],
            FilterStart = eh.FilterStart == null ? null : imap[eh.FilterStart]
        });
    }
}

var pairs = new (uint token, string type, string method, int parameters)[]
{
    (0x0600010C, "Damage", "AreaDamage", 0),
    (0x0600010D, "Damage", "AreaDamage_Device", 1),
    (0x0600010E, "Damage", "AreaDamage_Plant", 0),
    (0x0600010F, "Damage", "AreaDamage_Zombie", 0),
    (0x06000124, "ElementManager", "ToEffect", 1),
    (0x06000127, "ElementManager", "GetElement", 1),
    (0x06000310, "Device", "CanAttacked", 1),
    (0x06000336, "Device", "TakeDamage", 2),
    (0x060003B3, "Plant", "TakeDamage", 2),
    (0x06000431, "Zombie", "CanAttacked", 0),
    (0x06000441, "Zombie", "GetATK", 0),
    (0x0600047F, "Zombie", "TakeDamage", 2),
};

var templateTypes = AllTypes(template.Types).ToDictionary(t => t.FullName, StringComparer.Ordinal);
foreach (var p in pairs)
{
    var dst = target.LookupToken(new MetadataToken(TokenType.Method, (int)(p.token & 0x00FFFFFF))) as MethodDefinition
        ?? throw new InvalidDataException($"HF22 target token missing 0x{p.token:X8}");
    if (dst.DeclaringType.FullName != p.type || dst.Name != p.method || dst.Parameters.Count != p.parameters)
        throw new InvalidDataException($"HF22 token drift 0x{p.token:X8}: {dst.FullName}");
    var st = templateTypes[$"Template.{p.type}"];
    var srcs = st.Methods.Where(m => m.Name == p.method && m.Parameters.Count == p.parameters).ToList();
    if (srcs.Count != 1) throw new InvalidDataException($"HF22 template {p.type}.{p.method}/{p.parameters} count={srcs.Count}");
    CloneBody(srcs[0], dst);
    Console.WriteLine($"PATCH 0x{p.token:X8} {p.type}.{p.method}/{p.parameters} il={dst.Body.Instructions.Count} bytes={dst.Body.CodeSize} eh={dst.Body.ExceptionHandlers.Count}");
}

target.Write(output);

using (var reopen = ModuleDefinition.ReadModule(output, new ReaderParameters { InMemory = true, ReadSymbols = false }))
{
    foreach (var p in pairs)
    {
        var m = reopen.LookupToken(new MetadataToken(TokenType.Method, (int)(p.token & 0x00FFFFFF))) as MethodDefinition
            ?? throw new InvalidDataException($"HF22 reopen missing 0x{p.token:X8}");
        if (!m.HasBody || m.Body.Instructions.Count < 5) throw new InvalidDataException($"HF22 reopen invalid {m.FullName}");
        var helper = m.Body.Instructions.Any(i => i.Operand is MethodReference mr && mr.DeclaringType.FullName.Contains("Cpp2IL", StringComparison.Ordinal));
        if (helper) throw new InvalidDataException($"HF22 Cpp2IL helper remained in {m.FullName}");
        Console.WriteLine($"REOPEN 0x{p.token:X8} il={m.Body.Instructions.Count} bytes={m.Body.CodeSize} eh={m.Body.ExceptionHandlers.Count}");
    }
}
return 0;

namespace UnityEngine
{
    public class Object
    {
        public static bool operator ==(Object? a, Object? b) => ReferenceEquals(a,b);
        public static bool operator !=(Object? a, Object? b) => !ReferenceEquals(a,b);
        public static implicit operator bool(Object? a) => a != null;
        public override bool Equals(object? obj) => base.Equals(obj);
        public override int GetHashCode() => base.GetHashCode();
    }
    public class GameObject : Object { public bool activeSelf { get; set; } }
    public class Component : Object { public GameObject gameObject { get; } = null!; }
    public class MonoBehaviour : Component { }
    public static class Debug { public static void Log(object message) { } }
}

namespace Template
{
    public enum Camp { none, plant, zombie }
    public enum DamageAttribute { normal, throughout, artillery, ashes, real }
    public enum ElementType { fire_ice }

    public class Buff { }
    public class Projectile { }
    public class AttackRange { public bool TestInRange<T>(T target) => false; }
    public class Board : UnityEngine.MonoBehaviour { public PlantManager plantManager=null!; public ZombieManager zombieManager=null!; public DeviceManager deviceManager=null!; }
    public class PlantManager { public List<Plant> plants=null!; }
    public class ZombieManager { public List<Zombie> zombieList=null!; }
    public class DeviceManager { public List<Device> deviceList=null!; }
    public class BuffManager { public float GetIncrement(float value,string name)=>0; public void AddBuff(Buff buff){} }

    public class Element { public ElementType type; public float point; }
    public class ElementManager
    {
        public List<Element> elements=null!;
        public List<Element> elements_toEffect=null!;
        public void ToEffect(Element element) => elements_toEffect.Add(element);
        public Element? GetElement(ElementType elementType)
        {
            foreach (var element in elements)
            {
                if (element == null) throw new NullReferenceException();
                if (element.type == elementType) return element;
            }
            return null;
        }
    }

    public class Damage
    {
        public float damagePoint;
        public Camp camp;
        public DamageAttribute damageAttribute;
        public AttackRange attackRange=null!;
        public List<Element> elements=null!;
        public Plant sourcePlant=null!;
        public Zombie sourceZombie=null!;
        public Device sourceDevice=null!;
        public Board board=null!;
        public List<Buff> buffs_additional=null!;
        public float stiffnessTime;

        public bool AreaDamage()
        {
            if (attackRange == null) return false;
            UnityEngine.Debug.Log("进行范围伤害，伤害类型为" + damageAttribute.ToString());
            if (!board)
            {
                UnityEngine.Debug.Log("Board不存在，尝试获取Board");
                if (sourcePlant) board = sourcePlant.board;
                if (sourceZombie) board = sourceZombie.board;
                if (sourceDevice) board = sourceDevice.board;
                if (!board)
                {
                    UnityEngine.Debug.Log("Board获取失败");
                    return false;
                }
            }
            return camp switch
            {
                Camp.none => AreaDamage_Zombie() | AreaDamage_Plant() | AreaDamage_Device(camp),
                Camp.plant => AreaDamage_Zombie() | AreaDamage_Device(camp),
                Camp.zombie => AreaDamage_Plant() | AreaDamage_Device(camp),
                _ => false,
            };
        }

        private bool AreaDamage_Device(Camp camp)
        {
            var hits = new List<Device>();
            foreach (var device in board.deviceManager.deviceList)
            {
                if (device == null) continue;
                if (camp != Camp.none && device.camp == camp) continue;
                if (!device.CanAttacked(this)) continue;
                if (!attackRange.TestInRange(device)) continue;
                hits.Add(device);
            }
            bool result = hits.Count > 0;
            foreach (var device in hits)
            {
                float old = damagePoint;
                if (device == null) throw new NullReferenceException();
                device.TakeDamage(this, null!);
                damagePoint = old;
            }
            return result;
        }

        private bool AreaDamage_Plant()
        {
            var hits = new List<Plant>();
            foreach (var plant in board.plantManager.plants)
            {
                if (plant == null) throw new NullReferenceException();
                if (!plant.CanAttacked()) continue;
                if (!attackRange.TestInRange(plant)) continue;
                hits.Add(plant);
            }
            bool result = hits.Count > 0;
            foreach (var plant in hits)
            {
                float old = damagePoint;
                if (plant == null) throw new NullReferenceException();
                plant.TakeDamage(this, null!);
                damagePoint = old;
            }
            return result;
        }

        private bool AreaDamage_Zombie()
        {
            var hits = new List<Zombie>();
            foreach (var zombie in board.zombieManager.zombieList)
            {
                if (zombie == null) throw new NullReferenceException();
                if (!zombie.CanAttacked()) continue;
                if (!attackRange.TestInRange(zombie)) continue;
                hits.Add(zombie);
            }
            bool result = hits.Count > 0;
            foreach (var zombie in hits)
            {
                float old = damagePoint;
                if (zombie == null) throw new NullReferenceException();
                zombie.TakeDamage(this, null!);
                damagePoint = old;
            }
            return result;
        }
    }

    public class Device : UnityEngine.MonoBehaviour
    {
        public Board board=null!;
        public Camp camp;
        public int ID;
        public float healthPoint;
        public float defensePoint;
        public int brokenLevel;
        public float icePoint;
        public float maxIcePoint;
        public bool broken;
        public Zombie targetZombie=null!;
        public float brightEffectTime;
        public float brightTime;
        public float brightIntensity;
        public ICEUIController iceUIController=null!;
        public HPUIController hpUIController=null!;

        public class ICEUIController { public void Update(float icePoint,float maxIcePoint,Device device,bool destructive){} }
        public class HPUIController { public void RecordingHP(Device device){} }

        public bool CanAttacked(Damage damage)
        {
            if (ID == 2 || ID == 7 || ID == 8 || ID == 12 || ID == 14) return false;
            if (ID == 11)
            {
                if (damage == null) throw new NullReferenceException();
                return damage.damageAttribute == DamageAttribute.ashes;
            }
            return !broken;
        }

        public void TakeDamage(Damage damage, Projectile projectile)
        {
            if (broken || ID == 2 || ID == 7 || ID == 8 || ID == 12 || ID == 14) return;
            if (ID == 11)
            {
                if (damage == null) throw new NullReferenceException();
                if (damage.damageAttribute != DamageAttribute.ashes) return;
            }
            brightEffectTime = 0.2f;
            brightTime = 0.2f;
            brightIntensity = 4f;
            if (damage == null) throw new NullReferenceException();
            float point = damage.damagePoint;
            if (icePoint > 0f)
            {
                if (damage.elements == null) throw new NullReferenceException();
                foreach (var element in damage.elements)
                {
                    if (element == null) throw new NullReferenceException();
                    if (element.type != ElementType.fire_ice) continue;
                    icePoint = Math.Min(Math.Max(icePoint + element.point * 0.2f, 0f), maxIcePoint);
                }
                if (icePoint < point)
                {
                    point -= icePoint;
                    icePoint = 0f;
                    if (iceUIController == null) throw new NullReferenceException();
                    iceUIController.Update(icePoint, maxIcePoint, this, true);
                }
                else
                {
                    icePoint -= point;
                    if (iceUIController == null) throw new NullReferenceException();
                    iceUIController.Update(icePoint, maxIcePoint, this, true);
                    if (hpUIController == null) throw new NullReferenceException();
                    hpUIController.RecordingHP(this);
                    return;
                }
            }
            float reduction = Math.Min(Math.Max(1f - ((point * 3f - defensePoint * 2f) / (defensePoint * 2f + point * 2f)), 0f), 0.9f);
            float actual = (1f - reduction) * point;
            if (ID == 13 && targetZombie && targetZombie.brokenLevel >= 3)
            {
                if (board && targetZombie) targetZombie.healthPoint -= actual * 0.3f;
                actual = 0f;
            }
            healthPoint -= actual;
            if (hpUIController == null) throw new NullReferenceException();
            hpUIController.RecordingHP(this);
            TakeDamage_AudioParticle(damage, projectile);
            InjuryStatusUpdate();
        }
        private void TakeDamage_AudioParticle(Damage damage, Projectile projectile) { }
        private void InjuryStatusUpdate() { }
    }

    public class Plant : UnityEngine.MonoBehaviour
    {
        public Board board=null!;
        public int ID;
        public int order;
        public float healthPoint;
        public bool isDied;
        public bool skillOngoing;
        private float defensePoint;
        public float flash_Brightness;
        public float flash_Time;
        public ElementManager elementManager=null!;
        public BuffManager buffManager=null!;

        public bool CanAttacked() => !isDied;
        public void KillEvent(Zombie zombie) { }
        public void TakeDamage(Damage damage, Projectile projectile)
        {
            float def = Math.Max(MathF.Round(buffManager.GetIncrement(defensePoint, "Def") + defensePoint), 0f);
            float reduction = Math.Min(Math.Max(1f - ((damage.damagePoint * 3f - def * 2f) / (damage.damagePoint * 2f + def * 2f)), 0f), 0.9f);
            float actual = (1f - reduction) * damage.damagePoint;
            if (ID == 49 && !skillOngoing && order > 0) actual *= 0.7f;
            healthPoint -= actual;
            flash_Time = 0.2f;
            flash_Brightness = 4f;
            foreach (var element in damage.elements)
            {
                if (elementManager == null) throw new NullReferenceException();
                elementManager.ToEffect(element);
            }
        }
    }

    public class HPUIController_Zombie : UnityEngine.MonoBehaviour { public void RecordingHP(){} }

    public class Zombie : UnityEngine.MonoBehaviour
    {
        public Board board=null!;
        public int ID;
        private float attackPoint;
        public float healthPoint;
        public int brokenLevel;
        public bool isDied;
        public bool ashes;
        public bool invincible;
        public bool poleZombie_jump;
        public bool immune_stiff;
        public ElementManager elementManager=null!;
        public Plant targetPlant=null!;
        public float stiffnessTime;
        public HPUIController_Zombie hpUIController=null!;
        public BuffManager buffManager=null!;

        public bool CanAttacked()
        {
            var go = gameObject;
            if (go == null) throw new NullReferenceException();
            if (!go.activeSelf || invincible || isDied || ashes) return false;
            if (ID == 3 || ID == 23) return !poleZombie_jump;
            return ID != 13;
        }
        public float GetATK() => Math.Max(MathF.Round(buffManager.GetIncrement(attackPoint, "Atk") + attackPoint), 0f);
        public bool IsDisabled()=>false;
        private bool Hurt_Normal(Damage d,Projectile p)=>false;
        private bool Hurt_Throughout(Damage d,Projectile p)=>false;
        private bool Hurt_Artillery(Damage d,Projectile p)=>false;
        private bool Hurt_Ashes(Damage d,Projectile p)=>false;
        private bool Hurt_Real(Damage d,Projectile p)=>false;
        public void ZC_ArmoredFlagWakeUpZombies(){}

        public void TakeDamage(Damage damage, Projectile projectile)
        {
            if (damage == null) throw new NullReferenceException();
            if (damage.buffs_additional != null)
            {
                foreach (var buff in damage.buffs_additional)
                {
                    if (buffManager == null) throw new NullReferenceException();
                    buffManager.AddBuff(buff);
                }
            }
            if (damage.stiffnessTime > 0f)
            {
                if (!immune_stiff)
                {
                    if (stiffnessTime <= damage.stiffnessTime) stiffnessTime = damage.stiffnessTime;
                }
                else stiffnessTime = 0f;
            }
            var go = gameObject;
            if (go == null) throw new NullReferenceException();
            if (!go.activeSelf || invincible || isDied || ashes) return;
            if (ID == 3 || ID == 23)
            {
                if (poleZombie_jump) return;
            }
            else if (ID == 13) return;

            bool wasDisabled = IsDisabled();
            bool hurt = damage.damageAttribute switch
            {
                DamageAttribute.normal => Hurt_Normal(damage, projectile),
                DamageAttribute.throughout => Hurt_Throughout(damage, projectile),
                DamageAttribute.artillery => Hurt_Artillery(damage, projectile),
                DamageAttribute.ashes => Hurt_Ashes(damage, projectile),
                DamageAttribute.real => Hurt_Real(damage, projectile),
                _ => false,
            };

            if (damage.elements == null) throw new NullReferenceException();
            foreach (var element in damage.elements)
            {
                if (element == null) throw new NullReferenceException();
                if (element.type == ElementType.fire_ice) continue;
                if (elementManager == null) throw new NullReferenceException();
                elementManager.ToEffect(element);
            }
            if (hurt && !wasDisabled)
            {
                if (damage.sourcePlant) damage.sourcePlant.KillEvent(this);
            }
            else if (ID == 10)
            {
                ZC_ArmoredFlagWakeUpZombies();
            }
            if (!hpUIController) return;
            hpUIController.RecordingHP();
        }
    }
}
