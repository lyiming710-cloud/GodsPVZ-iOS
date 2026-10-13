from pathlib import Path
import base64
p=Path('audit/native86-codex-2026-10-13');p.mkdir(exist_ok=True)
s=Path('audit/native83-codex-2026-10-13/Inspect.cs').read_text().replace('new[]{0x06000056,0x06000119,0x060001C8,0x060002CE,0x06000443,0x0600047D}','new[]{0x060006DF,0x060006FE,0x06000199,0x06000386,0x060000E0,0x06000117,0x060000FD,0x06000169}')
s=s.replace('"PathDataEditor","HoleTeleport","MouseManager","Board","Zombie","PathData","Path_enemyEditor","GlobalStaticVars","ResourceManager","BoardManager","Projectile"','"TextLink","Window_Q","DialogueManager_OnBoard","Plant","AttackRange","Hide","Buff","DeviceEvent","Board","Zombie","GlobalStaticVars","ResourceManager","BoardManager","Projectile"')
(p/'Inspect.cs').write_text(s)
t=Path('audit/native83-codex-2026-10-13/inspect_remote.py').read_text();a=t.index("(s/'Program.cs').write_bytes");b=t.index('\n',a)
t=t[:a]+"(s/'Program.cs').write_bytes(base64.b64decode("+repr(base64.b64encode(s.encode()).decode())+"))"+t[b:]
t=t.replace('godspvz-native83-codex-2026-10-13','godspvz-native86-codex-2026-10-13')
(p/'inspect_remote.py').write_text(t)
print('Prepared read-only next caller and constructor metadata inventory')
