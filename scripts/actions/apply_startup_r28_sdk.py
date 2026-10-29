"""Replace only the two-method recovered SDK in a disposable Unity work copy."""
from pathlib import Path
import argparse,hashlib,json,zipfile
OLD='dc1b51ccb332d73e52642f6a50fe0694661e51b8eb2ff7ca25381f1ab877c8f9';NEW='51f72bcd146c5571f642f3c4149f0545eb704fd325d83eb0f7c4ca2deec14c2b';sha=lambda b:hashlib.sha256(b).hexdigest()
if __name__=='__main__':
 a=argparse.ArgumentParser();a.add_argument('--project',required=True);a.add_argument('--evidence',required=True);x=a.parse_args();project=Path(x.project).resolve();evidence=Path(x.evidence).resolve();assert(project/'ProjectSettings/ProjectVersion.txt').is_file();found=list((project/'Assets').rglob('Unity.VisualScripting.Core.dll'));assert len(found)==1,found;p=found[0];assert p.resolve().is_relative_to(project)and not p.is_symlink()and sha(p.read_bytes())==OLD;meta=p.with_suffix('.dll.meta');before=meta.read_bytes();archive=Path(__file__).resolve().parents[2]/'Recovery/Startup-R26-SDK-Helpers-2026-10-28/RECOVERED-VISUALSCRIPTING-CORE.zip'
 with zipfile.ZipFile(archive)as z:assert z.testzip()is None;b=z.read('Unity.VisualScripting.Core.dll')
 assert sha(b)==NEW;p.write_bytes(b);assert sha(p.read_bytes())==NEW and meta.read_bytes()==before;evidence.mkdir(parents=True,exist_ok=True);report=dict(scope='DISPOSABLE_WORKCOPY_TWO_METHOD_SDK_RECOVERY',path=p.relative_to(project).as_posix(),before_sha256=OLD,after_sha256=NEW,meta_sha256=sha(before),meta_guid_preserved=True,game_dll_changed=False,unity_started=False);(evidence/'R28-SDK-APPLICATION.json').write_text(json.dumps(report,indent=2)+'\n','utf-8');print(json.dumps(report))
