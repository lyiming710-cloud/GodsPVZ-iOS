from pathlib import Path
import sys,json,subprocess,importlib.util,hashlib,collections,copy
v=Path(__file__).resolve().parent;root=v.parents[1];rev=sys.argv[1]
dotnet=root/'.local/dotnet10/dotnet.exe';parent=root/'audit/native93-codex-2026-10-17/verification-2026-10-17/candidates/candidate1.dll'
def run(step,args):subprocess.run([sys.executable,str(v/'run_step.py'),rev+'-'+step,*map(str,args)],cwd=root,check=True)
run('build',[dotnet,'build',v/('revision-'+rev)/'Bulk350.csproj','-c','Release','-o',v/(rev+'-bin'),'--nologo','-p:UseSharedCompilation=false','--disable-build-servers'])
run('construct',[dotnet,v/(rev+'-bin/Bulk350.dll'),parent,v/(rev+'-candidate.dll'),v/(rev+'-construction.json')])
plan=json.loads((v/'frozen-source/IMPLEMENTATION-PLAN.json').read_text('utf-8'));tokens=','.join(t['token'] for t in plan['targets'])
run('export',[dotnet,root/'audit/bulk250-r2-evidence-39731970762/bulk250-r2-run/tools_bin/CilExporter/bin/CilExporter.dll',v/(rev+'-candidate.dll'),v/(rev+'-targets.json'),root/'audit/offline-test-handoff-2026-10-14/payload/references/managed-support',tokens])
d=json.loads((v/(rev+'-targets.json')).read_text('utf-8'))
for label,checker in [('frozen',root/'audit/offline-test-handoff-2026-10-14/payload/tools/frozen/verify_types.py'),('historical-extension',root/'audit/native-bulk145-r1-codex-2026-10-20/verification-2026-10-20/tools/verify_types_with_ldtoken_ldftn_copy.py')]:
    spec=importlib.util.spec_from_file_location(label,checker);m=importlib.util.module_from_spec(spec);spec.loader.exec_module(m);results=[]
    for a in d['methods']:
        try:r=m.verify(a,d['types'])
        except Exception as e:r=dict(token=a['token'],method=a['name'],status='FAIL' if isinstance(e,m.InvalidIL) else 'VERIFIER_ERROR',error=type(e).__name__+': '+str(e))
        results.append(r)
    # Real malformed IL negative control in exported model. This is a verifier
    # sensitivity control, not a candidate behavior mutant or semantic proof.
    bad=copy.deepcopy(d['methods'][0]);bad['instructions'][0]['opcode']='pop';bad['instructions'][0]['operand']=None
    try:m.verify(bad,d['types']);nc=False
    except m.InvalidIL as e:nc='underflow' in str(e)
    report=dict(candidate_sha256=hashlib.sha256((v/(rev+'-candidate.dll')).read_bytes()).hexdigest(),checker_sha256=hashlib.sha256(checker.read_bytes()).hexdigest(),counts=dict(collections.Counter(x['status'] for x in results)),stack_underflow_model_negative_control_rejected=nc,results=results)
    (v/(rev+'-'+label+'-types.json')).write_text(json.dumps(report,indent=2)+'\n','utf-8')
    print(label,report['counts'],'negative_control',nc)
    for r in results:
        if r['status']!='PASS':print(r['token'],r.get('error'))
