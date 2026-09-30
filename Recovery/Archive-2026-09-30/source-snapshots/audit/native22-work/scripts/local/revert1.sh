cd /workspaces/GodsPVZ-native19/.validation/native22
python3 - <<'PY'
import json, subprocess, os, sys
ROOT='/workspaces/GodsPVZ-native19/.validation/native22'
W=ROOT+'/work'
base=json.load(open(ROOT+'/edits-batch1.json'))
edits=base['edits']
label=[e['evidence'][:70] for e in edits]
env=dict(os.environ)
env['GODSPVZ_RESOLVER']='/workspaces/GodsPVZ-native19/.validation/native21/replay/canary/Library/Bee/artifacts/iOS/ManagedStripped'
sys.path.insert(0,ROOT)

def run(dll, outjson):
    r=subprocess.run(['dotnet',ROOT+'/bin/PatcherNative19.dll',dll,outjson,'export-all'],
                     capture_output=True,text=True,env=env)
    if r.returncode!=0:
        print('   export failed:',r.stderr[-300:]); return None
    return json.load(open(outjson))

from native23_verify import verify_all

print('%-4s %-12s %-8s %-6s  %s'%('#','token','offset','action','result'))
print('-'*104)
results={}
for i,e in enumerate(edits):
    variant={'edits':[x for j,x in enumerate(edits) if j!=i]}
    p=f'{W}/revert{i}.json'
    json.dump(variant,open(p,'w'),indent=1)
    dll=f'{W}/revert{i}.dll'
    r=subprocess.run(['dotnet',ROOT+'/bin23/RepairNative23.dll',f'{W}/stage0-batch1.dll',dll,p,f'{W}/revert{i}.report.txt'],
                     capture_output=True,text=True,env=env)
    if r.returncode!=0:
        print('%-4d PATCH-ABORT %s'%(i,r.stderr.strip()[:200])); results[i]=('ABORT',''); continue
    dump=run(dll,f'{W}/all-methods.revert{i}.json')
    v=verify_all(dump)
    tok=e['token']
    err=v.get(tok)
    status='FAILS AGAIN' if err else '*** STILL PASSES ***'
    print('%-4d %-12s IL_%04X  %-11s %s'%(i,tok,e['offset'],e['action'],status))
    print('     %s'%label[i])
    if err: print('     verifier: %s'%err[:150])
    else:   print('     verifier: <no error> -- this edit is NOT independently proven necessary')
    results[i]=(status,err)
    print()
json.dump({str(k):v for k,v in results.items()},open(f'{W}/revert-matrix.json','w'),indent=1)
bad=[k for k,(s,_) in results.items() if s!='FAILS AGAIN']
print('edits not independently necessary: %s'%bad)
PY
