using Mono.Cecil;
using Mono.Cecil.Cil;
using System.Globalization;
using System.Security.Cryptography;
using System.Text;

if (args.Length != 2)
{
    Console.Error.WriteLine("Usage: Stage9ResourceManagerStartPatch <input.dll> <output.dll>");
    return 2;
}

const string ExpectedInputSha256 = "9a01f9e44a056373ab230d121a8838e472cdd11a7b0d0d8ae662bb4ddb2148df";
const int ExpectedMethodDefCount = 2317;
const uint TargetToken = 0x06000216;
const string TargetType = "ResourceManager";
const string TargetMethod = "Start";

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
    if (operand is Instruction i) return $"I#{owner.Body.Instructions.IndexOf(i)}";
    if (operand is Instruction[] sw) return "SW[" + string.Join(',', sw.Select(i => owner.Body.Instructions.IndexOf(i))) + "]";
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

static bool IsCallTo(Instruction i, MethodDefinition m) =>
    (i.OpCode == OpCodes.Call || i.OpCode == OpCodes.Callvirt) &&
    i.Operand is MethodReference mr && mr.FullName == m.FullName;

var input = Path.GetFullPath(args[0]);
var output = Path.GetFullPath(args[1]);
var inputSha = Sha256(input);
Console.WriteLine($"INPUT_SHA256 {inputSha}");
if (inputSha != ExpectedInputSha256) throw new InvalidDataException($"input SHA mismatch: {inputSha}");

var beforeMethodSemantics = new Dictionary<uint, string>();
var beforeFieldSemantics = new Dictionary<uint, string>();

using (var module = ModuleDefinition.ReadModule(input, new ReaderParameters { InMemory = true, ReadingMode = ReadingMode.Immediate }))
{
    var types = AllTypes(module.Types).ToList();
    var methods = types.SelectMany(t => t.Methods).ToList();
    var fields = types.SelectMany(t => t.Fields).ToList();
    if (methods.Count != ExpectedMethodDefCount) throw new InvalidDataException($"MethodDef count drifted: {methods.Count}");
    foreach (var m in methods) beforeMethodSemantics[Raw(m)] = MethodSemantic(m);
    foreach (var f in fields) beforeFieldSemantics[Raw(f)] = FieldSemantic(f);

    var type = types.SingleOrDefault(t => t.FullName == TargetType)
        ?? throw new InvalidDataException($"target type missing: {TargetType}");
    var start = type.Methods.SingleOrDefault(m => m.Name == TargetMethod && m.IsStatic && m.Parameters.Count == 0)
        ?? throw new InvalidDataException("ResourceManager.Start() target missing/ambiguous");
    if (Raw(start) != TargetToken) throw new InvalidDataException($"target token mismatch: 0x{Raw(start):X8}");
    if (!start.HasBody || start.Body.CodeSize != 421) throw new InvalidDataException($"unexpected prepatch Start body size={start.Body.CodeSize}");

    var loadAudio = type.Methods.Single(m => m.Name == "LoadAudioClips" && m.IsStatic && m.Parameters.Count == 0);
    var loadSprites = type.Methods.Single(m => m.Name == "LoadSprites" && m.IsStatic && m.Parameters.Count == 0);
    var audioCalls = start.Body.Instructions.Where(i => IsCallTo(i, loadAudio)).ToList();
    var spriteCalls = start.Body.Instructions.Where(i => IsCallTo(i, loadSprites)).ToList();
    if (audioCalls.Count != 1) throw new InvalidDataException($"expected one LoadAudioClips call, got {audioCalls.Count}");
    if (spriteCalls.Count != 0) throw new InvalidDataException($"LoadSprites already present; refusing duplicate patch count={spriteCalls.Count}");

    var audio = audioCalls[0];
    var audioIndex = start.Body.Instructions.IndexOf(audio);
    var next = audio.Next ?? throw new InvalidDataException("LoadAudioClips unexpectedly last instruction");
    if (next.OpCode != OpCodes.Ldstr || (string?)next.Operand != "prefabs/Card/CardTemplate")
        throw new InvalidDataException($"prepatch fingerprint drift after LoadAudioClips: {next.OpCode} {next.Operand}");

    Console.WriteLine($"TARGET_METHOD token=0x{Raw(start):X8} type={TargetType} method={TargetMethod} code_size={start.Body.CodeSize} load_audio_calls=1 load_sprites_calls=0 audio_instruction_index={audioIndex}");
    Console.WriteLine("NATIVE_AUTHORITY token=0x06000216 address=0x000000018033BF30 load_audio_call=0x000000018033C4B4 target=0x0000000180330340 load_sprites_call=0x000000018033C4BB target=0x0000000180333DB0 order=LoadAudioClips_then_LoadSprites_then_prefab_loads");
    Console.WriteLine("RUNTIME_CAUSAL_EVIDENCE strict_run=34763654549 candidate=9a01f9e44a056373ab230d121a8838e472cdd11a7b0d0d8ae662bb4ddb2148df exception=ArgumentOutOfRangeException method=Card_Choose.Initialize il_offset=0x00129 expression=ResourceManager.cliqueLogos[(int)plantSave.clique] plant0_clique=1");

    var il = start.Body.GetILProcessor();
    il.InsertAfter(audio, il.Create(OpCodes.Call, loadSprites));

    module.Write(output);
    Console.WriteLine("PATCH_RESOURCE_MANAGER_START insert_call=ResourceManager.LoadSprites position=after_LoadAudioClips before_prefab_loads method_body_changes=1 field_metadata_changes=0");
}

var outputSha = Sha256(output);
Console.WriteLine($"OUTPUT_SHA256 {outputSha}");
if (outputSha == ExpectedInputSha256) throw new InvalidDataException("output SHA unexpectedly identical to input");

using (var after = ModuleDefinition.ReadModule(output, new ReaderParameters { InMemory = true, ReadingMode = ReadingMode.Immediate }))
{
    var types = AllTypes(after.Types).ToList();
    var methods = types.SelectMany(t => t.Methods).ToList();
    var fields = types.SelectMany(t => t.Fields).ToList();
    if (methods.Count != ExpectedMethodDefCount) throw new InvalidDataException($"postwrite MethodDef count drifted: {methods.Count}");

    var type = types.Single(t => t.FullName == TargetType);
    var start = type.Methods.Single(m => m.Name == TargetMethod && m.IsStatic && m.Parameters.Count == 0);
    var loadAudio = type.Methods.Single(m => m.Name == "LoadAudioClips" && m.IsStatic && m.Parameters.Count == 0);
    var loadSprites = type.Methods.Single(m => m.Name == "LoadSprites" && m.IsStatic && m.Parameters.Count == 0);
    var audioCalls = start.Body.Instructions.Where(i => IsCallTo(i, loadAudio)).ToList();
    var spriteCalls = start.Body.Instructions.Where(i => IsCallTo(i, loadSprites)).ToList();
    if (audioCalls.Count != 1 || spriteCalls.Count != 1) throw new InvalidDataException($"reopen call counts audio={audioCalls.Count} sprites={spriteCalls.Count}");
    var ai = start.Body.Instructions.IndexOf(audioCalls[0]);
    var si = start.Body.Instructions.IndexOf(spriteCalls[0]);
    var card = start.Body.Instructions.Select((ins, idx) => (ins, idx)).Single(x => x.ins.OpCode == OpCodes.Ldstr && (string?)x.ins.Operand == "prefabs/Card/CardTemplate").idx;
    if (!(ai < si && si < card && si == ai + 1)) throw new InvalidDataException($"reopen order mismatch audio={ai} sprites={si} card={card}");
    Console.WriteLine($"REOPEN_RESOURCE_MANAGER_START_PASS token=0x{Raw(start):X8} load_audio_calls=1 load_sprites_calls=1 order=audio<{si}<card card_instruction_index={card}");

    int changedMethods = 0, untouchedMethods = 0;
    foreach (var m in methods)
    {
        if (!beforeMethodSemantics.TryGetValue(Raw(m), out var old)) throw new InvalidDataException($"new method token=0x{Raw(m):X8}");
        var same = old == MethodSemantic(m);
        if (same) untouchedMethods++;
        else
        {
            changedMethods++;
            if (Raw(m) != TargetToken) throw new InvalidDataException($"unexpected method semantic drift token=0x{Raw(m):X8} {m.FullName}");
        }
    }
    if (changedMethods != 1 || untouchedMethods != 2316)
        throw new InvalidDataException($"method isolation mismatch untouched={untouchedMethods} changed={changedMethods}");
    Console.WriteLine($"SEMANTIC_ISOLATION_PASS untouched_methods={untouchedMethods} changed_methods={changedMethods} target_token=0x{TargetToken:X8}");

    if (fields.Count != beforeFieldSemantics.Count) throw new InvalidDataException($"field count drift {beforeFieldSemantics.Count}->{fields.Count}");
    foreach (var f in fields)
        if (!beforeFieldSemantics.TryGetValue(Raw(f), out var old) || old != FieldSemantic(f))
            throw new InvalidDataException($"field metadata drift token=0x{Raw(f):X8} {f.FullName}");
    Console.WriteLine($"FIELD_METADATA_ISOLATION_PASS unchanged_fields={fields.Count} changed_fields=0");

    var cardChoose = types.Single(t => t.FullName == "Card_Choose");
    var sfCount = cardChoose.Fields.Count(f => f.CustomAttributes.Any(a => a.AttributeType.FullName == "UnityEngine.SerializeField"));
    if (sfCount != 17) throw new InvalidDataException($"Card_Choose SerializeField preservation failed: {sfCount}");
    Console.WriteLine("CARD_CHOOSE_SERIALIZEFIELD_PRESERVATION_PASS fields=17");
}

return 0;
