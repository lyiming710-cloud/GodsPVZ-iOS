# GodsPVZ Bulk152 GitHub Actions Verification Runner

本目录包含专为 Ubuntu 24.04 GitHub Actions 环境设计的可重定位独立验证运行器 `actions_verifier_runner.py` 及其必要说明。

---

## 1. 资格判定边界与实质缺口说明 (Qualification Scope & Limits)

- **资格范围 (Scope)**: `isolated_target_with_stubbed_dependencies`
- **深层交互链判定**: `integration_01D3_01E0_0677 = UNPROVEN`
- **正式晋级许可**: `formal_promotion_allowed = false`

### 依赖打桩说明与证据限度
在交付的 `Native109Fixture.cs` 与 `native109_oracle.cpp` 实现中：
1. `MouseManager.GetZombieUnderMouse` 被桩函数重定向到静态桩对象 `MouseManager.UnderMouseZombie`；
2. `PrepareUIController.get_Instance` 被重定向到手写静态字段 getter，未执行真实单例查找逻辑；
3. 机器码 Oracle 在 `0x18031ee10` 地址注入了外部依赖拦截钩子。

因此，本测试套件虽然对 109 批次的三个方法目标本身完成了 768 例正向对齐与 5 个控制突变体验证，但真实的 `01D3 -> 01E0` 交互调用链以及真实 `0677` 逻辑并未在此次运行中被执行。本验证套件定位为**在 GitHub Actions 云端重放以校验环境与编译管线**的基础工程验证，不构成对深层交互链和正式产品上线的晋级资格。后续将在 GitHub Actions 流程中补充无桩真实交互链回归。

---

## 2. 目录布局规范与依赖解析优先级 (Path Resolution & Layout)

运行器优先接受显式命令行参数传入各文件路径，未传时按顺序回退解析：
1. 显式 CLI 参数（如 `--native109-oracle-cpp`）
2. `--portable-root`（或环境变量 `PORTABLE_ROOT`）下的 `work/...`
3. 本地同级 `sources/...`

标准 `portable_root` 布局：
```text
portable_root/
├── work/
│   ├── candidate1.dll                    # 待验 IL 程序集 (Bulk152)
│   ├── Native109Fixture.cs               # 交付版 CIL 夹具源码
│   ├── native109_oracle.cpp              # 交付版机器码 Oracle 源码
│   ├── PRIMARY-ACCEPTANCE-2026-10-16.json # 108 名单方法定义
│   ├── il2cpp_stage_coherent/            # 官方 iOS Player 依赖池
│   │   └── Library/Bee/artifacts/iOS/ManagedStripped/
│   └── results/
│       └── il2cpp_conversion/
│           └── il2cpp_convert.argv.json  # 权威 IL2CPP 转换命令行闭包
├── toolchain/
│   ├── il2cpp/                           # Canary IL2CPP 部署根目录
│   │   └── build/deploy/il2cpp           # IL2CPP 可执行程序
│   └── native18-IL2CPP/                  # Native18 头文件与 Baselib 头文件
│       ├── libil2cpp/
│       └── external/baselib/Include/
├── tools/
│   └── Mono.Cecil.dll                    # Cecil 依赖
└── original/
    └── GameAssembly.dll                  # 权威原版 PC GameAssembly.dll
```

> **隔离要求**：运行器以只读方式使用输入源。`--output-dir` 必须为全新不存在的目录，运行时若发现已存在则直接拒绝，严禁状态污染。

---

## 3. GitHub Actions (Ubuntu 24.04) 显式调用规范

```yaml
- name: Setup Toolchain
  run: |
    sudo apt-get update
    sudo apt-get install -y clang-18 g++
    dotnet --version
    clang++-18 --version
    sudo update-alternatives --install /usr/bin/clang++ clang++ /usr/bin/clang++-18 100

- name: Run Verification Pipeline
  env:
    PORTABLE_ROOT: /home/runner/work/portable_root
    VERIFIER_OUTPUT_DIR: /home/runner/work/verifier_output
  run: |
    python3 audit/actions-migration-bulk152-2026-10-20/verifier-authored/actions_verifier_runner.py \
      --portable-root "$PORTABLE_ROOT" \
      --output-dir "$VERIFIER_OUTPUT_DIR" \
      --native109-oracle-cpp "$PORTABLE_ROOT/work/native109_oracle.cpp" \
      --native109-fixture-cs "$PORTABLE_ROOT/work/Native109Fixture.cs" \
      --roster-json "$PORTABLE_ROOT/work/PRIMARY-ACCEPTANCE-2026-10-16.json" \
      --il2cpp-argv-json "$PORTABLE_ROOT/work/results/il2cpp_conversion/il2cpp_convert.argv.json"

- name: Upload Artifacts
  if: always()
  uses: actions/upload-artifact@v4
  with:
    name: bulk152-verification-artifacts
    path: |
      /home/runner/work/verifier_output/MANIFEST-v1.json
      /home/runner/work/verifier_output/Archive-v1-evidence.zip
      /home/runner/work/verifier_output/Archive-v1-game-sources.zip
      /home/runner/work/verifier_output/results/
```

---

## 4. 动态检验规则与产出规范 (Verification & Output Artifacts)

### 动态检验规则 (防空通过与硬编码校验)
1. **Oracle 校验**：严格校验 `0x06000115`, `0x0600011A`, `0x060001D3` 三个 Token，且各 Token 对应的 case id 0..255 恰好出现一次（共 768 行）；stderr 必须含 `ORACLE_SIGNAL_AUDIT` 且 `unexpected_signal_count == 0`。
2. **Fixture 校验**：实测总例数必须等于 Oracle TSV 行数（768），且 `total == pass + fail` 且 `fail == 0`。5 个预期突变体名称（`mut_0115_skip_countdown_check`, `mut_011A_invert_threshold`, `mut_011A_time_before_capture`, `mut_01D3_omit_click`, `mut_01D3_reuse_field`）必须恰好匹配且各 `detected_deltas > 0`，防空列表误通过。
3. **C++ 归因机制**：仅扫描非分号结尾的真实函数定义（排除前向声明），标明 `attribution_method = "heuristic_next_definition"`。诊断日志解析失败时不静默跳过，记录至 `UNPARSEABLE-DIAGNOSTIC-LINES.json`，同时全量落盘 `UNMAPPED-ERRORS.json` 和 `INCLUDED-FILE-ERRORS.json`。

### 产出物清单
1. **`MANIFEST-v1.json`**: 机器可读权威清单（含资格降级声明、各阶段状态、归档哈希）。
2. **`Archive-v1-evidence.zip`**: 证据全集（候选 DLL、Oracle 源码/TSV/stderr、Fixture 源码/报告、108 名单及全部 `results/`）。
3. **`Archive-v1-game-sources.zip`**: 19 个 `GodsPVZRuntime1*.cpp`、`GodsPVZRuntime1_CodeGen.c` 与 `global-metadata.dat`。
4. **`results/CPP-STATUS.json`**: Clang 状态汇总与动态 108 归因。
5. **`results/GAME-TU-RESULTS.json`**: 全部编译单元实际退出码与耗时。
6. **`results/ROSTER-108-ANALYSIS.json`**: 名单方法动态归因明细。
7. **`results/N109-FIXTURE-REPORT.json`**: 动态实测例数与突变体检出报告。

---

## 5. 退出码门禁策略 (Non-Zero Exit Code Policy)

- Oracle 编译/执行/信号审计失败：直接以 Oracle 退出码中断；
- Fixture 编译/执行/正向失败/突变体未检出：以非零退出码中断；
- IL2CPP 转换非零或缺少 metadata：以转换器退出码中断；
- **Clang 最终门禁**：即使阶段 1-3 全通并完成打包，**只要 19 个游戏 TU 中存在任何一个编译失败（exit code != 0），运行器最终强制退出码为 1**。
