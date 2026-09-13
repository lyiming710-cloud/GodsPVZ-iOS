using Mono.Cecil;
using Mono.Cecil.Cil;
using System.Security.Cryptography;
using System.Text;

if (args.Length != 2)
{
    Console.Error.WriteLine("Usage: GodsPVZ.Stage9CardChooseInitializePatch <input.dll> <output.dll>");
    return 2;
}

const string ExpectedInputSha256 = "4a2e07d7813f6b56cd125a212c25c6998a36fe9312f763814a786ce218a1b1ab";
const int ExpectedMethodDefCount = 2317;
const uint ExpectedToken = 0x060002F2;

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

var target = methods.SingleOrDefault(m =>
        m.DeclaringType.FullName == "Card_Choose" &&
        m.Name == "Initialize" &&
        m.Parameters.Count == 1 &&
        m.Parameters[0].ParameterType.FullName == "PlantSave")
    ?? throw new InvalidDataException("Card_Choose.Initialize(PlantSave) target missing or ambiguous");
var token = Raw(target);
Console.WriteLine($"TARGET_METHOD token=0x{token:X8} name={target.FullName}");
if (token != ExpectedToken) throw new InvalidDataException($"target token drifted: 0x{token:X8}");
if (!target.HasBody || target.Body.CodeSize < 1700) throw new InvalidDataException($"expected corrupt large body missing: codeSize={target.Body.CodeSize}");
if (target.Body.Variables.Count < 60) throw new InvalidDataException($"expected corrupt local explosion missing: locals={target.Body.Variables.Count}");
if (!target.Body.Variables.Any(v => v.Index == 8 && v.VariableType.FullName == "System.Object")) throw new InvalidDataException("expected corrupt local 8:System.Object missing");
if (!target.Body.Instructions.Any(i => i.OpCode.Code == Code.Stloc && i.Operand is VariableDefinition v && v.Index == 8))
    throw new InvalidDataException("expected corrupt stloc local8 signature missing");

TypeDefinition TD(string name) => types.SingleOrDefault(t => t.FullName == name) ?? throw new InvalidDataException($"type {name} missing");
FieldDefinition FD(TypeDefinition t, string name) => t.Fields.SingleOrDefault(f => f.Name == name) ?? throw new InvalidDataException($"field {t.FullName}.{name} missing");

var cardType = target.DeclaringType;
var plantType = TD("PlantSave");
var skillType = TD("Skill");
var resourceType = TD("ResourceManager");

var cardPlantSave = FD(cardType, "plantSave");
var cardPlantType = FD(cardType, "plantType");
var cardPlantID = FD(cardType, "plantID");
var cardLevel = FD(cardType, "level");
var cardLevelOrder = FD(cardType, "levelOrder");
var cardSunPrice = FD(cardType, "sunPrice");
var cardCD = FD(cardType, "CD");
var cardStars = FD(cardType, "stars");
var cardOrdersImage = FD(cardType, "ordersImage");
var cardStarsImage = FD(cardType, "starsImage");
var cardLevelText = FD(cardType, "levelText");
var cardCliqueLogo = FD(cardType, "cliqueLogo");
var cardPlantPortrait = FD(cardType, "plantPortrait");
var cardDataBackground = FD(cardType, "dataBackground");
var cardLv = FD(cardType, "Lv");
var cardBackground = FD(cardType, "background");
var cardForeground = FD(cardType, "foreground");
var cardOrderArabesques = FD(cardType, "orderArabesques");
var cardSunPriceText = FD(cardType, "sunPriceText");
var cardSkill = FD(cardType, "skill");
var cardSkillLogo = FD(cardType, "skillLogo");

var plantPlantType = FD(plantType, "plantType");
var plantStars = FD(plantType, "stars");
var plantID = FD(plantType, "ID");
var plantClique = FD(plantType, "clique");
var plantOrder = FD(plantType, "order");
var plantLevel = FD(plantType, "level");
var plantSunPrice = FD(plantType, "sunPrice");
var plantReplantCD = FD(plantType, "replantCD");
var plantSkill1 = FD(plantType, "skill1");
var plantSkill2 = FD(plantType, "skill2");
var plantSkill3 = FD(plantType, "skill3");
var plantDefaultSkillID = FD(plantType, "defaultSkillID");

var skillID = FD(skillType, "ID");
var skillName = FD(skillType, "name");

var resCliqueLogos = FD(resourceType, "cliqueLogos");
var resPlantPortraits = FD(resourceType, "card_Choose_PlantPortraits");
var resData = FD(resourceType, "card_Choose_Data");
var resLvBackgrounds = FD(resourceType, "card_Choose_LvBackgrounds");
var resInnerlining = FD(resourceType, "card_Choose_Innerlining");
var resOuterlining = FD(resourceType, "card_Choose_Outerlining");
var resOrderArabesques = FD(resourceType, "card_Choose_OrderArabesques");

var targetOperands = target.Body.Instructions.Select(i => i.Operand).ToList();
var allOperands = methods.Where(m => m.HasBody).SelectMany(m => m.Body.Instructions).Select(i => i.Operand).ToList();

MethodReference FindMethod(IEnumerable<object?> operands, string declaringType, string name, params string[] parameterTypes)
{
    var matches = operands.OfType<MethodReference>().Where(m =>
        m.DeclaringType.FullName == declaringType &&
        m.Name == name &&
        m.Parameters.Count == parameterTypes.Length &&
        m.Parameters.Select(p => p.ParameterType.FullName).SequenceEqual(parameterTypes)).ToList();
    if (matches.Count == 0) throw new InvalidDataException($"MethodRef missing: {declaringType}::{name}({string.Join(',', parameterTypes)})");
    return matches[0];
}

MethodReference TargetMethod(string declaringType, string name, params string[] parameterTypes) =>
    FindMethod(targetOperands, declaringType, name, parameterTypes);

var componentGetGameObject = TargetMethod("UnityEngine.Component", "get_gameObject");
var setActive = TargetMethod("UnityEngine.GameObject", "SetActive", "System.Boolean");
var imageSetSprite = TargetMethod("UnityEngine.UI.Image", "set_sprite", "UnityEngine.Sprite");
var tmpSetColor = TargetMethod("TMPro.TMP_Text", "set_color", "UnityEngine.Color");
var tmpSetText = TargetMethod("TMPro.TMP_Text", "set_text", "System.String");
var intToString = TargetMethod("System.Int32", "ToString");
var skillCopy = TargetMethod("Skill", "Copy");
var getSkillId = TargetMethod("PlantSave", "GetSkill_ID", "System.Int32");
var loadSkillLogo = TargetMethod("ResourceManager", "LoadSkillLogo", "System.Int32", "System.String");

var colorCtor = allOperands.OfType<MethodReference>().FirstOrDefault(m =>
    m.DeclaringType.FullName == "UnityEngine.Color" && m.Name == ".ctor" &&
    m.Parameters.Count == 4 && m.Parameters.All(p => p.ParameterType.FullName == "System.Single"));
if (colorCtor is null)
{
    var colorType = tmpSetColor.Parameters[0].ParameterType;
    colorCtor = new MethodReference(".ctor", module.TypeSystem.Void, colorType) { HasThis = true };
    for (var i = 0; i < 4; i++) colorCtor.Parameters.Add(new ParameterDefinition(module.TypeSystem.Single));
}

MethodReference ListGetItem(FieldDefinition field)
{
    var t = field.FieldType as GenericInstanceType ?? throw new InvalidDataException($"{field.FullName} is not generic List<T>");
    if (t.ElementType.GenericParameters.Count != 1) throw new InvalidDataException($"{field.FullName} List<T> generic parameter drift");
    var gp = t.ElementType.GenericParameters.Single();
    var m = new MethodReference("get_Item", gp, t) { HasThis = true };
    m.Parameters.Add(new ParameterDefinition(module.TypeSystem.Int32));
    return m;
}

var cliqueGetItem = ListGetItem(resCliqueLogos);
var portraitGetItem = ListGetItem(resPlantPortraits);
var changed = new HashSet<uint> { token };
var untouchedBefore = methods.Where(m => !changed.Contains(Raw(m))).ToDictionary(Raw, MethodSemantic);

var body = target.Body;
body.Instructions.Clear();
body.ExceptionHandlers.Clear();
body.Variables.Clear();
body.InitLocals = true;
body.MaxStackSize = 6;
var iLocal = new VariableDefinition(module.TypeSystem.Int32);
var firstCopyLocal = new VariableDefinition(skillType);
var resolvedSkillLocal = new VariableDefinition(skillType);
body.Variables.Add(iLocal);
body.Variables.Add(firstCopyLocal);
body.Variables.Add(resolvedSkillLocal);
var il = body.GetILProcessor();

// PC native: MethodDef 0x060002F2, pointer index 753, VA 0x180343BE0..0x1803442BF.
il.Append(il.Create(OpCodes.Ldarg_0));
il.Append(il.Create(OpCodes.Ldarg_1));
il.Append(il.Create(OpCodes.Stfld, cardPlantSave));

void CopyField(FieldDefinition src, FieldDefinition dst)
{
    il.Append(il.Create(OpCodes.Ldarg_0));
    il.Append(il.Create(OpCodes.Ldarg_1));
    il.Append(il.Create(OpCodes.Ldfld, src));
    il.Append(il.Create(OpCodes.Stfld, dst));
}
CopyField(plantPlantType, cardPlantType);
CopyField(plantID, cardPlantID);
CopyField(plantLevel, cardLevel);
CopyField(plantOrder, cardLevelOrder);

var ordersCheck = il.Create(OpCodes.Ldloc, iLocal);
var ordersDone = il.Create(OpCodes.Nop);
il.Append(il.Create(OpCodes.Ldc_I4_0));
il.Append(il.Create(OpCodes.Stloc, iLocal));
il.Append(ordersCheck);
il.Append(il.Create(OpCodes.Ldarg_0));
il.Append(il.Create(OpCodes.Ldfld, cardOrdersImage));
il.Append(il.Create(OpCodes.Ldlen));
il.Append(il.Create(OpCodes.Conv_I4));
il.Append(il.Create(OpCodes.Bge, ordersDone));
il.Append(il.Create(OpCodes.Ldarg_0));
il.Append(il.Create(OpCodes.Ldfld, cardOrdersImage));
il.Append(il.Create(OpCodes.Ldloc, iLocal));
il.Append(il.Create(OpCodes.Ldelem_Ref));
il.Append(il.Create(OpCodes.Callvirt, componentGetGameObject));
il.Append(il.Create(OpCodes.Ldloc, iLocal));
il.Append(il.Create(OpCodes.Ldarg_0));
il.Append(il.Create(OpCodes.Ldfld, cardLevelOrder));
il.Append(il.Create(OpCodes.Clt));
il.Append(il.Create(OpCodes.Callvirt, setActive));
il.Append(il.Create(OpCodes.Ldloc, iLocal));
il.Append(il.Create(OpCodes.Ldc_I4_1));
il.Append(il.Create(OpCodes.Add));
il.Append(il.Create(OpCodes.Stloc, iLocal));
il.Append(il.Create(OpCodes.Br, ordersCheck));
il.Append(ordersDone);

CopyField(plantSunPrice, cardSunPrice);
CopyField(plantReplantCD, cardCD);
CopyField(plantStars, cardStars);

var starsCheck = il.Create(OpCodes.Ldloc, iLocal);
var starsDone = il.Create(OpCodes.Nop);
il.Append(il.Create(OpCodes.Ldc_I4_0));
il.Append(il.Create(OpCodes.Stloc, iLocal));
il.Append(starsCheck);
il.Append(il.Create(OpCodes.Ldarg_0));
il.Append(il.Create(OpCodes.Ldfld, cardStarsImage));
il.Append(il.Create(OpCodes.Ldlen));
il.Append(il.Create(OpCodes.Conv_I4));
il.Append(il.Create(OpCodes.Bge, starsDone));
il.Append(il.Create(OpCodes.Ldarg_0));
il.Append(il.Create(OpCodes.Ldfld, cardStarsImage));
il.Append(il.Create(OpCodes.Ldloc, iLocal));
il.Append(il.Create(OpCodes.Ldelem_Ref));
il.Append(il.Create(OpCodes.Callvirt, componentGetGameObject));
il.Append(il.Create(OpCodes.Ldloc, iLocal));
il.Append(il.Create(OpCodes.Ldarg_0));
il.Append(il.Create(OpCodes.Ldfld, cardStars));
il.Append(il.Create(OpCodes.Clt));
il.Append(il.Create(OpCodes.Callvirt, setActive));
il.Append(il.Create(OpCodes.Ldloc, iLocal));
il.Append(il.Create(OpCodes.Ldc_I4_1));
il.Append(il.Create(OpCodes.Add));
il.Append(il.Create(OpCodes.Stloc, iLocal));
il.Append(il.Create(OpCodes.Br, starsCheck));
il.Append(starsDone);

il.Append(il.Create(OpCodes.Ldarg_0));
il.Append(il.Create(OpCodes.Ldfld, cardLevelText));
il.Append(il.Create(OpCodes.Ldc_R4, 0.9f));
il.Append(il.Create(OpCodes.Ldc_R4, 0.9f));
il.Append(il.Create(OpCodes.Ldc_R4, 0.9f));
il.Append(il.Create(OpCodes.Ldc_R4, 1.0f));
il.Append(il.Create(OpCodes.Newobj, colorCtor));
il.Append(il.Create(OpCodes.Callvirt, tmpSetColor));

var cliqueOff = il.Create(OpCodes.Ldarg_0);
var cliqueDone = il.Create(OpCodes.Nop);
il.Append(il.Create(OpCodes.Ldarg_1));
il.Append(il.Create(OpCodes.Ldfld, plantClique));
il.Append(il.Create(OpCodes.Brfalse, cliqueOff));
il.Append(il.Create(OpCodes.Ldarg_0));
il.Append(il.Create(OpCodes.Ldfld, cardCliqueLogo));
il.Append(il.Create(OpCodes.Callvirt, componentGetGameObject));
il.Append(il.Create(OpCodes.Ldc_I4_1));
il.Append(il.Create(OpCodes.Callvirt, setActive));
il.Append(il.Create(OpCodes.Ldarg_0));
il.Append(il.Create(OpCodes.Ldfld, cardCliqueLogo));
il.Append(il.Create(OpCodes.Ldsfld, resCliqueLogos));
il.Append(il.Create(OpCodes.Ldarg_1));
il.Append(il.Create(OpCodes.Ldfld, plantClique));
il.Append(il.Create(OpCodes.Callvirt, cliqueGetItem));
il.Append(il.Create(OpCodes.Callvirt, imageSetSprite));
il.Append(il.Create(OpCodes.Br, cliqueDone));
il.Append(cliqueOff);
il.Append(il.Create(OpCodes.Ldfld, cardCliqueLogo));
il.Append(il.Create(OpCodes.Callvirt, componentGetGameObject));
il.Append(il.Create(OpCodes.Ldc_I4_0));
il.Append(il.Create(OpCodes.Callvirt, setActive));
il.Append(cliqueDone);

il.Append(il.Create(OpCodes.Ldarg_0));
il.Append(il.Create(OpCodes.Ldfld, cardPlantPortrait));
il.Append(il.Create(OpCodes.Ldsfld, resPlantPortraits));
il.Append(il.Create(OpCodes.Ldarg_0));
il.Append(il.Create(OpCodes.Ldfld, cardPlantID));
il.Append(il.Create(OpCodes.Callvirt, portraitGetItem));
il.Append(il.Create(OpCodes.Callvirt, imageSetSprite));

void SetSpriteFromArray(FieldDefinition imageField, FieldDefinition resourceArray, FieldDefinition indexField)
{
    il.Append(il.Create(OpCodes.Ldarg_0));
    il.Append(il.Create(OpCodes.Ldfld, imageField));
    il.Append(il.Create(OpCodes.Ldsfld, resourceArray));
    il.Append(il.Create(OpCodes.Ldarg_0));
    il.Append(il.Create(OpCodes.Ldfld, indexField));
    il.Append(il.Create(OpCodes.Ldelem_Ref));
    il.Append(il.Create(OpCodes.Callvirt, imageSetSprite));
}
SetSpriteFromArray(cardDataBackground, resData, cardStars);
SetSpriteFromArray(cardLv, resLvBackgrounds, cardStars);
SetSpriteFromArray(cardBackground, resInnerlining, cardStars);
SetSpriteFromArray(cardForeground, resOuterlining, cardStars);

var orderOff = il.Create(OpCodes.Ldarg_0);
var orderDone = il.Create(OpCodes.Nop);
il.Append(il.Create(OpCodes.Ldarg_0));
il.Append(il.Create(OpCodes.Ldfld, cardLevelOrder));
il.Append(il.Create(OpCodes.Ldc_I4_0));
il.Append(il.Create(OpCodes.Ble, orderOff));
SetSpriteFromArray(cardOrderArabesques, resOrderArabesques, cardLevelOrder);
il.Append(il.Create(OpCodes.Ldarg_0));
il.Append(il.Create(OpCodes.Ldfld, cardOrderArabesques));
il.Append(il.Create(OpCodes.Callvirt, componentGetGameObject));
il.Append(il.Create(OpCodes.Ldc_I4_1));
il.Append(il.Create(OpCodes.Callvirt, setActive));
il.Append(il.Create(OpCodes.Br, orderDone));
il.Append(orderOff);
il.Append(il.Create(OpCodes.Ldfld, cardOrderArabesques));
il.Append(il.Create(OpCodes.Callvirt, componentGetGameObject));
il.Append(il.Create(OpCodes.Ldc_I4_0));
il.Append(il.Create(OpCodes.Callvirt, setActive));
il.Append(orderDone);

il.Append(il.Create(OpCodes.Ldarg_0));
il.Append(il.Create(OpCodes.Ldfld, cardSunPriceText));
il.Append(il.Create(OpCodes.Ldarg_0));
il.Append(il.Create(OpCodes.Ldflda, cardSunPrice));
il.Append(il.Create(OpCodes.Call, intToString));
il.Append(il.Create(OpCodes.Callvirt, tmpSetText));
il.Append(il.Create(OpCodes.Ldarg_0));
il.Append(il.Create(OpCodes.Ldfld, cardLevelText));
il.Append(il.Create(OpCodes.Ldarg_0));
il.Append(il.Create(OpCodes.Ldflda, cardLevel));
il.Append(il.Create(OpCodes.Call, intToString));
il.Append(il.Create(OpCodes.Callvirt, tmpSetText));

var chooseSkill2 = il.Create(OpCodes.Ldarg_1);
var chooseSkill3 = il.Create(OpCodes.Ldarg_1);
var copySelected = il.Create(OpCodes.Callvirt, skillCopy);
il.Append(il.Create(OpCodes.Ldarg_1));
il.Append(il.Create(OpCodes.Ldfld, plantDefaultSkillID));
il.Append(il.Create(OpCodes.Ldc_I4_2));
il.Append(il.Create(OpCodes.Beq, chooseSkill2));
il.Append(il.Create(OpCodes.Ldarg_1));
il.Append(il.Create(OpCodes.Ldfld, plantDefaultSkillID));
il.Append(il.Create(OpCodes.Ldc_I4_3));
il.Append(il.Create(OpCodes.Beq, chooseSkill3));
il.Append(il.Create(OpCodes.Ldarg_1));
il.Append(il.Create(OpCodes.Ldfld, plantSkill1));
il.Append(il.Create(OpCodes.Br, copySelected));
il.Append(chooseSkill2);
il.Append(il.Create(OpCodes.Ldfld, plantSkill2));
il.Append(il.Create(OpCodes.Br, copySelected));
il.Append(chooseSkill3);
il.Append(il.Create(OpCodes.Ldfld, plantSkill3));
il.Append(copySelected);
il.Append(il.Create(OpCodes.Stloc, firstCopyLocal));

il.Append(il.Create(OpCodes.Ldarg_0));
il.Append(il.Create(OpCodes.Ldloc, firstCopyLocal));
il.Append(il.Create(OpCodes.Stfld, cardSkill));
il.Append(il.Create(OpCodes.Ldarg_0));
il.Append(il.Create(OpCodes.Ldarg_0));
il.Append(il.Create(OpCodes.Ldfld, cardSkill));
il.Append(il.Create(OpCodes.Callvirt, skillCopy));
il.Append(il.Create(OpCodes.Stfld, cardSkill));

il.Append(il.Create(OpCodes.Ldarg_1));
il.Append(il.Create(OpCodes.Ldloc, firstCopyLocal));
il.Append(il.Create(OpCodes.Ldfld, skillID));
il.Append(il.Create(OpCodes.Callvirt, getSkillId));
il.Append(il.Create(OpCodes.Stloc, resolvedSkillLocal));

il.Append(il.Create(OpCodes.Ldarg_0));
il.Append(il.Create(OpCodes.Ldfld, cardSkillLogo));
il.Append(il.Create(OpCodes.Ldarg_1));
il.Append(il.Create(OpCodes.Ldfld, plantID));
il.Append(il.Create(OpCodes.Ldc_I4_3));
il.Append(il.Create(OpCodes.Mul));
il.Append(il.Create(OpCodes.Ldloc, resolvedSkillLocal));
il.Append(il.Create(OpCodes.Ldfld, skillID));
il.Append(il.Create(OpCodes.Add));
il.Append(il.Create(OpCodes.Ldloc, resolvedSkillLocal));
il.Append(il.Create(OpCodes.Ldfld, skillName));
il.Append(il.Create(OpCodes.Call, loadSkillLogo));
il.Append(il.Create(OpCodes.Callvirt, imageSetSprite));

il.Append(il.Create(OpCodes.Ldarg_0));
il.Append(il.Create(OpCodes.Ldfld, cardSkillLogo));
il.Append(il.Create(OpCodes.Callvirt, componentGetGameObject));
il.Append(il.Create(OpCodes.Ldc_I4_1));
il.Append(il.Create(OpCodes.Callvirt, setActive));
il.Append(il.Create(OpCodes.Ret));

Console.WriteLine("NATIVE_AUTHORITY token=0x060002F2 index=753 address=0x0000000180343BE0 function_end=0x00000001803442C0 gameassembly_sha256=9ebd7ca996a5b03fb4a766f7a2502b660d581ddbbb36af4d7bf2f06a211da39d metadata_sha256=ad992341add498bd1018ac980f42171e0737b0793bf945c8714ad70a445b36b9");
Console.WriteLine("PATCH_CARD_CHOOSE_INITIALIZE copy_fields=7 orders_loop=1 stars_loop=1 level_color_0_9=1 clique=1 portrait=1 card_shells=4 order_arabesques=1 texts=2 default_skill_switch=1 double_copy=1 get_skill_id=1 load_skill_logo=1 target_only=1");

module.Write(output);
var outputSha = Convert.ToHexString(SHA256.HashData(File.ReadAllBytes(output))).ToLowerInvariant();
Console.WriteLine($"OUTPUT_SHA256 {outputSha}");

using var reopen = ModuleDefinition.ReadModule(output, new ReaderParameters { InMemory = true, ReadingMode = ReadingMode.Immediate });
var after = AllTypes(reopen.Types).SelectMany(t => t.Methods).ToList();
if (after.Count != ExpectedMethodDefCount) throw new InvalidDataException("reopen MethodDef count drift");
var rt = after.SingleOrDefault(m => Raw(m) == token) ?? throw new InvalidDataException("reopen target missing");
if (rt.DeclaringType.FullName != "Card_Choose" || rt.Name != "Initialize" || rt.Parameters.Count != 1 || rt.Parameters[0].ParameterType.FullName != "PlantSave")
    throw new InvalidDataException("reopen target signature drift");
if (rt.Body.Variables.Count != 3) throw new InvalidDataException($"Card_Choose.Initialize repaired locals drift: {rt.Body.Variables.Count}");
if (rt.Body.ExceptionHandlers.Count != 0) throw new InvalidDataException("unexpected exception handlers after repair");
if (rt.Body.Variables.Any(v => v.VariableType.FullName == "System.Object")) throw new InvalidDataException("object corruption local survived repair");

var calls = rt.Body.Instructions.Where(i => i.Operand is MethodReference).Select(i => (MethodReference)i.Operand).ToList();
if (calls.Count(m => m.DeclaringType.FullName == "Skill" && m.Name == "Copy" && m.Parameters.Count == 0) != 2)
    throw new InvalidDataException("native double Skill.Copy sequence missing");
if (calls.Count(m => m.DeclaringType.FullName == "PlantSave" && m.Name == "GetSkill_ID" && m.Parameters.Count == 1) != 1)
    throw new InvalidDataException("PlantSave.GetSkill_ID missing");
if (calls.Count(m => m.DeclaringType.FullName == "ResourceManager" && m.Name == "LoadSkillLogo" && m.Parameters.Count == 2) != 1)
    throw new InvalidDataException("ResourceManager.LoadSkillLogo missing");
if (calls.Count(m => m.DeclaringType.FullName == "TMPro.TMP_Text" && m.Name == "set_text") != 2)
    throw new InvalidDataException("expected two TMP text setters");
if (calls.Count(m => m.DeclaringType.FullName == "TMPro.TMP_Text" && m.Name == "set_color") != 1)
    throw new InvalidDataException("expected level color setter");
if (calls.Count(m => m.DeclaringType.FullName == "UnityEngine.GameObject" && m.Name == "SetActive") != 7)
    throw new InvalidDataException("expected seven SetActive sites");
if (calls.Count(m => m.DeclaringType.FullName == "UnityEngine.UI.Image" && m.Name == "set_sprite") != 8)
    throw new InvalidDataException("expected eight Image.set_sprite sites");
if (!rt.Body.Instructions.Any(i => i.OpCode.Code == Code.Ldc_R4 && i.Operand is float f && Math.Abs(f - 0.9f) < 0.0001f))
    throw new InvalidDataException("0.9 level color constant missing");
if (!rt.Body.Instructions.Any(i => i.OpCode.Code == Code.Ldc_I4_3)) throw new InvalidDataException("skill logo x3 index constant missing");
foreach (var f in new[] { cardPlantSave, cardPlantType, cardPlantID, cardLevel, cardLevelOrder, cardSunPrice, cardCD, cardStars, cardSkill, cardSkillLogo })
    if (!rt.Body.Instructions.Any(i => i.Operand is FieldReference fr && fr.FullName == f.FullName)) throw new InvalidDataException($"required field reference missing after reopen: {f.FullName}");
Console.WriteLine($"REOPEN_CARD_CHOOSE_INITIALIZE_PASS locals=3 object_locals=0 copy_calls=2 get_skill_id=1 load_skill_logo=1 set_active_sites=7 set_sprite_sites=8 texts=2 color=0.9 token=0x{token:X8}");

var untouchedAfter = after.Where(m => !changed.Contains(Raw(m))).ToDictionary(Raw, MethodSemantic);
if (untouchedAfter.Count != untouchedBefore.Count) throw new InvalidDataException("untouched method count drift");
var drift = untouchedBefore.Where(kv => !untouchedAfter.TryGetValue(kv.Key, out var sem) || sem != kv.Value).Select(kv => kv.Key).ToList();
if (drift.Count != 0) throw new InvalidDataException("semantic drift outside Card_Choose.Initialize: " + string.Join(',', drift.Select(t => $"0x{t:X8}")));
Console.WriteLine($"SEMANTIC_ISOLATION_PASS untouched_methods={untouchedAfter.Count} changed_methods=1");
return 0;
