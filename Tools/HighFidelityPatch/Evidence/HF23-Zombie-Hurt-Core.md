# HF23 — Zombie Hurt Core native recovery evidence

Date: 2026-09-09  
Branch: `high-fidelity`  
Classification: **Native-backed managed-observable recovery of the nine declared Zombie Hurt MethodDefs**

## Formal result

Formal HF22 input SHA-256:

`502d6d61c18c17e29acb2f96451e9e41f98a403d8f8e915a76285c5b51f04997`

HF23 cumulative final SHA-256:

`35d13b0d7fc5f82e04b837b812fab501a933b00e21f3431b2f6759c8e0c2b39c`

HF23 restores exactly these nine MethodDefs and no others:

1. `Zombie.Hurt_Armor1(Damage,Projectile)` — `0x06000456` — PC `0x1803625D0`
2. `Zombie.Hurt_Armor2(Damage,Projectile)` — `0x06000457` — PC `0x180362BE0`
3. `Zombie.Hurt_Artillery(Damage,Projectile)` — `0x06000458` — PC `0x1803630B0`
4. `Zombie.Hurt_Ashes(Damage,Projectile)` — `0x06000459` — PC `0x180363320`
5. `Zombie.Hurt_Body(Damage,Projectile)` — `0x0600045A` — PC `0x1803635F0`
6. `Zombie.Hurt_FinalDamageReduction(float,DamageAttribute)` — `0x0600045B` — PC `0x180363980`
7. `Zombie.Hurt_Normal(Damage,Projectile)` — `0x0600045C` — PC `0x180363B00`
8. `Zombie.Hurt_Real(Damage,Projectile)` — `0x0600045D` — PC `0x180363D90`
9. `Zombie.Hurt_Throughout(Damage,Projectile)` — `0x0600045E` — PC `0x180363FD0`

Attribution was re-derived from the original PC Assembly-CSharp CodeGenModule using `original MethodDef RID - 1 -> methodPointers index`; the archived preview was not substituted for authoritative attribution. The original PC ZIP, `GameAssembly.dll`, and `global-metadata.dat` were re-fetched/extracted and independently re-hashed before formal reconstruction:

- PC ZIP `2f08b4e2243e2bd296f3db694b4a2c9656252d76202c80167bdf3340b9f65b48`
- PC `GameAssembly.dll` `9ebd7ca996a5b03fb4a766f7a2502b660d581ddbbb36af4d7bf2f06a211da39d`
- PC `global-metadata.dat` `ad992341add498bd1018ac980f42171e0737b0793bf945c8714ad70a445b36b9`

## Native-observable behavior

The original Zombie armor/body defense path is distinct from the Plant/Device mitigation path. For non-real damage the native defense helper gives `damage - 0.9*defense` when defense is below damage and a minimum `damage*0.1` when defense is at least damage; real damage bypasses this reduction. Defense is fed by `max(round(buffManager.GetIncrement(baseDefense,"Def") + baseDefense), 0)`.

`Hurt_FinalDamageReduction` preserves the PoleCommander gate: non-real, ID 3, pole present, not `isStant`, `stiffnessTime <= 0`; a `StatsIncreased` returned by `FindBuff("PoleCommander.speed")` gives `Clamp((value + 1) * 0.3, 0, 0.95)`. The native cap is `0.95`.

The original differences among normal, throughout, artillery, ashes and real are retained. `Hurt_Artillery` damages Armor2 but ignores its returned remainder; Armor1 remainder gates body damage. `Hurt_Real` executes armor side effects but ignores both armor remainders and enters body with the original damage. `Hurt_Throughout` uses armor penetration remainder and restores `Damage.damagePoint` to the method-entry value after successful `Hurt_Body`.

Throughout armor remainder uses native `cvttss2si` float-to-int truncation before conversion back to float. Armor2 normal damage does not pass overkill remainder when positive armor damage was applied. Armor1 iceCube applies `fire_ice` at `element.point * 0.2`, clamped to `[0,maxArmor1Point]`.

`Hurt_Ashes` preserves the disabled/dead destroy branch and lethal `Ashe(damage)` branch, including the second `IsDisabled()` observation. `Hurt_Body` preserves flashing, HP subtraction/status update and the large-damage text gate: actual damage >= 300 and either >= max body HP / 3 or >= 1800, with Y=`fZ + fY + 134`.

Armor2 hit audio is limited to screendoor/heavyShield/ladder using `Random.Range(2,4)` and `ResourceManager.zombieClips`. Original metadata-usage string-literal decoding closes Armor1 display anchors without inference: `anim_cone`, `anim_bucket`, `anim_brick`, `IceCube1`, heavyHelmet=`anim_bucket`, saboteursArmor=`LadderSaboteurs_helmet1`.

## Patcher and repeatability

HF23 patcher source is under `Tools/HF23Patch/`.

- `Program.cs` Git blob `e39d080dcb68ff30b1d2a5408fc2b23387c71e95`
- build workflow blob `c2bb5d07a308da39b8c614842e3ee7b47b2fb2aa`
- build head `80187a41b3652cc2fc29ab980fcfa6ade6cd0f80`
- workflow run `34302953269` — success
- patcher artifact SHA-256 `dee8e41e1f01d23e63def80092d50a8272a2db92cb3049520d9190fc762948f5`

The patcher hard-rejects any input not hashing to accepted HF22. Two independent applications to the re-fetched HF22 formal input produced byte-identical outputs at the HF23 final SHA.

Cecil reopen measurements:

- Hurt_Armor1 — 241 IL / 664 bytes / 1 EH
- Hurt_Armor2 — 203 / 516 / 0 EH
- Hurt_Artillery — 70 / 176 / 1 EH
- Hurt_Ashes — 97 / 243 / 1 EH
- Hurt_Body — 143 / 382 / 0 EH
- Hurt_FinalDamageReduction — 47 / 117 / 0 EH
- Hurt_Normal — 73 / 184 / 1 EH
- Hurt_Real — 59 / 152 / 1 EH
- Hurt_Throughout — 78 / 193 / 1 EH

No target retains a Cpp2IL helper after reopen.

## Independent RecoveryAudit

Permanent `Tools/RecoveryAudit/` was extended through HF23 in commit `504c9b36db406e6ed67fbe3e5d018f7e1cf746b7`; resulting `Program.cs` blob `f9a912a69b120956e5871f7176d35456e6bfda88`.

RecoveryAudit workflow run `34303834229` succeeded. Published artifact SHA-256:

`ccfc1f16e88ce3b8b5d482b65baf7f17030002bbef51ad95e9b4c77a1c98986d`

The published auditor was run on final HF23. OPEN1 and OPEN2 each report 320 types, 2317 methods, 2297 bodies; all earlier accepted targets and all nine HF23 targets pass on both opens, ending `RECOVERY_AUDIT_OK`.

## Fixed ILSpy / whole-assembly isolation

ILSpyCmd / ICSharpCode.Decompiler remain fixed at `11.0.0.9375`. References are the reproduced 56-DLL fixed-Cpp2IL set from source commit `5fb20304df698ffd3d0e664b2a698cd911dc9d57`, reference ZIP SHA-256 `fe091e5a5389c2491eebeff72c83c394097bef67ffd45a25cf7de01c9aad9ef1`.

All nine member readbacks exit 0 with stderr 0. Whole HF22/HF23 readbacks exit 0 with stderr 0. Re-decompiling HF22 in the exact HF23 environment reproduces the archived HF22 whole-IL SHA exactly.

- HF22 whole IL SHA-256 `dab15e70a4769b3f9c8af6b8dab91400151608df0625a70256f59c752f9be924`
- HF23 whole IL SHA-256 `7ea1f0c41035d87165b72ee462828a8c5b1c176e42db608c4f3dd72dcb54aa51`
- MethodDef count `2317 -> 2317`
- normalized non-method skeleton byte-identical
- exactly nine declared MethodDefs change
- semantic diff SHA-256 `bf0d7505ebc82ca185290ef81b524ffe4946b6a96ad2f33919b41ed8f8af3b1b`

Before using the semantic-isolation implementation on HF23, it independently reproduced the archived HF21->HF22 diff SHA `1adaabb8c214895f9244b85b1fcd620e42c67aa1b7e4bab0bb86165b2c528e7c`.

## Drive acceptance

Archive folder `HF23-Zombie-Hurt-Core`, ID `1nvim2033w3T_GnmaDHlSf8rVyF6CT-0K`.

Key artifacts:

- cumulative audited DLL `1qcGGHQZuVe5s60jje-dIJFzHVH-gkD7M`
- patcher `1FqOSA3Zuh_DXZkL4NW8TfIVJ2Y5L_bby`
- published RecoveryAudit `1GsLxEckfzbm09KBpo1M7dX2Hk5umFmTU`
- fixed ILSpy bundle `1y4_qQ920mNoQVX8ckKln4zRk2dKySPCM`
- native evidence `1RnnZo0HCw1Al3c5dKKRhZWw1c5TNOepi`
- patch source `1LIEAv9G1leSuP63UzA22AiLznBat06mY`
- semantic diff `1B8xkLMLHCOIiRT3_haLynEBD5aAX9Yjo`
- semantic isolation `1LmhFhaL9gQDmdznFnr-lqszXpjLtRkXX`
- MethodDef table `1qeRRmc2u3Qoqz5mW3FKqn5z5lxF87uK9`
- RecoveryAudit log `1mUGfRynao8RJWIVIyAzlCyoitCoXw-VH`
- payload SHA manifest `1EvMzDA0yo0ZcbKMyl9De53U8dT81PySx`
- Evidence-FINAL `1rp6hKit37kn72uBwNfW2DwNwhmYUz_hL`, SHA-256 `6f5deaaa2229873e77369de79b39a60fd3668df3c8f8ac473c2105d384b0e81b`
- SHA256SUMS-FINAL `10FF_4PdwqG4VsmuSyPFVfRyRKnnac_7I`, SHA-256 `913665935e7781e44bb43d8c45d999ba5d783e3073df03df3aad6b6324f73e75`

Provider readback after closure has no next page and verifies exactly **22 files = 20 payloads + 2 closure files**.

**HF23 formal acceptance: PASS.**

## Next decision gate

Do not open HF24 automatically. Run the remaining managed-damage / active-path decision scan. A new HF stage is justified only by both concrete managed-damage evidence and active-path importance; otherwise stop HF recovery and proceed to Unity `2022.3.44f1c1`, package restoration, and the outstanding 67/67 package-script validation.
