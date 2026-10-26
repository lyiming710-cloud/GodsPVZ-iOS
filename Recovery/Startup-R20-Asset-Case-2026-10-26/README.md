# 六组资源大小写冲突：保留原资源，真实导入和 IPA 构建通过

[实际工作流 40263428409](https://github.com/lyiming710-cloud/GodsPVZ-iOS/actions/runs/40263428409) 已验证修复：Unity 导出 0 errors，12 份资源的 GUID→新路径→LoadMainAsset.name 均符合原始名称；全部实际 324 个 C/C++ 和后续 Xcode 构建通过，并生成未签名 IPA。[成功证据及下载](../Startup-R20-IPA-2026-10-26/README.md)。

原版 R3 内 AnimationClip 的 Enter/enter、Exit/exit、Idle/idle，Sprite 的 Background/background、Background_0/background_0，以及 Texture2D 的 Background.png/background.png，共六组大小写路径冲突。40259026509 的真实 BuildReport 分四条 Error 重复报告这些路径，原错误门拒绝，Mac 跳过；全部失败证据仍保留。本次没有将错误降级或忽略。

精确源检查 40262106436 核对全部 6,842 个 Assets 文件。12 份资源和 12 份 meta 各移入其 GUID 命名子文件夹，原 basename、原始字节/hash、GUID 保持；其他 6,818 份文件路径和字节不变。大小写冲突归零。旧路径引用、特殊 Resources/StreamingAssets 路径、丢 meta/重复 GUID 会阻止操作；四个错误计划在第一次移动前被拒绝。第一次源码预检查因 epoch 日期无法写传统 ZIP 而停止的证据仍保留，修复仅钳制 ZIP 元数据日期。

冻结计划和原始导入名称表在本目录。路径修复在 R3 解开后、第一次 Unity 导入前执行。GodsPVZAssetCaseAudit 真实读取 Unity GUIDToAssetPath、AssetPathToGUID、LoadMainAsset.name；Sprite 的内部名称和 PNG 默认 Texture2D 名称均按原版检查。游戏候选 SHA-256 保持 d887b370…095b37。

失败导出的完整源归档 SHA-256 53b73734f7edc2ed64134c86e7b7ee584aebea12a33ace2f100876d47ee7e80e，继续保存在旧 Release 的 Startup-R20-UNQUALIFIED-40259026509-generated-and-report.zip；新成功导出归档 SHA-256 c6cba5a27d9ce2151366a5f0db56ef1cf12c6107f8b171c46e301893bc8bfbe2，独立保存。每次 329 个原始文件均已字节回读，不能互相借用结论。PRE-IPA-SUCCESS 文件保留本目录先前状态。

这是既有 R20 超过 100 位置批次的资源/导出修复，不把 24 个文件路径计作新游戏语义修复。新 IPA 尚无 iPad Pro M5 上的启动、持续画面、声音或 Board 结果，不能据构建成功声称黑屏已经解决。
