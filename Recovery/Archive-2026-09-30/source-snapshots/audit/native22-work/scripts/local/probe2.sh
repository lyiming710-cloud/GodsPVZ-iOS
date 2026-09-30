cd /workspaces/GodsPVZ-native19/.validation/native22
python3 - <<'PY'
import json, collections, os
p='out/all-methods.json'
print('size', os.path.getsize(p))
d=json.load(open(p))
print('top keys', list(d.keys())[:10])
ms=d['methods']
print('methods', len(ms))
m=ms[0]
print('method keys', list(m.keys()))
print(json.dumps({k:(v if k!='instructions' else v[:6]) for k,v in m.items()}, ensure_ascii=False)[:1200])
print('--- locals sample', json.dumps(ms[0].get('locals'), ensure_ascii=False)[:300])
PY
echo "=== ls out"
ls -la out
echo "=== engineer logs present"
ls -la *.log 2>/dev/null
