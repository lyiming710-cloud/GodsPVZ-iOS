cd /workspaces/GodsPVZ-native19/.validation/native22
python3 - <<'PY'
import json
ROOT='/workspaces/GodsPVZ-native19/.validation/native22'
C=json.load(open(ROOT+'/out/afam2-candidates.json'))
D=json.load(open(ROOT+'/work/all-methods.batch1.json'))
B={m['token']:m for m in D['methods']}
def dsc(o):
    if o is None: return ''
    if isinstance(o,dict): return o.get('identity', json.dumps(o,ensure_ascii=False))
    return json.dumps(o,ensure_ascii=False)
for tok in ['0x060005C8','0x06000360']:
    c=C[tok]; b=B[tok]
    marks={f['offset'] for f in c['flips']}
    print('='*90)
    print('%s %s  flips=%d  status=%s'%(tok,c['name'],len(c['flips']),c['status']))
    print('clang file=%s  n=%d   A-family=%d'%(c['clang_file'],len(c['clang']),
          sum(1 for x in c['clang'] if 'comparison between pointer and integer' in x)))
    print('--- flips with context ---')
    ins=b['instructions']
    for k,x in enumerate(ins):
        if x['offset'] not in marks: continue
        for y in ins[max(0,k-3):k]:
            print('      IL_%04X %-14s %s'%(y['offset'],y['opcode'],dsc(y)))
        print('   >>>IL_%04X %-14s %s'%(x['offset'],x['opcode'],dsc(x)))
        for y in ins[k+1:k+3]:
            print('      IL_%04X %-14s %s'%(y['offset'],y['opcode'],dsc(y)))
        print('      ...')
PY
