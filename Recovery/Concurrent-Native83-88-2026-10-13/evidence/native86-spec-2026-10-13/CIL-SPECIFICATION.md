# Native86 read-only caller specification

Static evidence only: no tests, native/CIL method execution, build, candidate construction, patch application, or existing-file changes. Evidence readers wrote only into this new directory. Native55 damaged instructions are used for reference leads, not behavior. `SYMBOLIC-CIL.il` is a textual recipe, not an applied MethodBody.

Original PE hash is `9ebd7ca996a5b03fb4a766f7a2502b660d581ddbbb36af4d7bf2f06a211da39d`; original global metadata hash is `ad992341add498bd1018ac980f42171e0737b0793bf945c8714ad70a445b36b9`. Original pointers and full slices match native55. Parent82 actual bytes were read in memory from the existing Native78–82 archive, member `candidates/native82/candidate1.dll`, and independently hash to `48b9e89e941c041d6c11c4064ad05d9384a1a4dd51d5f8d3fad9192cbda5adbf`. No DLL was exported or executed. The supplied Native86 metadata inventory matches that hash.

## 0x060006DF — TextLink.OnPointerClick(PointerEventData)

Original native `[0x1803AD1B0,0x1803AD3AE)`, hash `89e70a405deacb818cb518ea0aabaa24c051222518d928cf9dbce64474c8db22`, three contiguous `.pdata` parts with flags 0/4, no exception cleanup. PointerEventData is a reference-type parameter, not a struct or ref parameter.

### Exact caller sequence

1. Capture `this.TMP_Text` first, before checking/dereferencing eventData. Do not raw-null or Unity-truth-check this captured text before FindIntersectingLink.
2. Raw-null eventData fails before Camera.main. Capture its position x/y before the camera getter; original caller reads x from +0x104 then y from +0x108. Their bits are copied, with no arithmetic. The FindIntersectingLink position is Vector3(x,y,**positive zero**) by value.
3. Call Camera.main once. A null returned Camera is passed through to FindIntersectingLink; there is no camera raw-null/pseudo-null guard in this caller.
4. Call static `TMP_TextUtilities.FindIntersectingLink(capturedText,capturedPosition,currentCamera)`. The method gets the old captured text even if Camera.main changed this.TMP_Text. Its Vector3 parameter is by value, not `Vector3&` or an Object-local address.
5. Capture its returned Int32; write it into `this.linkIndex`; compare that **returned value**, not a field reread, to -1. Exactly -1 returns. Any other negative value proceeds and eventually gets the array's normal bounds failure after the object/array null checks.
6. If not -1, reread current `this.TMP_Text`, get its current textInfo, get that object's linkInfo array, and index using the saved FindIntersectingLink result. Text/textInfo/array raw-null failures occur after the linkIndex write. A Find callback may replace the text or its textInfo/array. Do not index the earlier captured text or use a callback-modified this.linkIndex in place of the returned index.
7. Copy the selected **40-byte TMP_LinkInfo struct by value** into a typed local. Call its GetLinkID with the address of that copied local. Do not call on `ldelema` of the original array element, which changes alias/writeback semantics.
8. Capture the returned string once. Call Debug.Log(string), then compare that same captured string to `"jump0"` with static String.op_Equality. A Log callback does not cause a new array/field/GetLinkID read.
9. On equality, call Application.OpenURL with the fixed original URL; otherwise return. It does not call TextLink.Jump0 from this caller. All exceptions propagate immediately; there is no cleanup or subsequent compensating state write.

If the original captured text is null but Find returns -1, the caller can return after writing -1 without its later textInfo dereference. Adding a pre-Find text-null failure changes that behavior. There are no Unity object pseudo-null predicates in OnPointerClick.

### Original strings and current reusable tokens

| Use | Original runtime slot / literal index | Verified text | Current user-string token |
| --- | --- | --- | --- |
| String comparison | `0x181BBC628`, usage `0xA000391B`, index 7309 | `jump0`, UTF-8 `6A756D7030` | `0x7001CCC2` |
| OpenURL argument | `0x181BB86F0`, usage `0xA00037AB`, index 7125 | `https://space.bilibili.com/525659346` | `0x7001CCCE` |

Both tokens and contents were read from actual parent82 CIL. No new string heap data is needed. The only float constant introduced into the argument is positive zero `0x00000000`; x/y must retain arbitrary float32/NaN payload bits. There is no normalization, cast to integer, addition or multiplication in this caller.

### Independent public getter proof

The original external CodeGenModules were found by exact image-name references and method-pointer tables, independently of native55's damaged caller references:

| Getter | Original image / token | Native body | Flags / proof |
| --- | --- | --- | --- |
| TMP_Text.get_textInfo | Unity.TextMeshPro.dll / `0x06000526` | `0x1811B8720–0x1811B8728` | flags `0x886`: public, instance, nonvirtual; exact `mov rax,[rcx+0x370]; ret` |
| PointerEventData.get_position | UnityEngine.UI.dll / `0x06000609` | `0x1815805B0–0x1815805C9` | flags `0x886`: public, instance, nonvirtual; copies y/x into Vector2 return with no arithmetic/callback |

The getter hashes are respectively `fceeafd0dc388f71ebe0027857589238ca9d87055cd984e81fa647e147313a75` and `2bbce0ff8bcf7d83d4bad29db6b22d971a1132b703eb3f025c01854b1b5f6290`. Their CodeGenModule identities, table RVAs, original method definitions and full leaf instruction bytes are saved. TMP_Text.m_textInfo is protected (`0x4` field access); PointerEventData position backing field is private (`0x1`). Public getters are therefore the legal replacements, with direct original-native proof, not guessed C#.

Pointer getter loads y before x, whereas the inlined caller loaded x before y. Both simply copy the same stable coordinate bits and have no intervening application callback. The replacement preserves the sequential callback/value semantics, but this report does not claim equivalence under concurrent mutation of the two fields. Capture the returned Vector2 before Camera.main. TMP_Text getter is nonvirtual, so ordinary callvirt receiver checking does not introduce override dispatch.

TMP_TextInfo.linkInfo is public (`0x6` access in original metadata). Its existing parent MemberRef is `0x0A0002B9`, field signature `06 1D 11 81 95`: an array of value-type TMP_LinkInfo. Existing textInfo getter MemberRef `0x0A0002B8` has signature `20 00 12 81 9D` and returns the TMP_TextInfo reference. The original TMP_TextUtilities.FindIntersectingLink independently maps to Unity.TextMeshPro `0x06000624`, `0x1811CC730`; GetLinkID independently maps to `0x06000118`, `0x18119BC90`. Find has public static flags `0x96`; GetLinkID public instance `0x86`.

### Current bindings and minimal locals

Current method is 209 bytes, signature `0x11000533`, no EH. Existing Camera V0, Int32 V1, String V5 and typed TMP_LinkInfo V8 are usable. There is no existing typed Vector2 local. Append one local for a captured Vector2, preserving the old local-signature prefix; do not use Object V7 as a vector.

| Binding | Current token |
| --- | --- |
| TextLink.TMP_Text / linkIndex | `0x040008E1` / `0x040008E2`, same-owner private access |
| PointerEventData TypeRef / Vector2 TypeRef | `0x01000066` / `0x0100004B` |
| Vector2.x / Vector2.y | `0x0A0001D4` / `0x0A0001D5`, actual parent signatures `06 0C` |
| Vector3 constructor | `0x0A00009C`, three Single arguments |
| Camera.main / FindIntersectingLink | `0x0A000004` / `0x0A0002B5` |
| TMP_Text.get_textInfo / TMP_TextInfo.linkInfo | `0x0A0002B8` / `0x0A0002B9` |
| TMP_LinkInfo TypeRef / GetLinkID | `0x01000065` / `0x0A0002B6` |
| Debug.Log / String.op_Equality / Application.OpenURL | `0x0A000010` / `0x0A00003A` / `0x0A0002B7` |

PointerEventData.get_position is absent from current refs and needs one public instance zero-argument getter MemberRef, declaring parent TypeRef `0x01000066`, returning Vector2 TypeRef `0x0100004B` **by value**. Signature bytes should be `20 00 11 81 2D` with these current tokens; recompute/guard those bindings rather than inserting them blindly. MemberRefParent coded index is `0x331`. An appended Vector2 local's type suffix is `11 81 2D`; old local count 9 becomes 10.

The minimal stack recipe keeps the captured TMP_Text typed reference on the stack across the position getter and Camera.main, then constructs the Vector3 on the stack and calls Find. Thus only the Vector2 local must be appended; no new TMP_Text or Vector3 local is needed. MaxStack at least 4 is required for captured text plus three constructor floats. Alternative explicitly typed captures are valid if their signatures are scoped correctly.

## 0x060006FE — Window_Q.Yes

Original `[0x1803B0320,0x1803B0584)`, hash `52b13dd5cc1fe4e321dc95a1f7384949ce620f961e1fdf6bff614a6ac8e9903a`, six contiguous `.pdata` parts, flags 0/4, no cleanup. The six-entry jump table at `0x1803B056C` is data. Q is an Int32 field, not an invented enum; unsigned bounds route negative and >5 to return. Dispatch Q once and never restart it after callbacks.

| Q | Native branch | Caller behavior |
| --- | --- | --- |
| 0 | `0x1803B0354` | Return |
| 1 | `0x1803B034A` | this.BoardReturnMain(); return, no additional caller Destroy |
| 2,3 | `0x1803B035F` | this.BoardResume(); return, no additional caller Destroy |
| 4 | `0x1803B0373` | Inline selected-save deletion path below |
| 5 | `0x1803B04CD` | Inline saves-manager backup path below |
| negative / >5 | default | Return |

Do not replace Q4/Q5 with Window_Q.DeletePlayerSave or Window_Q.BackupPlayerSave helpers; this caller's inline conditions/captures/destruction scope must be retained. The original Q1/Q2/Q3 helpers remain opaque same-owner calls with their existing effects.

### Q4 deletion: early return is distinct from Destroy

1. If initial this.info0_int < 0, return **without Destroy**. Do not read singleton/UI/list first.
2. Resolve current MainUI singleton; raw-null singleton throws. Capture its savesUIController field. Call Unity Object.op_Equality(capturedUI,null). If true (including Unity destroyed/pseudo-null UI), return **without Destroy**.
3. Use that same captured UI after the equality predicate, not a new singleton/UI field. Raw-null captured UI fails if an adversarial equality returned false for null. Read current capturedUI.playerSaveList; raw-null list fails.
4. Reread this.info0_int **after** the equality predicate/list acquisition; capture that current integer. Read this captured List's Count. If **signed Count <= capturedIndex**, return **without Destroy**. If a callback changed index negative after the initial gate, signed Count > negative passes to the real indexer, which throws for negative index; do not add a second nonnegative filter.
5. Call real typed List<SavesUIController_PlayerSave>.get_Item(capturedIndex) and capture the item. Call Unity Object.op_Implicit(capturedItem). If false, skip deletion but proceed to **Destroy window**.
6. On true, raw-null item fails. Physically check capturedItem.savesManager for null; if null, skip deletion but proceed to **Destroy window**. This is raw-null behavior, not a Unity fake-null predicate.
7. Read capturedItem.playerSaveID, then reread capturedItem.savesManager; call that reread manager.DeletePlayerSave_ID(capturedID). The original manager field is read once for the null gate and again for the call, with ID read between them. No application callback intervenes in those primitive reads.
8. After Delete returns, resolve **current MainUI singleton again**, raw-null fail if absent, then call that current instance.LoadData. Do not reuse the pre-delete singleton or UI.
9. After LoadData returns, enter the common Destroy-window block.

The initial UI capture survives the Unity equality callback, whereas its list and this.info0_int are read afterwards. The index survives get_Item and the later Unity item predicate; the selected item is captured once. The predicate callback can replace fields on that captured item, so its manager/ID are read only afterwards. Deletion/LoadData exceptions skip Destroy. A false item predicate or null item manager still destroys the window; an initial negative index, pseudo-null UI, or Count<=index leaves it open.

### Q5 backup and common destruction

Resolve current MainUI, raw-null fail if absent, then read/capture its savesManager. If it is physically null, return **without Destroy**. Otherwise call that captured manager.BackupPlayerSave and then Destroy window. There is no Unity fake-null gate on this manager and no extra LoadData call in this caller.

Common destruction occurs only after the selected branch's completed work or its explicit destroy-side skip: call this Component.get_gameObject once; capture/pass its return to static Object.Destroy(Object). The original does not raw-null or Unity-truth-check that returned GameObject. Do not put Destroy into a finally and do not force it on Q0/1/2/3/default or the early-return paths.

### Proven MainUI/list helpers and current bindings

Current public static MainUIController.get_instance `0x06000636` was read from actual parent82: exactly `br; ldsfld 0x04000843; ret`, body hash `abd76d37e86268c635b733ef9819be2f2c4b763dd5134095e7a569610acea652`. This proves the replacement for the private inlined singleton field. Return type and public/static flags are stored in JSON. All MainUI/UI/item fields used above are current-parent public; Window Q/info0_int are public and same-owner.

Original slots `0x181BB8718`/`0x181BB8770` resolve respectively to MethodSpecs 72502/72503, original mscorlib Count `0x060023C7`/Item `0x060023CD`, class instantiation 2489 with the sole argument original SavesUIController_PlayerSave type index 5289. Public Count replacement for the inlined `_size` read is proved by actual pinned mscorlib CIL `ldarg.0; ldfld _size; ret`, body hash `1f9b3b3cabc80680eba2fe6d74d14ce7b4005d0ac4019f23070d0005242b87c9`. Item performs its normal unsigned index bounds check and then `_items[index]`; do not bypass it. Those actual library bodies were previously statically extracted and copied into this JSON after rechecking the library hash `4d1725f1ae54a22f69b79c2b1569181ed6653dd484c35005e29c40300c6673be`.

| Binding | Current token |
| --- | --- |
| Window_Q.Q / info0_int | `0x04000900` / `0x04000909` |
| Window_Q.BoardReturnMain / BoardResume | `0x06000702` / `0x06000701`, private same-owner legal calls |
| MainUI.get_instance / LoadData | `0x06000636` / `0x0600063A` |
| MainUI.savesUIController / savesManager | `0x0400084B` / `0x0400084A` |
| SavesUIController.playerSaveList | `0x040008B1` |
| List<SavesUIController_PlayerSave>.Count / Item | `0x0A000443` / `0x0A0002C7` |
| Item.savesManager / playerSaveID | `0x040008B2` / `0x040008B4` |
| SavesManager.DeletePlayerSave_ID / BackupPlayerSave | `0x0600023C` / `0x06000238`, public void(Int32) / void() |
| Unity Object.op_Equality / op_Implicit | `0x0A000012` / `0x0A00000B` |
| Component.get_gameObject / Object.Destroy(Object) | `0x0A000009` / `0x0A000025` |

Current Yes is only 83 bytes, max stack 2, locals `0x11000549`, no EH. A correct full switch can use typed stack references and existing Int32 V0, first for the reread index and later for captured playerSaveID; erased Object scratch locals need not be used. The recipe's dup/UI/null equality sequence reaches MaxStack 3. No new Window local signature or refs are required. The complete inline paths exceed the damaged short placeholder and require complete supported placement; do not omit gates/Destroy distinctions to fit it.

This establishes static original helper identity, caller CFG, argument shape, field/callback/null ordering and current references. It does not execute TMPro, UI, collections, private-helper replacements, logging or OpenURL, and does not assert acceptance or concurrent-field equivalence. Static property getter/load substitutions preserve sequential callback semantics; executed bit-level/exception observations remain outside this subtask.
