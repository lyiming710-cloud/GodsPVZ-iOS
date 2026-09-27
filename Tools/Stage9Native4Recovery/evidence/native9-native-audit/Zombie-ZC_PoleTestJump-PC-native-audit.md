# Zombie::ZC_PoleTestJump — direct PC-native audit

Status: **native semantics established; no production promotion**.

Authority is the original GodsPVZ 1.0.2 PC x86-64 IL2CPP release.

## Locked identity

- package SHA256: `2f08b4e2243e2bd296f3db694b4a2c9656252d76202c80167bdf3340b9f65b48`
- `GameAssembly.dll` SHA256: `9ebd7ca996a5b03fb4a766f7a2502b660d581ddbbb36af4d7bf2f06a211da39d`
- `global-metadata.dat` SHA256: `ad992341add498bd1018ac980f42171e0737b0793bf945c8714ad70a445b36b9`
- managed method: `System.Boolean Zombie::ZC_PoleTestJump()`
- MethodDef: `0x060004A3`, RID `1187`
- method-pointer entry: `0x181B85270`
- native VA: `0x180371750`
- exact `.pdata` range: `0x180371750–0x180371989`
- exact native length: `569` bytes
- unwind RVA: `0x1A90010` (version 1, flags 0; not chained)
- exact native SHA256: `b9cb66024814e677417c73e96cacce5f4636bc12baf7119e2f2f4ae4c16df9ae`

The MethodDef-to-native mapping is independently consistent with the neighboring Zombie method-pointer run: `ZC_LadderTestPlace` RID 1180 maps through entry `0x181B85238` to its already-locked PC VA `0x180370C70`.

## Direct native semantics

The exact PC control flow is equivalent to:

```text
if (!poleZombie_pole)
    return false;
if (poleZombie_jump)
    return false;
if (isStant)
    return false;
if (IsDisabled())
    return false;

float x = fX - rDirection.x * 134.0f;
float y = fY - rDirection.y * 134.0f;

int gx = board.boardConfig.GetGridX(x, y);
int gy = board.boardConfig.GetGridY(x, y);
Grid grid = board.GetGrid(gx, gy);
if (grid == null)
    return false;

int gridPassable = grid.GetPassablePoint();
if ((float)gridPassable > 100.0f && (float)passablePoint >= (float)gridPassable)
    return true;

Plant p;
if (grid.plant_sheath)
    p = grid.plant_sheath;
else if (grid.plant_common)
    p = grid.plant_common;
else if (grid.plant_bottom)
    p = grid.plant_bottom;
else
    p = null;

return p != null; // UnityEngine.Object lifetime-aware inequality semantics
```

### IsDisabled signature correction

The locked managed MethodDef `0x06000464` is `System.Boolean Zombie::IsDisabled()` and has **zero managed parameters**. An earlier audit draft described the native call as `IsDisabled(false)` because the x64 call site clears a register before the call. That cleared register belongs to IL2CPP's native calling convention / hidden method metadata plumbing and is not a managed Boolean parameter. The managed reconstruction must therefore emit only `ldarg.0; call Zombie::IsDisabled()`.

This distinction was independently exposed by the direct IL2CPP canary: emitting an extra `ldc.i4.0` before the zero-argument call polluted the evaluation stack and caused a later `ret` failure (`Attempting to return a value ... when there is no value on the stack`).

## Native field attribution

Zombie instance offsets used by the PC body align with the locked managed field order:

| Native offset | managed field | FieldDef |
|---|---|---|
| `+0x20` | `board` | `0x040005AA` |
| `+0x30` | `fX` | `0x040005AD` |
| `+0x34` | `fY` | `0x040005AE` |
| `+0xBA` | `isStant` | `0x040005D4` |
| `+0xCD` | `poleZombie_pole` | `0x040005DE` |
| `+0xCE` | `poleZombie_jump` | `0x040005DF` |
| `+0x11C` | `passablePoint` | `0x040005F8` |
| `+0x13C` | `rDirection` | `0x040005FC` |

Grid offsets used by the native body correspond to:

| Native offset | managed field | FieldDef |
|---|---|---|
| `+0x28` | `plant_bottom` | `0x04000388` |
| `+0x30` | `plant_common` | `0x04000389` |
| `+0x38` | `plant_sheath` | `0x0400038A` |

The priority in the PC native body is therefore sheath → common → bottom.

## Direct native call attribution

Reverse lookup through the locked Assembly-CSharp method-pointer table gives:

| PC call target | managed method |
|---|---|
| `0x1803659E0` | `Zombie::IsDisabled()` (`0x06000464`, RID 1124) |
| `0x18030ED50` | `BoardConfig::GetGridX` (`0x06000159`) |
| `0x18030EE00` | `BoardConfig::GetGridY` (`0x0600015A`) |
| `0x180326FE0` | `Board::GetGrid` (`0x060002C4`) |
| `0x18032A110` | `Grid::GetPassablePoint` (`0x06000295`) |

The PC constants are directly present in the original image:

- `0x1815A7B2C` = `134.0f`
- `0x1815A7C70` = `100.0f`
- `0x1815A7CF0` is the sign-bit mask used to negate `rDirection.x/y` before multiplication.

The helper at `0x18131F870` implements Unity object truthiness: null returns false; a non-null object performs the Unity lifetime check. The helper at `0x18131F900` implements Unity object inequality semantics; two nulls return false, distinct/non-null live objects return true, and destroyed-object behavior is routed through the same Unity lifetime helper. The final plant result must therefore preserve Unity `Object` truthiness/inequality rather than CLR-only reference comparison.

## Current iOS blocker history

The native7 full Unity iOS seed reached IL2CPP and failed this exact method with:

```text
IL2CPP error for method 'System.Boolean Zombie::ZC_PoleTestJump()'
System.ArgumentException: Cannot get stack type for Vector3
... WriteBinaryOperationUsingLargestOperandTypeAsResultType ...
```

This is the same class of Cpp2IL SIMD/vector lifting corruption previously observed in `ZC_LadderTestPlace`: the original PC native performs scalar component arithmetic on `rDirection.x` and `.y`, while the damaged managed body exposes an invalid binary operation whose operand stack contains a full `UnityEngine.Vector3`.

The first native9 reconstructions removed that Vector3 failure but initially emitted an extra managed `false` argument to `IsDisabled()`. The direct post-Linker canary caught the resulting invalid stack before any expensive full Unity rerun. The current reconstruction is required to use the exact zero-argument MethodDef signature and remain isolated to MethodDef `0x060004A3`.
