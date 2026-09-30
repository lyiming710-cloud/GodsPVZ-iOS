cd /workspaces/GodsPVZ-native19 || exit 1
echo "=== branch / HEAD ==="
git rev-parse --abbrev-ref HEAD
git rev-parse HEAD
echo "=== status ==="
git status --porcelain
echo "=== diff stat vs HEAD ==="
git diff --stat HEAD | tail -5
echo
echo "=== artifact hashes ==="
cd /workspaces/GodsPVZ-native19/.validation/native22 || exit 1
sha256sum work/batch1.dll work/batch2.dll work/batch2a.dll 2>&1
sha256sum edits-batch2.json edits-batch2a.json 2>&1
sha256sum out/afam2-candidates.json out/afam3-accepted.json out/afam3-quarantine.json 2>&1
sha256sum out/behave-sites.json out/tiers.json 2>&1
sha256sum pcnative/GameAssembly.dll 2>&1
echo
echo "=== baseline dll ==="
sha256sum /workspaces/GodsPVZ-native19/.validation/native21/replay/canary/Library/Bee/artifacts/iOS/ManagedStripped/Assembly-CSharp.dll 2>&1 | head -2
echo
echo "=== out dir ==="
ls -la out | head -30
echo "=== logs present ==="
ls -la *.log *.log.txt 2>/dev/null | head -20
