# DialogueManager_OnBoard.Update PC native authority

Read-only prefetch. This is not a promoted mutation target until the preceding runtime gate selects it as the first causal gameplay blocker.

Authority: original GodsPVZ 1.0.2 PC x86-64 IL2CPP `GameAssembly.dll`.

- GameAssembly SHA256: `9ebd7ca996a5b03fb4a766f7a2502b660d581ddbbb36af4d7bf2f06a211da39d`
- Assembly-CSharp method pointer table VA: `0x181B82D60`
- Managed MethodDef: `DialogueManager_OnBoard.Update()`
- Token: `0x06000194`
- RID: `404`
- Pointer-table entry VA: `0x181B839F8`
- Native method VA: `0x180314920`
- `.pdata` exact range: `0x180314920–0x1803149FD` (221 bytes)
- Native slice SHA256: `e40d885774b7c6ddeb7651e6091d748daa75bd3551e3229532c21ea45c97141b`

## Recovered semantics

The native code performs the same control flow visible in the managed reconstruction:

1. If `board == null`, return.
2. If `isRunning` is true, return.
3. Read `dialogueList` and compare `dialogueIndex` with the list's size.
4. If `dialogueIndex >= size`, return.
5. Fetch `dialogueList[dialogueIndex]` and call `TestLogTrigger`.
6. If it returns true, fetch the same item and call `LoadDialogue`.

Key native sequence:

```text
0x180314968 mov  rdi,[rbx+0x20]     ; board
...
0x180314982 call 0x18131f760       ; Unity object equality / null test
0x180314987 test al,al
0x180314989 jne  0x1803149ec       ; return if board == null
0x18031498b cmp  al,[rbx+0x44]     ; isRunning; al is false on non-null path
0x18031498e jne  0x1803149ec       ; return if isRunning != false
0x180314990 mov  rax,[rbx+0x38]    ; dialogueList
0x180314994 test rax,rax
0x180314997 je   0x1803149f7       ; null helper
0x180314999 mov  eax,[rax+0x18]    ; List<T> internal size
0x18031499c cmp  [rbx+0x40],eax    ; dialogueIndex vs size
0x18031499f jge  0x1803149ec
...
0x1803149bd call 0x180314790       ; TestLogTrigger
...
0x1803149e7 call 0x180314150       ; LoadDialogue
```

The damaged managed body directly reads private `List<BoardDialogue>._size`, causing Mono `FieldAccessException`. If this method is promoted by runtime truth, the minimal CLR adaptation is the same semantic size query through public `List<BoardDialogue>.Count`; no gameplay field metadata needs to change.
