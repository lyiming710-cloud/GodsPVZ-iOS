python3 - <<'PY'
import os, re, json, collections
ROOT='/workspaces/GodsPVZ-native19/.validation/native22'
PREB1='/workspaces/GodsPVZ-native19/.validation/native22-full2'   # unpatched (batch0)
POSTB1=ROOT+'/retest/clang'                                        # after batch 1
POSTB2=ROOT+'/retest2/clang'                                       # after batch 2
ERR=re.compile(r'^([^:]+\.(?:cpp|c)):(\d+):(\d+): error: (.*)$')

def fam(m):
    if 'comparison between pointer and integer' in m: return 'ptr-vs-int'
    if 'assigning to' in m or 'incompatible integer to pointer conversion' in m: return 'assign-type'
    if 'no matching function for call to' in m: return 'call-signature'
    if 'invalid operands to binary expression' in m: return 'invalid-operands'
    if 'C-style cast' in m: return 'cast-not-allowed'
    if 'member reference base type' in m or 'no member named' in m: return 'member-access'
    return 'other'

def scan(d):
    tot=0; per=collections.Counter(); fams=collections.Counter()
    for lg in sorted(os.listdir(d)):
        if not lg.endswith('.log'): continue
        n=0
        for line in open(os.path.join(d,lg),errors='replace'):
            m=ERR.match(line)
            if m and os.path.basename(m.group(1))==lg[:-4]:
                n+=1; fams[fam(m.group(4))]+=1
        per[lg[:-4]]=n; tot+=n
    return tot, per, fams

a,pa,fa=scan(PREB1)
b,pb,fb=scan(POSTB1)
c,pc,fc=scan(POSTB2)
print('=== total clang error lines ===')
print('  batch0 (unpatched) : %5d   failing files=%d'%(a,sum(1 for v in pa.values() if v)))
print('  after batch 1      : %5d   failing files=%d'%(b,sum(1 for v in pb.values() if v)))
print('  after batch 2      : %5d   failing files=%d'%(c,sum(1 for v in pc.values() if v)))
print('  batch2 delta       : %+d'%(c-b))
print()
print('=== diagnostic families ===')
keys=sorted(set(fa)|set(fb)|set(fc))
print('   %-20s %8s %8s %8s'%('family','batch0','b1','b2'))
for k in keys:
    print('   %-20s %8d %8d %8d   (%+d)'%(k,fa[k],fb[k],fc[k],fc[k]-fb[k]))
print()
print('=== per-file changes, batch1 -> batch2 ===')
ch=[]
for f in sorted(set(pb)|set(pc)):
    x=pb.get(f,0); y=pc.get(f,0)
    if x!=y: ch.append((f,x,y,y-x))
for f,x,y,d in sorted(ch,key=lambda r:r[3]):
    print('   %-34s %6d -> %6d   %+d'%(f,x,y,d))
if not ch: print('   (none)')
print()
print('=== regressions (a file that was clean after batch1 but fails after batch2) ===')
reg=[f for f in pc if pc.get(f,0)>0 and pb.get(f,0)==0]
print('   %s'%(reg or 'none'))
print()
print('=== files newly clean ===')
nc=[f for f in pb if pb.get(f,0)>0 and pc.get(f,0)==0]
print('   %s'%(nc or 'none'))
PY
