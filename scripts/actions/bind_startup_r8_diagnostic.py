"""Bind exact R8 to a restricted Editor diagnostic, preserving all failed gates."""
from pathlib import Path
import argparse,hashlib,json,zipfile
PARENT='6de4db6c94b80a43876aea060b573ed60c1fa7170cff485103d2c808b94762ae'
CANDIDATE='6189387c6af11108eba9a9855bfd6d6af567ec000a4999d1ed1b0328ba0e1d08'
CHECKER='2bb0a8c229a63fd33269433759a587ff551c6fde19669bafc506033df9b63d14'
CHECKPOINT=Path(__file__).resolve().parents[2]/'Recovery/Startup-R8-2026-10-25'
def sha(b):return hashlib.sha256(b).hexdigest()
def validate_checkpoint():
    rows=json.loads((CHECKPOINT/'MANIFEST.json').read_bytes())
    actual={p.relative_to(CHECKPOINT).as_posix() for p in CHECKPOINT.rglob('*') if p.is_file() and p!=CHECKPOINT/'MANIFEST.json'}
    if actual!={r['file'] for r in rows}:raise ValueError('Checkpoint inventory differs')
    for row in rows:
        p=CHECKPOINT/row['file']
        if not p.resolve().is_relative_to(CHECKPOINT.resolve()) or p.is_symlink():raise ValueError('Unsafe checkpoint member')
        b=p.read_bytes()
        if len(b)!=row['bytes'] or sha(b)!=row['sha256']:raise ValueError('Checkpoint bytes differ: '+row['file'])
    def report(name):
        value=json.loads((CHECKPOINT/name).read_bytes())
        if value['candidate_sha256']!=CANDIDATE:raise ValueError('Wrong child report '+name)
        return value
    isolation=report('ISOLATION.json');form=report('INDEPENDENT-CIL-EH-FORM.json');matrix=report('EXACT-JIT-SIX.json');runtime=report('RUNTIME-SUMMARY.json');current=report('CURRENT-RESULT.json')
    if not isolation['pass_'] or isolation['direct_method_targets']!=6 or isolation['untouched_methoddefs']!=2311 or not isolation['metadata_rvas_and_other_bytes_identical']:raise ValueError('Missing raw isolation')
    if not form['pass'] or form['methods']!=6 or matrix['parent_rejected']!=6 or matrix['candidate_prepared']!=6:raise ValueError('Missing actual six-method form/JIT evidence')
    if runtime['cases']!=2475 or runtime['methods']!=6 or len(runtime['suites'])!=5:raise ValueError('Missing scoped runtime corpus')
    for suite in runtime['suites']:
        r=suite['result']
        if r['candidate_sha256']!=CANDIDATE or not r['pass_']:raise ValueError('Scoped suite failure or wrong binding')
    width=json.loads((CHECKPOINT/'INT64-NEGATIVE-WIDTH.json').read_bytes())
    if width['checker_sha256']!=CHECKER or not width['baseline_passed'] or not width['rejected'] or not width['runtime_did_not_distinguish']:raise ValueError('Width counterevidence missing')
    if current['whole_assembly']['all_pass'] is not False or current['ipa'] is not None:raise ValueError('Restricted diagnostic must retain full-pipeline hold')
    with zipfile.ZipFile(CHECKPOINT/'STARTUP-R8-CANDIDATE.zip') as z:
        if set(z.namelist())!={'STARTUP-R8.dll','MANIFEST.json'} or z.testzip() is not None:raise ValueError('Unexpected R8 archive')
        data=z.read('STARTUP-R8.dll');inside=json.loads(z.read('MANIFEST.json'))
        if inside!=[dict(file='STARTUP-R8.dll',bytes=len(data),sha256=sha(data))]:raise ValueError('Inner byte manifest differs')
    if len(data)!=18863104 or sha(data)!=CANDIDATE:raise ValueError('R8 DLL bytes differ')
    return data,current
def bind(output,source):
    out=Path(output).resolve();evidence=out/'evidence';target=out/'candidate/Assembly-CSharp-bulk401.dll';provenance=evidence/'PROVENANCE.json'
    if sha(target.read_bytes())!=PARENT:raise ValueError('Exact separately qualified parent input required')
    parent=json.loads(provenance.read_bytes())
    if parent['outputs']['unlinked']['sha256']!=PARENT or parent['source_commit']!=source or parent['status']!='BULK401_LINUX_AND_APPLE_CPP_PRECHECK_PASS':raise ValueError('Same-run parent input binding failed')
    backup=evidence/'PARENT-BULK401-PROVENANCE.json'
    if backup.exists():raise ValueError('Do not overwrite prior binding')
    data,current=validate_checkpoint()
    # All validation completes before the disposable output is changed.
    provenance.rename(backup);target.write_bytes(data)
    report=dict(status='STARTUP_R8_RESTRICTED_EDITOR_DIAGNOSTIC_ONLY',source_commit=source,parent_sha256=PARENT,candidate_sha256=CANDIDATE,
        outputs={'unlinked':{'sha256':CANDIDATE}},scoped_form_methods=6,scoped_caller_runtime_cases=2475,
        failed_whole_assembly_gates_preserved=current['whole_assembly'],parent_compiler_results_apply_to_child=False,
        child_unity_export_performed=False,child_cpp_compilation_performed=False,child_device_execution_performed=False,
        ipa_promotion_allowed=False,diagnostic_scope='Actual Editor MainMenu startup, original asset references, Metal compilation and controlled shader pixels only')
    provenance.write_text(json.dumps(report,indent=2)+'\n',encoding='utf-8')
    inspection=evidence/'candidate-inspection.txt'
    if inspection.exists():inspection.rename(evidence/'PARENT-BULK401-candidate-inspection.txt')
    inspection.write_text(json.dumps(report,indent=2)+'\n',encoding='utf-8')
    for name in ['ISOLATION.json','INDEPENDENT-CIL-EH-FORM.json','EXACT-JIT-SIX.json','RUNTIME-SUMMARY.json','NEXT-JIT-QUEUE.json']:(evidence/name).write_bytes((CHECKPOINT/name).read_bytes())
    return report
if __name__=='__main__':
    p=argparse.ArgumentParser();p.add_argument('--output',required=True);p.add_argument('--source',required=True);a=p.parse_args();print(json.dumps(bind(a.output,a.source)))
