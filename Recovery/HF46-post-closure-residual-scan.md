# HF46 post-closure residual active-path scan

Date: 2026-09-11  
Branch: `high-fidelity`

## Purpose

This is the mandatory fresh scan required after formal HF46 closure. It does **not** reuse `HF46-residual-scan.txt` as a post-HF46 result; that archived record was the post-HF45 decision that opened HF46.

No HF47 patcher, workflow, candidate DLL, or formal stage is created by this record.

## Locked input

Formal cumulative input:

`GodsPVZ-Assembly-CSharp-HF46-EnemyManagerDependency-Audited-2026-09-10.dll`

SHA-256 reverified from the provider-fetched HF46 DLL:

`900d3aabbf79b094a6b412f7d869cc5285ce206a2d17501b69fc3cd8651beefd`

Fixed ILSpyCmd / ICSharpCode.Decompiler `11.0.0.9375` raw whole-assembly IL decompilation completed with exit 0 and zero stderr. The reproduced whole-IL SHA-256 is:

`3bd8d8b77fa63476da612b5a26ad799395dbd2f0eedf836e50ef86cf991c5901`

This exactly matches the HF46 locked whole-assembly IL provenance. The assembly still contains 2317 MethodDefs.

## Fresh residual observations

The raw whole-assembly IL contains 3920 calls to `Cpp2ILHelpers.NoteDecompilerIssue` distributed across 757 MethodDefs. This is only a damage-candidate pool; issue count is not a promotion gate. A large part of the residual pool is editor/demo/package-support code and methods without independently proven material gameplay reachability.

The strongest retained gameplay candidate remains the EnemyManager wave-timing chain:

- `EnemyManager.Update()` directly calls `EnemyManager.TimeUpdate()` on every Update invocation.
- `0x060001C0` — RID 448 — `EnemyManager.TimeUpdate()` still contains 5 explicit Cpp2IL issue calls and concrete invalid/mis-reconstructed arithmetic/type paths in the HF46 managed body.
- `0x060001B5` — RID 437 — `EnemyManager.DispatcheWave(Wave)` is a direct `TimeUpdate()` dependency and still contains 3 explicit Cpp2IL issue calls plus invalid managed reconstruction.
- `0x060001B6` — RID 438 — `EnemyManager.DispatcheZombie(Enemy)` is called by `DispatcheWave`. Although it contains no `NoteDecompilerIssue` call, its raw HF46 IL still contains a concrete type-invalid failure branch that constructs `System.IndexOutOfRangeException` and returns that object from a method whose declared return type is `Zombie`; therefore zero issue-call count is not evidence of semantic closure.

HF45/HF46 already closed the directly relevant `FlagMeter.UpdateMeter`, `EnemyManager.PlayBoardAudio(int)`, and `EnemyManager.TextWaveHealth()` dependencies. That materially reduced the TimeUpdate dependency surface, but it did not close `DispatcheWave`, `DispatcheZombie`, or the huge-wave/final-particle Board/state paths.

## Original-PC-native gate

The fixed project rule remains: a later cumulative HF stage may open only when a target simultaneously has:

1. concrete managed loss/mis-reconstruction;
2. material gameplay reachability; and
3. behaviorally closed original-PC-native evidence for the target and every dependency required to reproduce its behavior without invention.

Conditions 1 and 2 are satisfied for the `TimeUpdate -> DispatcheWave -> DispatcheZombie` chain. Condition 3 is **not** satisfied by the currently retained HF46 closure package. The HF46 native-evidence archive behaviorally closes only the two HF46 targets (`PlayBoardAudio` and `TextWaveHealth`); it does not contain complete original-PC native bodies/attribution sufficient to reconstruct `TimeUpdate`, `DispatcheWave`, and `DispatcheZombie` as a new cumulative patch without guessing.

No target is promoted merely from warning count, RID adjacency, Cpp2IL issue density, high-level decompiler output, shared-stub xrefs, or apparent arithmetic intent.

## Decision

**HF47 is not authorized by this fresh post-HF46 scan.**

This is a gate failure, not a claim that the remaining methods are unrecoverable. If the exact original PC `GameAssembly.dll` / metadata evidence is later reintroduced and the three-method wave chain plus huge-wave/final-state dependencies can be behaviorally closed, the gate may be re-run from the locked HF46 cumulative input. Until then the formal managed-recovery chain stops at HF46.

Per `Recovery/STATUS.md`, the next workstream is now:

1. perform the 67/67 Unity package-script validation;
2. integrate the formal HF46 cumulative `Assembly-CSharp` into the reconstructed Unity project;
3. compile and validate `MainMenu`, `Board`, and representative gameplay paths;
4. only after those gates pass, apply the minimum necessary iOS adaptations and proceed toward the unsigned IPA path.

## 67/67 package-validation scope

`Recovery/package-script-map-editor.json` contains exactly 67 mapped package script types:

- `UnityEngine.UI.dll`: 19
- `Unity.TextMeshPro.dll`: 9
- `Unity.RenderPipelines.Core.Runtime.dll`: 32
- `Unity.RenderPipelines.Universal.Runtime.dll`: 5
- `Unity.2D.Animation.Runtime.dll`: 2

Total: **67**.

The exact package versions remain the project-locked set recorded in STATUS/history; validation must be against the reconstructed project and must not silently substitute newer package mappings.
