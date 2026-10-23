#!/usr/bin/env python3
"""
GodsPVZ Bulk250-R2 Behavioral Execution Runner.
Invokes the authentic C# Cecil / Reflection.Emit / CLR DynamicMethod fixture:
scripts/actions/Bulk250R2BehaviorFixture.csproj
"""

import argparse
import hashlib
import json
import os
import pathlib
import subprocess
import sys

def parse_args():
    parser = argparse.ArgumentParser(description="Candidate Behavioral Evaluation Runner (Bulk250-R2)")
    parser.add_argument("--candidate-dll", required=True, help="Path to candidate1.dll")
    parser.add_argument("--output-json", required=True, help="Path to output report JSON")
    return parser.parse_args()

def main():
    args = parse_args()
    cand_path = pathlib.Path(args.candidate_dll).resolve()
    out_json = pathlib.Path(args.output_json).resolve()

    if not cand_path.exists():
        print(f"ERROR: Candidate DLL not found at: {cand_path}")
        sys.exit(2)

    script_dir = pathlib.Path(__file__).resolve().parent
    csproj_path = script_dir / "Bulk250R2BehaviorFixture.csproj"

    if not csproj_path.exists():
        print(f"ERROR: Fixture project not found at: {csproj_path}")
        sys.exit(3)

    print("-----------------------------------------------------------------")
    print("Executing Isolated Subprocess Gate Controls...")
    print("-----------------------------------------------------------------")

    gate_negative_controls = []
    neg_modes = [
        ("neg_assertion_failure", 10, None),
        ("neg_escaped_mutant", 12, None),
        ("neg_unsupported_opcode", None, "NotSupportedException"),
        ("neg_unexpected_exception", None, "DivideByZeroException"),
        ("neg_wrong_sha", 3, "Candidate SHA-256 mismatch"),
    ]

    for mode, expected_exit, expected_stderr_token in neg_modes:
        dummy_out = out_json.parent / f"report_{mode}.json"
        cmd_neg = ["dotnet", "run", "--project", str(csproj_path), "--", str(cand_path), str(dummy_out), f"--mode={mode}"]
        p_neg = subprocess.run(cmd_neg, capture_output=True, text=True)

        passed = False
        if expected_exit is not None:
            passed = (p_neg.returncode == expected_exit)
        elif expected_stderr_token is not None:
            passed = (p_neg.returncode != 0 and expected_stderr_token in p_neg.stderr)

        gate_negative_controls.append({
            "mode": mode,
            "argv": cmd_neg,
            "expected_exit": expected_exit,
            "observed_exit": p_neg.returncode,
            "expected_stderr_token": expected_stderr_token,
            "passed": passed,
            "stdout": p_neg.stdout,
            "stderr": p_neg.stderr
        })

        status_str = "PASSED (rejected as required)" if passed else "FAILED TO REJECT"
        print(f"  Negative Control [{mode}]: Exit code {p_neg.returncode} -> {status_str}")
        if not passed:
            print(f"GATE CONTROL FAILURE: Mode {mode} did not satisfy rejection requirements!")
            print("STDOUT: " + p_neg.stdout)
            print("STDERR: " + p_neg.stderr)
            sys.exit(20)

    print("All isolated gate controls verified successfully.")

    print("-----------------------------------------------------------------")
    print("Launching C# Real Cecil / CLR DynamicMethod Behavioral Fixture...")
    print(f"Project: {csproj_path}")
    print(f"Candidate: {cand_path}")
    print(f"Output: {out_json}")
    print("-----------------------------------------------------------------")

    cmd = ["dotnet", "run", "--project", str(csproj_path), "--", str(cand_path), str(out_json), "--mode=normal"]
    p = subprocess.run(cmd, capture_output=False)
    if p.returncode != 0:
        print(f"ERROR: Fixture execution failed with exit code: {p.returncode}")
        sys.exit(p.returncode)

    if not out_json.exists():
        print(f"ERROR: Fixture completed but output report was not created at: {out_json}")
        sys.exit(4)

    report = json.loads(out_json.read_text(encoding="utf-8"))
    report["gate_negative_controls"] = gate_negative_controls
    out_json.write_text(json.dumps(report, indent=2), encoding="utf-8")

    summary = report.get("summary", {})
    print("Behavioral Qualification Summary:")
    print(json.dumps(summary, indent=2))

    if summary.get("methods_failed", 0) > 0:
        print(f"GATE REJECTION: {summary['methods_failed']} methods failed assertions!")
        sys.exit(10)

    if summary.get("methods_genuinely_passed", 0) == 0:
        print("GATE REJECTION: Zero methods genuinely passed!")
        sys.exit(11)

    if summary.get("total_mutants_detected", 0) < summary.get("total_mutants_tested", 0):
        print("GATE REJECTION: One or more mutants escaped detection!")
        sys.exit(12)

    print("Gate Validation Passed: Real CIL execution chain and mutant detections verified.")
    sys.exit(0)

if __name__ == "__main__":
    main()
