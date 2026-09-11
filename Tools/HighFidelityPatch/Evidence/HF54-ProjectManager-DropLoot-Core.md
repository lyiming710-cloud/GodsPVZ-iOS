# HF54 — ProjectManager DropLoot Core

HF54 restores exactly one managed MethodDef from original-PC-native-backed behavior:

- `0x06000202` / RID 514 / `ProjectManager.DropLootPiece(Zombie, Vector3)`.

Formal input is the audited HF53 cumulative DLL only:

- HF53 SHA-256: `924a06f86d648d1bc04bb1d73ccc610dbd59daeb040331910dad86541777d9b5`.
- HF54 candidate SHA-256: `307dc20c09a6338ecf4a4917b489cc0303d61c79a6aafeb34011e3e67961af2c`.

## Original-PC attribution

Original source hashes:

- PC ZIP SHA-256: `2f08b4e2243e2bd296f3db694b4a2c9656252d76202c80167bdf3340b9f65b48`.
- `GameAssembly.dll` SHA-256: `9ebd7ca996a5b03fb4a766f7a2502b660d581ddbbb36af4d7bf2f06a211da39d`.
- `global-metadata.dat` SHA-256: `ad992341add498bd1018ac980f42171e0737b0793bf945c8714ad70a445b36b9`.

Using the ordinary original MethodDef ownership rule, `Assembly-CSharp.dll` is CodeGenModule 0 and RID 514 maps to method-pointer index 513. The original PC address is:

- `ProjectManager.DropLootPiece(Zombie,Vector3)` -> `0x180323260`.

Neighbor attribution is consistent:

- RID 513 `ProjectManager.CreateProject(int,int,Vector3)` -> `0x180323050`.
- RID 515 begins at `0x1803235E0`.

The native body at `0x180323260` closes the method semantics without guessed fallback behavior. It:

1. obtains `Camera.main.orthographicSize` and clamps `position.x` to `±orthographicSize * 920f / 540f`;
2. checks the board object, `!board.isFinished`, then calls RID 720 `Board.TestWinTargetZombie()` at PC `0x180328000`;
3. on the target-win branch calls RID 704 `Board.GameFinished()` at PC `0x180326A70`, creates project ID 4 through RID 513 `CreateProject`, calls the already-HF51-closed `Project.SetEndPosition<Zombie>` implementation at PC `0x1804A1AA0`, and returns null;
4. reads `zombie.enemyPoint`, calls the original random-range path with `[0,10000)`, and uses native multipliers 30, 100, 150 and 800;
5. selects project IDs 3, 2, 1 or 8 on the corresponding native threshold branches;
6. on the sun branch creates three ID-0 projects, calls RID 974 `Project.SunSet(int)` at PC `0x180378750` with 25, 50 and 100, calls `SetEndPosition<Zombie>` for each, and returns the 100-sun project;
7. otherwise creates the selected project, calls `SetEndPosition<Zombie>` when non-null, and returns it.

## Deterministic patch gate

HF54 patcher build:

- build head: `d9cc2dc18fda2a087f1ec81eb016fc57388dcfc0`;
- workflow run: `34572300501` PASS;
- artifact ID: `10188237519`;
- artifact SHA-256: `357f588db9a6da8b780cad002dce778b934d356ddf8b601fa634f99ef75e5116`.

Two independent executions from the locked HF53 input produce byte-identical output:

- output SHA-256: `307dc20c09a6338ecf4a4917b489cc0303d61c79a6aafeb34011e3e67961af2c`;
- both stderr: 0 bytes;
- Cecil reopen: 159 IL instructions / 341 bytes / 0 exception handlers.

## Fixed decompiler and semantic isolation

Fixed tooling:

- ILSpyCmd / ICSharpCode.Decompiler `11.0.0.9375`;
- fixed 56-DLL Cpp2IL reference archive SHA-256: `fe091e5a5389c2491eebeff72c83c394097bef67ffd45a25cf7de01c9aad9ef1`;
- ILSpyCmd archive SHA-256: `446bb884aa014ae04d6825ad6430e85dc1a827d2526075bbde754c9f85b3a1da`.

Target member readback has stderr 0 and zero target-local Cpp2IL / Unknown / NotImplemented / Expected / invalid markers.

Whole-assembly checks:

- HF53 whole IL SHA-256: `9d02f7b55aac9fa89b0ffb0f5bc2e1a4644513dc46632fa7d8acc82091c5ca4a` (historical value reproduced exactly);
- HF54 whole IL SHA-256: `2b342d49e118eb70c16137132270ddd37672e268d69e517322797fdf6bd43f42`;
- MethodDef count: `2317 -> 2317`;
- emitted bodies: `2297 -> 2297`;
- distinct nonzero body RVAs: `2140 -> 2140`;
- normalized non-method/method-signature skeleton: identical;
- changed normalized MethodDef blocks: exactly one, `0x06000202 ProjectManager.DropLootPiece(Zombie,Vector3)`.

## Cumulative audit

Composite auditor build:

- build head: `73d3ad3b4ec239a21855b74e5eb38eb980607bb4`;
- workflow run: `34572736707` PASS;
- artifact ID: `10188399748`;
- artifact SHA-256: `4bdf83b65696faf21ca861dee0080ce56a6de48a3f4a7445b37c31cf416b2683`.

Two independent actual executions pass both the retained historical RecoveryAudit and HF54-specific audit, ending in `RECOVERY_AUDIT_OK` and `HF54_AUDIT_OK`, with zero stderr. The two composite logs are byte-identical; log SHA-256 is `09973cba9a90d3e28a4450c6cd7b3ef51fd150a243907b8d24b72f8bca529a68`.

## Scope boundary

HF54 modifies only `ProjectManager.DropLootPiece(Zombie,Vector3)`. It does **not** claim repair of:

- `Zombie.Path_Test()`;
- `Zombie.DestroyZombie()`;
- `Zombie.DropLootPiece()`.

Those remain subject to a later original-native-backed gate. If no further minimal managed candidate closes cleanly, the deterministic next route remains Stage9.1 integration, 67/67 package-script reference closure, and exact Unity `2022.3.44f1c1` import/compile validation.