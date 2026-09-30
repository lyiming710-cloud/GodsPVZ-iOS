cd /workspaces/GodsPVZ-native19/.validation/native22
python3 - <<'PY'
import json
W='/workspaces/GodsPVZ-native19/.validation/native22/work'
a=json.load(open(W+'/all-methods.batch1.json'))
b=json.load(open(W+'/all-methods.batch2.json'))
da={m['token']:m for m in a['methods']}
db={m['token']:m for m in b['methods']}

def strip(o):
    """Branch/switch operands embed whole Instruction objects; strip them so that
    a change elsewhere cannot masquerade as a change to the branch itself."""
    if isinstance(o,dict):
        if 'opcode' in o and 'offset' in o:
            return '<instr@%04X>'%o['offset']
        return {k:strip(v) for k,v in o.items()}
    if isinstance(o,list): return [strip(x) for x in o]
    return o

def norm_ins(x):
    return (x['offset'], x['opcode'], json.dumps(strip(x.get('operand')),sort_keys=True))

changed=[t for t in da if json.dumps(da[t],sort_keys=True)!=json.dumps(db[t],sort_keys=True)]
print('methods changed: %d'%len(changed))
diffs=0; kinds={}; anomalies=[]
for t in changed:
    ia=da[t]['instructions']; ib=db[t]['instructions']
    if len(ia)!=len(ib):
        anomalies.append((t,'length %d != %d'%(len(ia),len(ib)))); continue
    for x,y in zip(ia,ib):
        nx,ny=norm_ins(x),norm_ins(y)
        if nx==ny: continue
        diffs+=1
        kinds[(nx[1],ny[1])]=kinds.get((nx[1],ny[1]),0)+1
        if not (nx[0]==ny[0] and nx[1].startswith('ldc.i4') and ny[1]=='ldnull'):
            anomalies.append((t,'IL_%04X %s -> %s'%(nx[0],nx[1],ny[1])))
print('instruction-level diffs: %d'%diffs)
print('diff kinds (before -> after : count):')
for k,v in sorted(kinds.items(), key=lambda kv:-kv[1]):
    print('   %-12s -> %-10s %d'%(k[0],k[1],v))
print('anomalies: %d'%len(anomalies))
for x in anomalies[:20]: print('   ',x)
print()
print('EXPECTED DIFFS (from the edit list): %d'
      % len(json.load(open('/workspaces/GodsPVZ-native19/.validation/native22/edits-batch2.json'))['edits']))
PY
