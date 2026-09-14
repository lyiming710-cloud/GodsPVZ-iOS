using Mono.Cecil;
using Mono.Cecil.Cil;
using System.Globalization;
using System.Security.Cryptography;
using System.Text;

if (args.Length != 3)
{
    Console.Error.WriteLine("Usage: Stage9DialogueUpdatePatch <input.dll> <output.dll> <unityjit-linux-mscorlib.dll>");
    return 2;
}

const string ExpectedInputSha = "864481ad0bbbf6799a7c14a858e857823402edf62c19900ed15a70cc471c44ce";
const string ExpectedCorelibSha = "4d1725f1ae54a22f69b79c2b1569181ed6653dd484c35005e29c40300c6673be";
const uint TargetToken = 0x06000194;
const int ExpectedRid = 404;
const int ExpectedMethods = 2317;
const int ExpectedFields = 2802;
const int ExpectedOldCodeSize = 234;
const int ExpectedOldLocals = 9;
const string NativeSliceSha = "e40d885774b7c6ddeb7651e6091d748daa75bd3551e3229532c21ea45c97141b";

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
static MethodReference MakeHostInstanceMethod(ModuleDefinition targetModule, MethodDefinition openDefinition, TypeReference closedHost)
{
    var open = targetModule.ImportReference(openDefinition);
    var host = new MethodReference(open.Name, open.ReturnType, closedHost)
    {
        HasThis = open.HasThis,
        ExplicitThis = open.ExplicitThis,
        CallingConvention = open.CallingConvention
    };
    foreach (var p in open.Parameters) host.Parameters.Add(new ParameterDefinition(p.Name, p.Attributes, p.ParameterType));
    foreach (var gp in open.GenericParameters) host.GenericParameters.Add(new GenericParameter(gp.Name, host));
    return host;
}

var input = Path.GetFullPath(args[0]);
var output = Path.GetFullPath(args[1]);
var corelib = Path.GetFullPath(args[2]);
if (Sha(input) != ExpectedInputSha) throw new InvalidDataException("input SHA mismatch");
if (Sha(corelib) != ExpectedCorelibSha) throw new InvalidDataException("Unity corelib SHA mismatch");
Console.WriteLine($"INPUT_SHA256 {Sha(input)}");
Console.WriteLine($"UNITYJIT_LINUX_MSCORLIB_SHA256 {Sha(corelib)}");

using var core = ModuleDefinition.ReadModule(corelib, new ReaderParameters { InMemory=true, ReadingMode=ReadingMode.Immediate });
var listOpen = core.GetType("System.Collections.Generic.List`1") ?? throw new InvalidDataException("List<T> missing in exact corelib");
var getCountDef = listOpen.Methods.Single(m => m.Name == "get_Count" && !m.IsStatic && m.Parameters.Count == 0 && m.ReturnType.FullName == "System.Int32");
Console.WriteLine($"CORELIB_COUNT_AUTHORITY token=0x{Raw(getCountDef):X8} signature={getCountDef.FullName}");

var beforeM = new Dictionary<uint,string>();
var beforeF = new Dictionary<uint,string>();
string beforeRefs;

using (var module = ModuleDefinition.ReadModule(input, new ReaderParameters { InMemory=true, ReadingMode=ReadingMode.Immediate }))
{
    var types = AllTypes(module.Types).ToList();
    var methods = types.SelectMany(t => t.Methods).ToList();
    var fields = types.SelectMany(t => t.Fields).ToList();
    if (methods.Count != ExpectedMethods || fields.Count != ExpectedFields) throw new InvalidDataException("metadata count drift");
    foreach (var m in methods) beforeM[Raw(m)] = MethodSig(m);
    foreach (var f in fields) beforeF[Raw(f)] = FieldSig(f);
    beforeRefs = string.Join("\n", module.AssemblyReferences.Select(a => a.FullName).OrderBy(x => x, StringComparer.Ordinal));
    if (module.AssemblyReferences.Any(a => a.Name == "System.Private.CoreLib")) throw new InvalidDataException("input CoreLib pollution");

    var target = types.Single(t => t.FullName == "DialogueManager_OnBoard").Methods.Single(m => m.Name == "Update" && m.Parameters.Count == 0);
    if (Raw(target) != TargetToken || target.MetadataToken.RID != ExpectedRid || target.Body.CodeSize != ExpectedOldCodeSize || target.Body.Variables.Count != ExpectedOldLocals)
        throw new InvalidDataException("Dialogue Update fingerprint drift");
    var bad = target.Body.Instructions.Where(i => i.OpCode == OpCodes.Ldfld && i.Operand is FieldReference f &&
        f.Name == "_size" && f.DeclaringType.FullName == "System.Collections.Generic.List`1<BoardDialogue>").ToList();
    if (bad.Count != 1) throw new InvalidDataException($"expected one private List._size access, got {bad.Count}");
    var sizeField = (FieldReference)bad[0].Operand;
    var getCount = MakeHostInstanceMethod(module, getCountDef, sizeField.DeclaringType);

    Console.WriteLine($"NATIVE_AUTHORITY target_token=0x{TargetToken:X8} rid={ExpectedRid} method_pointer_entry=0x181B839F8 va=0x180314920 end=0x1803149FD native_slice_sha256={NativeSliceSha}");
    Console.WriteLine("NATIVE_SEMANTICS board_null_return=1 isRunning_return=1 dialogueIndex_vs_list_size=1 get_Item=2 TestLogTrigger=1 LoadDialogue=1");
    Console.WriteLine("RECOVERY_STRATEGY replace_private_List_size_with_public_Count=1 preserve_all_other_IL=1 metadata_changes=0 exception_swallowing=0");

    bad[0].OpCode = OpCodes.Callvirt;
    bad[0].Operand = getCount;
    Directory.CreateDirectory(Path.GetDirectoryName(output)!);
    module.Write(output);
}

Console.WriteLine($"OUTPUT_SHA256 {Sha(output)}");
var resolver = new DefaultAssemblyResolver();
resolver.AddSearchDirectory(Path.GetDirectoryName(corelib)!);
resolver.AddSearchDirectory(Path.GetDirectoryName(output)!);
using (var module = ModuleDefinition.ReadModule(output, new ReaderParameters { InMemory=true, ReadingMode=ReadingMode.Immediate, AssemblyResolver=resolver }))
{
    var types = AllTypes(module.Types).ToList();
    var methods = types.SelectMany(t => t.Methods).ToList();
    var fields = types.SelectMany(t => t.Fields).ToList();
    var target = methods.Single(m => Raw(m) == TargetToken);
    var afterRefs = string.Join("\n", module.AssemblyReferences.Select(a => a.FullName).OrderBy(x => x, StringComparer.Ordinal));
    if (afterRefs != beforeRefs) throw new InvalidDataException("assembly reference set changed");
    if (module.AssemblyReferences.Any(a => a.Name == "System.Private.CoreLib")) throw new InvalidDataException("CoreLib pollution");

    var privateSize = target.Body.Instructions.Count(i => i.Operand is FieldReference f && f.Name == "_size" && f.DeclaringType.FullName == "System.Collections.Generic.List`1<BoardDialogue>");
    var countCalls = target.Body.Instructions.Where(i => (i.OpCode == OpCodes.Call || i.OpCode == OpCodes.Callvirt) && i.Operand is MethodReference m && m.Name == "get_Count" && m.DeclaringType.FullName == "System.Collections.Generic.List`1<BoardDialogue>").ToList();
    if (privateSize != 0 || countCalls.Count != 1) throw new InvalidDataException("Count adaptation mismatch");
    var countRef = (MethodReference)countCalls[0].Operand;
    var resolved = countRef.Resolve() ?? throw new InvalidDataException("get_Count MemberRef did not resolve against exact Unity mscorlib");
    if (resolved.Name != "get_Count" || resolved.ReturnType.FullName != "System.Int32") throw new InvalidDataException("resolved get_Count signature mismatch");
    Console.WriteLine($"COUNT_MEMBERREF_RESOLUTION_PASS resolved_token=0x{Raw(resolved):X8} declaring={resolved.DeclaringType.FullName} return={resolved.ReturnType.FullName}");

    var changedM = methods.Where(m => beforeM[Raw(m)] != MethodSig(m)).Select(m => Raw(m)).OrderBy(x => x).ToList();
    if (changedM.Count != 1 || changedM[0] != TargetToken) throw new InvalidDataException("semantic isolation failed: " + string.Join(',', changedM.Select(x => $"0x{x:X8}")));
    Console.WriteLine($"SEMANTIC_ISOLATION_PASS untouched_methods={ExpectedMethods-1} changed_methods=1 target=0x{TargetToken:X8}");
    var changedF = fields.Where(f => beforeF[Raw(f)] != FieldSig(f)).Select(f => Raw(f)).ToList();
    if (changedF.Count != 0) throw new InvalidDataException("field metadata changed");
    Console.WriteLine($"FIELD_METADATA_ISOLATION_PASS unchanged_fields={ExpectedFields} changed_fields=0");
    Console.WriteLine($"REOPEN_DIALOGUE_UPDATE_PASS token=0x{TargetToken:X8} code_size={target.Body.CodeSize} locals={target.Body.Variables.Count} private_List_size_refs=0 List_Count_calls=1");
    Console.WriteLine("FRAMEWORK_REFERENCE_GATE_PASS system_private_corelib_refs=0 assembly_reference_set_unchanged=1");
}
return 0;
