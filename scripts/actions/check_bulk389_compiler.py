"""Fixed-candidate compiler diagnostics only; no behavioral or IPA qualification."""
import importlib.util,json,pathlib,sys,hashlib
here=pathlib.Path(__file__).resolve().parent
spec=importlib.util.spec_from_file_location('portable',here/'verify_bulk152_portable.py')
p=importlib.util.module_from_spec(spec);spec.loader.exec_module(p)
args=p.parse_args();out=pathlib.Path(args.output_dir);candidate=pathlib.Path(args.candidate_dll)
assert hashlib.sha256(candidate.read_bytes()).hexdigest()=='ef1992326d1ee494a389bc2c78bdfab131721434c0b08ab5f41dbc3e506c4e48'
cfg=p.PathConfig(pathlib.Path(args.portable_root).resolve(),out.resolve(),args)
report=dict(scope='BULK389_R4_LINUX_IL2CPP_AND_CPP_COMPILER_DIAGNOSTICS_ONLY',candidate_sha256=hashlib.sha256(candidate.read_bytes()).hexdigest(),behavior='NOT_RUN_NOT_QUALIFIED',unity='NOT_RUN',xcode='NOT_RUN',ipa=None)
try:
    report['toolchain']=p.record_toolchain_identities(cfg)
    cpp,conversion=p.execute_phase3_il2cpp(cfg);report['il2cpp']=conversion
    clang=p.execute_phase4_clang(cfg,cpp);report['clang']=clang
    report['game_tu_compiler_pass']=clang['total_game_failed']==0
except BaseException as e:
    report['execution_failure']=type(e).__name__+': '+str(e)
    (cfg.output_dir/'BULK389-COMPILER-REPORT.json').write_text(json.dumps(report,indent=2,default=str)+'\n')
    raise
(cfg.output_dir/'BULK389-COMPILER-REPORT.json').write_text(json.dumps(report,indent=2,default=str)+'\n')
raise SystemExit(0 if report['game_tu_compiler_pass'] else 1)
