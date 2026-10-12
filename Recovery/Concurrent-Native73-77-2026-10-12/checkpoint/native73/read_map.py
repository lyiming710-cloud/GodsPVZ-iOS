from pathlib import Path
import json
p=Path('audit/stage9-native3/evidence/native-method-map.json')
d=json.loads(p.read_text())
print('TYPE',type(d).__name__)
if isinstance(d,dict):
    print('KEYS',list(d));rows=next((v for v in d.values()if isinstance(v,list)),[])
else:rows=d
for r in rows:
    if '13645c0' in json.dumps(r).lower()or r.get('name','')in ('GetAxis','GetAxisRaw'):
        print(r)
d=json.loads(Path('audit/native73-codex-2026-10-12/metadata-current.json').read_text())
for t in d['internals']:
    if t['name']in ('VFXAnimationEvent','ParticlesManager'):
        print(t['name'],t['methods'])
print('AttackPoint',[f for t in d['internals']if t['name']=='Plant' for f in t['fields']if 'attackPoint' in f['name']])
