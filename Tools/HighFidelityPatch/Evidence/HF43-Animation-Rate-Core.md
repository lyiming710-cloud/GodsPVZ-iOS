# HF43 — Animation Rate Core

Date: 2026-09-10  
Branch: `high-fidelity`

## Scope

HF43 restores exactly three original `Assembly-CSharp` MethodDefs and no others:

- `0x0600039E` — RID 926 — `Plant.ResetUpdateRate(float)` — PC `0x180356970`
- `0x060003A7` — RID 935 — `Plant.SetUpdateRate()` — PC `0x180359430`
- `0x06000451` — RID 1105 — `Zombie.GetRandenAnimationSpeedMagnification()` — PC `0x180361870`

Formal input is the re-fetched HF42 cumulative final:

`687a973f640fc1be1e092fc75f6d09e697995563c605763d1195751af91dc7d6`

Accepted HF43 cumulative candidate:

`c5d998c691f5e32edb5bf48f7d5da3d9ed03f362cb049c3fa3ed8a11617ee078`

This file records an accepted candidate only. HF43 is not formal until the Drive 20+2 closure is provider-read back as exactly 22 files and `Recovery/STATUS.md` is advanced afterward.

## Fresh residual scan and native attribution

HF43 was not opened automatically after HF42. A fresh residual active-path scan against the HF42 cumulative assembly confirmed concrete managed reconstruction loss in the three methods above. `Plant.SetUpdateRate()` and `Plant.ResetUpdateRate(float)` participate in retained Plant update/animation-rate paths. `Zombie.GetRandenAnimationSpeedMagnification()` participates in retained Zombie initialization/animation speed setup and its original jump-table logic was lost by the managed reconstruction.

The fixed original PC baseline was used for attribution:

- PC ZIP SHA-256 `2f08b4e2243e2bd296f3db694b4a2c9656252d76202c80167bdf3340b9f65b48`
- PC `GameAssembly.dll` SHA-256 `9ebd7ca996a5b03fb4a766f7a2502b660d581ddbbb36af4d7bf2f06a211da39d`
- PC `global-metadata.dat` SHA-256 `ad992341add498bd1018ac980f42171e0737b0793bf945c8714ad70a445b36b9`
- PC CodeRegistration `0x1815E88C0`

Ordinary attribution used original MethodDef RID-1 into the original `Assembly-CSharp.dll` CodeGenModule methodPointers table. Patched-DLL MethodDef ordering and Cpp2IL executable-body ordering were not used.

## Accepted native behavior

### `Plant.ResetUpdateRate(float)`

The original PC routine first applies Unity `Animator` object truthiness. If the animator is absent/destroyed, it skips all animator access and still stores `updateRate = newUpdateRate` at the end.

With a live animator, original `updateRate == +0.0/-0.0` takes the initialization path: it stores `newUpdateRate`, computes `Math.Max(buffManager.GetIncrement(1f, "Is") + 1f, 0.05f) * updateRate`, writes `Ani_SpeedHash`, and writes `Ani_AttackSpeedHash` from `GetAS()`.

For nonzero or unordered/NaN old `updateRate`, it reads the existing speed and attack-speed animator floats and rescales both by `newUpdateRate / updateRate`. The final field assignment to `updateRate` occurs after either path. No defensive divide-by-zero/NaN normalization was introduced.

### `Plant.SetUpdateRate()`

The original routine gets the `fire_ice` element and starts with a target rate of `1f`. The elemental slowdown path requires a non-null plain Element reference, `ID != 5`, and ordered `element.point > 0f`.

If `burstTime > 0f`, then when `updateRate != 0f` and the Animator is live, the routine reads the current animation speed, obtains `GetAS()`, computes the original `0f / updateRate` factors, and writes speed/attack-speed through those factors. It then stores `updateRate = 0f` and returns. The `0f / updateRate` expression is retained intentionally because NaN propagation is part of the original floating behavior.

Outside burst, ID 6 is exempt from the elemental rate reduction. Other eligible IDs compute `MathF.Ceiling(element.point / 1000f) * 0.05f` and then `Math.Max(0.05f, 1f - scaled)`. If current `updateRate != targetRate`, the routine calls `ResetUpdateRate(targetRate)`, then stores the target rate.

### `Zombie.GetRandenAnimationSpeedMagnification()`

The original PC two-level jump tables were decoded directly. Zombie IDs `{0,2,4,5,6,7,8,9,11,12,14}` call `UnityEngine.Random.Range(0.75f, 1.3f)` and return that result. All other IDs return exactly `1f`.

The recovered method preserves that exact ID set rather than inferring categories from nearby IDs or names.

All project dependencies required by the three methods are closed: retained Plant fields/methods, `ElementManager.GetElement`, `BuffManager.GetIncrement`, `Plant.GetAS`, Unity Animator object semantics, `Animator.GetFloat/SetFloat`, `Math/MathF`, and `UnityEngine.Random.Range`. No unexplained project-native helper was deleted or replaced by guessed gameplay logic.

## Deliberately excluded residual candidates

`EnemyManager.TimeUpdate()` and `Zombie.TestPosition(float,float)` remain outside HF43. `EnemyManager.TimeUpdate()` still carries broader huge-wave/final-particle state dependencies that require separate native closure; `Zombie.TestPosition` carries additional position/fallback and floating-branch behavior. Their existence does not authorize automatic HF44 creation. They must be reconsidered only by a fresh scan after HF43 formal closure.

## Published patcher and deterministic patching

HF43 v1 patcher head `eaafbbefcecf0c5a6489796ace5408c54b827b83`, workflow `34479347287`, published successfully, but its first formal application was rejected by the patcher's own reopen gate because `Zombie.GetRandenAnimationSpeedMagnification()` compiled to 12 IL while the conservative reopen floor had been set to 15. No candidate from v1 was accepted or propagated. Diagnostic fixed-ILSpy readback showed the intended native-backed body was intact, so gameplay semantics were not changed to satisfy the tool.

The only v2 source change lowered that method's reopen floor to 10, consistent with the compiled switch body. Gameplay Template semantics were unchanged.

Published HF43 v2 patcher head:

`25ada471bf94f96dc5abc7affdcd6e616e02767d`

Workflow run: `34479568604` PASS  
Artifact ID: `10152984471`  
Artifact ZIP SHA-256:

`b7193f7f6dc11f86ae433c3db0ab2a04670e0893b70c59f48fbe6f0ad0034b03`

The patcher hard-locks formal input SHA-256 to HF42 `687a973f640fc1be1e092fc75f6d09e697995563c605763d1195751af91dc7d6` and hard-locks the three target tokens.

After the v2 artifact had been published, the HF42 formal cumulative DLL was re-fetched from its Drive final and SHA-verified again. Two independent applications of the published v2 patcher to independent copies completed with exit 0 and zero stderr. Outputs are byte-identical at:

`c5d998c691f5e32edb5bf48f7d5da3d9ed03f362cb049c3fa3ed8a11617ee078`

Cecil reopen:

- `0x0600039E Plant.ResetUpdateRate/1` — `70 IL / 216 bytes / 0 EH`
- `0x060003A7 Plant.SetUpdateRate/0` — `94 IL / 276 bytes / 0 EH`
- `0x06000451 Zombie.GetRandenAnimationSpeedMagnification/0` — `12 IL / 97 bytes / 0 EH`
- all three exceed their v2 locked reopen floors
- no Cpp2IL helper remains in any target

## Fixed ILSpy member gate

Fixed toolchain:

- ILSpyCmd / ICSharpCode.Decompiler `11.0.0.9375`
- fixed ILSpy archive SHA-256 `446bb884aa014ae04d6825ad6430e85dc1a827d2526075bbde754c9f85b3a1da`
- fixed 56-DLL reference ZIP SHA-256 `fe091e5a5389c2491eebeff72c83c394097bef67ffd45a25cf7de01c9aad9ef1`
- exactly 56 reference DLLs

Plant and Zombie type decompilation completed with exit 0 and zero stderr. The three targets contain zero Cpp2IL references, zero NotImplemented/decompiler-issue markers, zero invalid stack/type/comparison markers, and zero warning/error markers. Readback retains Animator truthiness, the `0f / updateRate` behavior, Ceiling/Max rate scaling, and the exact randomized Zombie ID switch set.

## Whole-assembly semantic isolation

Using the same fixed ILSpy toolchain:

- HF41 whole IL reproduced exactly at `48183cd6963c3bde3056e73d8983eeceea36574199b881077f76091bd458b4c0`
- HF42 whole IL reproduced exactly at `ad4b9a6ce967456c40b04b258b02eb7470ec8de747d8d077d13ec135007587ed`
- HF43 whole IL SHA-256 is `26da408a5fe00508522d3a784338c4c50577ab1a78226d3c1929886e42760743`
- all whole-assembly decompiles completed with zero stderr

Before computing HF42->HF43 isolation, the retained normalization/parser reproduced the Drive-archived HF41->HF42 semantic diff byte-for-byte at:

`1f321b7604d20bf6aa700310c2ec90c10717be02fb2c5de21ea31a710dfb52b6`

HF42->HF43 results:

- MethodDef `2317 -> 2317`
- distinct method-body RVAs `2139 -> 2139`
- normalized non-method skeleton byte-identical
- normalized skeleton SHA-256 on both sides `afc47c7eaa314ef12143a7ac99ab0db9fcb484b045d76b2fc74d6f2c93ac3b88`
- changed MethodDefs exactly `0x0600039E`, `0x060003A7`, `0x06000451`
- HF42->HF43 semantic diff SHA-256 `4538f058ada84cf46c899cf9c85bc07db7c1a49780720e6d4298ea2f9d3e0f91`
- HF42 MethodDef table was reproduced at its accepted canonical path byte-for-byte at `34f45e7c4beb53630472ae0a5cba63db6622029e131f193cbc1dacaec05d3e9b`
- HF43 MethodDef table SHA-256 `b856427fdef2d77350ab805f1aaa0fab35d77bfb8a7bd1d0fb22e02829869149`

No fourth MethodDef changed.

## Permanent RecoveryAudit

RecoveryAudit was extended only after native closure, deterministic v2 patching, Cecil reopen, fixed-ILSpy member readback, whole-assembly continuity, prior-diff reproduction and structural isolation had passed.

RecoveryAudit commit:

`fde3f301afd31ad0cd5588ea7a8dee36758f4ebb`

Workflow run: `34480261357` PASS  
Published auditor artifact ID: `10153273106`  
Auditor artifact ZIP SHA-256:

`b200e4a830eb274d333415d396207398ad2642dffc77446e853ec71fcf3ae116`

The published auditor was executed independently twice against the accepted HF43 candidate. Both process runs completed with exit 0 and zero stderr. Each process performed internal OPEN1 and OPEN2 over the complete retained historical target set and ended with `RECOVERY_AUDIT_OK`.

HF43 readback in both OPEN1 and OPEN2 was stable at:

- `Plant.ResetUpdateRate/1` — `70 IL / 216 bytes`
- `Plant.SetUpdateRate/0` — `94 IL / 276 bytes`
- `Zombie.GetRandenAnimationSpeedMagnification/0` — `12 IL / 97 bytes`

## Source provenance

Published HF43 v2 patcher source is pinned to immutable build head `25ada471bf94f96dc5abc7affdcd6e616e02767d`:

- `HF43Patch.csproj` blob `3f3a9b0bca2af61dfed71922f59b5bc9fb95d75e`
- `Program.cs` blob `fa8b8da7ffe3b4ba824319d1e56f8bc568b56711`
- `Template.cs` blob `074e22b8fb66d5d910348c5ee722120ce5af291c`
- workflow blob `45f6f395b680920ec7dcbddb2b8fab67cb0cc555`

RecoveryAudit source blob after HF43 extension is `e3f126696c1612673d8d3471c72ab90636e018ef`.

## Closure state

All gates through permanent published RecoveryAudit are PASS. The remaining formal steps are strict Drive 19 ordinary payload + payload-manifest pre-closure readback, the two final closure files, exact 22-file final provider readback, and only then `Recovery/STATUS.md` advancement. Until those complete, HF42 remains the only formal cumulative final and no later HF stage may consume the HF43 candidate.
