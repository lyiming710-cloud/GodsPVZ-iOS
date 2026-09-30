"""Inventory for the behaviour-test work: how big / how diverse are the methods
that carry ACCEPT sites, and which opcodes would a concrete interpreter have to
model to reach those sites?"""
import collections
import json
import os

ROOT = '/workspaces/GodsPVZ-native19/.validation/native22'

def main():
    dump = ROOT + '/work/all-methods.batch1.json'
    acc_path = ROOT + '/out/afam3-accepted.json'
    print('exists all-methods:', os.path.exists(dump))
    print('exists accepted   :', os.path.exists(acc_path))
    data = json.load(open(dump))
    acc = json.load(open(acc_path))
    types = data['types']
    by_tok = {m['token']: m for m in data['methods']}

    print('methods in dump   :', len(data['methods']))
    print('methods w/ ACCEPT :', len(acc))
    print('ACCEPT sites      :', sum(len(v['sites']) for v in acc.values()))

    ops = collections.Counter()
    sizes = []
    n_hander = 0
    n_switch = 0
    for tok, v in acc.items():
        m = by_tok.get(tok)
        if m is None:
            print('MISSING body for', tok)
            continue
        sizes.append(len(m['instructions']))
        if m['handlers']:
            n_hander += 1
        for x in m['instructions']:
            ops[x['opcode']] += 1
            if x['opcode'] == 'switch':
                n_switch += 1
    sizes.sort()
    print()
    print('body size: min %d  p25 %d  median %d  p75 %d  max %d'
          % (sizes[0], sizes[len(sizes)//4], sizes[len(sizes)//2],
             sizes[3*len(sizes)//4], sizes[-1]))
    print('methods with EH handlers:', n_hander)
    print('switch instructions     :', n_switch)
    print()
    print('=== opcode histogram over ACCEPT methods (all instructions) ===')
    for k, c in ops.most_common():
        print('   %-16s %d' % (k, c))
    print('   distinct opcodes: %d' % len(ops))

    # how many flip sites sit inside a handler region
    print()
    print('=== sample 3 accepted methods ===')
    for tok in list(acc)[:3]:
        m = by_tok[tok]
        print(tok, m['name'], '| owner', m.get('owner'), '| ins', len(m['instructions']),
              '| sites', len(acc[tok]['sites']))
        print('   args', m['args'], 'ret', m['ret'], 'hasThis', m['hasThis'])
        print('   locals', m['locals'])

    # which opcode precedes / follows the flip site
    print()
    print('=== opcode shape around ACCEPT flip sites ===')
    shape = collections.Counter()
    for tok, v in acc.items():
        m = by_tok.get(tok)
        if m is None:
            continue
        by = {x['offset']: i for i, x in enumerate(m['instructions'])}
        for s in v['sites']:
            i = by.get(s['flip_offset'])
            if i is None:
                continue
            prev = m['instructions'][i-1]['opcode'] if i > 0 else '<entry>'
            nxt = m['instructions'][i+1]['opcode'] if i+1 < len(m['instructions']) else '<end>'
            nxt2 = m['instructions'][i+2]['opcode'] if i+2 < len(m['instructions']) else '<end>'
            shape[(prev, nxt, nxt2)] += 1
    for k, c in shape.most_common(15):
        print('   %-14s %-10s %-10s  %d' % (k[0], k[1], k[2], c))


if __name__ == '__main__':
    main()
