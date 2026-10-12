from pathlib import Path
import subprocess, json, shutil
r=Path('/workspaces/GodsPVZ-native24-codespace')
for args in (['git','status','--short'],['git','rev-parse','HEAD'],['git','branch','--show-current']):
    s=subprocess.run(args,cwd=r,text=True,capture_output=True)
    print(args,s.returncode,s.stdout,s.stderr)
print('disk',shutil.disk_usage(r),shutil.disk_usage('/tmp'))
print('parent72',Path('/tmp/godspvz-native72-codex-2026-10-12/candidate1.dll').exists())
print('AGENTS',[(str(p),p.read_text())for p in (Path('/AGENTS.md'),Path('/workspaces/AGENTS.md'),r/'AGENTS.md',r/'scripts/AGENTS.md',r/'scripts/takeover/AGENTS.md')if p.exists()])
