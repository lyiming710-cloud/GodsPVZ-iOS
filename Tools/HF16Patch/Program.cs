using Mono.Cecil;
using Mono.Cecil.Cil;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

if (args.Length != 2)
{
    Console.Error.WriteLine("Usage: GodsPVZ.HF16Patch <input.dll> <output.dll>");
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
MethodDefinition M(TypeDefinition t, string n, int pc) => t.Methods.Single(m => m.Name == n && m.Parameters.Count == pc);
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

var attackRange = T("AttackRange");
var rangeType = T("RangeType");
var device = T("Device");
var plant = T("Plant");
var projectile = T("Projectile");
var zombie = T("Zombie");

var rangeTypeField = F(attackRange, "rangeType");
if (rangeTypeField.FieldType.FullName != "RangeType") throw new InvalidDataException($"HF16 rangeType field drift: {rangeTypeField.FieldType.FullName}");

FieldDefinition FX(TypeDefinition t) => F(t, "fX");
FieldDefinition FY(TypeDefinition t) => F(t, "fY");
FieldDefinition FW(TypeDefinition t) => F(t, "fW");
FieldDefinition FD(TypeDefinition t) => F(t, "fD");
foreach (var t in new[] { device, plant, projectile, zombie })
foreach (var f in new[] { FX(t), FY(t), FW(t), FD(t) })
    if (f.FieldType.FullName != "System.Single") throw new InvalidDataException($"HF16 expected float field {f.FullName}");

var testRect = M(attackRange, "TestInRects_Rect", 1);
var testCircle = M(attackRange, "TestInCircles_Position", 1);
var rectType = testRect.Parameters[0].ParameterType;
var vector3Type = testCircle.Parameters[0].ParameterType;
if (rectType.FullName != "UnityEngine.Rect") throw new InvalidDataException($"HF16 Rect signature drift: {rectType.FullName}");
if (vector3Type.FullName != "UnityEngine.Vector3") throw new InvalidDataException($"HF16 Vector3 signature drift: {vector3Type.FullName}");

var debugLog = Ref("UnityEngine.Debug", "Log", "System.Object");
var rectCtor = new MethodReference(".ctor", module.TypeSystem.Void, rectType) { HasThis = true };
for (var i = 0; i < 4; i++) rectCtor.Parameters.Add(new ParameterDefinition(module.TypeSystem.Single));
var vectorCtor = new MethodReference(".ctor", module.TypeSystem.Void, vector3Type) { HasThis = true };
for (var i = 0; i < 3; i++) vectorCtor.Parameters.Add(new ParameterDefinition(module.TypeSystem.Single));

var method = attackRange.Methods.Single(m => m.Name == "TestInRange" && m.Parameters.Count == 1 && m.GenericParameters.Count == 1);
if (method.MetadataToken.ToUInt32() != 0x060000D8) throw new InvalidDataException($"HF16 TestInRange token drift: 0x{method.MetadataToken.ToUInt32():X8}");

Patch(method);
Directory.CreateDirectory(Path.GetDirectoryName(output)!);
module.Write(output, new WriterParameters { WriteSymbols = false });

using (var verify = ModuleDefinition.ReadModule(output, new ReaderParameters { InMemory = true, ReadSymbols = false }))
{
    var all = All(verify.Types).ToList();
    var ar = all.Single(t => t.Name == "AttackRange");
    var m = ar.Methods.Single(x => x.Name == "TestInRange" && x.Parameters.Count == 1 && x.GenericParameters.Count == 1);
    if (m.MetadataToken.ToUInt32() != 0x060000D8) throw new InvalidDataException("HF16 TestInRange token drift after reopen");
    if (!m.HasBody || m.Body.Instructions.Count < 150) throw new InvalidDataException($"HF16 TestInRange body too small: {m.Body.Instructions.Count}");
    if (m.Body.ExceptionHandlers.Count != 0) throw new InvalidDataException("HF16 TestInRange must not contain exception handlers");
    var refs = m.Body.Instructions.Select(i => i.Operand).OfType<MethodReference>().ToList();
    if (refs.Any(r => r.DeclaringType.FullName.Contains("Cpp2ILHelpers", StringComparison.Ordinal))) throw new InvalidDataException("HF16 still contains Cpp2IL helper calls");
    if (refs.Count(r => r.DeclaringType.Name == "AttackRange" && r.Name == "TestInRects_Rect") != 2) throw new InvalidDataException("HF16 Rect helper count mismatch");
    if (refs.Count(r => r.DeclaringType.Name == "AttackRange" && r.Name == "TestInCircles_Position") != 2) throw new InvalidDataException("HF16 Circle helper count mismatch");
    if (refs.Count(r => r.DeclaringType.FullName == "UnityEngine.Debug" && r.Name == "Log" && r.Parameters.Count == 1) != 1) throw new InvalidDataException("HF16 Debug.Log count mismatch");
    if (m.Body.Instructions.Count(i => i.OpCode == OpCodes.Ldstr && Equals(i.Operand, "检测植物是否在范围内")) != 1) throw new InvalidDataException("HF16 exact Plant debug literal missing");
    if (m.Body.Instructions.Count(i => i.OpCode == OpCodes.Isinst && i.Operand is TypeReference tr && new[] { "Device", "Plant", "Projectile", "Zombie" }.Contains(tr.Name)) != 4) throw new InvalidDataException("HF16 must retain four independent host gates");
    int Ld(string decl, string name) => m.Body.Instructions.Count(i => i.OpCode == OpCodes.Ldfld && i.Operand is FieldReference f && f.DeclaringType.Name == decl && f.Name == name);
    if (Ld("AttackRange", "rangeType") != 1) throw new InvalidDataException("HF16 must read rangeType exactly once");
    foreach (var tn in new[] { "Device", "Plant", "Projectile", "Zombie" })
    {
        foreach (var fn in new[] { "fX", "fY", "fW", "fD" })
            if (Ld(tn, fn) != 2) throw new InvalidDataException($"HF16 must read {tn}.{fn} exactly twice, got {Ld(tn, fn)}");
        foreach (var fn in new[] { "fZ", "fH" })
            if (Ld(tn, fn) != 0) throw new InvalidDataException($"HF16 native body must not read {tn}.{fn}");
    }
    if (m.Body.Instructions.Count(i => i.OpCode == OpCodes.Mul) != 8 || m.Body.Instructions.Count(i => i.OpCode == OpCodes.Sub) != 8)
        throw new InvalidDataException("HF16 geometry arithmetic shape mismatch");
    Console.WriteLine($"VERIFY AttackRange.TestInRange<T>: token=0x{m.MetadataToken.ToUInt32():X8}, IL={m.Body.Instructions.Count}, bytes={m.Body.CodeSize}, EH={m.Body.ExceptionHandlers.Count}");
}

Console.WriteLine("HF16 AttackRange.TestInRange<T> @0x180426DD0-0x180427102 restored from PC native x86-64");
Console.WriteLine($"WROTE {output} ({new FileInfo(output).Length} bytes)");
return 0;

void Patch(MethodDefinition m)
{
    var hostT = m.GenericParameters.Single();
    var body = new MethodBody(m) { InitLocals = true, MaxStackSize = 6 };
    m.Body = body;
    var il = body.GetILProcessor();

    var d = new VariableDefinition(device);
    var p = new VariableDefinition(plant);
    var pr = new VariableDefinition(projectile);
    var z = new VariableDefinition(zombie);
    var rect = new VariableDefinition(rectType);
    var pos = new VariableDefinition(vector3Type);
    var half = new VariableDefinition(module.TypeSystem.Single);
    var a = new VariableDefinition(module.TypeSystem.Single);
    var b = new VariableDefinition(module.TypeSystem.Single);
    var c = new VariableDefinition(module.TypeSystem.Single);
    var e = new VariableDefinition(module.TypeSystem.Single);
    var xPos = new VariableDefinition(module.TypeSystem.Single);
    var yPos = new VariableDefinition(module.TypeSystem.Single);
    var range = new VariableDefinition(rangeType);
    foreach (var v in new[] { d, p, pr, z, rect, pos, half, a, b, c, e, xPos, yPos, range }) body.Variables.Add(v);

    var afterDevice = Instruction.Create(OpCodes.Nop);
    var afterPlant = Instruction.Create(OpCodes.Nop);
    var afterProjectile = Instruction.Create(OpCodes.Nop);
    var afterZombie = Instruction.Create(OpCodes.Nop);
    var falseRet = Instruction.Create(OpCodes.Ldc_I4_0);
    var trueRet = Instruction.Create(OpCodes.Ldc_I4_1);
    var rectCase = Instruction.Create(OpCodes.Nop);
    var circleCase = Instruction.Create(OpCodes.Nop);
    var mixedCase = Instruction.Create(OpCodes.Nop);
    var mixedCircle = Instruction.Create(OpCodes.Nop);

    E(il, OpCodes.Ldarg_1);
    E(il, OpCodes.Box, hostT);
    E(il, OpCodes.Brfalse, falseRet);
    E(il, OpCodes.Ldloca, rect);
    E(il, OpCodes.Initobj, rectType);
    E(il, OpCodes.Ldloca, pos);
    E(il, OpCodes.Initobj, vector3Type);
    E(il, OpCodes.Ldc_R4, 0.5f);
    E(il, OpCodes.Stloc, half);

    EmitHost(device, d, afterDevice, logPlant: false);
    il.Append(afterDevice);
    EmitHost(plant, p, afterPlant, logPlant: true);
    il.Append(afterPlant);
    EmitHost(projectile, pr, afterProjectile, logPlant: false);
    il.Append(afterProjectile);
    EmitHost(zombie, z, afterZombie, logPlant: false);
    il.Append(afterZombie);

    E(il, OpCodes.Ldarg_0);
    E(il, OpCodes.Ldfld, rangeTypeField);
    E(il, OpCodes.Stloc, range);
    E(il, OpCodes.Ldloc, range);
    il.Append(Instruction.Create(OpCodes.Switch, new[] { falseRet, rectCase, circleCase, mixedCase, trueRet }));
    E(il, OpCodes.Br, falseRet);

    il.Append(rectCase);
    E(il, OpCodes.Ldarg_0);
    E(il, OpCodes.Ldloc, rect);
    E(il, OpCodes.Call, testRect);
    E(il, OpCodes.Ret);

    il.Append(circleCase);
    E(il, OpCodes.Ldarg_0);
    E(il, OpCodes.Ldloc, pos);
    E(il, OpCodes.Call, testCircle);
    E(il, OpCodes.Ret);

    il.Append(mixedCase);
    E(il, OpCodes.Ldarg_0);
    E(il, OpCodes.Ldloc, rect);
    E(il, OpCodes.Call, testRect);
    E(il, OpCodes.Brfalse, mixedCircle);
    E(il, OpCodes.Br, trueRet);
    il.Append(mixedCircle);
    E(il, OpCodes.Ldarg_0);
    E(il, OpCodes.Ldloc, pos);
    E(il, OpCodes.Call, testCircle);
    E(il, OpCodes.Ret);

    il.Append(falseRet);
    E(il, OpCodes.Ret);
    il.Append(trueRet);
    E(il, OpCodes.Ret);

    void EmitHost(TypeDefinition hostType, VariableDefinition host, Instruction next, bool logPlant)
    {
        E(il, OpCodes.Ldarg_1);
        E(il, OpCodes.Box, hostT);
        E(il, OpCodes.Isinst, hostType);
        E(il, OpCodes.Stloc, host);
        E(il, OpCodes.Ldloc, host);
        E(il, OpCodes.Brfalse, next);
        if (logPlant)
        {
            E(il, OpCodes.Ldstr, "检测植物是否在范围内");
            E(il, OpCodes.Call, debugLog);
        }

        E(il, OpCodes.Ldloc, host); E(il, OpCodes.Ldfld, FW(hostType)); E(il, OpCodes.Stloc, a);
        E(il, OpCodes.Ldloc, host); E(il, OpCodes.Ldfld, FD(hostType)); E(il, OpCodes.Stloc, b);
        E(il, OpCodes.Ldloc, host); E(il, OpCodes.Ldfld, FX(hostType)); E(il, OpCodes.Stloc, c);
        E(il, OpCodes.Ldloc, host); E(il, OpCodes.Ldfld, FW(hostType)); E(il, OpCodes.Stloc, e);
        E(il, OpCodes.Ldloc, host); E(il, OpCodes.Ldfld, FX(hostType)); E(il, OpCodes.Stloc, xPos);
        E(il, OpCodes.Ldloc, host); E(il, OpCodes.Ldfld, FY(hostType)); E(il, OpCodes.Stloc, yPos);

        E(il, OpCodes.Ldloc, c); E(il, OpCodes.Ldloc, a); E(il, OpCodes.Ldloc, half); E(il, OpCodes.Mul); E(il, OpCodes.Sub); E(il, OpCodes.Stloc, c);
        E(il, OpCodes.Ldloc, host); E(il, OpCodes.Ldfld, FY(hostType)); E(il, OpCodes.Ldloc, b); E(il, OpCodes.Ldloc, half); E(il, OpCodes.Mul); E(il, OpCodes.Sub); E(il, OpCodes.Stloc, a);
        E(il, OpCodes.Ldloc, host); E(il, OpCodes.Ldfld, FD(hostType)); E(il, OpCodes.Stloc, b);

        E(il, OpCodes.Ldloc, c);
        E(il, OpCodes.Ldloc, a);
        E(il, OpCodes.Ldloc, e);
        E(il, OpCodes.Ldloc, b);
        E(il, OpCodes.Newobj, rectCtor);
        E(il, OpCodes.Stloc, rect);

        E(il, OpCodes.Ldloc, xPos);
        E(il, OpCodes.Ldloc, yPos);
        E(il, OpCodes.Ldc_R4, 0f);
        E(il, OpCodes.Newobj, vectorCtor);
        E(il, OpCodes.Stloc, pos);
    }
}
