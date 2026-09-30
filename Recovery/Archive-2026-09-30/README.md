# GodsPVZ 当前与历史资料归档 · 2026-09-30
> **发布完成**：归档于 2026-09-30T08:59:22Z 正式发布，13 个资产（1,220,069,545 字节）全部核对通过；tag 固定在 `555cc0fafbf7b35361112bb02c6e04839780f4b1`。完整发布结果见 [FINAL-ARCHIVE-RESULT.json](FINAL-ARCHIVE-RESULT.json)。`RELEASE-VERIFICATION.json` 是发布前草稿状态的验收快照，最终状态以上述发布记录为准。


本目录是项目资料入口。**目标仍是高保真恢复并导出未签名 IPA，目前没有合格 IPA；归档并不意味着候选通过验收。** 当前独立审查见 [Native23 审查](../Native23-Review-2026-09-30/README.md) 和 [完整结论](../Native23-Review-2026-09-30/REVIEW.md)。

## 下载与保存范围

完整资料位于 [GitHub 分类归档发布](https://github.com/lyiming710-cloud/GodsPVZ-iOS/releases/tag/recovery-archive-2026-09-30)。这是资料归档预发布，不是游戏发行版。

| 文件 | 内容与用途 |
|---|---|
| `GodsPVZ_1.0.2.zip` | 原版 PC 发行包，原样保存，包含 native/metadata/游戏资源；不是原始开发工程 |
| `GodsPVZ_1.0.2_Android.apk` | 原版 Android 发行包，原样保存，供双版本对照 |
| `local-project-history.zip` | 本地 5347 个项目文件，2894 个去重对象，原路径映射完整保留 |
| `codespace-historical-evidence.zip` | 活跃 Codespace 中 5576 个文件，含 Native19–23 验证、实验候选和生成的 C++；完整 Xcode tar 单独保存 |
| `GodsPVZ-Stage9-native18-unsigned-Xcode.tar.zst` | Native18 成功导出的完整 Xcode 工程；其后编译失败，**不是 IPA** |
| `project-history.bundle` | 09dd564 之后 Native22、Native23、独立审查两个分叉的完整增量 Git 对象；需要前置提交 09dd564 |
| `LOCAL-MANIFEST.json`、`CODESPACE-MANIFEST.json` | 每个文件的来源、原路径、大小、SHA-256、内容对象/独立资产，以及排除理由 |
| `ASSET-SHA256.json`、`SHA256SUMS.txt` | 上传资产的大小、哈希和固定下载链接 |
| `restore_archive.py` | 校验归档并安全恢复原目录；仅依赖 Python 标准库 |

各 ZIP 使用 `objects/<sha256>` 存储独立内容，`MANIFEST.json` 映射原路径。多个历史版本有同名文件时保留各自路径；只有字节完全相同的内容才去重。仓库中的可浏览快照也通过 `.gitattributes` 保留原字节，内容以清单和归档为准。

## 分类与历史代码

- **原版输入**：PC ZIP、Android APK、metadata、原始 managed 基线。`18e44…f18bd` 的身份不因归档中出现其它 DLL 而改变。
- **代码/工作流快照**：本地 2437 个，另有 Codespace 的 57 个，共 2494 个，可通过下表浏览；[逐文件索引](SOURCE-INDEX.json) 记录原路径和哈希。历史工作流作为资料存放于本目录，不会替换根 `.github/workflows`。
- **基线与实验候选**：包括各阶段 DLL；Native23 Batch2/Batch2a 是实验资料，不能自动进入正式流程。
- **native/编译证据**：IL、反汇编、元数据、类型检查、IL2CPP 转换、完整 C++ 编译结果与日志。
- **交接与报告**：各阶段 handoff、验证清单和最新独立审查。更早报告中的成功描述应结合后来修正阅读。
- **无效早期产物**：`build/output/GodsPVZ_1.0.2_unsigned.ipa` 仅 8192 字节，缺少有效游戏构建，明确不能作为交付件。

| 原始资料目录/阶段 | 可浏览代码文件 | 入口 |
|---|---:|---|
| `.github` | 1 | [浏览代码](source-snapshots/.github/) |
| `audit` | 13 | [浏览代码](source-snapshots/audit/) |
| `audit/codespace-native14-review` | 60 | [浏览代码](source-snapshots/audit/codespace-native14-review/) |
| `audit/local-preflight` | 21 | [浏览代码](source-snapshots/audit/local-preflight/) |
| `audit/native15-work` | 510 | [浏览代码](source-snapshots/audit/native15-work/) |
| `audit/native18-review` | 12 | [浏览代码](source-snapshots/audit/native18-review/) |
| `audit/native19-analysis` | 2 | [浏览代码](source-snapshots/audit/native19-analysis/) |
| `audit/native19-work` | 545 | [浏览代码](source-snapshots/audit/native19-work/) |
| `audit/native22-work` | 720 | [浏览代码](source-snapshots/audit/native22-work/) |
| `audit/native23-full-review-2026-09-30` | 60 | [浏览代码](source-snapshots/audit/native23-full-review-2026-09-30/) |
| `audit/native23-independent-review` | 2 | [浏览代码](source-snapshots/audit/native23-independent-review/) |
| `audit/stage9-native3` | 450 | [浏览代码](source-snapshots/audit/stage9-native3/) |
| `audit/stage9-native4` | 37 | [浏览代码](source-snapshots/audit/stage9-native4/) |
| `audit/stage9-native5` | 1 | [浏览代码](source-snapshots/audit/stage9-native5/) |
| `codespace` | 57 | [浏览代码](source-snapshots/codespace/) |
| `scripts` | 3 | [浏览代码](source-snapshots/scripts/) |

这些源文件包括恢复/补丁器、验证脚本、生成/反编译材料及工作流。我们目前持有发行包和恢复工程，**不能称为取得了原作者完整 C# 开发源码**。

## Git 历史与隔离

此次归档前的最新进度提交为 `77fb194e59ea420b480620001a2d30b3d8b9adad`，其父链为 `09dd564 → 232273a → f1a9651 → 77fb194`。Native22 的另一条分叉 `09dd564 → b56b2d7 → e35640e` 也保存在增量 bundle 中，未被合并为候选修复。

[Git 引用快照](GIT-REFS.txt)、[Git 范围说明](HISTORY-SCOPE.json)、[bundle 验证](history-verify.txt) 精确说明哪些提交和分叉被保存。更早历史可从 GitHub origin 获取；本地 checkout 是部分克隆，本 bundle **不等同于完全离线的全仓库镜像**。旧阶段现存的文件快照已另外完整纳入本地资料 ZIP。

Codespace 仓库 `HEAD` 保持 `f1a96517d21826f0de9efb579b481dee4a9863b0`，归档时工作树干净。本次只发布分类资料和文档，没有改变候选输入、补丁逻辑或构建工作流，也没有手动启动新的 Actions。

## 验证与恢复

将所需资产下载到同一目录，先核对 `ASSET-SHA256.json`。例如：

```sh
python restore_archive.py --archive local-project-history.zip --assets . --verify-only
python restore_archive.py --archive local-project-history.zip --assets . --destination restored/local
python restore_archive.py --archive codespace-historical-evidence.zip --assets . --verify-only
python restore_archive.py --archive codespace-historical-evidence.zip --assets . --destination restored/codespace
```

工具先逐内容验证 SHA-256 和大小，再恢复原路径；拒绝越界路径和覆盖不同内容。PC、APK、Xcode tar 为清单指定的独立资产，验证对应归档时也需要下载。

恢复 Git 增量时先取得原仓库和前置提交，再导入到独立审查引用，避免误更新正式分支：

```sh
git clone https://github.com/lyiming710-cloud/GodsPVZ-iOS.git restored/git
git -C restored/git bundle verify ../../project-history.bundle
git -C restored/git fetch ../../project-history.bundle refs/heads/docs/native23-review-2026-09-30:refs/heads/archive/native23-reviewed
git -C restored/git fetch ../../project-history.bundle refs/heads/repair/codex-native22-ipa:refs/heads/archive/native22-history
```

## 明确排除与限制

本地初始盘点为 6310 个文件、约 5.66 GB；其中 Unity 安装包约 4.26 GB。Unity/.NET 编辑器与运行时下载缓存、可重建工具输出、授权激活/归还日志、Git 工作树机器路径、重复的 base64 传输文本、个人助手记忆和网络诊断不作为项目源码上传；逐文件理由保存在清单中。`bin/obj/Library/Temp/node_modules/python-deps` 等缓存目录在初始遍历时剪枝，其目录级范围也有记录。

Codespace 的重复解包 Xcode 工程及 Actions 下载容器已由原样 Xcode tar 和 artifact 元数据覆盖；再生成的 C++、验证结果和实验修改输入仍保存。此归档覆盖当前本地工作区、活跃恢复仓库的 `.validation` 资料，以及所列 Git 分叉；不声称包括 Codespace 其它工作目录的全部忽略缓存、已删除文件、从未下载过的 CI 附件或其它账户的私有资料。

## 接手时的验收约束

高还原要求仍有效：原始 native 与字段偏移/调用约定是语义依据；静态栈检查、类型检查、IL2CPP 转换成功、C++ 编译成功和真实运行行为属于不同证据层。不可用 `ldnull` 全局替换、`ret → throw`、防御性判空或只改局部变量类型来掩盖缺失控制流。非目标隔离应同时说明规范化语义与原始字节，不把 MemberRef 编号变化偷换成字节完全不变。

下一步应先修正独立审查指出的来源污染、opcode 前置约束、分支/EH 目标、解释器写字段和站点级 native 证据，再对明确限定的目标重建候选。完整 Unity/Xcode gate 通过后才能检查未签名 IPA 的 Mach-O ARM64、资源和安装/运行行为。归档完成不改变这些验收条件。
