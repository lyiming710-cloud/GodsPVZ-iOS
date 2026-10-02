using Mono.Cecil;
using Mono.Cecil.Cil;
using System.Reflection.PortableExecutable;
using System.Security.Cryptography;
using System.Text.Json;
using MD=Mono.Cecil.MethodDefinition;
using MR=Mono.Cecil.MethodReference;
using FR=Mono.Cecil.FieldReference;

static class RepairNative39 {
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
  R(H(before)=="be3bd138a4db23ac7d4f2e9046101002623ac94b1599aad0870d7fde2718066c","Pinned Native38 input");
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
  m=M(0x06000091);var log=m.Body.Instructions.Select(i=>i.Operand).OfType<MR>().Single(c=>c.Name=="Log");il=new IL().Arg(1).Ref(0x8C,m.Body.Instructions.Where(i=>i.OpCode.Code==Mono.Cecil.Cil.Code.Box).Select(i=>i.Operand).OfType<Mono.Cecil.TypeReference>().First(t=>t.FullName=="System.Int32")).Ref(0x28,log).Arg(0).Ref(0x7B,F(m,"boardConfig")).Ref(0x7B,F(m,"map")).Arg(1).Ref(0x7D,F(m,"senarioState")).O(0x2A);Set(0x06000091,il,2);
  m=M(0x060008A5);var format=m.Body.Instructions.Select(i=>i.Operand).OfType<MR>().Single(c=>c.Name=="Format");var str=m.Body.Instructions.Single(i=>i.OpCode.Code==Mono.Cecil.Cil.Code.Ldstr);R((string)str.Operand=="{0}.{1}.{2}","Pinned original format literal");var originalCode=System.Reflection.Metadata.PEReaderExtensions.GetMethodBody(pe,m.RVA).GetILBytes()!;int userString=BitConverter.ToInt32(originalCode,str.Offset+1);il=new IL().Ref(0x72,userString).Int(1).Ref(0x8C,m.Body.Instructions.Where(i=>i.OpCode.Code==Mono.Cecil.Cil.Code.Box).Select(i=>i.Operand).OfType<Mono.Cecil.TypeReference>().First(t=>t.FullName=="System.Int32")).Int(3).Ref(0x8C,m.Body.Instructions.Where(i=>i.OpCode.Code==Mono.Cecil.Cil.Code.Box).Select(i=>i.Operand).OfType<Mono.Cecil.TypeReference>().First(t=>t.FullName=="System.Int32")).Int(15).Ref(0x8C,m.Body.Instructions.Where(i=>i.OpCode.Code==Mono.Cecil.Cil.Code.Box).Select(i=>i.Operand).OfType<Mono.Cecil.TypeReference>().First(t=>t.FullName=="System.Int32")).Ref(0x28,format).O(0x2A);Set(0x060008A5,il,4);
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
      scope="2 native-pinned boxing callers; existing metadata rows/heaps and method RVAs preserved; no new metadata sections"
  },new JsonSerializerOptions{WriteIndented=true}));
  Console.WriteLine("CANDIDATE "+H(after));
 }
}
