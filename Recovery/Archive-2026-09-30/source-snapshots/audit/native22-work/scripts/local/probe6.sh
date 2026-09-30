echo "=== full2 counts: files / logs / non-empty logs"
cd /workspaces/GodsPVZ-native19/.validation/native22-full2
ls | sed 's/.*\.//' | sort | uniq -c
echo "logs with bytes>0: $(find . -name '*.log' -size +0c | wc -l)"
echo "total 'error:' lines across all full2 logs: $(cat *.log 2>/dev/null | grep -c 'error:')"
echo ""
echo "=== full-tu (old) counts"
cd /workspaces/GodsPVZ-native19/.validation/native22-full-tu
echo "total 'error:' lines: $(cat *.log 2>/dev/null | grep -c 'error:')"
echo ""
echo "=== sample full2 log with errors"
cd /workspaces/GodsPVZ-native19/.validation/native22-full2
for f in *.log; do n=$(grep -c 'error:' "$f" 2>/dev/null || echo 0); if [ "$n" -gt 0 ]; then echo "--- $f ($n errors)"; head -12 "$f"; break; fi; done
echo ""
echo "=== il2cppOutput composition"
cd /workspaces/GodsPVZ-native19/.validation/xcode-native18/expanded/xcode/Il2CppOutputProject/Source/il2cppOutput
ls | sed 's/.*\.//' | sort | uniq -c
