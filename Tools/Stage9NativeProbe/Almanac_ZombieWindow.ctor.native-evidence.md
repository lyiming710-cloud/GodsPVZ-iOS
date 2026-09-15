# Almanac_ZombieWindow::.ctor PC native authority

Direct binary read from the original GodsPVZ 1.0.2 PC package stored in Google Drive. This is a read-only authority record; it does not promote any candidate or alter the sealed HF55 baseline.

## Source provenance

- Google Drive file: `GodsPVZ_1.0.2.zip`
- Google Drive file ID: `1_5kVGTFmsvwcdRLQ2ah_rtyC6DqvLnf0`
- ZIP size: `242474120` bytes
- ZIP SHA256: `2f08b4e2243e2bd296f3db694b4a2c9656252d76202c80167bdf3340b9f65b48`
- Embedded `GameAssembly.dll` size: `31969792` bytes
- `GameAssembly.dll` SHA256: `9ebd7ca996a5b03fb4a766f7a2502b660d581ddbbb36af4d7bf2f06a211da39d`
- Embedded `global-metadata.dat` SHA256: `ad992341add498bd1018ac980f42171e0737b0793bf945c8714ad70a445b36b9`

The `GameAssembly.dll` SHA exactly matches the previously locked PC native authority.

## Parser self-check against SuppliesInitialValue::.ctor

Before reading Almanac, the same directly downloaded `GameAssembly.dll` was used to reproduce the already-qualified Supplies native evidence:

- Supplies MethodDef RID: `604`
- Assembly-CSharp method pointer table VA: `0x181B82D60`
- Pointer-table entry VA: `0x181B84038`
- Pointer value read from the binary: `0x180341320`
- `.pdata` range: `0x180341320–0x18034146C`
- Range size: `332` bytes
- Native slice SHA256: `3688ac9e8b1ca81c33bb128a816e2a5ab536fd6e48126a5e093749dfd964c6f8`

All values exactly match the existing Supplies evidence. This validates the VA-to-file-offset, method-pointer-table and `.pdata` parsing path used below.

## Managed identity

- Type: `Almanac_ZombieWindow`
- TypeDef token: `0x020000E6`
- TypeDef RID: `230`
- Method: `Almanac_ZombieWindow::.ctor()`
- MethodDef token: `0x060005BA`
- MethodDef RID: `1466`
- Managed RVA: `0x7DC24`
- Field: `Almanac_ZombieWindow::zombieID`
- Field token: `0x04000768`
- Field RID: `1896`
- Field type: `System.Int32`

Damaged managed body uses `ldc.i8 4294967295` immediately before storing into the `System.Int32` field, which produces the known verifier/runtime Invalid IL condition.

## PC native mapping

Assembly-CSharp method pointer table base:

`0x181B82D60`

For RID 1466:

`0x181B82D60 + (1466 - 1) * 8 = 0x181B85B28`

Directly reading the 8-byte entry from the original `GameAssembly.dll` gives:

- Pointer-table entry VA: `0x181B85B28`
- Native method VA: `0x18037E1C0`

`0x18037E1C0` is not covered by a `.pdata` RuntimeFunction entry. This is consistent with the function being a tiny x64 leaf/tail-call function requiring no unwind record.

The preceding RuntimeFunction ends at `0x18037E1B9`; bytes `0x18037E1B9–0x18037E1BF` are `INT3` padding. The next function begins at `0x18037E1D0`; bytes `0x18037E1CE–0x18037E1CF` are also `INT3` padding. Therefore the exact native function body is:

- Native range: `0x18037E1C0–0x18037E1CE`
- Native length: `14` bytes
- Native bytes: `33 d2 c7 41 28 ff ff ff ff e9 d2 40 f6 00`
- Native slice SHA256: `f45ad37c4c4cf57a18ea2891a76433a467a8c55b6f48907c573db40c236ff6a9`

## Native disassembly

```text
0x18037E1C0  xor edx, edx
0x18037E1C2  mov dword ptr [rcx+0x28], 0xffffffff
0x18037E1C9  jmp 0x1812E22A0
```

On Windows x64 IL2CPP instance methods, `rcx` is the object instance and `rdx` is the hidden `MethodInfo*` argument. The function clears the hidden method-info argument, writes a 32-bit `-1` to the instance field at offset `0x28`, then tail-jumps into the base-constructor path.

This directly confirms the intended value and width of the managed assignment:

```csharp
zombieID = -1;
```

Because `zombieID` is `System.Int32`, the CLR-faithful managed repair is to load an I4 `-1` (`ldc.i4.m1`) before `stfld int32 Almanac_ZombieWindow::zombieID`, while preserving the existing `UnityEngine.MonoBehaviour::.ctor()` base-constructor call and all metadata.

## Repair constraint

A subsequent patcher may change only `Almanac_ZombieWindow::.ctor()` MethodDef `0x060005BA` and should minimally replace the invalid I8 constant with the native-authoritative I4 `-1`. It must not add guards, exception swallowing, field metadata changes, assembly-reference changes, or silently promote the resulting candidate to the sealed baseline.
