#!/usr/bin/env python3
"""Authenticate Native115 source delivery only; never execute the contained patcher or tools."""
import argparse, hashlib, json, pathlib, stat, zipfile

ZIP_SHA = '077aba07f5bf8882971786f1c56f8e3c2ad080dc1fe9cf814496aaae1609dc64'
SOURCE_MANIFEST_SHA = '150e84c88d648a4e170ca93516c1993f301ed5cc238cd9bc55f819895968fc72'

def sha(b): return hashlib.sha256(b).hexdigest()

def main():
    p = argparse.ArgumentParser()
    p.add_argument('--archive', type=pathlib.Path, required=True)
    p.add_argument('--destination', type=pathlib.Path, required=True)
    a = p.parse_args()
    assert sha(a.archive.read_bytes()) == ZIP_SHA, 'Source archive digest mismatch'
    root = a.destination.resolve()
    root.mkdir(parents=True, exist_ok=False)
    with zipfile.ZipFile(a.archive) as z:
        for f in z.infolist():
            n = pathlib.PurePosixPath(f.filename)
            assert not n.is_absolute() and '..' not in n.parts and '\\' not in f.filename
            assert not stat.S_ISLNK(f.external_attr >> 16), 'Symlink refused'
            z.extract(f, root)
    man_path = root / 'native-bulk200-codex-2026-10-22/SOURCE-MANIFEST.json'
    assert sha(man_path.read_bytes()) == SOURCE_MANIFEST_SHA, 'Source manifest digest mismatch'
    m = json.loads(man_path.read_text(encoding='utf-8'))
    for entry in m['files']:
        f_p = root / 'native-bulk200-codex-2026-10-22' / entry['path']
        assert f_p.stat().st_size == entry['bytes'] and sha(f_p.read_bytes()) == entry['sha256'], entry['path']
    print(json.dumps({'source_sha256': SOURCE_MANIFEST_SHA, 'files_verified': len(m['files']), 'project_tests_executed': False}))

if __name__ == '__main__':
    main()

