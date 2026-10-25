# Startup R17: fifty new methods, then one shared verification round

Exact final candidate `d2a645ad0d8164af5fbdcbd9fcaa578d5a28b27e7b13f3012cdb51e63a256091` (21,467,136 bytes), directly based on R16 `5454887dc125cf56b7de5fc6079441075b91972e64db40e52f70d1129a87c73f`. The user's 2026-10-25 instruction is honored: construct fifty new distinct changed methods before starting a new shared test round. Earlier repaired methods and untouched plans are not counted. Subsequent corrections stay within the selected fifty.

## Fifty source changes

- 27 coroutine creation/start helpers remove an inaccessible duplicate write to the generated private state field. Each public iterator constructor already initializes that same Int32 state argument; the original native factory inlines a single write, not a second caller write.
- 17 generated iterator constructors replace an incorrectly bound IDisposable.Dispose call with the real System.Object base constructor. The native empty base constructor shares code address 0x180302170 with empty Dispose aliases; that shared address must not determine the managed constructor's identity. Original constructor machine code calls that empty base and stores the supplied state at +0x10.
- Four WaitFor* factories now call the public wait constructor with the actual controller, replacing ctor(null) followed by an illegal second private Subscribe call. The public constructor already performs Subscribe; the original factory allocates, invokes the empty CustomYield base and subscribes once.
- SwfAssocList<T>.Clear now calls legal public SwfList<T>.Clear, reloads _dict and calls Dictionary<T,Int32>.Clear. The original reference form inlines the list clear; its fully-shared form calls that same RGCTX method. The previous managed body omitted dictionary clearing entirely and wrote private fields of another class.
- CameraController.Awake restores the native Int32 -1 value for Application.targetFrameRate when vSyncCount<=0, retaining the original two platform reads and transform/smoothing order.

These are fifty constructed/restored method bodies, not a claim that fifty distinct gameplay symptoms or all methods in the game have been proved fixed.

## Shared execution found and corrected a real discrepancy

Initial fifty-source candidate `ab834084…b04416` passed the initial static/JIT scope but failed six of 801 coroutine native/CIL records, all in TextConsoleSimulator.Start (0x06000784). Original code captures m_TextComponent into RSI before allocation; old managed control flow read it afterward. A controlled allocation changing the text field exposed the mismatch. The exact failed DLL, original code, raw records and six concrete counterexamples are archived. The correction adds one method-private TMP_Text local, captures before allocation and preserves the original state/owner/text assignment and StartCoroutine order. No additional target method was added. Two fresh constructions of the corrected candidate match byte for byte.

Final complete candidate CIL versus original PC CPU execution matches 1,251 records across all fifty methods: 801 coroutine and 450 wait/camera/collection records. Every method is reached. All original caller bytes are verified after controlled hooks. Original empty base helpers and the original generic SwfList.Clear machine code actually execute. Allocation, engine calls, wait subscription and dictionary clearing remain explicit controlled contracts; this is not proof of their actual Unity/GC/BCL implementations, native SEH, concurrency, or full gameplay. Original pdata ranges, external/funclet boundaries and generic RGCTX identities are retained rather than silently followed or guessed.

Fifty physical fault DLLs are exercised, one per target. Forty-nine produce runtime differences (wrong iterator state/argument, dropped wait controller or omitted dictionary clear). The restored historical Int64 frame-rate fault produces zero CLR runtime differences, and is independently rejected by the unchanged frozen width checker. That non-detection remains visible. The runtime layer alone cannot qualify widths or constructor/type identities.

## Direct actual-DLL supplement

A separate fixture invokes real unmodified DLL methods, without Reflection.Emit or source pruning: 153 constructor cases over 17 methods, 34 pure iterator-factory cases over 17 methods, 42 real closed generic Clear cases and six null-order cases (Int32, String and a 24-byte value type). All 235 executions across 35 distinct MethodDefs pass; the three closed generic Clear instantiations also prepare under actual CLR JIT. Engine-start methods remain covered by the controlled complete-CIL profile, not silently included as real engine execution.

Running the same direct fixture on parent R16 preserves 76 FieldAccessException failures and six null-order failures; corrected R17 has zero. The first direct fixture also had a name-format mismatch: reflection uses '+' for nested types while Cecil uses '/'. That failed comparison output is retained and corrected without changing candidate behavior. Native callback null None/zero normalization is recorded separately. Neither test-setup failure is counted as a candidate pass.

## Isolation and wider checks

Independent raw all-MethodDef/body/metadata and decoded CIL audits confirm exactly fifty changed bodies. The other 2,267 MethodDefs and all old rows/heap prefixes remain intact. No RVA, MaxStack, field/type/method visibility or readonly declaration changes. Shorter bodies retain unused old slot bytes. Two public generic Clear MemberRefs and one method-private local signature are appended; original local entries stay intact. Three out-of-scope physical faults are rejected. The unchanged frozen checker passes 49 targets; the open generic Clear limitation is retained in the raw census and supplemented by two finite closed models plus three actual closed CLR instantiations. This is not universal generic proof.

Whole assembly: raw frozen 2,156 PASS /141 failure-or-unsupported; direct InterfaceImpl supplement 2,184 PASS /113 failure-or-unsupported. Access drops from R16's 142 sites/68 methods to 108 sites/36 methods, zero unresolved, actual auditor exit 2. Changed-method JIT has 49 prepared and one open generic unsupported; the latter has separate closed execution. Zero unterminated lexical tails. These overlapping queues are not an additive gameplay bug count and remain held.

Exact R16 separately passed fresh IL2CPP and all 756 C++/274 C Linux syntax checks in run 40140320130, with all 1,030 returned source sizes/hashes and raw staged-input manifests independently verified. At initial publication R17 has no fresh IL2CPP, Unity Editor, Apple build, player link, IPA or iPad acceptance. The existing iPad black-screen/flicker remains unaccepted.

## Reproduction and archive

Use exact R16, original GameAssembly SHA `9ebd7ca996a5b03fb4a766f7a2502b660d581ddbbb36af4d7bf2f06a211da39d`, original metadata/maps and authentic SDK support. Python dnfile/pefile/Capstone, .NET and the existing Cecil exporter/access auditor are required; no dummy support substitution is permitted. Frozen checker SHA is `2bb0a8c229a63fd33269433759a587ff551c6fde19669bafc506033df9b63d14` and is unchanged.

research_plan.py and complete_source_evidence.py read source contracts only. construct.py creates the initial fifty into fresh output paths; --capture-fix produces the corrected pair. ACTIVE-INPUT.json binds candidate3 and candidate4. prepare_shared_tests.py enforces the fifty-change threshold before deriving input-only full-census/JIT drivers. Initial shared reports are preserved under initial-shared-results. Build coroutine-runtime, extra-runtime and direct-clr fixtures; run their case generators/oracles/full candidate CIL, run_shared_runtime.py, direct actual DLL invocation, check_closed_clear.py and independent isolation. Read the source/arguments in scripts before reproducing. All raw CIL/native/input/output logs, initial failure, exact final candidate, compiled fixtures, physical byte-delta variants and per-archive SHA manifests are retained. The archive is not a full SDK/Unity distribution.

## Actual R17 compiler readback: run 40166633598

Exact source commit `670bf20741f3b6c9f7ceb60db9ef3963be755741` completed fresh R17 IL2CPP conversion and all 756 C++/274 C Linux syntax checks. All 1030 returned generated source sizes/hashes were independently rechecked against their zero-error compiler records. The raw runner manifests bind GodsPVZRuntime1.dll to the exact fifty-method candidate and 21,467,136 bytes; these are runner-manifest readbacks, not a physical staged DLL download. All six compiler controls meet expectations and the real damaged R4 control still produces 37 errors. Actual raw logs, sources identities and downloaded archive hashes are retained. No Unity Editor import, Apple compilation, player linking, IPA or iPad acceptance is implied. Wider semantic/type/access queues remain held.
