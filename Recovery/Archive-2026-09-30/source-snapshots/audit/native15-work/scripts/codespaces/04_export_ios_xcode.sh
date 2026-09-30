#!/usr/bin/env bash
# ==============================================================================
# 04_export_ios_xcode.sh - Execute Unity BatchMode iOS Export & IL2CPP CodeGen
# ==============================================================================
set -euo pipefail

SCRIPT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
REPO_ROOT="/workspaces/GodsPVZ-iOS"

WORK_DIR="${GATE_WORK_DIR:-/tmp/stage9-native4-ios}"
E="$WORK_DIR/evidence"
mkdir -p "$E"

if [ ! -f "$WORK_DIR/unity-path.txt" ] || [ ! -f "$WORK_DIR/project-path.txt" ]; then
  echo "[!] Error: Unity and Project paths not found. Run 03_setup_unity_and_project.sh first." >&2
  exit 1
fi

UNITY="$(cat "$WORK_DIR/unity-path.txt")"
PROJECT="$(cat "$WORK_DIR/project-path.txt")"

# Candidate DLL selection (allow passing as $1)
CANDIDATE_DLL="${1:-$REPO_ROOT/Tools/Stage9Native4Recovery/candidate/Assembly-CSharp-native4-six-method.dll}"
if [ ! -f "$CANDIDATE_DLL" ]; then
  echo "[!] Candidate DLL not found at: $CANDIDATE_DLL"
  echo "    Checking fallback location..."
  CANDIDATE_DLL="$REPO_ROOT/candidate/Assembly-CSharp-native4-six-method.dll"
fi

if [ ! -f "$CANDIDATE_DLL" ]; then
  echo "[!] Error: Candidate DLL not found!" >&2
  exit 1
fi

CAND_SHA="$(sha256sum "$CANDIDATE_DLL" | awk '{print $1}')"
echo "=================================================================="
echo "  Executing Unity iOS Xcode Export with Candidate DLL"
echo "  Candidate: $CANDIDATE_DLL"
echo "  SHA256:    $CAND_SHA"
echo "=================================================================="

# 1. Inject candidate into project
PLUGIN_DIR="$PROJECT/Assets/Plugins"
mkdir -p "$PLUGIN_DIR"
cp "$CANDIDATE_DLL" "$PLUGIN_DIR/Assembly-CSharp.dll"
echo "[+] Injected candidate DLL into $PLUGIN_DIR/Assembly-CSharp.dll"

# 2. Run Unity BatchMode Export under xvfb-run
XCODE_OUT="$WORK_DIR/xcode"
rm -rf "$XCODE_OUT"

echo "[*] Launching Unity BatchMode export (Mesa software GL)..."
set +e
xvfb-run -a -s '-screen 0 1280x720x24' "$UNITY" \
  -batchmode -quit -force-glcore \
  -buildTarget iOS \
  -projectPath "$PROJECT" \
  -executeMethod GodsPVZIOSBuild.BuildIOS \
  -logFile "$E/ios-export.log" > "$E/ios-export.stdout" 2> "$E/ios-export.stderr"

RC=$?
set -e
echo "$RC" > "$E/ios-export-exit.txt"

# 3. Analyze Output & Diagnostics
if [ "$RC" -eq 0 ] && [ -d "$XCODE_OUT" ]; then
  echo "=================================================================="
  echo "  [SUCCESS] Unity iOS Xcode Export & IL2CPP CodeGen PASSED!"
  echo "=================================================================="
  CPP_FILE="$XCODE_OUT/Il2CppOutputProject/Source/il2cppOutput/Assembly-CSharp.cpp"
  META_FILE="$XCODE_OUT/Data/Managed/Metadata/global-metadata.dat"
  
  if [ -f "$CPP_FILE" ]; then
    echo "[+] IL2CPP generated Assembly-CSharp.cpp successfully!"
    echo "    CPP SHA256: $(sha256sum "$CPP_FILE" | awk '{print $1}')"
  fi
  if [ -f "$META_FILE" ]; then
    echo "[+] Generated metadata global-metadata.dat successfully!"
    echo "    Metadata SHA256: $(sha256sum "$META_FILE" | awk '{print $1}')"
  fi

  CPP_COUNT="$(find "$XCODE_OUT/Il2CppOutputProject/Source/il2cppOutput" -maxdepth 1 -type f -name '*.cpp' 2>/dev/null | wc -l | tr -d ' ')"
  echo "[+] Total IL2CPP C++ files: $CPP_COUNT"
  echo "[+] Xcode project generated at: $XCODE_OUT"
else
  echo "=================================================================="
  echo "  [FAILURE] Unity iOS Export failed with exit code $RC"
  echo "=================================================================="
  echo "[*] Extracting error summary from $E/ios-export.log:"
  echo "------------------------------------------------------------------"
  grep -inE 'error|exception|fail|il2cpp' "$E/ios-export.log" | tail -n 30 || true
  echo "------------------------------------------------------------------"
  echo "Full log saved at: $E/ios-export.log"
  exit "$RC"
fi
