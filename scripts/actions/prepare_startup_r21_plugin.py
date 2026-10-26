"""Align the disposable plugin filename with its fixed assembly identity.

The DLL and its .meta stay byte-identical. Keeping the original GUID allows
Unity to resolve every existing (GUID, local fileID) script reference again.
"""
from pathlib import Path
import argparse,hashlib,json,os

CANDIDATE='75160a3233f1e1ab13a44f712efb4b47fdeb9b57bd38fa25f180276bb7b50e15'
GUID='60b84a767c46ced9aacc93f38da4d2f3'

def prepare(project,evidence):
    root=Path(project).resolve();evidence=Path(evidence).resolve()
    source=root/'Assets/Plugins/Assembly-CSharp.dll';target=root/'Assets/Plugins/GodsPVZRuntime1.dll'
    if not (root/'ProjectSettings/ProjectVersion.txt').is_file():raise ValueError('Require disposable Unity work copy')
    for path in [source,source.with_suffix('.dll.meta'),target,target.with_suffix('.dll.meta')]:
        if not path.resolve().is_relative_to(root) or path.is_symlink():raise ValueError('Unsafe plugin path')
    if target.exists() or target.with_suffix('.dll.meta').exists():raise ValueError('Do not overwrite an existing aligned plugin')
    if hashlib.sha256(source.read_bytes()).hexdigest()!=CANDIDATE:raise ValueError('Fixed candidate hash mismatch')
    meta=source.with_suffix('.dll.meta').read_bytes()
    if ('guid: '+GUID).encode() not in meta.splitlines():raise ValueError('Unexpected plugin GUID')
    evidence.mkdir(parents=True,exist_ok=True);receipt=evidence/'startup-plugin-transition.json'
    if receipt.exists():raise ValueError('Do not overwrite transition evidence')
    before_meta=hashlib.sha256(meta).hexdigest()
    source.rename(target);source.with_suffix('.dll.meta').rename(target.with_suffix('.dll.meta'))
    assert hashlib.sha256(target.read_bytes()).hexdigest()==CANDIDATE
    assert target.with_suffix('.dll.meta').read_bytes()==meta
    data=dict(scope='PLUGIN_FILENAME_ALIGNMENT_ONLY',candidate_sha256=CANDIDATE,dll_bytes_changed=False,
        meta_bytes_changed=False,meta_sha256=before_meta,script_guid=GUID,old_path=source.relative_to(root).as_posix(),
        new_path=target.relative_to(root).as_posix(),expected_assembly_identity='GodsPVZRuntime1',
        script_local_fileids_changed=False,unity_resolution='REQUIRES_EDITOR_AUDIT')
    receipt.write_text(json.dumps(data,indent=2)+'\n','utf-8')
    return data

if __name__=='__main__':
    p=argparse.ArgumentParser();p.add_argument('--project',required=True);p.add_argument('--evidence',required=True);a=p.parse_args()
    print(json.dumps(prepare(a.project,a.evidence)))
