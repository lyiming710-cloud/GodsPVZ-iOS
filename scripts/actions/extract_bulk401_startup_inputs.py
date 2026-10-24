"""Extract source/settings text from the exact R3 archive; never run its code."""
from pathlib import Path, PurePosixPath
import argparse,hashlib,json,subprocess,tarfile

p=argparse.ArgumentParser();p.add_argument('--input-dir',required=True);p.add_argument('--output-dir',required=True);a=p.parse_args()
out=Path(a.output_dir);out.mkdir(parents=True,exist_ok=False)
archives=list(Path(a.input_dir).glob('*.tar.zst'));assert len(archives)==1
archive=archives[0]
h=hashlib.sha256()
with archive.open('rb') as f:
    for block in iter(lambda:f.read(1024*1024),b''):h.update(block)
assert h.hexdigest()=='d4264f12a00e86b6149d20ecf04caa9761af510aece6107bcceb155ea4ab0bbc'
allowed={'.unity','.prefab','.asset','.mat','.meta','.shader','.json','.asmdef','.cs','.txt','.hlsl'}
inventory=[];selected=[];total=0
proc=subprocess.Popen(['zstd','-dc',str(archive)],stdout=subprocess.PIPE)
try:
    with tarfile.open(fileobj=proc.stdout,mode='r|') as tar:
        for member in tar:
            if not member.isfile():continue
            parts=PurePosixPath(member.name).parts
            assert not member.name.startswith('/') and '..' not in parts
            inventory.append(dict(path=member.name,bytes=member.size))
            if 'ExportedProject' not in parts:continue
            start=parts.index('ExportedProject')+1
            relative=PurePosixPath(*parts[start:])
            if not relative.parts or relative.parts[0] not in ('Assets','ProjectSettings','Packages'):continue
            if relative.suffix.lower() not in allowed:continue
            if member.size>32*1024*1024:continue
            data=tar.extractfile(member).read()
            if b'\x00' in data[:512]:continue
            target=out/'project-text'/Path(*relative.parts)
            assert target.resolve().is_relative_to(out.resolve())
            target.parent.mkdir(parents=True,exist_ok=True);target.write_bytes(data)
            total+=len(data);assert total<500*1024*1024
            selected.append(dict(path=relative.as_posix(),bytes=len(data),sha256=hashlib.sha256(data).hexdigest()))
    assert proc.wait(timeout=60)==0
finally:
    if proc.poll() is None:proc.terminate()
assert any(x['path']=='Assets/Scenes/MainMenu.unity' for x in selected)
assert any(x['path']=='ProjectSettings/ProjectSettings.asset' for x in selected)
for name,data in [('R3-INVENTORY.json',inventory),('TEXT-MANIFEST.json',selected),('INPUT-IDENTITY.json',dict(
    scope='EXACT_R3_TEXT_INSPECTION_ONLY',archive_sha256=h.hexdigest(),archive_bytes=archive.stat().st_size,
    selected_files=len(selected),selected_bytes=total,game_candidate_modified=False,unity_started=False))]:
    (out/name).write_text(json.dumps(data,indent=2)+'\n','utf-8')
print(json.dumps(dict(selected_files=len(selected),selected_bytes=total)),flush=True)
