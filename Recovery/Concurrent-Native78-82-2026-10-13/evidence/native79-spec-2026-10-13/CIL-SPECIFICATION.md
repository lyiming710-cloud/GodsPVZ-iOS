# Native79 reconstruction specification

This is static evidence and a proposed reconstruction specification. No candidate, runtime, fixture, compiler, acceptance, or Unity test was executed. Existing files were read only. The generated evidence script, JSON, copied native disassembly, and this specification are confined to this new folder.

`PRIMARY-EVIDENCE.json` records the original input hashes, direct PE literal and float decoding, original method-pointer/slice agreement, and the public quaternion getter proof. The three native55 disassembly extracts are exact copies of the previously recorded disassembly; their native bytes were independently matched to the unchanged original PC PE.

| Original input | SHA-256 |
| --- | --- |
| `audit/stage9-native3/inputs/GameAssembly.dll` | `9ebd7ca996a5b03fb4a766f7a2502b660d581ddbbb36af4d7bf2f06a211da39d` |
| `audit/stage9-native3/inputs/global-metadata.dat` | `ad992341add498bd1018ac980f42171e0737b0793bf945c8714ad70a445b36b9` |
| `audit/native30-continuation-2026-10-02/native55-native.json` | `304b632ad13168da91e4b03e2824f69f053c592f59b3a089756a5e4114445277` |

## 0x06000238 — SavesManager.BackupPlayerSave

Original native interval: `[0x18033E270, 0x18033E3EF)`, 383 bytes, SHA-256 `d2f9ae1da30e6a10be3b9c0885f4b28640682b61c807db2cb5ed6a285edf9959`.

The suffix slot `0x181BA18C8` contains tagged literal usage `0xA00004E5`, usage type 5, literal index 626. Original v31 metadata yields UTF-8 `2D E5 A4 87 E4 BB BD`, exactly `-备份` (`U+002D U+5907 U+4EFD`). The JSON preserves the UTF-8 bytes and code units even if a terminal renders non-ASCII incorrectly.

The reconstruction should use one `Save` local, initialized by `newobj Save::.ctor()`. The original has allocation at `0x18033E2B8`, constructor at `0x18033E2C5`, and passes that same allocation to `InitializeSave` at `0x18033E2D3`.

The required sequence, expressed as semantic CIL operations, is:

```text
newobj instance void Save::.ctor()
stloc newSave
ldarg.0; ldloc newSave
call instance void SavesManager::InitializeSave(Save)

ldloc newSave
ldarg.0; ldfld Save SavesManager::playerSave
callvirt instance void Save::Copy(Save)

ldloc newSave
ldarg.0; ldfld Save SavesManager::playerSave
ldfld string Save::playerName
ldstr "-备份"
call string System.String::Concat(string,string)
stfld string Save::playerName

ldarg.0; ldfld SaveList SavesManager::saveList
ldfld System.Collections.Generic.List<Save> SaveList::saveList
ldloc newSave
callvirt instance void System.Collections.Generic.List<Save>::Add(!0)

ldarg.0
call instance void SavesManager::SavePlayerSave()
call MainUIController MainUIController::get_instance()
callvirt instance void MainUIController::LoadData()
ret
```

Current-module references/signatures are the implementer's responsibility; original MethodDef identities are:

| Operation | Original Assembly-CSharp MethodDef | Native VA |
| --- | --- | --- |
| `Save::.ctor()` | `0x060004C7` | `0x180380390` |
| `SavesManager::InitializeSave(Save)` | `0x06000242` | `0x18033F0E0` |
| `Save::Copy(Save)` | `0x060004C0` | `0x18037F1B0` |
| `SavesManager::SavePlayerSave()` | `0x0600024B` | `0x1803402F0` |
| `MainUIController::LoadData()` | `0x0600063A` | `0x18039FBB0` |

Required alias and exception behavior:

- Allocate and call the constructor before `InitializeSave`. Do not obtain `playerSave` or `saveList` before these operations.
- Pass `newSave` to `InitializeSave`, then pass the *current* `this.playerSave` to `newSave.Copy`. A null source is passed into `Copy`; do not preempt it with a source-null skip or an early source-null throw.
- Read `this.playerSave` again after `Copy` returns. `Copy` can change it through observable state/callbacks. Read `playerName` from that second object; do not reuse the first source or `newSave.playerName`.
- Write `newSave.playerName` only after `String.Concat` returns. Concat accepts a null string operand according to its existing contract.
- Read `this.saveList` and then its `saveList` field only after the name write. Add the captured `newSave` exactly once. Use `List<Save>.Add`, not `List<object>` private fields or `AddWithResize`.
- Invoke `SavePlayerSave` only after Add returns. Resolve the MainUI singleton after `SavePlayerSave` returns, so a callback replacement is observed; call `LoadData` on that newly read singleton.
- Native raw-null failures occur at the dereferences/calls, with the preceding allocations, callbacks and writes already performed. Do not replace a null failure with a return. CLR `callvirt` is suitable for ordinary receiver null enforcement; `call` to a null receiver can defer failure into a callee and needs care.

The original Add is an inlined generic-list operation: increment `_version`, read `_items`, read `_size`, and then use a fast store or `AddWithResize`. This confirms the typed Add operation for the real `List<Save>` field. This report does not independently prove a replacement library's `Add` under deliberately corrupted private list internals; the selected current CLR library can be inspected by the main agent if that scenario is in scope.

## 0x06000365 — Plant.Explode

Original native interval: `[0x18034FF40, 0x1803500B2)`, 370 bytes, SHA-256 `b46d6d981e0e29e11a67f76322c8b27532394acadb686790767b06414792439b`.

| Operand | Original address | Single bits | Exact decoded single |
| --- | --- | --- | --- |
| Uniform local scale | `0x1815A7AC8` | `0x3F99999A` | `1.2000000476837158` (`1.2f`) |
| Shake argument 1 | `0x1815A7C0C` | `0x3E19999A` | `0.15000000596046448` (`0.15f`) |
| Shake argument 2 | `0x1815A7B04` | `0x41000000` | `8.0f` |

Native fields `Plant.fX`, `fY`, and `fZ` are respectively offsets `0x30`, `0x34`, `0x38` and original fields `0x040004A4`, `0x040004A5`, `0x040004A6`.

Required typed locals can be `GameObject particle`, `Transform positionTransform`, `Transform scaleTransform`, `Single positionY`, `Vector3 position`, `Vector3 scale`, `Damage damage`, and `Skill capturedSkill`. Additional locals used only for explicit failure enforcement must remain typed and initialized normally.

Required semantic sequence:

```text
if (this.ID != 2) return;
particle = current ParticlesManager.instance.CreatNewParticle((ParticleState)2);
positionTransform = particle.get_transform();
positionY = this.fZ + this.fY; // left operand fZ, right operand fY
raw-null failure if positionTransform is null;
position = Vector3(this.fX, positionY, +0.0f);
positionTransform.set_position(position); // Vector3 BY VALUE

scaleTransform = particle.get_transform(); // second getter on same captured particle
raw-null failure if scaleTransform is null;
scale = Vector3(1.2f, 1.2f, 1.2f);
scaleTransform.set_localScale(scale); // Vector3 BY VALUE

current this.board.Shake(0.15f, 8.0f);
damage = this.GetDamage(null, -1, 0);
damage.AreaDamage(); // discard the Boolean result
capturedSkill = current this.skill;
raw-null failure if capturedSkill is null;
if (capturedSkill.ID == 1) capturedSkill.Function(this);
this.PlantDie();
return;
```

Exact first position-read timing is visible at `0x18034FFAC–0x18034FFC7`: the first transform getter returns, native loads `fZ`, performs `addss fY` with `fZ` as the first operand, checks the returned transform for raw null, then loads `fX`. A fully literal reconstruction can put the raw-null check after the y computation and before the x read; a C# expression `new Vector3(fX, fY + fZ, 0)` changes both field/operand order. Preserve the single precision operation, including NaN operand selection. The z component is positive zero from `xorps`.

Keep the first getter before field reads, because it can mutate position fields in an adversarial callback model. Keep the second getter after `set_position`, and call it on the previously captured particle. It can return a different transform. Do not cache the first transform for both setters or recreate/reload the particle. Read `board` after the scale setter so callback replacements are observed.

`GetDamage` passes `Projectile null`, signed integer `-1`, and integer `0`, in that order. A null returned Damage fails before any `skill` read. `AreaDamage` is called regardless of its Boolean result, and its return is popped/discarded. Read `this.skill` only after AreaDamage returns. Capture that skill once for both the ID comparison and `Function` receiver. If ID is not 1, proceed directly to `PlantDie`; if Skill is null, throw before `PlantDie`. If any getter, setter, Shake, GetDamage, AreaDamage, Function, or PlantDie throws, propagate it immediately.

The native ID check is the sole ordinary early return. No Unity fake-null truth test or defensive null skip appears on the particle, transforms, board, Damage, or Skill paths.

| Call | Original identity |
| --- | --- |
| `CreatNewParticle(ParticleState)` | `0x060001EA`, `0x180321F30` |
| `Board.Shake(Single,Single)` | `0x060002CD`, `0x1803276E0` |
| `Plant.GetDamage(Projectile,Int32,Int32)` | `0x0600036D`, `0x180350E40` |
| `Damage.AreaDamage()` | `0x0600010C`, `0x180311890` |
| `Skill.Function(Plant)` | `0x060004E0`, `0x180386550` |
| `Plant.PlantDie()` | `0x06000397`, `0x180356160` |
| `GameObject.get_transform()` | Unity Core `0x06000840`, `0x18131C070` |
| `Transform.set_position(Vector3)` | Unity Core `0x06000995`, `0x18132FBB0` |
| `Transform.set_localScale(Vector3)` | Unity Core `0x060009A4`, `0x18132FA50` |

CIL setter arguments must be actual `Vector3` values: `ldloc vector`, or `ldloca vector; ...` for construction followed by `ldloc vector`. The damaged source's address of an `Object` local cannot be used as a by-value `Vector3` operand.

## 0x0600075F — TMPro.Examples.ObjectSpin.Awake

Original native interval: `[0x1803B4D80, 0x1803B4F0A)`, 394 bytes, SHA-256 `4f014919f172dab528d838970231c04e6ed8adc26217ae346532100915a1ae09`.

The public `Quaternion.get_eulerAngles` replacement has direct proof, rather than a familiarity-based assumption:

1. In the hashed resolver `audit/stage9-native3/resolver/UnityEngine.CoreModule.dll`, SHA-256 `83192116b34abac2bb1797ec0e3943362be33b028b1eefd2e561d5fdc1c3312f`, the public instance getter is MethodDef `0x060017A8`, signature bytes `20 00 11 88 1C`, body hash `f0beaff87a0b2736d6e8328fcd2beb6a684715837789f9c1e75ab0a2d5d7c576`.
2. Its actual CIL loads the quaternion value, calls private `Internal_ToEulerRad` (`0x06001794`), loads `ldc.r4` raw bytes `E1 2E 65 42` (`0x42652EE1`, `57.295780181884766`), calls `Vector3.op_Multiply(Vector3,Single)` (`0x0600177E`), and calls private `Internal_MakePositive` (`0x060017A7`). There are no extra application-state callbacks in this getter.
3. That Vector3 multiply CIL loads each vector component first and the scalar second before `mul`. Its body hash is `2732399e87eb8e33f8c9bd400152148e39a43e4199139839e489420fa01383ec`.
4. Original PC native `Quaternion.get_eulerAngles` is mapped as original Unity MethodDef `0x0600069D`, native `[0x18130CE20,0x18130CEFC)`, SHA-256 `91550921b8f376a9f93c05a193e7588957cc14dadce3aa82bb49d4b528c8232b`. It invokes the same engine function-pointer slot `0x181CEAFC8` and the same icall name pointer `0x181621060` as original `Internal_ToEulerRad` at `0x18130C7F0`. The name is exactly `UnityEngine.Quaternion::Internal_ToEulerRad_Injected(UnityEngine.Quaternion&,UnityEngine.Vector3&)`.
5. The original public getter multiplies component-first by the same rad-to-degree bits `0x42652EE1` and inlines the same MakePositive logic/constants as original `Internal_MakePositive` at `0x18130C710`: lower bound `0xBBBBBF2F`, upper bound `0x43B3FF44`, and full turn `0x43B40000`. Ordered comparisons leave NaN components unchanged; below lower bound add 360, otherwise above upper bound subtract 360. The static getter proof and the original helper proof are both preserved in `PRIMARY-EVIDENCE.json`.

The original MakePositive helper is a leaf without an exact `.pdata` entry. Its extracted interval is bounded by the next mapped native method pointer at `0x18130C7A0`, with `ret` at `0x18130C791` and alignment `int3`s after it. The public getter and ToEulerRad wrapper have exact `.pdata` intervals. This boundary distinction does not affect the getter proof.

The proposed legal public sequence is:

```text
this.m_transform = this.get_transform();
Quaternion rotation = current this.m_transform.get_rotation();
Vector3 euler = rotation.get_eulerAngles(); // address of quaternion LOCAL for instance value-type call
Transform positionTransform = current this.m_transform;
this.m_initial_Rotation = euler;
raw-null failure if positionTransform is null;
this.m_initial_Position = positionTransform.get_position();
Light capturedLight = this.GetComponent<Light>();
Color color;
if (UnityEngine.Object.op_Inequality(capturedLight, null)) {
    raw-null failure if capturedLight is null;
    color = capturedLight.get_color();
} else {
    color = Color(+0.0f,+0.0f,+0.0f,1.0f);
}
this.m_lightColor = UnityEngine.Color32.op_Implicit(color);
return;
```

Use `stloc rotation; ldloca rotation; call instance Vector3 Quaternion::get_eulerAngles()` for the public instance getter. Do not call private Unity helpers from Assembly-CSharp. Do not call Transform.get_eulerAngles in place of the native rotation getter plus quaternion conversion; its engine boundary/callback trace would differ.

The first `m_transform` read for rotation occurs after the initial transform field store. Read `m_transform` again after quaternion conversion returns and before assigning `m_initial_Rotation`: the original loads the field at `0x1803B4E59`, stores the rotation at `0x1803B4E5D` and `0x1803B4E62`, raw-null checks that captured transform, and invokes its position getter. Thus an already computed initial rotation is written before a null second transform fails. `get_rotation` and ToEulerRad operate on a captured quaternion value. GetComponent is performed once after the initial-position write. The Light local is captured once and is used for the Unity `op_Inequality` comparison and the color getter.

The fallback 16 bytes at `0x1815A7EE0` are `00 00 00 00 00 00 00 00 00 00 00 00 00 00 80 3F`, exactly `Color(0,0,0,1)` (black with alpha 1), not white or clear. Convert the complete Color by the existing Color32 implicit conversion exactly once, then store the resulting Color32 field. Do not hand-round channels or replace the conversion with integer constants when the Light branch supplies an arbitrary Color.

The Unity fake-null comparison is intentional on the Light branch. Do not replace it with a CLR reference-null check, and do not use a Unity fake-null comparison for the Transform raw-null failures.

Native ObjectSpin multiplies y, then x, then z when preparing the degree vector; the public getter evaluates x, y, z. Each operation has the same component-first/scalar-second operands, no intervening user callback, and the same MakePositive component processing. This is a static semantic-equivalence argument, not an executed floating-point acceptance result.

## Remaining scope and gaps

The primary PE constants, three original method bodies, field identities, and the optional public quaternion getter equivalence are supported by read-only evidence. The current recovery DLL's references, local table, visibility, field layout, stack validity, chosen public singleton getter bodies, and typed generic MemberRef encoding must still be inspected by the implementation owner before patching. None of those checks or any candidate execution were performed in this subtask. Do not count this specification as a qualified recovery or whole-game result.
