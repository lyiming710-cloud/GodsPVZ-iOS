// Symbolic specification only. Tokens/TypeSpecs/locals must be resolved in the current parent.
// This file is not an assembly, candidate, patch or executed test.

// 0x060001AC: instance ZombieSelect ZombieSelectRandomGet(List<ZombieSelect>,float32)
// locals: listArg : List<ZombieSelect>; remaining : float32;
//         enum : List<ZombieSelect>/Enumerator; item : ZombieSelect; result : ZombieSelect

ldarg.1
stloc listArg
ldc.r4 0.0                  // positive zero
ldarg.2
call float32 UnityEngine.Random::Range(float32,float32)
stloc remaining
ldloc listArg
callvirt instance valuetype List<ZombieSelect>/Enumerator List<ZombieSelect>::GetEnumerator()
stloc enum
ldnull
stloc result
.try {
LOOP:
    ldloca enum
    call instance bool List<ZombieSelect>/Enumerator::MoveNext()
    brfalse EXHAUSTED
    ldloca enum
    call instance !0 List<ZombieSelect>/Enumerator::get_Current()
    stloc item
    ldloc remaining
    ldloc item
    ldfld float32 ZombieSelect::weight // raw null throws here, inside protected body
    bgt SUBTRACT                      // ordered; unordered falls through to selection
    ldloc item
    stloc result
    leave RETURN
SUBTRACT:
    ldloc remaining
    ldloc item
    ldfld float32 ZombieSelect::weight // second native weight read
    sub                               // remaining - weight, per-iteration float32 coercion/store
    // optional explicit conv.r4 when needed by selected CIL/runtime rounding strategy
    stloc remaining
    br LOOP
EXHAUSTED:
    leave RETURN
}
finally {
    ldloca enum
    call instance void List<ZombieSelect>/Enumerator::Dispose()
    endfinally
}
RETURN:
ldloc result
ret

// 0x060003C5: Project.GetEndPosition()
// locals: transform : Transform; position : Vector3; sampled : float32;
//         arg : Vector3; board : Board; banks : Banks; manager : ProjectManager
// The following is CIL semantic-block notation for field-address loads/stores;
// every helper Vector3 parameter is BY VALUE, never &Object or &Vector3.

if (ldarg.0; ldfld int32 Project::movementTracks) != 0 -> CHECK_TRACK4
    transform = callvirt instance Transform UnityEngine.Component::get_transform(this)
    position = callvirt instance Vector3 UnityEngine.Transform::get_position(transform)
    (ldarg.0; ldflda Vector3 Project::endPosition).x = position.x
    sampled = call float32 UnityEngine.Random::Range(-380.0f,120.0f)
    // Preserve the current x/z after the callback; no pre-Random cached-vector overwrite.
    (ldarg.0; ldflda Vector3 Project::endPosition).y = sampled
    arg = ldarg.0; ldfld Vector3 Project::endPosition
    this.endPosition = call instance Vector3 Project::PositionRevision_Camera(this,arg)
CHECK_TRACK4:
if (ldarg.0; ldfld int32 Project::movementTracks) != 4 -> RETURN_PROJECT
switch (ldarg.0; ldfld int32 Project::ID) {
    [SUN,COIN,COIN,DIAMOND,CAMERA_115,CAMERA_115,RETURN_PROJECT,RETURN_PROJECT,COIN]
}
br RETURN_PROJECT                    // negative or >8 default
SUN:
    board = current this.board
    banks = board.banks              // raw null board fails; no skip
    this.endPosition = banks.GetSunBankPosition()
    this.endPosition.y = this.endPosition.y + 12.0f
    br RETURN_PROJECT
COIN:
    board = current this.board
    manager = board.projectManager   // raw null board fails; no skip
    this.endPosition = manager.GetCoinBankPosition()
    br SUBTRACT_OFFSETS
DIAMOND:
    board = current this.board
    manager = board.projectManager
    this.endPosition = manager.GetDiamondBankPosition()
SUBTRACT_OFFSETS:
    this.endPosition.y = this.endPosition.y - 5.0f // field component left, constant right
    this.endPosition.x = this.endPosition.x - 85.0f
    br RETURN_PROJECT
CAMERA_115:
    this.endPosition = Vector3(+0.0f,115.0f,+0.0f)
    arg = current this.endPosition
    this.endPosition = call instance Vector3 Project::PositionRevision_Camera(this,arg)
RETURN_PROJECT:
ret
