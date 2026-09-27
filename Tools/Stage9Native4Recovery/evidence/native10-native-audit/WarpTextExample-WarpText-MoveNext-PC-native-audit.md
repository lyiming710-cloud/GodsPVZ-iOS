# WarpTextExample/<WarpText>d__8::MoveNext — direct PC-native audit

Status: **native semantics established; recovery remains experimental; no production promotion**.

Authority is the original GodsPVZ 1.0.2 PC x86-64 IL2CPP release.

## Locked identity

- package SHA256: `2f08b4e2243e2bd296f3db694b4a2c9656252d76202c80167bdf3340b9f65b48`
- `GameAssembly.dll` SHA256: `9ebd7ca996a5b03fb4a766f7a2502b660d581ddbbb36af4d7bf2f06a211da39d`
- `global-metadata.dat` SHA256: `ad992341add498bd1018ac980f42171e0737b0793bf945c8714ad70a445b36b9`
- managed method: `System.Boolean TMPro.Examples.WarpTextExample/<WarpText>d__8::MoveNext()`
- MethodDef: `0x0600081C`, RID `2076`
- method-pointer entry: `0x181B86E38`
- native VA: `0x1803CD3F0`
- exact `.pdata` range: `0x1803CD3F0–0x1803CDEE6`
- exact native length: `2806` bytes
- unwind RVA: `0x1A94974`
- exact native SHA256: `55d77a157aa8dca15d1d8dd95267ba4f5b7a12ef4849d1cfa1ca97c8cab75daf`

## Exact managed metadata in the native9 candidate

The cheap Cecil inspection gate (`36309804413`) independently locks the iterator metadata used by recovery:

- parent TypeDef `TMPro.Examples.WarpTextExample`: `0x02000118`
- iterator TypeDef `<WarpText>d__8`: `0x02000119`
- `MoveNext`: `0x0600081C`
- `<>1__state`: `0x04000A64`
- `<>2__current`: `0x04000A65`
- `<>4__this`: `0x04000A66`
- `<old_CurveScale>5__2`: `0x04000A67`
- `<old_curve>5__3`: `0x04000A68`
- parent `m_TextComponent`: `0x04000A5F`
- parent `VertexCurve`: `0x04000A60`
- parent `AngleMultiplier`: `0x04000A61`
- parent `SpeedMultiplier`: `0x04000A62`
- parent `CurveScale`: `0x04000A63`

The same inspection proves the exact existing `WaitForSeconds::.ctor(float)` MemberRef is `0x0A000341`; recovery must reuse it rather than synthesize a signature.

## Direct native semantics

The native iterator has compiler states `0`, `1`, and `2`; running state is `-1`. Invalid/terminal state returns `false`.

State 0 initializes:

1. `VertexCurve.preWrapMode = Clamp`
2. `VertexCurve.postWrapMode = Clamp`
3. `m_TextComponent.havePropertiesChanged = true`
4. `CurveScale *= 10.0f`
5. cache `old_CurveScale`
6. copy `VertexCurve` into `old_curve`

The loop skips mesh work only when all of these remain unchanged:

- `!m_TextComponent.havePropertiesChanged`
- `old_CurveScale == CurveScale`
- `old_curve.keys[1].value == VertexCurve.keys[1].value`

That fast path stores `current = null`, state `1`, and returns `true`.

The processing path refreshes the cached scale/curve, calls `ForceMeshUpdate`, obtains `textInfo` and `characterCount`, and immediately loops again when character count is zero. Otherwise it reads text bounds and iterates visible characters. For each visible character it:

- resolves `vertexIndex`, `materialReferenceIndex`, and that material's vertex array;
- computes the baseline midpoint from vertices 0/2 and character baseline;
- subtracts that offset from all four vertices;
- computes normalized curve position `x0`, `x1 = x0 + 0.0001f`, `y0`, and `y1`;
- builds the horizontal/tangent vectors;
- computes `acos(dot) * 57.2957795f`, uses cross-product Z to select `dot` versus `360.0f - dot`;
- creates `Matrix4x4.TRS(new Vector3(0,y0,0), Quaternion.Euler(0,0,angle), Vector3.one)`;
- transforms all four vertices and restores the baseline offset.

After the character loop it calls parameterless `TMP_Text.UpdateVertexData()`, allocates `new WaitForSeconds(0.025f)`, stores it as `current`, sets state `2`, and returns `true`.

The native constants are directly present in the locked PC image:

- `10.0f`
- `0.0001f`
- `57.2957795f`
- `360.0f`
- `0.025f` at PC VA `0x1815A8040`

## Public structural corroboration

TextMesh Pro 3.0.6 example source in `gammawizard12345/Shinobi-Chogumelo` commit `68fe89358ab19b78624e34eb8ed436311685ca95`, path `Assets/TextMesh Pro/Examples & Extras/Scripts/WarpTextExample.cs`, has the same iterator structure and mesh algorithm. It is corroboration only; the PC native body above remains authoritative.

## Current iOS blocker

The direct post-Linker IL2CPP canary after native9 reports this exact method as one of two remaining blockers. The current damaged managed body is a large Cpp2IL-generated body and is not suitable for an opcode-level patch. Recovery must replace only MethodDef `0x0600081C`, prove deterministic materialization and non-target isolation, then prove this method disappears from the direct IL2CPP error set before any full Unity rebuild.
