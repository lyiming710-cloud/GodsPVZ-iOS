# ZombieManager.Update_Ladder PC native authority

Read-only prefetch. This method is not a promoted mutation target until runtime selects it as the first causal gameplay blocker.

Authority: original GodsPVZ 1.0.2 PC x86-64 IL2CPP `GameAssembly.dll`.

- GameAssembly SHA256: `9ebd7ca996a5b03fb4a766f7a2502b660d581ddbbb36af4d7bf2f06a211da39d`
- Assembly-CSharp method pointer table VA: `0x181B82D60`
- Managed MethodDef: `ZombieManager.Update_Ladder()`
- Token: `0x06000274`
- RID: `628`
- Pointer-table entry VA: `0x181B840F8`
- Native entry VA: `0x1803427B0`
- Method spans three consecutive runtime-function fragments:
  - `0x1803427B0–0x180342937` (391 bytes), SHA256 `2413b07e1558191f9d9434495b77ff2967d8ff1d418c3e88b4b971e885649c42`
  - `0x180342937–0x18034299E` (103 bytes), SHA256 `d155fce37bf89605...`
  - `0x18034299E–0x180342AB2` (276 bytes), SHA256 `40151f83300dd6cf...`
- Contiguous method span: `0x1803427B0–0x180342AB2` (770 bytes)
- Contiguous span SHA256: `c15d5dc6e6cbe2c2a011ff983a5cd044f33d3b36eb8ec2ebd0f682abf873cbd2`
- The next independent runtime function begins at `0x180342AD0`.

## Recovered semantics

The PC native body matches the high-level managed intent and fixes the damaged value-type reconstruction around the audio position path:

1. Return if `board.gameStart == false` or `board.gamePause == true`.
2. Continue only when `ladderTrigger == true` and `ladderCountDown > 0f`.
3. Subtract `Time.deltaTime` from `ladderCountDown`; if the result remains greater than zero, skip dispatch logic.
4. When the countdown expires and `ladderTimes == 0`, open `Window_T.PopupNewWindow(5, board.WindowsUI.transform)`.
5. On that first trigger, if `board.isFailed == false`, play `ResourceManager.boardClips[11]` at `Camera.main.transform.position` using `GlobalStaticVars.AudioVolume()` / `CreateAudioAtPoint`, activate `board.pause`, log `"GamePause"`, set `board.gamePause = true`, and set `Time.timeScale = 0f`.
6. Call `board.enemyManager.DispatcheLadderWave()`.
7. Reset `ladderCountDown = Max(15f - ladderTimes, 5f)` and increment `ladderTimes`.
8. If the incremented count reaches `10`, call `board.enemyManager.DispatcheSPHWave()`.
9. Clear `ladderTrigger = false` before returning from the active path.

Key native evidence includes:

```text
0x1803427F6  cmp byte [board+0x41],0   ; gameStart
0x180342800  cmp byte [board+0x42],0   ; gamePause
0x18034280A  cmp byte [this+0x48],0    ; ladderTrigger
0x180342814  movss xmm0,[this+0x44]    ; ladderCountDown
0x18034282A  call Time.get_deltaTime
0x18034282F  subss xmm6,xmm0
...
0x180342867  call Component.get_transform
0x180342877  call Window_T.PopupNewWindow
...
0x180342909  call List<AudioClip>.get_Item(index=11)
0x180342913  call Camera.get_main
0x180342926  call Component.get_transform
0x180342944  call Transform.get_position
0x180342967  call GlobalStaticVars.AudioVolume
0x180342984  call GlobalStaticVars.CreateAudioAtPoint
...
0x1803429A3  call GameObject.SetActive(true)
0x1803429EE  call Debug.Log("GamePause")
0x1803429F5  board.gamePause = true
0x1803429FC  call Time.set_timeScale(0f)
0x180342A20  call EnemyManager.DispatcheLadderWave
0x180342A5B  call Math.Max(15f-ladderTimes,5f)
0x180342A6A  ladderTimes++
0x180342A89  call EnemyManager.DispatcheSPHWave
0x180342A8E  ladderTrigger = false
```

If runtime promotes this method, reconstruct the complete MethodDef from these semantics with typed `Vector3` handling for `Camera.main.transform.position`. Do not patch only the first decompiler diagnostic or introduce guards not present in the PC authority.
