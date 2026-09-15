using Mono.Cecil;
using Mono.Cecil.Cil;
using System.Globalization;
using System.Security.Cryptography;
using System.Text;

if (args.Length != 3)
{
    Console.Error.WriteLine("Usage: Stage9LevelItemCtorPatch <input.dll> <output.dll> <expected-input-sha256>");
    return 2;
}

const uint TargetToken = 0x0600062D;
const int ExpectedRid = 1581;
const uint RescueSeedFieldToken = 0x04000838;
const int ExpectedMethods = 2317;
const int ExpectedFields = 2802;
const int ExpectedOldCodeSize = 675;
const int ExpectedOldLocals = 17;
const string NativeSha = "00de368a2072ce9b97ae6e5211dd9262b61537bbd1be1d608638c09cdf7e295c";

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
static string MethodRefSig(MethodReference m) => $"{m.FullName}@{Scope(m.DeclaringType.Scope)}";
static string OpSig(object? o, MethodDefinition owner)
{
    if (o is null) return "";
    if (o is Instruction i) return $"I#{owner.Body.Instructions.IndexOf(i)}";
    if (o is Instruction[] sw) return "SW[" + string.Join(',', sw.Select(i => owner.Body.Instructions.IndexOf(i))) + "]";
    if (o is MethodReference mr) return "M:" + MethodRefSig(mr);
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
static List<MethodReference> UsedMethodRefs(IEnumerable<MethodDefinition> methods) => methods.Where(m => m.HasBody)
    .SelectMany(m => m.Body.Instructions).Select(i => i.Operand).OfType<MethodReference>()
    .GroupBy(MethodRefSig, StringComparer.Ordinal).Select(g => g.First()).ToList();
static MethodReference SingleRef(IEnumerable<MethodReference> refs, Func<MethodReference,bool> pred, string label)
{
    var x = refs.Where(pred).GroupBy(MethodRefSig, StringComparer.Ordinal).Select(g => g.First()).ToList();
    if (x.Count != 1) throw new InvalidDataException($"{label} refs={x.Count}");
    return x[0];
}

var input = Path.GetFullPath(args[0]);
var output = Path.GetFullPath(args[1]);
var expectedInputSha = args[2].Trim().ToLowerInvariant();
if (expectedInputSha.Length != 64 || expectedInputSha.Any(c => !Uri.IsHexDigit(c))) throw new InvalidDataException("expected input SHA invalid");
var inputSha = Sha(input);
if (inputSha != expectedInputSha) throw new InvalidDataException($"input SHA mismatch actual={inputSha} expected={expectedInputSha}");
Console.WriteLine($"INPUT_SHA256 {inputSha}");

var beforeM = new Dictionary<uint,string>();
var beforeF = new Dictionary<uint,string>();
HashSet<string> beforeUsedMethodRefs;
string beforeAssemblyRefs;

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
    beforeUsedMethodRefs = UsedMethodRefs(methods).Select(MethodRefSig).ToHashSet(StringComparer.Ordinal);

    var t = types.Single(x => x.FullName == "LevelItem");
    var target = methods.Single(m => Raw(m) == TargetToken);
    if (target.DeclaringType != t || !target.IsConstructor || target.IsStatic || target.Parameters.Count != 0)
        throw new InvalidDataException("target ctor identity drift");
    if (target.MetadataToken.RID != ExpectedRid || !target.HasBody || target.Body.CodeSize != ExpectedOldCodeSize || target.Body.Variables.Count != ExpectedOldLocals || target.Body.ExceptionHandlers.Count != 0)
        throw new InvalidDataException($"target body fingerprint drift rid={target.MetadataToken.RID} size={target.Body.CodeSize} locals={target.Body.Variables.Count} handlers={target.Body.ExceptionHandlers.Count}");
    var rescue = fields.Single(f => Raw(f) == RescueSeedFieldToken);
    if (rescue.DeclaringType != t || rescue.Name != "rescueSeedID" || rescue.FieldType.FullName != "System.Collections.Generic.List`1<System.Int32>")
        throw new InvalidDataException("rescueSeedID field identity drift");

    var refs = UsedMethodRefs(methods);
    var listCtor = SingleRef(refs, m => m.DeclaringType.FullName == "System.Collections.Generic.List`1<System.Int32>" && m.Name == ".ctor" && m.HasThis && m.Parameters.Count == 0, "List<int>::.ctor");
    var listAdd = SingleRef(refs, m => m.DeclaringType.FullName == "System.Collections.Generic.List`1<System.Int32>" && m.Name == "Add" && m.HasThis && m.Parameters.Count == 1, "List<int>::Add");
    var monoCtor = SingleRef(refs, m => m.DeclaringType.FullName == "UnityEngine.MonoBehaviour" && m.Name == ".ctor" && m.HasThis && m.Parameters.Count == 0, "MonoBehaviour::.ctor");

    int unmanaged = target.Body.Instructions.Count(i => i.Operand is string s && s.Contains("Unmanaged memory load:", StringComparison.Ordinal));
    int badI8 = target.Body.Instructions.Count(i => i.OpCode == OpCodes.Ldc_I8 && i.Operand is long n && n == 4294967295L);
    int addResize = target.Body.Instructions.Count(i => i.Operand is MethodReference m && m.Name == "AddWithResize");
    if (unmanaged != 25 || badI8 != 2 || addResize != 5)
        throw new InvalidDataException($"damaged-body fingerprint drift unmanaged={unmanaged} i8={badI8} addWithResize={addResize}");

    Console.WriteLine($"NATIVE_AUTHORITY target_token=0x{TargetToken:X8} rid={ExpectedRid} method_pointer_entry=0x181B85EC0 va=0x18039F130 end=0x18039F365 native_slice_sha256={NativeSha}");
    Console.WriteLine("NATIVE_SEMANTICS rescueSeedID=List<int>{-1,3,5,6,7} base_ctor=UnityEngine.MonoBehaviour::.ctor");
    Console.WriteLine("RECOVERY_STRATEGY full_single_MethodDef_rebuild=1 reuse_existing_ListInt_ctor_ref=1 reuse_existing_ListInt_Add_ref=1 reuse_existing_MonoBehaviour_ctor_ref=1 new_method_refs=0 metadata_changes=0 guards=0 exception_swallowing=0");

    var body = target.Body;
    body.Instructions.Clear();
    body.Variables.Clear();
    body.ExceptionHandlers.Clear();
    body.InitLocals = false;
    body.MaxStackSize = 4;
    var il = body.GetILProcessor();
    il.Append(il.Create(OpCodes.Ldarg_0));
    il.Append(il.Create(OpCodes.Newobj, listCtor));
    foreach (var v in new[] { -1, 3, 5, 6, 7 })
    {
        il.Append(il.Create(OpCodes.Dup));
        il.Append(il.Create(v == -1 ? OpCodes.Ldc_I4_M1 : v == 3 ? OpCodes.Ldc_I4_3 : v == 5 ? OpCodes.Ldc_I4_5 : v == 6 ? OpCodes.Ldc_I4_6 : OpCodes.Ldc_I4_7));
        il.Append(il.Create(OpCodes.Callvirt, listAdd));
    }
    il.Append(il.Create(OpCodes.Stfld, rescue));
    il.Append(il.Create(OpCodes.Ldarg_0));
    il.Append(il.Create(OpCodes.Call, monoCtor));
    il.Append(il.Create(OpCodes.Ret));

    Directory.CreateDirectory(Path.GetDirectoryName(output)!);
    module.Write(output);
}

Console.WriteLine($"OUTPUT_SHA256 {Sha(output)}");
using (var module = ModuleDefinition.ReadModule(output, new ReaderParameters { InMemory=true, ReadingMode=ReadingMode.Immediate }))
{
    var types = AllTypes(module.Types).ToList();
    var methods = types.SelectMany(t => t.Methods).ToList();
    var fields = types.SelectMany(t => t.Fields).ToList();
    if (methods.Count != ExpectedMethods || fields.Count != ExpectedFields) throw new InvalidDataException("metadata count drift after reopen");
    var target = methods.Single(m => Raw(m) == TargetToken);
    var rescue = fields.Single(f => Raw(f) == RescueSeedFieldToken);
    if (target.Body.Variables.Count != 0 || target.Body.ExceptionHandlers.Count != 0 || target.Body.InitLocals) throw new InvalidDataException("rebuild locals/handlers/initlocals drift");
    var ins = target.Body.Instructions;
    if (ins.Count != 21) throw new InvalidDataException($"rebuild instruction count={ins.Count}");
    if (!(ins[0].OpCode == OpCodes.Ldarg_0 && ins[1].OpCode == OpCodes.Newobj && ins[1].Operand is MethodReference lc && lc.DeclaringType.FullName == "System.Collections.Generic.List`1<System.Int32>" && lc.Name == ".ctor" && lc.Parameters.Count == 0))
        throw new InvalidDataException("List<int> ctor sequence drift");
    int[] constants = { -1, 3, 5, 6, 7 };
    for (int j = 0; j < 5; j++)
    {
        int k = 2 + j * 3;
        if (ins[k].OpCode != OpCodes.Dup) throw new InvalidDataException($"dup drift at add {j}");
        int? val = ins[k+1].OpCode.Code switch { Code.Ldc_I4_M1 => -1, Code.Ldc_I4_3 => 3, Code.Ldc_I4_5 => 5, Code.Ldc_I4_6 => 6, Code.Ldc_I4_7 => 7, _ => null };
        if (val != constants[j]) throw new InvalidDataException($"constant drift at add {j}: {val}");
        if (!(ins[k+2].OpCode == OpCodes.Callvirt && ins[k+2].Operand is MethodReference a && a.DeclaringType.FullName == "System.Collections.Generic.List`1<System.Int32>" && a.Name == "Add" && a.Parameters.Count == 1))
            throw new InvalidDataException($"List<int>.Add drift at add {j}");
    }
    if (!(ins[17].OpCode == OpCodes.Stfld && ins[17].Operand is FieldReference rf && rf.Resolve() is FieldDefinition rfd && Raw(rfd) == RescueSeedFieldToken))
        throw new InvalidDataException("rescueSeedID store drift");
    if (!(ins[18].OpCode == OpCodes.Ldarg_0 && ins[19].OpCode == OpCodes.Call && ins[19].Operand is MethodReference mc && mc.DeclaringType.FullName == "UnityEngine.MonoBehaviour" && mc.Name == ".ctor" && mc.Parameters.Count == 0 && ins[20].OpCode == OpCodes.Ret))
        throw new InvalidDataException("MonoBehaviour ctor/ret drift");

    var targetText = string.Join("\n", ins.Select(i => $"{i.OpCode} {i.Operand}"));
    foreach (var bad in new[] { "Unmanaged memory load:", "AddWithResize", "System.Private.CoreLib" })
        if (targetText.Contains(bad, StringComparison.Ordinal)) throw new InvalidDataException("corruption remains: " + bad);

    var changedM = methods.Where(m => beforeM[Raw(m)] != MethodSig(m)).Select(Raw).OrderBy(x => x).ToList();
    if (changedM.Count != 1 || changedM[0] != TargetToken) throw new InvalidDataException("semantic isolation failed: " + string.Join(',', changedM.Select(x => $"0x{x:X8}")));
    var changedF = fields.Where(f => beforeF[Raw(f)] != FieldSig(f)).Select(Raw).ToList();
    if (changedF.Count != 0) throw new InvalidDataException("field metadata isolation failed");
    var afterAssemblyRefs = string.Join("\n", module.AssemblyReferences.Select(a => a.FullName).OrderBy(x => x, StringComparer.Ordinal));
    if (beforeAssemblyRefs != afterAssemblyRefs) throw new InvalidDataException("assembly reference set changed");
    if (module.AssemblyReferences.Any(a => a.Name == "System.Private.CoreLib")) throw new InvalidDataException("System.Private.CoreLib pollution");
    var afterUsedMethodRefs = UsedMethodRefs(methods).Select(MethodRefSig).ToHashSet(StringComparer.Ordinal);
    var newRefs = afterUsedMethodRefs.Except(beforeUsedMethodRefs, StringComparer.Ordinal).OrderBy(x => x, StringComparer.Ordinal).ToList();
    if (newRefs.Count != 0) throw new InvalidDataException("new used method refs introduced: " + string.Join(" | ", newRefs));

    Console.WriteLine($"REOPEN_LEVELITEM_CTOR_PASS token=0x{TargetToken:X8} rescueSeed_token=0x{RescueSeedFieldToken:X8} list_values=-1,3,5,6,7 public_add_calls=5 base_ctor=1 fake_diagnostics=0 addWithResize=0");
    Console.WriteLine($"SEMANTIC_ISOLATION_PASS untouched_methods={ExpectedMethods-1} changed_methods=1 target=0x{TargetToken:X8}");
    Console.WriteLine($"FIELD_METADATA_ISOLATION_PASS unchanged_fields={ExpectedFields} changed_fields=0");
    Console.WriteLine("FRAMEWORK_REFERENCE_GATE_PASS system_private_corelib_refs=0 assembly_reference_set_unchanged=1 new_used_method_refs=0");
}
return 0;
