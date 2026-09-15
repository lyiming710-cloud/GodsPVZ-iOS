using Mono.Cecil;
using Mono.Cecil.Cil;
using System.Globalization;
using System.Security.Cryptography;
using System.Text;

if (args.Length != 3)
{
    Console.Error.WriteLine("Usage: Stage9PlantDetailSkillProgressPatch <input.dll> <output.dll> <expected-input-sha256>");
    return 2;
}

const uint TargetToken = 0x06000664;
const int ExpectedRid = 1636;
const int ExpectedMethods = 2317;
const int ExpectedFields = 2802;
const int ExpectedOldCodeSize = 2481;
const int ExpectedOldLocals = 96;
const string NativeSpanSha = "5050f85b69db4e37dcf86232a9c388ac32e4b622972556a8bfa4340cf3f8dbe4";

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

var input = Path.GetFullPath(args[0]);
var output = Path.GetFullPath(args[1]);
var expectedInputSha = args[2].Trim().ToLowerInvariant();
if (expectedInputSha.Length != 64 || expectedInputSha.Any(c => !Uri.IsHexDigit(c))) throw new InvalidDataException("expected input SHA is not 64 hex chars");
if (Sha(input) != expectedInputSha) throw new InvalidDataException("input SHA mismatch");
Console.WriteLine($"INPUT_SHA256 {Sha(input)}");

var beforeM = new Dictionary<uint,string>();
var beforeF = new Dictionary<uint,string>();
string beforeRefs;

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

    var detailT = types.Single(t => t.FullName == "Plant_DetaiPage");
    var skillT = types.Single(t => t.FullName == "Skill");
    var plantT = types.Single(t => t.FullName == "Plant");
    var target = detailT.Methods.Single(m => m.Name == "Update_SkillProgress" && m.Parameters.Count == 0);
    if (Raw(target) != TargetToken || target.MetadataToken.RID != ExpectedRid || target.Body.CodeSize != ExpectedOldCodeSize || target.Body.Variables.Count != ExpectedOldLocals)
        throw new InvalidDataException($"Update_SkillProgress fingerprint drift token=0x{Raw(target):X8} rid={target.MetadataToken.RID} size={target.Body.CodeSize} locals={target.Body.Variables.Count}");
    if (!target.Body.Instructions.Any(i => i.Offset == 0x10 && i.OpCode == OpCodes.Add)) throw new InvalidDataException("expected p_skill plus 72 corruption missing");
    if (!target.Body.Instructions.Any(i => i.Operand is FieldReference f && f.DeclaringType.FullName == "UnityEngine.UI.Image" && f.Name == "m_FillAmount")) throw new InvalidDataException("expected private Image.m_FillAmount lift missing");
    if (target.Body.Variables.Count(v => v.VariableType.FullName == "System.Object") < 5) throw new InvalidDataException("expected damaged object locals missing");

    FieldDefinition DF(string n) => detailT.Fields.Single(f => f.Name == n);
    FieldDefinition SF(string n) => skillT.Fields.Single(f => f.Name == n);
    FieldDefinition PF(string n) => plantT.Fields.Single(f => f.Name == n);
    var pSkill = DF("p_skill");
    var plant = DF("plant");
    var skillLogo = DF("skillLogo");
    var skillChargeTexts = DF("skillChargeTexts");
    var textChargeLayer = DF("text_chargeLayer");
    var chargedLayer = SF("chargedLayer");
    var chargeType = SF("ChargeType");
    var chargeTicking = SF("chargeTicking");
    var maxChargeTicking = SF("maxChargeTicking");
    var duration = SF("duration");
    var skillOngoing = PF("skillOngoing");
    var skillRemainTime = PF("skillRemainTime");

    var listImageGetItem = FindRef(methods, m => m.Name == "get_Item" && m.Parameters.Count == 1 && m.Parameters[0].ParameterType.FullName == "System.Int32" && m.ReturnType.FullName == "UnityEngine.UI.Image" && m.DeclaringType.FullName.Contains("System.Collections.Generic.List`1", StringComparison.Ordinal), "List<Image>.get_Item");
    var getGameObject = FindRef(methods, m => m.DeclaringType.FullName == "UnityEngine.Component" && m.Name == "get_gameObject" && m.Parameters.Count == 0, "Component.get_gameObject");
    var setActive = FindRef(methods, m => m.DeclaringType.FullName == "UnityEngine.GameObject" && m.Name == "SetActive" && m.Parameters.Count == 1 && m.Parameters[0].ParameterType.FullName == "System.Boolean", "GameObject.SetActive");
    var setFillAmount = FindRef(methods, m => m.DeclaringType.FullName == "UnityEngine.UI.Image" && m.Name == "set_fillAmount" && m.Parameters.Count == 1 && m.Parameters[0].ParameterType.FullName == "System.Single", "Image.set_fillAmount");
    var getFillAmount = new MethodReference("get_fillAmount", module.TypeSystem.Single, setFillAmount.DeclaringType)
    {
        HasThis = true,
        ExplicitThis = false,
        CallingConvention = MethodCallingConvention.Default
    };
    var setTextFormatted = FindRef(methods, m => m.DeclaringType.FullName == "TMPro.TMP_Text" && m.Name == "SetText" && m.Parameters.Count == 2 && m.Parameters[0].ParameterType.FullName == "System.String" && m.Parameters[1].ParameterType.FullName == "System.Boolean", "TMP_Text.SetText(string,bool)");
    var setText = FindRef(methods, m => m.DeclaringType.FullName == "TMPro.TMP_Text" && m.Name == "set_text" && m.Parameters.Count == 1 && m.Parameters[0].ParameterType.FullName == "System.String", "TMP_Text.set_text");
    var setColor = FindRef(methods, m => m.DeclaringType.FullName == "TMPro.TMP_Text" && m.Name == "set_color" && m.Parameters.Count == 1 && m.Parameters[0].ParameterType.FullName == "UnityEngine.Color", "TMP_Text.set_color");
    var intToString = FindRef(methods, m => m.DeclaringType.FullName == "System.Int32" && m.Name == "ToString" && m.Parameters.Count == 0, "Int32.ToString");
    var singleToStringFormat = FindRef(methods, m => m.DeclaringType.FullName == "System.Single" && m.Name == "ToString" && m.Parameters.Count == 1 && m.Parameters[0].ParameterType.FullName == "System.String", "Single.ToString(string)");
    var colorCtor = FindRef(methods, m => m.DeclaringType.FullName == "UnityEngine.Color" && m.Name == ".ctor" && m.Parameters.Count == 4 && m.Parameters.All(p => p.ParameterType.FullName == "System.Single"), "Color.ctor(float,float,float,float)");

    Console.WriteLine($"NATIVE_AUTHORITY target_token=0x{TargetToken:X8} rid={ExpectedRid} method_pointer_entry=0x181B86078 va=0x1803A4BB0 end=0x1803A573B native_span_sha256={NativeSpanSha}");
    Console.WriteLine("NATIVE_SEMANTICS chargedLayer_text=1 ready_branch=1 ready_color_white=1 charge_ratio=1 charge_text_current_max=1 passive_dash_text=1 complementary_fill_via_stored_fill=1 normal_color_white=1 ongoing_duration_fill=1 layer_select_fill_one=1");
    Console.WriteLine("RECOVERY_STRATEGY full_single_MethodDef_rebuild=1 typed_array_ldelem_ref=1 public_Image_fillAmount_getter=1 no_private_field_access=1 metadata_changes=0 exception_swallowing=0");

    var body = target.Body;
    body.Instructions.Clear(); body.Variables.Clear(); body.ExceptionHandlers.Clear();
    body.InitLocals = true; body.MaxStackSize = 6;
    var bodyRatio = new VariableDefinition(module.TypeSystem.Single);
    body.Variables.Add(bodyRatio);
    var il = body.GetILProcessor();

    void EmitLogo(int index)
    {
        il.Append(il.Create(OpCodes.Ldarg_0));
        il.Append(il.Create(OpCodes.Ldfld, skillLogo));
        il.Append(il.Create(OpCodes.Ldc_I4, index));
        il.Append(il.Create(OpCodes.Callvirt, listImageGetItem));
    }
    void EmitLogoActive(int index, bool active)
    {
        EmitLogo(index);
        il.Append(il.Create(OpCodes.Call, getGameObject));
        il.Append(il.Create(active ? OpCodes.Ldc_I4_1 : OpCodes.Ldc_I4_0));
        il.Append(il.Create(OpCodes.Call, setActive));
    }
    void EmitChargeText(int index)
    {
        il.Append(il.Create(OpCodes.Ldarg_0));
        il.Append(il.Create(OpCodes.Ldfld, skillChargeTexts));
        il.Append(il.Create(OpCodes.Ldc_I4, index));
        il.Append(il.Create(OpCodes.Ldelem_Ref));
    }
    void EmitChargeTextActive(int index, bool active)
    {
        EmitChargeText(index);
        il.Append(il.Create(OpCodes.Call, getGameObject));
        il.Append(il.Create(active ? OpCodes.Ldc_I4_1 : OpCodes.Ldc_I4_0));
        il.Append(il.Create(OpCodes.Call, setActive));
    }
    void EmitTextLiteral(int index, string text)
    {
        EmitChargeText(index);
        il.Append(il.Create(OpCodes.Ldstr, text));
        il.Append(il.Create(OpCodes.Call, setText));
    }
    void EmitWhite(int index)
    {
        EmitChargeText(index);
        il.Append(il.Create(OpCodes.Ldc_R4, 1f)); il.Append(il.Create(OpCodes.Ldc_R4, 1f)); il.Append(il.Create(OpCodes.Ldc_R4, 1f)); il.Append(il.Create(OpCodes.Ldc_R4, 1f));
        il.Append(il.Create(OpCodes.Newobj, colorCtor));
        il.Append(il.Create(OpCodes.Call, setColor));
    }

    var normalDispatch = il.Create(OpCodes.Ldarg_0);
    var ongoing = il.Create(OpCodes.Ldarg_0);
    var passive = il.Create(OpCodes.Ldarg_0);
    var afterChargeDisplay = il.Create(OpCodes.Ldarg_0);
    var layerAtMostOne = il.Create(OpCodes.Ldarg_0);
    var ret = il.Create(OpCodes.Ret);

    // text_chargeLayer.SetText(p_skill.chargedLayer.ToString(), true)
    il.Append(il.Create(OpCodes.Ldarg_0)); il.Append(il.Create(OpCodes.Ldfld, textChargeLayer));
    il.Append(il.Create(OpCodes.Ldarg_0)); il.Append(il.Create(OpCodes.Ldfld, pSkill)); il.Append(il.Create(OpCodes.Ldflda, chargedLayer)); il.Append(il.Create(OpCodes.Call, intToString));
    il.Append(il.Create(OpCodes.Ldc_I4_1)); il.Append(il.Create(OpCodes.Call, setTextFormatted));

    // Ready branch: chargedLayer > 0 && !plant.skillOngoing.
    il.Append(il.Create(OpCodes.Ldarg_0)); il.Append(il.Create(OpCodes.Ldfld, pSkill)); il.Append(il.Create(OpCodes.Ldfld, chargedLayer)); il.Append(il.Create(OpCodes.Ldc_I4_0)); il.Append(il.Create(OpCodes.Ble, normalDispatch));
    il.Append(il.Create(OpCodes.Ldarg_0)); il.Append(il.Create(OpCodes.Ldfld, plant)); il.Append(il.Create(OpCodes.Ldfld, skillOngoing)); il.Append(il.Create(OpCodes.Brtrue, normalDispatch));
    EmitLogoActive(0, false); EmitLogoActive(1, false); EmitLogoActive(2, true); EmitLogoActive(3, false);
    EmitTextLiteral(1, "Ready!"); EmitWhite(1);
    EmitChargeTextActive(0, false); EmitChargeTextActive(1, true); EmitChargeTextActive(2, false);
    il.Append(il.Create(OpCodes.Ret));

    // Remaining state split by plant.skillOngoing.
    il.Append(normalDispatch); il.Append(il.Create(OpCodes.Ldfld, plant)); il.Append(il.Create(OpCodes.Ldfld, skillOngoing)); il.Append(il.Create(OpCodes.Brtrue, ongoing));

    // Not ongoing: show charging logos.
    EmitLogoActive(0, true); EmitLogoActive(1, true); EmitLogoActive(2, false); EmitLogoActive(3, false);
    il.Append(il.Create(OpCodes.Ldarg_0)); il.Append(il.Create(OpCodes.Ldfld, pSkill)); il.Append(il.Create(OpCodes.Ldfld, chargeType)); il.Append(il.Create(OpCodes.Ldc_I4_3)); il.Append(il.Create(OpCodes.Beq, passive));

    // Active charge type: fill ratio and current/max formatted as 0.0.
    EmitLogo(1);
    il.Append(il.Create(OpCodes.Ldarg_0)); il.Append(il.Create(OpCodes.Ldfld, pSkill)); il.Append(il.Create(OpCodes.Ldfld, chargeTicking));
    il.Append(il.Create(OpCodes.Ldarg_0)); il.Append(il.Create(OpCodes.Ldfld, pSkill)); il.Append(il.Create(OpCodes.Ldfld, maxChargeTicking));
    il.Append(il.Create(OpCodes.Div)); il.Append(il.Create(OpCodes.Stloc, bodyRatio)); il.Append(il.Create(OpCodes.Ldloc, bodyRatio)); il.Append(il.Create(OpCodes.Call, setFillAmount));
    EmitChargeText(0); il.Append(il.Create(OpCodes.Ldarg_0)); il.Append(il.Create(OpCodes.Ldfld, pSkill)); il.Append(il.Create(OpCodes.Ldflda, chargeTicking)); il.Append(il.Create(OpCodes.Ldstr, "0.0")); il.Append(il.Create(OpCodes.Call, singleToStringFormat)); il.Append(il.Create(OpCodes.Call, setText));
    EmitTextLiteral(1, "/");
    EmitChargeText(2); il.Append(il.Create(OpCodes.Ldarg_0)); il.Append(il.Create(OpCodes.Ldfld, pSkill)); il.Append(il.Create(OpCodes.Ldflda, maxChargeTicking)); il.Append(il.Create(OpCodes.Ldstr, "0.0")); il.Append(il.Create(OpCodes.Call, singleToStringFormat)); il.Append(il.Create(OpCodes.Call, setText));
    il.Append(il.Create(OpCodes.Br, afterChargeDisplay));

    // Passive charge type.
    il.Append(passive); il.Append(il.Create(OpCodes.Ldfld, skillLogo)); il.Append(il.Create(OpCodes.Ldc_I4_1)); il.Append(il.Create(OpCodes.Callvirt, listImageGetItem)); il.Append(il.Create(OpCodes.Ldc_R4, 0f)); il.Append(il.Create(OpCodes.Call, setFillAmount));
    EmitTextLiteral(0, "-"); EmitTextLiteral(1, "/"); EmitTextLiteral(2, "-");

    // Complementary ring reads the public fillAmount value that the setter stored.
    il.Append(afterChargeDisplay); il.Append(il.Create(OpCodes.Ldfld, skillLogo)); il.Append(il.Create(OpCodes.Ldc_I4_0)); il.Append(il.Create(OpCodes.Callvirt, listImageGetItem));
    il.Append(il.Create(OpCodes.Ldc_R4, 1f)); EmitLogo(1); il.Append(il.Create(OpCodes.Call, getFillAmount)); il.Append(il.Create(OpCodes.Sub)); il.Append(il.Create(OpCodes.Call, setFillAmount));
    EmitWhite(1);
    EmitChargeTextActive(0, true); EmitChargeTextActive(1, true); EmitChargeTextActive(2, true);
    il.Append(il.Create(OpCodes.Ret));

    // Skill is ongoing: hide charge texts, show duration ring.
    il.Append(ongoing); il.Append(il.Create(OpCodes.Ldfld, skillChargeTexts)); il.Append(il.Create(OpCodes.Ldc_I4_0)); il.Append(il.Create(OpCodes.Ldelem_Ref)); il.Append(il.Create(OpCodes.Call, getGameObject)); il.Append(il.Create(OpCodes.Ldc_I4_0)); il.Append(il.Create(OpCodes.Call, setActive));
    EmitChargeTextActive(1, false); EmitChargeTextActive(2, false);
    EmitLogoActive(2, false); EmitLogoActive(3, true);
    EmitLogo(3); il.Append(il.Create(OpCodes.Ldarg_0)); il.Append(il.Create(OpCodes.Ldfld, plant)); il.Append(il.Create(OpCodes.Ldfld, skillRemainTime)); il.Append(il.Create(OpCodes.Ldarg_0)); il.Append(il.Create(OpCodes.Ldfld, pSkill)); il.Append(il.Create(OpCodes.Ldfld, duration)); il.Append(il.Create(OpCodes.Div)); il.Append(il.Create(OpCodes.Call, setFillAmount));
    il.Append(il.Create(OpCodes.Ldarg_0)); il.Append(il.Create(OpCodes.Ldfld, pSkill)); il.Append(il.Create(OpCodes.Ldfld, chargedLayer)); il.Append(il.Create(OpCodes.Ldc_I4_1)); il.Append(il.Create(OpCodes.Ble, layerAtMostOne));

    EmitLogoActive(0, false); EmitLogoActive(1, true); EmitLogo(1); il.Append(il.Create(OpCodes.Ldc_R4, 1f)); il.Append(il.Create(OpCodes.Call, setFillAmount)); il.Append(il.Create(OpCodes.Br, ret));
    il.Append(layerAtMostOne); il.Append(il.Create(OpCodes.Ldfld, skillLogo)); il.Append(il.Create(OpCodes.Ldc_I4_0)); il.Append(il.Create(OpCodes.Callvirt, listImageGetItem)); il.Append(il.Create(OpCodes.Call, getGameObject)); il.Append(il.Create(OpCodes.Ldc_I4_1)); il.Append(il.Create(OpCodes.Call, setActive));
    EmitLogoActive(1, false); EmitLogo(0); il.Append(il.Create(OpCodes.Ldc_R4, 1f)); il.Append(il.Create(OpCodes.Call, setFillAmount));
    il.Append(ret);

    Directory.CreateDirectory(Path.GetDirectoryName(output)!);
    module.Write(output);
}

Console.WriteLine($"OUTPUT_SHA256 {Sha(output)}");

using (var module = ModuleDefinition.ReadModule(output, new ReaderParameters { InMemory=true, ReadingMode=ReadingMode.Immediate }))
{
    var types = AllTypes(module.Types).ToList(); var methods = types.SelectMany(t => t.Methods).ToList(); var fields = types.SelectMany(t => t.Fields).ToList();
    var target = methods.Single(m => Raw(m) == TargetToken);
    var afterRefs = string.Join("\n", module.AssemblyReferences.Select(a => a.FullName).OrderBy(x => x, StringComparer.Ordinal));
    if (afterRefs != beforeRefs) throw new InvalidDataException("assembly reference set changed");
    if (module.AssemblyReferences.Any(a => a.Name == "System.Private.CoreLib")) throw new InvalidDataException("System.Private.CoreLib pollution");
    int Calls(string type,string name) => target.Body.Instructions.Count(i => (i.OpCode == OpCodes.Call || i.OpCode == OpCodes.Callvirt || i.OpCode == OpCodes.Newobj) && i.Operand is MethodReference m && m.DeclaringType.FullName == type && m.Name == name);
    if (target.Body.Variables.Count != 1 || target.Body.Variables.Any(v => v.VariableType.FullName == "System.Object")) throw new InvalidDataException("typed local gate failed");
    if (target.Body.Instructions.Any(i => i.Operand is FieldReference f && f.DeclaringType.FullName == "UnityEngine.UI.Image" && f.Name == "m_FillAmount")) throw new InvalidDataException("private Image.m_FillAmount survived");
    if (Calls("UnityEngine.UI.Image","get_fillAmount") != 1 || Calls("UnityEngine.UI.Image","set_fillAmount") != 6 || Calls("UnityEngine.GameObject","SetActive") != 23 || Calls("TMPro.TMP_Text","set_text") != 7 || Calls("TMPro.TMP_Text","set_color") != 2 || Calls("TMPro.TMP_Text","SetText") != 1 || Calls("System.Single","ToString") != 2 || Calls("System.Int32","ToString") != 1 || Calls("UnityEngine.Color",".ctor") != 2)
        throw new InvalidDataException("reopen semantic call-count mismatch");
    if (target.Body.Instructions.Count(i => i.OpCode == OpCodes.Ldelem_Ref) != 18) throw new InvalidDataException("typed TextMeshPro array access count mismatch");
    var changedM = methods.Where(m => beforeM[Raw(m)] != MethodSig(m)).Select(m => Raw(m)).OrderBy(x => x).ToList();
    if (changedM.Count != 1 || changedM[0] != TargetToken) throw new InvalidDataException("semantic isolation failed: " + string.Join(',', changedM.Select(x => $"0x{x:X8}")));
    var changedF = fields.Where(f => beforeF[Raw(f)] != FieldSig(f)).Select(f => Raw(f)).ToList();
    if (changedF.Count != 0) throw new InvalidDataException("field metadata drift");
    Console.WriteLine($"REOPEN_SKILL_PROGRESS_PASS token=0x{TargetToken:X8} code_size={target.Body.CodeSize} locals=1 object_locals=0 private_fill_refs=0 public_fill_get=1 fill_set=6 SetActive=23 TMP_set_text=7 TMP_set_color=2 typed_array_ldelem_ref=18 Color_ctor=2");
    Console.WriteLine($"SEMANTIC_ISOLATION_PASS untouched_methods={ExpectedMethods-1} changed_methods=1 target=0x{TargetToken:X8}");
    Console.WriteLine($"FIELD_METADATA_ISOLATION_PASS unchanged_fields={ExpectedFields} changed_fields=0");
    Console.WriteLine("FRAMEWORK_REFERENCE_GATE_PASS system_private_corelib_refs=0 assembly_reference_set_unchanged=1");
}

return 0;
