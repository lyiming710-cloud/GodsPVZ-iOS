"""Fresh fixed-candidate conversion and ALL generated-source syntax census.

Linux compiler evidence only. No Unity activation, import, Xcode or IPA build.
The immutable R4 is regenerated too, as a real damaged-input negative control.
"""
from pathlib import Path
import importlib.util,json,sys,hashlib,re,subprocess,concurrent.futures,time,os
here=Path(__file__).resolve().parent
s=importlib.util.spec_from_file_location('portable',here/'verify_bulk152_portable.py')
p=importlib.util.module_from_spec(s);s.loader.exec_module(p)
args=p.parse_args();root=Path(args.output_dir).resolve();candidate=Path(args.candidate_dll).resolve()
expected='d2a645ad0d8164af5fbdcbd9fcaa578d5a28b27e7b13f3012cdb51e63a256091'
assert p.sha256_file(candidate)==expected,'Fixed Startup R17 Batch50 input'
root.mkdir(parents=True,exist_ok=False)
report=dict(scope='STARTUP_R17_FRESH_LINUX_ALL_GENERATED_SOURCE_COMPILER_ONLY',candidate_sha256=expected,
            actual_game_behavior='NOT_PROVEN',unity='NOT_RUN',xcode='NOT_RUN',ipa=None)
def config(out,dll):
    import copy
    a=copy.copy(args);a.output_dir=str(out);a.candidate_dll=str(dll)
    return p.PathConfig(Path(args.portable_root),out,a)
def flags(cfg,cpp):
    return ['clang++','-std=c++11','-fsyntax-only','-ferror-limit=0',
      '-Wno-tautological-compare','-Wno-unused-value','-Wno-invalid-noreturn',
      '-DRUNTIME_IL2CPP','-DIL2CPP_MONO_DEBUGGER_DISABLED','-DIL2CPP_DEBUG=0',
      '-DNDEBUG','-DBASELIB_INLINE_NAMESPACE=il2cpp_baselib',f'-I{cpp}',
      f'-I{cfg.native18_il2cpp}/libil2cpp/pch',f'-I{cfg.native18_il2cpp}/libil2cpp',
      f'-I{cfg.native18_il2cpp}/external/baselib/Include',
      f'-I{cfg.native18_il2cpp}/external/baselib/Platforms/Linux/Include']
def compile_one(src,argv,logs):
    start=time.monotonic();log=logs/(src.name+'.log')
    with log.open('w') as f:r=subprocess.run(argv+[str(src)],stdout=f,stderr=subprocess.STDOUT)
    text=log.read_text(errors='replace')
    return dict(file=src.name,bytes=src.stat().st_size,sha256=p.sha256_file(src),exit_code=r.returncode,
      error_count=sum(bool(re.search(r': (?:fatal )?error:',l)) for l in text.splitlines()),
      warning_count=text.count(': warning:'),seconds=round(time.monotonic()-start,3))
try:
    cfg=config(root/'candidate',candidate);report['tools']=p.record_toolchain_identities(cfg)
    clang=flags(cfg,cfg.output_dir/'unused')[:4]
    control_dir=root/'controls';control_dir.mkdir();controls=[]
    for name,src,want in [
      ('valid_pointer','int f(void* p){return p==nullptr;} int g(int i){return i==0;}',0),
      ('bad_pointer_int','int f(void* p){return p==(int)0;}',1),
      ('bad_int_null','int f(int i){return i==nullptr;}',1),
      ('bad_object_index','void* f(void** a,void* i){return a[i];}',1)]:
        file=control_dir/(name+'.cpp');file.write_text(src+'\n');result=compile_one(file,clang,control_dir)
        assert (result['exit_code']!=0)==bool(want),(name,result)
        controls.append(dict(name=name,expected_nonzero=bool(want),**result))
    report['compiler_controls']=controls
    for name,src,want in [
      ('valid_c_mode','#ifdef __cplusplus\n#error C mode required\n#endif\nint value=42;',0),
      ('bad_c_identifier','int value=missing_identifier;',1)]:
        file=control_dir/(name+'.c');file.write_text(src+'\n')
        result=compile_one(file,['clang','-std=c11','-fsyntax-only','-ferror-limit=0'],control_dir)
        assert (result['exit_code']!=0)==bool(want),(name,result)
        controls.append(dict(name=name,expected_nonzero=bool(want),**result))
    # Reproduce the real generic failure on the same converter and headers.
    old=here.parents[1]/'Recovery/Bulk389-Qualification-2026-10-23/BULK389-R4-CANDIDATE.zip'
    import zipfile
    with zipfile.ZipFile(old) as z:b=z.read('BULK389-R4-CANDIDATE.dll')
    assert hashlib.sha256(b).hexdigest()=='ef1992326d1ee494a389bc2c78bdfab131721434c0b08ab5f41dbc3e506c4e48'
    (root/'R4-control.dll').write_bytes(b);oldcfg=config(root/'r4-control',root/'R4-control.dll')
    oldcpp,oldconvert=p.execute_phase3_il2cpp(oldcfg)
    # A definition is required: declarations alone do not select a TU.
    pattern=re.compile(r'IL2CPP_METHOD_ATTR[^\n]*(?:SwfList_1_Pop_|SwfAssocList_1_Add_|Map_RandomGet_Grid_TestPlace_)[^\n]*\n\{')
    oldunits=[x for x in sorted(oldcpp.glob('*.cpp')) if pattern.search(x.read_text(errors='replace'))]
    assert oldunits,'Damaged generic definitions must be present'
    oldresults=[compile_one(x,flags(oldcfg,oldcpp),oldcfg.clang_logs_dir) for x in oldunits]
    assert any(x['exit_code'] and x['error_count'] for x in oldresults),'Damaged R4 was not detected'
    report['damaged_r4_control']=dict(conversion=oldconvert,units=oldresults)
    cpp,conversion=p.execute_phase3_il2cpp(cfg);report['conversion']=conversion
    argv=flags(cfg,cpp);(root/'CPP-FLAGS.json').write_text(json.dumps(argv,indent=2)+'\n')
    sources=sorted(cpp.glob('*.cpp'));assert sources and len(sources)==conversion['total_cpp_count']
    results=[];start=time.monotonic()
    with concurrent.futures.ThreadPoolExecutor(max_workers=min(4,os.cpu_count() or 1)) as pool:
        for result in pool.map(lambda x:compile_one(x,argv,cfg.clang_logs_dir),sources):
            results.append(result)
            if result['exit_code']:print('FAILED',result,flush=True)
    report['cpp']=dict(attempted=len(results),passed=sum(x['exit_code']==0 for x in results),
      failed=sum(x['exit_code']!=0 for x in results),diagnostics=sum(x['error_count'] for x in results),
      generics_named=sum(x['file'].startswith('Generics') for x in results),
      generic_methods_named=sum(x['file'].startswith('GenericMethods') for x in results),
      seconds=round(time.monotonic()-start,3))
    (root/'ALL-CPP-RESULTS.json').write_text(json.dumps(results,indent=2)+'\n')
    cargv=['clang','-std=c11']+argv[2:]
    cresults=[compile_one(x,cargv,cfg.clang_logs_dir) for x in sorted(cpp.glob('*.c'))]
    report['c']=dict(attempted=len(cresults),failed=sum(x['exit_code']!=0 for x in cresults),diagnostics=sum(x['error_count'] for x in cresults))
    (root/'ALL-C-RESULTS.json').write_text(json.dumps(cresults,indent=2)+'\n')
    assert p.sha256_file(candidate)==expected,'Candidate changed during check'
    report['pass']=report['cpp']['failed']==report['c']['failed']==0
except BaseException as e:
    report['execution_failure']=type(e).__name__+': '+str(e)
    raise
finally:
    (root/'STARTUP-R17-BATCH50-COMPILER-REPORT.json').write_text(json.dumps(report,indent=2,default=str)+'\n')
print(json.dumps(report,default=str),flush=True)
raise SystemExit(0 if report.get('pass') else 1)
