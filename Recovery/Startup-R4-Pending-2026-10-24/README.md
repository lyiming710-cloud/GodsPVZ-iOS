# Startup R4 — pending behavior qualification

This is a five-method construction checkpoint, **not an accepted replacement DLL**. Shader R1's Unity workflow still uses Startup R3 (`752939d1…cc2e7f`). No R4 Unity/IL2CPP/IPA/device claim is made.

The candidate SHA-256 is `530550053cfae3d8be6ea886f87ee77b74e5c1cc8c2b96517b5ac18d3cbb4ec9` (18,429,952 bytes). Repeated construction produces identical bytes. Scope:

* `0x06000089 System2.ChangeEnemyType`: use the original public `TMP_InputField.get_text` for its protected `m_Text` read. Original PC TMP image module/pointer and native `mov rax,[rcx+0x220]; ret` are archived.
* `0x060002AD Board.Update_isWon`: restore three float-zero constants and two existing singleton getter calls. Native `COMISS 0,endTime; JB` requires the final `clt.un`, including unordered/NaN; the separate boolean-zero comparison stays integer.
* `0x060005E7 BuffUIController_Zombie.FixedUpdate`: restore two float-zero constants and three original-backed AttackPoint getter reads. Two native `MOV r8d,0xffffffff` arguments are Int32 -1, not Int64 4294967295. Each 9-byte damaged literal becomes 5-byte `ldc.i4 -1` plus four NOPs, preserving every old IL target and body size. Existing `!=0` comparison keeps native JP/JNE NaN behavior.
* `0x060007B7 / 0x060007B8 TMP_TextEventCheck.OnEnable/OnDisable`: close the link listener constructor as `UnityAction<string,string,int>` and use ten existing public event getters.

The frozen checker is unchanged (`2bb0a8c2…63d14`). It passes the 44-method union of the old 40-target startup set and these five targets (System2 overlaps). The independent member-access checker passes all five new targets with zero unresolved/rejected references. These are static gates only.

The independent raw audit imports no patch constructor. It verifies all 2,317 method rows, the exact five expected body deltas, 2,312 untouched methods, identical old metadata rows/heaps, two appended MemberRefs, one appended TypeSpec and one 9-byte blob entry. Headers, RVAs, locals, branches, EH regions, visibility, original SerializeField markers and the R3 helper-root repair stay unchanged. Seven physical isolation mutants are rejected.

The first candidate passed four of five stack checks but exposed two additional malformed Int64 literals in BuffUIController. Its actual DLL/CIL/report remain in `FIRST-ATTEMPT-STACK-FAILURE.zip`; it is not promoted. The Int32 correction is bound to native addresses `0x18039A604` and `0x18039A767`.

Next required work: execute the actual restored CIL in controlled fixtures, compare normal paths and side-effect traces against original native code, retain invalid controls, then qualify any candidate compiler/startup run separately. Native CFG evidence currently follows normal direct control flow only; it does not model Windows exception unwinding. Administrator.Start_关卡配置文件 and other previously held access/method defects remain open. Construction counts must not be presented as semantically accepted game methods.
