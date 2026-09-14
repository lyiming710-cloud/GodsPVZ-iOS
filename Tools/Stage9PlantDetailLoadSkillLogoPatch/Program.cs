using Mono.Cecil;
using Mono.Cecil.Cil;
using System.Globalization;
using System.Security.Cryptography;
using System.Text;

if (args.Length != 2)
{
    Console.Error.WriteLine("Usage: Stage9PlantDetailLoadSkillLogoPatch <input.dll> <output.dll>");
    return 2;
}

const string ExpectedInputSha = "273ddef385775d169aa88d63a505aeb2b99aab4b8c32204a4929175310e2969b";
const int ExpectedMethods = 2317;
const int ExpectedFields = 2802;
const uint TargetToken = 0x06000668;
const int ExpectedRid = 1640;
const int ExpectedOldCodeSize = 440;
const int ExpectedOldLocals = 21;
const string NativeSliceSha = "cc2dbd31e0ffb8e304825a7cfa00fb876f9ae902b01cf4192d6fd031ed59a3bf";

var PreservationTokens = new HashSet<uint>
{
    0x0600067E, 0x06000285, 0x06000289, 0x0600028A, // gameplay Batch1
    0x060003BA,                                     // Plant::.ctor
    0x060003FD,                                     // Projectile::.ctor
    0x060003DD, 0x060003DE,                        // Projectile.ResetData / BindTrack
    0x060001F3, 0x06000216,                        // ProjectileManager.Start / ResourceManager.Start
    0x06000667                                      // runtime-validated Plant_DetaiPage.Initialize
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
static string Sha(string p) => Convert.ToHexString(SHA256.HashData(File.ReadAllBytes(p))).ToLowerInvariant();
static string ScopeName(TypeReference t) => t.Scope switch
{
    AssemblyNameReference a => a.Name,
    ModuleDefinition m => m.Assembly?.Name?.Name ?? m.Name,
    ModuleReference mr => mr.Name,
    _ => t.Scope?.ToString() ?? "<null>"
};
static string TypeSig(TypeReference t) => $"{t.FullName}@{ScopeName(t)}";
static string OperandSig(object? o, MethodDefinition owner)
{
    if (o is null) return "";
    if (o is Instruction i) return $"I#{owner.Body.Instructions.IndexOf(i)}";
    if (o is Instruction[] sw) return "SW[" + string.Join(',', sw.Select(i => owner.Body.Instructions.IndexOf(i))) + "]";
    if (o is MethodReference mr) return $"M:{mr.FullName}@{ScopeName(mr.DeclaringType)}";
    if (o is FieldReference fr) return $"F:{fr.FullName}@{ScopeName(fr.DeclaringType)}";
    if (o is TypeReference tr) return $"T:{TypeSig(tr)}";
    if (o is VariableDefinition vr) return $"V:{vr.Index}:{TypeSig(vr.VariableType)}";
    if (o is ParameterDefinition pr) return $"P:{pr.Index}:{TypeSig(pr.ParameterType)}";
    if (o is string s) return "S:" + Convert.ToBase64String(Encoding.UTF8.GetBytes(s));
    if (o is float f) return $"R4:{BitConverter.SingleToInt32Bits(f):X8}";
    if (o is double d) return $"R8:{BitConverter.DoubleToInt64Bits(d):X16}";
    return "C:" + Convert.ToString(o, CultureInfo.InvariantCulture);
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
    var c = f.HasConstant ? Convert.ToString(f.Constant, CultureInfo.InvariantCulture) ?? "<null>" : "<none>";
    return $"{f.FullName}|{f.Attributes}|{TypeSig(f.FieldType)}|const={c}|ca={attrs}";
}
static MethodReference FindInMethod(MethodDefinition m, Func<MethodReference, bool> pred, string label)
{
    var hits = m.Body.Instructions
        .Where(i => (i.OpCode == OpCodes.Call || i.OpCode == OpCodes.Callvirt || i.OpCode == OpCodes.Newobj) && i.Operand is MethodReference mr && pred(mr))
        .Select(i => (MethodReference)i.Operand)
        .DistinctBy(mr => mr.FullName + "@" + ScopeName(mr.DeclaringType))
        .ToList();
    if (hits.Count != 1) throw new InvalidDataException($"reference lookup {label} count={hits.Count}");
    return hits[0];
}
static int CountCalls(MethodDefinition m, Func<MethodReference, bool> pred) => m.Body.Instructions.Count(i =>
    (i.OpCode == OpCodes.Call || i.OpCode == OpCodes.Callvirt || i.OpCode == OpCodes.Newobj) && i.Operand is MethodReference mr && pred(mr));

var input = Path.GetFullPath(args[0]);
var output = Path.GetFullPath(args[1]);
var inputSha = Sha(input);
Console.WriteLine($"INPUT_SHA256 {inputSha}");
if (inputSha != ExpectedInputSha) throw new InvalidDataException("input SHA mismatch");

var beforeM = new Dictionary<uint, string>();
var beforeF = new Dictionary<uint, string>();
var preservedBefore = new Dictionary<uint, string>();

using (var module = ModuleDefinition.ReadModule(input, new ReaderParameters { InMemory = true, ReadingMode = ReadingMode.Immediate }))
{
    var types = AllTypes(module.Types).ToList();
    var methods = types.SelectMany(t => t.Methods).ToList();
    var fields = types.SelectMany(t => t.Fields).ToList();
    if (methods.Count != ExpectedMethods || fields.Count != ExpectedFields)
        throw new InvalidDataException($"metadata count drift methods={methods.Count} fields={fields.Count}");
    if (module.AssemblyReferences.Any(a => a.Name == "System.Private.CoreLib"))
        throw new InvalidDataException("unexpected System.Private.CoreLib input ref");
    foreach (var m in methods) beforeM[Raw(m)] = MethodSemantic(m);
    foreach (var f in fields) beforeF[Raw(f)] = FieldSemantic(f);
    foreach (var token in PreservationTokens)
        preservedBefore[token] = beforeM.TryGetValue(token, out var sem) ? sem : throw new InvalidDataException($"preservation token missing 0x{token:X8}");

    var pd = types.Single(t => t.FullName == "Plant_DetaiPage");
    var plantT = types.Single(t => t.FullName == "Plant");
    var skillT = types.Single(t => t.FullName == "Skill");
    var target = pd.Methods.Single(m => m.Name == "LoadSkillLogo" && m.Parameters.Count == 0);
    if (Raw(target) != TargetToken || target.MetadataToken.RID != ExpectedRid || target.Body.CodeSize != ExpectedOldCodeSize || target.Body.Variables.Count != ExpectedOldLocals)
        throw new InvalidDataException($"target fingerprint drift token=0x{Raw(target):X8} rid={target.MetadataToken.RID} size={target.Body.CodeSize} locals={target.Body.Variables.Count}");
    var badStore = target.Body.Instructions.SingleOrDefault(i => i.Offset == 0x5F && i.OpCode.Code.ToString().StartsWith("Stloc") && i.Operand is VariableDefinition v && v.Index == 6 && v.VariableType.FullName == "System.Object");
    if (badStore is null) throw new InvalidDataException("expected IL_005F integer-to-System.Object corruption missing");
    if (!target.Body.Instructions.Any(i => i.Operand is string s && s.Contains("Method not found @1808197F0", StringComparison.Ordinal)))
        throw new InvalidDataException("expected damaged generic-enumerator placeholder missing");

    FieldDefinition PF(string n) => pd.Fields.Single(f => f.Name == n);
    FieldDefinition PlF(string n) => plantT.Fields.Single(f => f.Name == n);
    FieldDefinition SF(string n) => skillT.Fields.Single(f => f.Name == n);
    var plantF = PF("plant");
    var pSkillF = PF("p_skill");
    var skillLogoF = PF("skillLogo");
    var plantIdF = PlF("ID");
    var skillIdF = SF("ID");
    var skillNameF = SF("name");
    if (skillLogoF.FieldType is not GenericInstanceType listImage || listImage.GenericArguments.Count != 1 || listImage.GenericArguments[0].FullName != "UnityEngine.UI.Image")
        throw new InvalidDataException($"skillLogo type drifted: {skillLogoF.FieldType.FullName}");

    var objectImplicit = FindInMethod(target, mr => mr.DeclaringType.FullName == "UnityEngine.Object" && mr.Name == "op_Implicit", "Object.op_Implicit");
    var loadSkillLogo = FindInMethod(target, mr => mr.DeclaringType.FullName == "ResourceManager" && mr.Name == "LoadSkillLogo" && mr.Parameters.Count == 2, "ResourceManager.LoadSkillLogo");
    var setSprite = FindInMethod(target, mr => mr.DeclaringType.FullName == "UnityEngine.UI.Image" && mr.Name == "set_sprite", "Image.set_sprite");

    // Reuse an already-valid List<Image>.get_Item MemberRef from the same module instead of
    // manufacturing a concrete-return generic signature.
    var listGetItem = methods.SelectMany(m => m.HasBody ? m.Body.Instructions : Enumerable.Empty<Instruction>())
        .Select(i => i.Operand).OfType<MethodReference>()
        .FirstOrDefault(mr => mr.Name == "get_Item" && mr.DeclaringType.FullName == listImage.FullName && mr.Parameters.Count == 1 && mr.Parameters[0].ParameterType.FullName == "System.Int32")
        ?? throw new InvalidDataException("valid List<Image>.get_Item MethodRef not found in module");
    var listDecl = module.ImportReference(listImage);
    var listCount = new MethodReference("get_Count", module.TypeSystem.Int32, listDecl)
    {
        HasThis = true,
        ExplicitThis = false,
        CallingConvention = MethodCallingConvention.Default
    };

    Console.WriteLine($"NATIVE_AUTHORITY target_token=0x{TargetToken:X8} rid={ExpectedRid} method_pointer_entry=0x181B86098 va=0x1803A41D0 end=0x1803A444E native_slice_sha256={NativeSliceSha}");
    Console.WriteLine("NATIVE_SEMANTICS unity_plant_truthiness=1 live_id=p_skill.ID+3*plant.ID live_load_sprite=1 live_assign_all_skillLogo=1 dead_assign_all_null=1");
    Console.WriteLine("CLR_LOOP_ADAPTATION pc_native=List<Image>.Enumerator managed=stable_List<Image>_index_loop body_only_Image.set_sprite mutation_of_list=0");

    var body = target.Body;
    body.Instructions.Clear();
    body.Variables.Clear();
    body.ExceptionHandlers.Clear();
    body.InitLocals = true;
    body.MaxStackSize = 4;
    var sprite = new VariableDefinition(module.ImportReference(listImage.GenericArguments[0].Module.GetType("UnityEngine.Sprite") ?? setSprite.Parameters[0].ParameterType));
    // The expression above deliberately resolves through the already-valid Image.set_sprite parameter
    // when UnityEngine.Sprite is external and therefore absent from Assembly-CSharp's Module.GetType table.
    sprite.VariableType = module.ImportReference(setSprite.Parameters[0].ParameterType);
    var index = new VariableDefinition(module.TypeSystem.Int32);
    body.Variables.Add(sprite);
    body.Variables.Add(index);
    var il = body.GetILProcessor();

    var deadPlant = il.Create(OpCodes.Nop);
    var liveCheck = il.Create(OpCodes.Nop);
    var liveBody = il.Create(OpCodes.Nop);
    var deadCheck = il.Create(OpCodes.Nop);
    var deadBody = il.Create(OpCodes.Nop);
    var done = il.Create(OpCodes.Ret);

    // if (!plant) goto dead branch
    il.Append(il.Create(OpCodes.Ldarg_0));
    il.Append(il.Create(OpCodes.Ldfld, plantF));
    il.Append(il.Create(OpCodes.Call, objectImplicit));
    il.Append(il.Create(OpCodes.Brfalse, deadPlant));

    // sprite = ResourceManager.LoadSkillLogo(p_skill.ID + 3 * plant.ID, p_skill.name)
    il.Append(il.Create(OpCodes.Ldarg_0));
    il.Append(il.Create(OpCodes.Ldfld, pSkillF));
    il.Append(il.Create(OpCodes.Ldfld, skillIdF));
    il.Append(il.Create(OpCodes.Ldarg_0));
    il.Append(il.Create(OpCodes.Ldfld, plantF));
    il.Append(il.Create(OpCodes.Ldfld, plantIdF));
    il.Append(il.Create(OpCodes.Ldc_I4_3));
    il.Append(il.Create(OpCodes.Mul));
    il.Append(il.Create(OpCodes.Add));
    il.Append(il.Create(OpCodes.Ldarg_0));
    il.Append(il.Create(OpCodes.Ldfld, pSkillF));
    il.Append(il.Create(OpCodes.Ldfld, skillNameF));
    il.Append(il.Create(OpCodes.Call, loadSkillLogo));
    il.Append(il.Create(OpCodes.Stloc, sprite));

    // for (int i=0; i<skillLogo.Count; i++) skillLogo[i].sprite = sprite;
    il.Append(il.Create(OpCodes.Ldc_I4_0));
    il.Append(il.Create(OpCodes.Stloc, index));
    il.Append(il.Create(OpCodes.Br, liveCheck));
    il.Append(liveBody);
    il.Append(il.Create(OpCodes.Ldarg_0));
    il.Append(il.Create(OpCodes.Ldfld, skillLogoF));
    il.Append(il.Create(OpCodes.Ldloc, index));
    il.Append(il.Create(OpCodes.Callvirt, listGetItem));
    il.Append(il.Create(OpCodes.Ldloc, sprite));
    il.Append(il.Create(OpCodes.Call, setSprite));
    il.Append(il.Create(OpCodes.Ldloc, index));
    il.Append(il.Create(OpCodes.Ldc_I4_1));
    il.Append(il.Create(OpCodes.Add));
    il.Append(il.Create(OpCodes.Stloc, index));
    il.Append(liveCheck);
    il.Append(il.Create(OpCodes.Ldloc, index));
    il.Append(il.Create(OpCodes.Ldarg_0));
    il.Append(il.Create(OpCodes.Ldfld, skillLogoF));
    il.Append(il.Create(OpCodes.Callvirt, listCount));
    il.Append(il.Create(OpCodes.Blt, liveBody));
    il.Append(il.Create(OpCodes.Br, done));

    // dead plant: for (...) skillLogo[i].sprite = null;
    il.Append(deadPlant);
    il.Append(il.Create(OpCodes.Ldc_I4_0));
    il.Append(il.Create(OpCodes.Stloc, index));
    il.Append(il.Create(OpCodes.Br, deadCheck));
    il.Append(deadBody);
    il.Append(il.Create(OpCodes.Ldarg_0));
    il.Append(il.Create(OpCodes.Ldfld, skillLogoF));
    il.Append(il.Create(OpCodes.Ldloc, index));
    il.Append(il.Create(OpCodes.Callvirt, listGetItem));
    il.Append(il.Create(OpCodes.Ldnull));
    il.Append(il.Create(OpCodes.Call, setSprite));
    il.Append(il.Create(OpCodes.Ldloc, index));
    il.Append(il.Create(OpCodes.Ldc_I4_1));
    il.Append(il.Create(OpCodes.Add));
    il.Append(il.Create(OpCodes.Stloc, index));
    il.Append(deadCheck);
    il.Append(il.Create(OpCodes.Ldloc, index));
    il.Append(il.Create(OpCodes.Ldarg_0));
    il.Append(il.Create(OpCodes.Ldfld, skillLogoF));
    il.Append(il.Create(OpCodes.Callvirt, listCount));
    il.Append(il.Create(OpCodes.Blt, deadBody));
    il.Append(done);

    Directory.CreateDirectory(Path.GetDirectoryName(output)!);
    module.Write(output);
}

Console.WriteLine($"OUTPUT_SHA256 {Sha(output)}");

using (var module = ModuleDefinition.ReadModule(output, new ReaderParameters { InMemory = true, ReadingMode = ReadingMode.Immediate }))
{
    var types = AllTypes(module.Types).ToList();
    var methods = types.SelectMany(t => t.Methods).ToList();
    var fields = types.SelectMany(t => t.Fields).ToList();
    if (methods.Count != ExpectedMethods || fields.Count != ExpectedFields)
        throw new InvalidDataException($"reopen metadata drift methods={methods.Count} fields={fields.Count}");
    var target = methods.Single(m => Raw(m) == TargetToken);
    if (target.DeclaringType.FullName != "Plant_DetaiPage" || target.Name != "LoadSkillLogo")
        throw new InvalidDataException("reopen target identity mismatch");
    if (target.Body.Variables.Count != 2 || target.Body.Variables[0].VariableType.FullName != "UnityEngine.Sprite" || target.Body.Variables[1].VariableType.FullName != "System.Int32")
        throw new InvalidDataException("reopen locals mismatch");
    if (target.Body.Variables.Any(v => v.VariableType.FullName == "System.Object" || v.VariableType.FullName.Contains("Enumerator", StringComparison.Ordinal)))
        throw new InvalidDataException("damaged object/enumerator locals survived");
    if (target.Body.Instructions.Any(i => i.Operand is string s && (s.Contains("Method not found", StringComparison.Ordinal) || s.Contains("Warning: Method ends", StringComparison.Ordinal))))
        throw new InvalidDataException("decompiler placeholder survived");
    if (CountCalls(target, mr => mr.DeclaringType.FullName == "ResourceManager" && mr.Name == "LoadSkillLogo") != 1)
        throw new InvalidDataException("ResourceManager.LoadSkillLogo call count mismatch");
    if (CountCalls(target, mr => mr.DeclaringType.FullName == "System.Collections.Generic.List`1<UnityEngine.UI.Image>" && mr.Name == "get_Item") != 2)
        throw new InvalidDataException("List<Image>.get_Item call count mismatch");
    if (CountCalls(target, mr => mr.DeclaringType.FullName == "System.Collections.Generic.List`1<UnityEngine.UI.Image>" && mr.Name == "get_Count") != 2)
        throw new InvalidDataException("List<Image>.get_Count call count mismatch");
    if (CountCalls(target, mr => mr.DeclaringType.FullName == "UnityEngine.UI.Image" && mr.Name == "set_sprite") != 2)
        throw new InvalidDataException("Image.set_sprite call count mismatch");
    Console.WriteLine($"REOPEN_LOAD_SKILL_LOGO_PASS token=0x{TargetToken:X8} code_size={target.Body.CodeSize} locals={target.Body.Variables.Count} object_locals=0 enumerator_locals=0 load_calls=1 get_item_calls=2 count_calls=2 set_sprite_calls=2");

    var changed = new List<uint>();
    foreach (var m in methods)
    {
        var token = Raw(m);
        if (!beforeM.TryGetValue(token, out var old)) throw new InvalidDataException($"unexpected MethodDef 0x{token:X8}");
        if (MethodSemantic(m) != old) changed.Add(token);
    }
    if (changed.Count != 1 || changed[0] != TargetToken)
        throw new InvalidDataException("semantic isolation failed changed=" + string.Join(',', changed.Select(t => $"0x{t:X8}")));
    Console.WriteLine($"SEMANTIC_ISOLATION_PASS untouched_methods={ExpectedMethods - 1} changed_methods=1 target=0x{TargetToken:X8}");

    var changedFields = new List<uint>();
    foreach (var f in fields)
    {
        var token = Raw(f);
        if (!beforeF.TryGetValue(token, out var old) || FieldSemantic(f) != old) changedFields.Add(token);
    }
    if (changedFields.Count != 0)
        throw new InvalidDataException("field metadata drift=" + string.Join(',', changedFields.Select(t => $"0x{t:X8}")));
    Console.WriteLine($"FIELD_METADATA_ISOLATION_PASS unchanged_fields={ExpectedFields} changed_fields=0");

    foreach (var token in PreservationTokens)
    {
        var m = methods.Single(x => Raw(x) == token);
        if (MethodSemantic(m) != preservedBefore[token]) throw new InvalidDataException($"preservation drift 0x{token:X8}");
        Console.WriteLine($"PRESERVE_PASS token=0x{token:X8}");
    }
}

return 0;
