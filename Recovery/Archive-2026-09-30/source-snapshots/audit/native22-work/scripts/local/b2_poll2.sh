cd /workspaces/GodsPVZ-native19/.validation/native22
pgrep -fa "native23_retest2.sh" | head -2
echo "cpp: $(ls regen-batch2/cpp/*.cpp 2>/dev/null | wc -l)"
echo "convert done: $(ls regen-batch2/_convert_done 2>/dev/null || echo no)"
echo "clang logs: $(ls retest2/clang/*.log 2>/dev/null | wc -l)"
echo "clang done: $(ls regen-batch2/_clang_done 2>/dev/null || echo no)"
tail -3 retest2/convert.log 2>/dev/null
