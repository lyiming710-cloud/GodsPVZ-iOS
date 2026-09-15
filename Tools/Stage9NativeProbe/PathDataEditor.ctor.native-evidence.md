# PathDataEditor::.ctor PC native authority

Read-only prefetch from the original GodsPVZ 1.0.2 PC `GameAssembly.dll`; this does not promote or patch the method.

- PC ZIP Drive ID: `1_5kVGTFmsvwcdRLQ2ah_rtyC6DqvLnf0`
- PC ZIP SHA256: `2f08b4e2243e2bd296f3db694b4a2c9656252d76202c80167bdf3340b9f65b48`
- GameAssembly SHA256: `9ebd7ca996a5b03fb4a766f7a2502b660d581ddbbb36af4d7bf2f06a211da39d`
- Method: `PathDataEditor::.ctor()`
- MethodDef token: `0x0600005C`, RID `92`, managed RVA `0x8B48`
- Managed damaged body: code size `22`, one preserved local `PathDataEditor`; `ldc.i8 4294967295` is stored into `int32 PathDataEditor::pathIndex`, followed by `UnityEngine.MonoBehaviour::.ctor()`.
- Assembly-CSharp method pointer table: `0x181B82D60`
- Pointer entry: `0x181B83038`
- Native VA: `0x1803061E0`
- No `.pdata` RuntimeFunction: tiny leaf/tail-call ctor.
- Previous RuntimeFunction ends `0x1803061D7`; next starts `0x1803061F0`; INT3 padding locks exact body `0x1803061E0–0x1803061EE`.
- Native bytes: `33 d2 c7 41 38 ff ff ff ff e9 b2 c0 fd 00`
- Native length: `14` bytes
- Native SHA256: `4ad673a89e631b0de2f3d08ae0d0cadeac3ad6bf354f6d7bf3f9ac84e46786e0`

```text
0x1803061E0 xor edx,edx
0x1803061E2 mov dword ptr [rcx+0x38],0xffffffff
0x1803061E9 jmp 0x1812E22A0
```

Native semantics are `pathIndex = -1` as a 32-bit store followed by the same MonoBehaviour base-constructor tail path used by the Almanac leaf ctor. If runtime truth promotes this method, the CLR-faithful repair is the same single-opcode replacement `ldc.i8 4294967295 -> ldc.i4.m1`, preserving the existing local, field metadata, `stfld`, base ctor call and all assembly references.
