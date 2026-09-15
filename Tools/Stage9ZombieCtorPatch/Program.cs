using Mono.Cecil;
using Mono.Cecil.Cil;
using System.Globalization;
using System.Security.Cryptography;
using System.Text;

if (args.Length != 4)
{
    Console.Error.WriteLine("Usage: Stage9ZombieCtorPatch <input.dll> <output.dll> <expected-input-sha256> <unityjit-linux-mscorlib.dll>");
    return 2;
}

const uint TargetToken = 0x060004BB;
const int ExpectedRid = 1211;
const int ExpectedMethods = 2317;
const int ExpectedFields = 2802;
const int ExpectedOldCodeSize = 457;
const int ExpectedOldLocals = 14;
const string ExpectedCorelibSha = "4d1725f1ae54a22f69b79c2b1569181ed6653dd484c35005e29c40300c6673be";
const string NativeSpanSha = "84d27db79e7cf21970718ac79287badd239f62af082e008e02fea7e6cef88cb0";

static IEnumerable<TypeDefinition> AllTypes(IEnumerable<TypeDefinition> roots)
{
    foreach (var t in roots)
    {
        yield return t;
        foreach (var n in AllTypes(t.NestedTypes)) yield return n;
    }
}
static uint Raw(IMetadataTokenProvider p) => p.MetadataToken.ToUInt32();
static string Sha(string p) => Convert.ToHexString(SHA256.HashData(File.ReadAllBytes(p))).ToLowerInvariant();
static string Scope(IMetadataScope? s) => s switch
{
    AssemblyNameReference a => a.Name,
    ModuleDefinition m => m.Assembly?.Name?.Name ?? m.Name,
    ModuleReference mr => mr.Name,
    _ => s?.ToString() ?? "<null>"
};
static string TypeSig(TypeReference t) => $"{t.FullName}@{Scope(t.Scope)}";
static string OpSig(object? o, MethodDefinition owner)
{
    if (o is null) return "";
    if (o is Instruction i) return $"I#{owner.Body.Instructions.IndexOf(i)}";
    if (o is Instruction[] sw) return "SW[" + string.Join(',', sw.Select(i => owner.Body.Instructions.IndexOf(i))) + "]";
    if (o is MethodReference mr) return $"M:{mr.FullName}@{Scope(mr.DeclaringType.Scope)}";
    if (o is FieldReference fr) return $"F:{fr.FullName}@{Scope(fr.DeclaringType.Scope)}";
    if (o is TypeReference tr) return $"T:{TypeSig(tr)}";
    if (o is VariableDefinition v) return $"V:{v.Index}:{TypeSig(v.VariableType)}";
    if (o is ParameterDefinition p) return $"P:{p.Index}:{TypeSig(p.ParameterType)}";
    if (o is string s) return "S:" + Convert.ToBase64String(Encoding.UTF8.GetBytes(s));
    if (o is float f) return $"R4:{BitConverter.SingleToInt32Bits(f):X8}";
    if (o is double d) return $"R8:{BitConverter.DoubleToInt64Bits(d):X16}";
    return "C:" + Convert.ToString(o, CultureInfo.InvariantCulture);
}
static string MethodSig(MethodDefinition m)
{
    var sb = new StringBuilder();
    sb.Append(m.FullName).Append('|').Append(m.Attributes).Append('|').Append(m.ImplAttributes).Append('|');
    if (!m.HasBody) return sb.Append("NOBODY").ToString();
    sb.Append("init=").Append(m.Body.InitLocals).Append(";max=").Append(m.Body.MaxStackSize).Append(';');
    foreach (var v in m.Body.Variables) sb.Append("L:").Append(v.Index).Append(':').Append(TypeSig(v.VariableType)).Append(';');
    foreach (var i in m.Body.Instructions) sb.Append("I:").Append(i.OpCode.Code).Append(':').Append(OpSig(i.Operand, m)).Append(';');
    foreach (var h in m.Body.ExceptionHandlers)
    {
        int Idx(Instruction? x) => x is null ? -1 : m.Body.Instructions.IndexOf(x);
        sb.Append("EH:").Append(h.HandlerType).Append(':').Append(Idx(h.TryStart)).Append(':').Append(Idx(h.TryEnd)).Append(':')
          .Append(Idx(h.HandlerStart)).Append(':').Append(Idx(h.HandlerEnd)).Append(':').Append(Idx(h.FilterStart)).Append(':')
          .Append(h.CatchType is null ? "" : TypeSig(h.CatchType)).Append(';');
    }
    return sb.ToString();
}
static string FieldSig(FieldDefinition f)
{
    var attrs = string.Join(',', f.CustomAttributes.Select(a => a.AttributeType.FullName).OrderBy(x => x, StringComparer.Ordinal));
    var c = f.HasConstant ? Convert.ToString(f.Constant, CultureInfo.InvariantCulture) ?? "<null>" : "<none>";
    return $"{f.FullName}|{f.Attributes}|{TypeSig(f.FieldType)}|const={c}|ca={attrs}";
}
static IEnumerable<MethodReference> MethodRefs(IEnumerable<MethodDefinition> methods) => methods
    .Where(m => m.HasBody).SelectMany(m => m.Body.Instructions).Select(i => i.Operand).OfType<MethodReference>();
static MethodReference FindRef(IEnumerable<MethodDefinition> methods, Func<MethodReference,bool> pred, string label)
{
    var hits = MethodRefs(methods).Where(pred)
        .GroupBy(m => m.FullName + "@" + Scope(m.DeclaringType.Scope), StringComparer.Ordinal)
        .Select(g => g.First()).ToList();
    if (hits.Count != 1) throw new InvalidDataException($"{label} refs={hits.Count}: {string.Join(" | ", hits.Select(h => h.FullName))}");
    return hits[0];
}
static MethodReference MakeHostInstanceMethod(ModuleDefinition targetModule, MethodDefinition openDefinition, TypeReference closedHost)
{
    var open = targetModule.ImportReference(openDefinition);
    var host = new MethodReference(open.Name, open.ReturnType, closedHost)
    {
        HasThis = open.HasThis,
        ExplicitThis = open.ExplicitThis,
        CallingConvention = open.CallingConvention
    };
    foreach (var p in open.Parameters)
        host.Parameters.Add(new ParameterDefinition(p.Name, p.Attributes, p.ParameterType));
    foreach (var gp in open.GenericParameters)
        host.GenericParameters.Add(new GenericParameter(gp.Name, host));
    return host;
}
static void AssertClosedListCtor(MethodReference ctor, string argFullName)
{
    if (ctor.Name != ".ctor" || ctor.Parameters.Count != 0 || !ctor.HasThis || ctor.ReturnType.FullName != "System.Void")
        throw new InvalidDataException($"List<{argFullName}> ctor signature drift");
    if (ctor.DeclaringType is not GenericInstanceType host || host.ElementType.FullName != "System.Collections.Generic.List`1" || host.GenericArguments.Count != 1 || host.GenericArguments[0].FullName != argFullName)
        throw new InvalidDataException($"List<{argFullName}> closed host drift");
}

var input = Path.GetFullPath(args[0]);
var output = Path.GetFullPath(args[1]);
var expectedInputSha = args[2].Trim().ToLowerInvariant();
var corelib = Path.GetFullPath(args[3]);
if (expectedInputSha.Length != 64 || expectedInputSha.Any(c => !Uri.IsHexDigit(c))) throw new InvalidDataException("expected input SHA is not 64 hex chars");
if (Sha(input) != expectedInputSha) throw new InvalidDataException("input SHA mismatch");
if (Sha(corelib) != ExpectedCorelibSha) throw new InvalidDataException("Unity corelib SHA mismatch");
Console.WriteLine($"INPUT_SHA256 {Sha(input)}");
Console.WriteLine($"UNITYJIT_LINUX_MSCORLIB_SHA256 {Sha(corelib)}");

var beforeM = new Dictionary<uint,string>();
var beforeF = new Dictionary<uint,string>();
string beforeRefs;

using var core = ModuleDefinition.ReadModule(corelib, new ReaderParameters { InMemory=true, ReadingMode=ReadingMode.Immediate });
var listOpen = core.GetType("System.Collections.Generic.List`1") ?? throw new InvalidDataException("List`1 not found in exact corelib");
var listCtorDef = listOpen.Methods.Single(m => m.Name == ".ctor" && !m.IsStatic && m.Parameters.Count == 0);

using (var module = ModuleDefinition.ReadModule(input, new ReaderParameters { InMemory=true, ReadingMode=ReadingMode.Immediate }))
{
    var types = AllTypes(module.Types).ToList();
    var methods = types.SelectMany(t => t.Methods).ToList();
    var fields = types.SelectMany(t => t.Fields).ToList();
    if (methods.Count != ExpectedMethods || fields.Count != ExpectedFields) throw new InvalidDataException("metadata count drift");
    if (module.AssemblyReferences.Any(a => a.Name == "System.Private.CoreLib")) throw new InvalidDataException("input references System.Private.CoreLib");
    foreach (var m in methods) beforeM[Raw(m)] = MethodSig(m);
    foreach (var f in fields) beforeF[Raw(f)] = FieldSig(f);
    beforeRefs = string.Join("\n", module.AssemblyReferences.Select(a => a.FullName).OrderBy(x => x, StringComparer.Ordinal));

    var zombieT = types.Single(t => t.FullName == "Zombie");
    var target = zombieT.Methods.Single(m => m.Name == ".ctor" && !m.IsStatic && m.Parameters.Count == 0);
    if (Raw(target) != TargetToken || target.MetadataToken.RID != ExpectedRid || target.Body.CodeSize != ExpectedOldCodeSize || target.Body.Variables.Count != ExpectedOldLocals)
        throw new InvalidDataException($"Zombie ctor fingerprint drift token=0x{Raw(target):X8} rid={target.MetadataToken.RID} size={target.Body.CodeSize} locals={target.Body.Variables.Count}");
    if (!target.Body.Instructions.Any(i => i.Offset == 0x55 && i.OpCode == OpCodes.Stfld && i.Operand is FieldReference f && f.Name == "color"))
        throw new InvalidDataException("expected IL_0055 Color corruption missing");
    if (target.Body.Instructions.Count(i => i.Operand is FieldReference f && f.DeclaringType.FullName == "UnityEngine.Vector3" && f.Name == "zeroVector") != 3)
        throw new InvalidDataException("expected three fake Vector3.zeroVector refs missing");

    FieldDefinition F(string n) => zombieT.Fields.Single(f => f.Name == n);
    var attackPoint = F("attackPoint");
    var isStant = F("isStant");
    var isOnBoard = F("isOnBoard");
    var camp = F("camp");
    var elementManager = F("elementManager");
    var waitingTime = F("waitingTime");
    var prePath = F("prePath");
    var path = F("path");
    var rSpeed = F("rSpeed");
    var rDirection = F("rDirection");
    var previousPosition = F("previousPosition");
    var animationSprites = F("animationSprites");
    var color = F("color");
    var brightIntensity = F("brightIntensity");
    var brightIntensityArmor2 = F("brightIntensity_Armor2");
    var hpUIController = F("hpUIController");
    var elementUIControllers = F("elementUIControllers");
    var buffManager = F("buffManager");
    var updateRate = F("updateRate");

    MethodReference ExistingCtor(string typeName)
    {
        var refs = target.Body.Instructions.Select(i => i.Operand).OfType<MethodReference>()
            .Where(m => m.Name == ".ctor" && m.Parameters.Count == 0 && m.DeclaringType.FullName == typeName)
            .GroupBy(m => m.FullName + "@" + Scope(m.DeclaringType.Scope), StringComparer.Ordinal).Select(g => g.First()).ToList();
        if (refs.Count != 1) throw new InvalidDataException($"{typeName} ctor refs={refs.Count}");
        return refs[0];
    }
    var elementManagerCtor = ExistingCtor("ElementManager");
    var hpCtor = ExistingCtor("HPUIController_Zombie");
    var buffCtor = ExistingCtor("BuffManager");
    var monoCtor = ExistingCtor("UnityEngine.MonoBehaviour");
    var colorCtor = FindRef(methods, m => m.DeclaringType.FullName == "UnityEngine.Color" && m.Name == ".ctor" && m.Parameters.Count == 4 && m.Parameters.All(p => p.ParameterType.FullName == "System.Single"), "Color.ctor(float,float,float,float)");

    MethodReference ListCtor(FieldDefinition field, string argFullName)
    {
        if (field.FieldType is not GenericInstanceType git || git.ElementType.FullName != "System.Collections.Generic.List`1" || git.GenericArguments.Count != 1 || git.GenericArguments[0].FullName != argFullName)
            throw new InvalidDataException($"{field.Name} is not List<{argFullName}>");
        var mr = MakeHostInstanceMethod(module, listCtorDef, git);
        AssertClosedListCtor(mr, argFullName);
        return mr;
    }
    var enemyPathCtor = ListCtor(prePath, "EnemyPath");
    var enemyPathCtor2 = ListCtor(path, "EnemyPath");
    var gameObjectCtor = ListCtor(animationSprites, "UnityEngine.GameObject");
    var elementUiCtor = ListCtor(elementUIControllers, "ElementUIController");

    Console.WriteLine($"NATIVE_AUTHORITY target_token=0x{TargetToken:X8} rid={ExpectedRid} method_pointer_entry=0x181B85330 va=0x1803741D0 end=0x1803744DD native_span_sha256={NativeSpanSha}");
    Console.WriteLine("NATIVE_SEMANTICS attackPoint=100 isStant=1 isOnBoard=1 camp=2 elementManager=1 waitingTime=3 prePath=empty path=empty rSpeed=zero rDirection=zero previousPosition=zero animationSprites=empty color=white brightIntensity=1 brightIntensity_Armor2=1 hpUIController=1 elementUIControllers=empty buffManager=1 updateRate=1 base_ctor=1");
    Console.WriteLine("RECOVERY_STRATEGY full_single_MethodDef_rebuild=1 typed_Vector3_initobj=1 typed_Color_ctor=1 exact_unityjit_linux_List_ctor=1 metadata_changes=0 null_guard_changes=0 exception_swallowing=0");

    var body = target.Body;
    body.Instructions.Clear(); body.Variables.Clear(); body.ExceptionHandlers.Clear();
    body.InitLocals = true; body.MaxStackSize = 6;
    var zero = new VariableDefinition(rSpeed.FieldType);
    body.Variables.Add(zero);
    var il = body.GetILProcessor();

    void StoreR4(FieldDefinition f, float v) { il.Append(il.Create(OpCodes.Ldarg_0)); il.Append(il.Create(OpCodes.Ldc_R4, v)); il.Append(il.Create(OpCodes.Stfld, f)); }
    void StoreI4(FieldDefinition f, int v) { il.Append(il.Create(OpCodes.Ldarg_0)); il.Append(il.Create(OpCodes.Ldc_I4, v)); il.Append(il.Create(OpCodes.Stfld, f)); }
    void StoreNew(FieldDefinition f, MethodReference ctor) { il.Append(il.Create(OpCodes.Ldarg_0)); il.Append(il.Create(OpCodes.Newobj, ctor)); il.Append(il.Create(OpCodes.Stfld, f)); }
    void StoreZero(FieldDefinition f) { il.Append(il.Create(OpCodes.Ldarg_0)); il.Append(il.Create(OpCodes.Ldloc, zero)); il.Append(il.Create(OpCodes.Stfld, f)); }

    StoreR4(attackPoint, 100f);
    StoreI4(isStant, 1);
    StoreI4(isOnBoard, 1);
    StoreI4(camp, 2);
    StoreNew(elementManager, elementManagerCtor);
    StoreR4(waitingTime, 3f);
    StoreNew(prePath, enemyPathCtor);
    StoreNew(path, enemyPathCtor2);

    il.Append(il.Create(OpCodes.Ldloca, zero));
    il.Append(il.Create(OpCodes.Initobj, rSpeed.FieldType));
    StoreZero(rSpeed);
    StoreZero(rDirection);
    StoreZero(previousPosition);
    StoreNew(animationSprites, gameObjectCtor);

    il.Append(il.Create(OpCodes.Ldarg_0));
    il.Append(il.Create(OpCodes.Ldc_R4, 1f));
    il.Append(il.Create(OpCodes.Ldc_R4, 1f));
    il.Append(il.Create(OpCodes.Ldc_R4, 1f));
    il.Append(il.Create(OpCodes.Ldc_R4, 1f));
    il.Append(il.Create(OpCodes.Newobj, colorCtor));
    il.Append(il.Create(OpCodes.Stfld, color));

    StoreR4(brightIntensity, 1f);
    StoreR4(brightIntensityArmor2, 1f);
    StoreNew(hpUIController, hpCtor);
    StoreNew(elementUIControllers, elementUiCtor);
    StoreNew(buffManager, buffCtor);
    StoreR4(updateRate, 1f);
    il.Append(il.Create(OpCodes.Ldarg_0));
    il.Append(il.Create(OpCodes.Call, monoCtor));
    il.Append(il.Create(OpCodes.Ret));

    Directory.CreateDirectory(Path.GetDirectoryName(output)!);
    module.Write(output);
}

Console.WriteLine($"OUTPUT_SHA256 {Sha(output)}");

var resolver = new DefaultAssemblyResolver();
resolver.AddSearchDirectory(Path.GetDirectoryName(corelib)!);
resolver.AddSearchDirectory(Path.GetDirectoryName(output)!);
using (var module = ModuleDefinition.ReadModule(output, new ReaderParameters { InMemory=true, ReadingMode=ReadingMode.Immediate, AssemblyResolver=resolver }))
{
    var types = AllTypes(module.Types).ToList();
    var methods = types.SelectMany(t => t.Methods).ToList();
    var fields = types.SelectMany(t => t.Fields).ToList();
    var target = methods.Single(m => Raw(m) == TargetToken);
    if (target.MetadataToken.RID != ExpectedRid || target.Body.Variables.Count != 1 || target.Body.Variables[0].VariableType.FullName != "UnityEngine.Vector3")
        throw new InvalidDataException("reopen target/local drift");
    if (target.Body.Variables.Any(v => v.VariableType.FullName == "System.Object" || v.VariableType.FullName == "System.IntPtr"))
        throw new InvalidDataException("damaged object/intptr locals remain");
    if (target.Body.Instructions.Any(i => i.Operand is FieldReference f && f.DeclaringType.FullName == "UnityEngine.Vector3" && f.Name == "zeroVector"))
        throw new InvalidDataException("fake Vector3.zeroVector ref remains");
    if (target.Body.Instructions.Any(i => i.Operand is string s && s.StartsWith("Unmanaged memory load:", StringComparison.Ordinal)))
        throw new InvalidDataException("unmanaged-memory scaffolding remains");
    if (target.Body.Instructions.Count(i => i.OpCode == OpCodes.Initobj && i.Operand is TypeReference t && t.FullName == "UnityEngine.Vector3") != 1)
        throw new InvalidDataException("Vector3 initobj count mismatch");
    if (target.Body.Instructions.Count(i => i.OpCode == OpCodes.Stfld && i.Operand is FieldReference f && new[]{"rSpeed","rDirection","previousPosition"}.Contains(f.Name)) != 3)
        throw new InvalidDataException("Vector3 field store count mismatch");
    if (target.Body.Instructions.Count(i => i.OpCode == OpCodes.Newobj && i.Operand is MethodReference m && m.DeclaringType.FullName == "UnityEngine.Color") != 1)
        throw new InvalidDataException("Color ctor count mismatch");

    var listCtors = target.Body.Instructions.Select(i => i.Operand).OfType<MethodReference>()
        .Where(m => m.Name == ".ctor" && m.DeclaringType is GenericInstanceType git && git.ElementType.FullName == "System.Collections.Generic.List`1").ToList();
    if (listCtors.Count != 4) throw new InvalidDataException($"generic List ctor call count {listCtors.Count}");
    var expectedArgs = new[]{"EnemyPath","EnemyPath","UnityEngine.GameObject","ElementUIController"}.OrderBy(x=>x,StringComparer.Ordinal).ToArray();
    var actualArgs = listCtors.Select(m => ((GenericInstanceType)m.DeclaringType).GenericArguments[0].FullName).OrderBy(x=>x,StringComparer.Ordinal).ToArray();
    if (!expectedArgs.SequenceEqual(actualArgs)) throw new InvalidDataException("generic List ctor closed-host set mismatch");
    foreach (var r in listCtors)
    {
        var arg=((GenericInstanceType)r.DeclaringType).GenericArguments[0].FullName;
        AssertClosedListCtor(r,arg);
        var def=r.Resolve() ?? throw new InvalidDataException($"List<{arg}> ctor did not resolve");
        if (def.Name != ".ctor" || def.DeclaringType.FullName != "System.Collections.Generic.List`1" || def.Parameters.Count != 0)
            throw new InvalidDataException($"List<{arg}> ctor resolved to wrong MethodDef");
        var rp=def.Module.FileName;
        if (string.IsNullOrEmpty(rp) || Sha(rp) != ExpectedCorelibSha) throw new InvalidDataException($"List<{arg}> ctor resolved against wrong corelib");
    }
    Console.WriteLine($"GENERIC_MEMBERREF_RESOLUTION_PASS List_ctor_calls=4 corelib_sha256={ExpectedCorelibSha}");

    var changedM = methods.Where(m => beforeM[Raw(m)] != MethodSig(m)).Select(m => Raw(m)).OrderBy(x => x).ToList();
    if (changedM.Count != 1 || changedM[0] != TargetToken) throw new InvalidDataException("semantic isolation failed: " + string.Join(',', changedM.Select(x => $"0x{x:X8}")));
    var changedF = fields.Where(f => beforeF[Raw(f)] != FieldSig(f)).Select(f => Raw(f)).ToList();
    if (changedF.Count != 0) throw new InvalidDataException("field metadata drift");
    var afterRefs = string.Join("\n", module.AssemblyReferences.Select(a => a.FullName).OrderBy(x => x, StringComparer.Ordinal));
    if (afterRefs != beforeRefs) throw new InvalidDataException("assembly reference set changed");
    if (module.AssemblyReferences.Any(a => a.Name == "System.Private.CoreLib")) throw new InvalidDataException("System.Private.CoreLib pollution");

    Console.WriteLine($"REOPEN_ZOMBIE_CTOR_PASS token=0x{TargetToken:X8} code_size={target.Body.CodeSize} locals=1 object_locals=0 intptr_locals=0 Vector3_initobj=1 Vector3_stores=3 Color_ctor=1 List_ctor_calls=4");
    Console.WriteLine($"SEMANTIC_ISOLATION_PASS untouched_methods={ExpectedMethods-1} changed_methods=1 target=0x{TargetToken:X8}");
    Console.WriteLine($"FIELD_METADATA_ISOLATION_PASS unchanged_fields={ExpectedFields} changed_fields=0");
    Console.WriteLine("FRAMEWORK_REFERENCE_GATE_PASS system_private_corelib_refs=0 assembly_reference_set_unchanged=1");
}
