cd /workspaces/GodsPVZ-native19
echo "=== native22-full2 top ==="
ls -la .validation/native22-full2 | head -20
echo ""
echo "=== counts ==="
ls .validation/native22-full2/*.log 2>/dev/null | wc -l
echo "total error lines:"
cat .validation/native22-full2/*.log 2>/dev/null | wc -l
echo ""
echo "=== non-log files in native22-full2 ==="
find .validation/native22-full2 -maxdepth 1 -type f ! -name "*.log" -printf "%p %s\n" | head -20
echo ""
echo "=== sample: GodsPVZRuntime1__8.cpp.log first 30 lines ==="
head -30 .validation/native22-full2/GodsPVZRuntime1__8.cpp.log
echo ""
echo "=== does any log include an absolute source path? ==="
grep -h -o "/[^ :]*GodsPVZRuntime1__8\.cpp" .validation/native22-full2/*.log 2>/dev/null | sort -u | head
echo ""
echo "=== the tu scan script ==="
ls -la .validation/native22/../ 2>/dev/null | head -30
find /workspaces/GodsPVZ-native19 -maxdepth 3 -name "native22_full_tu_scan.sh" -printf "%T@ %p %s\n"
