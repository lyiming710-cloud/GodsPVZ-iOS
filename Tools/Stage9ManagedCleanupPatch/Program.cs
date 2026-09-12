using Mono.Cecil;
using Mono.Cecil.Cil;
using System.Security.Cryptography;
using System.Text;

if (args.Length != 2)
{
    Console.Error.WriteLine("Usage: GodsPVZ.Stage9ManagedCleanupPatch <input.dll> <output.dll>");
    return 2;
}

const string ExpectedInputSha256 = "00ba34c43e08c73c3cc8a39e0bf7f0d35fd280e360dd8a2907e9304d93343980";
const int ExpectedMethodDefCount = 2317;
const int ExpectedHelperCallCount = 3866;
const uint ResourceLoadBoardsToken = 0x06000220u;
const uint ParticlesStartToken = 0x060001E8u;
const uint ButtonAwakeToken = 0x060005F8u;

var input = Path.GetFullPath(args[0]);
var output = Path.GetFullPath(args[1]);
var inputSha = Convert.ToHexString(SHA256.HashData(File.ReadAllBytes(input))).ToLowerInvariant();
Console.WriteLine($"POSTMECHANICAL_INPUT_SHA256 {inputSha}");
if (inputSha != ExpectedInputSha256)
    throw new InvalidDataException($"Stage9 managed cleanup input SHA mismatch: expected {ExpectedInputSha256}, got {inputSha}");

static IEnumerable<TypeDefinition> AllTypes(IEnumerable<TypeDefinition> roots)
{
    foreach (var t in roots)
    {
        yield return t;
        foreach (var n in AllTypes(t.NestedTypes)) yield return n;
    }
}

static uint RawToken(IMetadataTokenProvider p) => p.MetadataToken.ToUInt32();

static string OperandSemantic(object? operand, MethodDefinition owner)
{
    if (operand is null) return "";
    if (operand is Instruction bi)
        return $"I#{owner.Body.Instructions.IndexOf(bi)}";
    if (operand is Instruction[] sw)
        return "SW[" + string.Join(',', sw.Select(x => owner.Body.Instructions.IndexOf(x))) + "]";
    if (operand is MethodReference mr) return $"M:{mr.FullName}";
    if (operand is FieldReference fr) return $"F:{fr.FullName}";
    if (operand is TypeReference tr) return $"T:{tr.FullName}";
    if (operand is VariableDefinition vr) return $"V:{vr.Index}:{vr.VariableType.FullName}";
    if (operand is ParameterDefinition pr) return $"P:{pr.Index}:{pr.ParameterType.FullName}";
    if (operand is string s) return $"S:{Convert.ToBase64String(Encoding.UTF8.GetBytes(s))}";
    if (operand is float f) return $"R4:{BitConverter.SingleToInt32Bits(f):X8}";
    if (operand is double d) return $"R8:{BitConverter.DoubleToInt64Bits(d):X16}";
    return $"C:{Convert.ToString(operand, System.Globalization.CultureInfo.InvariantCulture)}";
}

static string MethodSemantic(MethodDefinition m)
{
    var sb = new StringBuilder();
    sb.Append(m.FullName).Append('|').Append(m.Attributes).Append('|').Append(m.ImplAttributes).Append('|');
    if (!m.HasBody) return sb.Append("NOBODY").ToString();
    sb.Append("init=").Append(m.Body.InitLocals).Append(";max=").Append(m.Body.MaxStackSize).Append(';');
    foreach (var v in m.Body.Variables)
        sb.Append("L:").Append(v.Index).Append(':').Append(v.VariableType.FullName).Append(';');
    foreach (var i in m.Body.Instructions)
        sb.Append("I:").Append(i.OpCode.Code).Append(':').Append(OperandSemantic(i.Operand, m)).Append(';');
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

static bool IsHelperCall(Instruction i)
{
    return i.OpCode.Code is Code.Call or Code.Callvirt
        && i.Operand is MethodReference mr
        && mr.DeclaringType.FullName == "Cpp2ILInjected.Cpp2ILHelpers"
        && mr.Name == "NoteDecompilerIssue"
        && mr.ReturnType.FullName == "System.Void"
        && mr.Parameters.Count == 1
        && mr.Parameters[0].ParameterType.FullName == "System.String";
}

static HashSet<Instruction> ControlFlowTargets(MethodDefinition m)
{
    var targets = new HashSet<Instruction>();
    foreach (var i in m.Body.Instructions)
    {
        if (i.Operand is Instruction q) targets.Add(q);
        else if (i.Operand is Instruction[] sw) foreach (var q2 in sw) targets.Add(q2);
    }
    foreach (var eh in m.Body.ExceptionHandlers)
    {
        foreach (var q in new[] { eh.TryStart, eh.TryEnd, eh.HandlerStart, eh.HandlerEnd, eh.FilterStart })
            if (q is not null) targets.Add(q);
    }
    return targets;
}

static MethodReference CloneMethodRefForDeclaring(MethodReference template, TypeReference declaring)
{
    var r = new MethodReference(template.Name, template.ReturnType, declaring)
    {
        HasThis = template.HasThis,
        ExplicitThis = template.ExplicitThis,
        CallingConvention = template.CallingConvention
    };
    foreach (var p in template.Parameters)
        r.Parameters.Add(new ParameterDefinition(p.Name, p.Attributes, p.ParameterType));
    return r;
}

static int? LoadLocalAddressIndex(Instruction i)
{
    if (i.OpCode.Code is not (Code.Ldloca or Code.Ldloca_S)) return null;
    return i.Operand is VariableDefinition v ? v.Index : null;
}

static int? StoreLocalIndex(Instruction i)
{
    return i.OpCode.Code switch
    {
        Code.Stloc_0 => 0,
        Code.Stloc_1 => 1,
        Code.Stloc_2 => 2,
        Code.Stloc_3 => 3,
        Code.Stloc or Code.Stloc_S when i.Operand is VariableDefinition v => v.Index,
        _ => null
    };
}

using var module = ModuleDefinition.ReadModule(input, new ReaderParameters { InMemory = true, ReadingMode = ReadingMode.Immediate });
var allBefore = AllTypes(module.Types).SelectMany(t => t.Methods).ToList();
if (allBefore.Count != ExpectedMethodDefCount)
    throw new InvalidDataException($"Stage9 managed cleanup MethodDef count drifted: {allBefore.Count}");

var helperMethods = new HashSet<uint>();
int helperCallsBefore = 0;
foreach (var m in allBefore.Where(x => x.HasBody))
{
    foreach (var i in m.Body.Instructions)
    {
        if (!IsHelperCall(i)) continue;
        helperCallsBefore++;
        helperMethods.Add(RawToken(m));
    }
}
if (helperCallsBefore != ExpectedHelperCallCount)
    throw new InvalidDataException($"Expected {ExpectedHelperCallCount} Cpp2IL helper calls, got {helperCallsBefore}");

var changedMethods = new HashSet<uint>(helperMethods)
{
    ResourceLoadBoardsToken,
    ParticlesStartToken,
    ButtonAwakeToken
};

var untouchedBefore = allBefore
    .Where(m => !changedMethods.Contains(RawToken(m)))
    .ToDictionary(m => RawToken(m), MethodSemantic);

int helperCallsNeutralized = 0;
int helperTargetedCalls = 0;
foreach (var m in allBefore.Where(x => x.HasBody))
{
    var targets = ControlFlowTargets(m);
    var ins = m.Body.Instructions;
    for (int j = 1; j < ins.Count; j++)
    {
        var call = ins[j];
        if (!IsHelperCall(call)) continue;
        var text = ins[j - 1];
        if (text.OpCode.Code != Code.Ldstr || text.Operand is not string)
            throw new InvalidDataException($"Cpp2IL helper call lost adjacent ldstr: 0x{RawToken(m):X8} {m.FullName} IL_{call.Offset:X4}");
        if (targets.Contains(call)) helperTargetedCalls++;
        // Preserve the original stack effect and instruction identity. The synthetic
        // sequence ldstr; call void NoteDecompilerIssue(string) becomes ldstr; pop.
        // This remains correct even when the call instruction itself is a branch/EH target.
        call.OpCode = OpCodes.Pop;
        call.Operand = null;
        helperCallsNeutralized++;
    }
}
if (helperCallsNeutralized != ExpectedHelperCallCount)
    throw new InvalidDataException($"Expected to neutralize {ExpectedHelperCallCount} helper calls, got {helperCallsNeutralized}");
Console.WriteLine($"SYNTHETIC_HELPERS_NEUTRALIZED calls={helperCallsNeutralized} methods={helperMethods.Count} targeted_calls={helperTargetedCalls} strategy=call_to_pop");

var goodListAdd = allBefore.Where(x => x.HasBody).SelectMany(x => x.Body.Instructions)
    .Select(i => i.Operand as MethodReference)
    .FirstOrDefault(mr => mr is not null
        && mr.Name == "Add"
        && mr.DeclaringType is GenericInstanceType gi
        && gi.ElementType.FullName == "System.Collections.Generic.List`1"
        && mr.Parameters.Count == 1
        && mr.Parameters[0].ParameterType is GenericParameter gp
        && gp.Type == GenericParameterType.Type)
    ?? throw new InvalidDataException("Could not find a known-good List<T>.Add(!0) MemberRef template");

var goodDictAdd = allBefore.Where(x => x.HasBody).SelectMany(x => x.Body.Instructions)
    .Select(i => i.Operand as MethodReference)
    .FirstOrDefault(mr => mr is not null
        && mr.Name == "Add"
        && mr.DeclaringType is GenericInstanceType gi
        && gi.ElementType.FullName == "System.Collections.Generic.Dictionary`2"
        && mr.Parameters.Count == 2
        && mr.Parameters[0].ParameterType is GenericParameter gp0
        && gp0.Type == GenericParameterType.Type
        && mr.Parameters[1].ParameterType is GenericParameter gp1
        && gp1.Type == GenericParameterType.Type)
    ?? throw new InvalidDataException("Could not find a known-good Dictionary<TKey,TValue>.Add(!0,!1) MemberRef template");

MethodDefinition MethodByToken(uint token, string type, string name)
{
    var m = module.LookupToken(new MetadataToken(TokenType.Method, (int)(token & 0x00FFFFFF))) as MethodDefinition
        ?? throw new InvalidDataException($"Missing target method 0x{token:X8}");
    if (m.DeclaringType.FullName != type || m.Name != name || !m.HasBody)
        throw new InvalidDataException($"Target method drift 0x{token:X8}: {m.FullName}");
    return m;
}

var loadBoards = MethodByToken(ResourceLoadBoardsToken, "ResourceManager", "Load_boardPrefabs");
int listAddFixed = 0;
foreach (var i in loadBoards.Body.Instructions)
{
    if (i.Operand is not MethodReference mr || mr.Name != "Add" || mr.DeclaringType is not GenericInstanceType gi) continue;
    if (gi.FullName != "System.Collections.Generic.List`1<Board>") continue;
    if (mr.Parameters.Count != 1 || mr.Parameters[0].ParameterType.FullName != "Board") continue;
    i.Operand = CloneMethodRefForDeclaring(goodListAdd, mr.DeclaringType);
    listAddFixed++;
}
if (listAddFixed != 4)
    throw new InvalidDataException($"Expected four List<Board>.Add concrete-signature repairs, got {listAddFixed}");
Console.WriteLine("GENERIC_MEMBERREF_FIX ResourceManager.Load_boardPrefabs List<Board>.Add(Board) -> Add(!0) uses=4");

var particlesStart = MethodByToken(ParticlesStartToken, "ParticlesManager", "Start");
int dictAddFixed = 0;
foreach (var i in particlesStart.Body.Instructions)
{
    if (i.Operand is not MethodReference mr || mr.Name != "Add" || mr.DeclaringType is not GenericInstanceType gi) continue;
    if (gi.FullName != "System.Collections.Generic.Dictionary`2<ParticleState,UnityEngine.GameObject>") continue;
    if (mr.Parameters.Count != 2
        || mr.Parameters[0].ParameterType.FullName != "ParticleState"
        || mr.Parameters[1].ParameterType.FullName != "UnityEngine.GameObject") continue;
    i.Operand = CloneMethodRefForDeclaring(goodDictAdd, mr.DeclaringType);
    dictAddFixed++;
}
if (dictAddFixed != 2)
    throw new InvalidDataException($"Expected two ParticleDictionary.Add concrete-signature repairs, got {dictAddFixed}");
Console.WriteLine("GENERIC_MEMBERREF_FIX ParticlesManager.Start Dictionary<ParticleState,GameObject>.Add(concrete) -> Add(!0,!1) uses=2");

var buttonType = AllTypes(module.Types).SingleOrDefault(t => t.FullName == "Button_z")
    ?? throw new InvalidDataException("Button_z type missing");
var colorFields = buttonType.Fields.Where(f => f.FieldType.FullName == "UnityEngine.Color").ToList();
Console.WriteLine("BUTTON_Z_COLOR_FIELDS " + string.Join(",", colorFields.Select(f => $"0x{RawToken(f):X8}:{f.Name}")));
var normalText = colorFields.SingleOrDefault(f => f.Name == "normalColor_Text")
    ?? throw new InvalidDataException("Button_z.normalColor_Text missing or duplicated");
var imageNormalCandidates = colorFields
    .Where(f => f != normalText
        && f.Name.Contains("normal", StringComparison.OrdinalIgnoreCase)
        && !f.Name.Contains("Text", StringComparison.OrdinalIgnoreCase))
    .ToList();
if (imageNormalCandidates.Count != 1)
    throw new InvalidDataException("Could not uniquely identify Button_z image normal Color field; candidates=" + string.Join(',', imageNormalCandidates.Select(f => f.Name)));
var normalImage = imageNormalCandidates[0];
Console.WriteLine($"BUTTON_Z_IMAGE_NORMAL_FIELD 0x{RawToken(normalImage):X8}:{normalImage.Name}");

var buttonAwake = MethodByToken(ButtonAwakeToken, "Button_z", "Awake");
var buttonTargets = ControlFlowTargets(buttonAwake);
var bi = buttonAwake.Body.Instructions;

int textSetterFix = 0;
for (int k = 1; k + 3 < bi.Count; k++)
{
    if (bi[k].OpCode.Code != Code.Call || bi[k].Operand is not MethodReference setter) continue;
    if (setter.DeclaringType.FullName != "TMPro.TMP_Text" || setter.Name != "set_color" || setter.Parameters.Count != 1 || setter.Parameters[0].ParameterType.FullName != "UnityEngine.Color") continue;
    var prev = bi[k - 1]; var n1 = bi[k + 1]; var n2 = bi[k + 2]; var n3 = bi[k + 3];
    var cvIndex = LoadLocalAddressIndex(prev);
    if (cvIndex is null || cvIndex < 0 || cvIndex >= buttonAwake.Body.Variables.Count || buttonAwake.Body.Variables[cvIndex.Value].VariableType.FullName != "UnityEngine.Color")
        throw new InvalidDataException($"Button_z text setter source pattern drift: prev={prev.OpCode.Code} operand={prev.Operand}");
    if (n1.OpCode.Code != Code.Ldarg_0 || n2.OpCode.Code != Code.Ldfld || n2.Operand is not FieldReference nf || nf.MetadataToken.ToUInt32() != normalText.MetadataToken.ToUInt32())
        throw new InvalidDataException($"Button_z text normalColor load pattern drift: next={n1.OpCode.Code},{n2.OpCode.Code} field={n2.Operand}");
    if (StoreLocalIndex(n3) != cvIndex)
        throw new InvalidDataException($"Button_z text temporary store pattern drift: store={n3.OpCode.Code} operand={n3.Operand} expectedLocal={cvIndex}");
    if (new[] { prev, bi[k], n1, n2, n3 }.Any(buttonTargets.Contains))
        throw new InvalidDataException("Button_z text color repair span unexpectedly contains a branch/EH target");
    prev.OpCode = OpCodes.Ldarg_0; prev.Operand = null;
    bi[k].OpCode = OpCodes.Ldfld; bi[k].Operand = normalText;
    n1.OpCode = OpCodes.Call; n1.Operand = setter;
    n2.OpCode = OpCodes.Nop; n2.Operand = null;
    n3.OpCode = OpCodes.Nop; n3.Operand = null;
    textSetterFix++;
}
if (textSetterFix != 1)
    throw new InvalidDataException($"Expected one Button_z TMP color setter repair, got {textSetterFix}");

int imageSetterFix = 0;
for (int k = 1; k < bi.Count; k++)
{
    if (bi[k].OpCode.Code != Code.Call || bi[k].Operand is not MethodReference setter) continue;
    if (setter.DeclaringType.FullName != "UnityEngine.UI.Graphic" || setter.Name != "set_color" || setter.Parameters.Count != 1 || setter.Parameters[0].ParameterType.FullName != "UnityEngine.Color") continue;
    var prev = bi[k - 1];
    var cvIndex = LoadLocalAddressIndex(prev);
    if (cvIndex is null || cvIndex < 0 || cvIndex >= buttonAwake.Body.Variables.Count || buttonAwake.Body.Variables[cvIndex.Value].VariableType.FullName != "UnityEngine.Color")
        throw new InvalidDataException($"Button_z image setter source pattern drift: prev={prev.OpCode.Code} operand={prev.Operand}");
    if (buttonTargets.Contains(prev) || buttonTargets.Contains(bi[k]))
        throw new InvalidDataException("Button_z image color repair span unexpectedly contains a branch/EH target");
    var savedOp = bi[k].OpCode;
    prev.OpCode = OpCodes.Ldarg_0; prev.Operand = null;
    bi[k].OpCode = OpCodes.Ldfld; bi[k].Operand = normalImage;
    buttonAwake.Body.GetILProcessor().InsertAfter(bi[k], Instruction.Create(savedOp, setter));
    imageSetterFix++;
    break;
}
if (imageSetterFix != 1)
    throw new InvalidDataException($"Expected one Button_z Image color setter repair, got {imageSetterFix}");
Console.WriteLine($"BUTTON_Z_COLOR_STACK_FIX text=1 image=1 image_field={normalImage.Name}");

module.Write(output);
var outputSha = Convert.ToHexString(SHA256.HashData(File.ReadAllBytes(output))).ToLowerInvariant();
Console.WriteLine($"OUTPUT_SHA256 {outputSha}");

using var reopen = ModuleDefinition.ReadModule(output, new ReaderParameters { InMemory = true, ReadingMode = ReadingMode.Immediate });
var allAfter = AllTypes(reopen.Types).SelectMany(t => t.Methods).ToList();
if (allAfter.Count != ExpectedMethodDefCount)
    throw new InvalidDataException($"Reopen MethodDef count drifted: {allAfter.Count}");

int helperCallsAfter = allAfter.Where(x => x.HasBody).SelectMany(x => x.Body.Instructions).Count(IsHelperCall);
if (helperCallsAfter != 0)
    throw new InvalidDataException($"Cpp2IL helper calls remain after reopen: {helperCallsAfter}");

MethodDefinition ReopenMethod(uint token, string type, string name)
{
    var m = reopen.LookupToken(new MetadataToken(TokenType.Method, (int)(token & 0x00FFFFFF))) as MethodDefinition
        ?? throw new InvalidDataException($"Reopen target missing 0x{token:X8}");
    if (m.DeclaringType.FullName != type || m.Name != name || !m.HasBody)
        throw new InvalidDataException($"Reopen target drift 0x{token:X8}: {m.FullName}");
    return m;
}

var rBoards = ReopenMethod(ResourceLoadBoardsToken, "ResourceManager", "Load_boardPrefabs");
var rListAdds = rBoards.Body.Instructions
    .Select(i => i.Operand as MethodReference)
    .Where(mr => mr is not null && mr.Name == "Add" && mr.DeclaringType.FullName == "System.Collections.Generic.List`1<Board>")
    .Cast<MethodReference>().ToList();
if (rListAdds.Count != 4 || rListAdds.Any(mr => mr.Parameters.Count != 1 || mr.Parameters[0].ParameterType.FullName != "!0"))
    throw new InvalidDataException("List<Board>.Add reopen generic signature validation failed");

var rParticles = ReopenMethod(ParticlesStartToken, "ParticlesManager", "Start");
var rDictAdds = rParticles.Body.Instructions
    .Select(i => i.Operand as MethodReference)
    .Where(mr => mr is not null && mr.Name == "Add" && mr.DeclaringType.FullName == "System.Collections.Generic.Dictionary`2<ParticleState,UnityEngine.GameObject>")
    .Cast<MethodReference>().ToList();
if (rDictAdds.Count != 2 || rDictAdds.Any(mr => mr.Parameters.Count != 2 || mr.Parameters[0].ParameterType.FullName != "!0" || mr.Parameters[1].ParameterType.FullName != "!1"))
    throw new InvalidDataException("ParticleDictionary.Add reopen generic signature validation failed");

var rButton = ReopenMethod(ButtonAwakeToken, "Button_z", "Awake");
int rTextGood = 0, rImageGood = 0;
for (int k = 1; k < rButton.Body.Instructions.Count; k++)
{
    var cur = rButton.Body.Instructions[k];
    if (cur.OpCode.Code != Code.Call || cur.Operand is not MethodReference setter || setter.Name != "set_color") continue;
    var prev = rButton.Body.Instructions[k - 1];
    if (setter.DeclaringType.FullName == "TMPro.TMP_Text" && prev.OpCode.Code == Code.Ldfld && prev.Operand is FieldReference tf && tf.Name == normalText.Name) rTextGood++;
    if (setter.DeclaringType.FullName == "UnityEngine.UI.Graphic" && prev.OpCode.Code == Code.Ldfld && prev.Operand is FieldReference imf && imf.Name == normalImage.Name) rImageGood++;
}
if (rTextGood != 1 || rImageGood != 1)
    throw new InvalidDataException($"Button_z color setter reopen validation failed text={rTextGood} image={rImageGood}");

var untouchedAfter = allAfter.Where(m => !changedMethods.Contains(RawToken(m))).ToDictionary(m => RawToken(m), MethodSemantic);
if (untouchedAfter.Count != untouchedBefore.Count)
    throw new InvalidDataException($"Untouched MethodDef count drifted: before={untouchedBefore.Count} after={untouchedAfter.Count}");
var semanticDrift = new List<uint>();
foreach (var (token, before) in untouchedBefore)
{
    if (!untouchedAfter.TryGetValue(token, out var after) || before != after) semanticDrift.Add(token);
}
if (semanticDrift.Count != 0)
    throw new InvalidDataException("Unexpected semantic drift outside proven cleanup targets: " + string.Join(',', semanticDrift.Select(t => $"0x{t:X8}")));

Console.WriteLine($"REOPEN_HELPER_CALLS {helperCallsAfter}");
Console.WriteLine("REOPEN_GENERIC_MEMBERREF_PASS list_board=4 particle_dict=2");
Console.WriteLine($"REOPEN_BUTTON_Z_PASS text={rTextGood} image={rImageGood}");
Console.WriteLine($"SEMANTIC_ISOLATION_PASS untouched_methods={untouchedAfter.Count} changed_methods={changedMethods.Count}");
return 0;
