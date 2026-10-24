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

Recovery now includes compiler repair, actual Editor startup diagnosis, restored original serialization markers and two custom shaders. Latest [Startup R12](Recovery/Startup-R12-2026-10-25/README.md) restores three complete native initializer trampolines and passes 90 controlled native/CIL cases, preserving all other bytes. Frozen type failures or unsupported cases fall to 128; access still rejects 145 sites across 70 methods. Its [R11 parent](Recovery/Startup-R11-2026-10-25/README.md) legalizes 71 pure accesses across 47 methods, passes 532 isolated operation cases and reruns all 5,583 prior controlled native/CIL records across 13 methods. Exact R11 passed fresh IL2CPP plus 756 C++/274 C Linux checks in [run 40121851208](https://github.com/lyiming710-cloud/GodsPVZ-iOS/actions/runs/40121851208). Exact [R10](Recovery/Startup-R10-2026-10-25/README.md) separately passed all 1,030 source files under physical Apple arm64 syntax checks in [run 40121853897](https://github.com/lyiming710-cloud/GodsPVZ-iOS/actions/runs/40121853897); 17 generated files differ from R11, so that Apple result is not silently inherited. R12 has no fresh compiler/player export yet. The earlier [R8 checkpoint](Recovery/Startup-R8-2026-10-25/README.md) passed real MainMenu Editor startup and Metal shader compilation. These separate scopes do not establish a working iPad game. Updated IPA and device acceptance for the reported black screen/flicker remain pending.

See `Recovery/STATUS.md` for the detailed recovery state.
