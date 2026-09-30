cd /workspaces/GodsPVZ-native19/.validation/native22
python3 - <<'PY'
import json, struct

class P:
    def __init__(self, path):
        self.data=open(path,'rb').read()
        d=self.data
        coff=struct.unpack_from('<I',d,0x3C)[0]
        _m,nsec,_t,_s,_ns,opt,_c=struct.unpack_from('<HHIIIHH',d,coff)
        self.image_base=struct.unpack_from('<Q',d,coff+24+24)[0]
        opt=coff+24
        self.secs=[]
        o=opt+240
        for i in range(nsec):
            name=d[o:o+8].rstrip(b'\0').decode()
            vsize,va,rsize,roff=struct.unpack_from('<IIII',d,o+8)
            self.secs.append((name,va,vsize,roff,rsize))
            o+=40
    def off(self, rva):
        for name,va,vsize,roff,rsize in self.secs:
            if va<=rva<va+max(vsize,rsize):
                return roff+(rva-va)
        raise ValueError('rva 0x%x unmapped'%rva)
    def pdata(self):
        for name,va,vsize,roff,rsize in self.secs:
            if name=='.pdata':
                return self.data[roff:roff+rsize], va, rsize
        raise ValueError('no .pdata')

pe=P('pcnative/GameAssembly.dll')
print('image_base=0x%X'%pe.image_base)
blob,pva,psz=pe.pdata()
n=psz//12
print('.pdata entries=%d'%n)
ents=[]
for i in range(n):
    b,e,u=struct.unpack_from('<III',blob,i*12)
    ents.append((pe.image_base+b, pe.image_base+e, pe.image_base+u))
ents.sort()
print('first=%08X last=%08X'%(ents[0][0],ents[-1][0]))

mp=json.load(open('pcnative/native-method-map.json'))
print('map type',type(mp),'len',len(mp))
def find(tok):
    if isinstance(mp,dict): return mp.get(tok)
    for x in mp:
        if str(x.get('token','')).lower()==tok.lower(): return x
    return None
for tok in ['0x06000348','0x0600034C','0x06000339','0x06000443']:
    e=find(tok)
    print(tok,'->',e)
    if not e: continue
    va=e.get('va') or e.get('address') or e.get('image')
    if va is None:
        print('   keys:',list(e)); continue
    # entries whose begin is exactly va
    exact=[x for x in ents if x[0]==va]
    print('   VA=0x%08X  exact .pdata entries=%d %s'%(va,len(exact),['%08X..%08X'%(a,b) for a,b,_ in exact]))
    if not exact:
        # nearest begins
        import bisect
        begins=[x[0] for x in ents]
        i=bisect.bisect_left(begins,va)
        for j in range(max(0,i-2),min(len(ents),i+3)):
            print('      near[%d] %08X..%08X size=%d'%(j,ents[j][0],ents[j][1],ents[j][1]-ents[j][0]))
PY
