from pathlib import Path
import base64
p=Path('audit/native74-codex-2026-10-12')
s=Path('audit/native71-codex-2026-10-12/Patcher71.cs').read_text().replace('RepairNative71','RepairNative74')
s=s.replace('775d43f4413940b5c2e1cb440fff0933e6c58388d7794992baff72fcbe8c8973','47df498caf8fd0d02a494c995654c972399e98d2d0614fa25faaf7036b125f43').replace('Pinned untested Native70 parent','Pinned untested Native73 parent')
a=s.index('  var selectable=');b=s.index('  ulong valid=',a)
s=s[:a]+'''  Member("Add",TS("System.Collections.Generic.List`1<SettlementUIController>")*8+4,new byte[]{0x20,1,1,0x13,0});
  R(memberRows.Count==6,"Exactly one public typed settlement-list Add");
  R(md.GetTableRowCount(TableIndex.TypeSpec)<8192&&md.GetTableRowCount(TableIndex.TypeRef)<8192&&md.GetTableRowCount(TableIndex.TypeDef)<8192&&md.GetTableRowCount(TableIndex.MethodDef)<8192,"No parent index widening");
  var additions=new Dictionary<TableIndex,byte[]>{{TableIndex.MemberRef,memberRows.ToArray()}};
'''+s[b:]
a=s.index('  var m=M(0x06000532)');b=s.index('  foreach(var(token,(code,max))in targets)',a)
s=s[:a]+'''  var m=M(0x0600027C);
  R(m.Body.Variables[12].VariableType.FullName=="Zombie","Pinned captured zombie scratch");
  var il=new IL().Arg(0).Arg(1).Ref(0x28,C(m,"CreateNewZombie")).S(12)
   .L(12).Ref(0x28,C(m,"op_Implicit")).Branch(0x2C,"return-null")
   .L(12).Arg(0).Ref(0x7B,F("ZombieManager","board")).Ref(0x7B,F("Board","boardConfig"))
   .Arg(2).Arg(3).Ref(0x6F,C(m,"GetZombiePosition")).Float(0f).Int(0).Ref(0x6F,C(m,"TeleportTo"))
   .L(12).Arg(2).Ref(0x7D,F("Zombie","gridX")).L(12).Arg(3).Ref(0x7D,F("Zombie","gridY"))
   .L(12).Float(BitConverter.Int32BitsToSingle(0x3C23D70A)).Ref(0x7D,F("Zombie","waitingTime"))
   .L(12).Ref(0x6F,C(m,"CreateStartPrePath"))
   .L(12).Int(-1).Ref(0x7D,F("Zombie","wave")).L(12).Int(0).Ref(0x7D,F("Zombie","enemyPoint"))
   .L(12).O(0x2A).Label("return-null").O(0x14,0x2A);
  Set(0x0600027C,il,4);
  m=M(0x06000208);
  R(m.Body.Variables[4].VariableType.FullName=="Board","Pinned captured board scratch");
  il=new IL().Arg(0).Ref(0x7B,F("ProjectManager","board")).Ref(0x28,C(m,"op_Implicit")).Branch(0x2C,"false")
   .Arg(0).Ref(0x7B,F("ProjectManager","board")).S(4)
   .L(4).Ref(0x7B,F("Board","isFinished")).Branch(0x2D,"false")
   .L(4).Ref(0x6F,C(m,"TestWinTargetZombie")).Branch(0x2C,"false")
   .Arg(0).Ref(0x7B,F("ProjectManager","board")).Ref(0x6F,C(m,"GameFinished"))
   .Arg(0).Int(2).Int(4).Arg(2).Ref(0x28,C(m,"CreateProject"))
   .Arg(1).Ref(0x6F,C(m,"SetEndPosition")).Int(1).O(0x2A)
   .Label("false").Int(0).O(0x2A);
  Set(0x06000208,il,4);
  m=M(0x06000173);
  R(m.Body.Variables[1].VariableType.FullName=="SettlementUIController","Pinned settlement capture");
  il=new IL().Arg(0).Ref(0x7B,F("BoardManager","settlementUIControllerPrefab")).Ref(0x28,C(m,"Instantiate")).S(1)
   .Arg(0).Ref(0x7B,F("BoardManager","settlementUIControllers")).L(1).Ref(0x6F,first)
   .L(1).Arg(1).Arg(0).Ref(0x7B,F("BoardManager","board")).Ref(0x7B,F("Board","reward")).Ref(0x6F,C(m,"Settlement"))
   .L(1).Arg(0).Ref(0x7B,F("BoardManager","challengeType"))
   .Arg(0).Ref(0x7B,F("BoardManager","level")).Arg(0).Ref(0x7B,F("BoardManager","playerSave")).Ref(0x6F,C(m,"PopupSet"))
   .L(1).Ref(0x6F,C(m,"Awaken")).L(1).O(0x2A);
  Set(0x06000173,il,4);
  m=M(0x060005CE);
  R(m.Body.Variables[10].VariableType.FullName=="UnityEngine.Vector3","Pinned copied camera position");
  var vx=module.GetMemberReferences().OfType<FR>().Single(r=>r.FullName=="System.Single UnityEngine.Vector3::x");
  il=new IL().Arg(0).Ref(0x7B,F("BoardPreview","prepareUIController")).Ref(0x28,C(m,"op_Implicit")).Branch(0x2C,"camera")
   .Arg(0).Ref(0x7B,F("BoardPreview","prepareUIController")).Ref(0x6F,C(m,"get_gameObject")).Int(1).Ref(0x6F,C(m,"SetActive"))
   .Label("camera").Ref(0x28,C(m,"get_main")).Ref(0x6F,C(m,"get_transform")).Ref(0x6F,C(m,"get_position")).S(10)
   .A(10).Float(0f).Ref(0x7D,vx)
   .Ref(0x28,C(m,"get_main")).Ref(0x6F,C(m,"get_transform")).L(10).Ref(0x6F,C(m,"set_position"))
   .Ref(0x28,C(m,"get_main")).Float(540f).Ref(0x6F,C(m,"set_orthographicSize"))
   .Arg(0).Ref(0x6F,C(m,"get_gameObject")).Int(0).Ref(0x6F,C(m,"SetActive")).O(0x2A);
  Set(0x060005CE,il,2);
'''+s[b:]
s=s.replace('R(report.Count==2,"Pinned scope 2")','R(report.Count==4,"Pinned scope 4")')
a=s.index('scope="Two native-backed button-audio bodies;');b=s.index('"},new JsonSerializerOptions',a)
s=s[:a]+'scope="Four native-backed bodies; captured zombie placement with post-path wave resets, board rereads during award creation, captured settlement controller and typed list Add, repeated camera getters with x-only reset and original 540 size; exactly one public collection binding, original local signatures unchanged; unqualified parent chain pending'+s[b:]
(p/'Patcher74.cs').write_text(s)
t=Path('audit/native73-codex-2026-10-12/generate_remote.py').read_text();a=t.index("(s/'Program.cs').write_bytes");b=t.index('\n',a)
t=t[:a]+"(s/'Program.cs').write_bytes(base64.b64decode("+repr(base64.b64encode(s.encode()).decode())+"))"+t[b:]
t=t.replace('godspvz-native73-codex','godspvz-native74-codex').replace('scripts/takeover/native73/Patcher','scripts/takeover/native74/Patcher').replace('/tmp/godspvz-native72-restored-2026-10-12/snapshot/candidates/native72/candidate1.dll','/tmp/godspvz-native73-codex-2026-10-12/candidate1.dll')
(p/'generate_remote.py').write_text(t)
