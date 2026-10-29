"""Retain failed/unqualified exports for analysis; never manufacture an export PASS."""
from pathlib import Path
import argparse,hashlib,json,os,zipfile

EXPECTED='9081408a2226a79e23991355108540a0260a3966a500cecf4524f3cba9dfb364'
def digest(path):
    h=hashlib.sha256()
    with Path(path).open('rb') as f:
        for b in iter(lambda:f.read(1024*1024),b''):h.update(b)
    return h.hexdigest()

def capture(out,project):
    out=Path(out);project=Path(project);e=out/'evidence';e.mkdir(parents=True,exist_ok=True);x=out/'xcode';dest=out/'unqualified-export-forensics';dest.mkdir(exist_ok=True)
    plugin=project/'Assets/Plugins/GodsPVZRuntime1.dll';exitfile=e/'ios-export-exit.txt';buildreport=e/'actual-unity-build-report.json';report=json.loads(buildreport.read_bytes()) if buildreport.exists() else None
    info=dict(scope='UNQUALIFIED_EXPORT_FORENSICS_ONLY_NOT_BUILD_ACCEPTANCE',run_id=os.environ.get('GITHUB_RUN_ID'),source_commit=os.environ.get('GITHUB_SHA'),expected_candidate_sha256=EXPECTED,observed_plugin_sha256=digest(plugin) if plugin.exists() else None,unity_exit=exitfile.read_text().strip() if exitfile.exists() else None,actual_report_found=report is not None,errors_ignored=False,qualified=False,ipa=False)
    if report:
        messages=[dict(step=s['name'],**m) for s in (report.get('steps') or []) for m in (s.get('messages') or []) if m['type'] in ['Error','Exception','Assert']]
        info.update(build_result=report['result'],summary_errors=report['total_errors'],report_error_messages=messages,observed_error_logs=report.get('logs',[]),helper_exception=report.get('exception'))
    files=[]
    generated=x/'Il2CppOutputProject/Source/il2cppOutput'
    if generated.is_dir():files.extend((p,'xcode/'+p.relative_to(x).as_posix()) for p in sorted(generated.rglob('*')) if p.is_file())
    metadata=x/'Data/Managed/Metadata'
    if metadata.is_dir():files.extend((p,'xcode/'+p.relative_to(x).as_posix()) for p in sorted(metadata.rglob('*')) if p.is_file())
    for p in [x/'Unity-iPhone.xcodeproj/project.pbxproj',buildreport,e/'LastBuild.buildreport',e/'build-report-instrumentation.json']:
        if p.exists():files.append((p,'xcode/'+p.relative_to(x).as_posix() if p.is_relative_to(x) else 'evidence/'+p.name))
    # Keep the actually linked/stripped runtime for diagnosis, without treating it as the input DLL.
    bee=project/'Library/Bee/artifacts/iOS'
    if bee.is_dir():files.extend((p,'project/'+p.relative_to(project).as_posix()) for p in sorted(bee.rglob('GodsPVZRuntime1.dll')) if p.is_file())
    names=[n for p,n in files];assert len(names)==len(set(names))
    inventory=[dict(file=n,bytes=p.stat().st_size,sha256=digest(p)) for p,n in files];info.update(captured_files=len(inventory),actual_generated_cpp=sum(p.suffix=='.cpp' and p.parent==generated for p,n in files),actual_generated_c=sum(p.suffix=='.c' and p.parent==generated for p,n in files),files=inventory)
    raw=(json.dumps(info,indent=2)+'\n').encode();(dest/'FORENSICS.json').write_bytes(raw);(e/'export-forensics-summary.json').write_bytes(raw)
    archive=dest/'UNQUALIFIED-GENERATED-SOURCE-AND-BUILD-REPORT.zip'
    with zipfile.ZipFile(archive,'w',zipfile.ZIP_DEFLATED,compresslevel=6) as z:
        for p,n in files:z.write(p,n)
        z.writestr('MANIFEST.json',json.dumps(inventory,indent=2))
    with zipfile.ZipFile(archive) as z:
        assert z.testzip() is None and set(z.namelist())=={'MANIFEST.json'}|set(names)
        for row in inventory:
            b=z.read(row['file']);assert len(b)==row['bytes'] and hashlib.sha256(b).hexdigest()==row['sha256']
    receipt=dict(file=archive.name,bytes=archive.stat().st_size,sha256=digest(archive),qualified=False)
    (dest/'ARCHIVE.json').write_text(json.dumps(receipt,indent=2)+'\n','utf-8');print(json.dumps({k:v for k,v in info.items() if k not in ['files','report_error_messages','observed_error_logs']}));return info

if __name__=='__main__':
    p=argparse.ArgumentParser();p.add_argument('--out',required=True);p.add_argument('--project',required=True);a=p.parse_args();capture(a.out,a.project)
