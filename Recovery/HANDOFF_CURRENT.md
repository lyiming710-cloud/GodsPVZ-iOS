# GodsPVZ 1.0.2 高保真恢复 / iOS 移植 — CURRENT HANDOFF

快照时间：2026-09-16 UTC
仓库：lyiming710-cloud/GodsPVZ-iOS
分支：high-fidelity
当前 HEAD（上传前）：0cd13b110189041a8ded70e54ad58ccbe8384ac9

## 1. 当前结论

当前正式 sealed baseline 仍为 HF55。任何 Stage9.1 后续结果均属于 development candidate，不能自动晋级 HF56，也不能改写 HF55。

当前 FIRST_INVALID_IL / 正式 blocker：Administrator.Start()，MethodDef 0x06000004，RID 4。

Administrator.Start 目前仍未完成 static-qualified 或 runtime-qualified。仓库中已经上传专用 Cecil patcher 源码和项目文件，但还没有完成该 patcher 的 CI 执行、静态资格审计或 exact-R3 natural runtime。

Administrator.Start 的正式输入 candidate：

26064265fdba4e9b501f0c3312260503fdde43b9e111072c19dd39891e5ca022

不得把 TextLink.Update 的 runtime 通过误写成 Administrator.Start 已通过。TextLink.Update 的 exact-R3 natural runtime 已通过，但它只是当前 candidate 的前置恢复链。

## 2. 已确认的 Administrator.Start 证据

- managed preimage SHA256：b6aa91789131594a0b1ac89a83197f026261ea6e9867df34cc2b8b15b46b8089
- 原 body：635 bytes，30 locals，168 instructions，0 exception handlers
- PC native：0x1802FDD20–0x1802FE980，3168 bytes
- PC native slice SHA256：3d83a514d37c18b7d91df7ceba17803bb0ac3cbd670dab15e823af4be8b58d02
- pointer table entry：0x181B82D78；RID 4
- valid-mode gate：unsigned (mode + 1) <= 5；只有 mode -1..4 执行 systems deactivation foreach
- BoardManager current-board 合法访问：BoardManager.get_Instance() -> BoardManager.Board_OnPlay()
- 禁止直接生成跨类型 private field BoardManager.board 的 ldfld

## 3. 当前仓库内的关键文件

| 文件 | 作用 | 当前状态 |
|---|---|---|
| Tools/Stage9AdministratorStartPatch/Program.cs | 只改 0x06000004 的恢复 patcher | 已上传，未完成资格执行 |
| Tools/Stage9AdministratorStartPatch/Stage9AdministratorStartPatch.csproj | Mono.Cecil/net10 构建项目 | 已上传 |
| Tools/Stage9AdministratorStartDependencyProbe/Program.cs | 依赖、preimage、token 证据 probe | 已上传 |
| Tools/Stage9AdministratorStartRefProbe/Program.cs | exact reference 证据 probe | 已上传 |
| Tools/Stage9NativeProbe/Administrator.Start.native-evidence.md | PC native 语义证据 | 已上传 |
| .github/workflows/stage9-2606-administrator-start-dependency-probe.yml | dependency probe CI | 已上传 |
| .github/workflows/stage9-2606-administrator-start-exact-ref-probe.yml | exact-ref probe CI | 已上传 |
| .github/workflows/stage9-2606-administrator-start-readonly-probe.yml | read-only native/metadata probe CI | 已上传 |

## 4. 最新 CI 结果

- run 35049752106：Administrator Start dependency probe，SUCCESS；head 236229b951154dccfd31984dbcbaa5218f8c0fbc；最新 artifact 10428660878，digest sha256:f76f7fcaad17070ac8b7ab93337b8cf2e76db10dc401303c96e8a7d2a625a669
- run 35043040058：Administrator Start exact-ref probe，SUCCESS；artifact 10425308520，digest sha256:639b506e8750faee339f7d35cd022f1d9b19aa3f023156b20cf4afb21bb8b736
- run 35041620741：TextLink.Update exact-R3 natural runtime，SUCCESS；artifact 10425941126，digest sha256:1b02ffb7484c80632f51fa6a8d46328d3c2f3cada6094d6691bc751fd3399745
- run 35046561491：失败原因是旧 workflow 的 methods=17 机械断言；不能当作 Administrator.Start patch 或 IL 失败

## 5. 下一位接手者的执行顺序

1. 重新读取 high-fidelity 最新 HEAD；不要假设本交接提交后没有并发变化。
2. 以 candidate SHA 26064265...e5ca022 和 preimage SHA b6aa9178...b46b8089 双重锁定输入。
3. 在 GitHub runner 构建并执行 Tools/Stage9AdministratorStartPatch，只修改 MethodDef 0x06000004。
4. 读取 pre/post IL 和 Cecil reopen audit，确认 2317 MethodDefs、2802 fields、method headers、assembly refs、MemberRefs、TypeRefs 均按 patcher 门槛保持；确认没有 System.Private.CoreLib、zeroVector、unmanaged scaffolding、List<T> 私有字段或 BoardManager.board 跨类型访问。
5. 检查历史已恢复方法不回归；静态门禁未全过时不得运行昂贵 Unity runtime。
6. 静态门禁全部通过后，只运行一次 exact-R3 natural runtime，继续复用 exact R3 run 34747460206 和 exact Unity China Editor run 34668863583 的固定输入。
7. runtime 读取最终 audit、五阶段 pass、target exception count 和新的 FIRST_INVALID_IL；只有 direct target runtime 通过后才更新 candidate 状态。
8. 根据新的 FIRST_INVALID_IL 继续下一个 MethodDef；不要预先猜测下一个 target。

## 6. 固定禁止事项

- 不修改或删除 HF55 校验文件，不把 development candidate 自动 seal。
- 不修改方法或字段 visibility，不添加 public wrapper。
- 不添加 catch/try-catch 作为 fallback，不吞异常。
- 不直接访问 BoardManager.board private field。
- 不访问 List<T>._items、_size、_version。
- 不根据玩法猜测未闭环逻辑，不用 Cpp2IL 的 unmanaged-memory-load 或 indirect-jump 文本当业务语义。

旧的 HANDOFF_CURRENT 快照已由本文件替换。Recovery 目录中的 FIRST_IMPORT、R2、HF46 和 STATUS 文件继续作为历史审计资料保留，不替代本文件的当前指令。