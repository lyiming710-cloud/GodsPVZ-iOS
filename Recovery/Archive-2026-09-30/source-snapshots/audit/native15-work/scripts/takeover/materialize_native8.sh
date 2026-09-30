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
FALLBACKS="$REPO_ROOT/scripts/takeover/PatcherNative8/ReferenceFallbacks.cs"

fail(){ echo "[native8-materialize] ERROR: $*" >&2; exit 1; }
sha(){ sha256sum "$1" | awk '{print $1}'; }

[[ -f "$NATIVE7_SOURCE" ]] || fail "native7 input missing: $NATIVE7_SOURCE"
[[ "$(sha "$NATIVE7_SOURCE")" == "$EXPECTED_NATIVE7_SHA256" ]] || fail "native7 SHA mismatch"
[[ -f "$CECIL" ]] || fail "Mono.Cecil missing"
[[ -f "$FALLBACKS" ]] || fail "ReferenceFallbacks.cs missing"
command -v dotnet >/dev/null 2>&1 || fail "dotnet missing"

if [[ -e "$LEGACY_ROOT" || -L "$LEGACY_ROOT" ]]; then
  [[ "$(realpath "$LEGACY_ROOT")" == "$REPO_ROOT" ]] || fail "$LEGACY_ROOT points elsewhere"
else
  if [[ -d /workspaces && -w /workspaces ]]; then ln -s "$REPO_ROOT" "$LEGACY_ROOT"; else sudo mkdir -p /workspaces; sudo ln -s "$REPO_ROOT" "$LEGACY_ROOT"; fi
fi

# Normalize two source-level facts before compiling the experimental patcher:
#  1) ForceMeshUpdate() binds to two optional bool parameters (false,false).
#  2) Cpp2IL corruption can remove MemberRef rows that the original source needs.
#     When an exact existing MemberRef is unavailable, synthesize its known TMP/Unity
#     signature through the audited ReferenceFallbacks map instead of guessing in IL.
python3 - "$PROGRAM" <<'PY'
from pathlib import Path
import sys
p=Path(sys.argv[1]); s=p.read_text()

def once(old,new,label):
    global s
    if old not in s: raise SystemExit(label+' patch site missing')
    s=s.replace(old,new,1)

once('var forceMesh = RM(mod, "TMP_Text", "ForceMeshUpdate", 0);',
     'var forceMesh = RM(mod, "TMP_Text", "ForceMeshUpdate", 2);',
     'ForceMesh lookup')
once('il.Emit(OpCodes.Ldloc, self); il.Emit(OpCodes.Ldfld, fText); il.Emit(OpCodes.Callvirt, forceMesh);',
     'il.Emit(OpCodes.Ldloc, self); il.Emit(OpCodes.Ldfld, fText); LdcI4(il,0); LdcI4(il,0); il.Emit(OpCodes.Callvirt, forceMesh);',
     'ForceMesh call')
once('''    static FieldReference RF(ModuleDefinition m, string declaringName, string name) =>\n        m.GetMemberReferences().OfType<FieldReference>().First(f => f.DeclaringType.Name == declaringName && f.Name == name);''',
     '''    static FieldReference RF(ModuleDefinition m, string declaringName, string name) =>\n        m.GetMemberReferences().OfType<FieldReference>().FirstOrDefault(f => f.DeclaringType.Name == declaringName && f.Name == name)\n        ?? ReferenceFallbacks.Field(m, declaringName, name);''',
     'RF fallback')
once('''    static MethodReference RM(ModuleDefinition m, string declaringName, string name, int argc) =>\n        m.GetMemberReferences().OfType<MethodReference>().First(x => x.DeclaringType.Name == declaringName && x.Name == name && x.Parameters.Count == argc);''',
     '''    static MethodReference RM(ModuleDefinition m, string declaringName, string name, int argc) =>\n        m.GetMemberReferences().OfType<MethodReference>().FirstOrDefault(x => x.DeclaringType.Name == declaringName && x.Name == name && x.Parameters.Count == argc)\n        ?? ReferenceFallbacks.Method(m, declaringName, name, argc);''',
     'RM fallback')
p.write_text(s)
print('SOURCE_BINDING_NORMALIZATION_PASS')
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
  "reference_fallbacks_sha256=$(sha "$FALLBACKS")" \
  "patcher_project_sha256=$(sha "$PATCHER")" \
  "materializer_sha256=$(sha "$REPO_ROOT/scripts/takeover/materialize_native8.sh")" \
  "force_mesh_optional_args_binding=PASS" \
  "decompiler_lost_memberref_fallback=ENABLED_AUDITED_SIGNATURES_ONLY" \
  "deterministic_run_pair=PASS" \
  "semantic_scope=TMPro.Examples.SkewTextExample/<WarpText>d__7::MoveNext" \
  "pc_native_authority=0x1803C2450-0x1803C3085" \
  "production_promotion=NO"
echo NATIVE8_MATERIALIZATION_PASS
