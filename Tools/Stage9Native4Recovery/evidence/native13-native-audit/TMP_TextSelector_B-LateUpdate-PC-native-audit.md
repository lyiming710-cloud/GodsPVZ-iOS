# TMPro.Examples.TMP_TextSelector_B::LateUpdate — direct PC-native audit

Status: **native extent and source-level semantics established for recovery; no production promotion**.

Authority is the original GodsPVZ 1.0.2 PC x86-64 IL2CPP release. A public copy of Unity's standard TextMesh Pro `TMP_TextSelector_B` example is used only as structural corroboration.

## Locked identity

- package SHA256: `2f08b4e2243e2bd296f3db694b4a2c9656252d76202c80167bdf3340b9f65b48`
- `GameAssembly.dll` SHA256: `9ebd7ca996a5b03fb4a766f7a2502b660d581ddbbb36af4d7bf2f06a211da39d`
- `global-metadata.dat` SHA256: `ad992341add498bd1018ac980f42171e0737b0793bf945c8714ad70a445b36b9`
- managed method: `System.Void TMPro.Examples.TMP_TextSelector_B::LateUpdate()`
- MethodDef: `0x060007C9`, RID `1993`
- Assembly-CSharp method-pointer base: `0x181B82D60`
- method-pointer entry: `0x181B86BA0`
- entry value / PC native VA: `0x1803B8D90`

The method-pointer entry is independently derived as `0x181B82D60 + (1993 - 1) * 8 = 0x181B86BA0`.

## Exact native extent / unwind chain

`LateUpdate` is a large function split across six contiguous `.pdata` records. Later records use `UNW_FLAG_CHAININFO`; therefore treating only the first record as the method body is incorrect.

| fragment | native range | unwind RVA | flags / chain |
|---|---|---|---|
| primary | `0x1803B8D90–0x1803B8E71` | `0x1A9428C` | flags 0 |
| 2 | `0x1803B8E71–0x1803B91DC` | `0x1A94298` | CHAININFO → primary |
| 3 | `0x1803B91DC–0x1803B933C` | `0x1A942CC` | CHAININFO → fragment 2 |
| 4 | `0x1803B933C–0x1803B9DBA` | `0x1A942E4` | CHAININFO → fragment 2 |
| 5 | `0x1803B9DBA–0x1803B9DC4` | `0x1A942F4` | CHAININFO → primary |
| 6 | `0x1803B9DC4–0x1803B9E51` | `0x1A94304` | CHAININFO → primary |

The complete contiguous native extent is therefore:

- range: `0x1803B8D90–0x1803B9E51`
- length: `4289` bytes
- SHA256: `254235dc30236ceabc03e652cf1165f043449f226c2f2dd7c52ec71a1389b246`

GNU `objdump -d -Mintel` over that exact range produces 983 text lines including headers; the body contains the expected IL2CPP metadata initialization, TMP text-info accesses, character/word/link branches, Matrix4x4/vector operations, tint/update calls, and normal IL2CPP null/range exception exits.

## Managed metadata / current blocker

Native13 inspection locks:

- type `TMPro.Examples.TMP_TextSelector_B`: `0x02000109`
- target `LateUpdate`: `0x060007C9`
- damaged body: about 4591 bytes, 1409 IL instructions, 209 locals
- `RestoreCachedVertexAttributes(System.Int32)`: `0x060007CE`

Important fields include `m_TextMeshPro`, `m_Camera`, `isHoveringObject`, `m_selectedWord`, `m_selectedLink`, `m_lastIndex`, `m_matrix`, popup fields, and cached mesh data. The damaged Cpp2IL body contains fake unmanaged-memory strings/pointer arithmetic around `TMP_WordInfo[]`; direct IL2CPP currently reports this method as a blocker with `Cannot get stack type for TMP_WordInfo`.

## Source-level semantics corroborated by native / surviving managed structure

The original PC native call/field shape and surviving managed references match Unity's stock TMP selector example:

1. If `isHoveringObject` is false, restore `m_lastIndex` when needed and return.
2. Character selection:
   - `FindIntersectingCharacter(m_TextMeshPro, Input.mousePosition, m_Camera, true)`;
   - restore previous cached vertex attributes when selection changes;
   - with LeftShift or RightShift, zoom the selected character around its baseline midpoint using `Matrix4x4.TRS(..., scale 1.5f)`;
   - color the four selected vertices `(255,255,192,255)`;
   - swap the selected character's vertex data with the last vertex quad and call `UpdateVertexData(All)`.
3. Word selection:
   - `FindIntersectingWord(...)`;
   - when clearing a previous word, iterate its `TMP_WordInfo.characterCount`, use `firstCharacterIndex + i`, tint the four vertex colors by `1.33333f`, update vertex data, and reset `m_selectedWord`;
   - for a new word while Shift is not held, tint the word by `0.75f` and update vertex data.
4. Link handling:
   - `FindIntersectingLink(...)`;
   - hide popup and clear the old selection when the intersected link changes;
   - for a new link, copy `TMP_LinkInfo`, call `ScreenPointToWorldPointInRectangle`, obtain `GetLinkID()`, and handle `id_01` / `id_02` by positioning/activating the popup and setting `k_LinkText + " ID 01"` or `" ID 02"`.

The constants `1.5f`, `1.33333f`, `0.75f`, the link IDs, popup strings, and the relevant TMP/Unity member references are all present in the managed/native evidence. The standard source is therefore used to regenerate legal typed CIL, while the locked PC native identity/extent and the target assembly metadata remain the fidelity authority.

## Recovery strategy

Do not hand-edit 1409 damaged instructions. Compile an isolated donor implementation of only the stock `LateUpdate` logic against the exact post-Linker Unity/TMP assemblies, then use Mono.Cecil to transplant that method body into MethodDef `0x060007C9`, remapping only the selector's own fields/method and importing external Unity/TMP references. Candidate materialization still starts from the exact native12 unlinked candidate; the post-Linker seed is used only as compiler reference input.

Require:

- target-only MethodDef replacement (`0x060007C9`);
- TypeDef/MethodDef/FieldDef counts unchanged;
- MVID preserved;
- semantic fingerprints for all non-target method bodies unchanged;
- deterministic two-run output;
- direct post-Linker IL2CPP closure of Selector_B before any full Unity/iOS run.
