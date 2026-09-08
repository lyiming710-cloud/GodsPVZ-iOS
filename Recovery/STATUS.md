# Recovery status

## Current strategy

Use the PC x86-64 IL2CPP build as the primary gameplay-logic source because Cpp2IL recovers it substantially more accurately, while retaining the Android/mobile asset export and using the Android build as the reference for touch/mobile-specific behavior.

## Reverse-engineering results

- Unity editor version: `2022.3.44f1c1`
- IL2CPP metadata: `31.1`
- Android CodeRegistration / MetadataRegistration: `0x2772B68` / `0x285F870`
- PC CodeRegistration / MetadataRegistration: `0x1815E88C0` / `0x1818C6D00`
- Initial PC Cpp2IL recovery: `2318 / 2319` methods; the only full method-level failure was `Zombie::InjuryStatusUpdate_Body`.
- That missing method is now recovered in HF3 directly from PC native x86-64 at `0x1803652B0`, independently Cecil/ILSpy audited, and classified `Exact`. Final HF3 SHA-256: `23014656af797490bc35d9feb8f78950bcfa79a167c6a1de3303e1d30251c32f`.
- HF1/HF2/HF3 are cumulative high-fidelity managed recovery stages. HF3 modifies exactly one managed method relative to the audited HF2 after normalizing physical RVA/data-placement shifts.
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

1. Continue native-backed recovery of critical gameplay paths that still contain `NoteDecompilerIssue`, invalid-IL artifacts, or previously introduced stability/behavior-equivalent implementations. Do not replace them with guessed gameplay logic.
2. Import the reconstructed project in Unity `2022.3.44f1c1` and let Package Manager restore the exact packages.
3. Run/verify the package-reference migration (67/67 types must resolve).
4. Verify the cumulative high-fidelity game DLL is accepted by Unity and can be converted by IL2CPP for iOS.
5. Only after core recovery is sufficiently complete, add the minimal iOS adaptation layer, export the iOS Xcode project, compile with code signing disabled, and package `Payload/*.app` into an unsigned IPA.
6. Install on a signed/sideload-capable test device and validate startup, menu -> Board transition, touch placement, dragging, pause/time-slow controls, save/load, and a full level.

The repository intentionally does not mark the port complete until those runtime checks pass.
