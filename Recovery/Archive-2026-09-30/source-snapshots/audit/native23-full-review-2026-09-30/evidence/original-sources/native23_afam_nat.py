"""Dump the PC-native body of A-family sample methods with call targets resolved,
so the null-check pattern (`test reg,reg` / `cmp qword ptr, 0`) can be read
directly against the repaired IL."""
import re
import sys
from pathlib import Path

import native23_native as N

ROOT = N.ROOT


def main(tokens):
    pe = N.PE(N.DLL)
    _raw, by_token, by_va, by_name = N.build_map()
    for tok in tokens:
        norm = '0x%06X' % int(tok, 16)
        hits = by_token.get(norm, [])
        pref = [h for h in hits if h['image'] == N.PREFERRED_IMAGE] or hits
        print('=' * 100)
        if not pref:
            print('%s : no native entry' % tok)
            continue
        rec = pref[0]
        va = rec['va_i']
        try:
            start, end, nfrag = pe.extent(va)
        except ValueError as e:
            print('%s : %s' % (tok, e))
            continue
        print('%s  %s::%s  VA 0x%08X..0x%08X  (%d bytes, %d .pdata fragments)'
              % (tok, rec['type'], rec['name'], start, end, end - start, nfrag))
        rows, _ = N.disasm(pe, start, end - start)
        for addr, text in rows:
            note = ''
            m = re.search(r'0x([0-9a-f]+)$', text)
            if text.startswith(('call', 'jmp')) and m:
                tgt = int(m.group(1), 16)
                name = N.resolve(tgt, by_va, by_name)
                note = '   -> ' + (name or '<unresolved>')
            if text.startswith('test'):
                note += '   <<< NULL-CHECK'
            print('   0x%08X  %-46s%s' % (addr, text, note))
        print()


if __name__ == '__main__':
    main(sys.argv[1:] or [])
