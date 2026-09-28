# Native16: sorting and associative-list removal

Experimental reconstruction on the exact native15 lineage. No production
promotion, full Unity export, or device-runtime qualification is claimed.

## Authority and write boundary

Only these MethodBodies are replaced:

| Method | MethodDef | Global definition | Reference VA | Fully shared VA |
|---|---|---:|---|---|
| VFXAnimationEvent.SetSorting<T> |0x060000C6|41963|0x1804C4F70|0x1804C4BB0|
| FTRuntime.Internal.SwfAssocList<T>.Remove |0x060008D9|44030|0x1809D3850|0x1809D3A90|
| FTRuntime.Internal.SwfList<T>.UnorderedRemoveAt |0x060008E9|44046|0x1809D52C0|0x1809D53A0|

The third method is a necessary damaged dependency of Remove. Its original
native implementation is inlined in Remove and also exists independently;
both bodies corroborate the same operation. No other list helper is replaced.

PC GameAssembly SHA256:
`9ebd7ca996a5b03fb4a766f7a2502b660d581ddbbb36af4d7bf2f06a211da39d`.
PC metadata SHA256:
`ad992341add498bd1018ac980f42171e0737b0793bf945c8714ad70a445b36b9`.
`mapping.json` records the generic MethodSpec, class/method instantiation,
pointer-table index, exact .pdata extent, and body hash for all six bodies.
Class instantiation is essential for the Swf generic types; method_inst=-1
does not mean that their native implementations are nongeneric.

## MethodBody specification

SetSorting allocates List<GameObject> and calls LoopAddAnimation(this.transform,
list) before any state/host check. Unless state==30 and boxed host is Plant,
return. Lookup `Lower`, then evaluate plant.board.boardConfig.GetGridPosition
(GridX, GridY+1). Call lower.GetComponent<SpriteRenderer>() separately for
sortingLayerName="Entity" and sortingOrder=(int)((position.y+1f)*-10f).
Lookup `Upper`, reread the board/config/coordinates, and use GridY-2 and
(int)((position.y-1f)*-10f). Four independent GetComponent calls are preserved.
All null failures retain their position in this call order. Neither the
particleState field nor Binding is written/called by this method.

The native annotations identify the literal strings, float constants,
GetComponent MethodSpec 74359 and calls to GetAnimationSprite_Name (0x06000142),
GetGridPosition (0x0600015B), and LoopAddAnimation (0x060000C7). Plant GridX,
GridY and board offsets are 0x20, 0x24 and 0x1E8; Board.boardConfig is 0x28.

Remove first calls _dict.TryGetValue(item,out index). If absent, return.
Remove the dictionary entry, then call _list.UnorderedRemoveAt(index).
Call _comp.Equals(moved,item) in that argument order even for a last-element
removal. If false, write _dict[moved]=index. The comparer sees both earlier
mutations; if it throws, the final dictionary update has not happened.

UnorderedRemoveAt checks `(uint)index < (uint)_size`, otherwise throws a new
IndexOutOfRangeException. Read moved=_data[_size-1], write _data[index]=moved,
then clear _data[--_size] to default(T), and return moved. There is no extra
index decrement and no replacement of default(T) by a null literal. The type
of the explicit range exception is independently decoded from native slot
0x181BA8DC0; the generic field-offset metadata itself contains zero offsets,
so it is not used as proof of the instantiated Swf field layout.

## Verification

`python3 scripts/codespaces/test_native16_fixture.py` compiles a surrounding
stub fixture, replaces its three actual target bodies using PatcherNative16,
and executes them on CLR. **37 assertions passed** in Codespace, including
reference/value/struct types, swap/clear/return behavior, missing keys, invalid
indices, corrupt backing state, comparer argument/exception order, component
call identity and ordering, float truncation, invalid hosts and null paths.
This is controlled behavior testing, not a claim about Unity runtime visuals.

Three deliberately damaged-stack controls are rejected. The patcher checks
identity counts, MVID, and semantic fingerprints of all non-target bodies,
both before writing and after reopening. The default Unity resolver cannot
fall back to the host runtime; System.Private.CoreLib leakage is rejected.

`python3 scripts/codespaces/validate_native16.py` rebuilds and replays the
native13/14/15 positive controls, verifies native15 input hashes, materializes
linked and unlinked native16 twice, and runs the exact pinned IL2CPP converter.
Only a zero exit is full direct-converter success. Target closure with new
failures is recorded separately and does not justify a full Unity rerun.

At source `a1673f63faaab2346bb96e43efcda1d47c651d51`, the Codespace replay
completed in 187.29 seconds with **NATIVE16_TARGET_CLOSURE_PASS**. IL2CPP still
exited 255, now reporting only SwfList<T>.AssignTo(List<T>). Full Unity was
therefore not dispatched. All three target bodies passed, 2294 non-target
MethodBodies were isolated, and both outputs reproduced byte-for-byte.

- Unlinked: `51a749bde92ec75e53dbeed5d35d083ba3616d30f4e4fd756af48a0bd3aed1a5`
- Linked: `7576209f5c31c673434f95975d5e57cd8140f2b16f5608ddb8a521e6e721eadc`

Raw validation logs and result JSON are archived under `validation/`.
