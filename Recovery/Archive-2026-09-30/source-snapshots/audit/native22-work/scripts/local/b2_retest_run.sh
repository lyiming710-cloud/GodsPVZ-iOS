cat > /workspaces/GodsPVZ-native19/.validation/native22/retest2/native23_retest2.sh <<'SCRIPT_EOF'
#!/bin/bash
CAN=/workspaces/GodsPVZ-native19/.validation/native21/replay/canary
IL2CPP=/workspaces/GodsPVZ-native19/.validation/native19/replay/canary/il2cpp/build/deploy/il2cpp
W=/workspaces/GodsPVZ-native19/.validation/native22
XINC=/workspaces/GodsPVZ-native19/.validation/xcode-native18/expanded/xcode/Il2CppOutputProject/IL2CPP
OUT=/workspaces/GodsPVZ-native19/.validation/native22/regen-batch2
ARGS=$(cat /workspaces/GodsPVZ-native19/.validation/native22/regen-args.txt)

rm -rf "$OUT"
mkdir -p "$OUT/cpp" "$OUT/data" "$OUT/symbols" "$W/retest2/clang"
cd "$CAN" || exit 1
export PROJECT_DIR="$CAN"

$IL2CPP --convert-to-cpp --custom-il2-cpp-root="$CAN/il2cpp" $ARGS \
  --generatedcppdir="$OUT/cpp" --symbols-folder="$OUT/symbols" \
  --emit-null-checks --enable-array-bounds-check --dotnetprofile=unityaot-macos \
  --data-folder="$OUT/data" > "$W/retest2/convert.log" 2>&1
echo "convert rc=$?" >> "$W/retest2/convert.log"
date +%s > "$OUT/_convert_done"

rm -rf "$W/retest2/clang"
mkdir -p "$W/retest2/clang"
cd "$OUT/cpp" || exit 1
ls *.cpp | xargs -P 4 -I{} bash -c 'clang++ -std=c++11 -fsyntax-only -ferror-limit=0 \
  -Wno-tautological-compare -Wno-unused-value -Wno-invalid-noreturn \
  -DRUNTIME_IL2CPP -DIL2CPP_MONO_DEBUGGER_DISABLED -DIL2CPP_DEBUG=0 -DNDEBUG \
  -DBASELIB_INLINE_NAMESPACE=il2cpp_baselib \
  -I'"$OUT"'/cpp -I'"$XINC"'/libil2cpp/pch -I'"$XINC"'/libil2cpp \
  -I'"$XINC"'/external/baselib/Include -I'"$XINC"'/external/baselib/Platforms/Linux/Include \
  "$1" > '"$W"'/retest2/clang/"$1".log 2>&1' _ {}
date +%s > "$OUT/_clang_done"
SCRIPT_EOF
cd /workspaces/GodsPVZ-native19/.validation/native22
nohup bash retest2/native23_retest2.sh > retest2/driver.log 2>&1 &
echo "started pid=$!"
sleep 8
echo "cpp files: $(ls regen-batch2/cpp 2>/dev/null | wc -l)"
