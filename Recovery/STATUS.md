# Recovery status

## Current strategy

Use PC x86-64 IL2CPP as the primary gameplay source, Android native second, then original metadata/assets/JSON; use Cpp2IL/ILSpy only for attribution and managed reconstruction support. Do not substitute rescue-route approximations for native-backed gameplay.

High-fidelity-first remains mandatory: a stage is not final until original native attribution, formal-input SHA validation, deterministic independent patching, Cecil reopen, permanent RecoveryAudit OPEN1/OPEN2, fixed ILSpy readback, whole-assembly semantic isolation, GitHub Evidence, Google Drive archive, provider readback, and STATUS closure all pass.

## Fixed original baseline

- Unity `2022.3.44f1c1`; metadata `31.1`.
- PC CodeRegistration / MetadataRegistration `0x1815E88C0` / `0x1818C6D00`.
- PC ZIP SHA-256 `2f08b4e2243e2bd296f3db694b4a2c9656252d76202c80167bdf3340b9f65b48`.
- PC GameAssembly SHA-256 `9ebd7ca996a5b03fb4a766f7a2502b660d581ddbbb36af4d7bf2f06a211da39d`.
- PC metadata SHA-256 `ad992341add498bd1018ac980f42171e0737b0793bf945c8714ad70a445b36b9`.
- Android APK SHA-256 `428e0ba2e46645a905fb9fbb00cfde42406727a3ddfce9a7df1e0875889a739f`.
- Fixed Cpp2IL source commit `5fb20304df698ffd3d0e664b2a698cd911dc9d57`; reproduced PC baseline `2318/2319`, sole full failure closed in HF3.
- Native attribution rule: original MethodDef RID-1 -> Assembly-CSharp CodeGenModule methodPointers index.
- Fixed ILSpyCmd / ICSharpCode.Decompiler `11.0.0.9375` with reproduced 56-DLL fixed-Cpp2IL reference set, reference ZIP SHA-256 `fe091e5a5389c2491eebeff72c83c394097bef67ffd45a25cf7de01c9aad9ef1`.

## Final cumulative HF chain

- HF1 `31e6f6a781c6350a99a7317cade6cebc11cacd562a4c129961eef2d53af1d471`.
- HF2 audited `a21c9e1d1e6994eb559694a51123d33b8a1eb3b262bda6d372f97bbf51523633`.
- HF3 `23014656af797490bc35d9feb8f78950bcfa79a167c6a1de3303e1d30251c32f`.
- HF4 `2abbc9eb02b93bcd0178091bbc38ca450b6162875df9dcb55e874d5b9af9f0a6`.
- HF5 `58fe2001b6d9df986a08a930e60ea936fbf9392d82532f2e815bda63557d5e3a`.
- HF6 `e6395652d0d413fbb496dc9cae8f2712d6a65bcfad109d160818983cde234e75`.
- HF7 `ed01e0aa7dac968e8203131e6274a4da718ebe5fdcd0dcb3d3bc41d3b0f60ea4`.
- HF8 `4dc9b2486ae41b2d9c340293dada22e74ee6877290e0e79f456b770b92bb4688`.
- HF9 `c3e1716bbd598a8b2d2af3b1b5907869cc581271645ccc2a9e81595dfac07ec2`.
- HF10 `0684c52733afd5dd1d45e455ede8ec5355d8b53bb65875506cb97f9a29f07b8a`.
- HF11 `c990a8be0a615cbd1aed15b08251ae892889c056badc3e4070d7c6c24a6cb07f`.
- HF12 `963f24a8323025e424c2e392d4f90ab1d7d48a8f5aa0b671775ac64402788930`.
- HF13 `a389fcf0f6a97b4ace5cc580fb9704fa9830dbeba9b50edc4c5cad90489ebac3`.
- HF14 `cbe30c99a973f41bbf4bb0f0f56f62d37e1c752c0723159cc07feeb3d2a0efe7`.
- HF15 `87c74aea233372a5a7af28c0df244a18873af111c0230b54d9acf82e68ef9ec1`.
- HF16 `57100e6b9296f5a6c1ff4710cc2859810dcfff9992a27ea5238000729c3888c3`.
- HF17 `0eb0eba10cb27c5cff61e1a75947f146f5213ec036ff2ca3f95cf7d406ff1a67`.
- HF18 Buff infrastructure `0e1b1acffa3329b3f98d2b61fb346c3f6647bee84e38bfab1d067a7aa17d58ed`.
- HF19 Zombie predicates `e6e303c660b0351611370ebe29746954c5535344160c6e580d9314405b67f720`.
- HF20 Plant damage pipeline `b16b3b89fad8f8081d86611a732696c56db6bd2dbab6302b6cef7fa94e141b3e`.
- HF21 Projectile / Resource cluster `888cab48b5e488ca05ed8ec58a11503c9f9a8ce0bac23a60c933f8628834e8f0`.
- HF22 Damage Dispatch / Area Core `502d6d61c18c17e29acb2f96451e9e41f98a403d8f8e915a76285c5b51f04997`.
- HF23 Zombie Hurt Core `35d13b0d7fc5f82e04b837b812fab501a933b00e21f3431b2f6759c8e0c2b39c`.
- HF24 Projectile Runtime Core `bdf1c0802685fa56a43602c7a8e45a30b48593b2903e99715a260473f5a09577`.
- HF25 Projectile Collision Dispatch / Ground `eb0823deff8289d152af52ab89b91672b64bb786701d31d4871ebfcdbe9f9b71`.
- HF26 Projectile Collision Audio / Particle `afa0e052902139c09cee9c715fe9a75c6f124af3af8a5647c799dc6b457fefac`.
- **HF27 Projectile Collision Resolution Core — native-backed formal final — `18d9efebdcfcccbbe6c805a9276c4427b4e6baa33673fb71b96a04245bb0e8de`.**

## Retained recent recovery results

### HF23 — Zombie Hurt Core

Restores exactly `0x06000456` through `0x0600045E`: Hurt_Armor1, Hurt_Armor2, Hurt_Artillery, Hurt_Ashes, Hurt_Body, Hurt_FinalDamageReduction, Hurt_Normal, Hurt_Real, Hurt_Throughout. Accepted semantics include native armor/body defense, 0.1 non-real floor, real bypass, `MathF.Round`, PoleCommander.speed cap 0.95, iceCube fire_ice 0.2, throughout truncation, Artillery/Real distinctions, Ashes lethal behavior, armor presentation/audio and Body damage text. Drive final DLL `1qcGGHQZuVe5s60jje-dIJFzHVH-gkD7M`.

### HF24 — Projectile Runtime Core

Restores exactly `0x060003D2 Projectile.Update()` from PC `0x18037D710`. Accepted behavior includes sorting-order truncation, board/pause gates, movement tracks 3/4/5/6/7, native-order `Time.deltaTime` integration, shadow synchronization, out-of-map destruction, active-frame `CollisionDetect`, tracking/rotation, LightSaber cleanup and previousPosition update. Drive final DLL `1UIzsJRqKwT0R-xCSaWqCG6CtenvPj2he`.

### HF25 — Projectile Collision Dispatch / Ground

Restores exactly `0x060003E0 CollisionDetect()` and `0x060003E2 CollisionDetect_Ground()`. Accepted behavior includes native hitType 0/1/2/3 routing/camp gates, ground threshold 240 only for IDs 29/30, native unordered/NaN comparison semantics, ground AreaDamage IDs `9-11,15,26-32`, then `Collision_AudioParticle()` + `DestroyProjectile()`. Drive final DLL `1ENIzUe4qD4qdSBGXACyzQ2CmMIwja2i5`.

### HF26 — Projectile Collision Audio / Particle

Restores exactly `0x060003EB Collision_AudioParticle()` from PC `0x18037A410`. Accepted behavior includes native particle/audio ID switches, particle positions/scales/shakes/sorting layer, ID30 Animator type, native audio clip indices/volume/pitch, ID0/1 RNG ordering and ID30 two separate audio calls. Drive final DLL `1yc82VM4pK5DdIlu7-qS9VLdcGDsy-ti_`. Provider final readback verified 22 files = 20 payloads + 2 closure files. Evidence: `Tools/HighFidelityPatch/Evidence/HF26-Projectile-Collision-Audio-Particle.md`.

### HF27 — Projectile Collision Resolution Core

HF27 restores exactly:

- `0x060003E6 Projectile.Collision_Device(Device)` — RID 998 — PC `0x18037B010`;
- `0x060003EA Projectile.Collision_Zombie(Zombie)` — RID 1002 — PC `0x18037B170`.

Accepted E6 behavior: IDs `{10,11,15,26-32}` use `damage.AreaDamage()`, otherwise `device.TakeDamage(damage,this)`; then `Collision_AudioParticle()`; IDs 19/23 preserve the projectile, all others destroy it.

Accepted EA behavior: IDs `{10,11,15,26,27,28,31,32}` use `damage.AreaDamage()`, otherwise `zombie.TakeDamage(damage,this)`; then `Collision_AudioParticle()`; IDs 19/23 preserve the projectile, all others destroy it.

HF27 formal validation:

- formal HF26 re-fetched from Drive and SHA-verified `afa0e052902139c09cee9c715fe9a75c6f124af3af8a5647c799dc6b457fefac`;
- patcher workflow `34323560548` PASS, artifact SHA `9fc93909009beafa088f17504fc0e78288bb2432be41d878bbb574a76ba38479`;
- deterministic double patch -> final SHA `18d9efebdcfcccbbe6c805a9276c4427b4e6baa33673fb71b96a04245bb0e8de`;
- Cecil reopen E6 `43 IL / 110 bytes / 0 EH`, EA `55 IL / 140 bytes / 0 EH`;
- fixed ILSpy 11.0.0.9375 member readback 2/2 PASS;
- HF26 whole IL reproduced, HF27 whole IL SHA `33260b8a10a7b51ccf19366420ef50bd3fd24c10dee91628db98107902f5b993`;
- MethodDef `2317 -> 2317`, normalized non-method skeleton identical, exactly E6/EA changed;
- semantic diff SHA `7238e4a4873917787a976c3e9255ec2bb11176591dcc940ffc01e2374c10676f`;
- permanent RecoveryAudit commit `b4d96066aecc5a1fb5f29d13ff3380530b163d72`, workflow `34324427275` PASS, artifact SHA `17d60177a8b82bc3ecd162813bc9f414dc21798c0990ad5c1da74c6fc6155c61`;
- OPEN1/OPEN2 `320 types / 2317 methods / 2297 bodies`, E6 `43/110`, EA `55/140`, `RECOVERY_AUDIT_OK`.

HF27 Drive archive:

- folder `13_2KR3QSuTAjthqiMyd3scOL-oOMOawN`;
- cumulative audited DLL `1eS4hAmbRZKOO6oVzS8Tj7n1arpbconP6`;
- patcher `1EBcpQCvSI3qi-xgiZczTkyS3HdkSjqFU`;
- RecoveryAudit `1Q7faENFReWN8wmP307DIq1jdBtjRnWTM`;
- fixed ILSpy `1uEKA74lshuJqLO7mTLmasBLN-ANZ6zo5`;
- native evidence `1Je7MYXbmxs5L2N8NCt6JecBPcW5S9MlD`;
- payload manifest `1sFinNdM1CAxvpD0f2AxBdZIG0sGyhKk6`, SHA `4b80595844a198abe372732fd433708224dd9502df8f2d9042d0a49c9f061351`;
- Evidence-FINAL `1H7HgEQBfg2QKZKTnH03mu2tYMu6OWKIA`, SHA `05f883018b5031ff55d4da29b3d142b1d387977cb0db89f8ebdf27e7ef32d98f`;
- SHA256SUMS-FINAL `1CHAe372DCQGHft1VFEyHAARamw_eRuB6`, SHA `332fb1fbc9d835cc8c5b427d4c3baec3d109ad411f44ed58fa235e58ae7e7691`;
- provider final readback `has_more=false`, exactly 22 files = 20 payloads + 2 closure files.

GitHub Evidence: `Tools/HighFidelityPatch/Evidence/HF27-Projectile-Collision-Resolution-Core.md`.

**HF27 formal acceptance: PASS. HF27 is now the only allowed formal input for any later cumulative HF stage.**

## Unity reconstruction state

AssetRipper ~3,149 objects; reconstructed project ~6,783 files. 173 game script types and 263 refs across 114 assets migrated. MainMenu/Board scenes restored. Fixed package set: UGUI 1.0.0, TMP 3.0.6, Core/URP 14.0.11, 2D Animation 9.1.1, Tilemap Extras 3.1.2, Burst 1.8.17, Collections 1.2.4, Mathematics 1.2.6, Visual Scripting 1.9.4. 67 package script types still require 67/67 Unity validation.

## Current decision gate — do not auto-open HF28

HF27 closes target-resolution handlers E6/EA. Do **not** open HF28 merely because adjacent Projectile methods contain Cpp2IL warnings or because they are MethodDef neighbors.

Rerun the remaining native-vs-managed active-path scan over:

- `Projectile.CollisionDetect_Device` `0x060003E1` — PC `0x180379180`;
- `Projectile.CollisionDetect_Plant` `0x060003E3` — PC `0x1803799B0`;
- `Projectile.CollisionDetect_Zombie` `0x060003E4` — PC `0x180379D00`.

Include a helper/resolution method only when direct native call evidence proves it belongs to the active business path and its managed body has concrete loss. MethodDef adjacency and warning count alone are insufficient.

A later HF stage is allowed only when both conditions hold:

1. concrete managed loss/mis-reconstruction is proven from original PC native/metadata evidence; and
2. the affected method is materially active in gameplay.

If no remaining detector/helper cluster satisfies both conditions, stop HF recovery and proceed to Unity `2022.3.44f1c1`, package restoration, 67/67 package-script validation, recovered Assembly-CSharp integration, compile/scene/gameplay validation, then necessary iOS platform adaptation.
