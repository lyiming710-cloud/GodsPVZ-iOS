# Administrator::Start PC native authority — switch prefetch

Read-only prefetch evidence only. This file does not mutate any managed candidate, does not select `Administrator::Start` ahead of runtime chronology, and does not promote the sealed HF55 baseline.

## Source authority

- Original PC package: `GodsPVZ_1.0.2.zip`, Drive ID `1_5kVGTFmsvwcdRLQ2ah_rtyC6DqvLnf0`
- ZIP SHA256: `2f08b4e2243e2bd296f3db694b4a2c9656252d76202c80167bdf3340b9f65b48`
- `GameAssembly.dll` SHA256: `9ebd7ca996a5b03fb4a766f7a2502b660d581ddbbb36af4d7bf2f06a211da39d`
- method-pointer table base `0x181B82D60`

## Stable managed identity

- `Administrator` TypeDef `0x02000002`, RID2
- `Administrator::Start()` MethodDef `0x06000004`, RID4
- `Administrator::mode` FieldDef `0x0400000A`

On the inspected managed recovery body, the method contains a decompiler artifact after `mode + 1` range checking:

```text
ldc.i8 6442450944                         // 0x180000000
...
"Unmanaged memory load: [...+2FE968+...*4]"
...
conv.i
ldc.i8 6442450944
add
...
"Indirect jump: ... (should have been resolved before IL gen)"
```

This is not source-level pointer arithmetic. The original PC native body identifies it as a normal compiler switch jump table.

## Exact native body

For RID4:

- method pointer entry: `0x181B82D78`
- direct pointer: `0x1802FDD20`
- `.pdata` RuntimeFunction: `0x1802FDD20–0x1802FE980`
- body size: 3168 bytes
- exact native SHA256: `3d83a514d37c18b7d91df7ceba17803bb0ac3cbd670dab15e823af4be8b58d02`

The switch dispatch begins:

```text
0x1802FDE1B  mov eax, dword ptr [rdi+0x60]    ; mode
0x1802FDE1E  inc eax                           ; mode + 1
0x1802FDE20  cmp eax, 5
0x1802FDE23  ja  0x1802FE803                  ; out of table range
0x1802FDE29  cdqe
0x1802FDE2B  lea rdx, [0x180000000]
0x1802FDE32  mov ecx, dword ptr [rdx+rax*4+0x2FE968]
0x1802FDE39  add rcx, rdx
0x1802FDE3C  jmp rcx
```

Directly reading six `uint32` table entries from VA `0x1802FE968` gives:

| index (`mode+1`) | RVA | target VA |
|---:|---:|---:|
| 0 | `0x002FDE3E` | `0x1802FDE3E` |
| 1 | `0x002FDFF1` | `0x1802FDFF1` |
| 2 | `0x002FE185` | `0x1802FE185` |
| 3 | `0x002FE291` | `0x1802FE291` |
| 4 | `0x002FE3EF` | `0x1802FE3EF` |
| 5 | `0x002FE5F4` | `0x1802FE5F4` |

Therefore the six table cases correspond to `mode = -1, 0, 1, 2, 3, 4`; values where unsigned `mode+1 > 5` flow to `0x1802FE803`.

Each target contains ordinary managed/Unity behavior. None of the native evidence supports retaining an unmanaged load, an image-base integer, `conv.i`, or an indirect-jump diagnostic in recovered managed IL.

## Future repair constraint

If a later exact-R3 runtime selects `Administrator::Start()` as `FIRST_INVALID_IL`, repair must reconstruct this switch as managed control flow using the six native-authoritative targets while preserving the surrounding Unity calls, null semantics, fields, and metadata. Do not use pointer arithmetic, indirect jumps, blanket guards, or exception swallowing as a shortcut.

Because `Administrator::Start` is not the currently selected blocker, this evidence intentionally stops before creating a managed patch candidate.
