#!/usr/bin/env bash
set -euo pipefail

REPO_ROOT="$(cd "$(dirname "${BASH_SOURCE[0]}")/../.." && pwd)"
NATIVE7_SOURCE="${1:-$REPO_ROOT/Tools/Stage9Native4Recovery/candidate/Assembly-CSharp-native7-ladder-closure.dll}"
NATIVE8_OUT="${2:-$REPO_ROOT/Tools/Stage9Native4Recovery/candidate/Assembly-CSharp-native8-skewtext.dll}"
EXPECTED_NATIVE7_SHA256="b9dadd905244ccfddfa86f87c2edc1e60d883441632b5bb359ffccab4d2faa29"
LEGACY_ROOT="/workspaces/GodsPVZ-iOS"
CECIL="$REPO_ROOT/Tools/Stage9Native4Recovery/tools/lib/netstandard2.0/Mono.Cecil.dll"
PATCHER="$REPO_ROOT/scripts/takeover/PatcherNative8/PatcherNative8.csproj"
PROGRAM="$REPO_ROOT/scripts/takeover/PatcherNative8/Program.cs"

fail(){ echo "[native8-materialize] ERROR: $*" >&2; exit 1; }
sha(){ sha256sum "$1" | awk '{print $1}'; }

[[ -f "$NATIVE7_SOURCE" ]] || fail "native7 input missing: $NATIVE7_SOURCE"
[[ "$(sha "$NATIVE7_SOURCE")" == "$EXPECTED_NATIVE7_SHA256" ]] || fail "native7 SHA mismatch"
[[ -f "$CECIL" ]] || fail "Mono.Cecil missing"
command -v dotnet >/dev/null 2>&1 || fail "dotnet missing"

if [[ -e "$LEGACY_ROOT" || -L "$LEGACY_ROOT" ]]; then
  [[ "$(realpath "$LEGACY_ROOT")" == "$REPO_ROOT" ]] || fail "$LEGACY_ROOT points elsewhere"
else
  if [[ -d /workspaces && -w /workspaces ]]; then ln -s "$REPO_ROOT" "$LEGACY_ROOT"; else sudo mkdir -p /workspaces; sudo ln -s "$REPO_ROOT" "$LEGACY_ROOT"; fi
fi

# The TMP 3.0.6 API declares ForceMeshUpdate(bool ignoreActiveState=false,
# bool forceTextReparsing=false). The public source spells ForceMeshUpdate(), so
# Roslyn supplies two false optional arguments. Apply that source-equivalent
# binding deterministically before compiling the takeover patcher.
python3 - "$PROGRAM" <<'PY'
from pathlib import Path
import sys
p=Path(sys.argv[1]); s=p.read_text()
a='var forceMesh = RM(mod, "TMP_Text", "ForceMeshUpdate", 0);'
b='var forceMesh = RM(mod, "TMP_Text", "ForceMeshUpdate", 2);'
c='il.Emit(OpCodes.Ldloc, self); il.Emit(OpCodes.Ldfld, fText); il.Emit(OpCodes.Callvirt, forceMesh);'
d='il.Emit(OpCodes.Ldloc, self); il.Emit(OpCodes.Ldfld, fText); LdcI4(il,0); LdcI4(il,0); il.Emit(OpCodes.Callvirt, forceMesh);'
if a not in s: raise SystemExit('ForceMeshUpdate lookup patch site missing')
if c not in s: raise SystemExit('ForceMeshUpdate call patch site missing')
s=s.replace(a,b,1).replace(c,d,1)
p.write_text(s)
print('FORCE_MESH_OPTIONAL_ARGS_BIND_PASS')
PY

mkdir -p "$(dirname "$NATIVE8_OUT")"
A="${NATIVE8_OUT}.run1"; B="${NATIVE8_OUT}.run2"
rm -f "$A" "$B" "$NATIVE8_OUT"
dotnet run --project "$PATCHER" -- "$NATIVE7_SOURCE" "$A" | tee "${NATIVE8_OUT}.run1.log"
dotnet run --project "$PATCHER" -- "$NATIVE7_SOURCE" "$B" | tee "${NATIVE8_OUT}.run2.log"
SA="$(sha "$A")"; SB="$(sha "$B")"
[[ "$SA" == "$SB" ]] || fail "non-deterministic SHA: $SA != $SB"
cmp -s "$A" "$B" || fail "non-deterministic bytes"
mv "$A" "$NATIVE8_OUT"; rm -f "$B"

printf '%s\n' \
  "source_native7_sha256=$(sha "$NATIVE7_SOURCE")" \
  "native8_sha256=$SA" \
  "effective_patcher_program_sha256=$(sha "$PROGRAM")" \
  "patcher_project_sha256=$(sha "$PATCHER")" \
  "materializer_sha256=$(sha "$REPO_ROOT/scripts/takeover/materialize_native8.sh")" \
  "force_mesh_optional_args_binding=PASS" \
  "deterministic_run_pair=PASS" \
  "semantic_scope=TMPro.Examples.SkewTextExample/<WarpText>d__7::MoveNext" \
  "pc_native_authority=0x1803C2450-0x1803C3085" \
  "production_promotion=NO"
echo NATIVE8_MATERIALIZATION_PASS
