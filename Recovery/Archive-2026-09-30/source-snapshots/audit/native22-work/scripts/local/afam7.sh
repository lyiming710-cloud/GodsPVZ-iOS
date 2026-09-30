cd /workspaces/GodsPVZ-native19/.validation/native22
python3 - <<'PY'
import json
c=json.load(open('out/afam2-candidates.json'))
rows=[]
for t,v in c.items():
    cls=(v['owner'] or '?').split('.')[-1].split('/')[0]
    rows.append((len(v['flips']), t, cls, v['name'], v['status']))
rows.sort()
print('%-12s %-4s %-22s %s'%('token','n','class','method'))
for n,t,cls,name,st in rows:
    print('%-12s %-4d %-22s %-46s %s'%(t,n,cls,name[:46],st))
print()
print('total', len(rows))
PY
