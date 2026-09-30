cd /workspaces/GodsPVZ-native19/.validation/native22
python3 - <<'PY'
import json, struct, bisect
exec(open('native22_native_probe.py').read().split('def read_cstr')[0].replace('def __init__(self, data):','def __init__(self, path=None, data=None):\n        if path: data=open(path,"rb").read()'))
pe=PE(path='pcnative/GameAssembly.dll')
print('image_base=0x%X'%pe.image_base)
print('sections:',[(n,hex(va),hex(span)) for n,va,span,_,_ in pe.sections][:20])

sec=[s for s in pe.sections if s[0]=='.pdata'][0]
name,sva,span,roff,rsize=sec
ents=[]
for o in range(roff, roff+rsize-11, 12):
    b,e,u=struct.unpack_from('<III',pe.data,o)
    ents.append((pe.image_base+b, pe.image_base+e))
ents.sort()
print('.pdata entries=%d first=%08X last=%08X'%(len(ents),ents[0][0],ents[-1][0]))

mp=json.load(open('pcnative/native-method-map.json'))
def find(tok):
    if isinstance(mp,dict): return mp.get(tok)
    for x in mp:
        if str(x.get('token','')).lower()==tok.lower(): return x
for tok in ['0x06000348','0x0600034C','0x06000339','0x06000443']:
    e=find(tok)
    va=e.get('va')
    exact=[x for x in ents if x[0]==va]
    print('%s -> VA=0x%08X  exact entries=%d %s'%(tok,va,len(exact),['%08X..%08X (%d B)'%(a,b,b-a) for a,b in exact]))
    if not exact:
        i=bisect.bisect_left([x[0] for x in ents], va)
        for j in range(max(0,i-2), min(len(ents), i+3)):
            print('     near %08X..%08X size=%d'%(ents[j][0],ents[j][1],ents[j][1]-ents[j][0]))
PY
