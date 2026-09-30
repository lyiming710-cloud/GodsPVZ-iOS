set -e
cd /workspaces/GodsPVZ-native19/.validation/native22
W=/workspaces/GodsPVZ-native19/.validation/native22/work
export GODSPVZ_RESOLVER=/workspaces/GodsPVZ-native19/.validation/native21/replay/canary/Library/Bee/artifacts/iOS/ManagedStripped
dotnet bin/PatcherNative19.dll "$W/batch1.dll" "$W/all-methods.batch1.json" export-all > "$W/export-batch1.log" 2>&1
echo "export rc=$?"
sha256sum "$W/batch1.dll"
python3 - <<'PY'
import json
a=json.load(open('/workspaces/GodsPVZ-native19/.validation/native22/out/all-methods.json'))
b=json.load(open('/workspaces/GodsPVZ-native19/.validation/native22/work/all-methods.batch1.json'))
da={m['token']:m for m in a['methods']}
db={m['token']:m for m in b['methods']}
print('methods before=%d after=%d'%(len(da),len(db)))
print('token sets identical:', set(da)==set(db))
same=0; changed=[]
for t,m in da.items():
    n=db.get(t)
    if n is None: continue
    if json.dumps(m,sort_keys=True)==json.dumps(n,sort_keys=True): same+=1
    else: changed.append(t)
print('unchanged=%d  CHANGED=%s'%(same, sorted(changed)))
print('types unchanged:', json.dumps(a['types'],sort_keys=True)==json.dumps(b['types'],sort_keys=True))
PY
