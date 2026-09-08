using Mono.Cecil;
using Mono.Cecil.Cil;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

if (args.Length != 2)
{
    Console.Error.WriteLine("Usage: GodsPVZ.HF12Patch <input.dll> <output.dll>");
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

var buff = T("Buff");
var bleed = T("Bleed");
var hide = T("Hide");
var attackRange = T("AttackRange");
var buffManager = T("BuffManager");
var zombie = T("Zombie");
var projectile = T("Projectile");
var plant = T("Plant");
PatchBuffUpdate();

Directory.CreateDirectory(Path.GetDirectoryName(output)!);
module.Write(output, new WriterParameters { WriteSymbols = false });

using (var verify = ModuleDefinition.ReadModule(output, new ReaderParameters { InMemory = true, ReadSymbols = false }))
{
    var b = All(verify.Types).Single(t => t.Name == "Buff");
    var m = b.Methods.Single(x => x.Name == "Update" && x.Parameters.Count == 3 && x.GenericParameters.Count == 1);
    if (m.MetadataToken.ToUInt32() != 0x060000F2) throw new InvalidDataException($"HF12 token drift: 0x{m.MetadataToken.ToUInt32():X8}");
    if (!m.HasBody || m.Body.Instructions.Count < 150) throw new InvalidDataException($"HF12 body too small: {m.Body.Instructions.Count}");
    if (m.Body.ExceptionHandlers.Count != 1 || m.Body.ExceptionHandlers[0].HandlerType != ExceptionHandlerType.Finally)
        throw new InvalidDataException("HF12 must retain childBuff List<Buff>.Enumerator finally/Dispose semantics");
    var refs = m.Body.Instructions.Select(i => i.Operand).OfType<MethodReference>().ToList();
    if (refs.Any(r => r.DeclaringType.FullName.Contains("Cpp2ILHelpers", StringComparison.Ordinal)))
        throw new InvalidDataException("HF12 still contains Cpp2IL helper calls");
    if (refs.Count(r => r.Name == "get_deltaTime") != 1) throw new InvalidDataException("HF12 must retain exactly one Time.deltaTime read in Routine lifecycle path");
    if (refs.Count(r => r.Name == "Bleeding") != 1) throw new InvalidDataException("HF12 must call Bleed.Bleeding<T> exactly once");
    if (refs.Count(r => r.Name == "Updata_Hide") != 1) throw new InvalidDataException("HF12 must call Hide.Updata_Hide exactly once");
    if (refs.Count(r => r.Name == "TestInRange") != 2) throw new InvalidDataException("HF12 must retain Zombie and Projectile TestInRange<T> calls");
    if (refs.Count(r => r.Name == "RemoveBuff") != 1) throw new InvalidDataException("HF12 must have one shared RemoveBuff tail");
    if (refs.Count(r => r.Name == "Dispose") != 1) throw new InvalidDataException("HF12 must dispose childBuff enumerator exactly once");
    var recursive = refs.OfType<GenericInstanceMethod>().Where(r => r.Name == "Update" && r.DeclaringType.Name == "Buff").ToList();
    if (recursive.Count != 1 || recursive[0].GenericArguments.Count != 1 || recursive[0].GenericArguments[0].MetadataType != MetadataType.MVar)
        throw new InvalidDataException("HF12 child recursion must bind Buff.Update<T> to this method's generic host parameter");
    Console.WriteLine($"VERIFY Buff.Update<T>: token=0x{m.MetadataToken.ToUInt32():X8}, IL={m.Body.Instructions.Count}, bytes={m.Body.CodeSize}, finally={m.Body.ExceptionHandlers.Count}");
}

Console.WriteLine("HF12 Buff.Update<T> shared native instance @0x18042AF00 restored from PC x86-64");
Console.WriteLine("HF12 preserves Bleed/Hide dispatch, recursive child updates, original/range lifetime gates, Routine NaN behavior, and Skill lifetime semantics");
Console.WriteLine($"WROTE {output} ({new FileInfo(output).Length} bytes)");
return 0;

void PatchBuffUpdate()
{
    var m = buff.Methods.Single(x => x.Name == "Update" && x.Parameters.Count == 3 && x.GenericParameters.Count == 1);
    if (m.MetadataToken.ToUInt32() != 0x060000F2) throw new InvalidDataException($"Unexpected Buff.Update<T> token 0x{m.MetadataToken.ToUInt32():X8}");

    var fBuffType = F(buff, "buffType");
    var fDuration = F(buff, "duration");
    var fOriginal = F(buff, "original");
    var fOriginalPlant = F(buff, "originalPlant");
    var fOriginalZombie = F(buff, "originalZombie");
    var fOriginalProjectile = F(buff, "originalProjectile");
    var fBuffRange = F(buff, "buffRange");
    var fChildBuffs = F(buff, "childBuffs");
    var fVfx = F(buff, "vfx");
    var fRangeType = F(attackRange, "rangeType");
    var fSkillOngoing = F(plant, "skillOngoing");

    if (fChildBuffs.FieldType is not GenericInstanceType listType || listType.GenericArguments.Count != 1 || listType.GenericArguments[0].FullName != buff.FullName)
        throw new InvalidDataException("HF12 expected Buff.childBuffs to be List<Buff>");

    var oldMoveNext = m.Body.Instructions.Select(i => i.Operand).OfType<MethodReference>()
        .FirstOrDefault(r => r.Name == "MoveNext" && r.DeclaringType.FullName.Contains("System.Collections.Generic.List`1/Enumerator", StringComparison.Ordinal))
        ?? throw new InvalidDataException("HF12 could not recover List<T>.Enumerator shape");
    if (oldMoveNext.DeclaringType is not GenericInstanceType oldEnum)
        throw new InvalidDataException("HF12 enumerator reference is not generic");
    var enumType = new GenericInstanceType(oldEnum.ElementType);
    enumType.GenericArguments.Add(buff);
    var getEnumerator = new MethodReference("GetEnumerator", enumType, listType) { HasThis = true };
    var moveNext = new MethodReference("MoveNext", module.TypeSystem.Boolean, enumType) { HasThis = true };
    var getCurrent = new MethodReference("get_Current", buff, enumType) { HasThis = true };
    var dispose = new MethodReference("Dispose", module.TypeSystem.Void, enumType) { HasThis = true };

    var hostT = m.GenericParameters[0];
    GenericInstanceMethod BindHost(MethodDefinition def)
    {
        var gi = new GenericInstanceMethod(def);
        gi.GenericArguments.Add(hostT);
        return gi;
    }
    var bleedCall = BindHost(M(bleed, "Bleeding", 1));
    var selfUpdate = BindHost(m);

    var attackRangeOpen = M(attackRange, "TestInRange", 1);
    GenericInstanceMethod BindRange(TypeReference target)
    {
        var gi = new GenericInstanceMethod(attackRangeOpen);
        gi.GenericArguments.Add(target);
        return gi;
    }
    var testZombie = BindRange(zombie);
    var testProjectile = BindRange(projectile);

    var hideUpdate = M(hide, "Updata_Hide", 0);
    var removeBuff = M(buffManager, "RemoveBuff", 1);
    var nreCtor = m.Body.Instructions.Select(i => i.Operand).OfType<MethodReference>()
        .FirstOrDefault(r => r.DeclaringType.FullName == "System.NullReferenceException" && r.Name == ".ctor" && r.Parameters.Count == 0)
        ?? throw new InvalidDataException("HF12 could not recover NullReferenceException::.ctor");

    var objectEq = Ref("UnityEngine.Object", "op_Equality", "UnityEngine.Object", "UnityEngine.Object");
    var objectNe = Ref("UnityEngine.Object", "op_Inequality", "UnityEngine.Object", "UnityEngine.Object");
    var objectImplicit = Ref("UnityEngine.Object", "op_Implicit", "UnityEngine.Object");
    var goTransform = Ref("UnityEngine.GameObject", "get_transform");
    var componentTransform = Ref("UnityEngine.Component", "get_transform");
    var transformGetPosition = Ref("UnityEngine.Transform", "get_position");
    var transformSetPosition = Ref("UnityEngine.Transform", "set_position", "UnityEngine.Vector3");
    var deltaTime = Ref("UnityEngine.Time", "get_deltaTime");

    var b = new MethodBody(m) { InitLocals = true, MaxStackSize = 8 };
    m.Body = b;
    var il = b.GetILProcessor();

    var bleedLocal = new VariableDefinition(bleed);
    var hideLocal = new VariableDefinition(hide);
    var en = new VariableDefinition(enumType);
    var childLocal = new VariableDefinition(buff);
    var zombieHost = new VariableDefinition(zombie);
    var projectileHost = new VariableDefinition(projectile);
    var durationLocal = new VariableDefinition(module.TypeSystem.Single);
    foreach (var v in new[] { bleedLocal, hideLocal, en, childLocal, zombieHost, projectileHost, durationLocal }) b.Variables.Add(v);

    var afterBleed = Instruction.Create(OpCodes.Nop);
    var afterHide = Instruction.Create(OpCodes.Nop);
    var returnNow = Instruction.Create(OpCodes.Ret);
    var childTry = Instruction.Create(OpCodes.Nop);
    var childLoopCheck = Instruction.Create(OpCodes.Nop);
    var childLoopBody = Instruction.Create(OpCodes.Nop);
    var childFinally = Instruction.Create(OpCodes.Nop);
    var afterChildren = Instruction.Create(OpCodes.Nop);
    var originalGateDone = Instruction.Create(OpCodes.Nop);
    var rangeDone = Instruction.Create(OpCodes.Nop);
    var afterZombieRange = Instruction.Create(OpCodes.Nop);
    var afterProjectileRange = Instruction.Create(OpCodes.Nop);
    var lifecycle = Instruction.Create(OpCodes.Nop);
    var skillLifecycle = Instruction.Create(OpCodes.Nop);
    var remove = Instruction.Create(OpCodes.Nop);

    // Native dispatch before the child early-return: Bleed.Bleeding<T>(host), then Hide.Updata_Hide().
    E(il, OpCodes.Ldarg_0); E(il, OpCodes.Isinst, bleed); E(il, OpCodes.Stloc, bleedLocal);
    E(il, OpCodes.Ldloc, bleedLocal); E(il, OpCodes.Brfalse, afterBleed);
    E(il, OpCodes.Ldloc, bleedLocal); E(il, OpCodes.Ldarg_1); E(il, OpCodes.Call, bleedCall);
    il.Append(afterBleed);
    E(il, OpCodes.Ldarg_0); E(il, OpCodes.Isinst, hide); E(il, OpCodes.Stloc, hideLocal);
    E(il, OpCodes.Ldloc, hideLocal); E(il, OpCodes.Brfalse, afterHide);
    E(il, OpCodes.Ldloc, hideLocal); E(il, OpCodes.Call, hideUpdate);
    il.Append(afterHide);
    E(il, OpCodes.Ldarg_2); E(il, OpCodes.Brtrue, returnNow);

    // foreach (Buff childBuff in childBuffs) childBuff.Update<T>(host,true,buffManager);
    // Native keeps a real List<Buff>.Enumerator finally/Dispose and faults on a null child/list.
    E(il, OpCodes.Ldarg_0); E(il, OpCodes.Ldfld, fChildBuffs); E(il, OpCodes.Callvirt, getEnumerator); E(il, OpCodes.Stloc, en);
    il.Append(childTry); E(il, OpCodes.Br, childLoopCheck);
    il.Append(childLoopBody);
    E(il, OpCodes.Ldloca, en); E(il, OpCodes.Call, getCurrent); E(il, OpCodes.Stloc, childLocal);
    var childOk = Instruction.Create(OpCodes.Nop);
    E(il, OpCodes.Ldloc, childLocal); E(il, OpCodes.Brtrue, childOk);
    E(il, OpCodes.Newobj, nreCtor); E(il, OpCodes.Throw);
    il.Append(childOk);
    E(il, OpCodes.Ldloc, childLocal); E(il, OpCodes.Ldarg_1); E(il, OpCodes.Ldc_I4_1); E(il, OpCodes.Ldarg_3); E(il, OpCodes.Call, selfUpdate);
    il.Append(childLoopCheck);
    E(il, OpCodes.Ldloca, en); E(il, OpCodes.Call, moveNext); E(il, OpCodes.Brtrue, childLoopBody);
    E(il, OpCodes.Leave, afterChildren);
    il.Append(childFinally); E(il, OpCodes.Ldloca, en); E(il, OpCodes.Call, dispose); E(il, OpCodes.Endfinally);
    il.Append(afterChildren);
    b.ExceptionHandlers.Add(new ExceptionHandler(ExceptionHandlerType.Finally)
    {
        TryStart = childTry,
        TryEnd = childFinally,
        HandlerStart = childFinally,
        HandlerEnd = afterChildren
    });

    // Native source-lifetime gate: if original && originalPlant == null && originalZombie == null & originalProjectile == null => remove.
    E(il, OpCodes.Ldarg_0); E(il, OpCodes.Ldfld, fOriginal); E(il, OpCodes.Brfalse, originalGateDone);
    E(il, OpCodes.Ldarg_0); E(il, OpCodes.Ldfld, fOriginalPlant); E(il, OpCodes.Ldnull); E(il, OpCodes.Call, objectEq); E(il, OpCodes.Brfalse, originalGateDone);
    E(il, OpCodes.Ldarg_0); E(il, OpCodes.Ldfld, fOriginalZombie); E(il, OpCodes.Ldnull); E(il, OpCodes.Call, objectEq);
    E(il, OpCodes.Ldarg_0); E(il, OpCodes.Ldfld, fOriginalProjectile); E(il, OpCodes.Ldnull); E(il, OpCodes.Call, objectEq);
    E(il, OpCodes.And); E(il, OpCodes.Brtrue, remove);
    il.Append(originalGateDone);

    // Optional AttackRange gate applies only to non-null Zombie or Projectile hosts when rangeType != Null.
    E(il, OpCodes.Ldarg_0); E(il, OpCodes.Ldfld, fBuffRange); E(il, OpCodes.Brfalse, lifecycle);
    E(il, OpCodes.Ldarg_0); E(il, OpCodes.Ldfld, fBuffRange); E(il, OpCodes.Ldfld, fRangeType); E(il, OpCodes.Brfalse, lifecycle);
    E(il, OpCodes.Ldarg_1); E(il, OpCodes.Box, hostT); E(il, OpCodes.Brfalse, lifecycle);

    E(il, OpCodes.Ldarg_1); E(il, OpCodes.Box, hostT); E(il, OpCodes.Isinst, zombie); E(il, OpCodes.Stloc, zombieHost);
    E(il, OpCodes.Ldloc, zombieHost); E(il, OpCodes.Brfalse, afterZombieRange);
    E(il, OpCodes.Ldarg_0); E(il, OpCodes.Ldfld, fBuffRange); E(il, OpCodes.Ldloc, zombieHost); E(il, OpCodes.Call, testZombie); E(il, OpCodes.Brfalse, remove);
    EmitVfxFollow(il, zombieHost, afterZombieRange);
    il.Append(afterZombieRange);

    E(il, OpCodes.Ldarg_1); E(il, OpCodes.Box, hostT); E(il, OpCodes.Isinst, projectile); E(il, OpCodes.Stloc, projectileHost);
    E(il, OpCodes.Ldloc, projectileHost); E(il, OpCodes.Brfalse, afterProjectileRange);
    E(il, OpCodes.Ldarg_0); E(il, OpCodes.Ldfld, fBuffRange); E(il, OpCodes.Ldloc, projectileHost); E(il, OpCodes.Call, testProjectile); E(il, OpCodes.Brfalse, remove);
    EmitVfxFollow(il, projectileHost, afterProjectileRange);
    il.Append(afterProjectileRange);

    il.Append(lifecycle);
    // buffType Routine(0): duration -= deltaTime; native COMISS 0,duration + JB returns for duration>0 OR unordered/NaN.
    E(il, OpCodes.Ldarg_0); E(il, OpCodes.Ldfld, fBuffType); E(il, OpCodes.Brtrue, skillLifecycle);
    E(il, OpCodes.Ldarg_0); E(il, OpCodes.Ldfld, fDuration); E(il, OpCodes.Call, deltaTime); E(il, OpCodes.Sub); E(il, OpCodes.Stloc, durationLocal);
    E(il, OpCodes.Ldarg_0); E(il, OpCodes.Ldloc, durationLocal); E(il, OpCodes.Stfld, fDuration);
    E(il, OpCodes.Ldc_R4, 0f); E(il, OpCodes.Ldloc, durationLocal); E(il, OpCodes.Blt_Un, returnNow);
    E(il, OpCodes.Br, remove);

    il.Append(skillLifecycle);
    // Only Skill(2) has source-driven lifetime; Passively/Range/other values return unchanged.
    E(il, OpCodes.Ldarg_0); E(il, OpCodes.Ldfld, fBuffType); E(il, OpCodes.Ldc_I4_2); E(il, OpCodes.Bne_Un, returnNow);
    E(il, OpCodes.Ldarg_0); E(il, OpCodes.Ldfld, fOriginalPlant); E(il, OpCodes.Call, objectImplicit); E(il, OpCodes.Brfalse, remove);
    E(il, OpCodes.Ldarg_0); E(il, OpCodes.Ldfld, fOriginalPlant); E(il, OpCodes.Ldfld, fSkillOngoing); E(il, OpCodes.Brtrue, returnNow);

    il.Append(remove);
    var managerOk = Instruction.Create(OpCodes.Nop);
    E(il, OpCodes.Ldarg_3); E(il, OpCodes.Brtrue, managerOk);
    E(il, OpCodes.Newobj, nreCtor); E(il, OpCodes.Throw);
    il.Append(managerOk);
    E(il, OpCodes.Ldarg_3); E(il, OpCodes.Ldarg_0); E(il, OpCodes.Call, removeBuff); E(il, OpCodes.Ret);
    il.Append(returnNow);
}

void EmitVfxFollow(ILProcessor il, VariableDefinition hostLocal, Instruction continuation)
{
    var skip = Instruction.Create(OpCodes.Nop);
    E(il, OpCodes.Ldarg_0); E(il, OpCodes.Ldfld, F(buff, "vfx")); E(il, OpCodes.Ldnull); E(il, OpCodes.Call, Ref("UnityEngine.Object", "op_Inequality", "UnityEngine.Object", "UnityEngine.Object")); E(il, OpCodes.Brfalse, skip);
    E(il, OpCodes.Ldarg_0); E(il, OpCodes.Ldfld, F(buff, "vfx")); E(il, OpCodes.Callvirt, Ref("UnityEngine.GameObject", "get_transform"));
    E(il, OpCodes.Ldloc, hostLocal); E(il, OpCodes.Callvirt, Ref("UnityEngine.Component", "get_transform")); E(il, OpCodes.Callvirt, Ref("UnityEngine.Transform", "get_position"));
    E(il, OpCodes.Callvirt, Ref("UnityEngine.Transform", "set_position", "UnityEngine.Vector3"));
    il.Append(skip);
}
