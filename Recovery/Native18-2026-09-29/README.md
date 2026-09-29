# Native18: Zombie Method C++ Clang Compilation Error Fixes

Eleven MethodBody replacements in class `Zombie`, targeting all 20 Apple Clang compilation errors in `GodsPVZRuntime1__9.cpp` that previously halted the Xcode iOS ARM64 build (observed in CI runs 36435550922 and 36446058431).

## Root Causes in native17
1. `Zombie.PreviousPosition()` (`0x06000418`): Field assignment mismatch `L_0->___x = L_1` where `L_0` was a struct value and `L_1` was `Vector3`.
2. `Zombie.Start_PreviousPosition()` (`0x0600041E`): Type mismatch assigning float `fX` (`L_0`) directly to `Vector3` field `___previousPosition`.
3. `Zombie.Update_Move()` (`0x06000427`): Passing float pointer `&V_15` to `SetrSpeed(Vector3)` and `set_position(Vector3)`, plus invalid `conv.r4` on `Vector3` struct.
4. `Zombie.Update_PreviousPosition()` (`0x06000429`): Inlined pointer subtraction `&V_44 - 25` assigned to `Vector3`, and passing `RuntimeObject**` to `RuntimeObject*`.
5. `Zombie.ArmBroken(bool)` (`0x0600042F`): Passing `&V_63` (`List<Object>::Enumerator*`) instead of `Vector3` to `CreateAudioAtPoint` and `Transform.set_position`.
6. `Zombie.Ashe(Damage)` (`0x06000430`): Passing `&V_8` (float*) as 3rd argument (`Vector3`) to `DamageText.CreatDamageText`.
7. `Zombie.CheckZombieWin(int, int)` (`0x06000432`): `ceq` comparing `Grid*` pointer with integer `0` instead of `null`.
8. `Zombie.CreateStartPrePath()` (`0x06000433`): Assigning `List<EnemyPath>` object reference to `int32_t` local variable; comparing pointer against integer literal 0.
9. `Zombie.CreatParticles(ParticleState)` (`0x06000436`): Passing object reference `&V_6` instead of `Vector3` by value (`V_3`) to `Transform.set_position(Vector3)`.
10. `Zombie.DestroyZombie()` (`0x06000437`): Loop variables `V_10` and `V_17` de-typed as `System.Object`, causing illegal pointer casts when indexing `GameObject[]`; `List<object>::Remove` method reference invalid for `List<Zombie>`.
11. `Zombie.Die(bool, bool)` (`0x06000438`): Integer loop variables and offsets de-typed as `System.Object`; `Transform.set_position` passing wrong object references; decompiler dead strings and dead code leaving non-empty stack.

## Implementation & Non-Target Isolation
- Implemented via `scripts/takeover/PatcherNative18`.
- Stack underflow controls: 11/11 negative control checks pass.
- Non-target isolation: All 2286 other MethodBodies in `GodsPVZRuntime1.dll` remain 100% byte-for-byte identical both in memory and after disk reopening.
- Assembly metadata preserved: 320 types, 2317 methods, 2802 fields.
- Replay qualification: Real IL2CPP exited with code 0 and `methods: []`.
- C++ output syntax check: Generated `GodsPVZRuntime1__9.cpp` verified with zero unmanaged memory load strings, zero stack warning strings, and all 11 `Zombie_*` methods successfully synthesized.

## Verified Hashes
- **Unlinked candidate** (`Assembly-CSharp-native18.dll`): `f9122641902d0c4a65bf7120c29b64c01cf830506e4b1dd14479ad1fd81226b1`
- **Linked candidate** (`native18-linked.dll`): `7e04eb13515e204a8638a300cea3b6628d67266ab37be7a59c3e11c9bbe39543`
- **Input Linked native17 base**: `9bf2d7a033632061e8e71d32c70312c3c234298649495afc384d0dfe7120a125`
- **Input Unlinked native17 base**: `74c7474f0592d3f577f8376d75d26528624798794983b3f07db5fd973e7456b2`
