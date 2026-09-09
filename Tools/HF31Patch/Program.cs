using Mono.Cecil;
using Mono.Cecil.Cil;
using System.Security.Cryptography;

if (args.Length != 2)
{
    Console.Error.WriteLine("Usage: GodsPVZ.HF31Patch <input.dll> <output.dll>");
    return 2;
}

const string ExpectedInputSha256 = "6ccccf685950edba428b51dbe3aa95e511caa467d134dd4202939e19ef655c24";
var input = Path.GetFullPath(args[0]);
var output = Path.GetFullPath(args[1]);
var inputSha = Convert.ToHexString(SHA256.HashData(File.ReadAllBytes(input))).ToLowerInvariant();
Console.WriteLine($"FORMAL_INPUT_SHA256 {inputSha}");
if (inputSha != ExpectedInputSha256) throw new InvalidDataException($"HF31 formal input SHA mismatch: expected {ExpectedInputSha256}, got {inputSha}");

var self = typeof(Template.Projectile).Assembly.Location;
if (string.IsNullOrEmpty(self) || !File.Exists(self)) throw new InvalidOperationException("HF31 requires multi-file publish.");
using var target = ModuleDefinition.ReadModule(input, new ReaderParameters { InMemory = true });
using var template = ModuleDefinition.ReadModule(self, new ReaderParameters { InMemory = true });

static IEnumerable<TypeDefinition> AllTypes(IEnumerable<TypeDefinition> roots)
{
    foreach (var t in roots)
    {
        yield return t;
        foreach (var n in AllTypes(t.NestedTypes)) yield return n;
    }
}
var tTypes = AllTypes(target.Types).ToList();
var tDefs = tTypes.ToDictionary(t => t.FullName, StringComparer.Ordinal);
var tTypeRefs = target.GetTypeReferences().GroupBy(t => t.FullName).ToDictionary(g => g.Key, g => g.First(), StringComparer.Ordinal);
var tMethodRefs = tTypes.SelectMany(t => t.Methods).Where(m => m.HasBody).SelectMany(m => m.Body.Instructions)
    .Select(i => i.Operand).OfType<MethodReference>().Select(m => m is GenericInstanceMethod g ? g.ElementMethod : m).ToList();
var tFieldRefs = tTypes.SelectMany(t => t.Methods).Where(m => m.HasBody).SelectMany(m => m.Body.Instructions).Select(i => i.Operand).OfType<FieldReference>().ToList();
string TN(string n) => n.StartsWith("Template.", StringComparison.Ordinal) ? n[9..] : n;

TypeReference MT(TypeReference t, MethodDefinition? dst = null, GenericInstanceType? ctx = null)
{
    if (t is GenericParameter gp)
    {
        if (gp.Type == GenericParameterType.Type && ctx != null) return MT(ctx.GenericArguments[gp.Position], dst);
        if (gp.Type == GenericParameterType.Method && dst != null) return dst.GenericParameters[gp.Position];
        throw new InvalidDataException($"HF31 generic param {gp.FullName}");
    }
    if (t is ByReferenceType br) return new ByReferenceType(MT(br.ElementType,dst,ctx));
    if (t is ArrayType ar) return new ArrayType(MT(ar.ElementType,dst,ctx), ar.Rank);
    if (t is GenericInstanceType gi)
    {
        var g = new GenericInstanceType(MT(gi.ElementType,dst,ctx));
        foreach (var a in gi.GenericArguments) g.GenericArguments.Add(MT(a,dst));
        return g;
    }
    var n = TN(t.FullName);
    if (tDefs.TryGetValue(n, out var d)) return d;
    if (tTypeRefs.TryGetValue(n, out var r)) return r;
    return n switch
    {
        "System.Void" => target.TypeSystem.Void,
        "System.Boolean" => target.TypeSystem.Boolean,
        "System.Int32" => target.TypeSystem.Int32,
        "System.Single" => target.TypeSystem.Single,
        "System.String" => target.TypeSystem.String,
        "System.Object" => target.TypeSystem.Object,
        _ => throw new InvalidDataException($"HF31 cannot map type {t.FullName} -> {n}")
    };
}
string Sig(TypeReference t) => TN(t.FullName);
FieldReference MF(FieldReference f, MethodDefinition dst)
{
    var dn = TN(f.DeclaringType.FullName);
    if (tDefs.TryGetValue(dn, out var td)) return td.Fields.Single(x => x.Name == f.Name);
    var md = MT(f.DeclaringType,dst); var ft = MT(f.FieldType,dst,f.DeclaringType as GenericInstanceType);
    return tFieldRefs.FirstOrDefault(x => x.DeclaringType.FullName == md.FullName && x.Name == f.Name) ?? new FieldReference(f.Name,ft,md);
}
MethodReference Construct(MethodReference m, MethodDefinition dst)
{
    var ctx = m.DeclaringType as GenericInstanceType;
    var x = new MethodReference(m.Name, MT(m.ReturnType,dst,ctx), MT(m.DeclaringType,dst)) { HasThis=m.HasThis, ExplicitThis=m.ExplicitThis, CallingConvention=m.CallingConvention };
    foreach (var gp in m.GenericParameters) x.GenericParameters.Add(new GenericParameter(gp.Name,x));
    foreach (var p in m.Parameters) x.Parameters.Add(new ParameterDefinition(MT(p.ParameterType,dst,ctx)));
    return x;
}
MethodReference MM(MethodReference m, MethodDefinition dst)
{
    if (m is GenericInstanceMethod gim)
    {
        var g = new GenericInstanceMethod(MM(gim.ElementMethod,dst));
        foreach (var a in gim.GenericArguments) g.GenericArguments.Add(MT(a,dst));
        return g;
    }
    var dn = TN(m.DeclaringType.FullName);
    if (tDefs.TryGetValue(dn, out var td))
    {
        var c = td.Methods.Where(x => x.Name==m.Name && x.Parameters.Count==m.Parameters.Count && x.GenericParameters.Count==m.GenericParameters.Count).ToList();
        if (c.Count==1) return c[0];
        var want = m.Parameters.Select(p => Sig(MT(p.ParameterType,dst,m.DeclaringType as GenericInstanceType))).ToArray();
        var exact = c.Where(x => x.Parameters.Select(p=>Sig(p.ParameterType)).SequenceEqual(want)).ToList();
        if (exact.Count==1) return exact[0];
        throw new InvalidDataException($"HF31 method {dn}.{m.Name}/{m.Parameters.Count} candidates={c.Count} exact={exact.Count}");
    }
    var ctx = m.DeclaringType as GenericInstanceType; var md = MT(m.DeclaringType,dst);
    var wantP = m.Parameters.Select(p=>Sig(MT(p.ParameterType,dst,ctx))).ToArray();
    var refs = tMethodRefs.Where(x=>x.DeclaringType.FullName==md.FullName && x.Name==m.Name && x.Parameters.Count==wantP.Length).Where(x=>x.Parameters.Select(p=>Sig(p.ParameterType)).SequenceEqual(wantP)).ToList();
    return refs.Count>0 ? refs[0] : Construct(m,dst);
}
Instruction CI(Instruction old, MethodDefinition dst, Dictionary<VariableDefinition,VariableDefinition> vars)
{
    var op=old.OpCode;
    return old.Operand switch
    {
        null => Instruction.Create(op), sbyte x=>Instruction.Create(op,x), byte x=>Instruction.Create(op,(sbyte)x), int x=>Instruction.Create(op,x), long x=>Instruction.Create(op,x), float x=>Instruction.Create(op,x), double x=>Instruction.Create(op,x), string x=>Instruction.Create(op,x),
        FieldReference x=>Instruction.Create(op,MF(x,dst)), MethodReference x=>Instruction.Create(op,MM(x,dst)), TypeReference x=>Instruction.Create(op,MT(x,dst)), VariableDefinition x=>Instruction.Create(op,vars[x]), ParameterDefinition x=>Instruction.Create(op,dst.Parameters[x.Index]), Instruction x=>Instruction.Create(op,x), Instruction[] x=>Instruction.Create(op,x),
        _=>throw new NotSupportedException($"HF31 operand {old.Operand.GetType().FullName} at {old}")
    };
}
void Clone(MethodDefinition src, MethodDefinition dst)
{
    var sb=src.Body; var db=new Mono.Cecil.Cil.MethodBody(dst){InitLocals=sb.InitLocals,MaxStackSize=Math.Max(sb.MaxStackSize,8)}; dst.Body=db;
    var vars=new Dictionary<VariableDefinition,VariableDefinition>(); foreach(var v in sb.Variables){var nv=new VariableDefinition(MT(v.VariableType,dst));db.Variables.Add(nv);vars[v]=nv;}
    var map=new Dictionary<Instruction,Instruction>(); foreach(var i in sb.Instructions){var ni=CI(i,dst,vars);db.Instructions.Add(ni);map[i]=ni;}
    foreach(var i in sb.Instructions){if(i.Operand is Instruction b) map[i].Operand=map[b]; else if(i.Operand is Instruction[] sw) map[i].Operand=sw.Select(x=>map[x]).ToArray();}
    foreach(var eh in sb.ExceptionHandlers) db.ExceptionHandlers.Add(new ExceptionHandler(eh.HandlerType){CatchType=eh.CatchType==null?null:MT(eh.CatchType,dst),TryStart=eh.TryStart==null?null:map[eh.TryStart],TryEnd=eh.TryEnd==null?null:map[eh.TryEnd],HandlerStart=eh.HandlerStart==null?null:map[eh.HandlerStart],HandlerEnd=eh.HandlerEnd==null?null:map[eh.HandlerEnd],FilterStart=eh.FilterStart==null?null:map[eh.FilterStart]});
}

const uint Token=0x060003F6;
var dstM=target.LookupToken(new MetadataToken(TokenType.Method,(int)(Token&0x00FFFFFF))) as MethodDefinition ?? throw new InvalidDataException("HF31 target missing");
if(dstM.DeclaringType.FullName!="Projectile"||dstM.Name!="Rotating"||dstM.Parameters.Count!=0) throw new InvalidDataException($"HF31 token drift: {dstM.FullName}");
var srcT=AllTypes(template.Types).Single(t=>t.FullName=="Template.Projectile"); var srcM=srcT.Methods.Single(m=>m.Name=="Rotating"&&m.Parameters.Count==0);
Clone(srcM,dstM);
Console.WriteLine($"PATCH 0x{Token:X8} Projectile.Rotating/0 il={dstM.Body.Instructions.Count} bytes={dstM.Body.CodeSize} eh={dstM.Body.ExceptionHandlers.Count}");
target.Write(output);
using(var reopen=ModuleDefinition.ReadModule(output,new ReaderParameters{InMemory=true}))
{
    var m=reopen.LookupToken(new MetadataToken(TokenType.Method,(int)(Token&0x00FFFFFF))) as MethodDefinition ?? throw new InvalidDataException("HF31 reopen missing");
    if(!m.HasBody||m.Body.Instructions.Count<20) throw new InvalidDataException($"HF31 reopen invalid {m.Body.Instructions.Count}");
    if(m.Body.Instructions.Any(i=>i.Operand is MethodReference mr&&mr.DeclaringType.FullName.Contains("Cpp2IL",StringComparison.Ordinal))) throw new InvalidDataException("HF31 Cpp2IL helper remained");
    Console.WriteLine($"REOPEN 0x{Token:X8} il={m.Body.Instructions.Count} bytes={m.Body.CodeSize} eh={m.Body.ExceptionHandlers.Count}");
}
return 0;
