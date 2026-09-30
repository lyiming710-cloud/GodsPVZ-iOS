cd /workspaces/GodsPVZ-native19/.validation/native22
cat > /workspaces/GodsPVZ-native19/.validation/native22/b2_revert.py <<'PYEOF'
import json, os, subprocess, collections
ROOT='/workspaces/GodsPVZ-native19/.validation/native22'
W=ROOT+'/work'
env=dict(os.environ)
env['GODSPVZ_RESOLVER']='/workspaces/GodsPVZ-native19/.validation/native21/replay/canary/Library/Bee/artifacts/iOS/ManagedStripped'
import native23_multi as M

edits=json.load(open(ROOT+'/edits-batch2.json'))['edits']

SAMPLE=['0x060000D9','0x06000338','0x0600043B','0x06000263','0x06000045','0x060002FF',
        '0x06000360','0x060001DB','0x060003A9','0x0600047A','0x0600018B','0x06000334',
        '0x06000493','0x0600057E','0x06000585','0x06000041','0x060001A7','0x060003B5',
        '0x060004AA','0x0600051B','0x06000156','0x06000507','0x06000634','0x060005C8']

def dump(dll,out):
    r=subprocess.run(['dotnet',ROOT+'/bin/PatcherNative19.dll',dll,out,'export-all'],
                     capture_output=True,text=True,env=env)
    if r.returncode!=0:
        print('   EXPORT FAILED',r.stderr[-200:]); return None
    return json.load(open(out))

def clash(d,tok):
    types=d['types']
    for m in d['methods']:
        if m['token']==tok: return M.verify_multi(m,types)
    return None

full=json.load(open(W+'/all-methods.batch2.json'))
print('%-12s %-6s %-8s %-8s  %s'%('token','IL_off','with all','without','verdict'))
print('-'*88)
bad=[]
for tok in SAMPLE:
    e=[x for x in edits if x['token']==tok][0]
    off=e['offset']
    variant={'edits':[x for x in edits if x is not e]}
    p=f'{W}/b2rev_{tok}.json'; json.dump(variant,open(p,'w'),indent=0)
    dll=f'{W}/b2rev_{tok}.dll'
    r=subprocess.run(['dotnet',ROOT+'/bin23/RepairNative23.dll',f'{W}/batch1.dll',dll,p,f'{W}/b2rev_{tok}.report.txt'],
                     capture_output=True,text=True,env=env)
    if r.returncode!=0:
        print('%-12s PATCH ABORT %s'%(tok,r.stderr.strip()[:150])); bad.append(tok); continue
    d=dump(dll,f'{W}/b2rev_{tok}.methods.json')
    if d is None: bad.append(tok); continue
    a=len(clash(full,tok) or [])
    b_=clash(d,tok) or []
    verdict='FAILS AGAIN' if len(b_)>a else '*** still passes ***'
    if len(b_)<=a: bad.append(tok)
    print('%-12s IL_%04X  %-8d %-8d  %s'%(tok,off,a,len(b_),verdict))
    for x in b_[:1]:
        print('              reappears: IL_%04X %s'%(x['offset'],x['msg'][:90]))
print()
print('edits NOT proven necessary: %s'%(bad or 'none'))
PYEOF
nohup python3 -u /workspaces/GodsPVZ-native19/.validation/native22/b2_revert.py > /workspaces/GodsPVZ-native19/.validation/native22/b2_revert.out.txt 2>&1 &
echo "started pid=$!"
sleep 25
tail -5 /workspaces/GodsPVZ-native19/.validation/native22/b2_revert.out.txt
