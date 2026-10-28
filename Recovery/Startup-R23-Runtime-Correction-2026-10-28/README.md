# R23: correction after the actual R22 gameplay failure

Candidate 1d244776ee85083c2ed298c4f749f96b6c5ae820a78d50b0ce47480691775122,
24,095,232 bytes, is based on exact R22 15e01503. This is a correction to the
completed 112-position cohort, not a claim of 100 new repairs. Three bodies change:
Board.Update 0x060002AB, Device.Update 0x06000309 and Device.Update_Brightness 0x0600030A.

Ten distinct native-backed corrections restore Animator.SetBool("hide",false),
three float-zero comparisons, ordered NaN expiration behavior in both brightness
forms, capture brightTime before the deltaTime API in both forms, and clear both
float fields in both native qword resets. All branches are re-emitted with label
targets; old body slots remain unchanged. All 2,314 other MethodDef bodies, old
metadata rows/heaps, method flags and section permissions are preserved. No new
MemberRef, MethodSpec or local signature is added. Two fresh constructions match.

Actual raw candidate CIL was executed against the five old caller profiles plus
two Device profiles. The corrected, fully parameterized native oracle observes
128 Board/bank normal cases and 124 Device cases with zero differences. Three
managed finally cases still do not execute original Windows SEH unwind. Seven
physical type-valid semantic mutants execute normally and disagree with the
original callers. A malformed F/I4 comparison mutant is rejected by the unchanged
strict checker. Stock CLR tolerated that mutant; this negative finding is retained
and is not mislabeled as InvalidProgramException. Unity Mono rejected that shape
in the actual failed run. Exact DLL JIT preparation passed for three changed
methods, without claiming method execution in Unity. Raw full type census is
2,200 PASS / 97 FAIL_OR_UNSUPPORTED; it is not whole-assembly acceptance.

The old R22 trigger oracle's false acceptance and the new parent mismatch report
are preserved. Incomplete SDK ComponentHolderProtocol behavior and the actual
Device.InjuryStatusUpdate NRE remain open. Official Visual Scripting 1.9.4 source
was retrieved for investigation only; no SDK or resource binary was patched here.
Fresh IL2CPP/all-source compiler checks and actual Unity input/play remain pending.
No IPA or device acceptance is claimed.
