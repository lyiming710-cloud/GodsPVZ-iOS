# Recovery status

## Current strategy

Use PC x86-64 IL2CPP as primary gameplay source, Android native second, then original metadata/assets/JSON; use Cpp2IL/ILSpy for attribution and managed reconstruction support. Do not substitute rescue-route approximations for native-backed gameplay.

## Fixed original baseline

- Unity `2022.3.44f1c1`; metadata `31.1`.
- PC CodeRegistration / MetadataRegistration `0x1815E88C0` / `0x1818C6D00`.
- PC ZIP SHA-256 `2f08b4e2243e2bd296f3db694b4a2c9656252d76202c80167bdf3340b9f65b48`.
- PC GameAssembly SHA-256 `9ebd7ca996a5b03fb4a766f7a2502b660d581ddbbb36af4d7bf2f06a211da39d`.
- PC metadata SHA-256 `ad992341add498bd1018ac980f42171e0737b0793bf945c8714ad70a445b36b9`.
- Android APK SHA-256 `428e0ba2e46645a905fb9fbb00cfde42406727a3ddfce9a7df1e0875889a739f`.
- Cpp2IL source commit `5fb20304df698ffd3d0e664b2a698cd911dc9d57`; reproduced PC baseline `2318/2319`, sole full failure closed in HF3.
- Native attribution: original RID-1 -> Assembly-CSharp CodeGenModule methodPointers index.

## Final cumulative HF chain

- HF1 `31e6f6a781c6350a99a7317cade6cebc11cacd562a4c129961eef2d53af1d471`.
- HF2 audited `a21c9e1d1e6994eb559694a51123d33b8a1eb3b262bda6d372f97bbf51523633`.
- HF3 `23014656af797490bc35d9feb8f78950bcfa79a167c6a1de3303e1d30251c32f`.
- HF4 `2abbc9eb02b93bcd0178091bbc38ca450b6162875df9dcb55e874d5b9af9f0a6`.
- HF5 `58fe2001b6d9df986a08a930e60ea936fbf9392d82532f2e815bda63557d5e3a`.
- HF6 `e6395652d0d413fbb496dc9cae8f2712d6a65bcfad109d160818983cde234e75`.
- HF7 `ed01e0aa7dac968e8203131e6274a4da718ebe5fdcd0dcb3d3bc41d3b0f60ea4`.
- HF8 `4dc9b2486ae41b2d9c340293dada22e74ee6877290e0e79f456b770b92bb4688`.
- HF9 `c3e1716bbd598a8b2d2af3b1b5907869cc581271645ccc2a9e81595dfac07ec2`.
- HF10 `0684c52733afd5dd1d45e455ede8ec5355d8b53bb65875506cb97f9a29f07b8a`.
- HF11 `c990a8be0a615cbd1aed15b08251ae892889c056badc3e4070d7c6c24a6cb07f`.
- HF12 `963f24a8323025e424c2e392d4f90ab1d7d48a8f5aa0b671775ac64402788930`.
- HF13 `a389fcf0f6a97b4ace5cc580fb9704fa9830dbeba9b50edc4c5cad90489ebac3`.
- HF14 `cbe30c99a973f41bbf4bb0f0f56f62d37e1c752c0723159cc07feeb3d2a0efe7`.
- HF15 `87c74aea233372a5a7af28c0df244a18873af111c0230b54d9acf82e68ef9ec1`.
- HF16 `57100e6b9296f5a6c1ff4710cc2859810dcfff9992a27ea5238000729c3888c3`.
- HF17 `0eb0eba10cb27c5cff61e1a75947f146f5213ec036ff2ca3f95cf7d406ff1a67`.
- HF18 Buff infrastructure `0e1b1acffa3329b3f98d2b61fb346c3f6647bee84e38bfab1d067a7aa17d58ed`.
- HF19 Zombie predicates `e6e303c660b0351611370ebe29746954c5535344160c6e580d9314405b67f720`.
- **HF20 Plant damage pipeline — Exact managed-observable — `b16b3b89fad8f8081d86611a732696c56db6bd2dbab6302b6cef7fa94e141b3e`.**

## HF20 technical result

HF20 restores exactly seven MethodDefs from PC native:

- `AttackRange.NewCircleRange<T>(T, Transform, float)` token `0x060000CF`, shared PC `0x180426380`;
- `AttackRange.NewCircleRange<T>(T, Transform, float, Vector3)` token `0x060000D0`, shared PC `0x180426070`;
- `Damage.AddElement(Element)` token `0x0600010B`, PC `0x180310DC0`;
- `Element::.ctor(ElementType,float,Damage,bool)` token `0x0600011C`, PC `0x180316940`;
- `Plant.GetATK()` token `0x06000367`, PC `0x180350310`;
- `Plant.GetDamage(Projectile,int,int)` token `0x0600036D`, PC `0x180350E40`;
- `Plant.GetDamageRange(Damage,Projectile,int,int)` token `0x06000371`, PC `0x1803508A0`.

Recovered behavior includes the Circles AttackRange builders and host gates; exact Element merge/constructor semantics; `MathF.Round` in Plant ATK; native range radii `80/155/160/195/225`; actual `specialType` forwarding; two Plant damage jump tables; native damage flags/multipliers; and ID5 Element creation with `shuttle_able=true`.

Validation:

- first compiled patcher produced no candidate because an existing `Vector3.op_Subtraction` MemberRef was required;
- source repaired in commit `a2a38eb8f0b943e28534bfb4ab0b31f935fd0be2` to construct the static MethodRef explicitly;
- corrected CI run `34235308941` success; artifact SHA-256 `38275ef0a2831c5e1498155558b48ba4ca2151da3f67ec877c801e5b2bb3caed`;
- formal HF19 re-fetched from Drive and re-hashed; two HF20 outputs byte-identical;
- permanent RecoveryAudit commit `ca3bad1e0669d3019646125cbee726dbbb8828e7`, workflow `34235553691` success; OPEN1/OPEN2 `RECOVERY_AUDIT_OK`;
- ILSpyCmd `11.0.0.9375` with full reproduced fixed-Cpp2IL refs: all seven target and whole-assembly readbacks stderr 0;
- HF19->HF20 semantic isolation changes exactly the seven declared MethodDefs, no eighth; diff SHA-256 `5094a498d6998ae8a60040ffb3791f909ec2e5987428ebdbdccee42ef5b1e358`.

HF20 Drive archive is accepted:

- folder `15ZPNk4Zb5LuSdZ8RrueUGpYowmo9AFGP`;
- final DLL `1VmdyyZzfA4rmjZHeMrttHXYfgXPqnDBZ`;
- corrected patcher `1N9_ExWfDO8JD_GoVfz_xAB4UHEtom69p`;
- authoritative Evidence-FINAL `1T52CgV39NdDPo3V2QLJMzxWAy6TVUTIF`;
- authoritative SHA256SUMS-FINAL `1uppHfrvURT0S5rqqc-iBygcvL5eSr0iC`.

Drive ordinary-file overwrite was unavailable, so the two initial closure files are retained as explicitly superseded immutable drafts. Final provider listing verified 29 expected files: 25 payloads + 2 superseded closure drafts + 2 authoritative FINAL closure files.

GitHub Evidence: `Tools/HighFidelityPatch/Evidence/HF20-Plant-Damage-Pipeline.md`.

## Unity reconstruction state

AssetRipper ~3,149 objects; reconstructed project ~6,783 files. 173 game script types and 263 refs across 114 assets migrated. MainMenu/Board scenes restored. Fixed package set: UGUI 1.0.0, TMP 3.0.6, Core/URP 14.0.11, 2D Animation 9.1.1, Tilemap Extras 3.1.2, Burst 1.8.17, Collections 1.2.4, Mathematics 1.2.6, Visual Scripting 1.9.4. 67 package script types still need 67/67 Unity validation.

## Current decision gate

Audit the remaining ProjectileManager core before opening another HF stage. Primary suspects are `ProjectileManager.CrateNewProjectile(int)`, `Start()`, and `SetFloatScale()`: prior Cpp2IL evidence shows material damage, but exact PC native scope must close first.

- If ProjectileManager forms a coherent critical active-path damaged cluster, recover only that cluster as HF21 with the same native/formal/CI/Cecil/ILSpy/Drive gates.
- If the ProjectileManager audit shows no remaining critical active-path loss, **stop adding HF stages** and move to Unity `2022.3.44f1c1` import, package restoration, 67/67 package-script validation, and recovered Assembly-CSharp IL2CPP conversion.

High-fidelity-first remains mandatory.