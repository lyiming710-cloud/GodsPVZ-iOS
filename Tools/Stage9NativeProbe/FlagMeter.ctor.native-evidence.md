# Stage9.1 FlagMeter::.ctor PC native authority (prefetched; not yet selected)

Read-only prefetch record only. This file does not select `FlagMeter::.ctor`, patch it, qualify it, or promote any development candidate to the sealed HF55 baseline. Runtime chronology remains authoritative.

- Locked PC `GameAssembly.dll` SHA256: `9ebd7ca996a5b03fb4a766f7a2502b660d581ddbbb36af4d7bf2f06a211da39d`
- Method-pointer table base: `0x181B82D60`
- Type: `FlagMeter`, TypeDef `0x020000BA`, RID 186
- Method: `.ctor()`, MethodDef `0x0600061B`, RID 1563
- Managed RVA: `0x860F0`
- Managed code size: 52 bytes; locals: 2
- Damaged field: `System.Int32 FlagMeter::theFlagID`, FieldDef `0x04000811`
- Pointer entry: `0x181B85E30`
- PC native range: `0x18039D230–0x18039D2AE` (126 bytes)
- Native slice SHA256: `3b9b04d203dad93fff7dcfa459f4b17cd9bd7f779d7418c9c2d6e5b4ca5d1fee`

Targeted native semantics: the PC constructor contains a 32-bit sentinel write equivalent to `mov dword ptr [this+0x34], 0xffffffff`, constructs/stores its `List<UnityEngine.GameObject>` field, and eventually tail-jumps into the same MonoBehaviour constructor chain used by the already-qualified constructor recoveries. The damaged managed ctor contains `ldc.i8 4294967295` before `stfld int32 FlagMeter::theFlagID`.

If and only if a later exact-R3 runtime log selects `FlagMeter::.ctor` as the next causal MethodDef, the authorized minimal recovery for that field is `ldc.i8 4294967295 -> ldc.i4.m1`, with the remaining constructor body preserved exactly and verified by MethodDef/FieldDef/reference isolation and exact-reference decompilation.
