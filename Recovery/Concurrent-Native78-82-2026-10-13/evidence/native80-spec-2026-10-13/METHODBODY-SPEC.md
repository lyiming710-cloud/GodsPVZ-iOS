# Native80 source-only MethodBody specification

Scope: original-PC `Plant.SetBrightness(float)` / `0x060003A0` and `ProjectManager.PlayAudio_Click(Project)` / `0x06000206`. Only source/PE/metadata parsing was performed. No application native/CLR execution, acceptability test, build, candidate generation, existing-file edits, commit, or push was performed. Writes are confined to this new analysis directory. All testing remains with Gemini.

## Source identities

- `audit/stage9-native3/inputs/GameAssembly.dll`: `9ebd7ca996a5b03fb4a766f7a2502b660d581ddbbb36af4d7bf2f06a211da39d`.
- `audit/stage9-native3/inputs/global-metadata.dat`: `ad992341add498bd1018ac980f42171e0737b0793bf945c8714ad70a445b36b9`.
- Native55 records were used to locate targets and candidate reference identities only. Their damaged managed IL/local types/EH are not semantic authority.
- Original native `0x060003A0`: `0x1803583F0..0x180358593`, 419 bytes, SHA256 `42e763585b112498ad7a8869f8325602646e5ffc7bb612745ee3efc5430353d4`, re-read from the original PE.
- Original native `0x06000206`: `0x180323840..0x180323A24`, 484 bytes, SHA256 `538036afc036222f45277a753309d4394b7492ff2739f7728d5ea76150c972bb`, re-read from the original PE. The final 36 bytes at `0x180323A00..A24` are a nine-entry switch table, not executable instructions.
- Current parent inventory read: `audit/native80-codex-2026-10-13/metadata-current.json`, parent SHA256 `3b3bcfca51402e1eedf5d59fbe1bf9d923336730325550205f960bd8a8bceec6`. It supplies reference/local binding information only.

This specification is logical CIL, not candidate bytes. MaxStack values are direct source-level stack accounting, not verifier results. Concrete body headers, EH section storage, local signature tokens and byte isolation remain implementation work for the parent.

## Original generic closure and calls

Metadata registration at `0x1818C6D00`: Types count 27,557/table `0x1818EDEB0`; GenericInst count 5,659/table `0x181679AE0`; MethodSpec count 74,797/table `0x181684EF0`. The original initializer decodes kind with `encoded >> 29` and source index with `(encoded >> 1) & 0x0FFFFFFF`.

| Usage cell | Encoded | Original MethodSpec | Definition / closed arguments | Native use |
|---|---|---:|---|---|
| `0x181BABE78` | `0xC00232B9` | 72028 | method index 9190 / mscorlib `0x060023E7`, `List<T>.GetEnumerator`, class T = UnityEngine.GameObject | `0x180358479 -> 0x1808197F0` |
| `0x181BC34B8` | `0xC002284B` | 70693 | method index 9215 / `0x06002400`, `List<T>.Enumerator.MoveNext`, class T = GameObject | `0x1803584BC -> 0x180669280` |
| `0x181BC3510` | `0xC002284D` | 70694 | method index 9217 / `0x06002402`, `List<T>.Enumerator.get_Current`, class T = GameObject | initialized in prolog; current read inlined at `0x1803584C5` |
| `0x181BC3460` | `0xC0022849` | 70692 | method index 9214 / `0x060023FF`, `List<T>.Enumerator.Dispose`, class T = GameObject | normal `0x180358541`, exceptional `0x180358554`, shared target `0x180302170` |
| `0x181BA51B8` | `0xC00244EF` | 74359 | Unity Core method index 36969 / `0x06000820`, public instance `GameObject.GetComponent<T>`, method T = SpriteRenderer | `0x1803584DA -> 0x18046DC80` |
| `0x181BA4278` | `0xC0023075` | 71738 | mscorlib method index 9164 / `0x060023CD`, `List<T>.get_Item(int32)`, class T = AudioClip | `0x180323997 -> 0x180825710` |

The helper's passed original MethodInfo usage cell fixes the generic closure; a shared native address alone would not identify its generic type. In particular, `Enumerator<System.Object>` in the damaged current SetBrightness locals is incorrect for the active iterator.

The original Unity Core and Assembly-CSharp CodeGenModule method-pointer tables were independently located again from image name strings/references. All ordinary call identities below were re-derived through those tables in `original-source-inspection.json`.

## Plant.SetBrightness(float), 0x060003A0

Original fields:

- `Plant.animationSprites`, `0x040004DF`, native instance offset **+0x148 (328)**, `List<UnityEngine.GameObject>`, private. `animationGroup` is a different field at +0x150 and is not accessed here.
- `Plant.Mat_Brightness`, `0x04000507`, native static storage offset +0x20, System.String, private static readonly.

There are **no Color operations, float constants, brightness arithmetic, clamping, or special brightness-zero branch** in this method. The single input brightness is forwarded unchanged to every SetFloat call.

Native sequence and null/order contract:

1. Read animationSprites once at `0x18035845D`. Raw null calls the NRE path `0x18035858D`; this happens before an enumerator is successfully acquired. No cleanup runs for that failure.
2. Get a typed `List<GameObject>.Enumerator` once at `0x180358479`; capture/copy the returned 24-byte value at stack +0x40. The captured list is retained throughout iteration.
3. Each iteration first calls MoveNext at `0x1803584BC`. If false, call typed Dispose at `0x180358541`, then return. An empty list follows that same false/Dispose/return path. There is no other normal early return.
4. When MoveNext is true, read the enumerator's current GameObject from its +0x10 current field at `0x1803584C5`. Raw null -> NRE at `0x180358581`.
5. Call the current GameObject's public generic GetComponent<SpriteRenderer> once at `0x1803584DA`. Capture its returned SpriteRenderer. Raw null -> NRE at `0x18035857C`.
6. Call the renderer material getter at `0x1803584ED`, original target `0x1812FAE70`, once. Capture the returned material in rdi. Then initialize Plant static storage if necessary. Raw null material -> NRE at `0x180358576`.
7. Read the current static Mat_Brightness field at `0x180358526` and call `Material.SetFloat(string,float)` at `0x18035852D`, target `0x1812EFC70`, once with the captured material and unchanged input brightness. Then loop to MoveNext.
8. Exceptions after successful enumerator acquisition enter the native cleanup block at `0x180358548`, call typed Dispose at `0x180358554`, and propagate the pending exception through `0x180358587 -> 0x180200E60`. They do not return normally.

The original RUNTIME_FUNCTION is exactly `0x1803583F0..0x180358593`. UNWIND_INFO `0x181A8D9C0` has flags 3 (native EH/UH), handler `0x1802C70FC`, compressed FuncInfo at `0x181A8D9D8`. This agrees with the visible normal and exceptional Dispose paths. The compressed FuncInfo was not falsely interpreted as an old fixed-width FuncInfo3. A CIL foreach-style **finally** is required; the damaged body reporting zero EH does not erase the native cleanup.

Captured receiver / callback effects:

- Replacing `Plant.animationSprites` during GetComponent/get_material/SetFloat does not replace the already captured enumerator list. Mutating that original list's version is observed by the next MoveNext and throws the normal enumeration-version exception. An index/count loop would change this behavior and is not an equivalent recipe.
- The current GameObject, returned SpriteRenderer and returned Material are captured across subsequent callbacks. Replacing a list element does not retroactively replace the current callback receiver.
- Mat_Brightness is read after the material getter, **again for each iteration**. Do not hoist or replace it with a guessed literal name.
- This method performs **no Unity Object op_Implicit/op_Equality/op_Inequality call**. Unity pseudo-null is not an early skip. Only raw managed/native null guards exist at the list/current/component/material boundaries; a non-null destroyed Unity object reaches the engine operation and its engine-defined failure.

Public material helper proof:

- Original metadata private `Renderer.GetMaterial()` is Unity Core MethodDef `0x06000382`, flags `0x81`.
- Original metadata public `Renderer.get_material()` is Unity Core MethodDef `0x06000397`, flags `0x886`.
- Both original method-pointer table entries are **exactly `0x1812FAE70`**, the target called by SetBrightness. This is the original-PC mapping evidence authorizing use of the public accessor instead of the private member.
- The local full Unity Core reference is SHA256 `83192116b34abac2bb1797ec0e3943362be33b028b1eefd2e561d5fdc1c3312f`, 1,532,928 bytes. Its public get_material MethodDef `0x06000EC9`, RVA `0x17234`, CodeSize 39, contains an IsPersistent branch: if persistent, LogError and return null; otherwise call private GetMaterial `0x06000E72` and return it. Complete helper bytes/IL are in `reference-helper-source-inspection.json`.
- Consequently, **do not claim that the full reference getter is an unconditional IL wrapper**. The unconditional original-PC equivalence witness is the independently re-derived original method-pointer alias. Any target/editor helper behavior differing from that original alias remains a Gemini/target-runtime qualification concern.

Public generic GetComponent proof: full Core reference public instance generic method `0x06001B3B`, signature `3001001e00`, CodeSize 50, initializes CastHelper<T>, uses typeof(T), calls private GetComponentFastPath, and returns the helper's t field. Complete IL is archived. Original MethodSpec 74359 closes this official generic method to SpriteRenderer.

Typed Current/Dispose proof: full `audit/stage9-native3/resolver/mscorlib.dll` is 4,622,848 bytes, SHA256 `4d1725f1ae54a22f69b79c2b1569181ed6653dd484c35005e29c40300c6673be`. Its correctly nested `System.Collections.Generic.List<T>/Enumerator.get_Current`, full-ref MethodDef `0x06006862`, is `ldarg.0; ldfld !0 ...::_current; ret` (7 bytes). Dispose, full-ref MethodDef `0x0600685F`, is `ret` (1 byte). Original MethodSpec closure fixes T=GameObject; original shared Dispose body `0x180302170` is also exactly `ret`. These tokens belong to the full reference, not the stripped original image.

Proposed MethodBody: MaxStack 3, active local `List<GameObject>.Enumerator E`, one finally. Parent can clone existing 15-slot local signature and retype active V_10 from Enumerator<object> to Enumerator<GameObject>, using V_10 for E. No other active local is required. Existing unused locals may remain unused. Entry GetEnumerator stays outside the try; MoveNext/current/GetComponent/get_material/SetFloat are within it; Dispose is the finally handler; successful completion leaves to the return after the handler.

```cil
    ldarg.0
    ldfld class List<GameObject> Plant::animationSprites
    callvirt instance valuetype List<GameObject>/Enumerator List<GameObject>::GetEnumerator()
    stloc E
TRY_START:
    br CHECK_NEXT
BODY:
    ldloca E
    call instance GameObject List<GameObject>/Enumerator::get_Current()
    callvirt instance !!0 GameObject::GetComponent<SpriteRenderer>()
    callvirt instance Material Renderer::get_material()
    ldsfld string Plant::Mat_Brightness
    ldarg.1
    callvirt instance void Material::SetFloat(string, float32)
CHECK_NEXT:
    ldloca E
    call instance bool List<GameObject>/Enumerator::MoveNext()
    brtrue BODY
    leave RETURN
TRY_END = FINALLY_START:
    ldloca E
    call instance void List<GameObject>/Enumerator::Dispose()
    endfinally
FINALLY_END = RETURN:
    ret
```

Each callvirt above targets the exact nonvirtual instance operation while supplying the native raw-null behavior. There is no conditional null skip. For the material case, argument evaluation reads the Plant static field before the callvirt null check; native performs Plant class initialization, then its raw-null check, then the plain static field load. Both preserve class-initialization faults before the material NRE, and no callback occurs between that raw-null check and the plain field load. The captured material receiver remains on the evaluation stack.

Current-parent binding evidence: generic SpriteRenderer GetComponent `0x2B000011`; public Renderer.get_material `0x0A00021D` (the damaged target uses private `0x0A0001E6`, which must not be retained); Material.SetFloat(string,float) `0x0A00003E`; typed GameObject GetEnumerator `0x0A0000FD`, Current `0x0A0000FE`, MoveNext `0x0A0000FF`, Dispose `0x0A000128`. Existing local signature is `0x110002DA`, currently V_10 and V_12 are Enumerator<object>; only a verified, scoped signature update may establish the active typed slot.

## ProjectManager.PlayAudio_Click(Project), 0x06000206

Original fields: Project.projectType `0x04000515`, instance +0x20, ProjectType with int32 underlying storage; ResourceManager.projectClips `0x040002F0`, static +0x128, List<AudioClip>.

Constants read from original PE:

| Address | IEEE-754 bytes little endian | Bits | Float literal |
|---|---|---|---|
| `0x1815A7A10` | `0000803f` | `0x3F800000` | 1.0f |
| `0x1815A7A14` | `cdcc8c3f` | `0x3F8CCCCD` | 1.1f |
| `0x1815A7AC4` | `6666663f` | `0x3F666666` | 0.9f |
| `0x1815A7ABC` | `cdcc4c3f` | `0x3F4CCCCD` | 0.8f |
| `0x1815A7AD0` | `6666e63f` | `0x3FE66666` | 1.8f |

The exact jump table is authoritative for numeric ProjectType branching:

| Type int32 | Original target | Clip index | Volume | Pitch |
|---|---|---:|---|---|
| 0 | `0x1803238DF` | 4 | captured AudioVolume result | Random.Range(0.9f,1.1f) |
| 1,2,8 | `0x1803238FE` | 1 | captured AudioVolume result * 0.8f | Random.Range(1.0f,1.8f) |
| 4 | `0x180323906` | 1 | captured AudioVolume result | Random.Range(1.0f,1.8f) |
| 3,5,6,7 | `0x180323921` | 0 | captured AudioVolume result | 1.0f, no Random call |
| negative or >8 | unsigned out-of-range branch to `0x180323921` | 0 | captured AudioVolume result | 1.0f, no Random call |

Metadata enum fields are named Sun, Coin_Sliver, Coin_Gold, Diamond, Moneybag, Trophy, Present, Chocolate, Coin_Copper. Their original compressed default storage begins with bytes 00,02,04,06,08,0A,0C,0E,10; the source JSON records those bytes and locations. This specification does not misread those version-31 compressed defaults as fixed-width int32s. CIL branching is expressed numerically from the actual machine jump table and does not depend on an inferred enum-name assignment.

Call identities:

- `0x18031AEA0`: GlobalStaticVars.AudioVolume(), Assembly-CSharp `0x06000138`, static ()->float.
- `0x18130D350`: UnityEngine.Random.Range(float,float), original Core `0x0600079B`; int Range is a different address `0x18130D310` and is not used.
- `0x1812E27C0`: Camera.get_main(), Core `0x060001C1`, static ()->Camera.
- `0x181304510`: Component.get_transform(), Core `0x060007E6`, instance ()->Transform.
- `0x18132F3A0`: Transform.get_position(), Core `0x06000994`, instance ()->Vector3 by value.
- `0x180825710`: List<AudioClip>.get_Item(int32), closed by original MethodSpec 71738 above.
- `0x18031B350`: GlobalStaticVars.CreateAudioAtPoint(AudioClip,Vector3,float,float), Assembly-CSharp `0x0600013E`, static ->AudioSource. The 1/2/3-argument overloads have different original addresses and are not called.

Native sequence and callback capture:

1. Initialize/call GlobalStaticVars.AudioVolume exactly once at `0x1803238A8`, **before checking the Project parameter for raw null**. Capture the return in xmm7. Initialize pitch=1 and clipIndex=0. Null Project raises NRE at `0x1803239F9`; the volume call already occurred.
2. Read projectType exactly once at `0x1803238C3`, after AudioVolume. Dispatch using that captured int32. AudioVolume may mutate the project before this read; Random callbacks after the read do not cause another projectType read.
3. For 1/2/8, native mulss multiplies the captured volume (left operand) by 0.8f before Random.Range. Type 4 bypasses that multiplication. Preserve float32 constants and this operation order; no clamp/recompute is present.
4. Run zero or one Random.Range call as specified above, capturing pitch. Then call Camera.get_main exactly once at `0x180323923`. Raw null camera -> NRE before any transform call.
5. Call get_transform on that captured returned camera once at `0x180323936`; raw null returned Transform -> NRE before get_position. Call get_position once at `0x18032394F`, copying/capturing all 12 bytes of the returned Vector3.
6. Only after get_position, initialize ResourceManager static storage if necessary and read ResourceManager.projectClips once at `0x180323982`. Raw null list -> NRE. Call get_Item on that captured list at `0x180323997`, once, using the captured clip index. Capture the returned AudioClip in rbx; there is no clip null guard.
7. Call the four-argument CreateAudioAtPoint once at `0x1803239D6`, passing captured clip, captured camera position, captured/scaled volume and captured pitch in that order. Ignore/pop its AudioSource result. A null return is not inspected or dereferenced. Return normally if that call returns.

The method does not access `this` ProjectManager, write any fields, perform Unity pseudo-null comparisons, re-read Camera.main/projectType/projectClips, or re-fetch AudioVolume. Camera/Transform callbacks can affect the later ResourceManager.projectClips read. A list replacement during get_Item cannot replace that call's captured list or the returned clip passed onward. Audio callbacks do not trigger another camera/position/volume/pitch read.

All explicit object faults here are raw-null faults. Camera.main returning a non-null destroyed object does not cause a Unity-style early return; the next engine operation determines its error. The native RUNTIME_FUNCTION for this method has UNWIND_INFO flags 0, no handler. Proposed CIL has no EH.

Proposed MethodBody: MaxStack 4; float volume, float pitch, int32 clipIndex, int32/project-enum type, Vector3 position, AudioClip clip as named locals. Existing parent V_0/V_17 are Single, V_1 is ProjectType, V_3 Camera, V_4 Transform, V_8 Vector3, V_9 AudioClip. The implementation may store clipIndex in V_1 after dispatch (same int32 evaluation/storage representation) and keep the existing signature; this is a local allocation decision, not a changed branch contract. No System.Object slot needs to act as a struct.

```cil
    call float32 GlobalStaticVars::AudioVolume()
    stloc volume
    ldc.r4 1.0
    stloc pitch
    ldc.i4.0
    stloc clipIndex
    ldarg.1
    ldfld ProjectType Project::projectType
    switch (SUN, COIN, COIN, COMMON, MONEY, COMMON, COMMON, COMMON, COIN)
    br COMMON
SUN:
    ldc.r4 0.9
    ldc.r4 1.1
    call float32 Random::Range(float32,float32)
    stloc pitch
    ldc.i4.4
    stloc clipIndex
    br COMMON
COIN:
    ldloc volume
    ldc.r4 0.8
    mul
    stloc volume
MONEY:
    ldc.r4 1.0
    ldc.r4 1.8
    call float32 Random::Range(float32,float32)
    stloc pitch
    ldc.i4.1
    stloc clipIndex
COMMON:
    call Camera Camera::get_main()
    callvirt instance Transform Component::get_transform()
    callvirt instance Vector3 Transform::get_position()
    stloc position
    ldsfld class List<AudioClip> ResourceManager::projectClips
    ldloc clipIndex
    callvirt instance AudioClip List<AudioClip>::get_Item(int32)
    stloc clip
    ldloc clip
    ldloc position
    ldloc volume
    ldloc pitch
    call AudioSource GlobalStaticVars::CreateAudioAtPoint(AudioClip,Vector3,float32,float32)
    pop
    ret
```

Current-parent references already include AudioVolume `0x06000138`, Random float Range `0x0A000083`, Camera.main `0x0A000004`, Component.transform `0x0A000001`, Transform.position `0x0A000037`, List<AudioClip>.get_Item `0x0A000084`, CreateAudioAtPoint4 `0x0600013E`; no alternate overload is required.

## Handoff boundaries

- Source evidence is complete for generic closure, ordinary call targets, exact float constants, branch grouping, field capture/re-read and raw-null behavior in these two bodies.
- Public material accessor equivalence is grounded in the original-PC method-pointer alias. The real full-reference getter's additional IsPersistent branch is preserved in the helper evidence and must not be hidden in the claim.
- Enumerator EH/local retyping and concrete storage/metadata binding remain scoped parent implementation work. There is no acceptance claim and no test was run by this subtask.
- Evidence files: `original-source-inspection.json`, `native-helper-source-inspection.txt`, `reference-helper-source-inspection.json`; each has a companion read-only parsing script. Runtime NRE factory identity was previously traced from the same original PE in `audit/native78-spec-2026-10-13/runtime-helper-source-inspection.txt`.
