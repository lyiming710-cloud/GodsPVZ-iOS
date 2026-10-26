# R20：104 个位置、8 个方法；未签名 IPA 已构建

当前候选 SHA-256：`d887b370c95c2966dc40d80648d85c286bb42d85004c32d095f64f004e095b37`。固定父候选 R19 candidate9：`0ba5bc7ec9603d14a1c0bde6d784d232aa78efaa1700304e75e4f2b56ef7dec2`。

按用户“先一共修复一百个地方以上再检测”，先构造 104 个原生语义位置、8 个不同 MethodDef，再共享检测。按 MethodDef + 原版指令 VA 去重：35 调用、15 字段写入、41 字段读取、11 控制流、2 循环状态；参数栈准备、元数据辅助、GC barrier、重复改动不计入。这不是 104 个方法或独立 bug。

## 范围和隔离

DeviceManager.Start/Update、ProjectManager.Start/Update、SunManager.Start/Update、ZombieManager.Start、SavesUIController.LoadPlayerSaves。八个损坏空方法与其他回调共享旧 ret 槽位，恢复时追加独立方法体，只改目标 RVA；旧共享槽位、2,309 个非目标 MethodDef、原元数据前缀、权限保持不变。新增两个 MemberRef、三个局部签名、一个闭合 Instantiate MethodSpec。冻结类型、成员访问及目标实际 DLL JIT 准备通过。

## 已完成的分层证据

- 完整候选 CIL 经冻结 Rehost 发射并执行，与原版 PC 完整未改调用方比较：管理器 552、存档 UI 216，共 768 正常记录一致；另 87 个 CLR-only 异常独立保留，不冒充 Windows SEH。八个物理退回 tiny-ret 的遗漏控制全部产生目标行为差异，其他方法不变。
- 精确 DLL 的新 IL2CPP 和 755 C++/274 C（1,029 单元）Linux/Apple 语法检查通过，全部返回源文件身份与原始日志已独立核对。Apple 损坏控制原始日志被同名覆盖的问题保留，并由独立 34-error 回放补齐，不改正常候选。
- [完整实际构建 40263428409](https://github.com/lyiming710-cloud/GodsPVZ-iOS/actions/runs/40263428409)：R3 路径大小写修复保留全部 6,842 个资源文件 hash、basename、GUID；Unity 中 12 个原始名称验证通过。实际 MainMenu 运行 1,862 帧，零错误，148 脚本/130 序列化标记，无缺失组件；76 shader stage、20 软件 OpenGL 受控像素检查通过。
- 实际 Unity BuildReport Succeeded、0 errors、1,364 warnings。此次真实导出共 258 C++/66 C，全部 Apple 原始日志为 0 errors、3,584 warnings；此 324 单元集合与可移植检查的 1,029 单元分开记录。真实 Xcode 编译链接 BUILD SUCCEEDED。
- [未签名 arm64 iPad IPA](https://github.com/lyiming710-cloud/GodsPVZ-iOS/releases/tag/startup-r20-ipad-test-2026-10-26) 已下载并独立验证 CRC、Mach-O、平台、无签名、导出绑定和包内 metadata；SHA-256 `f7334b1b330cdc7dc2cbeeb0c9851c8c233bc0fc923c69e6de36f38aeda56c75`，192,091,143 字节。[完整新包证据](../Startup-R20-IPA-2026-10-26/README.md)。

## 保留的失败与边界

旧候选误读 MouseManager+0x68 的 Card/Prop 差异、原生模型丢独立 Card 对象、存档 fixture 泛型绑定失败、控制启动路径失败、Apple 原始日志覆盖均保留。当前 Card.cardDeviceType 链修复为原版 +0x68/+0x2C，有限记录零分歧。旧 40255311794/40259026509 真实导出因资源大小写冲突被拒绝，失败的全部报告和生成源码独立归档。此前 README/PROGRESS 原样保存于 PRE-IPA-SUCCESS 文件，详尽早期源、构造记录和原始测试仍在本目录其余文件。

R19 当前 81 个有限 profile、31,812 正常原版对照，19 个仍待验收；四个已有差异方法仍保留待修状态。R21 有 16 个源提案、5 个方法、0 个构造位置，未写入本包。全程序集 70 条类型失败或不支持项未被这些有限结果抹掉。qualified_methods=0/whole_game_accepted=false 保留产品语义验收边界。

新 IPA 尚未在 iPad Pro M5 / iPadOS 27.0 验收。黑屏、闪烁、声音、Board、真实 Metal 渲染、异常展开与整个游戏行为都不能从 Editor 或构建结果推导为通过。后续新游戏批次仍按超过 100 个有原版依据的实际位置再集中检测，不修改正确代码凑数。
