using Mono.Cecil;
using Mono.Cecil.Cil;
using System.Buffers.Binary;
using System.Reflection.PortableExecutable;
using System.Security.Cryptography;
using System.Text.Json;
internal static class RawMaterialize
{
    static void Require(bool condition,string message) { if(!condition)throw new InvalidOperationException(message); }
    static IEnumerable<TypeDefinition> Types(TypeDefinition t) {yield return t;foreach(var n in t.NestedTypes)foreach(var x in Types(n))yield return x;}
    static string Hash(byte[] bytes)=>Convert.ToHexString(SHA256.HashData(bytes)).ToLowerInvariant();
    public static void Write(string input,string output,string report)
    {
        Require(Path.GetFullPath(input)!=Path.GetFullPath(output) && !File.Exists(output),"Raw output must be new and distinct");
        var original=File.ReadAllBytes(input);
        Require(Hash(original)=="0d6bc587c98ff05b733d9d5bced8b96726d2dd5d2ba82cb949c6ca90b8f86e8f","Raw pinned input mismatch");
        var support=Environment.GetEnvironmentVariable("GODSPVZ_RESOLVER");Require(Directory.Exists(support),"Explicit support required");
        var resolver=new DefaultAssemblyResolver();foreach(var dir in resolver.GetSearchDirectories())resolver.RemoveSearchDirectory(dir);resolver.AddSearchDirectory(support!);
        using var asm=AssemblyDefinition.ReadAssembly(input,new ReaderParameters {AssemblyResolver=resolver});var module=asm.MainModule;
        var methods=module.Types.SelectMany(Types).SelectMany(t=>t.Methods).ToDictionary(m=>m.MetadataToken.ToUInt32());
        var references=methods.Values.Where(m=>m.HasBody).SelectMany(m=>m.Body.Instructions).Select(i=>i.Operand).OfType<MethodReference>().ToArray();
        using var stream=File.OpenRead(input);using var pe=new PEReader(stream);
        int FileOffset(int rva) {var s=pe.PEHeaders.SectionHeaders.Single(s=>rva>=s.VirtualAddress && rva<s.VirtualAddress+s.SizeOfRawData);return checked(s.PointerToRawData+rva-s.VirtualAddress);}
        int CodeStart(MethodDefinition m) {var at=FileOffset(m.RVA);if((original[at]&3)==2)return at+1;Require((original[at]&3)==3,"Unknown method header");var size=(BinaryPrimitives.ReadUInt16LittleEndian(original.AsSpan(at,2))>>12)*4;Require(size>=12,"Invalid fat header");Require(BinaryPrimitives.ReadUInt32LittleEndian(original.AsSpan(at+4,4))==m.Body.CodeSize,"Code size mismatch");return at+size;}
        var copy=(byte[])original.Clone();var rows=new List<object>();var allowed=new HashSet<int>();
        foreach(var site in new[]{(0x06000348u,7,"call"),(0x06000348u,35,"call"),(0x0600034Cu,225,"call"),(0x06000339u,6,"null"),(0x06000339u,58,"null"),(0x06000339u,161,"throw")})
        {
            var (token,offset,action)=site;var m=methods[token];var ins=m.Body.Instructions.Single(i=>i.Offset==offset);var at=CodeStart(m)+offset;var before=ins.ToString();uint? destination=null;int bytes;
            if(action=="null")
            {
                Require(ins.OpCode==OpCodes.Ldc_I4 && ins.Operand is int value && value==0 && ins.GetSize()==5,"Expected pinned ldc.i4 0");
                // Same byte span: ldnull plus four nops. Branches, EH, RVA and metadata stay unchanged.
                copy[at]=0x14;for(int k=1;k<5;k++)copy[at+k]=0x00;bytes=5;
            }
            else if(action=="throw")
            {
                var load=ins.Previous;var store=load?.Previous;var create=store?.Previous;
                Require(ins.OpCode==OpCodes.Ret && m.ReturnType.FullName=="System.Boolean" && load?.OpCode==OpCodes.Ldloc && store?.OpCode==OpCodes.Stloc && load.Operand is VariableDefinition lv && store.Operand is VariableDefinition sv && ReferenceEquals(lv,sv) && lv.Index==5 && create?.OpCode==OpCodes.Newobj && create.Operand is MethodReference ctor && ctor.DeclaringType.FullName=="System.NullReferenceException","Expected pinned exception path");
                copy[at]=0x7a;bytes=1;
            }
            else
            {
                Require(ins.OpCode==OpCodes.Callvirt && ins.Operand is MethodReference,"Expected callvirt");var source=(MethodReference)ins.Operand;
                Require(source.DeclaringType.FullName=="UnityEngine.Component","Expected Component receiver");
                var target=source.DeclaringType.Resolve().Module.GetType("UnityEngine.GameObject");
                Require(target.Methods.Count(m=>Program.SameShape(m,source))==1,"Target Unity full signature ambiguous");
                var matches=references.Where(r=>r.DeclaringType.FullName=="UnityEngine.GameObject" && Program.SameShape(r,source) && Program.SameInstantiation(r,source)).GroupBy(r=>r.MetadataToken.ToUInt32()).Select(g=>g.First()).OrderBy(r=>r.MetadataToken.ToUInt32()).ToArray();
                Require(matches.Length>0,"Exact GameObject call has no existing baseline token; refuse metadata growth");
                destination=matches[0].MetadataToken.ToUInt32();Require((destination.Value&0xff000000) is 0x0a000000 or 0x2b000000,"Unexpected target token kind");
                Require(ins.GetSize()==5 && copy[at]==0x6f,"Expected five-byte callvirt encoding");
                BinaryPrimitives.WriteUInt32LittleEndian(copy.AsSpan(at+1,4),destination.Value);bytes=5;
            }
            for(int k=0;k<bytes;k++)allowed.Add(at+k);
            rows.Add(new {token=$"0x{token:X8}",offset,action,fileOffset=at,before,reusedBaselineToken=destination==null?null:$"0x{destination:X8}",beforeBytes=Convert.ToHexString(original.AsSpan(at,bytes)),afterBytes=Convert.ToHexString(copy.AsSpan(at,bytes))});
        }
        var changed=Enumerable.Range(0,copy.Length).Where(i=>copy[i]!=original[i]).ToArray();Require(changed.All(allowed.Contains),"Bytes changed outside exact target spans");
        var temp=output+".tmp";Require(!File.Exists(temp),"Temporary output exists");File.WriteAllBytes(temp,copy);
        using(var reread=AssemblyDefinition.ReadAssembly(temp))
        {
            Require(reread.MainModule.Mvid==module.Mvid,"MVID changed");
            foreach(var m in reread.MainModule.Types.SelectMany(Types).SelectMany(t=>t.Methods).Where(m=>m.HasBody))
            {
                var set=m.Body.Instructions.ToHashSet();
                foreach(var i in set){if(i.Operand is Instruction one)Require(set.Contains(one),"Dangling branch");if(i.Operand is Instruction[] all)Require(all.All(set.Contains),"Dangling switch");}
                foreach(var h in m.Body.ExceptionHandlers)Require(new[]{h.TryStart,h.TryEnd,h.HandlerStart,h.HandlerEnd,h.FilterStart}.All(i=>i==null||set.Contains(i)),"Dangling EH");
            }
        }
        File.Move(temp,output);
        File.WriteAllText(report,JsonSerializer.Serialize(new {inputSha256=Hash(original),outputSha256=Hash(copy),fileLengthUnchanged=true,bytesChanged=changed,allowedTargetSpansOnly=true,metadataAndMethodHeadersUntouched=true,rows,qualification="Six pinned native sites; four padding nops per zero literal; not full-game acceptance"},new JsonSerializerOptions {WriteIndented=true}));
        Console.WriteLine(JsonSerializer.Serialize(new {output=Hash(copy),changedBytes=changed.Length,sites=6,backend="existing-token raw PE patch"}));
    }
}
