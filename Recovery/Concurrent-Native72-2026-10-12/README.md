# Native72 pending implementation — 2026-10-12

**UNQUALIFIED_IMPLEMENTATION_SNAPSHOT.** Native59 remains the accepted base at 40/100 pending review of Gemini's Native60 evidence. Native60–71 contain 40 pending unique methods; this batch adds four, for 44 pending. The integrator has executed no acceptance tests. All test execution is delegated to AGY Gemini 3.8 Flash in conversation `01a12e9a-49e8-71c1-acf0-a634ac5215af`.

Parent Native71 SHA: `e2cbc546cf479f12076d27661aabecf77acf00f27c02eca3f939983eb2794a8e`.

Candidate Native72 SHA: `a830ce9a66331c669cdc7d77e191855b2409a2f2ef9f6aa1448f0e6dadfdd0dc`.

## Method contracts

- `0x0600013A GlobalStaticVars.ChangeSprite`: this is a static caller with a GameObject lookup followed by SpriteRenderer lookup. The original native Object.op_Equality(result,null) true branch returns false. The damaged IL had the opposite named predicate. Use the actual equality reference, preserve the returned GameObject across the Unity comparison callback, resolve GetComponent<SpriteRenderer>, preserve that renderer across op_Implicit, then assign the input sprite and return true. Null/destroyed objects return false through Unity semantics; failures in called helpers remain observable. This does not reconstruct its independent lookup callee.
- `0x06000544 AlmanacUIController.CheckOtherAlmanac`: capture the original crossbarExpand, invert the field before any transform call, and choose both branches using the captured value. Old true uses image scale `(1,1,1)` and crossbar position `(-1435,-435,+0)`; old false uses `(-1,1,1)` and `(-715,-435,+0)`. Distinguish Component.get_transform on the image from GameObject.get_transform on crossbar. Both Vector3 parameters are by value, using the existing public constructor. Callback changes to the boolean must not alter the branch halfway through. The field inversion remains if an image/transform failure occurs.
- `0x0600021A ResourceManager.LoadAnimation`: `Path.Combine("prefabs","Charred")`, capture the current static zombie_Charreds list, combine with `"Zombie_charred"`, perform the existing actual closed InternalResourceLoader.Load<Zombie_charred>, then Add the returned object to the captured list using public List<Zombie_charred>.Add. Do not reread the list after the loader callback. A null captured list throws only after loading; a null loaded object is still added. Preserve platform Path.Combine behavior by retaining the real BCL call, not a hardcoded Windows separator. The original ten local definitions stay in order; one typed list scratch local is appended through a new scoped StandAloneSig. Private List<object> array/version mutations are removed only within this target body.
- `0x060006BC SeedChooserScreen.SetPrechooseList(List<Card_Choose>,int)`: invoke the existing one-argument overload before capturing this.card_ChooseList. Iterate that resulting field rather than the supplied argument list. For every current plantID equal to the second argument, reread this.plantDetail and call Awake(current); preserve duplicate and negative matching IDs. GetEnumerator lies outside try; iteration and Awake lie inside a real finally/Dispose structure. Reuse Native68's closed Card_Choose iterator bindings and enum/current local signature; do not mutate the shared signature row.

## Primary evidence and isolation boundary

Original GameAssembly SHA: `9ebd7ca996a5b03fb4a766f7a2502b660d581ddbbb36af4d7bf2f06a211da39d`. Original metadata SHA: `ad992341add498bd1018ac980f42171e0737b0793bf945c8714ad70a445b36b9`.

Native LoadAnimation literal usage cells are decoded independently from original metadata: `0x181BC68F0`, seed `A0003CCF`, literal 7783 `prefabs`; `0x181BB5FD8`, seed `A0000C1D`, literal 1550 `Charred`; `0x181BCD0F0`, seed `A00029EB`, literal 5365 `Zombie_charred`. Current parent IL uses the same exact strings. No decompiler diagnostic literal is inherited.

Native almanac constants: `0x1815A7B54` = -1f (`000080bf`), `0x1815A7EC4` = -435f (`0080d9c3`), `0x1815A7EC8` = -715f (`00c032c4`), `0x1815A7ECC` = -1435f (`0060b3c4`).

Only one public MemberRef and one local-signature row are appended. All original metadata rows/heaps/signatures are retained; only scoped body headers, code and one finally clause are intentionally reconstructed. The generated construction manifest is not an independent byte-isolation verdict. Gemini must still compare every non-target method and permitted metadata extension exactly.

## Validation remains pending

Gemini must run original native bytes against the actual candidate CIL and hash-bind all reports. Sprite cases must distinguish equality from inequality and renderer/source changes during callbacks. Almanac cases must include branch changes attempted during getters, receiver capture, both vector bit patterns and side effects before null/throw. Loader cases must distinguish a captured list from a reread list and verify Add after successful/failed/null loads. Seed cases need overload changes to the chosen list, list mutation/version exceptions, duplicate/negative IDs and Dispose on normal and exceptional paths. Each method needs a behavior mutant that actually executes and differs from the original; emitter/build failures cannot be counted as behavior discrimination.

Native60 v3 has now supplied a real 8192-case native mmap/CIL run, but the integrator requested per-mutant failure causes and a complete hash/exit-bound evidence archive before acceptance. Early counter and PASS claims remain superseded by the accepted ledger. Native61–64 have incomplete independent behavioral evidence; Native65 has a frozen ldftn coverage gap. Full conversion/translation-unit, licensed Unity, real Xcode and IPA remain pending. No workflow was launched by this implementation checkpoint.
