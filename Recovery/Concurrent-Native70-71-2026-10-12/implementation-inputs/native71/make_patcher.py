from pathlib import Path
import base64
p=Path('audit/native71-codex-2026-10-12')
s=Path('audit/native70-codex-2026-10-12/Patcher70.cs').read_text().replace('RepairNative70','RepairNative71')
s=s.replace('67611b6286fe37c58f706eaab114037bc1bce4a10187a3f28e8327e519e28359','775d43f4413940b5c2e1cb440fff0933e6c58388d7794992baff72fcbe8c8973').replace('Pinned untested Native69 parent','Pinned untested Native70 parent')
s=s.replace('throw new Exception("Existing public member name required: "+name);', 'R(name=="get_interactable","Only the public Selectable getter name may append");int ix=strings.Length;strings=strings.Concat(raw).ToArray();R(strings.Length<65536,"No string index widening");return ix;')
a=s.index('  int popup=');b=s.index('  ulong valid=',a)
s=s[:a]+'''  var selectable=module.GetTypeReferences().Single(t=>t.FullName=="UnityEngine.UI.Selectable");R(selectable.MetadataToken.TokenType==TokenType.TypeRef,"Existing Selectable TypeRef");
  Member("get_interactable",checked((int)selectable.MetadataToken.RID*8+1),new byte[]{0x20,0,2});
  R(memberRows.Count==6,"Exactly one public Boolean getter");
  R(md.GetTableRowCount(TableIndex.TypeSpec)<8192&&md.GetTableRowCount(TableIndex.TypeRef)<8192&&md.GetTableRowCount(TableIndex.TypeDef)<8192&&md.GetTableRowCount(TableIndex.MethodDef)<8192,"No parent index widening");
  var additions=new Dictionary<TableIndex,byte[]>{{TableIndex.MemberRef,memberRows.ToArray()}};
'''+s[b:]
s=s.replace('streams[streams.FindIndex(s=>s.Name=="#Blob")]=new("#Blob",blob);', 'streams[streams.FindIndex(s=>s.Name=="#Blob")]=new("#Blob",blob);streams[streams.FindIndex(s=>s.Name=="#Strings")]=new("#Strings",strings);')
a=s.index('  var m=M(0x060002AE)');b=s.index('  foreach(var(token,(code,max))in targets)',a)
s=s[:a]+'''  var m=M(0x06000532);
  foreach(var(token,owner)in new[]{(0x06000532,"AdventureMenu_ModeButton"),(0x06000632,"LevelItem_HA")}){
   m=M(token);
   var il=new IL().Arg(0).Ref(0x7B,F(owner,"button")).Ref(0x6F,first).Branch(0x2C,"return")
    .Arg(1).Ref(0x28,C(m,"op_Implicit")).Branch(0x2C,"return")
    .Arg(1).Ref(0x28,C(m,"get_main")).Ref(0x6F,C(m,"get_transform")).Ref(0x6F,C(m,"get_position"))
    .Ref(0x28,C(m,"AudioVolume")).Ref(0x28,C(m,"CreateAudioAtPoint")).O(0x26)
    .Label("return").O(0x2A);
   Set(token,il,3);
  }
'''+s[b:]
a=s.index('  m=M(0x060001FA)');b=s.index('  R(report.Count==4',a)
s=s[:a]+s[b:]
s=s.replace('R(report.Count==4,"Pinned scope 4")','R(report.Count==2,"Pinned scope 2")')
s=s.replace('Four native-backed bodies; project-ID filtered foreach with finally, popup queue handling, chomper classification and bonfire scale; two public Popup bindings appended; existing enum/local signatures and Vector3 constructor reused; scalar native constants and NaN guards retained; unqualified parent chain pending','Two native-backed button-audio bodies; public Selectable getter replaces inaccessible private field read, original helper field-read equivalence required; camera position captured by value before volume; one MemberRef and bounded getter-name append; unqualified parent chain pending')
(p/'Patcher71.cs').write_text(s)
t=Path('audit/native70-codex-2026-10-12/generate_remote.py').read_text();start=t.index("(s/'Program.cs').write_bytes");end=t.index('\n',start)
t=t[:start]+"(s/'Program.cs').write_bytes(base64.b64decode("+repr(base64.b64encode(s.encode()).decode())+"))"+t[end:]
t=t.replace('godspvz-native70-codex','godspvz-native71-codex').replace('scripts/takeover/native70/Patcher','scripts/takeover/native71/Patcher')
t=t.replace('/tmp/godspvz-native69-codex-2026-10-12/candidate1.dll','/tmp/godspvz-native70-codex-2026-10-12/candidate1.dll')
(p/'generate_remote.py').write_text(t)
