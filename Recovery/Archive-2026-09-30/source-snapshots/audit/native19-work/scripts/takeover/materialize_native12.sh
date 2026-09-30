#!/usr/bin/env bash
set -euo pipefail
REPO_ROOT="$(cd "$(dirname "${BASH_SOURCE[0]}")/../.." && pwd)"
SOURCE="${1:-$REPO_ROOT/Tools/Stage9Native4Recovery/candidate/Assembly-CSharp-native11-selector.dll}"
OUT="${2:-$REPO_ROOT/Tools/Stage9Native4Recovery/candidate/Assembly-CSharp-native12-damage.dll}"
EXPECTED_SOURCE_SHA="c78fe103cf7b1cccda016ab9f272d668ce3c44fe9450f2ac1d9d97508af5838a"
CECIL="$REPO_ROOT/Tools/Stage9Native4Recovery/tools/lib/netstandard2.0/Mono.Cecil.dll"
PATCHER="$REPO_ROOT/scripts/takeover/PatcherNative12/PatcherNative12.csproj"
sha(){ sha256sum "$1"|awk '{print $1}'; }
fail(){ echo "[native12-materialize] ERROR: $*" >&2; exit 1; }
[[ -f "$SOURCE" ]] || fail "native11 input missing"
[[ "$(sha "$SOURCE")" == "$EXPECTED_SOURCE_SHA" ]] || fail "native11 SHA mismatch: $(sha "$SOURCE")"
[[ -f "$CECIL" ]] || fail "Mono.Cecil missing"
mkdir -p "$(dirname "$OUT")"
A="$OUT.run1"; B="$OUT.run2"; rm -f "$A" "$B" "$OUT"
dotnet run --project "$PATCHER" -- "$SOURCE" "$A" | tee "$OUT.run1.log"
dotnet run --project "$PATCHER" -- "$SOURCE" "$B" | tee "$OUT.run2.log"
SA="$(sha "$A")"; SB="$(sha "$B")"
[[ "$SA" == "$SB" ]] || fail "non-deterministic SHA $SA != $SB"
cmp -s "$A" "$B" || fail "non-deterministic bytes"
mv "$A" "$OUT"; rm -f "$B"
printf '%s\n' \
  "source_native11_sha256=$(sha "$SOURCE")" \
  "native12_sha256=$SA" \
  "patcher_program_sha256=$(sha "$REPO_ROOT/scripts/takeover/PatcherNative12/Program.cs")" \
  "patcher_project_sha256=$(sha "$PATCHER")" \
  "deterministic_run_pair=PASS" \
  "target=Damage::CalculateAD<T>" \
  "target_methoddef=0x06000111" \
  "pc_refshare_va=0x180434460" \
  "pc_refshare_sha256=a31476d0aeae5eacae566ddf469ae9a78df098381a45c68fc13de786daf020dd" \
  "pc_valueshare_va=0x180434280" \
  "pc_valueshare_sha256=755675ec4a32581a56792b5412fb37861b86e1546236700eb2a5a48b7776b9b4" \
  "production_promotion=NO"
echo NATIVE12_MATERIALIZATION_PASS
