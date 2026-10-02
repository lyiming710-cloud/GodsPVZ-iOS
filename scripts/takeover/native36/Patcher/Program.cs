using Mono.Cecil;
using Mono.Cecil.Cil;
using System.Reflection.PortableExecutable;
using System.Reflection.Metadata;
using System.Reflection.Metadata.Ecma335;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using MD=Mono.Cecil.MethodDefinition;
using MR=Mono.Cecil.MethodReference;
using FR=Mono.Cecil.FieldReference;
static class RepairNative36 {
 static void R(bool x,string m){if(!x)throw new InvalidOperationException(m);}
 static string H(byte[] b)=>Convert.ToHexString(SHA256.HashData(b)).ToLowerInvariant();
 static int Align(int x,int a)=>checked((x+a-1)/a*a);
 class IL {
  public List<byte> B=new();Dictionary<string,int> labels=new();List<(int p,string l)> branches=new();
  public IL O(params byte[] x){B.AddRange(x);return this;}public IL I(int x){B.AddRange(BitConverter.GetBytes(x));return this;}
  public IL Ref(byte op,IMetadataTokenProvider r)=>O(op).I(r.MetadataToken.ToInt32());public IL Ref(byte op,int t)=>O(op).I(t);
  public IL Int(int x)=>x>=-1&&x<=8?O((byte)(x+0x16)):O(0x20).I(x);public IL Float(float x)=>O(0x22).I(BitConverter.SingleToInt32Bits(x));
  public IL L(int i)=>i<4?O((byte)(6+i)):O(0x11,(byte)i);public IL S(int i)=>i<4?O((byte)(10+i)):O(0x13,(byte)i);public IL A(int i)=>O(0x12,(byte)i);
  public IL Arg(int i)=>i<4?O((byte)(2+i)):O(0x0E,(byte)i);
  public IL Label(string s){labels.Add(s,B.Count);return this;}public IL Branch(byte op,string s){O(op);branches.Add((B.Count,s));O(0);return this;}
  public byte[] Done(){foreach(var(p,l)in branches){int d=labels[l]-p-1;R(d>=-128&&d<=127,"Short branch overflow");B[p]=unchecked((byte)(sbyte)d);}return B.ToArray();}
 }
 record Stream(string Name,byte[] Data);
 record Added(string Name,int Token,int Parent,int NameIndex,int SignatureIndex);
 static (byte[] Image,object Evidence) Metadata(byte[] input,PEReader pe,Mono.Cecil.ModuleDefinition module){
  var md=pe.GetMetadataReader();int rootRva=pe.PEHeaders.CorHeader!.MetadataDirectory.RelativeVirtualAddress;var section=pe.PEHeaders.SectionHeaders.Single(s=>rootRva>=s.VirtualAddress&&rootRva<s.VirtualAddress+s.SizeOfRawData);int rootOffset=section.PointerToRawData+rootRva-section.VirtualAddress;int rootSize=pe.PEHeaders.CorHeader.MetadataDirectory.Size;byte[] root=input[rootOffset..(rootOffset+rootSize)];R(BitConverter.ToUInt32(root,0)==0x424A5342,"Metadata root");int vlen=BitConverter.ToInt32(root,12);int countAt=Align(16+vlen,4)+2;int count=BitConverter.ToUInt16(root,countAt);int p=countAt+2;var streams=new List<Stream>();
  for(int i=0;i<count;i++){int off=BitConverter.ToInt32(root,p),len=BitConverter.ToInt32(root,p+4);int end=Array.IndexOf(root,(byte)0,p+8);R(end>=0,"Stream name");string name=Encoding.ASCII.GetString(root,p+8,end-p-8);streams.Add(new(name,root[off..(off+len)]));p=Align(end+1,4);}int headEnd=p;
  int ti=streams.FindIndex(s=>s.Name=="#~");R(ti>=0,"Compressed tables required");var table=streams[ti].Data;var strings=streams.Single(s=>s.Name=="#Strings").Data;var blob=streams.Single(s=>s.Name=="#Blob").Data;int oldStrings=strings.Length,oldBlob=blob.Length;R((table[6]&3)==0,"2-byte heap indexes");R(md.GetTableRowCount(TableIndex.TypeRef)==176&&md.GetTableRowCount(TableIndex.MemberRef)==1027&&md.GetTableRowCount(TableIndex.MethodSpec)==180,"Pinned table counts");
  int Name(string name){byte[] b=Encoding.UTF8.GetBytes(name+"\0");for(int i=0;i+b.Length<=strings.Length;i++)if(strings[i..(i+b.Length)].SequenceEqual(b))return i;R(new[]{"Interlocked","CompareExchange","System.Threading"}.Contains(name),"Approved new name");int ix=strings.Length;strings=strings.Concat(b).ToArray();return ix;}
  int Blob(byte[] b){R(b.Length<128,"Short blob length");int ix=blob.Length;blob=blob.Concat(new[]{(byte)b.Length}).Concat(b).ToArray();return ix;}
  byte[] Words(params int[] values)=>values.SelectMany(v=>{R(v>=0&&v<65536,"Index overflow");return BitConverter.GetBytes((ushort)v);}).ToArray();
  int ts(string name){var hits=Enumerable.Range(1,md.GetTableRowCount(TableIndex.TypeSpec)).Select(i=>module.LookupToken(0x1B000000+i)).OfType<Mono.Cecil.TypeReference>().Where(t=>t.FullName==name).ToArray();R(hits.Length==1,"Exact TypeSpec "+name);return checked((int)hits[0].MetadataToken.RID);}
  int clip=ts("System.Action`1<FTRuntime.SwfClip>"),controller=ts("System.Action`1<FTRuntime.SwfClipController>");
  R(module.GetTypeReferences().All(t=>t.FullName!="System.Threading.Interlocked"),"Interlocked initially absent");var core=md.AssemblyReferences.Single(h=>md.GetString(md.GetAssemblyReference(h).Name)=="mscorlib");int scope=MetadataTokens.GetRowNumber(core)*4+2,newType=177,newMember=1028;
  byte[] typeRow=Words(scope,Name("Interlocked"),Name("System.Threading"));int compareSig=Blob(new byte[]{0x10,1,3,0x1e,0,0x10,0x1e,0,0x1e,0,0x1e,0});byte[] memberRow=Words(newType*8+1,Name("CompareExchange"),compareSig);
  int ms(int rid)=>Blob(new byte[]{0x0A,1}.Concat(md.GetBlobBytes(md.GetTypeSpecification(MetadataTokens.TypeSpecificationHandle(rid)).Signature)).ToArray());
  byte[] specRows=Words(newMember*2+1,ms(clip)).Concat(Words(newMember*2+1,ms(controller))).ToArray();R(strings.Length<65536&&blob.Length<65536,"No heap widening");
  var additions=new Dictionary<TableIndex,byte[]>{{TableIndex.TypeRef,typeRow},{TableIndex.MemberRef,memberRow},{TableIndex.MethodSpec,specRows}};ulong valid=BitConverter.ToUInt64(table,8);int tableStream=0,scan2=countAt+2;for(int i=0;i<count;i++){int e=Array.IndexOf(root,(byte)0,scan2+8);if(Encoding.ASCII.GetString(root,scan2+8,e-scan2-8)=="#~")tableStream=BitConverter.ToInt32(root,scan2);scan2=Align(e+1,4);}R(tableStream>0,"Table stream offset");
  var expanded=table.ToList();foreach(var(t,bytes)in additions.OrderByDescending(k=>k.Key)){int width=md.GetTableRowSize(t);R(width==(t==TableIndex.MethodSpec?4:6),"Pinned row widths");int end=md.GetTableMetadataOffset(t)-tableStream+md.GetTableRowCount(t)*width;expanded.InsertRange(end,bytes);}
  var tables=expanded.ToArray();foreach(var(t,bytes)in additions){int at=24+4*Enumerable.Range(0,(int)t).Count(i=>(valid&(1UL<<i))!=0);BitConverter.GetBytes(md.GetTableRowCount(t)+bytes.Length/md.GetTableRowSize(t)).CopyTo(tables,at);}ulong sorted=BitConverter.ToUInt64(table,16),clear=(1UL<<(int)TableIndex.TypeRef)|(1UL<<(int)TableIndex.MemberRef)|(1UL<<(int)TableIndex.MethodSpec);BitConverter.GetBytes(sorted&~clear).CopyTo(tables,16);streams[ti]=new("#~",tables);streams[streams.FindIndex(s=>s.Name=="#Strings")]=new("#Strings",strings);streams[streams.FindIndex(s=>s.Name=="#Blob")]=new("#Blob",blob);
  var nr=new List<byte>(root[..headEnd]);int scan=countAt+2;foreach(var s in streams){while(nr.Count%4!=0)nr.Add(0);BitConverter.GetBytes(nr.Count).CopyTo(nr,scan);BitConverter.GetBytes(s.Data.Length).CopyTo(nr,scan+4);nr.AddRange(s.Data);int e=Array.IndexOf(root,(byte)0,scan+8);scan=Align(e+1,4);}byte[] meta=nr.ToArray();
  // The qualified Native29 metadata section is already the last section.
  // Its header has no room for another section header. Extend that read-only
  // section at EOF while retaining all existing bytes, raw starts and RVAs.
  var ph=pe.PEHeaders;var opt=ph.PEHeader!;R(opt.Magic==PEMagic.PE32&&opt.CertificateTableDirectory.Size==0&&ph.CorHeader.StrongNameSignatureDirectory.Size==0&&(ph.CorHeader.Flags&CorFlags.StrongNameSigned)==0,"Unsupported signed/input layout");var last=ph.SectionHeaders.Last();R(last.Name==".n29meta"&&last.PointerToRawData+last.SizeOfRawData==input.Length&&last.VirtualSize<=last.SizeOfRawData&&last.SectionCharacteristics==(SectionCharacteristics)0x40000040,"Pinned extensible metadata section");R(rootOffset>=last.PointerToRawData&&rootOffset+rootSize<=input.Length,"Parent metadata in last section");int header=ph.PEHeaderStartOffset+ph.CoffHeader.SizeOfOptionalHeader+(ph.CoffHeader.NumberOfSections-1)*40;int raw=Align(input.Length,opt.FileAlignment),va=last.VirtualAddress+raw-last.PointerToRawData,append=Align(meta.Length,opt.FileAlignment);var output=new byte[raw+append];input.CopyTo(output,0);meta.CopyTo(output,raw);int newVirtual=raw-last.PointerToRawData+meta.Length,newRaw=output.Length-last.PointerToRawData;
  BitConverter.GetBytes(newVirtual).CopyTo(output,header+8);BitConverter.GetBytes(newRaw).CopyTo(output,header+16);BitConverter.GetBytes(opt.SizeOfInitializedData+output.Length-input.Length).CopyTo(output,ph.PEHeaderStartOffset+8);BitConverter.GetBytes(Align(last.VirtualAddress+newVirtual,opt.SectionAlignment)).CopyTo(output,ph.PEHeaderStartOffset+56);new byte[4].CopyTo(output,ph.PEHeaderStartOffset+64);BitConverter.GetBytes(va).CopyTo(output,ph.CorHeaderStartOffset+8);BitConverter.GetBytes(meta.Length).CopyTo(output,ph.CorHeaderStartOffset+12);
  return(output,new{old_root_offset=rootOffset,old_root_size=rootSize,new_root_offset=raw,new_root_size=meta.Length,extended_section_header=header,old_virtual_size=last.VirtualSize,new_virtual_size=newVirtual,old_raw_size=last.SizeOfRawData,new_raw_size=newRaw,added_type_ref="0x010000B1",added_member_ref="0x0A000404",added_method_specs=new[]{"0x2B0000B5","0x2B0000B6"},existing_heap_prefixes_unchanged=true,string_heap_added_bytes=strings.Length-oldStrings,blob_heap_added_bytes=blob.Length-oldBlob});

 }
 static void CopyTo(this byte[] b,List<byte> list,int p){for(int j=0;j<b.Length;j++)list[p+j]=b[j];}
 static int Main(string[] args){try{Run(args);return 0;}catch(Exception e){Console.Error.WriteLine(e);return 3;}}
 static void Run(string[] args){R(args.Length==3,"input new-output report");R(!File.Exists(args[1])&&!File.Exists(args[2]),"Fresh outputs required");byte[] before=File.ReadAllBytes(args[0]);R(H(before)=="55cf8ca5de4ee0a4eefa2a36ddf3d6df86f125a73cd5769a8558ea83cde2e0ea","Pinned Native35 input");using var asm=Mono.Cecil.AssemblyDefinition.ReadAssembly(args[0]);var module=asm.MainModule;using var fs=File.OpenRead(args[0]);using var pe=new PEReader(fs);var(after,metadata)=Metadata(before,pe,module);
  MD M(int t)=>(MD)module.LookupToken(t);var targets=new Dictionary<int,(byte[] code,int max)>();var report=new List<object>();MD m;
  foreach(int token in new[]{0x06000822,0x06000823,0x06000824,0x06000825,0x06000826,0x06000827,0x06000858,0x06000859,0x0600085A,0x0600085B,0x0600085C,0x0600085D}){
   m=M(token);var field=m.Body.Instructions.Select(i=>i.Operand).OfType<FR>().DistinctBy(f=>f.MetadataToken).Single();var combine=m.Body.Instructions.Select(i=>i.Operand).OfType<MR>().Single(c=>c.Name==(m.Name.StartsWith("add_")?"Combine":"Remove"));bool clip=m.DeclaringType.FullName=="FTRuntime.SwfClip";string action="System.Action`1<"+m.DeclaringType.FullName+">";
   int type=Enumerable.Range(1,pe.GetMetadataReader().GetTableRowCount(TableIndex.TypeSpec)).Select(i=>module.LookupToken(0x1B000000+i)).OfType<Mono.Cecil.TypeReference>().Single(t=>t.FullName==action).MetadataToken.ToInt32();R(m.Body.Variables[0].VariableType.FullName==action&&m.Body.Variables[8].VariableType.FullName=="System.Delegate"&&m.Body.Variables[9].VariableType.FullName=="System.Delegate","Typed delegate scratches");
   var il=new IL().Arg(0).Ref(0x7B,field).S(8).Label("retry").L(8).S(9).L(9).Arg(1).Ref(0x28,combine).Ref(0x74,type).S(0).Arg(0).Ref(0x7C,field).L(0).L(9).Ref(0x74,type).Ref(0x28,clip?0x2B0000B5:0x2B0000B6).S(8).L(8).L(9).Branch(0x33,"retry").O(0x2A);targets.Add(token,(il.Done(),3));
  }
  foreach(var(token,(code,max))in targets){m=M(token);R(m.Body.ExceptionHandlers.Count==0,"EH relocation unsupported");var sec=pe.PEHeaders.SectionHeaders.Single(s=>m.RVA>=s.VirtualAddress&&m.RVA<s.VirtualAddress+s.SizeOfRawData);int h=sec.PointerToRawData+m.RVA-sec.VirtualAddress;int hs=(BitConverter.ToUInt16(before,h)>>12)*4;R((before[h]&3)==3&&hs==12,"Pinned fat body");R(code.Length<=m.Body.CodeSize,"Body exceeds reserved slot "+m.FullName);Array.Clear(after,h+hs,m.Body.CodeSize);code.CopyTo(after,h+hs);BitConverter.GetBytes(code.Length).CopyTo(after,h+4);BitConverter.GetBytes((ushort)max).CopyTo(after,h+2);report.Add(new{token=$"0x{token:X8}",method=m.FullName,header=h,code_start=h+hs,reserved_code_size=m.Body.CodeSize,new_code_size=code.Length,old_max=m.Body.MaxStackSize,new_max=max,local_sig=m.Body.LocalVarToken.ToInt32(),code_sha256=H(code)});}
  R(targets.Count==12,"Pinned scope 12");File.WriteAllBytes(args[1],after);File.WriteAllText(args[2],JsonSerializer.Serialize(new{input_sha256=H(before),candidate_sha256=H(after),metadata,methods=report,scope="12 native-pinned atomic event accessors; old rows/heaps preserved; one public Interlocked TypeRef/MemberRef and two closed MethodSpecs appended"},new JsonSerializerOptions{WriteIndented=true}));Console.WriteLine("CANDIDATE "+H(after));
 }
}
