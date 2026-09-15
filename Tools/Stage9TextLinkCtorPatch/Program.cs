using Mono.Cecil;
using Mono.Cecil.Cil;
using System.Globalization;
using System.Security.Cryptography;
using System.Text;

if (args.Length != 3)
{
    Console.Error.WriteLine("Usage: Stage9TextLinkCtorPatch <input.dll> <output.dll> <expected-input-sha256>");
    return 2;
}

const uint TargetToken = 0x060006E5;
const int ExpectedRid = 1765;
const uint LinkIndexToken = 0x040008E2;
const uint LastLinkIndexToken = 0x040008E3;
const uint HoverColorToken = 0x040008E4;
const uint OriginalColorToken = 0x040008E5;
const int ExpectedMethods = 2317;
const int ExpectedFields = 2802;
const int ExpectedOldCodeSize = 46;
const int ExpectedOldLocals = 1;
const string NativeSha = "699419627a7893a66b38156beaf1ff5b0fe8530e3015e6fab90d976d7b70669c";

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
static string RefSig(MethodReference m) => $"{m.FullName}@{Scope(m.DeclaringType.Scope)}";
static string OpSig(object? o, MethodDefinition owner)
{
    if (o is null) return "";
    if (o is Instruction i) return $"I#{owner.Body.Instructions.IndexOf(i)}";
    if (o is Instruction[] sw) return "SW[" + string.Join(',', sw.Select(i => owner.Body.Instructions.IndexOf(i))) + "]";
    if (o is MethodReference mr) return $"M:{RefSig(mr)}";
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
static List<MethodReference> MethodRefs(IEnumerable<MethodDefinition> methods) => methods.Where(m => m.HasBody)
    .SelectMany(m => m.Body.Instructions).Select(i => i.Operand).OfType<MethodReference>().ToList();
static string UniqueMethodRefSet(IEnumerable<MethodDefinition> methods) => string.Join("\n", MethodRefs(methods).Select(RefSig).Distinct(StringComparer.Ordinal).OrderBy(x => x, StringComparer.Ordinal));
static MethodReference FindUniqueRef(IEnumerable<MethodDefinition> methods, Func<MethodReference,bool> pred, string label)
{
    var hits = MethodRefs(methods).Where(pred).GroupBy(RefSig, StringComparer.Ordinal).Select(g => g.First()).ToList();
    if (hits.Count != 1) throw new InvalidDataException($"{label} refs={hits.Count}: {string.Join(" | ", hits.Select(RefSig))}");
    return hits[0];
}

var input = Path.GetFullPath(args[0]);
var output = Path.GetFullPath(args[1]);
var expectedInputSha = args[2].Trim().ToLowerInvariant();
if (expectedInputSha.Length != 64 || expectedInputSha.Any(c => !Uri.IsHexDigit(c))) throw new InvalidDataException("expected input SHA invalid");
if (Sha(input) != expectedInputSha) throw new InvalidDataException("input SHA mismatch");
Console.WriteLine($"INPUT_SHA256 {Sha(input)}");

var beforeM = new Dictionary<uint,string>();
var beforeF = new Dictionary<uint,string>();
string beforeAssemblyRefs;
string beforeMethodRefs;
using (var module = ModuleDefinition.ReadModule(input, new ReaderParameters { InMemory=true, ReadingMode=ReadingMode.Immediate }))
{
    var types = AllTypes(module.Types).ToList();
    var methods = types.SelectMany(t => t.Methods).ToList();
    var fields = types.SelectMany(t => t.Fields).ToList();
    if (methods.Count != ExpectedMethods || fields.Count != ExpectedFields) throw new InvalidDataException("metadata count drift");
    if (module.AssemblyReferences.Any(a => a.Name == "System.Private.CoreLib")) throw new InvalidDataException("input references System.Private.CoreLib");
    foreach (var m in methods) beforeM[Raw(m)] = MethodSig(m);
    foreach (var f in fields) beforeF[Raw(f)] = FieldSig(f);
    beforeAssemblyRefs = string.Join("\n", module.AssemblyReferences.Select(a => a.FullName).OrderBy(x => x, StringComparer.Ordinal));
    beforeMethodRefs = UniqueMethodRefSet(methods);

    var t = types.Single(x => x.FullName == "TextLink");
    var target = methods.Single(m => Raw(m) == TargetToken);
    if (target.DeclaringType != t || !target.IsConstructor || target.IsStatic || target.Parameters.Count != 0)
        throw new InvalidDataException("target ctor identity drift");
    if (target.MetadataToken.RID != ExpectedRid || !target.HasBody || target.Body.CodeSize != ExpectedOldCodeSize || target.Body.Variables.Count != ExpectedOldLocals)
        throw new InvalidDataException($"target body fingerprint drift rid={target.MetadataToken.RID} size={target.Body.CodeSize} locals={target.Body.Variables.Count}");

    FieldDefinition F(uint token, string name, string type)
    {
        var f = fields.Single(x => Raw(x) == token);
        if (f.DeclaringType != t || f.Name != name || f.FieldType.FullName != type) throw new InvalidDataException($"field identity drift 0x{token:X8}");
        return f;
    }
    var linkIndex = F(LinkIndexToken, "linkIndex", "System.Int32");
    var lastLinkIndex = F(LastLinkIndexToken, "lastLinkIndex", "System.Int32");
    var hoverColor = F(HoverColorToken, "hoverColor", "UnityEngine.Color");
    var originalColor = F(OriginalColorToken, "originalColor", "UnityEngine.Color");

    var fakeLoads = target.Body.Instructions.Count(i => i.Operand is string s && s == "Unmanaged memory load: [1815A7B70]");
    var convIs = target.Body.Instructions.Count(i => i.OpCode == OpCodes.Conv_I);
    var linkMinusOne = target.Body.Instructions.Count(i => i.OpCode == OpCodes.Ldc_I4 && i.Operand is int n && n == -1);
    var lastStores = target.Body.Instructions.Count(i => i.OpCode == OpCodes.Stfld && i.Operand is FieldReference fr && fr.Name == "lastLinkIndex" && fr.DeclaringType.FullName == "TextLink");
    if (fakeLoads != 2 || convIs != 2 || linkMinusOne != 1 || lastStores != 0) throw new InvalidDataException($"damaged fingerprint drift fake={fakeLoads} convi={convIs} minus1={linkMinusOne} lastStores={lastStores}");

    var colorCtor = FindUniqueRef(methods, m => m.DeclaringType.FullName == "UnityEngine.Color" && m.Name == ".ctor" && m.HasThis && m.Parameters.Count == 4 && m.Parameters.All(p => p.ParameterType.FullName == "System.Single") && m.ReturnType.FullName == "System.Void" && Scope(m.DeclaringType.Scope) == "UnityEngine.CoreModule", "Color rgba ctor");
    var monoCtor = FindUniqueRef(methods, m => m.DeclaringType.FullName == "UnityEngine.MonoBehaviour" && m.Name == ".ctor" && m.HasThis && m.Parameters.Count == 0 && Scope(m.DeclaringType.Scope) == "UnityEngine.CoreModule", "MonoBehaviour ctor");

    Console.WriteLine($"NATIVE_AUTHORITY target_token=0x{TargetToken:X8} rid={ExpectedRid} method_pointer_entry=0x181B86480 va=0x1803ADFA0 end=0x1803ADFBF native_slice_sha256={NativeSha} color_constant_va=0x1815A7B70 rgba=1,1,1,1");
    Console.WriteLine("NATIVE_SEMANTICS linkIndex=-1 lastLinkIndex=-1 hoverColor=Color.white originalColor=Color.white base_ctor=UnityEngine.MonoBehaviour::.ctor");
    Console.WriteLine("RECOVERY_STRATEGY full_single_MethodDef_rebuild=1 reuse_existing_Color_RGBA_ctor_ref=1 reuse_existing_MonoBehaviour_ctor_ref=1 new_method_refs=0 metadata_changes=0 guards=0 exception_swallowing=0");

    var body = target.Body;
    body.Instructions.Clear(); body.Variables.Clear(); body.ExceptionHandlers.Clear();
    body.InitLocals = false; body.MaxStackSize = 5;
    var il = body.GetILProcessor();
    void EmitWhite(FieldDefinition field)
    {
        il.Append(il.Create(OpCodes.Ldarg_0));
        il.Append(il.Create(OpCodes.Ldc_R4, 1f));
        il.Append(il.Create(OpCodes.Ldc_R4, 1f));
        il.Append(il.Create(OpCodes.Ldc_R4, 1f));
        il.Append(il.Create(OpCodes.Ldc_R4, 1f));
        il.Append(il.Create(OpCodes.Newobj, colorCtor));
        il.Append(il.Create(OpCodes.Stfld, field));
    }
    il.Append(il.Create(OpCodes.Ldarg_0));
    il.Append(il.Create(OpCodes.Ldc_I4_M1));
    il.Append(il.Create(OpCodes.Stfld, linkIndex));
    il.Append(il.Create(OpCodes.Ldarg_0));
    il.Append(il.Create(OpCodes.Ldc_I4_M1));
    il.Append(il.Create(OpCodes.Stfld, lastLinkIndex));
    EmitWhite(hoverColor);
    EmitWhite(originalColor);
    il.Append(il.Create(OpCodes.Ldarg_0));
    il.Append(il.Create(OpCodes.Call, monoCtor));
    il.Append(il.Create(OpCodes.Ret));
    Directory.CreateDirectory(Path.GetDirectoryName(output)!);
    module.Write(output);
}

Console.WriteLine($"OUTPUT_SHA256 {Sha(output)}");
using (var module = ModuleDefinition.ReadModule(output, new ReaderParameters { InMemory=true, ReadingMode=ReadingMode.Immediate }))
{
    var types = AllTypes(module.Types).ToList(); var methods = types.SelectMany(t => t.Methods).ToList(); var fields = types.SelectMany(t => t.Fields).ToList();
    if (methods.Count != ExpectedMethods || fields.Count != ExpectedFields) throw new InvalidDataException("metadata count drift after reopen");
    var target = methods.Single(m => Raw(m) == TargetToken);
    if (target.Body.Variables.Count != 0 || target.Body.ExceptionHandlers.Count != 0 || target.Body.InitLocals) throw new InvalidDataException("rebuild locals/handlers/initlocals drift");
    var ins = target.Body.Instructions;
    if (ins.Count != 23) throw new InvalidDataException($"rebuild instruction count={ins.Count}");
    int StoreCount(uint tok) => ins.Count(i => i.OpCode == OpCodes.Stfld && i.Operand is FieldReference fr && fr.Resolve() is FieldDefinition fd && Raw(fd) == tok);
    if (StoreCount(LinkIndexToken) != 1 || StoreCount(LastLinkIndexToken) != 1 || StoreCount(HoverColorToken) != 1 || StoreCount(OriginalColorToken) != 1) throw new InvalidDataException("target field store counts drift");
    if (ins.Count(i => i.OpCode == OpCodes.Ldc_I4_M1) != 2) throw new InvalidDataException("two Int32 -1 constants missing");
    if (ins.Count(i => i.OpCode == OpCodes.Ldc_R4 && i.Operand is float f && BitConverter.SingleToInt32Bits(f) == BitConverter.SingleToInt32Bits(1f)) != 8) throw new InvalidDataException("eight exact 1.0f constants missing");
    if (ins.Count(i => i.OpCode == OpCodes.Newobj && i.Operand is MethodReference mr && mr.DeclaringType.FullName == "UnityEngine.Color" && mr.Name == ".ctor" && mr.Parameters.Count == 4 && mr.Parameters.All(p => p.ParameterType.FullName == "System.Single") && Scope(mr.DeclaringType.Scope) == "UnityEngine.CoreModule") != 2) throw new InvalidDataException("Color rgba ctor count drift");
    if (ins.Count(i => i.OpCode == OpCodes.Call && i.Operand is MethodReference mr && mr.DeclaringType.FullName == "UnityEngine.MonoBehaviour" && mr.Name == ".ctor" && mr.Parameters.Count == 0 && Scope(mr.DeclaringType.Scope) == "UnityEngine.CoreModule") != 1) throw new InvalidDataException("MonoBehaviour ctor count drift");
    var text = string.Join("\n", ins.Select(i => $"{i.OpCode} {i.Operand}"));
    foreach (var bad in new[]{"Unmanaged memory load:","System.Private.CoreLib"}) if (text.Contains(bad,StringComparison.Ordinal)) throw new InvalidDataException("corruption remains: "+bad);
    if (ins.Any(i => i.OpCode == OpCodes.Conv_I || i.OpCode == OpCodes.Ldc_I8)) throw new InvalidDataException("native-int/I8 corruption remains");

    var changedM = methods.Where(m => beforeM[Raw(m)] != MethodSig(m)).Select(Raw).OrderBy(x => x).ToList();
    if (changedM.Count != 1 || changedM[0] != TargetToken) throw new InvalidDataException("semantic isolation failed: "+string.Join(',',changedM.Select(x=>$"0x{x:X8}")));
    var changedF = fields.Where(f => beforeF[Raw(f)] != FieldSig(f)).Select(Raw).ToList();
    if (changedF.Count != 0) throw new InvalidDataException("field metadata isolation failed");
    var afterAssemblyRefs = string.Join("\n", module.AssemblyReferences.Select(a => a.FullName).OrderBy(x => x, StringComparer.Ordinal));
    if (beforeAssemblyRefs != afterAssemblyRefs) throw new InvalidDataException("assembly reference set changed");
    var afterMethodRefs = UniqueMethodRefSet(methods);
    if (beforeMethodRefs != afterMethodRefs) throw new InvalidDataException("unique method reference set changed");
    if (module.AssemblyReferences.Any(a => a.Name == "System.Private.CoreLib")) throw new InvalidDataException("System.Private.CoreLib pollution");
    Console.WriteLine($"REOPEN_TEXTLINK_CTOR_PASS token=0x{TargetToken:X8} linkIndex_token=0x{LinkIndexToken:X8} lastLinkIndex_token=0x{LastLinkIndexToken:X8} hoverColor_token=0x{HoverColorToken:X8} originalColor_token=0x{OriginalColorToken:X8} int32_minus_one=2 color_white_rgba=2 base_ctor=1 fake_diagnostics=0");
    Console.WriteLine($"SEMANTIC_ISOLATION_PASS untouched_methods={ExpectedMethods-1} changed_methods=1 target=0x{TargetToken:X8}");
    Console.WriteLine($"FIELD_METADATA_ISOLATION_PASS unchanged_fields={ExpectedFields} changed_fields=0");
    Console.WriteLine("FRAMEWORK_REFERENCE_GATE_PASS system_private_corelib_refs=0 assembly_reference_set_unchanged=1 unique_method_ref_set_unchanged=1");
}
return 0;
