from pathlib import Path
import base64
p=Path('audit/native83-codex-2026-10-13');p.mkdir(exist_ok=True)
s=Path('audit/native80-codex-2026-10-13/Inspect.cs').read_text().replace('new[]{0x060003A0,0x06000206,0x060001AC,0x060003C5,0x0600087D,0x06000734}','new[]{0x06000056,0x06000119,0x060001C8,0x060002CE,0x06000443,0x0600047D}')
s=s.replace('"Plant","ProjectManager","EnemySelecter","ZombieSelect","Project","SwfManager","TMP_TextEventHandler"','"PathDataEditor","HoleTeleport","MouseManager","Board","Zombie","PathData","Path_enemyEditor","GlobalStaticVars","ResourceManager","BoardManager","Projectile"')
s=s.replace('t.FullName.Contains("List`1<ZombieSelect>")||t.FullName.Contains("SwfClipAsset")','true')
(p/'Inspect.cs').write_text(s)
t=Path('audit/native80-codex-2026-10-13/inspect_remote.py').read_text();a=t.index("(s/'Program.cs').write_bytes");b=t.index('\n',a)
t=t[:a]+"(s/'Program.cs').write_bytes(base64.b64decode("+repr(base64.b64encode(s.encode()).decode())+"))"+t[b:]
t=t.replace('godspvz-native80-codex-2026-10-13','godspvz-native83-codex-2026-10-13').replace('/tmp/godspvz-native79-codex-2026-10-13/candidate1.dll','/tmp/godspvz-native82-codex-2026-10-13/candidate1.dll')
(p/'inspect_remote.py').write_text(t)
print('Prepared read-only current parent82 source metadata inventory')
