#!/usr/bin/env python3
"""Deterministically migrate AssetRipper package-script references using locked GUID evidence."""
from __future__ import annotations

import argparse
import hashlib
import json
from pathlib import Path

SERIALIZED_EXTENSIONS = {'.unity', '.prefab', '.asset', '.mat', '.controller', '.anim', '.overridecontroller'}
GAME_DLL_GUID = '7a9d04f2a2417489da795057c9d7db84'
EXPECTED_TYPES = 67
EXPECTED_LEGACY_REFS = 3126
EXPECTED_GAME_REFS = 263
EXPECTED_GAME_REF_FILES = 114


def sha256(data: bytes) -> str:
    return hashlib.sha256(data).hexdigest()


def main() -> int:
    ap = argparse.ArgumentParser()
    ap.add_argument('--project', required=True, type=Path)
    ap.add_argument('--evidence', required=True, type=Path)
    ap.add_argument('--dry-run', action='store_true')
    ap.add_argument('--report', type=Path)
    args = ap.parse_args()

    project = args.project.resolve()
    evidence_path = args.evidence.resolve()
    map_path = project / 'Recovery' / 'package-script-map-editor.json'
    assets = project / 'Assets'
    report_path = args.report.resolve() if args.report else project / 'Recovery' / 'package-reference-migration-report.json'

    if not map_path.is_file():
        raise SystemExit(f'missing map: {map_path}')
    if not evidence_path.is_file():
        raise SystemExit(f'missing evidence: {evidence_path}')
    if not assets.is_dir():
        raise SystemExit(f'missing Assets: {assets}')

    map_root = json.loads(map_path.read_text(encoding='utf-8'))
    evidence = json.loads(evidence_path.read_text(encoding='utf-8'))
    rows = evidence.get('mapping') or []
    if evidence.get('expected') != EXPECTED_TYPES or evidence.get('resolved') != EXPECTED_TYPES or len(rows) != EXPECTED_TYPES:
        raise SystemExit(f'evidence is not a closed {EXPECTED_TYPES}/{EXPECTED_TYPES} mapping')
    if evidence.get('failures'):
        raise SystemExit('evidence contains failures')

    expected_map = {}
    for pkg in map_root.get('packages') or []:
        for item in pkg.get('mapping') or []:
            key = (pkg['dll'], item['fullName'])
            expected_map[key] = (pkg['guid'].lower(), int(item['fileID']))
    if len(expected_map) != EXPECTED_TYPES:
        raise SystemExit(f'project map has {len(expected_map)} entries, expected {EXPECTED_TYPES}')

    replacements = []
    seen = set()
    for row in rows:
        key = (row['dll'], row['fullName'])
        if key in seen:
            raise SystemExit(f'duplicate evidence entry: {key}')
        seen.add(key)
        if key not in expected_map:
            raise SystemExit(f'evidence entry absent from project map: {key}')
        old_guid, old_file_id = expected_map[key]
        if row['old_dll_guid'].lower() != old_guid or int(row['old_fileID']) != old_file_id:
            raise SystemExit(f'evidence/project map mismatch: {key}')
        guid = (row.get('monoscript_guid') or row.get('official_monoscript_guid') or '').lower()
        if len(guid) != 32 or any(c not in '0123456789abcdef' for c in guid):
            raise SystemExit(f'invalid target guid for {key}: {guid}')
        old = f'{{fileID: {old_file_id}, guid: {old_guid}, type: 3}}'.encode('ascii')
        new = f'{{fileID: 11500000, guid: {guid}, type: 3}}'.encode('ascii')
        replacements.append((key, old, new, row))
    if seen != set(expected_map):
        missing = sorted(set(expected_map) - seen)
        raise SystemExit(f'evidence missing map entries: {missing}')

    files = sorted(p for p in assets.rglob('*') if p.is_file() and p.suffix.lower() in SERIALIZED_EXTENSIONS)
    before_counts = {key: 0 for key, *_ in replacements}
    official_before_counts = {key: 0 for key, *_ in replacements}
    game_refs_before = 0
    game_ref_files_before = 0
    game_token = f'guid: {GAME_DLL_GUID}'.encode('ascii')

    file_data = {}
    for path in files:
        data = path.read_bytes()
        file_data[path] = data
        g = data.count(game_token)
        if g:
            game_refs_before += g
            game_ref_files_before += 1
        for key, old, new, _ in replacements:
            before_counts[key] += data.count(old)
            official_before_counts[key] += data.count(new)

    legacy_before = sum(before_counts.values())
    nonzero_types = sum(1 for v in before_counts.values() if v)
    if legacy_before != EXPECTED_LEGACY_REFS or nonzero_types != EXPECTED_TYPES:
        raise SystemExit(
            f'pre-migration gate failed: legacy={legacy_before}/{EXPECTED_LEGACY_REFS}, '
            f'nonzero_types={nonzero_types}/{EXPECTED_TYPES}'
        )
    if game_refs_before != EXPECTED_GAME_REFS or game_ref_files_before != EXPECTED_GAME_REF_FILES:
        raise SystemExit(
            f'game reference baseline mismatch: refs={game_refs_before}/{EXPECTED_GAME_REFS}, '
            f'files={game_ref_files_before}/{EXPECTED_GAME_REF_FILES}'
        )

    changed = []
    replacements_done = 0
    for path, original in file_data.items():
        data = original
        count = 0
        for _, old, new, _ in replacements:
            c = data.count(old)
            if c:
                data = data.replace(old, new)
                count += c
        if data != original:
            changed.append({
                'path': path.relative_to(project).as_posix(),
                'replacements': count,
                'sha256_before': sha256(original),
                'sha256_after': sha256(data),
            })
            replacements_done += count
            if not args.dry_run:
                path.write_bytes(data)

    if replacements_done != EXPECTED_LEGACY_REFS:
        raise SystemExit(f'replacement count mismatch: {replacements_done}/{EXPECTED_LEGACY_REFS}')

    verification_data = file_data if args.dry_run else {p: p.read_bytes() for p in files}
    if args.dry_run:
        # Reconstruct post-state in memory for verification.
        verification_data = {}
        for path, original in file_data.items():
            data = original
            for _, old, new, _ in replacements:
                data = data.replace(old, new)
            verification_data[path] = data

    legacy_after = 0
    game_refs_after = 0
    game_ref_files_after = 0
    target_after_counts = {key: 0 for key, *_ in replacements}
    for path, data in verification_data.items():
        g = data.count(game_token)
        if g:
            game_refs_after += g
            game_ref_files_after += 1
        for key, old, new, _ in replacements:
            legacy_after += data.count(old)
            target_after_counts[key] += data.count(new)

    if legacy_after != 0:
        raise SystemExit(f'post-migration legacy references remain: {legacy_after}')
    if game_refs_after != EXPECTED_GAME_REFS or game_ref_files_after != EXPECTED_GAME_REF_FILES:
        raise SystemExit(
            f'post-migration game refs changed: refs={game_refs_after}/{EXPECTED_GAME_REFS}, '
            f'files={game_ref_files_after}/{EXPECTED_GAME_REF_FILES}'
        )

    type_rows = []
    for key, _, _, row in replacements:
        type_rows.append({
            'dll': key[0],
            'fullName': key[1],
            'old_fileID': int(row['old_fileID']),
            'old_dll_guid': row['old_dll_guid'].lower(),
            'new_fileID': 11500000,
            'new_guid': (row.get('monoscript_guid') or row.get('official_monoscript_guid')).lower(),
            'legacy_hits_before': before_counts[key],
            'target_hits_before': official_before_counts[key],
            'target_hits_after': target_after_counts[key],
        })

    result = {
        'schema': 1,
        'dry_run': args.dry_run,
        'project': str(project),
        'source_map_sha256': sha256(map_path.read_bytes()),
        'evidence_sha256': sha256(evidence_path.read_bytes()),
        'types': EXPECTED_TYPES,
        'legacy_refs_before': legacy_before,
        'legacy_refs_after': legacy_after,
        'replacements': replacements_done,
        'files_changed': len(changed),
        'game_dll_guid': GAME_DLL_GUID,
        'game_refs_before': game_refs_before,
        'game_ref_files_before': game_ref_files_before,
        'game_refs_after': game_refs_after,
        'game_ref_files_after': game_ref_files_after,
        'result': 'PASS',
        'type_results': sorted(type_rows, key=lambda r: (r['dll'], r['fullName'])),
        'changed_files': changed,
    }
    report_path.parent.mkdir(parents=True, exist_ok=True)
    report_path.write_text(json.dumps(result, indent=2, sort_keys=True) + '\n', encoding='utf-8')
    print(
        f"PASS types={EXPECTED_TYPES}/{EXPECTED_TYPES} refs={legacy_before}->0 "
        f"replacements={replacements_done} files={len(changed)} "
        f"gameRefs={game_refs_after}/{EXPECTED_GAME_REFS} gameFiles={game_ref_files_after}/{EXPECTED_GAME_REF_FILES}"
    )
    print(f'report={report_path}')
    return 0


if __name__ == '__main__':
    raise SystemExit(main())
