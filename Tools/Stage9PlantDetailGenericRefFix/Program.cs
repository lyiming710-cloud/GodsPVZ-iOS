using Mono.Cecil;
using Mono.Cecil.Cil;
using System.Security.Cryptography;
using System.Text;

if (args.Length != 2)
{
    Console.Error.WriteLine("Usage: GodsPVZ.Stage9PlantDetailGenericRefFix <input.dll> <output.dll>");
    return 2;
}

const string ExpectedInputSha256 = "eaf77ffecc233fa612d0977d8867ab32a2a695fb88c2dd09d86a6920fd2e8995";
const int ExpectedMethodDefCount = 2317;
const uint TargetToken = 0x060006C2u;

static IEnumerable<TypeDefinition> AllTypes(IEnumerable<TypeDefinition> roots)
{
    foreach (var t in roots)
    {
        yield return t;
        foreach (var n in AllTypes(t.NestedTypes)) yield return n;
    }
}

static uint Raw(IMetadataTokenProvider p) => p.MetadataToken.ToUInt32();

static bool IsListGetItem(MethodReference mr)
    => mr.Name == "get_Item"
       && mr.DeclaringType is GenericInstanceType gi
       && gi.ElementType.FullName == "System.Collections.Generic.List`1"
       && mr.Parameters.Count == 1
       && mr.Parameters[0].ParameterType.FullName == "System.Int32";

static string OperandSemantic(object? operand, MethodDefinition owner)
{
    if (operand is null) return "";
    if (operand is Instruction bi) return $"I#{owner.Body.Instructions.IndexOf(bi)}";
    if (operand is Instruction[] sw) return "SW[" + string.Join(',', sw.Select(x => owner.Body.Instructions.IndexOf(x))) + "]";
    if (operand is MethodReference mr) return $"M:{mr.FullName}";
    if (operand is FieldReference fr) return $"F:{fr.FullName}";
    if (operand is TypeReference tr) return $"T:{tr.FullName}";
    if (operand is VariableDefinition vr) return $"V:{vr.Index}:{vr.VariableType.FullName}";
    if (operand is ParameterDefinition pr) return $"P:{pr.Index}:{pr.ParameterType.FullName}";
    if (operand is string s) return $"S:{Convert.ToBase64String(Encoding.UTF8.GetBytes(s))}";
    return $"C:{Convert.ToString(operand, System.Globalization.CultureInfo.InvariantCulture)}";
}

static string MethodSemantic(MethodDefinition m)
{
    var sb = new StringBuilder();
    sb.Append(m.FullName).Append('|').Append(m.Attributes).Append('|').Append(m.ImplAttributes).Append('|');
    if (!m.HasBody) return sb.Append("NOBODY").ToString();
    sb.Append("init=").Append(m.Body.InitLocals).Append(";max=").Append(m.Body.MaxStackSize).Append(';');
    foreach (var v in m.Body.Variables) sb.Append("L:").Append(v.Index).Append(':').Append(v.VariableType.FullName).Append(';');
    foreach (var i in m.Body.Instructions) sb.Append("I:").Append(i.OpCode.Code).Append(':').Append(OperandSemantic(i.Operand, m)).Append(';');
    foreach (var h in m.Body.ExceptionHandlers)
    {
        int Idx(Instruction? x) => x is null ? -1 : m.Body.Instructions.IndexOf(x);
        sb.Append("EH:").Append(h.HandlerType).Append(':')
          .Append(Idx(h.TryStart)).Append(':').Append(Idx(h.TryEnd)).Append(':')
          .Append(Idx(h.HandlerStart)).Append(':').Append(Idx(h.HandlerEnd)).Append(':')
          .Append(Idx(h.FilterStart)).Append(':').Append(h.CatchType?.FullName ?? "").Append(';');
    }
    return sb.ToString();
}

var input = Path.GetFullPath(args[0]);
var output = Path.GetFullPath(args[1]);
var inputSha = Convert.ToHexString(SHA256.HashData(File.ReadAllBytes(input))).ToLowerInvariant();
Console.WriteLine($"INPUT_SHA256 {inputSha}");
if (inputSha != ExpectedInputSha256) throw new InvalidDataException($"input SHA mismatch: {inputSha}");

using var module = ModuleDefinition.ReadModule(input, new ReaderParameters { InMemory = true, ReadingMode = ReadingMode.Immediate });
var allBefore = AllTypes(module.Types).SelectMany(t => t.Methods).ToList();
if (allBefore.Count != ExpectedMethodDefCount) throw new InvalidDataException($"MethodDef count drifted: {allBefore.Count}");
var target = module.LookupToken(new MetadataToken(TokenType.Method, (int)(TargetToken & 0x00FFFFFF))) as MethodDefinition
    ?? throw new InvalidDataException("PlantDetail.Awake missing");
if (target.DeclaringType.FullName != "SeedChooserScreen/PlantDetail" || target.Name != "Awake" || target.Parameters.Count != 1 || !target.HasBody)
    throw new InvalidDataException($"target drift: {target.FullName}");

var untouchedBefore = allBefore.Where(m => Raw(m) != TargetToken).ToDictionary(Raw, MethodSemantic);

var allOperands = allBefore.Where(m => m.HasBody)
    .SelectMany(m => m.Body.Instructions.Select(i => (Method: m, Operand: i.Operand)))
    .Where(x => x.Operand is MethodReference)
    .Select(x => (x.Method, Ref: (MethodReference)x.Operand!))
    .ToList();

var template = allOperands.Select(x => x.Ref).FirstOrDefault(m =>
    IsListGetItem(m)
    && m.ReturnType is GenericParameter gp
    && gp.Type == GenericParameterType.Type
    && gp.Position == 0)
    ?? throw new InvalidDataException("No valid List<T>.get_Item !0 template found in module");
var templateGp = (GenericParameter)template.ReturnType;
Console.WriteLine($"TEMPLATE token=0x{Raw(template):X8} return={templateGp.FullName} position={templateGp.Position} type={templateGp.Type}");

var badRefs = target.Body.Instructions
    .Select(i => i.Operand)
    .OfType<MethodReference>()
    .Where(IsListGetItem)
    .Where(m => m.ReturnType is not GenericParameter)
    .GroupBy(Raw)
    .Select(g => g.First())
    .ToList();

if (badRefs.Count != 3)
    throw new InvalidDataException($"Expected exactly 3 malformed List<T>.get_Item refs in PlantDetail.Awake, got {badRefs.Count}");

foreach (var mr in badRefs)
{
    var users = allOperands.Where(x => Raw(x.Ref) == Raw(mr)).Select(x => Raw(x.Method)).Distinct().ToList();
    if (users.Count != 1 || users[0] != TargetToken)
        throw new InvalidDataException($"MemberRef 0x{Raw(mr):X8} is shared outside target: {string.Join(',', users.Select(x => $"0x{x:X8}"))}");
    var old = mr.ReturnType.FullName;
    mr.ReturnType = templateGp;
    Console.WriteLine($"PATCH memberref=0x{Raw(mr):X8} declaring={mr.DeclaringType.FullName} return={old}->!0");
}

module.Write(output);
var outputSha = Convert.ToHexString(SHA256.HashData(File.ReadAllBytes(output))).ToLowerInvariant();
Console.WriteLine($"OUTPUT_SHA256 {outputSha}");

using var reopen = ModuleDefinition.ReadModule(output, new ReaderParameters { InMemory = true, ReadingMode = ReadingMode.Immediate });
var allAfter = AllTypes(reopen.Types).SelectMany(t => t.Methods).ToList();
if (allAfter.Count != ExpectedMethodDefCount) throw new InvalidDataException($"reopen MethodDef count drifted: {allAfter.Count}");
var rTarget = reopen.LookupToken(new MetadataToken(TokenType.Method, (int)(TargetToken & 0x00FFFFFF))) as MethodDefinition
    ?? throw new InvalidDataException("reopen target missing");
var listItems = rTarget.Body.Instructions.Select(i => i.Operand).OfType<MethodReference>().Where(IsListGetItem).ToList();
if (listItems.Count == 0) throw new InvalidDataException("reopen target has no List<T>.get_Item calls");
var malformed = listItems.Where(m => m.ReturnType is not GenericParameter gp || gp.Type != GenericParameterType.Type || gp.Position != 0).ToList();
if (malformed.Count != 0)
    throw new InvalidDataException("reopen still contains malformed List<T>.get_Item signatures: " + string.Join(" | ", malformed.Select(m => m.FullName)));

var distinctTokens = listItems.Select(Raw).Distinct().Count();
Console.WriteLine($"REOPEN_GENERIC_LIST_ITEM_PASS calls={listItems.Count} distinct_refs={distinctTokens} all_return_var0=1");

var untouchedAfter = allAfter.Where(m => Raw(m) != TargetToken).ToDictionary(Raw, MethodSemantic);
if (untouchedAfter.Count != untouchedBefore.Count) throw new InvalidDataException("untouched method count drift");
var drift = untouchedBefore.Where(kv => !untouchedAfter.TryGetValue(kv.Key, out var a) || a != kv.Value).Select(kv => kv.Key).ToList();
if (drift.Count != 0) throw new InvalidDataException("semantic drift outside PlantDetail.Awake: " + string.Join(',', drift.Select(t => $"0x{t:X8}")));
Console.WriteLine($"SEMANTIC_ISOLATION_PASS untouched_methods={untouchedAfter.Count} changed_methods=1");
return 0;
