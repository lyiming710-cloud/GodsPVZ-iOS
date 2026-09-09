# HF27 — Projectile Collision Resolution Core native recovery evidence

Date: 2026-09-09  
Branch: `high-fidelity`  
Classification: **Native-backed projectile target-resolution recovery**  
Formal status: **PASS**

## Formal result

Formal HF26 input SHA-256:

`afa0e052902139c09cee9c715fe9a75c6f124af3af8a5647c799dc6b457fefac`

HF27 cumulative formal-final SHA-256:

`18d9efebdcfcccbbe6c805a9276c4427b4e6baa33673fb71b96a04245bb0e8de`

HF27 restores exactly two MethodDefs and no others:

- `Projectile.Collision_Device(Device)` — `0x060003E6` — RID `998` — PC `0x18037B010`;
- `Projectile.Collision_Zombie(Zombie)` — `0x060003EA` — RID `1002` — PC `0x18037B170`.

No detector MethodDef is modified in HF27. `Collision_AudioParticle` was already formally recovered by HF26.

## Why HF27 is required

The post-HF26 decision scan proved these two methods are direct business dependencies of active projectile target detectors and still contained concrete managed mis-reconstruction. HF26 E6 retained invalid int/object ID arithmetic; EA also lost the native tail jump to `DestroyProjectile` (`0x18037B200`). Both native bodies are compact and fully attributable, so they were recovered independently rather than expanding scope into the larger detector methods.

## Original PC attribution and native behavior

Original Assembly-CSharp MethodDef RID-1 to CodeGenModule method-pointer attribution was re-derived from the original PC image:

- E6 RID 998 -> `0x18037B010`;
- EA RID 1002 -> `0x18037B170`;
- post-processing dependency EB RID 1003 -> `0x18037A410`;
- destroy target EC RID 1004 -> `0x18037B200`.

Projectile fields used here are `ID +0x24` and `damage +0x30`.

Native E6 behavior:

- IDs `{10,11,15,26,27,28,29,30,31,32}` -> `damage.AreaDamage()`;
- otherwise -> `device.TakeDamage(damage,this)`;
- then `Collision_AudioParticle()`;
- ID 19/23 return without destroying the projectile;
- all other IDs tail-jump `DestroyProjectile()`.

Native EA behavior:

- IDs `{10,11,15,26,27,28,31,32}` -> `damage.AreaDamage()`;
- otherwise -> `zombie.TakeDamage(damage,this)`;
- then `Collision_AudioParticle()`;
- ID 19/23 return without destroying the projectile;
- all other IDs tail-jump `DestroyProjectile()`.

The archived native bundle contains both original PC bodies, attribution, the broken HF26 managed readbacks and the HF27 fixed readbacks.

## Patcher and deterministic formal application

HF27 patcher source is under `Tools/HF27Patch/`.

- `Program.cs` blob `4e47907291fdf75fc00ececfad2698854079a1f8`;
- `Template.cs` blob `bf7bfb54ae7044b057a93c2a73b2c2861e1c85f8`;
- `HF27Patch.csproj` blob `3f38ee9cf5306143169941590972662372c4372c`;
- workflow blob `24a357e7268de9079e182d6a8b365ca5cb28d440`;
- patcher source head `d0339e197f57f0d7e74f30a0b9909701b8845309`;
- workflow run `34323560548` PASS;
- patcher artifact SHA-256 `9fc93909009beafa088f17504fc0e78288bb2432be41d878bbb574a76ba38479`.

Formal HF26 was re-fetched from accepted Drive ID `1yc82VM4pK5DdIlu7-qS9VLdcGDsy-ti_` and independently re-hashed. Two independent applications are byte-identical at HF27 SHA `18d9efebdcfcccbbe6c805a9276c4427b4e6baa33673fb71b96a04245bb0e8de`.

Cecil reopen on both runs:

- E6 `43 IL / 110 bytes / 0 EH`;
- EA `55 IL / 140 bytes / 0 EH`;
- zero Cpp2IL helper refs in both declared bodies.

## Independent permanent RecoveryAudit

Permanent `Tools/RecoveryAudit/` was extended in commit `b4d96066aecc5a1fb5f29d13ff3380530b163d72`; Program blob `e2c66f82dced12bae6585e04ab999c25b0767e70`.

RecoveryAudit workflow run `34324427275` PASS. Published artifact SHA-256:

`17d60177a8b82bc3ecd162813bc9f414dc21798c0990ad5c1da74c6fc6155c61`

Published auditor on HF27:

- OPEN1: `320 types / 2317 methods / 2297 bodies`, E6 `43/110`, EA `55/140`;
- OPEN2: identical counts and measurements;
- final `RECOVERY_AUDIT_OK`.

## Fixed ILSpy / whole-assembly semantic isolation

ILSpyCmd / ICSharpCode.Decompiler remain fixed at `11.0.0.9375`; references remain the reproduced 56-DLL fixed-Cpp2IL set from Cpp2IL source commit `5fb20304df698ffd3d0e664b2a698cd911dc9d57`, reference ZIP SHA-256 `fe091e5a5389c2491eebeff72c83c394097bef67ffd45a25cf7de01c9aad9ef1`.

Both HF27 member readbacks exit 0 with stderr 0, zero Cpp2IL refs and zero issue markers. Whole HF26/HF27 stderr is 0, and HF26 whole IL is reproduced exactly in the HF27 environment.

- HF26 whole IL SHA-256 `4e3d8039a6a1a07e02b8e9d998be057e1eee95d4bdf139d91ac832dd144f1714`;
- HF27 whole IL SHA-256 `33260b8a10a7b51ccf19366420ef50bd3fd24c10dee91628db98107902f5b993`;
- MethodDef `2317 -> 2317`;
- normalized non-method skeleton byte-identical;
- exactly E6 and EA change;
- semantic diff SHA-256 `7238e4a4873917787a976c3e9255ec2bb11176591dcc940ffc01e2374c10676f`.

The semantic-normalization implementation was first checked by regenerating accepted HF25->HF26 semantic diff SHA `08c012c242a50c97d26fb773dddf9f48eabf541b07b465e4c1f4c560178de0f9` byte-for-byte.

## Drive formal acceptance

Archive folder `HF27-Projectile-Collision-Resolution-Core`, ID `13_2KR3QSuTAjthqiMyd3scOL-oOMOawN`.

Key artifacts:

- cumulative audited DLL `1eS4hAmbRZKOO6oVzS8Tj7n1arpbconP6`;
- patcher `1EBcpQCvSI3qi-xgiZczTkyS3HdkSjqFU`;
- published RecoveryAudit `1Q7faENFReWN8wmP307DIq1jdBtjRnWTM`;
- fixed ILSpy `1uEKA74lshuJqLO7mTLmasBLN-ANZ6zo5`;
- native evidence `1Je7MYXbmxs5L2N8NCt6JecBPcW5S9MlD`;
- patch source `1U3uB0M5vqejoMO9FVggWtFBIMxV2tP9v`;
- semantic diff `1ydlK6gbZnVb-2iE9B0AWOM6DaKYq5AVb`;
- semantic isolation `1eF-bzGA5LAM5IB7zpQw9bLf-V2x_HDqQ`;
- MethodDef table `1K3T_-KfRror810bpa3uBs7PhVePYzeap`;
- RecoveryAudit log `1uPgwkGWP7ejpGi0rfGLqTE3TdWbQh-fF`;
- payload manifest `1sFinNdM1CAxvpD0f2AxBdZIG0sGyhKk6`, SHA-256 `4b80595844a198abe372732fd433708224dd9502df8f2d9042d0a49c9f061351`;
- Evidence-FINAL `1H7HgEQBfg2QKZKTnH03mu2tYMu6OWKIA`, SHA-256 `05f883018b5031ff55d4da29b3d142b1d387977cb0db89f8ebdf27e7ef32d98f`;
- SHA256SUMS-FINAL `1CHAe372DCQGHft1VFEyHAARamw_eRuB6`, SHA-256 `332fb1fbc9d835cc8c5b427d4c3baec3d109ad411f44ed58fa235e58ae7e7691`.

The final checksum file contains exactly 21 entries: the 19 non-manifest payloads, the payload manifest itself, and Evidence-FINAL. Provider final readback has `has_more=false` and verifies exactly **22 files = 20 payloads + 2 closure files**. Temporary readback marker folders were removed and are not part of the archive.

## Formal acceptance

**HF27 formal acceptance: PASS. HF27 is the only allowed formal input for any later cumulative HF stage.**

## Next decision gate

Do not automatically open HF28. Rerun the active-path native-vs-managed scan on `CollisionDetect_Device`, `CollisionDetect_Plant`, and `CollisionDetect_Zombie`. Include no adjacent helper unless direct native business-path evidence and concrete managed loss both require it. If no remaining candidate passes both gates, stop HF recovery and move to Unity/package validation.
