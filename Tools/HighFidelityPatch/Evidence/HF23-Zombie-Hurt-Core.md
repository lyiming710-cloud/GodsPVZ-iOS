# HF23 — Zombie Hurt Core native recovery evidence

Date: 2026-09-09  
Branch: `high-fidelity`  
Classification: **Native-backed managed-observable recovery of the nine declared Zombie Hurt MethodDefs**

## Formal result

Formal HF22 input SHA-256:

`502d6d61c18c17e29acb2f96451e9e41f98a403d8f8e915a76285c5b51f04997`

HF23 gated cumulative candidate SHA-256:

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

Attribution was re-derived from the original PC Assembly-CSharp CodeGenModule using the project rule `original MethodDef RID - 1 -> methodPointers index`. The HF23 preview was not treated as the authoritative attribution source.

Primary originals were re-fetched/re-extracted and re-hashed before recovery:

- PC ZIP: `2f08b4e2243e2bd296f3db694b4a2c9656252d76202c80167bdf3340b9f65b48`
- PC `GameAssembly.dll`: `9ebd7ca996a5b03fb4a766f7a2502b660d581ddbbb36af4d7bf2f06a211da39d`
- PC `global-metadata.dat`: `ad992341add498bd1018ac980f42171e0737b0793bf945c8714ad70a445b36b9`

## Native-observable behavior

The original Zombie armor/body defense path is distinct from the previously restored Plant/Device mitigation path. For non-real damage the native defense helper gives `damage - 0.9*defense` when defense is below damage, and a minimum `damage*0.1` when defense is at least damage. Real damage bypasses this reduction. The defense input is computed as `max(round(buffManager.GetIncrement(baseDefense,"Def") + baseDefense), 0)`.

`Hurt_FinalDamageReduction` preserves the original PoleCommander gate: non-real damage, zombie ID 3, pole present, not `isStant`, and `stiffnessTime <= 0`. If `buffManager.FindBuff("PoleCommander.speed")` is a `StatsIncreased`, the reduction is `Clamp((value + 1) * 0.3, 0, 0.95)`. The native upper cap is therefore `0.95`, not `1.0`.

Armor handling restores the native separation between normal, throughout, artillery, ashes, and real paths. `Hurt_Artillery` damages Armor2 but deliberately ignores its returned remainder; Armor1 remainder controls whether body damage follows. `Hurt_Real` executes armor side effects but ignores both armor remainders and enters body with the original damage. `Hurt_Throughout` uses the armor penetration remainder but restores `Damage.damagePoint` to the method-entry value after the successful `Hurt_Body` call.

The throughout armor remainder uses the native float-to-int truncation (`cvttss2si`) before the integer is converted back to float. Armor2 normal damage does not pass overkill remainder to the next layer when positive armor damage was applied. Armor1 iceCube absorbs `fire_ice` using `element.point * 0.2` and clamps Armor1 HP to `[0,maxArmor1Point]`.

`Hurt_Ashes` preserves the disabled/dead destroy branch and the lethal `Ashe(damage)` branch, including the second `IsDisabled()` observation used for the returned bool. `Hurt_Body` preserves flashing, HP subtraction/status update, and large-damage text. Damage text requires actual damage >= 300 and either >= one third of max body HP or >= 1800; its Y coordinate is `fZ + fY + 134`.

Armor2 hit audio is limited to screendoor/heavyShield/ladder and uses `Random.Range(2,4)` with `ResourceManager.zombieClips`. Metadata-usage decoding of the original string-literal table closed the Armor1 display anchors without inference: `anim_cone`, `anim_bucket`, `anim_brick`, `IceCube1`, heavyHelmet also `anim_bucket`, and `LadderSaboteurs_helmet1`.

## Patcher and repeatability

HF23 patcher source lives under `Tools/HF23Patch/`.

- `Tools/HF23Patch/Program.cs` Git blob: `e39d080dcb68ff30b1d2a5408fc2b23387c71e95`
- build workflow blob: `c2bb5d07a308da39b8c614842e3ee7b47b2fb2aa`
- build head: `80187a41b3652cc2fc29ab980fcfa6ade6cd0f80`
- patcher workflow run: `34302953269` — success
- patcher artifact SHA-256: `dee8e41e1f01d23e63def80092d50a8272a2db92cb3049520d9190fc762948f5`

The patcher hard-rejects any input whose SHA-256 is not the accepted HF22 cumulative final. It was applied twice independently to the re-fetched formal HF22 DLL. Both outputs are byte-identical at the HF23 SHA above.

Reopen measurements:

- Hurt_Armor1 — 241 IL / 664 bytes / 1 EH
- Hurt_Armor2 — 203 / 516 / 0 EH
- Hurt_Artillery — 70 / 176 / 1 EH
- Hurt_Ashes — 97 / 243 / 1 EH
- Hurt_Body — 143 / 382 / 0 EH
- Hurt_FinalDamageReduction — 47 / 117 / 0 EH
- Hurt_Normal — 73 / 184 / 1 EH
- Hurt_Real — 59 / 152 / 1 EH
- Hurt_Throughout — 78 / 193 / 1 EH

No target contains a remaining Cpp2IL helper after Cecil reopen.

## Independent RecoveryAudit

Permanent `Tools/RecoveryAudit/` was extended through HF23 in commit `504c9b36db406e6ed67fbe3e5d018f7e1cf746b7`; resulting `Program.cs` blob `f9a912a69b120956e5871f7176d35456e6bfda88`.

RecoveryAudit workflow run `34303834229` succeeded. Published RecoveryAudit artifact SHA-256:

`ccfc1f16e88ce3b8b5d482b65baf7f17030002bbef51ad95e9b4c77a1c98986d`

The published auditor was executed against the HF23 gated DLL. OPEN1 and OPEN2 both reopened the assembly independently and reported 320 types, 2317 methods, 2297 bodies. All prior accepted targets plus the nine HF23 targets passed their body floors on both opens, ending with:

`RECOVERY_AUDIT_OK`

## Fixed ILSpy / whole-assembly isolation

ILSpyCmd and ICSharpCode.Decompiler remain fixed at `11.0.0.9375`. The reference set is the reproduced 56-DLL fixed-Cpp2IL set from source commit `5fb20304df698ffd3d0e664b2a698cd911dc9d57`, ZIP SHA-256 `fe091e5a5389c2491eebeff72c83c394097bef67ffd45a25cf7de01c9aad9ef1`.

All nine HF23 member readbacks exit 0 with stderr 0. Whole-assembly HF22 and HF23 readbacks also exit 0 with stderr 0. Re-decompiling HF22 with the exact HF23 gate environment reproduced the archived HF22 whole-IL SHA exactly, excluding tool/reference drift.

- HF22 whole IL SHA-256: `dab15e70a4769b3f9c8af6b8dab91400151608df0625a70256f59c752f9be924`
- HF23 whole IL SHA-256: `7ea1f0c41035d87165b72ee462828a8c5b1c176e42db608c4f3dd72dcb54aa51`
- MethodDef count: 2317 -> 2317
- normalized non-method skeleton: byte-identical
- changed MethodDefs: exactly the nine declared HF23 targets
- semantic diff SHA-256: `bf0d7505ebc82ca185290ef81b524ffe4946b6a96ad2f33919b41ed8f8af3b1b`

The semantic-isolation implementation was first checked by independently reproducing the previously archived HF21->HF22 semantic diff SHA `1adaabb8c214895f9244b85b1fcd620e42c67aa1b7e4bab0bb86165b2c528e7c`; only after that reproduction was the same normalization applied to HF22->HF23.

## Drive archive — pre-closure payload

Archive folder `HF23-Zombie-Hurt-Core`:

- folder ID `1nvim2033w3T_GnmaDHlSf8rVyF6CT-0K`
- cumulative audited DLL ID `1qcGGHQZuVe5s60jje-dIJFzHVH-gkD7M`
- patcher ID `1FqOSA3Zuh_DXZkL4NW8TfIVJ2Y5L_bby`
- published RecoveryAudit ID `1GsLxEckfzbm09KBpo1M7dX2Hk5umFmTU`
- fixed ILSpy bundle ID `1y4_qQ920mNoQVX8ckKln4zRk2dKySPCM`
- native evidence ID `1RnnZo0HCw1Al3c5dKKRhZWw1c5TNOepi`
- patch source ID `1LIEAv9G1leSuP63UzA22AiLznBat06mY`
- semantic diff ID `1B8xkLMLHCOIiRT3_haLynEBD5aAX9Yjo`
- semantic isolation ID `1LmhFhaL9gQDmdznFnr-lqszXpjLtRkXX`
- MethodDef table ID `1qeRRmc2u3Qoqz5mW3FKqn5z5lxF87uK9`
- RecoveryAudit log ID `1mUGfRynao8RJWIVIyAzlCyoitCoXw-VH`
- payload checksum manifest ID `1EvMzDA0yo0ZcbKMyl9De53U8dT81PySx`

Provider readback before closure reports exactly **20 payload files**. Final Evidence and final SHA256SUMS closure files are deliberately added only after this evidence source is committed; the folder must then be listed again before HF23 is declared formal PASS.

## Next decision gate

After Drive closure and `Recovery/STATUS.md` advancement, do not open HF24 automatically. Run the remaining managed-damage / active-path decision scan. A new HF stage is justified only by both concrete managed-damage evidence and active-path importance; otherwise stop HF recovery and proceed to Unity 2022.3.44f1c1/package restoration and the outstanding 67/67 package-script validation.
