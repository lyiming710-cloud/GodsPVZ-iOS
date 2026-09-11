# Stage9.1 Pre-Import R2 — Formal Status

Date: 2026-09-12
Branch: `high-fidelity`

## Authority

The original pre-import archive `GodsPVZ-Stage9.1-preimport-2026-09-11.tar.zst` (SHA-256 `a23dbd43ba42aa4d6ddcbbacfe326c2d1cf6348fc6e8717121ee4bdfe540ae4c`) was never successfully persisted and is formally `LOST_SUPERSEDED`. It is not an allowed import input.

The only allowed first-import input is now:

`GodsPVZ-Stage9.1-preimport-r2-2026-09-12.tar.zst`

- size: `786481679`
- SHA-256: `99bc1ed7a713b627919fee8c3f63bbed5eb949ede72b32a7a5866a067d1b2a0e`
- archive entries: `20709`
- Unity ProjectVersion: `2022.3.44f1c1`
- project files covered by the frozen project manifest: `19782`
- `zstd -t`: PASS

## Static gates

- game `m_Script`: `3367 / 3367` resolved
- unresolved game script refs: `0`
- zero-fileID game script refs: `0`
- active Meta GUIDs: `8147`
- duplicate active Meta GUIDs: `0`
- embedded packages: `15`
- missing non-module package dependencies: `0`
- unexpected version overrides: `0`
- active assembly provider collisions: `0`
- HF55 cumulative managed input SHA-256: `dc205a40dc2478b3aacbb3a7d6bb1ca96ffb0a964648d34062b4ddc75f3b3655`

## Drive persistence and provider readback

Drive folder: `Stage9.1-PreImport-R2-2026-09-12`

Folder ID: `1XE6CdcBq7P-__OV7ZfdD23FdYumYjIvG`

The archive is stored as twelve 64-MiB-class chunks. All 12 chunks were downloaded back through the Drive provider and individually re-hashed. A file reconstructed only from provider-readback chunks had:

- size: `786481679`
- SHA-256: `99bc1ed7a713b627919fee8c3f63bbed5eb949ede72b32a7a5866a067d1b2a0e`
- `zstd -t`: PASS
- entries: `20709`
- ProjectVersion: `2022.3.44f1c1`

Control files were also provider-read back byte-identically.

Drive verification file ID: `1CpPY_vJ1gGWetCU-nr4Y4oALqg4w5DcV` (`R2-DRIVE-READBACK-VERIFICATION.txt`).

## Current gate

First Unity import has NOT started.

The next mandatory gate is exact Editor identity and persistence. The formal import must use the Unity China Linux Editor corresponding to changeset `c3ae09b9f03c`, and the Editor itself must report exactly `2022.3.44f1c1` before the R2 project is opened.

Do not substitute global `2022.3.44f1`. Do not open HF56 unless subsequent real Unity compile/runtime evidence establishes a native-backed managed blocker.
