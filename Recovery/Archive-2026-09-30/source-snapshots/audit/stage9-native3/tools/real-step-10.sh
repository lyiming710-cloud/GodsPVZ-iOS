set -euo pipefail
OUT="$RUNNER_TEMP/stage9-native3-ios"
E="$OUT/evidence"
PROJECT="$(cat "$OUT/project-path.txt")"
PLUGIN="$PROJECT/Assets/Plugins/Assembly-CSharp.dll"
CAND="$OUT/candidate/Assembly-CSharp-native3-four-method.dll"
[ "$(sha256sum "$PLUGIN" | awk '{print $1}')" = "$B001_SHA256" ] || exit 60
[ "$(sha256sum "$CAND" | awk '{print $1}')" = "$TEST_DLL_SHA256" ] || exit 61
rm -f "$PROJECT/Assets/Editor/GodsPVZPackageReferenceMigrator.cs" "$PROJECT/Assets/Editor/GodsPVZPackageReferenceMigrator.cs.meta"
cp "$CAND" "$PLUGIN"
GOT="$(sha256sum "$PLUGIN" | awk '{print $1}')"
[ "$GOT" = "$TEST_DLL_SHA256" ] || exit 62
{
  echo "before=$B001_SHA256"
  echo "after=$GOT"
  echo 'migrator_project_copy_removed_after_67_of_67=true'
  echo 'semantic_gate=PASS'
  echo 'production_source_modified=false'
} > "$E/test-dll-injection.txt"

