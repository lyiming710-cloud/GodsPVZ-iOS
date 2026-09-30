python3 - <<'PYEOF'
import json, os, re, sys
ROOT='/workspaces/GodsPVZ-native19/.validation/native22'
NEWCPP='/workspaces/GodsPVZ-native19/.validation/native22/regen-batch1/cpp'
NEWCLANG='/workspaces/GodsPVZ-native19/.validation/native22/retest/clang'
CAN='/workspaces/GodsPVZ-native19/.validation/native21/replay/canary/Library/Bee/artifacts/iOS/il2cppOutput/cpp'
OLDCLANG='/workspaces/GodsPVZ-native19/.validation/native22-full2'
TARGET='/workspaces/GodsPVZ-native19/.validation/native22/out/body-map.new.json'

DEF_RE=re.compile(r'IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR[^\n]*?\b([A-Za-z0-9_]+)\s*\(')
ERR_RE=re.compile(r'^([^:]+\.(?:cpp|c)):(\d+):(\d+): error: (.*)$')

def table(d):
    lines=open(os.path.join(d,'GodsPVZRuntime1_CodeGen.c'),encoding='utf-8-sig').read().splitlines()
    s=next(i for i,l in enumerate(lines) if 's_methodPointers[' in l)
    e=next(i for i in range(s,len(lines)) if lines[i].startswith('}'))
    t={}
    for pos,l in enumerate(lines[s+2:e]):
        v=l.strip().rstrip(',')
        if v and v!='NULL': t.setdefault(v,[]).append('0x%08X'%(0x06000000+pos+1))
    return t

def build(cppdir, clangdir, out):
    sym2tok=table(cppdir)
    diags={}
    for lg in sorted(os.listdir(clangdir)):
        if not lg.endswith('.log'): continue
        base=lg[:-4]
        for line in open(os.path.join(clangdir,lg),errors='replace'):
            m=ERR_RE.match(line)
            if m and os.path.basename(m.group(1))==base:
                diags.setdefault(base,{}).setdefault(int(m.group(2)),[]).append((int(m.group(3)),m.group(4)))
    res={}
    for name in sorted(os.listdir(cppdir)):
        if not name.endswith('.cpp'): continue
        p=os.path.join(cppdir,name)
        src=open(p,encoding='utf-8-sig',errors='replace').read().splitlines()
        n=len(src)
        pos=[]
        for i,l in enumerate(src,1):
            if 'IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR' in l:
                m=DEF_RE.search(l)
                if m: pos.append((m.group(1),i))
        fd=diags.get(name,{})
        if not pos and not fd: continue
        for k,(sym,sline) in enumerate(pos):
            end=pos[k+1][1]-1 if k+1<len(pos) else n
            for j in range(sline,min(end,n)+1):
                if src[j-1]=='}': end=j; break
            for tok in sym2tok.get(sym,[]):
                errs=[]
                for ln in sorted(fd):
                    if sline<=ln<=end:
                        for col,msg in fd[ln]: errs.append({'line':ln,'col':col,'msg':msg})
                res[tok]={'symbol':sym,'file':name,'start':sline,'end':end,'errors':errs}
    json.dump(res,open(out,'w'))
    return res

old=json.load(open(ROOT+'/out/body-map.json'))
new=build(NEWCPP,NEWCLANG,TARGET)
print('tokens mapped: old=%d new=%d'%(len(old),len(new)))
print('tokens with >=1 error: old=%d new=%d'%(sum(1 for v in old.values() if v['errors']),sum(1 for v in new.values() if v['errors'])))
fixed=[t for t in old if old[t].get('errors') and not new.get(t,{}).get('errors')]
worse=[t for t in old if not old[t].get('errors') and new.get(t,{}).get('errors')]
reduced=[t for t in old if len(old[t].get('errors',[]))>len(new.get(t,{}).get('errors',[]))]
grown=[t for t in old if len(old[t].get('errors',[]))<len(new.get(t,{}).get('errors',[]))]
print()
print('cleared completely   : %s'%(sorted(fixed) or 'none'))
print('NEWLY broken (bad!)  : %s'%(sorted(worse) or 'none'))
print('error count reduced  : %s'%(sorted(reduced) or 'none'))
print('error count INCREASED: %s'%(sorted(grown) or 'none'))
print()
tot_o=sum(len(v.get('errors',[])) for v in old.values())
tot_n=sum(len(v.get('errors',[])) for v in new.values())
print('error lines inside mapped bodies: %d -> %d (%+d)'%(tot_o,tot_n,tot_n-tot_o))
print()
print('=== the three targets after repair ===')
for t in ['0x06000348','0x0600034C','0x06000339']:
    o=old.get(t); n_=new.get(t)
    print(' %s %-34s  before=%d  after=%d'%(t, (o or {}).get('symbol','?'), len((o or {}).get('errors',[])), len((n_ or {}).get('errors',[]))))
    if n_ and n_.get('errors'):
        for e in n_['errors']: print('      still: %s:%d %s'%(n_['file'],e['line'],e['msg'][:110]))

PYEOF
