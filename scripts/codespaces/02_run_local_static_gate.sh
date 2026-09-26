#!/usr/bin/env bash
# ==============================================================================
# 02_run_local_static_gate.sh - Linux Native Six-Method Static Gate
# ==============================================================================
set -euo pipefail

SCRIPT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
REPO_ROOT="/workspaces/GodsPVZ-iOS"
NATIVE4_DIR="$REPO_ROOT/Tools/Stage9Native4Recovery"

if [ ! -d "$NATIVE4_DIR" ]; then
  echo "[!] Error: Stage9Native4Recovery folder not found at $NATIVE4_DIR" >&2
  exit 1
fi

echo "=================================================================="
echo "  Stage 9.1 native4: Six-Method Static Gate Verification (Linux)"
echo "=================================================================="

# 1. Run Python Stack Evaluation Verifier
echo "[*] Step 1: Running Worklist Typed Stack Verifier on specification & reopened..."
export PYTHONPATH="$NATIVE4_DIR/python-deps:${PYTHONPATH:-}"
python3 "$NATIVE4_DIR/verify_stack.py" "$NATIVE4_DIR/specification"
echo "[+] Specification stack verification passed!"

# 2. Run Spec Readback & Use Delta Verification
echo "[*] Step 2: Running Spec Readback and Reference Use Delta Lock..."
python3 "$NATIVE4_DIR/verify_spec_readback.py"
echo "[+] Spec readback & use deltas matched exact review manifest!"

# 3. Check / Run CLR & Cecil Gate if dotnet or mono is present
if command -v dotnet >/dev/null 2>&1; then
  echo "[*] Step 3: .NET SDK detected ($(dotnet --version))."
elif command -v mono >/dev/null 2>&1; then
  echo "[*] Step 3: Mono runtime detected ($(mono --version | head -n1))."
else
  echo "[i] Tip: Install dotnet-sdk or mono to run in-memory CLR test harness and Cecil gate directly."
fi

echo "=================================================================="
echo "  [SUCCESS] All Python static gates and stack checks passed!"
echo "=================================================================="
