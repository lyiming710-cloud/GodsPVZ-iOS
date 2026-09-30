cd /workspaces/GodsPVZ-native19/.validation/native22
python3 - <<'PY'
import json
mp=json.load(open('pcnative/native-method-map.json'))
print('type',type(mp),'len',len(mp))
if isinstance(mp,dict):
    ks=list(mp)[:5]
    print('sample keys:',ks)
    print('sample vals:',[mp[k] for k in ks])
else:
    for x in mp[:3]: print(x)
    kk=set()
    for x in mp: kk|=set(x)
    print('keys present:',kk)
PY
