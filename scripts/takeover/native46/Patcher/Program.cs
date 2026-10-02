using Mono.Cecil;
using Mono.Cecil.Cil;
using System.Reflection.PortableExecutable;
using System.Security.Cryptography;
using System.Text.Json;
using MD=Mono.Cecil.MethodDefinition;
using MR=Mono.Cecil.MethodReference;
using FR=Mono.Cecil.FieldReference;

static class RepairNative46 {
 static void R(bool x,string m){if(!x)throw new InvalidOperationException(m);}
 static string H(byte[] b)=>Convert.ToHexString(SHA256.HashData(b)).ToLowerInvariant();
 class IL {
  public List<byte> B=new();Dictionary<string,int> labels=new();List<(int p,string l)> branches=new();
  public IL O(params byte[] x){B.AddRange(x);return this;}public IL I(int x){B.AddRange(BitConverter.GetBytes(x));return this;}
  public IL Ref(byte op,IMetadataTokenProvider r)=>O(op).I(r.MetadataToken.ToInt32());public IL Ref(byte op,int t)=>O(op).I(t);
  public IL Int(int x)=>x>=-1&&x<=8?O((byte)(x+0x16)):O(0x20).I(x);public IL Float(float x)=>O(0x22).I(BitConverter.SingleToInt32Bits(x));
  public IL L(int i)=>i<4?O((byte)(6+i)):O(0x11,(byte)i);public IL S(int i)=>i<4?O((byte)(10+i)):O(0x13,(byte)i);public IL A(int i)=>O(0x12,(byte)i);
  public IL Arg(int i)=>i<4?O((byte)(2+i)):O(0x0E,(byte)i);
  public IL Label(string s){labels.Add(s,B.Count);return this;}public IL Branch(byte op,string s){O(op);branches.Add((B.Count,s));O(0);return this;}
  public byte[] Done(){foreach(var(p,l)in branches){int d=labels[l]-p-1;R(d>=-128&&d<=127,"Short branch overflow");B[p]=unchecked((byte)(sbyte)d);}return B.ToArray();}
 }

 static void CopyTo(this byte[] b,List<byte> list,int p){for(int j=0;j<b.Length;j++)list[p+j]=b[j];}
 static int Main(string[] args){try{Run(args);return 0;}catch(Exception e){Console.Error.WriteLine(e);return 3;}}
 static void Run(string[] args){
  R(args.Length==3,"input new-output report");
  R(!File.Exists(args[1])&&!File.Exists(args[2]),"Fresh outputs required");
  byte[] before=File.ReadAllBytes(args[0]);
  R(H(before)=="ad67277f2ed009402a36b0728eb864ef08a521b07b9523b714dc9a437fdf845a","Pinned Native45 input");
  byte[] after=(byte[])before.Clone();
  using var asm=Mono.Cecil.AssemblyDefinition.ReadAssembly(args[0]);
  var module=asm.MainModule;
  using var fs=File.OpenRead(args[0]);
  using var pe=new PEReader(fs);
  var secHeaders=pe.PEHeaders.SectionHeaders;

  MD M(int t)=>(MD)module.LookupToken(t);
  FR F(MD m,string name)=>m.Body.Instructions.Select(i=>i.Operand).OfType<FR>().FirstOrDefault(f=>f.Name==name)
      ?? m.DeclaringType.Fields.FirstOrDefault(f=>f.Name==name)
      ?? module.GetMemberReferences().OfType<FR>().First(f=>f.Name==name);
  MR C(MD m,string name)=>m.Body.Instructions.Select(i=>i.Operand).OfType<MR>().First(c=>c.Name==name);
  MR CallRef(string fullName)=>module.GetMemberReferences().OfType<MR>().First(c=>c.FullName==fullName);

  var targets=new Dictionary<int,(byte[] code,int max)>();
  var report=new List<object>();
  void Set(int token,IL il,int max=4){targets.Add(token,(il.Done(),max));}

  MD m;IL il;
  int StringToken(MD method,string value){var str=method.Body.Instructions.Single(i=>i.OpCode==OpCodes.Ldstr&&(string)i.Operand==value);var oldCode=System.Reflection.Metadata.PEReaderExtensions.GetMethodBody(pe,method.RVA).GetILBytes()!;return BitConverter.ToInt32(oldCode,str.Offset+1);}
  FR Field(int token,string expected){var f=(FR)module.LookupToken(token);R(f.FullName==expected,"Pinned field "+expected);return f;}
  m=M(0x060006E8);var coins=Field(0x0400063B,"System.Int32 Save::coins");var info=Field(0x040008EE,"SuppliesInfo Warehouse_SuppliesInfoPage::suppliesInfo");var fmt=C(m,"ToString");R(fmt.FullName=="System.String System.Int32::ToString(System.String)","Public formatted int32 overload");il=new IL().Arg(0).Ref(0x28,C(m,"ClearSuppliesItemList")).Arg(0).Ref(0x7B,F(m,"suppliesInfoPage")).S(3).L(3).O(0x14).Ref(0x7D,F(m,"supplies")).L(3).O(0x14).Ref(0x7D,info).L(3).Ref(0x28,C(m,"SetInfo")).Arg(0).Arg(1).Ref(0x7D,F(m,"playerSave")).Arg(0).Ref(0x7B,F(m,"text_money")).Arg(1).Ref(0x7C,coins).Ref(0x72,StringToken(m,"N0")).Ref(0x28,fmt).Int(1).Ref(0x6F,C(m,"SetText")).Arg(0).Ref(0x28,C(m,"CreatSuppliesItemList")).O(0x2A);Set(0x060006E8,il,3);
  m=M(0x060000A4);fmt=C(m,"ToString");R(fmt.FullName=="System.String System.Int32::ToString(System.String)","Same public formatted int32 overload");il=new IL().Arg(1).Branch(0x2C,"end").Arg(0).Arg(1).Ref(0x7D,F(m,"playerSave")).Arg(0).Ref(0x7B,F(m,"challenge")).Branch(0x2D,"format").Arg(0).Arg(1).Ref(0x7B,F(m,"adventureLevel")).Ref(0x7D,F(m,"level")).Label("format").Arg(0).Ref(0x7B,F(m,"text_Level")).Arg(0).Ref(0x7C,F(m,"level")).Ref(0x72,StringToken(m,"当前关卡：0")).Ref(0x28,fmt).Int(1).Ref(0x6F,C(m,"SetText")).Label("end").O(0x2A);Set(0x060000A4,il,3);
  // Apply all 2 target method bodies
  foreach(var(token,(code,max)) in targets){
      m=M(token);
      R(m.Body.ExceptionHandlers.Count==0,"EH relocation unsupported");
      var sec=secHeaders.Single(s=>m.RVA>=s.VirtualAddress&&m.RVA<s.VirtualAddress+s.SizeOfRawData);
      int h=sec.PointerToRawData+m.RVA-sec.VirtualAddress;
      int hs=(BitConverter.ToUInt16(before,h)>>12)*4;
      R((before[h]&3)==3&&hs==12,"Pinned fat body");
      R(code.Length<=m.Body.CodeSize,$"Body exceeds reserved slot {m.FullName}: {code.Length} > {m.Body.CodeSize}");
      Array.Clear(after,h+hs,m.Body.CodeSize);
      code.CopyTo(after,h+hs);
      BitConverter.GetBytes(code.Length).CopyTo(after,h+4);
      BitConverter.GetBytes((ushort)max).CopyTo(after,h+2);
      report.Add(new{
          token=$"0x{token:X8}",
          method=m.FullName,
          header=h,
          code_start=h+hs,
          reserved_code_size=m.Body.CodeSize,
          new_code_size=code.Length,
          old_max=m.Body.MaxStackSize,
          new_max=max,
          local_sig=m.Body.LocalVarToken.ToInt32(),
          code_sha256=H(code)
      });
  }

  R(targets.Count==2,$"Pinned scope 2, got {targets.Count}");
  File.WriteAllBytes(args[1],after);
  File.WriteAllText(args[2],JsonSerializer.Serialize(new{
      input_sha256=H(before),
      candidate_sha256=H(after),
      methods=report,
      scope="2 native-pinned formatted numeric UI callers with existing public references; existing metadata rows/heaps and method RVAs preserved; no new metadata sections"
  },new JsonSerializerOptions{WriteIndented=true}));
  Console.WriteLine("CANDIDATE "+H(after));
 }
}
