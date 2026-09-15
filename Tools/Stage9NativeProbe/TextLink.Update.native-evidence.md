# TextLink::Update PC native authority

Read-only evidence only. This file does not mutate a managed candidate and does not promote the sealed HF55 baseline.

## Source authority

- Original PC package: `GodsPVZ_1.0.2.zip`, Drive ID `1_5kVGTFmsvwcdRLQ2ah_rtyC6DqvLnf0`
- ZIP SHA256: `2f08b4e2243e2bd296f3db694b4a2c9656252d76202c80167bdf3340b9f65b48`
- `GameAssembly.dll` SHA256: `9ebd7ca996a5b03fb4a766f7a2502b660d581ddbbb36af4d7bf2f06a211da39d`
- `global-metadata.dat` SHA256: `ad992341add498bd1018ac980f42171e0737b0793bf945c8714ad70a445b36b9`
- method pointer table base: `0x181B82D60`
- mapping: `entryVA = 0x181B82D60 + (RID-1)*8`

## Managed identity

- `TextLink` TypeDef `0x020000D0`
- `TextLink::Update()` MethodDef `0x060006DE`, RID 1758
- damaged code size 235 bytes
- maxstack 3
- 9 locals, initlocals true
- no exception handlers
- relevant fields:
  - `TMP_Text` `0x040008E1`
  - `linkIndex` `0x040008E2`
  - `lastLinkIndex` `0x040008E3`

Managed RVA is file-layout output and is not used as method identity. It was `0x93510` on the pre-LevelItem TextLink candidate and is `0x93294` on static-qualified LevelItem candidate `fe67249cf3bdb30b94aa44045548c3de5ce6861e7686c7acebd4c97e4a3ead7b`; MethodDef token/RID and body semantics remain the stable identity.

Read-only probe run `34989463898`, artifact `10404822152`, on `fe67249c…` confirms that the damaged body stores `Input.mousePosition` into local 1 (`UnityEngine.Vector3`) and `Camera.main` into local 2, but then calls `TMP_TextUtilities.FindIntersectingLink` using `ldloca V_8:System.Object` as the Vector3 argument. That is type-invalid IL. It also confirms one call each to mousePosition, Camera.main, FindIntersectingLink, SetLink and ResetLink. The remaining high-level state-machine structure already resembles the native implementation.

## Native pointer and body

For RID 1758:

- pointer entry: `0x181B86448`
- direct pointer: `0x1803ADEB0`
- `.pdata` RuntimeFunction: `0x1803ADEB0–0x1803ADF93`
- exact body size: 227 bytes
- exact native SHA256: `5bf83d68b7d1c545595f58f52527bf0d29dfb7da03f76220fc74e4ed4c48b0a6`

## Native semantics

The PC body performs:

1. `TMP_Text` = instance field at `this+0x20`.
2. Obtains `Input.mousePosition` as a `Vector3` value.
3. Obtains `Camera.main`.
4. Calls `TMP_TextUtilities.FindIntersectingLink(TMP_Text, mousePosition, Camera.main)` and stores the returned `int` to `linkIndex` at `this+0x28`.
5. If `linkIndex != -1` and differs from `lastLinkIndex`, calls `SetLink(linkIndex)`.
6. If a previous link was active (`lastLinkIndex != -1`), calls `ResetLink(lastLinkIndex)` when appropriate.
7. Copies `linkIndex` to `lastLinkIndex` at `this+0x2c` when the active link state changes.
8. Returns normally.

Key disassembly:

```text
0x1803ADEE3  mov rdi, qword ptr [rbx+0x20]    ; TMP_Text
0x1803ADEE7  lea rcx, [rsp+0x20]
0x1803ADEEC  xor edx, edx
0x1803ADEEE  call 0x181364CA0                ; Input.mousePosition
...
0x1803ADEF3  xor ecx, ecx
0x1803ADF02  call 0x1812E27C0                ; Camera.main
...
0x1803ADF3A  mov rcx, rdi
0x1803ADF3D  call 0x1811CC730                ; TMP_TextUtilities.FindIntersectingLink
0x1803ADF42  mov dword ptr [rbx+0x28], eax   ; linkIndex
0x1803ADF45  cmp eax, -1
...
0x1803ADF57  call 0x1803AD940                ; TextLink.SetLink, MethodDef 0x060006E4
...
0x1803ADF73  call 0x1803AD3D0                ; TextLink.ResetLink, MethodDef 0x060006E3
0x1803ADF78  mov eax, dword ptr [rbx+0x28]
0x1803ADF7B  mov dword ptr [rbx+0x2c], eax   ; lastLinkIndex = linkIndex
```

Method-pointer cross-checks:

- `ResetLink`, RID1763: pointer entry `0x181B86470` -> `0x1803AD3D0`
- `SetLink`, RID1764: pointer entry `0x181B86478` -> `0x1803AD940`

## Repair constraint if chronology selects this method

The strongest minimal repair is to replace only the invalid Vector3 argument load immediately before `FindIntersectingLink`: the damaged `ldloca V_8:System.Object` must become a normal load of local 1 (`UnityEngine.Vector3`), preserving all other verified state-machine logic and metadata unless an independent exact-ref gate reveals another mismatch.

This is pre-fetched evidence only. A managed patch must not be created until an exact-R3 runtime after the current LevelItem candidate selects `TextLink::Update()` as `FIRST_INVALID_IL`.
