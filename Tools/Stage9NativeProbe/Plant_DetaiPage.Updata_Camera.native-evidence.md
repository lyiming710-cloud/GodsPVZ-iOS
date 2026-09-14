# Plant_DetaiPage.Updata_Camera PC native authority

Read-only prefetch. This method is not a promoted mutation target until runtime selects it as the first causal gameplay blocker.

Authority: original GodsPVZ 1.0.2 PC x86-64 IL2CPP `GameAssembly.dll`.

- GameAssembly SHA256: `9ebd7ca996a5b03fb4a766f7a2502b660d581ddbbb36af4d7bf2f06a211da39d`
- Assembly-CSharp method pointer table VA: `0x181B82D60`
- Managed MethodDef: `Plant_DetaiPage.Updata_Camera()`
- Token: `0x06000662`
- RID: `1634`
- Pointer-table entry VA: `0x181B86068`
- Native entry VA: `0x1803A4810`
- Runtime-function fragments:
  - `0x1803A4810–0x1803A4872` (98 bytes), SHA256 `c02667b8c9fe744de3e53b00925cceacf93a7304c827aaa39e720fb6f97e81f1`
  - `0x1803A4872–0x1803A4B92` (800 bytes)
  - `0x1803A4B92–0x1803A4BA5` (19 bytes)
  - `0x1803A4BA5–0x1803A4BAB` (6-byte cold null-failure tail)
- Contiguous method span: `0x1803A4810–0x1803A4BAB` (923 bytes)
- Contiguous span SHA256: `1cff925d1002b279b599a7a18bc076ad423f89878bc26b3ec04ed665c2e66ee9`
- The next independent runtime function begins at `0x1803A4BB0`.

## Constant-pool authority

- `0x1815A7C08` = `0.1f`
- `0x1815A7A14` = `1.1f`
- `0x1815A7AB4` = `0.3f`
- `0x1815A7F98` = `880.0f`
- `0x1815A7A10` = `1.0f`
- `0x1815A7A04` = `0.4f`
- `0x1815A7C2C` = `1.4f`

## Recovered semantics

1. `Camera main = Camera.main`; if the Unity object is not live, return.
2. Fetch `Camera.main` again for the working camera reference, matching the original call sequence.
3. If `skill.activeSelf == false`:
   - Smooth camera X toward `board.cameraPosition`:
     `x = x + (board.cameraPosition - x) * 0.1f` while preserving current Y/Z.
   - If `board.mouseManager.handItemType != ItemType.PlantSkillCrosshairs`, target orthographic size is `board.map.cameraSize`.
   - Otherwise target size is `board.map.cameraSize * 1.1f`.
4. If `skill.activeSelf == true`:
   - Smooth camera X toward `skill.transform.position.x * 0.3f`:
     `x = x + (skillX * 0.3f - x) * 0.1f`, preserving current Y/Z.
   - Compute `ratio = board.map.cameraSize / 880f`.
   - Clamp ratio to `[0,1]` using the original comparison behavior. Native unordered/NaN comparisons fall through to the raw ratio rather than forcing either bound.
   - Target size is `(1.4f - ratio * 0.4f) * board.map.cameraSize`.
5. Smooth the camera's orthographic size toward the chosen target:
   `orthographicSize = orthographicSize + (targetSize - orthographicSize) * 0.1f`.

Key native sections:

```text
0x1803A4843  call Camera.get_main
0x1803A4865  call UnityEngine.Object.op_Implicit
0x1803A486C  je   return
...
0x1803A489A  call GameObject.get_activeSelf
0x1803A48A1  jne  skill-active path
...
0x1803A4912  load board.cameraPosition
0x1803A491C  (targetX-currentX)
0x1803A492C  * 0.1f
0x1803A4930  + currentX
0x1803A495F  call Transform.set_position
...
0x1803A497E  load map.cameraSize
...
0x1803A4A2A  map.cameraSize * 1.1f
...
0x1803A4AAC  load skill.transform.position.x
0x1803A4AB0  * 0.3f
0x1803A4ABD  * 0.1f after subtracting currentX
0x1803A4AF0  call Transform.set_position
0x1803A4B14  map.cameraSize / 880f
0x1803A4B1F..0x1803A4B39 clamp to [0,1], preserving NaN fallthrough
0x1803A4B3C  * 0.4f
0x1803A4B4C  1.4f - value
0x1803A4B50  * map.cameraSize
0x1803A4B5A  Camera.get_orthographicSize
0x1803A4B68  Camera.get_orthographicSize
0x1803A4B6D  * 0.1f
0x1803A4B77  + current size
0x1803A4B7E  Camera.set_orthographicSize
```

If runtime promotes this method, reconstruct the complete MethodDef with typed `Vector3` values and public Unity references. Do not attempt to repair the existing Cpp2IL object/pointer arithmetic piecemeal: the current managed body has numerous invalid Vector3 operations and a damaged clamp comparison.
