cd /workspaces/GodsPVZ-native19/.validation/native22
python3 - <<'PY'
import json, struct, bisect, collections
exec(open('native22_native_probe.py').read().split('def read_cstr')[0].replace('def __init__(self, data):','def __init__(self, path=None, data=None):\n        if path: data=open(path,"rb").read()'))
pe=PE(path='pcnative/GameAssembly.dll')
sec=[s for s in pe.sections if s[0]=='.pdata'][0]
name,sva,span,roff,rsize=sec
ents=[]
for o in range(roff, roff+rsize-11, 12):
    b,e,u=struct.unpack_from('<III',pe.data,o)
    ents.append((pe.image_base+b, pe.image_base+e))
ents=set(ents); ents=sorted(ents)
begins=[x[0] for x in ents]
print('.pdata entries=%d'%len(ents))

mp=json.load(open('pcnative/native-method-map.json'))
print('image counts:',collections.Counter(x['image'] for x in mp).most_common(5))
idx={}
for x in mp:
    t='0x%06X'%int(x['token'],16)
    idx.setdefault(t,[]).append(x)
print('Assembly-CSharp tokens in map: %d'%len([t for t,xs in idx.items() if xs[0]['image']=='Assembly-CSharp.dll']))
def norm(t): return '0x%06X'%int(t,16)
for tok in ['0x06000348','0x0600034C','0x06000339','0x06000443','0x0600034B']:
    xs=idx.get(norm(tok),[])
    print('%s -> %s'%(tok,[(x['image'],x['type'],x['name'],x['va']) for x in xs]))
    for x in xs:
        va=int(x['va'],16)
        i=bisect.bisect_left(begins,va)
        exact=[e for e in ents if e[0]==va]
        print('     VA=0x%08X exact=%s'%(va,['%08X..%08X size=%d'%(a,b,b-a) for a,b in exact]))
        if not exact:
            for j in range(max(0,i-2),min(len(ents),i+3)):
                print('        near %08X..%08X size=%d'%(ents[j][0],ents[j][1],ents[j][1]-ents[j][0]))
PY
