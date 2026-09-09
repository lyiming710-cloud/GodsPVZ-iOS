# HF25 — Projectile Collision Dispatch / Ground native recovery evidence

Date: 2026-09-09  
Branch: `high-fidelity`  
Classification: **Native-backed managed-observable recovery of Projectile collision dispatch and ground-hit routing**

## Formal result

Formal HF24 input SHA-256:

`bdf1c0802685fa56a43602c7a8e45a30b48593b2903e99715a260473f5a09577`

HF25 cumulative candidate/final payload SHA-256:

`eb0823deff8289d152af52ab89b91672b64bb786701d31d4871ebfcdbe9f9b71`

HF25 restores exactly two MethodDefs and no others:

1. `Projectile.CollisionDetect()` — `0x060003E0` — RID `992` — PC `0x18037A2D0`, logical native end `0x18037A410`.
2. `Projectile.CollisionDetect_Ground()` — `0x060003E2` — RID `994` — PC `0x180379930`, logical native end `0x1803799B0`.

`CollisionDetect_Device`, `CollisionDetect_Plant`, `CollisionDetect_Zombie`, the collision resolution handlers, `Collision_AudioParticle`, `Aim`, `Update_Tracking`, and `SetEulerAngles` are explicitly outside HF25 and remain later decision-gate candidates.

## Why HF25 is required

HF24 restored `Projectile.Update`, which directly calls `CollisionDetect()` on the active per-frame projectile path. The post-HF24 decision scan then proved independent native-backed managed loss in the collision dispatcher itself, rather than relying on shared-stub xref counts.

The damaged managed `CollisionDetect` reused mis-reconstructed boolean/object temporaries in its `hitType` routing, changing target/camp dispatch semantics. `CollisionDetect_Ground` also retained invalid object/numeric comparison reconstruction on the hit threshold. These are active gameplay-path errors between projectile movement and the already recovered damage pipeline, satisfying both required gates: concrete native-vs-managed loss and material active-path importance.

## Original attribution and metadata closure

Attribution was re-derived from the original Assembly-CSharp CodeGenModule with `original MethodDef RID - 1 -> methodPointers index`:

- RID 992 / index 991 -> `0x18037A2D0` `Projectile.CollisionDetect`;
- RID 994 / index 993 -> `0x180379930` `Projectile.CollisionDetect_Ground`.

Original metadata identifies Projectile TypeDef index `5212`. Relevant original field offsets are:

- `ID +0x24`;
- `camp +0x28`;
- `damage +0x30`;
- `fZ +0x4C`;
- `fZ_shadow +0x50`;
- `hitType +0x70`.

The archived native-evidence bundle contains the original codegen attribution sequence, metadata field-offset reconstruction, both PC disassemblies and the raw `240f` ground constant.

## Native-observable behavior restored

`CollisionDetect` is the original four-way `hitType` dispatcher:

- `hitType == 0`: ground detection only;
- `hitType == 1`: opposite-camp detector routing followed by ground fallback;
- `hitType == 2`: same-camp detector routing under the native camp gates followed by ground fallback; unsupported camp returns;
- `hitType == 3`: first opposite-camp detector routing, then same-camp detector routing, then ground fallback;
- all other values return.

Each detector chain preserves native short-circuit ordering rather than evaluating every detector unconditionally.

`CollisionDetect_Ground` uses threshold `240f` only for projectile IDs 29 and 30; all other IDs use `0f`. The PC body performs `COMISS threshold,diff` followed by `JB return`, where `diff = fZ - fZ_shadow`. Consequently the unordered/NaN case also returns. The final recovered managed body expresses that behavior as `if (!(threshold >= diff)) return;`; a prior candidate using ordinary `<` was rejected because it did not preserve unordered semantics.

On a ground hit, `Damage.AreaDamage()` is called only for IDs `9-11`, `15`, and `26-32`. The hit then always calls `Collision_AudioParticle()` and `DestroyProjectile()`.

## Scope discipline and rejected intermediate candidate

HF25 intentionally stops at dispatch + ground behavior. Direct-xref review showed that Plant resolution is partly inlined, Device has only selected direct resolution calls, and `Collision_Plant` has no direct xref from the relevant detector body. Therefore adjacency in the MethodDef table was not treated as sufficient evidence to widen this stage.

The first compiled HF25 template produced a deterministic intermediate candidate, but a subsequent instruction-level review identified the `COMISS/JB` unordered/NaN mismatch described above. That candidate was explicitly rejected before formal acceptance and was not used as a cumulative input. The final template was rebuilt and all formal gates were rerun from accepted HF24.

## Patcher and repeatability

HF25 patcher source is under `Tools/HF25Patch/`.

- `Program.cs` Git blob `c96f18ed69d76859ccd28df1d26ea8db919f585a`
- final `Template.cs` Git blob `4703439207e7c22b6d89572b5d7fc9ea659a098d`
- `HF25Patch.csproj` Git blob `1e40833410b567b19d08b745665f388dfc5ecaa8`
- build workflow Git blob `4c6a25790beb2b75a676a2c4462fc8c755cec707`
- final patcher source head `37cc9c3cc82e3fb3244a70747219cb4fe4cc4257`
- final patcher workflow run `34309550102` — success
- final patcher artifact SHA-256 `9e4c863c1b3e5d6d01b297b05561e82d15657d864f440579c9cfe3680a39f2a9`

The patcher hard-rejects any input not hashing to accepted HF24. Formal HF24 was re-fetched from Drive ID `1UIzsJRqKwT0R-xCSaWqCG6CtenvPj2he` and independently re-hashed before the final runs.

Two independent applications of the final patcher to that same formal input produce byte-identical HF25 output:

`eb0823deff8289d152af52ab89b91672b64bb786701d31d4871ebfcdbe9f9b71`

Cecil reopen for both runs:

- `0x060003E0 Projectile.CollisionDetect` — `105 IL / 245 bytes / 0 EH`;
- `0x060003E2 Projectile.CollisionDetect_Ground` — `49 IL / 125 bytes / 0 EH`.

Neither target retains a Cpp2IL helper after reopen.

## Independent RecoveryAudit

Permanent `Tools/RecoveryAudit/` was extended to the final HF25 body measurements in commit `529f457a30dcc4f7e4d9c330fa7cc74e403ea75a`; resulting `Program.cs` blob `e4780fb52131ad1c1c34f002a0cd8d976926c274`.

RecoveryAudit workflow run `34309694411` succeeded. Published artifact SHA-256:

`565d69d2cd816cd552303eff3c521e525687b5d26eb8ec6125aa12c72f4ec769`

The published auditor was run on final HF25. OPEN1 and OPEN2 each report `320 types / 2317 methods / 2297 bodies`; E0 is `105 IL / 245 bytes` and E2 is `49 IL / 125 bytes` on both opens. The audit ends `RECOVERY_AUDIT_OK`.

## Fixed ILSpy / whole-assembly isolation

ILSpyCmd / ICSharpCode.Decompiler remain fixed at `11.0.0.9375`. References remain the reproduced 56-DLL fixed-Cpp2IL set from source commit `5fb20304df698ffd3d0e664b2a698cd911dc9d57`, reference ZIP SHA-256 `fe091e5a5389c2491eebeff72c83c394097bef67ffd45a25cf7de01c9aad9ef1`.

Both member readbacks exit 0 with stderr 0 and contain zero Cpp2IL references / decompiler issue markers. Whole HF24/HF25 readback stderr is 0. Re-decompiling HF24 in the exact HF25 environment reproduces the accepted HF24 whole-IL SHA exactly.

- HF24 whole IL SHA-256 `32e11c3eb33f19b357f34ef22f0dfb96eb91181cd4ff0f5d50ecf742ffd18d21`
- HF25 whole IL SHA-256 `9fc5f9d317d6469ba8e24ad09ff22e8480e562e81b5595f1e4ae3d1a50f10722`
- MethodDef count `2317 -> 2317`
- normalized non-method skeleton byte-identical
- exactly two declared MethodDefs change: `0x060003E0` and `0x060003E2`
- semantic diff SHA-256 `e64591dafd6297de6c3193683eef80078cc27490a849ce92206250f450cea2a2`

## Drive pre-closure acceptance

Archive folder `HF25-Projectile-Collision-Dispatch-Ground`, ID `1DfwwokTD0k8_-J2Z5virKxrHocc9Pszf`.

Key payload artifacts:

- cumulative audited DLL `1ENIzUe4qD4qdSBGXACyzQ2CmMIwja2i5`
- patcher `1jgkTsMxW4hzA8RWJnQ6NChywVELofePR`
- published RecoveryAudit `1g9k62ovG6p7WqdkrIcJKteEnR8ChEia4`
- fixed ILSpy bundle `1IkJOZR2RmqSNBPKfEkp8CglBO65V99IL`
- native evidence `1-Y6_ef9PiMhmDL1wj9nK1U7f6XlfMCyN`
- patch source `1jXupJAr18EHQNJPrBrxPKdMrjZhd_gJF`
- semantic diff `1adHN8Dk4QhgiH0BnIPsCIpCOd76ljblX`
- semantic isolation `1_JGXxwQgit-_1tiC64P4Cqx8sinLkJ0b`
- MethodDef table `12CskzYPviPBWWlgMKIBrgBxrcOUl2dE3`
- RecoveryAudit log `1Yl5XVgWANf_PMuHCq4gp4-hLehizKm-R`
- payload SHA manifest `1roFxlfQPAA4XEiT9zjPmAzC56hkxQolV`, SHA-256 `b3d9636c802d12f598f644c0d1a929acb4ecd8272c498f188efa2c9ef6bec8be`

Provider pre-closure readback has no next page and verifies exactly **20 payload files**.

Closure IDs/hashes are appended after final Drive closure.

## Next decision gate

Do not automatically open HF26. After formal HF25 acceptance, rerun the active-path decision gate on the three target detectors and their true direct business dependencies. Adjacency or Cpp2IL warnings alone do not justify a stage. A later HF stage requires both concrete native-backed managed loss and active-path importance; otherwise stop HF recovery and move to Unity/package validation.
