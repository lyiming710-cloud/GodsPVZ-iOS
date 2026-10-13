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
static class RepairNative85 {
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
  var initial=(MD)module.LookupToken(0x060003ED);
  R(initial.IsPublic&&initial.HasThis&&initial.GenericParameters.Count==1&&initial.Parameters.Count==6&&initial.Parameters[0].ParameterType.FullName=="UnityEngine.Vector3"&&initial.Parameters[5].ParameterType.FullName=="T","Pinned six-argument public Projectile Initial definition");
  R(module.GetTypes().Single(t=>t.FullName=="Zombie").MetadataToken.ToInt32()==0x02000084,"Exact Zombie TypeDef");
  int first=md.GetTableRowCount(TableIndex.MethodSpec)+1,si=Blob(new byte[]{0x0A,1,0x12,0x82,0x10});
  byte[] row=Words(0x3ED*2,si);R(row.Length==4,"Pinned MethodDefOrRef and blob widths");
  added.Add(new{kind="MethodSpec",token=$"0x{0x2B000000+first:X8}",method="0x060003ED",type_argument="0x02000084 Zombie",instantiation_hex="0A01128210",signature_index=si});
  var additions=new Dictionary<TableIndex,byte[]>{{TableIndex.MethodSpec,row}};
  ulong valid=BitConverter.ToUInt64(table,8);int tableStream=0,scan2=countAt+2;
  for(int i=0;i<count;i++){int e=Array.IndexOf(root,(byte)0,scan2+8);if(Encoding.ASCII.GetString(root,scan2+8,e-scan2-8)=="#~")tableStream=BitConverter.ToInt32(root,scan2);scan2=Align(e+1,4);}
  R(tableStream>0,"Existing tables offset");var expanded=table.ToList();
  foreach(var(t,bytes)in additions.OrderByDescending(x=>x.Key)){R((valid&(1UL<<(int)t))!=0,"Existing table required");int width=md.GetTableRowSize(t);R(t==TableIndex.MethodSpec&&width==4,"Pinned row width");int end=md.GetTableMetadataOffset(t)-tableStream+md.GetTableRowCount(t)*width;expanded.InsertRange(end,bytes);}
  var tables=expanded.ToArray();foreach(var(t,bytes)in additions){int at=24+4*Enumerable.Range(0,(int)t).Count(i=>(valid&(1UL<<i))!=0);BitConverter.GetBytes(md.GetTableRowCount(t)+bytes.Length/md.GetTableRowSize(t)).CopyTo(tables,at);}
  ulong sorted=BitConverter.ToUInt64(table,16),clear=1UL<<(int)TableIndex.MethodSpec;
  BitConverter.GetBytes(sorted&~clear).CopyTo(tables,16);streams[ti]=new("#~",tables);streams[streams.FindIndex(s=>s.Name=="#Blob")]=new("#Blob",blob);streams[streams.FindIndex(s=>s.Name=="#Strings")]=new("#Strings",strings);
  var nr=new List<byte>(root[..headEnd]);int scan=countAt+2;foreach(var s in streams){while(nr.Count%4!=0)nr.Add(0);BitConverter.GetBytes(nr.Count).CopyTo(nr,scan);BitConverter.GetBytes(s.Data.Length).CopyTo(nr,scan+4);nr.AddRange(s.Data);int e=Array.IndexOf(root,(byte)0,scan+8);scan=Align(e+1,4);}byte[] meta=nr.ToArray();
  // The qualified Native29 metadata section is already the last section.
  // Its header has no room for another section header. Extend that read-only
  // section at EOF while retaining all existing bytes, raw starts and RVAs.
  var ph=pe.PEHeaders;var opt=ph.PEHeader!;R(opt.Magic==PEMagic.PE32&&opt.CertificateTableDirectory.Size==0&&ph.CorHeader.StrongNameSignatureDirectory.Size==0&&(ph.CorHeader.Flags&CorFlags.StrongNameSigned)==0,"Unsupported signed/input layout");var last=ph.SectionHeaders.Last();R(last.Name==".n29meta"&&last.PointerToRawData+last.SizeOfRawData==input.Length&&last.VirtualSize<=last.SizeOfRawData&&last.SectionCharacteristics==(SectionCharacteristics)0x40000040,"Pinned extensible metadata section");R(rootOffset>=last.PointerToRawData&&rootOffset+rootSize<=input.Length,"Parent metadata in last section");int header=ph.PEHeaderStartOffset+ph.CoffHeader.SizeOfOptionalHeader+(ph.CoffHeader.NumberOfSections-1)*40;int raw=Align(input.Length,opt.FileAlignment),va=last.VirtualAddress+raw-last.PointerToRawData,append=Align(meta.Length,opt.FileAlignment);var output=new byte[raw+append];input.CopyTo(output,0);meta.CopyTo(output,raw);int newVirtual=raw-last.PointerToRawData+meta.Length,newRaw=output.Length-last.PointerToRawData;
  BitConverter.GetBytes(newVirtual).CopyTo(output,header+8);BitConverter.GetBytes(newRaw).CopyTo(output,header+16);BitConverter.GetBytes(opt.SizeOfInitializedData+output.Length-input.Length).CopyTo(output,ph.PEHeaderStartOffset+8);BitConverter.GetBytes(Align(last.VirtualAddress+newVirtual,opt.SectionAlignment)).CopyTo(output,ph.PEHeaderStartOffset+56);new byte[4].CopyTo(output,ph.PEHeaderStartOffset+64);BitConverter.GetBytes(va).CopyTo(output,ph.CorHeaderStartOffset+8);BitConverter.GetBytes(meta.Length).CopyTo(output,ph.CorHeaderStartOffset+12);
  return(output,new{old_root_offset=rootOffset,old_root_size=rootSize,new_root_offset=raw,new_root_size=meta.Length,extended_section_header=header,old_virtual_size=last.VirtualSize,new_virtual_size=newVirtual,old_raw_size=last.SizeOfRawData,new_raw_size=newRaw,added,blob_heap_added_bytes=blob.Length-oldBlob,methodspec_sorted_bit_cleared=(sorted&(1UL<<43))!=0,existing_heap_prefixes_unchanged=true,string_heap_added_bytes=strings.Length-oldStrings});

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
  byte[] before=File.ReadAllBytes(args[0]);R(H(before)=="b3284d01f0a57c9d22f6297741e96a831874cb0a584af2e165b98c42def84587","Pinned untested Native84 parent");
  using var asm=Mono.Cecil.AssemblyDefinition.ReadAssembly(args[0]);var module=asm.MainModule;using var fs=File.OpenRead(args[0]);using var pe=new PEReader(fs);
  var(after,metadata)=Metadata(before,pe,module);int initialZombie=0x2B000000+pe.GetMetadataReader().GetTableRowCount(TableIndex.MethodSpec)+1;
  MD M(int token)=>(MD)module.LookupToken(token);
  FR F(string owner,string name)=>module.GetTypes().Single(t=>t.FullName==owner).Fields.Single(f=>f.Name==name);
  MR C(MD method,string name)=>method.Body.Instructions.Select(i=>i.Operand).OfType<MR>().First(r=>r.Name==name);
  int localFirst=pe.GetMetadataReader().GetTableRowCount(TableIndex.StandAloneSig)+1;
  var targets=new Dictionary<int,(byte[] code,int max)>();var report=new List<object>();void Set(int token,IL il,int max){targets.Add(token,(il.Done(),max));}
  int StringToken(MD method,string text){var i=method.Body.Instructions.Single(i=>i.OpCode==OpCodes.Ldstr&&(string)i.Operand==text);var sec=pe.PEHeaders.SectionHeaders.Single(s=>method.RVA>=s.VirtualAddress&&method.RVA<s.VirtualAddress+s.SizeOfRawData);int header=sec.PointerToRawData+method.RVA-sec.VirtualAddress;R((before[header]&3)==3&&(BitConverter.ToUInt16(before,header)>>12)==3,"Pinned fat literal body");return BitConverter.ToInt32(before,header+12+i.Offset+1);}
  var m=M(0x06000443);R(m.Body.Variables[4].VariableType.FullName=="Damage"&&m.Body.Variables[7].VariableType.FullName=="Damage"&&m.Body.Variables[5].VariableType.FullName=="System.Int32"&&m.Body.Variables[6].VariableType.FullName=="System.Single","Exact ref slot/final Damage and numeric locals");
  R(M(0x06000447).Parameters[0].ParameterType.FullName=="Damage&"&&M(0x06000446).Parameters[0].ParameterType.FullName=="Damage&","Both helpers take the same managed-reference slot shape");
  var il=new IL().Ref(0x73,M(0x06000112)).S(4).L(4).Branch(0x39,"fail")
   .L(4).Arg(0).Ref(0x7B,F("Zombie","camp")).Ref(0x7D,F("Damage","camp"))
   .Arg(0).A(4).Arg(1).Arg(3).Ref(0x28,M(0x06000447))
   .Arg(0).A(4).Arg(1).Arg(2).Arg(3).Ref(0x28,M(0x06000446))
   .Arg(0).Ref(0x28,M(0x06000441)).S(6)
   .Arg(0).Ref(0x7B,F("Zombie","ID")).S(5).L(5).Int(3).O(0x59)
   .Switch("id3","common","common","common","common","common","common","common","id11","common","id13","common","common","id16","id17","id18","common","common","common","common").Branch(0x38,"common")
   .Label("id3").Arg(1).Int(0).Branch(0x3F,"common").L(6).Float(12f).Branch(0x38,"multiply")
   .Label("id11").L(6).Float(15f).Branch(0x38,"multiply")
   .Label("id13").Arg(0).Ref(0x28,M(0x0600044B)).L(6).O(0x5A,0x6B).S(6)
   .L(6).O(0x25,0x58,0x6B).S(6).Branch(0x38,"common")
   .Label("id16").Arg(1).Int(0).Branch(0x3F,"id16-negative").L(6).Float(4.5f).Branch(0x38,"multiply")
   .Label("id16-negative").L(6).Float(9f).Branch(0x38,"multiply")
   .Label("id17").Arg(1).Int(0).Branch(0x3F,"common").L(6).Float(5f).Branch(0x38,"multiply")
   .Label("id18").Arg(3).Branch(0x39,"id18-zero").Arg(3).Int(1).Branch(0x40,"common").L(6).Float(8f).Branch(0x38,"multiply")
   .Label("id18-zero").L(6).Float(0.5f)
   .Label("multiply").O(0x5A,0x6B).S(6)
   .Label("common").L(4).S(7).L(7).Branch(0x39,"fail")
   .L(7).L(6).Ref(0x7D,F("Damage","damagePoint"))
   .Arg(0).Ref(0x7B,F("Zombie","ID")).S(5).L(5).Int(7).Branch(0x3B,"check-projectile-id")
   .L(5).Int(20).Branch(0x40,"source-fields")
   .Label("check-projectile-id").Arg(1).Int(0).Branch(0x3F,"source-fields")
   .L(7).Int(0).Float(1000f).L(7).Int(1).Ref(0x73,M(0x0600011C)).Ref(0x28,M(0x0600010B))
   .Label("source-fields").L(7).Arg(0).Ref(0x7D,F("Damage","sourceZombie"))
   .L(7).Arg(0).Ref(0x7B,F("Zombie","board")).Ref(0x7D,F("Damage","board"))
   .L(7).O(0x2A).Label("fail").O(0x14,0x7A);
  Set(0x06000443,il,5);
  m=M(0x0600047D);R(m.Body.Variables[1].VariableType.FullName=="System.Object"&&m.Body.Variables[2].VariableType.FullName=="System.Boolean"&&m.Body.Variables[5].VariableType.FullName=="Board"&&m.Body.Variables[6].VariableType.FullName=="Projectile"&&m.Body.Variables[7].VariableType.FullName=="UnityEngine.Vector3"&&m.Body.Variables[8].VariableType.FullName=="Damage","Existing Shooting captures");
  var stringType=module.GetTypeReferences().Single(t=>t.FullName=="System.String");
  il=new IL().Int(0).S(2).O(0x72).I(StringToken(m,"")).S(1)
   .Arg(0).Ref(0x7B,F("Zombie","ID")).Int(6).O(0x59)
   .Switch("mouth","mouth-one","mouth","ordinary","ordinary","ordinary","ordinary","ordinary","ordinary","ordinary","ordinary","special","ordinary","mouth","mouth-one","mouth","ordinary").Branch(0x38,"ordinary")
   .Label("mouth-one").Int(1).S(2)
   .Label("mouth").O(0x72).I(StringToken(m,"PeaShooter_mouth")).S(1).Branch(0x38,"ordinary")
   .Label("special").Arg(0).Ref(0x28,M(0x060004AF)).O(0x2A)
   .Label("ordinary").Arg(0).Ref(0x7B,F("Zombie","board")).S(5).L(5).Branch(0x39,"fail")
   .L(5).Ref(0x7B,F("Board","projectileManager")).O(0x25).Branch(0x3A,"manager-present").O(0x26).Branch(0x38,"fail")
   .Label("manager-present").L(2).O(0x69).Ref(0x28,M(0x060001F6)).S(6)
   .Arg(0).Ref(0x7B,F("Zombie","animationSprites")).L(1).Ref(0x74,stringType).Ref(0x28,M(0x06000141)).S(7)
   .Arg(0).L(2).O(0x69).L(6).Int(0).Ref(0x28,M(0x06000443)).S(8)
   .L(6).O(0x25).Branch(0x3A,"projectile-present").O(0x26).Branch(0x38,"fail")
   .Label("projectile-present").L(7)
   .O(0x22).I(unchecked((int)0xC40B9333)).Float(0f).Float(0f).Ref(0x73,0x0A00009C)
   .Ref(0x28,0x0A0000FC).Arg(0).Ref(0x7B,F("Zombie","fZ")).Float(120f).O(0x58,0x6B).Int(0).Arg(0).Ref(0x28,initialZombie)
   .L(6).L(8).Arg(0).Ref(0x7B,F("Zombie","camp")).Ref(0x28,M(0x060003F7)).O(0x2A)
   .Label("fail").O(0x14,0x7A);
  Set(0x0600047D,il,7);
  foreach(var(token,(code,max))in targets){
   m=M(token);R(m.Body.ExceptionHandlers.Count==0,"No EH target");
   if(code.Length>m.Body.CodeSize){var relocated=AppendBody(after,m,code,max);after=relocated.Image;report.Add(new{token=$"0x{token:X8}",method=m.FullName,old_reserved_code_size=m.Body.CodeSize,new_code_size=code.Length,old_max=m.Body.MaxStackSize,new_max=max,local_sig=m.Body.LocalVarToken.ToInt32(),code_sha256=H(code),relocation=relocated.Evidence});continue;}
   var sec=pe.PEHeaders.SectionHeaders.Single(x=>m.RVA>=x.VirtualAddress&&m.RVA<x.VirtualAddress+x.SizeOfRawData);int h=sec.PointerToRawData+m.RVA-sec.VirtualAddress;
   R((before[h]&3)==3&&(BitConverter.ToUInt16(before,h)>>12)==3,"Pinned fat header");Array.Clear(after,h+12,m.Body.CodeSize);code.CopyTo(after,h+12);BitConverter.GetBytes(code.Length).CopyTo(after,h+4);BitConverter.GetBytes((ushort)max).CopyTo(after,h+2);
   report.Add(new{token=$"0x{token:X8}",method=m.FullName,header=h,code_start=h+12,reserved_code_size=m.Body.CodeSize,new_code_size=code.Length,old_max=m.Body.MaxStackSize,new_max=max,local_sig=m.Body.LocalVarToken.ToInt32(),code_sha256=H(code)});
  }
  R(targets.Count==2,"Pinned scope 2");File.WriteAllBytes(args[1],after);File.WriteAllText(args[2],JsonSerializer.Serialize(new{status="UNTESTED_CANDIDATE",input_sha256=H(before),candidate_sha256=H(after),metadata,methods=report,scope="Two complete original-native-backed Zombie bodies; GetDamage retains constructor camp timing, one byref slot across ordered helpers, late raw-null failure, two ID snapshots, exact Single operand/order and post-AddElement board reread; Shooting preserves 17-case dispatch, late projectile null failure after position/GetDamage, captured full position, exact speed bits, current fZ plus120 and post-Initial camp; one explicit MethodSpec Initial<Zombie> bound to six-argument public generic definition, no shared signature mutation; complete oversize bodies relocated with target-only RVA changes and old slots preserved; no tests executed; unqualified parent chain pending"},new JsonSerializerOptions{WriteIndented=true}));Console.WriteLine("CANDIDATE "+H(after));
 }
}
