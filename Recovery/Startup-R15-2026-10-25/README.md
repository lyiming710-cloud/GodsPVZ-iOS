# Startup R15: restore SwfList<T> initialization

Exact candidate `261068dda5af1f6915ea7c2183fa8b2b0d69092db6777e5643a39552669ccba1` (20,599,808 bytes), directly based on R14 `9b53867fb47378190f147f9b474ed24525b15dde44ce753143e56934aaf2c491`.

## Recovered behavior

Three methods: SwfList<T>..ctor() (0x060008E3), .ctor(Int32) (0x060008E4), .cctor() (0x060008F1). These initializer bodies retained RGCTX/native-memory placeholder strings and had lost real field/array initialization, despite earlier repairs to Pop/Push/indexing. This checkpoint does not repair SwfAssocList<T>'s two still-damaged constructors.

The static constructor creates a new zero-length T[] and assigns the private static readonly _emptyData field. The default constructor calls Object..ctor, assigns the shared empty array to _data and writes Int32 _size=0. The capacity constructor calls Object..ctor; negative capacity throws ArgumentOutOfRangeException with paramName `capacity` and exact original message `capacity must be >= 0` before changing the receiver's data/size. Capacity zero reuses _emptyData; positive capacity creates a new T[capacity]; size is then set to zero. Array.Empty<T> is not substituted, preserving the original new-array/static-sharing structure.

Original generic bindings are independently rechecked against PC CodeRegistration 0x1815E88C0, MetadataRegistration 0x1818C6D00, MethodSpecs, generic function rows, method tokens and pointer-array entries. Both object and fully-shared generic instantiations resolve to the same three original initializer bodies. Complete direct normal/negative-branch CFG bytes, original parameter/message string slots and the original Object..ctor alias's `ret 0` (C2 00 00) bytes are retained. The base helper is executed unchanged, without replacing it by a stub.

## Actual tests and limits

The CLR fixture directly invokes real methods in the actual DLL, with explicit Int32, String and a 24-byte value-type instantiation. It does not rehost or delete source instructions and uses no Reflection.Emit. 504 normal constructor records plus three actual automatic static-initializer observations match original PC CPU execution. Cases seed arbitrary instance sizes (including Int32 extremes) and null/zero/nonempty previous data; they compare array length/default values, identity against old data/shared empty storage, and final size. Nine managed negative-capacity cases preserve receiver data/size and the original exception class/parameter/message. Native Windows exception allocation/throw/SEH are read but not executed.

Original native array allocation, class-ready state and GC write barriers use explicit controlled contracts; this is not proof of real engine initialization, GC allocation failures, OOM equivalence, all generic contexts or full gameplay. All caller bytes are checked after installing controlled hooks. All nine closed actual methods prepare under CLR JIT. Six finite Int32/String models pass the unchanged frozen checker, with wrong receiver, wrong array type and wrong width controls rejected. These finite models supplement rather than overwrite the global raw census.

Eleven physical fault DLLs are detected: wrong static array length, missing shared assignment, allocating instead of sharing, retaining old data, wrong size resets, inverted capacity-zero branch, unsigned negative-capacity guard, size mutation before throwing, swapped exception arguments and restored historical placeholder bodies. The last three argument/guard/order faults show no differences on positive cases and are detected specifically by managed negative tests. No native exception equivalence is claimed.

The first fixture failed 420 setup cases because it attempted to overwrite _emptyData through reflection after real type initialization. The field is InitOnly; CLR correctly rejected that operation. The failed source, cases and raw records are retained, not counted as successful tests. The corrected fixture observes the real readonly static initialization and seeds only instance fields. Candidate bytes, visibility and readonly flags did not change to accommodate testing.

## Isolation and remaining gates

Independent whole-prefix, raw-body, metadata-table/heap and decoded CIL checks preserve all 2,314 other MethodDefs, every old metadata row/heap, target RVAs/locals/MaxStack, visibility and readonly flags. Only three in-place code/CodeSize fields change. One legal generic _emptyData MemberRef is appended by copying the existing _data field signature and SwfList<!0> owner; the new metadata root is placed in the existing read-only append section. Unused original slots remain intact. Fresh process reconstruction matches exactly. Four out-of-scope physical faults are rejected.

Full 2,297-body census: 2,155 raw frozen PASS /142 failures-or-unsupported; exact InterfaceImpl supplement 2,183 PASS /114 failures-or-unsupported. The retained set includes generic checker limitations; finite closed results are not relabeled as a universal proof. Access still rejects 145 sites across 70 methods, zero unresolved, actual audit exit 2. Zero unterminated lexical tails. The two SwfAssocList constructors are a known next dependency; R15 does not make that whole initialization chain correct.

Exact R14 separately passed fresh IL2CPP and Linux syntax checks of all 756 C++/274 C files in run 40137829491, with all 1,030 downloaded source hashes verified. At initial publication R15 has no fresh IL2CPP/Unity Editor/Apple build, player link, unsigned IPA or iPad acceptance. The iPad black-screen/flicker remains unaccepted.

## Reproduction

Use the exact R14 candidate, authentic SDK support and original PC GameAssembly SHA `9ebd7ca996a5b03fb4a766f7a2502b660d581ddbbb36af4d7bf2f06a211da39d` and metadata SHA `ad992341add498bd1018ac980f42171e0737b0793bf945c8714ad70a445b36b9`. Existing Python dnfile/pefile/Capstone and .NET SDK, Cecil exporter/access auditor and raw metadata helpers are required. Original inputs/dependencies are not silently substituted by dummy DLLs.

Run research.py; run construct.py with fresh output DLL/report paths twice. Run prepare_checks.py, full_type_census_r15.py, interface_facts_r15.py, check_closed_types.py and verify_independent.py. Build runtime/Fixture.csproj, run generate_cases.py, native_oracle.py CASES.json NATIVE.json and direct CLR Fixture.dll candidate CASES.json ACTUAL-CIL.json, plus NEGATIVE-CASES.json -> MANAGED-NEGATIVE.json, then run_runtime.py. Run full_member_access.py separately (retained actual exit 2) and census_lexical_tails.py. All failed/corrected fixture sources, complete native/CIL exports, raw reports, compiled CLR fixture, reconstructable physical faults and SHA manifests are archived. No full SDK/Unity install is bundled.

## Actual compiler readback: run 40139239336

Exact source commit `235713abe1a380afa2ff590c9dd2b630db126507` completed fresh R15 IL2CPP conversion and Linux syntax checks for all 756 C++ and 274 C files. Downloaded generated archive was streamed independently: every one of the 1,030 source sizes and hashes matches its zero-error compiler record. The raw runner input manifests identify candidate GodsPVZRuntime1 as the exact R15 hash and 20,599,808 bytes; those manifests are returned, not the physical staged DLL. Fresh historical R4 control still produces 37 errors, and all six C/C++ controls meet expectations. Full raw reports/logs and archive identities are retained. This is license-free Linux compiler evidence; no R15 Unity Editor, Apple compiler, player link, IPA or device acceptance is claimed. Wider gates remain held.
