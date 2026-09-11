# Recovery status

## Current strategy

Use PC x86-64 IL2CPP as the primary gameplay source, Android native second, then original metadata/assets/JSON. Cpp2IL/ILSpy are attribution and managed-reconstruction aids, not authoritative source.

A stage is not final until original native attribution, formal-input SHA validation, deterministic independent patching, Cecil reopen, permanent/cumulative RecoveryAudit OPEN1/OPEN2, fixed ILSpy readback, whole-assembly semantic isolation, GitHub Evidence, Google Drive archive, provider readback and STATUS closure all pass.

Historical per-stage detail remains preserved in Git history and `Tools/HighFidelityPatch/Evidence/`. This STATUS records the authoritative cumulative chain and current next-stage gate.

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
- HF37 `412e12b39d1107c29a20027a37094e796d0059e0a3b8637d78233599148d0c56`
- HF38 `25ca8b5afe45930dd39517c2477a96db520d067c067493c334f107201a973b5d`
- HF39 `7c6360c79852f94a86e560367f25c79298d62d3e1bf86027f86112d9fc333f6e`
- HF40 `726c7ceda476cb0034b9394964407a72d2ab7a1a92424dee9c81e118f5590cce`
- HF41 `2e03010cab5a4776243ecf3509402f387c39ccb9faa039876c85ad629ba7fcfa`
- HF42 `687a973f640fc1be1e092fc75f6d09e697995563c605763d1195751af91dc7d6`
- HF43 `c5d998c691f5e32edb5bf48f7d5da3d9ed03f362cb049c3fa3ed8a11617ee078`
- HF44 `fe5e73abead459121ea00c0f91be7e7abc8b3392eb924458b9c0c0352b444118`
- HF45 `a34feef56319f047a9f60caef0028db9592536b89fa484c41e371a826d6ccd5a`
- HF46 `900d3aabbf79b094a6b412f7d869cc5285ce206a2d17501b69fc3cd8651beefd`
- HF47 `e94942c50cdce952ca37f746bc5c3ab83b49106ae0837a077045f54f6ecb9533` — EnemyManager Wave Progression Core.
- HF48 `6c1eb1468f398610cc61ea89b7f3a9409f6baeb451e6df0ae6243b28af50e48e` — Ladder Runtime Core.
- **HF49 `6dff7975abd2b62d2cc40564a17f21518526d8dae2ce7f53a7be3e49c2114f69` — EnemyPath Arrival Core — native-backed formal final.**

HF49 is the only allowed formal cumulative input for any later HF stage.

## Latest formal stages

### HF47 — EnemyManager Wave Progression Core

Restores `EnemyManager.DispatcheWave(Wave)`, `EnemyManager.DispatcheZombie(Enemy)`, and `EnemyManager.TimeUpdate()`. Final `e94942c50cdce952ca37f746bc5c3ab83b49106ae0837a077045f54f6ecb9533`. Evidence commit `c9e53de6dcb58bcdbf2a69ca9eb7bcb59869e0ff`; Drive folder `1YKFGYZXZYTWwfqqFp3-v4PrkMX1ux_ii`; final provider readback exactly 22 files.

### HF48 — Ladder Runtime Core

Restores `EnemyManager.DispatcheLadderWave`, `EnemyManager.DispatcheSPHWave`, `ZombieManager.Update`, `ZombieManager.Update_Ladder`, `ZombieManager.TriggerLadder`, and `EnemyPath..ctor(int,int,float,BoardConfig)`. Final `6c1eb1468f398610cc61ea89b7f3a9409f6baeb451e6df0ae6243b28af50e48e`. Evidence commit `2edfe75f17a42e27d0f9a3e6ee7be15d2ebf8a47`; Drive folder `1945a0ednBRxXQcnzw9RLR4icPXJVTUXY`; final provider readback exactly 22 files. MethodDef `2317 -> 2317`; distinct nonzero body RVAs `2139 -> 2140` solely because authorized `ZombieManager.Update()` was restored from an empty/shared body to its original 7-byte body.

### HF49 — EnemyPath Arrival Core

Restores exactly one original native-backed MethodDef:

- `0x060002A3 EnemyPath.ArrivalTest(Zombie)` / RID 675 / PC `0x180329620`.

Accepted behavior restores the original path-arrival predicate used from live `Zombie.Path_Test()` calls in the `Zombie.Update()` movement path: target position comes from `father.board.boardConfig.GetZombiePosition(gridX,gridY)`; arrival is true only when both absolute X/Y deltas are strictly below `Zombie.deadzone_distance`, or when the path is `original` and both associated `zombie` and `plant` compare Unity-null. No defensive null fallback, iOS guard, or guessed gameplay behavior was added. `Zombie.Path_Test()` itself remains outside HF49.

Formal validation:

- formal HF48 input SHA-verified `6c1eb1468f398610cc61ea89b7f3a9409f6baeb451e6df0ae6243b28af50e48e` before mutation;
- patcher build head `904ba2fad29112b5be9b6ffe5a28047b5f9f0a46`, workflow `34548504310` PASS, artifact ID `10179914173`, SHA `eaa26b2547e90c0c1735683f78da439ab549f58ed536c6d5a9fa1d366ab7849a`;
- independent double patch -> byte-identical `6dff7975abd2b62d2cc40564a17f21518526d8dae2ce7f53a7be3e49c2114f69`, both exit 0;
- Cecil reopen: `0x060002A3` 52 IL / 137 bytes / 0 EH; zero Cpp2IL helper references;
- fixed ILSpy 11.0.0.9375 + 56 refs: target exit 0, stderr 0, target-local Cpp2IL/Unknown/NotImplemented/invalid stack-type-comparison markers 0;
- HF48 whole IL `47726f82264e97ecc44a9fdb5a41fcb447c76da57a16cce3c2c20622e7ecb0bd`; HF49 whole IL `c60eb750651402273ec5cc7ac98520c500dd361568cd2214c42518ed7a737455`;
- HF48->HF49 semantic diff `600fa54b571638988192906f1c53c16d38c944dd20d1c4855be7a636bdaac8ca`;
- MethodDef `2317 -> 2317`, emitted bodies `2297 -> 2297`, distinct nonzero body RVAs `2140 -> 2140`, normalized non-method/method-signature skeleton identical, changed exactly `0x060002A3`;
- HF49 MethodDef table `8da2a6270cecdba89df7cd269b479a74ad9cb109a2bcf9d63ad65cf36696370b`;
- cumulative RecoveryAudit build head `4ca357f60140fb947565a3f214237d0804807998`, workflow `34548784207` PASS, artifact ID `10180014456`, SHA `9faded8ec48694bf9ed9bf94c0934fd283138d48b7dabdcd995d4bb2edadc58d`; unchanged historical auditor covers HF1-HF46, cumulative HF49 auditor locks HF49 SHA and rechecks all 10 HF47-HF49 late-stage targets; two independent actual executions ended `RECOVERY_AUDIT_OK` and `HF49_AUDIT_OK`, stderr 0, byte-identical formal log SHA `cdb51adbb05a842361a07896a96078d479708ce5788064bd62c90a8b64dc1011`;
- GitHub Evidence commit `b3d3747710dcba5aea3f5a73593fcf4cacfa6d80`;
- Drive folder `1LsjyP8K8z8CeW75Q0Vdp8r0sao39XoR3`;
- cumulative DLL `1Go2yumYd0VIxJN_hNkkbuwz1eBwCjwUb`;
- payload manifest `1AOyYemW1ybD9yacidtAbG0uoh5lMqZHo`, SHA `3495fb0493f4beb995f43e3f0fb644f6eb8bbf0d070be0cd417437f0d74ac44e`; pre-closure provider readback exactly 20 files with 19/19 ordinary payload SHA equality and payload-manifest readback equality;
- Evidence-FINAL `1XsJg_pkgAX7Dh6MfLaeFWHUD1VGfuVSO`, SHA `cceacc6eb11baceb4b7419b365fa7d0320990323855eab9805456ca098454730`;
- SHA256SUMS-FINAL `1w-XP1ly_w2780wjk8Ay-FEm7WHdNjwP9`, 21 entries, SHA `5b52d4f04fba7a4f394e73ffdd551d1467a9fc888582ff49e18ef232d8feac42`;
- final Google Drive provider readback exactly 22 files; both FINAL files were downloaded back and byte-hashed to the recorded SHA values.

**HF49 formal acceptance: PASS.**

## Unity reconstruction state

AssetRipper ~3,149 objects; reconstructed project ~6,783 files. 173 game script types and 263 refs across 114 assets migrated. MainMenu/Board scenes restored. Package scripts still require 67/67 validation before final Unity integration. The cumulative managed DLL to integrate is now HF49, not HF48.

## Decision gate — do not auto-open HF50

Run a fresh residual active-path scan against the HF49 cumulative assembly. A later HF stage may open only if an original-PC-native-backed MethodDef simultaneously has concrete managed loss/mis-reconstruction, material gameplay reachability, and behaviorally closed native dependencies.

Strongest current residual candidates:

- `Zombie.Path_Test()` is live from `Zombie.Update()`, calls the now-restored `EnemyPath.ArrivalTest`, and still contains two `Cpp2ILHelpers.NoteDecompilerIssue` calls plus malformed managed reconstruction. Its original PC entry is `0x180366030`, but its substantially larger native body and dependencies are not yet fully closed; it is a candidate, not an authorized HF50 patch.
- `EnemyPath.DistanceStatistics(...)` retains invalid managed stack/type reconstruction and has live callers from `Zombie.CreateStartPrePath` and `Zombie.ZC_SnowbeastSeekBait`; it also requires independent native closure.
- `Zombie.Update_Path()` retains suspicious managed reconstruction but currently has no callsite in the cumulative assembly and is not promoted on warning count alone.

If no residual candidate satisfies all gates, stop managed HF recovery and proceed to 67/67 package validation, integrate the HF49 cumulative Assembly-CSharp recovery into the reconstructed Unity project, compile and validate MainMenu/Board/gameplay paths, and only then perform necessary iOS adaptation.
