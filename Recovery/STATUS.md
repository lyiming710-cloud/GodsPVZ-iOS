# Recovery status

## Current strategy

Use the PC x86-64 IL2CPP build as the primary gameplay-logic source, Android native as the second reference, then original metadata/assets/JSON, with Cpp2IL/ILSpy used for attribution and managed reconstruction support. Do not replace native-backed gameplay with rescue-route approximations.

## Fixed original baseline

- Unity editor: `2022.3.44f1c1`.
- IL2CPP metadata: `31.1`.
- PC CodeRegistration / MetadataRegistration: `0x1815E88C0` / `0x1818C6D00`.
- Android CodeRegistration / MetadataRegistration: `0x2772B68` / `0x285F870`.
- Original PC ZIP SHA-256: `2f08b4e2243e2bd296f3db694b4a2c9656252d76202c80167bdf3340b9f65b48`.
- Original Android APK SHA-256: `428e0ba2e46645a905fb9fbb00cfde42406727a3ddfce9a7df1e0875889a739f`.
- Cpp2IL authoritative source commit: `5fb20304df698ffd3d0e664b2a698cd911dc9d57`.
- Reproduced PC Cpp2IL baseline: `2318 / 2319`; sole full failure `Zombie::InjuryStatusUpdate_Body`, already closed in HF3.
- Original Assembly-CSharp MethodDef -> native attribution remains `original RID - 1 -> Assembly-CSharp CodeGenModule.methodPointers[index]`; do not use Cpp2IL-rewritten RID.

## Final cumulative HF chain

- HF1 — `31e6f6a781c6350a99a7317cade6cebc11cacd562a4c129961eef2d53af1d471`.
- HF2 audited — `a21c9e1d1e6994eb559694a51123d33b8a1eb3b262bda6d372f97bbf51523633`. Old HF2 `d445395f...` is superseded.
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
- **HF15 `Bleed.Bleeding<T>` — Exact managed-observable — `87c74aea233372a5a7af28c0df244a18873af111c0230b54d9acf82e68ef9ec1`.**

HF3 through HF15 are cumulative. Whole-assembly semantic isolation confirms that each stage changes only its declared target MethodDef(s): HF13->HF14 has exactly two target hunks; HF14->HF15 has exactly one hunk ending at `Bleed::Bleeding`.

## HF14 final archive

HF14 is fully closed, including the previously pending Drive gate.

- Drive directory: `PVZ GOD/HighFidelity-Recovery-2026-09-08/HF14-StatsIncreased`.
- Folder ID: `1-WOW4iCvmUAX9k6DAwxoXpc3frcCxCef`.
- Post-upload listing verified 13 final files.

## HF15 technical result

Target: `Bleed.Bleeding<T>(T target)` — original RID `251`, token `0x060000FB`, direct generic-definition pointer `0`.

PC shared generic body:
- HF12 direct callsite `0x18042B02E -> 0x180428C90`.
- logical native range `0x180428C90–0x180428E64` fixed by four `.pdata` runtime-function entries.
- full executable-section E8 scan finds exactly one direct xref to the body.

Native-backed behavior:
- null reference host returns;
- independent `Zombie`, `Plant`, `Device` gates;
- `multi == false`: rate = `value`;
- `multi == true`: rate = `host.maxHealthPoint * value`;
- each matched host independently reads current `healthPoint`, calls `Time.get_deltaTime()`, subtracts `deltaTime * rate`, and writes `healthPoint`;
- `Bleed.minHealthPoint` exists at `+0x5C` but is not read by this PC native body.

Original field offsets used by native:
- Bleed `value +0x58`, `minHealthPoint +0x5C`, `multi +0x60`;
- Zombie `healthPoint/maxHealthPoint +0x70/+0x74`;
- Plant `+0x94/+0x98`;
- Device `+0x74/+0x78`.

Original UnityEngine.CoreModule mapping resolves PC call target `0x18132C2F0` to `Time.get_deltaTime()` RID `2370`, token `0x06000942`.

Validation:
- HF15 patcher workflow `34201832021` — success.
- Formal HF14 actual patch + repeat are byte-identical.
- Patcher reopen: `93 IL / 317 bytes / 0 EH`.
- Permanent RecoveryAudit workflow `34201989968` — success; actual OPEN1/OPEN2 pass HF3–HF15, `RECOVERY_AUDIT_OK`.
- Exact Cpp2IL source-rebuild workflow `34202655285` — success; source SHA verified as `5fb20304...`.
- Fresh PC reference regeneration: 56 DLLs, `2318/2319`, sole old HF3 failure only.
- ILSpyCmd `11.0.0.9375` with full regenerated references: member stderr `0`; whole HF14/HF15 stderr `0`.
- Whole-assembly HF14->HF15 semantic isolation: exactly one target hunk.

HF15 Drive archive is complete and independently listed:
- directory: `PVZ GOD/HighFidelity-Recovery-2026-09-08/HF15-Bleed`;
- folder ID: `1y7WGFVI7q1RGZjedzqj2hIWpT-58GhbV`;
- 15 final files, including final DLL, patcher, native/metadata evidence, ILSpy/Cecil, semantic diff, fixed Cpp2IL tool/reference set, logs and SHA256SUMS.

## Unity reconstruction state

- AssetRipper export: about 3,149 Unity objects; reconstructed project about 6,783 files.
- 173 serialized game script types migrated to recovered `Assembly-CSharp.dll` local file IDs; PC DLL replacement still matches 173/173.
- 263 game-script references across 114 serialized assets migrated.
- Restored scenes: `Assets/Scenes/MainMenu.unity`, `Assets/Scenes/Board.unity`.
- Exact package versions remain fixed: UGUI 1.0.0, TMP 3.0.6, RenderPipelines Core/URP 14.0.11, 2D Animation 9.1.1, Tilemap Extras 3.1.2, Burst 1.8.17, Collections 1.2.4, Mathematics 1.2.6, Visual Scripting 1.9.4.
- Serialized package references cover 67 package script types and must still be validated 67/67 in Unity.

## Remaining blockers before iOS/IPA

Do not begin IPA packaging yet. Continue native-backed runtime recovery by call centrality.

Immediate next priority:
1. `AttackRange.TestInRange<T>` — direct HF12 dependency for Zombie/Projectile range buffs.
2. `Hide.Updata_Hide()` / `Hide.End_Hide()` — HF12/HF13 specialized lifecycle dependencies.
3. Then rescan native call centrality for path helpers, Device, Element, Projectile, Skill and remaining Plant specialized combat logic.

After core recovery is sufficiently closed:
1. import in Unity `2022.3.44f1c1`;
2. restore exact packages;
3. verify 67/67 package script migration;
4. verify final recovered Assembly-CSharp is accepted and Unity IL2CPP converts it;
5. add only minimal iOS adaptation;
6. export Xcode project, unsigned build, package IPA;
7. true-device validation through a complete level.

The project remains high-fidelity-first; candidate stages must not be called final until native evidence, CI, formal-input patch, reopen, Cecil, ILSpy, semantic isolation, Evidence, STATUS, Drive archive and fixed SHA are all complete.
