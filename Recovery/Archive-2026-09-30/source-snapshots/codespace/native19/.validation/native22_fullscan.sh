#!/bin/bash
SRC=/workspaces/GodsPVZ-native19/.validation/native21/replay/canary/Library/Bee/artifacts/iOS/il2cppOutput/cpp
HDR=/workspaces/GodsPVZ-native19/.validation/xcode-native18/expanded/xcode/Il2CppOutputProject/IL2CPP/libil2cpp
OUT=/workspaces/GodsPVZ-native19/.validation/native22-full-tu
mkdir -p $OUT
cd $SRC
for f in *.cpp; do
  clang++ -std=c++11 -fsyntax-only -ferror-limit=0 \
    -Wno-tautological-compare -Wno-unused-value -Wno-invalid-noreturn \
    -DRUNTIME_IL2CPP -DIL2CPP_MONO_DEBUGGER_DISABLED -DIL2CPP_DEBUG=0 -DNDEBUG \
    -DBASELIB_INLINE_NAMESPACE=il2cpp_baselib \
    -I$SRC -I$HDR/pch -I$HDR \
    -I$HDR/external/baselib/Include \
    -I$HDR/external/baselib/Platforms/Linux/Include \
    "$f" > "$OUT/$f.log" 2>&1
  echo "$?" > "$OUT/$f.exit"
done
echo DONE > $OUT/_complete
