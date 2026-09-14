#!/usr/bin/env python3
"""Resolve an Assembly-CSharp MethodDef RID to its original PC IL2CPP body.

Uses only the Python standard library. The default method-pointer table is the
verified GodsPVZ 1.0.2 Assembly-CSharp CodeGenModule table at VA 0x181B82D60.
It also resolves the covering .pdata runtime-function entry and hashes the
exact native slice so future recovery targets do not need one-off PE scripts.
"""
from __future__ import annotations

import argparse
import hashlib
import struct
from dataclasses import dataclass
from pathlib import Path

DEFAULT_TABLE_VA = 0x181B82D60
DEFAULT_IMAGE_BASE = 0x180000000
DEFAULT_GAMEASSEMBLY_SHA256 = "9ebd7ca996a5b03fb4a766f7a2502b660d581ddbbb36af4d7bf2f06a211da39d"


@dataclass(frozen=True)
class Section:
    name: str
    va: int
    virtual_size: int
    raw_offset: int
    raw_size: int


class PE:
    def __init__(self, data: bytes):
        self.data = data
        if data[:2] != b"MZ":
            raise ValueError("not a PE file: missing MZ")
        pe_off = struct.unpack_from("<I", data, 0x3C)[0]
        if data[pe_off : pe_off + 4] != b"PE\0\0":
            raise ValueError("not a PE file: missing PE signature")
        coff = pe_off + 4
        _machine, nsec, _ts, _sym, _nsym, opt_size, _chars = struct.unpack_from("<HHIIIHH", data, coff)
        opt = coff + 20
        magic = struct.unpack_from("<H", data, opt)[0]
        if magic != 0x20B:
            raise ValueError(f"expected PE32+ image, magic=0x{magic:04X}")
        self.image_base = struct.unpack_from("<Q", data, opt + 24)[0]
        self.sections: list[Section] = []
        sec_off = opt + opt_size
        for i in range(nsec):
            o = sec_off + i * 40
            name = data[o : o + 8].split(b"\0", 1)[0].decode("ascii", "replace")
            vsize, va, rsize, roff = struct.unpack_from("<IIII", data, o + 8)
            self.sections.append(Section(name, va, vsize, roff, rsize))

    def rva_to_offset(self, rva: int) -> int:
        for s in self.sections:
            span = max(s.virtual_size, s.raw_size)
            if s.va <= rva < s.va + span:
                delta = rva - s.va
                if delta >= s.raw_size:
                    raise ValueError(f"RVA 0x{rva:X} lies in zero-filled tail of section {s.name}")
                return s.raw_offset + delta
        raise ValueError(f"RVA 0x{rva:X} not mapped by a section")

    def va_to_offset(self, va: int) -> int:
        if va < self.image_base:
            raise ValueError(f"VA 0x{va:X} below image base 0x{self.image_base:X}")
        return self.rva_to_offset(va - self.image_base)

    def section(self, name: str) -> Section:
        return next(s for s in self.sections if s.name == name)


def sha256_bytes(data: bytes) -> str:
    return hashlib.sha256(data).hexdigest()


def main() -> int:
    ap = argparse.ArgumentParser()
    ap.add_argument("gameassembly", type=Path)
    ap.add_argument("rid", type=lambda s: int(s, 0), help="MethodDef RID, decimal or 0x-prefixed")
    ap.add_argument("--table-va", type=lambda s: int(s, 0), default=DEFAULT_TABLE_VA)
    ap.add_argument("--expected-sha256", default=DEFAULT_GAMEASSEMBLY_SHA256)
    ap.add_argument("--allow-hash-mismatch", action="store_true")
    ap.add_argument("--context", type=int, default=3, help="neighbor pointer entries to print")
    args = ap.parse_args()

    if args.rid < 1:
        raise SystemExit("RID must be >= 1")
    data = args.gameassembly.read_bytes()
    file_sha = sha256_bytes(data)
    print(f"GAMEASSEMBLY path={args.gameassembly.resolve()}")
    print(f"GAMEASSEMBLY_SHA256 {file_sha}")
    if args.expected_sha256 and file_sha.lower() != args.expected_sha256.lower() and not args.allow_hash_mismatch:
        raise SystemExit(f"GameAssembly SHA mismatch: expected {args.expected_sha256}")

    pe = PE(data)
    print(f"IMAGE_BASE 0x{pe.image_base:X}")
    table_entry_va = args.table_va + (args.rid - 1) * 8
    table_entry_off = pe.va_to_offset(table_entry_va)
    target_va = struct.unpack_from("<Q", data, table_entry_off)[0]
    print(f"METHOD_POINTER_TABLE_VA 0x{args.table_va:X}")
    print(f"METHOD rid={args.rid} token=0x{0x06000000 + args.rid:08X} entry_va=0x{table_entry_va:X} pointer=0x{target_va:X}")

    lo = max(1, args.rid - args.context)
    hi = args.rid + args.context
    for rid in range(lo, hi + 1):
        ev = args.table_va + (rid - 1) * 8
        try:
            ptr = struct.unpack_from("<Q", data, pe.va_to_offset(ev))[0]
        except ValueError:
            break
        print(f"NEIGHBOR rid={rid} pointer=0x{ptr:X}")

    pdata = pe.section(".pdata")
    if pdata.raw_size % 12:
        print(f"WARNING pdata_raw_size_not_multiple_of_12 size={pdata.raw_size}")
    exact = None
    covering = None
    for o in range(pdata.raw_offset, pdata.raw_offset + pdata.raw_size - 11, 12):
        begin_rva, end_rva, unwind_rva = struct.unpack_from("<III", data, o)
        if begin_rva == 0 and end_rva == 0:
            continue
        begin = pe.image_base + begin_rva
        end = pe.image_base + end_rva
        unwind = pe.image_base + unwind_rva
        if begin == target_va:
            exact = (begin, end, unwind)
            break
        if begin <= target_va < end:
            covering = (begin, end, unwind)
    rf = exact or covering
    if rf is None:
        raise SystemExit(f"no .pdata runtime function covers 0x{target_va:X}")
    begin, end, unwind = rf
    relation = "exact" if exact else "covering"
    print(f"PDATA relation={relation} begin=0x{begin:X} end=0x{end:X} unwind=0x{unwind:X} size={end-begin}")

    start_off = pe.va_to_offset(begin)
    end_off = pe.va_to_offset(end - 1) + 1
    native = data[start_off:end_off]
    print(f"NATIVE_SLICE_SHA256 {sha256_bytes(native)}")
    print(f"NATIVE_SLICE_HEX {native.hex()}")
    return 0


if __name__ == "__main__":
    raise SystemExit(main())
