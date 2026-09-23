#!/usr/bin/env bash
set -euo pipefail

if [[ $# -lt 2 || $# -gt 3 ]]; then
  echo "usage: $0 <GodsPVZ_1.0.2.zip> <out_dir> [repo_root]" >&2
  exit 2
fi

ZIP_PATH="$1"
OUT_DIR="$2"
REPO_ROOT="${3:-$(cd "$(dirname "$0")/../.." && pwd)}"

PREP="$REPO_ROOT/Tools/Stage9NativeEvidence/prepare_pc_input.py"
EXTRACT="$REPO_ROOT/Tools/Stage9LogicExceptionAudit/extract_pc_native_ranges.py"
SPEC="$REPO_ROOT/Recovery/Stage9.1-native-extraction-priority-v1.json"
PC_DIR="$OUT_DIR/pc-input"
NATIVE_DIR="$OUT_DIR/native-priority"

for f in "$PREP" "$EXTRACT" "$SPEC"; do
  [[ -f "$f" ]] || { echo "required file missing: $f" >&2; exit 3; }
done

mkdir -p "$OUT_DIR"
python3 "$PREP" "$ZIP_PATH" "$PC_DIR"
python3 "$EXTRACT" "$PC_DIR/GameAssembly.dll" "$SPEC" "$NATIVE_DIR" --run-objdump

python3 - "$NATIVE_DIR/manifest.json" <<'PY'
import json
import sys
from pathlib import Path

p = Path(sys.argv[1])
obj = json.loads(p.read_text(encoding="utf-8"))
assert obj.get("schema") == "stage9.1-pc-native-extraction-output-v1", obj.get("schema")
groups = obj.get("groups", [])
assert len(groups) == 35, f"expected 35 unique native VA groups, got {len(groups)}"
targets = [t for g in groups for t in g.get("targets", [])]
assert len(targets) == 36, f"expected 36 managed MethodDef targets, got {len(targets)}"
ids = [t.get("id") for t in targets]
assert len(ids) == len(set(ids)), "duplicate target ids in native manifest"
assert all(g.get("classification") == "UNREVIEWED_NATIVE_CONTROL_FLOW" for g in groups)
shared = [g for g in groups if len(g.get("targets", [])) > 1]
assert len(shared) == 1, f"expected one shared native VA group, got {len(shared)}"
assert {t.get("id") for t in shared[0]["targets"]} == {"L005", "L007"}
print("PRIORITY_NATIVE_MANIFEST_GATE_OK")
print(f"TARGET_METHODDEFS={len(targets)}")
print(f"UNIQUE_NATIVE_VA_GROUPS={len(groups)}")
PY

cat > "$OUT_DIR/GATE-SUMMARY.txt" <<EOF
STAGE9_PRIORITY_NATIVE_GATE_PREPARATION_OK
Source ZIP: $ZIP_PATH
Locked PC payload: $PC_DIR/GameAssembly.dll
Metadata: $PC_DIR/global-metadata.dat
Native evidence manifest: $NATIVE_DIR/manifest.json
Expected managed targets: 36
Expected unique native VA groups: 35

IMPORTANT: extraction success is not semantic promotion. Every group remains
UNREVIEWED_NATIVE_CONTROL_FLOW until the native disassembly is manually checked
against the recovered managed exception tail. No managed DLL is modified here.
EOF

cat "$OUT_DIR/GATE-SUMMARY.txt"
