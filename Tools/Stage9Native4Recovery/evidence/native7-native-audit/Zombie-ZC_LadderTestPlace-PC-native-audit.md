# Zombie::ZC_LadderTestPlace — direct PC-native audit

Status: **native semantics established; no production promotion**.

Authority is the original `GodsPVZ_1.0.2.zip` / PC x86-64 IL2CPP binary.

## Locked identity

- ZIP SHA256: `2f08b4e2243e2bd296f3db694b4a2c9656252d76202c80167bdf3340b9f65b48`
- `GameAssembly.dll` SHA256: `9ebd7ca996a5b03fb4a766f7a2502b660d581ddbbb36af4d7bf2f06a211da39d`
- `global-metadata.dat` SHA256: `ad992341add498bd1018ac980f42171e0737b0793bf945c8714ad70a445b36b9`
- managed method: `System.Boolean Zombie::ZC_LadderTestPlace()`
- MethodDef: `0x0600049C`, RID `1180`
- method-pointer entry: `0x181B85238`
- native VA: `0x180370C70`
- exact `.pdata` range: `0x180370C70–0x180370D80`
- exact native length: `272` bytes
- unwind RVA: `0x1A8FFA8` (flags `0`, not chained)
- exact native SHA256: `4282e47305c8efe720a005ec03366a10a8c090d068f0bc3a56dc96e60e105b17`

## Direct native semantics

The exact native control flow is:

```text
if (armor2Type != 4)
    return false;

float x = fX - rDirection.x * 67.0f;
float y = fY - rDirection.y * 67.0f;

int gridX = board.boardConfig.GetGridX(x, y);
int gridY = board.boardConfig.GetGridY(x, y);
Grid grid = board.GetGrid(gridX, gridY);

if (grid == null)
    return false;

if (grid.GetPassablePoint() > passablePoint)
    return false;

if (grid.GetPassablePoint() <= 60)
    return false;

return true;
```

Important observable details:

- `board` and `board.boardConfig` are null-checked by the native code before each conversion call. Null follows the normal IL2CPP null-reference exception helper, not a defensive `false` return.
- `board` is read again for the Y conversion and again for `GetGrid`.
- `Grid::GetPassablePoint()` is called **twice**, not cached once.
- A null `Grid` is an ordinary `false` result.
- The first passability test is `GetPassablePoint() <= passablePoint`; the second is strictly `GetPassablePoint() > 60`.

## Native call attribution

Reverse lookup through the locked Assembly-CSharp method-pointer table gives:

| PC call target | managed method |
|---|---|
| `0x18030ED50` | `BoardConfig::GetGridX` (`0x06000159`) |
| `0x18030EE00` | `BoardConfig::GetGridY` (`0x0600015A`) |
| `0x180326FE0` | `Board::GetGrid` (`0x060002C4`) |
| `0x18032A110` | `Grid::GetPassablePoint` (`0x06000295`) |

Relevant managed fields are `armor2Type` (`0x040005C8`), `board` (`0x040005AA`), `fX` (`0x040005AD`), `fY` (`0x040005AE`), `passablePoint` (`0x040005F8`), and `rDirection` (`0x040005FC`).

## Current damaged managed body

The current experimental native6 candidate still contains the original Cpp2IL SIMD-lifting corruption at the start of this method:

```text
ldarg.0
ldfld Zombie::rDirection
ldstr "..."
pop
ldc.i4.0
conv.i
xor
...
conv.r4
```

This attempts a binary integer operation on a full `UnityEngine.Vector3`, which directly explains the Unity IL2CPP `Cannot get stack type for Vector3` failure. The correct native operation is per-component sign inversion followed by `* 67.0f`, equivalent to typed managed subtraction from `fX` / `fY`.

This method is therefore suitable for a target-specific full MethodBody reconstruction; a generic Vector3 rewrite is not acceptable.
