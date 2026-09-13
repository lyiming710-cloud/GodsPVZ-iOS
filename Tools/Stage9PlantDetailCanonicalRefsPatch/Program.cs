using Mono.Cecil;
using Mono.Cecil.Cil;
using System.Security.Cryptography;

if(args.Length!=2){Console.Error.WriteLine("Usage: Stage9PlantDetailCanonicalRefsPatch <input.dll> <output.dll>");return 2;}
const string ExpectedInputSha="20802aef2aa7cf5d430cf780d7511dd4ddd8afc7e97260903dc9977951cba739";
const uint TargetToken=0x06000667;
static IEnumerable<TypeDefinition> AllTypes(IEnumerable<TypeDefinition> roots){foreach(var t in roots){yield return t;foreach(var n in AllTypes(t.NestedTypes))yield return n;}}
static uint Raw(IMetadataTokenProvider p)=>p.MetadataToken.ToUInt32();
static string Sha(string p)=>Convert.ToHexString(SHA256.HashData(File.ReadAllBytes(p))).ToLowerInvariant();
static bool IsCall(Instruction i)=>i.OpCode==OpCodes.Call||i.OpCode==OpCodes.Callvirt||i.OpCode==OpCodes.Newobj;
static MethodReference Canonical(IEnumerable<MethodDefinition> methods,uint excludeToken,Func<MethodReference,bool> pred,string label){foreach(var m in methods){if(Raw(m)==excludeToken||!m.HasBody)continue;foreach(var i in m.Body.Instructions)if(IsCall(i)&&i.Operand is MethodReference mr&&pred(mr))return mr;}throw new InvalidDataException("canonical method ref missing: "+label);}
static FieldReference CanonicalField(IEnumerable<MethodDefinition> methods,uint excludeToken,Func<FieldReference,bool> pred,string label){foreach(var m in methods){if(Raw(m)==excludeToken||!m.HasBody)continue;foreach(var i in m.Body.Instructions)if(i.Operand is FieldReference fr&&pred(fr))return fr;}throw new InvalidDataException("canonical field ref missing: "+label);}

var input=Path.GetFullPath(args[0]);var output=Path.GetFullPath(args[1]);var sha=Sha(input);Console.WriteLine($"INPUT_SHA256 {sha}");if(sha!=ExpectedInputSha)throw new InvalidDataException("input SHA mismatch");
using(var module=ModuleDefinition.ReadModule(input,new ReaderParameters{InMemory=true,ReadingMode=ReadingMode.Immediate})){
 var types=AllTypes(module.Types).ToList();var methods=types.SelectMany(t=>t.Methods).ToList();var target=types.Single(t=>t.FullName=="Plant_DetaiPage").Methods.Single(m=>m.Name=="Initialize"&&m.Parameters.Count==1&&m.Parameters[0].ParameterType.FullName=="Plant");if(Raw(target)!=TargetToken)throw new InvalidDataException("target token drift");
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
 var types=AllTypes(module.Types).ToList();var target=types.Single(t=>t.FullName=="Plant_DetaiPage").Methods.Single(m=>m.Name=="Initialize"&&m.Parameters.Count==1&&m.Parameters[0].ParameterType.FullName=="Plant");
 if(target.Body.Variables.Count!=3)throw new InvalidDataException("target locals drift");
 int r4_30=target.Body.Instructions.Count(i=>i.OpCode==OpCodes.Ldc_R4&&i.Operand is float f&&Math.Abs(f-30f)<0.0001f);
 int r4_1=target.Body.Instructions.Count(i=>i.OpCode==OpCodes.Ldc_R4&&i.Operand is float f&&Math.Abs(f-1f)<0.0001f);
 if(r4_30!=1||r4_1<4)throw new InvalidDataException($"native constants drift 30={r4_30} one={r4_1}");
 if(module.AssemblyReferences.Any(a=>a.Name=="System.Private.CoreLib"))throw new InvalidDataException("corelib introduced");
 Console.WriteLine("REOPEN_PLANT_DETAIL_CANONICAL_REFS_PASS token=0x06000667 locals=3 y_offset_30=1 white_color_components=4 corelib_refs=0");
}
return 0;
