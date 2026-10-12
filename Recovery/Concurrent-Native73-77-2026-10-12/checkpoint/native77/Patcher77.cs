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
static class RepairNative77 {
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
 static void CopyTo(this byte[] b,List<byte> list,int p){for(int j=0;j<b.Length;j++)list[p+j]=b[j];}
 static int Main(string[] args){try{Run(args);return 0;}catch(Exception e){Console.Error.WriteLine(e);return 3;}}
 static void Run(string[] args){
  R(args.Length==3&&!File.Exists(args[1])&&!File.Exists(args[2]),"Fresh outputs required");
  byte[] before=File.ReadAllBytes(args[0]);R(H(before)=="9816bf53598fdbc2a63f95fadb6b2b86873561181d4ac3a7acc44ede36d25c1c","Pinned untested Native76 parent");
  using var asm=Mono.Cecil.AssemblyDefinition.ReadAssembly(args[0]);var module=asm.MainModule;using var fs=File.OpenRead(args[0]);using var pe=new PEReader(fs);
  byte[] after=before.ToArray();
  MD M(int token)=>(MD)module.LookupToken(token);
  FR F(string owner,string name)=>module.GetTypes().Single(t=>t.FullName==owner).Fields.Single(f=>f.Name==name);
  MR C(MD method,string name)=>method.Body.Instructions.Select(i=>i.Operand).OfType<MR>().First(r=>r.Name==name);
  int localFirst=pe.GetMetadataReader().GetTableRowCount(TableIndex.StandAloneSig)+1;
  var targets=new Dictionary<int,(byte[] code,int max)>();var report=new List<object>();void Set(int token,IL il,int max){targets.Add(token,(il.Done(),max));}
  int StringToken(MD method,string text){var i=method.Body.Instructions.Single(i=>i.OpCode==OpCodes.Ldstr&&(string)i.Operand==text);var sec=pe.PEHeaders.SectionHeaders.Single(s=>method.RVA>=s.VirtualAddress&&method.RVA<s.VirtualAddress+s.SizeOfRawData);int header=sec.PointerToRawData+method.RVA-sec.VirtualAddress;R((before[header]&3)==3&&(BitConverter.ToUInt16(before,header)>>12)==3,"Pinned fat literal body");return BitConverter.ToInt32(before,header+12+i.Offset+1);}
  var m=M(0x060003D9);
  R(m.Body.Variables[3].VariableType.FullName=="System.Single"&&m.Body.Variables[4].VariableType.FullName=="System.Single"&&m.Body.Variables[9].VariableType.FullName=="System.Single","Existing native scalar scratch");
  var vx=module.GetMemberReferences().OfType<FR>().Single(r=>r.FullName=="System.Single UnityEngine.Vector3::x");
  var il=new IL().Arg(0).Ref(0x7B,F("Projectile","livingTime")).Float(1.5f).Branch(0x31,"formula")
   .Arg(0).Ref(0x7C,F("Projectile","speed")).Float(1500f).Ref(0x7D,vx)
   .Arg(0).Float(0f).Ref(0x7D,F("Projectile","zSpeed"))
   .Arg(0).Ref(0x28,C(m,"Update_Tracking"))
   .Arg(0).Int(0).Ref(0x7D,F("Projectile","movementTracks")).O(0x2A)
   .Label("formula").Double(10d).Ref(0x28,C(m,"Log")).O(0x6B).S(3)
   .O(0x72).I(StringToken(m,"a=")).A(3).Ref(0x28,C(m,"ToString")).Ref(0x28,C(m,"Concat"))
   .Ref(0x28,m.Body.Instructions.Select(i=>i.Operand).OfType<MR>().First(r=>r.Name=="Log"&&r.DeclaringType.FullName=="UnityEngine.Debug"))
   .Float(95f).Arg(0).Ref(0x7B,F("Projectile","fZ")).O(0x59,0x6B).S(4)
   .O(0x72).I(StringToken(m,"d=")).A(4).Ref(0x28,C(m,"ToString")).Ref(0x28,C(m,"Concat"))
   .Ref(0x28,m.Body.Instructions.Select(i=>i.Operand).OfType<MR>().First(r=>r.Name=="Log"&&r.DeclaringType.FullName=="UnityEngine.Debug"))
   .Arg(0).Ref(0x7B,F("Projectile","livingTime")).Float(6f).O(0x5A,0x6B).L(3).O(0x5A,0x6B).L(3).O(0x58,0x6B).S(9)
   .L(4).Float(6f).O(0x5A,0x6B).S(10)
   .Arg(0).L(10).L(9).O(0x5B,0x6B).Ref(0x7D,F("Projectile","zSpeed"))
   .Arg(0).Ref(0x28,C(m,"Update_Tracking")).O(0x2A);
  Set(0x060003D9,il,3);
  m=M(0x06000491);
  var getter=M(0x060001E5);var gi=getter.Body.Instructions.ToArray();
  R(getter.IsPublic&&getter.IsStatic&&getter.Parameters.Count==0&&gi.Length==3&&gi[0].OpCode.FlowControl==FlowControl.Branch&&gi[0].Operand==gi[1]&&gi[1].OpCode==OpCodes.Ldsfld&&gi[1].Operand is FR gf&&gf.FullName==F("ParticlesManager","<instance>k__BackingField").FullName&&gi[2].OpCode==OpCodes.Ret,"Exact unchanged pure particle singleton getter");
  R(m.Body.Variables[7].VariableType.FullName=="UnityEngine.Vector3"&&m.Body.Variables[8].VariableType.FullName=="UnityEngine.GameObject","Existing copied position and captured particle");
  il=new IL().Arg(0).Ref(0x7B,F("Zombie","board")).Ref(0x7B,F("Board","boardConfig")).Arg(1).Arg(2).Ref(0x6F,C(m,"GetPlantingPosition")).S(7)
   .Ref(0x28,getter).Int(13).Ref(0x6F,C(m,"CreatNewParticle")).S(8)
   .L(8).Ref(0x28,C(m,"op_Implicit")).Branch(0x2C,"return")
   .L(8).Ref(0x6F,C(m,"get_transform")).L(7).Ref(0x6F,C(m,"set_position"))
   .L(8).Ref(0x6F,C(m,"GetComponent")).Arg(0).Ref(0x7D,F("ZombieAnimationEvent","zombie"))
   .Label("return").O(0x2A);
  Set(0x06000491,il,3);
  foreach(var(token,(code,max))in targets){
   m=M(token);R(m.Body.ExceptionHandlers.Count==0,"EH relocation unsupported");var sec=pe.PEHeaders.SectionHeaders.Single(x=>m.RVA>=x.VirtualAddress&&m.RVA<x.VirtualAddress+x.SizeOfRawData);int h=sec.PointerToRawData+m.RVA-sec.VirtualAddress,hs=(BitConverter.ToUInt16(before,h)>>12)*4;
   R((before[h]&3)==3&&hs==12,"Pinned fat body");R(code.Length<=m.Body.CodeSize,"Body exceeds reserved slot "+m.FullName);
   Array.Clear(after,h+hs,m.Body.CodeSize);code.CopyTo(after,h+hs);BitConverter.GetBytes(code.Length).CopyTo(after,h+4);BitConverter.GetBytes((ushort)max).CopyTo(after,h+2);
   report.Add(new{token=$"0x{token:X8}",method=m.FullName,header=h,code_start=h+hs,reserved_code_size=m.Body.CodeSize,new_code_size=code.Length,old_max=m.Body.MaxStackSize,new_max=max,local_sig=m.Body.LocalVarToken.ToInt32(),code_sha256=H(code)});
  }
  R(report.Count==2,"Pinned scope 2");File.WriteAllBytes(args[1],after);File.WriteAllText(args[2],JsonSerializer.Serialize(new{status="UNTESTED_CANDIDATE",input_sha256=H(before),candidate_sha256=H(after),metadata_unchanged_by_construction=true,methods=report,scope="Two native-backed bodies; ordered 1.5 threshold including NaN reset, x-only speed write, real log and string formatting helpers, separate float32 numerator/denominator with field rereads after callbacks, captured boxing particle positioned before event assignment; all metadata and local signatures unchanged; pure public singleton getter guarded; unqualified parent chain pending"},new JsonSerializerOptions{WriteIndented=true}));Console.WriteLine("CANDIDATE "+H(after));
 }
}
