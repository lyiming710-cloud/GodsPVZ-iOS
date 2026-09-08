# HF14 — StatsIncreased.Start_stats<T> / End_stats<T> native recovery

## Targets
- `StatsIncreased.Start_stats<T>(T host)` — original RID 247, token `0x060000F7`, direct CodeGenModule pointer `0`.
- `StatsIncreased.End_stats<T>(T host)` — original RID 249, token `0x060000F9`, direct CodeGenModule pointer `0`.
- Formal input: HF13 `a389fcf0f6a97b4ace5cc580fb9704fa9830dbeba9b50edc4c5cad90489ebac3`.
- Primary source: original PC `GameAssembly.dll` + original `global-metadata.dat`.
- Confidence: **Exact for managed-observable behavior**.

## Generic instance attribution and `.pdata`
HF13 direct callsites expose the shared PC generic bodies:
- `Buff.Start<T>` callsite `0x18042A721` -> `StatsIncreased.Start_stats<T>` body at `0x1804ACA50`.
- `Buff.End<T>` callsite `0x18042A26C` -> `StatsIncreased.End_stats<T>` body at `0x1804AC8E0`.

The PE exception table shows the logical bodies are split into runtime-function/funclet entries rather than one label range:

`Start_stats<T>` logical range `0x1804ACA50–0x1804ACBB7`:
- `0x1804ACA50–0x1804ACAB4`
- `0x1804ACAB4–0x1804ACAB9`
- `0x1804ACAB9–0x1804ACAFE`
- `0x1804ACAFE–0x1804ACB44`
- `0x1804ACB44–0x1804ACB63`
- `0x1804ACB63–0x1804ACBAB`
- `0x1804ACBAB–0x1804ACBB7`

`End_stats<T>` logical range `0x1804AC8E0–0x1804ACA47`:
- `0x1804AC8E0–0x1804AC944`
- `0x1804AC944–0x1804AC949`
- `0x1804AC949–0x1804AC98E`
- `0x1804AC98E–0x1804AC9D4`
- `0x1804AC9D4–0x1804AC9F3`
- `0x1804AC9F3–0x1804ACA3B`
- `0x1804ACA3B–0x1804ACA47`

A full executable-section `E8 rel32` scan finds exactly one direct xref to each body: the two HF13 lifecycle callsites above. This is consistent with the same reference-type generic body being reached through the already-shared `Buff.Start<T>/End<T>` lifecycle.

## Native-backed metadata, fields, types, and strings
Original metadata identifies exactly three `StatsIncreased` instance fields; the PC MetadataRegistration field-offset table fixes their runtime offsets:
- `valueName` — token `0x0400010F`, `System.String`, `+0x58`.
- `value` — token `0x04000110`, `System.Single`, `+0x60`.
- `multi` — token `0x04000111`, `System.Boolean`, `+0x64`.

The target native bodies only read `valueName` (`this+0x58`). The Zombie branch reads it once and reuses the loaded value for two comparisons; the later Plant branch performs a fresh read. HF14 preserves this read pattern.

Type-info usages decode as:
- global `0x181BC4620`, encoded `0x20009E7B` -> decoded type index 20285 -> class TypeDef 5219 -> `Zombie`.
- global `0x181BB1FC8`, encoded `0x200083DB` -> decoded type index 16877 -> class TypeDef 5207 -> `Plant`.

String-literal usages decode as:
- global `0x181BAD258`, encoded `0xA00008E5` -> literal index 1138 -> exact UTF-8 `"As"`.
- global `0x181B9FF10`, encoded `0xA00019B1` -> literal index 3288 -> exact UTF-8 `"Ms"`.

## Direct managed call targets
Original Assembly-CSharp CodeGenModule mapping resolves:
- `0x180366BE0` -> `Zombie.ResetAttackSpeed()` (`0x06000471`).
- `0x180366D90` -> `Zombie.ResetMoveSpeed()` (`0x06000472`).
- `0x1803567C0` -> `Plant.ResetAttackSpeed()` (`0x0600039C`).

The comparison target `0x180B76170` is a real mscorlib native alias used by both:
- `System.String.Equals(string,string)` (`0x06000142`), and
- `System.String.op_Equality(string,string)` (`0x06000144`).

The direct machine-code address cannot distinguish those two MethodDefs because they share the same native body. HF14 emits the canonical `op_Equality(string,string)` reference. This does not change managed-observable behavior and is explicitly recorded rather than treating the alias as uniquely attributable.

## Recovered managed-observable control flow
Both `Start_stats<T>` and `End_stats<T>` have the same native-observable behavior:
1. If the reference-type generic host is null, return.
2. If host is `Zombie`, read `valueName` once:
   - `"As"` -> `Zombie.ResetAttackSpeed()`;
   - otherwise `"Ms"` -> `Zombie.ResetMoveSpeed()`.
3. Independently test host as `Plant`; perform a fresh `valueName` read:
   - `"As"` -> `Plant.ResetAttackSpeed()`.
4. Return.

There is no float arithmetic, NaN/unordered comparison, Unity Object lifetime test, enumeration, exception region, or managed finally in either target.

## Patcher and validation
- HF14 patcher CI workflow run: `34198452745` — **success**; publish, usage smoke test, and artifact upload all succeeded.
- CI artifact: `GodsPVZ-HF14Patch-linux-x64`; archive SHA-256 `75e13806aa58a09042fe2c1a29d884601373908773ac2a25cb9b1f0808e5474d`.
- Actual patch input was the formal HF13 DLL; its SHA-256 was rechecked immediately before patching as `a389fcf0f6a97b4ace5cc580fb9704fa9830dbeba9b50edc4c5cad90489ebac3`.
- Patcher reopen on actual output:
  - `Start_stats<T>`: 41 IL / 163 bytes / 0 EH.
  - `End_stats<T>`: 41 IL / 163 bytes / 0 EH.
- A repeat application from the same formal HF13 input produced a byte-identical output.
- Final HF14 SHA-256: `cbe30c99a973f41bbf4bb0f0f56f62d37e1c752c0723159cc07feeb3d2a0efe7`.

### Permanent Cecil regression
- `Tools/RecoveryAudit/Program.cs` was extended with both HF14 targets.
- Recovery-audit tools workflow run `34198547240` — **success**.
- Actual local audit on the final output: OPEN1 and OPEN2 both pass every pre-existing HF3–HF13 target plus both HF14 targets; terminal result `RECOVERY_AUDIT_OK`.

### ILSpy independent readback
Using `ILSpyCmd 11.0.0.9375`:
- member `0x060000F7` decompiles with stderr = 0 and no target-method anomaly.
- member `0x060000F9` decompiles with stderr = 0 and no target-method anomaly.
- Both decompile to strongly typed `Zombie`/`Plant` checks and exact `"As"`/`"Ms"` comparisons. No generic type or overload error is present in either target.
- A pre-existing inline warning remains in untouched `StatsIncreased.ComputingIncrementS`; it is outside HF14 scope and is not produced by either HF14 target.

### Whole-assembly semantic isolation
ILSpy whole-assembly IL was generated for formal HF13 and HF14 with stderr = 0 for both. After normalizing:
- method RVA comments,
- `.data cil I_*` physical labels, and
- `<PrivateImplementationDetails>` physical data-address labels,

the HF13 -> HF14 unified diff contains exactly **two** semantic hunks. They terminate at:
- `StatsIncreased::Start_stats`, and
- `StatsIncreased::End_stats`.

No third managed method changes semantically.

## Final classification
HF14 is **Exact for managed-observable behavior**. Field identity/offset, host type gates, string constants, branch order, `valueName` read multiplicity, and reset-method call targets are native-backed. The only MethodDef-level attribution ambiguity is the explicitly documented mscorlib `String.Equals`/`String.op_Equality` native alias; both map to the exact same native function and semantics.
