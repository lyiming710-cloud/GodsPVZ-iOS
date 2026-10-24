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

Recovery now includes compiler repair, actual Editor startup diagnosis, restored original serialization markers and two custom shaders. [Startup R11](Recovery/Startup-R11-2026-10-25/README.md) legalizes 71 exact pure field/API accesses across 47 methods, passing 532 isolated operation cases and rerunning all 5,583 prior controlled native/CIL records across 13 methods. Access failures fall from 216 sites/111 methods to 145 sites/70 methods; 131 frozen type failures or unsupported cases remain held. R11 directly checks JIT preparation of its 47 changed methods; its [R10 parent](Recovery/Startup-R10-2026-10-25/README.md) has zero direct JIT rejections (50 open generics unsupported) and passed fresh IL2CPP plus all 756 C++/274 C Linux compiler checks in [run 40120380134](https://github.com/lyiming710-cloud/GodsPVZ-iOS/actions/runs/40120380134). Each generated source hash and staged R10 input was independently read back. The earlier [R8 checkpoint](Recovery/Startup-R8-2026-10-25/README.md) passed actual MainMenu Editor startup and Metal shader compilation in run 40115001552. These separate scopes do not establish a working iPad game. Updated IPA and device acceptance for the reported black screen/flicker remain pending.

See `Recovery/STATUS.md` for the detailed recovery state.
