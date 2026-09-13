# Stage9 Card_Choose SerializeField recovery evidence

## Locked inputs

- Managed candidate before this metadata repair:
  - `GodsPVZRuntime1-card-choose-initialize.dll`
  - SHA-256: `d255aec78c28b6269934c73f8581ce16cdc4209533172baa965977f0d63b7d89`
  - source artifact: `Stage9.1-4A2E-CARD-CHOOSE-INITIALIZE`, run `34760293134`
- Exact R3 Unity project archive:
  - size: `786765618` bytes
  - SHA-256: `d4264f12a00e86b6149d20ecf04caa9761af510aece6107bcceb155ea4ab0bbc`
- PC 1.0.2 authority:
  - `GodsPVZ_1.0.2.zip`: `2f08b4e2243e2bd296f3db694b4a2c9656252d76202c80167bdf3340b9f65b48`
  - `GameAssembly.dll`: `9ebd7ca996a5b03fb4a766f7a2502b660d581ddbbb36af4d7bf2f06a211da39d`
  - `global-metadata.dat`: `ad992341add498bd1018ac980f42171e0737b0793bf945c8714ad70a445b36b9`

## Runtime reason for this repair

The exact R3 strict Board rerun with the green `d255...` candidate removed the prior `InvalidProgramException`. The next first-causal failure is a `NullReferenceException` inside `Card_Choose.Initialize(PlantSave)` at the first `ordersImage[i].gameObject` dereference. `SeedChooserScreen.SetCardChooseList()` is only the caller.

The repaired managed method now matches PC-native semantics at `Card_Choose.Initialize`, token `0x060002F2`, PC VA `0x0000000180343BE0`. The native implementation dereferences the UI references and does not contain a null-tolerant fallback for missing serialized references. Adding null guards would therefore weaken fidelity rather than recover the original object graph.

## Exact R3 prefab authority

Path inside the exact R3 archive:

`UnityProject-AssetRipper-2.0.0/ExportedProject/Assets/Resources/prefabs/card_choose/Card_Choose.prefab`

`Card_Choose` MonoBehaviour fileID: `114320256170494724`.

The prefab contains serialized keys for all 17 fields that are private in the managed metadata:

| Field | Managed type | Exact R3 serialized value / evidence |
|---|---|---|
| `lockLogo` | `UnityEngine.UI.Image` | fileID `114292246286975720` |
| `chain` | `UnityEngine.UI.Image` | fileID `114279359913592680` |
| `chosen` | `UnityEngine.UI.Image` | fileID `114998874197659892` |
| `Lv` | `UnityEngine.UI.Image` | fileID `114703931505913403` |
| `background` | `UnityEngine.UI.Image` | fileID `114518489005915545` |
| `foreground` | `UnityEngine.UI.Image` | fileID `114693329660128220` |
| `orderArabesques` | `UnityEngine.UI.Image` | fileID `114477046003934300` |
| `ordersImage` | `UnityEngine.UI.Image[]` | 4 refs: `114855482821919830`, `114880619614767448`, `114533803739292891`, `114007513189690300` |
| `starsImage` | `UnityEngine.UI.Image[]` | 6 refs: `114574304967836189`, `114799762105739359`, `114552309709192268`, `114651560128206891`, `114431740505123533`, `114699617135816954` |
| `plantPortrait` | `UnityEngine.UI.Image` | fileID `114967328832014496` |
| `hormonLogo` | `UnityEngine.UI.Image` | serialized key present, fileID `0` |
| `dataBackground` | `UnityEngine.UI.Image` | fileID `114924050743934937` |
| `skillLogo` | `UnityEngine.UI.Image` | fileID `114094320201520353` |
| `cliqueLogo` | `UnityEngine.UI.Image` | fileID `114719342369648492` |
| `sunPriceText` | `TMPro.TextMeshProUGUI` | fileID `114704330047573733` |
| `levelText` | `TMPro.TextMeshProUGUI` | fileID `114194081240383615` |
| `serialNumBeChosenText` | `TMPro.TextMeshProUGUI` | fileID `114190181876552715` |

The non-null Image refs above point to Unity UI Image MonoBehaviours using script GUID `fe87c0e1cc204ed48ad3b37840f39efc`. The three TMP refs use script GUID `f4688fdb7df04437aeb418b961361dc5`.

## Why `[SerializeField]` is the targeted repair

In the `d255...` managed candidate, the 17 fields above are still `private` and currently have no `UnityEngine.SerializeField` custom attribute. Unity does not restore private inspector references from prefab YAML unless those fields are serialized. The prefab proves that these exact private fields were part of the original serialized object graph.

Therefore this patch only adds one `UnityEngine.SerializeField` custom attribute to each of those 17 fields. It must not:

- change any method body;
- change any field type;
- change field visibility or static/instance status;
- add null guards to gameplay code;
- touch public fields;
- touch any other type or field metadata.

The gate requires all 2317 MethodDefs to remain semantically identical and exactly 17 FieldDefs to change, all inside `Card_Choose` and only by the expected custom attribute.
