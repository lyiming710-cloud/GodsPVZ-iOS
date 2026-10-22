# Bulk250 R2: two source repairs; retest previous batch, hold Bulk300

The primary author has read immutable CIL/native metadata and saved AGY results, and authored two source repairs only. No patcher, candidate, project test, verifier, native fixture, compiler or workflow was executed by the primary author. Source changes are not passing results.

## Result reviewed

AGY run [39703773021](https://github.com/lyiming710-cloud/GodsPVZ-iOS/actions/runs/39703773021) produced Bulk250 R1 candidate `dceb44b834889ec6ebcd36baaadbf0b2062fc41982f3d3b64144461a99338996`,16573440 bytes. Recorded deterministic construction, eight changed bodies versus original Bulk250,52 versus Bulk200,250-target isolation and type/reference checks progressed. MemberAccess still reports **pass=false,target_methods=250,rejected=3,unresolved=0**. Three rejected sites belong to two methods. The saved raw report, negative control, construction/isolation outputs and review are retained in repair-evidence. No formal behavior/IPA acceptance follows from these static/compiler results.

## Exact R2 source changes

- 0600001C Admin_system2_boardEdior.CreateNewEnemy, emitter N116_CreateEnemy: replace its two cross-type reads of private BoardConfig.map04000198 with existing public BoardConfig.Map06000158. Preserve the separate reads before GetMapX and GetMapY and all subsequent writes/callback order.
- 06000183 DeviceManager.DisplayBan_device, emitter N116_DisplayBannedDevice: replace the one cross-type private Device.animationSprites04000488 read with existing public Device.AnimationSprites06000303. Preserve the outer/inner enumerations, two finally handlers, alpha selection and renderer writes.

Both accessors are public, have exact return types, and have immutable CIL `ldarg.0; ldfld <same field>; ret`. Original PC native Map at18030EF70 loads[this+30]; AnimationSprites at180346070 loads[this+D8]; both return immediately. PUBLIC-GETTER-SOURCE-EVIDENCE.json records signatures, visibility, raw bytes and native disassembly. They allocate no replacement objects, cache nothing and add no defensive null branch. Private field flags stay unchanged. Existing own-class accesses to Device.animationSprites in other target methods are not cross-class violations and were left alone.

No new target, metadata row/name/blob/user string is added by R2. Total remains250 target bodies,244 direct historical entries+6 inherited companions. Keep all eight R1 source corrections. Do not reconstruct from an older inherited source tree and lose repairs.

## Construction and body isolation

Build source250-r2/Bulk250.csproj twice in fresh output directories **from Native93**,16033792 bytes,SHA256 `b074224112d46a959de074d3090ab6d0614b036959c66c63b4f49954472e2162`. A previous candidate is comparison evidence, never the patcher input.

Expected comparisons, not executed by the author:

| Comparison | Changed bodies | Identical bodies |
|---|---:|---:|
| Actual Bulk250 R1 dceb44b8... |2 (0600001C,06000183)|2315|
| Original Bulk250 ad25359a2a36b3b9632e89ad0752bb25657de0f917a23aed2b664dd479c384f8 |10 (8 R1+2 R2)|2307|
| Actual Bulk200 667e032fe7512f471bcc891c80fc81fd22d27e0ce0ae675bb226f7bbe0e00bc0 |52|2265|
| Construction parent Native93 |250 allowed targets|2067 other methods|

Check full method header/code/local/InitLocals/EH content, preserved non-target RVAs and tables/heaps, explicit append-only metadata and relocation. Run the existing meaningful wrong-parent/overwrite/body/header/table/EH negative controls. Totals remain1232 MemberRef,1958 StandAloneSig,176 TypeSpec,191 MethodSpec and85 finally clauses. All two revisions reuse existing references; no field/method visibility edits and no blanket access bypass.

## Required execution work

1. Authenticate delivery/source manifest, fixed parent and genuine comparison candidates; build deterministically and archive exact commands, exits, stdout/stderr and candidate hashes/bytes in a new revision directory. Do not overwrite old evidence or repurpose old candidate identity receipts.
2. Execute all250 target type/access/reference/metadata/isolation gates. The new member-access baseline should pass if no other defects remain. A private-field mutant must fail at the added mutant sites with specific diagnosis; a generic false exit shared by a failing baseline is not an adequate negative control. Keep the seven historical unsupported opcode/library cases explicit; existing ldtoken/ldftn checker copies are separate controlled extensions, not frozen-verifier rewrites.
3. Add candidate-bound normal/exception tests for the two accessor replacements, actual receiver identity/read count, null BoardConfig/Device/list/element, changing map across GetMapX/GetMapY callbacks, enumeration order and disposal on faults. Read and execute actual accessor CIL as well as caller CIL when using fixtures; do not rely solely on a stub that returns the expected field by construction. Retain preceding50 coverage requirements and targeted mutants from the R1 handoff. Report per-method behavior coverage and open gates, not merely checker PASS counts.
4. Run appropriate existing IL2CPP/Clang prechecks, with any blocked-dependency bypass explicitly disclosed and never treated as gate acceptance. Capture the exact candidate preparation/linking lineage, conversion and compiled-unit scope.
5. Correct two report conclusions: v18 source registry is294 direct newly authored method sources and87 still unwritten;137 pertains to the tested250-source scope (244 direct). Equality between137 compiler-failing methods and137 unimplemented-in-that-candidate methods does **not** prove they are the same methods. Produce an explicit failing-method-token set,250-target-token intersection/difference, unbound/ambiguous diagnostics and included-file errors. Explain why clean2130+failed137=2267 differs from2317 managed MethodDefs; identify non-emitted/stripped/generic/extern methods or unsupported mapping honestly. Do not assert250 targets clean from count equality.

Keep 01D3→01E0→0677 UNPROVEN and unmodeled Windows SEH UNSUPPORTED. Candidate execution, semantic equivalence, code-generation progress, Linux18-TU compilation, Unity/Xcode and IPA are separate gates. All-NRE cases, oracle signal999, skipped Windows unwind or a successful conversion cannot substitute for behavior evidence. No newly qualified methods or IPA are claimed by this source delivery.

## Dispatch order

AGY executes this corrected **Bulk250 R2 only**, through GitHub workflow; Codespace stays stopped. Continue all applicable available gates rather than stopping after construction. Report real source counterexamples to the primary author. Preserve frozen validators/input hashes and original reports; do not modify game logic or lower thresholds to claim PASS. The next50 Native117/Bulk300 sources remain saved and held pending the primary author's review of preceding-batch acceptance. No Bulk300 build/test is authorized by this delivery.
