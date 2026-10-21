# Native115 / Bulk200 independent execution requirements

Five authored bodies only, completing the previous twelve-method input set after Native114's seven bodies. The primary author does not construct candidates or execute project tests. AGY owns actual construction, independent verification, oracle/CLR execution and compiler work. Do not change main/production, historical evidence, frozen tools or restart Codespace.

## Parent, comparison and metadata identity

Always construct from fixed Native93 `b074224112d46a959de074d3090ab6d0614b036959c66c63b4f49954472e2162`. Latest candidate identity known to this source author is Bulk188 `1ed555461d3eb2c6003487ba36ec1a09d66e4061aba3c2c877c49f026f25be93`. Bulk195 was delivered as source; do not invent a Bulk195 candidate hash or presume it executed. Compared with Bulk188, Bulk200 should have **twelve** body differences: Native114's seven plus Native115's five; all other2,305 bodies must match. If an independently constructed/identified Bulk195 exists, additionally compare against it for exactly five differences and2,312 preserved bodies. SOURCE-LINEAGE.json distinguishes five new-source tokens from twelve comparison-delta tokens.

Expected aggregate:200 body sources (194 direct worklist targets+6 companions),2,117 non-target methods,39 finally regions, no new EH in this batch. MemberRef1192 / StandAloneSig1908 / TypeSpec175 / MethodSpec189. Cumulative additions from Native93:92 refs,143 locals,2 TypeSpecs,4 MethodSpecs;858-byte metadata-table row payload. New batch adds3 refs and5 local signatures, no TypeSpec/MethodSpec/#US.

The public Renderer.set_sharedMaterial MemberRef copies the existing SetMaterial signature and declaring type. Two **new field** references TMP_MeshInfo.uvs0/uvs2 copy the original vertices row shape/owner but have explicit **Vector2[]** signatures `061d11812d`, not the template's Vector3[] `061d114d`. METADATA-ADDITIONS.json keeps the canonical template pin and separately records emitted_signature/emitted_field_signatures. MetadataAppend.cs appends exactly two five-byte field blobs (12 bytes including compressed-length prefixes), narrowly pins the two tokens4A7/4A8, declaring TypeRef106, public field names and Vector2 TypeRef75, and writes only the new row signature handles. Independent auditing must validate actual emitted signatures rather than incorrectly assuming every cloned row keeps its template signature. Add negative controls for a Vector3[] substitution, wrong UV owner/name/signature handle and any old-row mutation. Preserve historical verifiers; version supplementary validation separately where needed.

## 065C PlantDataUIController.SetData(float)

Assign this.data before anything else. Capture String.Empty before the first Object.name call; capture that name once for the compiler-generated ComputeStringHash helper (MethodDef090C, assembly visibility) and one corresponding String.op_Equality collision guard. Preserve the seven exact hash/name/format bindings from native:

| Name | UInt32 hash | Single.ToString format |
|---|---|---|
| CD |7ADA1982|种植冷却:0.0s|
| ARM |BAABD2D5|护甲:0|
| SP |67029C76|阳光价格:0|
| HP |70EB52E5|生命值:0|
| DEF |4A0EFFAC|防御值:0|
| ATI |529C2493|攻击间隔:0.0s|
| ATK |549C27B9|攻击力:0|

Unknown/null/collision-mismatched names retain the empty format. Capture current textMeshProUGUI before formatting the **input argument's address** with Single.ToString(format); SetText(result,true). Do not format a later read of this.data, replace formatting with string concatenation, or inherit a damaged fake-hash local.

After SetText, obtain Object.name independently in order SP, ATI, CD, with short-circuit equality checks. Any match sets current strip0.fillAmount=0 then current strip1.fillAmount=0 and returns without color writes. Other names calculate float32 ratio=originalInput/currentCup, after all those calls.

Default fills0,0 and both colors white. All interval comparisons are ordered; negative/NaN ratio retains these defaults. Exact tiers:

| Ratio | Fill0 | Fill1 | Color0 RGB | Color1 RGB |
|---|---|---|---|---|
|[0,1]|ratio|0|1,1,1|1,1,1|
|(1,3]|1|(ratio-1)*.5|1,1,1|.61,1,.34|
|(3,7]|1|(ratio-3)*.25|.61,1,.34|.28,.9,1|
|(7,15)|1|(ratio-7)*.125|.28,.9,1|.51,.455,1|
|[15,+infinity]|0|1|1,1,1|1,.95,0|

Clamp both calculated fills with the existing real System.Math.Clamp(float,0,1), in order, before either UI setter. Then set strip0 fill, strip1 fill, strip0 Color(...,1), strip1 Color(...,1), re-reading each field. Test every boundary and nextafter neighbor, NaN/infinities/signed zero, zero/negative cup, name/text callbacks replacing fields/cup/data, and late failures preserving earlier writes. Mutants: signed hash corruption, no collision guard, cached name for later checks, formatting this.data after callback, misplaced range15 boundary, swapped palette components, omitted clamp, color writes on hidden paths. ComputeStringHash remains an independently executed dependency, not newly qualified by this body.

## 075C TMPro.Examples.CameraController.LateUpdate

GetPlayerInput is always called before Unity CameraTarget!=null. A Unity-null target returns afterward. Modes1 and0 update desiredPosition; other signed values leave it unchanged but still execute movement and rotation handling. Capture target.position first, then ElevationAngle and OrbitalAngle. Mode0 additionally captures current CameraTarget before Euler/offset work. Use existing **public Quaternion.Euler(float,float,float)**, followed by quaternion*Vector3(0,0,-FollowDistance). Original native inlined degree-to-radian multiplies use float32 constant0.01745329238474369. The saved genuine Core reference's public Euler body constructs the same Vector3, multiplies by that constant and calls Internal_FromEulerRad. Do not call private Internal_FromEulerRad from game code. Retain signed negation of FollowDistance, including signed zero/NaN.

Mode1 adds the rotated offset directly to captured target.position. Mode0 first applies captured target.TransformDirection(offset), then adds to captured position. Preserve float32 per-component addition. Do not re-read CameraMode to change the selected branch after callbacks.

Capture MovementSmoothing flag then current cameraTransform. Without smoothing, set that captured transform to current desiredPosition. With smoothing: capture that transform's position; capture MovementSmoothingValue and desiredPosition before Time.fixedDeltaTime; calculate smoothTime=fixedDeltaTime*capturedValue; then obtain a separate Time.deltaTime. Call real Vector3.SmoothDamp(current,capturedDesired,ref this.currentVelocity,smoothTime,+infinity,capturedDelta), passing Vector3 values and only the actual velocity field byref. Set position on the originally captured transform even if callbacks changed the field.

After position update, capture RotationSmoothing then a fresh cameraTransform. False uses that transform.LookAt(current CameraTarget). True captures its rotation, gets current target.position, then gets position of the newly re-read cameraTransform, computes float32 direction=targetPosition-cameraPosition, calls real Quaternion.LookRotation(direction), captures RotationSmoothingValue before a new deltaTime call, then Quaternion.Lerp(oldRotation,lookRotation,deltaTime*capturedValue) and assigns to the earlier captured rotation receiver.

Test all modes/flags, destroyed Unity targets, multiple transforms returned/replaced inside callbacks, rotation/movement values changed inside time calls, velocity writes through the real field, signed zero/NaNs/infinities, partial writes and receiver-null timing. Mutants: use deltaTime in place of fixedDeltaTime, reverse the two calls, fake non-field velocity ref, stale/fresh receiver changes, skip GetPlayerInput on null target, apply TransformDirection in mode1, omit it in mode0, byref value arguments and private Euler API. Bind actual GetPlayerInput and real math helper behavior or retain explicit integration UNPROVEN.

## 07AA TMPro.Examples.TextMeshSpawner.Start

Loop index begins0, condition re-reads current NumberOfNPC before each iteration. Read SpawnType per iteration: **zero selects TMP; every nonzero value selects legacy TextMesh**, including negative values. Create a default GameObject, capture its transform before two ordered Random.Range(-95f,95f) calls, and set position(x,.5,z) on that captured transform. Both random calls occur before a null captured transform is dereferenced for the setter.

Legacy: AddComponent<TextMesh>, then Component.GetComponent<Renderer> on that text component; obtain current TheFont.material before dereferencing the captured renderer for public set_sharedMaterial. Re-read TheFont for text.font, set anchor7, fontSize96(int), exact yellow Color(1,1,0,1), then text="!". TMP: AddComponent<TextMeshPro>, set fontSize96(float), text="!", then yellow Color, in this distinct order. Do not invent font/alignment/kerning setup for the TMP branch.

Both branches AddComponent<TextMeshProFloatingText> to the root GameObject, write the result into this.floatingText_Script, re-read that field and set SpawnType1 for legacy or0 for TMP, then increment. Use genuine existing closed MethodSpecs99/9A/9F and1F; their actual owners differ (AddComponent belongs to GameObject; GetComponent belongs to Component). Capture/re-read decisions are observable on callback field replacement. Test NumberOfNPC<=0, bounded counts with mutation, every SpawnType, TheFont null/material failure, exact order and floating-field replacement. Mutants: only allow legacy value1, cache loop count, color/text swap on TMP, skip second font read, assign material to a different renderer and add floating script to another object.

## 0753 TMPro.Examples.Benchmark02.Start

Loop condition uses current NumberOfNPC and each iteration re-reads SpawnType. Types0/1/2 have distinct paths; **all other values allocate nothing** in that iteration.

- Type0: create root GameObject, position(x,.25,z) with captured transform before two Range calls. Add TextMeshPro, autoSizeTextContainer=true, get rectTransform and set pivot(.5,0), alignment1026, fontSize96f, enableKerning=false, yellow, text="!", current IsTextObjectScaleStatic through the TMP setter. Add floating script to root, save/re-read the owner field, SpawnType0, then re-read owner IsTextObjectScaleStatic for the script's public field.
- Type1: create root and position(x,.25,z), Add TextMesh, Resources.Load<Font>("Fonts/ARIAL") before assigning font. Get Renderer from text, then get that text's current font.material before public sharedMaterial assignment. Anchor7, fontSize96int, yellow, text="!"; add floating script to root and set SpawnType1. No invented static-scale assignment in this branch.
- Type2: create **Canvas root GameObject**, Add Canvas, set worldCamera=Camera.main, set root transform.localScale(.1,.1,.1), then capture a fresh root transform before two Range calls and set position(x,5,z). Create a **separate child GameObject**, Add TextMeshProUGUI, get its rectTransform, get root transform afterward, SetParent(rootTransform,false). Then yellow, alignment1026, fontSize96f, text="!". Add floating script to the **Canvas root**, not text child, save/re-read field and SpawnType0. No invented Canvas.renderMode or static-scale settings.

Color packed constantFF00FFFF from native converts to exact yellow. Do not inherit damaged CIL's unmasked intermediate integers65280/16711935 as color components. Test each valid and invalid mode, allocation/callback order, changed counts/modes/static-scale flags, both objects in Canvas path and failures after earlier UI effects. Mutants: child instead of root floating script, wrong y/scale, SetParent(true), count caching, missing final static flag re-read, TMP/legacy property order and malformed Color/Vector arguments.

## 07CE TMPro.Examples.TMP_TextSelector_B.RestoreCachedVertexAttributes(int)

Argument-1 returns **before touching this**, including a null raw receiver when invoked nonvirtually. Other values first evaluate current textInfo.characterCount-1 and return only when argument is signed greater. Negative arguments other than-1 are not defensively filtered; array access raises the original exception. Preserve unchecked Int32 decrement behavior.

Use public TMP_Text.get_textInfo, whose supplied genuine support CIL is exactly ldarg.0/ldfld/ret, instead of illegal cross-type private m_textInfo access. The fixture must use this actual pure getter or a proven identical implementation, not arbitrary callback behavior for a pure accessor. Read materialReferenceIndex and vertexIndex from two separately acquired current characterInfo arrays. Value-type field bindings/physical offsets are saved separately; do not treat boxed-field offsets as array-element offsets.

For selected material, capture cached vertices and current vertices. Copy selected vertexIndex+[0,1,2,3] **sequentially**, by typed element values. Acquire current colors32 before cached colors32, copy four; acquire cached uvs0 before current uvs0, copy four; then cached/current uvs2 and copy four. Preserve null/index checking and source-before-destination element evaluation. No Array.Copy, vector-wide memcpy, normals copy or up-front validation that erases partial writes.

Compute lastQuadStart=(cachedVertices.Length/4)*4-4 from the original cached vertex array, **not vertexCount or current mesh capacity**. Copy four vertices at that index using the initially captured vertex arrays; then re-acquire cached/current colors32, uvs0, uvs2 in that order and copy each four, respecting aliasing and partial writes. Finally re-read current m_TextMeshPro and invoke UpdateVertexData((TMP_VertexDataUpdateFlags)255). This original method restores both the selected quad and the last complete cached quad.

Test selected==last and selected!=last, array lengths not divisible by4, aliased arrays across cache/current/UV channels, null arrays, each successive source/destination short-array position, material/character indices including-2, characterCount0 and Int32 extremes, and absence of UpdateVertexData on an earlier exception. Preserve exact array snapshots after partial failure. Mutants: omit the last quad, use normals instead of vertices, wrong UV field/signature, swap uvs0/uvs2, bulk-copy all channels first, validate all arrays before writing, guard every negative index, cache public textInfo across all acquisitions, wrong update flags and Vector2/Color32 byref-as-object substitutions.

## Gate and reporting boundaries

Construct twice and bind hashes; reject wrong parent/overwrite and preserve old195 source emissions. Independently validate table payload/pins/old metadata and all non-target methods, including InitLocals and39 finally regions. Add scoped negative controls for new UV signatures and parser/schema failures; a crash must never count as an intended rejection. Preserve the frozen type verifier's historical limitations and separate supplementary models.

Run real candidate-bound CIL/original-native behavior and justified mutants for these five, plus unresolved Native114/earlier items. Historical Native109's768 stubbed cases do not cover this batch. Preserve unresolved integration/EH states and do not claim new formal acceptance from compilation. Keep the support-profile identity/comparability task and overall failing game-TU status explicit. If Bulk195 has not begun downstream conversion, a combined Bulk200 run may avoid redundant conversion of195; preserve both source checkpoints and any already-produced195 evidence. No full Unity/Xcode/IPA run is requested by this source delivery.
