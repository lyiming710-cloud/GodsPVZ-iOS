# Native78 source-only MethodBody specification

Prepared from original PE/metadata source inspection on 2026-10-13. This folder contains analysis and source-reading scripts only. No tests, builds, CLR/native runtime execution, candidates, edits to pre-existing project files, commits, or pushes were performed. All acceptability testing remains with Gemini.

## Source identity and scope

- Original `audit/stage9-native3/inputs/GameAssembly.dll`: SHA256 `9ebd7ca996a5b03fb4a766f7a2502b660d581ddbbb36af4d7bf2f06a211da39d`.
- Original `audit/stage9-native3/inputs/global-metadata.dat`: SHA256 `ad992341add498bd1018ac980f42171e0737b0793bf945c8714ad70a445b36b9`.
- Native records: `audit/native30-continuation-2026-10-02/native55-native.json`. Its damaged managed instructions were consulted only to discover references, never as semantic authority. The facts below were independently traced to the original PE and metadata tables.
- Field/type/map records: `audit/stage9-native3/evidence/native-fields.json`, `native-types.json`, `native-method-map.json`.
- Native `0x0600061D`: `0x18039D2B0..0x18039D41D`, 365 bytes, SHA256 `bbe5a81e0f05e8fad1a2ba41299082700136338fae68c459bd8409afc9b74492` read again from the original PE.
- Native `0x06000483`: `0x18036A460..0x18036A5DE`, 382 bytes, SHA256 `f026bb7413282bc7832e19d52fd54c86253936ef6b239bb23add45d09d3c0f85` read again from the original PE.

The CIL recipes below specify operand identities and control flow; they do not generate candidate bytes. The current parent77 metadata/RVAs/reserved slots must supply the final concrete MethodRef/TypeRef/UserString/local-signature tokens. Proposed `.maxstack` values follow direct stack accounting, not a verifier result.

## Independent usage-cell decoding

The original metadata registration is at `0x1818C6D00`. The repeated type-definition-count/FieldOffsets pair is at `0x1818C6D50`, pointing to FieldOffsets `0x181B5E000`. Relative to that pair, TypesCount is at `0x1818C6D30` and Types is at `0x1818C6D38`: 27,557 entries, pointer table `0x1818EDEB0`.

The original initializer chain is `0x18024FEF0 -> 0x180298710 -> 0x180232F00`. At `0x180232F3E` it computes the usage kind by shifting 29; at `0x180232F43`/`F45` it computes the source index by shifting 1 and masking `0x0FFFFFFF`. Its jump table at `0x180233100` sends kind 1 to the class-resolution arm `0x180232F71`, kind 2 to the registration Types-table arm `0x180232F84`, and kind 5 to the string-literal arm `0x180232FB4`.

| Usage cell | Original 8 bytes | Encoded value | Kind/index | Original resolution |
|---|---|---|---|---|
| `0x181B9E7A8` | `1b73004000000000` | `0x4000731B` | kind 2 / 14733 | Types slot `0x18190AB18` -> Il2CppType `0x181C4B160`, bytes `40140000000000000000118000000000`; VALUETYPE / typeDefinition 5184 / `GridState`, Assembly-CSharp TypeDef `0x02000061` |
| `0x181BA95A8` | `b178002000000000` | `0x200078B1` | kind 1 / 15448 | Types slot `0x18190C170` -> Il2CppType `0x181BD6D80`, bytes `c6000000000000000000088000000000`; I4 / typeDefinition 198 / `System.Int32` |
| `0x181BA4EC8` | `054600a000000000` | `0xA0004605` | kind 5 / 8962 | String record `0x11910`, data `0x49E82`, 36 UTF-8 bytes: `此序号对应格子类型不存在` |
| `0x181BBAB00` | `7d3800a000000000` | `0xA000387D` | kind 5 / 7230 | String record `0xE2F0`, data `0x43E68`, 11 UTF-8 bytes: `isAttacking` |
| `0x181BC27E8` | `919b002000000000` | `0x20009B91` | kind 1 / 19912 | Vector3 typeDefinition 4008, Unity Core TypeDef `0x0200010E` in original stripped PC metadata |

GridState's original `elementTypeIndex` is 15448, its underlying System.Int32 type. Its `value__` field uses type index 15470, also I4. The enum and boxed primitive are therefore different operands: `ldtoken GridState`; `box System.Int32`.

The call at `0x18039D34D` was independently mapped from the original `mscorlib.dll` CodeGenModule (`0x1819412E0`, 9,995 pointers at `0x181C4E110`) to `System.Type::GetTypeFromHandle(System.RuntimeTypeHandle)`, original mscorlib MethodDef `0x060008AD`, address `0x180CCEE30`. The call at `0x18039D38A` maps to `System.Enum::IsDefined(System.Type,System.Object)`, original MethodDef `0x06000AF2`, address `0x180CDDFE0`.

## 0x0600061D GridConfig_A.SetGridState(int32)

Native contract:

1. Start with result state zero. Obtain `typeof(GridState)` exactly once, box the input as `System.Int32`, and call `Enum.IsDefined(Type,Object)` exactly once.
2. When defined, preserve the original input int32 bits as the result state. When undefined, call `Debug.Log(Object)` once with the exact string above and retain zero. There is no early return on an invalid ID.
3. Read `this.system` at `0x18039D3C0` **after** enum validation and any logging callback. Compare that read against null through `UnityEngine.Object::op_Inequality(Object,Object)` at `0x18039D3DA`.
4. If Unity inequality returns false, return. If true, read `this.system` **again** at `0x18039D3E3`. A raw null on this second read branches to `0x18039D417 -> 0x180250150`, which raises System.NullReferenceException. A non-null receiver calls `System2.SetGridState(GridState,int32,int32)` once at `0x18039D3FF`, using result state, `this.gridX`, and `this.gridY` read after the comparison.
5. This method does not assign `GridConfig_A.gridState` or any other GridConfig_A field.

Fields: system `0x0400081D` / +0x38 / System2; gridX `0x04000818` / +0x20 / I4; gridY `0x04000819` / +0x24 / I4. The native callee is Assembly-CSharp MethodDef `0x06000093`, address `0x18030C720`; original metadata flags `0x86` make it a public nonvirtual instance method.

Proposed exact logical MethodBody: `.maxstack 4`, one used local of `System.Int32`, no EH. The parent may preserve its existing local signature if a verified int32 slot is available; the body initializes the used slot explicitly and needs no other local. `FAIL_NULL` is an exception branch, not a defensive null skip.

```cil
    ldc.i4.0
    stloc.0
    ldtoken GridState
    call System.Type System.Type::GetTypeFromHandle(System.RuntimeTypeHandle)
    ldarg.1
    box System.Int32
    call bool System.Enum::IsDefined(System.Type, object)
    brfalse INVALID_ID
    ldarg.1
    stloc.0
    br CHECK_SYSTEM
INVALID_ID:
    ldstr "此序号对应格子类型不存在"
    call void UnityEngine.Debug::Log(object)
CHECK_SYSTEM:
    ldarg.0
    ldfld System2 GridConfig_A::system
    ldnull
    call bool UnityEngine.Object::op_Inequality(UnityEngine.Object, UnityEngine.Object)
    brfalse RETURN
    ldarg.0
    ldfld System2 GridConfig_A::system
    dup
    brtrue RECEIVER_READY
    pop
    newobj instance void System.NullReferenceException::.ctor()
    throw
RECEIVER_READY:
    ldloc.0
    ldarg.0
    ldfld int32 GridConfig_A::gridX
    ldarg.0
    ldfld int32 GridConfig_A::gridY
    call instance void System2::SetGridState(GridState, int32, int32)
RETURN:
    ret
```

The native machine reads gridY before gridX while arranging registers, with no call/write/volatile access between them. Both values are read after the second system read/raw-null guard. The recipe uses signature order gridX,gridY; it preserves all values and observable callback boundaries. If a specification insists on native register-load order as an additional mechanical requirement, a verified extra int32 local can capture gridY first without altering the contract.

## 0x06000483 Zombie.TranToAttack(Plant)

Native contract:

1. Read `this.targetPlant` at `0x18036A4A4` and compare it with the preserved input `newTargetPlant` via Unity inequality at `0x18036A4C2`. When false, jump straight to the common tail at `0x18036A55A`.
2. When true, re-read `this.targetPlant` at `0x18036A4D6` and compare that read to null via Unity inequality at `0x18036A4F3`. When false, skip only the old-plant callback.
3. When the old-plant comparison is true, re-read `this.targetPlant` **again** at `0x18036A4FC`. Raw null raises NRE at `0x18036A5D8`; otherwise call this third receiver's `Plant.BlockZombie(this,false)` at `0x18036A515`. Do not cache the first/second target read for this call.
4. After the old callback (or its skip), compare the preserved input plant against null through Unity inequality at `0x18036A537`. If true, raw null raises NRE; otherwise call the preserved input plant's `BlockZombie(this,true)` at `0x18036A555`.
5. Common tail: read animator at `0x18036A55A`; raw null raises NRE. Call `Animator.SetBool("isAttacking",true)` at `0x18036A573`. Call `this.ResetAttackSpeed()` at `0x18036A57D`. Then copy Vector3.zeroVector to rSpeed at `0x18036A5B8/5C0`, and finally write `isEat=true` at `0x18036A5C6`.
6. **No assignment to `targetPlant` exists in this native method.** Its callbacks may mutate that field; the MethodBody must preserve those callbacks and re-reads, not invent an assignment.

Fields: targetPlant `0x04000601` / +0x170 / Plant; animator `0x040005FE` / +0x158 / Animator; rSpeed `0x040005FB` / +0x130 / Vector3; isEat `0x040005D5` / +0xBB / bool. Plant.BlockZombie is Assembly-CSharp MethodDef `0x06000358`, address `0x18034DF60`, public nonvirtual instance `(Zombie,bool)->void`. ResetAttackSpeed is `0x06000471`, address `0x180366BE0`, public nonvirtual instance `()->void`.

Animator.SetBool was independently mapped from original `UnityEngine.AnimationModule.dll` CodeGenModule `0x18198B630`, 89 pointers at `0x181CBCF50`. MethodDef `0x06000016` / native `0x1812DBA00` is the `(string,bool)` overload. The `(int32,bool)` overload is MethodDef `0x06000017` / native `0x1812DB9A0` and is not called here.

Proposed exact logical MethodBody: `.maxstack 3`, no used locals, no EH. Existing local metadata may be preserved without use. Explicit raw-null guards preserve the native fault after a successful Unity comparison.

```cil
    ldarg.0
    ldfld Plant Zombie::targetPlant
    ldarg.1
    call bool UnityEngine.Object::op_Inequality(UnityEngine.Object, UnityEngine.Object)
    brfalse COMMON_TAIL
    ldarg.0
    ldfld Plant Zombie::targetPlant
    ldnull
    call bool UnityEngine.Object::op_Inequality(UnityEngine.Object, UnityEngine.Object)
    brfalse CHECK_NEW
    ldarg.0
    ldfld Plant Zombie::targetPlant
    dup
    brtrue OLD_READY
    pop
    newobj instance void System.NullReferenceException::.ctor()
    throw
OLD_READY:
    ldarg.0
    ldc.i4.0
    call instance void Plant::BlockZombie(Zombie, bool)
CHECK_NEW:
    ldarg.1
    ldnull
    call bool UnityEngine.Object::op_Inequality(UnityEngine.Object, UnityEngine.Object)
    brfalse COMMON_TAIL
    ldarg.1
    dup
    brtrue NEW_READY
    pop
    newobj instance void System.NullReferenceException::.ctor()
    throw
NEW_READY:
    ldarg.0
    ldc.i4.1
    call instance void Plant::BlockZombie(Zombie, bool)
COMMON_TAIL:
    ldarg.0
    ldfld UnityEngine.Animator Zombie::animator
    dup
    brtrue ANIMATOR_READY
    pop
    newobj instance void System.NullReferenceException::.ctor()
    throw
ANIMATOR_READY:
    ldstr "isAttacking"
    ldc.i4.1
    call instance void UnityEngine.Animator::SetBool(string, bool)
    ldarg.0
    call instance void Zombie::ResetAttackSpeed()
    ldarg.0
    call UnityEngine.Vector3 UnityEngine.Vector3::get_zero()
    stfld UnityEngine.Vector3 Zombie::rSpeed
    ldarg.0
    ldc.i4.1
    stfld bool Zombie::isEat
    ret
```

## Vector3 public getter proof

Both local source references `audit/stage9-native3/resolver/UnityEngine.CoreModule.dll` and `audit/local-preflight/exact-editor-tools/resolver/UnityEngine.CoreModule.dll` are 1,532,928 bytes and SHA256 `83192116b34abac2bb1797ec0e3943362be33b028b1eefd2e561d5fdc1c3312f`. These exact files contain real MethodBody IL, not stripped empty bodies. Module MVID: `ed526bd9-3af5-4472-9082-ec05c53ac499`.

In this full reference, public Vector3 TypeDef is `0x02000207`; `get_zero` is public static MethodDef `0x06001771`, RVA `0x24818`, signature `000011881c`, CodeSize 11, MaxStack 1, local signature `0x110000AE` / blob `070111881c`, no EH. Whole getter header+body is `133001000b000000ae000011007e730800040a2b00062a`:

```cil
IL_0000: nop
IL_0001: ldsfld UnityEngine.Vector3 UnityEngine.Vector3::zeroVector // full-ref FieldDef 0x04000873
IL_0006: stloc.0
IL_0007: br.s IL_0009
IL_0009: ldloc.0
IL_000A: ret
```

That field is private static readonly, signature `0611881c`. The public getter reads the exact same private field and returns it unchanged. Original stripped PC metadata independently identifies its zeroVector as Vector3 field token `0x040004D0`, static offset zero; native TranToAttack reads the Vector3 static storage at that offset. Full-reference tokens and original stripped-native tokens belong to different metadata images and must not be interchanged.

The remote `/workspaces/GodsPVZ-iOS/Tools/Stage9Native4Recovery/resolver/UnityEngine.CoreModule.dll` was not accessed by this subtask. If parent78 requires that specific path as the provenance witness, compare its hash to the proven local full reference; this remains a path/provenance check, not a getter-semantic gap.

## Native exception helper proof

`0x180250150 -> 0x180209500 -> 0x1802094E0 -> 0x1802078E0` reaches a native factory whose type/namespace ASCII operands are at `0x1819A7AE8` (`NullReferenceException`) and `0x1819A7998` (`System`). The exception path raises rather than returning. The proposed raw-null guards therefore throw; none return or silently skip the operation.

## Remaining uncertainty and handoff boundary

- Current parent77 MethodBody header, local signatures, concrete reference tokens, access scopes, body storage budget, and exact metadata extension strategy are outside this subtask. Parent/root is inspecting them separately. Symbolic recipes cannot claim byte isolation or candidate validity.
- No acceptance result is asserted: no candidate CIL, native oracle, runtime comparison, verifier, build, Unity/iOS/Xcode gate, or game behavior was run. Gemini must implement the authorized testing workflow separately after any authorized candidate construction.
- Native helper/enum/getter evidence is complete for these two methods. There is no helper contract gap in `Vector3.get_zero` because a full reference MethodBody was actually parsed.
- The existing parent lineage's semantic qualifications are not implied by these two source analyses.

Inspectible evidence: `original-source-inspection.txt`, `runtime-helper-source-inspection.txt`, `unity-zero-source-inspection.json`. Their scripts read original inputs and print source facts only; they do not load or execute recovered application CIL.
