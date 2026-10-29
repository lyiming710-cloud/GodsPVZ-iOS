"""Check ALL generated C/C++ in the actual new Unity export before building."""
from pathlib import Path
import argparse,concurrent.futures,json,os,subprocess,hashlib
from audit_bulk389_all_generated_cpp import run_one,controls
from bulk401_apple_flags import compile_flags
from startup_r28_export_binding import validate
p=argparse.ArgumentParser();p.add_argument('--xcode-dir',required=True);p.add_argument('--output-dir',required=True)
p.add_argument('--export-workflow-head',default=os.environ.get('GITHUB_SHA'));a=p.parse_args()
if not a.export_workflow_head:p.error('An explicit export workflow head or GITHUB_SHA is required')
root=Path(a.xcode_dir).resolve();out=Path(a.output_dir).resolve();out.mkdir(parents=True,exist_ok=False);(out/'logs').mkdir()
binding=validate(root,a.export_workflow_head);folder=root/'Il2CppOutputProject/Source/il2cppOutput'
sources=sorted(x for x in folder.iterdir() if x.is_file() and x.suffix in ('.cpp','.c'))
assert any(x.name.startswith('Generics') for x in sources) and any(x.name.startswith('GenericMethods') for x in sources)
assert len(sources)>300
(out/'SOURCE-INVENTORY.json').write_text(json.dumps([dict(file=x.name,bytes=x.stat().st_size,sha256=hashlib.sha256(x.read_bytes()).hexdigest()) for x in sources],indent=2)+'\n')
flags=compile_flags(root);controls(flags,root,out)
cflags=flags.copy();cflags[0]=subprocess.check_output(['xcrun','--find','clang'],text=True).strip()
cflags=[x for x in cflags if x not in ('-frtti','-fexceptions','-stdlib=libc++')]
cflags[cflags.index('-std=c++11')]='-std=c11';cflags[cflags.index('-x')+1]='c'
(out/'CPP-FLAGS.json').write_text(json.dumps(flags,indent=2)+'\n');(out/'C-FLAGS.json').write_text(json.dumps(cflags,indent=2)+'\n')
ccontrols=[]
for name,code,success in [('valid_c_mode','#ifdef __cplusplus\n#error C_mode_required\n#endif\nint value=42;',True),('bad_c_identifier','int value=deliberately_missing;',False)]:
    source=out/(name+'.c');source.write_text(code+'\n');r=run_one(source,cflags,root,out)
    r['expectation_met']=(r['exit_code']==0) if success else (r['exit_code']!=0 and r['error_count']>0)
    if not r['expectation_met']:raise ValueError('C compiler control failed')
    ccontrols.append(r)
(out/'C-COMPILER-CONTROLS.json').write_text(json.dumps(ccontrols,indent=2)+'\n')
with concurrent.futures.ThreadPoolExecutor(max_workers=min(3,os.cpu_count() or 1)) as pool:
    results=list(pool.map(lambda x:run_one(x,flags if x.suffix=='.cpp' else cflags,root,out),sources))
report=dict(scope='ACTUAL_NEW_UNITY_EXPORT_ALL_GENERATED_APPLE_ARM64_SYNTAX_ONLY',candidate_sha256=binding['candidate_sha256'],
    workflow_head=binding['workflow_head'],audit_workflow_head=os.environ.get('GITHUB_SHA'),attempted=len(results),
    cpp=sum(x['file'].endswith('.cpp') for x in results),c=sum(x['file'].endswith('.c') for x in results),
    failed=sum(x['exit_code']!=0 for x in results),compiler_controls=6,
    diagnostics=sum(x['error_count'] for x in results),unparsed=sum(len(x['unparsed_errors']) for x in results),linked=False,ipa=None)
(out/'ALL-RESULTS.json').write_text(json.dumps(results,indent=2)+'\n');(out/'REPORT.json').write_text(json.dumps(report,indent=2)+'\n')
print(json.dumps(report),flush=True);raise SystemExit(report['failed']!=0)
