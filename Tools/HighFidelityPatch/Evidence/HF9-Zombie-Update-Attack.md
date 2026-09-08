# HF9 — Zombie.Update_Attack() native recovery evidence

## Target
- Method: `Zombie.Update_Attack()`
- Original MethodDef RID: `1058`
- Metadata token: `0x06000422`
- PC native x86-64: `0x18036ADC0`–`0x18036B75F`
- Primary source: original PC `GameAssembly.dll` + original metadata.
- Confidence: **Exact** for the recovered method body.

## Native behavior recovered
HF9 replaces the damaged Cpp2IL attack-loop body with the PC-native control flow. The recovered method preserves:
- Shooting IDs `{6,7,8,9,19,20,21,22}` and the native shooting interval/update-rate timing path.
- Two independent `Time.deltaTime` reads rather than caching a frame delta.
- Exact animator strings `"ShootingTrigger"`, `"isAttacking"`, `"Group"`, and `"Ready"`.
- ID `18` early return, ID `17` SPH seek path, ID `13` Snowbeast path, and the pole/ID-23 gate before generic melee handling.
- Unity Object comparison semantics for plants/devices instead of CLR reference-only null tests.
- PC-native inlined attack/walk transitions; HF9 deliberately does not call the still-untrusted `Update_Shooting`, `TranFromAttack`, `TranToAttack`, or `TranToWalk` helper bodies.
- Generic melee target handling, `Plant.BlockZombie`, animation-state changes, move/attack speed reset order, and rSpeed zeroing.
- Native Group randomization set `{0,2,4,5,14,15}` plus `IsPlantZombie()` rather than the broader damaged Cpp2IL behavior.
- Snowbeast target-device logic, including broken-device and seek-CD side-effect-only `TrySeekTragetPlant()` paths.
- Snowbeast temporary path insertion using `new EnemyPath(gridX, gridY, 9999f, board.boardConfig)`, followed by `Path_Finding`, removal, seekCD reset, and target-device assignment.
- Native `waitingTime = 0.02f` fallback path.

## Exact constants / comparison details
- Snowbeast temporary `EnemyPath` waitingTime is `9999f` in PC native; the Android Cpp2IL recovery's `0f` value is not used.
- Snowbeast fallback waitingTime is `0.02f`.
- Shooting and seek timers preserve native ordered/unordered floating-point branch behavior, including NaN fall-through behavior.
- The seek path does not assign the return value of `TrySeekTragetPlant()` in branches where PC native uses the call for side effects only.

## Validation
1. HF9 patcher CI run `34184931178` succeeded (head `86ad7f5c233443d518a4d4f01d42f5b270cd1f80`).
2. Real audited HF8 input was patched successfully.
3. Patcher reopen verification: `Zombie.Update_Attack` = `408` IL instructions / `1383` bytes; exactly two `Time.deltaTime` references retained; no `Cpp2ILHelpers` and no calls to the four inlined forbidden helpers.
4. ILSpyCmd 11.0.0.9375 with the original Unity reference directory decompiled the recovered method with stderr = 0. Unity Object comparisons, Snowbeast `9999f` path construction, and all principal branches read back as strong typed C#.
5. Permanent Mono.Cecil recovery auditor run `34185052863` (head `6d9f6900280ff68097955f190a9f1f6a70c7acbb`) succeeded. Local execution of that artifact against the HF9 DLL passed OPEN1 and OPEN2; both reads report token `0x06000422`, `408` IL, `1383` bytes, while all prior HF3–HF8 recovery checks also pass.
6. Full-assembly IL isolation: after normalizing method RVAs, private/static physical data placement, and `.data cil I_xxxxxxxx` physical labels, HF8→HF9 contains exactly **one** diff hunk, ending at `Zombie::Update_Attack`; no other MethodDef changes semantically.

## Final artifact
SHA-256: `c3e1716bbd598a8b2d2af3b1b5907869cc581271645ccc2a9e81595dfac07ec2`

## Scope note
HF9 makes `Zombie.Update_Attack()` Exact. It does not imply that `Zombie.Update_Characteristic()`, `BuffManager.Update<Zombie>`, or remaining path/combat helpers are already Exact; those remain later recovery stages.
