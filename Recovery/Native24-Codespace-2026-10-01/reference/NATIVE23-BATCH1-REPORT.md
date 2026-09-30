# Native23 Batch 1 —— 证据与结果

> 范围：从剩余编译错误中选取一组代表性、运行时重要的方法，核对原 PC native，  
> 形成完整 MethodBody 规格，完成修复 + 独立行为测试 + 反向测试 + 非目标隔离 +  
> 真实全量 C++ 复测。**编译成功 / IPA 成功 / 行为正确三项分开报告。**

生成时间：2026-09-30  
Codespace 分支：`repair/codex-native19-ipa` @ `09dd564`（锁定 handoff 提交，未继承后续分支状态）

---

## 0. 先修正此前三处错误结论（重要）

在开工前先发现并修正了会污染全部归因的三处缺陷：

1. **C++ 归因用错了源码树。** `.validation/native22-full2` 的 264 份 clang 日志由  
   **canary 树**产生（`native22_scan2.sh` 的 `SRC` 指向  
   `native21/replay/canary/.../il2cppOutput/cpp`），但 `native22_cpp_errors.py` 的  
   `CPP` 指向的是 **xcode-native18 树**。两棵树有 18 个 `.cpp` 不同  
   （例如 `GodsPVZRuntime1__8.cpp`：canary 16505 行 / xcode 16912 行）。  
   → **xcode 树是陈旧产物，不得用于归因。**
2. **此前「D 桶 855 个方法」虚高。** 用 canary 树重做归因后，真实数字是  
   **2267 个 token 中 608 个**的方法体内含 clang 错误。此前 pilot 名单里的  
   `0x060003F7 Projectile::SetDamage`、`0x060003FD Projectile::.ctor`、  
   `0x06000451 Zombie::GetRandenAnimationSpeedMagnification` 实际是 **0 错误**。
3. **PC 原生取证工具两处缺陷**（已修，见 `native23_native.py`）：
   - **函数边界**：`Plant::Update` 在 `.pdata` 里是**连续 8 段**（4D0..4EB..61F..  
     64D..844..89B..8B9..8CA..8E3，共 1043 字节），旧工具只取首段 27 字节，  
     导致「0 调用点」的假象。
   - **token 跨 image 碰撞**：`0x0600034C` 同时是 CoreModule 的  
     `ColorGamutUtility::GetTransferFunction` 和 Assembly-CSharp 的 `Plant::Update`。  
     必须优先取 `Assembly-CSharp.dll`。

---

## 1. 三层证据链（可复现性已证明）

| 环节          | 证据                                                                                                              |
| ----------- | --------------------------------------------------------------------------------------------------------------- |
| 基线 DLL      | `ManagedStripped/GodsPVZRuntime1.dll` sha256 `0d6bc587c98ff05b733d9d5bced8b96726d2dd5d2ba82cb949c6ca90b8f86e8f` |
| IL dump 同源性 | 用 `PatcherNative19 export-all` 从基线**重新导出**，与既有 `all-methods.json` 的 2297 个方法**逐字段完全相同**                         |
| C++ 可复现性    | 用完全相同的 il2cpp 参数重跑转换，**264/264 个 `.cpp` 与 canary 树逐字节相同**                                                       |
| 诊断归属        | canary 树 → 264 份 clang 日志，6006 条 error；每份文件 sha256 与 `input/cpp-manifest.json` 比对，**drift 为 none**              |

即：改 IL → 重新转译 → 全量 clang 复测，是一条闭合且可复现的链路。

---

## 2. 批次选择（从 608 个真实错误方法中）

核心类（Plant/Zombie/Board/Device/Projectile/MouseManager/Grid/…）共 **186** 个方法含错误，  
其中 **58 个**是纯类型编码错误（A/B 族），**128 个**是结构性错误（多数为单条 `call-signature`）。

本批选 3 个方法 / 6 处修改，覆盖三个族群且都是运行时核心：

| token        | 方法                   | 族群                | 错误数 | 为什么重要           |
| ------------ | -------------------- | ----------------- | --- | --------------- |
| `0x06000348` | `Plant::Awake`       | C 调用归属            | 2   | 植物初始化，每株植物创建时必走 |
| `0x0600034C` | `Plant::Update`      | C 调用归属            | 1   | 每帧对每株植物执行       |
| `0x06000339` | `Device::TryPlacing` | A 引用/整型零比较 + 异常语义 | 2   | 放置植物的判定入口       |

---

## 3. 修复规格与证据

### 3.1 Plant::Awake（0x06000348）

完整 MethodBody 规格（恢复后 IL，14 条指令，0 局部变量，0 异常处理）：

```
IL_0000 ldarg.0
IL_0001 ldarg.0
IL_0002 ldfld    UnityEngine.GameObject Plant::animationGroup
IL_0007 callvirt !!0 UnityEngine.Component::GetComponent<UnityEngine.Animator>()   ← 错
IL_000C stfld    UnityEngine.Animator Plant::animator
IL_0011 ldarg.0
IL_0012 ldfld    List<GameObject> Plant::animationSprites
IL_0017 callvirt Void List<GameObject>::Clear()
IL_001C ldarg.0
IL_001D ldarg.0
IL_001E ldfld    UnityEngine.GameObject Plant::ani_aniPart
IL_0023 callvirt UnityEngine.Transform UnityEngine.Component::get_transform()      ← 错
IL_0028 call     Void Plant::LoopAddAnimation(UnityEngine.Transform)
IL_002D ret
```

PC 原生 `0x18034DEA0..0x18034DF58`（184 字节）逐条对照：

| 原生                                                           | 含义                              |
| ------------------------------------------------------------ | ------------------------------- |
| `mov 0x150(%rbx),%rcx; test; je -> raise NRE`                | 读 `animationGroup`，空则抛 NRE      |
| `call 0x18046DC80`                                           | **GameObject::GetComponent<T>** |
| `mov %rax,0x168(%rbx)` + `call 0x18024F360`                  | 写 `animator` 并写屏障               |
| `mov 0x148(%rbx),%rcx; test; je -> NRE` → `Array::Clear`     | 读 `animationSprites` 并 Clear    |
| `mov 0x158(%rbx),%rcx; test; je -> NRE` → `call 0x18131C070` | **GameObject::get_transform**   |
| `jmp 0x180351AC0`                                            | 尾调用 `Plant::LoopAddAnimation`   |

指令顺序、三处空检查、字段次序、尾调用**全部一致**，唯二差异就是两处调用被错误归属到 `Component`。

修复：

- **R1** IL_0007 `Component::GetComponent<Animator>` → `GameObject::GetComponent<Animator>`
- **R2** IL_0023 `Component::get_transform` → `GameObject::get_transform`

### 3.2 Plant::Update（0x0600034C）

原生 `0x18035C4D0..0x18035C8E3`（1043 字节，8 个 .pdata 片段）。关键区段：

```
18035c652: mov 0x178(%rbx),%rcx        ; Plant::shadow
18035c659: test %rcx,%rcx ; je → NRE
18035c664: call 0x18131C070            ; GameObject::get_transform
18035c67d: call 0x18132F3A0            ; Transform::get_position
18035c684: mov %ecx,0x30(%rbx)         ; Plant::fX = position.x
18035c687: mov 0x178(%rbx),%rcx        ; 再次取 shadow
18035c699: call 0x18131C070            ; GameObject::get_transform
18035c6b2: call 0x18132F3A0            ; Transform::get_position
18035c6c1: mov %ecx,0x34(%rbx)         ; Plant::fY = position.y
...
18035c820: call 0x1812E27C0            ; Camera::get_main
18035c833: call 0x181304510            ; Component::get_transform  ← 同一方法内另一个调用点
```

**天然反向对照**：同一方法里 IL_00E1（receiver `GameObject shadow`）原生用  
`GameObject::get_transform (0x18131C070)`，IL_020B（receiver `Camera::get_main()`）原生用  
`Component::get_transform (0x181304510)`。clang 也只报了前者。  
→ 修复必须是**逐调用点**的，不能全局替换。

修复：

- **R3** IL_00E1 `Component::get_transform` → `GameObject::get_transform`
- IL_020B **保持不动**（有原生与 clang 双重证据）

### 3.3 Device::TryPlacing（0x06000339）

原生 `0x18034BE30..0x18034BEA9`（121 字节）：

```
test %rcx,%rcx ; je 18034BEA3          ; board 为空 → 抛 NRE
call Board::GetGrid
test %rax,%rax ; je → return false     ; grid 为空 → 返回 false
call Grid::CanPlacing
test %al,%al   ; je → return false
call Device::Placing  → return true
18034BEA3: call 0x180250150 ; int3     ; 抛 NullReferenceException，帧不返回
```

恢复 IL 的三处偏差：

- IL_0006 / IL_003A 用 `ldc.i4 0` 与引用做 `ceq` → 原生是 `test reg,reg` 与 null 比较
- IL_00A1 在 `newobj NullReferenceException` 之后用 `ret` **把异常对象当 bool 返回**  
  → 原生是抛异常后 `int3`，帧永不返回

修复：

- **R4** IL_0006 `ldc.i4 0` → `ldnull`
- **R5** IL_003A `ldc.i4 0` → `ldnull`
- **R6** IL_00A1 `ret` → `throw`

---

## 4. 四项验证结果

| 闸门                        | 结果                                                                                                                             |
| ------------------------- | ------------------------------------------------------------------------------------------------------------------------------ |
| **G1 非目标隔离**              | 重新导出 2297 个方法：**仅 3 个目标方法变化**，2294 个逐字段相同；token 集合一致（无 MethodDef 重排）；类型表一致                                                     |
| **G2 独立行为测试（类型化 CIL 验证）** | 全量 2297 方法：FAIL 872 → **869**；`0x06000348`/`0x0600034C`/`0x06000339` FAIL→PASS；**PASS→FAIL 为 0**                               |
| **G3 反向测试**               | 6 处修改**逐项单独回退**，6/6 均复现失败，无一项是"可有可无"                                                                                           |
| **G4 真实全量 C++ 复测**        | 放回 IL → 重新转译 264 个 `.cpp` → 全量 clang 扫描：error **6006 → 6001（−5）**；失败文件 24 → 24；**无新增失败文件**；token 级 608 → **605**，仅 3 个目标清零，无增长 |

G3 逐项回退的验证器输出：

```
0 IL_0007 retype_call  → receiver UnityEngine.GameObject incompatible with UnityEngine.Component
1 IL_0023 retype_call  → receiver UnityEngine.GameObject incompatible with UnityEngine.Component
2 IL_00E1 retype_call  → receiver UnityEngine.GameObject incompatible with UnityEngine.Component
3 IL_0006 to_ldnull    → IL_000B: invalid comparison Board, I4
4 IL_003A to_ldnull    → IL_003B: invalid comparison Grid, I4
5 IL_00A1 ret_to_throw → IL_0099: System.NullReferenceException is not assignable to System.Boolean
```

产物 sha256：

- 基线 `0d6bc587…`（存于 `.validation/native22/input/GodsPVZRuntime1.baseline.dll`，未被改动）
- 修复后 `4d970f5ed7e63dd42815c3c843ce51918328ff790fa5d1eb77e2a80e6f3b0a1b`  
  （已放回 `ManagedStripped/GodsPVZRuntime1.dll`；原文件另存为  
  `retest/ManagedStripped.pre-batch1.dll`）

---

## 5. 三项结果分开报告

### 5.1 编译（本批次范围内）

- **本批 3 个方法：编译通过**（重转译后方法体内 0 条 clang error）。
- **整包：仍未编译通过。** 全量 264 个 TU 仍有 **6001 条 error**、24 个文件失败、  
  605 个方法体内含错误。本批只解决了其中 5 条。
- 这是语法/类型层面的 `-fsyntax-only` 结果，**不等于链接通过，更不等于语义正确**。

### 5.2 IPA

- **未做任何打包。** 本批不涉及 Unity/Apple 构建、链接或 `.ipa` 产出。
- 目标仍为**未签名** IPA（用户已明确"不用签名"），因此即使产出也无法安装到未越狱 iPhone。

### 5.3 游戏行为正确

- **本批 3 个方法的 IL 语义已与 PC 原生逐条比对确认**（调用归属、空检查、异常语义、  
  字段次序、尾调用），并通过了类型化 CIL 验证。
- **没有任何真机/模拟器运行验证。** 未启动游戏、未截图、未跑过一帧。
- 因此"游戏行为正确"目前只对这 3 个方法有原生级证据，**对整包不成立**。

---

## 6. 本批明确未做的事

1. **未修复其余 605 个含错误的方法。**
2. **`Zombie::GetDamage`（0x06000443）已停止机械修复，转入原生取证，本批未完成：**  
   其恢复 IL 中混入了反编译器错误消息作为 `ldstr`+`pop`  
   （`"Invalid instruction: 82 …"`、`"Jump target not found in method: 0x180360E45"`、  
   `"Method ends with non empty stack (-58), the output could be wrong!"`），  
   且存在缺失分支与死代码。原生区段 `0x180360D00..0x180360F54`（596 字节，22 调用点，  
   9 个未解析目标）需要完整重建。按约定，遇到缺失操作数/分支/调用时不机械修复。
3. **未做** Unity 真机构建、Apple 链接、IPA 打包、真机行为验证。
4. **未提交任何 git 提交**；工作区改动都在 `.validation/` 与 `scripts/` 下。

---

## 7. 下一步建议（待你定夺批次范围）

按结构族群，后续可分批推进：

- **A 族（引用与整型零比较）共 2194 条**，核心类纯 A/B 的有 **58 个方法**。  
  本批已证明其变换规则（`ldc.i4 0` → `ldnull`）在整体验证 + 反向测试下成立，  
  可在"整体验证通过"前提下按同一规则批量推进。
- **C 族（调用归属 `Component` vs `GameObject`）**：核心类有 128 个，  
  多数只有 1 条错误。建议按接收者类型逐调用点核对后推进。
- **`Zombie::GetDamage` 一类"反编译器噪声 + 缺失分支"**：单独一批，需完整原生重建。
