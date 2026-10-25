# R20：先完成 104 个修复位置，再统一检测

本批基线是固定 R19 candidate9（0ba5bc7ec9603d14a1c0bde6d784d232aa78efaa1700304e75e4f2b56ef7dec2）。当前修正候选 SHA-256：`d887b370c95c2966dc40d80648d85c286bb42d85004c32d095f64f004e095b37`。

按用户 2026-10-26 的“先一共修复一百个地方以上再检测”，本批记录 **104 个原版语义操作位置，分布在 8 个 MethodDef**。这不是 104 个方法，也不是 104 个独立游戏 bug。每个位置以“MethodDef + 原版指令地址”去重，记录原版指令字节和新 CIL 区间；类型分别为 35 个调用、15 个字段写入、41 个字段读取、11 个控制流位置和两个循环状态位置。普通参数/栈指令、元数据初始化、GC barrier、空白和重复修改不计。历史 R19 的 100-method 政策保存在 PARENT-BATCH-POLICY.json。

## 实际恢复范围

- DeviceManager.Start：原版板块初始化、资源加载、字段赋值、预览初始化顺序。
- DeviceManager.Update：Unity 对象判定后的 board 重载、mouse 捕获、无符号手持物分区、道具 -1 sentinel、仅状态改变时调用显示刷新。
- ProjectManager.Start/Update：prefab 加载，以及分别读取全局存档链的金币/钻石初始化；逐帧项目和钱库更新。
- SunManager.Start/Update：银行 UI 文本绑定、初始落阳光时间；真实整数格式化、文本回调后的 board 捕获、游戏开始/暂停/结束条件及原版两个更新调用顺序。
- ZombieManager.Start：prefab 赋值和关卡数据初始化。
- SavesUIController.LoadPlayerSaves：旧 UI 项销毁和列表重载/清理、实际存档数量、逐项实例化/父级绑定/ID 与管理器赋值/Start0/Add，以及最后 ID=-1 的新建项。

这些原方法与许多正常空回调共用同一 ret 槽位。本批为八个目标追加独立方法体，保留所有旧槽位。只追加两个 MemberRef、三个局部签名和一个 Instantiate<SavesUIController_PlayerSave> MethodSpec。

## 检测顺序与当前结论

先完成 104 处构造，再重建第二份相同候选并记录 SHARED-TEST-START.json，之后才执行共享检测。八个新方法的冻结类型检查、实际成员访问、真实 DLL JIT 准备均通过；全程序集保留原有 70 条失败或不支持项。独立隔离检查确认其余 2,309 个 MethodDef、旧堆/表行/方法体/权限不变，三个出界物理负控制被拒绝。

当前尚无这八个完整调用方的原版运行对照、当前候选 IL2CPP/Apple 实测结果、新 Unity 导出、链接后 IPA 或 iPad 验收。编译工作流已绑定这个新候选，仅作诊断；不得继承 R19 的编译绿色作为 R20 通过。qualified_methods=0。原 R19 的 35 个待运行对照方法和其他全量清单问题保持开放。

## 实际运行对照发现并修正 Card 对象链

最初 authored1/authored2 将 DeviceManager.Update 的 MouseManager+0x68 错写为 Prop+0xB0。初版原生执行桩也只构造了 Prop 对象，首次到达真实 Card 路径发生进程异常；两次崩溃和输入/桩源码已保留，不算检测通过。核对原版字段表后给原生桩和 CLR fixture 同时补充独立 Card/Prop 对象，并给它们相反的 sentinel 值，原版 552 条正常记录与旧候选产生 27 处真实差异。

修正为 MouseManager.cardOfBankOnHand -> Card.cardDeviceType，分别对应原版 +0x68、+0x2C。authored3/authored4 字节一致，只有 DeviceManager.Update 相对旧候选改变；仍为原来的八个目标/104 个位置，不额外计数。新候选七个管理器方法的 552 条完整 CIL/原版正常对照一致；85 条 CLR-only 空引用记录另列。调用链内的资源、游戏叶函数和文本设置是明确受控的依赖，这不是实际 Unity 生命周期或画面验收。存档列表重建的完整运行对照尚待完成。

旧候选的 GitHub Linux run 40218209871 界面显示 success，但它属于旧 SHA，既不能证明旧候选语义正确，也不能资格化当前新 SHA；其完整源文件/原始日志尚未独立回读。当前工作流已重新绑定新 SHA。PRE-CARD-CORRECTION 文件、旧 27 处差异和原生桩进程异常均保留。
