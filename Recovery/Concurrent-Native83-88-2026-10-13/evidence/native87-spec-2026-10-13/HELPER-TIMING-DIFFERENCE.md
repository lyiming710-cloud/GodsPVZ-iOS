# Pre-existing CreateAudioAtPoint call-order difference

This is a static source finding, not a test result or a helper repair. Both Native87 caller specifications preserve their original direct call to public `GlobalStaticVars.CreateAudioAtPoint(AudioClip,Vector3,float32,float32)`, MethodDef `0x0600013E`. The current parent82 helper is outside these two target bodies and was read only.

Original PC helper source is the full split-pdata interval `[0x18031B350,0x18031B580)`, including alignment, SHA-256 `406900bf73b2fa0555b77bc1276a399669dc5992e6a8800f1012d4f5d9b34cad`. `PRIMARY-EVIDENCE.json` contains its disassembly and independently maps the audio getter from original AudioModule registration.

After AudioSource.Play:

| Operation | Original PC helper | Actual parent82 CIL helper |
| --- | --- | --- |
| AudioClip.length | `0x18031B4F7 -> 0x1812DBE00`; original AudioModule MethodDef `0x06000006` | IL offset 132 |
| First Time.timeScale | `0x18031B501 -> 0x18132C440` | IL offset 110 |
| Compare with float32 0.01 | `0x18031B50E`, `comiss 0.01, timeScale`; ordered greater branch chooses floor | IL offset 115, ordered `bgt` |
| Optional second Time.timeScale | `0x18031B515`; executes when the first comparison is false, including unordered/NaN | IL offset 117 |
| Clip length × selected factor | `0x18031B532`, length left operand / factor right operand, float32 multiplication | IL offset 138, same operand order |
| Destroy created GameObject | `0x18031B53F` | IL offset 139 |

Parent82 DLL SHA-256 is `48b9e89e941c041d6c11c4064ad05d9384a1a4dd51d5f8d3fad9192cbda5adbf`. Current helper raw MethodBody SHA-256 is `5520ac1a25114890963a4712d8865cd1027064d3d7423f015ec678e3a1ee6f77`, stored with all CIL in `MANAGED-SOURCE-EVIDENCE.json`.

The scalar branch and multiplication order agree. The **length/timeScale read order differs**. A length getter exception occurs before any timeScale call in the original PC body and after those calls in current CIL; callbacks or changing property state can therefore distinguish the helper. Do not describe complete end-to-end native equivalence solely from correct Native87 caller bodies. Do not silently replace or edit the existing helper under the two-method scope. A separate scoped helper repair, if chosen by the parent, should preserve the original property-read order and undergo Gemini's acceptance work.

This observed difference is original **player native versus recovered managed helper**, not proof of an intended Editor-specific feature. The inspected helper CIL has no `Application.isEditor`/Editor API branch. Full Core and exact-editor Core resolver DLLs inspected for public Random.Range wrappers are byte-identical, SHA-256 `83192116b34abac2bb1797ec0e3943362be33b028b1eefd2e561d5fdc1c3312f`. Platform-specific RNG/audio engine outcomes remain untested.
