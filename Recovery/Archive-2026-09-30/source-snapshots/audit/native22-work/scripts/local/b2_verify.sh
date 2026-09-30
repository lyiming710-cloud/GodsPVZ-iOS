cd /workspaces/GodsPVZ-native19/.validation/native22
W=/workspaces/GodsPVZ-native19/.validation/native22/work
export GODSPVZ_RESOLVER=/workspaces/GodsPVZ-native19/.validation/native21/replay/canary/Library/Bee/artifacts/iOS/ManagedStripped
dotnet bin/PatcherNative19.dll "$W/batch2.dll" "$W/all-methods.batch2.json" export-all > "$W/export-batch2.log" 2>&1
echo "export rc=$?"
python3 - <<'PY'
import json, collections
W='/workspaces/GodsPVZ-native19/.validation/native22/work'
a=json.load(open(W+'/all-methods.batch1.json'))
b=json.load(open(W+'/all-methods.batch2.json'))
da={m['token']:m for m in a['methods']}
db={m['token']:m for m in b['methods']}
print('=== G1 NON-TARGET ISOLATION ===')
print('methods before=%d after=%d'%(len(da),len(db)))
print('token sets identical:', set(da)==set(db))
same=0; changed=[]
for t,m in da.items():
    n=db.get(t)
    if n is None: continue
    if json.dumps(m,sort_keys=True)==json.dumps(n,sort_keys=True): same+=1
    else: changed.append(t)
print('unchanged=%d  CHANGED=%d'%(same,len(changed)))
print('types unchanged:', json.dumps(a['types'],sort_keys=True)==json.dumps(b['types'],sort_keys=True))
C=set(json.load(open('/workspaces/GodsPVZ-native19/.validation/native22/out/afam2-candidates.json')))
print('changed set == planned candidate set:', set(changed)==C)
print('  changed but not planned:', sorted(set(changed)-C)[:10])
print('  planned but not changed:', sorted(C-set(changed))[:10])

# ---- per-method: how many instructions changed, and are they only ldnull swaps?
import copy
only_ldnull=True; bad=[]
instr_total=0
for t in changed:
    ia=da[t]['instructions']; ib=db[t]['instructions']
    if len(ia)!=len(ib): bad.append((t,'length %d!=%d'%(len(ia),len(ib)))); continue
    for x,y in zip(ia,ib):
        if x==y: continue
        instr_total+=1
        if not (x['opcode'].startswith('ldc.i4') and y['opcode']=='ldnull' and x['offset']==y['offset']):
            only_ldnull=False; bad.append((t,'%s -> %s @IL_%04X'%(x['opcode'],y['opcode'],x['offset'])))
print('instruction-level diffs: %d   all of them ldc.i4*->ldnull at the same offset: %s'%(instr_total,only_ldnull))
for x in bad[:10]: print('   anomaly:',x)
PY
echo
echo "=== G2 TYPED VERIFICATION (fail-closed oracle, whole dump) ==="
python3 native23_verify.py "$W/all-methods.batch1.json" "$W/all-methods.batch2.json" 2>&1 | tail -12
echo
echo "=== G2b MULTI-ERROR ORACLE: total clashes in the whole dump ==="
python3 - <<'PY'
import json
import native23_multi as M
W='/workspaces/GodsPVZ-native19/.validation/native22/work'
for tag,p in (('before (batch1)',W+'/all-methods.batch1.json'),('after  (batch2)',W+'/all-methods.batch2.json')):
    d=json.load(open(p)); types=d['types']
    tot=0; meth=0; afam=0
    import native23_afam as A
    for m in d['methods']:
        e=M.verify_multi(m,types)
        if e:
            meth+=1; tot+=len(e)
            afam+=sum(1 for x in e if x['offset']>=0 and (lambda o: o is not None and A.is_ref(o,types))(A.afam_other(x['msg'])))
    print('  %-18s methods with clashes=%4d  total clashes=%5d  of which I4-vs-reference=%4d'%(tag,meth,tot,afam))
PY
