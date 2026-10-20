# Native110 / Bulk162 R1: Actions continuation

2026-10-20. This checkpoint adds ten native-backed method-body sources to the previous 152-body aggregate. All sources, native disassemblies, current CIL identities, specifications and metadata additions are retained here. The earlier unbuilt draft is preserved locally; R1 corrects the `ZombieInfo` local signature before any candidate construction. No candidate or project test was executed by the primary author.

## Fixed inputs

- Construction parent: Native93, 16,033,792 bytes, SHA-256 `b074224112d46a959de074d3090ab6d0614b036959c66c63b4f49954472e2162`.
- Regression baseline: Bulk152, 16,518,144 bytes, SHA-256 `8ef0f2bc62658bd7a4bdeb2cb25a10cadaa9f5418e8f97910f7ac1b9604823b1`. It is deliberately also the wrong-parent negative control; it must never be accepted as the construction parent.
- R1 source manifest: `63f1e651fce1576110482efd3d70bca104e938683bd3d20f150022ae3aa8e793`.
- [Source and offline tool capsule](https://github.com/lyiming710-cloud/GodsPVZ-iOS/releases/tag/native110-bulk162-r1-source-2026-10-20): ZIP 5,260,353 bytes, SHA-256 `6091ec35681dabf63db93abed5f4de1de242e54f10044a6aa07704d4c687fc4d`.
- [Portable converter and genuine support checkpoint](https://github.com/lyiming710-cloud/GodsPVZ-iOS/releases/tag/bulk152-actions-checkpoint-2026-10-20): unchanged from the first Actions replay.

## New source scope

| Token | Method |
|---|---|
| `0x0600049A` | `Zombie.ZC_LadderSaboteursBoom` |
| `0x0600049D` | `Zombie.ZC_NutBoom` |
| `0x060005D2` | `BoardPreview.CreateDevicePreview_Device` |
| `0x060005D4` | `BoardPreview.CreateEnemyPreview_Zombie` |
| `0x06000711` | `Zombie_Select.Initialize` |
| `0x060007AC` | `TMPro_InstructionOverlay.Awake` |
| `0x060007B2` | `TMP_FrameRateCounter.Awake` |
| `0x060007AF` | `TMP_ExampleScript_01.Awake` |
| `0x060007B5` | `TMP_FrameRateCounter.Set_FrameCounter_Position` |
| `0x060000B9` | `PlantAnimationEvent.PC_SW_CreateCircle` |

The aggregate has 162 method bodies: 156 direct historical error targets and six previously explicit companions. The historical source-authoring worklist has 225 direct methods without new source. These are source inventory counts; they are not measured compiler improvements or semantic acceptance counts. No new formal method is promoted by this checkpoint.

## Required gates and evidence boundary

The AGY Flash verifier authors the execution runner; GitHub Actions performs all project execution. Codespaces remains stopped. The primary author reviews evidence and writes source without running patchers, fixtures or project compilation locally.

The new runner must authenticate the capsule, build the patcher, construct twice from Native93, reject an existing output and a wrong parent with exit code 3, independently validate all 2,155 non-target methods, preserve the original 152 restored bodies, and find exactly the ten expected body differences against Bulk152. It must preserve the existing 30 finally clauses and run the full isolation auditor with its 14 negative controls. Genuine iOS metadata resolution, member access, typed CIL flow and eager body parsing precede downstream conversion. Existing unsupported checker instructions remain explicit; they do not become a universal PASS.

The portable replay executes the archived finite Native109 fixture and its native oracle, then reruns the full IL2CPP converter and every game translation unit with the same support profile as [baseline run 39416885984](https://github.com/lyiming710-cloud/GodsPVZ-iOS/actions/runs/39416885984). This enables a comparable compiler delta. That fixture stubs dependencies, so the actual `01D3 -> 01E0 -> 0677` interaction chain remains **UNPROVEN**. It is not a ten-method behavioral acceptance suite. Every new method still requires its own native-backed behavior checks, meaningful mutants and bound regression evidence before formal promotion.

Failed gates must fail the job while retaining candidate bytes, exact command lines, full logs, isolation/typing reports and generated C++. Linux Clang success, if achieved, does not establish licensed Unity import/export, Apple SDK compilation, Xcode linking, IPA packaging or device behavior. No qualified IPA is claimed.

Workflow: `.github/workflows/bulk162-candidate-verification.yml`. New work is isolated on `repair/native110-bulk162-actions-2026-10-20`; production and main are unchanged.
