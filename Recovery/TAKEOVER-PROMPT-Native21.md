# 给接手 AI 的任务说明

请接手 `lyiming710-cloud/GodsPVZ-iOS` 的高保真原版恢复和未签名 iOS IPA 构建。先完整阅读同目录 `HANDOFF-2026-09-29-Native21.md`，再读其中链接的 Native18 独立审查、Native19 行为恢复证据和 Native20/21 的 RESULT、SHA256、候选与日志。

交接分支是 `repair/codex-native19-ipa`，但最新二进制已是 Native21。请锁定本文所在提交，不要自动继承分支以后可能新增的状态。Codespace 是 `glowing-train-p7j9gp74q6jwc76v6`，现有验证目录 `/workspaces/GodsPVZ-native19`。建议另建隔离分支/worktree，保留原工作区、生产分支和 Antigravity 分支。

最新 Native21：unlinked SHA256 `5c34c8b20787d1ea56932ab636dede404eb352f6597de7959b93f44cd4546842`；linked SHA256 `0d6bc587c98ff05b733d9d5bced8b96726d2dd5d2ba82cb949c6ca90b8f86e8f`。真实 IL2CPP 转换通过；本批38方法的关联C++错误为零，但21个游戏C++单元仍有6032条诊断、关联676方法。**没有IPA，也未证明所有候选方法与原版行为等价。**

请根据完整证据判断下一批修复，不要把676个方法盲目全改，也不要用强转、空实现、删除缺失语义提示或禁用检查来让编译通过。对象链、值/地址、泛型实例、局部变量、switch、枚举清理、异常和副作用必须有native/type-flow依据。先本地闭合已知编译错误，准备真实可执行打包流程后才启动完整Unity/Apple构建。

工作结束时，按总交接文档第11节交回：精确commit、输入/输出和源码哈希、目标token与native证据、正负测试、非目标隔离、所有编译日志、workflow链接、实际IPA及校验，明确未完成与未验证项目。用户会再交由Codex独立审核；请不要将自己的报告当成独立验收。
