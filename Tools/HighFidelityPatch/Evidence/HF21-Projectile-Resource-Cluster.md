# HF21 — Projectile / Resource cluster native recovery evidence

Date: 2026-09-08  
Branch: `high-fidelity`  
Classification: **Exact for managed-observable behavior**

## Formal result

Formal HF20 input SHA-256:

`b16b3b89fad8f8081d86611a732696c56db6bd2dbab6302b6cef7fa94e141b3e`

HF21 cumulative final SHA-256:

`888cab48b5e488ca05ed8ec58a11503c9f9a8ce0bac23a60c933f8628834e8f0`

HF21 restores exactly eight MethodDefs from the original PC x86-64 IL2CPP behavior:

1. `ParticlesManager.Start()` — RID 488, token `0x060001E8`, PC body `0x180322040`.
2. `ParticlesManager.CreatNewParticle(ParticleState)` — RID 490, token `0x060001EA`, PC body `0x180321F30`.
3. `ProjectileManager.Start()` — RID 499, token `0x060001F3`, PC body `0x1803248E0`.
4. `ProjectileManager.SetFloatScale()` — RID 500, token `0x060001F4`, PC body `0x1803242F0`.
5. `ProjectileManager.CrateNewProjectile(int)` — RID 502, token `0x060001F6`, PC body `0x180324150`.
6. `ResourceManager.Load_projectileSprite()` — RID 558, token `0x0600022E`, PC body `0x18033A520`.
7. `Projectile.ResetData()` — RID 989, token `0x060003DD`, PC body `0x18037BB60`.
8. `Projectile.BindTrack()` — RID 990, token `0x060003DE`, PC body `0x180378FC0`.

No unrelated MethodDef is included in HF21.

## Native-observable behavior

`ProjectileManager.Start()` restores initialization in the original order: `SetFloatScale`, resource list/prefab binding, then construction of a 1000-projectile inactive pool. Each instantiated Projectile is bound to the Board and Board ProjectileManager, parented under the manager Transform with `worldPositionStays=false`, deactivated, and appended to the pool.

`ProjectileManager.SetFloatScale()` restores exactly 54 float-array stores. The native table includes the original W/D/H values for projectile IDs 0/1, 9, 10/11, 15, 19, 20, 21, 23, 24/25, 26/31, 27, 28, 29, and 32; no synthetic defaults were introduced.

`ProjectileManager.CrateNewProjectile(int)` restores the pool scan. The first inactive Projectile executes `ResetData()`, `StartData(ID)`, is activated, and is returned. The `List<Projectile>.Enumerator` is protected by the original `finally`/`Dispose`; exhausting the pool throws `NullReferenceException`.

`Projectile.ResetData()` restores the complete reset state: `damage=null`, `ID=0`, `camp=Camp.none`, `scale=1`, `livingTime=0`, `updateRate=1`, position/size scalars reset with `fZ=65`, movement/hit state cleared, speed/acceleration vectors zeroed, z/angular movement cleared, Transform rotations reset, animation destroyed and nulled, and the projectile SpriteRenderer sprite cleared. Native 64-bit stores crossing adjacent managed fields were explicitly decomposed: `+0x24` clears `ID/camp`, `+0x38` encodes `scale=1/livingTime=0`, `+0x40` encodes `updateRate=1/fX=0`, `+0x4C` encodes `fZ=65/fZ_shadow=0`, `+0x6C` clears `movementTracks/hitType`, and `+0xA4` clears `zSpeed/zAcceleration`.

`Projectile.BindTrack()` restores the native ID routing: IDs 1/19/20 create `ParticleState.Track_Snowflakes` (37), ID 25 creates `ParticleState.Track_Bolt` (38), and all other IDs return. The new particle is placed at the projectile sprite position, parented to the projectile with `worldPositionStays=false`, and saved in `track`.

`ParticlesManager.Start()` restores enumeration of every `ParticleState`, `Enum.GetName`, resource loading from `prefabs/ParticleSystem/<name>`, dictionary insertion, and the original `"加载特效" + name + "失败"` Debug.Log path for missing resources. The enumerator `finally`/`Dispose` is retained.

`ParticlesManager.CreatNewParticle(ParticleState)` restores `TryGetValue`, Unity `Object.op_Implicit` truthiness for the cached prefab, `MonoBehaviour.print("该特效不存在")` plus null return on absence, and `Object.Instantiate` on success.

`ResourceManager.Load_projectileSprite()` restores enumeration of all `ProjectileType` values, resource paths under `sprites/Projectile/<name>`, the original `"加载子弹" + name + "的贴图失败"` log path, list growth with null entries until the enum index exists, indexed Sprite assignment, and the original enumerator `finally`/`Dispose`.

## Patcher and repeatability

Formal patch source SHA-256:

`f5b7085dea090b2ea97434fd317b8db5e5035a3ee241bc3ee311f0be6165715e`

Final normal-build trigger commit:

`515c5f78ddd5abe68b8176e1a05b367f5a1fcf5e`

Final patcher workflow run `34248270465` succeeded. Final downloaded patcher artifact ZIP SHA-256:

`4bf66a06db9387e2bf5a1d150c8f5ab25d8af6840d6ac672d63a1b9730bd4960`

During formal application, three verifier floors were calibrated to the actual emitted IL counts (`CrateNewProjectile` 36, `CreatNewParticle` 20, `Load_projectileSprite` 64). Each pre-calibration run stopped before writing a candidate; no failed output was retained or reused. The final calibrated patcher was applied twice independently to the same re-hashed formal HF20 input and produced byte-identical HF21 outputs.

Reopen measurements:

- `ProjectileManager.Start`: 54 IL / 188 bytes / 0 EH
- `ProjectileManager.SetFloatScale`: 271 IL / 919 bytes / 0 EH
- `ProjectileManager.CrateNewProjectile`: 36 IL / 140 bytes / 1 finally
- `Projectile.ResetData`: 101 IL / 360 bytes / 0 EH
- `Projectile.BindTrack`: 50 IL / 191 bytes / 0 EH
- `ParticlesManager.Start`: 58 IL / 243 bytes / 1 finally
- `ParticlesManager.CreatNewParticle`: 20 IL / 64 bytes / 0 EH
- `ResourceManager.Load_projectileSprite`: 64 IL / 279 bytes / 1 finally

## Independent validation

Permanent RecoveryAudit was extended through HF21 in commit `a8926a18467fa1d436f086e71c5296dc75fe3bc6`. Workflow run `34248508707` succeeded. The published auditor was executed against the HF21 candidate; OPEN1 and OPEN2 both reported 320 types, 2317 methods, 2297 bodies, validated all prior accepted HF methods plus the eight HF21 targets, and ended `RECOVERY_AUDIT_OK`.

The audit artifact ZIP SHA-256 is `e998add22dc898e367ed611645e26e4db00a5ad2e862365f907f3dae469f4a17`.

ILSpyCmd is fixed at `11.0.0.9375` / `ICSharpCode.Decompiler 11.0.0.9375`, using the complete 56-DLL reproduced Cpp2IL reference set from source commit `5fb20304df698ffd3d0e664b2a698cd911dc9d57`. The ILSpy bundle ZIP SHA-256 is `a3f15ed3602859ab5da8969718f560a4bc555ab56cf49af27104ce91fd94c305`.

All eight target MethodDef readbacks exited 0 with zero stderr. Whole-assembly HF20 and HF21 readbacks also exited 0 with zero stderr. Re-running HF20 with the exact HF21 ILSpy binary/reference set reproduced the previously archived HF20 whole-IL SHA exactly, eliminating tool-version drift as a diff source.

Whole-assembly MethodDef count remains 2317 -> 2317. After normalizing only physical method RVA annotations and `I_XXXXXXXX` address/layout labels, the non-method skeleton is byte-identical and exactly these eight MethodDefs change:

- `0x060001E8` `ParticlesManager.Start`
- `0x060001EA` `ParticlesManager.CreatNewParticle`
- `0x060001F3` `ProjectileManager.Start`
- `0x060001F4` `ProjectileManager.SetFloatScale`
- `0x060001F6` `ProjectileManager.CrateNewProjectile`
- `0x0600022E` `ResourceManager.Load_projectileSprite`
- `0x060003DD` `Projectile.ResetData`
- `0x060003DE` `Projectile.BindTrack`

Semantic diff SHA-256:

`b63926c9bfaafd2f5f00e4fdf2f3054831152c06285066778b43fcbdf83e20a1`

## Drive acceptance

Archive folder `HF21-Projectile-Resource-Cluster`:

- folder ID `1YCwBHrkS1cfqSTV8R-oE0HPxgGrUffsb`;
- cumulative audited DLL ID `1pg6j1bkcQTyWj9_obFCXryQvL3dASwEm`;
- final patcher ZIP ID `1H8lakklFTIcQWpbE3uvDEsYac_nT51Qd`;
- RecoveryAudit bundle ID `1xc-KUhgx-QLASfF7YPssZ_6miid3N0ql`;
- ILSpy bundle ID `15csOFTqduImTepAtjKZugfAJVIKA7GZK`;
- patch source ID `1OHc3zwGYtQ7TH9_1Mt39qvCGmiVLNMPF`;
- semantic diff ID `1bzx9fdS1qys0VYkP1SX9kDPkE29vsDDA`;
- Cecil audit ID `1mz8jaHhbDxEwjzS0OBjlVB9KLJunT8fK`;
- target manifest ID `11gUBDr3oQogIm1s1XquJOXm_Eq517K_6`;
- tool/reference provenance ID `1v_sV7W63M-fxAwUdCeWLWsBJ-hGdmZki`.

The provider listing was first verified after the payload upload and contained exactly 33 pre-closure payload files. The authoritative closure pair was then uploaded:

- `HF21-Projectile-Resource-Cluster-Evidence-FINAL.md` — Drive `1Qrd2cZ3J1Zub7o89-IFV5S6fWMkxjaLr`, SHA-256 `e751d733781906485d65d47c5ee392508748b1baa0b30bc529381143ee317b25`;
- `HF21-SHA256SUMS-FINAL.txt` — Drive `1jggygaIrRNdSgKYDkNThjiAZ6tDuGXVw`, SHA-256 `f86314aebeec2833a962a09b850242632294f8e8f661972caa272706d216903a`.

A second provider-level folder readback verified exactly **35 expected files**: 33 payload files plus the two authoritative closure files, with no duplicate or superseded drafts.

**HF21 formal acceptance: PASS.**

## Next decision gate

Do not open HF22 automatically. Run one remaining managed-damage / active-path gate scan across the recovered Assembly-CSharp and original PC-native attribution. If no critical active-path damaged cluster remains, stop adding HF stages and move to Unity `2022.3.44f1c1` import/build validation, package restoration including the outstanding 67/67 package-script validation, and recovered Assembly-CSharp IL2CPP conversion. Only a native-backed critical blocker discovered by that gate may justify HF22.
