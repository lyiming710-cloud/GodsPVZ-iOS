# HF13 — Buff.Start<T> / Buff.End<T> shared generic lifecycle recovery

## Targets
- `Buff.Start<T>(T host, bool child)` — token `0x060000F1`; PC shared generic native `0x18042A660–0x18042A7F1`.
- `Buff.End<T>(T host, bool child)` — token `0x060000F4`; PC shared generic native `0x18042A110–0x18042A37A`.
- Primary source: original PC `GameAssembly.dll`; final input is audited HF12.
- Confidence: **Exact** for managed-observable behavior.

## Start native behavior
1. Reference-type generic host null -> immediate return.
2. If `this is StatsIncreased`, call `Start_stats<T>(host)`.
3. Enumerate `childBuffs` with real `List<Buff>.Enumerator` try/finally/Dispose semantics.
4. For every child Buff, call `child.Start<T>(host, true)`.
5. The incoming `child` parameter is not used to branch in the PC native instance.

## End native behavior
1. Log the exact string `结束buff`.
2. If `vfx != null` under Unity Object semantics, destroy `vfx.gameObject`.
3. If `this is StatsIncreased`, call `End_stats<T>(host)`.
4. If `this is Hide`, call `End_Hide()`.
5. Enumerate `childBuffs` with real `List<Buff>.Enumerator` try/finally/Dispose semantics.
6. Recursively call `child.End<T>(host, true)`.
7. The incoming `child` parameter is not used to branch.

The PC metadata usage backing the log decodes to string-literal index `0x2328`, whose original UTF-8 value is exactly `结束buff`.

## Validation
- Final HF13 patcher workflow run: `34194921004` — success.
- Audited HF12 input SHA-256: `963f24a8323025e424c2e392d4f90ab1d7d48a8f5aa0b671775ac64402788930`.
- Final HF13 SHA-256: `a389fcf0f6a97b4ace5cc580fb9704fa9830dbeba9b50edc4c5cad90489ebac3`.
- Patcher reopen: Start = 43 IL / 137 bytes / 1 finally; End = 59 IL / 198 bytes / 1 finally.
- The initially written candidate and final threshold-calibrated patcher output are byte-identical.
- ILSpyCmd 11.0.0.9375 with full Unity reference path: Start stderr = 0; End stderr = 0; both decompile to strongly typed generic lifecycle code.
- Permanent Mono.Cecil recovery-auditor workflow run: `34195201832`; actual local OPEN1 and OPEN2 both pass Start/End plus all prior HF3–HF12 checks.
- Whole-assembly semantic isolation: after normalizing method RVAs and physical private/static data labels, HF12→HF13 has exactly **two** diff hunks, corresponding to `Buff.Start<T>` and `Buff.End<T>`.

## Scope note
HF13 completes the generic Buff container lifecycle triad together with HF12 `Buff.Update<T>`. It does not by itself make every specialized hook (`StatsIncreased.Start_stats/End_stats`, `Hide.End_Hide`, Bleed, AttackRange, etc.) Exact; those remain independently auditable dependencies.
