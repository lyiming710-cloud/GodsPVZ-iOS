from pathlib import Path
import base64
p=Path('audit/native80-codex-2026-10-13');p.mkdir(exist_ok=True)
s=Path('audit/native79-codex-2026-10-13/Inspect.cs').read_text().replace('new[]{0x06000238,0x06000365,0x0600075F,0x060001E5,0x06000636}','new[]{0x060003A0,0x06000206,0x060001AC,0x060003C5,0x0600087D,0x06000734}')
s=s.replace('"ObjectSpin","Plant","Save","SaveList","SavesManager","MainUIController","ParticlesManager"','"Plant","ProjectManager","EnemySelecter","ZombieSelect","Project","SwfManager","TMP_TextEventHandler"')
a=s.index('var refs=');b=s.index('\nvar internals=',a)
s=s[:a]+'var refs=a.MainModule.GetMemberReferences().OfType<MethodReference>().Select(Method);'+s[b:]
s=s.replace('t.FullName.Contains("List`1<Save>")','t.FullName.Contains("List`1<ZombieSelect>")||t.FullName.Contains("SwfClipAsset")')
(p/'Inspect.cs').write_text(s)
t=Path('audit/native79-codex-2026-10-13/inspect_remote.py').read_text()
a=t.index("(s/'Program.cs').write_bytes");b=t.index('\n',a)
t=t[:a]+"(s/'Program.cs').write_bytes(base64.b64decode("+repr(base64.b64encode(s.encode()).decode())+"))"+t[b:]
t=t.replace('godspvz-native79-codex-2026-10-13','godspvz-native80-codex-2026-10-13').replace('/tmp/godspvz-native77-restored-2026-10-13/snapshot/candidates/native77/candidate1.dll','/tmp/godspvz-native79-codex-2026-10-13/candidate1.dll')
(p/'inspect_remote.py').write_text(t)
print('Prepared read-only current metadata for six source targets')
