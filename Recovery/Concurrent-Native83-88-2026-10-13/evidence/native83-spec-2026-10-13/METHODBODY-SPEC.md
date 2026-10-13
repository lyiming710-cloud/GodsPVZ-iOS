# Native83 source-only specifications

Targets: `0x06000056 PathDataEditor.InsertNewPath()` and `0x06000119 HoleTeleport.ctor(Zombie,int32,int32)`. No tests, application runtime, builds, candidates, edits outside this new directory, commits or pushes were performed. The scripts parse original PE/metadata and the existing parent82 archived helper bodies as source data only. Gemini retains all testing.

Original inputs were re-hashed:

- GameAssembly.dll: `9ebd7ca996a5b03fb4a766f7a2502b660d581ddbbb36af4d7bf2f06a211da39d`.
- global-metadata.dat: `ad992341add498bd1018ac980f42171e0737b0793bf945c8714ad70a445b36b9`.

Parent82 bytes read from existing bundle member `candidates/native82/candidate1.dll` match SHA256 **`48b9e89e941c041d6c11c4064ad05d9384a1a4dd51d5f8d3fad9192cbda5adbf`**, as does current Cecil inventory `audit/native83-codex-2026-10-13/metadata-current.json`.

Original native hashes re-read from PE match native55:

| Method | Native range | Bytes | SHA256 |
|---|---|---:|---|
| InsertNewPath | `0x180305940..0x180305AFC` | 444 | `8dc4cd61b139da50ec857f46e40aba3b2d4025d1602724aa19d33a69d4c1c436` |
| HoleTeleport.ctor | `0x18031CCA0..0x18031CE6F` | 463 | `986b68b03aab2ad6eef16ec36b58c26efb9b126eec58c513c363315c3eb86be2` |

The first MethodBody specification is closed at the source/call-contract level. The second original logical recipe is also fully derived, but a **current transitive constructor dependency discrepancy** remains below; it must not be hidden in an acceptance claim.

## Original generic and class identities

Registration Types/GenericInst/MethodSpec tables were decoded through the same inspectible original-PC parser used previously. Ordinary Core/Assembly-CSharp call pointers were independently re-derived from original CodeGenModule tables, not inferred from damaged IL.

| Original usage cell | Encoded value | Resolution |
|---|---|---|
| `0x181B9E828` | `0x20006CC3` | EnemyPath class, Types index 13921 |
| `0x181BAA508` | `0xC0023233` | MethodSpec 71961, List<EnemyPath>.Insert(int32,EnemyPath) |
| `0x181BB3DF0` | `0xC002350D` | MethodSpec 72326, List<Path_enemyEditor>.Insert(int32,Path_enemyEditor) |
| `0x181BB7DB8` | `0xC0024627` | MethodSpec 74515, Object.Instantiate<Path_enemyEditor>(Path_enemyEditor) |
| `0x181BA35D0` | `0x200073F5` | Hide class, Types index 14842 |
| `0x181BA4278` | `0xC0023075` | MethodSpec 71738, List<AudioClip>.get_Item(int32) |

Both typed Insert MethodSpecs share original mscorlib MethodDef `0x060023ED` and native body `0x18084D9C0`, but their generic class arguments differ. Neither is List<object>.Insert.

Instantiate MethodSpec 74515 fixes original Unity Core generic MethodDef `0x060008AD` to Path_enemyEditor; native caller invokes shared specialization `0x1804A0690` with this precise MethodInfo cell. This is an existing public generic call, not a replacement for a guessed private helper.

## 0x06000056 PathDataEditor.InsertNewPath()

Original field bindings:

| Owner.field | Token | Native offset | Type |
|---|---|---:|---|
| PathDataEditor.enemy_BoardEditor | `0x04000055` | +0x20 | Enemy_boardEditor |
| PathDataEditor.path_EnemyEditor_prefab | `0x04000057` | +0x30 | Path_enemyEditor |
| PathDataEditor.pathIndex | `0x04000058` | +0x38 | int32 |
| Enemy_boardEditor.preEnemy | `0x04000051` | +0x38 | Enemy |
| Enemy.gridX | `0x040001F9` | +0x18 | int32 |
| Enemy.row | `0x040001F8` | +0x14 | int32 |
| Enemy.myPath | `0x040001FD` | +0x30 | List<EnemyPath> |
| Path_enemyEditor.enemyEditor | `0x04000067` | +0x38 | Enemy_boardEditor |
| Path_enemyEditor.enemyPath | `0x04000068` | +0x40 | EnemyPath |
| Enemy_boardEditor.path_EnemyEditors | `0x04000054` | +0x50 | List<Path_enemyEditor> |

Native sequence, captures and fault order:

1. Read this.enemy_BoardEditor at `0x180305995`; raw null -> common NRE at `0x180305AF6`. Read that editor's preEnemy at `0x1803059B6`; raw null -> same NRE. **Capture that Enemy for the first constructor/list operation.**
2. Read captured Enemy.gridX at `0x1803059CA`, then Enemy.row at `0x1803059CD`, **before allocating the EnemyPath** at `0x1803059D0`. Call its actual `(int32 gridX,int32 gridY,float waitingTime,BoardConfig)` constructor `0x060002A4 / 0x1803299E0` with those captured int32 values, **+0.0f (bits 0x00000000)** and **literal null BoardConfig**. Capture the returned object.
3. Only after the constructor returns, read captured Enemy.myPath at `0x1803059F4`. Raw null -> NRE before reading index. Read current this.pathIndex at `0x180305A01`, add one with int32 wrapping, and call **List<EnemyPath>.Insert** at `0x180305A10`. A constructor callback changing the captured Enemy.myPath affects this read; changing this.enemy_BoardEditor does not replace the captured Enemy.
4. After that Insert returns, read this.path_EnemyEditor_prefab at `0x180305A1C`, capture it across Unity Object class initialization, and call Instantiate<Path_enemyEditor> at `0x180305A38`. Do not read the prefab earlier. The public helper determines null/destroyed-template errors; the caller has no preliminary Unity-null skip.
5. Read **current this.enemy_BoardEditor again** at `0x180305A3D`, after Instantiate. Raw null cloned result -> NRE at `0x180305A44`; otherwise assign that current editor value (which may itself be null) to clone.enemyEditor, then assign captured new EnemyPath to clone.enemyPath. Do not substitute the initially captured board editor.
6. Call captured clone.LoadData() at `0x180305A6F`, exact MethodDef `0x06000060 / 0x180306260`. Call that clone's Component.get_transform at `0x180305A79`; capture returned **child transform**, even if raw null.
7. Re-read current this.enemy_BoardEditor at `0x180305A7E`; raw null -> NRE **before checking the child transform**. Call that editor's get_transform at `0x180305A8C`. After this call returns, raw null child -> NRE. Call captured child.SetParent(returned parent transform, true) at `0x180305AA2`, exact Core bool overload `0x060009AB / 0x18132E900`.
8. A null **parent transform return value** is permitted here; the native code has no check for it and forwards it to SetParent. Do not add a parent-transform null skip/throw. The child receiver is the captured result, not a second getter read.
9. After SetParent returns, re-read current this.enemy_BoardEditor at `0x180305AA7`; raw null -> NRE. Read that editor's path_EnemyEditors at `0x180305AB0`; raw null -> NRE. Read current pathIndex **again** at `0x180305AB9`, increment with wrapping, and call **List<Path_enemyEditor>.Insert** at `0x180305AC8`, using captured clone.
10. After that second Insert returns, read current pathIndex **a third time** at `0x180305ACD`, increment with wrapping, and transfer to **this.LoadData(int32)** at native tail target `0x180305B00`, MethodDef `0x06000058`. Recover as a CIL call followed by ret; no caller field write to pathIndex is present.

All raw-null paths converge on System.NullReferenceException helper `0x180250150`; all later side effects stop at the fault. No Unity op_Implicit/op_Equality/op_Inequality calls occur in this caller. This method has no loops or managed cleanup/EH; its original UNWIND_INFO flags are 0. CIL must not append an unconditional NRE after the successful final LoadData call.

Constructor witness: original EnemyPath.ctor calls the no-op Object base constructor, stores waitingTime/gridX/gridY/isEnd=false, and for literal null BoardConfig reads Vector3.zeroVector into position. Parent82's actual `0x060002A4` body implements that same branch with public Vector3.get_zero (`0x0A0000FC`), and no BoardConfig position helper runs on this caller's null argument. Full Vector3.get_zero IL was proven in `audit/native78-spec-2026-10-13/unity-zero-source-inspection.json`: it returns the same private zeroVector unchanged. No guessed constructor arguments or current unused BoardConfig local are needed.

Proposed body: MaxStack 4, no EH. Reuse existing typed locals Enemy V_1, Path_enemyEditor V_2, child Transform V_3, parent Transform V_4, EnemyPath V_6. Explicit list raw-null checks below preserve their position before reading pathIndex. They throw; they do not silently skip.

```cil
    ldarg.0
    ldfld Enemy_boardEditor PathDataEditor::enemy_BoardEditor
    ldfld Enemy Enemy_boardEditor::preEnemy
    stloc.1
    ldloc.1
    ldfld int32 Enemy::gridX
    ldloc.1
    ldfld int32 Enemy::row
    ldc.r4 0.0
    ldnull
    newobj instance void EnemyPath::.ctor(int32,int32,float32,BoardConfig)
    stloc.s V_6
    ldloc.1
    ldfld class List<EnemyPath> Enemy::myPath
    dup
    brtrue FIRST_LIST_READY
    pop
    newobj instance void System.NullReferenceException::.ctor()
    throw
FIRST_LIST_READY:
    ldarg.0
    ldfld int32 PathDataEditor::pathIndex
    ldc.i4.1
    add
    ldloc.s V_6
    callvirt instance void List<EnemyPath>::Insert(int32,EnemyPath)
    ldarg.0
    ldfld Path_enemyEditor PathDataEditor::path_EnemyEditor_prefab
    call !!0 UnityEngine.Object::Instantiate<Path_enemyEditor>(!!0)
    stloc.2
    ldloc.2
    ldarg.0
    ldfld Enemy_boardEditor PathDataEditor::enemy_BoardEditor
    stfld Enemy_boardEditor Path_enemyEditor::enemyEditor
    ldloc.2
    ldloc.s V_6
    stfld EnemyPath Path_enemyEditor::enemyPath
    ldloc.2
    call instance void Path_enemyEditor::LoadData()
    ldloc.2
    call instance Transform UnityEngine.Component::get_transform()
    stloc.3
    ldarg.0
    ldfld Enemy_boardEditor PathDataEditor::enemy_BoardEditor
    callvirt instance Transform UnityEngine.Component::get_transform()
    stloc.s V_4
    ldloc.3
    ldloc.s V_4
    ldc.i4.1
    callvirt instance void UnityEngine.Transform::SetParent(Transform,bool)
    ldarg.0
    ldfld Enemy_boardEditor PathDataEditor::enemy_BoardEditor
    ldfld class List<Path_enemyEditor> Enemy_boardEditor::path_EnemyEditors
    dup
    brtrue SECOND_LIST_READY
    pop
    newobj instance void System.NullReferenceException::.ctor()
    throw
SECOND_LIST_READY:
    ldarg.0
    ldfld int32 PathDataEditor::pathIndex
    ldc.i4.1
    add
    ldloc.2
    callvirt instance void List<Path_enemyEditor>::Insert(int32,Path_enemyEditor)
    ldarg.0
    ldarg.0
    ldfld int32 PathDataEditor::pathIndex
    ldc.i4.1
    add
    call instance void PathDataEditor::LoadData(int32)
    ret
```

Captured clone cannot become raw null after the first successful stfld; direct nonvirtual clone.LoadData/get_transform calls retain the proven receiver. Child SetParent uses callvirt so its raw-null check occurs **after** parent getter evaluation. All additions are `add`, not `add.ovf`.

Current-parent bindings: EnemyPath constructor `0x060002A4`; typed EnemyPath Insert `0x0A000148` (a second equivalent typed MemberRef exists at `0x0A000436`); Instantiate<Path_enemyEditor> `0x2B000008`; clone.LoadData `0x06000060`; get_transform `0x0A000001`; bool SetParent `0x0A00002F`; final caller.LoadData(int32) `0x06000058`; NRE ctor `0x0A000022`. List<Path_enemyEditor> TypeSpec `0x1B00000C` exists, but no typed Insert MemberRef for it was present in the inspected all-MemberRef inventory. Bind/add that precise typed member rather than retaining object Insert `0x0A000049`. Existing local signature `0x1100003F` already has the used types; no retyping is needed for this recipe.

## 0x06000119 HoleTeleport.ctor(Zombie,int32,int32)

Original parent type is **DeviceEvent**, not Buff. DeviceEvent has countdown(float) at +0x10 and device(Device) at +0x18. HoleTeleport's fields are zombie `0x0400013E`/+0x20, gridX `0x0400013F`/+0x28, gridY `0x04000140`/+0x2C.

Base constructor proof: original DeviceEvent.ctor `0x06000117 / 0x18030F300` is `xor edx,edx; jmp 0x180302170`, and the shared Object.ctor body at `0x180302170` is `ret`. HoleTeleport has inlined this base no-op call at `0x18031CD05`. Parent82's actual DeviceEvent constructor is `ldarg.0; call Object::.ctor; ret`, 7 bytes. The proper CIL base call is `DeviceEvent::.ctor()`. Calling Buff::.ctor on the HoleTeleport receiver would use the wrong hierarchy.

Native sequence:

1. Perform the no-op base construction, then assign this.zombie from the preserved argument, this.gridX and this.gridY, in that order, at `0x18031CD0E/CD1A/CD1D`. These writes happen **before Hide allocation**, and remain observable if a later operation throws.
2. Allocate the **Hide** class at `0x18031CD27`, using original class usage cell `0x181BA35D0`. Call Buff.ctor on the newly allocated Hide at `0x18031CD34`, then write Hide.hide=true, Hide.zombie=preserved zombie argument, Buff.duration=0.5f, Buff.buffType=0, Buff.original=false.
3. This exact inlined sequence is also the complete original real **Hide(bool,Zombie)** constructor: `0x060000FD / 0x18031C900..0x18031C954`. That constructor calls Buff.ctor once, writes those same fields in the same order and returns. Parent82's actual Hide ctor body has the same calls/stores. Therefore the original logical CIL recipe may use **one `newobj Hide::.ctor(true,zombie)`**. Do not allocate Buff and cast it to Hide, and do not call Buff.ctor twice.
4. Only after all Hide initialization, raw-null-check the preserved zombie argument at `0x18031CD62`. Then read that argument's buffManager (+0x228, field `0x0400061A`) once at `0x18031CD6B`; raw null -> NRE. Call BuffManager.AddBuff(Buff) `0x06000102 / 0x180310190` at `0x18031CD81`, passing captured Hide. Never substitute a re-read of this.zombie.
5. After AddBuff returns, initialize ResourceManager static storage if needed and read ResourceManager.zombieClips `0x040002EF` / static +0x120, once. Raw null list -> NRE. Get Item **6** through the closed List<AudioClip> method at `0x18031CDC7`; capture returned clip. No clip-null check occurs.
6. Only after the clip lookup, call Camera.main once at `0x18031CDD1`; raw null camera -> NRE. Call captured camera.transform once at `0x18031CDE4`; raw null result Transform -> NRE. Call captured Transform.position once at `0x18031CDFE`; copy/capture the Vector3.
7. Then call GlobalStaticVars.AudioVolume once at `0x18031CE2A`; multiply its return **as the left operand** by float32 **1.2f**, original bytes `9a99993f` / bits `0x3F99999A` at `0x1815A7AC8`. Pass this scaled value and float32 **1.5f**, bytes `0000c03f` / bits `0x3FC00000` at `0x1815A7ACC`, to the exact four-argument CreateAudioAtPoint `0x0600013E / 0x18031B350` at `0x18031CE4F`. Arguments are captured clip, captured position, scaled volume, pitch 1.5. Ignore its AudioSource result.
8. There are no field writes after initial HoleTeleport assignment, no Unity pseudo-null comparisons, no Random calls and no cleanup handlers. The five native pdata parts use flags 0/4; **4 is CHAININFO, not EH/UH**. They form one method with one common NRE path `0x18031CE69`, not managed finally regions.

Constructor side effects and allocation order proven from complete original helper bodies:

- Buff.ctor `0x180310BB0..0x180310C8D`: set name to String.Empty; set duration=1.0f; allocate/construct AttackRange and assign buffRange; allocate/construct List<Buff> and assign childBuffs; finish no-op Object base construction.
- AttackRange.ctor `0x060000E0 / 0x1803002A0..0x1803003A3`: allocate/construct **List<Rect>** and assign rects (+0x40 / `0x040000E3`), then **List<Single>** and assign radius (+0x48 / `0x040000E4`), then **List<Vector3>** and assign centers (+0x50 / `0x040000E5`); finish Object base construction.
- Original usage cells identify the typed parameterless list constructors: Rect MethodSpec 34288 at `0x181BB6568`, Single MethodSpec 34768 at `0x181BB9B58`, Vector3 MethodSpec 35788 at `0x181BC12D8`, Buff MethodSpec 71771 at `0x181BA4FF8`. The JSON contains exact decoded indices/records; no missing list/enum structure was guessed.
- Explicit instance allocation order before the zombie-null check is: **Hide -> AttackRange -> List<Rect> -> List<Single> -> List<Vector3> -> List<Buff>**. Normal generic type initialization/backing-empty-array initialization remains the called standard constructors' own behavior; this list counts only explicit allocations visible in these original bodies.
- Null zombie does not bypass these allocations. An allocation/constructor fault takes precedence over the later zombie/buffManager NRE. AddBuff's callback can mutate later resource/camera state. Clip is captured before Camera callbacks; position is captured before AudioVolume; those callbacks do not trigger refetches.

Proposed original logical MethodBody: MaxStack 4, no EH. Reuse Hide V_5, AudioClip V_10, actual Vector3 **V_11**, Single V_8. V_14 is Single and must not be used as a Vector3 address. No local signature retyping is required for these used slots.

```cil
    ldarg.0
    call instance void DeviceEvent::.ctor()
    ldarg.0
    ldarg.1
    stfld Zombie HoleTeleport::zombie
    ldarg.0
    ldarg.2
    stfld int32 HoleTeleport::gridX
    ldarg.0
    ldarg.3
    stfld int32 HoleTeleport::gridY
    ldc.i4.1
    ldarg.1
    newobj instance void Hide::.ctor(bool,Zombie)
    stloc.s V_5
    ldarg.1
    ldfld BuffManager Zombie::buffManager
    ldloc.s V_5
    callvirt instance void BuffManager::AddBuff(Buff)
    ldsfld class List<AudioClip> ResourceManager::zombieClips
    ldc.i4.6
    callvirt instance AudioClip List<AudioClip>::get_Item(int32)
    stloc.s V_10
    call Camera UnityEngine.Camera::get_main()
    callvirt instance Transform UnityEngine.Component::get_transform()
    callvirt instance Vector3 UnityEngine.Transform::get_position()
    stloc.s V_11
    call float32 GlobalStaticVars::AudioVolume()
    ldc.r4 1.2
    mul
    stloc.s V_8
    ldloc.s V_10
    ldloc.s V_11
    ldloc.s V_8
    ldc.r4 1.5
    call AudioSource GlobalStaticVars::CreateAudioAtPoint(AudioClip,Vector3,float32,float32)
    pop
    ret
```

Current-parent existing refs: DeviceEvent ctor `0x06000117`, Hide ctor `0x060000FD`, AddBuff `0x06000102`, List<AudioClip>.Item `0x0A000084`, Camera.main `0x0A000004`, transform `0x0A000001`, position `0x0A000037`, AudioVolume `0x06000138`, CreateAudioAtPoint4 `0x0600013E`. The current signature `0x110000E4` has all used typed slots.

### Current constructor dependency gap

Actual parent82 helper bodies were read directly from its matching archived DLL, not from historical damaged IL. `current-parent-constructors.json` includes their exact bytes and instructions.

- DeviceEvent.ctor is the proper Object-base-call body.
- Hide.ctor has the expected one Buff.ctor call, hide/zombie/duration/type/original stores.
- Buff.ctor allocates AttackRange then List<Buff> and stores the required name/duration fields. Its Object no-op base call is placed before the initializers instead of the original native tail placement; the original mapped base body has no observable effect.
- **AttackRange.ctor remains damaged at source level**: it has the expected Rect/Single/Vector3 list allocation sequence, but ends with diagnostic `ldstr "Method not found @180302170"; pop; ret` and omits the explicit no-op Object constructor call. This is an actual extra string-resolution instruction in a transitive constructor used by proposed newobj Hide, not a tested equivalence result. No verifier/runtime test was run to resolve its effect or constructor validity.

Therefore the original HoleTeleport recipe is fully specified, but **strict current-parent transitive constructor/heap-allocation equivalence cannot be claimed closed**. Retain that dependency discrepancy in Native83 review/acceptance, or handle its bounded constructor repair under separately authorized implementation scope. Do not replace it with a new reasonable-looking initializer chain merely to hide the gap.

## Evidence files and handoff

- `original-source-inspection.json`: original target disassembly, exact fields/types/MethodSpecs, helper constructor disassembly, raw float bytes, pdata flags, independently mapped calls and hashes.
- `current-parent-constructors.json`: actual parent82 DeviceEvent/Hide/Buff/AttackRange/EnemyPath CIL source, parent hash and bytes.
- Companion source-reading scripts remain in this directory; one parser reuses the inspectible original-PE utility definitions from `audit/native80-spec-2026-10-13/inspect_original.py`, whose hash is recorded in the JSON.
- Source specifications are not candidate validity, byte-isolation, CLR/native behavioral acceptance, Unity/IL2CPP success, or iOS/Xcode results. No such checks were executed here.
