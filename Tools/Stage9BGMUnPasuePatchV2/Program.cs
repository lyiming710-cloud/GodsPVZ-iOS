using Mono.Cecil;
using Mono.Cecil.Cil;
using System.Globalization;
using System.Security.Cryptography;
using System.Text;

if (args.Length != 3)
{
    Console.Error.WriteLine("Usage: Stage9BGMUnPasuePatchV2 <input.dll> <output.dll> <unityjit-linux-mscorlib.dll>");
    return 2;
}

const string ExpectedInputSha = "a005d602d20814c5917d6f36ac7d85ee3013615aea54019ac417f5c84aed3b28";
const string ExpectedCorelibSha = "4d1725f1ae54a22f69b79c2b1569181ed6653dd484c35005e29c40300c6673be";
const uint TargetToken = 0x06000278;
const int ExpectedRid = 632;
const uint HealthySiblingToken = 0x06000277;
const uint ZombieUnPauseToken = 0x0600041A;
const int ExpectedMethods = 2317;
const int ExpectedFields = 2802;
const int ExpectedOldCodeSize = 138;
const int ExpectedOldLocals = 10;
const int ExpectedSiblingCodeSize = 52;
const string NativeSliceSha = "70e89e1031d7c703b2e919b2669723929e0741f48233720eddf91120df9fd832";
const string HealthySiblingNativeSliceSha = "e240f87fe9eedf3bdb620bddb4f0a94568aeab511b5e7dd9c1f2fba77e1bb42f";

var preservation = new HashSet<uint>
{
    0x0600067E, 0x06000285, 0x06000289, 0x0600028A,
    0x060003BA, 0x060003FD, 0x060003DD, 0x060003DE,
    0x060001F3, 0x06000216, 0x06000667, 0x06000668,
    0x060005E1, 0x06000157, 0x06000294, 0x06000193,
    0x060002BD, HealthySiblingToken
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
static int MscorlibRefs(ModuleDefinition m) => m.AssemblyReferences.Count(a => a.Name == "mscorlib");

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

static void AssertOpenListSignatures(MethodReference getEnumerator, MethodReference getCurrent)
{
    if (getEnumerator.ReturnType is not GenericInstanceType ret || ret.GenericArguments.Count != 1 ||
        ret.GenericArguments[0] is not GenericParameter gp0 || gp0.Position != 0 || gp0.Type != GenericParameterType.Type)
        throw new InvalidDataException("GetEnumerator MemberRef return lost open !0 generic variable");
    if (getCurrent.ReturnType is not GenericParameter gp1 || gp1.Position != 0 || gp1.Type != GenericParameterType.Type)
        throw new InvalidDataException("get_Current MemberRef return lost open !0 generic variable");
}

var input = Path.GetFullPath(args[0]);
var output = Path.GetFullPath(args[1]);
var corelib = Path.GetFullPath(args[2]);
if (Sha(input) != ExpectedInputSha) throw new InvalidDataException("input SHA mismatch");
if (Sha(corelib) != ExpectedCorelibSha) throw new InvalidDataException("Unity corelib SHA mismatch");
Console.WriteLine($"INPUT_SHA256 {Sha(input)}");
Console.WriteLine($"UNITYJIT_LINUX_MSCORLIB_SHA256 {Sha(corelib)}");

var beforeM = new Dictionary<uint,string>();
var beforeF = new Dictionary<uint,string>();
var preserveBefore = new Dictionary<uint,string>();
int mscorlibRefsBefore;

using var core = ModuleDefinition.ReadModule(corelib, new ReaderParameters { InMemory=true, ReadingMode=ReadingMode.Immediate });
var listOpen = core.GetType("System.Collections.Generic.List`1") ?? throw new InvalidDataException("List`1 not found in exact corelib");
var enumeratorOpen = listOpen.NestedTypes.Single(t => t.Name == "Enumerator");
var disposableOpen = core.GetType("System.IDisposable") ?? throw new InvalidDataException("IDisposable not found in exact corelib");
var getEnumeratorDef = listOpen.Methods.Single(m => m.Name == "GetEnumerator" && !m.IsStatic && m.Parameters.Count == 0);
var getCurrentDef = enumeratorOpen.Methods.Single(m => m.Name == "get_Current" && !m.IsStatic && m.Parameters.Count == 0);
var moveNextDef = enumeratorOpen.Methods.Single(m => m.Name == "MoveNext" && !m.IsStatic && m.Parameters.Count == 0);
var disposeDef = disposableOpen.Methods.Single(m => m.Name == "Dispose" && !m.IsStatic && m.Parameters.Count == 0);

using (var module = ModuleDefinition.ReadModule(input, new ReaderParameters { InMemory=true, ReadingMode=ReadingMode.Immediate }))
{
    if (CoreLibRefs(module) != 0) throw new InvalidDataException("input references System.Private.CoreLib");
    mscorlibRefsBefore = MscorlibRefs(module);
    if (mscorlibRefsBefore != 1) throw new InvalidDataException($"unexpected mscorlib reference count {mscorlibRefsBefore}");
    var types = AllTypes(module.Types).ToList();
    var methods = types.SelectMany(t => t.Methods).ToList();
    var fields = types.SelectMany(t => t.Fields).ToList();
    if (methods.Count != ExpectedMethods || fields.Count != ExpectedFields) throw new InvalidDataException("metadata count drift");
    foreach (var m in methods) beforeM[Raw(m)] = MethodSig(m);
    foreach (var f in fields) beforeF[Raw(f)] = FieldSig(f);
    foreach (var tok in preservation) preserveBefore[tok] = beforeM[tok];

    TypeDefinition T(string n) => types.Single(t => t.FullName == n);
    var manager = T("ZombieManager");
    var zombie = T("Zombie");
    var target = manager.Methods.Single(m => m.Name == "BGMUnPasue" && m.Parameters.Count == 0);
    var sibling = manager.Methods.Single(m => m.Name == "BGMPasue" && m.Parameters.Count == 0);
    var zombieUnPause = zombie.Methods.Single(m => m.Name == "BGMUnPasue" && m.Parameters.Count == 0);

    if (Raw(target) != TargetToken || target.MetadataToken.RID != ExpectedRid || target.Body.CodeSize != ExpectedOldCodeSize || target.Body.Variables.Count != ExpectedOldLocals)
        throw new InvalidDataException("ZombieManager.BGMUnPasue fingerprint drift");
    if (Raw(sibling) != HealthySiblingToken || sibling.Body.CodeSize != ExpectedSiblingCodeSize || sibling.Body.Variables.Count != 1 || sibling.Body.ExceptionHandlers.Count != 1 || sibling.Body.ExceptionHandlers[0].HandlerType != ExceptionHandlerType.Finally)
        throw new InvalidDataException("healthy BGMPasue sibling fingerprint drift");
    if (Raw(zombieUnPause) != ZombieUnPauseToken)
        throw new InvalidDataException("Zombie.BGMUnPasue token drift");
    if (!target.Body.Instructions.Any(i => i.Offset == 0x19 && i.OpCode == OpCodes.Ceq))
        throw new InvalidDataException("expected damaged IL_0019 ceq missing");

    var zombieList = manager.Fields.Single(f => f.Name == "zombieList" && f.FieldType.FullName == "System.Collections.Generic.List`1<Zombie>");
    var enumeratorType = sibling.Body.Variables[0].VariableType;
    if (!enumeratorType.FullName.Contains("System.Collections.Generic.List`1/Enumerator<Zombie>", StringComparison.Ordinal))
        throw new InvalidDataException("healthy sibling enumerator local drift");

    var getEnumerator = MakeHostInstanceMethod(module, getEnumeratorDef, zombieList.FieldType);
    var getCurrent = MakeHostInstanceMethod(module, getCurrentDef, enumeratorType);
    var moveNext = MakeHostInstanceMethod(module, moveNextDef, enumeratorType);
    var dispose = module.ImportReference(disposeDef);
    AssertOpenListSignatures(getEnumerator, getCurrent);

    Console.WriteLine($"NATIVE_AUTHORITY target_token=0x{TargetToken:X8} rid={ExpectedRid} method_pointer_entry=0x181B84118 va=0x180341800 end=0x1803418FC native_slice_sha256={NativeSliceSha}");
    Console.WriteLine($"NATIVE_SIBLING_AUTHORITY token=0x{HealthySiblingToken:X8} rid=631 method_pointer_entry=0x181B84110 va=0x180341700 end=0x1803417FC native_slice_sha256={HealthySiblingNativeSliceSha}");
    Console.WriteLine("NATIVE_SEMANTICS zombieList_foreach=1 typed_Zombie_enumerator=1 per_item_BGMUnPasue=1 dispose_finally=1 pause_unpause_structural_twin=1");
    Console.WriteLine("RECOVERY_STRATEGY exact_unityjit_linux_corelib_methoddefs=1 closed_host_open_member_signature=1 per_item_call=Zombie.BGMUnPasue metadata_changes=0 null_guard_changes=0 exception_swallowing=0");

    var body = target.Body;
    body.Instructions.Clear(); body.Variables.Clear(); body.ExceptionHandlers.Clear();
    body.InitLocals = true; body.MaxStackSize = 1;
    var enumVar = new VariableDefinition(enumeratorType);
    body.Variables.Add(enumVar);
    var il = body.GetILProcessor();

    var loopBody = il.Create(OpCodes.Ldloca_S, enumVar);
    var loopCheck = il.Create(OpCodes.Ldloca_S, enumVar);
    var finallyStart = il.Create(OpCodes.Ldloca_S, enumVar);
    var ret = il.Create(OpCodes.Ret);
    var tryStart = il.Create(OpCodes.Br_S, loopCheck);

    il.Append(il.Create(OpCodes.Ldarg_0));
    il.Append(il.Create(OpCodes.Ldfld, zombieList));
    il.Append(il.Create(OpCodes.Callvirt, getEnumerator));
    il.Append(il.Create(OpCodes.Stloc_0));
    il.Append(tryStart);
    il.Append(loopBody);
    il.Append(il.Create(OpCodes.Call, getCurrent));
    il.Append(il.Create(OpCodes.Callvirt, zombieUnPause));
    il.Append(loopCheck);
    il.Append(il.Create(OpCodes.Call, moveNext));
    il.Append(il.Create(OpCodes.Brtrue_S, loopBody));
    il.Append(il.Create(OpCodes.Leave_S, ret));
    il.Append(finallyStart);
    il.Append(il.Create(OpCodes.Constrained, enumeratorType));
    il.Append(il.Create(OpCodes.Callvirt, dispose));
    il.Append(il.Create(OpCodes.Endfinally));
    il.Append(ret);

    body.ExceptionHandlers.Add(new ExceptionHandler(ExceptionHandlerType.Finally)
    {
        TryStart = tryStart,
        TryEnd = finallyStart,
        HandlerStart = finallyStart,
        HandlerEnd = ret
    });

    Directory.CreateDirectory(Path.GetDirectoryName(output)!);
    module.Write(output);
}

Console.WriteLine($"OUTPUT_SHA256 {Sha(output)}");

var resolver = new DefaultAssemblyResolver();
resolver.AddSearchDirectory(Path.GetDirectoryName(corelib)!);
resolver.AddSearchDirectory(Path.GetDirectoryName(output)!);
using (var module = ModuleDefinition.ReadModule(output, new ReaderParameters { InMemory=true, ReadingMode=ReadingMode.Immediate, AssemblyResolver=resolver }))
{
    if (CoreLibRefs(module) != 0) throw new InvalidDataException("System.Private.CoreLib reference pollution");
    if (MscorlibRefs(module) != mscorlibRefsBefore) throw new InvalidDataException("mscorlib AssemblyRef count drift");
    var types=AllTypes(module.Types).ToList();
    var methods=types.SelectMany(t=>t.Methods).ToList();
    var fields=types.SelectMany(t=>t.Fields).ToList();
    var target=methods.Single(m=>Raw(m)==TargetToken);
    var sibling=methods.Single(m=>Raw(m)==HealthySiblingToken);

    int MethodCalls(string name, string? type = null) => target.Body.Instructions.Count(i =>
        (i.OpCode == OpCodes.Call || i.OpCode == OpCodes.Callvirt) && i.Operand is MethodReference m && m.Name == name && (type is null || m.DeclaringType.FullName == type));
    int FieldRefs(string type,string name) => target.Body.Instructions.Count(i => i.Operand is FieldReference f && f.DeclaringType.FullName==type && f.Name==name);

    var ceq=target.Body.Instructions.Count(i=>i.OpCode==OpCodes.Ceq);
    var objectLocals=target.Body.Variables.Count(v=>v.VariableType.FullName=="System.Object");
    var frameworkExceptionCtors=target.Body.Instructions.Count(i=>i.OpCode==OpCodes.Newobj && i.Operand is MethodReference m &&
        (m.DeclaringType.FullName=="System.NullReferenceException" || m.DeclaringType.FullName=="System.IndexOutOfRangeException"));
    var badStrings=target.Body.Instructions.Count(i=>i.OpCode==OpCodes.Ldstr && i.Operand is string s &&
        (s.Contains("Method not found @",StringComparison.Ordinal) || s.Contains("Warning: Method ends",StringComparison.Ordinal)));
    var constrained=target.Body.Instructions.Count(i=>i.OpCode==OpCodes.Constrained && i.Operand is TypeReference t && t.FullName.Contains("Enumerator<Zombie>",StringComparison.Ordinal));

    if (target.Body.CodeSize != ExpectedSiblingCodeSize || target.Body.Variables.Count!=1 || objectLocals!=0 || ceq!=0 || frameworkExceptionCtors!=0 || badStrings!=0 ||
        target.Body.ExceptionHandlers.Count!=1 || target.Body.ExceptionHandlers[0].HandlerType!=ExceptionHandlerType.Finally || constrained!=1 ||
        FieldRefs("ZombieManager","zombieList")!=1 || MethodCalls("GetEnumerator")!=1 || MethodCalls("get_Current")!=1 || MethodCalls("MoveNext")!=1 ||
        MethodCalls("Dispose","System.IDisposable")!=1 || MethodCalls("BGMUnPasue","Zombie")!=1 || MethodCalls("BGMPasue","Zombie")!=0)
        throw new InvalidDataException("reopen ZombieManager.BGMUnPasue mismatch");

    var getEnumRef = target.Body.Instructions.Select(i=>i.Operand).OfType<MethodReference>().Single(m=>m.Name=="GetEnumerator");
    var currentRef = target.Body.Instructions.Select(i=>i.Operand).OfType<MethodReference>().Single(m=>m.Name=="get_Current");
    var moveRef = target.Body.Instructions.Select(i=>i.Operand).OfType<MethodReference>().Single(m=>m.Name=="MoveNext");
    var disposeRef = target.Body.Instructions.Select(i=>i.Operand).OfType<MethodReference>().Single(m=>m.Name=="Dispose" && m.DeclaringType.FullName=="System.IDisposable");
    AssertOpenListSignatures(getEnumRef, currentRef);

    var resolvedGetEnum = getEnumRef.Resolve() ?? throw new InvalidDataException("GetEnumerator MemberRef did not resolve");
    var resolvedCurrent = currentRef.Resolve() ?? throw new InvalidDataException("get_Current MemberRef did not resolve");
    var resolvedMove = moveRef.Resolve() ?? throw new InvalidDataException("MoveNext MemberRef did not resolve");
    var resolvedDispose = disposeRef.Resolve() ?? throw new InvalidDataException("Dispose MemberRef did not resolve");
    if (resolvedGetEnum.Name!="GetEnumerator" || resolvedGetEnum.DeclaringType.FullName!="System.Collections.Generic.List`1") throw new InvalidDataException("GetEnumerator resolved to wrong MethodDef");
    if (resolvedCurrent.Name!="get_Current" || resolvedCurrent.DeclaringType.Name!="Enumerator") throw new InvalidDataException("get_Current resolved to wrong MethodDef");
    if (resolvedMove.Name!="MoveNext" || resolvedMove.DeclaringType.Name!="Enumerator") throw new InvalidDataException("MoveNext resolved to wrong MethodDef");
    if (resolvedDispose.Name!="Dispose" || resolvedDispose.DeclaringType.FullName!="System.IDisposable") throw new InvalidDataException("Dispose resolved to wrong MethodDef");
    var resolvedCorelibPath = resolvedGetEnum.Module.FileName;
    if (string.IsNullOrEmpty(resolvedCorelibPath) || Sha(resolvedCorelibPath)!=ExpectedCorelibSha) throw new InvalidDataException("generic MemberRefs resolved against wrong corelib");

    Console.WriteLine($"GENERIC_MEMBERREF_RESOLUTION_PASS GetEnumerator=1 get_Current=1 MoveNext=1 Dispose=1 corelib_sha256={ExpectedCorelibSha} open_var_signatures=1");
    Console.WriteLine($"REOPEN_BGMUNPAUSE_V2_PASS token=0x{TargetToken:X8} code_size={target.Body.CodeSize} locals=1 eh_finally=1 ceq=0 object_locals=0 framework_exception_ctors=0 bad_strings=0 zombieList_refs=1 GetEnumerator=1 get_Current=1 MoveNext=1 Dispose=1 Zombie_BGMUnPasue=1 Zombie_BGMPasue=0");
    Console.WriteLine("FRAMEWORK_REFERENCE_GATE_PASS system_private_corelib_refs=0 mscorlib_ref_count_unchanged=1");

    var changedM=methods.Where(m=>beforeM[Raw(m)]!=MethodSig(m)).Select(m=>Raw(m)).OrderBy(x=>x).ToList();
    if (changedM.Count!=1 || changedM[0]!=TargetToken)
        throw new InvalidDataException("semantic method isolation failed: "+string.Join(',',changedM.Select(x=>$"0x{x:X8}")));
    Console.WriteLine($"SEMANTIC_ISOLATION_PASS untouched_methods={ExpectedMethods-1} changed_methods=1 target=0x{TargetToken:X8}");

    var changedF=fields.Where(f=>beforeF[Raw(f)]!=FieldSig(f)).Select(f=>Raw(f)).ToList();
    if (changedF.Count!=0) throw new InvalidDataException("field metadata drift");
    Console.WriteLine($"FIELD_METADATA_ISOLATION_PASS unchanged_fields={ExpectedFields} changed_fields=0");

    foreach (var tok in preservation)
    {
        var m=methods.Single(x=>Raw(x)==tok);
        if (MethodSig(m)!=preserveBefore[tok]) throw new InvalidDataException($"preservation drift 0x{tok:X8}");
    }
    if (MethodSig(sibling)!=preserveBefore[HealthySiblingToken]) throw new InvalidDataException("healthy BGMPasue sibling changed");
    Console.WriteLine("PRESERVATION_PASS batch1=4 plant_ctor=1 projectile_ctor=1 projectile_hf21=2 managers=2 plant_detail_initialize=1 load_skill_logo=1 load_data=1 create_map=1 grid_copy=1 dialogue_start=1 board_gamecontinue=1 healthy_BGMPasue=1");
}

return 0;
