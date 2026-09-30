using System;using System.Linq;using System.Collections.Generic;using Mono.Cecil;
class DumpNative16{
static IEnumerable<TypeDefinition> Types(IEnumerable<TypeDefinition> ts){foreach(var t in ts){yield return t;foreach(var n in Types(t.NestedTypes))yield return n;}}
static void Main(string[] args){using(var a=AssemblyDefinition.ReadAssembly(args[0]))foreach(var t in Types(a.MainModule.Types))if(new[]{"VFXAnimationEvent","SwfAssocList`1","SwfList`1","ParticleState","BoardConfig"}.Contains(t.Name)){
Console.WriteLine("TYPE "+t.FullName+" "+t.MetadataToken);foreach(var f in t.Fields)Console.WriteLine("FIELD "+f.FullName+" "+f.MetadataToken+(f.HasConstant?" = "+f.Constant:""));
foreach(var m in t.Methods){Console.WriteLine("METHOD "+m.FullName+" "+m.MetadataToken);if((t.Name=="VFXAnimationEvent"&&m.Name=="SetSorting")||(t.Name=="SwfAssocList`1"&&m.Name=="Remove")||(t.Name=="SwfList`1"&&m.Name.Contains("Remove"))){foreach(var v in m.Body.Variables)Console.WriteLine("LOCAL "+v.Index+" "+v.VariableType);foreach(var i in m.Body.Instructions)Console.WriteLine(i);}}
}}}
