# Stage9.1 Card_Choose.Initialize(PlantSave) native evidence

## Scope

This patch is intentionally limited to one MethodDef:

- Managed target: `System.Void Card_Choose::Initialize(PlantSave)`
- MethodDef token: `0x060002F2`
- PC IL2CPP method-pointer index: `753`
- PC x86-64 VA: `0x0000000180343BE0`
- Function boundary used for reconstruction: `0x0000000180343BE0..0x00000001803442BF`
- Next mapped function: `Card_Choose.Lock()` at `0x00000001803442C0`

Authority hashes:

- `GodsPVZ_1.0.2.zip`: `2f08b4e2243e2bd296f3db694b4a2c9656252d76202c80167bdf3340b9f65b48`
- PC `GameAssembly.dll`: `9ebd7ca996a5b03fb4a766f7a2502b660d581ddbbb36af4d7bf2f06a211da39d`
- PC `global-metadata.dat`: `ad992341add498bd1018ac980f42171e0737b0793bf945c8714ad70a445b36b9`
- Exact managed input candidate: `4a2e07d7813f6b56cd125a212c25c6998a36fe9312f763814a786ce218a1b1ab`

The current managed body is known invalid at runtime: local 8 is `System.Object`, while the corrupted recovered IL executes `ldc.i4 0; stloc 8` at IL_004B/IL_0050. The strict runtime gate therefore fails before Board construction with `InvalidProgramException` in this method.

## PC native behavior reconstructed

The PC function performs the following managed-visible behavior, in order:

1. Stores the incoming `PlantSave` in `Card_Choose.plantSave`.
2. Copies `plantType`, `ID -> plantID`, `level`, and `order -> levelOrder`.
3. Iterates `ordersImage`; each image GameObject is active iff its index is less than `levelOrder`.
4. Copies `sunPrice`, `replantCD -> CD`, and `stars`.
5. Iterates `starsImage`; each image GameObject is active iff its index is less than `stars`.
6. Sets `levelText.color` to `(0.9, 0.9, 0.9, 1.0)`. The 16-byte native constant at `0x1815A7CE0` decodes to those four floats.
7. If `PlantSave.clique != 0`, activates `cliqueLogo` and assigns `ResourceManager.cliqueLogos[clique]`; otherwise deactivates it.
8. Assigns `plantPortrait` from `ResourceManager.card_Choose_PlantPortraits[plantID]`.
9. Assigns the star-indexed `card_Choose_Data`, `card_Choose_LvBackgrounds`, `card_Choose_Innerlining`, and `card_Choose_Outerlining` sprites to the corresponding card images.
10. If `levelOrder > 0`, assigns `ResourceManager.card_Choose_OrderArabesques[levelOrder]` and activates `orderArabesques`; otherwise deactivates it.
11. Writes `sunPrice.ToString()` and `level.ToString()` to the two TMP labels.
12. Selects the source skill using PC native switch semantics: `defaultSkillID == 2 -> skill2`, `== 3 -> skill3`, every other value -> `skill1`.
13. Calls `Skill.Copy()` on the selected source and assigns the result to `Card_Choose.skill`.
14. Calls `Skill.Copy()` a second time on `Card_Choose.skill` and assigns that second copy back to the field. The double-copy sequence is present in the PC binary and is preserved deliberately.
15. Calls `PlantSave.GetSkill_ID(firstCopy.ID)` using the first copied Skill's ID.
16. Computes the skill-logo index as `PlantSave.ID * 3 + resolvedSkill.ID`.
17. Calls `ResourceManager.LoadSkillLogo(index, resolvedSkill.name)`, assigns the returned Sprite to `skillLogo`, and activates its GameObject.

No unrelated method is repaired by this patch. IL2CPP runtime bookkeeping, class-initialization guards, GC write barriers, null-failure stubs, and array bounds stubs are represented by normal managed IL operations rather than copied machine-code scaffolding.

## Required acceptance checks

The patcher must refuse any input whose SHA, MethodDef count, target signature, token, or known corrupt-body fingerprint differs. After writing, it must reopen the output with Mono.Cecil and prove the target has exactly three typed locals, no `System.Object` corruption locals, exactly two `Skill.Copy` calls, one `PlantSave.GetSkill_ID`, one `ResourceManager.LoadSkillLogo`, the expected UI/resource operations, and no exception handlers introduced by the reconstruction.

All other MethodDefs are semantic-hashed before and after. Acceptance requires exactly `2316` untouched methods and exactly one changed method (`0x060002F2`). Independent ILSpy decompilation is then required before the candidate can proceed to the unchanged strict R3 PlayMode Board gate.
