#!/bin/bash
set -e
CAN=/workspaces/GodsPVZ-native19/.validation/native21/replay/canary
W=/workspaces/GodsPVZ-native19/.validation/native22
IN=$W/input
SRC=$CAN/Library/Bee/artifacts/iOS/ManagedStripped/GodsPVZRuntime1.dll

mkdir -p "$IN"
cp -p "$SRC" "$IN/GodsPVZRuntime1.baseline.dll"
echo "=== baseline snapshot ==="
ls -la "$IN"
sha256sum "$IN/GodsPVZRuntime1.baseline.dll" "$SRC"

echo ""
echo "=== is the input git-tracked? ==="
cd /workspaces/GodsPVZ-native19
git ls-files --error-unmatch .validation/native21/replay/canary/Library/Bee/artifacts/iOS/ManagedStripped/GodsPVZRuntime1.dll 2>&1 | head -2 || echo "NOT tracked in git"
echo "-- any GodsPVZRuntime1.dll tracked anywhere:"
git ls-files | grep -i "GodsPVZRuntime1" | head -10
echo "-- any Assembly-CSharp tracked:"
git ls-files | grep -i "Assembly-CSharp" | head -10

echo ""
echo "=== cpp baseline manifest (first 5 + counts) ==="
cd "$CAN/Library/Bee/artifacts/iOS/il2cppOutput/cpp"
ls *.cpp | wc -l
python3 - <<'PY'
import hashlib, os, json
d='/workspaces/GodsPVZ-native19/.validation/native21/replay/canary/Library/Bee/artifacts/iOS/il2cppOutput/cpp'
man={}
for n in sorted(os.listdir(d)):
    p=os.path.join(d,n)
    if os.path.isfile(p) and (n.endswith('.cpp') or n.endswith('.h')):
        with open(p,'rb') as f: b=f.read()
        man[n]={'sha256':hashlib.sha256(b).hexdigest(),'bytes':len(b),'lines':b.count(b'\n')}
o='/workspaces/GodsPVZ-native19/.validation/native22/input/cpp-manifest.json'
json.dump(man,open(o,'w'),indent=0)
print('manifest entries:',len(man))
for k in list(man)[:5]:
    print(' ',k,man[k]['sha256'][:16],man[k]['lines'])
print('--- GodsPVZRuntime1__8.cpp:',man.get('GodsPVZRuntime1__8.cpp'))
PY
echo ""
echo "=== engine.py head (how repairs are expressed) ==="
head -60 "$W/engine.py"
