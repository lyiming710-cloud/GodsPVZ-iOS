#!/usr/bin/env bash
# ==============================================================================
# 03_setup_unity_and_project.sh - Codespaces Unity 2022.3.44f1c1 & Project Setup
# ==============================================================================
set -euo pipefail

SCRIPT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
REPO_ROOT="/workspaces/GodsPVZ-iOS"

WORK_DIR="${GATE_WORK_DIR:-/tmp/stage9-native4-ios}"
mkdir -p "$WORK_DIR/evidence" "$WORK_DIR/r3" "$WORK_DIR/editor-root"

# Pinned Artifact Identifiers
REPO="lyiming710-cloud/GodsPVZ-iOS"
R3_RUN_ID="34747460206"
R3_ARTIFACT_ID="10314333890"
R3_SHA256="d4264f12a00e86b6149d20ecf04caa9761af510aece6107bcceb155ea4ab0bbc"

BASE_RUNTIME_RUN_ID="35236211080"
PACKAGE_BUNDLE_RUN_ID="35239547918"
PACKAGE_CLOSURE_RUN_ID="35239319822"
PACKAGE_GUID_RUN_ID="35238895838"

EDITOR_RUN_ID="34668863583"
EDITOR_SHA256="0008115c785784baddb19e2b13b384ac845438239f293efb0c2e8b1379a1fe14"

IOS_MODULE_RUN_ID="35339261693"
IOS_MODULE_SHA256="2c2e259415ab0f940889b6b5bf55c56bc2710bbd823ddc05c5ab67ebc137a16e"

UNITY_VERSION="2022.3.44f1c1"

echo "=================================================================="
echo "  Setting up Unity 2022.3.44f1c1 and R3 Project in $WORK_DIR"
echo "=================================================================="

TOKEN="${GH_TOKEN:-${GITHUB_TOKEN:-}}"
if [ -z "$TOKEN" ]; then
  if command -v gh >/dev/null 2>&1 && gh auth token >/dev/null 2>&1; then
    TOKEN="$(gh auth token)"
  else
    echo "[!] Error: GH_TOKEN / GITHUB_TOKEN or 'gh auth' is required to download pinned CI artifacts." >&2
    exit 1
  fi
fi

download_artifact() {
  local artifact_id="$1"
  local out_path="$2"
  python3 - <<PY
import os, urllib.request, zipfile, io
req = urllib.request.Request(
    'https://api.github.com/repos/${REPO}/actions/artifacts/${artifact_id}/zip',
    headers={'Authorization': 'Bearer ${TOKEN}', 'Accept': 'application/vnd.github+json', 'User-Agent': 'Codespaces-Setup'}
)
class NoRedirect(urllib.request.HTTPRedirectHandler):
    def redirect_request(self, *args, **kwargs): return None
opener = urllib.request.build_opener(NoRedirect)
try:
    with opener.open(req, timeout=90) as r: data = r.read()
except urllib.error.HTTPError as e:
    if e.code == 302:
        with urllib.request.urlopen(e.headers['Location'], timeout=120) as r: data = r.read()
    else: raise
with zipfile.ZipFile(io.BytesIO(data)) as z:
    z.extractall('${out_path}')
print("Extracted artifact ${artifact_id} into ${out_path}")
PY
}

# 1. Reconstruct Unity China Editor (if not already reconstructed)
UNITY_BIN="$WORK_DIR/editor-root/Editor/Unity"
if [ -x "$UNITY_BIN" ]; then
  echo "[+] Reconstructed Unity Editor already exists at $UNITY_BIN"
else
  echo "[*] Step 1: Downloading 15 Unity China Editor parts (stream-clean)..."
  mkdir -p "$WORK_DIR/editor-parts"
  python3 - <<PY
import os, urllib.request, json, zipfile, io
req = urllib.request.Request(
    'https://api.github.com/repos/${REPO}/actions/runs/${EDITOR_RUN_ID}/artifacts',
    headers={'Authorization': 'Bearer ${TOKEN}', 'Accept': 'application/vnd.github+json'}
)
with urllib.request.urlopen(req) as resp:
    data = json.loads(resp.read().decode())
parts = sorted([a for a in data['artifacts'] if a['name'].startswith('Stage9.1-unity-china-editor-part-')], key=lambda x: x['name'])
assert len(parts) == 15, f"Expected 15 parts, found {len(parts)}"
out_dir = '${WORK_DIR}/editor-parts'
for p in parts:
    fn = os.path.join(out_dir, f"{p['name']}.zip")
    if not os.path.exists(fn):
        print(f"Downloading {p['name']}...")
        r2 = urllib.request.Request(p['archive_download_url'], headers={'Authorization': 'Bearer ${TOKEN}', 'Accept': 'application/vnd.github+json'})
        try:
            with urllib.request.build_opener().open(r2) as r: c = r.read()
        except urllib.error.HTTPError as e:
            if e.code == 302:
                with urllib.request.urlopen(e.headers['Location']) as r: c = r.read()
            else: raise
        with open(fn, 'wb') as f: f.write(c)
        with zipfile.ZipFile(fn) as z: z.extractall(out_dir)
        os.remove(fn)
PY

  echo "[*] Concatenating and unpacking Unity Editor..."
  mapfile -t PARTS < <(find "$WORK_DIR/editor-parts" -maxdepth 1 -type f -name 'Unity-China-2022.3.44f1c1.tar.xz.part-*' | sort)
  {
    for part in "${PARTS[@]}"; do cat "$part"; rm -f "$part"; done
  } | tar -xJf - -C "$WORK_DIR/editor-root"
  rmdir "$WORK_DIR/editor-parts" 2>/dev/null || true
  chmod +x "$UNITY_BIN"
  echo "[+] Unity Editor reconstructed successfully."
fi

# 2. Overlay iOS Support Module
if [ -d "$WORK_DIR/editor-root/Editor/Data/PlaybackEngines/iOSSupport" ]; then
  echo "[+] iOS Support module already present."
else
  echo "[*] Step 2: Downloading & overlaying iOS Support Module..."
  mkdir -p "$WORK_DIR/ios-mod"
  download_artifact "$IOS_MODULE_RUN_ID" "$WORK_DIR/ios-mod"
  tar -xJf "$WORK_DIR/ios-mod/UnitySetup-iOS-Support-for-Editor-2022.3.44f1.tar.xz" -C "$WORK_DIR/editor-root"
  rm -rf "$WORK_DIR/ios-mod"
  echo "[+] iOS Support module integrated."
fi

# 3. Reconstruct R3 Project
PROJECT_DIR="$WORK_DIR/r3/UnityProject-AssetRipper-2.0.0/ExportedProject"
if [ -d "$PROJECT_DIR" ]; then
  echo "[+] R3 ExportedProject already exists at $PROJECT_DIR"
else
  echo "[*] Step 3: Downloading and extracting R3 Project Cache..."
  mkdir -p "$WORK_DIR/r3-cache"
  download_artifact "$R3_ARTIFACT_ID" "$WORK_DIR/r3-cache"
  R3_ARCHIVE="$(find "$WORK_DIR/r3-cache" -type f -name '*.tar.zst' | head -n1)"
  zstd -dc "$R3_ARCHIVE" | tar -xf - -C "$WORK_DIR/r3"
  rm -rf "$WORK_DIR/r3-cache"
  echo "[+] R3 Project cache unpacked."
fi

# 4. Integrate packages and editor build scripts
echo "[*] Step 4: Injecting GodsPVZ Editor Build Scripts..."
mkdir -p "$PROJECT_DIR/Assets/Editor" "$PROJECT_DIR/Recovery"
if [ -f "$REPO_ROOT/Assets/Editor/GodsPVZIOSBuild.cs" ]; then
  cp "$REPO_ROOT/Assets/Editor/GodsPVZIOSBuild.cs" "$PROJECT_DIR/Assets/Editor/"
  cp "$REPO_ROOT/Assets/Editor/GodsPVZPackageReferenceMigrator.cs" "$PROJECT_DIR/Assets/Editor/"
  cp "$REPO_ROOT/Recovery/package-script-map-editor.json" "$PROJECT_DIR/Recovery/"
fi

# 5. Unity Licensing
echo "[*] Step 5: Checking Unity Licensing..."
CLIENT="$(dirname "$UNITY_BIN")/Data/Resources/Licensing/Client/Unity.Licensing.Client"
if [ -n "${UNITY_EMAIL:-}" ] && [ -n "${UNITY_PASSWORD:-}" ]; then
  echo "[*] Activating Unity Personal License..."
  set +e
  "$CLIENT" --activate-all --include-personal --username "$UNITY_EMAIL" --password "$UNITY_PASSWORD"
  RC=$?
  set -e
  if [ $RC -eq 0 ]; then
    echo "[+] Unity Personal License activated successfully!"
  else
    echo "[!] Warning: License activation returned code $RC. If already activated, continuing..."
  fi
else
  echo "[!] NOTICE: UNITY_EMAIL or UNITY_PASSWORD not set. Skipping automated activation."
fi

printf '%s\n' "$UNITY_BIN" > "$WORK_DIR/unity-path.txt"
printf '%s\n' "$PROJECT_DIR" > "$WORK_DIR/project-path.txt"

echo "=================================================================="
echo "  [SUCCESS] Unity Editor and R3 Project are fully set up!"
echo "  Unity path:   $UNITY_BIN"
echo "  Project path: $PROJECT_DIR"
echo "=================================================================="
