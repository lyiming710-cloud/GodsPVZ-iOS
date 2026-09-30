cd /workspaces/GodsPVZ-native19/.validation/native22
python3 - <<'PY'
import json
W='/workspaces/GodsPVZ-native19/.validation/native22/work'
a=json.load(open(W+'/all-methods.batch1.json'))
b=json.load(open(W+'/all-methods.batch2.json'))
da={m['token']:m for m in a['methods']}
db={m['token']:m for m in b['methods']}

def strip(o):
    if isinstance(o,dict):
        if 'opcode' in o and 'offset' in o: return '<instr@%04X>'%o['offset']
        return {k:strip(v) for k,v in o.items()}
    if isinstance(o,list): return [strip(x) for x in o]
    return o

def sig(x):
    return (x['opcode'], json.dumps(strip(x.get('operand')),sort_keys=True))

changed=[t for t in da if json.dumps(da[t],sort_keys=True)!=json.dumps(db[t],sort_keys=True)]
print('methods changed: %d'%len(changed))
diffs=0; kinds={}; anomalies=[]; lenbad=[]
for t in changed:
    ia=da[t]['instructions']; ib=db[t]['instructions']
    if len(ia)!=len(ib):
        lenbad.append((t,len(ia),len(ib))); continue
    for k,(x,y) in enumerate(zip(ia,ib)):
        sx,sy=sig(x),sig(y)
        if sx==sy: continue
        diffs+=1
        kinds[(sx[0],sy[0])]=kinds.get((sx[0],sy[0]),0)+1
        if not (sx[0].startswith('ldc.i4') and sy[0]=='ldnull'):
            anomalies.append((t,'#%d %s -> %s'%(k,sx[0],sy[0])))
print('methods whose instruction COUNT changed: %s'%(lenbad or 'none'))
print('instruction-level diffs (compared by position): %d'%diffs)
print('diff kinds:')
for k,v in sorted(kinds.items(), key=lambda kv:-kv[1]):
    print('   %-12s -> %-10s %d'%(k[0],k[1],v))
print('anomalies (anything that is not ldc.i4* -> ldnull): %d'%len(anomalies))
for x in anomalies[:20]: print('   ',x)
print()
E=json.load(open('/workspaces/GodsPVZ-native19/.validation/native22/edits-batch2.json'))['edits']
print('edits in the edit list: %d'%len(E))
print('match: %s'%(diffs==len(E) and not anomalies and not lenbad))
PY
