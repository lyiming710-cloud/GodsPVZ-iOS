using System;using System.Linq;using System.Collections.Generic;using Mono.Cecil;using Mono.Cecil.Cil;
class Rehost {
 static ModuleDefinition h;
 static TypeReference T(TypeReference t){
  var gi=t as GenericInstanceType;if(gi!=null){var x=new GenericInstanceType(T(gi.ElementType));foreach(var a in gi.GenericArguments)x.GenericArguments.Add(T(a));return x;}
  if(t is GenericParameter)return t;
  var ar=t as ArrayType;if(ar!=null)return new ArrayType(T(ar.ElementType),ar.Rank);
  var br=t as ByReferenceType;if(br!=null)return new ByReferenceType(T(br.ElementType));
  var local=h.Types.FirstOrDefault(x=>x.FullName==t.FullName);return local==null?h.ImportReference(t):local;
 }
 static MethodReference M(MethodReference r){
  var owner=T(r.DeclaringType);var td=owner as TypeDefinition;
  if(td!=null)return td.Methods.Single(x=>x.Name==r.Name&&x.Parameters.Count==r.Parameters.Count);
  var n=new MethodReference(r.Name,T(r.ReturnType),owner){HasThis=r.HasThis,ExplicitThis=r.ExplicitThis,CallingConvention=r.CallingConvention};foreach(var p in r.Parameters)n.Parameters.Add(new ParameterDefinition(T(p.ParameterType)));return n;
 }
 public static void Main(string[] a){
  using(var src=AssemblyDefinition.ReadAssembly(a[0]))using(var dst=AssemblyDefinition.ReadAssembly(a[1])){h=dst.MainModule;
   foreach(uint tok in new uint[]{0x060001a5,0x060001b2,0x060001e0,0x06000248,0x0600039f,0x06000267}){
    var s=(MethodDefinition)src.MainModule.LookupToken((int)tok);var d=(MethodDefinition)M(s);var b=new MethodBody(d){InitLocals=s.Body.InitLocals,MaxStackSize=s.Body.MaxStackSize};d.Body=b;
    foreach(var v in s.Body.Variables)b.Variables.Add(new VariableDefinition(T(v.VariableType)));
    var map=new Dictionary<Instruction,Instruction>();foreach(var i in s.Body.Instructions){var n=Instruction.Create(OpCodes.Nop);n.OpCode=i.OpCode;map[i]=n;b.Instructions.Add(n);}
    foreach(var i in s.Body.Instructions){object o=i.Operand;
     if(o is Instruction)o=map[(Instruction)o];else if(o is VariableDefinition)o=b.Variables[((VariableDefinition)o).Index];else if(o is ParameterDefinition)o=d.Parameters[((ParameterDefinition)o).Index];
     else if(o is MethodReference)o=M((MethodReference)o);else if(o is FieldReference){var f=(FieldReference)o;var td=T(f.DeclaringType) as TypeDefinition;o=td!=null?(FieldReference)td.Fields.Single(x=>x.Name==f.Name):new FieldReference(f.Name,T(f.FieldType),T(f.DeclaringType));}
     else if(o is TypeReference)o=T((TypeReference)o);map[i].Operand=o;
    }
    foreach(var e in s.Body.ExceptionHandlers)b.ExceptionHandlers.Add(new ExceptionHandler(e.HandlerType){TryStart=map[e.TryStart],TryEnd=map[e.TryEnd],HandlerStart=map[e.HandlerStart],HandlerEnd=map[e.HandlerEnd]});
   }dst.Write(a[2]);
  }
 }
}
