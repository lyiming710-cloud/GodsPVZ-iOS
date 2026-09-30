#!/usr/bin/env bash
set -euo pipefail
REPO_ROOT="$(cd "$(dirname "${BASH_SOURCE[0]}")/../.." && pwd)"
SOURCE="${1:-$REPO_ROOT/Tools/Stage9Native4Recovery/candidate/Assembly-CSharp-native10-warptext.dll}"
OUT="${2:-$REPO_ROOT/Tools/Stage9Native4Recovery/candidate/Assembly-CSharp-native11-selector.dll}"
EXPECTED_SOURCE_SHA="7708dd46a22838b02112995a953993e410de73bb17393282dcae96161f3b0cd2"
CECIL="$REPO_ROOT/Tools/Stage9Native4Recovery/tools/lib/netstandard2.0/Mono.Cecil.dll"
PATCHER="$REPO_ROOT/scripts/takeover/PatcherNative11/PatcherNative11.csproj"
sha(){ sha256sum "$1"|awk '{print $1}'; }
fail(){ echo "[native11-materialize] ERROR: $*" >&2; exit 1; }
[[ -f "$SOURCE" ]] || fail "native10 input missing"
[[ "$(sha "$SOURCE")" == "$EXPECTED_SOURCE_SHA" ]] || fail "native10 SHA mismatch: $(sha "$SOURCE")"
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
  "source_native10_sha256=$(sha "$SOURCE")" \
  "native11_sha256=$SA" \
  "patcher_program_sha256=$(sha "$REPO_ROOT/scripts/takeover/PatcherNative11/Program.cs")" \
  "patcher_project_sha256=$(sha "$PATCHER")" \
  "deterministic_run_pair=PASS" \
  "target=TMPro.Examples.TMP_TextSelector_A::LateUpdate" \
  "target_methoddef=0x060007C1" \
  "pc_native_va=0x1803B8350" \
  "pc_native_full_sha256=042a4c2e4429f02430a72782f3aa7058e3be947edc322c6791cb9144c2434f55" \
  "production_promotion=NO"
echo NATIVE11_MATERIALIZATION_PASS
