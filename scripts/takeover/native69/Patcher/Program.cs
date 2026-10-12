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
static class RepairNative69 {
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
  int ti=streams.FindIndex(s=>s.Name=="#~");R(ti>=0,"Compressed tables required");
  var table=streams[ti].Data;var strings=streams.Single(s=>s.Name=="#Strings").Data;var blob=streams.Single(s=>s.Name=="#Blob").Data;
  int oldStrings=strings.Length,oldBlob=blob.Length;R((table[6]&3)==0,"Existing two-byte heaps");
  byte[] Compress(int v){R(v>=0&&v<16384,"Existing compressed index width");return v<128?new[]{(byte)v}:new[]{(byte)(0x80|(v>>8)),(byte)v};}
  int Blob(byte[] desired){
   foreach(var h in md.MemberReferences){var r=md.GetMemberReference(h);if(md.GetBlobBytes(r.Signature).SequenceEqual(desired))return MetadataTokens.GetHeapOffset(r.Signature);}
   R(desired.Length<128,"Bounded new signature");int ix=blob.Length;blob=blob.Concat(new[]{(byte)desired.Length}).Concat(desired).ToArray();R(blob.Length<65536,"No blob width growth");return ix;
  }
  int Name(string name){byte[] raw=Encoding.UTF8.GetBytes(name+"\0");for(int i=0;i+raw.Length<=strings.Length;i++)if(strings[i..(i+raw.Length)].SequenceEqual(raw))return i;throw new Exception("Existing public member name required: "+name);}
  byte[] Words(params int[] values)=>values.SelectMany(v=>{R(v>=0&&v<65536,"Two-byte row index");return BitConverter.GetBytes((ushort)v);}).ToArray();
  int TS(string name){var hits=Enumerable.Range(1,md.GetTableRowCount(TableIndex.TypeSpec)).Select(i=>module.LookupToken(0x1B000000+i)).OfType<Mono.Cecil.TypeReference>().Where(t=>t.FullName==name).ToArray();R(hits.Length==1,"Exact existing list TypeSpec: "+name);return checked((int)hits[0].MetadataToken.RID);}
  var memberRows=new List<byte>();var localRows=new List<byte>();var added=new List<object>();
  int localFirst=md.GetTableRowCount(TableIndex.StandAloneSig)+1,memberFirst=md.GetTableRowCount(TableIndex.MemberRef)+1;
  int Member(string name,int parent,byte[] signature){int rid=memberFirst+memberRows.Count/6,ni=Name(name),si=Blob(signature);memberRows.AddRange(Words(parent,ni,si));added.Add(new{kind="MemberRef",token=$"0x{0x0A000000+rid:X8}",name,parent,name_index=ni,signature_index=si});return 0x0A000000+rid;}
  int paths=TS("System.Collections.Generic.List`1<EnemyPath>")*8+4;
  Member("get_Count",paths,new byte[]{0x20,0,8});
  Member("Insert",paths,new byte[]{0x20,2,1,8,0x13,0});
  Member("Remove",paths,new byte[]{0x20,1,2,0x13,0});
  Member("Remove",TS("System.Collections.Generic.List`1<Path_enemyEditor>")*8+4,new byte[]{0x20,1,2,0x13,0});
  var target=(MD)module.LookupToken(0x06000055);var element=module.GetTypes().Single(t=>t.FullName=="EnemyPath");
  var oldSig=md.GetBlobBytes(md.GetStandaloneSignature(MetadataTokens.StandaloneSignatureHandle((int)target.Body.LocalVarToken.RID)).Signature);
  R(oldSig.Length>2&&oldSig[0]==7&&oldSig[1]==10&&target.Body.Variables.Count==10,"Pinned existing 10-local editor method");
  var newSig=new byte[]{7,11}.Concat(oldSig[2..]).Concat(new byte[]{0x12}).Concat(Compress(checked((int)element.MetadataToken.RID*4))).ToArray();
  int newSignature=Blob(newSig);localRows.AddRange(Words(newSignature));
  added.Add(new{kind="StandAloneSig",token=$"0x{0x11000000+localFirst:X8}",signature_index=newSignature,old_signature_hex=Convert.ToHexString(oldSig),signature_hex=Convert.ToHexString(newSig),appended_local="EnemyPath"});
  R(memberRows.Count==24&&localRows.Count==2,"Exactly four public collection bindings and one local signature");
  R(md.GetTableRowCount(TableIndex.TypeSpec)<8192&&md.GetTableRowCount(TableIndex.TypeRef)<8192&&md.GetTableRowCount(TableIndex.TypeDef)<8192&&md.GetTableRowCount(TableIndex.MethodDef)<8192,"No parent index widening");
  var additions=new Dictionary<TableIndex,byte[]>{{TableIndex.MemberRef,memberRows.ToArray()},{TableIndex.StandAloneSig,localRows.ToArray()}};
  ulong valid=BitConverter.ToUInt64(table,8);int tableStream=0,scan2=countAt+2;
  for(int i=0;i<count;i++){int e=Array.IndexOf(root,(byte)0,scan2+8);if(Encoding.ASCII.GetString(root,scan2+8,e-scan2-8)=="#~")tableStream=BitConverter.ToInt32(root,scan2);scan2=Align(e+1,4);}
  R(tableStream>0,"Existing tables offset");var expanded=table.ToList();
  foreach(var(t,bytes)in additions.OrderByDescending(x=>x.Key)){R((valid&(1UL<<(int)t))!=0,"Existing table required");int width=md.GetTableRowSize(t);R(width==(t==TableIndex.MemberRef?6:2),"Pinned row width");int end=md.GetTableMetadataOffset(t)-tableStream+md.GetTableRowCount(t)*width;expanded.InsertRange(end,bytes);}
  var tables=expanded.ToArray();foreach(var(t,bytes)in additions){int at=24+4*Enumerable.Range(0,(int)t).Count(i=>(valid&(1UL<<i))!=0);BitConverter.GetBytes(md.GetTableRowCount(t)+bytes.Length/md.GetTableRowSize(t)).CopyTo(tables,at);}
  ulong sorted=BitConverter.ToUInt64(table,16),clear=(1UL<<(int)TableIndex.MemberRef)|(1UL<<(int)TableIndex.StandAloneSig);
  BitConverter.GetBytes(sorted&~clear).CopyTo(tables,16);streams[ti]=new("#~",tables);streams[streams.FindIndex(s=>s.Name=="#Blob")]=new("#Blob",blob);
  var nr=new List<byte>(root[..headEnd]);int scan=countAt+2;foreach(var s in streams){while(nr.Count%4!=0)nr.Add(0);BitConverter.GetBytes(nr.Count).CopyTo(nr,scan);BitConverter.GetBytes(s.Data.Length).CopyTo(nr,scan+4);nr.AddRange(s.Data);int e=Array.IndexOf(root,(byte)0,scan+8);scan=Align(e+1,4);}byte[] meta=nr.ToArray();
  // The qualified Native29 metadata section is already the last section.
  // Its header has no room for another section header. Extend that read-only
  // section at EOF while retaining all existing bytes, raw starts and RVAs.
  var ph=pe.PEHeaders;var opt=ph.PEHeader!;R(opt.Magic==PEMagic.PE32&&opt.CertificateTableDirectory.Size==0&&ph.CorHeader.StrongNameSignatureDirectory.Size==0&&(ph.CorHeader.Flags&CorFlags.StrongNameSigned)==0,"Unsupported signed/input layout");var last=ph.SectionHeaders.Last();R(last.Name==".n29meta"&&last.PointerToRawData+last.SizeOfRawData==input.Length&&last.VirtualSize<=last.SizeOfRawData&&last.SectionCharacteristics==(SectionCharacteristics)0x40000040,"Pinned extensible metadata section");R(rootOffset>=last.PointerToRawData&&rootOffset+rootSize<=input.Length,"Parent metadata in last section");int header=ph.PEHeaderStartOffset+ph.CoffHeader.SizeOfOptionalHeader+(ph.CoffHeader.NumberOfSections-1)*40;int raw=Align(input.Length,opt.FileAlignment),va=last.VirtualAddress+raw-last.PointerToRawData,append=Align(meta.Length,opt.FileAlignment);var output=new byte[raw+append];input.CopyTo(output,0);meta.CopyTo(output,raw);int newVirtual=raw-last.PointerToRawData+meta.Length,newRaw=output.Length-last.PointerToRawData;
  BitConverter.GetBytes(newVirtual).CopyTo(output,header+8);BitConverter.GetBytes(newRaw).CopyTo(output,header+16);BitConverter.GetBytes(opt.SizeOfInitializedData+output.Length-input.Length).CopyTo(output,ph.PEHeaderStartOffset+8);BitConverter.GetBytes(Align(last.VirtualAddress+newVirtual,opt.SectionAlignment)).CopyTo(output,ph.PEHeaderStartOffset+56);new byte[4].CopyTo(output,ph.PEHeaderStartOffset+64);BitConverter.GetBytes(va).CopyTo(output,ph.CorHeaderStartOffset+8);BitConverter.GetBytes(meta.Length).CopyTo(output,ph.CorHeaderStartOffset+12);
  return(output,new{old_root_offset=rootOffset,old_root_size=rootSize,new_root_offset=raw,new_root_size=meta.Length,extended_section_header=header,old_virtual_size=last.VirtualSize,new_virtual_size=newVirtual,old_raw_size=last.SizeOfRawData,new_raw_size=newRaw,added,blob_heap_added_bytes=blob.Length-oldBlob,table_sorted_bit_cleared=(sorted&(1UL<<10))!=0,existing_heap_prefixes_unchanged=true,string_heap_added_bytes=strings.Length-oldStrings});

 }
 static void CopyTo(this byte[] b,List<byte> list,int p){for(int j=0;j<b.Length;j++)list[p+j]=b[j];}
 static int Main(string[] args){try{Run(args);return 0;}catch(Exception e){Console.Error.WriteLine(e);return 3;}}
 static void Run(string[] args){
  R(args.Length==3&&!File.Exists(args[1])&&!File.Exists(args[2]),"Fresh outputs required");
  byte[] before=File.ReadAllBytes(args[0]);R(H(before)=="58e44b184e89ee3d587b7c7c8d9f57bf58740b96e9c8c71303414aac59cad7a0","Pinned untested Native68 parent");
  using var asm=Mono.Cecil.AssemblyDefinition.ReadAssembly(args[0]);var module=asm.MainModule;using var fs=File.OpenRead(args[0]);using var pe=new PEReader(fs);
  var(after,metadata)=Metadata(before,pe,module);int first=0x0A000000+pe.GetMetadataReader().GetTableRowCount(TableIndex.MemberRef)+1;
  MD M(int token)=>(MD)module.LookupToken(token);
  FR F(string owner,string name)=>module.GetTypes().Single(t=>t.FullName==owner).Fields.Single(f=>f.Name==name);
  MR C(MD method,string name)=>method.Body.Instructions.Select(i=>i.Operand).OfType<MR>().First(r=>r.Name==name);
  int localFirst=pe.GetMetadataReader().GetTableRowCount(TableIndex.StandAloneSig)+1;
  var targets=new Dictionary<int,(byte[] code,int max)>();var report=new List<object>();void Set(int token,IL il,int max){targets.Add(token,(il.Done(),max));}
  var m=M(0x06000463);
  R(m.Body.Variables[1].VariableType.FullName=="System.Int32"&&m.Body.Variables[2].VariableType.FullName=="System.Collections.Generic.List`1<EnemyPath>","Existing index/list scratch");
  var il=new IL().Int(0).S(1).Label("loop").L(1).Arg(0).Ref(0x7B,F("Zombie","prePath")).Ref(0x6F,first).Branch(0x2F,"insert")
   .Arg(0).Ref(0x7B,F("Zombie","prePath")).L(1).Ref(0x6F,C(m,"get_Item")).Ref(0x7B,F("EnemyPath","arrived")).Branch(0x2C,"insert")
   .L(1).Int(1).O(0x58).S(1).Branch(0x2B,"loop")
   .Label("insert").Arg(0).Ref(0x7B,F("Zombie","prePath")).S(2).L(2).Branch(0x2C,"return")
   .L(2).L(1).Arg(1).Ref(0x6F,first+1).Label("return").O(0x2A);
  Set(0x06000463,il,3);
  m=M(0x06000055);
  il=new IL().Arg(0).Ref(0x7B,F("PathDataEditor","pathOnCheke")).Ref(0x7B,F("Path_enemyEditor","enemyPath")).S(10)
   .Arg(0).Ref(0x7B,F("PathDataEditor","enemy_BoardEditor")).O(0x14).Ref(0x28,C(m,"op_Inequality")).Branch(0x39,"load")
   .Arg(0).Ref(0x7B,F("PathDataEditor","enemy_BoardEditor")).Ref(0x7B,F("Enemy_boardEditor","preEnemy")).Ref(0x7B,F("Enemy","myPath")).L(10).Ref(0x6F,first+2).O(0x26)
   .Arg(0).Ref(0x7B,F("PathDataEditor","enemy_BoardEditor")).Ref(0x7B,F("Enemy_boardEditor","path_EnemyEditors"))
   .Arg(0).Ref(0x7B,F("PathDataEditor","pathOnCheke")).Ref(0x6F,first+3).O(0x26)
   .Arg(0).Ref(0x7B,F("PathDataEditor","pathOnCheke")).Ref(0x6F,C(m,"get_gameObject")).Ref(0x28,C(m,"Destroy"))
   .Arg(0).O(0x14).Ref(0x7D,F("PathDataEditor","pathOnCheke"))
   .Label("load").Arg(0).Arg(0).Ref(0x7B,F("PathDataEditor","pathIndex")).Ref(0x28,C(m,"LoadData")).O(0x2A);
  Set(0x06000055,il,2);
  m=M(0x060002C3);
  il=new IL().Arg(0).Arg(1).Ref(0x7D,F("Board","endTime"))
   .Arg(0).Float(1f).Ref(0x7D,F("Board","gameSpeed"))
   .Float(1f).Ref(0x28,C(m,"set_timeScale"))
   .Arg(0).Ref(0x7B,F("Board","buttomButtons")).Int(0).Ref(0x6F,C(m,"SetActive"))
   .Arg(0).Int(1).Ref(0x7D,F("Board","isWon"))
   .Ref(0x7E,F("ResourceManager","boardClips")).O(0x1F,10).Ref(0x6F,C(m,"get_Item"))
   .Arg(0).Ref(0x7B,F("Board","cameraPosition")).Ref(0x28,C(m,"AudioVolume")).Ref(0x28,C(m,"CreateAudioAtPoint")).O(0x26,0x2A);
  Set(0x060002C3,il,3);
  m=M(0x06000590);
  R(m.Body.Variables[1].VariableType.FullName=="Skill"&&m.Body.Variables[2].VariableType.FullName=="PlantSave"&&m.Body.Variables[9].VariableType.FullName=="System.Int32","Existing Skill/PlantSave/level scratch");
  var color=module.GetMemberReferences().OfType<MR>().Single(r=>r.FullName=="System.Void UnityEngine.Color::.ctor(System.Single,System.Single,System.Single,System.Single)");
  il=new IL().Arg(0).Ref(0x7B,F("Almanac_SkillSystem","plantSave")).Arg(0).Ref(0x7B,F("Almanac_SkillSystem","skillID")).Ref(0x6F,C(m,"GetSkill_ID")).S(1)
   .Arg(0).Ref(0x7B,F("Almanac_SkillSystem","plantSave")).O(0x25).Ref(0x7B,F("PlantSave","skillLevel")).Int(1).O(0x58).Ref(0x7D,F("PlantSave","skillLevel"))
   .Ref(0x7E,F("GlobalStaticVars","gLawnApp")).Ref(0x7B,F("GlobalStaticVars/LawnApp","savesManager"))
   .Arg(0).Ref(0x7B,F("Almanac_SkillSystem","plantSave")).Ref(0x7B,F("PlantSave","skillLevel")).S(9)
   .Arg(0).Ref(0x7B,F("Almanac_SkillSystem","plantSave")).L(9).Ref(0x6F,C(m,"GetSkillData_All"))
   .Arg(0).L(1).Float(1f).Float(1f).Float(0.2f).Float(1f).Ref(0x73,color).Ref(0x28,C(m,"UpdateSkillTexts"))
   .Arg(0).Ref(0x7B,F("Almanac_SkillSystem","plantSave")).S(2)
   .Arg(0).Ref(0x7B,F("Almanac_SkillSystem","button_TargetUp"))
   .L(2).Ref(0x7B,F("PlantSave","skillLevel")).L(2).Ref(0x7B,F("PlantSave","order")).L(2).Ref(0x7B,F("PlantSave","stars")).Int(1).Ref(0x6F,C(m,"SetState")).O(0x2A);
  Set(0x06000590,il,6);
  foreach(var(token,(code,max))in targets){
   m=M(token);R(m.Body.ExceptionHandlers.Count==0,"EH relocation unsupported");var sec=pe.PEHeaders.SectionHeaders.Single(x=>m.RVA>=x.VirtualAddress&&m.RVA<x.VirtualAddress+x.SizeOfRawData);int h=sec.PointerToRawData+m.RVA-sec.VirtualAddress,hs=(BitConverter.ToUInt16(before,h)>>12)*4;
   R((before[h]&3)==3&&hs==12,"Pinned fat body");R(code.Length<=m.Body.CodeSize,"Body exceeds reserved slot "+m.FullName);
   Array.Clear(after,h+hs,m.Body.CodeSize);code.CopyTo(after,h+hs);BitConverter.GetBytes(code.Length).CopyTo(after,h+4);BitConverter.GetBytes((ushort)max).CopyTo(after,h+2);
   int oldLocal=m.Body.LocalVarToken.ToInt32(),newLocal=token==0x06000055?0x11000000+localFirst:oldLocal;if(newLocal!=oldLocal)BitConverter.GetBytes(newLocal).CopyTo(after,h+8);
   report.Add(new{token=$"0x{token:X8}",method=m.FullName,header=h,code_start=h+hs,reserved_code_size=m.Body.CodeSize,new_code_size=code.Length,old_max=m.Body.MaxStackSize,new_max=max,old_local_sig=oldLocal,local_sig=newLocal,code_sha256=H(code)});
  }
  R(targets.Count==4,"Pinned scope 4");File.WriteAllBytes(args[1],after);File.WriteAllText(args[2],JsonSerializer.Serialize(new{status="UNTESTED_CANDIDATE",input_sha256=H(before),candidate_sha256=H(after),metadata,methods=report,scope="Four native-backed bodies; arrived-prefix insert, editor deletion, win audio and skill level update; four public collection bindings, one captured EnemyPath scratch signature; original Color constructor reused with native RGBA bits; unqualified parent chain pending"},new JsonSerializerOptions{WriteIndented=true}));Console.WriteLine("CANDIDATE "+H(after));
 }
}
