echo "=== cpp scan logs dirs"
find /workspaces/GodsPVZ-native19 -maxdepth 5 -type d \( -name 'cpp*' -o -name '*tuscan*' -o -name '*tu*' \) 2>/dev/null | head -20
echo ""
echo "=== files matching cpp"
find /workspaces/GodsPVZ-native19 -maxdepth 6 \( -name '*.log' -o -name '*cpp*' \) -newermt '2026-09-29' 2>/dev/null | head -40
echo ""
echo "=== any 'final38' or 'full' dirs"
find /workspaces/GodsPVZ-native19 -maxdepth 6 -type d -name '*full*' -o -maxdepth 6 -type d -name '*final38*' 2>/dev/null | head
echo ""
echo "=== analyze_cpp_failures.py usage"
head -30 /workspaces/GodsPVZ-native19/scripts/codespaces/analyze_cpp_failures.py
echo ""
echo "=== audit_all.py"
head -40 /workspaces/GodsPVZ-native19/.validation/native22/audit_all.py
