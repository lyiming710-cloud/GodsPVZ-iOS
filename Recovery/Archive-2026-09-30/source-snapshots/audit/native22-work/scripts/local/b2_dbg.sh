cd /workspaces/GodsPVZ-native19/.validation/native22
python3 - <<'PY'
import json
W='/workspaces/GodsPVZ-native19/.validation/native22/work'
a=json.load(open(W+'/all-methods.batch1.json'))
b=json.load(open(W+'/all-methods.batch2.json'))
da={m['token']:m for m in a['methods']}
db={m['token']:m for m in b['methods']}
t='0x0600000B'
ia=da[t]['instructions']; ib=db[t]['instructions']
for k in [0,10]:
    print('--- index',k,'---')
    print('  before:',json.dumps(ia[k],ensure_ascii=False)[:400])
    print('  after :',json.dumps(ib[k],ensure_ascii=False)[:400])
PY
