#!/usr/bin/env python3
"""Authenticate source delivery only; never execute the contained patcher or tools."""
import argparse, hashlib, json, pathlib, stat, zipfile

ZIP_SHA = '9064b38e33daafe7812b40d3036c055f0bb51c0d1cdf613e10d70980b1ad3e66'
SOURCE_MANIFEST_SHA = '68914bbc9742797075bab725e8b7bfe320dc8b48be2984a20ebab60e8c71d95d'

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
    man_path = root / 'native-bulk180-codex-2026-10-21/SOURCE-MANIFEST.json'
    assert sha(man_path.read_bytes()) == SOURCE_MANIFEST_SHA, 'Source manifest digest mismatch'
    m = json.loads(man_path.read_text(encoding='utf-8'))
    for entry in m['files']:
        f_p = root / 'native-bulk180-codex-2026-10-21' / entry['path']
        assert f_p.stat().st_size == entry['bytes'] and sha(f_p.read_bytes()) == entry['sha256'], entry['path']
    print(json.dumps({'source_sha256': SOURCE_MANIFEST_SHA, 'files_verified': len(m['files']), 'project_tests_executed': False}))

if __name__ == '__main__':
    main()

