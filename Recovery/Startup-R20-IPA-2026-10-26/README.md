# R20 未签名 iPad 测试 IPA：真实构建通过

[实际构建 40263428409](https://github.com/lyiming710-cloud/GodsPVZ-iOS/actions/runs/40263428409) 已成功完成 Unity 导出、全部实际导出 C/C++ 检查、Xcode 编译链接及 IPA 打包。固定构建提交为 `c22560ed9e025f04d4e58bc38ba0f23df8b9feb4`，游戏候选仍是 `d887b370c95c2966dc40d80648d85c286bb42d85004c32d095f64f004e095b37`。

下载入口：[R20 iPad 测试包 Release](https://github.com/lyiming710-cloud/GodsPVZ-iOS/releases/tag/startup-r20-ipad-test-2026-10-26)。IPA 为 `GodsPVZ_1.0.2_startup_r20_unsigned.ipa`，192,091,143 字节（约 183.2 MiB），SHA-256 `f7334b1b330cdc7dc2cbeeb0c9851c8c233bc0fc923c69e6de36f38aeda56c75`。未签名，沿用已有签名安装方式；本次没有请求或使用签名身份。

## 本次实际证据

- Unity BuildReport 为 Succeeded、0 errors、1,364 warnings；逐步骤也没有 Error/Exception/Assert。保留警告，不解释为零缺陷。
- 真实 Editor MainMenu 连续 1,862 帧，两个 Canvas、一个 MainUI，0 启动错误；148 个插件脚本绑定、130 个原序列化标记均保留，无缺失组件。这是 Editor 启动范围。
- 六组大小写冲突通过仅移动 12 份资源及 12 份 meta 解决；原始 basename、内容、GUID 保留，全部 6,842 个 Assets 文件 hash 保留。实际 Unity 对 12 份资源的 GUID、路径、LoadMainAsset.name 全部核验通过。
- 从本次真实导出取得 258 个 C++、66 个 C 文件，共 324 个。逐个源文件 hash、原始 Apple 编译日志已回读：0 errors，3,584 warnings。六个编译控制为两正四负，均符合预期。独立的真实 xcodebuild 最终记录 BUILD SUCCEEDED。
- IPA ZIP 共 647 项，CRC 完整；主程序和 UnityFramework 均为 thin arm64、physical iOS，无有效签名或 provisioning；实际 Mach-O 最低系统 12.0，SDK 18.5。包内 6,985,488 字节 metadata 与本次真实导出逐字节一致。
- 329 个原始导出归档文件、326 个导出绑定文件、324 个源文件/原始日志、96 个 shader 程序/PNG 均逐份核对。20 张受控渲染 PNG 中心像素另行解码核对；76 个 shader stage 检查通过。像素结果来自软件 OpenGL，不能据此代替 iPad Metal 实测。

Apple 的 syntax-only driver 保留历史 `-mios-version-min=10.12`，与最终链接 Mach-O 的 12.0 不同；原始 flags 全部保存，没有声称两组命令完全相同。真正 IPA 由之后的独立 xcodebuild 产生，已另行检查最终二进制。首次独立 reader 误假定 target 参数拼写，先在断言处停止，初始源代码和失败记录保留；修正为读取真实 arm64/SDK 参数后完成验证，没有更改产物。

## 证据如何保存

`ACTUAL-BUILD-AND-INDEPENDENT-READBACK.zip` 包含完整返回的 Editor/Apple/着色器原始文件、独立 reader、第一次 reader 假定失败、固定提交的 scripts/actions 与实际工作流、原始构建 helper、资源路径计划，以及固定输入候选包；内有逐文件 MANIFEST。生成 C/C++/metadata/剥离后程序集的原始大归档另存于 Release 的 `Startup-R20-Actual-Export-40263428409-generated-and-report.zip`。

大归档内历史捕获器的 qualified=false/UNQUALIFIED 名称原样保留，表示捕获器自身不授予资格；本目录单独以本次真实 BuildReport、Apple/Xcode 日志、导出绑定和 IPA 字节建立构建通过结论，没有改写原始标记为全游戏合格。较早的 40255311794/40259026509 失败、原报告和源文件继续保留在既有检查点。

## 当前验收边界及下一步

这次 R20 是先完成 104 个原生语义位置、8 个方法后集中检测的包，不是修复了 104 个独立 bug。R19 有 81 个有限输入 profile、31,812 条正常原版对照；另 19 个方法仍待验收，其中四个已有差异。新源队列有 16 个提案、5 个方法、0 个新构造位置，未包含在此 IPA；继续按超过 100 个实际源位置再集中检测。

尚未在用户的 iPad Pro M5 / iPadOS 27.0 运行新包，未证明此前黑屏、闪烁、无声音已解决。设备验收应记录启动后能否进入 MainMenu、画面是否持续稳定、按钮是否响应、是否能进入 Board、是否有实际声音；使用现有签名安装方法并保留存档。游戏行为与高还原验收仍开放，不能把本次构建成功提升为整个游戏语义恢复完成。
