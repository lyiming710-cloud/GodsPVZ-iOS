from pathlib import Path
import base64
p=Path('audit/native79-codex-2026-10-13');p.mkdir(exist_ok=True)
s=Path('audit/native78-codex-2026-10-13/Inspect.cs').read_text().replace('new[]{0x0600061D,0x06000483,0x06000238,0x06000365,0x060003CA,0x060001E5,0x06000636}','new[]{0x06000238,0x06000365,0x0600075F,0x060001E5,0x06000636}')
s=s.replace('m.DeclaringType.FullName=="UnityEngine.Vector3"','m.DeclaringType.FullName=="UnityEngine.Quaternion"||m.DeclaringType.FullName=="UnityEngine.Color32"||m.DeclaringType.FullName=="UnityEngine.Vector3"')
s=s.replace('"GridConfig_A","System2","GridState","Zombie","Plant","Save","SaveList","SavesManager","MainUIController","ParticlesManager"','"ObjectSpin","Plant","Save","SaveList","SavesManager","MainUIController","ParticlesManager"')
(p/'Inspect.cs').write_text(s)
t=Path('audit/native78-codex-2026-10-13/inspect_remote.py').read_text()
a=t.index("(s/'Program.cs').write_bytes");b=t.index('\n',a)
t=t[:a]+"(s/'Program.cs').write_bytes(base64.b64decode("+repr(base64.b64encode(s.encode()).decode())+"))"+t[b:]
t=t.replace('godspvz-native78-codex-2026-10-13','godspvz-native79-codex-2026-10-13')
(p/'inspect_remote.py').write_text(t)
print('Prepared read-only three-method current metadata inspection')
