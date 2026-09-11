# Recovery status

## Current strategy

Use PC x86-64 IL2CPP as the primary gameplay source, Android native second, then original metadata/assets/JSON. Cpp2IL/ILSpy are attribution and managed-reconstruction aids, not authoritative source.

A stage is not final until original native attribution, formal-input SHA validation, deterministic independent patching, Cecil reopen, permanent/cumulative RecoveryAudit OPEN1/OPEN2, fixed ILSpy readback, whole-assembly semantic isolation, GitHub Evidence, Google Drive archive, provider readback and STATUS closure all pass.

Historical per-stage detail remains preserved in Git history and `Tools/HighFidelityPatch/Evidence/`. This STATUS records the authoritative cumulative chain and current next-stage gate.

## Fixed original baseline

- Unity `2022.3.44f1c1`; IL2CPP metadata layout `31.1`.
- PC CodeRegistration / MetadataRegistration `0x1815E88C0` / `0x1818C6D00`.
- PC ZIP SHA-256 `2f08b4e2243e2bd296f3db694b4a2c9656252d76202c80167bdf3340b9f65b48`.
- PC GameAssembly SHA-256 `9ebd7ca996a5b03fb4a766f7a2502b660d581ddbbb36af4d7bf2f06a211da39d`.
- PC metadata SHA-256 `ad992341add498bd1018ac980f42171e0737b0793bf945c8714ad70a445b36b9`.
- Android APK SHA-256 `428e0ba2e46645a905fb9fbb00cfde42406727a3ddfce9a7df1e0875889a739f`.
- Android arm64 `libil2cpp.so` SHA-256 `cfa13d53d7c3e218221a90c5fb615a012339393774af3160ff1c17f3826c61a6`.
- Android armeabi-v7a `libil2cpp.so` SHA-256 `3d035afbf3e419a0d48947ec460eb910731727b3e0da5a2ee83e20650668fadb`.
- Android metadata SHA-256 `e7a4412e3af25da3ba2c806d3c691ec68d00c1e7915d8fb6843c3a82422f5005`.
- PC `globalgamemanagers` and Android UnityFS both identify Unity `2022.3.44f1c1`.
- Cpp2IL source commit `5fb20304df698ffd3d0e664b2a698cd911dc9d57`.
- Native attribution must use original IL2CPP metadata / CodeGenModule method-pointer identity. A Cpp2IL-generated reference PE MethodDef RID/token is **not** automatically the original native methodPointers index: generated-reference rows can be omitted or inserted, so name/signature, neighboring identity and native behavior must be reconciled before assigning a pointer. Formal restored-DLL tokens are a third identifier namespace. Generic MethodDefs additionally require original MethodSpec / generic-method-function attribution.
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
- HF50 `74ae15e2ed7f1626ecea7741d83803208ea0e8381306f1900f7a46c198c425cb` — Path Support Dependency Core.
- HF51 `89e1961cefec6f8c532a0ca7cda4bd4afff2152c14f3856842ee44e5d7d7d90e` — Project Generic EndPosition Core.
- HF52 `1db686c32f24ea81660f3d18ccfd649ee7aeb5a40902e2042639e47355184b82` — Project Sun Scale Core.
- HF53 `924a06f86d648d1bc04bb1d73ccc610dbd59daeb040331910dad86541777d9b5` — GameFail Dependency Core.
- HF54 `307dc20c09a6338ecf4a4917b489cc0303d61c79a6aafeb34011e3e67961af2c` — ProjectManager DropLoot Core.
- **HF55 `dc205a40dc2478b3aacbb3a7d6bb1ca96ffb0a964648d34062b4ddc75f3b3655` — Zombie DropLoot Core — original-PC-native-backed formal final.**

HF55 is the only allowed formal cumulative managed input for HF56 or any later HF stage.

## Latest formal stages

### HF47 — EnemyManager Wave Progression Core

Restores `EnemyManager.DispatcheWave(Wave)`, `EnemyManager.DispatcheZombie(Enemy)`, and `EnemyManager.TimeUpdate()`. Final `e94942c50cdce952ca37f746bc5c3ab83b49106ae0837a077045f54f6ecb9533`. Evidence commit `c9e53de6dcb58bcdbf2a69ca9eb7bcb59869e0ff`; Drive folder `1YKFGYZXZYTWwfqqFp3-v4PrkMX1ux_ii`; final provider readback exactly 22 files.

### HF48 — Ladder Runtime Core

Restores `EnemyManager.DispatcheLadderWave`, `EnemyManager.DispatcheSPHWave`, `ZombieManager.Update`, `ZombieManager.Update_Ladder`, `ZombieManager.TriggerLadder`, and `EnemyPath..ctor(int,int,float,BoardConfig)`. Final `6c1eb1468f398610cc61ea89b7f3a9409f6baeb451e6df0ae6243b28af50e48e`. Evidence commit `2edfe75f17a42e27d0f9a3e6ee7be15d2ebf8a47`; Drive folder `1945a0ednBRxXQcnzw9RLR4icPXJVTUXY`; final provider readback exactly 22 files.

### HF49 — EnemyPath Arrival Core

Restores `EnemyPath.ArrivalTest(Zombie)`. Final `6dff7975abd2b62d2cc40564a17f21518526d8dae2ce7f53a7be3e49c2114f69`. Evidence commit `b3d3747710dcba5aea3f5a73593fcf4cacfa6d80`; Drive folder `1LsjyP8K8z8CeW75Q0Vdp8r0sao39XoR3`; final provider readback exactly 22 files. `Zombie.Path_Test()` remains outside HF49.

### HF50 — Path Support Dependency Core

Restores five native-backed formal MethodDefs: `GlobalStaticVars.GetAnimationSpritePosition`, `ProjectManager.CreateProject`, `Grid.FindDevice_Occupy`, `Board.TestWinTargetZombie`, and `Zombie.TranToStant`. Final `74ae15e2ed7f1626ecea7741d83803208ea0e8381306f1900f7a46c198c425cb`. Evidence commit `5385bc2b9d4afdc17772e71856146b0c3dd3926f`; Drive folder `1Fmvna-e9i8bAKNuGJ88mI_j2Q5SvX4Cf`; final provider readback exactly 22 files.

### HF51 — Project Generic EndPosition Core

Restores formal `0x060003CA Project.SetEndPosition<T>(T)`. Original generic attribution closes `SetEndPosition<Zombie>` MethodSpec index `74551`, hidden MethodInfo/RGCTX global slot `0x181BBB770`, encoded MethodRef `0xC002466F`, and reference-type shared PC implementation `0x1804A1AA0`. Final `89e1961cefec6f8c532a0ca7cda4bd4afff2152c14f3856842ee44e5d7d7d90e`. Evidence commit `56778abb461c2e47e5d15e8b527d8df6c0922fb4`; Drive folder `1CW2-2_bowUTC7d97V38Yp--RrroVNrYD`; final provider readback exactly 22 files.

### HF52 — Project Sun Scale Core

Restores formal `0x060003CE Project.SunSet(int)` from PC native `0x180378750..0x1803787CC`, including the lost double-to-float conversion after `Math.Pow`. Final `1db686c32f24ea81660f3d18ccfd649ee7aeb5a40902e2042639e47355184b82`. Evidence commit `5d3b9f9b96a29c522eadc56aa97837d8ba6be929`; Drive folder `1gHTJvN3hQ2EV6DpER8m7MYQyP5IcZ51s`; final provider readback exactly 22 files.

### HF53 — GameFail Dependency Core

Restores four native-backed formal MethodDefs: `GlobalStaticVars.CreateAudioAtPoint(AudioClip,Vector3,float)`, `ZombieManager.BGMPasue()`, `Board.GameFail()`, and `Window_Q.PopupNewWindow(int,Transform,Board)`. Final `924a06f86d648d1bc04bb1d73ccc610dbd59daeb040331910dad86541777d9b5`. Evidence commit `074172f2f60f92c8b54b7a11c4d8d7a3217c877d`; Drive folder `19xtYYFiYYIsGK0IYYM2yIIEfIDRasXMT`; final provider readback exactly 22 files.

### HF54 — ProjectManager DropLoot Core

Restores exactly one formal managed MethodDef:

- formal HF53/HF54 identifier `0x06000202` / RID 514 / `ProjectManager.DropLootPiece(Zombie,Vector3)`.

Original-file and identifier reconciliation was re-run before formal closure:

- the PC ZIP and Android APK were re-materialized directly from Google Drive and reproduced the fixed package hashes above;
- PC `GameAssembly.dll` and metadata reproduced the fixed original hashes above; PC metadata header is version 31 and `globalgamemanagers` identifies Unity `2022.3.44f1c1`;
- Android arm64/armv7 native binaries and metadata were independently extracted and locked above; Android UnityFS also identifies `2022.3.44f1c1`;
- fixed Cpp2IL was rerun on Android arm64 and completed `2319/2319` methods;
- both the fixed original-PC generated reference and independent Android generated reference identify `ProjectManager.DropLootPiece` at generated-reference token `0x060001FC` / RID 508, while the formal recovery DLL identifies it at `0x06000202` / RID 514;
- sequence alignment shows six formal MethodDefs before `ProjectManager` absent from the generated-reference PE, so Cpp2IL reference tokens are explicitly not used as native pointer indexes;
- verified original PC `Assembly-CSharp` methodPointers slot 513 resolves to `0x180323260`, and direct disassembly of that function independently closes the defining DropLoot behavior: Camera X clamp with `920/540`, `TestWinTargetZombie`, `GameFinished`, award ID 4, random `[0,10000)`, enemyPoint multipliers `30/100/150/800`, project IDs `3/2/1/8`, SunSet `25/50/100`, and `SetEndPosition<Zombie>`.

Formal validation:

- formal HF53 input SHA-verified `924a06f86d648d1bc04bb1d73ccc610dbd59daeb040331910dad86541777d9b5`;
- patcher build head `d9cc2dc18fda2a087f1ec81eb016fc57388dcfc0`, workflow `34572300501` PASS, artifact ID `10188237519`, SHA `357f588db9a6da8b780cad002dce778b934d356ddf8b601fa634f99ef75e5116`;
- two independent patches -> byte-identical `307dc20c09a6338ecf4a4917b489cc0303d61c79a6aafeb34011e3e67961af2c`, both stderr 0;
- Cecil reopen: 159 IL / 341 bytes / 0 EH;
- fixed ILSpy 11.0.0.9375 + locked 56 refs: target stderr 0 and target-local Cpp2IL/Unknown/NotImplemented/Expected/invalid markers 0;
- HF53 whole IL baseline reproduced `9d02f7b55aac9fa89b0ffb0f5bc2e1a4644513dc46632fa7d8acc82091c5ca4a`; HF54 whole IL `2b342d49e118eb70c16137132270ddd37672e268d69e517322797fdf6bd43f42`;
- MethodDef `2317 -> 2317`, emitted bodies `2297 -> 2297`, distinct nonzero body RVAs `2140 -> 2140`, normalized skeleton identical, changed exactly formal `0x06000202 ProjectManager.DropLootPiece(Zombie,Vector3)`;
- cumulative audit build head `73d3ad3b4ec239a21855b74e5eb38eb980607bb4`, workflow `34572736707` PASS, artifact ID `10188399748`, SHA `4bdf83b65696faf21ca861dee0080ce56a6de48a3f4a7445b37c31cf416b2683`;
- two independent actual composite executions ended `RECOVERY_AUDIT_OK` + `HF54_AUDIT_OK`, stderr 0, byte-identical log SHA `09973cba9a90d3e28a4450c6cd7b3ef51fd150a243907b8d24b72f8bca529a68`;
- corrected GitHub Evidence commit `9553754787aae9f35b1087e36bd098c0104b0409`;
- Drive folder `1I6tuQRxbDKcUF507_Tx_iTlhDn3Xsd75`; cumulative DLL `1Eraz5XKozAOjXKXCqmvMSN0YmJKKao8S`;
- ordinary provider readback 19/19 SHA PASS, 0 missing, 0 mismatch; payload manifest SHA `25f6a049346e8946142dff7a7e70f4dd43fc15daf5459d99aa80cb18b706a2e4`;
- final Drive folder exactly 22 files;
- FINAL Evidence provider-readback SHA `416fb2f15e1a8e1c9243637fa2743dc0306f0f9430a667935f2706290de305f4`;
- FINAL SHA manifest provider-readback SHA `ba709d4c6509f58c20fef3355288871e5efb8fb0b5e111a22cfcfe2f71e1802f`.

HF54 deliberately does not claim to repair `Zombie.Path_Test()`, `Zombie.DestroyZombie()`, or `Zombie.DropLootPiece()`.

### HF55 — Zombie DropLoot Core

Restores exactly one formal managed MethodDef:

- formal HF54/HF55 identifier `0x06000438` / RID 1080 / `Zombie.DropLootPiece()`.

Original-file and identifier reconciliation was re-run before formal closure:

- fixed original PC and independently regenerated Android arm64 Cpp2IL references both identify the generated-reference method as `0x0600041E` / RID 1054, while the formal recovery DLL uses `0x06000438` / RID 1080;
- generated-reference tokens are not used directly as original native pointer indexes;
- reconciled original-PC native body is `0x18035EDB0..0x18035F040`;
- PC native reads `shadow.transform.position`, gates on `isOnBoard`, computes the head-position Vector3, loads `board.projectManager`, and calls `0x180323260` (HF54 `ProjectManager.DropLootPiece`) with `r8 = &Vector3`, `rdx = this Zombie`, `rcx = projectManager`;
- Android Cpp2IL independently recovers the same control flow but corrupts the final value-type operand to `(Vector3)0`; PC native ABI proves the real operand is the computed `GetHaedPosition()` Vector3.

Formal validation:

- formal HF54 input SHA-verified `307dc20c09a6338ecf4a4917b489cc0303d61c79a6aafeb34011e3e67961af2c`;
- corrected patcher build head `e9be71217fad3c1d36d4ea8a0ff1de2053cba472`, workflow `34600412954` PASS, artifact ID `10264430775`, SHA `372809225401b203da6244f08148f9242cd206cd7ed6853aa10d275cbc99bb0d`;
- two independent patches -> byte-identical `dc205a40dc2478b3aacbb3a7d6bb1ca96ffb0a964648d34062b4ddc75f3b3655`, both stderr 0;
- Cecil reopen: 19 IL / 52 bytes / 0 EH;
- provisional template using `GameObject shadow` was discarded; corrected formal template uses actual `Transform shadow`, and fixed ILSpy readback has no artificial object/GameObject cast;
- fixed ILSpy 11.0.0.9375 + locked 56 refs: target stderr 0 and target-local Cpp2IL/Unknown/NotImplemented/Expected/invalid markers 0;
- HF54 whole IL baseline reproduced exactly `2b342d49e118eb70c16137132270ddd37672e268d69e517322797fdf6bd43f42`; HF55 whole IL `e0e4dc29327bf63ee0b48fdecd7ccd071dfa1fd9f0f62194512d49cd3d6b0c9c`;
- MethodDef `2317 -> 2317`, normalized skeleton identical at `8c48e2535749c44abf5e696553cd25f1fc352425d1603d9135b67bb942ee10c9`, changed exactly formal `0x06000438 Zombie.DropLootPiece()`;
- normalized semantic diff SHA `05d10d12823183a4f15c4645a0081b9709fbea8f238f53f6e1314ec490b5c62c`; MethodDef table SHA `44bf60f800146ee195d07383c00988f7e41a82ae3ada1a67cfe7c4bcb2d1ecb2`;
- cumulative audit build head `5448fe17bf6a85a30d871a0cbd6c7f9374d5437c`, workflow `34600623192` PASS, artifact ID `10263064154`, SHA `37644faaf7984cc6d80a5e2faae8c86aca1cbbef5796babf7431bc1b12da4a2c`;
- two independent actual composite executions ended `RECOVERY_AUDIT_OK` + `HF55_AUDIT_OK`, stderr 0, byte-identical log SHA `abad1e0674b4358fb9012efa51d01b800b74dfd44489f818afee221a62bec679`;
- GitHub Evidence commit `2f5372514e23829d2c832c4ecf02d8c5de921b9b`;
- Drive folder `1RCKjozAtPyF4XnDy6RSBiombtbmNDtl1`; cumulative DLL `1yjwk3g13yf42qg_5bRq2UBULy-ROeQvn`;
- ordinary provider readback 19/19 SHA PASS, 0 missing, 0 mismatch; payload manifest SHA `e423aa80014638473c557a8d2214fcc0cad7c28c8b06f71342b562d0b51e5f35`;
- final Drive folder exactly 22 files;
- FINAL Evidence provider-readback SHA `cdd96ece4e2543fb6e00fc1fc5da88efb474ced49aaf95f9a8795b3ed229b668`;
- FINAL SHA manifest provider-readback SHA `ba3128441baec591ebc3976dd87964af0a5bb9b05a75b97dc04704b8dfe51c54`.

HF55 deliberately does not claim to repair `Zombie.Path_Test()` or `Zombie.DestroyZombie()`.

## HF56 / Unity gate

Use only the formal HF55 DLL above as managed input. Open HF56 only for a residual method or minimal dependency cluster that satisfies all three gates: concrete managed reconstruction damage, real gameplay reachability, and original-PC-native behavior/dependency closure.

Primary residual candidates are `Zombie.Path_Test()` and `Zombie.DestroyZombie()`, but neither is pre-authorized. Before any native-pointer attribution, reconcile formal restored-DLL identity, original PC/APK method identity and Cpp2IL generated-reference identity by name/signature/neighbor sequence; never infer a native slot directly from a regenerated reference token.

If no further minimal native-backed managed candidate closes cleanly, stop opening HF stages and resume the deterministic Unity path: integrate the formal HF55 cumulative DLL into Stage9.1, finish the 67/67 package-script reference closure, then validate exact Unity `2022.3.44f1c1` import/compile before MainMenu/Board runtime testing. Do not invent gameplay behavior merely to advance the HF number.
