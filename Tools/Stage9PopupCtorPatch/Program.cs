using Mono.Cecil;
using Mono.Cecil.Cil;
using System.Globalization;
using System.Security.Cryptography;
using System.Text;

if (args.Length != 3)
{
    Console.Error.WriteLine("Usage: Stage9PopupCtorPatch <input.dll> <output.dll> <expected-input-sha256>");
    return 2;
}

const uint TargetToken = 0x06000676;
const int ExpectedRid = 1654;
const uint FieldToken = 0x04000884;
const int ExpectedMethods = 2317;
const int ExpectedFields = 2802;
const int ExpectedOldCodeSize = 22;
const int ExpectedOldLocals = 1;
const string NativeSliceSha = "4a7c3b3f5728880eb795eab8b5ede2a4d9b7ce6ef4de9f0d6b308376245813a3";

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

    var t = types.Single(x => x.FullName == "Popup");
    var target = t.Methods.Single(m => m.IsConstructor && !m.IsStatic && m.Parameters.Count == 0);
    if (Raw(target) != TargetToken || target.MetadataToken.RID != ExpectedRid || target.Body.CodeSize != ExpectedOldCodeSize || target.Body.Variables.Count != ExpectedOldLocals)
        throw new InvalidDataException($"Popup ctor fingerprint drift token=0x{Raw(target):X8} rid={target.MetadataToken.RID} size={target.Body.CodeSize} locals={target.Body.Variables.Count}");
    if (!target.Body.InitLocals || target.Body.Variables[0].VariableType.FullName != "Popup") throw new InvalidDataException("Popup ctor local metadata drift");
    if (target.Body.ExceptionHandlers.Count != 0) throw new InvalidDataException("unexpected exception handlers");
    var ins = target.Body.Instructions;
    if (ins.Count != 6) throw new InvalidDataException($"unexpected instruction count {ins.Count}");
    if (ins[0].OpCode != OpCodes.Ldarg_0) throw new InvalidDataException("expected ldarg.0 at IL_0000");
    if (ins[1].OpCode != OpCodes.Ldc_I8 || ins[1].Operand is not long bad || bad != 4294967295L) throw new InvalidDataException("expected damaged ldc.i8 4294967295");
    if (ins[2].OpCode != OpCodes.Stfld || ins[2].Operand is not FieldReference fr || Raw(fr.Resolve()) != FieldToken || fr.Name != "info0_int" || fr.FieldType.FullName != "System.Int32")
        throw new InvalidDataException("expected stfld int32 info0_int");
    if (ins[3].OpCode != OpCodes.Ldarg_0) throw new InvalidDataException("expected second ldarg.0");
    if (ins[4].OpCode != OpCodes.Call || ins[4].Operand is not MethodReference mr || mr.Name != ".ctor" || mr.DeclaringType.FullName != "UnityEngine.MonoBehaviour" || mr.Parameters.Count != 0)
        throw new InvalidDataException("expected MonoBehaviour base ctor call");
    if (ins[5].OpCode != OpCodes.Ret) throw new InvalidDataException("expected ret");

    Console.WriteLine($"NATIVE_AUTHORITY target_token=0x{TargetToken:X8} rid={ExpectedRid} method_pointer_entry=0x181B86108 va=0x1803A6450 end=0x1803A645E native_slice_sha256={NativeSliceSha}");
    Console.WriteLine("NATIVE_SEMANTICS info0_int_offset=0x60 store_width=32 value=-1 base_ctor_tailcall=1 target=0x1812E22A0");
    Console.WriteLine("RECOVERY_STRATEGY single_instruction_replace=1 old=ldc.i8_4294967295 new=ldc.i4.m1 metadata_changes=0 guards=0 exception_swallowing=0");

    var il = target.Body.GetILProcessor();
    il.Replace(ins[1], il.Create(OpCodes.Ldc_I4_M1));
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
    var ins = target.Body.Instructions;
    if (ins.Count != 6 || ins[0].OpCode != OpCodes.Ldarg_0 || ins[1].OpCode != OpCodes.Ldc_I4_M1 || ins[1].Operand is not null || ins[2].OpCode != OpCodes.Stfld || ins[3].OpCode != OpCodes.Ldarg_0 || ins[4].OpCode != OpCodes.Call || ins[5].OpCode != OpCodes.Ret)
        throw new InvalidDataException("reopened target instruction shape drift");
    if (ins[2].Operand is not FieldReference fr || Raw(fr.Resolve()) != FieldToken || fr.FieldType.FullName != "System.Int32") throw new InvalidDataException("reopened info0_int field drift");
    if (ins[4].Operand is not MethodReference mr || mr.DeclaringType.FullName != "UnityEngine.MonoBehaviour" || mr.Name != ".ctor") throw new InvalidDataException("reopened base ctor drift");
    if (target.Body.ExceptionHandlers.Count != 0 || target.Body.Variables.Count != 1 || target.Body.Variables[0].VariableType.FullName != "Popup" || !target.Body.InitLocals)
        throw new InvalidDataException("reopened body metadata drift");

    var changedM = methods.Where(m => beforeM[Raw(m)] != MethodSig(m)).Select(Raw).OrderBy(x => x).ToList();
    if (changedM.Count != 1 || changedM[0] != TargetToken) throw new InvalidDataException("semantic isolation failed: " + string.Join(',', changedM.Select(x => $"0x{x:X8}")));
    var changedF = fields.Where(f => beforeF[Raw(f)] != FieldSig(f)).Select(Raw).ToList();
    if (changedF.Count != 0) throw new InvalidDataException("field metadata isolation failed: " + string.Join(',', changedF.Select(x => $"0x{x:X8}")));
    var afterRefs = string.Join("\n", module.AssemblyReferences.Select(a => a.FullName).OrderBy(x => x, StringComparer.Ordinal));
    if (beforeRefs != afterRefs) throw new InvalidDataException("assembly reference set changed");
    if (module.AssemblyReferences.Any(a => a.Name == "System.Private.CoreLib")) throw new InvalidDataException("System.Private.CoreLib pollution");

    Console.WriteLine($"REOPEN_POPUP_CTOR_PASS token=0x{TargetToken:X8} instructions=6 code_size={target.Body.CodeSize} ldc_i4_m1=1 info0_int_stfld=1 monoBehaviour_ctor=1 exception_handlers=0 locals=1");
    Console.WriteLine($"SEMANTIC_ISOLATION_PASS untouched_methods={ExpectedMethods - 1} changed_methods=1 target=0x{TargetToken:X8}");
    Console.WriteLine($"FIELD_METADATA_ISOLATION_PASS unchanged_fields={ExpectedFields} changed_fields=0");
    Console.WriteLine("FRAMEWORK_REFERENCE_GATE_PASS system_private_corelib_refs=0 assembly_reference_set_unchanged=1");
}
return 0;
