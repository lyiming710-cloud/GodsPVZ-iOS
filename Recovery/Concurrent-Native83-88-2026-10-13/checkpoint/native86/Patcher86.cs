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
static class RepairNative86 {
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
  var added=new List<object>();
  R(module.GetTypeReferences().Single(t=>t.FullName=="UnityEngine.EventSystems.PointerEventData").MetadataToken.ToInt32()==0x01000066&&module.GetTypeReferences().Single(t=>t.FullName=="UnityEngine.Vector2").MetadataToken.ToInt32()==0x0100004B,"Pinned legal position getter types");
  int first=md.GetTableRowCount(TableIndex.MemberRef)+1,ni=Name("get_position"),si=Blob(new byte[]{0x20,0,0x11,0x81,0x2D});
  byte[] memberRow=Words(0x331,ni,si);R(memberRow.Length==6,"Pinned getter row width");
  added.Add(new{kind="MemberRef",token=$"0x{0x0A000000+first:X8}",name="get_position",declaring_type="0x01000066",parent=0x331,signature_hex="200011812D",signature_index=si,name_index=ni});
  var method=(MD)module.LookupToken(0x060006DF);byte[] old=md.GetBlobBytes(md.GetStandaloneSignature(MetadataTokens.StandaloneSignatureHandle((int)method.Body.LocalVarToken.RID)).Signature);
  R(old[0]==7&&old[1]==9&&method.Body.Variables.Count==9,"Pinned original text-link local count");
  byte[] sig=new byte[]{7,10}.Concat(old[2..]).Concat(new byte[]{0x11,0x81,0x2D}).ToArray();int li=Blob(sig),localFirst=md.GetTableRowCount(TableIndex.StandAloneSig)+1;
  added.Add(new{kind="StandAloneSig",token=$"0x{0x11000000+localFirst:X8}",target="0x060006DF",active_local=9,old_signature_hex=Convert.ToHexString(old),signature_hex=Convert.ToHexString(sig),signature_index=li});
  var additions=new Dictionary<TableIndex,byte[]>{{TableIndex.MemberRef,memberRow},{TableIndex.StandAloneSig,Words(li)}};
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
  return(output,new{old_root_offset=rootOffset,old_root_size=rootSize,new_root_offset=raw,new_root_size=meta.Length,extended_section_header=header,old_virtual_size=last.VirtualSize,new_virtual_size=newVirtual,old_raw_size=last.SizeOfRawData,new_raw_size=newRaw,added,blob_heap_added_bytes=blob.Length-oldBlob,memberref_sorted_bit_cleared=(sorted&(1UL<<10))!=0,existing_heap_prefixes_unchanged=true,string_heap_added_bytes=strings.Length-oldStrings});

 }
 static (byte[] Image,object Evidence) AppendBody(byte[] current,MD method,byte[] code,int max){
  using var pe=new PEReader(new MemoryStream(current));var ph=pe.PEHeaders;var opt=ph.PEHeader!;var md=pe.GetMetadataReader();var last=ph.SectionHeaders.Last();
  R(opt.Magic==PEMagic.PE32&&last.Name==".n29meta"&&last.PointerToRawData+last.SizeOfRawData==current.Length&&last.VirtualSize<=last.SizeOfRawData&&last.SectionCharacteristics==(SectionCharacteristics)0x40000040&&opt.CertificateTableDirectory.Size==0&&ph.CorHeader!.StrongNameSignatureDirectory.Size==0&&(ph.CorHeader.Flags&CorFlags.StrongNameSigned)==0,"Pinned unsigned EOF body relocation layout");
  int rootRva=ph.CorHeader.MetadataDirectory.RelativeVirtualAddress,root=checked(last.PointerToRawData+rootRva-last.VirtualAddress),rootSize=ph.CorHeader.MetadataDirectory.Size;
  R(root>=last.PointerToRawData&&checked(root+rootSize)<=current.Length,"Active metadata in extensible last section");
  int row=checked(md.GetTableMetadataOffset(TableIndex.MethodDef)+(checked((int)method.MetadataToken.RID)-1)*md.GetTableRowSize(TableIndex.MethodDef));
  R(row>=0&&checked(row+4)<=rootSize&&BitConverter.ToInt32(current,root+row)==method.RVA,"Pinned target RVA in active metadata row");
  R(method.Body.ExceptionHandlers.Count==0&&code.Length>method.Body.CodeSize,"Only complete oversize no-EH target bodies are relocated");
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
  byte[] before=File.ReadAllBytes(args[0]);R(H(before)=="ba1b6ae5cd5b27b89797c16484cab5a6eb52157deab0bd4251e5fefe1aa562b0","Pinned untested Native85 parent");
  using var asm=Mono.Cecil.AssemblyDefinition.ReadAssembly(args[0]);var module=asm.MainModule;using var fs=File.OpenRead(args[0]);using var pe=new PEReader(fs);
  var(after,metadata)=Metadata(before,pe,module);int positionGetter=0x0A000000+pe.GetMetadataReader().GetTableRowCount(TableIndex.MemberRef)+1;
  MD M(int token)=>(MD)module.LookupToken(token);
  FR F(string owner,string name)=>module.GetTypes().Single(t=>t.FullName==owner).Fields.Single(f=>f.Name==name);
  MR C(MD method,string name)=>method.Body.Instructions.Select(i=>i.Operand).OfType<MR>().First(r=>r.Name==name);
  int localFirst=pe.GetMetadataReader().GetTableRowCount(TableIndex.StandAloneSig)+1;
  var targets=new Dictionary<int,(byte[] code,int max)>();var report=new List<object>();void Set(int token,IL il,int max){targets.Add(token,(il.Done(),max));}
  int StringToken(MD method,string text){var i=method.Body.Instructions.Single(i=>i.OpCode==OpCodes.Ldstr&&(string)i.Operand==text);var sec=pe.PEHeaders.SectionHeaders.Single(s=>method.RVA>=s.VirtualAddress&&method.RVA<s.VirtualAddress+s.SizeOfRawData);int header=sec.PointerToRawData+method.RVA-sec.VirtualAddress;R((before[header]&3)==3&&(BitConverter.ToUInt16(before,header)>>12)==3,"Pinned fat literal body");return BitConverter.ToInt32(before,header+12+i.Offset+1);}
  int ExistingStringToken(string text){
   var md=pe.GetMetadataReader();byte[] content=pe.GetMetadata().GetContent().ToArray();int start=md.GetHeapMetadataOffset(HeapIndex.UserString),length=md.GetHeapSize(HeapIndex.UserString);
   for(int ix=1;ix<length;){int p=start+ix,size=content[p++];if((size&0x80)!=0){if((size&0xC0)==0x80)size=((size&0x3F)<<8)|content[p++];else{R((size&0xE0)==0xC0,"Valid four-byte compressed user-string length");size=((size&0x1F)<<24)|(content[p++]<<16)|(content[p++]<<8)|content[p++];}}
    R(size>=0&&p+size<=start+length,"Existing bounded user-string entry");if(size>0&&md.GetUserString(MetadataTokens.UserStringHandle(ix))==text)return 0x70000000+ix;ix=p+size-start;
   }throw new InvalidOperationException("Exact existing user string absent: "+text);
  }
  var m=M(0x060006DF);R(m.Body.Variables[0].VariableType.FullName=="UnityEngine.Camera"&&m.Body.Variables[1].VariableType.FullName=="System.Int32"&&m.Body.Variables[5].VariableType.FullName=="System.String"&&m.Body.Variables[8].VariableType.FullName=="TMPro.TMP_LinkInfo","Pinned typed link captures");
  R(StringToken(m,"jump0")==0x7001CCC2&&StringToken(m,"https://space.bilibili.com/525659346")==0x7001CCCE,"Original exact fixed link literals");
  var linkInfo=module.GetTypeReferences().Single(t=>t.FullName=="TMPro.TMP_LinkInfo");R(linkInfo.MetadataToken.ToInt32()==0x01000065,"Exact struct array element");
  var il=new IL().Arg(0).Ref(0x7B,F("TextLink","TMP_Text"))
   .Arg(1).Ref(0x6F,positionGetter).S(9).Ref(0x28,0x0A000004).S(0)
   .A(9).Ref(0x7B,0x0A0001D4).A(9).Ref(0x7B,0x0A0001D5).Float(0f).Ref(0x73,0x0A00009C).L(0).Ref(0x28,0x0A0002B5).S(1)
   .Arg(0).L(1).Ref(0x7D,F("TextLink","linkIndex")).L(1).Int(-1).Branch(0x3B,"return")
   .Arg(0).Ref(0x7B,F("TextLink","TMP_Text")).Ref(0x6F,0x0A0002B8).Ref(0x7B,0x0A0002B9).L(1).Ref(0xA3,linkInfo).S(8)
   .A(8).Ref(0x28,0x0A0002B6).S(5).L(5).Ref(0x28,0x0A000010)
   .L(5).O(0x72).I(StringToken(m,"jump0")).Ref(0x28,0x0A00003A).Branch(0x39,"return")
   .O(0x72).I(StringToken(m,"https://space.bilibili.com/525659346")).Ref(0x28,0x0A0002B7)
   .Label("return").O(0x2A);
  Set(0x060006DF,il,4);
  m=M(0x060006FE);R(m.Body.Variables[0].VariableType.FullName=="System.Int32","Existing window index/ID local");
  il=new IL().Arg(0).Ref(0x7B,F("Window_Q","Q")).Switch("return","return-main","resume","resume","delete","backup").Branch(0x38,"return")
   .Label("return-main").Arg(0).Ref(0x28,M(0x06000702)).Branch(0x38,"return")
   .Label("resume").Arg(0).Ref(0x28,M(0x06000701)).Branch(0x38,"return")
   .Label("delete").Arg(0).Ref(0x7B,F("Window_Q","info0_int")).Int(0).Branch(0x3F,"return")
   .Ref(0x28,M(0x06000636)).Ref(0x7B,F("MainUIController","savesUIController")).O(0x25,0x14).Ref(0x28,0x0A000012).Branch(0x3A,"pop-return")
   .Ref(0x7B,F("SavesUIController","playerSaveList")).Arg(0).Ref(0x7B,F("Window_Q","info0_int")).S(0)
   .O(0x25).Ref(0x6F,0x0A000443).L(0).Branch(0x3E,"pop-return")
   .L(0).Ref(0x6F,0x0A0002C7).O(0x25).Ref(0x28,0x0A00000B).Branch(0x39,"pop-destroy")
   .O(0x25).Ref(0x7B,F("SavesUIController_PlayerSave","savesManager")).Branch(0x39,"pop-destroy")
   .O(0x25).Ref(0x7B,F("SavesUIController_PlayerSave","playerSaveID")).S(0)
   .Ref(0x7B,F("SavesUIController_PlayerSave","savesManager")).L(0).Ref(0x6F,M(0x0600023C))
   .Ref(0x28,M(0x06000636)).Ref(0x6F,M(0x0600063A)).Branch(0x38,"destroy")
   .Label("backup").Ref(0x28,M(0x06000636)).Ref(0x7B,F("MainUIController","savesManager")).O(0x25).Branch(0x39,"pop-return")
   .Ref(0x6F,M(0x06000238)).Branch(0x38,"destroy")
   .Label("pop-return").O(0x26).Branch(0x38,"return")
   .Label("pop-destroy").O(0x26)
   .Label("destroy").Arg(0).Ref(0x28,0x0A000009).Ref(0x28,0x0A000025)
   .Label("return").O(0x2A);
  Set(0x060006FE,il,3);
  foreach(var(token,(code,max))in targets){
   m=M(token);R(m.Body.ExceptionHandlers.Count==0,"No EH target");
   if(code.Length>m.Body.CodeSize){R(token!=0x060006DF,"TextLink must fit old slot with its scoped new locals");var relocated=AppendBody(after,m,code,max);after=relocated.Image;report.Add(new{token=$"0x{token:X8}",method=m.FullName,old_reserved_code_size=m.Body.CodeSize,new_code_size=code.Length,old_max=m.Body.MaxStackSize,new_max=max,local_sig=m.Body.LocalVarToken.ToInt32(),code_sha256=H(code),relocation=relocated.Evidence});continue;}
   var sec=pe.PEHeaders.SectionHeaders.Single(x=>m.RVA>=x.VirtualAddress&&m.RVA<x.VirtualAddress+x.SizeOfRawData);int h=sec.PointerToRawData+m.RVA-sec.VirtualAddress;
   R((before[h]&3)==3&&(BitConverter.ToUInt16(before,h)>>12)==3,"Pinned fat header");Array.Clear(after,h+12,m.Body.CodeSize);code.CopyTo(after,h+12);BitConverter.GetBytes(code.Length).CopyTo(after,h+4);BitConverter.GetBytes((ushort)max).CopyTo(after,h+2);int local=token==0x060006DF?0x11000000+localFirst:m.Body.LocalVarToken.ToInt32();BitConverter.GetBytes(local).CopyTo(after,h+8);
   report.Add(new{token=$"0x{token:X8}",method=m.FullName,header=h,code_start=h+12,reserved_code_size=m.Body.CodeSize,new_code_size=code.Length,old_max=m.Body.MaxStackSize,new_max=max,old_local_sig=m.Body.LocalVarToken.ToInt32(),local_sig=local,code_sha256=H(code)});
  }
  R(targets.Count==2,"Pinned scope 2");File.WriteAllBytes(args[1],after);File.WriteAllText(args[2],JsonSerializer.Serialize(new{status="UNTESTED_CANDIDATE",input_sha256=H(before),candidate_sha256=H(after),metadata,methods=report,scope="Two complete original-native-backed callers; TextLink captures old text before event position and Camera, passes Vector3 by value, writes saved Find index before exact -1 return, rereads current text, copies array struct before GetLinkID and captures string across Log; one proven public PointerEventData position MemberRef and one target-only appended Vector2 local signature; Window retains six-case switch, initial/reread signed index distinctions, Unity equality/implicit and raw manager checks, captured item/list across callbacks, reread current MainUI after deletion, exact return-versus-Destroy scope without finally; complete oversize Window body relocated with only its MethodDef RVA changed; no tests executed; unqualified parent chain pending"},new JsonSerializerOptions{WriteIndented=true}));Console.WriteLine("CANDIDATE "+H(after));
 }
}
