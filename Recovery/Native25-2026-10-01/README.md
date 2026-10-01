# Native25 — Device.TestPlacing 完整方法体恢复

2026-10-01，在 `glowing-train-p7j9gp74q6jwc76v6` 的持久工作区执行。此检查点累计继承 Native24 已验证的三个方法六站点，再完整恢复 `Device.TestPlacing`。**未生成 IPA；全程序集和整游戏尚未通过。没有本轮持证 Unity/iOS 导出、Apple 构建、链接或设备测试。**

## 输入恢复与工作位置

Codespace 重启后，Native24 的 `/tmp` 输出已丢失。已从已发布的 Native24 Release 按 SHA 恢复完整 1930 成员及三套各 330 个生成文件，并核对 `raw-1.dll` 与真实 DLL/EXE 支持程序集。以后使用 `/workspaces/GodsPVZ-native24-codespace/.validation/native25-2026-10-01` 保存工作文件。该目录仍应依靠已发布归档备份，不能将工作区持久性当成归档替代。

锁定输入 SHA-256：`8300c14ebac3b45cc00397c69cd72d4517366587acbe081b6e8b5a98f8fdedf9`。

确定性输出 SHA-256：`9cbf08472d6c22f1b68393656ac2d36576fd6b21d49c4738f80aec2e2cbe749a`。两次输出逐字节相同。

工作分支沿用 `repair/codex-native24-codespace`，本轮起点 `a45d851f84f620fbc32e44446b70b98240843fcf`。main、原工作树及原版 PC 输入保持原样。

## 原版 native 证据与完整规格

原版 `GameAssembly.dll` SHA-256：`9ebd7ca996a5b03fb4a766f7a2502b660d581ddbbb36af4d7bf2f06a211da39d`。

优先并且严格选择 `Assembly-CSharp.dll` image 的 MethodDef `0x06000338`，避开跨 image token 碰撞。PC 函数范围 `0x18034BDF0..0x18034BE2E`，62 字节；紧邻下一个映射方法起点为 `0x18034BE30`。函数字节 SHA-256：`de866271dda65fcc8a2e8e761cc543ac4200daaa43d4ffb5b6edd3aa690e3e49`。

| native 地址 | 数据流/分支 | 恢复 CIL |
|---|---|---|
| `18034BDF6..18034BDF9` | 将 Device this 保存于 RBX，加载 this+0x28 的 board | `ldarg.0; ldfld Board Device::board` |
| `18034BDFD..18034BE00` | board 物理空引用跳转至异常 helper | `dup; brtrue.s board; pop; newobj NullReferenceException; throw` |
| `18034BE02..18034BE05` | 原 EDX/R8D 坐标保持，R9 method-info=0，调用 Board.GetGrid | `ldarg.1; ldarg.2; call Grid Board::GetGrid(int,int)` |
| `18034BE0A..18034BE0D` | GetGrid 的 RAX 为零时跳转至 return | `dup; brtrue.s grid; pop; ldc.i4.0; ret` |
| `18034BE22..18034BE27` | RAX 保持 GetGrid 的零返回值，AL=0，即 bool false | 上述 false 返回路径 |
| `18034BE0F..18034BE1D` | RCX=Grid，RDX=原 Device，R8 method-info=0，尾跳 Grid.CanPlacing | `ldarg.0; call bool Grid::CanPlacing(Device); ret` |
| `18034BE28..18034BE2D` | `call 180250150; int3` 异常路径 | 抛出 NRE，方法不返回 |

两个业务调用地址分别为 `0x180326FE0` / `0x060002C4 Board.GetGrid` 和 `0x180329AA0` / `0x06000297 Grid.CanPlacing`，PC 映射为唯一目标。`0x180250150` 是历史审计识别的 NRE helper，不具有 managed MethodDef 映射，本轮记录实际调用和终止指令，不伪造其 managed token。

旧 body 的问题超出判空：将 Grid 返回值写入 bool V2，null 路径把 V2 当 bool 返回，异常路径将 NRE 当 bool 返回。因此完整重建 body，而不是在损坏的 local flow 上继续修补常量。

完整新 body 是 36 字节、18 条指令，无 locals、无 EH，MaxStack=3：

```text
IL_0000 ldarg.0
IL_0001 ldfld Board Device::board              [0x04000463]
IL_0006 dup
IL_0007 brtrue.s IL_0010
IL_0009 pop
IL_000A newobj NullReferenceException::.ctor() [0x0A000022]
IL_000F throw
IL_0010 ldarg.1
IL_0011 ldarg.2
IL_0012 call Grid Board::GetGrid(int,int)       [0x060002C4]
IL_0017 dup
IL_0018 brtrue.s IL_001D
IL_001A pop
IL_001B ldc.i4.0
IL_001C ret
IL_001D ldarg.0
IL_001E call bool Grid::CanPlacing(Device)      [0x06000297]
IL_0023 ret
```

上述 token 均复用锁定输入的已有字段/方法引用。没有新增元数据行。旧方法体原有的 117 字节存储区容纳新 body；方法头的 CodeSize 改为 36，LocalVarSigTok 改为 0，余下 81 字节清零为存储区填充，**不属于新 CIL，也不是不可达 NOP 指令**。RVA、整个 DLL 长度和其他方法的存储位置不变。预备的旧布局试验如被保存在归档中，只作未验收实验，最终资格结果只对应 `9cbf0847…`。

## 独立验证

| 验证 | 实测结果 |
|---|---|
| 确定性构造 | 两次输出全文件相同；错误父 DLL、同路径、已有输出均拒绝 |
| 增量字节隔离 | 92 个变化字节全部在目标原存储区或方法头的 CodeSize/LocalVarSigTok；2316 个非目标 MethodDef、其中 2296 个方法体原始字节及完整内容完全相同 |
| 累计字节隔离 | 相对最初 `0d6bc587…` 基线，累计四个目标外的 2313 个 MethodDef、2293 个方法体完全不变 |
| 全部声明 | assembly/module/MVID、类型、字段、方法签名、属性、参数、约束、资源等独立快照相同；metadata row counts 相同 |
| E1 指令子集类型门 | 2297 个 body，PASS 1428→1429，FAIL 869→868；只有目标 FAIL→PASS，没有 PASS→FAIL；目标18/18可达、无不可达指令 |
| 实际 CLR 执行 | .NET 10.0.12，46 个新用例全部通过；覆盖 null this/board/grid、允许/拒绝、坐标极值、helper 异常和 helper 修改对象图 |
| 行为反例 | 六种仍能执行的错误 CIL：两个分支反转、null-grid 错误返回 true、坐标交换、错误 Device 实参、跳过 CanPlacing，全部被实际执行结果检出 |
| 继承行为回归 | Native24 的24个 CLR 用例继续通过，两个原行为反例继续检出 |
| 实际 IL2CPP | exit 0，264 个 CPP TU；历史 unityaot-macos qualifier on Linux，非持证 Unity iOS 导出 |
| 生成代码隔离 | 2266 个已映射非目标 CPP 函数文本逐一相同；仅 `GodsPVZRuntime1__6.cpp` 和 `Il2CppMetadataUsage.c` 文件发生变化；global-metadata.dat 与输入阶段相同 |
| 完整 Linux C++ 预检 | 全部264个TU：6065→6063总错误，24个失败TU保持；含错误方法673→672；仅目标2→0，没有方法错误增长 |
| 额外 C 文件检查 | 父版与新版 Il2CppMetadataUsage.c 均显式作为C++检查，exit0、0错误 |

**行为证据的范围**：执行的是候选真实 CIL，但将 Board/Grid 业务调用显式绑定到 helper doubles，用原版 native 的调用顺序、参数、分支和返回约束作预期。没有执行原版 GameAssembly 函数、原版 Board.GetGrid/Grid.CanPlacing 实现或 Unity Player。本轮不宣称整方法在真实游戏中的全部行为已实测，更不宣称整个游戏已还原。

Linux 编译器为 Ubuntu Clang18.1.3，使用锁定 Native18 导出的头文件与既有参数。总错误6063包含64条头文件错误；生成CPP错误5999，其中38条位于游戏方法之外。仍有672个含编译错误的方法和868个E1失败方法。这两组集合不是同一个分类。

## 完整归档及复现

源码为 `scripts/takeover/native25/RepairNative25` 和 `RuntimeFixture`；具体复现脚本在 `scripts/takeover/native25/checkpoint`，运行记录保留实际Codespace路径。

完整证据 ZIP 在本检查点对应的 GitHub Release，下载信息与 SHA 见 `EVIDENCE-ARCHIVE.json`。它包含所有本轮源文件/日志、独立元数据和声明快照、typed 全量输出、native 字节及反汇编、完整 PC method-map、候选，以及生成的两个变化文件和全部生成文件 SHA。其余328个生成文件由固定 Native24 父归档恢复，不重复存储；恢复时必须先核对父归档 SHA，再核对本轮每个生成文件 SHA。Native24 的真实支持 DLL/EXE 继续作为锁定输入，不使用 dummy 代替。

所有完整归档成员按 MANIFEST.json 核验。当前网页摘要和原始机器数据出现解释冲突时，优先检查具体输入 SHA、源代码、指令和日志，不将“错误数减少”当作语义证明。

## 下一批与尚未通过的门

`AttackRange.TestInRange_Device`（0x060000D9）已取回289字节PC函数：native是完整的 rangeType 分派，恢复IL却重复使用旧 bool 判据，丢失Rect/Vector3构造并错误按地址传参。只有判空站点证据不能修好它。后续应完整重建，保留 Rect→Circle 的短路顺序及 helper 调用前捕获坐标的时机。历史 `AttackRange.TestInRange<T>` 具有包含Rect/Vector3的闭合局部表，可作为复用现有StandaloneSig的候选；必须在实际当前DLL上核对其token、blob及无未闭合泛型，不能仅凭旧typed字符串采用。

`BoardInfoPage.SetLootList`（0x060005C8）已有有界native反汇编，但原674条IL、98个locals有多个损坏循环/类型路径。此次没有修改它。Native24 的254个E2来源支持站点仍不作为批量晋级依据。

继续顺序：逐方法原版native CFG/字段/调用证据→完整规格→确定性构造及原始字节隔离→类型门和真实行为反例→新IL2CPP及所有编译单元→源码与证据归档。整包静态与平台条件满足后才进入持证Unity iOS导出和macOS Xcode构建。最终目标保持未签名IPA。

归档已实际恢复并重建全部330个生成文件，各文件SHA及候选SHA核验通过。下载：[完整证据 ZIP](https://github.com/lyiming710-cloud/GodsPVZ-iOS/releases/download/native25-codespace-2026-10-01/native25-codespace-2026-10-01-evidence.zip)。
