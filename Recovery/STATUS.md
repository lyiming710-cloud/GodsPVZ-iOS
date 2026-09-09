# Recovery status

## Current strategy

Use PC x86-64 IL2CPP as the primary gameplay source, Android native second, then original metadata/assets/JSON. Cpp2IL/ILSpy are attribution and managed-reconstruction aids, not authoritative source.

A stage is not final until original native attribution, formal-input SHA validation, deterministic independent patching, Cecil reopen, permanent RecoveryAudit OPEN1/OPEN2, fixed ILSpy readback, whole-assembly semantic isolation, GitHub Evidence, Google Drive archive, provider readback and STATUS closure all pass.

## Fixed original baseline

- Unity `2022.3.44f1c1`; metadata `31.1`.
- PC CodeRegistration / MetadataRegistration `0x1815E88C0` / `0x1818C6D00`.
- PC ZIP SHA-256 `2f08b4e2243e2bd296f3db694b4a2c9656252d76202c80167bdf3340b9f65b48`.
- PC GameAssembly SHA-256 `9ebd7ca996a5b03fb4a766f7a2502b660d581ddbbb36af4d7bf2f06a211da39d`.
- PC metadata SHA-256 `ad992341add498bd1018ac980f42171e0737b0793bf945c8714ad70a445b36b9`.
- Android APK SHA-256 `428e0ba2e46645a905fb9fbb00cfde42406727a3ddfce9a7df1e0875889a739f`.
- Cpp2IL source commit `5fb20304df698ffd3d0e664b2a698cd911dc9d57`; reproduced PC baseline `2318/2319`, sole full failure closed in HF3.
- Native attribution rule: original MethodDef RID-1 -> Assembly-CSharp CodeGenModule methodPointers index.
- Fixed ILSpyCmd / ICSharpCode.Decompiler `11.0.0.9375`, reproduced 56-DLL fixed-Cpp2IL reference set, reference ZIP SHA `fe091e5a5389c2491eebeff72c83c394097bef67ffd45a25cf7de01c9aad9ef1`.

## Final cumulative HF chain

- HF1 `31e6f6a781c6350a99a7317cade6cebc11cacd562a4c129961eef2d53af1d471`
- HF2 `a21c9e1d1e6994eb559694a51123d33b8a1eb3b262bda6d372f97bbf51523633`
- HF3 `23014656af797490bc35d9feb8f78950bcfa79a167c6a1de3303e1d30251c32f`
- HF4 `2abbc9eb02b93bcd0178091bbc38ca450b6162875df9dcb55e874d5b9af9f0a6`
- HF5 `58fe2001b6d9df986a08a930e60ea936fbf9392d82532f2e815bda63557d5e3a`
- HF6 `e6395652d0d413fbb496dc9cae8f2712d6a65bcfad109d160818983cde234e75`
- HF7 `ed01e0aa7dac968e8203131e6274a4da718ebe5fdcd0dcb3d3bc41d3b0f60ea4`
- HF8 `4dc9b2486ae41b2d9c340293dada22e74ee6877290e0e79f456b770b92bb4688`
- HF9 `c3e1716bbd598a8b2d2af3b1b5907869cc581271645ccc2a9e81595dfac07ec2`
- HF10 `0684c52733afd5dd1d45e455ede8ec5355d8b53bb65875506cb97f9a29f07b8a`
- HF11 `c990a8be0a615cbd1aed15b08251ae892889c056badc3e4070d7c6c24a6cb07f`
- HF12 `963f24a8323025e424c2e392d4f90ab1d7d48a8f5aa0b671775ac64402788930`
- HF13 `a389fcf0f6a97b4ace5cc580fb9704fa9830dbeba9b50edc4c5cad90489ebac3`
- HF14 `cbe30c99a973f41bbf4bb0f0f56f62d37e1c752c0723159cc07feeb3d2a0efe7`
- HF15 `87c74aea233372a5a7af28c0df244a18873af111c0230b54d9acf82e68ef9ec1`
- HF16 `57100e6b9296f5a6c1ff4710cc2859810dcfff9992a27ea5238000729c3888c3`
- HF17 `0eb0eba10cb27c5cff61e1a75947f146f5213ec036ff2ca3f95cf7d406ff1a67`
- HF18 `0e1b1acffa3329b3f98d2b61fb346c3f6647bee84e38bfab1d067a7aa17d58ed`
- HF19 `e6e303c660b0351611370ebe29746954c5535344160c6e580d9314405b67f720`
- HF20 `b16b3b89fad8f8081d86611a732696c56db6bd2dbab6302b6cef7fa94e141b3e`
- HF21 `888cab48b5e488ca05ed8ec58a11503c9f9a8ce0bac23a60c933f8628834e8f0`
- HF22 `502d6d61c18c17e29acb2f96451e9e41f98a403d8f8e915a76285c5b51f04997`
- HF23 `35d13b0d7fc5f82e04b837b812fab501a933b00e21f3431b2f6759c8e0c2b39c`
- HF24 `bdf1c0802685fa56a43602c7a8e45a30b48593b2903e99715a260473f5a09577`
- HF25 `eb0823deff8289d152af52ab89b91672b64bb786701d31d4871ebfcdbe9f9b71`
- HF26 `afa0e052902139c09cee9c715fe9a75c6f124af3af8a5647c799dc6b457fefac`
- HF27 `18d9efebdcfcccbbe6c805a9276c4427b4e6baa33673fb71b96a04245bb0e8de`
- HF28 `8e342bcf856b638d551e286034a5acf32d46a8a07e35e48333f480499bd705dd`
- **HF29 Projectile Zombie Collision Detector — native-backed formal final — `8c13a6638276a0e201ee39192545bbbe73703a7251f4188080dcfcf0c4d55590`.**

## Recent retained results

### HF23 — Zombie Hurt Core

Restores exactly `0x06000456–0x0600045E` Hurt_Armor1/2, Artillery, Ashes, Body, FinalDamageReduction, Normal, Real, Throughout. Accepted semantics include native armor/body DEF, non-real 0.1 floor, real bypass, `MathF.Round`, PoleCommander cap 0.95, iceCube fire_ice 0.2, throughout truncation and correct armor/body side effects.

### HF24 — Projectile Runtime Core

Restores exactly `0x060003D2 Projectile.Update()`: sorting-order truncation, pause/start gates, movement tracks 3/4/5/6/7, native-order delta-time integration, shadow sync, out-of-map destruction, CollisionDetect, tracking/rotation, LightSaber cleanup and previousPosition.

### HF25 — Collision Dispatch / Ground

Restores exactly `0x060003E0 CollisionDetect()` and `0x060003E2 CollisionDetect_Ground()`: hitType routing, camp gates, ground threshold 240 only for IDs 29/30, native unordered comparison behavior, ground AreaDamage ID set, then Collision_AudioParticle + DestroyProjectile.

### HF26 — Collision Audio / Particle

Restores exactly `0x060003EB Collision_AudioParticle()` from PC `0x18037A410`: native particle/audio switches, positions/scales/shakes, sorting layer, ID30 Animator, clip/volume/pitch rules, ID0/1 RNG order and ID30 double audio.

### HF27 — Collision Resolution Core

Restores exactly `0x060003E6 Collision_Device(Device)` and `0x060003EA Collision_Zombie(Zombie)`. Accepted AreaDamage ID sets, direct TakeDamage fallback, Collision_AudioParticle post-processing and 19/23 projectile-preservation rules.

### HF28 — Projectile Plant Collision Detector

Restores exactly `0x060003E3 CollisionDetect_Plant(bool sameCamp)` from PC `0x1803799B0`: Plant list enumeration/disposal, ordered X/Y AABB, camp/sameCamp gate, Z/H overlap, first match resolution, AreaDamage ID set, post-hit audio/particle and 19/23 projectile-preservation rule. Final Drive DLL `1T-UTvHpTZbhSs49DBjwWYz1qK_YCeOQF`; provider closure 22 files.

### HF29 — Projectile Zombie Collision Detector

Restores exactly `0x060003E4 CollisionDetect_Zombie(bool sameCamp)` — RID 996 — PC `0x180379D00`.

Accepted behavior:

- ordinary IDs enumerate `board.zombieManager.zombieList` with disposal, require `CanAttacked`, camp/sameCamp, ordered X/Y AABB and Z/H overlap, then dispatch the first match to `Collision_Zombie`;
- IDs 19/23 use indexed iteration plus `GetPredictedPosition(0f)` for current-frame position;
- native current-frame COMISS/JAE rejection semantics are kept distinct from previous-frame overlap semantics;
- persistent previous-frame overlap is suppressed using Zombie/Projectile `previousPosition`; only new contact dispatches `Collision_Zombie`;
- inactive projectile after resolution stops special-path iteration.

HF29 formal validation:

- formal HF28 re-fetched and SHA-verified `8e342bcf856b638d551e286034a5acf32d46a8a07e35e48333f480499bd705dd`;
- patcher workflow `34346510664` PASS, artifact SHA `83632ffd83dcc4c335926101fa757414458edf2f7b87ca93eb575e7690e34d67`;
- deterministic double patch -> final SHA `8c13a6638276a0e201ee39192545bbbe73703a7251f4188080dcfcf0c4d55590`;
- Cecil reopen E4 `252 IL / 727 bytes / 1 EH`;
- fixed ILSpy 11.0.0.9375 member readback PASS;
- HF28 whole IL reproduced exactly; HF29 whole IL SHA `4baa4fc78ed28ca7ed903e3c80895a358adb9eb2ab25d77ee0cc09a1549953a4`;
- MethodDef `2317 -> 2317`, normalized non-method skeleton identical, exactly E4 changed;
- semantic diff SHA `86f00e9e65ae591a29dd63f0c796bca08bec29e887f14fb8f3ced56e239e3052`;
- RecoveryAudit commit `76a97ca21c9bd973f76bb658ead398ad63175749`, workflow `34347012719` PASS, artifact SHA `43285cc5a7e47c65817fcb3e4d9b85756c413a875370f50ec621e3cdf2aef2ac`;
- OPEN1/OPEN2 `320 types / 2317 methods / 2297 bodies`, E4 `252/727`, `RECOVERY_AUDIT_OK`.

HF29 Drive archive:

- folder `1O6A2gXb983Z9-JDQWrI6DNTH27E05Xyb`;
- cumulative audited DLL `1oQ8nG22Y4isLLlx8p9IkREwRDql0TVg1`;
- patcher `19YVeHpF2OQNWQcY72ovwit8PfpmXEPw5`;
- RecoveryAudit `13Y4SQ-E6ddzUbEYwbz_xPU9K_KLWNi0I`;
- fixed ILSpy `14P_ve19i1cUMO9P2fYQwNZ5GRuCj7EvK`;
- native evidence `1jPVmFMVcF5SOo472PmMGgOJWYjiTxzBW`;
- payload manifest `1hc-k482JPIe0gRbDC2vJvZdbfpsPx-l3`, SHA `e1fceb7193a29b84eb1492d9d90de7673f8bd958279c01f265163846f406e4a4`;
- Evidence-FINAL `1ZfzRa3dCnZDUS7AA946YN00X8mvgJrAg`, SHA `8798bd15d3fe5ef048a7676b51530e9b693d9e4da6c9c93598fcbc3439cd2c08`;
- SHA256SUMS-FINAL `1ztmpGKao9WYnGGkG9GN4cqoVngfEfAGh`, SHA `597986324dee080221c2da179de8f1da7ca62b2dcd7f21b0010d740cfaf55793`;
- provider final readback `has_more=false`, exactly 22 files = 20 payloads + 2 closure files.

Evidence: `Tools/HighFidelityPatch/Evidence/HF29-Projectile-Zombie-Collision-Detector.md`.

**HF29 formal acceptance: PASS. HF29 is now the only allowed formal input for any later cumulative HF stage.**

## Unity reconstruction state

AssetRipper ~3,149 objects; reconstructed project ~6,783 files. 173 game script types and 263 refs across 114 assets migrated. MainMenu/Board scenes restored. Fixed package set includes UGUI 1.0.0, TMP 3.0.6, Core/URP 14.0.11, 2D Animation 9.1.1, Tilemap Extras 3.1.2, Burst 1.8.17, Collections 1.2.4, Mathematics 1.2.6, Visual Scripting 1.9.4. 67 package script types still require 67/67 Unity validation.

## Current decision gate — do not auto-open HF30

HF29 closes the Zombie detector. Re-run the remaining native-vs-managed active-path scan on the final unrecovered dispatcher target:

- `Projectile.CollisionDetect_Device` `0x060003E1` — PC `0x180379180`.

Open HF30 only when both are true:

1. original PC native/metadata proves concrete managed loss or mis-reconstruction; and
2. the method is materially active in gameplay.

Do not include neighboring methods by MethodDef adjacency or warning count alone. If E1 does not pass both gates, stop HF recovery and proceed to Unity/package validation, recovered Assembly-CSharp integration, compile/scene/gameplay validation and only then necessary iOS adaptation.
