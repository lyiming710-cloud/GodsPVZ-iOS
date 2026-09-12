# Stage9.1 bounded generic MemberRef repair evidence

This is development-only Stage9 integration evidence. Formal cumulative recovery remains HF55. Nothing in this document promotes the output to a formal HF stage.

## Input

Exact Stage9 candidate:

`c8d0fdf2c6c03188ebe8a123121c6ef8972d9541ed8f630e95ee4e17b268b425`

The CI workflow reconstructs this input from formal HF55 through the already-fixed Stage9 candidate chain and asserts every intermediate SHA before running the repair.

## Repair scope

The tool only recognizes malformed concrete substitutions on constructed BCL generic owners for these standard method shapes:

- `List<T>.Add(T)`
- `List<T>.get_Item(int)`
- `List<T>.set_Item(int,T)`
- `List<T>.Contains(T)`
- `List<T>.IndexOf(T)`
- `List<T>.Remove(T)`
- `Dictionary<TKey,TValue>.Add(TKey,TValue)`
- `Dictionary<TKey,TValue>.get_Item(TKey)`
- `Dictionary<TKey,TValue>.set_Item(TKey,TValue)`
- `Dictionary<TKey,TValue>.ContainsKey(TKey)`
- `Dictionary<TKey,TValue>.Remove(TKey)`
- `Dictionary<TKey,TValue>.TryGetValue(TKey,TValue&)`

Known-good in-assembly VAR anchors are mandatory:

- List anchor: MemberRef `0x0A00006B`, `List<Zombie_Select>.Add(!0)`
- Dictionary anchor: MemberRef `0x0A00015A`, `Dictionary<ParticleState,GameObject>.Add(!0,!1)`

The repair reuses only the proven `VAR 0` / `VAR 1` forms from those anchors and preserves the original constructed declaring type, calling convention, non-generic parameters and non-generic return types. Unsupported owners/method shapes abort.

## Locked pre-repair audit

The exact input must contain exactly:

- suspicious callsites: `199`
- distinct malformed MemberRefs: `45`
- caller methods touched: `54`
- total MethodDefs: `2317`

Any drift aborts before write.

## Successful CI run

Workflow:

`.github/workflows/stage9-c8d0-generic-memberref-repair.yml`

Run:

`34716405258`

Job:

`103614332694`

Artifact:

`10304945416` — `Stage9.1-c8d0-GENERIC-MEMBERREF-REPAIR`

Artifact ZIP digest reported by GitHub Actions:

`sha256:20de03ea58d407585a5809c25d525c03672b9060f0d9d8fed1f923ec020573d9`

## Output

Stage9 diagnostic candidate SHA-256:

`dc2dccac079f4db46934283c380817d7b60e2a82ae71e8e18ba0b9f199bb6931`

The downloaded candidate was independently re-hashed after artifact extraction and matched the workflow output SHA.

## Reopen gates

The successful run reported:

- `AUDIT_BEFORE suspicious_calls=199 distinct_bad_refs=45 changed_methods=54`
- `PATCH_CALLS replaced=199 distinct_old_refs=45`
- `REOPEN_GENERIC_AUDIT_PASS suspicious_calls=0`
- `SEMANTIC_ISOLATION_PASS untouched_methods=2263 changed_methods=54`

Therefore all 45 audited malformed generic MemberRefs were eliminated after Cecil reopen, and every MethodDef outside the 54 caller-method scope retained the same semantic snapshot.

This does not prove runtime correctness of all 54 repaired methods. The output must still pass Unity runtime diagnostics. In particular, the next MainMenu test must verify that the former `ResourceManager.Start` / `LoadAudioClips` generic failures no longer prevent `SavesManager.Start -> LoadPlayerSaves` from running.
