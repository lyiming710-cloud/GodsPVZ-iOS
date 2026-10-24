"""Install two original-backed shaders only in a disposable Unity work copy."""
from pathlib import Path
import argparse
import hashlib
import json
import re

ROOT=Path(__file__).resolve().parents[2]
SOURCE=ROOT/'Recovery/Custom-Shaders-2026-10-24'

def sha(path):
    return hashlib.sha256(path.read_bytes()).hexdigest()

def install(project,evidence):
    project=Path(project).resolve();evidence=Path(evidence).resolve()
    if not (project/'ProjectSettings/ProjectVersion.txt').is_file():
        raise ValueError('Require reconstructed Unity project')
    manifest=json.loads((SOURCE/'SOURCE-MANIFEST.json').read_text('utf-8'))
    if manifest['scope']!='TWO_CUSTOM_SHADER_SOURCES_ONLY':
        raise ValueError('Unexpected source manifest')
    for row in manifest['files']:
        path=SOURCE/row['file']
        if not path.resolve().is_relative_to(SOURCE.resolve()) or path.stat().st_size!=row['bytes'] or sha(path)!=row['sha256']:
            raise ValueError('Source input mismatch: '+str(path))
    targets=[]
    for row in manifest['shaders']:
        target=project/'Assets/Shader'/row['file'];meta=target.with_suffix('.shader.meta')
        if not target.resolve().is_relative_to(project) or target.is_symlink() or meta.is_symlink():
            raise ValueError('Target escapes work copy')
        if sha(target)!=row['original_dummy_sha256'] or sha(meta)!=row['original_meta_sha256']:
            raise ValueError('Exact original dummy and GUID bytes required: '+str(target))
        if 'guid: '+row['guid'] not in meta.read_text('utf-8'):
            raise ValueError('Wrong shader GUID')
        targets.append((row,target,meta))
    for row in manifest['includes']:
        target=project/'Assets/Shader'/row['file']
        if target.exists() or target.with_suffix('.hlsl.meta').exists():
            raise ValueError('Do not overwrite an existing include')
        if not target.resolve().is_relative_to(project):
            raise ValueError('Include escapes work copy')
        matches=[p for p in (project/'Assets').rglob('*.meta') if re.search(r'^guid: '+row['guid']+r'\s*$',p.read_text('utf-8',errors='replace'),re.M)]
        if matches:
            raise ValueError('New include GUID collides')
    evidence.mkdir(parents=True,exist_ok=True)
    receipt=evidence/'custom-shader-workcopy-transition.json'
    if receipt.exists():
        raise ValueError('Preserve previous work-copy transition receipt')
    rows=[]
    for row,target,meta in targets:
        before=sha(meta)
        target.write_bytes((SOURCE/'Shaders'/row['file']).read_bytes())
        assert sha(meta)==before
        rows.append(dict(path=target.relative_to(project).as_posix(),before_sha256=row['original_dummy_sha256'],after_sha256=sha(target),guid=row['guid'],meta_sha256=before,meta_bytes_preserved=True))
    for row in manifest['includes']:
        target=project/'Assets/Shader'/row['file']
        target.write_bytes((SOURCE/'Shaders'/row['file']).read_bytes())
        target.with_suffix('.hlsl.meta').write_bytes(('fileFormatVersion: 2\nguid: '+row['guid']+'\nShaderIncludeImporter:\n  externalObjects: {}\n  userData:\n  assetBundleName:\n  assetBundleVariant:\n').encode())
    result=dict(scope='DISPOSABLE_UNITY_WORK_COPY_TWO_CUSTOM_SHADERS',shaders=rows,includes=manifest['includes'],dll_changed=False,serialized_references_changed=False)
    receipt.write_bytes((json.dumps(result,indent=2)+'\n').encode())
    return result

if __name__=='__main__':
    p=argparse.ArgumentParser();p.add_argument('--project',required=True);p.add_argument('--evidence',required=True);a=p.parse_args()
    print(json.dumps(install(a.project,a.evidence)))
