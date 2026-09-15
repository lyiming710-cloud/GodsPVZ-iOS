#!/usr/bin/env python3
import argparse, hashlib, struct
from pathlib import Path

LOCKED_GAMEASSEMBLY_SHA = '9ebd7ca996a5b03fb4a766f7a2502b660d581ddbbb36af4d7bf2f06a211da39d'
DEFAULT_POINTER_TABLE_VA = 0x181B82D60
SUPPLIES_RID = 604
SUPPLIES_ENTRY = 0x181B84038
SUPPLIES_PTR = 0x180341320
SUPPLIES_END = 0x18034146C
SUPPLIES_SHA = '3688ac9e8b1ca81c33bb128a816e2a5ab536fd6e48126a5e093749dfd964c6f8'

class PE64:
    def __init__(self, path: Path):
        self.path = path
        self.data = path.read_bytes()
        e = struct.unpack_from('<I', self.data, 0x3C)[0]
        if self.data[e:e+4] != b'PE\0\0':
            raise ValueError('not PE')
        fh = e + 4
        nsec = struct.unpack_from('<H', self.data, fh + 2)[0]
        optsz = struct.unpack_from('<H', self.data, fh + 16)[0]
        opt = fh + 20
        if struct.unpack_from('<H', self.data, opt)[0] != 0x20B:
            raise ValueError('not PE32+')
        self.imagebase = struct.unpack_from('<Q', self.data, opt + 24)[0]
        self.exc_rva, self.exc_size = struct.unpack_from('<II', self.data, opt + 112 + 3 * 8)
        sh = opt + optsz
        self.sections = []
        for i in range(nsec):
            o = sh + i * 40
            name = self.data[o:o+8].split(b'\0', 1)[0].decode(errors='replace')
            vs, va, rs, rp = struct.unpack_from('<IIII', self.data, o + 8)
            self.sections.append((name, va, vs, rs, rp))
        self.runtime_functions = []
        o = self.rva_to_offset(self.exc_rva)
        for p in range(o, o + self.exc_size, 12):
            if p + 12 > len(self.data):
                break
            s, en, u = struct.unpack_from('<III', self.data, p)
            if s or en:
                self.runtime_functions.append((s, en, u))
        self.runtime_functions.sort()

    def rva_to_offset(self, rva):
        for _, va, vs, rs, rp in self.sections:
            if va <= rva < va + max(vs, rs):
                return rp + (rva - va)
        raise ValueError(f'RVA not mapped: 0x{rva:X}')

    def read_va(self, va, n):
        o = self.rva_to_offset(va - self.imagebase)
        return self.data[o:o+n]

    def runtime_for_va(self, va):
        rva = va - self.imagebase
        for s, e, u in self.runtime_functions:
            if s <= rva < e:
                return self.imagebase + s, self.imagebase + e, self.imagebase + u
        return None

    def runtime_neighbors(self, va):
        rva = va - self.imagebase
        prev = max((x for x in self.runtime_functions if x[1] <= rva), default=None, key=lambda x: x[1])
        nxt = min((x for x in self.runtime_functions if x[0] > rva), default=None, key=lambda x: x[0])
        return ((self.imagebase + prev[0], self.imagebase + prev[1]) if prev else None,
                (self.imagebase + nxt[0], self.imagebase + nxt[1]) if nxt else None)

def sha_bytes(data):
    return hashlib.sha256(data).hexdigest()

def sha_file(path):
    return hashlib.sha256(path.read_bytes()).hexdigest()

def extract(pe, rid, table):
    entry = table + (rid - 1) * 8
    ptr = struct.unpack('<Q', pe.read_va(entry, 8))[0]
    rf = pe.runtime_for_va(ptr)
    out = {'rid': rid, 'entry': entry, 'pointer': ptr, 'runtime': rf}
    if rf:
        s, e, _ = rf
        blob = pe.read_va(s, e - s)
        out['length'] = e - s
        out['sha'] = sha_bytes(blob)
    else:
        prev, nxt = pe.runtime_neighbors(ptr)
        out['prev'] = prev
        out['next'] = nxt
        out['context'] = pe.read_va(ptr, 32).hex(' ')
    return out

def main():
    ap = argparse.ArgumentParser(description='Read-only GodsPVZ PC IL2CPP method-pointer/.pdata extractor')
    ap.add_argument('gameassembly', type=Path)
    ap.add_argument('rid', type=int)
    ap.add_argument('--pointer-table-va', type=lambda x: int(x, 0), default=DEFAULT_POINTER_TABLE_VA)
    ap.add_argument('--expected-sha256', default=LOCKED_GAMEASSEMBLY_SHA)
    ap.add_argument('--skip-supplies-self-check', action='store_true')
    a = ap.parse_args()
    actual = sha_file(a.gameassembly)
    print('GAMEASSEMBLY_SHA256', actual)
    if a.expected_sha256 and actual.lower() != a.expected_sha256.lower():
        raise SystemExit('GameAssembly SHA mismatch')
    pe = PE64(a.gameassembly)
    print(f'IMAGEBASE 0x{pe.imagebase:X}')
    print(f'POINTER_TABLE_VA 0x{a.pointer_table_va:X}')
    if not a.skip_supplies_self_check:
        q = extract(pe, SUPPLIES_RID, a.pointer_table_va)
        assert q['entry'] == SUPPLIES_ENTRY
        assert q['pointer'] == SUPPLIES_PTR
        assert q['runtime'][:2] == (SUPPLIES_PTR, SUPPLIES_END)
        assert q['sha'] == SUPPLIES_SHA
        print('SUPPLIES_SELF_CHECK_PASS rid=604 entry=0x181B84038 va=0x180341320 end=0x18034146C native_sha256=' + SUPPLIES_SHA)
    q = extract(pe, a.rid, a.pointer_table_va)
    print(f"TARGET rid={q['rid']} entry=0x{q['entry']:X} pointer=0x{q['pointer']:X}")
    if q['runtime']:
        s, e, u = q['runtime']
        print(f"PDATA start=0x{s:X} end=0x{e:X} length={q['length']} unwind=0x{u:X} native_sha256={q['sha']}")
    else:
        print('PDATA NONE')
        if q['prev']:
            print(f"PREV_RUNTIME start=0x{q['prev'][0]:X} end=0x{q['prev'][1]:X}")
        if q['next']:
            print(f"NEXT_RUNTIME start=0x{q['next'][0]:X} end=0x{q['next'][1]:X}")
        print('CONTEXT32', q['context'])

if __name__ == '__main__':
    main()
