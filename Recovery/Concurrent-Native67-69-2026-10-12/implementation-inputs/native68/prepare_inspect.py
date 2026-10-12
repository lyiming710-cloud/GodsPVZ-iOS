from pathlib import Path
import base64
p=Path('audit/native68-codex-2026-10-12')
s=Path('audit/native67-codex-2026-10-12/Inspect.cs').read_text()
s=s.replace('new[]{0x0600023C,0x06000877}', 'new[]{0x06000110,0x060004C1,0x06000683,0x060006BF}')
s=s.replace('new[]{"SavesManager","SaveList","Save","SwfClipController"}', 'new[]{"Damage","Element","Save","PlantSave","Card_Choose","PrepareUIController","SeedChooserScreen"}')
start=s.index('.Where(m=>m.DeclaringType.FullName.Contains("List`1<Save>")')
end=s.index('.Select(Method);',start)
s=s[:start]+'.Where(m=>m.Name=="GetEnumerator"||m.DeclaringType.FullName.Contains("Enumerator"))'+s[end:]
(p/'Inspect.cs').write_text(s)
t=Path('audit/native67-codex-2026-10-12/inspect_remote.py').read_text()
start=t.index("(s/'Program.cs').write_bytes");end=t.index('\n',start)
t=t[:start]+"(s/'Program.cs').write_bytes(base64.b64decode("+repr(base64.b64encode(s.encode()).decode())+"))"+t[end:]
t=t.replace('godspvz-native67-codex','godspvz-native68-codex').replace('/tmp/godspvz-pending-restored-2026-10-12/snapshot/candidates/native66/candidate1.dll','/tmp/godspvz-native67-codex-2026-10-12/candidate1.dll')
(p/'inspect_remote.py').write_text(t)
