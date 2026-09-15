#!/usr/bin/env python3
from pathlib import Path
import re
import sys

BASELINE_SHA = "2b2fdf165076ff9f48c577b84fb4c62346b78c925e46c6bff5f8ec6a79f67ce5"
CURRENT_SHA = "68a105d366f0e7bfa9215c6e09040b9275bcad7e6c2771fec745340eb8e08d97"
UNITY = "2022.3.44f1c1"

if len(sys.argv) != 4:
    raise SystemExit("usage: reaudit-zombie-natural-runtime.py <baseline-evidence-dir> <current-evidence-dir> <output.txt>")

baseline = Path(sys.argv[1])
current = Path(sys.argv[2])
out = Path(sys.argv[3])


def read(root: Path, name: str) -> str:
    p = root / name
    if not p.is_file():
        raise FileNotFoundError(p)
    return p.read_text(errors="replace")


def candidate_sha(root: Path) -> str:
    m = re.fullmatch(r"candidate_sha256=([0-9a-f]{64})\s*", read(root, "input-gate.txt"))
    if not m:
        raise AssertionError(f"invalid input-gate.txt in {root}")
    return m.group(1)


def counts(text: str) -> dict[str, int]:
    lines = text.splitlines()
    return {
        "zombie_invalid": len(re.findall(r"InvalidProgramException: Invalid IL code in Zombie:\.ctor", text)),
        "zombie_fieldaccess": sum("FieldAccessException" in x and "Zombie:.ctor" in x for x in lines),
        "zombie_missingmethod": sum("MissingMethodException" in x and "Zombie:.ctor" in x for x in lines),
        "zombieinfo_invalid": len(re.findall(r"InvalidProgramException: Invalid IL code in ZombieInfo:\.ctor", text)),
        "device_fieldaccess": sum("FieldAccessException" in x and "Device:.ctor" in x for x in lines),
        "supplies_fieldaccess": sum("FieldAccessException" in x and "SuppliesInitialValue:.ctor" in x for x in lines),
        "almanac_zombie_window_invalid": len(re.findall(r"InvalidProgramException: Invalid IL code in Almanac_ZombieWindow:\.ctor", text)),
        "corelib_text": text.count("System.Private.CoreLib"),
        "zerovector_text": text.count("zeroVector"),
    }


b_sha = candidate_sha(baseline)
c_sha = candidate_sha(current)
assert b_sha == BASELINE_SHA, (b_sha, BASELINE_SHA)
assert c_sha == CURRENT_SHA, (c_sha, CURRENT_SHA)
assert UNITY in read(baseline, "unity-version.txt")
assert UNITY in read(current, "unity-version.txt")

b_log = read(baseline, "playmode.log")
c_log = read(current, "playmode.log")
b = counts(b_log)
c = counts(c_log)

# The two referenced workflow artifacts are immutable inputs. Pin the observed
# counts so a renamed/wrong artifact cannot silently satisfy this re-audit.
assert b["zombie_invalid"] == 26, b
assert c["zombie_invalid"] == 0, c
assert c["zombie_fieldaccess"] == 0, c
assert c["zombie_missingmethod"] == 0, c
assert b["corelib_text"] == c["corelib_text"] == 0, (b, c)
assert b["zerovector_text"] == c["zerovector_text"] == 0, (b, c)
assert b["zombieinfo_invalid"] == 12 and c["zombieinfo_invalid"] == 6, (b, c)
assert c["zombieinfo_invalid"] > 0, c

baseline_exit = read(baseline, "playmode-exit.txt").strip()
current_exit = read(current, "playmode-exit.txt").strip()
current_markers = [x for x in c_log.splitlines() if x.startswith("STAGE9_ZOMBIE_CTOR_")]

lines = [
    "STAGE9_ZOMBIE_CTOR_NATURAL_RUNTIME_REAUDIT",
    f"BASELINE_CANDIDATE_SHA256 {b_sha}",
    f"CURRENT_CANDIDATE_SHA256 {c_sha}",
    f"UNITY_VERSION {UNITY}",
    f"BASELINE_PLAYMODE_EXIT {baseline_exit}",
    f"CURRENT_PLAYMODE_EXIT {current_exit}",
    f"ZOMBIE_CTOR_INVALID_IL baseline={b['zombie_invalid']} current={c['zombie_invalid']}",
    f"ZOMBIE_CTOR_FIELDACCESS current={c['zombie_fieldaccess']}",
    f"ZOMBIE_CTOR_MISSINGMETHOD current={c['zombie_missingmethod']}",
    f"CORELIB_TEXT baseline={b['corelib_text']} current={c['corelib_text']}",
    f"ZEROVECTOR_TEXT baseline={b['zerovector_text']} current={c['zerovector_text']}",
    f"ZOMBIEINFO_CTOR_INVALID_IL baseline={b['zombieinfo_invalid']} current={c['zombieinfo_invalid']}",
    f"DEVICE_CTOR_FIELDACCESS baseline={b['device_fieldaccess']} current={c['device_fieldaccess']}",
    f"SUPPLIESINITIALVALUE_CTOR_FIELDACCESS baseline={b['supplies_fieldaccess']} current={c['supplies_fieldaccess']}",
    f"ALMANAC_ZOMBIEWINDOW_CTOR_INVALID_IL baseline={b['almanac_zombie_window_invalid']} current={c['almanac_zombie_window_invalid']}",
    f"DIRECT_ZOMBIE_MARKER_COUNT current={len(current_markers)}",
    "NEXT_SELECTED_UNRESOLVED_CTOR ZombieInfo note=preexisting_not_newly_introduced",
    "ZOMBIE_CTOR_NATURAL_RUNTIME_REAUDIT_PASS baseline_invalid=26 current_invalid=0 corelib_pollution=0 zerovector_pollution=0",
]
out.parent.mkdir(parents=True, exist_ok=True)
out.write_text("\n".join(lines) + "\n")
print(out.read_text(), end="")
