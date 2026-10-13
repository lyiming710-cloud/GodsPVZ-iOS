# GodsPVZ Native78–82 implementation checkpoint — 2026-10-13

This is an unqualified implementation snapshot. Native59 remains the accepted base, SHA256 `d198bb3f63a21d95d708a66546b3b12529d52aee810508712db6e32f43b021c0`, with **40/100 qualified unique methods since Native53**. Native60–77 already contained 61 pending unique methods; the five batches here add **11** unique methods, bringing the pending total to **72**. Accepted plus pending implementations total 112. Implemented methods are not qualified methods.

The integrator read source/native/metadata evidence, compiled patch generators, generated candidates, and archived this checkpoint. **The integrator executed no acceptance tests.** Real native/CIL comparisons, valid behavioral mutants, frozen Native40 verification, member-access checks, body/metadata isolation, regressions, conversion and compiler gates remain assigned to `GodsPVZ — AGY Flash 协作验证`, thread `01a12e9a-49e8-71c1-acf0-a634ac5215af`, using AGY Gemini 3.8 Flash.

The original thread messaging interface resumed working on 2026-10-13. Dispatches for Native78–82 and the outstanding Native60/61 evidence corrections were successfully delivered. Direct collaboration-provider AGY spawning still returned `unreadable_encrypted_agent_task`; this was a separate failed route. Default-model source-analysis agents performed read-only specification work, not tests.

## Candidate lineage

| Batch | Parent SHA256 | Candidate SHA256 | New target methods |
|---|---|---|---|
|78|`290767e0d4e513612c56b3d06cff3edf0894e58808a6338d6c6b87019a2a3f83`|`de2f98d4e793b91fba17ba7eff3aeba3130282f5e477f0daae1aa358287868f7`|2|
|79|`de2f98d4e793b91fba17ba7eff3aeba3130282f5e477f0daae1aa358287868f7`|`3b3bcfca51402e1eedf5d59fbe1bf9d923336730325550205f960bd8a8bceec6`|3|
|80|`3b3bcfca51402e1eedf5d59fbe1bf9d923336730325550205f960bd8a8bceec6`|`119c847000d84922e5089cfed3bb464366251aa05597f4c7fa22706368211950`|2|
|81|`119c847000d84922e5089cfed3bb464366251aa05597f4c7fa22706368211950`|`5b143bae30c9747fa7beab7d92c580b8c556523857fd35b0e4bb1dab0164ddae`|2|
|82|`5b143bae30c9747fa7beab7d92c580b8c556523857fd35b0e4bb1dab0164ddae`|`48b9e89e941c041d6c11c4064ad05d9384a1a4dd51d5f8d3fad9192cbda5adbf`|2|

Parent77 was restored from the immutable Release `native73-77-pending-2026-10-12`; `/tmp` had reset. Its archive SHA256 is `7efd9c78c6eca2f7cc9eedd71dae59fd60dc84c57e3fbbca353c3dd58ca10be7`. Its root source commit is `2608e3bda00c2bb5212b160a2184d15835892e2b`. Restoration reads `/tmp/godspvz-native77-restored-2026-10-13/snapshot/candidates/native77/candidate1.dll`. New generated files live at `/tmp/godspvz-nativeNN-codex-2026-10-13/` until an ephemeral reset; use the new Release archive for durable recovery.

## Source-backed behavior and edit scope

| Batch / MethodDef | Method | Required behavior preserved in implementation |
|---|---|---|
|78 / `0x0600061D`|GridConfig_A.SetGridState|Original usage cells prove typeof GridState versus boxed Int32. One GetTypeFromHandle; invalid ID logs exact original message and continues with state zero. Unity system comparison precedes a fresh system read and raw-null fault. No gridState field assignment.|
|78 / `0x06000483`|Zombie.TranToAttack|Three targetPlant reads across Unity comparisons and old-plant callback; preserved input new plant callback. No invented targetPlant assignment. Tail is Animator.SetBool, ResetAttackSpeed, public Vector3.zero getter, isEat=true. Exact getter IL reads original private zero field.|
|79 / `0x06000238`|SavesManager.BackupPlayerSave|Allocate/construct Save before Initialize. Copy current source, reread source afterward for name plus exact suffix `-备份`. Typed List<Save>.Add. SavePlayerSave before fresh public MainUI singleton lookup/LoadData.|
|79 / `0x06000365`|Plant.Explode|ID2 only. Captured particle; first transform getter, fZ+fY rounded Single, transform raw-null check, then fX. By-value vectors; second getter on same particle. Scale1.2, Shake0.15/8, GetDamage(null,-1,0), discard AreaDamage Boolean, captured skill after callback, then PlantDie.|
|79 / `0x0600075F`|ObjectSpin.Awake|Typed Quaternion local passed by address to proven public Euler getter. Capture second transform, write rotation before position failure. Captured Light Unity comparison; black Color(0,0,0,1) fallback, existing Color32 conversion.|
|80 / `0x060003A0`|Plant.SetBrightness|Typed GameObject enumerator with real finally, GetEnumerator outside try, Dispose on normal/exception exit. animationSprites captured once; Mat_Brightness reread each iteration. Raw-null faults; no clamp, brightness write or Unity fake-null skip. Material getter qualification limitation below.|
|80 / `0x06000206`|ProjectManager.PlayAudio_Click|AudioVolume before project null fault; one projectType read and original nine-case dispatch. Exact float32 volume/pitch cases, captured Camera/position before current clip-list read. Four-argument CreateAudioAtPoint; discarded return.|
|81 / `0x060001AC`|EnemySelecter.ZombieSelectRandomGet|Float Random before null/GetEnumerator, typed ZombieSelect iterator, ordered greater-than to subtraction with unordered/NaN selection, second weight read, float32 per step, real finally on selection/exhaustion/fault.|
|81 / `0x060003C5`|Project.GetEndPosition|Track0 partial x write before Random, preserve callback-modified x/z, camera revision by value, then independent fresh track4 test. Original ID switch and helper selection; y offset write before x offset read/write.|
|82 / `0x0600087D`|SwfManager.GetInstance|Two separate Unity truth tests, search even if creation is disabled, fresh singleton read at every return. Constructor before AddComponent; helper results overwrite callback changes only after helper return.|
|82 / `0x06000734`|TMP_TextEventHandler.Awake|Original native leaf proves exact Type reference equality, not IsSubclassOf. Separate gameObject reads, text reread, canvas rereads after Unity comparison/renderMode callbacks. False canvas comparison preserves prior camera.|

Native78 and Native82 keep all metadata and local signatures unchanged by construction. Native79 adds one public Quaternion MemberRef and one scoped Explode local signature, appending Single V11. Native80 adds only a scoped local signature retyping V10 to Enumerator<GameObject>; V12 and all other original locals remain unchanged. Native81 adds three public typed iterator MemberRefs and one scoped signature appending Enumerator<ZombieSelect> V16.

Native81's Project body is 331 bytes, exceeding its 180-byte old code slot. The existing Native40/43 raw relocation pattern appends a complete fat MethodBody within the current final readonly `.n29meta` section and updates only Project's four-byte MethodDef RVA. It preserves the old Project slot, its local signature, all other method RVAs and the active MetadataDirectory. The generation report records exact row offset, new RVA and PE size changes. This is a construction report; Gemini must independently reopen and verify the scoped metadata and all non-target bytes.

## Material getter qualification limitation

Original PC Renderer.get_material and private GetMaterial map to the same native pointer `0x1812FAE70`. The exact full Editor Core DLL, SHA256 `83192116b34abac2bb1797ec0e3943362be33b028b1eefd2e561d5fdc1c3312f`, has an extra IsPersistent/LogError/null branch. IsPersistent has no NativeConditional attribute; do not invent an iOS constant-false proof.

The complete historical Native18 iOS Xcode archive, SHA256 `651bf0da8300107c5ef99173493ed503b9f1f4e681bff3949f510adb3088495f`, contains actual UnityEngine.CoreModule.cpp SHA256 `ae9945e6cc3038b1e70082b8f811e1c3708d6bfebe6ba5c209e7acd9bdb4e7b8`. In that identified export, public Renderer_get_material calls private GetMaterial on the same receiver and returns it; four Core source units have no Renderer_IsPersistent identifier. This supports that historical player helper. Binding the **current target** player/linked Core and helper remains required; the fixture must not conceal a real helper difference behind a stub. Native80 remains pending.

## Validation ledger and next work

Native60's earlier dependency records were corrected, but its actual historical oracle binary/source/input/result binding still required explicit review. Native61's newer fixture genuinely executed 4096 CIL cases; outstanding work includes independent device/countdown/list native columns, a valid executable set_Item behavioral mutant, and preserved callvirt receiver-null semantics. Messages with these concrete findings were delivered to the original Gemini thread. Versioned packages and unsuccessful attempts must remain inspectible. No Native60+ acceptance is asserted by this checkpoint.

Complete conversion/all translation-unit work remains deferred until **100 independently qualified unique methods since Native53**, not 112 implementations. The last full compiler measurement remains Native53: 516 errored methods, 5668 diagnostics, 24 failing translation units out of264. It is historical; no new residual-error count can be derived by subtracting generated methods.

No GitHub Actions workflow, licensed Unity/Xcode build, IPA export or device test was initiated by this checkpoint. The deliverable remains an unsigned IPA; none has been qualified. Do not change main, broaden the frozen verifier, overwrite published candidate hashes, skip native callback/alias/exception observations, or convert missing evidence into an OK verdict.

The Release snapshot includes source projects, candidates78–82, pinned parent77, generator binaries/runtime files, build/generation logs, current metadata inventories, original-source specifications/helper evidence, a file manifest and pipeline ledger. Original PC inputs and large historical Xcode archives remain at their existing registered archive/Release locations.
