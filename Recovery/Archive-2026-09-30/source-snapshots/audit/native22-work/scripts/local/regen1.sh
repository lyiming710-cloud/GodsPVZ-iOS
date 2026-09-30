#!/bin/bash
set -x
CAN=/workspaces/GodsPVZ-native19/.validation/native21/replay/canary
IL2CPP=/workspaces/GodsPVZ-native19/.validation/native19/replay/canary/il2cpp/build/deploy/il2cpp
RSP=$CAN/Library/Bee/artifacts/rsp/8082527414012784887.rsp
W=/workspaces/GodsPVZ-native19/.validation/native22
OUT=$W/regen-baseline
LOG=$W/regen-baseline.log

echo "=== 1. ldd missing libs ==="
ldd "$IL2CPP" 2>&1 | grep -i "not found"
echo "ldd-ok"

echo "=== 2. external lib present? ==="
for cand in "$CAN/Libraries/libil2cpp.a" "$CAN/il2cpp/Libraries/libil2cpp.a" "$CAN/il2cpp/build/deploy/Libraries/libil2cpp.a"; do
  [ -f "$cand" ] && echo "FOUND $cand"
done
find "$CAN" -maxdepth 4 -name "libil2cpp.a" -printf "%p\n" 2>/dev/null | head -5
echo "lib-scan-done"

rm -rf "$OUT"; mkdir -p "$OUT/cpp" "$OUT/data" "$OUT/symbols" "$W"

cd "$CAN" || exit 1

python3 - "$RSP" "$CAN" "$W/regen-args.txt" <<'PY'
import shlex, sys
rsp, can, out = sys.argv[1], sys.argv[2], sys.argv[3]
toks = shlex.split(open(rsp, encoding='utf-8-sig').read())
args = []
for t in toks:
    if t.startswith('--assembly='):
        v = t[len('--assembly='):]
        args.append('--assembly=%s/%s' % (can, v))
open(out, 'w').write(' '.join(args))
PY

ARGS=$(cat "$W/regen-args.txt")
echo "assembly count: $(tr ' ' '\n' <<< "$ARGS" | grep -c '^--assembly=')"
head -c 300 "$W/regen-args.txt"; echo

export PROJECT_DIR="$CAN"

nohup "$IL2CPP" --convert-to-cpp $ARGS \
  --generatedcppdir="$OUT/cpp" \
  --symbols-folder="$OUT/symbols" \
  --emit-null-checks \
  --enable-array-bounds-check \
  --dotnetprofile=unityaot-macos \
  --data-folder="$OUT/data" \
  > "$LOG" 2>&1 &

echo "started pid=$!"
sleep 8
echo "=== log size: $(stat -c%s "$LOG") ==="
tail -15 "$LOG"
