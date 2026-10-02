# Native30 independent correction — 2026-10-02

Candidate: `6e4f81c860236b57261905a24eb7ba344b15a332eded64cc628d2c482ffcbb55`. Input is pinned Native29 `4572e7e2...`; source parent is `07202fc86fee30687f049f77a82ab861f66d84d1`.

The prior revised candidate still retained camera x/y in Return, contrary to PC native. Return now disables its own GameObject and writes (0,0,old z), preserving the two camera reads. SunFall fixture now observes the actual CreateProject Vector3 argument and counter state after failed calls. Distinct audio clips and three index mutants expose wrong selection. Prior native contract descriptions were independently corrected; see NATIVE-REVIEW.json.

1970 actual CLR positive cases, 68 negative controls, zero tool errors; all pass. 1915 positive cases are inherited Native29 cases, not newly recovered methods. Actual old Return candidate, old Unity null candidate, and an actual SunFall wrong-position DLL are rejected by positive assertions with zero tool errors. Earlier Native24–28 fixtures pass separately.

2280 non-target raw bodies, existing metadata rows/heaps, declarations, method RVAs and PE sections remain byte-identical. Two builds match. Non-target and metadata-row corruption negative controls are rejected. stind.r4 support was added to the typed checker with 12 positive/negative controls; unsupported operations still fail. This is an instruction-subset checker, not a complete ECMA verifier.

Fresh historical IL2CPP CLI conversion passes. Full Linux Clang syntax check covers 264 translation units: 5920 diagnostics, 24 failing units, 599 errored game methods remain. All 17 targets are clear and 2250 non-target generated C++ methods are unchanged. The compiler-check postprocess was rerun after correcting report key/name errors; it did not rerun the compiler or mask compiler failures.

No new Unity Editor export, Apple Xcode build, IPA or device test. Helper doubles do not prove helper implementations. ShootingTrigger text is pinned to managed metadata but not independently native string-decoded here. Full-game high-fidelity acceptance remains outstanding.

Generated Native26–28 caches were copied with per-file SHA verification to /tmp/godspvz-preserved-generated-2026-10-02 and symlinked at original paths to restore Codespace disk space. /tmp is ephemeral; preserve needed generated output in published archives before stopping the instance. Original inputs, candidate DLLs, reports and source were not removed.

Native31 continues on the same repair branch. Its pending candidate is a separate twelve-method scope and must independently qualify before adoption. Do not infer its acceptance from this checkpoint.
