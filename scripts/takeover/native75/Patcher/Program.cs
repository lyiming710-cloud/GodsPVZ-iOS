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
static class RepairNative75 {
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
  byte[] before=File.ReadAllBytes(args[0]);R(H(before)=="71c28b1cad9ff8e5dd456121c2b0747792c3281f66c3da701002e91d9e3b9f4c","Pinned untested Native74 parent");
  using var asm=Mono.Cecil.AssemblyDefinition.ReadAssembly(args[0]);var module=asm.MainModule;using var fs=File.OpenRead(args[0]);using var pe=new PEReader(fs);
  byte[] after=before.ToArray();
  MD M(int token)=>(MD)module.LookupToken(token);
  FR F(string owner,string name)=>module.GetTypes().Single(t=>t.FullName==owner).Fields.Single(f=>f.Name==name);
  MR C(MD method,string name)=>method.Body.Instructions.Select(i=>i.Operand).OfType<MR>().First(r=>r.Name==name);
  int localFirst=pe.GetMetadataReader().GetTableRowCount(TableIndex.StandAloneSig)+1;
  var targets=new Dictionary<int,(byte[] code,int max)>();var report=new List<object>();void Set(int token,IL il,int max){targets.Add(token,(il.Done(),max));}
  int StringToken(MD method,string text){var i=method.Body.Instructions.Single(i=>i.OpCode==OpCodes.Ldstr&&(string)i.Operand==text);var sec=pe.PEHeaders.SectionHeaders.Single(s=>method.RVA>=s.VirtualAddress&&method.RVA<s.VirtualAddress+s.SizeOfRawData);int header=sec.PointerToRawData+method.RVA-sec.VirtualAddress;R((before[header]&3)==3&&(BitConverter.ToUInt16(before,header)>>12)==3,"Pinned fat literal body");return BitConverter.ToInt32(before,header+12+i.Offset+1);}
  var vector=module.GetMemberReferences().OfType<MR>().Single(r=>r.FullName=="System.Void UnityEngine.Vector3::.ctor(System.Single,System.Single,System.Single)");
  var color=module.GetMemberReferences().OfType<MR>().Single(r=>r.FullName=="System.Void UnityEngine.Color::.ctor(System.Single,System.Single,System.Single,System.Single)");
  var gameTransform=module.GetMemberReferences().OfType<MR>().Single(r=>r.FullName=="UnityEngine.Transform UnityEngine.GameObject::get_transform()");
  var m=M(0x060002B5);
  var il=new IL().Arg(0).Ref(0x7B,F("Board","isFailed")).Branch(0x2D,"return")
   .Ref(0x7E,F("ResourceManager","boardClips")).Int(11).Ref(0x6F,C(m,"get_Item"))
   .Ref(0x28,C(m,"get_main")).Ref(0x6F,C(m,"get_transform")).Ref(0x6F,C(m,"get_position"))
   .Ref(0x28,C(m,"AudioVolume")).Ref(0x28,C(m,"CreateAudioAtPoint")).O(0x26)
   .Arg(0).Ref(0x7B,F("Board","pause")).Int(1).Ref(0x6F,C(m,"SetActive"))
   .O(0x72).I(StringToken(m,"GamePause")).Ref(0x28,C(m,"Log"))
   .Arg(0).Int(1).Ref(0x7D,F("Board","gamePause"))
   .Float(0f).Ref(0x28,C(m,"set_timeScale")).Label("return").O(0x2A);
  Set(0x060002B5,il,3);
  m=M(0x060002B7);
  il=new IL().Arg(0).Ref(0x7E,F("ResourceManager","prefab_FlagMeter")).Ref(0x28,C(m,"Instantiate")).Ref(0x7D,F("Board","flagMeter"))
   .Arg(0).Ref(0x7B,F("Board","flagMeter")).Arg(0).Ref(0x7B,F("Board","enemyManager")).Ref(0x6F,C(m,"Initialize"))
   .Arg(0).Ref(0x7B,F("Board","flagMeter")).Ref(0x6F,C(m,"get_transform"))
   .Arg(0).Ref(0x7B,F("Board","UI_flagMeterButtom")).Ref(0x6F,gameTransform).Int(0).Ref(0x6F,C(m,"SetParent"))
   .Arg(0).Ref(0x7B,F("Board","flagMeter")).Ref(0x6F,C(m,"get_transform"))
   .Float(-195f).Float(0f).Float(0f).Ref(0x73,vector).Ref(0x6F,C(m,"set_localPosition"))
   .Arg(0).Ref(0x7B,F("Board","flagMeter")).Ref(0x6F,C(m,"get_gameObject")).Int(0).Ref(0x6F,C(m,"SetActive")).O(0x2A);
  Set(0x060002B7,il,4);
  m=M(0x0600056F);
  R(m.Body.Variables[1].VariableType.FullName=="System.Single"&&m.Body.Variables[6].VariableType.FullName=="UnityEngine.Vector3"&&m.Body.Variables[11].VariableType.FullName=="System.Single","Existing scalar/vector scratch");
  var vy=module.GetMemberReferences().OfType<FR>().Single(r=>r.FullName=="System.Single UnityEngine.Vector3::y");var vt=module.GetTypeReferences().Single(t=>t.FullName=="UnityEngine.Vector3");
  il=new IL().Arg(0).Ref(0x7B,F("Almanac_PlantDetail","animtion_time")).S(1)
   .L(1).Float(0f).O(0xFE,0x02).Branch(0x39,"return")
   .Arg(0).L(1).Ref(0x28,C(m,"get_deltaTime")).O(0x59,0x6B,0x25).S(1).Ref(0x7D,F("Almanac_PlantDetail","animtion_time"))
   .Arg(0).Ref(0x7B,F("Almanac_PlantDetail","scrollViewContent")).Ref(0x6F,C(m,"GetComponent")).Ref(0x6F,C(m,"get_localPosition")).S(6)
   .L(1).Float(4f).O(0x5A,0x6B).S(1)
   .Arg(0).Ref(0x7B,F("Almanac_PlantDetail","animtion_contentStartY")).S(11)
   .L(1).Float(0f).Branch(0x32,"clamp-zero")
   .L(1).Float(1f).Branch(0x36,"lerp").Float(1f).S(1).Branch(0x2B,"lerp")
   .Label("clamp-zero").Float(0f).S(1)
   .Label("lerp").A(6).L(11).Float(0f).O(0x59,0x6B).L(1).O(0x5A,0x6B).Float(0f).O(0x58,0x6B).Ref(0x7D,vy)
   .Arg(0).Ref(0x7B,F("Almanac_PlantDetail","scrollViewContent")).Ref(0x6F,C(m,"GetComponent")).L(6).Ref(0x6F,C(m,"set_localPosition"))
   .L(6).Ref(0x8C,vt).Ref(0x28,C(m,"Log")).Label("return").O(0x2A);
  Set(0x0600056F,il,3);
  m=M(0x060006DB);
  il=new IL().Arg(0).Ref(0x7B,F("SkillButtonController","image_lock")).Arg(1).Branch(0x2D,"open")
   .Int(0).O(0x72).I(StringToken(m,"Lock")).Ref(0x28,C(m,"LoadSkillLogo")).Branch(0x2B,"loaded")
   .Label("open").Int(-1).O(0x72).I(StringToken(m,"Lock_open")).Ref(0x28,C(m,"LoadSkillLogo"))
   .Label("loaded").Ref(0x6F,C(m,"set_sprite"))
   .Arg(0).Ref(0x7B,F("SkillButtonController","image_lock")).Float(1f).Float(1f).Float(1f)
   .Arg(1).Branch(0x2D,"dim").Float(1f).Branch(0x2B,"color")
   .Label("dim").Float(BitConverter.Int32BitsToSingle(0x3F19999A))
   .Label("color").Ref(0x73,color).Ref(0x6F,C(m,"set_color"))
   .Arg(0).Ref(0x7B,F("SkillButtonController","image_select")).Ref(0x6F,C(m,"get_gameObject"))
   .Arg(1).Int(0).O(0xFE,0x01).Ref(0x6F,C(m,"SetActive")).O(0x2A);
  Set(0x060006DB,il,5);
  foreach(var(token,(code,max))in targets){
   m=M(token);R(m.Body.ExceptionHandlers.Count==0,"EH relocation unsupported");var sec=pe.PEHeaders.SectionHeaders.Single(x=>m.RVA>=x.VirtualAddress&&m.RVA<x.VirtualAddress+x.SizeOfRawData);int h=sec.PointerToRawData+m.RVA-sec.VirtualAddress,hs=(BitConverter.ToUInt16(before,h)>>12)*4;
   R((before[h]&3)==3&&hs==12,"Pinned fat body");R(code.Length<=m.Body.CodeSize,"Body exceeds reserved slot "+m.FullName);
   Array.Clear(after,h+hs,m.Body.CodeSize);code.CopyTo(after,h+hs);BitConverter.GetBytes(code.Length).CopyTo(after,h+4);BitConverter.GetBytes((ushort)max).CopyTo(after,h+2);
   report.Add(new{token=$"0x{token:X8}",method=m.FullName,header=h,code_start=h+hs,reserved_code_size=m.Body.CodeSize,new_code_size=code.Length,old_max=m.Body.MaxStackSize,new_max=max,local_sig=m.Body.LocalVarToken.ToInt32(),code_sha256=H(code)});
  }
  R(report.Count==4,"Pinned scope 4");File.WriteAllBytes(args[1],after);File.WriteAllText(args[2],JsonSerializer.Serialize(new{status="UNTESTED_CANDIDATE",input_sha256=H(before),candidate_sha256=H(after),metadata_unchanged_by_construction=true,methods=report,scope="Four native-backed bodies; ordered pause effects and captured audio, flag-meter field rereads and -195 local x, almanac captured pre-delta countdown with ordered NaN clamp and vector logging, captured lock image with original color alpha; all original metadata and local signatures retained; unqualified parent chain pending"},new JsonSerializerOptions{WriteIndented=true}));Console.WriteLine("CANDIDATE "+H(after));
 }
}
