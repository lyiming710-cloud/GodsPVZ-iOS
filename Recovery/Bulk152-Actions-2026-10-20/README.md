# Bulk152: Codespaces checkpoint and Actions migration

Date: 2026-10-20 (Asia/Shanghai). Candidate remains unchanged:

- Bytes: 16,518,144.
- SHA-256: `8ef0f2bc62658bd7a4bdeb2cb25a10cadaa9f5418e8f97910f7ac1b9604823b1`.
- This branch adds preservation, execution infrastructure and review notes. It does not replace game method bodies or modify production/main.

## Durable recovery inputs

Diagnostic prerelease: https://github.com/lyiming710-cloud/GodsPVZ-iOS/releases/tag/bulk152-actions-checkpoint-2026-10-20

`PORTABLE-BULK152-CHECKPOINT.tar.gz` holds 4,173 regular files, 340,825,789 compressed bytes. SHA-256: `40603568bae03a8e99ef9530650e6927d400e8503cabfcc58fc86ec92cbb3947`.

`PORTABLE-MANIFEST.json` authenticates every archived file. Its SHA-256 is `9cafa4298205d4156f5c7f6e8a4c8896877e7bc2c6c94e6eedabc8688275e2e1`. GitHub's upload digest was read back and matched both files before stopping Codespaces. `REMOTE-INVENTORY.json` records four clean Git checkouts and their commit identities.

Portable layout: `work/` contains the entire AGY run directory, exact candidate, original fixture sources, logs, managed support inputs and generated C++; `toolchain/il2cpp/` contains the converter; `toolchain/native18-IL2CPP/` contains native headers; `tools/Mono.Cecil.dll`, `original/GameAssembly.dll`, `fixture-bin/` and `fixture-obj/` complete the replay inputs. Historical source/specification and verification ZIPs are additional release assets. No login files or conversation reasoning are archived.

The original Codespace `glowing-train-p7j9gp74q6jwc76v6` was stopped after the durable upload. Do not restart it for this pipeline. All current remote Git worktrees were clean; new AGY work was in the archived temporary directory.

## Evidence and unresolved limits

The original AGY report and files remain immutable. AGY reported 768 positive Native109 cases, five detected mutants and IL2CPP exit zero. Linux Clang still failed all 19 game TUs: 4,502 reported diagnostics, 235 attributed methods, plus 58 included-file diagnostics within that total. These are captured observations, not full-product acceptance. Historical profiles differ, so subtracting historical counts does not prove a causal repair delta.

Primary source inspection found an important scope limitation: the fixture maps `MouseManager.GetZombieUnderMouse` to a handwritten `UnderMouseZombie` stub and `PrepareUIController.get_Instance` to a handwritten getter. The native oracle also hooks address `0x18031ee10`. Thus the actual `01D3 -> 01E0 -> 0677` interaction chain was not executed as claimed. The fresh migration replay is an isolated-target check with stubbed dependencies. The full interaction gate remains **UNPROVEN**, even if that finite replay passes. Preserve the original report; do not silently relabel it as full integration evidence.

No new formal method promotion is made. The prior 112-method scoped ledger remains historical; a complete new-candidate regression still needs exact input binding. No Unity Editor activation, Unity project import, Apple SDK compilation, Xcode link, IPA export or device run has been qualified here. No qualified IPA exists.

## Actions continuation

Workflow: `.github/workflows/bulk152-actions-verification.yml`, on Ubuntu 24.04, .NET SDK 10.0.401 and Clang 18. It requires no Codespace, Unity credentials or local editor.

First replay: [Actions run 39416885984](https://github.com/lyiming710-cloud/GodsPVZ-iOS/actions/runs/39416885984), executed commit `bc77466c1055ccd1f5ff26b169440d48c5fe9f97`. The clean runner downloaded the release checkpoint and authenticated all 4,173 files. The archived finite Native109 tests replayed at 768/768 and five detected mutants, with zero unexpected oracle signals. IL2CPP generated 760 C++ files, including 19 game TUs; all 19 game TUs failed Clang with 4,502 diagnostics and 235 heuristically attributed methods. The job correctly concluded **failure**. Both the fresh generated-source preservation and evidence artifact upload succeeded. This confirms this scoped pipeline runs without Codespaces; it does not qualify the game or the stubbed interaction chain.

1. Download the frozen diagnostic assets using the repository token.
2. Check both outer SHA-256 values, reject unsafe/duplicate archive members, then check every extracted file against the manifest.
3. Rebuild and execute the actual archived oracle and CIL fixture in a new output directory, retaining the finite-test scope limitation.
4. Remap the original IL2CPP command into a fresh stage using the same input closure and options; preserve command, complete stdout/stderr and exit code.
5. Compile every generated game TU, retain complete diagnostics and compute attribution from this run. Failed compiler gates produce a nonzero job result.
6. Upload diagnostics on both success and failure; short-lived run artifacts retain three days. The release checkpoint is the durable input archive.

For subsequent method repair batches, author source in a new directory, preserve frozen candidates and validators, bind every report to the exact candidate, and reuse this workflow infrastructure. Repair the true-chain fixture before claiming full Native109 integration. Once local/static/native prerequisites are actually met, reuse the separately qualified Unity and macOS/Xcode routes for product gates. Do not launch expensive full exports solely because a report generator ran successfully.
