#!/usr/bin/env bash
set -euo pipefail
REPO_ROOT="$(cd "$(dirname "${BASH_SOURCE[0]}")/../.." && pwd)"
INPUT="${1:?usage: patch_native12_linked.sh <postlink-native7.dll> <postlink-native12.dll>}"
OUTPUT="${2:?usage: patch_native12_linked.sh <postlink-native7.dll> <postlink-native12.dll>}"
PATCHER="$REPO_ROOT/scripts/takeover/PatcherNative12/PatcherNative12.csproj"
CECIL="$REPO_ROOT/Tools/Stage9Native4Recovery/tools/lib/netstandard2.0/Mono.Cecil.dll"
EXPECTED_LINKED_NATIVE11_SHA="54b41555bc7c124d61ee18b08f3fc10b8ba6b0fc934df9c4a040c95a0cffde5d"
sha(){ sha256sum "$1"|awk '{print $1}'; }
fail(){ echo "[native12-linked] ERROR: $*" >&2; exit 1; }
[[ -f "$INPUT" ]] || fail "input missing"
[[ -f "$CECIL" ]] || fail "Mono.Cecil missing"
TMP11="${OUTPUT}.native11.tmp"
rm -f "$TMP11" "$TMP11".run* "$OUTPUT" "$OUTPUT".run*
bash "$REPO_ROOT/scripts/takeover/patch_native11_linked.sh" "$INPUT" "$TMP11" | tee "${OUTPUT}.native11.log"
[[ "$(sha "$TMP11")" == "$EXPECTED_LINKED_NATIVE11_SHA" ]] || fail "linked native11 SHA mismatch: $(sha "$TMP11")"
A="$OUTPUT.run1"; B="$OUTPUT.run2"
dotnet run --project "$PATCHER" -- "$TMP11" "$A" linked | tee "$OUTPUT.run1.log"
dotnet run --project "$PATCHER" -- "$TMP11" "$B" linked | tee "$OUTPUT.run2.log"
SA="$(sha "$A")"; SB="$(sha "$B")"
[[ "$SA" == "$SB" ]] || fail "native12 linked non-deterministic SHA"
cmp -s "$A" "$B" || fail "native12 linked non-deterministic bytes"
mv "$A" "$OUTPUT"; rm -f "$B" "$TMP11"
printf '%s\n' \
  "linked_native7_sha256=$(sha "$INPUT")" \
  "linked_native11_sha256=$EXPECTED_LINKED_NATIVE11_SHA" \
  "linked_native12_sha256=$SA" \
  "linked_deterministic_pair=PASS" \
  "targets=SkewText_d7,PoleTestJump,WarpText_d8,TMP_TextSelector_A::LateUpdate,Damage::CalculateAD<T>" \
  "production_promotion=NO"
echo NATIVE12_LINKED_PATCH_PASS
