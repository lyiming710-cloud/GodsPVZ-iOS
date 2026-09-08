using Mono.Cecil;
using Mono.Cecil.Cil;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

if (args.Length != 2)
{
    Console.Error.WriteLine("Usage: GodsPVZ.HF3Patch <HF2.dll> <HF3.dll>");
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

TypeDefinition T(string n) => All(module.Types).Single(x => x.Name == n || x.FullName == n);
FieldDefinition F(TypeDefinition t, string n) => t.Fields.Single(x => x.Name == n);
MethodDefinition M(TypeDefinition t, string n, int pc)
{
    var q = t.Methods.Where(x => x.Name == n && x.Parameters.Count == pc).ToList();
    return q.Count == 1 ? q[0] : throw new Exception($"Expected one {t.FullName}.{n}/{pc}, got {q.Count}");
}

IEnumerable<MethodReference> AllRefs() => All(module.Types)
    .SelectMany(t => t.Methods)
    .Where(m => m.HasBody)
    .SelectMany(m => m.Body.Instructions)
    .Select(i => i.Operand)
    .OfType<MethodReference>();

MethodReference ExactRef(string decl, string name, params string[] parameterTypes)
{
    var q = AllRefs().Where(x =>
        x.DeclaringType.FullName == decl &&
        x.Name == name &&
        x.Parameters.Count == parameterTypes.Length &&
        x.Parameters.Select(p => p.ParameterType.FullName).SequenceEqual(parameterTypes)).ToList();
    if (q.Count == 0) throw new Exception($"Missing MethodRef {decl}.{name}({string.Join(',', parameterTypes)})");
    return q[0];
}

GenericInstanceMethod GenericRef(string decl, string name, string genericArg)
{
    var q = AllRefs().OfType<GenericInstanceMethod>().Where(x =>
        x.DeclaringType.FullName.Contains(decl) &&
        x.Name == name &&
        x.GenericArguments.Any(a => a.Name == genericArg || a.FullName == genericArg)).ToList();
    if (q.Count == 0) throw new Exception($"Missing generic MethodRef {decl}.{name}<{genericArg}>");
    return q[0];
}

MethodReference ListMethod(GenericInstanceType list, string name, TypeReference ret, params TypeReference[] ps)
{
    var mr = new MethodReference(name, ret, list) { HasThis = true };
    foreach (var p in ps) mr.Parameters.Add(new ParameterDefinition(p));
    return mr;
}

MethodBody Fresh(MethodDefinition m, bool initLocals = false)
{
    var b = new MethodBody(m) { InitLocals = initLocals, MaxStackSize = 64 };
    m.Body = b;
    return b;
}

void E(ILProcessor il, OpCode op, object? value = null)
{
    Instruction x = value switch
    {
        null => Instruction.Create(op),
        string s => Instruction.Create(op, s),
        int i => Instruction.Create(op, i),
        float f => Instruction.Create(op, f),
        MethodReference mr => Instruction.Create(op, mr),
        FieldReference fr => Instruction.Create(op, fr),
        TypeReference tr => Instruction.Create(op, tr),
        ParameterDefinition pd => Instruction.Create(op, pd),
        VariableDefinition vd => Instruction.Create(op, vd),
        Instruction ins => Instruction.Create(op, ins),
        _ => throw new NotSupportedException(value.GetType().FullName)
    };
    il.Append(x);
}

var zombie = T("Zombie");
var globals = T("GlobalStaticVars");
var resources = T("ResourceManager");
var method = M(zombie, "InjuryStatusUpdate_Body", 1);
var body = Fresh(method, initLocals: true);
var il = body.GetILProcessor();
var noResidue = method.Parameters[0];

var fID = F(zombie, "ID");
var fHealth = F(zombie, "healthPoint");
var fMaxHealth = F(zombie, "maxHealthPoint");
var fBroken = F(zombie, "brokenLevel");
var fArmBroken = F(zombie, "armBroken");
var fDying = F(zombie, "isDying");
var fDied = F(zombie, "isDied");
var fInvincible = F(zombie, "invincible");
var fNutHot = F(zombie, "nutZombie_hotNut");
var fShootReady = F(zombie, "SPH_shootReady");
var fSkill3 = F(zombie, "IVH_skill3");
var fSkillOngoing = F(zombie, "IVH_skillOngoning");
var fPrePath = F(zombie, "prePath");
var fAnimator = F(zombie, "animator");
var fAnimationSprites = F(zombie, "animationSprites");
var fZombieSprites = F(resources, "zombieSprites");

var armBroken = M(zombie, "ArmBroken", 1);
var headDrop = M(zombie, "HeadDrop", 1);
var dropLoot = M(zombie, "DropLootPiece", 0);
var die = M(zombie, "Die", 2);
var isDisabled = M(zombie, "IsDisabled", 0);
var createStartPrePath = M(zombie, "CreateStartPrePath", 0);
var pathFinding = M(zombie, "Path_Finding", 0);
var tranToWalk = M(zombie, "TranToWalk", 0);
var nutHot = M(zombie, "ZC_NutHot", 0);
var nutBoom = M(zombie, "ZC_NutBoom", 0);
var getAnimationSprite = M(globals, "GetAnimationSprite_Name", 2);

var objImplicit = ExactRef("UnityEngine.Object", "op_Implicit", "UnityEngine.Object");
var animSetInteger = ExactRef("UnityEngine.Animator", "SetInteger", "System.String", "System.Int32");
var animSetBool = ExactRef("UnityEngine.Animator", "SetBool", "System.String", "System.Boolean");
var animSetTrigger = ExactRef("UnityEngine.Animator", "SetTrigger", "System.String");
var mathFloor = ExactRef("System.Math", "Floor", "System.Double");
var getRenderer = GenericRef("UnityEngine.Component", "GetComponent", "SpriteRenderer");
var spriteRendererType = getRenderer.GenericArguments[0];
var setSprite = ExactRef("UnityEngine.SpriteRenderer", "set_sprite", "UnityEngine.Sprite");

var prePathType = (GenericInstanceType)fPrePath.FieldType;
var clearPrePath = ListMethod(prePathType, "Clear", module.TypeSystem.Void);
var zombieSpritesType = (GenericInstanceType)fZombieSprites.FieldType;
var getZombieSprite = ListMethod(zombieSpritesType, "get_Item", zombieSpritesType.GenericArguments[0], module.TypeSystem.Int32);

var damageRatio = new VariableDefinition(module.TypeSystem.Single);
var stage = new VariableDefinition(module.TypeSystem.Int32);
var changed = new VariableDefinition(module.TypeSystem.Boolean);
var spriteIndex = new VariableDefinition(module.TypeSystem.Int32);
var bodyObject = new VariableDefinition(getAnimationSprite.ReturnType);
var renderer = new VariableDefinition(spriteRendererType);
body.Variables.Add(damageRatio);
body.Variables.Add(stage);
body.Variables.Add(changed);
body.Variables.Add(spriteIndex);
body.Variables.Add(bodyObject);
body.Variables.Add(renderer);

var id11 = Instruction.Create(OpCodes.Nop);
var id17 = Instruction.Create(OpCodes.Nop);
var id18 = Instruction.Create(OpCodes.Nop);
var generic = Instruction.Create(OpCodes.Nop);
var deathCheck = Instruction.Create(OpCodes.Nop);
var doDie = Instruction.Create(OpCodes.Nop);
var retDisabled = Instruction.Create(OpCodes.Nop);

// Native dispatch: ID 11 / 12 / 17 / 18 have dedicated paths.
E(il, OpCodes.Ldarg_0); E(il, OpCodes.Ldfld, fID); E(il, OpCodes.Ldc_I4, 11); E(il, OpCodes.Beq, id11);
E(il, OpCodes.Ldarg_0); E(il, OpCodes.Ldfld, fID); E(il, OpCodes.Ldc_I4, 12); E(il, OpCodes.Beq, retDisabled);
E(il, OpCodes.Ldarg_0); E(il, OpCodes.Ldfld, fID); E(il, OpCodes.Ldc_I4, 17); E(il, OpCodes.Beq, id17);
E(il, OpCodes.Ldarg_0); E(il, OpCodes.Ldfld, fID); E(il, OpCodes.Ldc_I4, 18); E(il, OpCodes.Beq, id18);
E(il, OpCodes.Br, generic);

// Generic body: 2/3 arm loss, 1/3 head loss, <=0 death.
il.Append(generic);
var genericHead = Instruction.Create(OpCodes.Nop);
E(il, OpCodes.Ldarg_0); E(il, OpCodes.Ldfld, fArmBroken); E(il, OpCodes.Brtrue, genericHead);
E(il, OpCodes.Ldarg_0); E(il, OpCodes.Ldfld, fHealth);
E(il, OpCodes.Ldarg_0); E(il, OpCodes.Ldfld, fMaxHealth); E(il, OpCodes.Dup); E(il, OpCodes.Add); E(il, OpCodes.Ldc_R4, 3f); E(il, OpCodes.Div);
E(il, OpCodes.Bge_Un, genericHead);
E(il, OpCodes.Ldarg_0); E(il, OpCodes.Ldarg, noResidue); E(il, OpCodes.Call, armBroken);
il.Append(genericHead);
var genericDeath = Instruction.Create(OpCodes.Nop);
E(il, OpCodes.Ldarg_0); E(il, OpCodes.Ldfld, fDying); E(il, OpCodes.Brtrue, genericDeath);
E(il, OpCodes.Ldarg_0); E(il, OpCodes.Ldfld, fHealth);
E(il, OpCodes.Ldarg_0); E(il, OpCodes.Ldfld, fMaxHealth); E(il, OpCodes.Ldc_R4, 3f); E(il, OpCodes.Div);
E(il, OpCodes.Bge_Un, genericDeath);
E(il, OpCodes.Ldarg_0); E(il, OpCodes.Ldarg, noResidue); E(il, OpCodes.Call, headDrop);
E(il, OpCodes.Ldarg_0); E(il, OpCodes.Call, dropLoot);
il.Append(genericDeath);
E(il, OpCodes.Br, deathCheck);

// ID 18: Imperial Vanguard Harbinger. brokenLevel=floor((max-hp)*4/max).
il.Append(id18);
E(il, OpCodes.Ldarg_0);
E(il, OpCodes.Ldarg_0); E(il, OpCodes.Ldfld, fMaxHealth);
E(il, OpCodes.Ldarg_0); E(il, OpCodes.Ldfld, fHealth);
E(il, OpCodes.Sub); E(il, OpCodes.Ldc_R4, 4f); E(il, OpCodes.Mul);
E(il, OpCodes.Ldarg_0); E(il, OpCodes.Ldfld, fMaxHealth); E(il, OpCodes.Div);
E(il, OpCodes.Conv_R8); E(il, OpCodes.Call, mathFloor); E(il, OpCodes.Conv_I4); E(il, OpCodes.Stfld, fBroken);
E(il, OpCodes.Ldarg_0); E(il, OpCodes.Ldfld, fBroken); E(il, OpCodes.Ldc_I4_3); E(il, OpCodes.Blt, deathCheck);
E(il, OpCodes.Ldarg_0); E(il, OpCodes.Ldfld, fSkill3); E(il, OpCodes.Brfalse, deathCheck);
E(il, OpCodes.Ldarg_0); E(il, OpCodes.Ldarg_0); E(il, OpCodes.Ldfld, fMaxHealth); E(il, OpCodes.Ldc_R4, 0.25f); E(il, OpCodes.Mul); E(il, OpCodes.Stfld, fHealth);
E(il, OpCodes.Ldarg_0); E(il, OpCodes.Ldc_I4_0); E(il, OpCodes.Stfld, fSkill3);
var id18AfterAnimator = Instruction.Create(OpCodes.Nop);
E(il, OpCodes.Ldarg_0); E(il, OpCodes.Ldfld, fAnimator); E(il, OpCodes.Call, objImplicit); E(il, OpCodes.Brfalse, id18AfterAnimator);
E(il, OpCodes.Ldarg_0); E(il, OpCodes.Ldc_I4_1); E(il, OpCodes.Stfld, fSkillOngoing);
E(il, OpCodes.Ldarg_0); E(il, OpCodes.Ldfld, fAnimator); E(il, OpCodes.Ldstr, "SkillTrigger3"); E(il, OpCodes.Callvirt, animSetTrigger);
il.Append(id18AfterAnimator);
E(il, OpCodes.Ldarg_0); E(il, OpCodes.Ldc_I4_1); E(il, OpCodes.Stfld, fInvincible);
E(il, OpCodes.Br, deathCheck);

// ID 17: SPH. Damage stage drives BrokenLevel animator int and Ready state.
il.Append(id17);
E(il, OpCodes.Ldc_I4_0); E(il, OpCodes.Stloc, stage);
var id17AfterTwoThirds = Instruction.Create(OpCodes.Nop);
E(il, OpCodes.Ldarg_0); E(il, OpCodes.Ldfld, fHealth);
E(il, OpCodes.Ldarg_0); E(il, OpCodes.Ldfld, fMaxHealth); E(il, OpCodes.Dup); E(il, OpCodes.Add); E(il, OpCodes.Ldc_R4, 3f); E(il, OpCodes.Div);
E(il, OpCodes.Bge_Un, id17AfterTwoThirds); E(il, OpCodes.Ldc_I4_1); E(il, OpCodes.Stloc, stage);
il.Append(id17AfterTwoThirds);
var id17AfterOneThird = Instruction.Create(OpCodes.Nop);
E(il, OpCodes.Ldarg_0); E(il, OpCodes.Ldfld, fHealth);
E(il, OpCodes.Ldarg_0); E(il, OpCodes.Ldfld, fMaxHealth); E(il, OpCodes.Ldc_R4, 3f); E(il, OpCodes.Div);
E(il, OpCodes.Bge_Un, id17AfterOneThird); E(il, OpCodes.Ldc_I4_2); E(il, OpCodes.Stloc, stage);
il.Append(id17AfterOneThird);
E(il, OpCodes.Ldarg_0); E(il, OpCodes.Ldfld, fAnimator); E(il, OpCodes.Call, objImplicit); E(il, OpCodes.Brfalse, deathCheck);
E(il, OpCodes.Ldarg_0); E(il, OpCodes.Ldfld, fAnimator); E(il, OpCodes.Ldstr, "BrokenLevel"); E(il, OpCodes.Ldloc, stage); E(il, OpCodes.Callvirt, animSetInteger);
var id17SetBroken = Instruction.Create(OpCodes.Nop);
E(il, OpCodes.Ldloc, stage); E(il, OpCodes.Ldc_I4_2); E(il, OpCodes.Blt, id17SetBroken);
var id17AfterPathReset = Instruction.Create(OpCodes.Nop);
E(il, OpCodes.Ldarg_0); E(il, OpCodes.Ldfld, fBroken); E(il, OpCodes.Ldc_I4_2); E(il, OpCodes.Bge, id17AfterPathReset);
E(il, OpCodes.Ldarg_0); E(il, OpCodes.Ldfld, fPrePath); E(il, OpCodes.Callvirt, clearPrePath);
E(il, OpCodes.Ldarg_0); E(il, OpCodes.Call, createStartPrePath);
E(il, OpCodes.Ldarg_0); E(il, OpCodes.Call, pathFinding); E(il, OpCodes.Brfalse, id17AfterPathReset);
E(il, OpCodes.Ldarg_0); E(il, OpCodes.Call, tranToWalk);
il.Append(id17AfterPathReset);
E(il, OpCodes.Ldarg_0); E(il, OpCodes.Ldfld, fAnimator); E(il, OpCodes.Ldstr, "Ready"); E(il, OpCodes.Ldc_I4_0); E(il, OpCodes.Callvirt, animSetBool);
E(il, OpCodes.Ldarg_0); E(il, OpCodes.Ldc_I4_0); E(il, OpCodes.Stfld, fShootReady);
il.Append(id17SetBroken);
E(il, OpCodes.Ldarg_0); E(il, OpCodes.Ldloc, stage); E(il, OpCodes.Stfld, fBroken);
E(il, OpCodes.Br, deathCheck);

// ID 11: nut zombie. Preserve original 0.2/0.4/0.6/0.8 thresholds and original brokenLevel writes.
il.Append(id11);
E(il, OpCodes.Ldc_R4, 1f);
E(il, OpCodes.Ldarg_0); E(il, OpCodes.Ldfld, fHealth);
E(il, OpCodes.Ldarg_0); E(il, OpCodes.Ldfld, fMaxHealth); E(il, OpCodes.Div); E(il, OpCodes.Sub); E(il, OpCodes.Stloc, damageRatio);
E(il, OpCodes.Ldc_I4_0); E(il, OpCodes.Stloc, changed);
E(il, OpCodes.Ldc_I4_M1); E(il, OpCodes.Stloc, spriteIndex);

var id11AfterArm = Instruction.Create(OpCodes.Nop);
E(il, OpCodes.Ldarg_0); E(il, OpCodes.Ldfld, fArmBroken); E(il, OpCodes.Brtrue, id11AfterArm);
E(il, OpCodes.Ldloc, damageRatio); E(il, OpCodes.Ldc_R4, 0.6f); E(il, OpCodes.Ble_Un, id11AfterArm);
var id11SkipHot = Instruction.Create(OpCodes.Nop);
E(il, OpCodes.Ldloc, damageRatio); E(il, OpCodes.Ldc_R4, 0.8f); E(il, OpCodes.Bgt_Un, id11SkipHot);
E(il, OpCodes.Ldarg_0); E(il, OpCodes.Call, nutHot); E(il, OpCodes.Ldc_I4_1); E(il, OpCodes.Stloc, changed);
il.Append(id11SkipHot);
E(il, OpCodes.Ldarg_0); E(il, OpCodes.Ldarg, noResidue); E(il, OpCodes.Call, armBroken);
il.Append(id11AfterArm);

var id11AfterHead = Instruction.Create(OpCodes.Nop);
E(il, OpCodes.Ldarg_0); E(il, OpCodes.Ldfld, fDying); E(il, OpCodes.Brtrue, id11AfterHead);
E(il, OpCodes.Ldloc, damageRatio); E(il, OpCodes.Ldc_R4, 0.8f); E(il, OpCodes.Ble_Un, id11AfterHead);
E(il, OpCodes.Ldarg_0); E(il, OpCodes.Ldarg, noResidue); E(il, OpCodes.Call, headDrop);
E(il, OpCodes.Ldarg_0); E(il, OpCodes.Call, dropLoot);
var id11AfterBoom = Instruction.Create(OpCodes.Nop);
E(il, OpCodes.Ldarg_0); E(il, OpCodes.Ldfld, fNutHot); E(il, OpCodes.Brfalse, id11AfterBoom);
E(il, OpCodes.Ldarg_0); E(il, OpCodes.Call, nutBoom);
il.Append(id11AfterBoom);
il.Append(id11AfterHead);

var id11StageSelect = Instruction.Create(OpCodes.Nop);
E(il, OpCodes.Ldarg_0); E(il, OpCodes.Ldfld, fDied); E(il, OpCodes.Brtrue, id11StageSelect);
var id11SkipDeath = Instruction.Create(OpCodes.Nop);
E(il, OpCodes.Ldarg_0); E(il, OpCodes.Ldfld, fHealth); E(il, OpCodes.Ldc_R4, 0f); E(il, OpCodes.Bgt_Un, id11SkipDeath);
E(il, OpCodes.Ldarg_0); E(il, OpCodes.Ldarg, noResidue); E(il, OpCodes.Ldc_I4_0); E(il, OpCodes.Call, die);
il.Append(id11SkipDeath);
il.Append(id11StageSelect);

var id11Stage1 = Instruction.Create(OpCodes.Nop);
var id11Stage2 = Instruction.Create(OpCodes.Nop);
var id11AfterStages = Instruction.Create(OpCodes.Nop);
E(il, OpCodes.Ldloc, damageRatio); E(il, OpCodes.Ldc_R4, 0f); E(il, OpCodes.Blt_Un, id11Stage1);
E(il, OpCodes.Ldloc, damageRatio); E(il, OpCodes.Ldc_R4, 0.2f); E(il, OpCodes.Bgt_Un, id11Stage1);
var id11Stage0SetIndex = Instruction.Create(OpCodes.Nop);
E(il, OpCodes.Ldarg_0); E(il, OpCodes.Ldfld, fBroken); E(il, OpCodes.Brfalse, id11Stage0SetIndex);
E(il, OpCodes.Ldarg_0); E(il, OpCodes.Ldc_I4_0); E(il, OpCodes.Stfld, fBroken); E(il, OpCodes.Ldc_I4_1); E(il, OpCodes.Stloc, changed);
il.Append(id11Stage0SetIndex); E(il, OpCodes.Ldc_I4, 19); E(il, OpCodes.Stloc, spriteIndex); E(il, OpCodes.Br, id11AfterStages);

il.Append(id11Stage1);
E(il, OpCodes.Ldloc, damageRatio); E(il, OpCodes.Ldc_R4, 0.2f); E(il, OpCodes.Ble_Un, id11Stage2);
E(il, OpCodes.Ldloc, damageRatio); E(il, OpCodes.Ldc_R4, 0.4f); E(il, OpCodes.Bgt_Un, id11Stage2);
var id11Stage1SetIndex = Instruction.Create(OpCodes.Nop);
E(il, OpCodes.Ldarg_0); E(il, OpCodes.Ldfld, fBroken); E(il, OpCodes.Ldc_I4_1); E(il, OpCodes.Beq, id11Stage1SetIndex);
E(il, OpCodes.Ldarg_0); E(il, OpCodes.Ldc_I4_0); E(il, OpCodes.Stfld, fBroken); E(il, OpCodes.Ldc_I4_1); E(il, OpCodes.Stloc, changed);
il.Append(id11Stage1SetIndex); E(il, OpCodes.Ldc_I4, 20); E(il, OpCodes.Stloc, spriteIndex); E(il, OpCodes.Br, id11AfterStages);

il.Append(id11Stage2);
E(il, OpCodes.Ldloc, damageRatio); E(il, OpCodes.Ldc_R4, 0.4f); E(il, OpCodes.Ble_Un, id11AfterStages);
var id11Stage2SetIndex = Instruction.Create(OpCodes.Nop);
E(il, OpCodes.Ldarg_0); E(il, OpCodes.Ldfld, fBroken); E(il, OpCodes.Ldc_I4_2); E(il, OpCodes.Beq, id11Stage2SetIndex);
E(il, OpCodes.Ldarg_0); E(il, OpCodes.Ldc_I4_0); E(il, OpCodes.Stfld, fBroken); E(il, OpCodes.Ldc_I4_1); E(il, OpCodes.Stloc, changed);
il.Append(id11Stage2SetIndex); E(il, OpCodes.Ldc_I4, 21); E(il, OpCodes.Stloc, spriteIndex);

il.Append(id11AfterStages);
E(il, OpCodes.Ldloc, spriteIndex); E(il, OpCodes.Ldc_I4_M1); E(il, OpCodes.Beq, retDisabled);
E(il, OpCodes.Ldloc, changed); E(il, OpCodes.Brfalse, retDisabled);
var id11NoHotIndex = Instruction.Create(OpCodes.Nop);
E(il, OpCodes.Ldarg_0); E(il, OpCodes.Ldfld, fNutHot); E(il, OpCodes.Brfalse, id11NoHotIndex);
E(il, OpCodes.Ldloc, spriteIndex); E(il, OpCodes.Ldc_I4_3); E(il, OpCodes.Add); E(il, OpCodes.Stloc, spriteIndex);
il.Append(id11NoHotIndex);
E(il, OpCodes.Ldnull); E(il, OpCodes.Stloc, renderer);
E(il, OpCodes.Ldarg_0); E(il, OpCodes.Ldfld, fAnimationSprites); E(il, OpCodes.Ldstr, "Wallnut_body"); E(il, OpCodes.Call, getAnimationSprite); E(il, OpCodes.Stloc, bodyObject);
var id11AfterGetRenderer = Instruction.Create(OpCodes.Nop);
E(il, OpCodes.Ldloc, bodyObject); E(il, OpCodes.Brfalse, id11AfterGetRenderer);
E(il, OpCodes.Ldloc, bodyObject); E(il, OpCodes.Callvirt, getRenderer); E(il, OpCodes.Stloc, renderer);
il.Append(id11AfterGetRenderer);
E(il, OpCodes.Ldloc, renderer); E(il, OpCodes.Call, objImplicit); E(il, OpCodes.Brfalse, retDisabled);
E(il, OpCodes.Ldloc, renderer); E(il, OpCodes.Ldsfld, fZombieSprites); E(il, OpCodes.Ldloc, spriteIndex); E(il, OpCodes.Callvirt, getZombieSprite); E(il, OpCodes.Callvirt, setSprite);
E(il, OpCodes.Br, retDisabled);

// Shared death check for generic/ID17/ID18. Native condition is ordered health <= 0 and !isDied.
il.Append(deathCheck);
E(il, OpCodes.Ldarg_0); E(il, OpCodes.Ldfld, fDied); E(il, OpCodes.Brtrue, retDisabled);
E(il, OpCodes.Ldarg_0); E(il, OpCodes.Ldfld, fHealth); E(il, OpCodes.Ldc_R4, 0f); E(il, OpCodes.Ble, doDie);
E(il, OpCodes.Br, retDisabled);
il.Append(doDie);
E(il, OpCodes.Ldarg_0); E(il, OpCodes.Ldarg, noResidue); E(il, OpCodes.Ldc_I4_0); E(il, OpCodes.Call, die);
E(il, OpCodes.Br, retDisabled);

il.Append(retDisabled);
E(il, OpCodes.Ldarg_0); E(il, OpCodes.Call, isDisabled); E(il, OpCodes.Ret);

Directory.CreateDirectory(Path.GetDirectoryName(output)!);
module.Write(output, new WriterParameters { WriteSymbols = false });

using (var verify = ModuleDefinition.ReadModule(output, new ReaderParameters { InMemory = true, ReadSymbols = false }))
{
    var z = All(verify.Types).Single(t => t.Name == "Zombie");
    var m = z.Methods.Single(x => x.Name == "InjuryStatusUpdate_Body" && x.Parameters.Count == 1);
    if (!m.HasBody || m.Body.Instructions.Count < 200) throw new Exception($"HF3 body too small: {m.Body.Instructions.Count}");
    var strings = m.Body.Instructions.Where(i => i.OpCode == OpCodes.Ldstr).Select(i => (string)i.Operand).ToHashSet();
    foreach (var s in new[] { "SkillTrigger3", "BrokenLevel", "Ready", "Wallnut_body" })
        if (!strings.Contains(s)) throw new Exception($"HF3 missing native string {s}");
    var calls = m.Body.Instructions.Select(i => i.Operand).OfType<MethodReference>().Select(x => x.Name).ToHashSet();
    foreach (var c in new[] { "ArmBroken", "HeadDrop", "DropLootPiece", "Die", "IsDisabled", "ZC_NutHot", "ZC_NutBoom", "CreateStartPrePath", "Path_Finding", "TranToWalk", "GetAnimationSprite_Name", "Floor" })
        if (!calls.Contains(c)) throw new Exception($"HF3 missing native call {c}");
    if (m.Body.Instructions.Any(i => i.OpCode == OpCodes.Newobj && i.Operand is MethodReference mr && mr.DeclaringType.FullName == "System.Exception"))
        throw new Exception("HF3 still contains Cpp2IL failure throw");
    Console.WriteLine($"VERIFY Zombie.InjuryStatusUpdate_Body: {m.Body.Instructions.Count} IL, {strings.Count} strings, {calls.Count} call targets");
}

Console.WriteLine("HF3 Zombie.InjuryStatusUpdate_Body @0x1803652B0 restored from PC native x86-64");
Console.WriteLine($"WROTE {output} ({new FileInfo(output).Length} bytes)");
return 0;
