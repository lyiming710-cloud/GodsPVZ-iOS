from pathlib import Path
import json,subprocess
p=Path(__file__).parent
d=json.loads((p/'inventory.json').read_text(encoding='utf-8'))
for k,v in d['groups'].items():
 if '/' not in k or not Path(k).suffix:print(k,v['files'],round(v['bytes']/1e6,1),'MB')
rows=json.loads((p/'inventory-files.json').read_text(encoding='utf-8'))
print('LARGEST')
for x in sorted(rows,key=lambda x:-x['bytes'])[:65]:print(x['bytes'],x['path'])
print('POTENTIAL PRIVATE/TOOL FILES')
for x in rows:
 if any(s in x['path'].lower() for s in ('license','credential','.env','token','password','certificate','secret','runtime-6','transport','adapters')):print(x['path'])
git='C:/Users/86136/.cache/codex-runtimes/codex-primary-runtime/dependencies/native/git/cmd/git.exe'
print(subprocess.check_output([git,'-C',str(p.parent/'native23-publication'),'worktree','list','--porcelain'],text=True))
print(subprocess.check_output([git,'-C',str(p.parent/'native23-publication'),'for-each-ref','--format=%(refname) %(objectname)'],text=True))
