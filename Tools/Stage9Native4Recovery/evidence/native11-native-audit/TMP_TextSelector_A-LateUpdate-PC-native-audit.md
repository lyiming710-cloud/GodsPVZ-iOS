# TMP_TextSelector_A::LateUpdate — direct PC-native audit

Status: **native semantics established; recovery remains experimental; no production promotion**.

Authority is the original GodsPVZ 1.0.2 PC x86-64 IL2CPP release.

## Locked identity

- package SHA256: `2f08b4e2243e2bd296f3db694b4a2c9656252d76202c80167bdf3340b9f65b48`
- `GameAssembly.dll` SHA256: `9ebd7ca996a5b03fb4a766f7a2502b660d581ddbbb36af4d7bf2f06a211da39d`
- `global-metadata.dat` SHA256: `ad992341add498bd1018ac980f42171e0737b0793bf945c8714ad70a445b36b9`
- managed method: `System.Void TMPro.Examples.TMP_TextSelector_A::LateUpdate()`
- MethodDef: `0x060007C1`, RID `1985`
- method-pointer entry: `0x181B86B60`
- native entry VA: `0x1803B8350`

The method is split across one root `.pdata` entry and two chained unwind fragments:

| fragment | range | bytes | unwind RVA | unwind flags | SHA256 |
|---|---|---:|---|---|---|
| root | `0x1803B8350–0x1803B8745` | 1013 | `0x1A9424C` | 0 | `ee2541c6bb59bd09a47fc127defeb0f69920ec3ac1afa75945a3218c55952d9b` |
| chained 1 | `0x1803B8745–0x1803B87DE` | 153 | `0x1A94268` | `CHAININFO` | `eb23d0d6fdc172e12b1ada1cb16d804948b88c6b07993c7df361058a1e26ba98` |
| chained 2 | `0x1803B87DE–0x1803B8AEE` | 784 | `0x1A9427C` | `CHAININFO` | `6f091b28b79f1bcb51f00e952e29c87cc5b4f9b732a7e88f9cb5f97184f287f1` |

Both chained unwind records point back to the root runtime-function tuple `(0x3B8350, 0x3B8745, 0x1A9424C)`. Concatenating the three exact runtime-function byte ranges gives 1950 bytes with SHA256:

`042a4c2e4429f02430a72782f3aa7058e3be947edc322c6791cb9144c2434f55`

This chained representation is why treating only the first `.pdata` range as the whole method would truncate the word-selection half of the function.

## Exact managed metadata in the candidate

The cheap Cecil inspection gate `36310200059` locks:

- TypeDef `TMPro.Examples.TMP_TextSelector_A`: `0x02000108`
- `m_TextMeshPro`: `0x04000A05`, `TMPro.TextMeshPro`
- `m_Camera`: `0x04000A06`, `UnityEngine.Camera`
- `m_isHoveringObject`: `0x04000A07`, `System.Boolean`
- `m_selectedLink`: `0x04000A08`, `System.Int32`
- `m_lastCharIndex`: `0x04000A09`, `System.Int32`
- `m_lastWordIndex`: `0x04000A0A`, `System.Int32`
- `LateUpdate`: `0x060007C1`

The damaged managed body is 2490 bytes. Relevant existing MemberRefs include:

- `TMP_TextUtilities.IsIntersectingRectTransform`: `0x0A0002E0`
- `TMP_TextUtilities.FindIntersectingCharacter`: `0x0A0002E1`
- `TMP_TextUtilities.FindIntersectingLink`: `0x0A0002AA`
- `TMP_TextUtilities.FindIntersectingWord`: `0x0A0002E3`
- `TMP_Text.get_textInfo`: `0x0A0002AD`
- `TMP_Text.get_rectTransform`: `0x0A0002D8`
- `TMP_TextInfo.characterInfo`: `0x0A0002B0`
- `TMP_TextInfo.meshInfo`: `0x0A0002B3`
- `TMP_TextInfo.linkInfo`: `0x0A0002AE`
- `TMP_TextInfo.wordInfo`: `0x0A0002DA`
- `TMP_CharacterInfo.materialReferenceIndex`: `0x0A0002B1`
- `TMP_CharacterInfo.vertexIndex`: `0x0A0002B2`
- `TMP_MeshInfo.colors32`: `0x0A0002B4`
- `TMP_LinkInfo.GetLinkID`: `0x0A0002AB`
- `Input.get_mousePosition`: `0x0A000023`
- `Input.GetKeyInt`: `0x0A00000E`
- `Camera.get_main`: `0x0A000004`
- `Random.Range(int,int)`: `0x0A000129`
- `RectTransformUtility.ScreenPointToWorldPointInRectangle`: `0x0A000362`
- `Transform.TransformPoint(Vector3)`: `0x0A000364`
- `Camera.WorldToScreenPoint(Vector3)`: `0x0A000365`
- `Mesh.set_colors32(Color32[])`: `0x0A000361`

## Direct PC-native semantics

The PC control flow matches the TextMesh Pro 3.0.6 example behavior:

1. Set `m_isHoveringObject = false`.
2. Test `TMP_TextUtilities.IsIntersectingRectTransform(m_TextMeshPro.rectTransform, Input.mousePosition, Camera.main)` and set the flag true on intersection.
3. Return immediately from the selection logic when the object is not hovered.
4. Character selection:
   - call `FindIntersectingCharacter(..., true)`;
   - require index != -1, index != `m_lastCharIndex`, and either LeftShift (`304`) or RightShift (`303`);
   - update `m_lastCharIndex`;
   - read `materialReferenceIndex` and `vertexIndex` from the selected `TMP_CharacterInfo`;
   - generate three `Random.Range(0,255)` byte components with alpha 255;
   - write the same `Color32` to four consecutive vertex-color entries;
   - assign the color array back through that material's mesh.
5. Link selection:
   - call `FindIntersectingLink(m_TextMeshPro, Input.mousePosition, m_Camera)`;
   - clear `m_selectedLink` when selection changes or disappears;
   - for a new link, set `m_selectedLink`, load the `TMP_LinkInfo`, call `ScreenPointToWorldPointInRectangle`, call `GetLinkID()`, and execute the no-op `id_01` / `id_02` string comparisons present in the example.
6. Word selection:
   - call `FindIntersectingWord(m_TextMeshPro, Input.mousePosition, Camera.main)`;
   - require index != -1 and != `m_lastWordIndex`, then update `m_lastWordIndex`;
   - load the selected `TMP_WordInfo`;
   - transform the first character's `bottomLeft` through the text transform and then `Camera.main.WorldToScreenPoint`;
   - obtain material 0's color array, generate another random `Color32`, and recolor all four vertices of each character in the word;
   - assign the resulting array to `m_TextMeshPro.mesh.colors32`.

## Native structure attribution around the blocker

The native object field offsets align exactly with the managed fields:

- `+0x20` `m_TextMeshPro`
- `+0x28` `m_Camera`
- `+0x30` `m_isHoveringObject`
- `+0x34` `m_selectedLink`
- `+0x38` `m_lastCharIndex`
- `+0x3C` `m_lastWordIndex`

The TextMesh Pro runtime layout observed by the native body uses `m_TextMeshPro`'s text-info pointer at `+0x370`, then:

- textInfo `+0x38` characterInfo
- textInfo `+0x40` wordInfo
- textInfo `+0x48` linkInfo
- textInfo `+0x60` meshInfo

The selected `TMP_WordInfo` element is 24 bytes. The PC body copies the 24-byte value, then consumes:

- `firstCharacterIndex` at word-info offset `+8`
- `characterCount` at word-info offset `+16`

For `TMP_CharacterInfo`, native element size is `0x178`; the same function reads:

- `materialReferenceIndex` at `+0x58`
- `vertexIndex` at `+0x6C`
- `bottomLeft` Vector3 at `+0x11C`

This directly explains the current iOS failure: the damaged Cpp2IL body exposes an invalid `TMP_WordInfo` stack operation, whereas the PC native code performs an ordinary typed 24-byte struct load followed by scalar field reads.

## Public structural corroboration

TextMesh Pro 3.0.6 example source in `gammawizard12345/Shinobi-Chogumelo`, commit `68fe89358ab19b78624e34eb8ed436311685ca95`, path `Assets/TextMesh Pro/Examples & Extras/Scripts/TMP_TextSelector_A.cs`, has the same character/link/word selection structure. It is corroboration only; the locked PC native body above remains authoritative.

## Recovery rule

Do not apply a generic `TMP_WordInfo` opcode rewrite. Replace only MethodDef `0x060007C1` with a typed body that preserves the native control-flow ordering, Unity/TMP calls, random-color mutations, link-side effects, and word field semantics. Then require deterministic materialization, non-target isolation, and direct post-Linker IL2CPP target removal before any full Unity rebuild.
