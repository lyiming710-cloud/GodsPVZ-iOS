echo "=== branches ==="
git branch -a | head -30
echo "=== worktrees ==="
git worktree list
echo "=== stash ==="
git stash list
echo "=== search for prior artifacts ==="
find /workspaces -maxdepth 6 -name "batch2a.dll" -o -maxdepth 6 -name "RepairNative23" -o -maxdepth 6 -name "edits-batch2*.json" 2>/dev/null | head -20
echo "=== /workspaces top ==="
ls -la /workspaces
echo "=== any dll named batch ==="
find / -maxdepth 8 -name "batch1.dll" -o -maxdepth 8 -name "batch2*.dll" 2>/dev/null | head
echo "=== git log current ==="
git log --oneline -5
echo "=== reflog ==="
git reflog -20
echo "=== dotnet ==="
dotnet --list-sdks 2>&1 | head
echo "=== python pkgs ==="
python3 -c "import pefile" 2>&1 | tail -1
python3 -m pip list 2>/dev/null | head -40
