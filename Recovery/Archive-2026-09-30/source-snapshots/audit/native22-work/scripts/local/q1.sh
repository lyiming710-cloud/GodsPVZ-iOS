cd /workspaces/GodsPVZ-native19/.validation/native22
echo "=== native-fields.json head ==="
python3 - <<'PY'
import json
d=json.load(open('/workspaces/GodsPVZ-native19/.validation/native22/pcnative/native-fields.json'))
print('type:', type(d).__name__)
if isinstance(d,list):
    print('len',len(d)); print(json.dumps(d[:3],ensure_ascii=False)[:600])
else:
    print('keys',list(d)[:10]); print(json.dumps(d,ensure_ascii=False)[:600])
PY
echo
echo "=== git state ==="
cd /workspaces/GodsPVZ-native19
git rev-parse --abbrev-ref HEAD
git rev-parse HEAD
git status --porcelain | head -20
git status --porcelain | wc -l
echo "--- is .validation ignored? ---"
git check-ignore -v .validation/native22/work/batch2.dll || echo "NOT ignored"
git check-ignore -v audit 2>/dev/null || echo "audit: not ignored/not present"
