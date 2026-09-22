#!/usr/bin/env python3
"""Extract the minimum Unity managed resolver inputs from a *prefix* of the
preserved Unity China 2022.3.44f1c1 tar.xz archive.

The editor archive was split as raw consecutive 256 MiB chunks.  XZ can emit
all complete decompressed data that precedes a truncated tail, so this tool
scans the tar stream and stops immediately once the requested DLLs are found.
It never treats an expected truncated-stream error as proof of archive damage.

Usage:
  python3 extract_editor_managed_from_prefix.py PART_DIR OUT_DIR

PART_DIR must contain consecutive files named:
  Unity-China-2022.3.44f1c1.tar.xz.part-00
  Unity-China-2022.3.44f1c1.tar.xz.part-01
  ...

The default targets are basename matches for mscorlib.dll and
UnityEngine.CoreModule.dll.  All matching archive paths and SHA256 values are
recorded in OUT_DIR/extraction-report.json.
"""
from __future__ import annotations

import hashlib
import io
import json
import lzma
import os
from pathlib import Path
import re
import sys
import tarfile
from typing import BinaryIO

ARCHIVE_FULL_SHA256 = "0008115c785784baddb19e2b13b384ac845438239f293efb0c2e8b1379a1fe14"
ARCHIVE_FULL_SIZE = 3_906_640_940
PART_SIZE = 268_435_456
TARGETS = {"mscorlib.dll", "UnityEngine.CoreModule.dll"}
PART_RE = re.compile(r"^Unity-China-2022\.3\.44f1c1\.tar\.xz\.part-(\d{2})$")


def sha256_file(path: Path) -> str:
    h = hashlib.sha256()
    with path.open("rb") as f:
        for block in iter(lambda: f.read(1024 * 1024), b""):
            h.update(block)
    return h.hexdigest()


class ConcatenatedParts(io.RawIOBase):
    """Forward-only stream over raw split files without making a 1+ GiB copy."""

    def __init__(self, parts: list[Path]):
        super().__init__()
        self.parts = parts
        self.index = 0
        self.current: BinaryIO | None = None
        self.total_read = 0
        self._open_next()

    def readable(self) -> bool:
        return True

    def seekable(self) -> bool:
        return False

    def _open_next(self) -> bool:
        if self.current is not None:
            self.current.close()
            self.current = None
        if self.index >= len(self.parts):
            return False
        self.current = self.parts[self.index].open("rb")
        self.index += 1
        return True

    def readinto(self, b: bytearray | memoryview) -> int:
        view = memoryview(b)
        written = 0
        while written < len(view):
            if self.current is None:
                break
            n = self.current.readinto(view[written:])
            if n:
                written += n
                self.total_read += n
                continue
            if not self._open_next():
                break
        return written

    def close(self) -> None:
        if self.current is not None:
            self.current.close()
            self.current = None
        super().close()


def discover_parts(root: Path) -> list[Path]:
    found: list[tuple[int, Path]] = []
    for p in root.iterdir():
        m = PART_RE.match(p.name)
        if m and p.is_file():
            found.append((int(m.group(1)), p))
    found.sort()
    if not found:
        raise SystemExit(f"no preserved editor parts found in {root}")
    expected = list(range(found[-1][0] + 1))
    actual = [i for i, _ in found]
    if actual != expected:
        raise SystemExit(f"parts are not a contiguous prefix starting at 00: {actual}")
    for i, p in found[:-1]:
        if p.stat().st_size != PART_SIZE:
            raise SystemExit(f"part {i:02d} has unexpected size {p.stat().st_size}")
    return [p for _, p in found]


def safe_output_name(member_name: str) -> str:
    # Preserve basename only; report keeps the exact archive path.
    return Path(member_name).name


def main() -> int:
    if len(sys.argv) != 3:
        print(f"usage: {Path(sys.argv[0]).name} PART_DIR OUT_DIR", file=sys.stderr)
        return 2
    part_dir = Path(sys.argv[1]).resolve()
    out_dir = Path(sys.argv[2]).resolve()
    out_dir.mkdir(parents=True, exist_ok=True)

    parts = discover_parts(part_dir)
    report: dict[str, object] = {
        "schema": 1,
        "expected_full_archive_sha256": ARCHIVE_FULL_SHA256,
        "expected_full_archive_size": ARCHIVE_FULL_SIZE,
        "part_count_available": len(parts),
        "prefix_size": sum(p.stat().st_size for p in parts),
        "parts": [
            {"name": p.name, "size": p.stat().st_size, "sha256": sha256_file(p)}
            for p in parts
        ],
        "targets": sorted(TARGETS),
        "matches": [],
        "complete_targets": False,
        "stream_end": None,
    }

    found: dict[str, dict[str, object]] = {}
    concat = ConcatenatedParts(parts)
    buffered = io.BufferedReader(concat, buffer_size=1024 * 1024)
    xz = lzma.LZMAFile(buffered, mode="rb")
    tar = tarfile.open(fileobj=xz, mode="r|")

    try:
        for member in tar:
            base = Path(member.name).name
            if base not in TARGETS or not member.isfile():
                continue
            source = tar.extractfile(member)
            if source is None:
                continue
            data = source.read()
            out = out_dir / safe_output_name(member.name)
            # Do not silently overwrite a different match with the same basename.
            digest = hashlib.sha256(data).hexdigest()
            if out.exists() and sha256_file(out) != digest:
                out = out_dir / (base + "." + digest[:12])
            out.write_bytes(data)
            rec = {
                "basename": base,
                "archive_path": member.name,
                "size": len(data),
                "sha256": digest,
                "output": out.name,
            }
            found[base] = rec
            cast_matches = report["matches"]
            assert isinstance(cast_matches, list)
            cast_matches.append(rec)
            print(f"FOUND {base} path={member.name} size={len(data)} sha256={digest}")
            if TARGETS.issubset(found):
                report["complete_targets"] = True
                report["stream_end"] = "stopped-after-all-targets-found"
                break
        else:
            report["stream_end"] = "tar-stream-ended-before-all-targets"
    except (EOFError, lzma.LZMAError, tarfile.ReadError) as exc:
        # With a raw prefix, truncated-tail failure is expected.  It is only a
        # successful extraction when every requested target was already emitted.
        report["stream_end"] = f"expected-prefix-truncation:{type(exc).__name__}:{exc}"
    finally:
        try:
            tar.close()
        except Exception:
            pass
        try:
            xz.close()
        except Exception:
            pass
        try:
            buffered.close()
        except Exception:
            pass

    missing = sorted(TARGETS.difference(found))
    report["missing_targets"] = missing
    report_path = out_dir / "extraction-report.json"
    report_path.write_text(json.dumps(report, indent=2, sort_keys=True) + "\n", encoding="utf-8")
    print(f"REPORT {report_path}")
    if missing:
        print("NEED_MORE_PREFIX_PARTS " + ",".join(missing), file=sys.stderr)
        return 3
    print("STAGE9_EDITOR_MANAGED_PREFIX_EXTRACT_OK")
    return 0


if __name__ == "__main__":
    raise SystemExit(main())
