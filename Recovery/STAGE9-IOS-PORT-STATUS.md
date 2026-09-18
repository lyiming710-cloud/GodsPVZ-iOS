# Stage9.1 iOS port status

Authoritative branch: `stage9-admin-start-static`

This file tracks the iOS toolchain and export gates layered on top of the formal HF55 / Stage9.1 recovery baseline. It does not replace native-method recovery evidence.

## Locked recovery/build inputs

- Unity Editor: `2022.3.44f1c1`
- Unity changeset: `c3ae09b9f03c`
- Preserved exact Editor run: `34668863583`
- Preserved Editor SHA-256: `0008115c785784baddb19e2b13b384ac845438239f293efb0c2e8b1379a1fe14`
- R3 pre-import archive SHA-256: `d4264f12a00e86b6149d20ecf04caa9761af510aece6107bcceb155ea4ab0bbc`
- Current runtime-qualified Assembly-CSharp SHA-256: `047054e0db594b6e3385fe4d2555c5932dcd4c28e4b2cb6fb39acbc4336d43ee`
- Final DLL source run: `35236211080`, artifact `Stage9.1-BGMVOLUME-STATIC-QUALIFIED`
- Package GUID evidence run: `35238895838`, artifact `HF46-package-reference-evidence`, `67/67`
- Pinned direct-package bundle run: `35239547918`, artifact `Stage9.1-pinned-unity-packages`
- Package dependency closure run: `35239319822`, artifact `Stage9.1-package-dependency-closure`

## Exact iOS module compatibility gate — PASS

The initial validation run `35332956511` failed before module installation because the hosted runner held split Editor parts, a reconstructed multi-GB Editor archive, and the extracted Editor simultaneously.

Disk-peak fix commit:

- `449cef3df65a40c00100f5e1b4e468f614de5c9e` — `ci: reduce exact editor reconstruction disk peak`

The Editor is now SHA-verified as one streamed byte sequence and streamed directly into `tar`; source parts are removed as they are consumed. Unrelated hosted-runner SDKs are also reclaimed before reconstruction.

A second issue was an overly strict pre-probe assumption about the internal path layout of the iOS support module. The module archive SHA/xz/tar integrity remains a hard gate, but module usability is now decided by Unity itself.

Compatibility-probe fix commit:

- `85a156af1f2e2d89407f98a5d7fa0a650a19a9fd` — `ci: let Unity judge iOS module compatibility`

Authoritative PASS run:

- Run `35339261693`
- Exact Editor `2022.3.44f1c1`
- Exact changeset `c3ae09b9f03c`
- iOS module SHA-256 `2c2e259415ab0f940889b6b5bf55c56bc2710bbd823ddc05c5ab67ebc137a16e`
- Editor reconstruction: PASS
- iOS module download / SHA / xz / extraction: PASS
- Unity Personal seat: PASS
- `BuildPipeline.IsBuildTargetSupported(iOS)`: PASS
- `EditorUserBuildSettings.SwitchActiveBuildTarget(iOS)`: PASS
- active build target: iOS
- Validation evidence artifact: `Stage9.1-IOS-MODULE-INSTALL-VALIDATION`
- Preserved validated module artifact: `Stage9.1-unity-china-ios-module-c3ae09b9f03c`

Conclusion: the exact China `2022.3.44f1c1` Editor and the same-changeset Linux iOS support module are a validated usable pair for this Stage9.1 build chain.

## Current-baseline unsigned Xcode export gate

Workflow: `.github/workflows/stage9-current-baseline-ios-xcode-export.yml`

Preparation / trigger commit:

- `e41f1ad83dc95f27631128c480a18b8c5d61bd91` — `ci: run iOS Xcode export from validated module`

Authoritative run in progress:

- Run `35339764011`
- Job `105582747574`

Completed gates in that run as of this status update:

1. hosted-runner disk reclamation — PASS
2. exact R3 download and reconstruction — PASS
3. runtime-qualified DLL download / SHA lock — PASS
4. package GUID evidence download — PASS
5. pinned package bundle download — PASS
6. package dependency closure download — PASS
7. current Stage9.1 baseline integration — PASS
8. exact Unity Editor parts download — PASS
9. bounded-disk exact Editor reconstruction — PASS
10. validated iOS module download — PASS
11. validated iOS module overlay — PASS
12. Unity Personal seat acquisition — PASS
13. first full current-baseline Unity import / compile — PASS
14. explicit package script reference migration — PASS
15. unsigned iOS Xcode export / IL2CPP generation — IN PROGRESS

The export is locked to:

- scenes: `Assets/Scenes/MainMenu.unity`, `Assets/Scenes/Board.unity`
- scripting backend: IL2CPP
- architecture: ARM64
- device family: iPhone + iPad
- deployment target: iOS 12.0
- bundle identifier: `com.tipsGodsStudio.godsPVZ`

A successful export must subsequently pass an artifact audit requiring the Xcode project, Unity generated data, IL2CPP output, generated `Assembly-CSharp.cpp`, `global-metadata.dat`, bundle ID and deployment-target checks before the unsigned Xcode archive is published.

## macOS compile / unsigned IPA gate — prepared, not yet triggered

Workflow: `.github/workflows/stage9-current-baseline-macos-unsigned-ipa.yml`

Preparation commits:

- `0281dc253e0bc0c277f2c2aa092ff66b0ef47425` — `ci: prepare macOS unsigned IPA compile gate`
- `9fa40284002e3f5865cbf80d77ac74ba49ccb1a0` — `ci: make macOS unsigned IPA gate native-runner compatible`

This gate intentionally does not re-run Unity. It consumes only a successful `GodsPVZ-Stage9.1-unsigned-Xcode` artifact, discovers the actual Xcode scheme/target with `xcodebuild -list -json`, compiles Release `iphoneos` with signing disabled, audits the ARM64 `.app`, and packages it as `GodsPVZ-Stage9.1-unsigned.ipa` for later external signing.

It remains manual until the current Linux Xcode-export run produces a validated artifact. Do not point it at a failed or unaudited export run.
