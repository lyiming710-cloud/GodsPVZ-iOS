# Stage9.1 SeedChooserScreen.SetCardChooseList native recovery evidence

## Locked input

- Input candidate SHA-256: `3b1bfe50761537734063070618ea33e8b0d36d88f3d161861733449d34490243`
- Expected MethodDef count: `2317`
- Target only: `SeedChooserScreen.SetCardChooseList()`
- MethodDef token: `0x060006BD`

## PC native authority

Original PC 1.0.2 authority was re-verified before reconstruction:

- Original PC ZIP SHA-256: `2f08b4e2243e2bd296f3db694b4a2c9656252d76202c80167bdf3340b9f65b48`
- `GameAssembly.dll` SHA-256: `9ebd7ca996a5b03fb4a766f7a2502b660d581ddbbb36af4d7bf2f06a211da39d`
- `global-metadata.dat` SHA-256: `ad992341add498bd1018ac980f42171e0737b0793bf945c8714ad70a445b36b9`
- Method map entry: native index `1724`, token `0x060006BD`, VA `0x00000001803AB510`, `SeedChooserScreen SetCardChooseList 0`
- Function body/tails end immediately before the next mapped function at `0x00000001803ABA00`.

The PC native control flow establishes the following behavior:

1. Iterate existing `card_ChooseList`, destroy each card's `gameObject`, then clear the list.
2. Resolve `GlobalStaticVars.gLawnApp -> savesManager -> playerSave -> plantSaves`.
3. If `playerSave` is null, log `存档不存在：创建选卡列表时` and follow the generated null-failure path.
4. Iterate `plantSaves`. For each `PlantSave`:
   - instantiate `card_Choose_Prefab` as `Card_Choose`;
   - append it to `card_ChooseList`;
   - parent its transform under `card_ChooseList_Gameobject.transform` with `worldPositionStays=false`;
   - set local scale to `(0.75, 0.75, 1.0)`;
   - assign `card.seedChooserScreen = this`;
   - call `card.Initialize(plantSave)`.
5. Log `执行完成` and return.

## Managed corruption being removed

The 3b1b managed body is 1351 bytes and is not trustworthy as executable IL. It contains an invalid comparison ending at the runtime blocker `IL_002f: ceq`, malformed `List<object>.Enumerator` attribution, and direct accesses to private `List<T>` internals (`_version`, `_items`, `_size`, `AddWithResize`).

## Reconstruction rule

Only MethodDef `0x060006BD` is rewritten. Native foreach loops are lowered to index loops using the public `List<T>.Count` and `List<T>.get_Item(int)` APIs. This is behavior-preserving here because neither native loop mutates the collection being enumerated; `List<T>.Enumerator.Dispose()` has no externally visible state change.

All closed-generic `List<T>` MemberRefs retain the declaring generic type parameter `!0` in signatures. In particular, `List<Card_Choose>.Add` is encoded as `Add(!0)`, and both `get_Item` MemberRefs return `!0`, avoiding the concrete-parameter MemberRef error previously proven to cause `MissingMethodException`.

The static gate must prove:

- exact input SHA;
- target token/PC VA;
- no private `List<T>` field access or malformed `List<object>` refs in the repaired method;
- public Count/get_Item/Clear/Add usage with correct `!0` metadata;
- presence of Instantiate/Destroy/SetParent/localScale/Initialize and both native log strings;
- reopen validation after writing;
- semantic isolation: `2316` untouched methods, `1` changed method.
