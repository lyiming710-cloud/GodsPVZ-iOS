# GodsPVZ Native83–88 implementation and Native60 acceptance checkpoint

Updated 2026-10-13. **Native60 is now explicitly accepted at `APPROVED_PREBUILD_SCOPE`, advancing qualified unique methods since Native53 from40 to42.** This promotion follows passive integrator review of AGY Gemini's executed, independently bound native/CIL evidence. The exact acceptance receipt and frozen evidence archive are included. It is not a Unity/Xcode gate result or a whole-game acceptance.

The six new batches below add **12 unique pending methods**. Native61–82 retain70 pending methods, so Native61–88 have **82 pending methods**. Accepted plus pending implementations total124. The100-method trigger counts **independently qualified unique methods**, so it remains42/100. Generation, compilation of patchers and an `OK` report do not qualify a method.

The integrator implemented changes, built patch generators, generated candidates, reviewed source and already executed Gemini evidence, and checked archive identities. **The integrator executed no acceptance tests.** All native-machinecode/CIL runtime comparisons, fixtures, valid behavioral mutants, frozen verifier, isolation/access/repro/malformed/historical checks, conversion/TU/Unity/Xcode gates belong to `GodsPVZ — AGY Flash 协作验证`, thread `01a12e9a-49e8-71c1-acf0-a634ac5215af`, AGY Gemini3.8Flash. Tasks83–88 were delivered while implementation continued.

## Candidate chain and actual target scope

Parent82 is `48b9e89e941c041d6c11c4064ad05d9384a1a4dd51d5f8d3fad9192cbda5adbf`.

|Batch|Candidate SHA256|Methods|Implementation boundary|
|---|---|---:|---|
|83|`d1727135f7fcb7141a62ad5bbe0e2342e6d25efb97d25b59ae223bb26fcd9975`|3|`06000056` InsertNewPath, `06000119` HoleTeleport constructor, explicit transitive `060000E0` AttackRange constructor; one typed public List<Path_enemyEditor>.Insert reference.|
|84|`b3284d01f0a57c9d22f6297741e96a831874cb0a584af2e165b98c42def84587`|2|`060001C8` DropDownAll, `060002CE` Space_down; complete mouse body relocated, only its target RVA changes; no new rows/heaps/locals. Current file is candidate3.dll with generation3.json.|
|85|`ba1b6ae5cd5b27b89797c16484cab5a6eb52157deab0bd4251e5fefe1aa562b0`|2|`06000443` GetDamage, `0600047D` Shooting; exact six-argument Initial<Zombie> MethodSpec `2B0000B7`, complete body relocation.|
|86|`20ec9187fb3edc51d80cd486a70f6e5e91d7a1958ce4f4db456a2bee7124fa11`|2|`060006DF` OnPointerClick, `060006FE` Yes; one proven public position getter and one target Vector2 local; complete Window relocation.|
|87|`78aae6f5b3e96006a5bf278fabe1a7e86dbfcd7dfe2ac6279d1df47035d272d8`|2|`06000199` PlayAudio, `06000386` PC_PotatoBoom; existing references/locals, complete oversized caller relocation. Known audio helper ordering defect remains explicit in this intermediate snapshot.|
|88|`bff7fa2956077e5ac06a4b903fbb5a1be4aad989ac6b5cb08fd7d0aa6e690951`|1|`0600013E` four-argument CreateAudioAtPoint; original105-byte prefix retained, length read moved before timeScale with a scoped Single local, complete148-byte body relocated. Full helper/dependency qualification pending.|

All parents are the immediately preceding row. These are generated pending candidates on a chain that still contains unqualified earlier batches. They must not replace the accepted Native60 base or be presented as release-ready game DLLs.

## Fidelity decisions

- InsertNewPath retains pre-allocation grid arguments, post-constructor myPath, callback-sensitive editor/index rereads, wrapping index+1 and parent transform acquisition before child-null failure. HoleTeleport writes fields before one real Hide construction. AttackRange's Rect/Single/Vector3 list construction order is reconstructed from its complete native body; the diagnostic-string residue is removed only in this explicit constructor target.
- DropDownAll preserves its exact dispatch and distinct Unity predicates, rereads crosshair/prop/board after helpers, permits static Destroy of a currently null crosshair, captures the Fold receiver, and keeps Reset as ordinary tail flow. Space_down continues after BoardResume, captures pause state before the pauseTemp write and preserves clip/position/volume/UI/log/time partial-write order.
- GetDamage uses the same managed ref slot for ordered Type/Range helpers, delays the final null guard, preserves two independent ID snapshots, Single arithmetic and GetMS-left multiplication followed by result+result, and rereads board after AddElement. Shooting keeps position capture and delayed nullProjectile failure, exact speed bits, fresh fZ/camp and the actual Zombie generic instantiation.
- TextLink keeps old text before event/camera, a by-value Vector3, Find's saved index, fresh text after Find, a copied TMP_LinkInfo value and one captured ID across Log. Window_Q preserves distinct early-return and Destroy paths, Unity versus physical-null gates, captured list/item, second index read, fresh MainUI after deletion and no invented finally.
- Dialogue audio uses the original integer Random pairs `(1,3)/(4,3)/(7,2)/(9,3)`, capturing the list before Random and checking it afterwards. PotatoBoom discards AreaDamage's Boolean, uses Shake(0.08,5), delays the first effect-transform fault until after plant position, gets transform twice on the same particle and does not skip camera/volume/Random for a null clip.
- The independently discovered audio helper defect is repaired separately: clip.length precedes the first timeScale getter, the ordered floor comparison preserves the optional second getter including NaN, and duration retains length-left/factor-right Single multiplication. Preserving its earlier prefix does not prove all engine helper dependencies; those remain in Gemini's complete-method acceptance scope.

Legal public getter substitutions have original-native and actual managed source evidence. PointerEventData's public getter reads y then x whereas the caller inline reads x then y; there is no intervening callback. The claim is sequential stable-field/callback equivalence, **not equivalence under concurrent field mutation**.

## Evidence and current review state

Native60 accepted candidate: `4e1b743d87db3d78816b37e0a114b105e59b0f25fcbe6cb90e0797378d188b35`, parent59 `d198bb3f63a21d95d708a66546b3b12529d52aee810508712db6e32f43b021c0`. Frozen reviewed archive: `Native60-reviewed-evidence-0b21024f.zip`,2,211,909 bytes,75 members, SHA256 `0b21024f659fdab0caca461b48bde50975af2cfd71c95e8da6ee594466e4fb95`. `qualification/native60/REVIEW.md` and `ACCEPTANCE.json` bind the actual historical compiled oracle, source, embedded runner sources, executed full15-output TSV, valid behavioral controls, frozen/isolation/access/repro/historical evidence. Only the two recorded Native60 targets are promoted.

Native61 v7 is still pending. Passive review now confirms13 observed/expected output columns and an executed wrong-value behavior control for376. It also still finds unified Call emission for original callvirt, a five-file incomplete archive, and cases0 infinite-loop/emission rejections mixed with behavior controls. Explicit feedback asks Gemini to preserve receiver-null semantics, add independent null cases, disaggregate malformed controls and archive the complete executable/runners/rawlogs/dependencies. A report's `pass: true` does not override these gaps.

Native80 material helper qualification still needs binding to the actual current linked/player Core runtime. The original PC alias and an identified historical Native18 player C++ getter are source evidence, not current candidate acceptance.

## Durable recovery and history limits

During implementation `/tmp/godspvz-native83–85...` and older temporary directories disappeared. A later read found only the newly recreated86 directory. The cause of that reset has not been established; it is not a game test result. Parent82 was restored from the immutable Native78–82 Release archive, SHA256 `50823009a158de06a8e40056491d0fd0189fadcac87738abc250bc398de0d282`. Regenerated83–85 artifact bytes match their earlier generated hashes.

New batches are archived immediately on persistent `/workspaces` under `.validation/native83-86-2026-10-13-checkpoints/nativeNN-generated.zip` and copied locally. The joint83–86 build checkpoint is19,984,653 bytes, SHA256 `113e4771c2628b133016712008c2a5d36e07f41e524656a5b620c54b8e141399`. Latest rebuilt sources/binaries/generation/build logs and available original generation/failure records are included. Some pre-reset temporary first-build stdout/cache was not recovered; this archive does not claim to reconstruct missing historical logs.

Current direct temporary input paths are `/tmp/godspvz-nativeNN-codex-2026-10-13/` (candidate1.dll except84 candidate3.dll). If reset again, extract the immutable Release/checkpoint archive to an ordinary independent work directory and use its recorded hashes. Do not edit read-only references or rely on temporary paths as durable inputs.

The incomplete SkillManager investigation in `evidence/native88-spec-2026-10-13` is source reference work only; it adds no implemented or qualified method.

## Remaining gates

The last complete conversion/TU measurement is the historical Native53 result:516 errored methods,5668 diagnostics and24 failing translation units of264. No current full measurement exists, so do not subtract12 methods from those counts. No new Unity/Xcode workflow was started, no Apple compile or device test was executed, and **no qualified IPA has been produced**. Continue implementation and Gemini acceptance concurrently; reach100 qualified unique methods before the deferred full conversion/all-TU gate, then require the licensed Unity export and real Apple Xcode build for an unsigned IPA.
