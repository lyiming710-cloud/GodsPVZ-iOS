# Native33 — grid coordinate dependency closure

Candidate `4ce81321b2d4d7bab3918fb3e8eceb39b2f6124d033d48dddac80ec7b831abb0` derives from Native32 `46f2c279e677a2e02a93c11de7e0620e6ab8803b1e2f338ebbc49b7d93ff0e1e`. Only GetGridPosition (0x0600015B) and GetGridCenterPosition (0x0600015C) change. 2295 other raw bodies, existing managed metadata/declarations/RVAs/PE sections remain byte-identical. Map.GetMapX/GetMapY remain unchanged. Two builds produce identical DLLs.

The center's damaged body copied x and z and lost the helper's y. The helper itself had invalid Vector3 write receivers and object-typed arithmetic, so repairing only the center would preserve a broken dependency. Both complete bodies now follow the original PC flow; see NATIVE-REVIEW.json for signed arithmetic and axis constants.

56,012 actual CLR cases pass across two targets and two unchanged dependencies: 56,000 against executed original PC coordinate/List helper code, twelve null-path CLR assertions supported by native failure CFG. Original PE instructions execute under explicit initialized Map metadata flags and valid constructed Map/List/Row/array layouts. No native throw helpers, imports, DllMain or Unity startup run. Width/height 0..100, signed input edge cases and 8192 randomized grid inputs form a finite test domain. Every Vector3 component's bits, including +0 z, match exactly.

Fixture calls execute the actual candidate GetGridPosition and retained Map helper IL; no handwritten helper supplies their result. DynamicMethods use static explicit-this adapters internally, with each real dependency body dereferencing this; CLR List methods remain real List methods. 10 emitted controls pass. Four actual wrong DLLs (two targets, two retained helpers) are rejected by ordinary assertions, including changed outcomes in the two callers, with zero tool errors. The mutator's tiny-header bug was independently detected as a tool failure, fixed, and retested; it is not counted as semantic rejection.

Both targets pass the frozen Native32 subset type checker, and no prior passing method fails under the same checker. Native24–32 CLR regression suites pass. Deliberate non-target body and old metadata-row changes are rejected by the isolation audit.

Full historical Linux IL2CPP conversion passes. All 264 Clang units checked: 5816 diagnostics, 580 errored game methods, 24 failed units; both targets clear. 2265 non-target generated C++ methods remain unchanged. This is not licensed Unity export or Apple Xcode/device validation; no IPA produced.

The Native32 remaining-method classification is included as a planning inventory. Its categories overlap and never authorize broad object-local/null rewriting. Native34's further evidence is separate from this qualified batch. `replay_native33.py ARCHIVE.zip FRESH_DIRECTORY` replays candidate builds, isolation, original native closure output, CLR controls and four actual DLL rejections; Linux x64, Python3, .NET10 and Clang required.
