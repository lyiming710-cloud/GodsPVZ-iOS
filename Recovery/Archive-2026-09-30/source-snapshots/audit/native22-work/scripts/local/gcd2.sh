cd /workspaces/GodsPVZ-native19/.validation/native22
python3 - <<'PY'
import json
from pathlib import Path
NB=Path('.')
bodies=json.loads((NB/'out'/'all-methods.json').read_text())
by={m['token']:m for m in bodies['methods']}
cls=json.loads((NB/'out'/'classification.json').read_text())
mth={m['token']:m for m in cls['methods']}
targets=['0x060003D1','0x06000540','0x060003C7','0x0600041C','0x0600041F','0x060000FE','0x06000100','0x0600034B','0x0600040B','0x06000348']
for t in targets:
    b=by.get(t)
    if not b: continue
    print('%-12s cpp=%-3d %s' % (t, mth[t]['cpp']['errors'] if 'cpp' in mth[t] else 0, b['name']))
    for x in b['instructions']:
        o=x['operand'] or {}
        if isinstance(o,dict) and o.get('name') in ('GetComponent','get_transform','get_gameObject'):
            print('      IL_%04X %-8s owner=%-28s ret=%-28s identity=%s' % (x['offset'],x['opcode'],o.get('owner'),o.get('ret'),o.get('identity','')[:90]))
PY
