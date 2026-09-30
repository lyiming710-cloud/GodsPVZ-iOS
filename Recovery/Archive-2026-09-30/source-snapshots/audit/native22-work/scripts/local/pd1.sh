cd /workspaces/GodsPVZ-native19/.validation/native22
python3 - <<'PY'
import json, struct, sys
sys.path.insert(0,'.')
from native22_native_probe import *  # noqa

dll = 'pcnative/GameAssembly.dll'
data = open(dll,'rb').read()
pe = PE(dll) if 'PE' in dir() else None
print('dir:', [n for n in dir() if n[0].isupper()])

mp = json.load(open('pcnative/native-method-map.json'))
print('map type:', type(mp), 'len', len(mp))
ent = None
if isinstance(mp, dict):
    ent = mp.get('0x0600034C') or mp.get('0X0600034C')
else:
    for x in mp:
        if str(x.get('token','')).lower()=='0x0600034c': ent=x; break
print('entry for 0x0600034C:', ent)
# how many entries map to this VA
if isinstance(mp, list):
    va = ent.get('va') if ent else None
    hits=[x for x in mp if x.get('va')==va]
    print('entries sharing VA 0x%X: %d' % (va, len(hits)))
    for h in hits[:10]: print('   ', h.get('token'), h.get('name'), h.get('image'))
PY
