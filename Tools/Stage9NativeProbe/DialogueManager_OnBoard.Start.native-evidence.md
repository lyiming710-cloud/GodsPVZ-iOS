# DialogueManager_OnBoard.Start PC native authority

Source authority: original GodsPVZ 1.0.2 PC x86-64 IL2CPP `GameAssembly.dll`.

- GameAssembly SHA256: `9ebd7ca996a5b03fb4a766f7a2502b660d581ddbbb36af4d7bf2f06a211da39d`
- Method: `DialogueManager_OnBoard.Start()`
- Managed token: `0x06000193`
- RID: `403`
- Assembly-CSharp CodeGenModule pointer-table entry: `0x181B839F0`
- Native method pointer: `0x180314620`
- `.pdata` function range: `0x180314620-0x18031478F`
- Native function size: `367` bytes
- Native slice SHA256: `600d505be96241a0c336dd07725df87fdfc5e32184162e16aad70234a261a8d5`

## Recovered semantics

The PC native method performs the following operations, in order:

1. `playerSave = GlobalStaticVars.gLawnApp.savesManager.playerSave`.
2. `image_Curtain.gameObject.SetActive(false)`.
3. `isRunning = false`.
4. `dialogueTriggerType = DialogueTriggerType.None` (native value `0`).
5. `board.GameContinue()`.
6. Dispatch on the integer value of `board.challengeType`:
   - `-1`: continue to dialogue-list assignment.
   - `0`: continue to dialogue-list assignment.
   - `1`: if `playerSave.rescuePassNum[board.level] > 0`, return; otherwise continue.
   - `2`: if `playerSave.adventureHardMaxStarNum[board.level] > 0`, return; otherwise continue.
   - any other value: return.
7. `dialogueList = board.boardConfig.boardDialogues`.

The integer dispatch is visible directly in native code: `challengeType + 1`, two successive decrements, then `cmp 1`. It maps exactly to native values `-1, 0, 1, 2` above.

Null and bounds behavior must not be replaced by defensive guards. The PC native body uses IL2CPP null/bounds failure helpers when the corresponding reference or array access is invalid. A managed reconstruction should therefore use ordinary direct dereferences/indexing and let CLR/Unity preserve the same failure class instead of silently returning.

## Damaged managed fingerprint on 273d base

- input DLL SHA256: `273ddef385775d169aa88d63a505aeb2b99aab4b8c32204a4929175310e2969b`
- token `0x06000193`
- RID `403`
- managed RVA `0x1C5C0`
- managed code size `580`
- locals `22`
- semantic fingerprint `3459add3f752d46c4703e2164a017c58d0375db38db79ae8a1a0544917a4d5c0`
- exact-reference readback contains two `Unknown result type` diagnostics and three `Expected ...` diagnostics.
- the corrupted body stores integer challenge arithmetic into `System.Object` locals and then performs object/integer arithmetic.

## Promotion rule

This evidence was prefetched read-only because the 273d exact-R3 log showed `DialogueManager_OnBoard.Start` immediately after the then-first-causal `Plant_DetaiPage.LoadSkillLogo` failure. Do not generate or advance a gameplay candidate for this method until the 4110 `LoadSkillLogo` exact-R3 runtime gate formally removes that earlier blocker and promotes `Start` as the next first causal Board-path failure.
