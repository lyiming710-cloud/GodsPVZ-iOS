# HF2 native evidence: Plant.Start / Plant.Update

Source of truth: GodsPVZ 1.0.2 PC `GameAssembly.dll` x86-64 + original PC `global-metadata.dat` (metadata v31). Native addresses below are resolved against the original `Assembly-CSharp.dll` MethodDef tokens using `Tools/MethodIndexDump/native_map.py`; Cpp2IL-rewritten RIDs are deliberately not used for address attribution.

## Method identity

| Method | Original MethodDef | Native address |
|---|---:|---:|
| `Plant.Start` | RID 841 / `0x06000349` | `0x18035A490` |
| `Plant.Start_Characteristic` | RID 842 / `0x0600034A` | `0x18035A160` |
| `Plant.LoopAddAnimation` | RID 843 / `0x0600034B` | `0x180351AC0` |
| `Plant.Update` | RID 844 / `0x0600034C` | `0x18035C4D0` |

The PC `Assembly-CSharp.dll` CodeGenModule has `methodPointerCount = 0x90C = 2316` and method-pointer table VA `0x181B82D60`. This matches the 2316 original MethodDefs for the image. The native pointer index is original MethodDef RID - 1.

## Plant.Start @ 0x18035A490

### Field offsets observed in native code

| Offset | Field / relationship | Evidence |
|---:|---|---|
| `Plant+0xF8` | `elementManager` | dereferenced before `CreateNewElements<Plant>` and `elements` |
| `Plant+0x1B8` | `elementUIControllers` | inner List enumerator source |
| `Plant+0x1E0` | `plantManager` | assigned from `board.plantManager` |
| `Plant+0x1E8` | `board` | Unity Object truth test and manager source |
| `Plant+0x1F8` | `produce_Brightness` | initialized to `1.0f` at `0x18035A547` |
| `Plant+0x1FC` | `flash_Brightness` | initialized to `1.0f` at `0x18035A551` |
| `Board+0xE8` | `plantManager` | source for `Plant.plantManager` |
| `ElementManager+0x20` | `elements` | outer List enumerator source |
| `Element+0x20` | `type` | compared with UI-controller type |
| `Element+0x28` | `UIController` | assigned on type match |
| `ElementUIController+0x20` | `type` | inner comparison operand |

### Native calls / control flow

- `0x18035A547`: `produce_Brightness = 1.0f`.
- `0x18035A551`: `flash_Brightness = 1.0f`.
- `0x18035A55B..0x18035A5A9`: read `board`, call Unity `Object.op_Implicit` (`0x18131F870`), and if true assign `plantManager = board.plantManager` with IL2CPP write barrier.
- `0x18035A5AE..0x18035A5C8`: dereference `elementManager`, invoke generic `ElementManager.CreateNewElements<Plant>(this)` at instantiated native target `0x180439680`. The generic MethodDef itself has no direct non-instantiated pointer in the CodeGenModule.
- `0x18035A5CD..0x18035A716`: enumerate `elementManager.elements` with a real `List<Element>.Enumerator`.
- `0x18035A641..0x18035A6D3`: for every outer `Element`, enumerate all `elementUIControllers` with a real `List<ElementUIController>.Enumerator`; compare `Element.type` against `ElementUIController.type`; on equality assign `Element.UIController = controller`. There is no `break` after a match; the inner enumeration continues.
- `0x18035A6D3..0x18035A74C`: enumerator dispose/finally paths are emitted for both loops.
- `0x18035A751`: direct call `Plant.Start_Characteristic()` → `0x18035A160`, then return.

### Constants / conditions

- Brightness constants: `1.0f`, `1.0f`.
- Board assignment is conditional on Unity Object truthiness, not a guessed null-only branch.
- Missing `elementManager`, `elements`, or `elementUIControllers` follows native null-reference behavior; HF2 must not silently skip those failures.

### HF2 reconstruction status

Native behavior is **Exact** at the control-flow/field/call level. The current HF2 CIL uses nested integer-indexed List loops rather than the native enumerator + dispose shape. That preserves the expected pairing result when the Lists are not mutated, but it is not byte/exception-semantics exact. Therefore the current `Plant.Start` HF2 patch is marked **Behavior-equivalent**, not Exact. Do not promote it to Exact until the enumerator shape is restored and read back successfully.

## Plant.Update @ 0x18035C4D0

### Field offsets observed in native code

| Offset | Field |
|---:|---|
| `Plant+0x30` | `fX` |
| `Plant+0x34` | `fY` |
| `Plant+0x7C` | `ID` |
| `Plant+0x90` | `state` |
| `Plant+0x94` | `healthPoint` |
| `Plant+0xAC` | `livingTime` |
| `Plant+0xB0` | `isOnField` |
| `Plant+0xB3` | `isDied` |
| `Plant+0xB4` | `isSleep` |
| `Plant+0xB8` | `active` |
| `Plant+0xB9` | `beEaten` |
| `Plant+0xC0` | `updateRate` |
| `Plant+0xF8` | `elementManager` |
| `Plant+0x178` | `shadow` |
| `Plant+0x1A8` | `hpUIController` |
| `Plant+0x1E8` | `board` |
| `Plant+0x1F8` | `produce_Brightness` |
| `Plant+0x1FC` | `flash_Brightness` |
| `Plant+0x200` | `flash_Time` |
| `Plant+0x204` | `selected_Bright` |
| `Plant+0x20C` | `pT_chomperviking_barrelsPoint` |
| `Board+0x40` | `isFinished` |
| `Board+0xF0` | `zombieManager` |
| `ZombieManager+0x28` | zombie audio clip List |

### Direct managed call targets resolved by original MethodDef mapping

| Call site | Target | Original RID | Native target |
|---:|---|---:|---:|
| `0x18035C516` | `Plant.SetUpdateRate()` | 935 | `0x180359430` |
| `0x18035C604` | `Plant.SetBrightness(float)` | 928 | `0x1803583F0` |
| `0x18035C6DC` | `Board.BoardRuntime()` | 678 | `0x180325CC0` |
| `0x18035C739` | `HPUIController_Plant.Update_HPUI()` | 1570 | `0x18039D720` |
| `0x18035C743` | `Plant.Update_PlantFight()` | 852 | `0x18035BEE0` |
| `0x18035C75A` | `ElementManager.Update()` | 293 | `0x180315440` |
| `0x18035C7A9` | `Plant.Healed(float, Plant)` | 888 | `0x180351680` |
| `0x18035C7BD` | `Plant.PC_ChompervikingUpdateBarrels()` | 898 | `0x180352570` |
| `0x18035C8AF` | `Plant.PC_PotatoBoom()` | 902 | `0x180353A90` |
| `0x18035C8CF` | `Plant.PlantDie()` | 919 | `0x180356160` |
| `0x18035C874` | `GlobalStaticVars.AudioVolume()` | 312 | `0x18031AEA0` |
| `0x18035C891` | `GlobalStaticVars.CreateAudioAtPoint(..., ..., ...)` | 317 | `0x18031B230` |

Unity/BCL calls observed include `Time.deltaTime`, `System.Math.Max(float,float)`, `Component.get_transform`, `Transform.get_position`, `Camera.get_main`, and `List<AudioClip>.get_Item(24)`.

### Basic-block behavior

1. Call `SetUpdateRate()`.
2. If `flash_Time > 0`: `flash_Brightness -= (Time.deltaTime / flash_Time) * (flash_Brightness - 1.0f)` and `flash_Time -= Time.deltaTime`.
3. Start brightness candidates from `flash_Brightness` and `produce_Brightness`.
4. If `selected_Bright`: first candidate becomes `Math.Max(flash_Brightness, produce_Brightness)` and the second candidate becomes `3.0f`.
5. Call `SetBrightness(Math.Max(candidateA, candidateB))`.
6. If `produce_Brightness > 1.0f`, subtract `Time.deltaTime * 2.0f`.
7. Clear `selected_Bright` before the `active` test.
8. If not `active`, return.
9. Read `shadow.transform.position`; copy `x`/`y` to `fX`/`fY`.
10. If `isOnField && board.BoardRuntime() && !board.isFinished`: increment `livingTime` by `Time.deltaTime * updateRate`; if not sleeping, update plant HP UI, plant fight logic, and element manager.
11. Death path only executes when `!isDied && healthPoint < 0` (native floating comparison preserves unordered/NaN behavior).
12. For `ID == 6`, when `-healthPoint <= pT_chomperviking_barrelsPoint`: call `Healed(barrelsPoint, this)`, zero barrel points, refresh barrel visuals, and re-test health before continuing death.
13. If `beEaten`, play zombie audio clip index `24` at `Camera.main.transform.position` using `AudioVolume()` and `CreateAudioAtPoint`.
14. If `ID == 4 && state == 0`, call `PC_PotatoBoom()` and return immediately. This branch does **not** pass through the common `beEaten = false` tail.
15. Otherwise call `PlantDie()`.
16. All non-potato active-path exits that reach the common tail clear `beEaten = false`.

### Constants / branch conditions

- Brightness/time constants: `0.0f`, `1.0f`, `2.0f`, `3.0f`.
- Special IDs: `6` (Viking/chomper barrel protection) and `4` (potato boom death branch).
- Eaten audio List index: `24` (`0x18`).
- The native HP death comparison is strict negative (`healthPoint < 0`), not `<= 0`.

### HF2 reconstruction status

Native evidence for `Plant.Update` is **Exact**: field accesses, constants, branch order, and direct game call targets are all resolved. The current HF2 CIL follows this control flow, including selected-bright dual `Math.Max`, runtime gating, Viking barrel recovery, eaten-audio path, potato early return, and common `beEaten` reset. Until a real HF1/clean historical DLL is applied and independently read back, the emitted CIL is annotated **Strong (validation pending)** rather than Exact.

## Validation gate

HF2 is not considered fully accepted solely because the patcher compiles. Required gate remains:

1. CI build of patcher.
2. Apply to the same clean PC Cpp2IL/HF1-compatible input used for HF1 validation.
3. Reopen output with Mono.Cecil.
4. Independent ILSpy readback.
5. Compare emitted CIL behavior against the native control flow above.
6. Promote confidence only after steps 2-5 pass.

A newly regenerated Cpp2IL DLL is not interchangeable with the historical HF1 input: current Cpp2IL versions can renumber MethodDefs and can produce different MethodRefs. Native address attribution must therefore stay anchored to original metadata, and a failure caused solely by regenerated Cpp2IL reference shape must not be mislabeled as a gameplay/native mismatch.