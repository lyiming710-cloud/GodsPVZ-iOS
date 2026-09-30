"""Native23 PC-native forensics.

Fixes two defects found in native22_native_probe.py:
  1. Extent: a function may occupy a CHAIN of contiguous .pdata entries
     (Plant::Update = 18035C4D0..18035C8E3 spread over 8 entries). Taking only the
     single exact-match entry yielded 27 bytes and silently truncated the body.
  2. Token collisions: metadata tokens repeat across images
     (0x600034C = CoreModule ColorGamutUtility::GetTransferFunction AND
      Assembly-CSharp Plant::Update). Always prefer Assembly-CSharp.dll.
"""
import bisect
import collections
import hashlib
import json
import re
import struct
import subprocess
import sys
from pathlib import Path

ROOT = Path('/workspaces/GodsPVZ-native19/.validation/native22')
DLL = ROOT / 'pcnative/GameAssembly.dll'
MAP = ROOT / 'pcnative/native-method-map.json'
EXPECTED_SHA = '9ebd7ca996a5b03fb4a766f7a2502b660d581ddbbb36af4d7bf2f06a211da39d'
PREFERRED_IMAGE = 'Assembly-CSharp.dll'


class PE:
    def __init__(self, path):
        self.data = path.read_bytes()
        d = self.data
        coff = struct.unpack_from('<I', d, 0x3C)[0] + 4
        _m, nsec, _t, _s, _ns, opt_size, _c = struct.unpack_from('<HHIIIHH', d, coff)
        opt = coff + 20
        self.image_base = struct.unpack_from('<Q', d, opt + 24)[0]
        self.sections = []
        o = opt + opt_size
        for _ in range(nsec):
            name = d[o:o + 8].split(b'\0', 1)[0].decode('ascii', 'replace')
            vsize, va, rsize, roff = struct.unpack_from('<IIII', d, o + 8)
            self.sections.append((name, va, max(vsize, rsize), roff, rsize))
            o += 40

    def va_to_offset(self, va):
        return self.rva_to_offset(va - self.image_base)

    def rva_to_offset(self, rva):
        for name, va, span, roff, rsize in self.sections:
            if va <= rva < va + span:
                delta = rva - va
                if delta >= rsize:
                    raise ValueError('zero-filled tail of %s' % name)
                return roff + delta
        raise ValueError('unmapped RVA 0x%X' % rva)

    def pdata_entries(self):
        out = []
        for name, va, span, roff, rsize in self.sections:
            if name == '.pdata':
                for o in range(roff, roff + rsize - 11, 12):
                    b, e, _u = struct.unpack_from('<III', self.data, o)
                    out.append((self.image_base + b, self.image_base + e))
        return sorted(set(out))

    def extent(self, va):
        """Maximal contiguous chain of .pdata fragments starting at va."""
        ents = self.pdata_entries()
        begins = [a for a, _ in ents]
        i = bisect.bisect_left(begins, va)
        if i >= len(ents) or begins[i] != va:
            raise ValueError('no .pdata entry starts at 0x%X' % va)
        start, end = ents[i]
        j = i
        while j + 1 < len(ents) and ents[j + 1][0] == end:
            end = ents[j + 1][1]
            j += 1
        return start, end, (j - i + 1)


def read_cstr(data, pe, va):
    if not va:
        return None
    try:
        off = pe.va_to_offset(va)
    except ValueError:
        return None
    end = data.find(b'\0', off)
    if end < 0 or end - off > 512:
        return None
    return data[off:end].decode('utf-8', 'replace')


def disasm(pe, start, size):
    tmp = Path('/tmp/native23.bin')
    tmp.write_bytes(pe.data[pe.va_to_offset(start):pe.va_to_offset(start) + size])
    out = subprocess.run(['objdump', '-D', '-b', 'binary', '-m', 'i386:x86-64',
                          '--adjust-vma=0x%X' % start, str(tmp)],
                         capture_output=True, text=True).stdout
    rows = []
    for line in out.splitlines():
        m = re.match(r'^\s+([0-9a-f]+):\s+((?:[0-9a-f]{2} )+)\s*(.*)$', line)
        if m:
            rows.append((int(m.group(1), 16), m.group(3).strip()))
    return rows, out


def build_map():
    raw = json.loads(MAP.read_text())
    by_token = collections.defaultdict(list)
    by_va = collections.defaultdict(list)
    by_name = collections.defaultdict(list)
    for x in raw:
        tok = '0x%06X' % int(x['token'], 16)
        va = int(x['va'], 16)
        rec = dict(x, token=tok, va_i=va)
        by_token[tok].append(rec)
        by_va[va].append(rec)
        by_name[(x['type'], x['name'])].append(rec)
    return raw, by_token, by_va, by_name


HELPERS = {
    0x18024fef0: 'il2cpp: runtime class-init guard',
    0x18024f360: 'il2cpp: write barrier / store check',
    0x180250150: 'il2cpp: raise NullReferenceException',
    0x1802501e0: 'il2cpp: raise MissingMethodException / type-init',
    0x180cb86f0: 'System.Array::Clear',
    0x180258b00: 'il2cpp: generic context / rgctx lazy init',
    0x180ccee30: 'System.Type::GetTypeFromHandle',
    0x18046dc80: 'UnityEngine.GameObject::GetComponent<T>',
    0x18042e9f0: 'UnityEngine.Component::GetComponent<T>',
    0x18131c070: 'UnityEngine.GameObject::get_transform',
}


def resolve(va, by_va, by_name):
    if va in HELPERS:
        return HELPERS[va]
    hits = by_va.get(va, [])
    if not hits:
        return None
    pref = [h for h in hits if h['image'] == PREFERRED_IMAGE]
    pick = (pref or hits)[0]
    return '%s::%s  [%s %s]' % (pick['type'], pick['name'], pick['image'], pick['token'])


def main(tokens):
    sha = hashlib.sha256(DLL.read_bytes()).hexdigest()
    print('GAMEASSEMBLY_SHA256 %s %s' % (sha, 'MATCH' if sha == EXPECTED_SHA else 'MISMATCH!!'))
    _raw, by_token, by_va, by_name = build_map()
    print('native-method-map entries: %d' % len(_raw))

    # resolve helper names we quote in conclusions
    for key, label in list(HELPERS.items()):
        _ = key, label
    extra = {}
    for t, n in [('UnityEngine.Component', 'get_transform'),
                 ('UnityEngine.GameObject', 'get_transform'),
                 ('UnityEngine.Camera', 'get_main'),
                 ('UnityEngine.GameObject', 'GetComponent')]:
        for h in by_name.get((t, n), []):
            extra.setdefault((t, n), []).append((h['image'], h['token'], h['va_i']))
    print()
    print('=== symbol VA table for the calls under discussion ===')
    for (t, n), v in sorted(extra.items()):
        for img, tok, va in v:
            print('   %-24s %-18s %-28s 0x%08X' % (t, n, img, va))
    print()

    for tok in tokens:
        norm = '0x%06X' % int(tok, 16)
        hits = by_token.get(norm, [])
        pref = [h for h in hits if h['image'] == PREFERRED_IMAGE] or hits
        print('=' * 104)
        print('%s   candidates=%s' % (tok, ['%s %s::%s' % (h['image'], h['type'], h['name']) for h in hits]))
        if not pref:
            print('  !! no native entry for this token')
            continue
        rec = pref[0]
        va = rec['va_i']
        try:
            start, end, nfrag = PE(DLL).extent(va)
        except ValueError as e:
            print('  !! %s' % e)
            continue
        size = end - start
        print('  %s::%s  VA 0x%08X..0x%08X (%d bytes, %d .pdata fragments)'
              % (rec['type'], rec['name'], start, end, size, nfrag))
        rows, raw = disasm(PE(DLL), start, size)
        # call sites
        sites = []
        for addr, text in rows:
            m = re.match(r'^(call|jmp)\s+(?:.*?\s)?0x([0-9a-f]+)$', text) or \
                re.match(r'^(call|jmp)\s+\*?.*$', text)
            if not m:
                continue
            if m.groups()[0] in ('call', 'jmp'):
                tm = re.search(r'0x([0-9a-f]+)$', text)
                if not tm:
                    continue
                target = int(tm.group(1), 16)
                if text.startswith('jmp') and (target < 0x180000000 or target > 0x182000000):
                    continue
                sites.append((addr, m.groups()[0], target))
        unresolved = 0
        for addr, kind, target in sites:
            name = resolve(target, by_va, by_name)
            if name is None:
                unresolved += 1
                # try rip operand from the disassembly text
                name = '<unresolved 0x%08X>' % target
            print('   0x%08X  %-5s 0x%08X  -> %s' % (addr, kind, target, name))
        print('  call/jmp sites: %d, unresolved targets: %d' % (len(sites), unresolved))
        out = ROOT / ('out/asm-%s.txt' % tok.lower().replace('0x', ''))
        out.write_text(raw)
        print('  disassembly: %s' % out)
        print()


if __name__ == '__main__':
    main(sys.argv[1:] or [])
