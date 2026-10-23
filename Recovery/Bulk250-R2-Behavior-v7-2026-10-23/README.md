# Bulk250-R2 受控 CLR 执行 v7，2026-10-23

接续 5824260 / v6，仍在 Gemini 额度接管期间由 Codex 执行。本轮游戏候选 DLL 不变，旧归档不改写。

## 实际执行

Windows x64 / .NET 10.0.13 的 local-gate-v7-final.json：335 个用例通过，38 个语义变异体实际执行并被检出，17 项独立进程拒绝控制通过。新增 3 个目标、121 个用例、11 个变异体。

随后 [GitHub Actions 39943347184](https://github.com/lyiming710-cloud/GodsPVZ-iOS/actions/runs/39943347184) 对提交 2612c1aa435a06a04dacd70e6e21af6f5aaf910f 在 Ubuntu 24.04.5 LTS / .NET 10.0.13 上实际复现成功。原始产物在 ci-39943347184/；CI-REVIEW.json 已核对 64 个日志文件的大小/哈希、发布源码哈希、335 个用例、38 个变异体及 17 项拒绝控制。

CROSS-PLATFORM-COMPARISON.json 中，335 个用例行与 38 个变异体观测行的 Windows / Linux 差异均为零；16 个实际执行方法输入、38 个变异体输入的 canonical CIL 哈希也全部一致。这不包括 Bulk388 工具的 Linux 构建，也不扩展为原版 native 语义等价。

| 新目标 | Token | 本轮用例 | 本轮变异体 |
| --- | --- | --- | --- |
| SeedChooserScreen.LoadTeam_Card | 0x060006B6 | 19 | 3 |
| Card_Choose.Choose_Reset | 0x060002F5 | 22 | 4 |
| Grid.CanPlacing | 0x06000297 | 80 | 4 |

50 个目标内：14 个通过受控 CLR fixture，14 个尚未执行，22 个仍声明缺少引擎上下文。2 个辅助 getter 单独计数，共 16 个执行输入身份。PASS 不代表 native 语义等价、正式资格或全程序集通过。

输入仍为 16,573,440 字节、SHA-256 4732b0c1ee64be4224649ee43c88ddd805f5e1ea461e43ecf52febd7a756f903 的固定 candidate1.dll。Cecil 读取真实方法体，逐条保留 opcode、分支、local、finally，再由 DynamicMethod / CLR 执行，不用手写目标方法替换测试输入。

## 覆盖与缺项

- LoadTeam_Card：重置全部卡，重复 ID 的匹配顺序，技能按请求索引读取，短技能列表，null 返回值和异常清理，回调修改列表或替换字段，以及 Team 值类型参数的字段副本。
- Choose_Reset：三次独立颜色读取、精确 alpha、标记与文字显隐、序号递减、重新读取 chosen / seedChooserScreen 字段，以及各阶段异常与 foreach 清理。
- CanPlacing：54 组植物/设备占用层矩阵，地形、水面、承载顺序、受控 destroyed/null 对象、短路、回调重新读取字段及未知占用数值。它的候选 CIL 无 EH，不能为了统一发射器而添加 finally。
- 两个真实跨方法用例将候选 Choose_Reset 动态方法接入候选 LoadTeam_Card / SetPrechooseList 调用链；其他历史用例仍使用原受控 reset 替身。报告明确标注范围，不将两次闭合扩展到全部调用。

Unity GetComponent<Image> / Graphic / Color / Transform / GameObject / TMP_Text、CanPlaceOnWater / CanBear、技能查找与设置、Choose_Being 等仍是可记录副作用的受控依赖；其原版行为为 UNPROVEN。Unity 假空适配器并非引擎原生实现。没有执行原版 native oracle、Unity、IL2CPP、Xcode 或设备运行。

新增三项拒绝控制验证 GetComponent 的泛型返回类型不匹配、Team 参数错误改为 byref，以及无 EH 方法被注入 EH。总共 17 项拒绝控制必须逐项成功拒绝；子进程超时、异常种类不符、报告缺失和变异漏检均不算通过。

## 本轮抓到的测试器缺陷

首轮 normal-v7-first.json 真实失败：受控 GetSkill_ID 使用空合并默认值，把回调主动返回的 null 偷换为新 Skill。L.getSkill.null.result.passed.to.setter 检出该错误；当时只有 13 个目标通过，不能记作 14。

已保存首轮源码到 source-snapshots/first-failed-null-skill/，保留日志及失败 JSON。修正为只在未提供回调时创建默认值；回调存在时原样返回其结果。最终完整门禁重新执行通过，未改候选游戏方法来迎合测试。

## 复现与归档

    gh release download bulk250-r2-candidate-2026-10-22 --repo lyiming710-cloud/GodsPVZ-iOS --pattern candidate1.dll --dir /tmp/bulk250-r2-v7-input
    python3 scripts/actions/test_behavior_bulk250_r2.py --candidate-dll /tmp/bulk250-r2-v7-input/candidate1.dll --output-json /tmp/bulk250-r2-v7-result.json --fixture-project scripts/actions/Bulk250R2BehaviorFixture.csproj

输出路径必须不存在，每个 fixture 子进程限时 30 秒；Cecil、候选、执行二进制和逐项状态均校验。source/ 保留本地执行的真实源码字节，发布源码仅规范文件名与 LF，哈希对应表见 SOURCE-PUBLICATION-MAP.json。原始错误候选不重复存放，但其构造方式及 SHA 保留。

QUEUE.json 列出 14 个未执行目标及 22 个引擎上下文缺项。另见同分支 Recovery/REMAINING-WORK-2026-10-23.md，编译余项 38 个方法是 Bulk350-R6 的历史实测口径，不能与本行为队列相加。
