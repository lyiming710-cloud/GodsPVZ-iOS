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
static class RepairNative80 {
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
  var method=(MD)module.LookupToken(0x060003A0);
  byte[] old=md.GetBlobBytes(md.GetStandaloneSignature(MetadataTokens.StandaloneSignatureHandle((int)method.Body.LocalVarToken.RID)).Signature);
  R(old[0]==7&&old[1]==15&&method.Body.Variables.Count==15&&method.Body.Variables[10].VariableType.FullName=="System.Collections.Generic.List`1/Enumerator<System.Object>"&&method.Body.Variables[12].VariableType.FullName=="System.Collections.Generic.List`1/Enumerator<System.Object>","Pinned damaged brightness local slots");
  var enumType=module.GetMemberReferences().OfType<MR>().Single(r=>r.FullName=="System.Void System.Collections.Generic.List`1/Enumerator<UnityEngine.GameObject>::Dispose()").DeclaringType;
  int enumRid=TS(enumType.FullName);byte[] enumSig=md.GetBlobBytes(md.GetTypeSpecification(MetadataTokens.TypeSpecificationHandle(enumRid)).Signature);
  R(enumSig[0]==0x15&&enumSig[1]==0x11,"Value-type closed enumerator TypeSpec");
  var enumeratorRef=module.GetTypeReferences().Single(t=>t.FullName=="System.Collections.Generic.List`1/Enumerator");
  byte[] damagedEnum=new byte[]{0x15,0x11}.Concat(Compress(checked((int)enumeratorRef.MetadataToken.RID*4+1))).Concat(new byte[]{1,0x1C}).ToArray();
  var positions=Enumerable.Range(2,old.Length-damagedEnum.Length-1).Where(i=>old[i..(i+damagedEnum.Length)].SequenceEqual(damagedEnum)).ToArray();R(positions.Length==2,"Exact two damaged Enumerator<object> encodings");
  byte[] sig=old[..positions[0]].Concat(enumSig).Concat(old[(positions[0]+damagedEnum.Length)..]).ToArray();int ix=Blob(sig);localRows.AddRange(Words(ix));
  added.Add(new{kind="StandAloneSig",token=$"0x{0x11000000+localFirst:X8}",target="0x060003A0",active_local=10,signature_index=ix,old_signature_hex=Convert.ToHexString(old),signature_hex=Convert.ToHexString(sig)});
  R(memberRows.Count==0&&localRows.Count==2,"Only one scoped brightness enumerator local signature");
  var additions=new Dictionary<TableIndex,byte[]>{{TableIndex.StandAloneSig,localRows.ToArray()}};
  ulong valid=BitConverter.ToUInt64(table,8);int tableStream=0,scan2=countAt+2;
  for(int i=0;i<count;i++){int e=Array.IndexOf(root,(byte)0,scan2+8);if(Encoding.ASCII.GetString(root,scan2+8,e-scan2-8)=="#~")tableStream=BitConverter.ToInt32(root,scan2);scan2=Align(e+1,4);}
  R(tableStream>0,"Existing tables offset");var expanded=table.ToList();
  foreach(var(t,bytes)in additions.OrderByDescending(x=>x.Key)){R((valid&(1UL<<(int)t))!=0,"Existing table required");int width=md.GetTableRowSize(t);R(width==(t==TableIndex.MemberRef?6:2),"Pinned row width");int end=md.GetTableMetadataOffset(t)-tableStream+md.GetTableRowCount(t)*width;expanded.InsertRange(end,bytes);}
  var tables=expanded.ToArray();foreach(var(t,bytes)in additions){int at=24+4*Enumerable.Range(0,(int)t).Count(i=>(valid&(1UL<<i))!=0);BitConverter.GetBytes(md.GetTableRowCount(t)+bytes.Length/md.GetTableRowSize(t)).CopyTo(tables,at);}
  ulong sorted=BitConverter.ToUInt64(table,16),clear=1UL<<(int)TableIndex.StandAloneSig;
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
  byte[] before=File.ReadAllBytes(args[0]);R(H(before)=="3b3bcfca51402e1eedf5d59fbe1bf9d923336730325550205f960bd8a8bceec6","Pinned untested Native79 parent");
  using var asm=Mono.Cecil.AssemblyDefinition.ReadAssembly(args[0]);var module=asm.MainModule;using var fs=File.OpenRead(args[0]);using var pe=new PEReader(fs);
  var(after,metadata)=Metadata(before,pe,module);int first=0x0A000000+pe.GetMetadataReader().GetTableRowCount(TableIndex.MemberRef)+1;
  MD M(int token)=>(MD)module.LookupToken(token);
  FR F(string owner,string name)=>module.GetTypes().Single(t=>t.FullName==owner).Fields.Single(f=>f.Name==name);
  MR C(MD method,string name)=>method.Body.Instructions.Select(i=>i.Operand).OfType<MR>().First(r=>r.Name==name);
  int localFirst=pe.GetMetadataReader().GetTableRowCount(TableIndex.StandAloneSig)+1;
  var targets=new Dictionary<int,(byte[] code,int max)>();var report=new List<object>();void Set(int token,IL il,int max){targets.Add(token,(il.Done(),max));}
  var random=module.GetMemberReferences().OfType<MR>().Single(r=>r.FullName=="System.Single UnityEngine.Random::Range(System.Single,System.Single)");
  var m=M(0x06000206);
  R(m.Body.Variables[0].VariableType.FullName=="System.Single"&&m.Body.Variables[1].VariableType.FullName=="ProjectType"&&m.Body.Variables[8].VariableType.FullName=="UnityEngine.Vector3"&&m.Body.Variables[9].VariableType.FullName=="UnityEngine.AudioClip"&&m.Body.Variables[17].VariableType.FullName=="System.Single","Existing audio capture scratch");
  var il=new IL().Ref(0x28,C(m,"AudioVolume")).S(0).Float(1f).S(17).Int(0).S(1)
   .Arg(1).Ref(0x7B,F("Project","projectType")).Switch("sun","coin","coin","common","money","common","common","common","coin").Branch(0x2B,"common")
   .Label("sun").Float(0.9f).Float(1.1f).Ref(0x28,random).S(17).Int(4).S(1).Branch(0x2B,"common")
   .Label("coin").L(0).Float(0.8f).O(0x5A,0x6B).S(0)
   .Label("money").Float(1f).Float(1.8f).Ref(0x28,random).S(17).Int(1).S(1)
   .Label("common").Ref(0x28,C(m,"get_main")).Ref(0x6F,C(m,"get_transform")).Ref(0x6F,C(m,"get_position")).S(8)
   .Ref(0x7E,F("ResourceManager","projectClips")).L(1).Ref(0x6F,C(m,"get_Item")).S(9)
   .L(9).L(8).L(0).L(17).Ref(0x28,C(m,"CreateAudioAtPoint")).O(0x26,0x2A);
  Set(0x06000206,il,4);
  foreach(var(token,(code,max))in targets){
   m=M(token);R(m.Body.ExceptionHandlers.Count==0,"EH relocation unsupported");var sec=pe.PEHeaders.SectionHeaders.Single(x=>m.RVA>=x.VirtualAddress&&m.RVA<x.VirtualAddress+x.SizeOfRawData);int h=sec.PointerToRawData+m.RVA-sec.VirtualAddress,hs=(BitConverter.ToUInt16(before,h)>>12)*4;
   R((before[h]&3)==3&&hs==12,"Pinned fat body");R(code.Length<=m.Body.CodeSize,"Body exceeds reserved slot "+m.FullName);
   Array.Clear(after,h+hs,m.Body.CodeSize);code.CopyTo(after,h+hs);BitConverter.GetBytes(code.Length).CopyTo(after,h+4);BitConverter.GetBytes((ushort)max).CopyTo(after,h+2);
   int oldLocal=m.Body.LocalVarToken.ToInt32(),newLocal=oldLocal;if(newLocal!=oldLocal)BitConverter.GetBytes(newLocal).CopyTo(after,h+8);
   report.Add(new{token=$"0x{token:X8}",method=m.FullName,header=h,code_start=h+hs,reserved_code_size=m.Body.CodeSize,new_code_size=code.Length,old_max=m.Body.MaxStackSize,new_max=max,old_local_sig=oldLocal,local_sig=newLocal,code_sha256=H(code)});
  }
  m=M(0x060003A0);R(m.Body.ExceptionHandlers.Count==0,"Pinned damaged brightness no-EH body");
  int Binding(string name){string owner=name=="GetEnumerator"?"System.Collections.Generic.List`1<UnityEngine.GameObject>":"System.Collections.Generic.List`1/Enumerator<UnityEngine.GameObject>";return module.GetMemberReferences().OfType<MR>().Single(r=>r.Name==name&&r.DeclaringType.FullName==owner&&r.Parameters.Count==0).MetadataToken.ToInt32();}
  var material=module.GetMemberReferences().OfType<MR>().Single(r=>r.FullName=="UnityEngine.Material UnityEngine.Renderer::get_material()");
  il=new IL().Arg(0).Ref(0x7B,F("Plant","animationSprites")).Ref(0x6F,Binding("GetEnumerator")).S(10);int tryStart=il.B.Count;
  il.Branch(0x2B,"check").Label("body").A(10).Ref(0x28,Binding("get_Current")).Ref(0x6F,C(m,"GetComponent"))
   .Ref(0x6F,material).Ref(0x7E,F("Plant","Mat_Brightness")).Arg(1).Ref(0x6F,C(m,"SetFloat"))
   .Label("check").A(10).Ref(0x28,Binding("MoveNext")).Branch(0x2D,"body").Branch(0xDE,"return");int finallyStart=il.B.Count;
  il.A(10).Ref(0x28,Binding("Dispose")).O(0xDC);int finallyEnd=il.B.Count;
  il.Label("return").O(0x2A);byte[] enumCode=il.Done();
  var section=pe.PEHeaders.SectionHeaders.Single(x=>m.RVA>=x.VirtualAddress&&m.RVA<x.VirtualAddress+x.SizeOfRawData);int header=section.PointerToRawData+m.RVA-section.VirtualAddress;
  R((before[header]&3)==3&&(BitConverter.ToUInt16(before,header)>>12)==3,"Pinned fat brightness header");int eh=Align(header+12+enumCode.Length,4),extent=eh+28-(header+12);
  R(extent<=m.Body.CodeSize,"Brightness body/finally fits reserved slot");Array.Clear(after,header+12,m.Body.CodeSize);enumCode.CopyTo(after,header+12);
  BitConverter.GetBytes((ushort)(BitConverter.ToUInt16(before,header)|8)).CopyTo(after,header);BitConverter.GetBytes((ushort)3).CopyTo(after,header+2);BitConverter.GetBytes(enumCode.Length).CopyTo(after,header+4);int local=0x11000000+localFirst;BitConverter.GetBytes(local).CopyTo(after,header+8);
  after[eh]=0x41;after[eh+1]=28;foreach(var(offset,value)in new[]{(4,2),(8,tryStart),(12,finallyStart-tryStart),(16,finallyStart),(20,finallyEnd-finallyStart),(24,0)})BitConverter.GetBytes(value).CopyTo(after,eh+offset);
  report.Add(new{token="0x060003A0",method=m.FullName,header,code_start=header+12,reserved_code_size=m.Body.CodeSize,new_code_size=enumCode.Length,old_max=m.Body.MaxStackSize,new_max=3,old_local_sig=m.Body.LocalVarToken.ToInt32(),local_sig=local,code_sha256=H(enumCode),new_finally=new{section_offset=eh,try_start=tryStart,try_length=finallyStart-tryStart,handler_start=finallyStart,handler_length=finallyEnd-finallyStart}});
  R(report.Count==2,"Pinned scope 2");File.WriteAllBytes(args[1],after);File.WriteAllText(args[2],JsonSerializer.Serialize(new{status="UNTESTED_CANDIDATE",input_sha256=H(before),candidate_sha256=H(after),metadata,methods=report,scope="Two original-native-backed bodies; typed GameObject foreach with finally, per-iteration static brightness name and raw-null faults, no brightness assignment or clamp; public Renderer.get_material original native pointer aliases private GetMaterial but full reference persistent branch explicitly requires target-runtime qualification; original audio float dispatch and order/captured arguments; only one scoped enumerator local signature added, no MemberRef additions; no tests executed; unqualified parent chain pending"},new JsonSerializerOptions{WriteIndented=true}));Console.WriteLine("CANDIDATE "+H(after));
 }
}
