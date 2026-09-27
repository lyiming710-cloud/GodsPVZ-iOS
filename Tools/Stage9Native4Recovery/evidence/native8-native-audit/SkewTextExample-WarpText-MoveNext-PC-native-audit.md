# TMPro.Examples.SkewTextExample/<WarpText>d__7::MoveNext — PC-native audit

Status: **read-only authority / next-stage evidence; no managed patch and no promotion**.

The current real iOS IL2CPP gate selected this generated iterator method as one of two remaining `Cannot get stack type for Vector3` blockers. This audit deliberately does not fold it into native7; native7 is reserved for the TextLink + Ladder closure.

## Locked identity

Authority is the original GodsPVZ 1.0.2 PC x86-64 IL2CPP release.

- package SHA256: `2f08b4e2243e2bd296f3db694b4a2c9656252d76202c80167bdf3340b9f65b48`
- `GameAssembly.dll` SHA256: `9ebd7ca996a5b03fb4a766f7a2502b660d581ddbbb36af4d7bf2f06a211da39d`
- `global-metadata.dat` SHA256: `ad992341add498bd1018ac980f42171e0737b0793bf945c8714ad70a445b36b9`
- parent method: `TMPro.Examples.SkewTextExample::WarpText()`, MethodDef `0x06000772`, RID `1906`
- generated iterator: `TMPro.Examples.SkewTextExample/<WarpText>d__7::MoveNext()`, MethodDef `0x06000776`, RID `1910`
- method-pointer entry: `0x181B86908`
- direct PC native VA: `0x1803C2450`
- exact `.pdata` range: `0x1803C2450–0x1803C3085`
- exact native length: `3125` bytes
- unwind RVA: `0x181A93FCC`
- exact native SHA256: `984d165d8221b695bfe0783ed72aa2ca32112923a41d00223b32859d902137af`

The current damaged managed body is 4439 bytes and contains a large amount of Cpp2IL native/SIMD lifting debris and fake pointer/string scaffolding. This is not a one-opcode Vector3 repair candidate.

## Iterator object layout established by native accesses

In the PC body `r14` is the generated `<WarpText>d__7` instance and `rsi` becomes the captured parent `SkewTextExample` instance.

Generated iterator fields observed directly:

- `r14 + 0x10` = `<>1__state`
- `r14 + 0x18` = `<>2__current`
- `r14 + 0x20` = `<>4__this`
- `r14 + 0x28` = `<old_CurveScale>5__2`
- `r14 + 0x2C` = `<old_ShearValue>5__3`
- `r14 + 0x30` = `<old_curve>5__4`

Captured parent fields observed directly:

- `rsi + 0x20` = `m_TextComponent`
- `rsi + 0x28` = `VertexCurve`
- `rsi + 0x30` = `CurveScale`
- `rsi + 0x34` = `ShearAmount`

The native body explicitly stores iterator states `-1`, `1`, and `2` and clears/sets `<>2__current` around `yield return null` paths. A replacement must preserve generated-iterator state/yield behavior; replacing `MoveNext` with a simple synchronous helper is not equivalent.

## Native constants

Direct reads/references in the original PC body establish the numeric constants used by the algorithm:

- `0x1815A7C54` = `10.0f` (`CurveScale *= 10`)
- `0x1815A7AA0` = `0.01f` (shear scale)
- `0x1815A7A08` = `0.5f` (baseline midpoint)
- `0x1815A7A00` = `0.0001f` (`x1 = x0 + 0.0001f`)
- `0x1815A7E1C` = approximately `57.29578f` (radians-to-degrees multiplier)
- `0x1815A7E38` = `360.0f`

These constants independently match the expected TextMeshPro example algorithm; they are not inferred from managed decompiler output.

## Source-level semantics corroborated by the exact PC native body

The native control flow and data accesses correspond to the standard TextMeshPro `SkewTextExample.WarpText()` algorithm:

1. Set `VertexCurve.preWrapMode` and `postWrapMode` to Clamp.
2. Mark `m_TextComponent.havePropertiesChanged = true`.
3. Multiply `CurveScale` by `10`, cache old CurveScale / ShearAmount, and copy the animation curve.
4. Enter the iterator loop. If properties, scale, curve key 1 value, and shear are unchanged, set iterator current to null and yield.
5. Refresh cached values and call `ForceMeshUpdate()`.
6. Read `textInfo` and `characterCount`; zero characters continue the loop.
7. Read text bounds `min.x` / `max.x`.
8. Iterate characters and skip characters whose `TMP_CharacterInfo.isVisible` is false. **This visibility branch is correct for SkewText** and must not be confused with the unsupported visibility branch previously found in Gemini's TextLink ResetLink/SetLink patch.
9. Read `vertexIndex`, `materialReferenceIndex`, and the material's vertex array.
10. Compute the midpoint/baseline offset from vertices 0 and 2 plus the character baseline, then subtract it from all four character vertices.
11. Compute shear using `ShearAmount * 0.01f`; create top and bottom shear vectors from `topRight.y`, `baseLine`, and `bottomRight.y`; apply them to the four vertices.
12. Compute normalized x position `x0`, `x1=x0+0.0001f`, then `y0/y1 = VertexCurve.Evaluate(...) * CurveScale`.
13. Construct the horizontal vector and tangent; compute angle from `Acos(Dot(horizontal, tangent.normalized)) * 57.29578f`, then use `Cross(...).z` to choose `dot` or `360-dot`.
14. Construct `Matrix4x4.TRS(new Vector3(0,y0,0), Quaternion.Euler(0,0,angle), Vector3.one)`.
15. Run `MultiplyPoint3x4` on all four vertices, then add the midpoint/baseline offset back.
16. After the character loop, call `m_TextComponent.UpdateVertexData()`.
17. Set iterator current to null and yield; later MoveNext resumes the outer loop through the saved state.

## Independent public-source corroboration

A public Unity project containing the TextMeshPro 3.0.6 Examples & Extras source has the matching source at:

- repository: `gammawizard12345/Shinobi-Chogumelo`
- commit: `68fe89358ab19b78624e34eb8ed436311685ca95`
- path: `Shinobi Chogumel/Assets/TextMesh Pro/Examples & Extras/Scripts/SkewTextExample.cs`
- the same repository also contains `Library/PackageCache/com.unity.textmeshpro@3.0.6/PackageConversionData.json`

That source is corroborating evidence only. The GodsPVZ PC native body, locked hashes, exact method mapping, constants, iterator field accesses, and control flow remain the semantic authority.

## Recovery constraint

Do not patch this method by mass-replacing Vector3-related opcodes in the 4439-byte damaged CIL. The safe next stage is a target-specific reconstruction of MethodDef `0x06000776` that preserves:

- the existing generated iterator type and fields;
- exact `<>1__state` / `<>2__current` yield-resume behavior;
- null/array exception behavior from typed managed operations;
- TextMeshPro character visibility and mesh indexing behavior;
- all four vertex transformations and ordering;
- full non-target MethodDef semantic isolation;
- original assembly identity counts and MVID.

Only after a deterministic static candidate passes those gates should it enter another expensive Unity/IL2CPP iOS export.
