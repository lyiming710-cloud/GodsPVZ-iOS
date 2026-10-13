# Native60 integrator acceptance — 2026-10-13

Decision: **APPROVED_PREBUILD_SCOPE**, limited to `0x0600046D Zombie.PlayEatSound` and `0x060005B9 Almanac_ZombieWindow.SetWindowData(Save,int)`. The qualified unique-method count since Native53 advances from **40 to 42**. This is an explicit integrator promotion based on review of Gemini's executed evidence, not a new test run by the integrator.

Accepted parent: Native59 `d198bb3f63a21d95d708a66546b3b12529d52aee810508712db6e32f43b021c0`. Accepted Native60 candidate: `4e1b743d87db3d78816b37e0a114b105e59b0f25fcbe6cb90e0797378d188b35`. Native61–82 remain pending: 70 unique implemented methods. Accepted plus pending implementations still total112.

## Evidence reviewed

The reviewed Archive-v6.zip was frozen into this directory under a new immutable filename. Its SHA256 is `0b21024f659fdab0caca461b48bde50975af2cfd71c95e8da6ee594466e4fb95`, with75 members. Earlier versions were not used as the final qualified record. The original mutable filename's corrected content differs from the previously referenced v6 hash; this receipt identifies the exact newly reviewed bytes.

The original PC input is31,969,792 bytes, SHA256 `9ebd7ca996a5b03fb4a766f7a2502b660d581ddbbb36af4d7bf2f06a211da39d`. The external-dependency document now binds the actual historical oracle **51,608 bytes**, SHA256 `5a3f28d9bb1d071144061871ae8ec79e183cf5c747dddf388c0d992b125b0565`, and the actual source **11,107 bytes**, SHA256 `b9c070f96fdeb4a669a825e941f7f62586c5de166aeae47a7653807b4e2f1df8`. It no longer substitutes the unrelated current-directory47,824-byte oracle for historical execution evidence.

The master runner embeds that exact native source and the exact actual-CIL emitter source,17856 bytes, SHA256 `90a06d30bc3614764f3427d4ab7f395044be5a24c7dba54b31bee9ef0284c622`. It compiles the embedded native source, invokes the resulting oracle with the fixed original PC input path, saves the raw TSV, compiles the embedded candidate-CIL fixture and executes it on the pinned candidate. These source bindings were read from the runner's AST and compared to the archived source bytes without executing tests.

The oracle source maps the original PE sections at their original image base, calls the two original machine-code entry points, and hooks dependency boundaries. The actual candidate fixture reads each selected candidate MethodBody and emits its CIL into DynamicMethod, retaining the recorded dependency contracts. The final TSV contains8192 rows with26 columns:11 inputs and15 observed outputs. The fixture compares all15 outputs, including trace, exception, receiver effects, state/ID, audio parameters and exact Vector3 component bits.

The final master execution log records candidate reproduction with the exact accepted hash;2295/2295 non-target raw method bodies and complete metadata identical; frozen Native40 verifier SHA256 `5175b68ae0cb9366a493499b566a7430dc1d8986d81cf17be91e0640974dd47a`, both targets PASS with no non-target verdict drift; member access zero rejected/zero unresolved; and8192 actual-CIL cases with zero positive failures. Final oracle TSV SHA256 is `978a8ee336f028c55be988f8866a5aa93dbe106553422ffe4a9e35829761809f`.

All five targeted erroneous semantic variants were observed at **behavior_diff**, with executed cases and concrete first differences:

- `eat_skip_isdisabled`: case1, trace differs.
- `eat_wrong_range`: case1, selected audio index differs.
- `almanac_wrong_state`: case4097, state differs.
- `almanac_no_id_write`: case4225, zombieID differs.
- `almanac_no_null_check`: case4100, trace and exception differ.

The fixture's broad control catch can classify emission/runtime failures as runtime_exception. That remains a harness limitation, but the five accepted behavioral controls above all executed and produced genuine behavior differences; none is justified by a cases=0 emission failure. Malformed IL/DLL rejection is recorded separately.

The master and archived reports also record the existing35 historical suites passing, malformed max-stack/branch/type cases, forged non-target tampering, wrong parent and existing-output rejection. Complete original master/fixture stdout/stderr, runners and historical reports remain in the frozen archive. An older oracle build log mentions an earlier TSV hash `4dfd02…`; it is historical and is not the final8192-row `978a8e…` record used for this decision.

## Qualification boundary

The integrator reviewed source, archived execution records, dependency identities and package contents; **the integrator did not run candidate, native, CLR, fixture, verifier, compiler or Unity/Xcode acceptance tests**. Execution belongs to the original AGY Gemini testing conversation.

Approval covers these two reconstructed callers under their original-native specification and the tested dependency contracts. It does not prove engine internals, complete game behavior, full IL2CPP conversion/all translation units, Unity player acceptance, Xcode build, IPA packaging or device operation. Full conversion/all-TU remains deferred until100 qualified unique methods. The last full compiler result remains historical Native53:516 errored methods,5668 diagnostics,24 failing translation units.

Native61's expanded native outputs still need matching actual-CIL observations and a valid executable set_Item behavioral mutant. Native62 and all later candidate descendants stay pending until their own evidence is reviewed. Native80 additionally retains its current-player material-helper binding requirement. No published candidate or historical snapshot is overwritten by this acceptance.
