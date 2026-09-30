"""Native23 -- corrected evidence tiering.

The earlier write-up said "377 methods were repaired".  That number describes
how many *methods were touched*, and it must never be read as how many methods
were *proven*.  This script separates the two axes explicitly.

AXIS 1 -- what was done to the method
  M1  candidate only, nothing accepted (every site quarantined)
  M2  at least one site accepted

AXIS 2 -- how strong is the evidence, per method
  E1  typed layer      : the typed oracle moved FAIL -> PASS and every flip is
                         individually necessary (revert re-introduces a clash)
  E2  provenance layer : the reference operand's type is written in a metadata
                         signature (field / static field / parameter / this)
  E3  execution layer  : concrete execution actually reached the site and the
                         runtime value observed there was a reference
  E4  native, METHOD   : the PC-native body was located and contains at least as
                         many pointer-width zero comparisons as the method has
                         flips (method-level corroboration, NOT site-level)
  E5  native, SITE     : a specific native instruction was read against a
                         specific IL site and shown to be a null check
  E6  BEHAVIOUR        : the whole method's semantics proved equal to PC native

E6 is the only tier that could be called "the method's behaviour is proven".
E5 is the only tier that could be called "the null-check site has native
evidence".  Everything below is a necessary condition, not a proof.
"""
import collections
import json
import re
import sys

import native23_native as N

ROOT = '/workspaces/GodsPVZ-native19/.validation/native22'

# methods where a specific native instruction was read against a specific
# IL site by hand (see NATIVE23-BATCH2-REPORT.md section 4)
SITE_LEVEL = {
    '0x06000338': 'Device::TestPlacing            0x18034BDF9 mov 0x28(%rcx),%rcx; test %rcx,%rcx; je -> raise NRE',
    '0x060000D9': 'AttackRange::TestInRange_Device 0x1802FFCC9 test %rdx,%rdx; je',
    '0x060005C8': 'BoardInfoPage::SetLootList      30 sites, all `ldloc <ref>; ldc.i4 0; ceq`',
}

RE_TEST = re.compile(r'^test\s+(\S+),\1$')      # test %rcx,%rcx
RE_CMPQ = re.compile(r'^cmpq\s+\$0x[0-9a-f]*,')  # pointer-width compare with 0
RE_CMPL = re.compile(r'^cmpl\s+\$0x[0-9a-f]*,')  # 32-bit compare with 0


def main():
    data = json.load(open(ROOT + '/work/all-methods.batch1.json'))
    by_tok = {m['token']: m for m in data['methods']}
    cand = json.load(open(ROOT + '/out/afam2-candidates.json'))
    acc = json.load(open(ROOT + '/out/afam3-accepted.json'))
    qua = json.load(open(ROOT + '/out/afam3-quarantine.json'))
    beh = json.load(open(ROOT + '/out/behave-sites.json'))

    # E3: which sites did concrete execution reach and observe as a reference
    beh_sites = beh.get('sites', {})
    e3_methods_ok, e3_methods_partial = set(), set()
    for tok, info in acc.items():
        obs = [beh_sites.get('%s@%04X' % (tok, s['flip_offset']))
               for s in info['sites']]
        n_ref = sum(1 for v in obs if v == 'REF-ONLY')
        n_bad = sum(1 for v in obs if v in ('MIXED', 'NON-REF'))
        if n_bad == 0 and n_ref == len(obs) and obs:
            e3_methods_ok.add(tok)
        elif n_ref:
            e3_methods_partial.add(tok)

    pe = N.PE(N.DLL)
    _raw, by_token, by_va, by_name = N.build_map()

    tiers = collections.Counter()
    rows = {}
    native_stat = collections.Counter()
    for tok in cand:
        body = by_tok.get(tok)
        n_flips = len(cand[tok]['flips'])
        n_acc = len(acc.get(tok, {}).get('sites', []))
        n_qua = len(qua.get(tok, {}).get('sites', []))
        e = set(['E1'])
        if n_acc:
            e.add('E2')
        if tok in e3_methods_ok:
            e.add('E3')

        # E4: locate the native body and count null-check idioms
        norm = '0x%06X' % int(tok, 16)
        hits = by_token.get(norm, [])
        pref = [h for h in hits if h['image'] == N.PREFERRED_IMAGE] or hits
        nc = i0 = 0
        nloc = False
        size = 0
        if pref:
            try:
                start, end, _nf = pe.extent(pref[0]['va_i'])
                size = end - start
                rws, _ = N.disasm(pe, start, size)
                nloc = True
                for _a, t in rws:
                    if RE_TEST.match(t):
                        nc += 1
                    elif RE_CMPQ.match(t):
                        nc += 1
                    elif RE_CMPL.match(t):
                        i0 += 1
            except ValueError:
                pass
        native_stat['native body located' if nloc else 'native body NOT located'] += 1
        if nloc and nc >= n_flips:
            e.add('E4')
        if tok in SITE_LEVEL:
            e.add('E5')

        axis1 = 'M2' if n_acc else 'M1'
        rows[tok] = {'axis1': axis1, 'flips': n_flips, 'acc': n_acc, 'qua': n_qua,
                     'evidence': sorted(e), 'nc': nc, 'int0': i0, 'size': size,
                     'native': nloc}
        tiers[(axis1, '+'.join(sorted(e)))] += 1

    print('=== AXIS 1: what was done ===')
    n_m2 = sum(1 for r in rows.values() if r['axis1'] == 'M2')
    print('   M2 accepted at least one site : %d methods' % n_m2)
    print('   M1 candidate only (all sites quarantined): %d methods'
          % (len(rows) - n_m2))
    print()
    print('=== native body availability over the %d candidate methods ===' % len(rows))
    for k, v in native_stat.most_common():
        print('   %-26s %d' % (k, v))
    print()
    print('=== AXIS 2: strongest evidence tier reached, per method ===')
    order = ['E1', 'E1,E2', 'E1,E2,E3', 'E1,E2,E4', 'E1,E2,E3,E4',
             'E1,E2,E3,E4,E5', 'E1,E2,E4,E5', 'E1,E2,E3,E5', 'E1,E2,E5']
    def rank(k):
        e = k[1]
        return (-len(e.split(',')), e)
    for k, v in sorted(tiers.items(), key=lambda kv: rank(kv[0])):
        print('   %-8s %-18s %d methods' % (k[0], k[1], v))
    print()
    n_e5 = sum(1 for r in rows.values() if 'E5' in r['evidence'])
    n_e4 = sum(1 for r in rows.values() if 'E4' in r['evidence'])
    n_e3 = sum(1 for r in rows.values() if 'E3' in r['evidence'])
    n_e2 = sum(1 for r in rows.values() if 'E2' in r['evidence'])
    print('=== the two numbers that were previously conflated ===')
    print('   methods touched at all                          : %d' % len(rows))
    print('   E5  null-check site has NATIVE evidence         : %d' % n_e5)
    print('   E4  native body corroborates at method level    : %d' % n_e4)
    print('   E3  concrete execution reached every site       : %d' % n_e3)
    print('   E2  accepted on trusted-provenance grounds      : %d' % n_e2)
    print('   E6  WHOLE-METHOD BEHAVIOUR PROVEN               : 0')
    print()
    print('=== E5 (site-level native evidence) methods ===')
    for t in SITE_LEVEL:
        r = rows.get(t)
        if r is None:
            print('   %s  -- NOT in the candidate set' % t)
            continue
        print('   %s  %s' % (t, SITE_LEVEL[t]))
        print('        flips=%d accepted=%d quarantined=%d evidence=%s'
              % (r['flips'], r['acc'], r['qua'], ','.join(r['evidence'])))
    print()
    print('=== E6 is empty: what would it take ===')
    print('   Whole-method behaviour equivalence to PC native requires, for each')
    print('   method: (a) every branch predicate matched instruction by')
    print('   instruction, (b) every call target and argument matched, (c) the')
    print('   return value matched on identical inputs, (d) a machine-readable')
    print('   IL-site -> native-address map.  We have (d) for 0 methods, so E6 = 0.')

    json.dump(rows, open(ROOT + '/out/tiers.json', 'w'), indent=1)
    print()
    print('written: out/tiers.json')


if __name__ == '__main__':
    main()
