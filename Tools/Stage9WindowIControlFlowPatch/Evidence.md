# Stage9.1 Window_I control-flow recovery evidence

Development-only repair candidate. Formal cumulative recovery remains HF55 until separately sealed.

## Exact input

- Input SHA-256: `dc2dccac079f4db46934283c380817d7b60e2a82ae71e8e18ba0b9f199bb6931`
- `Window_I::Start()` token: `0x060006F3`
- `Window_I::Yes()` token: `0x060006F6`

## Recoverable Cpp2IL defect

Both methods contain the same broken switch-lowering pattern: integer expressions `I - 1` and `I - 2` were reconstructed into `System.Object` locals, then arithmetic is performed on those object locals. Cpp2IL also re-used the original `I == 0` boolean at the branches that should discriminate the `I == 1` and `I == 2` cases.

The surrounding preserved branches make the intended cases self-contained and do not require gameplay invention:

- `I == 0`: return/no action.
- `I == 1`: new-save flow with cancel semantics.
- `I == 2`: first-launch new-save flow with exit semantics.
- `I == 3`: rename flow.

`Window_I::No()` independently confirms `I == 2` is the exit-app variant. Call sites in `SavesManager::TryCreateNewPlayerSave()` and `CreateNewPlayerSaveList()` independently pass `2` to `Window_I::PopupNewWindow()` for the no-save startup path.

## Bounded repair

The patch changes only two MethodDefs and only the corrupted switch discriminators:

- `Window_I::Start`: locals V1/V2 `System.Object -> System.Int32`; use V1/V2 zero tests for cases 1/2.
- `Window_I::Yes`: locals V2/V4 `System.Object -> System.Int32`; use V2/V4 zero tests for cases 1/2.

No UI strings, calls, branch targets, save semantics, fields, or any other MethodDef are changed. `Window_I::CreatNewSave()` is deliberately not modified because it contains deeper reconstruction debt requiring separate evidence.
