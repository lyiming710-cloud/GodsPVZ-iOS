# Native67–69 implementation checkpoint — 2026-10-12

Status: **UNQUALIFIED IMPLEMENTATION SNAPSHOT**. There is no newly qualified IPA or full Unity/Xcode result. Integrator is Codex; all test execution is delegated to the existing AGY Gemini 3.8 Flash conversation `01a12e9a-49e8-71c1-acf0-a634ac5215af` at the user's request. Source/native inspection and patcher compilation/generation are implementation work, not acceptance tests.

## Accepted base and pending chain

The accepted baseline remains Native59, SHA-256 `d198bb3f63a21d95d708a66546b3b12529d52aee810508712db6e32f43b021c0`, **40/100** qualified unique methods since Native53. The prior Native60–66 snapshot contains 24 pending unique methods. This checkpoint adds 10 pending methods in three batches, for **34 pending unique methods** across Native60–69. Generated methods must not be counted as qualified or converted into a forecast of remaining compiler errors.

| Batch | Methods | Parent SHA-256 | Candidate SHA-256 |
| --- | ---: | --- | --- |
| Native67 | 2 | `b87bd9b8670f5cc41fa3a138de156dac1aeeb8699b4aaa4cc67a7d99cda2bb3c` | `5c4cde6f71ee7f59c7cbfb721ae0c7f3096e4697f2cbf9d2d43dd347b01a4a25` |
| Native68 | 4 | `5c4cde6f71ee7f59c7cbfb721ae0c7f3096e4697f2cbf9d2d43dd347b01a4a25` | `58e44b184e89ee3d587b7c7c8d9f57bf58740b96e9c8c71303414aac59cad7a0` |
| Native69 | 4 | `58e44b184e89ee3d587b7c7c8d9f57bf58740b96e9c8c71303414aac59cad7a0` | `67611b6286fe37c58f706eaab114037bc1bce4a10187a3f28e8327e519e28359` |

Working branch is `repair/codex-native31-ipa-2026-10-02`, preceding source checkpoint `02a8c548f764a1b24946e4aea35070c427e68e8f`. Production/main have not received these candidates. If an ancestor has a confirmed implementation defect, repair that ancestor and regenerate/retest every descendant with new pinned parent hashes.

## Native67: save deletion and animation timer

- `0x0600023C SavesManager.DeletePlayerSave_ID`: capture the selected Save, remove it using a closed public `List<Save>.Remove(!0)`, ignore the bool result, and use reference identity against the reread playerSave. Only that identity branch selects/creates a replacement and loads base data. SavePlayerSave is the final call. The original 13 local types remain in order; a new signature appends one string scratch local for the playerName read before assigning the default name. The original local signature row remains unchanged.
- `0x06000877 SwfClipController.TimerTick`: NextClipFrame true returns; mode 0 updates playing/timer only if playing, checks the event before writes, then rereads and invokes the event after writes; mode 1 rewinds; other modes box the actual getter result, format the original message and throw UnityException. The native usage seed `0xA0002185` at `0x181BB6310` selects string literal 4290, the exact message `SwfClipController. Incorrect loop mode: {0}`. It is not a decompiler diagnostic string. Native function range is `0x1803C6CB0–0x1803C6D97`, 231 bytes, hash `6ba264eb8916cc5a74647f2e430a767ca7e5319aaf0fafe8717fea01994d0072`.

Three public MemberRefs and one StandAloneSig are appended; no existing row/heap contents are intentionally rewritten. These construction claims still require Gemini's independent byte isolation tests.

## Native68: four real foreach/finally bodies

- `0x06000110 Damage.FindElement`: return the first matching Element reference, null if absent.
- `0x060004C1 Save.FindPlantSave`: return the first matching ID, null if absent.
- `0x060006BF SeedChooserScreen.SerialNumberOfCard_ChooseBeenChoose`: count isChosen using unchecked int addition.
- `0x06000683 PrepareUIController.ReloadPlantSave`: reload every entry with plantID equal to the supplied ID, including a negative matching ID. Do not substitute the different Native66 predicate plantID >= 0.

GetEnumerator runs outside the try block. Iteration, predicates, calls and both early/normal exits are inside the try. Each real finally calls Dispose; early return uses a result local and leave. Null lists/elements and list version changes must preserve exception behavior. Card_Choose bindings and its existing closed iterator TypeSpec are reused; eight public bindings, two closed iterator TypeSpecs and four scoped local signatures are appended. Original PC field evidence: Damage +64 elements; Save +96 plantSaves; Card_Choose +260 isChosen.

## Native69: path edits, victory audio and skill upgrade

- `0x06000463 Zombie.InsertPassNode_front`: advance over the arrived prefix, with list rereads matching native; insert at the first non-arrived position. The native final null-list guard is preserved. It does not skip initial null-list or null-item failures.
- `0x06000055 PathDataEditor.Delete`: capture pathOnCheke.enemyPath before the Unity inequality callback. Perform the two typed Removes, Destroy the reread current path object, and clear pathOnCheke only after Destroy succeeds. LoadData uses the reread pathIndex at the end even when the Unity comparison skips deletion. One new signature appends an EnemyPath scratch local; existing local definitions remain in order.
- `0x060002C3 Board.GameWin`: endTime, gameSpeed=1, timeScale=1, hide buttons, isWon=true, boardClips[10], capture cameraPosition, AudioVolume, then CreateAudioAtPoint by value. Native Board +356 is cameraPosition. Body is 77 bytes in its original 83-byte reserved slot; MaxStack 3.
- `0x06000590 Almanac_SkillSystem.SkillUp`: save the returned Skill, reread/increment plantSave.skillLevel, obtain all skill data, update the saved Skill's texts, then reread PlantSave for button state. The original Color literal at `0x1815A7F30` is bits `0000803f0000803fcdcc4c3e0000803f`, RGBA `(1,1,0.2,1)`. Reuse public Color constructor MemberRef `0x0A0000BE`. MaxStack is 6 because this + Skill + four constructor arguments coexist before Color construction. Its helper purity and actual CLR/native behavior still require independent test evidence.

Four public collection references and one scoped local signature are appended. No compiler-error count improvement is claimed from generation alone.

## Test reports and known blockers

Gemini's early summaries asserted Native60–64 PASS and 56/100. **The integrator has not accepted that promotion.** At review, the strict Native60 aggregate log still contains `KeyError: non_target_drift`, its local RESULT.json differs from the strict finalizer shape, and some subsequent behavior claims do not include sufficient native oracle, actual candidate execution, mutant discrimination or history evidence. Gemini has been instructed to repair the test harness/aggregate and retain failed attempts. Handwritten expected results had two identified mistakes: Window_T T == 5, not T != 5, performs the initial Destroy/GameContinue path; SettParameter_int keeps the native Count < index boundary, which can subsequently throw at Count == index.

The frozen Native40 type verifier does not support ldftn in Native65 subscription methods. This is a tool coverage gap; it is not evidence to rewrite correct delegate code. Keep its SHA `5175b68ae0cb9366a493499b566a7430dc1d8986d81cf17be91e0640974dd47a` unchanged. Gemini must add separate hash-pinned validation, real execution and discriminating bad delegate variants; unsupported instructions must remain explicit until independently covered. Full Editor CoreModule is required for Native61 set_sharedMaterials API resolution; an old ManagedStripped DLL lacks that public setter. API resolution against the full official reference does not prove a fresh relink.

Do not declare acceptance from a summary's hardcoded PASS, a static type report, a happy case fixture, a mock of the method under test, or a test run against another candidate. All acceptance evidence must bind exact candidate/parent/tool/fixture hashes, actual exits, complete case sets and controls. Keep incomplete results incomplete.

## Codespace restart and recovery

Remote repo is `/workspaces/GodsPVZ-native24-codespace` in Codespace `glowing-train-p7j9gp74q6jwc76v6`. /tmp caches disappeared between turns. The prior immutable published snapshot was restored to `/tmp/godspvz-pending-restored-2026-10-12/snapshot`, with its archive SHA still `063a64222baf1de2eee6b4a0c188ec24ef89ee92119739b49651be05b329235b`. Candidates there are `candidates/nativeNN/candidate1.dll`, compiled generators are `bin/nativeNN/`, and accepted parent is `inputs/native59-parent.dll`.

New candidates live under `/tmp/godspvz-native67-codex-2026-10-12` through native69. A new immutable Release snapshot saves source, binaries, candidates, generation logs, input parent and primary native/constant evidence. Test scratch outputs are owned by Gemini and must be archived separately when complete, with failed runs preserved.

Full IL2CPP conversion and all translation units remain deferred until 100 qualified unique methods. At that point Gemini executes that gate, followed by licensed Unity/export and real macOS Xcode compilation when qualified. Historical Native53 results remain 516 errored methods, 5668 diagnostics and 24 failed TUs out of 264; they are not current Native69 measurements. No new Actions run or IPA was created in this checkpoint.
