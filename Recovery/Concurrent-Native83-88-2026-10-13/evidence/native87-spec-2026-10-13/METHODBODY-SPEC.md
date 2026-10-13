# Native87 source-only MethodBody specifications

Both original caller MethodBodies are resolved to concrete CIL recipes below. All original input bytes, native switch data, signatures, registered field/generic types, and the necessary public helper contracts were inspected statically. No test, build, target CIL/native execution, candidate generation, repository edit, commit, or push occurred. Only new files in this folder were written. Testing belongs to Gemini.

One pre-existing dependency difference is recorded separately in `HELPER-TIMING-DIFFERENCE.md`: current `GlobalStaticVars.CreateAudioAtPoint` reads clip.length after Time.timeScale, while the original PC helper reads it before. Correct caller reconstruction does not qualify that helper or establish whole-program native equivalence.

## Sources and exact original bounds

| Input/source | SHA-256 |
| --- | --- |
| original PC GameAssembly | `9ebd7ca996a5b03fb4a766f7a2502b660d581ddbbb36af4d7bf2f06a211da39d` |
| original v31 metadata | `ad992341add498bd1018ac980f42171e0737b0793bf945c8714ad70a445b36b9` |
| native55 roster | `304b632ad13168da91e4b03e2824f69f053c592f59b3a089756a5e4114445277` |
| actual parent82 DLL, read directly from its existing archive without extraction/execution | `48b9e89e941c041d6c11c4064ad05d9384a1a4dd51d5f8d3fad9192cbda5adbf` |
| full Core resolver and exact-editor Core resolver, identical bytes | `83192116b34abac2bb1797ec0e3943362be33b028b1eefd2e561d5fdc1c3312f` |

| Target | Original interval | Bytes | SHA-256 |
| --- | --- | ---: | --- |
| `0x06000199` PlayAudio | `[0x1803142E0,0x180314558)` | 632 | `52437dcecdcfd8cd9c6e7f0ebe3c2f05a7f382bddd0ab23955f619d2fb66f093` |
| `0x06000386` PC_PotatoBoom | `[0x180353A90,0x180353D31)` | 673 | `1bb0e885ee1fcb262f5c65012609cb34296a790dc198462b16b45bfe58b0d925` |

Both CSharp pointer entries in original table `0x181B82D60` and slice hashes were matched. PlayAudio executable instructions end at `0x180314538`; the bytes at `0x18031453C` are the seven-entry switch table, not instructions. Native55 damaged IL supplied reference/local leads only. `PRIMARY-EVIDENCE.json` contains the independent original evidence; `MANAGED-SOURCE-EVIDENCE.json` contains actual current and full resolver CIL.

## Exact enum, list and shared generic identities

PlayAudio is public nonvirtual instance `void DialogueManager_OnBoard.PlayAudio(DialogueAudioType dialogueAudioType)`. The original parameter is registered VALUETYPE type 13665 / TypeDef 5153 / CSharp TypeDef `0x02000042`. Its enum element type is registered type 15448, System.Int32. Original v31 field defaults contain compressed I4 bytes `00 02 04 06 08 0A 0C` at consecutive data indices 237817–237823; their 0–6 values independently match actual parent82 CLR Constant rows:

| Bits | Exact original/current name |
| ---: | --- |
| 0 | None |
| 1 | RadioSignal |
| 2 | WabbyWabbo0 |
| 3 | WabbyWabbo1 |
| 4 | WabbyWabbo2 |
| 5 | WabbyWabbo3 |
| 6 | WabbyWabbo4 |

Both ResourceManager fields are **List<AudioClip>**, not AudioClip arrays:

| Field | FieldDef | Static offset | Original type/access |
| --- | --- | ---: | --- |
| `dialogueClips` | `0x040002F3` | +0x140 | registered type 7534 / GENERICINST (`0x15`), public static |
| `particleClips` | `0x040002F1` | +0x130 | same List<AudioClip> closure, public static |

Original GenericClass `0x181C415D0` uses List`1 definition registered type 15897 / System.Collections.Generic.List`1 / TypeDef 1272. Its class-inst argument is AudioClip type 12146 / TypeDef 7547 / original AudioModule TypeDef `0x02000004`.

Usage cell `0x181BA4278` contains `0xC0023075`, kind 6 / MethodSpec 71738, record `[9164,2364,-1]` at `0x1817571A8`. This is original mscorlib MethodDef `0x060023CD`, **List`1.get_Item(int32)**, class-inst 2364 / AudioClip, no method type arguments. Return kind VAR (`0x13`) is class parameter !0, resolved to AudioClip; it is not an uninstantiated method-generic !!0.

The canonical List<Object> record, MethodSpec 34164 / generic row 30196 at `0x18184D640`, selects pointer index 30069 / cell `0x18179ACB8` -> **`0x180825710`**, precisely both native get_Item calls. This body compares the index unsigned against List._size before reading _items; invalid index calls `0x180CCA8D0`, independently mapped to original mscorlib `ThrowArgumentOutOfRange_IndexException` M`0x06000981`. Only after that range check does it load the backing array and perform its own raw-null/array-bounds checks. Preserve the public list getter; do not replace it with direct array access or a caller Count check.

Full resolver mscorlib List.get_Item M`0x0600681F`, signature `2001130008`, body SHA-256 `c68a35b843104f0c39d5b17964698c104a790ce9acd1cdc33ac43ad973c36f57`, retains that unsigned size guard, ThrowHelper call and typed ldelem !0 sequence. Current closed MemberRef is **`0x0A000084`**, declaring TypeSpec **`0x1B00001A`**, signature `15120d011280b5` / List<AudioClip>. Current AudioClip TypeRef is **`0x0100002D`**.

Other usage cells resolve independently to GlobalStaticVars (`0x181BA2018`: `0x2000720F`, type 14599), ResourceManager (`0x181BB6478`: `0x200089EB`, type 17653), Unity Object (`0x181BB0500`: `0x200081D1`, type 16616), and ParticlesManager (`0x181BB1890`: `0x20008355`, type 16810). The helper's GameObject name cell `0x181BA7408` is tagged literal `0xA0001C21`, literal 3600, exactly 14 UTF-8 bytes for `One shot audio`; neither caller invents a new string.

## Public/private helper proof and Editor/player distinction

Original player public `Random.Range(int32,int32)` M`0x0600079C` and private `Random.RandomRangeInt(int32,int32)` M`0x0600079D` share the exact native pointer **`0x18130D310`**. The bounded body hashes to `e189740a5a5fb83af8d743e306d69663b9e2ebcd21eccaf7df58c2a03e6d44e5` and forwards ECX then EDX unchanged to the resolved icall named `UnityEngine.Random::RandomRangeInt(System.Int32,System.Int32)` at original name pointer `0x181621750`.

The actual full and exact-editor Core DLLs are byte-identical. Their public Range(int,int) M`0x06001A26` has body SHA-256 `87352b91e2e063e9b6ac75d7898acef45631b595e13f83ddfee7de41116173dd`: `nop; ldarg.0; ldarg.1; call private RandomRangeInt; stloc.0; br; ldloc.0; ret`. Private RandomRangeInt M`0x06001A27` is an internalcall with no CIL RVA. This proves public **current MemberRef `0x0A00012B`** is a legal exact wrapper for the original private/shared operation. Do not emit an illegal external private RandomRangeInt reference. Original float Range M`0x0600079B` maps to `0x18130D350`; current public float Range MemberRef is `0x0A000083`, full resolver M`0x06001A25`, internalcall.

Original ParticlesManager private static `<instance>k__BackingField` F`0x04000278`, static offset 0, has attributes 0x11 / private static. A direct Plant access is illegal managed member access. Original public static getter M**`0x060001E5`** at **`0x180322500`** resolves the same ParticlesManager usage cell and returns static-fields slot 0, with no Unity truth conversion, fallback search or field write. Native body SHA-256 is `c63714142677041824a496862a9c99aff2cfebafaa8cf4e2268c00e933d769ea`. Actual parent82 getter CIL body SHA-256 is `c233b6a3eaf7df384af439528eae7e4d73f5951291e5e934edc6a3cff2a2316c`: `br IL_0005; ldsfld same F04000278; ret`. Calling this public getter exactly represents Plant's private inline load. No private access expansion or invented singleton fallback is needed.

Full and exact-editor Core `Vector3..ctor(float32,float32,float32)` bodies are also retained. Public M`0x06001757`, current MemberRef **`0x0A00009C`**, has body SHA-256 `982df965bdff4fefb59a288e63d3d1b363587e86ed38d8e6d21348ffa1fc35a1` and only writes x, y, z from its arguments. It legally represents the inline scale-vector construction.

The inspected getter, AudioVolume and audio helper contain no Editor API/Application.isEditor branch. Player MethodDef tokens differ from the full Editor/resolver tokens because the original player metadata is stripped; public wrapper semantics above agree. No native RNG sequence, audio playback or Editor/iOS engine equivalence was executed or claimed. The separate length/timeScale ordering difference is a recovered-helper fidelity issue, not a proven intended Editor-specific feature.

## 0x06000199 PlayAudio

### Exact CFG, captures and exceptions

1. Call `GlobalStaticVars.AudioVolume()` once at `0x18031434A` **before** any enum selection/list lookup, capture the returned float32 volume in XMM7. There is no instance receiver field access in this method; do not add an entry check of this or consult unrelated dialogue state.
2. Start selected clip at raw null. The switch is an unsigned comparison of the argument bits against 6, then the seven-entry table at `0x18031453C`. Values 0/1 and every other int32 outside 0–6 retain null and still proceed through the later Unity inequality call.
3. For value 2, read ResourceManager.dialogueClips after its class initialization, raw-null-check it immediately (`439A`), and call typed get_Item(0) (`4471`). No Random call occurs.
4. For values 3–6, read/capture the then-current dialogueClips list (`4450`) **before Random.Range**, call Random.Range with the exact argument pair below (`4457`), and only then raw-null-check the captured list (`445C`). A Random callback may replace the static field; use the captured list. A null captured list still causes the Random call first; a Random exception wins over the later NRE. Call get_Item with the returned int32 unchanged. Do not repair, offset, clamp or normalize the reversed bounds/index.
5. Retain the returned AudioClip, then call Unity `Object.op_Inequality(capturedClip,null)` once (`4496`) for every enum path. False -> return without Camera or audio creation. This is the only caller Unity pseudo-null decision; a destroyed AudioClip is false here.
6. True -> call `Camera.main` once (`44A5`), raw-null-check its captured return (`44AA`), call Component.get_transform on that camera (`44B8`), raw-null-check its return (`44BD`), then get_position once (`44D2`). A destroyed non-null camera/transform reaches the appropriate getter, preserving its callee behavior; no Unity truth operator is substituted.
7. Capture the resulting Vector3 by value, all 12 bytes, before any subsequent helper call/class initialization. Call `CreateAudioAtPoint(capturedClip,capturedCameraPosition,capturedVolume,1.0f)` once (`4519`), ignore its AudioSource return, then return. No list, clip, volume, Camera.main or position reread occurs across later callbacks.

| Enum | Exact lookup |
| --- | --- |
| None / 0, RadioSignal / 1, out-of-range bit patterns | selected clip stays null; no list read/Random/get_Item; Unity inequality still called |
| WabbyWabbo0 / 2 | dialogueClips[0] |
| WabbyWabbo1 / 3 | captured dialogueClips[Random.Range(1,3)] |
| WabbyWabbo2 / 4 | captured dialogueClips[Random.Range(4,3)] |
| WabbyWabbo3 / 5 | captured dialogueClips[Random.Range(7,2)] |
| WabbyWabbo4 / 6 | captured dialogueClips[Random.Range(9,3)] |

The tuples above are the original ECX/EDX arguments. Do not reinterpret the second argument as a count, add it to the first, or use an inferred intended range. Actual engine-selected values are the engine's responsibility. List.get_Item applies its own range/array exceptions after the caller raw-null timing described above. All explicit caller raw-null failures join `0x180314533 -> 0x180250150` / NullReferenceException. There is no EH.

The volume is passed unchanged, including NaN, infinities and signed zero. Pitch is exactly float32 **1.0**, original `0x1815A7A10`, bits `0x3F800000`. Vector3 loads/copies preserve component bits; native movsd copies two float32 fields as raw bytes and is not float64 arithmetic.

### Concrete CIL recipe and locals

Preserve local signature `0x11000141`, proposed maxstack 4, no EH. Available slots: V_1 Single volume; V_3 object clip capture; V_11 object List capture; V_7 Camera; V_9 Transform; V_10 Vector3. V_0 DialogueAudioType can store the returned int32 index because its reduced enum storage type is I4; explicitly `conv.i4` on its use as an int32 argument. Existing typed references for casts are AudioClip `0x0100002D` and List<AudioClip> `0x1B00001A`. The object slots receive only those typed values/null, so the casts introduce no failure for legal typed input/helper returns. If the implementation prefers dedicated typed locals, append a new target-specific local signature rather than modifying a shared pre-existing row.

```cil
    call float32 GlobalStaticVars::AudioVolume() // 06000138
    stloc.1
    ldnull
    stloc.3
    ldarg.1
    switch (CHECK_CLIP,CHECK_CLIP,INDEX_ZERO,RANGE_1_3,RANGE_4_3,RANGE_7_2,RANGE_9_3)
    br CHECK_CLIP
INDEX_ZERO:
    ldsfld List<AudioClip> ResourceManager::dialogueClips // 040002F3
    stloc.s 11
    ldc.i4.0
    stloc.0
    br LIST_CHECK
RANGE_1_3:
    ldsfld List<AudioClip> ResourceManager::dialogueClips
    stloc.s 11
    ldc.i4.1
    ldc.i4.3
    br RANDOM_CALL
RANGE_4_3:
    ldsfld List<AudioClip> ResourceManager::dialogueClips
    stloc.s 11
    ldc.i4.4
    ldc.i4.3
    br RANDOM_CALL
RANGE_7_2:
    ldsfld List<AudioClip> ResourceManager::dialogueClips
    stloc.s 11
    ldc.i4.7
    ldc.i4.2
    br RANDOM_CALL
RANGE_9_3:
    ldsfld List<AudioClip> ResourceManager::dialogueClips
    stloc.s 11
    ldc.i4.s 9
    ldc.i4.3
RANDOM_CALL:
    call int32 UnityEngine.Random::Range(int32,int32) // 0A00012B
    stloc.0
LIST_CHECK:
    ldloc.s 11
    castclass List<AudioClip> // 1B00001A
    dup
    brtrue LIST_PRESENT
    pop
    br FAIL_NULL
LIST_PRESENT:
    ldloc.0
    conv.i4
    callvirt instance !0 List<AudioClip>::get_Item(int32) // 0A000084
    stloc.3
CHECK_CLIP:
    ldloc.3
    castclass UnityEngine.AudioClip // 0100002D
    ldnull
    call bool UnityEngine.Object::op_Inequality(UnityEngine.Object,UnityEngine.Object) // 0A00000C
    brfalse RETURN
    call UnityEngine.Camera UnityEngine.Camera::get_main() // 0A000004
    stloc.s 7
    ldloc.s 7
    brfalse FAIL_NULL
    ldloc.s 7
    call instance UnityEngine.Transform UnityEngine.Component::get_transform() // 0A000001
    stloc.s 9
    ldloc.s 9
    brfalse FAIL_NULL
    ldloc.s 9
    call instance UnityEngine.Vector3 UnityEngine.Transform::get_position() // 0A000037
    stloc.s 10
    ldloc.3
    castclass UnityEngine.AudioClip
    ldloc.s 10
    ldloc.1
    ldc.r4 1.0
    call UnityEngine.AudioSource GlobalStaticVars::CreateAudioAtPoint(AudioClip,Vector3,float32,float32) // 0600013E
    pop
RETURN:
    ret
FAIL_NULL:
    newobj instance void System.NullReferenceException::.ctor() // 0A000022
    throw
```

## 0x06000386 Plant.PC_PotatoBoom

Original signature is private nonvirtual instance void, no parameters. Exact sequence:

1. Call `this.GetDamage(null,-1,0)` (`3AF4`), retain the returned Damage, raw-null-check it (`3AF9`), call AreaDamage once (`3B07`), and **discard** its Boolean result. True/false both proceed; no new condition is invented.
2. Reread `this.board` after AreaDamage (`3B0C`), raw-null-check it (`3B13`), then call `Shake(time:0.08f,amplitude:5.0f)` (`3B2F`), preserving the original metadata parameter names and order. Source board FieldDef is `0x040004F3`, offset +0x1E8, public. A callback swap is used; a callback null produces NRE here.
3. Read ParticlesManager's current backing slot after Shake through the proven public pure getter; raw-null-check it (`3B61`). Call `CreatNewParticle((ParticleState)23)` once (`3B72`), retain returned GameObject, raw-null-check it (`3B7A`). Actual CLR Constant and original enum contract name this value **ParticleState.PotatoBoom**.
4. Call the created GameObject.get_transform once (`3B88`) and capture its returned effect Transform in RSI, **without checking it yet**. Independently call `this.get_transform` (`3B95`), raw-null-check the plant Transform (`3B9A`), then get its position (`3BAE`). Only after that call returns raw-null-check the captured effect Transform (`3BB3`). Thus an effect-transform null does not suppress plant-transform/position calls, and a plant-transform failure takes precedence.
5. Copy the plant position Vector3 by value and call set_position on the captured first effect Transform (`3BD8`). A callback may alter the particle object's transform association or plant fields.
6. Call the **same captured particle GameObject's get_transform again** (`3BE2`) after SetPosition. Raw-null-check this new return (`3BEF`); construct scale `(1.2f,1.2f,1.2f)` by value and call set_localScale on this newly returned Transform (`3C15`). Do not reuse the first Transform or compare either through Unity pseudo-null semantics.
7. After SetLocalScale, load ResourceManager.particleClips (`3C40`), raw-null-check the captured List (`3C47`), call typed get_Item(0) (`3C59`), and capture its AudioClip. **Do not null/pseudo-null-check the AudioClip in this caller.**
8. Call Camera.main once (`3C63`), raw-null-check it (`3C68`), get_transform (`3C76`), raw-null-check that return (`3C7B`), then get_position (`3C99`), capture all three component bits by value.
9. Call AudioVolume once **after position capture** (`3CBC`), capture the Single return. Call public Random.Range(float32 0.9,float32 1.1) once **after AudioVolume** (`3CD7`), capture the Single pitch. Preserve that order and operand order; do not cache either before particle operations/camera position.
10. Call `CreateAudioAtPoint(capturedClip,capturedCameraPosition,capturedVolume,capturedPitch)` once (`3CFD`), discard its AudioSource result, then call `this.PlantDie()` exactly once (`3D26` tail transfer), return. PlantDie still occurs if the audio helper returns null, including its own null/destroyed clip early return. Every earlier exception skips PlantDie.

All explicit caller raw-null failures join `0x180353D2B -> 0x180250150`. No Unity truth or equality operator is used anywhere in this caller, and no EH exists. A non-null destroyed Board/manager/GameObject/Transform/Camera proceeds to its relevant callee, whose exception is retained. A null clip does not skip the camera, volume or Random calls. Captured particle/Transform/clip/position/volume identities survive later callbacks; the field reads described as fresh must remain fresh.

| Constant | Original VA | Exact float32 bits/value |
| --- | --- | --- |
| Shake time | `0x1815A7C04` | `0x3DA3D70A`, 0.07999999821186066 |
| Shake amplitude | `0x1815A7BE0` | `0x40A00000`, 5.0 |
| Scale x/y/z | `0x1815A7AC8` | `0x3F99999A`, 1.2000000476837158 |
| Random min | `0x1815A7AC4` | `0x3F666666`, 0.8999999761581421 |
| Random max | `0x1815A7A14` | `0x3F8CCCCD`, 1.100000023841858 |

There is no caller arithmetic on volume/pitch/position. Preserve the Single results and Vector3 component bits, including NaNs/infinities/signed zero; do not approximate, clamp, normalize or multiply the passed volume/pitch in these caller bodies.

### Concrete CIL recipe

Preserve local signature `0x110002C5`, proposed maxstack 4, no EH. Use V_1 Damage, V_3 GameObject, V_4 first effect Transform, V_6 plant Transform, V_5 position, V_7 Camera, V_8 camera Transform, V_11 AudioClip, V_12 camera position, V_13 Single volume, V_16 Single pitch. The second effect Transform can remain checked on the evaluation stack. No new metadata/local is required; public getter M`0x060001E5` and constructor MemberRef `0x0A00009C` already exist.

```cil
    ldarg.0
    ldnull
    ldc.i4.m1
    ldc.i4.0
    call instance Damage Plant::GetDamage(Projectile,int32,int32) // 0600036D
    stloc.1
    ldloc.1
    brfalse FAIL_NULL
    ldloc.1
    call instance bool Damage::AreaDamage() // 0600010C
    pop
    ldarg.0
    ldfld Board Plant::board // 040004F3
    dup
    brtrue BOARD_PRESENT
    pop
    br FAIL_NULL
BOARD_PRESENT:
    ldc.r4 <bits 0x3DA3D70A>
    ldc.r4 5.0
    call instance void Board::Shake(float32,float32) // 060002CD
    call ParticlesManager ParticlesManager::get_instance() // 060001E5, proven pure getter
    dup
    brtrue MANAGER_PRESENT
    pop
    br FAIL_NULL
MANAGER_PRESENT:
    ldc.i4.s 23
    call instance UnityEngine.GameObject ParticlesManager::CreatNewParticle(ParticleState) // 060001EA
    stloc.3
    ldloc.3
    brfalse FAIL_NULL
    ldloc.3
    call instance UnityEngine.Transform UnityEngine.GameObject::get_transform() // 0A00002E
    stloc.s 4 // intentionally no raw check yet
    ldarg.0
    call instance UnityEngine.Transform UnityEngine.Component::get_transform() // 0A000001
    stloc.s 6
    ldloc.s 6
    brfalse FAIL_NULL
    ldloc.s 6
    call instance UnityEngine.Vector3 UnityEngine.Transform::get_position() // 0A000037
    stloc.s 5
    ldloc.s 4
    brfalse FAIL_NULL
    ldloc.s 4
    ldloc.s 5
    call instance void UnityEngine.Transform::set_position(UnityEngine.Vector3) // 0A000038
    ldloc.3
    call instance UnityEngine.Transform UnityEngine.GameObject::get_transform() // second getter
    dup
    brtrue SCALE_TARGET_PRESENT
    pop
    br FAIL_NULL
SCALE_TARGET_PRESENT:
    ldc.r4 <bits 0x3F99999A>
    ldc.r4 <bits 0x3F99999A>
    ldc.r4 <bits 0x3F99999A>
    newobj instance void UnityEngine.Vector3::.ctor(float32,float32,float32) // 0A00009C, pure source body proved
    call instance void UnityEngine.Transform::set_localScale(UnityEngine.Vector3) // 0A0000E5
    ldsfld List<AudioClip> ResourceManager::particleClips // 040002F1
    dup
    brtrue LIST_PRESENT
    pop
    br FAIL_NULL
LIST_PRESENT:
    ldc.i4.0
    callvirt instance !0 List<AudioClip>::get_Item(int32) // 0A000084
    stloc.s 11
    call UnityEngine.Camera UnityEngine.Camera::get_main() // 0A000004
    stloc.s 7
    ldloc.s 7
    brfalse FAIL_NULL
    ldloc.s 7
    call instance UnityEngine.Transform UnityEngine.Component::get_transform()
    stloc.s 8
    ldloc.s 8
    brfalse FAIL_NULL
    ldloc.s 8
    call instance UnityEngine.Vector3 UnityEngine.Transform::get_position()
    stloc.s 12
    call float32 GlobalStaticVars::AudioVolume() // 06000138
    stloc.s 13
    ldc.r4 <bits 0x3F666666>
    ldc.r4 <bits 0x3F8CCCCD>
    call float32 UnityEngine.Random::Range(float32,float32) // 0A000083
    stloc.s 16
    ldloc.s 11
    ldloc.s 12
    ldloc.s 13
    ldloc.s 16
    call UnityEngine.AudioSource GlobalStaticVars::CreateAudioAtPoint(AudioClip,Vector3,float32,float32) // 0600013E
    pop
    ldarg.0
    call instance void Plant::PlantDie() // 06000397
    ret
FAIL_NULL:
    newobj instance void System.NullReferenceException::.ctor() // 0A000022
    throw
```

## Limits and handoff

Native87 has no unresolved native caller CFG, signature, enum/List closure, constant, legal public substitute, or current operand identity. Manual stack accounting yields maxstack 4 for both recipes. Actual CodeSize/storage capacity, physical branch/header/local tokens, byte isolation, typed validation, callback/exception fixtures, malformed controls, RNG/audio engine behavior and platform acceptance have not been tested. All such work is Gemini's responsibility.

Current old CodeSizes are PlayAudio 209 and PotatoBoom 339; these are not measured reserved capacities. If the implementation chooses new typed locals, scope their new StandAloneSig row to this target rather than mutating shared signatures. Keep the documented pre-existing M`0x0600013E` timing difference distinct from caller implementation success; a separate helper repair is a parent scope decision.
