from pathlib import Path
import base64
p=Path('audit/native70-codex-2026-10-12')
s=Path('audit/native69-codex-2026-10-12/Inspect.cs').read_text()
s=s.replace('new[]{0x06000463,0x06000055,0x060002C3,0x06000590}', 'new[]{0x060001FA,0x060002AE,0x060003AA,0x06000318,0x06000261}')
s=s.replace('new[]{"Zombie","EnemyPath","PathDataEditor","Enemy_boardEditor","Path_enemyEditor","Enemy","Board","Almanac_SkillSystem","PlantSave","GlobalStaticVars","LawnApp"}', 'new[]{"ProjectManager","Board","Plant","Project","Device","Damage","Zombie"}')
start=s.index('.Where(m=>m.DeclaringType.FullName=="UnityEngine.Color"');end=s.index('.Select(Method);',start)
s=s[:start]+'.Where(m=>m.DeclaringType.FullName=="UnityEngine.Vector3"||m.DeclaringType.FullName.Contains("<Project>")||m.DeclaringType.FullName.Contains("<Popup>")||m.Name=="Add"&&m.DeclaringType.FullName.Contains("<Zombie>"))'+s[end:]
(p/'Inspect.cs').write_text(s)
t=Path('audit/native69-codex-2026-10-12/inspect_remote.py').read_text();start=t.index("(s/'Program.cs').write_bytes");end=t.index('\n',start)
t=t[:start]+"(s/'Program.cs').write_bytes(base64.b64decode("+repr(base64.b64encode(s.encode()).decode())+"))"+t[end:]
t=t.replace('godspvz-native69-codex','godspvz-native70-codex').replace('/tmp/godspvz-native68-codex-2026-10-12/candidate1.dll','/tmp/godspvz-native69-codex-2026-10-12/candidate1.dll')
(p/'inspect_remote.py').write_text(t)
