# HF35 — Device Injury Status Core native recovery evidence

Date: 2026-09-10
Branch: `high-fidelity`
Classification: **Exact for managed-observable behavior of the declared MethodDef**

## Formal scope

HF35 restores exactly one MethodDef:

- `0x0600032B Device.InjuryStatusUpdate()` — original MethodDef RID `811`, PC methodPointer `0x180349280`.

Formal HF34 input SHA-256:

`ead8dee5def2818ca8c64c36b4fe65f1830dbf873fae34dbc84d2f07b103d999`

HF35 cumulative candidate / closure SHA-256:

`ffd3e858a6ea15c1203a8c543e800b9181e0ab115fa7ab6a84d00975be5b947b`

No other MethodDef is modified in HF35.

## Original native attribution and active path

Attribution uses the fixed rule `original MethodDef RID-1 -> Assembly-CSharp CodeGenModule methodPointers[index]` against the original PC release.

Original fixed hashes were re-verified before closure:

- PC ZIP: `2f08b4e2243e2bd296f3db694b4a2c9656252d76202c80167bdf3340b9f65b48`
- PC `GameAssembly.dll`: `9ebd7ca996a5b03fb4a766f7a2502b660d581ddbbb36af4d7bf2f06a211da39d`
- PC `global-metadata.dat`: `ad992341add498bd1018ac980f42171e0737b0793bf945c8714ad70a445b36b9`

The PC native body at `0x180349280` was disassembled directly. A direct original-PC callsite exists at `0x18034BD5A: call 0x180349280`. Independently, the already-formal HF22 Device damage recovery records `Device.TakeDamage` audio/particle processing followed by `InjuryStatusUpdate`, establishing material gameplay activity.

## Native-observable behavior restored

The original body preserves these semantics:

- The initial `COMISS 0,healthPoint` / `JAE` behavior calls `Broken()` only for ordered `healthPoint <= 0`. Unordered/NaN proceeds through the active body; the recovered C# therefore uses `!(healthPoint <= 0f)` rather than simplifying it to `healthPoint > 0f`.
- The active body returns unless `ID == 4`.
- `damagedFraction = 1f - healthPoint / maxHealthPoint`.
- While `damagedFraction > (brokenLevel + 1f) / 3f`, increment `brokenLevel`.
- Resolve animation sprite `"Roadblock"`, get its `SpriteRenderer`, and if Unity-truthy assign `ResourceManager.deviceSprites[brokenLevel]`.
- Native helper `0x1802FB100` was closed as the `ParticlesManager` singleton backing-field getter/shared thunk; the next call is `ParticlesManager.CreatNewParticle(26)`, i.e. `ParticleState.RoadblockBroken`.
- If the new particle is Unity-truthy, copy this Device transform position to the particle transform.
- Read `ResourceManager.particleClips[12]`.
- Use `Camera.main.transform.position` as the audio point.
- Call the already recovered four-argument `CreateAudioAtPoint` with `AudioVolume() * 1.6f` and pitch `0.7f`.
- Continue the thirds-threshold loop; ordered non-positive health invokes `Broken()`.

The pre-HF35 managed body contained concrete Cpp2IL / invalid stack-type / missing-call loss, so HF35 satisfies both the managed-loss and material-active-path decision gates.

## Patcher and deterministic formal application

Final HF35 patch source blobs:

- `Template.cs`: `1a4137739f10e7b268a9b71ed51e9bc3ab05c7df`
- `Program.cs`: `5fb1fcf77e6c89d406e99ee9e8a8fa20b7f85933`
- `HF35Patch.csproj`: `9479f52eb55be844a77646f01899c986f408fe19`
- `build-hf35-patcher.yml`: `f9c274b75496d11e6f141dd6cf48e887d0b38a98`

Final patcher build head: `85934bf0ca63b0faa9c1f59a96ed938293f5012c`.

Patcher workflow `34391370140` passed. Published artifact ID `10119789189`, ZIP SHA-256:

`fabd1010ee865a1c3021ddb37a457c24115d74c1ecddcdb0ba3c145c9cdf27dc`

An earlier tool-only application was rejected because the reopen floor was set to 80 while the recovered method has 79 IL instructions. No semantic template line changed; only the tool floor was corrected to 70 before the final published build.

Two independent applications of the final published patcher to the SHA-verified HF34 formal DLL were byte-identical at:

`ffd3e858a6ea15c1203a8c543e800b9181e0ab115fa7ab6a84d00975be5b947b`

Cecil reopen on both outputs: `79 IL / 244 bytes / 0 EH`, stderr 0.

## Independent fixed ILSpy and whole-assembly validation

Fixed ILSpyCmd / ICSharpCode.Decompiler is `11.0.0.9375`, with the reproduced 56-DLL reference ZIP SHA:

`fe091e5a5389c2491eebeff72c83c394097bef67ffd45a25cf7de01c9aad9ef1`

`Device.InjuryStatusUpdate()` member readback: exit 0, stderr 0, Cpp2IL refs 0, issue markers 0.

HF34 whole IL was independently reproduced at the already accepted SHA:

`96d4bd053040cb1531d3feb5f2e28d20cfd9f51ebc273d6a457127f312088599`

HF35 whole IL SHA-256:

`ca155db767d34a814ce322166216aa7c83a868e5d44743c25b71d2c120f22a9e`

Both whole-assembly reads have stderr 0.

Before calculating the new isolation, the same parser/normalization algorithm reproduced the already accepted HF33->HF34 semantic diff byte-for-byte at:

`27e250ef34624ad70c5345c770eefe52d7204a4452d7c861013e1a72dcc59697`

HF34->HF35 isolation result:

- MethodDef count `2317 -> 2317`;
- normalized non-method skeleton byte-identical;
- exactly one changed MethodDef: `0x0600032B Device::InjuryStatusUpdate`;
- semantic diff SHA-256 `9c0dacb91256ecadf658ae35faba6474ee5f3f269e7d0f4b2b35b3cfacc22a2e`.

## Permanent RecoveryAudit

Permanent RecoveryAudit was extended for `Device.InjuryStatusUpdate/0` in commit:

`11cb9d454071cf4162ed4813b0c376d120408bd1`

Workflow run `34417105324` passed. Published RecoveryAudit artifact ID `10129507233`, ZIP SHA-256:

`32a52ed6ccdd3cf29808e86da1324c474e8cf5f456f3caef32b7cc5d947bbe75`

The published auditor was independently run against the HF35 candidate. OPEN1 and OPEN2 both report `320 types / 2317 methods / 2297 bodies`; the HF35 target is `79 IL / 244 bytes` in both opens; stderr is zero and the run ends `RECOVERY_AUDIT_OK`.

## Drive pre-closure acceptance

HF35 Drive folder `HF35-Device-Injury-Status-Core`:

- folder ID `1ZqhsosBN7DZ2RBQcK_b6xJ_RRcu8QZgc`
- cumulative DLL `1mjP1TgepHjgjw-r2y-Y3K576nhQXOxCF`
- published patcher `1HggbixmDAu6dEtaQ8RfATv1QXi0dVTvN`
- fixed ILSpy `1W1vrOSdabizyKrE-Gf8GXKy7pAoGXkBY`
- published RecoveryAudit `13ejCFG8F13pt8VKrxJbJWJhm1JNxz1iM`
- semantic diff `1yY3mWdrpqdKFm-qjKIDGwKkF2PADcjJd`
- Cecil reopen `1jyiyG9t8pMIPQk5ZqIwgqgCU79109NZs`
- member summary `1ZpTBtPrJKj-Ag9fNsjIAHmqrw1vaE18s`
- member bundle `12MkjkGdNNcL8-m_putURHQpNvjUJlUt5`
- whole provenance `1_FiJZU6SWF_hIMv3F3Toldp45_LEAYII`
- MethodDef table `1Rh_Yz-BOi7ID1BLT28X-my7HJcGRQgT8`
- RecoveryAudit log `1qaDDvO2NTQfo_pwxKUIuuRJklWKsjljt`
- native evidence `1MPyUuuXRAUGe1zwpd39vf3hThgZ6gq-_`
- patch source `1mrDAxVdV4RbTU8uS8hq_LnFFG934lshI`
- formal run1 `1IGFv5vFWJDrfYBj737V5E9dKYC5i_hR9`
- formal run2 `17S2DI2R9x6t9zfYBi1e9eR1oUPZ2Y1ra`
- reference provenance `1dB1yxaIF8SFYz4TKbo3DCzA1yBPBMlcC`
- semantic isolation `1ry9Gm6aC_EbnlsoZOmbpLr51P7f5LHbE`
- source/build provenance `1RQvgSH1vhW1sBwzlDnCH4EGWxmd0-l_0`
- target manifest `1peOVZDE-EIBFXt16VmCY1n8Cygl3QQ_N`
- payload manifest `19WftCs0qhKBGrntxvhNKh2PtLuThnKtP`, SHA-256 `56fef5e8e4d82b8b7504bdffebaa8ffb01fe7b447c0d407aa976b8132b2518f0`.

Provider pre-closure readback returned exactly 20 files = 19 ordinary payloads + the payload manifest.

HF35 becomes FORMAL PASS only after the Evidence-FINAL and SHA256SUMS-FINAL closure pair is uploaded, the provider folder is independently re-listed as exactly 22 files, and only then `Recovery/STATUS.md` is advanced.