# HF53 — GameFail Dependency Core

## Scope

Formal input: HF52 `1db686c32f24ea81660f3d18ccfd649ee7aeb5a40902e2042639e47355184b82`.

HF53 authorizes exactly four native-backed MethodDefs:

- `0x0600013D` RID317 `GlobalStaticVars.CreateAudioAtPoint(AudioClip, Vector3, float)` → PC `0x18031B230`;
- `0x06000277` RID631 `ZombieManager.BGMPasue()` → PC `0x180341700`;
- `0x060002BC` RID700 `Board.GameFail()` → PC `0x180326910`;
- `0x06000705` RID1797 `Window_Q.PopupNewWindow(int, Transform, Board)` → PC `0x1803AFC60`.

No other MethodDef is authorized by HF53.

## Original-PC closure

Original locks used again:

- PC ZIP `2f08b4e2243e2bd296f3db694b4a2c9656252d76202c80167bdf3340b9f65b48`;
- `GameAssembly.dll` `9ebd7ca996a5b03fb4a766f7a2502b660d581ddbbb36af4d7bf2f06a211da39d`;
- `global-metadata.dat` `ad992341add498bd1018ac980f42171e0737b0793bf945c8714ad70a445b36b9`.

Native behavior closure:

1. Three-argument `CreateAudioAtPoint` copies the real `Vector3` argument and forwards `(clip, position, volume, 1.0f)` to the already-usable four-argument overload at PC `0x18031B350`.
2. `ZombieManager.BGMPasue` enumerates `zombieList`, calls `Zombie.BGMPasue()` for each element, and disposes the enumerator.
3. `Board.GameFail` sets `isFailed`, pauses the game, reads `audioList1[2]`, obtains `Camera.main.transform.position`, multiplies `AudioVolume()` by `0.8f`, plays the fail audio at that real camera position, pauses board audio/BGM, then opens window Q=3 under `WindowsUI.transform`.
4. `Window_Q.PopupNewWindow(int,Transform,Board)` instantiates `ResourceManager.prefab_Window_Q`, stores `Q`, `onBoard=true`, and `board`, parents its transform with `worldPositionStays=false`, and returns it. Current managed reconstruction's exception-object cast on the null path is not original behavior.

Direct dependencies retained rather than rewritten where already usable include `Board.GamePause(bool)`, `GlobalStaticVars.AudioVolume()`, the four-argument `CreateAudioAtPoint`, `Zombie.BGMPasue()`, and Unity Camera/Transform/AudioSource/Object APIs.

## Managed damage before HF53

HF52 fixed readback showed concrete normal-path reconstruction loss:

- `Board.GameFail`: camera `Vector3` was replaced by a stack-address cast before `CreateAudioAtPoint`;
- three-argument `CreateAudioAtPoint`: `Vector3` was likewise replaced by an address cast;
- `ZombieManager.BGMPasue`: missing `List<Zombie>.GetEnumerator` / Dispose reconstruction;
- Board overload of `Window_Q.PopupNewWindow`: invalid `(Window_Q)(object)new NullReferenceException()` reconstruction on null flow.

All four are gameplay reachable through the board fail path.

## Patcher and candidate

HF53 patcher build head `7891747d33470a877ca5779e06e85f0c691b1da5`; workflow run `34569860029` PASS; artifact `10187362596`; artifact SHA-256 `d7652ec5dabc8916bcf6cf2227198cfac2278ed5dd36ccac39029ee7137e8912`.

Independent double patch from the formal HF52 DLL produced byte-identical output:

`924a06f86d648d1bc04bb1d73ccc610dbd59daeb040331910dad86541777d9b5`

Both runs exited 0 with stderr 0.

Cecil reopen:

- `0x0600013D`: 6 IL / 14 bytes / 0 EH;
- `0x06000277`: 17 IL / 52 bytes / 1 EH;
- `0x060002BC`: 38 IL / 102 bytes / 0 EH;
- `0x06000705`: 17 IL / 45 bytes / 0 EH.

## Fixed ILSpy and semantic isolation

Fixed ILSpyCmd / ICSharpCode.Decompiler `11.0.0.9375` with the fixed 56-DLL reference set was used.

All four target members: stderr 0 and target-local `Cpp2IL`, `Unknown result type`, `NotImplemented`, `Expected`, and invalid-type markers 0.

HF52 whole IL was reproduced exactly as `515fd597660dd2baafc251c3797707186839ccac01727f32ff4ad2ba52c39994`; HF53 whole IL is `9d02f7b55aac9fa89b0ffb0f5bc2e1a4644513dc46632fa7d8acc82091c5ca4a`; both whole decompilations had zero-byte stderr.

Whole-assembly isolation:

- MethodDefs `2317 -> 2317`;
- emitted bodies `2297 -> 2297`;
- distinct nonzero body RVAs `2140 -> 2140`;
- relocation-normalized non-method/method-signature skeleton identical;
- changed method blocks exactly the four authorized targets above.

## Cumulative RecoveryAudit

HF53 cumulative audit build head `80cd3838c91f98b04761e8c6713dcea1d4e9dd96`; workflow run `34570344401` PASS; composite artifact `10187537492`; artifact SHA-256 `d9d9273bf7277608bb06dacde940fab667d02f9c203130c5bdf24f5be915143f`.

Two independent local executions on the candidate both produced historical `RECOVERY_AUDIT_OK` plus cumulative `HF53_AUDIT_OK`, stderr 0. The full logs are byte-identical with SHA-256 `2f3846aba608d83dc10415ee2704b2b2666a11fede51aa9b771423121235a30c`.

## Residual boundary

HF53 does **not** claim repair of `Zombie.Path_Test`, `Zombie.DestroyZombie`, `Zombie.DropLootPiece`, or `ProjectManager.DropLootPiece`. Those remain subsequent native/dependency-gate candidates.

At this commit the technical HF53 gates pass; formal acceptance still requires the standard Google Drive 19+1+2 archive/provider-readback closure and `Recovery/STATUS.md` advancement.
