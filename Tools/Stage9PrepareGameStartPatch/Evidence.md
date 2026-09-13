# PrepareUIController.GameStart native recovery evidence

Input candidate: `1c62f969409ed16c9d7c7855f7b52e6b593701524e2b2d75ec0affbc3d05b209`.

Corrected exact R3 lifecycle run `34774556853` reaches:

`STAGE9_STRICT_BOARD_PRESTART scene=Board gos=927 missing=0 boardStart=0 board=0 boardManager=1 prepare=1 seedChooser=1`

then fails exactly at `PrepareUIController.GameStart()` with:

`FieldAccessException: Field BoardManager:<Instance>k__BackingField is inaccessible from method PrepareUIController:GameStart()`.

Managed target:
- token `0x0600067E`
- code size `255`
- 11 locals
- one illegal cross-type `ldsfld BoardManager::<Instance>k__BackingField`
- one existing `BoardManager.LoadBoard()` call

PC x86-64 IL2CPP authority:
- `PrepareUIController.GameStart`: VA `0x1803A6D10`
- `BoardManager.get_Instance`: token `0x06000169`, VA `0x1803100F0`
- `BoardManager.LoadBoard`: VA `0x18030F830`
- GameStart reads the BoardManager singleton static slot, checks challenge/level, optionally destroys the preview, calls LoadBoard, then deactivates its own GameObject.

Recovery:
- replace exactly the illegal cross-type backing-field read with `call BoardManager.get_Instance()`
- preserve all other GameStart instructions and behavior
- change no fields
- introduce no `System.Private.CoreLib` reference
