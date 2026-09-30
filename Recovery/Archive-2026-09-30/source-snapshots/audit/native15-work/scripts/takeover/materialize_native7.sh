#!/usr/bin/env bash
set -euo pipefail

REPO_ROOT="$(cd "$(dirname "${BASH_SOURCE[0]}")/../.." && pwd)"
NATIVE4_SOURCE="${1:-$REPO_ROOT/Tools/Stage9Native4Recovery/candidate/Assembly-CSharp-native4-six-method.dll}"
NATIVE7_OUT="${2:-$REPO_ROOT/Tools/Stage9Native4Recovery/candidate/Assembly-CSharp-native7-ladder-closure.dll}"
EXPECTED_NATIVE4_SHA256="11c1a9648ea67cb0ac66b84ddde50cb4ef46d24152f67515a99afb526a2d4b4e"
LEGACY_ROOT="/workspaces/GodsPVZ-iOS"
CECIL="$REPO_ROOT/Tools/Stage9Native4Recovery/tools/lib/netstandard2.0/Mono.Cecil.dll"
PATCHER="$REPO_ROOT/scripts/takeover/PatcherNative7/PatcherNative7.csproj"

fail() { echo "[native7-materialize] ERROR: $*" >&2; exit 1; }
sha256_file() { sha256sum "$1" | awk '{print $1}'; }

[[ -f "$NATIVE4_SOURCE" ]] || fail "native4 input missing: $NATIVE4_SOURCE"
[[ "$(sha256_file "$NATIVE4_SOURCE")" == "$EXPECTED_NATIVE4_SHA256" ]] || fail "native4 SHA mismatch"
[[ -f "$CECIL" ]] || fail "Mono.Cecil missing; run prepare_ci_inputs.py"
command -v dotnet >/dev/null 2>&1 || fail "dotnet missing"

if [[ -e "$LEGACY_ROOT" || -L "$LEGACY_ROOT" ]]; then
  [[ "$(realpath "$LEGACY_ROOT")" == "$REPO_ROOT" ]] || fail "$LEGACY_ROOT points elsewhere"
else
  if [[ -d /workspaces && -w /workspaces ]]; then
    ln -s "$REPO_ROOT" "$LEGACY_ROOT"
  else
    sudo mkdir -p /workspaces
    sudo ln -s "$REPO_ROOT" "$LEGACY_ROOT"
  fi
fi

mkdir -p "$(dirname "$NATIVE7_OUT")"
TMP1="${NATIVE7_OUT}.run1"
TMP2="${NATIVE7_OUT}.run2"
rm -f "$TMP1" "$TMP2" "$NATIVE7_OUT"

dotnet run --project "$PATCHER" -- "$NATIVE4_SOURCE" "$TMP1" | tee "${NATIVE7_OUT}.run1.log"
dotnet run --project "$PATCHER" -- "$NATIVE4_SOURCE" "$TMP2" | tee "${NATIVE7_OUT}.run2.log"

SHA1="$(sha256_file "$TMP1")"
SHA2="$(sha256_file "$TMP2")"
[[ "$SHA1" == "$SHA2" ]] || fail "non-deterministic output: $SHA1 != $SHA2"
cmp -s "$TMP1" "$TMP2" || fail "byte comparison failed despite equal SHA"

mv "$TMP1" "$NATIVE7_OUT"
rm -f "$TMP2"

printf '%s\n' \
  "source_native4_sha256=$(sha256_file "$NATIVE4_SOURCE")" \
  "native7_sha256=$SHA1" \
  "patcher_program_sha256=$(sha256_file "$REPO_ROOT/scripts/takeover/PatcherNative7/Program.cs")" \
  "patcher_project_sha256=$(sha256_file "$PATCHER")" \
  "deterministic_run_pair=PASS" \
  "semantic_scope=TextLink.ResetLink,TextLink.SetLink,Zombie.ZC_LadderPlaceEnd,Zombie.ZC_LadderTestPlace" \
  "skewtext_blocker=UNFIXED" \
  "production_promotion=NO"

echo "NATIVE7_MATERIALIZATION_PASS"
