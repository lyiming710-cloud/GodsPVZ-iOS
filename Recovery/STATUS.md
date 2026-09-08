# Recovery status

## Current strategy

Use PC x86-64 IL2CPP as the primary gameplay source, Android native second, then original metadata/assets/JSON; use Cpp2IL/ILSpy for attribution and managed reconstruction support. Do not substitute rescue-route approximations for native-backed gameplay.

High-fidelity-first remains mandatory: a recovered stage is not final until native attribution, deterministic patching, permanent Cecil audit, fixed-version ILSpy readback, whole-assembly semantic isolation, GitHub evidence, and Google Drive archive acceptance all close.

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
- HF20 Plant damage pipeline `b16b3b89fad8f8081d86611a732696c56db6bd2dbab6302b6cef7fa94e141b3e`.
- **HF21 Projectile / Resource cluster — Exact managed-observable — `888cab48b5e488ca05ed8ec58a11503c9f9a8ce0bac23a60c933f8628834e8f0`.**

## HF20 transition result

HF20 restored the Plant damage pipeline and its direct range/element dependencies: `AttackRange.NewCircleRange<T>` overloads, `Damage.AddElement`, `Element::.ctor`, `Plant.GetATK`, `Plant.GetDamage`, and `Plant.GetDamageRange`. HF19 -> HF20 semantic isolation changed exactly those seven MethodDefs. The accepted HF20 Drive archive remains folder `15ZPNk4Zb5LuSdZ8RrueUGpYowmo9AFGP`; detailed evidence is `Tools/HighFidelityPatch/Evidence/HF20-Plant-Damage-Pipeline.md`.

## HF21 technical result

HF21 restores exactly eight MethodDefs from PC native:

- `ParticlesManager.Start()` — token `0x060001E8`, PC `0x180322040`;
- `ParticlesManager.CreatNewParticle(ParticleState)` — token `0x060001EA`, PC `0x180321F30`;
- `ProjectileManager.Start()` — token `0x060001F3`, PC `0x1803248E0`;
- `ProjectileManager.SetFloatScale()` — token `0x060001F4`, PC `0x1803242F0`;
- `ProjectileManager.CrateNewProjectile(int)` — token `0x060001F6`, PC `0x180324150`;
- `ResourceManager.Load_projectileSprite()` — token `0x0600022E`, PC `0x18033A520`;
- `Projectile.ResetData()` — token `0x060003DD`, PC `0x18037BB60`;
- `Projectile.BindTrack()` — token `0x060003DE`, PC `0x180378FC0`.

Recovered behavior includes the 1000-object projectile pool; the exact 54-entry W/D/H scale table; inactive-pool reuse with `ResetData -> StartData -> SetActive(true)` and enumerator `finally`; complete Projectile state reset including native cross-field 64-bit stores; Snowflakes/Bolt track routing; ParticleState resource loading and prefab instantiation; and ProjectileType sprite loading/list growth with the original failure logs and enumerator cleanup.

## HF21 validation and acceptance

- formal HF20 input was re-hashed as `b16b3b89fad8f8081d86611a732696c56db6bd2dbab6302b6cef7fa94e141b3e` before patching;
- final patch source SHA-256 `f5b7085dea090b2ea97434fd317b8db5e5035a3ee241bc3ee311f0be6165715e`;
- final patcher build commit `515c5f78ddd5abe68b8176e1a05b367f5a1fcf5e`, workflow run `34248270465` success, artifact ZIP SHA-256 `4bf66a06db9387e2bf5a1d150c8f5ab25d8af6840d6ac672d63a1b9730bd4960`;
- verifier calibration attempts stopped before candidate output; the accepted patcher was applied twice independently and both outputs were byte-identical at `888cab48b5e488ca05ed8ec58a11503c9f9a8ce0bac23a60c933f8628834e8f0`;
- final reopen counts: ProjectileManager.Start 54/188/0EH, SetFloatScale 271/919/0, CrateNewProjectile 36/140/1 finally, Projectile.ResetData 101/360/0, BindTrack 50/191/0, ParticlesManager.Start 58/243/1 finally, CreatNewParticle 20/64/0, ResourceManager.Load_projectileSprite 64/279/1 finally;
- permanent RecoveryAudit commit `a8926a18467fa1d436f086e71c5296dc75fe3bc6`, workflow `34248508707` success; OPEN1/OPEN2 both `RECOVERY_AUDIT_OK`;
- ILSpyCmd / ICSharpCode.Decompiler fixed at `11.0.0.9375`, complete reproduced 56-DLL Cpp2IL refs; all eight member readbacks and both whole-assembly readbacks exited 0 with stderr 0;
- whole MethodDef count remains 2317 -> 2317; normalized non-method skeleton is byte-identical; HF20 -> HF21 changes exactly the eight declared MethodDefs, semantic diff SHA-256 `b63926c9bfaafd2f5f00e4fdf2f3054831152c06285066778b43fcbdf83e20a1`.

HF21 Drive archive is accepted:

- folder `1YCwBHrkS1cfqSTV8R-oE0HPxgGrUffsb` (`HF21-Projectile-Resource-Cluster`);
- final audited DLL `1pg6j1bkcQTyWj9_obFCXryQvL3dASwEm`;
- final patcher `1H8lakklFTIcQWpbE3uvDEsYac_nT51Qd`;
- RecoveryAudit bundle `1xc-KUhgx-QLASfF7YPssZ_6miid3N0ql`;
- ILSpy bundle `15csOFTqduImTepAtjKZugfAJVIKA7GZK`;
- semantic diff `1bzx9fdS1qys0VYkP1SX9kDPkE29vsDDA`;
- Cecil audit `1mz8jaHhbDxEwjzS0OBjlVB9KLJunT8fK`;
- authoritative Evidence-FINAL `1Qrd2cZ3J1Zub7o89-IFV5S6fWMkxjaLr`, SHA-256 `e751d733781906485d65d47c5ee392508748b1baa0b30bc529381143ee317b25`;
- authoritative SHA256SUMS-FINAL `1jggygaIrRNdSgKYDkNThjiAZ6tDuGXVw`, SHA-256 `f86314aebeec2833a962a09b850242632294f8e8f661972caa272706d216903a`.

Provider readback verified exactly 35 expected files: 33 payloads plus the two closure files, with no duplicate/superseded drafts.

GitHub Evidence: `Tools/HighFidelityPatch/Evidence/HF21-Projectile-Resource-Cluster.md` (formal evidence commit `8d3a7f0774589abc721de0a6702f293366a13db0`).

**HF21 formal acceptance: PASS.**

## Unity reconstruction state

AssetRipper ~3,149 objects; reconstructed project ~6,783 files. 173 game script types and 263 refs across 114 assets migrated. MainMenu/Board scenes restored. Fixed package set: UGUI 1.0.0, TMP 3.0.6, Core/URP 14.0.11, 2D Animation 9.1.1, Tilemap Extras 3.1.2, Burst 1.8.17, Collections 1.2.4, Mathematics 1.2.6, Visual Scripting 1.9.4. 67 package script types still require 67/67 Unity validation.

## Current decision gate

Do **not** open HF22 automatically.

Run a remaining managed-damage / active-path gate scan across the accepted HF21 Assembly-CSharp and the original PC-native attribution. The purpose is to identify only surviving managed damage that can materially block original gameplay or Unity reconstruction.

- If no critical active-path damaged cluster remains, stop the HF sequence and move to Unity `2022.3.44f1c1` import/build validation, exact package restoration, 67/67 package-script validation, recovered Assembly-CSharp integration, and IL2CPP/iOS conversion work.
- If the gate finds a native-backed critical blocker, isolate that exact cluster first; only then may HF22 be opened under the same native/formal/CI/Cecil/ILSpy/Drive acceptance gates.

High-fidelity-first remains mandatory.