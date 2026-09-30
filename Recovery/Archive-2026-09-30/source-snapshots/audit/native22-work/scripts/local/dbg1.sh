cd /workspaces/GodsPVZ-native19
echo "=== pwd ==="; pwd
echo ""
echo "=== git ==="; git rev-parse --abbrev-ref HEAD; git rev-parse HEAD
echo ""
echo "=== native22 dir ==="
ls -la .validation/native22 2>/dev/null
echo ""
echo "=== find candidate clang logs (mtime desc) ==="
find .validation -maxdepth 3 -type f \( -name "*.log" -o -name "*.txt" -o -name "*.err" \) -printf "%T@ %p %s\n" 2>/dev/null | sort -rn | head -40
echo ""
echo "=== any tu-scan outputs ==="
find .validation/native22 -maxdepth 2 -type f -printf "%T@ %p %s\n" 2>/dev/null | sort -rn | head -40
