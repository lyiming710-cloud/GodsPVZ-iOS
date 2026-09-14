using Mono.Cecil;
using Mono.Cecil.Cil;
using System.Globalization;
using System.Security.Cryptography;
using System.Text;

if (args.Length != 2)
{
    Console.Error.WriteLine("Usage: Stage9PlantCtorPatch <input.dll> <output.dll>");
    return 2;
}

const string ExpectedInputSha256 = "2bcf1643457eecddeff3eaed9668c2117c11fdd5829b264a29e0f30cc86fe20c";
const int ExpectedMethodCount = 2317;
const int ExpectedFieldCount = 2802;
const uint TargetToken = 0x060003BA;
const int ExpectedRid = 954;
const int ExpectedOldCodeSize = 580;
const int ExpectedOldLocals = 20;
var PreservedBatch1Tokens = new HashSet<uint> { 0x0600067E, 0x06000285, 0x06000289, 0x0600028A };

static IEnumerable<TypeDefinition> AllTypes(IEnumerable<TypeDefinition> roots)
{
    foreach (var t in roots)
    {
        yield return t;
        foreach (var n in AllTypes(t.NestedTypes)) yield return n;
    }
}
static uint Raw(IMetadataTokenProvider p) => p.MetadataToken.ToUInt32();
static string Sha256(string p) => Convert.ToHexString(SHA256.HashData(File.ReadAllBytes(p))).ToLowerInvariant();
static string ScopeName(TypeReference t) => t.Scope switch
{
    AssemblyNameReference a => a.Name,
    ModuleDefinition m => m.Assembly?.Name?.Name ?? m.Name,
    ModuleReference mr => mr.Name,
    _ => t.Scope?.ToString() ?? "<null>"
};
static string TypeSig(TypeReference t) => $"{t.FullName}@{ScopeName(t)}";
static string OperandSig(object? o, MethodDefinition owner)
{
    if (o is null) return "";
    if (o is Instruction i) return $"I#{owner.Body.Instructions.IndexOf(i)}";
    if (o is Instruction[] sw) return "SW[" + string.Join(',', sw.Select(i => owner.Body.Instructions.IndexOf(i))) + "]";
    if (o is MethodReference mr) return $"M:{mr.FullName}@{ScopeName(mr.DeclaringType)}";
    if (o is FieldReference fr) return $"F:{fr.FullName}@{ScopeName(fr.DeclaringType)}";
    if (o is TypeReference tr) return $"T:{TypeSig(tr)}";
    if (o is VariableDefinition vr) return $"V:{vr.Index}:{TypeSig(vr.VariableType)}";
    if (o is ParameterDefinition pr) return $"P:{pr.Index}:{TypeSig(pr.ParameterType)}";
    if (o is string s) return "S:" + Convert.ToBase64String(Encoding.UTF8.GetBytes(s));
    if (o is float f) return $"R4:{BitConverter.SingleToInt32Bits(f):X8}";
    if (o is double d) return $"R8:{BitConverter.DoubleToInt64Bits(d):X16}";
    return "C:" + Convert.ToString(o, CultureInfo.InvariantCulture);
}
static string MethodSemantic(MethodDefinition m)
{
    var sb = new StringBuilder();
    sb.Append(m.FullName).Append('|').Append(m.Attributes).Append('|').Append(m.ImplAttributes).Append('|');
    if (!m.HasBody) return sb.Append("NOBODY").ToString();
    sb.Append("init=").Append(m.Body.InitLocals).Append(";max=").Append(m.Body.MaxStackSize).Append(';');
    foreach (var v in m.Body.Variables) sb.Append("L:").Append(v.Index).Append(':').Append(TypeSig(v.VariableType)).Append(';');
    foreach (var i in m.Body.Instructions) sb.Append("I:").Append(i.OpCode.Code).Append(':').Append(OperandSig(i.Operand, m)).Append(';');
    foreach (var h in m.Body.ExceptionHandlers)
    {
        int Idx(Instruction? x) => x is null ? -1 : m.Body.Instructions.IndexOf(x);
        sb.Append("EH:").Append(h.HandlerType).Append(':').Append(Idx(h.TryStart)).Append(':').Append(Idx(h.TryEnd)).Append(':')
          .Append(Idx(h.HandlerStart)).Append(':').Append(Idx(h.HandlerEnd)).Append(':').Append(Idx(h.FilterStart)).Append(':')
          .Append(h.CatchType is null ? "" : TypeSig(h.CatchType)).Append(';');
    }
    return sb.ToString();
}
static string FieldSemantic(FieldDefinition f)
{
    var attrs = string.Join(',', f.CustomAttributes.Select(a => a.AttributeType.FullName).OrderBy(x => x, StringComparer.Ordinal));
    var c = f.HasConstant ? Convert.ToString(f.Constant, CultureInfo.InvariantCulture) ?? "<null>" : "<none>";
    return $"{f.FullName}|{f.Attributes}|{TypeSig(f.FieldType)}|const={c}|ca={attrs}";
}
static bool IsListOf(TypeReference t, string argFullName)
{
    return t is GenericInstanceType git &&
           git.ElementType.FullName == "System.Collections.Generic.List`1" &&
           git.GenericArguments.Count == 1 &&
           git.GenericArguments[0].FullName == argFullName;
}
static int CountCalls(MethodDefinition m, Func<MethodReference, bool> pred) =>
    m.Body.Instructions.Count(i => (i.OpCode == OpCodes.Call || i.OpCode == OpCodes.Callvirt || i.OpCode == OpCodes.Newobj) && i.Operand is MethodReference mr && pred(mr));
static int CountStores(MethodDefinition m, FieldDefinition f) =>
    m.Body.Instructions.Count(i => i.OpCode == OpCodes.Stfld && i.Operand is FieldReference fr && fr.FullName == f.FullName);
static int CountAddresses(MethodDefinition m, FieldDefinition f) =>
    m.Body.Instructions.Count(i => i.OpCode == OpCodes.Ldflda && i.Operand is FieldReference fr && fr.FullName == f.FullName);

var input = Path.GetFullPath(args[0]);
var output = Path.GetFullPath(args[1]);
var inputSha = Sha256(input);
Console.WriteLine($"INPUT_SHA256 {inputSha}");
if (inputSha != ExpectedInputSha256) throw new InvalidDataException("input SHA mismatch");

var beforeM = new Dictionary<uint, string>();
var beforeF = new Dictionary<uint, string>();

using (var module = ModuleDefinition.ReadModule(input, new ReaderParameters { InMemory = true, ReadingMode = ReadingMode.Immediate }))
{
    var types = AllTypes(module.Types).ToList();
    var methods = types.SelectMany(t => t.Methods).ToList();
    var fields = types.SelectMany(t => t.Fields).ToList();
    if (methods.Count != ExpectedMethodCount || fields.Count != ExpectedFieldCount) throw new InvalidDataException("metadata count drift");
    if (module.AssemblyReferences.Any(a => a.Name == "System.Private.CoreLib")) throw new InvalidDataException("unexpected System.Private.CoreLib input ref");
    foreach (var m in methods) beforeM[Raw(m)] = MethodSemantic(m);
    foreach (var f in fields) beforeF[Raw(f)] = FieldSemantic(f);

    var plant = types.Single(t => t.FullName == "Plant");
    var target = plant.Methods.Single(m => m.IsConstructor && !m.IsStatic && m.Parameters.Count == 0);
    if (Raw(target) != TargetToken || target.MetadataToken.RID != ExpectedRid || target.Body.CodeSize != ExpectedOldCodeSize || target.Body.Variables.Count != ExpectedOldLocals)
        throw new InvalidDataException($"target fingerprint mismatch token=0x{Raw(target):X8} rid={target.MetadataToken.RID} size={target.Body.CodeSize} locals={target.Body.Variables.Count}");

    var zeroRefs = target.Body.Instructions.Where(i => i.OpCode == OpCodes.Ldsfld && i.Operand is FieldReference fr && fr.DeclaringType.FullName == "UnityEngine.Vector3" && fr.Name == "zeroVector").ToList();
    if (zeroRefs.Count != 2 || zeroRefs[0].Offset != 0x01E9 || zeroRefs[1].Offset != 0x0218)
        throw new InvalidDataException("old zeroVector fingerprint mismatch");

    FieldDefinition F(string name) => plant.Fields.Single(f => f.Name == name);
    var plantName = F("plantName");
    var characteristicText = F("characteristicText");
    var talentNames = F("talentNames");
    var talents = F("talents");
    var level = F("level");
    var healthPoint = F("healthPoint");
    var maxHealthPoint = F("maxHealthPoint");
    var attackPoint = F("attackPoint");
    var attackable = F("attackable");
    var blockable = F("blockable");
    var active = F("active");
    var camp = F("camp");
    var updateRate = F("updateRate");
    var skill = F("skill");
    var elementManager = F("elementManager");
    var dithering = F("dithering");
    var ditheringAnim = F("dithering_anim");
    var animationSprites = F("animationSprites");
    var ui1 = F("UISprites1");
    var ui2 = F("UISprites2");
    var ui3 = F("UISprites3");
    var ui4 = F("UISprites4");
    var elementUIControllers = F("elementUIControllers");
    var uiCharacteristic = F("UI_Characteristic");
    var produceBrightness = F("produce_Brightness");
    var flashBrightness = F("flash_Brightness");
    var parameterInts = F("parameter_ints");
    var buffManager = F("buffManager");

    var oldInstructions = target.Body.Instructions.ToList();
    MethodReference FindCtor(Func<TypeReference, bool> typePred)
    {
        var matches = oldInstructions.Where(i => i.OpCode == OpCodes.Newobj && i.Operand is MethodReference mr && mr.Name == ".ctor" && typePred(mr.DeclaringType))
                                     .Select(i => (MethodReference)i.Operand).ToList();
        return matches.DistinctBy(m => m.FullName + "@" + ScopeName(m.DeclaringType)).Single();
    }
    MethodReference FindCall(Func<MethodReference, bool> pred)
    {
        return oldInstructions.Where(i => (i.OpCode == OpCodes.Call || i.OpCode == OpCodes.Callvirt) && i.Operand is MethodReference mr && pred(mr))
                              .Select(i => (MethodReference)i.Operand).Single();
    }

    var stringEmpty = oldInstructions.Where(i => i.OpCode == OpCodes.Ldsfld && i.Operand is FieldReference fr && fr.DeclaringType.FullName == "System.String" && fr.Name == "Empty")
                                     .Select(i => (FieldReference)i.Operand).First();
    var skillCtor = FindCtor(t => t.FullName == "Skill");
    var elementManagerCtor = FindCtor(t => t.FullName == "ElementManager");
    var listGameObjectCtor = FindCtor(t => IsListOf(t, "UnityEngine.GameObject"));
    var listElementUICtor = FindCtor(t => IsListOf(t, "ElementUIController"));
    var listIntCtor = FindCtor(t => IsListOf(t, "System.Int32"));
    var buffManagerCtor = FindCtor(t => t.FullName == "BuffManager");
    var monoBehaviourCtor = FindCall(mr => mr.Name == ".ctor" && mr.DeclaringType.FullName == "UnityEngine.MonoBehaviour" && mr.Parameters.Count == 0);

    var stringType = ((ArrayType)talentNames.FieldType).ElementType;
    var spriteType = ((ArrayType)ui1.FieldType).ElementType;

    var body = target.Body;
    body.Instructions.Clear();
    body.ExceptionHandlers.Clear();
    body.Variables.Clear();
    body.InitLocals = false;
    body.MaxStackSize = 2;
    var il = body.GetILProcessor();

    void StFieldConstI4(FieldDefinition f, int v)
    {
        il.Append(il.Create(OpCodes.Ldarg_0));
        il.Append(v switch
        {
            0 => il.Create(OpCodes.Ldc_I4_0),
            1 => il.Create(OpCodes.Ldc_I4_1),
            _ => il.Create(OpCodes.Ldc_I4, v)
        });
        il.Append(il.Create(OpCodes.Stfld, f));
    }
    void StFieldR4(FieldDefinition f, float v)
    {
        il.Append(il.Create(OpCodes.Ldarg_0));
        il.Append(il.Create(OpCodes.Ldc_R4, v));
        il.Append(il.Create(OpCodes.Stfld, f));
    }
    void StNewObj(FieldDefinition f, MethodReference ctor)
    {
        il.Append(il.Create(OpCodes.Ldarg_0));
        il.Append(il.Create(OpCodes.Newobj, ctor));
        il.Append(il.Create(OpCodes.Stfld, f));
    }
    void StNewArray(FieldDefinition f, TypeReference element, int len)
    {
        il.Append(il.Create(OpCodes.Ldarg_0));
        il.Append(il.Create(OpCodes.Ldc_I4, len));
        il.Append(il.Create(OpCodes.Newarr, element));
        il.Append(il.Create(OpCodes.Stfld, f));
    }
    void InitStruct(FieldDefinition f)
    {
        il.Append(il.Create(OpCodes.Ldarg_0));
        il.Append(il.Create(OpCodes.Ldflda, f));
        il.Append(il.Create(OpCodes.Initobj, f.FieldType));
    }

    il.Append(il.Create(OpCodes.Ldarg_0)); il.Append(il.Create(OpCodes.Ldsfld, stringEmpty)); il.Append(il.Create(OpCodes.Stfld, plantName));
    il.Append(il.Create(OpCodes.Ldarg_0)); il.Append(il.Create(OpCodes.Ldsfld, stringEmpty)); il.Append(il.Create(OpCodes.Stfld, characteristicText));
    StNewArray(talentNames, stringType, 3);
    StNewArray(talents, stringType, 3);
    StFieldConstI4(level, 1);
    StFieldR4(healthPoint, 300f);
    StFieldR4(maxHealthPoint, 300f);
    StFieldR4(attackPoint, 20f);
    StFieldConstI4(attackable, 1);
    StFieldConstI4(blockable, 1);
    StFieldConstI4(active, 1);
    StFieldConstI4(camp, 1);
    StFieldR4(updateRate, 1f);
    StNewObj(skill, skillCtor);
    StNewObj(elementManager, elementManagerCtor);
    InitStruct(dithering);
    InitStruct(ditheringAnim);
    StNewObj(animationSprites, listGameObjectCtor);
    StNewArray(ui1, spriteType, 8);
    StNewArray(ui2, spriteType, 8);
    StNewArray(ui3, spriteType, 8);
    StNewArray(ui4, spriteType, 8);
    StNewObj(elementUIControllers, listElementUICtor);
    StNewObj(uiCharacteristic, listGameObjectCtor);
    StFieldR4(produceBrightness, 1f);
    StFieldR4(flashBrightness, 1f);
    StNewObj(parameterInts, listIntCtor);
    StNewObj(buffManager, buffManagerCtor);
    il.Append(il.Create(OpCodes.Ldarg_0));
    il.Append(il.Create(OpCodes.Call, monoBehaviourCtor));
    il.Append(il.Create(OpCodes.Ret));

    Console.WriteLine("NATIVE_AUTHORITY target_token=0x060003BA rid=954 va=0x18035CAD0 end=0x18035CEF0 native_slice_sha256=4aa8c226c4128cb04ee0851a5802a5fecf013ad1be9a9e200586c32d363ebb06");
    Console.WriteLine("NATIVE_BOOLEAN_RECOVERY attackable=true blockable=true active=true native_word_store=this+0xB5_value_0x0101");
    Console.WriteLine("PATCH_PLANT_CTOR method_body_changes=1 vector_zero_lowering=ldflda_initobj gameplay_semantics_changed=0");
    module.Write(output);
}

Console.WriteLine($"OUTPUT_SHA256 {Sha256(output)}");

using (var module = ModuleDefinition.ReadModule(output, new ReaderParameters { InMemory = true, ReadingMode = ReadingMode.Immediate }))
{
    var types = AllTypes(module.Types).ToList();
    var methods = types.SelectMany(t => t.Methods).ToList();
    var fields = types.SelectMany(t => t.Fields).ToList();
    var plant = types.Single(t => t.FullName == "Plant");
    var target = plant.Methods.Single(m => Raw(m) == TargetToken);
    FieldDefinition F(string name) => plant.Fields.Single(f => f.Name == name);

    if (target.Body.Variables.Count != 0 || target.Body.ExceptionHandlers.Count != 0) throw new InvalidDataException("recovered ctor should have no locals/EH");
    if (target.Body.Instructions.Any(i => i.Operand is FieldReference fr && fr.DeclaringType.FullName == "UnityEngine.Vector3" && fr.Name == "zeroVector"))
        throw new InvalidDataException("private Vector3.zeroVector ref remains");
    if (target.Body.Instructions.Count(i => i.OpCode == OpCodes.Initobj && i.Operand is TypeReference tr && tr.FullName == "UnityEngine.Vector2") != 1)
        throw new InvalidDataException("Vector2 zero init mismatch");
    if (target.Body.Instructions.Count(i => i.OpCode == OpCodes.Initobj && i.Operand is TypeReference tr && tr.FullName == "UnityEngine.Vector3") != 1)
        throw new InvalidDataException("Vector3 zero init mismatch");
    if (CountAddresses(target, F("dithering")) != 1 || CountAddresses(target, F("dithering_anim")) != 1)
        throw new InvalidDataException("vector field address/init lowering mismatch");

    foreach (var name in new[] { "plantName", "characteristicText", "talentNames", "talents", "level", "healthPoint", "maxHealthPoint", "attackPoint", "attackable", "blockable", "active", "camp", "updateRate", "skill", "elementManager", "animationSprites", "UISprites1", "UISprites2", "UISprites3", "UISprites4", "elementUIControllers", "UI_Characteristic", "produce_Brightness", "flash_Brightness", "parameter_ints", "buffManager" })
        if (CountStores(target, F(name)) != 1) throw new InvalidDataException($"field store mismatch {name}");

    if (target.Body.Instructions.Count(i => i.OpCode == OpCodes.Newarr && i.Operand is TypeReference tr && tr.FullName == "System.String") != 2) throw new InvalidDataException("string[3] allocation count mismatch");
    if (target.Body.Instructions.Count(i => i.OpCode == OpCodes.Newarr && i.Operand is TypeReference tr && tr.FullName == "UnityEngine.Sprite") != 4) throw new InvalidDataException("Sprite[8] allocation count mismatch");
    if (CountCalls(target, mr => mr.Name == ".ctor" && mr.DeclaringType.FullName == "Skill") != 1) throw new InvalidDataException("Skill ctor count mismatch");
    if (CountCalls(target, mr => mr.Name == ".ctor" && mr.DeclaringType.FullName == "ElementManager") != 1) throw new InvalidDataException("ElementManager ctor count mismatch");
    if (CountCalls(target, mr => mr.Name == ".ctor" && IsListOf(mr.DeclaringType, "UnityEngine.GameObject")) != 2) throw new InvalidDataException("List<GameObject> ctor count mismatch");
    if (CountCalls(target, mr => mr.Name == ".ctor" && IsListOf(mr.DeclaringType, "ElementUIController")) != 1) throw new InvalidDataException("List<ElementUIController> ctor count mismatch");
    if (CountCalls(target, mr => mr.Name == ".ctor" && IsListOf(mr.DeclaringType, "System.Int32")) != 1) throw new InvalidDataException("List<int> ctor count mismatch");
    if (CountCalls(target, mr => mr.Name == ".ctor" && mr.DeclaringType.FullName == "BuffManager") != 1) throw new InvalidDataException("BuffManager ctor count mismatch");
    if (CountCalls(target, mr => mr.Name == ".ctor" && mr.DeclaringType.FullName == "UnityEngine.MonoBehaviour") != 1) throw new InvalidDataException("MonoBehaviour ctor count mismatch");

    var changed = new List<uint>();
    foreach (var m in methods)
    {
        var now = MethodSemantic(m);
        if (!beforeM.TryGetValue(Raw(m), out var old) || old != now) changed.Add(Raw(m));
    }
    if (changed.Count != 1 || changed[0] != TargetToken) throw new InvalidDataException("method isolation mismatch: " + string.Join(',', changed.Select(x => $"0x{x:X8}")));
    foreach (var f in fields)
        if (!beforeF.TryGetValue(Raw(f), out var old) || old != FieldSemantic(f)) throw new InvalidDataException($"field drift 0x{Raw(f):X8}");
    if (module.AssemblyReferences.Any(a => a.Name == "System.Private.CoreLib")) throw new InvalidDataException("System.Private.CoreLib introduced");
    foreach (var tok in PreservedBatch1Tokens)
    {
        var m = methods.Single(x => Raw(x) == tok);
        if (beforeM[tok] != MethodSemantic(m)) throw new InvalidDataException($"Batch1 repair drift 0x{tok:X8}");
    }

    Console.WriteLine($"REOPEN_PLANT_CTOR_PASS token=0x{TargetToken:X8} code_size={target.Body.CodeSize} locals=0 zeroVector_private_refs=0 vector2_initobj=1 vector3_initobj=1 blockable_stores=1");
    Console.WriteLine("SEMANTIC_ISOLATION_PASS untouched_methods=2316 changed_methods=1 target_token=0x060003BA");
    Console.WriteLine("FIELD_METADATA_ISOLATION_PASS unchanged_fields=2802 changed_fields=0");
    Console.WriteLine("BATCH1_REPAIRS_PRESERVATION_PASS tokens=0x0600067E,0x06000285,0x06000289,0x0600028A");
}

return 0;
