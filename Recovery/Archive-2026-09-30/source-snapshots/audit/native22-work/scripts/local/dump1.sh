set -x
cd /workspaces/GodsPVZ-native19
BASE=.validation/native22/input/GodsPVZRuntime1.baseline.dll
OUT=.validation/native22/out/all-methods.regen.json
export GODSPVZ_RESOLVER=/workspaces/GodsPVZ-native19/.validation/native21/replay/canary/Library/Bee/artifacts/iOS/ManagedStripped
dotnet .validation/native22/bin/PatcherNative19.dll "$BASE" "$OUT" export-all > .validation/native22/out/regen-dump.log 2>&1
echo "rc=$?"
tail -5 .validation/native22/out/regen-dump.log
ls -la "$OUT"
echo ""
echo "=== diff against old dump ==="
python3 - <<'PY'
import json,hashlib
a=json.load(open('/workspaces/GodsPVZ-native19/.validation/native22/out/all-methods.json'))
b=json.load(open('/workspaces/GodsPVZ-native19/.validation/native22/out/all-methods.regen.json'))
print('old methods=%d  new methods=%d'%(len(a['methods']),len(b['methods'])))
print('old types=%d  new types=%d'%(len(a['types']),len(b['types'])))
da={m['token']:m for m in a['methods']}
db={m['token']:m for m in b['methods']}
same=diff=0; examples=[]
for t,m in da.items():
    n=db.get(t)
    if n is None: continue
    if json.dumps(m,sort_keys=True)==json.dumps(n,sort_keys=True): same+=1
    else:
        diff+=1
        if len(examples)<5: examples.append(t)
print('identical=%d  differing=%d  tokens only in old=%d  only in new=%d'%(same,diff,len(set(da)-set(db)),len(set(db)-set(da))))
print('differing examples:',examples)
if diff==0 and len(set(da)-set(db))==0 and len(set(db)-set(da))==0:
    print('VERDICT: existing all-methods.json is PROVEN to describe the baseline dll')
else:
    print('VERDICT: existing dump is STALE - must use all-methods.regen.json')
PY
