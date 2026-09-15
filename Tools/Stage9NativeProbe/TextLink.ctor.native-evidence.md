# TextLink::.ctor PC native authority

Read-only authority record from the original GodsPVZ 1.0.2 PC release. This evidence does not select `TextLink::.ctor` as the next runtime blocker, does not patch a MethodDef, and does not promote the sealed HF55 baseline.

## Locked original source

- Google Drive package: `GodsPVZ_1.0.2.zip`
- Google Drive file ID: `1_5kVGTFmsvwcdRLQ2ah_rtyC6DqvLnf0`
- package SHA256: `2f08b4e2243e2bd296f3db694b4a2c9656252d76202c80167bdf3340b9f65b48`
- extracted `GameAssembly.dll` SHA256: `9ebd7ca996a5b03fb4a766f7a2502b660d581ddbbb36af4d7bf2f06a211da39d`
- extracted `global-metadata.dat` SHA256: `ad992341add498bd1018ac980f42171e0737b0793bf945c8714ad70a445b36b9`
- Assembly-CSharp method-pointer table VA: `0x181B82D60`

The extraction path was self-calibrated against the already locked SuppliesInitialValue authority before interpreting TextLink:

- Supplies RID604 pointer entry `0x181B84038` -> `0x180341320`
- `.pdata` range `0x180341320–0x18034146C`, 332 bytes
- extracted native SHA256 `3688ac9e8b1ca81c33bb128a816e2a5ab536fd6e48126a5e093749dfd964c6f8`

All values exactly match the previously qualified Supplies authority.

## Managed identity on the current read-only probe

- Type `TextLink`: TypeDef `0x020000D0`, RID208
- `.ctor()`: MethodDef `0x060006E5`, RID1765
- managed RVA `0x94BE8`, code size 46, locals 1
- fields:
  - `TMP_Text`: `0x040008E1`
  - `linkIndex`: `0x040008E2`, `System.Int32`
  - `lastLinkIndex`: `0x040008E3`, `System.Int32`
  - `hoverColor`: `0x040008E4`, `UnityEngine.Color`
  - `originalColor`: `0x040008E5`, `UnityEngine.Color`
  - `isHover`: `0x040008E6`, `System.Boolean`

The damaged recovered body correctly contains `linkIndex = -1`, but omits `lastLinkIndex = -1` and replaces both Color initializers with fake `"Unmanaged memory load: [1815A7B70]"` diagnostics followed by invalid native-integer stores into `UnityEngine.Color` fields.

## PC native mapping

- RID1765 pointer entry: `0x181B86480`
- native pointer: `0x1803ADFA0`
- leaf range: `0x1803ADFA0–0x1803ADFBF`
- length: 31 bytes
- native slice SHA256: `699419627a7893a66b38156beaf1ff5b0fe8530e3015e6fab90d976d7b70669c`
- this leaf has no `.pdata` unwind entry, which is expected for this frameless tail-call leaf

Exact bytes:

```text
66 0f 6f 05 c8 9b 1f 01
48 c7 41 28 ff ff ff ff
0f 11 41 30
33 d2
0f 11 41 40
e9 e1 42 f3 00
```

Independent GNU objdump / LLVM objdump decode:

```text
0x1803ADFA0  movdqa xmm0, xmmword ptr [rip+0x11F9BC8]  ; 0x1815A7B70
0x1803ADFA8  mov    qword ptr [rcx+0x28], -1
0x1803ADFB0  movups xmmword ptr [rcx+0x30], xmm0
0x1803ADFB4  xor    edx, edx
0x1803ADFB6  movups xmmword ptr [rcx+0x40], xmm0
0x1803ADFBA  jmp    0x1812E22A0
```

The 16 bytes at source constant VA `0x1815A7B70` are:

```text
00 00 80 3f 00 00 80 3f 00 00 80 3f 00 00 80 3f
```

Interpreted as four little-endian IEEE754 floats, they are exactly `(1.0, 1.0, 1.0, 1.0)`, i.e. Unity `Color.white`.

## Native semantics

The `qword` store at `this+0x28` spans the two adjacent Int32 sentinel fields (`linkIndex`, `lastLinkIndex`), setting both to `-1`. The following two 128-bit stores initialize the adjacent `hoverColor` and `originalColor` fields from the exact white RGBA constant, followed by the same MonoBehaviour base-constructor tail-call target used by the already qualified constructor authorities.

Managed-equivalent constructor semantics are therefore:

```csharp
linkIndex = -1;
lastLinkIndex = -1;
hoverColor = Color.white;
originalColor = Color.white;
base();
```

No value is written to `TMP_Text` or `isHover` by this native constructor; they remain their zero/default values.

If runtime chronology selects `TextLink::.ctor` as the next causal blocker, it requires a target-specific full MethodDef reconstruction using exact managed references for `UnityEngine.Color.white` (or an exactly equivalent field/value construction proven against the Unity reference assemblies) plus the two Int32 sentinel stores and base constructor. A generic I8-to-I4 repair is not valid for this method. Static semantic-isolation and exact-R3 natural-runtime gates remain mandatory before qualification.
