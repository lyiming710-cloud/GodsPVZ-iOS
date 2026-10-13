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
static class RepairNative81 {
 static void R(bool x,string m){if(!x)throw new InvalidOperationException(m);}
 static string H(byte[] b)=>Convert.ToHexString(SHA256.HashData(b)).ToLowerInvariant();
 static int Align(int x,int a)=>checked((x+a-1)/a*a);
 class IL {
  public List<byte> B=new();Dictionary<string,int> labels=new();List<(int p,string l,int width)> branches=new();List<(int p,int end,string l)> switches=new();
  public IL O(params byte[] x){B.AddRange(x);return this;}public IL I(int x){B.AddRange(BitConverter.GetBytes(x));return this;}
  public IL Ref(byte op,IMetadataTokenProvider r)=>O(op).I(r.MetadataToken.ToInt32());public IL Ref(byte op,int t)=>O(op).I(t);
  public IL Int(int x)=>x>=-1&&x<=8?O((byte)(x+0x16)):O(0x20).I(x);public IL Float(float x)=>O(0x22).I(BitConverter.SingleToInt32Bits(x));
  public IL L(int i)=>i<4?O((byte)(6+i)):O(0x11,(byte)i);public IL S(int i)=>i<4?O((byte)(10+i)):O(0x13,(byte)i);public IL A(int i)=>O(0x12,(byte)i);
  public IL Arg(int i)=>i<4?O((byte)(2+i)):O(0x0E,(byte)i);
  public IL Label(string s){labels.Add(s,B.Count);return this;}public IL Branch(byte op,string s){O(op);int width=op>=0x38&&op<=0x44?4:1;branches.Add((B.Count,s,width));if(width==4)I(0);else O(0);return this;}
  public IL Switch(params string[] names){O(0x45).I(names.Length);int end=B.Count+4*names.Length;foreach(var name in names){switches.Add((B.Count,end,name));I(0);}return this;}
  public byte[] Done(){foreach(var(p,end,l)in switches)BitConverter.GetBytes(labels[l]-end).CopyTo(B,p);foreach(var(p,l,width)in branches){int d=labels[l]-p-width;if(width==4)BitConverter.GetBytes(d).CopyTo(B,p);else{R(d>=-128&&d<=127,"Short branch overflow");B[p]=unchecked((byte)(sbyte)d);}}return B.ToArray();}
 }
 record Stream(string Name,byte[] Data);
 record Added(string Name,int Token,int Parent,int NameIndex,int SignatureIndex);
 static (byte[] Image,object Evidence) Metadata(byte[] input,PEReader pe,Mono.Cecil.ModuleDefinition module){
  var md=pe.GetMetadataReader();int rootRva=pe.PEHeaders.CorHeader!.MetadataDirectory.RelativeVirtualAddress;var section=pe.PEHeaders.SectionHeaders.Single(s=>rootRva>=s.VirtualAddress&&rootRva<s.VirtualAddress+s.SizeOfRawData);int rootOffset=section.PointerToRawData+rootRva-section.VirtualAddress;int rootSize=pe.PEHeaders.CorHeader.MetadataDirectory.Size;byte[] root=input[rootOffset..(rootOffset+rootSize)];R(BitConverter.ToUInt32(root,0)==0x424A5342,"Metadata root");int vlen=BitConverter.ToInt32(root,12);int countAt=Align(16+vlen,4)+2;int count=BitConverter.ToUInt16(root,countAt);int p=countAt+2;var streams=new List<Stream>();
  for(int i=0;i<count;i++){int off=BitConverter.ToInt32(root,p),len=BitConverter.ToInt32(root,p+4);int end=Array.IndexOf(root,(byte)0,p+8);R(end>=0,"Stream name");string name=Encoding.ASCII.GetString(root,p+8,end-p-8);streams.Add(new(name,root[off..(off+len)]));p=Align(end+1,4);}int headEnd=p;
  int ti=streams.FindIndex(s=>s.Name=="#~");R(ti>=0,"Compressed tables required");
  var table=streams[ti].Data;var strings=streams.Single(s=>s.Name=="#Strings").Data;var blob=streams.Single(s=>s.Name=="#Blob").Data;
  int oldStrings=strings.Length,oldBlob=blob.Length;R((table[6]&3)==0,"Existing two-byte heaps");
  byte[] Compress(int v){R(v>=0&&v<16384,"Existing compressed index width");return v<128?new[]{(byte)v}:new[]{(byte)(0x80|(v>>8)),(byte)v};}
  int Blob(byte[] desired){
   foreach(var h in md.MemberReferences){var r=md.GetMemberReference(h);if(md.GetBlobBytes(r.Signature).SequenceEqual(desired))return MetadataTokens.GetHeapOffset(r.Signature);}
   R(desired.Length<128,"Bounded new signature");int ix=blob.Length;blob=blob.Concat(new[]{(byte)desired.Length}).Concat(desired).ToArray();R(blob.Length<65536,"No blob width growth");return ix;
  }
  int Name(string name){byte[] raw=Encoding.UTF8.GetBytes(name+"\0");for(int i=0;i+raw.Length<=strings.Length;i++)if(strings[i..(i+raw.Length)].SequenceEqual(raw))return i;int ix=strings.Length;strings=strings.Concat(raw).ToArray();R(strings.Length<65536,"No string heap width growth");return ix;}
  byte[] Words(params int[] values)=>values.SelectMany(v=>{R(v>=0&&v<65536,"Two-byte row index");return BitConverter.GetBytes((ushort)v);}).ToArray();
  int TS(string name){var hits=Enumerable.Range(1,md.GetTableRowCount(TableIndex.TypeSpec)).Select(i=>module.LookupToken(0x1B000000+i)).OfType<Mono.Cecil.TypeReference>().Where(t=>t.FullName==name).ToArray();R(hits.Length==1,"Exact existing list TypeSpec: "+name);return checked((int)hits[0].MetadataToken.RID);}
  var memberRows=new List<byte>();var localRows=new List<byte>();var added=new List<object>();
  int localFirst=md.GetTableRowCount(TableIndex.StandAloneSig)+1,memberFirst=md.GetTableRowCount(TableIndex.MemberRef)+1;
  int Member(string name,int parent,byte[] signature){int rid=memberFirst+memberRows.Count/6,ni=Name(name),si=Blob(signature);memberRows.AddRange(Words(parent,ni,si));added.Add(new{kind="MemberRef",token=$"0x{0x0A000000+rid:X8}",name,parent,name_index=ni,signature_index=si});return 0x0A000000+rid;}
  var enumType=module.GetMemberReferences().OfType<MR>().Single(r=>r.FullName=="System.Void System.Collections.Generic.List`1/Enumerator<ZombieSelect>::Dispose()").DeclaringType;
  int enumRid=TS(enumType.FullName),listRid=TS("System.Collections.Generic.List`1<ZombieSelect>");
  var enumeratorRef=module.GetTypeReferences().Single(t=>t.FullName=="System.Collections.Generic.List`1/Enumerator");
  byte[] openEnum=new byte[]{0x15,0x11}.Concat(Compress(checked((int)enumeratorRef.MetadataToken.RID*4+1))).Concat(new byte[]{1,0x13,0}).ToArray();
  Member("GetEnumerator",listRid*8+4,new byte[]{0x20,0}.Concat(openEnum).ToArray());
  Member("MoveNext",enumRid*8+4,new byte[]{0x20,0,2});
  Member("get_Current",enumRid*8+4,new byte[]{0x20,0,0x13,0});
  var method=(MD)module.LookupToken(0x060001AC);byte[] old=md.GetBlobBytes(md.GetStandaloneSignature(MetadataTokens.StandaloneSignatureHandle((int)method.Body.LocalVarToken.RID)).Signature);
  R(old[0]==7&&old[1]==16&&method.Body.Variables.Count==16,"Pinned damaged random-selector local count");
  byte[] enumSig=md.GetBlobBytes(md.GetTypeSpecification(MetadataTokens.TypeSpecificationHandle(enumRid)).Signature);R(enumSig[0]==0x15&&enumSig[1]==0x11,"Closed value-type ZombieSelect enumerator");
  byte[] sig=new byte[]{7,17}.Concat(old[2..]).Concat(enumSig).ToArray();int ix=Blob(sig);localRows.AddRange(Words(ix));
  added.Add(new{kind="StandAloneSig",token=$"0x{0x11000000+localFirst:X8}",target="0x060001AC",active_local=16,signature_index=ix,old_signature_hex=Convert.ToHexString(old),signature_hex=Convert.ToHexString(sig)});
  R(memberRows.Count==18&&localRows.Count==2,"Three typed public iterator bindings and one appended typed enumerator local");
  var additions=new Dictionary<TableIndex,byte[]>{{TableIndex.MemberRef,memberRows.ToArray()},{TableIndex.StandAloneSig,localRows.ToArray()}};
  ulong valid=BitConverter.ToUInt64(table,8);int tableStream=0,scan2=countAt+2;
  for(int i=0;i<count;i++){int e=Array.IndexOf(root,(byte)0,scan2+8);if(Encoding.ASCII.GetString(root,scan2+8,e-scan2-8)=="#~")tableStream=BitConverter.ToInt32(root,scan2);scan2=Align(e+1,4);}
  R(tableStream>0,"Existing tables offset");var expanded=table.ToList();
  foreach(var(t,bytes)in additions.OrderByDescending(x=>x.Key)){R((valid&(1UL<<(int)t))!=0,"Existing table required");int width=md.GetTableRowSize(t);R(width==(t==TableIndex.MemberRef?6:2),"Pinned row width");int end=md.GetTableMetadataOffset(t)-tableStream+md.GetTableRowCount(t)*width;expanded.InsertRange(end,bytes);}
  var tables=expanded.ToArray();foreach(var(t,bytes)in additions){int at=24+4*Enumerable.Range(0,(int)t).Count(i=>(valid&(1UL<<i))!=0);BitConverter.GetBytes(md.GetTableRowCount(t)+bytes.Length/md.GetTableRowSize(t)).CopyTo(tables,at);}
  ulong sorted=BitConverter.ToUInt64(table,16),clear=(1UL<<(int)TableIndex.MemberRef)|(1UL<<(int)TableIndex.StandAloneSig);
  BitConverter.GetBytes(sorted&~clear).CopyTo(tables,16);streams[ti]=new("#~",tables);streams[streams.FindIndex(s=>s.Name=="#Blob")]=new("#Blob",blob);streams[streams.FindIndex(s=>s.Name=="#Strings")]=new("#Strings",strings);
  var nr=new List<byte>(root[..headEnd]);int scan=countAt+2;foreach(var s in streams){while(nr.Count%4!=0)nr.Add(0);BitConverter.GetBytes(nr.Count).CopyTo(nr,scan);BitConverter.GetBytes(s.Data.Length).CopyTo(nr,scan+4);nr.AddRange(s.Data);int e=Array.IndexOf(root,(byte)0,scan+8);scan=Align(e+1,4);}byte[] meta=nr.ToArray();
  // The qualified Native29 metadata section is already the last section.
  // Its header has no room for another section header. Extend that read-only
  // section at EOF while retaining all existing bytes, raw starts and RVAs.
  var ph=pe.PEHeaders;var opt=ph.PEHeader!;R(opt.Magic==PEMagic.PE32&&opt.CertificateTableDirectory.Size==0&&ph.CorHeader.StrongNameSignatureDirectory.Size==0&&(ph.CorHeader.Flags&CorFlags.StrongNameSigned)==0,"Unsupported signed/input layout");var last=ph.SectionHeaders.Last();R(last.Name==".n29meta"&&last.PointerToRawData+last.SizeOfRawData==input.Length&&last.VirtualSize<=last.SizeOfRawData&&last.SectionCharacteristics==(SectionCharacteristics)0x40000040,"Pinned extensible metadata section");R(rootOffset>=last.PointerToRawData&&rootOffset+rootSize<=input.Length,"Parent metadata in last section");int header=ph.PEHeaderStartOffset+ph.CoffHeader.SizeOfOptionalHeader+(ph.CoffHeader.NumberOfSections-1)*40;int raw=Align(input.Length,opt.FileAlignment),va=last.VirtualAddress+raw-last.PointerToRawData,append=Align(meta.Length,opt.FileAlignment);var output=new byte[raw+append];input.CopyTo(output,0);meta.CopyTo(output,raw);int newVirtual=raw-last.PointerToRawData+meta.Length,newRaw=output.Length-last.PointerToRawData;
  BitConverter.GetBytes(newVirtual).CopyTo(output,header+8);BitConverter.GetBytes(newRaw).CopyTo(output,header+16);BitConverter.GetBytes(opt.SizeOfInitializedData+output.Length-input.Length).CopyTo(output,ph.PEHeaderStartOffset+8);BitConverter.GetBytes(Align(last.VirtualAddress+newVirtual,opt.SectionAlignment)).CopyTo(output,ph.PEHeaderStartOffset+56);new byte[4].CopyTo(output,ph.PEHeaderStartOffset+64);BitConverter.GetBytes(va).CopyTo(output,ph.CorHeaderStartOffset+8);BitConverter.GetBytes(meta.Length).CopyTo(output,ph.CorHeaderStartOffset+12);
  return(output,new{old_root_offset=rootOffset,old_root_size=rootSize,new_root_offset=raw,new_root_size=meta.Length,extended_section_header=header,old_virtual_size=last.VirtualSize,new_virtual_size=newVirtual,old_raw_size=last.SizeOfRawData,new_raw_size=newRaw,added,blob_heap_added_bytes=blob.Length-oldBlob,table_sorted_bit_cleared=(sorted&(1UL<<10))!=0,existing_heap_prefixes_unchanged=true,string_heap_added_bytes=strings.Length-oldStrings});

 }
 static (byte[] Image,object Evidence) AppendBody(byte[] current,MD method,byte[] code,int max){
  using var pe=new PEReader(new MemoryStream(current));var ph=pe.PEHeaders;var opt=ph.PEHeader!;var md=pe.GetMetadataReader();var last=ph.SectionHeaders.Last();
  R(opt.Magic==PEMagic.PE32&&last.Name==".n29meta"&&last.PointerToRawData+last.SizeOfRawData==current.Length&&last.VirtualSize<=last.SizeOfRawData&&last.SectionCharacteristics==(SectionCharacteristics)0x40000040&&opt.CertificateTableDirectory.Size==0&&ph.CorHeader!.StrongNameSignatureDirectory.Size==0&&(ph.CorHeader.Flags&CorFlags.StrongNameSigned)==0,"Pinned unsigned EOF body relocation layout");
  int rootRva=ph.CorHeader.MetadataDirectory.RelativeVirtualAddress,root=checked(last.PointerToRawData+rootRva-last.VirtualAddress),rootSize=ph.CorHeader.MetadataDirectory.Size;
  R(root>=last.PointerToRawData&&checked(root+rootSize)<=current.Length,"Active metadata in extensible last section");
  int row=checked(md.GetTableMetadataOffset(TableIndex.MethodDef)+(checked((int)method.MetadataToken.RID)-1)*md.GetTableRowSize(TableIndex.MethodDef));
  R(row>=0&&checked(row+4)<=rootSize&&BitConverter.ToInt32(current,root+row)==method.RVA,"Pinned target RVA in active metadata row");
  R(method.Body.ExceptionHandlers.Count==0&&code.Length>method.Body.CodeSize,"Only oversize no-EH Project body is relocated");
  int body=Align(current.Length,4),rva=checked(last.VirtualAddress+body-last.PointerToRawData),end=checked(body+12+code.Length);R((rva&3)==0,"Fat MethodBody RVA alignment");
  var after=new byte[Align(end,opt.FileAlignment)];current.CopyTo(after,0);
  ushort flags=(ushort)(0x3003|(method.Body.InitLocals?0x10:0));BitConverter.GetBytes(flags).CopyTo(after,body);BitConverter.GetBytes(checked((ushort)max)).CopyTo(after,body+2);BitConverter.GetBytes(code.Length).CopyTo(after,body+4);BitConverter.GetBytes(method.Body.LocalVarToken.ToInt32()).CopyTo(after,body+8);code.CopyTo(after,body+12);
  BitConverter.GetBytes(rva).CopyTo(after,root+row);
  int sh=ph.PEHeaderStartOffset+ph.CoffHeader.SizeOfOptionalHeader+(ph.CoffHeader.NumberOfSections-1)*40,virtualSize=checked(end-last.PointerToRawData);
  BitConverter.GetBytes(virtualSize).CopyTo(after,sh+8);BitConverter.GetBytes(after.Length-last.PointerToRawData).CopyTo(after,sh+16);BitConverter.GetBytes(checked(opt.SizeOfInitializedData+after.Length-current.Length)).CopyTo(after,ph.PEHeaderStartOffset+8);BitConverter.GetBytes(Align(checked(last.VirtualAddress+virtualSize),opt.SectionAlignment)).CopyTo(after,ph.PEHeaderStartOffset+56);new byte[4].CopyTo(after,ph.PEHeaderStartOffset+64);
  return(after,new{old_rva=method.RVA,new_rva=rva,body_offset=body,active_root_offset=root,active_root_size=rootSize,method_row_offset_in_root=row,method_row_rva_file_offset=root+row,old_slot_preserved_by_construction=true,old_image_bytes=current.Length,new_image_bytes=after.Length,old_virtual_size=last.VirtualSize,new_virtual_size=virtualSize,old_raw_size=last.SizeOfRawData,new_raw_size=after.Length-last.PointerToRawData,metadata_directory_unchanged_by_construction=true});
 }
 static void CopyTo(this byte[] b,List<byte> list,int p){for(int j=0;j<b.Length;j++)list[p+j]=b[j];}
 static int Main(string[] args){try{Run(args);return 0;}catch(Exception e){Console.Error.WriteLine(e);return 3;}}
 static void Run(string[] args){
  R(args.Length==3&&!File.Exists(args[1])&&!File.Exists(args[2]),"Fresh outputs required");
  byte[] before=File.ReadAllBytes(args[0]);R(H(before)=="119c847000d84922e5089cfed3bb464366251aa05597f4c7fa22706368211950","Pinned untested Native80 parent");
  using var asm=Mono.Cecil.AssemblyDefinition.ReadAssembly(args[0]);var module=asm.MainModule;using var fs=File.OpenRead(args[0]);using var pe=new PEReader(fs);
  var(after,metadata)=Metadata(before,pe,module);int first=0x0A000000+pe.GetMetadataReader().GetTableRowCount(TableIndex.MemberRef)+1;
  MD M(int token)=>(MD)module.LookupToken(token);
  FR F(string owner,string name)=>module.GetTypes().Single(t=>t.FullName==owner).Fields.Single(f=>f.Name==name);
  MR C(MD method,string name)=>method.Body.Instructions.Select(i=>i.Operand).OfType<MR>().First(r=>r.Name==name);
  int localFirst=pe.GetMetadataReader().GetTableRowCount(TableIndex.StandAloneSig)+1;
  var targets=new Dictionary<int,(byte[] code,int max)>();var report=new List<object>();void Set(int token,IL il,int max){targets.Add(token,(il.Done(),max));}
  var random=module.GetMemberReferences().OfType<MR>().Single(r=>r.FullName=="System.Single UnityEngine.Random::Range(System.Single,System.Single)");
  var m=M(0x060001AC);R(m.Body.ExceptionHandlers.Count==0&&m.HasThis&&m.Parameters.Count==2&&m.Body.Variables[0].VariableType.FullName=="System.Single"&&m.Body.Variables[1].VariableType.FullName=="System.Collections.Generic.List`1<ZombieSelect>"&&m.Body.Variables[5].VariableType.FullName=="ZombieSelect"&&m.Body.Variables[11].VariableType.FullName=="ZombieSelect","Pinned selector signature and captured locals");
  var dispose=module.GetMemberReferences().OfType<MR>().Single(r=>r.FullName=="System.Void System.Collections.Generic.List`1/Enumerator<ZombieSelect>::Dispose()");
  var il=new IL().Arg(1).S(1).Float(0f).Arg(2).Ref(0x28,random).S(0).L(1).Ref(0x6F,first).S(16).O(0x14).S(11);int tryStart=il.B.Count;
  il.Label("loop").A(16).Ref(0x28,first+1).Branch(0x2C,"leave")
   .A(16).Ref(0x28,first+2).S(5).L(0).L(5).Ref(0x7B,F("ZombieSelect","weight")).Branch(0x30,"subtract")
   .L(5).S(11).Branch(0xDE,"return")
   .Label("subtract").L(0).L(5).Ref(0x7B,F("ZombieSelect","weight")).O(0x59,0x6B).S(0).Branch(0x2B,"loop")
   .Label("leave").Branch(0xDE,"return");int finallyStart=il.B.Count;
  il.A(16).Ref(0x28,dispose).O(0xDC);int finallyEnd=il.B.Count;il.Label("return").L(11).O(0x2A);byte[] enumCode=il.Done();
  var section=pe.PEHeaders.SectionHeaders.Single(x=>m.RVA>=x.VirtualAddress&&m.RVA<x.VirtualAddress+x.SizeOfRawData);int header=section.PointerToRawData+m.RVA-section.VirtualAddress;
  R((before[header]&3)==3&&(BitConverter.ToUInt16(before,header)>>12)==3,"Pinned fat selector header");int eh=Align(header+12+enumCode.Length,4),extent=eh+28-(header+12);
  R(extent<=m.Body.CodeSize,"Selector body/finally fits reserved slot");Array.Clear(after,header+12,m.Body.CodeSize);enumCode.CopyTo(after,header+12);
  BitConverter.GetBytes((ushort)(BitConverter.ToUInt16(before,header)|8)).CopyTo(after,header);BitConverter.GetBytes((ushort)2).CopyTo(after,header+2);BitConverter.GetBytes(enumCode.Length).CopyTo(after,header+4);int local=0x11000000+localFirst;BitConverter.GetBytes(local).CopyTo(after,header+8);
  after[eh]=0x41;after[eh+1]=28;foreach(var(offset,value)in new[]{(4,2),(8,tryStart),(12,finallyStart-tryStart),(16,finallyStart),(20,finallyEnd-finallyStart),(24,0)})BitConverter.GetBytes(value).CopyTo(after,eh+offset);
  report.Add(new{token="0x060001AC",method=m.FullName,header,code_start=header+12,reserved_code_size=m.Body.CodeSize,new_code_size=enumCode.Length,old_max=m.Body.MaxStackSize,new_max=2,old_local_sig=m.Body.LocalVarToken.ToInt32(),local_sig=local,code_sha256=H(enumCode),new_finally=new{section_offset=eh,try_start=tryStart,try_length=finallyStart-tryStart,handler_start=finallyStart,handler_length=finallyEnd-finallyStart}});
  m=M(0x060003C5);R(m.Body.Variables[3].VariableType.FullName=="UnityEngine.Vector3"&&m.Body.Variables[5].VariableType.FullName=="System.Single","Existing Project position/value scratch");
  var x=module.GetMemberReferences().OfType<FR>().Single(f=>f.FullName=="System.Single UnityEngine.Vector3::x");var y=module.GetMemberReferences().OfType<FR>().Single(f=>f.FullName=="System.Single UnityEngine.Vector3::y");
  var vector=module.GetMemberReferences().OfType<MR>().Single(r=>r.FullName=="System.Void UnityEngine.Vector3::.ctor(System.Single,System.Single,System.Single)");
  il=new IL().Arg(0).Ref(0x7B,F("Project","movementTracks")).Branch(0x2D,"check-four")
   .Arg(0).Ref(0x6F,C(m,"get_transform")).Ref(0x6F,C(m,"get_position")).S(3)
   .Arg(0).Ref(0x7C,F("Project","endPosition")).A(3).Ref(0x7B,x).Ref(0x7D,x)
   .Float(-380f).Float(120f).Ref(0x28,random).S(5)
   .Arg(0).Ref(0x7C,F("Project","endPosition")).L(5).Ref(0x7D,y)
   .Arg(0).Arg(0).Ref(0x7B,F("Project","endPosition")).Ref(0x28,C(m,"PositionRevision_Camera")).Ref(0x7D,F("Project","endPosition"))
   .Label("check-four").Arg(0).Ref(0x7B,F("Project","movementTracks")).Int(4).Branch(0x40,"return")
   .Arg(0).Ref(0x7B,F("Project","ID")).Switch("sun","coin","coin","diamond","fixed","fixed","return","return","coin").Branch(0x38,"return")
   .Label("sun").Arg(0).Arg(0).Ref(0x7B,F("Project","board")).Ref(0x7B,F("Board","banks")).Ref(0x6F,M(0x060000E9)).Ref(0x7D,F("Project","endPosition"))
   .Arg(0).Ref(0x7C,F("Project","endPosition")).O(0x25).Ref(0x7B,y).Float(12f).O(0x58,0x6B).Ref(0x7D,y).O(0x2A)
   .Label("coin").Arg(0).Arg(0).Ref(0x7B,F("Project","board")).Ref(0x7B,F("Board","projectManager")).Ref(0x6F,M(0x06000204)).Branch(0x2B,"bank-ready")
   .Label("diamond").Arg(0).Arg(0).Ref(0x7B,F("Project","board")).Ref(0x7B,F("Board","projectManager")).Ref(0x6F,M(0x06000205))
   .Label("bank-ready").Ref(0x7D,F("Project","endPosition"))
   .Arg(0).Ref(0x7C,F("Project","endPosition")).O(0x25).Ref(0x7B,y).Float(5f).O(0x59,0x6B).Ref(0x7D,y)
   .Arg(0).Ref(0x7C,F("Project","endPosition")).O(0x25).Ref(0x7B,x).Float(85f).O(0x59,0x6B).Ref(0x7D,x).O(0x2A)
   .Label("fixed").Arg(0).Float(0f).Float(115f).Float(0f).Ref(0x73,vector).Ref(0x7D,F("Project","endPosition"))
   .Arg(0).Arg(0).Ref(0x7B,F("Project","endPosition")).Ref(0x28,C(m,"PositionRevision_Camera")).Ref(0x7D,F("Project","endPosition")).O(0x2A)
   .Label("return").O(0x2A);
  byte[] projectCode=il.Done();var relocated=AppendBody(after,m,projectCode,4);after=relocated.Image;
  report.Add(new{token="0x060003C5",method=m.FullName,old_reserved_code_size=m.Body.CodeSize,new_code_size=projectCode.Length,old_max=m.Body.MaxStackSize,new_max=4,local_sig=m.Body.LocalVarToken.ToInt32(),code_sha256=H(projectCode),relocation=relocated.Evidence});
  R(report.Count==2,"Pinned scope 2");File.WriteAllBytes(args[1],after);File.WriteAllText(args[2],JsonSerializer.Serialize(new{status="UNTESTED_CANDIDATE",input_sha256=H(before),candidate_sha256=H(after),metadata,methods=report,scope="Two original-native-backed bodies; weighted selector uses float Range before GetEnumerator, ordered greater-than with NaN selection, second weight read and real finally; three public typed iterator references and one appended scoped enum local; Project uses independent track0 and fresh track4 check, original nine-case ID switch, captured helper results and partial writes with exact float32 operand order; complete Project body exceeds old slot and is appended to existing readonly section, only its MethodDef RVA changes, old slot/local signature preserved; no tests executed; unqualified parent chain pending"},new JsonSerializerOptions{WriteIndented=true}));Console.WriteLine("CANDIDATE "+H(after));
 }
}
