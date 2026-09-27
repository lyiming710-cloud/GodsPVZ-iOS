using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using Mono.Cecil;
using Mono.Cecil.Cil;

internal static class Program
{
    static IEnumerable<TypeDefinition> Types(TypeDefinition t){yield return t;foreach(var n in t.NestedTypes)foreach(var x in Types(n))yield return x;}
    static IEnumerable<TypeDefinition> Types(ModuleDefinition m){foreach(var t in m.Types)foreach(var x in Types(t))yield return x;}
    static string Operand(Instruction i)=>i.Operand switch{
        null=>"", Instruction x=>$"IL_{x.Offset:X4}", Instruction[] xs=>string.Join(",",xs.Select(x=>$"IL_{x.Offset:X4}")),
        MemberReference mr=>$"{mr.FullName} [0x{mr.MetadataToken.ToUInt32():X8}]", VariableDefinition v=>$"V_{v.Index}:{v.VariableType.FullName}",
        ParameterDefinition p=>$"P_{p.Index}:{p.ParameterType.FullName}", TypeReference tr=>$"{tr.FullName} [0x{tr.MetadataToken.ToUInt32():X8}]", _=>i.Operand.ToString()};
    static void Main(string[] args)
    {
        if(args.Length!=1)throw new ArgumentException("usage: InspectNative14 <dll>");
        using var asm=AssemblyDefinition.ReadAssembly(Path.GetFullPath(args[0])); var m=asm.MainModule;
        var type=Types(m).Single(x=>x.Name=="ElementManager");
        Console.WriteLine($"MVID={m.Mvid}"); Console.WriteLine($"TYPE token=0x{type.MetadataToken.ToUInt32():X8} {type.FullName}");
        foreach(var f in type.Fields) Console.WriteLine($"FIELD token=0x{f.MetadataToken.ToUInt32():X8} name={f.Name} type={f.FieldType.FullName}");
        foreach(var md in type.Methods) Console.WriteLine($"METHOD token=0x{md.MetadataToken.ToUInt32():X8} {md.FullName} body={(md.HasBody?md.Body.CodeSize:0)} gp={md.GenericParameters.Count}");
        var target=type.Methods.Single(x=>x.Name=="CreateNewElements"&&x.GenericParameters.Count==1);
        Console.WriteLine($"TARGET token=0x{target.MetadataToken.ToUInt32():X8} rid={target.MetadataToken.RID} {target.FullName} code_size={target.Body.CodeSize} il={target.Body.Instructions.Count} locals={target.Body.Variables.Count} eh={target.Body.ExceptionHandlers.Count}");
        foreach(var gp in target.GenericParameters){Console.WriteLine($"GENERIC {gp.Name} attrs={gp.Attributes}");foreach(var c in gp.Constraints)Console.WriteLine($"CONSTRAINT {c.ConstraintType.FullName}");}
        foreach(var v in target.Body.Variables)Console.WriteLine($"LOCAL V_{v.Index} {v.VariableType.FullName}");
        Console.WriteLine("TARGET_IL_BEGIN"); foreach(var i in target.Body.Instructions)Console.WriteLine($"IL_{i.Offset:X4}: {i.OpCode} {Operand(i)}"); Console.WriteLine("TARGET_IL_END");
        Console.WriteLine("RELATED_GENERIC_CALLS_BEGIN");
        foreach(var md in Types(m).SelectMany(x=>x.Methods).Where(x=>x.HasBody))
        foreach(var i in md.Body.Instructions)
            if(i.Operand is GenericInstanceMethod gim && gim.ElementMethod.Resolve()?.MetadataToken.ToUInt32()==target.MetadataToken.ToUInt32())
                Console.WriteLine($"CALLER token=0x{md.MetadataToken.ToUInt32():X8} {md.FullName} -> {gim.FullName} args={string.Join(",",gim.GenericArguments.Select(x=>x.FullName))}");
        Console.WriteLine("RELATED_GENERIC_CALLS_END");
        Console.WriteLine("INSPECT_NATIVE14_PASS");
    }
}
