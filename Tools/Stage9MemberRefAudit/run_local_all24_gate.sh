#!/usr/bin/env bash
set -euo pipefail

usage() {
  cat >&2 <<'EOF'
Usage:
  run_local_all24_gate.sh INPUT_DLL EDITOR_PART_SOURCE OUT_DIR

INPUT_DLL may be any hash recognized by build_all24_candidate.py:
  run-8 pre-B001, B001-only, GetEnumerator-family, or already-all24.

EDITOR_PART_SOURCE may contain either raw preserved part files or GitHub artifact
ZIPs that contain files named Unity-China-2022.3.44f1c1.tar.xz.part-NN.

Requirements: python3, unzip (only when ZIP artifacts are supplied), dotnet 8.
This script does not invoke GitHub Actions and does not modify the input DLL.
EOF
  exit 2
}

[ "$#" -eq 3 ] || usage

SCRIPT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
INPUT_DLL="$(realpath "$1")"
PART_SOURCE="$(realpath "$2")"
OUT_DIR="$(mkdir -p "$3" && cd "$3" && pwd)"
PART_DIR="$OUT_DIR/editor-parts"
RESOLVER_DIR="$OUT_DIR/editor-managed-resolver"
CANDIDATE="$OUT_DIR/Assembly-CSharp-all24-candidate.dll"
MANIFEST="$OUT_DIR/all24-builder-manifest.json"
AUDIT_LOG="$OUT_DIR/memberref-strict-resolve.log"
SUMMARY="$OUT_DIR/GATE-SUMMARY.txt"
EXPECTED_ALL24='b07254d5c4077e013bbc934734f7b0949c0942a4e667d6d2b23d2af6c830f720'

mkdir -p "$PART_DIR" "$RESOLVER_DIR"

sha256_file() { sha256sum "$1" | awk '{print $1}'; }

printf 'stage=prepare\ninput=%s\ninput_sha256=%s\n' \
  "$INPUT_DLL" "$(sha256_file "$INPUT_DLL")" > "$SUMMARY"

# Build the exact locked all-24 candidate. The builder rejects unknown inputs,
# validates metadata-table structure/canonical blobs/transform operands, and
# refuses output unless the final SHA is the locked b072... value.
python3 "$SCRIPT_DIR/build_all24_candidate.py" \
  "$INPUT_DLL" "$CANDIDATE" --manifest "$MANIFEST" \
  > "$OUT_DIR/all24-builder.stdout.json"

ACTUAL_ALL24="$(sha256_file "$CANDIDATE")"
if [ "$ACTUAL_ALL24" != "$EXPECTED_ALL24" ]; then
  echo "candidate hash mismatch: $ACTUAL_ALL24" >&2
  exit 31
fi
printf 'candidate_sha256=%s\n' "$ACTUAL_ALL24" >> "$SUMMARY"

# Normalize raw editor parts from either already-extracted files or downloaded
# GitHub artifact ZIPs. Never overwrite a conflicting part silently.
shopt -s nullglob
for raw in "$PART_SOURCE"/Unity-China-2022.3.44f1c1.tar.xz.part-*; do
  base="$(basename "$raw")"
  if [ -e "$PART_DIR/$base" ]; then
    [ "$(sha256_file "$raw")" = "$(sha256_file "$PART_DIR/$base")" ] || {
      echo "conflicting raw part: $base" >&2; exit 32;
    }
  else
    cp "$raw" "$PART_DIR/$base"
  fi
done

zips=("$PART_SOURCE"/*.zip)
if [ "${#zips[@]}" -gt 0 ]; then
  command -v unzip >/dev/null 2>&1 || { echo 'unzip is required for editor artifact ZIPs' >&2; exit 33; }
  for z in "${zips[@]}"; do
    tmp="$(mktemp -d)"
    unzip -qq "$z" 'Unity-China-2022.3.44f1c1.tar.xz.part-*' -d "$tmp" || true
    while IFS= read -r -d '' raw; do
      base="$(basename "$raw")"
      if [ -e "$PART_DIR/$base" ]; then
        [ "$(sha256_file "$raw")" = "$(sha256_file "$PART_DIR/$base")" ] || {
          echo "conflicting ZIP part: $base from $z" >&2; rm -rf "$tmp"; exit 34;
        }
      else
        cp "$raw" "$PART_DIR/$base"
      fi
    done < <(find "$tmp" -type f -name 'Unity-China-2022.3.44f1c1.tar.xz.part-*' -print0)
    rm -rf "$tmp"
  done
fi

PART_COUNT="$(find "$PART_DIR" -maxdepth 1 -type f -name 'Unity-China-2022.3.44f1c1.tar.xz.part-*' | wc -l | tr -d ' ')"
printf 'editor_part_count=%s\n' "$PART_COUNT" >> "$SUMMARY"
[ "$PART_COUNT" -gt 0 ] || { echo 'no editor parts available after normalization' >&2; exit 35; }

# Exit code 3 means the available contiguous prefix is valid but does not reach
# both exact Editor-managed resolver files yet. Preserve its report and stop;
# caller can add the next consecutive part(s) and rerun.
set +e
python3 "$SCRIPT_DIR/extract_editor_managed_from_prefix.py" "$PART_DIR" "$RESOLVER_DIR" \
  > "$OUT_DIR/editor-prefix-extractor.log" 2>&1
EXTRACT_RC=$?
set -e
if [ "$EXTRACT_RC" -eq 3 ]; then
  echo 'resolver_status=NEED_MORE_PREFIX_PARTS' >> "$SUMMARY"
  cat "$OUT_DIR/editor-prefix-extractor.log" >&2
  exit 36
fi
[ "$EXTRACT_RC" -eq 0 ] || { cat "$OUT_DIR/editor-prefix-extractor.log" >&2; exit "$EXTRACT_RC"; }
echo 'resolver_status=EXACT_EDITOR_TARGETS_FOUND' >> "$SUMMARY"

for required in mscorlib.dll UnityEngine.CoreModule.dll; do
  test -f "$RESOLVER_DIR/$required" || { echo "missing selected resolver file: $required" >&2; exit 37; }
  printf '%s_sha256=%s\n' "${required%.dll}" "$(sha256_file "$RESOLVER_DIR/$required")" >> "$SUMMARY"
done

command -v dotnet >/dev/null 2>&1 || {
  echo 'dotnet 8 is required for the independent Mono.Cecil audit' >&2
  echo 'audit_status=BLOCKED_DOTNET_MISSING' >> "$SUMMARY"
  exit 38
}
DOTNET_MAJOR="$(dotnet --version | cut -d. -f1)"
[ "$DOTNET_MAJOR" = '8' ] || { echo "dotnet 8 required, got $(dotnet --version)" >&2; exit 39; }
printf 'dotnet_version=%s\n' "$(dotnet --version)" >> "$SUMMARY"

AUDIT_PROJECT="$SCRIPT_DIR/Stage9MemberRefAudit.csproj"
export NUGET_PACKAGES="$OUT_DIR/nuget-packages"
dotnet restore "$AUDIT_PROJECT" --packages "$NUGET_PACKAGES" \
  > "$OUT_DIR/dotnet-restore.log"

dotnet run --project "$AUDIT_PROJECT" --no-restore -- \
  "$CANDIDATE" --require-resolve "$RESOLVER_DIR" \
  | tee "$AUDIT_LOG"

grep -q '^STAGE9_MEMBERREF_AUDIT_OK$' "$AUDIT_LOG" || {
  echo 'strict audit did not emit STAGE9_MEMBERREF_AUDIT_OK' >&2
  exit 40
}
grep -q '^ORPHAN_GENERIC_HITS=0$' "$AUDIT_LOG" || {
  echo 'ownerless generic gate failed' >&2
  exit 41
}
grep -q '^RESOLVE_SUMMARY ok=24 dependency_missing=0 hard_fail=0 require_resolve=True$' "$AUDIT_LOG" || {
  echo '24/24 strict Resolve summary gate failed' >&2
  exit 42
}

cat >> "$SUMMARY" <<EOF
methoddef_expected=2317
orphan_generic_expected=0
resolve_expected=24/24
audit_status=PASS
production_promotion=NOT_AUTOMATIC_REAL_UNITY_IL2CPP_GATE_STILL_REQUIRED
EOF

printf '\nSTAGE9_ALL24_LOCAL_GATE_PASS\nsummary=%s\n' "$SUMMARY"
