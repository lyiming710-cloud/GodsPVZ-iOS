python3 - <<'PY'
import json
p='/workspaces/GodsPVZ-native19/.validation/native22/out/all-methods.json'
d=json.load(open(p))
print('top keys:',list(d))
ms=d['methods']
print('methods:',len(ms))
for m in ms:
    if m.get('token')=='0x06000348':
        print('sample keys:',list(m))
        print(json.dumps(m,ensure_ascii=False)[:2000])
        break
else:
    print('0x06000348 not found; sample keys:',list(ms[0]))
    print(json.dumps(ms[0],ensure_ascii=False)[:1200])
PY
