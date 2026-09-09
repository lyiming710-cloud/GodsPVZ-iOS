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
- Fixed ILSpyCmd / ICSharpCode.Decompiler `11.0.0.9375` with the reproduced 56-DLL fixed-Cpp2IL reference set.

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
- **HF25 Projectile Collision Dispatch / Ground — native-backed formal final — `eb0823deff8289d152af52ab89b91672b64bb786701d31d4871ebfcdbe9f9b71`.**

## HF23 retained result

HF23 restores the nine directly dispatched Zombie lower-damage MethodDefs `0x06000456` through `0x0600045E`: Hurt_Armor1, Hurt_Armor2, Hurt_Artillery, Hurt_Ashes, Hurt_Body, Hurt_FinalDamageReduction, Hurt_Normal, Hurt_Real, and Hurt_Throughout.

Accepted semantics include native Zombie armor/body defense with the 0.1 non-real damage floor, real bypass, `MathF.Round`, PoleCommander.speed cap 0.95, iceCube fire_ice coefficient 0.2, throughout truncating penetration, distinct Artillery/Real armor-return handling, Ashes behavior, armor presentation/audio, body large-damage text, and restoration of `Damage.damagePoint` after successful throughout body damage.

HF23 Drive folder: `1nvim2033w3T_GnmaDHlSf8rVyF6CT-0K`; final DLL `1qcGGHQZuVe5s60jje-dIJFzHVH-gkD7M`.

## HF24 retained result — Projectile Runtime Core

HF24 was opened only after the post-HF23 decision scan proved both native-vs-managed loss and active-path importance in per-frame `Projectile.Update`.

HF24 restores exactly:

- `0x060003D2 Projectile.Update()` — RID `978` — PC `0x18037D710`, logical native end `0x18037DF0C`.

Key restored behavior includes SortingGroup/sorting-order truncation, board/gameStart/gamePause gating, movement tracks 3/4/5/6/7, native-order `Time.deltaTime` integration, shadow synchronization, out-of-map destruction, active-frame `CollisionDetect`, track following, rotation, LightSaber orphan cleanup, and previousPosition update.

HF24 formal validation:

- formal HF23 input Drive ID `1qcGGHQZuVe5s60jje-dIJFzHVH-gkD7M`, SHA `35d13b0d7fc5f82e04b837b812fab501a933b00e21f3431b2f6759c8e0c2b39c`;
- final patcher workflow `34306586359`, artifact SHA `fc7a62398efc3c3a9b3eddfc6cda43ba517f7938699904571a19fb86c0bb38be`;
- deterministic double patch, final SHA `bdf1c0802685fa56a43602c7a8e45a30b48593b2903e99715a260473f5a09577`;
- Cecil reopen `335 IL / 1047 bytes / 0 EH`;
- permanent RecoveryAudit commit `2470b3872e7be15b1fd662cdcae430b481b1fc20`, workflow `34306849398`, artifact SHA `bdfb83c9ce5dd5d74db4ec8988fc75cb1f666cab31e15c2a4fc73637452bffdd`, OPEN1/OPEN2 -> `RECOVERY_AUDIT_OK`;
- fixed ILSpy whole HF23 SHA `7ea1f0c41035d87165b72ee462828a8c5b1c176e42db608c4f3dd72dcb54aa51`, whole HF24 SHA `32e11c3eb33f19b357f34ef22f0dfb96eb91181cd4ff0f5d50ecf742ffd18d21`;
- MethodDef `2317 -> 2317`, non-method skeleton identical, only `0x060003D2` changed; semantic diff SHA `ac64cb323d71f4b8a2e7f8aab93da05aed6fa75de6627cd14963b9d976665d0b`.

HF24 Drive acceptance:

- folder `1jWriDX2NiJsO2hWhLwnebVtL4d2j5N37`;
- final DLL `1UIzsJRqKwT0R-xCSaWqCG6CtenvPj2he`;
- Evidence-FINAL `1nApSKSXQLiqqcezHhaa96n0KvSh8EvUF`, SHA `a6c83eff67a2ceb86e7ad44b2a111abed0bab7fee465ef4f19623b2b1aebc0ab`;
- SHA256SUMS-FINAL `14BTnOQrQdD5AkgxQN16TYgqIjW1ZNYRL`, SHA `bcca6a22804573053733ca5420a94efaa8121e69f1fe73d17777327aa01365c4`;
- provider final readback exactly 22 files = 20 payloads + 2 closure files.

GitHub Evidence: `Tools/HighFidelityPatch/Evidence/HF24-Projectile-Runtime-Core.md`.

## HF25 technical result — Projectile Collision Dispatch / Ground

The post-HF24 decision scan proved the dispatcher itself remained materially damaged. HF24 `Projectile.Update` calls `CollisionDetect()` every active projectile frame, so this is a direct runtime path rather than warning-only cleanup.

HF25 restores exactly two MethodDefs:

- `0x060003E0 Projectile.CollisionDetect()` — RID `992` — PC `0x18037A2D0`, logical native end `0x18037A410`;
- `0x060003E2 Projectile.CollisionDetect_Ground()` — RID `994` — PC `0x180379930`, logical native end `0x1803799B0`.

Original Projectile metadata closure used TypeDef index `5212` and field offsets `ID +0x24`, `camp +0x28`, `damage +0x30`, `fZ +0x4C`, `fZ_shadow +0x50`, `hitType +0x70`.

Accepted native behavior:

- `CollisionDetect` performs four-way `hitType` 0/1/2/3 routing with native camp gates and detector short-circuit order; unsupported values return;
- hitType 0 routes to ground only;
- hitType 1 routes opposite-camp detectors then ground fallback;
- hitType 2 routes same-camp detectors under the native camp gates then ground fallback, with unsupported camp returning;
- hitType 3 runs opposite-camp then same-camp detector chains before ground fallback;
- ground threshold is `240f` only for IDs 29/30, otherwise `0f`;
- native `COMISS threshold,diff; JB return` unordered/NaN behavior is preserved by `if (!(threshold >= diff)) return;`;
- ground `AreaDamage` IDs are `9-11`, `15`, and `26-32`;
- an accepted ground hit then always calls `Collision_AudioParticle()` and `DestroyProjectile()`.

A deterministic earlier candidate was rejected before formal acceptance because an ordinary `<` comparison did not preserve PC `COMISS/JB` unordered semantics. It was never used as a cumulative formal input. The final candidate was rebuilt and every formal gate rerun from accepted HF24.

## HF25 validation and acceptance

- formal HF24 was re-fetched from Drive ID `1UIzsJRqKwT0R-xCSaWqCG6CtenvPj2he` and re-hashed `bdf1c0802685fa56a43602c7a8e45a30b48593b2903e99715a260473f5a09577`;
- final patcher source head `37cc9c3cc82e3fb3244a70747219cb4fe4cc4257`, workflow `34309550102` success, artifact SHA `9e4c863c1b3e5d6d01b297b05561e82d15657d864f440579c9cfe3680a39f2a9`;
- final patcher applied twice independently to the same formal HF24 input; outputs byte-identical at HF25 SHA `eb0823deff8289d152af52ab89b91672b64bb786701d31d4871ebfcdbe9f9b71`;
- Cecil reopen both runs: E0 `105 IL / 245 bytes / 0 EH`, E2 `49 IL / 125 bytes / 0 EH`, no Cpp2IL helper;
- permanent RecoveryAudit final extension commit `529f457a30dcc4f7e4d9c330fa7cc74e403ea75a`, Program blob `e4780fb52131ad1c1c34f002a0cd8d976926c274`, workflow `34309694411` success, artifact SHA `565d69d2cd816cd552303eff3c521e525687b5d26eb8ec6125aa12c72f4ec769`;
- published auditor OPEN1/OPEN2 each report `320 types / 2317 methods / 2297 bodies`; E0 `105 / 245`, E2 `49 / 125`; final `RECOVERY_AUDIT_OK`;
- fixed ILSpy 11.0.0.9375 member readbacks both exit 0 / stderr 0 / zero Cpp2IL refs / zero issue markers;
- HF24 whole IL SHA `32e11c3eb33f19b357f34ef22f0dfb96eb91181cd4ff0f5d50ecf742ffd18d21` reproduced in the HF25 environment;
- HF25 whole IL SHA `9fc5f9d317d6469ba8e24ad09ff22e8480e562e81b5595f1e4ae3d1a50f10722`;
- MethodDef `2317 -> 2317`; normalized non-method skeleton identical; exactly E0/E2 changed;
- semantic diff SHA `e64591dafd6297de6c3193683eef80078cc27490a849ce92206250f450cea2a2`.

HF25 Drive archive is accepted:

- folder `1DfwwokTD0k8_-J2Z5virKxrHocc9Pszf`;
- cumulative audited DLL `1ENIzUe4qD4qdSBGXACyzQ2CmMIwja2i5`;
- patcher `1jgkTsMxW4hzA8RWJnQ6NChywVELofePR`;
- published RecoveryAudit `1g9k62ovG6p7WqdkrIcJKteEnR8ChEia4`;
- fixed ILSpy `1IkJOZR2RmqSNBPKfEkp8CglBO65V99IL`;
- native evidence `1-Y6_ef9PiMhmDL1wj9nK1U7f6XlfMCyN`;
- patch source `1jXupJAr18EHQNJPrBrxPKdMrjZhd_gJF`;
- semantic diff `1adHN8Dk4QhgiH0BnIPsCIpCOd76ljblX`;
- semantic isolation `1_JGXxwQgit-_1tiC64P4Cqx8sinLkJ0b`;
- MethodDef table `12CskzYPviPBWWlgMKIBrgBxrcOUl2dE3`;
- RecoveryAudit log `1Yl5XVgWANf_PMuHCq4gp4-hLehizKm-R`;
- payload SHA manifest `1roFxlfQPAA4XEiT9zjPmAzC56hkxQolV`, SHA `b3d9636c802d12f598f644c0d1a929acb4ecd8272c498f188efa2c9ef6bec8be`;
- Evidence-FINAL `1jUEhU_du7mcS7wUM9irOtJdMWeqPIdWm`, SHA `dfbea12528de6330880e8aa689f773045c638c510f72d044f33fd45bb321dadd`;
- SHA256SUMS-FINAL `1doFw5vN-WNwScQwaBPxJjk9bcMekSGky`, SHA `5124d43007890c53e10bf933a5dcc30b9d9d8777005b3627af7ab6aa28ff0d19`.

Provider final readback has `has_more=false` and verifies exactly **22 files = 20 payloads + 2 closure files**.

GitHub Evidence: `Tools/HighFidelityPatch/Evidence/HF25-Projectile-Collision-Dispatch-Ground.md`.

**HF25 formal acceptance: PASS. HF25 is now the only allowed formal input for any later cumulative HF stage.**

## Unity reconstruction state

AssetRipper ~3,149 objects; reconstructed project ~6,783 files. 173 game script types and 263 refs across 114 assets migrated. MainMenu/Board scenes restored. Fixed package set: UGUI 1.0.0, TMP 3.0.6, Core/URP 14.0.11, 2D Animation 9.1.1, Tilemap Extras 3.1.2, Burst 1.8.17, Collections 1.2.4, Mathematics 1.2.6, Visual Scripting 1.9.4. 67 package script types still require 67/67 Unity validation.

## Current decision gate — do not auto-open HF26

HF25 closes the collision dispatcher and ground-hit path. Do **not** open HF26 merely because adjacent Projectile methods contain Cpp2IL warnings or sit next to the recovered MethodDefs.

Rerun the remaining native-vs-managed active-path scan over the target detectors and their true direct business dependencies, beginning with:

- `Projectile.CollisionDetect_Device` `0x060003E1` — PC `0x180379180`;
- `Projectile.CollisionDetect_Plant` `0x060003E3` — PC `0x1803799B0`;
- `Projectile.CollisionDetect_Zombie` `0x060003E4` — PC `0x180379D00`.

Then include a resolution/helper method only if direct native call evidence proves it belongs to the detector's business path and its managed body has concrete loss. Existing candidates such as `Collision_Device`, `Collision_Zombie`, `Collision_AudioParticle`, `Aim`, `Update_Tracking`, and `SetEulerAngles` must each independently pass the same gate.

A later HF stage is allowed only when both conditions hold:

1. concrete managed loss/mis-reconstruction is proven from original PC native/metadata evidence; and
2. the affected method is materially active in gameplay.

If no detector/helper cluster satisfies both conditions, stop HF recovery and proceed to Unity `2022.3.44f1c1`, package restoration, 67/67 package-script validation, recovered Assembly-CSharp integration, compile/scene/gameplay validation, then necessary iOS platform adaptation.
