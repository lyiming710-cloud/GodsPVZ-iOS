# HF12 — Buff.Update<T> shared generic native recovery evidence

## Target
- Managed definition: `Buff.Update<T>(T host, bool child, BuffManager buffManager)`
- Managed token: `0x060000F2` (RID 242)
- PC shared generic native instance: `0x18042AF00`–`0x18042B472` (`0x573` bytes, PE `.pdata` boundary)
- Primary source: original PC `GameAssembly.dll`; Android/CIL used only as secondary signature/context evidence.
- Confidence: **Exact** for the recovered method's managed-observable behavior.

## Native behavior recovered
1. If the Buff instance is `Bleed`, invoke the shared `Bleed.Bleeding<T>(host)` generic instance.
2. If it is `Hide`, invoke `Hide.Updata_Hide()`.
3. If `child == true`, return immediately after those specialized updates.
4. Otherwise enumerate `childBuffs` using the real `List<Buff>.Enumerator` `try/finally` + `Dispose` shape and recursively invoke `Buff.Update<T>(host,true,buffManager)` for every child.
5. If `original` is set and `originalPlant == null`, then evaluate both `originalZombie == null` and `originalProjectile == null`; if both are null, route to the common removal tail. Unity Object null/equality semantics are retained.
6. If `buffRange != null`, `rangeType != Null`, and host is non-null:
   - a Zombie host is checked with `AttackRange.TestInRange<Zombie>` and optional `vfx.transform.position` follow;
   - a Projectile host is checked independently with `AttackRange.TestInRange<Projectile>` and optional VFX follow;
   - other host types are not range-tested in this native instance.
7. `BuffType.Routine` decrements `duration` by one independent `Time.deltaTime` read. The PC `COMISS/JB` ordering is preserved: positive duration **or unordered/NaN** keeps the Buff; ordered duration `<= 0` removes it.
8. `BuffType.Skill` is retained only while `originalPlant` is Unity-truthy and `originalPlant.skillOngoing` is true; otherwise it removes the Buff. Other BuffType values return unchanged.
9. Removal uses the shared `buffManager.RemoveBuff(this)` tail.

## Native-backed fields
- `Buff +0x18` = `buffType`
- `+0x1C` = `duration`
- `+0x20` = `original`
- `+0x28` = `originalPlant`
- `+0x30` = `originalZombie`
- `+0x38` = `originalProjectile`
- `+0x40` = `buffRange`
- `+0x48` = `childBuffs`
- `+0x50` = `vfx`
- `AttackRange +0x38` = `rangeType`

## Relevant native targets
- `0x180428C90` — shared `Bleed.Bleeding<T>` generic instance
- `0x18031C730` — `Hide.Updata_Hide`
- `0x180426DD0` — shared `AttackRange.TestInRange<T>` generic instance
- `0x180310580` — `BuffManager.RemoveBuff`
- `0x18042AF00` — recursive `Buff.Update<T>` call

## Validation
1. HF12 patcher CI run `34192745200` succeeded.
2. Frozen input HF11 SHA-256: `c990a8be0a615cbd1aed15b08251ae892889c056badc3e4070d7c6c24a6cb07f`.
3. Final HF12 SHA-256: `963f24a8323025e424c2e392d4f90ab1d7d48a8f5aa0b671775ac64402788930`.
4. Patcher reopen: token `0x060000F2`, 168 IL / 568 bytes, one childBuff Enumerator finally region.
5. ILSpyCmd 11.0.0.9375 with the same Cpp2IL `development.1736` regenerated dependency/reference directory decompiles the target with stderr = 0 and no target-method warnings. Two provisional `Unknown result type` annotations seen without Unity references disappeared without changing the HF12 DLL; they were reference-resolution artifacts on `Transform.get_position`, not invalid HF12 IL.
6. Permanent Mono.Cecil recovery auditor run `34193329676` passed. Actual OPEN1 and OPEN2 on the final HF12 DLL both report `Buff.Update/3` token `0x060000F2`, 168 IL / 568 bytes, while every prior permanent HF check also passes.
7. Whole-assembly IL isolation: after normalizing method RVAs, `.data cil I_*` labels, and `<PrivateImplementationDetails>` physical data labels, HF11→HF12 contains exactly one semantic diff hunk, ending at `Buff::Update`.

## Final artifact
SHA-256: `963f24a8323025e424c2e392d4f90ab1d7d48a8f5aa0b671775ac64402788930`

## Scope note
HF12 repairs the shared Buff update lifecycle. It does not yet claim that `Buff.Start<T>`, `Buff.End<T>`, `Bleed.Bleeding<T>`, or `AttackRange.TestInRange<T>` are independently Exact; those remain separate native-backed dependencies for subsequent stages.
