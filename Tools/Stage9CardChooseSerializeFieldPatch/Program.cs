using Mono.Cecil;
using Mono.Cecil.Cil;
using System.Globalization;
using System.Security.Cryptography;
using System.Text;

if (args.Length != 2)
{
    Console.Error.WriteLine("Usage: Stage9CardChooseSerializeFieldPatch <input.dll> <output.dll>");
    return 2;
}

const string ExpectedInputSha256 = "d255aec78c28b6269934c73f8581ce16cdc4209533172baa965977f0d63b7d89";
const int ExpectedMethodDefCount = 2317;
const string TargetType = "Card_Choose";
const string SerializeFieldAttribute = "UnityEngine.SerializeField";

var expectedFields = new Dictionary<string, string>(StringComparer.Ordinal)
{
    ["lockLogo"] = "UnityEngine.UI.Image",
    ["chain"] = "UnityEngine.UI.Image",
    ["chosen"] = "UnityEngine.UI.Image",
    ["Lv"] = "UnityEngine.UI.Image",
    ["background"] = "UnityEngine.UI.Image",
    ["foreground"] = "UnityEngine.UI.Image",
    ["orderArabesques"] = "UnityEngine.UI.Image",
    ["ordersImage"] = "UnityEngine.UI.Image[]",
    ["starsImage"] = "UnityEngine.UI.Image[]",
    ["plantPortrait"] = "UnityEngine.UI.Image",
    ["hormonLogo"] = "UnityEngine.UI.Image",
    ["dataBackground"] = "UnityEngine.UI.Image",
    ["skillLogo"] = "UnityEngine.UI.Image",
    ["cliqueLogo"] = "UnityEngine.UI.Image",
    ["sunPriceText"] = "TMPro.TextMeshProUGUI",
    ["levelText"] = "TMPro.TextMeshProUGUI",
    ["serialNumBeChosenText"] = "TMPro.TextMeshProUGUI",
};

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
    if (operand is float f) return $"R4:{BitConverter.SingleToInt32Bits(f):X8}";
    if (operand is double d) return $"R8:{BitConverter.DoubleToInt64Bits(d):X16}";
    return $"C:{Convert.ToString(operand, CultureInfo.InvariantCulture)}";
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

static string FieldSemantic(FieldDefinition f)
{
    var attrs = string.Join(',', f.CustomAttributes.Select(a => a.AttributeType.FullName).OrderBy(x => x, StringComparer.Ordinal));
    var constant = f.HasConstant ? Convert.ToString(f.Constant, CultureInfo.InvariantCulture) ?? "<null>" : "<none>";
    return $"{f.FullName}|{f.Attributes}|{f.FieldType.FullName}|const={constant}|ca={attrs}";
}

var input = Path.GetFullPath(args[0]);
var output = Path.GetFullPath(args[1]);
var inputSha = Sha256(input);
Console.WriteLine($"INPUT_SHA256 {inputSha}");
if (inputSha != ExpectedInputSha256) throw new InvalidDataException($"input SHA mismatch: {inputSha}");

var targetTokens = new HashSet<uint>();

using (var module = ModuleDefinition.ReadModule(input, new ReaderParameters { InMemory = true, ReadingMode = ReadingMode.Immediate }))
{
    var types = AllTypes(module.Types).ToList();
    var methods = types.SelectMany(t => t.Methods).ToList();
    if (methods.Count != ExpectedMethodDefCount) throw new InvalidDataException($"MethodDef count drifted: {methods.Count}");
    var fields = types.SelectMany(t => t.Fields).ToList();

    var type = types.SingleOrDefault(t => t.FullName == TargetType)
        ?? throw new InvalidDataException($"target type missing: {TargetType}");

    var targets = new List<FieldDefinition>();
    foreach (var spec in expectedFields)
    {
        var field = type.Fields.SingleOrDefault(f => f.Name == spec.Key)
            ?? throw new InvalidDataException($"target field missing: {TargetType}::{spec.Key}");
        if (!field.IsPrivate)
            throw new InvalidDataException($"target field lost private visibility: {field.FullName} attrs={field.Attributes}");
        if (field.FieldType.FullName != spec.Value)
            throw new InvalidDataException($"target field type drift: {field.FullName} expected={spec.Value} actual={field.FieldType.FullName}");
        if (field.IsStatic)
            throw new InvalidDataException($"target field unexpectedly static: {field.FullName}");
        if (field.CustomAttributes.Any(a => a.AttributeType.FullName == SerializeFieldAttribute))
            throw new InvalidDataException($"target already has SerializeField; refusing duplicate patch: {field.FullName}");
        targets.Add(field);
        targetTokens.Add(Raw(field));
        Console.WriteLine($"TARGET_FIELD token=0x{Raw(field):X8} name={field.Name} type={field.FieldType.FullName} private=1 serialized_before=0");
    }

    if (targets.Count != 17 || targetTokens.Count != 17)
        throw new InvalidDataException($"target set mismatch count={targets.Count} unique_tokens={targetTokens.Count}");

    var exemplar = fields.FirstOrDefault(f => f.CustomAttributes.Any(a => a.AttributeType.FullName == SerializeFieldAttribute))
        ?? throw new InvalidDataException("assembly contains no existing UnityEngine.SerializeField exemplar");
    var exemplarAttribute = exemplar.CustomAttributes.Single(a => a.AttributeType.FullName == SerializeFieldAttribute);
    Console.WriteLine($"SERIALIZEFIELD_EXEMPLAR token=0x{Raw(exemplar):X8} name={exemplar.FullName}");
    Console.WriteLine("ASSET_AUTHORITY r3_sha256=d4264f12a00e86b6149d20ecf04caa9761af510aece6107bcceb155ea4ab0bbc prefab=Assets/Resources/prefabs/card_choose/Card_Choose.prefab mono_fileID=114320256170494724 private_serialized_fields=17 ordersImage_refs=4 starsImage_refs=6");
    Console.WriteLine("NATIVE_AUTHORITY token=0x060002F2 address=0x0000000180343BE0 semantics=Initialize_dereferences_serialized_UI_refs_without_null_tolerant_fallback");

    foreach (var field in targets)
        field.CustomAttributes.Add(new CustomAttribute(module.ImportReference(exemplarAttribute.Constructor)));

    module.Write(output);
    Console.WriteLine("PATCH_CARD_CHOOSE_SERIALIZEFIELD add=UnityEngine.SerializeField fields=17 method_body_changes=0 field_type_changes=0 visibility_changes=0");
}

var outputSha = Sha256(output);
Console.WriteLine($"OUTPUT_SHA256 {outputSha}");
if (outputSha == ExpectedInputSha256) throw new InvalidDataException("output SHA unexpectedly identical to input");

using (var before = ModuleDefinition.ReadModule(input, new ReaderParameters { InMemory = true, ReadingMode = ReadingMode.Immediate }))
using (var after = ModuleDefinition.ReadModule(output, new ReaderParameters { InMemory = true, ReadingMode = ReadingMode.Immediate }))
{
    var beforeTypes = AllTypes(before.Types).ToList();
    var afterTypes = AllTypes(after.Types).ToList();
    var beforeMethods = beforeTypes.SelectMany(t => t.Methods).ToDictionary(Raw, MethodSemantic);
    var afterMethods = afterTypes.SelectMany(t => t.Methods).ToDictionary(Raw, MethodSemantic);
    if (beforeMethods.Count != ExpectedMethodDefCount || afterMethods.Count != ExpectedMethodDefCount)
        throw new InvalidDataException("MethodDef count changed across write");
    foreach (var kv in beforeMethods)
        if (!afterMethods.TryGetValue(kv.Key, out var s) || s != kv.Value)
            throw new InvalidDataException($"method semantic drift token=0x{kv.Key:X8}");
    Console.WriteLine($"METHOD_SEMANTIC_ISOLATION_PASS unchanged_methods={beforeMethods.Count}");

    var bf = beforeTypes.SelectMany(t => t.Fields).ToDictionary(Raw, x => x);
    var af = afterTypes.SelectMany(t => t.Fields).ToDictionary(Raw, x => x);
    if (bf.Count != af.Count) throw new InvalidDataException($"field count drift {bf.Count}->{af.Count}");

    int changed = 0;
    foreach (var kv in bf)
    {
        if (!af.TryGetValue(kv.Key, out var other)) throw new InvalidDataException($"missing field token=0x{kv.Key:X8}");
        var same = FieldSemantic(kv.Value) == FieldSemantic(other);
        if (!same)
        {
            changed++;
            if (!targetTokens.Contains(kv.Key))
                throw new InvalidDataException($"unexpected field metadata drift token=0x{kv.Key:X8}");
        }
        else if (targetTokens.Contains(kv.Key))
        {
            throw new InvalidDataException($"expected target field to change token=0x{kv.Key:X8}");
        }
    }
    if (changed != 17) throw new InvalidDataException($"expected exactly 17 changed fields, got {changed}");

    var afterType = afterTypes.Single(t => t.FullName == TargetType);
    foreach (var spec in expectedFields)
    {
        var field = afterType.Fields.Single(f => f.Name == spec.Key);
        if (!targetTokens.Contains(Raw(field))) throw new InvalidDataException($"target token set drift for {field.FullName}");
        var sfCount = field.CustomAttributes.Count(a => a.AttributeType.FullName == SerializeFieldAttribute);
        if (sfCount != 1 || !field.IsPrivate || field.IsStatic || field.FieldType.FullName != spec.Value)
            throw new InvalidDataException($"reopen target field metadata mismatch: {field.FullName} sf={sfCount} private={field.IsPrivate} static={field.IsStatic} type={field.FieldType.FullName}");
        Console.WriteLine($"REOPEN_SERIALIZEFIELD_PASS token=0x{Raw(field):X8} name={field.Name} custom_attr={SerializeFieldAttribute} count={sfCount} private=1 type={field.FieldType.FullName}");
    }

    Console.WriteLine($"FIELD_METADATA_ISOLATION_PASS unchanged_fields={bf.Count - 17} changed_fields=17 target_type={TargetType}");
}

return 0;
