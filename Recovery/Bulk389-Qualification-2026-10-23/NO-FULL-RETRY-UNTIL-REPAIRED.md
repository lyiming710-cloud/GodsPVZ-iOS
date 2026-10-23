# 接下来使用独立检查，不用完整流程探错

当前冻结 R4 的真实 Unity 导出已经保存，归档 SHA-256 为 `93d32bb83a6d93b1c9d8a490bf057db7cfe2e35dad1219ab4924b6ec38538e04`。独立 Apple 检查完成 259 CPP + 66 C，覆盖所有 325 个生成源文件；3 个泛型 CPP 仍有 38 条诊断。不能因为辅助脚本、转换或普通分片通过就重新启动这个已知失败的候选。

两个独立入口均可手动调度：

- `bulk389-reuse-export-all-cpp.yml`：固定导出，所有生成 CPP，错误上限 0、失败继续，保留全量日志与失败源文件。
- `bulk389-reuse-export-generated-c.yml`：固定导出，所有 66 个生成 metadata C，显式 C11 前端。

它们不启动 Unity，不重新执行 IL2CPP，不运行完整 Xcode build 或打包；真实检查步骤分别为 66 秒、6 秒，不含排队下载。

完整流程已加入精确哈希的已知失败拦截。此 R4 如果再次手动调度，会在 checkout 后立刻报 `FULL_PIPELINE_BLOCKED_KNOWN_CPP_ERRORS`，后续 Editor 恢复、授权、Unity 导出全部跳过。已用真实候选 ZIP 执行过该拦截并保存退出结果。这只是已知失败缓存，不是新候选的资格验证。

下一步先把生成方法符号回映射到精确 CLR 定义与原版 native。名字族对应的 12 个签名候选已列在 `all-generated-cpp-39998642128/GENERIC-METHOD-NAME-CANDIDATES.json`，其中有重载，不能把它们全部当成已确认修复目标。按证据闭环修复 managed body，保留隔离性并重新执行快速 IL2CPP/全生成代码编译检查。

改了 DLL 就必须重新生成对应代码；旧导出只能证明旧候选，不能靠修改生成 C++ 或把旧检查结果套给新 DLL。新候选的独立门通过后，再进行最终 Unity 集成、实际 Apple 编译/链接和未签名 IPA 打包。当前没有 IPA，游戏语义与设备验收均未完成。
