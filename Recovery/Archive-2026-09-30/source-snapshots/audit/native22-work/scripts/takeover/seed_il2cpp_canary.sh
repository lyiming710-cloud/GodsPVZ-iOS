#!/usr/bin/env bash
set -euo pipefail

REPO_ROOT="${GITHUB_WORKSPACE:-$(cd "$(dirname "${BASH_SOURCE[0]}")/../.." && pwd)}"
OUT="${GATE_WORK_DIR:-${RUNNER_TEMP:-/tmp}/godspvz-native7-canary-seed}"
E="$OUT/evidence"
PROJECT_ROOT="$OUT/r3"
PROJECT="$PROJECT_ROOT/UnityProject-AssetRipper-2.0.0/ExportedProject"
EDITOR_ROOT="$OUT/editor-root"
UNITY="$EDITOR_ROOT/Editor/Unity"
CANARY="$OUT/canary"
FETCH="$REPO_ROOT/scripts/takeover/fetch_actions_artifact.py"
mkdir -p "$E" "$OUT/downloads" "$PROJECT_ROOT" "$EDITOR_ROOT"

# Immutable authorities.
R3_ARTIFACT_ID=10314333890
R3_ARTIFACT_ZIP_SHA256=1c2183569b5d2273cdbf209992223d9f6a5d3cc0090bdbb8f68c8bf4ad448a78
R3_SHA256=d4264f12a00e86b6149d20ecf04caa9761af510aece6107bcceb155ea4ab0bbc
R3_ORIGINAL_PLUGIN_SHA256=dc205a40dc2478b3aacbb3a7d6bb1ca96ffb0a964648d34062b4ddc75f3b3655
BASE_RUNTIME_ARTIFACT_ID=10502973448
BASE_RUNTIME_ZIP_SHA256=c15e57bb6d92d9d94039f05bfeae28de9192afdbae1159be3b639cfa1057a24f
BASE_RUNTIME_SHA256=047054e0db594b6e3385fe4d2555c5932dcd4c28e4b2cb6fb39acbc4336d43ee
PACKAGE_GUID_ARTIFACT_ID=10505616227
PACKAGE_GUID_ZIP_SHA256=77c956492fe4690f9e3369979738ac745e7a283d2d7cd9b234abc7ac5baf8820
PACKAGE_BUNDLE_ARTIFACT_ID=10505237349
PACKAGE_BUNDLE_ZIP_SHA256=e94aaa2c0a85832ef9e0263dbd925d9d3f8a71803e199867f876ae223ec440c0
PACKAGE_CLOSURE_ARTIFACT_ID=10505060075
PACKAGE_CLOSURE_ZIP_SHA256=e7a949235772cab30c9465071f91daf76d9313ebe05812aae7c632b470ebdeee
EDITOR_RUN_ID=34668863583
EDITOR_PREFIX=Stage9.1-unity-china-editor-part-
EDITOR_SHA256=0008115c785784baddb19e2b13b384ac845438239f293efb0c2e8b1379a1fe14
IOS_MODULE_ARTIFACT_ID=10544905103
IOS_MODULE_ARTIFACT_ZIP_SHA256=c00dd0303cec8acadfd126719103c7f552008fa30a61794acfee10d747ccadbd
IOS_MODULE_SHA256=2c2e259415ab0f940889b6b5bf55c56bc2710bbd823ddc05c5ab67ebc137a16e
B001_SHA256=f144dd624d2b0c05b5ff01fc12ff455ac394c87852a4fc3c87e8c3611582a433
NATIVE7_ARTIFACT_ID=10922591252
NATIVE7_ARTIFACT_ZIP_SHA256=f0f712160e1606a49eb08662da05f65c413278cb936df30193d3d56879957320
NATIVE7_SHA256=b9dadd905244ccfddfa86f87c2edc1e60d883441632b5bb359ffccab4d2faa29
UNITY_VERSION=2022.3.44f1c1
SEAT=0

sha() { sha256sum "$1" | awk '{print $1}'; }
die() { echo "[native7-canary-seed] ERROR: $*" >&2; exit 1; }
fetch() {
  python3 "$FETCH" --artifact-id "$1" --dest "$2" --zip-sha256 "$3"
}
find_sha() {
  python3 - "$1" "$2" <<'PY'
import hashlib,sys
from pathlib import Path
root=Path(sys.argv[1]); want=sys.argv[2]
h=[]
for p in root.rglob('*'):
    if p.is_file():
        x=hashlib.sha256(p.read_bytes()).hexdigest()
        if x==want: h.append(p)
if len(h)!=1: raise SystemExit(f'expected one SHA hit {want}, got {len(h)}: {h}')
print(h[0])
PY
}
return_seat() {
  if [[ "$SEAT" = 1 && -x "$UNITY" ]]; then
    local client="$EDITOR_ROOT/Editor/Data/Resources/Licensing/Client/Unity.Licensing.Client"
    set +e
    "$client" --return-ulf > "$E/license-return.log" 2>&1
    echo "$?" > "$E/license-return-exit.txt"
    set -e
  fi
}
trap return_seat EXIT

[[ -n "${GH_TOKEN:-${GITHUB_TOKEN:-}}" ]] || die "GitHub token missing"
[[ -n "${UNITY_EMAIL:-}" && -n "${UNITY_PASSWORD:-}" ]] || die "Unity credentials missing"

cat > "$E/LOCKS.txt" <<EOF
stage=native7_il2cpp_canary_seed
formal_native4_sha256=11c1a9648ea67cb0ac66b84ddde50cb4ef46d24152f67515a99afb526a2d4b4e
native7_sha256=$NATIVE7_SHA256
r3_sha256=$R3_SHA256
base_runtime_sha256=$BASE_RUNTIME_SHA256
b001_sha256=$B001_SHA256
editor_sha256=$EDITOR_SHA256
ios_module_sha256=$IOS_MODULE_SHA256
unity_version=$UNITY_VERSION
production_promotion=NO
EOF

# Exact native7 candidate.
fetch "$NATIVE7_ARTIFACT_ID" "$OUT/native7-artifact" "$NATIVE7_ARTIFACT_ZIP_SHA256"
CAND="$(find_sha "$OUT/native7-artifact" "$NATIVE7_SHA256")"
echo "native7_candidate=$CAND" > "$E/native7-candidate.txt"

# Exact R3 project.
fetch "$R3_ARTIFACT_ID" "$OUT/r3-artifact" "$R3_ARTIFACT_ZIP_SHA256"
mapfile -t R3S < <(find "$OUT/r3-artifact" -type f -name '*.tar.zst' | sort)
[[ ${#R3S[@]} -eq 1 ]] || die "expected one R3 tar.zst"
[[ "$(sha "${R3S[0]}")" = "$R3_SHA256" ]] || die "R3 inner SHA mismatch"
zstd -t "${R3S[0]}" 2> "$E/r3-zstd-test.txt"
zstd -dc "${R3S[0]}" | tar -xf - -C "$PROJECT_ROOT"
[[ -d "$PROJECT" ]] || die "R3 ExportedProject missing"
[[ "$(sha "$PROJECT/Assets/Plugins/Assembly-CSharp.dll")" = "$R3_ORIGINAL_PLUGIN_SHA256" ]] || die "R3 original plugin SHA mismatch"
rm -rf "$OUT/r3-artifact"

# Exact baseline runtime and package closure.
fetch "$BASE_RUNTIME_ARTIFACT_ID" "$OUT/base-runtime" "$BASE_RUNTIME_ZIP_SHA256"
BASE_DLL="$(find_sha "$OUT/base-runtime" "$BASE_RUNTIME_SHA256")"
cp "$BASE_DLL" "$PROJECT/Assets/Plugins/Assembly-CSharp.dll"
fetch "$PACKAGE_GUID_ARTIFACT_ID" "$OUT/package-guid" "$PACKAGE_GUID_ZIP_SHA256"
fetch "$PACKAGE_BUNDLE_ARTIFACT_ID" "$OUT/package-bundle" "$PACKAGE_BUNDLE_ZIP_SHA256"
fetch "$PACKAGE_CLOSURE_ARTIFACT_ID" "$OUT/package-closure" "$PACKAGE_CLOSURE_ZIP_SHA256"
mkdir -p "$PROJECT/Assets/Editor" "$PROJECT/Recovery" "$PROJECT/Packages"
cp "$REPO_ROOT/Assets/Editor/GodsPVZPackageReferenceMigrator.cs" "$PROJECT/Assets/Editor/"
cp "$REPO_ROOT/Assets/Editor/GodsPVZIOSBuild.cs" "$PROJECT/Assets/Editor/"
cp "$REPO_ROOT/Recovery/package-script-map-editor.json" "$PROJECT/Recovery/"
python3 - "$OUT/package-bundle/Packages" "$OUT/package-closure/Packages" "$PROJECT/Packages" <<'PY'
import shutil,sys
from pathlib import Path
dst=Path(sys.argv[3]); dst.mkdir(parents=True,exist_ok=True)
for root in map(Path,sys.argv[1:3]):
    if not root.is_dir(): raise SystemExit('missing package source '+str(root))
    for src in sorted(p for p in root.iterdir() if p.is_dir()):
        target=dst/src.name
        if target.exists(): shutil.rmtree(target)
        shutil.copytree(src,target)
PY
python3 - "$OUT/package-guid/package-guid-map.json" "$PROJECT/Packages" "$E/package-guid-preflight.txt" <<'PY'
import json,re,sys
from pathlib import Path
d=json.load(open(sys.argv[1])); root=Path(sys.argv[2]); out=Path(sys.argv[3])
assert d['resolved']==67 and d['expected']==67 and not d['failures']
bad=[]; n=0
for m in d['mapping']:
    meta=root/m['package']/m['meta_path']
    if not meta.is_file(): bad.append((m['fullName'],'missing')); continue
    mm=re.search(r'(?m)^guid:\s*([0-9a-fA-F]{32})\s*$',meta.read_text(errors='replace'))
    got=mm.group(1).lower() if mm else ''
    n+=1
    if got!=m['monoscript_guid'].lower(): bad.append((m['fullName'],got,m['monoscript_guid']))
out.write_text(f'resolved={n}/67\nfailures={len(bad)}\n{bad!r}\n')
if n!=67 or bad: raise SystemExit(1)
PY
rm -f "$PROJECT/Packages/packages-lock.json"
rm -rf "$OUT/base-runtime" "$OUT/package-guid" "$OUT/package-bundle" "$OUT/package-closure"

# Exact Unity China Editor + exact iOS module.
python3 "$FETCH" --run-id "$EDITOR_RUN_ID" --prefix "$EDITOR_PREFIX" --dest "$OUT/editor-parts"
mapfile -t EPARTS < <(find "$OUT/editor-parts" -maxdepth 1 -type f -name 'Unity-China-2022.3.44f1c1.tar.xz.part-*' | sort)
[[ ${#EPARTS[@]} -eq 15 ]] || die "expected 15 editor parts, got ${#EPARTS[@]}"
ESHA="$({ for p in "${EPARTS[@]}"; do cat "$p"; done; } | sha256sum | awk '{print $1}')"
[[ "$ESHA" = "$EDITOR_SHA256" ]] || die "Editor SHA mismatch: $ESHA"
{ for p in "${EPARTS[@]}"; do cat "$p"; done; } | tar -xJf - -C "$EDITOR_ROOT"
chmod +x "$UNITY"
[[ "$($UNITY -version 2>&1 | grep -Eo '2022\.3\.44f1c1|2022\.3\.44f1' | head -1)" = "$UNITY_VERSION" ]] || die "Unity version mismatch"
rm -rf "$OUT/editor-parts"

fetch "$IOS_MODULE_ARTIFACT_ID" "$OUT/ios-module" "$IOS_MODULE_ARTIFACT_ZIP_SHA256"
mapfile -t MODS < <(find "$OUT/ios-module" -type f -name 'UnitySetup-iOS-Support-for-Editor-2022.3.44f1.tar.xz' | sort)
[[ ${#MODS[@]} -eq 1 ]] || die "expected one iOS module tar"
[[ "$(sha "${MODS[0]}")" = "$IOS_MODULE_SHA256" ]] || die "iOS module SHA mismatch"
xz -t "${MODS[0]}"
tar -xJf "${MODS[0]}" -C "$EDITOR_ROOT"
rm -rf "$OUT/ios-module"

# License, baseline import, then the already-proven 67/67 migration.
CLIENT="$EDITOR_ROOT/Editor/Data/Resources/Licensing/Client/Unity.Licensing.Client"
"$CLIENT" --activate-all --include-personal --username "$UNITY_EMAIL" --password "$UNITY_PASSWORD" > "$E/license-activate.log" 2>&1
SEAT=1
"$UNITY" -batchmode -nographics -quit -projectPath "$PROJECT" -logFile "$E/import-before-migration.log"
! grep -Eiq '\berror CS[0-9]+\b|Scripts have compiler errors|Compilation failed' "$E/import-before-migration.log" || die "baseline import compile failure"
"$UNITY" -batchmode -nographics -quit -projectPath "$PROJECT" -executeMethod GodsPVZPackageReferenceMigrator.RunBatch -logFile "$E/package-migration.log"
MARKER="$PROJECT/Library/GodsPVZ.package-reference-migration.done"
[[ -f "$MARKER" ]] || die "migration marker missing"
grep -q '^resolved=67/67$' "$MARKER" || die "migration not 67/67"
PLUGIN="$PROJECT/Assets/Plugins/Assembly-CSharp.dll"
[[ "$(sha "$PLUGIN")" = "$B001_SHA256" ]] || die "B001 SHA mismatch"
echo "B001_MIGRATION_PASS sha256=$B001_SHA256" | tee "$E/b001.txt"

# Candidate-specific work begins here. Everything above is the expensive stable prefix.
rm -f "$PROJECT/Assets/Editor/GodsPVZPackageReferenceMigrator.cs" "$PROJECT/Assets/Editor/GodsPVZPackageReferenceMigrator.cs.meta"
cp "$CAND" "$PLUGIN"
[[ "$(sha "$PLUGIN")" = "$NATIVE7_SHA256" ]] || die "native7 inject SHA mismatch"
"$UNITY" -batchmode -nographics -quit -projectPath "$PROJECT" -logFile "$E/import-native7.log"
! grep -Eiq '\berror CS[0-9]+\b|Scripts have compiler errors|Compilation failed' "$E/import-native7.log" || die "native7 reimport compile failure"
[[ "$(sha "$PLUGIN")" = "$NATIVE7_SHA256" ]] || die "native7 mutated during reimport"

# Software GL check required by the original 8192 TMP atlas.
if ! command -v xvfb-run >/dev/null 2>&1 || ! command -v glxinfo >/dev/null 2>&1; then
  sudo apt-get update -qq
  sudo apt-get install -y --no-install-recommends xvfb mesa-utils libgl1-mesa-dri
fi
export LIBGL_ALWAYS_SOFTWARE=1
xvfb-run -a -s '-screen 0 1280x720x24' glxinfo -l > "$E/software-gl-limits.txt" 2>&1
MAXT="$(sed -n 's/.*GL_MAX_TEXTURE_SIZE[^0-9]*\([0-9][0-9]*\).*/\1/p' "$E/software-gl-limits.txt" | head -1)"
[[ -n "$MAXT" && "$MAXT" -ge 8192 ]] || die "software GL texture limit insufficient: ${MAXT:-missing}"

# One full native7 export is intentionally used to seed a reusable post-Linker checkpoint.
export GODSPVZ_IOS_BUILD_PATH="$OUT/xcode"
set +e
xvfb-run -a -s '-screen 0 1280x720x24' "$UNITY" -batchmode -quit -force-glcore -buildTarget iOS -projectPath "$PROJECT" -executeMethod GodsPVZIOSBuild.BuildIOS -logFile "$E/ios-export-native7.log" > "$E/ios-export-native7.stdout" 2> "$E/ios-export-native7.stderr"
EXPORT_RC=$?
set -e
echo "$EXPORT_RC" > "$E/ios-export-native7.exit"

# Capture the exact post-Linker input universe and response file that Unity gave IL2CPP.
MS="$PROJECT/Library/Bee/artifacts/iOS/ManagedStripped"
[[ -d "$MS" ]] || die "ManagedStripped checkpoint missing after export"
mapfile -t GAME_DLLS < <(find "$MS" -maxdepth 1 -type f -name 'GodsPVZRuntime1.dll' | sort)
[[ ${#GAME_DLLS[@]} -eq 1 ]] || die "expected one stripped GodsPVZRuntime1.dll"
RSP_REL="$(grep -Eo 'Library/Bee/artifacts/rsp/[0-9]+\.rsp' "$E/ios-export-native7.log" | tail -1 || true)"
[[ -n "$RSP_REL" && -f "$PROJECT/$RSP_REL" ]] || die "exact IL2CPP rsp missing"
RSP_BASE="$(basename "$RSP_REL")"
rm -rf "$CANARY"
mkdir -p "$CANARY/Library/Bee/artifacts/iOS" "$CANARY/Library/Bee/artifacts/rsp" "$CANARY/il2cpp-deploy" "$CANARY/evidence"
cp -a "$MS" "$CANARY/Library/Bee/artifacts/iOS/ManagedStripped"
cp "$PROJECT/$RSP_REL" "$CANARY/Library/Bee/artifacts/rsp/$RSP_BASE.original"
cp -a "$EDITOR_ROOT/Editor/Data/il2cpp/build/deploy/." "$CANARY/il2cpp-deploy/"
chmod +x "$CANARY/il2cpp-deploy/il2cpp" || true
python3 - "$PROJECT" "$PROJECT/$RSP_REL" "$CANARY/Library/Bee/artifacts/rsp/$RSP_BASE.template" <<'PY'
import sys
from pathlib import Path
root=str(Path(sys.argv[1]).resolve())
s=Path(sys.argv[2]).read_text()
if root not in s: raise SystemExit('project root not present in rsp; cannot normalize safely')
Path(sys.argv[3]).write_text(s.replace(root,'__PROJECT_ROOT__'))
PY

STRIPPED_SHA="$(sha "${GAME_DLLS[0]}")"
cat > "$CANARY/MANIFEST.txt" <<EOF
status=NATIVE7_POST_LINKER_CANARY_SEED
source_workflow_head=${GITHUB_SHA:-unknown}
source_native7_sha256=$NATIVE7_SHA256
stripped_godspvzruntime1_sha256=$STRIPPED_SHA
rsp_basename=$RSP_BASE
unity_version=$UNITY_VERSION
editor_sha256=$EDITOR_SHA256
ios_module_sha256=$IOS_MODULE_SHA256
managed_stripped_file_count=$(find "$MS" -maxdepth 1 -type f | wc -l | tr -d ' ')
full_export_exit=$EXPORT_RC
production_promotion=NO
EOF

# Prove the seed is relocatable: instantiate the rsp under CANARY and call IL2CPP directly.
python3 - "$CANARY" "$CANARY/Library/Bee/artifacts/rsp/$RSP_BASE.template" "$CANARY/Library/Bee/artifacts/rsp/$RSP_BASE" <<'PY'
import sys
from pathlib import Path
root=str(Path(sys.argv[1]).resolve())
s=Path(sys.argv[2]).read_text().replace('__PROJECT_ROOT__',root)
Path(sys.argv[3]).write_text(s)
PY
mkdir -p "$CANARY/Library/Bee/artifacts/iOS/il2cppOutput/cpp/Symbols" "$CANARY/Library/Bee/artifacts/iOS/il2cppOutput/data"
set +e
(
  cd "$CANARY"
  ./il2cpp-deploy/il2cpp @"Library/Bee/artifacts/rsp/$RSP_BASE" > evidence/direct-il2cpp.stdout 2> evidence/direct-il2cpp.stderr
)
DIRECT_RC=$?
set -e
echo "$DIRECT_RC" > "$CANARY/evidence/direct-il2cpp.exit"
cat "$CANARY/evidence/direct-il2cpp.stdout" "$CANARY/evidence/direct-il2cpp.stderr" > "$CANARY/evidence/direct-il2cpp.combined"

# The direct canary is valid only if it reproduces exactly the full Unity IL2CPP method-error set.
python3 - "$E/ios-export-native7.log" "$CANARY/evidence/direct-il2cpp.combined" "$CANARY/evidence/error-set.txt" <<'PY'
import re,sys
from pathlib import Path
def methods(p):
    s=Path(p).read_text(errors='replace')
    return sorted(set(re.findall(r"IL2CPP error for method '([^']+)'",s)))
a=methods(sys.argv[1]); b=methods(sys.argv[2])
Path(sys.argv[3]).write_text('full='+repr(a)+'\ndirect='+repr(b)+'\n')
if a!=b: raise SystemExit(f'IL2CPP canary error-set mismatch: full={a} direct={b}')
if not a and (Path(sys.argv[1]).read_text(errors='replace').find('IL2CPP')<0):
    raise SystemExit('full run did not reach IL2CPP')
print('CANARY_ERROR_SET_MATCH_PASS',a)
PY

# Keep only the reusable late-stage state; the giant Editor/R3 prefixes are deliberately not uploaded.
du -sh "$CANARY" | tee "$E/canary-size.txt"
sha256sum "$CANARY/Library/Bee/artifacts/iOS/ManagedStripped/GodsPVZRuntime1.dll" > "$CANARY/evidence/stripped-game.sha256"
cp "$E/ios-export-native7.log" "$CANARY/evidence/full-unity-export.log"
cp "$E/ios-export-native7.exit" "$CANARY/evidence/full-unity-export.exit"
echo "CANARY_SEED_PASS direct_rc=$DIRECT_RC full_export_rc=$EXPORT_RC stripped_sha256=$STRIPPED_SHA"
