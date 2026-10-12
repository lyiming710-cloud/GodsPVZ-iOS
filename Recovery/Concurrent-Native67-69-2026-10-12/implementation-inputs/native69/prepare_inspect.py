from pathlib import Path
import base64
p=Path('audit/native69-codex-2026-10-12')
s=Path('audit/native68-codex-2026-10-12/Inspect.cs').read_text()
s=s.replace('new[]{0x06000110,0x060004C1,0x06000683,0x060006BF}', 'new[]{0x06000463,0x06000055,0x060002C3,0x06000590}')
s=s.replace('new[]{"Damage","Element","Save","PlantSave","Card_Choose","PrepareUIController","SeedChooserScreen"}', 'new[]{"Zombie","EnemyPath","PathDataEditor","Enemy_boardEditor","Path_enemyEditor","Enemy","Board","Almanac_SkillSystem","PlantSave","GlobalStaticVars","LawnApp"}')
start=s.index('.Where(m=>m.Name=="GetEnumerator"');end=s.index('.Select(Method);',start)
s=s[:start]+'.Where(m=>m.DeclaringType.FullName=="UnityEngine.Color"||m.DeclaringType.FullName=="UnityEngine.Vector3"||m.DeclaringType.FullName=="System.Collections.Generic.List`1<EnemyPath>"||m.DeclaringType.FullName=="System.Collections.Generic.List`1<Path_enemyEditor>")'+s[end:]
(p/'Inspect.cs').write_text(s)
t=Path('audit/native68-codex-2026-10-12/inspect_remote.py').read_text();start=t.index("(s/'Program.cs').write_bytes");end=t.index('\n',start)
t=t[:start]+"(s/'Program.cs').write_bytes(base64.b64decode("+repr(base64.b64encode(s.encode()).decode())+"))"+t[end:]
t=t.replace('godspvz-native68-codex','godspvz-native69-codex').replace('/tmp/godspvz-native67-codex-2026-10-12/candidate1.dll','/tmp/godspvz-native68-codex-2026-10-12/candidate1.dll')
(p/'inspect_remote.py').write_text(t)
