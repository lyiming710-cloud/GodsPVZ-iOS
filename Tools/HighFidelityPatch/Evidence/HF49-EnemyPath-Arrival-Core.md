# HF49 — EnemyPath Arrival Core

Date: 2026-09-11  
Branch: `high-fidelity`

## Formal input

HF48 Ladder Runtime Core SHA-256:

`6c1eb1468f398610cc61ea89b7f3a9409f6baeb451e6df0ae6243b28af50e48e`

HF49 restores exactly one MethodDef:

- `0x060002A3` / RID 675 / `EnemyPath.ArrivalTest(Zombie)` / original PC entry `0x180329620`.

No other MethodDef is authorized in this stage.

## Why this method passes the gate

The HF48 managed body has concrete reconstruction loss: invalid object/float operations, unmanaged-memory placeholder logic and broken null-failure reconstruction. The method is materially gameplay reachable: `Zombie.Path_Test()` calls `EnemyPath.ArrivalTest(Zombie)` twice, and `Zombie.Update()` directly calls `Path_Test()` from the active Unity lifecycle movement loop.

The original PC native behavior is closed from the locked 1.0.2 baseline (`GameAssembly.dll` SHA `9ebd7ca996a5b03fb4a766f7a2502b660d581ddbbb36af4d7bf2f06a211da39d`, metadata SHA `ad992341add498bd1018ac980f42171e0737b0793bf945c8714ad70a445b36b9`). Native execution dereferences `father.board.boardConfig`, calls `GetZombiePosition(gridX,gridY)`, reads `father.transform.position`, and returns true when both absolute X/Y deltas are strictly below `Zombie.deadzone_distance`. Otherwise, when `original` is true, it returns true only if both the associated `zombie` and `plant` compare Unity-null through `UnityEngine.Object` equality. It returns false otherwise. Native null failure semantics are retained; no defensive fallback or iOS-specific guard is added.

`Zombie.Path_Test()` remains outside HF49. Its original PC body at `0x180366030` is materially larger and still requires an independent dependency closure. HF49 does not claim to repair the whole path-selection state machine.

## Published patcher and deterministic candidate

Patcher source head: `904ba2fad29112b5be9b6ffe5a28047b5f9f0a46`  
Workflow: `34548504310` PASS  
Artifact ID: `10179914173`  
Artifact SHA-256: `eaa26b2547e90c0c1735683f78da439ab549f58ed536c6d5a9fa1d366ab7849a`

The published patcher hard-validates the formal HF48 input SHA and MethodDef total `2317` before mutation. Two independent patch executions produced byte-identical output:

`6dff7975abd2b62d2cc40564a17f21518526d8dae2ce7f53a7be3e49c2114f69`

Both executions exited 0.

## Cecil reopen

`0x060002A3 EnemyPath.ArrivalTest(Zombie)` reopens as:

- 52 IL instructions
- 137 bytes
- 0 exception handlers
- zero `Cpp2ILHelpers` references

Assembly MethodDef total remains `2317`.

## Fixed ILSpy validation

Fixed ILSpyCmd / ICSharpCode.Decompiler: `11.0.0.9375` with the locked 56-DLL reference set.

The HF49 target readback is clean: exit 0, stderr 0, and target-local `Cpp2IL`, `Unknown result type`, `NotImplemented`, invalid comparison and invalid stack/type markers are all zero. The decompiled body reads as the original-PC-native behavior: `GetZombiePosition`, transform position, strict deadzone X/Y comparisons, then Unity-null checks for `original && zombie == null && plant == null`.

HF48 whole IL SHA-256:
`47726f82264e97ecc44a9fdb5a41fcb447c76da57a16cce3c2c20622e7ecb0bd`

HF49 whole IL SHA-256:
`c60eb750651402273ec5cc7ac98520c500dd361568cd2214c42518ed7a737455`

## Whole-assembly semantic isolation

- MethodDef count: `2317 -> 2317`
- emitted method bodies: `2297 -> 2297`
- distinct nonzero body RVAs: `2140 -> 2140`
- normalized non-method/method-signature skeleton: byte-identical
- changed normalized MethodDef blocks: exactly one, `0x060002A3 EnemyPath.ArrivalTest(Zombie)`
- semantic diff SHA-256: `600fa54b571638988192906f1c53c16d38c944dd20d1c4855be7a636bdaac8ca`
- HF49 MethodDef table SHA-256: `8da2a6270cecdba89df7cd269b479a74ad9cb109a2bcf9d63ad65cf36696370b`

## Permanent cumulative RecoveryAudit

Historical HF1-HF46 RecoveryAudit remains unchanged and is paired with a new cumulative HF49 auditor that locks the HF49 candidate SHA and rechecks all HF47+HF48+HF49 late-stage targets.

Audit build head: `4ca357f60140fb947565a3f214237d0804807998`  
Workflow: `34548784207` PASS  
Artifact ID: `10180014456`  
Artifact SHA-256: `9faded8ec48694bf9ed9bf94c0934fd283138d48b7dabdcd995d4bb2edadc58d`

Two independent actual executions against the same HF49 candidate passed. Each historical run ended `RECOVERY_AUDIT_OK`; each cumulative late-stage run performed OPEN1/OPEN2 over 10 HF47-HF49 targets and ended `HF49_AUDIT_OK`; stderr was zero. The complete audit logs were byte-identical, SHA-256 `cdb51adbb05a842361a07896a96078d479708ce5788064bd62c90a8b64dc1011`.

## Fresh residual gate

HF49 cleans `EnemyPath.ArrivalTest` only. `Zombie.Path_Test()` remains the strongest active residual candidate: it is live from `Zombie.Update()` and still contains two `Cpp2ILHelpers.NoteDecompilerIssue` calls plus malformed managed reconstruction, but its native body/dependencies have not yet been fully closed. `EnemyPath.DistanceStatistics(...)` also retains invalid managed reconstruction and has live callers, while `Zombie.Update_Path()` has no callsite in the current cumulative assembly and is not promoted on warning count alone.

A future HF50 may open only after an independent original-PC-native reachability/dependency gate. If no residual candidate passes, managed recovery should stop and the project should proceed to 67/67 package validation, integration of the cumulative DLL, Unity compile/runtime validation and only then iOS adaptation.
