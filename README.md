# GodsPVZ iOS recovery

Reverse-recovery workspace for the GodsPVZ 1.0.2 Unity IL2CPP builds supplied by the repository owner.

Next-batch cadence: **100 actual distinct method changes before shared testing**, effective for R19 from the fixed R18 candidate. See [batch policy](Recovery/BATCH-POLICY.json); historical fifty-method reports retain their original scope.

## Confirmed source build

- Unity: `2022.3.44f1c1`
- Android package: `com.tipsGodsStudio.godsPVZ`
- IL2CPP metadata: `31.1`
- Android CodeRegistration: `0x2772B68`
- Android MetadataRegistration: `0x285F870`
- Recovered `Assembly-CSharp.dll`: 2317 MethodDef rows, 2297 methods with CIL bodies, about 1.29 MB of CIL code
- Cpp2IL reported `2319 / 2319` target methods successfully recovered
- 173 serialized game script types have been mapped from AssetRipper `.cs` GUIDs to the recovered Assembly-CSharp managed DLL local file IDs

## Exact Unity package versions recovered from embedded build paths

- `com.unity.ugui@1.0.0`
- `com.unity.textmeshpro@3.0.6`
- `com.unity.render-pipelines.core@14.0.11`
- `com.unity.render-pipelines.universal@14.0.11`
- `com.unity.2d.animation@9.1.1`
- `com.unity.2d.tilemap.extras@3.1.2`
- `com.unity.burst@1.8.17`
- `com.unity.collections@1.2.4`
- `com.unity.mathematics@1.2.6`
- `com.unity.visualscripting@1.9.4`

## Current phase

Recovery now includes compiler repair, original-native execution, real Editor diagnosis and shader restoration. Latest [R19 Batch100](Recovery/Startup-R19-Batch100-2026-10-25/README.md) has 100 actual source changes before shared tests. Exactly 2,217 other MethodDefs and all old metadata remain intact; member-access audit has zero rejections. Fifty methods now have 11,096 finite original-native comparisons, including direct-DLL/complete-CIL controlled profiles and declared-field snapshots. Fifty other runtime profiles and full-batch qualification remain pending. Fifty physical fault variants differ; three overwritten-store faults remain undetected and explicitly archived. Current exact candidate9 passes fresh IL2CPP and all 1,029 Linux compiler units; every returned source/log is rechecked. Whole actual-DLL JIT has 2,247 prepared/fifty unsupported/zero rejected. A 22-caller operation audit is recorded separately from the fifty finite behavior profiles. Apple checking is prepared. No updated IPA or iPad black-screen/flicker acceptance is claimed.

See `Recovery/STATUS.md` for the detailed recovery state.
