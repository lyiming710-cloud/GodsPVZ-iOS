#!/usr/bin/env bash
set -euo pipefail
REPO_ROOT="$(cd "$(dirname "${BASH_SOURCE[0]}")/../.." && pwd)"
INPUT="${1:?usage: patch_native10_linked.sh <postlink-native7.dll> <postlink-native10.dll>}"
OUTPUT="${2:?usage: patch_native10_linked.sh <postlink-native7.dll> <postlink-native10.dll>}"
PATCHER="$REPO_ROOT/scripts/takeover/PatcherNative10/PatcherNative10.csproj"
CECIL="$REPO_ROOT/Tools/Stage9Native4Recovery/tools/lib/netstandard2.0/Mono.Cecil.dll"
EXPECTED_LINKED_NATIVE9_SHA="6b0acdd62e595762c6a529b4b8e8908df1a560e52bdb3b5458cd1fb1c361a241"
sha(){ sha256sum "$1"|awk '{print $1}'; }
fail(){ echo "[native10-linked] ERROR: $*" >&2; exit 1; }
[[ -f "$INPUT" ]] || fail "input missing"
[[ -f "$CECIL" ]] || fail "Mono.Cecil missing"
TMP9="${OUTPUT}.native9.tmp"
rm -f "$TMP9" "$TMP9".run* "$OUTPUT" "$OUTPUT".run*
bash "$REPO_ROOT/scripts/takeover/patch_native9_linked.sh" "$INPUT" "$TMP9" | tee "${OUTPUT}.native9.log"
[[ "$(sha "$TMP9")" == "$EXPECTED_LINKED_NATIVE9_SHA" ]] || fail "linked native9 SHA mismatch: $(sha "$TMP9")"
A="$OUTPUT.run1"; B="$OUTPUT.run2"
dotnet run --project "$PATCHER" -- "$TMP9" "$A" linked | tee "$OUTPUT.run1.log"
dotnet run --project "$PATCHER" -- "$TMP9" "$B" linked | tee "$OUTPUT.run2.log"
SA="$(sha "$A")"; SB="$(sha "$B")"
[[ "$SA" == "$SB" ]] || fail "native10 linked non-deterministic SHA"
cmp -s "$A" "$B" || fail "native10 linked non-deterministic bytes"
mv "$A" "$OUTPUT"; rm -f "$B" "$TMP9"
printf '%s\n' \
  "linked_native7_sha256=$(sha "$INPUT")" \
  "linked_native9_sha256=$EXPECTED_LINKED_NATIVE9_SHA" \
  "linked_native10_sha256=$SA" \
  "linked_deterministic_pair=PASS" \
  "targets=SkewTextExample/<WarpText>d__7::MoveNext,Zombie::ZC_PoleTestJump,WarpTextExample/<WarpText>d__8::MoveNext" \
  "production_promotion=NO"
echo NATIVE10_LINKED_PATCH_PASS
