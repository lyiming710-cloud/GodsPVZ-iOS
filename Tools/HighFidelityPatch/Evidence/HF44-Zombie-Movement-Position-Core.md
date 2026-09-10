# HF44 — Zombie Movement Position Core

Date: 2026-09-10  
Branch: `high-fidelity`

## Scope

HF44 restores exactly three original `Assembly-CSharp` MethodDefs and no others:

- `0x06000472` — RID 1138 — `Zombie.ResetMoveSpeed()` — PC `0x180366D90`
- `0x06000474` — RID 1140 — `Zombie.ResetUpdateRate(float)` — PC `0x180366E40`
- `0x06000481` — RID 1153 — `Zombie.TestPosition(float,float)` — PC `0x180369FC0`

Formal input is the re-fetched HF43 cumulative final:

`c5d998c691f5e32edb5bf48f7d5da3d9ed03f362cb049c3fa3ed8a11617ee078`

Accepted HF44 cumulative candidate:

`fe5e73abead459121ea00c0f91be7e7abc8b3392eb924458b9c0c0352b444118`

This file records an accepted candidate only. HF44 is not formal until Drive 20+2 closure is provider-read back as exactly 22 files and `Recovery/STATUS.md` is advanced afterward.

## Fresh residual scan and native attribution

HF44 was not opened automatically after HF43. A fresh post-HF43 scan confirmed concrete managed loss and live gameplay reachability in `Zombie.ResetUpdateRate(float)` and `Zombie.TestPosition(float,float)`. `ResetUpdateRate` depends on `ResetIdleSpeed`, `ResetAttackSpeed`, and `ResetMoveSpeed`; the first two retained helpers were healthy, while `ResetMoveSpeed` itself contained a concrete floating comparison reconstruction loss and was therefore native-closed and included in this scope.

The fixed original PC baseline was used directly:

- PC ZIP SHA-256 `2f08b4e2243e2bd296f3db694b4a2c9656252d76202c80167bdf3340b9f65b48`
- PC `GameAssembly.dll` SHA-256 `9ebd7ca996a5b03fb4a766f7a2502b660d581ddbbb36af4d7bf2f06a211da39d`
- PC `global-metadata.dat` SHA-256 `ad992341add498bd1018ac980f42171e0737b0793bf945c8714ad70a445b36b9`
- PC CodeRegistration `0x1815E88C0`

Ordinary attribution used original MethodDef RID-1 into the original `Assembly-CSharp.dll` CodeGenModule methodPointers table. The resolved pointers are `methodPointers[1137]=0x180366D90`, `methodPointers[1139]=0x180366E40`, and `methodPointers[1152]=0x180369FC0`. Patched-DLL ordering and Cpp2IL executable-body ordering were not used.

Original `.pdata` boundaries used for disassembly are:

- `ResetMoveSpeed`: `[0x180366D90, 0x180366E40)`
- `ResetUpdateRate`: `[0x180366E40, 0x180367038)`
- `TestPosition`: `[0x180369FC0, 0x18036A362)`

## Accepted native behavior

### `Zombie.ResetMoveSpeed()`

The original PC routine gets `GetMS()`. For ID 17 only, ordered `GetMoveDirection().x < 0f` negates the move speed. Native COMISS/JBE means NaN/unordered does not take the negative branch. The resulting value is written directly to Animator `Ani_MoveSpeedHash`; no extra Animator truthiness guard was introduced.

### `Zombie.ResetUpdateRate(float)`

The original routine first applies Unity Animator object truthiness. With a live Animator and ordered old `updateRate == +0.0/-0.0`, it stores the new rate and calls `ResetIdleSpeed()`, `ResetAttackSpeed()`, then `ResetMoveSpeed()` in that order. With nonzero or unordered/NaN old rate, it rescales existing idle, attack, and move Animator floats by `newUpdateRate / updateRate`. The final `updateRate = newUpdateRate` store occurs after either path. No NaN/divide behavior was normalized.

All three initialization helpers are closed for this path. `ResetMoveSpeed()` is included in HF44; retained `ResetIdleSpeed()` and `ResetAttackSpeed()` require no additional patch.

### `Zombie.TestPosition(float,float)`

The original routine converts candidate coordinates through `BoardConfig.GetGridX/GetGridY` and returns when the resulting cell is unchanged. It computes signed grid deltas and `Math.Sign` values, then obtains the attempted Grid through `Board.GetGrid`.

`Grid` is a plain managed reference and uses ordinary null semantics. If the Grid is passable (`passablePoint >= grid.GetPassablePoint()`), the new grid coordinates are accepted; ID 13 invokes `ZC_SnowbeastImpact`; movement toward negative X searches `OccupyState.Ladder` (enum value 4), and the returned Device uses Unity Object truthiness before `DC_LadderClimb(this)`.

For a blocked non-null Grid, the routine gets current and attempted grid-center positions. A nonzero X delta sets `this.fX = (nextCenter.x + currentCenter.x) * 0.5f - signX * 3f`; a nonzero Y delta sets `this.fY = (nextCenter.y + currentCenter.y) * 0.5f + signY * 3f`. Native unordered branches are preserved. It then gets `transform.position`, preserves Z, writes the original candidate input X/Y to Transform position, and for ID 13 invokes `ZC_SnowbeastHitWall()`. If Grid is null, only the grid coordinates are accepted.

No defensive iOS null guards or guessed movement corrections were added.

## Deliberately excluded residual candidates

`EnemyManager.TimeUpdate()` remains deferred because its huge-wave/final-particle paths still carry broader Board/state dependencies requiring separate native closure. `Zombie.SetUpdateRate()` is not included merely because adjacent rate helpers are damaged; fresh reachability/closure must independently satisfy the later-stage gate. HF44 does not authorize automatic HF45 creation.

## Published patcher and deterministic patching

Published patcher head:

`33ef6bc3e9d5b1dcd92f4fee970336f4ce503cdc`

Workflow run: `34484120971` PASS  
Artifact ID: `10154861901`  
Artifact ZIP SHA-256:

`f4c3fa9f4f4e992e4721ebc4480e86dd67382bf68fdb03b6636476b7c73d17eb`

After the artifact was published, HF43 formal cumulative DLL Drive ID `15pAzAORNoxbQamjmj4xotoQ82RjaeirI` was re-fetched and SHA-verified at `c5d998c691f5e32edb5bf48f7d5da3d9ed03f362cb049c3fa3ed8a11617ee078`.

Two independent applications of the published patcher completed with exit 0 and zero stderr. Outputs are byte-identical at:

`fe5e73abead459121ea00c0f91be7e7abc8b3392eb924458b9c0c0352b444118`

Cecil reopen:

- `0x06000472 Zombie.ResetMoveSpeed/0` — `22 IL / 61 bytes / 0 EH`
- `0x06000474 Zombie.ResetUpdateRate/1` — `67 IL / 196 bytes / 0 EH`
- `0x06000481 Zombie.TestPosition/2` — `161 IL / 408 bytes / 0 EH`
- all targets exceed locked reopen floors
- no Cpp2IL helper remains in any target

## Fixed ILSpy member gate

Fixed ILSpyCmd / ICSharpCode.Decompiler `11.0.0.9375` with exactly 56 fixed references was used. Zombie type decompilation completed with exit 0 and zero stderr. All three target blocks contain zero Cpp2IL references, zero NotImplemented/decompiler-issue markers, zero invalid stack/type/comparison markers, and zero warning/error markers.

Readback preserves the ID17 ordered-negative direction test, Animator truthiness and three-rate rescaling, Grid ordinary-null versus Device Unity-Object semantics, Ladder lookup, midpoint ±3 blocked-grid correction, Transform input-coordinate writeback, and ID13 Snowbeast calls.

## Whole-assembly semantic isolation

Using the same fixed ILSpy toolchain:

- HF42 whole IL reproduced `ad4b9a6ce967456c40b04b258b02eb7470ec8de747d8d077d13ec135007587ed`
- HF43 whole IL reproduced `26da408a5fe00508522d3a784338c4c50577ab1a78226d3c1929886e42760743`
- HF44 whole IL SHA-256 `dbb10be399d1672a0927510a2fb05481783aabdfd56c9eb921d2bb9982ce2b1f`
- all whole-assembly decompiles exit 0 with zero stderr

Before HF43->HF44 isolation, the retained normalization/parser reproduced the Drive-archived HF42->HF43 semantic diff byte-for-byte at `4538f058ada84cf46c899cf9c85bc07db7c1a49780720e6d4298ea2f9d3e0f91`.

HF43->HF44 results:

- MethodDef `2317 -> 2317`
- distinct method-body RVAs `2139 -> 2139`
- normalized non-method skeleton byte-identical
- normalized skeleton SHA-256 both sides `afc47c7eaa314ef12143a7ac99ab0db9fcb484b045d76b2fc74d6f2c93ac3b88`
- changed exactly `0x06000472`, `0x06000474`, `0x06000481`
- HF43->HF44 semantic diff SHA-256 `c79688ce7518eb8fd5fe0cc697d2569ff39b4a189c004ceacd9460a09c430294`
- canonical HF43 MethodDef table reproduced byte-for-byte at `b856427fdef2d77350ab805f1aaa0fab35d77bfb8a7bd1d0fb22e02829869149`
- HF44 MethodDef table SHA-256 `ab1557de0172766a51cc30dd7edae12d5a0cd29551cfb659a0e3641571afc790`

No fourth MethodDef changed.

## Permanent RecoveryAudit

RecoveryAudit commit:

`fec4f209eadb00d4620983c93b88e0d107baba72`

Workflow run: `34484770234` PASS  
Published auditor artifact ID: `10155135509`  
Auditor artifact ZIP SHA-256:

`075dfd1a1a84f779e074c0da64cd58bcd301f8a7da15d0a1eb79db7fd33e94b8`

The published auditor was executed independently twice against the accepted HF44 candidate. Both process runs completed with exit 0 and zero stderr. Each process performed internal OPEN1 and OPEN2 over the complete retained historical target set and ended with `RECOVERY_AUDIT_OK`.

HF44 readback in OPEN1 and OPEN2 was stable at:

- `Zombie.ResetMoveSpeed/0` — `22 IL / 61 bytes`
- `Zombie.ResetUpdateRate/1` — `67 IL / 196 bytes`
- `Zombie.TestPosition/2` — `161 IL / 408 bytes`

## Source provenance

Published HF44 patcher source is pinned to immutable build head `33ef6bc3e9d5b1dcd92f4fee970336f4ce503cdc`:

- `HF44Patch.csproj` blob `bb80cf3e0814e87a00abb1882332d3706339ca9f`
- `Program.cs` blob `5125708e91eb660cf80c90afced3b3f07a7fefc3`
- `Template.cs` blob `b18b035f069a20b3853e11315131b4d759a2a5d8`
- workflow blob `ef8dfda5be5f1e8039b13715c6873e72815b1170`

RecoveryAudit source blob after HF44 extension is `e70ce75d861f7991d028e5766b9763cbbf966f9d`.

## Closure state

All gates through permanent published RecoveryAudit are PASS. Remaining formal steps are GitHub Evidence retention, strict Drive 19 ordinary payload + payload manifest pre-closure readback, the two final closure files, exact 22-file final provider readback, and only then `Recovery/STATUS.md` advancement. Until those complete, HF43 remains the only formal cumulative final and no later HF stage may consume the HF44 candidate.
