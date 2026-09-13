# ResourceManager.Load_card_Choose_Sprites recovery evidence

Target: `ResourceManager.Load_card_Choose_Sprites()` token `0x06000222`.

PC 1.0.2 authority: `GameAssembly.dll` SHA-256 `9ebd7ca996a5b03fb4a766f7a2502b660d581ddbbb36af4d7bf2f06a211da39d`, native VA `0x0000000180338130`, function end `0x0000000180338500`.

Native semantics:
- base path: `Path.Combine("sprites", "Card_Choose")`
- loop `i=0..6`: load `data{i}`, `lv{i}`, `innerlining{i}`, `outerlining{i}` into the four corresponding 7-element Sprite arrays
- loop `i=0..4`: load `orderArabesques{i}` into the 5-element Sprite array
- 33 `InternalResourceLoader.Load<Sprite>` calls total
- 34 `Path.Combine(string,string)` calls total (one base path + one per sprite)
- 33 `Int32.ToString`, 33 `String.Concat(string,string)`, 33 array stores

Runtime causal evidence: exact R3 strict run `34767194659`, candidate `498b34ca2db493d42676a3283edac8126180fd624d6664b3c39e855abfa905ba`, first direct blocker inside the restored `ResourceManager.LoadSprites()` call chain: `InvalidProgramException: Invalid IL code in ResourceManager:Load_card_Choose_Sprites (): IL_018d: stloc 2`.

The prepatch managed body is 636 bytes with 37 locals and ILSpy type-stack diagnostics. Recovery is restricted to this one MethodDef; all other method semantics and all field metadata must remain unchanged.
