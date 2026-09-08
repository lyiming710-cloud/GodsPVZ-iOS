# HF19 — Zombie classification / disabled predicates native recovery

Date: 2026-09-08  
Branch: `high-fidelity`  
Formal input: HF18 final `0e1b1acffa3329b3f98d2b61fb346c3f6647bee84e38bfab1d067a7aa17d58ed`  
**HF19 final: `e6e303c660b0351611370ebe29746954c5535344160c6e580d9314405b67f720`**

## Why HF19 exists

After HF18, an active-path rescan suppressed shared/trivial native-pointer groups before ranking remaining damaged methods. `Zombie.IsDisabled()` was the strongest unique active predicate: the rescan reports 34 direct E8 callsites plus one secondary/native-reference attribution (35 active references), while the HF18 managed body still contained unresolved Cpp2IL jump-table helpers. Its dependency `IsPlantZombie()` and adjacent `IsNormalZombie()` were also damaged by unresolved native jump tables. HF19 therefore restores these three methods as one coherent Zombie predicate cluster.

Other rescan candidates such as `Plant.GetDamage`, `ProjectileManager.CrateNewProjectile`, and `Damage.AreaDamage` were deliberately not widened into this stage. They remain inputs to the post-HF19 decision rescan.

## Scope / native attribution

- `Zombie.IsDisabled()` — RID 1124, token `0x06000464`, PC `0x1803659E0`.
- `Zombie.IsNormalZombie()` — RID 1126, token `0x06000466`, PC `0x180365AB0`.
- `Zombie.IsPlantZombie()` — RID 1127, token `0x06000467`, PC `0x180365B00`.

Original PC code regions before inline jump-table data are `0x1803659E0–0x180365A7D`, `0x180365AB0–0x180365ADD`, and `0x180365B00–0x180365B2E` respectively.

Primary source: PC `GameAssembly.dll` SHA-256 `9ebd7ca996a5b03fb4a766f7a2502b660d581ddbbb36af4d7bf2f06a211da39d`; PC `global-metadata.dat` SHA-256 `ad992341add498bd1018ac980f42171e0737b0793bf945c8714ad70a445b36b9`.

## Original fields / offsets

- `Zombie.ID` — token `0x040005B9`, `Int32`, native `+0x60`.
- `Zombie.isDying` — token `0x040005D1`, `Boolean`, native `+0xB7`.
- `Zombie.isDied` — token `0x040005D2`, `Boolean`, native `+0xB8`.
- `Zombie.ashes` — token `0x040005D3`, `Boolean`, native `+0xB9`.

## Decoded native truth tables

`IsNormalZombie()` is true exactly for `{0,2,4,5,14,15}` and false for every other Int32 ID.

`IsPlantZombie()` is true exactly for `{6,7,8,9,11,12,19,20,21,22}` and false for every other Int32 ID.

`IsDisabled()` preserves native branch order:

1. if `isDied`, return true;
2. if `ashes`, return true;
3. IDs `{0,2,4,5,14,15}` return `isDying`;
4. otherwise call `IsPlantZombie()`; if true, return `isDying`;
5. explicit IDs `{1,10,3,23,16}` return `isDying`;
6. otherwise false.

Thus, after the two unconditional true gates, the complete ID set whose result is `isDying` is `{0,1,2,3,4,5,6,7,8,9,10,11,12,14,15,16,19,20,21,22,23}`. HF19 intentionally retains one call to the independently restored `IsPlantZombie()` rather than flattening that union.

Nearby `IsFlagZombie`, `IsPoleZombie`, and `IsWinTarget` were audited but remain unchanged because their current bodies are sufficiently reliable.

## Patcher / formal input

HF19 patcher project: `Tools/HF19Patch`.

The first patcher build (run `34223888878`) compiled, but the first formal application was rejected only by an internal verifier threshold: generated `IsDisabled` measured 52 IL instructions while the verifier minimum was 55. That failed application is not a candidate and is not part of the final provenance. No recovery semantics changed.

Verifier-only correction commit: `4994778de21b3403f0a54c82879a07d4e05e77b1`. Corrected CI run `34224218252` succeeded. Corrected artifact SHA-256: `2ed8f8fed6f62cf7f581e5ef1be774858862579a5c077be0fa85072579dcc076`.

Formal HF18 was re-fetched from Drive final ID `1gXxdtyNf8w3vtOGWzu2vlWmsmN_ZH9G4` and re-hashed immediately before patching to `0e1b1acffa3329b3f98d2b61fb346c3f6647bee84e38bfab1d067a7aa17d58ed`. Two independent applications of the corrected published patcher to that same formal input are byte-identical.

Reopen:

- `IsDisabled`: `52 IL / 208 bytes / 0 EH`.
- `IsNormalZombie`: `25 IL / 98 bytes / 0 EH`.
- `IsPlantZombie`: `37 IL / 154 bytes / 0 EH`.

## Permanent Cecil validation

Permanent RecoveryAudit was extended in commit `e14cf3cb918a99aafc7e946c031b2f45dc798dc4`; workflow `34224367146` succeeded. Running the published auditor against HF19 gives OPEN1 and OPEN2 passes across the cumulative chain, including all three HF19 targets, and ends `RECOVERY_AUDIT_OK`. Assembly snapshot remains 320 types, 2317 methods, 2297 bodies.

## ILSpy / whole-assembly isolation

ILSpyCmd exact `11.0.0.9375` was run with the full 56-DLL PC reference set freshly reproduced from fixed Cpp2IL source commit `5fb20304df698ffd3d0e664b2a698cd911dc9d57`.

- Zombie type readback stderr: 0.
- target member `0x06000464` stderr: 0.
- whole HF19 IL stderr: 0.
- whole HF19 IL: 319,456 lines.

The restored target bodies decompile as clean strongly typed boolean predicates. After normalizing only method RVA and ILSpy physical `I_XXXXXXXX` data/private-implementation addresses, HF18->HF19 changes exactly the three declared MethodDefs and no fourth method. The unified diff has two physical hunks because `IsNormalZombie` and `IsPlantZombie` are adjacent. Semantic diff SHA-256: `a90fbb0a5cc96c2a341bbd19dfce6e4e97aff4d4330863e76fb4bf3625595631`.

## Fidelity classification

**Exact for managed-observable behavior.** Original field reads, early-return ordering, all native ID truth sets, dependency call shape, and negative/out-of-range ID behavior are preserved. Native indirect jump tables are represented as explicit managed equality branches, which is managed-observably equivalent.

## Drive archive — accepted

HF19 archive was uploaded and independently listed:

- directory `PVZ GOD/HighFidelity-Recovery-2026-09-08/HF19-Zombie-Predicates`;
- folder ID `1-PpX9-cAmJ7Sx_eUdC0QOSh5GSgGvZSo`;
- final DLL Drive ID `1kxSsPudimmcq9o_evxLqSIVTavoF4DZ9`;
- corrected patcher Drive ID `1R2BSes6C-p08B741i7375XhKKlnXWM_w`;
- semantic diff Drive ID `1Jza7oDQgiSHNdBDyuLCcLFnCXVDcScuh`;
- SHA256SUMS Drive ID `1f2BqIp0vAH1qE0qaPK0HiHSjjtGBl6xW`;
- independent post-upload listing verified **17 final files** covering final DLL, corrected patcher, native/jump-table evidence, centrality rescan, pre/post IL, Cecil/ILSpy, semantic diff, patch logs, provenance, Evidence snapshot, and SHA256SUMS.

All HF19 gates are therefore closed.