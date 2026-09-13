# Stage9 ResourceManager.Start native-backed recovery evidence

## Locked managed input

- `GodsPVZRuntime1-card-choose-serializefield.dll`
- SHA-256: `9a01f9e44a056373ab230d121a8838e472cdd11a7b0d0d8ae662bb4ddb2148df`
- source gate: run `34763592918`, artifact `Stage9.1-D255-CARD-CHOOSE-SERIALIZEFIELD`

## Runtime causal evidence

Exact R3 strict Board run `34763654549` advanced past the former `ordersImage[i].gameObject` NullReferenceException after restoring the 17 `Card_Choose` serialized private fields. It then failed at:

`ArgumentOutOfRangeException -> Card_Choose.Initialize(PlantSave) [0x00129] -> SeedChooserScreen.SetCardChooseList() [0x00150] -> SeedChooserScreen.Ininitialize() -> BoardStart.Awake()`

Managed IL `0x00129..0x0013a` resolves to:

`ResourceManager.cliqueLogos[(int)plantSave.clique]`

The test-created first plant has `clique = 1`. `ResourceManager.cliqueLogos` is initialized as an empty List by the static constructor and must be populated by the normal sprite-loading path.

## PC 1.0.2 native authority

Locked PC hashes:

- `GodsPVZ_1.0.2.zip`: `2f08b4e2243e2bd296f3db694b4a2c9656252d76202c80167bdf3340b9f65b48`
- `GameAssembly.dll`: `9ebd7ca996a5b03fb4a766f7a2502b660d581ddbbb36af4d7bf2f06a211da39d`
- `global-metadata.dat`: `ad992341add498bd1018ac980f42171e0737b0793bf945c8714ad70a445b36b9`

Original metadata/native mapping:

- `ResourceManager.Start()` token `0x06000216`, native index 533, VA `0x000000018033BF30`
- `ResourceManager.LoadAudioClips()` token `0x0600021B`, VA `0x0000000180330340`
- `ResourceManager.LoadSprites()` token `0x0600021C`, VA `0x0000000180333DB0`

PC native `ResourceManager.Start()` contains the direct call sequence:

- `0x18033C4B4: call 0x180330340` -> `LoadAudioClips()`
- `0x18033C4BB: call 0x180333DB0` -> `LoadSprites()`
- then normal prefab loading begins.

The current managed `ResourceManager.Start()` has exactly one `LoadAudioClips()` call followed immediately by `ldstr "prefabs/Card/CardTemplate"`; it has zero calls to `LoadSprites()`.

## Repair scope

Restore exactly the missing `call ResourceManager.LoadSprites()` immediately after the existing `LoadAudioClips()` call and before prefab loading.

The gate must require:

- input SHA locked to `9a01...`;
- target token exactly `0x06000216`;
- all other 2316 MethodDefs semantically unchanged;
- exactly one MethodDef changed;
- every FieldDef metadata entry unchanged;
- all 17 previously recovered `Card_Choose` SerializeField attributes preserved;
- independent fixed ILSpy verification of `LoadAudioClips(); LoadSprites();` ordering.

Do not repair `LoadSprites()` in this change. If invoking the original reconstructed `LoadSprites()` exposes a new first-causal blocker, that blocker is handled separately after the same strict R3 runtime gate.
