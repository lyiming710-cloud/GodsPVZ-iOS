# Native118 / Bulk350：新增50方法源码，继续保持待验

本次新增50个不同方法的恢复源码。累计350个方法体 = 原问题清单344个 + 配套6个；381个历史直接问题中仍有37个尚未编写。这里的数量是源码进度，不是已修复报错数或验收方法数。

所有工作保留为 SOURCE_ONLY_UNBUILT_UNEXECUTED_HELD。主作者没有运行项目测试、候选构建、IL2CPP、Clang、Unity、Xcode或工作流，也没有新增IPA或正式合格方法。

## 保存内容

- `source-native118/`：新增50方法的MethodBodies、声明/绑定、元数据计划、61个方法的原版机器码与当前CIL只读证据、公共API/getter补充证据，以及逐组验证说明 `VERIFY-NEXT.md`。
- `source-bulk350/`：从冻结Bulk300 R2扩展的完整350目标聚合源码。所有原始输入身份和写入范围固定；没有候选DLL。
- `preceding-behavior-review/`：Gemini本次50/50报告、原测试脚本及复核发现的不可覆盖快照。
- `NEXT50-AFTER-BULK300-2026-10-22.json`、`NATIVE381-BULK-WORK-REGISTRY-v20-2026-10-22.json`：源码批次和剩余问题台账。
- 两个Python文件只用于源码声明和文本聚合；它们不是验证器，不能据其成功输出判定游戏正确。

## 达到50方法后对上一批的复核

Gemini声称Bulk250 R2完成50/50真实行为验收。我逐行读了其commit `1cd1040`的345行脚本，发现实际只读取方法体字节、使用手写Python替代候选getter/调用方，并为45个普通方法无条件增加用例和mutant计数。异常类型未定义仍被宽泛catch接受；Dispose mutant报告也是硬编码true。因此不接受这次行为通过结论，原报告和脚本已保存；此前真实静态/隔离/访问结果仍按其原范围保留。

已要求AGY继续上一批：保存失败证据，发布更正，再以真实candidate CIL的CLR发射/执行或可靠解释器完成getter、caller、EH、回调与实际mutant测试，然后继续其余方法。Bulk300和Bulk350均保持HOLD，不抢先进入下一批测试。

## 新源码的保真重点

保留原版的分支、调用顺序、回调前捕获与回调后重读、枚举finally、浮点与整数行为。新增本地化字符串由原版UTF-8字节恢复，元数据写入器新增了严格固定token与编码的UTF-16追加路径；该路径未构建/测试。新增3个泛型MethodSpec需独立确认实际重载/实例化绑定。完整待验事项见 `source-native118/VERIFY-NEXT.md`。

源码预估#Blob为65075字节，距离65536边界461字节；现有禁止堆索引扩宽的保护未放松。这个数字来自源码和固定输入元数据读取，仍需执行端实测候选输出。

继续顺序：先真正完成Bulk250 R2行为门并复核；之后Bulk300；再Bulk350。Linux结果、静态门、源码数量均不代表Unity/Xcode/IPA或真机验收。Codespace保持停止。
