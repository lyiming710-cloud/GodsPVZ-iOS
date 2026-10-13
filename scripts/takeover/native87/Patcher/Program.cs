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
static class RepairNative87 {
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
  byte[] before=File.ReadAllBytes(args[0]);R(H(before)=="20ec9187fb3edc51d80cd486a70f6e5e91d7a1958ce4f4db456a2bee7124fa11","Pinned untested Native86 parent");
  using var asm=Mono.Cecil.AssemblyDefinition.ReadAssembly(args[0]);var module=asm.MainModule;using var fs=File.OpenRead(args[0]);using var pe=new PEReader(fs);
  byte[] after=before.ToArray();
  MD M(int token)=>(MD)module.LookupToken(token);
  FR F(string owner,string name)=>module.GetTypes().Single(t=>t.FullName==owner).Fields.Single(f=>f.Name==name);
  MR C(MD method,string name)=>method.Body.Instructions.Select(i=>i.Operand).OfType<MR>().First(r=>r.Name==name);
  int localFirst=pe.GetMetadataReader().GetTableRowCount(TableIndex.StandAloneSig)+1;
  var targets=new Dictionary<int,(byte[] code,int max)>();var report=new List<object>();void Set(int token,IL il,int max){targets.Add(token,(il.Done(),max));}
  int StringToken(MD method,string text){var i=method.Body.Instructions.Single(i=>i.OpCode==OpCodes.Ldstr&&(string)i.Operand==text);var sec=pe.PEHeaders.SectionHeaders.Single(s=>method.RVA>=s.VirtualAddress&&method.RVA<s.VirtualAddress+s.SizeOfRawData);int header=sec.PointerToRawData+method.RVA-sec.VirtualAddress;R((before[header]&3)==3&&(BitConverter.ToUInt16(before,header)>>12)==3,"Pinned fat literal body");return BitConverter.ToInt32(before,header+12+i.Offset+1);}
  var m=M(0x06000199);R(m.Body.Variables[1].VariableType.FullName=="System.Single"&&m.Body.Variables[3].VariableType.FullName=="System.Object"&&m.Body.Variables[11].VariableType.FullName=="System.Object"&&m.Body.Variables[7].VariableType.FullName=="UnityEngine.Camera"&&m.Body.Variables[9].VariableType.FullName=="UnityEngine.Transform"&&m.Body.Variables[10].VariableType.FullName=="UnityEngine.Vector3","Original dialogue captures with proven typed sources");
  var audioType=module.GetTypeReferences().Single(t=>t.FullName=="UnityEngine.AudioClip");R(audioType.MetadataToken.ToInt32()==0x0100002D,"Exact clip TypeRef");
  var clipList=module.LookupToken(0x1B00001A) as Mono.Cecil.TypeReference;R(clipList?.FullName=="System.Collections.Generic.List`1<UnityEngine.AudioClip>","Exact closed List<AudioClip>");
  var il=new IL().Ref(0x28,M(0x06000138)).S(1).O(0x14).S(3)
   .Arg(1).Switch("check-clip","check-clip","index-zero","range-one","range-four","range-seven","range-nine").Branch(0x38,"check-clip")
   .Label("index-zero").Ref(0x7E,F("ResourceManager","dialogueClips")).S(11).Int(0).S(0).Branch(0x38,"list-check")
   .Label("range-one").Ref(0x7E,F("ResourceManager","dialogueClips")).S(11).Int(1).Int(3).Branch(0x38,"random")
   .Label("range-four").Ref(0x7E,F("ResourceManager","dialogueClips")).S(11).Int(4).Int(3).Branch(0x38,"random")
   .Label("range-seven").Ref(0x7E,F("ResourceManager","dialogueClips")).S(11).Int(7).Int(2).Branch(0x38,"random")
   .Label("range-nine").Ref(0x7E,F("ResourceManager","dialogueClips")).S(11).Int(9).Int(3)
   .Label("random").Ref(0x28,0x0A00012B).S(0)
   .Label("list-check").L(11).Ref(0x74,clipList!).O(0x25).Branch(0x3A,"list-present").O(0x26).Branch(0x38,"fail")
   .Label("list-present").L(0).O(0x69).Ref(0x6F,0x0A000084).S(3)
   .Label("check-clip").L(3).Ref(0x74,audioType).O(0x14).Ref(0x28,0x0A00000C).Branch(0x39,"return")
   .Ref(0x28,0x0A000004).S(7).L(7).Branch(0x39,"fail")
   .L(7).Ref(0x6F,0x0A000001).S(9).L(9).Branch(0x39,"fail")
   .L(9).Ref(0x6F,0x0A000037).S(10)
   .L(3).Ref(0x74,audioType).L(10).L(1).Float(1f).Ref(0x28,M(0x0600013E)).O(0x26)
   .Label("return").O(0x2A).Label("fail").O(0x14,0x7A);
  Set(0x06000199,il,4);
  m=M(0x06000386);R(m.Body.Variables[1].VariableType.FullName=="Damage"&&m.Body.Variables[3].VariableType.FullName=="UnityEngine.GameObject"&&m.Body.Variables[4].VariableType.FullName=="UnityEngine.Transform"&&m.Body.Variables[5].VariableType.FullName=="UnityEngine.Vector3"&&m.Body.Variables[6].VariableType.FullName=="UnityEngine.Transform"&&m.Body.Variables[7].VariableType.FullName=="UnityEngine.Camera"&&m.Body.Variables[8].VariableType.FullName=="UnityEngine.Transform"&&m.Body.Variables[11].VariableType.FullName=="UnityEngine.AudioClip"&&m.Body.Variables[12].VariableType.FullName=="UnityEngine.Vector3"&&m.Body.Variables[13].VariableType.FullName=="System.Single"&&m.Body.Variables[16].VariableType.FullName=="System.Single","Exact typed PotatoBoom captures");
  il=new IL().Arg(0).O(0x14).Int(-1).Int(0).Ref(0x28,M(0x0600036D)).S(1).L(1).Branch(0x39,"fail")
   .L(1).Ref(0x6F,M(0x0600010C)).O(0x26)
   .Arg(0).Ref(0x7B,F("Plant","board")).O(0x25).Branch(0x3A,"board-present").O(0x26).Branch(0x38,"fail")
   .Label("board-present").Float(0.08f).Float(5f).Ref(0x6F,M(0x060002CD))
   .Ref(0x28,M(0x060001E5)).O(0x25).Branch(0x3A,"manager-present").O(0x26).Branch(0x38,"fail")
   .Label("manager-present").Int(23).Ref(0x6F,M(0x060001EA)).S(3).L(3).Branch(0x39,"fail")
   .L(3).Ref(0x6F,0x0A00002E).S(4)
   .Arg(0).Ref(0x6F,0x0A000001).S(6).L(6).Branch(0x39,"fail")
   .L(6).Ref(0x6F,0x0A000037).S(5).L(4).Branch(0x39,"fail")
   .L(4).L(5).Ref(0x6F,0x0A000038)
   .L(3).Ref(0x6F,0x0A00002E).O(0x25).Branch(0x3A,"scale-target-present").O(0x26).Branch(0x38,"fail")
   .Label("scale-target-present").Float(1.2f).Float(1.2f).Float(1.2f).Ref(0x73,0x0A00009C).Ref(0x6F,0x0A0000E5)
   .Ref(0x7E,F("ResourceManager","particleClips")).O(0x25).Branch(0x3A,"list-present").O(0x26).Branch(0x38,"fail")
   .Label("list-present").Int(0).Ref(0x6F,0x0A000084).S(11)
   .Ref(0x28,0x0A000004).S(7).L(7).Branch(0x39,"fail")
   .L(7).Ref(0x6F,0x0A000001).S(8).L(8).Branch(0x39,"fail")
   .L(8).Ref(0x6F,0x0A000037).S(12)
   .Ref(0x28,M(0x06000138)).S(13)
   .Float(0.9f).Float(1.1f).Ref(0x28,0x0A000083).S(16)
   .L(11).L(12).L(13).L(16).Ref(0x28,M(0x0600013E)).O(0x26)
   .Arg(0).Ref(0x28,M(0x06000397)).O(0x2A).Label("fail").O(0x14,0x7A);
  Set(0x06000386,il,4);
  foreach(var(token,(code,max))in targets){
   m=M(token);R(m.Body.ExceptionHandlers.Count==0,"No EH target");
   if(code.Length>m.Body.CodeSize){var relocated=AppendBody(after,m,code,max);after=relocated.Image;report.Add(new{token=$"0x{token:X8}",method=m.FullName,old_reserved_code_size=m.Body.CodeSize,new_code_size=code.Length,old_max=m.Body.MaxStackSize,new_max=max,local_sig=m.Body.LocalVarToken.ToInt32(),code_sha256=H(code),relocation=relocated.Evidence});continue;}
   var sec=pe.PEHeaders.SectionHeaders.Single(x=>m.RVA>=x.VirtualAddress&&m.RVA<x.VirtualAddress+x.SizeOfRawData);int h=sec.PointerToRawData+m.RVA-sec.VirtualAddress;
   R((before[h]&3)==3&&(BitConverter.ToUInt16(before,h)>>12)==3,"Pinned fat header");Array.Clear(after,h+12,m.Body.CodeSize);code.CopyTo(after,h+12);BitConverter.GetBytes(code.Length).CopyTo(after,h+4);BitConverter.GetBytes((ushort)max).CopyTo(after,h+2);
   report.Add(new{token=$"0x{token:X8}",method=m.FullName,header=h,code_start=h+12,reserved_code_size=m.Body.CodeSize,new_code_size=code.Length,old_max=m.Body.MaxStackSize,new_max=max,local_sig=m.Body.LocalVarToken.ToInt32(),code_sha256=H(code)});
  }
  R(targets.Count==2,"Pinned scope 2");File.WriteAllBytes(args[1],after);File.WriteAllText(args[2],JsonSerializer.Serialize(new{status="UNTESTED_CANDIDATE",input_sha256=H(before),candidate_sha256=H(after),metadata_scope="Existing table rows except target MethodDef RVA remain unchanged by construction; no new rows or heap edits",methods=report,scope="Two complete original-native-backed callers; PlayAudio captures AudioVolume first, original seven-entry enum dispatch, captures dialogue list before exact Random pairs and checks it afterwards, retained clip Unity inequality on every path, typed position and pitch1; PotatoBoom discards AreaDamage bool, rereads board, exact Shake order, legal pure Particles getter, delayed first-effect null failure after plant position, second transform getter on same particle, captured clip without null skip and ordered Camera-volume-Random-audio-PlantDie; full bodies only target RVA relocations if oversize, all signatures/metadata rows otherwise intact; dependency M0600013E known clip-length/timeScale ordering gap remains explicitly unqualified pending separate repair; no tests executed; parent chain pending"},new JsonSerializerOptions{WriteIndented=true}));Console.WriteLine("CANDIDATE "+H(after));
 }
}
