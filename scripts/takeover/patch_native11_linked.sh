#!/usr/bin/env bash
set -euo pipefail
REPO_ROOT="$(cd "$(dirname "${BASH_SOURCE[0]}")/../.." && pwd)"
INPUT="${1:?usage: patch_native11_linked.sh <postlink-native7.dll> <postlink-native11.dll>}"
OUTPUT="${2:?usage: patch_native11_linked.sh <postlink-native7.dll> <postlink-native11.dll>}"
PATCHER="$REPO_ROOT/scripts/takeover/PatcherNative11/PatcherNative11.csproj"
CECIL="$REPO_ROOT/Tools/Stage9Native4Recovery/tools/lib/netstandard2.0/Mono.Cecil.dll"
EXPECTED_LINKED_NATIVE10_SHA="8c71e31ae33991e95cc07129c8040d2eebe3b2e7db0006c8702ee39eaf75c595"
sha(){ sha256sum "$1"|awk '{print $1}'; }
fail(){ echo "[native11-linked] ERROR: $*" >&2; exit 1; }
[[ -f "$INPUT" ]] || fail "input missing"
[[ -f "$CECIL" ]] || fail "Mono.Cecil missing"
TMP10="${OUTPUT}.native10.tmp"
rm -f "$TMP10" "$TMP10".run* "$OUTPUT" "$OUTPUT".run*
bash "$REPO_ROOT/scripts/takeover/patch_native10_linked.sh" "$INPUT" "$TMP10" | tee "${OUTPUT}.native10.log"
[[ "$(sha "$TMP10")" == "$EXPECTED_LINKED_NATIVE10_SHA" ]] || fail "linked native10 SHA mismatch: $(sha "$TMP10")"
A="$OUTPUT.run1"; B="$OUTPUT.run2"
dotnet run --project "$PATCHER" -- "$TMP10" "$A" linked | tee "$OUTPUT.run1.log"
dotnet run --project "$PATCHER" -- "$TMP10" "$B" linked | tee "$OUTPUT.run2.log"
SA="$(sha "$A")"; SB="$(sha "$B")"
[[ "$SA" == "$SB" ]] || fail "native11 linked non-deterministic SHA"
cmp -s "$A" "$B" || fail "native11 linked non-deterministic bytes"
mv "$A" "$OUTPUT"; rm -f "$B" "$TMP10"
printf '%s\n' \
  "linked_native7_sha256=$(sha "$INPUT")" \
  "linked_native10_sha256=$EXPECTED_LINKED_NATIVE10_SHA" \
  "linked_native11_sha256=$SA" \
  "linked_deterministic_pair=PASS" \
  "targets=SkewText_d7,PoleTestJump,WarpText_d8,TMP_TextSelector_A::LateUpdate" \
  "production_promotion=NO"
echo NATIVE11_LINKED_PATCH_PASS
