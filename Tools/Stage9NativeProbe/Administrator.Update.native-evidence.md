# Administrator::Update PC native authority

This evidence is derived from the original PC `GodsPVZ_1.0.2.zip` and the current Stage9.1 managed baseline. It is intended to constrain a future repair after `Administrator::Start` is accepted; it does not promote any candidate by itself.

## Source authority

- Original PC package: `GodsPVZ_1.0.2.zip`, Drive ID `1_5kVGTFmsvwcdRLQ2ah_rtyC6DqvLnf0`
- ZIP SHA256: `2f08b4e2243e2bd296f3db694b4a2c9656252d76202c80167bdf3340b9f65b48`
- `GameAssembly.dll` SHA256: `9ebd7ca996a5b03fb4a766f7a2502b660d581ddbbb36af4d7bf2f06a211da39d`
- `global-metadata.dat` SHA256: `ad992341add498bd1018ac980f42171e0737b0793bf945c8714ad70a445b36b9`
- method-pointer table base: `0x181B82D60`

## Stable managed identity

- `Administrator` TypeDef `0x02000002`
- `Administrator::Update()` MethodDef `0x06000006`, RID6
- damaged managed RVA: `0x235C`
- managed code size: 492 bytes
- managed locals: 22
- `Administrator::mode` FieldDef `0x0400000A`, native instance offset `+0x60`
- `GlobalStaticVars::IsOnBoard()` MethodDef `0x06000146`, RID326

The current managed body already preserves the high-level source control flow, but contains one stack-type corruption at `IL_00D4`: `ldc.i8 4294967295` immediately followed by `stfld int32 Administrator::mode`.

## Exact PC native body

For RID6:

- method-pointer entry: `0x181B82D88`
- direct pointer: `0x1802FE980`
- `.pdata` RuntimeFunction: `0x1802FE980–0x1802FEAEE`
- exact body size: 366 bytes (`0x16E`)
- exact native SHA256: `13f85136116347b993ef4733341830e2a3a9f63dca9c8203cbb960114193bd76`

The first source-level call is exactly `GlobalStaticVars::IsOnBoard()`:

```text
0x1802FE9BA  xor ecx, ecx
0x1802FE9BC  call 0x18031BB70   ; RID326 GlobalStaticVars::IsOnBoard()
0x1802FE9C1  test al, al
0x1802FE9C3  jne  0x1802FEA57  ; on-board path
```

The method then has two environment gates which converge onto the same keyboard/mode-toggle logic.

### Off-board path

The TypeInfo at `0x181BA2018` is the `GlobalStaticVars` type used by its own `.cctor`. `Update` obtains the first static field (`gLawnApp`) and tests the byte at `LawnApp + 0x10`, matching managed `GlobalStaticVars.gLawnApp.administratorMode`:

```text
0x1802FE9C9  mov  rax, [0x181BA2018]
...
0x1802FE9E8  mov  rax, [rax+0xB8]   ; static fields
0x1802FE9EF  mov  rcx, [rax]        ; gLawnApp
0x1802FE9FB  cmp  byte ptr [rcx+0x10], 0
0x1802FE9FF  je   0x1802FEA51       ; return if administratorMode == false
```

### On-board path

`GlobalStaticVars::IsOnBoard()` independently uses TypeInfo `0x181BD1A40` to access `BoardManager.<Instance>k__BackingField` and the `board` field at instance offset `+0x48`. `Update` uses the same TypeInfo and tests the board byte at `+0x42`, matching managed `Board.gamePause`:

```text
0x1802FEA73  mov  rax, [0x181BD1A40]
0x1802FEA7A  mov  rcx, [rax+0xB8]   ; BoardManager static fields
0x1802FEA81  mov  rax, [rcx]        ; BoardManager.Instance
0x1802FEA89  mov  rax, [rax+0x48]   ; board
0x1802FEA92  cmp  byte ptr [rax+0x42], 0
0x1802FEA96  je   0x1802FEA51       ; return if gamePause == false
```

## Shared keyboard gate

Both paths then require `A` to go down and either shift key to be held. The native constants exactly match Unity `KeyCode.A = 97`, `KeyCode.LeftShift = 304`, and `KeyCode.RightShift = 303` used by the recovered managed body:

```text
mov ecx, 0x61   ; A
call ...GetKeyDown...
test al, al
je   return

mov ecx, 0x130  ; LeftShift
call ...GetKey...
test al, al
jne  toggle

mov ecx, 0x12F  ; RightShift
call ...GetKey...
test al, al
je   return
```

## Exact mode-toggle semantics

The native body proves the intended field writes are 32-bit values:

```text
0x1802FEA36  cmp dword ptr [rbx+0x60], -1
0x1802FEA3A  je  0x1802FEAD7
0x1802FEA40  mov dword ptr [rbx+0x60], 0xFFFFFFFF  ; mode = -1
0x1802FEA4C  jmp 0x1802FDD20                      ; tail-call Administrator::Start()
...
0x1802FEACD  cmp dword ptr [rbx+0x60], -1
0x1802FEAD1  jne 0x1802FEA40
0x1802FEAD7  mov dword ptr [rbx+0x60], 0           ; mode = 0
0x1802FEAE3  jmp 0x1802FDD20                       ; tail-call Administrator::Start()
```

Therefore the recovered managed instruction

```text
IL_00D4: ldc.i8 4294967295
IL_00DD: stfld int32 Administrator::mode
```

is a decompiler artifact, not source semantics. The native value is signed 32-bit `-1`.

## Managed/native control-flow equivalence

Outside the single `ldc.i8` stack-type corruption, the current managed `Administrator::Update()` matches the native body at source level:

1. `GlobalStaticVars.IsOnBoard()` selects off-board vs on-board gate.
2. Off-board requires `GlobalStaticVars.gLawnApp.administratorMode`.
3. On-board requires `BoardManager.Instance.board.gamePause`.
4. Both require `Input.GetKeyDown(A)` and `(Input.GetKey(LeftShift) || Input.GetKey(RightShift))`.
5. `mode == -1` becomes `0`; otherwise it becomes `-1`.
6. Both toggle branches invoke `Administrator.Start()` and return.

The exact-R3 runtime with the metadata-preserving Start candidate reported `NEXT_ADMINISTRATOR_UPDATE_INVALID_IL_COUNT 265`, consistent with this single invalid method being invoked repeatedly per frame rather than 265 distinct damaged instructions.

## Minimal future repair

A branch-offset-preserving repair can replace the 9-byte preimage

```text
21 FF FF FF FF 00 00 00 00   ; ldc.i8 4294967295
```

with

```text
15 00 00 00 00 00 00 00 00   ; ldc.i4.m1 + eight nop
```

at managed IL offset `IL_00D4` (code RVA `0x243C` in the current layout). This keeps `IL_00DD` and every existing branch target unchanged, keeps the local signature and metadata unchanged, and restores the exact native `mode = -1` semantics.

Do not replace the surrounding control flow, add guards, swallow exceptions, or alter the on-board/off-board conditions.