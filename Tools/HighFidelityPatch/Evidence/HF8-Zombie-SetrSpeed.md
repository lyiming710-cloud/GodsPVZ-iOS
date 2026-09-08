# HF8 — Zombie.SetrSpeed(Vector3) native recovery evidence

## Target
- Method: `Zombie.SetrSpeed(UnityEngine.Vector3)`
- Original MethodDef RID: `1145`
- Metadata token: `0x06000479`
- PC native x86-64: `0x180368280`–`0x180368F5F`
- Primary source: original PC `GameAssembly.dll` + original metadata.
- Confidence: **Exact** for the recovered method body.

## Native behavior recovered
HF8 restores the complete PC-native movement-speed helper rather than retaining the damaged Cpp2IL body. The recovered method preserves:
- ID `17` special tail: `rDirection = direction`, then `rSpeed = direction * GetAnimationFrameXSpeed() * sign(direction.x)`.
- Generic path using `sign(direction.x)` and `sign(transform.localScale.x)` independently.
- Snowbeast (ID `13`) pre-pass and stop/deceleration logic.
- `GetMS()` scaling path.
- Acceleration steering and the native double-cross-product turn term.
- Native 1.5 magnitude clamp.
- `BuffManager.FindBuff("Snowbeast.speed")` and `StatsIncreased.value = magnitude(rDirection) - 1.0f`.
- localScale x flip when direction sign differs from scale sign, while preserving native repeated transform/localScale reads.
- Final `rSpeed = rDirection * GetAnimationFrameXSpeed() * sign(localScale.x)` for the generic path.

## Exact constants / evaluation details
- Vector magnitude is emitted from component-level float multiply/add, converted to double for `System.Math.Sqrt`, then converted back to float.
- Small-vector squared-magnitude threshold: `9.9999994E-11f`.
- Snowbeast steering constants include `1.5f` and `0.25f` in their PC-native positions.
- Six independent `Time.deltaTime` calls are retained; they are not cached or folded.
- Native metadata usage slot `0x181BB34B8` decodes to the exact original string literal `"Snowbeast.speed"`.

## Validation
1. HF8 patcher CI run `34182953283` succeeded (head `42b7973760d934d346692a861672b7fbb9a103b8`).
2. Real audited HF7 input was patched successfully.
3. Patcher reopen verification: `Zombie.SetrSpeed` = `1082` IL instructions / `3744` bytes; six `Time.deltaTime` references retained.
4. ILSpyCmd 11.0.0.9375 with the original Unity reference directory decompiled the method with stderr = 0 and no unknown-result / Cpp2IL helper artifacts in the recovered body.
5. Permanent Mono.Cecil recovery auditor run `34183652113` (head `a38ec956e717cd79b6e6971cf336fce6b69a59bc`) passed OPEN1 and OPEN2. Both reads report token `0x06000479`, `1082` IL, `3744` bytes.
6. Full-assembly IL isolation: after normalizing method RVAs and private/static physical data placement, HF7→HF8 contains exactly **one** diff hunk, ending at `Zombie::SetrSpeed`; no other MethodDef changes semantically.

## Final artifact
SHA-256: `4dc9b2486ae41b2d9c340293dada22e74ee6877290e0e79f456b770b92bb4688`

## Scope note
HF8 makes `Zombie.SetrSpeed(Vector3)` Exact. It does **not** by itself make every helper called by `Zombie.Update()` Exact; remaining high-centrality combat/path helpers continue as later HF stages.
