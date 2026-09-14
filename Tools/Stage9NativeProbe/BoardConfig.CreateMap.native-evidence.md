# BoardConfig.CreateMap PC native evidence

Authority: original PC GodsPVZ 1.0.2 `GameAssembly.dll` SHA256 `9ebd7ca996a5b03fb4a766f7a2502b660d581ddbbb36af4d7bf2f06a211da39d`.

Managed target on runtime-qualified 5695 candidate:
- Method: `BoardConfig.CreateMap()`
- MethodDef token: `0x06000157`
- RID: `343`
- damaged managed RVA: `0x19564`
- damaged managed body: 53 bytes / 3 locals
- first causal invalid IL: `IL_000B: ceq` after `ldfld BoardConfig::map` and `ldc.i4 0`.

PC method pointer attribution:
- Assembly-CSharp CodeGenModule method-pointer table VA: `0x181B82D60`
- RID formula: `0x181B82D60 + (343 - 1) * 8 = 0x181B83810`
- pointer-table entry: `0x181B83810`
- native method VA: `0x18030EBB0`
- this method is a leaf function and has no `.pdata` unwind entry.
- exact native leaf bytes: 21
- bytes: `48 83 79 30 00 74 0B 48 8B 49 30 33 D2 E9 2E CD 01 00 33 C0 C3`
- exact leaf SHA256: `c45928899d11a9a908c583d4190b11c29d70a4fccf8b7c2eb8cadb93da620549`

Disassembly:
```text
18030ebb0  cmp qword ptr [rcx+30h], 0
18030ebb5  je  18030ebc2
18030ebb7  mov rcx, qword ptr [rcx+30h]
18030ebbb  xor edx, edx
18030ebbd  jmp 18032b8f0
18030ebc2  xor eax, eax
18030ebc4  ret
```

Field attribution and semantics:
- `BoardConfig.map` is the managed field used by the damaged method and corresponds to native object offset `+0x30`.
- the managed damaged body already identifies the live-path callee as `Map.Copy()`.
- PC semantics are exactly: `return map == null ? null : map.Copy();`
- the recovery therefore removes only the invalid object-vs-int `ceq` lowering and preserves the existing private `map` field metadata unchanged.
