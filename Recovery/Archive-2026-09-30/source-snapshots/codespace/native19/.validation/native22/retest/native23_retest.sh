#!/bin/bash
CAN=/workspaces/GodsPVZ-native19/.validation/native21/replay/canary
IL2CPP=/workspaces/GodsPVZ-native19/.validation/native19/replay/canary/il2cpp/build/deploy/il2cpp
W=/workspaces/GodsPVZ-native19/.validation/native22
XINC=/workspaces/GodsPVZ-native19/.validation/xcode-native18/expanded/xcode/Il2CppOutputProject/IL2CPP
OUT=/workspaces/GodsPVZ-native19/.validation/native22/regen-batch1
ARGS=$(cat /workspaces/GodsPVZ-native19/.validation/native22/regen-args.txt)

rm -rf "$OUT"
mkdir -p "$OUT/cpp" "$OUT/data" "$OUT/symbols" "$W/retest/clang"
cd "$CAN" || exit 1
export PROJECT_DIR="$CAN"

$IL2CPP --convert-to-cpp --custom-il2-cpp-root="$CAN/il2cpp" $ARGS \
  --generatedcppdir="$OUT/cpp" --symbols-folder="$OUT/symbols" \
  --emit-null-checks --enable-array-bounds-check --dotnetprofile=unityaot-macos \
  --data-folder="$OUT/data" > "$W/retest/convert.log" 2>&1
date +%s > "$OUT/_convert_done"

rm -rf "$W/retest/clang"
mkdir -p "$W/retest/clang"
cd "$OUT/cpp" || exit 1
ls *.cpp | xargs -P 4 -I{} clang++ -std=c++11 -fsyntax-only -ferror-limit=0 \
  -Wno-tautological-compare -Wno-unused-value -Wno-invalid-noreturn \
  -DRUNTIME_IL2CPP -DIL2CPP_MONO_DEBUGGER_DISABLED -DIL2CPP_DEBUG=0 -DNDEBUG \
  -DBASELIB_INLINE_NAMESPACE=il2cpp_baselib \
  -I"$OUT/cpp" -I"$XINC/libil2cpp/pch" -I"$XINC/libil2cpp" \
  -I"$XINC/external/baselib/Include" -I"$XINC/external/baselib/Platforms/Linux/Include" \
  {} > "$W/retest/clang/{}.log" 2>&1
date +%s > "$OUT/_clang_done"
