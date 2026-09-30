W=/workspaces/GodsPVZ-native19/.validation/native22
OUT=$W/regen-batch1
echo "convert_done: $([ -f $OUT/_convert_done ] && echo YES || echo no)"
echo "clang_done  : $([ -f $OUT/_clang_done ] && echo YES || echo no)"
echo "cpp files: $(ls $OUT/cpp/*.cpp 2>/dev/null | wc -l)"
echo "clang logs: $(ls $W/retest/clang/*.log 2>/dev/null | wc -l)"
echo "alive: $(pgrep -f native23_retest.sh | wc -l)"
tail -3 $W/retest/driver.log 2>/dev/null
