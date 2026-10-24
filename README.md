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

Recovery now includes compiler repair, actual Editor startup diagnosis, restored original serialization markers and two custom shaders. Latest [Startup R13](Recovery/Startup-R13-2026-10-25/README.md) restores eight float methods, including two lost jump-table getters, DamageText receiver/hash order and Zombie.GetTOU's original custom rounding. All 13,167 new and 5,673 replayed controlled native/CIL records match on exact R13: 18,840 records across 24 methods. Independent raw/CIL audits preserve the other 2,309 MethodDefs. Frozen type failures or unsupported cases fall to 120; access still rejects 145 sites/70 methods. A new read-only census flags 119 conservative decompiler-marker leads, including 81 methods passing type checks; these are not proven bug counts. R13 compiler-only validation is prepared separately. Exact R11 passed IL2CPP and 756 C++/274 C Linux checks; exact R10 separately passed Apple arm64 syntax checks of all 1,030 generated sources. The earlier R8 checkpoint passed real MainMenu Editor startup and Metal shader compilation. All prior results stay bound to their inputs. Updated IPA and iPad acceptance for the reported black screen/flicker remain pending.

See `Recovery/STATUS.md` for the detailed recovery state.
