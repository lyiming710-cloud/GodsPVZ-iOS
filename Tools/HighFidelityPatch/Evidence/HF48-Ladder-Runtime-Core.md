# HF48 — Ladder Runtime Core

Date: 2026-09-11  
Branch: `high-fidelity`

## Scope

HF48 restores exactly six original native-backed MethodDefs and no others:

- `0x060001B3` / RID 435 — `EnemyManager.DispatcheLadderWave()` — PC `0x1803170D0`
- `0x060001B4` / RID 436 — `EnemyManager.DispatcheSPHWave()` — PC `0x1803172F0`
- `0x06000273` / RID 627 — `ZombieManager.Update()` — PC `0x180342AC0`
- `0x06000274` / RID 628 — `ZombieManager.Update_Ladder()` — PC `0x1803427B0`
- `0x0600027D` / RID 637 — `ZombieManager.TriggerLadder()` — PC `0x180342620`
- `0x060002A4` / RID 676 — `EnemyPath..ctor(int,int,float,BoardConfig)` — PC `0x1803299E0`

Formal input is HF47 cumulative final:

`e94942c50cdce952ca37f746bc5c3ab83b49106ae0837a077045f54f6ecb9533`

Accepted HF48 cumulative candidate:

`6c1eb1468f398610cc61ea89b7f3a9409f6baeb451e6df0ae6243b28af50e48e`

This file records an accepted candidate only. HF48 is not formal until strict Google Drive 20+2 closure is provider-read back as exactly 22 files and `Recovery/STATUS.md` is advanced afterward.

## Original-PC attribution

The original PC 1.0.2 baseline was re-fetched from the user's `PVZ GOD` Drive folder and retains the locked hashes:

- PC ZIP SHA-256 `2f08b4e2243e2bd296f3db694b4a2c9656252d76202c80167bdf3340b9f65b48`
- PC `GameAssembly.dll` SHA-256 `9ebd7ca996a5b03fb4a766f7a2502b660d581ddbbb36af4d7bf2f06a211da39d`
- PC `global-metadata.dat` SHA-256 `ad992341add498bd1018ac980f42171e0737b0793bf945c8714ad70a445b36b9`

Ordinary MethodDef attribution uses original RID-1 into the original Assembly-CSharp CodeGenModule methodPointers table, not recovered-DLL adjacency.

Closed native ranges used for the evidence package:

- `DispatcheLadderWave`: `[0x1803170D0,0x1803172F0)`
- `DispatcheSPHWave`: `[0x1803172F0,0x1803175F0)`
- `ZombieManager.TriggerLadder`: `[0x180342620,0x1803427B0)`
- `ZombieManager.Update_Ladder`: `[0x1803427B0,0x180342AC0)`
- `ZombieManager.Update`: `[0x180342AC0,0x180342AD0)`; native body is a direct tail jump to `Update_Ladder`
- `EnemyPath` 4-argument constructor: `[0x1803299E0,0x180329AA0)`

## Accepted behavior

`EnemyManager.DispatcheLadderWave()` enumerates every map row, accepts only `EnemyType == 1`, obtains the current map X and grid position, creates an ID16 Enemy at grid position plus `(57,28,0)`, sets row and `waitingTime=0.02f`, and dispatches it through the already restored `DispatcheZombie` path.

`EnemyManager.DispatcheSPHWave()` similarly enumerates traversable rows, creates ID17 enemies from the native spawn-grid expression, dispatches them, and for a live returned Zombie inserts at `prePath[0]` a new `EnemyPath(board.map.GetMapX()-1,row,999f,board.boardConfig)`. Original Unity-object/list/null behavior is retained.

`ZombieManager.Update()` is restored to the original direct `Update_Ladder()` call rather than an empty managed body.

`ZombieManager.Update_Ladder()` preserves the `gameStart`/`gamePause` gate, active ladder countdown decrement, first-trigger window/pause/audio path, ladder-wave dispatch, `max(15-ladderTimes,5)` countdown reset, increment of `ladderTimes`, ID17 SPH wave at the tenth ladder event, and the final clearing of `ladderTrigger`. No iOS-oriented guards were inserted.

`ZombieManager.TriggerLadder()` preserves the early return when already triggered and the complete Zombie list enumeration/Dispose semantics. It only blocks triggering when a live ID16 ladder-armor Zombie that is not disabled still has a non-empty path; otherwise it sets `ladderTrigger=true`.

`EnemyPath..ctor(int,int,float,BoardConfig)` restores `waitingTime/gridX/gridY`, clears `isEnd`, uses `boardConfig.GetZombiePosition(gridX,gridY)` when the plain managed `boardConfig` reference is non-null, and otherwise stores `Vector3.zero`. The previous managed position reconstruction was incorrect.

## Deterministic patching and readback

HF48 patcher build head `a756f88bdc5d502ea06a19146ae33362e2564bef`; workflow `34543329650` PASS; artifact ID `10178091954`; artifact ZIP SHA-256 `6708200c297776a9685a7ebca56eed7d77340825ce4543411dff03a0130ce19b`.

The patcher hard-locks formal HF47 input SHA and all six target tokens/names/parameter counts. Two independent applications completed with zero stderr and produced byte-identical candidate SHA `6c1eb1468f398610cc61ea89b7f3a9409f6baeb451e6df0ae6243b28af50e48e`.

Cecil reopen:

- `0x060001B3` — 64 IL / 186 bytes / 0 EH
- `0x060001B4` — 89 / 260 / 0 EH
- `0x06000273` — 3 / 7 / 0 EH
- `0x06000274` — 97 / 308 / 0 EH
- `0x0600027D` — 44 / 115 / 1 EH
- `0x060002A4` — 27 / 66 / 0 EH

No target contains a Cpp2IL helper reference.

## Fixed ILSpy and semantic isolation

Fixed ILSpyCmd / ICSharpCode.Decompiler `11.0.0.9375` and the locked 56-DLL reference set were reused. HF47 whole IL reproduced `e30a0559b3b2c6fbbafcc1783287a94a83364602de3891222f469556eb91438d`; HF48 whole IL is `47726f82264e97ecc44a9fdb5a41fcb447c76da57a16cce3c2c20622e7ecb0bd`.

All six target member decompiles exit 0 with zero stderr and zero Cpp2IL/Unknown/NotImplemented/invalid stack-type-comparison/warning-error markers.

Whole-IL isolation parses 2317 method blocks before and after. The normalized non-method/method-signature skeleton is byte-identical; changed methods are exactly the six Cecil-authoritative target tokens above. HF47->HF48 semantic diff SHA-256 is `94bbc39d24b4aabc5c31b425d42badabb88fe916452e551d11e24eea4f5b7b1b`.

The canonical HF48 MethodDef table contains 2317 rows and SHA-256 `c7d4dfa7af316c26366dfa2f5a1a75b5c47d418bf1d5c269f014ac6ec99f6aab`. Distinct nonzero body RVAs move from 2139 to 2140 because `ZombieManager.Update`, previously an empty/shared reconstruction body, now owns the original seven-byte call body; this is expected and confined to the authorized target set.

## Permanent cumulative RecoveryAudit

The first composite layout was deliberately rejected during real execution because the HF47 targeted auditor hard-locks the HF47 whole-file SHA and therefore correctly refuses an HF48 candidate. No candidate was accepted from that layout.

The corrected HF48 cumulative auditor locks the HF48 whole-file SHA and checks the three HF47 late-stage targets plus all six HF48 targets, while the unchanged historical RecoveryAudit independently covers HF1-HF46. Corrected composite build head `54cedf35bfdfd8244155e625dbf497d250f2d40e`; workflow `34546011331` PASS; artifact ID `10179032692`; artifact ZIP SHA-256 `3867f6e6116cdc6deaacdf7ba6f24b0087cf69aca6cc593d528fc0f5df9e3154`.

Two independent corrected composite executions completed with zero stderr. Each historical run ended `RECOVERY_AUDIT_OK`; each cumulative late-stage run performed OPEN1 and OPEN2 for HF47+HF48 targets and ended `HF48_AUDIT_OK`.

## Residual gate

The six-method ladder runtime cluster is clean. Residual markers remain elsewhere, including the broad EnemyManager initialization/EnemySelecter dependency cluster and several ZombieManager enumeration/utility paths. They are not automatically promoted. Any HF49 requires a fresh original-PC-native active-path/dependency closure; if no candidate satisfies that gate, managed recovery stops and the project proceeds to 67/67 Unity/package validation.

**HF48 is authorized for exactly these six MethodDefs and no others.**
