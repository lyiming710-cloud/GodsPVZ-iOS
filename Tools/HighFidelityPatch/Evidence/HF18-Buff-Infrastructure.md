# HF18 — Buff infrastructure native recovery

Date: 2026-09-08  
Branch: `high-fidelity`  
Formal input: HF17 final `0eb0eba10cb27c5cff61e1a75947f146f5213ec036ff2ca3f95cf7d406ff1a67`  
**HF18 final: `0e1b1acffa3329b3f98d2b61fb346c3f6647bee84e38bfab1d067a7aa17d58ed`**

## Why HF18 exists

After HF17 closed the Hide lifecycle, a fresh call-centrality rescan crossed two independent signals: remaining Cpp2IL helper/unknown-IL sites in the HF17 assembly and original PC native direct-call centrality. Shared empty/trivial pointer groups were suppressed so common stubs could not masquerade as high-centrality gameplay methods.

The strongest active-path result was `BuffManager.GetIncrement(float,string)`: its original PC body has 25 incoming E8 callsites spanning Plant/Zombie attack, defense, attack speed, elemental stats, injury, update-rate and damage paths. Its downstream `Buff.ComputingIncrement` was itself Cpp2IL-damaged. The same audit found broken lookup, cleanup and constructor entry points, so HF18 restores one coherent Buff infrastructure cluster.

## Scope

Seven MethodDefs are restored:

- `Buff.ComputingIncrement(float,string)` — RID 243, token `0x060000F3`, PC `0x180310980`.
- `Buff::.ctor()` — RID 245, token `0x060000F5`, PC `0x180310BB0`.
- `BuffManager.GetIncrement(float,string)` — RID 260, token `0x06000104`, PC `0x180310420`.
- `BuffManager.EndAll<T>(T)` — RID 261, token `0x06000105`, generic definition pointer `0`; active Zombie specialization `0x180429310`.
- `BuffManager.FindBuff(string)` — RID 262, token `0x06000106`, PC `0x180310250`.
- `BuffManager.FindStatsIncreased(string)` — RID 263, token `0x06000107`, PC `0x1803103A0`.
- `BuffManager::.ctor()` — RID 264, token `0x06000108`, PC `0x180310630`.

Primary evidence: original PC `GameAssembly.dll` SHA-256 `9ebd7ca996a5b03fb4a766f7a2502b660d581ddbbb36af4d7bf2f06a211da39d`; original PC `global-metadata.dat` SHA-256 `ad992341add498bd1018ac980f42171e0737b0793bf945c8714ad70a445b36b9`.

## Native attribution and centrality

Logical native ranges:

- `FindBuff`: `0x180310250–0x180310394`.
- `FindStatsIncreased`: `0x1803103A0–0x180310418`.
- `GetIncrement`: `0x180310420–0x180310574`.
- `BuffManager::.ctor`: `0x180310630–0x180310703`.
- `Buff.ComputingIncrement`: `0x180310980–0x180310BA6`.
- `Buff::.ctor`: `0x180310BB0–0x180310C8D`.
- shared `EndAll<Zombie>` specialization: `0x180429310–0x1804293DE`.

After shared-stub suppression: `BuffManager.GetIncrement` has 25 direct incoming E8 sites, `FindBuff` 13, `FindStatsIncreased` 7. `Zombie.DestroyZombie` contains `0x18035E5B1 -> 0x180429310`, closing active `EndAll<Zombie>` attribution. Full rescan is archived as `HF18-centrality-rescan.txt` and `HF18-centrality-methods.json`.

## Original fields / offsets

`Buff`: `name +0x10` token `0x04000105`; `duration +0x1C` token `0x04000107`; `buffRange +0x40` token `0x0400010C`; `childBuffs +0x48` token `0x0400010D`.

`StatsIncreased`: `valueName +0x58` token `0x0400010F`; `value +0x60` token `0x04000110`; `multi +0x64` token `0x04000111`.

`BuffManager`: `buffs +0x10`, `buffs_toAdd +0x18`, `buffs_toRemove +0x20`.

## Restored managed-observable behavior

`BuffManager.GetIncrement`: enumerate `buffs`, native-equivalent null-current failure, sum `current.ComputingIncrement(basePoint,valueName)`, preserve Enumerator `Dispose`, return total.

`FindBuff`: enumerate `buffs`, compare `current.name == buffName`, return first match after disposal, otherwise null. `FindStatsIncreased`: call `FindBuff` then native-equivalent runtime cast/type gate to `StatsIncreased`.

`Buff.ComputingIncrement`: contribute this object when it is `StatsIncreased` and `valueName` matches; use `value` and multiply by `basePoint` when `multi=true`; then enumerate direct `childBuffs`, skip null/non-StatsIncreased children, and add `StatsIncreased.ComputingIncrementS(basePoint,valueName)`. Native does not recurse arbitrary child Buff trees here. Enumerator disposal is preserved.

`EndAll<T>`: start at `buffs.Count-1`, descend to zero, preserve the native two distinct `buffs[i]` reads/null checks, and call `Buff.End<T>(host,false)` when applicable. It does not itself clear the collection.

`Buff::.ctor`: `name=string.Empty`, `duration=1f`, `buffRange=new AttackRange()`, `childBuffs=new List<Buff>()`. `BuffManager::.ctor`: create three separate lists for `buffs`, `buffs_toAdd`, `buffs_toRemove`. The PC optimizer tail-transfers constructors to an empty shared `System.Object::.ctor` body; restored source-valid CIL calls Object constructor at entry, with no managed-observable difference.

`StatsIncreased.ComputingIncrementS(float,string)` was independently audited and excluded from modification because its current managed body already matches native. Existing `BuffManager.Update<T>`, `AddBuff` and `RemoveBuff` were not broadened into HF18.

## Patcher and formal-input gate

HF18 patcher project: `Tools/HF18Patch`.

- implementation commit `1732c2a817ea49fbc62767d39cbf30be75a9a876`;
- workflow commit `7b862a2ecd7c57a1d688d6600c6839eb46cbc82f`;
- CI run `34222051988`: success;
- artifact SHA-256 `634f9db3249f5fb9de8782079a6bc6d64f394585177fb5974e5f01bfac35cbf8`.

Formal HF17 was re-fetched from Drive ID `1RVb5I23-C0LJ3EXCXCnP4KBn9Vuon6vS`, re-hashed to `0eb0eba10cb27c5cff61e1a75947f146f5213ec036ff2ca3f95cf7d406ff1a67`, and patched. A second application to that same formal input is byte-identical.

Reopen results: `Buff.ComputingIncrement 64 IL/223 bytes/1 finally`; `Buff::.ctor 15/51/0 EH`; `GetIncrement 36/117/1 finally`; `EndAll 39/120/0 EH`; `FindBuff 37/126/1 finally`; `FindStatsIncreased 5/13/0 EH`; `BuffManager::.ctor 12/40/0 EH`.

## Permanent Cecil / ILSpy / semantic isolation

Permanent RecoveryAudit commit `18bfd6740faf86307c52edaea74e1445512bcee6`; workflow `34222352921` succeeded. Published auditor run against HF18: OPEN1 and OPEN2 pass the cumulative chain and end `RECOVERY_AUDIT_OK`; assembly snapshot is 320 types, 2317 methods, 2297 bodies.

ILSpyCmd exact `11.0.0.9375` with the fresh full PC reference set from fixed Cpp2IL source commit `5fb20304df698ffd3d0e664b2a698cd911dc9d57`: Buff stderr 0, BuffManager stderr 0, whole-assembly stderr 0; whole IL 319,514 lines.

After normalizing only method RVA and ILSpy physical `I_XXXXXXXX` layout labels, HF17→HF18 changes exactly the seven declared MethodDefs and no eighth method. Six physical unified-diff hunks occur because adjacent MethodDefs coalesce. Semantic diff SHA-256: `40466bcb749014bc2415e4a5a4345b5c551c106af8ceff0fe067a42ffb9459ef`.

## Fidelity classification

**Exact for managed-observable behavior.** Native field effects, loop direction/order, null semantics, runtime type gates, string comparisons, accumulation, direct-child scope, generic End dispatch and Enumerator disposal are preserved.

## Drive archive — accepted

The formal archive is complete and was independently listed after upload:

- directory: `PVZ GOD/HighFidelity-Recovery-2026-09-08/HF18-Buff-Infrastructure`;
- **formal folder ID: `1hdSQPuV42nk6wx3knep8y8mqM3z8dVtV`**;
- final DLL Drive ID: `1gXxdtyNf8w3vtOGWzu2vlWmsmN_ZH9G4`;
- patcher Drive ID: `1ZwQcIhHdO9w_Pv6-UyccqU_J3bUPezxJ`;
- SHA256SUMS Drive ID: `1NSohvFWm0Y5D2nIc5ZVZ7DGDtLEc42E3`;
- **post-upload listing verified 29 final files** covering final DLL, patcher, rescan, native/prepatch evidence, Cecil/ILSpy, semantic diff, provenance, logs, IL snapshots, Evidence and SHA256SUMS.

A later-created empty duplicate folder is not the formal HF18 archive and must not be used for provenance. With the formal folder above independently listed, the Drive gate is closed and HF18 is final.