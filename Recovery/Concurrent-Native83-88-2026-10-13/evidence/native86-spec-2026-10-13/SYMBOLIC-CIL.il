// Symbolic recipe only: no candidate or executed assembly.

// 0x060006DF OnPointerClick; append one typed Vector2 local V9.
// Existing V0 Camera, V1 int32, V5 String, V8 TMP_LinkInfo remain typed.
ldarg.0
ldfld TMP_Text TextLink::TMP_Text        // captured text stays typed on stack
ldarg.1
callvirt instance Vector2 PointerEventData::get_position() // NEW public getter ref
stloc V9
call Camera UnityEngine.Camera::get_main() // 0x0A000004
stloc V0
ldloca V9
ldfld float32 Vector2::x               // 0x0A0001D4
ldloca V9
ldfld float32 Vector2::y               // 0x0A0001D5
ldc.r4 bits(0x00000000)
newobj instance void Vector3::.ctor(float32,float32,float32) // 0x0A00009C; VALUE
ldloc V0
call int32 TMP_TextUtilities::FindIntersectingLink(TMP_Text,Vector3,Camera) // 0x0A0002B5
stloc V1
ldarg.0
ldloc V1
stfld int32 TextLink::linkIndex
ldloc V1
ldc.i4.m1
beq TEXT_RETURN
ldarg.0
ldfld TMP_Text TextLink::TMP_Text        // fresh after Find callback
callvirt instance TMP_TextInfo TMP_Text::get_textInfo() // 0x0A0002B8, proven nonvirtual
ldfld TMP_LinkInfo[] TMP_TextInfo::linkInfo // 0x0A0002B9; raw null failures preserved
ldloc V1
ldelem TMP_LinkInfo                    // TypeRef 0x01000065; COPY, not ldelema
stloc V8
ldloca V8
call instance string TMP_LinkInfo::GetLinkID() // 0x0A0002B6; copied local address
stloc V5
ldloc V5
call void Debug::Log(Object)           // 0x0A000010
ldloc V5
ldstr token(0x7001CCC2)                 // "jump0"
call bool String::op_Equality(string,string) // 0x0A00003A
brfalse TEXT_RETURN
ldstr token(0x7001CCCE)                 // fixed original URL
call void Application::OpenURL(string) // 0x0A0002B7
TEXT_RETURN:
ret

// 0x060006FE Yes; typed stack plus existing Int32 V0, no new locals required.
ldarg.0
ldfld int32 Window_Q::Q
switch [YES_RETURN,RETURN_MAIN,RESUME,RESUME,DELETE_SAVE,BACKUP_SAVE]
br YES_RETURN
RETURN_MAIN:
    ldarg.0
    call instance void Window_Q::BoardReturnMain() // 0x06000702
    br YES_RETURN
RESUME:
    ldarg.0
    call instance void Window_Q::BoardResume() // 0x06000701
    br YES_RETURN
DELETE_SAVE:
    ldarg.0
    ldfld int32 Window_Q::info0_int
    ldc.i4.0
    blt YES_RETURN
    call MainUIController MainUIController::get_instance() // 0x06000636
    ldfld SavesUIController MainUIController::savesUIController
    dup
    ldnull
    call bool Object::op_Equality(Object,Object) // exact Unity helper, 0x0A000012
    brtrue POP_RETURN                       // captured UI left on stack
    ldfld List<SavesUIController_PlayerSave> SavesUIController::playerSaveList
    ldarg.0
    ldfld int32 Window_Q::info0_int           // reread after predicate/list capture
    stloc V0                                // captured index, list stays on stack
    dup
    callvirt instance int32 List<SavesUIController_PlayerSave>::get_Count() // 0x0A000443
    ldloc V0
    ble POP_RETURN                          // SIGNED Count <= index
    ldloc V0
    callvirt instance !0 List<SavesUIController_PlayerSave>::get_Item(int32) // 0x0A0002C7
    dup
    call bool Object::op_Implicit(Object)    // 0x0A00000B, captured item remains
    brfalse POP_DESTROY
    dup
    ldfld SavesManager SavesUIController_PlayerSave::savesManager // physical-null gate
    brfalse POP_DESTROY
    dup
    ldfld int32 SavesUIController_PlayerSave::playerSaveID
    stloc V0                                // captured item remains, ID retained
    ldfld SavesManager SavesUIController_PlayerSave::savesManager // reread manager
    ldloc V0
    callvirt instance void SavesManager::DeletePlayerSave_ID(int32) // 0x0600023C
    call MainUIController MainUIController::get_instance() // fresh after deletion
    callvirt instance void MainUIController::LoadData() // 0x0600063A
    br DESTROY_WINDOW
BACKUP_SAVE:
    call MainUIController MainUIController::get_instance()
    ldfld SavesManager MainUIController::savesManager
    dup
    brfalse POP_RETURN
    callvirt instance void SavesManager::BackupPlayerSave() // 0x06000238
    br DESTROY_WINDOW
POP_RETURN:
    pop                                     // UI/list/manager left by the relevant gate
    br YES_RETURN
POP_DESTROY:
    pop                                     // selected item left by predicate/manager gate
DESTROY_WINDOW:
    ldarg.0
    call instance GameObject Component::get_gameObject() // 0x0A000009
    call void Object::Destroy(Object)        // 0x0A000025; no returned-object null guard
YES_RETURN:
ret
