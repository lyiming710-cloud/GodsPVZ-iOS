# ZombieManager.BGMUnPasue native authority

Authority: original PC GodsPVZ 1.0.2 `GameAssembly.dll`.

- GameAssembly SHA256: `9ebd7ca996a5b03fb4a766f7a2502b660d581ddbbb36af4d7bf2f06a211da39d`
- MethodDef: `ZombieManager::BGMUnPasue()`
- token: `0x06000278`
- RID: `632`
- Assembly-CSharp method-pointer table entry: `0x181B84118`
- native pointer / runtime-function begin: `0x180341800`
- runtime-function end: `0x1803418FC`
- native size: `252` bytes
- native slice SHA256: `70e89e1031d7c703b2e919b2669723929e0741f48233720eddf91120df9fd832`

Healthy adjacent authority used as a structural cross-check:

- `ZombieManager::BGMPasue()` token `0x06000277`, RID `631`
- pointer-table entry `0x181B84110`
- native pointer `0x180341700`
- runtime-function `0x180341700–0x1803417FC`
- native size `252` bytes
- native slice SHA256 `e240f87fe9eedf3bdb620bddb4f0a94568aeab511b5e7dd9c1f2fba77e1bb42f`

The two native functions are instruction-for-instruction structural twins: both read `ZombieManager::zombieList` at manager offset `+0x38`, construct the typed list enumerator, loop with `MoveNext`, null-check the current Zombie, invoke one per-Zombie BGM method, then dispose the enumerator on loop completion / unwind. The only gameplay-semantic difference in the loop body is the final per-Zombie method target:

- pause path calls `Zombie::BGMPasue()` (RID 1049, PC pointer `0x18035DD50`)
- unpause path calls `Zombie::BGMUnPasue()` (RID 1050, PC pointer `0x18035DDD0`)

Key `BGMUnPasue` native instructions:

```text
0x180341849  mov  rdx,[rbx+0x38]       ; zombieList
0x18034184d  test rdx,rdx
0x180341856  ...
0x180341862  call 0x1808197F0          ; typed List<Zombie>.GetEnumerator lowering
...
0x180341890  ...
0x18034189c  call 0x180669280          ; typed enumerator MoveNext/current lowering
0x1803418a1  test al,al
0x1803418a3  je   0x1803418b8
0x1803418a5  mov  rcx,[rsp+0x50]       ; current Zombie
0x1803418aa  test rcx,rcx
0x1803418ad  je   0x1803418ea
0x1803418af  xor  edx,edx
0x1803418b1  call 0x18035DDD0          ; Zombie::BGMUnPasue()
0x1803418b6  jmp  0x180341890
0x1803418b8  ...
0x1803418c2  call 0x180302170          ; enumerator disposal/finalization lowering
```

Therefore the recovery is constrained to the canonical managed equivalent already present and healthy in adjacent `ZombieManager::BGMPasue()`: clone its strong `List<Zombie>.Enumerator` foreach/finally control-flow shape and replace only the per-item call `Zombie::BGMPasue()` with `Zombie::BGMUnPasue()`. No field visibility, metadata, null-guard, collection-layout, or exception-swallowing change is authorized.
