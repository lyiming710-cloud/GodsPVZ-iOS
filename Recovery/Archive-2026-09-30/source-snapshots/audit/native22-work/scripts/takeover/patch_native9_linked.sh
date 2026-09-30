#!/usr/bin/env bash
set -euo pipefail
REPO_ROOT="$(cd "$(dirname "${BASH_SOURCE[0]}")/../.." && pwd)"
INPUT="${1:?usage: patch_native9_linked.sh <postlink-native7.dll> <postlink-native9.dll>}"
OUTPUT="${2:?usage: patch_native9_linked.sh <postlink-native7.dll> <postlink-native9.dll>}"
PATCHER="$REPO_ROOT/scripts/takeover/PatcherNative9/PatcherNative9.csproj"
CECIL="$REPO_ROOT/Tools/Stage9Native4Recovery/tools/lib/netstandard2.0/Mono.Cecil.dll"
EXPECTED_LINKED_NATIVE8_SHA="910a1a23f2d97eca8756b2be99591e0c14209a60fd02c2bd816f349796864138"
sha(){ sha256sum "$1"|awk '{print $1}'; }
fail(){ echo "[native9-linked] ERROR: $*" >&2; exit 1; }
[[ -f "$INPUT" ]] || fail "input missing"
[[ -f "$CECIL" ]] || fail "Mono.Cecil missing"
TMP8="${OUTPUT}.native8.tmp"
rm -f "$TMP8" "$TMP8".run* "$OUTPUT" "$OUTPUT".run*
bash "$REPO_ROOT/scripts/takeover/patch_native8_linked.sh" "$INPUT" "$TMP8" | tee "${OUTPUT}.native8.log"
[[ "$(sha "$TMP8")" == "$EXPECTED_LINKED_NATIVE8_SHA" ]] || fail "linked native8 SHA mismatch: $(sha "$TMP8")"
A="$OUTPUT.run1"; B="$OUTPUT.run2"
dotnet run --project "$PATCHER" -- "$TMP8" "$A" | tee "$OUTPUT.run1.log"
dotnet run --project "$PATCHER" -- "$TMP8" "$B" | tee "$OUTPUT.run2.log"
SA="$(sha "$A")"; SB="$(sha "$B")"
[[ "$SA" == "$SB" ]] || fail "native9 linked non-deterministic SHA"
cmp -s "$A" "$B" || fail "native9 linked non-deterministic bytes"
mv "$A" "$OUTPUT"; rm -f "$B" "$TMP8"
printf '%s\n' \
  "linked_native7_sha256=$(sha "$INPUT")" \
  "linked_native8_sha256=$EXPECTED_LINKED_NATIVE8_SHA" \
  "linked_native9_sha256=$SA" \
  "linked_deterministic_pair=PASS" \
  "targets=SkewTextExample/<WarpText>d__7::MoveNext,Zombie::ZC_PoleTestJump" \
  "production_promotion=NO"
echo NATIVE9_LINKED_PATCH_PASS
