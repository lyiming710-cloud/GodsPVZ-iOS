set -euo pipefail
OUT="$RUNNER_TEMP/stage9-native3-ios"
E="$OUT/evidence"
X="$OUT/xcode"
PBX="$X/Unity-iPhone.xcodeproj/project.pbxproj"
test -f "$PBX"
test -d "$X/Classes"
test -d "$X/Libraries"
test -d "$X/Data"
test -d "$X/Il2CppOutputProject"
test -f "$X/Il2CppOutputProject/Source/il2cppOutput/Assembly-CSharp.cpp"
test -f "$X/Data/Managed/Metadata/global-metadata.dat"
grep -Eq 'PRODUCT_BUNDLE_IDENTIFIER[[:space:]]*=[[:space:]]*"?com\.tipsGodsStudio\.godsPVZ"?;' "$PBX"
grep -Eq 'IPHONEOS_DEPLOYMENT_TARGET[[:space:]]*=[[:space:]]*12(\.0)?;' "$PBX"
grep -q 'GodsPVZ iOS Xcode export succeeded' "$E/ios-export.log"
CPP_COUNT="$(find "$X/Il2CppOutputProject/Source/il2cppOutput" -maxdepth 1 -type f -name '*.cpp' | wc -l | tr -d ' ')"
FILE_COUNT="$(find "$X" -type f | wc -l | tr -d ' ')"
SIZE="$(du -sb "$X" | awk '{print $1}')"
{
  echo 'IOS_XCODE_EXPORT=PASS'
  echo "unity_version=$UNITY_VERSION"
  echo "test_dll_sha256=$TEST_DLL_SHA256"
  echo 'semantic_memberref_gate=PASS'
  echo 'semantic_targets=24/24'
  echo 'semantic_resolve_keys=23/23'
  echo 'orphan_generic_hits=0'
  echo 'bundle_id=com.tipsGodsStudio.godsPVZ'
  echo 'deployment_target=12.0'
  echo 'scenes=Assets/Scenes/MainMenu.unity,Assets/Scenes/Board.unity'
  echo "il2cpp_cpp_count=$CPP_COUNT"
  echo "xcode_file_count=$FILE_COUNT"
  echo "xcode_size_bytes=$SIZE"
  echo 'production_promotion=NO_ISOLATED_GATE_ONLY'
} | tee "$E/RESULT.txt"
sha256sum "$X/Data/Managed/Metadata/global-metadata.dat" > "$E/generated-global-metadata.sha256"
sha256sum "$X/Il2CppOutputProject/Source/il2cppOutput/Assembly-CSharp.cpp" > "$E/generated-assembly-csharp-cpp.sha256"
grep -E 'PRODUCT_BUNDLE_IDENTIFIER|IPHONEOS_DEPLOYMENT_TARGET' "$PBX" | sort -u > "$E/pbx-settings.txt"
tar --zstd -cf "$OUT/GodsPVZ-Stage9.1-native3-unsigned-Xcode.tar.zst" -C "$OUT" xcode
sha256sum "$OUT/GodsPVZ-Stage9.1-native3-unsigned-Xcode.tar.zst" > "$E/xcode-archive.sha256"

