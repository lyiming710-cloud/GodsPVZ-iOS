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
- Cpp2IL source commit `5fb20304df698ffd3d0e664b2a698cd911dc9d57`.
- Ordinary native attribution: original MethodDef RID-1 -> Assembly-CSharp CodeGenModule `methodPointers[index]`. Generic MethodDefs additionally require original MethodSpec / generic-method-function attribution.
- Fixed ILSpyCmd / ICSharpCode.Decompiler `11.0.0.9375`; reproduced 56-DLL fixed-Cpp2IL reference ZIP SHA `fe091e5a5389c2491eebeff72c83c394097bef67ffd45a25cf7de01c9aad9ef1`.
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
- HF36 `291e22bffefdf98f0fca29475c485264ad5b9eb6cf8a934226d1e73a187ddfb0`
- **HF37 Projectile Initial Core — native-backed formal final — `412e12b39d1107c29a20027a37094e796d0059e0a3b8637d78233599148d0c56`.**

## Recent retained formal results

### HF31–HF33 — Projectile rotation / tracking / aim-euler

HF31 restores `Projectile.Rotating()`. HF32 restores `Projectile.Update_Tracking()` with minimum-fX enabled/attackable zombie selection and foreach Dispose/finally. HF33 restores `Projectile.Aim(Vector3)` and `Projectile.SetEulerAngles(float,float)` with native atan2 direction, speed-magnitude preservation and original sprite/animation/shadow/track Euler behavior. All formal gates and Drive 20+2 closures passed.

### HF34 — Projectile Runtime Support Core

Restores exactly `0x0600013E CreateAudioAtPoint/4`, `0x060003D6 Update_Time`, `0x060003DB Update_MoveTrack7`, `0x06000450 Zombie.GetPredictedPosition`. HF34 final `ead8dee5def2818ca8c64c36b4fe65f1830dbf873fae34dbc84d2f07b103d999`; all formal gates and Drive 20+2 closure passed. Evidence commit `edc56dd90e92131e2dba544bd0761f977ac7e712`.

### HF35 — Device Injury Status Core

Restores exactly `0x0600032B Device.InjuryStatusUpdate()` / RID 811 / PC `0x180349280`, including roadblock damage thresholds, sprite/particle/audio behavior and native ordered-float handling. HF35 final `ffd3e858a6ea15c1203a8c543e800b9181e0ab115fa7ab6a84d00975be5b947b`; all formal gates and Drive 20+2 closure passed. Evidence commit `d08323ea70f0600bb23543108e1b4cea2d5e8327`.

### HF36 — Armored Flag Wake-Up Core

Restores exactly `0x0600048C Zombie.ZC_ArmoredFlagWakeUpZombies()` / RID 1164 / PC `0x18036D560`: wake self when `isStant && !immune_wakeUp && !hide`; enumerate `board.zombieManager.zombieList`; for each eligible stunned zombie call `Path_Finding()` then `TranToWalk()`; preserve Enumerator Dispose/finally. HF36 final `291e22bffefdf98f0fca29475c485264ad5b9eb6cf8a934226d1e73a187ddfb0`; all formal gates and Drive 20+2 closure passed. Evidence commit `6206eb4c2ca07f72edd108fe0f8e6168f0b36062`.

### HF37 — Projectile Initial Core

Restores exactly:

- `0x060003ED Projectile.Initial<T>/6` — RID 1005;
- `0x060003EE Projectile.Initial<T>/7` — RID 1006;
- `0x060003EF Projectile.Initial<T>/10` — RID 1007.

Generic attribution is closed through original MethodSpec/generic-method tables. In particular, `Plant.KillEvent` encoded MethodInfo `0xC0024671` resolves under metadata v31 to MethodSpec 74552 -> global methodDefinitionIndex 42770 -> `0x060003ED`, method instantiation 2453 -> typeDefinition `0x1457` -> `Plant`; this is therefore `Initial<Plant>` at metadata level.

Accepted behavior: /6 and /7 forward default z/angular arguments to /10. /10 sets fX/fY/fZ, speed, zSpeed, movementTracks, `SetEulerAngles(0,zAngular)`, angular speed/acceleration, track-1 `zAcceleration=-2025`, calls `Moving_SetNewPosition()`, writes the native previousPosition sentinel, and assigns Plant/Zombie origin through runtime type tests. The `acceleration` parameter is intentionally unused, matching native.

Formal validation:

- formal HF36 input SHA-verified `291e22bffefdf98f0fca29475c485264ad5b9eb6cf8a934226d1e73a187ddfb0`;
- patcher build head `58b9316853725a0c5da0c51960ce7e381fa56473`, workflow `34423760532` PASS, artifact ID `10131864829`, SHA `4b33df295489a157ca5008734ea7321d5b60641703b55b44c0c98416f49ae478`;
- independent double patch -> byte-identical `412e12b39d1107c29a20027a37094e796d0059e0a3b8637d78233599148d0c56`, stderr 0;
- Cecil reopen /6 `13 IL / 36 bytes / 0 EH`, /7 `13/33/0`, /10 `75/213/0`;
- fixed ILSpy 11.0.0.9375 + 56 refs: all three exit 0, stderr 0, Cpp2IL refs 0, issue markers 0;
- HF36 whole IL reproduced `6a0c79a8c0b758c35d61e7edba8da5740dd72cb82426f976c064ad358b7d5391`; HF37 whole IL `dd045b66df4989f3e617ec2b4ee7f8f398dbba28b9305b31433de5314cffe655`;
- prior HF35->HF36 diff reproduced first at `83f0cd40ea1d266d2f78972b18ccc177fd77a9b12792109e40a36db9e134250e`;
- MethodDef `2317 -> 2317`, normalized non-method skeleton identical, changed exactly `0x060003ED`, `0x060003EE`, `0x060003EF`;
- HF36->HF37 semantic diff `125ca6d7ec2e0322056bc7014bae7aed911a9953973e804009cc8ae8d6c35d0b`;
- permanent RecoveryAudit commit `04355be372d0ae6c928bf7e2b8e5c2a1647f0903`, run `34424112325` PASS, artifact ID `10131987391`, SHA `9a9d594eddec824e56202093788e243d86410cdbb0505bf1035e17c16130a1e1`; published auditor OPEN1/OPEN2 passed all retained targets and ended `RECOVERY_AUDIT_OK`;
- Evidence commit `b4c50aece1eb0e5f4f27096eb510ffc5b88ae394`;
- Drive folder `1L7mnzN8udi4HX-NiusZsOOgX4tp9S_9u`;
- payload manifest `1f49bqZdssXrAuAk0ATh2JIbjSUPrblwj`, SHA `e47ceee4b97030c4f94b304c707b857c47a038c895f0e5b99ebe1c39b5b0a22e`;
- Evidence-FINAL `13pw2zMplFFR_mglXITNwI95B0fT6Pyxd`, SHA `6de264117c6733c07832bfddd74bb709fbe63bce405019c861bb22927e8efcc8`;
- SHA256SUMS-FINAL `1g76O9rpH2fJtTL3hXH6MmYCR2PZbUBs3`, SHA `13c92441ac6d7b51013eb3a0d4de33746f878c32d5ecfb6a0c84c13c4fd2ef62`;
- final provider readback exactly 22 files = 20 payloads + 2 closure files.

**HF37 formal acceptance: PASS. HF37 is now the only allowed formal input for any later cumulative HF stage.**

## Unity reconstruction state

AssetRipper ~3,149 objects; reconstructed project ~6,783 files. 173 game script types and 263 refs across 114 assets migrated. MainMenu/Board scenes restored. Fixed package set includes UGUI 1.0.0, TMP 3.0.6, Core/URP 14.0.11, 2D Animation 9.1.1, Tilemap Extras 3.1.2, Burst 1.8.17, Collections 1.2.4, Mathematics 1.2.6, Visual Scripting 1.9.4. 67 package script types still require 67/67 Unity validation.

## Current decision gate — do not auto-open HF38

Re-run the original-native-vs-managed decision gate from the new HF37 formal cumulative DLL.

`Plant.KillEvent` is the first candidate to reconsider because its previously blocking `Projectile.Initial<Plant>` dependency is now behaviorally and formally closed in HF37. Do not assume that makes `Plant.KillEvent` automatically eligible. Open HF38 only if both are independently proven against the HF37 cumulative assembly and original PC native body:

1. original PC native/metadata demonstrates concrete managed loss or mis-reconstruction in `Plant.KillEvent`; and
2. it is materially active in gameplay and every remaining native dependency in its body is behaviorally closed.

Do not use warning count, MethodDef adjacency, shared-stub xref centrality or cosmetic decompiler quality as a gate. After any accepted HF38 candidate, re-run a fresh active-path residual scan rather than chaining stages automatically.

If no remaining candidate satisfies both gates, stop HF managed recovery and proceed to 67/67 Unity/package validation, integrate the HF37 cumulative Assembly-CSharp recovery, compile and validate MainMenu/Board/gameplay paths, and only then perform necessary iOS adaptation.
