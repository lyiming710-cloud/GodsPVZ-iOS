# Stage9 Row::.ctor(int,int) PC-native evidence

Authority inputs:

- `GameAssembly.dll` SHA-256 `9ebd7ca996a5b03fb4a766f7a2502b660d581ddbbb36af4d7bf2f06a211da39d`
- `global-metadata.dat` SHA-256 `ad992341add498bd1018ac980f42171e0737b0793bf945c8714ad70a445b36b9`
- MethodDef token `0x06000292`, native VA `0x18033DEB0`.
- Native normal-return endpoint `0x18033E074`; null/bounds throw stubs begin at `0x18033E075`/`0x18033E07B`.

Native behavior recovered from the PC 1.0.2 function:

1. Call the base object constructor.
2. Store constructor `gridY` into `Row.gridY`.
3. Allocate `List<Grid>` and store it in `Row.grids`.
4. While `grids.Count < mapX`:
   - snapshot the current count as the new grid X index;
   - construct `Grid(0, index)`;
   - explicitly set `gridState = GridState.Null`;
   - set `plant_bottom`, `plant_common`, `plant_sheath`, and `plant_top` to null;
   - set `gridX = index` and `gridY = constructor gridY`;
   - append the grid to `grids`.
5. Store `Row.EnemyType = 0` and return.

The PC IL2CPP native body inlines `List<Grid>.Add`, which explains its direct `_size`, `_version`, `_items`, and grow-path accesses. Those are implementation details of IL2CPP-generated native code, not legal managed accesses. The repaired managed body therefore uses `get_Count()` and the assembly's already-existing valid `List<Grid>::Add(!0)` MemberRef. No private `List<T>` fields are emitted.
