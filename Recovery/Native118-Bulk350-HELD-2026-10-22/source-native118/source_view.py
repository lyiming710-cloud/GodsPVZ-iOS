"""Read saved source data for manual authoring, never execute project code."""
from pathlib import Path
import json,sys
P=Path(__file__).resolve().parent
c=json.loads((P/'CURRENT-SOURCE.json').read_text(encoding='utf-8'))
o=json.loads((P/'ORIGINAL-SOURCE.json').read_text(encoding='utf-8'))
root=P.parents[1]
maps=json.loads((root/'audit/offline-test-handoff-2026-10-14/payload/inputs/maps/native-method-map.json').read_text(encoding='utf-8-sig'))
byva={int(m['va'],16):m['type']+'::'+m['name'] for m in maps}
cells={int(m['address'],16):m.get('text','') for m in o['cells']}
for token in sys.argv[1:]:
    tok=f'0x{int(token,16):08X}';m=next(m for m in c['methods'] if m['token']==tok)
    a=next(m for m in o['methods'] if m['token']==tok)
    print('\n###',tok,m['name'],m['signature'],'locals',m['locals_blob'])
    print('SOURCE BINDINGS')
    seen=set()
    for i in m['instructions']:
        v=i['operand']
        if isinstance(v,dict) and 'identity' in v and v['token'] not in seen:
            seen.add(v['token']);print(v['token'],v['identity'])
    print('ORIGINAL NATIVE')
    for i in a['instructions']:
        op=i['operands'];addr=int(i['address'],16);hint=''
        if i['mnemonic'] in ['call','jmp'] and op.startswith('0x'):hint=byva.get(int(op,16),'')
        # Retain all instructions except padding; authors inspect full saved asm as necessary.
        if i['mnemonic'] in ['int3','nop']:continue
        if i['mnemonic']=='call' and op=='0x18024fef0':continue
        if op.startswith('rcx, [rip +') and i['mnemonic']=='lea':continue
        print(i['address'],i['mnemonic'],op,hint)
