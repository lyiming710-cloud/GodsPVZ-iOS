# HF42 — Dithering Motion Core

Date: 2026-09-10  
Branch: `high-fidelity`

## Scope

HF42 restores exactly three original `Assembly-CSharp` MethodDefs and no others:

- `0x060002AF` — RID 687 — `Board.FixedUpdate()` — PC `0x1803265C0`
- `0x060002B0` — RID 688 — `Board.FixedUpdate_Shake()` — PC `0x1803262A0`
- `0x0600035E` — RID 862 — `Plant.Dithering_Animation(float)` — PC `0x18034E890`

Formal input is the re-fetched HF41 cumulative final:

`2e03010cab5a4776243ecf3509402f387c39ccb9faa039876c85ad629ba7fcfa`

Accepted HF42 cumulative candidate:

`687a973f640fc1be1e092fc75f6d09e697995563c605763d1195751af91dc7d6`

This file records an accepted candidate only. HF42 is not formal until the Drive 20+2 closure is provider-read back as exactly 22 files and `Recovery/STATUS.md` is advanced afterward.

## Fresh residual scan and original-native attribution

HF42 was not opened automatically after HF41. A fresh residual active-path scan first identified concrete managed reconstruction loss on current gameplay paths. `Board.FixedUpdate()` directly gates shake processing, `Board.FixedUpdate_Shake()` is reached while `shakeTime > 0`, and `Plant.Dithering_Animation(float)` is called from retained Plant fixed-update paths for the active ID/state combination. The existing Cpp2IL-derived bodies contained invalid float/type comparison or lost vector/square-root operations, so these were concrete managed losses rather than warning-count triage.

The fixed original PC baseline was used for attribution:

- PC ZIP SHA-256 `2f08b4e2243e2bd296f3db694b4a2c9656252d76202c80167bdf3340b9f65b48`
- PC `GameAssembly.dll` SHA-256 `9ebd7ca996a5b03fb4a766f7a2502b660d581ddbbb36af4d7bf2f06a211da39d`
- PC `global-metadata.dat` SHA-256 `ad992341add498bd1018ac980f42171e0737b0793bf945c8714ad70a445b36b9`
- PC CodeRegistration `0x1815E88C0`

Ordinary attribution used original MethodDef RID-1 into the original `Assembly-CSharp.dll` CodeGenModule methodPointers table. Patched-DLL MethodDef ordering and Cpp2IL executable-body ordering were not used.

## Accepted native behavior

### `Board.FixedUpdate()`

PC `0x1803265C0` loads `shakeTime`, compares it against +0.0 with `COMISS`, and calls/tail-jumps to `Board.FixedUpdate_Shake()` only for ordered `shakeTime > 0`. Zero, negative, and unordered/NaN return without calling the helper. The accepted CIL expresses the same `shakeTime > 0f` condition; managed floating comparison therefore preserves the native unordered false result.

### `Board.FixedUpdate_Shake()`

PC `0x1803262A0` first subtracts `Time.fixedDeltaTime` from `shakeTime`. It resolves `Camera.main.transform`, reads the current position, subtracts the previously stored `dithering` x/y/z, and writes that position back.

The native inner comparison is effectively `0 >= shakeTime` for the stop branch. Unordered/NaN does not take that stop branch, so the accepted CIL uses `!(0f >= shakeTime)` rather than replacing it with a superficially equivalent ordered-only expression.

On the continuing branch the native routine obtains `UnityEngine.Random.insideUnitCircle`, computes `sqrt(x*x + y*y)`, compares the magnitude with the original single-precision threshold `1e-5f`, normalizes x/y when the magnitude is above that threshold and otherwise uses zero, multiplies the direction by `amplitude`, stores z=0, assigns this vector to `dithering`, and adds it to `Camera.main.transform.position`.

It then updates amplitude as:

`(1f - Time.fixedDeltaTime / Time.fixedTime * 0.05f) * amplitude`

The original PC constants resolve to 1.0f and 0.05f. On the stop branch it sets `amplitude = 0f` and `dithering = Vector3(0,0,0)`. Original Unity/null/exception behavior is retained; no defensive iOS-oriented guard was added.

### `Plant.Dithering_Animation(float)`

PC `0x18034E890` starts with a zero new offset. Its amplitude comparison uses native unordered behavior such that ordered +0.0/-0.0 skips random generation while nonzero and unordered/NaN enter the random path; C# `amplitude != 0f` preserves that behavior.

The random path obtains `Random.insideUnitCircle`, computes the same `sqrt(x*x + y*y)` magnitude, applies the exact 1e-5f threshold, normalizes x/y or uses zero, then multiplies x/y by `amplitude` with z=0.

The native routine next resolves `animationGroup.transform.position`, subtracts the prior `dithering_anim`, writes the position, obtains position again, adds the new offset, writes it, and finally stores the new vector in `dithering_anim`. The accepted CIL retains this order and the original null/exception behavior.

All project dependencies required by these three bodies are closed: they use existing Board/Plant fields plus Unity `Camera`, `Transform`, `Random`, `Time`, Vector2/Vector3 operations, and `System.Math.Sqrt`. No unexplained project-native helper is removed or guessed.

## Residual candidates deliberately excluded

The fresh scan also found active-path damage in `EnemyManager.TimeUpdate()`, `Zombie.GetRandenAnimationSpeedMagnification()`, `Zombie.TestPosition(float,float)`, and `Plant.SetUpdateRate()`. They were not bundled into HF42. In particular, `EnemyManager.TimeUpdate()` still has an unresolved Board-associated data dependency in the huge-wave/final particle-scale path, so it remains non-closed and cannot be formally patched by inference. These candidates must be reconsidered in a fresh scan only after HF42 formal closure.

## Deterministic patching

Published HF42 patcher build head:

`3fb55af85182896824b3743fddda40f6fe8d5100`

Workflow run: `34476087441` PASS  
Artifact ID: `10151538241`  
Artifact ZIP SHA-256:

`c4910c871d83828555e993ff3c8484928e2464f7206e9811222d9b987f80ade1`

The patcher hard-locks formal input SHA-256 to HF41 `2e03010cab5a4776243ecf3509402f387c39ccb9faa039876c85ad629ba7fcfa` and hard-locks the three target tokens.

After the artifact had been published, the HF41 formal cumulative DLL was re-fetched from its Drive final and SHA-verified again. Two independent applications of the published patcher to independent copies completed with exit 0 and zero stderr. Outputs are byte-identical at:

`687a973f640fc1be1e092fc75f6d09e697995563c605763d1195751af91dc7d6`

Cecil reopen:

- `0x060002AF Board.FixedUpdate/0` — `7 IL / 20 bytes / 0 EH`
- `0x060002B0 Board.FixedUpdate_Shake/0` — `130 IL / 391 bytes / 0 EH`
- `0x0600035E Plant.Dithering_Animation/1` — `108 IL / 295 bytes / 0 EH`
- all three exceed their locked reopen floors
- no Cpp2IL helper remains in any target

## Fixed ILSpy member gate

Fixed toolchain:

- ILSpyCmd / ICSharpCode.Decompiler `11.0.0.9375`
- fixed ILSpy archive SHA-256 `446bb884aa014ae04d6825ad6430e85dc1a827d2526075bbde754c9f85b3a1da`
- fixed 56-DLL reference ZIP SHA-256 `fe091e5a5389c2491eebeff72c83c394097bef67ffd45a25cf7de01c9aad9ef1`
- exactly 56 reference DLLs

Board and Plant type decompilation completed with exit 0 and zero stderr. The three targets contain zero Cpp2IL references, zero NotImplemented/decompiler-issue markers, zero invalid stack/type/comparison markers, and zero warning/error markers. Readback retains the ordered `shakeTime > 0f` gate, the unordered-preserving `!(0f >= shakeTime)` inner condition, 1e-5 magnitude guard, random vector normalization, previous-offset subtraction, new-offset addition, and amplitude decay.

## Whole-assembly semantic isolation

Using the same fixed ILSpy toolchain:

- HF40 whole IL reproduced exactly at `fd43ae98ca25cc7a43f5265c76a924eaefb8fa8b4616575c6af914935fd0ff7e`
- HF41 whole IL reproduced exactly at `48183cd6963c3bde3056e73d8983eeceea36574199b881077f76091bd458b4c0`
- HF42 whole IL SHA-256 is `ad4b9a6ce967456c40b04b258b02eb7470ec8de747d8d077d13ec135007587ed`
- all whole-assembly decompiles completed with zero stderr

Before computing HF41->HF42 isolation, the retained normalization/parser reproduced the accepted Drive-archived HF40->HF41 semantic diff byte-for-byte at:

`8080c9fa957c43f70073275f44f9e81d5153670c563ca8bcbdf6e8629480873b`

HF41->HF42 results:

- MethodDef `2317 -> 2317`
- distinct method-body RVAs `2139 -> 2139`
- normalized non-method skeleton byte-identical
- normalized skeleton SHA-256 on both sides `afc47c7eaa314ef12143a7ac99ab0db9fcb484b045d76b2fc74d6f2c93ac3b88`
- changed MethodDefs exactly `0x060002AF`, `0x060002B0`, `0x0600035E`
- HF41->HF42 semantic diff SHA-256 `1f321b7604d20bf6aa700310c2ec90c10717be02fb2c5de21ea31a710dfb52b6`
- HF41 MethodDef table was reproduced at its accepted path byte-for-byte at `302fe34407fd0f44774b11d096debaae13b057c52d133d76b4a121d9c2410fb4`
- HF42 MethodDef table SHA-256 `34f45e7c4beb53630472ae0a5cba63db6622029e131f193cbc1dacaec05d3e9b`

No fourth MethodDef changed.

## Permanent RecoveryAudit

RecoveryAudit was extended only after native closure, deterministic patching, Cecil reopen, fixed-ILSpy member readback, whole-assembly continuity, prior-diff reproduction and structural isolation had passed.

RecoveryAudit commit:

`8fc4e7ddf6d3b5d551caf6d989def0f8fb3da3a2`

Workflow run: `34476864538` PASS  
Published auditor artifact ID: `10151865052`  
Auditor artifact ZIP SHA-256:

`e91943cfb5f5dc4692e207702fa3f2fa7c6208b68f6cc849023ac7ced2a5c4d8`

The published auditor was executed independently twice against the accepted HF42 candidate. Both process runs completed with exit 0 and zero stderr. Each process performed internal OPEN1 and OPEN2 over the complete retained historical target set and ended with `RECOVERY_AUDIT_OK`.

HF42 readback in both OPEN1 and OPEN2 was stable at:

- `Board.FixedUpdate/0` — `7 IL / 20 bytes`
- `Board.FixedUpdate_Shake/0` — `130 IL / 391 bytes`
- `Plant.Dithering_Animation/1` — `108 IL / 295 bytes`

## Source provenance

Published HF42 patcher source is pinned to immutable build head `3fb55af85182896824b3743fddda40f6fe8d5100`:

- `HF42Patch.csproj` blob `7e579f6302f3075884db0ae16e027e56bb3e28d7`
- `Program.cs` blob `8638ac56be3984fa967128ef77c75a3ab87453a2`
- `Template.cs` blob `3b13a432855df867774bba453973f07df961888e`
- workflow blob `a94efe8a0b2fe3c5909faa62c699fd15a0fa910b`

RecoveryAudit source blob after HF42 extension is `50146251f2c2bc1fa746dc429d407903ad87d348`.

## Closure state

All gates through permanent published RecoveryAudit are PASS. The remaining formal steps are strict Drive 19 ordinary payload + payload-manifest pre-closure readback, the two final closure files, exact 22-file final provider readback, and only then `Recovery/STATUS.md` advancement. Until those complete, HF41 remains the only formal cumulative final and no later HF stage may consume the HF42 candidate.
