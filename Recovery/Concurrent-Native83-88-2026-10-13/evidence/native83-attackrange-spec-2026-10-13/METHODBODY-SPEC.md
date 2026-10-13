# Native83 third target: AttackRange::.ctor (0x060000E0)

This is source-only specification and evidence. The original GameAssembly PE, original global metadata, archived parent82 managed bytes and full existing mscorlib reference were parsed as data. No game/assembly execution, acceptance tests, builds, candidate generation, commits or push occurred. New files are confined to this audit directory.

## Fixed inputs and identity

| Input | SHA256 |
| --- | --- |
| Original `audit/stage9-native3/inputs/GameAssembly.dll` | `9ebd7ca996a5b03fb4a766f7a2502b660d581ddbbb36af4d7bf2f06a211da39d` |
| Original `audit/stage9-native3/inputs/global-metadata.dat` | `ad992341add498bd1018ac980f42171e0737b0793bf945c8714ad70a445b36b9` |
| Archived parent82 `candidates/native82/candidate1.dll` | `48b9e89e941c041d6c11c4064ad05d9384a1a4dd51d5f8d3fad9192cbda5adbf` |
| Existing full `audit/stage9-native3/resolver/mscorlib.dll` (4,622,848 bytes) | `4d1725f1ae54a22f69b79c2b1569181ed6653dd484c35005e29c40300c6673be` |

Parent82 bytes were read inside `audit/native82-codex-2026-10-13/godspvz-native78-82-pending-2026-10-13.zip`, without extracting or modifying the candidate. Parent inventory `audit/native83-codex-2026-10-13/metadata-current.json` reports the same SHA. The latest supplied `audit/native86-codex-2026-10-13/metadata-current.json` was also read: its source SHA is the same parent82 hash, and E0's 92-byte body, local types and all four required MemberRefs remain identical to the bindings below.

Original `AttackRange` typedef index 5111 / `0x02000018`, original `.ctor` token `0x060000E0`, zero explicit arguments and void return. The independently rediscovered Assembly-CSharp CodeGenModule is `0x181639F70`, count 2316, pointer table `0x181B82D60`. E0's pointer is `0x1803002A0`. Its complete pdata span is `[0x1803002A0,0x1803003A3)`, 259 bytes, SHA256 `be44cff4e9b882bb0099684bb0b2510f2e4f786db989daa2c3200416d6c74784`. Unwind flags are zero; there is no native cleanup/handler in this span.

Machine-readable evidence, full disassemblies, original usage-cell closure records, current references and actual current constructor CIL are in `source-evidence.json`. `inspect_source.py` is the source-data reader that produced it.

## Exact native effects and ordering

The receiver is captured from RCX into RDI at `0x1803002B1` and remains that same receiver across all three allocations and constructor calls. RDI is not reloaded from any field. Each new list object is captured in RBX, passed to the shared constructor, and stored into its field only after the constructor returns.

| Order | Allocation | Constructor | Field store | GC barrier | Original field / current token |
| --- | --- | --- | --- | --- | --- |
| 1 | `0x18030030C` → allocator `0x180250100`, class cell `0x181BBFB18` | `0x18030031E` → shared `0x1808053D0`, MethodInfo cell `0x181BB6568` | `0x180300327`, receiver +0x40 | `0x18030032E` → `0x18024F360` | `List<Rect> rects`, `0x040000E3` |
| 2 | `0x18030033A` → same allocator, class cell `0x181BC0568` | `0x18030034C` → same shared ctor, MethodInfo cell `0x181BB9B58` | `0x180300355`, receiver +0x48 | `0x18030035C` → same barrier | `List<Single> radius`, `0x040000E4` |
| 3 | `0x180300368` → same allocator, class cell `0x181BC1A08` | `0x18030037A` → same shared ctor, MethodInfo cell `0x181BC12D8` | `0x180300383`, receiver +0x50 | `0x18030038A` → same barrier | `List<Vector3> centers`, `0x040000E5` |
| 4 | none | `0x18030039E` tail jumps to `0x180302170`, RCX = the captured receiver, EDX = zero | none | none | Original `System.Object::.ctor()` |

The metadata initialization branch at `0x1803002B4` skips only one-time resolution when its byte is set. Its cell resolution order is Rect ctor, Single ctor, Vector3 ctor, Single class, Vector3 class, Rect class; all paths then reach the operational sequence above. The CLR/newobj translation uses the concrete managed constructor operands rather than reproducing IL2CPP resolver calls or GC barrier APIs.

There is no method-level loop, callback, pseudo-null comparison, raw-null defensive skip, early return, catch or finally. The only direct application field writes are the three list references. No explicit writes initialize `transform` (`0x040000DD`), `plant` (DE), `zombie` (DF), `projectile` (E0), `device` (E1), `rangeType` (E2) or `ID` (E6). Do not introduce assignments to those fields from guessed C#.

Allocation and exception order is Rect list first, Single list second, Vector3 list third. A field is written after its constructor succeeds; if a later allocation/constructor throws, earlier field stores have already occurred. The base call is reached only after all three stores. Generic List static initialization and its empty-array initialization belong inside the typed constructor calls; the outer method must not replace them with guessed backing-array field writes. The three List objects are newly allocated for this receiver; standard empty backing arrays remain the List constructor's shared static references.

## Original generic closure proof

Shared native address `0x1808053D0` alone cannot identify a concrete List element type. Both the allocation class usage cell and the passed MethodInfo usage cell were decoded against the original registration. Registration address is `0x1818C6D00`; Types table `0x1818EDEB0`, GenericInst table `0x181679AE0`, MethodSpecs table `0x181684EF0`.

| List element | Original class cell / encoded value / Types index | Original ctor cell / encoded value / MethodSpec index | Concrete class instantiation |
| --- | --- | --- | --- |
| UnityEngine.Rect | `0x181BBFB18` / `0x2000CAE1` / 25968 | `0x181BB6568` / `0xC0010BE1` / 34288 | classInstIndex 344; Rect typedef 3899, token `0x020000A1` |
| System.Single | `0x181BC0568` / `0x2000CB01` / 25984 | `0x181BB9B58` / `0xC0010FA1` / 34768 | classInstIndex 91; primitive kind `0x0C` Single |
| UnityEngine.Vector3 | `0x181BC1A08` / `0x20003DF1` / 7928 | `0x181BC12D8` / `0xC0011799` / 35788 | classInstIndex 219; Vector3 typedef 4008, token `0x0200010E` |

All three MethodSpecs resolve original method definition index 9153, original mscorlib token `0x060023C2`, `System.Collections.Generic.List<T>::.ctor()`, flags `0x1886`, zero arguments, void return, methodInstIndex -1. Their allocation cells resolve the same List<T> generic base (typedef 1272, token `0x020004F8`) and match the method closure element type.

Complete original shared List constructor span `[0x1808053D0,0x180805459)` is included. It calls the same no-op Object base at `0x1808053E2`, retrieves generic metadata from its captured MethodInfo, performs generic class initialization as required, writes the generic static empty-array reference into the new list's +0x10 field at `0x180805446`, and finishes via the GC barrier. This is ordinary parameterless List construction; no extra outer-method field initialization is warranted.

The existing full reference also supplies the complete parameterless List<T> getter-independent constructor IL: reference token `0x06006814`, RVA `0x15F6A2`, CodeSize 18, whole tiny body `4a02281b1b0006027e3b0c000a7d3c0c000a2a`:

```cil
ldarg.0
call instance void System.Object::.ctor()
ldarg.0
ldsfld !0[] System.Collections.Generic.List`1<!0>::s_emptyArray
stfld !0[] System.Collections.Generic.List`1<!0>::_items
ret
```

This reference body is corroboration; original source calls and concrete closures above are the semantic authority. Reference tokens are not current-parent tokens.

## Object constructor identity and complete no-op evidence

Original `System.Object` is typedef index 374 / token `0x02000177`. Its constructor is original metadata method index 3288 / token `0x06000CD9`, flags `0x1886`, zero arguments and void return. This is independently mapped using the original mscorlib CodeGenModule, located by the exact image-name string and its reference:

| Original mapping component | Value |
| --- | --- |
| mscorlib module name cell | `0x181941368` |
| CodeGenModule | `0x1819412E0` |
| Method pointer count/table | 9995 / `0x181C4E110` |
| Object ctor pointer cell | `0x181C547D0` = table + 8*(0xCD9-1) |
| Pointer value | `0x180302170` |
| Complete native instruction | `C2 00 00`, `ret 0`, span `[0x180302170,0x180302173)` |

The operand of `ret imm16` is zero: no body writes, allocation, dereference or callback occurs. The following bytes are `CC` padding. There is no pdata record for this leaf. The pointer is shared by 242 original methods across the two inspected modules (including Object.Finalize and multiple no-op constructors), whose independently decoded identity/pointer cells are retained in the JSON. Sharing explains the historical ambiguous marker; the original mscorlib table explicitly proves Object ctor is a valid alias here.

The full existing mscorlib reference independently has `System.Object::.ctor`, reference token `0x06001B1B`, RVA `0x4A41`, CodeSize 1, whole tiny body `06 2A` = header followed by `ret`; no locals or EH. This supplies full actual helper IL in addition to the original native mapping. The earlier Native83 one-byte fragment at 302170 only captured `C2` and decoded no instruction; this report supersedes that incomplete fragment with all three bytes.

Current available Object ctor reference is `0x0A0000B6`, instance void `.ctor()`, signature `20 00 01`, TypeRef `0x0100001A` for `System.Object` in AssemblyRef row 4 `mscorlib`. It is the same valid reference used by current Buff and DeviceEvent constructors.

## Current parent bindings and exact logical CIL recipe

| Current token | Operand | Signature and class |
| --- | --- | --- |
| `0x0A000096` | instance void `List<Rect>::.ctor()` | `200001`; TypeSpec `0x1B00001E`, `15120d011180cd` |
| `0x0A000094` | instance void `List<Single>::.ctor()` | `200001`; TypeSpec `0x1B00001C`, `15120d010c` |
| `0x0A000095` | instance void `List<Vector3>::.ctor()` | `200001`; TypeSpec `0x1B00001D`, `15120d01114d` |
| `0x0A0000B6` | instance void `System.Object::.ctor()` | `200001`; TypeRef/AssemblyRef above |
| `0x040000E3` | `AttackRange.rects` | class `List<Rect>` |
| `0x040000E4` | `AttackRange.radius` | class `List<Single>` |
| `0x040000E5` | `AttackRange.centers` | class `List<Vector3>` |

The requested reconstruction is precisely this straight-line sequence (40 bytes of logical IL, maxstack 2, no EH, no used locals):

```cil
ldarg.0
newobj instance void System.Collections.Generic.List`1<UnityEngine.Rect>::.ctor()     // 0x0A000096
stfld class System.Collections.Generic.List`1<UnityEngine.Rect> AttackRange::rects  // 0x040000E3
ldarg.0
newobj instance void System.Collections.Generic.List`1<float32>::.ctor()             // 0x0A000094
stfld class System.Collections.Generic.List`1<float32> AttackRange::radius           // 0x040000E4
ldarg.0
newobj instance void System.Collections.Generic.List`1<UnityEngine.Vector3>::.ctor() // 0x0A000095
stfld class System.Collections.Generic.List`1<UnityEngine.Vector3> AttackRange::centers // 0x040000E5
ldarg.0
call instance void System.Object::.ctor()                                           // 0x0A0000B6
ret
```

Preserving current unused local signature `0x110000B3` is sufficient if required by the parent patch method; no new local types are needed. Its actual bytes are `070712601c1c15120d011180cd15120d010c15120d01114d1c`: V0 AttackRange, V1/V2 Object, V3 List<Rect>, V4 List<Single>, V5 List<Vector3>, V6 Object. The new recipe reads or writes none of them.

Current E0 is CodeSize 92, maxstack 2, no EH. Its branch order executes Rect allocation/store first, Single second, Vector3 third; the end block at IL offset 60 performs `ldstr 0x70000331` (`Method not found @180302170`), `pop`, `ret` instead of the proven Object base call. The literal load is observable managed work, so its removal is part of this explicit third repair target. The new recipe must not retain the marker or merely append the Object call after it.

## Hide/Buff dependency review and limits

Original Buff ctor `0x060000F5` / native `[0x180310BB0,0x180310C8D)` writes name = String.Empty (+0x10), duration = bit pattern `0x3F800000` (1.0f, +0x1C), allocates/calls AttackRange E0 then stores buffRange (+0x40), allocates/calls `List<Buff>::.ctor()` then stores childBuffs (+0x48), and tail calls the same no-op Object base. Original `List<Buff>` closure is separately retained: allocation cell `0x181BBCA10`, type index 25863, ctor cell `0x181BA4FF8`, MethodSpec index 71771; current typed ctor reference is `0x0A0000BB` (TypeSpec `0x1B000026`, signature `15120d011270`). No inferred initialization of other Buff fields is necessary.

Current Buff F5 actually calls Object base first, then executes those same field/constructor/store effects in the same order. The base-call position differs syntactically, but original complete native bytes and actual full helper IL prove that Object ctor has no side effects or receiver dereference. No additional non-equivalent managed operation was found in this current Buff body, apart from its dependence on the explicitly damaged AttackRange E0.

Original Hide ctor `0x060000FD` / native `[0x18031C900,0x18031C954)` captures receiver and both arguments, calls Buff F5 exactly once, writes hide bool (+0x58), zombie reference (+0x60, then barrier), duration `0x3F000000` (0.5f, +0x1C), buffType 0 (+0x18), original false (+0x20), then returns. Actual current Hide FD has the same calls, writes and order. No additional non-equivalent effect was found in the two inspected dependency bodies; there is no guessed dependency repair to add.

This closes E0's original semantic specification, concrete generic closures, base helper identity and current operand bindings. It does not accept a candidate or prove the user's required runtime/build criteria; those remain Gemini's responsibility. No private inline helper substitution is proposed for E0.
