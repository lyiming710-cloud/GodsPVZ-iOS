using Mono.Cecil;
using Mono.Cecil.Cil;
using System.Globalization;
using System.Security.Cryptography;
using System.Text;

if (args.Length != 2)
{
    Console.Error.WriteLine("Usage: Stage9ResourceManagerDeviceWindowPortraitsPatch <input.dll> <output.dll>");
    return 2;
}

const string ExpectedInputSha256 = "a7448cb9fc2c746c0fb57a535a7bda0516071e0afcf4f322388b5a937bbd8c9d";
const int ExpectedMethodDefCount = 2317;
const int ExpectedFieldCount = 2802;
const uint TargetToken = 0x06000227;
const string TargetType = "ResourceManager";
const string TargetMethod = "Load_devicePortraits_Window";

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
static string ScopeName(TypeReference t) => t.Scope switch
{
    AssemblyNameReference a => a.Name,
    ModuleDefinition m => m.Assembly?.Name?.Name ?? m.Name,
    ModuleReference mr => mr.Name,
    _ => t.Scope?.ToString() ?? "<null>"
};
static string TypeSig(TypeReference t) => $"{t.FullName}@{ScopeName(t)}";
static string OperandSemantic(object? operand, MethodDefinition owner)
{
    if (operand is null) return "";
    if (operand is Instruction i) return $"I#{owner.Body.Instructions.IndexOf(i)}";
    if (operand is Instruction[] sw) return "SW[" + string.Join(',', sw.Select(i => owner.Body.Instructions.IndexOf(i))) + "]";
    if (operand is MethodReference mr) return $"M:{mr.FullName}@{ScopeName(mr.DeclaringType)}";
    if (operand is FieldReference fr) return $"F:{fr.FullName}@{ScopeName(fr.DeclaringType)}";
    if (operand is TypeReference tr) return $"T:{TypeSig(tr)}";
    if (operand is VariableDefinition vr) return $"V:{vr.Index}:{TypeSig(vr.VariableType)}";
    if (operand is ParameterDefinition pr) return $"P:{pr.Index}:{TypeSig(pr.ParameterType)}";
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
    foreach (var v in m.Body.Variables) sb.Append("L:").Append(v.Index).Append(':').Append(TypeSig(v.VariableType)).Append(';');
    foreach (var i in m.Body.Instructions) sb.Append("I:").Append(i.OpCode.Code).Append(':').Append(OperandSemantic(i.Operand, m)).Append(';');
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
    var constant = f.HasConstant ? Convert.ToString(f.Constant, CultureInfo.InvariantCulture) ?? "<null>" : "<none>";
    return $"{f.FullName}|{f.Attributes}|{TypeSig(f.FieldType)}|const={constant}|ca={attrs}";
}
static MethodReference FindInMethod(MethodDefinition m, Func<MethodReference, bool> pred, string label)
{
    foreach (var ins in m.Body.Instructions)
        if ((ins.OpCode == OpCodes.Call || ins.OpCode == OpCodes.Callvirt || ins.OpCode == OpCodes.Newobj) && ins.Operand is MethodReference mr && pred(mr)) return mr;
    throw new InvalidDataException($"required target method reference missing: {label}");
}
static MethodReference FindInModule(IEnumerable<MethodDefinition> methods, Func<MethodReference, bool> pred, string label)
{
    foreach (var m in methods)
        if (m.HasBody)
            foreach (var ins in m.Body.Instructions)
                if ((ins.OpCode == OpCodes.Call || ins.OpCode == OpCodes.Callvirt || ins.OpCode == OpCodes.Newobj) && ins.Operand is MethodReference mr && pred(mr)) return mr;
    throw new InvalidDataException($"required module method reference missing: {label}");
}
static bool IsListOf(MethodReference mr, string elementFullName) =>
    mr.DeclaringType is GenericInstanceType git && git.ElementType.FullName == "System.Collections.Generic.List`1" &&
    git.GenericArguments.Count == 1 && git.GenericArguments[0].FullName == elementFullName;
static bool IsLoadSprite(MethodReference mr, string spriteFullName)
{
    if (mr is not GenericInstanceMethod gim || gim.GenericArguments.Count != 1 || gim.GenericArguments[0].FullName != spriteFullName) return false;
    return gim.ElementMethod.DeclaringType.FullName == "InternalResourceLoader" && gim.ElementMethod.Name == "Load" && gim.ElementMethod.Parameters.Count == 1;
}
static int CountCalls(MethodDefinition m, Func<MethodReference, bool> pred) =>
    m.Body.Instructions.Count(i => (i.OpCode == OpCodes.Call || i.OpCode == OpCodes.Callvirt) && i.Operand is MethodReference mr && pred(mr));

var input = Path.GetFullPath(args[0]);
var output = Path.GetFullPath(args[1]);
var inputSha = Sha256(input);
Console.WriteLine($"INPUT_SHA256 {inputSha}");
if (inputSha != ExpectedInputSha256) throw new InvalidDataException($"input SHA mismatch: {inputSha}");

var beforeMethodSemantics = new Dictionary<uint, string>();
var beforeFieldSemantics = new Dictionary<uint, string>();
var beforeAssemblyRefs = Array.Empty<string>();

using (var module = ModuleDefinition.ReadModule(input, new ReaderParameters { InMemory = true, ReadingMode = ReadingMode.Immediate }))
{
    var types = AllTypes(module.Types).ToList();
    var methods = types.SelectMany(t => t.Methods).ToList();
    var fields = types.SelectMany(t => t.Fields).ToList();
    if (methods.Count != ExpectedMethodDefCount) throw new InvalidDataException($"MethodDef count drifted: {methods.Count}");
    if (fields.Count != ExpectedFieldCount) throw new InvalidDataException($"Field count drifted: {fields.Count}");
    if (module.AssemblyReferences.Any(a => a.Name == "System.Private.CoreLib")) throw new InvalidDataException("input unexpectedly contains System.Private.CoreLib");
    beforeAssemblyRefs = module.AssemblyReferences.Select(a => a.FullName).OrderBy(x => x, StringComparer.Ordinal).ToArray();
    foreach (var m in methods) beforeMethodSemantics[Raw(m)] = MethodSemantic(m);
    foreach (var f in fields) beforeFieldSemantics[Raw(f)] = FieldSemantic(f);

    var rm = types.Single(t => t.FullName == TargetType);
    var target = rm.Methods.Single(m => m.Name == TargetMethod && m.IsStatic && m.Parameters.Count == 0);
    if (Raw(target) != TargetToken) throw new InvalidDataException($"target token mismatch: 0x{Raw(target):X8}");
    if (!target.HasBody || target.Body.CodeSize != 1168 || target.Body.Variables.Count != 56)
        throw new InvalidDataException($"unexpected prepatch body size={target.Body.CodeSize} locals={target.Body.Variables.Count}");

    var privateListRefs = target.Body.Instructions
        .Where(i => i.Operand is FieldReference fr && fr.DeclaringType.FullName.StartsWith("System.Collections.Generic.List`1", StringComparison.Ordinal))
        .Select(i => ((FieldReference)i.Operand).Name).ToList();
    if (privateListRefs.Count(n => n == "_size") != 2) throw new InvalidDataException("prepatch _size fingerprint mismatch");

    var portraitField = rm.Fields.Single(f => f.Name == "devicePortraits_Window");
    if (portraitField.FieldType is not GenericInstanceType listType || listType.GenericArguments.Count != 1) throw new InvalidDataException("devicePortraits_Window type drift");
    var spriteType = listType.GenericArguments[0];
    const string SpriteName = "UnityEngine.Sprite";
    if (spriteType.FullName != SpriteName) throw new InvalidDataException($"Sprite type drift: {spriteType.FullName}");
    var deviceType = types.Single(t => t.FullName == "DeviceType");

    var combine4 = FindInMethod(target, mr => mr.DeclaringType.FullName == "System.IO.Path" && mr.Name == "Combine" && mr.Parameters.Count == 4, "Path.Combine/4");
    var combine2 = FindInMethod(target, mr => mr.DeclaringType.FullName == "System.IO.Path" && mr.Name == "Combine" && mr.Parameters.Count == 2, "Path.Combine/2");
    var typeFromHandle = FindInMethod(target, mr => mr.DeclaringType.FullName == "System.Type" && mr.Name == "GetTypeFromHandle", "Type.GetTypeFromHandle");
    var enumGetValues = FindInMethod(target, mr => mr.DeclaringType.FullName == "System.Enum" && mr.Name == "GetValues" && mr.Parameters.Count == 1, "Enum.GetValues");
    var enumGetName = FindInMethod(target, mr => mr.DeclaringType.FullName == "System.Enum" && mr.Name == "GetName" && mr.Parameters.Count == 2, "Enum.GetName");
    var arrayGetEnumerator = FindInMethod(target, mr => mr.DeclaringType.FullName == "System.Array" && mr.Name == "GetEnumerator", "Array.GetEnumerator");
    var loadSprite = FindInMethod(target, mr => IsLoadSprite(mr, SpriteName), "InternalResourceLoader.Load<Sprite>");
    var objectImplicit = FindInMethod(target, mr => mr.DeclaringType.FullName == "UnityEngine.Object" && mr.Name == "op_Implicit", "UnityEngine.Object.op_Implicit");
    var listGetCount = FindInModule(methods, mr => mr.Name == "get_Count" && mr.Parameters.Count == 0 && IsListOf(mr, SpriteName), "List<Sprite>.get_Count");
    var listAdd = FindInModule(methods, mr => mr.Name == "Add" && mr.Parameters.Count == 1 && IsListOf(mr, SpriteName), "List<Sprite>.Add");
    var listSetItem = FindInModule(methods, mr => mr.Name == "set_Item" && mr.Parameters.Count == 2 && IsListOf(mr, SpriteName), "List<Sprite>.set_Item");

    var ienumType = target.Body.Variables.First(v => v.VariableType.FullName == "System.Collections.IEnumerator").VariableType;
    var ienumMoveNext = new MethodReference("MoveNext", module.TypeSystem.Boolean, ienumType) { HasThis = true };
    var ienumCurrent = new MethodReference("get_Current", module.TypeSystem.Object, ienumType) { HasThis = true };
    var idisposableType = new TypeReference("System", "IDisposable", module, ienumType.Scope);
    var disposeRef = new MethodReference("Dispose", module.TypeSystem.Void, idisposableType) { HasThis = true };

    Console.WriteLine($"TARGET_METHOD token=0x{Raw(target):X8} type={TargetType} method={TargetMethod} code_size={target.Body.CodeSize} locals={target.Body.Variables.Count} corrupt_private_list_size_refs=2");
    Console.WriteLine("NATIVE_AUTHORITY token=0x06000227 address=0x0000000180338B00 function_end=0x0000000180338FF0 semantics=existing_count_return_then_enum_DeviceType_load_Window_sparse_index_pad_null_set_item_foreach_dispose");
    Console.WriteLine("RUNTIME_CAUSAL_EVIDENCE strict_run=34772270056 candidate=a7448cb9fc2c746c0fb57a535a7bda0516071e0afcf4f322388b5a937bbd8c9d exception=FieldAccessException field=List`1._size method=ResourceManager.Load_devicePortraits_Window caller=ResourceManager.LoadSprites");

    target.Body.Instructions.Clear();
    target.Body.Variables.Clear();
    target.Body.ExceptionHandlers.Clear();
    target.Body.InitLocals = true;
    target.Body.MaxStackSize = 4;

    var pathVar = new VariableDefinition(module.TypeSystem.String);
    var valuesVar = new VariableDefinition(arrayGetEnumerator.DeclaringType);
    var enumeratorVar = new VariableDefinition(ienumType);
    var deviceVar = new VariableDefinition(deviceType);
    var nameVar = new VariableDefinition(module.TypeSystem.String);
    var spriteVar = new VariableDefinition(spriteType);
    var disposableVar = new VariableDefinition(idisposableType);
    foreach (var v in new[] { pathVar, valuesVar, enumeratorVar, deviceVar, nameVar, spriteVar, disposableVar }) target.Body.Variables.Add(v);

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
    il.Append(il.Create(OpCodes.Ldstr, "Device"));
    il.Append(il.Create(OpCodes.Ldstr, "Window"));
    il.Append(il.Create(OpCodes.Call, combine4));
    il.Append(il.Create(OpCodes.Stloc, pathVar));
    il.Append(il.Create(OpCodes.Ldtoken, deviceType));
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
    il.Append(il.Create(OpCodes.Unbox_Any, deviceType));
    il.Append(il.Create(OpCodes.Stloc, deviceVar));
    il.Append(il.Create(OpCodes.Ldtoken, deviceType));
    il.Append(il.Create(OpCodes.Call, typeFromHandle));
    il.Append(il.Create(OpCodes.Ldloc, deviceVar));
    il.Append(il.Create(OpCodes.Box, deviceType));
    il.Append(il.Create(OpCodes.Call, enumGetName));
    il.Append(il.Create(OpCodes.Stloc, nameVar));
    il.Append(il.Create(OpCodes.Ldloc, pathVar));
    il.Append(il.Create(OpCodes.Ldloc, nameVar));
    il.Append(il.Create(OpCodes.Call, combine2));
    il.Append(il.Create(OpCodes.Call, loadSprite));
    il.Append(il.Create(OpCodes.Stloc, spriteVar));
    il.Append(il.Create(OpCodes.Ldloc, spriteVar));
    il.Append(il.Create(OpCodes.Call, objectImplicit));
    il.Append(il.Create(OpCodes.Brfalse, loopCheck));

    var padCheck = il.Create(OpCodes.Ldsfld, portraitField);
    var assign = il.Create(OpCodes.Ldsfld, portraitField);
    il.Append(padCheck);
    il.Append(il.Create(OpCodes.Callvirt, listGetCount));
    il.Append(il.Create(OpCodes.Ldloc, deviceVar));
    il.Append(il.Create(OpCodes.Cgt));
    il.Append(il.Create(OpCodes.Brtrue, assign));
    il.Append(il.Create(OpCodes.Ldsfld, portraitField));
    il.Append(il.Create(OpCodes.Ldnull));
    il.Append(il.Create(OpCodes.Callvirt, listAdd));
    il.Append(il.Create(OpCodes.Br, padCheck));

    il.Append(assign);
    il.Append(il.Create(OpCodes.Ldloc, deviceVar));
    il.Append(il.Create(OpCodes.Ldloc, spriteVar));
    il.Append(il.Create(OpCodes.Callvirt, listSetItem));
    il.Append(loopCheck);
    il.Append(il.Create(OpCodes.Callvirt, ienumMoveNext));
    il.Append(il.Create(OpCodes.Brtrue, loopBody));
    il.Append(il.Create(OpCodes.Leave, afterFinally));

    var finallyStart = il.Create(OpCodes.Ldloc, enumeratorVar);
    var noDispose = il.Create(OpCodes.Endfinally);
    il.Append(finallyStart);
    il.Append(il.Create(OpCodes.Isinst, idisposableType));
    il.Append(il.Create(OpCodes.Stloc, disposableVar));
    il.Append(il.Create(OpCodes.Ldloc, disposableVar));
    il.Append(il.Create(OpCodes.Brfalse, noDispose));
    il.Append(il.Create(OpCodes.Ldloc, disposableVar));
    il.Append(il.Create(OpCodes.Callvirt, disposeRef));
    il.Append(noDispose);
    il.Append(afterFinally);
    il.Append(il.Create(OpCodes.Ret));

    target.Body.ExceptionHandlers.Add(new ExceptionHandler(ExceptionHandlerType.Finally)
    {
        TryStart = tryStart,
        TryEnd = finallyStart,
        HandlerStart = finallyStart,
        HandlerEnd = afterFinally
    });

    Console.WriteLine("PATCH_RESOURCE_MANAGER_DEVICE_WINDOW_PORTRAITS method_body_changes=1 field_metadata_changes=0 source=pc_native_sparse_index_semantics");
    module.Write(output);
}

Console.WriteLine($"OUTPUT_SHA256 {Sha256(output)}");

using (var after = ModuleDefinition.ReadModule(output, new ReaderParameters { InMemory = true, ReadingMode = ReadingMode.Immediate }))
{
    var types = AllTypes(after.Types).ToList();
    var methods = types.SelectMany(t => t.Methods).ToList();
    var fields = types.SelectMany(t => t.Fields).ToList();
    if (methods.Count != ExpectedMethodDefCount || fields.Count != ExpectedFieldCount) throw new InvalidDataException("metadata count drift after write");
    if (after.AssemblyReferences.Any(a => a.Name == "System.Private.CoreLib")) throw new InvalidDataException("System.Private.CoreLib introduced");
    var afterAssemblyRefs = after.AssemblyReferences.Select(a => a.FullName).OrderBy(x => x, StringComparer.Ordinal).ToArray();
    if (!beforeAssemblyRefs.SequenceEqual(afterAssemblyRefs)) throw new InvalidDataException("AssemblyRef set drifted");

    var rm = types.Single(t => t.FullName == TargetType);
    var target = rm.Methods.Single(m => m.Name == TargetMethod && m.IsStatic && m.Parameters.Count == 0);
    if (Raw(target) != TargetToken) throw new InvalidDataException("target token drift");
    if (target.Body.Variables.Count != 7) throw new InvalidDataException($"target locals mismatch: {target.Body.Variables.Count}");
    if (target.Body.Instructions.Any(i => i.Operand is FieldReference fr && fr.DeclaringType.FullName.StartsWith("System.Collections.Generic.List`1", StringComparison.Ordinal) && (fr.Name == "_size" || fr.Name == "_version" || fr.Name == "_items")))
        throw new InvalidDataException("private List field access survived");
    if (CountCalls(target, mr => mr.Name == "get_Count" && IsListOf(mr, "UnityEngine.Sprite")) != 2) throw new InvalidDataException("List<Sprite>.Count call count mismatch");
    if (CountCalls(target, mr => mr.Name == "Add" && IsListOf(mr, "UnityEngine.Sprite")) != 1) throw new InvalidDataException("List<Sprite>.Add call count mismatch");
    if (CountCalls(target, mr => mr.Name == "set_Item" && IsListOf(mr, "UnityEngine.Sprite")) != 1) throw new InvalidDataException("List<Sprite>.set_Item call count mismatch");
    if (CountCalls(target, mr => mr.DeclaringType.FullName == "System.Enum" && mr.Name == "GetValues") != 1) throw new InvalidDataException("Enum.GetValues call count mismatch");
    if (CountCalls(target, mr => mr.DeclaringType.FullName == "System.Enum" && mr.Name == "GetName") != 1) throw new InvalidDataException("Enum.GetName call count mismatch");
    if (CountCalls(target, mr => IsLoadSprite(mr, "UnityEngine.Sprite")) != 1) throw new InvalidDataException("Load<Sprite> call count mismatch");
    if (CountCalls(target, mr => mr.DeclaringType.FullName == "UnityEngine.Object" && mr.Name == "op_Implicit") != 1) throw new InvalidDataException("Object.op_Implicit count mismatch");
    if (target.Body.ExceptionHandlers.Count != 1 || target.Body.ExceptionHandlers[0].HandlerType != ExceptionHandlerType.Finally) throw new InvalidDataException("foreach finally mismatch");
    Console.WriteLine("REOPEN_RESOURCE_MANAGER_DEVICE_WINDOW_PORTRAITS_PASS token=0x06000227 list_count_calls=2 list_add_calls=1 list_set_item_calls=1 enum_getvalues_calls=1 enum_getname_calls=1 load_sprite_calls=1 object_implicit_calls=1 private_list_field_refs=0 finally_handlers=1 locals=7 corelib_refs=0");

    int changed = 0, untouched = 0;
    foreach (var m in methods)
    {
        if (!beforeMethodSemantics.TryGetValue(Raw(m), out var old)) throw new InvalidDataException("new method appeared");
        if (old == MethodSemantic(m)) untouched++;
        else
        {
            changed++;
            if (Raw(m) != TargetToken) throw new InvalidDataException($"unexpected method drift: 0x{Raw(m):X8} {m.FullName}");
        }
    }
    if (untouched != 2316 || changed != 1) throw new InvalidDataException($"method isolation mismatch untouched={untouched} changed={changed}");
    foreach (var f in fields)
        if (!beforeFieldSemantics.TryGetValue(Raw(f), out var old) || old != FieldSemantic(f)) throw new InvalidDataException($"field drift: 0x{Raw(f):X8} {f.FullName}");
    Console.WriteLine("SEMANTIC_ISOLATION_PASS untouched_methods=2316 changed_methods=1 target_token=0x06000227");
    Console.WriteLine("FIELD_METADATA_ISOLATION_PASS unchanged_fields=2802 changed_fields=0");

    var loadSprites = rm.Methods.Single(m => m.Name == "LoadSprites" && m.IsStatic && m.Parameters.Count == 0);
    var start = rm.Methods.Single(m => m.Name == "Start" && m.IsStatic && m.Parameters.Count == 0);
    foreach (var n in new[] { "Load_card_Choose_Sprites", "Load_card_Choose_PlantPortraits", "Load_card_Choose_DevicePortraits", "Load_devicePortraits_Window" })
        if (CountCalls(loadSprites, mr => mr.DeclaringType.FullName == "ResourceManager" && mr.Name == n) != 1) throw new InvalidDataException($"LoadSprites call drift: {n}");
    if (CountCalls(start, mr => mr.DeclaringType.FullName == "ResourceManager" && mr.Name == "LoadSprites") != 1) throw new InvalidDataException("Start LoadSprites call drift");
    Console.WriteLine("PRIOR_RECOVERY_PRESERVATION_PASS load_card_choose_sprites_calls=1 load_plant_portraits_calls=1 load_device_portraits_calls=1 load_device_window_portraits_calls=1 start_loadsprites_calls=1 corelib_refs=0");

    var serializeFields = new[] { "lockLogo", "chain", "chosen", "Lv", "background", "foreground", "orderArabesques", "ordersImage", "starsImage", "plantPortrait", "hormonLogo", "dataBackground", "skillLogo", "cliqueLogo", "sunPriceText", "levelText", "serialNumBeChosenText" };
    var cardChoose = types.Single(t => t.FullName == "Card_Choose");
    foreach (var name in serializeFields)
    {
        var f = cardChoose.Fields.Single(x => x.Name == name);
        if (!f.CustomAttributes.Any(a => a.AttributeType.FullName == "UnityEngine.SerializeField")) throw new InvalidDataException($"SerializeField preservation failed: {name}");
    }
    Console.WriteLine("CARD_CHOOSE_SERIALIZEFIELD_PRESERVATION_PASS fields=17");
}

return 0;
