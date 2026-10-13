// Symbolic CIL specification only. Not a candidate, compiled program, or applied patch.
// References below are the independently hash-locked parent82 bindings.

// 0x060001C8 MouseManager.DropDownAll; complete recipe can use no locals.
ldarg.0
ldfld ItemType MouseManager::handItemType // 0x04000233
switch [NONE,CARD,RETURN,RETURN,SKILL,SHOVEL,GLOVE]
br RETURN // includes negative and >6
NONE:
    ldarg.0
    ldfld Board MouseManager::board
    dup
    ldfld bool Board::onDetailPage // raw-null failure, captured board remains on stack
    brfalse POP_RETURN
    ldc.i4.1
    ldc.i4.1
    callvirt instance void Board::Fold_DetailPage(bool,bool) // 0x060002BA
    br RETURN
POP_RETURN:
    pop
    br RETURN
CARD:
    ldarg.0
    call instance void MouseManager::DropCardOnCardBank() // 0x060001CA
    br RETURN
SKILL:
    ldarg.0
    ldfld GameObject MouseManager::plantSkillCrosshairs
    call bool UnityEngine.Object::op_Implicit(Object) // 0x0A00000B
    brfalse DETAIL_THEN_RESET
    ldarg.0
    ldfld Plant MouseManager::skillOnHandOwnerPlant
    ldfld AttackRangeUIController Plant::attackRangeUIController
    callvirt instance void AttackRangeUIController::CollapseView() // 0x060005BE
    ldarg.0
    ldfld GameObject MouseManager::plantSkillCrosshairs // reread after Collapse
    call void UnityEngine.Object::Destroy(Object) // 0x0A000025; null may be passed
    ldarg.0
    ldfld Board MouseManager::board // reread after Destroy
    callvirt instance void Board::DisplayButtons() // 0x060002B8
DETAIL_THEN_RESET:
    ldarg.0
    ldfld Board MouseManager::board // independent current-board read
    dup
    ldfld bool Board::onDetailPage
    brfalse POP_RESET
    ldc.i4.1
    ldc.i4.1
    callvirt instance void Board::Fold_DetailPage(bool,bool)
    br RESET
POP_RESET:
    pop
    br RESET
SHOVEL:
    ldarg.0
    ldfld Prop MouseManager::propOnHand
    ldnull
    call bool UnityEngine.Object::op_Inequality(Object,Object) // 0x0A00000C
    brfalse RESET
    ldarg.0
    ldfld Prop MouseManager::propOnHand // reread after predicate
    callvirt instance void Prop::DropDown() // 0x06000401
    br RESET
GLOVE:
    ldarg.0
    ldfld Prop MouseManager::propOnHand
    call bool UnityEngine.Object::op_Implicit(Object)
    brfalse DROP_GLOVE
    ldarg.0
    ldfld Prop MouseManager::propOnHand // reread after predicate
    callvirt instance void Prop::DropDown()
DROP_GLOVE:
    ldarg.0
    call instance void MouseManager::DropPlantOnGlove() // 0x060001C9
RESET:
    ldarg.0
    call instance void MouseManager::ResetMouseState() // 0x060001E1; not a finally
RETURN:
ret

// 0x060002CE Board.Space_down; existing typed locals, existing local signature 0x11000239.
// V3 Boolean selected pause state; V9 AudioClip; V6 Camera; V7 Transform;
// V10 Vector3 captured camera position; V13 Single captured volume.
ldarg.0
ldfld bool Board::isFailed
brfalse PAUSE_GATES
call BoardManager BoardManager::get_Instance() // 0x06000169, proven pure public getter
callvirt instance void BoardManager::BoardResume() // 0x0600016E
PAUSE_GATES:
ldarg.0
ldfld bool Board::pauseTemp // fresh after BoardResume
brtrue RETURN_SPACE
ldarg.0
ldfld bool Board::menuOpen
brtrue RETURN_SPACE
ldarg.0
ldfld bool Board::gamePause
stloc V3
ldarg.0
ldc.i4.1
stfld bool Board::pauseTemp
ldloc V3
brtrue CONTINUE_GAME
ldsfld List<AudioClip> ResourceManager::boardClips // 0x040002EB, public static
ldc.i4.s 11
callvirt instance !0 List<AudioClip>::get_Item(int32) // 0x0A000084
stloc V9
call Camera UnityEngine.Camera::get_main() // 0x0A000004
stloc V6
ldloc V6
callvirt instance Transform UnityEngine.Component::get_transform() // 0x0A000001
stloc V7
ldloc V7
callvirt instance Vector3 UnityEngine.Transform::get_position() // 0x0A000037
stloc V10
call float32 GlobalStaticVars::AudioVolume() // 0x06000138
stloc V13
ldloc V9
ldloc V10 // BY VALUE, captured before volume callback
ldloc V13
call AudioSource GlobalStaticVars::CreateAudioAtPoint(AudioClip,Vector3,float32) // 0x0600013D
pop
ldarg.0
ldfld GameObject Board::pause // reread after audio helper
ldc.i4.1
callvirt instance void UnityEngine.GameObject::SetActive(bool) // 0x0A000007
ldstr token(0x70009EE2) // exactly "GamePause"
call void UnityEngine.Debug::Log(Object) // 0x0A000010
ldarg.0
ldc.i4.1
stfld bool Board::gamePause
ldc.r4 bits(0x00000000)
call void UnityEngine.Time::set_timeScale(float32) // 0x0A0000F2
br RETURN_SPACE
CONTINUE_GAME:
ldarg.0
call instance void Board::GameContinue() // 0x060002BD
RETURN_SPACE:
ret
