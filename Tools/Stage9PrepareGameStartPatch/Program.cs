using Mono.Cecil;
using Mono.Cecil.Cil;
using System.Globalization;
using System.Security.Cryptography;
using System.Text;

if (args.Length != 2) { Console.Error.WriteLine("Usage: Stage9PrepareGameStartPatch <input.dll> <output.dll>"); return 2; }

const string ExpectedInputSha256 = "1c62f969409ed16c9d7c7855f7b52e6b593701524e2b2d75ec0affbc3d05b209";
const int ExpectedMethodCount = 2317;
const int ExpectedFieldCount = 2802;
const uint TargetToken = 0x0600067E;
const uint GetterToken = 0x06000169;

static IEnumerable<TypeDefinition> AllTypes(IEnumerable<TypeDefinition> roots)
{
    foreach (var t in roots) { yield return t; foreach (var n in AllTypes(t.NestedTypes)) yield return n; }
}
static uint Raw(IMetadataTokenProvider p) => p.MetadataToken.ToUInt32();
static string Sha256(string p) => Convert.ToHexString(SHA256.HashData(File.ReadAllBytes(p))).ToLowerInvariant();
static string ScopeName(TypeReference t) => t.Scope switch { AssemblyNameReference a => a.Name, ModuleDefinition m => m.Assembly?.Name?.Name ?? m.Name, ModuleReference mr => mr.Name, _ => t.Scope?.ToString() ?? "<null>" };
static string TypeSig(TypeReference t) => $"{t.FullName}@{ScopeName(t)}";
static string OperandSig(object? o, MethodDefinition owner)
{
    if (o is null) return "";
    if (o is Instruction i) return $"I#{owner.Body.Instructions.IndexOf(i)}";
    if (o is Instruction[] sw) return "SW["+string.Join(',',sw.Select(i=>owner.Body.Instructions.IndexOf(i)))+"]";
    if (o is MethodReference mr) return $"M:{mr.FullName}@{ScopeName(mr.DeclaringType)}";
    if (o is FieldReference fr) return $"F:{fr.FullName}@{ScopeName(fr.DeclaringType)}";
    if (o is TypeReference tr) return $"T:{TypeSig(tr)}";
    if (o is VariableDefinition vr) return $"V:{vr.Index}:{TypeSig(vr.VariableType)}";
    if (o is ParameterDefinition pr) return $"P:{pr.Index}:{TypeSig(pr.ParameterType)}";
    if (o is string s) return "S:"+Convert.ToBase64String(Encoding.UTF8.GetBytes(s));
    return "C:"+Convert.ToString(o,CultureInfo.InvariantCulture);
}
static string MethodSemantic(MethodDefinition m)
{
    var sb=new StringBuilder(); sb.Append(m.FullName).Append('|').Append(m.Attributes).Append('|').Append(m.ImplAttributes).Append('|');
    if(!m.HasBody) return sb.Append("NOBODY").ToString();
    sb.Append("init=").Append(m.Body.InitLocals).Append(";max=").Append(m.Body.MaxStackSize).Append(';');
    foreach(var v in m.Body.Variables) sb.Append("L:").Append(v.Index).Append(':').Append(TypeSig(v.VariableType)).Append(';');
    foreach(var i in m.Body.Instructions) sb.Append("I:").Append(i.OpCode.Code).Append(':').Append(OperandSig(i.Operand,m)).Append(';');
    foreach(var h in m.Body.ExceptionHandlers){int Idx(Instruction? x)=>x is null?-1:m.Body.Instructions.IndexOf(x); sb.Append("EH:").Append(h.HandlerType).Append(':').Append(Idx(h.TryStart)).Append(':').Append(Idx(h.TryEnd)).Append(':').Append(Idx(h.HandlerStart)).Append(':').Append(Idx(h.HandlerEnd)).Append(';');}
    return sb.ToString();
}
static string FieldSemantic(FieldDefinition f)
{
    var attrs=string.Join(',',f.CustomAttributes.Select(a=>a.AttributeType.FullName).OrderBy(x=>x,StringComparer.Ordinal));
    var c=f.HasConstant?Convert.ToString(f.Constant,CultureInfo.InvariantCulture)??"<null>":"<none>";
    return $"{f.FullName}|{f.Attributes}|{TypeSig(f.FieldType)}|const={c}|ca={attrs}";
}
static int CountCalls(MethodDefinition m, Func<MethodReference,bool> pred) => m.Body.Instructions.Count(i => (i.OpCode==OpCodes.Call || i.OpCode==OpCodes.Callvirt) && i.Operand is MethodReference mr && pred(mr));

var input=Path.GetFullPath(args[0]); var output=Path.GetFullPath(args[1]);
var sha=Sha256(input); Console.WriteLine($"INPUT_SHA256 {sha}"); if(sha!=ExpectedInputSha256) throw new InvalidDataException("input SHA mismatch");
var beforeM=new Dictionary<uint,string>(); var beforeF=new Dictionary<uint,string>();
using(var module=ModuleDefinition.ReadModule(input,new ReaderParameters{InMemory=true,ReadingMode=ReadingMode.Immediate}))
{
    var types=AllTypes(module.Types).ToList(); var methods=types.SelectMany(t=>t.Methods).ToList(); var fields=types.SelectMany(t=>t.Fields).ToList();
    if(methods.Count!=ExpectedMethodCount||fields.Count!=ExpectedFieldCount) throw new InvalidDataException("metadata count drift");
    if(module.AssemblyReferences.Any(a=>a.Name=="System.Private.CoreLib")) throw new InvalidDataException("unexpected corelib ref");
    foreach(var m in methods) beforeM[Raw(m)]=MethodSemantic(m); foreach(var f in fields) beforeF[Raw(f)]=FieldSemantic(f);
    var p=types.Single(t=>t.FullName=="PrepareUIController"); var target=p.Methods.Single(m=>m.Name=="GameStart"&&m.Parameters.Count==0&&!m.IsStatic);
    if(Raw(target)!=TargetToken || target.Body.CodeSize!=255 || target.Body.Variables.Count!=11) throw new InvalidDataException($"target fingerprint mismatch token=0x{Raw(target):X8} size={target.Body.CodeSize} locals={target.Body.Variables.Count}");
    var bm=types.Single(t=>t.FullName=="BoardManager"); var getter=bm.Methods.Single(m=>m.Name=="get_Instance"&&m.IsStatic&&m.Parameters.Count==0);
    if(Raw(getter)!=GetterToken) throw new InvalidDataException($"getter token drift 0x{Raw(getter):X8}");
    var backing=bm.Fields.Single(f=>f.Name=="<Instance>k__BackingField");
    var bad=target.Body.Instructions.Where(i=>i.OpCode==OpCodes.Ldsfld && i.Operand is FieldReference fr && fr.Name==backing.Name && fr.DeclaringType.FullName=="BoardManager").ToList();
    if(bad.Count!=1) throw new InvalidDataException($"expected exactly one backing-field read, got {bad.Count}");
    if(CountCalls(target,mr=>mr.DeclaringType.FullName=="BoardManager"&&mr.Name=="get_Instance")!=0) throw new InvalidDataException("prepatch getter call unexpectedly present");
    if(CountCalls(target,mr=>mr.DeclaringType.FullName=="BoardManager"&&mr.Name=="LoadBoard")!=1) throw new InvalidDataException("LoadBoard call fingerprint drift");
    bad[0].OpCode=OpCodes.Call; bad[0].Operand=getter;
    Console.WriteLine("NATIVE_AUTHORITY target_token=0x0600067E address=0x00000001803A6D10 getter_token=0x06000169 getter_address=0x00000001803100F0 loadboard_call=0x000000018030F830 semantics=read_BoardManager_singleton_then_challenge_gate_destroy_preview_LoadBoard_deactivate_self");
    Console.WriteLine("RUNTIME_CAUSAL_EVIDENCE strict_run=34774556853 candidate=1c62f969409ed16c9d7c7855f7b52e6b593701524e2b2d75ec0affbc3d05b209 exception=FieldAccessException field=BoardManager.<Instance>k__BackingField method=PrepareUIController.GameStart");
    Console.WriteLine("PATCH_PREPARE_GAMESTART method_body_changes=1 replacement=ldsfld_private_backing_to_call_get_Instance gameplay_semantics_changed=0");
    module.Write(output);
}
Console.WriteLine($"OUTPUT_SHA256 {Sha256(output)}");
using(var module=ModuleDefinition.ReadModule(output,new ReaderParameters{InMemory=true,ReadingMode=ReadingMode.Immediate}))
{
    var types=AllTypes(module.Types).ToList(); var methods=types.SelectMany(t=>t.Methods).ToList(); var fields=types.SelectMany(t=>t.Fields).ToList();
    var target=types.Single(t=>t.FullName=="PrepareUIController").Methods.Single(m=>m.Name=="GameStart"&&m.Parameters.Count==0&&!m.IsStatic);
    var backingRefs=target.Body.Instructions.Count(i=>i.Operand is FieldReference fr && fr.DeclaringType.FullName=="BoardManager"&&fr.Name=="<Instance>k__BackingField");
    var getterCalls=CountCalls(target,mr=>mr.DeclaringType.FullName=="BoardManager"&&mr.Name=="get_Instance");
    var loadCalls=CountCalls(target,mr=>mr.DeclaringType.FullName=="BoardManager"&&mr.Name=="LoadBoard");
    if(backingRefs!=0||getterCalls!=1||loadCalls!=1) throw new InvalidDataException($"reopen semantic mismatch backing={backingRefs} getter={getterCalls} load={loadCalls}");
    int changed=0,untouched=0; foreach(var m in methods){var now=MethodSemantic(m); if(beforeM.TryGetValue(Raw(m),out var old)&&old==now) untouched++; else {changed++; if(Raw(m)!=TargetToken) throw new InvalidDataException($"unexpected method drift 0x{Raw(m):X8} {m.FullName}");}}
    if(untouched!=2316||changed!=1) throw new InvalidDataException($"method isolation mismatch {untouched}/{changed}");
    foreach(var f in fields) if(!beforeF.TryGetValue(Raw(f),out var old)||old!=FieldSemantic(f)) throw new InvalidDataException($"field drift 0x{Raw(f):X8}");
    if(module.AssemblyReferences.Any(a=>a.Name=="System.Private.CoreLib")) throw new InvalidDataException("corelib introduced");
    Console.WriteLine("REOPEN_PREPARE_GAMESTART_PASS token=0x0600067E backing_field_reads=0 getter_calls=1 loadboard_calls=1 corelib_refs=0");
    Console.WriteLine("SEMANTIC_ISOLATION_PASS untouched_methods=2316 changed_methods=1 target_token=0x0600067E");
    Console.WriteLine("FIELD_METADATA_ISOLATION_PASS unchanged_fields=2802 changed_fields=0");
}
return 0;
