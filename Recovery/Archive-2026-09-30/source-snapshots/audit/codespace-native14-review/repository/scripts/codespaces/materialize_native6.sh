#!/usr/bin/env bash
# Deterministically materialize the experimental native6 candidate from the
# formally qualified native4 DLL. This script changes plumbing only: all CIL
# patch semantics remain in scripts/codespaces/Patcher/Program.cs.
set -euo pipefail

REPO_ROOT="$(cd "$(dirname "${BASH_SOURCE[0]}")/../.." && pwd)"
NATIVE4_SOURCE="${1:-$REPO_ROOT/Tools/Stage9Native4Recovery/candidate/Assembly-CSharp-native4-six-method.dll}"
NATIVE6_OUT="${2:-$REPO_ROOT/Tools/Stage9Native4Recovery/candidate/Assembly-CSharp-native6-ladder.dll}"
EXPECTED_NATIVE4_SHA256="11c1a9648ea67cb0ac66b84ddde50cb4ef46d24152f67515a99afb526a2d4b4e"
EXPECTED_NATIVE6_SHA256="70a45b9add1e402ba8aeea5c1bf315cf62a0c7d0f39481528c88b0cc4c0688ee"
LEGACY_ROOT="/workspaces/GodsPVZ-iOS"
LOCAL_NATIVE4="$REPO_ROOT/Tools/Stage9Native4Recovery/candidate/Assembly-CSharp-native4-six-method.dll"
GENERATED_NATIVE6="$REPO_ROOT/Tools/Stage9Native4Recovery/candidate/Assembly-CSharp-native6-ladder.dll"

fail() { echo "[native6-materialize] ERROR: $*" >&2; exit 1; }
sha256_file() { sha256sum "$1" | awk '{print $1}'; }

[[ -f "$NATIVE4_SOURCE" ]] || fail "native4 source missing: $NATIVE4_SOURCE"
N4_SHA="$(sha256_file "$NATIVE4_SOURCE")"
[[ "$N4_SHA" == "$EXPECTED_NATIVE4_SHA256" ]] || fail "native4 SHA mismatch: $N4_SHA"
[[ -f "$REPO_ROOT/Tools/Stage9Native4Recovery/tools/lib/netstandard2.0/Mono.Cecil.dll" ]] || fail "Mono.Cecil input missing; run prepare_ci_inputs.py first"
command -v dotnet >/dev/null 2>&1 || fail "dotnet not found"

mkdir -p "$(dirname "$LOCAL_NATIVE4")" "$(dirname "$NATIVE6_OUT")"
if [[ "$(realpath "$NATIVE4_SOURCE")" != "$(realpath -m "$LOCAL_NATIVE4")" ]]; then
  cp "$NATIVE4_SOURCE" "$LOCAL_NATIVE4"
fi
[[ "$(sha256_file "$LOCAL_NATIVE4")" == "$EXPECTED_NATIVE4_SHA256" ]] || fail "local native4 copy changed"

# The experimental patcher predates Actions support and has absolute
# /workspaces/GodsPVZ-iOS paths. Preserve its source verbatim and provide a
# deterministic compatibility alias rather than silently changing CIL logic.
if [[ -e "$LEGACY_ROOT" || -L "$LEGACY_ROOT" ]]; then
  [[ "$(realpath "$LEGACY_ROOT")" == "$REPO_ROOT" ]] || fail "$LEGACY_ROOT already points elsewhere"
else
  if [[ -d /workspaces && -w /workspaces ]]; then
    ln -s "$REPO_ROOT" "$LEGACY_ROOT"
  else
    sudo mkdir -p /workspaces
    sudo ln -s "$REPO_ROOT" "$LEGACY_ROOT"
  fi
fi

PATCHER="$REPO_ROOT/scripts/codespaces/Patcher/Patcher.csproj"
dotnet run --project "$PATCHER" -- "$LOCAL_NATIVE4"
[[ -f "$GENERATED_NATIVE6" ]] || fail "patcher did not create native6 candidate"
N6_SHA="$(sha256_file "$GENERATED_NATIVE6")"
[[ "$N6_SHA" == "$EXPECTED_NATIVE6_SHA256" ]] || fail "native6 SHA mismatch: $N6_SHA"

if [[ "$(realpath "$GENERATED_NATIVE6")" != "$(realpath -m "$NATIVE6_OUT")" ]]; then
  cp "$GENERATED_NATIVE6" "$NATIVE6_OUT"
fi
[[ "$(sha256_file "$NATIVE6_OUT")" == "$EXPECTED_NATIVE6_SHA256" ]] || fail "output copy changed"

printf '%s\n' \
  "native4_sha256=$N4_SHA" \
  "native6_sha256=$N6_SHA" \
  "patcher_program_sha256=$(sha256_file "$REPO_ROOT/scripts/codespaces/Patcher/Program.cs")" \
  "patcher_project_sha256=$(sha256_file "$PATCHER")"
echo "NATIVE6_MATERIALIZATION_PASS"
