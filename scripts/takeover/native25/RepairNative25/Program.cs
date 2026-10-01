using Mono.Cecil;
using Mono.Cecil.Cil;
using System.Buffers.Binary;
using System.Reflection.PortableExecutable;
using System.Security.Cryptography;
using System.Text.Json;
internal static class RepairNative25 {
 static void Require(bool ok,string message){if(!ok)throw new InvalidOperationException(message);}
 static string Hash(byte[] b)=>Convert.ToHexString(SHA256.HashData(b)).ToLowerInvariant();
 static int Main(string[] a){try{Write(a);return 0;}catch(Exception e){Console.Error.WriteLine("REJECT: "+e.Message);return 3;}}
 static void Write(string[] a){
  Require(a.Length==3,"usage: locked-native24.dll new-output.dll report.json");var input=Path.GetFullPath(a[0]);var output=Path.GetFullPath(a[1]);Require(input!=output&&!File.Exists(output),"Output must be new and distinct");var before=File.ReadAllBytes(input);Require(Hash(before)=="8300c14ebac3b45cc00397c69cd72d4517366587acbe081b6e8b5a98f8fdedf9","Native24 input SHA mismatch");
  using var asm=AssemblyDefinition.ReadAssembly(input);var m=asm.MainModule.Types.Single(t=>t.FullName=="Device").Methods.Single(m=>m.MetadataToken.ToUInt32()==0x06000338);
  Require(m.FullName=="System.Boolean Device::TestPlacing(System.Int32,System.Int32)"&&m.HasThis&&m.Body.CodeSize==117&&m.Body.ExceptionHandlers.Count==0,"Pinned method mismatch");
  var f=(FieldReference)m.Body.Instructions.Single(i=>i.Offset==1).Operand;Require(f.FullName=="Board Device::board","Board field mismatch");
  var get=(MethodReference)m.Body.Instructions.Single(i=>i.Offset==45).Operand;Require(get.FullName=="Grid Board::GetGrid(System.Int32,System.Int32)"&&get.HasThis,"GetGrid signature mismatch");
  var can=(MethodReference)m.Body.Instructions.Single(i=>i.Offset==84).Operand;Require(can.FullName=="System.Boolean Grid::CanPlacing(Device)"&&can.HasThis,"CanPlacing signature mismatch");
  var ctor=(MethodReference)m.Body.Instructions.Single(i=>i.Offset==103).Operand;Require(ctor.FullName=="System.Void System.NullReferenceException::.ctor()"&&ctor.HasThis,"NRE signature mismatch");
  using var fs=File.OpenRead(input);using var pe=new PEReader(fs);var section=pe.PEHeaders.SectionHeaders.Single(s=>m.RVA>=s.VirtualAddress&&m.RVA<s.VirtualAddress+s.SizeOfRawData);var header=section.PointerToRawData+m.RVA-section.VirtualAddress;var headerSize=(BinaryPrimitives.ReadUInt16LittleEndian(before.AsSpan(header,2))>>12)*4;Require((before[header]&3)==3&&headerSize==12,"Pinned fat header mismatch");var codeStart=header+headerSize;Require(BinaryPrimitives.ReadUInt32LittleEndian(before.AsSpan(header+4,4))==117,"Code size mismatch");
  var body=new List<byte>();var labels=new Dictionary<string,int>();var branches=new List<(int Operand,string Target)>();
  void Op(byte b)=>body.Add(b);void Token(byte op,uint token){Op(op);body.AddRange(BitConverter.GetBytes(token));}void Br(string label){Op(0x2d);branches.Add((body.Count,label));Op(0);}void Label(string name)=>labels.Add(name,body.Count);
  // PC 18034BDF9..18034BE00: physical board null check; 18034BE28 raises NRE.
  Op(0x02);Token(0x7b,f.MetadataToken.ToUInt32());Op(0x25);Br("board");Op(0x26);Token(0x73,ctor.MetadataToken.ToUInt32());Op(0x7a);
  // PC 18034BE05 GetGrid; 18034BE0A..18034BE27 return false for a null Grid.
  Label("board");Op(0x03);Op(0x04);Token(0x28,get.MetadataToken.ToUInt32());Op(0x25);Br("grid");Op(0x26);Op(0x16);Op(0x2a);
  // PC 18034BE0F..18034BE1D: Grid receiver, Device argument, CanPlacing result.
  Label("grid");Op(0x02);Token(0x28,can.MetadataToken.ToUInt32());Op(0x2a);
  foreach(var b in branches){var delta=labels[b.Target]-(b.Operand+1);Require(delta>=sbyte.MinValue&&delta<=sbyte.MaxValue,"Short branch overflow");body[b.Operand]=unchecked((byte)(sbyte)delta);}
  var liveBytes=body.Count;Require(liveBytes==36,"Body specification drift");while(body.Count<117)Op(0);var after=(byte[])before.Clone();body.ToArray().CopyTo(after,codeStart);BinaryPrimitives.WriteUInt32LittleEndian(after.AsSpan(header+4,4),(uint)liveBytes);BinaryPrimitives.WriteUInt32LittleEndian(after.AsSpan(header+8,4),0);
  var changed=Enumerable.Range(0,before.Length).Where(i=>before[i]!=after[i]).ToArray();Require(changed.All(i=>i>=codeStart&&i<codeStart+117||i>=header+4&&i<header+12),"Bytes outside target reserved slot/CodeSize/LocalVarSigTok");
  var temp=output+".tmp";Require(!File.Exists(temp),"Temporary output exists");File.WriteAllBytes(temp,after);
  using(var reopened=AssemblyDefinition.ReadAssembly(temp)){
   var r=reopened.MainModule.Types.Single(t=>t.FullName=="Device").Methods.Single(m=>m.MetadataToken.ToUInt32()==0x06000338);Require(r.Body.Variables.Count==0&&r.Body.CodeSize==36,"Reopened locals/code size mismatch");var ins=r.Body.Instructions;var set=ins.ToHashSet();foreach(var i in ins){if(i.Operand is Instruction target)Require(set.Contains(target)&&target.Offset<liveBytes,"Invalid branch target");}
   Require(ins.Count(i=>i.OpCode==OpCodes.Nop)==0&&ins.Count==18,"Reopened body shape mismatch");
  }
  File.Move(temp,output);var report=new {inputSha256=Hash(before),outputSha256=Hash(after),method="0x06000338",bodyStart=codeStart,headerStart=header,codeSize=liveBytes,reservedSlotBytes=117,unusedSlotZeroBytes=81,localSigToken=0,metadataUntouched=true,nonTargetBytesUntouched=true,changedBytes=changed,labels,fieldToken=$"0x{f.MetadataToken.ToUInt32():X8}",getGridToken=$"0x{get.MetadataToken.ToUInt32():X8}",canPlacingToken=$"0x{can.MetadataToken.ToUInt32():X8}",nreCtorToken=$"0x{ctor.MetadataToken.ToUInt32():X8}",scope="One native-backed MethodBody; no whole-game acceptance"};File.WriteAllText(a[2],JsonSerializer.Serialize(report,new JsonSerializerOptions{WriteIndented=true}));Console.WriteLine(JsonSerializer.Serialize(report));
 }
}
