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
static class RepairNative89 {
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
  var binding=(MD)module.LookupToken(0x060000C4);R(binding.IsPublic&&binding.HasThis&&binding.GenericParameters.Count==1&&binding.Parameters.Count==4&&binding.Parameters[0].ParameterType.FullName=="T"&&binding.Parameters[1].ParameterType.FullName=="System.Boolean"&&binding.Parameters[2].ParameterType.FullName=="System.Single"&&binding.Parameters[3].ParameterType.FullName=="System.String","Exact public Binding<T> contract");
  R(module.GetTypes().Single(t=>t.FullName=="Plant").MetadataToken.ToInt32()==0x02000078,"Exact Plant TypeDef");
  int first=md.GetTableRowCount(TableIndex.MethodSpec)+1,si=Blob(new byte[]{0x0A,1,0x12,0x81,0xE0});
  byte[] row=Words(0xC4*2,si);R(row.Length==4,"Pinned MethodDefOrRef and blob widths");
  added.Add(new{kind="MethodSpec",token=$"0x{0x2B000000+first:X8}",method="0x060000C4",type_argument="0x02000078 Plant",instantiation_hex="0A011281E0",signature_index=si});
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
  byte[] before=File.ReadAllBytes(args[0]);R(H(before)=="bff7fa2956077e5ac06a4b903fbb5a1be4aad989ac6b5cb08fd7d0aa6e690951","Pinned untested Native88 parent");
  using var asm=Mono.Cecil.AssemblyDefinition.ReadAssembly(args[0]);var module=asm.MainModule;using var fs=File.OpenRead(args[0]);using var pe=new PEReader(fs);
  var(after,metadata)=Metadata(before,pe,module);int bindingPlant=0x2B000000+pe.GetMetadataReader().GetTableRowCount(TableIndex.MethodSpec)+1;
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
  var m=M(0x06000517);R(m.IsStatic&&m.Parameters.Count==4&&m.Parameters[0].ParameterType.FullName=="Plant"&&m.Parameters[1].ParameterType.FullName=="Zombie"&&m.Parameters[2].ParameterType.FullName=="Projectile"&&m.Parameters[3].ParameterType.FullName=="Damage"&&m.Body.Variables[1].VariableType.FullName=="UnityEngine.GameObject"&&m.Body.Variables[2].VariableType.FullName=="VFXAnimationEvent"&&m.Body.Variables[3].VariableType.FullName=="UnityEngine.Transform"&&m.Body.Variables[4].VariableType.FullName=="UnityEngine.Vector3"&&m.Body.Variables[7].VariableType.FullName=="UnityEngine.Transform","Static Awake149 and original typed captures");
  var component=(GenericInstanceMethod)module.LookupToken(0x2B000069);R(component.ElementMethod.Name=="GetComponent"&&component.DeclaringType.FullName=="UnityEngine.GameObject"&&component.GenericArguments.Single().FullName=="VFXAnimationEvent","Exact GetComponent<VFXAnimationEvent>");
  var il=new IL().Arg(0).Int(1).Ref(0x7D,F("Plant","blockable"))
   .Ref(0x28,M(0x060001E5)).Int(35).Ref(0x6F,M(0x060001EA)).S(1).L(1).Branch(0x39,"fail")
   .L(1).Ref(0x6F,component).S(2).L(2).Branch(0x39,"fail")
   .L(2).Arg(0).Int(1).Float(0f).O(0x72).I(ExistingStringToken("PlantSkill")).Ref(0x28,bindingPlant)
   .L(1).Ref(0x6F,0x0A00002E).S(3)
   .Arg(0).Ref(0x6F,0x0A000001).S(7).L(7).Branch(0x39,"fail")
   .L(7).Ref(0x6F,0x0A000037).S(4).L(3).Branch(0x39,"fail")
   .L(3).L(4).Ref(0x6F,0x0A000038).O(0x2A).Label("fail").O(0x14,0x7A);
  Set(0x06000517,il,5);
  m=M(0x06000510);R(m.IsStatic&&m.Parameters.Count==4&&m.Parameters[0].ParameterType.FullName=="Plant"&&m.Body.Variables[1].VariableType.FullName=="Board"&&m.Body.Variables[2].VariableType.FullName=="Skill"&&m.Body.Variables[4].VariableType.FullName=="Damage"&&m.Body.Variables[5].VariableType.FullName=="System.Single[]"&&m.Body.Variables[6].VariableType.FullName=="Element"&&m.Body.Variables[7].VariableType.FullName=="System.Single[]"&&m.Body.Variables[8].VariableType.FullName=="Projectile"&&m.Body.Variables[9].VariableType.FullName=="UnityEngine.Vector3","Static Function16 captured skill, two arrays, projectile/position");
  R(M(0x0600036C).IsPublic&&M(0x0600036C).Parameters.Count==2&&M(0x060003B9).IsPublic&&M(0x060003B9).ReturnType.FullName=="System.Collections.Generic.List`1<UnityEngine.GameObject>"&&M(0x06000110).IsPublic,"Actual public inherited getter/two-parameter GetDamage and FindElement contracts");
  var initial=(GenericInstanceMethod)module.LookupToken(0x2B000060);R(initial.ElementMethod.MetadataToken.ToInt32()==0x060003ED&&initial.GenericArguments.Single().FullName=="Plant","Exact existing Initial<Plant> six-argument binding");
  il=new IL().Arg(0).Ref(0x7B,F("Plant","board")).S(1).Arg(0).Ref(0x7B,F("Plant","skill")).S(2).L(1).Branch(0x39,"fail")
   .L(1).Ref(0x7B,F("Board","projectileManager")).Int(19).Ref(0x6F,M(0x060001F6)).S(8)
   .Arg(0).Ref(0x6F,M(0x060003B9)).O(0x72).I(ExistingStringToken("PeaShooter_mouth")).Ref(0x28,M(0x06000141)).S(9)
   .Arg(0).L(8).Int(19).Ref(0x28,M(0x0600036C)).S(4).L(4).Branch(0x39,"fail")
   .L(4).L(4).Ref(0x7B,F("Damage","damagePoint"))
   .L(2).Ref(0x7B,F("Skill","value_float")).S(5).L(5).Int(0).O(0x98,0x5A,0x6B).Ref(0x7D,F("Damage","damagePoint"))
   .L(4).L(5).Int(1).O(0x98).Ref(0x7D,F("Damage","penetrationVigour"))
   .L(4).Int(0).Ref(0x6F,M(0x06000110)).S(6).L(6).Branch(0x39,"initialize")
   .L(6).L(6).Ref(0x7B,F("Element","point"))
   .L(2).Ref(0x7B,F("Skill","value_float")).S(7).L(7).Int(2).O(0x98,0x5A,0x6B).Ref(0x7D,F("Element","point"))
   .Label("initialize").L(8).O(0x25).Branch(0x3A,"projectile-present").O(0x26).Branch(0x38,"fail")
   .Label("projectile-present").L(9).Float(1675f).Float(0f).Float(0f).Ref(0x73,0x0A00009C).Ref(0x28,0x0A0000FC).Float(70f).Int(0).Arg(0).Ref(0x28,initial)
   .L(8).Arg(0).Ref(0x7B,F("Plant","camp"))
   .L(8).L(4).Ref(0x7D,F("Projectile","damage")).Ref(0x7D,F("Projectile","camp"))
   .O(0x2A).Label("fail").O(0x14,0x7A);
  Set(0x06000510,il,7);
  foreach(var(token,(code,max))in targets){
   m=M(token);R(m.Body.ExceptionHandlers.Count==0,"No EH target");
   if(code.Length>m.Body.CodeSize){var relocated=AppendBody(after,m,code,max);after=relocated.Image;report.Add(new{token=$"0x{token:X8}",method=m.FullName,old_reserved_code_size=m.Body.CodeSize,new_code_size=code.Length,old_max=m.Body.MaxStackSize,new_max=max,local_sig=m.Body.LocalVarToken.ToInt32(),code_sha256=H(code),relocation=relocated.Evidence});continue;}
   var sec=pe.PEHeaders.SectionHeaders.Single(x=>m.RVA>=x.VirtualAddress&&m.RVA<x.VirtualAddress+x.SizeOfRawData);int h=sec.PointerToRawData+m.RVA-sec.VirtualAddress;
   R((before[h]&3)==3&&(BitConverter.ToUInt16(before,h)>>12)==3,"Pinned fat header");Array.Clear(after,h+12,m.Body.CodeSize);code.CopyTo(after,h+12);BitConverter.GetBytes(code.Length).CopyTo(after,h+4);BitConverter.GetBytes((ushort)max).CopyTo(after,h+2);
   report.Add(new{token=$"0x{token:X8}",method=m.FullName,header=h,code_start=h+12,reserved_code_size=m.Body.CodeSize,new_code_size=code.Length,old_max=m.Body.MaxStackSize,new_max=max,local_sig=m.Body.LocalVarToken.ToInt32(),code_sha256=H(code)});
  }
  R(targets.Count==2,"Pinned scope 2");File.WriteAllBytes(args[1],after);File.WriteAllText(args[2],JsonSerializer.Serialize(new{status="UNTESTED_CANDIDATE",input_sha256=H(before),candidate_sha256=H(after),metadata,methods=report,scope="Two original-native-backed static SkillManager methods with exact Plant/Zombie/Projectile/Damage parameter identities; Awake149 sets blockable before creation, retains captured particle/component, restores Binding<Plant>(plant,true,+0,PlantSkill), acquires plant position before delayed effect-transform fault; Function16 captures skill before creation, captures position before fresh Damage, retains first value_float array for damage/penetration, rereads value_float after FindElement, preserves ordered partial writes and late projectile null fault, Initial<Plant> with speed1675/fZ70, fresh camp captured before damage/camp writes; one exact Binding<Plant> MethodSpec, no local mutations, complete oversize body relocation if needed; no tests executed; pending parent chain"},new JsonSerializerOptions{WriteIndented=true}));Console.WriteLine("CANDIDATE "+H(after));
 }
}
