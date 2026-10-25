# GodsPVZ iOS recovery

Reverse-recovery workspace for the GodsPVZ 1.0.2 Unity IL2CPP builds supplied by the repository owner.

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

Recovery now includes compiler repair, original-native execution, real Editor diagnosis and shader restoration. Latest [Startup R18 Batch50](Recovery/Startup-R18-Batch50-2026-10-25/README.md) constructs fifty new methods before shared testing. All 2,521 native/full-CIL records match; complementary runtime/type/access checks detect all fifty exact historical-body reversions. Direct real-DLL invocation has 74 passes across three methods plus sixteen retained engine-cctor blocks. Other 2,267 MethodDefs and old metadata remain intact, appending one legal public getter reference and increasing eight target stack headers. Wider checks retain 97 type failures or unsupported items and 57 access sites/twelve methods. R18 platform compiler checks and updated IPA/device acceptance are pending; exact R17 compiler evidence stays separate.

See `Recovery/STATUS.md` for the detailed recovery state.
