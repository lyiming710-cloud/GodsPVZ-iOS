using Mono.Cecil;
using Mono.Cecil.Cil;
using System.Globalization;
using System.Security.Cryptography;
using System.Text;

if (args.Length != 2)
{
    Console.Error.WriteLine("Usage: Stage9PlantDetailSerializeFieldPatch <input.dll> <output.dll>");
    return 2;
}

const string ExpectedInputSha256 = "0112cbc9df8c729e4b96d72af7dd3399706accf5c26196177b1b8d47f7e53ef9";
const int ExpectedMethodDefCount = 2317;
const string TargetType = "SeedChooserScreen/PlantDetail";
const string TargetField = "button_Almanac";
const string SerializeFieldAttribute = "UnityEngine.SerializeField";

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

using (var module = ModuleDefinition.ReadModule(input, new ReaderParameters { InMemory = true, ReadingMode = ReadingMode.Immediate }))
{
    var types = AllTypes(module.Types).ToList();
    var methods = types.SelectMany(t => t.Methods).ToList();
    if (methods.Count != ExpectedMethodDefCount) throw new InvalidDataException($"MethodDef count drifted: {methods.Count}");
    var fields = types.SelectMany(t => t.Fields).ToList();

    var type = types.SingleOrDefault(t => t.FullName == TargetType)
        ?? throw new InvalidDataException($"target type missing: {TargetType}");
    var field = type.Fields.SingleOrDefault(f => f.Name == TargetField)
        ?? throw new InvalidDataException($"target field missing: {TargetType}::{TargetField}");
    if (field.FieldType.FullName != "UnityEngine.GameObject" || !field.IsPrivate)
        throw new InvalidDataException($"target field signature drift: {field.FullName} attrs={field.Attributes}");
    if (!type.IsSerializable)
        throw new InvalidDataException("PlantDetail lost Serializable type flag");
    if (field.CustomAttributes.Any(a => a.AttributeType.FullName == SerializeFieldAttribute))
        throw new InvalidDataException("target already has SerializeField; refusing duplicate patch");

    var exemplar = fields.FirstOrDefault(f => f.CustomAttributes.Any(a => a.AttributeType.FullName == SerializeFieldAttribute))
        ?? throw new InvalidDataException("assembly contains no existing UnityEngine.SerializeField exemplar");
    var exemplarAttribute = exemplar.CustomAttributes.Single(a => a.AttributeType.FullName == SerializeFieldAttribute);

    Console.WriteLine($"TARGET_FIELD token=0x{Raw(field):X8} name={field.FullName} attrs={field.Attributes} custom_before={field.CustomAttributes.Count} serializable_parent=1");
    Console.WriteLine($"SERIALIZEFIELD_EXEMPLAR token=0x{Raw(exemplar):X8} name={exemplar.FullName}");
    Console.WriteLine("ASSET_AUTHORITY r3_sha256=d4264f12a00e86b6149d20ecf04caa9761af510aece6107bcceb155ea4ab0bbc prefab=Assets/Resources/prefabs/ui/SeedChooserScreen.prefab mono_fileID=114998874197659892 script_guid=60b84a767c46ced9aacc93f38da4d2f3 field=plantDetail.button_Almanac object_fileID=1354824845356902 object_name=Almanac");
    Console.WriteLine("NATIVE_AUTHORITY token=0x060006C2 address=0x00000001803A1C60 semantics=button_Almanac_required_nonnull_in_both_card_branches");

    field.CustomAttributes.Add(new CustomAttribute(module.ImportReference(exemplarAttribute.Constructor)));
    module.Write(output);
    Console.WriteLine("PATCH_PLANTDETAIL_SERIALIZEFIELD add=UnityEngine.SerializeField method_body_changes=0 field_type_changes=0 visibility_changes=0");
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
    var targetBefore = bf.Values.Single(f => f.DeclaringType.FullName == TargetType && f.Name == TargetField);
    var targetAfter = af.Values.Single(f => f.DeclaringType.FullName == TargetType && f.Name == TargetField);
    var targetToken = Raw(targetBefore);
    if (Raw(targetAfter) != targetToken) throw new InvalidDataException("target field token drift");
    int changed = 0;
    foreach (var kv in bf)
    {
        if (!af.TryGetValue(kv.Key, out var other)) throw new InvalidDataException($"missing field token=0x{kv.Key:X8}");
        if (FieldSemantic(kv.Value) != FieldSemantic(other))
        {
            changed++;
            if (kv.Key != targetToken) throw new InvalidDataException($"unexpected field metadata drift token=0x{kv.Key:X8}");
        }
    }
    if (changed != 1) throw new InvalidDataException($"expected exactly one changed field, got {changed}");
    var sfCount = targetAfter.CustomAttributes.Count(a => a.AttributeType.FullName == SerializeFieldAttribute);
    if (sfCount != 1 || !targetAfter.IsPrivate || targetAfter.FieldType.FullName != "UnityEngine.GameObject" || !targetAfter.DeclaringType.IsSerializable)
        throw new InvalidDataException("reopen target field metadata mismatch");
    Console.WriteLine($"REOPEN_SERIALIZEFIELD_PASS token=0x{targetToken:X8} custom_attr={SerializeFieldAttribute} count={sfCount} private=1 type=UnityEngine.GameObject serializable_parent=1");
    Console.WriteLine($"FIELD_METADATA_ISOLATION_PASS unchanged_fields={bf.Count - 1} changed_fields=1 target_token=0x{targetToken:X8}");
}

return 0;
