# Native113: eight source bodies, independent execution required

Primary authorship only. Do not inherit semantic acceptance from any earlier candidate. Construct Bulk188 from the fixed Native93 parent and compare inherited bodies to the AGY-reported Bulk180 candidate `e9abad3a618ceeac93a9494507c40669f84efdaa8f917c144dbc411736cb3157`. That predecessor remains blocked on its genuine support profile; its hash is an identity pin, not a promotion.

## 0306 Device.Start_Ani

Return unless ID==3 and content is raw non-null. Allocate/populate the six-string array before reading the content type, including on no-hide branches. Order: Present, PresentOpen, ZombiePresent, ZombiePresentOpen, FacilitiesPresent, FacilitiesPresentOpen. Original jump-table bytes are separately decoded in PENDING-SOURCE-INPUTS.json; bytes after the table start are not additional machine instructions. Types 1/3/4 hide nothing; type2 hides slots 0,1,4,5; type8 hides 0,1,2,3; all other signed values hide 2,3,4,5. Re-read current animationSprites for each HideSprite call; preserve ordered partial effects and exceptions.

## 03FF Prop.Update

Call Camera.main, then Input.mousePosition, then ScreenToWorldPoint on the captured camera. A null camera does not skip Input. Write only x/y of the Vector2 mousePosition before the board checks. Use Unity equality for board-null/destroyed state, then read current board.gameStart. State2 captures both mouse coordinates, adds 50f to each only for propType5, and sets the current cloned image transform position to (x,y,0).

State0 captures old alTime before deltaTime, writes the sum, and updates two fill images in order. First fraction is (cdTime-alTime)/cdTime; second is 1 minus the first image's actual fillAmount after its setter, accessed through existing public get_fillAmount 0A000299, never private m_FillAmount. Capture CDText before Single.ToString("0.0s"). Re-read times after UI callbacks; only ordered alTime>=cdTime sets state1, calls PropBankStateSet and saves current prop.transform.localPosition. Preserve NaN/zero denominator, callback mutation and partial failures. Other states return after common position/board work.

## 054A AlmanacUIController.ClosePlantDetial

Capture initial plantDetail. Mismatched targetLevel restores SetLevelUpPreview(current save.level); otherwise mismatched targetOrder invokes CloseOrderUpPage; otherwise mismatched targetSkillLevel invokes CloseSkillUpPage. Those are separate early returns. On matching state, Unity-truthy current mainUIController calls PlayBGM(1). After this callback, re-read boardManager and sceneType. Board-manager/scene1 branch captures seedChooserScreen, reloads its current detail card from current playerSave, then separately re-reads current card.plantID and current prepareUIController for ReloadPlantSave. Hide this object's gameObject and show the captured chooser.

The other path shows plantHome, hides the current detail gameObject, shows the current list canvas gameObject, and calls LoadPlants only if the current public list Count<=0. It must not silently omit the original out-of-method tail jump to LoadPlants(0547). Test UI callback replacement of fields between steps.

## 06AC SavesUIController_PlayerSave.Start0

playerSaveID==-1 is a distinct empty-slot path: SetText("空",true), font42, button interactable true, white color, delete GameObject inactive. Otherwise raw-null savesManager causes immediate return. Capture list[index] as Save; do not add bounds guards. Null Save sets "NULL", button false, then white color. Non-null Save sets playerName first, then compares captured Save by raw reference to the current savesManager.playerSave. Equal means green color then button false; unequal means white then button true. All API call order and effects before later exceptions remain. Text callbacks may replace savesManager. Do not use Unity object equality for Save.

## 02F4 Card_Choose.Choose_Being

Return when already chosen. Capture current chooser's serial, then re-read chooser.prepareUIController.GetCardBankLenth. Only serial<length proceeds. Write isChosen before any subsequent component lookup. Capture chosen.GetComponent<Image>, then perform three get_color calls on that same Image: r from first result, g from second, b from third. Write Color(r,g,b,1). Re-read chosen for transform.GetChild(0).gameObject activation, then activate current serial text GameObject. Write serial+1 to the field before capturing current text and formatting the actual field address with Int32.ToString. Finally invoke current chooser.plantDetail.Awake(this). Include mutations/late exceptions after isChosen was already set.

## 038D Plant.PC_StarfruitSwordImmortal

Initial FindStatsIncreased("SSI_Characteristic_Atk") is always called. When skillOngoing is false, a raw non-null stats value<2 grows by deltaTime*captured updateRate*.04f added to captured old value; clamp only on ordered newValue>2. For order>=1 and maxHealth>health, capture maxHealth and updateRate before deltaTime, then calculate ((maxHealth*.02f)*updateRate)*deltaTime + the health read after the callback. Assign and clamp only against current maxHealth, read again.

If captured formation time>0, subtract deltaTime*updateRate (rate read after callback), then capture countdown and obtain a second deltaTime for its increment. Clamp formation time only on ordered negative result; invoke current PC_SSI_SummonLightSaber when newly computed countdown>.3f. Do not collapse deltaTime calls or reassociate products. Existing summon helper is a separate dependency with its own unproven status.

Tail: if current animator equals Unity-null, return. Re-find the stats entry, return for raw null; capture value and then current animator. Ordered thresholds >=2 / >=1 choose integer2 /1 /0 for SetInteger("CharactristicInt"); NaN takes0. Preserve float32 constants, ordered comparisons, callback captures and all prior mutations.

## 04A7 Zombie.ZC_SnowbeastHitWall

Raw snowbeast_isRest returns immediately. FindBuff("Snowbeast.speed") and isinst StatsIncreased; non-null result gets value=-1. Assign full rDirection=Vector3.zero. Capture ResourceManager.mouseClips[12] before obtaining camera transform position and AudioVolume; call CreateAudioAtPoint and discard AudioSource result. Subtract exactly float32(9999999) from current restTime and assign before comparison. Ordered result<=0 sets restTime0, isRest true, calls current animator.SetBool("rest",true), then TranToStant(.02f). Finally Unity-truthy hpUIController causes a fresh field read and RecordingHP call. Test audio/animator callback replacement, NaN and exceptions after earlier effects.

## 04A8 Zombie.ZC_SnowbeastHitZombieTest

Return for snowbeast_isRest or isStant. Capture current board.zombieManager.zombieList and acquire its typed public enumerator before try. Use the exact original closed Enumerator<Zombie> references: TypeSpec46 `15115101128210`, get_Current0A0000DA, MoveNext0A0000DC, Dispose0A00015E. TypeSpec4 is Enumerator<object> and is invalid here; never reuse its MoveNext0A000024.

For each candidate: skip on Unity equality with this, false candidate.CanAttacked, or ordered candidate.snowbeast_impactCD>0. Require strict ordered overlap in both dimensions: this.fW+candidate.fW > 2*Abs(candidate.fX-this.fX), then analogous fD/fY. NaN overlap fails. Invoke this.ZC_SnowbeastHitZombie(candidate), keeping its mutations and later enumerator invalidation. Finally disposes on normal completion or exceptions; do not remove it because native Dispose is currently a no-op/shared stub. Include null/destroyed/self, callback movement, version mutation, boundary/touching, NaN and exception mutants.

## Gates and boundaries

Bulk188: 188 method bodies, 2,129 non-targets, 39 finally regions, MemberRef1185 / StandAloneSig1896 / TypeSpec175 / MethodSpec189. New metadata consists solely of eight local signatures; no new MemberRefs, strings, TypeSpecs, MethodSpecs or #US. #~ row payload increment from Native93 is 792 bytes. Preserve all old 180 bodies byte-for-byte and all earlier metadata pins/negative controls.

AGY owns construction, isolation, parser/negative-control validation, static/API gates, real original-native versus candidate-CIL behavior tests, regressions and Actions compilation. Root author runs no project tests. Current missing AnimatorStateInfo getter remains inherited BLOCKED_DEPENDENCY_PROFILE; do not fake support APIs or treat an observed first exception as the only possible downstream issue. No product or IPA qualification from static green results.
