# Plant_DetaiPage.Update PC native authority

Read-only prefetch. This method is not a promoted mutation target until runtime selects it as the first causal gameplay blocker.

Authority: original GodsPVZ 1.0.2 PC x86-64 IL2CPP `GameAssembly.dll`.

- GameAssembly SHA256: `9ebd7ca996a5b03fb4a766f7a2502b660d581ddbbb36af4d7bf2f06a211da39d`
- Assembly-CSharp method pointer table VA: `0x181B82D60`
- Managed MethodDef: `Plant_DetaiPage.Update()`
- Token: `0x06000661`
- RID: `1633`
- Pointer-table entry VA: `0x181B86060`
- Native method VA: `0x1803A57D0`
- `.pdata` exact range: `0x1803A57D0–0x1803A5934` (356 bytes)
- Native slice SHA256: `8d2c51790690cb695fa590e65e09378555174d4f050dba2a716a6530b3052996`

Managed 5789 fingerprint before recovery:

- code size: `355`
- locals: `14`
- semantic fingerprint: `3c14baa548dc90bc73b00f0e524b8257b6e48c1ca2506b0fb5c0efe9885d56ef`
- corrupt local: `System.Object` local 13 is used by both `Transform.Rotate(Vector3,float,Space)` calls as if it were a `Vector3` address.

## Recovered semantics

The PC native body establishes both Rotate calls precisely:

1. `roll.transform.Rotate(Vector3.forward, -60f * Time.deltaTime, Space.Self)`
2. `skillAuto.transform.Rotate(Vector3.forward, -75f * Time.deltaTime, Space.Self)`

The axis constant is `(0, 0, 1)`: native zeroes x/y and loads float `1.0f` from `0x1815A7A10` into z. Rotation-speed constants are `-60.0f` at `0x1815A7F9C` and `-75.0f` at `0x1815A7FA0`; the Space argument is native integer `0` (`Space.Self`).

Key native sequence for the first rotation:

```text
0x1803A57E2  mov rcx,[rcx+0x50]       ; roll
0x1803A57F6  call 0x181304510         ; Component.get_transform
0x1803A580B  call 0x18132C2F0         ; Time.get_deltaTime
0x1803A5819  mulss xmm0,[0x1815A7F9C] ; * -60.0f
0x1803A582C  xorps xmm1,xmm1          ; x/y = 0
0x1803A582F  movss [rsp+0x38],xmm7    ; z = 1.0f
0x1803A584A  call 0x18132E150         ; Transform.Rotate(Vector3,float,Space)
```

Second rotation is structurally identical and uses `skillAuto` plus `-75.0f`.

Remaining gameplay flow matches the managed reconstruction:

- if `p_skill` is live, call `Update_SkillProgress()`;
- inspect `plant`;
- `skillLock.gameObject.SetActive(true)` only when `plant.ID == 4` and `plant.state != 0`, otherwise false;
- always tail into `Updata_Camera()` after that logic;
- if `p_skill` is absent, skip skill-progress/lock logic and tail into `Updata_Camera()`.

If runtime promotes this method, the recovery must use typed `Vector3.forward`/equivalent `(0,0,1)` values for both Rotate calls and preserve the rest of the PC-native branch structure. Do not substitute a zero vector, expose fields, or add defensive guards solely for CLR execution.
