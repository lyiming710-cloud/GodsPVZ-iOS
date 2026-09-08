using Mono.Cecil;
using Mono.Cecil.Cil;
using System;
using System.IO;
using System.Linq;

if (args.Length != 2)
{
    Console.Error.WriteLine("Usage: GodsPVZ.HF4Patch <input.dll> <output.dll>");
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

var zombie = All(module.Types).Single(t => t.Name == "Zombie");
FieldDefinition F(string n) => zombie.Fields.Single(f => f.Name == n);
var awake = zombie.Methods.Single(m => m.Name == "Awake" && m.Parameters.Count == 0);
if (awake.MetadataToken.ToUInt32() != 0x0600041B)
    throw new InvalidDataException($"Unexpected Zombie.Awake token 0x{awake.MetadataToken.ToUInt32():X8}");

var body = new MethodBody(awake) { InitLocals = true, MaxStackSize = 8 };
awake.Body = body;
var il = body.GetILProcessor();
var tmp = new VariableDefinition(module.TypeSystem.Single);
body.Variables.Add(tmp);

var armor1Type = F("armor1Type");
var armor1Defense = F("armor1Defense");
var armor1Point = F("armor1Point");
var maxArmor1Point = F("maxArmor1Point");
var armor1Toughness = F("armor1Toughness");
var armor2Type = F("armor2Type");
var armor2Defense = F("armor2Defense");
var armor2Point = F("armor2Point");
var maxArmor2Point = F("maxArmor2Point");
var armor2Toughness = F("armor2Toughness");

// PC x86-64 0x18035DAD0. Native order is:
// armor1 defense -> armor1 max/current HP -> armor1 toughness ->
// armor2 defense -> armor2 max/current HP -> armor2 toughness.
EmitIntMap(armor1Type, armor1Defense, new[] { 0, 60, 120, 40, 200, 320, 150 });
EmitFloatMapTwoTargets(armor1Type, maxArmor1Point, armor1Point, new[] { 0f, 370f, 1100f, 2580f, 1680f, 1680f, 1980f });
EmitIntMap(armor1Type, armor1Toughness, new[] { 0, 20, 35, 15, 15, 35, 15 });
EmitIntMap(armor2Type, armor2Defense, new[] { 0, 110, 20, 400, 120 });
EmitFloatMapTwoTargets(armor2Type, maxArmor2Point, armor2Point, new[] { 0f, 1100f, 240f, 2200f, 1440f });
EmitIntMap(armor2Type, armor2Toughness, new[] { 0, 30, 5, 40, 30 });
il.Append(Instruction.Create(OpCodes.Ret));

Directory.CreateDirectory(Path.GetDirectoryName(output)!);
module.Write(output, new WriterParameters { WriteSymbols = false });

using (var verify = ModuleDefinition.ReadModule(output, new ReaderParameters { InMemory = true, ReadSymbols = false }))
{
    var z = All(verify.Types).Single(t => t.Name == "Zombie");
    var m = z.Methods.Single(x => x.Name == "Awake" && x.Parameters.Count == 0);
    if (!m.HasBody || m.Body.Instructions.Count < 100)
        throw new InvalidDataException($"HF4 Zombie.Awake body too small: {m.Body.Instructions.Count}");
    if (m.Body.Instructions.Count(i => i.OpCode == OpCodes.Switch) != 6)
        throw new InvalidDataException("HF4 Zombie.Awake must contain exactly six enum switch tables");
    if (m.Body.Instructions.Any(i => i.OpCode == OpCodes.Call || i.OpCode == OpCodes.Callvirt || i.OpCode == OpCodes.Newobj))
        throw new InvalidDataException("HF4 Zombie.Awake unexpectedly contains managed calls");

    var written = m.Body.Instructions.Where(i => i.OpCode == OpCodes.Stfld)
        .Select(i => ((FieldReference)i.Operand).Name).ToHashSet();
    foreach (var name in new[] {
        "armor1Defense", "armor1Point", "maxArmor1Point", "armor1Toughness",
        "armor2Defense", "armor2Point", "maxArmor2Point", "armor2Toughness"
    })
        if (!written.Contains(name)) throw new InvalidDataException($"HF4 missing write to {name}");

    var floats = m.Body.Instructions.Where(i => i.OpCode == OpCodes.Ldc_R4).Select(i => (float)i.Operand).ToHashSet();
    foreach (var value in new[] { 370f, 1100f, 2580f, 1680f, 1980f, 240f, 2200f, 1440f })
        if (!floats.Contains(value)) throw new InvalidDataException($"HF4 missing native HP constant {value}");

    Console.WriteLine($"VERIFY Zombie.Awake token=0x{m.MetadataToken.ToUInt32():X8}: {m.Body.Instructions.Count} IL, {m.Body.CodeSize} bytes, six native enum tables");
}

Console.WriteLine("HF4 Zombie.Awake @0x18035DAD0 restored from PC native x86-64");
Console.WriteLine($"WROTE {output} ({new FileInfo(output).Length} bytes)");
return 0;

void EmitIntMap(FieldDefinition selector, FieldDefinition target, int[] values)
{
    var labels = values.Select(_ => Instruction.Create(OpCodes.Nop)).ToArray();
    var done = Instruction.Create(OpCodes.Nop);

    il.Append(Instruction.Create(OpCodes.Ldarg_0));
    il.Append(Instruction.Create(OpCodes.Ldfld, selector));
    il.Append(Instruction.Create(OpCodes.Switch, labels));
    il.Append(Instruction.Create(OpCodes.Ldc_I4_0));
    il.Append(Instruction.Create(OpCodes.Conv_R4));
    il.Append(Instruction.Create(OpCodes.Stloc, tmp));
    il.Append(Instruction.Create(OpCodes.Br, done));

    for (var i = 0; i < values.Length; i++)
    {
        il.Append(labels[i]);
        il.Append(Instruction.Create(OpCodes.Ldc_I4, values[i]));
        il.Append(Instruction.Create(OpCodes.Conv_R4));
        il.Append(Instruction.Create(OpCodes.Stloc, tmp));
        il.Append(Instruction.Create(OpCodes.Br, done));
    }

    il.Append(done);
    il.Append(Instruction.Create(OpCodes.Ldarg_0));
    il.Append(Instruction.Create(OpCodes.Ldloc, tmp));
    il.Append(Instruction.Create(OpCodes.Stfld, target));
}

void EmitFloatMapTwoTargets(FieldDefinition selector, FieldDefinition firstTarget, FieldDefinition secondTarget, float[] values)
{
    var labels = values.Select(_ => Instruction.Create(OpCodes.Nop)).ToArray();
    var done = Instruction.Create(OpCodes.Nop);

    il.Append(Instruction.Create(OpCodes.Ldarg_0));
    il.Append(Instruction.Create(OpCodes.Ldfld, selector));
    il.Append(Instruction.Create(OpCodes.Switch, labels));
    il.Append(Instruction.Create(OpCodes.Ldc_R4, 0f));
    il.Append(Instruction.Create(OpCodes.Stloc, tmp));
    il.Append(Instruction.Create(OpCodes.Br, done));

    for (var i = 0; i < values.Length; i++)
    {
        il.Append(labels[i]);
        il.Append(Instruction.Create(OpCodes.Ldc_R4, values[i]));
        il.Append(Instruction.Create(OpCodes.Stloc, tmp));
        il.Append(Instruction.Create(OpCodes.Br, done));
    }

    il.Append(done);
    il.Append(Instruction.Create(OpCodes.Ldarg_0));
    il.Append(Instruction.Create(OpCodes.Ldloc, tmp));
    il.Append(Instruction.Create(OpCodes.Stfld, firstTarget));
    il.Append(Instruction.Create(OpCodes.Ldarg_0));
    il.Append(Instruction.Create(OpCodes.Ldloc, tmp));
    il.Append(Instruction.Create(OpCodes.Stfld, secondTarget));
}
