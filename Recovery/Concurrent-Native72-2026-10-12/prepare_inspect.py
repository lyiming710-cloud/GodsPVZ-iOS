from pathlib import Path
import base64
p=Path('audit/native72-codex-2026-10-12')
s=Path('audit/native71-codex-2026-10-12/Inspect.cs').read_text()
s=s.replace('new[]{0x06000532,0x06000632}', 'new[]{0x0600013A,0x06000544,0x060006BC,0x0600021A,0x06000683}')
s=s.replace('new[]{"AdventureMenu_ModeButton","LevelItem_HA"}', 'new[]{"ResourceManager","Zombie_charred","AlmanacUIController","SeedChooserScreen"}')
start=s.index('.Where(m=>m.DeclaringType.FullName.Contains("Selectable")');end=s.index('.Select(Method);',start)
s=s[:start]+'.Where(m=>m.DeclaringType.FullName=="UnityEngine.Vector3"||m.Name=="op_Equality"||m.Name=="GetEnumerator"&&m.DeclaringType.FullName.Contains("Card_Choose"))'+s[end:]
(p/'Inspect.cs').write_text(s)
t=Path('audit/native71-codex-2026-10-12/inspect_remote.py').read_text();start=t.index("(s/'Program.cs').write_bytes");end=t.index('\n',start)
t=t[:start]+"(s/'Program.cs').write_bytes(base64.b64decode("+repr(base64.b64encode(s.encode()).decode())+"))"+t[end:]
t=t.replace('godspvz-native71-codex','godspvz-native72-codex').replace('/tmp/godspvz-native70-codex-2026-10-12/candidate1.dll','/tmp/godspvz-native71-codex-2026-10-12/candidate1.dll')
(p/'inspect_remote.py').write_text(t)
