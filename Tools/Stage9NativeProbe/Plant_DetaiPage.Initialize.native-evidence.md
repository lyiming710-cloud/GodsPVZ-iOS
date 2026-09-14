# Stage9.1 prefetch — `Plant_DetaiPage.Initialize(Plant)` PC-native evidence

Status: **read-only prefetch**. Do not mutate the gameplay DLL from this evidence alone. Promote this target only if the post-Projectile exact-R3 runtime reproduces the same Board-path failure.

## Authority inputs

- Original PC archive: `GodsPVZ_1.0.2.zip`
- Original `GameAssembly.dll` SHA-256: `9ebd7ca996a5b03fb4a766f7a2502b660d581ddbbb36af4d7bf2f06a211da39d`
- Original `global-metadata.dat` SHA-256: `ad992341add498bd1018ac980f42171e0737b0793bf945c8714ad70a445b36b9`
- Assembly-CSharp CodeGenModule method-pointer table VA: `0x181B82D60`

## Managed fingerprint on dc39

- Type: `Plant_DetaiPage`
- Target: `Initialize(Plant)`
- token: `0x06000667`
- RID: `1639`
- current managed CodeSize: `830`
- locals: `25`
- EH regions: `0`
- managed semantic fingerprint: `56e926b418a2fd871556be2bd7652b46b8c7361f5e308b54fa2d3351c2ff005c`
- first runtime-visible access fault: `IL_007C ldfld bool Plant::isOnField`
- `Plant::isOnField` is private in reconstructed metadata.

The same managed body also contains corrupted value-type lowering, so the method must not be repaired as a one-instruction FieldAccess patch:

- position path eventually feeds `Transform.set_position(Vector3)` from an invalid float/byref/object lowering;
- final range-image path feeds `Graphic.set_color(Color)` from an invalid float/byref lowering.

Exact-reference ILSpy still reports target-local diagnostics at these value-type sites, confirming that exact Unity references do not repair the body.

## PC-native attribution

- RID 1639 method-pointer entry VA: `0x181B86090`
- direct PC method pointer: `0x1803A3E30`
- `.pdata` runtime-function range: `0x1803A3E30–0x1803A41C3`
- native span: `0x393` bytes / 915 bytes
- exact native slice SHA-256: `80527520200166f1c38e6e3c64f952e5e99d439b59cff56b888295d9a5640fdc`
- next MethodDef RID 1640 pointer is `0x1803A41D0`, consistent with the end of the attributed body.

## Recovered control-flow semantics

PC native performs the following high-level behavior:

1. `this.plant = thePlant`.
2. Evaluate Unity object truthiness for `thePlant`.
   - false: `skill.SetActive(false)` and `p_skill = null`.
   - true:
     - `text_name.SetText(thePlant.plantName, true)`;
     - `p_skill = this.plant.skill`;
     - read the original Plant `isOnField` byte and call `skill.SetActive(isOnField)`.
3. If `this.plant` is a live Unity object:
   - read `this.plant.transform.position`;
   - set `this.skill.transform.position` to `(x, y + 30.0f, z)`.
   - PC constant at `0x1815A7A2C` starts with `30.0f` and is the Y offset used by this path.
4. Call `LoadSkillLogo()`.
5. If `p_skill == null`:
   - `skillButton.enabled = false`;
   - `skillAuto.gameObject.SetActive(true)`;
   - `text_key.gameObject.SetActive(false)`;
   - `chargeLayer.SetActive(false)`.
6. Else:
   - if `p_skill.skillTriggerType == SkillTriggerType.Manual` (native enum value 1):
     - `skillButton.enabled = true`;
     - `skillAuto.gameObject.SetActive(false)`;
     - `text_key.gameObject.SetActive(true)`;
   - if `p_skill.maxChargedLayer > 1`, call `chargeLayer.SetActive(true)`; when `<= 1`, native does not issue a charge-layer SetActive call in this branch.
7. `dataPage.LoadData(this)`.
8. `dataPage.CheckText(0)`.
9. `skillRange = false`.
10. Set `rangeIamge.color` to `(1,1,1,1)`; PC constant at `0x1815A7B70` is four `1.0f` values.

## Managed reconstruction constraint

Do **not** make `Plant::isOnField` public merely to make reconstructed CLR IL legal. `Plant` already exposes a valid public `IsOnField()` method whose current managed body returns the private field. If this target is promoted after runtime confirmation, the managed recovery should use that existing accessor as a CLR-safe lowering for the native direct field read, while preserving field metadata unchanged.

Because the target contains multiple Cpp2IL value-type corruptions beyond the access fault, recovery should rebuild the whole single MethodDef from the native semantics above, with normal typed `Vector3` / `Color` lowering. Required gates remain: one-MethodDef semantic isolation, zero FieldDef drift, preservation of Batch1 + Plant ctor + Projectile ctor, exact-reference ILSpy, then one exact-R3 runtime.
