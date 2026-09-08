using Mono.Cecil;
using Mono.Cecil.Cil;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

if (args.Length != 2)
{
    Console.Error.WriteLine("Usage: GodsPVZ.HF7Patch <input.dll> <output.dll>");
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
        string s => Instruction.Create(op, s),
        int i => Instruction.Create(op, i),
        float f => Instruction.Create(op, f),
        double d => Instruction.Create(op, d),
        MethodReference mr => Instruction.Create(op, mr),
        FieldReference fr => Instruction.Create(op, fr),
        TypeReference tr => Instruction.Create(op, tr),
        VariableDefinition vd => Instruction.Create(op, vd),
        Instruction target => Instruction.Create(op, target),
        _ => throw new NotSupportedException(value.GetType().FullName)
    };
    il.Append(ins);
}

var zombie = T("Zombie");
var enemyPath = T("EnemyPath");
PatchGetMoveDirection();

Directory.CreateDirectory(Path.GetDirectoryName(output)!);
module.Write(output, new WriterParameters { WriteSymbols = false });

using (var verify = ModuleDefinition.ReadModule(output, new ReaderParameters { InMemory = true, ReadSymbols = false }))
{
    var z = All(verify.Types).Single(t => t.Name == "Zombie");
    var m = z.Methods.Single(x => x.Name == "GetMoveDirection" && x.Parameters.Count == 0);
    if (m.MetadataToken.ToUInt32() != 0x0600044F) throw new InvalidDataException("HF7 token drift");
    if (!m.HasBody || m.Body.Instructions.Count < 120) throw new InvalidDataException($"HF7 body too small: {m.Body.Instructions.Count}");
    if (m.Body.ExceptionHandlers.Count != 1 || m.Body.ExceptionHandlers[0].HandlerType != ExceptionHandlerType.Finally)
        throw new InvalidDataException("HF7 must retain List<EnemyPath>.Enumerator finally/Dispose semantics");
    if (m.Body.Instructions.Select(i => i.Operand).OfType<MethodReference>().Any(r => r.DeclaringType.FullName.Contains("Cpp2ILHelpers", StringComparison.Ordinal)))
        throw new InvalidDataException("HF7 still contains Cpp2IL helper calls");
    if (!m.Body.Instructions.Any(i => i.OpCode == OpCodes.Ldc_R4 && i.Operand is float f && f == 1e-5f))
        throw new InvalidDataException("HF7 missing native normalization epsilon");
    if (!m.Body.Instructions.Any(i => i.OpCode == OpCodes.Ldc_R4 && i.Operand is float f && f == -1f))
        throw new InvalidDataException("HF7 missing native final -1 multiplier");
    var refs = m.Body.Instructions.Select(i => i.Operand).OfType<MethodReference>().ToList();
    foreach (var n in new[] { "GetEnumerator", "MoveNext", "get_Current", "Dispose", "Sqrt", "Abs" })
        if (!refs.Any(r => r.Name == n)) throw new InvalidDataException($"HF7 missing {n}");
    Console.WriteLine($"VERIFY Zombie.GetMoveDirection: {m.Body.Instructions.Count} IL, {m.Body.CodeSize} bytes, finally={m.Body.ExceptionHandlers.Count}");
}

Console.WriteLine("HF7 Zombie.GetMoveDirection @0x180361440 restored from PC native x86-64");
Console.WriteLine($"WROTE {output} ({new FileInfo(output).Length} bytes)");
return 0;

void PatchGetMoveDirection()
{
    var m = M(zombie, "GetMoveDirection", 0);
    if (m.MetadataToken.ToUInt32() != 0x0600044F) throw new InvalidDataException("Unexpected Zombie.GetMoveDirection token");

    var fPath = F(zombie, "path");
    var fPrevious = F(zombie, "previousPosition");
    var fDeadzone = F(zombie, "deadzone_distance");
    var fPosition = F(enemyPath, "position");
    var fArrived = F(enemyPath, "arrived");
    var vec3 = fPrevious.FieldType;
    var vecX = new FieldReference("x", module.TypeSystem.Single, vec3);
    var vecY = new FieldReference("y", module.TypeSystem.Single, vec3);
    var vecZ = new FieldReference("z", module.TypeSystem.Single, vec3);

    var listType = (GenericInstanceType)fPath.FieldType;
    var oldMoveNext = m.Body.Instructions.Select(i => i.Operand).OfType<MethodReference>()
        .First(r => r.Name == "MoveNext" && r.DeclaringType.FullName.Contains("System.Collections.Generic.List`1/Enumerator", StringComparison.Ordinal));
    if (oldMoveNext.DeclaringType is not GenericInstanceType oldEnum)
        throw new InvalidDataException("HF7 could not recover List enumerator type shape");
    var enumType = new GenericInstanceType(oldEnum.ElementType);
    enumType.GenericArguments.Add(enemyPath);
    var getEnumerator = new MethodReference("GetEnumerator", enumType, listType) { HasThis = true };
    var moveNext = new MethodReference("MoveNext", module.TypeSystem.Boolean, enumType) { HasThis = true };
    var getCurrent = new MethodReference("get_Current", enemyPath, enumType) { HasThis = true };
    var dispose = new MethodReference("Dispose", module.TypeSystem.Void, enumType) { HasThis = true };
    var getCount = new MethodReference("get_Count", module.TypeSystem.Int32, listType) { HasThis = true };
    var abs = Ref("System.Math", "Abs", "System.Single");
    var sqrt = Ref("System.Math", "Sqrt", "System.Double");
    var nreCtor = Ref("System.NullReferenceException", ".ctor");

    var b = new MethodBody(m) { InitLocals = true, MaxStackSize = 8 };
    m.Body = b;
    var il = b.GetILProcessor();

    var pathLocal = new VariableDefinition(listType);
    var en = new VariableDefinition(enumType);
    var selected = new VariableDefinition(enemyPath);
    var dx = new VariableDefinition(module.TypeSystem.Single);
    var dy = new VariableDefinition(module.TypeSystem.Single);
    var dz = new VariableDefinition(module.TypeSystem.Single);
    var absDx = new VariableDefinition(module.TypeSystem.Single);
    var magSq = new VariableDefinition(module.TypeSystem.Single);
    var mag = new VariableDefinition(module.TypeSystem.Single);
    var result = new VariableDefinition(vec3);
    foreach (var v in new[] { pathLocal, en, selected, dx, dy, dz, absDx, magSq, mag, result }) b.Variables.Add(v);

    var returnZero = Instruction.Create(OpCodes.Nop);
    var tryStart = Instruction.Create(OpCodes.Nop);
    var loopCheck = Instruction.Create(OpCodes.Nop);
    var loopBody = Instruction.Create(OpCodes.Nop);
    var keepScanning = Instruction.Create(OpCodes.Nop);
    var finallyStart = Instruction.Create(OpCodes.Nop);
    var afterFinally = Instruction.Create(OpCodes.Nop);
    var useFullX = Instruction.Create(OpCodes.Nop);
    var vectorReady = Instruction.Create(OpCodes.Nop);
    var normalizeNonZero = Instruction.Create(OpCodes.Nop);
    var multiplyAndReturn = Instruction.Create(OpCodes.Nop);

    // if (path == null || path.Count == 0) return Vector3.zero;
    E(il, OpCodes.Ldarg_0); E(il, OpCodes.Ldfld, fPath); E(il, OpCodes.Stloc, pathLocal);
    E(il, OpCodes.Ldloc, pathLocal); E(il, OpCodes.Brfalse, returnZero);
    E(il, OpCodes.Ldloc, pathLocal); E(il, OpCodes.Callvirt, getCount); E(il, OpCodes.Brfalse, returnZero);
    E(il, OpCodes.Ldnull); E(il, OpCodes.Stloc, selected);
    E(il, OpCodes.Ldloc, pathLocal); E(il, OpCodes.Callvirt, getEnumerator); E(il, OpCodes.Stloc, en);

    // Native foreach: choose the first non-arrived EnemyPath, with real finally/Dispose.
    il.Append(tryStart);
    E(il, OpCodes.Br, loopCheck);
    il.Append(loopBody);
    E(il, OpCodes.Ldloca, en); E(il, OpCodes.Call, getCurrent); E(il, OpCodes.Stloc, selected);
    E(il, OpCodes.Ldloc, selected); E(il, OpCodes.Brtrue, keepScanning);
    E(il, OpCodes.Newobj, nreCtor); E(il, OpCodes.Throw);
    il.Append(keepScanning);
    E(il, OpCodes.Ldloc, selected); E(il, OpCodes.Ldfld, fArrived); E(il, OpCodes.Brtrue, loopCheck);
    E(il, OpCodes.Leave, afterFinally);
    il.Append(loopCheck);
    E(il, OpCodes.Ldloca, en); E(il, OpCodes.Call, moveNext); E(il, OpCodes.Brtrue, loopBody);
    E(il, OpCodes.Ldnull); E(il, OpCodes.Stloc, selected);
    E(il, OpCodes.Leave, afterFinally);

    il.Append(finallyStart);
    E(il, OpCodes.Ldloca, en); E(il, OpCodes.Call, dispose); E(il, OpCodes.Endfinally);
    il.Append(afterFinally);
    b.ExceptionHandlers.Add(new ExceptionHandler(ExceptionHandlerType.Finally)
    {
        TryStart = tryStart,
        TryEnd = finallyStart,
        HandlerStart = finallyStart,
        HandlerEnd = afterFinally
    });

    E(il, OpCodes.Ldloc, selected); E(il, OpCodes.Brfalse, returnZero);

    // Native field-read order: abs(dx), dz, deadzone branch, then dy/full dx.
    E(il, OpCodes.Ldloc, selected); E(il, OpCodes.Ldflda, fPosition); E(il, OpCodes.Ldfld, vecX);
    E(il, OpCodes.Ldarg_0); E(il, OpCodes.Ldflda, fPrevious); E(il, OpCodes.Ldfld, vecX); E(il, OpCodes.Sub); E(il, OpCodes.Stloc, dx);
    E(il, OpCodes.Ldloc, dx); E(il, OpCodes.Call, abs); E(il, OpCodes.Stloc, absDx);
    E(il, OpCodes.Ldloc, selected); E(il, OpCodes.Ldflda, fPosition); E(il, OpCodes.Ldfld, vecZ);
    E(il, OpCodes.Ldarg_0); E(il, OpCodes.Ldflda, fPrevious); E(il, OpCodes.Ldfld, vecZ); E(il, OpCodes.Sub); E(il, OpCodes.Stloc, dz);
    // Native COMISS deadzone,absDx + JA: use x=0 only for ordered absDx < deadzone. NaN goes full-x.
    E(il, OpCodes.Ldsfld, fDeadzone); E(il, OpCodes.Ldloc, absDx); E(il, OpCodes.Ble_Un, useFullX);
    E(il, OpCodes.Ldc_R4, 0f); E(il, OpCodes.Stloc, dx);
    E(il, OpCodes.Ldloc, selected); E(il, OpCodes.Ldflda, fPosition); E(il, OpCodes.Ldfld, vecY);
    E(il, OpCodes.Ldarg_0); E(il, OpCodes.Ldflda, fPrevious); E(il, OpCodes.Ldfld, vecY); E(il, OpCodes.Sub); E(il, OpCodes.Stloc, dy);
    E(il, OpCodes.Br, vectorReady);
    il.Append(useFullX);
    // Re-read x/y pair on the full-x branch, matching the native MOVSD pair load.
    E(il, OpCodes.Ldloc, selected); E(il, OpCodes.Ldflda, fPosition); E(il, OpCodes.Ldfld, vecX);
    E(il, OpCodes.Ldarg_0); E(il, OpCodes.Ldflda, fPrevious); E(il, OpCodes.Ldfld, vecX); E(il, OpCodes.Sub); E(il, OpCodes.Stloc, dx);
    E(il, OpCodes.Ldloc, selected); E(il, OpCodes.Ldflda, fPosition); E(il, OpCodes.Ldfld, vecY);
    E(il, OpCodes.Ldarg_0); E(il, OpCodes.Ldflda, fPrevious); E(il, OpCodes.Ldfld, vecY); E(il, OpCodes.Sub); E(il, OpCodes.Stloc, dy);

    il.Append(vectorReady);
    // magSq is float; native converts that float to double, sqrt, then converts back to float.
    E(il, OpCodes.Ldloc, dx); E(il, OpCodes.Ldloc, dx); E(il, OpCodes.Mul);
    E(il, OpCodes.Ldloc, dy); E(il, OpCodes.Ldloc, dy); E(il, OpCodes.Mul); E(il, OpCodes.Add);
    E(il, OpCodes.Ldloc, dz); E(il, OpCodes.Ldloc, dz); E(il, OpCodes.Mul); E(il, OpCodes.Add); E(il, OpCodes.Stloc, magSq);
    E(il, OpCodes.Ldloc, magSq); E(il, OpCodes.Conv_R8); E(il, OpCodes.Call, sqrt); E(il, OpCodes.Conv_R4); E(il, OpCodes.Stloc, mag);
    // COMISS magnitude,1e-5 + JA: only ordered > epsilon normalizes. NaN takes the zero branch.
    E(il, OpCodes.Ldloc, mag); E(il, OpCodes.Ldc_R4, 1e-5f); E(il, OpCodes.Bgt, normalizeNonZero);
    E(il, OpCodes.Ldloca, result); E(il, OpCodes.Initobj, vec3); E(il, OpCodes.Br, multiplyAndReturn);

    il.Append(normalizeNonZero);
    E(il, OpCodes.Ldloca, result); E(il, OpCodes.Ldloc, dx); E(il, OpCodes.Ldloc, mag); E(il, OpCodes.Div); E(il, OpCodes.Stfld, vecX);
    E(il, OpCodes.Ldloca, result); E(il, OpCodes.Ldloc, dy); E(il, OpCodes.Ldloc, mag); E(il, OpCodes.Div); E(il, OpCodes.Stfld, vecY);
    E(il, OpCodes.Ldloca, result); E(il, OpCodes.Ldloc, dz); E(il, OpCodes.Ldloc, mag); E(il, OpCodes.Div); E(il, OpCodes.Stfld, vecZ);

    il.Append(multiplyAndReturn);
    // Native multiplies each normalized component by -1 after the epsilon branch; this preserves signed zero.
    E(il, OpCodes.Ldloca, result); E(il, OpCodes.Dup); E(il, OpCodes.Ldfld, vecX); E(il, OpCodes.Ldc_R4, -1f); E(il, OpCodes.Mul); E(il, OpCodes.Stfld, vecX);
    E(il, OpCodes.Ldloca, result); E(il, OpCodes.Dup); E(il, OpCodes.Ldfld, vecY); E(il, OpCodes.Ldc_R4, -1f); E(il, OpCodes.Mul); E(il, OpCodes.Stfld, vecY);
    E(il, OpCodes.Ldloca, result); E(il, OpCodes.Dup); E(il, OpCodes.Ldfld, vecZ); E(il, OpCodes.Ldc_R4, -1f); E(il, OpCodes.Mul); E(il, OpCodes.Stfld, vecZ);
    E(il, OpCodes.Ldloc, result); E(il, OpCodes.Ret);

    il.Append(returnZero);
    // No path/no unarrived node returns +Vector3.zero directly (native does not apply the -1 multiply on this path).
    E(il, OpCodes.Ldloca, result); E(il, OpCodes.Initobj, vec3); E(il, OpCodes.Ldloc, result); E(il, OpCodes.Ret);
}
