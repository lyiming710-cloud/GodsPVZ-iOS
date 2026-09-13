# Map immediate gameplay-path recovery evidence

Input: `1c62f969409ed16c9d7c7855f7b52e6b593701524e2b2d75ec0affbc3d05b209`.

The corrected lifecycle path is:

`PrepareUIController.GameStart -> BoardManager.LoadBoard -> Board.Start -> BoardConfig.CreateMap -> Map.Copy`.

Static inspection of the input shows `Map.Copy`, `Map.GetMapX`, and `Map.GetMapY` directly accessing `List<T>._size/_version/_items`. `Map.GetMapX` also has an ILSpy type-stack diagnostic.

PC x86-64 IL2CPP authority:
- `Map.Copy`: token `0x06000285`, VA `0x18032B8F0`, end `0x18032BC80`
- `Map.GetMapX`: token `0x06000289`, VA `0x18032C3B0`
- `Map.GetMapY`: token `0x0600028A`, VA `0x18032C440`
- `Row..ctor(int,int)`: VA `0x18033DEB0`
- `Grid.Copy`: VA `0x18032A010`

Native semantics for `Map.Copy`:
1. obtain map width through `GetMapX()`;
2. obtain row count (0 if `rows == null`);
3. construct a new Map with those dimensions (IL2CPP inlines the constructor's row creation loop);
4. copy `senarioState` and `cameraSize`;
5. for every source row, copy `EnemyType`;
6. for every grid in the destination row, assign `sourceGrid.Copy()` and normalize `gridX/gridY` to loop indices;
7. return the new Map.

Recovery changes exactly three methods and replaces runtime-private List field manipulation with public `Count/get_Item/set_Item` semantics. `Map`/`Row` constructors and unrelated A* methods are not modified in this batch.
