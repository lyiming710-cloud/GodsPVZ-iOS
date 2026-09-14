using Mono.Cecil;
using Mono.Cecil.Cil;
using System.Globalization;
using System.Security.Cryptography;
using System.Text;

if (args.Length != 2)
{
    Console.Error.WriteLine("Usage: Stage9GridCopyPatchV2 <input.dll> <output.dll>");
    return 2;
}

const string ExpectedInputSha = "d65c0dc71b1f47d590e96d617c5d0d3aaa693a478ef433f0d9d572dd933fe132";
const uint TargetToken = 0x06000294;
const int ExpectedRid = 660;
const int ExpectedMethods = 2317;
const int ExpectedFields = 2802;
const int ExpectedOldCodeSize = 120;
const int ExpectedOldLocals = 4;
const string NativeSliceSha = "26d0e35128665a985e660cd922b345fc01fc5857e2e78e9421a167a949d21617";

var preservation = new HashSet<uint>
{
    0x0600067E, 0x06000285, 0x06000289, 0x0600028A,
    0x060003BA, 0x060003FD, 0x060003DD, 0x060003DE,
    0x060001F3, 0x06000216, 0x06000667, 0x06000668,
    0x060005E1, 0x06000157
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
static string Scope(IMetadataScope? s) => s switch
{
    AssemblyNameReference a => a.Name,
    ModuleDefinition m => m.Assembly?.Name?.Name ?? m.Name,
    ModuleReference mr => mr.Name,
    _ => s?.ToString() ?? "<null>"
};
static string TypeSig(TypeReference t) => $"{t.FullName}@{Scope(t.Scope)}";
static string OpSig(object? o, MethodDefinition owner)
{
    if (o is null) return "";
    if (o is Instruction i) return $"I#{owner.Body.Instructions.IndexOf(i)}";
    if (o is Instruction[] sw) return "SW[" + string.Join(',', sw.Select(i => owner.Body.Instructions.IndexOf(i))) + "]";
    if (o is MethodReference mr) return $"M:{mr.FullName}@{Scope(mr.DeclaringType.Scope)}";
    if (o is FieldReference fr) return $"F:{fr.FullName}@{Scope(fr.DeclaringType.Scope)}";
    if (o is TypeReference tr) return $"T:{TypeSig(tr)}";
    if (o is VariableDefinition v) return $"V:{v.Index}:{TypeSig(v.VariableType)}";
    if (o is ParameterDefinition p) return $"P:{p.Index}:{TypeSig(p.ParameterType)}";
    if (o is string s) return "S:" + Convert.ToBase64String(Encoding.UTF8.GetBytes(s));
    if (o is float f) return $"R4:{BitConverter.SingleToInt32Bits(f):X8}";
    if (o is double d) return $"R8:{BitConverter.DoubleToInt64Bits(d):X16}";
    return "C:" + Convert.ToString(o, CultureInfo.InvariantCulture);
}
static string MethodSig(MethodDefinition m)
{
    var sb = new StringBuilder();
    sb.Append(m.FullName).Append('|').Append(m.Attributes).Append('|').Append(m.ImplAttributes).Append('|');
    if (!m.HasBody) return sb.Append("NOBODY").ToString();
    sb.Append("init=").Append(m.Body.InitLocals).Append(";max=").Append(m.Body.MaxStackSize).Append(';');
    foreach (var v in m.Body.Variables) sb.Append("L:").Append(v.Index).Append(':').Append(TypeSig(v.VariableType)).Append(';');
    foreach (var i in m.Body.Instructions) sb.Append("I:").Append(i.OpCode.Code).Append(':').Append(OpSig(i.Operand, m)).Append(';');
    foreach (var h in m.Body.ExceptionHandlers)
    {
        int Idx(Instruction? x) => x is null ? -1 : m.Body.Instructions.IndexOf(x);
        sb.Append("EH:").Append(h.HandlerType).Append(':').Append(Idx(h.TryStart)).Append(':').Append(Idx(h.TryEnd)).Append(':')
          .Append(Idx(h.HandlerStart)).Append(':').Append(Idx(h.HandlerEnd)).Append(':').Append(Idx(h.FilterStart)).Append(':')
          .Append(h.CatchType is null ? "" : TypeSig(h.CatchType)).Append(';');
    }
    return sb.ToString();
}
static string FieldSig(FieldDefinition f)
{
    var attrs = string.Join(',', f.CustomAttributes.Select(a => a.AttributeType.FullName).OrderBy(x => x, StringComparer.Ordinal));
    var c = f.HasConstant ? Convert.ToString(f.Constant, CultureInfo.InvariantCulture) ?? "<null>" : "<none>";
    return $"{f.FullName}|{f.Attributes}|{TypeSig(f.FieldType)}|const={c}|ca={attrs}";
}
static int CoreLibRefs(ModuleDefinition m) => m.AssemblyReferences.Count(a => a.Name == "System.Private.CoreLib");

var input = Path.GetFullPath(args[0]);
var output = Path.GetFullPath(args[1]);
var inputSha = Sha(input);
Console.WriteLine($"INPUT_SHA256 {inputSha}");
if (inputSha != ExpectedInputSha) throw new InvalidDataException("input SHA mismatch");

var beforeM = new Dictionary<uint,string>();
var beforeF = new Dictionary<uint,string>();
var preserveBefore = new Dictionary<uint,string>();

using (var module = ModuleDefinition.ReadModule(input, new ReaderParameters { InMemory=true, ReadingMode=ReadingMode.Immediate }))
{
    if (CoreLibRefs(module) != 0) throw new InvalidDataException("input already references System.Private.CoreLib");
    var types = AllTypes(module.Types).ToList();
    var methods = types.SelectMany(t => t.Methods).ToList();
    var fields = types.SelectMany(t => t.Fields).ToList();
    if (methods.Count != ExpectedMethods || fields.Count != ExpectedFields) throw new InvalidDataException("metadata count drift");
    foreach (var m in methods) beforeM[Raw(m)] = MethodSig(m);
    foreach (var f in fields) beforeF[Raw(f)] = FieldSig(f);
    foreach (var tok in preservation) preserveBefore[tok] = beforeM[tok];

    var grid = types.Single(t => t.FullName == "Grid");
    var target = grid.Methods.Single(m => m.Name == "Copy" && m.Parameters.Count == 0 && m.ReturnType.FullName == "Grid");
    if (Raw(target) != TargetToken || target.MetadataToken.RID != ExpectedRid || target.Body.CodeSize != ExpectedOldCodeSize || target.Body.Variables.Count != ExpectedOldLocals)
        throw new InvalidDataException("Grid.Copy fingerprint drift");
    if (!target.Body.Instructions.Any(i => i.Offset == 0x64 && i.OpCode == OpCodes.Ceq)) throw new InvalidDataException("expected IL_0064 ceq missing");

    FieldDefinition F(string n) => grid.Fields.Single(f => f.Name == n);
    var gridX=F("gridX"); var gridY=F("gridY"); var gridState=F("gridState"); var isHome=F("isHome"); var passablePoint=F("passablePoint");
    var ctor = grid.Methods.Single(m => m.IsConstructor && !m.IsStatic && m.Parameters.Count == 2 && m.Parameters.All(p => p.ParameterType.FullName == "System.Int32"));

    Console.WriteLine($"NATIVE_AUTHORITY target_token=0x{TargetToken:X8} rid={ExpectedRid} method_pointer_entry=0x181B841F8 va=0x18032A010 end=0x18032A09E native_slice_sha256={NativeSliceSha}");
    Console.WriteLine("NATIVE_SEMANTICS new_Grid_gridX_gridY=1 copy_gridState=1 copy_isHome=1 copy_passablePoint=1 allocation_null_failure=1");
    Console.WriteLine("CLR_ALLOCATION_ADAPTATION explicit_null_branch=0 managed_newobj_failure_semantics=1 system_private_corelib_refs=0");

    var body=target.Body;
    body.Instructions.Clear(); body.Variables.Clear(); body.ExceptionHandlers.Clear(); body.InitLocals=true; body.MaxStackSize=3;
    var result=new VariableDefinition(grid); body.Variables.Add(result);
    var il=body.GetILProcessor();
    il.Append(il.Create(OpCodes.Ldarg_0)); il.Append(il.Create(OpCodes.Ldfld,gridX));
    il.Append(il.Create(OpCodes.Ldarg_0)); il.Append(il.Create(OpCodes.Ldfld,gridY));
    il.Append(il.Create(OpCodes.Newobj,ctor)); il.Append(il.Create(OpCodes.Stloc,result));
    foreach (var f in new[]{gridState,isHome,passablePoint})
    {
        il.Append(il.Create(OpCodes.Ldloc,result)); il.Append(il.Create(OpCodes.Ldarg_0)); il.Append(il.Create(OpCodes.Ldfld,f)); il.Append(il.Create(OpCodes.Stfld,f));
    }
    il.Append(il.Create(OpCodes.Ldloc,result)); il.Append(il.Create(OpCodes.Ret));

    Directory.CreateDirectory(Path.GetDirectoryName(output)!);
    module.Write(output);
}

Console.WriteLine($"OUTPUT_SHA256 {Sha(output)}");
using (var module = ModuleDefinition.ReadModule(output, new ReaderParameters { InMemory=true, ReadingMode=ReadingMode.Immediate }))
{
    if (CoreLibRefs(module) != 0) throw new InvalidDataException("System.Private.CoreLib reference pollution");
    var types=AllTypes(module.Types).ToList(); var methods=types.SelectMany(t=>t.Methods).ToList(); var fields=types.SelectMany(t=>t.Fields).ToList();
    var target=methods.Single(m=>Raw(m)==TargetToken);
    int FC(string n, OpCode op)=>target.Body.Instructions.Count(i=>i.OpCode==op && i.Operand is FieldReference fr && fr.DeclaringType.FullName=="Grid" && fr.Name==n);
    var ceq=target.Body.Instructions.Count(i=>i.OpCode==OpCodes.Ceq);
    var ctorCalls=target.Body.Instructions.Count(i=>i.OpCode==OpCodes.Newobj && i.Operand is MethodReference mr && mr.DeclaringType.FullName=="Grid" && mr.Parameters.Count==2);
    var throws=target.Body.Instructions.Count(i=>i.OpCode==OpCodes.Throw);
    var nreRefs=target.Body.Instructions.Count(i=>i.Operand is MethodReference mr && mr.DeclaringType.FullName=="System.NullReferenceException");
    if (ceq!=0 || ctorCalls!=1 || throws!=0 || nreRefs!=0 || target.Body.Variables.Count!=1 ||
        FC("gridX",OpCodes.Ldfld)!=1 || FC("gridY",OpCodes.Ldfld)!=1 ||
        FC("gridState",OpCodes.Ldfld)!=1 || FC("gridState",OpCodes.Stfld)!=1 ||
        FC("isHome",OpCodes.Ldfld)!=1 || FC("isHome",OpCodes.Stfld)!=1 ||
        FC("passablePoint",OpCodes.Ldfld)!=1 || FC("passablePoint",OpCodes.Stfld)!=1)
        throw new InvalidDataException("reopen Grid.Copy V2 mismatch");
    Console.WriteLine($"REOPEN_GRID_COPY_V2_PASS token=0x{TargetToken:X8} code_size={target.Body.CodeSize} locals={target.Body.Variables.Count} ceq={ceq} Grid_ctor_calls={ctorCalls} explicit_throw={throws} nre_refs={nreRefs} gridState_copy=1 isHome_copy=1 passablePoint_copy=1");
    Console.WriteLine("FRAMEWORK_REFERENCE_GATE_PASS system_private_corelib_refs=0");

    var changedM=methods.Where(m=>beforeM[Raw(m)]!=MethodSig(m)).Select(Raw).ToList();
    if (changedM.Count!=1 || changedM[0]!=TargetToken) throw new InvalidDataException("method semantic isolation failed: "+string.Join(',',changedM.Select(x=>$"0x{x:X8}")));
    Console.WriteLine($"SEMANTIC_ISOLATION_PASS untouched_methods={methods.Count-1} changed_methods=1 target=0x{TargetToken:X8}");
    var changedF=fields.Where(f=>beforeF[Raw(f)]!=FieldSig(f)).Select(Raw).ToList();
    if (changedF.Count!=0) throw new InvalidDataException("field metadata isolation failed");
    Console.WriteLine($"FIELD_METADATA_ISOLATION_PASS unchanged_fields={fields.Count} changed_fields=0");
    foreach (var tok in preservation) if (MethodSig(methods.Single(m=>Raw(m)==tok))!=preserveBefore[tok]) throw new InvalidDataException($"preservation drift 0x{tok:X8}");
    Console.WriteLine("PRESERVATION_PASS batch1=4 plant_ctor=1 projectile_ctor=1 projectile_hf21=2 managers=2 plant_detail_initialize=1 load_skill_logo=1 load_data=1 create_map=1");
    Console.WriteLine("PATCH_GRID_COPY_V2 method_body_changes=1 pc_native_semantics=1 framework_ref_pollution=0 metadata_visibility_changes=0");
}
return 0;
