using System;
using System.IO;
using System.Linq;
using Mono.Cecil;

internal static class Program
{
    static System.Collections.Generic.IEnumerable<TypeDefinition> Types(TypeDefinition t){yield return t;foreach(var n in t.NestedTypes)foreach(var x in Types(n))yield return x;}
    static System.Collections.Generic.IEnumerable<TypeDefinition> Types(ModuleDefinition m){foreach(var t in m.Types)foreach(var x in Types(t))yield return x;}
    static void Main(string[] args)
    {
        if(args.Length!=1) throw new ArgumentException("usage: InspectNative10 <native9.dll>");
        using var a=AssemblyDefinition.ReadAssembly(Path.GetFullPath(args[0])); var m=a.MainModule;
        var p=Types(m).Single(t=>t.Namespace=="TMPro.Examples"&&t.Name=="WarpTextExample");
        var it=p.NestedTypes.Single(t=>t.Name=="<WarpText>d__8");
        var move=it.Methods.Single(x=>x.Name=="MoveNext"&&x.Parameters.Count==0);
        Console.WriteLine($"MVID={m.Mvid}");
        Console.WriteLine($"PARENT={p.FullName} token=0x{p.MetadataToken.ToUInt32():X8}");
        foreach(var f in p.Fields) Console.WriteLine($"PARENT_FIELD token=0x{f.MetadataToken.ToUInt32():X8} name={f.Name} type={f.FieldType.FullName}");
        foreach(var md in p.Methods) Console.WriteLine($"PARENT_METHOD token=0x{md.MetadataToken.ToUInt32():X8} {md.FullName}");
        Console.WriteLine($"ITERATOR={it.FullName} token=0x{it.MetadataToken.ToUInt32():X8}");
        foreach(var f in it.Fields) Console.WriteLine($"ITER_FIELD token=0x{f.MetadataToken.ToUInt32():X8} name={f.Name} type={f.FieldType.FullName}");
        foreach(var md in it.Methods) Console.WriteLine($"ITER_METHOD token=0x{md.MetadataToken.ToUInt32():X8} {md.FullName} body={(md.HasBody?md.Body.CodeSize:0)}");
        Console.WriteLine($"TARGET token=0x{move.MetadataToken.ToUInt32():X8} full={move.FullName}");
        string[] decl={"AnimationCurve","Keyframe","TMP_Text","TMP_TextInfo","TMP_CharacterInfo","TMP_MeshInfo","Bounds","Vector3","Mathf","Quaternion","Matrix4x4","WaitForSeconds"};
        foreach(var r in m.GetMemberReferences().OrderBy(x=>x.MetadataToken.ToUInt32()))
        {
            if(!decl.Contains(r.DeclaringType?.Name)) continue;
            if(r is MethodReference mr) Console.WriteLine($"METHODREF token=0x{mr.MetadataToken.ToUInt32():X8} {mr.FullName} scope={mr.DeclaringType.Scope?.Name}");
            else if(r is FieldReference fr) Console.WriteLine($"FIELDREF token=0x{fr.MetadataToken.ToUInt32():X8} {fr.FullName} scope={fr.DeclaringType.Scope?.Name}");
        }
        Console.WriteLine("INSPECT_NATIVE10_PASS");
    }
}
