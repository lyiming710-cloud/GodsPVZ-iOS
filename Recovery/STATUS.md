# Recovery status

## Latest checkpoint — 2026-10-25

[Startup R10](Startup-R10-2026-10-25/README.md) restores `0x06000383 Plant.PC_Nutknight` from the original 592-instruction caller and actual 49-instruction Floor helper. It restores portable PC integer-conversion behavior, missing middle-state sprite dispatch, and 21 animation-list captures before sprite callbacks. The initial numeric-only trial passed type/JIT but failed 65 runtime comparisons; that failed candidate and evidence are retained. Final 2,116 new comparisons match, nine fault variants are detected, and all 3,467 prior records rerun successfully on exact R10: 5,583 controlled cases across 13 methods. Only one MethodDef changes; the other 2,316 and all old code/metadata bytes remain intact except bounded PE/CLI append headers. Fresh full JIT has 2,247 prepared, zero rejected and 50 unsupported open generics. Frozen types still retain 131 failures or unsupported cases after exact interface facts; access audit exits 2 and rejects 216 sites across 111 methods. There are zero unterminated lexical tails. A separate license-free IL2CPP/all-generated-C/C++ diagnostic is prepared before another licensed export. No R10 Unity/Apple/IPA/device success is claimed.

2026-10-25 live readback: exact R8 real Editor diagnostic [40115001552](https://github.com/lyiming710-cloud/GodsPVZ-iOS/actions/runs/40115001552) passed: normal MainMenu 2,113 frames, two active Canvases, one MainUIController, zero errors; 76 shader compilations (38 Metal), 20 controlled OpenGL pixel readbacks and all 96 raw program/PNG files verified. This is not an iPad test or IPA export.

[Startup R9](Startup-R9-2026-10-25/README.md) restores five complete original native constant-boolean functions and SimpleScript.Update's actual remainder/frame-cache behavior. Every MethodDef is compared to handle the four shared managed-body aliases correctly: six logical methods change in three physical slots, 2,311 others remain identical. New native/CIL execution covers 992 cases; prior suites rerun successfully, total 3,467 controlled cases across 12 methods. Fresh JIT: 2,246 prepared / one rejected (PC_Nutknight) / 50 unsupported open generics. Unterminated lexical tails: zero. Frozen types still have 132 failures or unsupported cases after exact interface facts; access still rejects 216 sites across 111 methods. These held scopes prevent a whole-game/IPA acceptance claim.

Updated 2026-10-25: [Startup R8](Startup-R8-2026-10-25/README.md) repairs four unreachable nonvoid tails by shortening only CodeSize, seals a dead try-region edge in Administrator.Start and orders the two existing Zombie.SetUIs finally clauses inner-first. All six real methods change from CLR rejection to prepared, preserving reachable CIL, exception regions/handlers and the other 2,311 MethodDefs. A fresh 2,297-body JIT scan reports 2,240 prepared / seven rejected / 50 unsupported open generics; all 2,475 prior controlled caller cases were rerun successfully on exact R8. Five reachable damaged tails remain. Full frozen type/access queues are retained as failed or unsupported. A separate restricted Editor/shader diagnostic is prepared; no full export or updated IPA is claimed.

[Startup R7](Startup-R7-2026-10-24/README.md) adds 132 proven unreachable void-tail repairs to R6. Independent actual-CIL and whole-file audits preserve every reachable instruction and all other bytes. Direct preparation of each real method changes from 132/132 CLR rejections to 132/132 prepared, with no method execution by that check. The six controlled caller suites were reexecuted on exact R7 and again matched in 2,475 cases. Full assembly JIT prechecks, after an explicit existing-reference supplement, yield 2,234 prepared, 13 rejected and 50 open generics unsupported. The lexical-tail queue shrinks from 141 to nine. JIT preparation does not prove gameplay and does not override the frozen checker/access failures below. Full player export remains held.

[Startup R6](Startup-R6-2026-10-24/README.md) archives actual original-PC-caller versus actual-CIL execution for six targets in 2,475 controlled cases. Twenty-three fault controls are detected by execution; one Int64 sentinel defect is invisible to permissive CLR truncation and is rejected separately by the unchanged frozen width checker. R6 also fixes four proven unreachable unterminated warning tails discovered by actual execution. All other file bytes remain identical to its R5 parent. This is scoped caller qualification, not whole-game acceptance.

The exact R6 DLL still has whole-assembly gate queues: 217 rejected access sites across 112 methods; 133 type failures or unsupported cases after supplementing exact interface metadata facts; 141 unterminated lexical tails, of which 132 are unreachable void warning tails and nine require separate specifications. These categories overlap and are not a total gameplay-bug count. Full player export remains held while batch form repairs and wider qualification proceed.

The separate [custom shader checkpoint](Custom-Shaders-2026-10-24/README.md), actual run [40094204567](https://github.com/lyiming710-cloud/GodsPVZ-iOS/actions/runs/40094204567), passed 76 Unity shader stage compilations (38 Metal / 38 OpenGLCore), 20 controlled Editor pixel readbacks and a normal R3 MainMenu startup probe. This does not establish iPad execution. No updated IPA has been accepted for the reported iPad black-screen/flicker issue.

The earlier strategy and reconstruction notes below are retained as historical context; use the dated checkpoints above for current results.

## Current strategy

Use the PC x86-64 IL2CPP build as the primary gameplay-logic source because Cpp2IL recovers it substantially more accurately, while retaining the Android/mobile asset export and using the Android build as the reference for touch/mobile-specific behavior.

## Reverse-engineering results

- Unity editor version: `2022.3.44f1c1`
- IL2CPP metadata: `31.1`
- Android CodeRegistration / MetadataRegistration: `0x2772B68` / `0x285F870`
- PC CodeRegistration / MetadataRegistration: `0x1815E88C0` / `0x1818C6D00`
- PC Cpp2IL recovery: `2318 / 2319` methods; only `Zombie::InjuryStatusUpdate_Body` was reported as a method-level decompilation failure.
- Android/ARM64 ILSpy output: 15,946 `Cpp2ILHelpers.NoteDecompilerIssue` calls.
- PC/x86-64 ILSpy output: 4,304 `Cpp2ILHelpers.NoteDecompilerIssue` calls, a ~73% reduction.
- Critical scene transition `GlobalStaticVars.EnterBoard()` is correctly recovered on PC as `SceneManager.LoadScene("Board")`; the Android recovery lost that string through an unresolved unmanaged-memory load.
- The PC build still contains touch APIs (`Input.GetTouch`, `touchCount`) in the same gameplay classes, so mobile input was not wholly compiled out of the PC build.

## Unity reconstruction

- 3,149 Unity asset objects were exported with AssetRipper.
- The recovered project contains roughly 6,783 files.
- 173 serialized game script types were moved from individual AssetRipper `.cs` GUIDs to the recovered `Assembly-CSharp.dll` local file IDs.
- After replacing the Android-recovered game DLL with the PC-recovered game DLL, those 173 type/fileID pairs still match 173/173.
- 263 game-script references across 114 serialized assets were previously migrated to the recovered game DLL.
- Original scene names recovered from `globalgamemanagers` are restored as `Assets/Scenes/MainMenu.unity` and `Assets/Scenes/Board.unity` in that build order.

## Unity package restoration

Cpp2IL-produced copies of package/runtime assemblies are not trusted as runtime implementations. The project now restores the exact package versions embedded in original build paths and removes the recovered package DLLs that would shadow the real packages.

Exact package versions:

- `com.unity.ugui@1.0.0`
- `com.unity.textmeshpro@3.0.6`
- `com.unity.render-pipelines.core@14.0.11`
- `com.unity.render-pipelines.universal@14.0.11`
- `com.unity.2d.animation@9.1.1`
- `com.unity.2d.tilemap.extras@3.1.2`
- `com.unity.burst@1.8.17`
- `com.unity.collections@1.2.4`
- `com.unity.mathematics@1.2.6`
- `com.unity.visualscripting@1.9.4`

Serialized package script references cover 67 distinct types: 19 UGUI, 9 TMP, 32 RenderPipeline Core debug-UI types, 5 URP types, and 2 2D Animation types. `GodsPVZPackageReferenceMigrator.cs` resolves the actual `MonoScript` GUIDs from installed official packages inside Unity and rewrites the old recovered-DLL references automatically.

## Remaining blockers before claiming a working IPA

1. Import the reconstructed project in Unity `2022.3.44f1c1` and let Package Manager restore the exact packages.
2. Run/verify the package-reference migration (67/67 types must resolve).
3. Verify the recovered PC game DLL is accepted by Unity and can be converted by IL2CPP for iOS.
4. Repair the remaining semantic recovery defects, starting with the single failed method and critical gameplay paths that still contain `NoteDecompilerIssue`/invalid-IL artifacts.
5. Export the iOS Xcode project and compile it with code signing disabled, then package `Payload/*.app` into an unsigned IPA.
6. Install on a signed/sideload-capable test device and validate startup, menu -> Board transition, touch placement, dragging, pause/time-slow controls, save/load, and a full level.

The repository intentionally does not mark the port complete until those runtime checks pass.
