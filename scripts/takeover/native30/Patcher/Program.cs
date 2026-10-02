using Mono.Cecil;
using Mono.Cecil.Cil;
using System.Reflection.PortableExecutable;
using System.Security.Cryptography;
using System.Text.Json;
using MD=Mono.Cecil.MethodDefinition;
using MR=Mono.Cecil.MethodReference;
using FR=Mono.Cecil.FieldReference;

static class RepairNative30 {
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
  R(H(before)=="4572e7e2d121f58e84af65c78056a42b5e919b06275d8915024401e3b4256fe2","Pinned Native29 input");
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

  // 1. Call site Vector3 argument fixes (6 sites)
  var callSites=new (int token, int offset, int local, int arg)[]{
      (0x060000C1, 0x4E, 4, -1),
      (0x060001C7, 0x5B, 6, -1),
      (0x060002B4, 0x58, 5, -1),
      (0x06000480, 0x13, -1, 1),
      (0x0600001F, 0x6E, 5, -1),
      (0x06000060, 0x67, 5, -1)
  };
  foreach(var(token,offset,local,arg) in callSites){
      var sm=M(token);
      var si=sm.Body.Instructions.Single(i=>i.Offset==offset);
      R(si.OpCode==Mono.Cecil.Cil.OpCodes.Ldloca&&si.GetSize()==4,"Argument site shape");
      var sec=secHeaders.Single(s=>sm.RVA>=s.VirtualAddress&&sm.RVA<s.VirtualAddress+s.SizeOfRawData);
      int h=sec.PointerToRawData+sm.RVA-sec.VirtualAddress;
      int hs=(BitConverter.ToUInt16(before,h)>>12)*4;
      R(hs==12,"Fat site header");
      byte[] code=before[(h+hs)..(h+hs+sm.Body.CodeSize)];
      code[offset]=0xFE;
      code[offset+1]=(byte)(local>=0?0x0C:0x09);
      BitConverter.GetBytes((ushort)(local>=0?local:arg)).CopyTo(code,offset+2);
      foreach(var call in sm.Body.Instructions.Where(q=>q.OpCode==Mono.Cecil.Cil.OpCodes.Call && q.Operand is MR cc && (cc.Name=="get_transform" || cc.Name=="get_position" || cc.Name=="set_position" || cc.Name=="SetEndPosition"))){
          R(code[call.Offset]==0x28,"Native physical call guard site");
          code[call.Offset]=0x6F;
      }
      targets.Add(token,(code,sm.Body.MaxStackSize));
  }

  // 2. 0x060003C9: Project::Rotating()
  var m=M(0x060003C9);
  var il=new IL();
  il.Arg(0).Ref(0x7B,F(m,"projectType")).Branch(0x2D,"ret");
  il.Arg(0).Ref(0x6F,C(m,"get_transform"));
  il.Ref(0x28,CallRef("UnityEngine.Vector3 UnityEngine.Vector3::get_forward()"));
  il.Ref(0x28,CallRef("System.Single UnityEngine.Time::get_deltaTime()"));
  il.Float(-18.0f).O(0x5A); // mul
  il.Int(0); // Space.Self
  il.Ref(0x6F,C(m,"Rotate"));
  il.Label("ret").O(0x2A);
  Set(0x060003C9,il,5);

  // 3. 0x0600026A: SunManager::SunFall()
  m=M(0x0600026A);
  il=new IL();
  il.Arg(0).Ref(0x7B,F(m,"board")).Ref(0x7B,F(m,"projectManager"));
  il.Int(0).Int(0).Ref(0x28,CallRef("UnityEngine.Vector3 UnityEngine.Vector3::get_zero()"));
  il.Ref(0x6F,C(m,"CreateProject"));
  il.Ref(0x6F,C(m,"SetNatureSunInitialPostion"));
  il.Arg(0).O(0x25).Ref(0x7B,F(m,"sunFallCount")).Int(1).O(0x58).Ref(0x7D,F(m,"sunFallCount"));
  il.O(0x2A);
  Set(0x0600026A,il,5);

  // 4. 0x0600026B: SunManager::SunPointTextUpdate()
  m=M(0x0600026B);
  il=new IL();
  il.Arg(0).Ref(0x7B,F(m,"sunPointText"));
  il.Arg(0).Ref(0x7C,m.DeclaringType.Fields.Single(f=>f.Name=="sunPoint")); // ldflda
  il.Ref(0x28,C(m,"ToString"));
  il.Ref(0x6F,C(m,"set_text"));
  il.O(0x2A);
  Set(0x0600026B,il,3);

  // 5. 0x06000448: Zombie::GetElementPoint(ElementType)
  m=M(0x06000448);
  var elType=module.Types.Single(t=>t.Name=="Element");
  il=new IL();
  il.Arg(0).Ref(0x7B,F(m,"elementManager")).Arg(1).Ref(0x6F,C(m,"GetElement"));
  il.O(0x25).Branch(0x2D,"has").O(0x26).Float(0.0f).O(0x2A);
  il.Label("has").Ref(0x7B,elType.Fields.Single(f=>f.Name=="point")).O(0x2A);
  Set(0x06000448,il,2);

  // 6. 0x06000488: Zombie::TryShooting()
  m=M(0x06000488);
  il=new IL();
  var nreCtor=module.GetMemberReferences().OfType<MR>().First(c=>c.DeclaringType.FullName=="System.NullReferenceException"&&c.Name==".ctor");
  int strToken=BitConverter.ToInt32(before,secHeaders.Single(s=>m.RVA>=s.VirtualAddress&&m.RVA<s.VirtualAddress+s.SizeOfRawData).PointerToRawData+m.RVA-secHeaders.Single(s=>m.RVA>=s.VirtualAddress&&m.RVA<s.VirtualAddress+s.SizeOfRawData).VirtualAddress+((BitConverter.ToUInt16(before,secHeaders.Single(s=>m.RVA>=s.VirtualAddress&&m.RVA<s.VirtualAddress+s.SizeOfRawData).PointerToRawData+m.RVA-secHeaders.Single(s=>m.RVA>=s.VirtualAddress&&m.RVA<s.VirtualAddress+s.SizeOfRawData).VirtualAddress)>>12)*4)+m.Body.Instructions.Single(i=>i.OpCode==Mono.Cecil.Cil.OpCodes.Ldstr&&i.Operand as string=="ShootingTrigger").Offset+1);
  il.Arg(0).Ref(0x7B,F(m,"animator")).O(0x25).Branch(0x2D,"ok");
  il.Ref(0x73,nreCtor).O(0x7A);
  il.Label("ok");
  il.O(0x72).I(strToken);
  il.Ref(0x6F,C(m,"SetTrigger"));
  il.Int(1).O(0x2A);
  Set(0x06000488,il,3);

  // 7. 0x060006E9: WarehouseUIController::CheckSupplies(Supplies)
  m=M(0x060006E9);
  var pageType=module.Types.Single(t=>t.Name=="Warehouse_SuppliesInfoPage");
  var supType=module.Types.Single(t=>t.Name=="Supplies");
  il=new IL();
  il.Arg(0).Ref(0x7B,F(m,"suppliesInfoPage"));
  il.O(0x25).Arg(1).Ref(0x7D,pageType.Fields.Single(f=>f.Name=="supplies"));
  il.O(0x25).Arg(1).Branch(0x2C,"null");
  il.Arg(1).Ref(0x7B,supType.Fields.Single(f=>f.Name=="ID"));
  il.Ref(0x28,C(m,"GetSuppliesInfo"));
  il.Branch(0x2B,"set");
  il.Label("null").O(0x14); // ldnull
  il.Label("set");
  il.Ref(0x7D,pageType.Fields.Single(f=>f.Name=="suppliesInfo"));
  il.Ref(0x6F,C(m,"SetInfo"));
  il.O(0x2A);
  Set(0x060006E9,il,3);

  // 8. 0x06000675: Popup::PopupPopup(int)
  m=M(0x06000675);
  il=new IL();
  il.Ref(0x7E,F(m,"prefab_Popup")).Ref(0x28,C(m,"Instantiate"));
  il.O(0x25).Branch(0x2D,"ok");
  il.Ref(0x73,nreCtor).O(0x7A);
  il.Label("ok").O(0x25).Arg(0).Ref(0x7D,F(m,"P")).O(0x2A);
  Set(0x06000675,il,3);

  // 9. 0x06000673: Popup::PopupPopup(int, int)
  m=M(0x06000673);
  il=new IL();
  il.Ref(0x7E,F(m,"prefab_Popup")).Ref(0x28,C(m,"Instantiate"));
  il.O(0x25).Branch(0x2D,"ok");
  il.Ref(0x73,nreCtor).O(0x7A);
  il.Label("ok").O(0x25).Arg(1).Ref(0x7D,F(m,"info0_int"));
  il.O(0x25).Arg(0).Ref(0x7D,F(m,"P")).O(0x2A);
  Set(0x06000673,il,3);

  // 10. 0x06000704: Window_Q::PopupNewWindow(int, Transform, int)
  m=M(0x06000704);
  il=new IL();
  il.Arg(0).Arg(1).Ref(0x28,C(m,"PopupNewWindow"));
  il.O(0x25).Branch(0x2D,"ok");
  il.Ref(0x73,nreCtor).O(0x7A);
  il.Label("ok").O(0x25).Arg(2).Ref(0x7D,F(m,"info0_int")).O(0x2A);
  Set(0x06000704,il,3);

  // 11. 0x060008F2: SwfUtils::UnpackUV(uint, ref float, ref float)
  m=M(0x060008F2);
  il=new IL();
  il.Arg(1).Arg(0).O(0x1F,16).O(0x64).O(0x6B).Float(65535.0f).O(0x5B).O(0x56);
  il.Arg(2).Arg(0).Int(65535).O(0x5F).O(0x6B).Float(65535.0f).O(0x5B).O(0x56);
  il.O(0x2A);
  Set(0x060008F2,il,3);

  // 12. 0x06000401: Prop::DropDown()
  m=M(0x06000401);
  var propType=module.Types.Single(t=>t.Name=="Prop");
  il=new IL();
  il.Arg(0).Ref(0x7B,F(m,"prop")).Ref(0x6F,C(m,"get_transform"));
  il.Arg(0).Ref(0x7B,propType.Fields.Single(f=>f.Name=="originalPosition"));
  il.Ref(0x6F,C(m,"set_localPosition"));
  il.Arg(0).Int(1).Ref(0x7D,F(m,"propState"));
  il.Arg(0).Ref(0x7B,F(m,"propImagex"));
  il.O(0x14); // ldnull
  il.Ref(0x28,CallRef("System.Boolean UnityEngine.Object::op_Inequality(UnityEngine.Object,UnityEngine.Object)"));
  il.Branch(0x2C,"bank"); // brfalse.s bank
  il.Arg(0).Ref(0x7B,F(m,"propImagex"));
  il.Ref(0x28,C(m,"Destroy"));
  il.Label("bank");
  il.Arg(0).Ref(0x6F,C(m,"PropBankStateSet"));
  il.O(0x2A);
  Set(0x06000401,il,3);

  // Apply all 17 target method bodies
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

  R(targets.Count==17,$"Pinned scope 17, got {targets.Count}");
  File.WriteAllBytes(args[1],after);
  File.WriteAllText(args[2],JsonSerializer.Serialize(new{
      input_sha256=H(before),
      candidate_sha256=H(after),
      methods=report,
      scope="17 native-pinned methods; existing metadata rows/heaps and method RVAs preserved; no new metadata sections"
  },new JsonSerializerOptions{WriteIndented=true}));
  Console.WriteLine("CANDIDATE "+H(after));
 }
}
