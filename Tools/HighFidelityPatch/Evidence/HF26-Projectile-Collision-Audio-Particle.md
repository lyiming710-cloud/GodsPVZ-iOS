# HF26 — Projectile Collision Audio / Particle native recovery evidence

Date: 2026-09-09  
Branch: `high-fidelity`  
Classification: **Native-backed managed-observable recovery of Projectile collision hit audio/particle post-processing**

## Formal result

Formal HF25 input SHA-256:

`eb0823deff8289d152af52ab89b91672b64bb786701d31d4871ebfcdbe9f9b71`

HF26 cumulative candidate/final payload SHA-256:

`afa0e052902139c09cee9c715fe9a75c6f124af3af8a5647c799dc6b457fefac`

HF26 restores exactly one MethodDef and no others:

- `Projectile.Collision_AudioParticle()` — `0x060003EB` — RID `1003` — PC `0x18037A410`.

The target detectors/resolution bodies (`CollisionDetect_Device`, `CollisionDetect_Plant`, `CollisionDetect_Zombie`, `Collision_Device`, `Collision_Zombie`) remain outside HF26 and require a later decision gate rather than automatic scope expansion.

## Why HF26 is required

HF25 formally restored `Projectile.CollisionDetect_Ground()`. Every accepted ground hit directly calls `Collision_AudioParticle()` before `DestroyProjectile()`. The HF25 managed EB body still contained unresolved indirect-jump/unmanaged-memory Cpp2IL placeholders plus invalid object/int/string conversions, so the newly restored ground-hit path still entered a materially damaged observable post-processing method.

This satisfies both stage-opening conditions independently of detector warnings: original PC native proves concrete managed loss, and HF25 itself proves the method is on the active projectile collision path.

## Original attribution / metadata evidence

Original Assembly-CSharp attribution was re-derived with `original MethodDef RID - 1 -> methodPointers index`:

- RID 1003 / index 1002 -> `0x18037A410` `Projectile.Collision_AudioParticle`.

Original Projectile field-offset evidence used by the body includes `ID +0x24`, `fX +0x44`, `fY +0x48`, `fZ +0x4C`, `index +0x74`, and `board +0xE8`.

The archived native bundle contains the original PC body, broken HF25 managed readback, attribution/field-offset evidence, raw float constants and both native jump-table decodes.

## Native-observable particle behavior restored

The original particle dispatch is restored as follows:

- ID 0 -> `PeaSlapt`;
- ID 1 -> `SnowPeaSlapt`;
- ID 9 -> `CherryBoom`, y=`fY+40`, scale `1.1`, `Board.Shake(0.15,8)`;
- ID 10/11 -> `CherryBoom`, `Board.Shake(0.1,5)`;
- ID 15 -> `PotatoBoom`, y=`fY`;
- ID 19/20 -> `IcicleSlapt`;
- ID 26 -> `SPHBombing`, `Board.Shake(0.5,10)`;
- ID 27/28 -> `CherryBoom`, scale `0.75`, `Board.Shake(0.1,5)`;
- ID 30 -> `LightSaberDoom`, y=`fY`, `Board.Shake(0.15,10)`, Animator integer `"type"=index`;
- ID 31 -> `SPHBombing`, `Board.Shake(0.5,10)`;
- ID 32 -> `CherryBoom`, scale `1.2`, `Board.Shake(0.2,8)`.

Created particles are placed at `(fX, particleY, 0)`, receive a `SortingGroup` on layer `Particles`, and have their existing local scale multiplied component-wise by the native ID-specific multiplier.

## Native-observable audio behavior restored

Supported audio paths use native pitch `Random.Range(0.9f,1.1f)` and the original clip/volume routing:

- ID 0/1 -> `particleClips[Random.Range(0,3)+7]`, volume `*0.8`;
- ID 9/10/11 -> `particleClips[1]`, volume `*0.8`;
- ID 15 -> `particleClips[0]`;
- ID 19 -> `particleClips[10]`;
- ID 20 -> `particleClips[11]`;
- ID 23 -> `particleClips[21]`;
- ID 24 -> `particleClips[11]`, volume `*0.5`;
- ID 25 -> `particleClips[11]`, volume `*0.75`;
- ID 26/31 -> `particleClips[16]`, volume `*1.5`;
- ID 27/28 -> `particleClips[1]`, volume `*0.6`;
- ID 30 -> first `zombieClips[10]`, then `particleClips[22]`;
- ID 32 -> `particleClips[1]`, volume `*1.2`.

ID 0/1 preserve native RNG ordering: float pitch RNG first, integer clip-index RNG second. ID 30 preserves two observable `CreateAudioAtPoint` calls and separately evaluates `Camera.main.transform.position` for each call.

## Patcher and repeatability

HF26 patcher source is under `Tools/HF26Patch/`.

- `Program.cs` Git blob `02e07de8ded9262835c09f4ecbc4a48227b1e06f`
- `Template.cs` Git blob `ec4172326b128884d56a1a995fac449ab1ab2d37`
- `HF26Patch.csproj` Git blob `2bde0667aac852b72b2f32a83082f6504f7f8ad3`
- build workflow Git blob `710170c5b73c2e196a46e6d6b4730775e9aa08bb`
- patcher source head `85b73cdc82cd697cc8b4bd3df8bee2eb896db24d`
- workflow run `34319860392` — success
- patcher artifact SHA-256 `981d2945a73e436a6e7710c0d904c88f6b3df603c471853697d3c56de16e3a90`

The patcher hard-rejects any input not hashing to accepted HF25. Formal HF25 was re-fetched from Drive ID `1ENIzUe4qD4qdSBGXACyzQ2CmMIwja2i5` and independently re-hashed before the final runs.

Two independent applications to the same formal HF25 input are byte-identical at:

`afa0e052902139c09cee9c715fe9a75c6f124af3af8a5647c799dc6b457fefac`

Cecil reopen on both runs:

- `0x060003EB Projectile.Collision_AudioParticle` — `392 IL / 1379 bytes / 0 EH`;
- no Cpp2IL helper remains.

## Independent permanent RecoveryAudit

Permanent `Tools/RecoveryAudit/` was extended with EB's formal measurement in commit `6148f8d036c6d0a64aeb4efb661ff342dc5784b6`; resulting `Program.cs` blob `fdd8775f4422fbf937f2c9ca6000b708fb737aba`.

RecoveryAudit workflow run `34320757146` succeeded. Published artifact SHA-256:

`8274ba48f23aacceb0ee0e1bed9548d88f5396f1cdd4a33fb6f5756f6d439b29`

Published auditor run on final HF26:

- OPEN1: `320 types / 2317 methods / 2297 bodies`, EB `392 IL / 1379 bytes`;
- OPEN2: `320 types / 2317 methods / 2297 bodies`, EB `392 IL / 1379 bytes`;
- final `RECOVERY_AUDIT_OK`.

## Fixed ILSpy / whole-assembly isolation

ILSpyCmd / ICSharpCode.Decompiler remain fixed at `11.0.0.9375`; references remain the reproduced 56-DLL fixed-Cpp2IL set from Cpp2IL source commit `5fb20304df698ffd3d0e664b2a698cd911dc9d57` with reference ZIP SHA-256 `fe091e5a5389c2491eebeff72c83c394097bef67ffd45a25cf7de01c9aad9ef1`.

EB member readback exits 0 with stderr 0, zero Cpp2IL references and zero issue markers. Whole HF25/HF26 stderr is 0. Re-decompiling HF25 in the HF26 environment reproduces accepted HF25 whole-IL SHA exactly.

- HF25 whole IL SHA-256 `9fc5f9d317d6469ba8e24ad09ff22e8480e562e81b5595f1e4ae3d1a50f10722`;
- HF26 whole IL SHA-256 `4e3d8039a6a1a07e02b8e9d998be057e1eee95d4bdf139d91ac832dd144f1714`;
- MethodDef count `2317 -> 2317`;
- normalized non-method skeleton byte-identical;
- exactly one declared MethodDef changes: `0x060003EB`;
- semantic diff SHA-256 `08c012c242a50c97d26fb773dddf9f48eabf541b07b465e4c1f4c560178de0f9`.

The semantic-normalization implementation was first checked by regenerating HF24->HF25: it reproduced accepted HF25 semantic diff SHA `e64591dafd6297de6c3193683eef80078cc27490a849ce92206250f450cea2a2` byte-for-byte before being used for HF25->HF26.

## Drive formal acceptance

Archive folder `HF26-Projectile-Collision-Audio-Particle`, ID `1mPZ-qZ5WBdjfBzPpR4nKxiufPvLbK-TH`.

Key payload artifacts:

- cumulative audited DLL `1yc82VM4pK5DdIlu7-qS9VLdcGDsy-ti_`;
- patcher `1OMV__8C5FwuPyrtyZeQ1WgMzZh_UBJcg`;
- published RecoveryAudit `1-V7BOEo5P2lKVQ83BZZQwFSJ5vFWBG0w`;
- fixed ILSpy bundle `1DMN2tq_QZI3qlpRgfRI-VWQba9C9MT5F`;
- native evidence `1j6fZZcIN4NYkYLenLyMy25ImfnIJYCYp`;
- patch source `1tUo0A9fs6lgwgkSeJcxiJ4yxfg1ecq58`;
- semantic diff `1S9sijFx7gg4tnvmBQSe8eMKFqtNq90Sn`;
- semantic isolation `1pCt00yAEI7bqcY772nG7cRDgaRDEKUwB`;
- MethodDef table `1iRV-EdQA-35OJZ3v0aZHFtI7B9gAOljv`;
- RecoveryAudit log `1vwxGm00hsoATQe-omWcynVSTLIRFZtxB`;
- payload SHA manifest `1h7P7OUC_-tQuoip3wEZSDK_rNvitXWQr`, SHA-256 `b5fedf16e09450c1494414910d7d8af5523ab817d46e6fb028cb40f02027503d`;
- authoritative Evidence-FINAL `1cMvaPfip_Pg5zHIOHfqNWdjcOned6hBX`, SHA-256 `6355726fe788f5124fd75d3fa842e12f5b517b22af16a881b4af399adecd5ab7`;
- authoritative SHA256SUMS-FINAL `1ZhXT2lU60PVs6ob0F2zL2slGRfupeBE4`, SHA-256 `0c8c724f972e089e77dd540c922bccec28b6f43524cbe5b14ee53d50be92c657`.

Provider final readback has no next page and verifies exactly **22 files = 20 payloads + 2 closure files**.

**HF26 formal acceptance: PASS. After STATUS advancement, HF26 is the only allowed formal input for any later cumulative HF stage.**

## Next decision gate

Do not automatically open HF27. After formal HF26 acceptance, rerun the active-path decision gate on `CollisionDetect_Device`, `CollisionDetect_Plant`, `CollisionDetect_Zombie` and only their demonstrated direct business dependencies. MethodDef adjacency and warning counts alone remain insufficient. If no remaining candidate satisfies both concrete PC-native managed loss and material active-path importance, stop HF recovery and proceed to Unity/package validation.
