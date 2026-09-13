using Mono.Cecil;
using Mono.Cecil.Cil;
using System.Globalization;
using System.Security.Cryptography;
using System.Text;

if (args.Length != 2)
{
    Console.Error.WriteLine("Usage: Stage9PlantPortraitsCorelibScopePatch <input.dll> <output.dll>");
    return 2;
}

const string ExpectedInputSha256 = "d1decdd1788db6a6e78c7890311067bea5012323fd71bda914e3bfca20b70126";
const int ExpectedMethodDefCount = 2317;
const int ExpectedFieldCount = 2802;
const uint TargetToken = 0x06000223;

static IEnumerable<TypeDefinition> AllTypes(IEnumerable<TypeDefinition> roots)
{
    foreach (var t in roots)
    {
        yield return t;
        foreach (var n in AllTypes(t.NestedTypes)) yield return n;
    }
}

static uint Raw(IMetadataTokenProvider p) => p.MetadataToken.ToUInt32();
static string Sha256(string path) => Convert.ToHexString(SHA256.HashData(File.ReadAllBytes(path))).ToLowerInvariant();
static string ScopeName(TypeReference t) => t.Scope switch
{
    AssemblyNameReference a => a.Name,
    ModuleDefinition m => m.Assembly?.Name?.Name ?? m.Name,
    ModuleReference mr => mr.Name,
    _ => t.Scope?.ToString() ?? "<null>"
};
static string TypeSig(TypeReference t) => $"{t.FullName}@{ScopeName(t)}";

static string OperandSig(object? operand, MethodDefinition owner)
{
    if (operand is null) return "";
    if (operand is Instruction i) return $"I#{owner.Body.Instructions.IndexOf(i)}";
    if (operand is Instruction[] sw) return "SW[" + string.Join(',', sw.Select(i => owner.Body.Instructions.IndexOf(i))) + "]";
    if (operand is MethodReference mr) return $"M:{mr.FullName}@{ScopeName(mr.DeclaringType)}";
    if (operand is FieldReference fr) return $"F:{fr.FullName}@{ScopeName(fr.DeclaringType)}";
    if (operand is TypeReference tr) return $"T:{TypeSig(tr)}";
    if (operand is VariableDefinition vr) return $"V:{vr.Index}:{TypeSig(vr.VariableType)}";
    if (operand is ParameterDefinition pr) return $"P:{pr.Index}:{TypeSig(pr.ParameterType)}";
    if (operand is string s) return "S:" + Convert.ToBase64String(Encoding.UTF8.GetBytes(s));
    if (operand is float f) return $"R4:{BitConverter.SingleToInt32Bits(f):X8}";
    if (operand is double d) return $"R8:{BitConverter.DoubleToInt64Bits(d):X16}";
    return "C:" + Convert.ToString(operand, CultureInfo.InvariantCulture);
}

static string OperandShape(object? operand, MethodDefinition owner)
{
    if (operand is null) return "";
    if (operand is Instruction i) return $"I#{owner.Body.Instructions.IndexOf(i)}";
    if (operand is Instruction[] sw) return "SW[" + string.Join(',', sw.Select(i => owner.Body.Instructions.IndexOf(i))) + "]";
    if (operand is MethodReference mr) return $"M:{mr.FullName}@{ScopeName(mr.DeclaringType)}";
    if (operand is FieldReference fr) return $"F:{fr.FullName}@{ScopeName(fr.DeclaringType)}";
    if (operand is TypeReference tr) return $"T:{TypeSig(tr)}";
    if (operand is VariableDefinition vr) return $"V:{vr.Index}";
    if (operand is ParameterDefinition pr) return $"P:{pr.Index}";
    if (operand is string s) return "S:" + Convert.ToBase64String(Encoding.UTF8.GetBytes(s));
    if (operand is float f) return $"R4:{BitConverter.SingleToInt32Bits(f):X8}";
    if (operand is double d) return $"R8:{BitConverter.DoubleToInt64Bits(d):X16}";
    return "C:" + Convert.ToString(operand, CultureInfo.InvariantCulture);
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
    var constant = f.HasConstant ? Convert.ToString(f.Constant, CultureInfo.InvariantCulture) ?? "<null>" : "<none>";
    return $"{f.FullName}|{f.Attributes}|{TypeSig(f.FieldType)}|const={constant}|ca={attrs}";
}

static string InstructionShape(MethodDefinition m)
{
    var sb = new StringBuilder();
    foreach (var i in m.Body.Instructions) sb.Append(i.OpCode.Code).Append(':').Append(OperandShape(i.Operand, m)).Append(';');
    foreach (var h in m.Body.ExceptionHandlers)
    {
        int Idx(Instruction? x) => x is null ? -1 : m.Body.Instructions.IndexOf(x);
        sb.Append("EH:").Append(h.HandlerType).Append(':').Append(Idx(h.TryStart)).Append(':').Append(Idx(h.TryEnd)).Append(':')
          .Append(Idx(h.HandlerStart)).Append(':').Append(Idx(h.HandlerEnd)).Append(':').Append(Idx(h.FilterStart)).Append(';');
    }
    return sb.ToString();
}

var input = Path.GetFullPath(args[0]);
var output = Path.GetFullPath(args[1]);
var inputSha = Sha256(input);
Console.WriteLine($"INPUT_SHA256 {inputSha}");
if (inputSha != ExpectedInputSha256) throw new InvalidDataException($"input SHA mismatch: {inputSha}");

var beforeMethods = new Dictionary<uint, string>();
var beforeFields = new Dictionary<uint, string>();
string targetInstructionShape;
string[] targetLocalSigs;

using (var module = ModuleDefinition.ReadModule(input, new ReaderParameters { InMemory = true, ReadingMode = ReadingMode.Immediate }))
{
    var types = AllTypes(module.Types).ToList();
    var methods = types.SelectMany(t => t.Methods).ToList();
    var fields = types.SelectMany(t => t.Fields).ToList();
    if (methods.Count != ExpectedMethodDefCount) throw new InvalidDataException($"MethodDef count drift: {methods.Count}");
    if (fields.Count != ExpectedFieldCount) throw new InvalidDataException($"Field count drift: {fields.Count}");
    foreach (var m in methods) beforeMethods[Raw(m)] = MethodSemantic(m);
    foreach (var f in fields) beforeFields[Raw(f)] = FieldSemantic(f);

    var rm = types.Single(t => t.FullName == "ResourceManager");
    var target = rm.Methods.Single(m => m.Name == "Load_card_Choose_PlantPortraits" && m.IsStatic && m.Parameters.Count == 0);
    if (Raw(target) != TargetToken) throw new InvalidDataException($"target token drift: 0x{Raw(target):X8}");
    if (!target.HasBody || target.Body.Variables.Count != 7) throw new InvalidDataException("unexpected target local layout");

    targetInstructionShape = InstructionShape(target);
    targetLocalSigs = target.Body.Variables.Select(v => TypeSig(v.VariableType)).ToArray();
    var badLocals = target.Body.Variables.Where(v => v.VariableType.FullName == "System.Array" && ScopeName(v.VariableType) == "System.Private.CoreLib").ToList();
    if (badLocals.Count != 1 || badLocals[0].Index != 1)
        throw new InvalidDataException($"expected one System.Private.CoreLib System.Array local at index 1; got {string.Join(',', badLocals.Select(v => v.Index))}");

    var arrayGetEnumerator = target.Body.Instructions
        .Where(i => (i.OpCode == OpCodes.Call || i.OpCode == OpCodes.Callvirt) && i.Operand is MethodReference)
        .Select(i => (MethodReference)i.Operand)
        .Single(mr => mr.DeclaringType.FullName == "System.Array" && mr.Name == "GetEnumerator");
    if (ScopeName(arrayGetEnumerator.DeclaringType) != "mscorlib")
        throw new InvalidDataException($"canonical System.Array scope is not mscorlib: {ScopeName(arrayGetEnumerator.DeclaringType)}");

    var corelibRef = module.AssemblyReferences.SingleOrDefault(a => a.Name == "System.Private.CoreLib")
        ?? throw new InvalidDataException("System.Private.CoreLib AssemblyRef missing from d1de fingerprint");
    if (corelibRef.Version?.Major != 10) throw new InvalidDataException($"unexpected corelib version: {corelibRef.Version}");

    badLocals[0].VariableType = arrayGetEnumerator.DeclaringType;
    module.AssemblyReferences.Remove(corelibRef);

    Console.WriteLine("RUNTIME_CAUSAL_EVIDENCE strict_run=34770060252 candidate=d1decdd1788db6a6e78c7890311067bea5012323fd71bda914e3bfca20b70126 exception=FileNotFoundException assembly=System.Private.CoreLib,Version=10.0.0.0 caller=ResourceManager.LoadSprites");
    Console.WriteLine("PATCH_PLANT_PORTRAITS_CORELIB_SCOPE target_token=0x06000223 local_index=1 from=System.Array@System.Private.CoreLib to=System.Array@mscorlib instruction_changes=0 eh_changes=0");
    module.Write(output);
}

Console.WriteLine($"OUTPUT_SHA256 {Sha256(output)}");

using (var after = ModuleDefinition.ReadModule(output, new ReaderParameters { InMemory = true, ReadingMode = ReadingMode.Immediate }))
{
    var types = AllTypes(after.Types).ToList();
    var methods = types.SelectMany(t => t.Methods).ToList();
    var fields = types.SelectMany(t => t.Fields).ToList();
    if (methods.Count != ExpectedMethodDefCount || fields.Count != ExpectedFieldCount) throw new InvalidDataException("metadata count drift after write");
    var rm = types.Single(t => t.FullName == "ResourceManager");
    var target = rm.Methods.Single(m => m.Name == "Load_card_Choose_PlantPortraits" && m.IsStatic && m.Parameters.Count == 0);

    var afterLocalSigs = target.Body.Variables.Select(v => TypeSig(v.VariableType)).ToArray();
    if (afterLocalSigs.Length != targetLocalSigs.Length) throw new InvalidDataException("local count changed");
    for (int i = 0; i < afterLocalSigs.Length; i++)
    {
        if (i == 1)
        {
            if (targetLocalSigs[i] != "System.Array@System.Private.CoreLib" || afterLocalSigs[i] != "System.Array@mscorlib")
                throw new InvalidDataException($"local 1 scope correction mismatch: {targetLocalSigs[i]} -> {afterLocalSigs[i]}");
        }
        else if (afterLocalSigs[i] != targetLocalSigs[i]) throw new InvalidDataException($"unexpected local drift at {i}: {targetLocalSigs[i]} -> {afterLocalSigs[i]}");
    }
    if (InstructionShape(target) != targetInstructionShape) throw new InvalidDataException("instruction/EH shape changed during scope correction");
    if (after.AssemblyReferences.Any(a => a.Name == "System.Private.CoreLib")) throw new InvalidDataException("System.Private.CoreLib AssemblyRef survived output");

    int changed = 0, unchanged = 0;
    foreach (var m in methods)
    {
        if (!beforeMethods.TryGetValue(Raw(m), out var old)) throw new InvalidDataException("new method appeared");
        if (old == MethodSemantic(m)) unchanged++;
        else
        {
            changed++;
            if (Raw(m) != TargetToken) throw new InvalidDataException($"unexpected method drift 0x{Raw(m):X8}");
        }
    }
    if (changed != 1 || unchanged != 2316) throw new InvalidDataException($"method isolation mismatch {unchanged}/{changed}");
    foreach (var f in fields)
        if (!beforeFields.TryGetValue(Raw(f), out var old) || old != FieldSemantic(f)) throw new InvalidDataException($"field drift 0x{Raw(f):X8}");

    var corelibTypeRefs = after.GetTypeReferences().Where(t => ScopeName(t) == "System.Private.CoreLib").Select(t => t.FullName).Distinct().ToArray();
    if (corelibTypeRefs.Length != 0) throw new InvalidDataException("System.Private.CoreLib TypeRefs remain: " + string.Join(',', corelibTypeRefs));

    Console.WriteLine("REOPEN_PLANT_PORTRAITS_CORELIB_SCOPE_PASS local_index=1 scope=mscorlib corelib_assembly_refs=0 corelib_type_refs=0 instructions_unchanged=1 eh_unchanged=1");
    Console.WriteLine("SEMANTIC_ISOLATION_PASS untouched_methods=2316 changed_methods=1 target_token=0x06000223");
    Console.WriteLine("FIELD_METADATA_ISOLATION_PASS unchanged_fields=2802 changed_fields=0");
}

return 0;
