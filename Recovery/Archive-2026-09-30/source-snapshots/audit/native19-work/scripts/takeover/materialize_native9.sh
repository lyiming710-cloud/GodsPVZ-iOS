#!/usr/bin/env bash
set -euo pipefail
REPO_ROOT="$(cd "$(dirname "${BASH_SOURCE[0]}")/../.." && pwd)"
SOURCE="${1:-$REPO_ROOT/Tools/Stage9Native4Recovery/candidate/Assembly-CSharp-native8-skewtext.dll}"
OUT="${2:-$REPO_ROOT/Tools/Stage9Native4Recovery/candidate/Assembly-CSharp-native9-polejump.dll}"
EXPECTED_SOURCE_SHA="f31775b974b14f6ee420496efb8fe0eb7636132ae55f8d3bee2cb87565a34e07"
LEGACY_ROOT="/workspaces/GodsPVZ-iOS"
CECIL="$REPO_ROOT/Tools/Stage9Native4Recovery/tools/lib/netstandard2.0/Mono.Cecil.dll"
PATCHER="$REPO_ROOT/scripts/takeover/PatcherNative9/PatcherNative9.csproj"
sha(){ sha256sum "$1"|awk '{print $1}'; }
fail(){ echo "[native9-materialize] ERROR: $*" >&2; exit 1; }
[[ -f "$SOURCE" ]] || fail "native8 input missing"
[[ "$(sha "$SOURCE")" == "$EXPECTED_SOURCE_SHA" ]] || fail "native8 SHA mismatch"
[[ -f "$CECIL" ]] || fail "Mono.Cecil missing"
if [[ -e "$LEGACY_ROOT" || -L "$LEGACY_ROOT" ]]; then
  [[ "$(realpath "$LEGACY_ROOT")" == "$REPO_ROOT" ]] || fail "$LEGACY_ROOT points elsewhere"
else
  if [[ -d /workspaces && -w /workspaces ]]; then ln -s "$REPO_ROOT" "$LEGACY_ROOT"; else sudo mkdir -p /workspaces; sudo ln -s "$REPO_ROOT" "$LEGACY_ROOT"; fi
fi
mkdir -p "$(dirname "$OUT")"
A="$OUT.run1"; B="$OUT.run2"; rm -f "$A" "$B" "$OUT"
dotnet run --project "$PATCHER" -- "$SOURCE" "$A" | tee "$OUT.run1.log"
dotnet run --project "$PATCHER" -- "$SOURCE" "$B" | tee "$OUT.run2.log"
SA="$(sha "$A")"; SB="$(sha "$B")"
[[ "$SA" == "$SB" ]] || fail "non-deterministic SHA $SA != $SB"
cmp -s "$A" "$B" || fail "non-deterministic bytes"
mv "$A" "$OUT"; rm -f "$B"
printf '%s\n' \
  "source_native8_sha256=$(sha "$SOURCE")" \
  "native9_sha256=$SA" \
  "patcher_program_sha256=$(sha "$REPO_ROOT/scripts/takeover/PatcherNative9/Program.cs")" \
  "patcher_project_sha256=$(sha "$PATCHER")" \
  "deterministic_run_pair=PASS" \
  "target=Zombie::ZC_PoleTestJump" \
  "target_methoddef=0x060004A3" \
  "pc_native_va=0x180371750" \
  "pc_native_sha256=b9cb66024814e677417c73e96cacce5f4636bc12baf7119e2f2f4ae4c16df9ae" \
  "production_promotion=NO"
echo NATIVE9_MATERIALIZATION_PASS
