# HF24 — Projectile Runtime Core native recovery evidence

Date: 2026-09-09  
Branch: `high-fidelity`  
Classification: **Native-backed managed-observable recovery of the active Projectile.Update runtime path**

## Formal result

Formal HF23 input SHA-256:

`35d13b0d7fc5f82e04b837b812fab501a933b00e21f3431b2f6759c8e0c2b39c`

HF24 cumulative candidate/final payload SHA-256:

`bdf1c0802685fa56a43602c7a8e45a30b48593b2903e99715a260473f5a09577`

HF24 restores exactly one MethodDef and no others:

1. `Projectile.Update()` — `0x060003D2` — RID `978` — PC `0x18037D710`, logical native function end `0x18037DF0C`.

Collision detectors are explicitly excluded from HF24 and remain a later active-path decision gate.

## Why HF24 is required

HF23 closed the direct Zombie Hurt chain, then the mandated remaining managed-damage / active-path decision scan was rerun. Shared-stub fake centrality was rejected: the high-xref 16-byte body at `0x1802FFCB0` was not used to justify a new stage.

`Projectile.Update`, however, is a Unity per-frame lifecycle method with its own substantial PC native body and direct calls into projectile movement/collision. The HF23 managed body retained concrete Cpp2IL mis-reconstruction on that active path, including the native float-to-int sorting-order conversion, movement-track branching and float/Vector3 integration. That satisfies both required gates: proven native-vs-managed loss and material active-path importance.

## Native-observable behavior restored

The native body preserves creation/initialization of the projectile `SortingGroup`, sorting layer `"Entity"`, and sorting order derived from `fY` using native float multiplication followed by truncating float-to-int conversion (`cvttss2si`) before the `-10 - value` adjustment.

Runtime progression remains gated by non-null `board`, `board.gameStart`, and `!board.gamePause`. The original direct-call graph includes `Update_Time`, `Update_Tracking`, `Update_MoveTrack7`, `TestOutofMap`, `SetEulerAngles`, `Moving_SetNewPosition`, `DestroyProjectile`, `CollisionDetect`, and `Rotating`.

Movement-track behavior restored in this stage includes:

- track 3 -> `Update_Tracking`;
- track 4 -> after `livingTime > 1.5`, zero zSpeed, set X speed to 1500, update tracking, then set track 0; otherwise use the original `log(10)` / `95-fZ` trajectory formula and update tracking;
- track 5 -> when `fZ >= 1200` or out-of-map, set Euler angles `(0,-90)`, zero 3D speed, set zSpeed `-3000`, zAcceleration 0, snap X/Y to `targetPosition`, and set fZ 1190;
- track 6 -> zSpeed `(fZ - 75) * -5.2`;
- track 7 -> `Update_MoveTrack7`.

Per-axis motion uses repeated `Time.deltaTime` observations in the native order for fX/fY, acceleration into speed, fZ, and zAcceleration into zSpeed. `Moving_SetNewPosition` follows the integration. Shadow local scale is recomputed from fW/fD with the native `1.425 / 40` relationship, shadow Y is synchronized to `fZ_shadow`, out-of-map destroys the projectile, then `CollisionDetect()` is invoked on the active frame.

Outside the board-active block, an existing track follows `projectileSprite`; `Rotating()` still runs; LightSaber projectiles with null `origin_Plant` self-destroy; `previousPosition` is updated from fX/fY and shadow world Y minus fY.

## Scope discipline

HF24 intentionally does **not** repair `Projectile.CollisionDetect_Device`, `Projectile.CollisionDetect_Plant`, `Projectile.CollisionDetect_Zombie`, `Projectile.Collision_AudioParticle`, `Projectile.Collision_Zombie`, `Projectile.Aim`, `Projectile.Update_Tracking`, or `Projectile.SetEulerAngles`. Those remain separate evidence/decision candidates. HF24 changes only the lifecycle dispatcher/runtime method proven active and damaged.

## Patcher and repeatability

HF24 patcher source is under `Tools/HF24Patch/`.

- `Program.cs` Git blob `1f4975ab031bee3f8964d1edb3b5e78a93e1079b`
- `Template.cs` Git blob `77285a5500ea6cd6bf673d953c23d8981533cc76`
- `HF24Patch.csproj` Git blob `5f60b56da2e37ec95680297bcb4b48bd8bbea40b`
- final patcher source head `7c4da1c0c14d916fc9a526f526298f9bd44d8c2d`
- final patcher workflow run `34306586359` — success
- final patcher artifact SHA-256 `fc7a62398efc3c3a9b3eddfc6cda43ba517f7938699904571a19fb86c0bb38be`

The patcher hard-rejects any input not hashing to accepted HF23. The formal HF23 DLL was re-fetched from Drive ID `1qcGGHQZuVe5s60jje-dIJFzHVH-gkD7M` and re-hashed before patching.

Two earlier tool-only attempts did not produce candidates and were not propagated:

- workflow `34306195447`: single-file publish made `Assembly.Location` unsuitable for reading the template assembly; stopped before output;
- workflow `34306382755`: nested `MethodSpec` mapping was rejected by Cecil during write; stopped before output.

The mapper was corrected without changing the native-backed template semantics. Two independent applications of the final patcher to the same re-fetched HF23 formal input produced byte-identical output SHA `bdf1c0802685fa56a43602c7a8e45a30b48593b2903e99715a260473f5a09577`.

Cecil reopen for both formal runs:

- `Projectile.Update` — `335 IL / 1047 bytes / 0 EH`.

No Cpp2IL helper remains in the target after reopen.

## Independent RecoveryAudit

Permanent `Tools/RecoveryAudit/` was extended through HF24 in commit `2470b3872e7be15b1fd662cdcae430b481b1fc20`; resulting `Program.cs` blob `e1823f55fbc59eee0e79bf1f18fbd07ebb87ce5d`.

RecoveryAudit workflow run `34306849398` succeeded. Published artifact SHA-256:

`bdfb83c9ce5dd5d74db4ec8988fc75cb1f666cab31e15c2a4fc73637452bffdd`

The published auditor was run on HF24. OPEN1 and OPEN2 each report `320 types / 2317 methods / 2297 bodies`; `Projectile.Update` is `335 IL / 1047 bytes` on both opens, and the run ends `RECOVERY_AUDIT_OK`.

## Fixed ILSpy / whole-assembly isolation

ILSpyCmd / ICSharpCode.Decompiler remain fixed at `11.0.0.9375`. References remain the reproduced 56-DLL fixed-Cpp2IL set from source commit `5fb20304df698ffd3d0e664b2a698cd911dc9d57`, reference ZIP SHA-256 `fe091e5a5389c2491eebeff72c83c394097bef67ffd45a25cf7de01c9aad9ef1`.

`Projectile.Update` member readback exits 0 with stderr 0, contains zero Cpp2IL references and zero decompiler issue markers. Whole HF23/HF24 readback stderr is 0.

- HF23 whole IL SHA-256 `7ea1f0c41035d87165b72ee462828a8c5b1c176e42db608c4f3dd72dcb54aa51`
- HF24 whole IL SHA-256 `32e11c3eb33f19b357f34ef22f0dfb96eb91181cd4ff0f5d50ecf742ffd18d21`
- MethodDef count `2317 -> 2317`
- normalized non-method skeleton byte-identical
- exactly one declared MethodDef changes: `0x060003D2 Projectile.Update`
- semantic diff SHA-256 `ac64cb323d71f4b8a2e7f8aab93da05aed6fa75de6627cd14963b9d976665d0b`

## Drive pre-closure acceptance

Archive folder `HF24-Projectile-Runtime-Core`, ID `1jWriDX2NiJsO2hWhLwnebVtL4d2j5N37`.

Key payload artifacts:

- cumulative audited DLL `1UIzsJRqKwT0R-xCSaWqCG6CtenvPj2he`
- patcher `14j-vZPT1pbMx9GDOao1OUuijVA4tQjZj`
- published RecoveryAudit `12n8Q5LfxPI4c_vHcQlPmPr-EBF4VD2Mk`
- fixed ILSpy bundle `1UkLFLjtwjzuI8bg2HoekYSk7a1fl1xIR`
- native evidence `15hgH1xBLjhxAtNFbJV2eGykwlBlMCvpz`
- patch source `1otAgdL6uKWC-PTKE7jb0dkmqGS9Bt2n4`
- semantic diff `1G35WUYlxsh0CrAKTV7lRrxV0cZMlbE-7`
- semantic isolation `153b0_cW_caCxfsUEjsALFdJhsBbaWam_`
- MethodDef table `1djCIKX1gReUwDm7Ma5t-J73bqDxEBGZj`
- RecoveryAudit log `1oqk2t-J8wm72E-EdDzCHIvKNOkcOTRny`
- payload SHA manifest `1sWwMxyt1k3CGv_pR2nmgrrlOaYLcRjNt`

Provider pre-closure readback has no next page and verifies exactly **20 payload files**.

Closure IDs/hashes are appended after final Drive closure.

## Next decision gate

Do not automatically widen HF24 into collision recovery. After formal HF24 acceptance, rerun the active-path decision gate on the collision/aim helper cluster using original PC native evidence. A later stage is justified only for methods that independently satisfy both concrete managed-loss evidence and active-path importance.
