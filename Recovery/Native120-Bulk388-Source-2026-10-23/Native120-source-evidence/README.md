# Native120: Projectile.SetSortingGroup source proposal

2026-10-23. One additional MethodDef, `0x060003F9`, outside the historical 381-entry direct worklist. The Bulk350 R6 compiler log exposed a header-instantiation diagnostic in this method. This directory contains source and input evidence only; no new candidate, compile, tests, workflow or qualification was performed.

The authoritative map entry is image `Assembly-CSharp.dll`, type `Projectile`, method `SetSortingGroup`, native VA `0x18037C5E0`. Tokens alone are not unique across images. The withdrawn first extraction from another image is retained under `withdrawn-ambiguous-token-read` and is not evidence for this proposal.

Native operations: read `Projectile + 0xF8` (`sortingGroup`), call Unity `Object.op_Equality(group, null)`, conditionally obtain `gameObject` and call `GameObject.AddComponent<SortingGroup>()`, store and reload `sortingGroup`, set the layer string to exact original `Entity`, reload again, then set sorting order to unchecked `-10 - (int)(fY * 10.0f)`. `fY` is at `+0x48`; the original RIP constant at `0x1815A7C54` has bits `0x41200000` (10.0f). The native integer instruction is `cvttss2si eax,xmm0`, followed by 32-bit subtraction.

`MethodBodies.cs` uses existing references and strings, no new metadata rows. It declares MaxStack 4 and no new local signature (old unused locals retained). Unity equality is a static `call`; instance component/property calls use `callvirt`. It adds no defensive early return. The runtime-generated write barrier and lazy metadata/class initialization belong to IL2CPP, not manually emitted game CIL.

External verification must compare actual candidate execution with original native behavior, including already-present and Unity-destroyed groups, AddComponent callbacks, setter callbacks that replace the field, null/throw paths and positive/negative fractional fY. NaN, infinity, out-of-range float-to-int conversion and signed overflow remain explicit cross-platform fidelity questions; source transcription is not proof that CLR and arm64 conversions match the PC instruction. Do not label these cases accepted without evidence.
