cd /workspaces/GodsPVZ-native19
set -u
TOOLS=/workspaces/GodsPVZ-native19/Tools/Stage9NativeProbe
GA=/workspaces/GodsPVZ-native19/.validation/native22/pcnative/GameAssembly.dll
OUT=/workspaces/GodsPVZ-native19/.validation/native22/pcnative/slices
mkdir -p "$OUT"
ls -la "$GA"
echo "sha256: $(sha256sum "$GA" | cut -c1-64)"
echo "probe exists: $([ -f $TOOLS/pc_method_probe.py ] && echo yes || echo no)"
for rid in 840 824 825 1015 1021 1105; do
  token=$(printf '0x0600%04X' $rid)
  echo "---- token $token (rid $rid)"
  python3 "$TOOLS/pc_method_probe.py" "$GA" "$rid" 2>&1 | grep -E '^(METHOD|PDATA|NATIVE_SLICE_SHA256)' | head -5
done
