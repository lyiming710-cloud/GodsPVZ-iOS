# Stage9.1 GetEnumerator family candidate — local proof

Status: `GETENUMERATOR_FAMILY_RAW_REPRO_PASS`

This checkpoint was produced in the current Linux container without triggering GitHub Actions. It is a candidate-only proof and is **not** formal integration beyond B001.

Input run-8 work-copy SHA256:
`f47b1b37f1d4d3a1ddeb44bb6dbe399c58dc6dee3d4602618b2790f839d3e321`

Candidate SHA256:
`f01cbf6cc768b48388d8f37fe665914e8f5954902a1f0328357965d762c24bc5`

Nine `List<T>.GetEnumerator()` MemberRefs were changed to reuse the existing canonical definition-level return signature blob at #Blob `0x1C24`, while keeping each existing closed `List<Concrete>` Parent unchanged:

- `0x0A0000B4` at `0xC99DE`: `811A -> 241C` (Buff)
- `0x0A0000C9` at `0xC9A5C`: `661C -> 241C` (Device)
- `0x0A0000D0` at `0xC9A86`: `A91C -> 241C` (Plant)
- `0x0A0000D6` at `0xC9AAA`: `D71C -> 241C` (Zombie)
- `0x0A0000E4` at `0xC9AFE`: `DC1E -> 241C` (BoardEntry)
- `0x0A0000FB` at `0xC9B88`: `2B21 -> 241C` (UnityEngine.GameObject)
- `0x0A000141` at `0xC9D2C`: `AA2E -> 241C` (Enemy)
- `0x0A000162` at `0xC9DF2`: `4137 -> 241C` (Projectile)
- `0x0A00021D` at `0xCA254`: `6574 -> 241C` (EnemyPath)

The local raw reproduction matches the packaged candidate exactly: 9 MemberRefs, 15 changed bytes, output SHA `f01cbf...4bc5`.

Existing packaged Cecil audit for this exact candidate reports:
- unresolved MemberRefs: `24 -> 15`
- unresolved MemberRefs with IL uses: `23 -> 14`
- MethodDef count: `2317`
- orphan generic hits: `0`
- the nine targeted GetEnumerator MemberRefs resolve after the change

Boundary: no new exact Unity/IL2CPP export has been run, so these eight additional family changes are not yet merged into the production migrator. B001 Buff.GetEnumerator remains the only formally integrated new repair in commit `6b52f719f70232131ca3de0b4d431adcb3244f67`.
