using Mono.Cecil;
using Mono.Cecil.Cil;
using System.Security.Cryptography;
using System.Text;

if (args.Length != 2)
{
    Console.Error.WriteLine("Usage: GodsPVZ.Stage9PlantDetailAwakePatch <input.dll> <output.dll>");
    return 2;
}

const string ExpectedInputSha256 = "7bdddb8c5f86db6f87dd9b602a2b850af6999d7965aa09bbfced59ef62a815f3";
const int ExpectedMethodDefCount = 2317;
const uint TargetToken = 0x060006C2u;

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
    if (operand is float f) return $"R4:{BitConverter.SingleToInt32Bits(f):X8}";
    if (operand is double d) return $"R8:{BitConverter.DoubleToInt64Bits(d):X16}";
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

var input = Path.GetFullPath(args[0]);
var output = Path.GetFullPath(args[1]);
var inputSha = Convert.ToHexString(SHA256.HashData(File.ReadAllBytes(input))).ToLowerInvariant();
Console.WriteLine($"INPUT_SHA256 {inputSha}");
if (inputSha != ExpectedInputSha256) throw new InvalidDataException($"input SHA mismatch: {inputSha}");

using var module = ModuleDefinition.ReadModule(input, new ReaderParameters { InMemory = true, ReadingMode = ReadingMode.Immediate });
var allBefore = AllTypes(module.Types).SelectMany(t => t.Methods).ToList();
if (allBefore.Count != ExpectedMethodDefCount) throw new InvalidDataException($"MethodDef count drifted: {allBefore.Count}");

var target = module.LookupToken(new MetadataToken(TokenType.Method, (int)(TargetToken & 0x00FFFFFF))) as MethodDefinition
    ?? throw new InvalidDataException("PlantDetail.Awake token missing");
if (target.DeclaringType.FullName != "SeedChooserScreen/PlantDetail" || target.Name != "Awake" || target.Parameters.Count != 1 || target.Parameters[0].ParameterType.FullName != "Card_Choose" || !target.HasBody)
    throw new InvalidDataException($"target signature drift: {target.FullName}");

Console.WriteLine($"SOURCE_TARGET token=0x{Raw(target):X8} locals={target.Body.Variables.Count} instructions={target.Body.Instructions.Count} code_size={target.Body.CodeSize}");

var untouchedBefore = allBefore.Where(m => Raw(m) != TargetToken).ToDictionary(Raw, MethodSemantic);
var oldRefs = target.Body.Instructions.Select(i => i.Operand).OfType<MethodReference>().ToList();

MethodReference OldRef(string fullName)
{
    var x = oldRefs.FirstOrDefault(m => m.FullName == fullName);
    if (x is null) throw new InvalidDataException($"required existing MethodRef missing: {fullName}");
    return x;
}

GenericInstanceMethod OldGeneric(string declaringType, string genericArg)
{
    var x = oldRefs.OfType<GenericInstanceMethod>().FirstOrDefault(m =>
        m.Name == "GetComponent" && m.ElementMethod.DeclaringType.FullName == declaringType &&
        m.GenericArguments.Count == 1 && m.GenericArguments[0].FullName == genericArg);
    if (x is null) throw new InvalidDataException($"required generic MethodRef missing: {declaringType}.GetComponent<{genericArg}>");
    return x;
}

FieldDefinition Field(string name)
{
    var f = target.DeclaringType.Fields.SingleOrDefault(x => x.Name == name);
    if (f is null) throw new InvalidDataException($"PlantDetail field missing: {name}");
    return f;
}

var dataControllerType = AllTypes(module.Types).Single(t => t.FullName == "SeedChooserScreen/PlantDetail/DataUIController");
var dataGameObjectField = dataControllerType.Fields.Single(f => f.Name == "gameObject");

var fCard = Field("card_Choose");
var fInfo = Field("infoButtons");
var fPlantName = Field("plantName");
var fData = Field("dataUIControllers");
var fSkillText = Field("skillText");
var fSkillButton = Field("skillButton");
var fSkillBank = Field("skillBank");
var fCharacteristicText = Field("characteristicText");
var fTalentText = Field("talentText");
var fAlmanac = Field("button_Almanac");

var setActive = OldRef("System.Void UnityEngine.GameObject::SetActive(System.Boolean)");
var setText = OldRef("System.Void TMPro.TMP_Text::SetText(System.String,System.Boolean)");
var componentGameObject = OldRef("UnityEngine.GameObject UnityEngine.Component::get_gameObject()");
var getTransform = OldRef("UnityEngine.Transform UnityEngine.GameObject::get_transform()");
var getChild = OldRef("UnityEngine.Transform UnityEngine.Transform::GetChild(System.Int32)");
var setSprite = OldRef("System.Void UnityEngine.UI.Image::set_sprite(UnityEngine.Sprite)");
var setEnabled = OldRef("System.Void UnityEngine.Behaviour::set_enabled(System.Boolean)");
var objectEquality = OldRef("System.Boolean UnityEngine.Object::op_Equality(UnityEngine.Object,UnityEngine.Object)");
var loadSkillLogo = OldRef("UnityEngine.Sprite ResourceManager::LoadSkillLogo(System.Int32,System.String)");
var nreCtor = OldRef("System.Void System.NullReferenceException::.ctor()");
var getImage = OldGeneric("UnityEngine.Component", "UnityEngine.UI.Image");
var getButton = OldGeneric("UnityEngine.GameObject", "UnityEngine.UI.Button");
var loadPlantData = target.DeclaringType.Methods.Single(m => m.Name == "LoadPlantData" && m.Parameters.Count == 0);

MethodReference ListCount(TypeReference listType)
{
    return new MethodReference("get_Count", module.TypeSystem.Int32, module.ImportReference(listType))
    {
        HasThis = true,
        ExplicitThis = false,
        CallingConvention = MethodCallingConvention.Default
    };
}

MethodReference ListItem(TypeReference listType)
{
    if (listType is not GenericInstanceType gi || gi.GenericArguments.Count != 1)
        throw new InvalidDataException($"not List<T>: {listType.FullName}");
    var mr = new MethodReference("get_Item", module.ImportReference(gi.GenericArguments[0]), module.ImportReference(listType))
    {
        HasThis = true,
        ExplicitThis = false,
        CallingConvention = MethodCallingConvention.Default
    };
    mr.Parameters.Add(new ParameterDefinition(module.TypeSystem.Int32));
    return mr;
}

var dataCount = ListCount(fData.FieldType);
var dataItem = ListItem(fData.FieldType);
var infoCount = ListCount(fInfo.FieldType);
var infoItem = ListItem(fInfo.FieldType);
var skillItem = ListItem(fSkillButton.FieldType);

var body = target.Body;
body.ExceptionHandlers.Clear();
body.Instructions.Clear();
body.Variables.Clear();
body.InitLocals = true;
body.MaxStackSize = 8;

TypeReference Elem(TypeReference listType) => module.ImportReference(((GenericInstanceType)listType).GenericArguments[0]);

var vI = new VariableDefinition(module.TypeSystem.Int32);
var vDataList = new VariableDefinition(module.ImportReference(fData.FieldType));
var vInfoList = new VariableDefinition(module.ImportReference(fInfo.FieldType));
var vSkillList = new VariableDefinition(module.ImportReference(fSkillButton.FieldType));
var vData = new VariableDefinition(Elem(fData.FieldType));
var vInfoButton = new VariableDefinition(Elem(fInfo.FieldType));
var vGo = new VariableDefinition(module.ImportReference(fAlmanac.FieldType));
var vText = new VariableDefinition(module.ImportReference(fPlantName.FieldType));
var vTransform = new VariableDefinition(module.ImportReference(getTransform.ReturnType));
var vChild = new VariableDefinition(module.ImportReference(getChild.ReturnType));
var vImage = new VariableDefinition(module.ImportReference(getImage.GenericArguments[0]));
var vSprite = new VariableDefinition(module.ImportReference(loadSkillLogo.ReturnType));
var vButton = new VariableDefinition(module.ImportReference(getButton.GenericArguments[0]));
foreach (var v in new[] { vI, vDataList, vInfoList, vSkillList, vData, vInfoButton, vGo, vText, vTransform, vChild, vImage, vSprite, vButton }) body.Variables.Add(v);

var il = body.GetILProcessor();

void Require(VariableDefinition v)
{
    var ok = Instruction.Create(OpCodes.Nop);
    il.Emit(OpCodes.Ldloc, v);
    il.Emit(OpCodes.Brtrue, ok);
    il.Emit(OpCodes.Newobj, nreCtor);
    il.Emit(OpCodes.Throw);
    il.Append(ok);
}

void LoadPlantField(FieldDefinition f, VariableDefinition v)
{
    il.Emit(OpCodes.Ldarg_0);
    il.Emit(OpCodes.Ldfld, f);
    il.Emit(OpCodes.Stloc, v);
    Require(v);
}

void SetGoActive(bool active)
{
    il.Emit(OpCodes.Ldloc, vGo);
    il.Emit(active ? OpCodes.Ldc_I4_1 : OpCodes.Ldc_I4_0);
    il.Emit(OpCodes.Call, setActive);
}

void DataLoop(bool active)
{
    LoadPlantField(fData, vDataList);
    il.Emit(OpCodes.Ldc_I4_0);
    il.Emit(OpCodes.Stloc, vI);
    var bodyLabel = Instruction.Create(OpCodes.Nop);
    var condLabel = Instruction.Create(OpCodes.Nop);
    il.Emit(OpCodes.Br, condLabel);
    il.Append(bodyLabel);
    il.Emit(OpCodes.Ldloc, vDataList);
    il.Emit(OpCodes.Ldloc, vI);
    il.Emit(OpCodes.Callvirt, dataItem);
    il.Emit(OpCodes.Stloc, vData);
    Require(vData);
    il.Emit(OpCodes.Ldloc, vData);
    il.Emit(OpCodes.Ldfld, dataGameObjectField);
    il.Emit(OpCodes.Stloc, vGo);
    Require(vGo);
    SetGoActive(active);
    il.Emit(OpCodes.Ldloc, vI);
    il.Emit(OpCodes.Ldc_I4_1);
    il.Emit(OpCodes.Add);
    il.Emit(OpCodes.Stloc, vI);
    il.Append(condLabel);
    il.Emit(OpCodes.Ldloc, vI);
    il.Emit(OpCodes.Ldloc, vDataList);
    il.Emit(OpCodes.Callvirt, dataCount);
    il.Emit(OpCodes.Blt, bodyLabel);
}

void InfoLoop(bool active)
{
    LoadPlantField(fInfo, vInfoList);
    il.Emit(OpCodes.Ldc_I4_0);
    il.Emit(OpCodes.Stloc, vI);
    var bodyLabel = Instruction.Create(OpCodes.Nop);
    var condLabel = Instruction.Create(OpCodes.Nop);
    il.Emit(OpCodes.Br, condLabel);
    il.Append(bodyLabel);
    il.Emit(OpCodes.Ldloc, vInfoList);
    il.Emit(OpCodes.Ldloc, vI);
    il.Emit(OpCodes.Callvirt, infoItem);
    il.Emit(OpCodes.Stloc, vInfoButton);
    Require(vInfoButton);
    il.Emit(OpCodes.Ldloc, vInfoButton);
    il.Emit(OpCodes.Call, componentGameObject);
    il.Emit(OpCodes.Stloc, vGo);
    Require(vGo);
    SetGoActive(active);
    il.Emit(OpCodes.Ldloc, vI);
    il.Emit(OpCodes.Ldc_I4_1);
    il.Emit(OpCodes.Add);
    il.Emit(OpCodes.Stloc, vI);
    il.Append(condLabel);
    il.Emit(OpCodes.Ldloc, vI);
    il.Emit(OpCodes.Ldloc, vInfoList);
    il.Emit(OpCodes.Callvirt, infoCount);
    il.Emit(OpCodes.Blt, bodyLabel);
}

void FieldGoSet(FieldDefinition f, bool active)
{
    LoadPlantField(f, vGo);
    SetGoActive(active);
}

void ReloadSkillButton()
{
    LoadPlantField(fSkillButton, vSkillList);
    il.Emit(OpCodes.Ldloc, vSkillList);
    il.Emit(OpCodes.Ldloc, vI);
    il.Emit(OpCodes.Callvirt, skillItem);
    il.Emit(OpCodes.Stloc, vGo);
    Require(vGo);
}

void SkillChildToGameObject(int childIndex)
{
    il.Emit(OpCodes.Ldloc, vGo);
    il.Emit(OpCodes.Call, getTransform);
    il.Emit(OpCodes.Stloc, vTransform);
    Require(vTransform);
    il.Emit(OpCodes.Ldloc, vTransform);
    il.Emit(OpCodes.Ldc_I4, childIndex);
    il.Emit(OpCodes.Call, getChild);
    il.Emit(OpCodes.Stloc, vChild);
    Require(vChild);
    il.Emit(OpCodes.Ldloc, vChild);
    il.Emit(OpCodes.Call, componentGameObject);
    il.Emit(OpCodes.Stloc, vGo);
    Require(vGo);
}

// Native-backed body begins.
DataLoop(false);

LoadPlantField(fPlantName, vText);
il.Emit(OpCodes.Ldloc, vText);
il.Emit(OpCodes.Ldstr, "NaN");
il.Emit(OpCodes.Ldc_I4_1);
il.Emit(OpCodes.Call, setText);

InfoLoop(false);

il.Emit(OpCodes.Ldarg_0);
il.Emit(OpCodes.Ldarg_1);
il.Emit(OpCodes.Stfld, fCard);

// PC native loads button_Almanac before testing the Unity-object null result.
il.Emit(OpCodes.Ldarg_0);
il.Emit(OpCodes.Ldfld, fAlmanac);
il.Emit(OpCodes.Stloc, vGo);
il.Emit(OpCodes.Ldarg_1);
il.Emit(OpCodes.Ldnull);
il.Emit(OpCodes.Call, objectEquality);
var nullCard = Instruction.Create(OpCodes.Nop);
il.Emit(OpCodes.Brtrue, nullCard);

// card_Choose != null
Require(vGo);
SetGoActive(true);
il.Emit(OpCodes.Ldarg_0);
il.Emit(OpCodes.Call, loadPlantData);
DataLoop(true);
InfoLoop(true);
il.Emit(OpCodes.Ret);

// card_Choose == null
il.Append(nullCard);
Require(vGo);
SetGoActive(false);
FieldGoSet(fSkillText, false);
FieldGoSet(fSkillBank, false);
FieldGoSet(fCharacteristicText, false);
FieldGoSet(fTalentText, false);

il.Emit(OpCodes.Ldc_I4_0);
il.Emit(OpCodes.Stloc, vI);
var skillBody = Instruction.Create(OpCodes.Nop);
var skillCond = Instruction.Create(OpCodes.Nop);
il.Emit(OpCodes.Br, skillCond);
il.Append(skillBody);

// skillButton[i] -> transform -> child 4 -> Image; then load Lock sprite; then assign.
ReloadSkillButton();
il.Emit(OpCodes.Ldloc, vGo);
il.Emit(OpCodes.Call, getTransform);
il.Emit(OpCodes.Stloc, vTransform);
Require(vTransform);
il.Emit(OpCodes.Ldloc, vTransform);
il.Emit(OpCodes.Ldc_I4_4);
il.Emit(OpCodes.Call, getChild);
il.Emit(OpCodes.Stloc, vChild);
Require(vChild);
il.Emit(OpCodes.Ldloc, vChild);
il.Emit(OpCodes.Call, getImage);
il.Emit(OpCodes.Stloc, vImage);
il.Emit(OpCodes.Ldc_I4_0);
il.Emit(OpCodes.Ldstr, "Lock");
il.Emit(OpCodes.Call, loadSkillLogo);
il.Emit(OpCodes.Stloc, vSprite);
Require(vImage);
il.Emit(OpCodes.Ldloc, vImage);
il.Emit(OpCodes.Ldloc, vSprite);
il.Emit(OpCodes.Call, setSprite);

// Reload exactly as native does before each independent UI operation.
ReloadSkillButton();
il.Emit(OpCodes.Ldloc, vGo);
il.Emit(OpCodes.Call, getButton);
il.Emit(OpCodes.Stloc, vButton);
Require(vButton);
il.Emit(OpCodes.Ldloc, vButton);
il.Emit(OpCodes.Ldc_I4_0);
il.Emit(OpCodes.Call, setEnabled);

ReloadSkillButton();
SkillChildToGameObject(2);
SetGoActive(true);

ReloadSkillButton();
SkillChildToGameObject(5);
SetGoActive(false);

il.Emit(OpCodes.Ldloc, vI);
il.Emit(OpCodes.Ldc_I4_1);
il.Emit(OpCodes.Add);
il.Emit(OpCodes.Stloc, vI);
il.Append(skillCond);
il.Emit(OpCodes.Ldloc, vI);
il.Emit(OpCodes.Ldc_I4_3);
il.Emit(OpCodes.Blt, skillBody);
il.Emit(OpCodes.Ret);

Console.WriteLine("PATCH PlantDetail.Awake native_backed pc=0x1803A1C60 android=0x13E0A88 skill_iterations=3 child4_lock=1 child2_on=1 child5_off=1");

module.Write(output);
var outputSha = Convert.ToHexString(SHA256.HashData(File.ReadAllBytes(output))).ToLowerInvariant();
Console.WriteLine($"OUTPUT_SHA256 {outputSha}");

using var reopen = ModuleDefinition.ReadModule(output, new ReaderParameters { InMemory = true, ReadingMode = ReadingMode.Immediate });
var allAfter = AllTypes(reopen.Types).SelectMany(t => t.Methods).ToList();
if (allAfter.Count != ExpectedMethodDefCount) throw new InvalidDataException($"reopen MethodDef count drifted: {allAfter.Count}");
var rTarget = reopen.LookupToken(new MetadataToken(TokenType.Method, (int)(TargetToken & 0x00FFFFFF))) as MethodDefinition
    ?? throw new InvalidDataException("reopen PlantDetail.Awake missing");
if (!rTarget.HasBody || rTarget.Body.ExceptionHandlers.Count != 0) throw new InvalidDataException("reopen target body/EH drift");

int helperCalls = rTarget.Body.Instructions.Count(i => i.Operand is MethodReference m && m.DeclaringType.FullName == "Cpp2ILInjected.Cpp2ILHelpers");
int loadDataCalls = rTarget.Body.Instructions.Count(i => i.Operand is MethodReference m && m.Name == "LoadPlantData" && m.DeclaringType.FullName == "SeedChooserScreen/PlantDetail");
int lockStrings = rTarget.Body.Instructions.Count(i => i.OpCode.Code == Code.Ldstr && (string?)i.Operand == "Lock");
int nanStrings = rTarget.Body.Instructions.Count(i => i.OpCode.Code == Code.Ldstr && (string?)i.Operand == "NaN");
int loadLogoCalls = rTarget.Body.Instructions.Count(i => i.Operand is MethodReference m && m.Name == "LoadSkillLogo" && m.DeclaringType.FullName == "ResourceManager");
int objectEqCalls = rTarget.Body.Instructions.Count(i => i.Operand is MethodReference m && m.Name == "op_Equality" && m.DeclaringType.FullName == "UnityEngine.Object");
int cardStores = rTarget.Body.Instructions.Count(i => i.OpCode.Code == Code.Stfld && i.Operand is FieldReference f && f.Name == "card_Choose" && f.DeclaringType.FullName == "SeedChooserScreen/PlantDetail");
int oldBadNullCompares = 0;
for (int i = 0; i + 1 < rTarget.Body.Instructions.Count; i++)
{
    var a = rTarget.Body.Instructions[i];
    var b = rTarget.Body.Instructions[i + 1];
    if ((a.OpCode.Code == Code.Ldc_I4 || a.OpCode.Code == Code.Ldc_I4_0) && b.OpCode.Code == Code.Ceq) oldBadNullCompares++;
}
if (helperCalls != 0 || loadDataCalls != 1 || lockStrings != 1 || nanStrings != 1 || loadLogoCalls != 1 || objectEqCalls != 1 || cardStores != 1 || oldBadNullCompares != 0)
    throw new InvalidDataException($"reopen target validation failed helpers={helperCalls} loadData={loadDataCalls} lock={lockStrings} nan={nanStrings} logo={loadLogoCalls} objectEq={objectEqCalls} cardStores={cardStores} oldBadNull={oldBadNullCompares}");

var untouchedAfter = allAfter.Where(m => Raw(m) != TargetToken).ToDictionary(Raw, MethodSemantic);
if (untouchedAfter.Count != untouchedBefore.Count) throw new InvalidDataException("untouched method count drift");
var drift = untouchedBefore.Where(kv => !untouchedAfter.TryGetValue(kv.Key, out var a) || a != kv.Value).Select(kv => kv.Key).ToList();
if (drift.Count != 0) throw new InvalidDataException("semantic drift outside PlantDetail.Awake: " + string.Join(',', drift.Select(t => $"0x{t:X8}")));

Console.WriteLine($"REOPEN_PLANTDETAIL_AWAKE_PASS helpers={helperCalls} loadData={loadDataCalls} lock={lockStrings} nan={nanStrings} logo={loadLogoCalls} objectEq={objectEqCalls} cardStores={cardStores} locals={rTarget.Body.Variables.Count}");
Console.WriteLine($"SEMANTIC_ISOLATION_PASS untouched_methods={untouchedAfter.Count} changed_methods=1");
return 0;
