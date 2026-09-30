"""Recover the exact missing managed executable from the hash-locked historical seed."""
import argparse
import hashlib
import json
import os
from pathlib import Path
import zipfile

SEED_SHA256 = 'ec652fbedfbd75fd4616aa18986ee26e2791b60ff3dbd099ecf9d52b2342b0b0'


def main():
    parser = argparse.ArgumentParser()
    parser.add_argument('--seed', type=Path, required=True)
    parser.add_argument('--review-status', type=Path, required=True)
    parser.add_argument('--output', type=Path, required=True)
    args = parser.parse_args()
    assert hashlib.sha256(args.seed.read_bytes()).hexdigest() == SEED_SHA256
    args.output.mkdir(parents=True, exist_ok=False)
    expected = json.loads(args.review_status.read_text())['stages']['baseline']['assemblies']
    with zipfile.ZipFile(args.seed) as archive:
        prefix = 'Library/Bee/artifacts/iOS/ManagedStripped/'
        name = prefix+'Unity.VisualScripting.Core.exe'
        assert name in archive.namelist(), 'exact managed executable absent from historical seed'
        payload = archive.read(name)
        assert payload[:2] == b'MZ' and len(payload) > 8192
        records = {}
        for dll, want in expected.items():
            if dll == 'GodsPVZRuntime1.dll':
                continue
            value = archive.read(prefix+dll)
            got = hashlib.sha256(value).hexdigest()
            records[dll] = {'expected': want, 'actual': got, 'matches_review': got == want}
        # Retain evidence of any identity difference; it prevents future exact-input claims.
        raw_manifest = archive.read('MANIFEST.txt')
    path = args.output/'Unity.VisualScripting.Core.exe'
    path.write_bytes(payload)
    (args.output/'historical-seed-MANIFEST.txt').write_bytes(raw_manifest)
    report = {'seed_artifact_id': 10924263662, 'seed_zip_sha256': SEED_SHA256,
              'file': path.name, 'sha256': hashlib.sha256(payload).hexdigest(), 'bytes': len(payload),
              'other_dlls': records, 'all_support_dlls_match_review': all(v['matches_review'] for v in records.values()),
              'source_commit': os.environ['GITHUB_SHA'], 'run_id': os.environ['GITHUB_RUN_ID'],
              'qualification': 'INPUT_RECOVERY_ONLY; original Native23 exe hash was not archived; compare regenerated CPP and metadata before exact-replay acceptance'}
    (args.output/'INPUT-RECOVERY.json').write_text(json.dumps(report, indent=2)+'\n')
    print(json.dumps({k:v for k,v in report.items() if k != 'other_dlls'}, indent=2))
    assert report['all_support_dlls_match_review'], 'support DLL identities differ; retain recovery evidence but reject exact replay'


if __name__ == '__main__':
    main()
