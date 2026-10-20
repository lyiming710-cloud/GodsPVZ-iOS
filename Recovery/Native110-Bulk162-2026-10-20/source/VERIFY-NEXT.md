# Native110: ten native-backed method-body specifications

Source authored only. No candidate was constructed or executed by the primary author. The unmodified Native93 is the fixed aggregate construction parent. Original PC bytes, field offsets, constants and CIL identities are saved next to this specification. Six additional disassemblies in the evidence directory are reconnaissance only and are not modified or counted.

## Zombie bombs: 049A and 049D

Both original bodies have the same effects: GetDamage(-1,null,0).AreaDamage() then DestroyZombie; particleClips[1] and Unity truthiness; on truth, capture Camera.main.transform.position before AudioVolume and Random.Range(0.9,1.1), then CreateAudioAtPoint with the captured value and discard its AudioSource. Always continue to ParticlesManager.instance.CreatNewParticle(2). Only a Unity-truthy particle receives position (current fX, current fY+current fZ,0), localScale(1.2,1.2,1.2), then current board.Shake(0.15,8). A false particle skips the shake. All fields are re-read after earlier callbacks, including DestroyZombie. Private singleton backing field is accessed through public getter 01E5; its current body is a branch followed by ldsfld 04000278 and ret, so the instance slot is identical.

Controls must distinguish damage/area/destroy order; false audio vs false particle; missing camera/transform/manager/board; mutation in AudioVolume/Random/transform callbacks; position and localScale value bytes; no added silent-null returns.

## BoardPreview device: 05D2

Capture PreDevice.data.ID, capture devicePrefabList. Native tests Count < ID, then calls get_Item(ID); ID==Count is intentionally not silently rejected. Unity inequality determines a possible prefab, then a separate Unity truthiness check controls instantiation. Clone, clear board, re-read data and Initialize. BoardConfig.GetPlantingPosition(gridX,gridY), subtract exactly 52 from x, preserve y/z, pass Vector3 by value into SetTransformPosition. Capture clone.transform before this.transform; invoke public Transform.SetParent(Transform), whose PC pointer is the same 18132E8B0 as the inlined internal implementation. Re-read the two PreDevice grid coordinates, call SetLayer, SetUIs, SetAnimationState_Placing in that order; set isOnField=false; append to List<Device> using its public closed Add(!0).

Controls: negative/out-of-range ID and ID==Count exceptions, null list/data/config, prefab replacement during Unity callbacks, callback mutation of data/grid/parent/list, and exactly one list insertion only after all earlier work succeeds. No direct List private-field writes.

## BoardPreview zombie: 05D4

Read zombiePrefabs[enemy.id], test Unity equality with null. The missing case logs the exact original prefix + id.ToString() + suffix and returns. The non-missing case re-reads both static list and key before cloning. Read Enemy.position (040001FA / native+1C); subtract 52 from x only, and assign the clone's transform.position by value. Capture clone.gameObject.transform and use SetParent(current enemyPreview,false). Set gridY, waitingTime, null prePath, wave in original order, then List<Zombie>.Add. Read shadow.position twice separately to set fX then fY; SetLayer; set isOnBoard=false; finally unlock the current playerSave.almanac_ZombieLock[current enemy.id]. Every exceptional path must retain its prior effects.

Controls: Unity equality replaces prefab list/key, missing-prefab exact text, worldPositionStays=false, clone/parent mutations, shadow getter replacement between reads, list insertion before later shadow/unlock failures, invalid unlock index, precise field owner/Vector3 data.

## Zombie_Select.Initialize: 0711

Use Unity equality(arg1,null). On non-empty input clone it and store this.zombie; set isOnBoard=false. On Unity-false sortingGroup capture this.zombie before ComponentHolderProtocol.AddComponent<SortingGroup>(captured), and assign the result on that captured zombie. Re-read the current zombie/sortingGroup for UpperUI and sortingOrder10000. Parent clone transform under this.transform, then localPosition=Vector3.zero. Set select=arg2, use original argument's ID for Load_zombieInfo. Null info skips the lock controls but still calls UpdateSelectButtonImage. Otherwise isElite || isBoss uses short-circuit reads; set button.interactable=!locked and image.gameObject.active=locked. Empty input sets select=false, calls UpdateSelectButtonImage once before changing the UI, then calls it AGAIN at the common tail. Do not collapse these two calls or add an assignment to this.zombie in the empty branch.

Controls: callback replaces this.zombie, destroyed/non-null inputs, both sorting branches, null info, elite short-circuit, stale old zombie in empty branch, exact early/tail callback count and UI exception timing. The private helper is called only from its declaring type.

## TMPro overlay/frame/example Awake: 07AC, 07B2, 07AF

Overlay and frame Awake return when Behaviour.enabled is false. Overlay captures camera, creates exact Frame Counter object, stores transform before parenting with public set_parent, applies Quaternion.identity by value, adds TextMeshPro, loads the exact original font/material paths with captured receivers, fontSize30/isOverlaytrue, gets TextContainer, calls existing 07AD, then sets the exact instruction string. Frame captures camera, sets Application.targetFrameRate=9999, creates and adds TextMeshPro, sets exact font/material, stores transform, uses public SetParent(single argument), applies Quaternion.identity by value, enableWordWrapping=false/fontSize24, calls restored 07B5, then re-reads AnchorPosition into last_AnchorPosition.

Example uses ObjectType==0 for TextMeshPro and nonzero for TextMeshProUGUI. GetComponent result is tested as a raw managed reference (not Unity truthiness); only a raw null gets the matching AddComponent. Set the common TMP_Text field, original Anton font/material, fontSize120 and exact text. Capture GetPreferredValues(+Infinity,+Infinity) before re-reading m_text.rectTransform; assign the captured Vector2 by value.

Controls: disabled paths have no camera/resource/allocation calls, each ObjectType branch uses its exact generic argument, raw-null vs Unity-destroyed distinctions, callback replacement during resource lookup and setters, identity Quaternion bytes, positive-infinity bits, captured preferred size, exact private field ownership and constructor/API resolution.

## TMP_FrameRateCounter.Set_FrameCounter_Position: 07B5

Always set margin Vector4(1,1,1,1), including invalid anchor values. Anchor0/1/2/3 select alignments257/1025/260/1028 and pivots(0,1)/(0,0)/(1,1)/(1,0). After setting pivot, capture current camera then current frame transform; call camera.ViewportToWorldPoint(Vector3(pivot.x,pivot.y,100)), then set position on the captured transform. Invalid enum values return after margin without other calls. Vector4 constructor shares the original public four-float signature template; Vector2 x/y are public fields initialized in a typed local. No private oneVector or zeroVector fields are used.

Controls: four valid anchors and negative/out-of-range values; margin must happen first even on the default branch; mutation of m_TextMeshPro between setters, camera/frame mutation in pivot or viewport callbacks, captured receiver identity and bit-exact value arguments. All compiler and runtime evidence remains pending independent Actions execution.

## Shared validation boundary

Authenticate source and native SHA pins, build twice from Native93, reopen and independently verify all 2,155 non-target methods and all 152 inherited target bodies/header/local-signature/EH bytes. Validate the five new MemberRefs against genuine iOS Player metadata, branch/stack/typed local flow and negative controls. All ten new bodies are EH-free; existing thirty aggregate finally clauses must remain intact. Compilation alone cannot establish these behavioral specifications. Keep failures, unsupported exception unwinding, stubbed dependency checks and unproven full interactions separate; no formal promotion or IPA claim is made.
