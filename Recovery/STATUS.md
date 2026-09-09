# Recovery status

## Current strategy

Use PC x86-64 IL2CPP as the primary gameplay source, Android native second, then original metadata/assets/JSON; use Cpp2IL/ILSpy for attribution and managed reconstruction support. Do not substitute rescue-route approximations for native-backed gameplay.

High-fidelity-first remains mandatory: a recovered stage is not final until native attribution, deterministic formal-input patching, permanent Cecil audit, fixed-version ILSpy readback, whole-assembly semantic isolation, GitHub evidence, and Google Drive provider acceptance all close.

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
- HF21 Projectile / Resource cluster `888cab48b5e488ca05ed8ec58a11503c9f9a8ce0bac23a60c933f8628834e8f0`.
- **HF22 Damage Dispatch / Area Core — Exact managed-observable — `502d6d61c18c17e29acb2f96451e9e41f98a403d8f8e915a76285c5b51f04997`.**

## HF22 technical result

HF22 restores exactly 12 MethodDefs:

- `Damage.AreaDamage` `0x0600010C`;
- `Damage.AreaDamage_Device` `0x0600010D`;
- `Damage.AreaDamage_Plant` `0x0600010E`;
- `Damage.AreaDamage_Zombie` `0x0600010F`;
- `ElementManager.ToEffect` `0x06000124`;
- `ElementManager.GetElement` `0x06000127`;
- `Device.CanAttacked` `0x06000310`;
- `Device.TakeDamage` `0x06000336`;
- `Plant.TakeDamage` `0x060003B3`;
- `Zombie.CanAttacked` `0x06000431`;
- `Zombie.GetATK` `0x06000441`;
- `Zombie.TakeDamage` `0x0600047F`.

Recovered behavior includes original AreaDamage Board/camp routing; typed target snapshot loops and damagePoint restoration; Element queue/search semantics; Device attackability and ice/HP damage (`0.2` ice coefficient, `0.9` reduction cap, ID13 `0.3` transfer); Plant/Zombie `MathF.Round` stats paths; and Zombie damage dispatch/stiffness/buff/element/KillEvent behavior.

## HF22 validation and acceptance

- formal HF21 input re-fetched from accepted Drive archive and re-hashed `888cab48b5e488ca05ed8ec58a11503c9f9a8ce0bac23a60c933f8628834e8f0`;
- final patcher build head `74f23e6a08757c2e0f567338f437f350b84f1c27`, CI run `34259165102` success, artifact SHA-256 `95b1c0d4fd54bd5a03e2b80f5a457264e472db312c4365d59e631badc6e6483a`;
- two earlier tool-only failures produced no candidate; the accepted patcher applied twice independently and both outputs were byte-identical at `502d6d61c18c17e29acb2f96451e9e41f98a403d8f8e915a76285c5b51f04997`;
- permanent RecoveryAudit commit `1c25249751f45935b468f56cad8f9c66cbfcb56e`, workflow `34259348121` success; OPEN1/OPEN2 `RECOVERY_AUDIT_OK`;
- ILSpyCmd / ICSharpCode.Decompiler fixed at `11.0.0.9375`; 12/12 member readbacks stderr 0; whole HF21/HF22 readbacks with the reproduced 56-DLL reference set stderr 0;
- MethodDef count `2317 -> 2317`; normalized non-method skeleton byte-identical; exactly the declared 12 MethodDefs change; formal semantic diff SHA-256 `1adaabb8c214895f9244b85b1fcd620e42c67aa1b7e4bab0bb86165b2c528e7c`.

HF22 Drive archive is accepted:

- folder `1hxsyCUrCVtDwDgkIYAg_y6T0wedtecwT`;
- final audited DLL `1VH6agXwBpnGVoonUpkLP9a-vt6Xbv8YP`;
- final patcher `1Tr85G5PmeFTeZUmW0RDjvOAJCW77ka9m`;
- RecoveryAudit `1chfZ5ev40t1XVAKPKjlYCLa9LqMGjY3H`;
- ILSpy bundle `1KeEM8fA5eXVebqH1yToyEdEi5R8rmA3w`;
- native evidence `12QjaZhKLtj1ZKF8MWhW3y5BLqvY0HhHu`;
- semantic diff `11kgk3lTBn_Nsm5bJ1Af-OwDZQqqefZ-_`;
- authoritative Evidence-FINAL `18-SpQLwYEmopX7uD7BLNqC_uzJXJYvhZ`, SHA-256 `6e7a165b4ac6651387870c9cd7d336338b2356e502736556ef3f7547328856fa`;
- authoritative SHA256SUMS-FINAL `1ZnPqsmLpKjBpOqOg3sxbFVqCO_PRRRCm`, SHA-256 `ea8bf2ec285fd5250eea861325e40a167415a3e07eaa01bdc458a12d2bddfc86`.

Provider readback verified exactly 22 expected files: 20 payloads plus 2 closure files.

GitHub Evidence: `Tools/HighFidelityPatch/Evidence/HF22-Damage-Dispatch-Core.md`.

**HF22 formal acceptance: PASS.**

## Unity reconstruction state

AssetRipper ~3,149 objects; reconstructed project ~6,783 files. 173 game script types and 263 refs across 114 assets migrated. MainMenu/Board scenes restored. Fixed package set: UGUI 1.0.0, TMP 3.0.6, Core/URP 14.0.11, 2D Animation 9.1.1, Tilemap Extras 3.1.2, Burst 1.8.17, Collections 1.2.4, Mathematics 1.2.6, Visual Scripting 1.9.4. 67 package script types still require 67/67 Unity validation.

## Current blocker: HF23 Zombie Hurt core

HF23 is a direct managed blocker, not optional cleanup. HF22-restored `Zombie.TakeDamage` directly dispatches into nine still-damaged original bodies:

- `0x06000456 Zombie.Hurt_Armor1` — PC `0x1803625D0`;
- `0x06000457 Zombie.Hurt_Armor2` — PC `0x180362BE0`;
- `0x06000458 Zombie.Hurt_Artillery`;
- `0x06000459 Zombie.Hurt_Ashes`;
- `0x0600045A Zombie.Hurt_Body` — PC `0x1803635F0`;
- `0x0600045B Zombie.Hurt_FinalDamageReduction`;
- `0x0600045C Zombie.Hurt_Normal`;
- `0x0600045D Zombie.Hurt_Real`;
- `0x0600045E Zombie.Hurt_Throughout`.

A native preview bundle is already archived under HF22 Drive (`1C5LhRgA1spuU2MDQnyb4U_Pm95pCbYq4`). Close these nine under the same formal gates before deciding whether managed recovery can stop and Unity/iOS build validation may begin.
