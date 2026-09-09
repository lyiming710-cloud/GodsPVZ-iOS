# HF29 — Projectile Zombie Collision Detector native recovery evidence

Date: 2026-09-09  
Branch: `high-fidelity`  
Classification: **Native-backed active-path projectile zombie-collision recovery**

## Formal result

Formal HF28 input SHA-256:

`8e342bcf856b638d551e286034a5acf32d46a8a07e35e48333f480499bd705dd`

HF29 cumulative candidate/final payload SHA-256:

`8c13a6638276a0e201ee39192545bbbe73703a7251f4188080dcfcf0c4d55590`

HF29 restores exactly one MethodDef:

- `0x060003E4 Projectile.CollisionDetect_Zombie(bool sameCamp)` — RID `996` — PC `0x180379D00`.

No Device detector or adjacent resolution/helper MethodDef is modified in HF29.

## Why HF29 is required

After HF28 formal acceptance, `CollisionDetect_Zombie` remained a direct target of the recovered `Projectile.CollisionDetect()` dispatcher and its HF28 managed body still contained concrete Cpp2IL helper calls, invalid object/float reconstruction and control-flow placeholders. PC native provides an independently closable body and the downstream `Collision_Zombie` resolution handler was already formally restored in HF27.

## Original PC native behavior

Original MethodDef RID-1 -> Assembly-CSharp CodeGenModule methodPointers attribution gives RID 996 -> `0x180379D00`.

Accepted behavior:

- for IDs other than 19/23, enumerate `board.zombieManager.zombieList` with enumerator disposal;
- require `Zombie.CanAttacked()`, camp/sameCamp match, ordered X/Y AABB overlap and Z/H overlap;
- the first ordinary matching Zombie calls `Collision_Zombie(zombie)` and returns true; no match returns false;
- IDs 19/23 use indexed iteration and `Zombie.GetPredictedPosition(0f)` for current-frame horizontal position;
- current-frame COMISS/JAE rejection semantics are kept distinct from previous-frame overlap semantics;
- previous-frame horizontal overlap uses `zombie.previousPosition` versus projectile `previousPosition` with strict half-extent comparisons;
- a persistent previous-frame overlap is skipped; only a new contact calls `Collision_Zombie`;
- after a special-path collision, `gameObject.activeSelf` is checked and iteration stops if the projectile was deactivated;
- the special path returns true iff at least one new collision was dispatched.

## Patcher / deterministic formal application

HF29 patcher source is under `Tools/HF29Patch/`.

- source head `8436a5667cb783f07de57055b77c8ed932da7421`;
- `Program.cs` blob `ef3b928d77b3ced25e3480b51caf303e7f64b4dd`;
- `Template.cs` blob `6a77de5eefc0e0567560bc856c8e3923ac74e3d8`;
- csproj blob `0b0194bb13953c779e4b087509a06134ae204299`;
- workflow blob `c2656b2332e4d0f22a3307f9a40e71fb5124d244`;
- workflow run `34346510664` success;
- patcher artifact SHA-256 `83632ffd83dcc4c335926101fa757414458edf2f7b87ca93eb575e7690e34d67`.

Formal HF28 was re-fetched from accepted Drive ID `1T-UTvHpTZbhSs49DBjwWYz1qK_YCeOQF` and re-hashed before patching. Two independent applications are byte-identical at HF29 SHA `8c13a6638276a0e201ee39192545bbbe73703a7251f4188080dcfcf0c4d55590`.

Cecil reopen both runs: E4 `252 IL / 727 bytes / 1 EH`; the EH is the ordinary foreach enumerator-disposal finally; zero Cpp2IL helper refs remain in the target.

## Permanent RecoveryAudit

RecoveryAudit extension commit `76a97ca21c9bd973f76bb658ead398ad63175749`, Program blob `fa61f68fd56c22dc4793bf996cddbc74be4dabd3`.

Workflow run `34347012719` success; published artifact SHA-256 `43285cc5a7e47c65817fcb3e4d9b85756c413a875370f50ec621e3cdf2aef2ac`.

Published auditor on HF29:

- OPEN1 `320 types / 2317 methods / 2297 bodies`, E4 `252/727`;
- OPEN2 identical;
- final `RECOVERY_AUDIT_OK`.

## Fixed ILSpy / semantic isolation

ILSpyCmd and ICSharpCode.Decompiler remain fixed at `11.0.0.9375` with the reproduced 56-DLL reference set.

Member readback: exit 0, stderr 0, zero Cpp2IL refs, zero issue markers.

- HF28 whole IL SHA `dc734bc6f29e57a8a41ee7bf639bdf2efbad704d509e69ca2c2f9511c6200430`, reproduced exactly in the HF29 environment;
- HF29 whole IL SHA `4baa4fc78ed28ca7ed903e3c80895a358adb9eb2ab25d77ee0cc09a1549953a4`;
- MethodDef `2317 -> 2317`;
- normalized non-method skeleton byte-identical;
- exactly `0x060003E4` changed;
- semantic diff SHA `86f00e9e65ae591a29dd63f0c796bca08bec29e887f14fb8f3ced56e239e3052`.

The comparison implementation first reproduced accepted HF27->HF28 semantic diff SHA `d32b9b4fc23040276b894b8498afc6f576c29c3c651de3da107c7172bc95eb42` byte-for-byte.

## Drive pre-closure acceptance

Folder `HF29-Projectile-Zombie-Collision-Detector`, ID `1O6A2gXb983Z9-JDQWrI6DNTH27E05Xyb`.

Key payload IDs:

- cumulative DLL `1oQ8nG22Y4isLLlx8p9IkREwRDql0TVg1`;
- patcher `19YVeHpF2OQNWQcY72ovwit8PfpmXEPw5`;
- published RecoveryAudit `13Y4SQ-E6ddzUbEYwbz_xPU9K_KLWNi0I`;
- fixed ILSpy `14P_ve19i1cUMO9P2fYQwNZ5GRuCj7EvK`;
- native evidence `1jPVmFMVcF5SOo472PmMGgOJWYjiTxzBW`;
- patch source `1uKaryWdIYImp7pUE0b_QhZADTDg0a5Xa`;
- semantic diff `1Di_zhDBalS2TH9J4nh-KG-dgN-a8oN0H`;
- semantic isolation `1Fu2gvdiRVid2d9-3mYGvSjEf7mTy9FUa`;
- MethodDef table `1tZCMNCcvlxsZ83WIsVt5u67dq_2LDAdQ`;
- RecoveryAudit log `1nsPhEybHGYKCqIWgaD_k_lGiQPLNc6Ne`;
- payload manifest `1hc-k482JPIe0gRbDC2vJvZdbfpsPx-l3`, SHA-256 `e1fceb7193a29b84eb1492d9d90de7673f8bd958279c01f265163846f406e4a4`.

Provider pre-closure readback verifies exactly 20 payload files with `has_more=false`.

Closure IDs/hashes are appended after final Drive closure.

## Next decision gate

Do not automatically open HF30. After HF29 formal acceptance, rerun native-vs-managed active-path decision on `Projectile.CollisionDetect_Device` `0x060003E1` — PC `0x180379180`. A later HF stage is allowed only if original native evidence proves concrete managed loss and the method remains materially active. If no remaining candidate passes both gates, stop HF recovery and move to Unity/package validation.
