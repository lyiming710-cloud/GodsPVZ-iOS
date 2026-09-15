using Mono.Cecil;
using Mono.Cecil.Cil;
using System.Globalization;
using System.Security.Cryptography;
using System.Text;

if (args.Length != 3)
{
    Console.Error.WriteLine("Usage: Stage9PlantDataUpdatePatch <input.dll> <output.dll> <expected-input-sha256>");
    return 2;
}

const uint TargetToken = 0x060005DF;
const int ExpectedRid = 1503;
const int ExpectedMethods = 2317;
const int ExpectedFields = 2802;
const int ExpectedOldCodeSize = 943;
const int ExpectedOldLocals = 41;
const string NativeSpanSha = "34d236f7fdea1ea1c0db610af7200e0202ce8d84372af0c2312ffbfcc26ca191";

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

    var dataT = types.Single(t => t.FullName == "Board_PlantDetail_PlantData");
    var plantT = types.Single(t => t.FullName == "Plant");
    var target = dataT.Methods.Single(m => m.Name == "Update" && m.Parameters.Count == 0);
    if (Raw(target) != TargetToken || target.MetadataToken.RID != ExpectedRid || target.Body.CodeSize != ExpectedOldCodeSize || target.Body.Variables.Count != ExpectedOldLocals)
        throw new InvalidDataException($"PlantData.Update fingerprint drift token=0x{Raw(target):X8} rid={target.MetadataToken.RID} size={target.Body.CodeSize} locals={target.Body.Variables.Count}");
    if (!target.Body.Instructions.Any(i => i.Offset == 0x349 && i.OpCode == OpCodes.Ceq)) throw new InvalidDataException("expected IL_0349 invalid ceq marker missing");
    if (!target.Body.Instructions.Any(i => i.Operand is string s && s.Contains("non empty stack", StringComparison.Ordinal))) throw new InvalidDataException("expected damaged warning marker missing");

    FieldDefinition DF(string n) => dataT.Fields.Single(f => f.Name == n);
    FieldDefinition PF(string n) => plantT.Fields.Single(f => f.Name == n);
    var plant = DF("plant");
    var coverHP = DF("coverHP");
    var textHP = DF("textHP");
    var textATK = DF("textATK");
    var textARM = DF("textARM");
    var textDEF = DF("textDEF");
    var animator = DF("animator");
    var healthPoint = PF("healthPoint");
    var maxHealthPoint = PF("maxHealthPoint");

    var objectImplicit = FindRef(methods, m => m.DeclaringType.FullName == "UnityEngine.Object" && m.Name == "op_Implicit" && m.Parameters.Count == 1, "Object.op_Implicit");
    var mathMin = FindRef(methods, m => m.DeclaringType.FullName == "System.Math" && m.Name == "Min" && m.Parameters.Count == 2 && m.Parameters.All(p => p.ParameterType.FullName == "System.Single"), "Math.Min(float,float)");
    var mathMax = FindRef(methods, m => m.DeclaringType.FullName == "System.Math" && m.Name == "Max" && m.Parameters.Count == 2 && m.Parameters.All(p => p.ParameterType.FullName == "System.Single"), "Math.Max(float,float)");
    var truncate = FindRef(methods, m => m.DeclaringType.FullName == "System.Math" && m.Name == "Truncate" && m.Parameters.Count == 1 && m.Parameters[0].ParameterType.FullName == "System.Double", "Math.Truncate(double)");
    var doubleToString = FindRef(methods, m => m.DeclaringType.FullName == "System.Double" && m.Name == "ToString" && m.Parameters.Count == 0, "Double.ToString");
    var singleToString = FindRef(methods, m => m.DeclaringType.FullName == "System.Single" && m.Name == "ToString" && m.Parameters.Count == 0, "Single.ToString");
    var concat2 = FindRef(methods, m => m.DeclaringType.FullName == "System.String" && m.Name == "Concat" && m.Parameters.Count == 2 && m.Parameters.All(p => p.ParameterType.FullName == "System.String"), "String.Concat(string,string)");
    var concat3 = FindRef(methods, m => m.DeclaringType.FullName == "System.String" && m.Name == "Concat" && m.Parameters.Count == 3 && m.Parameters.All(p => p.ParameterType.FullName == "System.String"), "String.Concat(string,string,string)");
    var setText = FindRef(methods, m => m.DeclaringType.FullName == "TMPro.TMP_Text" && m.Name == "SetText" && m.Parameters.Count == 2 && m.Parameters[0].ParameterType.FullName == "System.String" && m.Parameters[1].ParameterType.FullName == "System.Boolean", "TMP_Text.SetText(string,bool)");
    var setFillAmount = FindRef(methods, m => m.DeclaringType.FullName == "UnityEngine.UI.Image" && m.Name == "set_fillAmount" && m.Parameters.Count == 1 && m.Parameters[0].ParameterType.FullName == "System.Single", "Image.set_fillAmount");
    var getTimeScale = FindRef(methods, m => m.DeclaringType.FullName == "UnityEngine.Time" && m.Name == "get_timeScale" && m.Parameters.Count == 0, "Time.get_timeScale");
    var setAnimatorFloat = FindRef(methods, m => m.DeclaringType.FullName == "UnityEngine.Animator" && m.Name == "SetFloat" && m.Parameters.Count == 2 && m.Parameters[0].ParameterType.FullName == "System.String" && m.Parameters[1].ParameterType.FullName == "System.Single", "Animator.SetFloat(string,float)");
    var getATK = plantT.Methods.Single(m => m.Name == "GetATK" && m.Parameters.Count == 0);
    var getARM = plantT.Methods.Single(m => m.Name == "GetARM" && m.Parameters.Count == 0);
    var getDEF = plantT.Methods.Single(m => m.Name == "GetDEF" && m.Parameters.Count == 0);

    Console.WriteLine($"NATIVE_AUTHORITY target_token=0x{TargetToken:X8} rid={ExpectedRid} method_pointer_entry=0x181B85C50 va=0x180399ED0 end=0x18039A320 native_span_sha256={NativeSpanSha}");
    Console.WriteLine("NATIVE_SEMANTICS plant_live_gate=1 coverHP_clamp=1 HP_truncated_current_max=1 ATK_live=1 ARM_live=1 DEF_live=1 animator_live=1 zero_timescale_speed_0=1 nonzero_timescale_reciprocal=1 nan_reciprocal=1");
    Console.WriteLine("RECOVERY_STRATEGY full_single_MethodDef_rebuild=1 reuse_runtime_qualified_LoadData_numeric_text_refs=1 typed_numeric_locals=1 metadata_changes=0 null_guard_changes=0 exception_swallowing=0");

    var body = target.Body;
    body.Instructions.Clear(); body.Variables.Clear(); body.ExceptionHandlers.Clear();
    body.InitLocals = true; body.MaxStackSize = 5;
    var currentHP = new VariableDefinition(module.TypeSystem.Double);
    var maxHP = new VariableDefinition(module.TypeSystem.Double);
    var stat = new VariableDefinition(module.TypeSystem.Single);
    var timeScale = new VariableDefinition(module.TypeSystem.Single);
    var speed = new VariableDefinition(module.TypeSystem.Single);
    body.Variables.Add(currentHP); body.Variables.Add(maxHP); body.Variables.Add(stat); body.Variables.Add(timeScale); body.Variables.Add(speed);
    var il = body.GetILProcessor();

    var animatorCheck = il.Create(OpCodes.Ldarg_0);
    var hpCheck = il.Create(OpCodes.Ldarg_0);
    var atkCheck = il.Create(OpCodes.Ldarg_0);
    var armCheck = il.Create(OpCodes.Ldarg_0);
    var defCheck = il.Create(OpCodes.Ldarg_0);
    var zeroSpeed = il.Create(OpCodes.Ldc_R4, 0f);
    var setSpeed = il.Create(OpCodes.Ldarg_0);
    var ret = il.Create(OpCodes.Ret);

    // if (!plant) skip stat UI and continue to animator handling.
    il.Append(il.Create(OpCodes.Ldarg_0)); il.Append(il.Create(OpCodes.Ldfld, plant)); il.Append(il.Create(OpCodes.Call, objectImplicit)); il.Append(il.Create(OpCodes.Brfalse, animatorCheck));

    // coverHP.fillAmount = Max(Min(health/max,1),0)
    il.Append(il.Create(OpCodes.Ldarg_0)); il.Append(il.Create(OpCodes.Ldfld, coverHP)); il.Append(il.Create(OpCodes.Call, objectImplicit)); il.Append(il.Create(OpCodes.Brfalse, hpCheck));
    il.Append(il.Create(OpCodes.Ldarg_0)); il.Append(il.Create(OpCodes.Ldfld, coverHP));
    il.Append(il.Create(OpCodes.Ldarg_0)); il.Append(il.Create(OpCodes.Ldfld, plant)); il.Append(il.Create(OpCodes.Ldfld, healthPoint));
    il.Append(il.Create(OpCodes.Ldarg_0)); il.Append(il.Create(OpCodes.Ldfld, plant)); il.Append(il.Create(OpCodes.Ldfld, maxHealthPoint));
    il.Append(il.Create(OpCodes.Div)); il.Append(il.Create(OpCodes.Ldc_R4, 1f)); il.Append(il.Create(OpCodes.Call, mathMin)); il.Append(il.Create(OpCodes.Ldc_R4, 0f)); il.Append(il.Create(OpCodes.Call, mathMax)); il.Append(il.Create(OpCodes.Call, setFillAmount));

    // HP text: truncated current/max.
    il.Append(hpCheck); il.Append(il.Create(OpCodes.Ldfld, textHP)); il.Append(il.Create(OpCodes.Call, objectImplicit)); il.Append(il.Create(OpCodes.Brfalse, atkCheck));
    il.Append(il.Create(OpCodes.Ldarg_0)); il.Append(il.Create(OpCodes.Ldfld, plant)); il.Append(il.Create(OpCodes.Ldfld, healthPoint)); il.Append(il.Create(OpCodes.Conv_R8)); il.Append(il.Create(OpCodes.Call, truncate)); il.Append(il.Create(OpCodes.Stloc, currentHP));
    il.Append(il.Create(OpCodes.Ldarg_0)); il.Append(il.Create(OpCodes.Ldfld, plant)); il.Append(il.Create(OpCodes.Ldfld, maxHealthPoint)); il.Append(il.Create(OpCodes.Conv_R8)); il.Append(il.Create(OpCodes.Call, truncate)); il.Append(il.Create(OpCodes.Stloc, maxHP));
    il.Append(il.Create(OpCodes.Ldarg_0)); il.Append(il.Create(OpCodes.Ldfld, textHP));
    il.Append(il.Create(OpCodes.Ldloca, currentHP)); il.Append(il.Create(OpCodes.Call, doubleToString)); il.Append(il.Create(OpCodes.Ldstr, "/")); il.Append(il.Create(OpCodes.Ldloca, maxHP)); il.Append(il.Create(OpCodes.Call, doubleToString)); il.Append(il.Create(OpCodes.Call, concat3)); il.Append(il.Create(OpCodes.Ldc_I4_1)); il.Append(il.Create(OpCodes.Call, setText));

    void EmitStat(Instruction entry, FieldReference textField, MethodReference getter, string prefix, Instruction next)
    {
        il.Append(entry); il.Append(il.Create(OpCodes.Ldfld, textField)); il.Append(il.Create(OpCodes.Call, objectImplicit)); il.Append(il.Create(OpCodes.Brfalse, next));
        il.Append(il.Create(OpCodes.Ldarg_0)); il.Append(il.Create(OpCodes.Ldfld, plant)); il.Append(il.Create(OpCodes.Call, getter)); il.Append(il.Create(OpCodes.Stloc, stat));
        il.Append(il.Create(OpCodes.Ldarg_0)); il.Append(il.Create(OpCodes.Ldfld, textField)); il.Append(il.Create(OpCodes.Ldstr, prefix)); il.Append(il.Create(OpCodes.Ldloca, stat)); il.Append(il.Create(OpCodes.Call, singleToString)); il.Append(il.Create(OpCodes.Call, concat2)); il.Append(il.Create(OpCodes.Ldc_I4_1)); il.Append(il.Create(OpCodes.Call, setText));
    }
    EmitStat(atkCheck, textATK, getATK, "攻击:", armCheck);
    EmitStat(armCheck, textARM, getARM, "护甲:", defCheck);
    EmitStat(defCheck, textDEF, getDEF, "防御:", animatorCheck);

    // Animator speed: zero at exactly 0; otherwise 1 / current Time.timeScale. NaN is not equal to 0 and follows reciprocal path.
    il.Append(animatorCheck); il.Append(il.Create(OpCodes.Ldfld, animator)); il.Append(il.Create(OpCodes.Call, objectImplicit)); il.Append(il.Create(OpCodes.Brfalse, ret));
    il.Append(il.Create(OpCodes.Call, getTimeScale)); il.Append(il.Create(OpCodes.Stloc, timeScale));
    il.Append(il.Create(OpCodes.Ldloc, timeScale)); il.Append(il.Create(OpCodes.Ldc_R4, 0f)); il.Append(il.Create(OpCodes.Ceq)); il.Append(il.Create(OpCodes.Brtrue, zeroSpeed));
    il.Append(il.Create(OpCodes.Ldc_R4, 1f)); il.Append(il.Create(OpCodes.Call, getTimeScale)); il.Append(il.Create(OpCodes.Div)); il.Append(il.Create(OpCodes.Stloc, speed)); il.Append(il.Create(OpCodes.Br, setSpeed));
    il.Append(zeroSpeed); il.Append(il.Create(OpCodes.Stloc, speed));
    il.Append(setSpeed); il.Append(il.Create(OpCodes.Ldfld, animator)); il.Append(il.Create(OpCodes.Ldstr, "speed")); il.Append(il.Create(OpCodes.Ldloc, speed)); il.Append(il.Create(OpCodes.Call, setAnimatorFloat));
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
    if (target.Body.Variables.Count != 5 || target.Body.Variables.Any(v => v.VariableType.FullName == "System.Object")) throw new InvalidDataException("typed local gate failed");
    if (Calls("UnityEngine.Object","op_Implicit") != 7 || Calls("System.Math","Min") != 1 || Calls("System.Math","Max") != 1 || Calls("System.Math","Truncate") != 2 || Calls("System.Double","ToString") != 2 || Calls("System.Single","ToString") != 3 || Calls("System.String","Concat") != 4 || Calls("TMPro.TMP_Text","SetText") != 4 || Calls("UnityEngine.UI.Image","set_fillAmount") != 1 || Calls("Plant","GetATK") != 1 || Calls("Plant","GetARM") != 1 || Calls("Plant","GetDEF") != 1 || Calls("UnityEngine.Time","get_timeScale") != 2 || Calls("UnityEngine.Animator","SetFloat") != 1)
        throw new InvalidDataException("reopen call-count mismatch");
    if (target.Body.Instructions.Count(i => i.OpCode == OpCodes.Ceq) != 1) throw new InvalidDataException("expected one valid float-zero ceq");
    if (target.Body.Instructions.Any(i => i.Operand is string s && (s.Contains("non empty stack", StringComparison.Ordinal) || s.Contains("Not implemented instruction", StringComparison.Ordinal)))) throw new InvalidDataException("damaged marker survived");
    var changedM = methods.Where(m => beforeM[Raw(m)] != MethodSig(m)).Select(m => Raw(m)).OrderBy(x => x).ToList();
    if (changedM.Count != 1 || changedM[0] != TargetToken) throw new InvalidDataException("semantic isolation failed: " + string.Join(',', changedM.Select(x => $"0x{x:X8}")));
    var changedF = fields.Where(f => beforeF[Raw(f)] != FieldSig(f)).Select(f => Raw(f)).ToList();
    if (changedF.Count != 0) throw new InvalidDataException("field metadata drift");
    Console.WriteLine($"REOPEN_PLANTDATA_UPDATE_PASS token=0x{TargetToken:X8} code_size={target.Body.CodeSize} locals=5 object_locals=0 valid_float_ceq=1 Object_Implicit=7 Truncate=2 Double_ToString=2 Single_ToString=3 SetText=4 GetATK=1 GetARM=1 GetDEF=1 TimeScale=2 Animator_SetFloat=1");
    Console.WriteLine($"SEMANTIC_ISOLATION_PASS untouched_methods={ExpectedMethods-1} changed_methods=1 target=0x{TargetToken:X8}");
    Console.WriteLine($"FIELD_METADATA_ISOLATION_PASS unchanged_fields={ExpectedFields} changed_fields=0");
    Console.WriteLine("FRAMEWORK_REFERENCE_GATE_PASS system_private_corelib_refs=0 assembly_reference_set_unchanged=1");
}

return 0;
