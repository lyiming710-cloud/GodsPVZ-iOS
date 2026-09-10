# HF36 — Armored Flag Wake-Up Core native recovery evidence

Date: 2026-09-10
Branch: `high-fidelity`
Classification: **Exact for managed-observable behavior of the declared MethodDef**

## Formal result

Formal HF35 input SHA-256:

`ffd3e858a6ea15c1203a8c543e800b9181e0ab115fa7ab6a84d00975be5b947b`

HF36 cumulative candidate / final-after-closure SHA-256:

`291e22bffefdf98f0fca29475c485264ad5b9eb6cf8a934226d1e73a187ddfb0`

HF36 restores exactly one MethodDef:

- `0x0600048C Zombie.ZC_ArmoredFlagWakeUpZombies()` — original RID `1164`, PC methodPointer `0x18036D560`.

Attribution follows the fixed rule: original MethodDef RID-1 -> Assembly-CSharp CodeGenModule `methodPointers[index]`.

## Why this method qualifies

The pre-HF36 managed body has concrete reconstruction damage, not merely cosmetic decompiler noise. Fixed ILSpy on the HF35 input shows Cpp2IL helper calls around original helper `0x1808197F0` / Enumerator construction and `0x180302170` / Enumerator cleanup, with invalid Enumerator/current reconstruction and issue markers.

The method is materially active. The already-formal `Zombie.TakeDamage(Damage,Projectile)` body directly calls `ZC_ArmoredFlagWakeUpZombies()` from its `ID == 10` branch.

## Original PC native closure

Original PC `GameAssembly.dll` SHA-256:

`9ebd7ca996a5b03fb4a766f7a2502b660d581ddbbb36af4d7bf2f06a211da39d`

The original body at `0x18036D560` establishes:

1. For `this`, require `isStant && !immune_wakeUp && !hide`; when true call `Path_Finding()` then `TranToWalk()`.
2. Resolve `board -> zombieManager -> zombieList` and enumerate the list.
3. For each zombie, require `!immune_wakeUp && !hide && !IsDisabled() && isStant`.
4. For every passing zombie call `Path_Finding()` then `TranToWalk()`.
5. Preserve the list Enumerator cleanup/finally path.
6. Preserve original null/throw behavior; no defensive sanitization is added.

Native project callees are closed: `IsDisabled` = PC `0x1803659E0`, `Path_Finding` = `0x180365EA0`, `TranToWalk` = `0x18036A8C0`. Collection construction/MoveNext/Dispose is standard managed List Enumerator machinery.

## Patcher and deterministic formal application

HF36 patch source blobs:

- `Tools/HF36Patch/Template.cs`: `0a97701c0af955237470cc0a244e4f09dbeacc3d`
- `Tools/HF36Patch/Program.cs`: `802834454feaa4273671e9c1bd343f4104c4fb7d`
- `Tools/HF36Patch/HF36Patch.csproj`: `ae426c7cf72ad3188988a47eb80417965c0a9dde`
- `.github/workflows/build-hf36-patcher.yml`: `1b8bde505bdfb91148b8e5d4382f330a2802b7f1`

The archived source copies were independently re-hashed using Git's blob-object algorithm and matched all four IDs.

Final patcher build head: `7233686d52ee43149c45f76b35f09011809e6ecf`.
Workflow run `34420563912`: PASS.
Published patcher artifact ID `10130723306`, SHA-256 `c34d9d8e78e10dad7edc60cb2c3d6fc107818bb82d0ab6bb1742530f152899d1`.

The published patcher was applied independently twice to the SHA-verified HF35 formal DLL. Both outputs are byte-identical at HF36 SHA `291e22bffefdf98f0fca29475c485264ad5b9eb6cf8a934226d1e73a187ddfb0`; both stderr streams are empty.

Cecil reopen on both outputs: `50 IL / 140 bytes / 1 EH`. The EH is the foreach Enumerator Dispose/finally region. No Cpp2IL helper remains in the target.

## Fixed ILSpy and whole-assembly isolation

ILSpyCmd / ICSharpCode.Decompiler: `11.0.0.9375`.
Reference set: reproduced 56-DLL fixed-Cpp2IL set, ZIP SHA-256 `fe091e5a5389c2491eebeff72c83c394097bef67ffd45a25cf7de01c9aad9ef1`.

Target member readback exits 0 with stderr 0, Cpp2IL refs 0, issue markers 0. The decompiled method exactly reads as the self wake-up gate followed by the zombie-list foreach gate and `Path_Finding()` / `TranToWalk()` calls.

HF35 whole IL was revalidated at SHA-256 `ca155db767d34a814ce322166216aa7c83a868e5d44743c25b71d2c120f22a9e`, stderr 0.
HF36 whole IL SHA-256 is `6a0c79a8c0b758c35d61e7edba8da5740dd72cb82426f976c064ad358b7d5391`, stderr 0.

Before new isolation, the accepted HF34->HF35 semantic diff was regenerated with the identical parser/normalization algorithm and reproduced byte-for-byte at `9c0dacb91256ecadf658ae35faba6474ee5f3f269e7d0f4b2b35b3cfacc22a2e`.

HF35 -> HF36 isolation:

- MethodDef count `2317 -> 2317`;
- whole-IL mapped method blocks `2139 -> 2139`;
- normalized non-method skeleton byte-identical;
- changed MethodDefs: exactly `0x0600048C`;
- semantic diff SHA-256 `83f0cd40ea1d266d2f78972b18ccc177fd77a9b12792109e40a36db9e134250e`.

Normalization is unchanged from the accepted chain: method RVA comments and physical `I_XXXXXXXX` data-address labels are canonicalized; no semantic opcode/type/member text is normalized away.

## Permanent RecoveryAudit

`Tools/RecoveryAudit/Program.cs` was extended with `Zombie.ZC_ArmoredFlagWakeUpZombies/0`, minimum 50 IL.

Permanent RecoveryAudit commit: `623b6ad710577828261b4612b71c2e7fa6e6396b`.
Workflow run `34420936171`: PASS.
Published auditor artifact ID `10130857784`, SHA-256 `163022dd32f85899b857ef96254851f7a5745edb85dc793d9edc6170257f2cff`.

Independent published-auditor execution on the HF36 candidate:

- OPEN1: `320 types / 2317 methods / 2297 bodies`;
- target: `50 IL / 140 bytes`;
- OPEN2: `320 types / 2317 methods / 2297 bodies`;
- target: `50 IL / 140 bytes`;
- stderr 0;
- terminal `RECOVERY_AUDIT_OK`.

## Google Drive pre-closure archive

Folder `HF36-Armored-Flag-Wake-Up-Core`, ID `1PO7YbeLkbQt_Y5uhN3Xnx4Eg6BHu198z`.

Key provider IDs:

- cumulative candidate DLL: `15eQIfAuueLIeWtncXBQWmxldDR3E0fmK`
- patcher: `1M8iC3VU9vaUZpdN53I7h_aCIGqQqzaC6`
- fixed ILSpy: `1fk1qWTa4EYAmWlkpDS40_wBdxfHykfth`
- RecoveryAudit: `13V90m53NH6Xp3j48VNxwozhWZ4FcYjTf`
- semantic diff: `1j1oH-IDWZHWlGC6WmexhJPFb0oyQCZoI`
- Cecil reopen: `19JC1Clqlt85EIZ7l8ToNOVlUPlztpqqr`
- member summary: `1oxZmJMBdU_lCm-a516J_5bKbFDFRZPf-`
- member ZIP: `1rQ3eL_VJFp6mgBjd4johCOQR205ctP9L`
- whole provenance: `1qu09me7tYhc_IyvHQ_daupBTSVh1RZAM`
- MethodDef table: `1yxWcDQx7ZMlGdkCfTMT10JM2lnawTUAj`
- RecoveryAudit log: `1lA31AFrtKdfkjDX-HbXiuy7pq0iuCj13`
- native evidence: `1MGxnOvGAUcIERM1M80re7Tx1fAVr15n-`
- patch source: `1lvY-IEzq7g1kUW1-QmhFy3rYs4uVIhFA`
- formal run1: `1bxVA6EATB6VPhK3n72EiMV8tGixngUwc`
- formal run2: `1dDs0FZxwhedgPQFyPY6Qyi3neK2iKa-c`
- reference provenance: `1v5nup6wlOvIycCLo-pPEjvx0vY2uGgVu`
- semantic isolation: `13atNeFLwYjiUYja-80312iwDbLR_ivX-`
- source/build provenance: `1RhioMmGLB2Cr3AJLm2XGdjDOC2_u090Y`
- target manifest: `1sSN9xQbjlY5JE79JREdbj4PqOGgvOLp7`
- payload manifest: `1rvAnrzZdWkLG6oUBQerOamT76BV8PjAt`, SHA-256 `51185e6d4b21ca0be3084c2e80e92232909b450d2560596a1279c7566253a061`.

Provider pre-closure readback returned exactly 20 files = 19 ordinary payloads + payload manifest.

HF36 is not formal until the Evidence-FINAL and SHA256SUMS-FINAL closure pair is uploaded and the provider folder is independently read back as exactly 22 files. STATUS must be advanced only after that final readback.
