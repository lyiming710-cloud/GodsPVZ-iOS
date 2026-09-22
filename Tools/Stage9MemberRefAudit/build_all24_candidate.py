#!/usr/bin/env python3
"""Build the locked Stage9.1 all-24 MemberRef *candidate* without dnfile.

This is intentionally candidate tooling.  It parses PE/CLI metadata tables
itself, discovers MemberRef row/column offsets from the #~ stream, validates the
known B001 anchor, applies only the locked normalization rules, and refuses to
write unless the resulting file SHA256 is the already-proven all-24 candidate.

It never modifies the formal runtime artifact in place.
"""
from __future__ import annotations

import argparse
import hashlib
import json
import struct
from pathlib import Path

RUN8 = "f47b1b37f1d4d3a1ddeb44bb6dbe399c58dc6dee3d4602618b2790f839d3e321"
B001 = "f144dd624d2b0c05b5ff01fc12ff455ac394c87852a4fc3c87e8c3611582a433"
GETENUM_FAMILY = "f01cbf6cc768b48388d8f37fe665914e8f5954902a1f0328357965d762c24bc5"
ALL24 = "b07254d5c4077e013bbc934734f7b0949c0942a4e667d6d2b23d2af6c830f720"
EXPECTED_SIZE = 1_201_152
EXPECTED_METHODDEFS = 2317

KNOWN_INPUTS = {
    RUN8: ("run8", 50),
    B001: ("b001-only", 48),
    GETENUM_FAMILY: ("getenumerator-family", 35),
    ALL24: ("all24-already", 0),
}

GETENUM = [0x0A0000B4,0x0A0000C9,0x0A0000D0,0x0A0000D6,0x0A0000E4,0x0A0000FB,0x0A000141,0x0A000162,0x0A00021D]
GETCURRENT = [0x0A0000B5,0x0A0000C3,0x0A0000CA,0x0A0000D1,0x0A0000D7,0x0A0000E5,0x0A0000E7,0x0A0000FC,0x0A000142,0x0A000163,0x0A00021E]
INSERT = [0x0A000140,0x0A00022B]
TRANSFORMS = {
    0x0A000149: 0x0A000008,  # Component: bad transform -> canonical get_transform
    0x0A000284: 0x0A00002C,  # GameObject: bad transform -> canonical get_transform
}
TRANSFORM_IL = [
    (0x1E569, 0x0A000149, "EnemyManager.PlayBoardAudio IL_0019"),
    (0x83E4E, 0x0A000149, "FlagMeter.Update IL_002A"),
    (0x83E9B, 0x0A000284, "FlagMeter.Update IL_0077"),
]

CANONICAL_BLOBS = {
    "GetEnumerator": (0x1C24, bytes.fromhex("20 00 15 11 55 01 13 00")),
    "get_Current": (0x4630, bytes.fromhex("20 00 13 00")),
    "Insert": (0x0976, bytes.fromhex("20 02 01 08 13 00")),
    "get_transform": (None, bytes.fromhex("20 00 12 11")),
}


def u16(b, o): return struct.unpack_from("<H", b, o)[0]
def u32(b, o): return struct.unpack_from("<I", b, o)[0]
def u64(b, o): return struct.unpack_from("<Q", b, o)[0]
def sha256(b): return hashlib.sha256(b).hexdigest()


def rva_to_file(b: bytes, pe: int, rva: int) -> int:
    coff = pe + 4
    sections = u16(b, coff + 2)
    opt_size = u16(b, coff + 16)
    s0 = coff + 20 + opt_size
    for i in range(sections):
        s = s0 + i * 40
        vsize, va, raw_size, raw = u32(b,s+8),u32(b,s+12),u32(b,s+16),u32(b,s+20)
        if va <= rva < va + max(vsize, raw_size):
            return raw + (rva - va)
    raise ValueError(f"RVA 0x{rva:x} outside PE sections")


def table_index_size(rows: list[int], table: int) -> int:
    return 2 if rows[table] < 0x10000 else 4


def coded_index_size(rows: list[int], tables: list[int], tag_bits: int) -> int:
    limit = 1 << (16 - tag_bits)
    return 2 if max((rows[t] for t in tables), default=0) < limit else 4


def compressed_uint(blob: bytes, pos: int) -> tuple[int,int]:
    a = blob[pos]
    if a & 0x80 == 0:
        return a, 1
    if a & 0xC0 == 0x80:
        return ((a & 0x3F) << 8) | blob[pos+1], 2
    if a & 0xE0 == 0xC0:
        return ((a & 0x1F) << 24) | (blob[pos+1] << 16) | (blob[pos+2] << 8) | blob[pos+3], 4
    raise ValueError(f"invalid compressed uint lead 0x{a:02x}")


class Metadata:
    def __init__(self, data: bytearray):
        self.data = data
        if data[:2] != b"MZ": raise ValueError("not MZ")
        self.pe = u32(data, 0x3C)
        if data[self.pe:self.pe+4] != b"PE\0\0": raise ValueError("bad PE signature")
        opt = self.pe + 24
        magic = u16(data, opt)
        dd = opt + (96 if magic == 0x10B else 112 if magic == 0x20B else -1)
        if dd < opt: raise ValueError(f"unsupported optional header magic 0x{magic:x}")
        cli_rva = u32(data, dd + 14*8)
        if not cli_rva: raise ValueError("missing CLI directory")
        cli = rva_to_file(data, self.pe, cli_rva)
        md_rva = u32(data, cli + 8)
        self.md = rva_to_file(data, self.pe, md_rva)
        if data[self.md:self.md+4] != b"BSJB": raise ValueError("bad CLI metadata signature")
        ver_len = u32(data, self.md + 12)
        p = self.md + 16 + ((ver_len + 3) & ~3)
        self.flags, streams = u16(data,p), u16(data,p+2)
        p += 4
        self.streams = {}
        for _ in range(streams):
            off, size = u32(data,p), u32(data,p+4)
            n0 = p + 8
            n1 = data.index(0, n0)
            name = bytes(data[n0:n1]).decode("ascii")
            self.streams[name] = (self.md + off, size)
            p = n0 + (((n1 - n0) + 1 + 3) & ~3)
        if "#~" not in self.streams: raise ValueError("missing #~ stream")
        self.strings_off, _ = self.streams["#Strings"]
        self.blob_off, _ = self.streams["#Blob"]
        tables, _ = self.streams["#~"]
        self.heap_sizes = data[tables+6]
        valid = u64(data, tables+8)
        self.rows = [0]*64
        q = tables + 24
        for tid in range(64):
            if valid & (1 << tid):
                self.rows[tid] = u32(data,q); q += 4
        self.table_data = q
        self.string_ix = 4 if self.heap_sizes & 0x01 else 2
        self.guid_ix = 4 if self.heap_sizes & 0x02 else 2
        self.blob_ix = 4 if self.heap_sizes & 0x04 else 2
        self.res_scope = coded_index_size(self.rows,[0,26,35,1],2)
        self.typedef_or_ref = coded_index_size(self.rows,[2,1,27],2)
        self.memberref_parent = coded_index_size(self.rows,[2,1,26,6,27],3)
        self.memberref_row_size = self.memberref_parent + self.string_ix + self.blob_ix
        sizes = {
            0: 2 + self.string_ix + 3*self.guid_ix,
            1: self.res_scope + 2*self.string_ix,
            2: 4 + 2*self.string_ix + self.typedef_or_ref + table_index_size(self.rows,4) + table_index_size(self.rows,6),
            3: table_index_size(self.rows,4),
            4: 2 + self.string_ix + self.blob_ix,
            5: table_index_size(self.rows,6),
            6: 4+2+2+self.string_ix+self.blob_ix+table_index_size(self.rows,8),
            7: table_index_size(self.rows,8),
            8: 2+2+self.string_ix,
            9: table_index_size(self.rows,2)+self.typedef_or_ref,
        }
        self.memberref_table = self.table_data + sum(self.rows[i]*sizes[i] for i in range(10))
        if self.rows[6] != EXPECTED_METHODDEFS:
            raise ValueError(f"MethodDef drift: {self.rows[6]} != {EXPECTED_METHODDEFS}")

    def read_ix(self, off: int, width: int) -> int:
        return int.from_bytes(self.data[off:off+width], "little")

    def write_ix(self, off: int, width: int, value: int):
        if value >= 1 << (8*width): raise ValueError(f"index 0x{value:x} does not fit {width} bytes")
        self.data[off:off+width] = value.to_bytes(width,"little")

    def memberref(self, token: int) -> dict:
        if token >> 24 != 0x0A: raise ValueError(f"not MemberRef token 0x{token:08x}")
        rid = token & 0xFFFFFF
        if rid < 1 or rid > self.rows[10]: raise ValueError(f"MemberRef RID out of range: {rid}")
        row = self.memberref_table + (rid-1)*self.memberref_row_size
        name_off = row + self.memberref_parent
        sig_off = name_off + self.string_ix
        return {
            "token": token, "rid": rid, "row_offset": row,
            "parent_offset": row, "name_offset": name_off, "signature_offset": sig_off,
            "parent": self.read_ix(row,self.memberref_parent),
            "name": self.read_ix(name_off,self.string_ix),
            "signature": self.read_ix(sig_off,self.blob_ix),
        }

    def string(self, ix: int) -> str:
        start = self.strings_off + ix
        end = self.data.index(0,start)
        return bytes(self.data[start:end]).decode("utf-8",errors="strict")

    def blob(self, ix: int) -> bytes:
        start = self.blob_off + ix
        n, prefix = compressed_uint(self.data,start)
        return bytes(self.data[start+prefix:start+prefix+n])


def main() -> int:
    ap = argparse.ArgumentParser()
    ap.add_argument("input")
    ap.add_argument("output")
    ap.add_argument("--manifest", default=None)
    a = ap.parse_args()
    src = Path(a.input).resolve(); dst = Path(a.output).resolve()
    if src == dst: raise SystemExit("refusing in-place write")
    original = src.read_bytes()
    if len(original) != EXPECTED_SIZE: raise SystemExit(f"unexpected size {len(original)}")
    before = sha256(original)
    if before not in KNOWN_INPUTS: raise SystemExit(f"unknown input SHA256 {before}")
    label, expected_diff = KNOWN_INPUTS[before]
    data = bytearray(original)
    md = Metadata(data)

    # Strong parser anchor from independently proven B001 raw patch.
    b001 = md.memberref(0x0A0000B4)
    if b001["signature_offset"] != 0xC99DE:
        raise SystemExit(f"metadata parser anchor mismatch: B001 signature offset 0x{b001['signature_offset']:x} != 0xC99DE")
    if md.string(b001["name"]) != "GetEnumerator":
        raise SystemExit("B001 name mismatch")

    for family,(ix,expected) in CANONICAL_BLOBS.items():
        if ix is not None and md.blob(ix) != expected:
            raise SystemExit(f"canonical {family} blob mismatch at 0x{ix:x}: {md.blob(ix).hex()}")

    patches = []
    def set_sig(token: int, family: str):
        row = md.memberref(token)
        expected_name = family
        if md.string(row["name"]) != expected_name:
            raise SystemExit(f"0x{token:08X} name {md.string(row['name'])!r} != {expected_name!r}")
        ix = CANONICAL_BLOBS[family][0]
        assert ix is not None
        old = bytes(data[row["signature_offset"]:row["signature_offset"]+md.blob_ix])
        md.write_ix(row["signature_offset"],md.blob_ix,ix)
        new = bytes(data[row["signature_offset"]:row["signature_offset"]+md.blob_ix])
        patches.append({"kind":"memberref-signature","token":f"0x{token:08X}","family":family,"file_offset":row["signature_offset"],"width":md.blob_ix,"old":old.hex(),"new":new.hex()})

    for t in GETENUM: set_sig(t,"GetEnumerator")
    for t in GETCURRENT: set_sig(t,"get_Current")
    for t in INSERT: set_sig(t,"Insert")

    # Transform rows: preserve the exact same Parent coded index, copy only the
    # canonical same-parent Name and Signature columns.
    for bad_token, canonical_token in TRANSFORMS.items():
        bad, can = md.memberref(bad_token), md.memberref(canonical_token)
        if bad["parent"] != can["parent"]:
            raise SystemExit(f"transform parent mismatch bad=0x{bad_token:08X} canonical=0x{canonical_token:08X}")
        if md.string(can["name"]) != "get_transform" or md.blob(can["signature"]) != CANONICAL_BLOBS["get_transform"][1]:
            raise SystemExit(f"canonical get_transform row invalid 0x{canonical_token:08X}")
        old_name, old_sig = md.string(bad["name"]), md.blob(bad["signature"])
        if old_name not in ("transform","get_transform"):
            raise SystemExit(f"unexpected transform row name {old_name!r}")
        for field,width in (("name",md.string_ix),("signature",md.blob_ix)):
            off = bad[field+"_offset"]
            old = bytes(data[off:off+width])
            value = can[field]
            md.write_ix(off,width,value)
            new = bytes(data[off:off+width])
            patches.append({"kind":"memberref-"+field,"token":f"0x{bad_token:08X}","canonical_token":f"0x{canonical_token:08X}","file_offset":off,"width":width,"old":old.hex(),"new":new.hex()})

    for off, token, label2 in TRANSFORM_IL:
        old = data[off]
        if old not in (0x7B,0x6F):
            raise SystemExit(f"{label2}: unexpected opcode 0x{old:02x} at 0x{off:x}")
        data[off] = 0x6F
        patches.append({"kind":"il-opcode","token":f"0x{token:08X}","label":label2,"file_offset":off,"width":1,"old":f"{old:02x}","new":"6f"})

    after = sha256(data)
    diffs = [i for i,(x,y) in enumerate(zip(original,data)) if x != y]
    if after != ALL24:
        raise SystemExit(f"candidate SHA mismatch {after} != {ALL24}; refusing output")
    if len(diffs) != expected_diff:
        raise SystemExit(f"diff-count mismatch for {label}: {len(diffs)} != {expected_diff}; refusing output")

    dst.parent.mkdir(parents=True,exist_ok=True)
    dst.write_bytes(data)
    manifest = {
        "schema":1,"input":str(src),"input_label":label,"input_sha256":before,
        "output":str(dst),"output_sha256":after,"size":len(data),
        "methoddef_count":md.rows[6],"memberref_count":md.rows[10],
        "heap_index_widths":{"strings":md.string_ix,"blob":md.blob_ix,"guid":md.guid_ix},
        "memberref_parent_width":md.memberref_parent,"memberref_row_size":md.memberref_row_size,
        "memberref_table_file_offset":md.memberref_table,
        "b001_signature_file_offset":b001["signature_offset"],
        "diff_count":len(diffs),"diff_offsets":[f"0x{x:X}" for x in diffs],
        "patches":patches,
        "status":"ALL24_RAW_CANDIDATE_HASH_GATE_PASS",
    }
    mp = Path(a.manifest).resolve() if a.manifest else dst.with_suffix(dst.suffix+".manifest.json")
    mp.write_text(json.dumps(manifest,indent=2,sort_keys=True)+"\n",encoding="utf-8")
    print(json.dumps(manifest,indent=2,sort_keys=True))
    return 0

if __name__ == "__main__":
    raise SystemExit(main())
