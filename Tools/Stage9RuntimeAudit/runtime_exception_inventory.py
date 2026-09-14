#!/usr/bin/env python3
"""Compact exception inventory for Stage9 Unity runtime logs.

This is intentionally evidence-only: it never decides which unrecovered exception
should be patched. It prevents audit scripts from hiding a changed failure mode by
showing every managed exception header, first stack frame, count, and first line.
"""
from __future__ import annotations

import argparse
import collections
import re
from pathlib import Path

HEADER = re.compile(r"^(?P<kind>[A-Za-z_][A-Za-z0-9_.+`]*Exception):\s*(?P<msg>.*)$")
STACK = re.compile(r"^\s+at\s+(?P<frame>.+)$")


def main() -> int:
    ap = argparse.ArgumentParser()
    ap.add_argument("log")
    ap.add_argument("--after-marker", default="STAGE9_STRICT_BOARD_PRESTART ")
    ap.add_argument("--top", type=int, default=80)
    ns = ap.parse_args()

    lines = Path(ns.log).read_text(errors="replace").splitlines()
    start = 0
    if ns.after_marker:
        for i, line in enumerate(lines):
            if line.startswith(ns.after_marker):
                start = i + 1
                break

    rows: dict[tuple[str, str, str], list[int]] = collections.defaultdict(list)
    current: tuple[str, str] | None = None
    current_line = 0
    first_frame = "<no-managed-frame>"

    def flush() -> None:
        nonlocal current, current_line, first_frame
        if current is not None:
            rows[(current[0], current[1], first_frame)].append(current_line)
        current = None
        current_line = 0
        first_frame = "<no-managed-frame>"

    for idx in range(start, len(lines)):
        line = lines[idx]
        m = HEADER.match(line)
        if m:
            flush()
            current = (m.group("kind"), m.group("msg"))
            current_line = idx + 1
            continue
        if current is not None and first_frame == "<no-managed-frame>":
            s = STACK.match(line)
            if s:
                first_frame = s.group("frame")
                continue
        if current is not None and not line.strip():
            flush()
    flush()

    print(f"EXCEPTION_INVENTORY_START_LINE {start + 1}")
    print(f"EXCEPTION_INVENTORY_UNIQUE {len(rows)}")
    print(f"EXCEPTION_INVENTORY_TOTAL {sum(len(v) for v in rows.values())}")
    ordered = sorted(rows.items(), key=lambda kv: (kv[1][0], kv[0]))
    for n, ((kind, msg, frame), locs) in enumerate(ordered[: ns.top], 1):
        safe_msg = msg.replace("\t", " ")
        safe_frame = frame.replace("\t", " ")
        print(f"EXCEPTION {n} count={len(locs)} first_line={locs[0]} kind={kind} msg={safe_msg}")
        print(f"  FIRST_FRAME {safe_frame}")
    if len(ordered) > ns.top:
        print(f"EXCEPTION_INVENTORY_TRUNCATED {len(ordered) - ns.top}")
    return 0


if __name__ == "__main__":
    raise SystemExit(main())
