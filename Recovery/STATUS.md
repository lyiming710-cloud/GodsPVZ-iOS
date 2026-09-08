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
- **HF8 `Zombie.SetrSpeed` `4dc9b2486ae41b2d9c340293dada22e74ee6877290e0e79f456b770b92bb4688`.**
- HF9 `Zombie.Update_Attack` `c3e1716bbd598a8b2d2af3b1b5907869cc581271645ccc2a9e81595dfac07ec2`.
- HF10 `0684c52733afd5dd1d45e455ede8ec5355d8b53bb65875506cb97f9a29f07b8a`.
- HF11 `c990a8be0a615cbd1aed15b08251ae892889c056badc3e4070d7c6c24a6cb07f`.
- HF12 `963f24a8323025e424c2e392d4f90ab1d7d48a8f5aa0b671775ac64402788930`.
- HF13 `a389fcf0f6a97b4ace5cc580fb9704fa9830dbeba9b50edc4c5cad90489ebac3`.
- HF14 `cbe30c99a973f41bbf4bb0f0f56f62d37e1c752c0723159cc07feeb3d2a0efe7`.
- HF15 `87c74aea233372a5a7af28c0df244a18873af111c0230b54d9acf82e68ef9ec1`.
- HF16 `57100e6b9296f5a6c1ff4710cc2859810dcfff9992a27ea5238000729c3888c3`.
- HF17 `0eb0eba10cb27c5cff61e1a75947f146f5213ec036ff2ca3f95cf7d406ff1a67`.
- **HF18 Buff infrastructure — Exact managed-observable — `0e1b1acffa3329b3f98d2b61fb346c3f6647bee84e38bfab1d067a7aa17d58ed`.**

HF18 restores `Buff.ComputingIncrement`, `Buff::.ctor`, `BuffManager.GetIncrement`, `EndAll<T>`, `FindBuff`, `FindStatsIncreased`, and `BuffManager::.ctor`. Patcher run `34222051988` passed; permanent audit run `34222352921` passed OPEN1/OPEN2; ILSpy 11 full-ref readback and whole-assembly stderr are 0; semantic diff `40466bcb749014bc2415e4a5a4345b5c551c106af8ceff0fe067a42ffb9459ef` changes exactly seven MethodDefs.

HF18 formal Drive archive: folder `1hdSQPuV42nk6wx3knep8y8mqM3z8dVtV`, final DLL `1gXxdtyNf8w3vtOGWzu2vlWmsmN_ZH9G4`, patcher `1ZwQcIhHdO9w_Pv6-UyccqU_J3bUPezxJ`, SHA256SUMS `1NSohvFWm0Y5D2nIc5ZVZ7DGDtLEc42E3`; independently listed 29 files. A later empty duplicate folder is not provenance.

## Unity reconstruction state

AssetRipper ~3,149 objects; reconstructed project ~6,783 files. 173 game script types and 263 refs across 114 assets migrated. MainMenu/Board scenes restored. Fixed package set remains UGUI 1.0.0, TMP 3.0.6, Core/URP 14.0.11, 2D Animation 9.1.1, Tilemap Extras 3.1.2, Burst 1.8.17, Collections 1.2.4, Mathematics 1.2.6, Visual Scripting 1.9.4. 67 package script types still need 67/67 Unity validation.

## Current next gate

HF18->HF19 active-path rescan is in progress. After suppressing shared-pointer false centrality, the leading unique damaged active predicate is `Zombie.IsDisabled` (35 native incoming sites). Its dependency `Zombie.IsPlantZombie` and adjacent `Zombie.IsNormalZombie` both contain lost native jump tables. These three are being audited as one classification-predicate cluster. If this cluster closes cleanly, it is the current HF19 candidate. After that, rescan again; if no critical damaged active-path cluster remains, stop adding HF and move to Unity import/IL2CPP validation.

High-fidelity-first remains mandatory: native evidence -> formal input -> CI -> reopen -> permanent Cecil -> full-ref ILSpy -> semantic isolation -> Evidence/STATUS -> Drive archive -> fixed SHA.