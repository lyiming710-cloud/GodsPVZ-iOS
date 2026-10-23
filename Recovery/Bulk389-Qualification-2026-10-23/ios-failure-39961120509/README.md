# 真实 Unity 失败后的扩大检查

39961120509 已失败，无 IPA。Unity 本身重新导入退出 0，但首次 Bee 编译响应文件仍引用被工作流删除的迁移脚本，产生一次 CS2001；下一轮内部重新编译成功并未抹除这条错误，严格日志门正确拦截。未将 grep 放宽以掩盖错误。

另发现许可证归还步骤假绿：--return-ulf 退出 28，ULF 文件不存在。Personal 激活实际使用 entitlement，不能据绿色步骤宣称席位已归还。本轮历史席位归还状态保留 NOT_CONFIRMED。

修复限于一次性重建工程和构建脚本，R4 候选仍为 ef1992326d1ee494a389bc2c78bdfab131721434c0b08ab5f41dbc3e506c4e48。完成 67/67 迁移且 B001、原迁移脚本、候选哈希均正确后，原路径写入无自动初始化/无 DLL 改写功能的迁移脚本占位类，保留 .meta GUID，清理仅 Library/Bee 与 Library/ScriptAssemblies；保留导入的纹理等资产缓存。候选注入后及导出后再次核对 SHA。

本地用保留的真实 Unity/Mono API 引用编译两套 helper：原迁移状态、惰性占位状态均 exit 0；删除源文件反例 exit 1 且重现 CS2001。该本地 Roslyn 不是 Linux Editor 自带编译器；新 CI 在获取许可证前，使用还原 Editor 自带 Roslyn、runtime、4.8 API 编译两状态，若失败就先停。

17 项流程回归检查通过。覆盖迁移标记/父与候选/原脚本哈希、保留路径/GUID/资产缓存、导出绑定及错误候选/工作流/C++ 改写的拒绝；实际 IPA Python 校验代码在合成 Mach-O/ZIP 上验证合法样本和 x86_64、模拟器平台、签名、错误文件角色、不同 metadata、嵌套签名反例。Apple ditto 在这些 fixture 中是明确的测试替身，未执行 Xcode，也没有生成可交付游戏 IPA。

Xcode 16.4 / iphoneos18.5 / ditto 先由独立轻量 Mac job 验证。8192 纹理软件 GL 检查也提前。新导出收据绑定本轮代码、候选、游戏 C++、PBX 和 metadata；Mac 编译前、打包前校验，包内 metadata 与本次导出逐字节比较。签名检查覆盖整个包。许可证 CLI --help 必须有 --deactivate-all，清理使用当前机器的订阅停用命令并严格检查退出码。命令支持与实际停用仍由下一次 CI 观测，不冒充已执行。

完整失败日志、最终 job 状态、本地编译源码/响应文件/引用文件哈希、反例结果和 actionlint 结果均在本目录。actionlint 未调用 shellcheck；YAML 和所有内嵌 Python 已解析。原 R3 资源大小写冲突及 headless shader 警告保留为资源/视觉验收风险。另有本地历史 Native18 Xcode 归档截断，仅为不使用的参考文件，不混入新导出。

这轮检查证明流程边界与拒绝规则，不能替代 Unity/Xcode、389 方法完整语义或真机验收。

成功导出的 Xcode 归档现在即使随后清理步骤失败也会上传，避免丢失可复用的长流程产物。本机 Hub licensing client 1.18.3 的 --help 已保存，仅证明本机选项；还原 CI Editor 的客户端会在激活前单独验证。第一次本地编译日志读取受 Windows GBK 解码影响而中止，该次未计通过；已在独立 helper-api-check-r2 目录用 UTF-8 重做，正式证据为本目录 helper-api-check。
