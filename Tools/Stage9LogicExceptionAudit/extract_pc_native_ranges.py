#!/usr/bin/env python3
"""Read-only PE/x64 native evidence extractor for Stage9.1 exception-return audit.

This tool never modifies GameAssembly.dll or a managed DLL.  It hash-gates the
original PC GameAssembly, parses PE32+ sections and the x64 exception directory
(.pdata / RUNTIME_FUNCTION records), maps configured target VAs to file bytes,
and emits bounded evidence windows plus a machine-readable manifest.

A contiguous sequence of RUNTIME_FUNCTION records is reported only as a
candidate unwind/funclet chain.  It is NOT treated as a managed-method boundary.
"""

from __future__ import annotations

import argparse
import hashlib
import json
import os
from pathlib import Path
import shutil
import struct
import subprocess
import sys
from typing import Any

EXPECTED_GAMEASSEMBLY_SHA256 = (
    "9ebd7ca996a5b03fb4a766f7a2502b660d581ddbbb36af4d7bf2f06a211da39d"
)
DEFAULT_WINDOW = 0x2000
MAX_CONTIGUOUS_RUNTIME_FUNCTIONS = 24


def die(message: str) -> "NoReturn":
    raise SystemExit(f"ERROR: {message}")


def sha256_file(path: Path) -> str:
    h = hashlib.sha256()
    with path.open("rb") as f:
        for chunk in iter(lambda: f.read(1024 * 1024), b""):
            h.update(chunk)
    return h.hexdigest()


def u16(buf: bytes, off: int) -> int:
    return struct.unpack_from("<H", buf, off)[0]


def u32(buf: bytes, off: int) -> int:
    return struct.unpack_from("<I", buf, off)[0]


def u64(buf: bytes, off: int) -> int:
    return struct.unpack_from("<Q", buf, off)[0]


def checked_slice(buf: bytes, off: int, size: int, label: str) -> bytes:
    if off < 0 or size < 0 or off + size > len(buf):
        die(f"{label} is outside file bounds: off=0x{off:X} size=0x{size:X}")
    return buf[off : off + size]


def parse_pe(buf: bytes) -> dict[str, Any]:
    if len(buf) < 0x100:
        die("input is too small to be a PE image")
    if buf[:2] != b"MZ":
        die("missing DOS MZ signature")

    pe_off = u32(buf, 0x3C)
    checked_slice(buf, pe_off, 24, "PE headers")
    if buf[pe_off : pe_off + 4] != b"PE\0\0":
        die("missing PE signature")

    coff = pe_off + 4
    machine = u16(buf, coff)
    section_count = u16(buf, coff + 2)
    size_optional = u16(buf, coff + 16)
    if machine != 0x8664:
        die(f"unexpected COFF Machine 0x{machine:04X}; expected AMD64 0x8664")
    if section_count == 0:
        die("PE has zero sections")

    opt = coff + 20
    checked_slice(buf, opt, size_optional, "optional header")
    magic = u16(buf, opt)
    if magic != 0x20B:
        die(f"unexpected optional-header magic 0x{magic:04X}; expected PE32+ 0x20B")
    if size_optional < 112:
        die("PE32+ optional header is too short for data directories")

    image_base = u64(buf, opt + 24)
    size_of_image = u32(buf, opt + 56)
    size_of_headers = u32(buf, opt + 60)
    number_of_dirs = u32(buf, opt + 108)
    if number_of_dirs <= 3:
        die("PE does not expose an exception data directory")

    data_dirs = opt + 112
    exception_rva = u32(buf, data_dirs + 3 * 8)
    exception_size = u32(buf, data_dirs + 3 * 8 + 4)
    if exception_rva == 0 or exception_size < 12:
        die("x64 exception directory is absent or empty")

    section_table = opt + size_optional
    checked_slice(buf, section_table, section_count * 40, "section table")
    sections: list[dict[str, Any]] = []
    for i in range(section_count):
        off = section_table + i * 40
        raw_name = buf[off : off + 8].split(b"\0", 1)[0]
        name = raw_name.decode("ascii", "replace")
        virtual_size = u32(buf, off + 8)
        virtual_address = u32(buf, off + 12)
        raw_size = u32(buf, off + 16)
        raw_pointer = u32(buf, off + 20)
        if raw_size:
            checked_slice(buf, raw_pointer, raw_size, f"section {name} raw data")
        sections.append(
            {
                "name": name,
                "virtual_size": virtual_size,
                "virtual_address": virtual_address,
                "raw_size": raw_size,
                "raw_pointer": raw_pointer,
            }
        )

    return {
        "pe_offset": pe_off,
        "machine": machine,
        "section_count": section_count,
        "image_base": image_base,
        "size_of_image": size_of_image,
        "size_of_headers": size_of_headers,
        "exception_rva": exception_rva,
        "exception_size": exception_size,
        "sections": sections,
    }


def rva_to_file_offset(rva: int, sections: list[dict[str, Any]], file_size: int) -> int:
    for s in sections:
        start = int(s["virtual_address"])
        # Only raw bytes can be materialized from the file. Do not map the
        # zero-filled virtual tail beyond SizeOfRawData.
        end = start + int(s["raw_size"])
        if start <= rva < end:
            off = int(s["raw_pointer"]) + (rva - start)
            if off >= file_size:
                die(f"RVA 0x{rva:X} maps outside file")
            return off
    die(f"RVA 0x{rva:X} is not backed by raw section data")


def section_for_rva(rva: int, sections: list[dict[str, Any]]) -> dict[str, Any]:
    for s in sections:
        start = int(s["virtual_address"])
        end = start + max(int(s["virtual_size"]), int(s["raw_size"]))
        if start <= rva < end:
            return s
    die(f"RVA 0x{rva:X} is outside all sections")


def parse_runtime_functions(buf: bytes, pe: dict[str, Any]) -> list[dict[str, int]]:
    ex_rva = int(pe["exception_rva"])
    ex_size = int(pe["exception_size"])
    ex_off = rva_to_file_offset(ex_rva, pe["sections"], len(buf))
    checked_slice(buf, ex_off, ex_size, "exception directory")

    records: list[dict[str, int]] = []
    for rel in range(0, ex_size - (ex_size % 12), 12):
        begin, end, unwind = struct.unpack_from("<III", buf, ex_off + rel)
        if begin == 0 and end == 0 and unwind == 0:
            continue
        if begin >= end:
            die(
                "invalid RUNTIME_FUNCTION record at exception+"
                f"0x{rel:X}: begin=0x{begin:X} end=0x{end:X}"
            )
        records.append({"begin_rva": begin, "end_rva": end, "unwind_rva": unwind})

    records.sort(key=lambda x: (x["begin_rva"], x["end_rva"], x["unwind_rva"]))
    if not records:
        die("no nonzero RUNTIME_FUNCTION records parsed")
    return records


def find_runtime_record(rva: int, records: list[dict[str, int]]) -> tuple[int, dict[str, int]] | None:
    matches = [(i, r) for i, r in enumerate(records) if r["begin_rva"] <= rva < r["end_rva"]]
    if len(matches) > 1:
        die(f"RVA 0x{rva:X} is covered by multiple RUNTIME_FUNCTION records")
    return matches[0] if matches else None


def contiguous_runtime_chain(index: int, records: list[dict[str, int]]) -> list[dict[str, int]]:
    left = index
    right = index
    while left > 0 and right - left + 1 < MAX_CONTIGUOUS_RUNTIME_FUNCTIONS:
        prev = records[left - 1]
        cur = records[left]
        if prev["end_rva"] != cur["begin_rva"]:
            break
        left -= 1
    while right + 1 < len(records) and right - left + 1 < MAX_CONTIGUOUS_RUNTIME_FUNCTIONS:
        cur = records[right]
        nxt = records[right + 1]
        if cur["end_rva"] != nxt["begin_rva"]:
            break
        right += 1
    return records[left : right + 1]


def hex_int(value: str | int) -> int:
    if isinstance(value, int):
        return value
    return int(value, 0)


def safe_stem(targets: list[dict[str, Any]], va: int) -> str:
    ids = "_".join(str(t["id"]) for t in targets)
    return f"{ids}_VA_{va:016X}"


def maybe_disassemble(binary: Path, start_va: int, stop_va: int, output: Path) -> dict[str, Any]:
    tool = shutil.which("llvm-objdump") or shutil.which("objdump")
    if not tool:
        return {"status": "not_run", "reason": "llvm-objdump/objdump not found"}

    command = [
        tool,
        "-d",
        f"--start-address=0x{start_va:X}",
        f"--stop-address=0x{stop_va:X}",
        str(binary),
    ]
    proc = subprocess.run(command, capture_output=True, text=True, check=False)
    output.write_text(proc.stdout + ("\n[stderr]\n" + proc.stderr if proc.stderr else ""), encoding="utf-8")
    return {
        "status": "ok" if proc.returncode == 0 else "failed",
        "tool": tool,
        "returncode": proc.returncode,
        "command": command,
        "output": output.name,
    }


def main() -> int:
    ap = argparse.ArgumentParser()
    ap.add_argument("gameassembly", type=Path)
    ap.add_argument("spec", type=Path)
    ap.add_argument("outdir", type=Path)
    ap.add_argument("--window", type=lambda x: int(x, 0), default=DEFAULT_WINDOW)
    ap.add_argument("--run-objdump", action="store_true")
    args = ap.parse_args()

    if args.window <= 0 or args.window > 0x20000:
        die("--window must be in range 1..0x20000")
    if not args.gameassembly.is_file():
        die(f"GameAssembly not found: {args.gameassembly}")
    if not args.spec.is_file():
        die(f"spec not found: {args.spec}")

    source_hash = sha256_file(args.gameassembly)
    if source_hash.lower() != EXPECTED_GAMEASSEMBLY_SHA256:
        die(
            "original PC GameAssembly SHA256 mismatch: "
            f"got {source_hash}, expected {EXPECTED_GAMEASSEMBLY_SHA256}"
        )

    spec = json.loads(args.spec.read_text(encoding="utf-8"))
    spec_hash = str(spec.get("source", {}).get("sha256", "")).lower()
    if spec_hash != EXPECTED_GAMEASSEMBLY_SHA256:
        die("spec source SHA256 does not match the locked original PC GameAssembly")

    buf = args.gameassembly.read_bytes()
    pe = parse_pe(buf)
    expected_base = hex_int(spec.get("source", {}).get("expected_image_base", "0x0"))
    if expected_base and int(pe["image_base"]) != expected_base:
        die(
            f"image base mismatch: got 0x{int(pe['image_base']):X}, "
            f"expected 0x{expected_base:X}"
        )

    records = parse_runtime_functions(buf, pe)
    targets = list(spec.get("targets", []))
    if not targets:
        die("spec has no targets")

    # Group shared IL2CPP native bodies by exact VA.  L005/L007 are expected to
    # collapse into one extraction group while retaining both managed tokens.
    by_va: dict[int, list[dict[str, Any]]] = {}
    seen_tokens: set[str] = set()
    for t in targets:
        token = str(t.get("token", "")).lower()
        if not token or token in seen_tokens:
            die(f"missing or duplicate target token: {token!r}")
        seen_tokens.add(token)
        va = hex_int(t["pc_va"])
        by_va.setdefault(va, []).append(t)

    args.outdir.mkdir(parents=True, exist_ok=True)
    manifest: dict[str, Any] = {
        "schema": "stage9.1-pc-native-extraction-output-v1",
        "source": {
            "path": str(args.gameassembly),
            "sha256": source_hash,
            "file_size": len(buf),
        },
        "pe": {
            "machine": f"0x{int(pe['machine']):04X}",
            "image_base": f"0x{int(pe['image_base']):X}",
            "size_of_image": int(pe["size_of_image"]),
            "exception_rva": f"0x{int(pe['exception_rva']):X}",
            "exception_size": int(pe["exception_size"]),
            "runtime_function_count": len(records),
            "sections": pe["sections"],
        },
        "warning": (
            "contiguous_runtime_chain is unwind-layout evidence only and must not "
            "be treated as a managed-method boundary"
        ),
        "groups": [],
    }

    for va in sorted(by_va):
        group_targets = by_va[va]
        rva = va - int(pe["image_base"])
        if rva < 0:
            die(f"target VA 0x{va:X} is below image base")
        sec = section_for_rva(rva, pe["sections"])
        raw_start = int(sec["virtual_address"])
        raw_end = raw_start + int(sec["raw_size"])
        if not (raw_start <= rva < raw_end):
            die(f"target VA 0x{va:X} lies in a virtual-only section tail")

        file_off = rva_to_file_offset(rva, pe["sections"], len(buf))
        available = raw_end - rva
        length = min(args.window, available)
        raw = checked_slice(buf, file_off, length, f"target window VA 0x{va:X}")

        stem = safe_stem(group_targets, va)
        raw_name = stem + ".bin"
        (args.outdir / raw_name).write_bytes(raw)

        rf_match = find_runtime_record(rva, records)
        runtime: dict[str, Any] | None = None
        if rf_match:
            idx, rec = rf_match
            chain = contiguous_runtime_chain(idx, records)
            runtime = {
                "containing": rec,
                "contiguous_runtime_chain": chain,
                "chain_record_count": len(chain),
            }

        disassembly: dict[str, Any] = {"status": "not_requested"}
        if args.run_objdump:
            disassembly = maybe_disassemble(
                args.gameassembly,
                va,
                va + length,
                args.outdir / (stem + ".objdump.txt"),
            )

        manifest["groups"].append(
            {
                "pc_va": f"0x{va:X}",
                "rva": f"0x{rva:X}",
                "file_offset": f"0x{file_off:X}",
                "section": sec["name"],
                "window_length": length,
                "raw_file": raw_name,
                "targets": group_targets,
                "runtime_function": runtime,
                "disassembly": disassembly,
                "classification": "UNREVIEWED_NATIVE_CONTROL_FLOW",
            }
        )

    manifest_path = args.outdir / "manifest.json"
    manifest_path.write_text(json.dumps(manifest, indent=2, ensure_ascii=False) + "\n", encoding="utf-8")
    print(f"SOURCE_SHA256={source_hash}")
    print(f"IMAGE_BASE=0x{int(pe['image_base']):X}")
    print(f"RUNTIME_FUNCTIONS={len(records)}")
    print(f"UNIQUE_NATIVE_VA_GROUPS={len(by_va)}")
    print(f"TARGET_METHODDEFS={len(targets)}")
    print(f"MANIFEST={manifest_path}")
    print("STAGE9_NATIVE_EXTRACTION_OK")
    return 0


if __name__ == "__main__":
    sys.exit(main())
