# 审查工具与复现说明

这里是审查新增工具，不是被批准的游戏补丁。被审查的原 Native23 工具在仓库 scripts/codespaces/native23_*.py 与 RepairNative23 中；审查时源码快照也在证据包 original-sources 中。

- Inspector.cs：独立读取 metadata、方法体与 raw IL，另反射测试原补丁器 Swap。
- RuntimeFixture.cs：读取候选完整 CIL，绑定明确辅助函数替身，实际执行 24 场景及两个行为 mutant。
- start_full_remote.py：原环境的完整重建 / 四组 IL2CPP / 1056 TU Clang driver。固定输出到独立 .validation/native23-independent-full-review，不写共享 canary。
- audit_analysis_remote.py：实际源码四个污染反例、接受站点 receiver 追踪、实际 DLL 导出后的类型门及单项回退。
- final_checks_remote.py：从真实 CodeGen 指针表和 CPP 函数定义归属诊断。
- check_native.py / check_native_helper.py：Windows 上原 PC unwind 与 capstone 核对。
- remote_run.py：Windows gh codespace SSH，Python stdin 传输，避免旧 base64 命令长度截断。

脚本保存了本次环境路径，不是全新环境的一键安装器。需要既有 Unity China IL2CPP canary、原 PC 输入、归档 Unity headers、Cecil、dotnet 和 Clang；源码参考路径及完整运行命令在证据包 review 中。不要在缺失路径或不同输入下跳过 SHA / 指针表数 / TU 数断言。

start_inspector_remote.py 和 start_runtime_remote.py 含显式 source 占位符；先用相应 .cs 内容替换再通过 remote_run.py 发往远端。证据包还保留当时已填充的脚本/driver。check_tools_exact.py 从已解压的 evidence/original-sources 读取被审查源码。

证据包保存四组生成的游戏 .cpp、全部 264 个 TU/组的预检日志、每个 TU 的源码哈希和 DLL 阅读数据。其他 Unity 生成源码可按锁定输入和归档命令重新生成。仓库展开保存重点结果，避免重复提交所有大型导出 JSON。执行这些工具不等于验收游戏或 IPA。
