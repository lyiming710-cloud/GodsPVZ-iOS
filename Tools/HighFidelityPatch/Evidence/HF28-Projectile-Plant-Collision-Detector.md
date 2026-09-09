# HF28 — Projectile Plant Collision Detector native recovery evidence

Date: 2026-09-09  
Branch: `high-fidelity`  
Classification: **Native-backed active-path projectile plant-collision recovery**

## Formal result

Formal HF27 input SHA-256:

`18d9efebdcfcccbbe6c805a9276c4427b4e6baa33673fb71b96a04245bb0e8de`

HF28 cumulative candidate/final payload SHA-256:

`8e342bcf856b638d551e286034a5acf32d46a8a07e35e48333f480499bd705dd`

HF28 restores exactly one MethodDef:

- `0x060003E3 Projectile.CollisionDetect_Plant(bool sameCamp)` — RID `995` — PC `0x1803799B0`.

No Device/Zombie detector or adjacent `Collision_Plant` MethodDef is modified in HF28.

## Why HF28 is required

After HF27 formal acceptance, `CollisionDetect_Plant` remained a direct target of the recovered `Projectile.CollisionDetect()` dispatcher and its HF27 managed body still contained twelve Cpp2IL helper references plus concrete invalid float/object/reference comparisons and unmanaged-memory placeholders. PC native provides a compact, independently closable body; Plant resolution is inlined in this detector, so expanding scope into adjacent E8 is not justified.

## Original PC native behavior

Original MethodDef RID-1 -> Assembly-CSharp CodeGenModule methodPointers attribution gives RID 995 -> `0x1803799B0`.

Accepted behavior:

- enumerate `board.plantManager.plants` with enumerator disposal;
- ordered X overlap: `(fW + plant.fW) * 0.5f > abs(plant.fX - fX)`;
- ordered Y overlap: `(fD + plant.fD) * 0.5f > abs(plant.fY - fY)`;
- require `((plant.camp == camp) == sameCamp)`;
- require `plant.fH > fZ - fH * 0.5f` and `fZ + fH * 0.5f > 0f`;
- first matching Plant wins;
- IDs `{10,11,15,26,27,28,29,30,31,32}` -> `damage.AreaDamage()`; otherwise `Plant.TakeDamage(damage,this)`;
- then `Collision_AudioParticle()`;
- IDs 19/23 preserve the projectile; all other IDs call `DestroyProjectile()`;
- accepted hit returns true; no matching Plant returns false.

Native-observed Projectile offsets include ID `+0x24`, camp `+0x28`, damage `+0x30`, fX/fY/fZ `+0x44/+0x48/+0x4C`, fW/fD/fH `+0x54/+0x58/+0x5C`, board `+0xE8`. Native-observed Plant offsets include fX/fY `+0x30/+0x34`, fW/fD/fH `+0x40/+0x44/+0x48`, camp `+0xBC`.

## Patcher / deterministic formal application

HF28 patcher source is under `Tools/HF28Patch/`.

- source head `e00f4453f8426f3872aa8517eb941a687c7c6172`;
- `Program.cs` blob `1194dfc65ff5bce73af65ec79c84a362284551ea`;
- `Template.cs` blob `448c70c8f92f8e8bb7a69f5bd66962f5f747c6a0`;
- csproj blob `f46a36708ced0cbde720551eb5744cfcba65847f`;
- workflow blob `6c69ccd72ee2f483777dee699f281d19bbe8bd8a`;
- workflow run `34341642401` success;
- patcher artifact SHA-256 `06875e9e7abda4211bc922ab073545eaa8e500ed06b1b7fd9d883ef578a95fa5`.

Formal HF27 was re-fetched from accepted Drive ID `1eS4hAmbRZKOO6oVzS8Tj7n1arpbconP6` and re-hashed before patching. Two independent applications are byte-identical at HF28 SHA `8e342bcf856b638d551e286034a5acf32d46a8a07e35e48333f480499bd705dd`.

Cecil reopen both runs: E3 `127 IL / 339 bytes / 1 EH`; the EH is the foreach enumerator-disposal finally; zero Cpp2IL helper refs remain in the target.

## Permanent RecoveryAudit

RecoveryAudit extension commit `8fe91bf10daac53642fb27718563d5494c97198a`, Program blob `7f81da77ba2bab7af00840b7ae9b87285aae8e66`.

Workflow run `34342510687` success; published artifact SHA-256 `ab13b80d322826a47cd102d58000844c28d5fa5ca6a50ec01e6a4f87b1230183`.

Published auditor on HF28:

- OPEN1 `320 types / 2317 methods / 2297 bodies`, E3 `127/339`;
- OPEN2 identical;
- final `RECOVERY_AUDIT_OK`.

## Fixed ILSpy / semantic isolation

ILSpyCmd and ICSharpCode.Decompiler remain fixed at `11.0.0.9375` with the reproduced 56-DLL reference set.

Member readback: exit 0, stderr 0, zero Cpp2IL refs, zero issue markers.

- HF27 whole IL SHA `33260b8a10a7b51ccf19366420ef50bd3fd24c10dee91628db98107902f5b993`, reproduced exactly in the HF28 environment;
- HF28 whole IL SHA `dc734bc6f29e57a8a41ee7bf639bdf2efbad704d509e69ca2c2f9511c6200430`;
- MethodDef `2317 -> 2317`;
- normalized non-method skeleton byte-identical;
- exactly `0x060003E3` changed;
- semantic diff SHA `d32b9b4fc23040276b894b8498afc6f576c29c3c651de3da107c7172bc95eb42`.

The comparison implementation first reproduced accepted HF26->HF27 semantic diff SHA `7238e4a4873917787a976c3e9255ec2bb11176591dcc940ffc01e2374c10676f` byte-for-byte.

## Drive pre-closure acceptance

Folder `HF28-Projectile-Plant-Collision-Detector`, ID `1MG3S59l1pfSR9ewxESCHeJ24T-Wtd4d7`.

Key payload IDs:

- cumulative DLL `1T-UTvHpTZbhSs49DBjwWYz1qK_YCeOQF`;
- patcher `1Z-fgWwZWOiexSpLQdRZ7F7dFCWEbgfEW`;
- published RecoveryAudit `1QkrVV_qkgQJUFyXEsLImEi7bcpRl-73C`;
- fixed ILSpy `1p2iGbUW_KXQP2HMoxU3xPyfl8eSe2Uf8`;
- native evidence `1B_MOONS_Hdr9NtT4Lp5pz01T3w-ApI1S`;
- patch source `17NZG0XmWuwy2v-ztSTr76bcvWAx7B_mG`;
- semantic diff `1Cw7P5RzfWP_mt7BRmvs82NSTouQ3fbsc`;
- semantic isolation `1LDOPgj3gg4r6J4mV7UhmqBqUaBtkvVvj`;
- MethodDef table `1r8IeyQzW9IoH0yBq_A8W4rKfbfgu1XYV`;
- RecoveryAudit log `1_Ngv1VVw64GbhIgaxBb4FuUAs20RN5S1`;
- payload manifest `1nBJaowojqhyFoInLe6Uc7OePnjsqHVsF`, SHA-256 `38a8fb7790383323ded6535c0580754c235db0491ab009740cb9ace12a39d7ff`.

Provider pre-closure readback verifies exactly 20 payload files with `has_more=false`.

Closure IDs/hashes are appended after final Drive closure.

## Next decision gate

Do not automatically open HF29. After HF28 formal acceptance, rerun native-vs-managed active-path decision on `CollisionDetect_Device` and `CollisionDetect_Zombie`; do not include adjacent methods unless both concrete managed loss and direct business-path evidence require them. If no remaining candidate passes both gates, stop HF recovery and move to Unity/package validation.
