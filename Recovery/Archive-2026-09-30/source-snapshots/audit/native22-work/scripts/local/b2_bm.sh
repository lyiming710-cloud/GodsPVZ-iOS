python3 - <<'PY'
import json, os, re
ROOT='/workspaces/GodsPVZ-native19/.validation/native22'
OLDCPP=ROOT+'/regen-batch1/cpp'
OLDCLANG=ROOT+'/retest/clang'
NEWCPP=ROOT+'/regen-batch2/cpp'
NEWCLANG=ROOT+'/retest2/clang'
TARGET=ROOT+'/out/body-map.b2.json'
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
        src=open(os.path.join(cppdir,name),encoding='utf-8-sig',errors='replace').read().splitlines()
        n=len(src); pos=[]
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

old=json.load(open(ROOT+'/out/body-map.new.json'))   # after batch 1
new=build(NEWCPP,NEWCLANG,TARGET)                    # after batch 2
print('tokens mapped: b1=%d b2=%d'%(len(old),len(new)))
print('tokens with >=1 error: b1=%d b2=%d'%(sum(1 for v in old.values() if v['errors']),
                                            sum(1 for v in new.values() if v['errors'])))
fixed=[t for t in old if old[t].get('errors') and not new.get(t,{}).get('errors')]
worse=[t for t in old if not old[t].get('errors') and new.get(t,{}).get('errors')]
grown=[t for t in old if len(old.get(t,{}).get("errors",[]))<len(new.get(t,{}).get("errors",[]))]
reduced=[t for t in old if len(old.get(t,{}).get("errors",[]))>len(new.get(t,{}).get("errors",[]))]
print()
print('cleared completely   : %d tokens'%len(fixed))
print('NEWLY broken (bad!)  : %s'%(sorted(worse) or 'none'))
print('error count INCREASED: %s'%(sorted(grown) or 'none'))
print('error count reduced  : %d tokens'%len(reduced))
to=sum(len(v.get('errors',[])) for v in old.values())
tn=sum(len(v.get('errors',[])) for v in new.values())
print('error lines inside mapped bodies: %d -> %d (%+d)'%(to,tn,tn-to))
C=set(json.load(open(ROOT+'/out/afam2-candidates.json')))
print()
print('planned candidates: %d'%len(C))
print('  cleared completely   : %d'%len([t for t in fixed if t in C]))
print('  reduced but not clear: %d'%len([t for t in reduced if t in C]))
print('  unchanged meanwhile  : %d'%len([t for t in C if len(old.get(t,{}).get("errors",[]))==len(new.get(t,{}).get("errors",[]))]))
print('  any candidate that GREW: %s'%(sorted(t for t in C if len(new.get(t,{}).get("errors",[]))>len(old.get(t,{}).get("errors",[]))) or 'none'))
print()
print('tokens fixed that were NOT planned candidates: %s'%(sorted(t for t in fixed if t not in C) or 'none'))
PY
