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
static class RepairNative82 {
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
  byte[] before=File.ReadAllBytes(args[0]);R(H(before)=="5b143bae30c9747fa7beab7d92c580b8c556523857fd35b0e4bb1dab0164ddae","Pinned untested Native81 parent");
  using var asm=Mono.Cecil.AssemblyDefinition.ReadAssembly(args[0]);var module=asm.MainModule;using var fs=File.OpenRead(args[0]);using var pe=new PEReader(fs);
  byte[] after=before.ToArray();
  MD M(int token)=>(MD)module.LookupToken(token);
  FR F(string owner,string name)=>module.GetTypes().Single(t=>t.FullName==owner).Fields.Single(f=>f.Name==name);
  MR C(MD method,string name)=>method.Body.Instructions.Select(i=>i.Operand).OfType<MR>().First(r=>r.Name==name);
  int localFirst=pe.GetMetadataReader().GetTableRowCount(TableIndex.StandAloneSig)+1;
  var targets=new Dictionary<int,(byte[] code,int max)>();var report=new List<object>();void Set(int token,IL il,int max){targets.Add(token,(il.Done(),max));}
  int StringToken(MD method,string text){var i=method.Body.Instructions.Single(i=>i.OpCode==OpCodes.Ldstr&&(string)i.Operand==text);var sec=pe.PEHeaders.SectionHeaders.Single(s=>method.RVA>=s.VirtualAddress&&method.RVA<s.VirtualAddress+s.SizeOfRawData);int header=sec.PointerToRawData+method.RVA-sec.VirtualAddress;R((before[header]&3)==3&&(BitConverter.ToUInt16(before,header)>>12)==3,"Pinned fat literal body");return BitConverter.ToInt32(before,header+12+i.Offset+1);}
  var m=M(0x0600087D);R(m.IsStatic&&m.Parameters.Count==1&&m.Body.Variables[7].VariableType.FullName=="UnityEngine.GameObject","Pinned Swf singleton static signature and created object local");
  var il=new IL().Ref(0x7E,F("FTRuntime.SwfManager","_instance")).Ref(0x28,C(m,"op_Implicit")).Branch(0x2D,"return")
   .Ref(0x28,C(m,"FindObjectOfType")).Ref(0x80,F("FTRuntime.SwfManager","_instance"))
   .Arg(0).O(0xD2).Branch(0x2C,"return")
   .Ref(0x7E,F("FTRuntime.SwfManager","_instance")).Ref(0x28,C(m,"op_Implicit")).Branch(0x2D,"return")
   .O(0x72).I(StringToken(m,"[SwfManager]")).Ref(0x73,C(m,".ctor")).S(7)
   .L(7).O(0x25).Branch(0x2D,"created-live").O(0x26,0x14,0x7A)
   .Label("created-live").Ref(0x6F,C(m,"AddComponent")).Ref(0x80,F("FTRuntime.SwfManager","_instance"))
   .Label("return").Ref(0x7E,F("FTRuntime.SwfManager","_instance")).O(0x2A);
  Set(0x0600087D,il,2);
  m=M(0x06000734);var textType=module.GetTypeReferences().Single(t=>t.FullName=="TMPro.TextMeshProUGUI");
  var getType=module.GetMemberReferences().OfType<MR>().Single(r=>r.FullName=="System.Type System.Object::GetType()");
  il=new IL().Arg(0).Arg(0).Ref(0x6F,C(m,"get_gameObject")).Ref(0x6F,C(m,"GetComponent")).Ref(0x7D,F("TMPro.TMP_TextEventHandler","m_TextComponent"))
   .Arg(0).Ref(0x7B,F("TMPro.TMP_TextEventHandler","m_TextComponent")).Ref(0x6F,getType)
   .Ref(0xD0,textType).Ref(0x28,C(m,"GetTypeFromHandle")).O(0xFE,1).Branch(0x2D,"ui-text")
   .Arg(0).Ref(0x28,C(m,"get_main")).Ref(0x7D,F("TMPro.TMP_TextEventHandler","m_Camera")).O(0x2A)
   .Label("ui-text").Arg(0).Arg(0).Ref(0x6F,C(m,"get_gameObject")).Ref(0x6F,C(m,"GetComponentInParent")).Ref(0x7D,F("TMPro.TMP_TextEventHandler","m_Canvas"))
   .Arg(0).Ref(0x7B,F("TMPro.TMP_TextEventHandler","m_Canvas")).O(0x14).Ref(0x28,C(m,"op_Inequality")).Branch(0x2C,"return")
   .Arg(0).Ref(0x7B,F("TMPro.TMP_TextEventHandler","m_Canvas")).Ref(0x6F,C(m,"get_renderMode")).Branch(0x2C,"overlay")
   .Arg(0).Arg(0).Ref(0x7B,F("TMPro.TMP_TextEventHandler","m_Canvas")).Ref(0x6F,C(m,"get_worldCamera")).Ref(0x7D,F("TMPro.TMP_TextEventHandler","m_Camera")).O(0x2A)
   .Label("overlay").Arg(0).O(0x14).Ref(0x7D,F("TMPro.TMP_TextEventHandler","m_Camera"))
   .Label("return").O(0x2A);
  Set(0x06000734,il,2);
  foreach(var(token,(code,max))in targets){
   m=M(token);R(m.Body.ExceptionHandlers.Count==0,"EH relocation unsupported");var sec=pe.PEHeaders.SectionHeaders.Single(x=>m.RVA>=x.VirtualAddress&&m.RVA<x.VirtualAddress+x.SizeOfRawData);int h=sec.PointerToRawData+m.RVA-sec.VirtualAddress,hs=(BitConverter.ToUInt16(before,h)>>12)*4;
   R((before[h]&3)==3&&hs==12,"Pinned fat body");R(code.Length<=m.Body.CodeSize,"Body exceeds reserved slot "+m.FullName);
   Array.Clear(after,h+hs,m.Body.CodeSize);code.CopyTo(after,h+hs);BitConverter.GetBytes(code.Length).CopyTo(after,h+4);BitConverter.GetBytes((ushort)max).CopyTo(after,h+2);
   report.Add(new{token=$"0x{token:X8}",method=m.FullName,header=h,code_start=h+hs,reserved_code_size=m.Body.CodeSize,new_code_size=code.Length,old_max=m.Body.MaxStackSize,new_max=max,local_sig=m.Body.LocalVarToken.ToInt32(),code_sha256=H(code)});
  }
  R(report.Count==2,"Pinned scope 2");File.WriteAllBytes(args[1],after);File.WriteAllText(args[2],JsonSerializer.Serialize(new{status="UNTESTED_CANDIDATE",input_sha256=H(before),candidate_sha256=H(after),metadata_unchanged_by_construction=true,methods=report,scope="Two original-native-backed bodies; Swf singleton has two independent Unity truth tests, Find even when creation disabled, fresh field at return, constructor before raw-null AddComponent guard and result assignments after callbacks; TMP Awake uses source-proven exact Type reference equality, two independent gameObject reads, canvas field rereads after Unity comparison/renderMode, raw receiver faults and prior camera preservation; all metadata/local signatures unchanged; no tests executed; unqualified parent chain pending"},new JsonSerializerOptions{WriteIndented=true}));Console.WriteLine("CANDIDATE "+H(after));
 }
}
