# TextLink ResetLink / SetLink — direct PC native audit

Status: **read-only native evidence; native6 semantic formalization remains NO**.

Authority is the original `GodsPVZ_1.0.2.zip` fetched from Drive, not a decompiler reconstruction.

## Locked source

- package SHA256: `2f08b4e2243e2bd296f3db694b4a2c9656252d76202c80167bdf3340b9f65b48`
- `GameAssembly.dll` SHA256: `9ebd7ca996a5b03fb4a766f7a2502b660d581ddbbb36af4d7bf2f06a211da39d`
- `global-metadata.dat` SHA256: `ad992341add498bd1018ac980f42171e0737b0793bf945c8714ad70a445b36b9`
- TextLink TypeDef: `0x020000D0`
- ResetLink MethodDef: `0x060006E3`, native VA `0x1803AD3D0`
- SetLink MethodDef: `0x060006E4`, native VA `0x1803AD940`

## Exact `.pdata` bounds

The PE `.pdata` entries were parsed directly as 12-byte `RUNTIME_FUNCTION` records.

| method | begin | end | bytes | unwind RVA | native SHA256 |
|---|---:|---:|---:|---:|---|
| ResetLink | `0x1803AD3D0` | `0x1803AD934` | 1380 (`0x564`) | `0x1A931A4` | `58e82c9eb7f5e2e643b8eb185521051d44a7c5f7ac99ff282f8ee00f5a8e7f70` |
| SetLink | `0x1803AD940` | `0x1803ADEA4` | 1380 (`0x564`) | `0x1A931A4` | `53bce33f6d063cf20b8e08c6c5aa15927caa37a3800e9a0b3e3008cc076f8571` |

GNU objdump over those exact ranges yields 354 non-`INT3` instructions for each method. Therefore the earlier informal “142 instructions” count is not a valid exact-body count; the `.pdata` bounds above are the current direct binary authority.

## Native ResetLink semantics established directly

The native body does the following:

1. Read `this+0x20` (`TMP_Text`), then its text-info object at `+0x370`, then the link-info array at `+0x48`.
2. Bounds-check `linkIndex` and copy the selected 40-byte link-info element fields used by the loop.
3. Loop for the link text length, starting from the link text first-character index.
4. For each character, read character-info data from the text-info character array (`+0x38`, element size `0x178`).
5. Obtain `materialReferenceIndex` and `vertexIndex`; use the text-info mesh-info array (`+0x60`) to locate that material's `colors32` array.
6. Load `this+0x40`, i.e. `originalColor`, independently for each of the four vertex writes.
7. For each RGBA channel, clamp float Color components to `[0,1]`, multiply by `255`, use the native float-to-byte conversion helper, pack a `Color32`, and write vertices `vertexIndex + 0..3`.
8. After the loop, call the virtual TMP text update path with `edx=0x10`, i.e. the Colors32 vertex-data update flag.
9. Native null and array-range failures leave through the standard IL2CPP exception helpers at `0x180250150` / `0x180250140`.

## Native SetLink symmetry

`SetLink` has the same exact body size and the same loop/conversion shape. The four color loads are from `this+0x30` (`hoverColor`) instead of ResetLink's `this+0x40` (`originalColor`). Its tail likewise updates vertex data with flag `0x10`.

## Audit of current experimental Patcher

The current `scripts/codespaces/Patcher/Program.cs` gets the main operation right at a source level: selected link span, `materialReferenceIndex`, `vertexIndex`, four `Color -> Color32` writes, ResetLink=`originalColor`, SetLink=`hoverColor`, then `UpdateVertexData(16)`.

However, it currently adds this branch before every character write:

```text
characterInfo.isVisible == false -> skip this character
```

No corresponding `isVisible` test/branch exists anywhere in the exact ResetLink or SetLink PC native loop. The native loop goes from character lookup directly to material/vertex lookup and color writes (apart from null/range checks). This is a real semantic difference for an invisible character inside a link span.

Therefore the currently materialized `70a45b9a...` native6 candidate must remain **experimental**, even if a Unity/IL2CPP gate accepts it. Compilation/export success cannot promote TextLink fidelity while this discrepancy remains.

## Next semantic action

Do not mass-edit the candidate. First build a target-specific TextLink spec from these exact native bodies, remove the unsupported `isVisible` branch, and prove the reconstructed ResetLink/SetLink against:

- exact field/member references,
- null/range exception behavior,
- four independent Color32 conversions/writes,
- loop bounds/first-character index,
- `UpdateVertexData(16)`,
- whole-assembly non-target isolation.
