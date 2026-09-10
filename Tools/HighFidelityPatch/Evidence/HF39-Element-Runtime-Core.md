# HF39 — Element Runtime Core

Date: 2026-09-10  
Branch: `high-fidelity`

## Scope

HF39 restores exactly fourteen original `Assembly-CSharp` MethodDefs and no others:

- `0x06000102` — `BuffManager.AddBuff(Buff)`
- `0x0600011D` — `Element.Burst()`
- `0x0600011F` — `Element.Decay(int)`
- `0x06000121` — `ElementManager.GetEffectBoardEntry(ElementType,int)`
- `0x06000123` — `ElementManager.Effect(Element)`
- `0x06000125` — `ElementManager.Update()`
- `0x0600012D` — `ElementUIController.UpdateUI(Element)`
- `0x0600013F` — `GlobalStaticVars.AppearSprite(List<GameObject>,string)`
- `0x06000142` — `GlobalStaticVars.GetAnimationSprite_Name(List<GameObject>,string)`
- `0x06000145` — `GlobalStaticVars.HideSprite(List<GameObject>,string)`
- `0x0600035F` — `Plant.ElementLevelUp(ElementType,int)`
- `0x0600036A` — `Plant.GetER()`
- `0x06000373` — `Plant.GetElementPreference(ElementType)`
- `0x0600044D` — `Zombie.GetER()`

Formal input is HF38 cumulative final:

`25ca8b5afe45930dd39517c2477a96db520d067c067493c334f107201a973b5d`

HF39 candidate/final-to-be-closed:

`7c6360c79852f94a86e560367f25c79298d62d3e1bf86027f86112d9fc333f6e`

## Original-native attribution and active-path / managed-loss gate

Fixed originals were re-verified against the retained baseline:

- PC ZIP SHA-256 `2f08b4e2243e2bd296f3db694b4a2c9656252d76202c80167bdf3340b9f65b48`
- PC `GameAssembly.dll` SHA-256 `9ebd7ca996a5b03fb4a766f7a2502b660d581ddbbb36af4d7bf2f06a211da39d`
- PC `global-metadata.dat` SHA-256 `ad992341add498bd1018ac980f42171e0737b0793bf945c8714ad70a445b36b9`

Ordinary MethodDef attribution used original RID-1 -> Assembly-CSharp CodeGenModule `methodPointers[index]`. Retained PC pointers are:

- `BuffManager.AddBuff` `0x180310190`
- `Element.Burst` `0x180315F60`
- `Element.Decay` `0x180316790`
- `ElementManager.GetEffectBoardEntry` `0x180314D50`
- `ElementManager.Effect` `0x180314A30`
- `ElementManager.Update` `0x180315440`
- `ElementUIController.UpdateUI` `0x180315910`
- `GlobalStaticVars.AppearSprite` `0x18031ADF0`
- `GlobalStaticVars.GetAnimationSprite_Name` `0x18031B790`
- `GlobalStaticVars.HideSprite` `0x18031BAC0`
- `Plant.ElementLevelUp` `0x18034EB40`
- `Plant.GetER` `0x180351380`
- `Plant.GetElementPreference` `0x180351460`
- `Zombie.GetER` `0x180360F80`

The HF38 residual scan reached `ElementManager.Update()` directly from the already-formal Plant/Zombie per-frame update paths. Dependency closure then showed that `Update`, `Effect`, `Decay`, `Burst`, UI update, ER/preference, SnowPea sprite helpers and `BuffManager.AddBuff` form one active cohesive runtime chain. HF38 managed readback had concrete semantic loss: broken foreach/enumerator reconstruction, invalid stack/type operations, unresolved native calls, and in `BuffManager.AddBuff` a superficially marker-free but semantically wrong reconstructed list write that effectively targeted `_items[0]` instead of `List<Buff>.Add`.

No further damaged project-level callee remained below the accepted fourteen-method cluster.

## Accepted native behavior

- `BuffManager.AddBuff`: exactly enqueue via `buffs_toAdd.Add(buff)` and call `buff.Awake(false)`.
- `Element.Decay`: while burst is active, subtract `Time.deltaTime`, call `BurstEnd()` when it no longer remains positive, then return. Otherwise honor zero/preference gates, decay according to the original sign and `decaySpeedE`, clamp sign-crossing to zero, then clear `decaySpeedE`.
- `ElementManager.Effect`: obtain ER from Zombie and then Plant when present; compute `incoming = element.point * (1 - ER*0.01)`; find same-type element; stop while it is in burst; add incoming; on sign crossing zero unless the incoming element is shuttle-able, in which case call `Shuttle()`; call `Burst()` at absolute point >= 15000.
- `ElementManager.Update`: process every `elements_toEffect` entry through `Effect`, clear that queue, then decay every retained element. Plant and Zombie decay checks are independent, so a manager containing both can call `Decay` twice on the same element. Finally update a non-null `UIController` for each element.
- `ElementUIController.UpdateUI`: `_Brightness` is 4 during burst and 1 otherwise; progress is `point / 15000`. Zero disables both sides; positive uses back1/image1; negative uses back2/image2. Original signed negative `progress` is written directly to negative-side `fillAmount`; HF39 does not sanitize it with `Abs`.
- `Element.Burst`: set `burstTime=10`, saturate point to `sign*15000`; preserve Plant/Zombie fire_ice VFX and buff behavior. Plant VFX scale is `(65,65,1)`, sprite index 0, Y offset `fZ+30`, parent animationGroup transform. Zombie VFX scale is `(142.5,142.5,1)`, sprite index 6, Y offset `fZ+8`, parent zombie transform. Both use `Particles` sorting and alpha 0.75. The Plant same-preference branch calls `ElementLevelUp`.
- `GetEffectBoardEntry`: preserve the original key mapping and foreach over static `boardEntries`, returning the matching entry or null.
- `GetAnimationSprite_Name`: enumerate animation sprites, compare `sprite.name` to the requested name and return the first match; preserve Enumerator Dispose/finally. `AppearSprite` / `HideSprite` call it and use Unity-object truthiness before `SetActive(true/false)`.
- `Plant.GetER` and `Zombie.GetER`: compute base ER plus BuffManager increment, apply banker-style `MathF.Round`, then clamp to `[0,100]`.
- `Plant.GetElementPreference`: fire_ice on Plant ID 5 returns preference 1; otherwise 0.
- `Plant.ElementLevelUp`: preserve the original max level 3, positive fire_ice stat changes and SnowPea ID5 sprite/talent transitions, including the original `LevelUp` debug log.

## Deterministic patching

The first HF39 patcher build failed at compile time because the Unity template stub lacked `Object.name`; it produced no artifact/candidate. A second published build passed CI but formal application was rejected by its mapper because common Buff fields were declared on the template `StatsIncreased` instead of target base `Buff`; that output was not accepted or propagated. Only metadata-stub/template inheritance was corrected; the fourteen native-backed method bodies were unchanged.

Accepted published patcher build head:

`bd8f690f38eb9d872275f707e303442da2265271`

Accepted workflow run: `34431291604` PASS  
Artifact ID: `10134579437`  
Artifact ZIP SHA-256:

`244d4931e1612fd5afc631c7b3b125def0787881d0b9b2b8c9616fd690d046e8`

Two independent applications to the SHA-verified HF38 formal input were byte-identical at:

`7c6360c79852f94a86e560367f25c79298d62d3e1bf86027f86112d9fc333f6e`

Both runs had zero stderr.

Cecil reopen sizes:

- AddBuff `8 IL / 20 bytes / 0 EH`
- Burst `245 / 840 / 0`
- Decay `79 / 204 / 0`
- GetEffectBoardEntry `44 / 102 / 1`
- Effect `92 / 266 / 1`
- ElementManager.Update `80 / 236 / 3`
- UpdateUI `164 / 512 / 0`
- AppearSprite `11 / 24 / 0`
- GetAnimationSprite_Name `27 / 64 / 1`
- HideSprite `11 / 24 / 0`
- ElementLevelUp `119 / 395 / 0`
- Plant.GetER `14 / 50 / 0`
- GetElementPreference `10 / 16 / 0`
- Zombie.GetER `14 / 50 / 0`

Fixed ILSpyCmd / ICSharpCode.Decompiler `11.0.0.9375` with the reproduced 56-DLL reference set passed all fourteen members with exit 0, stderr 0, Cpp2IL refs 0 and issue markers 0.

## Whole-assembly semantic isolation

HF38 whole IL was reproduced exactly at:

`a6a49a269cf0ef7f45094c0a7d329c0a076d6f42cb651d00a8d9019f87efe836`

HF39 whole IL SHA-256:

`23e2734766d033c4b22767fd9b969d8312261cedc24622a38bf6ff5dc5dcc54d`

Before computing the new diff, the same retained parser/normalization algorithm reproduced the accepted HF37->HF38 semantic diff byte-for-byte at:

`c4e5311c70cefb2da5c5d14696df0b7383cc99dab8479f833cc6c763254ec0be`

HF38->HF39 results:

- MethodDef `2317 -> 2317`
- whole-IL method blocks `2139 -> 2139`
- normalized non-method skeleton byte-identical
- changed MethodDefs exactly the fourteen tokens listed in Scope
- semantic diff SHA-256 `c7c2b30f98986999cf5ed1de9ef800f044e796a2f4f1002c769cb7c0b57868e0`

MethodDef table SHA-256:

`bcf888681f2f630430a695028ebca781771ef4ef76efec16ebf7170bb9b98566`

## Permanent RecoveryAudit

RecoveryAudit commit:

`1dfb57a1840ec0e8d4ce5a74f4d4e419f445f76a`

Workflow run: `34431587167` PASS  
Published auditor artifact ID: `10134685295`  
Auditor artifact SHA-256:

`409e8692556c0210702a0cf868073f26af3fe702e66ae892504e01669b34eed1`

Independent published-auditor execution passed the full retained audit set twice, with identical OPEN1/OPEN2 results for all fourteen new targets, zero stderr, and terminated with `RECOVERY_AUDIT_OK`.

## Source provenance

Accepted published patcher source is pinned to immutable build head `bd8f690f38eb9d872275f707e303442da2265271`:

- `Template.cs` blob `26fcfed342aba3265638971609205324b6dced46`
- `Program.cs` blob `1efdfb61d3e352253f8ba0e0b4eb3b564d120586`
- `HF39Patch.csproj` blob `d500ed90c5755ce51cfe2254c73ae46ae351800e`
- workflow blob `89da43ea8fdf64843038fe6f724bf03f38deaa25`

Archived source provenance records these immutable IDs; mirrored source files were checked against Git blob identities where retained.

## Drive pre-closure archive

Folder: `HF39-Element-Runtime-Core`  
Folder ID: `1Xf5KA-ZsvUUKbnL8PveKx5ia2AOPpOZT`

Key provider IDs:

- cumulative DLL `13G9muTu1ewxGljnTEIJAaxQONe4c16TS`
- patcher `1Qoh3G5g_DpVQ_TomLQ4RWvH7xHik3A5K`
- fixed ILSpy `1D0t_4FQ4IuI0X3Q8o6p9Ei6YhGx8_zu-`
- RecoveryAudit `1abx-UPBVJ5yyPDMGn2GiwLsQ3z1qTw4U`
- semantic diff `1vAYiIB7pAFRO35D32cu1Hh0LgOedofKj`
- native evidence `15HxUKLLSf0svBKF3v_wyup8ba7iSnxYo`
- patch source `1GVn347uKFYZ5KAl-QgRuoOY14-h1689l`
- payload manifest `1ila6MOgwuU5DeUFKN4UOt4x7EtuQOR3K`

Payload manifest SHA-256:

`a1fab01211b526fcdff6fc2f84d2aafd96cd6a37546abbdb13329a80bdebb7a3`

Provider readback returned exactly 20 pre-closure files. HF39 is not formal until Evidence-FINAL and SHA256SUMS-FINAL are added, the folder is read back as exactly 22 files, and `Recovery/STATUS.md` is advanced only afterward.
