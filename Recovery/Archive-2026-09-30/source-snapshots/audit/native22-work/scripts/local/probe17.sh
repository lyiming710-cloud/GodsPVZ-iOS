set -x
echo "=== pwd / repo ==="
pwd
git rev-parse --abbrev-ref HEAD
git rev-parse HEAD
git status --porcelain | head -20
echo "=== work dir ==="
ls -la work 2>/dev/null | head -30
echo "=== out dir ==="
ls -la out 2>/dev/null | head -30
echo "=== runtimes ==="
which mono monodis dotnet python3 2>/dev/null
python3 -c "import sys; print(sys.version)"
echo "=== RepairNative23 ==="
ls -la scripts/codespaces/RepairNative23 2>/dev/null | head -40
