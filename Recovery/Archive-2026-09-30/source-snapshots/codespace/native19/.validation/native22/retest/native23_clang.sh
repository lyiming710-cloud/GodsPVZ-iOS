#!/bin/bash
W=/workspaces/GodsPVZ-native19/.validation/native22
XINC=/workspaces/GodsPVZ-native19/.validation/xcode-native18/expanded/xcode/Il2CppOutputProject/IL2CPP
OUT=/workspaces/GodsPVZ-native19/.validation/native22/regen-batch1
CLANGDIR=/workspaces/GodsPVZ-native19/.validation/native22/retest/clang

rm -rf "$CLANGDIR"
mkdir -p "$CLANGDIR"
cd "$OUT/cpp" || exit 1
ls *.cpp | xargs -P 4 -I{} bash -c 'clang++ -std=c++11 -fsyntax-only -ferror-limit=0 \
  -Wno-tautological-compare -Wno-unused-value -Wno-invalid-noreturn \
  -DRUNTIME_IL2CPP -DIL2CPP_MONO_DEBUGGER_DISABLED -DIL2CPP_DEBUG=0 -DNDEBUG \
  -DBASELIB_INLINE_NAMESPACE=il2cpp_baselib \
  -I'"$OUT"'/cpp -I'"$XINC"'/libil2cpp/pch -I'"$XINC"'/libil2cpp \
  -I'"$XINC"'/external/baselib/Include -I'"$XINC"'/external/baselib/Platforms/Linux/Include \
  "$1" > '"$CLANGDIR"'/"$1".log 2>&1' _ {}
date +%s > "$OUT/_clang_done2"
