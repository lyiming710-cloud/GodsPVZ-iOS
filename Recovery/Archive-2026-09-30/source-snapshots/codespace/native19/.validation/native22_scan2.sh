#!/bin/bash
SRC=/workspaces/GodsPVZ-native19/.validation/native21/replay/canary/Library/Bee/artifacts/iOS/il2cppOutput/cpp
BASE=/workspaces/GodsPVZ-native19/.validation/xcode-native18/expanded/xcode/Il2CppOutputProject/IL2CPP
HDR=$BASE/libil2cpp
OUT=/workspaces/GodsPVZ-native19/.validation/native22-full2
rm -rf $OUT; mkdir -p $OUT
cd $SRC
ls *.cpp | xargs -P 4 -I{} bash -c '
  clang++ -std=c++11 -fsyntax-only -ferror-limit=0 \
    -Wno-tautological-compare -Wno-unused-value -Wno-invalid-noreturn \
    -DRUNTIME_IL2CPP -DIL2CPP_MONO_DEBUGGER_DISABLED -DIL2CPP_DEBUG=0 -DNDEBUG \
    -DBASELIB_INLINE_NAMESPACE=il2cpp_baselib \
    -I'"$SRC"' -I'"$HDR"'/pch -I'"$HDR"' \
    -I'"$BASE"'/external/baselib/Include \
    -I'"$BASE"'/external/baselib/Platforms/Linux/Include \
    "{}" > "'"$OUT"'/{}.log" 2>&1
  echo "$?" > "'"$OUT"'/{}.exit"
'
date +%s > $OUT/_complete
