#!/usr/bin/env python3
"""
Actions Verification Runner for GodsPVZ Native Bulk152 Takeover.

Fully relocatable runner designed to execute inside Ubuntu 24.04 GitHub Actions.
Consumes a portable_root checkpoint containing:
  - work/: candidate1.dll, Native109Fixture.cs, native109_oracle.cpp, PRIMARY-ACCEPTANCE-2026-10-16.json,
           il2cpp_stage_coherent/Library/Bee/artifacts/iOS/ManagedStripped, results/il2cpp_conversion/il2cpp_convert.argv.json
  - toolchain/il2cpp/: canary/il2cpp deploy directory
  - toolchain/native18-IL2CPP/: libil2cpp + external baselib headers
  - tools/Mono.Cecil.dll: Mono.Cecil assembly for fixture compilation
  - original/GameAssembly.dll: Authoritative PC GameAssembly.dll (31,969,792 bytes)
  - fixture-bin/, fixture-obj/: precompiled fixture binaries (optional)
"""

import argparse
import concurrent.futures
from collections import Counter, defaultdict
from datetime import datetime, timezone
import hashlib
import json
import os
import pathlib
import re
import shutil
import subprocess
import sys
import time
import zipfile

def sha256_file(p: pathlib.Path) -> str:
    h = hashlib.sha256()
    with open(p, "rb") as f:
        while chunk := f.read(65536):
            h.update(chunk)
    return h.hexdigest()

class PathConfig:
    def __init__(self, portable_root: pathlib.Path, output_dir: pathlib.Path, args: argparse.Namespace):
        self.portable_root = portable_root.resolve()
        self.output_dir = output_dir.resolve()
        if self.output_dir.exists():
            raise FileExistsError(
                f"Output directory already exists: {self.output_dir}. "
                "The verifier runner requires a fresh, non-existent output directory to ensure isolation."
            )
        self.output_dir.mkdir(parents=True, exist_ok=False)

        self.res_dir = self.output_dir / "results"
        self.res_dir.mkdir(parents=True, exist_ok=True)
        (self.res_dir / "batches").mkdir(parents=True, exist_ok=True)
        (self.res_dir / "per_method").mkdir(parents=True, exist_ok=True)
        (self.res_dir / "il2cpp_conversion").mkdir(parents=True, exist_ok=True)
        self.clang_logs_dir = self.res_dir / "clang_logs"
        self.clang_logs_dir.mkdir(parents=True, exist_ok=True)

        sources_dir = pathlib.Path(__file__).resolve().parent / "sources"

        self.candidate_dll = self._resolve(
            args.candidate_dll,
            [
                self.portable_root / "work/candidate1.dll",
                self.portable_root / "candidates/candidate1.dll",
                self.portable_root / "candidate1.dll"
            ]
        )
        self.game_assembly = self._resolve(
            args.game_assembly_dll,
            [
                self.portable_root / "original/GameAssembly.dll",
                self.portable_root / "original_inputs/GameAssembly.dll",
                self.portable_root / "GameAssembly.dll"
            ]
        )
        self.il2cpp_exe = self._resolve(
            args.il2cpp_exe,
            [
                self.portable_root / "toolchain/il2cpp/build/deploy/il2cpp",
                self.portable_root / "il2cpp/build/deploy/il2cpp"
            ]
        )
        self.custom_il2cpp_root = self._resolve(
            args.custom_il2cpp_root,
            [
                self.portable_root / "toolchain/il2cpp",
                self.portable_root / "il2cpp"
            ]
        )
        self.native18_il2cpp = self._resolve(
            args.native18_il2cpp,
            [
                self.portable_root / "toolchain/native18-IL2CPP",
                self.portable_root / "native18-IL2CPP"
            ]
        )
        self.mono_cecil_dll = self._resolve(
            args.mono_cecil_dll,
            [
                self.portable_root / "tools/Mono.Cecil.dll",
                self.portable_root / "work/inspector/Mono.Cecil.dll",
                self.portable_root / "Mono.Cecil.dll"
            ]
        )
        self.managed_support_dir = self._resolve(
            args.managed_support_dir,
            [
                self.portable_root / "work/il2cpp_stage_coherent/Library/Bee/artifacts/iOS/ManagedStripped",
                self.portable_root / "ManagedStripped"
            ]
        )
        self.oracle_cpp_source = self._resolve(
            args.native109_oracle_cpp,
            [
                self.portable_root / "work/native109_oracle.cpp",
                self.portable_root / "native109_oracle.cpp",
                sources_dir / "native109_oracle.cpp"
            ]
        )
        self.fixture_cs_source = self._resolve(
            args.native109_fixture_cs,
            [
                self.portable_root / "work/Native109Fixture.cs",
                self.portable_root / "Native109Fixture.cs",
                sources_dir / "Native109Fixture.cs"
            ]
        )
        self.roster_json = self._resolve(
            args.roster_json,
            [
                self.portable_root / "work/PRIMARY-ACCEPTANCE-2026-10-16.json",
                self.portable_root / "PRIMARY-ACCEPTANCE-2026-10-16.json",
                sources_dir / "PRIMARY-ACCEPTANCE-2026-10-16.json"
            ]
        )
        self.il2cpp_argv_json = self._resolve(
            args.il2cpp_argv_json,
            [
                self.portable_root / "work/results/il2cpp_conversion/il2cpp_convert.argv.json",
                self.portable_root / "work/il2cpp_convert.argv.json",
                sources_dir / "il2cpp_convert.argv.json"
            ],
            optional=True
        )

    def _resolve(self, explicit: str, fallbacks: list, optional: bool = False) -> pathlib.Path:
        if explicit:
            p = pathlib.Path(explicit).resolve()
            if p.exists(): return p
            raise FileNotFoundError(f"Explicit path not found: {explicit}")
        for fb in fallbacks:
            if fb and fb.exists(): return fb.resolve()
        if optional:
            return None
        raise FileNotFoundError(f"Could not locate required dependency. Checked: {[str(x) for x in fallbacks]}")

def run_logged(cmd: list, log_prefix: pathlib.Path, cwd: pathlib.Path = None, env: dict = None) -> subprocess.CompletedProcess:
    log_prefix.parent.mkdir(parents=True, exist_ok=True)
    log_prefix.with_suffix(".argv.json").write_text(json.dumps(cmd, indent=2), encoding="utf-8")
    t0 = time.time()
    p = subprocess.run(cmd, cwd=str(cwd) if cwd else None, env=env, capture_output=True, text=True)
    dur = time.time() - t0
    log_prefix.with_suffix(".stdout.log").write_text(p.stdout, encoding="utf-8")
    log_prefix.with_suffix(".stderr.log").write_text(p.stderr, encoding="utf-8")
    log_prefix.with_suffix(".exitcode.txt").write_text(str(p.returncode), encoding="utf-8")
    return p

def record_toolchain_identities(cfg: PathConfig) -> dict:
    identities = {}
    for tool, cmd in [
        ("g++", ["g++", "--version"]),
        ("clang++", ["clang++", "--version"]),
        ("dotnet", ["dotnet", "--version"])
    ]:
        try:
            r = subprocess.run(cmd, capture_output=True, text=True)
            identities[tool] = r.stdout.splitlines()[0] if r.stdout else r.stderr.splitlines()[0]
        except Exception as e:
            identities[tool] = f"error: {e}"
    (cfg.res_dir / "TOOLCHAIN-IDENTITIES.json").write_text(json.dumps(identities, indent=2), encoding="utf-8")
    return identities

def execute_phase1_oracle(cfg: PathConfig) -> dict:
    print()
    print("=================================================================")
    print("PHASE 1: Native109 Machine Code Oracle Execution")
    print("=================================================================")
    oracle_dir = cfg.output_dir / "oracle_run"
    oracle_dir.mkdir(parents=True, exist_ok=True)

    oracle_cpp = oracle_dir / "native109_oracle.cpp"
    shutil.copy2(cfg.oracle_cpp_source, oracle_cpp)
    oracle_bin = oracle_dir / "native109_oracle"

    print(f"Compiling {oracle_cpp.name} with g++ -O2...")
    r_comp = run_logged(["g++", "-O2", str(oracle_cpp), "-o", str(oracle_bin)], oracle_dir / "compile_oracle")
    if r_comp.returncode != 0:
        print("ERROR: Oracle compilation failed!")
        print(r_comp.stderr)
        sys.exit(r_comp.returncode)

    tsv_out = oracle_dir / "native109_oracle.tsv"
    stderr_out = oracle_dir / "native109_oracle.stderr"
    print(f"Running oracle against {cfg.game_assembly.name}...")
    with open(tsv_out, "w", encoding="utf-8") as f_tsv, open(stderr_out, "w", encoding="utf-8") as f_err:
        r_run = subprocess.run([str(oracle_bin), str(cfg.game_assembly)], stdout=f_tsv, stderr=f_err)

    (oracle_dir / "run_oracle.exitcode.txt").write_text(str(r_run.returncode), encoding="utf-8")
    stderr_txt = stderr_out.read_text(encoding="utf-8", errors="replace")
    print(f"Oracle run finished with exit code: {r_run.returncode}")

    signal_audit_payload = None
    for l in stderr_txt.splitlines():
        if "ORACLE_SIGNAL_AUDIT" in l:
            try:
                raw_json = l[l.index("{"):]
                signal_audit_payload = json.loads(raw_json)
                print("Signal audit record:", signal_audit_payload)
            except Exception as e:
                print("Failed to parse ORACLE_SIGNAL_AUDIT line:", l, e)

    if signal_audit_payload is None:
        print("ERROR: Missing ORACLE_SIGNAL_AUDIT in oracle stderr output! Verification must fail.")
        sys.exit(98)

    unexpected_signal_count = signal_audit_payload.get("unexpected_signal_count", -1)
    if unexpected_signal_count != 0:
        print(f"ERROR: Oracle encountered {unexpected_signal_count} unexpected signals! Verification must fail.")
        sys.exit(97)

    tsv_lines = [l for l in tsv_out.read_text(encoding="utf-8", errors="replace").splitlines() if l.strip()]
    EXPECTED_TOKENS = {"0x06000115", "0x0600011A", "0x060001D3"}
    token_case_ids = defaultdict(list)
    for idx, l in enumerate(tsv_lines):
        parts = l.split()
        if len(parts) < 2:
            print(f"ERROR: Oracle TSV line {idx+1} malformed: {l}")
            sys.exit(99)
        tok = parts[0]
        try:
            cid = int(parts[1])
        except ValueError:
            print(f"ERROR: Oracle TSV line {idx+1} invalid case id: {parts[1]}")
            sys.exit(99)
        token_case_ids[tok].append(cid)

    actual_tokens = set(token_case_ids.keys())
    if actual_tokens != EXPECTED_TOKENS:
        print(f"ERROR: Oracle TSV tokens {actual_tokens} do not match expected {EXPECTED_TOKENS}!")
        sys.exit(99)

    expected_case_set = set(range(256))
    token_cases = {}
    for tok in sorted(EXPECTED_TOKENS):
        cids = token_case_ids[tok]
        if len(cids) != 256 or set(cids) != expected_case_set:
            print(f"ERROR: Token {tok} has {len(cids)} cases (unique: {len(set(cids))}), expected 0..255 exactly once!")
            sys.exit(99)
        token_cases[tok] = len(cids)

    print(f"Oracle TSV verified: exactly 3 tokens with case IDs 0..255 each (total {len(tsv_lines)} cases).")

    if r_run.returncode != 0:
        print(f"ERROR: Oracle failed execution with exit code {r_run.returncode}.")
        sys.exit(r_run.returncode)

    shutil.copy2(tsv_out, cfg.res_dir / "native109_oracle.tsv")
    shutil.copy2(stderr_out, cfg.res_dir / "native109_oracle.stderr")

    return {
        "tsv_path": tsv_out,
        "stderr_path": stderr_out,
        "exit_code": r_run.returncode,
        "signal_clean": (unexpected_signal_count == 0),
        "unexpected_signal_count": unexpected_signal_count,
        "signal_audit": signal_audit_payload,
        "line_count": len(tsv_lines),
        "token_cases": dict(token_cases)
    }

def execute_phase2_fixture(cfg: PathConfig, oracle_res: dict) -> dict:
    print()
    print("=================================================================")
    print("PHASE 2: Native109 CIL Fixture & Negative Controls")
    print("=================================================================")
    tsv_path = oracle_res["tsv_path"]
    fixture_dir = cfg.output_dir / "fixture_run"
    fixture_dir.mkdir(parents=True, exist_ok=True)

    fixture_cs = fixture_dir / "Native109Fixture.cs"
    shutil.copy2(cfg.fixture_cs_source, fixture_cs)

    csproj_content = f"""<Project Sdk="Microsoft.NET.Sdk">
  <PropertyGroup>
    <OutputType>Exe</OutputType>
    <TargetFramework>net10.0</TargetFramework>
    <Nullable>disable</Nullable>
  </PropertyGroup>
  <ItemGroup>
    <Reference Include="Mono.Cecil" HintPath="{cfg.mono_cecil_dll}" />
  </ItemGroup>
</Project>
"""
    (fixture_dir / "Native109Fixture.csproj").write_text(csproj_content, encoding="utf-8")

    bin_dir = fixture_dir / "bin"
    obj_dir = fixture_dir / "obj"
    print("Building Native109Fixture.csproj with dotnet...")
    bld = run_logged([
        "dotnet", "build", str(fixture_dir / "Native109Fixture.csproj"),
        f"-p:BaseIntermediateOutputPath={obj_dir}/",
        f"-p:IntermediateOutputPath={obj_dir}/",
        "-o", str(bin_dir), "-nologo"
    ], fixture_dir / "build_fixture")

    if bld.returncode != 0:
        print("ERROR: Fixture compilation failed!")
        print(bld.stderr)
        sys.exit(bld.returncode)

    fixture_dll = bin_dir / "Native109Fixture.dll"
    print(f"Running {fixture_dll.name} against candidate {cfg.candidate_dll.name}...")
    run_log = fixture_dir / "run_fixture"
    r_fix = run_logged(["dotnet", str(fixture_dll), str(cfg.candidate_dll), str(tsv_path)], run_log)
    print(r_fix.stdout)
    if r_fix.stderr:
        print("STDERR:", r_fix.stderr)

    pos_m = re.search(r"POSITIVE CASES EVALUATED:\s*(\d+),\s*PASS:\s*(\d+),\s*FAIL:\s*(\d+)", r_fix.stdout)
    if not pos_m:
        print("ERROR: Could not parse POSITIVE CASES EVALUATED from fixture stdout!")
        sys.exit(r_fix.returncode if r_fix.returncode != 0 else 101)

    total_pos = int(pos_m.group(1))
    pos_pass = int(pos_m.group(2))
    pos_fail = int(pos_m.group(3))

    if total_pos != oracle_res["line_count"]:
        print(f"ERROR: Fixture total cases ({total_pos}) does not match Oracle line count ({oracle_res['line_count']})!")
        sys.exit(101)
    if total_pos != (pos_pass + pos_fail):
        print(f"ERROR: Fixture total cases ({total_pos}) != pass ({pos_pass}) + fail ({pos_fail})!")
        sys.exit(101)
    if pos_fail != 0:
        print(f"ERROR: Fixture had {pos_fail} failing positive cases!")
        sys.exit(101)

    EXPECTED_MUTANT_NAMES = [
        "mut_0115_skip_countdown_check",
        "mut_011A_invert_threshold",
        "mut_011A_time_before_capture",
        "mut_01D3_omit_click",
        "mut_01D3_reuse_field"
    ]
    mutant_matches = re.findall(r"Mutant\s+(.*?)\s+on\s+(.*?):\s+detected deltas\s*=\s*(\d+)\s*/\s*(\d+)", r_fix.stdout)
    mutants_data = []
    seen_mutant_names = []
    for m_name, m_tok, m_det, m_tot in mutant_matches:
        d = int(m_det)
        t = int(m_tot)
        seen_mutant_names.append(m_name)
        mutants_data.append({
            "name": m_name,
            "token": m_tok,
            "detected_deltas": d,
            "total": t,
            "passed": d > 0
        })

    if sorted(seen_mutant_names) != sorted(EXPECTED_MUTANT_NAMES):
        print(f"ERROR: Parsed mutant names {seen_mutant_names} do not match expected {EXPECTED_MUTANT_NAMES}!")
        sys.exit(102)

    for m in mutants_data:
        if m["detected_deltas"] <= 0:
            print(f"ERROR: Mutant {m['name']} had 0 detected deltas!")
            sys.exit(102)

    summary_m = re.search(r"NEGATIVE CONTROL SUMMARY:\s*(\d+)\s*/\s*(\d+)\s*MUTANTS EFFECTIVELY DETECTED", r_fix.stdout)
    if not summary_m:
        print("ERROR: Could not parse NEGATIVE CONTROL SUMMARY from fixture stdout!")
        sys.exit(r_fix.returncode if r_fix.returncode != 0 else 102)

    mut_detected = int(summary_m.group(1))
    mut_total = int(summary_m.group(2))
    if mut_detected != 5 or mut_total != 5:
        print(f"ERROR: Expected 5/5 mutants detected, got {mut_detected}/{mut_total}!")
        sys.exit(102)

    all_mutants_passed = (mut_detected == 5 and all(m["passed"] for m in mutants_data))
    if r_fix.returncode != 0:
        print(f"ERROR: Native109 fixture test execution failed with exit code {r_fix.returncode}!")
        sys.exit(r_fix.returncode)

    methods_summary = {}
    for tok, ccount in oracle_res["token_cases"].items():
        methods_summary[tok] = {
            "token": tok,
            "cases": ccount,
            "status": "PASS" if pos_fail == 0 else "FAIL"
        }

    fixture_report = {
        "batch": "Native109",
        "verified_at_utc": datetime.now(timezone.utc).isoformat(),
        "candidate_sha256": sha256_file(cfg.candidate_dll),
        "candidate_bytes": cfg.candidate_dll.stat().st_size,
        "qualification_scope": "isolated_target_with_stubbed_dependencies",
        "integration_01D3_01E0_0677": "UNPROVEN",
        "formal_promotion_allowed": False,
        "qualification_note": (
            "Native109Fixture stubs MouseManager.GetZombieUnderMouse via UnderMouseZombie and ",
            "PrepareUIController.get_Instance via static field getter, while oracle hooks 0x18031ee10. ",
            "Real CIL/machine-code integration chain for 01D3->01E0->0677 remains unproven."
        ),
        "total_positive_cases": total_pos,
        "positive_pass": pos_pass,
        "positive_fail": pos_fail,
        "oracle_signal_clean": oracle_res["signal_clean"],
        "unexpected_signal_count": oracle_res["unexpected_signal_count"],
        "oracle_exit_code": oracle_res["exit_code"],
        "fixture_exit_code": r_fix.returncode,
        "methods": methods_summary,
        "mutants": mutants_data,
        "mutants_summary": {
            "total_mutants": mut_total,
            "detected_mutants": mut_detected,
            "all_controls_passed": all_mutants_passed
        }
    }

    (cfg.res_dir / "batches/N109-FIXTURE-REPORT.json").write_text(json.dumps(fixture_report, indent=2), encoding="utf-8")
    (cfg.res_dir / "N109-FIXTURE-REPORT.json").write_text(json.dumps(fixture_report, indent=2), encoding="utf-8")
    for tok, info in methods_summary.items():
        (cfg.res_dir / f"per_method/{tok}.json").write_text(json.dumps(info, indent=2), encoding="utf-8")

    print(f"Native109 verification reports saved: {total_pos} cases PASS, {mut_detected}/{mut_total} mutants detected.")
    return fixture_report

def execute_phase3_il2cpp(cfg: PathConfig) -> tuple[pathlib.Path, dict]:
    print()
    print("=================================================================")
    print("PHASE 3: Fresh Full IL2CPP Conversion")
    print("=================================================================")
    stage = cfg.output_dir / "il2cpp_stage"
    managed_dir = stage / "Library/Bee/artifacts/iOS/ManagedStripped"
    managed_dir.mkdir(parents=True, exist_ok=True)

    print(f"Staging support assemblies from {cfg.managed_support_dir}...")
    staged_map = {}
    for f in sorted(cfg.managed_support_dir.iterdir()):
        if f.is_file() and f.suffix.lower() in (".dll", ".exe"):
            if f.name.lower() == "godspvzruntime1.dll":
                continue
            shutil.copy2(f, managed_dir / f.name)
            staged_map[f.name] = {
                "name": f.name,
                "bytes": f.stat().st_size,
                "sha256": sha256_file(f)
            }

    cand_dest = managed_dir / "GodsPVZRuntime1.dll"
    shutil.copy2(cfg.candidate_dll, cand_dest)
    staged_map["GodsPVZRuntime1.dll"] = {
        "name": "GodsPVZRuntime1.dll",
        "bytes": cand_dest.stat().st_size,
        "sha256": sha256_file(cand_dest)
    }

    staged_files = [staged_map[k] for k in sorted(staged_map.keys())]
    print(f"Total unique staged assemblies: {len(staged_files)}")

    (cfg.res_dir / "il2cpp_conversion/COHERENT-PLAYER-INPUT-ASSEMBLIES.json").write_text(
        json.dumps({"count": len(staged_files), "files": staged_files}, indent=2), encoding="utf-8"
    )

    out_cpp = stage / "il2cppOutput/cpp"
    out_sym = stage / "il2cppOutput/cpp/Symbols"
    out_dat = stage / "il2cppOutput/data"
    out_cpp.mkdir(parents=True, exist_ok=True)
    out_sym.mkdir(parents=True, exist_ok=True)
    out_dat.mkdir(parents=True, exist_ok=True)

    if cfg.il2cpp_argv_json and cfg.il2cpp_argv_json.exists():
        print(f"Relocating authoritative argv from {cfg.il2cpp_argv_json}...")
        orig_argv = json.loads(cfg.il2cpp_argv_json.read_text(encoding="utf-8"))
        relocated_cmd = [str(cfg.il2cpp_exe)]
        for arg in orig_argv[1:]:
            if arg.startswith("--custom-il2-cpp-root="):
                relocated_cmd.append(f"--custom-il2-cpp-root={cfg.custom_il2cpp_root}")
            elif arg.startswith("--assembly="):
                asm_name = pathlib.Path(arg.split("=", 1)[1]).name
                target_asm = managed_dir / asm_name
                if not target_asm.exists():
                    source_cand = cfg.managed_support_dir / asm_name
                    if source_cand.exists():
                        shutil.copy2(source_cand, target_asm)
                    else:
                        raise FileNotFoundError(f"Required assembly in authoritative argv missing from stage: {asm_name}")
                relocated_cmd.append(f"--assembly={target_asm}")
            elif arg.startswith("--generatedcppdir="):
                relocated_cmd.append(f"--generatedcppdir={out_cpp}")
            elif arg.startswith("--symbols-folder="):
                relocated_cmd.append(f"--symbols-folder={out_sym}")
            elif arg.startswith("--data-folder="):
                relocated_cmd.append(f"--data-folder={out_dat}")
            else:
                relocated_cmd.append(arg)
        cmd = relocated_cmd
    else:
        print("Constructing IL2CPP command from uniquely staged assembly closure...")
        cmd = [
            str(cfg.il2cpp_exe),
            "--convert-to-cpp",
            f"--custom-il2-cpp-root={cfg.custom_il2cpp_root}"
        ]
        for sf in staged_files:
            cmd.append(f"--assembly={managed_dir / sf['name']}")
        cmd.extend([
            f"--generatedcppdir={out_cpp}",
            f"--symbols-folder={out_sym}",
            "--emit-null-checks",
            "--enable-array-bounds-check",
            "--dotnetprofile=unityaot-macos",
            f"--data-folder={out_dat}"
        ])

    print("Launching IL2CPP conversion process...")
    t0 = time.time()
    env = dict(os.environ, PROJECT_DIR=str(stage))
    r_conv = run_logged(cmd, cfg.res_dir / "il2cpp_conversion/il2cpp_convert", cwd=stage, env=env)
    dur = time.time() - t0
    print(f"IL2CPP completed in {dur:.1f}s with exit code: {r_conv.returncode}")

    if r_conv.returncode != 0:
        print("ERROR: IL2CPP conversion failed!")
        print(r_conv.stdout[-2000:])
        sys.exit(r_conv.returncode)

    all_cpps = list(out_cpp.glob("*.cpp"))
    gpvz_cpps = sorted(out_cpp.glob("GodsPVZRuntime1*.cpp"))
    meta_dat = out_dat / "Metadata/global-metadata.dat"
    if not meta_dat.exists():
        print("ERROR: global-metadata.dat was not generated!")
        sys.exit(104)

    print(f"IL2CPP SUCCESS! Total .cpp files: {len(all_cpps)}, GodsPVZRuntime1 game TUs: {len(gpvz_cpps)}")
    print(f"global-metadata.dat size={meta_dat.stat().st_size}, sha={sha256_file(meta_dat)}")

    il2cpp_info = {
        "exit_code": r_conv.returncode,
        "duration_seconds": round(dur, 2),
        "total_cpp_count": len(all_cpps),
        "game_tu_count": len(gpvz_cpps),
        "metadata_bytes": meta_dat.stat().st_size,
        "metadata_sha256": sha256_file(meta_dat),
        "meta_dat_path": meta_dat
    }
    return out_cpp, il2cpp_info

def execute_phase4_clang(cfg: PathConfig, out_cpp: pathlib.Path) -> dict:
    print()
    print("=================================================================")
    print("PHASE 4: Clang++ Syntax/Type Compilation & Token Attribution")
    print("=================================================================")
    gpvz_cpps = sorted(out_cpp.glob("GodsPVZRuntime1*.cpp"))
    all_cpps = sorted(out_cpp.glob("*.cpp"))

    clang_flags = [
        "clang++", "-std=c++11", "-fsyntax-only", "-ferror-limit=0",
        "-Wno-tautological-compare", "-Wno-unused-value", "-Wno-invalid-noreturn",
        "-DRUNTIME_IL2CPP", "-DIL2CPP_MONO_DEBUGGER_DISABLED", "-DIL2CPP_DEBUG=0",
        "-DNDEBUG", "-DBASELIB_INLINE_NAMESPACE=il2cpp_baselib",
        f"-I{out_cpp}",
        f"-I{cfg.native18_il2cpp}/libil2cpp/pch",
        f"-I{cfg.native18_il2cpp}/libil2cpp",
        f"-I{cfg.native18_il2cpp}/external/baselib/Include",
        f"-I{cfg.native18_il2cpp}/external/baselib/Platforms/Linux/Include"
    ]
    (cfg.res_dir / "CLANG-FLAGS.json").write_text(json.dumps(clang_flags, indent=2), encoding="utf-8")

    def compile_one_tu(p: pathlib.Path):
        log_file = cfg.clang_logs_dir / (p.name + ".log")
        t0 = time.time()
        with log_file.open("w", encoding="utf-8") as f:
            r = subprocess.run(clang_flags + [str(p)], stdout=f, stderr=subprocess.STDOUT)
        dur = time.time() - t0
        txt = log_file.read_text(encoding="utf-8", errors="replace")
        err_cnt = len([l for l in txt.splitlines() if ": error:" in l])
        fat_cnt = len([l for l in txt.splitlines() if ": fatal error:" in l])
        return {
            "file": p.name,
            "bytes": p.stat().st_size,
            "sha256": sha256_file(p),
            "exit_code": r.returncode,
            "error_count": err_cnt,
            "fatal_count": fat_cnt,
            "duration_seconds": round(dur, 2),
        }

    print(f"Compiling all {len(gpvz_cpps)} GodsPVZRuntime1*.cpp game TUs with parallel workers...")
    game_results = []
    workers = min(4, os.cpu_count() or 1)
    with concurrent.futures.ThreadPoolExecutor(max_workers=workers) as executor:
        futures = {executor.submit(compile_one_tu, p): p for p in gpvz_cpps}
        for f in concurrent.futures.as_completed(futures):
            res = f.result()
            game_results.append(res)
            print(f"  Game TU {res['file']}: exit {res['exit_code']}, errors {res['error_count']} ({res['duration_seconds']}s)", flush=True)

    game_results.sort(key=lambda x: x["file"])
    (cfg.res_dir / "GAME-TU-RESULTS.json").write_text(json.dumps(game_results, indent=2), encoding="utf-8")

    tu_exit_map = {r["file"]: r for r in game_results}
    total_game_errors = sum(r["error_count"] for r in game_results)
    total_game_failed = sum(r["exit_code"] != 0 for r in game_results)
    print(f"Total Game TU errors: {total_game_errors}, Failed Game TUs: {total_game_failed} / {len(game_results)}")

    codegen_text = (out_cpp / "GodsPVZRuntime1_CodeGen.c").read_text(encoding="utf-8", errors="replace")
    match_ptrs = re.search(r"s_methodPointers\[(\d+)\]\s*=\s*\{(.*?)\};", codegen_text, re.DOTALL)
    if not match_ptrs:
        print("ERROR: s_methodPointers table not found in CodeGen.c!")
        sys.exit(105)

    ptr_entries = [x.strip() for x in match_ptrs.group(2).split(",") if x.strip()]
    sym_to_tokens = defaultdict(list)
    for idx, sym in enumerate(ptr_entries):
        if sym not in ("NULL", "0"):
            tok = f"0x{0x06000001 + idx:08X}"
            sym_to_tokens[sym].append(tok)

    method_defs = {}
    tu_method_ranges = defaultdict(list)
    for p in gpvz_cpps:
        text = p.read_text(encoding="utf-8", errors="replace")
        lines = text.splitlines()
        for line_idx, line in enumerate(lines):
            stripped = line.strip()
            if stripped.startswith("IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR ") and "(" in stripped and not stripped.endswith(";"):
                sym_m = re.search(r"IL2CPP_METHOD_ATTR\s+.*?([A-Za-z0-9_]+_m[A-Fa-f0-9]{40})\s*\(", line)
                if sym_m:
                    sym = sym_m.group(1)
                    tokens = sym_to_tokens.get(sym, [])
                    start_l = line_idx + 1
                    tu_method_ranges[p.name].append((start_l, sym, tokens))

    for tu_name, rlist in tu_method_ranges.items():
        rlist.sort(key=lambda x: x[0])
        for i in range(len(rlist)):
            start_l, sym, tokens = rlist[i]
            end_l = rlist[i + 1][0] - 1 if i + 1 < len(rlist) else 999999
            for tok in tokens:
                method_defs[tok] = {
                    "token": tok,
                    "symbol": sym,
                    "file": tu_name,
                    "start_line": start_l,
                    "end_line": end_l,
                    "errors": []
                }

    print(f"Mapped {len(method_defs)} methods in C++ sources.")

    all_error_categories = Counter()
    sample_error_msgs = {}
    unmapped_cpp_errors = []
    included_file_errors = []
    unparseable_diagnostic_lines = []

    for r in game_results:
        log_p = cfg.clang_logs_dir / (r["file"] + ".log")
        txt = log_p.read_text(encoding="utf-8", errors="replace")
        r_ranges = tu_method_ranges[r["file"]]

        for line in txt.splitlines():
            if ": error:" in line or ": fatal error:" in line:
                m = re.match(r"^([^:]+):(\d+):(?:\d+:)? (?:fatal )?error: (.*)", line)
                if not m:
                    unparseable_diagnostic_lines.append({"tu": r["file"], "raw_line": line})
                    continue
                src_file = pathlib.Path(m.group(1)).name
                line_no = int(m.group(2))
                msg = m.group(3).strip()

                cat = "other"
                if "incompatible integer to pointer" in msg or "incompatible pointer to integer" in msg:
                    cat = "integer_pointer_mismatch"
                elif "comparison between pointer and integer" in msg:
                    cat = "pointer_integer_comparison"
                elif "no member named" in msg:
                    cat = "unresolved_struct_or_class_member"
                elif "cannot initialize" in msg:
                    cat = "type_initialization_mismatch"
                elif "no matching function" in msg:
                    cat = "no_matching_function_call"
                elif "undeclared identifier" in msg:
                    cat = "undeclared_identifier"
                elif "assigning to" in msg and "from incompatible type" in msg:
                    cat = "incompatible_pointer_assignment"
                elif "invalid static_cast" in msg or "cannot cast" in msg:
                    cat = "invalid_cast"
                elif "no viable overloaded" in msg:
                    cat = "no_viable_overloaded_operator"
                elif "invalid operands to binary expression" in msg:
                    cat = "invalid_binary_operands"

                all_error_categories[cat] += 1
                if cat not in sample_error_msgs:
                    sample_error_msgs[cat] = msg

                if src_file != r["file"]:
                    included_file_errors.append({"tu": r["file"], "header": src_file, "line": line_no, "message": msg})
                    continue

                matched_tok = None
                for idx in range(len(r_ranges)):
                    s_line, sym, tokens = r_ranges[idx]
                    e_line = r_ranges[idx + 1][0] - 1 if idx + 1 < len(r_ranges) else 999999
                    if s_line <= line_no <= e_line:
                        matched_tok = tokens[0] if tokens else None
                        break

                if matched_tok and matched_tok in method_defs:
                    method_defs[matched_tok]["errors"].append({
                        "line": line_no,
                        "category": cat,
                        "message": msg
                    })
                else:
                    unmapped_cpp_errors.append({
                        "file": r["file"],
                        "line": line_no,
                        "category": cat,
                        "message": msg
                    })

    methods_with_errors = {tok: info for tok, info in method_defs.items() if len(info["errors"]) > 0}
    clean_methods = {tok: info for tok, info in method_defs.items() if len(info["errors"]) == 0}

    (cfg.res_dir / "UNMAPPED-ERRORS.json").write_text(json.dumps(unmapped_cpp_errors, indent=2), encoding="utf-8")
    (cfg.res_dir / "INCLUDED-FILE-ERRORS.json").write_text(json.dumps(included_file_errors, indent=2), encoding="utf-8")
    (cfg.res_dir / "UNPARSEABLE-DIAGNOSTIC-LINES.json").write_text(json.dumps(unparseable_diagnostic_lines, indent=2), encoding="utf-8")

    roster_data = json.loads(cfg.roster_json.read_text(encoding="utf-8"))
    roster_tokens = roster_data.get("tokens", [])
    roster_analysis = []
    roster_clean_count = 0
    roster_errored_count = 0
    roster_in_failed_tu_count = 0

    for tok in roster_tokens:
        info = method_defs.get(tok)
        if not info:
            item = {"token": tok, "generated": False, "status": "NOT_GENERATED"}
        else:
            tu_file = info["file"]
            tu_res = tu_exit_map.get(tu_file, {})
            tu_exit = tu_res.get("exit_code", -1)
            tu_failed = (tu_exit != 0)
            errs = info["errors"]
            attributed_err_count = len(errs)

            if attributed_err_count == 0:
                roster_clean_count += 1
                status = "ATTRIBUTED_CLEAN_IN_FAILED_TU" if tu_failed else "ATTRIBUTED_CLEAN"
            else:
                roster_errored_count += 1
                status = "ATTRIBUTED_ERRORS"

            if tu_failed:
                roster_in_failed_tu_count += 1

            item = {
                "token": tok,
                "symbol": info["symbol"],
                "generated": True,
                "file": tu_file,
                "start_line": info["start_line"],
                "end_line": info["end_line"],
                "attributed_errors_count": attributed_err_count,
                "enclosing_tu_exit_code": tu_exit,
                "enclosing_tu_failed": tu_failed,
                "attributed_errors": errs,
                "status": status
            }
        roster_analysis.append(item)

    (cfg.res_dir / "ROSTER-108-ANALYSIS.json").write_text(json.dumps(roster_analysis, indent=2), encoding="utf-8")

    remaining_by_cause = defaultdict(list)
    for tok, info in methods_with_errors.items():
        cat_counts = Counter(e["category"] for e in info["errors"])
        primary = cat_counts.most_common(1)[0][0]
        remaining_by_cause[primary].append({
            "token": tok,
            "symbol": info["symbol"],
            "file": info["file"],
            "start_line": info["start_line"],
            "end_line": info["end_line"],
            "error_count": len(info["errors"]),
            "primary_cause": primary,
            "first_error_line": info["errors"][0]["line"],
            "first_error_message": info["errors"][0]["message"]
        })

    (cfg.res_dir / "REMAINING-METHODS-BY-ROOTCAUSE.json").write_text(json.dumps(remaining_by_cause, indent=2), encoding="utf-8")

    try:
        compiler_ver = subprocess.check_output(["clang++", "--version"], text=True).splitlines()[0]
    except Exception:
        compiler_ver = "clang++ (unknown version)"

    status_report = {
        "compiler": compiler_ver,
        "candidate_sha256": sha256_file(cfg.candidate_dll),
        "candidate_bytes": cfg.candidate_dll.stat().st_size,
        "attribution_method": "heuristic_next_definition",
        "attribution_method_note": "Method boundary spans from definition line until the next function definition start line (heuristic_next_definition), not an AST brace-balanced parser.",
        "total_generated_cpp_files": len(all_cpps),
        "godspvzruntime1_tu_count": len(gpvz_cpps),
        "godspvzruntime1_failing_tu_count": total_game_failed,
        "total_errors_in_game_tus": total_game_errors,
        "mapped_methods_count": len(method_defs),
        "methods_with_errors_count": len(methods_with_errors),
        "clean_methods_count": len(clean_methods),
        "unmapped_errors_count": len(unmapped_cpp_errors),
        "included_file_errors_count": len(included_file_errors),
        "unparseable_diagnostic_lines_count": len(unparseable_diagnostic_lines),
        "error_categories": dict(all_error_categories),
        "sample_error_messages": sample_error_msgs,
        "roster_108_evaluation": {
            "total_roster_methods": len(roster_tokens),
            "generated_and_mapped": sum(x.get("generated", False) for x in roster_analysis),
            "attributed_clean_count": roster_clean_count,
            "attributed_errored_count": roster_errored_count,
            "enclosing_tu_failed_count": roster_in_failed_tu_count,
            "all_attributed_clean": (roster_clean_count == len(roster_tokens) and roster_errored_count == 0)
        },
        "baseline_context": {
            "native53_historical_record": {
                "profile": "Historical Canary stripped mscorlib, stripped CoreModule",
                "raw_errors": 5668,
                "methods_with_errors": 516,
                "clean_methods": 1751
            },
            "native90_historical_record": {
                "profile": "Linux syntax check with stripped mscorlib + CoreModule with set_sharedMaterials",
                "raw_errors": 5430,
                "methods_with_errors": 393,
                "clean_methods": 1874
            },
            "bulk152_current_measured": {
                "profile": "Official coherent iOS Player profile",
                "raw_errors": total_game_errors,
                "methods_with_errors": len(methods_with_errors),
                "clean_methods": len(clean_methods),
                "failing_tu_count": total_game_failed,
                "total_tu_count": len(gpvz_cpps)
            }
        }
    }

    (cfg.res_dir / "CPP-STATUS.json").write_text(json.dumps(status_report, indent=2), encoding="utf-8")

    comp_status = "PASS_CLEAN" if total_game_failed == 0 else "FAILED_COMPILATION"
    (cfg.res_dir / "COMPILER-COMPLETE.json").write_text(
        json.dumps({
            "complete": True,
            "timestamp": time.time(),
            "status": comp_status,
            "total_game_failed": total_game_failed,
            "total_game_errors": total_game_errors
        }, indent=2),
        encoding="utf-8"
    )

    print(f"Clang compilation finished: {total_game_errors} errors across {len(methods_with_errors)} methods ({len(clean_methods)} clean, {total_game_failed} failing TUs).")

    return {
        "status_report": status_report,
        "total_game_failed": total_game_failed,
        "total_game_errors": total_game_errors,
        "mapped_methods_count": len(method_defs),
        "clean_methods_count": len(clean_methods),
        "methods_with_errors_count": len(methods_with_errors),
        "roster_clean_count": roster_clean_count,
        "roster_errored_count": roster_errored_count,
        "all_cpps_count": len(all_cpps),
        "gpvz_cpps_count": len(gpvz_cpps)
    }

def execute_phase5_package(cfg: PathConfig, out_cpp: pathlib.Path, oracle_res: dict, fixture_report: dict, il2cpp_info: dict, clang_res: dict) -> dict:
    print()
    print("=================================================================")
    print("PHASE 5: Packaging Artifacts & Generating Authoritative Manifest")
    print("=================================================================")

    evidence_zip_path = cfg.output_dir / "Archive-v1-evidence.zip"
    print(f"Creating evidence archive {evidence_zip_path.name}...")
    with zipfile.ZipFile(evidence_zip_path, "w", compression=zipfile.ZIP_DEFLATED) as zf:
        zf.write(cfg.candidate_dll, arcname="candidate1.dll")
        zf.write(oracle_res["tsv_path"], arcname="native109_oracle.tsv")
        zf.write(oracle_res["stderr_path"], arcname="native109_oracle.stderr")
        zf.write(cfg.oracle_cpp_source, arcname="native109_oracle.cpp")
        zf.write(cfg.fixture_cs_source, arcname="Native109Fixture.cs")
        zf.write(cfg.res_dir / "N109-FIXTURE-REPORT.json", arcname="N109-FIXTURE-REPORT.json")
        zf.write(cfg.roster_json, arcname="PRIMARY-ACCEPTANCE-2026-10-16.json")

        for f in sorted(cfg.res_dir.rglob("*")):
            if f.is_file():
                rel = f.relative_to(cfg.res_dir)
                zf.write(f, arcname=f"results/{rel.as_posix()}")

    game_sources_zip_path = cfg.output_dir / "Archive-v1-game-sources.zip"
    print(f"Creating game sources archive {game_sources_zip_path.name}...")
    with zipfile.ZipFile(game_sources_zip_path, "w", compression=zipfile.ZIP_DEFLATED) as zf:
        for p in sorted(out_cpp.glob("GodsPVZRuntime1*.cpp")):
            zf.write(p, arcname=f"cpp/{p.name}")
        codegen = out_cpp / "GodsPVZRuntime1_CodeGen.c"
        if codegen.exists():
            zf.write(codegen, arcname=f"cpp/{codegen.name}")
        meta_dat = il2cpp_info["meta_dat_path"]
        if meta_dat.exists():
            zf.write(meta_dat, arcname="data/Metadata/global-metadata.dat")

    manifest = {
        "version": "v1",
        "batch": "Native152-Takeover-Verification",
        "date": datetime.now(timezone.utc).strftime("%Y-%m-%d"),
        "candidate_sha256": sha256_file(cfg.candidate_dll),
        "candidate_bytes": cfg.candidate_dll.stat().st_size,
        "qualification": {
            "scope": "isolated_target_with_stubbed_dependencies",
            "integration_01D3_01E0_0677": "UNPROVEN",
            "formal_promotion_allowed": False,
            "finding": (
                "Native109Fixture stubs MouseManager.GetZombieUnderMouse via UnderMouseZombie and ",
                "PrepareUIController.get_Instance via static field getter; oracle hooks 0x18031ee10. ",
                "Real CIL/machine-code interaction chain for 01D3->01E0 and 0677 was not exercised."
            )
        },
        "native109_methods_verification": {
            "status": "PASS" if fixture_report["positive_fail"] == 0 and fixture_report["fixture_exit_code"] == 0 and oracle_res["signal_clean"] else "FAIL",
            "total_cases": fixture_report["total_positive_cases"],
            "oracle_signal_clean": oracle_res["signal_clean"],
            "unexpected_signal_count": oracle_res["unexpected_signal_count"],
            "positive_pass": fixture_report["positive_pass"],
            "positive_fail": fixture_report["positive_fail"],
            "negative_control_mutants_total": fixture_report["mutants_summary"]["total_mutants"],
            "negative_control_mutants_detected": fixture_report["mutants_summary"]["detected_mutants"]
        },
        "il2cpp_coherent_player_conversion": {
            "status": "PASS" if il2cpp_info["exit_code"] == 0 else "FAIL",
            "exit_code": il2cpp_info["exit_code"],
            "total_generated_cpp_files": il2cpp_info["total_cpp_count"],
            "godspvzruntime1_tu_files": il2cpp_info["game_tu_count"],
            "global_metadata_sha256": il2cpp_info["metadata_sha256"]
        },
        "clang_compilation_and_token_attribution": clang_res["status_report"],
        "archives": {
            "evidence_archive": {
                "name": evidence_zip_path.name,
                "bytes": evidence_zip_path.stat().st_size,
                "sha256": sha256_file(evidence_zip_path)
            },
            "game_sources_archive": {
                "name": game_sources_zip_path.name,
                "bytes": game_sources_zip_path.stat().st_size,
                "sha256": sha256_file(game_sources_zip_path)
            }
        }
    }

    manifest_path = cfg.output_dir / "MANIFEST-v1.json"
    manifest_path.write_text(json.dumps(manifest, indent=2), encoding="utf-8")
    print(f"Authoritative manifest saved to {manifest_path.name}.")
    print(f"Evidence archive: {evidence_zip_path.stat().st_size} bytes (SHA: {manifest['archives']['evidence_archive']['sha256']})")
    print(f"Game sources archive: {game_sources_zip_path.stat().st_size} bytes (SHA: {manifest['archives']['game_sources_archive']['sha256']})")

    return manifest

def parse_args():
    parser = argparse.ArgumentParser(description="GodsPVZ Native Bulk152 Actions Verification Runner")
    parser.add_argument("--portable-root", default=os.environ.get("PORTABLE_ROOT", "./portable_root"),
                        help="Path to unpacked portable root directory")
    parser.add_argument("--output-dir", default=os.environ.get("VERIFIER_OUTPUT_DIR", "./verifier_output"),
                        help="Path to brand-new, isolated output directory")
    parser.add_argument("--candidate-dll", default=None, help="Explicit path to candidate1.dll")
    parser.add_argument("--game-assembly-dll", default=None, help="Explicit path to authoritative GameAssembly.dll")
    parser.add_argument("--il2cpp-exe", default=None, help="Explicit path to il2cpp executable")
    parser.add_argument("--custom-il2cpp-root", default=None, help="Explicit path to custom il2cpp root")
    parser.add_argument("--native18-il2cpp", default=None, help="Explicit path to native18-IL2CPP directory")
    parser.add_argument("--mono-cecil-dll", default=None, help="Explicit path to Mono.Cecil.dll")
    parser.add_argument("--managed-support-dir", default=None, help="Explicit path to ManagedStripped support directory")
    parser.add_argument("--native109-oracle-cpp", default=None, help="Explicit path to native109_oracle.cpp")
    parser.add_argument("--native109-fixture-cs", default=None, help="Explicit path to Native109Fixture.cs")
    parser.add_argument("--roster-json", default=None, help="Explicit path to PRIMARY-ACCEPTANCE-2026-10-16.json")
    parser.add_argument("--il2cpp-argv-json", default=None, help="Explicit path to il2cpp_convert.argv.json")
    return parser.parse_args()

def main():
    args = parse_args()
    portable_root = pathlib.Path(args.portable_root).resolve()
    output_dir = pathlib.Path(args.output_dir).resolve()

    print("=================================================================")
    print("GodsPVZ Actions Verification Runner (Bulk152 Relocatable)")
    print(f"Portable root: {portable_root}")
    print(f"Output directory: {output_dir}")
    print("=================================================================")

    cfg = PathConfig(portable_root, output_dir, args)
    toolchain_info = record_toolchain_identities(cfg)
    print("Recorded toolchain identities:", toolchain_info)

    # Phase 1: Oracle
    oracle_res = execute_phase1_oracle(cfg)
    if oracle_res["exit_code"] != 0 or not oracle_res["signal_clean"] or oracle_res["unexpected_signal_count"] != 0:
        print(f"FATAL: Oracle phase failed (exit {oracle_res['exit_code']}, signals {oracle_res['unexpected_signal_count']})")
        sys.exit(oracle_res["exit_code"] if oracle_res["exit_code"] != 0 else 97)

    # Phase 2: Fixture
    fixture_report = execute_phase2_fixture(cfg, oracle_res)
    if fixture_report["fixture_exit_code"] != 0 or fixture_report["positive_fail"] > 0 or not fixture_report["mutants_summary"]["all_controls_passed"]:
        print(f"FATAL: Fixture phase failed (exit {fixture_report['fixture_exit_code']}, fail {fixture_report['positive_fail']})")
        sys.exit(fixture_report["fixture_exit_code"] if fixture_report["fixture_exit_code"] != 0 else 1)

    # Phase 3: IL2CPP Conversion
    out_cpp, il2cpp_info = execute_phase3_il2cpp(cfg)
    if il2cpp_info["exit_code"] != 0:
        print(f"FATAL: IL2CPP conversion failed (exit {il2cpp_info['exit_code']})")
        sys.exit(il2cpp_info["exit_code"])

    # Phase 4: Clang Compilation & Attribution
    clang_res = execute_phase4_clang(cfg, out_cpp)

    # Phase 5: Packaging
    manifest = execute_phase5_package(cfg, out_cpp, oracle_res, fixture_report, il2cpp_info, clang_res)

    print()
    print("=================================================================")
    print("VERIFICATION RUN COMPLETE SUMMARY")
    print("=================================================================")
    print(f"Candidate SHA-256: {manifest['candidate_sha256']}")
    print(f"Phase 1 Oracle: {oracle_res['line_count']} cases generated, unexpected signals: {oracle_res['unexpected_signal_count']}")
    print(f"Phase 2 Fixture: {fixture_report['positive_pass']} / {fixture_report['total_positive_cases']} PASS, mutants {fixture_report['mutants_summary']['detected_mutants']}/{fixture_report['mutants_summary']['total_mutants']} detected")
    print(f"Phase 3 IL2CPP: {il2cpp_info['game_tu_count']} game TUs, {il2cpp_info['total_cpp_count']} total CPPs, metadata SHA: {il2cpp_info['metadata_sha256'][:16]}...")
    print(f"Phase 4 Clang++: {clang_res['total_game_errors']} errors, {clang_res['clean_methods_count']} clean methods, {clang_res['methods_with_errors_count']} methods with errors")
    print(f"Phase 4 Roster 108: {clang_res['roster_clean_count']} attributed clean (in failed enclosing TUs), {clang_res['roster_errored_count']} attributed errors")
    print(f"Phase 5 Archives: {manifest['archives']['evidence_archive']['name']}, {manifest['archives']['game_sources_archive']['name']}")
    print("=================================================================")

    # Exit code policy: If Clang has ANY failed TU, exit non-zero (1)
    if clang_res["total_game_failed"] > 0:
        print(f"GATE RESULT: NON-ZERO EXIT REQUIRED ({clang_res['total_game_failed']} failing Game TUs).")
        sys.exit(1)
    else:
        print("GATE RESULT: ALL TUS CLEAN (ZERO FAILURES).")
        sys.exit(0)

if __name__ == "__main__":
    main()
