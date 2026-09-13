using Mono.Cecil;
using Mono.Cecil.Cil;
using System.Security.Cryptography;
using System.Text;

if (args.Length != 2)
{
    Console.Error.WriteLine("Usage: GodsPVZ.Stage9SeedChooserSetCardChooseListPatch <input.dll> <output.dll>");
    return 2;
}

const string ExpectedInputSha256 = "3b1bfe50761537734063070618ea33e8b0d36d88f3d161861733449d34490243";
const int ExpectedMethodDefCount = 2317;
const uint ExpectedToken = 0x060006BD;

static IEnumerable<TypeDefinition> AllTypes(IEnumerable<TypeDefinition> roots)
{
    foreach (var t in roots)
    {
        yield return t;
        foreach (var n in AllTypes(t.NestedTypes)) yield return n;
    }
}
static uint Raw(IMetadataTokenProvider p) => p.MetadataToken.ToUInt32();
static string OperandSemantic(object? operand, MethodDefinition owner)
{
    if (operand is null) return "";
    if (operand is Instruction bi) return $"I#{owner.Body.Instructions.IndexOf(bi)}";
    if (operand is Instruction[] sw) return "SW[" + string.Join(',', sw.Select(x => owner.Body.Instructions.IndexOf(x))) + "]";
    if (operand is MethodReference mr) return $"M:{mr.FullName}";
    if (operand is FieldReference fr) return $"F:{fr.FullName}";
    if (operand is TypeReference tr) return $"T:{tr.FullName}";
    if (operand is VariableDefinition vr) return $"V:{vr.Index}:{vr.VariableType.FullName}";
    if (operand is ParameterDefinition pr) return $"P:{pr.Index}:{pr.ParameterType.FullName}";
    if (operand is string s) return $"S:{Convert.ToBase64String(Encoding.UTF8.GetBytes(s))}";
    return $"C:{Convert.ToString(operand, System.Globalization.CultureInfo.InvariantCulture)}";
}
static string MethodSemantic(MethodDefinition m)
{
    var sb = new StringBuilder();
    sb.Append(m.FullName).Append('|').Append(m.Attributes).Append('|').Append(m.ImplAttributes).Append('|');
    if (!m.HasBody) return sb.Append("NOBODY").ToString();
    sb.Append("init=").Append(m.Body.InitLocals).Append(";max=").Append(m.Body.MaxStackSize).Append(';');
    foreach (var v in m.Body.Variables) sb.Append("L:").Append(v.Index).Append(':').Append(v.VariableType.FullName).Append(';');
    foreach (var i in m.Body.Instructions) sb.Append("I:").Append(i.OpCode.Code).Append(':').Append(OperandSemantic(i.Operand, m)).Append(';');
    return sb.ToString();
}

var input = Path.GetFullPath(args[0]);
var output = Path.GetFullPath(args[1]);
var inputSha = Convert.ToHexString(SHA256.HashData(File.ReadAllBytes(input))).ToLowerInvariant();
Console.WriteLine($"INPUT_SHA256 {inputSha}");
if (inputSha != ExpectedInputSha256) throw new InvalidDataException($"input SHA mismatch: {inputSha}");

using var module = ModuleDefinition.ReadModule(input, new ReaderParameters { InMemory = true, ReadingMode = ReadingMode.Immediate });
var types = AllTypes(module.Types).ToList();
var methods = types.SelectMany(t => t.Methods).ToList();
if (methods.Count != ExpectedMethodDefCount) throw new InvalidDataException($"MethodDef count drifted: {methods.Count}");

var target = methods.SingleOrDefault(m => m.DeclaringType.FullName == "SeedChooserScreen" && m.Name == "SetCardChooseList" && m.Parameters.Count == 0)
    ?? throw new InvalidDataException("SeedChooserScreen.SetCardChooseList() target missing or ambiguous");
var token = Raw(target);
Console.WriteLine($"TARGET_METHOD token=0x{token:X8} name={target.FullName}");
if (token != ExpectedToken) throw new InvalidDataException($"target token drifted: 0x{token:X8}");
if (!target.HasBody || target.Body.CodeSize < 1000) throw new InvalidDataException($"expected large corrupt body missing: codeSize={target.Body.CodeSize}");
if (!target.Body.Instructions.Any(i => i.Operand is FieldReference f && f.DeclaringType.FullName.StartsWith("System.Collections.Generic.List`1") && (f.Name == "_size" || f.Name == "_version" || f.Name == "_items")))
    throw new InvalidDataException("expected corrupt private List<T> access missing");

TypeDefinition TD(string name) => types.SingleOrDefault(t => t.FullName == name) ?? throw new InvalidDataException($"type {name} missing");
FieldDefinition FD(TypeDefinition t, string name) => t.Fields.SingleOrDefault(f => f.Name == name) ?? throw new InvalidDataException($"field {t.FullName}.{name} missing");

var seedType = target.DeclaringType;
var cardType = TD("Card_Choose");
var lawnType = TD("GlobalStaticVars/LawnApp");
var globalsType = TD("GlobalStaticVars");
var savesType = TD("SavesManager");
var saveType = TD("Save");
var plantType = TD("PlantSave");

var cardListField = FD(seedType, "card_ChooseList");
var cardListGoField = FD(seedType, "card_ChooseList_Gameobject");
var cardPrefabField = FD(seedType, "card_Choose_Prefab");
var gLawnAppField = FD(globalsType, "gLawnApp");
var savesManagerField = FD(lawnType, "savesManager");
var playerSaveField = FD(savesType, "playerSave");
var plantSavesField = FD(saveType, "plantSaves");
var cardSeedChooserField = FD(cardType, "seedChooserScreen");

var listCardType = cardListField.FieldType as GenericInstanceType ?? throw new InvalidDataException("card_ChooseList is not List<Card_Choose>");
var listPlantType = plantSavesField.FieldType as GenericInstanceType ?? throw new InvalidDataException("plantSaves is not List<PlantSave>");
if (listCardType.GenericArguments.Count != 1 || listCardType.GenericArguments[0].FullName != "Card_Choose") throw new InvalidDataException("card list generic argument drift");
if (listPlantType.GenericArguments.Count != 1 || listPlantType.GenericArguments[0].FullName != "PlantSave") throw new InvalidDataException("plant save list generic argument drift");
var listCardT = listCardType.ElementType.GenericParameters.Single();
var listPlantT = listPlantType.ElementType.GenericParameters.Single();

MethodReference ListGetCount(GenericInstanceType t) => new("get_Count", module.TypeSystem.Int32, t) { HasThis = true };
MethodReference ListClear(GenericInstanceType t) => new("Clear", module.TypeSystem.Void, t) { HasThis = true };
MethodReference ListGetItem(GenericInstanceType t, GenericParameter gp)
{
    var m = new MethodReference("get_Item", gp, t) { HasThis = true };
    m.Parameters.Add(new ParameterDefinition(module.TypeSystem.Int32));
    return m;
}
MethodReference ListAdd(GenericInstanceType t, GenericParameter gp)
{
    var m = new MethodReference("Add", module.TypeSystem.Void, t) { HasThis = true };
    m.Parameters.Add(new ParameterDefinition(gp));
    return m;
}

var cardCount = ListGetCount(listCardType);
var cardGetItem = ListGetItem(listCardType, listCardT);
var cardAdd = ListAdd(listCardType, listCardT);
var cardClear = ListClear(listCardType);
var plantCount = ListGetCount(listPlantType);
var plantGetItem = ListGetItem(listPlantType, listPlantT);

var targetOperands = target.Body.Instructions.Select(i => i.Operand).ToList();
var allOperands = methods.Where(m => m.HasBody).SelectMany(m => m.Body.Instructions).Select(i => i.Operand).ToList();
MethodReference TargetMethod(string fullName) => targetOperands.OfType<MethodReference>().FirstOrDefault(m => m.FullName == fullName)
    ?? throw new InvalidDataException($"target MethodRef missing: {fullName}");
MethodReference AnyMethod(string fullName) => allOperands.OfType<MethodReference>().FirstOrDefault(m => m.FullName == fullName)
    ?? throw new InvalidDataException($"assembly MethodRef missing: {fullName}");

var componentGetGameObject = TargetMethod("UnityEngine.GameObject UnityEngine.Component::get_gameObject()");
var destroy = TargetMethod("System.Void UnityEngine.Object::Destroy(UnityEngine.Object)");
var instantiateCard = targetOperands.OfType<GenericInstanceMethod>().FirstOrDefault(m => m.Name == "Instantiate" && m.GenericArguments.Count == 1 && m.GenericArguments[0].FullName == "Card_Choose")
    ?? throw new InvalidDataException("Object.Instantiate<Card_Choose> MethodRef missing");
var componentGetTransform = TargetMethod("UnityEngine.Transform UnityEngine.Component::get_transform()");
var gameObjectGetTransform = TargetMethod("UnityEngine.Transform UnityEngine.GameObject::get_transform()");
var setParent = TargetMethod("System.Void UnityEngine.Transform::SetParent(UnityEngine.Transform,System.Boolean)");
var setLocalScale = TargetMethod("System.Void UnityEngine.Transform::set_localScale(UnityEngine.Vector3)");
var initializeCard = TargetMethod("System.Void Card_Choose::Initialize(PlantSave)");
var debugLog = TargetMethod("System.Void UnityEngine.Debug::Log(System.Object)");
var nullRefCtor = TargetMethod("System.Void System.NullReferenceException::.ctor()");
var vector3Ctor = allOperands.OfType<MethodReference>().FirstOrDefault(m => m.FullName == "System.Void UnityEngine.Vector3::.ctor(System.Single,System.Single,System.Single)");
if (vector3Ctor is null)
{
    var vector3Type = setLocalScale.Parameters[0].ParameterType;
    vector3Ctor = new MethodReference(".ctor", module.TypeSystem.Void, vector3Type) { HasThis = true };
    vector3Ctor.Parameters.Add(new ParameterDefinition(module.TypeSystem.Single));
    vector3Ctor.Parameters.Add(new ParameterDefinition(module.TypeSystem.Single));
    vector3Ctor.Parameters.Add(new ParameterDefinition(module.TypeSystem.Single));
}

var changed = new HashSet<uint> { token };
var untouchedBefore = methods.Where(m => !changed.Contains(Raw(m))).ToDictionary(Raw, MethodSemantic);

var body = target.Body;
body.Instructions.Clear();
body.ExceptionHandlers.Clear();
body.Variables.Clear();
body.InitLocals = true;
body.MaxStackSize = 5;
var iLocal = new VariableDefinition(module.TypeSystem.Int32);
var cardLocal = new VariableDefinition(cardType);
var lawnLocal = new VariableDefinition(lawnType);
var savesLocal = new VariableDefinition(savesType);
var saveLocal = new VariableDefinition(saveType);
var plantListLocal = new VariableDefinition(listPlantType);
var plantLocal = new VariableDefinition(plantType);
body.Variables.Add(iLocal);
body.Variables.Add(cardLocal);
body.Variables.Add(lawnLocal);
body.Variables.Add(savesLocal);
body.Variables.Add(saveLocal);
body.Variables.Add(plantListLocal);
body.Variables.Add(plantLocal);
var il = body.GetILProcessor();

var firstInit = il.Create(OpCodes.Ldc_I4_0);
var firstCheck = il.Create(OpCodes.Ldloc, iLocal);
var firstDone = il.Create(OpCodes.Ldarg_0);
var saveOk = il.Create(OpCodes.Ldloc, saveLocal);
var plantsOk = il.Create(OpCodes.Ldc_I4_0);
var secondCheck = il.Create(OpCodes.Ldloc, iLocal);
var secondDone = il.Create(OpCodes.Ldstr, "执行完成");
var throwNull = il.Create(OpCodes.Newobj, nullRefCtor);

// PC x86-64 native authority 0x1803AB510..0x1803AB9FF.
// Native uses two List<T> enumerators. We lower them to public Count/get_Item index loops:
// neither loop mutates the collection it is iterating, and List<T>.Enumerator.Dispose is a no-op.
il.Append(il.Create(OpCodes.Ldarg_0));
il.Append(il.Create(OpCodes.Ldfld, cardListField));
il.Append(il.Create(OpCodes.Brtrue, firstInit));
il.Append(il.Create(OpCodes.Br, throwNull));

il.Append(firstInit);
il.Append(il.Create(OpCodes.Stloc, iLocal));
il.Append(firstCheck);
il.Append(il.Create(OpCodes.Ldarg_0));
il.Append(il.Create(OpCodes.Ldfld, cardListField));
il.Append(il.Create(OpCodes.Callvirt, cardCount));
il.Append(il.Create(OpCodes.Bge, firstDone));
il.Append(il.Create(OpCodes.Ldarg_0));
il.Append(il.Create(OpCodes.Ldfld, cardListField));
il.Append(il.Create(OpCodes.Ldloc, iLocal));
il.Append(il.Create(OpCodes.Callvirt, cardGetItem));
il.Append(il.Create(OpCodes.Callvirt, componentGetGameObject));
il.Append(il.Create(OpCodes.Call, destroy));
il.Append(il.Create(OpCodes.Ldloc, iLocal));
il.Append(il.Create(OpCodes.Ldc_I4_1));
il.Append(il.Create(OpCodes.Add));
il.Append(il.Create(OpCodes.Stloc, iLocal));
il.Append(il.Create(OpCodes.Br, firstCheck));

il.Append(firstDone);
il.Append(il.Create(OpCodes.Ldfld, cardListField));
il.Append(il.Create(OpCodes.Callvirt, cardClear));

il.Append(il.Create(OpCodes.Ldsfld, gLawnAppField));
il.Append(il.Create(OpCodes.Stloc, lawnLocal));
il.Append(il.Create(OpCodes.Ldloc, lawnLocal));
il.Append(il.Create(OpCodes.Brfalse, throwNull));
il.Append(il.Create(OpCodes.Ldloc, lawnLocal));
il.Append(il.Create(OpCodes.Ldfld, savesManagerField));
il.Append(il.Create(OpCodes.Stloc, savesLocal));
il.Append(il.Create(OpCodes.Ldloc, savesLocal));
il.Append(il.Create(OpCodes.Brfalse, throwNull));
il.Append(il.Create(OpCodes.Ldloc, savesLocal));
il.Append(il.Create(OpCodes.Ldfld, playerSaveField));
il.Append(il.Create(OpCodes.Stloc, saveLocal));
il.Append(il.Create(OpCodes.Ldloc, saveLocal));
il.Append(il.Create(OpCodes.Brtrue, saveOk));
il.Append(il.Create(OpCodes.Ldstr, "存档不存在：创建选卡列表时"));
il.Append(il.Create(OpCodes.Call, debugLog));
il.Append(il.Create(OpCodes.Br, throwNull));

il.Append(saveOk);
il.Append(il.Create(OpCodes.Ldfld, plantSavesField));
il.Append(il.Create(OpCodes.Stloc, plantListLocal));
il.Append(il.Create(OpCodes.Ldloc, plantListLocal));
il.Append(il.Create(OpCodes.Brtrue, plantsOk));
il.Append(il.Create(OpCodes.Br, throwNull));

il.Append(plantsOk);
il.Append(il.Create(OpCodes.Stloc, iLocal));
il.Append(secondCheck);
il.Append(il.Create(OpCodes.Ldloc, plantListLocal));
il.Append(il.Create(OpCodes.Callvirt, plantCount));
il.Append(il.Create(OpCodes.Bge, secondDone));
il.Append(il.Create(OpCodes.Ldloc, plantListLocal));
il.Append(il.Create(OpCodes.Ldloc, iLocal));
il.Append(il.Create(OpCodes.Callvirt, plantGetItem));
il.Append(il.Create(OpCodes.Stloc, plantLocal));

il.Append(il.Create(OpCodes.Ldarg_0));
il.Append(il.Create(OpCodes.Ldfld, cardPrefabField));
il.Append(il.Create(OpCodes.Call, instantiateCard));
il.Append(il.Create(OpCodes.Stloc, cardLocal));
il.Append(il.Create(OpCodes.Ldarg_0));
il.Append(il.Create(OpCodes.Ldfld, cardListField));
il.Append(il.Create(OpCodes.Ldloc, cardLocal));
il.Append(il.Create(OpCodes.Callvirt, cardAdd));

il.Append(il.Create(OpCodes.Ldloc, cardLocal));
il.Append(il.Create(OpCodes.Callvirt, componentGetTransform));
il.Append(il.Create(OpCodes.Ldarg_0));
il.Append(il.Create(OpCodes.Ldfld, cardListGoField));
il.Append(il.Create(OpCodes.Callvirt, gameObjectGetTransform));
il.Append(il.Create(OpCodes.Ldc_I4_0));
il.Append(il.Create(OpCodes.Callvirt, setParent));

il.Append(il.Create(OpCodes.Ldloc, cardLocal));
il.Append(il.Create(OpCodes.Callvirt, componentGetTransform));
il.Append(il.Create(OpCodes.Ldc_R4, 0.75f));
il.Append(il.Create(OpCodes.Ldc_R4, 0.75f));
il.Append(il.Create(OpCodes.Ldc_R4, 1.0f));
il.Append(il.Create(OpCodes.Newobj, vector3Ctor));
il.Append(il.Create(OpCodes.Callvirt, setLocalScale));

il.Append(il.Create(OpCodes.Ldloc, cardLocal));
il.Append(il.Create(OpCodes.Ldarg_0));
il.Append(il.Create(OpCodes.Stfld, cardSeedChooserField));
il.Append(il.Create(OpCodes.Ldloc, cardLocal));
il.Append(il.Create(OpCodes.Ldloc, plantLocal));
il.Append(il.Create(OpCodes.Callvirt, initializeCard));
il.Append(il.Create(OpCodes.Ldloc, iLocal));
il.Append(il.Create(OpCodes.Ldc_I4_1));
il.Append(il.Create(OpCodes.Add));
il.Append(il.Create(OpCodes.Stloc, iLocal));
il.Append(il.Create(OpCodes.Br, secondCheck));

il.Append(secondDone);
il.Append(il.Create(OpCodes.Call, debugLog));
il.Append(il.Create(OpCodes.Ret));
il.Append(throwNull);
il.Append(il.Create(OpCodes.Throw));

Console.WriteLine("NATIVE_AUTHORITY token=0x060006BD address=0x00000001803AB510 function_end=0x00000001803ABA00 gameassembly_sha256=9ebd7ca996a5b03fb4a766f7a2502b660d581ddbbb36af4d7bf2f06a211da39d metadata_sha256=ad992341add498bd1018ac980f42171e0737b0793bf945c8714ad70a445b36b9");
Console.WriteLine("PATCH_SETCARDCHOOSELIST destroy_old=1 clear=1 load_player_plants=1 instantiate_add_parent_scale_initialize=1 lowering=index_loops add_signature=!0 card_get_item_return=!0 plant_get_item_return=!0 private_list_fields=0 malformed_list_object_refs=0");

module.Write(output);
var outputSha = Convert.ToHexString(SHA256.HashData(File.ReadAllBytes(output))).ToLowerInvariant();
Console.WriteLine($"OUTPUT_SHA256 {outputSha}");

using var reopen = ModuleDefinition.ReadModule(output, new ReaderParameters { InMemory = true, ReadingMode = ReadingMode.Immediate });
var after = AllTypes(reopen.Types).SelectMany(t => t.Methods).ToList();
if (after.Count != ExpectedMethodDefCount) throw new InvalidDataException("reopen MethodDef count drift");
var rt = after.SingleOrDefault(m => Raw(m) == token) ?? throw new InvalidDataException("reopen target missing");
if (rt.Body.Variables.Count != 7) throw new InvalidDataException($"SetCardChooseList repaired locals drift: {rt.Body.Variables.Count}");
if (rt.Body.ExceptionHandlers.Count != 0) throw new InvalidDataException("unexpected exception handlers after index-loop lowering");
if (rt.Body.Instructions.Any(i => i.Operand is FieldReference f && f.DeclaringType.FullName.StartsWith("System.Collections.Generic.List`1") && (f.Name == "_size" || f.Name == "_version" || f.Name == "_items"))) throw new InvalidDataException("private List<T> field access survived repair");
if (rt.Body.Instructions.Any(i => i.Operand is MethodReference m && (m.DeclaringType.FullName == "System.Collections.Generic.List`1<System.Object>" || m.DeclaringType.FullName.Contains("Enumerator<System.Object>")))) throw new InvalidDataException("malformed List<object>/Enumerator<object> reference survived repair");

var calls = rt.Body.Instructions.Where(i => i.Operand is MethodReference).Select(i => (MethodReference)i.Operand).ToList();
if (calls.Count(m => m.Name == "get_Count" && m.DeclaringType.FullName == "System.Collections.Generic.List`1<Card_Choose>") != 1) throw new InvalidDataException("expected one List<Card_Choose>.Count call");
if (calls.Count(m => m.Name == "get_Count" && m.DeclaringType.FullName == "System.Collections.Generic.List`1<PlantSave>") != 1) throw new InvalidDataException("expected one List<PlantSave>.Count call");
var cardItem = calls.SingleOrDefault(m => m.Name == "get_Item" && m.DeclaringType.FullName == "System.Collections.Generic.List`1<Card_Choose>") ?? throw new InvalidDataException("List<Card_Choose>.get_Item missing");
var plantItem = calls.SingleOrDefault(m => m.Name == "get_Item" && m.DeclaringType.FullName == "System.Collections.Generic.List`1<PlantSave>") ?? throw new InvalidDataException("List<PlantSave>.get_Item missing");
var reopenedAdd = calls.SingleOrDefault(m => m.Name == "Add" && m.DeclaringType.FullName == "System.Collections.Generic.List`1<Card_Choose>") ?? throw new InvalidDataException("List<Card_Choose>.Add missing");
if (cardItem.ReturnType is not GenericParameter cgp || cgp.Type != GenericParameterType.Type || cgp.Position != 0) throw new InvalidDataException($"Card get_Item return is not !0: {cardItem.FullName}");
if (plantItem.ReturnType is not GenericParameter pgp || pgp.Type != GenericParameterType.Type || pgp.Position != 0) throw new InvalidDataException($"Plant get_Item return is not !0: {plantItem.FullName}");
if (reopenedAdd.Parameters.Count != 1 || reopenedAdd.Parameters[0].ParameterType is not GenericParameter agp || agp.Type != GenericParameterType.Type || agp.Position != 0) throw new InvalidDataException($"Card Add signature is not Add(!0): {reopenedAdd.FullName}");
if (calls.Count(m => m.Name == "Clear" && m.DeclaringType.FullName == "System.Collections.Generic.List`1<Card_Choose>") != 1) throw new InvalidDataException("List<Card_Choose>.Clear missing");
if (!calls.Any(m => m is GenericInstanceMethod gim && gim.Name == "Instantiate" && gim.GenericArguments.Count == 1 && gim.GenericArguments[0].FullName == "Card_Choose")) throw new InvalidDataException("Instantiate<Card_Choose> missing after repair");
if (!calls.Any(m => m.FullName == "System.Void Card_Choose::Initialize(PlantSave)")) throw new InvalidDataException("Card_Choose.Initialize(PlantSave) missing after repair");
if (!calls.Any(m => m.FullName == "System.Void UnityEngine.Object::Destroy(UnityEngine.Object)")) throw new InvalidDataException("Object.Destroy missing after repair");
if (!calls.Any(m => m.FullName == "System.Void UnityEngine.Transform::SetParent(UnityEngine.Transform,System.Boolean)")) throw new InvalidDataException("Transform.SetParent missing after repair");
if (!calls.Any(m => m.FullName == "System.Void UnityEngine.Transform::set_localScale(UnityEngine.Vector3)")) throw new InvalidDataException("Transform.localScale setter missing after repair");
foreach (var s in new[] { "存档不存在：创建选卡列表时", "执行完成" }) if (!rt.Body.Instructions.Any(i => i.OpCode.Code == Code.Ldstr && Convert.ToString(i.Operand) == s)) throw new InvalidDataException($"native log string missing: {s}");
Console.WriteLine($"REOPEN_SETCARDCHOOSELIST_PASS locals=7 card_count=1 plant_count=1 card_get_item_return=!0 plant_get_item_return=!0 add_signature=!0 clear=1 instantiate=1 initialize=1 private_list_fields=0 malformed_list_object_refs=0 token=0x{token:X8}");

var untouchedAfter = after.Where(m => !changed.Contains(Raw(m))).ToDictionary(Raw, MethodSemantic);
if (untouchedAfter.Count != untouchedBefore.Count) throw new InvalidDataException("untouched method count drift");
var drift = untouchedBefore.Where(kv => !untouchedAfter.TryGetValue(kv.Key, out var sem) || sem != kv.Value).Select(kv => kv.Key).ToList();
if (drift.Count != 0) throw new InvalidDataException("semantic drift outside SeedChooserScreen.SetCardChooseList: " + string.Join(',', drift.Select(t => $"0x{t:X8}")));
Console.WriteLine($"SEMANTIC_ISOLATION_PASS untouched_methods={untouchedAfter.Count} changed_methods=1");
return 0;
