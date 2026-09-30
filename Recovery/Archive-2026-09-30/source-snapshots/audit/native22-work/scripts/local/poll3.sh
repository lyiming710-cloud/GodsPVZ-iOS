W=/workspaces/GodsPVZ-native19/.validation/native22
OUT=$W/regen-batch1
echo "clang_done2: $([ -f $OUT/_clang_done2 ] && echo YES || echo no)"
echo "logs: $(ls $W/retest/clang/*.log 2>/dev/null | wc -l)"
echo "alive: $(pgrep -f native23_clang.sh | wc -l)"
