using Mono.Cecil;
using Mono.Cecil.Cil;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

if (args.Length != 2)
{
    Console.Error.WriteLine("Usage: GodsPVZ.HF9Patch <input.dll> <output.dll>");
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
TypeDefinition T(string n) => All(module.Types).Single(t => t.Name == n || t.FullName == n);
FieldDefinition F(TypeDefinition t, string n) => t.Fields.Single(f => f.Name == n);
MethodDefinition M(TypeDefinition t, string n, int pc)
{
    var q = t.Methods.Where(m => m.Name == n && m.Parameters.Count == pc).ToList();
    return q.Count == 1 ? q[0] : throw new InvalidDataException($"Expected one {t.FullName}.{n}/{pc}, got {q.Count}");
}
IEnumerable<MethodReference> AllRefs() => All(module.Types).SelectMany(t => t.Methods).Where(m => m.HasBody)
    .SelectMany(m => m.Body.Instructions).Select(i => i.Operand).OfType<MethodReference>();
MethodReference Ref(string decl, string name, params string[] ps)
{
    var q = AllRefs().Where(r => r.DeclaringType.FullName == decl && r.Name == name && r.Parameters.Count == ps.Length)
        .Where(r => r.Parameters.Select(p => p.ParameterType.FullName).SequenceEqual(ps)).ToList();
    if (q.Count == 0) throw new InvalidDataException($"Missing MethodRef {decl}::{name}({string.Join(',', ps)})");
    return q[0];
}
void E(ILProcessor il, OpCode op, object? value = null)
{
    var ins = value switch
    {
        null => Instruction.Create(op),
        string s => Instruction.Create(op, s),
        int i => Instruction.Create(op, i),
        float f => Instruction.Create(op, f),
        MethodReference mr => Instruction.Create(op, mr),
        FieldReference fr => Instruction.Create(op, fr),
        TypeReference tr => Instruction.Create(op, tr),
        VariableDefinition vd => Instruction.Create(op, vd),
        Instruction target => Instruction.Create(op, target),
        _ => throw new NotSupportedException(value.GetType().FullName)
    };
    il.Append(ins);
}

var zombie = T("Zombie");
var plant = T("Plant");
var device = T("Device");
var enemyPath = T("EnemyPath");
var board = T("Board");
PatchUpdateAttack();

Directory.CreateDirectory(Path.GetDirectoryName(output)!);
module.Write(output, new WriterParameters { WriteSymbols = false });

using (var verify = ModuleDefinition.ReadModule(output, new ReaderParameters { InMemory = true, ReadSymbols = false }))
{
    var z = All(verify.Types).Single(t => t.Name == "Zombie");
    var m = z.Methods.Single(x => x.Name == "Update_Attack" && x.Parameters.Count == 0);
    if (m.MetadataToken.ToUInt32() != 0x06000422) throw new InvalidDataException("HF9 token drift");
    if (!m.HasBody || m.Body.Instructions.Count < 300) throw new InvalidDataException($"HF9 body too small: {m.Body.Instructions.Count}");
    if (m.Body.Instructions.Select(i => i.Operand).OfType<MethodReference>().Any(r => r.DeclaringType.FullName.Contains("Cpp2ILHelpers", StringComparison.Ordinal)))
        throw new InvalidDataException("HF9 still contains Cpp2IL helper calls");
    var strings = m.Body.Instructions.Where(i => i.OpCode == OpCodes.Ldstr).Select(i => (string)i.Operand).ToList();
    foreach (var s in new[] { "ShootingTrigger", "isAttacking", "Group", "Ready" })
        if (!strings.Contains(s)) throw new InvalidDataException($"HF9 missing native string {s}");
    var refs = m.Body.Instructions.Select(i => i.Operand).OfType<MethodReference>().ToList();
    if (refs.Count(r => r.DeclaringType.FullName == "UnityEngine.Time" && r.Name == "get_deltaTime") != 2)
        throw new InvalidDataException("HF9 must retain exactly two independent Time.deltaTime calls");
    foreach (var n in new[] { "TrySeekTragetPlant", "Update_Eat", "GetMoveDirection", "SetrSpeed", "ResetMoveSpeed", "ResetAttackSpeed", "ZC_SPHSeekEnemy", "ZC_SnowbeastHitZombieTest", "ZC_SnowbeastSeekBait", "Path_Finding" })
        if (!refs.Any(r => r.Name == n)) throw new InvalidDataException($"HF9 missing {n}");
    foreach (var forbidden in new[] { "Update_Shooting", "TranFromAttack", "TranToAttack", "TranToWalk" })
        if (refs.Any(r => r.Name == forbidden)) throw new InvalidDataException($"HF9 must keep PC-native inlined {forbidden} behavior");
    if (!m.Body.Instructions.Any(i => i.OpCode == OpCodes.Ldc_R4 && i.Operand is float f && f == 9999f))
        throw new InvalidDataException("HF9 missing native EnemyPath waitingTime 9999f");
    if (!m.Body.Instructions.Any(i => i.OpCode == OpCodes.Ldc_R4 && i.Operand is float f && f == 0.02f))
        throw new InvalidDataException("HF9 missing native Snowbeast waitingTime 0.02f");
    Console.WriteLine($"VERIFY Zombie.Update_Attack: {m.Body.Instructions.Count} IL, {m.Body.CodeSize} bytes, deltaTimeRefs=2");
}

Console.WriteLine("HF9 Zombie.Update_Attack @0x18036ADC0 restored from PC native x86-64");
Console.WriteLine($"WROTE {output} ({new FileInfo(output).Length} bytes)");
return 0;

void PatchUpdateAttack()
{
    var m = M(zombie, "Update_Attack", 0);
    if (m.MetadataToken.ToUInt32() != 0x06000422) throw new InvalidDataException("Unexpected Zombie.Update_Attack token");

    var fID = F(zombie, "ID");
    var fShootingTime = F(zombie, "shootingTime");
    var fShootingInterval = F(zombie, "shootingInterval");
    var fUpdateRate = F(zombie, "updateRate");
    var fAnimator = F(zombie, "animator");
    var fPole = F(zombie, "poleZombie_pole");
    var fIsEat = F(zombie, "isEat");
    var fTargetPlant = F(zombie, "targetPlant");
    var fRSpeed = F(zombie, "rSpeed");
    var fTargetDevice = F(zombie, "targetDevice");
    var fSnowSeekCd = F(zombie, "snowbeast_seekCD");
    var fWaitingTime = F(zombie, "waitingTime");
    var fPath = F(zombie, "path");
    var fPrePath = F(zombie, "prePath");
    var fIsStant = F(zombie, "isStant");
    var fSphReady = F(zombie, "SPH_shootReady");
    var fAniStantHash = F(zombie, "Ani_StantHash");
    var fBoard = F(zombie, "board");

    var fDeviceGridX = F(device, "gridX");
    var fDeviceGridY = F(device, "gridY");
    var fDeviceBroken = F(device, "broken");
    var fBoardConfig = F(board, "boardConfig");

    var isDisabled = M(zombie, "IsDisabled", 0);
    var trySeekPlant = M(zombie, "TrySeekTragetPlant", 0);
    var updateEat = M(zombie, "Update_Eat", 1);
    var getMoveDirection = M(zombie, "GetMoveDirection", 0);
    var setrSpeed = M(zombie, "SetrSpeed", 1);
    var resetMoveSpeed = M(zombie, "ResetMoveSpeed", 0);
    var resetAttackSpeed = M(zombie, "ResetAttackSpeed", 0);
    var sphSeekEnemy = M(zombie, "ZC_SPHSeekEnemy", 0);
    var snowHitZombieTest = M(zombie, "ZC_SnowbeastHitZombieTest", 0);
    var snowSeekBait = M(zombie, "ZC_SnowbeastSeekBait", 0);
    var isPlantZombie = M(zombie, "IsPlantZombie", 0);
    var pathFinding = M(zombie, "Path_Finding", 0);
    var blockZombie = M(plant, "BlockZombie", 2);
    var enemyPathCtor = M(enemyPath, ".ctor", 4);

    var deltaTime = Ref("UnityEngine.Time", "get_deltaTime");
    var setTrigger = Ref("UnityEngine.Animator", "SetTrigger", "System.String");
    var setBoolString = Ref("UnityEngine.Animator", "SetBool", "System.String", "System.Boolean");
    var setBoolInt = Ref("UnityEngine.Animator", "SetBool", "System.Int32", "System.Boolean");
    var setInteger = Ref("UnityEngine.Animator", "SetInteger", "System.String", "System.Int32");
    var randomRange = Ref("UnityEngine.Random", "Range", "System.Int32", "System.Int32");
    var objectEq = Ref("UnityEngine.Object", "op_Equality", "UnityEngine.Object", "UnityEngine.Object");
    var objectNeq = Ref("UnityEngine.Object", "op_Inequality", "UnityEngine.Object", "UnityEngine.Object");
    var objectImplicit = Ref("UnityEngine.Object", "op_Implicit", "UnityEngine.Object");

    var listType = (GenericInstanceType)fPrePath.FieldType;
    var getCount = new MethodReference("get_Count", module.TypeSystem.Int32, listType) { HasThis = true };
    var insert = new MethodReference("Insert", module.TypeSystem.Void, listType) { HasThis = true };
    insert.Parameters.Add(new ParameterDefinition(module.TypeSystem.Int32));
    insert.Parameters.Add(new ParameterDefinition(enemyPath));
    var removeAt = new MethodReference("RemoveAt", module.TypeSystem.Void, listType) { HasThis = true };
    removeAt.Parameters.Add(new ParameterDefinition(module.TypeSystem.Int32));

    var b = new MethodBody(m) { InitLocals = true, MaxStackSize = 8 };
    m.Body = b;
    var il = b.GetILProcessor();

    var plantLocal = new VariableDefinition(plant);
    var deviceLocal = new VariableDefinition(device);
    var pathItem = new VariableDefinition(enemyPath);
    var shootNew = new VariableDefinition(module.TypeSystem.Single);
    var seekNew = new VariableDefinition(module.TypeSystem.Single);
    var zeroVec = new VariableDefinition(fRSpeed.FieldType);
    var idLocal = new VariableDefinition(module.TypeSystem.Int32);
    foreach (var v in new[] { plantLocal, deviceLocal, pathItem, shootNew, seekNew, zeroVec, idLocal }) b.Variables.Add(v);

    var afterShooting = Instruction.Create(OpCodes.Nop);
    var shooting = Instruction.Create(OpCodes.Nop);
    var snow = Instruction.Create(OpCodes.Nop);
    var sph = Instruction.Create(OpCodes.Nop);
    var generic = Instruction.Create(OpCodes.Nop);
    var poleCheck = Instruction.Create(OpCodes.Nop);
    var ret = Instruction.Create(OpCodes.Ret);

    // Native shooting-ID mask: {6,7,8,9,19,20,21,22}.
    E(il, OpCodes.Ldarg_0); E(il, OpCodes.Ldfld, fID); E(il, OpCodes.Ldc_I4_6); E(il, OpCodes.Sub); E(il, OpCodes.Ldc_I4_3); E(il, OpCodes.Ble_Un, shooting);
    E(il, OpCodes.Ldarg_0); E(il, OpCodes.Ldfld, fID); E(il, OpCodes.Ldc_I4, 19); E(il, OpCodes.Sub); E(il, OpCodes.Ldc_I4_3); E(il, OpCodes.Bgt_Un, afterShooting);
    il.Append(shooting);
    E(il, OpCodes.Ldarg_0); E(il, OpCodes.Call, isDisabled); E(il, OpCodes.Brtrue, afterShooting);
    E(il, OpCodes.Ldarg_0); E(il, OpCodes.Ldfld, fShootingInterval); E(il, OpCodes.Ldc_R4, 0f); E(il, OpCodes.Ble_Un, afterShooting);
    E(il, OpCodes.Ldarg_0); E(il, OpCodes.Ldfld, fShootingTime);
    E(il, OpCodes.Call, deltaTime); E(il, OpCodes.Ldarg_0); E(il, OpCodes.Ldfld, fUpdateRate); E(il, OpCodes.Mul); E(il, OpCodes.Add); E(il, OpCodes.Stloc, shootNew);
    E(il, OpCodes.Ldarg_0); E(il, OpCodes.Ldloc, shootNew); E(il, OpCodes.Stfld, fShootingTime);
    E(il, OpCodes.Ldloc, shootNew); E(il, OpCodes.Ldarg_0); E(il, OpCodes.Ldfld, fShootingInterval); E(il, OpCodes.Blt_Un, afterShooting);
    E(il, OpCodes.Ldarg_0); E(il, OpCodes.Ldc_R4, 0f); E(il, OpCodes.Stfld, fShootingTime);
    E(il, OpCodes.Ldarg_0); E(il, OpCodes.Ldfld, fAnimator); E(il, OpCodes.Ldstr, "ShootingTrigger"); E(il, OpCodes.Callvirt, setTrigger);

    il.Append(afterShooting);
    E(il, OpCodes.Ldarg_0); E(il, OpCodes.Ldfld, fID); E(il, OpCodes.Ldc_I4, 18); E(il, OpCodes.Beq, ret);
    E(il, OpCodes.Ldarg_0); E(il, OpCodes.Ldfld, fID); E(il, OpCodes.Ldc_I4, 13); E(il, OpCodes.Beq, snow);
    E(il, OpCodes.Ldarg_0); E(il, OpCodes.Ldfld, fID); E(il, OpCodes.Ldc_I4, 17); E(il, OpCodes.Beq, sph);
    E(il, OpCodes.Ldarg_0); E(il, OpCodes.Ldfld, fID); E(il, OpCodes.Ldc_I4_3); E(il, OpCodes.Beq, poleCheck);
    E(il, OpCodes.Ldarg_0); E(il, OpCodes.Ldfld, fID); E(il, OpCodes.Ldc_I4, 23); E(il, OpCodes.Bne_Un, generic);
    il.Append(poleCheck);
    E(il, OpCodes.Ldarg_0); E(il, OpCodes.Ldfld, fPole); E(il, OpCodes.Brtrue, ret);
    E(il, OpCodes.Br, generic);

    il.Append(sph);
    E(il, OpCodes.Ldarg_0); E(il, OpCodes.Call, sphSeekEnemy); E(il, OpCodes.Br, ret);

    il.Append(generic);
    E(il, OpCodes.Ldarg_0); E(il, OpCodes.Call, trySeekPlant); E(il, OpCodes.Stloc, plantLocal);

    var afterUpdateEat = Instruction.Create(OpCodes.Nop);
    E(il, OpCodes.Ldloc, plantLocal); E(il, OpCodes.Ldnull); E(il, OpCodes.Call, objectNeq); E(il, OpCodes.Brfalse, afterUpdateEat);
    E(il, OpCodes.Ldarg_0); E(il, OpCodes.Ldfld, fIsEat); E(il, OpCodes.Brfalse, afterUpdateEat);
    E(il, OpCodes.Ldarg_0); E(il, OpCodes.Ldloc, plantLocal); E(il, OpCodes.Call, updateEat); E(il, OpCodes.Br, ret);
    il.Append(afterUpdateEat);

    var afterFromAttack = Instruction.Create(OpCodes.Nop);
    var skipOldUnblock = Instruction.Create(OpCodes.Nop);
    E(il, OpCodes.Ldloc, plantLocal); E(il, OpCodes.Ldnull); E(il, OpCodes.Call, objectEq); E(il, OpCodes.Brfalse, afterFromAttack);
    E(il, OpCodes.Ldarg_0); E(il, OpCodes.Ldfld, fIsEat); E(il, OpCodes.Brfalse, afterFromAttack);
    E(il, OpCodes.Ldarg_0); E(il, OpCodes.Ldfld, fTargetPlant); E(il, OpCodes.Call, objectImplicit); E(il, OpCodes.Brfalse, skipOldUnblock);
    E(il, OpCodes.Ldarg_0); E(il, OpCodes.Ldfld, fTargetPlant); E(il, OpCodes.Ldarg_0); E(il, OpCodes.Ldc_I4_0); E(il, OpCodes.Call, blockZombie);
    il.Append(skipOldUnblock);
    E(il, OpCodes.Ldarg_0); E(il, OpCodes.Ldfld, fAnimator); E(il, OpCodes.Ldstr, "isAttacking"); E(il, OpCodes.Ldc_I4_0); E(il, OpCodes.Callvirt, setBoolString);
    E(il, OpCodes.Ldarg_0); E(il, OpCodes.Ldarg_0); E(il, OpCodes.Call, getMoveDirection); E(il, OpCodes.Call, setrSpeed);
    E(il, OpCodes.Ldarg_0); E(il, OpCodes.Call, resetMoveSpeed);
    E(il, OpCodes.Ldarg_0); E(il, OpCodes.Ldc_I4_0); E(il, OpCodes.Stfld, fIsEat);
    il.Append(afterFromAttack);

    var afterToAttack = Instruction.Create(OpCodes.Nop);
    E(il, OpCodes.Ldloc, plantLocal); E(il, OpCodes.Ldnull); E(il, OpCodes.Call, objectNeq); E(il, OpCodes.Brfalse, afterToAttack);
    E(il, OpCodes.Ldarg_0); E(il, OpCodes.Ldfld, fIsEat); E(il, OpCodes.Brtrue, afterToAttack);

    var sameTarget = Instruction.Create(OpCodes.Nop);
    var oldDone = Instruction.Create(OpCodes.Nop);
    var newDone = Instruction.Create(OpCodes.Nop);
    E(il, OpCodes.Ldarg_0); E(il, OpCodes.Ldfld, fTargetPlant); E(il, OpCodes.Ldloc, plantLocal); E(il, OpCodes.Call, objectNeq); E(il, OpCodes.Brfalse, sameTarget);
    E(il, OpCodes.Ldarg_0); E(il, OpCodes.Ldfld, fTargetPlant); E(il, OpCodes.Ldnull); E(il, OpCodes.Call, objectNeq); E(il, OpCodes.Brfalse, oldDone);
    E(il, OpCodes.Ldarg_0); E(il, OpCodes.Ldfld, fTargetPlant); E(il, OpCodes.Ldarg_0); E(il, OpCodes.Ldc_I4_0); E(il, OpCodes.Call, blockZombie);
    il.Append(oldDone);
    E(il, OpCodes.Ldloc, plantLocal); E(il, OpCodes.Ldnull); E(il, OpCodes.Call, objectNeq); E(il, OpCodes.Brfalse, newDone);
    E(il, OpCodes.Ldloc, plantLocal); E(il, OpCodes.Ldarg_0); E(il, OpCodes.Ldc_I4_1); E(il, OpCodes.Call, blockZombie);
    il.Append(newDone);
    il.Append(sameTarget);
    E(il, OpCodes.Ldarg_0); E(il, OpCodes.Ldfld, fAnimator); E(il, OpCodes.Ldstr, "isAttacking"); E(il, OpCodes.Ldc_I4_1); E(il, OpCodes.Callvirt, setBoolString);
    E(il, OpCodes.Ldarg_0); E(il, OpCodes.Call, resetAttackSpeed);
    E(il, OpCodes.Ldloca, zeroVec); E(il, OpCodes.Initobj, fRSpeed.FieldType);
    E(il, OpCodes.Ldarg_0); E(il, OpCodes.Ldloc, zeroVec); E(il, OpCodes.Stfld, fRSpeed);
    E(il, OpCodes.Ldarg_0); E(il, OpCodes.Ldc_I4_1); E(il, OpCodes.Stfld, fIsEat);
    il.Append(afterToAttack);
    E(il, OpCodes.Ldarg_0); E(il, OpCodes.Ldloc, plantLocal); E(il, OpCodes.Stfld, fTargetPlant);
    E(il, OpCodes.Br, ret);

    il.Append(snow);
    E(il, OpCodes.Ldarg_0); E(il, OpCodes.Call, snowHitZombieTest);
    var snowNoTarget = Instruction.Create(OpCodes.Nop);
    var snowTryPlant = Instruction.Create(OpCodes.Nop);
    var snowReturn = Instruction.Create(OpCodes.Nop);
    E(il, OpCodes.Ldarg_0); E(il, OpCodes.Ldfld, fTargetDevice); E(il, OpCodes.Ldnull); E(il, OpCodes.Call, objectEq); E(il, OpCodes.Brtrue, snowNoTarget);
    E(il, OpCodes.Ldarg_0); E(il, OpCodes.Ldfld, fTargetDevice); E(il, OpCodes.Ldfld, fDeviceBroken); E(il, OpCodes.Brtrue, snowTryPlant);
    E(il, OpCodes.Ldarg_0); E(il, OpCodes.Ldfld, fIsStant); E(il, OpCodes.Brfalse, snowReturn);
    E(il, OpCodes.Ldarg_0); E(il, OpCodes.Ldc_R4, 0f); E(il, OpCodes.Stfld, fWaitingTime);
    var existingNoPath = Instruction.Create(OpCodes.Nop);
    E(il, OpCodes.Ldarg_0); E(il, OpCodes.Ldfld, fPath); E(il, OpCodes.Brfalse, existingNoPath);
    E(il, OpCodes.Ldarg_0); E(il, OpCodes.Ldfld, fPath); E(il, OpCodes.Callvirt, getCount); E(il, OpCodes.Brfalse, existingNoPath);
    EmitResumeWalk(il);
    E(il, OpCodes.Br, snowReturn);
    il.Append(existingNoPath);
    E(il, OpCodes.Ldarg_0); E(il, OpCodes.Ldc_R4, 0.02f); E(il, OpCodes.Stfld, fWaitingTime);
    E(il, OpCodes.Br, snowReturn);

    il.Append(snowNoTarget);
    E(il, OpCodes.Ldarg_0); E(il, OpCodes.Ldfld, fSnowSeekCd); E(il, OpCodes.Call, deltaTime); E(il, OpCodes.Sub); E(il, OpCodes.Stloc, seekNew);
    E(il, OpCodes.Ldarg_0); E(il, OpCodes.Ldloc, seekNew); E(il, OpCodes.Stfld, fSnowSeekCd);
    E(il, OpCodes.Ldc_R4, 0f); E(il, OpCodes.Ldloc, seekNew); E(il, OpCodes.Blt_Un, snowTryPlant);
    E(il, OpCodes.Ldarg_0); E(il, OpCodes.Call, snowSeekBait); E(il, OpCodes.Stloc, deviceLocal);
    var haveDevice = Instruction.Create(OpCodes.Nop);
    E(il, OpCodes.Ldloc, deviceLocal); E(il, OpCodes.Ldnull); E(il, OpCodes.Call, objectEq); E(il, OpCodes.Brfalse, haveDevice);
    E(il, OpCodes.Ldarg_0); E(il, OpCodes.Ldnull); E(il, OpCodes.Stfld, fPath); E(il, OpCodes.Br, snowReturn);
    il.Append(haveDevice);

    var buildTempPath = Instruction.Create(OpCodes.Nop);
    E(il, OpCodes.Ldarg_0); E(il, OpCodes.Ldfld, fWaitingTime); E(il, OpCodes.Ldc_R4, 0f); E(il, OpCodes.Ble_Un, buildTempPath);
    E(il, OpCodes.Ldarg_0); E(il, OpCodes.Ldc_R4, 0f); E(il, OpCodes.Stfld, fWaitingTime);
    var noResumePath = Instruction.Create(OpCodes.Nop);
    E(il, OpCodes.Ldarg_0); E(il, OpCodes.Ldfld, fPath); E(il, OpCodes.Brfalse, noResumePath);
    E(il, OpCodes.Ldarg_0); E(il, OpCodes.Ldfld, fPath); E(il, OpCodes.Callvirt, getCount); E(il, OpCodes.Brfalse, noResumePath);
    EmitResumeWalk(il);
    E(il, OpCodes.Br, buildTempPath);
    il.Append(noResumePath);
    E(il, OpCodes.Ldarg_0); E(il, OpCodes.Ldc_R4, 0.02f); E(il, OpCodes.Stfld, fWaitingTime);

    il.Append(buildTempPath);
    E(il, OpCodes.Ldloc, deviceLocal); E(il, OpCodes.Ldfld, fDeviceGridX);
    E(il, OpCodes.Ldloc, deviceLocal); E(il, OpCodes.Ldfld, fDeviceGridY);
    E(il, OpCodes.Ldc_R4, 9999f);
    E(il, OpCodes.Ldarg_0); E(il, OpCodes.Ldfld, fBoard); E(il, OpCodes.Ldfld, fBoardConfig);
    E(il, OpCodes.Newobj, enemyPathCtor); E(il, OpCodes.Stloc, pathItem);
    E(il, OpCodes.Ldarg_0); E(il, OpCodes.Ldfld, fPrePath); E(il, OpCodes.Ldc_I4_0); E(il, OpCodes.Ldloc, pathItem); E(il, OpCodes.Callvirt, insert);
    E(il, OpCodes.Ldarg_0); E(il, OpCodes.Call, pathFinding); E(il, OpCodes.Pop);
    E(il, OpCodes.Ldarg_0); E(il, OpCodes.Ldfld, fPrePath); E(il, OpCodes.Ldc_I4_0); E(il, OpCodes.Callvirt, removeAt);
    E(il, OpCodes.Ldarg_0); E(il, OpCodes.Ldc_R4, 1f); E(il, OpCodes.Stfld, fSnowSeekCd);
    E(il, OpCodes.Ldarg_0); E(il, OpCodes.Ldloc, deviceLocal); E(il, OpCodes.Stfld, fTargetDevice);
    E(il, OpCodes.Br, snowReturn);

    il.Append(snowTryPlant);
    E(il, OpCodes.Ldarg_0); E(il, OpCodes.Call, trySeekPlant); E(il, OpCodes.Pop);
    il.Append(snowReturn);
    E(il, OpCodes.Br, ret);

    il.Append(ret);

    void EmitResumeWalk(ILProcessor p)
    {
        var doGroup = Instruction.Create(OpCodes.Nop);
        var checkPlant = Instruction.Create(OpCodes.Nop);
        var afterGroup = Instruction.Create(OpCodes.Nop);
        var afterReady = Instruction.Create(OpCodes.Nop);

        E(p, OpCodes.Ldarg_0); E(p, OpCodes.Ldfld, fID); E(p, OpCodes.Stloc, idLocal);
        foreach (var id in new[] { 0, 2, 4, 5, 14, 15 })
        {
            E(p, OpCodes.Ldloc, idLocal); E(p, OpCodes.Ldc_I4, id); E(p, OpCodes.Beq, doGroup);
        }
        E(p, OpCodes.Br, checkPlant);
        p.Append(checkPlant);
        E(p, OpCodes.Ldarg_0); E(p, OpCodes.Call, isPlantZombie); E(p, OpCodes.Brfalse, afterGroup);
        p.Append(doGroup);
        E(p, OpCodes.Ldarg_0); E(p, OpCodes.Ldfld, fAnimator); E(p, OpCodes.Ldstr, "Group"); E(p, OpCodes.Ldc_I4_0); E(p, OpCodes.Ldc_I4_2); E(p, OpCodes.Call, randomRange); E(p, OpCodes.Callvirt, setInteger);
        p.Append(afterGroup);
        E(p, OpCodes.Ldarg_0); E(p, OpCodes.Ldfld, fAnimator); E(p, OpCodes.Ldsfld, fAniStantHash); E(p, OpCodes.Ldc_I4_0); E(p, OpCodes.Callvirt, setBoolInt);
        E(p, OpCodes.Ldarg_0); E(p, OpCodes.Ldarg_0); E(p, OpCodes.Call, getMoveDirection); E(p, OpCodes.Call, setrSpeed);
        E(p, OpCodes.Ldarg_0); E(p, OpCodes.Call, resetMoveSpeed);
        E(p, OpCodes.Ldarg_0); E(p, OpCodes.Ldc_I4_0); E(p, OpCodes.Stfld, fIsStant);
        E(p, OpCodes.Ldarg_0); E(p, OpCodes.Ldfld, fID); E(p, OpCodes.Ldc_I4, 17); E(p, OpCodes.Bne_Un, afterReady);
        E(p, OpCodes.Ldarg_0); E(p, OpCodes.Ldfld, fAnimator); E(p, OpCodes.Ldstr, "Ready"); E(p, OpCodes.Ldc_I4_0); E(p, OpCodes.Callvirt, setBoolString);
        E(p, OpCodes.Ldarg_0); E(p, OpCodes.Ldc_I4_0); E(p, OpCodes.Stfld, fSphReady);
        p.Append(afterReady);
    }
}
