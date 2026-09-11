# HF52 — Project Sun Scale Core

## Scope

HF52 restores exactly one managed MethodDef from the formal HF51 cumulative DLL:

- `0x060003CE Project.SunSet(int)` / RID 974.

No other MethodDef is authorized in HF52.

## Formal input / output

- formal HF51 input SHA-256: `89e1961cefec6f8c532a0ca7cda4bd4afff2152c14f3856842ee44e5d7d7d90e`
- HF52 candidate/final-candidate SHA-256: `1db686c32f24ea81660f3d18ccfd649ee7aeb5a40902e2042639e47355184b82`
- Assembly-CSharp MethodDef count: `2317 -> 2317`
- emitted bodies: `2297 -> 2297`
- distinct nonzero body RVAs: `2140 -> 2140`

## Original-PC native attribution

Locked original inputs:

- PC `GameAssembly.dll` SHA-256 `9ebd7ca996a5b03fb4a766f7a2502b660d581ddbbb36af4d7bf2f06a211da39d`
- `global-metadata.dat` SHA-256 `ad992341add498bd1018ac980f42171e0737b0793bf945c8714ad70a445b36b9`

Ordinary MethodDef attribution uses Assembly-CSharp CodeGenModule `methodPointers[RID-1]`.
For RID 974, index 973 resolves to PC native VA `0x180378750`.
The PE unwind table gives exact function range `0x180378750..0x1803787CC`.

Observed original native behavior:

1. store input `value` to `Project + 0x28` (`Project.value`);
2. convert the integer to float32;
3. divide with scalar float32 `divss` by the original constant at `0x1815A7B14`, bytes `00 00 48 42` = `50.0f`;
4. convert that float32 result to double;
5. call the already-attributed `System.Math.Pow(double,double)` path with exponent from `0x1815A7A18`, bytes `00 00 00 00 00 00 E0 3F` = double `0.5`;
6. execute native `cvtsd2ss` to convert the double result back to float32;
7. store it to `Project + 0x2C` (`Project.size`).

The managed reconstruction before HF52 had preserved the `Math.Pow` path but lost the native `cvtsd2ss`, emitted a Cpp2IL `NoteDecompilerIssue`, and wrote `0f` to `size`. HF52 restores the exact conversion chain instead of guessing a replacement formula.

Equivalent clean managed readback after HF52:

```csharp
public void SunSet(int value)
{
    this.value = value;
    size = (float)Math.Pow((float)value / 50f, 0.5);
}
```

## Reachability

This is not a dead-code cleanup. The formal cumulative assembly contains live calls to `Project.SunSet(int)`, including the zombie loot path that configures Sun drops with values `25`, `50`, and `100`.

## Patcher provenance

- patcher build head: `0f93223733b55b0a7a70d12eafa1f8c40a8e288e`
- workflow run: `34558616229` — PASS
- artifact ID: `10183500128`
- artifact SHA-256/digest: `55649525e05a065b3606b6f7599b01c2eedd9a808b02bd1706d7028a1f8745ee`
- patcher hard-locks the HF51 input SHA, token `0x060003CE`, the `Project.value:int` and `Project.size:float` fields, MethodDef count 2317, and the existing `System.Math.Pow(double,double)` reference.
- two independent formal applications produced byte-identical output `1db686c32f24ea81660f3d18ccfd649ee7aeb5a40902e2042639e47355184b82`, both with stderr 0.
- Cecil reopen: 14 IL / 38 bytes / 0 EH / 0 generic parameters, no Cpp2IL helper reference.

## Fixed ILSpy validation

Fixed ILSpyCmd / ICSharpCode.Decompiler `11.0.0.9375` with the locked 56-DLL fixed-Cpp2IL reference set was used.

- HF51 whole-IL baseline reproduced SHA-256 `0b153d64cc9ae3c49f9386f2dfc75a48db2485c788c2448bcf9160eb3d6c2a4a`
- HF52 whole-IL SHA-256 `515fd597660dd2baafc251c3797707186839ccac01727f32ff4ad2ba52c39994`
- target member stderr: 0
- target-local Cpp2IL / Unknown result type / NotImplemented / invalid-type markers: 0

## Whole-assembly semantic isolation

After normalizing method RVA comments and PE static-data relocation labels:

- normalized method/non-method skeleton is byte-identical between HF51 and HF52;
- MethodDef count `2317 -> 2317`;
- emitted bodies `2297 -> 2297`;
- distinct nonzero body RVAs `2140 -> 2140`;
- only `Project.SunSet(int)` changes semantically;
- HF51->HF52 semantic diff SHA-256: `9de97a39558bc03122f3248da7ebbef24c165f3d13a3f1e5dae9133e0952acca`;
- HF52 candidate-specific MethodDef table SHA-256: `76289ddaba44cb95cad0c297e71c1336202068c5eca4b99a199aacfbe1daa889`.

## Cumulative RecoveryAudit

- audit build head: `43ca5dedcdb407083aa9b2499d0da0d3bb315ac9`
- workflow run: `34559275724` — PASS
- artifact ID: `10183727793`
- artifact SHA-256/digest: `db0152a55fc5f21022eb213fbf03bc2724e431705ce4ecfcba13fc8ca172deb3`
- retained historical auditor remains unchanged for HF1-HF46;
- cumulative HF52 auditor locks the HF52 SHA and rechecks all HF47-HF52 late-stage targets;
- HF52 adds strict checks for exactly 14 IL / 38 bytes / 0 EH, divisor `50f`, exponent `0.5`, `conv.r8`, final `conv.r4`, `Math.Pow(double,double)`, and both `Project.value` / `Project.size` stores;
- two independent actual composite executions ended `RECOVERY_AUDIT_OK` + `HF52_AUDIT_OK`, stderr 0;
- byte-identical formal audit log SHA-256 `0e80cb5849e8a1cc2ad1b312943062d6872fcb144505405bcb426e027f4ca06d`.

## Residual boundary

HF52 deliberately does **not** claim to repair `Zombie.Path_Test`, `Board.GameFail`, `Zombie.DestroyZombie`, `Zombie.DropLootPiece`, or `ProjectManager.DropLootPiece`. Fresh readback still shows reconstruction-loss / Unknown-result evidence in several of those methods. They require their own native/dependency closure before any later patch.

## Acceptance state

All technical recovery gates for HF52 are PASS. Google Drive 19+1+2 provider-readback closure and `Recovery/STATUS.md` advancement remain the final acceptance steps after this evidence commit.
