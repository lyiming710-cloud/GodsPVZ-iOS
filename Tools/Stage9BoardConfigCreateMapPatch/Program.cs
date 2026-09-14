using Mono.Cecil;
using Mono.Cecil.Cil;
using System.Globalization;
using System.Security.Cryptography;
using System.Text;

if (args.Length != 2)
{
    Console.Error.WriteLine("Usage: Stage9BoardConfigCreateMapPatch <input.dll> <output.dll>");
    return 2;
}

const string ExpectedInputSha = "56958c11480ad840904c07e2194c7f36304287c6c7a170be964d581a99d16751";
const int ExpectedMethods = 2317;
const int ExpectedFields = 2802;
const uint TargetToken = 0x06000157;
const int ExpectedRid = 343;
const int ExpectedOldCodeSize = 53;
const int ExpectedOldLocals = 3;
const string NativeLeafSha = "c45928899d11a9a908c583d4190b11c29d70a4fccf8b7c2eb8cadb93da620549";

var PreservationTokens = new HashSet<uint>
{
    0x0600067E, 0x06000285, 0x06000289, 0x0600028A, // gameplay Batch1
    0x060003BA,                                     // Plant::.ctor
    0x060003FD,                                     // Projectile::.ctor
    0x060003DD, 0x060003DE,                        // Projectile.ResetData / BindTrack
    0x060001F3, 0x06000216,                        // ProjectileManager.Start / ResourceManager.Start
    0x06000667,                                     // Plant_DetaiPage.Initialize
    0x06000668,                                     // Plant_DetaiPage.LoadSkillLogo
    0x060005E1                                      // Board_PlantDetail_PlantData.LoadData
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
    foreach (var fld in fields) beforeF[Raw(fld)] = FieldSemantic(fld);
    foreach (var token in PreservationTokens)
        preservedBefore[token] = beforeM.TryGetValue(token, out var sem) ? sem : throw new InvalidDataException($"preservation token missing 0x{token:X8}");

    var boardConfigT = types.Single(t => t.FullName == "BoardConfig");
    var mapT = types.Single(t => t.FullName == "Map");
    var target = boardConfigT.Methods.Single(m => m.Name == "CreateMap" && m.Parameters.Count == 0);
    if (Raw(target) != TargetToken || target.MetadataToken.RID != ExpectedRid || target.Body.CodeSize != ExpectedOldCodeSize || target.Body.Variables.Count != ExpectedOldLocals)
        throw new InvalidDataException($"target fingerprint drift token=0x{Raw(target):X8} rid={target.MetadataToken.RID} size={target.Body.CodeSize} locals={target.Body.Variables.Count}");
    if (!target.Body.Instructions.Any(i => i.Offset == 0x0B && i.OpCode == OpCodes.Ceq))
        throw new InvalidDataException("expected IL_000B ceq corruption missing");

    var mapF = boardConfigT.Fields.Single(f => f.Name == "map" && f.FieldType.FullName == "Map");
    var copyRef = target.Body.Instructions
        .Where(i => (i.OpCode == OpCodes.Call || i.OpCode == OpCodes.Callvirt) && i.Operand is MethodReference)
        .Select(i => (MethodReference)i.Operand)
        .Single(mr => mr.Name == "Copy" && mr.DeclaringType.FullName == "Map" && mr.Parameters.Count == 0 && mr.ReturnType.FullName == "Map");

    Console.WriteLine($"NATIVE_AUTHORITY target_token=0x{TargetToken:X8} rid={ExpectedRid} method_pointer_entry=0x181B83810 va=0x18030EBB0 leaf_bytes=21 native_leaf_sha256={NativeLeafSha}");
    Console.WriteLine("NATIVE_SEMANTICS map_null_returns_null=1 map_live_tailcalls_Map.Copy=1 boardconfig_map_offset=0x30");

    var body = target.Body;
    body.Instructions.Clear();
    body.Variables.Clear();
    body.ExceptionHandlers.Clear();
    body.InitLocals = false;
    body.MaxStackSize = 1;
    var il = body.GetILProcessor();
    var nullCase = il.Create(OpCodes.Ldnull);

    il.Append(il.Create(OpCodes.Ldarg_0));
    il.Append(il.Create(OpCodes.Ldfld, mapF));
    il.Append(il.Create(OpCodes.Brfalse_S, nullCase));
    il.Append(il.Create(OpCodes.Ldarg_0));
    il.Append(il.Create(OpCodes.Ldfld, mapF));
    il.Append(il.Create(OpCodes.Call, copyRef));
    il.Append(il.Create(OpCodes.Ret));
    il.Append(nullCase);
    il.Append(il.Create(OpCodes.Ret));

    Directory.CreateDirectory(Path.GetDirectoryName(output)!);
    module.Write(output);
}

var outputSha = Sha(output);
Console.WriteLine($"OUTPUT_SHA256 {outputSha}");

using (var module = ModuleDefinition.ReadModule(output, new ReaderParameters { InMemory = true, ReadingMode = ReadingMode.Immediate }))
{
    var types = AllTypes(module.Types).ToList();
    var methods = types.SelectMany(t => t.Methods).ToList();
    var fields = types.SelectMany(t => t.Fields).ToList();
    var target = methods.Single(m => Raw(m) == TargetToken);
    var ceq = target.Body.Instructions.Count(i => i.OpCode == OpCodes.Ceq);
    var mapRefs = target.Body.Instructions.Count(i => i.Operand is FieldReference fr && fr.DeclaringType.FullName == "BoardConfig" && fr.Name == "map");
    var copyCalls = target.Body.Instructions.Count(i => (i.OpCode == OpCodes.Call || i.OpCode == OpCodes.Callvirt) && i.Operand is MethodReference mr && mr.DeclaringType.FullName == "Map" && mr.Name == "Copy");
    var ldcZero = target.Body.Instructions.Count(i => i.OpCode == OpCodes.Ldc_I4_0 || (i.OpCode == OpCodes.Ldc_I4 && Equals(i.Operand, 0)));
    var ldnull = target.Body.Instructions.Count(i => i.OpCode == OpCodes.Ldnull);
    if (ceq != 0 || ldcZero != 0 || mapRefs != 2 || copyCalls != 1 || ldnull != 1 || target.Body.Variables.Count != 0)
        throw new InvalidDataException($"reopen target mismatch ceq={ceq} ldc0={ldcZero} mapRefs={mapRefs} copyCalls={copyCalls} ldnull={ldnull} locals={target.Body.Variables.Count}");
    Console.WriteLine($"REOPEN_CREATE_MAP_PASS token=0x{TargetToken:X8} code_size={target.Body.CodeSize} locals={target.Body.Variables.Count} ceq={ceq} ldc_i4_0={ldcZero} map_refs={mapRefs} Map_Copy_calls={copyCalls} ldnull={ldnull}");

    var changedMethods = new List<uint>();
    foreach (var m in methods)
    {
        var token = Raw(m);
        if (!beforeM.TryGetValue(token, out var before)) throw new InvalidDataException($"unexpected method 0x{token:X8}");
        if (before != MethodSemantic(m)) changedMethods.Add(token);
    }
    if (changedMethods.Count != 1 || changedMethods[0] != TargetToken)
        throw new InvalidDataException("method semantic isolation failed: " + string.Join(',', changedMethods.Select(x => $"0x{x:X8}")));
    Console.WriteLine($"SEMANTIC_ISOLATION_PASS untouched_methods={methods.Count - 1} changed_methods=1 target=0x{TargetToken:X8}");

    var changedFields = new List<uint>();
    foreach (var fld in fields)
    {
        var token = Raw(fld);
        if (!beforeF.TryGetValue(token, out var before)) throw new InvalidDataException($"unexpected field 0x{token:X8}");
        if (before != FieldSemantic(fld)) changedFields.Add(token);
    }
    if (changedFields.Count != 0) throw new InvalidDataException("field metadata isolation failed: " + string.Join(',', changedFields.Select(x => $"0x{x:X8}")));
    Console.WriteLine($"FIELD_METADATA_ISOLATION_PASS unchanged_fields={fields.Count} changed_fields=0");

    foreach (var token in PreservationTokens)
    {
        var m = methods.Single(x => Raw(x) == token);
        if (MethodSemantic(m) != preservedBefore[token]) throw new InvalidDataException($"preservation drift 0x{token:X8}");
    }
    Console.WriteLine("PRESERVATION_PASS batch1=4 plant_ctor=1 projectile_ctor=1 projectile_hf21=2 managers=2 plant_detail_initialize=1 load_skill_logo=1 load_data=1");
    Console.WriteLine("PATCH_CREATE_MAP method_body_changes=1 native_leaf_semantics=1 object_integer_ceq_removed=1 metadata_visibility_changes=0");
}

return 0;
