# HF22 — Damage Dispatch / Area Core native recovery evidence

Date: 2026-09-09  
Branch: `high-fidelity`  
Classification: **Exact for managed-observable behavior of the 12 declared MethodDefs**

## Formal result

Formal HF21 input SHA-256:

`888cab48b5e488ca05ed8ec58a11503c9f9a8ce0bac23a60c933f8628834e8f0`

HF22 cumulative final SHA-256:

`502d6d61c18c17e29acb2f96451e9e41f98a403d8f8e915a76285c5b51f04997`

HF22 restores exactly these 12 MethodDefs and no others:

- `Damage.AreaDamage()` — `0x0600010C`
- `Damage.AreaDamage_Device(Camp)` — `0x0600010D`
- `Damage.AreaDamage_Plant()` — `0x0600010E`
- `Damage.AreaDamage_Zombie()` — `0x0600010F`
- `ElementManager.ToEffect(Element)` — `0x06000124`
- `ElementManager.GetElement(ElementType)` — `0x06000127`
- `Device.CanAttacked(Damage)` — `0x06000310`
- `Device.TakeDamage(Damage,Projectile)` — `0x06000336`
- `Plant.TakeDamage(Damage,Projectile)` — `0x060003B3`
- `Zombie.CanAttacked()` — `0x06000431`
- `Zombie.GetATK()` — `0x06000441`
- `Zombie.TakeDamage(Damage,Projectile)` — `0x0600047F`

The nine Zombie `Hurt_*` MethodDefs are deliberately not modified in HF22; they remain the direct HF23 blocker.

## Native-observable behavior

`Damage.AreaDamage` restores the attackRange-null gate, original logging, Board recovery from sourcePlant/sourceZombie/sourceDevice in order, and Camp routing. The typed helpers collect targets before applying damage, preserve `damagePoint` around every target call, and preserve enumerator finally/dispose and native null behavior.

`ElementManager.ToEffect` appends to `elements_toEffect`; `GetElement` returns the first matching `Element.type` with original null/enumerator behavior.

`Device.CanAttacked` restores ID 2/7/8/12/14 rejection, ID11 ashes-only behavior, and `!broken`. `Device.TakeDamage` restores bright-state writes; fire/ice coefficient `0.2`; ICE UI update with `destructive=true`; native mitigation using constants `3`, `2`, and max reduction `0.9`; ID13 targetZombie transfer ratio `0.3`; HP UI, audio/particle and InjuryStatusUpdate ordering.

`Plant.TakeDamage` restores defense as `max(MathF.Round(buffManager.GetIncrement(defensePoint,"Def") + defensePoint),0)`, native mitigation, ID49 `0.7` multiplier under the original gate, flash values, HP subtraction and element queueing.

`Zombie.CanAttacked` restores activeSelf/invincible/isDied/ashes checks, pole-jump behavior for IDs 3/23 and ID13 exclusion. `Zombie.GetATK` restores `MathF.Round` before zero clamp. `Zombie.TakeDamage` restores additional buffs, stiffness logic, activity/death gates, DamageAttribute dispatch to Hurt methods, non-fire_ice element queueing, KillEvent/ID10 wakeup behavior and optional HP UI recording.

## Patcher and repeatability

Final patcher source: `Tools/HF22Patch/Program.cs`, Git blob `48628fa32d27b52cc9664e8cd8bd01ff0a04071f`.

- final build head `74f23e6a08757c2e0f567338f437f350b84f1c27`;
- final CI run `34259165102` — success;
- patcher artifact SHA-256 `95b1c0d4fd54bd5a03e2b80f5a457264e472db312c4365d59e631badc6e6483a`.

Two earlier tool-only attempts stopped before output: run `34257793931` (MethodBody namespace ambiguity) and run `34258487690` (generic `!0` mapper rejection). Neither produced a candidate.

Formal HF21 was re-fetched from its accepted Drive archive and re-hashed. Two independent applications of the final patcher were byte-identical at the HF22 SHA above.

Reopen measurements:

- AreaDamage 93 IL / 284 bytes / 0 EH
- AreaDamage_Device 78 / 205 / 2 EH
- AreaDamage_Plant 73 / 198 / 2 EH
- AreaDamage_Zombie 73 / 198 / 2 EH
- ToEffect 5 / 13 / 0 EH
- GetElement 31 / 73 / 1 EH
- Device.CanAttacked 40 / 88 / 0 EH
- Device.TakeDamage 240 / 655 / 1 EH
- Plant.TakeDamage 97 / 275 / 1 EH
- Zombie.CanAttacked 41 / 97 / 0 EH
- Zombie.GetATK 13 / 45 / 0 EH
- Zombie.TakeDamage 197 / 536 / 2 EH

## Independent validation

Permanent RecoveryAudit was extended through HF22 in commit `1c25249751f45935b468f56cad8f9c66cbfcb56e`; workflow run `34259348121` succeeded. The published auditor was executed against the final HF22 DLL; OPEN1 and OPEN2 both ended `RECOVERY_AUDIT_OK`.

ILSpyCmd / ICSharpCode.Decompiler are fixed at `11.0.0.9375`. The whole-assembly check uses the full reproduced 56-DLL fixed-Cpp2IL reference set. All 12 member readbacks exit 0 with stderr 0, and both HF21/HF22 whole-assembly reads exit 0 with stderr 0.

Whole MethodDef count remains 2317 -> 2317. After normalizing only physical method RVA/address labels, the non-method skeleton is byte-identical and exactly the 12 declared MethodDefs change. Formal archived semantic diff SHA-256:

`1adaabb8c214895f9244b85b1fcd620e42c67aa1b7e4bab0bb86165b2c528e7c`

## Drive acceptance

Archive folder `HF22-Damage-Dispatch-Core`, ID `1hxsyCUrCVtDwDgkIYAg_y6T0wedtecwT`.

Key artifacts:

- final audited DLL `1VH6agXwBpnGVoonUpkLP9a-vt6Xbv8YP`;
- final patcher `1Tr85G5PmeFTeZUmW0RDjvOAJCW77ka9m`;
- RecoveryAudit bundle `1chfZ5ev40t1XVAKPKjlYCLa9LqMGjY3H`;
- ILSpy bundle `1KeEM8fA5eXVebqH1yToyEdEi5R8rmA3w`;
- Cecil audit `1e20KrFBlt736yxCk_VSJxy6qoYOOM17e`;
- native evidence `12QjaZhKLtj1ZKF8MWhW3y5BLqvY0HhHu`;
- semantic isolation `1OMNpcH4mydZu_Sb-z2h77N2hivuwLwrp`;
- semantic diff `11kgk3lTBn_Nsm5bJ1Af-OwDZQqqefZ-_`;
- Evidence-FINAL `18-SpQLwYEmopX7uD7BLNqC_uzJXJYvhZ`, SHA-256 `6e7a165b4ac6651387870c9cd7d336338b2356e502736556ef3f7547328856fa`;
- SHA256SUMS-FINAL `1ZnPqsmLpKjBpOqOg3sxbFVqCO_PRRRCm`, SHA-256 `ea8bf2ec285fd5250eea861325e40a167415a3e07eaa01bdc458a12d2bddfc86`.

Provider readback verified exactly **22 files = 20 payloads + 2 closure files**.

**HF22 formal acceptance: PASS.**

## Next blocker

HF23 is mandatory. The nine Zombie lower-damage bodies directly called by the restored `Zombie.TakeDamage` remain damaged: tokens `0x06000456` through `0x0600045E` (`Hurt_Armor1`, `Hurt_Armor2`, `Hurt_Artillery`, `Hurt_Ashes`, `Hurt_Body`, `Hurt_FinalDamageReduction`, `Hurt_Normal`, `Hurt_Real`, `Hurt_Throughout`). Native preview/evidence is already archived in the HF22 Drive folder.
