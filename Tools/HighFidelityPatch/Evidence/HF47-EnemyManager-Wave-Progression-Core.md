# HF47 — EnemyManager wave progression core

Date: 2026-09-11  
Formal input: HF46 cumulative DLL  
Input SHA-256: `900d3aabbf79b094a6b412f7d869cc5285ce206a2d17501b69fc3cd8651beefd`

## Fixed original inputs

The original PC 1.0.2 source was reintroduced directly from the user's `PVZ GOD` Drive folder and rehashed before this gate.

- PC ZIP SHA-256: `2f08b4e2243e2bd296f3db694b4a2c9656252d76202c80167bdf3340b9f65b48`
- PC `GameAssembly.dll` SHA-256: `9ebd7ca996a5b03fb4a766f7a2502b660d581ddbbb36af4d7bf2f06a211da39d`
- PC `global-metadata.dat` SHA-256: `ad992341add498bd1018ac980f42171e0737b0793bf945c8714ad70a445b36b9`
- Original Assembly-CSharp MethodDefs / method pointers: 2316 / 2316.

Native attribution is derived from the original metadata and original PC Assembly-CSharp CodeGenModule method-pointer table, not from recovered-DLL RID adjacency.

## Exact HF47 target set

| Token | RID | Method | Original PC entry |
|---|---:|---|---|
| `0x060001B5` | 437 | `EnemyManager.DispatcheWave(Wave)` | `0x1803175F0` |
| `0x060001B6` | 438 | `EnemyManager.DispatcheZombie(Enemy)` | `0x180317980` |
| `0x060001C0` | 448 | `EnemyManager.TimeUpdate()` | `0x180318EB0` |

Supporting original PC method:

- `0x060001EA`, RID 490, `ParticlesManager.CreatNewParticle(ParticleState)` -> `0x180321F30`.

`.pdata`/unwind closure:

- `DispatcheWave`: `[0x1803175F0, 0x180317973)`
- `DispatcheZombie`: `[0x180317980, 0x180317B47)`
- `TimeUpdate`: chained unwind regions `0x318EB0–0x318F56`, `0x318F56–0x318FCC`, `0x318FCC–0x3195E8`; effective body `[0x180318EB0, 0x1803195E8)`.

Evidence disassembly SHA-256 values:

- `DispatcheWave.asm`: `d1a3ff3f97479ada5f1861b46ef4db9802202d377f347ede3915dcb72cacc87f`
- `DispatcheZombie.asm`: `31c8b364991a9fa73dc1965650c8228210a0a19337514413f7a38d3f41286ef0`
- `TimeUpdate.asm`: `7e397741d0f50363f5dbb329f6560ee2a3f05ccf68eebcbaa48b20da355e1bea`
- `ParticlesManager.CreatNewParticle.asm`: `18f4b49a1a767e5b5b7607e1cdbdd3df1385beced2472259e79729f6a844d706`

## `DispatcheZombie` closure

Original native behavior is closed:

1. `board.zombieManager.CreateNewZombie(theEnemy.id)`.
2. Unity-object truthiness test; false/destroyed zombie returns null.
3. `zombie.TeleportTo(theEnemy.position, 0f, false)`; the full original `Vector3` is copied.
4. Copies `row -> gridY`, `waitingTime`, `ClonePath() -> prePath`, and current `theWave -> zombie.wave`.
5. Accumulates `waveHealth += healthPoint + armor1Point + armor2Point * 0.2f` in the original order.
6. Traverses `GlobalStaticVars.gLawnApp.savesManager.playerSave.almanac_ZombieLock` and stores `true` at `theEnemy.id`.
7. Native array bounds failure is the normal array-bounds exception path. The HF46 decompile that appeared to return an `IndexOutOfRangeException` object as `Zombie` is a reconstruction error.
8. Returns the created zombie.

Raw-reference null dereferences are not replaced with defensive guards.

## `DispatcheWave` closure

Original native behavior is closed:

1. Sets `waveHealth = 0f`.
2. If `theWave.enemies` is non-null, enumerates the complete list and calls `DispatcheZombie` for every entry, preserving enumerator cleanup semantics.
3. If `theWave.isFlagWave == false`, returns.
4. For each `board.map.rows[rowIndex]` with `EnemyType == 1`, creates a flag enemy:
   - `row = rowIndex`
   - initial `id = 1`
   - `waitingTime = 0.02f`
   - `grid = board.boardConfig.GetGridPosition(board.map.GetMapX(), rowIndex)`
   - `position = (grid.x + 57f, grid.y + 28f, grid.z)`
5. Reads `playerSave.adventureLevel`:
   - `<= 10`: ID remains 1
   - `11..15`: even rows use ID 10, odd rows remain ID 1
   - `> 15`: ID 10
6. Calls `DispatcheZombie` for the generated flag enemy.

The Cpp2IL casts between `Enemy` and `Wave` in the damaged managed decompile are not source behavior.

## `TimeUpdate` closure

Original native field offsets and managed identities agree for the timer/wave state. The active behavior is closed as follows.

### Early health-test timer

- Requires `board.gameStart`; raw null board dereference retains normal NRE behavior.
- While `nextTestTime > 0 && !finish`, subtracts `Time.deltaTime`.
- On crossing zero, computes `threshold = shortWavelength - testWavelength`, clears `nextTestTime`, and only tests wave health when `threshold < nextWaveTime`.
- If `TextWaveHealth()` is false, restores the **pre-decrement** `nextTestTime` value. The HF46 decompile's apparent self-assignment is wrong.
- If true, sets `nextTestTime = testWavelength` and clamps `nextWaveTime` to `threshold` when required.

### Normal wave timer

When no flag-wave delay is active:

- Requires `nextWaveTime > 0` and `!finish`.
- Subtracts `Time.deltaTime`; on expiry sets `nextWaveTime = 0`.
- Wave 0 activates the flag meter object and plays board audio ID 0.
- For non-flag wave indices, dispatches `flags[theFlag].waves[theWave % 10]`, updates the flag meter, finishes at `wavesNum - 1`, otherwise increments `theWave`, sets long/test wavelengths, and doubles the next-wave delay when the new `theWave % 10 == 9`.

### Huge-wave warning

When current `theWave % 10 == 9`:

- sets `nextFlagWaveTime = 7.5f`
- instantiates `ResourceManager.prefab_HugeWave`
- scales every component of the instantiated transform's `localScale` by `board.map.cameraSize / 540f`
- plays board audio ID 3
- if this is the final wave, clears `nextTestTime`, but does not mark `finish` until the delayed wave actually dispatches.

### Delayed flag wave and final wave

While `nextFlagWaveTime > 0`, subtracts `Time.deltaTime`. On expiry:

- updates the flag meter
- dispatches the current `flags[theFlag].waves[theWave % 10]`
- if not final: plays audio ID 8, increments both `theWave` and `theFlag`, resets long/test timers, and doubles long delay before the next `% 10 == 9` wave
- if final: creates `ParticleState.FinalWave`, scales it by `cameraSize / 540f`, sets its position to `Vector3.zero`, plays audio IDs 2 and 8, clears `nextTestTime`, and sets `finish = true`.

The original compiler inlined the core of `ParticlesManager.CreatNewParticle((ParticleState)4)` into `TimeUpdate`; the independently attributed original `CreatNewParticle` at `0x180321F30` confirms that enum value 4 is `ParticleState.FinalWave` and that the public managed call preserves the same dictionary lookup / Unity truthiness / missing-effect print / instantiate behavior.

## Gate decision

All three HF47 targets satisfy the project gate simultaneously:

1. concrete managed loss/mis-reconstruction exists in HF46;
2. the methods are materially reachable from the EnemyManager update/wave progression path; and
3. the original PC native body and required behavioral dependencies are now closed without invented gameplay logic.

**HF47 is authorized for exactly these three MethodDefs and no others.**

Formal patch input must remain the HF46 cumulative DLL SHA above. Any additional target requires a separate fresh residual gate.
