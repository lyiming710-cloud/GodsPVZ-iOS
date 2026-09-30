cd /workspaces/GodsPVZ-native19/.validation/native22-full2
echo "=== raw log: SetDamage errors with caret context"
grep -n -A8 -B2 '14105:22\|14105:10\|14107:22\|14107:10' GodsPVZRuntime1__8.cpp.log | head -60
echo ""
echo "=== all error kinds per file (top 12) for GodsPVZRuntime1__8.cpp"
grep -oE 'error: [^,]*' GodsPVZRuntime1__8.cpp.log | cut -c1-90 | sort | uniq -c | sort -rn | head -12
echo ""
echo "=== total errors in file"
grep -c 'error:' GodsPVZRuntime1__8.cpp.log
