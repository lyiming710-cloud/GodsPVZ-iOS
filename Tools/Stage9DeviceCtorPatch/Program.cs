using Mono.Cecil;
using Mono.Cecil.Cil;
using System.Globalization;
using System.Security.Cryptography;
using System.Text;

if (args.Length != 3)
{
    Console.Error.WriteLine("Usage: Stage9DeviceCtorPatch <input.dll> <output.dll> <expected-input-sha256>");
    return 2;
}

const uint TargetToken = 0x0600033A;
const int ExpectedRid = 826;
const int ExpectedMethods = 2317;
const int ExpectedFields = 2802;
const int ExpectedOldCodeSize = 205;
const int ExpectedOldLocals = 6;
const string NativeSpanSha = "40738cc7ee1cd160439bb04745a2279b7509e202ccc6e91d9cd7e6888fd2f699";

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
    return sb.ToString();
}
static string FieldSig(FieldDefinition f)
{
    var attrs = string.Join(',', f.CustomAttributes.Select(a => a.AttributeType.FullName).OrderBy(x => x, StringComparer.Ordinal));
    var c = f.HasConstant ? Convert.ToString(f.Constant, CultureInfo.InvariantCulture) ?? "<null>" : "<none>";
    return $"{f.FullName}|{f.Attributes}|{TypeSig(f.FieldType)}|const={c}|ca={attrs}";
}
static string ListCtorSig(MethodDefinition m) => string.Join("\n", m.Body.Instructions
    .Where(i => i.OpCode == OpCodes.Newobj)
    .Select(i => i.Operand).OfType<MethodReference>()
    .Where(x => x.Name == ".ctor" && x.Parameters.Count == 0 && x.DeclaringType.FullName.StartsWith("System.Collections.Generic.List`1", StringComparison.Ordinal))
    .Select(x => x.FullName + "@" + Scope(x.DeclaringType.Scope)));

var input = Path.GetFullPath(args[0]);
var output = Path.GetFullPath(args[1]);
var expectedInputSha = args[2].Trim().ToLowerInvariant();
if (expectedInputSha.Length != 64 || expectedInputSha.Any(c => !Uri.IsHexDigit(c))) throw new InvalidDataException("expected input SHA is not 64 hex chars");
if (Sha(input) != expectedInputSha) throw new InvalidDataException("input SHA mismatch");
Console.WriteLine($"INPUT_SHA256 {Sha(input)}");

var beforeM = new Dictionary<uint,string>();
var beforeF = new Dictionary<uint,string>();
string beforeRefs;
string beforeListCtors;
using (var module = ModuleDefinition.ReadModule(input, new ReaderParameters { InMemory=true, ReadingMode=ReadingMode.Immediate }))
{
    var types = AllTypes(module.Types).ToList();
    var methods = types.SelectMany(t => t.Methods).ToList();
    var fields = types.SelectMany(t => t.Fields).ToList();
    if (methods.Count != ExpectedMethods || fields.Count != ExpectedFields) throw new InvalidDataException("metadata count drift");
    if (module.AssemblyReferences.Any(a => a.Name == "System.Private.CoreLib")) throw new InvalidDataException("input references System.Private.CoreLib");
    foreach (var m in methods) beforeM[Raw(m)] = MethodSig(m);
    foreach (var field in fields) beforeF[Raw(field)] = FieldSig(field);
    beforeRefs = string.Join("\n", module.AssemblyReferences.Select(a => a.FullName).OrderBy(x => x, StringComparer.Ordinal));

    var t = types.Single(x => x.FullName == "Device");
    var target = t.Methods.Single(m => m.IsConstructor && !m.IsStatic && m.Parameters.Count == 0);
    if (Raw(target) != TargetToken || target.MetadataToken.RID != ExpectedRid || target.Body.CodeSize != ExpectedOldCodeSize || target.Body.Variables.Count != ExpectedOldLocals)
        throw new InvalidDataException($"Device ctor fingerprint drift token=0x{Raw(target):X8} rid={target.MetadataToken.RID} size={target.Body.CodeSize} locals={target.Body.Variables.Count}");

    var hpType = t.NestedTypes.Single(x => x.Name == "HPUIController");
    var maxHp = hpType.Fields.Single(field => field.Name == "maxHPEffect");
    if (!maxHp.IsPrivate) throw new InvalidDataException("HPUIController.maxHPEffect visibility drift");
    var hpCtor = hpType.Methods.Single(m => m.IsConstructor && !m.IsStatic && m.Parameters.Count == 0);
    if (!hpCtor.Body.Instructions.Any(i => i.OpCode == OpCodes.Stfld && i.Operand is FieldReference fr && fr.Name == "maxHPEffect" && fr.DeclaringType.FullName == "Device/HPUIController"))
        throw new InvalidDataException("nested HPUIController ctor no longer initializes maxHPEffect");

    var ins = target.Body.Instructions;
    var badStores = ins.Where(i => i.OpCode == OpCodes.Stfld && i.Operand is FieldReference fr && fr.Name == "maxHPEffect" && fr.DeclaringType.FullName == "Device/HPUIController").ToList();
    if (badStores.Count != 1) throw new InvalidDataException($"expected exactly one outer maxHPEffect store, found {badStores.Count}");
    var badStore = badStores[0];
    var idx = ins.IndexOf(badStore);
    if (idx < 2) throw new InvalidDataException("bad store has no expected producers");
    var loadHp = ins[idx-2];
    var one = ins[idx-1];
    if (loadHp.OpCode != OpCodes.Ldloc || loadHp.Operand is not VariableDefinition v || v.Index != 5)
        throw new InvalidDataException("expected ldloc 5 before private store");
    if (one.OpCode != OpCodes.Ldc_R4 || one.Operand is not float oneValue || BitConverter.SingleToInt32Bits(oneValue) != BitConverter.SingleToInt32Bits(1f))
        throw new InvalidDataException("expected ldc.r4 1 before private store");
    if (!ins.Any(i => i.Operand == loadHp && (i.OpCode.FlowControl == FlowControl.Branch || i.OpCode.FlowControl == FlowControl.Cond_Branch)))
        throw new InvalidDataException("expected control-flow edge into redundant store block");

    if (ins.Count(i => i.OpCode == OpCodes.Newobj && i.Operand is MethodReference mr && mr.Name == ".ctor" && mr.DeclaringType.FullName == "Device/HPUIController") != 1)
        throw new InvalidDataException("HPUIController construction drift");
    if (ins.Count(i => i.OpCode == OpCodes.Stfld && i.Operand is FieldReference fr && fr.Name == "hpUIController" && fr.DeclaringType.FullName == "Device") != 1)
        throw new InvalidDataException("Device.hpUIController assignment drift");
    beforeListCtors = ListCtorSig(target);
    if (beforeListCtors.Split('\n', StringSplitOptions.RemoveEmptyEntries).Length != 4)
        throw new InvalidDataException("expected four List<T> ctor refs");

    Console.WriteLine($"NATIVE_AUTHORITY target_token=0x{TargetToken:X8} rid={ExpectedRid} method_pointer_entry=0x181B84728 va=0x18034CAD0 end=0x18034CC66 native_span_sha256={NativeSpanSha}");
    Console.WriteLine("CLR_ADAPTATION remove_outer_private_store=Device/HPUIController::maxHPEffect preserve_nested_ctor=1 preserve_hpui_assignment=1 preserve_list_ctor_refs=4 preserve_field_visibility=1");

    foreach (var i in new[] { loadHp, one, badStore }) { i.OpCode = OpCodes.Nop; i.Operand = null; }
    Directory.CreateDirectory(Path.GetDirectoryName(output)!);
    module.Write(output);
}

Console.WriteLine($"OUTPUT_SHA256 {Sha(output)}");
using (var module = ModuleDefinition.ReadModule(output, new ReaderParameters { InMemory=true, ReadingMode=ReadingMode.Immediate }))
{
    var types=AllTypes(module.Types).ToList(); var methods=types.SelectMany(t=>t.Methods).ToList(); var fields=types.SelectMany(t=>t.Fields).ToList();
    var target=methods.Single(m=>Raw(m)==TargetToken);
    if(target.MetadataToken.RID!=ExpectedRid || target.Body.Variables.Count!=ExpectedOldLocals) throw new InvalidDataException("reopen target/local drift");
    if(target.Body.Instructions.Any(i=>i.OpCode==OpCodes.Stfld && i.Operand is FieldReference fr && fr.Name=="maxHPEffect" && fr.DeclaringType.FullName=="Device/HPUIController"))
        throw new InvalidDataException("outer private maxHPEffect store remains");
    if(target.Body.Instructions.Count(i=>i.OpCode==OpCodes.Newobj && i.Operand is MethodReference mr && mr.Name==".ctor" && mr.DeclaringType.FullName=="Device/HPUIController")!=1)
        throw new InvalidDataException("HPUIController ctor lost");
    if(target.Body.Instructions.Count(i=>i.OpCode==OpCodes.Stfld && i.Operand is FieldReference fr && fr.Name=="hpUIController" && fr.DeclaringType.FullName=="Device")!=1)
        throw new InvalidDataException("Device.hpUIController assignment lost");
    if(ListCtorSig(target)!=beforeListCtors) throw new InvalidDataException("List<T> ctor refs changed");
    var changedM=methods.Where(m=>beforeM[Raw(m)]!=MethodSig(m)).Select(Raw).OrderBy(x=>x).ToList();
    if(changedM.Count!=1 || changedM[0]!=TargetToken) throw new InvalidDataException("semantic isolation failed: "+string.Join(',',changedM.Select(x=>$"0x{x:X8}")));
    var changedF=fields.Where(field=>beforeF[Raw(field)]!=FieldSig(field)).Select(Raw).ToList(); if(changedF.Count!=0) throw new InvalidDataException("field metadata drift");
    var afterRefs=string.Join("\n",module.AssemblyReferences.Select(a=>a.FullName).OrderBy(x=>x,StringComparer.Ordinal)); if(afterRefs!=beforeRefs) throw new InvalidDataException("assembly reference set changed");
    if(module.AssemblyReferences.Any(a=>a.Name=="System.Private.CoreLib")) throw new InvalidDataException("System.Private.CoreLib pollution");
    Console.WriteLine($"REOPEN_DEVICE_CTOR_PASS token=0x{TargetToken:X8} code_size={target.Body.CodeSize} locals={target.Body.Variables.Count} outer_maxHPEffect_store=0 hpui_ctor=1 hpui_assignment=1");
    Console.WriteLine("GENERIC_REF_PRESERVATION_PASS list_ctor_refs=4 source=runtime_qualified_bb33");
    Console.WriteLine($"SEMANTIC_ISOLATION_PASS untouched_methods={ExpectedMethods-1} changed_methods=1 target=0x{TargetToken:X8}");
    Console.WriteLine($"FIELD_METADATA_ISOLATION_PASS unchanged_fields={ExpectedFields} changed_fields=0 private_maxHPEffect_preserved=1");
    Console.WriteLine("FRAMEWORK_REFERENCE_GATE_PASS system_private_corelib_refs=0 assembly_reference_set_unchanged=1");
}
return 0;
