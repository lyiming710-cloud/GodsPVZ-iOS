from pathlib import Path
import json,base64,hashlib,subprocess,shutil
review=Path(__file__).resolve().parent
repo=review.parent/'native19-work'
git='C:/Users/86136/.cache/codex-runtimes/codex-primary-runtime/dependencies/native/git/cmd/git.exe'
header,encoded=(review/'checkpoints-transport.txt').read_bytes().split(b'\n',1)
metadata=json.loads(header);data=base64.b64decode(encoded,validate=True)
assert len(data)==metadata['bytes'] and hashlib.sha256(data).hexdigest()==metadata['sha256']
bundle=review/'native23-checkpoints.bundle';bundle.write_bytes(data)
subprocess.run([git,'-C',str(repo),'bundle','verify',str(bundle)],check=True)
subprocess.run([git,'-C',str(repo),'fetch',str(bundle),'HEAD'],check=True)
work=review.parent/'native23-publication'
assert not work.exists(),str(work)
subprocess.run([git,'-C',str(repo),'worktree','add','-b','docs/native23-review-2026-09-30',str(work),metadata['head']],check=True)
dest=work/'Recovery/Native23-Review-2026-09-30';dest.mkdir(parents=True)
for name in ['REVIEW.md','full-review-evidence.zip','DOWNLOAD-VERIFIED.json','native-independent.json','native-helpers-independent.json','evidence-tool-probes.json']:
 shutil.copy2(review/name,dest/name)
selected=['status.json','analysis.json','independent-isolation.json','final-checks.json','attribution-comparison.json','diagnostics.json','governance.json','runtime-fixture-results.json','patcher-swap-probe.json','wrong-shape-patch.log','typed-method-results.json','baseline-clang-results.json','batch1-clang-results.json','batch2-clang-results.json','batch2a-clang-results.json','baseline-clang-flags.json','batch1-clang-flags.json','batch2-clang-flags.json','batch2a-clang-flags.json','baseline-conversion-command.json','batch1-conversion-command.json','batch2-conversion-command.json','batch2a-conversion-command.json']
for name in selected:
 pp=dest/'evidence/review'/name;pp.parent.mkdir(parents=True,exist_ok=True);shutil.copy2(review/'evidence/review'/name,pp)
for name in ['Inspector.cs','RuntimeFixture.cs','remote_run.py','inventory_remote.py','start_full_remote.py','start_inspector_remote.py','start_runtime_remote.py','audit_analysis_remote.py','check_native.py','check_native_helper.py','check_tools_exact.py','diagnostics_remote.py','final_checks_remote.py','compare_attribution_remote.py','archive_remote.py','unpack_evidence.py']:
 target=dest/'reproduce'/name;target.parent.mkdir(exist_ok=True);shutil.copy2(review/name,target)
text=(dest/'REVIEW.md').read_text(encoding='utf-8')
text=text.replace('日期：2026-09-30。审查人：Codex。','> 此报告保留推送前的审查快照。“GitHub 尚未发布”“没有提交或推送”描述审查执行当时的状态；本次仓库更新已发布两个 Native23 检查点与审查资料。候选验收结论不变。\n\n日期：2026-09-30。审查人：Codex。',1)
(dest/'REVIEW.md').write_text(text,encoding='utf-8')
(dest/'README.md').write_text('''# Native23 独立审查与最新进度（2026-09-30）

**尚未导出 IPA。Native23 有真实编译改善，但 Batch2 / Batch2a 未通过高还原验收，全量 C++ 仍失败。** 本次提交只发布检查点、审查资料和进度入口，没有晋级生产候选、改工作流或触发 Actions。

## 从这里阅读

- [全面独立评估](REVIEW.md)：实际候选、隔离性、编译、native、行为测试、七项发现与下一步。
- [完整证据包](full-review-evidence.zip)：28,851,145 bytes；SHA256 `b7565f28d68a91620d42f9b6d154d7943beefe9a5932ece290eadf93fe994aed`。1260 个成员，1259 个文件按 manifest 哈希核对通过。
- [下载核对记录](DOWNLOAD-VERIFIED.json)、[本目录文件哈希](SHA256.json)。
- [四组转换和编译结果](evidence/review/status.json)、[实际 DLL 隔离检查](evidence/review/independent-isolation.json)。
- [污染反例和 225 个实际站点](evidence/review/analysis.json)、[纠正后的方法错误归属](evidence/review/final-checks.json)。
- [24 场景实际 CIL 运行与两个行为负例](evidence/review/runtime-fixture-results.json)。
- [审查复现工具](reproduce/README.md)。
- [上一版完整项目交接](../HANDOFF-2026-09-29-Native21.md)。

## 发布内容与谱系

恢复分支：`repair/codex-native19-ipa`。本次发布保留 `09dd564962f6b83b3175194db97d95cb7e5346b9 -> 232273a851538a73eaa9c6837efd091f063ebd62 -> f1a96517d21826f0de9efb579b481dee4a9863b0`，然后追加本目录及进度入口记录。前两个 Native23 提交来自原 Codespace；审查中没有重写它们。

原 PC native 仍是主要语义权威。Native21 linked `0d6bc587c98ff05b733d9d5bced8b96726d2dd5d2ba82cb949c6ca90b8f86e8f` 是本轮比较输入，不等于全游戏已验收。原 handoff、原 native 和此前证据继续保留。

| 候选 | SHA256 | 方法 / 编辑 |
|---|---|---:|
| Batch1 | `4d970f5ed7e63dd42815c3c843ce51918328ff790fa5d1eb77e2a80e6f3b0a1b` | 3 / 6 |
| Batch2 | `1d8e743df3407b3769f50885a55556ae380525595b4284ad223795af0aa22e5d` | 377 / 1623 |
| Batch2a | `6e8caffc6188d1f93a72d999f955056836c6cc263e21cda8cad49222191d3c79` | 249 / 865 |

候选 DLL 在证据包中，属于实验审查资料；未覆盖项目运行时输入。

## 实测状态

三个候选各从源码重建两次，六次 DLL 哈希全部复现。四组各实际重跑 IL2CPP，264 个 C++ 文件/组，共 1056 次 Clang 预检；转换均 exit=0。

| 输入 | 失败 TU / 264 | .cpp 错误 | 头文件错误 | 总错误 | 含 .cpp 错误的方法 |
|---|---:|---:|---:|---:|---:|
| Native21 baseline | 24 | 6006 | 64 | 6070 | 676 |
| Batch1 | 24 | 6001 | 64 | 6065 | 673 |
| Batch2 | 23 | 4368 | 64 | 4432 | 612 |
| Batch2a | 23 | 5136 | 64 | 5200 | 651 |

这是 Codespace / Ubuntu Clang 18 的语法与类型预检，不是 Apple 编译、链接、Unity player 或真机验收。Batch1 六个具体站点有原 native 支持；Plant.Update 的八个片段独立证明属于同一 unwind root。两个完整候选 CIL 的有限运行测试为 24/24，两个行为 mutant 被识别。它们不构成全游戏行为等价。

## 必须解决的门禁问题

1. provenance 的四个污染反例全被误接受；865 个接受站点中 225 个涉及未经证明的 LOCAL receiver，跨 97 个方法。需要修正持久化来源、调用污染和合流处理后重新分类。
2. 方法归属工具把声明当定义并覆盖真实错误，漏报 68/68/61 个方法。正确数据见上表。
3. Batch1 有 18 个非目标方法 raw IL 字节变化，解析后逻辑相同；严格“字节 100% 不变”尚未满足。
4. 补丁器缺旧指令形状检查、switch/EH 重定向；本次实际候选未发现悬空目标，但通用约束不可靠。
5. E1=377、E4=362、E5=3 不能按报告的定义验收；整体类型 FAIL->PASS 实际为 Batch2 11 个、Batch2a 5 个。
6. 通用 native 提取器可能跨方法，解释器也未实现字段存储，不能把路径探索当真实运行覆盖。

审查结束时，原共享 canary `ManagedStripped/GodsPVZRuntime1.dll` 仍为 Batch2 `1d8e743d...`，**不是 Batch2a**。本次发布不修改该文件；任何后续执行必须在读取点校验候选 SHA。Git clean 不代表忽略目录中的构建输入正确。

下一步先修正验证工具、重算站点、证明方法语义和序列化隔离。完整预检通过后才进入 Apple/Xcode/unsigned IPA 流程。没有新增 Actions 运行或 IPA 资产。
''',encoding='utf-8')
(dest/'reproduce/README.md').write_text('''# 审查工具与复现说明

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

完整 .cpp、失败日志和 DLL 阅读数据可能较大；可在完整证据包查找。仓库展开保存重点结果，避免重复提交所有大型导出 JSON。执行这些工具不等于验收游戏或 IPA。
''',encoding='utf-8')
entry='''> **2026-09-30 最新进度：Native23 全面独立审查已完成，先读 [审查与当前状态](Native23-Review-2026-09-30/README.md)。有真实编译改善，但 Batch2a 全量预检仍有 5200 条错误、23 个失败 TU，没有 IPA。Batch1 六个站点可保留为受限候选；Batch2/2a 未通过高还原验收。来源追踪、方法归属、字节隔离和补丁器约束需先纠正。此前 Native21 交接继续保留为项目历史与输入依据。**\n\n'''
for name in ('Recovery/HANDOFF_CURRENT.md','Recovery/STATUS.md'):
 pp=work/name;s=pp.read_text(encoding='utf-8');parts=s.split('\n',1);pp.write_text(parts[0]+'\n\n'+entry+parts[1].lstrip('\n'),encoding='utf-8')
root=work/'README.md';s=root.read_text(encoding='utf-8');parts=s.split('\n',1)
intro='''## Latest recovery checkpoint — 2026-09-30

Read [Native23 independent review and current progress](Recovery/Native23-Review-2026-09-30/README.md) first. The unsigned IPA has **not** been exported. Native23 Batch2a has reproducible compilation improvement, but full preflight still fails with **5200 diagnostics / 23 failed translation units**. Provenance, method attribution and strict byte-isolation findings remain open. Experimental DLLs are evidence, not a promoted production baseline. The older Stage9.1 sections below are historical context.

'''
root.write_text(parts[0]+'\n\n'+intro+parts[1].lstrip('\n'),encoding='utf-8')
manifest={str(f.relative_to(dest)).replace('\\','/'):{'bytes':f.stat().st_size,'sha256':hashlib.sha256(f.read_bytes()).hexdigest()} for f in sorted(dest.rglob('*')) if f.is_file()}
(dest/'SHA256.json').write_text(json.dumps(manifest,ensure_ascii=False,indent=2),encoding='utf-8')
print(json.dumps({'worktree':str(work),'parent':metadata['head'],'files':len(manifest)+1,'bytes':sum(x['bytes'] for x in manifest.values())},indent=2))
