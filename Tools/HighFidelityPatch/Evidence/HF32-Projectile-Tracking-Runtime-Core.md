# HF32 — Projectile Tracking Runtime Core native recovery evidence

Date: 2026-09-10  
Branch: `high-fidelity`  
Classification: **Native-backed active-path projectile tracking recovery**

## Formal result

Formal HF31 input SHA-256:

`a4d001581430fe440e50feb37f3abd189a1ae86a00f4fe41114ae3f9b99386d2`

**HF32 cumulative candidate SHA-256 pending final Drive 20+2 / STATUS closure:**

`a959e450bd71beb67c948a9a85a1263a7739ccd37b2438405ae954fe10b7bf87`

HF32 restores exactly one MethodDef:

- `0x060003D8 Projectile.Update_Tracking()` — original RID `984`, PC methodPointer `0x18037D4D0`.

No neighboring Projectile MethodDef is modified.

## Original attribution, active path, and native-backed behavior

Attribution retains the project-wide rule: original Assembly-CSharp MethodDef RID-1 -> Assembly-CSharp CodeGenModule methodPointers index. Existing decision-scan evidence records RID 984 / `0x060003D8` / pointer `0x18037D4D0`.

HF24's accepted original-PC `Projectile.Update()` native evidence independently contains direct calls to `0x18037D4D0` at PC callsites `0x18037D952` and `0x18037DA54`. The target is therefore materially active on the restored per-frame projectile runtime path; HF32 is not opened from MethodDef adjacency, Cpp2IL warning count, or xref centrality alone.

Native-backed behavioral closure, completed before the published patcher was built, is preserved unchanged:

- initialize `target = null`;
- initialize `minX = 2147483648f`;
- enumerate `board.zombieManager.zombieList`;
- skip a zombie when `zombie.IsDisabled()`;
- otherwise require `zombie.CanAttacked()`;
- when `minX > zombie.fX`, retain that zombie as `target` and set `minX = zombie.fX`;
- preserve the foreach `Enumerator.Dispose` / `finally` region;
- if a target exists, call `Aim(new Vector3(target.fX - fX, target.fY - fY, 0f))`.

Cpp2IL is not treated as source for this behavior; its damaged managed body only establishes managed loss.

## Formal patcher and deterministic output

HF32 patcher provenance:

- patcher project commit `cb5e7671cfa42e989763209a513183d815fca31d`;
- Template commit `0ad8a06478b42c9f8cd89881849c7838ef5b7e14`;
- patcher engine commit `c0b8e54241472f2c0d8f7ff4fcfd86304f56849d`;
- workflow commit `164a2e469fead6bcacf2d3b896ce9d86bfe1f369`;
- workflow run `34374542786` PASS;
- artifact ID `10113295189`;
- artifact SHA-256 `1ca0b96f6c71dfaefc99642ba9bcc2a7cbf84f5c1c81405a3cd2a754535244cc`.

Formal HF31 was re-fetched from accepted Drive ID `18fn9_hdgHdKXQRMJfIcf0lfDkbwuZdam` and immediately re-hashed as exactly `a4d001581430fe440e50feb37f3abd189a1ae86a00f4fe41114ae3f9b99386d2` before patching.

The published HF32 patcher was independently applied twice to that same formal HF31 input. Both outputs are byte-identical at:

`a959e450bd71beb67c948a9a85a1263a7739ccd37b2438405ae954fe10b7bf87`

Cecil reopen both runs:

- `0x060003D8 Projectile.Update_Tracking/0`;
- `55 IL / 150 bytes / 1 EH`;
- zero target Cpp2IL helper references.

The one EH is the expected foreach `Enumerator.Dispose/finally` region.

## Fixed ILSpy member gate

Fixed ILSpyCmd / ICSharpCode.Decompiler `11.0.0.9375` with the reproduced 56-DLL fixed-Cpp2IL reference set, reference ZIP SHA-256:

`fe091e5a5389c2491eebeff72c83c394097bef67ffd45a25cf7de01c9aad9ef1`

Formal member readback of `0x060003D8` on the HF32 candidate:

- exit `0`;
- stderr `0` bytes;
- Cpp2IL helper refs `0`;
- issue markers `0`.

Readback selects the minimum-`fX` enabled/attackable zombie and then aims by the target-projectile XY delta, matching the native-backed template.

## Whole-assembly IL and semantic isolation

The current HF32 environment first reproduced the accepted prior whole-IL hashes with the same fixed ILSpy and 56 references:

- HF30 whole IL SHA-256 `4fe6bb80fe5de582c67c3e14427b526c244ab5f6dc9cba7cad0ce28970f2136e` — reproduced exactly;
- HF31 whole IL SHA-256 `cef4f8e34d87fb1409488b9e915eb8df39b1d4e699612032192e4be83be810ee` — reproduced exactly;
- HF32 whole IL SHA-256 `e81d4db126c25a9d3f593c0535017d4a0d24e3cca04e24635ea4bfe0b1769c89`;
- stderr `0` for HF30, HF31, and HF32 whole decompiles.

Before computing HF31 -> HF32, the accepted HF30 -> HF31 semantic diff algorithm was rerun and reproduced byte-for-byte at the accepted SHA:

`673a14dcd0b0e3bfa6b1b16bcaa09fed4d5d435b744f04ca406534125fc26638`

The same semantic normalization canonicalizes only physical method RVA comments and physical `I_XXXXXXXX` / `.data` / `<PrivateImplementationDetails>` data-address labels. No semantic opcode, type, member, branch, field, local, or constant text is normalized away.

HF31 -> HF32 results:

- MethodDef `2317 -> 2317`;
- normalized non-method skeleton byte-identical;
- changed MethodDefs: exactly **one**;
- changed target: exactly `0x060003D8 Projectile::Update_Tracking`;
- HF31 -> HF32 semantic diff SHA-256 `2c5caa7d12a898c3897ee123d0ab0e6400d1a2a482381a96ec0e8f6f1a09807a`.

## Permanent RecoveryAudit

RecoveryAudit was permanently extended for `Projectile.Update_Tracking/0 minIL=55` at commit:

`f63ea5fef9ccc27d67bebb153ebb8a7708f8b298`

Published auditor:

- workflow run `34375192695` PASS;
- artifact ID `10113566262`;
- artifact SHA-256 `6c48118b5cbbee81d67b61cdbd3b6a0616f2dc979f861e3a70b9001c3d3d1cde`.

The published auditor was rerun on the formal HF32 candidate for closure:

- OPEN1 `320 types / 2317 methods / 2297 bodies`;
- OPEN1 Update_Tracking `55 IL / 150 bytes`;
- OPEN2 identical;
- terminal `RECOVERY_AUDIT_OK`.

## Google Drive pre-closure archive

Folder: `HF32-Projectile-Tracking-Runtime-Core`  
Folder ID: `1Uww_2PvbJIX3IUvyQRlWIHjqZQ1YHtkO`

Provider pre-closure readback returns exactly 20 payload files with no missing or extra payload relative to the manifest.

Key provider IDs:

- cumulative audited DLL `1SS6-gWmohn9JqU772ZAJ8vkCB4t4QWwP`;
- patcher artifact `1Y4BDC7HoddK277GwwBSZ7PyADuqL_ZN5`;
- fixed ILSpy `1fFGsPZVVePwXf3rJwRrl6SLZg_JzirPw`;
- published RecoveryAudit `1qmjxGZxhq7Uw-OaOtiOA74-faQ6e7lU_`;
- semantic diff `1NEJ86pFbZNWCg_avqtbSA--eSQxmVQZh`;
- Cecil reopen `1cmLvqMkuNUEofRVVSozJiGqWRqtGAEZ_`;
- member summary `1BUi-JwEloCkkp4dvpCluWUelkBfvreFM`;
- member ZIP `1_5ixV8MgLIpUE4OoKrUK8EC8dUOpFsIq`;
- whole-IL provenance `13rZvHq572CvT4VLpN6ZB5diidtLEZV7p`;
- MethodDef table `1tYvHPrHJA0CvcIxRkmel2nKeETM24ay4`;
- RecoveryAudit log `1KdFg6PQidbQz_fcMSz7aHQzmXTnyhK9L`;
- native evidence `1nG12zMQQhBn5wQaDGC9C5bc57i8Mde-M`;
- patch source `1ujB8SbqlGnJ1fgBrWPiVk08E-TMbMPHV`;
- formal patch run1 `1rOYz_sAefrpAxGXc3EJX9ps0zOVNpUcL`;
- formal patch run2 `1e_08XA_-N5OUk1y-CX8K5MuH4LFwn7uF`;
- reference provenance `1Xr4GajjP19IzZwe87lAOYnCkdAA-M9gO`;
- semantic isolation `1GRG9GMATu6esw351SvG67IVxG_Ycdyvw`;
- source/build provenance `1ZsMARFlOTkZpL2XHkZB7ihZm9ZY29kBd`;
- target manifest `1FriKbPlWrgVsACC5EvpgM9k_YhaE5q3x`;
- payload manifest `1c00Jhu-CpQ9vxU3e0XZeA01ZpDA7sZrM`, SHA-256 `b145349724c3dde98d31e997ebbaeee590af7eb3082bb93efb1308bbf06ca49a`.

HF32 is not formal merely because this Evidence file exists. It becomes formal only after `HF32-Evidence-FINAL.txt` and `HF32-SHA256SUMS-FINAL.txt` are generated from this Evidence commit and the 20-file provider archive, both are uploaded, final provider readback proves exactly 22 files, and only then `Recovery/STATUS.md` is advanced.
