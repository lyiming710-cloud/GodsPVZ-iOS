# HF40 — Element UI Start Core

Date: 2026-09-10  
Branch: `high-fidelity`

## Scope

HF40 restores exactly one original `Assembly-CSharp` MethodDef and no others:

- `0x0600012B` — RID 299 — `ElementUIController.Start()` — PC `0x180315850`

Formal input is HF39 cumulative final:

`7c6360c79852f94a86e560367f25c79298d62d3e1bf86027f86112d9fc333f6e`

HF40 candidate/final-to-be-closed:

`726c7ceda476cb0034b9394964407a72d2ab7a1a92424dee9c81e118f5590cce`

## Original-native attribution and managed-loss gate

Fixed PC baseline remains:

- PC ZIP SHA-256 `2f08b4e2243e2bd296f3db694b4a2c9656252d76202c80167bdf3340b9f65b48`
- PC `GameAssembly.dll` SHA-256 `9ebd7ca996a5b03fb4a766f7a2502b660d581ddbbb36af4d7bf2f06a211da39d`
- PC `global-metadata.dat` SHA-256 `ad992341add498bd1018ac980f42171e0737b0793bf945c8714ad70a445b36b9`

Ordinary original RID-1 -> Assembly-CSharp CodeGenModule `methodPointers[index]` attribution resolves RID 299 to PC `0x180315850`.

HF39 fixed-ILSpy readback of `ElementUIController.Start()` contained a Cpp2IL decompiler-issue path for the native `JP` and an invalid managed comparison between `F4 progress` and `I4 0`. The high-level initialization intent was visible, but the emitted IL was not a valid native-faithful float comparison. `ElementUIController.Start()` is a live Unity lifecycle entrypoint for the element UI introduced in the already-formal HF39 runtime cluster, so it satisfies the active runtime + concrete managed-loss gate.

## Accepted native behavior

PC x86-64 at `0x180315850` establishes exactly:

1. Read `progress` and compare with `+0.0` using `UCOMISS`.
2. `JP` or `JNE` returns immediately. Thus every nonzero value and NaN/unordered returns; only ordered `+0.0` or `-0.0` enters initialization.
3. For `back1`, `image1`, `back2`, `image2`, independently: if the component reference is non-null, resolve `component.gameObject` and call `SetActive(false)`.
4. Null component references are skipped. A non-null component whose `gameObject` resolution reaches null follows the normal Unity/native null-throw path.
5. No other field/state is changed.

HF40 emits `ldc.r4 0` + `bne.un` so NaN/unordered follows the native return path rather than being normalized into an ordered comparison.

## Deterministic patching

Accepted published patcher build head:

`a5b337cc391264ff44a5655e9241b23e3cb9afec`

Workflow run: `34433570862` PASS  
Artifact ID: `10135362791`  
Artifact ZIP SHA-256:

`55929a360a706af6de10972389dbaad1fe6078ea4f5a60841603a5cb7bc8dbc6`

Two independent applications to the SHA-verified HF39 formal input were byte-identical at:

`726c7ceda476cb0034b9394964407a72d2ab7a1a92424dee9c81e118f5590cce`

Both runs had zero stderr.

Cecil reopen:

- `0x0600012B ElementUIController.Start/0` — `41 IL / 133 bytes / 0 EH`
- no Cpp2IL helper remains in the target
- no issue-marker string remains in the target

Fixed ILSpyCmd / ICSharpCode.Decompiler `11.0.0.9375` with the reproduced 56-DLL reference set passed the target with exit 0, stderr 0, Cpp2IL refs 0, invalid-comparison markers 0 and warning/error markers 0. Readback is the expected `if (progress == 0f)` guarded four-image deactivation; native unordered semantics are carried by the emitted `BNE.UN` IL.

## Whole-assembly semantic isolation

HF39 whole IL was reproduced exactly at:

`23e2734766d033c4b22767fd9b969d8312261cedc24622a38bf6ff5dc5dcc54d`

HF40 whole IL SHA-256:

`fd43ae98ca25cc7a43f5265c76a924eaefb8fa8b4616575c6af914935fd0ff7e`

Before computing the new diff, the same retained parser/normalization algorithm reproduced the accepted HF38->HF39 semantic diff byte-for-byte at:

`c7c2b30f98986999cf5ed1de9ef800f044e796a2f4f1002c769cb7c0b57868e0`

HF39->HF40 results:

- MethodDef `2317 -> 2317`
- whole-IL method blocks `2139 -> 2139`
- normalized non-method skeleton byte-identical
- changed MethodDefs exactly `0x0600012B ElementUIController.Start`
- semantic diff SHA-256 `053387ddf76a053cc57cbca509bb72c653c5becea3d92e7c595be6a58b670d76`
- MethodDef table SHA-256 `4e7eda66bf694343784da18909d0cb38eaddd8674ae08a866943cbfaac4be7d1`

## Permanent RecoveryAudit

RecoveryAudit commit:

`f418fa6214ff6cce680f00178ad54f5614e816bb`

Workflow run: `34433765345` PASS  
Published auditor artifact ID: `10135428591`  
Auditor artifact SHA-256:

`add5be4fafd3e2284ce0d76222b52a0b2a666418475ab59ccd8db5fdb914ebfb`

Fixed ILSpy artifact ID: `10135430229`, SHA-256 `446bb884aa014ae04d6825ad6430e85dc1a827d2526075bbde754c9f85b3a1da`.

Independent published-auditor execution passed the full retained audit set twice. `ElementUIController.Start/0` was `41 IL / 133 bytes` in OPEN1 and OPEN2, stderr was zero, and execution terminated with `RECOVERY_AUDIT_OK`.

## Source provenance

Accepted published patcher source is pinned to immutable build head `a5b337cc391264ff44a5655e9241b23e3cb9afec`:

- `HF40Patch.csproj` blob `79974692d8f6d45e828142d3a48d691b7c5da648`
- `Program.cs` blob `f4ec624601f653a5909d49a7dac8da56120debfe`
- workflow blob `63af6112c382fdb42978a80dbbb46f79ad170fd3`

The patcher directly emits the native-backed CIL; there is no separate Template source file for HF40.

## Drive pre-closure archive

Folder: `HF40-Element-UI-Start-Core`  
Folder ID: `1k_tnoO9Rx_BCVpkzdr7-tp06HDziPTXf`

Key provider IDs:

- cumulative DLL `1bX-nH1QtTSzz-Qs15FnSzFngZSuu3VfW`
- patcher `171MOULuKaECQ7N2g6dseJe9JFjHTvZlW`
- fixed ILSpy `10kpgT63ekJYp2YE2gr67GB6LvNKVWbok`
- RecoveryAudit `12FexP1NlnyhiOssIJC6IF1KyypUM0GYj`
- semantic diff `1aMJtXPdU7Sn7evbCUa1NZIEchLQnwsii`
- Cecil reopen `1WdvsUEKCru8qGMv9cNgW053A6rlyD9k9`
- member summary `1F6QMQSqAnSJtHc8qsiRnTj-aHj2_qktn`
- member ZIP `1BHr5VjmpW_gD5nSlZw9xFIBnQnYoyAFl`
- whole provenance `1kB3Oe0sDhrFt9dW2y7AVbV7hKaCbXwFQ`
- MethodDef table `1sjRgKm-bo62_TLsYUig9KlJ-Affi4qNt`
- RecoveryAudit log `1D7T35fK2vxRKTVajASXVw3unvi6hhttv`
- native evidence `1QBUHjkYM_HoA0ozLhtq4S-RcFWlkb9HL`
- patch source `1PWkMflW7c-faLShjyF8f90c8VXP9lgVS`
- formal run1 `1cqLRJAkAIBiY8tpxhYHYun_N5V25NOpK`
- formal run2 `1o_xERO9ZryPiGvkdraiGfWC6e7wXxNgk`
- reference provenance `1xBqBGrs2jlmQ-fcuFAwOyn1ynvFG3Y_e`
- semantic isolation `1suwG8sZjkDcvT6TJb_qlC9QxPgZspF6f`
- source/build provenance `1IBxygplLk_tkrvlJbH3iRQNnxp2DCrl4`
- target manifest `1mAbLfljr9aeGPecKUKBaMQi2dY5he9bZ`
- payload manifest `1uMrfEq6xgH07bUYc8RWC0V7Y52_lFvz6`

Payload manifest SHA-256:

`0f68c8c8c632124058ca6a00b1cd082e10ad5a1bc10a584a529bbc9f42ad5cb6`

Provider readback returned exactly 20 pre-closure files. HF40 is not formal until Evidence-FINAL and SHA256SUMS-FINAL are added, the folder is read back as exactly 22 files, and `Recovery/STATUS.md` is advanced only afterward.
