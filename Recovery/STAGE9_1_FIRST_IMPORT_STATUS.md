# Stage9.1 FIRST FORMAL UNITY IMPORT status

Date: 2026-09-12
Branch: `high-fidelity`

## Status

`FIRST_IMPORT_PASS = NO`

Classification: `INFRASTRUCTURE_BLOCKER_UNITY_LICENSE`

The first formal Unity launch reached the exact locked Unity China Editor, but stopped at licensing before any project import/compile began.

This is **not** a project/package/HF55/gameplay failure and does **not** authorize HF56.

## Formal inputs reverified

- R2 archive size: `786481679`
- R2 SHA256: `99bc1ed7a713b627919fee8c3f63bbed5eb949ede72b32a7a5866a067d1b2a0e`
- R2 `zstd -t`: PASS
- R2 project manifest: `19782 / 19782`
- missing: `0`
- extra: `0`
- size mismatch: `0`
- SHA mismatch: `0`
- HF55 SHA256: `dc205a40dc2478b3aacbb3a7d6bb1ca96ffb0a964648d34062b4ddc75f3b3655`
- ProjectVersion: `2022.3.44f1c1`
- scenes: `Assets/Scenes/MainMenu.unity`, `Assets/Scenes/Board.unity`

## Exact Editor reverified

- archive size: `3906640940`
- archive SHA256: `0008115c785784baddb19e2b13b384ac845438239f293efb0c2e8b1379a1fe14`
- all 15 raw part SHA256 values matched the locked list
- `xz -t`: PASS
- full extraction: PASS
- `Editor/Unity -version`: `2022.3.44f1c1`
- version command exit: `0`

## First import command

```bash
/mnt/data/unity-china-editor/Editor/Unity \
  -batchmode \
  -nographics \
  -quit \
  -projectPath /mnt/data/godspvz-stage91/first-import-work/ExportedProject \
  -logFile /mnt/data/godspvz-stage91/first-import-evidence/FirstImport-Editor.log
```

Formal evidentiary run:

- start UTC: `2026-09-12T03:46:14Z`
- end UTC: `2026-09-12T03:46:14Z`
- exit code: `1`

Relevant Editor log evidence:

```text
[Licensing::Module] Error: Access token is unavailable; failed to update
[Licensing::Client] Error: Code 500 while processing request (status: Unable to update licenses. Errors: No ULF license found.,Token not found in cache)
[Licensing::Module] Error: 'com.unity.editor.headless' was not found.
Pro License: NO
No valid Unity Editor license found. Please activate your license.
```

No C# compiler, package-resolution, serialization, shader, runtime, or gameplay diagnostics were reached.

## `-force-free` infrastructure check

A second controlled launch added Unity's documented `-force-free` option without changing any project content:

- start UTC: `2026-09-12T03:50:21Z`
- end UTC: `2026-09-12T03:50:21Z`
- exit code: `1`
- result: same license failure (`No ULF license found`, token unavailable, no matching free entitlement, `No valid Unity Editor license found`)
- formal project import started: NO

Therefore `-force-free` does not remove the activation requirement in this runtime.

## Post-attempt differential audit

The disposable work copy was compared again against the formal R2 `after-project-manifest.json` after both license-only launches:

- actual files: `19782`
- missing: `0`
- extra: `0`
- size mismatch: `0`
- SHA mismatch: `0`
- `Library/`: absent
- `Packages/packages-lock.json`: absent
- HF55 SHA unchanged: YES

Therefore neither license attempt mutated the recovered project.

## Evidence preservation

Full local evidence includes the complete Editor logs, exact command, timestamps, exit codes, stdout/stderr, pre-import verification, post-attempt differential audit, and the `-force-free` result.

Final Drive evidence archive:

`/Google Drive/PVZ GOD/HighFidelity-Recovery-2026-09-08/Stage9.1-PreImport-R2-2026-09-12/FirstImport-LicenseBlocker-2026-09-12/Stage9.1-FirstImport-LicenseBlocker-Evidence-2026-09-12-FINAL.zip`

- size: `7514` bytes
- SHA256: `e41d8ba92571931b9b1567a36911d147ea21dd346ef671005ccbbd79dfe88a60`
- ZIP integrity: PASS before upload

An earlier 4698-byte evidence archive from before the `-force-free` check is also preserved in the same Drive folder; the file with `-FINAL.zip` is the authoritative evidence archive for this status.

## Repository/runtime license discovery

No existing repository reference to `UNITY_LICENSE` or a Unity activation workflow was found during the blocker check. No Unity license/token file or relevant Unity license environment variable was present in the execution runtime.

## Required next action

Resolve Unity licensing as an infrastructure task, then rerun the **same exact first-import command against the unchanged disposable work copy**.

Until a licensed run reaches actual project import:

- do not modify HF55;
- do not open HF56;
- do not change packages;
- do not change serialized assets;
- do not classify this as a project import failure;
- do not start iOS adaptation/build.
