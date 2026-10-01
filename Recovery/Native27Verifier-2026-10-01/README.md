# Native27 验收器加固与假通过复现

2026-10-01。修复验证工具，游戏DLL保持Native27，SHA-256 ae6ac972e0615666091b7359774f0c0ec70d01d8e5d05d0942324b7d13b34920。本轮没有新的游戏方法修复，没有IL2CPP/Clang/Unity/Xcode重跑或IPA。整包状态仍为Linux6033条诊断、24个失败TU、659个报错方法。

## 为什么先做这一步

旧fixture从候选IL读取业务调用名称，再构造预期轨迹；改变候选调用时，答案也会跟着变化。负控制还将任意测试器异常记作detected=true。这些是验收工具真实漏洞，不能靠增加测试数量消除。

我们在只读Cecil对象中将四个方法的真实同签名调用GotoAndPlay(int)与GotoAndStop(int)对换：0x060008B0、8B2、8B4、8B8。旧fixture四例全部接受，新fixture四例全部按原版native固定契约拒绝。没有写回游戏DLL。证明的是旧fixture单独存在假通过，不代表已发布Native27候选包含这些错误；当时native调用核对和24-byte修改边界仍限制了实际候选。

## 实际修改

十二个方法的完整签名、参数类型、预期业务调用、返回Wait类型、计数字段和PC调用地址固定在独立契约表；测试期望不再从候选调用名生成。表与既有NATIVE-REVIEW逐方法核对。Getter Count目标0x18065BAA0；Play(string) 0x1803C68A0；GotoAndPlay(int) 0x1803C60D0；GotoAndPlay(string,int) 0x1803C6030；GotoAndStop(int) 0x1803C6240。

异常分成三类：BEHAVIOR_MISMATCH（断言不符）、CLR_REJECTED（明确InvalidProgramException或VerificationException）、TOOL_ERROR（发射、反射、测试器等其他故障）。TOOL_ERROR的Detected=false、gate_pass=false、进程exit2；不能混入成功检出数。正控制的工具故障同样失败。每次编排同时检查进程退出码与结构化报告。

## 验证结果

- 当前候选174个正用例全部通过。
- 72个负面变体：70个行为不匹配、2个明确的空栈pop非法IL被CLR拒绝；全部检出。
- 四个旧fixture假通过复现成功，新fixture按固定契约拒绝，均不是CLR拒绝。
- 发射器故障、调用器故障两次注入，均exit2、gate_pass=false、TOOL_ERROR，均未记作已检出变体。
- 首次70变体运行保留；增加两例非法IL后才重跑，没有删除原观测。
- 原候选SHA在运行前后相同。源代码、报告、日志、旧fixture和候选都进入完整归档。

纠正此前表述：旧报告没有逐例记录CLR拒绝类型，不能据其断言“部分非法返回变体由CLR拒绝”。本次原有变体加四个新业务变体全部实测为行为不匹配；只有新加入的两个空栈pop明确归类CLR拒绝。后续只按逐例实测分类，不以异常泛称代替证据。

## 证明边界与下一步

执行的是候选外层CIL，controller、列表和Wait仍使用明确的helper doubles。固定契约来自原版caller审计，但未执行原版helper内部或UnityPlayer，也不是任意IL修复的完备证明。native身份/映射本身也必须持续审计，不能让工具打印OK替代原版行为依据。

先将这个更严格的模式扩展到新批次，再逐方法核对十八个待审方法（对应45条诊断），并补入问题集中的数值/object、结构体传值或泛型方法，按100–200条已定位诊断组织批次。全部完成轻量证据门后再做每批一次full IL2CPP/Clang，不凑数放行证据缺失的方法。资格闭环后才持证Unity/iOS和真实Mac/Xcode，最终目标仍为未签名IPA。

## 可复现归档

完整ZIP包含当前候选、旧fixture二进制及Cecil、旧fixture源码、固定native契约记录、新验证器与portable replay脚本、完整逐例报告和全部日志。原版PC全文及Native27编译证据仍在固定父Release，地址和SHA在EVIDENCE-ARCHIVE/MANIFEST中。

恢复ZIP到新目录后，执行python3 tools/replay_native27_verifier.py <恢复目录> <新的输出目录>。需要dotnet10；脚本核对MANIFEST每个成员，普通复制源码到新输出目录重新构建，重现174正用例、72变体及两次工具故障，保留输入不变。它不是游戏构建脚本。

已实际恢复ZIP并从恢复源码重新构建复现全部控制，见ARCHIVE-REPLAY。
