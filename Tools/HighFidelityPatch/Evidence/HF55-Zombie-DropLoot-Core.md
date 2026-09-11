# HF55 — Zombie DropLoot Core

HF55 restores exactly one formal managed MethodDef:

- formal token `0x06000438` / RID 1080 / `Zombie.DropLootPiece()`.

Formal input is the audited HF54 cumulative DLL only:

- HF54 SHA-256: `307dc20c09a6338ecf4a4917b489cc0303d61c79a6aafeb34011e3e67961af2c`.
- HF55 candidate SHA-256: `dc205a40dc2478b3aacbb3a7d6bb1ca96ffb0a964648d34062b4ddc75f3b3655`.

## Original source revalidation and identity reconciliation

The original Google Drive source packages were re-materialized and rehashed before HF55 attribution:

- PC ZIP SHA-256 `2f08b4e2243e2bd296f3db694b4a2c9656252d76202c80167bdf3340b9f65b48`.
- PC `GameAssembly.dll` SHA-256 `9ebd7ca996a5b03fb4a766f7a2502b660d581ddbbb36af4d7bf2f06a211da39d`.
- PC `global-metadata.dat` SHA-256 `ad992341add498bd1018ac980f42171e0737b0793bf945c8714ad70a445b36b9`.
- Android APK SHA-256 `428e0ba2e46645a905fb9fbb00cfde42406727a3ddfce9a7df1e0875889a739f`.
- Android arm64 `libil2cpp.so` SHA-256 `cfa13d53d7c3e218221a90c5fb615a012339393774af3160ff1c17f3826c61a6`.
- Android armv7 `libil2cpp.so` SHA-256 `3d035afbf3e419a0d48947ec460eb910731727b3e0da5a2ee83e20650668fadb`.
- Android metadata SHA-256 `e7a4412e3af25da3ba2c806d3c691ec68d00c1e7915d8fb6843c3a82422f5005`.

The three identifier spaces are deliberately kept separate:

- formal HF54/HF55 recovery DLL: `0x06000438` / RID 1080 / `Zombie.DropLootPiece()`;
- original-PC fixed-Cpp2IL generated reference: `0x0600041E` / RID 1054 / same name and signature;
- Android-arm64 fixed-Cpp2IL generated reference: `0x0600041E` / RID 1054 / same name and signature.

Cpp2IL regenerated tokens are not used directly as original native pointer indexes. Name/signature/neighbor reconciliation plus native evidence is required.

## Concrete reconstruction damage

Before HF55, both the formal HF54 DLL and the original-PC fixed-Cpp2IL reference contain the same damaged managed reconstruction shape. The body calls `Cpp2ILHelpers.NoteDecompilerIssue` for invalid jump targets inside native `0x18035EDB0..0x18035F040` and loses the post-`shadow.transform.position` control flow.

Android arm64 independently reconstructs the missing high-level structure:

1. read `shadow.transform.position`;
2. gate on `isOnBoard`;
3. compute `Vector3 haedPosition = GetHaedPosition()`;
4. load `board.projectManager`;
5. call `ProjectManager.DropLootPiece(this, ...)`.

The Android generated IL is itself damaged at the final value-type argument and emits `ldc.i4 0`, decompiling as `(Vector3)0`. That value is not accepted as source truth; the original PC native ABI closes the operand.

## Original PC native closure

The original PC `Assembly-CSharp` methodPointers slot associated with the reconciled formal method resolves to `0x18035EDB0`; the next method begins at `0x18035F040`. The PC fixed-Cpp2IL error markers independently reference native addresses `0x18035EFD8`, `0x18035EFD2`, and `0x18035EE8D`, tying the managed method to this same native range.

Direct disassembly of `0x18035EDB0..0x18035F040` proves:

- `this+0x1F0` (`shadow`) is read and transform/position getters execute;
- `this+0xBD` (`isOnBoard`) is tested and false returns;
- the head-position computation produces a `Vector3` at stack `rsp+0x20`;
- `this+0x20` (`board`) is loaded, then `board+0x100` (`projectManager`);
- immediately before the call, `r8 = &Vector3`, `rdx = this Zombie`, and `rcx = projectManager`;
- `0x18035EFCD` calls `0x180323260`, already closed in HF54 as `ProjectManager.DropLootPiece(Zombie,Vector3)`;
- the function returns directly after the call.

Therefore the real final argument is the computed head-position `Vector3`, not zero/default.

The restored managed behavior is consequently:

```csharp
_ = shadow.transform.position;
if (isOnBoard)
{
    Vector3 haedPosition = GetHaedPosition();
    board.projectManager.DropLootPiece(this, haedPosition);
}
```

The apparently unused initial position getter is preserved because both original PC native and Android managed reconstruction execute it.

## Deterministic patch gate

Corrected HF55 patcher build:

- source/build head `e9be71217fad3c1d36d4ea8a0ff1de2053cba472`;
- workflow run `34600412954` PASS;
- artifact ID `10264430775`;
- artifact SHA-256 `372809225401b203da6244f08148f9242cd206cd7ed6853aa10d275cbc99bb0d`.

Two independent executions from the locked HF54 input produce byte-identical output:

- output SHA-256 `dc205a40dc2478b3aacbb3a7d6bb1ca96ffb0a964648d34062b4ddc75f3b3655`;
- both stderr 0 bytes;
- Cecil reopen: 19 IL instructions / 52 bytes / 0 exception handlers.

A provisional pre-formal candidate was explicitly discarded because its template declared `Zombie.shadow` as `GameObject`, causing ILSpy to expose an artificial `((GameObject)(object)shadow)` cast. The corrected template declares the actual formal/original field type `Transform`; fixed readback is clean and calls `Component.get_transform()`.

## Fixed ILSpy and whole-assembly isolation

Fixed tooling:

- ILSpyCmd / ICSharpCode.Decompiler `11.0.0.9375`;
- fixed 56-DLL Cpp2IL reference ZIP SHA-256 `fe091e5a5389c2491eebeff72c83c394097bef67ffd45a25cf7de01c9aad9ef1`;
- ILSpyCmd archive SHA-256 `446bb884aa014ae04d6825ad6430e85dc1a827d2526075bbde754c9f85b3a1da`.

Target member readback has stderr 0 and no target-local Cpp2IL / Unknown / NotImplemented / Expected / invalid markers. Its C# readback is the clean behavior shown above.

Whole-assembly checks:

- HF54 whole IL baseline SHA-256 reproduced exactly: `2b342d49e118eb70c16137132270ddd37672e268d69e517322797fdf6bd43f42`;
- HF55 whole IL SHA-256: `e0e4dc29327bf63ee0b48fdecd7ccd071dfa1fd9f0f62194512d49cd3d6b0c9c`;
- MethodDef blocks: `2317 -> 2317`;
- after normalizing physical RVA/data-label relocation, the non-method skeleton is identical, SHA-256 `8c48e2535749c44abf5e696553cd25f1fc352425d1603d9135b67bb942ee10c9`;
- changed method blocks exactly one: block/RID 1080, `Zombie.DropLootPiece`;
- normalized HF54->HF55 semantic diff SHA-256 `05d10d12823183a4f15c4645a0081b9709fbea8f238f53f6e1314ec490b5c62c`;
- HF55 MethodDef-table SHA-256 `44bf60f800146ee195d07383c00988f7e41a82ae3ada1a67cfe7c4bcb2d1ecb2`.

## Cumulative audit

Corrected cumulative auditor build:

- build head `5448fe17bf6a85a30d871a0cbd6c7f9374d5437c`;
- workflow run `34600623192` PASS;
- artifact ID `10263064154`;
- artifact SHA-256 `37644faaf7984cc6d80a5e2faae8c86aca1cbbef5796babf7431bc1b12da4a2c`.

The auditor locks the exact HF55 candidate SHA, retains the prior cumulative targets including the complete HF54 ProjectManager drop implementation, and additionally verifies the exact HF55 body (19 IL / 52 bytes / 0 EH), required fields/calls, call order, local materialization of `GetHaedPosition`, and reloading of that local into `ProjectManager.DropLootPiece`.

Two independent actual composite executions pass both the historical RecoveryAudit and HF55 cumulative audit, ending `RECOVERY_AUDIT_OK` and `HF55_AUDIT_OK`, with zero stderr. The two composite logs are byte-identical; log SHA-256 is `abad1e0674b4358fb9012efa51d01b800b74dfd44489f818afee221a62bec679`.

## Scope boundary

HF55 modifies only formal `0x06000438 Zombie.DropLootPiece()`.

It does **not** claim repair of:

- `Zombie.Path_Test()`;
- `Zombie.DestroyZombie()`.

`Zombie.Path_Test()` remains a large damaged method whose formal HF54 and original-PC Cpp2IL high-level reconstructions are identical and whose native closure is substantially larger. `Zombie.DestroyZombie()` did not present the same clean minimal reconstruction-loss boundary during this gate. Either requires a separate original-native-backed stage or the project should proceed to the deterministic Unity Stage9.1 gate if no minimal next target closes cleanly.