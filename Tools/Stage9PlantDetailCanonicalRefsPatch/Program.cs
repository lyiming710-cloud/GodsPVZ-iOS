using Mono.Cecil;
using Mono.Cecil.Cil;
using System.Globalization;
using System.Security.Cryptography;
using System.Text;

if(args.Length!=2){Console.Error.WriteLine("Usage: Stage9PlantDetailCanonicalRefsPatch <input.dll> <output.dll>");return 2;}
const string ExpectedInputSha="20802aef2aa7cf5d430cf780d7511dd4ddd8afc7e97260903dc9977951cba739";
const uint TargetToken=0x06000667;
const int ExpectedMethods=2317, ExpectedFields=2802;
static IEnumerable<TypeDefinition> AllTypes(IEnumerable<TypeDefinition> roots){foreach(var t in roots){yield return t;foreach(var n in AllTypes(t.NestedTypes))yield return n;}}
static uint Raw(IMetadataTokenProvider p)=>p.MetadataToken.ToUInt32();
static string Sha(string p)=>Convert.ToHexString(SHA256.HashData(File.ReadAllBytes(p))).ToLowerInvariant();
static string ScopeName(TypeReference t)=>t.Scope switch{AssemblyNameReference a=>a.Name,ModuleDefinition m=>m.Assembly?.Name?.Name??m.Name,ModuleReference mr=>mr.Name,_=>t.Scope?.ToString()??"<null>"};
static string TypeSig(TypeReference t)=>$"{t.FullName}@{ScopeName(t)}";
static bool IsCall(Instruction i)=>i.OpCode==OpCodes.Call||i.OpCode==OpCodes.Callvirt||i.OpCode==OpCodes.Newobj;
static string OperandSig(object? o,MethodDefinition owner){if(o is null)return"";if(o is Instruction i)return$"I#{owner.Body.Instructions.IndexOf(i)}";if(o is Instruction[] sw)return"SW["+string.Join(',',sw.Select(i=>owner.Body.Instructions.IndexOf(i)))+"]";if(o is MethodReference mr)return$"M:{mr.FullName}@{ScopeName(mr.DeclaringType)}";if(o is FieldReference fr)return$"F:{fr.FullName}@{ScopeName(fr.DeclaringType)}";if(o is TypeReference tr)return$"T:{TypeSig(tr)}";if(o is VariableDefinition vr)return$"V:{vr.Index}:{TypeSig(vr.VariableType)}";if(o is ParameterDefinition pr)return$"P:{pr.Index}:{TypeSig(pr.ParameterType)}";if(o is string s)return"S:"+Convert.ToBase64String(Encoding.UTF8.GetBytes(s));if(o is float f)return$"R4:{BitConverter.SingleToInt32Bits(f):X8}";if(o is double d)return$"R8:{BitConverter.DoubleToInt64Bits(d):X16}";return"C:"+Convert.ToString(o,CultureInfo.InvariantCulture);}
static string MethodSemantic(MethodDefinition m){var sb=new StringBuilder();sb.Append(m.FullName).Append('|').Append(m.Attributes).Append('|').Append(m.ImplAttributes).Append('|');if(!m.HasBody)return sb.Append("NOBODY").ToString();sb.Append("init=").Append(m.Body.InitLocals).Append(";max=").Append(m.Body.MaxStackSize).Append(';');foreach(var v in m.Body.Variables)sb.Append("L:").Append(v.Index).Append(':').Append(TypeSig(v.VariableType)).Append(';');foreach(var i in m.Body.Instructions)sb.Append("I:").Append(i.OpCode.Code).Append(':').Append(OperandSig(i.Operand,m)).Append(';');foreach(var h in m.Body.ExceptionHandlers){int Idx(Instruction? x)=>x is null?-1:m.Body.Instructions.IndexOf(x);sb.Append("EH:").Append(h.HandlerType).Append(':').Append(Idx(h.TryStart)).Append(':').Append(Idx(h.TryEnd)).Append(':').Append(Idx(h.HandlerStart)).Append(':').Append(Idx(h.HandlerEnd)).Append(':').Append(Idx(h.FilterStart)).Append(':').Append(h.CatchType is null?"":TypeSig(h.CatchType)).Append(';');}return sb.ToString();}
static string FieldSemantic(FieldDefinition f){var attrs=string.Join(',',f.CustomAttributes.Select(a=>a.AttributeType.FullName).OrderBy(x=>x,StringComparer.Ordinal));var c=f.HasConstant?Convert.ToString(f.Constant,CultureInfo.InvariantCulture)??"<null>":"<none>";return$"{f.FullName}|{f.Attributes}|{TypeSig(f.FieldType)}|const={c}|ca={attrs}";}
static MethodReference Canonical(IEnumerable<MethodDefinition> methods,uint excludeToken,Func<MethodReference,bool> pred,string label){foreach(var m in methods){if(Raw(m)==excludeToken||!m.HasBody)continue;foreach(var i in m.Body.Instructions)if(IsCall(i)&&i.Operand is MethodReference mr&&pred(mr))return mr;}throw new InvalidDataException("canonical method ref missing: "+label);}
static FieldReference CanonicalField(IEnumerable<MethodDefinition> methods,uint excludeToken,Func<FieldReference,bool> pred,string label){foreach(var m in methods){if(Raw(m)==excludeToken||!m.HasBody)continue;foreach(var i in m.Body.Instructions)if(i.Operand is FieldReference fr&&pred(fr))return fr;}throw new InvalidDataException("canonical field ref missing: "+label);}

var input=Path.GetFullPath(args[0]);var output=Path.GetFullPath(args[1]);var sha=Sha(input);Console.WriteLine($"INPUT_SHA256 {sha}");if(sha!=ExpectedInputSha)throw new InvalidDataException("input SHA mismatch");
var beforeM=new Dictionary<uint,string>();var beforeF=new Dictionary<uint,string>();
using(var module=ModuleDefinition.ReadModule(input,new ReaderParameters{InMemory=true,ReadingMode=ReadingMode.Immediate})){
 var types=AllTypes(module.Types).ToList();var methods=types.SelectMany(t=>t.Methods).ToList();var fields=types.SelectMany(t=>t.Fields).ToList();if(methods.Count!=ExpectedMethods||fields.Count!=ExpectedFields)throw new InvalidDataException("metadata count drift");foreach(var m in methods)beforeM[Raw(m)]=MethodSemantic(m);foreach(var f in fields)beforeF[Raw(f)]=FieldSemantic(f);
 var target=types.Single(t=>t.FullName=="Plant_DetaiPage").Methods.Single(m=>m.Name=="Initialize"&&m.Parameters.Count==1&&m.Parameters[0].ParameterType.FullName=="Plant");if(Raw(target)!=TargetToken)throw new InvalidDataException("target token drift");
 var vecCtor=Canonical(methods,TargetToken,mr=>mr.DeclaringType.FullName=="UnityEngine.Vector3"&&mr.Name==".ctor"&&mr.Parameters.Count==3&&mr.Parameters.All(p=>p.ParameterType.FullName=="System.Single"),"Vector3::.ctor/3");
 var colorCtor=Canonical(methods,TargetToken,mr=>mr.DeclaringType.FullName=="UnityEngine.Color"&&mr.Name==".ctor"&&mr.Parameters.Count==4&&mr.Parameters.All(p=>p.ParameterType.FullName=="System.Single"),"Color::.ctor/4");
 var getPos=Canonical(methods,TargetToken,mr=>mr.DeclaringType.FullName=="UnityEngine.Transform"&&mr.Name=="get_position"&&mr.Parameters.Count==0,"Transform.get_position");
 var setPos=Canonical(methods,TargetToken,mr=>mr.DeclaringType.FullName=="UnityEngine.Transform"&&mr.Name=="set_position"&&mr.Parameters.Count==1,"Transform.set_position");
 var setColor=Canonical(methods,TargetToken,mr=>mr.DeclaringType.FullName=="UnityEngine.UI.Graphic"&&mr.Name=="set_color"&&mr.Parameters.Count==1,"Graphic.set_color");
 var vecX=CanonicalField(methods,TargetToken,fr=>fr.DeclaringType.FullName=="UnityEngine.Vector3"&&fr.Name=="x","Vector3.x");
 var vecY=CanonicalField(methods,TargetToken,fr=>fr.DeclaringType.FullName=="UnityEngine.Vector3"&&fr.Name=="y","Vector3.y");
 int replaced=0;
 foreach(var i in target.Body.Instructions){
   if(IsCall(i)&&i.Operand is MethodReference mr){
     MethodReference? r=null;
     if(mr.DeclaringType.FullName=="UnityEngine.Vector3"&&mr.Name==".ctor"&&mr.Parameters.Count==3)r=vecCtor;
     else if(mr.DeclaringType.FullName=="UnityEngine.Color"&&mr.Name==".ctor"&&mr.Parameters.Count==4)r=colorCtor;
     else if(mr.DeclaringType.FullName=="UnityEngine.Transform"&&mr.Name=="get_position")r=getPos;
     else if(mr.DeclaringType.FullName=="UnityEngine.Transform"&&mr.Name=="set_position")r=setPos;
     else if(mr.DeclaringType.FullName=="UnityEngine.UI.Graphic"&&mr.Name=="set_color")r=setColor;
     if(r is not null){i.Operand=r;replaced++;}
   } else if(i.Operand is FieldReference fr){
     if(fr.DeclaringType.FullName=="UnityEngine.Vector3"&&fr.Name=="x"){i.Operand=vecX;replaced++;}
     else if(fr.DeclaringType.FullName=="UnityEngine.Vector3"&&fr.Name=="y"){i.Operand=vecY;replaced++;}
   }
 }
 if(replaced!=7)throw new InvalidDataException($"unexpected canonical replacement count: {replaced}");
 Console.WriteLine("PATCH_PLANT_DETAIL_CANONICAL_REFS target_token=0x06000667 replaced_refs=7 semantics_changed=0");
 module.Write(output);
}
Console.WriteLine($"OUTPUT_SHA256 {Sha(output)}");
using(var module=ModuleDefinition.ReadModule(output,new ReaderParameters{InMemory=true,ReadingMode=ReadingMode.Immediate})){
 var types=AllTypes(module.Types).ToList();var methods=types.SelectMany(t=>t.Methods).ToList();var fields=types.SelectMany(t=>t.Fields).ToList();var target=types.Single(t=>t.FullName=="Plant_DetaiPage").Methods.Single(m=>m.Name=="Initialize"&&m.Parameters.Count==1&&m.Parameters[0].ParameterType.FullName=="Plant");
 if(target.Body.Variables.Count!=3)throw new InvalidDataException("target locals drift");
 int r4_30=target.Body.Instructions.Count(i=>i.OpCode==OpCodes.Ldc_R4&&i.Operand is float f&&Math.Abs(f-30f)<0.0001f);int r4_1=target.Body.Instructions.Count(i=>i.OpCode==OpCodes.Ldc_R4&&i.Operand is float f&&Math.Abs(f-1f)<0.0001f);if(r4_30!=1||r4_1<4)throw new InvalidDataException($"native constants drift 30={r4_30} one={r4_1}");
 int changed=0,untouched=0;foreach(var m in methods){if(beforeM.TryGetValue(Raw(m),out var old)&&old==MethodSemantic(m))untouched++;else{changed++;if(Raw(m)!=TargetToken)throw new InvalidDataException($"unexpected method drift 0x{Raw(m):X8} {m.FullName}");}}if(changed!=1||untouched!=2316)throw new InvalidDataException($"method isolation mismatch {untouched}/{changed}");foreach(var f in fields)if(!beforeF.TryGetValue(Raw(f),out var old)||old!=FieldSemantic(f))throw new InvalidDataException($"field drift 0x{Raw(f):X8} {f.FullName}");if(module.AssemblyReferences.Any(a=>a.Name=="System.Private.CoreLib"))throw new InvalidDataException("corelib introduced");
 Console.WriteLine("REOPEN_PLANT_DETAIL_CANONICAL_REFS_PASS token=0x06000667 locals=3 y_offset_30=1 white_color_components=4 corelib_refs=0");Console.WriteLine("SEMANTIC_ISOLATION_PASS untouched_methods=2316 changed_methods=1 target_token=0x06000667");Console.WriteLine("FIELD_METADATA_ISOLATION_PASS unchanged_fields=2802 changed_fields=0");
}
return 0;
