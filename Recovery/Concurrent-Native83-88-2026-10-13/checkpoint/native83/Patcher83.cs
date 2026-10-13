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
static class RepairNative83 {
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
  int Name(string name){byte[] raw=Encoding.UTF8.GetBytes(name+"\0");for(int i=0;i+raw.Length<=strings.Length;i++)if(strings[i..(i+raw.Length)].SequenceEqual(raw))return i;int ix=strings.Length;strings=strings.Concat(raw).ToArray();R(strings.Length<65536,"No string heap width growth");return ix;}
  byte[] Words(params int[] values)=>values.SelectMany(v=>{R(v>=0&&v<65536,"Two-byte row index");return BitConverter.GetBytes((ushort)v);}).ToArray();
  int TS(string name){var hits=Enumerable.Range(1,md.GetTableRowCount(TableIndex.TypeSpec)).Select(i=>module.LookupToken(0x1B000000+i)).OfType<Mono.Cecil.TypeReference>().Where(t=>t.FullName==name).ToArray();R(hits.Length==1,"Exact existing list TypeSpec: "+name);return checked((int)hits[0].MetadataToken.RID);}
  var memberRows=new List<byte>();var localRows=new List<byte>();var added=new List<object>();
  int localFirst=md.GetTableRowCount(TableIndex.StandAloneSig)+1,memberFirst=md.GetTableRowCount(TableIndex.MemberRef)+1;
  int Member(string name,int parent,byte[] signature){int rid=memberFirst+memberRows.Count/6,ni=Name(name),si=Blob(signature);memberRows.AddRange(Words(parent,ni,si));added.Add(new{kind="MemberRef",token=$"0x{0x0A000000+rid:X8}",name,parent,name_index=ni,signature_index=si});return 0x0A000000+rid;}
  Member("Insert",TS("System.Collections.Generic.List`1<Path_enemyEditor>")*8+4,new byte[]{0x20,2,1,8,0x13,0});
  R(memberRows.Count==6&&localRows.Count==0,"Only one typed public Path_enemyEditor Insert binding");
  var additions=new Dictionary<TableIndex,byte[]>{{TableIndex.MemberRef,memberRows.ToArray()}};
  ulong valid=BitConverter.ToUInt64(table,8);int tableStream=0,scan2=countAt+2;
  for(int i=0;i<count;i++){int e=Array.IndexOf(root,(byte)0,scan2+8);if(Encoding.ASCII.GetString(root,scan2+8,e-scan2-8)=="#~")tableStream=BitConverter.ToInt32(root,scan2);scan2=Align(e+1,4);}
  R(tableStream>0,"Existing tables offset");var expanded=table.ToList();
  foreach(var(t,bytes)in additions.OrderByDescending(x=>x.Key)){R((valid&(1UL<<(int)t))!=0,"Existing table required");int width=md.GetTableRowSize(t);R(width==(t==TableIndex.MemberRef?6:2),"Pinned row width");int end=md.GetTableMetadataOffset(t)-tableStream+md.GetTableRowCount(t)*width;expanded.InsertRange(end,bytes);}
  var tables=expanded.ToArray();foreach(var(t,bytes)in additions){int at=24+4*Enumerable.Range(0,(int)t).Count(i=>(valid&(1UL<<i))!=0);BitConverter.GetBytes(md.GetTableRowCount(t)+bytes.Length/md.GetTableRowSize(t)).CopyTo(tables,at);}
  ulong sorted=BitConverter.ToUInt64(table,16),clear=1UL<<(int)TableIndex.MemberRef;
  BitConverter.GetBytes(sorted&~clear).CopyTo(tables,16);streams[ti]=new("#~",tables);streams[streams.FindIndex(s=>s.Name=="#Blob")]=new("#Blob",blob);streams[streams.FindIndex(s=>s.Name=="#Strings")]=new("#Strings",strings);
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
  byte[] before=File.ReadAllBytes(args[0]);R(H(before)=="48b9e89e941c041d6c11c4064ad05d9384a1a4dd51d5f8d3fad9192cbda5adbf","Pinned untested Native82 parent");
  using var asm=Mono.Cecil.AssemblyDefinition.ReadAssembly(args[0]);var module=asm.MainModule;using var fs=File.OpenRead(args[0]);using var pe=new PEReader(fs);
  var(after,metadata)=Metadata(before,pe,module);int first=0x0A000000+pe.GetMetadataReader().GetTableRowCount(TableIndex.MemberRef)+1;
  MD M(int token)=>(MD)module.LookupToken(token);
  FR F(string owner,string name)=>module.GetTypes().Single(t=>t.FullName==owner).Fields.Single(f=>f.Name==name);
  MR C(MD method,string name)=>method.Body.Instructions.Select(i=>i.Operand).OfType<MR>().First(r=>r.Name==name);
  int localFirst=pe.GetMetadataReader().GetTableRowCount(TableIndex.StandAloneSig)+1;
  var targets=new Dictionary<int,(byte[] code,int max)>();var report=new List<object>();void Set(int token,IL il,int max){targets.Add(token,(il.Done(),max));}
  var m=M(0x06000056);
  R(m.Body.Variables[1].VariableType.FullName=="Enemy"&&m.Body.Variables[2].VariableType.FullName=="Path_enemyEditor"&&m.Body.Variables[3].VariableType.FullName=="UnityEngine.Transform"&&m.Body.Variables[4].VariableType.FullName=="UnityEngine.Transform"&&m.Body.Variables[6].VariableType.FullName=="EnemyPath","Existing path editor captured locals");
  var insertPath=module.GetMemberReferences().OfType<MR>().Where(r=>r.FullName=="System.Void System.Collections.Generic.List`1<EnemyPath>::Insert(System.Int32,!0)").OrderBy(r=>r.MetadataToken.RID).First();
  var il=new IL().Arg(0).Ref(0x7B,F("PathDataEditor","enemy_BoardEditor")).Ref(0x7B,F("Enemy_boardEditor","preEnemy")).S(1)
   .L(1).Ref(0x7B,F("Enemy","gridX")).L(1).Ref(0x7B,F("Enemy","row")).Float(0f).O(0x14).Ref(0x73,M(0x060002A4)).S(6)
   .L(1).Ref(0x7B,F("Enemy","myPath")).O(0x25).Branch(0x2D,"path-list-live").O(0x26,0x14,0x7A)
   .Label("path-list-live").Arg(0).Ref(0x7B,F("PathDataEditor","pathIndex")).Int(1).O(0x58).L(6).Ref(0x6F,insertPath)
   .Arg(0).Ref(0x7B,F("PathDataEditor","path_EnemyEditor_prefab")).Ref(0x28,C(m,"Instantiate")).S(2)
   .L(2).Arg(0).Ref(0x7B,F("PathDataEditor","enemy_BoardEditor")).Ref(0x7D,F("Path_enemyEditor","enemyEditor"))
   .L(2).L(6).Ref(0x7D,F("Path_enemyEditor","enemyPath"))
   .L(2).Ref(0x28,M(0x06000060))
   .L(2).Ref(0x6F,C(m,"get_transform")).S(3)
   .Arg(0).Ref(0x7B,F("PathDataEditor","enemy_BoardEditor")).Ref(0x6F,C(m,"get_transform")).S(4)
   .L(3).L(4).Int(1).Ref(0x6F,C(m,"SetParent"))
   .Arg(0).Ref(0x7B,F("PathDataEditor","enemy_BoardEditor")).Ref(0x7B,F("Enemy_boardEditor","path_EnemyEditors")).O(0x25).Branch(0x2D,"editor-list-live").O(0x26,0x14,0x7A)
   .Label("editor-list-live").Arg(0).Ref(0x7B,F("PathDataEditor","pathIndex")).Int(1).O(0x58).L(2).Ref(0x6F,first)
   .Arg(0).Arg(0).Ref(0x7B,F("PathDataEditor","pathIndex")).Int(1).O(0x58).Ref(0x28,M(0x06000058)).O(0x2A);
  Set(0x06000056,il,4);
  m=M(0x06000119);
  R(m.IsConstructor&&m.DeclaringType.BaseType.FullName=="DeviceEvent"&&m.Body.Variables[5].VariableType.FullName=="Hide"&&m.Body.Variables[10].VariableType.FullName=="UnityEngine.AudioClip"&&m.Body.Variables[11].VariableType.FullName=="UnityEngine.Vector3"&&m.Body.Variables[8].VariableType.FullName=="System.Single","Actual HoleTeleport hierarchy and typed scratch");
  il=new IL().Arg(0).Ref(0x28,M(0x06000117))
   .Arg(0).Arg(1).Ref(0x7D,F("HoleTeleport","zombie"))
   .Arg(0).Arg(2).Ref(0x7D,F("HoleTeleport","gridX"))
   .Arg(0).Arg(3).Ref(0x7D,F("HoleTeleport","gridY"))
   .Int(1).Arg(1).Ref(0x73,M(0x060000FD)).S(5)
   .Arg(1).Ref(0x7B,F("Zombie","buffManager")).L(5).Ref(0x6F,M(0x06000102))
   .Ref(0x7E,F("ResourceManager","zombieClips")).Int(6).Ref(0x6F,C(m,"get_Item")).S(10)
   .Ref(0x28,C(m,"get_main")).Ref(0x6F,C(m,"get_transform")).Ref(0x6F,C(m,"get_position")).S(11)
   .Ref(0x28,C(m,"AudioVolume")).Float(1.2f).O(0x5A,0x6B).S(8)
   .L(10).L(11).L(8).Float(1.5f).Ref(0x28,C(m,"CreateAudioAtPoint")).O(0x26,0x2A);
  Set(0x06000119,il,4);
  m=M(0x060000E0);R(m.IsConstructor&&m.DeclaringType.BaseType.FullName=="System.Object","Actual AttackRange constructor hierarchy");
  MR ListCtor(string item)=>m.Body.Instructions.Select(i=>i.Operand).OfType<MR>().Single(r=>r.FullName=="System.Void System.Collections.Generic.List`1<"+item+">::.ctor()");
  var objectCtor=module.GetMemberReferences().OfType<MR>().Single(r=>r.FullName=="System.Void System.Object::.ctor()");
  il=new IL().Arg(0).Ref(0x73,ListCtor("UnityEngine.Rect")).Ref(0x7D,F("AttackRange","rects"))
   .Arg(0).Ref(0x73,ListCtor("System.Single")).Ref(0x7D,F("AttackRange","radius"))
   .Arg(0).Ref(0x73,ListCtor("UnityEngine.Vector3")).Ref(0x7D,F("AttackRange","centers"))
   .Arg(0).Ref(0x28,objectCtor).O(0x2A);
  Set(0x060000E0,il,2);
  foreach(var(token,(code,max))in targets){
   m=M(token);R(m.Body.ExceptionHandlers.Count==0,"EH relocation unsupported");var sec=pe.PEHeaders.SectionHeaders.Single(x=>m.RVA>=x.VirtualAddress&&m.RVA<x.VirtualAddress+x.SizeOfRawData);int h=sec.PointerToRawData+m.RVA-sec.VirtualAddress,hs=(BitConverter.ToUInt16(before,h)>>12)*4;
   R((before[h]&3)==3&&hs==12,"Pinned fat body");R(code.Length<=m.Body.CodeSize,"Body exceeds reserved slot "+m.FullName);
   Array.Clear(after,h+hs,m.Body.CodeSize);code.CopyTo(after,h+hs);BitConverter.GetBytes(code.Length).CopyTo(after,h+4);BitConverter.GetBytes((ushort)max).CopyTo(after,h+2);
   int oldLocal=m.Body.LocalVarToken.ToInt32(),newLocal=oldLocal;if(newLocal!=oldLocal)BitConverter.GetBytes(newLocal).CopyTo(after,h+8);
   report.Add(new{token=$"0x{token:X8}",method=m.FullName,header=h,code_start=h+hs,reserved_code_size=m.Body.CodeSize,new_code_size=code.Length,old_max=m.Body.MaxStackSize,new_max=max,old_local_sig=oldLocal,local_sig=newLocal,code_sha256=H(code)});
  }
  R(targets.Count==3,"Pinned scope 3");File.WriteAllBytes(args[1],after);File.WriteAllText(args[2],JsonSerializer.Serialize(new{status="UNTESTED_CANDIDATE",input_sha256=H(before),candidate_sha256=H(after),metadata,methods=report,scope="Three original-native-backed bodies including explicit transitive constructor repair; path insertion captures initial Enemy, uses captured pre-allocation grid args and post-constructor current list, rereads editor/index across Insert/Instantiate/LoadData/SetParent callbacks, parent getter precedes child-null failure, wrapping index additions; HoleTeleport calls correct DeviceEvent base, stores fields before one real Hide allocation, retains constructor allocation chain and clip/position/audio capture with1.2 volume and1.5 pitch; AttackRange allocates Rect/Single/Vector3 lists in original order and ends in no-op Object base call, removes diagnostic string; one typed public Path_enemyEditor Insert reference, all local signatures and method RVAs unchanged; no tests executed; parent chain pending"},new JsonSerializerOptions{WriteIndented=true}));Console.WriteLine("CANDIDATE "+H(after));
 }
}
