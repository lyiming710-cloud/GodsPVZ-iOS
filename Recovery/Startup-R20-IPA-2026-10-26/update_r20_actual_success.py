"""Refresh current status; retain pre-success documents and rejected build evidence."""
from pathlib import Path
import json
import shutil
from archive_support import js, manifest

W = Path(__file__).resolve().parent
R = W.parents[1] / 'audit/repo-native113'
D = R / 'Recovery/Startup-R20-IPA-2026-10-26'
T = R / 'Recovery/Startup-R20-Sites104-2026-10-26'
F = R / 'Recovery/Startup-R20-IPA-Build-2026-10-26'
A = R / 'Recovery/Startup-R20-Asset-Case-2026-10-26'
N = R / 'Recovery/Startup-R21-Source-Queue-2026-10-26'
BASE = 'https://github.com/lyiming710-cloud/GodsPVZ-iOS'
RUN = BASE + '/actions/runs/40263428409'
REL = BASE + '/releases/tag/startup-r20-ipad-test-2026-10-26'
IPA = dict(file='GodsPVZ_1.0.2_startup_r20_unsigned.ipa', bytes=192091143, sha256='f7334b1b330cdc7dc2cbeeb0c9851c8c233bc0fc923c69e6de36f38aeda56c75', unsigned=True, architecture='arm64', platform='physical_iOS', release_url=REL)
NEW = json.loads((D / 'PROGRESS.json').read_bytes())
for root in [T, F, A]:
    for name in ['README.md', 'PROGRESS.json']:
        saved = root / ('PRE-IPA-SUCCESS-' + name)
        assert not saved.exists(), saved
        shutil.copyfile(root / name, saved)

f = dict(NEW)
f.update(unity_export='PASS_ACTUAL_BUILDREPORT_SUCCEEDED_ZERO_ERRORS', actual_export_apple_compilation='PASS_258_CPP_66_C_SOURCE_AND_RAW_LOG_READBACK', xcode_link='BUILD_SUCCEEDED', ipa=IPA, ipad_acceptance=False,
         current_real_build_run=40263428409, current_real_build_source=NEW['source_commit'], current_real_build_status='COMPLETED_SUCCESS', current_real_build_url=RUN,
         previous_failed_exports=[40255311794, 40259026509], previous_failure_category='SIX_CASE_COLLISION_PAIRS_REPORTED_FOUR_ERRORS', source_case_preflight=40262106436,
         full_evidence='../'+D.name)
js(F / 'PROGRESS.json', f)
(F / 'README.md').write_bytes(f'''# R20 未签名 IPA 实际构建成功

[完整工作流 40263428409]({RUN}) 的 Unity 导出、全部实际导出源码 Apple 检查、Xcode 编译链接和未签名打包均成功。固定输入仍是 R20 d887b370…095b37，固定工作流提交 c22560ed9e025f04d4e58bc38ba0f23df8b9feb4。

[下载测试 IPA]({REL})。192,091,143 bytes，SHA-256 `{IPA['sha256']}`；arm64 / physical iOS / unsigned。实际 BuildReport 为 Succeeded、0 errors、1,364 warnings；258 C++ / 66 C 全部原始 Apple 日志核对为 0 errors、3,584 warnings；独立 xcodebuild 为 BUILD SUCCEEDED。包内 metadata 与本次真实导出完全相同。

此前 40255311794 和 40259026509 的失败继续保留：同一组资源大小写冲突被四次报告为 Error，原 BuildIOS 错误门正常拒绝。对 12 份资源及 12 份 meta 只移动路径，保留原 basename、内容和 GUID；全部 6,842 文件 hash 保留，实际 Unity 中 12 份资源的 GUID/路径/名称核验通过。没有删除原始资源、忽略 Error 或改变游戏 DLL 来换取构建成功。

真实 Editor MainMenu 1,862 帧/0 错误，148 脚本绑定及 76 shader stage/20 受控软件 OpenGL 像素通过。R19 仍是 81 个有限 profile、19 个待验收，R21 的 16 个提案尚未构造且不包含在此包。M5 iPad 的主菜单、闪烁、实际声音、Board 及整个游戏行为仍待真机与后续验收。

[完整字节回读、原始日志、源文件和 Release 资产清单](../{D.name}/README.md)。本目录 PRE-IPA-SUCCESS 文件保留构建前状态；历史检查结论不覆盖为成功。
'''.encode())

a = json.loads((A / 'PROGRESS.json').read_bytes())
a.update(status='CASE_PATH_FIX_PASSED_ACTUAL_UNITY_NAMES_AND_FULL_UNSIGNED_IPA_BUILD',
    previous_failed_replay=a.pop('actual_replay'), previous_failed_raw_report_sha256=a.pop('actual_raw_report_sha256'),
    previous_failed_report_errors=a['build_report_error_count'], build_report_error_count=0,
    previous_failed_source_archive_sha256=a['source_archive_sha256'], source_archive_sha256='c6cba5a27d9ce2151366a5f0db56ef1cf12c6107f8b171c46e301893bc8bfbe2',
    actual_apple_compile='PASS_324_UNITS_AND_XCODE_BUILD', unity_after_case_repair='PASS_ZERO_ERRORS_12_GUID_PATH_NAME_CHECKS',
    ipa=IPA, current_real_build_status='COMPLETED_SUCCESS', current_readback='../'+D.name,
    actual_unity_warnings=1364, actual_apple_warnings=3584, ipad_acceptance=False)
js(A / 'PROGRESS.json', a)
(A / 'README.md').write_bytes(f'''# 六组资源大小写冲突：保留原资源，真实导入和 IPA 构建通过

[实际工作流 40263428409]({RUN}) 已验证修复：Unity 导出 0 errors，12 份资源的 GUID→新路径→LoadMainAsset.name 均符合原始名称；全部实际 324 个 C/C++ 和后续 Xcode 构建通过，并生成未签名 IPA。[成功证据及下载](../{D.name}/README.md)。

原版 R3 内 AnimationClip 的 Enter/enter、Exit/exit、Idle/idle，Sprite 的 Background/background、Background_0/background_0，以及 Texture2D 的 Background.png/background.png，共六组大小写路径冲突。40259026509 的真实 BuildReport 分四条 Error 重复报告这些路径，原错误门拒绝，Mac 跳过；全部失败证据仍保留。本次没有将错误降级或忽略。

精确源检查 40262106436 核对全部 6,842 个 Assets 文件。12 份资源和 12 份 meta 各移入其 GUID 命名子文件夹，原 basename、原始字节/hash、GUID 保持；其他 6,818 份文件路径和字节不变。大小写冲突归零。旧路径引用、特殊 Resources/StreamingAssets 路径、丢 meta/重复 GUID 会阻止操作；四个错误计划在第一次移动前被拒绝。第一次源码预检查因 epoch 日期无法写传统 ZIP 而停止的证据仍保留，修复仅钳制 ZIP 元数据日期。

冻结计划和原始导入名称表在本目录。路径修复在 R3 解开后、第一次 Unity 导入前执行。GodsPVZAssetCaseAudit 真实读取 Unity GUIDToAssetPath、AssetPathToGUID、LoadMainAsset.name；Sprite 的内部名称和 PNG 默认 Texture2D 名称均按原版检查。游戏候选 SHA-256 保持 d887b370…095b37。

失败导出的完整源归档 SHA-256 53b73734f7edc2ed64134c86e7b7ee584aebea12a33ace2f100876d47ee7e80e，继续保存在旧 Release 的 Startup-R20-UNQUALIFIED-40259026509-generated-and-report.zip；新成功导出归档 SHA-256 c6cba5a27d9ce2151366a5f0db56ef1cf12c6107f8b171c46e301893bc8bfbe2，独立保存。每次 329 个原始文件均已字节回读，不能互相借用结论。PRE-IPA-SUCCESS 文件保留本目录先前状态。

这是既有 R20 超过 100 位置批次的资源/导出修复，不把 24 个文件路径计作新游戏语义修复。新 IPA 尚无 iPad Pro M5 上的启动、持续画面、声音或 Board 结果，不能据构建成功声称黑屏已经解决。
'''.encode())

t = json.loads((T / 'PROGRESS.json').read_bytes())
t.update(status='R20_FINITE_PROFILES_PORTABLE_CHECKS_ACTUAL_UNITY_XCODE_UNSIGNED_IPA_PASS_DEVICE_PENDING',
    historical_editor_mainmenu_frames=t['editor_mainmenu_frames'], editor_mainmenu_frames=1862,
    historical_failed_export_runs=[40255311794,40259026509], real_ios_export='PASS_ZERO_ERRORS_RUN_40263428409',
    real_ios_summary_result='Succeeded', real_ios_summary_errors=0, real_ios_export_blocker='RESOLVED_SIX_CASE_PAIRS_WITH_ASSET_BYTES_AND_GUIDS_PRESERVED',
    actual_export_apple='PASS_258_CPP_66_C_ZERO_ERRORS', actual_export_apple_warnings=3584,
    unity='PASS_ACTUAL_EXPORT_ZERO_ERRORS_AND_MAINMENU_1862_FRAMES_RUN_40263428409', xcode='BUILD_SUCCEEDED_RUN_40263428409', ipa=IPA,
    actual_build_source=NEW['source_commit'], actual_build_readback='../'+D.name,
    inherited_r19_known_difference_methods=4, next_source_proposals=16, next_constructed_positions=0,
    qualified_methods=0, whole_game_accepted=False, ipad_acceptance=False)
js(T / 'PROGRESS.json', t)
(T / 'README.md').write_bytes(f'''# R20：104 个位置、8 个方法；未签名 IPA 已构建

当前候选 SHA-256：`{NEW['candidate_sha256']}`。固定父候选 R19 candidate9：`0ba5bc7ec9603d14a1c0bde6d784d232aa78efaa1700304e75e4f2b56ef7dec2`。

按用户“先一共修复一百个地方以上再检测”，先构造 104 个原生语义位置、8 个不同 MethodDef，再共享检测。按 MethodDef + 原版指令 VA 去重：35 调用、15 字段写入、41 字段读取、11 控制流、2 循环状态；参数栈准备、元数据辅助、GC barrier、重复改动不计入。这不是 104 个方法或独立 bug。

## 范围和隔离

DeviceManager.Start/Update、ProjectManager.Start/Update、SunManager.Start/Update、ZombieManager.Start、SavesUIController.LoadPlayerSaves。八个损坏空方法与其他回调共享旧 ret 槽位，恢复时追加独立方法体，只改目标 RVA；旧共享槽位、2,309 个非目标 MethodDef、原元数据前缀、权限保持不变。新增两个 MemberRef、三个局部签名、一个闭合 Instantiate MethodSpec。冻结类型、成员访问及目标实际 DLL JIT 准备通过。

## 已完成的分层证据

- 完整候选 CIL 经冻结 Rehost 发射并执行，与原版 PC 完整未改调用方比较：管理器 552、存档 UI 216，共 768 正常记录一致；另 87 个 CLR-only 异常独立保留，不冒充 Windows SEH。八个物理退回 tiny-ret 的遗漏控制全部产生目标行为差异，其他方法不变。
- 精确 DLL 的新 IL2CPP 和 755 C++/274 C（1,029 单元）Linux/Apple 语法检查通过，全部返回源文件身份与原始日志已独立核对。Apple 损坏控制原始日志被同名覆盖的问题保留，并由独立 34-error 回放补齐，不改正常候选。
- [完整实际构建 40263428409]({RUN})：R3 路径大小写修复保留全部 6,842 个资源文件 hash、basename、GUID；Unity 中 12 个原始名称验证通过。实际 MainMenu 运行 1,862 帧，零错误，148 脚本/130 序列化标记，无缺失组件；76 shader stage、20 软件 OpenGL 受控像素检查通过。
- 实际 Unity BuildReport Succeeded、0 errors、1,364 warnings。此次真实导出共 258 C++/66 C，全部 Apple 原始日志为 0 errors、3,584 warnings；此 324 单元集合与可移植检查的 1,029 单元分开记录。真实 Xcode 编译链接 BUILD SUCCEEDED。
- [未签名 arm64 iPad IPA]({REL}) 已下载并独立验证 CRC、Mach-O、平台、无签名、导出绑定和包内 metadata；SHA-256 `{IPA['sha256']}`，192,091,143 字节。[完整新包证据](../{D.name}/README.md)。

## 保留的失败与边界

旧候选误读 MouseManager+0x68 的 Card/Prop 差异、原生模型丢独立 Card 对象、存档 fixture 泛型绑定失败、控制启动路径失败、Apple 原始日志覆盖均保留。当前 Card.cardDeviceType 链修复为原版 +0x68/+0x2C，有限记录零分歧。旧 40255311794/40259026509 真实导出因资源大小写冲突被拒绝，失败的全部报告和生成源码独立归档。此前 README/PROGRESS 原样保存于 PRE-IPA-SUCCESS 文件，详尽早期源、构造记录和原始测试仍在本目录其余文件。

R19 当前 81 个有限 profile、31,812 正常原版对照，19 个仍待验收；四个已有差异方法仍保留待修状态。R21 有 16 个源提案、5 个方法、0 个构造位置，未写入本包。全程序集 70 条类型失败或不支持项未被这些有限结果抹掉。qualified_methods=0/whole_game_accepted=false 保留产品语义验收边界。

新 IPA 尚未在 iPad Pro M5 / iPadOS 27.0 验收。黑屏、闪烁、声音、Board、真实 Metal 渲染、异常展开与整个游戏行为都不能从 Editor 或构建结果推导为通过。后续新游戏批次仍按超过 100 个有原版依据的实际位置再集中检测，不修改正确代码凑数。
'''.encode())

old_n = N / 'PRE-CURRENT-COUNT-README.md'
assert not old_n.exists()
shutil.copyfile(N / 'README.md', old_n)
(N / 'README.md').write_bytes('''# 后续源码队列：16 个提案，0 个构造位置

固定父候选 R20 d887b370。当前 5 个方法、16 个源位置提案：四个浮点 unordered 分支、三个 Buff.GetIncrement 前 float 捕获，以及两个亮度方法中的九个位置。亮度九处包括三处到期路径漏清 brightTime、三处 unordered 比较、三处 Time getter 前捕获。独立源 VA、CIL 位置和证据目录见 SOURCE-QUEUE.json。

亮度三处漏清字段有 228 条普通有限非负时间输入差异；受控 getter/callee 写字段或 NaN delta 的触发性分别标记，没有将它们等同真实 Unity 行为，也未证明任何一个就是 iPad 黑屏根因。R19 相关四个方法继续在 19 个待验收中。

这是源码提案，当前实际构造 0、新候选 0、新共享检测 0。现已导出的 R20 IPA 保持原 d887b370，未混入这些新提案。后续超过 100 个真实有原版依据的构造位置后集中检测；不把重复改动、元数据助手、参数准备或正确代码计数。初始七提案状态与旧文档均有 Git/归档保留。
'''.encode())

root_readme = R / 'README.md'
s = root_readme.read_text('utf-8')
begin = s.index('Latest [R20 Sites104]')
end = s.index('\n\nLatest [whole managed-code census]',begin)
s = s[:begin] + f'''Latest [R20 IPA]({REL}) is now built: actual Unity export, all 258 C++ / 66 C source checks, Xcode compilation/linking and unsigned arm64 IPA packaging succeed in [run 40263428409]({RUN}), bound to commit c22560ed9e025f04d4e58bc38ba0f23df8b9feb4 and unchanged game candidate d887b370…095b37. The resource casing fix preserves all 6,842 file hashes, basenames and GUIDs; actual Unity verifies all 12 relocated asset identities. Actual BuildReport has zero errors (1,364 warnings); all 324 generated sources/raw Apple logs have zero errors (3,584 warnings). Downloaded IPA ZIP, physical-iOS Mach-O, unsigned status and exact packaged metadata have been independently read back. [Complete evidence](Recovery/Startup-R20-IPA-2026-10-26/README.md). iPad Pro M5 black-screen/flicker/audio/Board acceptance remains pending.''' + s[end:]
s = s.replace('R19 now has81 finite profiles/19 pending, including two known floating-point differences; new IPA/device acceptance is pending.', 'R19 has 81 finite profiles / 19 pending, including four methods with observed differences. R21 records 16 source proposals / 0 constructed positions; these are not included in this IPA.')
s = s.replace('No updated IPA or iPad black-screen/flicker acceptance is claimed.', 'The current R20 unsigned IPA has actual build and byte-readback evidence above; iPad black-screen/flicker and full-game acceptance remain pending.')
root_readme.write_bytes(s.encode())

p = R / 'Recovery/STATUS.md'
s = p.read_text('utf-8')
anchor = '## Latest checkpoint - 2026-10-26\n\n'
assert anchor in s
new = f'''[R20 unsigned iPad IPA]({REL}) has completed actual [Unity/Xcode run 40263428409]({RUN}). Unity export: Succeeded / 0 errors / 1,364 warnings. All actual 258 C++ / 66 C source hashes and raw Apple logs: 0 errors / 3,584 warnings; Xcode BUILD SUCCEEDED. IPA: 192,091,143 bytes, arm64 / physical iOS / unsigned, SHA-256 f7334b1b330cdc7dc2cbeeb0c9851c8c233bc0fc923c69e6de36f38aeda56c75. Its metadata exactly matches this export. All asset hashes/GUIDs/names preserved; all 12 relocated assets pass actual Unity identity checks. [Independent readback and preserved raw files](Startup-R20-IPA-2026-10-26/README.md). **Device black screen/flicker, physical audio, Board and full-game acceptance remain pending.**

[Next source queue](Startup-R21-Source-Queue-2026-10-26/README.md): 16 proposals across five methods / 0 constructed positions. [Brightness review](Startup-R19-Brightness-Differences-2026-10-26/README.md) preserves 228 ordinary finite differences from three missing expiry field-zero sites, plus separately labelled expanded-input/callback differences. The R19 finite union stays 81 methods / 31,812 normal records, 19 pending, including four methods with observed differences. These proposals are not in the current IPA.

## Earlier checkpoints (historical status at each observation)

'''
s = s.replace(anchor,anchor+new,1)
p.write_bytes(s.encode())
for root in [T,F,A,N]:
    manifest(root)
print(json.dumps(dict(updated_current_checkpoints=4, root_documents=2, candidate_changed=False, ipa_bytes=IPA['bytes'], new_source_proposals=16, new_constructed_positions=0)))
