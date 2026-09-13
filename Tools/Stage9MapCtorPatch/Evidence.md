# Stage9 Map::.ctor(int,int) PC-native evidence

Authority: original PC GodsPVZ 1.0.2 `GameAssembly.dll` SHA256 `9ebd7ca996a5b03fb4a766f7a2502b660d581ddbbb36af4d7bf2f06a211da39d`, metadata SHA256 `ad992341add498bd1018ac980f42171e0737b0793bf945c8714ad70a445b36b9`.

Target managed MethodDef: `0x06000281`, `Map::.ctor(int,int)`. Native method map entry points to VA `0x18032D290`. The normal return is at `0x18032D3FA`; `0x18032D3FB` begins the null-failure helper path and padding follows at `0x18032D407`.

The native control flow is unambiguous:

1. Call the managed base/object constructor (`0x180302170`, with the IL2CPP hidden method argument zeroed).
2. Allocate `List<Row>` and assign it to `Map.rows` (field storage at object offset `0x10`).
3. Read the list count/size and compare against `mapY`; return once `Count >= mapY`.
4. For each row, construct `Row(mapX, rows.Count)`.
5. Append that Row to `rows`. The x64 code increments List version/size and writes the backing array directly because IL2CPP has inlined the `List<T>.Add` implementation; if capacity is exhausted it calls the resize helper.
6. Loop back to the count check.

The recovered managed-equivalent source is therefore:

```csharp
rows = new List<Row>();
while (rows.Count < mapY)
    rows.Add(new Row(mapX, rows.Count));
```

The corrupted Cpp2IL body incorrectly exposes IL2CPP implementation details as managed accesses to private `List<T>._size`, `_version`, `_items`, `AddWithResize`, and contains `"Method not found @180302170"` instead of the base constructor. Those accesses are not valid managed-source semantics and are the cause of the observed `FieldAccessException` in the Board load path.

Runtime causal stack before repair:

`BoardStart.Awake -> Instantiate(BoardManager) -> BoardManager.Awake -> BoardManager.LoadConfigFile -> BoardConfig..ctor -> Map..ctor`

The repair is intentionally limited to MethodDef `0x06000281` and replaces the inlined private-field manipulation with the semantically equivalent public `List<Row>.Count` / `List<Row>.Add` operations proven by the PC native flow.
