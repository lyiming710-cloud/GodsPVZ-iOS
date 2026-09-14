using Mono.Cecil;
using Mono.Cecil.Cil;
using System.Globalization;
using System.Security.Cryptography;
using System.Text;

if (args.Length != 2)
{
    Console.Error.WriteLine("Usage: Stage9GridCopyPatch <input.dll> <output.dll>");
    return 2;
}

const string ExpectedInputSha = "d65c0dc71b1f47d590e96d617c5d0d3aaa693a478ef433f0d9d572dd933fe132";
const int ExpectedMethods = 2317;
const int ExpectedFields = 2802;
const uint TargetToken = 0x06000294;
const int ExpectedRid = 660;
const int ExpectedOldCodeSize = 120;
const int ExpectedOldLocals = 4;
const string NativeSliceSha = "26d0e35128665a985e660cd922b345fc01fc5857e2e78e9421a167a949d21617";

var PreservationTokens = new HashSet<uint>
{
    0x0600067E, 0x06000285, 0x06000289, 0x0600028A,
    0x060003BA,
    0x060003FD,
    0x060003DD, 0x060003DE,
    0x060001F3, 0x06000216,
    0x06000667,
    0x06000668,
    0x060005E1,
    0x06000157
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
    foreach (var m in methods) beforeM[Raw(m)] = MethodSemantic(m);
    foreach (var fld in fields) beforeF[Raw(fld)] = FieldSemantic(fld);
    foreach (var token in PreservationTokens)
        preservedBefore[token] = beforeM.TryGetValue(token, out var sem) ? sem : throw new InvalidDataException($"preservation token missing 0x{token:X8}");

    var gridT = types.Single(t => t.FullName == "Grid");
    var target = gridT.Methods.Single(m => m.Name == "Copy" && m.Parameters.Count == 0 && m.ReturnType.FullName == "Grid");
    if (Raw(target) != TargetToken || target.MetadataToken.RID != ExpectedRid || target.Body.CodeSize != ExpectedOldCodeSize || target.Body.Variables.Count != ExpectedOldLocals)
        throw new InvalidDataException($"target fingerprint drift token=0x{Raw(target):X8} rid={target.MetadataToken.RID} size={target.Body.CodeSize} locals={target.Body.Variables.Count}");
    if (!target.Body.Instructions.Any(i => i.Offset == 0x64 && i.OpCode == OpCodes.Ceq))
        throw new InvalidDataException("expected IL_0064 ceq corruption missing");

    FieldDefinition GF(string n) => gridT.Fields.Single(f => f.Name == n);
    var gridStateF = GF("gridState");
    var isHomeF = GF("isHome");
    var passablePointF = GF("passablePoint");
    var gridXF = GF("gridX");
    var gridYF = GF("gridY");
    var ctor = gridT.Methods.Single(m => m.IsConstructor && !m.IsStatic && m.Parameters.Count == 2 && m.Parameters.All(p => p.ParameterType.FullName == "System.Int32"));
    var nreT = module.ImportReference(typeof(NullReferenceException));
    var nreCtor = new MethodReference(".ctor", module.TypeSystem.Void, nreT) { HasThis = true };

    Console.WriteLine($"NATIVE_AUTHORITY target_token=0x{TargetToken:X8} rid={ExpectedRid} method_pointer_entry=0x181B841F8 va=0x18032A010 end=0x18032A09E native_slice_sha256={NativeSliceSha}");
    Console.WriteLine("NATIVE_SEMANTICS new_Grid_gridX_gridY=1 copy_gridState=1 copy_isHome=1 copy_passablePoint=1 allocation_null_failure=1");

    var body = target.Body;
    body.Instructions.Clear();
    body.Variables.Clear();
    body.ExceptionHandlers.Clear();
    body.InitLocals = true;
    body.MaxStackSize = 3;
    var result = new VariableDefinition(gridT);
    body.Variables.Add(result);
    var il = body.GetILProcessor();
    var live = il.Create(OpCodes.Nop);

    il.Append(il.Create(OpCodes.Ldarg_0));
    il.Append(il.Create(OpCodes.Ldfld, gridXF));
    il.Append(il.Create(OpCodes.Ldarg_0));
    il.Append(il.Create(OpCodes.Ldfld, gridYF));
    il.Append(il.Create(OpCodes.Newobj, ctor));
    il.Append(il.Create(OpCodes.Stloc, result));
    il.Append(il.Create(OpCodes.Ldloc, result));
    il.Append(il.Create(OpCodes.Brtrue_S, live));
    il.Append(il.Create(OpCodes.Newobj, nreCtor));
    il.Append(il.Create(OpCodes.Throw));
    il.Append(live);
    il.Append(il.Create(OpCodes.Ldloc, result));
    il.Append(il.Create(OpCodes.Ldarg_0));
    il.Append(il.Create(OpCodes.Ldfld, gridStateF));
    il.Append(il.Create(OpCodes.Stfld, gridStateF));
    il.Append(il.Create(OpCodes.Ldloc, result));
    il.Append(il.Create(OpCodes.Ldarg_0));
    il.Append(il.Create(OpCodes.Ldfld, isHomeF));
    il.Append(il.Create(OpCodes.Stfld, isHomeF));
    il.Append(il.Create(OpCodes.Ldloc, result));
    il.Append(il.Create(OpCodes.Ldarg_0));
    il.Append(il.Create(OpCodes.Ldfld, passablePointF));
    il.Append(il.Create(OpCodes.Stfld, passablePointF));
    il.Append(il.Create(OpCodes.Ldloc, result));
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
    int CountField(string n, OpCode op) => target.Body.Instructions.Count(i => i.OpCode == op && i.Operand is FieldReference fr && fr.DeclaringType.FullName == "Grid" && fr.Name == n);
    var ceq = target.Body.Instructions.Count(i => i.OpCode == OpCodes.Ceq);
    var ctorCalls = target.Body.Instructions.Count(i => i.OpCode == OpCodes.Newobj && i.Operand is MethodReference mr && mr.DeclaringType.FullName == "Grid" && mr.Parameters.Count == 2);
    var throwCount = target.Body.Instructions.Count(i => i.OpCode == OpCodes.Throw);
    var nreCount = target.Body.Instructions.Count(i => i.OpCode == OpCodes.Newobj && i.Operand is MethodReference mr && mr.DeclaringType.FullName == "System.NullReferenceException");
    if (ceq != 0 || ctorCalls != 1 || throwCount != 1 || nreCount != 1 || target.Body.Variables.Count != 1 ||
        CountField("gridX", OpCodes.Ldfld) != 1 || CountField("gridY", OpCodes.Ldfld) != 1 ||
        CountField("gridState", OpCodes.Ldfld) != 1 || CountField("gridState", OpCodes.Stfld) != 1 ||
        CountField("isHome", OpCodes.Ldfld) != 1 || CountField("isHome", OpCodes.Stfld) != 1 ||
        CountField("passablePoint", OpCodes.Ldfld) != 1 || CountField("passablePoint", OpCodes.Stfld) != 1)
        throw new InvalidDataException("reopen Grid.Copy mismatch");
    Console.WriteLine($"REOPEN_GRID_COPY_PASS token=0x{TargetToken:X8} code_size={target.Body.CodeSize} locals={target.Body.Variables.Count} ceq={ceq} Grid_ctor_calls={ctorCalls} null_failure_throw={throwCount} gridState_copy=1 isHome_copy=1 passablePoint_copy=1");

    var changedMethods = methods.Where(m => beforeM[Raw(m)] != MethodSemantic(m)).Select(Raw).ToList();
    if (changedMethods.Count != 1 || changedMethods[0] != TargetToken)
        throw new InvalidDataException("method semantic isolation failed: " + string.Join(',', changedMethods.Select(x => $"0x{x:X8}")));
    Console.WriteLine($"SEMANTIC_ISOLATION_PASS untouched_methods={methods.Count - 1} changed_methods=1 target=0x{TargetToken:X8}");

    var changedFields = fields.Where(f => beforeF[Raw(f)] != FieldSemantic(f)).Select(Raw).ToList();
    if (changedFields.Count != 0) throw new InvalidDataException("field metadata isolation failed");
    Console.WriteLine($"FIELD_METADATA_ISOLATION_PASS unchanged_fields={fields.Count} changed_fields=0");

    foreach (var token in PreservationTokens)
    {
        var m = methods.Single(x => Raw(x) == token);
        if (MethodSemantic(m) != preservedBefore[token]) throw new InvalidDataException($"preservation drift 0x{token:X8}");
    }
    Console.WriteLine("PRESERVATION_PASS batch1=4 plant_ctor=1 projectile_ctor=1 projectile_hf21=2 managers=2 plant_detail_initialize=1 load_skill_logo=1 load_data=1 create_map=1");
    Console.WriteLine("PATCH_GRID_COPY method_body_changes=1 pc_native_semantics=1 object_integer_ceq_removed=1 metadata_visibility_changes=0");
}
return 0;
