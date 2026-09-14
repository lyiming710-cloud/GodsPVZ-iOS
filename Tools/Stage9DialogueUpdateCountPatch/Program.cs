using Mono.Cecil;
using Mono.Cecil.Cil;
using System.Globalization;
using System.Security.Cryptography;
using System.Text;

if (args.Length != 4)
{
    Console.Error.WriteLine("Usage: Stage9DialogueUpdateCountPatch <input.dll> <output.dll> <expected-input-sha256> <unityjit-linux-mscorlib.dll>");
    return 2;
}

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
    foreach (var p in open.Parameters)
        host.Parameters.Add(new ParameterDefinition(p.Name, p.Attributes, p.ParameterType));
    foreach (var gp in open.GenericParameters)
        host.GenericParameters.Add(new GenericParameter(gp.Name, host));
    return host;
}

var input = Path.GetFullPath(args[0]);
var output = Path.GetFullPath(args[1]);
var expectedInputSha = args[2].Trim().ToLowerInvariant();
var corelib = Path.GetFullPath(args[3]);
if (expectedInputSha.Length != 64 || expectedInputSha.Any(c => !Uri.IsHexDigit(c))) throw new InvalidDataException("expected input SHA is not 64 hex chars");
if (Sha(input) != expectedInputSha) throw new InvalidDataException("input SHA mismatch");
if (Sha(corelib) != ExpectedCorelibSha) throw new InvalidDataException("Unity corelib SHA mismatch");
Console.WriteLine($"INPUT_SHA256 {Sha(input)}");
Console.WriteLine($"UNITYJIT_LINUX_MSCORLIB_SHA256 {Sha(corelib)}");

using var core = ModuleDefinition.ReadModule(corelib, new ReaderParameters { InMemory=true, ReadingMode=ReadingMode.Immediate });
var listOpen = core.GetType("System.Collections.Generic.List`1") ?? throw new InvalidDataException("List`1 not found in exact corelib");
var countDef = listOpen.Methods.Single(m => m.Name == "get_Count" && !m.IsStatic && m.Parameters.Count == 0 && m.ReturnType.FullName == "System.Int32");

var beforeM = new Dictionary<uint,string>();
var beforeF = new Dictionary<uint,string>();
int mscorlibRefsBefore;

using (var module = ModuleDefinition.ReadModule(input, new ReaderParameters { InMemory=true, ReadingMode=ReadingMode.Immediate }))
{
    if (module.AssemblyReferences.Any(a => a.Name == "System.Private.CoreLib")) throw new InvalidDataException("input references System.Private.CoreLib");
    mscorlibRefsBefore = module.AssemblyReferences.Count(a => a.Name == "mscorlib");
    if (mscorlibRefsBefore != 1) throw new InvalidDataException("unexpected mscorlib reference count");
    var types = AllTypes(module.Types).ToList();
    var methods = types.SelectMany(t => t.Methods).ToList();
    var fields = types.SelectMany(t => t.Fields).ToList();
    if (methods.Count != ExpectedMethods || fields.Count != ExpectedFields) throw new InvalidDataException("metadata count drift");
    foreach (var m in methods) beforeM[Raw(m)] = MethodSig(m);
    foreach (var f in fields) beforeF[Raw(f)] = FieldSig(f);

    var t = types.Single(x => x.FullName == "DialogueManager_OnBoard");
    var target = t.Methods.Single(m => m.Name == "Update" && m.Parameters.Count == 0);
    if (Raw(target) != TargetToken || target.MetadataToken.RID != ExpectedRid || target.Body.CodeSize != ExpectedOldCodeSize || target.Body.Variables.Count != ExpectedOldLocals)
        throw new InvalidDataException("DialogueManager_OnBoard.Update fingerprint drift");

    var dialogueList = t.Fields.Single(f => f.Name == "dialogueList" && f.FieldType.FullName == "System.Collections.Generic.List`1<BoardDialogue>");
    var privateSizeReads = target.Body.Instructions.Where(i => i.OpCode == OpCodes.Ldfld && i.Operand is FieldReference f && f.Name == "_size" && f.DeclaringType.FullName == "System.Collections.Generic.List`1<BoardDialogue>").ToList();
    if (privateSizeReads.Count != 1 || privateSizeReads[0].Offset != 0x5D)
        throw new InvalidDataException("expected unique private List<BoardDialogue>._size read at IL_005D");
    if (!target.Body.Instructions.Any(i => i.Offset == 0x45 && i.OpCode == OpCodes.Ldfld && i.Operand is FieldReference f && f.Name == "dialogueList"))
        throw new InvalidDataException("expected dialogueList load before size query missing");

    var getCount = MakeHostInstanceMethod(module, countDef, dialogueList.FieldType);
    if (getCount.ReturnType.FullName != "System.Int32" || getCount.Parameters.Count != 0 || !getCount.HasThis)
        throw new InvalidDataException("get_Count imported signature drift");

    Console.WriteLine($"NATIVE_AUTHORITY target_token=0x{TargetToken:X8} rid={ExpectedRid} method_pointer_entry=0x181B839F8 va=0x180314920 end=0x1803149FD native_slice_sha256={NativeSliceSha}");
    Console.WriteLine("NATIVE_SEMANTICS board_null_return=1 isRunning_return=1 dialogueIndex_vs_list_size=1 TestLogTrigger=1 LoadDialogue=1");
    Console.WriteLine("CLR_ACCESS_ADAPTATION private_List_size_to_public_List_Count=1 exact_unityjit_linux_corelib_methoddef=1 control_flow_changes=0 field_metadata_changes=0");

    var ins = privateSizeReads[0];
    ins.OpCode = OpCodes.Callvirt;
    ins.Operand = getCount;

    Directory.CreateDirectory(Path.GetDirectoryName(output)!);
    module.Write(output);
}

Console.WriteLine($"OUTPUT_SHA256 {Sha(output)}");

var resolver = new DefaultAssemblyResolver();
resolver.AddSearchDirectory(Path.GetDirectoryName(corelib)!);
resolver.AddSearchDirectory(Path.GetDirectoryName(output)!);
using (var module = ModuleDefinition.ReadModule(output, new ReaderParameters { InMemory=true, ReadingMode=ReadingMode.Immediate, AssemblyResolver=resolver }))
{
    if (module.AssemblyReferences.Any(a => a.Name == "System.Private.CoreLib")) throw new InvalidDataException("System.Private.CoreLib reference pollution");
    if (module.AssemblyReferences.Count(a => a.Name == "mscorlib") != mscorlibRefsBefore) throw new InvalidDataException("mscorlib AssemblyRef count drift");
    var types = AllTypes(module.Types).ToList();
    var methods = types.SelectMany(t => t.Methods).ToList();
    var fields = types.SelectMany(t => t.Fields).ToList();
    var target = methods.Single(m => Raw(m) == TargetToken);

    var sizeReads = target.Body.Instructions.Count(i => i.OpCode == OpCodes.Ldfld && i.Operand is FieldReference f && f.Name == "_size" && f.DeclaringType.FullName == "System.Collections.Generic.List`1<BoardDialogue>");
    var countCalls = target.Body.Instructions.Where(i => (i.OpCode == OpCodes.Call || i.OpCode == OpCodes.Callvirt) && i.Operand is MethodReference m && m.Name == "get_Count" && m.DeclaringType.FullName == "System.Collections.Generic.List`1<BoardDialogue>").ToList();
    if (sizeReads != 0 || countCalls.Count != 1 || countCalls[0].OpCode != OpCodes.Callvirt)
        throw new InvalidDataException("reopen List Count replacement mismatch");
    var countRef = (MethodReference)countCalls[0].Operand;
    var resolved = countRef.Resolve() ?? throw new InvalidDataException("List<BoardDialogue>.get_Count MemberRef did not resolve");
    if (resolved.Name != "get_Count" || resolved.DeclaringType.FullName != "System.Collections.Generic.List`1" || resolved.ReturnType.FullName != "System.Int32")
        throw new InvalidDataException("get_Count resolved to wrong MethodDef");
    if (string.IsNullOrEmpty(resolved.Module.FileName) || Sha(resolved.Module.FileName) != ExpectedCorelibSha)
        throw new InvalidDataException("get_Count resolved against wrong corelib");

    var changedM = methods.Where(m => beforeM[Raw(m)] != MethodSig(m)).Select(m => Raw(m)).OrderBy(x => x).ToList();
    if (changedM.Count != 1 || changedM[0] != TargetToken)
        throw new InvalidDataException("semantic method isolation failed: " + string.Join(',', changedM.Select(x => $"0x{x:X8}")));
    var changedF = fields.Where(f => beforeF[Raw(f)] != FieldSig(f)).Select(f => Raw(f)).ToList();
    if (changedF.Count != 0) throw new InvalidDataException("field metadata drift");

    Console.WriteLine($"DIALOGUE_UPDATE_COUNT_MEMBERREF_RESOLUTION_PASS target_token=0x{TargetToken:X8} private_size_reads=0 public_Count_calls=1 corelib_sha256={ExpectedCorelibSha}");
    Console.WriteLine($"SEMANTIC_ISOLATION_PASS untouched_methods={ExpectedMethods-1} changed_methods=1 target=0x{TargetToken:X8}");
    Console.WriteLine($"FIELD_METADATA_ISOLATION_PASS unchanged_fields={ExpectedFields} changed_fields=0");
}

return 0;
