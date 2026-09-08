using Mono.Cecil;
using Mono.Cecil.Cil;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

if (args.Length != 2)
{
    Console.Error.WriteLine("Usage: GodsPVZ.HF6Patch <input.dll> <output.dll>");
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
IEnumerable<MethodReference> AllRefs() => All(module.Types).SelectMany(t => t.Methods).Where(m => m.HasBody).SelectMany(m => m.Body.Instructions).Select(i => i.Operand).OfType<MethodReference>();
MethodReference Ref(string decl, string name, params string[] parameterTypes)
{
    var q = AllRefs().Where(m => m.DeclaringType.FullName == decl && m.Name == name && m.Parameters.Count == parameterTypes.Length)
        .Where(m => m.Parameters.Select(p => p.ParameterType.FullName).SequenceEqual(parameterTypes)).ToList();
    if (q.Count == 0) throw new InvalidDataException($"Missing MethodRef {decl}::{name}({string.Join(',', parameterTypes)})");
    return q[0];
}
GenericInstanceMethod GRef(string decl, string name, string genericName)
{
    var q = AllRefs().OfType<GenericInstanceMethod>().Where(m => m.DeclaringType.FullName == decl && m.Name == name && m.GenericArguments.Count == 1)
        .Where(m => m.GenericArguments[0].Name == genericName || m.GenericArguments[0].FullName == genericName).ToList();
    if (q.Count == 0) throw new InvalidDataException($"Missing Generic MethodRef {decl}::{name}<{genericName}>");
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
var elementManager = T("ElementManager");
var element = T("Element");
var board = T("Board");

PatchUpdate();

Directory.CreateDirectory(Path.GetDirectoryName(output)!);
module.Write(output, new WriterParameters { WriteSymbols = false });

using (var verify = ModuleDefinition.ReadModule(output, new ReaderParameters { InMemory = true, ReadSymbols = false }))
{
    var z = All(verify.Types).Single(t => t.Name == "Zombie");
    var m = z.Methods.Single(x => x.Name == "Update" && x.Parameters.Count == 0);
    if (m.MetadataToken.ToUInt32() != 0x06000421) throw new InvalidDataException("HF6 Zombie.Update token drift");
    if (!m.HasBody || m.Body.Instructions.Count < 220) throw new InvalidDataException($"HF6 Zombie.Update too small: {m.Body.Instructions.Count}");
    if (m.Body.Instructions.Select(i => i.Operand).OfType<MethodReference>().Any(r => r.DeclaringType.FullName.Contains("Cpp2ILHelpers", StringComparison.Ordinal)))
        throw new InvalidDataException("HF6 Zombie.Update still contains Cpp2IL helper calls");
    var strings = m.Body.Instructions.Where(i => i.OpCode == OpCodes.Ldstr).Select(i => (string)i.Operand).ToHashSet();
    if (!strings.SetEquals(new[] { "Entity" })) throw new InvalidDataException("HF6 Zombie.Update native string set mismatch");
    var calls = m.Body.Instructions.Select(i => i.Operand).OfType<MethodReference>().Select(r => r.Name).ToHashSet();
    foreach (var c in new[] { "GetElement", "Ceiling", "Max", "ResetUpdateRate", "BoardRuntime", "GetMoveDirection", "SetrSpeed", "TestPosition", "IsDisabled", "Path_Test", "Path_Finding", "TranToWalk", "AddComponent", "Update_Attack", "Update_Characteristic", "Update", "InjuryStatusUpdate_Body", "Update_Brightness", "Update_PreviousPosition", "DestroyZombie" })
        if (!calls.Contains(c)) throw new InvalidDataException($"HF6 Zombie.Update missing native call {c}");
    var deltaCalls = m.Body.Instructions.Count(i => i.Operand is MethodReference r && r.DeclaringType.FullName == "UnityEngine.Time" && r.Name == "get_deltaTime");
    if (deltaCalls != 8) throw new InvalidDataException($"HF6 expected 8 Time.deltaTime call sites, got {deltaCalls}");
    if (!m.Body.Instructions.Any(i => i.OpCode == OpCodes.Ldc_R4 && i.Operand is float f && f == -10f)) throw new InvalidDataException("HF6 missing native sorting multiplier -10");
    if (!m.Body.Instructions.Any(i => i.OpCode == OpCodes.Conv_I4)) throw new InvalidDataException("HF6 missing native cvttss2si equivalent conv.i4");
    if (m.Body.ExceptionHandlers.Count != 0) throw new InvalidDataException("HF6 Zombie.Update unexpectedly contains exception handlers");
    Console.WriteLine($"VERIFY Zombie.Update: {m.Body.Instructions.Count} IL, {m.Body.CodeSize} bytes, deltaTimeCalls={deltaCalls}");
}

Console.WriteLine("HF6 Zombie.Update @0x18036CD60 restored from PC native x86-64");
Console.WriteLine($"WROTE {output} ({new FileInfo(output).Length} bytes)");
return 0;

void PatchUpdate()
{
    var m = M(zombie, "Update", 0);
    if (m.MetadataToken.ToUInt32() != 0x06000421) throw new InvalidDataException("Unexpected Zombie.Update token");

    var fBoard = F(zombie, "board");
    var fX = F(zombie, "fX");
    var fY = F(zombie, "fY");
    var fZ = F(zombie, "fZ");
    var fDied = F(zombie, "isDied");
    var fStant = F(zombie, "isStant");
    var fOnBoard = F(zombie, "isOnBoard");
    var fImpactCd = F(zombie, "snowbeast_impactCD");
    var fElementManager = F(zombie, "elementManager");
    var fLivingTime = F(zombie, "livingTime");
    var fWaitingTime = F(zombie, "waitingTime");
    var fDestroyTicking = F(zombie, "destroyTicking");
    var fRSpeed = F(zombie, "rSpeed");
    var fSortingGroup = F(zombie, "sortingGroup");
    var fAnimationGroup = F(zombie, "animationGroup");
    var fBuffManager = F(zombie, "buffManager");
    var fStiffness = F(zombie, "stiffnessTime");
    var fUpdateRate = F(zombie, "updateRate");

    var fGameStart = F(board, "gameStart");
    var fPoint = F(element, "point");
    var fBurstTime = F(element, "burstTime");

    var vec3 = fRSpeed.FieldType;
    var vecX = new FieldReference("x", module.TypeSystem.Single, vec3);
    var vecY = new FieldReference("y", module.TypeSystem.Single, vec3);
    var vecZ = new FieldReference("z", module.TypeSystem.Single, vec3);

    var getElement = M(elementManager, "GetElement", 1);
    var elementUpdate = M(elementManager, "Update", 0);
    var boardRuntime = M(board, "BoardRuntime", 0);
    var resetUpdateRate = M(zombie, "ResetUpdateRate", 1);
    var getMoveDirection = M(zombie, "GetMoveDirection", 0);
    var setrSpeed = M(zombie, "SetrSpeed", 1);
    var testPosition = M(zombie, "TestPosition", 2);
    var isDisabled = M(zombie, "IsDisabled", 0);
    var pathTest = M(zombie, "Path_Test", 0);
    var pathFinding = M(zombie, "Path_Finding", 0);
    var tranToWalk = M(zombie, "TranToWalk", 0);
    var updateAttack = M(zombie, "Update_Attack", 0);
    var updateCharacteristic = M(zombie, "Update_Characteristic", 0);
    var injury = M(zombie, "InjuryStatusUpdate_Body", 1);
    var updateBrightness = M(zombie, "Update_Brightness", 0);
    var updatePrevious = M(zombie, "Update_PreviousPosition", 0);
    var destroyZombie = M(zombie, "DestroyZombie", 0);

    var deltaTime = Ref("UnityEngine.Time", "get_deltaTime");
    var ceiling = Ref("System.MathF", "Ceiling", "System.Single");
    var maxFloat = Ref("System.Math", "Max", "System.Single", "System.Single");
    var componentGetTransform = Ref("UnityEngine.Component", "get_transform");
    var componentGetGameObject = Ref("UnityEngine.Component", "get_gameObject");
    var gameObjectGetTransform = Ref("UnityEngine.GameObject", "get_transform");
    var transformGetPosition = Ref("UnityEngine.Transform", "get_position");
    var transformSetPosition = Ref("UnityEngine.Transform", "set_position", "UnityEngine.Vector3");
    var objectEquality = Ref("UnityEngine.Object", "op_Equality", "UnityEngine.Object", "UnityEngine.Object");
    var addSortingGroup = GRef("UnityEngine.GameObject", "AddComponent", "SortingGroup");
    var setSortingLayer = Ref("UnityEngine.Rendering.SortingGroup", "set_sortingLayerName", "System.String");
    var setSortingOrder = Ref("UnityEngine.Rendering.SortingGroup", "set_sortingOrder", "System.Int32");
    var buffUpdate = GRef("BuffManager", "Update", "Zombie");

    var b = new MethodBody(m) { InitLocals = true, MaxStackSize = 12 };
    m.Body = b;
    var il = b.GetILProcessor();

    var desired = new VariableDefinition(module.TypeSystem.Single);
    var elem = new VariableDefinition(element);
    var move = new VariableDefinition(vec3);
    var rx = new VariableDefinition(module.TypeSystem.Single);
    var ry = new VariableDefinition(module.TypeSystem.Single);
    var dtx = new VariableDefinition(module.TypeSystem.Single);
    var dty = new VariableDefinition(module.TypeSystem.Single);
    var newX = new VariableDefinition(module.TypeSystem.Single);
    var newY = new VariableDefinition(module.TypeSystem.Single);
    var pos = new VariableDefinition(vec3);
    var pos2 = new VariableDefinition(vec3);
    var boardLocal = new VariableDefinition(board);
    foreach (var v in new[] { desired, elem, move, rx, ry, dtx, dty, newX, newY, pos, pos2, boardLocal }) b.Variables.Add(v);

    var desiredZero = Instruction.Create(OpCodes.Nop);
    var desiredCompare = Instruction.Create(OpCodes.Nop);
    var afterReset = Instruction.Create(OpCodes.Nop);
    var afterStiff = Instruction.Create(OpCodes.Nop);
    var afterImpact = Instruction.Create(OpCodes.Nop);
    var afterTransform = Instruction.Create(OpCodes.Nop);
    var waitingPath = Instruction.Create(OpCodes.Nop);
    var afterPath = Instruction.Create(OpCodes.Nop);
    var haveSorting = Instruction.Create(OpCodes.Nop);
    var skipCombat = Instruction.Create(OpCodes.Nop);
    var tail = Instruction.Create(OpCodes.Nop);
    var afterDestroy = Instruction.Create(OpCodes.Nop);

    // Native desired update-rate calculation. stiffnessTime > 0 is ordered-only; NaN follows the element path.
    E(il, OpCodes.Ldarg_0); E(il, OpCodes.Ldfld, fStiffness); E(il, OpCodes.Ldc_R4, 0f); E(il, OpCodes.Bgt, desiredZero);
    E(il, OpCodes.Ldarg_0); E(il, OpCodes.Ldfld, fElementManager); E(il, OpCodes.Ldc_I4_0); E(il, OpCodes.Callvirt, getElement); E(il, OpCodes.Stloc, elem);
    E(il, OpCodes.Ldc_R4, 1f); E(il, OpCodes.Stloc, desired);
    E(il, OpCodes.Ldloc, elem); E(il, OpCodes.Brfalse, desiredCompare);
    // point <= 0 OR unordered => retain 1.0.
    E(il, OpCodes.Ldloc, elem); E(il, OpCodes.Ldfld, fPoint); E(il, OpCodes.Ldc_R4, 0f); E(il, OpCodes.Ble_Un, desiredCompare);
    // burstTime > 0 ordered => desired 0. NaN continues into the finite point calculation, matching COMISS/JA.
    E(il, OpCodes.Ldloc, elem); E(il, OpCodes.Ldfld, fBurstTime); E(il, OpCodes.Ldc_R4, 0f); E(il, OpCodes.Bgt, desiredZero);
    // desired = Max(0.05, 1 - Ceiling(point / 1000) * 0.05)
    E(il, OpCodes.Ldc_R4, 0.05f);
    E(il, OpCodes.Ldc_R4, 1f);
    E(il, OpCodes.Ldloc, elem); E(il, OpCodes.Ldfld, fPoint); E(il, OpCodes.Ldc_R4, 1000f); E(il, OpCodes.Div);
    E(il, OpCodes.Call, ceiling); E(il, OpCodes.Ldc_R4, 0.05f); E(il, OpCodes.Mul); E(il, OpCodes.Sub);
    E(il, OpCodes.Call, maxFloat); E(il, OpCodes.Stloc, desired);

    il.Append(desiredCompare);
    // UCOMISS equality: NaN is deliberately treated as different and therefore resets.
    E(il, OpCodes.Ldarg_0); E(il, OpCodes.Ldfld, fUpdateRate); E(il, OpCodes.Ldloc, desired); E(il, OpCodes.Beq, afterReset);
    E(il, OpCodes.Ldarg_0); E(il, OpCodes.Ldloc, desired); E(il, OpCodes.Call, resetUpdateRate); E(il, OpCodes.Br, afterReset);

    il.Append(desiredZero);
    E(il, OpCodes.Ldarg_0); E(il, OpCodes.Ldfld, fUpdateRate); E(il, OpCodes.Ldc_R4, 0f); E(il, OpCodes.Beq, afterReset);
    E(il, OpCodes.Ldarg_0); E(il, OpCodes.Ldc_R4, 0f); E(il, OpCodes.Call, resetUpdateRate);

    il.Append(afterReset);
    // Main board-runtime block.
    E(il, OpCodes.Ldarg_0); E(il, OpCodes.Ldfld, fOnBoard); E(il, OpCodes.Brfalse, tail);
    E(il, OpCodes.Ldarg_0); E(il, OpCodes.Ldfld, fBoard); E(il, OpCodes.Stloc, boardLocal);
    E(il, OpCodes.Ldloc, boardLocal); E(il, OpCodes.Ldfld, fGameStart); E(il, OpCodes.Brfalse, tail);
    E(il, OpCodes.Ldloc, boardLocal); E(il, OpCodes.Callvirt, boardRuntime); E(il, OpCodes.Brfalse, tail);

    // livingTime += Time.deltaTime
    E(il, OpCodes.Ldarg_0); E(il, OpCodes.Ldarg_0); E(il, OpCodes.Ldfld, fLivingTime); E(il, OpCodes.Call, deltaTime); E(il, OpCodes.Add); E(il, OpCodes.Stfld, fLivingTime);

    // stiffnessTime countdown: clamp only ordered <= 0; NaN remains NaN as in COMISS/JB.
    E(il, OpCodes.Ldarg_0); E(il, OpCodes.Ldfld, fStiffness); E(il, OpCodes.Ldc_R4, 0f); E(il, OpCodes.Ble_Un, afterStiff);
    E(il, OpCodes.Ldarg_0); E(il, OpCodes.Ldarg_0); E(il, OpCodes.Ldfld, fStiffness); E(il, OpCodes.Call, deltaTime); E(il, OpCodes.Sub); E(il, OpCodes.Stfld, fStiffness);
    E(il, OpCodes.Ldarg_0); E(il, OpCodes.Ldfld, fStiffness); E(il, OpCodes.Ldc_R4, 0f); E(il, OpCodes.Bgt_Un, afterStiff);
    E(il, OpCodes.Ldarg_0); E(il, OpCodes.Ldc_R4, 0f); E(il, OpCodes.Stfld, fStiffness);
    il.Append(afterStiff);

    // snowbeast_impactCD countdown, same ordered/unordered behavior.
    E(il, OpCodes.Ldarg_0); E(il, OpCodes.Ldfld, fImpactCd); E(il, OpCodes.Ldc_R4, 0f); E(il, OpCodes.Ble_Un, afterImpact);
    E(il, OpCodes.Ldarg_0); E(il, OpCodes.Ldarg_0); E(il, OpCodes.Ldfld, fImpactCd); E(il, OpCodes.Call, deltaTime); E(il, OpCodes.Sub); E(il, OpCodes.Stfld, fImpactCd);
    E(il, OpCodes.Ldarg_0); E(il, OpCodes.Ldfld, fImpactCd); E(il, OpCodes.Ldc_R4, 0f); E(il, OpCodes.Bgt_Un, afterImpact);
    E(il, OpCodes.Ldarg_0); E(il, OpCodes.Ldc_R4, 0f); E(il, OpCodes.Stfld, fImpactCd);
    il.Append(afterImpact);

    // GetMoveDirection(); SetrSpeed(direction)
    E(il, OpCodes.Ldarg_0); E(il, OpCodes.Call, getMoveDirection); E(il, OpCodes.Stloc, move);
    E(il, OpCodes.Ldarg_0); E(il, OpCodes.Ldloc, move); E(il, OpCodes.Call, setrSpeed);

    // Native makes two independent Time.deltaTime calls, then stores fY before fX.
    E(il, OpCodes.Ldarg_0); E(il, OpCodes.Ldflda, fRSpeed); E(il, OpCodes.Ldfld, vecX); E(il, OpCodes.Stloc, rx);
    E(il, OpCodes.Call, deltaTime); E(il, OpCodes.Stloc, dtx);
    E(il, OpCodes.Ldarg_0); E(il, OpCodes.Ldflda, fRSpeed); E(il, OpCodes.Ldfld, vecY); E(il, OpCodes.Stloc, ry);
    E(il, OpCodes.Call, deltaTime); E(il, OpCodes.Stloc, dty);
    E(il, OpCodes.Ldarg_0); E(il, OpCodes.Ldfld, fY); E(il, OpCodes.Ldloc, dty); E(il, OpCodes.Ldloc, ry); E(il, OpCodes.Mul); E(il, OpCodes.Add); E(il, OpCodes.Stloc, newY);
    E(il, OpCodes.Ldarg_0); E(il, OpCodes.Ldfld, fX); E(il, OpCodes.Ldloc, dtx); E(il, OpCodes.Ldloc, rx); E(il, OpCodes.Mul); E(il, OpCodes.Add); E(il, OpCodes.Stloc, newX);
    E(il, OpCodes.Ldarg_0); E(il, OpCodes.Ldloc, newY); E(il, OpCodes.Stfld, fY);
    E(il, OpCodes.Ldarg_0); E(il, OpCodes.Ldloc, newX); E(il, OpCodes.Stfld, fX);
    E(il, OpCodes.Ldarg_0); E(il, OpCodes.Ldloc, newX); E(il, OpCodes.Ldloc, newY); E(il, OpCodes.Call, testPosition);

    // if (!isDied): base.transform.position = (fX,fY,oldZ); animationGroup.transform.position = (fX,fY+fZ,baseOldZ)
    E(il, OpCodes.Ldarg_0); E(il, OpCodes.Ldfld, fDied); E(il, OpCodes.Brtrue, afterTransform);
    E(il, OpCodes.Ldarg_0); E(il, OpCodes.Call, componentGetTransform); E(il, OpCodes.Callvirt, transformGetPosition); E(il, OpCodes.Stloc, pos);
    E(il, OpCodes.Ldloca, pos); E(il, OpCodes.Ldarg_0); E(il, OpCodes.Ldfld, fX); E(il, OpCodes.Stfld, vecX);
    E(il, OpCodes.Ldloca, pos); E(il, OpCodes.Ldarg_0); E(il, OpCodes.Ldfld, fY); E(il, OpCodes.Stfld, vecY);
    E(il, OpCodes.Ldarg_0); E(il, OpCodes.Call, componentGetTransform); E(il, OpCodes.Ldloc, pos); E(il, OpCodes.Callvirt, transformSetPosition);
    E(il, OpCodes.Ldarg_0); E(il, OpCodes.Call, componentGetTransform); E(il, OpCodes.Callvirt, transformGetPosition); E(il, OpCodes.Stloc, pos2);
    E(il, OpCodes.Ldloca, pos2); E(il, OpCodes.Ldarg_0); E(il, OpCodes.Ldfld, fX); E(il, OpCodes.Stfld, vecX);
    E(il, OpCodes.Ldloca, pos2); E(il, OpCodes.Ldarg_0); E(il, OpCodes.Ldfld, fY); E(il, OpCodes.Ldarg_0); E(il, OpCodes.Ldfld, fZ); E(il, OpCodes.Add); E(il, OpCodes.Stfld, vecY);
    E(il, OpCodes.Ldarg_0); E(il, OpCodes.Ldfld, fAnimationGroup); E(il, OpCodes.Call, gameObjectGetTransform); E(il, OpCodes.Ldloc, pos2); E(il, OpCodes.Callvirt, transformSetPosition);
    il.Append(afterTransform);

    // Path state machine.
    E(il, OpCodes.Ldarg_0); E(il, OpCodes.Call, isDisabled); E(il, OpCodes.Brtrue, afterPath);
    E(il, OpCodes.Ldarg_0); E(il, OpCodes.Ldfld, fStant); E(il, OpCodes.Brtrue, waitingPath);
    E(il, OpCodes.Ldarg_0); E(il, OpCodes.Call, pathTest); E(il, OpCodes.Brfalse, afterPath);
    E(il, OpCodes.Ldarg_0); E(il, OpCodes.Call, pathFinding); E(il, OpCodes.Pop); E(il, OpCodes.Br, afterPath);

    il.Append(waitingPath);
    // Initial NaN / <=0 skips waiting logic; after subtraction NaN or >0 also skips.
    E(il, OpCodes.Ldarg_0); E(il, OpCodes.Ldfld, fWaitingTime); E(il, OpCodes.Ldc_R4, 0f); E(il, OpCodes.Ble_Un, afterPath);
    E(il, OpCodes.Ldarg_0); E(il, OpCodes.Ldarg_0); E(il, OpCodes.Ldfld, fWaitingTime); E(il, OpCodes.Call, deltaTime); E(il, OpCodes.Sub); E(il, OpCodes.Stfld, fWaitingTime);
    E(il, OpCodes.Ldarg_0); E(il, OpCodes.Ldfld, fWaitingTime); E(il, OpCodes.Ldc_R4, 0f); E(il, OpCodes.Bgt_Un, afterPath);
    E(il, OpCodes.Ldarg_0); E(il, OpCodes.Call, pathFinding); E(il, OpCodes.Brtrue, haveSorting);
    E(il, OpCodes.Ldarg_0); E(il, OpCodes.Call, deltaTime); E(il, OpCodes.Stfld, fWaitingTime); E(il, OpCodes.Br, afterPath);
    // Native success path calls TranToWalk before sorting.
    il.Append(haveSorting);
    E(il, OpCodes.Ldarg_0); E(il, OpCodes.Call, tranToWalk);

    il.Append(afterPath);
    // Ensure SortingGroup exists using Unity Object equality, then update layer/order.
    E(il, OpCodes.Ldarg_0); E(il, OpCodes.Ldfld, fSortingGroup); E(il, OpCodes.Ldnull); E(il, OpCodes.Call, objectEquality); E(il, OpCodes.Brfalse, skipCombat);
    E(il, OpCodes.Ldarg_0); E(il, OpCodes.Ldarg_0); E(il, OpCodes.Call, componentGetGameObject); E(il, OpCodes.Callvirt, addSortingGroup); E(il, OpCodes.Stfld, fSortingGroup);

    il.Append(skipCombat);
    E(il, OpCodes.Ldarg_0); E(il, OpCodes.Ldfld, fSortingGroup); E(il, OpCodes.Ldstr, "Entity"); E(il, OpCodes.Callvirt, setSortingLayer);
    E(il, OpCodes.Ldarg_0); E(il, OpCodes.Ldfld, fSortingGroup); E(il, OpCodes.Ldarg_0); E(il, OpCodes.Ldfld, fY); E(il, OpCodes.Ldc_R4, -10f); E(il, OpCodes.Mul); E(il, OpCodes.Conv_I4); E(il, OpCodes.Callvirt, setSortingOrder);

    // Buffs always update; attack/characteristic only while enabled.
    E(il, OpCodes.Ldarg_0); E(il, OpCodes.Ldfld, fBuffManager); E(il, OpCodes.Ldarg_0); E(il, OpCodes.Callvirt, buffUpdate);
    E(il, OpCodes.Ldarg_0); E(il, OpCodes.Call, isDisabled); E(il, OpCodes.Brtrue, skipCombat = Instruction.Create(OpCodes.Nop));
    E(il, OpCodes.Ldarg_0); E(il, OpCodes.Call, updateAttack);
    E(il, OpCodes.Ldarg_0); E(il, OpCodes.Call, updateCharacteristic);
    il.Append(skipCombat);

    E(il, OpCodes.Ldarg_0); E(il, OpCodes.Ldfld, fElementManager); E(il, OpCodes.Callvirt, elementUpdate);
    E(il, OpCodes.Ldarg_0); E(il, OpCodes.Ldc_I4_0); E(il, OpCodes.Call, injury); E(il, OpCodes.Pop);

    il.Append(tail);
    E(il, OpCodes.Ldarg_0); E(il, OpCodes.Call, updateBrightness);
    E(il, OpCodes.Ldarg_0); E(il, OpCodes.Call, updatePrevious);

    // destroyTicking countdown: only ordered >0 enters, and NaN after subtraction is retained.
    E(il, OpCodes.Ldarg_0); E(il, OpCodes.Ldfld, fDestroyTicking); E(il, OpCodes.Ldc_R4, 0f); E(il, OpCodes.Ble_Un, afterDestroy);
    E(il, OpCodes.Ldarg_0); E(il, OpCodes.Ldarg_0); E(il, OpCodes.Ldfld, fDestroyTicking); E(il, OpCodes.Call, deltaTime); E(il, OpCodes.Sub); E(il, OpCodes.Stfld, fDestroyTicking);
    E(il, OpCodes.Ldarg_0); E(il, OpCodes.Ldfld, fDestroyTicking); E(il, OpCodes.Ldc_R4, 0f); E(il, OpCodes.Bgt_Un, afterDestroy);
    E(il, OpCodes.Ldarg_0); E(il, OpCodes.Call, destroyZombie);
    il.Append(afterDestroy);
    E(il, OpCodes.Ret);
}
