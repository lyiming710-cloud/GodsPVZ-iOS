# Native84 read-only caller specifications

No tests, target native/CIL execution, build, candidate construction, patch application, or existing-file change occurred. Static byte/metadata readers wrote evidence only into this new directory. Native55 damaged CIL supplies reference identities only; its unresolved indirect jump and erased locals are not a behavioral specification.

Original PE SHA-256 is `9ebd7ca996a5b03fb4a766f7a2502b660d581ddbbb36af4d7bf2f06a211da39d`; original metadata SHA-256 is `ad992341add498bd1018ac980f42171e0737b0793bf945c8714ad70a445b36b9`. Both target method-pointer entries and full native slices match the original bytes and native55 slice hashes.

The supplied parent82 was independently read **in memory** from existing `audit/native82-codex-2026-10-13/godspvz-native78-82-pending-2026-10-13.zip`, member `candidates/native82/candidate1.dll`. Its actual bytes hash to `48b9e89e941c041d6c11c4064ad05d9384a1a4dd51d5f8d3fad9192cbda5adbf`, matching `audit/native83-codex-2026-10-13/metadata-current.json`. No new DLL was exported. The archive/member hashes, actual declaration/access metadata, public singleton getter body, original generic binding, and saved inventory subset are in `PRIMARY-EVIDENCE.json`.

## 0x060001C8 — MouseManager.DropDownAll

Native interval `[0x18031D7E0,0x18031D9F0)`, SHA-256 `6aa20faca7c030a9d855db86bbf83baf7bb0b9b6fca6c0bada6df093b00828ef`. This is six contiguous `.pdata` parts bounded by the next mapped method pointer; the seven-entry jump table starts at `0x18031D9D4`. Table bytes are data, not fallthrough instructions. Unwind flags are 0 or chained-info 4; there is no exception cleanup handler to manufacture in CIL.

Read handItemType once for dispatch. The unsigned native bound check sends negative values and values above 6 directly to return. Original v31 enum default bytes and actual parent CLR constants agree on the following names/values; original integer defaults are compressed one-byte nonnegative encodings (`0,2,4,...`), not uncompressed four-byte values.

| Value / ItemType | Native branch | Behavior |
| --- | --- | --- |
| 0 / None | `0x18031D82A` | Capture current board; raw-null failure on its flag read; if onDetailPage, call that captured board.Fold_DetailPage(true,true); return. No ResetMouseState. |
| 1 / CardOnCardbank | `0x18031D860` | this.DropCardOnCardBank(); return. No explicit ResetMouseState in this caller. |
| 2 / CardOnField | `0x18031D9C0` | Return. |
| 3 / Plant | `0x18031D9C0` | Return. |
| 4 / PlantSkillCrosshairs | `0x18031D874` | Crosshair cleanup described below; then independently current board detail-page cleanup; then ResetMouseState. |
| 5 / Shovel | `0x18031D92D` | Unity op_Inequality(captured propOnHand,null); if true, reread propOnHand and call DropDown; then ResetMouseState. |
| 6 / Glove | `0x18031D96F` | Unity op_Implicit(captured propOnHand); if true, reread propOnHand and call DropDown; always then DropPlantOnGlove; then ResetMouseState. |
| negative / >6 | default | Return. |

The Shovel case's use of propOnHand is original behavior. Do not infer a more intuitive route from the enum name. Values Hammer=7 through Handcart=13 are defined enum values but take the default return in this method.

### Crosshair path and callback reads

Case 4 begins by capturing `this.plantSkillCrosshairs` and passing that captured GameObject to **UnityEngine.Object.op_Implicit**. This is Unity pseudo-null behavior, not a CLR reference-null check.

If the Unity predicate returns true:

1. Read current `this.skillOnHandOwnerPlant`; raw-null failure if absent. Read and capture that Plant's `attackRangeUIController`; raw-null failure if absent. Call that controller's CollapseView.
2. After CollapseView returns, reread current `this.plantSkillCrosshairs` and pass it to **static `Object.Destroy(Object)`**. Do not reuse the crosshair captured for the earlier truth test. Do not add a raw-null check before Destroy: the original passes a currently null field to the static API too.
3. After Destroy returns, reread current `this.board`, raw-null check, and call DisplayButtons on it.

Regardless of the original truth result, read current `this.board` again for the independent onDetailPage test. Raw-null board fails here even if the crosshair truth result was false. Capture that board for both its flag read and Fold_DetailPage receiver; if onDetailPage is true call Fold_DetailPage(true,true). Then call ResetMouseState once.

CollapseView/Destroy/DisplayButtons can replace the next-read fields. Preserve those rereads, and do not cache owner/controller/board across these call boundaries. A flag change from DisplayButtons affects the subsequent detail-page test. The switch itself is not restarted if a helper changes handItemType.

Cases 5/6 similarly capture the old prop only for the Unity predicate, then **reread** propOnHand for DropDown after the predicate. An adversarial predicate replacement can therefore cause DropDown to use a replacement or fail on a new raw-null field. Case 5 uses op_Inequality(prop,null), whereas case 6 uses op_Implicit(prop); preserve their distinct public API calls. A false predicate skips only DropDown; Glove still invokes DropPlantOnGlove.

ResetMouseState is ordinary control flow, not a finally: exceptions from predicates, CollapseView, Destroy, DisplayButtons, Fold_DetailPage, DropDown, or DropPlantOnGlove propagate before Reset. After Reset throws, propagate it. Cases 0–3/default do not add an explicit Reset here, even if a selected helper performs its own state changes.

### Concrete parent82 references and types

| Operation/field | Current token | Access/signature |
| --- | --- | --- |
| MouseManager.board | `0x04000230` | public Board |
| MouseManager.handItemType | `0x04000233` | public ItemType |
| MouseManager.plantSkillCrosshairs | `0x04000240` | private GameObject; same-owner access is legal |
| MouseManager.skillOnHandOwnerPlant | `0x04000244` | public Plant |
| MouseManager.propOnHand | `0x04000245` | public Prop |
| Plant.attackRangeUIController | `0x040004AF` | public AttackRangeUIController, proved in actual parent bytes |
| Board.onDetailPage | `0x040003D0` | public Boolean |
| Board.Fold_DetailPage | `0x060002BA` | public instance void(Boolean,Boolean), signature `20 02 01 02 02` |
| Board.DisplayButtons | `0x060002B8` | public instance void() |
| MouseManager.DropCardOnCardBank | `0x060001CA` | public instance void() |
| MouseManager.DropPlantOnGlove | `0x060001C9` | public instance void() |
| MouseManager.ResetMouseState | `0x060001E1` | public instance void() |
| Prop.DropDown | `0x06000401` | public instance void() |
| AttackRangeUIController.CollapseView | `0x060005BE` | public instance void(), signature `20 00 01` |
| Unity op_Implicit | `0x0A00000B` | static Boolean(Object) |
| Unity op_Inequality | `0x0A00000C` | static Boolean(Object,Object) |
| Unity Destroy | `0x0A000025` | static void(Object), **one-argument overload** |

All game call declarations and field access were independently read from actual parent82 CLR bytes, not only the inventory. Original native call mappings are also in the evidence JSON.

Current body is only 88 bytes, local signature `0x1100016F`, max stack 2, no EH. Most locals are erased Object types. A correct recipe can avoid new locals entirely: pass predicate input directly from the field; reread the required receiver afterwards; keep a Board value on the stack with `dup` for the flag/Fold pair, popping it on the false branch. This preserves typed stack operands without storing Board/Prop/Plant into Object scratch locals and using them uncast. The complete switch requires more code than the damaged placeholder; choose a complete supported placement rather than truncating it. Both Fold arguments plus receiver require MaxStack at least 3.

## 0x060002CE — Board.Space_down

Native interval `[0x180327700,0x180327917)`, SHA-256 `900963ccc47566f5a837ff5c9f294c0fcfb736be43aef96a06d88bf5b0756c73`. Three contiguous `.pdata` parts have flags 0/4, with no cleanup handler.

### Required control flow and partial writes

1. Read current this.isFailed. If true, read the current BoardManager singleton and raw-null fail if it is null; call BoardResume. **Continue afterwards**. Do not return or use an else branch after BoardResume.
2. After any BoardResume callback, read current this.pauseTemp. If true, return without reading menuOpen. Otherwise read current this.menuOpen; if true, return.
3. Read/capture current this.gamePause and then set this.pauseTemp=true. The native compare captures gamePause before the write and branches on that previously read value.
4. If captured gamePause is true, call this.GameContinue and return. The pauseTemp write precedes that call; no additional caller write follows it.
5. If captured gamePause is false, perform the pause-start sequence below. A later callback change to gamePause does not switch to GameContinue; the chosen path is already fixed.

Pause-start sequence:

```text
capturedClip = current ResourceManager.boardClips[11];
capturedCamera = Camera.main;                    // one getter
capturedTransform = capturedCamera.get_transform();
capturedPosition = capturedTransform.get_position(); // full Vector3 value
capturedVolume = GlobalStaticVars.AudioVolume();
GlobalStaticVars.CreateAudioAtPoint(capturedClip,capturedPosition,capturedVolume);
// discard returned AudioSource
current this.pause.SetActive(true);
Debug.Log("GamePause");
this.gamePause = true;
Time.timeScale = positive-zero Single;
return;
```

Read/capture the AudioClip **before** Camera.main. Camera and Transform have raw-null failures at their call boundaries; a null AudioClip is passed to CreateAudioAtPoint with no added null guard. The Vector3 position is captured before AudioVolume; that callback cannot replace the already captured position or clip arguments. The returned volume is passed unchanged, with no multiplier or substitute default: arbitrary float32/NaN bits are forwarded to the helper, not sanitized.

CreateAudioAtPoint is exactly the **three-argument overload** `AudioClip,Vector3,Single`, whose return is discarded. A four-argument pitch overload would add behavior absent from this caller. The Vector3 argument is by value, not an address of Object/Vector3.

Read this.pause only after CreateAudioAtPoint returns, so its callback replacement is observed; perform raw receiver-null enforcement for SetActive(true). Do not Unity-truth-test the pause GameObject. Debug.Log runs only after SetActive returns. Set gamePause=true only after Log returns, overwriting any Log callback change to that field. Time.set_timeScale(+0f) comes after the gamePause write. No pauseTemp restoration appears in this caller.

On any list/getter/audio/UI/log/time exception, propagate it and retain all earlier caller writes. In particular pauseTemp is already true before list indexing, Camera.main, or any audio/UI step. On a failing Log, the final gamePause=true write has not occurred. On a failing Time setter, that final gamePause write has occurred. BoardResume failure precedes the pauseTemp/menuOpen gates.

### Proven singleton getter replacement

Original native loads the private BoardManager static `<Instance>k__BackingField` inline. The current public static getter at `0x06000169` was independently parsed from parent82 and is exactly:

```text
IL_0000: br IL_0005
IL_0005: ldsfld BoardManager BoardManager::<Instance>k__BackingField // 0x040001AD
IL_000A: ret
```

Getter body SHA-256 is `44ba7da2231eed55aeda94be6cfe0eee8d3e81d86f4d618efbf929cc762b3fe0`; signature bytes are `00 00 12 80 EC`; flags prove public/static. Original native public getter `0x1803100F0–0x180310126` reads the same BoardManager type slot `0x181BD1A40` and backing field, hash `f88521b1f8b891c8fdf8b8d48ea80337cbb0b52c3b0eb3f19c6989f0f60c6eaf`. It has no application-level side effect beyond that field read. Therefore `call BoardManager::get_Instance()` is a supported legal replacement; do not access the private static backing field from Board.

### Real list binding, string and constants

Slot `0x181BA4278` contains tagged usage `0xC0023075`, original MethodSpec index 71738, original mscorlib MethodDef `0x060023CD` (`List<T>.get_Item`), class-instantiation index 2364. Its sole generic argument resolves to original UnityEngine.AudioClip type index 7547. This proves the actual closed public List<AudioClip> indexer, not an erased List<Object> helper.

Original get_Item native helper at `0x180825710` is saved. The available resolver mscorlib SHA `4d1725f1ae54a22f69b79c2b1569181ed6653dd484c35005e29c40300c6673be` public getter CIL checks unsigned index against `_size`, calls its normal out-of-range throw helper, then reads `_items[index]`. Its body hash is `c68a35b843104f0c39d5b17964698c104a790ce9acd1cdc33ac43ad973c36f57`. Preserve direct `get_Item(11)`; do not add Count checks, fallback clips, or internal-array access.

Original string slot `0x181BC6F38` is tagged usage `0xA0001249`, literal index 2340, UTF-8 `47 61 6D 65 50 61 75 73 65`, exactly `GamePause`. Actual parent Space_down already references that text with `ldstr` token **`0x70009EE2`** at IL offset 269, independently read from its raw MethodBody. Reuse this existing user-string token; no new string row/heap is needed.

The remaining constants are index 11, SetActive true, pauseTemp/gamePause true, and timeScale **positive zero bits `0x00000000`** from native xorps. There is no numeric field arithmetic in this caller.

### Concrete parent82 bindings and locals

| Reference | Current token |
| --- | --- |
| Board.isFailed / gamePause / menuOpen / pause / pauseTemp | `0x040003B6` / `0x040003BB` / `0x040003BD` / `0x040003C8` / `0x040003D2` |
| ResourceManager.boardClips | `0x040002EB`, actual parent proves public static |
| BoardManager.get_Instance / BoardResume | `0x06000169` / `0x0600016E` |
| List<AudioClip>.get_Item(Int32) | `0x0A000084` |
| Camera.get_main / Component.get_transform / Transform.get_position | `0x0A000004` / `0x0A000001` / `0x0A000037` |
| AudioVolume / CreateAudioAtPoint three-arg | `0x06000138` / `0x0600013D` |
| GameObject.SetActive(Boolean) / Debug.Log(Object) / Time.set_timeScale(Single) | `0x0A000007` / `0x0A000010` / `0x0A0000F2` |
| Board.GameContinue | `0x060002BD` |

Current Space_down is 354 bytes, local signature `0x11000239`, no EH. Existing Boolean locals V1–V4 can capture gamePause. Existing typed Camera V6, Transform V7, AudioClip V9, Vector3 V10, and Single V5/V13 can preserve the required captures. Discard CreateAudioAtPoint's return with pop, or store in its existing AudioSource V8 without using it. Three audio arguments require MaxStack at least 3; the damaged old max of 2 is insufficient for the reconstructed call sequence.

All needed current references already exist; no new generic MemberRef, TypeSpec, public helper, user string or local signature is required if the typed existing Space_down locals are used and DropDownAll is implemented with typed stack operands. The static evidence establishes caller CFG/bindings/capture/failure timing only. Candidate stack checks, placement/relocation encoding, library/Unity execution, NaN forwarding observations, compiler checks, and acceptance remain outside this subtask.
