python3 - <<'PY'
import os, re, collections
ROOT='/workspaces/GodsPVZ-native19/.validation/native22'
BASE='/workspaces/GodsPVZ-native19/.validation/native22-full2'          # baseline clang logs (from canary tree, unpatched IL)
NEW=ROOT+'/retest/clang'             # fresh clang logs (regen-batch1, patched IL)
ERR=re.compile(r'^([^:]+\.(?:cpp|c)):(\d+):(\d+): error: (.*)$')

def count(d):
    tot=0; per=collections.Counter(); lines={}
    for lg in sorted(os.listdir(d)):
        if not lg.endswith('.log'): continue
        n=0
        for line in open(os.path.join(d,lg),errors='replace'):
            m=ERR.match(line)
            if m and os.path.basename(m.group(1))==lg[:-4]:
                n+=1; lines.setdefault(lg[:-4],[]).append(int(m.group(2)))
        per[lg[:-4]]=n; tot+=n
    return tot, per, lines

bt,bp,bl=count(BASE)
nt,np,nl=count(NEW)
print('=== total clang error lines ===')
print('  baseline (unpatched) : %d   failing files=%d'%(bt,sum(1 for v in bp.values() if v)))
print('  after batch1 repair  : %d   failing files=%d'%(nt,sum(1 for v in np.values() if v)))
print('  delta                : %+d'%(nt-bt))
print()
print('=== per-file changes ===')
changed=[]
for f in sorted(set(bp)|set(np)):
    b=bp.get(f,0); n=np.get(f,0)
    if b!=n:
        changed.append((f,b,n))
        print('   %-32s %6d -> %6d   %+d'%(f,b,n,n-b))
if not changed: print('   (no file changed)')
print()
print('=== newly failing files (regressions) ===')
reg=[f for f in np if np.get(f,0)>0 and bp.get(f,0)==0]
print('   %s'%(reg or 'none'))
PY
