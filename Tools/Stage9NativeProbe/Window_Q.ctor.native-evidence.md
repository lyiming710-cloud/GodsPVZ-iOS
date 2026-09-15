# Stage9.1 Window_Q::.ctor PC native authority (prefetched; not yet selected)

Read-only prefetch only; runtime chronology must select this MethodDef before repair.

- PC `GameAssembly.dll` SHA256: `9ebd7ca996a5b03fb4a766f7a2502b660d581ddbbb36af4d7bf2f06a211da39d`
- Pointer table base: `0x181B82D60`
- TypeDef: `Window_Q` token `0x020000D4`, RID 212
- MethodDef `.ctor()`: token `0x06000707`, RID 1799; managed RVA `0x969FC`; code size 22; locals 1
- Field: `System.Int32 Window_Q::info0_int`, token `0x04000909`
- Pointer entry: `0x181B86590`
- PC native leaf: `0x1803B0590–0x1803B059E`, 14 bytes
- Native SHA256: `335bacd138f91ca7ee7cfd02b07f67a0392d938250f9e1fe5977f72f5cc63eda`
- Native bytes: `33 d2 c7 41 68 ff ff ff ff e9 02 1d f3 00`

The native leaf writes a 32-bit `-1` sentinel at object offset `0x68` and tail-jumps into the MonoBehaviour constructor chain. The managed damaged body uses `ldc.i8 4294967295` before `stfld int32 Window_Q::info0_int`.

If a later exact-R3 runtime selects it, the only authorized repair is `ldc.i8 4294967295 -> ldc.i4.m1`, preserving the field store and base constructor call, with full MethodDef/FieldDef/reference isolation.
