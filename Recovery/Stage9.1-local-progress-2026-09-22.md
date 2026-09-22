# Stage9.1 local repair progress — 2026-09-22

This progress checkpoint was produced in the current Linux container without triggering any GitHub Actions workflow.

## B001 Buff.GetEnumerator raw patch reproduction

Status: `LOCAL_RAW_PATCH_REPRO_PASS`

Locked source runtime:
- SHA256: `047054e0db594b6e3385fe4d2555c5932dcd4c28e4b2cb6fb39acbc4336d43ee`
- Source artifact was read-only and remained unchanged.

Reconstructed run-8 work copy:
- SHA256: `f47b1b37f1d4d3a1ddeb44bb6dbe399c58dc6dee3d4602618b2790f839d3e321`

Buff.GetEnumerator candidate:
- SHA256: `f144dd624d2b0c05b5ff01fc12ff455ac394c87852a4fc3c87e8c3611582a433`
- Size: `1201152` bytes
- Diff from run-8 work copy is exactly two bytes:
  - `0xC99DE`: `0x81 -> 0x24`
  - `0xC99DF`: `0x1A -> 0x1C`
- Total changed bytes relative to the locked source runtime: `11`.

Reproduced prerequisite stages and SHA256 values:
1. PE `IMAGE_FILE_DLL` repair -> `9295bcb90857502af5ab802839b5c560f9e4d956b99dfaeae4350bd5fa5754a3`
2. Damage.AddElement canonical Enumerator repair -> `3a9b38e7b4c941b11d5683f74a547540fd2e5c891318f1f2ca79dbb88ecdafa8`
3. Board.Start local2 `MVAR0 -> UnityEngine.Sprite` -> run-8 SHA `f47b1b37f1d4d3a1ddeb44bb6dbe399c58dc6dee3d4602618b2790f839d3e321`
4. MemberRef `0x0A0000B4` signature-index repair -> candidate SHA `f144dd624d2b0c05b5ff01fc12ff455ac394c87852a4fc3c87e8c3611582a433`

## Verification boundary

The raw byte-level proof has been independently reproduced. The packaged Cecil proof reports that `0x0A0000B4` changes from unresolved to resolved, MethodDef remains 2317, orphan generic count remains 0, and no new unresolved MemberRefs are introduced. The current container does not have the packaged Mono.Cecil/reference runtime inputs installed, so an independent Cecil rerun is still pending. A real Unity/IL2CPP export has not been performed in this checkpoint.

## Next step

Integrate this exact two-byte repair into `Assets/Editor/GodsPVZPackageReferenceMigrator.cs` with strict input-hash gating and idempotence, then continue MemberRef review without touching gameplay IL or the locked runtime artifact.
