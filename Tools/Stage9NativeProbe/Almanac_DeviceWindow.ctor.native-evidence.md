# Stage9.1 Almanac_DeviceWindow::.ctor PC native authority (prefetched; not yet selected)

Read-only prefetch only; runtime chronology must select this MethodDef before repair.

- PC `GameAssembly.dll` SHA256: `9ebd7ca996a5b03fb4a766f7a2502b660d581ddbbb36af4d7bf2f06a211da39d`
- Pointer table base: `0x181B82D60`
- TypeDef: `Almanac_DeviceWindow` token `0x020000A3`, RID 163
- MethodDef `.ctor()`: token `0x0600056A`, RID 1386; managed RVA `0x72450`; code size 22; locals 1
- Field: `System.Int32 Almanac_DeviceWindow::deviceID`, token `0x040006E6`
- Pointer entry: `0x181B858A8`
- PC native leaf: `0x18037E1C0–0x18037E1CE`, 14 bytes
- Native SHA256: `f45ad37c4c4cf57a18ea2891a76433a467a8c55b6f48907c573db40c236ff6a9`

The 14-byte native body is byte-identical to the already-authoritative `Almanac_ZombieWindow::.ctor` leaf. It writes a 32-bit `-1` sentinel at object offset `0x28` and tail-jumps into the MonoBehaviour constructor chain. The managed damaged body uses `ldc.i8 4294967295` before `stfld int32 Almanac_DeviceWindow::deviceID`.

If a later exact-R3 runtime selects it, the only authorized repair is `ldc.i8 4294967295 -> ldc.i4.m1`, preserving the field store and base constructor call, with full MethodDef/FieldDef/reference isolation.
