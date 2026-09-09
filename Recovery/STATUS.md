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
- Fixed ILSpyCmd / ICSharpCode.Decompiler `11.0.0.9375` with the reproduced 56-DLL fixed-Cpp2IL reference set.

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
- HF22 Damage Dispatch / Area Core `502d6d61c18c17e29acb2f96451e9e41f98a403d8f8e915a76285c5b51f04997`.
- **HF23 Zombie Hurt Core — native-backed formal final — `35d13b0d7fc5f82e04b837b812fab501a933b00e21f3431b2f6759c8e0c2b39c`.**

## HF23 technical result

HF23 restores exactly nine directly dispatched Zombie lower-damage MethodDefs:

- `0x06000456 Zombie.Hurt_Armor1` — PC `0x1803625D0`;
- `0x06000457 Zombie.Hurt_Armor2` — PC `0x180362BE0`;
- `0x06000458 Zombie.Hurt_Artillery` — PC `0x1803630B0`;
- `0x06000459 Zombie.Hurt_Ashes` — PC `0x180363320`;
- `0x0600045A Zombie.Hurt_Body` — PC `0x1803635F0`;
- `0x0600045B Zombie.Hurt_FinalDamageReduction` — PC `0x180363980`;
- `0x0600045C Zombie.Hurt_Normal` — PC `0x180363B00`;
- `0x0600045D Zombie.Hurt_Real` — PC `0x180363D90`;
- `0x0600045E Zombie.Hurt_Throughout` — PC `0x180363FD0`.

Important recovered behavior includes:

- original Zombie armor/body defense semantics with a `0.1` minimum non-real damage floor and real-damage bypass;
- `MathF.Round` in defense computation before the zero clamp;
- PoleCommander.speed damage-reduction cap `0.95` under the native gate;
- Armor1 iceCube `fire_ice` coefficient `0.2`;
- throughout armor penetration using native float->int truncation;
- distinct Artillery and Real handling of armor return values;
- Ashes disabled/dead and lethal Ashe behavior;
- original Armor1 display anchors from metadata-usage string literals;
- Armor2 hit-audio routing;
- body large-damage text thresholds/positioning;
- restoration of `Damage.damagePoint` after successful `Hurt_Throughout` body damage.

## HF23 validation and acceptance

- formal HF22 input was re-fetched from its accepted Drive archive and re-hashed `502d6d61c18c17e29acb2f96451e9e41f98a403d8f8e915a76285c5b51f04997`;
- original PC ZIP/GameAssembly/global-metadata were independently re-extracted/re-hashed before native closure;
- final patcher build head `80187a41b3652cc2fc29ab980fcfa6ade6cd0f80`, workflow run `34302953269` success, patcher artifact SHA-256 `dee8e41e1f01d23e63def80092d50a8272a2db92cb3049520d9190fc762948f5`;
- patcher applied twice independently to the same formal HF22 input, outputs byte-identical at HF23 SHA `35d13b0d7fc5f82e04b837b812fab501a933b00e21f3431b2f6759c8e0c2b39c`;
- Cecil reopen passes all nine targets and no target retains a Cpp2IL helper;
- permanent RecoveryAudit extension commit `504c9b36db406e6ed67fbe3e5d018f7e1cf746b7`, workflow `34303834229` success, published audit artifact SHA-256 `ccfc1f16e88ce3b8b5d482b65baf7f17030002bbef51ad95e9b4c77a1c98986d`;
- published auditor OPEN1/OPEN2 each report 320 types, 2317 methods, 2297 bodies and end `RECOVERY_AUDIT_OK`;
- fixed ILSpy 11.0.0.9375 gives 9/9 member readbacks with stderr 0 and whole HF22/HF23 stderr 0;
- HF22 whole IL re-read reproduces archived SHA `dab15e70a4769b3f9c8af6b8dab91400151608df0625a70256f59c752f9be924`; HF23 whole IL SHA is `7ea1f0c41035d87165b72ee462828a8c5b1c176e42db608c4f3dd72dcb54aa51`;
- MethodDef count `2317 -> 2317`; normalized non-method skeleton byte-identical; exactly the nine declared HF23 MethodDefs change;
- semantic diff SHA-256 `bf0d7505ebc82ca185290ef81b524ffe4946b6a96ad2f33919b41ed8f8af3b1b`;
- semantic normalizer was first verified by reproducing the accepted HF21->HF22 diff SHA `1adaabb8c214895f9244b85b1fcd620e42c67aa1b7e4bab0bb86165b2c528e7c`.

HF23 Drive archive is accepted:

- folder `1nvim2033w3T_GnmaDHlSf8rVyF6CT-0K`;
- cumulative audited DLL `1qcGGHQZuVe5s60jje-dIJFzHVH-gkD7M`;
- patcher `1FqOSA3Zuh_DXZkL4NW8TfIVJ2Y5L_bby`;
- published RecoveryAudit `1GsLxEckfzbm09KBpo1M7dX2Hk5umFmTU`;
- fixed ILSpy bundle `1y4_qQ920mNoQVX8ckKln4zRk2dKySPCM`;
- native evidence `1RnnZo0HCw1Al3c5dKKRhZWw1c5TNOepi`;
- patch source `1LIEAv9G1leSuP63UzA22AiLznBat06mY`;
- semantic diff `1B8xkLMLHCOIiRT3_haLynEBD5aAX9Yjo`;
- semantic isolation `1LmhFhaL9gQDmdznFnr-lqszXpjLtRkXX`;
- RecoveryAudit log `1mUGfRynao8RJWIVIyAzlCyoitCoXw-VH`;
- authoritative Evidence-FINAL `1rp6hKit37kn72uBwNfW2DwNwhmYUz_hL`, SHA-256 `6f5deaaa2229873e77369de79b39a60fd3668df3c8f8ac473c2105d384b0e81b`;
- authoritative SHA256SUMS-FINAL `10FF_4PdwqG4VsmuSyPFVfRyRKnnac_7I`, SHA-256 `913665935e7781e44bb43d8c45d999ba5d783e3073df03df3aad6b6324f73e75`.

Provider final readback has no next page and verifies exactly **22 files = 20 payloads + 2 closure files**.

GitHub Evidence: `Tools/HighFidelityPatch/Evidence/HF23-Zombie-Hurt-Core.md`.

**HF23 formal acceptance: PASS. HF23 is now the only allowed formal input for any later cumulative HF stage.**

## Unity reconstruction state

AssetRipper ~3,149 objects; reconstructed project ~6,783 files. 173 game script types and 263 refs across 114 assets migrated. MainMenu/Board scenes restored. Fixed package set: UGUI 1.0.0, TMP 3.0.6, Core/URP 14.0.11, 2D Animation 9.1.1, Tilemap Extras 3.1.2, Burst 1.8.17, Collections 1.2.4, Mathematics 1.2.6, Visual Scripting 1.9.4. 67 package script types still require 67/67 Unity validation.

## Current decision gate — do not auto-open HF24

HF23 closes the direct Zombie damage-dispatch blocker. Do **not** create HF24 merely because additional Cpp2IL warnings exist.

Next run the remaining managed-damage / active-path decision scan over the HF23 final, using original PC native attribution to distinguish true active-path damage from shared-stub fake centrality. Specifically check Cpp2IL helper calls, unknown IL, non-empty-stack warnings, unmanaged-memory placeholders, generic mis-binding, invalid enum conversion, wrong float-bit interpretation, and direct-call centrality.

A further HF stage is allowed only when both conditions hold:

1. concrete managed loss/mis-reconstruction is proven from native/metadata evidence; and
2. the affected method is materially active in the gameplay path.

If no such blocker remains, stop HF recovery and proceed to Unity `2022.3.44f1c1`, package restoration, 67/67 package-script validation, recovered Assembly-CSharp integration, compile/scene/gameplay validation, then necessary iOS platform adaptation.
