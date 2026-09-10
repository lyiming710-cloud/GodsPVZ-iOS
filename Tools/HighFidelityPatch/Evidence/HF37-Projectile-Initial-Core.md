# HF37 — Projectile Initial Core

Date: 2026-09-10  
Branch: `high-fidelity`

## Scope

HF37 restores exactly three original `Assembly-CSharp` MethodDefs and no others:

- `0x060003ED` / RID 1005 — `Projectile.Initial<T>(Vector3, Vector3, Vector3, float, int, T)`
- `0x060003EE` / RID 1006 — `Projectile.Initial<T>(Vector3, Vector3, Vector3, float, float, int, T)`
- `0x060003EF` / RID 1007 — `Projectile.Initial<T>(Vector3, Vector3, Vector3, float, float, int, float, float, float, T)`

Formal input is HF36 cumulative final:

`291e22bffefdf98f0fca29475c485264ad5b9eb6cf8a934226d1e73a187ddfb0`

HF37 candidate/final-to-be-closed:

`412e12b39d1107c29a20027a37094e796d0059e0a3b8637d78233599148d0c56`

## Original-native attribution and managed-loss gate

Fixed originals were re-verified:

- PC ZIP SHA-256 `2f08b4e2243e2bd296f3db694b4a2c9656252d76202c80167bdf3340b9f65b48`
- PC `GameAssembly.dll` SHA-256 `9ebd7ca996a5b03fb4a766f7a2502b660d581ddbbb36af4d7bf2f06a211da39d`
- PC `global-metadata.dat` SHA-256 `ad992341add498bd1018ac980f42171e0737b0793bf945c8714ad70a445b36b9`

These are generic MethodDefs, so concrete native attribution uses the original metadata MethodSpec / generic-method-function tables, not a guessed ordinary `methodPointers[]` slot. A concrete original `Plant.KillEvent` call carries encoded MethodInfo `0xC0024671`; metadata-v31 decoding gives MethodSpec index 74552, which resolves to global methodDefinitionIndex 42770 = `Assembly-CSharp` `0x060003ED Projectile.Initial<T>/6`, method-instantiation 2453. The sole type argument resolves `Il2CppType.CLASS -> typeDefinition 0x1457 -> Plant`, proving that call is `Initial<Plant>`.

Native evidence retains the six-parameter wrapper around `0x1804A25F0`, seven-parameter wrapper around `0x1804A24E0`, ten-parameter body around `0x1804A2700`, shared implementation around `0x180008CF0`, and the `Plant.KillEvent` callsite. Static PC callsite scanning found the overload family materially active across PlantAnimationEvent, Device, Plant, Zombie and SkillManager paths.

HF36 managed readback shows concrete loss: the 6/7 parameter overloads reduce to Cpp2IL unmanaged-load / missing-method markers, and the 10-parameter overload has extensive invalid-stack/type reconstruction and wrong field/value mappings.

## Accepted native behavior

- `/6` forwards to `/10` with `zSpeed=0`, `zAngular=0`, `angularSpeed=0`, `angularAcceleration=0`.
- `/7` preserves supplied `zSpeed` and forwards to `/10` with the three angular parameters zero.
- `/10` restores `fX`, `fY = startPosition.y - fZ`, `fZ`, `speed`, `zSpeed`, `movementTracks`; calls `SetEulerAngles(0f, zAngular)`; stores angular speed/acceleration; sets `zAcceleration = -2025f` only for movement track 1 and otherwise zero; calls `Moving_SetNewPosition()`; sets `previousPosition = new Vector3(-1000000f, fY, shadow.transform.position.y - fY)`; then assigns `origin_Plant` and/or `origin_Zombie` through native runtime type tests.
- The `acceleration` argument is intentionally unused by the original native body. HF37 does not invent a `this.acceleration` write.

## Deterministic patching and readback

HF37 patcher build head: `58b9316853725a0c5da0c51960ce7e381fa56473`  
Workflow run: `34423760532` PASS  
Artifact ID: `10131864829`  
Artifact SHA-256: `4b33df295489a157ca5008734ea7321d5b60641703b55b44c0c98416f49ae478`

Two independent applications to the SHA-verified HF36 formal input were byte-identical at:

`412e12b39d1107c29a20027a37094e796d0059e0a3b8637d78233599148d0c56`

Both runs had zero stderr. Cecil reopen sizes:

- `0x060003ED`: 13 IL / 36 bytes / 0 EH
- `0x060003EE`: 13 IL / 33 bytes / 0 EH
- `0x060003EF`: 75 IL / 213 bytes / 0 EH

Fixed ILSpyCmd / ICSharpCode.Decompiler `11.0.0.9375` with the reproduced 56-DLL reference set passed all three members with exit 0, stderr 0, Cpp2IL refs 0 and issue markers 0.

## Whole-assembly semantic isolation

HF36 whole IL was reproduced at:

`6a0c79a8c0b758c35d61e7edba8da5740dd72cb82426f976c064ad358b7d5391`

HF37 whole IL SHA-256:

`dd045b66df4989f3e617ec2b4ee7f8f398dbba28b9305b31433de5314cffe655`

Before computing the new diff, the same parser/normalization algorithm reproduced the accepted HF35->HF36 semantic diff byte-for-byte at:

`83f0cd40ea1d266d2f78972b18ccc177fd77a9b12792109e40a36db9e134250e`

HF36->HF37 results:

- MethodDef `2317 -> 2317`
- whole-IL method blocks `2139 -> 2139`
- normalized non-method skeleton byte-identical
- changed MethodDefs exactly `0x060003ED`, `0x060003EE`, `0x060003EF`
- semantic diff SHA-256 `125ca6d7ec2e0322056bc7014bae7aed911a9953973e804009cc8ae8d6c35d0b`

## Permanent RecoveryAudit

RecoveryAudit commit: `04355be372d0ae6c928bf7e2b8e5c2a1647f0903`  
Workflow run: `34424112325` PASS  
Published auditor artifact ID: `10131987391`  
Artifact SHA-256: `9a9d594eddec824e56202093788e243d86410cdbb0505bf1035e17c16130a1e1`

Independent published-auditor execution passed the entire retained audit set twice and ended in `RECOVERY_AUDIT_OK`. New targets were identical in OPEN1/OPEN2 at 13/36, 13/33 and 75/213.

## Source provenance

Published patcher source is pinned to immutable head `58b9316853725a0c5da0c51960ce7e381fa56473`:

- `Template.cs` blob `d0211af9535b8ff8d57b2d6cad09219a48e96dee`
- `Program.cs` blob `041e1da3a2fbfd36124b96c1692cde4b908a6977`
- `HF37Patch.csproj` blob `2b30683160b6c27e0ee00b50ce40f8fb69c08557`
- workflow blob `abaed5f96f823a150655641686ab52b4a081fa00`

All four archived files were independently re-hashed as Git blob objects and matched those IDs.

## Drive pre-closure archive

Folder: `HF37-Projectile-Initial-Core`  
Folder ID: `1L7mnzN8udi4HX-NiusZsOOgX4tp9S_9u`

Key provider IDs:

- cumulative DLL `15oNQ6MZE_wxygY1-ya1xQ5KbYfgRklBK`
- patcher `1d13Til10Ioy8gjNTC0J7ethFA8JdKo1_`
- fixed ILSpy `1vGzXWBADE5UzOSlT816cEsVeQrkw88z8`
- RecoveryAudit `1DtOyMMJAHw9eCtWaxfYjSWI8dDShFo1g`
- semantic diff `12KmNpluEBXYemmwiN2lz5UfXShwZyRWW`
- native evidence `1HNjfy4aLXq0JeFRZHOjRpF2AgtrdB7Qs`
- patch source `1zGw51m_VwnACOyBijLClMCdn7mtKn7fJ`
- payload manifest `1f49bqZdssXrAuAk0ATh2JIbjSUPrblwj`

Payload manifest SHA-256: `e47ceee4b97030c4f94b304c707b857c47a038c895f0e5b99ebe1c39b5b0a22e`.

Provider readback returned exactly 20 pre-closure files. HF37 is not formal until Evidence-FINAL and SHA256SUMS-FINAL are added, the folder is read back as exactly 22 files, and `Recovery/STATUS.md` is advanced only afterward.
