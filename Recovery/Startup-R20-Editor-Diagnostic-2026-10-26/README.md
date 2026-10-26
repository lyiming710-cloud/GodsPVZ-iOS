# R20 真实 Editor 启动诊断

当前 DLL 固定为 d887b370c95c2966dc40d80648d85c286bb42d85004c32d095f64f004e095b37，104处/8方法已先完成构造后通过本批有限运行对照和精确 Linux/Apple 全源码语法检查。新 binder 校验整个 R20 清单、内层 ZIP、目标类型/成员/JIT/隔离、768条正常对照、8个故障控制以及精确1029源码的两平台读回和34-error原始对照补跑。

沿用实际执行过的 R8 受限 Editor 流程：精确 R3/Unity China/iOS模块和软件GL上限、67项包GUID迁移、B001基线、插件GUID/文件名、资源/脚本引用、76项着色器编译和20项受控像素、真实MainMenu PlayMode。激活/归还席位、敏感信息清理、失败保留和所有原断言保持。两个工作副本注入工具仅替换预期DLL哈希。已登记入口 startup-r2-editor-diagnostic.yml 指向本次R20定义；R8历史文件保留。

该流程不会调用iOS导出器/链接器，不自动产出IPA或推进产品资格。整程序集70条失败/不支持、R19剩余完整对照和实际设备测试均继续开放。运行结果以实际回读为准，绿色也不能替代iPad Metal/画面/音效/触控验收。

## 已启动

[真实Editor诊断40224994102](https://github.com/lyiming710-cloud/GodsPVZ-iOS/actions/runs/40224994102) 使用固定提交e3b7e3d和d887b370候选；候选绑定、管线控制和辅助工具编译已通过。此检查点记录时正在还原精确Unity Editor，实际场景/着色器/PlayMode结果尚待回读。没有IPA产物。
