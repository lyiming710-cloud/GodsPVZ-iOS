# ResourceManager.Load_card_Choose_DevicePortraits PC-native recovery evidence

Input candidate: `d7076ccb929b52509820e3ca1e58982242d50c8e7dfa79efa88308c623a40f1d`.

Exact R3 strict runtime `34771137512` proves the previous PlantPortraits and `System.Private.CoreLib` blockers are gone. The next direct `ResourceManager.LoadSprites()` failure is:

`FieldAccessException: Field System.Collections.Generic.List`1:_size is inaccessible from method ResourceManager:Load_card_Choose_DevicePortraits()`

with call chain:

`ResourceManager.Load_card_Choose_DevicePortraits() -> ResourceManager.LoadSprites() -> ResourceManager.Start() -> GameStart.Start()`.

PC 1.0.2 authority:
- MethodDef token: `0x06000224`
- native VA: `0x0000000180337650`
- function end / next method boundary: `0x0000000180337C10`
- method map identifies `ResourceManager.Load_card_Choose_DevicePortraits()` at that address.

PC native plus the surviving damaged managed structure establish the source semantics:
1. If `card_Choose_DevicePortraits` already contains entries, return it.
2. Build base path `sprites/Portrait/Device`.
3. Iterate `Enum.GetValues(typeof(DeviceType))` using the array enumerator.
4. For each `DeviceType`, get its enum name.
5. Load `InternalResourceLoader.Load<Sprite>(Path.Combine(basePath, enumName))`.
6. Add the loaded sprite to `card_Choose_DevicePortraits` using normal `List<Sprite>.Add` semantics.
7. Evaluate Unity object truthiness via `UnityEngine.Object.op_Implicit`.
8. If the sprite is missing, preserve the original diagnostic: `Debug.Log("不存在路径为" + Path.Combine(basePath, enumName) + "的文件")`.
9. Dispose the foreach enumerator in `finally` and return the list.

The corrupt recovered body is 1567 bytes with 71 locals and directly manipulates List internals `_size`, `_version`, and `_items`, which is not legal managed source behavior and is the exact runtime failure.

Recovery constraints:
- rewrite only MethodDef `0x06000224`;
- preserve all other 2316 MethodDefs exactly under semantic canonicalization;
- preserve all 2802 FieldDefs exactly;
- preserve prior `Start -> LoadSprites`, `Load_card_Choose_Sprites`, PlantPortraits, and 17 `Card_Choose [SerializeField]` recoveries;
- bind `System.Array` and other BCL types to the target DLL's existing `mscorlib` references; never import host `.NET 10 System.Private.CoreLib`;
- independent fixed ILSpy must show the enum/load/add/log/finally structure and zero target-method invalid-IL diagnostics.
