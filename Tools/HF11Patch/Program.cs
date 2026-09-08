using Mono.Cecil;
using Mono.Cecil.Cil;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

if (args.Length != 2)
{
    Console.Error.WriteLine("Usage: GodsPVZ.HF11Patch <input.dll> <output.dll>");
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
void E(ILProcessor il, OpCode op, object? value = null)
{
    var ins = value switch
    {
        null => Instruction.Create(op),
        int i => Instruction.Create(op, i),
        MethodReference mr => Instruction.Create(op, mr),
        FieldReference fr => Instruction.Create(op, fr),
        TypeReference tr => Instruction.Create(op, tr),
        VariableDefinition vd => Instruction.Create(op, vd),
        Instruction target => Instruction.Create(op, target),
        _ => throw new NotSupportedException(value.GetType().FullName)
    };
    il.Append(ins);
}

var manager = T("BuffManager");
var buff = T("Buff");
PatchUpdateGeneric();

Directory.CreateDirectory(Path.GetDirectoryName(output)!);
module.Write(output, new WriterParameters { WriteSymbols = false });

using (var verify = ModuleDefinition.ReadModule(output, new ReaderParameters { InMemory = true, ReadSymbols = false }))
{
    var bm = All(verify.Types).Single(t => t.Name == "BuffManager");
    var m = bm.Methods.Single(x => x.Name == "Update" && x.Parameters.Count == 1 && x.GenericParameters.Count == 1);
    if (m.MetadataToken.ToUInt32() != 0x06000101) throw new InvalidDataException($"HF11 token drift: 0x{m.MetadataToken.ToUInt32():X8}");
    if (!m.HasBody || m.Body.Instructions.Count < 120) throw new InvalidDataException($"HF11 body too small: {m.Body.Instructions.Count}");
    if (m.Body.ExceptionHandlers.Count != 3 || m.Body.ExceptionHandlers.Any(e => e.HandlerType != ExceptionHandlerType.Finally))
        throw new InvalidDataException("HF11 must retain three List<Buff>.Enumerator finally/Dispose regions");
    var refs = m.Body.Instructions.Select(i => i.Operand).OfType<MethodReference>().ToList();
    if (refs.Any(r => r.DeclaringType.FullName.Contains("Cpp2ILHelpers", StringComparison.Ordinal)))
        throw new InvalidDataException("HF11 still contains Cpp2IL helper calls");
    foreach (var n in new[] { "GetEnumerator", "MoveNext", "get_Current", "Dispose", "Add", "Remove", "Clear", "Start", "Update", "End" })
        if (!refs.Any(r => r.Name == n)) throw new InvalidDataException($"HF11 missing {n}");
    if (refs.Count(r => r.Name == "Dispose") != 3) throw new InvalidDataException("HF11 must retain exactly three enumerator Dispose calls");
    if (refs.Count(r => r.Name == "Clear") != 2) throw new InvalidDataException("HF11 must clear add/remove queues exactly once each");
    foreach (var gi in refs.OfType<GenericInstanceMethod>().Where(r => r.Name is "Start" or "Update" or "End"))
        if (gi.GenericArguments.Count != 1 || gi.GenericArguments[0].MetadataType != MetadataType.MVar)
            throw new InvalidDataException($"HF11 {gi.Name} must be instantiated with BuffManager.Update<T>'s method generic parameter");
    Console.WriteLine($"VERIFY BuffManager.Update<T>: token=0x{m.MetadataToken.ToUInt32():X8}, IL={m.Body.Instructions.Count}, bytes={m.Body.CodeSize}, finally={m.Body.ExceptionHandlers.Count}");
}

Console.WriteLine("HF11 BuffManager.Update<T> shared instance @0x180429970 restored from PC native x86-64");
Console.WriteLine("Native xrefs: Plant.Update_PlantFight @0x18035C155 and Zombie.Update @0x18036D23D");
Console.WriteLine($"WROTE {output} ({new FileInfo(output).Length} bytes)");
return 0;

void PatchUpdateGeneric()
{
    var m = manager.Methods.Single(x => x.Name == "Update" && x.Parameters.Count == 1 && x.GenericParameters.Count == 1);
    if (m.MetadataToken.ToUInt32() != 0x06000101) throw new InvalidDataException($"Unexpected BuffManager.Update<T> token 0x{m.MetadataToken.ToUInt32():X8}");

    var fBuffs = F(manager, "buffs");
    var fAdd = F(manager, "buffs_toAdd");
    var fRemove = F(manager, "buffs_toRemove");
    if (fBuffs.FieldType is not GenericInstanceType listType || listType.GenericArguments.Count != 1 || listType.GenericArguments[0].FullName != buff.FullName)
        throw new InvalidDataException("HF11 expected List<Buff> fields");

    var oldMoveNext = m.Body.Instructions.Select(i => i.Operand).OfType<MethodReference>()
        .FirstOrDefault(r => r.Name == "MoveNext" && r.DeclaringType.FullName.Contains("System.Collections.Generic.List`1/Enumerator", StringComparison.Ordinal))
        ?? throw new InvalidDataException("HF11 could not recover List<T>.Enumerator shape");
    if (oldMoveNext.DeclaringType is not GenericInstanceType oldEnum)
        throw new InvalidDataException("HF11 enumerator reference is not generic");
    var enumType = new GenericInstanceType(oldEnum.ElementType);
    enumType.GenericArguments.Add(buff);

    var getEnumerator = new MethodReference("GetEnumerator", enumType, listType) { HasThis = true };
    var moveNext = new MethodReference("MoveNext", module.TypeSystem.Boolean, enumType) { HasThis = true };
    var getCurrent = new MethodReference("get_Current", buff, enumType) { HasThis = true };
    var dispose = new MethodReference("Dispose", module.TypeSystem.Void, enumType) { HasThis = true };
    var add = new MethodReference("Add", module.TypeSystem.Void, listType) { HasThis = true };
    add.Parameters.Add(new ParameterDefinition(buff));
    var remove = new MethodReference("Remove", module.TypeSystem.Boolean, listType) { HasThis = true };
    remove.Parameters.Add(new ParameterDefinition(buff));
    var clear = new MethodReference("Clear", module.TypeSystem.Void, listType) { HasThis = true };

    var startDef = M(buff, "Start", 2);
    var updateDef = M(buff, "Update", 3);
    var endDef = M(buff, "End", 2);
    if (startDef.GenericParameters.Count != 1 || updateDef.GenericParameters.Count != 1 || endDef.GenericParameters.Count != 1)
        throw new InvalidDataException("HF11 expected generic Buff.Start/Update/End definitions");
    var hostT = m.GenericParameters[0];
    GenericInstanceMethod Bind(MethodDefinition def)
    {
        var gi = new GenericInstanceMethod(def);
        gi.GenericArguments.Add(hostT);
        return gi;
    }
    var start = Bind(startDef);
    var update = Bind(updateDef);
    var end = Bind(endDef);

    var nreCtor = m.Body.Instructions.Select(i => i.Operand).OfType<MethodReference>()
        .FirstOrDefault(r => r.DeclaringType.FullName == "System.NullReferenceException" && r.Name == ".ctor" && r.Parameters.Count == 0)
        ?? throw new InvalidDataException("HF11 could not recover System.NullReferenceException::.ctor reference");

    var b = new MethodBody(m) { InitLocals = true, MaxStackSize = 6 };
    m.Body = b;
    var il = b.GetILProcessor();

    var enAdd = new VariableDefinition(enumType);
    var enBuffs = new VariableDefinition(enumType);
    var enRemove = new VariableDefinition(enumType);
    var current = new VariableDefinition(buff);
    foreach (var v in new[] { enAdd, enBuffs, enRemove, current }) b.Variables.Add(v);

    var try1 = Instruction.Create(OpCodes.Nop);
    var loop1 = Instruction.Create(OpCodes.Nop);
    var body1 = Instruction.Create(OpCodes.Nop);
    var finally1 = Instruction.Create(OpCodes.Nop);
    var after1 = Instruction.Create(OpCodes.Nop);
    E(il, OpCodes.Ldarg_0); E(il, OpCodes.Ldfld, fAdd); E(il, OpCodes.Callvirt, getEnumerator); E(il, OpCodes.Stloc, enAdd);
    il.Append(try1); E(il, OpCodes.Br, loop1);
    il.Append(body1);
    E(il, OpCodes.Ldloca, enAdd); E(il, OpCodes.Call, getCurrent); E(il, OpCodes.Stloc, current);
    EmitRequireCurrent(il, current, nreCtor);
    E(il, OpCodes.Ldarg_0); E(il, OpCodes.Ldfld, fBuffs); E(il, OpCodes.Ldloc, current); E(il, OpCodes.Callvirt, add);
    E(il, OpCodes.Ldloc, current); E(il, OpCodes.Ldarg_1); E(il, OpCodes.Ldc_I4_0); E(il, OpCodes.Call, start);
    il.Append(loop1); E(il, OpCodes.Ldloca, enAdd); E(il, OpCodes.Call, moveNext); E(il, OpCodes.Brtrue, body1); E(il, OpCodes.Leave, after1);
    il.Append(finally1); E(il, OpCodes.Ldloca, enAdd); E(il, OpCodes.Call, dispose); E(il, OpCodes.Endfinally);
    il.Append(after1);
    b.ExceptionHandlers.Add(new ExceptionHandler(ExceptionHandlerType.Finally) { TryStart = try1, TryEnd = finally1, HandlerStart = finally1, HandlerEnd = after1 });
    E(il, OpCodes.Ldarg_0); E(il, OpCodes.Ldfld, fAdd); E(il, OpCodes.Callvirt, clear);

    var try2 = Instruction.Create(OpCodes.Nop);
    var loop2 = Instruction.Create(OpCodes.Nop);
    var body2 = Instruction.Create(OpCodes.Nop);
    var finally2 = Instruction.Create(OpCodes.Nop);
    var after2 = Instruction.Create(OpCodes.Nop);
    E(il, OpCodes.Ldarg_0); E(il, OpCodes.Ldfld, fBuffs); E(il, OpCodes.Callvirt, getEnumerator); E(il, OpCodes.Stloc, enBuffs);
    il.Append(try2); E(il, OpCodes.Br, loop2);
    il.Append(body2);
    E(il, OpCodes.Ldloca, enBuffs); E(il, OpCodes.Call, getCurrent); E(il, OpCodes.Stloc, current);
    EmitRequireCurrent(il, current, nreCtor);
    E(il, OpCodes.Ldloc, current); E(il, OpCodes.Ldarg_1); E(il, OpCodes.Ldc_I4_0); E(il, OpCodes.Ldarg_0); E(il, OpCodes.Call, update);
    il.Append(loop2); E(il, OpCodes.Ldloca, enBuffs); E(il, OpCodes.Call, moveNext); E(il, OpCodes.Brtrue, body2); E(il, OpCodes.Leave, after2);
    il.Append(finally2); E(il, OpCodes.Ldloca, enBuffs); E(il, OpCodes.Call, dispose); E(il, OpCodes.Endfinally);
    il.Append(after2);
    b.ExceptionHandlers.Add(new ExceptionHandler(ExceptionHandlerType.Finally) { TryStart = try2, TryEnd = finally2, HandlerStart = finally2, HandlerEnd = after2 });

    var try3 = Instruction.Create(OpCodes.Nop);
    var loop3 = Instruction.Create(OpCodes.Nop);
    var body3 = Instruction.Create(OpCodes.Nop);
    var finally3 = Instruction.Create(OpCodes.Nop);
    var after3 = Instruction.Create(OpCodes.Nop);
    E(il, OpCodes.Ldarg_0); E(il, OpCodes.Ldfld, fRemove); E(il, OpCodes.Callvirt, getEnumerator); E(il, OpCodes.Stloc, enRemove);
    il.Append(try3); E(il, OpCodes.Br, loop3);
    il.Append(body3);
    E(il, OpCodes.Ldloca, enRemove); E(il, OpCodes.Call, getCurrent); E(il, OpCodes.Stloc, current);
    EmitRequireCurrent(il, current, nreCtor);
    E(il, OpCodes.Ldarg_0); E(il, OpCodes.Ldfld, fBuffs); E(il, OpCodes.Ldloc, current); E(il, OpCodes.Callvirt, remove); E(il, OpCodes.Pop);
    E(il, OpCodes.Ldloc, current); E(il, OpCodes.Ldarg_1); E(il, OpCodes.Ldc_I4_0); E(il, OpCodes.Call, end);
    il.Append(loop3); E(il, OpCodes.Ldloca, enRemove); E(il, OpCodes.Call, moveNext); E(il, OpCodes.Brtrue, body3); E(il, OpCodes.Leave, after3);
    il.Append(finally3); E(il, OpCodes.Ldloca, enRemove); E(il, OpCodes.Call, dispose); E(il, OpCodes.Endfinally);
    il.Append(after3);
    b.ExceptionHandlers.Add(new ExceptionHandler(ExceptionHandlerType.Finally) { TryStart = try3, TryEnd = finally3, HandlerStart = finally3, HandlerEnd = after3 });
    E(il, OpCodes.Ldarg_0); E(il, OpCodes.Ldfld, fRemove); E(il, OpCodes.Callvirt, clear);
    E(il, OpCodes.Ret);
}

void EmitRequireCurrent(ILProcessor il, VariableDefinition current, MethodReference nreCtor)
{
    var ok = Instruction.Create(OpCodes.Nop);
    E(il, OpCodes.Ldloc, current); E(il, OpCodes.Brtrue, ok);
    E(il, OpCodes.Newobj, nreCtor); E(il, OpCodes.Throw);
    il.Append(ok);
}
