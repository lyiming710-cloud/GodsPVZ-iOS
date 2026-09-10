# HF38 — Plant Kill Event Core

Date: 2026-09-10  
Branch: `high-fidelity`

## Scope

HF38 restores exactly two original `Assembly-CSharp` MethodDefs and no others:

- `0x0600037C` / RID 892 — `Plant.KillEvent(Zombie)` — PC `0x1803517F0`
- `0x0600038F` / RID 911 — `Plant.PC_SSI_KillEvent(Zombie)` — PC `0x1803544C0`

Formal input is HF37 cumulative final:

`412e12b39d1107c29a20027a37094e796d0059e0a3b8637d78233599148d0c56`

HF38 candidate/final-to-be-closed:

`25ca8b5afe45930dd39517c2477a96db520d067c067493c334f107201a973b5d`

## Original-native attribution and managed-loss gate

Fixed originals were re-verified:

- PC ZIP SHA-256 `2f08b4e2243e2bd296f3db694b4a2c9656252d76202c80167bdf3340b9f65b48`
- PC `GameAssembly.dll` SHA-256 `9ebd7ca996a5b03fb4a766f7a2502b660d581ddbbb36af4d7bf2f06a211da39d`
- PC `global-metadata.dat` SHA-256 `ad992341add498bd1018ac980f42171e0737b0793bf945c8714ad70a445b36b9`

Both ordinary MethodDefs were attributed by the fixed rule `original RID-1 -> Assembly-CSharp CodeGenModule methodPointers[index]`, yielding PC bodies `0x1803517F0` and `0x1803544C0`. HF37 managed readback retained concrete Cpp2IL/invalid-reconstruction loss in both methods. `KillEvent` is on the formal zombie damage/death path and its StarfruitSwordImmortal branch directly calls `PC_SSI_KillEvent`, so both are materially active.

HF37 formally closed the `Projectile.Initial<T>` overload family before HF38. The previously unknown shared call in `KillEvent` was resolved to `Initial<Plant>/6`; Unity helpers `0x18131F760` and `0x18131F900` were resolved to `Object.op_Equality` and `Object.op_Inequality`.

## Accepted native behavior

`Plant.KillEvent`:

- Peasniper with `order >= 1`: `attackIntervalCountdown *= 0.6f`.
- CherryBlaster with `order >= 1`, non-null zombie and `zombie.ashes == true`: create projectile 28; initialize at `(zombie.fX, zombie.fY + 20f, 0f)` with zero speed/acceleration, `fZ=20f`, `movementTracks=0`, `origin=this`; then `SetDamage(GetDamage(projectile,28,0), camp)`.
- StarfruitSwordImmortal: call `PC_SSI_KillEvent(zombie)`.

`Plant.PC_SSI_KillEvent`:

- Return for `order < 1`.
- Find `SSI_Characteristic_Atk`; add `0.15f` while below cap; normal cap `2f`, or `3.5f` while skill ID 2 is ongoing, with post-add clamp.
- With `order >= 3`, create projectile 21 at `zombie.transform.position` with `y += 1200f`; initialize with zero speed/acceleration, `fZ=1200f`, track 6, origin this; `SetDamage(null,camp)`; set `maxLivingTime=7f`; skill ID 3 ongoing multiplies that lifetime by `3f`.

## Deterministic patching and readback

HF38 patcher build head: `ce64aab6c719c4afb990c954cc7d3517499add7c`  
Workflow run: `34425980039` PASS  
Artifact ID: `10132666657`  
Artifact SHA-256: `9ebe61bbef22656e518c63c3d671916f1e6f9832d2f45bb9a430b740f301e0fd`

Two independent applications to the SHA-verified HF37 formal input were byte-identical at:

`25ca8b5afe45930dd39517c2477a96db520d067c067493c334f107201a973b5d`

Both runs had zero stderr. Cecil reopen sizes:

- `0x0600037C`: 70 IL / 189 bytes / 0 EH
- `0x0600038F`: 93 IL / 255 bytes / 0 EH

Fixed ILSpyCmd / ICSharpCode.Decompiler `11.0.0.9375` with the reproduced 56-DLL reference set passed both members with exit 0, stderr 0, Cpp2IL refs 0 and issue markers 0.

## Whole-assembly semantic isolation

HF37 whole IL was reproduced at:

`dd045b66df4989f3e617ec2b4ee7f8f398dbba28b9305b31433de5314cffe655`

HF38 whole IL SHA-256:

`a6a49a269cf0ef7f45094c0a7d329c0a076d6f42cb651d00a8d9019f87efe836`

Before computing the new diff, the same parser/normalization algorithm reproduced the accepted HF36->HF37 semantic diff byte-for-byte at:

`125ca6d7ec2e0322056bc7014bae7aed911a9953973e804009cc8ae8d6c35d0b`

HF37->HF38 results:

- MethodDef `2317 -> 2317`
- whole-IL method blocks `2139 -> 2139`
- normalized non-method skeleton byte-identical
- changed MethodDefs exactly `0x0600037C`, `0x0600038F`
- semantic diff SHA-256 `c4e5311c70cefb2da5c5d14696df0b7383cc99dab8479f833cc6c763254ec0be`

## Permanent RecoveryAudit

RecoveryAudit commit: `bd7497de3bc2c890b39146b8171fcb03e20ebc32`  
Workflow run: `34426647292` PASS  
Published auditor artifact ID: `10132916876`  
Artifact SHA-256: `d4f423f3b1755115bc852de69e51bf6173e5c6fd16943042e9994a2fa7af19b8`

Independent published-auditor execution passed the entire retained audit set twice and ended in `RECOVERY_AUDIT_OK`. New targets were identical in OPEN1/OPEN2 at 70/189 and 93/255; stderr was zero.

## Source provenance

Published patcher source is pinned to immutable head `ce64aab6c719c4afb990c954cc7d3517499add7c`:

- `Template.cs` blob `1c63ff06b8af349fccfad2560589ada255814cd3`
- `Program.cs` blob `2e47760bfff20eba195bc34f1e55b15fa098fc27`
- `HF38Patch.csproj` blob `7a08570da0c591b29e743733dcf6cf85a8718e2d`
- workflow blob `9a43bb0b2f75a813b9443cd60ce77c8be81bc9f1`

All four archived files were independently re-hashed as Git blob objects and matched those IDs.

## Drive pre-closure archive

Folder: `HF38-Plant-Kill-Event-Core`  
Folder ID: `1PbrPjBPT-EbBT9p78aroQHrgLxItcDni`

Key provider IDs:

- cumulative DLL `1S2Sx9PknvfoCs6nUoxECIN6AuFmvnHjP`
- patcher `1yTVTspbE6ptL5CtEarf6CRbCChylGfYI`
- fixed ILSpy `1QLfnwctD6CU2P_6huFwF0KHtZ-94ckVe`
- RecoveryAudit `1n8TbdOEpEMFMtavbcWS6GQbSu7PRRB37`
- semantic diff `1Dw9ApopPcrcdgbXV56upMUrT2IheGDYh`
- native evidence `1hBwQ1PsYmihRPbmjAIGFl4-INqQlzasz`
- patch source `1-mSzsUnUwyO0lufK1zw_8RYV__iUQxpo`
- payload manifest `1xN_ue6hFX_QEih5yFp0auTGyphKutORL`

Payload manifest SHA-256: `327912ce43294ca36ce363aa308be171953585197252663781f3e55f066ea9e1`.

Provider readback returned exactly 20 pre-closure files. HF38 is not formal until Evidence-FINAL and SHA256SUMS-FINAL are added, the folder is read back as exactly 22 files, and `Recovery/STATUS.md` is advanced only afterward.
