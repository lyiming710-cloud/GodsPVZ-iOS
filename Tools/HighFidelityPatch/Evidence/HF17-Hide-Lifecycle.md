# HF17 — Buff.Awake + Hide lifecycle native recovery

Date: 2026-09-08  
Branch: `high-fidelity`  
Input: formal HF16 final `57100e6b9296f5a6c1ff4710cc2859810dcfff9992a27ea5238000729c3888c3`  
HF17 final candidate: `0eb0eba10cb27c5cff61e1a75947f146f5213ec036ff2ca3f95cf7d406ff1a67`

## Scope and reason

HF12/HF13 restored Buff update/end paths that directly depend on Hide lifecycle behavior. Native audit then showed that repairing only `Hide.Updata_Hide()` and `Hide.End_Hide()` would leave Hide initialization incomplete: original PC `Buff.Awake(bool child)` contains the Hide-start behavior inline, while the Cpp2IL-managed `Buff.Awake` is badly broken. HF17 therefore restores the complete managed-observable lifecycle:

- `Buff.Awake(bool child)` — RID 240, token `0x060000F0`, PC `0x180310710`.
- `Hide.Awake_Hide()` — RID 254, token `0x060000FE`, PC `0x18031C530`.
- `Hide.Updata_Hide()` — RID 255, token `0x060000FF`, PC `0x18031C730`.
- `Hide.End_Hide()` — RID 256, token `0x06000100`, PC `0x18031C620`.

Primary evidence is the original PC `GameAssembly.dll` plus original `global-metadata.dat`. PC GameAssembly SHA-256 is `9ebd7ca996a5b03fb4a766f7a2502b660d581ddbbb36af4d7bf2f06a211da39d`; metadata SHA-256 is `ad992341add498bd1018ac980f42171e0737b0793bf945c8714ad70a445b36b9`.

## Native ranges and xrefs

`.pdata` fixes the logical bodies:

- `Buff.Awake`: `0x180310710–0x18031097B`.
- `Hide.Awake_Hide`: `0x18031C530–0x18031C61C`.
- `Hide.End_Hide`: `0x18031C620–0x18031C728`.
- `Hide.Updata_Hide`: five contiguous runtime functions covering `0x18031C730–0x18031C8F4`; the next Hide constructor starts at `0x18031C900`.

Executable-section direct `E8` scan:

- `Awake_Hide`: zero direct E8 xrefs; `Buff.Awake` carries the same Hide-start logic inline.
- `End_Hide`: two direct xrefs at `0x180429FC9` and `0x18042A2A1`, from HF13 `Buff.End` shared specializations.
- `Updata_Hide`: two direct xrefs at `0x18042AA11` and `0x18042B074`, from HF12 `Buff.Update` shared specializations.

## Original fields and runtime offsets

`Hide` TypeDef 5119:

- `hide`, token `0x04000115`, bool, `+0x58`.
- `zombie`, token `0x04000116`, `Zombie`, `+0x60`.

`Buff` TypeDef 5115:

- `duration`, token `0x04000107`, float, `+0x1C`.
- `childBuffs`, token `0x0400010D`, `List<Buff>`, `+0x48`.

`Zombie` TypeDef 5219:

- `fX` / `fY`, tokens `0x040005AD/AE`, `+0x30/+0x34`.
- `invincible`, token `0x040005D8`, `+0xBE`.
- `hide`, token `0x040005F0`, `+0xF9`.
- `waitingTime`, token `0x040005F6`, `+0x114`.

## Call attribution

Native type/method usages resolve to:

- `UnityEngine.Component.GetComponent<Renderer>()`.
- `UnityEngine.Object.op_Implicit(Object)`.
- `UnityEngine.Renderer.set_enabled(bool)`.
- `UnityEngine.Component.get_transform()`.
- `UnityEngine.Transform.set_position(Vector3)`.
- `Zombie.SetColor(Color)`, original Assembly-CSharp RID 1142 / token `0x06000476`.
- `List<Buff>.GetEnumerator()` plus `Enumerator.MoveNext/get_Current/Dispose`.
- `System.Math.Clamp(float,float,float)`; the original native float clamp target `0x18030E0E0` also initializes `System.Math.ThrowMinMaxException`, closing the ownership/signature attribution.

`UnityEngine.Color::.ctor(float,float,float,float)` original PC target `0x180E74A30` is four float stores only. `Vector3(float,float,float)` was already independently proven in HF16 as direct field stores only. Therefore using the managed value-type constructors does not add observable behavior.

## Exact constants

Native raw IEEE-754 values close the constants used here: `1.0`, `-0.8`, `0.5`, `0.8`, `0.2`, `0.3`, `4.0`, and `1000.0`. `Awake_Hide` writes immediate `0x3CA3D70A`, i.e. `0.02f`, to `Zombie.waitingTime`.

## Restored managed-observable behavior

`Hide.Awake_Hide()`:

- when `hide` is true: set `zombie.invincible=true`, set `zombie.hide=true`, return;
- otherwise get `Renderer`, enable it when Unity object truthiness succeeds, set `zombie.hide=false`, set `zombie.waitingTime=0.02f`.

`Hide.Updata_Hide()`:

- compute `t = duration + duration` and manually clamp it to `[0,1]` with ordered float comparisons, preserving native NaN behavior;
- `hide=false`: `rgb = 1 + (-0.8*t)`, `alphaBase = 0.5-duration`;
- `hide=true`: `rgb = 0.2 + (0.8*t)`, `alphaBase = duration`;
- `alpha = System.Math.Clamp(alphaBase*4, 0.3, 1)`;
- construct `Color(rgb,rgb,rgb,alpha)` and call `zombie.SetColor(color)`.

`Hide.End_Hide()`:

- `hide=false`: set `zombie.invincible=false`, return;
- `hide=true`: get Renderer and disable it if truthy, then set `zombie.transform.position = Vector3(zombie.fX,zombie.fY,1000)`.

`Buff.Awake(bool child)`:

- when `child=false`, enumerate `childBuffs`; a null child raises `NullReferenceException`; call `child.Awake(true)`; preserve Enumerator `Dispose` in one finally handler;
- independently, when `this is Hide`, dispatch to the now-native-backed `Hide.Awake_Hide()`.

The last dispatch is managed-observable equivalent to the PC native's inlined Hide-start block and avoids duplicating an already independently reconstructed body.

## Patcher / formal-input validation

HF17 patcher project is `Tools/HF17Patch`. The first published tool exposed only a MemberRef-discovery issue for a Color constructor and did not produce a candidate. The corrected tool explicitly creates the already-native-proven Color/Vector3 constructor references.

- corrected patcher source commit: `693c87be6f639cbbf507f787710fa348d7606c2c`;
- corrected CI run: `34217812188` — success;
- corrected artifact SHA-256: `97f63e050ce1356ff73dec6f1f4a25ea0245f6475315fd55e869de6ca13fd046`;
- formal HF16 input was re-fetched from its Drive final and re-hashed to `57100e6b9296f5a6c1ff4710cc2859810dcfff9992a27ea5238000729c3888c3`;
- formal patch output and a second application to that same HF16 input are byte-identical.

Patcher reopen:

- `Buff.Awake`: `39 IL / 129 bytes / 1 finally`.
- `Hide.Awake_Hide`: `33 IL / 110 bytes / 0 EH`.
- `Hide.Updata_Hide`: `66 IL / 234 bytes / 0 EH`.
- `Hide.End_Hide`: `37 IL / 134 bytes / 0 EH`.

## Permanent Cecil validation

Permanent `Tools/RecoveryAudit/Program.cs` was extended through HF17 in commit `3ff0a81c1944988d5eab132840678aa76d61b23d`. Workflow `34218022069` succeeded. Running that published auditor against the candidate gives OPEN1 and OPEN2 passes for the cumulative recovery chain, including all four HF17 targets, ending in `RECOVERY_AUDIT_OK`.

## ILSpy / whole-assembly isolation

ILSpyCmd exact version `11.0.0.9375` was run with the full fresh PC Cpp2IL reference directory reproduced from fixed Cpp2IL source commit `5fb20304df698ffd3d0e664b2a698cd911dc9d57`.

- `Hide` type readback stderr: 0.
- `Buff.Awake` member readback stderr: 0.
- whole HF17 IL stderr: 0; complete rerun exited 0 and was byte-identical.

After normalizing only method RVA and ILSpy physical `I_XXXXXXXX` layout labels, HF16→HF17 changes exactly four declared managed MethodDefs. The unified diff contains two physical hunks because the three Hide MethodDefs are adjacent: hunk 1 ends at `Buff::Awake`; hunk 2 contains only `Hide::Awake_Hide`, `Hide::Updata_Hide`, and `Hide::End_Hide`. No fifth MethodDef changes. Normalized semantic diff SHA-256: `e669dddcb73c075906e0e5470025229deb7b28508b701c101d517f6f658b8606`.

## Fidelity classification

**Exact for managed-observable behavior.** Native branch order, field effects, Unity calls, floating-point arithmetic/constant values, null-child behavior, and Enumerator disposal are preserved. The only structural substitution is calling the separately reconstructed `Hide.Awake_Hide()` from managed `Buff.Awake` instead of duplicating the same PC-native inlined block; this has the same managed-observable behavior.

## Archive gate

Drive archival is the remaining gate at the time of this Evidence revision. Do not call HF17 final until the final DLL, patcher, native/metadata evidence, Cecil/ILSpy logs, semantic diff, provenance, SHA256SUMS, and this Evidence file are uploaded and independently listed.
