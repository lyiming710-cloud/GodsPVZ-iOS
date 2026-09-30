cd /workspaces/GodsPVZ-native19/.validation
echo "=========== native22_fullscan.sh ==========="
cat native22_fullscan.sh
echo ""
echo "=========== native22_scan2.sh ==========="
cat native22_scan2.sh
echo ""
echo "=========== exit-code distribution (full2) ==========="
cat native22-full2/*.exit | sort | uniq -c
echo ""
echo "=========== total error/warning lines per category ==========="
cat native22-full2/*.log | grep -c ": error:" 
echo "-- warnings:"
cat native22-full2/*.log | grep -c ": warning:"
echo ""
echo "=========== per-GodsPVZRuntime error counts ==========="
for f in native22-full2/GodsPVZRuntime1*.log; do
  e=$(grep -c ": error:" "$f"); w=$(grep -c ": warning:" "$f")
  echo "$(basename $f) errors=$e warnings=$w"
done
