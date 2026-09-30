#!/bin/bash
# Native22: compile EVERY generated translation unit, not only the 21 game shards.
# Corrected include paths: baselib lives at IL2CPP/external/baselib, NOT
# IL2CPP/libil2cpp/external/baselib (the path recorded in the Native21
# invocation.json did not exist and was silently ignored by clang). With the
# wrong path the Generic*/Generics* units die on "Baselib.h file not found"
# before any real diagnostic is produced.
set -u
ROOT=${ROOT:-/workspaces/GodsPVZ-native19}
SRC=$ROOT/.validation/native21/replay/canary/Library/Bee/artifacts/iOS/il2cppOutput/cpp
BASE=$ROOT/.validation/xcode-native18/expanded/xcode/Il2CppOutputProject/IL2CPP
HDR=$BASE/libil2cpp
OUT=${OUT:-$ROOT/.validation/native22-full2}
PAR=${PAR:-4}
mkdir -p "$OUT"
cd "$SRC" || exit 1
ls *.cpp | xargs -P "$PAR" -I{} bash -c '
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
date +%s > "$OUT/_complete"
echo "done -> $OUT"
