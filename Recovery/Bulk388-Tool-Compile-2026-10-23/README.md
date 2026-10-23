# Bulk388 工具首次编译与单处 API 修正，2026-10-23

本检查点推进工具编译，尚未构造 Bulk388 游戏候选，也未重新统计 C++ 错误。

先对 Native120 / Bulk388 源码包的 54 项输入重新计算字节大小和 SHA-256，全部匹配 SOURCE-MANIFEST.input.json，再复制到独立目录。首次真实构建失败，错误 CS1061：N120.MethodBodies.cs 使用 IL.Token，而项目 IL 生成器已有的接口为 Ref(byte opcode, int token)。

只修改该文件的一处调用：

    .Token(0x72, 0x70001E24)
    .Ref(0x72, 0x70001E24)

原失败源码 N120.original.cs.txt、精确补丁 N120-REF-API-FIX.patch、失败与成功的 argv/退出码/完整流和输入哈希均已保存。其余 53 项输入不变，历史 Native120 源码包和 SOURCE-MANIFEST 未覆盖。source/ 内的 CHECKPOINT-STATUS / README 等为原输入快照；本轮实际状态以 CURRENT-RESULT.json 为准。原输入清单绑定修正之前的字节，本归档 MANIFEST.json 绑定修正后的所有文件。

修正后的整个 Bulk388.csproj 已在 .NET 10 上编译成功，0 警告、0 错误。生成的是补丁工具 Bulk388.dll，SHA-256：

    89f520fa676dcc8fc7d0c3a2abbfe62b859a3d0fb2adefbd7b707d3e68c89944

这不是 Assembly-CSharp 游戏候选。

额外调用编译工具的 N120_SetSortingGroup 源码发射函数，得到 81 字节 CIL，使用独立指令解码和 System.Reflection.Metadata 读取已哈希锁定的 Native93，确认唯一 ldstr 为 0x70001E24，原字面量为 Entity。错误 token 和 ldstr 改为整数的两个错误变体均被拒绝。EMITTER-LITERAL-CHECK.json 仅证明源发射器的字面量绑定，不执行游戏方法、不证明排序行为或 native 等价。

尚待完成：跨 64 KiB 的 metadata 列宽门槛及负控制、双次构造、独立读取和字节隔离、扩展类型/访问门、实际候选行为测试，再进行真实 IL2CPP / 全游戏 TU 编译。不能跳过这些门把工具编译通过说成 38 个报错方法已解决。

HISTORICAL-38-METHOD-COMPILER-ROSTER.json 原样保存 39790796339 的归档结果：37 个直接报错方法与 12 个头文件归属方法去重后为 38 个；1,592 条直接诊断加 24 条头文件诊断为 1,616；14 / 16 个游戏 TU 失败。新增头文件单独报错的 0x060003F9 已有本轮修正接口的源码方案，尚未进入新游戏候选验收。
