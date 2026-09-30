using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Reflection.Metadata;
using System.Reflection.Metadata.Ecma335;
using System.Reflection.PortableExecutable;
using System.Collections.Generic;
using System.Text.Json;
using Mono.Cecil;
using Mono.Cecil.Cil;
using CM=Mono.Cecil.Cil.MethodBody;
using TypeDefinition=Mono.Cecil.TypeDefinition;
using TypeReference=Mono.Cecil.TypeReference;
using MethodDefinition=Mono.Cecil.MethodDefinition;
using AssemblyDefinition=Mono.Cecil.AssemblyDefinition;
class Inspector {
 static IEnumerable<TypeDefinition> Types(TypeDefinition t) {yield return t; foreach(var n in t.NestedTypes)foreach(var x in Types(n))yield return x;}
 static string Scope(TypeReference t)=> t.Scope?.ToString()??"";
 static string TR(TypeReference t)=>t==null?null:t.FullName+" @"+Scope(t);
 static void Main(string[] args) {
  if(args[0]=="probe"){Probe(args[1],args[2]);return;}
  using var asm=AssemblyDefinition.ReadAssembly(args[0]);var mod=asm.MainModule;
  using var stream=File.OpenRead(args[0]);using var pe=new PEReader(stream);var md=pe.GetMetadataReader();
  var docs=new List<object>();var defects=new List<string>();
  foreach(var t in mod.Types.SelectMany(Types))foreach(var m in t.Methods){
   var tok=m.MetadataToken.ToInt32();var sm=md.GetMethodDefinition(MetadataTokens.MethodDefinitionHandle(tok & 0xffffff));
   byte[] raw=Array.Empty<byte>();if(sm.RelativeVirtualAddress!=0)raw=pe.GetMethodBody(sm.RelativeVirtualAddress).GetILBytes();
   object body=null;
   if(m.HasBody){var b=m.Body; var ix=b.Instructions.Select((i,k)=>(i,k)).ToDictionary(x=>x.i,x=>x.k);
    int Id(Instruction i){if(i==null)return -1;if(!ix.TryGetValue(i,out var k)){defects.Add($"{tok:X8} dangling instruction {i.Offset:X4}");return -2;}return k;}
    object Operand(object a)=>a switch{
     Instruction i=>new{branch=Id(i)},Instruction[] ii=>new{branches=ii.Select(Id).ToArray()},
     VariableDefinition v=>new{local=v.Index},ParameterDefinition p=>new{arg=p.Index},
     MethodReference r=>new{method=r.FullName,scope=Scope(r.DeclaringType),hasThis=r.HasThis,explicitThis=r.ExplicitThis,convention=r.CallingConvention.ToString()},
     FieldReference f=>new{field=f.FullName,scope=Scope(f.DeclaringType)},TypeReference r=>new{type=TR(r)},_=>a};
    body=new{init=b.InitLocals,max=b.MaxStackSize,locals=b.Variables.Select(v=>TR(v.VariableType)).ToArray(),instructions=b.Instructions.Select(i=>new{op=i.OpCode.Name,operand=Operand(i.Operand)}).ToArray(),eh=b.ExceptionHandlers.Select(h=>new{kind=h.HandlerType.ToString(),ts=Id(h.TryStart),te=Id(h.TryEnd),hs=Id(h.HandlerStart),he=Id(h.HandlerEnd),filter=Id(h.FilterStart),type=TR(h.CatchType)}).ToArray()};
   }
   docs.Add(new{token=$"0x{tok:X8}",name=m.FullName,attributes=(uint)m.Attributes,impl=(uint)m.ImplAttributes,signature=Convert.ToHexString(md.GetBlobBytes(sm.Signature)),body,rawIL=Convert.ToHexString(raw??Array.Empty<byte>())});
  }
  var output=new{assembly=asm.Name.FullName,module=mod.Name,mvid=mod.Mvid,tableCounts=Enumerable.Range(0,45).ToDictionary(x=>((TableIndex)x).ToString(),x=>md.GetTableRowCount((TableIndex)x)),types=mod.Types.SelectMany(Types).Select(t=>new{token=t.MetadataToken.ToString(),name=t.FullName,attributes=(uint)t.Attributes,baseType=TR(t.BaseType),packing=t.PackingSize,size=t.ClassSize,interfaces=t.Interfaces.Select(x=>TR(x.InterfaceType)).ToArray(),fields=t.Fields.Select(f=>new{token=f.MetadataToken.ToString(),name=f.FullName,attributes=(uint)f.Attributes,offset=f.Offset,type=TR(f.FieldType),constant=f.HasConstant?f.Constant:null}).ToArray()}).ToArray(),methods=docs,defects};
  File.WriteAllText(args[1],JsonSerializer.Serialize(output,new JsonSerializerOptions{WriteIndented=true,NumberHandling=System.Text.Json.Serialization.JsonNumberHandling.AllowNamedFloatingPointLiterals}));
 }
 static void Probe(string patcher,string output){
  var a=System.Reflection.Assembly.LoadFrom(patcher);var p=a.GetType("Program");var swap=p.GetMethod("Swap",BindingFlags.NonPublic|BindingFlags.Static);
  using var asm=AssemblyDefinition.CreateAssembly(new AssemblyNameDefinition("Probe",new Version(1,0)),"Probe",ModuleKind.Dll);var md=asm.MainModule;
  var td=new TypeDefinition("","Probe",Mono.Cecil.TypeAttributes.Public,md.TypeSystem.Object);md.Types.Add(td);
  var m=new MethodDefinition("Run",Mono.Cecil.MethodAttributes.Public|Mono.Cecil.MethodAttributes.Static,md.TypeSystem.Void);td.Methods.Add(m);m.Body=new CM(m);var il=m.Body.GetILProcessor();
  var old=Instruction.Create(OpCodes.Ldc_I4_0);var newer=Instruction.Create(OpCodes.Ldnull);var jump=Instruction.Create(OpCodes.Br,old);var sw=Instruction.Create(OpCodes.Switch,new[]{old});var ret=Instruction.Create(OpCodes.Ret);
  foreach(var i in new[]{jump,sw,old,Instruction.Create(OpCodes.Pop),ret})il.Append(i);
  var eh=new ExceptionHandler(ExceptionHandlerType.Finally){TryStart=old,TryEnd=ret,HandlerStart=ret,HandlerEnd=null};m.Body.ExceptionHandlers.Add(eh);
  var argv=new object[]{il,m.Body,old,newer,null,null};var rc=swap.Invoke(null,argv);
  File.WriteAllText(output,JsonSerializer.Serialize(new{swapReturn=rc,oldRemoved=!m.Body.Instructions.Contains(old),singleBranchFixed=ReferenceEquals(jump.Operand,newer),switchFixed=ReferenceEquals(((Instruction[])sw.Operand)[0],newer),ehFixed=ReferenceEquals(eh.TryStart,newer)},new JsonSerializerOptions{WriteIndented=true}));
 }
}
