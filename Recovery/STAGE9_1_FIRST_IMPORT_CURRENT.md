# Stage9.1 FIRST FORMAL UNITY IMPORT — current status

Date: 2026-09-12
Branch: `high-fidelity`

## Current state

- `UNITY_LICENSE_GATE = PASS`
- `FORMAL_STAGE9_1_INPUT = R3`
- `R3_FIRST_IMPORT_PASS = YES`
- `R3_CLASSIFICATION = R3_FIRST_IMPORT_PASS`
- `UNITY_EXIT_CODE = 0`
- HF56 authorized: **NO**

Stage9.1 has passed the first formal Unity import gate. The prior R2 import exposed a non-gameplay assembly-reference collision. The minimal importer-only correction was validated independently, frozen into R3, and R3 then passed a fresh formal import from its own persisted Drive parts.

Do not open HF56 merely because the import gate is complete. HF56 remains authorized only if a subsequent runtime/scene/build blocker is specifically attributable to missing native-backed gameplay reconstruction.

## Exact Editor gate — PASS

Formal Editor source: preserved GitHub Actions artifacts from run `34668863583`.

- Unity variant: China Linux Editor
- version: `2022.3.44f1c1`
- archive size: `3906640940`
- archive SHA256: `0008115c785784baddb19e2b13b384ac845438239f293efb0c2e8b1379a1fe14`
- 15/15 preserved raw parts: SHA256 PASS
- reconstructed Editor version gate: PASS

## Personal license gate — PASS

Proven CI activation path:

- `Unity.Licensing.Client --activate-all --include-personal`
- credentials are stored only in GitHub repository secrets
- exact Editor licensed probe: PASS
- Personal seat is returned at workflow exit

The Windows-Hub `Unity_lic.ulf` is machine-bound and is not used as the final Linux CI activation mechanism.

## R2 lineage and blocker

R2 remains preserved for provenance but is superseded as the formal Stage9.1 input.

- R2 archive SHA256: `99bc1ed7a713b627919fee8c3f63bbed5eb949ede72b32a7a5866a067d1b2a0e`
- R2 size: `786481679`
- R2 project files: `19782`
- HF55 DLL SHA256: `dc205a40dc2478b3aacbb3a7d6bb1ca96ffb0a964648d34062b4ddc75f3b3655`
- formal R2 workflow run: `34675644811`, successful import attempt job `103505188607`
- Unity launch/import command reached the project and exited normally, but compile diagnostics classified the input as `PROJECT_COMPILE_BLOCKER`
- blocker: SRP Core `RenderGraph.cs` resolved `DebugManager` against the HF55 precompiled game assembly and then failed on `GetPanel`

Root cause: `Assets/Plugins/Assembly-CSharp.dll.meta` had `isExplicitlyReferenced: 0`, so HF55 `Assembly-CSharp.dll` was automatically referenced by asmdef assemblies including SRP Core. HF55 contains a game-side global `DebugManager`, which shadowed `UnityEngine.Rendering.DebugManager` in that compile context.

This was not missing reconstructed gameplay and did not authorize HF56.

## Fix1 validation — PASS

Controlled disposable-copy change:

```text
Assets/Plugins/Assembly-CSharp.dll.meta
isExplicitlyReferenced: 0
->
isExplicitlyReferenced: 1
```

Nothing else was changed. HF55 DLL bytes remained exact.

Validation workflow:

- workflow: `.github/workflows/stage9-first-import-fix1-hf55-autoref.yml`
- commit: `d42882d6abfcfc87bea01fed31554073488ed1f6`
- run: `34676489013`
- job: `103507157972`
- result: SUCCESS / `FIX1_IMPORT_PASS`
- Unity exit: `0`
- compile errors: `0`
- package errors: `0`
- assembly collisions: `0`
- serialization errors: `0`
- missing-script errors: `0`
- license errors: `0`
- fatal errors: `0`
- HF55 DLL unchanged: YES
- GitHub artifact ID: `10292388034`
- artifact digest: `sha256:ba21e7e99a8deb107178813bb7cf01fcf8096e1962a46bbc0c67ab6f76d36516`

Drive preservation:

- folder ID: `1U56_5oPwqq22UM7I1NNtm1x33SYhYU9Q`
- evidence file ID: `1SuXlUlL-jehnsFdH1y86PxBB2AYp41B4`

## Formal Stage9.1 R3 input

R3 supersedes R2 as the only formal Stage9.1 pre-import input.

- archive: `GodsPVZ-Stage9.1-preimport-r3-2026-09-12.tar.zst`
- archive SHA256: `d4264f12a00e86b6149d20ecf04caa9761af510aece6107bcceb155ea4ab0bbc`
- archive size: `786765618`
- deterministic rebuild test: two independent builds produced identical SHA256 and size
- parts: `12`
- parts `00`–`10`: `67108864` bytes each
- part `11`: `48568114` bytes
- project file count: `19782`
- project manifest SHA256: `87ceec00ed89e4b7969fffbbeb1903435789966489ad9138b6d4cee74d176856`
- R2 -> R3 project delta: exactly one file changed: `Assets/Plugins/Assembly-CSharp.dll.meta`
- missing files: `0`
- extra files: `0`
- HF55 DLL SHA256: `dc205a40dc2478b3aacbb3a7d6bb1ca96ffb0a964648d34062b4ddc75f3b3655`
- project version: `2022.3.44f1c1`
- scenes: `Assets/Scenes/MainMenu.unity`, `Assets/Scenes/Board.unity`

Drive preservation:

- R3 folder ID: `1sGIgsBKpdntTNlORJkSjuWZgST3TacGa`
- R3 evidence ZIP ID: `1IshC5no-lOky0FXBMahDaE6iNa3sgxw7`
- part/hash manifests are stored alongside the 12 archive parts

## Formal R3 FIRST IMPORT — PASS

Workflow:

- workflow: `.github/workflows/stage9-r3-first-formal-import.yml`
- workflow commit: `3ed2a58a36ccbc9c6cbcf56b8bec6740594d87d4`
- run: `34677452067`
- job: `103509743561`
- result: SUCCESS
- classification: `R3_FIRST_IMPORT_PASS`
- Unity exit code: `0`
- Library created: YES

Formal pre-import audit:

- manifest entries: `19782`
- actual files: `19782`
- missing: `0`
- extra: `0`
- mismatch: `0`
- R3 archive SHA256 matched: YES
- R3 manifest SHA256 matched: YES
- HF55 DLL SHA256 matched: YES
- `isExplicitlyReferenced: 1` present exactly once: YES

Import diagnostics:

- compile errors: `0`
- package errors: `0`
- assembly collisions: `0`
- serialization errors: `0`
- missing-script errors: `0`
- license errors: `0`
- fatal errors: `0`

GitHub evidence:

- artifact name: `Stage9.1-R3-FIRST-FORMAL-UNITY-IMPORT-evidence`
- artifact ID: `10293065266`
- artifact digest: `sha256:0f574aea6e9692bf3c858b1e3b9bdde74ba2439943d841e73ea3f32c7cc14dde`
- retention: 90 days

Drive preservation:

- folder ID: `1dOXB9sHdnlgj6te_tMWaymEXlZaVh-6u`
- evidence file ID: `1VsPtvtwksYAU7t4fgOhb2TkNJVv0kgGP`

No raw Unity password, access token, refresh token, or license XML is preserved in these evidence artifacts.

## Next valid action

Stage9.1 import infrastructure is no longer blocked. Continue with runtime-oriented validation using R3 as the sole input:

1. inspect the successful R3 `Editor.log` for non-fatal warnings and import anomalies;
2. verify package resolution / generated `packages-lock.json` against the pinned embedded package set;
3. perform controlled scene-load validation for `MainMenu` and `Board`;
4. perform a minimal non-interactive runtime/play-mode smoke gate where feasible;
5. only if those gates expose a gameplay/runtime failure attributable to unrecovered native behavior should HF56 be considered.

R2 should be retained only as provenance. All new Stage9.1+ work must start from formal R3.
