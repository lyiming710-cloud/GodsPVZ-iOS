# Bulk401 generic repair checkpoint ¡ª 2026-10-24

See CURRENT-RESULT.json for current gate states and METHODBODY-SPEC.md for exact methods, original-native evidence and remaining limits. The candidate is `6de4db6c94b80a43876aea060b573ed60c1fa7170cff485103d2c808b94762ae`.

Completed: twelve scoped reconstructions (11 direct errors + damaged Push dependency); deterministic build; reopened isolation of 1916 non-target methods and byte-identical inherited 389 bodies; legal member access; finite closed-context static checks; actual candidate CIL tests with semantic mutants; fresh Linux IL2CPP plus all 1030 generated C/C++ units and independent complete log/hash readback.

Apple compiler precheck also passed all 1030 units; original-native controlled execution matched 139 candidate CIL rows for all twelve new methods. The complete export is next. New-candidate Unity export, final Xcode build/link, IPA packaging and game/device acceptance are pending. Native dynamic equivalence is not claimed. Main and prior accepted inputs remain unchanged; full known-bad R4 retries remain blocked.

BULK401-SOURCE.zip contains aggregate construction source and native evidence. RUNTIME-AND-REVIEW-TOOLS.zip contains the executable CLR fixture, independent audit tools, controls and their preserved first failures. MEMBER-ACCESS-RAW.zip preserves the full reopened reference audit; MEMBER-ACCESS.json is its summary. compiler-40012442171 includes the complete compiler report, per-file identities and all raw compiler logs. The 152 MB generated archive stays attached to the named Actions run; its SHA-256 is in COMPILER-READBACK.json.

Historical R4 transition/export/packaging helpers stay unchanged. New Bulk401 forks bind the new DLL and include every generated C/CPP file in export receipts. The full pipeline has 19 handoff controls, including changed Generics and GenericMethods rejection, and checks all actual-export generated code before Xcode compilation.

Full integration has been dispatched: [40016440758](https://github.com/lyiming710-cloud/GodsPVZ-iOS/actions/runs/40016440758), source commit `4b42bb841af9b70c44019746037aeccbc4c42d26`. The exact-input binding, real helper compilation and Apple toolchain preflight passed. Export is running; no new export/link/IPA success is claimed. The run snapshot is in FULL-RUN-40016440758.json.
