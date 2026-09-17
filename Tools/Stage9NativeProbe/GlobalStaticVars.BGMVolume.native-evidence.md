# GlobalStaticVars::BGMVolume PC native authority

This is a **read-only prefetch evidence record**. It does not promote or patch `GlobalStaticVars::BGMVolume()` by itself. A BGMVolume repair may proceed only after the currently running `Administrator::Update()` exact-R3 gate passes and the new `FIRST_INVALID_IL` independently identifies this method.

## Source authority

- Original PC package: `GodsPVZ_1.0.2.zip`, Google Drive ID `1_5kVGTFmsvwcdRLQ2ah_rtyC6DqvLnf0`.
- ZIP SHA256: `2f08b4e2243e2bd296f3db694b4a2c9656252d76202c80167bdf3340b9f65b48`.
- `GameAssembly.dll` SHA256: `9ebd7ca996a5b03fb4a766f7a2502b660d581ddbbb36af4d7bf2f06a211da39d`.
- `global-metadata.dat` SHA256: `ad992341add498bd1018ac980f42171e0737b0793bf945c8714ad70a445b36b9`.
- PC method-pointer table base: `0x181B82D60`.

The native function attributed below was read directly from that original `GameAssembly.dll`; no generated reference PE is used as native authority.

## Stable managed identity

On the Stage9.1 cumulative managed candidate:

- `GlobalStaticVars` TypeDef contains `BGMVolume()`.
- `GlobalStaticVars::BGMVolume()` MethodDef: `0x06000139`, RID313.
- Managed signature: `float32 GlobalStaticVars::BGMVolume()` (signature blob `00 00 0C`).
- Managed RVA: `0x17758`.
- Managed file offset: `0x15958`.
- Fat header: 12 bytes; code size: 78 bytes; max stack: 2.
- Local signature: `0x110000FA`.
- `GlobalStaticVars::gLawnApp` FieldDef: `0x0400015E`.
- `LawnApp::BGMVolume` FieldDef: `0x04000161`, field signature `float32` (`06 0C`).

The recovered managed body preserves the intended high-level branches, but its null test is stack-type-invalid:

```text
IL_002D: ldsfld    LawnApp GlobalStaticVars::gLawnApp
IL_0032: ldc.i4    0
IL_0037: ceq
IL_0039: ldc.i4.0
IL_003A: ceq
...
IL_0044: brtrue    IL_000B
IL_0049: br        IL_0005
```

A managed object reference and an `int32` constant are fed to `ceq`. The prior exact-R3 Start runtime (run `35212942551`) independently observed:

```text
InvalidProgramException: Invalid IL code in GlobalStaticVars:BGMVolume (): IL_0037: ceq
```

That run counted exactly one `GlobalStaticVars::BGMVolume` Invalid IL occurrence while `Administrator::Update` remained the first blocker.

## Exact PC native body

Using RID313 against the fixed original-PC method-pointer table gives entry:

```text
0x181B83720 = 0x18031AFC0
```

The containing `.pdata` RuntimeFunction is:

- start: `0x18031AFC0`
- end: `0x18031B04A`
- exact size: 138 bytes (`0x8A`)
- native slice SHA256: `2caeb3b868ea5be1e69abc3705506bb260d7cf0bd528a20806aa54514bd037bd`

Relevant original native instructions:

```text
0x18031AFE0  mov rcx, [0x181BA2018]      ; GlobalStaticVars TypeInfo
...
0x18031AFFC  mov rax, [rcx+0xB8]        ; GlobalStaticVars static fields
0x18031B003  cmp qword ptr [rax], 0     ; gLawnApp == null ?
0x18031B007  jne 0x18031B016
0x18031B009  movss xmm0, [0x1815A7AB4]  ; 0.3000000119f
0x18031B011  add rsp, 0x28
0x18031B015  ret
...
0x18031B02B  mov rax, [rcx+0xB8]        ; GlobalStaticVars static fields
0x18031B032  mov rcx, [rax]             ; gLawnApp
0x18031B035  test rcx, rcx
0x18031B038  je 0x18031B044             ; standard IL2CPP null-check failure path
0x18031B03A  movss xmm0, [rcx+0x14]     ; LawnApp.BGMVolume
0x18031B03F  add rsp, 0x28
0x18031B043  ret
```

The float at original-PC VA `0x1815A7AB4` is bytes `9A 99 99 3E`, i.e. approximately `0.3f`.

Therefore the original native source-level semantics are:

```csharp
return GlobalStaticVars.gLawnApp == null
    ? 0.3f
    : GlobalStaticVars.gLawnApp.BGMVolume;
```

The current managed branches already implement that structure. The corruption is specifically the null literal representation used before `ceq`.

## Minimal candidate repair if and only if runtime triage promotes this blocker

At managed `IL_0032` (file offset `0x15996` in the current `ad2743f5...` candidate), replace the 5-byte preimage:

```text
20 00 00 00 00   ; ldc.i4 0
```

with:

```text
14 00 00 00 00   ; ldnull + four nop
```

This preserves every existing branch target and all later IL offsets while changing the `ceq` operands from `(object, int32)` to `(object, object-null)`.

A local read-only preview against the current static-qualified `Administrator.Update` candidate SHA
`ad2743f5fc185ccb3b13bd5b4149db341377aebb016ed9f376f5b52b8ce3329f`
produces candidate SHA
`f1eda3324c04f0ce237846c3ea01ee94b00446997c6a3af8d2d1e6d3ae9ffa15`
with exactly one changed byte and unchanged CLR metadata directory SHA
`733f78b3cafe85511475c3037106bad7ee36224aa30c071d10f07fecfeff273e`.

This preview SHA is **not qualified** and must not be promoted unless the post-Update runtime triage identifies `GlobalStaticVars::BGMVolume()` as the actual next blocker and a dedicated static gate passes.
