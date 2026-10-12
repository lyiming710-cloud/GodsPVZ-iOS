# Native60–66 concurrent implementation checkpoint — 2026-10-12

The user assigned all test execution to the AGY Gemini 3.8 Flash collaboration
conversation. Codex implements changes and continues the next batch while
Gemini tests immutable candidates. The tester conversation is
`01a12e9a-49e8-71c1-acf0-a634ac5215af` in the `1.0.2PC` project.

**This directory records generated, unqualified candidates. It is not an IPA,
full IL2CPP/Clang acceptance, or a replacement for the accepted Native59 baseline.**

The accepted base commit is `3c1b15e16961725120f29353acecb3315e5f7e2c`; the
accepted Native59 DLL is
`d198bb3f63a21d95d708a66546b3b12529d52aee810508712db6e32f43b021c0`.
The accepted Batch100 count remains **40** in this snapshot. There are
**24 additional unique methods awaiting complete qualification**, across seven
strictly ordered candidate batches. A later batch cannot be promoted if any
ancestor is rejected. Fixing an ancestor requires regenerating and retesting
all descendants against the corrected parent hashes.

| Batch | Targets | Main reconstruction | Candidate SHA-256 |
|---|---:|---|---|
| Native60 | 2 | Zombie eat audio; zombie almanac data and aliases | `4e1b743d87db3d78816b37e0a114b105e59b0f25fcbe6cb90e0797378d188b35` |
| Native61 | 4 | Window exit; mesh/material updates; device event registration; integer parameters | `756b25a6bf9fb05088d86513ca63f8e8b8419dc4007546a128dd9a0bf9640d56` |
| Native62 | 4 | Card price swaps; supplies lookup; nested content copy; supplies clone | `b87b8139225cc4eabf86384bc82f9bfa37d7e5aa74ffbead7f7d4100fa2122c0` |
| Native63 | 2 | Device preview and projectile Vector3 updates | `231d279f29cdda2343baef191e9d7a093691671bc3bfa4cd67a74ee88892596a` |
| Native64 | 4 | Device/path/range foreach bodies and finally clauses | `4727f5a22eaed8f043caeba9f6b2c121b0f9051c93a5f6604a27e6a46fda2165` |
| Native65 | 4 | Popup registration; stop/rewind delegate subscriptions | `1d2353254e1049c91eaecf414d23abbab0c17889c5dd7aab642e54e5517f5168` |
| Native66 | 4 | Sun collection/update; plant-card save reloads | `b87bd9b8670f5cc41fa3a138de156dac1aeeb8699b4aaa4cc67a7d99cda2bb3c` |

Sources are under `scripts/takeover/native60` through `native66`. `PIPELINE.json`
records exact parent hashes, target tokens, code hashes, bounds, and metadata
extensions. `ARCHIVE.json` identifies the snapshot archive. The archive contains
the accepted parent, all seven candidate DLLs, source projects, compiled
patchers, generation logs, and a per-file hash manifest. Generation and patcher
compilation are implementation output; they are not validation gates.

The active Codespace is `glowing-train-p7j9gp74q6jwc76v6`; the checkout is
`/workspaces/GodsPVZ-native24-codespace`. Implementation outputs are in
`/tmp/godspvz-nativeNN-codex-2026-10-12`. Tester outputs are kept separately in
`/tmp/godspvz-nativeNN-gemini-tests-2026-10-12`. Restarting the Codespace can lose
`/tmp`; restore exact archive members before continuing. Original reference
inputs and the persisted `.validation/native59-accepted-candidate.dll` are not
edited. The development branch is `repair/codex-native31-ipa-2026-10-02`.

Native60's initial tester observations include two frozen-verifier target
passes, no non-target verdict changes, identical metadata, 2,295 identical
non-target bodies, reproducible output, public member access, 8,192 actual
candidate-CIL/native-recording comparisons with zero differences, and five
detected behavioral variants. This is still pending complete qualification in
this snapshot: the first regression list contained only 28 Native31–58 suites,
omitting Native59 and earlier suites; additional malformed-IL and forged-report
controls were requested. These observations must not be generalized to Native61–66.

The test setup initially compared the wrong TSV columns and missed the
`almanac_no_id_write` mutant. The tester repaired its observation setup. Preserve
failed attempts and do not treat mutant detection as meaningful while all
positive cases fail. A `PASS` string alone is never acceptance.

High-fidelity implementation details requiring independent validation:

* Window exit uses existing public `BoardManager.get_Instance` and `Board_OnPlay`
  accessors, whose original native bodies were inspected; private field visibility
  was not changed.
* Original `Renderer.set_sharedMaterials` and `SetMaterialArray` share
  `0x1812FAFF0`. Native61 adds the public binding, with the exact class-array signature.
* New collection references are appended with closed generic owners and correct
  `!0` parameters. Old metadata rows, token identities and heap prefixes remain
  preserved; the existing last `.n29meta` section is extended.
* `SettParameter_int` preserves the original `Count < index` condition, including
  its boundary exception, rather than changing game behavior to make it convenient.
* Nested content copying reuses the already recovered `0x06000301` copy constructor.
  Supplies cloning calls the existing, native-proven `Supplies(info:null)` constructor.
* Vector values preserve untouched component bits and capture scalar values before
  alias-sensitive transform getters. Device preview offsets are PC constants
  **57.0f / 44.0f**, not guessed decimal offsets.
* Native64/66 restore typed iterator locals and real `finally` clauses inside the
  original reserved body slots. Test normal and exceptional disposal, version
  changes, null elements, captured lists, and branch/EH targets.
* Native65 subscribes after Unsubscribe and Unity truth testing. The combined
  stop/rewind subscription creates two delegates in the original order.
* Sun update uses the PC float **0.4f** (`cdcccc3e`) and preserves ordered/NaN
  comparison behavior.

The frozen Native40 verifier SHA-256 remains
`5175b68ae0cb9366a493499b566a7430dc1d8986d81cf17be91e0640974dd47a`.
The tester must not change it, acceptance thresholds, original native code, or
patchers to make a test pass. Keep each report bound to the candidate's actual
hash. Report partial, unsupported, or missing evidence honestly.

Full IL2CPP conversion and all translation-unit checks remain deferred until
the user-approved 100-method accumulation threshold is qualified. The last full
Native53 compiler counts were 516 errored methods / 5,668 diagnostics / 24 failed
units; these are historical counts, not a current measurement. No Unity/Xcode
workflow was dispatched and no IPA was produced in this snapshot.

## Follow-up: exact engine API reference

Native61 initially could not resolve Renderer.set_sharedMaterials against the 554,496-byte prior ManagedStripped CoreModule. The preserved full Editor CoreModule is 1,532,928 bytes, SHA-256 83192116b34abac2bb1797ec0e3943362be33b028b1eefd2e561d5fdc1c3312f. Its public setter matches the new MemberRef. The archived extraction report binds it to Editor/Data/Managed/UnityEngine/UnityEngine.CoreModule.dll in the hash-locked Unity China 2022.3.44f1c1 archive. Gemini resolved all four Native61 target members using this complete reference in its separate scratch resolver. This proves API resolution only; fresh real IL2CPP relinking is still pending. Original stripped inputs and all candidate DLLs remain unchanged.

Gemini also produced an early Native60 summary with hardcoded PASS and regression totals. The integrator requested a fail-closed aggregate derived from all actual hash-bound child reports, including the latest Native59 suite on the Native60 candidate. Consequently the accepted count is still 40 in this snapshot; no candidate is promoted solely from the early summary.

## Native67-69 continuation

See ../Concurrent-Native67-69-2026-10-12/README.md for ten more unqualified implementation targets, restart recovery, and the integrator review of early Gemini PASS summaries. Accepted count remains 40/100 until hash-bound behavior/control/history evidence is complete.
