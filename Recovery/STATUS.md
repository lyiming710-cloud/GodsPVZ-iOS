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
- **HF31 Projectile Rotating Runtime Core — native-backed formal final — `a4d001581430fe440e50feb37f3abd189a1ae86a00f4fe41114ae3f9b99386d2`.**

## Recent retained results

### HF23 — Zombie Hurt Core

Restores exactly `0x06000456–0x0600045E` Hurt_Armor1/2, Artillery, Ashes, Body, FinalDamageReduction, Normal, Real, Throughout with native armor/body DEF, non-real 0.1 floor, real bypass, `MathF.Round`, PoleCommander cap 0.95, iceCube fire_ice 0.2 and throughout truncation.

### HF24 — Projectile Runtime Core

Restores exactly `0x060003D2 Projectile.Update()`: sorting-order truncation, pause/start gates, movement tracks 3/4/5/6/7, native-order delta-time integration, shadow sync, out-of-map destruction, CollisionDetect, tracking/rotation, LightSaber cleanup and previousPosition.

### HF25–HF30 — Projectile collision chain

HF25–HF30 formally restore `CollisionDetect`, Ground, AudioParticle, Device/Zombie resolution, and Plant/Zombie/Device detectors. The recovered collision dispatcher path is now closed end-to-end into damage and hit post-processing.

### HF31 — Projectile Rotating Runtime Core

Restores exactly `0x060003F6 Projectile.Rotating()` — PC `0x18037BE20`.

Accepted behavior:

- real `Projectile.speed` field is `UnityEngine.Vector3`; native reads `speed.x`;
- negative or NaN/unordered `speed.x` selects Y local Euler `180f`, otherwise `0f`;
- `angularAcceleration != 0` integrates `angularSpeed += Time.deltaTime * angularAcceleration`;
- non-zero angular speed rotates `projectileSprite` and `projectileAnimation` around Z/forward in `Space.World`.

HF31 formal validation:

- formal HF30 re-fetched and SHA-verified `6ccccf685950edba428b51dbe3aa95e511caa467d134dd4202939e19ef655c24`;
- two earlier candidate outputs were rejected by fixed ILSpy and never propagated;
- final patcher workflow `34371494005` PASS, artifact SHA `9433d5beb14d9f7f38731d52b269def671533dff414d008585f49d0feafaa150`;
- deterministic double patch -> final SHA `a4d001581430fe440e50feb37f3abd189a1ae86a00f4fe41114ae3f9b99386d2`;
- Cecil reopen Rotating `81 IL / 277 bytes / 0 EH`;
- fixed ILSpy 11.0.0.9375 member readback PASS;
- HF30 whole IL reproduced exactly; HF31 whole IL SHA `cef4f8e34d87fb1409488b9e915eb8df39b1d4e699612032192e4be83be810ee`;
- MethodDef `2317 -> 2317`, normalized non-method skeleton identical, exactly `0x060003F6` changed;
- semantic diff SHA `673a14dcd0b0e3bfa6b1b16bcaa09fed4d5d435b744f04ca406534125fc26638`;
- RecoveryAudit commit `15799da87b106b2679ac6a402ae671adfb74f5f7`, workflow `34372146093` PASS, artifact SHA `6eaab051eb29d3a47360cfa0436af05642090ee07164a4ec951d25331721fa65`;
- OPEN1/OPEN2 `320 types / 2317 methods / 2297 bodies`, Rotating `81/277`, `RECOVERY_AUDIT_OK`.

HF31 Drive archive:

- folder `1yJ8yC_nn7DdCPEgvhnnLBwcPY01HWvSm`;
- cumulative audited DLL `18fn9_hdgHdKXQRMJfIcf0lfDkbwuZdam`;
- patcher `1Z1o2wWJsVXN356OcxZ9eBIe46loBTRCO`;
- RecoveryAudit `1UzPYxEIWpuQY8Gi3oi__gT8trjEG7enS`;
- fixed ILSpy `1D_KEu4afQ03cXftXyi9EPc-u3oXGj9uU`;
- native evidence `1kAYoWXZ-Zp0JKzWb19G4jJwg3TECqXCR`;
- payload manifest `1_CWYlvylVsd76foHRN1vaT6_zhbomfIe`, SHA `dc6bc000da460c94af069361bd8682c58b40d05e70edafab4a4196c9f18ee4c8`;
- Evidence-FINAL `1wMiI37IqCcp-IYN8IspFl1l4_kvUC_Gw`, SHA `e971903841c245052b66cc37d265948f4c3b5c535d25baa55271f3260a8ca50d`;
- SHA256SUMS-FINAL `1Ikzx_u7IfS1y88uhqq9KEkhA75Ph6-Az`, SHA `94fa970a0a175ad9546529aff1246d0fa66e924f9c5e592be8d1ccb10b008556`;
- provider final readback `has_more=false`, exactly 22 files = 20 payloads + 2 closure files.

Evidence: `Tools/HighFidelityPatch/Evidence/HF31-Projectile-Rotating-Runtime-Core.md`.

**HF31 formal acceptance: PASS. HF31 is now the only allowed formal input for any later cumulative HF stage.**

## Unity reconstruction state

AssetRipper ~3,149 objects; reconstructed project ~6,783 files. 173 game script types and 263 refs across 114 assets migrated. MainMenu/Board scenes restored. Fixed package set includes UGUI 1.0.0, TMP 3.0.6, Core/URP 14.0.11, 2D Animation 9.1.1, Tilemap Extras 3.1.2, Burst 1.8.17, Collections 1.2.4, Mathematics 1.2.6, Visual Scripting 1.9.4. 67 package script types still require 67/67 Unity validation.

## Current decision gate — do not auto-open HF32

HF31 closes `Projectile.Rotating()` but not all remaining runtime helpers. Re-run native-vs-managed active-path scanning on methods actually reached by recovered runtime code, prioritizing:

- `Projectile.Update_Tracking`;
- `Projectile.SetEulerAngles`;
- `Projectile.Aim` and any creation/aim helper proven active.

Open HF32 only when both are true:

1. original PC native/metadata proves concrete managed loss or mis-reconstruction; and
2. the method is materially active in gameplay.

Do not open a stage from warning count, MethodDef adjacency or shared-stub xref centrality alone. If no remaining candidate passes both gates, stop HF managed recovery and proceed to 67/67 Unity/package validation, recovered Assembly-CSharp integration, compile/scene/gameplay validation and only then necessary iOS adaptation.
