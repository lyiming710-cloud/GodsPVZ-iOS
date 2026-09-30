#!/usr/bin/env bash
# ==============================================================================
# run_all.sh - Master Orchestrator for Codespaces Gate Pipeline
# ==============================================================================
set -euo pipefail

SCRIPT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"

SKIP_SETUP=false
SKIP_STATIC=false
CANDIDATE=""

while [[ $# -gt 0 ]]; do
  case "$1" in
    --skip-setup)
      SKIP_SETUP=true
      shift
      ;;
    --skip-static)
      SKIP_STATIC=true
      shift
      ;;
    --candidate)
      CANDIDATE="$2"
      shift 2
      ;;
    *)
      echo "Unknown flag: $1" >&2
      echo "Usage: ./run_all.sh [--skip-setup] [--skip-static] [--candidate <path/to/Assembly-CSharp.dll>]" >&2
      exit 1
      ;;
  esac
done

echo "=================================================================="
echo "  GodsPVZ iOS Port - Codespaces Master Gate Runner"
echo "=================================================================="

# Step 1: Pre-flight check
bash "$SCRIPT_DIR/01_check_environment.sh"

# Step 2: Static gate
if [ "$SKIP_STATIC" = false ]; then
  bash "$SCRIPT_DIR/02_run_local_static_gate.sh"
else
  echo "[*] Skipping static gate (--skip-static)."
fi

# Step 3: Setup Unity & Project
if [ "$SKIP_SETUP" = false ]; then
  bash "$SCRIPT_DIR/03_setup_unity_and_project.sh"
else
  echo "[*] Skipping Unity & project setup (--skip-setup)."
fi

# Step 4: Export & IL2CPP CodeGen
if [ -n "$CANDIDATE" ]; then
  bash "$SCRIPT_DIR/04_export_ios_xcode.sh" "$CANDIDATE"
else
  bash "$SCRIPT_DIR/04_export_ios_xcode.sh"
fi

echo "=================================================================="
echo "  [COMPLETE] Pipeline run finished successfully!"
echo "=================================================================="
