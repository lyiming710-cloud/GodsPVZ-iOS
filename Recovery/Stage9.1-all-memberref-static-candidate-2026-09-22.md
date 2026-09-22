# Stage9.1 all remaining MemberRef static candidate — 2026-09-22

Status: `ALL_REMAINING_MEMBERREF_CLEAN_STATIC_CANDIDATE_PASS`

This checkpoint was reproduced in the current Linux container without triggering any GitHub Actions workflow. It is a static candidate proof, not yet a production-migrator integration beyond B001.

## Locked input

Run-8 work-copy SHA256:
`f47b1b37f1d4d3a1ddeb44bb6dbe399c58dc6dee3d4602618b2790f839d3e321`

Clean candidate SHA256:
`b07254d5c4077e013bbc934734f7b0949c0942a4e667d6d2b23d2af6c830f720`

File size remains `1201152` bytes. The candidate differs from run 8 by exactly `50` byte positions.

## Generic MemberRef normalization

All changes reuse signature blobs already present in the same DLL and retain each existing closed generic Parent TypeSpec.

### List<T>.GetEnumerator()
Nine recovered concrete-return signatures are redirected to the already-existing canonical definition-level signature blob `#Blob 0x1C24`:
`20 00 15 11 55 01 13 00` = `instance Enumerator<!0> GetEnumerator()`.

Targets: `0x0A0000B4`, `0x0A0000C9`, `0x0A0000D0`, `0x0A0000D6`, `0x0A0000E4`, `0x0A0000FB`, `0x0A000141`, `0x0A000162`, `0x0A00021D`.

### Enumerator<T>.get_Current()
Eleven recovered concrete-return signatures are redirected to canonical `#Blob 0x4630`:
`20 00 13 00` = `instance !0 get_Current()`.

Targets: `0x0A0000B5`, `0x0A0000C3`, `0x0A0000CA`, `0x0A0000D1`, `0x0A0000D7`, `0x0A0000E5`, `0x0A0000E7`, `0x0A0000FC`, `0x0A000142`, `0x0A000163`, `0x0A00021E`.

Same-parent proof exists inside the DLL: canonical MemberRef `0x0A0001A9` has the same closed `Enumerator<Zombie>` Parent as bad `0x0A0000D7`, but uses `20 00 13 00` rather than a concrete Zombie return type.

### List<T>.Insert(int,T)
Two recovered concrete-T parameter signatures are redirected to canonical `#Blob 0x0976`:
`20 02 01 08 13 00` = `instance void Insert(int32,!0)`.

Targets: `0x0A000140` (`List<EnemyPath>`) and `0x0A00022B` (`List<PlantSave>`).

## transform field-recovery correction

The two remaining bad MemberRefs are not fields in Unity. The same DLL already contains exact same-parent canonical getter rows.

- `0x0A000149`, Parent `UnityEngine.Component`: recovered `transform` / field sig `06 12 11`; canonical same-parent `0x0A000008` is `get_transform` / method sig `20 00 12 11`.
- `0x0A000284`, Parent `UnityEngine.GameObject`: recovered `transform` / field sig `06 12 11`; canonical same-parent `0x0A00002C` is `get_transform` / method sig `20 00 12 11`.

The candidate canonicalizes each bad MemberRef row in place to the exact same-parent getter name/signature, then changes only the opcode at its three proven IL use sites from `ldfld (0x7B)` to `callvirt (0x6F)` while retaining the original MemberRef token:

- `EnemyManager.PlayBoardAudio`, `0x060001BC`, IL `0x19`, file `0x1E569`, token `0x0A000149`.
- `FlagMeter.Update`, `0x06000617`, IL `0x2A`, file `0x83E4E`, token `0x0A000149`.
- `FlagMeter.Update`, `0x06000617`, IL `0x77`, file `0x83E9B`, token `0x0A000284`.

The recovered call graphs support the intended semantics: `Camera.main -> transform -> Transform.get_position()` and `List<GameObject>.get_Item -> transform -> Transform.GetChild()`. Existing recovered methods already use `callvirt` for identical getter chains.

## Boundary

This proves the raw metadata/IL normalization and exact candidate hash in the current container. It does **not** yet claim a fresh Mono.Cecil all-reference audit or Unity/IL2CPP export. Therefore the additional repairs remain candidate-only until an independent resolver audit is reproduced. The production migrator still formally integrates only B001 in commit `6b52f719f70232131ca3de0b4d431adcb3244f67`.
