# Native114 / Bulk195 independent execution contract

Primary delivery is source only: seven bodies, four public Unity MemberRefs and seven local signatures. No candidate construction, compilation, converter, oracle, fixture or project tests have been run by the primary author. AGY owns execution in an independent Actions runner. Do not change main/production or start the exhausted Codespace.

Construct from the fixed Native93 parent `b074224112d46a959de074d3090ab6d0614b036959c66c63b4f49954472e2162`. Compare inherited bodies to the fixed Bulk188 candidate `1ed555461d3eb2c6003487ba36ec1a09d66e4061aba3c2c877c49f026f25be93`. That predecessor has conversion progress on an overlaid support profile and failed Linux compilation, not formal promotion. Never use Bulk188 as the construction parent.

## 018C DeviceManager.SSS_CreateIceCube

Construct DeviceData(3), then set name "未知的冰块", icePoint 6000f, camp 2, maxIcePoint 6000f in that order. Construct the original parameterized PreDevice(gridX,gridY,data,String.Empty), whose constructor retains its default DeviceData(-1) initializer and subsequent field writes. Do not pretend that constructor or DeviceData constructor has independent behavior qualification because its caller compiles. Record these dependencies separately.

Allocate DeviceContent before the Random.Range(0,5) call, and write contentType=2 before calling Range. The result map is 0->0, 1->2, 2->4, 3->14, 4->7; any other signed test result falls back to0, matching native default. Write contenID, data.content, preDevice.data again, then invoke the public closed List<PreDevice>.Add. The unused third method parameter remains unused. No guards around this.preDeviceList, no private List fields, no substituted dummy PreDevice.

Exercise every random result, invalid indices only where the actual method can observe them, null list after prior effects, constructor events/allocations and Range callback mutation of the owning preDeviceList. Mutants: map1->0, camp omission, Random before content initialization, reuse default -1 data instead of the created data, omit Add. Constructor dependencies remain unproven until actually executed/bound.

## 030B Device.Update_Characteristic

Initial checks are isOnField, Unity equality board==null, then re-read current board.gameStart. IDs are re-read sequentially after each completed branch, allowing callbacks to replace ID or other fields.

- ID2: EnemySeeking_Zombie then Unity truth test; true invokes current animator.SetTrigger("AttackTrigger").
- ID9: only an ordered positive captured HP proceeds. Capture HP before deltaTime; subtract deltaTime*10, assign, call DC_BonfireWarm, then re-read current HP. Ordered HP<=0 clamps to0 and calls Broken. Still continue scale work after Broken. Read current HP/maxHP, perform **float32 division before conversion to double**, Math.Pow(ratio,.5), cast result to float; current animationGroup.transform.GetChild(1) receives Vector3(scale,scale,0).
- ID10: DC_StoveWarm.
- ID13 and !broken: current target Unity-null or target.IsDisabled() true invokes Broken; otherwise positive captured characteristic timer decrements by deltaTime. Assign before comparison; ordered result<=0 resets to current max timer then calls DC_PortalSummon. Re-read current target.brokenLevel after that callback. If level<3 and current HP is ordered positive, subtract deltaTime*80 from the pre-callback HP; ordered result<=0 clamps to0 and calls Broken. Continue to the current-ID14 test.
- ID14: ordered livingTime>3 calls Broken and returns. ASGC_Main && ordered livingTime>1 invokes main gun then writes ASGC_Main=false even if the callback altered it. Re-read livingTime; when ordered >1 and the captured timer is ordered positive, subtract deltaTime and assign. Ordered result<=0 invokes secondary-gun helper **three times**, then resets the timer from the max field read after those calls.

NaN must not enter positive/<=0 branches. Cover signed zero, infinity, zero maxHP, callbacks changing HP/maxHP/ID/target/animationGroup/timers and exceptions after earlier mutations. Mutants must distinguish float-before-double division, stale/fresh HP timing, omitted third shot, broken early return, merged ID chain and missing false flag write. Called helpers keep their independent status.

## 034E Plant.Update_Attack

Return unless attackInterval is ordered positive and attackable is true. Re-read current board.isFinished; on finished board, IDs1/9 return. Capture old countdown before Time.deltaTime/GetAS and require ordered old>0. IDs1/9 have the producer branch, including a second current-board finished check.

Normal branch: deltaTime first, GetAS second; subtract their product from captured old countdown and assign. Only ordered new<=0 attempts PlantAttackStartTry. Success resets from current attackInterval. **Failure restores the captured old countdown**, not the decremented/current value. Native xmm9 at 0x18035B5D4 proves this lost behavior.

Producer branch: decrement in the same order. For ordered new<=1, set brightness to 3-(new+new). Only ordered new<=0 produces. Current ID1 obtains the named head position, then current board.projectManager.CreateProject(1,0,Vector3). Convert current attackPoint using x86 cvttss2si semantics, including INT_MIN for NaN/out-of-range, and call Project.SunSet. Preserve the existing original MethodSpec0x2B000059 Initial<Plant>, then SpeedStart; current order>=1 invokes sunflower heal.

Both producer IDs then capture audioSource[0] before AudioVolume, set volume, re-read the array/element for Play, capture skillOngoing, reset the countdown from current interval, and only then inspect current Skill.ID==1 and call Function. No added null/array guards.

Cases include false/true attack-start result with helper mutations, NaN before and after decrement, board replacement between initial and producer checks, ID replacement inside GetAS/head lookup, audio element null with AudioVolume side effects, array replacement between calls, skill null after countdown reset, cvtt boundaries and failed project construction. Mutants: retain decremented timer on failure, GetAS before deltaTime, omit brightness, constant sun value, byref Vector3 argument, cached audio array and reset after Skill.Function.

## 03C4 Project.CollectUpdate

Get current transform.position first; capture endPosition afterward. Calculate float32 dx/dy/dz; sum in the original order **dy*dy + dx*dx + dz*dz**, with float32 rounding before promoting to double for Math.Sqrt, then cast to float32. Ordered distance<=1 dispatches the decoded **nine-entry original jump table**, saved as PROJECT-COLLECT-JUMP-TABLE.json:

- type0: current board.sunManager.CollectSun(this), then Destroy(current gameObject).
- types1/2/8: current board.projectManager.CollectProject(this), then Destroy(current gameObject).
- type4: speed=Vector3.zero; no Destroy.
- other values, including negative and >8: return.

For larger/unordered distance, only IDs0/1/2/3/8 proceed to an ordered distance<=25 test. NaN fails that test. Capture a fresh transform before reading current size. Preserve per-component ((Vector3.one.component*capturedSize)*distance)/25 float32 arithmetic and assign localScale. For animationSprites, re-read current list for each public Count/get_Item, GetComponent<SpriteRenderer>, and set Color(1,1,1,distance/25), incrementing the loop index afterward. Do not use private vector constants or List fields.

Exercise every type/ID combination at 0/1/25 and nextafter boundaries, negative/huge coordinates, NaNs/infinities, rounding-sensitive dy/x sums, callback replacement of endPosition/size/list/board/gameObject and partial effects on loop failures. Mutants: damage the type4/no-destroy case, swap type0/1 manager, omit Destroy, normalize distance by another constant, cache animationSprites across callbacks, accept unordered near distance, reassociate the square sum.

## 03F4 Projectile.PC_LightSaber_Explosion

Unity-null origin_Plant returns. Re-read origin, GetDamage(this,21,1), AreaDamage and discard bool. Capture own transform.position, add exactly20f to y, allocate particle18 through current singleton. If Unity-non-null, use its gameObject.transform, set captured Vector3 **by value**, then GetComponent<SortingGroup> and set sortingLayerName="Particles".

Always perform first audio: capture ResourceManager.particleClips[27], get current camera transform position, then AudioVolume*.75, then CreateAudioAtPoint. For second audio, **capture the current particleClips list before Random.Range(0,2)**, then get item random+17 from that captured list; get camera and AudioVolume again, multiply1.25, create audio. Native0x18037B99E–0x18037B9CE demonstrates the list-capture order lost in damaged CIL. Finally DestroyProjectile then re-read origin_Plant and call PC_SSI_Charge.

Test particle Unity-null/destroyed/null-component cases, origin replacement during callbacks, partial damage before visual failures, first/second camera divergence, Range replacing the static list, and destruction replacing origin. Mutants: omit y+20, share camera/volume result, index from replaced list, reverse final calls and pointer-form Vector3 arguments. Dependencies including GetDamage/AreaDamage/DestroyProjectile/charge require their own scope labels.

## 0461 Zombie.InjuryStatusUpdate_Armor2(bool)

If current armor2Type!=0 and current armor2Point is ordered<=0, call Drop_Armor2 with the unchanged argument and return. Otherwise re-read armor2Type. Only type1 (screen), type3 (heavy) and type4 (ladder) enter sprite processing; type0/2/others return. Find the exact named GameObject then Unity-truth test and optional GetComponent<SpriteRenderer>; null renderer is still passed to SetSpriteOverride where native does so, with no new guard.

Re-read armor2BrokenLevel after lookup callbacks. If level<2 and float32(maxArmor2Point/3)>currentPoint ordered, invoke SetSpriteOverride with sprite indices11/31/35 for screen/heavy/ladder. Otherwise if level<1 and float32((max+max)/3)>currentPoint ordered, use10/30/34. Capture max once for max+max. No direct writes to broken level have been invented; preserve helper side effects.

Test all types and levels (including negative), exact thresholds, NaNs, null/destroyed sprite objects, null renderer, global zombieSprites replacement and mutated levels/points during lookup. Mutants: <= threshold, reversed severe/mild precedence, wrong named sprite, bad index, missing Drop argument, added null-renderer early return.

## 0846 FTRuntime.SwfClip.UpdatePropBlock

Unity-false renderer returns. Raw-null _curPropBlock allocates a real MaterialPropertyBlock. Use **public** Renderer.GetPropertyBlock/SetPropertyBlock, MaterialPropertyBlock.SetTexture(int,Texture) and SetFloat(int,float), with four appended MemberRefs copied from the matching signatures. The original CoreModule metadata maps each public method to the exact same native VA as its internal implementation (PUBLIC-API-BINDINGS.json); do not call the internal Unity APIs from game CIL, and do not fake support assemblies.

Capture current block before each ShaderPropsCache.LazyInitialize call; preserve four cache calls on either completed path (tint, main texture, alpha texture, external-alpha flag). Tint is loaded after LazyInitialize. Select current clip.Sprite only after Unity truth of clip; capture sprite. Main texture selection performs the first Sprite.texture getter plus Unity truth, then a **second getter** on success; otherwise whiteTexture. Alpha texture is independently selected from the captured sprite with Unity truth. Capture the block and shader ID before re-testing the selected main texture, possibly getting whiteTexture again. Set main. On alpha Unity-false, SetTexture(alpha,whiteTexture), then SetFloat(external,0); on true use alpha and1. Finally re-read current renderer and block for SetPropertyBlock.

Cover callbacks replacing _clip/_curPropBlock/_meshRenderer/_tint/shader IDs, Sprite.texture returning different objects on two calls, Unity destroyed objects, null/white alpha paths and exceptions after earlier writes. Do not simplify away repeated getters or LazyInitialize calls. Mutants: cache current block too late, use first texture result, invert external-alpha flag, use null instead of white, swap final renderer and cached renderer, byref Color, retain Internal_* API access. Public overload resolution on the actual pinned Player profile is a required independent gate.

## Isolation, parser controls and remaining acceptance

Expected aggregate: 195 bodies =189 direct worklist targets plus six companions; 2,122 unchanged non-targets; 39 existing finally clauses, none new in this batch. Metadata: MemberRef1189 / StandAloneSig1903 / TypeSpec175 / MethodSpec189. Relative Native93 append89 MemberRefs,138 locals,2 TypeSpecs,4 MethodSpecs, for an 830-byte table-row payload increment. New batch only adds four MemberRefs and seven local signatures; zero TypeSpec/MethodSpec/#US additions. Inherited 188 method emissions, metadata pins and finally allowlist remain unchanged.

Do not infer target correctness from a tool's final OK. Bind candidates/logs/tools/profile hashes; reject a wrong parent/overwrite attempt/non-target mutation/InitLocals flip, malformed data and genuine stack underflow with distinct expected diagnostics (a parser crash is INVALID_CONTROL_TOOL_CRASH). Preserve frozen model limitations and separate any extended verifier. Preserve raw failed compile status and distinguish attributed-clean methods from failed enclosing TUs. Existing Native109 stubbed fixture does not cover these seven methods; behavior and integration status must be per-token and candidate-bound.

Finish currently running work at a durable checkpoint, then queue this bundle in a fresh runner. Prefer combined execution evidence over repeated incomplete gate summaries. No production promotion, Unity/Xcode/IPA claim, removal of finally, private-field shortcuts, threshold relaxation, or reactivation of the Codex Gemini4 chat.
