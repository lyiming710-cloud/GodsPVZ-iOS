"""Build one disposable support copy with the exact actual-Editor SDK image.

The authenticated portable checkpoint is preserved. All other support files
must remain byte-identical. DLL/EXE source naming is preserved for the original
converter argv; the SDK assembly's bytes and identity are not rewritten.
"""
from pathlib import Path
import hashlib,json,shutil,zipfile
SDK='51f72bcd146c5571f642f3c4149f0545eb704fd325d83eb0f7c4ca2deec14c2b'
ARCHIVE=Path(__file__).resolve().parents[2]/'Recovery/Startup-R26-SDK-Helpers-2026-10-28/RECOVERED-VISUALSCRIPTING-CORE.zip'
sha=lambda b:hashlib.sha256(b).hexdigest()

def prepare(source,destination,receipt,archive=ARCHIVE):
 source=Path(source).resolve();destination=Path(destination).resolve();receipt=Path(receipt)
 assert source.is_dir()and not destination.exists()and not receipt.exists()
 assert not destination.is_relative_to(source)and not source.is_relative_to(destination)
 targets=[p for p in source.iterdir()if p.stem=='Unity.VisualScripting.Core'and p.suffix.lower()in ['.dll','.exe']]
 assert len(targets)==1,'Require one original SDK assembly name; do not admit duplicates or an invented missing assembly'
 before={p.name:dict(bytes=p.stat().st_size,sha256=sha(p.read_bytes()))for p in source.iterdir()if p.is_file()}
 with zipfile.ZipFile(archive)as z:
  assert z.testzip()is None and set(z.namelist())=={'Unity.VisualScripting.Core.dll','MANIFEST.json'}
  data=z.read('Unity.VisualScripting.Core.dll');assert sha(data)==SDK
  assert json.loads(z.read('MANIFEST.json'))==[dict(file='Unity.VisualScripting.Core.dll',bytes=len(data),sha256=SDK)]
 shutil.copytree(source,destination,symlinks=False)
 target=destination/targets[0].name
 assert not targets[0].is_symlink()and target.resolve().is_relative_to(destination)
 target.write_bytes(data)
 after={p.name:dict(bytes=p.stat().st_size,sha256=sha(p.read_bytes()))for p in destination.iterdir()if p.is_file()}
 assert set(before)==set(after)
 assert all(after[name]==row for name,row in before.items()if name!=target.name)
 assert all(sha((source/name).read_bytes())==row['sha256']for name,row in before.items())
 assert after[target.name]==dict(bytes=len(data),sha256=SDK)
 result=dict(scope='DISPOSABLE_COMPILER_SUPPORT_COPY_ONLY_ORIGINAL_PORTABLE_UNCHANGED',source_directory=str(source),destination_directory=str(destination),sdk_filename=target.name,original_sdk=before[target.name],actual_staged_sdk=after[target.name],other_files_byte_identical=len(before)-1,files=after,unity_or_gameplay_proof=False)
 receipt.write_text(json.dumps(result,indent=2)+'\n',encoding='utf-8')
 return destination,result
