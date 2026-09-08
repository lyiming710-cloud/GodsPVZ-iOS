using Mono.Cecil;
using Mono.Cecil.Cil;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

if (args.Length != 2)
{
    Console.Error.WriteLine("Usage: GodsPVZ.HF21Patch <input.dll> <output.dll>");
    return 2;
}

var input = Path.GetFullPath(args[0]);
var output = Path.GetFullPath(args[1]);
using var module = ModuleDefinition.ReadModule(input, new ReaderParameters { InMemory = true, ReadSymbols = false });

IEnumerable<TypeDefinition> All(IEnumerable<TypeDefinition> roots)
{
    foreach (var t in roots)
    {
        yield return t;
        foreach (var n in All(t.NestedTypes)) yield return n;
    }
}
var types = All(module.Types).ToList();
TypeDefinition T(string n)
{
    var q = types.Where(t => t.FullName == n || t.Name == n).ToList();
    if (q.Count != 1) throw new InvalidDataException($"Expected one type {n}, got {q.Count}");
    return q[0];
}
FieldDefinition F(TypeDefinition t, string n)
{
    var q = t.Fields.Where(f => f.Name == n).ToList();
    if (q.Count != 1) throw new InvalidDataException($"Expected one field {t.FullName}.{n}, got {q.Count}");
    return q[0];
}
MethodDefinition M(TypeDefinition t, string n, int pc)
{
    var q = t.Methods.Where(m => m.Name == n && m.Parameters.Count == pc).ToList();
    if (q.Count != 1) throw new InvalidDataException($"Expected one method {t.FullName}.{n}/{pc}, got {q.Count}");
    return q[0];
}
var refs = types.SelectMany(t => t.Methods).Where(m => m.HasBody)
    .SelectMany(m => m.Body.Instructions).Select(i => i.Operand).OfType<MethodReference>().ToList();

MethodReference R(string decl, string name, params string[] ps)
{
    var q = refs.Where(r => r.DeclaringType.FullName == decl && r.Name == name && r.Parameters.Count == ps.Length)
        .Where(r => r.Parameters.Select(p => p.ParameterType.FullName).SequenceEqual(ps)).ToList();
    if (q.Count == 0) throw new InvalidDataException($"Missing MethodRef {decl}::{name}({string.Join(",", ps)})");
    return q[0];
}
MethodReference RCount(string decl, string name, int pc)
{
    var q = refs.Where(r => r.DeclaringType.FullName == decl && r.Name == name && r.Parameters.Count == pc).ToList();
    if (q.Count == 0) throw new InvalidDataException($"Missing MethodRef {decl}::{name}/{pc}");
    return q[0];
}
MethodReference MR(string name, TypeReference ret, TypeReference decl, params TypeReference[] ps)
{
    var r = new MethodReference(name, ret, decl) { HasThis = true };
    foreach (var p in ps) r.Parameters.Add(new ParameterDefinition(p));
    return r;
}
GenericInstanceMethod G(MethodReference m, TypeReference arg)
{
    if (m is GenericInstanceMethod gim) m = gim.ElementMethod;
    var g = new GenericInstanceMethod(m);
    g.GenericArguments.Add(arg);
    return g;
}
void E(ILProcessor il, OpCode op, object? value = null)
{
    Instruction ins = value switch
    {
        null => Instruction.Create(op),
        int i => Instruction.Create(op, i),
        float f => Instruction.Create(op, f),
        string s => Instruction.Create(op, s),
        MethodReference mr => Instruction.Create(op, mr),
        FieldReference fr => Instruction.Create(op, fr),
        TypeReference tr => Instruction.Create(op, tr),
        VariableDefinition vd => Instruction.Create(op, vd),
        ParameterDefinition pd => Instruction.Create(op, pd),
        Instruction target => Instruction.Create(op, target),
        _ => throw new NotSupportedException(value.GetType().FullName)
    };
    il.Append(ins);
}
MethodBody B(MethodDefinition m, bool locals = true, int stack = 8)
{
    var b = new MethodBody(m) { InitLocals = locals, MaxStackSize = stack };
    m.Body = b;
    return b;
}
void ThrowNre(ILProcessor il, MethodReference ctor)
{
    E(il, OpCodes.Newobj, ctor);
    E(il, OpCodes.Throw);
}

var projectileManager = T("ProjectileManager");
var projectile = T("Projectile");
var particlesManager = T("ParticlesManager");
var particleState = T("ParticleState");
var resourceManager = types.Single(t => t.Name == "ResourceManager" && t.Namespace == "");
var projectileType = T("ProjectileType");
var internalLoader = T("InternalResourceLoader");
var board = T("Board");

var pmStart = M(projectileManager, "Start", 0);
var pmSetFloat = M(projectileManager, "SetFloatScale", 0);
var pmCreate = M(projectileManager, "CrateNewProjectile", 1);
var pReset = M(projectile, "ResetData", 0);
var pBind = M(projectile, "BindTrack", 0);
var particlesStart = M(particlesManager, "Start", 0);
var particlesCreate = M(particlesManager, "CreatNewParticle", 1);
var loadProjectileSprite = M(resourceManager, "Load_projectileSprite", 0);

var expected = new Dictionary<MethodDefinition,uint>
{
    [pmStart] = 0x060001F3,
    [pmSetFloat] = 0x060001F4,
    [pmCreate] = 0x060001F6,
    [particlesStart] = 0x060001E8,
    [particlesCreate] = 0x060001EA,
    [loadProjectileSprite] = 0x0600022E,
    [pReset] = 0x060003DD,
    [pBind] = 0x060003DE,
};
foreach (var kv in expected)
    if (kv.Key.MetadataToken.ToUInt32() != kv.Value)
        throw new InvalidDataException($"HF21 token drift {kv.Key.FullName}: 0x{kv.Key.MetadataToken.ToUInt32():X8}");

var pmProjectilePrefab = F(projectileManager, "projectilePrefab");
var pmProjectiles = F(projectileManager, "projectiles");
var pmProjectileSprites = F(projectileManager, "projectileSprites");
var pmProjectileSpritesSpecial = F(projectileManager, "projectileSprites_Special");
var pmProjectileAnimations = F(projectileManager, "projectileAnimations");
var pmBoard = F(projectileManager, "board");
var pmFW = F(projectileManager, "fW");
var pmFD = F(projectileManager, "fD");
var pmFH = F(projectileManager, "fH");

var pID = F(projectile, "ID");
var pCamp = F(projectile, "camp");
var pDamage = F(projectile, "damage");
var pScale = F(projectile, "scale");
var pLivingTime = F(projectile, "livingTime");
var pUpdateRate = F(projectile, "updateRate");
var pFX = F(projectile, "fX");
var pFY = F(projectile, "fY");
var pFZ = F(projectile, "fZ");
var pFZShadow = F(projectile, "fZ_shadow");
var pFW = F(projectile, "fW");
var pFD = F(projectile, "fD");
var pFH = F(projectile, "fH");
var pMovementTracks = F(projectile, "movementTracks");
var pHitType = F(projectile, "hitType");
var pSpeed = F(projectile, "speed");
var pAcceleration = F(projectile, "acceleration");
var pZSpeed = F(projectile, "zSpeed");
var pZAcceleration = F(projectile, "zAcceleration");
var pAngularSpeed = F(projectile, "angularSpeed");
var pAngularAcceleration = F(projectile, "angularAcceleration");
var pProjectileSprite = F(projectile, "projectileSprite");
var pProjectileAnimation = F(projectile, "projectileAnimation");
var pTrack = F(projectile, "track");
var pBoard = F(projectile, "board");
var pProjectileManager = F(projectile, "projectileManager");

var partDict = F(particlesManager, "ParticleDictionary");
var partInstance = F(particlesManager, "<instance>k__BackingField");
var partNewParticle = F(particlesManager, "newParticleSystem");

var rmProjectileSprites = F(resourceManager, "projectileSprites");
var rmProjectileSpritesSpecial = F(resourceManager, "projectileSprites_Special");
var rmProjectileAnimations = F(resourceManager, "projectileAnimations");
var rmPrefabProjectile = F(resourceManager, "prefab_Projectile");

var boardProjectileManager = F(board, "projectileManager");

var startData = M(projectile, "StartData", 1);

var nreCtor = R("System.NullReferenceException", ".ctor");
var getTransform = R("UnityEngine.Component", "get_transform");
var gameGetTransform = R("UnityEngine.GameObject", "get_transform");
var getGameObject = R("UnityEngine.Component", "get_gameObject");
var setParent = R("UnityEngine.Transform", "SetParent", "UnityEngine.Transform", "System.Boolean");
var gameSetActive = R("UnityEngine.GameObject", "SetActive", "System.Boolean");
var gameGetActiveSelf = R("UnityEngine.GameObject", "get_activeSelf");
var transformGetPosition = R("UnityEngine.Transform", "get_position");
var transformSetPosition = R("UnityEngine.Transform", "set_position", "UnityEngine.Vector3");
var transformSetLocalEuler = R("UnityEngine.Transform", "set_localEulerAngles", "UnityEngine.Vector3");
var objectImplicit = R("UnityEngine.Object", "op_Implicit", "UnityEngine.Object");
var objectInequality = R("UnityEngine.Object", "op_Inequality", "UnityEngine.Object", "UnityEngine.Object");
var objectDestroy = R("UnityEngine.Object", "Destroy", "UnityEngine.Object");
var debugLog = R("UnityEngine.Debug", "Log", "System.Object");
var monoPrint = R("UnityEngine.MonoBehaviour", "print", "System.Object");

var typeGetFromHandle = R("System.Type", "GetTypeFromHandle", "System.RuntimeTypeHandle");
var enumGetValues = R("System.Enum", "GetValues", "System.Type");
var enumGetName = R("System.Enum", "GetName", "System.Type", "System.Object");
var pathCombine2 = R("System.IO.Path", "Combine", "System.String", "System.String");
var pathCombine3 = R("System.IO.Path", "Combine", "System.String", "System.String", "System.String");
var concat3 = R("System.String", "Concat", "System.String", "System.String", "System.String");

var iEnumeratorType = new TypeReference("System.Collections", "IEnumerator", module, module.TypeSystem.CoreLibrary, false);
var iDisposableType = new TypeReference("System", "IDisposable", module, module.TypeSystem.CoreLibrary, false);
var arrayType = enumGetValues.ReturnType;
var arrayGetEnumerator = new MethodReference("GetEnumerator", iEnumeratorType, arrayType) { HasThis = true };
var enumMoveNext = new MethodReference("MoveNext", module.TypeSystem.Boolean, iEnumeratorType) { HasThis = true };
var enumCurrent = new MethodReference("get_Current", module.TypeSystem.Object, iEnumeratorType) { HasThis = true };
var disposeInterface = new MethodReference("Dispose", module.TypeSystem.Void, iDisposableType) { HasThis = true };

var projectileList = (GenericInstanceType)pmProjectiles.FieldType;
var projectileListAdd = MR("Add", module.TypeSystem.Void, projectileList, projectile);
var objectEnumMoveNext = refs.FirstOrDefault(r => r.Name == "MoveNext" && r.DeclaringType.FullName.Contains("System.Collections.Generic.List`1/Enumerator<System.Object>", StringComparison.Ordinal));
if (objectEnumMoveNext == null || objectEnumMoveNext.DeclaringType is not GenericInstanceType objectEnumType)
    throw new InvalidDataException("HF21 missing List<object>.Enumerator reference template");
var projectileEnumType = new GenericInstanceType(objectEnumType.ElementType);
projectileEnumType.GenericArguments.Add(projectile);
var projectileGetEnum = MR("GetEnumerator", projectileEnumType, projectileList);
var projectileEnumMoveNext = new MethodReference("MoveNext", module.TypeSystem.Boolean, projectileEnumType) { HasThis = true };
var projectileEnumCurrent = new MethodReference("get_Current", projectile, projectileEnumType) { HasThis = true };
var projectileEnumDispose = new MethodReference("Dispose", module.TypeSystem.Void, projectileEnumType) { HasThis = true };

var particleDictType = (GenericInstanceType)partDict.FieldType;
var gameObjectType = particleDictType.GenericArguments[1];
var dictAdd = MR("Add", module.TypeSystem.Void, particleDictType, particleState, gameObjectType);
var dictTryGet = MR("TryGetValue", module.TypeSystem.Boolean, particleDictType, particleState, new ByReferenceType(gameObjectType));

var spriteList = (GenericInstanceType)rmProjectileSprites.FieldType;
var spriteType = spriteList.GenericArguments[0];
var spriteListCount = MR("get_Count", module.TypeSystem.Int32, spriteList);
var spriteListAdd = MR("Add", module.TypeSystem.Void, spriteList, spriteType);
var spriteListSetItem = MR("set_Item", module.TypeSystem.Void, spriteList, module.TypeSystem.Int32, spriteType);

var loadGeneric = internalLoader.Methods.Single(m => m.Name == "Load" && m.HasGenericParameters && m.GenericParameters.Count == 1 && m.Parameters.Count == 1);
var loadGameObject = G(loadGeneric, gameObjectType);
var loadSprite = G(loadGeneric, spriteType);

var instantiateProjectileExisting = refs.OfType<GenericInstanceMethod>()
    .FirstOrDefault(g => g.Name == "Instantiate" && g.GenericArguments.Count == 1 && g.GenericArguments[0].FullName == projectile.FullName && g.Parameters.Count == 1);
if (instantiateProjectileExisting == null) throw new InvalidDataException("HF21 missing Object.Instantiate<Projectile> reference");
var instantiateGameObject = G(instantiateProjectileExisting.ElementMethod, gameObjectType);

var getComponentSpriteRenderer = refs.OfType<GenericInstanceMethod>()
    .FirstOrDefault(g => g.Name == "GetComponent" && g.GenericArguments.Count == 1 && g.GenericArguments[0].Name == "SpriteRenderer" && g.Parameters.Count == 0);
if (getComponentSpriteRenderer == null) throw new InvalidDataException("HF21 missing GetComponent<SpriteRenderer> reference");
var spriteRendererType = getComponentSpriteRenderer.ReturnType;
var setSprite = R("UnityEngine.SpriteRenderer", "set_sprite", "UnityEngine.Sprite");

PatchProjectileManagerSetFloat();
PatchProjectileManagerStart();
PatchProjectileResetData();
PatchProjectileBindTrack();
PatchParticlesStart();
PatchParticlesCreate();
PatchResourceLoadProjectileSprite();
PatchProjectileManagerCreate();

Directory.CreateDirectory(Path.GetDirectoryName(output)!);
module.Write(output, new WriterParameters { WriteSymbols = false });

using (var verify = ModuleDefinition.ReadModule(output, new ReaderParameters { InMemory = true, ReadSymbols = false }))
{
    var vt = All(verify.Types).ToList();
    MethodDefinition VM(string tn, string mn, int pc)
    {
        var t = vt.Where(x => x.Name == tn).Single();
        return t.Methods.Single(m => m.Name == mn && m.Parameters.Count == pc);
    }
    var checks = new[]
    {
        ("ProjectileManager.Start", VM("ProjectileManager","Start",0), 45, 0),
        ("ProjectileManager.SetFloatScale", VM("ProjectileManager","SetFloatScale",0), 200, 0),
        ("ProjectileManager.CrateNewProjectile", VM("ProjectileManager","CrateNewProjectile",1), 35, 1),
        ("Projectile.ResetData", VM("Projectile","ResetData",0), 75, 0),
        ("Projectile.BindTrack", VM("Projectile","BindTrack",0), 35, 0),
        ("ParticlesManager.Start", VM("ParticlesManager","Start",0), 55, 1),
        ("ParticlesManager.CreatNewParticle", VM("ParticlesManager","CreatNewParticle",1), 20, 0),
        ("ResourceManager.Load_projectileSprite", VM("ResourceManager","Load_projectileSprite",0), 65, 1),
    };
    foreach (var (name,m,min,eh) in checks)
    {
        if (!m.HasBody || m.Body.Instructions.Count < min)
            throw new InvalidDataException($"HF21 {name} body too small: {m.Body.Instructions.Count}");
        if (m.Body.ExceptionHandlers.Count != eh)
            throw new InvalidDataException($"HF21 {name} EH mismatch {m.Body.ExceptionHandlers.Count}");
        if (m.Body.Instructions.Select(i => i.Operand).OfType<MethodReference>()
            .Any(r => r.DeclaringType.FullName.Contains("Cpp2ILHelpers", StringComparison.Ordinal)))
            throw new InvalidDataException($"HF21 {name} still contains Cpp2IL helper");
        Console.WriteLine($"VERIFY {name}: token=0x{m.MetadataToken.ToUInt32():X8}, IL={m.Body.Instructions.Count}, bytes={m.Body.CodeSize}, EH={m.Body.ExceptionHandlers.Count}");
    }

    var vs = VM("ProjectileManager","SetFloatScale",0);
    if (vs.Body.Instructions.Count(i => i.OpCode == OpCodes.Stelem_R4) != 54)
        throw new InvalidDataException("HF21 SetFloatScale must contain exactly 54 float-array stores");

    var vr = VM("Projectile","ResetData",0);
    foreach (var c in new[]{1f,65f})
        if (!vr.Body.Instructions.Any(i => i.OpCode == OpCodes.Ldc_R4 && i.Operand is float f && f == c))
            throw new InvalidDataException($"HF21 ResetData missing constant {c}");

    var vb = VM("Projectile","BindTrack",0);
    foreach (var c in new[]{37,38})
        if (!vb.Body.Instructions.Any(i => (i.OpCode == OpCodes.Ldc_I4 || i.OpCode == OpCodes.Ldc_I4_S) && i.Operand is int v && v == c)
            && !vb.Body.Instructions.Any(i => c == 37 ? i.OpCode == OpCodes.Ldc_I4_S && Convert.ToInt32(i.Operand) == 37 : i.OpCode == OpCodes.Ldc_I4_S && Convert.ToInt32(i.Operand) == 38))
            throw new InvalidDataException($"HF21 BindTrack missing ParticleState {c}");

    var vps = VM("ParticlesManager","Start",0);
    foreach (var str in new[]{"prefabs","ParticleSystem","加载特效","失败"})
        if (!vps.Body.Instructions.Any(i => i.OpCode == OpCodes.Ldstr && (string)i.Operand == str))
            throw new InvalidDataException($"HF21 ParticlesManager.Start missing string {str}");

    var vrl = VM("ResourceManager","Load_projectileSprite",0);
    foreach (var str in new[]{"sprites","Projectile","加载子弹","的贴图失败"})
        if (!vrl.Body.Instructions.Any(i => i.OpCode == OpCodes.Ldstr && (string)i.Operand == str))
            throw new InvalidDataException($"HF21 ResourceManager.Load_projectileSprite missing string {str}");
}
Console.WriteLine("HF21 projectile pool/track cluster restored from PC native x86-64");
Console.WriteLine($"WROTE {output} ({new FileInfo(output).Length} bytes)");
return 0;

void PatchProjectileManagerSetFloat()
{
    var b = B(pmSetFloat, false, 4);
    var il = b.GetILProcessor();
    var data = new (FieldDefinition f, int index, float value)[]
    {
        (pmFW, 1, 40.0f),
        (pmFW, 0, 40.0f),
        (pmFD, 1, 40.0f),
        (pmFD, 0, 40.0f),
        (pmFH, 1, 40.0f),
        (pmFH, 0, 40.0f),
        (pmFW, 9, 50.0f),
        (pmFD, 9, 50.0f),
        (pmFH, 9, 50.0f),
        (pmFW, 11, 60.0f),
        (pmFW, 10, 60.0f),
        (pmFD, 11, 60.0f),
        (pmFD, 10, 60.0f),
        (pmFH, 11, 80.0f),
        (pmFH, 10, 80.0f),
        (pmFW, 15, 30.0f),
        (pmFD, 15, 30.0f),
        (pmFH, 15, 100.0f),
        (pmFW, 19, 120.0f),
        (pmFD, 19, 60.0f),
        (pmFH, 19, 60.0f),
        (pmFW, 20, 70.0f),
        (pmFD, 20, 40.0f),
        (pmFH, 20, 40.0f),
        (pmFW, 21, 50.0f),
        (pmFD, 21, 50.0f),
        (pmFH, 21, 140.0f),
        (pmFW, 23, 110.0f),
        (pmFD, 23, 110.0f),
        (pmFH, 23, 60.0f),
        (pmFW, 25, 90.0f),
        (pmFW, 24, 90.0f),
        (pmFD, 25, 30.0f),
        (pmFD, 24, 30.0f),
        (pmFH, 25, 30.0f),
        (pmFH, 24, 30.0f),
        (pmFW, 31, 120.0f),
        (pmFW, 26, 120.0f),
        (pmFD, 31, 50.0f),
        (pmFD, 26, 50.0f),
        (pmFH, 31, 50.0f),
        (pmFH, 26, 50.0f),
        (pmFW, 27, 60.0f),
        (pmFD, 27, 40.0f),
        (pmFH, 27, 40.0f),
        (pmFW, 28, 70.0f),
        (pmFD, 28, 40.0f),
        (pmFH, 28, 40.0f),
        (pmFW, 29, 80.0f),
        (pmFD, 29, 80.0f),
        (pmFH, 29, 300.0f),
        (pmFW, 32, 80.0f),
        (pmFD, 32, 40.0f),
        (pmFH, 32, 40.0f)
    };
    foreach (var (f,index,value) in data)
    {
        E(il, OpCodes.Ldarg_0);
        E(il, OpCodes.Ldfld, f);
        E(il, OpCodes.Ldc_I4, index);
        E(il, OpCodes.Ldc_R4, value);
        E(il, OpCodes.Stelem_R4);
    }
    E(il, OpCodes.Ret);
}

void PatchProjectileManagerStart()
{
    var b = B(pmStart, true, 8);
    var il = b.GetILProcessor();
    var i = new VariableDefinition(module.TypeSystem.Int32);
    var p = new VariableDefinition(projectile);
    b.Variables.Add(i); b.Variables.Add(p);

    E(il, OpCodes.Ldarg_0); E(il, OpCodes.Call, pmSetFloat);

    E(il, OpCodes.Ldarg_0); E(il, OpCodes.Ldsfld, rmPrefabProjectile); E(il, OpCodes.Stfld, pmProjectilePrefab);
    E(il, OpCodes.Ldarg_0); E(il, OpCodes.Call, loadProjectileSprite); E(il, OpCodes.Stfld, pmProjectileSprites);
    E(il, OpCodes.Ldarg_0); E(il, OpCodes.Ldsfld, rmProjectileSpritesSpecial); E(il, OpCodes.Stfld, pmProjectileSpritesSpecial);
    E(il, OpCodes.Ldarg_0); E(il, OpCodes.Ldsfld, rmProjectileAnimations); E(il, OpCodes.Stfld, pmProjectileAnimations);

    E(il, OpCodes.Ldc_I4_0); E(il, OpCodes.Stloc, i);
    var check = Instruction.Create(OpCodes.Nop);
    var body = Instruction.Create(OpCodes.Nop);
    E(il, OpCodes.Br, check);
    il.Append(body);

    E(il, OpCodes.Ldarg_0); E(il, OpCodes.Ldfld, pmProjectilePrefab);
    E(il, OpCodes.Call, instantiateProjectileExisting); E(il, OpCodes.Stloc, p);

    E(il, OpCodes.Ldloc, p); E(il, OpCodes.Ldarg_0); E(il, OpCodes.Ldfld, pmBoard); E(il, OpCodes.Stfld, pBoard);
    E(il, OpCodes.Ldloc, p);
    E(il, OpCodes.Ldarg_0); E(il, OpCodes.Ldfld, pmBoard); E(il, OpCodes.Ldfld, boardProjectileManager);
    E(il, OpCodes.Stfld, pProjectileManager);

    E(il, OpCodes.Ldloc, p); E(il, OpCodes.Call, getTransform);
    E(il, OpCodes.Ldarg_0); E(il, OpCodes.Call, getTransform);
    E(il, OpCodes.Ldc_I4_0); E(il, OpCodes.Callvirt, setParent);

    E(il, OpCodes.Ldloc, p); E(il, OpCodes.Call, getGameObject);
    E(il, OpCodes.Ldc_I4_0); E(il, OpCodes.Callvirt, gameSetActive);

    E(il, OpCodes.Ldarg_0); E(il, OpCodes.Ldfld, pmProjectiles);
    E(il, OpCodes.Ldloc, p); E(il, OpCodes.Callvirt, projectileListAdd);

    E(il, OpCodes.Ldloc, i); E(il, OpCodes.Ldc_I4_1); E(il, OpCodes.Add); E(il, OpCodes.Stloc, i);
    il.Append(check);
    E(il, OpCodes.Ldloc, i); E(il, OpCodes.Ldc_I4, 1000); E(il, OpCodes.Blt, body);
    E(il, OpCodes.Ret);
}

void PatchProjectileManagerCreate()
{
    var b = B(pmCreate, true, 8);
    var il = b.GetILProcessor();
    var en = new VariableDefinition(projectileEnumType);
    var cur = new VariableDefinition(projectile);
    var result = new VariableDefinition(projectile);
    b.Variables.Add(en); b.Variables.Add(cur); b.Variables.Add(result);

    E(il, OpCodes.Ldarg_0); E(il, OpCodes.Ldfld, pmProjectiles); E(il, OpCodes.Callvirt, projectileGetEnum); E(il, OpCodes.Stloc, en);
    var tryStart = Instruction.Create(OpCodes.Nop); il.Append(tryStart);
    var check = Instruction.Create(OpCodes.Ldloca, en);
    E(il, OpCodes.Br, check);
    var loop = Instruction.Create(OpCodes.Ldloca, en); il.Append(loop);
    E(il, OpCodes.Call, projectileEnumCurrent); E(il, OpCodes.Stloc, cur);

    E(il, OpCodes.Ldloc, cur); E(il, OpCodes.Call, getGameObject); E(il, OpCodes.Callvirt, gameGetActiveSelf);
    E(il, OpCodes.Brtrue, check);

    E(il, OpCodes.Ldloc, cur); E(il, OpCodes.Callvirt, pReset);
    E(il, OpCodes.Ldloc, cur); E(il, OpCodes.Ldarg_1); E(il, OpCodes.Callvirt, startData);
    E(il, OpCodes.Ldloc, cur); E(il, OpCodes.Call, getGameObject); E(il, OpCodes.Ldc_I4_1); E(il, OpCodes.Callvirt, gameSetActive);
    E(il, OpCodes.Ldloc, cur); E(il, OpCodes.Stloc, result);
    var returnTarget = Instruction.Create(OpCodes.Ldloc, result);
    E(il, OpCodes.Leave, returnTarget);

    il.Append(check);
    E(il, OpCodes.Call, projectileEnumMoveNext);
    E(il, OpCodes.Brtrue, loop);
    var throwTarget = Instruction.Create(OpCodes.Newobj, nreCtor);
    E(il, OpCodes.Leave, throwTarget);

    var finallyStart = Instruction.Create(OpCodes.Ldloca, en); il.Append(finallyStart);
    E(il, OpCodes.Call, projectileEnumDispose);
    E(il, OpCodes.Endfinally);

    il.Append(throwTarget); E(il, OpCodes.Throw);
    il.Append(returnTarget); E(il, OpCodes.Ret);

    b.ExceptionHandlers.Add(new ExceptionHandler(ExceptionHandlerType.Finally)
    {
        TryStart = tryStart,
        TryEnd = finallyStart,
        HandlerStart = finallyStart,
        HandlerEnd = throwTarget
    });
}

void PatchProjectileResetData()
{
    var b = B(pReset, true, 8);
    var il = b.GetILProcessor();
    var vector3 = pSpeed.FieldType;
    var zero = new VariableDefinition(vector3);
    b.Variables.Add(zero);
    E(il, OpCodes.Ldloca, zero); E(il, OpCodes.Initobj, vector3);

    void SetRef(FieldDefinition f)
    { E(il, OpCodes.Ldarg_0); E(il, OpCodes.Ldnull); E(il, OpCodes.Stfld, f); }
    void SetI(FieldDefinition f, int v)
    { E(il, OpCodes.Ldarg_0); E(il, OpCodes.Ldc_I4, v); E(il, OpCodes.Stfld, f); }
    void SetF(FieldDefinition f, float v)
    { E(il, OpCodes.Ldarg_0); E(il, OpCodes.Ldc_R4, v); E(il, OpCodes.Stfld, f); }
    void SetV(FieldDefinition f)
    { E(il, OpCodes.Ldarg_0); E(il, OpCodes.Ldloc, zero); E(il, OpCodes.Stfld, f); }

    SetRef(pDamage);
    SetI(pID, 0); SetI(pCamp, 0);
    SetF(pScale, 1f); SetF(pLivingTime, 0f);
    SetF(pUpdateRate, 1f); SetF(pFX, 0f); SetF(pFY, 0f);
    SetF(pFZ, 65f); SetF(pFZShadow, 0f);
    SetF(pFW, 0f); SetF(pFD, 0f); SetF(pFH, 0f);
    SetI(pMovementTracks, 0); SetI(pHitType, 0);
    SetV(pSpeed); SetV(pAcceleration);
    SetF(pZSpeed, 0f); SetF(pZAcceleration, 0f);

    E(il, OpCodes.Ldarg_0); E(il, OpCodes.Call, getTransform); E(il, OpCodes.Ldloc, zero); E(il, OpCodes.Callvirt, transformSetLocalEuler);

    var afterSpriteRotation = Instruction.Create(OpCodes.Nop);
    E(il, OpCodes.Ldarg_0); E(il, OpCodes.Ldfld, pProjectileSprite); E(il, OpCodes.Call, objectImplicit); E(il, OpCodes.Brfalse, afterSpriteRotation);
    E(il, OpCodes.Ldarg_0); E(il, OpCodes.Ldfld, pProjectileSprite); E(il, OpCodes.Call, gameGetTransform); E(il, OpCodes.Ldloc, zero); E(il, OpCodes.Callvirt, transformSetLocalEuler);
    il.Append(afterSpriteRotation);

    var afterAnimRotation = Instruction.Create(OpCodes.Nop);
    E(il, OpCodes.Ldarg_0); E(il, OpCodes.Ldfld, pProjectileAnimation); E(il, OpCodes.Call, objectImplicit); E(il, OpCodes.Brfalse, afterAnimRotation);
    E(il, OpCodes.Ldarg_0); E(il, OpCodes.Ldfld, pProjectileAnimation); E(il, OpCodes.Call, gameGetTransform); E(il, OpCodes.Ldloc, zero); E(il, OpCodes.Callvirt, transformSetLocalEuler);
    il.Append(afterAnimRotation);

    SetF(pAngularSpeed, 0f); SetF(pAngularAcceleration, 0f);

    E(il, OpCodes.Ldarg_0); E(il, OpCodes.Ldfld, pProjectileAnimation); E(il, OpCodes.Call, objectDestroy);
    SetRef(pProjectileAnimation);

    E(il, OpCodes.Ldarg_0); E(il, OpCodes.Ldfld, pProjectileSprite);
    E(il, OpCodes.Callvirt, getComponentSpriteRenderer);
    E(il, OpCodes.Ldnull);
    E(il, OpCodes.Callvirt, setSprite);
    E(il, OpCodes.Ret);
}

void PatchProjectileBindTrack()
{
    var b = B(pBind, true, 8);
    var il = b.GetILProcessor();
    var go = new VariableDefinition(gameObjectType);
    var state = new VariableDefinition(particleState);
    b.Variables.Add(go); b.Variables.Add(state);

    var over20 = Instruction.Create(OpCodes.Nop);
    var snow = Instruction.Create(OpCodes.Ldc_I4, 37);
    var bolt = Instruction.Create(OpCodes.Ldc_I4, 38);
    var create = Instruction.Create(OpCodes.Nop);
    var ret = Instruction.Create(OpCodes.Ret);

    E(il, OpCodes.Ldarg_0); E(il, OpCodes.Ldfld, pID); E(il, OpCodes.Ldc_I4, 20); E(il, OpCodes.Bgt, over20);
    E(il, OpCodes.Ldarg_0); E(il, OpCodes.Ldfld, pID); E(il, OpCodes.Ldc_I4_1); E(il, OpCodes.Beq, snow);
    E(il, OpCodes.Ldarg_0); E(il, OpCodes.Ldfld, pID); E(il, OpCodes.Ldc_I4, 19); E(il, OpCodes.Beq, snow);
    E(il, OpCodes.Ldarg_0); E(il, OpCodes.Ldfld, pID); E(il, OpCodes.Ldc_I4, 20); E(il, OpCodes.Beq, snow);
    E(il, OpCodes.Br, ret);

    il.Append(over20);
    E(il, OpCodes.Ldarg_0); E(il, OpCodes.Ldfld, pID); E(il, OpCodes.Ldc_I4, 25); E(il, OpCodes.Beq, bolt);
    E(il, OpCodes.Br, ret);

    il.Append(snow); E(il, OpCodes.Stloc, state); E(il, OpCodes.Br, create);
    il.Append(bolt); E(il, OpCodes.Stloc, state);
    il.Append(create);

    E(il, OpCodes.Ldsfld, partInstance); E(il, OpCodes.Ldloc, state); E(il, OpCodes.Callvirt, particlesCreate); E(il, OpCodes.Stloc, go);

    E(il, OpCodes.Ldloc, go); E(il, OpCodes.Call, gameGetTransform);
    E(il, OpCodes.Ldarg_0); E(il, OpCodes.Ldfld, pProjectileSprite); E(il, OpCodes.Call, gameGetTransform); E(il, OpCodes.Callvirt, transformGetPosition);
    E(il, OpCodes.Callvirt, transformSetPosition);

    E(il, OpCodes.Ldloc, go); E(il, OpCodes.Call, gameGetTransform);
    E(il, OpCodes.Ldarg_0); E(il, OpCodes.Call, getTransform);
    E(il, OpCodes.Ldc_I4_0); E(il, OpCodes.Callvirt, setParent);

    E(il, OpCodes.Ldarg_0); E(il, OpCodes.Ldloc, go); E(il, OpCodes.Stfld, pTrack);
    il.Append(ret);
}

void PatchParticlesStart()
{
    var b = B(particlesStart, true, 10);
    var il = b.GetILProcessor();
    var en = new VariableDefinition(iEnumeratorType);
    var disp = new VariableDefinition(iDisposableType);
    var state = new VariableDefinition(particleState);
    var name = new VariableDefinition(module.TypeSystem.String);
    var path = new VariableDefinition(module.TypeSystem.String);
    var prefab = new VariableDefinition(gameObjectType);
    b.Variables.Add(en); b.Variables.Add(disp); b.Variables.Add(state); b.Variables.Add(name); b.Variables.Add(path); b.Variables.Add(prefab);

    E(il, OpCodes.Ldtoken, particleState); E(il, OpCodes.Call, typeGetFromHandle); E(il, OpCodes.Call, enumGetValues);
    E(il, OpCodes.Callvirt, arrayGetEnumerator); E(il, OpCodes.Stloc, en);

    var tryStart = Instruction.Create(OpCodes.Nop); il.Append(tryStart);
    var check = Instruction.Create(OpCodes.Ldloc, en);
    E(il, OpCodes.Br, check);
    var loop = Instruction.Create(OpCodes.Ldloc, en); il.Append(loop);
    E(il, OpCodes.Callvirt, enumCurrent); E(il, OpCodes.Unbox_Any, particleState); E(il, OpCodes.Stloc, state);

    E(il, OpCodes.Ldtoken, particleState); E(il, OpCodes.Call, typeGetFromHandle);
    E(il, OpCodes.Ldloc, state); E(il, OpCodes.Box, particleState);
    E(il, OpCodes.Call, enumGetName); E(il, OpCodes.Stloc, name);

    E(il, OpCodes.Ldstr, "prefabs"); E(il, OpCodes.Ldstr, "ParticleSystem"); E(il, OpCodes.Ldloc, name);
    E(il, OpCodes.Call, pathCombine3); E(il, OpCodes.Stloc, path);

    E(il, OpCodes.Ldloc, path); E(il, OpCodes.Call, loadGameObject); E(il, OpCodes.Stloc, prefab);

    var loaded = Instruction.Create(OpCodes.Nop);
    E(il, OpCodes.Ldloc, prefab); E(il, OpCodes.Call, objectImplicit); E(il, OpCodes.Brtrue, loaded);

    E(il, OpCodes.Ldarg_0); E(il, OpCodes.Ldfld, partDict); E(il, OpCodes.Ldloc, state); E(il, OpCodes.Ldnull); E(il, OpCodes.Callvirt, dictAdd);
    E(il, OpCodes.Ldstr, "加载特效"); E(il, OpCodes.Ldloc, name); E(il, OpCodes.Ldstr, "失败"); E(il, OpCodes.Call, concat3); E(il, OpCodes.Call, debugLog);
    E(il, OpCodes.Br, check);

    il.Append(loaded);
    E(il, OpCodes.Ldarg_0); E(il, OpCodes.Ldfld, partDict); E(il, OpCodes.Ldloc, state); E(il, OpCodes.Ldloc, prefab); E(il, OpCodes.Callvirt, dictAdd);

    il.Append(check); E(il, OpCodes.Callvirt, enumMoveNext); E(il, OpCodes.Brtrue, loop);
    var afterFinally = Instruction.Create(OpCodes.Ret);
    E(il, OpCodes.Leave, afterFinally);

    var finallyStart = Instruction.Create(OpCodes.Ldloc, en); il.Append(finallyStart);
    E(il, OpCodes.Isinst, iDisposableType); E(il, OpCodes.Stloc, disp);
    var finallyEnd = Instruction.Create(OpCodes.Endfinally);
    E(il, OpCodes.Ldloc, disp); E(il, OpCodes.Brfalse, finallyEnd);
    E(il, OpCodes.Ldloc, disp); E(il, OpCodes.Callvirt, disposeInterface);
    il.Append(finallyEnd);
    il.Append(afterFinally);

    b.ExceptionHandlers.Add(new ExceptionHandler(ExceptionHandlerType.Finally)
    {
        TryStart = tryStart,
        TryEnd = finallyStart,
        HandlerStart = finallyStart,
        HandlerEnd = afterFinally
    });
}

void PatchParticlesCreate()
{
    var b = B(particlesCreate, true, 6);
    var il = b.GetILProcessor();
    var value = new VariableDefinition(gameObjectType);
    b.Variables.Add(value);

    E(il, OpCodes.Ldarg_0); E(il, OpCodes.Ldfld, partDict);
    E(il, OpCodes.Ldarg_1); E(il, OpCodes.Ldloca, value);
    E(il, OpCodes.Callvirt, dictTryGet); E(il, OpCodes.Pop);

    var found = Instruction.Create(OpCodes.Nop);
    E(il, OpCodes.Ldloc, value); E(il, OpCodes.Call, objectImplicit); E(il, OpCodes.Brtrue, found);
    E(il, OpCodes.Ldstr, "该特效不存在"); E(il, OpCodes.Call, monoPrint);
    E(il, OpCodes.Ldnull); E(il, OpCodes.Ret);

    il.Append(found);
    E(il, OpCodes.Ldarg_0); E(il, OpCodes.Ldloc, value); E(il, OpCodes.Stfld, partNewParticle);
    E(il, OpCodes.Ldloc, value); E(il, OpCodes.Call, instantiateGameObject); E(il, OpCodes.Ret);
}

void PatchResourceLoadProjectileSprite()
{
    var b = B(loadProjectileSprite, true, 10);
    var il = b.GetILProcessor();
    var basePath = new VariableDefinition(module.TypeSystem.String);
    var en = new VariableDefinition(iEnumeratorType);
    var disp = new VariableDefinition(iDisposableType);
    var state = new VariableDefinition(projectileType);
    var name = new VariableDefinition(module.TypeSystem.String);
    var path = new VariableDefinition(module.TypeSystem.String);
    var sprite = new VariableDefinition(spriteType);
    b.Variables.Add(basePath); b.Variables.Add(en); b.Variables.Add(disp); b.Variables.Add(state); b.Variables.Add(name); b.Variables.Add(path); b.Variables.Add(sprite);

    E(il, OpCodes.Ldstr, "sprites"); E(il, OpCodes.Ldstr, "Projectile"); E(il, OpCodes.Call, pathCombine2); E(il, OpCodes.Stloc, basePath);
    E(il, OpCodes.Ldtoken, projectileType); E(il, OpCodes.Call, typeGetFromHandle); E(il, OpCodes.Call, enumGetValues);
    E(il, OpCodes.Callvirt, arrayGetEnumerator); E(il, OpCodes.Stloc, en);

    var tryStart = Instruction.Create(OpCodes.Nop); il.Append(tryStart);
    var check = Instruction.Create(OpCodes.Ldloc, en);
    E(il, OpCodes.Br, check);
    var loop = Instruction.Create(OpCodes.Ldloc, en); il.Append(loop);
    E(il, OpCodes.Callvirt, enumCurrent); E(il, OpCodes.Unbox_Any, projectileType); E(il, OpCodes.Stloc, state);

    E(il, OpCodes.Ldtoken, projectileType); E(il, OpCodes.Call, typeGetFromHandle);
    E(il, OpCodes.Ldloc, state); E(il, OpCodes.Box, projectileType);
    E(il, OpCodes.Call, enumGetName); E(il, OpCodes.Stloc, name);

    E(il, OpCodes.Ldloc, basePath); E(il, OpCodes.Ldloc, name); E(il, OpCodes.Call, pathCombine2); E(il, OpCodes.Stloc, path);
    E(il, OpCodes.Ldloc, path); E(il, OpCodes.Call, loadSprite); E(il, OpCodes.Stloc, sprite);

    var loaded = Instruction.Create(OpCodes.Nop);
    E(il, OpCodes.Ldloc, sprite); E(il, OpCodes.Call, objectImplicit); E(il, OpCodes.Brtrue, loaded);
    E(il, OpCodes.Ldstr, "加载子弹"); E(il, OpCodes.Ldloc, name); E(il, OpCodes.Ldstr, "的贴图失败"); E(il, OpCodes.Call, concat3); E(il, OpCodes.Call, debugLog);
    E(il, OpCodes.Br, check);

    il.Append(loaded);
    var ensure = Instruction.Create(OpCodes.Ldsfld, rmProjectileSprites);
    var ready = Instruction.Create(OpCodes.Ldsfld, rmProjectileSprites);
    il.Append(ensure); E(il, OpCodes.Callvirt, spriteListCount); E(il, OpCodes.Ldloc, state); E(il, OpCodes.Bgt, ready);
    E(il, OpCodes.Ldsfld, rmProjectileSprites); E(il, OpCodes.Ldnull); E(il, OpCodes.Callvirt, spriteListAdd); E(il, OpCodes.Br, ensure);
    il.Append(ready); E(il, OpCodes.Ldloc, state); E(il, OpCodes.Ldloc, sprite); E(il, OpCodes.Callvirt, spriteListSetItem);

    il.Append(check); E(il, OpCodes.Callvirt, enumMoveNext); E(il, OpCodes.Brtrue, loop);
    var afterFinally = Instruction.Create(OpCodes.Ldsfld, rmProjectileSprites);
    E(il, OpCodes.Leave, afterFinally);

    var finallyStart = Instruction.Create(OpCodes.Ldloc, en); il.Append(finallyStart);
    E(il, OpCodes.Isinst, iDisposableType); E(il, OpCodes.Stloc, disp);
    var finallyEnd = Instruction.Create(OpCodes.Endfinally);
    E(il, OpCodes.Ldloc, disp); E(il, OpCodes.Brfalse, finallyEnd);
    E(il, OpCodes.Ldloc, disp); E(il, OpCodes.Callvirt, disposeInterface);
    il.Append(finallyEnd);

    il.Append(afterFinally); E(il, OpCodes.Ret);

    b.ExceptionHandlers.Add(new ExceptionHandler(ExceptionHandlerType.Finally)
    {
        TryStart = tryStart,
        TryEnd = finallyStart,
        HandlerStart = finallyStart,
        HandlerEnd = afterFinally
    });
}
