using Mono.Cecil;
using Mono.Cecil.Cil;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

if (args.Length != 2)
{
    Console.Error.WriteLine("Usage: GodsPVZ.HF15Patch <input.dll> <output.dll>");
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

var bleed = T("Bleed");
var zombie = T("Zombie");
var plant = T("Plant");
var device = T("Device");

var fValue = F(bleed, "value");
var fMinHealthPoint = F(bleed, "minHealthPoint");
var fMulti = F(bleed, "multi");
var zHealth = F(zombie, "healthPoint");
var zMaxHealth = F(zombie, "maxHealthPoint");
var pHealth = F(plant, "healthPoint");
var pMaxHealth = F(plant, "maxHealthPoint");
var dHealth = F(device, "healthPoint");
var dMaxHealth = F(device, "maxHealthPoint");

if (fValue.FieldType.FullName != "System.Single" || fMinHealthPoint.FieldType.FullName != "System.Single" || fMulti.FieldType.FullName != "System.Boolean")
    throw new InvalidDataException("HF15 Bleed field type drift");
foreach (var f in new[] { zHealth, zMaxHealth, pHealth, pMaxHealth, dHealth, dMaxHealth })
    if (f.FieldType.FullName != "System.Single") throw new InvalidDataException($"HF15 expected float field {f.FullName}");

var getDeltaTime = Ref("UnityEngine.Time", "get_deltaTime");
var method = bleed.Methods.Single(m => m.Name == "Bleeding" && m.Parameters.Count == 1 && m.GenericParameters.Count == 1);
if (method.MetadataToken.ToUInt32() != 0x060000FB) throw new InvalidDataException($"HF15 Bleeding token drift: 0x{method.MetadataToken.ToUInt32():X8}");

PatchBleeding(method);

Directory.CreateDirectory(Path.GetDirectoryName(output)!);
module.Write(output, new WriterParameters { WriteSymbols = false });

using (var verify = ModuleDefinition.ReadModule(output, new ReaderParameters { InMemory = true, ReadSymbols = false }))
{
    var all = All(verify.Types).ToList();
    var b = all.Single(t => t.Name == "Bleed");
    var m = b.Methods.Single(x => x.Name == "Bleeding" && x.Parameters.Count == 1 && x.GenericParameters.Count == 1);
    if (m.MetadataToken.ToUInt32() != 0x060000FB) throw new InvalidDataException("HF15 Bleeding token drift after reopen");
    if (!m.HasBody || m.Body.Instructions.Count < 75) throw new InvalidDataException($"HF15 Bleeding body too small: {m.Body.Instructions.Count}");
    if (m.Body.ExceptionHandlers.Count != 0) throw new InvalidDataException("HF15 Bleeding must not contain exception handlers");

    var refs = m.Body.Instructions.Select(i => i.Operand).OfType<MethodReference>().ToList();
    if (refs.Any(r => r.DeclaringType.FullName.Contains("Cpp2ILHelpers", StringComparison.Ordinal)))
        throw new InvalidDataException("HF15 Bleeding still contains Cpp2IL helper calls");
    if (refs.Count(r => r.DeclaringType.FullName == "UnityEngine.Time" && r.Name == "get_deltaTime" && r.Parameters.Count == 0) != 3)
        throw new InvalidDataException("HF15 must retain three independent Time.deltaTime calls");

    int Ld(string decl, string name) => m.Body.Instructions.Count(i => i.OpCode == OpCodes.Ldfld && i.Operand is FieldReference f && f.DeclaringType.Name == decl && f.Name == name);
    int St(string decl, string name) => m.Body.Instructions.Count(i => i.OpCode == OpCodes.Stfld && i.Operand is FieldReference f && f.DeclaringType.Name == decl && f.Name == name);
    if (Ld("Bleed", "multi") != 3) throw new InvalidDataException("HF15 must read Bleed.multi exactly three times");
    if (Ld("Bleed", "value") != 6) throw new InvalidDataException("HF15 must contain six native-path Bleed.value read sites");
    if (Ld("Bleed", "minHealthPoint") != 0) throw new InvalidDataException("HF15 native body must not read Bleed.minHealthPoint");
    foreach (var type in new[] { "Zombie", "Plant", "Device" })
    {
        if (Ld(type, "maxHealthPoint") != 1 || Ld(type, "healthPoint") != 1 || St(type, "healthPoint") != 1)
            throw new InvalidDataException($"HF15 {type} health field access count mismatch");
    }
    if (m.Body.Instructions.Count(i => i.OpCode == OpCodes.Isinst && i.Operand is TypeReference t && new[] { "Zombie", "Plant", "Device" }.Contains(t.Name)) != 3)
        throw new InvalidDataException("HF15 must retain three independent host type gates");
    if (m.Body.Instructions.Count(i => i.OpCode == OpCodes.Mul) != 6 || m.Body.Instructions.Count(i => i.OpCode == OpCodes.Sub) != 3)
        throw new InvalidDataException("HF15 arithmetic shape mismatch");

    Console.WriteLine($"VERIFY Bleed.Bleeding<T>: token=0x{m.MetadataToken.ToUInt32():X8}, IL={m.Body.Instructions.Count}, bytes={m.Body.CodeSize}, EH={m.Body.ExceptionHandlers.Count}");
}

Console.WriteLine("HF15 Bleed.Bleeding<T> @0x180428C90-0x180428E64 restored from PC native x86-64");
Console.WriteLine($"WROTE {output} ({new FileInfo(output).Length} bytes)");
return 0;

void PatchBleeding(MethodDefinition m)
{
    var hostT = m.GenericParameters.Single();
    var body = new MethodBody(m) { InitLocals = true, MaxStackSize = 4 };
    m.Body = body;
    var il = body.GetILProcessor();

    var z = new VariableDefinition(zombie);
    var p = new VariableDefinition(plant);
    var d = new VariableDefinition(device);
    var amount = new VariableDefinition(module.TypeSystem.Single);
    body.Variables.Add(z);
    body.Variables.Add(p);
    body.Variables.Add(d);
    body.Variables.Add(amount);

    var plantGate = Instruction.Create(OpCodes.Nop);
    var deviceGate = Instruction.Create(OpCodes.Nop);
    var ret = Instruction.Create(OpCodes.Ret);

    // PC shared generic body first tests the reference host for null.
    E(il, OpCodes.Ldarg_1);
    E(il, OpCodes.Box, hostT);
    E(il, OpCodes.Brfalse, ret);

    EmitHost(zombie, z, zHealth, zMaxHealth, plantGate);
    EmitHost(plant, p, pHealth, pMaxHealth, deviceGate);
    EmitHost(device, d, dHealth, dMaxHealth, ret);

    void EmitHost(TypeDefinition hostType, VariableDefinition hostLocal, FieldDefinition health, FieldDefinition maxHealth, Instruction nextGate)
    {
        var multiCase = Instruction.Create(OpCodes.Nop);
        var haveAmount = Instruction.Create(OpCodes.Nop);

        E(il, OpCodes.Ldarg_1);
        E(il, OpCodes.Box, hostT);
        E(il, OpCodes.Isinst, hostType);
        E(il, OpCodes.Stloc, hostLocal);
        E(il, OpCodes.Ldloc, hostLocal);
        E(il, OpCodes.Brfalse, nextGate);

        // Native reads multi for every host branch. Non-multi uses value directly;
        // multi uses maxHealthPoint * value. minHealthPoint is not read.
        E(il, OpCodes.Ldarg_0);
        E(il, OpCodes.Ldfld, fMulti);
        E(il, OpCodes.Brtrue, multiCase);
        E(il, OpCodes.Ldarg_0);
        E(il, OpCodes.Ldfld, fValue);
        E(il, OpCodes.Stloc, amount);
        E(il, OpCodes.Br, haveAmount);

        il.Append(multiCase);
        E(il, OpCodes.Ldloc, hostLocal);
        E(il, OpCodes.Ldfld, maxHealth);
        E(il, OpCodes.Ldarg_0);
        E(il, OpCodes.Ldfld, fValue);
        E(il, OpCodes.Mul);
        E(il, OpCodes.Stloc, amount);

        il.Append(haveAmount);
        // Preserve native ordering: healthPoint read, independent Time.deltaTime,
        // then deltaTime * amount and subtraction from the previously read health.
        E(il, OpCodes.Ldloc, hostLocal);
        E(il, OpCodes.Ldloc, hostLocal);
        E(il, OpCodes.Ldfld, health);
        E(il, OpCodes.Call, getDeltaTime);
        E(il, OpCodes.Ldloc, amount);
        E(il, OpCodes.Mul);
        E(il, OpCodes.Sub);
        E(il, OpCodes.Stfld, health);

        il.Append(nextGate);
    }
}
