# Stage9.1 Administrator::.ctor PC native authority (prefetched; not yet selected)

Read-only authority record. Runtime chronology must select this MethodDef before any repair. This file does not patch a candidate and does not alter the sealed HF55 baseline.

## Source lock

- PC GodsPVZ 1.0.2 `GameAssembly.dll` SHA256: `9ebd7ca996a5b03fb4a766f7a2502b660d581ddbbb36af4d7bf2f06a211da39d`
- Assembly-CSharp method-pointer table base: `0x181B82D60`
- The direct-binary parser was previously self-calibrated against the already-qualified Supplies native span/SHA before this prefetch chain.

## Managed identity on qualified development chain

Read-only Cecil probe against candidate `70ab5511b293d87d69caadc42a029bbe08b09d6b60c1ee8af443686bc481a4da`:

- Type: `Administrator`, TypeDef `0x02000002`
- Method: `.ctor()`, MethodDef `0x0600000C`, RID `12`
- Managed RVA: `0x2B28`
- Managed code size: `191`
- Managed locals: `7`, `InitLocals=true`
- Relevant damaged managed sequences include:
  - `ldc.i8 4294967295 -> stfld int32 Administrator::mode` (`0x0400000A`)
  - `ldc.i8 4294967295 -> stfld int32 System3::suppliesID` (`0x040000BD`) while constructing/initializing the Administrator-owned `System3` object
- The ctor also creates and attaches multiple subsystem objects/lists and calls `UnityEngine.MonoBehaviour::.ctor()`.

## PC native mapping

RID 12 maps to pointer-table entry:

`0x181B82D60 + (12 - 1) * 8 = 0x181B82DB8`

Direct reading of the locked original PC binary gives:

- Pointer entry: `0x181B82DB8`
- Native range: `0x1802FEB60–0x1802FED13`
- Native length: `435` bytes
- Native SHA256: `0fc07b9a066f57d91a828c58f90fca975de1e77bebf89d15b1f404c7b3f51da5`

Key native behavior observed in the authoritative slice:

```text
0x1802FEBCB call 0x1808053D0       ; initialize allocated subsystem object
...
0x1802FEC3C call 0x1808053D0       ; initialize another allocated subsystem object
0x1802FEC62 ...                     ; store String.Empty into that object's string field
0x1802FEC74 call 0x1812E22A0       ; MonoBehaviour ctor chain for child object
...
0x1802FECB4 mov dword ptr [rbx+0x28],0xffffffff
0x1802FECD0 ...                     ; store String.Empty into child string field
0x1802FECDE call 0x1812E22A0       ; MonoBehaviour ctor chain for child object
...
0x1802FECF5 mov dword ptr [rdi+0x60],0xffffffff
0x1802FED0E jmp 0x1812E22A0        ; Administrator base ctor tail path
```

The two native `-1` writes are **not two fields on the same object**: one belongs to a newly allocated child/subsystem object, while the final `+0x60` store belongs to the Administrator instance. This agrees with the managed damaged body referring to both `System3::suppliesID` and `Administrator::mode`.

## Repair constraint if runtime selects Administrator::.ctor

Do **not** use `Stage9DualInt32MinusOnePatch`: that tool intentionally requires two target Int32 fields on the same declaring type and is appropriate for `System0::.ctor`, not Administrator.

If a later exact-R3 runtime promotes `Administrator::.ctor` as the next causal blocker, perform a target-specific reconstruction/repair that preserves the ctor's subsystem allocations, field ownership, String.Empty initialization, base-constructor ordering, MethodDef/FieldDef metadata and assembly references. The native slice above is the authority; no guard insertion, exception swallowing, private-field widening, or blanket substitution is authorized.
