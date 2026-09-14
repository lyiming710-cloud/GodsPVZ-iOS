# Grid.Copy PC native evidence

Authority: original GodsPVZ 1.0.2 PC x86-64 IL2CPP `GameAssembly.dll` SHA256 `9ebd7ca996a5b03fb4a766f7a2502b660d581ddbbb36af4d7bf2f06a211da39d`.

Managed target on runtime-qualified D65C candidate:
- Method: `Grid.Copy()`
- MethodDef token: `0x06000294`
- RID: `660`
- damaged managed RVA: `0x35D1C`
- damaged managed body: 120 bytes / 4 locals
- first causal invalid IL after CreateMap recovery: `IL_0064: ceq` following `ldloc Grid` and `ldc.i4 0`.

PC method pointer attribution:
- CodeGenModule method-pointer entry: `0x181B82D60 + (660 - 1) * 8 = 0x181B841F8`
- native VA: `0x18032A010`
- `.pdata` function range: `0x18032A010–0x18032A09E`
- native function size: 142 bytes
- native slice SHA256: `26d0e35128665a985e660cd922b345fc01fc5857e2e78e9421a167a949d21617`

Native semantics:
1. Read `gridX` and `gridY` from source Grid.
2. Allocate/construct a new `Grid(gridX, gridY)`.
3. If allocation unexpectedly yields null, invoke the IL2CPP null-failure helper.
4. Copy `gridState`.
5. Copy `isHome`.
6. Copy `passablePoint`.
7. Return the new Grid.

Relevant native field offsets observed directly in the function:
- `gridState`: `+0x10`
- `isHome`: `+0x14`
- `passablePoint`: `+0x18`
- `gridX`: `+0x1C`
- `gridY`: `+0x20`

The managed object-vs-integer `ceq` is reconstruction corruption. Recovery must preserve ordinary reference null semantics and must not alter field visibility or add defensive guards that change source behavior.
