"""Turn out/afam2-candidates.json into a RepairNative23 edit list.

Every edit carries its own evidence string:
  * the exact clash the flip removes (offset + verifier text)
  * the clang pointer/integer diagnostic of the same body when present
  * the native grounding of the rule class
"""
import json
import os
import sys

ROOT = '/workspaces/GodsPVZ-native19/.validation/native22'

NATIVE_RULE = ('rule grounded in PC native: Device::TestPlacing 0x18034BDF9 '
               '"mov 0x28(%rcx),%rcx; test %rcx,%rcx; je -> raise NRE" and '
               'AttackRange::TestInRange_Device 0x1802FFCC9 "test %rdx,%rdx; je"; '
               'batch 1 proved the same shape on Device::TryPlacing.')


def main():
    cand = json.load(open(ROOT + '/out/afam2-candidates.json'))
    edits = []
    for tok in sorted(cand):
        c = cand[tok]
        rem = {(r['offset']): r['msg'] for r in c['removed']}
        ptrint = [m for m in c['clang'] if 'comparison between pointer and integer' in m]
        for f in c['flips']:
            off = f['offset']
            bits = ['removes clash IL_%04X "%s"' % (off, rem.get(off, '?'))]
            if ptrint:
                bits.append('clang %s: %s' % (c['clang_file'], ptrint[0][:110]))
            bits.append(NATIVE_RULE)
            edits.append({'token': tok, 'offset': off, 'action': 'to_ldnull',
                          'evidence': '; '.join(bits)})
    out = {'comment': ('Native23 batch 2 -- A family: integer literal zero used '
                       'where the original IL had ldnull. Selection rule: the flip '
                       'removes an I4-vs-reference clash from the typed verifier, '
                       'adds no clash, and is individually necessary.'),
           'edits': edits}
    p = ROOT + '/edits-batch2.json'
    json.dump(out, open(p, 'w'), indent=0)
    print('methods: %d   edits: %d   bytes: %d'
          % (len(cand), len(edits), os.path.getsize(p)))


if __name__ == '__main__':
    main()
