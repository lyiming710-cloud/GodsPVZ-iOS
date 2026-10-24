# Startup R14: prop sentinels and complete TMP constructor initialization

Exact candidate `9b53867fb47378190f147f9b474ed24525b15dde44ce753143e56934aaf2c491` (20,166,656 bytes), directly based on exact R13 `e6452513ce3c5a3d9449d8c7287aae39a8458cdc8e4eec09d199769aa1794b4b`.

## Complete caller reconstruction

Five MethodDefs: PropChooseSystem.DropDown (0x06000694), Cancle (0x06000695), Fold_PropList (0x06000696), TMPro.Examples.TMP_TextSelector_A..ctor (0x060007C4) and TMP_TextSelector_B..ctor (0x060007CF). The original spelling Cancle is retained. These five managed body slots are distinct, although Cancle and Fold share the same original native function pointer.

The three prop callers reset Int32 currentPropBankID to -1 and call list GameObject.SetActive(false). DropDown first calls Select(null, oldID), captures the list after Select, and then resets the ID before SetActive. Receiver capture and writes preserve original register lifetime. Native null guards correspond to CIL callvirt throwing; no default-on-null shortcut is added. Select and SetActive are controlled external contracts in these tests, not recovered callee implementations.

Each original TMP constructor writes qword -1 into two neighboring Int32 fields, then dword -1 into the third, before the MonoBehaviour base constructor. Previous managed bodies omitted the middle field entirely and wrote an Int64 -1 bit pattern to the final Int32. Selector A now initializes selectedLink, lastCharIndex and lastWordIndex. Selector B initializes selectedWord, selectedLink and lastIndex. All three values are -1 before the controlled base constructor executes; mutations made by that base callback remain visible afterward.

## Actual execution and controls

All five complete actual candidate CIL bodies execute through the existing full instruction emitter, with no instruction removal or extra return. 450 original-PC CPU versus CIL records match: 70 each for prop DropDown/Cancle/Fold, 120 each for the TMP constructors. Cases cover Int32 extremes, arbitrary seeded index values, retained neighboring hover flags, Select mutation/list replacement, SetActive ID mutation/list replacement, and base-constructor mutations to middle/last fields. Original caller bytes are verified before and after controlled hooks. Three separate managed null controls throw NullReferenceException after writing -1, with Select first on DropDown. Original native throw/SEH and real Unity side effects are outside scope.

Nineteen physical semantic faults are detected, including omitted index writes, zero resets, active=true, missing Select/base calls, wrong Select ID and writes moved after callbacks. The twentieth restores an exact historical Int64-reset body. CLR accepts its width defect and produces no runtime difference; the unchanged frozen width checker rejects I8 -> Int32. The runtime non-detection is preserved, and runtime matching alone is not a width proof. Four physical out-of-scope mutations are independently rejected by the raw boundary audit.

## Isolation and broader status

Independent raw checks compare every body and the whole file, without reading the writer plan. Exactly five bodies change; all other 2,312 MethodDefs, all metadata, RVAs, private/local signatures, MaxStack, visibility and SerializeField declarations remain byte-identical. Only target CodeSize fields and bounded shortened code bytes change. Unused old slot bytes remain intact. A second fresh process reconstructs the same candidate hash. All five changed methods prepare under actual-DLL CLR JIT and pass the unchanged frozen checker. No new full-assembly JIT run is claimed.

Full 2,297-body checks: raw frozen 2,154 PASS /143 failure-or-unsupported; exact InterfaceImpl supplement 2,182 PASS /115 failure-or-unsupported (77 type/metadata conflict leads and 38 checker-unsupported leads). Access retains 145 rejected sites across 70 methods, zero unresolved, actual auditor exit 2. Zero unterminated lexical tails. These overlapping queues are not a count of proven gameplay bugs. Prior R13's 24 scoped callers/18,840 records remain parent-bound; their bodies and metadata are unchanged here, but are not relabeled as a fresh R14 runtime replay.

At initial publication no R14 IL2CPP, Unity Editor, Apple compiler, player link, unsigned IPA or iPad acceptance has run. R13 separately passed fresh IL2CPP and Linux compilation of all 756 C++/274 C sources, with every source hash independently read back. Earlier R8 Editor/shader and R10 Apple checks remain bound to their exact inputs. The reported iPad black-screen/flicker remains unaccepted.

## Reproduction

Use the exact R13 candidate and immutable original PC GameAssembly SHA `9ebd7ca996a5b03fb4a766f7a2502b660d581ddbbb36af4d7bf2f06a211da39d`, original native-fields map, existing authentic SDK support, Cecil exporter/auditor and .NET/Python dependencies. Extract the source archive under the existing ipad-startup work root. The sibling historical archives retain tools/support references; this archive is not a full SDK or Unity installation.

Run research_and_construct.py only with fresh candidate outputs; --repeat writes candidate-reproduced.dll. Run prepare_checks.py, full_type_census_r14.py, interface_facts_r14.py and verify_independent.py. Build runtime/Fixture.csproj, run generate_cases.py, native_oracle.py CASES.json NATIVE.json and the actual CIL fixture with candidate/CASES/output arguments, then run_runtime.py. Run full_member_access.py (expected retained exit 2), census_lexical_tails.py and run_changed_exact_jit.py separately. Do not hide a nonzero access exit by chaining shell commands. Physical fault DLLs are reconstructable from PHYSICAL-VARIANTS.json; all outputs, native CFG bytes, complete CIL, frozen checker identity and per-archive member hashes are retained.
