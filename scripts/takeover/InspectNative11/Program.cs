using System;
using System.IO;
using System.Linq;
using Mono.Cecil;
using Mono.Cecil.Cil;

internal static class Program
{
    static System.Collections.Generic.IEnumerable<TypeDefinition> Types(TypeDefinition t){yield return t;foreach(var n in t.NestedTypes)foreach(var x in Types(n))yield return x;}
    static System.Collections.Generic.IEnumerable<TypeDefinition> Types(ModuleDefinition m){foreach(var t in m.Types)foreach(var x in Types(t))yield return x;}
    static string Operand(MethodDefinition m, Instruction i) => i.Operand switch {
        null => "",
        Instruction x => $"IL_{x.Offset:X4}",
        Instruction[] xs => string.Join(",",xs.Select(x=>$"IL_{x.Offset:X4}")),
        MemberReference mr => $"{mr.FullName} [0x{mr.MetadataToken.ToUInt32():X8}]",
        VariableDefinition v => $"V_{v.Index}:{v.VariableType.FullName}",
        ParameterDefinition p => $"P_{p.Index}:{p.ParameterType.FullName}",
        _ => i.Operand.ToString()
    };
    static void Main(string[] a)
    {
        if(a.Length!=1)throw new ArgumentException("usage: InspectNative11 <dll>");
        using var asm=AssemblyDefinition.ReadAssembly(Path.GetFullPath(a[0]));var m=asm.MainModule;
        var t=Types(m).Single(x=>x.Namespace=="TMPro.Examples"&&x.Name=="TMP_TextSelector_A");
        Console.WriteLine($"MVID={m.Mvid}");Console.WriteLine($"TYPE token=0x{t.MetadataToken.ToUInt32():X8} {t.FullName}");
        foreach(var f in t.Fields)Console.WriteLine($"FIELD token=0x{f.MetadataToken.ToUInt32():X8} name={f.Name} type={f.FieldType.FullName}");
        foreach(var md in t.Methods)Console.WriteLine($"METHOD token=0x{md.MetadataToken.ToUInt32():X8} {md.FullName} body={(md.HasBody?md.Body.CodeSize:0)}");
        var target=t.Methods.Single(x=>x.Name=="LateUpdate"&&x.Parameters.Count==0);
        Console.WriteLine($"TARGET token=0x{target.MetadataToken.ToUInt32():X8} {target.FullName} il={target.Body.Instructions.Count} locals={target.Body.Variables.Count} eh={target.Body.ExceptionHandlers.Count}");
        foreach(var v in target.Body.Variables) Console.WriteLine($"LOCAL V_{v.Index} {v.VariableType.FullName}");
        Console.WriteLine("TARGET_IL_BEGIN");
        foreach(var i in target.Body.Instructions) Console.WriteLine($"IL_{i.Offset:X4}: {i.OpCode} {Operand(target,i)}");
        Console.WriteLine("TARGET_IL_END");

        string[] interesting={"firstCharacterIndex","characterCount","bottomLeft","get_mesh","get_transform","op_Equality","GetKeyInt","Range","FindIntersectingCharacter","FindIntersectingWord","FindIntersectingLink","IsIntersectingRectTransform","ScreenPointToWorldPointInRectangle","TransformPoint","WorldToScreenPoint","get_textInfo","wordInfo","characterInfo","meshInfo","colors32","vertexIndex","materialReferenceIndex","GetLinkID","set_colors32","get_rectTransform"};
        Console.WriteLine("REQUIRED_REFS_BEGIN");
        foreach(var r in m.GetMemberReferences().OrderBy(x=>x.MetadataToken.ToUInt32()))
            if(interesting.Contains(r.Name)) Console.WriteLine($"REF token=0x{r.MetadataToken.ToUInt32():X8} kind={r.GetType().Name} {r.FullName} scope={r.DeclaringType?.Scope?.Name}");
        Console.WriteLine("REQUIRED_REFS_END");
        Console.WriteLine("INSPECT_NATIVE11_PASS");
    }
}
