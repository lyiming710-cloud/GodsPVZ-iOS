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
- **HF19 `Zombie.IsDisabled` + `IsNormalZombie` + `IsPlantZombie` — Exact managed-observable — `e6e303c660b0351611370ebe29746954c5535344160c6e580d9314405b67f720`.**

## HF19 technical result

HF19 restores original Zombie ID classification/disabled predicates from PC jump tables:

- `IsDisabled` RID1124 token `0x06000464` PC `0x1803659E0`;
- `IsNormalZombie` RID1126 token `0x06000466` PC `0x180365AB0`;
- `IsPlantZombie` RID1127 token `0x06000467` PC `0x180365B00`.

Native truth sets: normal `{0,2,4,5,14,15}`; plant-zombie `{6,7,8,9,11,12,19,20,21,22}`. `IsDisabled` returns true for `isDied` or `ashes`; otherwise the native-listed Zombie ID classes return `isDying`, all other IDs false. Original fields are `ID +0x60`, `isDying +0xB7`, `isDied +0xB8`, `ashes +0xB9`.

Validation:

- corrected patcher source commit `4994778de21b3403f0a54c82879a07d4e05e77b1`;
- corrected patcher CI run `34224218252` success; artifact SHA-256 `2ed8f8fed6f62cf7f581e5ef1be774858862579a5c077be0fa85072579dcc076`;
- formal HF18 re-fetched from Drive and re-hashed before patching; two outputs byte-identical;
- reopen `IsDisabled 52 IL/208 bytes`, `IsNormalZombie 25/98`, `IsPlantZombie 37/154`, all 0 EH;
- permanent RecoveryAudit commit `e14cf3cb918a99aafc7e946c031b2f45dc798dc4`, workflow `34224367146` success; OPEN1/OPEN2 end `RECOVERY_AUDIT_OK`;
- ILSpyCmd `11.0.0.9375`, full fresh fixed-Cpp2IL refs: Zombie and whole-assembly stderr 0;
- HF18->HF19 semantic isolation changes exactly three declared MethodDefs, no fourth; diff SHA-256 `a90fbb0a5cc96c2a341bbd19dfce6e4e97aff4d4330863e76fb4bf3625595631`.

HF19 Drive archive is complete and independently listed:

- folder `1-PpX9-cAmJ7Sx_eUdC0QOSh5GSgGvZSo`;
- final DLL `1kxSsPudimmcq9o_evxLqSIVTavoF4DZ9`;
- corrected patcher `1R2BSes6C-p08B741i7375XhKKlnXWM_w`;
- SHA256SUMS `1f2BqIp0vAH1qE0qaPK0HiHSjjtGBl6xW`;
- 17 final files verified.

GitHub Evidence: `Tools/HighFidelityPatch/Evidence/HF19-Zombie-Predicates.md`.

## Unity reconstruction state

AssetRipper ~3,149 objects; reconstructed project ~6,783 files. 173 game script types and 263 refs across 114 assets migrated. MainMenu/Board scenes restored. Fixed package set: UGUI 1.0.0, TMP 3.0.6, Core/URP 14.0.11, 2D Animation 9.1.1, Tilemap Extras 3.1.2, Burst 1.8.17, Collections 1.2.4, Mathematics 1.2.6, Visual Scripting 1.9.4. 67 package script types still need 67/67 Unity validation.

## Current decision gate

Run one final unique-pointer/active-path rescan from HF19. Previously visible next candidates include `Plant.GetDamage`, `ProjectileManager.CrateNewProjectile`, and `Damage.AreaDamage`; do not automatically patch them. Audit whether any remaining helper/unknown IL materially affects the active Plant/Zombie/Projectile/Board combat chain.

- If a coherent critical damaged cluster remains, recover only it as HF20+ with the same native/formal/CI/Cecil/ILSpy/Drive gates.
- If no critical cluster remains, **stop adding HF stages** and move to Unity `2022.3.44f1c1` import, package restoration, 67/67 package-script validation, and recovered Assembly-CSharp IL2CPP conversion.

High-fidelity-first remains mandatory.