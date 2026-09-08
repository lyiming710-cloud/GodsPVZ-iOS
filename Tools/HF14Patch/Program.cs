using Mono.Cecil;
using Mono.Cecil.Cil;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

if (args.Length != 2)
{
    Console.Error.WriteLine("Usage: GodsPVZ.HF14Patch <input.dll> <output.dll>");
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

var stats = T("StatsIncreased");
var zombie = T("Zombie");
var plant = T("Plant");
var valueName = F(stats, "valueName");
if (valueName.FieldType.FullName != "System.String") throw new InvalidDataException($"HF14 expected StatsIncreased.valueName:string, got {valueName.FieldType.FullName}");
var resetZombieAttack = M(zombie, "ResetAttackSpeed", 0);
var resetZombieMove = M(zombie, "ResetMoveSpeed", 0);
var resetPlantAttack = M(plant, "ResetAttackSpeed", 0);
var stringEq = Ref("System.String", "op_Equality", "System.String", "System.String");

var start = stats.Methods.Single(m => m.Name == "Start_stats" && m.Parameters.Count == 1 && m.GenericParameters.Count == 1);
var end = stats.Methods.Single(m => m.Name == "End_stats" && m.Parameters.Count == 1 && m.GenericParameters.Count == 1);
if (start.MetadataToken.ToUInt32() != 0x060000F7) throw new InvalidDataException($"HF14 Start_stats token drift: 0x{start.MetadataToken.ToUInt32():X8}");
if (end.MetadataToken.ToUInt32() != 0x060000F9) throw new InvalidDataException($"HF14 End_stats token drift: 0x{end.MetadataToken.ToUInt32():X8}");

PatchStats(start);
PatchStats(end);

Directory.CreateDirectory(Path.GetDirectoryName(output)!);
module.Write(output, new WriterParameters { WriteSymbols = false });

using (var verify = ModuleDefinition.ReadModule(output, new ReaderParameters { InMemory = true, ReadSymbols = false }))
{
    var all = All(verify.Types).ToList();
    var s = all.Single(t => t.Name == "StatsIncreased");
    foreach (var (name, token) in new[] { ("Start_stats", 0x060000F7u), ("End_stats", 0x060000F9u) })
    {
        var m = s.Methods.Single(x => x.Name == name && x.Parameters.Count == 1 && x.GenericParameters.Count == 1);
        if (m.MetadataToken.ToUInt32() != token) throw new InvalidDataException($"HF14 {name} token drift after reopen");
        if (!m.HasBody || m.Body.Instructions.Count < 35) throw new InvalidDataException($"HF14 {name} body too small: {m.Body.Instructions.Count}");
        if (m.Body.ExceptionHandlers.Count != 0) throw new InvalidDataException($"HF14 {name} must not contain exception handlers");
        var refs = m.Body.Instructions.Select(i => i.Operand).OfType<MethodReference>().ToList();
        if (refs.Any(r => r.DeclaringType.FullName.Contains("Cpp2ILHelpers", StringComparison.Ordinal))) throw new InvalidDataException($"HF14 {name} still contains Cpp2IL helper calls");
        if (refs.Count(r => r.DeclaringType.FullName == "Zombie" && r.Name == "ResetAttackSpeed") != 1) throw new InvalidDataException($"HF14 {name} Zombie.ResetAttackSpeed count mismatch");
        if (refs.Count(r => r.DeclaringType.FullName == "Zombie" && r.Name == "ResetMoveSpeed") != 1) throw new InvalidDataException($"HF14 {name} Zombie.ResetMoveSpeed count mismatch");
        if (refs.Count(r => r.DeclaringType.FullName == "Plant" && r.Name == "ResetAttackSpeed") != 1) throw new InvalidDataException($"HF14 {name} Plant.ResetAttackSpeed count mismatch");
        if (refs.Count(r => r.DeclaringType.FullName == "System.String" && r.Name == "op_Equality" && r.Parameters.Count == 2) != 3) throw new InvalidDataException($"HF14 {name} string equality count mismatch");
        if (m.Body.Instructions.Count(i => i.OpCode == OpCodes.Ldstr && Equals(i.Operand, "As")) != 2) throw new InvalidDataException($"HF14 {name} must compare As twice");
        if (m.Body.Instructions.Count(i => i.OpCode == OpCodes.Ldstr && Equals(i.Operand, "Ms")) != 1) throw new InvalidDataException($"HF14 {name} must compare Ms once");
        if (m.Body.Instructions.Count(i => i.OpCode == OpCodes.Ldfld && i.Operand is FieldReference f && f.DeclaringType.Name == "StatsIncreased" && f.Name == "valueName") != 2) throw new InvalidDataException($"HF14 {name} must read valueName exactly twice");
        Console.WriteLine($"VERIFY StatsIncreased.{name}<T>: token=0x{m.MetadataToken.ToUInt32():X8}, IL={m.Body.Instructions.Count}, bytes={m.Body.CodeSize}, EH={m.Body.ExceptionHandlers.Count}");
    }
}

Console.WriteLine("HF14 StatsIncreased.Start_stats<T> @0x1804ACA50 and End_stats<T> @0x1804AC8E0 restored from PC native x86-64");
Console.WriteLine($"WROTE {output} ({new FileInfo(output).Length} bytes)");
return 0;

void PatchStats(MethodDefinition m)
{
    var hostT = m.GenericParameters.Single();
    var body = new MethodBody(m) { InitLocals = true, MaxStackSize = 3 };
    m.Body = body;
    var il = body.GetILProcessor();
    var z = new VariableDefinition(zombie);
    var p = new VariableDefinition(plant);
    var zombieValueName = new VariableDefinition(module.TypeSystem.String);
    body.Variables.Add(z);
    body.Variables.Add(p);
    body.Variables.Add(zombieValueName);

    var checkZombieMs = Instruction.Create(OpCodes.Nop);
    var afterZombie = Instruction.Create(OpCodes.Nop);
    var ret = Instruction.Create(OpCodes.Ret);

    // PC shared generic body explicitly tests the reference host for null before either type gate.
    E(il, OpCodes.Ldarg_1);
    E(il, OpCodes.Box, hostT);
    E(il, OpCodes.Brfalse, ret);

    // Zombie gate. Native reads valueName once, caches it, and reuses that value for As then Ms.
    E(il, OpCodes.Ldarg_1);
    E(il, OpCodes.Box, hostT);
    E(il, OpCodes.Isinst, zombie);
    E(il, OpCodes.Stloc, z);
    E(il, OpCodes.Ldloc, z);
    E(il, OpCodes.Brfalse, afterZombie);
    E(il, OpCodes.Ldarg_0);
    E(il, OpCodes.Ldfld, valueName);
    E(il, OpCodes.Stloc, zombieValueName);
    E(il, OpCodes.Ldloc, zombieValueName);
    E(il, OpCodes.Ldstr, "As");
    E(il, OpCodes.Call, stringEq);
    E(il, OpCodes.Brfalse, checkZombieMs);
    E(il, OpCodes.Ldloc, z);
    E(il, OpCodes.Call, resetZombieAttack);
    E(il, OpCodes.Br, afterZombie);

    il.Append(checkZombieMs);
    E(il, OpCodes.Ldloc, zombieValueName);
    E(il, OpCodes.Ldstr, "Ms");
    E(il, OpCodes.Call, stringEq);
    E(il, OpCodes.Brfalse, afterZombie);
    E(il, OpCodes.Ldloc, z);
    E(il, OpCodes.Call, resetZombieMove);

    // Plant gate is independent and performs a fresh native read of valueName.
    il.Append(afterZombie);
    E(il, OpCodes.Ldarg_1);
    E(il, OpCodes.Box, hostT);
    E(il, OpCodes.Isinst, plant);
    E(il, OpCodes.Stloc, p);
    E(il, OpCodes.Ldloc, p);
    E(il, OpCodes.Brfalse, ret);
    E(il, OpCodes.Ldarg_0);
    E(il, OpCodes.Ldfld, valueName);
    E(il, OpCodes.Ldstr, "As");
    E(il, OpCodes.Call, stringEq);
    E(il, OpCodes.Brfalse, ret);
    E(il, OpCodes.Ldloc, p);
    E(il, OpCodes.Call, resetPlantAttack);
    il.Append(ret);
}
