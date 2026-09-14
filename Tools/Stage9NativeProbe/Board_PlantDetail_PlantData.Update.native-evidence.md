# Board_PlantDetail_PlantData.Update PC native authority

Read-only prefetch. This method is not a promoted mutation target until runtime selects it as the first causal gameplay blocker.

Authority: original GodsPVZ 1.0.2 PC x86-64 IL2CPP `GameAssembly.dll`.

- GameAssembly SHA256: `9ebd7ca996a5b03fb4a766f7a2502b660d581ddbbb36af4d7bf2f06a211da39d`
- Assembly-CSharp method pointer table VA: `0x181B82D60`
- Managed MethodDef: `Board_PlantDetail_PlantData.Update()`
- token: `0x060005DF`
- RID: `1503`
- pointer-table entry VA: `0x181B85C50`
- native entry VA: `0x180399ED0`
- next independent function begins at `0x18039A320`
- contiguous method span: `0x180399ED0–0x18039A320` (1104 bytes)
- contiguous span SHA256: `34d236f7fdea1ea1c0db610af7200e0202ce8d84372af0c2312ffbfcc26ca191`

The method is split across three consecutive x64 runtime-function fragments rather than one `.pdata` record:

- `0x180399ED0–0x180399FD6` (262 bytes), SHA256 `7864b668ee708a4bacc42b2c3c5d5b0572ea2134fef8924b68b65bac9612b42f`
- `0x180399FD6–0x18039A025` (79 bytes)
- `0x18039A025–0x18039A317` (754 bytes)

The cold null-throw tail at `0x18039A311` belongs to the method; alignment/int3 padding continues until the next function at `0x18039A320`.

## Managed damage observed before recovery

Current managed fingerprint on the post-BGM cheap candidate:

- token `0x060005DF`, RID `1503`
- code size `943`
- locals `41`
- fingerprint `efe4ecc63b04d9c2f76274cf5b36f60e707021ae5af500f3f98f2bd472118081`
- current runtime first invalid instruction: `IL_0349: ceq`

The `IL_0349` failure is caused by comparing `Time.timeScale` (`float`) against an integer zero produced by Cpp2IL. PC native proves the intended comparison is floating-point zero. However this is not the only reconstruction defect in the managed body: several numeric-to-string paths use mismatched temporary locals. Therefore a future promoted recovery must reconstruct the complete MethodDef semantics, not merely change `ldc.i4 0` to `ldc.r4 0` to silence the JIT.

## Recovered PC semantics

1. If `plant` is not Unity-live, skip plant-stat UI work and continue to animator-speed handling.
2. If `coverHP` is live, set `fillAmount = Max(Min(plant.healthPoint / plant.maxHealthPoint, 1f), 0f)`.
3. If `textHP` is live, display truncated current/max health as `current/max`.
4. If `textATK` is live, call `plant.GetATK()` and display prefix `攻击:` plus the current value.
5. If `textARM` is live, call `plant.GetARM()` and display prefix `护甲:` plus the current value.
6. If `textDEF` is live, call `plant.GetDEF()` and display prefix `防御:` plus the current value.
7. If `animator` is live, compute animation speed as `0f` when `Time.timeScale == 0f`; otherwise `1f / Time.timeScale`, then call `animator.SetFloat("speed", speed)`.

The final branch is explicit in native code:

```text
0x18039A2BD  call  Time.get_timeScale
0x18039A2C2  ucomiss xmm0,xmm6       ; compare with 0.0f
0x18039A2C5  jp    0x18039A2C9       ; unordered/NaN => reciprocal path
0x18039A2C7  je    0x18039A2DC       ; exactly zero => speed stays 0
0x18039A2C9  call  Time.get_timeScale
0x18039A2D0  movss xmm6,[1.0f]
0x18039A2D8  divss xmm6,xmm0         ; 1 / timeScale
0x18039A2DC  ...
0x18039A2F2  call  Animator.SetFloat("speed", speed)
```

This native `ucomiss`/`jp` behavior also establishes that NaN follows the reciprocal path rather than the zero path.

## Recovery constraint

If runtime promotes this method, rebuild the single MethodDef from the PC semantics above with typed numeric locals and exact public Unity/.NET references. Preserve all fields and surrounding methods. Do not treat the first `InvalidProgramException` instruction as proof that the rest of the Cpp2IL body is semantically trustworthy.
