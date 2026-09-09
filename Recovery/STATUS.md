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
- **HF30 Projectile Device Collision Detector — native-backed formal final — `6ccccf685950edba428b51dbe3aa95e511caa467d134dd4202939e19ef655c24`.**

## Recent retained results

### HF23 — Zombie Hurt Core

Restores exactly `0x06000456–0x0600045E` Hurt_Armor1/2, Artillery, Ashes, Body, FinalDamageReduction, Normal, Real, Throughout. Accepted semantics include native armor/body DEF, non-real 0.1 floor, real bypass, `MathF.Round`, PoleCommander cap 0.95, iceCube fire_ice 0.2, throughout truncation and correct armor/body side effects.

### HF24 — Projectile Runtime Core

Restores exactly `0x060003D2 Projectile.Update()`: sorting-order truncation, pause/start gates, movement tracks 3/4/5/6/7, native-order delta-time integration, shadow sync, out-of-map destruction, CollisionDetect, tracking/rotation, LightSaber cleanup and previousPosition.

### HF25–HF30 — Projectile collision chain

- HF25 restores `0x060003E0 CollisionDetect()` and `0x060003E2 CollisionDetect_Ground()`.
- HF26 restores `0x060003EB Collision_AudioParticle()`.
- HF27 restores `0x060003E6 Collision_Device(Device)` and `0x060003EA Collision_Zombie(Zombie)`.
- HF28 restores `0x060003E3 CollisionDetect_Plant(bool sameCamp)`.
- HF29 restores `0x060003E4 CollisionDetect_Zombie(bool sameCamp)`.
- HF30 restores `0x060003E1 CollisionDetect_Device(bool sameCamp)` — RID 993 — PC `0x180379180`.

HF30 accepted ordinary Device collision semantics: enumerate `board.deviceManager.deviceList` with disposal, `CanAttacked(damage)`, camp/sameCamp gate, ordered X/Y AABB and Z/H overlap, then first-match `Collision_Device` dispatch. IDs 19/23 retain the native two-phase new-contact path using `device.transform.position` for current contact and projectile `previousPosition` for persistent-contact suppression, collect new contacts before resolution, then inline AreaDamage/Device.TakeDamage -> Collision_AudioParticle with the `gameObject.activeSelf` break gate.

HF30 formal validation:

- formal HF29 re-fetched and SHA-verified `8c13a6638276a0e201ee39192545bbbe73703a7251f4188080dcfcf0c4d55590`;
- patcher workflow `34362687707` PASS, artifact SHA `970255e57dbcf414c06fc3dd36303d2a4db12e2b930f669510c9571d930313b6`;
- deterministic double patch -> final SHA `6ccccf685950edba428b51dbe3aa95e511caa467d134dd4202939e19ef655c24`;
- Cecil reopen E1 `311 IL / 904 bytes / 3 EH`;
- fixed ILSpy 11.0.0.9375 member readback PASS;
- HF29 whole IL reproduced exactly; HF30 whole IL SHA `4fe6bb80fe5de582c67c3e14427b526c244ab5f6dc9cba7cad0ce28970f2136e`;
- MethodDef `2317 -> 2317`, normalized non-method skeleton identical, exactly E1 changed;
- semantic diff SHA `59b9b5b2a1d7fcaf098b9185eea13ca8bb977183fcf4a8b8da25f2384d4735ba`;
- RecoveryAudit commit `917087166a26893fce6ede5afefb02924206f44d`, workflow `34364225011` PASS, artifact SHA `d6157d9ff1fb7c139fd989d217f929f38b0b07017de94f03d99d8ab6499101db`;
- OPEN1/OPEN2 `320 types / 2317 methods / 2297 bodies`, E1 `311/904`, `RECOVERY_AUDIT_OK`.

HF30 Drive archive:

- folder `1JXbnqb-JQ7ofCyL26NpieGgIVkp1AUdF`;
- cumulative audited DLL `1sjYzcOCDcRu7adJ-paQUKb95uHeMdTac`;
- patcher `1uPEcJcnx_tvfzrUFp8MYO4-WukwkiYHZ`;
- RecoveryAudit `1Li2VnNyhHisScOfvGhsTymoPDbtgD1MB`;
- fixed ILSpy `1Z1l7hSzuByFFPLD_ZlnYYVJ6fup0yjAZ`;
- native evidence `137zp1Wp15xZqMMApJ_TWILvnvi10eBzs`;
- payload manifest `1tn28Q06XCIOZV6obByGMjIwMY8ov79Lu`, SHA `edd247bfbfe66bd6c1333a03e35988c649ad82e7f6a3e80437fab484ff916663`;
- Evidence-FINAL `1XGdnMedJ9VCP4xRJNZARZJJze5-OepCx`, SHA `8ede9b565fa0e18fc43ad023c481715326d32936dd645ec5fba7388f681ed951`;
- SHA256SUMS-FINAL `1Gu2xYzt0B_jC7VJD7spznywlE_D6pG83`, SHA `e5fa7dd034e1358680eb65450c2a669421b5426ba39aec1ecb8fd7561c649afe`;
- provider final readback `has_more=false`, exactly 22 files = 20 payloads + 2 closure files.

Evidence: `Tools/HighFidelityPatch/Evidence/HF30-Projectile-Device-Collision-Detector.md`.

**HF30 formal acceptance: PASS. HF30 is the only allowed formal input for any later cumulative HF stage.**

## Unity reconstruction state

AssetRipper ~3,149 objects; reconstructed project ~6,783 files. 173 game script types and 263 refs across 114 assets migrated. MainMenu/Board scenes restored. Fixed package set includes UGUI 1.0.0, TMP 3.0.6, Core/URP 14.0.11, 2D Animation 9.1.1, Tilemap Extras 3.1.2, Burst 1.8.17, Collections 1.2.4, Mathematics 1.2.6, Visual Scripting 1.9.4. 67 package script types still require 67/67 Unity validation.

## Current decision gate — do not auto-open HF31

HF30 closes the final unrecovered direct target of the recovered `Projectile.CollisionDetect()` dispatcher. Re-run a broad remaining native-vs-managed active-path scan rather than continuing by warning count.

Open another HF stage only when both are true:

1. original PC native/metadata proves concrete managed loss or mis-reconstruction; and
2. the method is materially active in gameplay.

Priority scan targets include remaining methods actually called by the recovered projectile runtime (`Update_Tracking`, `Rotating`, `SetEulerAngles`, and any creation/aim helpers proven active), plus other gameplay-critical managed methods with independent native bodies. Shared-stub xref centrality and isolated Cpp2IL warnings are insufficient.

If no remaining candidate passes both gates, stop HF recovery and proceed to 67/67 Unity/package validation, recovered Assembly-CSharp integration, compile/scene/gameplay validation and only then necessary iOS adaptation.