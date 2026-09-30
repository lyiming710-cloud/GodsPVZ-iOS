import json, collections, sys
from pathlib import Path

OUT = Path('/workspaces/GodsPVZ-native19/.validation/native22/out')
TOKENS = ['0x0600000A', '0x06000348', '0x060003F7', '0x060003FD',
          '0x06000451', '0x06000338', '0x06000339']

cls = json.loads((OUT / 'classification.json').read_text())
methods = {m['token']: m for m in cls['methods']}
data = json.loads((OUT / 'all-methods.json').read_text())
bodies = {m['token']: m for m in data['methods']}

for t in TOKENS:
    m = methods.get(t)
    if m is None:
        print('!! %s not found' % t)
        continue
    b = bodies[t]
    print('=' * 100)
    print('TOKEN %s  bucket=%s  instrs=%d  maxstack=%s' % (t, m['bucket'], len(b['instructions']), b['maxStack']))
    print('NAME  %s' % m['name'])
    print('RET   %s   hasThis=%s  args=%s' % (b['ret'], b['hasThis'], b['args']))
    print('LOCALS %s' % json.dumps(b['locals'], ensure_ascii=False))
    if m['phantom']:
        print('STRICT-PHANTOM locals %s' % m['phantom'])
    if m['error']:
        print('VERIFY %s' % m['error'])
    if 'cpp' in m:
        print('CPP    %d errors in %s' % (m['cpp']['errors'], m['cpp']['files']))
        print('CPP    first=%s' % m['cpp']['first'])
    print('HANDLERS %s' % json.dumps(b['handlers'], ensure_ascii=False))
    print('IL:')
    for x in b['instructions']:
        print('  IL_%04X  %-14s %s' % (x['offset'], x['opcode'], json.dumps(x['operand'], ensure_ascii=False)))
    print()
