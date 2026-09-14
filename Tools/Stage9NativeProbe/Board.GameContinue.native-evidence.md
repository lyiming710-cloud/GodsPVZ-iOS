# Board.GameContinue PC native authority

Authority: original GodsPVZ 1.0.2 PC x86-64 IL2CPP `GameAssembly.dll`.

- GameAssembly SHA256: `9ebd7ca996a5b03fb4a766f7a2502b660d581ddbbb36af4d7bf2f06a211da39d`
- Assembly-CSharp method pointer table VA: `0x181B82D60`
- Managed MethodDef: `Board.GameContinue()`
- Token: `0x060002BD`
- RID: `701`
- Pointer-table entry VA: `0x181B84340`
- Native method VA: `0x180326700`
- `.pdata` exact range: `0x180326700–0x1803267D9` (217 bytes)
- Native slice SHA256: `28b029b2f9c2ff3b683305f9a3e8aede54b2e42e255ececc3c12542900463987`

## Recovered semantics

The native method tests key code `0x20` (Space). If the key-down result is true it writes `pauseTemp = true`. It then always writes `gamePause = false`, restores `Time.timeScale` from `gameSpeed`, applies `GlobalStaticVars.AutoTimeSlow(0.25f, this)` when `onDetailPage` is true, calls `audioSource[0].UnPause()`, calls `zombieManager.BGMUnPasue()`, and deactivates the pause GameObject.

Key native sequence:

```text
0x180326725 xor  edx,edx
0x180326727 mov  ecx,0x20
0x18032672c call 0x1813648a0       ; key-down binding
0x180326731 test al,al
0x180326733 je   0x18032673c
0x180326735 mov  byte ptr [rbx+0xe0],1  ; pauseTemp
0x18032673c movss xmm0,[rbx+0x44]       ; gameSpeed
0x180326743 mov  byte ptr [rbx+0x42],0  ; gamePause
0x180326747 call 0x18132c500            ; Time.timeScale
0x18032674c cmp  byte ptr [rbx+0xd0],0  ; onDetailPage
...
0x18032676a movss xmm0,[0x1815a7ab0]    ; 0.25f
0x180326775 mov  rdx,rbx
0x180326778 call 0x18031af30            ; AutoTimeSlow
```

The recovered managed DLL currently binds that key test directly to Unity's internal `Input.GetKeyDownInt(KeyCode)`, which Mono rejects with `MethodAccessException`. Exact Unity 2022.3.44f1c1 reference assemblies show public `Input.GetKeyDown(KeyCode)` is the wrapper for the same internal call. The CLR adaptation is therefore limited to `GetKeyDownInt(KeyCode.Space)` → public `GetKeyDown(KeyCode.Space)`; gameplay state transitions and field metadata remain unchanged.
