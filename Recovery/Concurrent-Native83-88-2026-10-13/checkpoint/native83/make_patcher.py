from pathlib import Path
import base64,json
p=Path('audit/native83-codex-2026-10-13')
s=Path('audit/native79-codex-2026-10-13/Patcher79.cs').read_text().replace('RepairNative79','RepairNative83')
s=s.replace('de2f98d4e793b91fba17ba7eff3aeba3130282f5e477f0daae1aa358287868f7','48b9e89e941c041d6c11c4064ad05d9384a1a4dd51d5f8d3fad9192cbda5adbf').replace('Pinned untested Native78 parent','Pinned untested Native82 parent')
a=s.index('  var quaternion=');b=s.index('  ulong valid=',a)
s=s[:a]+'''  Member("Insert",TS("System.Collections.Generic.List`1<Path_enemyEditor>")*8+4,new byte[]{0x20,2,1,8,0x13,0});
  R(memberRows.Count==6&&localRows.Count==0,"Only one typed public Path_enemyEditor Insert binding");
  var additions=new Dictionary<TableIndex,byte[]>{{TableIndex.MemberRef,memberRows.ToArray()}};
'''+s[b:]
s=s.replace('clear=(1UL<<(int)TableIndex.MemberRef)|(1UL<<(int)TableIndex.StandAloneSig)','clear=1UL<<(int)TableIndex.MemberRef')
a=s.index('  int StringToken(');b=s.index('  foreach(var(token,(code,max))in targets)',a)
s=s[:a]+'''  var m=M(0x06000056);
  R(m.Body.Variables[1].VariableType.FullName=="Enemy"&&m.Body.Variables[2].VariableType.FullName=="Path_enemyEditor"&&m.Body.Variables[3].VariableType.FullName=="UnityEngine.Transform"&&m.Body.Variables[4].VariableType.FullName=="UnityEngine.Transform"&&m.Body.Variables[6].VariableType.FullName=="EnemyPath","Existing path editor captured locals");
  var insertPath=module.GetMemberReferences().OfType<MR>().Where(r=>r.FullName=="System.Void System.Collections.Generic.List`1<EnemyPath>::Insert(System.Int32,!0)").OrderBy(r=>r.MetadataToken.RID).First();
  var il=new IL().Arg(0).Ref(0x7B,F("PathDataEditor","enemy_BoardEditor")).Ref(0x7B,F("Enemy_boardEditor","preEnemy")).S(1)
   .L(1).Ref(0x7B,F("Enemy","gridX")).L(1).Ref(0x7B,F("Enemy","row")).Float(0f).O(0x14).Ref(0x73,M(0x060002A4)).S(6)
   .L(1).Ref(0x7B,F("Enemy","myPath")).O(0x25).Branch(0x2D,"path-list-live").O(0x26,0x14,0x7A)
   .Label("path-list-live").Arg(0).Ref(0x7B,F("PathDataEditor","pathIndex")).Int(1).O(0x58).L(6).Ref(0x6F,insertPath)
   .Arg(0).Ref(0x7B,F("PathDataEditor","path_EnemyEditor_prefab")).Ref(0x28,C(m,"Instantiate")).S(2)
   .L(2).Arg(0).Ref(0x7B,F("PathDataEditor","enemy_BoardEditor")).Ref(0x7D,F("Path_enemyEditor","enemyEditor"))
   .L(2).L(6).Ref(0x7D,F("Path_enemyEditor","enemyPath"))
   .L(2).Ref(0x28,M(0x06000060))
   .L(2).Ref(0x6F,C(m,"get_transform")).S(3)
   .Arg(0).Ref(0x7B,F("PathDataEditor","enemy_BoardEditor")).Ref(0x6F,C(m,"get_transform")).S(4)
   .L(3).L(4).Int(1).Ref(0x6F,C(m,"SetParent"))
   .Arg(0).Ref(0x7B,F("PathDataEditor","enemy_BoardEditor")).Ref(0x7B,F("Enemy_boardEditor","path_EnemyEditors")).O(0x25).Branch(0x2D,"editor-list-live").O(0x26,0x14,0x7A)
   .Label("editor-list-live").Arg(0).Ref(0x7B,F("PathDataEditor","pathIndex")).Int(1).O(0x58).L(2).Ref(0x6F,first)
   .Arg(0).Arg(0).Ref(0x7B,F("PathDataEditor","pathIndex")).Int(1).O(0x58).Ref(0x28,M(0x06000058)).O(0x2A);
  Set(0x06000056,il,4);
  m=M(0x06000119);
  R(m.IsConstructor&&m.DeclaringType.BaseType.FullName=="DeviceEvent"&&m.Body.Variables[5].VariableType.FullName=="Hide"&&m.Body.Variables[10].VariableType.FullName=="UnityEngine.AudioClip"&&m.Body.Variables[11].VariableType.FullName=="UnityEngine.Vector3"&&m.Body.Variables[8].VariableType.FullName=="System.Single","Actual HoleTeleport hierarchy and typed scratch");
  il=new IL().Arg(0).Ref(0x28,M(0x06000117))
   .Arg(0).Arg(1).Ref(0x7D,F("HoleTeleport","zombie"))
   .Arg(0).Arg(2).Ref(0x7D,F("HoleTeleport","gridX"))
   .Arg(0).Arg(3).Ref(0x7D,F("HoleTeleport","gridY"))
   .Int(1).Arg(1).Ref(0x73,M(0x060000FD)).S(5)
   .Arg(1).Ref(0x7B,F("Zombie","buffManager")).L(5).Ref(0x6F,M(0x06000102))
   .Ref(0x7E,F("ResourceManager","zombieClips")).Int(6).Ref(0x6F,C(m,"get_Item")).S(10)
   .Ref(0x28,C(m,"get_main")).Ref(0x6F,C(m,"get_transform")).Ref(0x6F,C(m,"get_position")).S(11)
   .Ref(0x28,C(m,"AudioVolume")).Float(1.2f).O(0x5A,0x6B).S(8)
   .L(10).L(11).L(8).Float(1.5f).Ref(0x28,C(m,"CreateAudioAtPoint")).O(0x26,0x2A);
  Set(0x06000119,il,4);
  m=M(0x060000E0);R(m.IsConstructor&&m.DeclaringType.BaseType.FullName=="System.Object","Actual AttackRange constructor hierarchy");
  MR ListCtor(string item)=>m.Body.Instructions.Select(i=>i.Operand).OfType<MR>().Single(r=>r.FullName=="System.Void System.Collections.Generic.List`1<"+item+">::.ctor()");
  var objectCtor=module.GetMemberReferences().OfType<MR>().Single(r=>r.FullName=="System.Void System.Object::.ctor()");
  il=new IL().Arg(0).Ref(0x73,ListCtor("UnityEngine.Rect")).Ref(0x7D,F("AttackRange","rects"))
   .Arg(0).Ref(0x73,ListCtor("System.Single")).Ref(0x7D,F("AttackRange","radius"))
   .Arg(0).Ref(0x73,ListCtor("UnityEngine.Vector3")).Ref(0x7D,F("AttackRange","centers"))
   .Arg(0).Ref(0x28,objectCtor).O(0x2A);
  Set(0x060000E0,il,2);
'''+s[b:]
s=s.replace('token==0x06000365?0x11000000+localFirst:oldLocal','oldLocal')
old_scope=s[s.index('scope="')+7:s.index('"},new JsonSerializerOptions',s.index('scope="'))]
s=s.replace(old_scope,'Three original-native-backed bodies including explicit transitive constructor repair; path insertion captures initial Enemy, uses captured pre-allocation grid args and post-constructor current list, rereads editor/index across Insert/Instantiate/LoadData/SetParent callbacks, parent getter precedes child-null failure, wrapping index additions; HoleTeleport calls correct DeviceEvent base, stores fields before one real Hide allocation, retains constructor allocation chain and clip/position/audio capture with1.2 volume and1.5 pitch; AttackRange allocates Rect/Single/Vector3 lists in original order and ends in no-op Object base call, removes diagnostic string; one typed public Path_enemyEditor Insert reference, all local signatures and method RVAs unchanged; no tests executed; parent chain pending')
(p/'Patcher83.cs').write_text(s)
t=Path('audit/native82-codex-2026-10-13/generate_remote.py').read_text();a=t.index("(s/'Program.cs').write_bytes");b=t.index('\n',a)
t=t[:a]+"(s/'Program.cs').write_bytes(base64.b64decode("+repr(base64.b64encode(s.encode()).decode())+"))"+t[b:]
t=t.replace('godspvz-native82-codex-2026-10-13','godspvz-native83-codex-2026-10-13').replace("'/tmp/godspvz-native81-codex-2026-10-13/candidate1.dll'","'/tmp/godspvz-native82-codex-2026-10-13/candidate1.dll'")
(p/'generate_remote.py').write_text(t)
d=json.loads(Path('audit/native30-continuation-2026-10-02/native55-native.json').read_text());(p/'native-methods.json').write_text(json.dumps([x for x in d['methods']if x['token']in('0x06000056','0x06000119')],indent=2))
print('Prepared three-method caller/constructor patcher with one scoped typed Insert binding')
