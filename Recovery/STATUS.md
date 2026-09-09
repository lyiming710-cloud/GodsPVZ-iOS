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
- **HF26 Projectile Collision Audio / Particle — native-backed formal final — `afa0e052902139c09cee9c715fe9a75c6f124af3af8a5647c799dc6b457fefac`.**

## HF23 retained result — Zombie Hurt Core

HF23 restores exactly nine directly dispatched Zombie lower-damage MethodDefs `0x06000456` through `0x0600045E`: Hurt_Armor1, Hurt_Armor2, Hurt_Artillery, Hurt_Ashes, Hurt_Body, Hurt_FinalDamageReduction, Hurt_Normal, Hurt_Real, and Hurt_Throughout.

Accepted semantics include native Zombie armor/body defense with the 0.1 non-real damage floor, real bypass, `MathF.Round`, PoleCommander.speed cap 0.95, iceCube fire_ice coefficient 0.2, throughout truncating penetration, distinct Artillery/Real armor-return handling, Ashes behavior, armor presentation/audio, body large-damage text, and restoration of `Damage.damagePoint` after successful throughout body damage.

HF23 Drive folder `1nvim2033w3T_GnmaDHlSf8rVyF6CT-0K`; final DLL `1qcGGHQZuVe5s60jje-dIJFzHVH-gkD7M`.

## HF24 retained result — Projectile Runtime Core

HF24 restores exactly `0x060003D2 Projectile.Update()` from PC `0x18037D710` through logical native end `0x18037DF0C`. Accepted behavior includes SortingGroup/sorting-order truncation, board/gameStart/gamePause gates, movement tracks 3/4/5/6/7, native-order `Time.deltaTime` integration, shadow synchronization, out-of-map destruction, active-frame `CollisionDetect`, tracking/rotation, LightSaber orphan cleanup and previousPosition update.

HF24 final SHA `bdf1c0802685fa56a43602c7a8e45a30b48593b2903e99715a260473f5a09577`; Drive final DLL `1UIzsJRqKwT0R-xCSaWqCG6CtenvPj2he`. Provider final readback verified 22 files = 20 payloads + 2 closure files. GitHub Evidence: `Tools/HighFidelityPatch/Evidence/HF24-Projectile-Runtime-Core.md`.

## HF25 retained result — Projectile Collision Dispatch / Ground

HF25 restores exactly:

- `0x060003E0 Projectile.CollisionDetect()` — RID 992 — PC `0x18037A2D0`;
- `0x060003E2 Projectile.CollisionDetect_Ground()` — RID 994 — PC `0x180379930`.

Accepted behavior includes native hitType 0/1/2/3 routing, camp gates and detector short-circuit order; ground threshold `240f` only for IDs 29/30; preservation of native `COMISS/JB` unordered/NaN semantics; ground `AreaDamage` IDs `9-11`, `15`, `26-32`; and unconditional `Collision_AudioParticle()` + `DestroyProjectile()` after an accepted ground hit.

HF25 final SHA `eb0823deff8289d152af52ab89b91672b64bb786701d31d4871ebfcdbe9f9b71`; Drive folder `1DfwwokTD0k8_-J2Z5virKxrHocc9Pszf`; final DLL `1ENIzUe4qD4qdSBGXACyzQ2CmMIwja2i5`. Provider final readback verified 22 files = 20 payloads + 2 closure files. GitHub Evidence: `Tools/HighFidelityPatch/Evidence/HF25-Projectile-Collision-Dispatch-Ground.md`.

## HF26 technical result — Projectile Collision Audio / Particle

HF26 was opened only after HF25 proved an accepted ground hit directly enters `Collision_AudioParticle()` while the HF25 managed body still contained concrete Cpp2IL indirect-jump/unmanaged-memory/object-conversion loss.

HF26 restores exactly one MethodDef:

- `0x060003EB Projectile.Collision_AudioParticle()` — RID `1003` — PC `0x18037A410`.

Original Projectile field evidence used by the body includes `ID +0x24`, `fX +0x44`, `fY +0x48`, `fZ +0x4C`, `index +0x74`, `board +0xE8`.

Accepted native-observable behavior includes:

- particle ID routing for PeaSlapt, SnowPeaSlapt, CherryBoom, PotatoBoom, IcicleSlapt, SPHBombing and LightSaberDoom;
- original per-ID particle y offsets, component-wise scale factors and Board.Shake parameters;
- created particles placed at `(fX, particleY, 0)` and assigned SortingGroup layer `Particles`;
- ID30 Animator integer `"type"=index`;
- native audio clip indices, per-ID volume multipliers and pitch `Random.Range(0.9f,1.1f)`;
- ID0/1 preserve float-pitch RNG before int clip-index RNG;
- ID30 preserves two separate audio calls: `zombieClips[10]` then `particleClips[22]`, with separate camera-position reads.

## HF26 validation and acceptance

- formal HF25 input was re-fetched from accepted Drive ID `1ENIzUe4qD4qdSBGXACyzQ2CmMIwja2i5` and re-hashed `eb0823deff8289d152af52ab89b91672b64bb786701d31d4871ebfcdbe9f9b71`;
- patcher source head `85b73cdc82cd697cc8b4bd3df8bee2eb896db24d`, workflow `34319860392` success, artifact SHA `981d2945a73e436a6e7710c0d904c88f6b3df603c471853697d3c56de16e3a90`;
- patcher applied twice independently to the same formal HF25 input; outputs byte-identical at HF26 SHA `afa0e052902139c09cee9c715fe9a75c6f124af3af8a5647c799dc6b457fefac`;
- Cecil reopen both runs: EB `392 IL / 1379 bytes / 0 EH`, no Cpp2IL helper;
- permanent RecoveryAudit extension commit `6148f8d036c6d0a64aeb4efb661ff342dc5784b6`, Program blob `fdd8775f4422fbf937f2c9ca6000b708fb737aba`, workflow `34320757146` success, artifact SHA `8274ba48f23aacceb0ee0e1bed9548d88f5396f1cdd4a33fb6f5756f6d439b29`;
- published auditor OPEN1/OPEN2 each report `320 types / 2317 methods / 2297 bodies`; EB `392 / 1379`; final `RECOVERY_AUDIT_OK`;
- fixed ILSpy 11.0.0.9375 member readback exit 0 / stderr 0 / zero Cpp2IL refs / zero issue markers;
- HF25 whole IL SHA `9fc5f9d317d6469ba8e24ad09ff22e8480e562e81b5595f1e4ae3d1a50f10722` reproduced exactly in the HF26 environment;
- HF26 whole IL SHA `4e3d8039a6a1a07e02b8e9d998be057e1eee95d4bdf139d91ac832dd144f1714`;
- MethodDef `2317 -> 2317`; normalized non-method skeleton identical; exactly `0x060003EB` changed;
- semantic diff SHA `08c012c242a50c97d26fb773dddf9f48eabf541b07b465e4c1f4c560178de0f9`;
- the semantic normalization implementation was first proven by byte-identically reproducing accepted HF24->HF25 semantic diff SHA `e64591dafd6297de6c3193683eef80078cc27490a849ce92206250f450cea2a2`.

HF26 Drive archive is accepted:

- folder `1mPZ-qZ5WBdjfBzPpR4nKxiufPvLbK-TH`;
- cumulative audited DLL `1yc82VM4pK5DdIlu7-qS9VLdcGDsy-ti_`;
- patcher `1OMV__8C5FwuPyrtyZeQ1WgMzZh_UBJcg`;
- published RecoveryAudit `1-V7BOEo5P2lKVQ83BZZQwFSJ5vFWBG0w`;
- fixed ILSpy `1DMN2tq_QZI3qlpRgfRI-VWQba9C9MT5F`;
- native evidence `1j6fZZcIN4NYkYLenLyMy25ImfnIJYCYp`;
- patch source `1tUo0A9fs6lgwgkSeJcxiJ4yxfg1ecq58`;
- semantic diff `1S9sijFx7gg4tnvmBQSe8eMKFqtNq90Sn`;
- semantic isolation `1pCt00yAEI7bqcY772nG7cRDgaRDEKUwB`;
- MethodDef table `1iRV-EdQA-35OJZ3v0aZHFtI7B9gAOljv`;
- RecoveryAudit log `1vwxGm00hsoATQe-omWcynVSTLIRFZtxB`;
- payload SHA manifest `1h7P7OUC_-tQuoip3wEZSDK_rNvitXWQr`, SHA `b5fedf16e09450c1494414910d7d8af5523ab817d46e6fb028cb40f02027503d`;
- Evidence-FINAL `1cMvaPfip_Pg5zHIOHfqNWdjcOned6hBX`, SHA `6355726fe788f5124fd75d3fa842e12f5b517b22af16a881b4af399adecd5ab7`;
- SHA256SUMS-FINAL `1ZhXT2lU60PVs6ob0F2zL2slGRfupeBE4`, SHA `0c8c724f972e089e77dd540c922bccec28b6f43524cbe5b14ee53d50be92c657`.

Provider final readback has `has_more=false` and verifies exactly **22 files = 20 payloads + 2 closure files**.

GitHub Evidence: `Tools/HighFidelityPatch/Evidence/HF26-Projectile-Collision-Audio-Particle.md`, final Evidence commit `f9becdeeebcc58d1db8128549c59111a7e3c8203`.

**HF26 formal acceptance: PASS. HF26 is now the only allowed formal input for any later cumulative HF stage.**

## Unity reconstruction state

AssetRipper ~3,149 objects; reconstructed project ~6,783 files. 173 game script types and 263 refs across 114 assets migrated. MainMenu/Board scenes restored. Fixed package set: UGUI 1.0.0, TMP 3.0.6, Core/URP 14.0.11, 2D Animation 9.1.1, Tilemap Extras 3.1.2, Burst 1.8.17, Collections 1.2.4, Mathematics 1.2.6, Visual Scripting 1.9.4. 67 package script types still require 67/67 Unity validation.

## Current decision gate — do not auto-open HF27

HF26 closes collision hit audio/particle post-processing for the already restored ground-hit path. Do **not** open HF27 because detector methods are adjacent or carry warnings.

Rerun native-vs-managed active-path closure over:

- `Projectile.CollisionDetect_Device` `0x060003E1` — PC `0x180379180`;
- `Projectile.CollisionDetect_Plant` `0x060003E3` — PC `0x1803799B0`;
- `Projectile.CollisionDetect_Zombie` `0x060003E4` — PC `0x180379D00`.

Include resolution/helper methods only when direct original-native call evidence proves the dependency belongs to the detector's active business path and its managed body has concrete loss. Known dependency candidates include `Collision_Device` (`0x060003E6`, PC `0x18037B010`) and `Collision_Zombie` (`0x060003EA`, PC `0x18037B170`); `Collision_Plant` must not be included merely by MethodDef adjacency because prior direct-xref review did not establish it as the detector's direct resolution path.

A later HF stage is allowed only when both conditions hold:

1. concrete managed loss/mis-reconstruction is proven from original PC native/metadata evidence; and
2. the affected method is materially active in gameplay.

If no remaining detector/dependency cluster satisfies both conditions, stop HF recovery and proceed to Unity `2022.3.44f1c1`, package restoration, 67/67 package-script validation, recovered Assembly-CSharp integration, compile/scene/gameplay validation, then necessary iOS platform adaptation.
