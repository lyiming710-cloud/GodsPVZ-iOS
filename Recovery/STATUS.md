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
- Assembly-CSharp MethodDef total must remain `2317`.

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
- HF29 `8c13a6638276a0e201ee39192545bbbe73703a7251f4188080dcfcf0c4d55590`
- HF30 `6ccccf685950edba428b51dbe3aa95e511caa467d134dd4202939e19ef655c24`
- HF31 `a4d001581430fe440e50feb37f3abd189a1ae86a00f4fe41114ae3f9b99386d2`
- HF32 `a959e450bd71beb67c948a9a85a1263a7739ccd37b2438405ae954fe10b7bf87`
- HF33 `6b467ef40f2e8e4e7fcff48d71c328457ac4107261a95fbd59908f5516720518`
- HF34 `ead8dee5def2818ca8c64c36b4fe65f1830dbf873fae34dbc84d2f07b103d999`
- HF35 `ffd3e858a6ea15c1203a8c543e800b9181e0ab115fa7ab6a84d00975be5b947b`
- **HF36 Armored Flag Wake-Up Core — native-backed formal final — `291e22bffefdf98f0fca29475c485264ad5b9eb6cf8a934226d1e73a187ddfb0`.**

## Recent retained results

### HF23 — Zombie Hurt Core

Restores exactly `0x06000456–0x0600045E` Hurt_Armor1/2, Artillery, Ashes, Body, FinalDamageReduction, Normal, Real, Throughout with native armor/body DEF, non-real 0.1 floor, real bypass, `MathF.Round`, PoleCommander cap 0.95, iceCube fire_ice 0.2 and throughout truncation.

### HF24 — Projectile Runtime Core

Restores exactly `0x060003D2 Projectile.Update()`: sorting-order truncation, pause/start gates, movement tracks 3/4/5/6/7, native-order delta-time integration, shadow sync, out-of-map destruction, CollisionDetect, tracking/rotation, LightSaber cleanup and previousPosition.

### HF25–HF30 — Projectile collision chain

HF25–HF30 formally restore `CollisionDetect`, Ground, AudioParticle, Device/Zombie resolution, and Plant/Zombie/Device detectors. The recovered collision dispatcher path is closed end-to-end into damage and hit post-processing.

### HF31 — Projectile Rotating Runtime Core

Restores exactly `0x060003F6 Projectile.Rotating()` — PC `0x18037BE20`. `Projectile.speed` is a real `Vector3`; native uses `speed.x` for facing, integrates angularSpeed with angularAcceleration, and rotates sprite/animation around world Z. HF31 final SHA `a4d001581430fe440e50feb37f3abd189a1ae86a00f4fe41114ae3f9b99386d2`; all formal gates and 20+2 closure passed.

### HF32 — Projectile Tracking Runtime Core

Restores exactly `0x060003D8 Projectile.Update_Tracking()` — RID `984`, PC `0x18037D4D0`: choose the enabled/attackable zombie with minimum `fX`, preserve foreach Dispose/finally, and call `Aim(target-projectile XY delta)`. HF32 final SHA `a959e450bd71beb67c948a9a85a1263a7739ccd37b2438405ae954fe10b7bf87`; all formal gates and 20+2 closure passed.

### HF33 — Projectile Aim / Euler Runtime Core

Restores exactly:

- `0x060003DC Projectile.Aim(Vector3)` — RID `988`, PC `0x180378D50`;
- `0x060003F8 Projectile.SetEulerAngles(float,float)` — RID `1016`, PC `0x18037C120`.

`Aim` computes the atan2 direction, preserves speed magnitude while redirecting the Vector3 and calls SetEulerAngles. `SetEulerAngles` preserves original angular/zAngular fields, computes a local effective z through native Approximately/Lerp logic, applies effective z to sprite/animation/track and original angular to shadow. HF33 final SHA `6b467ef40f2e8e4e7fcff48d71c328457ac4107261a95fbd59908f5516720518`; fixed ILSpy, semantic isolation, permanent RecoveryAudit and Drive 20+2 closure all passed.

### HF34 — Projectile Runtime Support Core

Restores exactly:

- `0x0600013E GlobalStaticVars.CreateAudioAtPoint(AudioClip,Vector3,float,float)` — RID `318`, PC `0x18031B350`;
- `0x060003D6 Projectile.Update_Time()` — RID `982`, PC `0x18037D220`;
- `0x060003DB Projectile.Update_MoveTrack7()` — RID `987`, PC `0x18037CCF0`;
- `0x06000450 Zombie.GetPredictedPosition(float)` — RID `1104`, PC `0x1803617F0`.

Accepted behavior:

- `CreateAudioAtPoint/4`: Unity-null clip returns null; create one-shot GameObject and AudioSource; assign position, clip, volume, pitch and spatialBlend=0; Play; destroy the temporary object after `clip.length * max(0.01f, Time.timeScale)`; return the AudioSource.
- `Update_Time`: integrate `livingTime` by `Time.deltaTime * updateRate`; preserve ID27/28 timed AreaDamage/Collision_AudioParticle/DestroyProjectile behavior; preserve ID21 periodic plant damage and LightSaber explosion timeout.
- `Update_MoveTrack7`: preserve native zSpeed equation and plant-centered rotating-offset speed computation.
- `GetPredictedPosition`: preserve `(fX,fY,fZ)` plus `(time + Time.deltaTime) * rSpeed` in X/Y with Z unchanged.

HF34 formal validation and Drive 20+2 closure passed. Evidence: `Tools/HighFidelityPatch/Evidence/HF34-Projectile-Runtime-Support-Core.md`, commit `edc56dd90e92131e2dba544bd0761f977ac7e712`.

### HF35 — Device Injury Status Core

Restores exactly `0x0600032B Device.InjuryStatusUpdate()` — original RID `811`, PC `0x180349280`.

Accepted behavior:

- original `COMISS 0,healthPoint` / `JAE` semantics invoke `Broken()` only for ordered `healthPoint <= 0`; unordered/NaN proceeds through the active body;
- non-broken processing is specific to Device `ID == 4`;
- damage fraction is `1f - healthPoint / maxHealthPoint` and each crossed `(brokenLevel + 1) / 3` threshold increments `brokenLevel`;
- each threshold updates the `Roadblock` sprite, creates RoadblockBroken particle 26 at this Device position, and plays clip 12 at camera position with `AudioVolume() * 1.6f`, pitch `0.7f`.

HF35 formal validation and Drive 20+2 closure passed. Evidence: `Tools/HighFidelityPatch/Evidence/HF35-Device-Injury-Status-Core.md`, commit `d08323ea70f0600bb23543108e1b4cea2d5e8327`.

### HF36 — Armored Flag Wake-Up Core

Restores exactly `0x0600048C Zombie.ZC_ArmoredFlagWakeUpZombies()` — original RID `1164`, PC `0x18036D560`.

Accepted behavior:

- if this zombie is `isStant && !immune_wakeUp && !hide`, call `Path_Finding()` then `TranToWalk()`;
- enumerate `board.zombieManager.zombieList`;
- for each zombie require `!immune_wakeUp && !hide && !IsDisabled() && isStant`, then call `Path_Finding()` and `TranToWalk()`;
- preserve list Enumerator Dispose/finally and original null behavior.

HF36 formal validation:

- formal HF35 input SHA-verified `ffd3e858a6ea15c1203a8c543e800b9181e0ab115fa7ab6a84d00975be5b947b`;
- patcher build head `7233686d52ee43149c45f76b35f09011809e6ecf`, workflow `34420563912` PASS, artifact ID `10130723306`, SHA `c34d9d8e78e10dad7edc60cb2c3d6fc107818bb82d0ab6bb1742530f152899d1`;
- independent double patch -> byte-identical `291e22bffefdf98f0fca29475c485264ad5b9eb6cf8a934226d1e73a187ddfb0`, stderr 0;
- Cecil reopen `50 IL / 140 bytes / 1 EH`;
- fixed ILSpy 11.0.0.9375 + reproduced 56-DLL member readback PASS, stderr 0, Cpp2IL refs 0, issue markers 0;
- HF35 whole IL reproduced `ca155db767d34a814ce322166216aa7c83a868e5d44743c25b71d2c120f22a9e`; HF36 whole IL `6a0c79a8c0b758c35d61e7edba8da5740dd72cb82426f976c064ad358b7d5391`;
- prior accepted HF34->HF35 semantic diff reproduced byte-for-byte first at `9c0dacb91256ecadf658ae35faba6474ee5f3f269e7d0f4b2b35b3cfacc22a2e`;
- MethodDef `2317 -> 2317`, normalized non-method skeleton byte-identical, changed exactly `0x0600048C`; HF35->HF36 semantic diff SHA `83f0cd40ea1d266d2f78972b18ccc177fd77a9b12792109e40a36db9e134250e`;
- permanent RecoveryAudit commit `623b6ad710577828261b4612b71c2e7fa6e6396b`, workflow `34420936171` PASS, artifact ID `10130857784`, SHA `163022dd32f85899b857ef96254851f7a5745edb85dc793d9edc6170257f2cff`;
- independent published auditor OPEN1/OPEN2 `320 types / 2317 methods / 2297 bodies`, target `50/140` both, stderr 0, terminal `RECOVERY_AUDIT_OK`.

HF36 Drive archive:

- folder `1PO7YbeLkbQt_Y5uhN3Xnx4Eg6BHu198z`;
- cumulative audited DLL `15eQIfAuueLIeWtncXBQWmxldDR3E0fmK`;
- patcher `1M8iC3VU9vaUZpdN53I7h_aCIGqQqzaC6`;
- RecoveryAudit `13V90m53NH6Xp3j48VNxwozhWZ4FcYjTf`;
- fixed ILSpy `1fk1qWTa4EYAmWlkpDS40_wBdxfHykfth`;
- native evidence `1MGxnOvGAUcIERM1M80re7Tx1fAVr15n-`;
- semantic diff `1j1oH-IDWZHWlGC6WmexhJPFb0oyQCZoI`;
- payload manifest `1rvAnrzZdWkLG6oUBQerOamT76BV8PjAt`, SHA `51185e6d4b21ca0be3084c2e80e92232909b450d2560596a1279c7566253a061`;
- Evidence-FINAL `1MSxLqf7CrXl-ewJeTOon2QGWQWeM62G9`, SHA `9cf4eb000af23d35200d409ea07f1a2017c3a6fe774fd38e57812302594cc708`;
- SHA256SUMS-FINAL `153TnPiEcY74J7nXocK2nZmiebwngzwzf`, SHA `022e7f9c3c6bc3f0552418a9ff81b03e08adc752d5a24cc24482813bb6ca1bbb`;
- final provider readback: exactly 22 files = 20 payloads + 2 closure files.

Evidence: `Tools/HighFidelityPatch/Evidence/HF36-Armored-Flag-Wake-Up-Core.md`, commit `6206eb4c2ca07f72edd108fe0f8e6168f0b36062`.

**HF36 formal acceptance: PASS. HF36 is now the only allowed formal input for any later cumulative HF stage.**

## Unity reconstruction state

AssetRipper ~3,149 objects; reconstructed project ~6,783 files. 173 game script types and 263 refs across 114 assets migrated. MainMenu/Board scenes restored. Fixed package set includes UGUI 1.0.0, TMP 3.0.6, Core/URP 14.0.11, 2D Animation 9.1.1, Tilemap Extras 3.1.2, Burst 1.8.17, Collections 1.2.4, Mathematics 1.2.6, Visual Scripting 1.9.4. 67 package script types still require 67/67 Unity validation.

## Current decision gate — do not auto-open HF37

HF36 closes the active armored-flag zombie wake-up path. Do not create HF37 merely because another MethodDef has warnings or unattractive recovered IL.

Re-run the original-native-vs-managed decision scan from the HF36 formal cumulative DLL. `Plant.KillEvent` remains a known special case: it must not enter a formal stage until shared native helper `0x1804A25F0` is behaviorally identified and every dependency closes; do not infer or omit that call.

Open HF37 only if both are independently proven:

1. original PC native/metadata demonstrates concrete managed loss or mis-reconstruction; and
2. the method is materially active in gameplay, with every required native dependency behaviorally closed.

Do not use warning count, MethodDef adjacency, shared-stub xref centrality, or cosmetic decompiler quality as a gate. If no remaining candidate satisfies both conditions, stop HF managed recovery and proceed to 67/67 Unity/package validation, integrate the HF36 cumulative Assembly-CSharp recovery, compile and validate MainMenu/Board/gameplay paths, and only then perform necessary iOS adaptation.
