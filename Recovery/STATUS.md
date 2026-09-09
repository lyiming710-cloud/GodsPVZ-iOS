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
- HF29 `8c13a6638276a0e201ee39192545bbbe73703a7251f4188080dcfcf0c4d55590`
- HF30 `6ccccf685950edba428b51dbe3aa95e511caa467d134dd4202939e19ef655c24`
- HF31 `a4d001581430fe440e50feb37f3abd189a1ae86a00f4fe41114ae3f9b99386d2`
- HF32 `a959e450bd71beb67c948a9a85a1263a7739ccd37b2438405ae954fe10b7bf87`
- **HF33 Projectile Aim / Euler Runtime Core — native-backed formal final — `6b467ef40f2e8e4e7fcff48d71c328457ac4107261a95fbd59908f5516720518`.**

## Recent retained results

### HF23 — Zombie Hurt Core

Restores exactly `0x06000456–0x0600045E` Hurt_Armor1/2, Artillery, Ashes, Body, FinalDamageReduction, Normal, Real, Throughout with native armor/body DEF, non-real 0.1 floor, real bypass, `MathF.Round`, PoleCommander cap 0.95, iceCube fire_ice 0.2 and throughout truncation.

### HF24 — Projectile Runtime Core

Restores exactly `0x060003D2 Projectile.Update()`: sorting-order truncation, pause/start gates, movement tracks 3/4/5/6/7, native-order delta-time integration, shadow sync, out-of-map destruction, CollisionDetect, tracking/rotation, LightSaber cleanup and previousPosition.

### HF25–HF30 — Projectile collision chain

HF25–HF30 formally restore `CollisionDetect`, Ground, AudioParticle, Device/Zombie resolution, and Plant/Zombie/Device detectors. The recovered collision dispatcher path is closed end-to-end into damage and hit post-processing.

### HF31 — Projectile Rotating Runtime Core

Restores exactly `0x060003F6 Projectile.Rotating()` — PC `0x18037BE20`.

Accepted behavior:

- real `Projectile.speed` field is `UnityEngine.Vector3`; native reads `speed.x`;
- negative or NaN/unordered `speed.x` selects Y local Euler `180f`, otherwise `0f`;
- `angularAcceleration != 0` integrates `angularSpeed += Time.deltaTime * angularAcceleration`;
- non-zero angular speed rotates `projectileSprite` and `projectileAnimation` around Z/forward in `Space.World`.

HF31 final SHA `a4d001581430fe440e50feb37f3abd189a1ae86a00f4fe41114ae3f9b99386d2`; fixed ILSpy, whole-assembly semantic isolation, permanent RecoveryAudit and 20+2 Drive closure all passed. HF31 is retained as the accepted direct input to HF32.

### HF32 — Projectile Tracking Runtime Core

Restores exactly `0x060003D8 Projectile.Update_Tracking()` — RID `984`, PC `0x18037D4D0`.

Accepted behavior:

- initializes `target = null` and `minX = 2147483648f`;
- iterates `board.zombieManager.zombieList`;
- skips `IsDisabled()` zombies;
- requires `CanAttacked()` and retains the attackable zombie with minimum `fX`;
- preserves foreach `Enumerator.Dispose/finally`;
- if a target exists, calls `Aim(new Vector3(target.fX - fX, target.fY - fY, 0f))`.

HF32 formal validation:

- formal HF31 input re-fetched and SHA-verified;
- patcher workflow `34374542786` PASS, artifact ID `10113295189`;
- deterministic double patch -> `a959e450bd71beb67c948a9a85a1263a7739ccd37b2438405ae954fe10b7bf87`;
- Cecil reopen `55 IL / 150 bytes / 1 EH`;
- fixed ILSpy member gate PASS;
- HF31 whole IL reproduced exactly; HF32 whole IL `e81d4db126c25a9d3f593c0535017d4a0d24e3cca04e24635ea4bfe0b1769c89`;
- MethodDef `2317 -> 2317`, normalized non-method skeleton identical, exactly `0x060003D8` changed;
- semantic diff `2c5caa7d12a898c3897ee123d0ab0e6400d1a2a482381a96ec0e8f6f1a09807a`;
- published RecoveryAudit OPEN1/OPEN2 PASS and `RECOVERY_AUDIT_OK`;
- Drive folder `1Uww_2PvbJIX3IUvyQRlWIHjqZQ1YHtkO`, final DLL `1SS6-gWmohn9JqU772ZAJ8vkCB4t4QWwP`, final readback exactly 22 files.

Evidence: `Tools/HighFidelityPatch/Evidence/HF32-Projectile-Tracking-Runtime-Core.md`.

**HF32 formal acceptance: PASS. HF32 is retained as the accepted direct input to HF33.**

### HF33 — Projectile Aim / Euler Runtime Core

Restores exactly:

- `0x060003DC Projectile.Aim(Vector3)` — RID `988`, PC `0x180378D50`;
- `0x060003F8 Projectile.SetEulerAngles(float,float)` — RID `1016`, PC `0x18037C120`.

The original PC Windows release was re-fetched from the archived game package and the fixed GameAssembly/global-metadata hashes were re-verified before behavioral closure.

Accepted `Aim` behavior:

- `angle = Mathf.Atan2(distant.y, distant.x) * 57.29578f`;
- ordered-negative `speed.x` applies `180f - angle`; unordered/NaN does not take that negative branch;
- normalize `distant` with Unity Vector3 semantics;
- preserve the current three-component `speed` magnitude and redirect `speed` along normalized `distant`;
- call `SetEulerAngles(angle, 0f)`.

Accepted `SetEulerAngles` behavior:

- stores original `angular` and `zAngular` arguments in fields immediately;
- local effective z becomes `angular` for zero/Approximately cases, otherwise uses Abs-selected `Mathf.Lerp(angular,zAngular,selected/(angular+zAngular))`;
- sprite and animation preserve x/y and receive effective z;
- shadow preserves x/y and receives original `angular`;
- track preserves the original native behavior of sourcing x/y from `projectileSprite` and using effective z;
- local effective z is not written back into the `zAngular` field.

HF33 formal validation:

- formal HF32 Drive final re-fetched and SHA-verified `a959e450bd71beb67c948a9a85a1263a7739ccd37b2438405ae954fe10b7bf87`;
- patcher workflow `34381241262` PASS;
- patcher artifact ID `10115939120`, SHA `3b53a4e2a22d9efc41918c87f6648911018bdd9a0a1b7c075c864d0fd262db53`;
- independent double patch -> byte-identical `6b467ef40f2e8e4e7fcff48d71c328457ac4107261a95fbd59908f5516720518`;
- Cecil reopen Aim `31 IL / 93 bytes / 0 EH`, SetEulerAngles `118 IL / 386 bytes / 0 EH`;
- fixed ILSpy 11.0.0.9375 + reproduced 56-DLL member readback PASS, stderr 0, Cpp2IL refs 0, issue markers 0;
- HF32 whole IL reproduced exactly `e81d4db126c25a9d3f593c0535017d4a0d24e3cca04e24635ea4bfe0b1769c89`;
- HF33 whole IL SHA `36f84c63dc8906ab33424da6a67246569a7e103e5b2ffbf35ca757d698698450`;
- accepted HF31->HF32 semantic diff reproduced byte-for-byte first at `2c5caa7d12a898c3897ee123d0ab0e6400d1a2a482381a96ec0e8f6f1a09807a`;
- MethodDef `2317 -> 2317`, normalized non-method skeleton identical, changed exactly `0x060003DC` and `0x060003F8`;
- HF32->HF33 semantic diff SHA `59d5f0c92d7115c54bc4f5ee69881c5b120d1a99dabc640e7ce180dbfdc3ceae`;
- permanent RecoveryAudit commit `2d57910889988b590f1dd07cdcc34ee12897b952`;
- RecoveryAudit workflow `34381940381` PASS, artifact ID `10116213345`, SHA `3685e1d9fd17d8b277a64a8bd4cc05c293eaff87869bd07d81caa36f1cf51139`;
- published auditor OPEN1/OPEN2 `320 types / 2317 methods / 2297 bodies`, Aim `31/93`, SetEulerAngles `118/386`, `RECOVERY_AUDIT_OK`.

HF33 Drive archive:

- folder `1jGlVm6aPkLamhOSRuCPZezPqwC-zcQmT`;
- cumulative audited DLL `1Ogbe_UWDSdux7n9ROWyRr5e6dU1Nk5Vm`;
- patcher `1MbL4joVSYGeIMUJS4FO8qhWbDZ-3DUFZ`;
- RecoveryAudit `1kj41inhhOifAmMlcUL2ZGgHYP4g_KKrc`;
- fixed ILSpy `1bmEgTCcHue3J_xYaM3q2tqV0HdRvukOk`;
- native evidence `1aFA43x3zI2G1UdyXmKj1GeKPLFAFV_5F`;
- semantic diff `1WUOWoHmMJagr1N0WBuBHboKGL7YzAfUw`;
- payload manifest `1T5JqgT-axJVHjcKn2CKXfsgrbdNm4uDg`, SHA `bc6ae57092ea8d9e8ddec228b4f0070f5b3658c279a04cccebfab773ef45e84c`;
- Evidence-FINAL `1J2V5wEhjPWoSh_WnxiHCozcJ9q0fy2U4`, SHA `781eca6bd6fe245bdcdbca77c1efbf950728fb978641785c0865609b196d25d2`;
- SHA256SUMS-FINAL `1Q23ybMl2Boh2USLJPz4HYTm2m9ues4RZ`, SHA `bfbf169153ec1d9cc7821ec2fe74e4b6507ee333450c98d8d4b9a5880877c4d7`;
- final provider readback: exactly 22 files = 20 payloads + 2 closure files.

Evidence: `Tools/HighFidelityPatch/Evidence/HF33-Projectile-Aim-Euler-Runtime-Core.md`, commit `7d8b45d9dd7d410d9ea2f07fee17ff0310ef78e9`.

**HF33 formal acceptance: PASS. HF33 is now the only allowed formal input for any later cumulative HF stage.**

## Unity reconstruction state

AssetRipper ~3,149 objects; reconstructed project ~6,783 files. 173 game script types and 263 refs across 114 assets migrated. MainMenu/Board scenes restored. Fixed package set includes UGUI 1.0.0, TMP 3.0.6, Core/URP 14.0.11, 2D Animation 9.1.1, Tilemap Extras 3.1.2, Burst 1.8.17, Collections 1.2.4, Mathematics 1.2.6, Visual Scripting 1.9.4. 67 package script types still require 67/67 Unity validation.

## Current decision gate — do not auto-open HF34

HF33 closes the active `Update_Tracking -> Aim -> SetEulerAngles` tracking/orientation chain. Do not create HF34 merely because other methods still contain Cpp2IL warnings or unattractive recovered IL.

Open HF34 only when both are independently proven:

1. original PC native/metadata demonstrates concrete managed loss or mis-reconstruction; and
2. the method is materially active in gameplay on a path not already semantically closed by HF1–HF33.

Do not use warning count, MethodDef adjacency, shared-stub xref centrality, or cosmetic decompiler quality as a gate. If no remaining candidate satisfies both conditions, stop HF managed recovery and proceed to 67/67 Unity/package validation, integrate the HF33 cumulative Assembly-CSharp recovery, compile and validate MainMenu/Board/gameplay paths, and only then perform necessary iOS adaptation.
