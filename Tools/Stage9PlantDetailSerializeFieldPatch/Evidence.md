# Stage9 PlantDetail `button_Almanac` serialization metadata repair

## Scope

Input candidate is exactly:

`0112cbc9df8c729e4b96d72af7dd3399706accf5c26196177b1b8d47f7e53ef9`

Only `SeedChooserScreen/PlantDetail::button_Almanac` field metadata may change. No method body, field type, field visibility, or gameplay branch is modified.

## Runtime evidence

Strict Board runtime run `34747508962` reached:

- `STAGE9_STRICT_CREATE_INVOKE ok=1`
- `STAGE9_STRICT_SAVE_READY player=1 playerName=Stage9Test saveList=1 defaultName=Stage9Test`
- `STAGE9_STRICT_BOARD scene=Board gos=229 missing=0 boardStart=1 board=0`

First causal Board exception:

`NullReferenceException` in `SeedChooserScreen+PlantDetail.Awake(Card_Choose)` at reconstructed IL offset approximately `0x00299`, called by `SeedChooserScreen.Ininitialize()` from `BoardStart.Awake()`.

## PC native authority

MethodDef `0x060006C2`, `SeedChooserScreen/PlantDetail::Awake(Card_Choose)`, maps to PC x86-64 `0x00000001803A1C60` in:

- `GameAssembly.dll` SHA-256 `9ebd7ca996a5b03fb4a766f7a2502b660d581ddbbb36af4d7bf2f06a211da39d`
- `global-metadata.dat` SHA-256 `ad992341add498bd1018ac980f42171e0737b0793bf945c8714ad70a445b36b9`

Native control flow requires `this+0x78` (`button_Almanac`) to be non-null in both the `card_Choose != null` and `card_Choose == null` branches. Therefore adding a null guard or skipping Almanac behavior would diverge from original semantics and is prohibited.

## Serialized asset authority

Exact R3 archive:

- size `786765618`
- SHA-256 `d4264f12a00e86b6149d20ecf04caa9761af510aece6107bcceb155ea4ab0bbc`

In exact R3 file:

`Assets/Resources/prefabs/ui/SeedChooserScreen.prefab`

MonoBehaviour fileID `114998874197659892`, using script GUID `60b84a767c46ced9aacc93f38da4d2f3`, serializes nested `plantDetail` with:

`button_Almanac: {fileID: 1354824845356902}`

The referenced object exists in the same prefab as GameObject fileID `1354824845356902`, name `Almanac`.

`PlantDetail` is marked serializable, while both the R3 dummy managed DLL and the current `0112` runtime DLL expose the field only as:

`private UnityEngine.GameObject button_Almanac`

with no `UnityEngine.SerializeField` custom attribute. Other fields in the same managed assembly retain valid `UnityEngine.SerializeField` metadata, proving the required attribute constructor is available.

The asset therefore proves the original Unity type tree treated this private field as serialized, while the reconstructed managed metadata lost that eligibility. On Unity re-import, the prefab value cannot be restored to the plain private field, producing the observed null.

## Repair

Add exactly one custom attribute to exactly one field:

`[UnityEngine.SerializeField] private UnityEngine.GameObject button_Almanac;`

No method is altered. The static gate must prove:

1. exact `0112` input SHA;
2. target field remains private `UnityEngine.GameObject` under a serializable `PlantDetail` type;
3. exactly one `UnityEngine.SerializeField` attribute after reopen;
4. all 2317 MethodDefs are semantically unchanged;
5. exactly one FieldDef metadata record changes;
6. fixed ILSpy independently renders `[SerializeField]` on `button_Almanac`.

Runtime success is not assumed by this evidence. The repaired candidate must still pass the same exact-R3 strict Board gate.
