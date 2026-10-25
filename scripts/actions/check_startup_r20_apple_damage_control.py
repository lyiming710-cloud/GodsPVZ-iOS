"""Replay one real damaged Apple unit, preserving raw diagnostics separately."""
from pathlib import Path
import argparse,json,os,re
from audit_bulk389_all_generated_cpp import compile_flags,run_one,sha,EXPORT_HEAD
from bulk389_export_binding import validate
p=argparse.ArgumentParser();p.add_argument('--xcode-dir',required=True);p.add_argument('--output-dir',required=True);a=p.parse_args()
root=Path(a.xcode_dir).resolve();out=Path(a.output_dir).resolve();out.mkdir(parents=True,exist_ok=False);(out/'logs').mkdir()
validate(root,EXPORT_HEAD);source=root/'Il2CppOutputProject/Source/il2cppOutput/Generics__63.cpp'
assert sha(source)=='11427c29119a74621ebe7f94a0e043de4bfe7a0cd00f7ac0b2c2de5928ba0017'
flags=compile_flags(root)
assert '/Xcode_16.4.app/' in flags[0] and flags[flags.index('-arch')+1]=='arm64'
assert flags[flags.index('-isysroot')+1].endswith('/iPhoneOS18.5.sdk') and '-fsyntax-only' in flags
(out/'FLAGS.json').write_text(json.dumps(flags,indent=2)+'\n')
row=run_one(source,flags,root,out)
log=out/'logs/Generics__63.cpp.log';text=log.read_text('utf-8')
assert row['exit_code']!=0 and row['error_count']==34 and not row['unparsed_errors']
assert sum(bool(re.search(r': (?:fatal )?error:',line)) for line in text.splitlines())==34
assert '/Source/il2cppOutput/Generics__63.cpp' in text and '/fresh-generated/' not in text
(out/'DAMAGED-SOURCE.cpp').write_bytes(source.read_bytes())
result=dict(scope='RAW_APPLE_DAMAGED_CONTROL_REPLAY_ONLY_NOT_GAME_COMPILATION',
    source_commit=os.environ['GITHUB_SHA'],header_export_run=39988665189,control=row,
    raw_log=dict(file=log.relative_to(out).as_posix(),bytes=log.stat().st_size,sha256=sha(log)),
    expectation_met=True,raw_control_log_preserved=True,game_dll_changed=False,
    full_source_compilation=False,unity_export=False,linked=False,ipa=None)
(out/'RESULT.json').write_text(json.dumps(result,indent=2)+'\n');print(json.dumps(result))
