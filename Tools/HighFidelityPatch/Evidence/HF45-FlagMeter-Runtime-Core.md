# HF45 — FlagMeter Runtime Core

Date: 2026-09-10  
Branch: `high-fidelity`

## Scope

HF45 restores exactly two original `Assembly-CSharp` MethodDefs and no others:

- `0x06000617` — RID 1559 — `FlagMeter.Update()` — PC `0x18039D010`
- `0x06000618` — RID 1560 — `FlagMeter.UpdateMeter(int,int)` — PC `0x18039CF30`

Formal input is HF44 cumulative final:

`fe5e73abead459121ea00c0f91be7e7abc8b3392eb924458b9c0c0352b444118`

Accepted HF45 cumulative candidate:

`a34feef56319f047a9f60caef0028db9592536b89fa484c41e371a826d6ccd5a`

This file records an accepted candidate only. HF45 is not formal until strict Google Drive 20+2 closure is provider-read back as exactly 22 files and `Recovery/STATUS.md` is advanced afterward.

## Fresh post-HF44 residual decision

HF45 was not opened automatically. Fresh fixed-ILSpy active-path scanning of the HF44 formal cumulative assembly showed that `Zombie.SetUpdateRate()` and several residual Projectile helper bodies had no qualifying live call sites and therefore were not promoted by adjacency or warning count.

`EnemyManager.TimeUpdate()` remains a strongly live residual because `EnemyManager.Update()` directly calls it, but its dependency closure is broader: it directly reaches damaged wave/UI helpers including `TextWaveHealth()` and `FlagMeter.UpdateMeter()`. It was deliberately not patched in this stage.

The FlagMeter pair independently satisfies the late-stage gate: `FlagMeter.Update()` is a Unity lifecycle method, and `FlagMeter.UpdateMeter()` is a direct native dependency of live `EnemyManager.TimeUpdate()`. Both have concrete managed reconstruction loss and compact original-PC-native behavior with closable dependencies.

## Original PC attribution

Fixed original baseline:

- PC ZIP SHA-256 `2f08b4e2243e2bd296f3db694b4a2c9656252d76202c80167bdf3340b9f65b48`
- PC `GameAssembly.dll` SHA-256 `9ebd7ca996a5b03fb4a766f7a2502b660d581ddbbb36af4d7bf2f06a211da39d`
- PC `global-metadata.dat` SHA-256 `ad992341add498bd1018ac980f42171e0737b0793bf945c8714ad70a445b36b9`
- PC CodeRegistration `0x1815E88C0`

Ordinary attribution used original MethodDef RID-1 into the original `Assembly-CSharp.dll` CodeGenModule methodPointers table. Patched-DLL ordering and Cpp2IL executable-body ordering were not used.

Original `.pdata` boundaries:

- `FlagMeter.UpdateMeter`: `[0x18039CF30, 0x18039D009)`
- `FlagMeter.Update`: `[0x18039D010, 0x18039D226)`

## Accepted native behavior

### `FlagMeter.UpdateMeter(int theFlag, int theWave)`

The native body computes `currentWave = theWave + 1`, converts both `currentWave` and `wavesNum` to single precision, and stores `progress = (float)currentWave / (float)wavesNum`. This corrects the damaged managed reconstruction that had collapsed the path into integer/object arithmetic.

The native signed divide-by-10 compiler sequence tests `currentWave % 10 == 0`; on those waves only, `theFlagID = theFlag`. It then formats `currentWave.ToString() + "/" + wavesNum.ToString()` and calls `text_currentWave.SetText(..., true)`. The original null/exception behavior is retained; no defensive guard was added.

### `FlagMeter.Update()`

The native routine performs:

- `flagMeter1.fillAmount = progress`;
- `targetX = 218f - progress * 436f`;
- if the current head local X is ordered-greater than `targetX`, subtract `Time.deltaTime * 36f` from X while preserving Y/Z and write it back;
- when `theFlagID >= 0`, index `flagMeter_FlagList[theFlagID]`, obtain `transform.GetChild(1)`, and if its local Y is ordered-`<= 90f`, add `Time.deltaTime * 100f` to Y while preserving X/Z.

Native COMISS branch behavior is preserved: NaN/unordered does not enter either movement update. Constants `436f`, `218f`, `36f`, `90f`, and `100f` were read directly from the PC image. Original List/index/null/Transform exception behavior remains unchanged.

## Published patcher and deterministic patching

Published patcher head:

`289191fdf2fb9f30f2599833bd4b9b8de50fd6a0`

Workflow run: `34489420443` PASS  
Artifact ID: `10157078781`  
Artifact ZIP SHA-256:

`21fbdb6a5814dd328941ac0ae36ec45de981b3617e3edcb0c678568d6bee0b67`

After artifact publication, HF44 formal cumulative DLL Drive ID `1gGvEopIJHDzdVilPHSVf1ka_Rm-Pfx9a` was re-fetched and SHA-verified at `fe5e73abead459121ea00c0f91be7e7abc8b3392eb924458b9c0c0352b444118`.

Two independent applications of the published patcher completed with exit 0 and zero stderr. Outputs are byte-identical at:

`a34feef56319f047a9f60caef0028db9592536b89fa484c41e371a826d6ccd5a`

Cecil reopen:

- `0x06000617 FlagMeter.Update/0` — `68 IL / 184 bytes / 0 EH`
- `0x06000618 FlagMeter.UpdateMeter/2` — `31 IL / 74 bytes / 0 EH`
- both targets exceed locked reopen floors
- no Cpp2IL helper remains in either target

## Fixed ILSpy member gate

Fixed ILSpyCmd / ICSharpCode.Decompiler `11.0.0.9375` with exactly 56 fixed references was used. `FlagMeter` decompilation completed with exit 0 and zero stderr. Both target blocks contain zero Cpp2IL references, zero NotImplemented/decompiler-issue markers, zero invalid stack/type/comparison markers, and zero warning/error markers.

Readback shows the native-backed single-precision wave progress, signed `% 10` flag activation, current/total text update, head-meter movement, and flag-child vertical animation directly.

## Whole-assembly semantic isolation

Using the same fixed ILSpy toolchain:

- HF43 whole IL reproduced `26da408a5fe00508522d3a784338c4c50577ab1a78226d3c1929886e42760743`
- HF44 whole IL reproduced `dbb10be399d1672a0927510a2fb05481783aabdfd56c9eb921d2bb9982ce2b1f`
- HF45 whole IL SHA-256 `4d4ccd9b2c857ef9aceb0e18896e77cb6b493d9e654dc0a27dce2f5bfbd46668`
- all whole-assembly decompiles exit 0 with zero stderr

Before HF44->HF45 isolation, the retained normalization/parser reproduced the Drive-archived HF43->HF44 semantic diff byte-for-byte at `c79688ce7518eb8fd5fe0cc697d2569ff39b4a189c004ceacd9460a09c430294`.

HF44->HF45 results:

- MethodDef `2317 -> 2317`
- distinct method-body RVAs `2139 -> 2139`
- normalized non-method skeleton byte-identical
- normalized skeleton SHA-256 both sides `afc47c7eaa314ef12143a7ac99ab0db9fcb484b045d76b2fc74d6f2c93ac3b88`
- changed exactly `0x06000617`, `0x06000618`
- HF44->HF45 semantic diff SHA-256 `a1508fbe29703cec50c81b8db85857fcdb2e1fda4abbdf24a1059b0fcda225e5`
- canonical HF44 MethodDef table reproduced byte-for-byte at `ab1557de0172766a51cc30dd7edae12d5a0cd29551cfb659a0e3641571afc790`
- HF45 canonical MethodDef table SHA-256 `784c0d85a4dd74015670c865493c2e08e324fc54c83541caace717e06300b50d`

No third MethodDef changed.

## Permanent RecoveryAudit

RecoveryAudit commit:

`bf54c77b625d818449d3a80a724a00ae2c0b95f6`

Workflow run: `34489954734` PASS  
Published auditor artifact ID: `10157301323`  
Auditor artifact ZIP SHA-256:

`c3d5aa29b9b2ef40803924de08fac48a1e66bf7062d8782e3ef990db8a9f06fc`

The published auditor was executed independently twice against the accepted HF45 candidate. Both process runs completed with exit 0 and zero stderr. Each process performed internal OPEN1 and OPEN2 over the complete retained historical target set and ended with `RECOVERY_AUDIT_OK`.

HF45 readback in OPEN1 and OPEN2 was stable at:

- `FlagMeter.Update/0` — `68 IL / 184 bytes`
- `FlagMeter.UpdateMeter/2` — `31 IL / 74 bytes`

## Source provenance

Published HF45 patcher source is pinned to immutable build head `289191fdf2fb9f30f2599833bd4b9b8de50fd6a0`:

- `HF45Patch.csproj` blob `47c79877526686e4d06ba3d2edfca82e736e8847`
- `Program.cs` blob `3872f59bd8c983b82dadc30a724fb7912ebd1bc0`
- `Template.cs` blob `e3ec497077997c33b488135c202c42bb8f33b9e9`
- workflow blob `4a5993588b45bdf0c7510182af91f4d0dc2cf74c`

RecoveryAudit source blob after HF45 extension is `2afcc0562c8c7bdfea2a3576b4974385b6280ed7`.

## Closure state

All gates through permanent published RecoveryAudit are PASS. Remaining formal steps are strict Drive 19 ordinary payload + payload manifest pre-closure readback, the two final closure files, exact 22-file final provider readback, and only then `Recovery/STATUS.md` advancement. Until those complete, HF44 remains the only formal cumulative final and no later HF stage may consume the HF45 candidate.
