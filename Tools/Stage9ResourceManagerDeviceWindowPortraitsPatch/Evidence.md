# ResourceManager.Load_devicePortraits_Window recovery evidence

Input candidate SHA-256: `a7448cb9fc2c746c0fb57a535a7bda0516071e0afcf4f322388b5a937bbd8c9d`.

Exact R3 strict runtime `34772270056` shows the prior `Load_card_Choose_DevicePortraits()` blocker is gone. `ResourceManager.LoadSprites()` now proceeds to `ResourceManager.Load_devicePortraits_Window()` and fails directly with `FieldAccessException` on `System.Collections.Generic.List<T>._size`.

PC 1.0.2 authority:
- token `0x06000227`
- native VA `0x180338B00`
- next mapped method VA / function end `0x180338FF0`

Native semantics recovered from the PC function:
1. if `devicePortraits_Window` already has entries, return it;
2. build `sprites/Portrait/Device/Window`;
3. iterate `Enum.GetValues(typeof(DeviceType))`;
4. derive each enum name and load `InternalResourceLoader.Load<Sprite>(Path.Combine(path, name))`;
5. when the sprite exists, preserve sparse enum indexing: while `Count <= (int)deviceType`, append `null`; then assign `devicePortraits_Window[(int)deviceType] = sprite`;
6. dispose the enumerator in the foreach cleanup;
7. return `devicePortraits_Window`.

The existing managed body is damaged Cpp2IL output (`1168` bytes, `56` locals), contains two direct accesses to `List<Sprite>._size`, and numerous invalid object/native-int artifacts. The recovery changes only MethodDef token `0x06000227`, preserves all fields and earlier recoveries, and must not introduce `System.Private.CoreLib`.
