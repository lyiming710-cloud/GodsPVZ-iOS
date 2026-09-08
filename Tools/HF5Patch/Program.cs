using Mono.Cecil;
using Mono.Cecil.Cil;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

if (args.Length != 2)
{
    Console.Error.WriteLine("Usage: GodsPVZ.HF5Patch <input.dll> <output.dll>");
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
MethodReference ListMethod(GenericInstanceType list, string name, TypeReference ret, params TypeReference[] ps)
{
    var mr = new MethodReference(name, ret, list) { HasThis = true };
    foreach (var p in ps) mr.Parameters.Add(new ParameterDefinition(p));
    return mr;
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
        ParameterDefinition pd => Instruction.Create(op, pd),
        VariableDefinition vd => Instruction.Create(op, vd),
        Instruction target => Instruction.Create(op, target),
        _ => throw new NotSupportedException(value.GetType().FullName)
    };
    il.Append(ins);
}

var zombie = T("Zombie");
var elementManager = T("ElementManager");
var board = T("Board");
var zombieManager = T("ZombieManager");
var entryType = T("ZombieManager/BoardEntryData");

PatchLoopAddAnimation();
PatchStart();

Directory.CreateDirectory(Path.GetDirectoryName(output)!);
module.Write(output, new WriterParameters { WriteSymbols = false });

using (var verify = ModuleDefinition.ReadModule(output, new ReaderParameters { InMemory = true, ReadSymbols = false }))
{
    var z = All(verify.Types).Single(t => t.Name == "Zombie");
    var loop = z.Methods.Single(m => m.Name == "LoopAddAnimation" && m.Parameters.Count == 1);
    var start = z.Methods.Single(m => m.Name == "Start" && m.Parameters.Count == 0);
    if (loop.MetadataToken.ToUInt32() != 0x0600041F) throw new InvalidDataException("HF5 LoopAddAnimation token drift");
    if (start.MetadataToken.ToUInt32() != 0x0600041C) throw new InvalidDataException("HF5 Start token drift");
    if (!loop.HasBody || loop.Body.Instructions.Count < 30) throw new InvalidDataException($"HF5 LoopAddAnimation too small: {loop.Body.Instructions.Count}");
    if (loop.Body.ExceptionHandlers.Count != 1 || loop.Body.ExceptionHandlers[0].HandlerType != ExceptionHandlerType.Finally)
        throw new InvalidDataException("HF5 LoopAddAnimation must retain IEnumerator finally/dispose semantics");
    if (!start.HasBody || start.Body.Instructions.Count < 180) throw new InvalidDataException($"HF5 Start too small: {start.Body.Instructions.Count}");
    if (start.Body.Instructions.Count(i => i.OpCode == OpCodes.Switch) != 1) throw new InvalidDataException("HF5 Start must contain one ID grouping switch");
    foreach (var m in new[] { loop, start })
    {
        if (m.Body.Instructions.Select(i => i.Operand).OfType<MethodReference>().Any(r => r.DeclaringType.FullName.Contains("Cpp2ILHelpers", StringComparison.Ordinal)))
            throw new InvalidDataException($"HF5 still contains Cpp2IL helper calls in {m.Name}");
    }
    var strings = start.Body.Instructions.Where(i => i.OpCode == OpCodes.Ldstr).Select(i => (string)i.Operand).ToHashSet();
    foreach (var s in new[] { "Group", "Speed" }) if (!strings.Contains(s)) throw new InvalidDataException($"HF5 Start missing native string {s}");
    var calls = start.Body.Instructions.Select(i => i.Operand).OfType<MethodReference>().Select(r => r.Name).ToHashSet();
    foreach (var c in new[] { "LoopAddAnimation", "GetRandenAnimationSpeedMagnification", "CreateNewElements", "SetUIs", "GetBoardEntryData", "Start_Characteristic", "CreateStartPrePath" })
        if (!calls.Contains(c)) throw new InvalidDataException($"HF5 Start missing native call {c}");
    Console.WriteLine($"VERIFY Zombie.LoopAddAnimation: {loop.Body.Instructions.Count} IL, {loop.Body.CodeSize} bytes, finally={loop.Body.ExceptionHandlers.Count}");
    Console.WriteLine($"VERIFY Zombie.Start: {start.Body.Instructions.Count} IL, {start.Body.CodeSize} bytes, ID switch + board-entry scaling");
}

Console.WriteLine("HF5 Zombie.LoopAddAnimation @0x180365BA0 + Zombie.Start @0x1803695F0 restored from PC native x86-64");
Console.WriteLine($"WROTE {output} ({new FileInfo(output).Length} bytes)");
return 0;

void PatchLoopAddAnimation()
{
    var m = M(zombie, "LoopAddAnimation", 1);
    if (m.MetadataToken.ToUInt32() != 0x0600041F) throw new InvalidDataException("Unexpected Zombie.LoopAddAnimation token");
    var b = new MethodBody(m) { InitLocals = true, MaxStackSize = 8 };
    m.Body = b;
    var il = b.GetILProcessor();
    var parent = m.Parameters[0];
    var fSprites = F(zombie, "animationSprites");
    var list = (GenericInstanceType)fSprites.FieldType;

    var getRenderer = GRef("UnityEngine.Component", "GetComponent", "SpriteRenderer");
    var spriteRenderer = getRenderer.GenericArguments[0];
    var objImplicit = Ref("UnityEngine.Object", "op_Implicit", "UnityEngine.Object");
    var getGO = Ref("UnityEngine.Component", "get_gameObject");
    var add = ListMethod(list, "Add", module.TypeSystem.Void, list.GenericArguments[0]);
    var getEnum = Ref("UnityEngine.Transform", "GetEnumerator");
    var ienum = getEnum.ReturnType;
    var moveNext = new MethodReference("MoveNext", module.TypeSystem.Boolean, ienum) { HasThis = true };
    var getCurrent = new MethodReference("get_Current", module.TypeSystem.Object, ienum) { HasThis = true };
    var disposable = new TypeReference("System", "IDisposable", module, module.TypeSystem.Object.Scope);
    var dispose = new MethodReference("Dispose", module.TypeSystem.Void, disposable) { HasThis = true };

    var renderer = new VariableDefinition(spriteRenderer);
    var en = new VariableDefinition(ienum);
    var child = new VariableDefinition(parent.ParameterType);
    var disp = new VariableDefinition(disposable);
    b.Variables.Add(renderer); b.Variables.Add(en); b.Variables.Add(child); b.Variables.Add(disp);

    var skipAdd = Instruction.Create(OpCodes.Nop);
    var tryStart = Instruction.Create(OpCodes.Nop);
    var loopCheck = Instruction.Create(OpCodes.Nop);
    var loopBody = Instruction.Create(OpCodes.Nop);
    var finallyStart = Instruction.Create(OpCodes.Nop);
    var skipDispose = Instruction.Create(OpCodes.Nop);
    var afterFinally = Instruction.Create(OpCodes.Ret);

    E(il, OpCodes.Ldarg, parent); E(il, OpCodes.Callvirt, getRenderer); E(il, OpCodes.Stloc, renderer);
    E(il, OpCodes.Ldloc, renderer); E(il, OpCodes.Call, objImplicit); E(il, OpCodes.Brfalse, skipAdd);
    E(il, OpCodes.Ldarg_0); E(il, OpCodes.Ldfld, fSprites); E(il, OpCodes.Ldarg, parent); E(il, OpCodes.Callvirt, getGO); E(il, OpCodes.Callvirt, add);
    il.Append(skipAdd);
    E(il, OpCodes.Ldarg, parent); E(il, OpCodes.Callvirt, getEnum); E(il, OpCodes.Stloc, en);
    il.Append(tryStart); E(il, OpCodes.Br, loopCheck);
    il.Append(loopBody);
    E(il, OpCodes.Ldloc, en); E(il, OpCodes.Callvirt, getCurrent); E(il, OpCodes.Castclass, parent.ParameterType); E(il, OpCodes.Stloc, child);
    E(il, OpCodes.Ldarg_0); E(il, OpCodes.Ldloc, child); E(il, OpCodes.Call, m);
    il.Append(loopCheck);
    E(il, OpCodes.Ldloc, en); E(il, OpCodes.Callvirt, moveNext); E(il, OpCodes.Brtrue, loopBody);
    E(il, OpCodes.Leave, afterFinally);

    il.Append(finallyStart);
    E(il, OpCodes.Ldloc, en); E(il, OpCodes.Isinst, disposable); E(il, OpCodes.Stloc, disp);
    E(il, OpCodes.Ldloc, disp); E(il, OpCodes.Brfalse, skipDispose);
    E(il, OpCodes.Ldloc, disp); E(il, OpCodes.Callvirt, dispose);
    il.Append(skipDispose); E(il, OpCodes.Endfinally);
    il.Append(afterFinally);

    b.ExceptionHandlers.Add(new ExceptionHandler(ExceptionHandlerType.Finally)
    {
        TryStart = tryStart,
        TryEnd = finallyStart,
        HandlerStart = finallyStart,
        HandlerEnd = afterFinally
    });

    Console.WriteLine("HF5 Zombie.LoopAddAnimation @0x180365BA0: SpriteRenderer collect + recursive Transform enumeration + IDisposable finally");
}

void PatchStart()
{
    var m = M(zombie, "Start", 0);
    if (m.MetadataToken.ToUInt32() != 0x0600041C) throw new InvalidDataException("Unexpected Zombie.Start token");
    var b = new MethodBody(m) { InitLocals = true, MaxStackSize = 8 };
    m.Body = b;
    var il = b.GetILProcessor();

    var fAnimationGroup = F(zombie, "animationGroup");
    var fAnimator = F(zombie, "animator");
    var fID = F(zombie, "ID");
    var fElementManager = F(zombie, "elementManager");
    var fX = F(zombie, "fX");
    var fY = F(zombie, "fY");
    var fZ = F(zombie, "fZ");
    var fPrevious = F(zombie, "previousPosition");
    var fBoard = F(zombie, "board");
    var fHealth = F(zombie, "healthPoint");
    var fMaxHealth = F(zombie, "maxHealthPoint");
    var fAttack = F(zombie, "attackPoint");
    var fDefense = F(zombie, "defense");
    var fElemental = F(zombie, "elementalResistance");
    var fArmor1Type = F(zombie, "armor1Type");
    var fArmor1Point = F(zombie, "armor1Point");
    var fMaxArmor1 = F(zombie, "maxArmor1Point");
    var fArmor1Defense = F(zombie, "armor1Defense");
    var fArmor1Toughness = F(zombie, "armor1Toughness");
    var fArmor2Type = F(zombie, "armor2Type");
    var fArmor2Point = F(zombie, "armor2Point");
    var fMaxArmor2 = F(zombie, "maxArmor2Point");
    var fArmor2Defense = F(zombie, "armor2Defense");
    var fArmor2Toughness = F(zombie, "armor2Toughness");
    var fPrePath = F(zombie, "prePath");

    var fBoardZM = F(board, "zombieManager");
    var fAtkMag = F(entryType, "magnification_Atk");
    var fHpMag = F(entryType, "magnification_Hp");
    var fDefMag = F(entryType, "magnification_Def");
    var fTough = F(entryType, "toughness");
    var fElemRes = F(entryType, "elementalResistance");

    var gameObjectGetTransform = Ref("UnityEngine.GameObject", "get_transform");
    var componentGetTransform = Ref("UnityEngine.Component", "get_transform");
    var getPosition = Ref("UnityEngine.Transform", "get_position");
    var vector3 = getPosition.ReturnType;
    var vx = new FieldReference("x", module.TypeSystem.Single, vector3);
    var vy = new FieldReference("y", module.TypeSystem.Single, vector3);
    var vz = new FieldReference("z", module.TypeSystem.Single, vector3);
    var getAnimator = GRef("UnityEngine.Component", "GetComponent", "Animator");
    var randomRange = Ref("UnityEngine.Random", "Range", "System.Int32", "System.Int32");
    var setInteger = Ref("UnityEngine.Animator", "SetInteger", "System.String", "System.Int32");
    var setFloat = Ref("UnityEngine.Animator", "SetFloat", "System.String", "System.Single");
    var objInequality = Ref("UnityEngine.Object", "op_Inequality", "UnityEngine.Object", "UnityEngine.Object");
    var createElementsDef = M(elementManager, "CreateNewElements", 1);
    var createElements = new GenericInstanceMethod(createElementsDef); createElements.GenericArguments.Add(zombie);
    var loop = M(zombie, "LoopAddAnimation", 1);
    var speedMag = M(zombie, "GetRandenAnimationSpeedMagnification", 0);
    var setUIs = M(zombie, "SetUIs", 0);
    var getEntry = M(zombieManager, "GetBoardEntryData", 1);
    var startCharacteristic = M(zombie, "Start_Characteristic", 0);
    var createStartPrePath = M(zombie, "CreateStartPrePath", 0);
    var prePathList = (GenericInstanceType)fPrePath.FieldType;
    var prePathCount = ListMethod(prePathList, "get_Count", module.TypeSystem.Int32);

    var animatorLocal = new VariableDefinition(fAnimator.FieldType);
    var v1 = new VariableDefinition(vector3);
    var v2 = new VariableDefinition(vector3);
    var entry = new VariableDefinition(entryType);
    b.Variables.Add(animatorLocal); b.Variables.Add(v1); b.Variables.Add(v2); b.Variables.Add(entry);

    // animationGroup.transform -> LoopAddAnimation
    E(il, OpCodes.Ldarg_0); E(il, OpCodes.Ldfld, fAnimationGroup); E(il, OpCodes.Call, gameObjectGetTransform);
    E(il, OpCodes.Ldarg_0); // reorder to call instance(this, transform): stash transform in v1 is impossible (Vector3), use temporary Transform local below
    // replace the just-emitted order using a Transform local: pop and rebuild cleanly below is not valid; method is constructed in one pass.
    il.Body.Instructions.Clear();
    b.ExceptionHandlers.Clear();
    var transformLocal = new VariableDefinition(gameObjectGetTransform.ReturnType); b.Variables.Add(transformLocal);

    E(il, OpCodes.Ldarg_0); E(il, OpCodes.Ldfld, fAnimationGroup); E(il, OpCodes.Call, gameObjectGetTransform); E(il, OpCodes.Stloc, transformLocal);
    E(il, OpCodes.Ldarg_0); E(il, OpCodes.Ldloc, transformLocal); E(il, OpCodes.Call, loop);

    // animator = this.GetComponent<Animator>()
    E(il, OpCodes.Ldarg_0); E(il, OpCodes.Call, getAnimator); E(il, OpCodes.Stloc, animatorLocal);
    E(il, OpCodes.Ldarg_0); E(il, OpCodes.Ldloc, animatorLocal); E(il, OpCodes.Stfld, fAnimator);

    // Native ID grouping: Random.Range + SetInteger("Group") only for the exact cases below.
    var randomGroup = Instruction.Create(OpCodes.Nop);
    var afterGroup = Instruction.Create(OpCodes.Nop);
    var cases = new Instruction[23];
    var randomIds = new HashSet<int> { 0, 2, 4, 5, 6, 7, 8, 9, 11, 12, 14, 15, 19, 20, 21, 22 };
    for (var i = 0; i < cases.Length; i++) cases[i] = randomIds.Contains(i) ? randomGroup : afterGroup;
    E(il, OpCodes.Ldarg_0); E(il, OpCodes.Ldfld, fID); il.Append(Instruction.Create(OpCodes.Switch, cases)); E(il, OpCodes.Br, afterGroup);
    il.Append(randomGroup);
    E(il, OpCodes.Ldarg_0); E(il, OpCodes.Ldfld, fAnimator); E(il, OpCodes.Ldstr, "Group");
    E(il, OpCodes.Ldc_I4_0); E(il, OpCodes.Ldc_I4_2); E(il, OpCodes.Call, randomRange); E(il, OpCodes.Callvirt, setInteger);
    il.Append(afterGroup);

    // animator.SetFloat("Speed", GetRandenAnimationSpeedMagnification())
    E(il, OpCodes.Ldarg_0); E(il, OpCodes.Ldfld, fAnimator); E(il, OpCodes.Ldstr, "Speed");
    E(il, OpCodes.Ldarg_0); E(il, OpCodes.Call, speedMag); E(il, OpCodes.Callvirt, setFloat);

    // elementManager.CreateNewElements<Zombie>(this)
    E(il, OpCodes.Ldarg_0); E(il, OpCodes.Ldfld, fElementManager); E(il, OpCodes.Ldarg_0); E(il, OpCodes.Call, createElements);

    // fX = this.transform.position.x; native performs a distinct transform/position read.
    E(il, OpCodes.Ldarg_0); E(il, OpCodes.Call, componentGetTransform); E(il, OpCodes.Stloc, transformLocal);
    E(il, OpCodes.Ldloc, transformLocal); E(il, OpCodes.Call, getPosition); E(il, OpCodes.Stloc, v1);
    E(il, OpCodes.Ldarg_0); E(il, OpCodes.Ldloca, v1); E(il, OpCodes.Ldfld, vx); E(il, OpCodes.Stfld, fX);

    // fY = this.transform.position.y; second distinct read.
    E(il, OpCodes.Ldarg_0); E(il, OpCodes.Call, componentGetTransform); E(il, OpCodes.Stloc, transformLocal);
    E(il, OpCodes.Ldloc, transformLocal); E(il, OpCodes.Call, getPosition); E(il, OpCodes.Stloc, v1);
    E(il, OpCodes.Ldarg_0); E(il, OpCodes.Ldloca, v1); E(il, OpCodes.Ldfld, vy); E(il, OpCodes.Stfld, fY);

    // fZ = animationGroup.transform.position.y - this.transform.position.y, with independent reads.
    E(il, OpCodes.Ldarg_0); E(il, OpCodes.Ldfld, fAnimationGroup); E(il, OpCodes.Call, gameObjectGetTransform); E(il, OpCodes.Stloc, transformLocal);
    E(il, OpCodes.Ldloc, transformLocal); E(il, OpCodes.Call, getPosition); E(il, OpCodes.Stloc, v1);
    E(il, OpCodes.Ldarg_0); E(il, OpCodes.Call, componentGetTransform); E(il, OpCodes.Stloc, transformLocal);
    E(il, OpCodes.Ldloc, transformLocal); E(il, OpCodes.Call, getPosition); E(il, OpCodes.Stloc, v2);
    E(il, OpCodes.Ldarg_0); E(il, OpCodes.Ldloca, v1); E(il, OpCodes.Ldfld, vy); E(il, OpCodes.Ldloca, v2); E(il, OpCodes.Ldfld, vy); E(il, OpCodes.Sub); E(il, OpCodes.Stfld, fZ);

    // previousPosition.x/y are written from fX/fY after fZ, exactly in native order.
    E(il, OpCodes.Ldarg_0); E(il, OpCodes.Ldflda, fPrevious); E(il, OpCodes.Ldarg_0); E(il, OpCodes.Ldfld, fX); E(il, OpCodes.Stfld, vx);
    E(il, OpCodes.Ldarg_0); E(il, OpCodes.Ldflda, fPrevious); E(il, OpCodes.Ldarg_0); E(il, OpCodes.Ldfld, fY); E(il, OpCodes.Stfld, vy);

    // previousPosition.z = a fresh animationGroup.transform.position.y - previousPosition.y.
    E(il, OpCodes.Ldarg_0); E(il, OpCodes.Ldfld, fAnimationGroup); E(il, OpCodes.Call, gameObjectGetTransform); E(il, OpCodes.Stloc, transformLocal);
    E(il, OpCodes.Ldloc, transformLocal); E(il, OpCodes.Call, getPosition); E(il, OpCodes.Stloc, v1);
    E(il, OpCodes.Ldarg_0); E(il, OpCodes.Ldflda, fPrevious); E(il, OpCodes.Ldloca, v1); E(il, OpCodes.Ldfld, vy);
    E(il, OpCodes.Ldarg_0); E(il, OpCodes.Ldflda, fPrevious); E(il, OpCodes.Ldfld, vy); E(il, OpCodes.Sub); E(il, OpCodes.Stfld, vz);

    E(il, OpCodes.Ldarg_0); E(il, OpCodes.Call, setUIs);

    var afterBoard = Instruction.Create(OpCodes.Nop);
    var afterArmor1 = Instruction.Create(OpCodes.Nop);
    var afterArmor2 = Instruction.Create(OpCodes.Nop);
    // if (board != null) { entry = board.zombieManager.GetBoardEntryData(ID); ... }
    E(il, OpCodes.Ldarg_0); E(il, OpCodes.Ldfld, fBoard); E(il, OpCodes.Ldnull); E(il, OpCodes.Call, objInequality); E(il, OpCodes.Brfalse, afterBoard);
    E(il, OpCodes.Ldarg_0); E(il, OpCodes.Ldfld, fBoard); E(il, OpCodes.Ldfld, fBoardZM); E(il, OpCodes.Ldarg_0); E(il, OpCodes.Ldfld, fID); E(il, OpCodes.Call, getEntry); E(il, OpCodes.Stloc, entry);

    ScaleMul(fHealth, fHpMag); ScaleMul(fMaxHealth, fHpMag); ScaleMul(fAttack, fAtkMag); ScaleMul(fDefense, fDefMag); ScaleAdd(fElemental, fElemRes);

    E(il, OpCodes.Ldarg_0); E(il, OpCodes.Ldfld, fArmor1Type); E(il, OpCodes.Brfalse, afterArmor1);
    ScaleMul(fArmor1Point, fHpMag); ScaleMul(fMaxArmor1, fHpMag); ScaleMul(fArmor1Defense, fDefMag); ScaleAdd(fArmor1Toughness, fTough);
    il.Append(afterArmor1);

    E(il, OpCodes.Ldarg_0); E(il, OpCodes.Ldfld, fArmor2Type); E(il, OpCodes.Brfalse, afterArmor2);
    ScaleMul(fArmor2Point, fHpMag); ScaleMul(fMaxArmor2, fHpMag); ScaleMul(fArmor2Defense, fDefMag); ScaleAdd(fArmor2Toughness, fTough);
    il.Append(afterArmor2); il.Append(afterBoard);

    E(il, OpCodes.Ldarg_0); E(il, OpCodes.Call, startCharacteristic);

    var createPath = Instruction.Create(OpCodes.Nop);
    var ret = Instruction.Create(OpCodes.Ret);
    E(il, OpCodes.Ldarg_0); E(il, OpCodes.Ldfld, fPrePath); E(il, OpCodes.Brfalse, createPath);
    E(il, OpCodes.Ldarg_0); E(il, OpCodes.Ldfld, fPrePath); E(il, OpCodes.Callvirt, prePathCount); E(il, OpCodes.Brtrue, ret);
    il.Append(createPath); E(il, OpCodes.Ldarg_0); E(il, OpCodes.Call, createStartPrePath); il.Append(ret);

    Console.WriteLine("HF5 Zombie.Start @0x1803695F0: animation group, exact Group IDs, Speed, element creation, coordinate/previousPosition order, board-entry scaling, characteristic/path init");

    void ScaleMul(FieldDefinition target, FieldDefinition factor)
    {
        E(il, OpCodes.Ldarg_0); E(il, OpCodes.Ldloc, entry); E(il, OpCodes.Ldfld, factor); E(il, OpCodes.Ldarg_0); E(il, OpCodes.Ldfld, target); E(il, OpCodes.Mul); E(il, OpCodes.Stfld, target);
    }
    void ScaleAdd(FieldDefinition target, FieldDefinition addend)
    {
        E(il, OpCodes.Ldarg_0); E(il, OpCodes.Ldloc, entry); E(il, OpCodes.Ldfld, addend); E(il, OpCodes.Ldarg_0); E(il, OpCodes.Ldfld, target); E(il, OpCodes.Add); E(il, OpCodes.Stfld, target);
    }
}
