# HF16 — AttackRange.TestInRange<T> native recovery

## Target and formal input
- Target: `AttackRange.TestInRange<T>(T target)`.
- Original RID: 216; token: `0x060000D8`.
- Original Assembly-CSharp CodeGenModule definition pointer: `0`.
- Formal cumulative input: HF15 SHA-256 `87c74aea233372a5a7af28c0df244a18873af111c0230b54d9acf82e68ef9ec1`.
- Primary evidence: original PC `GameAssembly.dll` plus original `global-metadata.dat`.
- Final classification: **Exact for managed-observable behavior**.

## Shared generic body attribution
The PC shared body is `0x180426DD0–0x180427102`. PE `.pdata` divides it into three contiguous runtime-function ranges:
- `0x180426DD0–0x180426E52`
- `0x180426E52–0x180427049`
- `0x180427049–0x180427102`

A full executable-section `E8 rel32` scan finds 18 direct xrefs to `0x180426DD0`. Fourteen are directly attributable to original non-generic MethodDefs, including `Damage.AreaDamage_*`, `Device.EnemySeeking_Zombie`, multiple `Plant.EnemySeeking*` paths, `Plant.ST_Chomperviking`, and `SkillManager.Updata10`; two more occur in shared body `0x18042A800–0x18042AEF4`, and two occur inside the HF12-native-backed `Buff.Update<T>` body at `0x18042AF00–0x18042B473`. This is strong evidence that the recovered target must remain the shared generic definition rather than a single Zombie/Plant specialization.

## Original metadata and runtime field offsets
Original TypeDef index 5111 identifies `AttackRange`. PC `MetadataRegistration` is `0x1818C6D00`; its `fieldOffsets` table is at `0x181B5E000`. The runtime instance layout is:
- `transform +0x10`
- `plant +0x18`
- `zombie +0x20`
- `projectile +0x28`
- `device +0x30`
- `rangeType +0x38`
- `rects +0x40`
- `radius +0x48`
- `centers +0x50`
- `ID +0x58`

The four native host gates and their geometry fields decode as:
- `Device` TypeDef 5206: `fX +0x38`, `fY +0x3C`, `fW +0x48`, `fD +0x4C`, `fH +0x50`.
- `Plant` TypeDef 5207: `fX +0x30`, `fY +0x34`, `fW +0x40`, `fD +0x44`, `fH +0x48`.
- `Projectile` TypeDef 5212: `fX +0x44`, `fY +0x48`, `fW +0x54`, `fD +0x58`, `fH +0x5C`.
- `Zombie` TypeDef 5219: `fX +0x30`, `fY +0x34`, `fW +0x40`, `fD +0x44`, `fH +0x48`.

The native target reads `fX`, `fY`, `fW`, and `fD` exactly twice per matched host. It does not read `fZ` or `fH`. In particular, the second size axis is `fD`, not `fH`.

Metadata-usage decoding independently identifies the four type gates:
- `0x181B9CF08`, encoded `0x20006A91` -> type index 13640 -> TypeDef 5206 -> `Device`.
- `0x181BB1FC8`, encoded `0x200083DB` -> type index 16877 -> TypeDef 5207 -> `Plant`.
- `0x181BB38E0`, encoded `0x20008613` -> type index 17161 -> TypeDef 5212 -> `Projectile`.
- `0x181BC4620`, encoded `0x20009E7B` -> type index 20285 -> TypeDef 5219 -> `Zombie`.

## Exact string and direct call targets
The Plant gate performs the target body's only log call before reading Plant geometry fields:
- usage global `0x181BA4B68`, encoded `0xA00045F3` -> string literal index 8953 -> exact UTF-8 `检测植物是否在范围内`.
- type usage `0x181B9AD48`, encoded `0x2000680D` -> `UnityEngine.Debug`.
- native target `0x1812E68F0` -> original `UnityEngine.Debug.Log(object)` token `0x06000209`, RID 521.

The two range primitives map directly through the original Assembly-CSharp CodeGenModule:
- `0x1803000A0` -> `AttackRange.TestInRects_Rect(Rect)`, token `0x060000D7`, RID 215.
- `0x1802FFAE0` -> `AttackRange.TestInCircles_Position(Vector3)`, token `0x060000D4`, RID 212.

## Geometry and read order
Native constant `0x1815A7A08` contains IEEE-754 `0x3F000000`, exactly `0.5f`.

For every matched Device/Plant/Projectile/Zombie host, the native-observable geometry is:
- `Rect(fX - fW * 0.5f, fY - fD * 0.5f, fW, fD)`
- `Vector3(fX, fY, 0f)`

The recovered IL preserves the native field-read multiplicity and ordering: first `fW`, `fD`, `fX`, a fresh `fW`, a fresh `fX`, `fY`, then the x subtraction, a fresh `fY`, the y subtraction, and finally a fresh `fD`.

Using managed value-type constructors does not introduce a behavioral approximation. Original PC CoreModule native shows:
- `UnityEngine.Rect::.ctor(float,float,float,float)` token `0x0600027A`, RID 634, body `0x180E74A30...`: exactly four float stores then return.
- `UnityEngine.Vector3::.ctor(float,float,float)` token `0x06000666`, RID 1638, body `0x180F59910...`: exactly three float stores then return.

## RangeType dispatch
Original `RangeType` enum values are:
- `Null = 0`
- `Rects = 1`
- `Circles = 2`
- `Mixed = 3`
- `Unlimitied = 4` (original spelling)

Native control flow is:
- null target -> `false` immediately;
- Device gate -> geometry if matched;
- Plant gate -> exact Debug.Log, then geometry if matched;
- Projectile gate -> geometry if matched;
- Zombie gate -> geometry if matched;
- then read `AttackRange.rangeType` exactly once;
- `Null` -> false;
- `Rects` -> `TestInRects_Rect(rect)`;
- `Circles` -> `TestInCircles_Position(position)`;
- `Mixed` -> Rect test, short-circuit true, otherwise Circle test;
- `Unlimitied` -> true;
- any other enum value -> false.

A subtle native edge case is preserved: a non-null unsupported generic host does **not** return early. The zero-initialized `Rect` and `Vector3` remain in place and range-type dispatch still runs.

There is no managed exception region/finally, enumerator, UnityEngine.Object lifetime comparison, `Time.deltaTime`, NaN-special branch, or geometry clamp in this target.

## Patcher and reproducibility
- HF16 patcher project commit: `3fceb3b3fcb3d043ef551aae90c0de63a811277b`.
- HF16 patcher implementation commit: `1b87a7e1f9dc09d300f9813b6763e7929f9c5796`.
- HF16 CI workflow commit: `547ae3aee7445ec3ec1f592bb65cbe562bd3b8c8`.
- Patcher workflow run `34214727867`: **success**; publish, usage smoke test, and artifact upload all succeeded.
- Artifact `GodsPVZ-HF16Patch-linux-x64`, artifact ID `10051279966`, archive SHA-256 `88ddb21de04ab82a14cc9f69ed8c6eab2abdea04b3b748d13fbd5825fb6dddd2`.
- The formal HF15 input hash was rechecked immediately before patching as `87c74aea233372a5a7af28c0df244a18873af111c0230b54d9acf82e68ef9ec1`.
- Patcher reopen on the actual output reports `AttackRange.TestInRange<T>` = 250 IL / 971 bytes / 0 EH.
- Reapplying the CI artifact to the same formal HF15 input produces a byte-identical DLL.
- Final HF16 SHA-256: `57100e6b9296f5a6c1ff4710cc2859810dcfff9992a27ea5238000729c3888c3`.

## Permanent Cecil regression
`Tools/RecoveryAudit/Program.cs` was extended through HF16 in commit `3153427b77489449f4087895a71d2730eeb96775`.

Recovery-audit workflow run `34214966294` completed successfully. The actual auditor from that run was executed against the final HF16 DLL: OPEN1 and OPEN2 both pass every prior HF3–HF15 target plus `AttackRange.TestInRange<T>`, ending in `RECOVERY_AUDIT_OK`. HF16 remains 250 IL / 971 bytes in both opens.

## ILSpy independent readback
Using ILSpyCmd `11.0.0.9375` with the full 56-assembly PC Cpp2IL reference directory regenerated from exact Cpp2IL source commit `5fb20304df698ffd3d0e664b2a698cd911dc9d57`:
- member token `0x060000D8` decompiles with stderr = 0;
- the full `AttackRange` type decompiles with stderr = 0;
- there is no target-member Cpp2IL/decompiler warning;
- readback shows the four independent host gates, exact Plant log, exact `fX/fY/fW/fD` geometry, zero-position z, and the five-value `RangeType` dispatch.

The same reference provenance reproduces the established PC baseline of 2318/2319 recovered methods; the sole full Cpp2IL failure remains `Zombie::InjuryStatusUpdate_Body`, already closed by HF3.

## Whole-assembly semantic isolation
Whole-assembly IL was regenerated for formal HF15 and final HF16 with ILSpyCmd 11 and the same full reference directory; both stderr files are zero bytes.

After normalizing only physical placement artifacts already accepted in previous HF stages:
- method RVA comments;
- `.data cil I_*` labels;
- `<PrivateImplementationDetails>` `at I_*` physical labels;

the HF15 -> HF16 unified diff contains exactly **one** semantic hunk. It ends at `AttackRange::TestInRange`. No second managed method changes semantically.

## Final classification
HF16 is **Exact for managed-observable behavior**. Generic-body attribution, host gates, field identities and runtime offsets, field-read multiplicity/order, `0.5f` arithmetic, Plant log literal and call target, range helper targets, `RangeType` routing, unsupported-host behavior, and value-type construction semantics are all native- or original-metadata-backed. No guessed gameplay rule is introduced.
