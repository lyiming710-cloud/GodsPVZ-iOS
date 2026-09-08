# HF10 — Zombie.Update_Characteristic() native recovery evidence

## Target
- Method: `Zombie.Update_Characteristic()`
- Original MethodDef RID: `1060`
- Metadata token: `0x06000424`
- PC native x86-64: `0x18036B960`–`0x18036BEAF`
- Primary source: original PC `GameAssembly.dll` + original metadata.
- Confidence: **Exact** for the recovered method body.

## Native behavior recovered
HF10 replaces the damaged Cpp2IL characteristic-dispatch body with the PC-native control flow, including the inlined forms of several source-level zombie helpers.

- ID 3 Pole Commander preserves the asymmetric pole/isStant/buff control flow, metadata-backed `"PoleCommander.speed"`, `StatsIncreased.value < 1.5f`, `Time.deltaTime * 0.2f * updateRate`, `ResetMoveSpeed()`, and the exact per-frame pole-jump-test routing.
- ID 13 Snowbeast preserves rest/isStant gating, a fresh `Time.deltaTime`, native NaN comparison behavior, 10-second clamp, and `animator.SetBool("rest", false)`.
- ID 16 Ladder Saboteur preserves C4 placement, detonation countdown, a third independent `Time.deltaTime`, zero clamp/boom, ladder-place test, and `"PlaceTrigger"`.
- ID 18 calls `ZC_ImperialVanguardHarbinger()`.
- ID 23 inlines the PC-native pole jump test: probe point `(fX-rDirection.x*134f, fY-rDirection.y*134f)`, independent boardConfig reads for X/Y, `GetGrid`, float-converted passable comparison against `100f`, and sheath -> common -> bottom Unity-object plant fallback before `"JumpTrigger"`.

## Original metadata / constant evidence
The native metadata-usage slots decode directly from original v31 metadata:
- `0x181BAA888` -> `0xA0001D47` -> `"PoleCommander.speed"`
- `0x181BD3DF0` -> `0xA0001719` -> `"JumpTrigger"`
- `0x181BC7F48` -> `0xA0003D51` -> `"rest"`
- `0x181BAA200` -> `0xA0001D21` -> `"PlaceTrigger"`

Native float bit patterns: `1.5f=0x3FC00000`, `0.2f=0x3E4CCCCD`, `10f=0x41200000`, `134f=0x43060000`, `100f=0x42C80000`; the direction-negation sign mask begins with `0x80000000`.

## Validation
1. HF10 patcher CI run `34186123632` succeeded from head `8ad8f9605b4b27960304e13697925164636e648e`.
2. Real audited HF9 input was patched successfully.
3. Patcher reopen verification: `Zombie.Update_Characteristic` = `257` IL instructions / `904` bytes; exactly three `Time.deltaTime` references; all four metadata-backed strings and native constants present; no `Cpp2ILHelpers`.
4. ILSpyCmd 11.0.0.9375 with the original Unity reference directory decompiled the recovered method with stderr = 0 and strong typed C#.
5. Permanent Mono.Cecil recovery auditor run `34186347163` (head `46b9fdd8b586606422fc4b6b6b6f8fa4f606af30`) built successfully. Local execution against HF10 passed OPEN1 and OPEN2; both reads report token `0x06000424`, `257` IL, `904` bytes, while every previous HF recovery check also passes.
6. Full-assembly IL isolation: after normalizing method RVAs and private/static physical data labels, HF9→HF10 contains exactly **one** diff hunk, ending at `Zombie::Update_Characteristic`; no other MethodDef changes semantically.

## Final artifact
SHA-256: `0684c52733afd5dd1d45e455ede8ec5355d8b53bb65875506cb97f9a29f07b8a`

## Scope note
HF10 makes `Zombie.Update_Characteristic()` Exact. `BuffManager.Update<Zombie>` and remaining path/combat helpers remain later recovery stages.
