# Recovery status

## Current strategy

Use PC x86-64 IL2CPP as the primary gameplay source, Android native second, then original metadata/assets/JSON. Cpp2IL/ILSpy are attribution and managed-reconstruction aids, not authoritative source.

A stage is not final until original native attribution, formal-input SHA validation, deterministic independent patching, Cecil reopen, permanent/cumulative RecoveryAudit OPEN1/OPEN2, fixed ILSpy readback, whole-assembly semantic isolation, GitHub Evidence, Google Drive archive, provider readback and STATUS closure all pass.

Historical per-stage detail remains preserved in Git history and `Tools/HighFidelityPatch/Evidence/`. This STATUS file records the authoritative cumulative chain and current next-stage gate.

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
- **HF47 EnemyManager Wave Progression Core — native-backed formal final — `e94942c50cdce952ca37f746bc5c3ab83b49106ae0837a077045f54f6ecb9533`.**
- **HF48 Ladder Runtime Core — native-backed formal final — `6c1eb1468f398610cc61ea89b7f3a9409f6baeb451e6df0ae6243b28af50e48e`.**

HF48 is the only allowed formal cumulative input for any later HF stage.

## Latest formal stages

### HF47 — EnemyManager Wave Progression Core

Restores exactly `EnemyManager.DispatcheWave(Wave)`, `EnemyManager.DispatcheZombie(Enemy)`, and `EnemyManager.TimeUpdate()`. Final `e94942c50cdce952ca37f746bc5c3ab83b49106ae0837a077045f54f6ecb9533`. GitHub Evidence commit `c9e53de6dcb58bcdbf2a69ca9eb7bcb59869e0ff`; Drive folder `1YKFGYZXZYTWwfqqFp3-v4PrkMX1ux_ii`; final provider readback exactly 22 files.

### HF48 — Ladder Runtime Core

Restores exactly six original native-backed MethodDefs:

- `0x060001B3 EnemyManager.DispatcheLadderWave()` / RID 435 / PC `0x1803170D0`;
- `0x060001B4 EnemyManager.DispatcheSPHWave()` / RID 436 / PC `0x1803172F0`;
- `0x06000273 ZombieManager.Update()` / RID 627 / PC `0x180342AC0`;
- `0x06000274 ZombieManager.Update_Ladder()` / RID 628 / PC `0x1803427B0`;
- `0x0600027D ZombieManager.TriggerLadder()` / RID 637 / PC `0x180342620`;
- `0x060002A4 EnemyPath..ctor(int,int,float,BoardConfig)` / RID 676 / PC `0x1803299E0`.

Accepted behavior closes the active ladder runtime path: traversable-row ID16 ladder waves; ID17 SPH waves and original `prePath.Insert(0,new EnemyPath(...999f...))`; the real `ZombieManager.Update -> Update_Ladder` call; countdown/popup/audio/pause/timeScale behavior; ladder wave cadence and tenth-event SPH wave; complete TriggerLadder list enumeration/Dispose semantics; and the original four-argument EnemyPath position construction. No defensive iOS guards or guessed gameplay fallbacks were added.

Formal validation:

- formal HF47 input SHA-verified `e94942c50cdce952ca37f746bc5c3ab83b49106ae0837a077045f54f6ecb9533`;
- patcher build head `a756f88bdc5d502ea06a19146ae33362e2564bef`, workflow `34543329650` PASS, artifact ID `10178091954`, SHA `6708200c297776a9685a7ebca56eed7d77340825ce4543411dff03a0130ce19b`;
- independent double patch -> byte-identical `6c1eb1468f398610cc61ea89b7f3a9409f6baeb451e6df0ae6243b28af50e48e`, both exit 0 with zero stderr;
- Cecil reopen: `0x060001B3` 64 IL / 186 bytes / 0 EH; `0x060001B4` 89 / 260 / 0; `0x06000273` 3 / 7 / 0; `0x06000274` 97 / 308 / 0; `0x0600027D` 44 / 115 / 1; `0x060002A4` 27 / 66 / 0;
- fixed ILSpy 11.0.0.9375 + 56 refs: all six targets exit 0, stderr 0, Cpp2IL/Unknown/NotImplemented/invalid stack-type-comparison/warning-error markers 0;
- HF47 whole IL reproduced `e30a0559b3b2c6fbbafcc1783287a94a83364602de3891222f469556eb91438d`; HF48 whole IL `47726f82264e97ecc44a9fdb5a41fcb447c76da57a16cce3c2c20622e7ecb0bd`;
- HF47->HF48 semantic diff `94bbc39d24b4aabc5c31b425d42badabb88fe916452e551d11e24eea4f5b7b1b`;
- Method blocks `2317 -> 2317`; normalized non-method/method-signature skeleton byte-identical; changed exactly the six target tokens;
- distinct nonzero method-body RVAs `2139 -> 2140`, expected solely because authorized `ZombieManager.Update()` changes from a damaged empty/shared body to its original 7-byte body;
- HF48 MethodDef table SHA `c7d4dfa7af316c26366dfa2f5a1a75b5c47d418bf1d5c269f014ac6ec99f6aab`;
- corrected cumulative RecoveryAudit build head `54cedf35bfdfd8244155e625dbf497d250f2d40e`, workflow `34546011331` PASS, artifact ID `10179032692`, SHA `3867f6e6116cdc6deaacdf7ba6f24b0087cf69aca6cc593d528fc0f5df9e3154`; unchanged historical auditor covers HF1-HF46, and the HF48 cumulative auditor locks the HF48 whole-file SHA while checking HF47+HF48 late-stage targets; two independent corrected composite executions ended `RECOVERY_AUDIT_OK` and `HF48_AUDIT_OK` with stderr 0;
- GitHub Evidence commit `2edfe75f17a42e27d0f9a3e6ee7be15d2ebf8a47`;
- Drive folder `1945a0ednBRxXQcnzw9RLR4icPXJVTUXY`;
- cumulative DLL `1D-vEXZjQVW0CWkB0FIkh9ckVDeQ8lHyk`;
- payload manifest `1ZMNQCf4n170MDfS4PSVeJAyrxRn4XrmC`, SHA `3ac98631abd123e92c9c8175de67776bafa41dc2ef6835a7ec18db039ca01534`; pre-closure provider readback exactly 20 files with all SHA values matched;
- Evidence-FINAL `1mDd6TwSp8Y4mvK-SNk8_9lXLTVKBgdI5`, SHA `c9cff3cb6fff06b253be1c5926da3e7c35c6e7fdaea7069bd31cd3f5cc3f89cd`;
- SHA256SUMS-FINAL `1zSsy5KDgMsZN5uqPYA6BpSwvo-BiszHq`, 21 entries, SHA `d95ba51a5fce83b891da1844c6df93d493a2e3ff6c0d1a5dd1bc66365c142d0d`;
- final Google Drive provider readback exactly 22 files; both FINAL files were downloaded back from Drive and byte-hashed to the recorded SHA values.

**HF48 formal acceptance: PASS. HF48 is now the only allowed formal input for any later cumulative HF stage.**

## Unity reconstruction state

AssetRipper ~3,149 objects; reconstructed project ~6,783 files. 173 game script types and 263 refs across 114 assets migrated. MainMenu/Board scenes restored. Stage9.1 remains the reconstructed Unity project baseline. Package scripts still require 67/67 validation before final Unity integration.

## Decision gate — do not auto-open HF49

Run a fresh residual active-path scan against the HF48 cumulative assembly. A later HF stage may open only if an original-PC-native-backed MethodDef simultaneously has concrete managed loss/mis-reconstruction, material gameplay reachability, and behaviorally closed native dependencies. Warning count, MethodDef adjacency, shared-stub xref centrality, or cosmetic decompiler quality alone are not gates.

The HF48 ladder cluster is now clean. Remaining markers include the broad `EnemyManager.Start/CreateEnemyList/CreateEnemySelecter/ExtendFlagList/FillFlagList/FlushedWavelenth/GetWave/SetPreZombie/SetTargetGrid` initialization cluster, whose dependency graph includes damaged `EnemySelecter.FillWave`, plus residual ZombieManager initialization/board-entry/list-utility paths such as `AllFindPath`, `BGMPasue`, `BGMUnPasue`, and `KillAll`. These are candidates only and must be independently proven as live reconstruction loss with native dependency closure before any HF49 promotion.

If no remaining candidate satisfies the gate, stop managed HF recovery and proceed to 67/67 Unity/package validation, integrate the HF48 cumulative Assembly-CSharp recovery into Stage9.1, compile and validate MainMenu/Board/gameplay paths, and only then perform necessary iOS adaptation.
