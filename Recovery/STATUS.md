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
- HF4 restores `Zombie.Awake()` directly from PC native x86-64 at `0x18035DAD0`. The six armor initialization tables, enum default behavior, field identities, and native write order are independently Cecil/ILSpy audited and classified `Exact`. Final HF4 SHA-256: `2abbc9eb02b93bcd0178091bbc38ca450b6162875df9dcb55e874d5b9af9f0a6`.
- HF5 restores `Zombie.Start()` at `0x1803695F0` and its direct recursive dependency `Zombie.LoopAddAnimation(Transform)` at `0x180365BA0`. Native call ordering, exact Group-ID set, animation speed setup, independent transform-position reads, previous-position write order, BoardEntry scaling, pre-path initialization, recursive child enumeration, and IEnumerator finally/Dispose semantics are independently audited and classified `Exact`. Final HF5 SHA-256: `58fe2001b6d9df986a08a930e60ea936fbf9392d82532f2e815bda63557d5e3a`.
- HF6 restores `Zombie.Update()` directly from PC native x86-64 at `0x18036CD60` (RID 1057, token `0x06000421`). The update-rate formula and NaN behavior, eight independent `Time.deltaTime` calls, rSpeed movement, Transform writes, path state machine, SortingGroup order, buff/combat/element/injury ordering, brightness/previous-position tail, and destroy countdown are independently Cecil/ILSpy audited and classified `Exact`. Final HF6 SHA-256: `e6395652d0d413fbb496dc9cae8f2712d6a65bcfad109d160818983cde234e75`.
- HF7 restores `Zombie.GetMoveDirection()` directly from PC native x86-64 at `0x180361440` (RID 1103, token `0x0600044F`). It retains the real `List<EnemyPath>.Enumerator` plus finally/Dispose shape, first-unarrived-node selection, ordered x deadzone behavior, native `1e-5f` normalization epsilon, float/double sqrt path, and distinct signed-zero return paths. Cecil/ILSpy and whole-assembly semantic isolation classify it `Exact`. Final HF7 SHA-256: `ed01e0aa7dac968e8203131e6274a4da718ebe5fdcd0dcb3d3bc41d3b0f60ea4`.
- HF8 restores `Zombie.SetrSpeed(Vector3)` directly from PC native x86-64 at `0x180368280` (RID 1145, token `0x06000479`). It preserves the ID 17 tail, generic direction/localScale sign handling, Snowbeast stop/deceleration/pre-pass/steering path, `GetMS()` scaling, double-cross-product steering, native 1.5 magnitude clamp, six independent `Time.deltaTime` reads, the exact metadata-backed `"Snowbeast.speed"` lookup, `StatsIncreased.value = |rDirection| - 1`, localScale flip read order, and final animation-frame speed multiplication. Cecil/ILSpy and whole-assembly semantic isolation classify it `Exact`. Final HF8 SHA-256: `4dc9b2486ae41b2d9c340293dada22e74ee6877290e0e79f456b770b92bb4688`.
- HF9 restores `Zombie.Update_Attack()` directly from PC native x86-64 at `0x18036ADC0` (RID 1058, token `0x06000422`). It preserves the shooting-ID/timer path, exact animator strings, Unity Object comparison semantics, PC-native inlined attack/walk transitions, generic melee block/unblock behavior, ID 17/18/13 special routing, Snowbeast seek/device logic, exact `EnemyPath(..., 9999f, ...)` construction, `waitingTime = 0.02f`, and two independent `Time.deltaTime` reads. Cecil/ILSpy and whole-assembly semantic isolation classify it `Exact`. Final HF9 SHA-256: `c3e1716bbd598a8b2d2af3b1b5907869cc581271645ccc2a9e81595dfac07ec2`.
- HF10 restores `Zombie.Update_Characteristic()` directly from PC native x86-64 at `0x18036B960` (RID 1060, token `0x06000424`). The exact metadata-backed strings `PoleCommander.speed`, `JumpTrigger`, `rest`, and `PlaceTrigger`, the Pole Commander buff-null early exit, Snowbeast and ladder countdown/NaN behavior, ID23 jump/passable checks, and three independent `Time.deltaTime` reads are independently audited and classified `Exact`. Final HF10 SHA-256: `0684c52733afd5dd1d45e455ede8ec5355d8b53bb65875506cb97f9a29f07b8a`.
- HF11 restores the shared generic `BuffManager.Update<T>(T host)` from the PC generic native instance at `0x180429970`–`0x180429D93` (managed token `0x06000101`). Direct PC xref scanning finds the same native instance called from both the Plant combat-update path (`0x18035C155`) and `Zombie.Update` (`0x18036D23D`), confirming that the open generic definition rather than a Zombie-only specialization should be recovered. The three List<Buff> enumerations, three finally/Dispose regions, add/update/remove/clear ordering, and `Buff.Start<T>/Update<T>/End<T>` host binding are independently audited and classified `Exact` for managed-observable behavior. Final HF11 SHA-256: `c990a8be0a615cbd1aed15b08251ae892889c056badc3e4070d7c6c24a6cb07f`.
- HF1 through HF11 are cumulative high-fidelity managed recovery stages. After normalizing physical RVA/data-placement shifts, HF3 changes exactly one managed method relative to audited HF2, HF4 changes exactly one managed method (`Zombie.Awake`) relative to HF3, HF5 changes exactly two managed methods (`Zombie.Start`, `Zombie.LoopAddAnimation`) relative to HF4, HF6 changes exactly one managed method (`Zombie.Update`) relative to HF5, HF7 changes exactly one managed method (`Zombie.GetMoveDirection`) relative to HF6, HF8 changes exactly one managed method (`Zombie.SetrSpeed`) relative to HF7, HF9 changes exactly one managed method (`Zombie.Update_Attack`) relative to HF8, HF10 changes exactly one managed method (`Zombie.Update_Characteristic`) relative to HF9, and HF11 changes exactly one managed method (`BuffManager.Update<T>`) relative to HF10.
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

1. Continue native-backed recovery of remaining critical runtime dependencies called by the now-Exact Plant/Zombie update chains, prioritizing damaged `Buff.Start<T>/Buff.Update<T>/Buff.End<T>` implementations and path/device/element helpers according to native call centrality and current CIL damage. Do not replace them with guessed gameplay logic.
2. Import the reconstructed project in Unity `2022.3.44f1c1` and let Package Manager restore the exact packages.
3. Run/verify the package-reference migration (67/67 types must resolve).
4. Verify the cumulative high-fidelity game DLL is accepted by Unity and can be converted by IL2CPP for iOS.
5. Only after core recovery is sufficiently complete, add the minimal iOS adaptation layer, export the iOS Xcode project, compile with code signing disabled, and package `Payload/*.app` into an unsigned IPA.
6. Install on a signed/sideload-capable test device and validate startup, menu -> Board transition, touch placement, dragging, pause/time-slow controls, save/load, and a full level.

The repository intentionally does not mark the port complete until those runtime checks pass.
