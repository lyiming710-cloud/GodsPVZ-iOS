using Mono.Cecil;
using Mono.Cecil.Cil;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

if (args.Length != 2)
{
    Console.Error.WriteLine("Usage: GodsPVZ.HF18Patch <input.dll> <output.dll>");
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
        int i => Instruction.Create(op, i),
        float f => Instruction.Create(op, f),
        string s => Instruction.Create(op, s),
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
var stats = T("StatsIncreased");
var manager = T("BuffManager");
var attackRange = T("AttackRange");

var fBuffName = F(buff, "name");
var fDuration = F(buff, "duration");
var fBuffRange = F(buff, "buffRange");
var fChildren = F(buff, "childBuffs");
var fStatsName = F(stats, "valueName");
var fStatsValue = F(stats, "value");
var fStatsMulti = F(stats, "multi");
var fBuffs = F(manager, "buffs");
var fToAdd = F(manager, "buffs_toAdd");
var fToRemove = F(manager, "buffs_toRemove");

if (fBuffName.FieldType.FullName != "System.String" || fDuration.FieldType.FullName != "System.Single")
    throw new InvalidDataException("HF18 Buff field type drift");
if (fStatsName.FieldType.FullName != "System.String" || fStatsValue.FieldType.FullName != "System.Single" || fStatsMulti.FieldType.FullName != "System.Boolean")
    throw new InvalidDataException("HF18 StatsIncreased field type drift");
if (fBuffs.FieldType is not GenericInstanceType listType || listType.GenericArguments.Count != 1 || listType.GenericArguments[0].FullName != buff.FullName)
    throw new InvalidDataException("HF18 expected BuffManager.buffs List<Buff>");
foreach (var f in new[] { fToAdd, fToRemove, fChildren })
    if (f.FieldType.FullName != listType.FullName) throw new InvalidDataException($"HF18 expected List<Buff> field {f.FullName}");

var buffCtor = M(buff, ".ctor", 0);
var computing = M(buff, "ComputingIncrement", 2);
var managerCtor = M(manager, ".ctor", 0);
var getIncrement = M(manager, "GetIncrement", 2);
var endAll = M(manager, "EndAll", 1);
var findBuff = M(manager, "FindBuff", 1);
var findStats = M(manager, "FindStatsIncreased", 1);
var managerUpdate = M(manager, "Update", 1);
var computingS = M(stats, "ComputingIncrementS", 2);
var endDef = M(buff, "End", 2);
var attackRangeCtor = M(attackRange, ".ctor", 0);

var expectedTokens = new Dictionary<MethodDefinition,uint>
{
    [computing] = 0x060000F3,
    [buffCtor] = 0x060000F5,
    [getIncrement] = 0x06000104,
    [endAll] = 0x06000105,
    [findBuff] = 0x06000106,
    [findStats] = 0x06000107,
    [managerCtor] = 0x06000108,
};
foreach (var (m,tok) in expectedTokens)
    if (m.MetadataToken.ToUInt32() != tok) throw new InvalidDataException($"HF18 token drift {m.FullName}: 0x{m.MetadataToken.ToUInt32():X8}");

var oldBuffCtorRefs = buffCtor.Body.Instructions.Select(i => i.Operand).OfType<MethodReference>().ToList();
var oldManagerCtorRefs = managerCtor.Body.Instructions.Select(i => i.Operand).OfType<MethodReference>().ToList();
var listCtor = oldBuffCtorRefs.Concat(oldManagerCtorRefs).FirstOrDefault(r => r.Name == ".ctor" && r.Parameters.Count == 0 && r.DeclaringType.FullName == listType.FullName)
    ?? throw new InvalidDataException("HF18 missing List<Buff>::.ctor reference");
var stringEmpty = buffCtor.Body.Instructions.Select(i => i.Operand).OfType<FieldReference>()
    .FirstOrDefault(f => f.DeclaringType.FullName == "System.String" && f.Name == "Empty")
    ?? throw new InvalidDataException("HF18 missing System.String.Empty FieldRef");
var objectCtor = Ref("System.Object", ".ctor");
var stringEq = Ref("System.String", "op_Equality", "System.String", "System.String");
var nreCtor = Ref("System.NullReferenceException", ".ctor");

var updateRefs = managerUpdate.Body.Instructions.Select(i => i.Operand).OfType<MethodReference>().ToList();
var getEnumerator = updateRefs.FirstOrDefault(r => r.Name == "GetEnumerator" && r.DeclaringType.FullName == listType.FullName && r.Parameters.Count == 0)
    ?? throw new InvalidDataException("HF18 missing clean List<Buff>.GetEnumerator reference from HF11 Update");
var moveNext = updateRefs.FirstOrDefault(r => r.Name == "MoveNext" && r.DeclaringType.FullName.Contains("List`1/Enumerator", StringComparison.Ordinal) && r.Parameters.Count == 0)
    ?? throw new InvalidDataException("HF18 missing Enumerator.MoveNext reference");
var getCurrent = updateRefs.FirstOrDefault(r => r.Name == "get_Current" && r.DeclaringType.FullName.Contains("List`1/Enumerator", StringComparison.Ordinal) && r.Parameters.Count == 0)
    ?? throw new InvalidDataException("HF18 missing Enumerator.get_Current reference");
var dispose = updateRefs.FirstOrDefault(r => r.Name == "Dispose" && r.DeclaringType.FullName.Contains("List`1/Enumerator", StringComparison.Ordinal) && r.Parameters.Count == 0)
    ?? throw new InvalidDataException("HF18 missing Enumerator.Dispose reference");
var enumType = getEnumerator.ReturnType;

var getItem = endAll.Body.Instructions.Select(i => i.Operand).OfType<MethodReference>()
    .FirstOrDefault(r => r.Name == "get_Item" && r.DeclaringType.FullName == listType.FullName && r.Parameters.Count == 1)
    ?? new MethodReference("get_Item", buff, listType) { HasThis = true, Parameters = { new ParameterDefinition(module.TypeSystem.Int32) } };
var getCount = new MethodReference("get_Count", module.TypeSystem.Int32, listType) { HasThis = true };

PatchBuffCtor();
PatchComputingIncrement();
PatchManagerCtor();
PatchGetIncrement();
PatchEndAll();
PatchFindBuff();
PatchFindStats();

Directory.CreateDirectory(Path.GetDirectoryName(output)!);
module.Write(output, new WriterParameters { WriteSymbols = false });

using (var verify = ModuleDefinition.ReadModule(output, new ReaderParameters { InMemory = true, ReadSymbols = false }))
{
    var types = All(verify.Types).ToList();
    var b = types.Single(t => t.Name == "Buff");
    var bm = types.Single(t => t.Name == "BuffManager");
    var targets = new[]
    {
        ("Buff.ComputingIncrement", b.Methods.Single(m => m.Name == "ComputingIncrement" && m.Parameters.Count == 2), 55, 1),
        ("Buff..ctor", b.Methods.Single(m => m.Name == ".ctor" && m.Parameters.Count == 0), 12, 0),
        ("BuffManager.GetIncrement", bm.Methods.Single(m => m.Name == "GetIncrement" && m.Parameters.Count == 2), 30, 1),
        ("BuffManager.EndAll", bm.Methods.Single(m => m.Name == "EndAll" && m.Parameters.Count == 1), 25, 0),
        ("BuffManager.FindBuff", bm.Methods.Single(m => m.Name == "FindBuff" && m.Parameters.Count == 1), 30, 1),
        ("BuffManager.FindStatsIncreased", bm.Methods.Single(m => m.Name == "FindStatsIncreased" && m.Parameters.Count == 1), 3, 0),
        ("BuffManager..ctor", bm.Methods.Single(m => m.Name == ".ctor" && m.Parameters.Count == 0), 10, 0),
    };
    foreach (var (name,m,min,finallyCount) in targets)
    {
        if (!m.HasBody || m.Body.Instructions.Count < min) throw new InvalidDataException($"HF18 {name} body too small: {m.Body.Instructions.Count}");
        if (m.Body.ExceptionHandlers.Count != finallyCount || m.Body.ExceptionHandlers.Any(e => e.HandlerType != ExceptionHandlerType.Finally))
            throw new InvalidDataException($"HF18 {name} EH mismatch: {m.Body.ExceptionHandlers.Count}");
        if (m.Body.Instructions.Select(i => i.Operand).OfType<MethodReference>().Any(r => r.DeclaringType.FullName.Contains("Cpp2ILHelpers", StringComparison.Ordinal)))
            throw new InvalidDataException($"HF18 {name} still contains Cpp2IL helper calls");
        Console.WriteLine($"VERIFY {name}: token=0x{m.MetadataToken.ToUInt32():X8}, IL={m.Body.Instructions.Count}, bytes={m.Body.CodeSize}, EH={m.Body.ExceptionHandlers.Count}");
    }

    var comp = b.Methods.Single(m => m.Name == "ComputingIncrement" && m.Parameters.Count == 2);
    var compRefs = comp.Body.Instructions.Select(i => i.Operand).OfType<MethodReference>().ToList();
    if (comp.Body.Instructions.Count(i => i.OpCode == OpCodes.Isinst && i.Operand is TypeReference t && t.Name == "StatsIncreased") != 2)
        throw new InvalidDataException("HF18 ComputingIncrement must retain self + child StatsIncreased gates");
    if (compRefs.Count(r => r.Name == "ComputingIncrementS" && r.DeclaringType.Name == "StatsIncreased") != 1)
        throw new InvalidDataException("HF18 ComputingIncrement must call ComputingIncrementS exactly once in IL");
    if (compRefs.Count(r => r.Name == "op_Equality" && r.DeclaringType.FullName == "System.String") != 1)
        throw new InvalidDataException("HF18 ComputingIncrement must contain one string equality site");

    var gi = bm.Methods.Single(m => m.Name == "GetIncrement" && m.Parameters.Count == 2);
    if (gi.Body.Instructions.Select(i => i.Operand).OfType<MethodReference>().Count(r => r.Name == "ComputingIncrement" && r.DeclaringType.Name == "Buff") != 1)
        throw new InvalidDataException("HF18 GetIncrement must call Buff.ComputingIncrement exactly once in IL");

    var fb = bm.Methods.Single(m => m.Name == "FindBuff" && m.Parameters.Count == 1);
    if (fb.Body.Instructions.Select(i => i.Operand).OfType<MethodReference>().Count(r => r.Name == "op_Equality" && r.DeclaringType.FullName == "System.String") != 1)
        throw new InvalidDataException("HF18 FindBuff must contain one string equality site");

    var fs = bm.Methods.Single(m => m.Name == "FindStatsIncreased" && m.Parameters.Count == 1);
    if (fs.Body.Instructions.Count(i => i.OpCode == OpCodes.Isinst && i.Operand is TypeReference t && t.Name == "StatsIncreased") != 1)
        throw new InvalidDataException("HF18 FindStatsIncreased must be an isinst cast");

    var ea = bm.Methods.Single(m => m.Name == "EndAll" && m.Parameters.Count == 1);
    var eaRefs = ea.Body.Instructions.Select(i => i.Operand).OfType<MethodReference>().ToList();
    if (eaRefs.OfType<GenericInstanceMethod>().Count(r => r.Name == "End" && r.DeclaringType.Name == "Buff") != 1)
        throw new InvalidDataException("HF18 EndAll must call Buff.End<T> exactly once in IL");
    if (eaRefs.Count(r => r.Name == "get_Item") != 2) throw new InvalidDataException("HF18 EndAll must retain two native-source list index reads");

    var bc = b.Methods.Single(m => m.Name == ".ctor" && m.Parameters.Count == 0);
    if (bc.Body.Instructions.Count(i => i.OpCode == OpCodes.Stfld && i.Operand is FieldReference f && new[] { "name", "duration", "buffRange", "childBuffs" }.Contains(f.Name)) != 4)
        throw new InvalidDataException("HF18 Buff ctor initialization shape mismatch");
    var bmc = bm.Methods.Single(m => m.Name == ".ctor" && m.Parameters.Count == 0);
    if (bmc.Body.Instructions.Count(i => i.OpCode == OpCodes.Stfld && i.Operand is FieldReference f && new[] { "buffs", "buffs_toAdd", "buffs_toRemove" }.Contains(f.Name)) != 3)
        throw new InvalidDataException("HF18 BuffManager ctor initialization shape mismatch");
}

Console.WriteLine("HF18 Buff infrastructure restored from PC native x86-64: Buff ctor/ComputingIncrement + BuffManager ctor/GetIncrement/EndAll/FindBuff/FindStatsIncreased");
Console.WriteLine($"WROTE {output} ({new FileInfo(output).Length} bytes)");
return 0;

void AddFinally(MethodBody body, Instruction tryStart, Instruction finallyStart, Instruction afterFinally)
{
    body.ExceptionHandlers.Add(new ExceptionHandler(ExceptionHandlerType.Finally)
    {
        TryStart = tryStart,
        TryEnd = finallyStart,
        HandlerStart = finallyStart,
        HandlerEnd = afterFinally
    });
}

void PatchBuffCtor()
{
    var body = new MethodBody(buffCtor) { InitLocals = false, MaxStackSize = 3 }; buffCtor.Body = body;
    var il = body.GetILProcessor();
    // Source-valid placement of the empty System.Object constructor is managed-observable equivalent
    // to the PC optimizer's tail jump to that empty body.
    E(il, OpCodes.Ldarg_0); E(il, OpCodes.Call, objectCtor);
    E(il, OpCodes.Ldarg_0); E(il, OpCodes.Ldsfld, stringEmpty); E(il, OpCodes.Stfld, fBuffName);
    E(il, OpCodes.Ldarg_0); E(il, OpCodes.Ldc_R4, 1f); E(il, OpCodes.Stfld, fDuration);
    E(il, OpCodes.Ldarg_0); E(il, OpCodes.Newobj, attackRangeCtor); E(il, OpCodes.Stfld, fBuffRange);
    E(il, OpCodes.Ldarg_0); E(il, OpCodes.Newobj, listCtor); E(il, OpCodes.Stfld, fChildren);
    E(il, OpCodes.Ret);
}

void PatchManagerCtor()
{
    var body = new MethodBody(managerCtor) { InitLocals = false, MaxStackSize = 2 }; managerCtor.Body = body;
    var il = body.GetILProcessor();
    E(il, OpCodes.Ldarg_0); E(il, OpCodes.Call, objectCtor);
    foreach (var f in new[] { fBuffs, fToAdd, fToRemove })
    {
        E(il, OpCodes.Ldarg_0); E(il, OpCodes.Newobj, listCtor); E(il, OpCodes.Stfld, f);
    }
    E(il, OpCodes.Ret);
}

void PatchComputingIncrement()
{
    var body = new MethodBody(computing) { InitLocals = true, MaxStackSize = 4 }; computing.Body = body;
    var il = body.GetILProcessor();
    var total = new VariableDefinition(module.TypeSystem.Single);
    var contribution = new VariableDefinition(module.TypeSystem.Single);
    var selfStats = new VariableDefinition(stats);
    var en = new VariableDefinition(enumType);
    var child = new VariableDefinition(buff);
    var childStats = new VariableDefinition(stats);
    foreach (var v in new[] { total, contribution, selfStats, en, child, childStats }) body.Variables.Add(v);

    var afterOwn = Instruction.Create(OpCodes.Nop);
    var haveContribution = Instruction.Create(OpCodes.Nop);
    var tryStart = Instruction.Create(OpCodes.Nop);
    var loopCheck = Instruction.Create(OpCodes.Nop);
    var loopBody = Instruction.Create(OpCodes.Nop);
    var finallyStart = Instruction.Create(OpCodes.Nop);
    var afterFinally = Instruction.Create(OpCodes.Nop);

    E(il, OpCodes.Ldc_R4, 0f); E(il, OpCodes.Stloc, total);
    E(il, OpCodes.Ldarg_0); E(il, OpCodes.Isinst, stats); E(il, OpCodes.Stloc, selfStats);
    E(il, OpCodes.Ldloc, selfStats); E(il, OpCodes.Brfalse, afterOwn);
    E(il, OpCodes.Ldarg_2); E(il, OpCodes.Ldloc, selfStats); E(il, OpCodes.Ldfld, fStatsName); E(il, OpCodes.Call, stringEq); E(il, OpCodes.Brfalse, afterOwn);
    E(il, OpCodes.Ldloc, selfStats); E(il, OpCodes.Ldfld, fStatsValue); E(il, OpCodes.Stloc, contribution);
    E(il, OpCodes.Ldloc, selfStats); E(il, OpCodes.Ldfld, fStatsMulti); E(il, OpCodes.Brfalse, haveContribution);
    E(il, OpCodes.Ldloc, contribution); E(il, OpCodes.Ldarg_1); E(il, OpCodes.Mul); E(il, OpCodes.Stloc, contribution);
    il.Append(haveContribution);
    E(il, OpCodes.Ldloc, total); E(il, OpCodes.Ldloc, contribution); E(il, OpCodes.Add); E(il, OpCodes.Stloc, total);
    il.Append(afterOwn);

    E(il, OpCodes.Ldarg_0); E(il, OpCodes.Ldfld, fChildren); E(il, OpCodes.Callvirt, getEnumerator); E(il, OpCodes.Stloc, en);
    il.Append(tryStart); E(il, OpCodes.Br, loopCheck);
    il.Append(loopBody);
    E(il, OpCodes.Ldloca, en); E(il, OpCodes.Call, getCurrent); E(il, OpCodes.Stloc, child);
    E(il, OpCodes.Ldloc, child); E(il, OpCodes.Brfalse, loopCheck); // native skips null child buffs
    E(il, OpCodes.Ldloc, child); E(il, OpCodes.Isinst, stats); E(il, OpCodes.Stloc, childStats);
    E(il, OpCodes.Ldloc, childStats); E(il, OpCodes.Brfalse, loopCheck);
    E(il, OpCodes.Ldloc, total); E(il, OpCodes.Ldloc, childStats); E(il, OpCodes.Ldarg_1); E(il, OpCodes.Ldarg_2); E(il, OpCodes.Call, computingS); E(il, OpCodes.Add); E(il, OpCodes.Stloc, total);
    il.Append(loopCheck);
    E(il, OpCodes.Ldloca, en); E(il, OpCodes.Call, moveNext); E(il, OpCodes.Brtrue, loopBody); E(il, OpCodes.Leave, afterFinally);
    il.Append(finallyStart); E(il, OpCodes.Ldloca, en); E(il, OpCodes.Call, dispose); E(il, OpCodes.Endfinally);
    il.Append(afterFinally); E(il, OpCodes.Ldloc, total); E(il, OpCodes.Ret);
    AddFinally(body, tryStart, finallyStart, afterFinally);
}

void PatchGetIncrement()
{
    var body = new MethodBody(getIncrement) { InitLocals = true, MaxStackSize = 4 }; getIncrement.Body = body;
    var il = body.GetILProcessor();
    var total = new VariableDefinition(module.TypeSystem.Single);
    var en = new VariableDefinition(enumType);
    var cur = new VariableDefinition(buff);
    foreach (var v in new[] { total, en, cur }) body.Variables.Add(v);
    var tryStart = Instruction.Create(OpCodes.Nop);
    var loopCheck = Instruction.Create(OpCodes.Nop);
    var loopBody = Instruction.Create(OpCodes.Nop);
    var curOk = Instruction.Create(OpCodes.Nop);
    var finallyStart = Instruction.Create(OpCodes.Nop);
    var afterFinally = Instruction.Create(OpCodes.Nop);

    E(il, OpCodes.Ldc_R4, 0f); E(il, OpCodes.Stloc, total);
    E(il, OpCodes.Ldarg_0); E(il, OpCodes.Ldfld, fBuffs); E(il, OpCodes.Callvirt, getEnumerator); E(il, OpCodes.Stloc, en);
    il.Append(tryStart); E(il, OpCodes.Br, loopCheck);
    il.Append(loopBody);
    E(il, OpCodes.Ldloca, en); E(il, OpCodes.Call, getCurrent); E(il, OpCodes.Stloc, cur);
    E(il, OpCodes.Ldloc, cur); E(il, OpCodes.Brtrue, curOk); E(il, OpCodes.Newobj, nreCtor); E(il, OpCodes.Throw);
    il.Append(curOk);
    E(il, OpCodes.Ldloc, total); E(il, OpCodes.Ldloc, cur); E(il, OpCodes.Ldarg_1); E(il, OpCodes.Ldarg_2); E(il, OpCodes.Call, computing); E(il, OpCodes.Add); E(il, OpCodes.Stloc, total);
    il.Append(loopCheck);
    E(il, OpCodes.Ldloca, en); E(il, OpCodes.Call, moveNext); E(il, OpCodes.Brtrue, loopBody); E(il, OpCodes.Leave, afterFinally);
    il.Append(finallyStart); E(il, OpCodes.Ldloca, en); E(il, OpCodes.Call, dispose); E(il, OpCodes.Endfinally);
    il.Append(afterFinally); E(il, OpCodes.Ldloc, total); E(il, OpCodes.Ret);
    AddFinally(body, tryStart, finallyStart, afterFinally);
}

void PatchFindBuff()
{
    var body = new MethodBody(findBuff) { InitLocals = true, MaxStackSize = 3 }; findBuff.Body = body;
    var il = body.GetILProcessor();
    var result = new VariableDefinition(buff);
    var en = new VariableDefinition(enumType);
    var cur = new VariableDefinition(buff);
    foreach (var v in new[] { result, en, cur }) body.Variables.Add(v);
    var tryStart = Instruction.Create(OpCodes.Nop);
    var loopCheck = Instruction.Create(OpCodes.Nop);
    var loopBody = Instruction.Create(OpCodes.Nop);
    var curOk = Instruction.Create(OpCodes.Nop);
    var finallyStart = Instruction.Create(OpCodes.Nop);
    var afterFinally = Instruction.Create(OpCodes.Nop);

    E(il, OpCodes.Ldnull); E(il, OpCodes.Stloc, result);
    E(il, OpCodes.Ldarg_0); E(il, OpCodes.Ldfld, fBuffs); E(il, OpCodes.Callvirt, getEnumerator); E(il, OpCodes.Stloc, en);
    il.Append(tryStart); E(il, OpCodes.Br, loopCheck);
    il.Append(loopBody);
    E(il, OpCodes.Ldloca, en); E(il, OpCodes.Call, getCurrent); E(il, OpCodes.Stloc, cur);
    E(il, OpCodes.Ldloc, cur); E(il, OpCodes.Brtrue, curOk); E(il, OpCodes.Newobj, nreCtor); E(il, OpCodes.Throw);
    il.Append(curOk);
    E(il, OpCodes.Ldloc, cur); E(il, OpCodes.Ldfld, fBuffName); E(il, OpCodes.Ldarg_1); E(il, OpCodes.Call, stringEq); E(il, OpCodes.Brfalse, loopCheck);
    E(il, OpCodes.Ldloc, cur); E(il, OpCodes.Stloc, result); E(il, OpCodes.Leave, afterFinally);
    il.Append(loopCheck);
    E(il, OpCodes.Ldloca, en); E(il, OpCodes.Call, moveNext); E(il, OpCodes.Brtrue, loopBody); E(il, OpCodes.Leave, afterFinally);
    il.Append(finallyStart); E(il, OpCodes.Ldloca, en); E(il, OpCodes.Call, dispose); E(il, OpCodes.Endfinally);
    il.Append(afterFinally); E(il, OpCodes.Ldloc, result); E(il, OpCodes.Ret);
    AddFinally(body, tryStart, finallyStart, afterFinally);
}

void PatchFindStats()
{
    var body = new MethodBody(findStats) { InitLocals = false, MaxStackSize = 2 }; findStats.Body = body;
    var il = body.GetILProcessor();
    E(il, OpCodes.Ldarg_0); E(il, OpCodes.Ldarg_1); E(il, OpCodes.Call, findBuff); E(il, OpCodes.Isinst, stats); E(il, OpCodes.Ret);
}

void PatchEndAll()
{
    var body = new MethodBody(endAll) { InitLocals = true, MaxStackSize = 4 }; endAll.Body = body;
    var il = body.GetILProcessor();
    var i = new VariableDefinition(module.TypeSystem.Int32);
    var first = new VariableDefinition(buff);
    var second = new VariableDefinition(buff);
    body.Variables.Add(i); body.Variables.Add(first); body.Variables.Add(second);
    var hostT = endAll.GenericParameters.Single();
    var end = new GenericInstanceMethod(endDef); end.GenericArguments.Add(hostT);
    var loopCheck = Instruction.Create(OpCodes.Nop);
    var loopBody = Instruction.Create(OpCodes.Nop);
    var decrement = Instruction.Create(OpCodes.Nop);
    var secondOk = Instruction.Create(OpCodes.Nop);
    var ret = Instruction.Create(OpCodes.Ret);

    E(il, OpCodes.Ldarg_0); E(il, OpCodes.Ldfld, fBuffs); E(il, OpCodes.Callvirt, getCount); E(il, OpCodes.Ldc_I4_1); E(il, OpCodes.Sub); E(il, OpCodes.Stloc, i);
    E(il, OpCodes.Br, loopCheck);
    il.Append(loopBody);
    E(il, OpCodes.Ldarg_0); E(il, OpCodes.Ldfld, fBuffs); E(il, OpCodes.Ldloc, i); E(il, OpCodes.Callvirt, getItem); E(il, OpCodes.Stloc, first);
    E(il, OpCodes.Ldloc, first); E(il, OpCodes.Brfalse, decrement);
    // Native/source performs a second list index read before invoking End<T>.
    E(il, OpCodes.Ldarg_0); E(il, OpCodes.Ldfld, fBuffs); E(il, OpCodes.Ldloc, i); E(il, OpCodes.Callvirt, getItem); E(il, OpCodes.Stloc, second);
    E(il, OpCodes.Ldloc, second); E(il, OpCodes.Brtrue, secondOk); E(il, OpCodes.Newobj, nreCtor); E(il, OpCodes.Throw);
    il.Append(secondOk);
    E(il, OpCodes.Ldloc, second); E(il, OpCodes.Ldarg_1); E(il, OpCodes.Ldc_I4_0); E(il, OpCodes.Call, end);
    il.Append(decrement);
    E(il, OpCodes.Ldloc, i); E(il, OpCodes.Ldc_I4_1); E(il, OpCodes.Sub); E(il, OpCodes.Stloc, i);
    il.Append(loopCheck); E(il, OpCodes.Ldloc, i); E(il, OpCodes.Ldc_I4_0); E(il, OpCodes.Bge, loopBody);
    il.Append(ret);
}
