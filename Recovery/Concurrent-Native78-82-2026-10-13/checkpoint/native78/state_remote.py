from pathlib import Path
import subprocess,json,shutil
r=Path('/workspaces/GodsPVZ-native24-codespace')
for args in(['git','status','--short'],['git','rev-parse','HEAD'],['git','branch','--show-current']):
 p=subprocess.run(args,cwd=r,text=True,capture_output=True);print(args,p.returncode,p.stdout,p.stderr)
print('disk',shutil.disk_usage(r),shutil.disk_usage('/tmp'))
for p in (Path('/tmp/godspvz-native77-codex-2026-10-12/candidate1.dll'),Path('/tmp/godspvz-native73-77-pending-2026-10-12.zip'),Path('/tmp/godspvz-native61-gemini-tests-2026-10-12')):
 print('path',str(p),p.exists())
for n in(60,61):
 p=Path(f'/tmp/godspvz-native{n}-gemini-tests-2026-10-12')
 if p.exists():print('results',n,[(f.name,f.stat().st_size)for f in p.iterdir()if f.is_file()and('REPORT'in f.name or'result'in f.name or'Archive'in f.name)])
print('agents_instructions',[(str(p),p.read_text())for p in(Path('/AGENTS.md'),Path('/workspaces/AGENTS.md'),r/'AGENTS.md',r/'scripts/AGENTS.md',r/'scripts/takeover/AGENTS.md')if p.exists()])
