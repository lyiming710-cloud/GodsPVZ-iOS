# Stage9.1 System0::.ctor PC native authority (prefetched; not yet selected)

Read-only prefetch only. Runtime chronology must select this MethodDef before any repair; this record does not alter HF55.

- PC `GameAssembly.dll` SHA256: `9ebd7ca996a5b03fb4a766f7a2502b660d581ddbbb36af4d7bf2f06a211da39d`
- Method pointer table base: `0x181B82D60`
- TypeDef `System0`: token `0x0200000C`, RID 12
- MethodDef `.ctor()`: token `0x0600007B`, RID 123; managed RVA `0xB81C`; managed code size 138; locals 5
- Pointer entry: `0x181B83130`
- PC native range: `0x18030A030–0x18030A14C`, 284 bytes
- Native SHA256: `713893ee2471f7f22e47ecdfdac1f46f5cf2aab5711a9158d543d5b8cfb85bda`

Managed damage includes two separate `ldc.i8 4294967295 -> stfld int32` sequences:

- `System0::plantID`, FieldDef `0x0400006F`
- `System0::order_check`, FieldDef `0x04000082`

The authoritative PC native constructor contains two distinct 32-bit `-1` stores at the corresponding object offsets (`[this+0x50]` and `[this+0xD0]`), in addition to its normal object/list initialization.

This is explicitly **not** eligible for the reusable single-hit `Stage9Int32MinusOnePatch` driver. If exact-R3 runtime later selects `System0::.ctor`, it needs a target-specific patcher that verifies both native-authorized stores and proves that no other instruction, MethodDef, FieldDef, or assembly reference changes.
