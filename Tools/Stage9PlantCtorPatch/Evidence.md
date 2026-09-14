# Stage9.1 Plant::.ctor PC-native recovery evidence

## Locked authority

- Original PC archive SHA-256: `2f08b4e2243e2bd296f3db694b4a2c9656252d76202c80167bdf3340b9f65b48`
- Original PC `GameAssembly.dll` SHA-256: `9ebd7ca996a5b03fb4a766f7a2502b660d581ddbbb36af4d7bf2f06a211da39d`
- Original PC `global-metadata.dat` SHA-256: `ad992341add498bd1018ac980f42171e0737b0793bf945c8714ad70a445b36b9`
- Assembly-CSharp CodeGenModule method-pointer table VA: `0x181B82D60`
- Managed target: `Plant::.ctor()` token `0x060003BA`, RID `954`
- Method-pointer-table entry for RID 954: `0x18035CAD0`
- PE `.pdata` runtime-function range: `0x18035CAD0-0x18035CEF0`
- Exact 0x420-byte native slice SHA-256: `4aa8c226c4128cb04ee0851a5802a5fecf013ad1be9a9e200586c32d363ebb06`

## Runtime causal evidence

Batch1 corrected-lifecycle run `34791051801` successfully reached an active Board (`board=1 activeBoard=1`). The first exception after `STAGE9_STRICT_GAMESTART_INVOKE ok=1` is:

`FieldAccessException: Field UnityEngine.Vector3:zeroVector is inaccessible from method Plant:.ctor()`

Call chain: `PlantManager.Start -> ResourceManager.Load_plantPrefabs -> InternalResourceLoader.Load -> Resources.Load`.

## Malformed managed fingerprint

On Batch1 candidate SHA `2bcf1643457eecddeff3eaed9668c2117c11fdd5829b264a29e0f30cc86fe20c`:

- target token `0x060003BA`, RID `954`
- managed body size `580` bytes
- `20` locals
- direct inaccessible `UnityEngine.Vector3::zeroVector` reads at IL `0x01E9` and `0x0218`
- malformed Cpp2IL lowering also contains synthetic unmanaged-memory-load artifacts
- malformed IL encodes native two-byte boolean initialization as `ldc.i4 257; stfld bool Plant::attackable`, thereby losing the adjacent `blockable=true` write.

## PC-native constructor semantics

The PC body is straight-line initialization plus allocation and the base `MonoBehaviour` constructor. Relevant stores/calls, in native order:

1. `plantName = String.Empty` (`this+0x50`)
2. `characteristicText = String.Empty` (`+0x58`)
3. `talentNames = new string[3]` (`+0x60`)
4. `talents = new string[3]` (`+0x68`)
5. `level = 1` (`+0x8C`)
6. `healthPoint = 300.0f` (`+0x94`)
7. `maxHealthPoint = 300.0f` (`+0x98`)
8. `attackPoint = 20.0f` (`+0x9C`)
9. native `mov word ptr [this+0xB5], 0x0101` sets both `attackable=true` and adjacent `blockable=true`
10. `active = true` (`+0xB8`)
11. `camp = 1` / `Camp.plant` (`+0xBC`)
12. `updateRate = 1.0f` (`+0xC0`)
13. allocate/construct `Skill`, store `skill` (`+0xE0`)
14. allocate/construct `ElementManager`, store `elementManager` (`+0xF8`)
15. copy zero-valued vector data into `dithering` (`Vector2`, `+0x104`)
16. copy zero-valued vector data into `dithering_anim` (`Vector3`, `+0x10C`)
17. allocate `List<GameObject>`, store `animationSprites` (`+0x148`)
18. allocate four `Sprite[8]` arrays, store `UISprites1..4` (`+0x180,+0x188,+0x190,+0x198`)
19. allocate `List<ElementUIController>`, store `elementUIControllers` (`+0x1B8`)
20. allocate `List<GameObject>`, store `UI_Characteristic` (`+0x1C8`)
21. `produce_Brightness = 1.0f` (`+0x1F8`)
22. `flash_Brightness = 1.0f` (`+0x1FC`)
23. allocate `List<int>`, store `parameter_ints` (`+0x218`)
24. allocate/construct `BuffManager`, store `buffManager` (`+0x220`)
25. tail-call the `MonoBehaviour` base constructor.

The final repaired managed body lowers the two native zero-value copies through existing public `Vector2(float,float)` and `Vector3(float,float,float)` constructor MemberRefs with all-zero components. This produces the same field values as the PC native stores, avoids the illegal private `Vector3.zeroVector` access, introduces no new assembly reference or MemberRef, and is fully understood by the fixed ILSpy validator. No null guards or fallback gameplay behavior are introduced.
