using Mono.Cecil;
using Mono.Cecil.Cil;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

if (args.Length != 2)
{
    Console.Error.WriteLine("Usage: GodsPVZ.HF13Patch <input.dll> <output.dll>");
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
var hide = T("Hide");
PatchStart();
PatchEnd();

Directory.CreateDirectory(Path.GetDirectoryName(output)!);
module.Write(output, new WriterParameters { WriteSymbols = false });

using (var verify = ModuleDefinition.ReadModule(output, new ReaderParameters { InMemory = true, ReadSymbols = false }))
{
    var b = All(verify.Types).Single(t => t.Name == "Buff");
    var start = b.Methods.Single(x => x.Name == "Start" && x.Parameters.Count == 2 && x.GenericParameters.Count == 1);
    var end = b.Methods.Single(x => x.Name == "End" && x.Parameters.Count == 2 && x.GenericParameters.Count == 1);
    if (start.MetadataToken.ToUInt32() != 0x060000F1) throw new InvalidDataException($"HF13 Start token drift: 0x{start.MetadataToken.ToUInt32():X8}");
    if (end.MetadataToken.ToUInt32() != 0x060000F4) throw new InvalidDataException($"HF13 End token drift: 0x{end.MetadataToken.ToUInt32():X8}");
    foreach (var (name,m,min) in new[] { ("Start",start,40), ("End",end,55) })
    {
        if (!m.HasBody || m.Body.Instructions.Count < min) throw new InvalidDataException($"HF13 {name} body too small: {m.Body.Instructions.Count}");
        if (m.Body.ExceptionHandlers.Count != 1 || m.Body.ExceptionHandlers[0].HandlerType != ExceptionHandlerType.Finally)
            throw new InvalidDataException($"HF13 {name} must retain one childBuff Enumerator finally/Dispose");
        var refs = m.Body.Instructions.Select(i => i.Operand).OfType<MethodReference>().ToList();
        if (refs.Any(r => r.DeclaringType.FullName.Contains("Cpp2ILHelpers", StringComparison.Ordinal)))
            throw new InvalidDataException($"HF13 {name} still contains Cpp2IL helper calls");
        if (refs.Count(r => r.Name == "Dispose") != 1) throw new InvalidDataException($"HF13 {name} must dispose exactly one child enumerator");
    }
    var srefs = start.Body.Instructions.Select(i => i.Operand).OfType<MethodReference>().ToList();
    var erefs = end.Body.Instructions.Select(i => i.Operand).OfType<MethodReference>().ToList();
    if (srefs.Count(r => r.Name == "Start_stats") != 1) throw new InvalidDataException("HF13 Start must call Start_stats<T> exactly once");
    if (srefs.OfType<GenericInstanceMethod>().Count(r => r.Name == "Start" && r.DeclaringType.Name == "Buff") != 1) throw new InvalidDataException("HF13 Start must recurse exactly once in IL");
    if (!end.Body.Instructions.Any(i => i.OpCode == OpCodes.Ldstr && Equals(i.Operand, "结束buff"))) throw new InvalidDataException("HF13 End missing exact native log string");
    if (erefs.Count(r => r.Name == "End_stats") != 1 || erefs.Count(r => r.Name == "End_Hide") != 1 || erefs.Count(r => r.Name == "Destroy") != 1)
        throw new InvalidDataException("HF13 End missing specialized lifecycle calls");
    if (erefs.OfType<GenericInstanceMethod>().Count(r => r.Name == "End" && r.DeclaringType.Name == "Buff") != 1) throw new InvalidDataException("HF13 End must recurse exactly once in IL");
    Console.WriteLine($"VERIFY Buff.Start<T>: token=0x{start.MetadataToken.ToUInt32():X8}, IL={start.Body.Instructions.Count}, bytes={start.Body.CodeSize}, finally={start.Body.ExceptionHandlers.Count}");
    Console.WriteLine($"VERIFY Buff.End<T>: token=0x{end.MetadataToken.ToUInt32():X8}, IL={end.Body.Instructions.Count}, bytes={end.Body.CodeSize}, finally={end.Body.ExceptionHandlers.Count}");
}

Console.WriteLine("HF13 Buff.Start<T> @0x18042A660 and Buff.End<T> @0x18042A110 restored from PC native x86-64");
Console.WriteLine($"WROTE {output} ({new FileInfo(output).Length} bytes)");
return 0;

(GenericInstanceType listType, GenericInstanceType enumType, MethodReference getEnumerator, MethodReference moveNext, MethodReference getCurrent, MethodReference dispose, MethodReference nreCtor) ChildRefs(MethodDefinition source)
{
    var fChildren = F(buff, "childBuffs");
    if (fChildren.FieldType is not GenericInstanceType listType || listType.GenericArguments.Count != 1 || listType.GenericArguments[0].FullName != buff.FullName)
        throw new InvalidDataException("HF13 expected childBuffs List<Buff>");
    var oldMoveNext = source.Body.Instructions.Select(i => i.Operand).OfType<MethodReference>()
        .FirstOrDefault(r => r.Name == "MoveNext" && r.DeclaringType.FullName.Contains("System.Collections.Generic.List`1/Enumerator", StringComparison.Ordinal))
        ?? throw new InvalidDataException("HF13 could not recover List<T>.Enumerator shape");
    if (oldMoveNext.DeclaringType is not GenericInstanceType oldEnum) throw new InvalidDataException("HF13 enumerator ref is not generic");
    var enumType = new GenericInstanceType(oldEnum.ElementType); enumType.GenericArguments.Add(buff);
    var ge = new MethodReference("GetEnumerator", enumType, listType) { HasThis = true };
    var mn = new MethodReference("MoveNext", module.TypeSystem.Boolean, enumType) { HasThis = true };
    var gc = new MethodReference("get_Current", buff, enumType) { HasThis = true };
    var dp = new MethodReference("Dispose", module.TypeSystem.Void, enumType) { HasThis = true };
    var nre = source.Body.Instructions.Select(i => i.Operand).OfType<MethodReference>()
        .FirstOrDefault(r => r.DeclaringType.FullName == "System.NullReferenceException" && r.Name == ".ctor" && r.Parameters.Count == 0)
        ?? throw new InvalidDataException("HF13 missing NullReferenceException::.ctor reference");
    return (listType, enumType, ge, mn, gc, dp, nre);
}

void PatchStart()
{
    var m = buff.Methods.Single(x => x.Name == "Start" && x.Parameters.Count == 2 && x.GenericParameters.Count == 1);
    if (m.MetadataToken.ToUInt32() != 0x060000F1) throw new InvalidDataException("Unexpected Buff.Start<T> token");
    var children = F(buff, "childBuffs");
    var refs = ChildRefs(m);
    var hostT = m.GenericParameters[0];
    var startStatsDef = M(stats, "Start_stats", 1);
    var startStats = new GenericInstanceMethod(startStatsDef); startStats.GenericArguments.Add(hostT);
    var selfStart = new GenericInstanceMethod(m); selfStart.GenericArguments.Add(hostT);

    var body = new MethodBody(m) { InitLocals = true, MaxStackSize = 6 }; m.Body = body;
    var il = body.GetILProcessor();
    var statsLocal = new VariableDefinition(stats);
    var en = new VariableDefinition(refs.enumType);
    var child = new VariableDefinition(buff);
    foreach (var v in new[] { statsLocal, en, child }) body.Variables.Add(v);
    var ret = Instruction.Create(OpCodes.Ret);
    var afterStats = Instruction.Create(OpCodes.Nop);
    var tryStart = Instruction.Create(OpCodes.Nop);
    var loopCheck = Instruction.Create(OpCodes.Nop);
    var loopBody = Instruction.Create(OpCodes.Nop);
    var finallyStart = Instruction.Create(OpCodes.Nop);
    var afterFinally = Instruction.Create(OpCodes.Nop);
    var childOk = Instruction.Create(OpCodes.Nop);

    E(il, OpCodes.Ldarg_1); E(il, OpCodes.Box, hostT); E(il, OpCodes.Brfalse, ret);
    E(il, OpCodes.Ldarg_0); E(il, OpCodes.Isinst, stats); E(il, OpCodes.Stloc, statsLocal);
    E(il, OpCodes.Ldloc, statsLocal); E(il, OpCodes.Brfalse, afterStats);
    E(il, OpCodes.Ldloc, statsLocal); E(il, OpCodes.Ldarg_1); E(il, OpCodes.Call, startStats);
    il.Append(afterStats);

    E(il, OpCodes.Ldarg_0); E(il, OpCodes.Ldfld, children); E(il, OpCodes.Callvirt, refs.getEnumerator); E(il, OpCodes.Stloc, en);
    il.Append(tryStart); E(il, OpCodes.Br, loopCheck);
    il.Append(loopBody);
    E(il, OpCodes.Ldloca, en); E(il, OpCodes.Call, refs.getCurrent); E(il, OpCodes.Stloc, child);
    E(il, OpCodes.Ldloc, child); E(il, OpCodes.Brtrue, childOk);
    E(il, OpCodes.Newobj, refs.nreCtor); E(il, OpCodes.Throw);
    il.Append(childOk);
    E(il, OpCodes.Ldloc, child); E(il, OpCodes.Ldarg_1); E(il, OpCodes.Ldc_I4_1); E(il, OpCodes.Call, selfStart);
    il.Append(loopCheck);
    E(il, OpCodes.Ldloca, en); E(il, OpCodes.Call, refs.moveNext); E(il, OpCodes.Brtrue, loopBody); E(il, OpCodes.Leave, afterFinally);
    il.Append(finallyStart); E(il, OpCodes.Ldloca, en); E(il, OpCodes.Call, refs.dispose); E(il, OpCodes.Endfinally);
    il.Append(afterFinally); E(il, OpCodes.Ret);
    body.ExceptionHandlers.Add(new ExceptionHandler(ExceptionHandlerType.Finally) { TryStart = tryStart, TryEnd = finallyStart, HandlerStart = finallyStart, HandlerEnd = afterFinally });
    il.Append(ret);
}

void PatchEnd()
{
    var m = buff.Methods.Single(x => x.Name == "End" && x.Parameters.Count == 2 && x.GenericParameters.Count == 1);
    if (m.MetadataToken.ToUInt32() != 0x060000F4) throw new InvalidDataException("Unexpected Buff.End<T> token");
    var children = F(buff, "childBuffs");
    var vfx = F(buff, "vfx");
    var refs = ChildRefs(m);
    var hostT = m.GenericParameters[0];
    var endStatsDef = M(stats, "End_stats", 1); var endStats = new GenericInstanceMethod(endStatsDef); endStats.GenericArguments.Add(hostT);
    var endHide = M(hide, "End_Hide", 0);
    var selfEnd = new GenericInstanceMethod(m); selfEnd.GenericArguments.Add(hostT);
    var debugLog = Ref("UnityEngine.Debug", "Log", "System.Object");
    var objectNe = Ref("UnityEngine.Object", "op_Inequality", "UnityEngine.Object", "UnityEngine.Object");
    var getGameObject = Ref("UnityEngine.GameObject", "get_gameObject");
    var destroy = Ref("UnityEngine.Object", "Destroy", "UnityEngine.Object");

    var body = new MethodBody(m) { InitLocals = true, MaxStackSize = 6 }; m.Body = body;
    var il = body.GetILProcessor();
    var statsLocal = new VariableDefinition(stats);
    var hideLocal = new VariableDefinition(hide);
    var en = new VariableDefinition(refs.enumType);
    var child = new VariableDefinition(buff);
    foreach (var v in new[] { statsLocal, hideLocal, en, child }) body.Variables.Add(v);
    var noVfx = Instruction.Create(OpCodes.Nop);
    var afterStats = Instruction.Create(OpCodes.Nop);
    var afterHide = Instruction.Create(OpCodes.Nop);
    var tryStart = Instruction.Create(OpCodes.Nop);
    var loopCheck = Instruction.Create(OpCodes.Nop);
    var loopBody = Instruction.Create(OpCodes.Nop);
    var finallyStart = Instruction.Create(OpCodes.Nop);
    var afterFinally = Instruction.Create(OpCodes.Nop);
    var childOk = Instruction.Create(OpCodes.Nop);

    E(il, OpCodes.Ldstr, "结束buff"); E(il, OpCodes.Call, debugLog);
    E(il, OpCodes.Ldarg_0); E(il, OpCodes.Ldfld, vfx); E(il, OpCodes.Ldnull); E(il, OpCodes.Call, objectNe); E(il, OpCodes.Brfalse, noVfx);
    E(il, OpCodes.Ldarg_0); E(il, OpCodes.Ldfld, vfx); E(il, OpCodes.Call, getGameObject); E(il, OpCodes.Call, destroy);
    il.Append(noVfx);

    E(il, OpCodes.Ldarg_0); E(il, OpCodes.Isinst, stats); E(il, OpCodes.Stloc, statsLocal);
    E(il, OpCodes.Ldloc, statsLocal); E(il, OpCodes.Brfalse, afterStats);
    E(il, OpCodes.Ldloc, statsLocal); E(il, OpCodes.Ldarg_1); E(il, OpCodes.Call, endStats);
    il.Append(afterStats);
    E(il, OpCodes.Ldarg_0); E(il, OpCodes.Isinst, hide); E(il, OpCodes.Stloc, hideLocal);
    E(il, OpCodes.Ldloc, hideLocal); E(il, OpCodes.Brfalse, afterHide);
    E(il, OpCodes.Ldloc, hideLocal); E(il, OpCodes.Call, endHide);
    il.Append(afterHide);

    E(il, OpCodes.Ldarg_0); E(il, OpCodes.Ldfld, children); E(il, OpCodes.Callvirt, refs.getEnumerator); E(il, OpCodes.Stloc, en);
    il.Append(tryStart); E(il, OpCodes.Br, loopCheck);
    il.Append(loopBody);
    E(il, OpCodes.Ldloca, en); E(il, OpCodes.Call, refs.getCurrent); E(il, OpCodes.Stloc, child);
    E(il, OpCodes.Ldloc, child); E(il, OpCodes.Brtrue, childOk);
    E(il, OpCodes.Newobj, refs.nreCtor); E(il, OpCodes.Throw);
    il.Append(childOk);
    E(il, OpCodes.Ldloc, child); E(il, OpCodes.Ldarg_1); E(il, OpCodes.Ldc_I4_1); E(il, OpCodes.Call, selfEnd);
    il.Append(loopCheck);
    E(il, OpCodes.Ldloca, en); E(il, OpCodes.Call, refs.moveNext); E(il, OpCodes.Brtrue, loopBody); E(il, OpCodes.Leave, afterFinally);
    il.Append(finallyStart); E(il, OpCodes.Ldloca, en); E(il, OpCodes.Call, refs.dispose); E(il, OpCodes.Endfinally);
    il.Append(afterFinally); E(il, OpCodes.Ret);
    body.ExceptionHandlers.Add(new ExceptionHandler(ExceptionHandlerType.Finally) { TryStart = tryStart, TryEnd = finallyStart, HandlerStart = finallyStart, HandlerEnd = afterFinally });
}
