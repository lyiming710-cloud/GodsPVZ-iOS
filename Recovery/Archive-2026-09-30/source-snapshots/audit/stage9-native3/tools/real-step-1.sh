set -euo pipefail
OUT="$RUNNER_TEMP/stage9-native3-ios"
mkdir -p "$OUT/evidence" "$OUT/candidate" "$OUT/semantic-gate"
if [ -z "${UNITY_EMAIL:-}" ] || [ -z "${UNITY_PASSWORD:-}" ]; then
  echo 'UNITY_EMAIL or UNITY_PASSWORD secret missing' >&2
  exit 41
fi
cat > "$OUT/evidence/input-locks.txt" <<EOF
workflow_head=$GITHUB_SHA
r3_sha256=$R3_SHA256
r3_archive_run=$R3_ARCHIVE_RUN_ID
r3_archive_artifact_id=$R3_ARCHIVE_ARTIFACT_ID
r3_archive_artifact_zip_sha256=$R3_ARCHIVE_ARTIFACT_ZIP_SHA256
base_runtime_sha256=$BASE_RUNTIME_SHA256
base_runtime_run=$BASE_RUNTIME_RUN_ID
b001_sha256=$B001_SHA256
test_dll_sha256=$TEST_DLL_SHA256
test_candidate_run=$TEST_CANDIDATE_RUN_ID
test_candidate_artifact_id=$TEST_CANDIDATE_ARTIFACT_ID
test_candidate_artifact_sha256=$TEST_CANDIDATE_ARTIFACT_SHA256
editor_run=$EDITOR_RUN_ID
editor_sha256=$EDITOR_SHA256
ios_module_run=$IOS_MODULE_RUN_ID
ios_module_sha256=$IOS_MODULE_SHA256
unity_version=$UNITY_VERSION
unity_changeset=$UNITY_CHANGESET
production_promotion=NO_ISOLATED_GATE_ONLY
EOF

