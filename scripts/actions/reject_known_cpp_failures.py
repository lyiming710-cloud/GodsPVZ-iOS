"""Reject the exact candidate already proven to have generated-code errors.

This is a negative cache, not a qualification or semantic verifier for a new DLL.
"""
from pathlib import Path
import hashlib
import json
import zipfile

repo = Path(__file__).resolve().parents[2]
root = repo / 'Recovery/Bulk389-Qualification-2026-10-23'
known = json.loads((root / 'COMBINED-GENERATED-SYNTAX-CENSUS.json').read_text('utf-8'))
with zipfile.ZipFile(root / 'BULK389-R4-CANDIDATE.zip') as archive:
    dlls = [name for name in archive.namelist() if name.endswith('.dll')]
    if len(dlls) != 1:
        raise ValueError('Exactly one candidate DLL required')
    actual = hashlib.sha256(archive.read(dlls[0])).hexdigest()
if actual == known['candidate_sha256'] and known['total_failed_sources']:
    raise SystemExit('FULL_PIPELINE_BLOCKED_KNOWN_CPP_ERRORS: candidate=' + actual +
                     '; failed_files=' + str(known['total_failed_sources']) +
                     '; diagnostics=' + str(known['cpp']['error_diagnostics']) +
                     '; repair and independently qualify the changed candidate before another Unity export')
print('NO_MATCHING_KNOWN_BLOCKER; this is not compiler or semantic qualification')
