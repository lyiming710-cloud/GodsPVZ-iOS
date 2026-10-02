using Mono.Cecil;
using Mono.Cecil.Cil;
using System.Reflection.PortableExecutable;
using System.Security.Cryptography;
using System.Text.Json;
using MD=Mono.Cecil.MethodDefinition;
using MR=Mono.Cecil.MethodReference;
using FR=Mono.Cecil.FieldReference;

static class RepairNative31 {
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
  R(H(before)=="6e4f81c860236b57261905a24eb7ba344b15a332eded64cc628d2c482ffcbb55","Pinned corrected Native30 input");
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

  MD m; IL il;
  // Native signed division and remainder; subtraction deliberately wraps int32.
  foreach(int token in new[]{0x060001A3,0x060001A4}){
    il=new IL().Arg(0).Branch(0x2D,"nonzero").Int(-1).O(0x2A).Label("nonzero").Arg(0).Int(1).O(0x59).Int(10).O(token==0x060001A3?(byte)0x5B:(byte)0x5D).O(0x2A);Set(token,il,2);
  }
  m=M(0x06000375);il=new IL();
  foreach(var pair in new[]{(4,15),(5,1),(7,24)}) il.Arg(0).Ref(0x7B,F(m,"ID")).Int(pair.Item1).Branch(0x2E,"id"+pair.Item1);
  il.Int(-1).O(0x2A);foreach(var pair in new[]{(4,15),(5,1),(7,24)})il.Label("id"+pair.Item1).Int(pair.Item2).O(0x2A);Set(0x06000375,il,2);
  // Tables decoded from original branch/jump-table targets and PC float constants.
  foreach(var spec in new (int token,(int key,float value)[] values)[]{
    (0x0600026C,new[]{(0,8f),(8,16f),(9,12f),(10,24f)}),
    (0x060004B6,new[]{(1,370f),(2,1100f),(3,2580f),(4,1680f),(5,1680f),(6,1980f)}),
    (0x060004B7,new[]{(1,20f),(2,35f),(3,15f),(4,15f),(5,35f),(6,15f)}),
    (0x060004B8,new[]{(1,110f),(2,20f),(3,400f),(4,120f)}),
    (0x060004B9,new[]{(1,1100f),(2,240f),(3,2200f),(4,1440f)}),
    (0x060004BA,new[]{(1,30f),(2,5f),(3,40f),(4,30f)})}){
    il=new IL();foreach(var(key,value)in spec.values)il.Arg(0).Int(key).Branch(0x2E,"key"+key);
    il.Float(0f).O(0x2A);foreach(var(key,value)in spec.values)il.Label("key"+key).Float(value).O(0x2A);Set(spec.token,il,2);
  }
  // Preserve ordered abs comparison: unordered (NaN) follows the second numerator.
  m=M(0x060003DF);R(m.Body.Variables[0].VariableType.FullName=="System.Single"&&m.Body.Variables[1].VariableType.FullName=="System.Single","Float scratch locals");
  il=new IL();
  for(int arg=1;arg<=2;arg++)il.Arg(arg).Float(0f).Branch(0x32,"negative"+arg).Arg(arg).Branch(0x2B,"abs"+arg).Label("negative"+arg).Arg(arg).O(0x65).Label("abs"+arg).S(arg-1);
  il.L(1).L(0).Branch(0x30,"first").Arg(2).Arg(1).Arg(2).O(0x58,0x5B,0x2A).Label("first").Arg(1).Arg(1).Arg(2).O(0x58,0x5B,0x2A);Set(0x060003DF,il,3);
  m=M(0x0600048A);var plant=module.Types.Single(t=>t.FullName=="Plant");
  il=new IL().Arg(1).Ref(0x7B,plant.Fields.Single(f=>f.Name=="ID")).Int(4).Branch(0x33,"yes").Arg(1).Ref(0x7B,plant.Fields.Single(f=>f.Name=="state")).Int(2).O(0xFE,0x01).Int(0).O(0xFE,0x01,0x2A).Label("yes").Int(1).O(0x2A);Set(0x0600048A,il,2);
  il=new IL();
  for(int channel=0;channel<4;channel++){
    il.Arg(channel+2).Arg(channel<2?0:1);
    if(channel%2==0)il.Int(16).O(0x64);
    il.O(0x68,0x6B).Float(0.001953125f).O(0x5A,0x56); // signed short -> float * exact 1/512 -> stind.r4
  }
  il.O(0x2A);Set(0x060008F3,il,3);
  // Apply all 12 target method bodies
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

  R(targets.Count==12,$"Pinned scope 12, got {targets.Count}");
  File.WriteAllBytes(args[1],after);
  File.WriteAllText(args[2],JsonSerializer.Serialize(new{
      input_sha256=H(before),
      candidate_sha256=H(after),
      methods=report,
      scope="12 native-pinned leaf methods; existing metadata rows/heaps and method RVAs preserved; no new metadata sections"
  },new JsonSerializerOptions{WriteIndented=true}));
  Console.WriteLine("CANDIDATE "+H(after));
 }
}
