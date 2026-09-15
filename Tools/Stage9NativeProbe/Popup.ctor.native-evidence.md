# Popup::.ctor PC native authority

Read-only authority record derived directly from the original GodsPVZ 1.0.2 PC `GameAssembly.dll`. This does not promote any development candidate or alter the sealed HF55 baseline.

## Source provenance

- Original Google Drive PC package: `GodsPVZ_1.0.2.zip`
- Drive file ID: `1_5kVGTFmsvwcdRLQ2ah_rtyC6DqvLnf0`
- `GameAssembly.dll` SHA256: `9ebd7ca996a5b03fb4a766f7a2502b660d581ddbbb36af4d7bf2f06a211da39d`
- Assembly-CSharp method pointer table VA: `0x181B82D60`
- The extraction path is self-checked against the locked Supplies RID604 authority before use.

## Managed identity

- Type: `Popup`
- TypeDef token: `0x020000C4`, RID `196`
- Method: `Popup::.ctor()`
- MethodDef token: `0x06000676`, RID `1654`
- Managed RVA: `0x8B584`
- Field: `Popup::info0_int`
- Field token: `0x04000884`, RID `2180`
- Field type: `System.Int32`
- Damaged body: code size `22`, maxstack `2`, locals `1`, initlocals `true`

Damaged IL:

```text
IL_0000 ldarg.0
IL_0001 ldc.i8 4294967295
IL_000A stfld int32 Popup::info0_int
IL_000F ldarg.0
IL_0010 call instance void UnityEngine.MonoBehaviour::.ctor()
IL_0015 ret
```

## PC native mapping

For RID 1654:

`0x181B82D60 + (1654 - 1) * 8 = 0x181B86108`

Directly reading the original PC binary gives:

- Pointer-table entry VA: `0x181B86108`
- Native method VA: `0x1803A6450`
- No covering `.pdata` RuntimeFunction entry; this is a tiny x64 leaf/tail-call ctor.
- The previous RuntimeFunction ends before this leaf, and the next RuntimeFunction starts at `0x1803A6460`; bytes `0x1803A645E–0x1803A645F` are `INT3` padding.
- Exact native body: `0x1803A6450–0x1803A645E`
- Length: `14` bytes
- Native bytes: `33 d2 c7 41 60 ff ff ff ff e9 42 be f3 00`
- Native slice SHA256: `4a7c3b3f5728880eb795eab8b5ede2a4d9b7ce6ef4de9f0d6b308376245813a3`

Disassembly:

```text
0x1803A6450  xor edx, edx
0x1803A6452  mov dword ptr [rcx+0x60], 0xffffffff
0x1803A6459  jmp 0x1812E22A0
```

`rcx` is the instance pointer and `rdx` is the hidden IL2CPP `MethodInfo*` argument on Windows x64. The native ctor writes a 32-bit `-1` at instance offset `0x60`, then tail-jumps to the same base-constructor path used by the already-verified Almanac leaf ctor.

This directly confirms the intended managed semantics:

```csharp
info0_int = -1;
```

## Repair constraint

A recovery patch may modify only MethodDef `0x06000676`. The CLR-faithful repair is the single opcode/constant-width replacement `ldc.i8 4294967295` → `ldc.i4.m1`, preserving the existing `stfld int32 Popup::info0_int`, the `UnityEngine.MonoBehaviour::.ctor()` call, the existing local metadata, all fields, assembly references, and all other MethodDefs. No guards or exception swallowing are permitted.
