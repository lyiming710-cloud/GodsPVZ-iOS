from pathlib import Path
import os,json,subprocess
root=Path('/workspaces/GodsPVZ-native19')
result={'workspaces':sorted(p.name for p in Path('/workspaces').iterdir()),'head':subprocess.check_output(['git','-C',str(root),'rev-parse','HEAD'],text=True).strip(),'status':subprocess.check_output(['git','-C',str(root),'status','--porcelain'],text=True),'files':[]}
for base in [root/'.validation',root/'work',root/'logs']:
 if not base.exists():continue
 for top,dirs,names in os.walk(base):
  dirs[:]=[d for d in dirs if d not in {'bin','obj','__pycache__','node_modules','dotnet','runtime','canary'}]
  for n in names:
   p=Path(top)/n
   if not p.is_symlink():result['files'].append({'path':p.relative_to(root).as_posix(),'bytes':p.stat().st_size})
print(json.dumps(result))
