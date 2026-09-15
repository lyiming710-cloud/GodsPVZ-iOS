using Mono.Cecil;
using Mono.Cecil.Cil;
using System.Globalization;
using System.Security.Cryptography;
using System.Text;

if (args.Length != 4)
{
    Console.Error.WriteLine("Usage: Stage9SuppliesCtorPatch <input.dll> <output.dll> <expected-input-sha256> <unityjit-linux-mscorlib.dll>");
    return 2;
}

const uint TargetToken = 0x0600025C;
const int ExpectedRid = 604;
const int ExpectedMethods = 2317;
const int ExpectedFields = 2802;
const int ExpectedOldCodeSize = 285;
const int ExpectedOldLocals = 14;
const string ExpectedCorelibSha = "4d1725f1ae54a22f69b79c2b1569181ed6653dd484c35005e29c40300c6673be";
const string NativeSpanSha = "3688ac9e8b1ca81c33bb128a816e2a5ab536fd6e48126a5e093749dfd964c6f8";

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
static IEnumerable<MethodReference> MethodRefs(IEnumerable<MethodDefinition> methods) => methods
    .Where(m => m.HasBody).SelectMany(m => m.Body.Instructions).Select(i => i.Operand).OfType<MethodReference>();
static MethodReference FindRef(IEnumerable<MethodDefinition> methods, Func<MethodReference,bool> pred, string label)
{
    var hits = MethodRefs(methods).Where(pred)
        .GroupBy(m => m.FullName + "@" + Scope(m.DeclaringType.Scope), StringComparer.Ordinal)
        .Select(g => g.First()).ToList();
    if (hits.Count != 1) throw new InvalidDataException($"{label} refs={hits.Count}: {string.Join(" | ", hits.Select(h => h.FullName))}");
    return hits[0];
}
static MethodReference MakeHostInstanceMethod(ModuleDefinition targetModule, MethodDefinition openDefinition, TypeReference closedHost)
{
    var open = targetModule.ImportReference(openDefinition);
    var host = new MethodReference(open.Name, open.ReturnType, closedHost)
    {
        HasThis = open.HasThis,
        ExplicitThis = open.ExplicitThis,
        CallingConvention = open.CallingConvention
    };
    foreach (var p in open.Parameters)
        host.Parameters.Add(new ParameterDefinition(p.Name, p.Attributes, p.ParameterType));
    foreach (var gp in open.GenericParameters)
        host.GenericParameters.Add(new GenericParameter(gp.Name, host));
    return host;
}

var input = Path.GetFullPath(args[0]);
var output = Path.GetFullPath(args[1]);
var expectedInputSha = args[2].Trim().ToLowerInvariant();
var corelib = Path.GetFullPath(args[3]);
if (expectedInputSha.Length != 64 || expectedInputSha.Any(c => !Uri.IsHexDigit(c))) throw new InvalidDataException("expected input SHA is not 64 hex chars");
if (Sha(input) != expectedInputSha) throw new InvalidDataException("input SHA mismatch");
if (Sha(corelib) != ExpectedCorelibSha) throw new InvalidDataException("Unity corelib SHA mismatch");
Console.WriteLine($"INPUT_SHA256 {Sha(input)}");
Console.WriteLine($"UNITYJIT_LINUX_MSCORLIB_SHA256 {Sha(corelib)}");

var beforeM = new Dictionary<uint,string>();
var beforeF = new Dictionary<uint,string>();
string beforeRefs;
using var core = ModuleDefinition.ReadModule(corelib, new ReaderParameters { InMemory=true, ReadingMode=ReadingMode.Immediate });
var listOpen = core.GetType("System.Collections.Generic.List`1") ?? throw new InvalidDataException("List`1 not found in exact corelib");
var listCtorDef = listOpen.Methods.Single(m => m.Name == ".ctor" && !m.IsStatic && m.Parameters.Count == 0);
var listAddDef = listOpen.Methods.Single(m => m.Name == "Add" && !m.IsStatic && m.Parameters.Count == 1 && m.Parameters[0].ParameterType is GenericParameter);

using (var module = ModuleDefinition.ReadModule(input, new ReaderParameters { InMemory=true, ReadingMode=ReadingMode.Immediate }))
{
    var types = AllTypes(module.Types).ToList();
    var methods = types.SelectMany(t => t.Methods).ToList();
    var fields = types.SelectMany(t => t.Fields).ToList();
    if (methods.Count != ExpectedMethods || fields.Count != ExpectedFields) throw new InvalidDataException("metadata count drift");
    if (module.AssemblyReferences.Any(a => a.Name == "System.Private.CoreLib")) throw new InvalidDataException("input references System.Private.CoreLib");
    foreach (var m in methods) beforeM[Raw(m)] = MethodSig(m);
    foreach (var f in fields) beforeF[Raw(f)] = FieldSig(f);
    beforeRefs = string.Join("\n", module.AssemblyReferences.Select(a => a.FullName).OrderBy(x => x, StringComparer.Ordinal));

    var t = types.Single(x => x.FullName == "SuppliesInitialValue");
    var target = t.Methods.Single(m => m.IsConstructor && !m.IsStatic && m.Parameters.Count == 0);
    if (Raw(target) != TargetToken || target.MetadataToken.RID != ExpectedRid || target.Body.CodeSize != ExpectedOldCodeSize || target.Body.Variables.Count != ExpectedOldLocals)
        throw new InvalidDataException($"Supplies ctor fingerprint drift token=0x{Raw(target):X8} rid={target.MetadataToken.RID} size={target.Body.CodeSize} locals={target.Body.Variables.Count}");
    var textOps = target.Body.Instructions.Select(i => i.Operand).ToList();
    if (!textOps.OfType<string>().Any(s => s.Contains("Method not found @180302170", StringComparison.Ordinal))) throw new InvalidDataException("expected fake diagnostic missing");
    if (!target.Body.Instructions.Any(i => i.Operand is FieldReference fr && fr.DeclaringType.FullName == "System.Collections.Generic.List`1<System.Object>" && fr.Name == "_version")) throw new InvalidDataException("expected List<object> internal growth corruption missing");
    if (!target.Body.Instructions.Any(i => i.Operand is MethodReference mr && mr.DeclaringType.FullName == "System.Collections.Generic.List`1<System.Object>" && mr.Name == "AddWithResize")) throw new InvalidDataException("expected AddWithResize corruption missing");

    var suppliesInfos = t.Fields.Single(f => f.Name == "suppliesInfos");
    if (suppliesInfos.FieldType is not GenericInstanceType listHost || listHost.ElementType.FullName != "System.Collections.Generic.List`1" || listHost.GenericArguments.Count != 1 || listHost.GenericArguments[0].FullName != "SuppliesInfo")
        throw new InvalidDataException("suppliesInfos is not List<SuppliesInfo>");
    var suppliesInfoType = listHost.GenericArguments[0];
    var suppliesInfoCtor = FindRef(methods, m => m.DeclaringType.FullName == "SuppliesInfo" && m.Name == ".ctor" && m.Parameters.Count == 1 && m.Parameters[0].ParameterType.FullName == "System.Int32", "SuppliesInfo.ctor(int)");
    var objectCtor = FindRef(methods, m => m.DeclaringType.FullName == "System.Object" && m.Name == ".ctor" && m.Parameters.Count == 0, "System.Object.ctor");
    var listCtor = MakeHostInstanceMethod(module, listCtorDef, listHost);
    var listAdd = MakeHostInstanceMethod(module, listAddDef, listHost);
    if (listCtor.DeclaringType is not GenericInstanceType ctorHost || ctorHost.GenericArguments.Single().FullName != "SuppliesInfo") throw new InvalidDataException("closed List ctor host mismatch");
    if (listAdd.DeclaringType is not GenericInstanceType addHost || addHost.GenericArguments.Single().FullName != "SuppliesInfo") throw new InvalidDataException("closed List Add host mismatch");
    if (listAdd.Parameters.Count != 1 || listAdd.Parameters[0].ParameterType is not GenericParameter) throw new InvalidDataException("List Add imported signature drift");

    Console.WriteLine($"NATIVE_AUTHORITY target_token=0x{TargetToken:X8} rid={ExpectedRid} method_pointer_entry=0x181B84038 va=0x180341320 end=0x18034146C native_span_sha256={NativeSpanSha}");
    Console.WriteLine("NATIVE_SEMANTICS base_ctor=System.Object suppliesInfos=List<SuppliesInfo> loop_start=0 loop_end_exclusive=512 item_ctor=SuppliesInfo(int) add=List<SuppliesInfo>.Add");
    Console.WriteLine("RECOVERY_STRATEGY full_single_MethodDef_rebuild=1 exact_unityjit_linux_List_ctor=1 exact_unityjit_linux_List_Add=1 list_object_internals=0 metadata_changes=0 guards=0 exception_swallowing=0");

    var body = target.Body;
    body.Instructions.Clear(); body.Variables.Clear(); body.ExceptionHandlers.Clear();
    body.InitLocals = true; body.MaxStackSize = 4;
    var iVar = new VariableDefinition(module.TypeSystem.Int32);
    body.Variables.Add(iVar);
    var il = body.GetILProcessor();

    il.Append(il.Create(OpCodes.Ldarg_0));
    il.Append(il.Create(OpCodes.Call, objectCtor));
    il.Append(il.Create(OpCodes.Ldarg_0));
    il.Append(il.Create(OpCodes.Newobj, listCtor));
    il.Append(il.Create(OpCodes.Stfld, suppliesInfos));
    il.Append(il.Create(OpCodes.Ldc_I4_0));
    il.Append(il.Create(OpCodes.Stloc, iVar));
    var test = il.Create(OpCodes.Ldloc, iVar);
    var done = il.Create(OpCodes.Ret);
    il.Append(il.Create(OpCodes.Br, test));
    var loop = il.Create(OpCodes.Ldarg_0);
    il.Append(loop);
    il.Append(il.Create(OpCodes.Ldfld, suppliesInfos));
    il.Append(il.Create(OpCodes.Ldloc, iVar));
    il.Append(il.Create(OpCodes.Newobj, suppliesInfoCtor));
    il.Append(il.Create(OpCodes.Callvirt, listAdd));
    il.Append(il.Create(OpCodes.Ldloc, iVar));
    il.Append(il.Create(OpCodes.Ldc_I4_1));
    il.Append(il.Create(OpCodes.Add));
    il.Append(il.Create(OpCodes.Stloc, iVar));
    il.Append(test);
    il.Append(il.Create(OpCodes.Ldc_I4, 512));
    il.Append(il.Create(OpCodes.Blt, loop));
    il.Append(done);

    Directory.CreateDirectory(Path.GetDirectoryName(output)!);
    module.Write(output);
}

Console.WriteLine($"OUTPUT_SHA256 {Sha(output)}");
using (var module = ModuleDefinition.ReadModule(output, new ReaderParameters { InMemory=true, ReadingMode=ReadingMode.Immediate }))
{
    var types=AllTypes(module.Types).ToList(); var methods=types.SelectMany(t=>t.Methods).ToList(); var fields=types.SelectMany(t=>t.Fields).ToList();
    var target=methods.Single(m=>Raw(m)==TargetToken);
    if(target.MetadataToken.RID!=ExpectedRid || target.Body.Variables.Count!=1 || target.Body.Variables[0].VariableType.FullName!="System.Int32") throw new InvalidDataException("reopen target/local drift");
    var targetText=string.Join("\n",target.Body.Instructions.Select(i=>$"{i.OpCode} {i.Operand}"));
    foreach(var bad in new[]{"List`1<System.Object>","_version","_items","_size","AddWithResize","Method not found @","System.Private.CoreLib"}) if(targetText.Contains(bad,StringComparison.Ordinal)) throw new InvalidDataException("corruption remains: "+bad);
    if(target.Body.Instructions.Count(i=>i.OpCode==OpCodes.Newobj && i.Operand is MethodReference mr && mr.DeclaringType.FullName=="SuppliesInfo" && mr.Parameters.Count==1)!=1) throw new InvalidDataException("SuppliesInfo ctor count drift");
    if(target.Body.Instructions.Count(i=>(i.OpCode==OpCodes.Call || i.OpCode==OpCodes.Callvirt) && i.Operand is MethodReference mr && mr.Name=="Add" && mr.DeclaringType is GenericInstanceType git && git.GenericArguments.Single().FullName=="SuppliesInfo")!=1) throw new InvalidDataException("typed List Add missing");
    if(!target.Body.Instructions.Any(i=>i.OpCode==OpCodes.Ldc_I4 && i.Operand is int n && n==512)) throw new InvalidDataException("loop bound 512 missing");
    if(!target.Body.Instructions.Any(i=>i.OpCode==OpCodes.Call && i.Operand is MethodReference mr && mr.Name==".ctor" && mr.DeclaringType.FullName=="System.Object")) throw new InvalidDataException("base Object ctor missing");
    var changedM=methods.Where(m=>beforeM[Raw(m)]!=MethodSig(m)).Select(Raw).OrderBy(x=>x).ToList();
    if(changedM.Count!=1 || changedM[0]!=TargetToken) throw new InvalidDataException("semantic isolation failed: "+string.Join(',',changedM.Select(x=>$"0x{x:X8}")));
    var changedF=fields.Where(f=>beforeF[Raw(f)]!=FieldSig(f)).Select(Raw).ToList(); if(changedF.Count!=0) throw new InvalidDataException("field metadata drift");
    var afterRefs=string.Join("\n",module.AssemblyReferences.Select(a=>a.FullName).OrderBy(x=>x,StringComparer.Ordinal)); if(afterRefs!=beforeRefs) throw new InvalidDataException("assembly reference set changed");
    if(module.AssemblyReferences.Any(a=>a.Name=="System.Private.CoreLib")) throw new InvalidDataException("System.Private.CoreLib pollution");
    Console.WriteLine($"REOPEN_SUPPLIES_CTOR_PASS token=0x{TargetToken:X8} code_size={target.Body.CodeSize} locals=1 list_object_internal_refs=0 fake_diagnostics=0 suppliesInfo_ctor=1 typed_List_Add=1 loop_bound_512=1 base_object_ctor=1");
    Console.WriteLine("GENERIC_MEMBERREF_RESOLUTION_PASS list_ctor=1 list_add=1 corelib_sha256="+ExpectedCorelibSha);
    Console.WriteLine($"SEMANTIC_ISOLATION_PASS untouched_methods={ExpectedMethods-1} changed_methods=1 target=0x{TargetToken:X8}");
    Console.WriteLine($"FIELD_METADATA_ISOLATION_PASS unchanged_fields={ExpectedFields} changed_fields=0");
    Console.WriteLine("FRAMEWORK_REFERENCE_GATE_PASS system_private_corelib_refs=0 assembly_reference_set_unchanged=1");
}
return 0;
