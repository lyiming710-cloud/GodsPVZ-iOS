# 新增预检查自身的 CS0433 错误

这次由 Codex 新增的预检查写错引用选择造成，不能归咎于游戏修复又坏了。递归收集 Managed/Unity*.dll 扩为 342 个引用，混入根目录旧整体 UnityEngine.dll 与 UnityEditor.dll；真正 Unity 编译的引用使用 Managed/UnityEngine 子目录内的模块和转发程序集。Debug / JsonUtility 因而重复定义。两套辅助脚本编译都被此工具错误拦下，退出 70；许可证激活、Unity 导入、DLL 注入、iOS 导出和 Xcode 均未执行。

此前本地编译采用的是保留的扁平模块集合，没有根目录旧整体 DLL；17 项流程 fixture 也未覆盖真实 Editor 引用选择。它们不能支撑“Linux Editor 预检查已通过”。完整保留这次失败的 rsp / 编译日志 / helper 报告，明确归档为预检查工具缺陷。

修复：从 Unity 自己生成的编译 rsp 提取固定引用目录，去除仅与平台扩展有关且 helper 不使用的 4 项，保留 210 项 API 引用，禁止根目录旧整体 DLL，不再 glob。路径选择回归验证正确选择、忽略额外污染文件、缺必要引用时拒绝；这是路径 fixture，不冒充真实编译。真实 Linux Roslyn 另需执行两套成功编译，并真实加入旧整体 UnityEngine.dll 产生 CS0433，证明反例有效。

完整导出改为仅手动调度，且依赖独立 helper 资格检查。新独立工作流只恢复确切 Editor，不下载 R3 工程，不激活账号，不运行 Unity/Xcode；第一次恢复后只缓存 compiler/runtime/API/licensing client，以后逐文件 SHA-256 核验并复用。只有实际 Linux 检查通过后才启动完整导出，避免拿完整 12 分钟流程试预检查工具。

R4 DLL、389 方法源码、冻结验证器均不变；没有 IPA。真实预检查通过情况必须以新运行实际日志补充，不由本次代码审查推断。
