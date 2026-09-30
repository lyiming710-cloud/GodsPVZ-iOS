cd /workspaces/GodsPVZ-native19
echo "=== il2cpp.log ==="
cat .validation/native21/replay/il2cpp.log
echo ""
echo "=== candidate.json ==="
cat .validation/native21/replay/candidate.json
echo ""
echo "=== search for il2cpp binary / Unity installs (broad) ==="
for p in /opt /usr/local /home /srv /mnt /data; do
  [ -d "$p" ] && find "$p" -maxdepth 5 -name "il2cpp*" -printf "%p %s\n" 2>/dev/null | head -10
done
echo "-- Unity dirs:"
ls -d /opt/*nity* /home/*/*nity* /opt/*/*nity* 2>/dev/null | head
echo ""
echo "=== Assembly-CSharp anywhere in repo (any depth) ==="
find /workspaces/GodsPVZ-native19 -name "Assembly-CSharp*.dll" -printf "%T@ %p %s\n" 2>/dev/null | sort -rn | head -20
echo ""
echo "=== which of the two IL dlls produced canary cpp? check Tools/Stage9NativeProbe ==="
ls -la /workspaces/GodsPVZ-native19/Tools/Stage9NativeProbe 2>&1 | head -20
echo ""
echo "=== find scripts mentioning 'il2cppOutput' generation ==="
grep -rl "il2cppOutput" /workspaces/GodsPVZ-native19/Tools 2>/dev/null | head -10
