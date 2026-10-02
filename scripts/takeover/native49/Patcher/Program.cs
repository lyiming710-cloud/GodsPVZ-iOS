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
static class RepairNative49 {
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
  int ti=streams.FindIndex(s=>s.Name=="#~");R(ti>=0,"Compressed table stream required");var table=streams[ti].Data;var stringStream=streams.Single(s=>s.Name=="#Strings").Data;var blobStream=streams.Single(s=>s.Name=="#Blob").Data;R((table[6]&3)==0,"Pinned 2-byte string/blob indexes");int rows=md.GetTableRowCount(TableIndex.MemberRef);int rowSize=md.GetTableRowSize(TableIndex.MemberRef);R(rows==1037&&rowSize==6,"Pinned MemberRef layout");R(md.GetTableRowCount(TableIndex.TypeSpec)<8192&&md.GetTableRowCount(TableIndex.TypeDef)<8192&&md.GetTableRowCount(TableIndex.TypeRef)<8192&&md.GetTableRowCount(TableIndex.ModuleRef)<8192&&md.GetTableRowCount(TableIndex.MethodDef)<8192,"Parent index width");
  int ts(string name){var hits=Enumerable.Range(1,md.GetTableRowCount(TableIndex.TypeSpec)).Select(i=>module.LookupToken(0x1B000000+i)).OfType<Mono.Cecil.TypeReference>().Where(t=>t.FullName==name).ToArray();R(hits.Length==1,"Exact TypeSpec "+name);return hits[0].MetadataToken.RID is uint r?checked((int)r):throw new Exception();}
  int originalStringSize=stringStream.Length;int nameIndex(string name){for(int i=0;i<stringStream.Length-name.Length;i++){if(Encoding.UTF8.GetString(stringStream,i,name.Length)==name&&stringStream[i+name.Length]==0)return i;}throw new Exception("Existing member name required "+name);}
  int sig(byte[] desired){foreach(var h in md.MemberReferences){var m=md.GetMemberReference(h);if(md.GetBlobBytes(m.Signature).SequenceEqual(desired))return MetadataTokens.GetHeapOffset(m.Signature);}throw new Exception("Existing signature missing");}
  var specs=new[]{("get_Count",ts("System.Collections.Generic.List`1<Zombie_charred>")*8+4,sig(new byte[]{0x20,0,8}))};
  ulong valid=BitConverter.ToUInt64(table,8);R((valid&(1UL<<10))!=0,"MemberRef missing");int countPos=24+4*Enumerable.Range(0,10).Count(i=>(valid&(1UL<<i))!=0);R(BitConverter.ToInt32(table,countPos)==rows,"Row count pin");int tableStart=md.GetTableMetadataOffset(TableIndex.MemberRef);int streamRootOffset=Array.FindIndex(root,_=>false); // replaced with exact stream offset below
  int scan=countAt+2;for(int i=0;i<count;i++){int e=Array.IndexOf(root,(byte)0,scan+8);if(Encoding.ASCII.GetString(root,scan+8,e-scan-8)=="#~")streamRootOffset=BitConverter.ToInt32(root,scan);scan=Align(e+1,4);}R(streamRootOffset>=0,"Table stream root offset");int insert=tableStart-streamRootOffset+rows*rowSize;R(insert>=24&&insert<=table.Length,"MemberRef table end");var add=new List<byte>();var added=new List<Added>();foreach(var(name,parent,signature)in specs){int ni=nameIndex(name);R(ni<65536&&signature<65536&&parent<65536,"Index overflow");int coded=parent;add.AddRange(BitConverter.GetBytes((ushort)coded));add.AddRange(BitConverter.GetBytes((ushort)ni));add.AddRange(BitConverter.GetBytes((ushort)signature));added.Add(new(name,0x0A000000+rows+added.Count+1,((coded&7)==1?0x01000000:0x1B000000)+(coded>>3),ni,signature));}
  byte[] expanded=table[..insert].Concat(add).Concat(table[insert..]).ToArray();BitConverter.GetBytes(rows+added.Count).CopyTo(expanded,countPos);ulong sorted=BitConverter.ToUInt64(expanded,16);BitConverter.GetBytes(sorted&~(1UL<<10)).CopyTo(expanded,16);streams[ti]=new("#~",expanded);streams[streams.FindIndex(s=>s.Name=="#Strings")]=new("#Strings",stringStream);
  var nr=new List<byte>(root[..headEnd]);scan=countAt+2;foreach(var s in streams){while(nr.Count%4!=0)nr.Add(0);BitConverter.GetBytes(nr.Count).CopyTo(nr,scan);BitConverter.GetBytes(s.Data.Length).CopyTo(nr,scan+4);nr.AddRange(s.Data);int e=Array.IndexOf(root,(byte)0,scan+8);scan=Align(e+1,4);}byte[] meta=nr.ToArray();
  // The qualified Native29 metadata section is already the last section.
  // Its header has no room for another section header. Extend that read-only
  // section at EOF while retaining all existing bytes, raw starts and RVAs.
  var ph=pe.PEHeaders;var opt=ph.PEHeader!;R(opt.Magic==PEMagic.PE32&&opt.CertificateTableDirectory.Size==0&&ph.CorHeader.StrongNameSignatureDirectory.Size==0&&(ph.CorHeader.Flags&CorFlags.StrongNameSigned)==0,"Unsupported signed/input layout");var last=ph.SectionHeaders.Last();R(last.Name==".n29meta"&&last.PointerToRawData+last.SizeOfRawData==input.Length&&last.VirtualSize<=last.SizeOfRawData&&last.SectionCharacteristics==(SectionCharacteristics)0x40000040,"Pinned extensible metadata section");R(rootOffset>=last.PointerToRawData&&rootOffset+rootSize<=input.Length,"Parent metadata in last section");int header=ph.PEHeaderStartOffset+ph.CoffHeader.SizeOfOptionalHeader+(ph.CoffHeader.NumberOfSections-1)*40;int raw=Align(input.Length,opt.FileAlignment),va=last.VirtualAddress+raw-last.PointerToRawData,append=Align(meta.Length,opt.FileAlignment);var output=new byte[raw+append];input.CopyTo(output,0);meta.CopyTo(output,raw);int newVirtual=raw-last.PointerToRawData+meta.Length,newRaw=output.Length-last.PointerToRawData;
  BitConverter.GetBytes(newVirtual).CopyTo(output,header+8);BitConverter.GetBytes(newRaw).CopyTo(output,header+16);BitConverter.GetBytes(opt.SizeOfInitializedData+output.Length-input.Length).CopyTo(output,ph.PEHeaderStartOffset+8);BitConverter.GetBytes(Align(last.VirtualAddress+newVirtual,opt.SectionAlignment)).CopyTo(output,ph.PEHeaderStartOffset+56);new byte[4].CopyTo(output,ph.PEHeaderStartOffset+64);BitConverter.GetBytes(va).CopyTo(output,ph.CorHeaderStartOffset+8);BitConverter.GetBytes(meta.Length).CopyTo(output,ph.CorHeaderStartOffset+12);
  return(output,new{old_root_offset=rootOffset,old_root_size=rootSize,new_root_offset=raw,new_root_size=meta.Length,extended_section_header=header,old_virtual_size=last.VirtualSize,new_virtual_size=newVirtual,old_raw_size=last.SizeOfRawData,new_raw_size=newRaw,added,table_sorted_bit_cleared=(sorted&(1UL<<10))!=0,existing_heap_prefixes_unchanged=true,string_heap_added_bytes=stringStream.Length-originalStringSize});

 }
 static void CopyTo(this byte[] b,List<byte> list,int p){for(int j=0;j<b.Length;j++)list[p+j]=b[j];}
 static int Main(string[] args){try{Run(args);return 0;}catch(Exception e){Console.Error.WriteLine(e);return 3;}}
 static void Run(string[] args){R(args.Length==3,"input new-output report");R(!File.Exists(args[1])&&!File.Exists(args[2]),"Fresh outputs required");byte[]before=File.ReadAllBytes(args[0]);R(H(before)=="ced83247cabd24bf908b78720a914c1c5a1cc3c9be1689c8868db38033f144d4","Pinned Native48 input");using var asm=Mono.Cecil.AssemblyDefinition.ReadAssembly(args[0]);using var pe=new PEReader(new MemoryStream(before));var(after,metadata)=Metadata(before,pe,asm.MainModule);int first=0x0A000000+pe.GetMetadataReader().GetTableRowCount(TableIndex.MemberRef)+1;
  var m=(MD)asm.MainModule.LookupToken(0x06000219);R(m.IsStatic&&m.Body.ExceptionHandlers.Count==0&&m.Body.CodeSize==340&&m.Body.MaxStackSize==3,"Pinned target body");var ins=m.Body.Instructions;
  var size=ins.Single(i=>i.Operand is FR f&&f.Name=="_size"&&f.DeclaringType.FullName=="System.Collections.Generic.List`1<Zombie_charred>");R(size.Offset==9&&size.OpCode==OpCodes.Ldfld,"Pinned size site");
  var zeros=ins.Select((i,k)=>(i,k)).Where(x=>x.i.OpCode==OpCodes.Ldc_I4&&(int)x.i.Operand==0&&x.k>0&&ins[x.k-1].OpCode==OpCodes.Ldsfld&&ins[x.k-1].Operand is FR f&&f.Name=="zombie_Charreds"&&ins[x.k+1].OpCode==OpCodes.Ceq).Select(x=>x.i).ToArray();R(zeros.Select(i=>i.Offset).SequenceEqual(new[]{246,276,315}),"Three null sites");
  var terminal=ins.Select((i,k)=>(i,k)).Single(x=>x.i.OpCode==OpCodes.Ret&&x.k>0&&ins[x.k-1].Operand is VariableDefinition v&&v.VariableType.FullName=="System.NullReferenceException").i;R(terminal.Offset==235,"NRE-only terminal");
  var sec=pe.PEHeaders.SectionHeaders.Single(s=>m.RVA>=s.VirtualAddress&&m.RVA<s.VirtualAddress+s.SizeOfRawData);int h=sec.PointerToRawData+m.RVA-sec.VirtualAddress;R((before[h]&3)==3&&(BitConverter.ToUInt16(before,h)>>12)==3,"Fat header");int start=h+12;after[start+size.Offset]=0x6f;BitConverter.GetBytes(first).CopyTo(after,start+size.Offset+1);foreach(var i in zeros){after[start+i.Offset]=0x14;Array.Clear(after,start+i.Offset+1,4);}after[start+terminal.Offset]=0x7a;
  var row=new{token="0x06000219",method=m.FullName,header=h,code_start=start,reserved_code_size=340,new_code_size=340,old_max=3,new_max=3,local_sig=m.Body.LocalVarToken.ToInt32(),code_sha256=H(after[start..(start+340)]),count_call_offset=size.Offset,null_offsets=zeros.Select(i=>i.Offset).ToArray(),exception_return_offset=terminal.Offset};
  File.WriteAllBytes(args[1],after);File.WriteAllText(args[2],JsonSerializer.Serialize(new{input_sha256=H(before),candidate_sha256=H(after),metadata,methods=new[]{row},scope="One resource accessor: public closed List<Zombie_charred>.Count binding, three exact null constants and one native NRE terminal. Other target instructions/header/locals and every non-target body preserved. Original index-zero behavior retained."},new JsonSerializerOptions{WriteIndented=true}));Console.WriteLine("CANDIDATE "+H(after));
 }
}
