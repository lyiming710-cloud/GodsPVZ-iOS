"""Apple arm64 compiler precheck of hash-bound freshly generated Startup R20 code.

Runtime headers/physical-iOS flags come from the preserved real Unity export.
The generated code profile is the portable conversion, not a new Unity export.
"""
from pathlib import Path
import argparse,concurrent.futures,json,os,subprocess
from audit_bulk389_all_generated_cpp import compile_flags,run_one,sha,controls,EXPORT_HEAD
from bulk389_export_binding import validate
p=argparse.ArgumentParser();p.add_argument('--xcode-dir',required=True);p.add_argument('--fresh-root',required=True);p.add_argument('--compiler-input',required=True);p.add_argument('--output-dir',required=True);a=p.parse_args()
root=Path(a.xcode_dir).resolve();fresh=Path(a.fresh_root).resolve();input_root=Path(a.compiler_input);out=Path(a.output_dir).resolve()
out.mkdir(parents=True,exist_ok=False);(out/'logs').mkdir()
validate(root,EXPORT_HEAD)
report=json.loads((input_root/'startup-r20/STARTUP-R20-SITES104-COMPILER-REPORT.json').read_text())
expected='d887b370c95c2966dc40d80648d85c286bb42d85004c32d095f64f004e095b37'
assert report['pass'] and report['candidate_sha256']==expected
sources=[]
for key,suffix in [('ALL-CPP-RESULTS.json','.cpp'),('ALL-C-RESULTS.json','.c')]:
    entries=json.loads((input_root/'startup-r20'/key).read_text())
    assert all(x['exit_code']==x['error_count']==0 for x in entries)
    assert {x['file'] for x in entries}=={x.name for x in fresh.glob('*'+suffix)}
    for x in entries:
        file=fresh/x['file'];assert file.stat().st_size==x['bytes'] and sha(file)==x['sha256'];sources.append(file)
assert len(sources)==1029
old_flags=compile_flags(root);controls(old_flags,root,out)
oldcpp=root/'Il2CppOutputProject/Source/il2cppOutput'
control_out=out/'damaged-r4-control';control_out.mkdir();(control_out/'logs').mkdir()
known=run_one(oldcpp/'Generics__63.cpp',old_flags,root,control_out)
assert known['exit_code'] and known['error_count']==34,'Real damaged Apple R4 control'
(out/'DAMAGED-R4-CONTROL.json').write_text(json.dumps(known,indent=2)+'\n')
# Change only the generated include directory. Runtime include directories,
# SDK, architecture, defines, language and compatibility flags stay captured.
flags=[x.replace(str(oldcpp),str(fresh)) for x in old_flags]
(out/'APPLE-CPP-FLAGS.json').write_text(json.dumps(flags,indent=2)+'\n')
cflags=flags.copy();cflags[0]=subprocess.check_output(['xcrun','--find','clang'],text=True).strip()
cflags=[x for x in cflags if x not in ('-frtti','-fexceptions','-stdlib=libc++')]
cflags[cflags.index('-std=c++11')]='-std=c11';cflags[cflags.index('-x')+1]='c'
(out/'APPLE-C-FLAGS.json').write_text(json.dumps(cflags,indent=2)+'\n')
ccontrols=[]
for name,code,success in [('valid_c_mode','#ifdef __cplusplus\n#error C_mode_required\n#endif\nint value=42;',True),('bad_c_identifier','int value=deliberately_missing;',False)]:
    source=out/(name+'.c');source.write_text(code+'\n');r=run_one(source,cflags,root,out)
    r['expectation_met']=(r['exit_code']==0) if success else (r['exit_code']!=0 and r['error_count']>0)
    assert r['expectation_met'];ccontrols.append(r)
(out/'C-COMPILER-CONTROLS.json').write_text(json.dumps(ccontrols,indent=2)+'\n')
with concurrent.futures.ThreadPoolExecutor(max_workers=min(3,os.cpu_count() or 1)) as pool:
    results=[]
    for r in pool.map(lambda x:run_one(x,flags if x.suffix=='.cpp' else cflags,root,out),sources):
        results.append(r)
        if r['exit_code'] or len(results)%100==0:print('APPLE_PROGRESS',len(results),'/',len(sources),r['file'],r['exit_code'],r['error_count'],flush=True)
outcome=dict(scope='STARTUP_R20_FRESH_PORTABLE_CODE_APPLE_ARM64_SYNTAX_PRECHECK',candidate_sha256=expected,
    source_conversion_run=40219580971,apple_audit_source_commit=os.environ['GITHUB_SHA'],
    header_export_run=39988665189,attempted=len(results),cpp=sum(x['file'].endswith('.cpp') for x in results),
    c=sum(x['file'].endswith('.c') for x in results),failed=sum(x['exit_code']!=0 for x in results),
    diagnostics=sum(x['error_count'] for x in results),unparsed_error_lines=sum(len(x['unparsed_errors']) for x in results),
    compiler_controls=6,damaged_R4_expected_diagnostics=34,actual_unity_export_for_new_candidate=False,
    linked=False,ipa=None,game_semantics='NOT_PROVEN')
(out/'ALL-APPLE-RESULTS.json').write_text(json.dumps(results,indent=2)+'\n')
(out/'APPLE-REPORT.json').write_text(json.dumps(outcome,indent=2)+'\n');print(json.dumps(outcome),flush=True)
raise SystemExit(1 if outcome['failed'] else 0)
