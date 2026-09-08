using Mono.Cecil;
using Mono.Cecil.Cil;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

if (args.Length != 2)
{
    Console.Error.WriteLine("Usage: GodsPVZ.HF19Patch <input.dll> <output.dll>");
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
        VariableDefinition vd => Instruction.Create(op, vd),
        Instruction target => Instruction.Create(op, target),
        _ => throw new NotSupportedException(value.GetType().FullName)
    };
    il.Append(ins);
}

var zombie = T("Zombie");
var fId = F(zombie, "ID");
var fDied = F(zombie, "isDied");
var fAshes = F(zombie, "ashes");
var fDying = F(zombie, "isDying");
if (fId.FieldType.FullName != "System.Int32" || fDied.FieldType.FullName != "System.Boolean" || fAshes.FieldType.FullName != "System.Boolean" || fDying.FieldType.FullName != "System.Boolean")
    throw new InvalidDataException("HF19 Zombie field type drift");

var isDisabled = M(zombie, "IsDisabled", 0);
var isNormal = M(zombie, "IsNormalZombie", 0);
var isPlant = M(zombie, "IsPlantZombie", 0);
if (isDisabled.MetadataToken.ToUInt32() != 0x06000464 || isNormal.MetadataToken.ToUInt32() != 0x06000466 || isPlant.MetadataToken.ToUInt32() != 0x06000467)
    throw new InvalidDataException("HF19 MethodDef token drift");

PatchSetPredicate(isNormal, new[] { 0, 2, 4, 5, 14, 15 });
PatchSetPredicate(isPlant, new[] { 6, 7, 8, 9, 11, 12, 19, 20, 21, 22 });
PatchIsDisabled();

Directory.CreateDirectory(Path.GetDirectoryName(output)!);
module.Write(output, new WriterParameters { WriteSymbols = false });

using (var verify = ModuleDefinition.ReadModule(output, new ReaderParameters { InMemory = true, ReadSymbols = false }))
{
    var z = All(verify.Types).Single(t => t.Name == "Zombie");
    var targets = new[]
    {
        ("IsDisabled", z.Methods.Single(m => m.Name == "IsDisabled" && m.Parameters.Count == 0), 50),
        ("IsNormalZombie", z.Methods.Single(m => m.Name == "IsNormalZombie" && m.Parameters.Count == 0), 25),
        ("IsPlantZombie", z.Methods.Single(m => m.Name == "IsPlantZombie" && m.Parameters.Count == 0), 35),
    };
    foreach (var (name,m,min) in targets)
    {
        if (!m.HasBody || m.Body.Instructions.Count < min) throw new InvalidDataException($"HF19 {name} too small: {m.Body.Instructions.Count}");
        if (m.Body.ExceptionHandlers.Count != 0) throw new InvalidDataException($"HF19 {name} unexpected EH");
        if (m.Body.Instructions.Select(i => i.Operand).OfType<MethodReference>().Any(r => r.DeclaringType.FullName.Contains("Cpp2ILHelpers", StringComparison.Ordinal)))
            throw new InvalidDataException($"HF19 {name} still contains Cpp2IL helper calls");
        Console.WriteLine($"VERIFY Zombie.{name}: token=0x{m.MetadataToken.ToUInt32():X8}, IL={m.Body.Instructions.Count}, bytes={m.Body.CodeSize}");
    }
    var d = targets[0].Item2;
    if (d.Body.Instructions.Select(i => i.Operand).OfType<MethodReference>().Count(r => r.Name == "IsPlantZombie" && r.DeclaringType.Name == "Zombie") != 1)
        throw new InvalidDataException("HF19 IsDisabled must call IsPlantZombie exactly once");
}

Console.WriteLine("HF19 Zombie IsDisabled/IsNormalZombie/IsPlantZombie restored from PC native x86-64 jump tables");
Console.WriteLine($"WROTE {output} ({new FileInfo(output).Length} bytes)");
return 0;

void PatchSetPredicate(MethodDefinition m, int[] ids)
{
    var body = new MethodBody(m) { InitLocals = true, MaxStackSize = 2 }; m.Body = body;
    var il = body.GetILProcessor();
    var id = new VariableDefinition(module.TypeSystem.Int32); body.Variables.Add(id);
    var yes = Instruction.Create(OpCodes.Ldc_I4_1);
    E(il, OpCodes.Ldarg_0); E(il, OpCodes.Ldfld, fId); E(il, OpCodes.Stloc, id);
    foreach (var v in ids)
    {
        E(il, OpCodes.Ldloc, id); E(il, OpCodes.Ldc_I4, v); E(il, OpCodes.Beq, yes);
    }
    E(il, OpCodes.Ldc_I4_0); E(il, OpCodes.Ret);
    il.Append(yes); E(il, OpCodes.Ret);
}

void PatchIsDisabled()
{
    var body = new MethodBody(isDisabled) { InitLocals = true, MaxStackSize = 2 }; isDisabled.Body = body;
    var il = body.GetILProcessor();
    var id = new VariableDefinition(module.TypeSystem.Int32); body.Variables.Add(id);
    var retTrue = Instruction.Create(OpCodes.Ldc_I4_1);
    var retDying = Instruction.Create(OpCodes.Ldarg_0);

    E(il, OpCodes.Ldarg_0); E(il, OpCodes.Ldfld, fDied); E(il, OpCodes.Brtrue, retTrue);
    E(il, OpCodes.Ldarg_0); E(il, OpCodes.Ldfld, fAshes); E(il, OpCodes.Brtrue, retTrue);
    E(il, OpCodes.Ldarg_0); E(il, OpCodes.Ldfld, fId); E(il, OpCodes.Stloc, id);

    foreach (var v in new[] { 0, 2, 4, 5, 14, 15 })
    {
        E(il, OpCodes.Ldloc, id); E(il, OpCodes.Ldc_I4, v); E(il, OpCodes.Beq, retDying);
    }

    E(il, OpCodes.Ldarg_0); E(il, OpCodes.Call, isPlant); E(il, OpCodes.Brtrue, retDying);

    foreach (var v in new[] { 1, 10, 3, 23, 16 })
    {
        E(il, OpCodes.Ldloc, id); E(il, OpCodes.Ldc_I4, v); E(il, OpCodes.Beq, retDying);
    }

    E(il, OpCodes.Ldc_I4_0); E(il, OpCodes.Ret);
    il.Append(retDying); E(il, OpCodes.Ldfld, fDying); E(il, OpCodes.Ret);
    il.Append(retTrue); E(il, OpCodes.Ret);
}
