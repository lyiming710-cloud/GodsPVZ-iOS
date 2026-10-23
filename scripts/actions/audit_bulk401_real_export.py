"""Check ALL generated C/C++ in the actual new Unity export before building."""
from pathlib import Path
import argparse,concurrent.futures,json,os,subprocess
from audit_bulk389_all_generated_cpp import compile_flags,run_one,controls
from bulk401_export_binding import validate
p=argparse.ArgumentParser();p.add_argument('--xcode-dir',required=True);p.add_argument('--output-dir',required=True);a=p.parse_args()
root=Path(a.xcode_dir).resolve();out=Path(a.output_dir).resolve();out.mkdir(parents=True,exist_ok=False);(out/'logs').mkdir()
binding=validate(root,os.environ['GITHUB_SHA']);folder=root/'Il2CppOutputProject/Source/il2cppOutput'
sources=sorted(x for x in folder.iterdir() if x.is_file() and x.suffix in ('.cpp','.c'))
assert any(x.name.startswith('Generics') for x in sources) and any(x.name.startswith('GenericMethods') for x in sources)
assert len(sources)>300
flags=compile_flags(root);controls(flags,root,out)
cflags=flags.copy();cflags[0]=subprocess.check_output(['xcrun','--find','clang'],text=True).strip()
cflags=[x for x in cflags if x not in ('-frtti','-fexceptions','-stdlib=libc++')]
cflags[cflags.index('-std=c++11')]='-std=c11';cflags[cflags.index('-x')+1]='c'
(out/'CPP-FLAGS.json').write_text(json.dumps(flags,indent=2)+'\n');(out/'C-FLAGS.json').write_text(json.dumps(cflags,indent=2)+'\n')
with concurrent.futures.ThreadPoolExecutor(max_workers=min(3,os.cpu_count() or 1)) as pool:
    results=list(pool.map(lambda x:run_one(x,flags if x.suffix=='.cpp' else cflags,root,out),sources))
report=dict(scope='ACTUAL_NEW_UNITY_EXPORT_ALL_GENERATED_APPLE_ARM64_SYNTAX_ONLY',candidate_sha256=binding['candidate_sha256'],
    workflow_head=os.environ['GITHUB_SHA'],attempted=len(results),failed=sum(x['exit_code']!=0 for x in results),
    diagnostics=sum(x['error_count'] for x in results),unparsed=sum(len(x['unparsed_errors']) for x in results),linked=False,ipa=None)
(out/'ALL-RESULTS.json').write_text(json.dumps(results,indent=2)+'\n');(out/'REPORT.json').write_text(json.dumps(report,indent=2)+'\n')
print(json.dumps(report),flush=True);raise SystemExit(report['failed']!=0)
