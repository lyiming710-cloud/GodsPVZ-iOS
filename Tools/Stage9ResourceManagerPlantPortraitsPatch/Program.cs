using Mono.Cecil;
using Mono.Cecil.Cil;
using System.Globalization;
using System.Security.Cryptography;
using System.Text;

if (args.Length != 2)
{
    Console.Error.WriteLine("Usage: Stage9ResourceManagerPlantPortraitsPatch <input.dll> <output.dll>");
    return 2;
}

const string ExpectedInputSha256 = "b908f70d4520bea6369d3e78204d6ff017da8a5525ebac9f82e3603badc3add6";
const int ExpectedMethodDefCount = 2317;
const int ExpectedFieldCount = 2802;
const uint TargetToken = 0x06000223;
const string TargetType = "ResourceManager";
const string TargetMethod = "Load_card_Choose_PlantPortraits";

static IEnumerable<TypeDefinition> AllTypes(IEnumerable<TypeDefinition> roots)
{
    foreach (var t in roots)
    {
        yield return t;
        foreach (var n in AllTypes(t.NestedTypes)) yield return n;
    }
}

static uint Raw(IMetadataTokenProvider p) => p.MetadataToken.ToUInt32();
static string Sha256(string path) => Convert.ToHexString(SHA256.HashData(File.ReadAllBytes(path))).ToLowerInvariant();

static string OperandSemantic(object? operand, MethodDefinition owner)
{
    if (operand is null) return "";
    if (operand is Instruction i) return $"I#{owner.Body.Instructions.IndexOf(i)}";
    if (operand is Instruction[] sw) return "SW[" + string.Join(',', sw.Select(i => owner.Body.Instructions.IndexOf(i))) + "]";
    if (operand is MethodReference mr) return $"M:{mr.FullName}";
    if (operand is FieldReference fr) return $"F:{fr.FullName}";
    if (operand is TypeReference tr) return $"T:{tr.FullName}";
    if (operand is VariableDefinition vr) return $"V:{vr.Index}:{vr.VariableType.FullName}";
    if (operand is ParameterDefinition pr) return $"P:{pr.Index}:{pr.ParameterType.FullName}";
    if (operand is string s) return $"S:{Convert.ToBase64String(Encoding.UTF8.GetBytes(s))}";
    if (operand is float f) return $"R4:{BitConverter.SingleToInt32Bits(f):X8}";
    if (operand is double d) return $"R8:{BitConverter.DoubleToInt64Bits(d):X16}";
    return $"C:{Convert.ToString(operand, CultureInfo.InvariantCulture)}";
}

static string MethodSemantic(MethodDefinition m)
{
    var sb = new StringBuilder();
    sb.Append(m.FullName).Append('|').Append(m.Attributes).Append('|').Append(m.ImplAttributes).Append('|');
    if (!m.HasBody) return sb.Append("NOBODY").ToString();
    sb.Append("init=").Append(m.Body.InitLocals).Append(";max=").Append(m.Body.MaxStackSize).Append(';');
    foreach (var v in m.Body.Variables) sb.Append("L:").Append(v.Index).Append(':').Append(v.VariableType.FullName).Append(';');
    foreach (var i in m.Body.Instructions) sb.Append("I:").Append(i.OpCode.Code).Append(':').Append(OperandSemantic(i.Operand, m)).Append(';');
    foreach (var h in m.Body.ExceptionHandlers)
    {
        int Idx(Instruction? x) => x is null ? -1 : m.Body.Instructions.IndexOf(x);
        sb.Append("EH:").Append(h.HandlerType).Append(':')
          .Append(Idx(h.TryStart)).Append(':').Append(Idx(h.TryEnd)).Append(':')
          .Append(Idx(h.HandlerStart)).Append(':').Append(Idx(h.HandlerEnd)).Append(':')
          .Append(Idx(h.FilterStart)).Append(':').Append(h.CatchType?.FullName ?? "").Append(';');
    }
    return sb.ToString();
}

static string FieldSemantic(FieldDefinition f)
{
    var attrs = string.Join(',', f.CustomAttributes.Select(a => a.AttributeType.FullName).OrderBy(x => x, StringComparer.Ordinal));
    var constant = f.HasConstant ? Convert.ToString(f.Constant, CultureInfo.InvariantCulture) ?? "<null>" : "<none>";
    return $"{f.FullName}|{f.Attributes}|{f.FieldType.FullName}|const={constant}|ca={attrs}";
}

static MethodReference FindInMethod(MethodDefinition m, Func<MethodReference, bool> predicate, string label)
{
    foreach (var ins in m.Body.Instructions)
        if ((ins.OpCode == OpCodes.Call || ins.OpCode == OpCodes.Callvirt) && ins.Operand is MethodReference mr && predicate(mr))
            return mr;
    throw new InvalidDataException($"required target method reference missing: {label}");
}

static MethodReference FindInModule(IEnumerable<MethodDefinition> methods, Func<MethodReference, bool> predicate, string label)
{
    foreach (var m in methods)
        if (m.HasBody)
            foreach (var ins in m.Body.Instructions)
                if ((ins.OpCode == OpCodes.Call || ins.OpCode == OpCodes.Callvirt) && ins.Operand is MethodReference mr && predicate(mr))
                    return mr;
    throw new InvalidDataException($"required module method reference missing: {label}");
}

static bool IsListOf(MethodReference mr, string elementFullName) =>
    mr.DeclaringType is GenericInstanceType git &&
    git.ElementType.FullName == "System.Collections.Generic.List`1" &&
    git.GenericArguments.Count == 1 && git.GenericArguments[0].FullName == elementFullName;

static bool IsLoadSprite(MethodReference mr, string spriteFullName)
{
    if (mr is not GenericInstanceMethod gim || gim.GenericArguments.Count != 1 || gim.GenericArguments[0].FullName != spriteFullName) return false;
    return gim.ElementMethod.DeclaringType.FullName == "InternalResourceLoader" && gim.ElementMethod.Name == "Load" && gim.ElementMethod.Parameters.Count == 1;
}

static int CountCalls(MethodDefinition m, Func<MethodReference, bool> predicate) =>
    m.Body.Instructions.Count(i => (i.OpCode == OpCodes.Call || i.OpCode == OpCodes.Callvirt) && i.Operand is MethodReference mr && predicate(mr));

var input = Path.GetFullPath(args[0]);
var output = Path.GetFullPath(args[1]);
var inputSha = Sha256(input);
Console.WriteLine($"INPUT_SHA256 {inputSha}");
if (inputSha != ExpectedInputSha256) throw new InvalidDataException($"input SHA mismatch: {inputSha}");

var beforeMethodSemantics = new Dictionary<uint, string>();
var beforeFieldSemantics = new Dictionary<uint, string>();

using (var module = ModuleDefinition.ReadModule(input, new ReaderParameters { InMemory = true, ReadingMode = ReadingMode.Immediate }))
{
    var types = AllTypes(module.Types).ToList();
    var methods = types.SelectMany(t => t.Methods).ToList();
    var fields = types.SelectMany(t => t.Fields).ToList();
    if (methods.Count != ExpectedMethodDefCount) throw new InvalidDataException($"MethodDef count drifted: {methods.Count}");
    if (fields.Count != ExpectedFieldCount) throw new InvalidDataException($"Field count drifted: {fields.Count}");
    foreach (var m in methods) beforeMethodSemantics[Raw(m)] = MethodSemantic(m);
    foreach (var f in fields) beforeFieldSemantics[Raw(f)] = FieldSemantic(f);

    var rm = types.SingleOrDefault(t => t.FullName == TargetType) ?? throw new InvalidDataException("ResourceManager missing");
    var target = rm.Methods.SingleOrDefault(m => m.Name == TargetMethod && m.IsStatic && m.Parameters.Count == 0)
        ?? throw new InvalidDataException("PlantPortraits target missing/ambiguous");
    if (Raw(target) != TargetToken) throw new InvalidDataException($"target token mismatch: 0x{Raw(target):X8}");
    if (!target.HasBody || target.Body.CodeSize != 1484 || target.Body.Variables.Count != 69)
        throw new InvalidDataException($"unexpected prepatch body size={target.Body.CodeSize} locals={target.Body.Variables.Count}");

    var privateListRefs = target.Body.Instructions
        .Where(i => i.Operand is FieldReference fr && fr.DeclaringType.FullName.StartsWith("System.Collections.Generic.List`1", StringComparison.Ordinal))
        .Select(i => ((FieldReference)i.Operand).Name).ToHashSet(StringComparer.Ordinal);
    foreach (var n in new[] { "_size", "_version", "_items" })
        if (!privateListRefs.Contains(n)) throw new InvalidDataException($"prepatch corrupt List private-field fingerprint missing: {n}");

    var portraitField = rm.Fields.Single(f => f.Name == "card_Choose_PlantPortraits");
    if (portraitField.FieldType is not GenericInstanceType portraitListType || portraitListType.GenericArguments.Count != 1)
        throw new InvalidDataException("card_Choose_PlantPortraits type drift");
    var spriteType = portraitListType.GenericArguments[0];
    const string SpriteName = "UnityEngine.Sprite";
    if (spriteType.FullName != SpriteName) throw new InvalidDataException($"Sprite type drift: {spriteType.FullName}");
    var plantType = types.Single(t => t.FullName == "PlantType");

    var combine3 = FindInMethod(target, mr => mr.DeclaringType.FullName == "System.IO.Path" && mr.Name == "Combine" && mr.Parameters.Count == 3, "Path.Combine/3");
    var combine2 = FindInMethod(target, mr => mr.DeclaringType.FullName == "System.IO.Path" && mr.Name == "Combine" && mr.Parameters.Count == 2, "Path.Combine/2");
    var typeFromHandle = FindInMethod(target, mr => mr.DeclaringType.FullName == "System.Type" && mr.Name == "GetTypeFromHandle", "Type.GetTypeFromHandle");
    var enumGetValues = FindInMethod(target, mr => mr.DeclaringType.FullName == "System.Enum" && mr.Name == "GetValues" && mr.Parameters.Count == 1, "Enum.GetValues");
    var enumGetName = FindInMethod(target, mr => mr.DeclaringType.FullName == "System.Enum" && mr.Name == "GetName" && mr.Parameters.Count == 2, "Enum.GetName");
    var arrayGetEnumerator = FindInMethod(target, mr => mr.DeclaringType.FullName == "System.Array" && mr.Name == "GetEnumerator", "Array.GetEnumerator");
    var loadSprite = FindInMethod(target, mr => IsLoadSprite(mr, SpriteName), "InternalResourceLoader.Load<Sprite>");
    var objectImplicit = FindInMethod(target, mr => mr.DeclaringType.FullName == "UnityEngine.Object" && mr.Name == "op_Implicit", "UnityEngine.Object.op_Implicit");
    var listGetCount = FindInModule(methods, mr => mr.Name == "get_Count" && mr.Parameters.Count == 0 && IsListOf(mr, SpriteName), "List<Sprite>.get_Count");
    var listAdd = FindInModule(methods, mr => mr.Name == "Add" && mr.Parameters.Count == 1 && IsListOf(mr, SpriteName), "List<Sprite>.Add");

    var ienumType = target.Body.Variables.FirstOrDefault(v => v.VariableType.FullName == "System.Collections.IEnumerator")?.VariableType
        ?? throw new InvalidDataException("IEnumerator type reference missing from prepatch body");
    var ienumMoveNext = new MethodReference("MoveNext", module.TypeSystem.Boolean, module.ImportReference(ienumType)) { HasThis = true };
    var ienumCurrent = new MethodReference("get_Current", module.TypeSystem.Object, module.ImportReference(ienumType)) { HasThis = true };
    var idisposableType = new TypeReference("System", "IDisposable", module, ienumType.Scope);
    var disposeRef = new MethodReference("Dispose", module.TypeSystem.Void, module.ImportReference(idisposableType)) { HasThis = true };

    Console.WriteLine($"TARGET_METHOD token=0x{Raw(target):X8} type={TargetType} method={TargetMethod} code_size={target.Body.CodeSize} locals={target.Body.Variables.Count} corrupt_private_list_fields=_size,_version,_items");
    Console.WriteLine("NATIVE_AUTHORITY token=0x06000223 address=0x0000000180337C10 function_end=0x0000000180338130 semantics=existing_count_return_then_enum_PlantType_load_add_foreach_dispose");
    Console.WriteLine("RUNTIME_CAUSAL_EVIDENCE strict_run=34768204821 candidate=b908f70d4520bea6369d3e78204d6ff017da8a5525ebac9f82e3603badc3add6 exception=FieldAccessException field=List`1._size method=ResourceManager.Load_card_Choose_PlantPortraits caller=ResourceManager.LoadSprites");

    target.Body.Instructions.Clear();
    target.Body.Variables.Clear();
    target.Body.ExceptionHandlers.Clear();
    target.Body.InitLocals = true;
    target.Body.MaxStackSize = 4;

    var pathVar = new VariableDefinition(module.TypeSystem.String);
    var valuesVar = new VariableDefinition(module.ImportReference(typeof(Array)));
    var enumeratorVar = new VariableDefinition(module.ImportReference(ienumType));
    var plantVar = new VariableDefinition(module.ImportReference(plantType));
    var nameVar = new VariableDefinition(module.TypeSystem.String);
    var spriteVar = new VariableDefinition(module.ImportReference(spriteType));
    var disposableVar = new VariableDefinition(module.ImportReference(idisposableType));
    foreach (var v in new[] { pathVar, valuesVar, enumeratorVar, plantVar, nameVar, spriteVar, disposableVar }) target.Body.Variables.Add(v);

    var il = target.Body.GetILProcessor();
    var loadStart = il.Create(OpCodes.Nop);
    il.Append(il.Create(OpCodes.Ldsfld, portraitField));
    il.Append(il.Create(OpCodes.Brfalse, loadStart));
    il.Append(il.Create(OpCodes.Ldsfld, portraitField));
    il.Append(il.Create(OpCodes.Callvirt, listGetCount));
    il.Append(il.Create(OpCodes.Ldc_I4_0));
    il.Append(il.Create(OpCodes.Ble, loadStart));
    il.Append(il.Create(OpCodes.Ldsfld, portraitField));
    il.Append(il.Create(OpCodes.Ret));

    il.Append(loadStart);
    il.Append(il.Create(OpCodes.Ldstr, "sprites"));
    il.Append(il.Create(OpCodes.Ldstr, "Portrait"));
    il.Append(il.Create(OpCodes.Ldstr, "Plant"));
    il.Append(il.Create(OpCodes.Call, combine3));
    il.Append(il.Create(OpCodes.Stloc, pathVar));
    il.Append(il.Create(OpCodes.Ldtoken, plantType));
    il.Append(il.Create(OpCodes.Call, typeFromHandle));
    il.Append(il.Create(OpCodes.Call, enumGetValues));
    il.Append(il.Create(OpCodes.Stloc, valuesVar));
    il.Append(il.Create(OpCodes.Ldloc, valuesVar));
    il.Append(il.Create(OpCodes.Callvirt, arrayGetEnumerator));
    il.Append(il.Create(OpCodes.Stloc, enumeratorVar));

    var loopCheck = il.Create(OpCodes.Ldloc, enumeratorVar);
    var loopBody = il.Create(OpCodes.Ldloc, enumeratorVar);
    var afterFinally = il.Create(OpCodes.Ldsfld, portraitField);
    var tryStart = il.Create(OpCodes.Br, loopCheck);
    il.Append(tryStart);
    il.Append(loopBody);
    il.Append(il.Create(OpCodes.Callvirt, ienumCurrent));
    il.Append(il.Create(OpCodes.Unbox_Any, plantType));
    il.Append(il.Create(OpCodes.Stloc, plantVar));
    il.Append(il.Create(OpCodes.Ldtoken, plantType));
    il.Append(il.Create(OpCodes.Call, typeFromHandle));
    il.Append(il.Create(OpCodes.Ldloc, plantVar));
    il.Append(il.Create(OpCodes.Box, plantType));
    il.Append(il.Create(OpCodes.Call, enumGetName));
    il.Append(il.Create(OpCodes.Stloc, nameVar));
    il.Append(il.Create(OpCodes.Ldloc, pathVar));
    il.Append(il.Create(OpCodes.Ldloc, nameVar));
    il.Append(il.Create(OpCodes.Call, combine2));
    il.Append(il.Create(OpCodes.Call, loadSprite));
    il.Append(il.Create(OpCodes.Stloc, spriteVar));
    il.Append(il.Create(OpCodes.Ldsfld, portraitField));
    il.Append(il.Create(OpCodes.Ldloc, spriteVar));
    il.Append(il.Create(OpCodes.Callvirt, listAdd));
    il.Append(il.Create(OpCodes.Ldloc, spriteVar));
    il.Append(il.Create(OpCodes.Call, objectImplicit));
    il.Append(il.Create(OpCodes.Pop));
    il.Append(loopCheck);
    il.Append(il.Create(OpCodes.Callvirt, ienumMoveNext));
    il.Append(il.Create(OpCodes.Brtrue, loopBody));
    il.Append(il.Create(OpCodes.Leave, afterFinally));

    var finallyStart = il.Create(OpCodes.Ldloc, enumeratorVar);
    var endFinally = il.Create(OpCodes.Endfinally);
    il.Append(finallyStart);
    il.Append(il.Create(OpCodes.Isinst, idisposableType));
    il.Append(il.Create(OpCodes.Stloc, disposableVar));
    il.Append(il.Create(OpCodes.Ldloc, disposableVar));
    il.Append(il.Create(OpCodes.Brfalse, endFinally));
    il.Append(il.Create(OpCodes.Ldloc, disposableVar));
    il.Append(il.Create(OpCodes.Callvirt, disposeRef));
    il.Append(endFinally);
    il.Append(afterFinally);
    il.Append(il.Create(OpCodes.Ret));

    target.Body.ExceptionHandlers.Add(new ExceptionHandler(ExceptionHandlerType.Finally)
    {
        TryStart = tryStart,
        TryEnd = finallyStart,
        HandlerStart = finallyStart,
        HandlerEnd = afterFinally
    });

    module.Write(output);
    Console.WriteLine("PATCH_RESOURCE_MANAGER_PLANT_PORTRAITS method_body_changes=1 field_metadata_changes=0 source=pc_native_source_semantics_foreach_reconstruction");
}

var outputSha = Sha256(output);
Console.WriteLine($"OUTPUT_SHA256 {outputSha}");
if (outputSha == ExpectedInputSha256) throw new InvalidDataException("output SHA unexpectedly identical to input");

using (var after = ModuleDefinition.ReadModule(output, new ReaderParameters { InMemory = true, ReadingMode = ReadingMode.Immediate }))
{
    var types = AllTypes(after.Types).ToList();
    var methods = types.SelectMany(t => t.Methods).ToList();
    var fields = types.SelectMany(t => t.Fields).ToList();
    if (methods.Count != ExpectedMethodDefCount || fields.Count != ExpectedFieldCount) throw new InvalidDataException("postwrite metadata count drift");
    var rm = types.Single(t => t.FullName == TargetType);
    var target = rm.Methods.Single(m => m.Name == TargetMethod && m.IsStatic && m.Parameters.Count == 0);
    if (Raw(target) != TargetToken) throw new InvalidDataException("postwrite target token drift");

    int countCalls = CountCalls(target, mr => mr.Name == "get_Count" && IsListOf(mr, "UnityEngine.Sprite"));
    int addCalls = CountCalls(target, mr => mr.Name == "Add" && IsListOf(mr, "UnityEngine.Sprite"));
    int enumValues = CountCalls(target, mr => mr.DeclaringType.FullName == "System.Enum" && mr.Name == "GetValues");
    int enumNames = CountCalls(target, mr => mr.DeclaringType.FullName == "System.Enum" && mr.Name == "GetName");
    int loads = CountCalls(target, mr => IsLoadSprite(mr, "UnityEngine.Sprite"));
    int implicitCalls = CountCalls(target, mr => mr.DeclaringType.FullName == "UnityEngine.Object" && mr.Name == "op_Implicit");
    int privateRefs = target.Body.Instructions.Count(i => i.Operand is FieldReference fr && fr.DeclaringType.FullName.StartsWith("System.Collections.Generic.List`1", StringComparison.Ordinal) && (fr.Name == "_size" || fr.Name == "_version" || fr.Name == "_items"));
    int finallyCount = target.Body.ExceptionHandlers.Count(h => h.HandlerType == ExceptionHandlerType.Finally);
    if (countCalls != 1 || addCalls != 1 || enumValues != 1 || enumNames != 1 || loads != 1 || implicitCalls != 1 || privateRefs != 0 || finallyCount != 1 || target.Body.Variables.Count != 7)
        throw new InvalidDataException($"reopen shape count={countCalls} add={addCalls} values={enumValues} names={enumNames} loads={loads} implicit={implicitCalls} privateRefs={privateRefs} finally={finallyCount} locals={target.Body.Variables.Count}");
    Console.WriteLine("REOPEN_RESOURCE_MANAGER_PLANT_PORTRAITS_PASS token=0x06000223 list_count_calls=1 list_add_calls=1 enum_getvalues_calls=1 enum_getname_calls=1 load_sprite_calls=1 object_implicit_calls=1 private_list_field_refs=0 finally_handlers=1 locals=7");

    int changedMethods = 0, untouchedMethods = 0;
    foreach (var m in methods)
    {
        if (!beforeMethodSemantics.TryGetValue(Raw(m), out var old)) throw new InvalidDataException($"new method token=0x{Raw(m):X8}");
        if (old == MethodSemantic(m)) untouchedMethods++;
        else
        {
            changedMethods++;
            if (Raw(m) != TargetToken) throw new InvalidDataException($"unexpected method semantic drift token=0x{Raw(m):X8} {m.FullName}");
        }
    }
    if (changedMethods != 1 || untouchedMethods != 2316) throw new InvalidDataException($"method isolation mismatch untouched={untouchedMethods} changed={changedMethods}");
    Console.WriteLine("SEMANTIC_ISOLATION_PASS untouched_methods=2316 changed_methods=1 target_token=0x06000223");

    foreach (var f in fields)
        if (!beforeFieldSemantics.TryGetValue(Raw(f), out var old) || old != FieldSemantic(f))
            throw new InvalidDataException($"field metadata drift token=0x{Raw(f):X8} {f.FullName}");
    Console.WriteLine("FIELD_METADATA_ISOLATION_PASS unchanged_fields=2802 changed_fields=0");

    var loadSprites = rm.Methods.Single(m => m.Name == "LoadSprites" && m.IsStatic && m.Parameters.Count == 0);
    if (CountCalls(loadSprites, mr => mr.FullName == target.FullName) != 1) throw new InvalidDataException("LoadSprites PlantPortraits helper call drift");
    var cardSprites = rm.Methods.Single(m => m.Name == "Load_card_Choose_Sprites" && m.IsStatic && m.Parameters.Count == 0);
    if (Raw(cardSprites) != 0x06000222) throw new InvalidDataException("Card_Choose sprite helper token drift");
    var start = rm.Methods.Single(m => m.Name == "Start" && m.IsStatic && m.Parameters.Count == 0);
    if (CountCalls(start, mr => mr.DeclaringType.FullName == "ResourceManager" && mr.Name == "LoadSprites") != 1) throw new InvalidDataException("Start LoadSprites preservation failed");
    Console.WriteLine("PRIOR_RECOVERY_PRESERVATION_PASS load_card_choose_sprites_token=0x06000222 loadsprites_plantportrait_calls=1 start_loadsprites_calls=1");

    var cc = types.Single(t => t.FullName == "Card_Choose");
    var sfCount = cc.Fields.Count(f => f.CustomAttributes.Any(a => a.AttributeType.FullName == "UnityEngine.SerializeField"));
    if (sfCount != 17) throw new InvalidDataException($"Card_Choose SerializeField preservation failed: {sfCount}");
    Console.WriteLine("CARD_CHOOSE_SERIALIZEFIELD_PRESERVATION_PASS fields=17");
}

return 0;
