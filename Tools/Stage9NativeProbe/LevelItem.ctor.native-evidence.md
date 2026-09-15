# LevelItem::.ctor PC native authority

Read-only evidence for the Stage9.1 recovery chain. This file records the native authority used to constrain a later managed repair; it does not itself mutate a candidate and does not promote the sealed baseline.

## Source authority

- Original Drive package: `GodsPVZ_1.0.2.zip` (Drive ID `1_5kVGTFmsvwcdRLQ2ah_rtyC6DqvLnf0`)
- ZIP size: `242474120` bytes
- ZIP SHA256: `2f08b4e2243e2bd296f3db694b4a2c9656252d76202c80167bdf3340b9f65b48`
- Embedded `GameAssembly.dll` SHA256: `9ebd7ca996a5b03fb4a766f7a2502b660d581ddbbb36af4d7bf2f06a211da39d`
- Embedded `global-metadata.dat` SHA256: `ad992341add498bd1018ac980f42171e0737b0793bf945c8714ad70a445b36b9`

The same method-pointer-table mapping previously self-calibrated against SuppliesInitialValue::.ctor is used here:

`entryVA = 0x181B82D60 + (RID - 1) * 8`

## Managed identity on candidate 76c1

Read-only probe run `34988210632` / artifact `10404426608` on candidate:

`76c1c48219af73c73e48fcfb3d0a53254b567c07db7450a80ed51b61a2f6ce19`

- `LevelItem` TypeDef: `0x020000BE`, RID 190
- `LevelItem::.ctor()` MethodDef: `0x0600062D`, RID 1581
- managed RVA: `0x87684`
- damaged code size: 675 bytes
- damaged locals: 17
- exception handlers: 0
- `rescueSeedID` FieldDef: `0x04000838`, RID 2104, type `System.Collections.Generic.List<System.Int32>`
- damaged body contains 25 `Unmanaged memory load:` diagnostics, two `ldc.i8 4294967295`, and five exposed `List<int>::AddWithResize` calls.

The assembly already contains reusable references for:

- `System.Collections.Generic.List<int>::.ctor()`
- `System.Collections.Generic.List<int>::Add(!0)`
- `UnityEngine.MonoBehaviour::.ctor()`

No new method or assembly reference is required for a CLR-faithful rebuild.

## Method pointer and native range

For RID 1581:

- method pointer entry: `0x181B85EC0`
- direct 8-byte pointer value: `0x18039F130`
- `.pdata` RuntimeFunction: `0x18039F130–0x18039F365`
- exact native body size: 565 bytes
- exact native slice SHA256: `00de368a2072ce9b97ae6e5211dd9262b61537bbd1be1d608638c09cdf7e295c`

## Native semantics

The PC x64 body performs the following operations in order:

1. Initializes the required IL2CPP metadata for `List<int>`.
2. Allocates a new `System.Collections.Generic.List<int>` and invokes its constructor.
3. Appends five values in this exact order: `-1`, `3`, `5`, `6`, `7`.
   - IL2CPP uses the normal inlined `List<T>.Add` fast path.
   - Capacity overflow branches to the generic resize helper at `0x180827460`; this is an implementation detail and is not managed-source authority for calling `AddWithResize` directly.
4. Stores the resulting list into the instance field at `this + 0x60`, matching managed FieldDef `LevelItem::rescueSeedID` (`0x04000838`).
5. Executes the IL2CPP write barrier for that reference store.
6. Clears the hidden MethodInfo argument and tail-jumps to the same `UnityEngine.MonoBehaviour::.ctor()` base path at `0x1812E22A0`.

Key disassembly excerpts:

```text
0x18039F171  mov  rcx, [0x181BBE630]
0x18039F178  call 0x180250100              ; allocate object
0x18039F18A  call 0x1808053D0              ; List<int> ctor

; first value -1
0x18039F1BC  mov  edx, 0xffffffff
...
0x18039F1E5  mov  dword ptr [rdx+rcx*4+0x20], 0xffffffff

; subsequent values
0x18039F211  mov  edx, 3
0x18039F23A  mov  dword ptr [rdx+rcx*4+0x20], 3
0x18039F266  mov  edx, 5
0x18039F28F  mov  dword ptr [rdx+rcx*4+0x20], 5
0x18039F2BB  mov  edx, 6
0x18039F2E0  mov  dword ptr [rdx+rcx*4+0x20], 6
0x18039F308  mov  edx, 7
0x18039F32D  mov  dword ptr [rdx+rcx*4+0x20], 7

; rescueSeedID = list
0x18039F335  lea  rcx, [rdi+0x60]
0x18039F339  mov  qword ptr [rdi+0x60], rbx
0x18039F340  call 0x18024F360              ; write barrier

; base ctor
0x18039F345  xor  edx, edx
0x18039F347  mov  rcx, rdi
0x18039F354  jmp  0x1812E22A0
```

## Repair constraint

The managed repair must be a single-MethodDef rebuild of `0x0600062D` equivalent to:

```csharp
rescueSeedID = new List<int> { -1, 3, 5, 6, 7 };
base();
```

At IL level it should use the already-present public `List<int>::.ctor()` and `List<int>::Add(!0)` references, then store to FieldDef `0x04000838`, and invoke the already-present `MonoBehaviour::.ctor()` reference. It must not retain the decompiler-exposed `_items/_size/AddWithResize` implementation details, add guards, swallow exceptions, change metadata, or introduce `System.Private.CoreLib`.
