import subprocess,json
from pathlib import Path
p=Path(__file__).parent;git='C:/Users/86136/.cache/codex-runtimes/codex-primary-runtime/dependencies/native/git/cmd/git.exe';w=p.parent/'native23-publication'
for a in (['config','--get','remote.origin.url'],['config','--get','remote.origin.promisor'],['count-objects','-v'],['log','--all','--format=%h %ad %s','--date=short','-45']):
 r=subprocess.run([git,'-C',str(w)]+a,capture_output=True,text=True);print(r.stdout)
rows=json.loads((p/'inventory-files.json').read_text(encoding='utf-8'))
for x in rows:
 if x['group']=='audit/local-preflight' and x['bytes']>1000000:print(x['path'],x['bytes'])
