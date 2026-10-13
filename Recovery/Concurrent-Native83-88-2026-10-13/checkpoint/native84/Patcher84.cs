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
static class RepairNative84 {
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
  byte[] before=File.ReadAllBytes(args[0]);R(H(before)=="d1727135f7fcb7141a62ad5bbe0e2342e6d25efb97d25b59ae223bb26fcd9975","Pinned untested Native83 parent");
  using var asm=Mono.Cecil.AssemblyDefinition.ReadAssembly(args[0]);var module=asm.MainModule;using var fs=File.OpenRead(args[0]);using var pe=new PEReader(fs);
  byte[] after=before.ToArray();
  MD M(int token)=>(MD)module.LookupToken(token);
  FR F(string owner,string name)=>module.GetTypes().Single(t=>t.FullName==owner).Fields.Single(f=>f.Name==name);
  MR C(MD method,string name)=>method.Body.Instructions.Select(i=>i.Operand).OfType<MR>().First(r=>r.Name==name);
  int localFirst=pe.GetMetadataReader().GetTableRowCount(TableIndex.StandAloneSig)+1;
  var targets=new Dictionary<int,(byte[] code,int max)>();var report=new List<object>();void Set(int token,IL il,int max){targets.Add(token,(il.Done(),max));}
  int StringToken(MD method,string text){var i=method.Body.Instructions.Single(i=>i.OpCode==OpCodes.Ldstr&&(string)i.Operand==text);var sec=pe.PEHeaders.SectionHeaders.Single(s=>method.RVA>=s.VirtualAddress&&method.RVA<s.VirtualAddress+s.SizeOfRawData);int header=sec.PointerToRawData+method.RVA-sec.VirtualAddress;R((before[header]&3)==3&&(BitConverter.ToUInt16(before,header)>>12)==3,"Pinned fat literal body");return BitConverter.ToInt32(before,header+12+i.Offset+1);}
  var m=M(0x060001C8);R(m.HasThis&&m.Parameters.Count==0&&m.Body.ExceptionHandlers.Count==0,"Pinned DropDownAll signature");
  var il=new IL().Arg(0).Ref(0x7B,F("MouseManager","handItemType"))
   .Switch("none","card","return","return","skill","shovel","glove").Branch(0x38,"return")
   .Label("none").Arg(0).Ref(0x7B,F("MouseManager","board")).O(0x25).Ref(0x7B,F("Board","onDetailPage")).Branch(0x39,"pop-return")
   .Int(1).Int(1).Ref(0x6F,M(0x060002BA)).Branch(0x38,"return")
   .Label("pop-return").O(0x26).Branch(0x38,"return")
   .Label("card").Arg(0).Ref(0x28,M(0x060001CA)).Branch(0x38,"return")
   .Label("skill").Arg(0).Ref(0x7B,F("MouseManager","plantSkillCrosshairs")).Ref(0x28,0x0A00000B).Branch(0x39,"detail-reset")
   .Arg(0).Ref(0x7B,F("MouseManager","skillOnHandOwnerPlant")).Ref(0x7B,F("Plant","attackRangeUIController")).Ref(0x6F,M(0x060005BE))
   .Arg(0).Ref(0x7B,F("MouseManager","plantSkillCrosshairs")).Ref(0x28,0x0A000025)
   .Arg(0).Ref(0x7B,F("MouseManager","board")).Ref(0x6F,M(0x060002B8))
   .Label("detail-reset").Arg(0).Ref(0x7B,F("MouseManager","board")).O(0x25).Ref(0x7B,F("Board","onDetailPage")).Branch(0x39,"pop-reset")
   .Int(1).Int(1).Ref(0x6F,M(0x060002BA)).Branch(0x38,"reset")
   .Label("pop-reset").O(0x26).Branch(0x38,"reset")
   .Label("shovel").Arg(0).Ref(0x7B,F("MouseManager","propOnHand")).O(0x14).Ref(0x28,0x0A00000C).Branch(0x39,"reset")
   .Arg(0).Ref(0x7B,F("MouseManager","propOnHand")).Ref(0x6F,M(0x06000401)).Branch(0x38,"reset")
   .Label("glove").Arg(0).Ref(0x7B,F("MouseManager","propOnHand")).Ref(0x28,0x0A00000B).Branch(0x39,"drop-glove")
   .Arg(0).Ref(0x7B,F("MouseManager","propOnHand")).Ref(0x6F,M(0x06000401))
   .Label("drop-glove").Arg(0).Ref(0x28,M(0x060001C9))
   .Label("reset").Arg(0).Ref(0x28,M(0x060001E1))
   .Label("return").O(0x2A);
  Set(0x060001C8,il,3);
  m=M(0x060002CE);R(m.Body.Variables[3].VariableType.FullName=="System.Boolean"&&m.Body.Variables[9].VariableType.FullName=="UnityEngine.AudioClip"&&m.Body.Variables[10].VariableType.FullName=="UnityEngine.Vector3"&&m.Body.Variables[13].VariableType.FullName=="System.Single","Pinned typed pause capture locals");
  R(StringToken(m,"GamePause")==0x70009EE2,"Exact existing native literal");
  il=new IL().Arg(0).Ref(0x7B,F("Board","isFailed")).Branch(0x39,"pause-gates")
   .Ref(0x28,M(0x06000169)).Ref(0x6F,M(0x0600016E))
   .Label("pause-gates").Arg(0).Ref(0x7B,F("Board","pauseTemp")).Branch(0x3A,"return")
   .Arg(0).Ref(0x7B,F("Board","menuOpen")).Branch(0x3A,"return")
   .Arg(0).Ref(0x7B,F("Board","gamePause")).S(3)
   .Arg(0).Int(1).Ref(0x7D,F("Board","pauseTemp"))
   .L(3).Branch(0x3A,"continue")
   .Ref(0x7E,F("ResourceManager","boardClips")).Int(11).Ref(0x6F,0x0A000084).S(9)
   .Ref(0x28,0x0A000004).S(6).L(6).Ref(0x6F,0x0A000001).S(7).L(7).Ref(0x6F,0x0A000037).S(10)
   .Ref(0x28,M(0x06000138)).S(13)
   .L(9).L(10).L(13).Ref(0x28,M(0x0600013D)).O(0x26)
   .Arg(0).Ref(0x7B,F("Board","pause")).Int(1).Ref(0x6F,0x0A000007)
   .O(0x72).I(StringToken(m,"GamePause")).Ref(0x28,0x0A000010)
   .Arg(0).Int(1).Ref(0x7D,F("Board","gamePause"))
   .Float(0f).Ref(0x28,0x0A0000F2).Branch(0x38,"return")
   .Label("continue").Arg(0).Ref(0x28,M(0x060002BD))
   .Label("return").O(0x2A);
  Set(0x060002CE,il,3);
  foreach(var(token,(code,max))in targets){
   m=M(token);R(m.Body.ExceptionHandlers.Count==0,"No EH target");
   if(code.Length>m.Body.CodeSize){var relocated=AppendBody(after,m,code,max);after=relocated.Image;report.Add(new{token=$"0x{token:X8}",method=m.FullName,old_reserved_code_size=m.Body.CodeSize,new_code_size=code.Length,old_max=m.Body.MaxStackSize,new_max=max,local_sig=m.Body.LocalVarToken.ToInt32(),code_sha256=H(code),relocation=relocated.Evidence});continue;}
   var sec=pe.PEHeaders.SectionHeaders.Single(x=>m.RVA>=x.VirtualAddress&&m.RVA<x.VirtualAddress+x.SizeOfRawData);int h=sec.PointerToRawData+m.RVA-sec.VirtualAddress;
   R((before[h]&3)==3&&(BitConverter.ToUInt16(before,h)>>12)==3,"Pinned fat header");Array.Clear(after,h+12,m.Body.CodeSize);code.CopyTo(after,h+12);BitConverter.GetBytes(code.Length).CopyTo(after,h+4);BitConverter.GetBytes((ushort)max).CopyTo(after,h+2);
   report.Add(new{token=$"0x{token:X8}",method=m.FullName,header=h,code_start=h+12,reserved_code_size=m.Body.CodeSize,new_code_size=code.Length,old_max=m.Body.MaxStackSize,new_max=max,local_sig=m.Body.LocalVarToken.ToInt32(),code_sha256=H(code)});
  }
  R(targets.Count==2,"Pinned scope 2");File.WriteAllBytes(args[1],after);File.WriteAllText(args[2],JsonSerializer.Serialize(new{status="UNTESTED_CANDIDATE",input_sha256=H(before),candidate_sha256=H(after),metadata_scope="Existing table rows except target MethodDef RVA remain unchanged by construction; no new rows or heap edits",methods=report,scope="Two complete original-native-backed callers; DropDownAll dispatches once and retains distinct Unity predicates, callback field rereads, static Destroy(null), captured board/Fold receiver, and ordinary-tail Reset; Space_down continues after BoardResume, captures pause state before pauseTemp write, captures clip/position before volume, uses exact three-argument audio and ordered partial writes; legal proven BoardManager getter; no new metadata rows, heap edits or local signature changes; complete oversize DropDownAll appended with target-only MethodDef RVA update; no tests executed; unqualified parent chain pending"},new JsonSerializerOptions{WriteIndented=true}));Console.WriteLine("CANDIDATE "+H(after));
 }
}
