# Recovery status

## Current strategy

Use PC x86-64 IL2CPP as the primary gameplay source, Android native second, then original metadata/assets/JSON; use Cpp2IL/ILSpy for attribution and managed reconstruction support. Do not substitute rescue-route approximations for native-backed gameplay.

High-fidelity-first remains mandatory: a recovered stage is not final until native attribution, deterministic formal-input patching, permanent Cecil audit, fixed-version ILSpy readback, whole-assembly semantic isolation, GitHub evidence, and Google Drive provider acceptance all close.

## Fixed original baseline

- Unity `2022.3.44f1c1`; metadata `31.1`.
- PC CodeRegistration / MetadataRegistration `0x1815E88C0` / `0x1818C6D00`.
- PC ZIP SHA-256 `2f08b4e2243e2bd296f3db694b4a2c9656252d76202c80167bdf3340b9f65b48`.
- PC GameAssembly SHA-256 `9ebd7ca996a5b03fb4a766f7a2502b660d581ddbbb36af4d7bf2f06a211da39d`.
- PC metadata SHA-256 `ad992341add498bd1018ac980f42171e0737b0793bf945c8714ad70a445b36b9`.
- Android APK SHA-256 `428e0ba2e46645a905fb9fbb00cfde42406727a3ddfce9a7df1e0875889a739f`.
- Cpp2IL source commit `5fb20304df698ffd3d0e664b2a698cd911dc9d57`; reproduced PC baseline `2318/2319`, sole full failure closed in HF3.
- Native attribution: original RID-1 -> Assembly-CSharp CodeGenModule methodPointers index.
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
- **HF24 Projectile Runtime Core — native-backed formal final — `bdf1c0802685fa56a43602c7a8e45a30b48593b2903e99715a260473f5a09577`.**

## HF23 retained result

HF23 restores exactly nine directly dispatched Zombie lower-damage MethodDefs `0x06000456` through `0x0600045E`: Hurt_Armor1, Hurt_Armor2, Hurt_Artillery, Hurt_Ashes, Hurt_Body, Hurt_FinalDamageReduction, Hurt_Normal, Hurt_Real, and Hurt_Throughout.

Key accepted semantics include native Zombie armor/body defense with the 0.1 non-real damage floor, real bypass, `MathF.Round`, PoleCommander.speed cap 0.95, iceCube fire_ice coefficient 0.2, throughout truncating penetration, distinct Artillery/Real armor-return handling, Ashes behavior, Armor hit presentation/audio, body large-damage text, and restoration of `Damage.damagePoint` after successful throughout body damage.

HF23 remains fully archived under Drive folder `1nvim2033w3T_GnmaDHlSf8rVyF6CT-0K`; its final audited DLL is `1qcGGHQZuVe5s60jje-dIJFzHVH-gkD7M`.

## HF24 technical result

HF24 was opened only after the post-HF23 decision scan proved both concrete native-vs-managed loss and active-path importance in Unity's per-frame `Projectile.Update`.

HF24 restores exactly one MethodDef:

- `0x060003D2 Projectile.Update()` — RID `978` — PC native `0x18037D710`, logical native function end `0x18037DF0C`.

Restored runtime behavior includes:

- SortingGroup creation/initialization, sorting layer `Entity`, and sorting order using native float multiplication + `cvttss2si` truncation;
- board/gameStart/gamePause runtime gate;
- movement tracks 3/4/5/6/7, including the native track-4 `log(10)` trajectory and track-5 high-arc snap/descent;
- repeated native-order `Time.deltaTime` observations for fX/fY/speed/fZ/zSpeed integration;
- `Moving_SetNewPosition`;
- shadow scale and `fZ_shadow` synchronization;
- out-of-map destroy followed by `CollisionDetect` on active frames;
- track-following, `Rotating`, LightSaber orphan self-destruction, and previousPosition update.

HF24 intentionally excludes collision detector/resolution and aim/helper bodies; scope is exactly `Projectile.Update`.

## HF24 validation and acceptance

- formal HF23 input was re-fetched from accepted Drive ID `1qcGGHQZuVe5s60jje-dIJFzHVH-gkD7M` and re-hashed `35d13b0d7fc5f82e04b837b812fab501a933b00e21f3431b2f6759c8e0c2b39c`;
- final patcher source head `7c4da1c0c14d916fc9a526f526298f9bd44d8c2d`, workflow `34306586359` success, artifact SHA-256 `fc7a62398efc3c3a9b3eddfc6cda43ba517f7938699904571a19fb86c0bb38be`;
- earlier runs `34306195447` and `34306382755` were tool-only failures before output and produced no candidate;
- final patcher applied twice independently to the same formal HF23 input, outputs byte-identical at HF24 SHA `bdf1c0802685fa56a43602c7a8e45a30b48593b2903e99715a260473f5a09577`;
- Cecil reopen both runs: `Projectile.Update` = 335 IL / 1047 bytes / 0 EH, with no Cpp2IL helper;
- permanent RecoveryAudit extension commit `2470b3872e7be15b1fd662cdcae430b481b1fc20`, workflow `34306849398` success, published artifact SHA-256 `bdfb83c9ce5dd5d74db4ec8988fc75cb1f666cab31e15c2a4fc73637452bffdd`;
- published auditor OPEN1/OPEN2 each report 320 types, 2317 methods, 2297 bodies; `Projectile.Update` is 335 IL / 1047 bytes on both opens; final `RECOVERY_AUDIT_OK`;
- fixed ILSpy 11.0.0.9375 member readback exit 0 / stderr 0, zero Cpp2IL refs and zero issue markers; whole readback stderr 0;
- HF23 whole IL SHA `7ea1f0c41035d87165b72ee462828a8c5b1c176e42db608c4f3dd72dcb54aa51`;
- HF24 whole IL SHA `32e11c3eb33f19b357f34ef22f0dfb96eb91181cd4ff0f5d50ecf742ffd18d21`;
- MethodDef count `2317 -> 2317`; normalized non-method skeleton byte-identical; exactly `0x060003D2 Projectile.Update` changes;
- semantic diff SHA-256 `ac64cb323d71f4b8a2e7f8aab93da05aed6fa75de6627cd14963b9d976665d0b`.

HF24 Drive archive is accepted:

- folder `1jWriDX2NiJsO2hWhLwnebVtL4d2j5N37`;
- cumulative audited DLL `1UIzsJRqKwT0R-xCSaWqCG6CtenvPj2he`;
- patcher `14j-vZPT1pbMx9GDOao1OUuijVA4tQjZj`;
- published RecoveryAudit `12n8Q5LfxPI4c_vHcQlPmPr-EBF4VD2Mk`;
- fixed ILSpy bundle `1UkLFLjtwjzuI8bg2HoekYSk7a1fl1xIR`;
- native evidence `15hgH1xBLjhxAtNFbJV2eGykwlBlMCvpz`;
- patch source `1otAgdL6uKWC-PTKE7jb0dkmqGS9Bt2n4`;
- semantic diff `1G35WUYlxsh0CrAKTV7lRrxV0cZMlbE-7`;
- semantic isolation `153b0_cW_caCxfsUEjsALFdJhsBbaWam_`;
- MethodDef table `1djCIKX1gReUwDm7Ma5t-J73bqDxEBGZj`;
- RecoveryAudit log `1oqk2t-J8wm72E-EdDzCHIvKNOkcOTRny`;
- payload SHA manifest `1sWwMxyt1k3CGv_pR2nmgrrlOaYLcRjNt`;
- authoritative Evidence-FINAL `1nApSKSXQLiqqcezHhaa96n0KvSh8EvUF`, SHA-256 `a6c83eff67a2ceb86e7ad44b2a111abed0bab7fee465ef4f19623b2b1aebc0ab`;
- authoritative SHA256SUMS-FINAL `14BTnOQrQdD5AkgxQN16TYgqIjW1ZNYRL`, SHA-256 `bcca6a22804573053733ca5420a94efaa8121e69f1fe73d17777327aa01365c4`.

Provider final readback has no next page and verifies exactly **22 files = 20 payloads + 2 closure files**.

GitHub Evidence: `Tools/HighFidelityPatch/Evidence/HF24-Projectile-Runtime-Core.md`.

**HF24 formal acceptance: PASS. HF24 is now the only allowed formal input for any later cumulative HF stage.**

## Unity reconstruction state

AssetRipper ~3,149 objects; reconstructed project ~6,783 files. 173 game script types and 263 refs across 114 assets migrated. MainMenu/Board scenes restored. Fixed package set: UGUI 1.0.0, TMP 3.0.6, Core/URP 14.0.11, 2D Animation 9.1.1, Tilemap Extras 3.1.2, Burst 1.8.17, Collections 1.2.4, Mathematics 1.2.6, Visual Scripting 1.9.4. 67 package script types still require 67/67 Unity validation.

## Current decision gate — do not auto-open HF25

HF24 closes the damaged per-frame Projectile runtime dispatcher. Do **not** create HF25 merely because the surrounding Projectile methods contain Cpp2IL warnings.

Rerun the remaining managed-damage / active-path decision scan over the HF24 final, focusing on the collision/aim cluster already identified from PC native evidence:

- `Projectile.CollisionDetect_Device` `0x060003E1` PC `0x180379180`;
- `Projectile.CollisionDetect_Plant` `0x060003E3` PC `0x1803799B0`;
- `Projectile.CollisionDetect_Zombie` `0x060003E4` PC `0x180379D00`;
- `Projectile.Collision_AudioParticle` `0x060003EB` PC `0x18037A410`;
- `Projectile.Collision_Zombie` `0x060003EA` PC `0x18037B170`;
- `Projectile.Aim` `0x060003DC` PC `0x180378D50`;
- `Projectile.Update_Tracking` `0x060003D8` PC `0x18037D4D0`;
- `Projectile.SetEulerAngles` `0x060003F8` PC `0x18037C120`.

A later HF stage is allowed only when both conditions hold:

1. concrete managed loss/mis-reconstruction is proven from original PC native/metadata evidence; and
2. the affected method is materially active in the gameplay path.

If the collision/aim decision scan does not identify a new blocker that satisfies both gates, stop HF recovery and proceed to Unity `2022.3.44f1c1`, package restoration, 67/67 package-script validation, recovered Assembly-CSharp integration, compile/scene/gameplay validation, then necessary iOS platform adaptation.
