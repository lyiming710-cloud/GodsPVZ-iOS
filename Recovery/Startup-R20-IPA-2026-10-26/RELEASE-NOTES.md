本次实际 Unity → Xcode → 未签名 IPA 已构建成功，用于 iPad Pro M5 的新一轮真机反馈。

- 固定源码提交：`c22560ed9e025f04d4e58bc38ba0f23df8b9feb4`；游戏候选：`d887b370c95c2966dc40d80648d85c286bb42d85004c32d095f64f004e095b37`。
- [真实工作流](https://github.com/lyiming710-cloud/GodsPVZ-iOS/actions/runs/40263428409)：Unity 0 errors；实际导出的全部 258 C++ / 66 C 检查通过，Xcode BUILD SUCCEEDED。
- 资源大小写冲突修复保持全部 6,842 个 Assets 文件内容 hash、GUID 和原 basename。
- IPA：192,091,143 bytes；arm64 / physical iOS / unsigned。主程序与 UnityFramework 的最低系统为 12.0；使用 Xcode 16.4 / SDK 18.5。
- IPA SHA-256：`f7334b1b330cdc7dc2cbeeb0c9851c8c233bc0fc923c69e6de36f38aeda56c75`。

下载 `.ipa` 后沿用现有签名安装方式。尚无新包 iPad 实测结果；请记录主菜单、持续画面、触控、进入 Board 和声音。没有宣称黑屏/闪烁已经解决。

原始导出源归档和完整独立回读包随附；每份文件有 SHA-256 清单。生成源归档内 qualified=false 的诊断捕获标记保持原样，构建结论另由实际报告、全部原始编译日志、导出绑定和 IPA 字节核对支持。

R19 当前 81 个有限 profile、19 个待验收；R21 的 16 个提案尚未构造、未包含在此包。该预发布不代表整个游戏或全部方法已高还原验收。
