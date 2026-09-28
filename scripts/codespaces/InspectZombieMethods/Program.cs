using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using Mono.Cecil;
using Mono.Cecil.Cil;

internal static class Program
{
    static readonly HashSet<string> Names = new(StringComparer.Ordinal) {
        "PreviousPosition", "Start_PreviousPosition", "Update_Move", "Update_PreviousPosition",
        "ArmBroken", "Ashe", "CheckZombieWin", "CreateStartPrePath"
    };
    static IEnumerable<TypeDefinition> Types(TypeDefinition t){yield return t;foreach(var n in t.NestedTypes)foreach(var x in Types(n))yield return x;}
    static IEnumerable<TypeDefinition> Types(ModuleDefinition m){foreach(var t in m.Types)foreach(var x in Types(t))yield return x;}
    static string Operand(Instruction i)=>i.Operand switch{
        null=>"", Instruction x=>$"IL_{x.Offset:X4}", Instruction[] xs=>string.Join(",",xs.Select(x=>$"IL_{x.Offset:X4}")),
        VariableDefinition v=>$"V_{v.Index}:{v.VariableType.FullName}", ParameterDefinition p=>$"P_{p.Index}:{p.ParameterType.FullName}",
        TypeReference tr=>$"{tr.FullName} [0x{tr.MetadataToken.ToUInt32():X8}]", MemberReference mr=>$"{mr.FullName} [0x{mr.MetadataToken.ToUInt32():X8}]",
        _=>i.Operand?.ToString()??""};
    static void Main(string[] args)
    {
        if(args.Length!=1) throw new ArgumentException("usage: InspectZombieMethods <dll>");
        using var asm=AssemblyDefinition.ReadAssembly(Path.GetFullPath(args[0]));
        var m=asm.MainModule;
        var type=Types(m).Single(x=>x.FullName=="Zombie");
        Console.WriteLine($"ASSEMBLY={asm.Name.Name}");
        Console.WriteLine($"MVID={m.Mvid}");
        Console.WriteLine($"TYPE token=0x{type.MetadataToken.ToUInt32():X8} {type.FullName}");
        foreach(var f in type.Fields) Console.WriteLine($"FIELD token=0x{f.MetadataToken.ToUInt32():X8} name={f.Name} type={f.FieldType.FullName}");
        foreach(var md in type.Methods.Where(x=>Names.Contains(x.Name)).OrderBy(x=>x.MetadataToken.RID)) {
            Console.WriteLine($"METHOD_BEGIN token=0x{md.MetadataToken.ToUInt32():X8} rid={md.MetadataToken.RID} {md.FullName} code_size={(md.HasBody?md.Body.CodeSize:0)} il={(md.HasBody?md.Body.Instructions.Count:0)} locals={(md.HasBody?md.Body.Variables.Count:0)} eh={(md.HasBody?md.Body.ExceptionHandlers.Count:0)}");
            if(md.HasBody){
                foreach(var v in md.Body.Variables) Console.WriteLine($"LOCAL V_{v.Index} {v.VariableType.FullName}");
                foreach(var i in md.Body.Instructions) Console.WriteLine($"IL_{i.Offset:X4}: {i.OpCode} {Operand(i)}");
                foreach(var eh in md.Body.ExceptionHandlers) Console.WriteLine($"EH {eh.HandlerType} try=IL_{eh.TryStart?.Offset:X4}-IL_{eh.TryEnd?.Offset:X4} handler=IL_{eh.HandlerStart?.Offset:X4}-IL_{eh.HandlerEnd?.Offset:X4} catch={eh.CatchType?.FullName}");
            }
            Console.WriteLine("METHOD_END");
        }
    }
}
