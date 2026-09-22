# Stage9.1 all-24 production promotion gate

Status: `BLOCKED_PENDING_INDEPENDENT_RESOLVE_AND_REAL_IL2CPP`

This file describes the exact promotion sequence from the currently formal B001-only migrator to the all-24 MemberRef work-copy repair. It does not authorize promotion by itself.

## Locked states

- Formal runtime artifact, immutable: `047054e0db594b6e3385fe4d2555c5932dcd4c28e4b2cb6fb39acbc4336d43ee`
- Run-8 pre-B001 work copy: `f47b1b37f1d4d3a1ddeb44bb6dbe399c58dc6dee3d4602618b2790f839d3e321`
- Current formal B001-only work copy: `f144dd624d2b0c05b5ff01fc12ff455ac394c87852a4fc3c87e8c3611582a433`
- All-24 static candidate: `b07254d5c4077e013bbc934734f7b0949c0942a4e667d6d2b23d2af6c830f720`
- MethodDef invariant: `2317`
- Expected changed byte positions from B001 to all-24: `48`
- Expected changed byte positions from run 8 to all-24: `50`

## Mandatory gates before production promotion

All gates below must pass on the exact `b072...f720` candidate.

1. Raw builder gate
   - Input hash is recognized.
   - B001 parser anchor resolves to file offset `0xC99DE`.
   - Canonical blobs are byte-exact.
   - Output hash is exactly `b072...f720`.
   - Diff count is exact.

2. Independent Mono.Cecil structural gate
   - `ModuleKind.Dll`.
   - `MethodDef=2317`.
   - All 24 locked MemberRef tokens are present as `MethodReference` after repair.
   - Expected IL-use counts match.
   - The three transform use sites are `callvirt` with their original MemberRef tokens.
   - Ownerless generic hits across method returns, parameters, locals and MemberRefs are zero.

3. Strict Resolve gate
   - Run with the exact Unity China `2022.3.44f1c1 / c3ae09b9f03c` Editor resolver inputs.
   - `Resolve()` succeeds for all 24 targets (`24/24`).
   - No dependency-missing result is accepted as a pass.

4. Real Unity/IL2CPP gate
   - Reconstruct the exact R3 project.
   - Preserve original 8192 atlas and software-GL setup.
   - Preserve 67/67 package reference closure.
   - Apply repair to project work copy only.
   - UnityLinker must pass.
   - IL2CPP DataModel must pass the run-8 `List<Buff>.GetEnumerator()` failure and the other normalized targets.
   - Capture any new fatal rather than assuming successful Xcode export.

## Production migrator state-machine changes after the gates pass

The current production migrator uses `RuntimeDllIosFinalSha256=f144...a433`. Promotion must not simply append more writes after that return condition.

Required state split:

```text
RuntimeDllIosB001Sha256 = f144dd624d2b0c05b5ff01fc12ff455ac394c87852a4fc3c87e8c3611582a433
RuntimeDllIosFinalSha256 = b07254d5c4077e013bbc934734f7b0949c0942a4e667d6d2b23d2af6c830f720
```

Required flow:

```text
formal runtime / DLL-kind / Damage / pre-Buff
        -> existing formally proven repairs
        -> run-8 pre-Buff f47b...
        -> B001 repair
        -> B001 f144...
        -> all-24 post-B001 repair
        -> final b072...
```

Idempotency requirements:

- `b072...` returns immediately as complete.
- `f144...` is accepted as a valid intermediate input and advances to all-24.
- Older recognized formal states continue through the already-proven repair chain, then advance through B001 and all-24.
- Any unknown SHA is rejected before a write.

Marker requirements:

- Existing `Library/GodsPVZ.package-reference-migration.done` files containing the B001 SHA must be upgraded by running the post-B001 repair and replacing `dll_sha256=` with `b072...`.
- New markers must write `dll_sha256=b072...` only after the exact final hash gate passes.
- A marker must never cause a B001 work copy to be treated as all-24 complete.

## Production implementation preference

`Recovery/CandidateSource/GodsPVZAll24WorkcopyRepair.cs` is a reviewable dynamic ECMA-335 implementation and a fallback integration draft. It is deliberately not compiled by Unity.

After the independent audit runs, prefer the smallest production implementation that preserves the same safety properties:

1. use the builder manifest to record the exact discovered metadata offsets for the locked B001 input;
2. keep the whole-file B001 SHA gate;
3. validate every old byte/index/token before writing;
4. apply only the exact proven changes;
5. require the final `b072...` SHA before `File.WriteAllBytes`;
6. preserve the dynamic parser helper/evidence as an independent cross-check, not as an excuse to weaken byte-level validation.

This keeps the production Unity Editor script small while retaining reproducibility and exact-input safety.

## Non-promotion rule

Until gates 1-4 are satisfied, `stage9-admin-start-static` remains B001-only. Candidate tooling may be improved on `stage9-local-audit-tools`, but the all-24 repair must not be merged into the production migrator and no new GitHub Actions workflow should be triggered solely to bypass the missing local execution backend.
