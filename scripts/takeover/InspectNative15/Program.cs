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
        VariableDefinition v=>$"V_{v.Index}:{v.VariableType.FullName}", ParameterDefinition p=>$"P_{p.Index}:{p.ParameterType.FullName}",
        TypeReference tr=>$"{tr.FullName} [0x{tr.MetadataToken.ToUInt32():X8}]", MemberReference mr=>$"{mr.FullName} [0x{mr.MetadataToken.ToUInt32():X8}]",
        _=>i.Operand.ToString()};
    static void DumpType(TypeDefinition t)
    {
        Console.WriteLine($"DETAIL_TYPE token=0x{t.MetadataToken.ToUInt32():X8} {t.FullName} enum={t.IsEnum} valueType={t.IsValueType}");
        foreach(var f in t.Fields)Console.WriteLine($"DETAIL_FIELD token=0x{f.MetadataToken.ToUInt32():X8} name={f.Name} type={f.FieldType.FullName} static={f.IsStatic}");
        foreach(var md in t.Methods)Console.WriteLine($"DETAIL_METHOD token=0x{md.MetadataToken.ToUInt32():X8} {md.FullName} body={(md.HasBody?md.Body.CodeSize:0)} gp={md.GenericParameters.Count}");
    }
    static void Main(string[] args)
    {
        if(args.Length!=1)throw new ArgumentException("usage: InspectNative15 <dll>");
        using var asm=AssemblyDefinition.ReadAssembly(Path.GetFullPath(args[0]));var m=asm.MainModule;
        var map=Types(m).Single(x=>x.Name=="Map"&&x.Namespace=="");
        var target=map.Methods.Single(x=>x.Name=="RandomGet_Grid_TestPlace"&&x.GenericParameters.Count==1&&x.Parameters.Count==4);
        Console.WriteLine($"MVID={m.Mvid}");
        Console.WriteLine($"MAP token=0x{map.MetadataToken.ToUInt32():X8}");
        foreach(var f in map.Fields)Console.WriteLine($"FIELD token=0x{f.MetadataToken.ToUInt32():X8} name={f.Name} type={f.FieldType.FullName}");
        foreach(var md in map.Methods)Console.WriteLine($"METHOD token=0x{md.MetadataToken.ToUInt32():X8} {md.FullName} body={(md.HasBody?md.Body.CodeSize:0)} gp={md.GenericParameters.Count}");
        Console.WriteLine($"TARGET token=0x{target.MetadataToken.ToUInt32():X8} rid={target.MetadataToken.RID} {target.FullName} code_size={target.Body.CodeSize} il={target.Body.Instructions.Count} locals={target.Body.Variables.Count} eh={target.Body.ExceptionHandlers.Count}");
        foreach(var gp in target.GenericParameters){Console.WriteLine($"GENERIC name={gp.Name} attrs={gp.Attributes}");foreach(var c in gp.Constraints)Console.WriteLine($"CONSTRAINT {c.ConstraintType.FullName}");}
        foreach(var p in target.Parameters)Console.WriteLine($"PARAM index={p.Index} name={p.Name} type={p.ParameterType.FullName}");
        foreach(var v in target.Body.Variables)Console.WriteLine($"LOCAL V_{v.Index} {v.VariableType.FullName}");
        Console.WriteLine("TARGET_IL_BEGIN");foreach(var i in target.Body.Instructions)Console.WriteLine($"IL_{i.Offset:X4}: {i.OpCode} {Operand(i)}");Console.WriteLine("TARGET_IL_END");
        Console.WriteLine("RELATED_GENERIC_CALLS_BEGIN");
        foreach(var md in Types(m).SelectMany(x=>x.Methods).Where(x=>x.HasBody))
        foreach(var i in md.Body.Instructions)
            if(i.Operand is GenericInstanceMethod gim && gim.ElementMethod.Name==target.Name && gim.ElementMethod.DeclaringType.FullName==map.FullName && gim.ElementMethod.Parameters.Count==4 && gim.ElementMethod.GenericParameters.Count==1)
                Console.WriteLine($"CALLER token=0x{md.MetadataToken.ToUInt32():X8} {md.FullName} -> {gim.FullName} args={string.Join(",",gim.GenericArguments.Select(x=>x.FullName))}");
        Console.WriteLine("RELATED_GENERIC_CALLS_END");
        foreach(var name in new[]{"Grid","Plant","Zombie","Device","Map"})foreach(var t in Types(m).Where(x=>x.Name==name))DumpType(t);
        Console.WriteLine("RELEVANT_MEMBERREFS_BEGIN");
        foreach(var r in m.GetMemberReferences().OrderBy(x=>x.MetadataToken.ToUInt32())){
            var s=r.FullName;
            if(s.Contains("List`1<Grid>")||s.Contains("UnityEngine.Random")||s.Contains("Grid::")||s.Contains("System.Object::")||s.Contains("System.Type::"))
                Console.WriteLine($"MEMBERREF token=0x{r.MetadataToken.ToUInt32():X8} {r.FullName} scope={r.DeclaringType?.Scope?.Name}");
        }
        Console.WriteLine("RELEVANT_MEMBERREFS_END");
        Console.WriteLine("INSPECT_NATIVE15_PASS");
    }
}
