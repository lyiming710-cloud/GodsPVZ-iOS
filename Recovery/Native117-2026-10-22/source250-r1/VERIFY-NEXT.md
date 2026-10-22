# Bulk250 R1 — AGY retest scope; Bulk300 remains held

The primary author has made source edits only. No patcher, candidate, CLR fixture, verifier, IL2CPP, compiler or workflow has been executed by the author. This is a repair proposal, not a passing result.

## Why this revision exists

AGY run [39662784115](https://github.com/lyiming710-cloud/GodsPVZ-iOS/actions/runs/39662784115) constructed the original Bulk250 candidate `ad25359a2a36b3b9632e89ad0752bb25657de0f917a23aed2b664dd479c384f8`, 16,573,440 bytes. Its recorded comparison to Bulk200 (`667e032fe7512f471bcc891c80fc81fd22d27e0ce0ae675bb226f7bbe0e00bc0`) reports 50 changed /2,267 identical bodies. Its isolation report records 250 targets /2,067 unchanged other methods. These reports are historical execution evidence, not semantic acceptance of this revision.

The recorded type-flow output contains five failures: LoadReward, Save.CompletenessTest, Grid.CanPlacing, Plant.PC_NutknightTaunt and Benchmark02.Start. AGY additionally reported a cross-type private-field access in Projectile.PC_LightSaber_Explosion. The author found the same particle-field access mechanism in Awake148 and CreateParticleAndSound while editing these sources. The eight proposed repairs are enumerated in SOURCE-REPAIR-CHANGELOG.json. Existing target scope stays 250 =244 direct historical entries +6 inherited companions.

## Authoritative construction and comparisons

1. Use `Bulk250.csproj` in this revision, constructing once from the fixed Native93 parent: 16,033,792 bytes, SHA256 `b074224112d46a959de074d3090ab6d0614b036959c66c63b4f49954472e2162`. Never use original Bulk250 as the construction parent. Build twice in new output directories; save exact source manifest, tool versions, invocations, exits, stdout/stderr and candidate bytes/hashes.
2. Compare to the actual original Bulk250 candidate above: expected **8 changed /2,309 identical method bodies**. Compare to the actual Bulk200 candidate above: expected **52 changed /2,265 identical bodies**; the extra two are inherited 060003F4 and 06000753. These are proposed expectations, not measured outcomes. Do not retain the old 50-body comparison expectation for this revision.
3. Independently compare to Native93: 250 allowed bodies, 2,067 unchanged others. Preserve original bytes/RVAs where required, MethodDef rows, original heaps and table rows; account explicitly for allowed append-only metadata, method-body relocation and EH. Check InitLocals, MaxStack, locals, code and exception regions rather than code bytes alone. Run the existing meaningful negative controls, including wrong parent, output overwrite, non-target/header/EH/table mutations.
4. No metadata changes are proposed in R1. Expected totals remain MemberRef1232, StandAloneSig1958, TypeSpec176, MethodSpec191; 85 finally clauses. Keep exact generic signatures, MethodSpec190/191 ownership and the historical TMP UV Vector2[] overrides. No field visibility, validator threshold or private-member access bypass is authorized.

## Specific regression requirements

- 06000163: both List<Supplies>.Add calls must resolve via existing 0A00027F; distinguish a Buff-Add mutant, preserve first-list then NewPlant conversion and two finally regions.
- 060004BF: List<Supplies>.Count/Item via 0A00023A/0A000233, existing typed setter unchanged; verify sort bounds, equal IDs, callback mutations and original exception order.
- 06000297: call Device.CanBear(OccupyState, **"device"**) with existing #US 7000AE84. The argument is independently linked to original native metadata cell181BB0258, literal index6739. Keep Plant.CanBear as its one-argument method. Cover device/plant slot choices and true/false paths; omit/wrong-string mutants must not be accepted.
- 06000384: Path_Finding returns bool; pop before loop merge. Preserve Native93/original native neg/cmov behavior at Int32.MinValue, not Math.Abs; preserve path insertion and side effects. Pop-removal and private-field mutants should fail the relevant gates.
- 06000753: only the TextMeshPro-created branch casts local TMP_Text to TypeRef0100005E before derived-only set_autoSizeTextContainer. Canvas/TextMeshProUGUI branch still uses the base local. 0A000306/307 are TMP_Text members and remain unchanged. Execute all spawn branches.
- 060003F4, 06000514, 0600035B, and 06000384: use existing public ParticlesManager.get_instance (060001E5). Saved native and CIL evidence show a static backing-field return; it does not synthesize or create an instance. Preserve two separate reads in 0600035B. Test null instance/particle, normal paths and callback replacement; never flip backing-field visibility.

Run all 250-target type/access gates, not just the eight repaired bodies. Seven older unsupported ldtoken cases must remain explicitly UNSUPPORTED until actually implemented and controlled; do not exclude them and report universal PASS. Use the candidate-bound CIL/CLR/native-oracle methods already available, meaningful negative controls, compiler and generated-unit gates as applicable. Real candidate execution and native behavior remain separate from stack flow and isolation. Do not label an all-NRE fixture, signal999, Linux longjmp skipping Windows SEH, or a zero exit code as semantic equivalence. Preserve 01D3→01E0→0677 UNPROVEN and unmodeled SEH states.

## Evidence delivery defect also requiring repair

The restored `bulk250-restore-receipt.json` claims candidate SHA `8ef0f2bc62658bd7a4bdeb2cb25a10cadaa9f5418e8f97910f7ac1b9604823b1`, while the same run's construction and comparison records identify `ad25359a...`. Preserve the erroneous receipt as a counterexample and issue a new, correctly bound receipt. Reconcile archive, manifest, source, candidate and run identities; re-used legacy file names such as `actual-candidate-180methods-dump` must not silently imply the wrong scope. Include the full member-access invocation/output in the new archive; a final-message claim alone is not a substitute for the saved gate output.

## Dispatch boundary

The next fifty sources are saved in Native117/Bulk300, unbuilt and untested. **Retest Bulk250 R1 only now.** Finish all applicable existing gates, preserve FAIL/UNSUPPORTED/UNPROVEN, and report source-bound results and genuine blockers. Do not stop after candidate construction. Do not restart Codespace, dispatch Unity/Xcode as a first-error probe, change game semantics, frozen validators or acceptance standards. Bulk300 execution waits for the primary author's review of preceding-batch acceptance. No new formal qualifications or IPA are claimed here.
