# Plant_DetaiPage.Update_SkillProgress PC native authority

Read-only authority evidence. Do not mutate this MethodDef until runtime coverage explicitly promotes it.

Authority: original GodsPVZ 1.0.2 PC x86-64 IL2CPP `GameAssembly.dll`.

- GameAssembly SHA256: `9ebd7ca996a5b03fb4a766f7a2502b660d581ddbbb36af4d7bf2f06a211da39d`
- Assembly-CSharp method pointer table VA: `0x181B82D60`
- Managed MethodDef: `Plant_DetaiPage.Update_SkillProgress()`
- Token: `0x06000664`
- RID: `1636`
- Managed damaged code size: `2481` bytes
- Managed damaged locals: `96`
- Pointer-table entry VA: `0x181B86078`
- Native method VA: `0x1803A4BB0`
- `.pdata` exact range: `0x1803A4BB0–0x1803A573B`
- Native span length: `2955` bytes
- Native span SHA256: `5050f85b69db4e37dcf86232a9c388ac32e4b622972556a8bfa4340cf3f8dbe4`

## Object/field layout used by the native method

The function repeatedly loads the same object fields, which fixes the relevant native layout for this recovery:

- `Plant_DetaiPage +0x28` = `plant`
- `Plant_DetaiPage +0x30` = `p_skill`
- `Plant_DetaiPage +0x58` = `skillLogo`
- `Plant_DetaiPage +0x80` = `skillChargeTexts`
- `Plant_DetaiPage +0xA8` = `text_chargeLayer`
- `Skill +0x28` = `ChargeType`
- `Skill +0x38` = `duration`
- `Skill +0x40` = `chargeTicking`
- `Skill +0x44` = `maxChargeTicking`
- `Skill +0x48` = `chargedLayer`
- `Plant +0xE8` = `skillOngoing`
- `Plant +0xEC` = `skillRemainTime`

## Opening and charged-layer text

The first managed corruption is structural:

```text
IL_0005 ldarg.0
IL_0006 ldfld Skill Plant_DetaiPage::p_skill
IL_000b ldc.i4 72
IL_0010 add
```

This incorrectly treats the `Skill` reference as an integer address. PC native instead takes the address of the typed field at `Skill+0x48` and converts that value to text:

```text
0x1803A4C09 mov rcx,[rbx+0x30]    ; p_skill
0x1803A4C0D mov rdi,[rbx+0xA8]    ; text_chargeLayer
0x1803A4C1C test rcx,rcx
0x1803A4C25 add  rcx,0x48         ; &p_skill.chargedLayer
0x1803A4C2B call 0x180CA3820      ; typed value-to-string path
0x1803A4C45 call 0x1811AF070      ; TMP text update path
```

## Ready branch

The first state split is exact:

```text
0x1803A4C57 cmp dword ptr [p_skill+0x48],0 ; chargedLayer
0x1803A4C5B jle 0x1803A4EF3                ; no charged layer -> normal path
0x1803A4C6E cmp byte ptr [plant+0xE8],0    ; skillOngoing
0x1803A4C75 jne 0x1803A4EF3                ; ongoing -> normal/ongoing dispatcher
```

When `chargedLayer > 0 && !skillOngoing`, native executes the Ready UI path `0x1803A4C7B–0x1803A4EF2` and returns. It toggles four skill-logo objects, writes the Ready text/color state, toggles the three charge-text objects, then returns without entering the charging/ongoing paths.

## Non-ongoing dispatcher: active charge vs passive

The normal dispatcher begins at `0x1803A4EF3`. It tests `plant.skillOngoing` again:

```text
0x1803A4F00 cmp byte ptr [plant+0xE8],0
0x1803A4F07 jne 0x1803A540E               ; ongoing branch
```

For non-ongoing state, the charge-type split is:

```text
0x1803A5033 cmp dword ptr [p_skill+0x28],3 ; ChargeType.Passively
0x1803A503B je  0x1803A5199                ; passive path
```

Therefore `ChargeType == 3` is the passive branch; all other values follow the active-charge path.

### Active charge

At `0x1803A5071–0x1803A5081`, native computes and stores the charge-ring ratio directly as:

```text
p_skill.chargeTicking (+0x40) / p_skill.maxChargeTicking (+0x44)
```

The same branch formats the current and maximum charge values for the charge text array before joining the common complementary-ring path at `0x1803A5284`.

### Passive

The passive branch begins at `0x1803A5199`. It sets the charging ring fill to zero and writes the passive `- / -` text state before joining the same common path at `0x1803A5284`.

## CLR adaptation for the complementary fill

The common non-ongoing path at `0x1803A5284` reads the stored Unity `Image` fill value and writes the complementary ring. Native performs a direct IL2CPP object-layout read:

```text
0x1803A52D2 movss xmm1,[1.0 constant]
0x1803A52DD subss xmm1,dword ptr [image+0xFC]
0x1803A52E8 call 0x1813D7750              ; Image fill setter path
```

Cpp2IL lifted this internal layout read as private `UnityEngine.UI.Image.m_FillAmount`, which is illegal from reconstructed managed code. CLR recovery must preserve the semantic operation `1f - image.fillAmount` through the public `Image.fillAmount` getter rather than accessing `m_FillAmount` or changing Unity field metadata.

## Ongoing branch

The ongoing branch begins at `0x1803A540E`. It hides the charge texts, selects the duration logo state, and computes the duration ring at `0x1803A55A9–0x1803A55BC`:

```text
plant.skillRemainTime (+0xEC) / p_skill.duration (+0x38)
```

It then splits on the remaining charged layer count:

```text
0x1803A55CE cmp dword ptr [p_skill+0x48],1
0x1803A55D6 jg  0x1803A5677                ; chargedLayer > 1
```

- `chargedLayer <= 1`: path `0x1803A55DC–0x1803A5672`
- `chargedLayer > 1`: path `0x1803A5677–0x1803A56F9`
- both converge at `0x1803A5700`, set the selected ring fill to native constant `1.0f`, and return at `0x1803A572E`.

## Why the complete MethodDef must be rebuilt

Exact-reference ILSpy on the current development candidate reports independent corruption across the body: reference/value mismatches, invalid array-element lowering, multiple `System.Object` temporaries standing in for value types, the object-plus-72 opening, and the illegal private `Image.m_FillAmount` lift. The damaged managed body is 2481 bytes / 96 locals. Repairing only the first verifier error would leave independent semantic corruption behind.

If runtime coverage promotes this method, reconstruct the complete single MethodDef from the PC-native state machine above. The intended CLR adaptation is limited to replacing native/private layout access with equivalent public managed API access; do not alter original gameplay field metadata, add exception suppression, or add defensive guards not present in PC authority.

## Qualification coverage required after recovery

One targeted exact-R3 launch must exercise all five state families that the PC function distinguishes:

1. `ready`: `chargedLayer > 0`, `skillOngoing == false`.
2. `active_charge`: `chargedLayer == 0`, `skillOngoing == false`, `ChargeType != 3`.
3. `passive`: `chargedLayer == 0`, `skillOngoing == false`, `ChargeType == 3`.
4. `ongoing_high`: `skillOngoing == true`, `chargedLayer > 1`.
5. `ongoing_low`: `skillOngoing == true`, `chargedLayer <= 1`.

Static semantic isolation, field-metadata isolation, exact-reference ILSpy readback, preservation of all previously runtime-qualified recoveries, and absence of `System.Private.CoreLib` pollution remain mandatory.
