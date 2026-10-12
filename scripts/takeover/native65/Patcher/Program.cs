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
static class RepairNative65 {
 static void R(bool x,string m){if(!x)throw new InvalidOperationException(m);}
 static string H(byte[] b)=>Convert.ToHexString(SHA256.HashData(b)).ToLowerInvariant();
 static int Align(int x,int a)=>checked((x+a-1)/a*a);
 class IL {
  public List<byte> B=new();Dictionary<string,int> labels=new();List<(int p,string l,int width)> branches=new();
  public IL O(params byte[] x){B.AddRange(x);return this;}public IL I(int x){B.AddRange(BitConverter.GetBytes(x));return this;}
  public IL Ref(byte op,IMetadataTokenProvider r)=>O(op).I(r.MetadataToken.ToInt32());public IL Ref(byte op,int t)=>O(op).I(t);
  public IL Int(int x)=>x>=-1&&x<=8?O((byte)(x+0x16)):O(0x20).I(x);public IL Float(float x)=>O(0x22).I(BitConverter.SingleToInt32Bits(x));
  public IL L(int i)=>i<4?O((byte)(6+i)):O(0x11,(byte)i);public IL S(int i)=>i<4?O((byte)(10+i)):O(0x13,(byte)i);public IL A(int i)=>O(0x12,(byte)i);
  public IL Arg(int i)=>i<4?O((byte)(2+i)):O(0x0E,(byte)i);
  public IL Label(string s){labels.Add(s,B.Count);return this;}public IL Branch(byte op,string s){O(op);int width=op>=0x38&&op<=0x44?4:1;branches.Add((B.Count,s,width));if(width==4)I(0);else O(0);return this;}
  public byte[] Done(){foreach(var(p,l,width)in branches){int d=labels[l]-p-width;if(width==4)BitConverter.GetBytes(d).CopyTo(B,p);else{R(d>=-128&&d<=127,"Short branch overflow");B[p]=unchecked((byte)(sbyte)d);}}return B.ToArray();}
 }
 record Stream(string Name,byte[] Data);
 record Added(string Name,int Token,int Parent,int NameIndex,int SignatureIndex);
 static (byte[] Image,object Evidence) Metadata(byte[] input,PEReader pe,Mono.Cecil.ModuleDefinition module){
  var md=pe.GetMetadataReader();int rootRva=pe.PEHeaders.CorHeader!.MetadataDirectory.RelativeVirtualAddress;var section=pe.PEHeaders.SectionHeaders.Single(s=>rootRva>=s.VirtualAddress&&rootRva<s.VirtualAddress+s.SizeOfRawData);int rootOffset=section.PointerToRawData+rootRva-section.VirtualAddress;int rootSize=pe.PEHeaders.CorHeader.MetadataDirectory.Size;byte[] root=input[rootOffset..(rootOffset+rootSize)];R(BitConverter.ToUInt32(root,0)==0x424A5342,"Metadata root");int vlen=BitConverter.ToInt32(root,12);int countAt=Align(16+vlen,4)+2;int count=BitConverter.ToUInt16(root,countAt);int p=countAt+2;var streams=new List<Stream>();
  for(int i=0;i<count;i++){int off=BitConverter.ToInt32(root,p),len=BitConverter.ToInt32(root,p+4);int end=Array.IndexOf(root,(byte)0,p+8);R(end>=0,"Stream name");string name=Encoding.ASCII.GetString(root,p+8,end-p-8);streams.Add(new(name,root[off..(off+len)]));p=Align(end+1,4);}int headEnd=p;
  int ti=streams.FindIndex(s=>s.Name=="#~");R(ti>=0,"Compressed table stream required");var table=streams[ti].Data;var stringStream=streams.Single(s=>s.Name=="#Strings").Data;var blobStream=streams.Single(s=>s.Name=="#Blob").Data;R((table[6]&3)==0,"Pinned 2-byte string/blob indexes");int rows=md.GetTableRowCount(TableIndex.MemberRef);int rowSize=md.GetTableRowSize(TableIndex.MemberRef);R(rows>0&&rowSize==6,"Pinned MemberRef layout");R(md.GetTableRowCount(TableIndex.TypeSpec)<8192&&md.GetTableRowCount(TableIndex.TypeDef)<8192&&md.GetTableRowCount(TableIndex.TypeRef)<8192&&md.GetTableRowCount(TableIndex.ModuleRef)<8192&&md.GetTableRowCount(TableIndex.MethodDef)<8192,"Parent index width");
  int ts(string name){var hits=Enumerable.Range(1,md.GetTableRowCount(TableIndex.TypeSpec)).Select(i=>module.LookupToken(0x1B000000+i)).OfType<Mono.Cecil.TypeReference>().Where(t=>t.FullName==name).ToArray();R(hits.Length==1,"Exact TypeSpec "+name);return hits[0].MetadataToken.RID is uint r?checked((int)r):throw new Exception();}
  int originalStringSize=stringStream.Length,originalBlobSize=blobStream.Length;int nameIndex(string name){for(int i=0;i<stringStream.Length-name.Length;i++){if(Encoding.UTF8.GetString(stringStream,i,name.Length)==name&&stringStream[i+name.Length]==0)return i;}R(name=="set_sharedMaterials","Only the public materials setter name may append");int ix=stringStream.Length;stringStream=stringStream.Concat(Encoding.UTF8.GetBytes(name+"\0")).ToArray();R(stringStream.Length<65536,"Unchanged string index width");return ix;}
  int sig(byte[] desired){foreach(var h in md.MemberReferences){var m=md.GetMemberReference(h);if(md.GetBlobBytes(m.Signature).SequenceEqual(desired))return MetadataTokens.GetHeapOffset(m.Signature);}R(desired.Length<128&&desired[0]==0x20,"Bounded instance signature append");int ix=blobStream.Length;blobStream=blobStream.Concat(new[]{(byte)desired.Length}).Concat(desired).ToArray();R(blobStream.Length<65536,"Unchanged blob index width");return ix;}
  var specs=new[]{("Add",ts("System.Collections.Generic.List`1<Popup>")*8+4,sig(new byte[]{0x20,1,1,0x13,0}))};
  ulong valid=BitConverter.ToUInt64(table,8);R((valid&(1UL<<10))!=0,"MemberRef missing");int countPos=24+4*Enumerable.Range(0,10).Count(i=>(valid&(1UL<<i))!=0);R(BitConverter.ToInt32(table,countPos)==rows,"Row count pin");int tableStart=md.GetTableMetadataOffset(TableIndex.MemberRef);int streamRootOffset=Array.FindIndex(root,_=>false); // replaced with exact stream offset below
  int scan=countAt+2;for(int i=0;i<count;i++){int e=Array.IndexOf(root,(byte)0,scan+8);if(Encoding.ASCII.GetString(root,scan+8,e-scan-8)=="#~")streamRootOffset=BitConverter.ToInt32(root,scan);scan=Align(e+1,4);}R(streamRootOffset>=0,"Table stream root offset");int insert=tableStart-streamRootOffset+rows*rowSize;R(insert>=24&&insert<=table.Length,"MemberRef table end");var add=new List<byte>();var added=new List<Added>();foreach(var(name,parent,signature)in specs){int ni=nameIndex(name);R(ni<65536&&signature<65536&&parent<65536,"Index overflow");int coded=parent;add.AddRange(BitConverter.GetBytes((ushort)coded));add.AddRange(BitConverter.GetBytes((ushort)ni));add.AddRange(BitConverter.GetBytes((ushort)signature));added.Add(new(name,0x0A000000+rows+added.Count+1,((coded&7)==1?0x01000000:0x1B000000)+(coded>>3),ni,signature));}
  byte[] expanded=table[..insert].Concat(add).Concat(table[insert..]).ToArray();BitConverter.GetBytes(rows+added.Count).CopyTo(expanded,countPos);ulong sorted=BitConverter.ToUInt64(expanded,16);BitConverter.GetBytes(sorted&~(1UL<<10)).CopyTo(expanded,16);streams[ti]=new("#~",expanded);streams[streams.FindIndex(s=>s.Name=="#Blob")]=new("#Blob",blobStream);streams[streams.FindIndex(s=>s.Name=="#Strings")]=new("#Strings",stringStream);
  var nr=new List<byte>(root[..headEnd]);scan=countAt+2;foreach(var s in streams){while(nr.Count%4!=0)nr.Add(0);BitConverter.GetBytes(nr.Count).CopyTo(nr,scan);BitConverter.GetBytes(s.Data.Length).CopyTo(nr,scan+4);nr.AddRange(s.Data);int e=Array.IndexOf(root,(byte)0,scan+8);scan=Align(e+1,4);}byte[] meta=nr.ToArray();
  // The qualified Native29 metadata section is already the last section.
  // Its header has no room for another section header. Extend that read-only
  // section at EOF while retaining all existing bytes, raw starts and RVAs.
  var ph=pe.PEHeaders;var opt=ph.PEHeader!;R(opt.Magic==PEMagic.PE32&&opt.CertificateTableDirectory.Size==0&&ph.CorHeader.StrongNameSignatureDirectory.Size==0&&(ph.CorHeader.Flags&CorFlags.StrongNameSigned)==0,"Unsupported signed/input layout");var last=ph.SectionHeaders.Last();R(last.Name==".n29meta"&&last.PointerToRawData+last.SizeOfRawData==input.Length&&last.VirtualSize<=last.SizeOfRawData&&last.SectionCharacteristics==(SectionCharacteristics)0x40000040,"Pinned extensible metadata section");R(rootOffset>=last.PointerToRawData&&rootOffset+rootSize<=input.Length,"Parent metadata in last section");int header=ph.PEHeaderStartOffset+ph.CoffHeader.SizeOfOptionalHeader+(ph.CoffHeader.NumberOfSections-1)*40;int raw=Align(input.Length,opt.FileAlignment),va=last.VirtualAddress+raw-last.PointerToRawData,append=Align(meta.Length,opt.FileAlignment);var output=new byte[raw+append];input.CopyTo(output,0);meta.CopyTo(output,raw);int newVirtual=raw-last.PointerToRawData+meta.Length,newRaw=output.Length-last.PointerToRawData;
  BitConverter.GetBytes(newVirtual).CopyTo(output,header+8);BitConverter.GetBytes(newRaw).CopyTo(output,header+16);BitConverter.GetBytes(opt.SizeOfInitializedData+output.Length-input.Length).CopyTo(output,ph.PEHeaderStartOffset+8);BitConverter.GetBytes(Align(last.VirtualAddress+newVirtual,opt.SectionAlignment)).CopyTo(output,ph.PEHeaderStartOffset+56);new byte[4].CopyTo(output,ph.PEHeaderStartOffset+64);BitConverter.GetBytes(va).CopyTo(output,ph.CorHeaderStartOffset+8);BitConverter.GetBytes(meta.Length).CopyTo(output,ph.CorHeaderStartOffset+12);
  return(output,new{old_root_offset=rootOffset,old_root_size=rootSize,new_root_offset=raw,new_root_size=meta.Length,extended_section_header=header,old_virtual_size=last.VirtualSize,new_virtual_size=newVirtual,old_raw_size=last.SizeOfRawData,new_raw_size=newRaw,added,blob_heap_added_bytes=blobStream.Length-originalBlobSize,table_sorted_bit_cleared=(sorted&(1UL<<10))!=0,existing_heap_prefixes_unchanged=true,string_heap_added_bytes=stringStream.Length-originalStringSize});

 }
 static void CopyTo(this byte[] b,List<byte> list,int p){for(int j=0;j<b.Length;j++)list[p+j]=b[j];}
 static int Main(string[] args){try{Run(args);return 0;}catch(Exception e){Console.Error.WriteLine(e);return 3;}}
 static void Run(string[] args){
  R(args.Length==3&&!File.Exists(args[1])&&!File.Exists(args[2]),"Fresh outputs required");
  byte[] before=File.ReadAllBytes(args[0]);R(H(before)=="4727f5a22eaed8f043caeba9f6b2c121b0f9051c93a5f6604a27e6a46fda2165","Pinned untested Native64 parent");
  using var asm=Mono.Cecil.AssemblyDefinition.ReadAssembly(args[0]);var module=asm.MainModule;using var fs=File.OpenRead(args[0]);using var pe=new PEReader(fs);
  var(after,metadata)=Metadata(before,pe,module);int first=0x0A000000+pe.GetMetadataReader().GetTableRowCount(TableIndex.MemberRef)+1;
  MD M(int t)=>(MD)module.LookupToken(t);
  FR F(string owner,string name)=>module.GetTypes().Single(t=>t.FullName==owner).Fields.Single(f=>f.Name==name);
  MR C(MD m,string name)=>m.Body.Instructions.Select(i=>i.Operand).OfType<MR>().First(c=>c.Name==name);
  MR Exact(string name)=>module.GetMemberReferences().OfType<MR>().Single(m=>m.FullName==name);
  var targets=new Dictionary<int,(byte[] code,int max)>();var report=new List<object>();void Set(int token,IL il,int max){targets.Add(token,(il.Done(),max));}
  var m=M(0x060002CC);
  var il=new IL().Arg(1).Ref(0x6F,Exact("UnityEngine.Transform UnityEngine.Component::get_transform()"))
   .Arg(0).Ref(0x7B,F("Board","WindowsUI")).Ref(0x6F,Exact("UnityEngine.Transform UnityEngine.GameObject::get_transform()"))
   .Int(0).Ref(0x6F,C(m,"SetParent"))
   .Arg(1).Ref(0x6F,C(m,"get_gameObject")).Int(0).Ref(0x6F,C(m,"SetActive"))
   .Arg(0).Ref(0x7B,F("Board","popups")).Arg(1).Ref(0x6F,first).O(0x2A);
  Set(0x060002CC,il,3);
  foreach(int token in new[]{0x060008C3,0x060008C9,0x060008CF}){
   m=M(token);string owner=m.DeclaringType.FullName;
   var handler=m.Body.Instructions.Where(i=>i.OpCode==OpCodes.Ldftn).Select(i=>i.Operand).OfType<MR>().DistinctBy(r=>r.MetadataToken).Single();
   R(handler.DeclaringType.FullName==owner&&handler.Parameters.Count==1&&handler.Parameters[0].ParameterType.FullName=="FTRuntime.SwfClipController","Existing exact callback");
   var delegateCtor=m.Body.Instructions.Where(i=>i.OpCode==OpCodes.Newobj).Select(i=>i.Operand).OfType<MR>().First(r=>r.DeclaringType.FullName=="System.Action`1<FTRuntime.SwfClipController>");
   il=new IL().Arg(0).Ref(0x6F,C(m,"Unsubscribe")).Arg(1).Ref(0x28,C(m,"op_Implicit")).Branch(0x2C,"return")
    .Arg(0).Arg(1).Ref(0x7D,F(owner,"_waitCtrl"));
   string[] events=token==0x060008C9?new[]{"add_OnStopPlayingEvent","add_OnRewindPlayingEvent"}:new[]{token==0x060008C3?"add_OnRewindPlayingEvent":"add_OnStopPlayingEvent"};
   foreach(string name in events)il.Arg(1).Arg(0).O(0xFE,0x06).I(handler.MetadataToken.ToInt32()).Ref(0x73,delegateCtor).Ref(0x6F,C(m,name));
   il.Label("return").Arg(0).O(0x2A);Set(token,il,3);
  }
  foreach(var(token,(code,max))in targets){m=M(token);R(m.Body.ExceptionHandlers.Count==0,"EH relocation unsupported");var sec=pe.PEHeaders.SectionHeaders.Single(s=>m.RVA>=s.VirtualAddress&&m.RVA<s.VirtualAddress+s.SizeOfRawData);int h=sec.PointerToRawData+m.RVA-sec.VirtualAddress;int hs=(BitConverter.ToUInt16(before,h)>>12)*4;R((before[h]&3)==3&&hs==12,"Pinned fat body");R(code.Length<=m.Body.CodeSize,"Body exceeds reserved slot "+m.FullName);Array.Clear(after,h+hs,m.Body.CodeSize);code.CopyTo(after,h+hs);BitConverter.GetBytes(code.Length).CopyTo(after,h+4);BitConverter.GetBytes((ushort)max).CopyTo(after,h+2);report.Add(new{token=$"0x{token:X8}",method=m.FullName,header=h,code_start=h+hs,reserved_code_size=m.Body.CodeSize,new_code_size=code.Length,old_max=m.Body.MaxStackSize,new_max=max,local_sig=m.Body.LocalVarToken.ToInt32(),code_sha256=H(code)});}
  R(targets.Count==4,"Pinned scope 4");File.WriteAllBytes(args[1],after);File.WriteAllText(args[2],JsonSerializer.Serialize(new{status="UNTESTED_CANDIDATE",input_sha256=H(before),candidate_sha256=H(after),metadata,methods=report,scope="Four native-backed popup/event subscriptions; one public List<Popup>.Add binding appended; existing callbacks and atomic event accessors reused; parent chain pending"},new JsonSerializerOptions{WriteIndented=true}));Console.WriteLine("CANDIDATE "+H(after));
 }
}
