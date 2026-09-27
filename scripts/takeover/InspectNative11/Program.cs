using System;
using System.IO;
using System.Linq;
using Mono.Cecil;
internal static class Program{
 static System.Collections.Generic.IEnumerable<TypeDefinition> Types(TypeDefinition t){yield return t;foreach(var n in t.NestedTypes)foreach(var x in Types(n))yield return x;}
 static System.Collections.Generic.IEnumerable<TypeDefinition> Types(ModuleDefinition m){foreach(var t in m.Types)foreach(var x in Types(t))yield return x;}
 static void Main(string[] a){if(a.Length!=1)throw new ArgumentException("usage: InspectNative11 <dll>");using var asm=AssemblyDefinition.ReadAssembly(Path.GetFullPath(a[0]));var m=asm.MainModule;var t=Types(m).Single(x=>x.Namespace=="TMPro.Examples"&&x.Name=="TMP_TextSelector_A");
 Console.WriteLine($"MVID={m.Mvid}");Console.WriteLine($"TYPE token=0x{t.MetadataToken.ToUInt32():X8} {t.FullName}");
 foreach(var f in t.Fields)Console.WriteLine($"FIELD token=0x{f.MetadataToken.ToUInt32():X8} name={f.Name} type={f.FieldType.FullName}");
 foreach(var md in t.Methods)Console.WriteLine($"METHOD token=0x{md.MetadataToken.ToUInt32():X8} {md.FullName} body={(md.HasBody?md.Body.CodeSize:0)}");
 var target=t.Methods.Single(x=>x.Name=="LateUpdate"&&x.Parameters.Count==0);Console.WriteLine($"TARGET token=0x{target.MetadataToken.ToUInt32():X8} {target.FullName}");
 string[] names={"TMP_TextUtilities","TMP_Text","TMP_TextInfo","TMP_CharacterInfo","TMP_MeshInfo","TMP_WordInfo","TMP_LinkInfo","Input","Camera","Random","Mesh","Transform","RectTransformUtility","Color32"};
 foreach(var r in m.GetMemberReferences().OrderBy(x=>x.MetadataToken.ToUInt32())){if(!names.Contains(r.DeclaringType?.Name))continue;if(r is MethodReference mr)Console.WriteLine($"METHODREF token=0x{mr.MetadataToken.ToUInt32():X8} {mr.FullName} scope={mr.DeclaringType.Scope?.Name}");else if(r is FieldReference fr)Console.WriteLine($"FIELDREF token=0x{fr.MetadataToken.ToUInt32():X8} {fr.FullName} scope={fr.DeclaringType.Scope?.Name}");}
 Console.WriteLine("INSPECT_NATIVE11_PASS");}}
