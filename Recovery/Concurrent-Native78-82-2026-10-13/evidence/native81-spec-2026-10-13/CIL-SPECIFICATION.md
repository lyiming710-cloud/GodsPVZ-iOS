# Native81 read-only reconstruction specification

Scope: static original PE, original IL2CPP metadata, and library MethodBody inspection only. No candidate construction, CIL/native body execution, tests, builds, compiler invocation, or acceptance work. All newly written files are in this directory. Native55 damaged CIL was used only as a source of reference identities; no behavioral decision depends on its damaged instructions.

The unchanged original files were hashed and both native target pointers/slices independently matched:

| Input | SHA-256 |
| --- | --- |
| Original `GameAssembly.dll` | `9ebd7ca996a5b03fb4a766f7a2502b660d581ddbbb36af4d7bf2f06a211da39d` |
| Original `global-metadata.dat` | `ad992341add498bd1018ac980f42171e0737b0793bf945c8714ad70a445b36b9` |
| Recorded `native55-native.json` | `304b632ad13168da91e4b03e2824f69f053c592f59b3a089756a5e4114445277` |

`PRIMARY-EVIDENCE.json` retains literal bytes, native slices and helper disassembly, original closed-generic identities, original jump table, resolver MethodBodies and hashes, game-reference visibility evidence, and extraction provenance. `SYMBOLIC-CIL.il` is a symbolic recipe only; it is not a compiled or applied candidate.

## 0x060001AC — EnemySelecter.ZombieSelectRandomGet(List<ZombieSelect>, Single)

Original body `[0x18031A490,0x18031A5E4)`, SHA-256 `ff733f276783e28fcd1fd1c5c692374910edde5d49e22afea3fc815d52bfee0b`. It is an instance method but never accesses `this`; the original body uses the List argument and Single argument only.

### Required behavior and CFG

1. Capture the list argument; call the **Single overload** `UnityEngine.Random.Range(+0.0f, maximum)` exactly once. Do this before checking the list for null or creating the enumerator. Pass the maximum argument unchanged, including negative values, infinities, signed zero and NaN.
2. Fail for a raw-null list after Range returns. Otherwise call `List<ZombieSelect>.GetEnumerator()` once, after Range. Thus a Random callback that mutates the same list completes before the enumerator snapshots the list version and first index.
3. Enter the protected loop. Call the real typed value-type enumerator's MoveNext on the address of the same enumerator local. On false, exit to Dispose and return null.
4. On true, get and capture Current once through the public typed getter. Raw-null Current fails on reading weight; it is not skipped.
5. Compare `remaining` as the first operand with `current.weight` as the second operand. If **ordered `remaining > weight` is true**, reread `current.weight`, perform `remaining - weight` in that operand order, round/store as Single, and repeat MoveNext. Otherwise select and return the captured Current after Dispose.
6. Dispose the same enumerator on selected return, exhaustion, or an exception in the loop. Do not construct/box a second enumerator or dispose an uninitialized enumerator if Range or GetEnumerator failed.

The native branch is `comiss remaining, [current+0x1C]` followed by `ja subtract`. For unordered values, `ja` is false and the current item is selected. Symbolic CIL should use ordered **`bgt` to the subtraction block**, with selection on fallthrough. `bgt.un` is wrong. A direct ordered `ble` branch to selection is also wrong for NaN. `!(remaining > weight)` includes unordered selection.

This selects a zero-weight first item when remaining is +0 or -0, selects on equal weights, and selects when either comparison operand is NaN. Negative weights and infinities are not filtered or clamped. When subtraction is taken, its destination/left operand is remaining, its right operand is the *second read* of current.weight; preserve Single rounding on each iteration. No normalization, weight sum, integer random call, random index, fallback-last-element, or Count/indexer loop exists in the caller.

The captured List reference remains the same throughout enumeration. Enumerator MoveNext implements the real list version and unsigned index checks; replacing it with a manually incremented index loses those semantics. There are no user callbacks between the weight comparison and second weight read, but preserving the reread follows the original instructions exactly.

### Original closed generic identities

The original PE metadata registration was located at `0x1818C6D00`. Each method slot below is a tagged usage type 6 and resolves through the original MethodSpec table. All four use class-instantiation index 2549 with its sole argument resolving to original `ZombieSelect`, type index 5158. Nested ownership was resolved through the original nested-type table, not guessed from the name Enumerator.

| Runtime slot | Tagged value | Original MethodSpec index | Original mscorlib MethodDef | Closed operation |
| --- | --- | --- | --- | --- |
| `0x181BC3B60` | `0xC002396F` | 72887 | `0x060023E7` | `List<ZombieSelect>.GetEnumerator` |
| `0x181BCAAB0` | `0xC0022AAF` | 70999 | `0x06002400` | `List<ZombieSelect>.Enumerator.MoveNext` |
| `0x181BCAB08` | `0xC0022AB1` | 71000 | `0x06002402` | `List<ZombieSelect>.Enumerator.get_Current` |
| `0x181BCAA60` | `0xC0022AAD` | 70998 | `0x060023FF` | `List<ZombieSelect>.Enumerator.Dispose` |

Original native GetEnumerator at `0x1808197F0` zero-initializes a 24-byte result and calls Enumerator constructor `0x18066D250`; that constructor records `_list`, zero `_index`, captures list `_version`, and initializes null `_current`. MoveNext at `0x180669280` reads the enumerator's `_list`; checks captured version against current list version; checks `_index` against current `_size` using unsigned ordering; loads `_items[_index]`, assigns `_current`, and increments `_index`. Its rare path checks version again, raises the normal enumerator-version exception on mismatch, or writes `_index = list._size + 1` and null `_current` before returning false. A corrupted/null internal `_items` fails normally. Dispose uses the shared native one-byte `ret` stub at `0x180302170`.

### Actual public getter and library proof

Resolver mscorlib is `audit/stage9-native3/resolver/mscorlib.dll`, SHA-256 `4d1725f1ae54a22f69b79c2b1569181ed6653dd484c35005e29c40300c6673be`. Its saved extraction report identifies the exact selected source as `Editor/Data/MonoBleedingEdge/lib/mono/unityjit-linux/mscorlib.dll` with `exact_editor_target=true`; the currently read bytes match that selected hash. This is the available Unity editor resolver library, not an assertion that its CLR token numbering matches the stripped original PC library.

| Resolver MethodDef | Operation | Public | Body SHA-256 |
| --- | --- | --- | --- |
| `0x06006840` | `List<T>.GetEnumerator` | yes | `61bd96677105bc2a46c8d2dda9be3f854bfbdc0b7acca31bfe4060ee07e74032` |
| `0x06006860` | `Enumerator<T>.MoveNext` | yes | `2a7eedbc98bfcad1071b2ee5b4e7b5258b847ca72545db6233f7da51724bd45d` |
| `0x06006862` | `Enumerator<T>.get_Current` | yes | `6db5e480ae1eb5c90e8385652de06358d3439856cd1d761fb37b1751e1f8b180` |
| `0x0600685F` | `Enumerator<T>.Dispose` | yes | `538b6f46d0e72d0d0c4f89319ee94e4e14f4589ad0526a750ba1f42517008b16` |

The actual Current getter body is exactly `ldarg.0; ldfld !0 _current; ret`. It adds no version check, allocation, exception condition or state mutation. Therefore replacing the native inlined private `_current` read with this public typed getter is proved. GetEnumerator constructs the enumerator; Dispose is exactly `ret`; MoveNext and private MoveNextRare CIL implement the same version/index/current behavior described above. Full bodies and member signatures are in the JSON. Do not call the enumerator's private MoveNextRare from game code; it is called legally inside the real public MoveNext implementation.

The extracted Count and Item getter bodies are retained as additional library evidence only. They are not part of this caller's reconstruction, and should not be substituted for enumeration.

Original float Random.Range is original Unity Core MethodDef `0x0600079B`, `0x18130D350`; available resolver public static float overload is `0x06001A25`, signature bytes `00 02 0C 0C 0C`, with no managed body because it is an engine call. Preserve that helper boundary exactly. This analysis does not reconstruct or assume engine distribution, endpoint behavior, or responses to negative/NaN bounds.

### Exception scope

Original `.pdata` points to unwind data at `0x181A8A740`, flags 3; EH4 function info at `0x181A8A758` has header `0x38`. Decoded IP-to-state entries put state 1 at `0x18031A540` (MoveNext/Current/compare), back to -1 at `0x18031A565` (selected cleanup), state 1 at `0x18031A58A` (subtract/loop), -1 at `0x18031A59B` (exhausted cleanup), state 1 at `0x18031A5D8` (null-Current throw), and -1 at `0x18031A5DE` (rethrow). The try map has try-low=try-high=1, catch-high=2. The catch descriptor identifies `.?AUIl2CppExceptionWrapper@@` in original bytes. The catch continuation offset encoded by `49 04` decodes to 274, reaching `0x18031A5A2`; that landing region calls Dispose, reloads the pending exception, and reraises it.

A CIL try/finally around the loop, entered after successful GetEnumerator, represents those cleanup effects. Both normal return paths must `leave` to a return label outside the handler. Range and GetEnumerator remain outside the protected loop. The raw catch array and continuation arithmetic are retained; the entire native C++ handler ABI was not symbolically reconstructed.

## 0x060003C5 — Project.GetEndPosition

Original body `[0x180377420,0x18037762C)`, SHA-256 `42f01cb0da02d449d49e0f2ebff1a9c2c078c840b5689c7ebce977681747af5e`. The tail `[0x180377608,0x18037762C)` is the nine-entry jump table, not executable fallthrough instructions. The raw-null throw at `0x180377602` terminates the code preceding the table.

### Track 0 block, followed by a fresh track-4 check

On current `movementTracks == 0`:

1. Call this Component.get_transform once and raw-null check the returned Transform. Call that captured Transform.get_position once, obtaining a Vector3 by value.
2. Assign **only** `this.endPosition.x = returnedPosition.x`. Preserve the existing endPosition y/z. This write is before Random.Range.
3. Call float `Random.Range(-380f,120f)` exactly once. A callback can now observe the newly written x and can replace endPosition components, movementTracks, ID, or board.
4. After Range returns, native rereads current endPosition x/y, replaces y with the returned random Single, and writes x/y back; it then reads current z for the by-value camera argument. Thus preserve a Random callback's new x and z; never restore a Vector3 cached before Random. `endPosition.y = sampled` followed by loading the current complete endPosition has the same sequential callback effects. If the implementer requires literal redundant x-read/x-write ordering, use a post-Random x local; no volatile or concurrent-memory guarantee is claimed here.
5. Call same-owner `Project.PositionRevision_Camera(current endPosition)` by value; only after it returns overwrite the entire endPosition with its returned Vector3. A callback sees the intermediate x/y/z field state; an exception leaves that intermediate state intact.

After this block, reread `this.movementTracks` and check whether it equals 4. This is an independent second `if`, not an `else if`, and must not reuse a value captured before either Random.Range or PositionRevision_Camera. A track-0 invocation can continue into track 4 if a callback changed the field.

If the fresh value is not 4, return without reading ID or board. If it is 4, read current signed integer ID and dispatch the following exact jump table; IDs outside 0..8, including negative IDs, take the return/default branch. A normal CIL switch implements the bounds behavior.

| ID | Native branch | Required operation |
| --- | --- | --- |
| 0 | `0x1803774E1` | Current board.banks.GetSunBankPosition; assign entire endPosition; then current endPosition.y **+ 12f**; return |
| 1, 2, 8 | `0x1803775DB` | Current board.projectManager.GetCoinBankPosition; assign entire endPosition; then y **- 5f**, write it; then x **- 85f**, write it; return |
| 3 | `0x180377532` | Current board.projectManager.GetDiamondBankPosition; then exactly the same assignment/y-5/x-85 continuation as Coin |
| 4, 5 | `0x180377595` | Write entire endPosition `(positive zero,115f,positive zero)`; call PositionRevision_Camera with that current value by value; overwrite endPosition with its returned Vector3; return |
| 6, 7 | `0x18037758F` | Return with no branch helper or board read |
| negative or >8 | default | Return with no branch helper or board read |

Do not merge track 0 and ID4/5 into one camera call. Both can occur in one invocation when a first callback changes movementTracks/ID. The ID switch executes once after the fresh track check; after entering an ID case it does not restart if a branch helper changes ID or tracks.

Every board branch reads the current `this.board` at its own branch entry, checks raw null, reads and captures the relevant manager/banks field, checks raw null, then calls its selected helper. The helper result overwrites the entire endPosition before any offset arithmetic. Do not cache board or manager before the track-0 callbacks. Do not skip a branch on null, add Unity fake-null checks, or route an unused/null manager into the ID4/5 case, which does not read board.

### Constants and float ordering

| Use | Original address/immediate | Single bits | Value |
| --- | --- | --- | --- |
| Random minimum | `0x1815A7E70` | `0xC3BE0000` | `-380f` |
| Random maximum | `0x1815A7A40` | `0x42F00000` | `120f` |
| Sun y addition | `0x1815A7BE4` | `0x41400000` | `12f` |
| Coin/Diamond y subtraction | `0x1815A7BE0` | `0x40A00000` | `5f` |
| Coin/Diamond x subtraction | `0x1815A7E24` | `0x42AA0000` | `85f` |
| ID4/5 y | native immediate | `0x42E60000` | `115f` |

For each arithmetic operation, the field component is the left operand and the constant is the right operand. Do not use `12 + y`, `-(5-y)`, or `(x-85,y-5,z)` constructor ordering. Coin/Diamond y subtraction and field write precede the x read/subtraction/write. Preserve Single stores; do not widen calculations to double, rewrite them as vector arithmetic, or canonicalize/sanitize NaNs. There is no addition in the random block; sampled y directly replaces the field.

### Legal references and field access

| Helper | Original Assembly-CSharp token | Signature / access evidence |
| --- | --- | --- |
| Banks.GetSunBankPosition | `0x060000E9` | Public instance `Vector3()` |
| ProjectManager.GetCoinBankPosition | `0x06000204` | Public instance `Vector3()` |
| ProjectManager.GetDiamondBankPosition | `0x06000205` | Public instance `Vector3()` |
| Project.PositionRevision_Camera | `0x060003C6` | Private instance `Vector3(Vector3)`, legal from the same owner Project |

Original public Component.get_transform is Unity Core `0x060007E6`, `0x181304510`; Transform.get_position is `0x06000994`, `0x18132F3A0`. Both must keep their current-module public signatures and ordinary raw receiver-null enforcement.

Original native offsets map to Project.ID (`0x04000516`, +0x24), board (`0x0400051E`, +0x48), movementTracks (`0x04000520`, +0x58), endPosition (`0x04000521`, +0x5C), Board.projectManager (`0x040003D7`, +0x100), and Board.banks (`0x040003DC`, +0x128). Historical game CLR metadata confirms the Board fields, Project ID/board/tracks, and ZombieSelect.weight are public. Project.endPosition is private, but remains legal inside Project. Therefore no game-field public-getter substitution is needed or suggested.

The historical game metadata evidence is from unchanged `audit/stage9-native3/inputs/59bb-historical.dll`, with its hash and exact signatures recorded in the JSON; it establishes lineage reference/access evidence, not the current parent DLL's final rows. The implementer must resolve and verify those concrete references in the current parent.

## Required current-parent bindings, typed locals and gaps

The main agent supplied `audit/native80-codex-2026-10-13/metadata-current.json`. It was read here and a focused subset is saved in `PRIMARY-EVIDENCE.json`. Its recorded parent SHA-256 is `3b3bcfca51402e1eedf5d59fbe1bf9d923336730325550205f960bd8a8bceec6`. This is saved current-parent inventory evidence; the current parent DLL itself was not loaded independently in this subtask.

Current 1AC is 225 bytes, max stack 2, local signature `0x11000154`, no EH clauses. Its existing Single locals V0/V3/V9, List<ZombieSelect> V1, and ZombieSelect V5/V11 can be reused. Its Enumerator<Object> V7/V14/V15 are unsuitable for the repaired typed loop. Append one concrete value-type `List<ZombieSelect>.Enumerator` local (or replace the target local signature deliberately within authorized scope), initialize it from GetEnumerator, and add the real finally clause. Current List<ZombieSelect> TypeSpec is `0x1B00003E`; Enumerator TypeRef is `0x01000014` in mscorlib. The correctly closed typed Dispose MemberRef already exists at `0x0A000138`; obtain its declaring closed TypeSpec rather than inventing a new nested binding. Typed ZombieSelect GetEnumerator, MoveNext and Current are absent from the saved member-reference set and need correctly closed MemberRefs. Existing Buff bindings `0x0A0000B7–0x0A0000BA` show the established generic-signature pattern. Existing MoveNext `0x0A000024` is closed over Object and must not be reused for the ZombieSelect enumerator.

Current 3C5 is 180 bytes, max stack 2, local signature `0x110002F3`, no EH clauses. Its Transform V2, Vector3 V3/V4/V6, and Single V5 are suitable for the reconstruction. It has existing public getter refs `0x0A000001` (Component.get_transform), `0x0A000037` (Transform.get_position), and float Random.Range `0x0A000083`; the same float Range ref is used by 1AC. Vector3 component refs are x `0x0A00001D`, y `0x0A00008B`, z `0x0A00009D`, and Vector3 TypeRef `0x01000013`. Current game MethodDefs `0x06000204`/`0x06000205` are the public Coin/Diamond helpers and `0x060003C6` remains the private same-owner Camera helper. Current Project fields preserve IDs and access reported above, including private `endPosition`; current ZombieSelect.weight is public `0x04000202`. The saved internals subset omits Board and Banks, so their current concrete field/helper rows still require the main agent's direct current-parent lookup; historical/native identity evidence is already established.

For EnemySelecter, verify the current target's instance signature and bind its List<ZombieSelect> and Single argument indexes. Use typed locals for the captured List, remaining Single, `List<ZombieSelect>.Enumerator` value, captured Current ZombieSelect, and return ZombieSelect. Required public references are exactly float Random.Range, typed List.GetEnumerator, and the same typed Enumerator's MoveNext/Current/Dispose. Generic definitions contain `!0` signatures while their declaring TypeSpec is closed over ZombieSelect; do not close the owner over Object or manufacture a reference-type enumerator. Ensure the actual enumerator local receives GetEnumerator's return before any `ldloca` use. Define a real finally handler and use legal `leave` from protected normal-return paths.

For Project, verify same-owner fields and PositionRevision_Camera, public Board fields and selected helpers, public Component/Transform getters, float Range overload, and Vector3 component fields. Useful typed locals are Transform, returned/camera-argument Vector3, sampled Single, optional post-Random x Single, Board, Banks, and ProjectManager. A `Vector3` helper argument/return is by value; addresses are used only to access/construct value locals or actual Vector3 field components.

The original caller CFG, constants, closed generic helper identities, typed Current getter equivalence, resolver enumeration behavior, jump table, callback ordering, field-write timing, and raw-null behavior are evidenced here. Full C++ exception ABI reconstruction, engine Random internals, current-parent concrete token/local validation, candidate stack validation, compiler behavior, exact executed float/NaN observations, acceptance, and game/iOS fidelity remain outside this read-only subtask. The CIL Single rounding strategy must preserve the per-iteration float32 subtraction; adding an explicit `conv.r4` is a conventional coercion option but is not an executed NaN-payload equivalence result.
