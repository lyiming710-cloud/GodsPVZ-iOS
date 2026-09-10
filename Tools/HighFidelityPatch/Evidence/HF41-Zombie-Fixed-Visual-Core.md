# HF41 — Zombie Fixed Visual Core

Date: 2026-09-10  
Branch: `high-fidelity`

## Scope

HF41 restores exactly four original `Assembly-CSharp` MethodDefs and no others:

- `0x06000128` — RID 296 — `ElementManager.GetElementColor()` — PC `0x180314F20`
- `0x06000425` — RID 1061 — `Zombie.Update_Color()` — PC `0x18036BEB0`
- `0x0600042D` — RID 1069 — `Zombie.FixedUpdate()` — PC `0x180360080`
- `0x0600042E` — RID 1070 — `Zombie.FixedUpdate_BGM()` — PC `0x18035FD10`

Formal input is the re-fetched HF40 cumulative final:

`726c7ceda476cb0034b9394964407a72d2ab7a1a92424dee9c81e118f5590cce`

Accepted HF41 cumulative candidate:

`2e03010cab5a4776243ecf3509402f387c39ccb9faa039876c85ad629ba7fcfa`

This file records an accepted candidate only. HF41 is not formal until the Drive 20+2 closure is provider-read back as exactly 22 files and `Recovery/STATUS.md` is advanced afterward.

## Original-native attribution

Fixed original PC baseline was re-fetched and re-hashed before attribution:

- PC ZIP SHA-256 `2f08b4e2243e2bd296f3db694b4a2c9656252d76202c80167bdf3340b9f65b48`
- PC `GameAssembly.dll` SHA-256 `9ebd7ca996a5b03fb4a766f7a2502b660d581ddbbb36af4d7bf2f06a211da39d`
- PC `global-metadata.dat` SHA-256 `ad992341add498bd1018ac980f42171e0737b0793bf945c8714ad70a445b36b9`
- PC CodeRegistration `0x1815E88C0`

The original CodeRegistration contains 56 CodeGenModules. The original `Assembly-CSharp.dll` CodeGenModule reports 2316 method pointers. Ordinary attribution used only the original MethodDef RID-1 index into this original module's `methodPointers` table; patched-DLL MethodDef order and Cpp2IL executable-body order were not used.

That direct mapping resolves the four HF41 targets to the PC addresses listed in Scope.

## Accepted native behavior

### `ElementManager.GetElementColor()`

PC `0x180314F20` initializes the output to `Color(1,1,1,1)`.

For the Zombie path, the manager's Zombie field is tested with Unity `Object` inequality semantics. Its `fire_ice` element is a plain managed reference. Only `point > 0` enters the color transform. Native `UCOMISS/COMISS` branches make unordered/NaN fail that positive test. The scalar is `Ceiling(point / 1000f) * 0.05f`, then clamped to `[0,1]` by ordered comparisons. RGB becomes `r = 0.5 + t * -0.5`, `g = 0.8 + t * -0.8`; blue and alpha remain 1.

For the Plant path, the Plant field uses Unity `Object` inequality semantics, while its element is a plain reference. It additionally requires `Plant.GetElementPreference(...) != 1` and `Plant.ID != 6`. Its scalar uses the same ceiling and clamp. RGB becomes `r = 0.2 + t * 0.1`, `g = 0.88 + t * -0.58`; blue and alpha remain 1.

The accepted CIL preserves the native unordered behavior with unordered-aware branches and uses `MathF.Ceiling`; it does not normalize NaN or Unity Object semantics.

### `Zombie.Update_Color()`

PC `0x18036BEB0` snapshots the Zombie's current RGBA. If `ashes` is false, it calls `ElementManager.GetElementColor()` and multiplies the saved Zombie RGB by the returned RGB; if `ashes` is true, RGB becomes zero. The saved Zombie alpha remains the material alpha source.

The native routine iterates `animationSprites` through a real `List<GameObject>.Enumerator`. Each current GameObject resolves `GetComponent<SpriteRenderer>()`; the SpriteRenderer is tested with Unity Object equality semantics. For a non-null renderer it writes the computed RGB while preserving that renderer's existing alpha, obtains `renderer.material`, and calls `Material.SetFloat(Zombie.Mat_Alpha, savedZombieAlpha)`.

The normal and exceptional enumerator exits both dispose the enumerator. The accepted CIL therefore retains a `.try/finally` with `Dispose`; it does not flatten the loop. After the loop it resets `Zombie.color` to `Color(1,1,1,1)`.

### `Zombie.FixedUpdate_BGM()`

PC `0x18035FD10` tests `audioSource_BGM` with Unity Object implicit truthiness and returns immediately when false. Default target volume is `GlobalStaticVars.AudioVolume()`. Zombie ID 17 multiplies this by 2.5.

Zombie ID 18 enters the special cross-fade only when `board` is a valid Unity Object. While not dead, the Zombie BGM moves upward toward `GlobalStaticVars.BGMVolume() * 1.5`, while `board.audioSource[0]` is reduced toward zero. While dead, Zombie BGM moves toward zero and `board.audioSource[0]` is restored toward `BGMVolume`. Step factors are the native `BGMVolume * 0.02` multiplied by 0.4 or 0.5 as appropriate. The native array dereference/bounds/null behavior is retained; no defensive managed guard was added.

All native floating comparisons use branches that skip adjustment on unordered/NaN. The accepted CIL uses corresponding unordered-aware branch forms. Final assignment writes the computed volume to `audioSource_BGM.volume`.

### `Zombie.FixedUpdate()`

PC `0x180360080` calls, in order:

1. `Update_Color()`
2. `FixedUpdate_BGM()`

Only afterward, when `isOnBoard && board.gameStart && isDying && !isDied`, it subtracts `Time.fixedDeltaTime * 100f` from `healthPoint` and calls `InjuryStatusUpdate_Body(false)`. The board dereference keeps the original null/exception behavior rather than adding an iOS-oriented guard.

All project helper dependencies touched by these four bodies were closed against original attribution or previously formal recovered methods. No unexplained native helper was deleted or substituted.

## Deterministic patching

Accepted published HF41 v2 patcher build head:

`0984f986c6d96570c95d6f488753de8e3d3fbc02`

Workflow run: `34435893128` PASS  
Artifact ID: `10136146571`  
Artifact ZIP SHA-256:

`e53af852ecd3242df3acc1ec8f15abf52f3ba81fa1f6f9de293a28a313bc36cf`

The patcher hard-locks formal input SHA-256 to HF40 `726c7ceda476cb0034b9394964407a72d2ab7a1a92424dee9c81e118f5590cce` and hard-locks the four target tokens.

Two independent applications of the published patcher to independently copied, SHA-verified HF40 input completed with exit 0 and zero stderr. The outputs are byte-identical at:

`2e03010cab5a4776243ecf3509402f387c39ccb9faa039876c85ad629ba7fcfa`

Cecil reopen:

- `0x06000128 ElementManager.GetElementColor/0` — `117 IL / 340 bytes / 0 EH`
- `0x06000425 Zombie.Update_Color/0` — `90 IL / 264 bytes / 1 EH`
- `0x0600042D Zombie.FixedUpdate/0` — `30 IL / 82 bytes / 0 EH`
- `0x0600042E Zombie.FixedUpdate_BGM/0` — `122 IL / 350 bytes / 0 EH`
- all four exceed their locked reopen floors
- no Cpp2IL helper or decompiler-issue marker remains in any target

## Fixed ILSpy member gate

The accepted fixed toolchain was re-fetched and re-hashed:

- ILSpyCmd / ICSharpCode.Decompiler `11.0.0.9375`
- fixed ILSpy archive SHA-256 `446bb884aa014ae04d6825ad6430e85dc1a827d2526075bbde754c9f85b3a1da`
- fixed 56-DLL reference ZIP SHA-256 `fe091e5a5389c2491eebeff72c83c394097bef67ffd45a25cf7de01c9aad9ef1`
- exactly 56 reference DLLs after extraction

Each of the four target members decompiled with exit 0 and zero stderr. Marker scans found zero Cpp2IL references, zero NotImplemented/decompiler-issue markers, zero invalid stack/type/comparison markers, and zero warning/error markers. Readback retained `Ceiling + clamp`, Unity Object operators, the `Update_Color` enumerator `finally`, renderer alpha preservation/material alpha, `FixedUpdate` call order, and the ID18 BGM cross-fade branches.

## Whole-assembly semantic isolation

Using that same fixed ILSpy toolchain:

- HF39 whole IL reproduced exactly at `23e2734766d033c4b22767fd9b969d8312261cedc24622a38bf6ff5dc5dcc54d`
- HF40 whole IL reproduced exactly at `fd43ae98ca25cc7a43f5265c76a924eaefb8fa8b4616575c6af914935fd0ff7e`
- HF41 whole IL SHA-256 is `48183cd6963c3bde3056e73d8983eeceea36574199b881077f76091bd458b4c0`
- all whole-assembly decompiles completed with zero stderr

Before computing HF40->HF41 isolation, the retained normalization/parser reproduced the accepted HF39->HF40 semantic diff byte-for-byte at:

`053387ddf76a053cc57cbca509bb72c653c5becea3d92e7c595be6a58b670d76`

HF40->HF41 results:

- MethodDef `2317 -> 2317`
- whole-IL distinct method blocks `2139 -> 2139`
- normalized non-method skeleton byte-identical
- normalized skeleton SHA-256 on both sides `afc47c7eaa314ef12143a7ac99ab0db9fcb484b045d76b2fc74d6f2c93ac3b88`
- changed MethodDefs exactly `0x06000128`, `0x06000425`, `0x0600042D`, `0x0600042E`
- HF40->HF41 semantic diff SHA-256 `8080c9fa957c43f70073275f44f9e81d5153670c563ca8bcbdf6e8629480873b`
- HF40 archived MethodDef table was reproduced byte-for-byte at `4e7eda66bf694343784da18909d0cb38eaddd8674ae08a866943cbfaac4be7d1`
- HF41 MethodDef table SHA-256 `302fe34407fd0f44774b11d096debaae13b057c52d133d76b4a121d9c2410fb4`

No fifth MethodDef changed.

## Permanent RecoveryAudit

RecoveryAudit was extended only after the native, deterministic, Cecil, fixed-ILSpy, whole-assembly and structural gates had passed.

RecoveryAudit commit:

`31a9f759fd0c08ef5cb41de4db57680a9849b876`

Workflow run: `34446634528` PASS  
Published auditor artifact ID: `10139898714`  
Auditor artifact ZIP SHA-256:

`149c9fac6d0a40c308f7142509756b2e8b6d85a2acd18dfcc4364ec8de9ddeae`

The published auditor was executed independently twice against the accepted HF41 candidate. Both process runs completed with exit 0 and zero stderr. Each process performed internal OPEN1 and OPEN2 over the complete retained historical target set and ended with `RECOVERY_AUDIT_OK`.

HF41 target readback in both OPEN1 and OPEN2 was stable at:

- `ElementManager.GetElementColor/0` — `117 IL / 340 bytes`
- `Zombie.Update_Color/0` — `90 IL / 264 bytes`
- `Zombie.FixedUpdate/0` — `30 IL / 82 bytes`
- `Zombie.FixedUpdate_BGM/0` — `122 IL / 350 bytes`

## Source provenance

Accepted published HF41 patcher source is pinned to immutable build head `0984f986c6d96570c95d6f488753de8e3d3fbc02`:

- `HF41Patch.csproj` blob `c33e7df4878ea275f2e349c10cac92c1836b1637`
- `Program.cs` blob `f919a6a1a718a01426691180914335fb92b6960b`
- `Template.cs` blob `c4be3e69384462e0cefa8a38fa38998d49cdaa5c`
- workflow blob `8b3ff051b72e4a1619d613c7f28be7d33432c58e`

RecoveryAudit source blob after HF41 extension is `77713e6ed8ee03e0f67102926ea417b8c8668f54`; its build workflow remains `.github/workflows/build-recovery-audit-tools.yml`.

## Closure state

All gates through permanent published RecoveryAudit are PASS. GitHub Evidence is now permanent. The remaining formal steps are the strict Drive 19 ordinary payload + payload-manifest pre-closure readback, the two final closure files, exact 22-file final provider readback, and only then `Recovery/STATUS.md` advancement. Until those complete, HF40 remains the only formal cumulative final and HF42 is prohibited.
