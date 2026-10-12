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
static class RepairNative70 {
 static void R(bool x,string m){if(!x)throw new InvalidOperationException(m);}
 static string H(byte[] b)=>Convert.ToHexString(SHA256.HashData(b)).ToLowerInvariant();
 static int Align(int x,int a)=>checked((x+a-1)/a*a);
 class IL {
  public List<byte> B=new();Dictionary<string,int> labels=new();List<(int p,string l,int width)> branches=new();
  public IL O(params byte[] x){B.AddRange(x);return this;}public IL I(int x){B.AddRange(BitConverter.GetBytes(x));return this;}
  public IL Ref(byte op,IMetadataTokenProvider r)=>O(op).I(r.MetadataToken.ToInt32());public IL Ref(byte op,int t)=>O(op).I(t);
  public IL Int(int x)=>x>=-1&&x<=8?O((byte)(x+0x16)):O(0x20).I(x);public IL Float(float x)=>O(0x22).I(BitConverter.SingleToInt32Bits(x));
  public IL Double(double x)=>O(0x23).O(BitConverter.GetBytes(x));
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
  var memberRows=new List<byte>();var added=new List<object>();int memberFirst=md.GetTableRowCount(TableIndex.MemberRef)+1;
  int Member(string name,int parent,byte[] signature){int rid=memberFirst+memberRows.Count/6,ni=Name(name),si=Blob(signature);memberRows.AddRange(Words(parent,ni,si));added.Add(new{kind="MemberRef",token=$"0x{0x0A000000+rid:X8}",name,parent,name_index=ni,signature_index=si});return 0x0A000000+rid;}
  int popup=TS("System.Collections.Generic.List`1<Popup>")*8+4;
  Member("get_Count",popup,new byte[]{0x20,0,8});Member("RemoveAt",popup,new byte[]{0x20,1,1,8});
  R(memberRows.Count==12,"Exactly two public popup bindings");
  R(md.GetTableRowCount(TableIndex.TypeSpec)<8192&&md.GetTableRowCount(TableIndex.TypeRef)<8192&&md.GetTableRowCount(TableIndex.TypeDef)<8192&&md.GetTableRowCount(TableIndex.MethodDef)<8192,"No parent index widening");
  var additions=new Dictionary<TableIndex,byte[]>{{TableIndex.MemberRef,memberRows.ToArray()}};
  ulong valid=BitConverter.ToUInt64(table,8);int tableStream=0,scan2=countAt+2;
  for(int i=0;i<count;i++){int e=Array.IndexOf(root,(byte)0,scan2+8);if(Encoding.ASCII.GetString(root,scan2+8,e-scan2-8)=="#~")tableStream=BitConverter.ToInt32(root,scan2);scan2=Align(e+1,4);}
  R(tableStream>0,"Existing tables offset");var expanded=table.ToList();
  foreach(var(t,bytes)in additions.OrderByDescending(x=>x.Key)){R((valid&(1UL<<(int)t))!=0,"Existing table required");int width=md.GetTableRowSize(t);R(width==(t==TableIndex.MemberRef?6:2),"Pinned row width");int end=md.GetTableMetadataOffset(t)-tableStream+md.GetTableRowCount(t)*width;expanded.InsertRange(end,bytes);}
  var tables=expanded.ToArray();foreach(var(t,bytes)in additions){int at=24+4*Enumerable.Range(0,(int)t).Count(i=>(valid&(1UL<<i))!=0);BitConverter.GetBytes(md.GetTableRowCount(t)+bytes.Length/md.GetTableRowSize(t)).CopyTo(tables,at);}
  ulong sorted=BitConverter.ToUInt64(table,16),clear=(1UL<<(int)TableIndex.MemberRef);
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
  byte[] before=File.ReadAllBytes(args[0]);R(H(before)=="67611b6286fe37c58f706eaab114037bc1bce4a10187a3f28e8327e519e28359","Pinned untested Native69 parent");
  using var asm=Mono.Cecil.AssemblyDefinition.ReadAssembly(args[0]);var module=asm.MainModule;using var fs=File.OpenRead(args[0]);using var pe=new PEReader(fs);
  var(after,metadata)=Metadata(before,pe,module);int first=0x0A000000+pe.GetMetadataReader().GetTableRowCount(TableIndex.MemberRef)+1;
  MD M(int token)=>(MD)module.LookupToken(token);
  FR F(string owner,string name)=>module.GetTypes().Single(t=>t.FullName==owner).Fields.Single(f=>f.Name==name);
  MR C(MD method,string name)=>method.Body.Instructions.Select(i=>i.Operand).OfType<MR>().First(r=>r.Name==name);
  int localFirst=pe.GetMetadataReader().GetTableRowCount(TableIndex.StandAloneSig)+1;
  var targets=new Dictionary<int,(byte[] code,int max)>();var report=new List<object>();void Set(int token,IL il,int max){targets.Add(token,(il.Done(),max));}
  var m=M(0x060002AE);
  R(m.Body.Variables[0].VariableType.FullName=="System.Collections.Generic.List`1<Popup>"&&m.Body.Variables[1].VariableType.FullName=="System.Boolean","Existing popup/bool scratch");
  var il=new IL().Arg(0).Ref(0x7B,F("Board","popups")).S(0).L(0).Ref(0x6F,first).Int(0).Branch(0x3E,"return")
   .L(0).Int(0).Ref(0x6F,C(m,"get_Item")).O(0x14).Ref(0x28,C(m,"op_Equality")).S(1)
   .Arg(0).Ref(0x7B,F("Board","popups")).S(0).L(1).Branch(0x3A,"remove")
   .L(0).Int(0).Ref(0x6F,C(m,"get_Item")).Ref(0x6F,C(m,"get_gameObject")).Ref(0x6F,C(m,"get_activeSelf")).Branch(0x3A,"return")
   .Arg(0).Ref(0x7B,F("Board","popups")).S(0).Branch(0x38,"activate")
   .Label("remove").L(0).Int(0).Ref(0x6F,first+1)
   .Arg(0).Ref(0x7B,F("Board","popups")).S(0).L(0).Ref(0x6F,first).Int(0).Branch(0x3E,"return")
   .Label("activate").L(0).Int(0).Ref(0x6F,C(m,"get_Item")).Ref(0x6F,C(m,"get_gameObject")).Int(1).Ref(0x6F,C(m,"SetActive"))
   .Label("return").O(0x2A);
  Set(0x060002AE,il,2);
  m=M(0x060003AA);
  il=new IL().Arg(3).Ref(0x7B,F("Zombie","armor1Type")).Branch(0x3A,"normal")
   .Arg(3).Ref(0x7B,F("Zombie","armor2Type")).Branch(0x3A,"normal")
   .Arg(4).Ref(0x7B,F("Damage","damagePoint")).Float(7f).O(0x5A,0x6B).Arg(3).Ref(0x7B,F("Zombie","healthPoint")).Branch(0x43,"normal")
   .Arg(2).Arg(3).Ref(0x6F,C(m,"Add")).O(0x2A)
   .Label("normal").Arg(1).Arg(3).Ref(0x6F,C(m,"Add")).O(0x2A);
  Set(0x060003AA,il,2);
  m=M(0x06000318);
  R(m.Body.Variables[2].VariableType.FullName=="System.Single"&&m.Body.Variables[4].VariableType.FullName=="System.Single","Existing health/scale scratch");
  var vector=module.GetMemberReferences().OfType<MR>().Single(r=>r.FullName=="System.Void UnityEngine.Vector3::.ctor(System.Single,System.Single,System.Single)");
  il=new IL().Arg(0).Ref(0x7B,F("Device","healthPoint")).S(2).L(2).Float(0f).Branch(0x43,"return")
   .Arg(0).L(2).Ref(0x28,C(m,"get_deltaTime")).Float(10f).O(0x5A,0x6B,0x59,0x6B).Ref(0x7D,F("Device","healthPoint"))
   .Arg(0).Ref(0x28,C(m,"DC_BonfireWarm"))
   .Arg(0).Ref(0x7B,F("Device","healthPoint")).Float(0f).Branch(0x42,"scale")
   .Arg(0).Float(0f).Ref(0x7D,F("Device","healthPoint")).Arg(0).Ref(0x28,C(m,"Broken"))
   .Label("scale").Arg(0).Ref(0x7B,F("Device","healthPoint")).Arg(0).Ref(0x7B,F("Device","maxHealthPoint")).O(0x5B,0x6B,0x6C).Double(0.5d).Ref(0x28,C(m,"Pow")).O(0x6B).S(4)
   .Arg(0).Ref(0x7B,F("Device","animationGroup")).Ref(0x6F,C(m,"get_transform")).Int(1).Ref(0x6F,C(m,"GetChild"))
   .L(4).L(4).Float(0f).Ref(0x73,vector).Ref(0x6F,C(m,"set_localScale"))
   .Label("return").O(0x2A);
  Set(0x06000318,il,4);
  foreach(var(token,(code,max))in targets){
   m=M(token);R(m.Body.ExceptionHandlers.Count==0,"EH relocation unsupported");var sec=pe.PEHeaders.SectionHeaders.Single(x=>m.RVA>=x.VirtualAddress&&m.RVA<x.VirtualAddress+x.SizeOfRawData);int h=sec.PointerToRawData+m.RVA-sec.VirtualAddress,hs=(BitConverter.ToUInt16(before,h)>>12)*4;
   R((before[h]&3)==3&&hs==12,"Pinned fat body");R(code.Length<=m.Body.CodeSize,"Body exceeds reserved slot "+m.FullName);
   Array.Clear(after,h+hs,m.Body.CodeSize);code.CopyTo(after,h+hs);BitConverter.GetBytes(code.Length).CopyTo(after,h+4);BitConverter.GetBytes((ushort)max).CopyTo(after,h+2);
   report.Add(new{token=$"0x{token:X8}",method=m.FullName,header=h,code_start=h+hs,reserved_code_size=m.Body.CodeSize,new_code_size=code.Length,old_max=m.Body.MaxStackSize,new_max=max,local_sig=m.Body.LocalVarToken.ToInt32(),code_sha256=H(code)});
  }
  m=M(0x060001FA);R(m.Body.ExceptionHandlers.Count==0,"Pinned damaged no-EH project iterator");
  int ProjectBinding(string name){string owner=name=="GetEnumerator"?"System.Collections.Generic.List`1<Project>":"System.Collections.Generic.List`1/Enumerator<Project>";return module.GetMemberReferences().OfType<MR>().Where(r=>r.Name==name&&r.DeclaringType.FullName==owner&&r.HasThis&&r.Parameters.Count==0).OrderByDescending(r=>r.MetadataToken.RID).First().MetadataToken.ToInt32();}
  var reusable=M(0x06000261);R(reusable.Body.Variables.Count==2&&reusable.Body.Variables[0].VariableType.FullName=="System.Collections.Generic.List`1/Enumerator<Project>"&&reusable.Body.Variables[1].VariableType.FullName=="Project","Reuse typed enum/current signature from Native66");
  il=new IL().Arg(0).Ref(0x7B,F("ProjectManager","projects")).Ref(0x6F,ProjectBinding("GetEnumerator")).S(0);
  int tryStart=il.B.Count;
  il.Label("loop").A(0).Ref(0x28,ProjectBinding("MoveNext")).Branch(0x2C,"leave").A(0).Ref(0x28,ProjectBinding("get_Current")).S(1)
   .L(1).Ref(0x7B,F("Project","ID")).Int(1).Branch(0x2E,"qualifying-id")
   .L(1).Ref(0x7B,F("Project","ID")).Int(2).Branch(0x2E,"qualifying-id")
   .L(1).Ref(0x7B,F("Project","ID")).Int(8).Branch(0x33,"loop")
   .Label("qualifying-id").L(1).Ref(0x7B,F("Project","movementTracks")).Int(5).Branch(0x33,"loop")
   .L(1).Ref(0x7B,F("Project","stayedTime")).Float(0.4f).Branch(0x37,"loop")
   .L(1).Arg(0).Ref(0x7B,F("ProjectManager","board")).Ref(0x7B,F("Board","mouseManager")).Ref(0x6F,C(m,"Click")).Branch(0x2B,"loop")
   .Label("leave").Branch(0xDE,"return");int finallyStart=il.B.Count;
  il.A(0).Ref(0x28,ProjectBinding("Dispose")).O(0xDC);int finallyEnd=il.B.Count;
  il.Label("return").O(0x2A);byte[] enumCode=il.Done();
  var section=pe.PEHeaders.SectionHeaders.Single(x=>m.RVA>=x.VirtualAddress&&m.RVA<x.VirtualAddress+x.SizeOfRawData);int header=section.PointerToRawData+m.RVA-section.VirtualAddress;
  R((before[header]&3)==3&&(BitConverter.ToUInt16(before,header)>>12)==3,"Existing fat iterator header");int ehRelative=Align(12+enumCode.Length,4),extent=ehRelative+28-12;
  R(extent<=m.Body.CodeSize,"Project iterator body and finally fit reserved slot");Array.Clear(after,header+12,m.Body.CodeSize);enumCode.CopyTo(after,header+12);
  BitConverter.GetBytes((ushort)(BitConverter.ToUInt16(before,header)|0x18)).CopyTo(after,header);BitConverter.GetBytes((ushort)2).CopyTo(after,header+2);BitConverter.GetBytes(enumCode.Length).CopyTo(after,header+4);
  int local=reusable.Body.LocalVarToken.ToInt32();BitConverter.GetBytes(local).CopyTo(after,header+8);int eh=header+ehRelative;new byte[]{0x41,28,0,0}.CopyTo(after,eh);
  foreach(var(offset,value)in new[]{(4,2),(8,tryStart),(12,finallyStart-tryStart),(16,finallyStart),(20,finallyEnd-finallyStart),(24,0)})BitConverter.GetBytes(value).CopyTo(after,eh+offset);
  report.Add(new{token="0x060001FA",method=m.FullName,header,code_start=header+12,reserved_code_size=m.Body.CodeSize,new_code_size=enumCode.Length,old_max=m.Body.MaxStackSize,new_max=2,old_local_sig=m.Body.LocalVarToken.ToInt32(),local_sig=local,code_sha256=H(enumCode),new_finally=new{section_offset=eh,try_start=tryStart,try_length=finallyStart-tryStart,handler_start=finallyStart,handler_length=finallyEnd-finallyStart}});
  R(report.Count==4,"Pinned scope 4");File.WriteAllBytes(args[1],after);File.WriteAllText(args[2],JsonSerializer.Serialize(new{status="UNTESTED_CANDIDATE",input_sha256=H(before),candidate_sha256=H(after),metadata,methods=report,scope="Four native-backed bodies; project-ID filtered foreach with finally, popup queue handling, chomper classification and bonfire scale; two public Popup bindings appended; existing enum/local signatures and Vector3 constructor reused; scalar native constants and NaN guards retained; unqualified parent chain pending"},new JsonSerializerOptions{WriteIndented=true}));Console.WriteLine("CANDIDATE "+H(after));
 }
}
