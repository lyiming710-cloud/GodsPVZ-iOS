#!/usr/bin/env python3
"""Build the post-all24 orphan-Element-parent candidate.

Input must be the exact locked all24 candidate. This script changes only the
MemberRefParent coded index of C3/C4/C5, copying the already-active canonical
Enumerator<Element> parent from E7/E9/223 respectively. Names and signatures
must already match the canonical rows. No MethodDef, IL, TypeSpec, string or
blob bytes are modified.
"""
from __future__ import annotations

import argparse
import hashlib
import importlib.util
import json
from pathlib import Path

ROOT = Path(__file__).resolve().parents[2]
BASE_PATH = ROOT / "Tools" / "Stage9MemberRefAudit" / "build_all24_candidate.py"
spec = importlib.util.spec_from_file_location("stage9_all24_builder", BASE_PATH)
if spec is None or spec.loader is None:
    raise SystemExit("cannot import all24 metadata parser")
base = importlib.util.module_from_spec(spec)
spec.loader.exec_module(base)

ALL24 = "b07254d5c4077e013bbc934734f7b0949c0942a4e667d6d2b23d2af6c830f720"
EXPECTED_SIZE = 1_201_152
EXPECTED_METHODDEFS = 2317
EXPECTED_OLD_PARENT = (0x29 << 3) | 4  # TypeSpec 0x1B000029, MemberRefParent tag TypeSpec=4
EXPECTED_NEW_PARENT = (0x30 << 3) | 4  # TypeSpec 0x1B000030, canonical Enumerator<Element>
PAIRS = {
    0x0A0000C3: 0x0A0000E7,  # get_Current
    0x0A0000C4: 0x0A0000E9,  # MoveNext
    0x0A0000C5: 0x0A000223,  # Dispose
}


def sha256(data: bytes) -> str:
    return hashlib.sha256(data).hexdigest()


def main() -> int:
    ap = argparse.ArgumentParser()
    ap.add_argument("input")
    ap.add_argument("output")
    ap.add_argument("--manifest")
    a = ap.parse_args()
    src = Path(a.input).resolve()
    dst = Path(a.output).resolve()
    if src == dst:
        raise SystemExit("refusing in-place write")
    original = src.read_bytes()
    if len(original) != EXPECTED_SIZE:
        raise SystemExit(f"unexpected input size {len(original)}")
    before = sha256(original)
    if before != ALL24:
        raise SystemExit(f"input must be exact all24 {ALL24}, got {before}")

    data = bytearray(original)
    md = base.Metadata(data)
    if md.rows[6] != EXPECTED_METHODDEFS:
        raise SystemExit(f"MethodDef drift {md.rows[6]}")
    if md.memberref_parent != 2:
        raise SystemExit(f"unexpected MemberRefParent width {md.memberref_parent}")

    patches = []
    for bad_token, can_token in PAIRS.items():
        bad = md.memberref(bad_token)
        can = md.memberref(can_token)
        bad_name = md.string(bad["name"])
        can_name = md.string(can["name"])
        if bad_name != can_name:
            raise SystemExit(f"name mismatch 0x{bad_token:08X} {bad_name!r} vs canonical {can_name!r}")
        if md.blob(bad["signature"]) != md.blob(can["signature"]):
            raise SystemExit(f"signature mismatch 0x{bad_token:08X} vs canonical 0x{can_token:08X}")
        if bad["parent"] != EXPECTED_OLD_PARENT:
            raise SystemExit(f"0x{bad_token:08X} parent 0x{bad['parent']:x} != expected orphan 0x{EXPECTED_OLD_PARENT:x}")
        if can["parent"] != EXPECTED_NEW_PARENT:
            raise SystemExit(f"canonical 0x{can_token:08X} parent 0x{can['parent']:x} != expected Element 0x{EXPECTED_NEW_PARENT:x}")

        off = bad["parent_offset"]
        old = bytes(data[off:off + md.memberref_parent])
        md.write_ix(off, md.memberref_parent, can["parent"])
        new = bytes(data[off:off + md.memberref_parent])
        patches.append({
            "token": f"0x{bad_token:08X}",
            "canonical_token": f"0x{can_token:08X}",
            "name": bad_name,
            "file_offset": off,
            "width": md.memberref_parent,
            "old_parent_coded": f"0x{bad['parent']:x}",
            "new_parent_coded": f"0x{can['parent']:x}",
            "old": old.hex(),
            "new": new.hex(),
        })

    after_bytes = bytes(data)
    diffs = [i for i, (x, y) in enumerate(zip(original, after_bytes)) if x != y]
    expected_offsets = sorted(p["file_offset"] for p in patches)
    if diffs != expected_offsets:
        raise SystemExit(f"unexpected diff offsets {diffs}; expected {expected_offsets}")
    if len(diffs) != 3:
        raise SystemExit(f"expected exactly 3 changed byte positions, got {len(diffs)}")

    after = sha256(after_bytes)
    dst.parent.mkdir(parents=True, exist_ok=True)
    dst.write_bytes(after_bytes)
    manifest = {
        "schema": "stage9.1-orphan-element-parent-candidate-v1",
        "input_sha256": before,
        "output_sha256": after,
        "size": len(after_bytes),
        "methoddefs": md.rows[6],
        "changed_byte_positions": diffs,
        "patches": patches,
        "semantic_claim": "C3/C4/C5 only: reuse active canonical Enumerator<Element> MemberRefParent; no IL uses and no other bytes changed",
        "promotion": "candidate-only; strict Cecil Resolve/orphan and real Unity/IL2CPP still required",
    }
    if a.manifest:
        Path(a.manifest).write_text(json.dumps(manifest, indent=2) + "\n", encoding="utf-8")
    print(json.dumps(manifest, indent=2))
    return 0


if __name__ == "__main__":
    raise SystemExit(main())
