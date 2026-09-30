#!/usr/bin/env bash
set -euo pipefail
REPO_ROOT="$(cd "$(dirname "${BASH_SOURCE[0]}")/../.." && pwd)"
INPUT="${1:?usage: patch_native13_linked.sh <postlink-native7.dll> <donor.dll> <postlink-native13.dll>}"
DONOR="${2:?usage: patch_native13_linked.sh <postlink-native7.dll> <donor.dll> <postlink-native13.dll>}"
OUTPUT="${3:?usage: patch_native13_linked.sh <postlink-native7.dll> <donor.dll> <postlink-native13.dll>}"
PATCHER="$REPO_ROOT/scripts/takeover/PatcherNative13/PatcherNative13.csproj"
CECIL="$REPO_ROOT/Tools/Stage9Native4Recovery/tools/lib/netstandard2.0/Mono.Cecil.dll"
EXPECTED_LINKED_NATIVE12_SHA="182b06634ee7dea1dd39c57bbc98db5cdce4f05d6050a201273e35a3db40445b"
sha(){ sha256sum "$1"|awk '{print $1}'; }
fail(){ echo "[native13-linked] ERROR: $*" >&2; exit 1; }
[[ -f "$INPUT" ]] || fail "input missing"
[[ -f "$DONOR" ]] || fail "donor missing"
[[ -f "$CECIL" ]] || fail "Mono.Cecil missing"
TMP12="${OUTPUT}.native12.tmp"
rm -f "$TMP12" "$OUTPUT" "$OUTPUT".run* "$OUTPUT".native12.log
bash "$REPO_ROOT/scripts/takeover/patch_native12_linked.sh" "$INPUT" "$TMP12" | tee "$OUTPUT.native12.log"
[[ "$(sha "$TMP12")" == "$EXPECTED_LINKED_NATIVE12_SHA" ]] || fail "linked native12 SHA mismatch: $(sha "$TMP12")"
A="$OUTPUT.run1"; B="$OUTPUT.run2"
dotnet run --project "$PATCHER" -- "$TMP12" "$DONOR" "$A" | tee "$OUTPUT.run1.log"
dotnet run --project "$PATCHER" -- "$TMP12" "$DONOR" "$B" | tee "$OUTPUT.run2.log"
SA="$(sha "$A")"; SB="$(sha "$B")"
[[ "$SA" == "$SB" ]] || fail "native13 linked non-deterministic SHA"
cmp -s "$A" "$B" || fail "native13 linked non-deterministic bytes"
mv "$A" "$OUTPUT"; rm -f "$B" "$TMP12"
printf '%s\n' \
  "linked_native7_sha256=$(sha "$INPUT")" \
  "linked_native12_sha256=$EXPECTED_LINKED_NATIVE12_SHA" \
  "donor_sha256=$(sha "$DONOR")" \
  "linked_native13_sha256=$SA" \
  "linked_deterministic_pair=PASS" \
  "target=TMPro.Examples.TMP_TextSelector_B::LateUpdate" \
  "production_promotion=NO"
echo NATIVE13_LINKED_PATCH_PASS
