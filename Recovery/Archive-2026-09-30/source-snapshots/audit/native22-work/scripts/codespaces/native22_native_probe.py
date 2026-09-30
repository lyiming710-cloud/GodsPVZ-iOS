"""Resolve PC-native call targets for the pilot batch.

Inputs
  pcnative/GameAssembly.dll          (hash-locked to the handoff value)
  pcnative/native-method-map.json    token -> {image,type,name,va}
Emits, per token: the function extent, the disassembly, and every direct call
site with its resolved target identity.
"""
import collections
import json
import struct
import subprocess
import sys
from pathlib import Path

NB = Path('/workspaces/GodsPVZ-native19/.validation/native22')
PC = NB / 'pcnative'
OUT = PC / 'slices'
OUT.mkdir(exist_ok=True)
EXPECTED_SHA = '9ebd7ca996a5b03fb4a766f7a2502b660d581ddbbb36af4d7bf2f06a211da39d'

TABLE_VA = 0x181B82D60
IMAGE_BASE = 0x180000000


class PE:
    def __init__(self, data):
        self.data = data
        pe_off = struct.unpack_from('<I', data, 0x3C)[0]
        coff = pe_off + 4
        _m, nsec, _t, _s, _ns, opt_size, _c = struct.unpack_from('<HHIIIHH', data, coff)
        opt = coff + 20
        self.image_base = struct.unpack_from('<Q', data, opt + 24)[0]
        self.sections = []
        sec_off = opt + opt_size
        for i in range(nsec):
            o = sec_off + i * 40
            name = data[o:o + 8].split(b'\0', 1)[0].decode('ascii', 'replace')
            vsize, va, rsize, roff = struct.unpack_from('<IIII', data, o + 8)
            self.sections.append((name, va, max(vsize, rsize), roff, rsize))

    def va_to_offset(self, va):
        if va < self.image_base:
            raise ValueError('VA below image base')
        return self.rva_to_offset(va - self.image_base)

    def rva_to_offset(self, rva):
        for name, va, span, roff, rsize in self.sections:
            if va <= rva < va + span:
                delta = rva - va
                if delta >= rsize:
                    raise ValueError('zero-filled tail of %s' % name)
                return roff + delta
        raise ValueError('unmapped RVA 0x%X' % rva)

    def extent(self, va):
        """Return the .pdata runtime-function extent that starts exactly here."""
        for name, sva, span, roff, rsize in self.sections:
            if name == '.pdata':
                for o in range(roff, roff + rsize - 11, 12):
                    b, e, u = struct.unpack_from('<III', self.data, o)
                    if self.image_base + b == va:
                        return self.image_base + b, self.image_base + e
        raise ValueError('no exact .pdata entry for 0x%X' % va)


def read_cstr(data, pe, va):
    if va == 0:
        return None
    try:
        off = pe.va_to_offset(va)
    except ValueError:
        return None
    end = data.find(b'\0', off)
    if end < 0 or end - off > 512:
        return None
    return data[off:end].decode('utf-8', 'replace')


def method_info(data, pe, va):
    """MethodInfo { methodPointer; invoker; name; declaring_type; ... }."""
    try:
        off = pe.va_to_offset(va)
    except ValueError:
        return None
    mp, inv, name_p, decl_p = struct.unpack_from('<QQQQ', data, off)
    name = read_cstr(data, pe, name_p)
    cls = None
    if decl_p:
        try:
            co = pe.va_to_offset(decl_p)
            _img, _gc, cname_p, ns_p = struct.unpack_from('<QQQQ', data, co)
            cls = '%s.%s' % (read_cstr(data, pe, ns_p) or '', read_cstr(data, pe, cname_p) or '?')
        except ValueError:
            cls = None
    out = {'method_pointer': hex(mp), 'name': name, 'declaring_type': cls}
    if mp and cls is None:
        pass
    return out


def rip_targets_in(text):
    """Map instruction address -> every rip-relative operand address it mentions."""
    import re
    out = {}
    for line in text.splitlines():
        if '\t' not in line:
            continue
        parts = line.split('\t')
        try:
            addr = int(parts[0].strip().rstrip(':'), 16)
        except ValueError:
            continue
        tail = parts[2] if len(parts) > 2 else ''
        found = re.findall(r'0x([0-9a-f]+)(?=\s*\))|\b0x([0-9a-f]{6,})\b', tail)
        vals = [int(a or b, 16) for a, b in found]
        comment = tail.split('#')
        if len(comment) > 1:
            m = re.search(r'0x([0-9a-f]+)', comment[1])
            if m:
                vals.append(int(m.group(1), 16))
        if vals:
            out[addr] = vals
    return out


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
    0x1812dbbc0: 'UnityEngine.Component::get_transform ?(needs confirm)',
}


def build_va_map():
    entries = json.loads((PC / 'native-method-map.json').read_text())
    if isinstance(entries, dict):
        entries = list(entries.values())
    m = {}
    for e in entries:
        m[int(e['va'], 16)] = e
    return entries, m


def disasm(begin, blob):
    tmp = OUT / '_slice.bin'
    tmp.write_bytes(blob)
    r = subprocess.run(['objdump', '-D', '-b', 'binary', '-m', 'i386:x86-64',
                        '--adjust-vma=0x%X' % begin, '--insn-width=16', str(tmp)],
                       capture_output=True, text=True)
    return r.stdout, r.stderr


def main(tokens):
    import hashlib
    data = (PC / 'GameAssembly.dll').read_bytes()
    sha = hashlib.sha256(data).hexdigest()
    print('GAMEASSEMBLY_SHA256', sha, 'MATCH' if sha == EXPECTED_SHA else 'MISMATCH')
    if sha != EXPECTED_SHA:
        return 1
    pe = PE(data)
    entries, va_map = build_va_map()
    print('native-method-map entries      :', len(entries))
    print('distinct VAs                   :', len(va_map))

    report = {}
    for token in tokens:
        rid = int(token, 16) & 0xFFFFFF
        table_off = pe.va_to_offset(TABLE_VA + (rid - 1) * 8)
        target = struct.unpack_from('<Q', data, table_off)[0]
        begin, end = pe.extent(target)
        blob = data[pe.va_to_offset(begin):pe.va_to_offset(end - 1) + 1]
        text, err = disasm(begin, blob)

        rip = rip_targets_in(text)
        lines = text.splitlines()
        addr_of_line = {}
        for line in lines:
            if '\t' not in line:
                continue
            parts = line.split('\t')
            try:
                a = int(parts[0].strip().rstrip(':'), 16)
            except ValueError:
                continue
            addr_of_line[a] = parts[2] if len(parts) > 2 else ''

        calls = []
        unresolved = 0
        for line in text.splitlines():
            parts = line.split('\t')
            if len(parts) < 3:
                continue
            addr_s = parts[0].strip().rstrip(':')
            insn = parts[2]
            if not insn.startswith('call') and not insn.startswith('jmp'):
                continue
            try:
                addr = int(addr_s, 16)
            except ValueError:
                continue
            hexes = insn.split()[-1]
            if not hexes.startswith('0x'):
                calls.append({'at': addr, 'insn': insn, 'id': '<indirect>'})
                continue
            tgt = int(hexes, 16)
            info = va_map.get(tgt)
            # look back up to 4 instructions for a rip-relative MethodInfo operand
            mi = None
            mi_slot = None
            prior = [a for a in sorted(addr_of_line) if a < addr][-4:]
            for a in reversed(prior):
                for cand in rip.get(a, []):
                    # the same static slot first holds a RuntimeClass* for the
                    # class-init guard, then holds MethodInfo* once initialised
                    indirect = None
                    try:
                        indirect = struct.unpack_from('<Q', data, pe.va_to_offset(cand))[0]
                    except (ValueError, struct.error):
                        pass
                    for cand2 in (cand, indirect):
                        if not cand2:
                            continue
                        probe = method_info(data, pe, cand2)
                        if probe and probe['name'] and probe['method_pointer'] == hex(tgt):
                            mi, mi_slot = probe, cand2
                            break
                    if mi:
                        break
                if mi:
                    break
            if info:
                calls.append({'at': addr, 'insn': insn, 'va': hex(tgt),
                              'image': info.get('image', '?'),
                              'id': '%s::%s' % (info.get('type', '?'), info.get('name', '?')),
                              'token': info.get('token', '?'),
                              'source': 'native-method-map'})
            elif mi:
                calls.append({'at': addr, 'insn': insn, 'va': hex(tgt),
                              'image': 'MethodInfo slot 0x%x' % cand,
                              'id': '%s::%s' % (mi['declaring_type'], mi['name']),
                              'token': '-', 'source': 'methodinfo'})
            else:
                helper = HELPERS.get(tgt)
                if helper:
                    calls.append({'at': addr, 'insn': insn, 'va': hex(tgt), 'image': 'helper',
                                  'id': helper, 'token': '-', 'source': 'helper-table'})
                else:
                    unresolved += 1
                    calls.append({'at': addr, 'insn': insn, 'va': hex(tgt), 'id': '<unresolved>',
                                  'delta': tgt - addr})

        report[token] = {
            'token': token, 'va': hex(begin), 'end': hex(end), 'size': end - begin,
            'calls': calls, 'unresolved_calls': unresolved,
            'sha256': __import__('hashlib').sha256(blob).hexdigest(),
            'asm': text,
        }
        (OUT / ('%s.asm' % token)).write_text(text)
        print()
        print('=' * 100)
        print('%s  VA %s..%s (%d bytes)  %d call sites, %d unresolved'
              % (token, hex(begin), hex(end), end - begin, len(calls), unresolved))
        for c in calls:
            if c['id'] != '<unresolved>':
                print('   %s  %-44s -> %s  [%s %s]' % (hex(c['at']), c['insn'][:44], c['id'], c['image'], c['token']))
            else:
                print('   %s  %-44s -> %s  (delta %s)' % (hex(c['at']), c['insn'][:44], c['va'], c.get('delta')))
        print('  --- full disassembly (%d bytes) ---' % (end - begin))
        for line in text.splitlines():
            if '\t' in line:
                print('   ' + line)
    (NB / 'out' / 'native-forensics.json').write_text(json.dumps(report, indent=2))
    print()
    print('WROTE', NB / 'out' / 'native-forensics.json')
    return 0


if __name__ == '__main__':
    sys.exit(main(sys.argv[1:]))
