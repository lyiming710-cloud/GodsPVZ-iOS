# Native88: explicit CreateAudioAtPoint property-order repair

This is a single-method repair to `GlobalStaticVars.CreateAudioAtPoint(AudioClip, Vector3, Single, Single)` (`0x0600013E`). It is separate from the two Native87 callers. It changes the identified clip-length/time-scale ordering defect. It does not assert that all preserved prefix dependencies or the whole game are already qualified.

All evidence below is static. Candidate generation is implementation work; every verifier, fixture, native-machinecode/CIL execution, negative control, compiler gate, Unity/Xcode run and acceptance test remains delegated to AGY Gemini 3.8 Flash.

## Locked source and defect

Original `GameAssembly.dll` SHA256 is `9ebd7ca996a5b03fb4a766f7a2502b660d581ddbbb36af4d7bf2f06a211da39d`. The complete split-pdata native interval is `[0x18031B350, 0x18031B580)`, SHA256 `406900bf73b2fa0555b77bc1276a399669dc5992e6a8800f1012d4f5d9b34cad`. `helper-source.json` contains its full instructions and the actual parent82 managed body. The independent audio-module call maps and helper metadata are also retained in Native87 `PRIMARY-EVIDENCE.json`.

The parent87 must still contain the exact pre-existing 145-byte managed body, SHA256 of its complete fat header plus code `5520ac1a25114890963a4712d8865cd1027064d3d7423f015ec678e3a1ee6f77`, local signature `0x110000FD`, two locals (`GameObject V0`, `Single V1`), no EH. Construction checks those facts before editing.

Original native calls `AudioSource.Play` at `0x18031B4E8`, then raw-checks the original clip, calls `AudioClip.length` at `0x18031B4F7`, and only then `Time.timeScale` at `0x18031B501`. It compares the stored float32 floor **0.01 as the left operand** against the first scale. Ordered floor > scale selects the floor. Every other result, including unordered NaN, calls timeScale **a second time** at `0x18031B515`; this second value is the selected factor. The destroy duration at `0x18031B532` is float32 **captured length × selected factor**, with length on the left. `Object.Destroy(created GameObject, duration)` is followed by returning the captured AudioSource.

The existing CIL instead reads one/two timeScale values before length. A throwing length getter or a callback mutating scale can distinguish the order. This is a recovered-managed/native difference; there is no proven Editor branch explaining it.

## Narrow implementation

The first **105 code bytes**, through the `AudioSource.Play` call and retaining its captured AudioSource on the evaluation stack, are copied verbatim. All branches contained in that prefix retain their exact offsets and targets. This preserves the existing Unity clip equality gate, `One shot audio` construction, transform position assignment, AudioSource addition, setters, and Play. The patch does not add a replacement fallback, rerun a setter or reread a created receiver.

One new target-specific StandAloneSig clones the old signature and adds `Single V2` for captured length. Shared pre-existing local rows are not changed. No MemberRef, TypeSpec, MethodSpec, string, parameter or access-flag change is introduced.

Suffix after Play:

```cil
// Captured AudioSource remains below this suffix's temporary values.
ldarg.0
callvirt float32 AudioClip::get_length() // existing 0x0A0000FA; null throws here
stloc.2
ldc.r4 0.01
call float32 Time::get_timeScale()       // existing 0x0A0000F9, first call
bgt.s FLOOR                            // ordered comparison; NaN falls through
call float32 Time::get_timeScale()       // second call only when comparison false
br.s FACTOR
FLOOR:
ldc.r4 0.01
FACTOR:
stloc.1
ldloc.0                                 // same captured GameObject
ldloc.2                                 // captured length, left operand
ldloc.1                                 // selected factor, right operand
mul
conv.r4
call void Object::Destroy(Object,Single) // existing 0x0A000088
ret                                     // same captured AudioSource
```

The complete body is 148 bytes, so it is appended as a complete fat MethodBody in the existing read-only final section. Only this MethodDef's RVA is changed in the active metadata, and the old slot is preserved. The new local token is written only in this target's appended header. MaxStack remains 4, InitLocals is retained, no EH is invented.

## Required Gemini evidence and limits

Execute the original helper's actual machinecode and actual candidate CIL independently. Include callback/exception injection after every retained prefix call, length exceptions before any scale call, first and second scale mutations/exceptions, scale below/equal/above floor, unordered NaNs, signed zero, length/factor NaN operand ordering, original clip pseudo-null behavior, captured created-object/source identity, and duration/return observations. At least the old-order variant, skipped-second-getter variant, unordered floor branch and swapped multiplication must be valid executable behavioral negative controls.

Confirm exact native binding and actual player/Core helper implementations for the retained creation/AudioSource-addition dependencies. Preserving a pre-existing prefix does not independently prove that prefix's engine dependencies. Any additional difference found there is a reported failure or explicit limitation, never a fixture stub silently counted as full native equivalence. Scope/raw byte isolation, appended local signature, relocated RVA/header, reproducibility, frozen verifier, member access, malformed/forged inputs and historical suites remain mandatory before promotion.

The previously started SkillManager source investigation is unfinished reference work in `native88-spec-2026-10-13`. It is **not** part of this candidate or this method count.
