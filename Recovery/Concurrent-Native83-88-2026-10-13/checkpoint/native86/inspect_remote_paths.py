from pathlib import Path
import shutil,json
w=Path('/workspaces/GodsPVZ-native24-codespace')
roots=[Path('/tmp'),w]
print(json.dumps({'paths':[{'path':str(p),'exists':p.exists(),'disk':shutil.disk_usage(p)._asdict()} for p in roots],
 'candidate_directories':[{'path':str(p),'files':[x.name for x in p.iterdir()]} for p in Path('/tmp').glob('godspvz-native*-codex-2026-10-13') if p.is_dir()],
 'boot_id':Path('/proc/sys/kernel/random/boot_id').read_text().strip()},indent=2))
