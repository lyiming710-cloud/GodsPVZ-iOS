using Mono.Cecil;
using Mono.Cecil.Cil;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

if (args.Length != 2)
{
    Console.Error.WriteLine("Usage: GodsPVZ.HF17Patch <input.dll> <output.dll>");
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
TypeReference TR(string full) => module.GetTypeReferences().FirstOrDefault(t => t.FullName == full)
    ?? throw new InvalidDataException($"Missing TypeRef {full}");
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
var hide = T("Hide");
var zombie = T("Zombie");
var fChildren = F(buff, "childBuffs");
var fDuration = F(buff, "duration");
var fHideMode = F(hide, "hide");
var fHideZombie = F(hide, "zombie");
var zInvincible = F(zombie, "invincible");
var zHide = F(zombie, "hide");
var zWaitingTime = F(zombie, "waitingTime");
var zFX = F(zombie, "fX");
var zFY = F(zombie, "fY");

if (fHideMode.FieldType.FullName != "System.Boolean" || fHideZombie.FieldType.FullName != "Zombie")
    throw new InvalidDataException("HF17 Hide field type drift");
if (fDuration.FieldType.FullName != "System.Single" || zWaitingTime.FieldType.FullName != "System.Single" || zFX.FieldType.FullName != "System.Single" || zFY.FieldType.FullName != "System.Single")
    throw new InvalidDataException("HF17 float field type drift");
if (zInvincible.FieldType.FullName != "System.Boolean" || zHide.FieldType.FullName != "System.Boolean")
    throw new InvalidDataException("HF17 Zombie bool field type drift");

var rendererType = TR("UnityEngine.Renderer");
var colorType = TR("UnityEngine.Color");
var vector3Type = TR("UnityEngine.Vector3");
var mathType = TR("System.Math");
var getRenderer = AllRefs().OfType<GenericInstanceMethod>().FirstOrDefault(r => r.Name == "GetComponent" && r.GenericArguments.Count == 1 && r.GenericArguments[0].FullName == "UnityEngine.Renderer")
    ?? throw new InvalidDataException("HF17 missing Component.GetComponent<Renderer> MethodSpec");
var objectImplicit = Ref("UnityEngine.Object", "op_Implicit", "UnityEngine.Object");
var rendererSetEnabled = Ref("UnityEngine.Renderer", "set_enabled", "System.Boolean");
var getTransform = Ref("UnityEngine.Component", "get_transform");
var setPosition = Ref("UnityEngine.Transform", "set_position", "UnityEngine.Vector3");
var colorCtor = Ref("UnityEngine.Color", ".ctor", "System.Single", "System.Single", "System.Single", "System.Single");
var vector3Ctor = Ref("UnityEngine.Vector3", ".ctor", "System.Single", "System.Single", "System.Single");
var setColor = M(zombie, "SetColor", 1);
var nreCtor = AllRefs().FirstOrDefault(r => r.DeclaringType.FullName == "System.NullReferenceException" && r.Name == ".ctor" && r.Parameters.Count == 0)
    ?? throw new InvalidDataException("HF17 missing NullReferenceException::.ctor");
var clamp = new MethodReference("Clamp", module.TypeSystem.Single, mathType) { HasThis = false };
clamp.Parameters.Add(new ParameterDefinition(module.TypeSystem.Single));
clamp.Parameters.Add(new ParameterDefinition(module.TypeSystem.Single));
clamp.Parameters.Add(new ParameterDefinition(module.TypeSystem.Single));

var awakeHide = M(hide, "Awake_Hide", 0);
var updateHide = M(hide, "Updata_Hide", 0);
var endHide = M(hide, "End_Hide", 0);
var buffAwake = M(buff, "Awake", 1);
if (buffAwake.MetadataToken.ToUInt32() != 0x060000F0 || awakeHide.MetadataToken.ToUInt32() != 0x060000FE || updateHide.MetadataToken.ToUInt32() != 0x060000FF || endHide.MetadataToken.ToUInt32() != 0x06000100)
    throw new InvalidDataException("HF17 MethodDef token drift");

PatchHideAwake();
PatchHideUpdate();
PatchHideEnd();
PatchBuffAwake();

Directory.CreateDirectory(Path.GetDirectoryName(output)!);
module.Write(output, new WriterParameters { WriteSymbols = false });

using (var verify = ModuleDefinition.ReadModule(output, new ReaderParameters { InMemory = true, ReadSymbols = false }))
{
    var types = All(verify.Types).ToList();
    var b = types.Single(t => t.Name == "Buff");
    var h = types.Single(t => t.Name == "Hide");
    var ba = b.Methods.Single(m => m.Name == "Awake" && m.Parameters.Count == 1);
    var ha = h.Methods.Single(m => m.Name == "Awake_Hide" && m.Parameters.Count == 0);
    var hu = h.Methods.Single(m => m.Name == "Updata_Hide" && m.Parameters.Count == 0);
    var he = h.Methods.Single(m => m.Name == "End_Hide" && m.Parameters.Count == 0);
    var specs = new[] { ("Buff.Awake", ba, 35), ("Hide.Awake_Hide", ha, 25), ("Hide.Updata_Hide", hu, 55), ("Hide.End_Hide", he, 30) };
    foreach (var (name, m, min) in specs)
    {
        if (!m.HasBody || m.Body.Instructions.Count < min) throw new InvalidDataException($"HF17 {name} body too small: {m.Body.Instructions.Count}");
        if (m.Body.Instructions.Select(i => i.Operand).OfType<MethodReference>().Any(r => r.DeclaringType.FullName.Contains("Cpp2ILHelpers", StringComparison.Ordinal)))
            throw new InvalidDataException($"HF17 {name} still contains Cpp2IL helper calls");
    }
    if (ba.Body.ExceptionHandlers.Count != 1 || ba.Body.ExceptionHandlers[0].HandlerType != ExceptionHandlerType.Finally)
        throw new InvalidDataException("HF17 Buff.Awake must retain one childBuff Enumerator finally");
    if (ba.Body.Instructions.Select(i => i.Operand).OfType<MethodReference>().Count(r => r.Name == "Awake_Hide" && r.DeclaringType.Name == "Hide") != 1)
        throw new InvalidDataException("HF17 Buff.Awake must dispatch Hide.Awake_Hide exactly once");
    if (ba.Body.Instructions.Select(i => i.Operand).OfType<MethodReference>().Count(r => r.Name == "Awake" && r.DeclaringType.Name == "Buff") != 1)
        throw new InvalidDataException("HF17 Buff.Awake must recurse exactly once in IL");
    if (ha.Body.ExceptionHandlers.Count != 0 || hu.Body.ExceptionHandlers.Count != 0 || he.Body.ExceptionHandlers.Count != 0)
        throw new InvalidDataException("HF17 Hide lifecycle methods must have zero EH");
    int St(MethodDefinition m, string decl, string field) => m.Body.Instructions.Count(i => i.OpCode == OpCodes.Stfld && i.Operand is FieldReference f && f.DeclaringType.Name == decl && f.Name == field);
    if (St(ha, "Zombie", "invincible") != 1 || St(ha, "Zombie", "hide") != 2 || St(ha, "Zombie", "waitingTime") != 1)
        throw new InvalidDataException("HF17 Hide.Awake_Hide field-write shape mismatch");
    var urefs = hu.Body.Instructions.Select(i => i.Operand).OfType<MethodReference>().ToList();
    if (urefs.Count(r => r.DeclaringType.FullName == "System.Math" && r.Name == "Clamp" && r.Parameters.Count == 3) != 1 || urefs.Count(r => r.DeclaringType.Name == "Zombie" && r.Name == "SetColor") != 1 || urefs.Count(r => r.DeclaringType.FullName == "UnityEngine.Color" && r.Name == ".ctor") != 1)
        throw new InvalidDataException("HF17 Hide.Updata_Hide call shape mismatch");
    var erefs = he.Body.Instructions.Select(i => i.Operand).OfType<MethodReference>().ToList();
    if (St(he, "Zombie", "invincible") != 1 || erefs.Count(r => r.Name == "GetComponent" && r is GenericInstanceMethod) != 1 || erefs.Count(r => r.DeclaringType.FullName == "UnityEngine.Transform" && r.Name == "set_position") != 1)
        throw new InvalidDataException("HF17 Hide.End_Hide shape mismatch");
    Console.WriteLine($"VERIFY Buff.Awake: token=0x{ba.MetadataToken.ToUInt32():X8}, IL={ba.Body.Instructions.Count}, bytes={ba.Body.CodeSize}, EH={ba.Body.ExceptionHandlers.Count}");
    Console.WriteLine($"VERIFY Hide.Awake_Hide: token=0x{ha.MetadataToken.ToUInt32():X8}, IL={ha.Body.Instructions.Count}, bytes={ha.Body.CodeSize}, EH={ha.Body.ExceptionHandlers.Count}");
    Console.WriteLine($"VERIFY Hide.Updata_Hide: token=0x{hu.MetadataToken.ToUInt32():X8}, IL={hu.Body.Instructions.Count}, bytes={hu.Body.CodeSize}, EH={hu.Body.ExceptionHandlers.Count}");
    Console.WriteLine($"VERIFY Hide.End_Hide: token=0x{he.MetadataToken.ToUInt32():X8}, IL={he.Body.Instructions.Count}, bytes={he.Body.CodeSize}, EH={he.Body.ExceptionHandlers.Count}");
}

Console.WriteLine("HF17 Buff.Awake @0x180310710 and Hide lifecycle @0x18031C530/0x18031C730/0x18031C620 restored from PC native x86-64");
Console.WriteLine($"WROTE {output} ({new FileInfo(output).Length} bytes)");
return 0;

(GenericInstanceType enumType, MethodReference getEnumerator, MethodReference moveNext, MethodReference getCurrent, MethodReference dispose) ChildRefs()
{
    if (fChildren.FieldType is not GenericInstanceType listType || listType.GenericArguments.Count != 1 || listType.GenericArguments[0].FullName != buff.FullName)
        throw new InvalidDataException("HF17 expected childBuffs List<Buff>");
    var oldMoveNext = AllRefs().FirstOrDefault(r => r.Name == "MoveNext" && r.DeclaringType.FullName.Contains("System.Collections.Generic.List`1/Enumerator", StringComparison.Ordinal));
    if (oldMoveNext?.DeclaringType is not GenericInstanceType oldEnum) throw new InvalidDataException("HF17 missing List<Buff>.Enumerator type");
    var enumType = new GenericInstanceType(oldEnum.ElementType); enumType.GenericArguments.Add(buff);
    var ge = new MethodReference("GetEnumerator", enumType, listType) { HasThis = true };
    var mn = new MethodReference("MoveNext", module.TypeSystem.Boolean, enumType) { HasThis = true };
    var gc = new MethodReference("get_Current", buff, enumType) { HasThis = true };
    var dp = new MethodReference("Dispose", module.TypeSystem.Void, enumType) { HasThis = true };
    return (enumType, ge, mn, gc, dp);
}

void PatchBuffAwake()
{
    var m = buffAwake;
    var refs = ChildRefs();
    var body = new MethodBody(m) { InitLocals = true, MaxStackSize = 4 }; m.Body = body;
    var il = body.GetILProcessor();
    var en = new VariableDefinition(refs.enumType);
    var child = new VariableDefinition(buff);
    var h = new VariableDefinition(hide);
    body.Variables.Add(en); body.Variables.Add(child); body.Variables.Add(h);
    var afterChildren = Instruction.Create(OpCodes.Nop);
    var tryStart = Instruction.Create(OpCodes.Nop);
    var loopCheck = Instruction.Create(OpCodes.Nop);
    var loopBody = Instruction.Create(OpCodes.Nop);
    var childOk = Instruction.Create(OpCodes.Nop);
    var finallyStart = Instruction.Create(OpCodes.Nop);
    var afterFinally = Instruction.Create(OpCodes.Nop);
    var ret = Instruction.Create(OpCodes.Ret);

    E(il, OpCodes.Ldarg_1); E(il, OpCodes.Brtrue, afterChildren);
    E(il, OpCodes.Ldarg_0); E(il, OpCodes.Ldfld, fChildren); E(il, OpCodes.Callvirt, refs.getEnumerator); E(il, OpCodes.Stloc, en);
    il.Append(tryStart); E(il, OpCodes.Br, loopCheck);
    il.Append(loopBody);
    E(il, OpCodes.Ldloca, en); E(il, OpCodes.Call, refs.getCurrent); E(il, OpCodes.Stloc, child);
    E(il, OpCodes.Ldloc, child); E(il, OpCodes.Brtrue, childOk);
    E(il, OpCodes.Newobj, nreCtor); E(il, OpCodes.Throw);
    il.Append(childOk);
    E(il, OpCodes.Ldloc, child); E(il, OpCodes.Ldc_I4_1); E(il, OpCodes.Call, m);
    il.Append(loopCheck);
    E(il, OpCodes.Ldloca, en); E(il, OpCodes.Call, refs.moveNext); E(il, OpCodes.Brtrue, loopBody); E(il, OpCodes.Leave, afterFinally);
    il.Append(finallyStart); E(il, OpCodes.Ldloca, en); E(il, OpCodes.Call, refs.dispose); E(il, OpCodes.Endfinally);
    il.Append(afterFinally);
    il.Append(afterChildren);
    E(il, OpCodes.Ldarg_0); E(il, OpCodes.Isinst, hide); E(il, OpCodes.Stloc, h);
    E(il, OpCodes.Ldloc, h); E(il, OpCodes.Brfalse, ret);
    E(il, OpCodes.Ldloc, h); E(il, OpCodes.Call, awakeHide);
    il.Append(ret);
    body.ExceptionHandlers.Add(new ExceptionHandler(ExceptionHandlerType.Finally) { TryStart = tryStart, TryEnd = finallyStart, HandlerStart = finallyStart, HandlerEnd = afterFinally });
}

void PatchHideAwake()
{
    var m = awakeHide;
    var body = new MethodBody(m) { InitLocals = true, MaxStackSize = 3 }; m.Body = body;
    var il = body.GetILProcessor();
    var renderer = new VariableDefinition(rendererType); body.Variables.Add(renderer);
    var show = Instruction.Create(OpCodes.Nop);
    var afterRenderer = Instruction.Create(OpCodes.Nop);
    var ret = Instruction.Create(OpCodes.Ret);

    E(il, OpCodes.Ldarg_0); E(il, OpCodes.Ldfld, fHideMode); E(il, OpCodes.Brfalse, show);
    E(il, OpCodes.Ldarg_0); E(il, OpCodes.Ldfld, fHideZombie); E(il, OpCodes.Ldc_I4_1); E(il, OpCodes.Stfld, zInvincible);
    E(il, OpCodes.Ldarg_0); E(il, OpCodes.Ldfld, fHideZombie); E(il, OpCodes.Ldc_I4_1); E(il, OpCodes.Stfld, zHide);
    E(il, OpCodes.Br, ret);

    il.Append(show);
    E(il, OpCodes.Ldarg_0); E(il, OpCodes.Ldfld, fHideZombie); E(il, OpCodes.Callvirt, getRenderer); E(il, OpCodes.Stloc, renderer);
    E(il, OpCodes.Ldloc, renderer); E(il, OpCodes.Call, objectImplicit); E(il, OpCodes.Brfalse, afterRenderer);
    E(il, OpCodes.Ldloc, renderer); E(il, OpCodes.Ldc_I4_1); E(il, OpCodes.Callvirt, rendererSetEnabled);
    il.Append(afterRenderer);
    E(il, OpCodes.Ldarg_0); E(il, OpCodes.Ldfld, fHideZombie); E(il, OpCodes.Ldc_I4_0); E(il, OpCodes.Stfld, zHide);
    E(il, OpCodes.Ldarg_0); E(il, OpCodes.Ldfld, fHideZombie); E(il, OpCodes.Ldc_R4, 0.02f); E(il, OpCodes.Stfld, zWaitingTime);
    il.Append(ret);
}

void PatchHideUpdate()
{
    var m = updateHide;
    var body = new MethodBody(m) { InitLocals = true, MaxStackSize = 6 }; m.Body = body;
    var il = body.GetILProcessor();
    var d = new VariableDefinition(module.TypeSystem.Single);
    var t = new VariableDefinition(module.TypeSystem.Single);
    var rgb = new VariableDefinition(module.TypeSystem.Single);
    var alphaBase = new VariableDefinition(module.TypeSystem.Single);
    var alpha = new VariableDefinition(module.TypeSystem.Single);
    var color = new VariableDefinition(colorType);
    foreach (var v in new[] { d, t, rgb, alphaBase, alpha, color }) body.Variables.Add(v);
    var clampHigh = Instruction.Create(OpCodes.Nop);
    var clampDone = Instruction.Create(OpCodes.Nop);
    var visiblePath = Instruction.Create(OpCodes.Nop);
    var afterMode = Instruction.Create(OpCodes.Nop);

    E(il, OpCodes.Ldarg_0); E(il, OpCodes.Ldfld, fDuration); E(il, OpCodes.Stloc, d);
    E(il, OpCodes.Ldloc, d); E(il, OpCodes.Ldloc, d); E(il, OpCodes.Add); E(il, OpCodes.Stloc, t);
    E(il, OpCodes.Ldc_R4, 0f); E(il, OpCodes.Ldloc, t); E(il, OpCodes.Cgt); E(il, OpCodes.Brfalse, clampHigh);
    E(il, OpCodes.Ldc_R4, 0f); E(il, OpCodes.Stloc, t); E(il, OpCodes.Br, clampDone);
    il.Append(clampHigh);
    E(il, OpCodes.Ldloc, t); E(il, OpCodes.Ldc_R4, 1f); E(il, OpCodes.Cgt); E(il, OpCodes.Brfalse, clampDone);
    E(il, OpCodes.Ldc_R4, 1f); E(il, OpCodes.Stloc, t);
    il.Append(clampDone);

    E(il, OpCodes.Ldarg_0); E(il, OpCodes.Ldfld, fHideMode); E(il, OpCodes.Brfalse, visiblePath);
    E(il, OpCodes.Ldloc, t); E(il, OpCodes.Ldc_R4, 0.8f); E(il, OpCodes.Mul); E(il, OpCodes.Ldc_R4, 0.2f); E(il, OpCodes.Add); E(il, OpCodes.Stloc, rgb);
    E(il, OpCodes.Ldarg_0); E(il, OpCodes.Ldfld, fDuration); E(il, OpCodes.Stloc, alphaBase);
    E(il, OpCodes.Br, afterMode);

    il.Append(visiblePath);
    E(il, OpCodes.Ldloc, t); E(il, OpCodes.Ldc_R4, -0.8f); E(il, OpCodes.Mul); E(il, OpCodes.Ldc_R4, 1f); E(il, OpCodes.Add); E(il, OpCodes.Stloc, rgb);
    E(il, OpCodes.Ldc_R4, 0.5f); E(il, OpCodes.Ldarg_0); E(il, OpCodes.Ldfld, fDuration); E(il, OpCodes.Sub); E(il, OpCodes.Stloc, alphaBase);

    il.Append(afterMode);
    E(il, OpCodes.Ldloc, alphaBase); E(il, OpCodes.Ldc_R4, 4f); E(il, OpCodes.Mul);
    E(il, OpCodes.Ldc_R4, 0.3f); E(il, OpCodes.Ldc_R4, 1f); E(il, OpCodes.Call, clamp); E(il, OpCodes.Stloc, alpha);
    E(il, OpCodes.Ldloca, color); E(il, OpCodes.Ldloc, rgb); E(il, OpCodes.Ldloc, rgb); E(il, OpCodes.Ldloc, rgb); E(il, OpCodes.Ldloc, alpha); E(il, OpCodes.Call, colorCtor);
    E(il, OpCodes.Ldarg_0); E(il, OpCodes.Ldfld, fHideZombie); E(il, OpCodes.Ldloc, color); E(il, OpCodes.Callvirt, setColor);
    E(il, OpCodes.Ret);
}

void PatchHideEnd()
{
    var m = endHide;
    var body = new MethodBody(m) { InitLocals = true, MaxStackSize = 5 }; m.Body = body;
    var il = body.GetILProcessor();
    var renderer = new VariableDefinition(rendererType);
    var transformType = TR("UnityEngine.Transform");
    var transform = new VariableDefinition(transformType);
    var pos = new VariableDefinition(vector3Type);
    body.Variables.Add(renderer); body.Variables.Add(transform); body.Variables.Add(pos);
    var hidePath = Instruction.Create(OpCodes.Nop);
    var afterRenderer = Instruction.Create(OpCodes.Nop);
    var ret = Instruction.Create(OpCodes.Ret);

    E(il, OpCodes.Ldarg_0); E(il, OpCodes.Ldfld, fHideMode); E(il, OpCodes.Brtrue, hidePath);
    E(il, OpCodes.Ldarg_0); E(il, OpCodes.Ldfld, fHideZombie); E(il, OpCodes.Ldc_I4_0); E(il, OpCodes.Stfld, zInvincible); E(il, OpCodes.Br, ret);

    il.Append(hidePath);
    E(il, OpCodes.Ldarg_0); E(il, OpCodes.Ldfld, fHideZombie); E(il, OpCodes.Callvirt, getRenderer); E(il, OpCodes.Stloc, renderer);
    E(il, OpCodes.Ldloc, renderer); E(il, OpCodes.Call, objectImplicit); E(il, OpCodes.Brfalse, afterRenderer);
    E(il, OpCodes.Ldloc, renderer); E(il, OpCodes.Ldc_I4_0); E(il, OpCodes.Callvirt, rendererSetEnabled);
    il.Append(afterRenderer);
    E(il, OpCodes.Ldarg_0); E(il, OpCodes.Ldfld, fHideZombie); E(il, OpCodes.Callvirt, getTransform); E(il, OpCodes.Stloc, transform);
    E(il, OpCodes.Ldloca, pos);
    E(il, OpCodes.Ldarg_0); E(il, OpCodes.Ldfld, fHideZombie); E(il, OpCodes.Ldfld, zFX);
    E(il, OpCodes.Ldarg_0); E(il, OpCodes.Ldfld, fHideZombie); E(il, OpCodes.Ldfld, zFY);
    E(il, OpCodes.Ldc_R4, 1000f); E(il, OpCodes.Call, vector3Ctor);
    E(il, OpCodes.Ldloc, transform); E(il, OpCodes.Ldloc, pos); E(il, OpCodes.Callvirt, setPosition);
    il.Append(ret);
}
