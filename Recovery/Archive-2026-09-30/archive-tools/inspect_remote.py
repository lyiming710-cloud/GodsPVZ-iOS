from pathlib import Path
import json,collections
p=Path(__file__).parent;d=json.loads((p/'remote-inventory.json').read_text(encoding='utf-8'))
print('WORKSPACES',d['workspaces'],'HEAD',d['head'],'STATUS',d['status'])
g=collections.defaultdict(lambda:[0,0]);extensions=collections.Counter()
for x in d['files']:
 parts=x['path'].split('/');name='/'.join(parts[:3]);g[name][0]+=1;g[name][1]+=x['bytes'];extensions[Path(x['path']).suffix]+=1
for k,v in sorted(g.items(),key=lambda x:-x[1][1])[:30]:print(k,v[0],round(v[1]/1e6,1))
print(extensions)
print('PROJECT SCRIPT ROOTS')
for x in d['files']:
 if Path(x['path']).suffix in {'.py','.cs','.csproj','.sh','.md','.json'} and x['bytes']<100000 and len(x['path'].split('/'))<5:print(x['path'])
