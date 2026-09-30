set -e
cd /workspaces/GodsPVZ-native19
git rm -r --cached -q scripts/codespaces/RepairNative23/obj || true
grep -q "RepairNative23/obj" .gitignore || printf '\n# dotnet build artefacts from the IL patcher\nscripts/codespaces/RepairNative23/obj/\nscripts/codespaces/RepairNative23/bin/\n' >> .gitignore
git add .gitignore
git -c user.name=codex -c user.email=codex@local commit -q -m "chore(native23): stop tracking dotnet build artefacts of the IL patcher"
git rev-parse HEAD
git status --porcelain | head
echo "--- tracked files in this change ---"
git show --stat --oneline HEAD | head -8
