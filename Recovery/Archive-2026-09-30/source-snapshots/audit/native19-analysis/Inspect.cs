using System;using System.Linq;using System.IO;using System.Collections.Generic;using Mono.Cecil;using Mono.Cecil.Cil;
class Inspect{
static string Operand(MethodDefinition m,object o){if(o==null)return "";if(o is Instruction)return "@"+m.Body.Instructions.IndexOf((Instruction)o);if(o is Instruction[])return string.Join(",",((Instruction[])o).Select(x=>"@"+m.Body.Instructions.IndexOf(x)).ToArray());if(o is VariableDefinition){var v=(VariableDefinition)o;return "V"+v.Index+":"+v.VariableType.FullName;}if(o is ParameterDefinition){var v=(ParameterDefinition)o;return "P"+v.Index+":"+v.ParameterType.FullName;}if(o is MemberReference){var r=(MemberReference)o;return r.FullName+"@"+(r.DeclaringType==null?"":r.DeclaringType.Scope.Name);}return o.ToString();}
static string Hash(string s){using(var h=System.Security.Cryptography.SHA256.Create())return BitConverter.ToString(h.ComputeHash(System.Text.Encoding.UTF8.GetBytes(s))).Replace("-","");}
static IEnumerable<TypeDefinition> Types(IEnumerable<TypeDefinition> ts){foreach(var t in ts){yield return t;foreach(var n in Types(t.NestedTypes))yield return n;}}
static void Main(string[] args){using(var a=AssemblyDefinition.ReadAssembly(args[0])){
using(var map=new StreamWriter(args[1]+".map.tsv"))using(var il=new StreamWriter(args[1]+".il.txt"))using(var fp=new StreamWriter(args[1]+".normalized.tsv")){
foreach(var t in Types(a.MainModule.Types))foreach(var m in t.Methods){map.WriteLine(m.MetadataToken.ToInt32()+"\t"+m.RVA+"\t"+m.FullName);if(!m.HasBody)continue;
var s=m.FullName+"|"+m.Attributes+"|"+m.ImplAttributes+"|"+m.Body.InitLocals+"|"+m.Body.MaxStackSize+"|"+string.Join(";",m.Body.Variables.Select(v=>v.VariableType.FullName).ToArray());
foreach(var i in m.Body.Instructions)s+="|"+i.OpCode.Name+":"+Operand(m,i.Operand);
foreach(var eh in m.Body.ExceptionHandlers)s+="|EH:"+eh.HandlerType+":"+Operand(m,eh.TryStart)+":"+Operand(m,eh.TryEnd)+":"+Operand(m,eh.HandlerStart)+":"+Operand(m,eh.HandlerEnd)+":"+Operand(m,eh.FilterStart)+":"+eh.CatchType;
fp.WriteLine(m.MetadataToken.ToInt32()+"\t"+Hash(s)+"\t"+m.FullName);
foreach(var i in m.Body.Instructions){if(i.Operand is Instruction && !m.Body.Instructions.Contains((Instruction)i.Operand))Console.WriteLine("DANGLING "+m+" "+i);}
if(t.Name!="Zombie"&&t.Name!="ICEUIController"&&t.Name!="Project")continue;
il.WriteLine("METHOD "+m.FullName+" "+m.MetadataToken);
foreach(var v in m.Body.Variables)il.WriteLine("LOCAL "+v.Index+" "+v.VariableType);
foreach(var i in m.Body.Instructions)il.WriteLine(i);
}
}}}}
