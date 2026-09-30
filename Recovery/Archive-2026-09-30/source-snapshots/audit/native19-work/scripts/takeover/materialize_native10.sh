#!/usr/bin/env bash
set -euo pipefail
REPO_ROOT="$(cd "$(dirname "${BASH_SOURCE[0]}")/../.." && pwd)"
SOURCE="${1:-$REPO_ROOT/Tools/Stage9Native4Recovery/candidate/Assembly-CSharp-native9-polejump.dll}"
OUT="${2:-$REPO_ROOT/Tools/Stage9Native4Recovery/candidate/Assembly-CSharp-native10-warptext.dll}"
EXPECTED_SOURCE_SHA="6e3d3fe4283ce4079f22727e9e6a13c290cb2944b8b0eedd1c705a0815b3906f"
CECIL="$REPO_ROOT/Tools/Stage9Native4Recovery/tools/lib/netstandard2.0/Mono.Cecil.dll"
PATCHER="$REPO_ROOT/scripts/takeover/PatcherNative10/PatcherNative10.csproj"
sha(){ sha256sum "$1"|awk '{print $1}'; }
fail(){ echo "[native10-materialize] ERROR: $*" >&2; exit 1; }
[[ -f "$SOURCE" ]] || fail "native9 input missing"
[[ "$(sha "$SOURCE")" == "$EXPECTED_SOURCE_SHA" ]] || fail "native9 SHA mismatch: $(sha "$SOURCE")"
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
  "source_native9_sha256=$(sha "$SOURCE")" \
  "native10_sha256=$SA" \
  "patcher_program_sha256=$(sha "$REPO_ROOT/scripts/takeover/PatcherNative10/Program.cs")" \
  "patcher_project_sha256=$(sha "$PATCHER")" \
  "deterministic_run_pair=PASS" \
  "target=TMPro.Examples.WarpTextExample/<WarpText>d__8::MoveNext" \
  "target_methoddef=0x0600081C" \
  "pc_native_va=0x1803CD3F0" \
  "pc_native_sha256=55d77a157aa8dca15d1d8dd95267ba4f5b7a12ef4849d1cfa1ca97c8cab75daf" \
  "production_promotion=NO"
echo NATIVE10_MATERIALIZATION_PASS
