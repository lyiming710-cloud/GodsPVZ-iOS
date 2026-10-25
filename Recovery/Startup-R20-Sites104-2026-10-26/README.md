# R20 当前检查点：104 个位置，8 个方法

当前候选 SHA-256：`d887b370c95c2966dc40d80648d85c286bb42d85004c32d095f64f004e095b37`。父候选是固定 R19 candidate9：`0ba5bc7ec9603d14a1c0bde6d784d232aa78efaa1700304e75e4f2b56ef7dec2`。

按用户“先一共修复一百个地方以上再检测”，本批先构造 **104 个原版语义操作位置，涉及 8 个不同 MethodDef**，然后才启动共享检测。104 不是方法数，也不是独立 bug 总数。每个位置按 MethodDef + 原版指令 VA 去重，包含 35 个调用、15 个字段写入、41 个字段读取、11 个控制流和两个循环状态。参数/栈指令、元数据辅助、GC barrier、空白和重复修改不计。完整 CIL 区间、原版字节和开始检测的阈值记录均已归档。历史 R19 的 100-method 政策原样保留。

## 恢复范围

DeviceManager.Start/Update、ProjectManager.Start/Update、SunManager.Start/Update、ZombieManager.Start、SavesUIController.LoadPlayerSaves。保留原版资源加载/赋值/回调顺序、回调后重新读取字段、Unity 对象判定、列表循环和最后一个 ID=-1 的存档项。这八个历史空方法与正常空回调共享 ret 槽位，因此追加独立方法体，仅修改目标 RVA；旧槽位保持不变。

## 已执行的检查及其边界

- 8 个目标冻结类型、成员访问、实际 DLL JIT 准备通过；独立隔离保持其余 2,309 个 MethodDef、旧元数据前缀、权限及旧方法槽位不变。新增两个 MemberRef、三个局部签名、一个闭合 Instantiate MethodSpec。
- 完整候选 CIL 经冻结 Rehost 发射并在 CLR 执行，与原版 PC 完整调用方机器码比较：七个管理器 552 条、存档 UI 216 条正常路径，共 **768 条一致、0 处分歧**。另有 87 条仅 CLR 异常记录，没有伪装成原版 Windows SEH 对照。
- 存档 UI 覆盖旧项为空/存活/销毁/空引用、0/1/3 个存档、Instantiate 返回空、销毁或 Start0 回调替换列表/管理器/数量/父级内容，以及最终特殊项；真实 CLR List 的元素、容量、版本与原版内联 Clear/Add 效果一并观察。
- 8 个物理 DLL 故障变体逐一将目标 RVA 退回父版本 tiny-ret。全部进程正常完成，8/8 都出现目标行为差异，其他方法没有差异。差异不是靠 InvalidProgram 或崩溃计数。完整变体可由候选和 DELTA-RECONSTRUCTION 重建并校验 SHA。
- [当前 Linux run 40219580971](https://github.com/lyiming710-cloud/GodsPVZ-iOS/actions/runs/40219580971)：新 IL2CPP 转换退出 0，755 个 C++/274 个 C 源文件全部语法检查 0 错误。1,029 份返回源码与原始日志均独立回读并校验大小/SHA；保留 2,597 条警告、六个编译控制和真实损坏 R4 的 37 条错误。运行器 DLL 身份由原始输入清单回读支持，未返回物理 staged DLL。
- 当前候选 Apple 检查已使用精确 Linux 生成包准备；编译参数、1,029 源文件检查、六个控制和损坏 R4 34-error 控制沿用原算法。实际结果见后续 ACTUAL-APPLE 文件，未执行前不记通过。

## 实测纠错和失败证据

初版候选把 MouseManager+0x68 错认成 Prop+0xB0。初版原生桩也遗漏独立 Card 对象，产生两次进程异常。修正输入模型后，对旧候选观察到 27 条真实差异；随后只把 DeviceManager.Update 修正为 cardOfBankOnHand -> Card.cardDeviceType（原版 +0x68/+0x2C）。Card/Prop 使用相反 sentinel，避免共同建模错误被隐藏。全部原始崩溃、旧模型、旧候选和 27 处差异保留。旧 run 40218209871 的绿色只属于旧候选，不能继承。

存档 fixture 首次不能闭合 List<T>.Add 的 !0 参数，0 用例执行即失败；修正 fixture 的反射绑定后再运行，未修改冻结 Rehost 或游戏候选。负控制首次误用 Debug 路径，也按工具启动失败保留，未记为拦截成功。

## 尚未证明

Unity 生命周期、资源/游戏叶函数内部、实例化/销毁调度、UI 渲染和 Windows SEH 均不由上述受控叶函数证明。有限输入无差异不能推出普遍语义等价。全程序集仍保留 70 条失败或不支持项，R19 仍有 35 个运行 profile 待完成；其他语义标记/工具覆盖审查队列继续开放。

本批没有新的持证 Unity Editor 导入/导出、播放器链接、未签名 IPA 或 iPad 实测。qualified_methods=0 表示没有推进产品资格，并不抹掉明确列出的有限测试成果。iPad 黑屏/闪烁是否解决仍待新包和真机验收。

## 当前候选 Apple 实测和日志保存纠正

[Apple run 40220652002](https://github.com/lyiming710-cloud/GodsPVZ-iOS/actions/runs/40220652002) 的 755 C++/274 C arm64 语法单元全部通过。逐一核对 1,029 个返回源文件身份与原始日志，0 错误，保留 10,572 条警告；Xcode 16.4/iPhoneOS18.5/arm64 参数、六个编译控制均核对。没有新 Unity 导出或播放器链接。

回读时发现历史 Apple driver 的 run_one 只按 basename 保存日志：损坏的历史 Generics__63.cpp 先产生 34 条错误，随后同名正常文件覆盖其 raw log。JSON 中原始 34 条错误及非零进程结果仍保存；正常源码的 1,029 个日志有效，损坏控制 raw log 单列为缺项。这个问题在历史 R19 原始证据中也存在。本次先保存覆盖前 driver/两份 JSON 与现存日志，再为损坏控制增加独立日志目录，并只补跑该一个控制单元，未修改候选、编译参数、解析或判定规则。补跑未完成前不把 raw log 记作已保留。

完整生成源码包（152,239,861 字节）另外上传到仓库 Release，远端资产大小与 SHA-256 元数据已匹配，避免仅依赖短期 Actions artifact。见 GENERATED-SOURCE-RELEASE-RECEIPT.json。
