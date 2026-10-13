# Native89 static SkillManager caller specification

Two original-native-backed static caller bodies: `0x06000517 SkillManager.Awake149(Plant,Zombie,Projectile,Damage)` and `0x06000510 SkillManager.Function16(Plant,Zombie,Projectile,Damage)`. Only argument0, the original Plant, participates in these callers. The other supplied object arguments are not substituted for the newly created particle/projectile/Damage. There is no SkillManager instance receiver.

Source evidence is the complete `source-evidence.json` in the earlier unfinished `native88-spec-2026-10-13` investigation. Original PE SHA256 is `9ebd7ca996a5b03fb4a766f7a2502b660d581ddbbb36af4d7bf2f06a211da39d`; metadata SHA256 is `ad992341add498bd1018ac980f42171e0737b0793bf945c8714ad70a445b36b9`. Actual parent82 CLR source read there hashes to `48b9e89e941c041d6c11c4064ad05d9384a1a4dd51d5f8d3fad9192cbda5adbf`. Earlier83–88 edits append metadata or alter explicitly distinct bodies; this implementation also guards relevant signatures/local types/access in its actual pinned parent88.

## Original bounds and exact references

Awake149 is original `0x180381670–0x1803817B5`,325 bytes. Function16 is original `0x180382D80–0x18038300D`,653 bytes. Original pointer entries and entire pdata body bytes are retained, including NRE/bounds helper branches. Both methods' original/current signatures are static void(Plant,Zombie,Projectile,Damage), current blob `0004011281e01282101281f4128094`.

Original usage `0x181BA5578` resolves MethodSpec74369, GameObject.GetComponent<VFXAnimationEvent>, not Component.GetComponent and not another component type. Existing current token `0x2B000069` refers to GameObject generic MemberRef `0x0A000079` with instantiation `0A011254`; the type is CSharp VFXAnimationEvent `0x02000015`. The shared original generic pointer is `0x18046DC80`, with canonical records and direct usage proof saved in the evidence.

Original Binding usage `0x181B9E278` resolves MethodSpec74722 `[41961,-1,2453]`: public instance `VFXAnimationEvent.Binding<T>(T host,bool original,float32 time,string type)`, original/current MethodDef `0x060000C4`, method argument **Plant**, never Object/Zombie. Its canonical System.Object generic row60994 selects pointer60832 at cell `0x1817D6E10`, actual native `0x1804C4AB0`. The current generic definition is public, one method parameter T, no type/special constraints, signature `300104011E00020C0E`. New exact MethodSpec `0x2B0000B8` refers to that existing MethodDef with instantiation **`0A011281E0`** (current Plant TypeDef `0x02000078`). No generic definition is rewritten and no hand-made Binding behavior replaces the call.

`ParticlesManager.get_instance` `0x060001E5` is the already proven public static pure getter for the same private backing field in original native and actual managed source. `CreatNewParticle` is `0x060001EA`. Plant.blockable `0x040004C4` is public Boolean; original particle-state argument is exact int32 bits35. Original string usage `0x181BAA518` is literal3737, exactly **PlantSkill**. The implementation reuses its exact existing user-string entry.

Function16 uses public Plant.GetDamage **`0x0600036C(Projectile,int32)`**, actual signature **`20021280941281F408`**. Native R9=0 is the hidden IL2CPP MethodInfo argument. It is **not an extra managed specialType argument**. The nearby `0x0600036D(Projectile,int32,int32)` overload is a different method. This identity was independently checked against original parameter definitions and actual parent88 CLR bytes before the successful generation.

The public Plant.AnimationSprites getter `0x060003B9` legally represents the private inherited field read. Its actual managed body is exactly `ldarg.0; ldfld 0x040004DF; ret`, return signature List<GameObject>. Original native `0x18034DE90–0x18034DE98` is exactly `mov rax,[rcx+0x148]; ret`, SHA256 `63fb7a16952265c2f121170b4d1bcaf818295178dd1d839f957793af5ec479dc`. This getter introduces no application callback or alternate receiver selection.

Damage.FindElement `0x06000110` is public instance Element(ElementType), original native `0x180311B90`. Value0 is passed unchanged. Existing Projectile.Initial<Plant> `0x2B000060` points to the six-argument public definition `0x060003ED`, instantiation `0A011281E0`; native usage `0x181BBB7C8` resolves MethodSpec74552 `[42770,-1,2453]`, shared body `0x1804A25F0`. This is distinct from the new Initial<Zombie> used in Native85.

## Awake149 order and locals

1. Write `plant.blockable=true`; a raw-null Plant faults before any particle getter/creation.
2. Get current ParticlesManager through the proven public getter, raw-null-check at the creation call, create particle35 once, retain the returned GameObject in existing V1 and raw-null fail if absent.
3. Call that captured GameObject.GetComponent<VFXAnimationEvent> once; retain V2, raw-null fail if absent. Call its exact Binding<Plant>(originalPlant,true,**+0f**,**PlantSkill**) once. The component and particle survive any Binding callback replacement of Plant fields or global particle state.
4. Call capturedParticle.get_transform; retain V3 **without failing on a null result yet**. Call originalPlant.get_transform, retain V7 and fail if it is null; get_position into existing Vector3 V4.
5. Only after plant position returns, fail for raw-null V3, then pass captured V4 by value to V3.set_position. Return. There is no scale, audio, Unity pseudo-null check, reassignment of the supplied Zombie/Projectile/Damage, or finally.

Keep local signature `0x110003D9`, existing typed GameObject V1/VFX V2/Transform V3/Vector3 V4/Transform V7, MaxStack5. Other erased/unused locals are not retagged. Exceptions propagate with the preceding blockable write and callee effects retained.

## Function16 capture, arithmetic and partial writes

1. Read originalPlant.board into existing Board V1, then originalPlant.skill into existing Skill V2 **before** the board-null guard and projectile creation. The Skill object remains captured even if later callbacks replace plant.skill.
2. Read capturedBoard.projectileManager and create projectile19. Retain returned Projectile V8, **without checking its null return yet**.
3. After creation, read current animationSprites through its proven public getter, pass that list and exact **PeaShooter_mouth** to GlobalStaticVars.GetAnimationSpritePosition `0x06000141`, save the complete by-value Vector3 in V9. No list guard is added.
4. Call originalPlant.GetDamage(capturedProjectile,19) using the exact two-parameter overload. Capture returned Damage V4, raw-null fail if absent. Thus a null projectile still allows the position helper and GetDamage to execute.
5. Read capturedDamage.damagePoint before dereferencing capturedSkill.value_float. Capture that first Single[] in V5. Read index0, perform **damagePoint left × factor right**, round to Single, write damagePoint. Then read index1 from the **same captured array**, write penetrationVigour. A short array can fail after the damagePoint write; do not combine checks or roll back that partial write.
6. Call capturedDamage.FindElement((ElementType)0). Retain Element V6. Raw-null return skips only Element scaling. If nonnull, read capturedElement.point before rereading capturedSkill.value_float into V7, read index2, perform **point left × factor right**, round to Single and write that same Element.point. A callback may replace the captured Skill object's array; it does not replace the captured Skill receiver or captured Damage.
7. Only now fail for raw-null capturedProjectile. Get the stored Vector3.zero through its proven public getter after the check. Call existing Initial<Plant>(capturedPosition, speed(1675,+0,+0), capturedZero, **70f**, movementTracks0, originalPlant), all Vector3 arguments by value. Native constants are bits **`0x44D16000`** (1675) and **`0x428C0000`** (70). There is no plant.fZ read or offset addition here.
8. After Initial returns, read current originalPlant.camp **before either final projectile field write**. Write capturedDamage to capturedProjectile.damage, then that captured Camp to capturedProjectile.camp. The stack carries receiver/camp below the intervening damage assignment, preserving this read/write order. Return.

Keep local signature `0x110003D4`, existing Board V1/Skill V2/Damage V4/Single[] V5/Element V6/Single[] V7/Projectile V8/Vector3 V9, MaxStack7. Arrays use real `ldelem.r4`; no preflight Count/Length checks replace their actual null/index faults. Original uses raw-null checks, not Unity fake-null predicates.

## Construction and pending acceptance

Parent88 SHA256 is `bff7fa2956077e5ac06a4b903fbb5a1be4aad989ac6b5cb08fd7d0aa6e690951`. The first generation stopped at a guard that found the helper's actual two-parameter overload; it wrote no candidate. The incorrect pre-guard draft and failure record are retained. The corrected version removes the extra managed zero, preserving the native hidden-MethodInfo distinction. This is an implementation/source correction, not a failed/passed game acceptance test.

The successful candidate adds one MethodSpec and no local/TypeSpec/MemberRef/access/signature mutation. Both full bodies fit their original slots, so their MethodDef RVAs remain unchanged. Existing helper definitions and shared local rows are unchanged by construction; Gemini must verify that isolation and reproducibility on actual output bytes.

Every fixture, original-machinecode comparison, compiled actual-CIL execution, NaN/bit-level/exception/callback observation, effective negative control, frozen verifier, access/generic check, raw body/metadata isolation, malformed/forged input and historical regression is pending with Gemini. In particular prove the real typed Binding/GetComponent/Initial identities and the AnimationSprites/Particles public substitutes against the current runtime, not solely a permissive test stub. No Unity, Xcode, device test or IPA acceptance is claimed.
