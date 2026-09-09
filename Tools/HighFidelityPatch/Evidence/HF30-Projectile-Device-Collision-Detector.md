# HF30 — Projectile Device Collision Detector native recovery evidence

Date: 2026-09-09  
Branch: `high-fidelity`  
Classification: **Native-backed active-path projectile device-collision recovery**

## Formal result

Formal HF29 input SHA-256:

`8c13a6638276a0e201ee39192545bbbe73703a7251f4188080dcfcf0c4d55590`

HF30 cumulative final SHA-256:

`6ccccf685950edba428b51dbe3aa95e511caa467d134dd4202939e19ef655c24`

HF30 restores exactly one MethodDef:

- `0x060003E1 Projectile.CollisionDetect_Device(bool sameCamp)` — RID `993` — PC `0x180379180`.

No neighboring Projectile MethodDef is modified in HF30.

## Why HF30 is required

After HF29 formal acceptance, `CollisionDetect_Device` was the final unrecovered direct target of the already-restored `Projectile.CollisionDetect()` dispatcher. Its HF29 managed body still contained eleven Cpp2IL helper references plus concrete invalid object/float/reference reconstruction. Original PC native provides a fully attributable active gameplay body, while downstream `Collision_Device`, damage dispatch and `Collision_AudioParticle` were already formally recovered in earlier HF stages.

## Original PC native behavior

Original MethodDef RID-1 -> Assembly-CSharp CodeGenModule methodPointers attribution gives RID 993 -> `0x180379180`.

Accepted ordinary path for IDs other than 19/23:

- enumerate `board.deviceManager.deviceList` with enumerator disposal;
- require `Device.CanAttacked(damage)`;
- require `(device.camp == camp) == sameCamp`;
- ordered X/Y AABB overlap plus Z/H overlap;
- first accepted Device calls recovered `Collision_Device(device)` and returns true;
- no accepted Device returns false.

Accepted ID 19/23 path:

- enumerate Device candidates with `CanAttacked` and camp/sameCamp checks;
- use `device.transform.position` for current-frame horizontal contact;
- retain the native COMISS/JAE current-contact behavior through the corresponding `>=` rejection tests;
- test current Z/H overlap;
- suppress persistent contact by comparing that same Device position against projectile `previousPosition` with strict half-extent comparisons;
- collect new Device contacts before resolving them;
- for each collected Device, IDs `{10,11,15,26,27,28,29,30,31,32}` use `damage.AreaDamage()`, otherwise `Device.TakeDamage(damage,this)`;
- then call recovered `Collision_AudioParticle()`;
- preserve the native `gameObject.activeSelf` break gate after each resolved contact;
- return true iff at least one new Device contact was collected.

## Patcher / deterministic formal application

HF30 patcher source is under `Tools/HF30Patch/`.

- source head `22081c57c5bd5219990bda0b6e6bb71374616bbf`;
- `Program.cs` blob `cb554cfac520fd22a8fffc44f845572fa42474c7`;
- `Template.cs` blob `5672acc911f1c733c09f7b161bd1d5660f414b79`;
- csproj blob `008a1126120325fc2c983aac7ef489051577bbb6`;
- workflow blob `3e4699e4e34e253af6c573faf45a4ecbd7112147`;
- workflow run `34362687707` success;
- patcher artifact SHA-256 `970255e57dbcf414c06fc3dd36303d2a4db12e2b930f669510c9571d930313b6`.

Formal HF29 was re-fetched from accepted Drive ID `1oQ8nG22Y4isLLlx8p9IkREwRDql0TVg1` and re-hashed before patching. Two independent applications are byte-identical at HF30 SHA `6ccccf685950edba428b51dbe3aa95e511caa467d134dd4202939e19ef655c24`.

Cecil reopen both runs: E1 `311 IL / 904 bytes / 3 EH`; zero Cpp2IL helper refs remain in the target.

## Permanent RecoveryAudit

RecoveryAudit extension commit `917087166a26893fce6ede5afefb02924206f44d`, Program blob `199fee548873e650c19054855f61cc0a9d143032`.

Workflow run `34364225011` success; published artifact SHA-256 `d6157d9ff1fb7c139fd989d217f929f38b0b07017de94f03d99d8ab6499101db`.

Published auditor on HF30:

- OPEN1 `320 types / 2317 methods / 2297 bodies`, E1 `311/904`;
- OPEN2 identical;
- final `RECOVERY_AUDIT_OK`.

## Fixed ILSpy / semantic isolation

ILSpyCmd and ICSharpCode.Decompiler remain fixed at `11.0.0.9375` with the reproduced 56-DLL reference set.

Member readback: exit 0, stderr 0, zero Cpp2IL refs, zero issue markers.

- HF29 whole IL SHA `4baa4fc78ed28ca7ed903e3c80895a358adb9eb2ab25d77ee0cc09a1549953a4`, reproduced exactly in the HF30 environment;
- HF30 whole IL SHA `4fe6bb80fe5de582c67c3e14427b526c244ab5f6dc9cba7cad0ce28970f2136e`;
- MethodDef `2317 -> 2317`;
- normalized non-method skeleton byte-identical;
- exactly `0x060003E1` changed;
- semantic diff SHA `59b9b5b2a1d7fcaf098b9185eea13ca8bb977183fcf4a8b8da25f2384d4735ba`.

The comparison implementation first reproduced accepted HF28->HF29 semantic diff SHA `86f00e9e65ae591a29dd63f0c796bca08bec29e887f14fb8f3ced56e239e3052` byte-for-byte.

## Drive formal closure

Folder `HF30-Projectile-Device-Collision-Detector`, ID `1JXbnqb-JQ7ofCyL26NpieGgIVkp1AUdF`.

Key payload IDs:

- cumulative DLL `1sjYzcOCDcRu7adJ-paQUKb95uHeMdTac`;
- patcher `1uPEcJcnx_tvfzrUFp8MYO4-WukwkiYHZ`;
- published RecoveryAudit `1Li2VnNyhHisScOfvGhsTymoPDbtgD1MB`;
- fixed ILSpy `1Z1l7hSzuByFFPLD_ZlnYYVJ6fup0yjAZ`;
- native evidence `137zp1Wp15xZqMMApJ_TWILvnvi10eBzs`;
- patch source `1IjLEFJGQM7NvofOYp1jeQHWIjp4hVQaf`;
- semantic diff `1R26qSvQ0wy22XhGmeFamssv_juijJBVj`;
- semantic isolation `1XZvVKZH0HJttU8kOfaEARyNe-gnwjxzh`;
- MethodDef table `1QYRjli3z6GUYsaSjamk2UdfAuwIsS1wF`;
- RecoveryAudit log `1m7eJsSgfEkQjWiTbM4_I5xDRVnEyh5e5`;
- payload manifest `1tn28Q06XCIOZV6obByGMjIwMY8ov79Lu`, SHA-256 `edd247bfbfe66bd6c1333a03e35988c649ad82e7f6a3e80437fab484ff916663`;
- Evidence-FINAL `1XGdnMedJ9VCP4xRJNZARZJJze5-OepCx`, SHA-256 `8ede9b565fa0e18fc43ad023c481715326d32936dd645ec5fba7388f681ed951`;
- SHA256SUMS-FINAL `1Gu2xYzt0B_jC7VJD7spznywlE_D6pG83`, SHA-256 `e5fa7dd034e1358680eb65450c2a669421b5426ba39aec1ecb8fd7561c649afe`.

Provider final readback verifies exactly `22 files = 20 payloads + 2 closure files`, `has_more=false`.

**HF30 formal acceptance: PASS.**

## Next decision gate

Do not automatically open HF31. HF30 closes the final unrecovered target of the restored `Projectile.CollisionDetect()` dispatcher. After HF30 formal acceptance, rerun a broad remaining native-vs-managed active-path scan. A later HF stage is allowed only if original PC native/metadata proves concrete managed loss and the method is materially active in gameplay. If no remaining candidate passes both gates, stop HF recovery and proceed to Unity/package validation, recovered Assembly-CSharp integration, compile/scene/gameplay validation and only then necessary iOS adaptation.
