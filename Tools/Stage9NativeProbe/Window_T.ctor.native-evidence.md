# Stage9.1 Window_T::.ctor PC native authority

This is a read-only authority record for the next runtime-selected causal blocker after the exact-R3 qualification of `Almanac_TalentSystem::.ctor`. It does not promote any development candidate to the sealed HF55 baseline.

## Locked source

- PC source: original GodsPVZ 1.0.2 `GameAssembly.dll`
- `GameAssembly.dll` SHA256: `9ebd7ca996a5b03fb4a766f7a2502b660d581ddbbb36af4d7bf2f06a211da39d`
- Method-pointer table base: `0x181B82D60`
- Extraction path was self-checked against the already-qualified Supplies RID604 authority before the batch prefetch.

## Managed identity on runtime-qualified chain

- Type: `Window_T`
- TypeDef token: `0x020000D5`, RID 213
- Method: `Window_T::.ctor()`
- MethodDef token: `0x0600070C`, RID 1804
- Managed RVA: `0x96CA4`
- Managed code size: 1943 bytes
- Managed locals: 4
- Damaged field: `System.Int32 Window_T::T`
- Field token: `0x0400090A`
- Damaged managed tail sequence contains `ldc.i8 4294967295` followed by `stfld int32 Window_T::T`.

## PC native mapping

Method-pointer entry formula: `0x181B82D60 + (RID-1)*8`.

- RID: 1804
- Pointer entry VA: `0x181B865B8`
- Native function start: `0x1803B0880`
- Native function end: `0x1803B1647`
- Native length: 3527 bytes
- Native slice SHA256: `e03f74996efd73da64dffafcaa52bb23a6ac86159c698a898cd4161518710fa5`

## Authoritative semantics relevant to the damaged managed IL

The PC native constructor performs the large `Window_T` data/string-array initialization, then contains a 32-bit sentinel store equivalent to:

```text
mov dword ptr [this+0x20], 0xffffffff
```

and eventually tail-jumps into the same MonoBehaviour constructor chain used by the already-qualified constructor recoveries (`0x1812E22A0`). The managed field at this offset is `System.Int32 Window_T::T`.

Therefore the damaged managed sequence:

```text
ldarg.0
ldc.i8 4294967295
stfld int32 Window_T::T
```

must be recovered as the 32-bit value `-1`:

```text
ldarg.0
ldc.i4.m1
stfld int32 Window_T::T
```

No other part of the 1943-byte managed constructor is authorized to change. In particular, this authority record does not authorize reconstruction, removal, or rewriting of the constructor's large string/array initialization body.

## Runtime selection

On exact-R3 natural runtime candidate `9524b2339ba5a7572a16ae9def8c41ea4128af377d0d68dd9d6b5ad454b480d3`, the prior `Almanac_TalentSystem::.ctor` target is clean and `Window_T::.ctor()` becomes the first remaining Invalid IL in chronological order. This makes `Window_T::.ctor` the next causal MethodDef selected for repair.
