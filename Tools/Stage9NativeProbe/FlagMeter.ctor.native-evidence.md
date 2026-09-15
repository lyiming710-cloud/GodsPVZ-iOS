# Stage9.1 FlagMeter::.ctor PC native authority

This authority was prefetched read-only and was subsequently selected by exact-R3 runtime chronology after `Window_T::.ctor()` became clean. Updating this record does not promote any development candidate to the sealed HF55 baseline.

Runtime selection evidence:
- upstream runtime-qualified candidate: `51d3a4ac4132fe8933f034287591ee80a6c884877b3ae4f23e799f8217e40b63`
- exact-R3 run: `34961960130`
- `Window_T::.ctor` FieldAccess/InvalidIL/MissingMethod: all `0`
- five SkillProgress phases: pass
- first remaining Invalid IL: `FlagMeter::.ctor()`, line `20572`, failing at `IL_0016: stfld 0x04000811`

Locked PC authority:
- `GameAssembly.dll` SHA256: `9ebd7ca996a5b03fb4a766f7a2502b660d581ddbbb36af4d7bf2f06a211da39d`
- Method-pointer table base: `0x181B82D60`
- Type: `FlagMeter`, TypeDef `0x020000BA`, RID 186
- Method: `.ctor()`, MethodDef `0x0600061B`, RID 1563
- Managed RVA: `0x860F0`
- Managed code size: 52 bytes; locals: 2
- Damaged field: `System.Int32 FlagMeter::theFlagID`, FieldDef `0x04000811`
- Pointer entry: `0x181B85E30`
- PC native range: `0x18039D230–0x18039D2AE` (126 bytes)
- Native slice SHA256: `3b9b04d203dad93fff7dcfa459f4b17cd9bd7f779d7418c9c2d6e5b4ca5d1fee`

The PC constructor contains a 32-bit sentinel write equivalent to `mov dword ptr [this+0x34], 0xffffffff`, then constructs/stores its `List<UnityEngine.GameObject>` field and proceeds through the MonoBehaviour constructor chain. The damaged managed ctor contains `ldc.i8 4294967295` before `stfld int32 FlagMeter::theFlagID`.

The authorized minimal recovery is therefore only:

```text
ldc.i8 4294967295
->
ldc.i4.m1
```

The remaining constructor body, list initialization, field metadata, base-constructor behavior and assembly references must remain unchanged. Static qualification must prove one changed MethodDef, zero FieldDef drift and zero reference-set drift; exact-R3 natural runtime remains required before the candidate is considered runtime-qualified.
