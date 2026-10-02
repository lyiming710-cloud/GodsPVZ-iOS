using System.Reflection.Metadata;
using System.Reflection.Metadata.Ecma335;
using System.Reflection.PortableExecutable;
using System.Security.Cryptography;
using System.Text.Json;
using Mono.Cecil;

static class AuditNative39 {
 static void R(bool b,string s){if(!b)throw new Exception(s);}
 static string H(byte[] b)=>Convert.ToHexString(SHA256.HashData(b)).ToLowerInvariant();
 static byte[] Root(PEReader p)=>p.GetMetadata().GetContent().ToArray();
 static int Offset(PEReader p,int rva){var s=p.PEHeaders.SectionHeaders.Single(s=>rva>=s.VirtualAddress&&rva<s.VirtualAddress+s.SizeOfRawData);return s.PointerToRawData+rva-s.VirtualAddress;}

 static int Main(string[] args){
  try{
   var b=File.ReadAllBytes(args[0]);
   var c=File.ReadAllBytes(args[1]);
   R(H(b)=="be3bd138a4db23ac7d4f2e9046101002623ac94b1599aad0870d7fde2718066c","Pinned Native36 parent");using var pb=new PEReader(new MemoryStream(b));
   using var pc=new PEReader(new MemoryStream(c));
   var mb=pb.GetMetadataReader();
   var mc=pc.GetMetadataReader();
   var rb=Root(pb);
   var rc=Root(pc);
   using var ab=Mono.Cecil.AssemblyDefinition.ReadAssembly(args[0]);
   using var ac=Mono.Cecil.AssemblyDefinition.ReadAssembly(args[1]);
   var report=JsonDocument.Parse(File.ReadAllText(args[2]));
   var targets=report.RootElement.GetProperty("methods").EnumerateArray().Select(x=>Convert.ToInt32(x.GetProperty("token").GetString(),16)).ToHashSet();
   R(targets.SetEquals(new[]{0x06000091,0x060008A5}),$"Target cardinality expected 4, got {targets.Count}");
   var allowed=new HashSet<int>();
   void Allow(int p,int n){for(int j=0;j<n;j++)allowed.Add(p+j);}
   var tableRows=new List<object>();

   foreach(TableIndex t in Enum.GetValues<TableIndex>()){
       int n=mb.GetTableRowCount(t);
       R(mc.GetTableRowCount(t)==n,"Table count "+t);
       if(n==0)continue;
       int size=mb.GetTableRowSize(t);
       R(size==mc.GetTableRowSize(t),"Row width "+t);
       int x=mb.GetTableMetadataOffset(t),y=mc.GetTableMetadataOffset(t);
       R(rb[x..(x+n*size)].SequenceEqual(rc[y..(y+n*size)]),"Existing row drift "+t);
       tableRows.Add(new{table=t.ToString(),rows=n,row_size=size});
   }
   foreach(HeapIndex h in Enum.GetValues<HeapIndex>()){
       int n=mb.GetHeapSize(h);
       R(mc.GetHeapSize(h)==n,"Heap size "+h);
       int x=mb.GetHeapMetadataOffset(h),y=mc.GetHeapMetadataOffset(h);
       R(rb[x..(x+n)].SequenceEqual(rc[y..(y+n)]),"Heap bytes "+h);
   }

   int non=0;
   foreach(var h in mb.MethodDefinitions){
       int token=MetadataTokens.GetToken(h);
       var bm=mb.GetMethodDefinition(h);
       var cm=mc.GetMethodDefinition(h);
       R(bm.RelativeVirtualAddress==cm.RelativeVirtualAddress,"RVA drift");
       if(bm.RelativeVirtualAddress==0)continue;
       var ob=pb.GetMethodBody(bm.RelativeVirtualAddress);
       var nb=pc.GetMethodBody(cm.RelativeVirtualAddress);
       int off=Offset(pb,bm.RelativeVirtualAddress);
       if(targets.Contains(token)){
           R(ob.LocalSignature==nb.LocalSignature&&ob.LocalVariablesInitialized==nb.LocalVariablesInitialized&&ob.ExceptionRegions.Length==0&&nb.ExceptionRegions.Length==0,"Target local/header flags");
           var row=report.RootElement.GetProperty("methods").EnumerateArray().Single(x=>Convert.ToInt32(x.GetProperty("token").GetString(),16)==token);
           R(row.GetProperty("header").GetInt32()==off,"Pinned body offset");
           Allow(off+2,6); // max stack + code size
           Allow(off+12,ob.GetILBytes()!.Length);
           var md=(Mono.Cecil.MethodDefinition)ac.MainModule.LookupToken(token);
           R(md.Body.Instructions.All(i=>i.Operand is not Mono.Cecil.Cil.Instruction j||md.Body.Instructions.Contains(j)),"Invalid branch");
           R(!md.Body.Instructions.Any(i=>i.OpCode==Mono.Cecil.Cil.OpCodes.Ldstr&&i.Operand is string s&&(s.Contains("Indirect call:")||s.Contains("Warning:"))),"Unresolved reconstruction");
           R(H(nb.GetILBytes()!)==row.GetProperty("code_sha256").GetString(),"Reopened code digest");
       }else{
           R(ob.Size==nb.Size,"Non-target size");
           R(b[off..(off+ob.Size)].SequenceEqual(c[off..(off+nb.Size)]),"Non-target raw body "+token.ToString("X8"));
           non++;
       }
   }

   var diffs=Enumerable.Range(0,b.Length).Where(i=>b[i]!=c[i]).ToArray();
   R(diffs.All(allowed.Contains),"Unapproved original-file difference");
   R(b.Length==c.Length,"Candidate length changed");
   R(pc.PEHeaders.SectionHeaders.Length==pb.PEHeaders.SectionHeaders.Length,"Section count changed");
   for(int i=0;i<pb.PEHeaders.SectionHeaders.Length;i++)
       R(pb.PEHeaders.SectionHeaders[i].Equals(pc.PEHeaders.SectionHeaders[i]),"PE section changed");

   var r=new{
       input_sha256=H(b),
       candidate_sha256=H(c),
       targets=targets.Count,
       non_target_raw_bodies_equal=non,
       existing_table_rows_equal=tableRows,
       existing_heaps_equal=true,
       original_sections_equal=true,
       old_file_changed_bytes=diffs.Length,
       appended_bytes=0,
       pass=true
   };
   File.WriteAllText(args[3],JsonSerializer.Serialize(r,new JsonSerializerOptions{WriteIndented=true}));
   Console.WriteLine(JsonSerializer.Serialize(new{r.targets,r.non_target_raw_bodies_equal,r.old_file_changed_bytes,r.pass}));
   return 0;
  }catch(Exception e){
   Console.Error.WriteLine(e);
   return 2;
  }
 }
}
