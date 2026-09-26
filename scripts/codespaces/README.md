# GodsPVZ iOS Port - Codespaces 运行管道手册

本套脚本用于在 **GitHub Codespaces (Ubuntu Linux x86_64)** 容器中，**1:1 复现原 GitHub Actions 的 Unity iOS 导出与 IL2CPP 门禁流程**。

---

## 目录结构

```
scripts/codespaces/
├── 01_check_environment.sh      # 检查 Linux 环境依赖、磁盘配额、Unity 与 GitHub 授权
├── 02_run_local_static_gate.sh   # 运行 6 方法本地求值栈验证器与 CIL 规格检查
├── 03_setup_unity_and_project.sh # 下载并拼装 Unity 2022.3.44f1c1、iOS 模块与 R3 工程缓存
├── 04_export_ios_xcode.sh        # 执行 xvfb 虚拟渲染无头导出，触发 IL2CPP 生成 C++ 源码
├── run_all.sh                    # 一键编排执行器（支持 --skip-setup 等参数）
└── README.md                     # 本手册
```

---

## 环境变量要求

在 Codespaces 终端执行前，请先设置好以下环境变量：

```bash
# 1. GitHub Token（用于拉取 Actions 产物中固化的 R3 缓存与 Editor 分卷）
export GH_TOKEN="ghp_xxxxxxxxxxxx"  # 或使用 gh auth login

# 2. Unity 账号与密码（用于 Unity Licensing Client 激活 Personal 席位）
export UNITY_EMAIL="your_unity_account@example.com"
export UNITY_PASSWORD="your_unity_password"

# 3. （可选）工作目录（默认为 /tmp/stage9-native4-ios）
export GATE_WORK_DIR="/tmp/stage9-native4-ios"
```

---

## 快速使用

### 1. 一键运行全流程
```bash
cd scripts/codespaces
./run_all.sh
```

### 2. 仅验证静态门与求值栈
无需下载 Unity 编辑器，几秒内完成 6 方法 CIL 栈类型分析：
```bash
./02_run_local_static_gate.sh
```

### 3. 环境准备好后，快速迭代测试新的候选 DLL
当 Unity 编辑器与 R3 工程已经解压好后，后续测试新的 `Assembly-CSharp.dll` 无需重复下载：
```bash
# 跳过安装，仅替换 DLL 并触发 Unity IL2CPP 编译
./run_all.sh --skip-setup --candidate /path/to/new/Assembly-CSharp.dll
```
