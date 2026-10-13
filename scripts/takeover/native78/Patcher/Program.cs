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
static class RepairNative78 {
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
  byte[] before=File.ReadAllBytes(args[0]);R(H(before)=="290767e0d4e513612c56b3d06cff3edf0894e58808a6338d6c6b87019a2a3f83","Pinned untested Native77 parent");
  using var asm=Mono.Cecil.AssemblyDefinition.ReadAssembly(args[0]);var module=asm.MainModule;using var fs=File.OpenRead(args[0]);using var pe=new PEReader(fs);
  byte[] after=before.ToArray();
  MD M(int token)=>(MD)module.LookupToken(token);
  FR F(string owner,string name)=>module.GetTypes().Single(t=>t.FullName==owner).Fields.Single(f=>f.Name==name);
  MR C(MD method,string name)=>method.Body.Instructions.Select(i=>i.Operand).OfType<MR>().First(r=>r.Name==name);
  int localFirst=pe.GetMetadataReader().GetTableRowCount(TableIndex.StandAloneSig)+1;
  var targets=new Dictionary<int,(byte[] code,int max)>();var report=new List<object>();void Set(int token,IL il,int max){targets.Add(token,(il.Done(),max));}
  int StringToken(MD method,string text){var i=method.Body.Instructions.Single(i=>i.OpCode==OpCodes.Ldstr&&(string)i.Operand==text);var sec=pe.PEHeaders.SectionHeaders.Single(s=>method.RVA>=s.VirtualAddress&&method.RVA<s.VirtualAddress+s.SizeOfRawData);int header=sec.PointerToRawData+method.RVA-sec.VirtualAddress;R((before[header]&3)==3&&(BitConverter.ToUInt16(before,header)>>12)==3,"Pinned fat literal body");return BitConverter.ToInt32(before,header+12+i.Offset+1);}
  var m=M(0x0600061D);
  R(m.Body.Variables[0].VariableType.FullName=="System.Int32","Existing explicitly initialized enum scratch");
  var i4=module.GetTypeReferences().Single(t=>t.FullName=="System.Int32");
  var il=new IL().Int(0).S(0)
   .Ref(0xD0,module.GetType("GridState")).Ref(0x28,C(m,"GetTypeFromHandle"))
   .Arg(1).Ref(0x8C,i4).Ref(0x28,C(m,"IsDefined")).Branch(0x2C,"invalid")
   .Arg(1).S(0).Branch(0x2B,"check-system")
   .Label("invalid").O(0x72).I(StringToken(m,"此序号对应格子类型不存在")).Ref(0x28,C(m,"Log"))
   .Label("check-system").Arg(0).Ref(0x7B,F("GridConfig_A","system")).O(0x14).Ref(0x28,C(m,"op_Inequality")).Branch(0x2C,"return")
   .Arg(0).Ref(0x7B,F("GridConfig_A","system")).O(0x25).Branch(0x2D,"system-live").O(0x26,0x14,0x7A)
   .Label("system-live").L(0).Arg(0).Ref(0x7B,F("GridConfig_A","gridX")).Arg(0).Ref(0x7B,F("GridConfig_A","gridY")).Ref(0x28,C(m,"SetGridState"))
   .Label("return").O(0x2A);
  Set(0x0600061D,il,4);
  m=M(0x06000483);
  var zero=module.GetMemberReferences().OfType<MR>().Single(r=>r.FullName=="UnityEngine.Vector3 UnityEngine.Vector3::get_zero()");
  il=new IL().Arg(0).Ref(0x7B,F("Zombie","targetPlant")).Arg(1).Ref(0x28,C(m,"op_Inequality")).Branch(0x2C,"tail")
   .Arg(0).Ref(0x7B,F("Zombie","targetPlant")).O(0x14).Ref(0x28,C(m,"op_Inequality")).Branch(0x2C,"new-target")
   .Arg(0).Ref(0x7B,F("Zombie","targetPlant")).O(0x25).Branch(0x2D,"old-live").O(0x26,0x14,0x7A)
   .Label("old-live").Arg(0).Int(0).Ref(0x28,C(m,"BlockZombie"))
   .Label("new-target").Arg(1).O(0x14).Ref(0x28,C(m,"op_Inequality")).Branch(0x2C,"tail")
   .Arg(1).O(0x25).Branch(0x2D,"new-live").O(0x26,0x14,0x7A)
   .Label("new-live").Arg(0).Int(1).Ref(0x28,C(m,"BlockZombie"))
   .Label("tail").Arg(0).Ref(0x7B,F("Zombie","animator")).O(0x25).Branch(0x2D,"animator-live").O(0x26,0x14,0x7A)
   .Label("animator-live").O(0x72).I(StringToken(m,"isAttacking")).Int(1).Ref(0x28,C(m,"SetBool"))
   .Arg(0).Ref(0x28,C(m,"ResetAttackSpeed"))
   .Arg(0).Ref(0x28,zero).Ref(0x7D,F("Zombie","rSpeed"))
   .Arg(0).Int(1).Ref(0x7D,F("Zombie","isEat")).O(0x2A);
  Set(0x06000483,il,3);
  foreach(var(token,(code,max))in targets){
   m=M(token);R(m.Body.ExceptionHandlers.Count==0,"EH relocation unsupported");var sec=pe.PEHeaders.SectionHeaders.Single(x=>m.RVA>=x.VirtualAddress&&m.RVA<x.VirtualAddress+x.SizeOfRawData);int h=sec.PointerToRawData+m.RVA-sec.VirtualAddress,hs=(BitConverter.ToUInt16(before,h)>>12)*4;
   R((before[h]&3)==3&&hs==12,"Pinned fat body");R(code.Length<=m.Body.CodeSize,"Body exceeds reserved slot "+m.FullName);
   Array.Clear(after,h+hs,m.Body.CodeSize);code.CopyTo(after,h+hs);BitConverter.GetBytes(code.Length).CopyTo(after,h+4);BitConverter.GetBytes((ushort)max).CopyTo(after,h+2);
   report.Add(new{token=$"0x{token:X8}",method=m.FullName,header=h,code_start=h+hs,reserved_code_size=m.Body.CodeSize,new_code_size=code.Length,old_max=m.Body.MaxStackSize,new_max=max,local_sig=m.Body.LocalVarToken.ToInt32(),code_sha256=H(code)});
  }
  R(report.Count==2,"Pinned scope 2");File.WriteAllBytes(args[1],after);File.WriteAllText(args[2],JsonSerializer.Serialize(new{status="UNTESTED_CANDIDATE",input_sha256=H(before),candidate_sha256=H(after),metadata_unchanged_by_construction=true,methods=report,scope="Two original-native-backed bodies; GridState typeof and boxed Int32 independently mapped from original usage cells; one GetTypeFromHandle; invalid state logs then continues with zero; system and targetPlant rereads after Unity comparisons/callbacks; native method does not assign targetPlant; raw-null branches throw NullReferenceException; public Vector3.get_zero exact getter IL evidence; all metadata and local signatures unchanged; no tests executed; unqualified parent chain pending"},new JsonSerializerOptions{WriteIndented=true}));Console.WriteLine("CANDIDATE "+H(after));
 }
}
