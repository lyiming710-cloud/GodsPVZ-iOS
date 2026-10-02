using Mono.Cecil;
using Mono.Cecil.Cil;
using System.Reflection.PortableExecutable;
using System.Security.Cryptography;
using System.Text.Json;
using MD=Mono.Cecil.MethodDefinition;
using MR=Mono.Cecil.MethodReference;
using FR=Mono.Cecil.FieldReference;

static class RepairNative32 {
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
  R(H(before)=="66b1234b13fe812adca05cfc92bf01b11f44f6427c112ca2dc101ac713b85e0b","Pinned Native31 input");
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
  // Original table includes default zero, signed/unsigned out-of-range values.
  il=new IL();foreach(var pair in new[]{(1,60f),(2,120f),(3,40f),(4,200f),(5,320f),(6,150f)})il.Arg(0).Int(pair.Item1).Branch(0x2E,"armor"+pair.Item1);
  il.Float(0f).O(0x2A);foreach(var pair in new[]{(1,60f),(2,120f),(3,40f),(4,200f),(5,320f),(6,150f)})il.Label("armor"+pair.Item1).Float(pair.Item2).O(0x2A);Set(0x060004B5,il,2);
  // Multiplication/addition wrap int32 BEFORE signed division by three.
  m=M(0x060001AB);R(m.Body.Variables[1].VariableType.FullName=="System.Int32","Wave scratch type");
  il=new IL().Arg(1).Int(1).O(0x58).Arg(0).Ref(0x7B,F(m,"enemyPoint_Base")).O(0x5A).Arg(2).Arg(0).Ref(0x7B,F(m,"enemyPoint_Base")).O(0x5A).Int(3).O(0x5B,0x58).S(1);
  il.Arg(2).Int(10).O(0x5D).Int(9).Branch(0x33,"plain").L(1).Arg(0).Ref(0x7B,F(m,"enemyPoint_Base")).Int(3).O(0x5A,0x58,0x2A).Label("plain").L(1).O(0x2A);Set(0x060001AB,il,4);
  m=M(0x060003FC);R(m.Body.Variables[2].VariableType.FullName=="Map"&&m.Body.Variables[5].VariableType.FullName=="System.Single","Map scratch types");
  il=new IL().Arg(0).Ref(0x7B,F(m,"board")).Ref(0x7B,F(m,"map")).S(2);
  foreach(var dim in new[]{("fX",1160f,"Y"),("fY",670f,"end")}){
    il.L(2).Ref(0x7B,F(m,"cameraSize")).Float(540f).O(0x5B).Float(dim.Item2).O(0x5A).S(5);
    il.Arg(0).Ref(0x7B,F(m,dim.Item1)).L(5).Branch(0x30,dim.Item3+"yes").L(5).O(0x65).Arg(0).Ref(0x7B,F(m,dim.Item1)).Branch(0x30,dim.Item3+"yes").Branch(0x2B,dim.Item3).Label(dim.Item3+"yes").Int(1).O(0x2A).Label(dim.Item3);
  }
  il.Int(0).O(0x2A);Set(0x060003FC,il,2);
  m=M(0x06000470);R(m.Body.Variables[0].VariableType.FullName=="System.Single","Rect scratch type");
  il=new IL().Arg(1).Arg(3).O(0x59).S(0).L(0).Float(0f).Branch(0x32,"neg").L(0).Branch(0x2B,"abs").Label("neg").L(0).O(0x65).Label("abs").S(0).Arg(2).Arg(4).O(0x58).Float(0.5f).O(0x5A).L(0).O(0xFE,0x02,0x2A);Set(0x06000470,il,2);
  // Original uses ordered Mathf.Approximately and eight exact comparisons.
  m=M(0x060008A2);
  R(new[]{3,19,23}.All(i=>m.Body.Variables[i].VariableType.FullName=="System.Single"),"Settings float scratches");
  il=new IL();
  void FieldEqual(string f){il.Arg(0).Ref(0x7B,F(m,f)).O(0x0F,1).Ref(0x7B,F(m,f)).Branch(0x2E,"equal"+f).Int(0).O(0x2A).Label("equal"+f);}
  void Abs(int local,string label){il.L(local).Float(0f).Branch(0x32,"negative"+label).L(local).Branch(0x2B,"abs"+label).Label("negative"+label).L(local).O(0x65).Label("abs"+label).S(local);}
  FieldEqual("MaxAtlasSize");FieldEqual("AtlasPadding");
  il.O(0x0F,1).Ref(0x7B,F(m,"PixelsPerUnit")).Arg(0).Ref(0x7B,F(m,"PixelsPerUnit")).O(0x59).S(3);Abs(3,"diff");
  il.Arg(0).Ref(0x7B,F(m,"PixelsPerUnit")).S(19);Abs(19,"a");
  il.O(0x0F,1).Ref(0x7B,F(m,"PixelsPerUnit")).S(23);Abs(23,"b");
  il.L(19).L(23).Branch(0x30,"firstMax").L(23).Branch(0x2B,"max").Label("firstMax").L(19).Label("max").Float(0.000001f).O(0x5A).S(19);
  il.Ref(0x7E,F(m,"Epsilon")).Float(8f).O(0x5A).S(23);
  il.L(19).L(23).Branch(0x30,"relative").L(23).Branch(0x2B,"bound").Label("relative").L(19).Label("bound").L(3).Branch(0x30,"approximately").Int(0).O(0x2A).Label("approximately");
  foreach(string f in new[]{"BitmapTrimming","GenerateMipMaps","AtlasPowerOfTwo","AtlasForceSquare","AtlasTextureFilter","AtlasTextureFormat"})FieldEqual(f);
  il.Int(1).O(0x2A);Set(0x060008A2,il,2);
  // Apply all 5 target method bodies
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

  R(targets.Count==5,$"Pinned scope 5, got {targets.Count}");
  File.WriteAllBytes(args[1],after);
  File.WriteAllText(args[2],JsonSerializer.Serialize(new{
      input_sha256=H(before),
      candidate_sha256=H(after),
      methods=report,
      scope="5 native-pinned methods; existing metadata rows/heaps and method RVAs preserved; no new metadata sections"
  },new JsonSerializerOptions{WriteIndented=true}));
  Console.WriteLine("CANDIDATE "+H(after));
 }
}
