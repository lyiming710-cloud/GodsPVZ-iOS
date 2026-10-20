#!/usr/bin/env python3
"""Authenticate source delivery only; never execute the contained patcher or tools."""
import argparse, hashlib, json, pathlib, stat, zipfile

ZIP_SHA = '02110e9566830a6dcbf24396c315f89f246d599bdfaca6b820d7bdfef0458256'
MANIFEST_SHA = 'c8bd8e56fb7ea0ac15e2c2b2b1a4fc7cd8c1f616f964e9efe58ca25101ee6678'
SOURCE_SHA = 'd2d30958b80d63eb6793b640f466e2227f382126dee8ccb12405f2a0100facbd'

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
        assert len(z.namelist()) == len(set(z.namelist())), 'Duplicate entry'
        manifest_bytes = z.read('CAPSULE-MANIFEST.json')
        assert sha(manifest_bytes) == MANIFEST_SHA
        m = json.loads(manifest_bytes)
        entries = {x['path']: x for x in m['files']}
        assert set(z.namelist()) == set(entries) | {'CAPSULE-MANIFEST.json'}
        for f in z.infolist():
            n = pathlib.PurePosixPath(f.filename)
            assert not n.is_absolute() and '..' not in n.parts and '\\' not in f.filename
            assert not stat.S_ISLNK(f.external_attr >> 16), 'Symlink refused'
            b = z.read(f)
            if f.filename in entries:
                expected = entries[f.filename]
                assert len(b) == expected['bytes'] and sha(b) == expected['sha256'], f.filename
            target = root / f.filename
            target.parent.mkdir(parents=True, exist_ok=True)
            target.write_bytes(b)
    assert sha((root / 'src/SOURCE-MANIFEST.json').read_bytes()) == SOURCE_SHA
    print(json.dumps({'source_sha256': SOURCE_SHA, 'authenticated_payload_files': len(entries), 'project_tests_executed': False}))

if __name__ == '__main__':
    main()

