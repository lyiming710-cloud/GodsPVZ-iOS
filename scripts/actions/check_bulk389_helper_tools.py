"""Write/check the reusable subset's per-file digest list; no Editor activation."""
from pathlib import Path
import argparse
import hashlib
import json

EDITOR_SHA = '0008115c785784baddb19e2b13b384ac845438239f293efb0c2e8b1379a1fe14'


def main():
    p = argparse.ArgumentParser()
    p.add_argument('mode', choices=['create', 'verify'])
    p.add_argument('root')
    a = p.parse_args()
    root = Path(a.root).resolve()
    manifest = root / 'HELPER-TOOLS-MANIFEST.json'
    sha = lambda p: hashlib.sha256(p.read_bytes()).hexdigest()
    if a.mode == 'create':
        if manifest.exists():
            raise ValueError('Manifest already exists')
        entries = []
        for file in sorted(root.rglob('*')):
            if file.is_file():
                if not file.resolve().is_relative_to(root):
                    raise ValueError('Tool path outside subset')
                entries.append(dict(path=file.relative_to(root).as_posix(), bytes=file.stat().st_size, sha256=sha(file)))
        manifest.write_text(json.dumps(dict(source_editor_sha256=EDITOR_SHA, entries=entries), indent=2) + '\n', 'utf-8')
    data = json.loads(manifest.read_text('utf-8'))
    if data['source_editor_sha256'] != EDITOR_SHA:
        raise ValueError('Tool subset source mismatch')
    entries = data['entries']
    actual = {p.relative_to(root).as_posix() for p in root.rglob('*') if p.is_file() and p != manifest}
    if len(entries) < 50 or len(entries) != len(actual) or {e['path'] for e in entries} != actual:
        raise ValueError('Tool subset inventory mismatch')
    for item in entries:
        file = root / item['path']
        if not file.resolve().is_relative_to(root) or file.stat().st_size != item['bytes'] or sha(file) != item['sha256']:
            raise ValueError('Tool subset digest mismatch: ' + item['path'])
    print('HELPER_TOOLS_VERIFIED', len(entries), sha(manifest))


if __name__ == '__main__':
    main()
