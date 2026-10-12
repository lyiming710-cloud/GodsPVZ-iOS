# Native73–77 implementation checkpoint — 2026-10-12

**UNQUALIFIED_IMPLEMENTATION_SNAPSHOT.** Five new batches contain 17 distinct method implementations. They extend the 44 pending methods in Native60–72 to **61 pending methods**. The accepted base remains Native59, SHA-256 `d198bb3f63a21d95d708a66546b3b12529d52aee810508712db6e32f43b021c0`, at **40/100 qualified new methods since Native53**. The combined accepted plus pending implementation count is 101; that number is not an acceptance or compiler-success count.

The integrator has run no acceptance tests. The user assigned all testing to AGY Gemini 3.8 Flash in conversation `01a12e9a-49e8-71c1-acf0-a634ac5215af`. Patcher compilation and candidate generation are implementation work. Every batch remains pending original-native versus actual-candidate-CIL comparison, behavioral mutants, malformed/forged controls, frozen verifier or justified independent supplement, public member resolution, exact method/metadata isolation, reproduction and inherited regressions. No Unity/Xcode export or IPA has been produced by this checkpoint.

| Batch | Parent SHA-256 | Candidate SHA-256 | Methods |
| --- | --- | --- | --- |
| 73 | a830ce9a66331c669cdc7d77e191855b2409a2f2ef9f6aa1448f0e6dadfdd0dc | 47df498caf8fd0d02a494c995654c972399e98d2d0614fa25faaf7036b125f43 | 3 |
| 74 | 47df498caf8fd0d02a494c995654c972399e98d2d0614fa25faaf7036b125f43 | 71c28b1cad9ff8e5dd456121c2b0747792c3281f66c3da701002e91d9e3b9f4c | 4 |
| 75 | 71c28b1cad9ff8e5dd456121c2b0747792c3281f66c3da701002e91d9e3b9f4c | c2e0f13211a981b0fa9158cef09cc468e051ca2822ad7017f3450c87f5add725 | 4 |
| 76 | c2e0f13211a981b0fa9158cef09cc468e051ca2822ad7017f3450c87f5add725 | 9816bf53598fdbc2a63f95fadb6b2b86873561181d4ac3a7acc44ede36d25c1c | 4 |
| 77 | 9816bf53598fdbc2a63f95fadb6b2b86873561181d4ac3a7acc44ede36d25c1c | 290767e0d4e513612c56b3d06cff3edf0894e58808a6338d6c6b87019a2a3f83 | 2 |

## Original evidence

PC `GameAssembly.dll`: 31,969,792 bytes, SHA-256 `9ebd7ca996a5b03fb4a766f7a2502b660d581ddbbb36af4d7bf2f06a211da39d`. Original `global-metadata.dat`: SHA-256 `ad992341add498bd1018ac980f42171e0737b0793bf945c8714ad70a445b36b9`.

Each checkpoint includes exact native disassembly and bounded slice hashes from the authoritative PC mapping, native constant/literal provenance, and current-parent CLR metadata. The embedded managed body in `native-methods.json` is historical Native54 evidence, not the current parent. Use each batch's `metadata-current*.json` for current method bodies, local signatures and references. Native73 also records read-only inspection of `SkillManager.Awake149`; that method was not implemented or counted.

Native73 independently maps the InputLegacy CodeGenModule at `0x181992D90`, method pointers at `0x181CC8FB0`: public `UnityEngine.Input.GetAxis(string)` RID 22 maps to `0x1813645C0`, the exact missing call target in both camera bodies. The internal InputUnsafeUtility wrapper shares the pointer; the restored binding deliberately uses the original public Input API.

## Method contracts

Native73:

- `0x0600000F Admin_system2_boardEdior.Update` and `0x06000010 Update_Camera`: call GetAxis in the order `Mouse ScrollWheel`, `Horizontal`, `Vertical`. Native float32 math is `x=(horizontal+scroll*-400)*400`, `y=vertical*400`; read deltaTime once and multiply x/y by that captured value. Z is the actual multiplication `deltaTime*+0`, retaining negative zero/NaN behavior. Get Camera.main, then Component.transform, then Translate by value. Update calls Update_Mouse only after successful Translate; Update_Camera never reads this. Three damaged, otherwise unused object scalar locals in each body are recovered through two new scoped signatures, retaining all other local definitions and old signature rows.
- `0x0600039B Plant.PlantProduct`: only ID==1 proceeds. Capture GetAnimationSpritePosition(animationSprites,"SunFlower_head1") before reading board.projectManager; CreateProject(1,0,pos); then SunSet using the native SSE `cvttss2si` conversion of current attackPoint. NaN, infinities and out-of-range values map to Int32.MinValue; finite in-range values truncate toward zero. Retain SetEndPosition<Plant>, SpeedStart, then reread order and call PC_SunflowerHeal when order>=1. The returned Project is captured across callbacks.

Native74:

- `0x0600027C ZombieManager.PlaceNewZombie`: capture CreateNewZombie's return, Unity truth guard, current board.boardConfig.GetZombiePosition, TeleportTo(position,+0,false), grid fields, waitingTime bits `3C23D70A`, CreateStartPrePath, then wave=-1/enemyPoint=0. Do not move the final writes ahead of the path callback.
- `0x06000208 ProjectManager.TrySpawnLevelAward`: Unity-test current board, reread/capture it for isFinished and TestWinTargetZombie; reread board again for GameFinished. Then CreateProject(2,4,input position), SetEndPosition<Zombie>, return true. False guards return false; called-helper/null failures retain their timing.
- `0x06000173 BoardManager.CreatSettlementScreen`: capture instantiated controller; reread settlement list and use public typed Add, then Settlement(input config,current board.reward), PopupSet(current challenge/level/save), Awaken, return captured controller. The Add occurs before later failures.
- `0x060005CE BoardPreview.Return_PrepareGameScreen`: Unity-test prepare controller then reread to activate its GameObject. Three separate Camera.main calls: first capture position, second set the copied position with only x overwritten by +0, third set orthographic size to 540. Hide this GameObject last; preserve y/z bits and exceptions.

Native75:

- `0x060002B5 Board.ButtonDown_Pasue`: failed board returns; otherwise clip[11], camera position captured before AudioVolume, CreateAudio, activate pause, Log("GamePause"), write gamePause=true, set timeScale=+0, in that order.
- `0x060002B7 Board.CreateFlagMeter`: instantiate and store flagMeter before Initialize; reread flagMeter after callbacks. Capture its transform before getting the UI parent's transform, SetParent(false), reread its transform for local position (-195,+0,+0), then reread to deactivate its GameObject.
- `0x0600056F Almanac_PlantDetail.Update`: ordered old countdown>0; capture the old countdown before deltaTime and store the subtraction result. Keep that computed result across GetComponent/get_localPosition callbacks. Scale by 4 and apply the native ordered clamp, keeping NaN; compute `(animtion_contentStartY-+0)*t+ +0` with float32 boundaries. Retain copied x/z, reread the second RectTransform, set position, then box and log the actual updated Vector3. Do not reread countdown after the callback or remove logging.
- `0x060006DB SkillButtonController.PreView`: image_lock captured before LoadSkillLogo. False uses (0,"Lock"), RGBA (1,1,1,1), selected active=true; true uses (-1,"Lock_open"), RGBA (1,1,1,0.6), active=false. Reread image_lock for color and image_select for activation after prior callbacks. Alpha bits are `3F19999A`.

Native76:

- `0x06000039 Device_boardEditor.LoadData`: capture PreDevice.data, portrait list and its ID before assigning to captured image. Count>signed ID selects get_Item; IDs below zero can still throw, while Count<=ID assigns null. Capture boardConfig before Component.transform callback; then reread/capture PreDevice for both grid coordinates, get center position before assigning to the captured transform.
- `0x06000322 Device.DestroyThis`: Unity inequality-test UI_HP and reread it for Destroy; capture Grid, old passability, RemoveDevice, new passability. Reread board/device list and typed Remove ignoring bool. Only changed passability triggers AllFindPath on reread board/zombieManager. Destroy own GameObject last.
- `0x060006D4 SettlementUIController.NextLevel`: challenge==0 alone increments current gLawnApp.level unchecked. Manager getter/list are reread for Remove and Count; Count<=0 alone calls Pass through a fresh manager getter. Destroy own GameObject last. No early this-null guard.
- `0x06000703 Window_Q.DeletePlayerSave`: initial negative info index returns. Capture SavesUI through MainUI singleton, use correct original Unity equality test against null, capture list and index for Count/get_Item. Count<=index returns. Only a Unity-truth tile with non-CLR-null manager performs DeletePlayerSave_ID then fresh MainUI.LoadData. Valid selected entries without a usable tile/manager still destroy the window. A callback changing index to negative after the first guard must retain get_Item's exception; do not add another negative guard.

Native77:

- `0x060003D9 Projectile.Update_MoveTrack4`: livingTime<=1.5 ordered enters formula; NaN enters the reset branch. Reset writes only speed.x=1500 and zSpeed=+0, calls Update_Tracking, then writes movementTracks=0. Formula retains Math.Log(10d) converted to float32, real Single.ToString and Concat/Debug.Log("a="...), then computes 95-fZ and logs "d=". Reread livingTime after the second log. Preserve the separate float32 denominator `(livingTime*6)*a+a`, numerator `d*6`, division, zSpeed write, Update_Tracking. Do not clear speed.y/z or remove diagnostics.
- `0x06000491 Zombie.ZC_IVH_Skill1_Boxing_step2`: capture GetPlantingPosition before acquiring particles; Create particle state 13 and capture it across Unity truth callback. Set its transform position by value before GetComponent<ZombieAnimationEvent> and zombie field assignment. Public particle singleton getter is asserted to be an unchanged branch to ldsfld followed by ret.

## Isolation and restart

Native73 appends one GetAxis MemberRef, three exact original #US strings if absent, and two scoped local signatures. Native74 appends one typed settlement-list Add. Native76 appends five typed collection references and two signatures with additional typed capture locals. Native75/77 change no metadata or local signatures. Every construction writes only listed MethodBody slots plus explicitly described append-only metadata/PE relocation fields. This construction description is not an independent isolation verdict.

After /tmp loss, restore parent72 from Release `native72-pending-2026-10-12`, archive SHA-256 `76831fae19bfc91c1ae5b254034920cf5e9283416eb3d045d4132f8b8f23b39e`, and then this checkpoint's candidates/bin/source. Candidates, pinned parent72, patcher binaries/dependencies, helper source, generation logs and manifests are all archived. Work in ordinary copies, never edit reference inputs or archived candidates. Build logs from failed implementation attempts are retained; none are acceptance-test evidence.

If a confirmed ancestor implementation defect is found, repair it and regenerate/retest every descendant against newly pinned hashes. Do not mutate an already published candidate or reuse an old PASS against a new hash.

## Test-report review still open

Native60 v3 has actual 8,192 native/CIL matches and five behavioral controls with concrete first differences, plus reported independent structural/member/history gates. Its v5 external-dependency document gives a contradictory original GameAssembly hash/size (`485ca0...`, 23,347,200 bytes) for a path that the report binds to authoritative `9ebd7...`, 31,969,792 bytes. Gemini must demonstrate the actual oracle-process input and correct the dependency record before promotion; no broad rerun is needed for a documentation-only correction if the existing executions are already hash-bound.

Native61 v3 claims full completion, but Native61Fixture_final.cs has a few standalone assertions, does not read the native TSV and provides a behavior mutant only for 0x376. Its 4,096 native cases do not establish 4,096 candidate-CIL matches. Gemini has been asked for genuine per-case comparisons for all four methods and relevant wrong-variant/error/alias cases. Archive-v4 was named in the report but absent from the local folder at review time. No promotion was made.

Native65's frozen checker does not support ldftn. Keep `verify_types.py` SHA-256 `5175b68ae0cb9366a493499b566a7430dc1d8986d81cf17be91e0640974dd47a` unchanged and require an independent legal-delegate supplement with real valid/invalid CLR execution and signature/receiver/callback controls. Do not rewrite legal candidate IL to avoid the checker limitation.

The last full compiler measurement remains Native53: 516 errored methods, 5,668 diagnostics, 24 failed translation units. Pending method counts must not be subtracted from it. Full IL2CPP/all-TU work remains deferred to the accepted-method threshold; qualification and actual Unity/Xcode/unsigned IPA remain separate gates.
