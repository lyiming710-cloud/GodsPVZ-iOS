# HF31 — Projectile Rotating Runtime Core native recovery evidence

Date: 2026-09-09  
Branch: `high-fidelity`  
Classification: **Native-backed active-path projectile runtime recovery**

## Formal result

Formal HF30 input SHA-256:

`6ccccf685950edba428b51dbe3aa95e511caa467d134dd4202939e19ef655c24`

**HF31 formal cumulative SHA-256:**

`a4d001581430fe440e50feb37f3abd189a1ae86a00f4fe41114ae3f9b99386d2`

HF31 restores exactly one MethodDef:

- `0x060003F6 Projectile.Rotating()` — PC `0x18037BE20`.

No neighboring Projectile MethodDef is modified.

## Native behavior and managed-loss proof

`Projectile.Update()` directly calls `Rotating()` in the active per-frame runtime path. HF30 managed output still contained concrete scalar/object/native reconstruction damage. Original PC native gives an independent logical body at `0x18037BE20–0x18037C0F0`; the next MethodDef `SetDamage` begins at `0x18037C0F0`.

Accepted behavior:

- `Projectile.speed` is `UnityEngine.Vector3`; native `[this+0x88]` reads `speed.x`;
- negative or unordered/NaN `speed.x` selects Y local Euler angle `180f`; otherwise `0f`;
- recovered managed expression is `speed.x < 0f || float.IsNaN(speed.x)` to preserve native COMISS/JAE unordered semantics;
- if `angularAcceleration != 0f`, `angularSpeed += Time.deltaTime * angularAcceleration`;
- if `angularSpeed != 0f`, `projectileSprite` and `projectileAnimation`, when present, rotate around `(0,0,1)` by `Time.deltaTime * angularSpeed` in `Space.World`.

## Formal patcher and rejected outputs

Final HF31 source head `a99d10701b2746972d0b5ebcedadad64a1fdba70`:

- `Program.cs` blob `b69fdaa9164f699c1ee4c2b553ff71740e6bbd18`;
- `Template.cs` blob `db609788eaf6e3b75888dcf8ea8274b53b489e1b`;
- csproj blob `ccda9e8d512d71620155b3b8c78f940bfe5a9e26`;
- workflow blob `6de0c19c896694d5c44dc996ee559e68c4e540a5`.

Two earlier outputs were explicitly rejected and never propagated:

1. `999355e8720441b5b412b6fd8c6ed94cf6eb7d2763f1915179996c7907623174` — fixed ILSpy exposed invalid O/F4 comparison;
2. `db881c9745193ff623553eabef2c083ec10831a76496e322328ee4668debce0c` — fixed ILSpy proved the template had misdeclared real `Projectile.speed : Vector3` as `float`.

Final workflow run `34371494005` PASS; patcher artifact SHA-256 `9433d5beb14d9f7f38731d52b269def671533dff414d008585f49d0feafaa150`.

Formal HF30 was re-fetched from accepted Drive ID `1sjYzcOCDcRu7adJ-paQUKb95uHeMdTac` and SHA-verified before patching. Two independent final applications are byte-identical at HF31 SHA `a4d001581430fe440e50feb37f3abd189a1ae86a00f4fe41114ae3f9b99386d2`.

Cecil reopen both runs: `81 IL / 277 bytes / 0 EH`; zero Cpp2IL helper refs remain.

## Permanent RecoveryAudit

RecoveryAudit commit `15799da87b106b2679ac6a402ae671adfb74f5f7`; Program blob `240be0d91c60fda24c8e1db549209d08ed3f4776`.

Workflow run `34372146093` PASS; published artifact SHA-256 `6eaab051eb29d3a47360cfa0436af05642090ee07164a4ec951d25331721fa65`.

Published auditor:

- OPEN1 `320 types / 2317 methods / 2297 bodies`, Rotating `81/277`;
- OPEN2 identical;
- `RECOVERY_AUDIT_OK`.

## Fixed ILSpy / whole assembly / semantic isolation

Fixed ILSpyCmd / ICSharpCode.Decompiler `11.0.0.9375`, reproduced 56-DLL reference set.

Member readback: exit 0, stderr 0, zero Cpp2IL refs, zero issue markers.

- reproduced HF30 whole IL SHA `4fe6bb80fe5de582c67c3e14427b526c244ab5f6dc9cba7cad0ce28970f2136e`;
- HF31 whole IL SHA `cef4f8e34d87fb1409488b9e915eb8df39b1d4e699612032192e4be83be810ee`;
- MethodDef `2317 -> 2317`;
- normalized non-method skeleton byte-identical;
- exactly `0x060003F6` changed;
- semantic diff SHA `673a14dcd0b0e3bfa6b1b16bcaa09fed4d5d435b744f04ca406534125fc26638`.

The comparison implementation first reproduced accepted HF29→HF30 semantic diff SHA `59b9b5b2a1d7fcaf098b9185eea13ca8bb977183fcf4a8b8da25f2384d4735ba` byte-for-byte.

## Google Drive formal closure

Folder `HF31-Projectile-Rotating-Runtime-Core`, ID `1yJ8yC_nn7DdCPEgvhnnLBwcPY01HWvSm`.

Key provider IDs:

- cumulative audited DLL `18fn9_hdgHdKXQRMJfIcf0lfDkbwuZdam`;
- patcher `1Z1o2wWJsVXN356OcxZ9eBIe46loBTRCO`;
- published RecoveryAudit `1UzPYxEIWpuQY8Gi3oi__gT8trjEG7enS`;
- fixed ILSpy `1D_KEu4afQ03cXftXyi9EPc-u3oXGj9uU`;
- native evidence `1kAYoWXZ-Zp0JKzWb19G4jJwg3TECqXCR`;
- patch source `1NvICqdBiJvSEm82-ZPNcMlwXmCJ1BDGy`;
- semantic diff `1Uio3rgy51rQEpmaCTqzDeyg9ybaXxJRB`;
- semantic isolation `13zt28nXa9-7UqTGazEWSRtTq18EbeBdN`;
- MethodDef table `1rDkoxkTGucry6NXYkcotz24TlWyp6id3`;
- RecoveryAudit log `16_6rMl06O3RUH_oEQINfSObCMzsX5lKz`;
- payload manifest `1_CWYlvylVsd76foHRN1vaT6_zhbomfIe`, SHA `dc6bc000da460c94af069361bd8682c58b40d05e70edafab4a4196c9f18ee4c8`;
- Evidence-FINAL `1wMiI37IqCcp-IYN8IspFl1l4_kvUC_Gw`, SHA `e971903841c245052b66cc37d265948f4c3b5c535d25baa55271f3260a8ca50d`;
- SHA256SUMS-FINAL `1Ikzx_u7IfS1y88uhqq9KEkhA75Ph6-Az`, SHA `94fa970a0a175ad9546529aff1246d0fa66e924f9c5e592be8d1ccb10b008556`.

Provider final readback: `has_more=false`, exactly **22 files = 20 payloads + 2 closure files**.

**HF31 formal acceptance: PASS.**

## Next decision gate

Do not automatically open HF32. Re-scan remaining native-vs-managed active runtime helpers, especially `Update_Tracking`, `SetEulerAngles` and `Aim`. A later HF stage is permitted only where original PC native/metadata proves concrete managed loss and the method is materially active in gameplay. Otherwise stop HF managed recovery and proceed to Unity/package validation, recovered Assembly-CSharp integration, compile/scene/gameplay validation, then only necessary iOS adaptation.
