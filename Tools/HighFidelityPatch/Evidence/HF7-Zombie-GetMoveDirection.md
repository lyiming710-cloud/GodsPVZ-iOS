# HF7 Evidence — Zombie.GetMoveDirection

## Scope
- Method: `Zombie.GetMoveDirection()`
- Original MethodDef RID: `1103`
- Metadata token: `0x0600044F`
- PC native address: `0x180361440`
- Native function range: `0x180361440–0x1803617EF`
- Input: audited HF6 DLL
- Output SHA-256: `ed01e0aa7dac968e8203131e6274a4da718ebe5fdcd0dcb3d3bc41d3b0f60ea4`

## Native-backed behavior
- Returns `Vector3.zero` when no path / no unarrived path node is available.
- Iterates `List<EnemyPath>` using the real enumerator shape and `finally`/conditional `IDisposable.Dispose()` semantics.
- Skips nodes whose `EnemyPath.arrived` is true and selects the first unarrived node.
- Builds the movement direction from the selected node and either `previousPosition` or the preceding path point as observed in PC native.
- Applies the x deadzone using `deadzone_distance` and ordered `abs(dx) < deadzone_distance` semantics.
- Uses the native normalization path with `1e-5f` epsilon and double-precision square root conversion.
- Multiplies each final component by `-1f`; the distinct zero-return and normalized-zero paths are retained so signed-zero behavior is not collapsed.

## Validation
- HF7 patcher CI successful.
- Patcher reopens output and verifies token, IL size, one `finally`, native fields/constants and absence of Cpp2IL helper calls.
- ILSpy with the original Unity dependency reference path decompiles the target method with zero stderr/decompiler warnings and strong types (`List<EnemyPath>.Enumerator`, `EnemyPath.position`, `arrived`).
- RecoveryAudit OPEN1/OPEN2 succeeds with `Zombie.GetMoveDirection/0` included as a permanent regression gate: 158 IL / 562 bytes.
- Whole-assembly HF6→HF7 semantic IL comparison, after normalizing Method RVA and physical static-data placement, contains exactly one diff hunk and it is entirely `Zombie.GetMoveDirection`.

## Confidence
`Exact` for `Zombie.GetMoveDirection()` itself. This does not promote its callers or `Zombie.SetrSpeed(Vector3)` to Exact; `SetrSpeed` remains a separate HF8 target.
