"""Emit the ACCEPT-only edit list (batch 2a).

Each edit records: the clash it removes, WHERE the reference type came from
(provenance class + the actual producing instruction), and the native grounding
of the rule class.  A site whose reference type comes from a local, a merge or an
already-clashed value is NOT emitted -- it lives in out/afam3-quarantine.json.
"""
import json
import os
import sys

ROOT = '/workspaces/GodsPVZ-native19/.validation/native22'

NATIVE_RULE = ('rule class grounded in PC native: Device::TestPlacing 0x18034BDF9 '
               '"mov 0x28(%rcx),%rcx; test %rcx,%rcx; je -> raise NRE", '
               'AttackRange::TestInRange_Device 0x1802FFCC9 "test %rdx,%rdx; je", '
               'Device::TryPlacing 0x18034BE4B (batch 1).')


def main():
    acc = json.load(open(ROOT + '/out/afam3-accepted.json'))
    bm = json.load(open(ROOT + '/out/body-map.new.json'))
    edits = []
    for tok in sorted(acc):
        v = acc[tok]
        ptrint = [m for m in bm.get(tok, {}).get('errors', [])
                  if 'comparison between pointer and integer' in m['msg']]
        for s in v['sites']:
            bits = ['removes clash IL_%04X "%s"' % (s['clash_offset'], s['clash_msg']),
                    'reference side provenance: [%s] %s' % (s['prov_class'], s['prov']),
                    NATIVE_RULE]
            if ptrint:
                bits.append('clang %s: %s' % (bm[tok]['file'], ptrint[0]['msg'][:110]))
            edits.append({'token': tok, 'offset': s['flip_offset'],
                          'action': 'to_ldnull', 'evidence': '; '.join(bits)})
    out = {'comment': ('Native23 batch 2a -- A family, ACCEPT-only. A site is '
                       'accepted only when the reference operand\'s type is written '
                       'in a metadata signature (field / static field / parameter / '
                       'this / call return) and has not passed through a local '
                       'declaration, a control-flow merge, or a value that already '
                       'clashed. 758 further sites are quarantined in '
                       'out/afam3-quarantine.json and deliberately NOT repaired.'),
           'edits': edits}
    p = ROOT + '/edits-batch2a.json'
    json.dump(out, open(p, 'w'), indent=0)
    print('methods: %d   edits: %d   bytes: %d'
          % (len(acc), len(edits), os.path.getsize(p)))


if __name__ == '__main__':
    main()
