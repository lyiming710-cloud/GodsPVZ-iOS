# HF3 — Zombie.InjuryStatusUpdate_Body native recovery evidence

Date: 2026-09-08
Branch: `high-fidelity`

## Scope

HF3 replaces exactly one Cpp2IL failure body:

- Managed method: `Zombie.InjuryStatusUpdate_Body(bool noResidue)`
- Original MethodDef RID: 1119
- Original token: `0x0600045F`
- PC x86-64 native address: `0x1803652B0`
- PC native is authoritative; Android recovery was only auxiliary annotation where it agreed with PC native.

## Input / output

- HF2 input SHA-256: `a21c9e1d1e6994eb559694a51123d33b8a1eb3b262bda6d372f97bbf51523633`
- HF3 output SHA-256: `23014656af797490bc35d9feb8f78950bcfa79a167c6a1de3303e1d30251c32f`
- HF3 patcher executable SHA-256: `b69301117f315bc4c176caf7f301748140fb0234bf8f5bc1dbc734f6e37119bc`

## PC field offsets

| Offset | Field |
|---:|---|
| `+0x60` | `ID` |
| `+0x70` | `healthPoint` |
| `+0x74` | `maxHealthPoint` |
| `+0x80` | `brokenLevel` |
| `+0xB6` | `armBroken` |
| `+0xB7` | `isDying` |
| `+0xB8` | `isDied` |
| `+0xBE` | `invincible` |
| `+0xCC` | `nutZombie_hotNut` |
| `+0xD0` | `SPH_shootReady` |
| `+0xF4` | `IVH_skill3` |
| `+0xF5` | `IVH_skillOngoning` |
| `+0x120` | `prePath` |
| `+0x158` | `animator` |
| `+0x1B0` | `animationSprites` |

## Direct managed calls mapped from PC native

| PC address | Managed method |
|---:|---|
| `0x18035D0A0` | `Zombie.ArmBroken(bool)` |
| `0x180361A70` | `Zombie.HeadDrop(bool)` |
| `0x18035EDB0` | `Zombie.DropLootPiece()` |
| `0x18035E7E0` | `Zombie.Die(bool,bool)` |
| `0x1803659E0` | `Zombie.IsDisabled()` |
| `0x18035E080` | `Zombie.CreateStartPrePath()` |
| `0x180365EA0` | `Zombie.Path_Finding()` |
| `0x18036A8C0` | `Zombie.TranToWalk()` |
| `0x180371060` | `Zombie.ZC_NutHot()` |
| `0x180370D80` | `Zombie.ZC_NutBoom()` |
| `0x18031B790` | `GlobalStaticVars.GetAnimationSprite_Name(List<GameObject>,string)` |

External targets were independently identified as `System.Math.Floor(double)`, Animator `SetTrigger(string)`, `SetInteger(string,int)`, `SetBool(string,bool)`, Unity `Object.op_Implicit`, `GetComponent<SpriteRenderer>()`, and `SpriteRenderer.sprite` setter.

## Constants and strings

Exact PC constants used by this method: `0.2`, `0.25`, `0.4`, `0.6`, `0.8`, `1.0`, `3.0`, `4.0`.

Metadata-usage slots were decoded back to original literals, not guessed: `SkillTrigger3`, `BrokenLevel`, `Ready`, `Wallnut_body`.

## Native behavior

### Generic IDs

- `!armBroken && healthPoint < 2/3 * maxHealthPoint` -> `ArmBroken(noResidue)`.
- `!isDying && healthPoint < 1/3 * maxHealthPoint` -> `HeadDrop(noResidue)` then `DropLootPiece()`.
- Native death order is preserved: ordered `healthPoint <= 0` first, then `!isDied`, then `Die(noResidue,false)`.
- Return `IsDisabled()`.

### ID 12

Directly returns `IsDisabled()` and intentionally skips this injury/death body.

### ID 17 (SPH)

- Stage 0/1/2 is selected from full / below 2/3 / below 1/3 health.
- Only when Animator is a valid Unity object: set Animator integer `BrokenLevel`.
- At stage >=2 and old `brokenLevel < 2`: clear `prePath`, call `CreateStartPrePath()`, `Path_Finding()`, and `TranToWalk()` when path finding succeeds.
- Set Animator bool `Ready=false`, set `SPH_shootReady=false`, then write `brokenLevel=stage`.
- Shared native-order death check follows.

### ID 18 (IVH)

- `brokenLevel = floor((maxHealthPoint-healthPoint)*4/maxHealthPoint)`.
- If `brokenLevel >= 3 && IVH_skill3`: restore health to 25%, clear `IVH_skill3`; if Animator is valid, set `IVH_skillOngoning=true` and trigger `SkillTrigger3`; then set `invincible=true`.
- Shared native-order death check follows.

### ID 11 (nut zombie)

- `damageRatio = 1 - healthPoint/maxHealthPoint`.
- Above 60% damage, if not arm-broken: for damage <=80% call `ZC_NutHot()` and mark visual change, then `ArmBroken(noResidue)`.
- Above 80% damage, if not dying: `HeadDrop`, `DropLootPiece`, and if hot call `ZC_NutBoom()`.
- Preserve native ID11 death order: `!isDied` first, then ordered `healthPoint <= 0`.
- Visual bands are 0–20% -> sprite 19, 20–40% -> 20, >40% -> 21; hot state adds 3.
- Preserve unusual native writes exactly: in the 20–40% band, `brokenLevel != 1` causes `brokenLevel=0`; above 40%, `brokenLevel != 2` also causes `brokenLevel=0`.
- On visual change: find `Wallnut_body`, get `SpriteRenderer`, assign `ResourceManager.zombieSprites[index]`.

Ordered/unordered float branches were emitted to preserve PC COMISS/Jcc behavior, including NaN not spuriously entering damage/death thresholds.

## Validation gates

- HF3 patcher Actions run `34174963860`: success; native-order validation fix applied before build.
- RecoveryAudit Actions run `34174880608`: success; auditor includes `Zombie.InjuryStatusUpdate_Body/1` with a >=200 IL gate.
- Real apply from frozen HF2: success; final target body = 334 IL instructions, code size 1076 bytes.
- Mono.Cecil independent reopen: OPEN1 and OPEN2 both succeed; target token remains `0x0600045F`.
- ILSpyCmd `11.0.0.9375` independent decompile: exit 0, stderr 0 bytes, no decompiler anomaly markers.
- Full HF2/HF3 IL semantic-isolation check: after normalizing RVA/static-data placement addresses, exactly one diff hunk remains, solely `Zombie.InjuryStatusUpdate_Body`; all other method semantics are unchanged.

## Confidence

**Exact** for the recovered managed behavior of `Zombie.InjuryStatusUpdate_Body(bool)` relative to the observed GodsPVZ 1.0.2 PC IL2CPP native implementation.
