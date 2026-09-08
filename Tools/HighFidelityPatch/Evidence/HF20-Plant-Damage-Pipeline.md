# HF20 — Plant damage pipeline native recovery evidence

Date: 2026-09-08
Branch: `high-fidelity`
Classification: **Exact for managed-observable behavior**

## Formal result

Formal HF19 input SHA-256:

`e6e303c660b0351611370ebe29746954c5535344160c6e580d9314405b67f720`

HF20 cumulative final SHA-256:

`b16b3b89fad8f8081d86611a732696c56db6bd2dbab6302b6cef7fa94e141b3e`

HF20 restores exactly seven MethodDefs from the original PC x86-64 IL2CPP behavior:

1. `AttackRange.NewCircleRange<T>(T, Transform, float)` — RID 207, token `0x060000CF`, shared PC body `0x180426380`.
2. `AttackRange.NewCircleRange<T>(T, Transform, float, Vector3)` — RID 208, token `0x060000D0`, shared PC body `0x180426070`.
3. `Damage.AddElement(Element)` — RID 267, token `0x0600010B`, PC body `0x180310DC0`.
4. `Element::.ctor(ElementType, float, Damage, bool)` — RID 284, token `0x0600011C`, PC body `0x180316940`.
5. `Plant.GetATK()` — RID 871, token `0x06000367`, PC body `0x180350310`.
6. `Plant.GetDamage(Projectile, int, int)` — RID 877, token `0x0600036D`, PC body `0x180350E40`.
7. `Plant.GetDamageRange(Damage, Projectile, int, int)` — RID 881, token `0x06000371`, PC body `0x1803508A0`.

The two generic `NewCircleRange<T>` MethodDefs have zero direct definition pointers in the original CodeGen module; their shared PC bodies were attributed from native callsites. `Plant.GetDamage(Projectile,int,int)` has 24 direct native callers.

## Native-observable behavior

`NewCircleRange<T>` restores `AttackRange` construction, `transform`, one radius, one center, `RangeType.Circles`, and the independent Zombie → Plant → Projectile → Device parent type gates. The three-argument overload forwards a zero `Vector3` center to the four-argument implementation.

`Damage.AddElement` restores the original `List<Element>` foreach with Enumerator `finally`/`Dispose`: matching `Element.type` merges via `existing.point += incoming.point` and returns; otherwise the incoming element is appended.

`Element::.ctor` restores all four supplied values, including the original `shuttle_able` argument rather than forcing false.

`Plant.GetATK` restores the original rounding path:

`Math.Max(MathF.Round(buffManager.GetIncrement(attackPoint, "Atk") + attackPoint), 0f)`

The PC helper at `0x180003010` was independently disassembled and matches midpoint-to-even float rounding.

`Plant.GetDamageRange` restores the original range routing, including radii `80`, `155`, `160`, `195`, `225`; projectile IDs 28/10/11 and 29/30/21; Plant IDs 2/4/49; actual `specialType`; Skill ID 1 center offset `targetPosition - transform.position`; and Skill ID 3 UI range selection.

`Plant.GetDamage(Projectile,int,int)` restores the two native damage jump tables, the actual `specialType` forwarding into `GetDamageRange`, damage-attribute/penetration/anti-air/anti-submarine/no-residue flags, the native active multipliers including `0.2`, `8`, `1.2`, `2.8`, `0.3`, `1.15`, `2.5`, `4.5`, `1.4`, and ID 5 Element creation with `shuttle_able=true`.

## Patcher and repeatability

The first compiled patcher stopped before output because it required an already-existing `UnityEngine.Vector3.op_Subtraction` MemberRef. No candidate was produced by that attempt. Source was then repaired to construct the static MethodRef explicitly:

- source-repair commit `a2a38eb8f0b943e28534bfb4ab0b31f935fd0be2`;
- corrected normal-build trigger commit `557e2e54bd4229f688f6be546a77791d5a90b3d0`;
- corrected CI run `34235308941` — success;
- corrected artifact SHA-256 `38275ef0a2831c5e1498155558b48ba4ca2151da3f67ec877c801e5b2bb3caed`.

Formal HF19 was re-fetched from its accepted Drive archive and re-hashed before patching. Two independent applications produced byte-identical HF20 output.

Reopen measurements:

- `NewCircleRange/3`: 8 IL / 23 bytes / 0 EH
- `NewCircleRange/4`: 58 IL / 216 bytes / 0 EH
- `Element::.ctor`: 15 IL / 38 bytes / 0 EH
- `Damage.AddElement`: 47 IL / 158 bytes / 1 finally
- `Plant.GetATK`: 13 IL / 45 bytes / 0 EH
- `Plant.GetDamageRange`: 172 IL / 581 bytes / 0 EH
- `Plant.GetDamage/3`: 428 IL / 1457 bytes / 0 EH

## Independent validation

Permanent RecoveryAudit was extended through HF20 in commit `ca3bad1e0669d3019646125cbee726dbbb8828e7`. Workflow run `34235553691` succeeded. The published auditor was executed against HF20 and OPEN1/OPEN2 both ended `RECOVERY_AUDIT_OK`.

ILSpyCmd is fixed at `11.0.0.9375` with the full reproduced 56-DLL Cpp2IL reference set from source commit `5fb20304df698ffd3d0e664b2a698cd911dc9d57`. All seven target readbacks exited 0 with zero stderr. Whole-assembly HF19 and HF20 readbacks also exited 0 with zero stderr.

After normalizing only physical method RVA and `I_XXXXXXXX` address labels, HF19 → HF20 changes exactly the seven declared MethodDefs, with no eighth managed method change. Semantic diff SHA-256:

`5094a498d6998ae8a60040ffb3791f909ec2e5987428ebdbdccee42ef5b1e358`

## Drive acceptance

Archive folder `HF20-Plant-Damage-Pipeline`:

- folder ID `15ZPNk4Zb5LuSdZ8RrueUGpYowmo9AFGP`;
- cumulative DLL ID `1VmdyyZzfA4rmjZHeMrttHXYfgXPqnDBZ`;
- corrected patcher ID `1N9_ExWfDO8JD_GoVfz_xAB4UHEtom69p`;
- semantic diff ID `1kJnnIfa0s4CwOloz12facfj1Q5WSA9iY`;
- Cecil audit ID `1cdg7MpM5JUfh94shIKi10EGekGIG1Zcn`;
- metadata/native evidence ID `1njQbEyZUxS-U_xizHJGxCJEG8SnePaWQ`.

Because the available Drive path does not support in-place overwrite for stored ordinary files, the initial closure Evidence/SHA256SUMS pair is intentionally retained as immutable **superseded drafts** rather than silently replaced. The authoritative closure pair is:

- `HF20-Plant-Damage-Pipeline-Evidence-FINAL.md` — Drive `1T52CgV39NdDPo3V2QLJMzxWAy6TVUTIF`, SHA-256 `330bf328d0e1b6b4212fc7be3e94da2e6b27d21afc6a7db3839d0ba4bac582da`;
- `HF20-SHA256SUMS-FINAL.txt` — Drive `1uppHfrvURT0S5rqqc-iBygcvL5eSr0iC`, SHA-256 `eaba67da6b73ae15802ab1b850774860d64997f0a21eaefad0983341c021729b`.

After uploading those two FINAL closure files, the provider folder was independently re-listed and contained exactly **29 expected files**: 25 immutable payload files, two retained superseded closure drafts, and the two authoritative FINAL closure files.

**HF20 formal acceptance: PASS.**

## Next decision gate

HF20 deliberately excludes unrelated ProjectileManager damage. The next active-path audit is the ProjectileManager core, especially `CrateNewProjectile(int)`, `Start()`, and `SetFloatScale()`. Native scope must close before any HF21 patcher is written. If no remaining critical active-path cluster survives that audit, managed recovery should stop and the project should move to Unity `2022.3.44f1c1` validation rather than adding low-value HF stages.