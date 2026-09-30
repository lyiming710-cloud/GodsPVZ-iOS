#!/usr/bin/env bash
set -euo pipefail
REPO_ROOT="$(cd "$(dirname "${BASH_SOURCE[0]}")/../.." && pwd)"
SOURCE="${1:-$REPO_ROOT/Tools/Stage9Native4Recovery/candidate/Assembly-CSharp-native13-selectorb.dll}"
OUT="${2:-$REPO_ROOT/Tools/Stage9Native4Recovery/candidate/Assembly-CSharp-native14-elements.dll}"
EXPECTED_SOURCE_SHA="fbc42f136d28916d155b1876139c0b3b19994bdb4bf24aca7eacd8c7556a62b4"
CECIL="$REPO_ROOT/Tools/Stage9Native4Recovery/tools/lib/netstandard2.0/Mono.Cecil.dll"
PATCHER="$REPO_ROOT/scripts/takeover/PatcherNative14/PatcherNative14.csproj"
sha(){ sha256sum "$1"|awk '{print $1}'; }
fail(){ echo "[native14-materialize] ERROR: $*" >&2; exit 1; }
[[ -f "$SOURCE" ]] || fail "native13 input missing"
[[ "$(sha "$SOURCE")" == "$EXPECTED_SOURCE_SHA" ]] || fail "native13 SHA mismatch: $(sha "$SOURCE")"
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
  "source_native13_sha256=$(sha "$SOURCE")" \
  "native14_sha256=$SA" \
  "patcher_program_sha256=$(sha "$REPO_ROOT/scripts/takeover/PatcherNative14/Program.cs")" \
  "patcher_project_sha256=$(sha "$PATCHER")" \
  "deterministic_run_pair=PASS" \
  "target=ElementManager::CreateNewElements<T>" \
  "target_methoddef=0x06000126" \
  "pc_refshare_va=0x180439680" \
  "pc_refshare_sha256=191078e9a5434bd3f7d89c411ac132388ebef9799f154acf5889f9541291227e" \
  "pc_valueshare_va=0x1804390C0" \
  "pc_valueshare_sha256=64a719753c31e448656803a777561aeaf34e1197475ff6a4c5ce9b4c66627cb6" \
  "production_promotion=NO"
echo NATIVE14_MATERIALIZATION_PASS
