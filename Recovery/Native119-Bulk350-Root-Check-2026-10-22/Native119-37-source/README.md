# Native119: all final 37 direct method sources

2026-10-22. All 37 outstanding direct entries now have native-backed CIL emitter source and metadata declarations. The historical 381 direct worklist entries plus 6 explicit companions now have 387 source proposals. This is source completion only: no Native119 candidate, semantic qualification, Unity/Xcode or IPA result.

SOURCE-DECLARATIONS.json records exact tokens, local signatures, member/type/method references and EH labels. CURRENT-SOURCE.json pins Native93 CIL; ORIGINAL-SOURCE.json and per-token original.asm preserve the original PC native input. BOXING-DISPATCH-TABLES.json records all six native inline dispatch tables. WORK-QUEUE.json records all 37 source targets, not accepted targets.

Input hashes: Native93 b074224112d46a959de074d3090ab6d0614b036959c66c63b4f49954472e2162; original PC GameAssembly 9ebd7ca996a5b03fb4a766f7a2502b660d581ddbbb36af4d7bf2f06a211da39d; global metadata ad992341add498bd1018ac980f42171e0737b0793bf945c8714ad70a445b36b9 (see immutable source input records for canonical digest).

No constructor/null-check shortcuts, global repairs, or verifier relaxation. Preserve original float32 operations, literal UTF8-to-UTF16 mapping, foreach cleanup, coroutine states, Unity null checks, callback order, retained/native quirks and dependencies. Existing helper calls and native inlines require candidate-bound behavior coverage; source reconstruction alone is not proof.

Metadata extension includes one new public TMPro TypeRef, TMP_TextElementType, for the character elementType field. A future aggregate composer must explicitly append this row and account for metadata index widths. Bulk350 already has only 461 planned #Blob bytes below its no-widening guard; do not bypass that guard to force 387 methods into the old format.

The user's current authorization is root completion of these 37 sources, then root testing of the frozen Bulk350. Bulk350 source history remains immutable; checks and subsequent corrections belong in a separate verification/revision directory. No preceding AGY Python-simulation result is inherited as candidate behavior evidence.

All37 source files subsequently passed a standalone source compilation at `audit/bulk350-root-verification-2026-10-22/logs/09-compile-native119-source.*`. This did not invoke the37 emitters or create their candidate. Bulk350 root static-check results are recorded separately.
