using Mono.Cecil;
using Mono.Cecil.Cil;
using System.Reflection.PortableExecutable;
using System.Security.Cryptography;
using System.Text.Json;
using MD=Mono.Cecil.MethodDefinition;
using MR=Mono.Cecil.MethodReference;
using FR=Mono.Cecil.FieldReference;

static class RepairNative34 {
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
  R(H(before)=="4ce81321b2d4d7bab3918fb3e8eceb39b2f6124d033d48dddac80ec7b831abb0","Pinned Native33 input");
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
  m=M(0x060004C5);il=new IL().Arg(1).Int(0).Branch(0x2E,"adventure").Arg(1).Int(1).Branch(0x2E,"rescue").Arg(1).Int(2).Branch(0x2E,"hard").Int(0).O(0x2A);
  il.Label("adventure").Arg(2).Arg(0).Ref(0x7B,F(m,"adventureLevel")).O(0xFE,0x04,0x2A);
  il.Label("rescue").Arg(0).Ref(0x7B,F(m,"rescuePassNum")).Arg(2).O(0x94).Int(0).O(0xFE,0x02,0x2A);
  il.Label("hard").Arg(0).Ref(0x7B,F(m,"adventureHardPassed")).Arg(2).O(0x91,0x2A);Set(0x060004C5,il,2);
  m=M(0x060004C6);var boolean=(TypeReference)m.Body.Instructions.Single(i=>i.OpCode==OpCodes.Newarr).Operand;
  il=new IL().Arg(0).Ref(0x7B,F(m,"almanac_DeviceLock")).Branch(0x2D,"ready").Arg(0).Int(128).Ref(0x8D,boolean).Ref(0x7D,F(m,"almanac_DeviceLock")).Label("ready").Arg(0).Ref(0x7B,F(m,"almanac_DeviceLock")).Arg(1).O(0x91).Branch(0x2C,"write").Int(1).O(0x2A).Label("write").Arg(0).Ref(0x7B,F(m,"almanac_DeviceLock")).Arg(1).Int(1).O(0x9C).Int(0).O(0x2A);Set(0x060004C6,il,3);
  m=M(0x060004D1);R(m.Body.Variables[0].VariableType.FullName=="System.String[]"&&new[]{1,2}.All(i=>m.Body.Variables[i].VariableType.FullName=="System.Int32"),"Talent scratches");
  // Original PC target 0x180B7CC30 implements string INEQUALITY. The damaged
  // CIL names Equality; invert the existing public equality reference explicitly.
  il=new IL().Arg(0).Ref(0x7B,F(m,"talentNames")).S(0).Int(0).S(1).Int(0).S(2).Branch(0x2B,"cond").Label("loop").L(0).L(2).O(0x9A).Ref(0x7E,F(m,"Empty")).Ref(0x28,C(m,"op_Equality")).Branch(0x2D,"skip").L(1).Int(1).O(0x58).S(1).Label("skip").L(2).Int(1).O(0x58).S(2).Label("cond").L(2).L(0).O(0x8E,0x69).Branch(0x32,"loop").L(1).O(0x2A);Set(0x060004D1,il,2);
  m=M(0x060001BA);R(m.Body.Variables[12].VariableType.FullName=="System.Single","Wavelength scratch");
  il=new IL().Arg(1).Int(0).Branch(0x2E,"long").Arg(1).Int(1).Branch(0x2E,"short").Arg(1).Int(2).Branch(0x33,"unknown").Arg(0).Ref(0x7B,F(m,"finish")).Branch(0x2D,"unknown").Arg(0).Float(0.02f).Ref(0x7D,F(m,"nextWaveTime")).Label("unknown").O(0x2A);
  // The native jae skips only ordered >=. On NaN it falls through and writes.
  il.Label("short").Arg(0).Ref(0x7B,F(m,"shortWavelength")).Arg(0).Ref(0x7B,F(m,"testWavelength")).O(0x59).Arg(0).Ref(0x7B,F(m,"nextWaveTime")).Branch(0x2F,"ret");
  il.Arg(0).Ref(0x7B,F(m,"shortWavelength")).Arg(0).Ref(0x7B,F(m,"testWavelength")).O(0x59).S(12).Arg(0).Arg(0).Ref(0x7B,F(m,"testWavelength")).Ref(0x7D,F(m,"nextTestTime")).Arg(0).L(12).Ref(0x7D,F(m,"nextWaveTime")).O(0x2A);
  il.Label("long").Arg(0).Arg(0).Ref(0x7B,F(m,"longWavelength")).Ref(0x7D,F(m,"nextWaveTime")).Arg(0).Arg(0).Ref(0x7B,F(m,"testWavelength")).Ref(0x7D,F(m,"nextTestTime"));
  il.Arg(0).Ref(0x7B,F(m,"theWave")).Int(10).O(0x5D).Int(9).Branch(0x33,"ret").Arg(0).Arg(0).Ref(0x7B,F(m,"longWavelength")).Arg(0).Ref(0x7B,F(m,"longWavelength")).O(0x58).Ref(0x7D,F(m,"nextWaveTime")).Label("ret").O(0x2A);Set(0x060001BA,il,3);
  // Apply all 4 target method bodies
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

  R(targets.Count==4,$"Pinned scope 4, got {targets.Count}");
  File.WriteAllBytes(args[1],after);
  File.WriteAllText(args[2],JsonSerializer.Serialize(new{
      input_sha256=H(before),
      candidate_sha256=H(after),
      methods=report,
      scope="4 native-pinned state methods; existing metadata rows/heaps and method RVAs preserved; no new metadata sections"
  },new JsonSerializerOptions{WriteIndented=true}));
  Console.WriteLine("CANDIDATE "+H(after));
 }
}
