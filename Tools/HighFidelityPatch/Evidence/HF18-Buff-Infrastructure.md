# HF18 — Buff infrastructure native recovery

Date: 2026-09-08  
Branch: `high-fidelity`  
Formal input: HF17 final `0eb0eba10cb27c5cff61e1a75947f146f5213ec036ff2ca3f95cf7d406ff1a67`  
HF18 final candidate: `0e1b1acffa3329b3f98d2b61fb346c3f6647bee84e38bfab1d067a7aa17d58ed`

## Why HF18 exists

After HF17 closed the Hide lifecycle, a fresh native call-centrality rescan was run instead of preselecting another patch. The scan crossed two independent signals: remaining Cpp2IL helper/unknown-IL sites in the HF17 assembly and original PC native direct-call centrality. Shared empty/trivial native pointer groups were suppressed so that common stubs such as `0x180302170` could not masquerade as high-centrality gameplay methods.

The strongest active-path result was `BuffManager.GetIncrement(float,string)`: its original PC body has 25 direct incoming E8 callsites spanning Plant and Zombie attack, defense, attack speed, elemental stats, injury, idle/update-rate, armor/body damage, eating and UI paths. Its downstream `Buff.ComputingIncrement` was itself badly Cpp2IL-damaged. The same infrastructure audit also found broken lookup, cleanup and constructor entry points. HF18 therefore restores one coherent Buff infrastructure cluster rather than an isolated manager helper.

## Scope

Seven declared MethodDefs are restored:

- `Buff.ComputingIncrement(float basePoint, string valueName)` — RID 243, token `0x060000F3`, PC `0x180310980`.
- `Buff::.ctor()` — RID 245, token `0x060000F5`, PC `0x180310BB0`.
- `BuffManager.GetIncrement(float basePoint, string valueName)` — RID 260, token `0x06000104`, PC `0x180310420`.
- `BuffManager.EndAll<T>(T host)` — RID 261, token `0x06000105`, generic definition pointer `0`; active Zombie specialization `0x180429310`.
- `BuffManager.FindBuff(string buffName)` — RID 262, token `0x06000106`, PC `0x180310250`.
- `BuffManager.FindStatsIncreased(string buffName)` — RID 263, token `0x06000107`, PC `0x1803103A0`.
- `BuffManager::.ctor()` — RID 264, token `0x06000108`, PC `0x180310630`.

Primary evidence is original PC `GameAssembly.dll` SHA-256 `9ebd7ca996a5b03fb4a766f7a2502b660d581ddbbb36af4d7bf2f06a211da39d` plus original PC `global-metadata.dat` SHA-256 `ad992341add498bd1018ac980f42171e0737b0793bf945c8714ad70a445b36b9`.

## Native ranges / centrality

Logical PC native ranges are:

- `FindBuff`: `0x180310250–0x180310394`.
- `FindStatsIncreased`: `0x1803103A0–0x180310418`.
- `GetIncrement`: `0x180310420–0x180310574`.
- `BuffManager::.ctor`: `0x180310630–0x180310703`.
- `Buff.ComputingIncrement`: `0x180310980–0x180310BA6`.
- `Buff::.ctor`: `0x180310BB0–0x180310C8D`.
- shared `EndAll<Zombie>` specialization: `0x180429310–0x1804293DE`.

After shared-stub suppression, original direct-call counts include `BuffManager.GetIncrement` 25 incoming E8 callsites, `BuffManager.FindBuff` 13, `BuffManager.FindStatsIncreased` 7, and the active `EndAll<Zombie>` direct original call `0x18035E5B1 -> 0x180429310` from `Zombie.DestroyZombie`.

The complete rescan is archived as `HF18-centrality-rescan.txt` / `HF18-centrality-methods.json`.

## Original fields / offsets

`Buff` TypeDef 5115: `name` token `0x04000105` `+0x10`; `duration` token `0x04000107` `+0x1C`; `buffRange` token `0x0400010C` `+0x40`; `childBuffs` token `0x0400010D` `+0x48`.

`StatsIncreased` TypeDef 5116: `valueName` token `0x0400010F` `+0x58`; `value` token `0x04000110` `+0x60`; `multi` token `0x04000111` `+0x64`.

`BuffManager` native instance fields: `buffs +0x10`, `buffs_toAdd +0x18`, `buffs_toRemove +0x20`.

## Restored managed-observable behavior

### BuffManager.GetIncrement

Initialize `total=0`, enumerate `buffs`, throw `NullReferenceException` for a null current Buff as native does, add `current.ComputingIncrement(basePoint,valueName)`, preserve Enumerator `Dispose` in finally, and return the total.

### BuffManager.FindBuff / FindStatsIncreased

`FindBuff` enumerates `buffs`, throws on a null current Buff, compares `current.name == buffName` through `System.String` equality, returns the first match after disposal, and otherwise returns null. `FindStatsIncreased` calls `FindBuff` and performs the native-equivalent runtime cast/type gate to `StatsIncreased`; null or incompatible Buff returns null.

### Buff.ComputingIncrement

Start at zero. If `this` is `StatsIncreased` and `valueName` matches its field, contribution is `value`, multiplied by `basePoint` when `multi=true`. Then enumerate direct `childBuffs`: unlike manager enumeration, a null child is skipped by the native body; non-StatsIncreased children are skipped; each direct child StatsIncreased contributes `ComputingIncrementS(basePoint,valueName)`. Preserve Enumerator disposal and return the sum. Native does not recursively traverse arbitrary child Buffs here.

### BuffManager.EndAll<T>

The generic definition has no direct method pointer. Original `Zombie.DestroyZombie` contains `0x18035E5B1 -> 0x180429310`, attributing the active shared body to `EndAll<Zombie>`. Behavior is `i = buffs.Count - 1`, descend while `i >= 0`, first `buffs[i]` read for null skip, then a second `buffs[i]` read/null check before `Buff.End<T>(host,false)`. EndAll itself does not clear/remove the collection. The restored IL intentionally retains two distinct `get_Item` call sites.

### Constructors

`Buff::.ctor` initializes `name=string.Empty`, `duration=1.0f`, `buffRange=new AttackRange()`, and `childBuffs=new List<Buff>()`. `BuffManager::.ctor` creates separate new `List<Buff>` instances for `buffs`, `buffs_toAdd`, and `buffs_toRemove`.

The PC optimizer tail-transfers both constructors to shared native `0x180302170`, an empty `System.Object::.ctor` body also reused by many trivial methods. The restored managed constructor calls source-valid `System.Object::.ctor` at constructor entry. Because the original native Object constructor has no observable body, this is managed-observable equivalent while producing valid CIL.

## Dependency closure / excluded methods

`StatsIncreased.ComputingIncrementS(float,string)` — RID 248, token `0x060000F8`, PC `0x180325030` — was independently audited before the patch. It has no Cpp2IL helper and its current managed body matches native: name equality, value read, optional multiplication by basePoint. `BuffManager.Update<T>` remains the HF11-backed body. `AddBuff` and `RemoveBuff` were not modified. HF18 does not broaden into unrelated Projectile/Plant/Skill logic.

## Patcher / formal input

HF18 patcher project is `Tools/HF18Patch`. Source implementation commit: `1732c2a817ea49fbc62767d39cbf30be75a9a876`; workflow commit: `7b862a2ecd7c57a1d688d6600c6839eb46cbc82f`; CI run `34222051988` succeeded; artifact ZIP SHA-256/workflow digest is `634f9db3249f5fb9de8782079a6bc6d64f394585177fb5974e5f01bfac35cbf8`.

The formal HF17 DLL was re-fetched from Drive final ID `1RVb5I23-C0LJ3EXCXCnP4KBn9Vuon6vS` and immediately re-hashed to `0eb0eba10cb27c5cff61e1a75947f146f5213ec036ff2ca3f95cf7d406ff1a67` before patching. Two applications of the published HF18 patcher to that same formal input are byte-identical.

Patcher reopen: `Buff.ComputingIncrement` `64 IL / 223 bytes / 1 finally`; `Buff::.ctor` `15 / 51 / 0 EH`; `BuffManager.GetIncrement` `36 / 117 / 1 finally`; `BuffManager.EndAll` `39 / 120 / 0 EH`; `BuffManager.FindBuff` `37 / 126 / 1 finally`; `BuffManager.FindStatsIncreased` `5 / 13 / 0 EH`; `BuffManager::.ctor` `12 / 40 / 0 EH`.

## Permanent Cecil validation

Permanent `Tools/RecoveryAudit/Program.cs` was extended through all seven HF18 targets in commit `18bfd6740faf86307c52edaea74e1445512bcee6`. Workflow `34222352921` succeeded. The published auditor was downloaded and run against the formal candidate; OPEN1 and OPEN2 both pass the cumulative chain and end in `RECOVERY_AUDIT_OK`. Candidate assembly reopen snapshot: 320 types, 2317 methods, 2297 bodies.

## ILSpy / whole-assembly semantic isolation

ILSpyCmd exact version `11.0.0.9375` was run with the full fresh PC reference set reproduced from fixed Cpp2IL source commit `5fb20304df698ffd3d0e664b2a698cd911dc9d57`. Buff type, BuffManager type, and whole HF18 IL stderr are all 0; whole IL contains 319,514 lines.

After normalizing only method RVA and ILSpy physical `I_XXXXXXXX` labels, HF17->HF18 contains exactly seven changed MethodDef blocks: `Buff::ComputingIncrement`, `Buff::.ctor`, `BuffManager::GetIncrement`, `BuffManager::EndAll`, `BuffManager::FindBuff`, `BuffManager::FindStatsIncreased`, and `BuffManager::.ctor`. No eighth MethodDef changes. Six physical unified-diff hunks result because adjacent MethodDefs coalesce. Normalized semantic diff SHA-256 is `40466bcb749014bc2415e4a5a4345b5c551c106af8ceff0fe067a42ffb9459ef`.

## Fidelity classification

**Exact for managed-observable behavior.** Native field effects, loop direction/order, null semantics, type gates, string comparisons, accumulation, direct-child scope, generic End dispatch, and Enumerator disposal are preserved. Constructor Object-call placement is source-valid CIL rather than the optimizer's tail jump, but the original target body is empty, so no managed-observable behavior changes.

## Archive gate

Drive archival is still pending at this Evidence revision. HF18 must not be called final until the candidate DLL, published patcher, rescan, native disassemblies, prepatch evidence, Cecil/ILSpy records, semantic diff, provenance, SHA256SUMS and this Evidence file are uploaded and independently listed.