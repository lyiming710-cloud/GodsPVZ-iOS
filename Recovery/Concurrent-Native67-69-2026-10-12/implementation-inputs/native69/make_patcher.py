from pathlib import Path
import base64
p=Path('audit/native69-codex-2026-10-12')
s=Path('audit/native67-codex-2026-10-12/Patcher67.cs').read_text().replace('RepairNative67','RepairNative69')
s=s.replace('b87bd9b8670f5cc41fa3a138de156dac1aeeb8699b4aaa4cc67a7d99cda2bb3c','58e44b184e89ee3d587b7c7c8d9f57bf58740b96e9c8c71303414aac59cad7a0').replace('Pinned untested Native66 parent','Pinned untested Native68 parent')
a=s.index('  Member("Remove",');b=s.index('  ulong valid=',a)
s=s[:a]+'''  int paths=TS("System.Collections.Generic.List`1<EnemyPath>")*8+4;
  Member("get_Count",paths,new byte[]{0x20,0,8});
  Member("Insert",paths,new byte[]{0x20,2,1,8,0x13,0});
  Member("Remove",paths,new byte[]{0x20,1,2,0x13,0});
  Member("Remove",TS("System.Collections.Generic.List`1<Path_enemyEditor>")*8+4,new byte[]{0x20,1,2,0x13,0});
  var target=(MD)module.LookupToken(0x06000055);var element=module.GetTypes().Single(t=>t.FullName=="EnemyPath");
  var oldSig=md.GetBlobBytes(md.GetStandaloneSignature(MetadataTokens.StandaloneSignatureHandle((int)target.Body.LocalVarToken.RID)).Signature);
  R(oldSig.Length>2&&oldSig[0]==7&&oldSig[1]==10&&target.Body.Variables.Count==10,"Pinned existing 10-local editor method");
  var newSig=new byte[]{7,11}.Concat(oldSig[2..]).Concat(new byte[]{0x12}).Concat(Compress(checked((int)element.MetadataToken.RID*4))).ToArray();
  int newSignature=Blob(newSig);localRows.AddRange(Words(newSignature));
  added.Add(new{kind="StandAloneSig",token=$"0x{0x11000000+localFirst:X8}",signature_index=newSignature,old_signature_hex=Convert.ToHexString(oldSig),signature_hex=Convert.ToHexString(newSig),appended_local="EnemyPath"});
  R(memberRows.Count==24&&localRows.Count==2,"Exactly four public collection bindings and one local signature");
  R(md.GetTableRowCount(TableIndex.TypeSpec)<8192&&md.GetTableRowCount(TableIndex.TypeRef)<8192&&md.GetTableRowCount(TableIndex.TypeDef)<8192&&md.GetTableRowCount(TableIndex.MethodDef)<8192,"No parent index widening");
  var additions=new Dictionary<TableIndex,byte[]>{{TableIndex.MemberRef,memberRows.ToArray()},{TableIndex.StandAloneSig,localRows.ToArray()}};
'''+s[b:]
a=s.index('  var m=M(0x0600023C)');b=s.index('  foreach(var(token,(code,max))in targets)',a)
s=s[:a]+'''  var m=M(0x06000463);
  R(m.Body.Variables[1].VariableType.FullName=="System.Int32"&&m.Body.Variables[2].VariableType.FullName=="System.Collections.Generic.List`1<EnemyPath>","Existing index/list scratch");
  var il=new IL().Int(0).S(1).Label("loop").L(1).Arg(0).Ref(0x7B,F("Zombie","prePath")).Ref(0x6F,first).Branch(0x2F,"insert")
   .Arg(0).Ref(0x7B,F("Zombie","prePath")).L(1).Ref(0x6F,C(m,"get_Item")).Ref(0x7B,F("EnemyPath","arrived")).Branch(0x2C,"insert")
   .L(1).Int(1).O(0x58).S(1).Branch(0x2B,"loop")
   .Label("insert").Arg(0).Ref(0x7B,F("Zombie","prePath")).S(2).L(2).Branch(0x2C,"return")
   .L(2).L(1).Arg(1).Ref(0x6F,first+1).Label("return").O(0x2A);
  Set(0x06000463,il,3);
  m=M(0x06000055);
  il=new IL().Arg(0).Ref(0x7B,F("PathDataEditor","pathOnCheke")).Ref(0x7B,F("Path_enemyEditor","enemyPath")).S(10)
   .Arg(0).Ref(0x7B,F("PathDataEditor","enemy_BoardEditor")).O(0x14).Ref(0x28,C(m,"op_Inequality")).Branch(0x39,"load")
   .Arg(0).Ref(0x7B,F("PathDataEditor","enemy_BoardEditor")).Ref(0x7B,F("Enemy_boardEditor","preEnemy")).Ref(0x7B,F("Enemy","myPath")).L(10).Ref(0x6F,first+2).O(0x26)
   .Arg(0).Ref(0x7B,F("PathDataEditor","enemy_BoardEditor")).Ref(0x7B,F("Enemy_boardEditor","path_EnemyEditors"))
   .Arg(0).Ref(0x7B,F("PathDataEditor","pathOnCheke")).Ref(0x6F,first+3).O(0x26)
   .Arg(0).Ref(0x7B,F("PathDataEditor","pathOnCheke")).Ref(0x6F,C(m,"get_gameObject")).Ref(0x28,C(m,"Destroy"))
   .Arg(0).O(0x14).Ref(0x7D,F("PathDataEditor","pathOnCheke"))
   .Label("load").Arg(0).Arg(0).Ref(0x7B,F("PathDataEditor","pathIndex")).Ref(0x28,C(m,"LoadData")).O(0x2A);
  Set(0x06000055,il,2);
  m=M(0x060002C3);
  il=new IL().Arg(0).Arg(1).Ref(0x7D,F("Board","endTime"))
   .Arg(0).Float(1f).Ref(0x7D,F("Board","gameSpeed"))
   .Float(1f).Ref(0x28,C(m,"set_timeScale"))
   .Arg(0).Ref(0x7B,F("Board","buttomButtons")).Int(0).Ref(0x6F,C(m,"SetActive"))
   .Arg(0).Int(1).Ref(0x7D,F("Board","isWon"))
   .Ref(0x7E,F("ResourceManager","boardClips")).O(0x1F,10).Ref(0x6F,C(m,"get_Item"))
   .Arg(0).Ref(0x7B,F("Board","cameraPosition")).Ref(0x28,C(m,"AudioVolume")).Ref(0x28,C(m,"CreateAudioAtPoint")).O(0x26,0x2A);
  Set(0x060002C3,il,3);
  m=M(0x06000590);
  R(m.Body.Variables[1].VariableType.FullName=="Skill"&&m.Body.Variables[2].VariableType.FullName=="PlantSave"&&m.Body.Variables[9].VariableType.FullName=="System.Int32","Existing Skill/PlantSave/level scratch");
  var color=module.GetMemberReferences().OfType<MR>().Single(r=>r.FullName=="System.Void UnityEngine.Color::.ctor(System.Single,System.Single,System.Single,System.Single)");
  il=new IL().Arg(0).Ref(0x7B,F("Almanac_SkillSystem","plantSave")).Arg(0).Ref(0x7B,F("Almanac_SkillSystem","skillID")).Ref(0x6F,C(m,"GetSkill_ID")).S(1)
   .Arg(0).Ref(0x7B,F("Almanac_SkillSystem","plantSave")).O(0x25).Ref(0x7B,F("PlantSave","skillLevel")).Int(1).O(0x58).Ref(0x7D,F("PlantSave","skillLevel"))
   .Ref(0x7E,F("GlobalStaticVars","gLawnApp")).Ref(0x7B,F("GlobalStaticVars/LawnApp","savesManager"))
   .Arg(0).Ref(0x7B,F("Almanac_SkillSystem","plantSave")).Ref(0x7B,F("PlantSave","skillLevel")).S(9)
   .Arg(0).Ref(0x7B,F("Almanac_SkillSystem","plantSave")).L(9).Ref(0x6F,C(m,"GetSkillData_All"))
   .Arg(0).L(1).Float(1f).Float(1f).Float(0.2f).Float(1f).Ref(0x73,color).Ref(0x28,C(m,"UpdateSkillTexts"))
   .Arg(0).Ref(0x7B,F("Almanac_SkillSystem","plantSave")).S(2)
   .Arg(0).Ref(0x7B,F("Almanac_SkillSystem","button_TargetUp"))
   .L(2).Ref(0x7B,F("PlantSave","skillLevel")).L(2).Ref(0x7B,F("PlantSave","order")).L(2).Ref(0x7B,F("PlantSave","stars")).Int(1).Ref(0x6F,C(m,"SetState")).O(0x2A);
  Set(0x06000590,il,6);
'''+s[b:]
s=s.replace('token==0x0600023C','token==0x06000055')
s=s.replace('R(targets.Count==2,"Pinned scope 2")','R(targets.Count==4,"Pinned scope 4")')
s=s.replace('Two native-backed bodies; public List<Save> Remove/Count and Action<SwfClipController> Invoke; one appended string scratch signature, original rows unchanged; unqualified parent chain pending','Four native-backed bodies; arrived-prefix insert, editor deletion, win audio and skill level update; four public collection bindings, one captured EnemyPath scratch signature; original Color constructor reused with native RGBA bits; unqualified parent chain pending')
(p/'Patcher69.cs').write_text(s)
t=Path('audit/native68-codex-2026-10-12/generate_remote.py').read_text();start=t.index("(s/'Program.cs').write_bytes");end=t.index('\n',start)
t=t[:start]+"(s/'Program.cs').write_bytes(base64.b64decode("+repr(base64.b64encode(s.encode()).decode())+"))"+t[end:]
t=t.replace('godspvz-native68-codex','godspvz-native69-codex').replace('scripts/takeover/native68/Patcher','scripts/takeover/native69/Patcher')
t=t.replace('/tmp/godspvz-native67-codex-2026-10-12/candidate1.dll','/tmp/godspvz-native68-codex-2026-10-12/candidate1.dll')
(p/'generate_remote.py').write_text(t)
