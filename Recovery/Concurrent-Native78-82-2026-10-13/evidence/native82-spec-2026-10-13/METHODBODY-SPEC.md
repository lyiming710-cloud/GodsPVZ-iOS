# Native82 static MethodBody specification

This specification covers original Assembly-CSharp methods `0x0600087D` and `0x06000734`. Only original source bytes, original IL2CPP metadata/registration records, existing native55 disassembly, the full resolver DLLs, and the parent79 metadata inventory were inspected. No game/candidate CIL or native code was executed. No tests, builds, candidates, repository changes, commits, or pushes were produced. New source-inspection artifacts are confined to this folder; testing remains with Gemini.

## Source identity

| Source | SHA-256 |
| --- | --- |
| `audit/stage9-native3/inputs/GameAssembly.dll` | `9ebd7ca996a5b03fb4a766f7a2502b660d581ddbbb36af4d7bf2f06a211da39d` |
| `audit/stage9-native3/inputs/global-metadata.dat` | `ad992341add498bd1018ac980f42171e0737b0793bf945c8714ad70a445b36b9` |
| `audit/native30-continuation-2026-10-02/native55-native.json` | `304b632ad13168da91e4b03e2824f69f053c592f59b3a089756a5e4114445277` |
| full resolver `UnityEngine.CoreModule.dll` | `83192116b34abac2bb1797ec0e3943362be33b028b1eefd2e561d5fdc1c3312f` |
| full resolver `mscorlib.dll` | `4d1725f1ae54a22f69b79c2b1569181ed6653dd484c35005e29c40300c6673be` |

`extract_source.py` reads and hashes the original inputs, matches each target MethodDef pointer in the original table `0x181B82D60`, rehashes its exact native interval, decodes metadata usages, independently maps direct calls from original CodeGenModules, extracts relevant full resolver CIL as data, and writes `PRIMARY-EVIDENCE.json`. The native55 damaged instructions were used only to locate candidate references and existing local types. Native semantics below come from the original bytes and registration data.

| Target | Original interval | Bytes | SHA-256 |
| --- | --- | ---: | --- |
| `TMP_TextEventHandler.Awake` | `[0x1803B7560, 0x1803B770F)` | 431 | `96463776ff756abd4c7326880d5a2efdcad4a31ddd844158949544ebb05f2f17` |
| `SwfManager.GetInstance(bool allow_create)` | `[0x1803C9170, 0x1803C9310)` | 416 | `c671e10e6885d0ff6077b1ce0b34f4f45095f355e6516a5517e3a93fd5ebf5aa` |

The current references/locals below come from the read-only `audit/native80-codex-2026-10-13/metadata-current.json` inventory, whose described parent79 DLL SHA-256 is `3b3bcfca51402e1eedf5d59fbe1bf9d923336730325550205f960bd8a8bceec6`. Reconcile final tokens and storage against the actual selected candidate input; the specification does not claim candidate slot/metadata/isolation acceptance.

## Registration and exact generic identities

Original MetadataRegistration: `0x1818C6D00`. Relevant pairs:

- `+0x10/+0x18`: GenericInst count 5,659, pointer array `0x181679AE0`.
- `+0x20/+0x28`: GenericMethodFunctionsDefinition count 61,280, 16-byte record table `0x1817D7700`.
- `+0x30/+0x38`: Types count 27,557, pointer table `0x1818EDEB0`.
- `+0x40/+0x48`: MethodSpec count 74,797, 12-byte record table `0x181684EF0`.
- `+0x50/+0x58`: FieldOffsets count 7,696, table `0x181B5E000`.

Usage kind is `(raw >> 29) & 7`, source index is `(raw & 0x1fffffff) >> 1`; all listed file-time usages have the low tagged bit set. Kind 1 resolves an Il2CppClass from a registered type; kind 2 resolves the registered Il2CppType; kind 5 resolves a string literal; kind 6 resolves a MethodSpec plus generic instantiation. These are file-time cells, not already-mutated runtime pointers.

| Cell | File value | Kind/index | Exact resolution |
| --- | --- | --- | --- |
| `0x181BA5818` | `0xC0024511` | 6 / 74376 | MethodSpec `[36978,-1,3515]`: `GameObject.GetComponentInParent<Canvas>()`; original Core MethodDef `0x06000829`; GenericInst 3515 argument type 12657 / TypeDef 7431 / `UnityEngine.Canvas` |
| `0x181BA5338` | `0xC00244F7` | 6 / 74363 | MethodSpec `[36969,-1,2530]`: `GameObject.GetComponent<TMP_Text>()`; original Core MethodDef `0x06000820`; GenericInst 2530 argument type 18735 / TypeDef 5559 / `TMPro.TMP_Text` |
| `0x181BA40D8` | `0xC0024495` | 6 / 74314 | MethodSpec `[37000,-1,5501]`: `GameObject.AddComponent<SwfManager>()`; original Core MethodDef `0x0600083F`; GenericInst 5501 argument type 18603 / TypeDef 5379 / `FTRuntime.SwfManager` |
| `0x181BB74C8` | `0xC00245F3` | 6 / 74489 | MethodSpec `[37122,-1,5501]`: `Object.FindObjectOfType<SwfManager>()`; original Core MethodDef `0x060008B9`; same concrete SwfManager argument |
| `0x181BB0500` | `0x200081D1` | 1 / 16616 | `UnityEngine.Object`, TypeDef 4104 |
| `0x181BA1CB8` | `0x200071BD` | 1 / 14558 | `UnityEngine.GameObject`, TypeDef 4078 |
| `0x181BBC038` | `0x20009157` | 1 / 18603 | `FTRuntime.SwfManager`, TypeDef 5379 |
| `0x181BBFCF0` | `0x20009707` | 1 / 19331 | `System.Type`, TypeDef 262 |
| `0x181BA3130` | `0x4000944D` | 2 / 18982 | `TMPro.TextMeshProUGUI`, TypeDef 5423 in original `Unity.TextMeshPro.dll`, TypeDef token `0x0200000F` |
| `0x181BCE6F0` | `0xA0002A71` | 5 / 5432 | 12 exact UTF-8 bytes `5b5377664d616e616765725d`, literal `[SwfManager]` |

For the `typeof` operand, Types slot `0x181912FE0` points to Il2CppType `0x181BE0310`, bytes `2f150000000000000000120000000000`: kind CLASS (`0x12`), TypeDef index 5423. Its original parent type index is 18735 (`TMP_Text`). This proves the typeof identity independently of damaged CIL.

Concrete usages do not have separate emitted generic-method-table records; the original reference-type sharing records use GenericInst 85, registered type 16602 / OBJECT (`0x1C`) / `System.Object`, TypeDef data 374. Recovering the table from four independent generic call targets yields one common generic pointer table, `0x181760110`, also referenced at `0x1815E88D8` with preceding pointer count 61,118 (`0xEEBE`).

| Concrete operation | Canonical MethodSpec | Generic table row | Method pointer index/cell | Original native body |
| --- | ---: | ---: | --- | --- |
| `AddComponent<SwfManager>()` | 65998 | 58448 | 58291 / `0x1817D1EA8` | `0x18046D280` |
| `GetComponent<TMP_Text>()` | 66003 | 58449 | 58292 / `0x1817D1EB0` | `0x18046DC80` |
| `GetComponentInParent<Canvas>()` | 66012 | 58455 | 58298 / `0x1817D1EE0` | `0x18046D9C0` |
| `FindObjectOfType<SwfManager>()` | 67157 | 59242 | 59080 / `0x1817D3750` | `0x1804A0420` |

The full Core resolver CIL is recorded in JSON, including generic `GetComponent`, generic `GetComponentInParent` (no argument overload delegates with `includeInactive=false`), generic `AddComponent`, generic `FindObjectOfType` (passes `includeInactive=false`), `op_Implicit`, `op_Inequality`, and `CompareBaseObjects`. Reuse the existing typed MethodSpecs. Do not substitute a raw untyped result, hand-written component search, or private `CompareBaseObjects` call.

## The `0x1804C3750` call is equality, with a shared-address naming limit

The complete executable leaf is seven bytes `48 3b ca 0f 94 c0 c3`, `[0x1804C3750,0x1804C3757)`, SHA-256 `6c7155320c195d0556df9718ade8e8010234f4168a018c3a4ac41ba1b48480f7`:

```asm
cmp rcx, rdx
sete al
ret
```

At Awake's call site, RCX is the single captured result of `Object.GetType()`; RDX is the single result of `Type.GetTypeFromHandle(typeof(TextMeshProUGUI))`; R8 (MethodInfo) is zero. It compares those two Type object references for equality, without dereferencing either argument, walking a base-type chain, or making another call.

Independent original `mscorlib.dll` CodeGenModule mapping (`0x1819412E0`, pointer count 9,995, table `0x181C4E110`) maps `System.Type.op_Equality(Type,Type)` original MethodDef `0x060008B7` to `0x1804C3750`. The same address also represents `System.RuntimeType.op_Equality`, `System.Object.ReferenceEquals`, `System.Object.Equals`' simple body, and several pointer/value equality operations; all aliases are retained in JSON. Therefore the native pointer alone cannot prove which one of these equivalent original managed operand names was emitted. The exact **behavior** and a legal concrete CIL operation are proven: reference equality. Do not claim that the shared body uniquely recovers the original managed call token.

Original `System.Type.IsSubclassOf(Type)` is MethodDef `0x06000835`, address `0x180CCFFB0`. `System.RuntimeType.IsSubclassOf` is `0x06000BD1`, address `0x180CEF230`. Neither is the Awake call target. Full resolver mscorlib `System.Type.op_Equality` is public static MethodDef `0x06001216`, signature `000202128630128630`, body bytes `16 02 03 fe 01 2a` (tiny header then `ldarg.0; ldarg.1; ceq; ret`), body SHA-256 `f86465669b9068db23cdf91a2affadf201cdfa97d4edd32fa334d91665210bc4`.

Consequently `ceq` over the two captured Type references is an exact, metadata-neutral CIL representation. Calling a newly imported public `Type.op_Equality` would also represent the proven body but is unnecessary. `IsSubclassOf`, `isinst TextMeshProUGUI`, `IsAssignableFrom`, virtual `Equals`, and reversed/inverted branch meanings alter the contract. A subclass of TextMeshProUGUI follows the non-UI branch; exactly TextMeshProUGUI follows the canvas branch.

## Direct calls and current operands

| Native VA | Original image/MethodDef | Exact identity | Current parent79 operand |
| --- | --- | --- | --- |
| `0x18131F870` | Core `0x060008A5` | `bool Object.op_Implicit(Object)` | `0x0A00000B` |
| `0x18131F900` | Core `0x060008BE` | `bool Object.op_Inequality(Object,Object)` | `0x0A00000C` |
| `0x18131BCF0` | Core `0x06000847` | `void GameObject..ctor(string)` | `0x0A0000F3` |
| `0x1813044D0` | Core `0x060007E7` | `GameObject Component.get_gameObject()` | `0x0A000009` |
| `0x180D004C0` | mscorlib `0x06000CDC` | `Type Object.GetType()`; public nonvirtual instance | `0x0A00028C` |
| `0x180CCEE30` | mscorlib `0x060008AD` | `Type Type.GetTypeFromHandle(RuntimeTypeHandle)` | `0x0A00004C` |
| `0x1812E27C0` | Core `0x060001C1` | `Camera Camera.get_main()` | `0x0A000004` |
| `0x181546500` | UI `0x0600003F` | `RenderMode Canvas.get_renderMode()` | `0x0A0002E1` |
| `0x181546760` | UI `0x0600005A` | `Camera Canvas.get_worldCamera()` | `0x0A0002E2` |
| `0x1804A0420` | exact MethodSpec above | `SwfManager Object.FindObjectOfType<SwfManager>()` | `0x2B0000AE` |
| `0x18046D280` | exact MethodSpec above | `SwfManager GameObject.AddComponent<SwfManager>()` | `0x2B0000AF` |
| `0x18046DC80` | exact MethodSpec above | `TMP_Text GameObject.GetComponent<TMP_Text>()` | `0x2B000097` |
| `0x18046D9C0` | exact MethodSpec above | `Canvas GameObject.GetComponentInParent<Canvas>()` | `0x2B000098` |

`0x18024FEF0` is the metadata usage resolver, `0x1802501E0` class initialization, `0x180250100` object allocation, and `0x18024F360` the GC write barrier. These runtime operations are represented by ordinary managed metadata/newobj/field stores, not new managed helper calls. Raw-null failure calls `0x180250150`; original source fragments trace `0x180250150 -> 0x180209500 -> 0x1802094E0 -> 0x1802078E0`, whose factory uses original strings `NullReferenceException` at `0x1819A7AE8` and `System` at `0x1819A7998`. The fragments and strings were read anew and are recorded in JSON. A legal explicit CIL failure path is `newobj instance void NullReferenceException::.ctor(); throw`, existing current MemberRef `0x0A000022`.

## `0x0600087D` SwfManager.GetInstance(bool)

Original signature is public static `FTRuntime.SwfManager GetInstance(bool allow_create)`. `_instance` is private static FieldDef `0x04000AAA`, registered static offset 0, type SwfManager. Native accesses class static-fields pointer at class +0xB8 then slot +0. Same-owner `ldsfld/stsfld` is legal; no access expansion is needed.

### CFG and observable sequence

| Block | Native instructions | Next step |
| --- | --- | --- |
| Metadata prologue | `917A..91CE` | Resolve six usage cells once; proceed |
| First truth test | Capture `_instance` at `91E3`; class init if needed; `op_Implicit(captured)` at `9200` | true -> RETURN; false -> FIND |
| FIND | `FindObjectOfType<SwfManager>()` at `9229`; assign result `_instance` at `923C`; write barrier at `9250` | inspect allow_create |
| Creation flag | `9255..9258` | zero -> RETURN; nonzero -> second truth test |
| Second truth test | Capture current `_instance` at `926C`; `op_Implicit(captured)` at `9289` | true -> RETURN; false -> CREATE |
| CREATE | allocate GameObject `9299`; string ctor `92AE`; raw allocation reference check `92B3` | null -> FAIL; non-null -> ADD |
| ADD | call typed `AddComponent<SwfManager>()` on that exact created receiver `92C2`; assign returned result `_instance` at `92D5`; write barrier `92E9` | RETURN |
| RETURN | reread `_instance` at `9301` | return reread value |
| FAIL | `930A -> 0x180250150` | throw NullReferenceException |

`allow_create=false` still performs FindObjectOfType when the first Unity truth test is false. It suppresses only the second truth test and creation. An initially live reference skips the search regardless of the flag. A found null/destroyed reference is returned directly when creation is disabled.

Both truth tests use Unity `op_Implicit`, including destroyed-object pseudo-null behavior. They are not raw reference null tests. Each call receives the field value captured before the call/class initialization. A callback may mutate `_instance`; the decision uses the captured object's truth result, while RETURN reads the then-current field. The FindObjectOfType and AddComponent results overwrite callback changes to `_instance` only after each call returns. AddComponent may reenter GetInstance; the outer call still assigns its own result afterward. The created GameObject is retained across its constructor and passed to AddComponent without rereading any global.

The raw GameObject-null check occurs **after** its string constructor returns and **before** AddComponent. There is no Unity truth conversion of the created GameObject or AddComponent result. Managed `newobj` ordinarily either returns a non-null reference or throws; retaining the explicit post-constructor raw-null branch preserves the native statement boundary for instrumented call contracts.

### Concrete CIL recipe

Preserve current local signature `0x1100065A`. Use existing V_7 (`UnityEngine.GameObject`); V_5/V_8 SwfManager locals are optional. Proposed `.maxstack 2`, no EH, no additional locals or metadata. The input native ABI captures the low unsigned byte of allow_create (`movzx edi,cl`); `conv.u1` retains that representation if an instrumented caller supplies noncanonical bool stack bits. Ordinary bool arguments give the usual 0/1 behavior.

```cil
    ldsfld FTRuntime.SwfManager FTRuntime.SwfManager::_instance
    call bool UnityEngine.Object::op_Implicit(UnityEngine.Object) // 0A00000B
    brtrue RETURN

    call !!0 UnityEngine.Object::FindObjectOfType<FTRuntime.SwfManager>() // 2B0000AE
    stsfld FTRuntime.SwfManager FTRuntime.SwfManager::_instance
    ldarg.0
    conv.u1
    brfalse RETURN

    ldsfld FTRuntime.SwfManager FTRuntime.SwfManager::_instance
    call bool UnityEngine.Object::op_Implicit(UnityEngine.Object)
    brtrue RETURN

    ldstr "[SwfManager]"
    newobj instance void UnityEngine.GameObject::.ctor(string) // 0A0000F3
    stloc.s 7
    ldloc.s 7
    brfalse FAIL_NULL
    ldloc.s 7
    call instance !!0 UnityEngine.GameObject::AddComponent<FTRuntime.SwfManager>() // 2B0000AF
    stsfld FTRuntime.SwfManager FTRuntime.SwfManager::_instance
RETURN:
    ldsfld FTRuntime.SwfManager FTRuntime.SwfManager::_instance
    ret
FAIL_NULL:
    newobj instance void System.NullReferenceException::.ctor() // 0A000022
    throw
```

## `0x06000734` TMPro.TMP_TextEventHandler.Awake()

Original signature is private nonvirtual instance `void Awake()`. Private fields are legal within this same owning type:

| Field | FieldDef | Offset | Type |
| --- | --- | ---: | --- |
| `m_TextComponent` | `0x04000930` | +0x48 | TMP_Text; original registered field type 18736 |
| `m_Camera` | `0x04000931` | +0x50 | Camera; original registered field type 12601 |
| `m_Canvas` | `0x04000932` | +0x58 | Canvas; original registered field type 12658 |

### CFG and reads across callbacks

1. Call `this.get_gameObject()` once (`75BF`). Test the returned reference for **raw** null (`75C4`); raw null throws immediately at FAIL. A destroyed non-null GameObject still reaches typed `GetComponent<TMP_Text>()` (`75D7`). Assign the returned value to `this.m_TextComponent` (`75E0`).
2. Reread `m_TextComponent` (`75EC`); raw null (`75F0`) throws. Call `Object.GetType()` on this captured reread exactly once (`7600`), preserve its result, obtain `typeof(TextMeshProUGUI)` once (`7629`), then compare the two Type references once (`7637`). Never reread m_TextComponent after GetType.
3. If unequal (`7643` fallthrough), call `Camera.main` (`7647`), assign its result to `m_Camera` (`7650`) and return. This branch does not read/write m_Canvas. A derived TextMeshProUGUI follows this branch.
4. If equal (`7643 -> 7667`), call `this.get_gameObject()` **again** (`766C`), raw-null-check this independently returned reference (`7671`), call typed `GetComponentInParent<Canvas>()` on it (`7684`), and assign result to m_Canvas (`768D`). Do not reuse the first GameObject, even if ordinarily equal.
5. Capture the then-current m_Canvas (`76A0`) and pass that capture plus null to Unity `op_Inequality` (`76BA`). False -> return without a camera write (`76C1 -> 765C`). A destroyed non-null canvas is treated as false here. Do not replace this with raw null or a defensive skip of later failures.
6. If Unity inequality returns true, reread m_Canvas (`76C3`), raw-null-check (`76C7`), and call renderMode on that captured reread (`76CE`). A callback that clears the field between the Unity comparison and this read must produce NRE here. A swap must use the replacement canvas.
7. If renderMode's int32 bits are nonzero (`76D3/76D5`), reread m_Canvas again (`76D7`), raw-null-check (`76DB`), call worldCamera (`76E2`) and assign its result to m_Camera through the shared store block (`764C..7657`). A renderMode callback may swap/null the field; the next call uses the new read. No Unity truth conversion applies to this reread or returned camera.
8. If renderMode is exactly zero (`76D5 -> 76EC`), set m_Camera to raw null (`76F0`) and return. All nonzero enum values, including unexpected negative/positive values, take worldCamera.
9. Every explicit raw-null failure joins `7709 -> 0x180250150`, raising NullReferenceException. If a callee itself throws, propagate it at the original call boundary. No cleanup/finally or extra field stores exist.

The initial GetType path dereferences the managed TMP_Text wrapper by raw-null semantics even if Unity considers it destroyed. The canvas inequality is the only Unity pseudo-null decision in this method. A null GetComponent result is first stored in m_TextComponent, then its reread raises NRE; a null second gameObject result leaves m_Canvas/m_Camera as they were. A null GetComponentInParent result is stored in m_Canvas and then returns through the false Unity inequality, preserving the old camera. If renderMode/worldCamera throws, the new canvas remains assigned while the old camera remains until a successful camera/null assignment.

### Concrete CIL recipe

Preserve existing local signature `0x11000573`. Available locals needed by this recipe: V_1 GameObject, V_2 TMP_Text, V_3 GameObject, V_9 System.Type, V_11 Camera, V_13 Canvas. Proposed `.maxstack 2`, no EH. The TypeRef for TextMeshProUGUI is current `0x01000021`. All calls already have current operands in the table above; equality uses `ceq`, so no new MemberRef is required.

The `dup; brtrue` raw checks leave the exact checked receiver on the stack for the following call. The failure arm removes that receiver before joining FAIL_NULL. This avoids a second field read between a raw check and its callee.

```cil
    ldarg.0
    call instance UnityEngine.GameObject UnityEngine.Component::get_gameObject() // 0A000009
    stloc.1
    ldloc.1
    dup
    brtrue FIRST_GAMEOBJECT
    pop
    br FAIL_NULL
FIRST_GAMEOBJECT:
    call instance !!0 UnityEngine.GameObject::GetComponent<TMPro.TMP_Text>() // 2B000097
    stloc.2
    ldarg.0
    ldloc.2
    stfld TMPro.TMP_Text TMPro.TMP_TextEventHandler::m_TextComponent

    ldarg.0
    ldfld TMPro.TMP_Text TMPro.TMP_TextEventHandler::m_TextComponent
    dup
    brtrue TEXT_PRESENT
    pop
    br FAIL_NULL
TEXT_PRESENT:
    call instance System.Type System.Object::GetType() // 0A00028C, nonvirtual with explicit preceding null check
    stloc.s 9
    ldloc.s 9
    ldtoken TMPro.TextMeshProUGUI // 01000021
    call System.Type System.Type::GetTypeFromHandle(System.RuntimeTypeHandle) // 0A00004C
    ceq
    brtrue UI_TEXT

    call UnityEngine.Camera UnityEngine.Camera::get_main() // 0A000004
    stloc.s 11
    br STORE_CAMERA

UI_TEXT:
    ldarg.0
    call instance UnityEngine.GameObject UnityEngine.Component::get_gameObject()
    stloc.3
    ldloc.3
    dup
    brtrue SECOND_GAMEOBJECT
    pop
    br FAIL_NULL
SECOND_GAMEOBJECT:
    call instance !!0 UnityEngine.GameObject::GetComponentInParent<UnityEngine.Canvas>() // 2B000098
    stloc.s 13
    ldarg.0
    ldloc.s 13
    stfld UnityEngine.Canvas TMPro.TMP_TextEventHandler::m_Canvas

    ldarg.0
    ldfld UnityEngine.Canvas TMPro.TMP_TextEventHandler::m_Canvas
    ldnull
    call bool UnityEngine.Object::op_Inequality(UnityEngine.Object,UnityEngine.Object) // 0A00000C
    brfalse RETURN

    ldarg.0
    ldfld UnityEngine.Canvas TMPro.TMP_TextEventHandler::m_Canvas
    dup
    brtrue RENDER_RECEIVER
    pop
    br FAIL_NULL
RENDER_RECEIVER:
    call instance UnityEngine.RenderMode UnityEngine.Canvas::get_renderMode() // 0A0002E1
    brfalse OVERLAY

    ldarg.0
    ldfld UnityEngine.Canvas TMPro.TMP_TextEventHandler::m_Canvas
    dup
    brtrue WORLD_RECEIVER
    pop
    br FAIL_NULL
WORLD_RECEIVER:
    call instance UnityEngine.Camera UnityEngine.Canvas::get_worldCamera() // 0A0002E2
    stloc.s 11
    br STORE_CAMERA
OVERLAY:
    ldnull
    stloc.s 11
STORE_CAMERA:
    ldarg.0
    ldloc.s 11
    stfld UnityEngine.Camera TMPro.TMP_TextEventHandler::m_Camera
RETURN:
    ret
FAIL_NULL:
    newobj instance void System.NullReferenceException::.ctor() // 0A000022
    throw
```

These explicit raw checks followed by nonvirtual `call` retain the native caller's receiver checks without introducing another field read. A `callvirt` to these nonvirtual members with the same captured receiver is a legal alternative, but it must preserve the stated order and not replace the Unity inequality with a raw check. The recipes do not access private external Unity internals or require a private-inline/public-helper substitution.

## Remaining implementation and acceptance gaps

- Native equality behavior is fully determined, while the unique original managed token for the shared equality body cannot be determined from the native pointer alone. `ceq` avoids depending on that naming ambiguity and is source-proven equivalent by full resolver CIL.
- The actual selected descendant's local signatures, metadata operands, reserved MethodBody slots, UserString availability, header flags, CodeSize, and non-target isolation need inspection at implementation time. No candidate or bytes were generated here. The parent79 inventory supplies reusable tokens/locals, not acceptance evidence.
- Normal managed metadata/newobj/field stores replace IL2CPP metadata initialization, allocation and GC barriers. Internal allocation failure representations and one-time loader/class initialization are runtime implementation details; the source-defined explicit receiver exception points are preserved. No out-of-memory/loader behavior equivalence is claimed from static analysis alone.
- Stack maxima above follow manual direct accounting only. Typed verifier, real candidate-CIL/native fixtures, alias/reentry cases, malformed controls, member-access checks, determinism, and byte isolation have not been run. All such testing belongs to Gemini.
- Full IL2CPP/all-translation-unit gates, Unity/iOS export, and IPA remain outside this source-only deliverable.
