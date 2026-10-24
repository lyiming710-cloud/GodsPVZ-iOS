"""Restore only official TMP shader sources into a new work-copy directory.

Avoid Unity's asynchronous full-package importer and folder GUID remapping.
No sample fonts/settings are imported; existing original font data stays put.
"""
from pathlib import Path,PurePosixPath
import argparse,hashlib,json,tarfile,re

def prepare(project,evidence):
    root=Path(project).resolve();out=Path(evidence).resolve()
    package=root/'Packages/com.unity.textmeshpro/Package Resources/TMP Essential Resources.unitypackage'
    target=root/'Assets/Recovery/TMP-Official-Shaders'
    if not (root/'Assets/Plugins/GodsPVZRuntime1.dll').is_file():raise ValueError('Aligned plugin required')
    if target.exists() or not target.resolve().is_relative_to(root):raise ValueError('Require a fresh shader source directory')
    if not package.is_file():raise ValueError('Pinned TMP resource package missing')
    collected=[]
    with tarfile.open(package,'r:gz') as tar:
        members={m.name:m for m in tar.getmembers() if m.isfile()}
        for name,member in members.items():
            if not name.endswith('/pathname'):continue
            source=tar.extractfile(member).read().decode('utf-8-sig').rstrip('\0\r\n')
            if '/Shaders/' not in source or not source.endswith(('.shader','.cginc','.hlsl')):continue
            relative=PurePosixPath(source.split('/Shaders/',1)[1])
            if relative.is_absolute() or '..' in relative.parts:raise ValueError('Unsafe shader pathname')
            prefix=name.rsplit('/',1)[0];data=tar.extractfile(members[prefix+'/asset']).read()
            meta=tar.extractfile(members[prefix+'/asset.meta']).read()
            if b'DummyShaderTextExporter' in data:raise ValueError('Official package unexpectedly contains dummy shader')
            digest=hashlib.sha256(data).hexdigest()
            guid=hashlib.sha256(('GodsPVZ official TMP shader '+relative.as_posix()+' '+digest).encode()).hexdigest()[:32]
            original_guid=re.search(rb'(?m)^guid: ([0-9a-f]{32})',meta)
            if original_guid is None:raise ValueError('Official shader meta GUID missing')
            meta=re.sub(rb'(?m)^(guid: )[0-9a-f]{32}',lambda m:m.group(1)+guid.encode(),meta,count=1)
            collected.append((relative,data,meta,dict(source=source,path=relative.as_posix(),bytes=len(data),sha256=digest,source_guid=original_guid.group(1).decode(),workcopy_guid=guid)))
    if sum(x[0].suffix=='.shader' for x in collected)<7:raise ValueError('Official TMP shader inventory incomplete')
    if len({x[0] for x in collected})!=len(collected):raise ValueError('Duplicate shader path')
    target.mkdir(parents=True)
    for relative,data,meta,row in collected:
        path=target/Path(*relative.parts);path.parent.mkdir(parents=True,exist_ok=True);path.write_bytes(data)
        Path(str(path)+'.meta').write_bytes(meta)
    out.mkdir(parents=True,exist_ok=True)
    report=dict(scope='EXACT_PINNED_PACKAGE_SHADER_SOURCE_ONLY',package_sha256=hashlib.sha256(package.read_bytes()).hexdigest(),
        source_code_bytes_changed=False,full_package_imported=False,sample_fonts_imported=False,files=[x[3] for x in collected])
    (out/'official-tmp-shader-sources.json').write_text(json.dumps(report,indent=2)+'\n','utf-8')
    return report

if __name__=='__main__':
    p=argparse.ArgumentParser();p.add_argument('--project',required=True);p.add_argument('--evidence',required=True);a=p.parse_args()
    print(json.dumps(prepare(a.project,a.evidence)))
