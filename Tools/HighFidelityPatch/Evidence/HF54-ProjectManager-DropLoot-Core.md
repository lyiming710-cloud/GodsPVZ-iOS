# HF54 — ProjectManager DropLoot Core

HF54 restores exactly one formal managed MethodDef:

- formal HF53/HF54 identifier: `0x06000202` / RID 514 / `ProjectManager.DropLootPiece(Zombie, Vector3)`;
- HF53 input SHA-256: `924a06f86d648d1bc04bb1d73ccc610dbd59daeb040331910dad86541777d9b5`;
- HF54 candidate SHA-256: `307dc20c09a6338ecf4a4917b489cc0303d61c79a6aafeb34011e3e67961af2c`.

## Original-source recheck

The source packages were re-materialized directly from Google Drive before formal closure.

PC source:

- `GodsPVZ_1.0.2.zip`: 242474120 bytes; SHA-256 `2f08b4e2243e2bd296f3db694b4a2c9656252d76202c80167bdf3340b9f65b48`;
- extracted `GameAssembly.dll`: `9ebd7ca996a5b03fb4a766f7a2502b660d581ddbbb36af4d7bf2f06a211da39d`;
- extracted `global-metadata.dat`: `ad992341add498bd1018ac980f42171e0737b0793bf945c8714ad70a445b36b9`;
- metadata header version 31;
- Unity version from `globalgamemanagers`: `2022.3.44f1c1`.

Android cross-check:

- `GodsPVZ_1.0.2_Android.apk`: 247747121 bytes; SHA-256 `428e0ba2e46645a905fb9fbb00cfde42406727a3ddfce9a7df1e0875889a739f`;
- arm64 `libil2cpp.so`: `cfa13d53d7c3e218221a90c5fb615a012339393774af3160ff1c17f3826c61a6`;
- armeabi-v7a `libil2cpp.so`: `3d035afbf3e419a0d48947ec460eb910731727b3e0da5a2ee83e20650668fadb`;
- Android metadata: `e7a4412e3af25da3ba2c806d3c691ec68d00c1e7915d8fb6843c3a82422f5005`;
- UnityFS version: `2022.3.44f1c1`.

Fixed Cpp2IL `2022.1.0+5fb20304df698ffd3d0e664b2a698cd911dc9d57` was rerun against Android arm64 and completed `2319 / 2319` methods. Both the fixed original-PC reference and independently regenerated Android reference identify `ProjectManager.DropLootPiece(Zombie,Vector3)` at generated-reference PE token `0x060001FC` / RID 508.

### Identifier namespace correction

The Cpp2IL generated-reference PE RID is not the same namespace as the formal recovery DLL RID or native methodPointers slot. Sequence alignment shows six MethodDefs present in the formal table before `ProjectManager` but absent from the generated-reference PE, at formal RIDs `275`, `332-334`, `400-401`. Therefore:

- generated-reference PE: `DropLootPiece` = RID 508 / `0x060001FC`;
- formal HF53/HF54 DLL: the same method = RID 514 / `0x06000202`.

The earlier draft wording that called RID 514 the Cpp2IL original-PC PE RID was imprecise. This correction changes only identifier description, not the selected native function or recovered behavior.

## Original-PC native attribution

Direct readback from the verified original `GameAssembly.dll` gives:

- `Assembly-CSharp` methodPointers slot 512 -> `0x180323050`;
- slot 513 -> `0x180323260`;
- slot 514 -> `0x1803235E0`.

The native function at `0x180323260` independently identifies itself as `ProjectManager.DropLootPiece`. Its body:

1. uses `Camera.main.orthographicSize`; native literals decode to `920.0f`, `540.0f`, `-1.0f`, clamping `position.x` to `±orthographicSize*920/540`;
2. checks board truthiness and `!board.isFinished`, then calls `Board.TestWinTargetZombie()` at `0x180328000`;
3. target-win calls `Board.GameFinished()` at `0x180326A70`, `CreateProject(2,4,position)` at `0x180323050`, and HF51-closed `SetEndPosition<Zombie>` at `0x1804A1AA0`, then returns null;
4. reads `zombie.enemyPoint`, calls random range `[0,10000)`, and uses multipliers `30`, `100`, `150`, `800`;
5. selects IDs `3`, `2`, `1`, `8`;
6. sun branch creates three ID-0 projects and calls `SunSet(25/50/100)` at `0x180378750`, with `SetEndPosition<Zombie>` for each;
7. ordinary tail creates the selected project, checks Unity-object truthiness, conditionally calls `SetEndPosition<Zombie>`, and returns it.

PC native disassembly is the formal behavioral authority. PC and Android reconstructed managed bodies independently agree on the defining logic and serve as a second-source version/semantic cross-check.

## Deterministic patch gate

- patcher build head `d9cc2dc18fda2a087f1ec81eb016fc57388dcfc0`;
- workflow `34572300501` PASS;
- artifact ID `10188237519`;
- artifact SHA-256 `357f588db9a6da8b780cad002dce778b934d356ddf8b601fa634f99ef75e5116`;
- two independent patches are byte-identical at `307dc20c09a6338ecf4a4917b489cc0303d61c79a6aafeb34011e3e67961af2c`;
- both stderr 0;
- Cecil reopen: 159 IL / 341 bytes / 0 EH.

## Fixed decompiler / semantic isolation

- ILSpyCmd / ICSharpCode.Decompiler `11.0.0.9375`;
- fixed 56-DLL Cpp2IL reference archive `fe091e5a5389c2491eebeff72c83c394097bef67ffd45a25cf7de01c9aad9ef1`;
- ILSpyCmd archive `446bb884aa014ae04d6825ad6430e85dc1a827d2526075bbde754c9f85b3a1da`;
- target readback stderr 0 and target-local bad markers 0;
- HF53 whole IL `9d02f7b55aac9fa89b0ffb0f5bc2e1a4644513dc46632fa7d8acc82091c5ca4a` reproduced exactly;
- HF54 whole IL `2b342d49e118eb70c16137132270ddd37672e268d69e517322797fdf6bd43f42`;
- MethodDef `2317 -> 2317`, emitted bodies `2297 -> 2297`, distinct nonzero body RVAs `2140 -> 2140`;
- normalized skeleton identical;
- changed normalized MethodDef blocks exactly one: formal `0x06000202 ProjectManager.DropLootPiece(Zombie,Vector3)`.

## Cumulative audit

- audit build head `73d3ad3b4ec239a21855b74e5eb38eb980607bb4`;
- workflow `34572736707` PASS;
- artifact ID `10188399748`;
- artifact SHA-256 `4bdf83b65696faf21ca861dee0080ce56a6de48a3f4a7445b37c31cf416b2683`;
- two actual executions end `RECOVERY_AUDIT_OK` + `HF54_AUDIT_OK`, stderr 0;
- byte-identical composite log SHA-256 `09973cba9a90d3e28a4450c6cd7b3ef51fd150a243907b8d24b72f8bca529a68`.

## Scope boundary

HF54 modifies only formal `ProjectManager.DropLootPiece(Zombie,Vector3)`. It does not claim repair of `Zombie.Path_Test()`, `Zombie.DestroyZombie()`, or `Zombie.DropLootPiece()`. If no later minimal native-backed candidate closes cleanly, resume Stage9.1, 67/67 package-script reference closure, and exact Unity `2022.3.44f1c1` import/compile validation.