"""Keep original build decisions; add report capture and one exact API alias."""
from pathlib import Path
import argparse,hashlib,json
from instrument_startup_r20_build_helper import instrument as original_instrument,ORIGINAL_SHA

def instrument(data):
 observed=original_instrument(data)
 old=b'GodsPVZBuildReportCapture.Record(report);';new=b'GodsPVZBuildReportCaptureR28.Record(report);'
 assert observed.count(old)==1 and observed.count(b'EditorUserBuildSettings.symlinkLibraries')==1
 result=observed.replace(old,new).replace(b'EditorUserBuildSettings.symlinkLibraries',b'EditorUserBuildSettings.symlinkSources')
 restored=result.replace(b'EditorUserBuildSettings.symlinkSources',b'EditorUserBuildSettings.symlinkLibraries').replace(new,old)
 assert restored==observed and b'if (report.summary.result != BuildResult.Succeeded || report.summary.totalErrors != 0)'in result
 return result

def install(source,project,evidence):
 source=Path(source);project=Path(project);evidence=Path(evidence)
 original=source.read_bytes();new=instrument(original);build=project/'Assets/Editor/GodsPVZIOSBuild.cs'
 assert hashlib.sha256(original).hexdigest()==ORIGINAL_SHA and build.read_bytes()==original
 capture=Path(__file__).with_name('GodsPVZBuildReportCaptureR28.cs')
 build.write_bytes(new);(build.parent/capture.name).write_bytes(capture.read_bytes())
 evidence.mkdir(parents=True,exist_ok=True)
 result=dict(scope='REPORT_OBSERVER_AND_EXACT_SYMLINK_API_RENAME_ORIGINAL_BUILD_DECISIONS_PRESERVED',
  original_sha256=ORIGINAL_SHA,instrumented_sha256=hashlib.sha256(new).hexdigest(),capture_sha256=hashlib.sha256(capture.read_bytes()).hexdigest(),
  original_error_and_result_decisions_preserved=True,api_alias_replacements=1,errors_ignored=False,game_candidate_changed=False)
 (evidence/'build-report-instrumentation.json').write_text(json.dumps(result,indent=2)+'\n',encoding='utf-8');return result

if __name__=='__main__':
 p=argparse.ArgumentParser();p.add_argument('--source',required=True);p.add_argument('--project',required=True);p.add_argument('--evidence',required=True);a=p.parse_args();print(json.dumps(install(a.source,a.project,a.evidence)))
