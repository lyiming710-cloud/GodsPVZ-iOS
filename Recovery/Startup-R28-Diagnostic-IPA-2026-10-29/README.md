# R28 unsigned diagnostic IPA

This checkpoint prepares a fresh platform build of the exact R28 game
`9081408a2226a79e23991355108540a0260a3966a500cecf4524f3cba9dfb364`
and actual-Editor SDK
`51f72bcd146c5571f642f3c4149f0545eb704fd325d83eb0f7c4ca2deec14c2b`.
No additional game method or field is changed here. This is a test IPA build,
not production promotion or game acceptance.

The preceding actual Unity run 40794439817 is preserved as FAILURE: one real
plant was selected and planted through OS input, the game logged no runtime
errors, and an independently reconstructed raw profiler tree observes
PlantAnimationEvent -> Plant.Shooting -> ST_Peasniper -> Zombie.TakeDamage.
The original complex level ended in loss after 48.15 game seconds. Only one
attack stack was retained and 162 profiler frames were lost; target argument
identity, numerical damage equivalence, full-level paths and iPad performance
are unaccepted. The failed result is not rewritten as PASS. Its complete raw
evidence remains under Startup-R28-Attack115-2026-10-29.

Actual Linux conversion/compiler run 40794444003 passed, with all 755 C++ and
274 C sources and raw logs independently reread. It does not qualify Apple
compilation or an IPA. Admission rechecks all 1029 raw compiler logs and the
original Unity delivery and raw attack tree before platform setup.

The diagnostic export reuses the authenticated R3 resources, exact Unity
2022.3.44f1c1 and source restoration used by the real Editor run. The original
Unity build result/error decisions remain intact. The build helper gains one
report observer and an API rename from symlinkLibraries to symlinkSources.
The actual report must be Succeeded with zero errors and no logged errors.
The complete exported tree is hash-bound to the game, SDK and workflow head;
all generated C/C++ is checked with Apple ARM64 flags before Xcode linking.
The unsigned IPA is checked for physical-iOS arm64 executable/framework roles,
absence of signing artifacts, exact metadata and every exported Data file.

Local preflight: exact Editor API compilation of both export helpers passed;
25 transport/admission/format controls passed; actionlint and 76 decoded Bash
blocks passed syntax checks. Format controls simulate ditto on Windows and
use explicitly synthetic Mach-O fixtures. They are not an Apple build, real
Unity export, runnable app or behavioral test. Bad metadata, missing/changed
resources, simulator/cpu mismatches, altered source/header/library inventories,
forged qualification and hidden original failures are rejected.

Workflow: `.github/workflows/startup-r28-diagnostic-ipa.yml`, also installed at
the already-registered `bulk401-startup-runtime-gate.yml` for dispatch on this
isolated branch. The original R28 gameplay workflow remains separately saved.
No new IPA exists until the actual workflow produces and validates it.

Actual test-IPA workflow 40800918671 has been dispatched at source 7625352d818c5401324cccef76e7c596e0d8de13. Exact input admission, remote Editor helper compilation and Xcode/iOS SDK setup preflights passed. The export job is running. These setup results are not Apple source compilation/linking or an IPA. Source and failed/partial gameplay evidence remain unchanged; see DISPATCH-40800918671.json for this observed status snapshot.
