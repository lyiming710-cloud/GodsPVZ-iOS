**Latest update: read [LATEST.md](LATEST.md) first. R5 was rejected by real IL2CPP; R6 fixes the confirmed false-accept opcode and is being retested. The R5 scoped static report below is preserved as historical evidence.**

# Native119 final37 source completion / Bulk350 root verification

2026-10-22. Latest user instruction: finish the remaining37 sources, then root checks Bulk350. This checkpoint follows that order. All37 Native119 emitter sources now compile against inherited sources, without constructing a Native119 candidate. Historical source coverage is381 direct worklist entries plus6 companions. These are387 source proposals, not387 accepted repairs or a current compiler-error count.

Bulk350 was tested and corrected independently. Frozen source failed its obsolete StandAloneSig<2048 construction guard. Independent coded-index accounting proved the HasCustomAttribute column was already4 bytes in Native93. Revision guard still checks the actual already-wide parent and reopens every table row width; short heap guards were retained. No original source checkpoint was overwritten.

R5 candidate: `5a8f96cb2dc54a273a5e276a4dc24e138dce15c93873f1eeed75ec961df259a4`, 16,645,632 bytes. Native93 parent: `b074224112d46a959de074d3090ab6d0614b036959c66c63b4f49954472e2162`.

## Observed scoped results

- Construction and repeat construction produce identical bytes. Wrong parent and existing output controls are rejected.
- Full reopened isolation:350 target bodies changed;1967 non-target methods preserved (1947 executable bodies and20 zero-RVA methods);158 finally regions;10 negative controls rejected.
- Exact append-only heaps/rows:180 MemberRefs,293 local signatures,5 TypeSpecs,8 MethodSpecs,24 user strings. #Blob=65,075 bytes,461 bytes below65,536.
- Versus pinned Bulk250R2: exactly100 expected changed methods and2217 identical methods. The inherited helper's hardcoded `all_other_2307_methods_identical=false` is retained in its raw report; the separate interpretation verifies the actual100-method roster rather than treating that obsolete count as a universal PASS.
- Frozen typed checker:334PASS/16FAIL-or-unsupported. Historical extension:348PASS/2unsupported. These original reports remain unchanged.
- Separately reviewed static checker:350/350 supported type-flow checks pass;8 malformed controls rejected. Changes add real resolved interface graphs and one narrowly supported enum constrained-ToString shape. This is not a complete ECMA verifier.
- Member visibility:350 target methods,0 rejected,0 unresolved with the separate nested-private-aware checker. Genuine compiled/executed CLR probe confirms nested-to-enclosing private access is legal. R4's unrelated ResourceManager private-field access is rejected as a real candidate negative control; R5 uses the original public getter.
- Eager reference resolution:1280 MemberRefs,0 unresolved. Engine and Unity AOT BCL inputs are hashed against saved editor extraction manifest. Other project/package inputs retain their prior delivery origin; Unity.VisualScripting.Core.exe is included in the resolver, not replaced by a fabricated DLL.

## What changed

15 method bodies/headers were corrected after the first constructible version. Fixes retain ignored calls while popping unused return values, restore BoardPreview's correct receiver and actual CreateDevicePreview_Device call, pass the controller to the existing wait constructor exactly once, repair4 MaxStack declarations, and replace one illegal cross-type private field access with its native-equivalent public getter. See38-R1-TO-R5-RAW-BODY-CHANGES.json and the detailed correction logs. No global ret/throw replacement, fake-null bypass or non-target repair was introduced.

## Acceptance boundaries and next work

No Bulk350 candidate-bound game behavior fixture/native comparison was executed in this root check. CLR access-rule probe and malformed static models are tool-sensitivity tests, not game behavior evidence. No new semantic qualification, IL2CPP conversion, all-translation-unit Clang result, licensed Unity export, Xcode build, IPA or device acceptance is claimed. Prior AGY handwritten-Python behavior claims remain rejected.

Native11937 still need candidate integration and all their gates. Combining387 targets requires explicit metadata-width design because the350 #Blob already has only461 bytes of short-heap space. Do not remove the heap guard, alter the trusted Native93 baseline, or inherit approvals from source counts. Full semantic tests must execute the real candidate CIL and bind exact input/output/native traces and actual candidate mutants.

## Artifacts / reproduction

SUMMARY.json is the machine-readable status. Logs retain actual invocations, raw stdout/stderr and process exits, including failed construction/checker invocations. Early Windows CLI logs use the host console encoding; source/JSON files are UTF-8. SDK10.0.402/runtime10.0.13 were used; do not relabel these as older10.0.12 results.

Source and evidence ZIPs preserve original audit-relative paths. Keep all frozen source snapshots and the original checker. Restore Native93 at its pinned path and genuine resolver inputs using20-SUPPORT-PROVENANCE.json. Use `.local/dotnet10/dotnet.exe build .../revision-r5/Bulk350.csproj` and the three explicit patcher arguments (Native93 input, fresh candidate path, fresh construction-report path). Each output must be new. Reviewed tools require Mono.Cecil; published project sources retain the exact tested local build paths, which another environment must bind to its verified local toolchain. The SDK and editor binaries are not duplicated in this checkpoint.
