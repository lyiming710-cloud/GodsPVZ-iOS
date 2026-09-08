# HF11 — BuffManager.Update<T> shared generic native recovery evidence

## Target
- Managed definition: `BuffManager.Update<T>(T host)`
- Managed token in the recovered Assembly-CSharp: `0x06000101`
- PC shared generic native instance: `0x180429970`–`0x180429D93` (`0x423` bytes, PE `.pdata` boundary)
- Primary source: original PC `GameAssembly.dll` plus the audited HF10 managed assembly.
- Confidence: **Exact** for managed-observable behavior of the recovered generic method.

## Why the open generic definition is recovered
A full PC-code scan for direct `E8 rel32` calls to `0x180429970` finds exactly two callsites:
- `0x18035C155`, inside the Plant combat-update path.
- `0x18036D23D`, inside `Zombie.Update`.

The same native body therefore serves more than one reference-type host; it is not a Zombie-only specialization. The recovered managed body remains open over method generic parameter `T`.

## Native behavior recovered
Field offsets match `BuffManager` serialization order:
- `+0x10` = `buffs`
- `+0x18` = `buffs_toAdd`
- `+0x20` = `buffs_toRemove`

The native instance performs, in order:
1. Enumerate `buffs_toAdd`; for each non-null Buff, add it to `buffs`, then invoke `Buff.Start<T>(host,false)` (`0x18042A660`).
2. Clear `buffs_toAdd`.
3. Enumerate `buffs`; invoke `Buff.Update<T>(host,false,this)` (`0x18042AF00`).
4. Enumerate `buffs_toRemove`; remove each Buff from `buffs`, then invoke `Buff.End<T>(host,false)` (`0x18042A110`).
5. Clear `buffs_toRemove`.

All three enumerations retain real `List<Buff>.Enumerator` `try/finally` + `Dispose` semantics. The managed reconstruction uses canonical `List<Buff>.Add/Remove/Clear` calls for compiler-inlined BCL operations; the resulting managed-observable behavior and exception/null semantics match the native body.

## Validation
1. HF11 patcher run `34191076272` succeeded after the IL-size floor was calibrated to the independently decompiled 104-instruction body.
2. Real audited HF10 input SHA-256: `0684c52733afd5dd1d45e455ede8ec5355d8b53bb65875506cb97f9a29f07b8a`.
3. Final HF11 output SHA-256: `c990a8be0a615cbd1aed15b08251ae892889c056badc3e4070d7c6c24a6cb07f`.
4. The pre-threshold and final patchers generate byte-identical HF11 DLLs; changing the self-check floor from 120 to 100 changed no emitted IL.
5. Patcher reopen: token `0x06000101`, 104 IL / 337 bytes, exactly three finally regions.
6. ILSpyCmd 11.0.0.9375 decompiles token `0x06000101` with stderr = 0 into three strongly typed `List<Buff>` foreach blocks and generic Start/Update/End calls.
7. Permanent Mono.Cecil recovery auditor run `34191305634` passed OPEN1 and OPEN2; both reads report `BuffManager.Update/1` token `0x06000101`, 104 IL / 337 bytes, while all prior HF recovery checks also pass.
8. Whole-assembly IL isolation: after normalizing method RVAs and private/static physical data labels, HF10→HF11 contains exactly one diff hunk, ending at `BuffManager::Update`.

## Final artifact
SHA-256: `c990a8be0a615cbd1aed15b08251ae892889c056badc3e4070d7c6c24a6cb07f`

## Scope note
HF11 repairs the shared BuffManager update container used by Plant and Zombie paths. It does not claim that every individual `Buff.Start<T>`, `Buff.Update<T>`, or `Buff.End<T>` implementation is already Exact; those remain independently auditable dependencies.
