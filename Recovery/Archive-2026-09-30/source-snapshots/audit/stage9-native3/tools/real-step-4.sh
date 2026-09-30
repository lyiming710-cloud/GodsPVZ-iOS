set -euo pipefail
OUT="$RUNNER_TEMP/stage9-native3-ios"
E="$OUT/evidence"
PROJECT="$(cat "$OUT/project-path.txt")"
DLL="$OUT/base-runtime/GodsPVZRuntime1-bgmvolume.dll"
test -f "$DLL"
[ "$(sha256sum "$DLL" | awk '{print $1}')" = "$BASE_RUNTIME_SHA256" ] || exit 49
cp "$DLL" "$PROJECT/Assets/Plugins/Assembly-CSharp.dll"
mkdir -p "$PROJECT/Assets/Editor" "$PROJECT/Recovery"
cp "$GITHUB_WORKSPACE/Assets/Editor/GodsPVZPackageReferenceMigrator.cs" "$PROJECT/Assets/Editor/"
cp "$GITHUB_WORKSPACE/Assets/Editor/GodsPVZIOSBuild.cs" "$PROJECT/Assets/Editor/"
cp "$GITHUB_WORKSPACE/Recovery/package-script-map-editor.json" "$PROJECT/Recovery/"
python3 - "$OUT/package-bundle/Packages" "$OUT/package-closure/Packages" "$PROJECT/Packages" <<'PY'
import shutil,sys
from pathlib import Path
dst=Path(sys.argv[3]); dst.mkdir(parents=True,exist_ok=True)
for root in map(Path,sys.argv[1:3]):
    if not root.is_dir(): raise SystemExit('missing package source: '+str(root))
    for src in sorted(p for p in root.iterdir() if p.is_dir()):
        target=dst/src.name
        if target.exists(): shutil.rmtree(target)
        shutil.copytree(src,target)
PY
python3 - "$OUT/package-guid-evidence/package-guid-map.json" "$PROJECT/Packages" "$E/package-guid-preflight.txt" <<'PY'
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
out.write_text(f'resolved={n}/67\nfailures={len(bad)}\n'+repr(bad)+'\n')
if n!=67 or bad: raise SystemExit(50)
PY
rm -f "$PROJECT/Packages/packages-lock.json"
rm -rf "$OUT/package-bundle" "$OUT/package-closure" "$OUT/package-guid-evidence" "$OUT/base-runtime"

