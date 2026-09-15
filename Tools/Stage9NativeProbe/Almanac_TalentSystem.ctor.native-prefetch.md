# Almanac_TalentSystem::.ctor PC native prefetch

Read-only prefetch from the original GodsPVZ 1.0.2 PC `GameAssembly.dll`. This file is preparation only: it does not select this MethodDef as the next blocker, patch any candidate, or promote the sealed HF55 baseline.

## Locked source
- Original Google Drive PC package: `GodsPVZ_1.0.2.zip`, Drive ID `1_5kVGTFmsvwcdRLQ2ah_rtyC6DqvLnf0`
- `GameAssembly.dll` SHA256: `9ebd7ca996a5b03fb4a766f7a2502b660d581ddbbb36af4d7bf2f06a211da39d`
- Assembly-CSharp method-pointer table VA: `0x181B82D60`
- Extraction path self-check: locked Supplies RID604 authority.

## Managed identity on Popup-qualified candidate `805a177...`
- Type `Almanac_TalentSystem`: token `0x020000A7`, RID167
- `.ctor()`: token `0x0600059D`, RID1437, managed RVA `0x7B2BC`
- damaged body: code size107, locals4, initlocals=true
- relevant damaged IL: `ldc.i8 4294967295` at IL_003D immediately followed by `stfld int32 Almanac_TalentSystem::previewID` token `0x0400073D`
- other observed ctor operations use ordinary public generic List constructors for `talentNames`, `select`, and `buttons`.

## PC native mapping
- RID1437 entry: `0x181B85A40`
- pointer: `0x180391AC0`
- `.pdata` range: `0x180391AC0–0x180391BCA`
- native length: 266 bytes
- native slice SHA256: `cd8b7cd491c67f65e18def6efc23352a030a43a97862afc6c2eea7c7c5b46645`

The native ctor allocates/stores three generic-list instances, then ends with:

```text
0x180391BAF  xor edx, edx
0x180391BB1  mov dword ptr [rdi+0x64], 0xffffffff
0x180391BB8  mov rcx, rdi
...
0x180391BC5  jmp 0x1812E22A0
```

This directly establishes a 32-bit `-1` field initialization at instance offset `0x64`, followed by the same MonoBehaviour base-constructor tail-call target already observed in the Almanac_ZombieWindow and Popup PC authorities. It is consistent with the managed `System.Int32 Almanac_TalentSystem::previewID` assignment and contradicts the damaged managed `ldc.i8` width.

If later runtime chronology selects this ctor as the next causal blocker, the minimal repair candidate should be evaluated as a single `ldc.i8 4294967295` → `ldc.i4.m1` replacement while preserving every other instruction and metadata item; static and exact-R3 gates are still required before qualification.
