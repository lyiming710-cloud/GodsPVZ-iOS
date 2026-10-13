# Renderer material helper: exact metadata and existing iOS player source

This is a source-only follow-up to Native80. No tests, application execution, Editor startup, build, new C++ generation, candidate generation, existing-file changes, downloads of build packages, or installation were performed. Only exact local metadata, existing local export members and two official reference-source pages were read. Writes are confined to this new analysis directory.

## Conclusion and evidence boundary

The claim `Renderer.IsPersistent has NativeConditional("UNITY_EDITOR") and therefore returns false on iOS` is **not supported by the exact inspected Core metadata**. IsPersistent has no such custom attribute and no managed body from which a constant false return could be proven.

There is, however, stronger target-specific evidence than that unsupported attribute argument: the complete, existing **Native18 iOS Xcode export** contains a public `Renderer_get_material` implementation that unconditionally calls private `Renderer_GetMaterial` and returns its result. No IsPersistent call or guard occurs in that exported getter, and no `Renderer_IsPersistent` identifier occurs in its four Unity Core C++ source units. This is statically consistent with the original-PC public/private pointer alias `0x1812FAE70`.

That conclusion is scoped to the identified existing Native18 player export. It does not establish that an ungenerated/uninspected current Native80 player/linker Core necessarily has the same body. The latest target's helper equivalence remains **pending a lineage/source identity bridge or inspection of that actual target helper**. Native80 candidate acceptance is not asserted. Generalizing `IsPersistent is always false in every iOS player` would exceed the evidence.

## Exact full Core custom attributes

Three exact local Core references are identical:

- `audit/stage9-native3/resolver/UnityEngine.CoreModule.dll`
- `audit/stage9-native4/resolver/UnityEngine.CoreModule.dll`
- `audit/local-preflight/exact-editor-tools/resolver/UnityEngine.CoreModule.dll`

Each is 1,532,928 bytes, SHA256 `83192116b34abac2bb1797ec0e3943362be33b028b1eefd2e561d5fdc1c3312f`, MVID `ed526bd9-3af5-4472-9082-ec05c53ac499`.

Exact full-reference Renderer TypeDef `0x02000176`:

| Member | Full-ref token | Relevant flags/body | Custom attributes |
|---|---|---|---|
| `Renderer.IsPersistent()` | `0x06000EC6` | internal assembly visibility, instance, bool return, internal call, RVA zero, no managed body | **empty** |
| `Renderer.get_material()` | `0x06000EC9` | public instance, RVA `0x17234`, 39-byte body with IsPersistent branch | empty |
| `Renderer.GetMaterial()` | `0x06000E72` | private instance internal call, RVA zero | FreeFunctionAttribute only |
| `Renderer.material` property | `0x17000320` | getter above | empty |

The Renderer type attributes are NativeHeader/UsedByNativeCode/RequireComponent; none is NativeConditional. The same metadata parser detects **139 actual NativeConditional attributes elsewhere in this Core**. The absence on this method/property/type is therefore not caused by a parser that silently ignores NativeConditional constructors.

The Core has a TypeRef for NativeConditionalAttribute in UnityEngine.SharedInternalsModule. That merely makes the attribute type available; it does not attach it to IsPersistent.

The exact SharedInternals reference inspected is `audit/local-preflight/exact-editor-tools/resolver/UnityEngine.SharedInternalsModule.dll`, SHA256 `e23589c90c4e67a7e604959df53d6859e7760c31bfeb97b840c7a00d50056fc7`. Its NativeConditionalAttribute class stores Condition, StubReturnStatement and Enabled. Its constructors do not provide the native implementation of an unrelated internal call. Since Renderer.IsPersistent has no NativeConditional instance, neither the UNITY_EDITOR condition nor an assumed default stub can establish this method's return value.

Machine-readable exact metadata, constructor bodies and custom-attribute blobs are in `exact-metadata.json`, produced by `inspect_metadata.py`.

## Official 2022.3 reference source

Unity's official 2022.3 [GraphicsRenderers.bindings.cs](https://raw.githubusercontent.com/Unity-Technologies/UnityCsReference/2022.3/Runtime/Export/Graphics/GraphicsRenderers.bindings.cs) declares Renderer.IsPersistent as an internal extern method at line 116 without a NativeConditional attribute. The public material getter at lines 130-140 checks that method, logs/returns null on true, and calls GetMaterial on false. This agrees with the inspected full-reference managed getter; it does not supply an iOS native false-return implementation.

The official [BindingsAttributes.cs](https://raw.githubusercontent.com/Unity-Technologies/UnityCsReference/2022.3/Modules/SharedInternals/BindingsAttributes.cs), lines 62-97, defines the NativeConditional fields and constructor assignments. It is not native binding-generator output or proof that Renderer.IsPersistent has a particular platform return value. These upstream source pages are context; the exact local China-editor DLL bytes and target export remain the task's primary identity evidence.

## Actual existing iOS player getter

The complete archive inspected is:

`audit/project-archive-2026-09-30/assets/GodsPVZ-Stage9-native18-unsigned-Xcode.tar.zst`

- Size: **350,920,549 bytes**.
- SHA256: **`651bf0da8300107c5ef99173493ed503b9f1f4e681bff3949f510adb3088495f`**.
- Streamed all 3,692 tar entries successfully to archive end.
- No files were extracted into the project; only selected original function text and member identities were recorded in JSON.

Member:

`xcode/Il2CppOutputProject/Source/il2cppOutput/UnityEngine.CoreModule.cpp`

- Size: 2,055,527 bytes.
- SHA256: **`ae9945e6cc3038b1e70082b8f811e1c3708d6bfebe6ba5c209e7acd9bdb4e7b8`**.
- Private GetMaterial definition starts at original member line **24951**.
- Public get_material definition starts at original member line **25095**.

The exact exported public function is recorded verbatim in `existing-player-export.json`. Its operative sequence is:

```text
Renderer_get_material_m5BA2A00816C4CC66580D4B2E409CF10718C15656(this, method)
    L_0 = Renderer_GetMaterial_m890C1B7FAA74CFC4B362EE3E8E61F6778D0EA189(this, NULL)
    V_0 = L_0
    return V_0
```

There is no guard, alternate null return, logging call or other callback in that exported getter. The private function resolves the internal call named `UnityEngine.Renderer::GetMaterial()`, invokes it with the same receiver and returns that pointer.

The four archived Unity Core C++ members inspected have zero occurrences of `Renderer_IsPersistent`:

| Existing player member | SHA256 |
|---|---|
| UnityEngine.CoreModule.cpp | `ae9945e6cc3038b1e70082b8f811e1c3708d6bfebe6ba5c209e7acd9bdb4e7b8` |
| UnityEngine.CoreModule__1.cpp | `f402fe9b140790cf9bf0ffa625ca74a72cafcd4a5da2be74f9b076ace39ce97e` |
| UnityEngine.CoreModule__2.cpp | `9e5714052e5cb3ab4a0d5b99fed67aff46e454b05bbe9a3aadf167280dc02319` |
| UnityEngine.CoreModule__3.cpp | `679a8215425d52ea43ab6fffa38c27853a5092a5f8ba2600ea77ad964eeb42bf` |

This proves the **guard is absent in that actual existing player export**, rather than proving the native bool method globally returns false. The mechanism that removed the editor-reference guard was not established here; it should not be guessed as NativeConditional.

The archive contains no UnityEngine.CoreModule.dll member. Therefore this subtask cannot supply a hash of the historical linked player managed DLL to equate it with an eventual current target DLL. The generated historical helper body itself is inspectible source evidence.

## Truncated copy was not used as player evidence

The shorter sibling path `audit/project-archive-2026-09-30/GodsPVZ-Stage9-native18-unsigned-Xcode.tar.zst` is only 73,990,144 bytes, SHA256 `2ae53261d67c1c813dec9b8b3a89cd8debe506d7417b64dc35f674f885b87a47`. It ends with `tarfile.ReadError('unexpected end of data')` after 2,740 entries, inside Generics__26.cpp. It did not reach Unity Core sources. That truncated copy was explicitly excluded from the helper conclusion.

## Safe claim for the Native80 handoff

Supported: `The original PC public/private material methods share one native pointer, and the complete existing Native18 iOS player export implements public get_material as a direct call to private GetMaterial. The exact full editor reference contains an additional persistent-object guard.`

Unsupported: `Renderer.IsPersistent has NativeConditional("UNITY_EDITOR")`, `iOS necessarily forces that call to return false`, or `the current Native80 player helper has already been qualified`.

The existing-player source closes the helper equivalence question for that identified historical export. Retain the current-target gap until its engine/helper lineage is matched or its actual linked/generated helper is inspected. No new player generation or runtime test was performed to manufacture that evidence.
