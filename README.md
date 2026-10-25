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

Recovery now includes compiler repair, real Editor startup diagnosis, original serialization markers and restored shaders. Latest [Startup R17 Batch50](Recovery/Startup-R17-Batch50-2026-10-25/README.md) constructs fifty new distinct methods before shared testing. Final 1,251 native/CIL records match after preserving and correcting six text-capture failures; 49 semantic mutants differ at runtime and one width-only fault is rejected independently. A direct actual-DLL supplement passes 235 executions over 35 methods. All other 2,267 MethodDefs and old metadata prefixes remain intact; no visibility/readonly changes. Full type checks retain 113 failure-or-unsupported methods; access retains 108 sites/36 methods. Exact R16 separately passed fresh IL2CPP and all 1,030 Linux source checks with hashes read back. Updated IPA and iPad black-screen/flicker acceptance remain pending.

See `Recovery/STATUS.md` for the detailed recovery state.
