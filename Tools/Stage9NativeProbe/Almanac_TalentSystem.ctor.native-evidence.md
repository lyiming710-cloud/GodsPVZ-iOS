# Almanac_TalentSystem::.ctor PC native authority

Selected causal blocker after Popup exact-R3 runtime qualification. This record is derived directly from the original GodsPVZ 1.0.2 PC `GameAssembly.dll`; it does not promote the sealed HF55 baseline.

## Source provenance
- Original Google Drive PC package: `GodsPVZ_1.0.2.zip`, Drive ID `1_5kVGTFmsvwcdRLQ2ah_rtyC6DqvLnf0`
- `GameAssembly.dll` SHA256: `9ebd7ca996a5b03fb4a766f7a2502b660d581ddbbb36af4d7bf2f06a211da39d`
- Assembly-CSharp method-pointer table VA: `0x181B82D60`
- Extraction path self-checks against the locked Supplies RID604 authority.

## Runtime selection
Popup exact-R3 run `34958115564` passed with candidate `805a177be099c923daf93a93d98d461312c76711f112c8f778800a69f5874253`. Its new `playmode.log` has the first remaining Invalid IL at line 20420:

`InvalidProgramException: Invalid IL code in Almanac_TalentSystem:.ctor (): IL_0046: stfld 0x0400073d`

Thus this MethodDef is the next causal blocker.

## Managed identity
- Type: `Almanac_TalentSystem`, TypeDef token `0x020000A7`, RID167
- Method: `.ctor()`, MethodDef token `0x0600059D`, RID1437, managed RVA `0x7B2BC`
- body: code size107, locals4, initlocals=true
- field: `previewID`, token `0x0400073D`, type `System.Int32`
- damaged sequence: `ldc.i8 4294967295` at IL_003D immediately followed by `stfld int32 Almanac_TalentSystem::previewID` at IL_0046
- the remaining observed ctor operations use normal public generic List constructors for `talentNames`, `select`, and `buttons` and the existing `UnityEngine.MonoBehaviour::.ctor()` call.

## PC native mapping
- RID1437 pointer-table entry: `0x181B85A40`
- native pointer: `0x180391AC0`
- `.pdata` range: `0x180391AC0–0x180391BCA`
- native length: 266 bytes
- native slice SHA256: `cd8b7cd491c67f65e18def6efc23352a030a43a97862afc6c2eea7c7c5b46645`

The native ctor performs the three generic-list allocations/stores and ends with:

```text
0x180391BAF  xor edx, edx
0x180391BB1  mov dword ptr [rdi+0x64], 0xffffffff
0x180391BB8  mov rcx, rdi
...
0x180391BC5  jmp 0x1812E22A0
```

The write is explicitly 32-bit `-1`; the tail jump is the same MonoBehaviour base-constructor target already established by Almanac_ZombieWindow and Popup authority records. This directly confirms the intended managed assignment `previewID = -1` and contradicts the damaged managed I8 width.

## Repair constraint
Only MethodDef `0x0600059D` may change. The repair is a single `ldc.i8 4294967295` → `ldc.i4.m1` replacement immediately before `stfld int32 Almanac_TalentSystem::previewID`. All List construction, locals, exception-handler state, fields, assembly references, and every other MethodDef must remain unchanged. No guards or exception swallowing are permitted.
