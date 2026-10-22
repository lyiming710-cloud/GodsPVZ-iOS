#!/usr/bin/env python3
"""Authenticate Bulk250-R2 retest source delivery only; never execute the contained patcher or tools."""
import argparse, hashlib, json, pathlib, stat, zipfile

ZIP_SHA = '8243d3d19019b9c93fe9d231375df32ef6efbe807db04aa9ad9e5c3621497d70'
SOURCE_MANIFEST_SHA = 'fd5b40332113b679d2cb8b59c88d226dfa18526665ca9d6fea3a792235759f36'

def sha(b): return hashlib.sha256(b).hexdigest()

def main():
    p = argparse.ArgumentParser()
    p.add_argument('--archive', type=pathlib.Path, required=True)
    p.add_argument('--destination', type=pathlib.Path, required=True)
    p.add_argument('--receipt', type=pathlib.Path, default=None)
    a = p.parse_args()
    arch_bytes = a.archive.read_bytes()
    actual_zip_sha = sha(arch_bytes)
    assert actual_zip_sha == ZIP_SHA, f'Source archive digest mismatch: {actual_zip_sha} != {ZIP_SHA}'
    root = a.destination.resolve()
    root.mkdir(parents=True, exist_ok=False)
    with zipfile.ZipFile(a.archive) as z:
        for f in z.infolist():
            n = pathlib.PurePosixPath(f.filename)
            assert not n.is_absolute() and '..' not in n.parts and '\\' not in f.filename
            assert not stat.S_ISLNK(f.external_attr >> 16), 'Symlink refused'
            z.extract(f, root)
    man_path = root / 'source250-r2/SOURCE-MANIFEST.json'
    actual_man_sha = sha(man_path.read_bytes())
    assert actual_man_sha == SOURCE_MANIFEST_SHA, f'Source manifest digest mismatch: {actual_man_sha} != {SOURCE_MANIFEST_SHA}'
    m = json.loads(man_path.read_text(encoding='utf-8'))
    for entry in m['files']:
        f_p = root / 'source250-r2' / entry['path']
        assert f_p.stat().st_size == entry['bytes'] and sha(f_p.read_bytes()) == entry['sha256'], entry['path']
    
    receipt_data = {
        'schema': 'bulk250-r2-restore-receipt-v1',
        'archive_sha256': actual_zip_sha,
        'archive_bytes': len(arch_bytes),
        'manifest_sha256': actual_man_sha,
        'files_verified': len(m['files']),
        'project_tests_executed': False,
        'scope': 'source authentication only; no execution'
    }
    if a.receipt:
        a.receipt.parent.mkdir(parents=True, exist_ok=True)
        a.receipt.write_text(json.dumps(receipt_data, indent=2), encoding='utf-8')
    print(json.dumps(receipt_data))

if __name__ == '__main__':
    main()

