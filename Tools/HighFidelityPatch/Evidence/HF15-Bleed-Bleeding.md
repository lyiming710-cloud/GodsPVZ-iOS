# HF15 — Bleed.Bleeding<T> shared generic recovery

## Target
- Managed definition: `Bleed.Bleeding<T>(T target)`.
- Original MethodDef RID: `251`.
- Original token: `0x060000FB`.
- Original Assembly-CSharp CodeGenModule direct definition pointer: `0`.
- Formal input: final HF14 `cbe30c99a973f41bbf4bb0f0f56f62d37e1c752c0723159cc07feeb3d2a0efe7`.
- Primary source: original PC `GameAssembly.dll` + original `global-metadata.dat`.
- Confidence: **Exact for managed-observable behavior**.

## Generic instance attribution and native boundary
HF12 `Buff.Update<T>` contains the direct PC callsite `0x18042B02E -> 0x180428C90`, identifying the shared reference-type generic body used for `Bleed.Bleeding<T>`.

A full executable-section (`.text` + `il2cpp`) `E8 rel32` scan finds exactly one direct xref to `0x180428C90`, namely that HF12 callsite.

PE exception-table / `.pdata` coverage fixes the logical body at `0x180428C90–0x180428E64` as four runtime-function regions:
- `0x180428C90–0x180428CE7` — unwind `0x181A991F4`
- `0x180428CE7–0x180428DF8` — unwind `0x181A99200`
- `0x180428DF8–0x180428E59` — unwind `0x181A99220`
- `0x180428E59–0x180428E64` — unwind `0x181A9923C`

## Original metadata / MetadataRegistration evidence
The original PC metadata and `MetadataRegistration` identify the host gates and field layouts used by the native body.

### Type-info usages
- global `0x181B9CF08` contains encoded usage `0x20006A91` -> Il2CppType index `13640` -> class TypeDef index `5206` -> `Device`.
- global `0x181BB1FC8` contains encoded usage `0x200083DB` -> Il2CppType index `16877` -> class TypeDef index `5207` -> `Plant`.
- global `0x181BC4620` contains encoded usage `0x20009E7B` -> Il2CppType index `20285` -> class TypeDef index `5219` -> `Zombie`.

### Fields and runtime offsets
`Bleed` TypeDef index `5118`:
- `value` — token `0x04000112`, `System.Single`, `+0x58`.
- `minHealthPoint` — token `0x04000113`, `System.Single`, `+0x5C`.
- `multi` — token `0x04000114`, `System.Boolean`, `+0x60`.

The PC target reads `value` and `multi`; it **never reads `minHealthPoint`**.

Host health fields:
- `Zombie.healthPoint` `0x040005BD` `+0x70`; `maxHealthPoint` `0x040005BE` `+0x74`.
- `Plant.healthPoint` `0x040004B7` `+0x94`; `maxHealthPoint` `0x040004B8` `+0x98`.
- `Device.healthPoint` `0x04000474` `+0x74`; `maxHealthPoint` `0x04000475` `+0x78`.

### `Time.deltaTime`
All three host branches independently call PC target `0x18132C2F0`. Original `UnityEngine.CoreModule.dll` CodeGenModule attribution resolves it as:
- RID `2370`
- token `0x06000942`
- `UnityEngine.Time.get_deltaTime()`

This attribution uses original metadata + the original CoreModule CodeGenModule, not Cpp2IL-rewritten MethodDef numbering.

## Recovered native-observable behavior
1. If reference-type generic `target` is null, return.
2. Independently test `target` as `Zombie`.
   - If `multi == false`, damage rate = `value`.
   - If `multi == true`, damage rate = `zombie.maxHealthPoint * value`.
   - Read current `healthPoint`, call `Time.deltaTime`, subtract `deltaTime * damageRate`, write `healthPoint`.
3. Independently test `target` as `Plant` with the same formula using Plant fields and a **new, independent** `Time.deltaTime` call.
4. Independently test `target` as `Device` with the same formula using Device fields and a **third independent** `Time.deltaTime` call.
5. Return.

The native body has no `minHealthPoint` clamp/gate, no ordered/unordered floating comparison, no explicit NaN branch, no Unity `Object` lifetime comparison, no enumeration/finally, and no managed EH region.

## HF15 patcher
Repository target: `Tools/HF15Patch`.

The patcher modifies only `Bleed.Bleeding<T>` and validates after reopen that:
- token remains `0x060000FB`;
- body has no `Cpp2ILHelpers` calls;
- there are exactly three `Time.get_deltaTime` calls;
- `multi` is read three times;
- `value` has six branch-path read sites;
- `minHealthPoint` is never read;
- Zombie/Plant/Device each have exactly one `maxHealthPoint` read, one `healthPoint` read and one `healthPoint` write;
- there are three independent `isinst` host gates;
- no exception handler is present.

HF15 patcher CI workflow run: `34201832021` — **success**.
CI artifact: `GodsPVZ-HF15Patch-linux-x64`.
Artifact archive SHA-256: `1f0dbcbfa1e9f72437517d0b3a9eef41397827cd0ce79ace26eed9c7110f0598`.

## Formal HF14 -> HF15 application
Formal HF14 input SHA-256 was rechecked immediately before patching:
`cbe30c99a973f41bbf4bb0f0f56f62d37e1c752c0723159cc07feeb3d2a0efe7`.

Patcher reopen on output:
- `Bleed.Bleeding<T>`: `93 IL / 317 bytes / 0 EH`.

A second application from the same formal HF14 input produced a byte-identical DLL.

Final HF15 SHA-256:
`87c74aea233372a5a7af28c0df244a18873af111c0230b54d9acf82e68ef9ec1`.

## Permanent Cecil regression
`Tools/RecoveryAudit/Program.cs` was extended through HF15.

Recovery-audit workflow run `34201989968` — **success**.
Actual audit against the final HF15 DLL:
- OPEN1 passes all permanent HF3–HF15 targets;
- GC / finalizers;
- OPEN2 passes all permanent HF3–HF15 targets;
- final result: `RECOVERY_AUDIT_OK`.

HF15 remains `93 IL / 317 bytes` on both opens.

## Reproducible Cpp2IL reference set
The prior HF12 reference provenance was made fully reproducible for HF15.

A dedicated workflow checks out the authoritative Cpp2IL source commit:
`5fb20304df698ffd3d0e664b2a698cd911dc9d57`.

Successful fixed-tool workflow run: `34202655285`.
The locally rebuilt binary reports `Cpp2IL 2022.1.0+5fb20304df698ffd3d0e664b2a698cd911dc9d57`; the historical `development.1736` text is upstream CI build metadata, while the full commit is the authoritative source identity.

Running that tool against the original PC game with `dll_il_recovery` reproduces:
- 56 reference DLLs;
- `2318 / 2319` Assembly-CSharp methods recovered;
- sole failure: `Zombie::InjuryStatusUpdate_Body` (`Stack state not settling`), already closed in HF3.

Reproduced reference ZIP SHA-256:
`fe091e5a5389c2491eebeff72c83c394097bef67ffd45a25cf7de01c9aad9ef1`.

## ILSpy independent readback
ILSpyCmd fixed version: `11.0.0.9375`.

With the freshly regenerated 56-assembly reference directory:
- member token `0x060000FB` decompiles with stderr = `0`;
- no relevant `Unknown result type`;
- no generic type error;
- no overload error;
- decompilation is strongly typed as `Zombie`, `Plant`, and `Device` and preserves three independent `Time.deltaTime` uses.

The readback is equivalent to:
- `multi ? host.maxHealthPoint * value : value`;
- `host.healthPoint -= Time.deltaTime * amount`;
for each independent host type gate.

## Whole-assembly semantic isolation
Whole-assembly IL for formal HF14 and HF15 was regenerated with ILSpyCmd `11.0.0.9375` and the same complete reference directory. Both stderr streams are zero.

After normalizing only physical serialization changes:
- `// Method begins at RVA 0x...`;
- `.data cil I_XXXXXXXX` / `I_XXXXXXXX` physical labels;
- `<PrivateImplementationDetails>` physical data-address labels;

the HF14 -> HF15 diff contains exactly **one semantic hunk**. That hunk ends at:
`Bleed::Bleeding`.

No second managed method changes semantically.

## Drive archive
Final archive directory:
`PVZ GOD/HighFidelity-Recovery-2026-09-08/HF15-Bleed`

Drive folder ID:
`1y7WGFVI7q1RGZjedzqj2hIWpT-58GhbV`

The final archive contains the final DLL, CI patcher artifact, PC native disassembly, this Evidence, ILSpy readback, Cecil log, semantic diff, metadata/xref evidence, patcher run log, ILSpy zero-stderr record, fixed-reference provenance/generation log, reproduced reference set, and SHA256SUMS.

## Final classification
HF15 is **Exact for managed-observable behavior**. The host types, field identities/offsets, `multi` branch, damage arithmetic, independent `Time.deltaTime` calls, null behavior and absence of `minHealthPoint` logic are all backed by PC native + original metadata/MetadataRegistration evidence. No speculative gameplay logic is introduced.
