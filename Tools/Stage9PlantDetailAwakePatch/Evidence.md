# Stage9.1 PlantDetail.Awake native evidence

This is DEVELOPMENT / integration evidence only. It does not advance the formal cumulative recovery chain beyond HF55.

## Locked source inputs

- PC release ZIP SHA-256: `2f08b4e2243e2bd296f3db694b4a2c9656252d76202c80167bdf3340b9f65b48`
- PC `GameAssembly.dll` SHA-256: `9ebd7ca996a5b03fb4a766f7a2502b660d581ddbbb36af4d7bf2f06a211da39d`
- PC `global-metadata.dat` SHA-256: `ad992341add498bd1018ac980f42171e0737b0793bf945c8714ad70a445b36b9`
- Android APK SHA-256: `428e0ba2e46645a905fb9fbb00cfde42406727a3ddfce9a7df1e0875889a739f`
- Android arm64 `libil2cpp.so` SHA-256: `cfa13d53d7c3e218221a90c5fb615a012339393774af3160ff1c17f3826c61a6`
- Android `global-metadata.dat` SHA-256: `e7a4412e3af25da3ba2c806d3c691ec68d00c1e7915d8fb6843c3a82422f5005`

## Original MethodDef/native mapping

Using original metadata MethodDef RID-1 -> `Assembly-CSharp.dll` CodeGenModule methodPointers:

- declaring type: `SeedChooserScreen/PlantDetail`
- method: `Awake(Card_Choose)`
- original RID: `1730`
- original token: `0x060006C2`
- PC x86-64 method pointer: `0x1803A1C60`
- next PC method (`LoadPlantData`) begins at `0x1803A2610`
- Android arm64 cross-check method pointer: `0x13E0A88`

## Native-backed behavior recovered from both platforms

PC x86-64 is the primary authority. Android arm64 was used as the second-source control-flow and constant cross-check.

1. Require `dataUIControllers`; iterate it and set each controller's `gameObject` inactive.
2. Require `plantName`; set text to `"NaN"` with the boolean argument `true`.
3. Require `infoButtons`; iterate it and set each button's `gameObject` inactive.
4. Store the incoming `card_Choose` in `this.card_Choose`.
5. Test the incoming `card_Choose` with Unity `Object` equality against null.
6. Non-null card branch:
   - require and activate `button_Almanac`;
   - call `LoadPlantData()`;
   - iterate `dataUIControllers` and activate their game objects;
   - iterate `infoButtons` and activate their game objects;
   - return.
7. Null-card branch:
   - require and deactivate `button_Almanac`;
   - require/deactivate `skillText`, `skillBank`, `characteristicText`, and `talentText`;
   - perform exactly three iterations over skill-button indices `0..2`;
   - for each index, load child `4`, get `Image`, load `ResourceManager.LoadSkillLogo(0, "Lock")`, and assign the sprite;
   - reload the same skill button, get `Button`, and set `enabled=false`;
   - reload it again, child `2` game object -> `SetActive(true)`;
   - reload it again, child `5` game object -> `SetActive(false)`;
   - return after three iterations.

The PC native body performs raw pointer null checks for the managed/UI references and funnels failures into the IL2CPP null-reference helper. The reconstructed CIL preserves this with explicit `NullReferenceException` guards. The `card_Choose` branch deliberately preserves Unity `Object.op_Equality(card_Choose, null)` semantics rather than replacing it with a raw reference test.

The original Cpp2IL CIL for this method is not suitable for local repairs: it contains reference-vs-`int32 0` comparisons, cross-type local stores, malformed enumerator reconstruction, and synthetic decompiler-helper artifacts. The Stage9 patch therefore replaces only this one MethodDef body with a native-backed behavior-equivalent CIL body; all other MethodDefs are semantic-isolation checked.
