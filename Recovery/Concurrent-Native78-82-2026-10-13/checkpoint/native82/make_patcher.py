from pathlib import Path
import base64,json
p=Path('audit/native82-codex-2026-10-13');p.mkdir(exist_ok=True)
s=Path('audit/native78-codex-2026-10-13/Patcher78.cs').read_text().replace('RepairNative78','RepairNative82')
s=s.replace('290767e0d4e513612c56b3d06cff3edf0894e58808a6338d6c6b87019a2a3f83','5b143bae30c9747fa7beab7d92c580b8c556523857fd35b0e4bb1dab0164ddae').replace('Pinned untested Native77 parent','Pinned untested Native81 parent')
a=s.index('  var m=M(0x0600061D);');b=s.index('  foreach(var(token,(code,max))in targets)',a)
s=s[:a]+'''  var m=M(0x0600087D);R(m.IsStatic&&m.Parameters.Count==1&&m.Body.Variables[7].VariableType.FullName=="UnityEngine.GameObject","Pinned Swf singleton static signature and created object local");
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
'''+s[b:]
old_scope=s[s.index('scope="')+7:s.index('"},new JsonSerializerOptions',s.index('scope="'))]
s=s.replace(old_scope,'Two original-native-backed bodies; Swf singleton has two independent Unity truth tests, Find even when creation disabled, fresh field at return, constructor before raw-null AddComponent guard and result assignments after callbacks; TMP Awake uses source-proven exact Type reference equality, two independent gameObject reads, canvas field rereads after Unity comparison/renderMode, raw receiver faults and prior camera preservation; all metadata/local signatures unchanged; no tests executed; unqualified parent chain pending')
(p/'Patcher82.cs').write_text(s)
t=Path('audit/native81-codex-2026-10-13/generate_remote.py').read_text();a=t.index("(s/'Program.cs').write_bytes");b=t.index('\n',a)
t=t[:a]+"(s/'Program.cs').write_bytes(base64.b64decode("+repr(base64.b64encode(s.encode()).decode())+"))"+t[b:]
t=t.replace('godspvz-native81-codex-2026-10-13','godspvz-native82-codex-2026-10-13').replace("'/tmp/godspvz-native80-codex-2026-10-13/candidate1.dll'","'/tmp/godspvz-native81-codex-2026-10-13/candidate1.dll'")
(p/'generate_remote.py').write_text(t)
d=json.loads(Path('audit/native30-continuation-2026-10-02/native55-native.json').read_text());(p/'native-methods.json').write_text(json.dumps([x for x in d['methods']if x['token']in('0x0600087D','0x06000734')],indent=2))
print('Prepared two metadata-neutral bodies from source-proven equality and singleton CFG')
