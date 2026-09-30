cd /workspaces/GodsPVZ-native19/.validation/native22
python3 - <<'PY'
import json
W='/workspaces/GodsPVZ-native19/.validation/native22/work'
a=json.load(open(W+'/all-methods.batch1.json'))
b=json.load(open(W+'/all-methods.batch2.json'))
da={m['token']:m for m in a['methods']}
db={m['token']:m for m in b['methods']}

def sig(x, idx):
    """opcode + operand; branch targets and nested instruction references are
    expressed as INSTRUCTION INDEX so the 4-byte offset shift cannot look like a
    control-flow change."""
    def conv(v):
        if isinstance(v, dict):
            if 'opcode' in v and 'offset' in v:
                return '<instr#%d>' % idx[v['offset']]
            if 'target' in v and isinstance(v['target'], int):
                return {'target': '#%d' % idx[v['target']]}
            if 'targets' in v:
                return {'targets': ['#%d' % idx[t] for t in v['targets']]}
            return {k: conv(u) for k, u in v.items()}
        if isinstance(v, list):
            return [conv(u) for u in v]
        return v
    return (x['opcode'], json.dumps(conv(x.get('operand')), sort_keys=True))

changed=[t for t in da if json.dumps(da[t],sort_keys=True)!=json.dumps(db[t],sort_keys=True)]
diffs=0; kinds={}; anomalies=[]; lenbad=[]; ehdone=0
for t in changed:
    ia=da[t]['instructions']; ib=db[t]['instructions']
    if len(ia)!=len(ib):
        lenbad.append((t,len(ia),len(ib))); continue
    idxa={x['offset']:k for k,x in enumerate(ia)}
    idxb={x['offset']:k for k,x in enumerate(ib)}
    for k,(x,y) in enumerate(zip(ia,ib)):
        sx,sy=sig(x,idxa),sig(y,idxb)
        if sx==sy: continue
        diffs+=1
        kinds[(sx[0],sy[0])]=kinds.get((sx[0],sy[0]),0)+1
        if not (sx[0].startswith('ldc.i4') and sy[0]=='ldnull'):
            anomalies.append((t,'#%d %s -> %s'%(k,sx[0],sy[0])))
    for h1,h2 in zip(da[t].get('handlers') or [], db[t].get('handlers') or []):
        ehdone+=1
        for k in ('tryStart','tryEnd','handlerStart','handlerEnd'):
            if idxa[h1[k]] != idxb[h2[k]]:
                anomalies.append((t,'EH %s index %d -> %d'%(k,idxa[h1[k]],idxb[h2[k]])))
print('methods changed: %d'%len(changed))
print('methods whose instruction COUNT changed: %s'%(lenbad or 'none'))
print('instruction-level diffs: %d'%diffs)
print('diff kinds:')
for k,v in sorted(kinds.items(), key=lambda kv:-kv[1]):
    print('   %-12s -> %-10s %d'%(k[0],k[1],v))
print('anomalies: %d'%len(anomalies))
for x in anomalies[:20]: print('   ',x)
E=json.load(open('/workspaces/GodsPVZ-native19/.validation/native22/edits-batch2.json'))['edits']
print()
print('edits in the edit list: %d'%len(E))
print('EH regions compared: %d'%ehdone)
print('RESULT: only the 1623 planned literal swaps: %s'
      %(diffs==len(E) and not anomalies and not lenbad))
PY
