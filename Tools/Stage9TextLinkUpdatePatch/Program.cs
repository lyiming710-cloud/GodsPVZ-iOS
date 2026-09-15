using Mono.Cecil;
using Mono.Cecil.Cil;
using System.Globalization;
using System.Security.Cryptography;
using System.Text;

if (args.Length != 3)
{
    Console.Error.WriteLine("Usage: Stage9TextLinkUpdatePatch <input.dll> <output.dll> <expected-input-sha256>");
    return 2;
}

const uint TargetToken = 0x060006DE;
const int ExpectedRid = 1758;
const uint TmpFieldToken = 0x040008E1;
const uint LinkFieldToken = 0x040008E2;
const uint LastFieldToken = 0x040008E3;
const int ExpectedMethods = 2317;
const int ExpectedFields = 2802;
const int ExpectedOldCodeSize = 235;
const int ExpectedLocals = 9;
const int ExpectedInstructions = 63;
const string NativeSha = "5bf83d68b7d1c545595f58f52527bf0d29dfb7da03f76220fc74e4ed4c48b0a6";

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
static string InstructionSig(Instruction i, MethodDefinition owner) => $"{i.OpCode.Code}:{OpSig(i.Operand, owner)}";
static string MethodSig(MethodDefinition m)
{
    var sb = new StringBuilder();
    sb.Append(m.FullName).Append('|').Append(m.Attributes).Append('|').Append(m.ImplAttributes).Append('|');
    if (!m.HasBody) return sb.Append("NOBODY").ToString();
    sb.Append("init=").Append(m.Body.InitLocals).Append(";max=").Append(m.Body.MaxStackSize).Append(';');
    foreach (var v in m.Body.Variables) sb.Append("L:").Append(v.Index).Append(':').Append(TypeSig(v.VariableType)).Append(';');
    foreach (var i in m.Body.Instructions) sb.Append("I:").Append(InstructionSig(i,m)).Append(';');
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
static HashSet<string> UsedMethodRefs(IEnumerable<MethodDefinition> methods) => methods.Where(m => m.HasBody)
    .SelectMany(m => m.Body.Instructions).Select(i => i.Operand).OfType<MethodReference>()
    .Select(MethodRefSig).ToHashSet(StringComparer.Ordinal);

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
List<string> beforeTargetInstructions;

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
    beforeUsedMethodRefs = UsedMethodRefs(methods);

    var t = types.Single(x => x.FullName == "TextLink");
    var target = methods.Single(m => Raw(m) == TargetToken);
    if (target.DeclaringType != t || target.Name != "Update" || target.IsStatic || target.Parameters.Count != 0 || !target.HasBody)
        throw new InvalidDataException("target identity drift");
    if (target.MetadataToken.RID != ExpectedRid || target.Body.CodeSize != ExpectedOldCodeSize || target.Body.Variables.Count != ExpectedLocals || target.Body.ExceptionHandlers.Count != 0 || !target.Body.InitLocals || target.Body.Instructions.Count != ExpectedInstructions)
        throw new InvalidDataException($"target body fingerprint drift rid={target.MetadataToken.RID} size={target.Body.CodeSize} locals={target.Body.Variables.Count} handlers={target.Body.ExceptionHandlers.Count} init={target.Body.InitLocals} instructions={target.Body.Instructions.Count}");

    var tmp = fields.Single(f => Raw(f) == TmpFieldToken);
    var link = fields.Single(f => Raw(f) == LinkFieldToken);
    var last = fields.Single(f => Raw(f) == LastFieldToken);
    if (tmp.DeclaringType != t || tmp.Name != "TMP_Text" || tmp.FieldType.FullName != "TMPro.TMP_Text") throw new InvalidDataException("TMP_Text field drift");
    if (link.DeclaringType != t || link.Name != "linkIndex" || link.FieldType.FullName != "System.Int32") throw new InvalidDataException("linkIndex field drift");
    if (last.DeclaringType != t || last.Name != "lastLinkIndex" || last.FieldType.FullName != "System.Int32") throw new InvalidDataException("lastLinkIndex field drift");

    var body = target.Body;
    if (body.Variables[1].VariableType.FullName != "UnityEngine.Vector3" || body.Variables[2].VariableType.FullName != "UnityEngine.Camera" || body.Variables[8].VariableType.FullName != "System.Object")
        throw new InvalidDataException("local type fingerprint drift");
    var ins = body.Instructions;
    beforeTargetInstructions = ins.Select(i => InstructionSig(i,target)).ToList();
    if (ins[0].OpCode != OpCodes.Br || ins[1].OpCode != OpCodes.Ldarg_0 || ins[2].OpCode != OpCodes.Ldfld || ins[2].Operand is not FieldReference tf || tf.Resolve() is not FieldDefinition tfd || Raw(tfd) != TmpFieldToken)
        throw new InvalidDataException("FindIntersectingLink prelude drift");
    if (ins[3].OpCode != OpCodes.Ldloca || ins[3].Operand is not VariableDefinition bad || bad.Index != 8 || bad.VariableType.FullName != "System.Object")
        throw new InvalidDataException("damaged Vector3 argument fingerprint missing");
    if (ins[4].OpCode != OpCodes.Ldloc || ins[4].Operand is not VariableDefinition cam || cam.Index != 2 || cam.VariableType.FullName != "UnityEngine.Camera")
        throw new InvalidDataException("camera argument drift");
    if (ins[5].OpCode != OpCodes.Call || ins[5].Operand is not MethodReference find || find.DeclaringType.FullName != "TMPro.TMP_TextUtilities" || find.Name != "FindIntersectingLink" || find.Parameters.Count != 3 || find.Parameters[1].ParameterType.FullName != "UnityEngine.Vector3")
        throw new InvalidDataException("FindIntersectingLink reference drift");
    if (ins.Count(i => i.Operand is MethodReference m && m.DeclaringType.FullName == "TextLink" && m.Name == "SetLink") != 1 ||
        ins.Count(i => i.Operand is MethodReference m && m.DeclaringType.FullName == "TextLink" && m.Name == "ResetLink") != 1 ||
        ins.Count(i => i.Operand is MethodReference m && m.DeclaringType.FullName == "UnityEngine.Input" && m.Name == "get_mousePosition") != 1 ||
        ins.Count(i => i.Operand is MethodReference m && m.DeclaringType.FullName == "UnityEngine.Camera" && m.Name == "get_main") != 1)
        throw new InvalidDataException("state-machine call fingerprint drift");

    Console.WriteLine($"NATIVE_AUTHORITY target_token=0x{TargetToken:X8} rid={ExpectedRid} method_pointer_entry=0x181B86448 va=0x1803ADEB0 end=0x1803ADF93 native_slice_sha256={NativeSha}");
    Console.WriteLine("NATIVE_SEMANTICS findIntersectingLink_arg2=Input.mousePosition_Vector3 linkIndex_state_machine=preserve SetLink=1 ResetLink=1");
    Console.WriteLine("RECOVERY_STRATEGY single_instruction_repair=1 index=3 old=Ldloca_V8_Object new=Ldloc_V1_Vector3 rebuild=0 metadata_changes=0 new_method_refs=0 guards=0 exception_swallowing=0");

    ins[3].OpCode = OpCodes.Ldloc;
    ins[3].Operand = body.Variables[1];
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
    if (!target.HasBody || target.Body.Variables.Count != ExpectedLocals || target.Body.ExceptionHandlers.Count != 0 || !target.Body.InitLocals || target.Body.Instructions.Count != ExpectedInstructions)
        throw new InvalidDataException("target structural drift after reopen");
    var ins = target.Body.Instructions;
    if (ins[3].OpCode != OpCodes.Ldloc || ins[3].Operand is not VariableDefinition good || good.Index != 1 || good.VariableType.FullName != "UnityEngine.Vector3")
        throw new InvalidDataException("Vector3 repair did not survive reopen");
    if (ins[4].OpCode != OpCodes.Ldloc || ins[4].Operand is not VariableDefinition cam || cam.Index != 2 || cam.VariableType.FullName != "UnityEngine.Camera")
        throw new InvalidDataException("camera argument changed");
    if (ins[5].OpCode != OpCodes.Call || ins[5].Operand is not MethodReference find || find.DeclaringType.FullName != "TMPro.TMP_TextUtilities" || find.Name != "FindIntersectingLink")
        throw new InvalidDataException("FindIntersectingLink call changed");

    var afterTargetInstructions = ins.Select(i => InstructionSig(i,target)).ToList();
    if (beforeTargetInstructions.Count != afterTargetInstructions.Count) throw new InvalidDataException("target instruction count drift");
    var instructionDiffs = Enumerable.Range(0, beforeTargetInstructions.Count).Where(i => beforeTargetInstructions[i] != afterTargetInstructions[i]).ToList();
    if (instructionDiffs.Count != 1 || instructionDiffs[0] != 3)
        throw new InvalidDataException("target instruction isolation failed: " + string.Join(',', instructionDiffs));

    var changedM = methods.Where(m => beforeM[Raw(m)] != MethodSig(m)).Select(Raw).OrderBy(x => x).ToList();
    if (changedM.Count != 1 || changedM[0] != TargetToken) throw new InvalidDataException("semantic isolation failed: " + string.Join(',', changedM.Select(x => $"0x{x:X8}")));
    var changedF = fields.Where(f => beforeF[Raw(f)] != FieldSig(f)).Select(Raw).ToList();
    if (changedF.Count != 0) throw new InvalidDataException("field metadata isolation failed");
    var afterAssemblyRefs = string.Join("\n", module.AssemblyReferences.Select(a => a.FullName).OrderBy(x => x, StringComparer.Ordinal));
    if (beforeAssemblyRefs != afterAssemblyRefs) throw new InvalidDataException("assembly reference set changed");
    if (module.AssemblyReferences.Any(a => a.Name == "System.Private.CoreLib")) throw new InvalidDataException("System.Private.CoreLib pollution");
    var afterUsedMethodRefs = UsedMethodRefs(methods);
    var newRefs = afterUsedMethodRefs.Except(beforeUsedMethodRefs, StringComparer.Ordinal).OrderBy(x => x, StringComparer.Ordinal).ToList();
    if (newRefs.Count != 0) throw new InvalidDataException("new used method refs introduced: " + string.Join(" | ", newRefs));

    Console.WriteLine($"REOPEN_TEXTLINK_UPDATE_PASS token=0x{TargetToken:X8} instruction_index=3 vector_arg=V_1_Vector3 camera_arg=V_2_Camera findIntersectingLink=1 SetLink=1 ResetLink=1");
    Console.WriteLine("TARGET_INSTRUCTION_ISOLATION_PASS changed_instruction_count=1 changed_index=3 old=Ldloca_V8_Object new=Ldloc_V1_Vector3");
    Console.WriteLine($"SEMANTIC_ISOLATION_PASS untouched_methods={ExpectedMethods-1} changed_methods=1 target=0x{TargetToken:X8}");
    Console.WriteLine($"FIELD_METADATA_ISOLATION_PASS unchanged_fields={ExpectedFields} changed_fields=0");
    Console.WriteLine("FRAMEWORK_REFERENCE_GATE_PASS system_private_corelib_refs=0 assembly_reference_set_unchanged=1 new_used_method_refs=0");
}
return 0;
