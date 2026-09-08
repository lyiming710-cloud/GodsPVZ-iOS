# Recovery status

## Current strategy

Use the PC x86-64 IL2CPP build as the primary gameplay-logic source, Android native as the second reference, then original metadata/assets/JSON, with Cpp2IL/ILSpy used for attribution and managed reconstruction support. Do not replace native-backed gameplay with rescue-route approximations. Do not begin IPA packaging until the native-backed runtime recovery gate is sufficiently closed.

## Fixed original baseline

- Unity editor: `2022.3.44f1c1`.
- IL2CPP metadata: `31.1`.
- PC CodeRegistration / MetadataRegistration: `0x1815E88C0` / `0x1818C6D00`.
- Android CodeRegistration / MetadataRegistration: `0x2772B68` / `0x285F870`.
- Original PC ZIP SHA-256: `2f08b4e2243e2bd296f3db694b4a2c9656252d76202c80167bdf3340b9f65b48`.
- PC GameAssembly SHA-256: `9ebd7ca996a5b03fb4a766f7a2502b660d581ddbbb36af4d7bf2f06a211da39d`.
- PC global-metadata.dat SHA-256: `ad992341add498bd1018ac980f42171e0737b0793bf945c8714ad70a445b36b9`.
- Original Android APK SHA-256: `428e0ba2e46645a905fb9fbb00cfde42406727a3ddfce9a7df1e0875889a739f`.
- Cpp2IL authoritative source commit: `5fb20304df698ffd3d0e664b2a698cd911dc9d57`.
- Reproduced PC Cpp2IL baseline: `2318 / 2319`; sole full failure `Zombie::InjuryStatusUpdate_Body`, already closed in HF3.
- Original Assembly-CSharp MethodDef -> native attribution remains `original RID - 1 -> Assembly-CSharp CodeGenModule.methodPointers[index]`; do not use Cpp2IL-rewritten RID.

## Final cumulative HF chain

- HF1 — `31e6f6a781c6350a99a7317cade6cebc11cacd562a4c129961eef2d53af1d471`.
- HF2 audited — `a21c9e1d1e6994eb559694a51123d33b8a1eb3b262bda6d372f97bbf51523633`; old `d445395f...` superseded.
- HF3 `Zombie.InjuryStatusUpdate_Body` — Exact — `23014656af797490bc35d9feb8f78950bcfa79a167c6a1de3303e1d30251c32f`.
- HF4 `Zombie.Awake` — Exact — `2abbc9eb02b93bcd0178091bbc38ca450b6162875df9dcb55e874d5b9af9f0a6`.
- HF5 `Zombie.Start` + `LoopAddAnimation` — Exact — `58fe2001b6d9df986a08a930e60ea936fbf9392d82532f2e815bda63557d5e3a`.
- HF6 `Zombie.Update` — Exact — `e6395652d0d413fbb496dc9cae8f2712d6a65bcfad109d160818983cde234e75`.
- HF7 `Zombie.GetMoveDirection` — Exact — `ed01e0aa7dac968e8203131e6274a4da718ebe5fdcd0dcb3d3bc41d3b0f60ea4`.
- HF8 `Zombie.SetrSpeed` — Exact — `4dc9b2486ae41b2d9c340293dada22e74ee6877290e0e79f456b770b92bb4688`.
- HF9 `Zombie.Update_Attack` — Exact — `c3e1716bbd598a8b2d2af3b1b5907869cc581271645ccc2a9e81595dfac07ec2`.
- HF10 `Zombie.Update_Characteristic` — Exact — `0684c52733afd5dd1d45e455ede8ec5355d8b53bb65875506cb97f9a29f07b8a`.
- HF11 `BuffManager.Update<T>` — Exact managed-observable — `c990a8be0a615cbd1aed15b08251ae892889c056badc3e4070d7c6c24a6cb07f`.
- HF12 `Buff.Update<T>` — Exact managed-observable — `963f24a8323025e424c2e392d4f90ab1d7d48a8f5aa0b671775ac64402788930`.
- HF13 `Buff.Start<T>` + `Buff.End<T>` — Exact managed-observable — `a389fcf0f6a97b4ace5cc580fb9704fa9830dbeba9b50edc4c5cad90489ebac3`.
- HF14 `StatsIncreased.Start_stats<T>` + `End_stats<T>` — Exact managed-observable — `cbe30c99a973f41bbf4bb0f0f56f62d37e1c752c0723159cc07feeb3d2a0efe7`.
- HF15 `Bleed.Bleeding<T>` — Exact managed-observable — `87c74aea233372a5a7af28c0df244a18873af111c0230b54d9acf82e68ef9ec1`.
- HF16 `AttackRange.TestInRange<T>` — Exact managed-observable — `57100e6b9296f5a6c1ff4710cc2859810dcfff9992a27ea5238000729c3888c3`.
- **HF17 `Buff.Awake(bool)` + `Hide.Awake_Hide()` + `Hide.Updata_Hide()` + `Hide.End_Hide()` — Exact managed-observable — `0eb0eba10cb27c5cff61e1a75947f146f5213ec036ff2ca3f95cf7d406ff1a67`.**

HF3 through HF17 are cumulative. Whole-assembly semantic isolation confirms each stage changes only its declared target MethodDef(s). HF16->HF17 changes exactly four declared MethodDefs: one physical hunk for `Buff::Awake` and one physical hunk containing only the three adjacent Hide methods; no fifth method changes.

## HF17 technical result

Targets and original PC bodies:

- `Buff.Awake(bool child)` — RID 240, token `0x060000F0`, `0x180310710–0x18031097B`.
- `Hide.Awake_Hide()` — RID 254, token `0x060000FE`, `0x18031C530–0x18031C61C`.
- `Hide.Updata_Hide()` — RID 255, token `0x060000FF`, logical `0x18031C730–0x18031C8F4`.
- `Hide.End_Hide()` — RID 256, token `0x06000100`, `0x18031C620–0x18031C728`.

Why `Buff.Awake` is included: the PC native `Buff.Awake` carries the Hide-start behavior inline. Restoring only `Updata_Hide` / `End_Hide` would leave Hide initialization incomplete.

Native-backed lifecycle:

- `Hide.Awake_Hide`: hide mode sets `Zombie.invincible=true` and `Zombie.hide=true`; visible mode enables Renderer if truthy, clears `Zombie.hide`, and writes `waitingTime=0.02f`.
- `Hide.Updata_Hide`: derives `t` from doubled `duration`, preserves native ordered float clamp behavior, computes the exact grayscale/alpha formulas, clamps alpha through `System.Math.Clamp(float,float,float)`, constructs `Color`, and calls `Zombie.SetColor(Color)`.
- `Hide.End_Hide`: visible mode clears invincibility; hide mode disables Renderer if truthy and moves the Zombie to `Vector3(fX,fY,1000)`.
- `Buff.Awake`: when not a child, enumerates `childBuffs`, preserves null-child `NullReferenceException` and Enumerator finally/Dispose, calls `child.Awake(true)`, then dispatches Hide instances to the independently native-backed `Awake_Hide()`.

Validation:

- corrected HF17 patcher source commit `693c87be6f639cbbf507f787710fa348d7606c2c`;
- corrected patcher CI run `34217812188` — success;
- artifact SHA-256 `97f63e050ce1356ff73dec6f1f4a25ea0245f6475315fd55e869de6ca13fd046`;
- formal HF16 input re-fetched from Drive and re-hashed before patching;
- actual patch + repeat are byte-identical;
- reopen: `Buff.Awake 39 IL / 129 bytes / 1 finally`; Hide methods `33/110`, `66/234`, `37/134`, all `0 EH`;
- permanent RecoveryAudit commit `3ff0a81c1944988d5eab132840678aa76d61b23d`, workflow `34218022069` — success; OPEN1/OPEN2 cumulative chain ends `RECOVERY_AUDIT_OK`;
- ILSpyCmd `11.0.0.9375` with full fresh fixed-Cpp2IL reference set: Hide, Buff.Awake, and whole-assembly stderr `0`;
- normalized HF16->HF17 semantic diff SHA-256 `e669dddcb73c075906e0e5470025229deb7b28508b701c101d517f6f658b8606`.

HF17 Drive archive is complete and independently listed:

- directory: `PVZ GOD/HighFidelity-Recovery-2026-09-08/HF17-Hide-Lifecycle`;
- folder ID: `1gMPlCp0xtHXYK2hZaxTp2sb_CIX95HAE`;
- final DLL Drive ID: `1RVb5I23-C0LJ3EXCXCnP4KBn9Vuon6vS`;
- corrected patcher Drive ID: `1C8aub5egyJAkMOZljem8yT0qefH9LVBS`;
- independent post-upload listing verified 16 final files.

## Earlier final archives

- HF14: `HF14-StatsIncreased`, folder `1-WOW4iCvmUAX9k6DAwxoXpc3frcCxCef`, 13 files.
- HF15: `HF15-Bleed`, folder `1y7WGFVI7q1RGZjedzqj2hIWpT-58GhbV`, 15 files.
- HF16: `HF16-AttackRange`, folder `1sFxLFILZj2hLBn4VQ22NTxS6K52nCzPM`, 13 files; final DLL `1AVIqB82No7M3U_Y2eKLwOBbBZYDToRIQ`.

## Unity reconstruction state

- AssetRipper export: about 3,149 Unity objects; reconstructed project about 6,783 files.
- 173 serialized game script types migrated to recovered `Assembly-CSharp.dll` local file IDs; PC DLL replacement matches 173/173.
- 263 game-script references across 114 serialized assets migrated.
- Restored scenes: `Assets/Scenes/MainMenu.unity`, `Assets/Scenes/Board.unity`.
- Fixed package versions: UGUI 1.0.0, TMP 3.0.6, RenderPipelines Core/URP 14.0.11, 2D Animation 9.1.1, Tilemap Extras 3.1.2, Burst 1.8.17, Collections 1.2.4, Mathematics 1.2.6, Visual Scripting 1.9.4.
- Serialized package references cover 67 package script types and still require 67/67 Unity validation.

## Remaining blockers before iOS/IPA

Immediate next action is **HF18 call-centrality rescan**, not a preselected patch. Rescan original native/metadata against the active Plant/Zombie/Buff/Board paths and rank remaining damaged or unreliable methods in path helpers, Device, Element, Projectile, Skill, and Plant specialized combat logic.

Decision gate after rescan:

1. If one or more high-centrality active-path methods are materially damaged/untrustworthy, recover only those as HF18+ with the same native-evidence/formal-input/CI/Cecil/ILSpy/Drive gates.
2. If no critical damaged methods remain in the active Plant/Zombie/Buff/Board chains, **stop adding HF stages** and move to Unity import/IL2CPP validation instead of recovering low-centrality code for its own sake.

After managed recovery closes:

1. import in Unity `2022.3.44f1c1`;
2. restore exact packages;
3. verify 67/67 package script migration;
4. verify the final recovered Assembly-CSharp is accepted and Unity IL2CPP converts it;
5. add only minimal iOS adaptation;
6. export Xcode project, unsigned build, package IPA;
7. true-device validation through a complete level.

The project remains high-fidelity-first. A candidate stage is not final until native evidence, CI, formal-input patch, reopen, permanent Cecil, ILSpy with full refs, whole-assembly semantic isolation, GitHub Evidence/STATUS, Drive archive, and fixed SHA all pass.