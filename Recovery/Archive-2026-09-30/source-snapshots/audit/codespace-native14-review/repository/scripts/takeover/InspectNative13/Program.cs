using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using Mono.Cecil;
using Mono.Cecil.Cil;
internal static class Program{
 static IEnumerable<TypeDefinition> Types(TypeDefinition t){yield return t;foreach(var n in t.NestedTypes)foreach(var x in Types(n))yield return x;}
 static IEnumerable<TypeDefinition> Types(ModuleDefinition m){foreach(var t in m.Types)foreach(var x in Types(t))yield return x;}
 static void Main(string[] a){if(a.Length!=1)throw new ArgumentException("usage: InspectNative13 <dll>");using var asm=AssemblyDefinition.ReadAssembly(Path.GetFullPath(a[0]));var m=asm.MainModule;var t=Types(m).Single(x=>x.Namespace=="TMPro.Examples"&&x.Name=="TMP_TextSelector_B");var target=t.Methods.Single(x=>x.Name=="LateUpdate"&&x.Parameters.Count==0);
 Console.WriteLine($"MVID={m.Mvid}");Console.WriteLine($"TYPE token=0x{t.MetadataToken.ToUInt32():X8} {t.FullName}");foreach(var f in t.Fields)Console.WriteLine($"FIELD token=0x{f.MetadataToken.ToUInt32():X8} name={f.Name} type={f.FieldType.FullName}");foreach(var md in t.Methods)Console.WriteLine($"METHOD token=0x{md.MetadataToken.ToUInt32():X8} {md.FullName} body={(md.HasBody?md.Body.CodeSize:0)}");Console.WriteLine($"TARGET token=0x{target.MetadataToken.ToUInt32():X8} {target.FullName} locals={target.Body.Variables.Count} il={target.Body.Instructions.Count}");foreach(var v in target.Body.Variables)Console.WriteLine($"LOCAL index={v.Index} type={v.VariableType.FullName}");
 var refs=new SortedDictionary<uint,MemberReference>();foreach(var i in target.Body.Instructions)if(i.Operand is MemberReference r)refs[r.MetadataToken.ToUInt32()]=r;foreach(var kv in refs){var r=kv.Value;if(r is MethodReference mr)Console.WriteLine($"USED_METHODREF token=0x{kv.Key:X8} {mr.FullName} scope={mr.DeclaringType.Scope?.Name}");else if(r is FieldReference fr)Console.WriteLine($"USED_FIELDREF token=0x{kv.Key:X8} {fr.FullName} scope={fr.DeclaringType.Scope?.Name}");else Console.WriteLine($"USED_REF token=0x{kv.Key:X8} {r.FullName}");}
 foreach(var i in target.Body.Instructions)Console.WriteLine($"IL {i.Offset:X4} {i.OpCode} {i.Operand}");Console.WriteLine("INSPECT_NATIVE13_PASS");}}
