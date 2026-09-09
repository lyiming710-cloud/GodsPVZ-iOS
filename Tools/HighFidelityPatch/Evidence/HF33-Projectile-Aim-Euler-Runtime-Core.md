# HF33 — Projectile Aim / Euler Runtime Core native recovery evidence

Date: 2026-09-10  
Branch: `high-fidelity`  
Classification: **Original-PC-native-backed active-path projectile aiming/orientation recovery**

## Formal scope

Formal HF32 input SHA-256:

`a959e450bd71beb67c948a9a85a1263a7739ccd37b2438405ae954fe10b7bf87`

HF33 candidate pending final Drive 20+2 and STATUS closure:

`6b467ef40f2e8e4e7fcff48d71c328457ac4107261a95fbd59908f5516720518`

HF33 restores exactly two MethodDefs:

- `0x060003DC Projectile.Aim(Vector3)` — RID 988 — PC `0x180378D50`;
- `0x060003F8 Projectile.SetEulerAngles(float,float)` — RID 1016 — PC `0x18037C120`.

No adjacent Projectile MethodDef is modified.

## Original-file provenance

The original Windows release was re-fetched from the user's archived `GodsPVZ_1.0.2.zip`, not from a reconstructed or patched binary.

Verified fixed baseline:

- PC package SHA-256 `2f08b4e2243e2bd296f3db694b4a2c9656252d76202c80167bdf3340b9f65b48`;
- `GameAssembly.dll` SHA-256 `9ebd7ca996a5b03fb4a766f7a2502b660d581ddbbb36af4d7bf2f06a211da39d`;
- `global-metadata.dat` SHA-256 `ad992341add498bd1018ac980f42171e0737b0793bf945c8714ad70a445b36b9`.

Attribution uses the fixed rule: original Assembly-CSharp MethodDef RID-1 -> Assembly-CSharp CodeGenModule `methodPointers[index]`.

## Active-path gate

The active-path gate is independently satisfied:

- accepted HF32 `Projectile.Update_Tracking()` directly calls `Aim(Vector3)`;
- original PC `Aim` ends in a direct tail jump to `0x18037C120`, the attributed `SetEulerAngles` body.

Both targets are therefore reached by the formally recovered projectile tracking runtime path. HF33 was not opened from MethodDef adjacency, warning count, or shared-stub xref centrality.

## Native behavioral closure

### `Projectile.Aim(Vector3)`

Original PC `0x180378D50` establishes the managed-observable behavior:

- calculate `Mathf.Atan2(distant.y, distant.x) * 57.29578f`;
- if `speed.x` is ordered-negative, replace angle with `180f - angle`; unordered/NaN does not take the negative branch;
- normalize `distant` with Unity Vector3 normalization behavior, including the `1e-5` magnitude threshold and zero-vector fallback;
- calculate the current three-component `speed` magnitude and assign `speed = normalizedDistant * oldSpeedMagnitude`;
- invoke/tail-call `SetEulerAngles(angle, 0f)`.

### `Projectile.SetEulerAngles(float,float)`

Original PC `0x18037C120` establishes:

- immediately store `this.angular = angular` and `this.zAngular = zAngular`;
- the `this.zAngular` field retains the original argument even if the local effective z value is subsequently adjusted;
- when `angular != 0f`, effective z becomes `angular` when `zAngular == 0f` or `Mathf.Approximately(angular,zAngular)`;
- otherwise select `angular` when `Abs(zAngular) > Abs(angular)`, else `zAngular`, and compute `Mathf.Lerp(angular,zAngular,selected/(angular+zAngular))`;
- `projectileSprite` and `projectileAnimation` retain their existing local Euler x/y and receive effective z;
- `shadow` retains x/y and receives the original `angular` as z;
- when `track` exists, its x/y are intentionally read from `projectileSprite.transform.localEulerAngles`, with effective z; native null behavior is preserved rather than sanitized.

Cpp2IL was used only as evidence of damaged managed reconstruction, not as source for these semantics.

## Published HF33 patcher and deterministic output

Source/build provenance:

- project commit `4b28c285c1a4cddca313b3d74f973a57d5946c53`;
- native-backed template commit `c0feb6991bc94e0498c99cd7bae4aa08ce6909e6`;
- patcher engine commit `ccbe913e4e0996c730bd1be980b738119f2d033a`;
- workflow/head commit `32482ac22db115581b475641b0aa8a23478adc92`;
- workflow run `34381241262` PASS;
- artifact ID `10115939120`;
- artifact SHA-256 `3b53a4e2a22d9efc41918c87f6648911018bdd9a0a1b7c075c864d0fd262db53`.

HF32 was independently re-fetched from accepted Drive ID `1SS6-gWmohn9JqU772ZAJ8vkCB4t4QWwP` and re-hashed to the exact formal HF32 SHA before patching.

Two independent applications of the published patcher produced byte-identical output:

`6b467ef40f2e8e4e7fcff48d71c328457ac4107261a95fbd59908f5516720518`

Cecil reopen:

- `0x060003DC Aim/1`: `31 IL / 93 bytes / 0 EH`;
- `0x060003F8 SetEulerAngles/2`: `118 IL / 386 bytes / 0 EH`;
- target Cpp2IL helper refs: zero.

## Fixed ILSpy member gate

Fixed ILSpyCmd / ICSharpCode.Decompiler `11.0.0.9375` with the reproduced 56-DLL reference set was used. Reference ZIP SHA-256:

`fe091e5a5389c2491eebeff72c83c394097bef67ffd45a25cf7de01c9aad9ef1`

Both formal member readbacks:

- exit `0`;
- stderr `0` bytes;
- Cpp2IL refs `0`;
- issue markers `0`.

The decompiled member semantics match the original-PC-native closure above.

## Whole-assembly IL and semantic isolation

Before computing HF32 -> HF33, HF32 whole IL was regenerated in the HF33 environment and reproduced exactly:

`e81d4db126c25a9d3f593c0535017d4a0d24e3cca04e24635ea4bfe0b1769c89`

HF33 whole IL SHA-256:

`36f84c63dc8906ab33424da6a67246569a7e103e5b2ffbf35ca757d698698450`

Whole-assembly stderr is zero for both.

The accepted semantic algorithm was then validated by first reproducing the previous HF31 -> HF32 semantic diff byte-for-byte at:

`2c5caa7d12a898c3897ee123d0ab0e6400d1a2a482381a96ec0e8f6f1a09807a`

Only then was HF32 -> HF33 computed. Results:

- MethodDef `2317 -> 2317`;
- normalized non-method skeleton byte-identical;
- changed MethodDefs exactly two: `0x060003DC`, `0x060003F8`;
- HF32 -> HF33 semantic diff SHA-256 `59d5f0c92d7115c54bc4f5ee69881c5b120d1a99dabc640e7ce180dbfdc3ceae`.

## Permanent RecoveryAudit

Permanent audit update commit:

`2d57910889988b590f1dd07cdcc34ee12897b952`

Published auditor:

- workflow run `34381940381` PASS;
- artifact ID `10116213345`;
- artifact SHA-256 `3685e1d9fd17d8b277a64a8bd4cc05c293eaff87869bd07d81caa36f1cf51139`.

Published-auditor closure run on the HF33 candidate:

- OPEN1 `320 types / 2317 methods / 2297 bodies`;
- Aim `31 IL / 93 bytes`;
- SetEulerAngles `118 IL / 386 bytes`;
- OPEN2 identical;
- terminal `RECOVERY_AUDIT_OK`.

## Google Drive pre-closure archive

Folder: `HF33-Projectile-Aim-Euler-Runtime-Core`  
Folder ID: `1jGlVm6aPkLamhOSRuCPZezPqwC-zcQmT`

Provider pre-closure readback returns exactly 20 payload files matching the local manifest.

Key provider IDs:

- cumulative audited DLL `1Ogbe_UWDSdux7n9ROWyRr5e6dU1Nk5Vm`;
- patcher artifact `1MbL4joVSYGeIMUJS4FO8qhWbDZ-3DUFZ`;
- fixed ILSpy `1bmEgTCcHue3J_xYaM3q2tqV0HdRvukOk`;
- published RecoveryAudit `1kj41inhhOifAmMlcUL2ZGgHYP4g_KKrc`;
- semantic diff `1WUOWoHmMJagr1N0WBuBHboKGL7YzAfUw`;
- Cecil reopen `1McW1JkySyN1jITDMqoyHOVriLX9faA7u`;
- member summary `1ory8BcYmZPv3gfuG7498QEBaqRlty7Gb`;
- member ZIP `1rI2nisRHIbgyz04Iap6Wd8v6PAnAMRUa`;
- whole provenance `1drHhVaR5sodg1A3xVjVUdUM6VwnRcUGw`;
- MethodDef table `19Scon751pPswZJAVm_-ccqGRZR0nLt4f`;
- RecoveryAudit log `1OJRDOysK9RVjVV0qOgeprd_yo1-bSRnf`;
- native evidence `1aFA43x3zI2G1UdyXmKj1GeKPLFAFV_5F`;
- patch-source provenance `1ImyJFTjYA5SCAD7nFGSmEXmwhVA7Lue-`;
- formal run1 `1BYzYJlUGRlDuggwDnQf-aoOaTYm0Z5kZ`;
- formal run2 `1sjWDLFXb8mypwi8YbTAN1EmVSfExh-cu`;
- reference provenance `1Ji4Dp2ltv-dUwPUPayhg6DvIIulrIDCk`;
- semantic isolation `1N3T_dfsXUwg-pEyC2pbqbHc1GcbEtKfB`;
- source/build provenance `1xPQY2cBnFe1MMw9PbHls9KeutOiK20Li`;
- target manifest `1u0GiMMQpZbzADnm4fhOVYIYSAAvnMCsu`;
- payload manifest `1T5JqgT-axJVHjcKn2CKXfsgrbdNm4uDg`, SHA-256 `bc6ae57092ea8d9e8ddec228b4f0070f5b3658c279a04cccebfab773ef45e84c`.

HF33 remains non-formal until Evidence-FINAL and SHA256SUMS-FINAL are derived from this Evidence commit and the 20-file archive, uploaded, final provider readback proves exactly 22 files, and only then `Recovery/STATUS.md` is advanced.
