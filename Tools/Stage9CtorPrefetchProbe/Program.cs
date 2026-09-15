using Mono.Cecil;
using Mono.Cecil.Cil;
using System.Security.Cryptography;

if (args.Length < 2)
{
    Console.Error.WriteLine("usage: Stage9CtorPrefetchProbe <dll> <TypeName> [TypeName...]");
    return 2;
}
var input=Path.GetFullPath(args[0]);
Console.WriteLine($"INPUT_SHA256 {Convert.ToHexString(SHA256.HashData(File.ReadAllBytes(input))).ToLowerInvariant()}");
using var module=ModuleDefinition.ReadModule(input,new ReaderParameters{ReadSymbols=false,InMemory=true});
var types=module.Types.SelectMany(AllTypes).ToDictionary(t=>t.FullName,StringComparer.Ordinal);
foreach(var name in args.Skip(1))
{
    if(!types.TryGetValue(name,out var t)){Console.WriteLine($"TYPE_MISSING name={name}"); continue;}
    Console.WriteLine($"TYPE name={name} token=0x{t.MetadataToken.ToInt32():X8} rid={t.MetadataToken.RID}");
    foreach(var m in t.Methods.Where(m=>m.IsConstructor && !m.IsStatic))
    {
        var ps=string.Join(',',m.Parameters.Select(p=>p.ParameterType.FullName));
        Console.WriteLine($"CTOR type={name} token=0x{m.MetadataToken.ToInt32():X8} rid={m.MetadataToken.RID} rva=0x{m.RVA:X} params={m.Parameters.Count} sig={ps} has_body={(m.HasBody?1:0)} code_size={(m.HasBody?m.Body.CodeSize:0)} locals={(m.HasBody?m.Body.Variables.Count:0)} init_locals={(m.HasBody&&m.Body.InitLocals?1:0)}");
        if(!m.HasBody) continue;
        foreach(var i in m.Body.Instructions)
        {
            if(i.OpCode.Code==Code.Ldc_I8 || i.OpCode.Code==Code.Stfld || i.OpCode.Code==Code.Newobj || i.OpCode.Code==Code.Call || i.OpCode.Code==Code.Callvirt)
            {
                string op=i.Operand switch {FieldReference f=>$"F:{f.FullName} token=0x{f.MetadataToken.ToInt32():X8}",MethodReference mr=>$"M:{mr.FullName} token=0x{mr.MetadataToken.ToInt32():X8}",_=>i.Operand?.ToString()??""};
                Console.WriteLine($"  IL_{i.Offset:X4} {i.OpCode.Code} {op}");
            }
        }
    }
}
Console.WriteLine("READONLY_CTOR_PREFETCH_PASS mutation=0");
return 0;

static IEnumerable<TypeDefinition> AllTypes(TypeDefinition t)
{
    yield return t;
    foreach(var n in t.NestedTypes) foreach(var x in AllTypes(n)) yield return x;
}
