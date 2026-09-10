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
- HF37 `412e12b39d1107c29a20027a37094e796d0059e0a3b8637d78233599148d0c56`
- HF38 `25ca8b5afe45930dd39517c2477a96db520d067c067493c334f107201a973b5d`
- HF39 `7c6360c79852f94a86e560367f25c79298d62d3e1bf86027f86112d9fc333f6e`
- HF40 `726c7ceda476cb0034b9394964407a72d2ab7a1a92424dee9c81e118f5590cce`
- HF41 `2e03010cab5a4776243ecf3509402f387c39ccb9faa039876c85ad629ba7fcfa`
- HF42 `687a973f640fc1be1e092fc75f6d09e697995563c605763d1195751af91dc7d6`
- HF43 `c5d998c691f5e32edb5bf48f7d5da3d9ed03f362cb049c3fa3ed8a11617ee078`
- HF44 `fe5e73abead459121ea00c0f91be7e7abc8b3392eb924458b9c0c0352b444118`
- **HF45 FlagMeter Runtime Core — native-backed formal final — `a34feef56319f047a9f60caef0028db9592536b89fa484c41e371a826d6ccd5a`.**

## Recent retained formal results

### HF34 — Projectile Runtime Support Core

Restores `CreateAudioAtPoint/4`, `Projectile.Update_Time`, `Projectile.Update_MoveTrack7`, and `Zombie.GetPredictedPosition`. Final `ead8dee5def2818ca8c64c36b4fe65f1830dbf873fae34dbc84d2f07b103d999`. Evidence commit `edc56dd90e92131e2dba544bd0761f977ac7e712`.

### HF35 — Device Injury Status Core

Restores `0x0600032B Device.InjuryStatusUpdate()` / PC `0x180349280`. Final `ffd3e858a6ea15c1203a8c543e800b9181e0ab115fa7ab6a84d00975be5b947b`. Evidence commit `d08323ea70f0600bb23543108e1b4cea2d5e8327`.

### HF36 — Armored Flag Wake-Up Core

Restores `0x0600048C Zombie.ZC_ArmoredFlagWakeUpZombies()` / PC `0x18036D560`. Final `291e22bffefdf98f0fca29475c485264ad5b9eb6cf8a934226d1e73a187ddfb0`.

### HF37 — Projectile Initial Core

Restores `0x060003ED/3EE/3EF Projectile.Initial<T>` /6, /7, /10 with original MethodSpec/generic attribution and native defaults. Final `412e12b39d1107c29a20027a37094e796d0059e0a3b8637d78233599148d0c56`. Evidence commit `b4c50aece1eb0e5f4f27096eb510ffc5b88ae394`; Drive folder `1L7mnzN8udi4HX-NiusZsOOgX4tp9S_9u`; final provider readback exactly 22 files.

### HF38 — Plant Kill Event Core

Restores `0x0600037C Plant.KillEvent(Zombie)` and `0x0600038F Plant.PC_SSI_KillEvent(Zombie)`. Final `25ca8b5afe45930dd39517c2477a96db520d067c067493c334f107201a973b5d`. Evidence commit `9942516d8bb151b362619e29e20b03ea900d5a75`; Drive folder `1PbrPjBPT-EbBT9p78aroQHrgLxItcDni`; final provider readback exactly 22 files.

### HF39 — Element Runtime Core

Restores fourteen Element/Buff/ER/SnowPea runtime MethodDefs, including true `List<Buff>.Add`, native sign-crossing/shuttle/decay behavior, signed negative UI progress, banker-style ER rounding/clamp, and SnowPea ID5 element transitions. Final `7c6360c79852f94a86e560367f25c79298d62d3e1bf86027f86112d9fc333f6e`. Evidence commit `3725046b046c19299a8993b99d0a7ec73d7928ee`; Drive folder `1Xf5KA-ZsvUUKbnL8PveKx5ia2AOPpOZT`; final provider readback exactly 22 files.

### HF40 — Element UI Start Core

Restores exactly `0x0600012B ElementUIController.Start()` / RID 299 / PC `0x180315850`.

Accepted native behavior: compare `progress` against +0.0 with native `UCOMISS`; NaN/unordered and any nonzero value return, while ordered +0.0/-0.0 continues. Then independently deactivate non-null `back1`, `image1`, `back2`, `image2` GameObjects via `SetActive(false)`. HF40 emits `BNE.UN` so native unordered semantics are preserved.

Formal validation:

- formal HF39 input SHA-verified `7c6360c79852f94a86e560367f25c79298d62d3e1bf86027f86112d9fc333f6e`;
- patcher build head `a5b337cc391264ff44a5655e9241b23e3cb9afec`, workflow `34433570862` PASS, artifact ID `10135362791`, SHA `55929a360a706af6de10972389dbaad1fe6078ea4f5a60841603a5cb7bc8dbc6`;
- independent double patch -> byte-identical `726c7ceda476cb0034b9394964407a72d2ab7a1a92424dee9c81e118f5590cce`, stderr 0;
- Cecil reopen `41 IL / 133 bytes / 0 EH`; fixed ILSpy 11.0.0.9375 + 56 refs exit 0, stderr 0, Cpp2IL refs 0, issue markers 0;
- HF39 whole IL reproduced `23e2734766d033c4b22767fd9b969d8312261cedc24622a38bf6ff5dc5dcc54d`; HF40 whole IL `fd43ae98ca25cc7a43f5265c76a924eaefb8fa8b4616575c6af914935fd0ff7e`;
- prior HF38->HF39 semantic diff reproduced byte-for-byte at `c7c2b30f98986999cf5ed1de9ef800f044e796a2f4f1002c769cb7c0b57868e0`;
- MethodDef `2317 -> 2317`, method blocks `2139 -> 2139`, normalized non-method skeleton identical, changed exactly `0x0600012B`;
- HF39->HF40 semantic diff `053387ddf76a053cc57cbca509bb72c653c5becea3d92e7c595be6a58b670d76`;
- permanent RecoveryAudit commit `f418fa6214ff6cce680f00178ad54f5614e816bb`, workflow `34433765345` PASS, artifact ID `10135428591`, SHA `add5be4fafd3e2284ce0d76222b52a0b2a666418475ab59ccd8db5fdb914ebfb`; independent OPEN1/OPEN2 passed all retained targets and ended `RECOVERY_AUDIT_OK`;
- Evidence commit `6685eec19815bfc571f9eeab97319ba3e45be6b6`;
- Drive folder `1k_tnoO9Rx_BCVpkzdr7-tp06HDziPTXf`;
- cumulative DLL `1bX-nH1QtTSzz-Qs15FnSzFngZSuu3VfW`;
- payload manifest `1uMrfEq6xgH07bUYc8RWC0V7Y52_lFvz6`, SHA `0f68c8c8c632124058ca6a00b1cd082e10ad5a1bc10a584a529bbc9f42ad5cb6`;
- Evidence-FINAL `1hc2NvG7hH7zTVEqkaERgY0IzwiXXnTRc`, SHA `c1068ca7b689073b9fdd7b6932657f3c83a629851f9992de766989b012a8e6eb`;
- SHA256SUMS-FINAL `1xDuvQelRx61IUWSsYAZyEfWbWRMmsTqY`, SHA `6d202a09daf4405eec7f8b5b8983792524a09cfb790f60ee451a86da595b2f18`;
- final provider readback exactly 22 files = 20 payloads + 2 closure files.

**HF40 formal acceptance: PASS.**

### HF41 — Zombie Fixed Visual Core

Restores exactly four native-backed MethodDefs:

- `0x06000128 ElementManager.GetElementColor()` / RID 296 / PC `0x180314F20`;
- `0x06000425 Zombie.Update_Color()` / RID 1061 / PC `0x18036BEB0`;
- `0x0600042D Zombie.FixedUpdate()` / RID 1069 / PC `0x180360080`;
- `0x0600042E Zombie.FixedUpdate_BGM()` / RID 1070 / PC `0x18035FD10`.

Accepted behavior preserves original Unity Object versus plain-reference semantics, native unordered/NaN floating branches, `Ceiling`/clamp color scaling, ashes and renderer/material alpha behavior, `List<GameObject>.Enumerator` finally/Dispose, ID17 BGM scaling, ID18 Board BGM cross-fade, original array/null behavior, and the original `FixedUpdate` call order before dying-health drain.

Formal validation:

- formal HF40 input re-fetched from Drive and SHA-verified `726c7ceda476cb0034b9394964407a72d2ab7a1a92424dee9c81e118f5590cce`;
- patcher build head `0984f986c6d96570c95d6f488753de8e3d3fbc02`, workflow `34435893128` PASS, artifact ID `10136146571`, SHA `e53af852ecd3242df3acc1ec8f15abf52f3ba81fa1f6f9de293a28a313bc36cf`;
- independent double patch -> byte-identical `2e03010cab5a4776243ecf3509402f387c39ccb9faa039876c85ad629ba7fcfa`, both exit 0, stderr 0;
- Cecil reopen: `0x06000128` 117 IL / 340 bytes / 0 EH; `0x06000425` 90 / 264 / 1 EH; `0x0600042D` 30 / 82 / 0 EH; `0x0600042E` 122 / 350 / 0 EH; no Cpp2IL helper or issue marker remains;
- fixed ILSpy 11.0.0.9375 + 56 refs: all four targets exit 0, stderr 0, Cpp2IL/NotImplemented/invalid stack-type-comparison/warning-error markers 0;
- HF40 whole IL reproduced `fd43ae98ca25cc7a43f5265c76a924eaefb8fa8b4616575c6af914935fd0ff7e`; HF41 whole IL `48183cd6963c3bde3056e73d8983eeceea36574199b881077f76091bd458b4c0`;
- prior HF39->HF40 semantic diff reproduced byte-for-byte at `053387ddf76a053cc57cbca509bb72c653c5becea3d92e7c595be6a58b670d76`; HF40->HF41 semantic diff `8080c9fa957c43f70073275f44f9e81d5153670c563ca8bcbdf6e8629480873b`;
- MethodDef `2317 -> 2317`, method blocks `2139 -> 2139`, normalized non-method skeleton identical, changed exactly `0x06000128`, `0x06000425`, `0x0600042D`, `0x0600042E`;
- permanent RecoveryAudit commit `31a9f759fd0c08ef5cb41de4db57680a9849b876`, workflow `34446634528` PASS, artifact ID `10139898714`, SHA `149c9fac6d0a40c308f7142509756b2e8b6d85a2acd18dfcc4364ec8de9ddeae`; two independent auditor processes each passed internal OPEN1/OPEN2 for all retained targets and ended `RECOVERY_AUDIT_OK`;
- GitHub Evidence commit `12fa5db97745dd8d2069eb8a488a4a57e69e0de5`;
- Drive folder `1abfwNIw2Oq-gSIHTtVOnmCtJSxKy5D1D`;
- cumulative DLL `1iEUvEYiv2qUFFhFhpGSQXN4XycNnnmJ4`;
- payload manifest `1SjcGI1RDIlFI2mYORnXMukvU_eQGqWia`, SHA `c9fe6f84acde971fc59847ed664c9fe9fd77781b5e61f2d57ec0d9654bc8868e`; pre-closure provider readback exactly 20 files;
- Evidence-FINAL `1Qt-WPv8cdM4IBExDqwTF6s6EZszgn8pW`, SHA `7d4a2376ee5c66001884d1668e28ee36658e957efd435b857b157090baf1d043`;
- SHA256SUMS-FINAL `1ESErGFP2fVH9UKVX95IjWYE8ycmq3jkO`, 21 entries, SHA `1b01690089bb50937eecf3944dec1216a397411e55e46898314bc204ac495375`;
- final Google Drive provider readback exactly 22 files; FINAL pair was downloaded back from Drive and byte-hashed to the same recorded SHA values.

**HF41 formal acceptance: PASS.**

### HF42 — Dithering Motion Core

Restores exactly three original native-backed MethodDefs:

- `0x060002AF Board.FixedUpdate()` / RID 687 / PC `0x1803265C0`;
- `0x060002B0 Board.FixedUpdate_Shake()` / RID 688 / PC `0x1803262A0`;
- `0x0600035E Plant.Dithering_Animation(float)` / RID 862 / PC `0x18034E890`.

Accepted behavior restores the original Board/Plant dithering path: ordered `shakeTime > 0f` gate with NaN false, previous-frame offset removal, native unordered-preserving shake continuation, `Random.insideUnitCircle` normalization with the original `1e-5f` guard, new-offset addition, original Board amplitude decay, and Plant's `amplitude != 0f` unordered behavior. Original Unity/null/exception behavior is retained; no defensive iOS-oriented guard was added.

Formal validation:

- formal HF41 input was re-fetched from Drive after HF42 patcher publication and SHA-verified `2e03010cab5a4776243ecf3509402f387c39ccb9faa039876c85ad629ba7fcfa`;
- patcher build head `3fb55af85182896824b3743fddda40f6fe8d5100`, workflow `34476087441` PASS, artifact ID `10151538241`, SHA `c4910c871d83828555e993ff3c8484928e2464f7206e9811222d9b987f80ade1`;
- independent double patch -> byte-identical `687a973f640fc1be1e092fc75f6d09e697995563c605763d1195751af91dc7d6`, both exit 0, stderr 0;
- Cecil reopen: `0x060002AF` 7 IL / 20 bytes / 0 EH; `0x060002B0` 130 / 391 / 0 EH; `0x0600035E` 108 / 295 / 0 EH; no Cpp2IL helper remains;
- fixed ILSpy 11.0.0.9375 + 56 refs: all three targets exit 0, stderr 0, Cpp2IL/NotImplemented/invalid stack-type-comparison/warning-error markers 0;
- HF40 whole IL reproduced `fd43ae98ca25cc7a43f5265c76a924eaefb8fa8b4616575c6af914935fd0ff7e`; HF41 whole IL reproduced `48183cd6963c3bde3056e73d8983eeceea36574199b881077f76091bd458b4c0`; HF42 whole IL `ad4b9a6ce967456c40b04b258b02eb7470ec8de747d8d077d13ec135007587ed`;
- prior Drive-archived HF40->HF41 semantic diff reproduced byte-for-byte at `8080c9fa957c43f70073275f44f9e81d5153670c563ca8bcbdf6e8629480873b`; HF41->HF42 semantic diff `1f321b7604d20bf6aa700310c2ec90c10717be02fb2c5de21ea31a710dfb52b6`;
- MethodDef `2317 -> 2317`, distinct method-body RVAs `2139 -> 2139`, normalized non-method skeleton identical, changed exactly `0x060002AF`, `0x060002B0`, `0x0600035E`;
- permanent RecoveryAudit commit `8fc4e7ddf6d3b5d551caf6d989def0f8fb3da3a2`, workflow `34476864538` PASS, artifact ID `10151865052`, SHA `e91943cfb5f5dc4692e207702fa3f2fa7c6208b68f6cc849023ac7ced2a5c4d8`; two independent auditor processes each passed internal OPEN1/OPEN2 for all retained targets and ended `RECOVERY_AUDIT_OK`;
- GitHub Evidence commit `6afec47a028c806293046f70cd41858d0144a3c3`;
- Drive folder `1cXHCViPgqd6-glDnpiPUREXBD5ds1WT9`;
- cumulative DLL `1Aw_s062EA-uXi-MpZkIWx-7BElPHPpXM`;
- payload manifest `1bEoTJcNvhgoqhvi-KZ-LdTPo_L3Cg5ii`, SHA `496dc3deff88255d817f4ea7792b7cce6f93f6e9267372f5fd307a141ef5e519`; pre-closure provider readback exactly 20 files;
- Evidence-FINAL `1wQThN98KYQYA5NDuHawKZCD-u0UIisbz`, SHA `f4b4ce538a09d2669746e574d1df4707c57ec80656012ee5f26307cb43f3fbff`;
- SHA256SUMS-FINAL `19W3-Fq_rSsT54askm-1Lv-PeuL1xTbEN`, 21 entries, SHA `7ca3c4a43935d7a3deababe9af67be3fd7c8658ba8fb18f3e79ae258eb3371de`;
- final Google Drive provider readback exactly 22 files; FINAL pair was downloaded back from Drive and byte-hashed to the same recorded SHA values.

**HF42 formal acceptance: PASS.**

### HF43 — Animation Rate Core

Restores exactly three original native-backed MethodDefs:

- `0x0600039E Plant.ResetUpdateRate(float)` / RID 926 / PC `0x180356970`;
- `0x060003A7 Plant.SetUpdateRate()` / RID 935 / PC `0x180359430`;
- `0x06000451 Zombie.GetRandenAnimationSpeedMagnification()` / RID 1105 / PC `0x180361870`.

Accepted behavior preserves Animator Unity Object truthiness; ordered-zero versus unordered/nonzero update-rate paths; the original `0f / updateRate` floating behavior in burst; elemental Ceiling/Max slowdown with ID5/ID6 exceptions; and the original two-level Zombie ID jump table in which exactly IDs `{0,2,4,5,6,7,8,9,11,12,14}` use `Random.Range(0.75f,1.3f)` and all others return 1f.

Formal validation:

- formal HF42 input was re-fetched from Drive after the accepted v2 patcher publication and SHA-verified `687a973f640fc1be1e092fc75f6d09e697995563c605763d1195751af91dc7d6`;
- HF43 v1 patcher head `eaafbbefcecf0c5a6489796ace5408c54b827b83`, workflow `34479347287` published successfully but its first formal application was rejected by its own reopen floor (`GetRanden...` produced 12 IL while the conservative floor was 15); no v1 candidate was accepted or propagated;
- v2 changed only that reopen floor to 10, leaving gameplay Template semantics unchanged; v2 patcher head `25ada471bf94f96dc5abc7affdcd6e616e02767d`, workflow `34479568604` PASS, artifact ID `10152984471`, SHA `b7193f7f6dc11f86ae433c3db0ab2a04670e0893b70c59f48fbe6f0ad0034b03`;
- independent v2 double patch -> byte-identical `c5d998c691f5e32edb5bf48f7d5da3d9ed03f362cb049c3fa3ed8a11617ee078`, both exit 0, stderr 0;
- Cecil reopen: `0x0600039E` 70 IL / 216 bytes / 0 EH; `0x060003A7` 94 / 276 / 0 EH; `0x06000451` 12 / 97 / 0 EH; no Cpp2IL helper remains;
- fixed ILSpy 11.0.0.9375 + 56 refs: all three targets exit 0, stderr 0, Cpp2IL/NotImplemented/invalid stack-type-comparison/warning-error markers 0;
- HF41 whole IL reproduced `48183cd6963c3bde3056e73d8983eeceea36574199b881077f76091bd458b4c0`; HF42 whole IL reproduced `ad4b9a6ce967456c40b04b258b02eb7470ec8de747d8d077d13ec135007587ed`; HF43 whole IL `26da408a5fe00508522d3a784338c4c50577ab1a78226d3c1929886e42760743`;
- prior Drive-archived HF41->HF42 semantic diff reproduced byte-for-byte at `1f321b7604d20bf6aa700310c2ec90c10717be02fb2c5de21ea31a710dfb52b6`; HF42->HF43 semantic diff `4538f058ada84cf46c899cf9c85bc07db7c1a49780720e6d4298ea2f9d3e0f91`;
- MethodDef `2317 -> 2317`, distinct method-body RVAs `2139 -> 2139`, normalized non-method skeleton identical, changed exactly `0x0600039E`, `0x060003A7`, `0x06000451`;
- HF42 MethodDef table reproduced at the accepted canonical path `34f45e7c4beb53630472ae0a5cba63db6622029e131f193cbc1dacaec05d3e9b`; HF43 MethodDef table `b856427fdef2d77350ab805f1aaa0fab35d77bfb8a7bd1d0fb22e02829869149`;
- permanent RecoveryAudit commit `fde3f301afd31ad0cd5588ea7a8dee36758f4ebb`, workflow `34480261357` PASS, artifact ID `10153273106`, SHA `b200e4a830eb274d333415d396207398ad2642dffc77446e853ec71fcf3ae116`; two independent auditor processes each passed internal OPEN1/OPEN2 for all retained targets and ended `RECOVERY_AUDIT_OK`;
- GitHub Evidence commit `319ae0ea005a86e8bfadbb2b7c8a64f983ca4fe0`;
- Drive folder `1VLh1-JRb1-Y-iI0v3EW8wzvqtwvvQ36s`;
- cumulative DLL `15pAzAORNoxbQamjmj4xotoQ82RjaeirI`;
- payload manifest `1xUR8HBaQfFaTGLNwuWRlvf1TInzTsMoH`, SHA `18e27ee935590af8d150af0506fcdbf44b9ede88182cb8b22bdbc412f17fff68`; pre-closure provider readback exactly 20 files;
- Evidence-FINAL `1-tFd6uNFrTGUIM9MTKMeanFrTECLYMut`, SHA `a77fa3a2cbdaa3bd2a5730a10d473afb731577e2e2fbd2a8d3dc7af479de7715`;
- SHA256SUMS-FINAL `1yPYiGKVDikkTJ7jbGe3dZjDsBraICekP`, 21 entries, SHA `1e3954be58bbd85027dab9a665747c2fb3ebe070c27266d26d24ed88f3a983d8`;
- final Google Drive provider readback exactly 22 files; FINAL pair was downloaded back from Drive and byte-hashed to the same recorded SHA values.

**HF43 formal acceptance: PASS.**

### HF44 — Zombie Movement Position Core

Restores exactly three original native-backed MethodDefs:

- `0x06000472 Zombie.ResetMoveSpeed()` / RID 1138 / PC `0x180366D90`;
- `0x06000474 Zombie.ResetUpdateRate(float)` / RID 1140 / PC `0x180366E40`;
- `0x06000481 Zombie.TestPosition(float,float)` / RID 1153 / PC `0x180369FC0`.

Accepted behavior preserves the ID17 ordered-negative move-direction rule with NaN/unordered fall-through, Animator Unity Object truthiness, the ordered-zero initialization path and nonzero/unordered three-channel rescaling in `ResetUpdateRate`, ordinary managed `Grid` null semantics versus Unity `Device` truthiness, passability acceptance, negative-X Ladder interaction, ID13 Snowbeast impact/wall calls, blocked-grid midpoint ±3 correction, original candidate X/Y Transform writeback with Z preserved, and null-Grid coordinate acceptance. No defensive iOS null guards or guessed movement corrections were added.

Formal validation:

- formal HF43 input was re-fetched from Drive after HF44 patcher publication and SHA-verified `c5d998c691f5e32edb5bf48f7d5da3d9ed03f362cb049c3fa3ed8a11617ee078`;
- patcher build head `33ef6bc3e9d5b1dcd92f4fee970336f4ce503cdc`, workflow `34484120971` PASS, artifact ID `10154861901`, SHA `f4c3fa9f4f4e992e4721ebc4480e86dd67382bf68fdb03b6636476b7c73d17eb`;
- independent double patch -> byte-identical `fe5e73abead459121ea00c0f91be7e7abc8b3392eb924458b9c0c0352b444118`, both exit 0, stderr 0;
- Cecil reopen: `0x06000472` 22 IL / 61 bytes / 0 EH; `0x06000474` 67 / 196 / 0 EH; `0x06000481` 161 / 408 / 0 EH; no Cpp2IL helper remains;
- fixed ILSpy 11.0.0.9375 + 56 refs: all three targets exit 0, stderr 0, Cpp2IL/NotImplemented/invalid stack-type-comparison/warning-error markers 0;
- HF42 whole IL reproduced `ad4b9a6ce967456c40b04b258b02eb7470ec8de747d8d077d13ec135007587ed`; HF43 whole IL reproduced `26da408a5fe00508522d3a784338c4c50577ab1a78226d3c1929886e42760743`; HF44 whole IL `dbb10be399d1672a0927510a2fb05481783aabdfd56c9eb921d2bb9982ce2b1f`;
- prior Drive-archived HF42->HF43 semantic diff reproduced byte-for-byte at `4538f058ada84cf46c899cf9c85bc07db7c1a49780720e6d4298ea2f9d3e0f91`; HF43->HF44 semantic diff `c79688ce7518eb8fd5fe0cc697d2569ff39b4a189c004ceacd9460a09c430294`;
- MethodDef `2317 -> 2317`, distinct method-body RVAs `2139 -> 2139`, normalized non-method skeleton identical, changed exactly `0x06000472`, `0x06000474`, `0x06000481`;
- HF43 canonical MethodDef table reproduced byte-for-byte at `b856427fdef2d77350ab805f1aaa0fab35d77bfb8a7bd1d0fb22e02829869149`; HF44 MethodDef table `ab1557de0172766a51cc30dd7edae12d5a0cd29551cfb659a0e3641571afc790`;
- permanent RecoveryAudit commit `fec4f209eadb00d4620983c93b88e0d107baba72`, workflow `34484770234` PASS, artifact ID `10155135509`, SHA `075dfd1a1a84f779e074c0da64cd58bcd301f8a7da15d0a1eb79db7fd33e94b8`; two independent auditor processes each passed internal OPEN1/OPEN2 for all retained targets and ended `RECOVERY_AUDIT_OK`;
- GitHub Evidence commit `d39cdd6a0e2476eb97ca0f3d79821f876fb4fa05`;
- Drive folder `1AsQpnZd9MJlcPdihbJqN0izasvoLY_br`;
- cumulative DLL `1gGvEopIJHDzdVilPHSVf1ka_Rm-Pfx9a`;
- payload manifest `1w3QjD2USbRhIHNDOBIwsPeGncZVcHdQl`, SHA `5e511fe1825912fc571d0fb8956b860f45ef3370a49e4a737f28fdc44badef02`; pre-closure provider readback exactly 20 files;
- Evidence-FINAL `1NzJeS6yu6-CuvygC6W4DQw7lmJxBKVrg`, SHA `9b6c350b9548fb33f4e4e1ddac472f7e4d9d209cf17f2fdac31be8db176992d6`;
- SHA256SUMS-FINAL `14iwpKm3BI9YLUXwZbWfJAfKVXSC-lJbb`, 21 entries, SHA `94ed2598167b43c43f68d67803865c07efaf31707b789b64e9ac9e8c4d5723ab`;
- final Google Drive provider readback exactly 22 files; FINAL pair was downloaded back from Drive and byte-hashed to the same recorded SHA values.

**HF44 formal acceptance: PASS. HF44 is now the only allowed formal input for any later cumulative HF stage.**

### HF45 — FlagMeter Runtime Core

Restores exactly two original native-backed MethodDefs:

- `0x06000617 FlagMeter.Update()` / RID 1559 / PC `0x18039D010`;
- `0x06000618 FlagMeter.UpdateMeter(int,int)` / RID 1560 / PC `0x18039CF30`.

Accepted behavior restores the original wave-meter runtime: single-precision `(theWave + 1) / wavesNum` progress, signed `% 10` flag activation, current/total text refresh, head-meter movement using `218f - progress * 436f`, and active-flag child vertical movement. Native COMISS unordered/NaN fall-through, List/index/null/Transform exception behavior, and all original constants are retained; no defensive iOS guards were added.

Formal validation:

- formal HF44 input was re-fetched from Drive after HF45 patcher publication and SHA-verified `fe5e73abead459121ea00c0f91be7e7abc8b3392eb924458b9c0c0352b444118`;
- patcher build head `289191fdf2fb9f30f2599833bd4b9b8de50fd6a0`, workflow `34489420443` PASS, artifact ID `10157078781`, SHA `21fbdb6a5814dd328941ac0ae36ec45de981b3617e3edcb0c678568d6bee0b67`;
- independent double patch -> byte-identical `a34feef56319f047a9f60caef0028db9592536b89fa484c41e371a826d6ccd5a`, both exit 0, stderr 0;
- Cecil reopen: `0x06000617` 68 IL / 184 bytes / 0 EH; `0x06000618` 31 / 74 / 0 EH; no Cpp2IL helper remains;
- fixed ILSpy 11.0.0.9375 + 56 refs: both target blocks exit 0, stderr 0, Cpp2IL/NotImplemented/invalid stack-type-comparison/warning-error markers 0;
- HF43 whole IL reproduced `26da408a5fe00508522d3a784338c4c50577ab1a78226d3c1929886e42760743`; HF44 whole IL reproduced `dbb10be399d1672a0927510a2fb05481783aabdfd56c9eb921d2bb9982ce2b1f`; HF45 whole IL `4d4ccd9b2c857ef9aceb0e18896e77cb6b493d9e654dc0a27dce2f5bfbd46668`;
- prior Drive HF43->HF44 semantic diff reproduced byte-for-byte at `c79688ce7518eb8fd5fe0cc697d2569ff39b4a189c004ceacd9460a09c430294`; HF44->HF45 semantic diff `a1508fbe29703cec50c81b8db85857fcdb2e1fda4abbdf24a1059b0fcda225e5`;
- MethodDef `2317 -> 2317`, distinct method-body RVAs `2139 -> 2139`, normalized non-method skeleton identical, changed exactly `0x06000617`, `0x06000618`;
- HF44 canonical MethodDef table reproduced byte-for-byte at `ab1557de0172766a51cc30dd7edae12d5a0cd29551cfb659a0e3641571afc790`; HF45 MethodDef table `784c0d85a4dd74015670c865493c2e08e324fc54c83541caace717e06300b50d`;
- permanent RecoveryAudit commit `bf54c77b625d818449d3a80a724a00ae2c0b95f6`, workflow `34489954734` PASS, artifact ID `10157301323`, SHA `c3d5aa29b9b2ef40803924de08fac48a1e66bf7062d8782e3ef990db8a9f06fc`; two independent auditor processes each passed internal OPEN1/OPEN2 over all retained targets and ended `RECOVERY_AUDIT_OK`;
- GitHub Evidence commit `5fd1e2bd56d68c69e2e63c6484f1ffeea9b07351`;
- Drive folder `1j1M9AyrsNcnHXtMg2sdQiBTuODSmLbUi`;
- cumulative DLL `19HEqSmIkz4PUt-SIhXWdmEwBy6UMu8so`;
- payload manifest `1Q-frWbl9L_rHsGTCR7yUOMeXOvjzdHwI`, SHA `26b9ffbc923460adc3c18d86fa3123f742407627570d92c0658e96ac88f1154f`; pre-closure provider readback exactly 20 files;
- Evidence-FINAL `1qYAlZSvqvAjrQFvfZPqS7_t7gxLqjx4-`, SHA `5caff93348f409767016a3cb9c80f4d307f85bff02c649e739c20ff3f7e887ff`;
- SHA256SUMS-FINAL `1thN5bR035bBNDFY5jAHrl2AwnnsM-eE2`, 21 entries, SHA `c64bd4b37fd7af6ae0bb5cf656a8f927243021e82304a8bf60f9962b1bcc9d3a`;
- final Google Drive provider readback exactly 22 files; FINAL pair was downloaded back from Drive and byte-hashed to the same recorded SHA values.

**HF45 formal acceptance: PASS. HF45 is now the only allowed formal input for any later cumulative HF stage.**

## Unity reconstruction state

AssetRipper ~3,149 objects; reconstructed project ~6,783 files. 173 game script types and 263 refs across 114 assets migrated. MainMenu/Board scenes restored. Package scripts still require 67/67 validation before final Unity integration.

## Decision gate — do not auto-open HF46

Run a fresh residual active-path scan against the HF45 cumulative assembly. A later HF stage may open only if an original-PC-native-backed MethodDef simultaneously has concrete managed loss/mis-reconstruction, material gameplay reachability, and behaviorally closed native dependencies. Warning count, MethodDef adjacency, shared-stub xref centrality, or cosmetic decompiler quality alone are not gates.

`EnemyManager.TimeUpdate()` remains a candidate because it is directly reached from `EnemyManager.Update()`, but the newly repaired FlagMeter dependency does not by itself authorize patching it. Its remaining `TextWaveHealth` and broader huge-wave/final-particle Board/state dependencies must be closed from original native evidence first. `Zombie.SetUpdateRate()` and residual Projectile helper bodies remain unpromoted without independent live-call evidence.

If no remaining candidate satisfies the gates, stop HF managed recovery and proceed to 67/67 Unity/package validation, integrate the HF45 cumulative Assembly-CSharp recovery, compile and validate MainMenu/Board/gameplay paths, and only then perform necessary iOS adaptation.
