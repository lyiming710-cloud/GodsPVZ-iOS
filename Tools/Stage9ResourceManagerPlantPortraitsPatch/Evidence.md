# ResourceManager.Load_card_Choose_PlantPortraits recovery evidence

Target: `ResourceManager.Load_card_Choose_PlantPortraits()` token `0x06000223`.

PC 1.0.2 authority: `GameAssembly.dll` SHA-256 `9ebd7ca996a5b03fb4a766f7a2502b660d581ddbbb36af4d7bf2f06a211da39d`, native VA `0x0000000180337C10`, function end `0x0000000180338130`.

Native/source semantics recovered from PC authority plus the exact damaged managed body:
- if `card_Choose_PlantPortraits != null` and its count is greater than zero, return the existing list;
- build base path `Path.Combine("sprites", "Portrait", "Plant")`;
- enumerate `Enum.GetValues(typeof(PlantType))` with an enumerator;
- for each `PlantType`, resolve `Enum.GetName(typeof(PlantType), value)`;
- load `InternalResourceLoader.Load<Sprite>(Path.Combine(basePath, name))`;
- append with the public `List<Sprite>.Add` semantic rather than illegal private `_version/_size/_items` field accesses introduced by the damaged Cpp2IL reconstruction;
- preserve the native Unity Object implicit check after each load;
- dispose the enumerator through the normal foreach `finally` path;
- return `card_Choose_PlantPortraits`.

Runtime causal evidence: exact R3 strict run `34768204821`, candidate `b908f70d4520bea6369d3e78204d6ff017da8a5525ebac9f82e3603badc3add6`, direct blocker in the restored `ResourceManager.LoadSprites()` chain:
`FieldAccessException: Field System.Collections.Generic.List\`1:_size is inaccessible from method ResourceManager:Load_card_Choose_PlantPortraits()`.
Stack: `Load_card_Choose_PlantPortraits -> ResourceManager.LoadSprites [0x00005] -> ResourceManager.Start [0x00078] -> GameStart.Start`.

Prepatch managed fingerprint: code size 1484 bytes, 69 locals, direct references to private List internals `_size`, `_version`, `_items`, and multiple ILSpy stack diagnostics.

Recovery is restricted to this one MethodDef. All other method semantics and all field metadata must remain unchanged.
