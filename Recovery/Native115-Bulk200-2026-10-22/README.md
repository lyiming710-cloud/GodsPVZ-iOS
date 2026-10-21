# Native115 / Bulk200 source delivery — 2026-10-22

Five authored method bodies from immutable PC native/current CIL evidence. **Source only: no candidate construction, compilation, runtime tests or new formal acceptance by the primary author.**

New tokens:065C PlantDataUIController.SetData,075C CameraController.LateUpdate,07AA TextMeshSpawner.Start,0753 Benchmark02.Start,07CE TMP_TextSelector_B.RestoreCachedVertexAttributes. This completes source coverage of the original twelve-method input set after Native114's seven bodies.

Aggregate200 bodies =194 direct historical worklist targets+6 companions;187 direct worklist methods still without new source. Expected2,117 non-target bodies;39 finally clauses, none added. MemberRef1192 / StandAloneSig1908 / TypeSpec175 / MethodSpec189;858-byte cumulative table-row payload increment. Three new refs:public Renderer.set_sharedMaterial plus public TMP_MeshInfo.uvs0/uvs2 fields. The UV fields have explicit Vector2[] signatures, separately recorded from their Vector3[] row templates, and12 bytes of new field-signature blobs.

Construct from fixed Native93. Last known executed comparison candidate is Bulk188 SHA1ed55546…25be93; compare against it for twelve body differences (Native1147+Native1155) and2,305 identical bodies. If an independently identified Bulk195 candidate exists, additionally compare for five differences/2,312 preserved bodies. Do not fabricate an unexecuted predecessor hash.

`source/` is the complete aggregate C# patcher source, `native115/` includes the five bodies, untouched input evidence, reference metadata/CIL bindings, and detailed VERIFY-NEXT.md. The metadata/CIL reader never invokes reference methods. The authoring script writes source/metadata descriptions only; it never constructs a candidate or runs tests.

Notable native details:PlantData's exact palette intervals and repeated name calls;camera value captures around fixedDeltaTime/deltaTime and real velocity field reference;different legacy/TMP/Canvas spawner order and object ownership;two-quad restoration of vertices/colors/UV0/UV2 with sequential partial writes. No private TMP textInfo or private Renderer material API shortcut is used.

The source-worklist remainder187 is not a newly measured compiler result. The last executed checkpoint read by the primary author remains Bulk188:IL2CPP exit0 with the overlaid AnimationModule support profile,4,166 Linux errors,199 attributed errored methods,18/18 failed game TUs. Dependency profile comparability, behavior and integration gaps remain independent. No IPA is qualified by this delivery.

AGY owns execution in a fresh Actions workflow/output directory; keep existing work at a durable checkpoint and preserve historical failures. If Bulk195 has not entered conversion, a combined200 run may avoid duplicate downstream conversion while retaining its source checkpoint. Do not restart Codespace, change main/production/frozen verifiers, or claim200/200 acceptance from source counts.
