using Mono.Cecil;
using Mono.Cecil.Cil;
using System.Security.Cryptography;

if (args.Length < 2)
{
    Console.Error.WriteLine("usage: Stage9MethodPrefetchProbe <assembly.dll> <Type::Method[:paramCount]>...");
    return 2;
}
var input=Path.GetFullPath(args[0]);
if(!File.Exists(input)) throw new FileNotFoundException(input);
Console.WriteLine($"INPUT_SHA256 {Convert.ToHexString(SHA256.HashData(File.ReadAllBytes(input))).ToLowerInvariant()}");
using var module=ModuleDefinition.ReadModule(input,new ReaderParameters{ReadSymbols=false,InMemory=true});
IEnumerable<TypeDefinition> AllTypes(IEnumerable<TypeDefinition> roots){foreach(var t in roots){yield return t;foreach(var n in AllTypes(t.NestedTypes))yield return n;}}
var all=AllTypes(module.Types).ToArray();
string Fmt(Instruction i)=>i.Operand switch{null=>"",MethodReference m=>$"M:{m.FullName} token=0x{m.MetadataToken.ToInt32():X8}",FieldReference f=>$"F:{f.FullName} token=0x{f.MetadataToken.ToInt32():X8}",TypeReference t=>$"T:{t.FullName} token=0x{t.MetadataToken.ToInt32():X8}",Instruction x=>$"IL_{x.Offset:X4}",Instruction[] xs=>string.Join(",",xs.Select(x=>$"IL_{x.Offset:X4}")),_=>i.Operand?.ToString()??""};
foreach(var spec in args.Skip(1)){
    var sep=spec.IndexOf("::",StringComparison.Ordinal); if(sep<1) throw new ArgumentException($"bad spec {spec}");
    var tn=spec[..sep]; var tail=spec[(sep+2)..]; int? pc=null; var colon=tail.LastIndexOf(':');
    if(colon>0 && int.TryParse(tail[(colon+1)..],out var parsed)){pc=parsed;tail=tail[..colon];}
    var t=all.SingleOrDefault(x=>x.FullName==tn || x.Name==tn) ?? throw new InvalidOperationException($"type not found/ambiguous: {tn}");
    var ms=t.Methods.Where(m=>m.Name==tail && (!pc.HasValue || m.Parameters.Count==pc.Value)).ToArray();
    if(ms.Length!=1) throw new InvalidOperationException($"method not unique: {spec}, matches={ms.Length}");
    var m=ms[0];
    Console.WriteLine($"METHOD type={t.FullName} type_token=0x{t.MetadataToken.ToInt32():X8} token=0x{m.MetadataToken.ToInt32():X8} rid={m.MetadataToken.RID} rva=0x{m.RVA:X} name={m.Name} params={m.Parameters.Count} has_body={(m.HasBody?1:0)} code_size={(m.HasBody?m.Body.CodeSize:0)} locals={(m.HasBody?m.Body.Variables.Count:0)} init_locals={(m.HasBody&&m.Body.InitLocals?1:0)}");
    if(!m.HasBody) continue;
    foreach(var i in m.Body.Instructions){
        if(i.OpCode.Code==Code.Ldc_I8 || i.OpCode.Code==Code.Stfld || i.OpCode.Code==Code.Call || i.OpCode.Code==Code.Callvirt || i.OpCode.Code==Code.Newobj || i.OpCode.Code==Code.Ldfld)
            Console.WriteLine($"  IL_{i.Offset:X4} {i.OpCode.Code} {Fmt(i)}".TrimEnd());
    }
}
Console.WriteLine("READONLY_METHOD_PREFETCH_PASS mutation=0");
return 0;
