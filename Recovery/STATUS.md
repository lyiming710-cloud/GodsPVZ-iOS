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
- **HF35 Device Injury Status Core — native-backed formal final — `ffd3e858a6ea15c1203a8c543e800b9181e0ab115fa7ab6a84d00975be5b947b`.**

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

HF34 formal validation:

- formal HF33 input SHA-verified `6b467ef40f2e8e4e7fcff48d71c328457ac4107261a95fbd59908f5516720518`;
- final stable-audio-CIL template commit `ee6092042d3396e71c01a574fad5f59a6d84ff49`;
- final patcher workflow `34387511067` PASS;
- patcher artifact ID `10118287463`, SHA `e9e39330f029467bb6cf86af153a630e62283ee3214c1b2178494b7a0473a1f2`;
- independent double patch -> byte-identical `ead8dee5def2818ca8c64c36b4fe65f1830dbf873fae34dbc84d2f07b103d999`, stderr 0;
- Cecil reopen CreateAudioAtPoint/4 `55 IL / 145 bytes / 0 EH`, Update_Time `81/225/0`, Update_MoveTrack7 `67/207/0`, GetPredictedPosition `36/83/0`;
- fixed ILSpy 11.0.0.9375 + reproduced 56-DLL readback PASS for all four, stderr 0, Cpp2IL refs 0, issue markers 0;
- HF33 whole IL reproduced exactly `36f84c63dc8906ab33424da6a67246569a7e103e5b2ffbf35ca757d698698450`;
- HF34 whole IL SHA `96d4bd053040cb1531d3feb5f2e28d20cfd9f51ebc273d6a457127f312088599`;
- accepted HF32->HF33 semantic diff reproduced byte-for-byte first at `59d5f0c92d7115c54bc4f5ee69881c5b120d1a99dabc640e7ce180dbfdc3ceae`;
- MethodDef `2317 -> 2317`, normalized non-method skeleton identical, changed exactly `0x0600013E`, `0x060003D6`, `0x060003DB`, `0x06000450`;
- HF33->HF34 semantic diff SHA `27e250ef34624ad70c5345c770eefe52d7204a4452d7c861013e1a72dcc59697`;
- permanent RecoveryAudit commit `addee3ab7b67d406561e8b9e11a2eae75296a786`;
- RecoveryAudit workflow `34388391648` PASS, artifact ID `10118629226`, SHA `32637b66f392dfb4d4d880e54b3ce768e43178d4a04606bbcdce3e1d37645879`;
- published auditor OPEN1/OPEN2 `320 types / 2317 methods / 2297 bodies`, all four target sizes match, terminal `RECOVERY_AUDIT_OK`.

HF34 Drive archive:

- folder `18gmKI_V1HS2j1CFnzauo9GZkN3bJlTPf`;
- cumulative audited DLL `1UEZgP33n40vo9klmH0sU5r8bx7kZt1iB`;
- patcher `1jFoTaMOaneREMpLv-r0BLf2P-MEWVnFk`;
- RecoveryAudit `1U_y6fl44zIEooVR29rsRo0kDB4mB5D27`;
- fixed ILSpy `1cZD1za8a-LcKw6jL_7fbzuZAHUivyWCB`;
- native evidence `1gm4reiFQgviwP4xy7DZKux4dEOEnuCL2`;
- semantic diff `1BUnU0jZVFWonqWPPetcvw6Obtamy2_5e`;
- payload manifest `112piV84o2zvDaUeBen5MeKCnn0YAXi_S`, SHA `7c8dddee9cabaf5fb4bdf3b319c83bf9cfd0213e341f526e8e20a00d85ed3596`;
- Evidence-FINAL `14rbMKUmtCiEyz6ig4uC7fgm-fdCR3bhs`, SHA `0a684e5d9c151becfd6cb7dd787309100e625b34667799608215edbb638bb92a`;
- SHA256SUMS-FINAL `18yet-e3uG_PGvF8OnWJhmGReHrkyPEVK`, SHA `5eee708f63ca70dac4470b8abf0848210e6b91ce0a98f30f8b9b0c94a4e62128`;
- final provider readback: exactly 22 files = 20 payloads + 2 closure files.

Evidence: `Tools/HighFidelityPatch/Evidence/HF34-Projectile-Runtime-Support-Core.md`, commit `edc56dd90e92131e2dba544bd0761f977ac7e712`.

**HF34 formal acceptance: PASS. HF34 is retained as the accepted direct input to HF35.**

### HF35 — Device Injury Status Core

Restores exactly `0x0600032B Device.InjuryStatusUpdate()` — original RID `811`, PC `0x180349280`.

Accepted behavior:

- original `COMISS 0,healthPoint` / `JAE` semantics invoke `Broken()` only for ordered `healthPoint <= 0`; unordered/NaN proceeds through the active body;
- non-broken processing is specific to Device `ID == 4`;
- damage fraction is `1f - healthPoint / maxHealthPoint` and each crossed `(brokenLevel + 1) / 3` threshold increments `brokenLevel`;
- each crossed threshold updates the `Roadblock` SpriteRenderer from `ResourceManager.deviceSprites[brokenLevel]` when Unity-truthy;
- native helper `0x1802FB100` is closed as the ParticlesManager singleton backing-field getter/shared thunk, followed by `CreatNewParticle(ParticleState.RoadblockBroken)` where the native enum value is `26`;
- a valid particle copies this Device transform position;
- audio uses `ResourceManager.particleClips[12]`, `Camera.main.transform.position`, `AudioVolume() * 1.6f`, and pitch `0.7f` through the already formal four-argument `CreateAudioAtPoint`.

HF35 formal validation:

- formal HF34 input re-fetched and SHA-verified `ead8dee5def2818ca8c64c36b4fe65f1830dbf873fae34dbc84d2f07b103d999`;
- original PC GameAssembly/global-metadata fixed hashes re-verified and the native body/callsite closed;
- final patcher build head `85934bf0ca63b0faa9c1f59a96ed938293f5012c`;
- patcher workflow `34391370140` PASS;
- patcher artifact ID `10119789189`, SHA `fabd1010ee865a1c3021ddb37a457c24115d74c1ecddcdb0ba3c145c9cdf27dc`;
- independent double patch -> byte-identical `ffd3e858a6ea15c1203a8c543e800b9181e0ab115fa7ab6a84d00975be5b947b`, stderr 0;
- Cecil reopen `79 IL / 244 bytes / 0 EH`;
- fixed ILSpy 11.0.0.9375 + reproduced 56-DLL member readback PASS, stderr 0, Cpp2IL refs 0, issue markers 0;
- HF34 whole IL reproduced exactly `96d4bd053040cb1531d3feb5f2e28d20cfd9f51ebc273d6a457127f312088599`;
- HF35 whole IL SHA `ca155db767d34a814ce322166216aa7c83a868e5d44743c25b71d2c120f22a9e`;
- accepted HF33->HF34 semantic diff reproduced byte-for-byte first at `27e250ef34624ad70c5345c770eefe52d7204a4452d7c861013e1a72dcc59697`;
- MethodDef `2317 -> 2317`, normalized non-method skeleton identical, changed exactly `0x0600032B`;
- HF34->HF35 semantic diff SHA `9c0dacb91256ecadf658ae35faba6474ee5f3f269e7d0f4b2b35b3cfacc22a2e`;
- permanent RecoveryAudit commit `11cb9d454071cf4162ed4813b0c376d120408bd1`;
- RecoveryAudit workflow `34417105324` PASS, artifact ID `10129507233`, SHA `32a52ed6ccdd3cf29808e86da1324c474e8cf5f456f3caef32b7cc5d947bbe75`;
- published auditor OPEN1/OPEN2 `320 types / 2317 methods / 2297 bodies`, Device.InjuryStatusUpdate `79/244` both, stderr 0, terminal `RECOVERY_AUDIT_OK`.

HF35 Drive archive:

- folder `1ZqhsosBN7DZ2RBQcK_b6xJ_RRcu8QZgc`;
- cumulative audited DLL `1mjP1TgepHjgjw-r2y-Y3K576nhQXOxCF`;
- patcher `1HggbixmDAu6dEtaQ8RfATv1QXi0dVTvN`;
- RecoveryAudit `13ejCFG8F13pt8VKrxJbJWJhm1JNxz1iM`;
- fixed ILSpy `1W1vrOSdabizyKrE-Gf8GXKy7pAoGXkBY`;
- native evidence `1MPyUuuXRAUGe1zwpd39vf3hThgZ6gq-_`;
- semantic diff `1yY3mWdrpqdKFm-qjKIDGwKkF2PADcjJd`;
- payload manifest `19WftCs0qhKBGrntxvhNKh2PtLuThnKtP`, SHA `56fef5e8e4d82b8b7504bdffebaa8ffb01fe7b447c0d407aa976b8132b2518f0`;
- Evidence-FINAL `1sN88YRR6CiPaL5u2Qd7qbO4ioxe9TXe-`, SHA `66b7af677c9f96edfd852fe3944c492554811a64a37ff3212192240f6445beff`;
- SHA256SUMS-FINAL `1-FKM0F2CDmUu9Rca-f199-GB79PVVDCv`, SHA `6c3eabb8f0c7ae21dc015a59408affae9beb0cf80a9b7755d7609a5342ff86b5`;
- final provider readback: exactly 22 files = 20 payloads + 2 closure files.

Evidence: `Tools/HighFidelityPatch/Evidence/HF35-Device-Injury-Status-Core.md`, commit `d08323ea70f0600bb23543108e1b4cea2d5e8327`.

**HF35 formal acceptance: PASS. HF35 is now the only allowed formal input for any later cumulative HF stage.**

## Unity reconstruction state

AssetRipper ~3,149 objects; reconstructed project ~6,783 files. 173 game script types and 263 refs across 114 assets migrated. MainMenu/Board scenes restored. Fixed package set includes UGUI 1.0.0, TMP 3.0.6, Core/URP 14.0.11, 2D Animation 9.1.1, Tilemap Extras 3.1.2, Burst 1.8.17, Collections 1.2.4, Mathematics 1.2.6, Visual Scripting 1.9.4. 67 package script types still require 67/67 Unity validation.

## Current decision gate — do not auto-open HF36

HF35 closes the active Device roadblock injury-status path. Do not create HF36 merely because another MethodDef has warnings or unattractive recovered IL.

Re-run the original-native-vs-managed decision gate from the new HF35 formal cumulative DLL. Two known areas require special treatment:

- `Plant.KillEvent` remains blocked until shared native helper `0x1804A25F0` is behaviorally identified and all dependencies close; do not infer or omit that call.
- `Zombie.ZC_ArmoredFlagWakeUpZombies` may be reconsidered as a separate zombie wake-up subsystem, but must independently satisfy concrete managed-loss and materially-active-path gates against the HF35 cumulative assembly and original PC native body.

Open HF36 only if both are independently proven:

1. original PC native/metadata demonstrates concrete managed loss or mis-reconstruction; and
2. the method is materially active in gameplay, with every required native dependency behaviorally closed.

Do not use warning count, MethodDef adjacency, shared-stub xref centrality, or cosmetic decompiler quality as a gate. If no remaining candidate satisfies both conditions, stop HF managed recovery and proceed to 67/67 Unity/package validation, integrate the HF35 cumulative Assembly-CSharp recovery, compile and validate MainMenu/Board/gameplay paths, and only then perform necessary iOS adaptation.
