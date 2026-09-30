#!/usr/bin/env bash
set -euo pipefail
REPO_ROOT="$(cd "$(dirname "${BASH_SOURCE[0]}")/../.." && pwd)"
INPUT="${1:?usage: materialize_native13.sh <native12.dll> <donor.dll> <native13.dll>}"
DONOR="${2:?usage: materialize_native13.sh <native12.dll> <donor.dll> <native13.dll>}"
OUTPUT="${3:?usage: materialize_native13.sh <native12.dll> <donor.dll> <native13.dll>}"
PATCHER="$REPO_ROOT/scripts/takeover/PatcherNative13/PatcherNative13.csproj"
sha(){ sha256sum "$1"|awk '{print $1}'; }
fail(){ echo "[native13] ERROR: $*" >&2; exit 1; }
[[ -f "$INPUT" ]] || fail "input missing"
[[ -f "$DONOR" ]] || fail "donor missing"
A="$OUTPUT.run1"; B="$OUTPUT.run2"
rm -f "$A" "$B" "$OUTPUT" "$OUTPUT".run*.log

dotnet run --project "$PATCHER" -- "$INPUT" "$DONOR" "$A" | tee "$OUTPUT.run1.log"
dotnet run --project "$PATCHER" -- "$INPUT" "$DONOR" "$B" | tee "$OUTPUT.run2.log"
SA="$(sha "$A")"; SB="$(sha "$B")"
[[ "$SA" == "$SB" ]] || fail "non-deterministic SHA: $SA vs $SB"
cmp -s "$A" "$B" || fail "non-deterministic bytes"
mv "$A" "$OUTPUT"; rm -f "$B"
printf '%s\n' \
  "source_native12_sha256=$(sha "$INPUT")" \
  "donor_sha256=$(sha "$DONOR")" \
  "native13_sha256=$SA" \
  "patcher_program_sha256=$(sha "$REPO_ROOT/scripts/takeover/PatcherNative13/Program.cs")" \
  "donor_source_sha256=$(sha "$REPO_ROOT/scripts/takeover/native13_donor/TMP_TextSelector_B_Donor.cs")" \
  "deterministic_run_pair=PASS" \
  "target=TMPro.Examples.TMP_TextSelector_B::LateUpdate" \
  "target_methoddef=0x060007C9" \
  "pc_native_va=0x1803B8D90" \
  "pc_native_sha256=254235dc30236ceabc03e652cf1165f043449f226c2f2dd7c52ec71a1389b246" \
  "production_promotion=NO"
echo NATIVE13_MATERIALIZATION_PASS
