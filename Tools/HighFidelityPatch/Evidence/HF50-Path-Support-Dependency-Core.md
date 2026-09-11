# HF50 — Path Support Dependency Core

Status in this evidence commit: **technical recovery gates passed; Google Drive final closure and `Recovery/STATUS.md` advancement are still pending.**

HF50 deliberately does **not** patch `Zombie.Path_Test()`. The native investigation of that live path showed that several direct support methods were themselves concretely mis-reconstructed. HF50 closes a small five-MethodDef dependency set first, preserving the project rule that no broad gameplay path is rewritten before its dependencies are independently native-backed.

## Formal input

- HF49 cumulative input SHA-256: `6dff7975abd2b62d2cc40564a17f21518526d8dae2ce7f53a7be3e49c2114f69`
- Assembly-CSharp MethodDef count: `2317`
- Original PC ZIP SHA-256: `2f08b4e2243e2bd296f3db694b4a2c9656252d76202c80167bdf3340b9f65b48`
- Original PC `GameAssembly.dll` SHA-256: `9ebd7ca996a5b03fb4a766f7a2502b660d581ddbbb36af4d7bf2f06a211da39d`
- Original PC `global-metadata.dat` SHA-256: `ad992341add498bd1018ac980f42171e0737b0793bf945c8714ad70a445b36b9`
- Unity `2022.3.44f1c1`; metadata `31.1`

## Authorized targets only

| Token | RID | Managed method | Original PC native |
|---|---:|---|---|
| `0x06000141` | 321 | `GlobalStaticVars.GetAnimationSpritePosition(List<GameObject>, string)` | `0x18031B680` |
| `0x06000201` | 513 | `ProjectManager.CreateProject(int, int, Vector3)` | `0x180323050` |
| `0x06000299` | 665 | `Grid.FindDevice_Occupy(OccupyState)` | `0x18032A0A0` |
| `0x060002D0` | 720 | `Board.TestWinTargetZombie()` | `0x180328000` |
| `0x06000484` | 1156 | `Zombie.TranToStant(float)` | `0x18036A5E0` |

Ordinary native attribution uses original MethodDef RID-1 to the Assembly-CSharp CodeGenModule `methodPointers[index]`. None of the five HF50 targets is generic.

No other MethodDef is authorized by HF50.

## Recovered behavior

### `GlobalStaticVars.GetAnimationSpritePosition`

Original behavior is `GetAnimationSprite_Name(animationSprites, childName)`, Unity-object null test, return that object's `transform.position` if present, otherwise `Vector3.zero`.

### `ProjectManager.CreateProject`

Original behavior is:

1. fetch `projectPrefabs[ID]` and apply the original Unity-object truth test;
2. return null if the prefab is absent;
3. instantiate `projectPrefabs[ID]`;
4. set `movementTracks`, world `transform.position`, and `board`;
5. for nonzero IDs, append to `projects` and parent under the ProjectManager transform;
6. for ID 0, append to `board.sunManager.suns` and parent under the SunManager transform;
7. return the created project.

No defensive bounds or null guards were added.

### `Grid.FindDevice_Occupy`

Original OccupyState mapping is restored exactly:

- `Bottom -> device_bottom`
- `Cover -> device_sheath`
- `Normal -> device_common`
- `Floating -> device_top`
- `Ladder -> device_ladder`
- other/default -> null

### `Board.TestWinTargetZombie`

If the Unity-object `enemyManager` exists and `enemyManager.finish` is true, enumerate `zombieManager.zombieList`; return false on the first `Zombie.IsWinTarget()` result, otherwise true after enumeration. If the finish condition is not active, return false. The generated managed body retains normal `List<Zombie>.Enumerator`/Dispose semantics.

### `Zombie.TranToStant`

Restored native behavior:

- assign `waitingTime`;
- set `rSpeed = Vector3.zero` and `isStant = true`;
- if `ID != 13`, force localScale.x positive with `Math.Abs` while preserving y/z;
- for IDs `0,2,4,5,14,15`, or `IsPlantZombie()`, set animator integer `"Group"` to `Random.Range(0,2)`;
- set `Ani_StantHash` true;
- call `ResetIdleSpeed()`;
- if `ID == 17`, `waitingTime > 0`, and `brokenLevel < 2`, set animator `"Ready"` true.

## Explicitly out of HF50 scope

HF50 does not claim repair of:

- `Zombie.Path_Test()`;
- `Board.GameFail()`;
- `Zombie.DestroyZombie()`;
- `Zombie.DropLootPiece()`;
- `ProjectManager.DropLootPiece(Zombie, Vector3)`;
- `Project.SetEndPosition<T>()`;
- `Project.SunSet(int)`.

`Project.SetEndPosition<T>()` is generic and cannot be promoted under the ordinary MethodDef RID-1 attribution rule; it requires original MethodSpec / generic-method-function attribution first.

## Patcher build and deterministic application

- HF50 patcher build head: `865c7539a42dc9a5c7f1016430e16f094ace4983`
- GitHub Actions run: `34552198504` — PASS
- patcher artifact ID: `10181225959`
- artifact digest SHA-256: `9f5cd1779bc44fa3239d556384db38dfa8e56cc851ba241e0a8031d65b100a20`
- patcher hard-locks formal HF49 input SHA before mutation and validates all five tokens/type names/method names/parameter counts.
- two independent formal patch executions both exited 0 with stderr 0 and produced byte-identical output.
- HF50 candidate SHA-256: `74ae15e2ed7f1626ecea7741d83803208ea0e8381306f1900f7a46c198c425cb`

Cecil reopen results:

- `0x06000141` — 14 IL / 35 bytes / 0 EH
- `0x06000201` — 54 IL / 157 bytes / 0 EH
- `0x06000299` — 27 IL / 77 bytes / 0 EH
- `0x060002D0` — 35 IL / 94 bytes / 1 EH
- `0x06000484` — 93 IL / 264 bytes / 0 EH

All five target bodies reopen successfully and none contains a Cpp2IL helper call.

## Fixed ILSpy readback

- ILSpyCmd / ICSharpCode.Decompiler: `11.0.0.9375`
- fixed 56-DLL reference ZIP SHA-256: `fe091e5a5389c2491eebeff72c83c394097bef67ffd45a25cf7de01c9aad9ef1`
- formal extracted reference directory: `hf15_refs_repro`
- all five target type decompilations: exit 0, stderr 0
- extracted target-method markers: Cpp2IL 0; Unknown result type 0; Expected-type 0; invalid comparison/type 0; NotImplemented 0.

An earlier invocation pointed `-r` at the reference set's parent directory instead of the actual 56-DLL `hf15_refs_repro` directory. That invocation was discarded and is not evidence against the candidate; every formal target readback used the exact nested directory.

Whole-assembly IL:

- HF49 whole IL reproduced SHA-256: `c60eb750651402273ec5cc7ac98520c500dd361568cd2214c42518ed7a737455`
- HF50 whole IL SHA-256: `0e9929692d9627b8a8e0f1261293ee2bb9cc69ed346165e96458e9ff544c6df3`
- both whole-IL stderr files are 0 bytes.

## Whole-assembly semantic isolation

HF49 -> HF50:

- MethodDef blocks: `2317 -> 2317`
- methods with emitted bodies: `2297 -> 2297`
- distinct nonzero body RVAs: `2140 -> 2140`
- normalized non-method/method-signature skeleton is byte-identical
- normalized skeleton SHA-256 on both sides: `489fcd6bc8106049bc3780de1ac22cc9b691040e20bcbb1faac90a70b3999ca7`
- changed normalized MethodDef blocks are exactly the five authorized methods and no others.
- HF49->HF50 semantic diff SHA-256: `229de8a6ab43aa502d97f267b66b1bd6748e2ed35cf34732985bea59fddaf0b8`
- candidate-specific 2317-entry Cecil MethodDef table SHA-256: `b5aa2d0216863d128d286aea4b3ee537ebcadc496e1c0ac1178d676347743dce`

## Cumulative RecoveryAudit

- HF50 cumulative audit build head: `27a73fd974f66b18fc3557fa0df27aa0484ecb15`
- GitHub Actions run: `34552873703` — PASS
- audit artifact ID: `10181455848`
- artifact digest SHA-256: `271023744274706414e37c069c395c5c959ee66aea18ba07781c1ff9f41e43e6`
- the retained historical auditor remains unchanged and covers HF1-HF46.
- the HF50 cumulative auditor locks the complete HF50 candidate SHA and rechecks HF47 + HF48 + HF49 + HF50, totaling 15 late-stage targets.
- two independent actual composite executions both exited 0 with stderr 0 and produced `RECOVERY_AUDIT_OK` plus `HF50_AUDIT_OK`.
- the two complete audit logs are byte-identical; log SHA-256: `53ad11fe22d0fbcb91a0cf999708d5293f1df37b2f654da38388add9c0d86bf0`.

## Fresh residual scan

The five HF50 targets are clean. Important remaining live-path damage is intentionally not hidden:

- `Zombie.Path_Test()` remains live and concretely mis-reconstructed;
- `Zombie.DestroyZombie()` remains reconstruction-damaged;
- `Zombie.DropLootPiece()` remains reconstruction-damaged;
- `ProjectManager.DropLootPiece(Zombie, Vector3)` remains reconstruction-damaged;
- generic `Project.SetEndPosition<T>()` requires separate original generic attribution.

These observations are candidate signals only, not authorization for an HF51 patch. Any next managed stage must independently satisfy live reachability, original-PC-native attribution, and behaviorally closed dependencies.

## Current gate

**HF50 technical recovery gates: PASS.**

Do not mark HF50 formal-final until the standard Google Drive 19 ordinary payload + payload manifest + FINAL pair provider-readback closure reaches exactly 22 files and `Recovery/STATUS.md` is advanced afterward.
