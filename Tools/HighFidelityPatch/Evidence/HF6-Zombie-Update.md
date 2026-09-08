# HF6 — Zombie.Update native recovery evidence

## Scope

HF6 changes exactly one managed method relative to the audited HF5 DLL:

- `Zombie.Update()`
- original MethodDef RID: `1057`
- token: `0x06000421`
- PC native entry: `0x18036CD60`
- exact native function range: `0x18036CD60..0x18036D2FF` (`0x5A0` bytes including the IL2CPP null-throw tail)

Primary source is the original GodsPVZ 1.0.2 PC x86-64 `GameAssembly.dll` plus original metadata. Android/CIL output is not used as authority for this method.

## Native field bindings used by HF6

| Offset | Field |
|---|---|
| `+0x20` | `board` |
| `+0x30` | `fX` |
| `+0x34` | `fY` |
| `+0x38` | `fZ` |
| `+0xB8` | `isDied` |
| `+0xBA` | `isStant` |
| `+0xBD` | `isOnBoard` |
| `+0xC4` | `snowbeast_impactCD` |
| `+0x108` | `elementManager` |
| `+0x110` | `livingTime` |
| `+0x114` | `waitingTime` |
| `+0x118` | `destroyTicking` |
| `+0x130/+0x134/+0x138` | `rSpeed.x/y/z` |
| `+0x160` | `sortingGroup` |
| `+0x1A0` | `animationGroup` |
| `+0x228` | `buffManager` |
| `+0x230` | `stiffnessTime` |
| `+0x238` | `updateRate` |

Related native fields: `Board+0x41 = gameStart`, `Element+0x24 = point`, `Element+0x30 = burstTime`.

## Exact constants recovered from the PC image

- `1.0f`
- `0.05f`
- `1000.0f`
- `-10.0f`
- string: `"Entity"`

The update-rate branch is:

`Max(0.05f, 1.0f - Ceiling(element.point / 1000.0f) * 0.05f)`

unless stiffness is active or the element burst is active, in which case the desired rate is `0.0f`.

## Native control flow restored

1. Compute desired update rate from stiffness and fire/ice element state. `UCOMISS` equality behavior is preserved: NaN update rates are treated as different and reset.
2. If on-board, require `board.gameStart` and `board.BoardRuntime()`.
3. Increment `livingTime` using an independent `Time.deltaTime` call.
4. Countdown/clamp `stiffnessTime` and `snowbeast_impactCD`; unordered/NaN post-subtraction values are retained exactly as in the native `COMISS/JB` branches.
5. `GetMoveDirection()` → `SetrSpeed(direction)`.
6. Use two independent `Time.deltaTime` calls for `rSpeed.x` and `rSpeed.y`; store `fY` before `fX`; call `TestPosition(fX,fY)`.
7. If not dead, independently read base Transform positions and write `(fX,fY,z)` to the base transform and `(fX,fY+fZ,z)` to `animationGroup.transform`.
8. Restore the native path state machine using `IsDisabled`, `isStant`, `Path_Test`, `Path_Finding`, `waitingTime`, and `TranToWalk`. The native fallback `waitingTime = Time.deltaTime` is retained.
9. Ensure `SortingGroup`, set `sortingLayerName = "Entity"`, and set `sortingOrder = (int)(fY * -10f)` (native `cvttss2si`).
10. Call `BuffManager.Update<Zombie>(this)`; if enabled, call `Update_Attack()` and `Update_Characteristic()`.
11. Call `ElementManager.Update()` and `InjuryStatusUpdate_Body(false)`.
12. Always call `Update_Brightness()` and `Update_PreviousPosition()`.
13. Countdown `destroyTicking` with native unordered behavior and call `DestroyZombie()` when the ordered result reaches `<= 0`.

There are exactly eight native `Time.deltaTime` call sites in the recovered managed body.

## Direct PC-native call targets checked

- `0x180315250` — `ElementManager.GetElement`
- `0x180366E40` — `Zombie.ResetUpdateRate`
- `0x180325CC0` — `Board.BoardRuntime`
- `0x180361440` — `Zombie.GetMoveDirection`
- `0x180368280` — `Zombie.SetrSpeed`
- `0x180369FC0` — `Zombie.TestPosition`
- `0x1803659E0` — `Zombie.IsDisabled`
- `0x180366030` — `Zombie.Path_Test`
- `0x180365EA0` — `Zombie.Path_Finding`
- `0x18036A8C0` — `Zombie.TranToWalk`
- `0x18036ADC0` — `Zombie.Update_Attack`
- `0x18036B960` — `Zombie.Update_Characteristic`
- `0x180315440` — `ElementManager.Update`
- `0x1803652B0` — `Zombie.InjuryStatusUpdate_Body` (already Exact in HF3)
- `0x18036B760` — `Zombie.Update_Brightness`
- `0x18036C720` — `Zombie.Update_PreviousPosition`
- `0x18035E470` — `Zombie.DestroyZombie`

Unity/BCL calls were also checked for `Time.deltaTime`, Transform get/set position, `Object.op_Equality`, `GameObject.AddComponent<SortingGroup>`, SortingGroup setters, `MathF.Ceiling`, and `Math.Max(float,float)`.

## Validation

- HF6 patcher CI run: `34180363023` — success.
- Patcher reopen: `Zombie.Update` = `291 IL`, `1003 bytes`, exactly eight `Time.deltaTime` call sites.
- Independent ILSpyCmd 11.0.0.9375 with the recovered Unity reference set: exit 0, stderr empty, no decompiler warnings in `Zombie.Update`.
- Independent Mono.Cecil audit: OPEN1 and OPEN2 both succeed; `Zombie.Update` token remains `0x06000421`, `291 IL`, `1003 bytes`. Prior HF3/HF4/HF5 Zombie methods also pass the same audit.
- Full-assembly HF5→HF6 IL comparison: after normalizing method RVAs and `<PrivateImplementationDetails>` physical data offsets, exactly one diff hunk remains, wholly inside `Zombie.Update`.

Final HF6 SHA-256:

`e6395652d0d413fbb496dc9cae8f2712d6a65bcfad109d160818983cde234e75`

## Confidence

`Zombie.Update`: **Exact** for this method's native-visible control flow, constants, field accesses, call targets, argument values, branch ordering, and floating-point ordered/unordered behavior.

This does **not** imply that every callee is already Exact. In particular, high-centrality helpers such as `SetrSpeed`, `GetMoveDirection`, `Update_Attack`, `Update_Characteristic`, `BuffManager.Update<Zombie>`, and path helpers remain separate recovery targets where their current CIL still contains decompiler damage.
