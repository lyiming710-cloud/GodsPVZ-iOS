# HF34 — Projectile Runtime Support Core native recovery evidence

Date: 2026-09-10  
Branch: `high-fidelity`  
Classification: **Native-backed active-path projectile support recovery**

## Formal result

Formal HF33 input SHA-256:

`6b467ef40f2e8e4e7fcff48d71c328457ac4107261a95fbd59908f5516720518`

**HF34 cumulative candidate SHA-256 pending final Drive 20+2 / STATUS closure:**

`ead8dee5def2818ca8c64c36b4fe65f1830dbf873fae34dbc84d2f07b103d999`

HF34 restores exactly four MethodDefs:

- `0x0600013E GlobalStaticVars.CreateAudioAtPoint(AudioClip,Vector3,float,float)` — original RID 318, PC `0x18031B350`;
- `0x060003D6 Projectile.Update_Time()` — original RID 982, PC `0x18037D220`;
- `0x060003DB Projectile.Update_MoveTrack7()` — original RID 987, PC `0x18037CCF0`;
- `0x06000450 Zombie.GetPredictedPosition(float)` — original RID 1104, PC `0x1803617F0`.

No other MethodDef is modified.

## Scope admission and active-path proof

Attribution uses the fixed project rule: original Assembly-CSharp MethodDef RID-1 -> Assembly-CSharp CodeGenModule methodPointers index.

Each target independently satisfies both HF-stage admission gates: (1) the HF33 managed body contains concrete mis-reconstruction / invalid IL relative to original PC native behavior; and (2) the method is directly reached by an already accepted gameplay path.

Active path: accepted `Projectile.Update()` directly uses `Update_Time()` and the movement-track-7 branch calls `Update_MoveTrack7()`; accepted `Projectile.Collision_Zombie(Zombie)` calls `Zombie.GetPredictedPosition(float)`; accepted `Projectile.Collision_AudioParticle()` calls the four-argument `GlobalStaticVars.CreateAudioAtPoint(...)` overload. HF34 was not opened from warning count, MethodDef adjacency or shared-stub xrefs.

## Original PC native behavioral closure

Primary binary is the original PC `GameAssembly.dll`, SHA-256 `9ebd7ca996a5b03fb4a766f7a2502b660d581ddbbb36af4d7bf2f06a211da39d`, with original metadata SHA-256 `ad992341add498bd1018ac980f42171e0737b0793bf945c8714ad70a445b36b9`.

### `0x0600013E CreateAudioAtPoint(..., pitch)`

Unity-null `clip` returns null; create `GameObject("One shot audio")`; assign `transform.position`; add `AudioSource`; set clip/volume/pitch/spatialBlend=0; Play; destroy after `clip.length * (0.01f > Time.timeScale ? 0.01f : Time.timeScale)`; return the AudioSource. The restored CIL deliberately spills/checks intermediate Unity objects so fixed ILSpy reconstructs the same null-dereference behavior without invalid stack typing; these checks do not add gameplay behavior.

### `0x060003D6 Projectile.Update_Time()`

`step = Time.deltaTime * updateRate`; add to `livingTime`; ID27 after >3 and ID28 after >15 call `damage.AreaDamage()`, `Collision_AudioParticle()`, `DestroyProjectile()`; ID21 accumulates `pc_21_timing`, performs `origin_Plant.GetDamage(this,21,0).AreaDamage()` every >=1 second while the plant exists and resets the timing, and calls `PC_LightSaber_Explosion()` when `livingTime > maxLivingTime`.

### `0x060003DB Projectile.Update_MoveTrack7()`

`zSpeed=(fZ-75)*-5.2`; when `origin_Plant` exists, use plant transform position; angle is `index*51.43 + origin_Plant.LivingTime()*120` degrees; source-equivalent `Quaternion.Euler` rotates `(300,0,0)`; set speed to `((plantX+offsetX-fX)*2.2, (plantY+offsetY*0.75-fY)*2.2, 0)`.

### `0x06000450 Zombie.GetPredictedPosition(float)`

Start at `(fX,fY,fZ)`; `t=time+Time.deltaTime`; x += t*rSpeed.x; y += t*rSpeed.y; z unchanged.

Cpp2IL is used only to identify broken managed reconstruction; it is not treated as source for these behaviors.

## Formal patcher and deterministic output

Final stable-audio-CIL template commit `ee6092042d3396e71c01a574fad5f59a6d84ff49` follows earlier rejected tool/template candidates which were never propagated. Final published patcher workflow `34387511067` PASS, artifact ID `10118287463`, artifact SHA-256 `e9e39330f029467bb6cf86af153a630e62283ee3214c1b2178494b7a0473a1f2`.

Published final patcher applied independently twice to the same SHA-verified HF33 formal input produced byte-identical output `ead8dee5def2818ca8c64c36b4fe65f1830dbf873fae34dbc84d2f07b103d999`, stderr 0 both runs.

Cecil reopen: CreateAudioAtPoint/4 `55 IL / 145 bytes / 0 EH`; Update_Time `81 / 225 / 0`; Update_MoveTrack7 `67 / 207 / 0`; GetPredictedPosition `36 / 83 / 0`.

## Fixed ILSpy member gate

Fixed ILSpyCmd / ICSharpCode.Decompiler `11.0.0.9375` with reproduced 56-DLL reference set SHA-256 `fe091e5a5389c2491eebeff72c83c394097bef67ffd45a25cf7de01c9aad9ef1`. All four HF34 member readbacks: exit 0, stderr 0, Cpp2IL refs 0, issue markers 0.

## Whole-assembly IL and semantic isolation

HF33 whole IL reproduced exactly `36f84c63dc8906ab33424da6a67246569a7e103e5b2ffbf35ca757d698698450`; HF34 whole IL SHA-256 `96d4bd053040cb1531d3feb5f2e28d20cfd9f51ebc273d6a457127f312088599`; both stderr 0. Accepted HF32->HF33 semantic diff was reproduced byte-for-byte first at `59d5f0c92d7115c54bc4f5ee69881c5b120d1a99dabc640e7ce180dbfdc3ceae`.

Using the same accepted normalization: MethodDef `2317 -> 2317`; normalized non-method skeleton byte-identical; changed MethodDefs exactly `0x0600013E`, `0x060003D6`, `0x060003DB`, `0x06000450`; HF33->HF34 semantic diff SHA-256 `27e250ef34624ad70c5345c770eefe52d7204a4452d7c861013e1a72dcc59697`.

## Permanent RecoveryAudit

RecoveryAudit extended at commit `addee3ab7b67d406561e8b9e11a2eae75296a786`; workflow `34388391648` PASS; artifact ID `10118629226`; SHA-256 `32637b66f392dfb4d4d880e54b3ce768e43178d4a04606bbcdce3e1d37645879`. Published auditor closure run: OPEN1/OPEN2 `320 types / 2317 methods / 2297 bodies`, four HF34 target IL sizes match Cecil reopen, terminal `RECOVERY_AUDIT_OK`.

## Google Drive pre-closure archive

Folder `HF34-Projectile-Runtime-Support-Core`, ID `18gmKI_V1HS2j1CFnzauo9GZkN3bJlTPf`. Provider pre-closure readback returns exactly 20 files with no missing or extra file relative to the manifest.

Key provider IDs: cumulative DLL `1UEZgP33n40vo9klmH0sU5r8bx7kZt1iB`; patcher `1jFoTaMOaneREMpLv-r0BLf2P-MEWVnFk`; fixed ILSpy `1cZD1za8a-LcKw6jL_7fbzuZAHUivyWCB`; RecoveryAudit `1U_y6fl44zIEooVR29rsRo0kDB4mB5D27`; semantic diff `1BUnU0jZVFWonqWPPetcvw6Obtamy2_5e`; Cecil reopen `1G8-jEsb1UTz5chovWCSyGVKG2Tb6MQ5A`; member summary `1YFOIyo-4dWpC5zE2FHuSCjyowl6gfdWB`; member ZIP `1B4unKZ1PrkIJ9Zrn3LQJJ7SaEvYqWup8`; whole provenance `1R-WHMVrgrs9xW25T4MJRuvV4cQrLo9a2`; MethodDef table `1TLhp_pf-WozjI611ER4PrsB3StexGZuo`; RecoveryAudit log `1mOcdT4fcxflVx1GhnyUaaQiOFBIjeYXK`; native evidence `1gm4reiFQgviwP4xy7DZKux4dEOEnuCL2`; patch source `1FinO7y3gw5NL57-hrS0KHMAF4qpeQSF4`; formal run1 `1ohMxaRJPsgQbWKK9pigwcBZAZ6BBsBtg`; formal run2 `1d0YgD8eFikWEuNyyMEtGbXpTeHB_tJlG`; reference provenance `1KEMnevLSMfUPtLXuqbSrQUtTah-n2PNN`; semantic isolation `1muj4mT0EneLuoAAYYUlNgSkgtJg5u3zX`; source/build provenance `1WZj4Vq53o615Vr7iTu4FJtlYK8SNaTkj`; target manifest `1LkblocpEPPbLK6o62h77wYA5QWhsKbp6`; payload manifest `112piV84o2zvDaUeBen5MeKCnn0YAXi_S`, SHA-256 `7c8dddee9cabaf5fb4bdf3b319c83bf9cfd0213e341f526e8e20a00d85ed3596`.

HF34 is not formal merely because this Evidence file exists. It becomes formal only after `HF34-Evidence-FINAL.txt` and `HF34-SHA256SUMS-FINAL.txt` are generated from this Evidence commit and verified 20-file provider archive, uploaded, final provider readback proves exactly 22 files, and only then `Recovery/STATUS.md` is advanced.
