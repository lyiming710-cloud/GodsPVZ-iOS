"""Bind a fresh Unity export to its unchanged candidate and workflow head."""
from pathlib import Path
import argparse
import hashlib
import json
import os
from prepare_bulk401_unity import CANDIDATE

RECEIPT = 'bulk401-export-binding.json'
METADATA = 'Data/Managed/Metadata/global-metadata.dat'


def sha(path):
    return hashlib.sha256(path.read_bytes()).hexdigest()


def validate(xcode, workflow_head):
    xcode = Path(xcode).resolve()
    data = json.loads((xcode / RECEIPT).read_text('utf-8'))
    if data['candidate_sha256'] != CANDIDATE or data['workflow_head'] != workflow_head:
        raise ValueError('Export candidate/workflow binding mismatch')
    rows = data['files']
    expected = {METADATA, 'Unity-iPhone.xcodeproj/project.pbxproj'}
    expected.update(p.relative_to(xcode).as_posix() for p in (xcode / 'Il2CppOutputProject/Source/il2cppOutput').glob('*') if p.suffix in ('.cpp','.c'))
    if len(expected) < 3 or {r['path'] for r in rows} != expected or len(rows) != len(expected):
        raise ValueError('Incomplete/duplicate exported file binding')
    for row in rows:
        path = xcode / row['path']
        if not path.resolve().is_relative_to(xcode) or path.stat().st_size != row['bytes'] or sha(path) != row['sha256']:
            raise ValueError('Changed export file: ' + row['path'])
    return data


def create(xcode, plugin, workflow_head):
    xcode = Path(xcode).resolve()
    if sha(Path(plugin)) != CANDIDATE:
        raise ValueError('Unity export modified the fixed candidate')
    files = [xcode / METADATA, xcode / 'Unity-iPhone.xcodeproj/project.pbxproj']
    files += sorted(p for p in (xcode / 'Il2CppOutputProject/Source/il2cppOutput').glob('*') if p.suffix in ('.cpp','.c'))
    if len(files) < 3:
        raise ValueError('Game C++ export missing')
    result = dict(scope='UNITY_EXPORT_INPUT_AND_OUTPUT_BINDING_ONLY', candidate_sha256=CANDIDATE,
                  workflow_head=workflow_head, files=[dict(path=p.relative_to(xcode).as_posix(), bytes=p.stat().st_size, sha256=sha(p)) for p in files],
                  game_semantics='NOT_PROVEN', device_execution='NOT_PERFORMED')
    receipt = xcode / RECEIPT
    if receipt.exists():
        raise ValueError('Do not overwrite previous export binding')
    receipt.write_text(json.dumps(result, indent=2) + '\n', 'utf-8')
    return validate(xcode, workflow_head)


if __name__ == '__main__':
    p = argparse.ArgumentParser()
    p.add_argument('--xcode-dir', required=True)
    p.add_argument('--plugin')
    p.add_argument('--export-workflow-head', default=os.environ.get('GITHUB_SHA'))
    a = p.parse_args()
    head = a.export_workflow_head
    if not head:
        p.error('An explicit export workflow head or GITHUB_SHA is required')
    data = create(a.xcode_dir, a.plugin, head) if a.plugin else validate(a.xcode_dir, head)
    print('BULK401_EXPORT_BINDING_PASS', len(data['files']))
