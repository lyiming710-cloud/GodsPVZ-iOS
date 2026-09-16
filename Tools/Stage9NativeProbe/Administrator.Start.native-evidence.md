# Administrator::Start PC native authority — switch prefetch

Read-only prefetch evidence only. This file does not mutate any managed candidate, does not select `Administrator::Start` ahead of runtime chronology, and does not promote the sealed HF55 baseline.

## Source authority

- Original PC package: `GodsPVZ_1.0.2.zip`, Drive ID `1_5kVGTFmsvwcdRLQ2ah_rtyC6DqvLnf0`
- ZIP SHA256: `2f08b4e2243e2bd296f3db694b4a2c9656252d76202c80167bdf3340b9f65b48`
- `GameAssembly.dll` SHA256: `9ebd7ca996a5b03fb4a766f7a2502b660d581ddbbb36af4d7bf2f06a211da39d`
- `global-metadata.dat` SHA256: `ad992341add498bd1018ac980f42171e0737b0793bf945c8714ad70a445b36b9`
- method-pointer table base `0x181B82D60`

## Stable managed identity

- `Administrator` TypeDef `0x02000002`, RID2
- `Administrator::Start()` MethodDef `0x06000004`, RID4
- managed recovery body on candidate `26064265...`: RVA `0x20B4`, code size 635, 30 locals, no handlers
- `Administrator::mode` FieldDef `0x0400000A`

Read-only Cecil inventory on candidate `26064265fdba4e9b501f0c3312260503fdde43b9e111072c19dd39891e5ca022` gives the complete Administrator field order:

| native instance offset | FieldDef | managed field | type |
|---:|---:|---|---|
| `+0x20` | `0x04000002` | `mainSystem` | `UnityEngine.GameObject` |
| `+0x28` | `0x04000003` | `systems` | `List<UnityEngine.GameObject>` |
| `+0x30` | `0x04000004` | `system0` | `System0` |
| `+0x38` | `0x04000005` | `system1` | `System1` |
| `+0x40` | `0x04000006` | `system2` | `System2` |
| `+0x48` | `0x04000007` | `system3` | `System3` |
| `+0x50` | `0x04000008` | `boardEdior` | `Admin_system2_boardEdior` |
| `+0x58` | `0x04000009` | `skipLevel` | `SystemSkipLevel` |
| `+0x60` | `0x0400000A` | `mode` | `System.Int32` |

The table is consistent with every direct `[rdi+offset]` access in the native `Start` body and with IL2CPP's sequential instance-field layout for this type. Static fields are `<Instance>k__BackingField` (`0x04000001`) and `storedHash` (`0x0400000B`).

On the inspected managed recovery body, the method contains a decompiler artifact after `mode + 1` range checking:

```text
ldc.i8 6442450944                         // 0x180000000
...
"Unmanaged memory load: [...+2FE968+...*4]"
...
conv.i
ldc.i8 6442450944
add
...
"Indirect jump: ... (should have been resolved before IL gen)"
```

The damage fingerprint is exact on `26064265...`: two image-base I8 literals, one unmanaged-load diagnostic string, one indirect-jump diagnostic string, one `conv.i`, and two `mode` field references. This is not source-level pointer arithmetic. The original PC native body identifies it as a normal compiler switch jump table.

## Exact native body

For RID4:

- method pointer entry: `0x181B82D78`
- direct pointer: `0x1802FDD20`
- `.pdata` RuntimeFunction: `0x1802FDD20–0x1802FE980`
- body size: 3168 bytes
- exact native SHA256: `3d83a514d37c18b7d91df7ceba17803bb0ac3cbd670dab15e823af4be8b58d02`

The switch dispatch begins:

```text
0x1802FDE1B  mov eax, dword ptr [rdi+0x60]    ; mode
0x1802FDE1E  inc eax                           ; mode + 1
0x1802FDE20  cmp eax, 5
0x1802FDE23  ja  0x1802FE803                  ; out of table range
0x1802FDE29  cdqe
0x1802FDE2B  lea rdx, [0x180000000]
0x1802FDE32  mov ecx, dword ptr [rdx+rax*4+0x2FE968]
0x1802FDE39  add rcx, rdx
0x1802FDE3C  jmp rcx
```

Directly reading six `uint32` table entries from VA `0x1802FE968` gives:

| index (`mode+1`) | mode | RVA | target VA |
|---:|---:|---:|---:|
| 0 | -1 | `0x002FDE3E` | `0x1802FDE3E` |
| 1 | 0 | `0x002FDFF1` | `0x1802FDFF1` |
| 2 | 1 | `0x002FE185` | `0x1802FE185` |
| 3 | 2 | `0x002FE291` | `0x1802FE291` |
| 4 | 3 | `0x002FE3EF` | `0x1802FE3EF` |
| 5 | 4 | `0x002FE5F4` | `0x1802FE5F4` |

Values where unsigned `mode+1 > 5` flow to `0x1802FE803`.

## Native own-method call crosswalk

Direct method-pointer reverse mapping of unique own-project call targets inside the six case blocks resolves to:

- RID1607 / `0x06000647`: `MainUIController::SetGloveButtons()`
- RID7 / `0x06000007`: `Administrator::Start_植物存档数据()`
- RID115 / `0x06000073`: `System0::LoadPlantData(System.Int32)`
- RID561 / `0x06000231`: `ResourceManager::Load_zombieInfo_all()`
- RID129 / `0x06000081`: `System1::LoadZombieInfo(System.Int32)`
- RID9 / `0x06000009`: `Administrator::Start_关卡配置文件(Board)`
- RID136 / `0x06000088`: `System2::Start2()`
- RID158 / `0x0600009E`: `System3::LoadSuppliesInfo(System.Int32)`
- RID522 / `0x0600020A`: `ResourceManager::GetSuppliesInitialValuePath()`
- RID340 / `0x06000154`: `InternalResourceLoader::ReadJson_StreamingAssets(System.String)`
- RID604 / `0x0600025C`: `SuppliesInitialValue::.ctor()`

This crosswalk is read-only evidence. Shared generic/Unity/native helper targets are not assigned source-level identities merely from their code addresses.

## Case semantics established so far

The direct native body supports these concrete case facts:

- `mode = -1`: operates through `systems`, `mainSystem`, `boardEdior`, Camera/main UI state and reaches `MainUIController::SetGloveButtons()`.
- `mode = 0`: activates the relevant system object and configures the object reachable through `skipLevel`; this case currently has no unique own-project direct call sufficient by itself to reconstruct the entire source block.
- `mode = 1`: invokes `Administrator::Start_植物存档数据()`, then `system0.LoadPlantData(-1)`.
- `mode = 2`: calls `ResourceManager.Load_zombieInfo_all()`, writes the returned list into the object at `system1`, then calls `system1.LoadZombieInfo(-1)`.
- `mode = 3`: invokes `Administrator::Start_关卡配置文件(Board)` and `system2.Start2()` around BoardManager/board setup.
- `mode = 4`: calls `system3.LoadSuppliesInfo(-1)`, gets the supplies-initial-value path, reads StreamingAssets JSON, and either creates a `SuppliesInitialValue` fallback or assigns the deserialized result before continuing.

These statements constrain a future managed reconstruction but are not themselves a patch. Exact null semantics, object activation loops, fields on the subsidiary System classes, and shared Unity helper calls must be cross-checked before authoring recovered IL.

## Existing damaged managed refs are insufficient

The damaged managed `Start` body currently retains only a small subset of source-level calls (`Transform.GetChild`, `GetComponent<Canvas>`, `Camera.main`, `Canvas.worldCamera`, Unity null operators, `Board.GamePause`, and synthetic `NullReferenceException` paths). The native body is 3168 bytes versus 635 bytes of damaged managed IL. Therefore a future repair cannot be a one-instruction or switch-only substitution: the six native-authoritative case bodies must be reconstructed explicitly.

## Future repair constraint

If a later exact-R3 runtime selects `Administrator::Start()` as `FIRST_INVALID_IL`, repair must reconstruct this switch as managed control flow using the six native-authoritative targets while preserving the surrounding Unity calls, null semantics, fields, and metadata. Do not use pointer arithmetic, indirect jumps, blanket guards, exception swallowing, or a reduced approximation of the six cases.

Because `Administrator::Start` is not yet formally selected by the current TextLink.Update runtime chronology, this remains read-only prefetch evidence and no Administrator.Start candidate has been created.
