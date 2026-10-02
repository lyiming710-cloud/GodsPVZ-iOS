# Native31 — native execution comparison, 2026-10-02

Twelve methods were reconstructed from original PC branch/field/constant evidence. Candidate `66b1234b13fe812adca05cfc92bf01b11f44f6427c112ca2dc101ac713b85e0b` derives from corrected Native30 `6e4f81c860236b57261905a24eb7ba344b15a332eded64cc628d2c482ffcbb55`. No other methods or existing metadata bytes changed.

The oracle maps the SHA-pinned original GameAssembly PE image at its preferred base in a fresh Linux process, with audited Windows x64 calling convention. It invokes only the twelve reviewed arithmetic/lookup bodies and supplies controlled object layouts. No DllMain, Unity initialization/import calls or native throw path is executed. Source, native bytes, constants, TSV outputs and MXCSR observation are archived. The original image is included so oracle output can be independently regenerated.

77,741 actual CLR cases pass: 77,704 against original machine-code outputs, two CLR null checks supported by native failure CFG, and 35 original-native byref alias cases. Float results compare exact bits except NaNs, where classification is compared without a payload promise. 65,536 input combinations cover every signed 16-bit channel value. 28 emitted negative controls pass; twelve separate actual wrong-constant DLLs are rejected by positive assertions with zero tool errors. Actual DLL negative reports predate the alias extension and contain 77,706 cases; their result is not being counted as alias mutant coverage.

Two raw-slot builds reproduce the same candidate. 2285 non-target method bodies, method RVAs, PE sections, declarations and metadata rows/heaps are byte-identical. All twelve pass typed stack checks. Prior Native24–30 CLR suites pass and deliberate non-target/metadata corruption is rejected.

Fresh full IL2CPP conversion passes. Linux Clang checks all 264 units: diagnostics drop 5920 -> 5888, errored game methods 599 -> 587; 24 units remain failing. Twelve targets have zero errors, 2255 non-target generated C++ methods remain unchanged. Generated metadata usage is separately compiled; IL2CPP global metadata changes due to regenerated usage, while managed metadata is unchanged. This is a Linux qualifier, not a licensed Editor iOS export or Apple build.

No new full Unity/Xcode workflow or IPA. Native32 evidence collection has started independently; it is not qualified by these results. Continue native-backed reconstruction and per-batch isolation/regression gates before full workflow dispatch.
