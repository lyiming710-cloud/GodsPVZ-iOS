# HF46 — EnemyManager Dependency Core

Date: 2026-09-10  
Branch: `high-fidelity`

## Scope

HF46 restores exactly two original `Assembly-CSharp` MethodDefs and no others:

- `0x060001BC` — RID 444 — `EnemyManager.PlayBoardAudio(int)` — PC `0x180318240`
- `0x060001BF` — RID 447 — `EnemyManager.TextWaveHealth()` — PC `0x180318CB0`

Formal input is HF45 cumulative final:

`a34feef56319f047a9f60caef0028db9592536b89fa484c41e371a826d6ccd5a`

Accepted HF46 cumulative candidate:

`900d3aabbf79b094a6b412f7d869cc5285ce206a2d17501b69fc3cd8651beefd`

This file records an accepted candidate only. HF46 is not formal until strict Google Drive 20+2 closure is provider-read back as exactly 22 files and `Recovery/STATUS.md` is advanced afterward. Until then HF45 remains the only formal cumulative input.

## Fresh residual scan and native attribution

HF46 was not opened automatically after HF45. Fresh post-HF45 scanning reconfirmed that `EnemyManager.TimeUpdate()` is called unconditionally by `EnemyManager.Update()` and still contains concrete managed reconstruction loss, but `TimeUpdate()` was not promoted directly because its live wave progression path depends on other damaged helpers. Dependency peeling identified `PlayBoardAudio(int)` and `TextWaveHealth()` as direct `TimeUpdate()` callees with concrete loss, material gameplay reachability, and behaviorally closable original-PC-native bodies.

`Zombie.SetUpdateRate()` and residual Projectile helper bodies were not promoted merely because they still decompile poorly; current call-site evidence did not satisfy the late-stage active-path gate.

Original PC baseline:

- PC ZIP SHA-256 `2f08b4e2243e2bd296f3db694b4a2c9656252d76202c80167bdf3340b9f65b48`
- PC `GameAssembly.dll` SHA-256 `9ebd7ca996a5b03fb4a766f7a2502b660d581ddbbb36af4d7bf2f06a211da39d`
- PC `global-metadata.dat` SHA-256 `ad992341add498bd1018ac980f42171e0737b0793bf945c8714ad70a445b36b9`
- PC CodeRegistration `0x1815E88C0`
- Assembly-CSharp CodeGenModule methodPointers table `0x181B82D60`

Ordinary attribution used original MethodDef RID-1 into the original `Assembly-CSharp.dll` CodeGenModule methodPointers table. The resolved pointers are `methodPointers[443]=0x180318240` and `methodPointers[446]=0x180318CB0`. Patched-DLL ordering and Cpp2IL executable-body ordering were not used.

Original `.pdata` boundaries used for these bodies are:

- `PlayBoardAudio`: `[0x180318240, 0x180318319)`
- `TextWaveHealth`: `[0x180318CB0, 0x180318EA9)`

## Accepted native behavior

### `EnemyManager.PlayBoardAudio(int)`

The original routine indexes `ResourceManager.boardClips[ID]`, then tests the resulting `AudioClip` through UnityEngine.Object truthiness. A false/destroyed clip returns. A live clip obtains `Camera.main.transform.position`, calls `GlobalStaticVars.AudioVolume()`, and then calls the previously restored `GlobalStaticVars.CreateAudioAtPoint(clip, position, volume, 1f)`.

The Camera/Transform dereference path retains the original exception behavior; no defensive iOS null guard was introduced. The previous managed reconstruction did not preserve the real Vector3 position path, so this is concrete reconstruction loss rather than cosmetic decompiler cleanup.

### `EnemyManager.TextWaveHealth()`

The original routine first stores `waveHealthRemainder = 0f`, then raw-dereferences `board`, `board.zombieManager`, and `zombieManager.zombieList`. These are preserved as ordinary managed-reference/null-exception semantics rather than Unity Object truthiness.

It enumerates the Zombie list with normal `List<Zombie>.Enumerator` cleanup semantics. A raw null Zombie element follows the original null-reference failure path. It includes only Zombies where `z.wave == theWave - 1` and `!z.isDying`, accumulating:

`healthPoint + armor1Point + armor2Point * 0.2f`.

For waves where signed `theWave % 10 != 9`, the original PC code converts the two float health values to double and returns `(double)waveHealth * 0.5 >= (double)waveHealthRemainder`. For remainder 9 it returns `0f >= waveHealthRemainder`. Native `COMISD/COMISS + SETAE` behavior means NaN/unordered returns false in both comparisons; that behavior is retained. The `foreach` reconstruction retains the original finally/Dispose semantics.

## Published patcher and deterministic patching

Published patcher build head:

`82ecf79f54815ac83d890988591e388f7341a812`

Workflow run: `34494163583` PASS  
Artifact ID: `10159056462`  
Artifact ZIP SHA-256:

`2dd4544959cae93d05cddfbbf85993d63c5eda32c0141529a3fdc87fbbcbe6a2`

After artifact publication, HF45 formal cumulative DLL Drive ID `19HEqSmIkz4PUt-SIhXWdmEwBy6UMu8so` was re-fetched and SHA-verified at `a34feef56319f047a9f60caef0028db9592536b89fa484c41e371a826d6ccd5a`.

Two independent applications of the published patcher completed with exit 0 and zero stderr. Outputs are byte-identical at:

`900d3aabbf79b094a6b412f7d869cc5285ce206a2d17501b69fc3cd8651beefd`

Cecil reopen:

- `0x060001BC EnemyManager.PlayBoardAudio/1` — `20 IL / 57 bytes / 0 EH`
- `0x060001BF EnemyManager.TextWaveHealth/0` — `95 IL / 251 bytes / 1 EH`
- both targets meet locked reopen floors
- no Cpp2IL helper remains in either target

## Fixed ILSpy member gate

Fixed ILSpyCmd / ICSharpCode.Decompiler `11.0.0.9375` with exactly 56 fixed references was used. EnemyManager type decompilation completed with exit 0 and zero stderr. Both target blocks contain zero Cpp2IL references, zero NotImplemented/decompiler-issue markers, zero invalid stack/type/comparison markers, and zero warning/error markers.

Readback preserves the Unity `AudioClip` truthiness path, Camera position use, original raw-null failure chain for Board/ZombieManager/List/Zombie, Zombie list filtering, `armor2Point * 0.2f`, the double-precision `waveHealth * 0.5` comparison, the every-tenth-wave zero comparison, NaN/unordered behavior, and enumerator cleanup.

## Whole-assembly semantic isolation

Using the same fixed ILSpy toolchain:

- HF44 whole IL reproduced `dbb10be399d1672a0927510a2fb05481783aabdfd56c9eb921d2bb9982ce2b1f`
- HF45 whole IL reproduced `4d4ccd9b2c857ef9aceb0e18896e77cb6b493d9e654dc0a27dce2f5bfbd46668`
- HF46 whole IL SHA-256 `3bd8d8b77fa63476da612b5a26ad799395dbd2f0eedf836e50ef86cf991c5901`
- all whole-assembly decompiles exit 0 with zero stderr

Before HF45->HF46 isolation, the retained normalization/parser reproduced the Drive-archived HF44->HF45 semantic diff byte-for-byte at `a1508fbe29703cec50c81b8db85857fcdb2e1fda4abbdf24a1059b0fcda225e5`.

HF45->HF46 results:

- MethodDef `2317 -> 2317`
- distinct method-body RVAs `2139 -> 2139`
- normalized non-method skeleton byte-identical
- normalized skeleton SHA-256 both sides `afc47c7eaa314ef12143a7ac99ab0db9fcb484b045d76b2fc74d6f2c93ac3b88`
- changed exactly `0x060001BC`, `0x060001BF`
- HF45->HF46 semantic diff SHA-256 `d8d5e96b3d2e465f0cce0de909a6be146e162f074a229ed1c1b692c5f7d4b485`
- canonical HF45 MethodDef table reproduced byte-for-byte at `784c0d85a4dd74015670c865493c2e08e324fc54c83541caace717e06300b50d`
- HF46 MethodDef table SHA-256 `8266feac687beec6c232c4a5be3d062dbc033036a181534270627d4258706aa4`

No third MethodDef changed.

## Permanent RecoveryAudit

RecoveryAudit commit:

`f8c0f3a9947a71de01b23d8be7250e7178720313`

RecoveryAudit source blob `e025c7ee0bd9d2d5cc7deecb127d87c69ce2d618`.  
Workflow run: `34494674714` PASS  
Published auditor artifact ID: `10159255979`  
Auditor artifact ZIP SHA-256:

`3c29a0e9c04cc9b439ed11df2560d6d59db953cd036be73ce2c92547bbae71ac`

The published auditor was executed independently twice against the accepted HF46 candidate. Both process runs completed with exit 0 and zero stderr. Each process performed internal OPEN1 and OPEN2 over the complete retained historical target set and ended with `RECOVERY_AUDIT_OK`.

HF46 readback in OPEN1 and OPEN2 was stable at:

- `EnemyManager.PlayBoardAudio/1` — `20 IL / 57 bytes`
- `EnemyManager.TextWaveHealth/0` — `95 IL / 251 bytes`

## Source provenance

Published HF46 patcher source is pinned to immutable build head `82ecf79f54815ac83d890988591e388f7341a812`:

- `HF46Patch.csproj` blob `36e0a763e2a267c9ca51a4c526d9d7c27561bb16`
- `Program.cs` blob `127ac7aa70157b3d301ceebe621c0fe61e434b08`
- `Template.cs` blob `b454016694d109c8f2b9fa8adbb9367dfe87a4ff`
- workflow blob `4cd525d220c7481d3e132371ede7320d23d1785b`

## Closure state

All gates through permanent published RecoveryAudit are PASS. Remaining formal steps are strict Drive 19 ordinary payload + payload manifest pre-closure readback, the two FINAL closure files, exact 22-file final provider readback, and only then `Recovery/STATUS.md` advancement. Until those complete, HF45 remains the only formal cumulative final and no HF47 stage may consume the HF46 candidate.
