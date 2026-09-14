using Mono.Cecil;
using Mono.Cecil.Cil;
using System.Globalization;
using System.Security.Cryptography;
using System.Text;

if (args.Length != 2)
{
    Console.Error.WriteLine("Usage: Stage9BoardPlantDetailLoadDataPatch <input.dll> <output.dll>");
    return 2;
}

const string ExpectedInputSha = "4110588321372dd0c388f69485bcbdca6f908136c377d65d8903fe657c1e5035";
const int ExpectedMethods = 2317;
const int ExpectedFields = 2802;
const uint TargetToken = 0x060005E1;
const int ExpectedRid = 1505;
const int ExpectedOldCodeSize = 1836;
const int ExpectedOldLocals = 80;
const string NativeSpanSha = "24d71c446fd08dfa73dc6551d29431b1fa4bb62af348e224e4adf60f7165a898";

var PreservationTokens = new HashSet<uint>
{
    0x0600067E, 0x06000285, 0x06000289, 0x0600028A, // gameplay Batch1
    0x060003BA,                                     // Plant::.ctor
    0x060003FD,                                     // Projectile::.ctor
    0x060003DD, 0x060003DE,                        // Projectile ResetData / BindTrack
    0x060001F3, 0x06000216,                        // ProjectileManager.Start / ResourceManager.Start
    0x06000667,                                     // Plant_DetaiPage.Initialize
    0x06000668                                      // Plant_DetaiPage.LoadSkillLogo
};

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
static IEnumerable<MethodReference> MethodRefs(IEnumerable<MethodDefinition> methods) => methods
    .Where(m => m.HasBody)
    .SelectMany(m => m.Body.Instructions)
    .Select(i => i.Operand)
    .OfType<MethodReference>();
static MethodReference FindRef(IEnumerable<MethodDefinition> methods, Func<MethodReference, bool> pred, string label)
{
    var hits = MethodRefs(methods).Where(pred)
        .GroupBy(m => m.FullName + "@" + ScopeName(m.DeclaringType), StringComparer.Ordinal)
        .Select(g => g.First()).ToList();
    if (hits.Count != 1) throw new InvalidDataException($"method reference lookup {label} count={hits.Count}: {string.Join(" | ", hits.Select(h => h.FullName))}");
    return hits[0];
}
static int CountCalls(MethodDefinition m, Func<MethodReference, bool> pred) => m.Body.Instructions.Count(i =>
    (i.OpCode == OpCodes.Call || i.OpCode == OpCodes.Callvirt || i.OpCode == OpCodes.Newobj) && i.Operand is MethodReference mr && pred(mr));

var input = Path.GetFullPath(args[0]);
var output = Path.GetFullPath(args[1]);
var inputSha = Sha(input);
Console.WriteLine($"INPUT_SHA256 {inputSha}");
if (inputSha != ExpectedInputSha) throw new InvalidDataException("input SHA mismatch");

var beforeM = new Dictionary<uint, string>();
var beforeF = new Dictionary<uint, string>();
var preservedBefore = new Dictionary<uint, string>();

using (var module = ModuleDefinition.ReadModule(input, new ReaderParameters { InMemory = true, ReadingMode = ReadingMode.Immediate }))
{
    var types = AllTypes(module.Types).ToList();
    var methods = types.SelectMany(t => t.Methods).ToList();
    var fields = types.SelectMany(t => t.Fields).ToList();
    if (methods.Count != ExpectedMethods || fields.Count != ExpectedFields)
        throw new InvalidDataException($"metadata count drift methods={methods.Count} fields={fields.Count}");
    if (module.AssemblyReferences.Any(a => a.Name == "System.Private.CoreLib"))
        throw new InvalidDataException("unexpected System.Private.CoreLib input ref");

    foreach (var m in methods) beforeM[Raw(m)] = MethodSemantic(m);
    foreach (var fld in fields) beforeF[Raw(fld)] = FieldSemantic(fld);
    foreach (var token in PreservationTokens)
        preservedBefore[token] = beforeM.TryGetValue(token, out var sem) ? sem : throw new InvalidDataException($"preservation token missing 0x{token:X8}");

    var dataT = types.Single(t => t.FullName == "Board_PlantDetail_PlantData");
    var detailT = types.Single(t => t.FullName == "Plant_DetaiPage");
    var plantT = types.Single(t => t.FullName == "Plant");
    var skillT = types.Single(t => t.FullName == "Skill");
    var resourceT = types.Single(t => t.FullName == "ResourceManager");
    var target = dataT.Methods.Single(m => m.Name == "LoadData" && m.Parameters.Count == 1 && m.Parameters[0].ParameterType.FullName == "Plant_DetaiPage");

    if (Raw(target) != TargetToken || target.MetadataToken.RID != ExpectedRid || target.Body.CodeSize != ExpectedOldCodeSize || target.Body.Variables.Count != ExpectedOldLocals)
        throw new InvalidDataException($"target fingerprint drift token=0x{Raw(target):X8} rid={target.MetadataToken.RID} size={target.Body.CodeSize} locals={target.Body.Variables.Count}");
    var badAdd = target.Body.Instructions.SingleOrDefault(i => i.Offset == 0x54 && i.OpCode == OpCodes.Add);
    if (badAdd is null) throw new InvalidDataException("expected IL_0054 add corruption missing");
    if (!target.Body.Instructions.Any(i => i.Offset == 0x4F && i.OpCode.Code == Code.Ldc_I4 && Convert.ToInt32(i.Operand, CultureInfo.InvariantCulture) == 140))
        throw new InvalidDataException("expected Plant+140 corruption missing");
    if (target.Body.Variables.Count(v => v.VariableType.FullName == "System.Object") < 3)
        throw new InvalidDataException("expected object arithmetic locals missing");
    if (!target.Body.Instructions.Any(i => i.Operand is FieldReference fr && fr.DeclaringType.FullName.StartsWith("System.Collections.Generic.List`1", StringComparison.Ordinal) && fr.Name == "_size"))
        throw new InvalidDataException("expected private List._size lift missing");

    FieldDefinition DF(string n) => dataT.Fields.Single(f => f.Name == n);
    FieldDefinition DetF(string n) => detailT.Fields.Single(f => f.Name == n);
    FieldDefinition PF(string n) => plantT.Fields.Single(f => f.Name == n);
    FieldDefinition SF(string n) => skillT.Fields.Single(f => f.Name == n);
    var dataPlant = DF("plant");
    var coverHP = DF("coverHP");
    var cliqueLogo = DF("cliqueLogo");
    var skillLogo = DF("skillLogo");
    var textLv = DF("textLv");
    var textHP = DF("textHP");
    var textATK = DF("textATK");
    var textARM = DF("textARM");
    var textDEF = DF("textDEF");
    var characteristic = DF("characteristic");
    var talentNamesUI = DF("talentNames");
    var talentDescriptionsUI = DF("talentDescriptions");
    var starsUI = DF("stars");
    var detailPlant = DetF("plant");
    var plantLevel = PF("level");
    var plantHP = PF("healthPoint");
    var plantMaxHP = PF("maxHealthPoint");
    var plantClique = PF("clique");
    var plantSkill = PF("skill");
    var plantId = PF("ID");
    var plantCharacteristic = PF("characteristicText");
    var plantTalentNames = PF("talentNames");
    var plantTalents = PF("talents");
    var plantStars = PF("stars");
    var skillId = SF("ID");
    var skillName = SF("name");
    var cliqueLogos = resourceT.Fields.Single(f => f.Name == "cliqueLogos");

    var objectImplicit = FindRef(methods, mr => mr.DeclaringType.FullName == "UnityEngine.Object" && mr.Name == "op_Implicit" && mr.Parameters.Count == 1, "UnityEngine.Object.op_Implicit");
    var setText = FindRef(methods, mr => mr.DeclaringType.FullName == "TMPro.TMP_Text" && mr.Name == "SetText" && mr.Parameters.Count == 2 && mr.Parameters[0].ParameterType.FullName == "System.String" && mr.Parameters[1].ParameterType.FullName == "System.Boolean", "TMP_Text.SetText(string,bool)");
    var setFillAmount = FindRef(methods, mr => mr.DeclaringType.FullName == "UnityEngine.UI.Image" && mr.Name == "set_fillAmount" && mr.Parameters.Count == 1, "Image.set_fillAmount");
    var setSprite = FindRef(methods, mr => mr.DeclaringType.FullName == "UnityEngine.UI.Image" && mr.Name == "set_sprite" && mr.Parameters.Count == 1, "Image.set_sprite");
    var mathMin = FindRef(methods, mr => mr.DeclaringType.FullName == "System.Math" && mr.Name == "Min" && mr.Parameters.Count == 2 && mr.Parameters.All(p => p.ParameterType.FullName == "System.Single"), "Math.Min(float,float)");
    var mathMax = FindRef(methods, mr => mr.DeclaringType.FullName == "System.Math" && mr.Name == "Max" && mr.Parameters.Count == 2 && mr.Parameters.All(p => p.ParameterType.FullName == "System.Single"), "Math.Max(float,float)");
    var truncate = FindRef(methods, mr => mr.DeclaringType.FullName == "System.Math" && mr.Name == "Truncate" && mr.Parameters.Count == 1 && mr.Parameters[0].ParameterType.FullName == "System.Double", "Math.Truncate(double)");
    var intToString = FindRef(methods, mr => mr.DeclaringType.FullName == "System.Int32" && mr.Name == "ToString" && mr.Parameters.Count == 0, "Int32.ToString");
    var doubleToString = FindRef(methods, mr => mr.DeclaringType.FullName == "System.Double" && mr.Name == "ToString" && mr.Parameters.Count == 0, "Double.ToString");
    var singleToString = FindRef(methods, mr => mr.DeclaringType.FullName == "System.Single" && mr.Name == "ToString" && mr.Parameters.Count == 0, "Single.ToString");
    var concat2 = FindRef(methods, mr => mr.DeclaringType.FullName == "System.String" && mr.Name == "Concat" && mr.Parameters.Count == 2 && mr.Parameters.All(p => p.ParameterType.FullName == "System.String"), "String.Concat(string,string)");
    var concat3 = FindRef(methods, mr => mr.DeclaringType.FullName == "System.String" && mr.Name == "Concat" && mr.Parameters.Count == 3 && mr.Parameters.All(p => p.ParameterType.FullName == "System.String"), "String.Concat(string,string,string)");
    var strNe = FindRef(methods, mr => mr.DeclaringType.FullName == "System.String" && mr.Name == "op_Inequality" && mr.Parameters.Count == 2, "String.op_Inequality");
    var getGameObject = FindRef(methods, mr => mr.DeclaringType.FullName == "UnityEngine.Component" && mr.Name == "get_gameObject" && mr.Parameters.Count == 0, "Component.get_gameObject");
    var setActive = FindRef(methods, mr => mr.DeclaringType.FullName == "UnityEngine.GameObject" && mr.Name == "SetActive" && mr.Parameters.Count == 1, "GameObject.SetActive");

    MethodReference ListRef(FieldReference listField, string name, string label) => FindRef(methods,
        mr => mr.DeclaringType.FullName == listField.FieldType.FullName && mr.Name == name && (name != "get_Item" || (mr.Parameters.Count == 1 && mr.Parameters[0].ParameterType.FullName == "System.Int32")), label);
    var cliqueGetItem = ListRef(cliqueLogos, "get_Item", "List<Sprite>.get_Item");
    var talentGetItem = ListRef(talentNamesUI, "get_Item", "List<TextMeshProUGUI>.get_Item");
    var starsGetItem = ListRef(starsUI, "get_Item", "List<Image>.get_Item");
    var starsCount = ListRef(starsUI, "get_Count", "List<Image>.get_Count");

    var getATK = plantT.Methods.Single(m => m.Name == "GetATK" && m.Parameters.Count == 0);
    var getARM = plantT.Methods.Single(m => m.Name == "GetARM" && m.Parameters.Count == 0);
    var getDEF = plantT.Methods.Single(m => m.Name == "GetDEF" && m.Parameters.Count == 0);
    var loadSkillData = dataT.Methods.Single(m => m.Name == "LoadSkillData" && m.Parameters.Count == 1 && m.Parameters[0].ParameterType.FullName == "Skill");
    var loadSkillLogo = resourceT.Methods.Single(m => m.Name == "LoadSkillLogo" && m.Parameters.Count == 2 && m.Parameters[0].ParameterType.FullName == "System.Int32" && m.Parameters[1].ParameterType.FullName == "System.String");

    Console.WriteLine($"NATIVE_AUTHORITY target_token=0x{TargetToken:X8} rid={ExpectedRid} method_pointer_entry=0x181B85C60 va=0x180398B90 next_method_va=0x1803992F0 span_bytes=1888 native_span_sha256={NativeSpanSha}");
    Console.WriteLine("NATIVE_SEMANTICS level=plant.level hp_fill_clamp=1 hp_text_truncate=1 atk_arm_def=1 clique_logo=1 skill_logo_id=skill.ID+3*plant.ID skill_data=1 characteristic=1 talents_exact3=1 stars_active_i_lt_plant.stars=1");
    Console.WriteLine("CLR_ACCESS_ADAPTATION pc_native_List_size_read=1 managed_List_Count=1 field_metadata_changed=0 list_mutation=0");

    var body = target.Body;
    body.Instructions.Clear();
    body.Variables.Clear();
    body.ExceptionHandlers.Clear();
    body.InitLocals = true;
    body.MaxStackSize = 8;

    var skillLocal = new VariableDefinition(skillT);
    var iLocal = new VariableDefinition(module.TypeSystem.Int32);
    var nameLocal = new VariableDefinition(module.TypeSystem.String);
    var d1 = new VariableDefinition(module.TypeSystem.Double);
    var d2 = new VariableDefinition(module.TypeSystem.Double);
    var s1 = new VariableDefinition(module.TypeSystem.String);
    var s2 = new VariableDefinition(module.TypeSystem.String);
    var f = new VariableDefinition(module.TypeSystem.Single);
    body.Variables.Add(skillLocal);
    body.Variables.Add(iLocal);
    body.Variables.Add(nameLocal);
    body.Variables.Add(d1);
    body.Variables.Add(d2);
    body.Variables.Add(s1);
    body.Variables.Add(s2);
    body.Variables.Add(f);

    var il = body.GetILProcessor();
    void A(Instruction x) => il.Append(x);
    var ret = il.Create(OpCodes.Ret);
    var afterCover = il.Create(OpCodes.Nop);
    var afterHP = il.Create(OpCodes.Nop);
    var afterATK = il.Create(OpCodes.Nop);
    var afterARM = il.Create(OpCodes.Nop);
    var afterDEF = il.Create(OpCodes.Nop);
    var afterClique = il.Create(OpCodes.Nop);
    var afterSkill = il.Create(OpCodes.Nop);
    var talentNoSuffix = il.Create(OpCodes.Nop);
    var talentCheck = il.Create(OpCodes.Nop);
    var talentBody = il.Create(OpCodes.Nop);
    var starsCheck = il.Create(OpCodes.Nop);
    var starsBody = il.Create(OpCodes.Nop);

    // if (!detaiPage.plant) return;
    A(il.Create(OpCodes.Ldarg_1));
    A(il.Create(OpCodes.Ldfld, detailPlant));
    A(il.Create(OpCodes.Call, objectImplicit));
    A(il.Create(OpCodes.Brfalse, ret));

    // this.plant = detaiPage.plant;
    A(il.Create(OpCodes.Ldarg_0));
    A(il.Create(OpCodes.Ldarg_1));
    A(il.Create(OpCodes.Ldfld, detailPlant));
    A(il.Create(OpCodes.Stfld, dataPlant));

    // textLv.SetText("Lv." + plant.level.ToString(), true);
    A(il.Create(OpCodes.Ldarg_0)); A(il.Create(OpCodes.Ldfld, textLv));
    A(il.Create(OpCodes.Ldstr, "Lv."));
    A(il.Create(OpCodes.Ldarg_0)); A(il.Create(OpCodes.Ldfld, dataPlant)); A(il.Create(OpCodes.Ldflda, plantLevel));
    A(il.Create(OpCodes.Call, intToString));
    A(il.Create(OpCodes.Call, concat2));
    A(il.Create(OpCodes.Ldc_I4_1));
    A(il.Create(OpCodes.Call, setText));

    // if (coverHP) coverHP.fillAmount = Max(Min(health/maxHealth, 1), 0);
    A(il.Create(OpCodes.Ldarg_0)); A(il.Create(OpCodes.Ldfld, coverHP)); A(il.Create(OpCodes.Call, objectImplicit)); A(il.Create(OpCodes.Brfalse, afterCover));
    A(il.Create(OpCodes.Ldarg_0)); A(il.Create(OpCodes.Ldfld, coverHP));
    A(il.Create(OpCodes.Ldarg_0)); A(il.Create(OpCodes.Ldfld, dataPlant)); A(il.Create(OpCodes.Ldfld, plantHP));
    A(il.Create(OpCodes.Ldarg_0)); A(il.Create(OpCodes.Ldfld, dataPlant)); A(il.Create(OpCodes.Ldfld, plantMaxHP));
    A(il.Create(OpCodes.Div)); A(il.Create(OpCodes.Ldc_R4, 1f)); A(il.Create(OpCodes.Call, mathMin)); A(il.Create(OpCodes.Ldc_R4, 0f)); A(il.Create(OpCodes.Call, mathMax));
    A(il.Create(OpCodes.Call, setFillAmount));
    A(afterCover);

    // if (textHP) textHP.SetText(Truncate(health).ToString()+"/"+Truncate(maxHealth).ToString(), true);
    A(il.Create(OpCodes.Ldarg_0)); A(il.Create(OpCodes.Ldfld, textHP)); A(il.Create(OpCodes.Call, objectImplicit)); A(il.Create(OpCodes.Brfalse, afterHP));
    A(il.Create(OpCodes.Ldarg_0)); A(il.Create(OpCodes.Ldfld, dataPlant)); A(il.Create(OpCodes.Ldfld, plantHP)); A(il.Create(OpCodes.Conv_R8)); A(il.Create(OpCodes.Call, truncate)); A(il.Create(OpCodes.Stloc, d1));
    A(il.Create(OpCodes.Ldloca, d1)); A(il.Create(OpCodes.Call, doubleToString)); A(il.Create(OpCodes.Stloc, s1));
    A(il.Create(OpCodes.Ldarg_0)); A(il.Create(OpCodes.Ldfld, dataPlant)); A(il.Create(OpCodes.Ldfld, plantMaxHP)); A(il.Create(OpCodes.Conv_R8)); A(il.Create(OpCodes.Call, truncate)); A(il.Create(OpCodes.Stloc, d2));
    A(il.Create(OpCodes.Ldloca, d2)); A(il.Create(OpCodes.Call, doubleToString)); A(il.Create(OpCodes.Stloc, s2));
    A(il.Create(OpCodes.Ldarg_0)); A(il.Create(OpCodes.Ldfld, textHP)); A(il.Create(OpCodes.Ldloc, s1)); A(il.Create(OpCodes.Ldstr, "/")); A(il.Create(OpCodes.Ldloc, s2)); A(il.Create(OpCodes.Call, concat3)); A(il.Create(OpCodes.Ldc_I4_1)); A(il.Create(OpCodes.Call, setText));
    A(afterHP);

    // ATK / ARM / DEF text.
    A(il.Create(OpCodes.Ldarg_0)); A(il.Create(OpCodes.Ldfld, textATK)); A(il.Create(OpCodes.Call, objectImplicit)); A(il.Create(OpCodes.Brfalse, afterATK));
    A(il.Create(OpCodes.Ldarg_0)); A(il.Create(OpCodes.Ldfld, dataPlant)); A(il.Create(OpCodes.Call, getATK)); A(il.Create(OpCodes.Stloc, f));
    A(il.Create(OpCodes.Ldarg_0)); A(il.Create(OpCodes.Ldfld, textATK)); A(il.Create(OpCodes.Ldstr, "攻击:")); A(il.Create(OpCodes.Ldloca, f)); A(il.Create(OpCodes.Call, singleToString)); A(il.Create(OpCodes.Call, concat2)); A(il.Create(OpCodes.Ldc_I4_1)); A(il.Create(OpCodes.Call, setText));
    A(afterATK);

    A(il.Create(OpCodes.Ldarg_0)); A(il.Create(OpCodes.Ldfld, textARM)); A(il.Create(OpCodes.Call, objectImplicit)); A(il.Create(OpCodes.Brfalse, afterARM));
    A(il.Create(OpCodes.Ldarg_0)); A(il.Create(OpCodes.Ldfld, dataPlant)); A(il.Create(OpCodes.Call, getARM)); A(il.Create(OpCodes.Stloc, f));
    A(il.Create(OpCodes.Ldarg_0)); A(il.Create(OpCodes.Ldfld, textARM)); A(il.Create(OpCodes.Ldstr, "护甲:")); A(il.Create(OpCodes.Ldloca, f)); A(il.Create(OpCodes.Call, singleToString)); A(il.Create(OpCodes.Call, concat2)); A(il.Create(OpCodes.Ldc_I4_1)); A(il.Create(OpCodes.Call, setText));
    A(afterARM);

    A(il.Create(OpCodes.Ldarg_0)); A(il.Create(OpCodes.Ldfld, textDEF)); A(il.Create(OpCodes.Call, objectImplicit)); A(il.Create(OpCodes.Brfalse, afterDEF));
    A(il.Create(OpCodes.Ldarg_0)); A(il.Create(OpCodes.Ldfld, dataPlant)); A(il.Create(OpCodes.Call, getDEF)); A(il.Create(OpCodes.Stloc, f));
    A(il.Create(OpCodes.Ldarg_0)); A(il.Create(OpCodes.Ldfld, textDEF)); A(il.Create(OpCodes.Ldstr, "防御:")); A(il.Create(OpCodes.Ldloca, f)); A(il.Create(OpCodes.Call, singleToString)); A(il.Create(OpCodes.Call, concat2)); A(il.Create(OpCodes.Ldc_I4_1)); A(il.Create(OpCodes.Call, setText));
    A(afterDEF);

    // cliqueLogo.sprite = ResourceManager.cliqueLogos[(int)plant.clique];
    A(il.Create(OpCodes.Ldarg_0)); A(il.Create(OpCodes.Ldfld, cliqueLogo)); A(il.Create(OpCodes.Call, objectImplicit)); A(il.Create(OpCodes.Brfalse, afterClique));
    A(il.Create(OpCodes.Ldarg_0)); A(il.Create(OpCodes.Ldfld, cliqueLogo));
    A(il.Create(OpCodes.Ldsfld, cliqueLogos));
    A(il.Create(OpCodes.Ldarg_0)); A(il.Create(OpCodes.Ldfld, dataPlant)); A(il.Create(OpCodes.Ldfld, plantClique));
    A(il.Create(OpCodes.Callvirt, cliqueGetItem)); A(il.Create(OpCodes.Call, setSprite));
    A(afterClique);

    // Skill logo and skill data.
    A(il.Create(OpCodes.Ldarg_0)); A(il.Create(OpCodes.Ldfld, dataPlant)); A(il.Create(OpCodes.Ldfld, plantSkill)); A(il.Create(OpCodes.Stloc, skillLocal));
    A(il.Create(OpCodes.Ldloc, skillLocal)); A(il.Create(OpCodes.Brfalse, afterSkill));
    A(il.Create(OpCodes.Ldarg_0)); A(il.Create(OpCodes.Ldfld, skillLogo));
    A(il.Create(OpCodes.Ldloc, skillLocal)); A(il.Create(OpCodes.Ldfld, skillId));
    A(il.Create(OpCodes.Ldarg_0)); A(il.Create(OpCodes.Ldfld, dataPlant)); A(il.Create(OpCodes.Ldfld, plantId)); A(il.Create(OpCodes.Ldc_I4_3)); A(il.Create(OpCodes.Mul)); A(il.Create(OpCodes.Add));
    A(il.Create(OpCodes.Ldloc, skillLocal)); A(il.Create(OpCodes.Ldfld, skillName));
    A(il.Create(OpCodes.Call, loadSkillLogo)); A(il.Create(OpCodes.Call, setSprite));
    A(il.Create(OpCodes.Ldarg_0)); A(il.Create(OpCodes.Ldloc, skillLocal)); A(il.Create(OpCodes.Call, loadSkillData));
    A(afterSkill);

    // characteristic.SetText(plant.characteristicText, true);
    A(il.Create(OpCodes.Ldarg_0)); A(il.Create(OpCodes.Ldfld, characteristic));
    A(il.Create(OpCodes.Ldarg_0)); A(il.Create(OpCodes.Ldfld, dataPlant)); A(il.Create(OpCodes.Ldfld, plantCharacteristic));
    A(il.Create(OpCodes.Ldc_I4_1)); A(il.Create(OpCodes.Call, setText));

    // Exactly three talent rows.
    A(il.Create(OpCodes.Ldc_I4_0)); A(il.Create(OpCodes.Stloc, iLocal)); A(il.Create(OpCodes.Br, talentCheck));
    A(talentBody);
    A(il.Create(OpCodes.Ldarg_0)); A(il.Create(OpCodes.Ldfld, dataPlant)); A(il.Create(OpCodes.Ldfld, plantTalentNames)); A(il.Create(OpCodes.Ldloc, iLocal)); A(il.Create(OpCodes.Ldelem_Ref)); A(il.Create(OpCodes.Stloc, nameLocal));
    A(il.Create(OpCodes.Ldloc, nameLocal)); A(il.Create(OpCodes.Ldstr, "")); A(il.Create(OpCodes.Call, strNe)); A(il.Create(OpCodes.Brfalse, talentNoSuffix));
    A(il.Create(OpCodes.Ldloc, nameLocal)); A(il.Create(OpCodes.Ldstr, "：")); A(il.Create(OpCodes.Call, concat2)); A(il.Create(OpCodes.Stloc, nameLocal));
    A(talentNoSuffix);
    A(il.Create(OpCodes.Ldarg_0)); A(il.Create(OpCodes.Ldfld, talentNamesUI)); A(il.Create(OpCodes.Ldloc, iLocal)); A(il.Create(OpCodes.Callvirt, talentGetItem)); A(il.Create(OpCodes.Ldloc, nameLocal)); A(il.Create(OpCodes.Ldc_I4_1)); A(il.Create(OpCodes.Call, setText));
    A(il.Create(OpCodes.Ldarg_0)); A(il.Create(OpCodes.Ldfld, talentDescriptionsUI)); A(il.Create(OpCodes.Ldloc, iLocal)); A(il.Create(OpCodes.Callvirt, talentGetItem));
    A(il.Create(OpCodes.Ldarg_0)); A(il.Create(OpCodes.Ldfld, dataPlant)); A(il.Create(OpCodes.Ldfld, plantTalents)); A(il.Create(OpCodes.Ldloc, iLocal)); A(il.Create(OpCodes.Ldelem_Ref)); A(il.Create(OpCodes.Ldc_I4_1)); A(il.Create(OpCodes.Call, setText));
    A(il.Create(OpCodes.Ldloc, iLocal)); A(il.Create(OpCodes.Ldc_I4_1)); A(il.Create(OpCodes.Add)); A(il.Create(OpCodes.Stloc, iLocal));
    A(talentCheck); A(il.Create(OpCodes.Ldloc, iLocal)); A(il.Create(OpCodes.Ldc_I4_3)); A(il.Create(OpCodes.Blt, talentBody));

    // stars[i].gameObject.SetActive(i < plant.stars), using List<Image>.Count rather than private _size.
    A(il.Create(OpCodes.Ldc_I4_0)); A(il.Create(OpCodes.Stloc, iLocal)); A(il.Create(OpCodes.Br, starsCheck));
    A(starsBody);
    A(il.Create(OpCodes.Ldarg_0)); A(il.Create(OpCodes.Ldfld, starsUI)); A(il.Create(OpCodes.Ldloc, iLocal)); A(il.Create(OpCodes.Callvirt, starsGetItem)); A(il.Create(OpCodes.Call, getGameObject));
    A(il.Create(OpCodes.Ldloc, iLocal)); A(il.Create(OpCodes.Ldarg_0)); A(il.Create(OpCodes.Ldfld, dataPlant)); A(il.Create(OpCodes.Ldfld, plantStars)); A(il.Create(OpCodes.Clt));
    A(il.Create(OpCodes.Call, setActive));
    A(il.Create(OpCodes.Ldloc, iLocal)); A(il.Create(OpCodes.Ldc_I4_1)); A(il.Create(OpCodes.Add)); A(il.Create(OpCodes.Stloc, iLocal));
    A(starsCheck); A(il.Create(OpCodes.Ldloc, iLocal)); A(il.Create(OpCodes.Ldarg_0)); A(il.Create(OpCodes.Ldfld, starsUI)); A(il.Create(OpCodes.Callvirt, starsCount)); A(il.Create(OpCodes.Blt, starsBody));
    A(ret);

    Directory.CreateDirectory(Path.GetDirectoryName(output)!);
    module.Write(output);
}

var outputSha = Sha(output);
Console.WriteLine($"OUTPUT_SHA256 {outputSha}");

using (var reopened = ModuleDefinition.ReadModule(output, new ReaderParameters { InMemory = true, ReadingMode = ReadingMode.Immediate }))
{
    var types = AllTypes(reopened.Types).ToList();
    var methods = types.SelectMany(t => t.Methods).ToList();
    var fields = types.SelectMany(t => t.Fields).ToList();
    if (methods.Count != ExpectedMethods || fields.Count != ExpectedFields)
        throw new InvalidDataException($"reopen metadata count drift methods={methods.Count} fields={fields.Count}");
    if (reopened.AssemblyReferences.Any(a => a.Name == "System.Private.CoreLib"))
        throw new InvalidDataException("unexpected System.Private.CoreLib output ref");

    var target = methods.Single(m => Raw(m) == TargetToken);
    var objectLocals = target.Body.Variables.Count(v => v.VariableType.FullName == "System.Object");
    var privateSizeRefs = target.Body.Instructions.Count(i => i.Operand is FieldReference fr && fr.Name == "_size" && fr.DeclaringType.FullName.StartsWith("System.Collections.Generic.List`1", StringComparison.Ordinal));
    var levelRefs = target.Body.Instructions.Count(i => i.Operand is FieldReference fr && fr.DeclaringType.FullName == "Plant" && fr.Name == "level");
    var listCountCalls = CountCalls(target, mr => mr.Name == "get_Count" && mr.DeclaringType.FullName == fields.Single(f => f.DeclaringType.FullName == "Board_PlantDetail_PlantData" && f.Name == "stars").FieldType.FullName);
    var loadLogoCalls = CountCalls(target, mr => mr.DeclaringType.FullName == "ResourceManager" && mr.Name == "LoadSkillLogo");
    var loadSkillDataCalls = CountCalls(target, mr => mr.DeclaringType.FullName == "Board_PlantDetail_PlantData" && mr.Name == "LoadSkillData");
    var truncateCalls = CountCalls(target, mr => mr.DeclaringType.FullName == "System.Math" && mr.Name == "Truncate");
    var setTextCalls = CountCalls(target, mr => mr.DeclaringType.FullName == "TMPro.TMP_Text" && mr.Name == "SetText");
    var setActiveCalls = CountCalls(target, mr => mr.DeclaringType.FullName == "UnityEngine.GameObject" && mr.Name == "SetActive");
    if (objectLocals != 0 || privateSizeRefs != 0 || levelRefs != 1 || listCountCalls != 1 || loadLogoCalls != 1 || loadSkillDataCalls != 1 || truncateCalls != 2 || setTextCalls != 8 || setActiveCalls != 1)
        throw new InvalidDataException($"reopen semantic gate failed objects={objectLocals} _size={privateSizeRefs} level={levelRefs} count={listCountCalls} loadLogo={loadLogoCalls} loadSkillData={loadSkillDataCalls} truncate={truncateCalls} setText={setTextCalls} setActive={setActiveCalls}");
    if (CountCalls(target, mr => mr.DeclaringType.FullName == "Plant" && mr.Name == "GetATK") != 1 ||
        CountCalls(target, mr => mr.DeclaringType.FullName == "Plant" && mr.Name == "GetARM") != 1 ||
        CountCalls(target, mr => mr.DeclaringType.FullName == "Plant" && mr.Name == "GetDEF") != 1)
        throw new InvalidDataException("ATK/ARM/DEF call gate failed");
    Console.WriteLine($"REOPEN_LOAD_DATA_PASS token=0x{TargetToken:X8} code_size={target.Body.CodeSize} locals={target.Body.Variables.Count} object_locals=0 private_List_size_refs=0 Plant_level_refs=1 List_Count_calls=1 LoadSkillLogo_calls=1 LoadSkillData_calls=1 truncate_calls=2 SetText_calls=8 SetActive_calls=1");

    var afterM = methods.ToDictionary(Raw, MethodSemantic);
    var changedMethods = beforeM.Keys.Where(k => !afterM.TryGetValue(k, out var sem) || sem != beforeM[k]).OrderBy(k => k).ToList();
    if (changedMethods.Count != 1 || changedMethods[0] != TargetToken)
        throw new InvalidDataException("method semantic isolation failed: " + string.Join(',', changedMethods.Select(x => $"0x{x:X8}")));
    Console.WriteLine($"SEMANTIC_ISOLATION_PASS untouched_methods={ExpectedMethods - 1} changed_methods=1 target=0x{TargetToken:X8}");

    var afterF = fields.ToDictionary(Raw, FieldSemantic);
    var changedFields = beforeF.Keys.Where(k => !afterF.TryGetValue(k, out var sem) || sem != beforeF[k]).OrderBy(k => k).ToList();
    if (changedFields.Count != 0)
        throw new InvalidDataException("field metadata isolation failed: " + string.Join(',', changedFields.Select(x => $"0x{x:X8}")));
    Console.WriteLine($"FIELD_METADATA_ISOLATION_PASS unchanged_fields={ExpectedFields} changed_fields=0");

    foreach (var token in PreservationTokens)
    {
        if (!afterM.TryGetValue(token, out var sem) || sem != preservedBefore[token])
            throw new InvalidDataException($"preservation regression token=0x{token:X8}");
    }
    Console.WriteLine("PRESERVATION_PASS batch1=4 plant_ctor=1 projectile_ctor=1 projectile_hf21=2 managers=2 plant_detail_initialize=1 load_skill_logo=1");
}

Console.WriteLine("PATCH_LOAD_DATA method_body_changes=1 pc_native_semantics=1 private_List_size_removed=1 object_arithmetic_removed=1 metadata_visibility_changes=0");
return 0;
