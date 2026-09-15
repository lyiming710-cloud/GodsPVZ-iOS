# SuppliesInfo::.ctor(int) PC native authority

Read-only prefetch from the original GodsPVZ 1.0.2 PC x86-64 IL2CPP `GameAssembly.dll`. This does not promote SuppliesInfo as the next blocker and does not modify the sealed HF55 baseline.

## Source provenance

- PC package: Google Drive `GodsPVZ_1.0.2.zip`, file ID `1_5kVGTFmsvwcdRLQ2ah_rtyC6DqvLnf0`
- PC package SHA256: `2f08b4e2243e2bd296f3db694b4a2c9656252d76202c80167bdf3340b9f65b48`
- `GameAssembly.dll` SHA256: `9ebd7ca996a5b03fb4a766f7a2502b660d581ddbbb36af4d7bf2f06a211da39d`
- Assembly-CSharp method pointer table VA: `0x181B82D60`

The same direct-binary parser previously reproduced the qualified SuppliesInitialValue ctor native span and SHA exactly before being used here.

## Managed identity

- Type: `SuppliesInfo`
- TypeDef token: `0x02000034`
- TypeDef RID: `52`
- Method: `SuppliesInfo::.ctor(int32 id)`
- MethodDef token: `0x06000150`
- MethodDef RID: `336`
- Managed RVA: `0x186B4`
- Managed code size: `74` bytes
- Managed locals: `6`

The damaged managed body contains fake unmanaged-load scaffolding, a width-invalid `ldc.i8 4294967295` before `stfld int32 SuppliesInfo::ID`, and the fake diagnostic `Method not found @180302170`.

## PC native mapping

For RID 336:

`0x181B82D60 + (336 - 1) * 8 = 0x181B837D8`

Direct reading from the original `GameAssembly.dll` gives:

- Pointer-table entry VA: `0x181B837D8`
- Native method VA: `0x180325180`
- `.pdata` exact range: `0x180325180–0x1803251E8`
- Native length: `104` bytes
- Native slice SHA256: `8f3ec66297f376a23bbc8330f86db9030203d2f8ba4b3c5a2cbc26c49f1a437a`

## Key native sequence

```text
0x1803251AB mov  rax,[rip+...]        ; cached System.String class
0x1803251B2 lea  rcx,[rbx+0x10]      ; this->name
0x1803251B6 mov  r8,[rax+0xB8]
0x1803251BD mov  rdx,[r8]             ; System.String.Empty
0x1803251C0 mov  [rbx+0x10],rdx
0x1803251C4 call 0x18024F360          ; GC write barrier
0x1803251C9 xor  edx,edx
0x1803251CB mov  dword ptr [rbx+0x18],0xFFFFFFFF ; ID = -1
0x1803251D2 mov  rcx,rbx
0x1803251D5 call 0x180302170          ; System.Object::.ctor path
0x1803251DA mov  [rbx+0x18],edi       ; ID = id
0x1803251E7 ret
```

`0x180302170` is independently confirmed as the `System.Object::.ctor` path because the already-qualified `SuppliesInitialValue::.ctor()` native authority calls the same address for its base constructor.

## Recovered semantics

The PC-native constructor is equivalent to the high-level CLR semantics:

```csharp
public SuppliesInfo(int id)
{
    name = string.Empty;
    ID = -1;
    base();
    ID = id;
}
```

The explicit `base()` above is descriptive; normal C# source would express the Object constructor implicitly. There is no native evidence for the damaged managed body's `typeof(string)`/`nint` scaffolding or fake diagnostic string.

## Repair constraint if runtime promotes this blocker

If exact-R3 runtime truth selects `SuppliesInfo::.ctor(int)` as the next causal blocker, rebuild only MethodDef `0x06000150` to the native-authoritative high-level semantics above. Preserve all field metadata and assembly references; use a valid `System.Object::.ctor` MethodRef already present in the assembly; do not retain fake unmanaged-load scaffolding, do not add guards or exception swallowing, and do not promote the resulting candidate without a fresh exact-R3 natural runtime gate.
