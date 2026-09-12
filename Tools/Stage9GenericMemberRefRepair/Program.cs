using Mono.Cecil;
using Mono.Cecil.Cil;
using System.Security.Cryptography;
using System.Text;

if (args.Length != 2)
{
    Console.Error.WriteLine("Usage: GodsPVZ.Stage9GenericMemberRefRepair <input.dll> <output.dll>");
    return 2;
}

const string ExpectedInputSha256 = "c8d0fdf2c6c03188ebe8a123121c6ef8972d9541ed8f630e95ee4e17b268b425";
const int ExpectedMethodDefCount = 2317;
const int ExpectedSuspiciousCalls = 199;
const int ExpectedDistinctBadRefs = 45;
const int ExpectedChangedMethods = 54;

static IEnumerable<TypeDefinition> AllTypes(IEnumerable<TypeDefinition> roots)
{
    foreach (var t in roots)
    {
        yield return t;
        foreach (var n in AllTypes(t.NestedTypes)) yield return n;
    }
}

static uint Raw(IMetadataTokenProvider p) => p.MetadataToken.ToUInt32();
static bool IsGp(TypeReference t, int pos)
    => t is GenericParameter gp && gp.Type == GenericParameterType.Type && gp.Position == pos;
static bool IsByRefGp(TypeReference t, int pos)
    => t is ByReferenceType br && IsGp(br.ElementType, pos);

static string? Problem(MethodReference m)
{
    if (m.DeclaringType is not GenericInstanceType gi) return null;
    var owner = gi.ElementType.FullName;

    if (owner == "System.Collections.Generic.List`1")
    {
        if (m.Name == "Add" && m.Parameters.Count == 1 && !IsGp(m.Parameters[0].ParameterType, 0))
            return "List.Add param should !0";
        if (m.Name == "get_Item" && m.Parameters.Count == 1 && !IsGp(m.ReturnType, 0))
            return "List.get_Item return should !0";
        if (m.Name == "set_Item" && m.Parameters.Count == 2 && !IsGp(m.Parameters[1].ParameterType, 0))
            return "List.set_Item value should !0";
        if ((m.Name == "Contains" || m.Name == "IndexOf" || m.Name == "Remove") && m.Parameters.Count == 1 && !IsGp(m.Parameters[0].ParameterType, 0))
            return $"List.{m.Name} param should !0";
    }
    else if (owner == "System.Collections.Generic.Dictionary`2")
    {
        if (m.Name == "Add" && m.Parameters.Count == 2 && (!IsGp(m.Parameters[0].ParameterType, 0) || !IsGp(m.Parameters[1].ParameterType, 1)))
            return "Dictionary.Add params should !0,!1";
        if (m.Name == "get_Item" && m.Parameters.Count == 1 && (!IsGp(m.Parameters[0].ParameterType, 0) || !IsGp(m.ReturnType, 1)))
            return "Dictionary.get_Item should (!0)->!1";
        if (m.Name == "set_Item" && m.Parameters.Count == 2 && (!IsGp(m.Parameters[0].ParameterType, 0) || !IsGp(m.Parameters[1].ParameterType, 1)))
            return "Dictionary.set_Item should !0,!1";
        if ((m.Name == "ContainsKey" || m.Name == "Remove") && m.Parameters.Count == 1 && !IsGp(m.Parameters[0].ParameterType, 0))
            return $"Dictionary.{m.Name} key should !0";
        if (m.Name == "TryGetValue" && m.Parameters.Count == 2 && (!IsGp(m.Parameters[0].ParameterType, 0) || !IsByRefGp(m.Parameters[1].ParameterType, 1)))
            return "Dictionary.TryGetValue should !0,!1&";
    }
    return null;
}

static bool IsGoodListTemplate(MethodReference m, string name)
{
    if (m.DeclaringType is not GenericInstanceType gi || gi.ElementType.FullName != "System.Collections.Generic.List`1" || m.Name != name)
        return false;
    return name switch
    {
        "Add" => m.Parameters.Count == 1 && IsGp(m.Parameters[0].ParameterType, 0),
        "get_Item" => m.Parameters.Count == 1 && IsGp(m.ReturnType, 0),
        "set_Item" => m.Parameters.Count == 2 && IsGp(m.Parameters[1].ParameterType, 0),
        "Contains" or "IndexOf" or "Remove" => m.Parameters.Count == 1 && IsGp(m.Parameters[0].ParameterType, 0),
        _ => false
    };
}

static bool IsGoodDictionaryTemplate(MethodReference m, string name)
{
    if (m.DeclaringType is not GenericInstanceType gi || gi.ElementType.FullName != "System.Collections.Generic.Dictionary`2" || m.Name != name)
        return false;
    return name switch
    {
        "Add" => m.Parameters.Count == 2 && IsGp(m.Parameters[0].ParameterType, 0) && IsGp(m.Parameters[1].ParameterType, 1),
        "get_Item" => m.Parameters.Count == 1 && IsGp(m.Parameters[0].ParameterType, 0) && IsGp(m.ReturnType, 1),
        "set_Item" => m.Parameters.Count == 2 && IsGp(m.Parameters[0].ParameterType, 0) && IsGp(m.Parameters[1].ParameterType, 1),
        "ContainsKey" or "Remove" => m.Parameters.Count == 1 && IsGp(m.Parameters[0].ParameterType, 0),
        "TryGetValue" => m.Parameters.Count == 2 && IsGp(m.Parameters[0].ParameterType, 0) && IsByRefGp(m.Parameters[1].ParameterType, 1),
        _ => false
    };
}

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
    return sb.ToString();
}

var input = Path.GetFullPath(args[0]);
var output = Path.GetFullPath(args[1]);
var inputSha = Convert.ToHexString(SHA256.HashData(File.ReadAllBytes(input))).ToLowerInvariant();
Console.WriteLine($"INPUT_SHA256 {inputSha}");
if (inputSha != ExpectedInputSha256) throw new InvalidDataException($"input SHA mismatch: {inputSha}");

using var module = ModuleDefinition.ReadModule(input, new ReaderParameters { InMemory = true, ReadingMode = ReadingMode.Immediate });
var methods = AllTypes(module.Types).SelectMany(t => t.Methods).ToList();
if (methods.Count != ExpectedMethodDefCount) throw new InvalidDataException($"MethodDef count drifted: {methods.Count}");

var refs = methods.Where(m => m.HasBody)
    .SelectMany(m => m.Body.Instructions)
    .Select(i => i.Operand)
    .OfType<MethodReference>()
    .ToList();

var listTemplates = new Dictionary<string, MethodReference>();
foreach (var name in new[] { "Add", "get_Item", "set_Item", "Contains", "IndexOf", "Remove" })
{
    var t = refs.FirstOrDefault(m => IsGoodListTemplate(m, name));
    if (t is null) throw new InvalidDataException($"No valid List<T>.{name} template found");
    listTemplates[name] = t;
    Console.WriteLine($"TEMPLATE_LIST {name} token=0x{Raw(t):X8} sig={t.FullName}");
}

var dictTemplates = new Dictionary<string, MethodReference>();
foreach (var name in new[] { "Add", "get_Item", "set_Item", "ContainsKey", "Remove", "TryGetValue" })
{
    var t = refs.FirstOrDefault(m => IsGoodDictionaryTemplate(m, name));
    if (t is null) throw new InvalidDataException($"No valid Dictionary<TKey,TValue>.{name} template found");
    dictTemplates[name] = t;
    Console.WriteLine($"TEMPLATE_DICT {name} token=0x{Raw(t):X8} sig={t.FullName}");
}

var badSites = new List<(MethodDefinition Method, Instruction Ins, MethodReference Old, string Problem)>();
foreach (var m in methods.Where(m => m.HasBody))
{
    foreach (var ins in m.Body.Instructions)
    {
        if (ins.Operand is not MethodReference mr) continue;
        var p = Problem(mr);
        if (p is not null) badSites.Add((m, ins, mr, p));
    }
}

var distinctBad = badSites.Select(x => Raw(x.Old)).Distinct().OrderBy(x => x).ToList();
var changedTokens = badSites.Select(x => Raw(x.Method)).Distinct().OrderBy(x => x).ToList();
Console.WriteLine($"AUDIT_BEFORE suspicious_calls={badSites.Count} distinct_bad_refs={distinctBad.Count} changed_methods={changedTokens.Count}");
if (badSites.Count != ExpectedSuspiciousCalls || distinctBad.Count != ExpectedDistinctBadRefs || changedTokens.Count != ExpectedChangedMethods)
    throw new InvalidDataException($"audit drift: calls={badSites.Count} refs={distinctBad.Count} methods={changedTokens.Count}");

var changedSet = changedTokens.ToHashSet();
var untouchedBefore = methods.Where(m => !changedSet.Contains(Raw(m))).ToDictionary(Raw, MethodSemantic);

var replacements = new Dictionary<uint, MethodReference>();
foreach (var site in badSites)
{
    var oldToken = Raw(site.Old);
    if (!replacements.TryGetValue(oldToken, out var replacement))
    {
        MethodReference template;
        if (site.Old.DeclaringType is not GenericInstanceType gi)
            throw new InvalidDataException($"bad ref owner drift: {site.Old.FullName}");
        if (gi.ElementType.FullName == "System.Collections.Generic.List`1")
            template = listTemplates[site.Old.Name];
        else if (gi.ElementType.FullName == "System.Collections.Generic.Dictionary`2")
            template = dictTemplates[site.Old.Name];
        else
            throw new InvalidDataException($"unsupported bad ref owner: {site.Old.FullName}");

        replacement = new MethodReference(site.Old.Name, template.ReturnType, site.Old.DeclaringType)
        {
            HasThis = site.Old.HasThis,
            ExplicitThis = site.Old.ExplicitThis,
            CallingConvention = site.Old.CallingConvention
        };
        foreach (var p in template.Parameters)
            replacement.Parameters.Add(new ParameterDefinition(p.Name, p.Attributes, p.ParameterType));

        replacements.Add(oldToken, replacement);
        Console.WriteLine($"REPLACE_REF old=0x{oldToken:X8} problem={site.Problem} old_sig={site.Old.FullName} new_sig={replacement.FullName}");
    }
    site.Ins.Operand = replacement;
}

Console.WriteLine($"PATCH_CALLS replaced={badSites.Count} distinct_old_refs={replacements.Count}");
module.Write(output);
var outputSha = Convert.ToHexString(SHA256.HashData(File.ReadAllBytes(output))).ToLowerInvariant();
Console.WriteLine($"OUTPUT_SHA256 {outputSha}");

using var reopen = ModuleDefinition.ReadModule(output, new ReaderParameters { InMemory = true, ReadingMode = ReadingMode.Immediate });
var after = AllTypes(reopen.Types).SelectMany(t => t.Methods).ToList();
if (after.Count != ExpectedMethodDefCount) throw new InvalidDataException($"reopen MethodDef count drifted: {after.Count}");
var remaining = new List<string>();
foreach (var m in after.Where(m => m.HasBody))
{
    foreach (var ins in m.Body.Instructions)
    {
        if (ins.Operand is MethodReference mr)
        {
            var p = Problem(mr);
            if (p is not null) remaining.Add($"0x{Raw(m):X8} IL_{ins.Offset:X4} 0x{Raw(mr):X8} {mr.FullName} :: {p}");
        }
    }
}
if (remaining.Count != 0)
    throw new InvalidDataException("malformed generic refs remain: " + string.Join(" | ", remaining.Take(12)));
Console.WriteLine("REOPEN_GENERIC_AUDIT_PASS suspicious_calls=0");

var untouchedAfter = after.Where(m => !changedSet.Contains(Raw(m))).ToDictionary(Raw, MethodSemantic);
if (untouchedAfter.Count != untouchedBefore.Count) throw new InvalidDataException("untouched method count drift");
var drift = untouchedBefore.Where(kv => !untouchedAfter.TryGetValue(kv.Key, out var a) || a != kv.Value).Select(kv => kv.Key).ToList();
if (drift.Count != 0)
    throw new InvalidDataException("semantic drift outside repaired methods: " + string.Join(',', drift.Take(20).Select(t => $"0x{t:X8}")));
Console.WriteLine($"SEMANTIC_ISOLATION_PASS untouched_methods={untouchedAfter.Count} changed_methods={changedSet.Count}");

var repairedStartup = after.Where(m => m.HasBody && (m.DeclaringType.FullName == "ResourceManager" || m.DeclaringType.FullName == "MainUIController"))
    .SelectMany(m => m.Body.Instructions.Select(i => (m, i)))
    .Where(x => x.i.Operand is MethodReference mr && mr.DeclaringType is GenericInstanceType gi &&
        (gi.ElementType.FullName == "System.Collections.Generic.List`1" || gi.ElementType.FullName == "System.Collections.Generic.Dictionary`2"))
    .ToList();
Console.WriteLine($"STARTUP_GENERIC_CALLS_REOPEN total={repairedStartup.Count}");
return 0;
