# 实际 Unity 导出成功后的 Apple 泛型编译错误

39988665189 的辅助检查、67/67 迁移、R4 注入、重新导入、真实 Unity iOS 导出及导出绑定均成功；停用命令退出 0。本轮 Xcode 工程已保存，不应再为检查这批错误重跑 Unity。

Apple 编译首先在 Generics__63.cpp 发现游戏相关 SwfList<T> 的指针/整数比较、指针窄化、object 数组下标及 Dictionary 实参类型错误。日志有 19 个去重源码诊断站点，编译器达到 20 条上限后停止；异常回显重复了一遍，不能算两批独立错误，更不能说总共只有 19/20 个错误。

根本漏项：旧 Linux 门筛选 GodsPVZRuntime1*.cpp，仅编译 14 个前缀文件；泛型定义/实例化落在 Generics__*.cpp。此前将其余 742 个未检查文件统称“系统 CPP”不准确，它们也包含游戏方法。14/14 只能证明这 14 个文件，不能推出整个程序集可编译。

还有单独的 CLEAN FAILED：工作流手工创建了 DerivedData 外的 Release-iphoneos 目录，Xcode 拒绝删除。产品目录改到 DerivedData/Build/Products/Release-iphoneos，移除手工 mkdir。独立工作流先用实际导出的 Xcode 工程执行 clean（不 build）核实该调整。

新检查复用此轮 SHA-256 固定的 350 MB 导出，使用原失败 Cmdline 的 Apple ARM64/iOS SDK 编译参数，去掉目标文件/优化/调试生成参数，加入 fsyntax-only 与 ferror-limit=0。先执行真实编译器正负控制，并要求重现 Generics__63.cpp 的真实错误，再对导出目录所有 .cpp 无文件名前缀筛选地执行检查；每个文件失败后继续，保存全量日志/文件哈希/退出码/错误归属，最终严格非零。方法注释仅供定位，不等于 MethodDef/native 语义证据。

此门不启动 Unity、IL2CPP 重新转换、Xcode 完整编译或 IPA 打包。生成的 .c、native runtime、ObjC 和链接仍是单独范围，不会包装成全项目通过。managed 修复必须回到真实 DLL 与原版证据，不能靠修改生成 C++ 或全局替换零/空值消除编译错误。
