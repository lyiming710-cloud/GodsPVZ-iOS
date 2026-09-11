# HF51 — Project Generic EndPosition Core

Status: native/MethodSpec-backed candidate validated through deterministic patching, Cecil reopen, fixed ILSpy, whole-assembly semantic isolation, and cumulative RecoveryAudit. Formal Drive/STATUS closure follows this evidence commit.

## Formal input

- HF50 SHA-256: `74ae15e2ed7f1626ecea7741d83803208ea0e8381306f1900f7a46c198c425cb`
- Original PC ZIP SHA-256: `2f08b4e2243e2bd296f3db694b4a2c9656252d76202c80167bdf3340b9f65b48`
- Original `GameAssembly.dll` SHA-256: `9ebd7ca996a5b03fb4a766f7a2502b660d581ddbbb36af4d7bf2f06a211da39d`
- Original `global-metadata.dat` SHA-256: `ad992341add498bd1018ac980f42171e0737b0793bf945c8714ad70a445b36b9`

## Authorized target

Exactly one MethodDef is authorized:

- `0x060003CA Project.SetEndPosition<T>(T)` / RID 970 / one method generic parameter.

No other MethodDef is authorized in HF51.

## Generic/native attribution

Ordinary RID-1 attribution is insufficient for this generic target, so HF51 uses original MethodSpec/generic-method-function evidence.

- Original metadata contains `Project.SetEndPosition<Zombie>` MethodSpec index `74551`.
- A live `ProjectManager.DropLootPiece` callsite loads hidden MethodInfo/RGCTX from PC global slot `0x181BBB770` immediately before calling the shared generic implementation.
- Slot `0x181BBB770` contains encoded metadata value `0xC002466F`.
- Metadata-v31 MethodRef decoding maps `0xC002466F` to MethodSpec `74551`.
- The generic method table maps the reference-type shared implementation used by this MethodSpec family to PC `0x1804A1AA0`.
- Therefore the observed live callsite is formally attributable to `Project.SetEndPosition<Zombie>` rather than inferred from calling context alone.

The shared native implementation at `0x1804A1AA0` closes the open generic behavior:

- null host exits;
- Plant branch uses Unity-object validity semantics;
- Plant + `movementTracks == 1`: `endPosition = plant.transform.position`, then Y `+= Random.Range(0f,15f)`;
- Plant + `movementTracks == 2`: `endPosition = new Vector3(plant.fX, plant.fY + plant.fZ, 0f)`, then Y `+= Random.Range(0f,10f)`;
- Zombie branch uses Unity-object validity semantics;
- Zombie + `movementTracks == 2`: `endPosition = zombie.transform.position`, then Y `+= Random.Range(0f,15f)`.

No defensive fallback or iOS-specific behavior is added.

## Deterministic patch validation

- HF51 patcher build head: `ef8472d0b89991c77e3a4bc75fd56f5660fedb4d`
- workflow run: `34555405016` PASS
- artifact ID: `10182364367`
- artifact SHA-256: `fef2fdce1afb800a0e1f5780b2405ccb7479054b8429e89b3c225095170d6213`
- two independent formal patches from the locked HF50 input are byte-identical;
- HF51 candidate SHA-256: `89e1961cefec6f8c532a0ca7cda4bd4afff2152c14f3856842ee44e5d7d7d90e`;
- Cecil reopen for `0x060003CA`: 1 generic parameter / 87 IL / 250 bytes / 0 EH.

## Fixed ILSpy / semantic isolation

- fixed ILSpyCmd / ICSharpCode.Decompiler `11.0.0.9375` with the locked 56-DLL reference set;
- target member exit 0, stderr 0;
- target-local Cpp2IL/Unknown/NotImplemented/invalid-type markers: 0;
- HF50 whole IL SHA-256: `0e9929692d9627b8a8e0f1261293ee2bb9cc69ed346165e96458e9ff544c6df3`;
- HF51 whole IL SHA-256: `0b153d64cc9ae3c49f9386f2dfc75a48db2485c788c2448bcf9160eb3d6c2a4a`;
- MethodDef count `2317 -> 2317`;
- emitted bodies `2297 -> 2297`;
- distinct nonzero body RVAs `2140 -> 2140`;
- normalized non-method/method-signature skeleton identical;
- exactly one normalized MethodDef block changes: RID 970 / `0x060003CA Project.SetEndPosition`;
- semantic diff SHA-256: `e7c88166a41f65c7f871780ea018fc321baa429395a822225cd6c9f8fb93af56`;
- HF51 MethodDef table SHA-256: `7c6788afae725b36397b0e3a3143a3c446e948df8aea61212bf976a5f6258a7d`.

## Cumulative RecoveryAudit

- HF51 cumulative audit build head: `f0d1a40b33eda7449fcd00b04a486100f6506d83`
- workflow run: `34557272214` PASS
- artifact ID: `10183019307`
- artifact SHA-256: `84e2174133ee0167abd9736764f870250cce76b4d24fcedd239c51ff808f8c3c`
- retained historical auditor independently covers HF1-HF46;
- cumulative HF51 auditor locks the HF51 whole-file SHA and rechecks all 16 HF47-HF51 late-stage targets;
- `SetEndPosition<T>` additionally requires exactly one generic parameter;
- two independent actual executions both end `RECOVERY_AUDIT_OK` + `HF51_AUDIT_OK`, stderr 0;
- byte-identical audit log SHA-256: `05ce0727d1539a7665e155156a3ce6480283f1286d4d9d600005d26b0f4c141d`.

## Residual boundary

HF51 closes only `Project.SetEndPosition<T>`.

Still outside HF51 and not claimed repaired:

- `Zombie.Path_Test()` — 2 explicit Cpp2IL reconstruction-loss calls;
- `Zombie.DropLootPiece()` — 5;
- `Project.SunSet(int)` — 1;
- broader `GameFail / DestroyZombie / DropLootPiece` path remains subject to later native-backed gates.

HF51 must not be described as closing the entire drop/destroy or zombie-path state machine.
