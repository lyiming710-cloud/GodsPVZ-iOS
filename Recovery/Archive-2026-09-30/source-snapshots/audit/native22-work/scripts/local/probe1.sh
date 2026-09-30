cd /workspaces/GodsPVZ-native19 2>/dev/null || cd ~
echo "=== PWD: $(pwd)"
echo "=== branch: $(git rev-parse --abbrev-ref HEAD) $(git rev-parse --short HEAD)"
echo ""
echo "=== .validation/native22 tree (depth 3)"
find .validation/native22 -maxdepth 3 2>/dev/null | head -60
echo ""
echo "=== search tidy artifacts"
find /workspaces -maxdepth 8 \( -name 'phantom-scan.json' -o -name 'methods-all.json' -o -name '*.export.json' \) 2>/dev/null | head -30
echo ""
echo "=== out dirs"
find /workspaces -maxdepth 6 -type d -name 'out' 2>/dev/null | head -20
echo ""
echo "=== disk"
df -h /workspaces | tail -2
echo ""
echo "=== python"
python3 --version; command -v objdump pip3 mono dotnet
