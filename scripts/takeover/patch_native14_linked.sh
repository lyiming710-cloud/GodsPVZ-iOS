#!/usr/bin/env bash
set -euo pipefail
REPO_ROOT="$(cd "$(dirname "${BASH_SOURCE[0]}")/../.." && pwd)"
INPUT="${1:?usage: patch_native14_linked.sh <postlink-native7.dll> <selector-b-donor.dll> <postlink-native14.dll>}"
DONOR="${2:?usage: patch_native14_linked.sh <postlink-native7.dll> <selector-b-donor.dll> <postlink-native14.dll>}"
OUTPUT="${3:?usage: patch_native14_linked.sh <postlink-native7.dll> <selector-b-donor.dll> <postlink-native14.dll>}"
PATCHER="$REPO_ROOT/scripts/takeover/PatcherNative14/PatcherNative14.csproj"
CECIL="$REPO_ROOT/Tools/Stage9Native4Recovery/tools/lib/netstandard2.0/Mono.Cecil.dll"
EXPECTED_LINKED_NATIVE13_SHA="e7eaa90de7ed308b23efa9f20ef728e2f578523e8eea61a5547f97f85e908548"
sha(){ sha256sum "$1"|awk '{print $1}'; }
fail(){ echo "[native14-linked] ERROR: $*" >&2; exit 1; }
[[ -f "$INPUT" ]] || fail "input missing"
[[ -f "$DONOR" ]] || fail "Selector_B donor missing"
[[ -f "$CECIL" ]] || fail "Mono.Cecil missing"
TMP13="${OUTPUT}.native13.tmp"
rm -f "$TMP13" "$TMP13".run* "$OUTPUT" "$OUTPUT".run*
chmod +x "$REPO_ROOT/scripts/takeover/patch_native13_linked.sh"
"$REPO_ROOT/scripts/takeover/patch_native13_linked.sh" "$INPUT" "$DONOR" "$TMP13" | tee "${OUTPUT}.native13.log"
[[ "$(sha "$TMP13")" == "$EXPECTED_LINKED_NATIVE13_SHA" ]] || fail "linked native13 SHA mismatch: $(sha "$TMP13")"
A="$OUTPUT.run1"; B="$OUTPUT.run2"
dotnet run --project "$PATCHER" -- "$TMP13" "$A" linked | tee "$OUTPUT.run1.log"
dotnet run --project "$PATCHER" -- "$TMP13" "$B" linked | tee "$OUTPUT.run2.log"
SA="$(sha "$A")"; SB="$(sha "$B")"
[[ "$SA" == "$SB" ]] || fail "native14 linked non-deterministic SHA: $SA != $SB"
cmp -s "$A" "$B" || fail "native14 linked non-deterministic bytes"
mv "$A" "$OUTPUT"; rm -f "$B" "$TMP13"
printf '%s\n' \
  "linked_native7_sha256=$(sha "$INPUT")" \
  "linked_native13_sha256=$EXPECTED_LINKED_NATIVE13_SHA" \
  "linked_native14_sha256=$SA" \
  "linked_deterministic_pair=PASS" \
  "target=ElementManager::CreateNewElements<T>" \
  "target_methoddef=0x06000126" \
  "production_promotion=NO"
echo NATIVE14_LINKED_PATCH_PASS
