# Stage9.1 System3::.ctor PC native authority (prefetched; not yet selected)

Read-only authority record. Runtime chronology must select this MethodDef before repair; this file does not patch a candidate or alter HF55.

- PC `GameAssembly.dll` SHA256: `9ebd7ca996a5b03fb4a766f7a2502b660d581ddbbb36af4d7bf2f06a211da39d`
- Method-pointer table base: `0x181B82D60`
- Type: `System3`, TypeDef `0x0200000F`
- Method: `.ctor()`, MethodDef `0x060000A2`, RID `162`
- Managed RVA: `0xED24`
- Managed code size: `38`; locals: `2`; InitLocals=true
- Managed target field: `System.Int32 System3::suppliesID`, FieldDef `0x040000BD`
- Managed string field: `System.String System3::suppliesName`, FieldDef `0x040000BE`
- Managed damaged sequence: `ldc.i8 4294967295 -> stfld int32 System3::suppliesID`, followed by String.Empty initialization and `UnityEngine.MonoBehaviour::.ctor()`.

## PC native mapping

- Method pointer entry: `0x181B83268`
- Native range: `0x18030D720–0x18030D779`, 89 bytes
- Native slice SHA256: `8d2257bd236bd71ff8ebe92ec8ed16084e34889b00196f01121db926da60041e`

Authoritative key sequence:

```text
0x18030D745 mov dword ptr [rbx+0x28],0xffffffff  ; suppliesID = -1
0x18030D74C lea rcx,[rbx+0x30]
...
0x18030D761 mov [rbx+0x30],rdx                  ; suppliesName = String.Empty
0x18030D765 call 0x18024F360                    ; GC write barrier
0x18030D76A xor edx,edx
0x18030D774 jmp 0x1812E22A0                     ; MonoBehaviour ctor tail path
```

The PC native semantics therefore match the intended managed initialization `suppliesID = -1; suppliesName = string.Empty; base();` and contain no evidence for widening the Int32 sentinel to Int64.

If a later exact-R3 log selects `System3::.ctor`, the eligible minimal repair is the same strict single-op replacement `ldc.i8 4294967295 -> ldc.i4.m1`, preserving the string initialization, base ctor, locals, metadata and assembly references and requiring a fresh static qualification plus exact-R3 natural runtime pass.
