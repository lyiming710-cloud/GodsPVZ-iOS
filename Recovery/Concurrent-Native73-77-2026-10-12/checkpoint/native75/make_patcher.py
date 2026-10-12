from pathlib import Path
import base64
p=Path('audit/native75-codex-2026-10-12')
s=Path('audit/native74-codex-2026-10-12/Patcher74.cs').read_text().replace('RepairNative74','RepairNative75')
s=s.replace('47df498caf8fd0d02a494c995654c972399e98d2d0614fa25faaf7036b125f43','71c28b1cad9ff8e5dd456121c2b0747792c3281f66c3da701002e91d9e3b9f4c').replace('Pinned untested Native73 parent','Pinned untested Native74 parent')
a=s.index(' static (byte[] Image,object Evidence) Metadata');b=s.index(' static void CopyTo',a)
s=s[:a]+s[b:]
a=s.index('  var(after,metadata)=');b=s.index('  MD M(',a)
s=s[:a]+'  byte[] after=before.ToArray();\n'+s[b:]
a=s.index('  var m=M(0x0600027C)');b=s.index('  foreach(var(token,(code,max))in targets)',a)
s=s[:a]+'''  int StringToken(MD method,string text){var i=method.Body.Instructions.Single(i=>i.OpCode==OpCodes.Ldstr&&(string)i.Operand==text);var sec=pe.PEHeaders.SectionHeaders.Single(s=>method.RVA>=s.VirtualAddress&&method.RVA<s.VirtualAddress+s.SizeOfRawData);int header=sec.PointerToRawData+method.RVA-sec.VirtualAddress;R((before[header]&3)==3&&(BitConverter.ToUInt16(before,header)>>12)==3,"Pinned fat literal body");return BitConverter.ToInt32(before,header+12+i.Offset+1);}
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
'''+s[b:]
s=s.replace('candidate_sha256=H(after),metadata,methods=report','candidate_sha256=H(after),metadata_unchanged_by_construction=true,methods=report')
a=s.index('scope="Four native-backed bodies;');b=s.index('"},new JsonSerializerOptions',a)
s=s[:a]+'scope="Four native-backed bodies; ordered pause effects and captured audio, flag-meter field rereads and -195 local x, almanac captured pre-delta countdown with ordered NaN clamp and vector logging, captured lock image with original color alpha; all original metadata and local signatures retained; unqualified parent chain pending'+s[b:]
(p/'Patcher75.cs').write_text(s)
t=Path('audit/native74-codex-2026-10-12/generate_remote.py').read_text();a=t.index("(s/'Program.cs').write_bytes");b=t.index('\n',a)
t=t[:a]+"(s/'Program.cs').write_bytes(base64.b64decode("+repr(base64.b64encode(s.encode()).decode())+"))"+t[b:]
t=t.replace('godspvz-native74-codex','godspvz-native75-codex').replace('scripts/takeover/native74/Patcher','scripts/takeover/native75/Patcher').replace('/tmp/godspvz-native73-codex-2026-10-12/candidate1.dll','/tmp/godspvz-native74-codex-2026-10-12/candidate1.dll')
(p/'generate_remote.py').write_text(t)
