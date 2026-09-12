# Stage9.1 Board::.ctor native-backed recovery evidence

Development-only recovery evidence. Formal cumulative recovery remains HF55 until separately sealed.

## Exact target

- Managed token: `0x060002D1` (`Board::.ctor()`)
- Exact input SHA-256: `06be72c9a18079f6b0dea7e6026b1ad1f5cd6f083aedf2ea250e074330aef21d`
- Primary authority: original PC x86-64 `GameAssembly.dll`
- Original PC method pointer: `0x180329440`

## PC native observations

The original PC constructor at `0x180329440` performs, in order:

- `challengeType = -1` (`mov dword ptr [this+0x20], 0xffffffff`)
- `gameSpeed = 1.0f`
- `gameStartCountdown = 6.0f`
- allocate/store `List<Popup>`
- `cardBankHidden = true`
- allocate AudioClip arrays with lengths `64`, `128`, `64`
- write the adjacent bool pair at `this+0x160` as word `0x0101`, proving both `UI_able=true` and `zombieUI_able=true`
- copy `Vector3.zero` into `cameraPosition`
- independently copy `Vector3.zero` into `dithering`
- construct/store `RewardInBoard`
- tail-call the base `MonoBehaviour` constructor

Relevant disassembly includes the exact stores at `0x18032948d` onward; the two Vector3 copies begin at `0x180329577` and `0x1803295b6`.

## Cpp2IL corruption being removed

The managed candidate before this patch contains synthetic/invalid reconstruction around the Vector3 static fields, including `Type.GetTypeFromHandle`, native-int locals, and `Unmanaged memory load` diagnostics. It also collapses the two adjacent bool writes into `ldc.i4 257 -> UI_able`, losing the `zombieUI_able` store.

The recovery replaces the whole constructor body with behavior-equivalent managed CIL matching the PC native writes. No other MethodDef may change.
