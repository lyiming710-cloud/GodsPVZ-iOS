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
- HF49 `6dff7975abd2b62d2cc40564a17f21518526d8dae2ce7f53a7be3e49c2114f69` — EnemyPath Arrival Core.
- **HF50 `74ae15e2ed7f1626ecea7741d83803208ea0e8381306f1900f7a46c198c425cb` — Path Support Dependency Core — native-backed formal final.**

HF50 is the only allowed formal cumulative input for HF51 or any later HF stage.

## Latest formal stages

### HF47 — EnemyManager Wave Progression Core

Restores `EnemyManager.DispatcheWave(Wave)`, `EnemyManager.DispatcheZombie(Enemy)`, and `EnemyManager.TimeUpdate()`. Final `e94942c50cdce952ca37f746bc5c3ab83b49106ae0837a077045f54f6ecb9533`. Evidence commit `c9e53de6dcb58bcdbf2a69ca9eb7bcb59869e0ff`; Drive folder `1YKFGYZXZYTWwfqqFp3-v4PrkMX1ux_ii`; final provider readback exactly 22 files.

### HF48 — Ladder Runtime Core

Restores `EnemyManager.DispatcheLadderWave`, `EnemyManager.DispatcheSPHWave`, `ZombieManager.Update`, `ZombieManager.Update_Ladder`, `ZombieManager.TriggerLadder`, and `EnemyPath..ctor(int,int,float,BoardConfig)`. Final `6c1eb1468f398610cc61ea89b7f3a9409f6baeb451e6df0ae6243b28af50e48e`. Evidence commit `2edfe75f17a42e27d0f9a3e6ee7be15d2ebf8a47`; Drive folder `1945a0ednBRxXQcnzw9RLR4icPXJVTUXY`; final provider readback exactly 22 files.

### HF49 — EnemyPath Arrival Core

Restores exactly `0x060002A3 EnemyPath.ArrivalTest(Zombie)` / RID 675 / PC `0x180329620`. Final `6dff7975abd2b62d2cc40564a17f21518526d8dae2ce7f53a7be3e49c2114f69`. Evidence commit `b3d3747710dcba5aea3f5a73593fcf4cacfa6d80`; Drive folder `1LsjyP8K8z8CeW75Q0Vdp8r0sao39XoR3`; final provider readback exactly 22 files. `Zombie.Path_Test()` itself remains outside HF49.

### HF50 — Path Support Dependency Core

Restores exactly five original native-backed MethodDefs:

- `0x06000141 GlobalStaticVars.GetAnimationSpritePosition(List<GameObject>,string)` / RID 321 / PC `0x18031B680`;
- `0x06000201 ProjectManager.CreateProject(int,int,Vector3)` / RID 513 / PC `0x180323050`;
- `0x06000299 Grid.FindDevice_Occupy(OccupyState)` / RID 665 / PC `0x18032A0A0`;
- `0x060002D0 Board.TestWinTargetZombie()` / RID 720 / PC `0x180328000`;
- `0x06000484 Zombie.TranToStant(float)` / RID 1156 / PC `0x18036A5E0`.

Formal validation:

- formal HF49 input SHA-verified `6dff7975abd2b62d2cc40564a17f21518526d8dae2ce7f53a7be3e49c2114f69`;
- patcher build head `865c7539a42dc9a5c7f1016430e16f094ace4983`, workflow `34552198504` PASS, artifact ID `10181225959`, SHA `9f5cd1779bc44fa3239d556384db38dfa8e56cc851ba241e0a8031d65b100a20`;
- independent double patch -> byte-identical `74ae15e2ed7f1626ecea7741d83803208ea0e8381306f1900f7a46c198c425cb`, both exit 0 / stderr 0;
- Cecil reopen: `0x06000141` 14 IL / 35 bytes / 0 EH; `0x06000201` 54 / 157 / 0; `0x06000299` 27 / 77 / 0; `0x060002D0` 35 / 94 / 1; `0x06000484` 93 / 264 / 0;
- fixed ILSpy 11.0.0.9375 + 56 refs: all five targets clean, stderr 0, target-local Cpp2IL/Unknown/NotImplemented/invalid type-comparison markers 0;
- HF49 whole IL reproduced `c60eb750651402273ec5cc7ac98520c500dd361568cd2214c42518ed7a737455`; HF50 whole IL `0e9929692d9627b8a8e0f1261293ee2bb9cc69ed346165e96458e9ff544c6df3`;
- HF49->HF50 semantic diff `229de8a6ab43aa502d97f267b66b1bd6748e2ed35cf34732985bea59fddaf0b8`;
- MethodDef `2317 -> 2317`, emitted bodies `2297 -> 2297`, distinct nonzero body RVAs `2140 -> 2140`, normalized non-method/method-signature skeleton identical, changed exactly the five authorized targets;
- candidate-specific MethodDef table `b5aa2d0216863d128d286aea4b3ee537ebcadc496e1c0ac1178d676347743dce`;
- cumulative RecoveryAudit build head `27a73fd974f66b18fc3557fa0df27aa0484ecb15`, workflow `34552873703` PASS, artifact ID `10181455848`, SHA `271023744274706414e37c069c395c5c959ee66aea18ba07781c1ff9f41e43e6`; historical auditor remains unchanged for HF1-HF46 and HF50 cumulative auditor locks the HF50 whole-file SHA while rechecking HF47-HF50 targets; two independent actual executions ended `RECOVERY_AUDIT_OK` and `HF50_AUDIT_OK`, stderr 0;
- MethodDef inspector workflow `34553137764` PASS, artifact ID `10181537760`, digest `dd08cf4338076b71f2691ce59fe418f297b04a8d833f0fbb7c94762592c2383a`;
- GitHub Evidence commit `5385bc2b9d4afdc17772e71856146b0c3dd3926f`;
- Drive folder `1Fmvna-e9i8bAKNuGJ88mI_j2Q5SvX4Cf`; cumulative DLL `1VywZ8Yat0NdcDt37PeC1GYQFLZcnW204`;
- ordinary provider readback 19/19 SHA PASS; payload manifest `d08990bc30ac90e03206956936556bff01d0780c54faefd76d16487077803cb0`;
- final Drive folder exactly 22 files; FINAL Evidence provider-readback SHA `070fe0002008192887f70ab51135a656ca85b05fd4810203513946614ddfa5c7`; FINAL SHA manifest provider-readback SHA `ec86d57c1fafc8f4c0bbca5e384ffe1a98762a058447a5d87b552e11112d9542`.

HF50 deliberately does not claim to repair `Zombie.Path_Test`, `Board.GameFail`, `Zombie.DestroyZombie`, `Zombie.DropLootPiece`, `ProjectManager.DropLootPiece`, `Project.SetEndPosition<T>`, or `Project.SunSet`. `Project.SetEndPosition<T>` requires original MethodSpec/generic-method-function attribution before any managed repair.

## HF51 / Unity gate

Use only the formal HF50 DLL above as managed input. Open HF51 only for a residual method or minimal dependency cluster that satisfies all three gates: concrete managed reconstruction damage, real gameplay reachability, and original-PC-native behavior/dependency closure. `Zombie.Path_Test()` and the drop/destroy chain remain primary candidates but are not pre-authorized.

If no further minimal native-backed managed candidate closes cleanly, stop opening HF stages and resume the deterministic Unity path: integrate the formal cumulative DLL into Stage9.1, finish the 67/67 package-script reference closure, then validate exact Unity `2022.3.44f1c1` import/compile before MainMenu/Board runtime testing. Do not invent gameplay behavior merely to advance the HF number.
