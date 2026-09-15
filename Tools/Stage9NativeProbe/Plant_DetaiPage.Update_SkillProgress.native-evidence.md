# Plant_DetaiPage.Update_SkillProgress PC native authority

Read-only prefetch. Do not mutate this MethodDef unless runtime promotes it as the first causal gameplay blocker after `Plant_DetaiPage.Update()`.

Authority: original GodsPVZ 1.0.2 PC x86-64 IL2CPP `GameAssembly.dll`.

- GameAssembly SHA256: `9ebd7ca996a5b03fb4a766f7a2502b660d581ddbbb36af4d7bf2f06a211da39d`
- Assembly-CSharp method pointer table VA: `0x181B82D60`
- Managed MethodDef: `Plant_DetaiPage.Update_SkillProgress()`
- Token: `0x06000664`
- RID: `1636`
- Managed RVA on the current reconstructed assembly: `0x0008AA44`
- Managed damaged code size: `2481` bytes
- Managed damaged locals: `96`
- Pointer-table entry VA: `0x181B86078`
- Native method VA: `0x1803A4BB0`
- `.pdata` exact range: `0x1803A4BB0–0x1803A573B`
- Native span length: `2955` bytes
- Native span SHA256: `5050f85b69db4e37dcf86232a9c388ac32e4b622972556a8bfa4340cf3f8dbe4`

## Why the managed body cannot be trusted as-is

Exact-reference ILSpy on the current development candidate reports many independent reconstruction failures throughout the method, including `Unknown result type`, reference/value mismatches, invalid array element lowering, and several `System.Object` temporaries standing in for value types.

The first managed corruption is already structural:

```text
IL_0005 ldarg.0
IL_0006 ldfld Skill Plant_DetaiPage::p_skill
IL_000b ldc.i4 72
IL_0010 add
```

This incorrectly treats the `Skill` reference as an integer address. PC native instead reads `p_skill`, forms the address of its field at offset `0x48`, and performs the real typed operation. The corresponding native opening is:

```text
0x1803A4C09 mov rcx,[rbx+0x30]    ; p_skill
0x1803A4C0D mov rdi,[rbx+0xA8]    ; text_chargeLayer
0x1803A4C1C test rcx,rcx
0x1803A4C25 add  rcx,0x48         ; address of typed Skill field
0x1803A4C2B call 0x180CA3820      ; typed value-to-string path
```

The damaged managed body is 2481 bytes / 96 locals and contains failures far beyond the first instruction. Therefore, if promoted, recover the complete single MethodDef from PC native semantics rather than repairing individual invalid IL diagnostics.

## Recovery constraint

- Preserve original field metadata and access modifiers.
- Do not replace private/internal data with public fields.
- Do not suppress exceptions or add defensive guards just to make Mono execute.
- Reconstruct one MethodDef only, with typed locals and exact existing Unity/.NET references.
- Run static semantic isolation and exact-reference readback before one exact-R3 runtime gate.
