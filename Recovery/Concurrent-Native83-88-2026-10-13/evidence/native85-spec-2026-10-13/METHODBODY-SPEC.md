# Native85 source-only specifications

`Zombie.GetDamage` has a complete native-to-CIL source specification using existing parent82 metadata. `Zombie.Shooting` has a complete native behavior specification, with one concrete implementation gap: parent82 has no MethodSpec for `Projectile.Initial<Zombie>`. Existing `Initial<Plant>` is not a substitute. No candidate, test, build, target CIL/native execution, repository edit, commit, or push occurred. All files written by this work are new files in this folder. All acceptance testing belongs to Gemini.

## Source identities and bounds

| Source | SHA-256 |
| --- | --- |
| original `GameAssembly.dll` | `9ebd7ca996a5b03fb4a766f7a2502b660d581ddbbb36af4d7bf2f06a211da39d` |
| original `global-metadata.dat` | `ad992341add498bd1018ac980f42171e0737b0793bf945c8714ad70a445b36b9` |
| native55 roster | `304b632ad13168da91e4b03e2824f69f053c592f59b3a089756a5e4114445277` |
| actual parent82 DLL read from existing archive, without extraction/execution | `48b9e89e941c041d6c11c4064ad05d9384a1a4dd51d5f8d3fad9192cbda5adbf` |
| full Core resolver | `83192116b34abac2bb1797ec0e3943362be33b028b1eefd2e561d5fdc1c3312f` |

The actual parent82 archive member inspected was `candidates/native82/candidate1.dll` in `audit/native82-codex-2026-10-13/godspvz-native78-82-pending-2026-10-13.zip`. `CURRENT-METADATA.json` records actual CLR metadata, field/method access and existing Projectile Initial MethodSpecs. `PRIMARY-EVIDENCE.json` records original PE bytes, registered types, exact switch data, constants, original method definitions/pointers, generic instantiations, and full resolver CIL. Native55 damaged IL was consulted only to locate references/local inventory.

| Original target | Original interval including switch data | Bytes | Native SHA-256 |
| --- | --- | ---: | --- |
| `0x06000443` | `[0x180360D00,0x180360F54)` | 596 | `f4fbb5329fc95c1177751437bc7ef497a91cede600ba47edf2ea343c40cb0908` |
| `0x0600047D` | `[0x180368F60,0x1803691D0)` | 624 | `826866a88d3e12ef1f648a709aebdc1ab394f6852a54df447c38385a93dddcee` |

Both original CSharp pointer entries in `0x181B82D60` and both native slice hashes were matched. The bytes after GetDamage's `0x180360F03` and Shooting's `0x18036918A` are jump-table data/alignment, not managed executable instructions; the native55 disassembler's bad instructions there have no semantic role.

## Actual signatures, types and operands

GetDamage is private nonvirtual instance `Damage Zombie.GetDamage(int32 projectileID, Projectile projectile, int32 specialType)`. Shooting is private nonvirtual instance `void Zombie.Shooting()`.

Damage is a **reference class**, not a struct: usage cell `0x181B9A928` contains `0x200067AD`, kind 1 / type index 13270, registered CLASS (`0x12`) / TypeDef 5125 / `0x02000025`, parent System.Object. Element is also a reference class: cell `0x181B9E168`, `0x20006C4B`, kind 1 / 13861 / CLASS / TypeDef 5130 / `0x0200002B`.

Both damage helpers' first parameter is **`ref Damage`**, a managed reference to the variable containing a class reference. The original registered type is index 10209, pointer `0x181C29410`, bytes `05140000000000000000122000000000`: CLASS data 5125 with type bits `0x20120000`, including byref bit 29. The ordinary Damage class type has bits `0x00120000`. Native `lea rdx,[rsp+0x60]` passes the same local reference slot to both helpers; it does not pass the Damage object by value. The actual parent82 signatures independently retain `Damage&`.

| Original direct call | VA | Access/signature |
| --- | --- | --- |
| Damage `.ctor` `0x06000112` | `0x180311CD0` | public instance void, no arguments |
| Zombie `GetDamageType` `0x06000447` | `0x180360B00` | private instance void `(Damage&,int32,int32)` |
| Zombie `GetDamageRange` `0x06000446` | `0x1803608E0` | private instance void `(Damage&,int32,Projectile,int32)` |
| Zombie `GetATK` `0x06000441` | `0x180360280` | public nonvirtual instance float32, no arguments |
| Zombie `GetMS` `0x0600044B` | `0x180361380` | public nonvirtual instance float32, no arguments |
| Element `.ctor` `0x0600011C` | `0x180316940` | public instance void `(ElementType,float32,Damage,bool shuttle_able)` |
| Damage `AddElement` `0x0600010B` | `0x180310DC0` | public nonvirtual instance void `(Element)` |
| Zombie `ZC_SPHShooting` `0x060004AF` | `0x180372280` | private instance void, no arguments |
| ProjectileManager `CrateNewProjectile` `0x060001F6` | `0x180324150` | public instance Projectile `(int32 ID)`; preserve original spelling |
| GlobalStaticVars `GetAnimationSpritePosition` `0x06000141` | `0x18031B680` | public static Vector3 `(List<GameObject>,string childName)` |
| Projectile `SetDamage` `0x060003F7` | `0x18037C0F0` | public instance void `(Damage,Camp)` |

All CSharp direct mappings above were independently correlated to the original MethodDef metadata and method pointer table. Calls to private Zombie methods and its private animationSprites field are legal from these same-owner method bodies. Current Damage/Element methods are public. Damage fields below are public in both the original registration attributes and actual parent82 CLR metadata.

| Field | Token | Native offset | Type/access |
| --- | --- | ---: | --- |
| Zombie `board` | `0x040005AA` | +0x20 | Board / public |
| Zombie `fZ` | `0x040005AF` | +0x38 | float32 / public |
| Zombie `ID` | `0x040005B9` | +0x60 | int32 / public |
| Zombie `camp` | `0x040005F1` | +0xFC | Camp / public |
| Zombie `animationSprites` | `0x04000609` | +0x1B0 | List<GameObject> / private, same owner |
| Board `projectileManager` | `0x040003D6` | +0xF8 | ProjectileManager / public |
| Damage `damagePoint` | `0x04000123` | +0x10 | float32 / public |
| Damage `camp` | `0x04000124` | +0x14 | Camp / public |
| Damage `sourceZombie` | `0x04000135` | +0x58 | Zombie / public |
| Damage `board` | `0x04000137` | +0x68 | Board / public |

animationSprites is independently decoded as GENERICINST (`0x15`), GenericClass `0x181C45BC0`, definition System.Collections.Generic.List`1 / TypeDef 1272, class-inst argument registered GameObject type 14558 / TypeDef 4078. The deadzone_distance static field sharing numeric offset +0x20 is unrelated to the instance board load.

## 0x06000443 GetDamage — source closure complete

### CFG, local aliasing and callbacks

1. Allocate Damage at `D45`, invoke its no-argument constructor at `D52`, raw-null-check the retained allocated reference at `D57`. Null failure joins `0x180360EFD`. Load `this.camp` **after the constructor returns** (`D60`) and write the initial Damage object's camp (`D6B`).
2. Put that Damage class reference into the local slot at `[rsp+0x60]`. Call GetDamageType (`D83`) then GetDamageRange (`D9F`) with the **same slot address**. Retain original input projectileID, Projectile reference and specialType. No raw or Unity null check of the input Projectile occurs here. Do not check the Damage local between these two calls; the second helper receives any replacement/null written by the first.
3. Call GetATK once (`DA9`) after both helpers return. Capture its float32 result, then load `this.ID` (`DAE`) for the first switch. ID changes made by either helper/GetATK affect this switch. A null Damage local does not prevent GetATK or the selected GetMS branch from running.
4. Apply the exact first-switch operation below. ID 13 calls GetMS exactly once (`DF2`), using its return as the **left operand** of multiplication with captured ATK; then performs a second float32 operation, result + result.
5. Only at `E45` capture the then-current reference from the ref Damage local into RDI. Raw-null-check it (`E4A`), then write damagePoint (`E5A`). This captured object is retained for all later writes, AddElement and the return; do not return the initial allocation or reread the ref slot after callbacks.
6. Reload `this.ID` once (`E74`) **after** point assignment (and Element usage resolution) for a separate condition. If this new ID is 7 or 20 and the original projectileID is signed nonnegative, allocate Element, call `Element((ElementType)0,1000.0f,capturedDamage,true)` (`EAE`), then call capturedDamage.AddElement on that returned Element (`EBC`). The constructor receives the same Damage object as AddElement's receiver. Do not add a check of the Element result absent from the original caller.
7. Write capturedDamage.sourceZombie = this (`EC5`), then reread `this.board` **after AddElement returns** (`ED1`) and write that Board reference into capturedDamage.board (`ED9`). Return capturedDamage (`EE7`). The captured Damage reference survives constructor/AddElement callback mutations, while the board argument is freshly read after them.

The raw-null failure calls original `0x180250150` and raises NullReferenceException. No Unity Object pseudo-null/truth operator occurs in this caller. Null/destroyed input Projectile is handed unchanged to GetDamageRange; helper behavior is not invented. Any callee exception propagates at its original call point. The initial Damage object's camp write is not automatically repeated on a replacement object.

### Exact switch

The original operation is unsigned `(uint32)(ID - 3) <= 19`, then the 20-entry RVA table at `0x180360F04`. This is reproduced by unchecked `sub` followed by CIL `switch`; out-of-range values fall through unchanged.

| First ID snapshot | Exact effect on captured ATK |
| --- | --- |
| 3 | projectileID >= 0: ATK × 12.0f; negative projectileID: unchanged |
| 11 | ATK × 15.0f |
| 13 | round32(GetMS() × ATK), then round32(value + value) |
| 16 | projectileID < 0: ATK × 9.0f; otherwise ATK × 4.5f |
| 17 | projectileID >= 0: ATK × 5.0f; otherwise unchanged |
| 18 | specialType == 0: ATK × 0.5f; specialType == 1: ATK × 8.0f; other values unchanged |
| 4–10, 12, 14–15, 19–22, every other int32 | unchanged |

All comparisons on projectileID/specialType are int32 as described, with signed branch tests for projectileID. The later Element condition is a separate fresh ID snapshot; it is not limited by the first switch branch.

| Constant | Original VA | float32 bits |
| --- | --- | --- |
| 12.0 | `0x1815A7BE4` | `0x41400000` |
| 15.0 | `0x1815A7BE8` | `0x41700000` |
| 9.0 | `0x1815A7D10` | `0x41100000` |
| 4.5 | `0x1815A7C44` | `0x40900000` |
| 5.0 | `0x1815A7BE0` | `0x40A00000` |
| 0.5 | `0x1815A7A08` | `0x3F000000` |
| 8.0 | `0x1815A7B04` | `0x41000000` |
| 1000.0 | `0x1815A7B40` | `0x447A0000` |

For all constant branches the native destination/left operand is ATK, source/right operand is the constant. ID 13 is different: `mulss xmm0,xmm6` means GetMS first, ATK second; `addss xmm6,xmm6` then uses the rounded product twice. Do not commute operands, multiply ATK by `(GetMS*2)`, use one fused expression or promote the sequence to double. The CIL recipe states the same operand order and explicit round-to-float32 points. NaN payload/signaling behavior for a particular CIL compiler/runtime remains an acceptance item for Gemini; static CIL does not establish a portable NaN payload rule.

### Concrete recipe with existing metadata

Preserve local signature `0x1100034B`. Use V_4 Damage as the ref slot, V_6 Single as the arithmetic result, V_7 Damage as the final captured object, V_5 int32 for the first ID snapshot. V_4 and V_7 have distinct purposes. Proposed maxstack 5; no EH. All invoked CSharp methods and fields already exist. Explicit failure can use current NRE constructor MemberRef `0x0A000022` followed by `throw`.

```cil
    newobj instance void Damage::.ctor() // 06000112
    stloc.s 4
    ldloc.s 4
    brfalse FAIL_NULL
    ldloc.s 4
    ldarg.0
    ldfld Camp Zombie::camp
    stfld Camp Damage::camp

    ldarg.0
    ldloca.s 4
    ldarg.1
    ldarg.3
    call instance void Zombie::GetDamageType(Damage&,int32,int32) // 06000447
    ldarg.0
    ldloca.s 4
    ldarg.1
    ldarg.2
    ldarg.3
    call instance void Zombie::GetDamageRange(Damage&,int32,Projectile,int32) // 06000446
    ldarg.0
    call instance float32 Zombie::GetATK() // 06000441
    stloc.s 6
    ldarg.0
    ldfld int32 Zombie::ID
    stloc.s 5
    ldloc.s 5
    ldc.i4.3
    sub
    switch (ID3,COMMON,COMMON,COMMON,COMMON,COMMON,COMMON,COMMON,
            ID11,COMMON,ID13,COMMON,COMMON,ID16,ID17,ID18,
            COMMON,COMMON,COMMON,COMMON)
    br COMMON
ID3:
    ldarg.1
    ldc.i4.0
    blt COMMON
    ldloc.s 6
    ldc.r4 12.0
    mul
    conv.r4
    stloc.s 6
    br COMMON
ID11:
    ldloc.s 6
    ldc.r4 15.0
    mul
    conv.r4
    stloc.s 6
    br COMMON
ID13:
    ldarg.0
    call instance float32 Zombie::GetMS() // 0600044B
    ldloc.s 6
    mul
    conv.r4
    stloc.s 6
    ldloc.s 6
    dup
    add
    conv.r4
    stloc.s 6
    br COMMON
ID16:
    ldarg.1
    ldc.i4.0
    blt ID16_NEGATIVE
    ldloc.s 6
    ldc.r4 4.5
    br MULTIPLY
ID16_NEGATIVE:
    ldloc.s 6
    ldc.r4 9.0
    br MULTIPLY
ID17:
    ldarg.1
    ldc.i4.0
    blt COMMON
    ldloc.s 6
    ldc.r4 5.0
    br MULTIPLY
ID18:
    ldarg.3
    brfalse ID18_ZERO
    ldarg.3
    ldc.i4.1
    bne.un COMMON
    ldloc.s 6
    ldc.r4 8.0
    br MULTIPLY
ID18_ZERO:
    ldloc.s 6
    ldc.r4 0.5
MULTIPLY:
    mul
    conv.r4
    stloc.s 6
COMMON:
    ldloc.s 4
    stloc.s 7
    ldloc.s 7
    brfalse FAIL_NULL
    ldloc.s 7
    ldloc.s 6
    stfld float32 Damage::damagePoint

    ldarg.0
    ldfld int32 Zombie::ID
    stloc.s 5
    ldloc.s 5
    ldc.i4.7
    beq CHECK_PROJECTILE_ID
    ldloc.s 5
    ldc.i4.s 20
    bne.un SOURCE_FIELDS
CHECK_PROJECTILE_ID:
    ldarg.1
    ldc.i4.0
    blt SOURCE_FIELDS
    ldloc.s 7 // captured AddElement receiver retained across Element constructor
    ldc.i4.0 // exact ElementType enum bits
    ldc.r4 1000.0
    ldloc.s 7
    ldc.i4.1
    newobj instance void Element::.ctor(ElementType,float32,Damage,bool) // 0600011C
    call instance void Damage::AddElement(Element) // 0600010B
SOURCE_FIELDS:
    ldloc.s 7
    ldarg.0
    stfld Zombie Damage::sourceZombie
    ldloc.s 7
    ldarg.0
    ldfld Board Zombie::board
    stfld Board Damage::board
    ldloc.s 7
    ret
FAIL_NULL:
    newobj instance void System.NullReferenceException::.ctor() // 0A000022
    throw
```

## 0x0600047D Shooting — behavior closed, exact MethodSpec missing

### Switch and constants

Initialize projectile ID 0 and childName empty string. Load this.ID once, perform unsigned `(uint32)(ID-6)<=16`, then use the 17-entry table at `0x18036918C`:

| ID snapshot | Projectile ID | childName / action |
| --- | ---: | --- |
| 6, 8, 19, 21 | 0 | `PeaShooter_mouth` |
| 7, 20 | 1 | `PeaShooter_mouth` |
| 17 | unused | call `this.ZC_SPHShooting()` once, return immediately |
| every other int32 | 0 | empty string |

The empty string cell `0x181BCF088` is `0xA0000001`, kind 5 / literal 0 / length 0. `0x181BA9DE0` is `0xA0001D09`, kind 5 / literal 3716 / 16 UTF-8 bytes `50656153686f6f7465725f6d6f757468`, exactly `PeaShooter_mouth`. No sprite-name inference is needed.

Shooting speed x comes from `0x1815A7D68`, bytes `33 93 0b c4`, bits **`0xC40B9333`**, float32 exactly `-558.2999877929688`. Speed y/z are positive zero from xorps. The fZ offset at `0x1815A7A40` is float32 120.0, bits `0x42F00000`.

### CFG and capture/exception order

1. On the ordinary path, read `this.board` once (`8FF9`); raw null -> FAIL (`0x180369184`). Read that captured Board.projectileManager once (`901B`); raw null -> FAIL. Destroyed non-null Unity wrappers still reach the calls; no Unity truth operator is used.
2. Call the captured ProjectileManager.CrateNewProjectile(selected int32 ID) once (`9038`); retain its returned Projectile in RBP. **Do not null-check it yet.**
3. Capture `this.animationSprites` after projectile creation (`9047`), before GlobalStaticVars class initialization (`9057`), and pass it together with the previously selected immutable string to GetAnimationSpritePosition (`906A`). Do not reread animationSprites across the static class initialization/call. Do not add a null check of the list.
4. Capture the returned Vector3 by value, all 12 bytes, before calling GetDamage (`9083/9087`). The `movsd` is a raw eight-byte copy of x/y bits, not float64 arithmetic. Call `this.GetDamage(projectileID,capturedProjectile,0)` once (`908A`), retain its returned Damage. The helper receives the created Projectile even if raw null. Preserve the position snapshot across GetDamage.
5. After GetDamage returns, resolve Vector3's usage/static-fields pointer, then raw-null-check the captured Projectile (`90C7`) before reading the actual zeroVector bytes and before reading fZ or calling Initial. If raw null, GetAnimationSpritePosition/GetDamage have already happened; fZ/Initial/SetDamage have not.
6. Copy Vector3.zero's 12 bytes after that check (`90D0/90D9`). Construct speed by value as `(bits 0xC40B9333,+0,+0)`. Read `this.fZ` now (`9100`), add float32 120.0 with **fZ left, constant right** (`9105`), and round as float32. Do not capture fZ before GetDamage.
7. Call typed `capturedProjectile.Initial<Zombie>(capturedStartPosition,speed,zero,adjustedFZ,0,this)` once (`9140`). The movementTracks argument is **int32 zero**, not bool, and the origin is the exact Zombie receiver. All three Vector3 arguments are **by value**; the native x64 ABI temporarily passes addresses of 12-byte copies. They are not managed Vector3& arguments.
8. Read `this.camp` **after Initial returns** (`9145`), and call SetDamage on the original captured Projectile with captured Damage and that fresh Camp (`9155`). Return. Do not reread Board, manager, projectile result or position; do reread Camp after initialization callbacks. Do not null-check the returned Damage in this caller.

ID 17 returns after only its special call; Board/manager/creation/position/damage/Initial/SetDamage are absent. Every other unknown ID executes the default ordinary path rather than returning early. Native failures all join `0x180369184 -> 0x180250150`. Callee exceptions propagate in the order above. No target caller Unity pseudo-null check or EH exists.

### Generic call proof

Cell `0x181BBB820` contains `0xC0024673`, kind 6 / MethodSpec 74553. Its exact record at `0x18175F59C` is `[42770,-1,2547]`, bytes `12a70000fffffffff3090000`. Original method definition 42770 is **Projectile.Initial**, MethodDef `0x060003ED`, public nonvirtual instance generic void with one method type parameter:

```text
Initial<T>(Vector3 startPosition,
           Vector3 speed,
           Vector3 acceleration,
           float32 fZ,
           int32 movementTracks,
           T origin)
```

GenericInst 2547 at `0x1816676B0` has one argument, registered type index 20285 / pointer `0x181BE6580` / CLASS / TypeDef 5219 / **Zombie**, original/current CSharp TypeDef `0x02000084`. The ordinary shared canonical MethodSpec 67190 uses System.Object; GenericMethodFunctionsDefinition row 59269 at `0x1818BEF50` selects method pointer index 59107. Original generic pointer cell `0x1817D3828` contains **`0x1804A25F0`**, precisely the previously unmapped native call. Its exact pdata body `[0x1804A25F0,0x1804A26F1)` SHA-256 is `87254d8ec44c59a00211559213fefcbefdf3ac3523878c13323d06de510f8936`; JSON preserves its disassembly. It copies all three Vector3 values before forwarding to the next generic implementation.

Actual parent82 method definition `0x060003ED` has signature blob `30010601114d114d114d0c081e00`. GenericParam `0x2A000017` is T, index 0, with no type constraints or special constraints, so Zombie is a legal instantiation.

### Public Vector3 helper source proof

Native inline zero accesses registered Vector3 CLASS usage `0x181BC27E8`, `0x20009B91`, type 19912 / VALUETYPE (`0x11`) / TypeDef 4008. Original private static zeroVector FieldDef is `0x040004D0`, static offset 0. Do not access the private field directly from Zombie and do not replace its value with hard-coded zeros: the original reads the stored vector, which must remain observable if instrumented source alters that field.

The actual full resolver Core module was read, not a stub. Public `Vector3.get_zero` MethodDef `0x06001771`, body SHA-256 `cf2dc678197756f5ef7d35ac14df2caa1411e4657123bb6610e321df697afc2d`, contains only `nop; ldsfld Vector3 zeroVector; stloc.0; br.s; ldloc.0; ret`. Its actual raw MethodBody is retained in JSON. Current MemberRef **`0x0A0000FC`** legally returns that same private stored vector by value. Call it after the captured Projectile's raw-null check and before reading fZ.

Full resolver public `Vector3..ctor(float32,float32,float32)` MethodDef `0x06001757`, signature `2003010c0c0c`, body SHA-256 `982df965bdff4fefb59a288e63d3d1b363587e86ed38d8e6d21348ffa1fc35a1`, consists solely of assigning x, y and z from the three arguments, then return. Current MemberRef **`0x0A00009C`** can legally construct the speed value with exact bits; there are no external calls or private-access expansion in that helper.

### Current metadata gap and concrete recipe

The actual current DLL's MethodSpec table was read in memory from the archive and independently hashed. It contains:

- `0x2B000060 -> 0x060003ED`, instantiation blob `0a011281e0`, decoded **Plant**, not Zombie.
- `0x2B000062 -> 0x060003EF`, instantiation blob `0a011e00`, open MVAR 0 for the ten-argument overload; it is not a legal replacement from nongeneric Shooting.
- **No `Initial<Zombie>` MethodSpec.**

A correct implementation requires an exact MethodSpec referring to existing MethodDef `0x060003ED`, instantiated with current Zombie TypeDef `0x02000084`. Its required instantiation blob is **`0a01128210`** (GENERICINST method, one CLASS argument, TypeDefOrRef coded value 0x210). The final MethodSpec token must come from the implementation's actual metadata construction; no token is guessed here. If the parent's selected implementation boundary forbids new metadata rows, **defer Shooting**. Do not reuse Initial<Plant>, call an uninstantiated generic MethodDef, route to the unrelated ten-argument overload, or rewrite Initial's behavior from plausible C#.

Local signature `0x11000377` can be preserved: V_1 object holds only a selected string literal, V_2 bool holds only 0/1 as projectile ID, V_5 Board, V_6 Projectile, V_7 Vector3, V_8 Damage. Existing String TypeRef **`0x0100000B`** provides a legal cast for the V_1 string capture; every reaching assignment is a string literal, so this cast adds no reachable failure. `conv.i4` makes the 0/1 Boolean local explicit int32 when passing it as projectile ID. No new local or hidden ref-to-Vector3 argument is needed. Proposed maxstack 7, no EH; the seven stack items are Initial's receiver plus six arguments.

```cil
    ldc.i4.0
    stloc.2
    ldstr ""
    stloc.1 // object local containing only a selected string literal
    ldarg.0
    ldfld int32 Zombie::ID
    ldc.i4.6
    sub
    switch (MOUTH,MOUTH_ONE,MOUTH,ORDINARY,ORDINARY,ORDINARY,
            ORDINARY,ORDINARY,ORDINARY,ORDINARY,ORDINARY,SPECIAL,
            ORDINARY,MOUTH,MOUTH_ONE,MOUTH,ORDINARY)
    br ORDINARY
MOUTH_ONE:
    ldc.i4.1
    stloc.2
MOUTH:
    ldstr "PeaShooter_mouth"
    stloc.1
    br ORDINARY
SPECIAL:
    ldarg.0
    call instance void Zombie::ZC_SPHShooting() // 060004AF
    ret
ORDINARY:
    ldarg.0
    ldfld Board Zombie::board
    stloc.s 5
    ldloc.s 5
    brfalse FAIL_NULL
    ldloc.s 5
    ldfld ProjectileManager Board::projectileManager
    dup
    brtrue MANAGER_PRESENT
    pop
    br FAIL_NULL
MANAGER_PRESENT:
    ldloc.2
    conv.i4
    call instance Projectile ProjectileManager::CrateNewProjectile(int32) // 060001F6
    stloc.s 6
    ldarg.0
    ldfld System.Collections.Generic.List`1<UnityEngine.GameObject> Zombie::animationSprites
    ldloc.1
    castclass System.String // 0100000B
    call UnityEngine.Vector3 GlobalStaticVars::GetAnimationSpritePosition(List<GameObject>,string) // 06000141
    stloc.s 7
    ldarg.0
    ldloc.2
    conv.i4
    ldloc.s 6
    ldc.i4.0
    call instance Damage Zombie::GetDamage(int32,Projectile,int32) // 06000443
    stloc.s 8
    ldloc.s 6
    dup
    brtrue PROJECTILE_PRESENT
    pop
    br FAIL_NULL
PROJECTILE_PRESENT:
    ldloc.s 7 // by-value snapshot captured before GetDamage
    ldc.r4 <exact bits 0xC40B9333>
    ldc.r4 0.0 // positive zero
    ldc.r4 0.0 // positive zero
    newobj instance void UnityEngine.Vector3::.ctor(float32,float32,float32) // 0A00009C
    call UnityEngine.Vector3 UnityEngine.Vector3::get_zero() // 0A0000FC
    ldarg.0
    ldfld float32 Zombie::fZ
    ldc.r4 120.0
    add
    conv.r4
    ldc.i4.0 // int32 movementTracks
    ldarg.0 // Zombie origin
    call instance void Projectile::Initial<Zombie>(Vector3,Vector3,Vector3,float32,int32,!!0)
         // exact new MethodSpec required; currently absent
    ldloc.s 6
    ldloc.s 8
    ldarg.0
    ldfld Camp Zombie::camp // reread after Initial callback
    call instance void Projectile::SetDamage(Damage,Camp) // 060003F7
    ret
FAIL_NULL:
    newobj instance void System.NullReferenceException::.ctor() // 0A000022
    throw
```

## Explicit limits

GetDamage has no remaining native semantic/reference/access gap in this specification. Shooting's original native generic identity and behavior are resolved; its missing **current MethodSpec row** remains a concrete implementation dependency. Neither method's proposed CodeSize, physical storage capacity, header flags, actual branch encodings, local signature retention, byte isolation or acceptance has been checked. Current old CodeSizes are 129 and 219; those figures alone do not prove the reserved storage capacities. All compiler/verifier/runtime/native fixture/NaN/alias/ref/malformed/determinism/isolation testing remains with Gemini. No acceptance or full IL2CPP/iOS outcome is claimed.
