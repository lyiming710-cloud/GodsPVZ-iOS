using Mono.Cecil;
using Mono.Cecil.Cil;
using System.Globalization;
using System.Security.Cryptography;
using System.Text;

if (args.Length != 2)
{
    Console.Error.WriteLine("Usage: Stage9ProjectileCtorPatch <input.dll> <output.dll>");
    return 2;
}

const string ExpectedInputSha256 = "dc39b8bcbfcd61f04acf2c18c42e24583e9a6e75248bb24b6aae9e381b19f173";
const int ExpectedMethodCount = 2317;
const int ExpectedFieldCount = 2802;
const uint TargetToken = 0x060003FD;
const int ExpectedRid = 1021;
const int ExpectedOldCodeSize = 80;
const int ExpectedOldLocals = 3;
const string NativeSliceSha256 = "e843a1237bfb1e0355a1efe9ce989a2a8b02dbc19c0eedf09fb9db0f00e5b7ff";
var PreservationTokens = new HashSet<uint>
{
    0x0600067E, 0x06000285, 0x06000289, 0x0600028A, // Batch1
    0x060003BA,                                     // Plant::.ctor
    0x060003DD, 0x060003DE,                        // Projectile ResetData / BindTrack
    0x060001F6, 0x06000216                         // ProjectileManager.Start / ResourceManager.Start
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
static string Sha256(string p) => Convert.ToHexString(SHA256.HashData(File.ReadAllBytes(p))).ToLowerInvariant();
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
var inputSha = Sha256(input);
Console.WriteLine($"INPUT_SHA256 {inputSha}");
if (inputSha != ExpectedInputSha256) throw new InvalidDataException("input SHA mismatch");

var beforeM = new Dictionary<uint, string>();
var beforeF = new Dictionary<uint, string>();

using (var module = ModuleDefinition.ReadModule(input, new ReaderParameters { InMemory = true, ReadingMode = ReadingMode.Immediate }))
{
    var types = AllTypes(module.Types).ToList();
    var methods = types.SelectMany(t => t.Methods).ToList();
    var fields = types.SelectMany(t => t.Fields).ToList();
    if (methods.Count != ExpectedMethodCount || fields.Count != ExpectedFieldCount)
        throw new InvalidDataException($"metadata count drift methods={methods.Count} fields={fields.Count}");
    if (module.AssemblyReferences.Any(a => a.Name == "System.Private.CoreLib"))
        throw new InvalidDataException("unexpected System.Private.CoreLib input ref");
    foreach (var m in methods) beforeM[Raw(m)] = MethodSemantic(m);
    foreach (var f in fields) beforeF[Raw(f)] = FieldSemantic(f);

    var projectile = types.Single(t => t.FullName == "Projectile");
    var target = projectile.Methods.Single(m => m.IsConstructor && !m.IsStatic && m.Parameters.Count == 0);
    if (Raw(target) != TargetToken || target.MetadataToken.RID != ExpectedRid || target.Body.CodeSize != ExpectedOldCodeSize || target.Body.Variables.Count != ExpectedOldLocals)
        throw new InvalidDataException($"target fingerprint mismatch token=0x{Raw(target):X8} rid={target.MetadataToken.RID} size={target.Body.CodeSize} locals={target.Body.Variables.Count}");

    var badStloc = target.Body.Instructions.SingleOrDefault(i => i.Offset == 0x25 && i.OpCode.Code == Code.Stloc);
    if (badStloc?.Operand is not VariableDefinition badLocal || badLocal.VariableType.FullName != "System.IntPtr")
        throw new InvalidDataException("old IL_0025 stloc IntPtr fingerprint mismatch");
    var oldZero = target.Body.Instructions.SingleOrDefault(i => i.Offset == 0x36 && i.OpCode == OpCodes.Ldsfld && i.Operand is FieldReference fr && fr.DeclaringType.FullName == "UnityEngine.Vector3" && fr.Name == "zeroVector");
    if (oldZero is null) throw new InvalidDataException("old Vector3.zeroVector fingerprint mismatch");

    FieldDefinition F(string name) => projectile.Fields.Single(f => f.Name == name);
    var scale = F("scale");
    var updateRate = F("updateRate");
    var previousPosition = F("previousPosition");

    var monoCtor = target.Body.Instructions
        .Where(i => i.OpCode == OpCodes.Call && i.Operand is MethodReference)
        .Select(i => (MethodReference)i.Operand)
        .Single(m => m.Name == ".ctor" && m.DeclaringType.FullName == "UnityEngine.MonoBehaviour" && m.Parameters.Count == 0);

    var vector3Zero = methods
        .Where(m => m.HasBody)
        .SelectMany(m => m.Body.Instructions)
        .Where(i => i.OpCode == OpCodes.Call && i.Operand is MethodReference)
        .Select(i => (MethodReference)i.Operand)
        .FirstOrDefault(m => m.DeclaringType.FullName == "UnityEngine.Vector3" && m.Name == "get_zero" && m.Parameters.Count == 0 && m.ReturnType.FullName == "UnityEngine.Vector3")
        ?? throw new InvalidDataException("Vector3.get_zero MemberRef not found in input module");

    var body = target.Body;
    body.Instructions.Clear();
    body.ExceptionHandlers.Clear();
    body.Variables.Clear();
    body.InitLocals = false;
    body.MaxStackSize = 2;
    var il = body.GetILProcessor();

    void StoreR4(FieldDefinition f, float v)
    {
        il.Append(il.Create(OpCodes.Ldarg_0));
        il.Append(il.Create(OpCodes.Ldc_R4, v));
        il.Append(il.Create(OpCodes.Stfld, f));
    }

    StoreR4(scale, 1f);
    StoreR4(updateRate, 1f);
    il.Append(il.Create(OpCodes.Ldarg_0));
    il.Append(il.Create(OpCodes.Call, vector3Zero));
    il.Append(il.Create(OpCodes.Stfld, previousPosition));
    il.Append(il.Create(OpCodes.Ldarg_0));
    il.Append(il.Create(OpCodes.Call, monoCtor));
    il.Append(il.Create(OpCodes.Ret));

    Console.WriteLine($"NATIVE_AUTHORITY target_token=0x{TargetToken:X8} rid={ExpectedRid} method_pointer_entry=0x181B84D40 va=0x18037DF20 end=0x18037DF7F native_slice_sha256={NativeSliceSha256}");
    Console.WriteLine("NATIVE_SEMANTICS scale=1 updateRate=1 previousPosition=Vector3.zero tailcall=UnityEngine.MonoBehaviour::.ctor");
    Console.WriteLine("PATCH_PROJECTILE_CTOR method_body_changes=1 zeroVector_private_ref_removed=1 lowering=Vector3.get_zero gameplay_semantics_changed=0");
    module.Write(output);
}

Console.WriteLine($"OUTPUT_SHA256 {Sha256(output)}");

using (var module = ModuleDefinition.ReadModule(output, new ReaderParameters { InMemory = true, ReadingMode = ReadingMode.Immediate }))
{
    var types = AllTypes(module.Types).ToList();
    var methods = types.SelectMany(t => t.Methods).ToList();
    var fields = types.SelectMany(t => t.Fields).ToList();
    if (methods.Count != ExpectedMethodCount || fields.Count != ExpectedFieldCount)
        throw new InvalidDataException("output metadata count drift");
    if (module.AssemblyReferences.Any(a => a.Name == "System.Private.CoreLib"))
        throw new InvalidDataException("unexpected System.Private.CoreLib output ref");

    var target = methods.Single(m => Raw(m) == TargetToken);
    var projectile = target.DeclaringType;
    FieldDefinition F(string name) => projectile.Fields.Single(f => f.Name == name);
    int Stores(FieldDefinition f) => target.Body.Instructions.Count(i => i.OpCode == OpCodes.Stfld && i.Operand is FieldReference fr && fr.FullName == f.FullName);

    if (target.Body.Variables.Count != 0 || target.Body.ExceptionHandlers.Count != 0 || target.Body.InitLocals)
        throw new InvalidDataException("recovered ctor local/EH shape mismatch");
    if (target.Body.Instructions.Any(i => i.Operand is FieldReference fr && fr.DeclaringType.FullName == "UnityEngine.Vector3" && fr.Name == "zeroVector"))
        throw new InvalidDataException("private Vector3.zeroVector ref remains");
    if (Stores(F("scale")) != 1 || Stores(F("updateRate")) != 1 || Stores(F("previousPosition")) != 1)
        throw new InvalidDataException("target field store mismatch");
    if (target.Body.Instructions.Count(i => i.OpCode == OpCodes.Ldc_R4 && i.Operand is float f && BitConverter.SingleToInt32Bits(f) == BitConverter.SingleToInt32Bits(1f)) != 2)
        throw new InvalidDataException("1.0f initialization mismatch");
    if (target.Body.Instructions.Count(i => i.OpCode == OpCodes.Call && i.Operand is MethodReference mr && mr.DeclaringType.FullName == "UnityEngine.Vector3" && mr.Name == "get_zero") != 1)
        throw new InvalidDataException("Vector3.get_zero call mismatch");
    if (target.Body.Instructions.Count(i => i.OpCode == OpCodes.Call && i.Operand is MethodReference mr && mr.DeclaringType.FullName == "UnityEngine.MonoBehaviour" && mr.Name == ".ctor") != 1)
        throw new InvalidDataException("MonoBehaviour ctor call mismatch");

    var afterM = methods.ToDictionary(Raw, MethodSemantic);
    var changed = beforeM.Keys.Where(k => beforeM[k] != afterM[k]).OrderBy(x => x).ToList();
    if (changed.Count != 1 || changed[0] != TargetToken)
        throw new InvalidDataException("semantic isolation failure: " + string.Join(',', changed.Select(x => $"0x{x:X8}")));
    var afterF = fields.ToDictionary(Raw, FieldSemantic);
    var changedFields = beforeF.Keys.Where(k => beforeF[k] != afterF[k]).ToList();
    if (changedFields.Count != 0)
        throw new InvalidDataException("field metadata drift: " + string.Join(',', changedFields.Select(x => $"0x{x:X8}")));
    foreach (var tok in PreservationTokens)
        if (!beforeM.TryGetValue(tok, out var before) || !afterM.TryGetValue(tok, out var after) || before != after)
            throw new InvalidDataException($"preservation failure token=0x{tok:X8}");

    Console.WriteLine($"REOPEN_PROJECTILE_CTOR_PASS token=0x{TargetToken:X8} code_size={target.Body.CodeSize} locals={target.Body.Variables.Count} zeroVector_private_refs=0 get_zero_calls=1");
    Console.WriteLine($"SEMANTIC_ISOLATION_PASS untouched_methods={methods.Count - 1} changed_methods=1 target_token=0x{TargetToken:X8}");
    Console.WriteLine($"FIELD_METADATA_ISOLATION_PASS unchanged_fields={fields.Count} changed_fields=0");
    Console.WriteLine("PRESERVATION_PASS batch1=4 plant_ctor=1 projectile_hf21=2 managers=2");
}

return 0;
