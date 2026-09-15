# Stage9.1 805A next-ctor PC native prefetch

Read-only batch prefetch against the original GodsPVZ 1.0.2 PC `GameAssembly.dll`. This record does not select a blocker, patch a MethodDef, qualify a candidate, or alter the sealed HF55 baseline. Runtime chronology remains authoritative for selection.

Locked PC `GameAssembly.dll` SHA256: `9ebd7ca996a5b03fb4a766f7a2502b660d581ddbbb36af4d7bf2f06a211da39d`
Method-pointer table base: `0x181B82D60`. Extraction was self-checked against Supplies RID604 before this batch.

| Type / ctor | RID | entry | native pointer / range | native SHA256 / note |
|---|---:|---:|---|---|
| Almanac_TalentSystem::.ctor | 1437 | `0x181B85A40` | `0x180391AC0–0x180391BCA` (266 B) | `cd8b7cd491c67f65e18def6efc23352a030a43a97862afc6c2eea7c7c5b46645` |
| Window_T::.ctor | 1804 | `0x181B865B8` | `0x1803B0880–0x1803B1647` (3527 B) | `e03f74996efd73da64dffafcaa52bb23a6ac86159c698a898cd4161518710fa5` |
| FlagMeter::.ctor | 1563 | `0x181B85E30` | `0x18039D230–0x18039D2AE` (126 B) | `3b9b04d203dad93fff7dcfa459f4b17cd9bd7f779d7418c9c2d6e5b4ca5d1fee` |
| Almanac_DeviceWindow::.ctor | 1386 | `0x181B858A8` | leaf `0x18037E1C0–0x18037E1CE` (14 B) | `f45ad37c4c4cf57a18ea2891a76433a467a8c55b6f48907c573db40c236ff6a9`; exact same native leaf as Almanac_ZombieWindow |
| Window_Q::.ctor | 1799 | `0x181B86590` | leaf `0x1803B0590–0x1803B059E` (14 B) | `335bacd138f91ca7ee7cfd02b07f67a0392d938250f9e1fe5977f72f5cc63eda` |
| PathDataEditor::.ctor | 92 | `0x181B83038` | leaf `0x1803061E0–0x1803061EE` (14 B) | `4ad673a89e631b0de2f3d08ae0d0cadeac3ad6bf354f6d7bf3f9ac84e46786e0` |
| System0::.ctor | 123 | `0x181B83130` | `0x18030A030–0x18030A14C` (284 B) | `713893ee2471f7f22e47ecdfdac1f46f5cf2aab5711a9158d543d5b8cfb85bda` |
| Administrator::.ctor | 12 | `0x181B82DB8` | `0x1802FEB60–0x1802FED13` (435 B) | `0fc07b9a066f57d91a828c58f90fca975de1e77bebf89d15b1f404c7b3f51da5` |
| System3::.ctor | 162 | `0x181B83268` | `0x18030D720–0x18030D779` (89 B) | `8d2257bd236bd71ff8ebe92ec8ed16084e34889b00196f01121db926da60041e` |
| SuppliesInfo::.ctor(int) | 336 | `0x181B837D8` | `0x180325180–0x1803251E8` (104 B) | `8f3ec66297f376a23bbc8330f86db9030203d2f8ba4b3c5a2cbc26c49f1a437a` |
| TextLink::.ctor | 1765 | `0x181B86480` | leaf `0x1803ADFA0–0x1803ADFBF` (31 B) | `699419627a7893a66b38156beaf1ff5b0fe8530e3015e6fab90d976d7b70669c` |
| LevelItem::.ctor | 1581 | `0x181B85EC0` | `0x18039F130–0x18039F365` (565 B) | `00de368a2072ce9b97ae6e5211dd9262b61537bbd1be1d608638c09cdf7e295c` |

Observed targeted semantics useful for later triage:

- `Almanac_TalentSystem`: native ends with `mov dword ptr [this+0x64], 0xffffffff` then tail-jump `0x1812E22A0`; managed damaged field is Int32 `previewID`.
- `Window_T`: native contains `mov dword ptr [this+0x20], 0xffffffff` and eventually tail-jumps `0x1812E22A0`; managed damaged field is Int32 `T`.
- `FlagMeter`: native contains `mov dword ptr [this+0x34], 0xffffffff`, then constructs/stores its list and tail-jumps `0x1812E22A0`; managed damaged field is Int32 `theFlagID`.
- `Almanac_DeviceWindow`: exact leaf bytes are identical to already-authoritative Almanac_ZombieWindow and write Int32 `-1` at offset `0x28`.
- `Window_Q`: leaf bytes `33 d2 c7 41 68 ff ff ff ff e9 02 1d f3 00`; writes Int32 `-1` at offset `0x68`, then base tail-call.
- `PathDataEditor`: leaf bytes `33 d2 c7 41 38 ff ff ff ff e9 b2 c0 fd 00`; writes Int32 `-1` at offset `0x38`, then base tail-call.
- `System0`: native contains two distinct 32-bit `-1` stores (`[this+0x50]` and `[this+0xD0]`), matching the managed ctor's two damaged I8 constants; this requires a target-specific repair, not a generic single-op patch.
- `System3`: native writes 32-bit `-1` at `[this+0x28]`, stores the empty-string field, then base tail-calls.
- `TextLink`: not an I8/-1 ctor pattern. Its 31-byte native leaf initializes an integer sentinel and two 128-bit color/vector fields before the base tail-call. It must not reuse an I8→I4 patch template.
- `LevelItem`: large ctor and managed private-List/internal corruption; requires full target-specific native reconstruction if runtime selects it.

Before any repair, the new runtime log must select the next causal MethodDef. Each selected MethodDef still needs a dedicated authority/qualification record and exact-R3 runtime pass.
