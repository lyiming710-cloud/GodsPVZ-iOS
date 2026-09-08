# GodsPVZ 1.0.2 High-Fidelity Recovery Provenance — 2026-09-08

## Final accepted outputs

- HF1 `Assembly-CSharp.dll`: `31e6f6a781c6350a99a7317cade6cebc11cacd562a4c129961eef2d53af1d471`
- HF2 audited `Assembly-CSharp.dll`: `a21c9e1d1e6994eb559694a51123d33b8a1eb3b262bda6d372f97bbf51523633`
- The earlier HF2 `d445395f16b057cacfd6fac72cf84468ac987f314068e72d44f3da6df32c91ce` is superseded and must not be used.

## Original inputs
- `GodsPVZ_1.0.2.zip` — `2f08b4e2243e2bd296f3db694b4a2c9656252d76202c80167bdf3340b9f65b48`
- `GodsPVZ_1.0.2_Android.apk` — `428e0ba2e46645a905fb9fbb00cfde42406727a3ddfce9a7df1e0875889a739f`

## Cpp2IL reproducible baseline
- Version: `2022.1.0-development.1736+5fb2030.5fb20304df698ffd3d0e664b2a698cd911dc9d57`
- PC: 2318/2319; only failed method: `Zombie::InjuryStatusUpdate_Body (stack state not settling)`
- PC recovered DLL: `dc3911696e97ed707f204e2cf4920d97f26a4a64c376798d3a3aafee8e618ae9`
- Android: 2319/2319; recovered DLL: `8efbd210da1bf70e9277dc8a938d6ab784a5e06cc2c90866eacd606591afe95d`

## Recovery chain SHA-256
- `Cpp2IL-PC` — `dc3911696e97ed707f204e2cf4920d97f26a4a64c376798d3a3aafee8e618ae9`
- `Cpp2IL-Android` — `8efbd210da1bf70e9277dc8a938d6ab784a5e06cc2c90866eacd606591afe95d`
- `01_cecil.dll` — `b839cb09e9685c54f15b07218b21a10316bb0129fc8c0a65789528a804d94c24`
- `02_semantic.dll` — `723556a1591511b7b399cb81b89e70bead81e5ce2f3a1569509c7e831b0cae48`
- `03_savecore.dll` — `beb59ed7e64bb86f70ba06cae1b53f099afb60da1ecd6535568463a22bbae5f5`
- `04_datacore.dll` — `9c971719e64a0224192f37c6dbb88fcc712afbe17d686ac248a5d53d63bc416a`
- `05_bootpath.dll` — `53778186197d6ec56ec24e77e4df28fe432c944aa656a35b84d59b1e84f6f0f0`
- `06_runtime.dll` — `8880ab337c150d4514caaf55da793a7bae4135dd4a9417bff33ed03520dac3ca`
- `07_mainmenu.dll` — `aeeba2b765c74ec90582fc97b61efcfbac5dd1571719301e2ff7aa84be78cfbb`
- `08_stage8.dll` — `b08c837cf49f34e5bcecc133b93d2c69487df95cfa8bf2763de56152bce3428c`
- `09_stage9.dll` — `0dc5a56a97674cef6a5b77e4f04d68377415918c7888d9011d3dac4fa9b3f70f`
- `10_hf1.dll` — `31e6f6a781c6350a99a7317cade6cebc11cacd562a4c129961eef2d53af1d471`
- `11_hf2_final.dll` — `a21c9e1d1e6994eb559694a51123d33b8a1eb3b262bda6d372f97bbf51523633`

## Independent acceptance checks
- HF2 patcher: 19 internal method-body checks passed.
- Mono.Cecil 0.11.6: final HF2 reopened twice; all audited Plant/MouseManager/Card method bodies parsed; `RECOVERY_AUDIT_OK`.
- ILSpyCmd 11.0.0.9375: `Plant.Start`, `Plant.Start_Characteristic`, `Plant.LoopAddAnimation`, and `Plant.Update` decompiled independently with zero stderr and no relevant decompiler anomaly markers.
- ILSpy initially found two issues missed by patcher self-check: wrong `Animator.SetBool` overload selection and an open-generic `SpriteRenderer` local. Both were fixed before this final HF2.
- PC native `Plant.Start_Characteristic` at `0x18035A160` confirms the ID 4 Animator call passes the string literal `"prepared"` plus `true`; final HF2 explicitly resolves `SetBool(string,bool)`.

## Confidence / remaining fidelity notes
- **native_evidence**: Exact where mapped/disassembled
- **Plant.Start**: Behavior-equivalent CIL: indexed List loops remain instead of native Enumerator/finally shape; gameplay binding semantics match observed native flow.
- **Plant.Start_Characteristic**: Strong/Exact for recovered ID 4/5/6/49 branches after overload/type correction and ILSpy validation.
- **Plant.LoopAddAnimation**: Strong: recursive Transform enumeration and SpriteRenderer collection independently decompile cleanly; native flow matched.
- **Plant.Update**: Strong: native-backed control flow independently decompiles cleanly; known behavior-equivalent difference remains one shadow position read vs native separate position reads.

## Key successful GitHub Actions provenance
- MainMenu reference recovery: commit `4b86d04887b95f7f9ec3a479db9726345afea7a7`, run `34170772849`
- Stage 8 current-source recovery build: commit `b8489261e326efcb5b6233a4cd3e72aae0c95773`, run `34170855350`
- Stage 9 reference recovery: commit `e0b756cb30e4d2cb037d51f907145490a9ab1d3d`, run `34171163031`
- Independent Cecil + ILSpy audit tools: commit `2b8e34b2117e086140e7cb38d20a9ba2ce87dcfc`, run `34173085137`
- Final HF2 patcher with exact SetBool overload and concrete SpriteRenderer local: commit `740fc3f48e1432b2d9f91a39af71476bd8a16839`, run `34173430598`

## Google Drive archive

The final accepted binaries, recovery patchers, audit logs, ILSpy readback snippets, JSON provenance manifest and `SHA256SUMS.txt` are archived under:

`PVZ GOD/HighFidelity-Recovery-2026-09-08`

The JSON manifest in that archive is the machine-readable source of truth for artifact hashes.