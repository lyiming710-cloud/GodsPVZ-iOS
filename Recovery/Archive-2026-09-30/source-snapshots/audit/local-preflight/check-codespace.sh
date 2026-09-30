#!/usr/bin/env bash
# Read-only environment inventory. Does not print environment variables,
# credentials, license contents, or full process command lines.
set -u
printf '\n=== SYSTEM ===\n'
uname -sm
getconf _NPROCESSORS_ONLN
free -h
df -h . /tmp
printf '\n=== REPOSITORY ===\n'
pwd
git rev-parse --show-toplevel HEAD 2>/dev/null || true
git branch --show-current 2>/dev/null || true
git status --short 2>/dev/null | head -60
printf '\n=== TOOLS ===\n'
for t in Unity unity-editor mono csc mcs dotnet python3 clang clang++ cmake ninja Xvfb; do
  command -v "$t" 2>/dev/null || true
done
command -v mono >/dev/null && mono --version | head -2
command -v dotnet >/dev/null && dotnet --list-sdks
printf '\n=== INSTALLED FILES AND BUILD CACHES ===\n'
for root in /workspaces /opt /usr/local /tmp; do
  test -d "$root" || continue
  find "$root" -maxdepth 9 \( -path '*/.git' -o -path '*/node_modules' \) -prune -o \
    \( -type f \( -name Unity -o -name il2cpp -o -name il2cpp.dll -o -name ProjectVersion.txt -o -name Assembly-CSharp.dll \) \
    -o -type d \( -name ManagedStripped -o -name il2cppOutput -o -name iOSSupport \) \) -print 2>/dev/null | head -100
done
printf '\n=== PROJECT VERSIONS ===\n'
find /workspaces -maxdepth 9 -name ProjectVersion.txt -type f -exec sh -c 'printf "\n%s\n" "$1"; head -4 "$1"' sh '{}' \; 2>/dev/null
printf '\n=== CURRENT UNITY / IL2CPP PROCESS NAMES ===\n'
ps -eo pid,comm | awk 'NR==1 || /Unity|il2cpp|UnityLinker|Xvfb/'
printf '\n=== END: READ-ONLY INVENTORY ===\n'
