# PlantPortraits System.Private.CoreLib scope correction evidence

Input candidate: `d1decdd1788db6a6e78c7890311067bea5012323fd71bda914e3bfca20b70126`, produced by the PC-native-backed `ResourceManager.Load_card_Choose_PlantPortraits()` recovery.

Exact R3 strict runtime `34770060252` no longer reports the prior `List<T>._size` FieldAccessException. Instead `ResourceManager.LoadSprites()` fails immediately with:

`FileNotFoundException: Could not load file or assembly 'System.Private.CoreLib, Version=10.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e'`

Independent ILSpy inspection of the recovered target shows that only local `[1] System.Array` was emitted as `[System.Private.CoreLib]System.Array`, while the target Unity DLL's BCL references, including `System.Array::GetEnumerator`, remain `[mscorlib]`.

Cause: the recovery tool used the host runtime type `typeof(Array)` under .NET 10 when creating that local, accidentally importing `System.Private.CoreLib` into the Unity-target assembly.

Correction scope:
- target MethodDef remains token `0x06000223` (`ResourceManager.Load_card_Choose_PlantPortraits`)
- change only local index 1 type scope: `System.Array@System.Private.CoreLib` -> `System.Array@mscorlib`
- instruction sequence unchanged
- exception-handler boundaries unchanged
- other locals unchanged
- all other 2316 MethodDefs unchanged
- all 2802 FieldDefs unchanged
- remove the now-unused `System.Private.CoreLib` AssemblyRef and require zero remaining TypeRefs to that scope

This is a toolchain reference correction, not a gameplay-semantic change.
