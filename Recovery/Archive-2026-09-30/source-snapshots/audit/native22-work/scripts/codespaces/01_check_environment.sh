#!/usr/bin/env bash
# ==============================================================================
# 01_check_environment.sh - GitHub Codespaces / Linux Pre-flight Diagnostics
# ==============================================================================
set -euo pipefail

echo "=================================================================="
echo "  GodsPVZ iOS Port - Codespaces Environment Pre-flight Diagnostic"
echo "=================================================================="

# 1. OS & Architecture Check
OS="$(uname -s)"
ARCH="$(uname -m)"
echo "[*] System: $OS $ARCH"
if [ "$OS" != "Linux" ] || [ "$ARCH" != "x86_64" ]; then
  echo "[!] WARNING: Unity 2022.3.44f1c1 Linux Editor requires Linux x86_64."
fi

# 2. Disk Space Check
echo "[*] Checking available disk space..."
AVAILABLE_KB=$(df -k . | awk 'NR==2 {print $4}')
AVAILABLE_GB=$((AVAILABLE_KB / 1024 / 1024))
echo "    Available disk space in current mount: ~${AVAILABLE_GB} GB"
if [ "$AVAILABLE_GB" -lt 15 ]; then
  echo "[!] CRITICAL WARNING: Free disk space is low (${AVAILABLE_GB} GB)."
  echo "    Running Unity Editor reconstruction + R3 project export requires ~15-20 GB."
  echo "    Consider running 'docker system prune -af' or removing unused build caches."
elif [ "$AVAILABLE_GB" -lt 25 ]; then
  echo "[i] NOTICE: Disk space is sufficient with stream-cleaning (~${AVAILABLE_GB} GB)."
else
  echo "[+] Disk space is plentiful (${AVAILABLE_GB} GB)."
fi

# 3. System Packages Check & Auto-Install
MISSING_PKGS=()
for cmd in xvfb-run glxinfo zstd xz tar python3 curl; do
  if ! command -v "$cmd" >/dev/null 2>&1; then
    MISSING_PKGS+=("$cmd")
  fi
done

if [ ${#MISSING_PKGS[@]} -gt 0 ]; then
  echo "[*] Missing dependencies: ${MISSING_PKGS[*]}"
  if command -v sudo >/dev/null 2>&1; then
    echo "[*] Installing required packages via apt..."
    sudo apt-get update -qq
    sudo apt-get install -y -qq xvfb libgl1-mesa-dri mesa-utils zstd xz-utils tar curl python3 python3-pip
    echo "[+] System packages installed."
  else
    echo "[!] sudo not available. Please ensure: xvfb, mesa-utils, zstd, xz-utils are installed."
  fi
else
  echo "[+] All required system utilities are installed (xvfb, mesa, zstd, xz, python3)."
fi

# 4. GitHub Auth / Token Check
echo "[*] Checking GitHub API access..."
if [ -n "${GH_TOKEN:-}" ] || [ -n "${GITHUB_TOKEN:-}" ]; then
  echo "[+] GitHub token detected in environment."
elif command -v gh >/dev/null 2>&1 && gh auth status >/dev/null 2>&1; then
  echo "[+] GitHub CLI is authenticated."
else
  echo "[!] NOTICE: Neither GH_TOKEN nor 'gh auth' found."
  echo "    Downloading private CI artifacts (R3 cache, Editor parts) will require GH_TOKEN."
  echo "    Tip: export GH_TOKEN=\"ghp_...\""
fi

# 5. Unity Credentials Check
echo "[*] Checking Unity credentials..."
if [ -n "${UNITY_EMAIL:-}" ] && [ -n "${UNITY_PASSWORD:-}" ]; then
  echo "[+] Unity credentials detected (Account: ${UNITY_EMAIL})."
else
  echo "[!] NOTICE: UNITY_EMAIL or UNITY_PASSWORD not set."
  echo "    Step 03 / Step 04 will require these to acquire a Unity license seat."
  echo "    Tip: export UNITY_EMAIL=\"your@email.com\""
  echo "         export UNITY_PASSWORD=\"your_password\""
fi

echo "=================================================================="
echo "  Pre-flight check completed!"
echo "=================================================================="
