# Recovery status

## Current strategy

Use PC x86-64 IL2CPP as the primary gameplay source, Android native second, then original metadata/assets/JSON; use Cpp2IL/ILSpy for attribution and managed reconstruction support. Do not substitute rescue-route approximations for native-backed gameplay. Do not begin IPA packaging until managed recovery and Unity/IL2CPP validation gates close.

## Fixed original baseline

- Unity `2022.3.44f1c1`; IL2CPP metadata `31.1`.
- PC CodeRegistration / MetadataRegistration: `0x1815E88C0` / `0x1818C6D00`.
- Android CodeRegistration / MetadataRegistration: `0x2772B68` / `0x285F870`.
- PC ZIP SHA-256 `2f08b4e2243e2bd296f3db694b4a2c9656252d76202c80167bdf3340b9f65b48`.
- PC GameAssembly SHA-256 `9ebd7ca996a5b03fb4a766f7a2502b660d581ddbbb36af4d7bf2f06a211da39d`.
- PC metadata SHA-256 `ad992341add498bd1018ac980f42171e0737b0793bf945c8714ad70a445b36b9`.
- Android APK SHA-256 `428e0ba2e46645a905fb9fbb00cfde42406727a3ddfce9a7df1e0875889a739f`.
- authoritative Cpp2IL source commit `5fb20304df698ffd3d0e664b2a698cd911dc9d57`.
- reproduced PC Cpp2IL baseline `2318 / 2319`; sole full failure `Zombie::InjuryStatusUpdate_Body`, closed in HF3.
- Native attribution rule remains original RID-1 -> Assembly-CSharp CodeGenModule methodPointers index; never use Cpp2IL-rewritten RID.

## Final cumulative HF chain

- HF1 `31e6f6a781c6350a99a7317cade6cebc11cacd562a4c129961eef2d53af1d471`.
- HF2 audited `a21c9e1d1e6994eb559694a51123d33b8a1eb3b262bda6d372f97bbf51523633`; old `d445395f...` superseded.
- HF3 `Zombie.InjuryStatusUpdate_Body` — Exact — `23014656af797490bc35d9feb8f78950bcfa79a167c6a1de3303e1d30251c32f`.
- HF4 `Zombie.Awake` — Exact — `2abbc9eb02b93bcd0178091bbc38ca450b6162875df9dcb55e874d5b9af9f0a6`.
- HF5 `Zombie.Start` + `LoopAddAnimation` — Exact — `58fe2001b6d9df986a08a930e60ea936fbf9392d82532f2e815bda63557d5e3a`.
- HF6 `Zombie.Update` — Exact — `e6395652d0d413fbb496dc9cae8f2712d6a65bcfad109d160818983cde234e75`.
- HF7 `Zombie.GetMoveDirection` — Exact — `ed01e0aa7dac968e8203131e6274a4da718ebe5fdcd0dcb3d3bc41d3b0f60ea4`.
- HF8 `Zombie.SetrSpeed` — Exact — `4dc9b2486ae41b2d2af3b1b5907869cc581271645ccc2a9e81595dfac07ec2`.
- HF9 `Zombie.Update_Attack` — Exact — `c3e1716bbd598a8b2d2af3b1b5907869cc581271645ccc2a9e81595dfac07ec2`.
- HF10 `Zombie.Update_Characteristic` — Exact — `0684c52733afd5dd1d45e455ede8ec5355d8b53bb65875506cb97f9a29f07b8a`.
- HF11 `BuffManager.Update<T>` — Exact managed-observable — `c990a8be0a615cbd1aed15b08251ae892889c056badc3e4070d7c6c24a6cb07f`.
- HF12 `Buff.Update<T>` — Exact managed-observable — `963f24a8323025e424c2e392d4f90ab1d7d48a8f5aa0b671775ac64402788930`.
- HF13 `Buff.Start<T>` + `Buff.End<T>` — Exact managed-observable — `a389fcf0f6a97b4ace5cc580fb9704fa9830dbeba9b50edc4c5cad90489ebac3`.
- HF14 `StatsIncreased.Start_stats<T>` + `End_stats<T>` — Exact managed-observable — `cbe30c99a973f41bbf4bb0f0f56f62d37e1c752c0723159cc07feeb3d2a0efe7`.
- HF15 `Bleed.Bleeding<T>` — Exact managed-observable — `87c74aea233372a5a7af28c0df244a18873af111c0230b54d9acf82e68ef9ec1`.
- HF16 `AttackRange.TestInRange<T>` — Exact managed-observable — `57100e6b9296f5a6c1ff4710cc2859810dcfff9992a27ea5238000729c3888c3`.
- HF17 `Buff.Awake(bool)` + Hide lifecycle — Exact managed-observable — `0eb0eba10cb27c5cff61e1a75947f146f5213ec036ff2ca3f95cf7d406ff1a67`.
- **HF18 Buff infrastructure cluster — Exact managed-observable — `0e1b1acffa3329b3f98d2b61fb346c3f6647bee84e38bfab1d067a7aa17d58ed`.**

Note: HF8 canonical SHA remains the previously recorded `4dc9b2486ae41b2d9c340293dada22e74ee6877290e0e79f456b770b92bb4688`; do not replace it with the HF9 hash above when copying the chain.

HF3 through HF18 are cumulative. Whole-assembly semantic isolation has passed each stage. HF17->HF18 changes exactly seven declared MethodDefs and no eighth method.

## HF18 technical result

Targets:

- `Buff.ComputingIncrement(float,string)` token `0x060000F3`, PC `0x180310980`.
- `Buff::.ctor()` token `0x060000F5`, PC `0x180310BB0`.
- `BuffManager.GetIncrement(float,string)` token `0x06000104`, PC `0x180310420`.
- `BuffManager.EndAll<T>(T)` token `0x06000105`, generic definition pointer 0; active Zombie specialization `0x180429310`.
- `BuffManager.FindBuff(string)` token `0x06000106`, PC `0x180310250`.
- `BuffManager.FindStatsIncreased(string)` token `0x06000107`, PC `0x1803103A0`.
- `BuffManager::.ctor()` token `0x06000108`, PC `0x180310630`.

Why HF18: fresh native call-centrality rescan found `BuffManager.GetIncrement` at 25 direct incoming E8 sites across Plant/Zombie combat/stat paths, `FindBuff` 13, `FindStatsIncreased` 7; downstream `Buff.ComputingIncrement`, EndAll and constructors still contained damaged helper-derived CIL. Shared empty-stub groups were suppressed before ranking.

Restored behavior: manager-wide increment accumulation; direct-child StatsIncreased accumulation; lookup and typed lookup; reverse-order generic EndAll with native two-list-read behavior; valid constructor initialization for all Buff manager lists and Buff base fields. `StatsIncreased.ComputingIncrementS` was independently audited as already native-consistent and was not patched.

Validation:

- HF18 patcher CI run `34222051988` success; artifact SHA-256 `634f9db3249f5fb9de8782079a6bc6d64f394585177fb5974e5f01bfac35cbf8`.
- formal HF17 re-fetched from Drive and re-hashed before patching.
- formal patch + repeat byte-identical.
- reopen: `ComputingIncrement 64 IL/223/1 finally`; `Buff::.ctor 15/51`; `GetIncrement 36/117/1 finally`; `EndAll 39/120`; `FindBuff 37/126/1 finally`; `FindStatsIncreased 5/13`; `BuffManager::.ctor 12/40`.
- permanent RecoveryAudit commit `18bfd6740faf86307c52edaea74e1445512bcee6`; workflow `34222352921` success; OPEN1/OPEN2 end `RECOVERY_AUDIT_OK`.
- ILSpyCmd `11.0.0.9375` with full fresh fixed-Cpp2IL refs: Buff, BuffManager and whole-assembly stderr all 0; whole IL 319,514 lines.
- normalized HF17->HF18 semantic diff SHA-256 `40466bcb749014bc2415e4a5a4345b5c551c106af8ceff0fe067a42ffb9459ef`; exactly seven MethodDefs changed.

HF18 Drive archive is complete and independently listed:

- directory `PVZ GOD/HighFidelity-Recovery-2026-09-08/HF18-Buff-Infrastructure`.
- **formal folder ID `1hdSQPuV42nk6wx3knep8y8mqM3z8dVtV`**.
- final DLL ID `1gXxdtyNf8w3vtOGWzu2vlWmsmN_ZH9G4`.
- patcher ID `1ZwQcIhHdO9w_Pv6-UyccqU_J3bUPezxJ`.
- SHA256SUMS ID `1NSohvFWm0Y5D2nIc5ZVZ7DGDtLEc42E3`.
- post-upload listing verified **29 final files**.
- a later-created empty duplicate HF18 folder is not the formal archive and must not be used for provenance.

## Earlier archive anchors

- HF14 folder `1-WOW4iCvmUAX9k6DAwxoXpc3frcCxCef` — 13 files.
- HF15 folder `1y7WGFVI7q1RGZjedzqj2hIWpT-58GhbV` — 15 files.
- HF16 folder `1sFxLFILZj2hLBn4VQ22NTxS6K52nCzPM` — 13 files; final DLL `1AVIqB82No7M3U_Y2eKLwOBbBZYDToRIQ`.
- HF17 folder `1gMPlCp0xtHXYK2hZaxTp2sb_CIX95HAE` — 16 files; final DLL `1RVb5I23-C0LJ3EXCXCnP4KBn9Vuon6vS`.

## Unity reconstruction state

- AssetRipper about 3,149 objects; reconstructed project about 6,783 files.
- 173 serialized game script types migrated to recovered Assembly-CSharp local IDs; PC replacement 173/173.
- 263 game-script references across 114 assets migrated.
- scenes restored: `Assets/Scenes/MainMenu.unity`, `Assets/Scenes/Board.unity`.
- fixed packages: UGUI 1.0.0, TMP 3.0.6, Core/URP 14.0.11, 2D Animation 9.1.1, Tilemap Extras 3.1.2, Burst 1.8.17, Collections 1.2.4, Mathematics 1.2.6, Visual Scripting 1.9.4.
- 67 serialized package script types still require 67/67 Unity validation.

## Next recovery gate

Do **one more call-centrality / active-path rescan from HF18**. Focus on path helpers, Device, Element, Projectile, Skill, Plant specialized combat and Board-critical methods, while suppressing trivial/shared stubs.

Decision:

1. If a high-centrality active-path method remains materially damaged or helper-dependent, recover only that coherent cluster as HF19+ using the same native-evidence -> formal-input -> CI -> Cecil -> ILSpy -> semantic-isolation -> Drive gates.
2. If no critical damaged method remains, **stop adding HF stages** and move directly to Unity `2022.3.44f1c1` import / package restoration / 67-of-67 script validation / recovered Assembly-CSharp IL2CPP conversion.

After managed recovery closes: minimal iOS adaptation only, Xcode export, unsigned build/IPA packaging, then true-device validation through a complete level.

High-fidelity-first remains mandatory: no stage is final until native evidence, CI, formal-input patch, reopen, permanent Cecil, full-ref ILSpy, whole-assembly semantic isolation, GitHub Evidence/STATUS, Drive archive and fixed SHA all pass.