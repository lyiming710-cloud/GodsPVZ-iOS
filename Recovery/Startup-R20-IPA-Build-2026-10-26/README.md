# R20 未签名 IPA 实际构建成功

[完整工作流 40263428409](https://github.com/lyiming710-cloud/GodsPVZ-iOS/actions/runs/40263428409) 的 Unity 导出、全部实际导出源码 Apple 检查、Xcode 编译链接和未签名打包均成功。固定输入仍是 R20 d887b370…095b37，固定工作流提交 c22560ed9e025f04d4e58bc38ba0f23df8b9feb4。

[下载测试 IPA](https://github.com/lyiming710-cloud/GodsPVZ-iOS/releases/tag/startup-r20-ipad-test-2026-10-26)。192,091,143 bytes，SHA-256 `f7334b1b330cdc7dc2cbeeb0c9851c8c233bc0fc923c69e6de36f38aeda56c75`；arm64 / physical iOS / unsigned。实际 BuildReport 为 Succeeded、0 errors、1,364 warnings；258 C++ / 66 C 全部原始 Apple 日志核对为 0 errors、3,584 warnings；独立 xcodebuild 为 BUILD SUCCEEDED。包内 metadata 与本次真实导出完全相同。

此前 40255311794 和 40259026509 的失败继续保留：同一组资源大小写冲突被四次报告为 Error，原 BuildIOS 错误门正常拒绝。对 12 份资源及 12 份 meta 只移动路径，保留原 basename、内容和 GUID；全部 6,842 文件 hash 保留，实际 Unity 中 12 份资源的 GUID/路径/名称核验通过。没有删除原始资源、忽略 Error 或改变游戏 DLL 来换取构建成功。

真实 Editor MainMenu 1,862 帧/0 错误，148 脚本绑定及 76 shader stage/20 受控软件 OpenGL 像素通过。R19 仍是 81 个有限 profile、19 个待验收，R21 的 16 个提案尚未构造且不包含在此包。M5 iPad 的主菜单、闪烁、实际声音、Board 及整个游戏行为仍待真机与后续验收。

[完整字节回读、原始日志、源文件和 Release 资产清单](../Startup-R20-IPA-2026-10-26/README.md)。本目录 PRE-IPA-SUCCESS 文件保留构建前状态；历史检查结论不覆盖为成功。
