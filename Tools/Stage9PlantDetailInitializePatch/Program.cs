using Mono.Cecil;
using Mono.Cecil.Cil;
using System.Globalization;
using System.Security.Cryptography;
using System.Text;

if (args.Length != 2)
{
    Console.Error.WriteLine("Usage: Stage9PlantDetailInitializePatch <input.dll> <output.dll>");
    return 2;
}

const string ExpectedInputSha = "b192a02950c52778008b612c5afb2795f5124edb25e7744c68e1293be8dc34c0";
const int ExpectedMethods = 2317;
const int ExpectedFields = 2802;
const uint TargetToken = 0x06000667;
const int ExpectedRid = 1639;
const int ExpectedOldCodeSize = 830;
const int ExpectedOldLocals = 25;
const string NativeSliceSha = "80527520200166f1c38e6e3c64f952e5e99d439b59cff56b888295d9a5640fdc";

var PreservationTokens = new HashSet<uint>
{
    0x0600067E, 0x06000285, 0x06000289, 0x0600028A, // gameplay Batch1
    0x060003BA,                                     // Plant::.ctor
    0x060003FD,                                     // Projectile::.ctor
    0x060003DD, 0x060003DE,                        // HF21 Projectile.ResetData / BindTrack
    0x060001F3, 0x06000216                         // ProjectileManager.Start / ResourceManager.Start
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
static MethodReference FindInMethod(MethodDefinition m, Func<MethodReference, bool> pred, string label)
{
    var hits = m.Body.Instructions
        .Where(i => (i.OpCode == OpCodes.Call || i.OpCode == OpCodes.Callvirt || i.OpCode == OpCodes.Newobj) && i.Operand is MethodReference mr && pred(mr))
        .Select(i => (MethodReference)i.Operand)
        .DistinctBy(mr => mr.FullName + "@" + ScopeName(mr.DeclaringType))
        .ToList();
    if (hits.Count != 1) throw new InvalidDataException($"reference lookup {label} count={hits.Count}");
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
    foreach (var f in fields) beforeF[Raw(f)] = FieldSemantic(f);

    var pd = types.Single(t => t.FullName == "Plant_DetaiPage");
    var plantT = types.Single(t => t.FullName == "Plant");
    var skillT = types.Single(t => t.FullName == "Skill");
    var dataT = types.Single(t => t.FullName == "Board_PlantDetail_PlantData");
    var target = pd.Methods.Single(m => m.Name == "Initialize" && m.Parameters.Count == 1 && m.Parameters[0].ParameterType.FullName == "Plant");
    if (Raw(target) != TargetToken || target.MetadataToken.RID != ExpectedRid || target.Body.CodeSize != ExpectedOldCodeSize || target.Body.Variables.Count != ExpectedOldLocals)
        throw new InvalidDataException($"target fingerprint drift token=0x{Raw(target):X8} rid={target.MetadataToken.RID} size={target.Body.CodeSize} locals={target.Body.Variables.Count}");
    var oldPrivateRead = target.Body.Instructions.SingleOrDefault(i => i.Offset == 0x7C && i.OpCode == OpCodes.Ldfld && i.Operand is FieldReference fr && fr.DeclaringType.FullName == "Plant" && fr.Name == "isOnField");
    if (oldPrivateRead is null) throw new InvalidDataException("old IL_007C Plant.isOnField fingerprint mismatch");

    FieldDefinition PF(string n) => pd.Fields.Single(f => f.Name == n);
    FieldDefinition PlF(string n) => plantT.Fields.Single(f => f.Name == n);
    FieldDefinition SF(string n) => skillT.Fields.Single(f => f.Name == n);
    var plantF = PF("plant");
    var skillGoF = PF("skill");
    var pSkillF = PF("p_skill");
    var textNameF = PF("text_name");
    var skillButtonF = PF("skillButton");
    var skillAutoF = PF("skillAuto");
    var textKeyF = PF("text_key");
    var chargeLayerF = PF("chargeLayer");
    var dataPageF = PF("dataPage");
    var skillRangeF = PF("skillRange");
    var rangeImageF = PF("rangeIamge");
    var plantNameF = PlF("plantName");
    var plantSkillF = PlF("skill");
    var triggerF = SF("skillTriggerType");
    var maxChargeF = SF("maxChargedLayer");

    var objImplicit = FindInMethod(target, mr => mr.DeclaringType.FullName == "UnityEngine.Object" && mr.Name == "op_Implicit", "Object.op_Implicit");
    var setActive = FindInMethod(target, mr => mr.DeclaringType.FullName == "UnityEngine.GameObject" && mr.Name == "SetActive", "GameObject.SetActive");
    var setText = FindInMethod(target, mr => mr.DeclaringType.FullName == "TMPro.TMP_Text" && mr.Name == "SetText" && mr.Parameters.Count == 2, "TMP_Text.SetText");
    var componentTransform = FindInMethod(target, mr => mr.DeclaringType.FullName == "UnityEngine.Component" && mr.Name == "get_transform", "Component.get_transform");
    var gameObjectTransform = FindInMethod(target, mr => mr.DeclaringType.FullName == "UnityEngine.GameObject" && mr.Name == "get_transform", "GameObject.get_transform");
    var getPosition = FindInMethod(target, mr => mr.DeclaringType.FullName == "UnityEngine.Transform" && mr.Name == "get_position", "Transform.get_position");
    var setPosition = FindInMethod(target, mr => mr.DeclaringType.FullName == "UnityEngine.Transform" && mr.Name == "set_position", "Transform.set_position");
    var setEnabled = FindInMethod(target, mr => mr.DeclaringType.FullName == "UnityEngine.Behaviour" && mr.Name == "set_enabled", "Behaviour.set_enabled");
    var getGameObject = FindInMethod(target, mr => mr.DeclaringType.FullName == "UnityEngine.Component" && mr.Name == "get_gameObject", "Component.get_gameObject");
    var setColor = FindInMethod(target, mr => mr.DeclaringType.FullName == "UnityEngine.UI.Graphic" && mr.Name == "set_color", "Graphic.set_color");
    var loadSkillLogo = pd.Methods.Single(m => m.Name == "LoadSkillLogo" && m.Parameters.Count == 0);
    var loadData = dataT.Methods.Single(m => m.Name == "LoadData" && m.Parameters.Count == 1 && m.Parameters[0].ParameterType.FullName == "Plant_DetaiPage");
    var checkText = dataT.Methods.Single(m => m.Name == "CheckText" && m.Parameters.Count == 1 && m.Parameters[0].ParameterType.FullName == "System.Int32");
    var isOnField = plantT.Methods.Single(m => m.Name == "IsOnField" && m.Parameters.Count == 0 && m.ReturnType.FullName == "System.Boolean");

    // Preserve the PC direct read semantics without changing Plant field visibility: the existing
    // public IsOnField() accessor is a one-field getter for the same private bool.
    if (!isOnField.HasBody || isOnField.Body.Instructions.Count(i => i.OpCode == OpCodes.Ldfld && i.Operand is FieldReference fr && fr.DeclaringType.FullName == "Plant" && fr.Name == "isOnField") != 1)
        throw new InvalidDataException("Plant.IsOnField accessor no longer matches private field semantics");

    var vectorT = getPosition.ReturnType;
    var colorT = setColor.Parameters[0].ParameterType;
    var vecX = new FieldReference("x", module.TypeSystem.Single, vectorT);
    var vecY = new FieldReference("y", module.TypeSystem.Single, vectorT);
    var vecZ = new FieldReference("z", module.TypeSystem.Single, vectorT);
    var vecCtor = new MethodReference(".ctor", module.TypeSystem.Void, vectorT) { HasThis = true };
    vecCtor.Parameters.Add(new ParameterDefinition(module.TypeSystem.Single));
    vecCtor.Parameters.Add(new ParameterDefinition(module.TypeSystem.Single));
    vecCtor.Parameters.Add(new ParameterDefinition(module.TypeSystem.Single));
    var colorCtor = new MethodReference(".ctor", module.TypeSystem.Void, colorT) { HasThis = true };
    for (int i = 0; i < 4; i++) colorCtor.Parameters.Add(new ParameterDefinition(module.TypeSystem.Single));

    Console.WriteLine($"NATIVE_AUTHORITY target_token=0x{TargetToken:X8} rid={ExpectedRid} method_pointer_entry=0x181B86090 va=0x1803A3E30 end=0x1803A41C3 native_slice_sha256={NativeSliceSha}");
    Console.WriteLine("NATIVE_SEMANTICS assign_plant=1 null_branch_skill_off=1 live_branch_name_skill_isOnField=1 position_xyz_preserved_y_plus_30=1 loadSkillLogo=1 manual_skill_ui=1 maxCharge_gt1_only_enables=1 data_refresh=1 skillRange_false=1 rangeColor_rgba_1=1");
    Console.WriteLine("CLR_ACCESS_ADAPTATION native_private_read=Plant.isOnField managed_call=Plant.IsOnField field_metadata_changed=0");

    var body = target.Body;
    body.Instructions.Clear();
    body.Variables.Clear();
    body.ExceptionHandlers.Clear();
    body.InitLocals = true;
    body.MaxStackSize = 6;
    var pos = new VariableDefinition(vectorT);
    var shiftedPos = new VariableDefinition(vectorT);
    var white = new VariableDefinition(colorT);
    body.Variables.Add(pos);
    body.Variables.Add(shiftedPos);
    body.Variables.Add(white);
    var il = body.GetILProcessor();

    var livePlant = il.Create(OpCodes.Nop);
    var afterPlantBranch = il.Create(OpCodes.Nop);
    var afterPosition = il.Create(OpCodes.Nop);
    var hasSkill = il.Create(OpCodes.Nop);
    var afterManual = il.Create(OpCodes.Nop);
    var afterSkillUi = il.Create(OpCodes.Nop);

    // this.plant = thePlant
    il.Append(il.Create(OpCodes.Ldarg_0));
    il.Append(il.Create(OpCodes.Ldarg_1));
    il.Append(il.Create(OpCodes.Stfld, plantF));

    // Unity truthiness branch for thePlant.
    il.Append(il.Create(OpCodes.Ldarg_1));
    il.Append(il.Create(OpCodes.Call, objImplicit));
    il.Append(il.Create(OpCodes.Brtrue, livePlant));

    // Null/dead Plant: skill off, p_skill null.
    il.Append(il.Create(OpCodes.Ldarg_0));
    il.Append(il.Create(OpCodes.Ldfld, skillGoF));
    il.Append(il.Create(OpCodes.Ldc_I4_0));
    il.Append(il.Create(OpCodes.Call, setActive));
    il.Append(il.Create(OpCodes.Ldarg_0));
    il.Append(il.Create(OpCodes.Ldnull));
    il.Append(il.Create(OpCodes.Stfld, pSkillF));
    il.Append(il.Create(OpCodes.Br, afterPlantBranch));

    // Live Plant: name, skill pointer, and original isOnField semantics.
    il.Append(livePlant);
    il.Append(il.Create(OpCodes.Ldarg_0));
    il.Append(il.Create(OpCodes.Ldfld, textNameF));
    il.Append(il.Create(OpCodes.Ldarg_1));
    il.Append(il.Create(OpCodes.Ldfld, plantNameF));
    il.Append(il.Create(OpCodes.Ldc_I4_1));
    il.Append(il.Create(OpCodes.Callvirt, setText));
    il.Append(il.Create(OpCodes.Ldarg_0));
    il.Append(il.Create(OpCodes.Ldarg_0));
    il.Append(il.Create(OpCodes.Ldfld, plantF));
    il.Append(il.Create(OpCodes.Ldfld, plantSkillF));
    il.Append(il.Create(OpCodes.Stfld, pSkillF));
    il.Append(il.Create(OpCodes.Ldarg_0));
    il.Append(il.Create(OpCodes.Ldfld, skillGoF));
    il.Append(il.Create(OpCodes.Ldarg_1));
    il.Append(il.Create(OpCodes.Call, isOnField));
    il.Append(il.Create(OpCodes.Call, setActive));

    // If this.plant is live, move skill to plant.position + (0, 30, 0), preserving X/Y/Z.
    il.Append(afterPlantBranch);
    il.Append(il.Create(OpCodes.Ldarg_0));
    il.Append(il.Create(OpCodes.Ldfld, plantF));
    il.Append(il.Create(OpCodes.Call, objImplicit));
    il.Append(il.Create(OpCodes.Brfalse, afterPosition));
    il.Append(il.Create(OpCodes.Ldarg_0));
    il.Append(il.Create(OpCodes.Ldfld, plantF));
    il.Append(il.Create(OpCodes.Call, componentTransform));
    il.Append(il.Create(OpCodes.Call, getPosition));
    il.Append(il.Create(OpCodes.Stloc, pos));
    il.Append(il.Create(OpCodes.Ldloca, shiftedPos));
    il.Append(il.Create(OpCodes.Ldloca, pos));
    il.Append(il.Create(OpCodes.Ldfld, vecX));
    il.Append(il.Create(OpCodes.Ldloca, pos));
    il.Append(il.Create(OpCodes.Ldfld, vecY));
    il.Append(il.Create(OpCodes.Ldc_R4, 30f));
    il.Append(il.Create(OpCodes.Add));
    il.Append(il.Create(OpCodes.Ldloca, pos));
    il.Append(il.Create(OpCodes.Ldfld, vecZ));
    il.Append(il.Create(OpCodes.Call, vecCtor));
    il.Append(il.Create(OpCodes.Ldarg_0));
    il.Append(il.Create(OpCodes.Ldfld, skillGoF));
    il.Append(il.Create(OpCodes.Call, gameObjectTransform));
    il.Append(il.Create(OpCodes.Ldloc, shiftedPos));
    il.Append(il.Create(OpCodes.Call, setPosition));

    il.Append(afterPosition);
    il.Append(il.Create(OpCodes.Ldarg_0));
    il.Append(il.Create(OpCodes.Call, loadSkillLogo));

    // p_skill == null branch.
    il.Append(il.Create(OpCodes.Ldarg_0));
    il.Append(il.Create(OpCodes.Ldfld, pSkillF));
    il.Append(il.Create(OpCodes.Brtrue, hasSkill));
    il.Append(il.Create(OpCodes.Ldarg_0));
    il.Append(il.Create(OpCodes.Ldfld, skillButtonF));
    il.Append(il.Create(OpCodes.Ldc_I4_0));
    il.Append(il.Create(OpCodes.Call, setEnabled));
    il.Append(il.Create(OpCodes.Ldarg_0));
    il.Append(il.Create(OpCodes.Ldfld, skillAutoF));
    il.Append(il.Create(OpCodes.Call, getGameObject));
    il.Append(il.Create(OpCodes.Ldc_I4_1));
    il.Append(il.Create(OpCodes.Call, setActive));
    il.Append(il.Create(OpCodes.Ldarg_0));
    il.Append(il.Create(OpCodes.Ldfld, textKeyF));
    il.Append(il.Create(OpCodes.Call, getGameObject));
    il.Append(il.Create(OpCodes.Ldc_I4_0));
    il.Append(il.Create(OpCodes.Call, setActive));
    il.Append(il.Create(OpCodes.Ldarg_0));
    il.Append(il.Create(OpCodes.Ldfld, chargeLayerF));
    il.Append(il.Create(OpCodes.Ldc_I4_0));
    il.Append(il.Create(OpCodes.Call, setActive));
    il.Append(il.Create(OpCodes.Br, afterSkillUi));

    // p_skill != null: only change manual-trigger UI when trigger type == Manual (1).
    il.Append(hasSkill);
    il.Append(il.Create(OpCodes.Ldarg_0));
    il.Append(il.Create(OpCodes.Ldfld, pSkillF));
    il.Append(il.Create(OpCodes.Ldfld, triggerF));
    il.Append(il.Create(OpCodes.Ldc_I4_1));
    il.Append(il.Create(OpCodes.Bne_Un, afterManual));
    il.Append(il.Create(OpCodes.Ldarg_0));
    il.Append(il.Create(OpCodes.Ldfld, skillButtonF));
    il.Append(il.Create(OpCodes.Ldc_I4_1));
    il.Append(il.Create(OpCodes.Call, setEnabled));
    il.Append(il.Create(OpCodes.Ldarg_0));
    il.Append(il.Create(OpCodes.Ldfld, skillAutoF));
    il.Append(il.Create(OpCodes.Call, getGameObject));
    il.Append(il.Create(OpCodes.Ldc_I4_0));
    il.Append(il.Create(OpCodes.Call, setActive));
    il.Append(il.Create(OpCodes.Ldarg_0));
    il.Append(il.Create(OpCodes.Ldfld, textKeyF));
    il.Append(il.Create(OpCodes.Call, getGameObject));
    il.Append(il.Create(OpCodes.Ldc_I4_1));
    il.Append(il.Create(OpCodes.Call, setActive));

    // Native code only enables chargeLayer when maxChargedLayer > 1; <=1 leaves it unchanged.
    il.Append(afterManual);
    il.Append(il.Create(OpCodes.Ldarg_0));
    il.Append(il.Create(OpCodes.Ldfld, pSkillF));
    il.Append(il.Create(OpCodes.Ldfld, maxChargeF));
    il.Append(il.Create(OpCodes.Ldc_I4_1));
    il.Append(il.Create(OpCodes.Ble, afterSkillUi));
    il.Append(il.Create(OpCodes.Ldarg_0));
    il.Append(il.Create(OpCodes.Ldfld, chargeLayerF));
    il.Append(il.Create(OpCodes.Ldc_I4_1));
    il.Append(il.Create(OpCodes.Call, setActive));

    // dataPage refresh, skillRange=false, range image opaque white.
    il.Append(afterSkillUi);
    il.Append(il.Create(OpCodes.Ldarg_0));
    il.Append(il.Create(OpCodes.Ldfld, dataPageF));
    il.Append(il.Create(OpCodes.Ldarg_0));
    il.Append(il.Create(OpCodes.Call, loadData));
    il.Append(il.Create(OpCodes.Ldarg_0));
    il.Append(il.Create(OpCodes.Ldfld, dataPageF));
    il.Append(il.Create(OpCodes.Ldc_I4_0));
    il.Append(il.Create(OpCodes.Call, checkText));
    il.Append(il.Create(OpCodes.Ldarg_0));
    il.Append(il.Create(OpCodes.Ldc_I4_0));
    il.Append(il.Create(OpCodes.Stfld, skillRangeF));
    il.Append(il.Create(OpCodes.Ldloca, white));
    il.Append(il.Create(OpCodes.Ldc_R4, 1f));
    il.Append(il.Create(OpCodes.Ldc_R4, 1f));
    il.Append(il.Create(OpCodes.Ldc_R4, 1f));
    il.Append(il.Create(OpCodes.Ldc_R4, 1f));
    il.Append(il.Create(OpCodes.Call, colorCtor));
    il.Append(il.Create(OpCodes.Ldarg_0));
    il.Append(il.Create(OpCodes.Ldfld, rangeImageF));
    il.Append(il.Create(OpCodes.Ldloc, white));
    il.Append(il.Create(OpCodes.Callvirt, setColor));
    il.Append(il.Create(OpCodes.Ret));

    Console.WriteLine("PATCH_PLANT_DETAIL_INITIALIZE method_body_changes=1 private_field_access_removed=1 vector_lowering=typed_xyz_preserving color_lowering=typed_rgba charge_semantics=gt1_enable_only");
    module.Write(output);
}

Console.WriteLine($"OUTPUT_SHA256 {Sha(output)}");

using (var module = ModuleDefinition.ReadModule(output, new ReaderParameters { InMemory = true, ReadingMode = ReadingMode.Immediate }))
{
    var types = AllTypes(module.Types).ToList();
    var methods = types.SelectMany(t => t.Methods).ToList();
    var fields = types.SelectMany(t => t.Fields).ToList();
    if (methods.Count != ExpectedMethods || fields.Count != ExpectedFields) throw new InvalidDataException("output metadata count drift");
    if (module.AssemblyReferences.Any(a => a.Name == "System.Private.CoreLib")) throw new InvalidDataException("unexpected System.Private.CoreLib output ref");
    var target = methods.Single(m => Raw(m) == TargetToken);
    if (target.Body.Variables.Count != 3 || !target.Body.InitLocals || target.Body.ExceptionHandlers.Count != 0) throw new InvalidDataException("recovered local/EH shape mismatch");
    if (target.Body.Instructions.Any(i => i.Operand is FieldReference fr && fr.DeclaringType.FullName == "Plant" && fr.Name == "isOnField")) throw new InvalidDataException("private Plant.isOnField reference remains");
    if (CountCalls(target, mr => mr.DeclaringType.FullName == "Plant" && mr.Name == "IsOnField" && mr.Parameters.Count == 0) != 1) throw new InvalidDataException("Plant.IsOnField adaptation mismatch");
    if (CountCalls(target, mr => mr.DeclaringType.FullName == "UnityEngine.Vector3" && mr.Name == ".ctor" && mr.Parameters.Count == 3) != 1) throw new InvalidDataException("typed Vector3 ctor mismatch");
    if (CountCalls(target, mr => mr.DeclaringType.FullName == "UnityEngine.Color" && mr.Name == ".ctor" && mr.Parameters.Count == 4) != 1) throw new InvalidDataException("typed Color ctor mismatch");
    if (target.Body.Instructions.Count(i => i.OpCode == OpCodes.Ldc_R4 && i.Operand is float f && BitConverter.SingleToInt32Bits(f) == BitConverter.SingleToInt32Bits(30f)) != 1) throw new InvalidDataException("Y+30 native constant mismatch");
    if (CountCalls(target, mr => mr.DeclaringType.FullName == "Board_PlantDetail_PlantData" && mr.Name == "LoadData") != 1 || CountCalls(target, mr => mr.DeclaringType.FullName == "Board_PlantDetail_PlantData" && mr.Name == "CheckText") != 1) throw new InvalidDataException("dataPage call mismatch");

    var afterM = methods.ToDictionary(Raw, MethodSemantic);
    var changed = beforeM.Keys.Where(k => beforeM[k] != afterM[k]).OrderBy(x => x).ToList();
    if (changed.Count != 1 || changed[0] != TargetToken)
        throw new InvalidDataException("semantic isolation failure: " + string.Join(',', changed.Select(x => $"0x{x:X8}")));
    var afterF = fields.ToDictionary(Raw, FieldSemantic);
    var changedFields = beforeF.Keys.Where(k => beforeF[k] != afterF[k]).ToList();
    if (changedFields.Count != 0)
        throw new InvalidDataException("field metadata drift: " + string.Join(',', changedFields.Select(x => $"0x{x:X8}")));
    foreach (var tok in PreservationTokens)
        if (!beforeM.TryGetValue(tok, out var before) || !afterM.TryGetValue(tok, out var after) || before != after)
            throw new InvalidDataException($"preservation failure token=0x{tok:X8}");

    Console.WriteLine($"REOPEN_PLANT_DETAIL_INITIALIZE_PASS token=0x{TargetToken:X8} code_size={target.Body.CodeSize} locals={target.Body.Variables.Count} private_isOnField_refs=0 IsOnField_calls=1 vector_ctor=1 color_ctor=1");
    Console.WriteLine($"SEMANTIC_ISOLATION_PASS untouched_methods={methods.Count - 1} changed_methods=1 target_token=0x{TargetToken:X8}");
    Console.WriteLine($"FIELD_METADATA_ISOLATION_PASS unchanged_fields={fields.Count} changed_fields=0");
    Console.WriteLine("PRESERVATION_PASS batch1=4 plant_ctor=1 projectile_ctor=1 projectile_hf21=2 managers=2");
}

return 0;
