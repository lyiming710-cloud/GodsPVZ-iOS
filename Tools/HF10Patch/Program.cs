using Mono.Cecil;
using Mono.Cecil.Cil;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

if (args.Length != 2)
{
    Console.Error.WriteLine("Usage: GodsPVZ.HF10Patch <input.dll> <output.dll>");
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
var statsType = T("StatsIncreased");
var buffType = T("Buff");
var buffManagerType = T("BuffManager");
var boardType = T("Board");
var boardConfigType = T("BoardConfig");
var gridType = T("Grid");
var plantType = T("Plant");
PatchUpdateCharacteristic();

Directory.CreateDirectory(Path.GetDirectoryName(output)!);
module.Write(output, new WriterParameters { WriteSymbols = false });

using (var verify = ModuleDefinition.ReadModule(output, new ReaderParameters { InMemory = true, ReadSymbols = false }))
{
    var z = All(verify.Types).Single(t => t.Name == "Zombie");
    var m = z.Methods.Single(x => x.Name == "Update_Characteristic" && x.Parameters.Count == 0);
    if (m.MetadataToken.ToUInt32() != 0x06000424) throw new InvalidDataException("HF10 token drift");
    if (!m.HasBody || m.Body.Instructions.Count < 240) throw new InvalidDataException($"HF10 body too small: {m.Body.Instructions.Count}");
    if (m.Body.Instructions.Select(i => i.Operand).OfType<MethodReference>().Any(r => r.DeclaringType.FullName.Contains("Cpp2ILHelpers", StringComparison.Ordinal)))
        throw new InvalidDataException("HF10 still contains Cpp2IL helper calls");
    var strings = m.Body.Instructions.Where(i => i.OpCode == OpCodes.Ldstr).Select(i => (string)i.Operand).ToList();
    foreach (var s in new[] { "PoleCommander.speed", "JumpTrigger", "rest", "PlaceTrigger" })
        if (!strings.Contains(s)) throw new InvalidDataException($"HF10 missing metadata-backed native string {s}");
    var refs = m.Body.Instructions.Select(i => i.Operand).OfType<MethodReference>().ToList();
    if (refs.Count(r => r.DeclaringType.FullName == "UnityEngine.Time" && r.Name == "get_deltaTime") != 3)
        throw new InvalidDataException("HF10 must retain exactly three independent Time.deltaTime calls");
    foreach (var n in new[] { "FindBuff", "ResetMoveSpeed", "ZC_PoleTestJump", "IsDisabled", "ZC_LadderSaboteursPlaceC4", "ZC_LadderSaboteursBoom", "ZC_LadderTestPlace", "ZC_ImperialVanguardHarbinger", "GetGridX", "GetGridY", "GetGrid", "GetPassablePoint" })
        if (!refs.Any(r => r.Name == n)) throw new InvalidDataException($"HF10 missing native call {n}");
    foreach (var f in new[] { 1.5f, 0.2f, 10f, 134f, 100f })
        if (!m.Body.Instructions.Any(i => i.OpCode == OpCodes.Ldc_R4 && i.Operand is float x && x == f))
            throw new InvalidDataException($"HF10 missing native constant {f}");
    Console.WriteLine($"VERIFY Zombie.Update_Characteristic: {m.Body.Instructions.Count} IL, {m.Body.CodeSize} bytes, deltaTimeRefs=3");
}

Console.WriteLine("HF10 Zombie.Update_Characteristic @0x18036B960 restored from PC native x86-64");
Console.WriteLine($"WROTE {output} ({new FileInfo(output).Length} bytes)");
return 0;

void PatchUpdateCharacteristic()
{
    var m = M(zombie, "Update_Characteristic", 0);
    if (m.MetadataToken.ToUInt32() != 0x06000424) throw new InvalidDataException("Unexpected Zombie.Update_Characteristic token");

    var fID = F(zombie, "ID");
    var fPole = F(zombie, "poleZombie_pole");
    var fJump = F(zombie, "poleZombie_jump");
    var fIsStant = F(zombie, "isStant");
    var fBuffManager = F(zombie, "buffManager");
    var fUpdateRate = F(zombie, "updateRate");
    var fAnimator = F(zombie, "animator");
    var fSnowRest = F(zombie, "snowbeast_isRest");
    var fSnowRestTime = F(zombie, "snowbeast_restTime");
    var fC4 = F(zombie, "ladderSaboteurs_C4");
    var fDetonation = F(zombie, "ladderSaboteurs_detonationCountDown");
    var fLadderPlace = F(zombie, "ladderZombie_place");
    var fFX = F(zombie, "fX");
    var fFY = F(zombie, "fY");
    var fRDirection = F(zombie, "rDirection");
    var fBoard = F(zombie, "board");
    var fPassablePoint = F(zombie, "passablePoint");

    var fStatsValue = F(statsType, "value");
    var fBoardConfig = F(boardType, "boardConfig");
    var fPlantBottom = F(gridType, "plant_bottom");
    var fPlantCommon = F(gridType, "plant_common");
    var fPlantSheath = F(gridType, "plant_sheath");

    var findBuff = M(buffManagerType, "FindBuff", 1);
    var resetMoveSpeed = M(zombie, "ResetMoveSpeed", 0);
    var poleTestJump = M(zombie, "ZC_PoleTestJump", 0);
    var isDisabled = M(zombie, "IsDisabled", 0);
    var ladderPlaceC4 = M(zombie, "ZC_LadderSaboteursPlaceC4", 0);
    var ladderBoom = M(zombie, "ZC_LadderSaboteursBoom", 0);
    var ladderTestPlace = M(zombie, "ZC_LadderTestPlace", 0);
    var imperialVanguard = M(zombie, "ZC_ImperialVanguardHarbinger", 0);
    var getGridX = M(boardConfigType, "GetGridX", 2);
    var getGridY = M(boardConfigType, "GetGridY", 2);
    var getGrid = M(boardType, "GetGrid", 2);
    var getPassablePoint = M(gridType, "GetPassablePoint", 0);

    var deltaTime = Ref("UnityEngine.Time", "get_deltaTime");
    var setTrigger = Ref("UnityEngine.Animator", "SetTrigger", "System.String");
    var setBool = Ref("UnityEngine.Animator", "SetBool", "System.String", "System.Boolean");
    var objectImplicit = Ref("UnityEngine.Object", "op_Implicit", "UnityEngine.Object");
    var objectNeq = Ref("UnityEngine.Object", "op_Inequality", "UnityEngine.Object", "UnityEngine.Object");

    var vec3 = fRDirection.FieldType;
    var vecX = new FieldReference("x", module.TypeSystem.Single, vec3);
    var vecY = new FieldReference("y", module.TypeSystem.Single, vec3);

    var b = new MethodBody(m) { InitLocals = true, MaxStackSize = 8 };
    m.Body = b;
    var il = b.GetILProcessor();

    var buffLocal = new VariableDefinition(buffType);
    var statsLocal = new VariableDefinition(statsType);
    var tempFloat = new VariableDefinition(module.TypeSystem.Single);
    var pointX = new VariableDefinition(module.TypeSystem.Single);
    var pointY = new VariableDefinition(module.TypeSystem.Single);
    var gridX = new VariableDefinition(module.TypeSystem.Int32);
    var gridY = new VariableDefinition(module.TypeSystem.Int32);
    var gridLocal = new VariableDefinition(gridType);
    var passable = new VariableDefinition(module.TypeSystem.Int32);
    var chosenPlant = new VariableDefinition(plantType);
    foreach (var v in new[] { buffLocal, statsLocal, tempFloat, pointX, pointY, gridX, gridY, gridLocal, passable, chosenPlant }) b.Variables.Add(v);

    var poleTest = Instruction.Create(OpCodes.Nop);
    var id13 = Instruction.Create(OpCodes.Nop);
    var statsInc = Instruction.Create(OpCodes.Nop);
    var statsReset = Instruction.Create(OpCodes.Nop);
    var id16 = Instruction.Create(OpCodes.Nop);
    var restUpdate = Instruction.Create(OpCodes.Nop);
    var id18 = Instruction.Create(OpCodes.Nop);
    var skipC4 = Instruction.Create(OpCodes.Nop);
    var afterCountdown = Instruction.Create(OpCodes.Nop);
    var id23 = Instruction.Create(OpCodes.Nop);
    var plantFallback = Instruction.Create(OpCodes.Nop);
    var useSheath = Instruction.Create(OpCodes.Nop);
    var useCommon = Instruction.Create(OpCodes.Nop);
    var useBottom = Instruction.Create(OpCodes.Nop);
    var plantChosen = Instruction.Create(OpCodes.Nop);
    var jump = Instruction.Create(OpCodes.Nop);
    var ret = Instruction.Create(OpCodes.Ret);

    // ID 3 / Pole Commander. Native has intentionally asymmetric early exits.
    E(il, OpCodes.Ldarg_0); E(il, OpCodes.Ldfld, fID); E(il, OpCodes.Ldc_I4_3); E(il, OpCodes.Bne_Un, id13);
    E(il, OpCodes.Ldarg_0); E(il, OpCodes.Ldfld, fPole); E(il, OpCodes.Brfalse, poleTest);
    E(il, OpCodes.Ldarg_0); E(il, OpCodes.Ldfld, fIsStant); E(il, OpCodes.Brtrue, id13);
    E(il, OpCodes.Ldarg_0); E(il, OpCodes.Ldfld, fBuffManager); E(il, OpCodes.Ldstr, "PoleCommander.speed"); E(il, OpCodes.Callvirt, findBuff); E(il, OpCodes.Stloc, buffLocal);
    E(il, OpCodes.Ldloc, buffLocal); E(il, OpCodes.Brfalse, id13);
    E(il, OpCodes.Ldloc, buffLocal); E(il, OpCodes.Isinst, statsType); E(il, OpCodes.Stloc, statsLocal);
    E(il, OpCodes.Ldloc, statsLocal); E(il, OpCodes.Brfalse, poleTest);
    E(il, OpCodes.Ldloc, statsLocal); E(il, OpCodes.Ldfld, fStatsValue); E(il, OpCodes.Ldc_R4, 1.5f); E(il, OpCodes.Blt, statsInc);
    E(il, OpCodes.Br, statsReset);
    il.Append(statsInc);
    E(il, OpCodes.Ldloc, statsLocal);
    E(il, OpCodes.Ldloc, statsLocal); E(il, OpCodes.Ldfld, fStatsValue);
    E(il, OpCodes.Call, deltaTime); E(il, OpCodes.Ldc_R4, 0.2f); E(il, OpCodes.Mul);
    E(il, OpCodes.Ldarg_0); E(il, OpCodes.Ldfld, fUpdateRate); E(il, OpCodes.Mul); E(il, OpCodes.Add);
    E(il, OpCodes.Stfld, fStatsValue);
    il.Append(statsReset);
    E(il, OpCodes.Ldarg_0); E(il, OpCodes.Call, resetMoveSpeed);

    il.Append(poleTest);
    E(il, OpCodes.Ldarg_0); E(il, OpCodes.Call, poleTestJump); E(il, OpCodes.Brfalse, id13);
    E(il, OpCodes.Br, jump);

    // ID 13 / Snowbeast rest timer.
    il.Append(id13);
    E(il, OpCodes.Ldarg_0); E(il, OpCodes.Ldfld, fID); E(il, OpCodes.Ldc_I4, 13); E(il, OpCodes.Bne_Un, id16);
    E(il, OpCodes.Ldarg_0); E(il, OpCodes.Ldfld, fSnowRest); E(il, OpCodes.Brtrue, restUpdate);
    E(il, OpCodes.Ldarg_0); E(il, OpCodes.Ldfld, fIsStant); E(il, OpCodes.Brfalse, id16);
    il.Append(restUpdate);
    E(il, OpCodes.Ldarg_0); E(il, OpCodes.Ldfld, fSnowRestTime); E(il, OpCodes.Call, deltaTime); E(il, OpCodes.Add); E(il, OpCodes.Stloc, tempFloat);
    E(il, OpCodes.Ldarg_0); E(il, OpCodes.Ldloc, tempFloat); E(il, OpCodes.Stfld, fSnowRestTime);
    // Native JB after COMISS(newRest,10): less-than OR unordered keeps resting.
    E(il, OpCodes.Ldloc, tempFloat); E(il, OpCodes.Ldc_R4, 10f); E(il, OpCodes.Blt_Un, id16);
    E(il, OpCodes.Ldarg_0); E(il, OpCodes.Ldc_R4, 10f); E(il, OpCodes.Stfld, fSnowRestTime);
    E(il, OpCodes.Ldarg_0); E(il, OpCodes.Ldc_I4_0); E(il, OpCodes.Stfld, fSnowRest);
    E(il, OpCodes.Ldarg_0); E(il, OpCodes.Ldfld, fAnimator); E(il, OpCodes.Ldstr, "rest"); E(il, OpCodes.Ldc_I4_0); E(il, OpCodes.Callvirt, setBool);

    // ID 16 / Ladder Saboteur.
    il.Append(id16);
    E(il, OpCodes.Ldarg_0); E(il, OpCodes.Ldfld, fID); E(il, OpCodes.Ldc_I4, 16); E(il, OpCodes.Bne_Un, id18);
    E(il, OpCodes.Ldarg_0); E(il, OpCodes.Ldfld, fC4); E(il, OpCodes.Ldc_I4_0); E(il, OpCodes.Ble, skipC4);
    E(il, OpCodes.Ldarg_0); E(il, OpCodes.Call, isDisabled); E(il, OpCodes.Brtrue, skipC4);
    E(il, OpCodes.Ldarg_0); E(il, OpCodes.Call, ladderPlaceC4);
    il.Append(skipC4);
    // Initial countdown > 0 only when ordered; NaN skips the countdown body.
    E(il, OpCodes.Ldarg_0); E(il, OpCodes.Ldfld, fDetonation); E(il, OpCodes.Ldc_R4, 0f); E(il, OpCodes.Ble_Un, afterCountdown);
    E(il, OpCodes.Ldarg_0); E(il, OpCodes.Call, isDisabled); E(il, OpCodes.Brtrue, afterCountdown);
    E(il, OpCodes.Ldarg_0); E(il, OpCodes.Ldfld, fDetonation);
    E(il, OpCodes.Call, deltaTime); E(il, OpCodes.Ldarg_0); E(il, OpCodes.Ldfld, fUpdateRate); E(il, OpCodes.Mul); E(il, OpCodes.Sub); E(il, OpCodes.Stloc, tempFloat);
    E(il, OpCodes.Ldarg_0); E(il, OpCodes.Ldloc, tempFloat); E(il, OpCodes.Stfld, fDetonation);
    // Native JB after COMISS(0,newCountdown): >0 OR unordered keeps countdown.
    E(il, OpCodes.Ldloc, tempFloat); E(il, OpCodes.Ldc_R4, 0f); E(il, OpCodes.Bgt_Un, afterCountdown);
    E(il, OpCodes.Ldarg_0); E(il, OpCodes.Ldc_R4, 0f); E(il, OpCodes.Stfld, fDetonation);
    E(il, OpCodes.Ldarg_0); E(il, OpCodes.Call, ladderBoom);
    il.Append(afterCountdown);
    E(il, OpCodes.Ldarg_0); E(il, OpCodes.Ldfld, fLadderPlace); E(il, OpCodes.Brtrue, id18);
    E(il, OpCodes.Ldarg_0); E(il, OpCodes.Call, ladderTestPlace); E(il, OpCodes.Brfalse, id18);
    E(il, OpCodes.Ldarg_0); E(il, OpCodes.Ldc_I4_1); E(il, OpCodes.Stfld, fLadderPlace);
    E(il, OpCodes.Ldarg_0); E(il, OpCodes.Ldfld, fAnimator); E(il, OpCodes.Ldstr, "PlaceTrigger"); E(il, OpCodes.Callvirt, setTrigger);

    // ID 18 special characteristic.
    il.Append(id18);
    E(il, OpCodes.Ldarg_0); E(il, OpCodes.Ldfld, fID); E(il, OpCodes.Ldc_I4, 18); E(il, OpCodes.Bne_Un, id23);
    E(il, OpCodes.Ldarg_0); E(il, OpCodes.Call, imperialVanguard);

    // ID 23 inlines ZC_PoleTestJump + ZC_PoleJump in the PC build.
    il.Append(id23);
    E(il, OpCodes.Ldarg_0); E(il, OpCodes.Ldfld, fID); E(il, OpCodes.Ldc_I4, 23); E(il, OpCodes.Bne_Un, ret);
    E(il, OpCodes.Ldarg_0); E(il, OpCodes.Ldfld, fPole); E(il, OpCodes.Brfalse, ret);
    E(il, OpCodes.Ldarg_0); E(il, OpCodes.Ldfld, fJump); E(il, OpCodes.Brtrue, ret);
    E(il, OpCodes.Ldarg_0); E(il, OpCodes.Ldfld, fIsStant); E(il, OpCodes.Brtrue, ret);
    E(il, OpCodes.Ldarg_0); E(il, OpCodes.Call, isDisabled); E(il, OpCodes.Brtrue, ret);

    // point = (fX - rDirection.x*134, fY - rDirection.y*134), matching the native sign-mask multiply path.
    E(il, OpCodes.Ldarg_0); E(il, OpCodes.Ldfld, fFX);
    E(il, OpCodes.Ldarg_0); E(il, OpCodes.Ldflda, fRDirection); E(il, OpCodes.Ldfld, vecX); E(il, OpCodes.Ldc_R4, 134f); E(il, OpCodes.Mul); E(il, OpCodes.Sub); E(il, OpCodes.Stloc, pointX);
    E(il, OpCodes.Ldarg_0); E(il, OpCodes.Ldfld, fFY);
    E(il, OpCodes.Ldarg_0); E(il, OpCodes.Ldflda, fRDirection); E(il, OpCodes.Ldfld, vecY); E(il, OpCodes.Ldc_R4, 134f); E(il, OpCodes.Mul); E(il, OpCodes.Sub); E(il, OpCodes.Stloc, pointY);

    // Native re-reads board and boardConfig independently for X and Y.
    E(il, OpCodes.Ldarg_0); E(il, OpCodes.Ldfld, fBoard); E(il, OpCodes.Ldfld, fBoardConfig);
    E(il, OpCodes.Ldloc, pointX); E(il, OpCodes.Ldloc, pointY); E(il, OpCodes.Callvirt, getGridX); E(il, OpCodes.Stloc, gridX);
    E(il, OpCodes.Ldarg_0); E(il, OpCodes.Ldfld, fBoard); E(il, OpCodes.Ldfld, fBoardConfig);
    E(il, OpCodes.Ldloc, pointX); E(il, OpCodes.Ldloc, pointY); E(il, OpCodes.Callvirt, getGridY); E(il, OpCodes.Stloc, gridY);
    E(il, OpCodes.Ldarg_0); E(il, OpCodes.Ldfld, fBoard); E(il, OpCodes.Ldloc, gridX); E(il, OpCodes.Ldloc, gridY); E(il, OpCodes.Callvirt, getGrid); E(il, OpCodes.Stloc, gridLocal);
    E(il, OpCodes.Ldloc, gridLocal); E(il, OpCodes.Brfalse, ret);
    E(il, OpCodes.Ldloc, gridLocal); E(il, OpCodes.Callvirt, getPassablePoint); E(il, OpCodes.Stloc, passable);

    // Native converts both passable ints to float before comparison.
    E(il, OpCodes.Ldloc, passable); E(il, OpCodes.Conv_R4); E(il, OpCodes.Ldc_R4, 100f); E(il, OpCodes.Ble, plantFallback);
    E(il, OpCodes.Ldarg_0); E(il, OpCodes.Ldfld, fPassablePoint); E(il, OpCodes.Conv_R4);
    E(il, OpCodes.Ldloc, passable); E(il, OpCodes.Conv_R4); E(il, OpCodes.Bge, jump);

    il.Append(plantFallback);
    E(il, OpCodes.Ldnull); E(il, OpCodes.Stloc, chosenPlant);
    E(il, OpCodes.Ldloc, gridLocal); E(il, OpCodes.Ldfld, fPlantSheath); E(il, OpCodes.Call, objectImplicit); E(il, OpCodes.Brtrue, useSheath);
    E(il, OpCodes.Ldloc, gridLocal); E(il, OpCodes.Ldfld, fPlantCommon); E(il, OpCodes.Call, objectImplicit); E(il, OpCodes.Brtrue, useCommon);
    E(il, OpCodes.Ldloc, gridLocal); E(il, OpCodes.Ldfld, fPlantBottom); E(il, OpCodes.Call, objectImplicit); E(il, OpCodes.Brtrue, useBottom);
    E(il, OpCodes.Br, plantChosen);
    il.Append(useSheath); E(il, OpCodes.Ldloc, gridLocal); E(il, OpCodes.Ldfld, fPlantSheath); E(il, OpCodes.Stloc, chosenPlant); E(il, OpCodes.Br, plantChosen);
    il.Append(useCommon); E(il, OpCodes.Ldloc, gridLocal); E(il, OpCodes.Ldfld, fPlantCommon); E(il, OpCodes.Stloc, chosenPlant); E(il, OpCodes.Br, plantChosen);
    il.Append(useBottom); E(il, OpCodes.Ldloc, gridLocal); E(il, OpCodes.Ldfld, fPlantBottom); E(il, OpCodes.Stloc, chosenPlant);
    il.Append(plantChosen);
    E(il, OpCodes.Ldloc, chosenPlant); E(il, OpCodes.Ldnull); E(il, OpCodes.Call, objectNeq); E(il, OpCodes.Brfalse, ret);

    il.Append(jump);
    E(il, OpCodes.Ldarg_0); E(il, OpCodes.Ldc_I4_1); E(il, OpCodes.Stfld, fJump);
    E(il, OpCodes.Ldarg_0); E(il, OpCodes.Ldfld, fAnimator); E(il, OpCodes.Ldstr, "JumpTrigger"); E(il, OpCodes.Callvirt, setTrigger);
    il.Append(ret);
}
