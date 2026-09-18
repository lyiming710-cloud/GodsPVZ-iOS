# GodsPVZ iOS recovery

Reverse-recovery workspace for the GodsPVZ 1.0.2 Unity IL2CPP builds supplied by the repository owner.

## Locked source/build identity

- Unity: `2022.3.44f1c1`
- Android package: `com.tipsGodsStudio.godsPVZ`
- IL2CPP metadata layout: `31.1`
- Assembly-CSharp MethodDef invariant: `2317`
- Formal sealed high-fidelity baseline: **HF55**
- HF55 SHA256: `dc205a40dc2478b3aacbb3a7d6bb1ca96ffb0a964648d34062b4ddc75f3b3655`

The PC x86-64 IL2CPP build remains the primary gameplay-semantic authority; the Android build is the independent mobile/native reference.

## Current Stage9.1 development baseline

The current runtime-qualified development DLL is:

`047054e0db594b6e3385fe4d2555c5932dcd4c28e4b2cb6fb39acbc4336d43ee`

Exact-R3 natural runtime has closed the current `Administrator.Start`, `Administrator.Update`, and `GlobalStaticVars.BGMVolume` Invalid-IL chain with no regression in the five-state SkillProgress path.

Package restoration is also closed for the current Unity gate:

- package-script GUID evidence: **67/67 resolved**
- dependency closure: **10 resolved / 4 controlled overrides / 0 unresolved**
- pinned direct Unity package bundle generated
- exact R3 project input preserved
- exact Unity China Editor `2022.3.44f1c1` preserved

## Current gate

The active gate is `.github/workflows/stage9-current-baseline-formal-import.yml`.

It reconstructs exact R3, overlays the runtime-qualified development DLL, embeds the pinned direct packages plus dependency closure, verifies the 67 exact MonoScript GUID mappings, and then runs a fresh Unity import/compile gate. It also requires the package-reference migration marker to report `67/67` and scans serialized assets for zero remaining old recovered-package references.

A working IPA is **not** claimed until import/compile, scene/runtime validation, iOS IL2CPP export, Xcode build, packaging, and device validation all pass.

See `Recovery/HANDOFF_CURRENT.md` for the authoritative current handoff. Historical recovery/audit evidence is retained separately and should not be treated as current execution guidance.
