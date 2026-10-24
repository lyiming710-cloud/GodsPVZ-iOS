# Startup R12: three native initializer trampoline Int32 sentinels

Candidate `28946ce7343a4ca7479fb1a3aeb6b191bcc74aae5292a8b0a18f06c5019e1bd5` (19,731,968 bytes), directly based on R11 `e101c701…c107637`. Exactly three methods change: System0.Start0 (`0x06000064`), System1.Start1 (`0x0600007C`) and System3.Start3 (`0x0600009A`). Each replaces the invalid I8 4294967295 with I4 -1 and four NOPs in its existing nine-byte slot. All metadata, headers, RVAs, CodeSize, locals, MaxStack, branch/EH offsets and the other 2,314 MethodDefs remain identical. Construction is reproducible and separate independent raw/CIL/native binding verifies the full file, including a physical unrelated-code rejection control.

## Exact original specification

The original PC functions at 0x18030A020, 0x18030AF30 and 0x18030D710 each have exactly three instructions: clear R8D (null hidden MethodInfo), load EDX=FFFFFFFF (Int32 -1), and tail-jump to the loader. Each actual tail address is independently matched through the original PC method-pointer table to the existing CIL callee: LoadPlantData (`0x06000073` -> 0x180308BD0), LoadZombieInfo (`0x06000081` -> 0x18030A600), LoadSuppliesInfo (`0x0600009E` -> 0x18030D140). No loader body is changed or guessed.

Original native bytes execute against controlled loader callbacks and all actual candidate CIL instructions execute in concrete CLR methods. All 90 records match across null/non-null receiver identities, Int32 boundary states and mutating callbacks. Native callback asserts hidden MethodInfo is null; original caller bytes are rechecked after hook setup. Real loader internals, native exception unwind, Unity and full gameplay are outside scope. This proves the three caller trampolines under those explicit callee contracts.

Physical wrong-zero and early-return faults each produce 90 differing records. Restoring all three original I8 faults yields exactly the R11 DLL and zero runtime differences on this permissive CLR. The unchanged frozen type checker independently rejects all three physical bad-width methods. Both the non-detection and rejection are retained; execution alone is insufficient. All three actual repaired methods also prepare under CLR JIT.

## Current whole-assembly state and compiler boundaries

Fresh type checking covers all 2,297 bodies: 2,141 PASS/156 FAIL_OR_UNSUPPORTED raw, or 2,169 PASS/128 FAIL_OR_UNSUPPORTED after exact nongeneric InterfaceImpl facts. This does not turn unsupported operations into proven game defects or proven correct behavior. The unchanged visibility auditor exits 2 with 145 rejected sites across 70 methods and zero unresolved members. Zero unterminated lexical tails remain. Detailed overlapping queues are archived; they are not an additive gameplay bug count.

The prior 13 controlled caller scopes were last rerun on exact R11 with 5,583 matching records; R11 also has 532 isolated pure-operation cases. Those 13 caller bodies and all metadata are unchanged in R12, but no rerun of those records on this exact R12 DLL is claimed. They are kept separate from R12's 90 new records. A fresh whole-assembly JIT run is not claimed; the three changed methods are directly checked.

R11 separately passed fresh IL2CPP and 756 C++/274 C Linux checks at run 40121851208. R10 separately passed physical Apple arm64 syntax checks on all 1,030 generated sources at run 40121853897. There are 17 source-file differences between the generated R10 and R11 trees, so the R10 Apple result is not silently applied to R11 or R12. R12 has no fresh IL2CPP, Unity export, Apple compilation, link, IPA or iPad acceptance. Wider type/access and gameplay scopes keep full player promotion held.

## Reproduction

Use the exact R11 parent and fresh paths for construct_and_static.py; build runtime/Fixture.csproj, run run_runtime.py, then verify_independent.py to complete the frozen-width countercontrol and raw proof. Whole-driver source hashes are recorded in DRIVER-DERIVATION. Native execution requires Windows x64 and immutable original GameAssembly SHA `9ebd7ca996a5b03fb4a766f7a2502b660d581ddbbb36af4d7bf2f06a211da39d`. Existing authentic support, pinned frozen checker/Cecil exporter/access auditor and .NET/Cecil dependencies are required as in R11. The ZIP is an evidence/source bundle rather than a full SDK distribution; per-member and top-level SHA manifests are verified.
