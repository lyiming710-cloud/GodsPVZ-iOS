# Bulk388 source checkpoint R1

2026-10-23. Source only: **not compiled, not executed, no candidate DLL, no project tests, no Actions dispatch, no new qualified methods**.

This aggregate contains 388 method-source proposals: 381 historical direct worklist entries, 6 explicit companions and 1 newly discovered header-only method (`0x060003F9 Projectile.SetSortingGroup`). It inherits the actual corrected Bulk350 R6 source, adds Native119's 37 sources and Native120's single source. Native93 `b074224112d46a959de074d3090ab6d0614b036959c66c63b4f49954472e2162` remains the only construction input; the R6 candidate is a comparison reference, not a construction parent.

`Program.cs` pins all 388 original identities/bodies, emits the exact target scope, preserves old body slots and permits only the existing PE/CLR header updates in the original file prefix. The other 1,929 MethodDefs are outside the new write scope. These are construction requirements, not independently observed isolation results.

`MetadataAppend.cs` is copied unchanged from R6. It still rejects heap growth beyond its short-index layout. A distinct second stage, `AppendNative119Metadata.cs` plus `WidthAwareTables.cs`, appends the Native119 metadata and re-encodes all modeled table columns at the resulting heap/table/coded-index widths. It does not merely flip `HeapSizes`, truncate a large offset, or relax the short writer's guard. Existing table RIDs and numeric column values must remain identical through the transcode. Existing heap payload prefixes and original metadata slots are retained. The new path rejects unsupported metadata stream/table formats and appends only to present, unsorted tables.

New rows after the R6 metadata stage: 1 TypeRef (TMPro.TMP_TextElementType in Unity.TextMeshPro), 37 MemberRefs, 37 StandAloneSigs and 3 TypeSpecs; 13 user strings are appended at pinned offsets. New MemberRef signatures are explicit payloads and can differ from the independently pinned template signatures. This matters for Vector2/Touch/Quaternion and similar renamed external references. No new MethodSpecs are required in this delta.

Source-declared final counts: 2,317 MethodDefs, 178 TypeRefs, 1,317 MemberRefs, 2,095 StandAloneSigs, 181 TypeSpecs, 193 MethodSpecs; 201 finally regions. The body and metadata validators must independently derive these from a future candidate. No behavior, compiler or platform qualification is inferred from these declarations.

`Bulk388.csproj` explicitly lists its source files and needs no third-party runtime package for the patcher. `IMPLEMENTATION-PLAN.json`, `METADATA-ADDITIONS.json`, `NATIVE119-METADATA-DELTA.json` and `SOURCE-LINEAGE.json` describe the source scope. Read `VERIFY-NEXT.md` before any execution. The previous Bulk250-R2 28-PASS behavioral claim is rejected and is not inherited.

The author ran only source-data extraction, source-file composition and archival/file-identity operations in this turn. Gemini remains responsible for execution verification. The existing independent verifier assignment is still Bulk250-R2; this source package does not authorize it to abandon that work or launch Unity/Xcode.
