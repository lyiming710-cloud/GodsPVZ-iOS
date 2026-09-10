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
- **HF39 Element Runtime Core — native-backed formal final — `7c6360c79852f94a86e560367f25c79298d62d3e1bf86027f86112d9fc333f6e`.**

## Recent retained formal results

### HF34 — Projectile Runtime Support Core

Restores `CreateAudioAtPoint/4`, `Projectile.Update_Time`, `Projectile.Update_MoveTrack7`, and `Zombie.GetPredictedPosition`. Final `ead8dee5def2818ca8c64c36b4fe65f1830dbf873fae34dbc84d2f07b103d999`. Evidence commit `edc56dd90e92131e2dba544bd0761f977ac7e712`.

### HF35 — Device Injury Status Core

Restores `0x0600032B Device.InjuryStatusUpdate()` / PC `0x180349280`, including roadblock thresholds, sprite/particle/audio behavior and native ordered-float semantics. Final `ffd3e858a6ea15c1203a8c543e800b9181e0ab115fa7ab6a84d00975be5b947b`. Evidence commit `d08323ea70f0600bb23543108e1b4cea2d5e8327`.

### HF36 — Armored Flag Wake-Up Core

Restores `0x0600048C Zombie.ZC_ArmoredFlagWakeUpZombies()` / PC `0x18036D560`, including self wake-up, zombieList eligibility filters and foreach Dispose/finally. Final `291e22bffefdf98f0fca29475c485264ad5b9eb6cf8a934226d1e73a187ddfb0`.

### HF37 — Projectile Initial Core

Restores `0x060003ED/3EE/3EF Projectile.Initial<T>` /6, /7, /10. Generic attribution is closed through original MethodSpec/generic-method tables. /6 and /7 forward defaults to /10; /10 restores position/speed/z/angular/track/origin initialization while intentionally preserving the original unused `acceleration` parameter. Final `412e12b39d1107c29a20027a37094e796d0059e0a3b8637d78233599148d0c56`. Evidence commit `b4c50aece1eb0e5f4f27096eb510ffc5b88ae394`; Drive folder `1L7mnzN8udi4HX-NiusZsOOgX4tp9S_9u`; final provider readback exactly 22 files.

### HF38 — Plant Kill Event Core

Restores `0x0600037C Plant.KillEvent(Zombie)` and `0x0600038F Plant.PC_SSI_KillEvent(Zombie)`. Final `25ca8b5afe45930dd39517c2477a96db520d067c067493c334f107201a973b5d`. Evidence commit `9942516d8bb151b362619e29e20b03ea900d5a75`; Drive folder `1PbrPjBPT-EbBT9p78aroQHrgLxItcDni`; final provider readback exactly 22 files.

### HF39 — Element Runtime Core

Restores exactly fourteen MethodDefs:

- `0x06000102 BuffManager.AddBuff(Buff)`;
- `0x0600011D Element.Burst()`;
- `0x0600011F Element.Decay(int)`;
- `0x06000121 ElementManager.GetEffectBoardEntry(ElementType,int)`;
- `0x06000123 ElementManager.Effect(Element)`;
- `0x06000125 ElementManager.Update()`;
- `0x0600012D ElementUIController.UpdateUI(Element)`;
- `0x0600013F GlobalStaticVars.AppearSprite(List<GameObject>,string)`;
- `0x06000142 GlobalStaticVars.GetAnimationSprite_Name(List<GameObject>,string)`;
- `0x06000145 GlobalStaticVars.HideSprite(List<GameObject>,string)`;
- `0x0600035F Plant.ElementLevelUp(ElementType,int)`;
- `0x0600036A Plant.GetER()`;
- `0x06000373 Plant.GetElementPreference(ElementType)`;
- `0x0600044D Zombie.GetER()`.

Accepted behavior includes `List<Buff>.Add` then `Awake(false)`, resistance through `GetER`, `abs(point)>=15000` burst threshold, original sign-crossing/shuttle/decay behavior, signed negative UI progress, `Round` then `[0,100]` ER clamp, and SnowPea ID5 preference/element-level sprite transitions.

Formal validation:

- formal HF38 input SHA-verified `25ca8b5afe45930dd39517c2477a96db520d067c067493c334f107201a973b5d`;
- patcher build head `bd8f690f38eb9d872275f707e303442da2265271`, workflow `34431291604` PASS, artifact ID `10134579437`, SHA `244d4931e1612fd5afc631c7b3b125def0787881d0b9b2b8c9616fd690d046e8`;
- independent double patch -> byte-identical `7c6360c79852f94a86e560367f25c79298d62d3e1bf86027f86112d9fc333f6e`, stderr 0;
- fixed ILSpy 11.0.0.9375 + 56 refs: all fourteen targets exit 0, stderr 0, Cpp2IL refs 0, issue markers 0;
- HF38 whole IL reproduced `a6a49a269cf0ef7f45094c0a7d329c0a076d6f42cb651d00a8d9019f87efe836`; HF39 whole IL `23e2734766d033c4b22767fd9b969d8312261cedc24622a38bf6ff5dc5dcc54d`;
- prior HF37->HF38 semantic diff reproduced first at `c4e5311c70cefb2da5c5d14696df0b7383cc99dab8479f833cc6c763254ec0be`;
- MethodDef `2317 -> 2317`, method blocks `2139 -> 2139`, normalized non-method skeleton identical, changed exactly the fourteen declared HF39 MethodDefs;
- HF38->HF39 semantic diff `c7c2b30f98986999cf5ed1de9ef800f044e796a2f4f1002c769cb7c0b57868e0`;
- permanent RecoveryAudit commit `1dfb57a1840ec0e8d4ce5a74f4d4e419f445f76a`, workflow `34431587167` PASS, artifact ID `10134685295`, SHA `409e8692556c0210702a0cf868073f26af3fe702e66ae892504e01669b34eed1`; independent OPEN1/OPEN2 passed all retained targets and ended `RECOVERY_AUDIT_OK`;
- Evidence commit `3725046b046c19299a8993b99d0a7ec73d7928ee`;
- Drive folder `1Xf5KA-ZsvUUKbnL8PveKx5ia2AOPpOZT`;
- cumulative DLL `13G9muTu1ewxGljnTEIJAaxQONe4c16TS`;
- payload manifest `1ila6MOgwuU5DeUFKN4UOt4x7EtuQOR3K`, SHA `a1fab01211b526fcdff6fc2f84d2aafd96cd6a37546abbdb13329a80bdebb7a3`;
- Evidence-FINAL `12ncLOlGKXyQoiFE4_ViV9qmfpYPumQyp`, SHA `809fd6836b0423d5656b405543f1059a36f1799e16ceeb6e302aeb85ee1374d4`;
- SHA256SUMS-FINAL `1MmJP9cjWeJKvjUhNQJg1O-D52dFSHyu2`, SHA `f8aebc6e8a546000cb76669e94b61cadb910df0493eeb93c83df93f7bd098a01`;
- final provider readback exactly 22 files = 20 payloads + 2 closure files.

**HF39 formal acceptance: PASS. HF39 is now the only allowed formal input for any later cumulative HF stage.**

## Unity reconstruction state

AssetRipper ~3,149 objects; reconstructed project ~6,783 files. 173 game script types and 263 refs across 114 assets migrated. MainMenu/Board scenes restored. Package scripts still require 67/67 validation before final Unity integration.

## Decision gate — do not auto-open HF40

Run a fresh residual active-path scan against the HF39 cumulative assembly. A later HF stage may open only if an original-PC-native-backed MethodDef simultaneously has concrete managed loss/mis-reconstruction, material gameplay reachability, and behaviorally closed native dependencies. Warning count, MethodDef adjacency, shared-stub xref centrality, or cosmetic decompiler quality alone are not gates.

If no remaining candidate satisfies those gates, stop HF managed recovery and proceed to 67/67 Unity/package validation, integrate the HF39 cumulative Assembly-CSharp recovery, compile and validate MainMenu/Board/gameplay paths, and only then perform necessary iOS adaptation.
