"""Validate and switch only the disposable Unity work copy after legacy migration.

Keep the migrator source path present, but remove its executable initializer and
discard stale compiled-script graphs. Asset imports and gameplay DLL bytes are
not rewritten by a legacy migration on subsequent Editor starts.
"""
from pathlib import Path
import argparse
import hashlib
import json
import shutil

CANDIDATE = 'eef6c35a2dceb0c38d1032837204abaff56f3a4c4d029ca47d76ff496fd69293'
BASELINE = 'f144dd624d2b0c05b5ff01fc12ff455ac394c87852a4fc3c87e8c3611582a433'
MIGRATOR = '26490d205ab838d1808b12bd66a3fa75d1363b73f4eca6d234c5140968edf304'
STUB = '''#if UNITY_EDITOR
// Work-copy-only tombstone: retain the source path used by the compilation graph.
// No InitializeOnLoad, static constructor, delayCall, or DLL rewriting is allowed.
public static class GodsPVZPackageReferenceMigrator
{
    public static void RunBatch()
    {
        throw new System.InvalidOperationException("Legacy migration was completed before Bulk389 injection; rerunning it is forbidden.");
    }
}
#endif
'''


def sha(path):
    return hashlib.sha256(path.read_bytes()).hexdigest()


def inject(project, candidate, evidence):
    project = Path(project).resolve()
    candidate = Path(candidate).resolve()
    evidence = Path(evidence).resolve()
    plugin = project / 'Assets/Plugins/Assembly-CSharp.dll'
    migrator = project / 'Assets/Editor/GodsPVZPackageReferenceMigrator.cs'
    marker = project / 'Library/GodsPVZ.package-reference-migration.done'
    if not (project / 'ProjectSettings/ProjectVersion.txt').is_file():
        raise ValueError('Require a reconstructed Unity project')
    if sha(plugin) != BASELINE or sha(candidate) != CANDIDATE:
        raise ValueError('Baseline/candidate hash mismatch')
    if sha(migrator) != MIGRATOR:
        raise ValueError('Unexpected legacy migrator source')
    lines = marker.read_text('utf-8').splitlines()
    if lines.count('resolved=67/67') != 1 or lines.count('dll_sha256=' + BASELINE) != 1:
        raise ValueError('Exact completed migration marker required')
    # Resolve all deletion targets BEFORE mutating anything. Refuse junctions or
    # symlinks so only the disposable project's two script caches can be removed.
    caches = [project / 'Library/Bee', project / 'Library/ScriptAssemblies']
    for path in caches + [plugin, migrator, marker]:
        if not path.resolve().is_relative_to(project) or path.is_symlink():
            raise ValueError('Work-copy path escapes project: ' + str(path))
    evidence.mkdir(parents=True, exist_ok=True)
    receipt = evidence / 'workcopy-transition.json'
    if receipt.exists():
        raise ValueError('Do not overwrite a previous transition receipt')
    before_meta = sha(migrator.with_suffix('.cs.meta')) if migrator.with_suffix('.cs.meta').exists() else None
    removed = []
    for path in caches:
        if path.exists():
            shutil.rmtree(path)
            removed.append(path.relative_to(project).as_posix())
    migrator.write_text(STUB, encoding='utf-8', newline='\n')
    shutil.copyfile(candidate, plugin)
    if sha(plugin) != CANDIDATE:
        raise ValueError('Injected candidate differs')
    after_meta = sha(migrator.with_suffix('.cs.meta')) if migrator.with_suffix('.cs.meta').exists() else None
    if before_meta != after_meta:
        raise ValueError('Migrator GUID changed')
    result = dict(scope='DISPOSABLE_UNITY_WORK_COPY_ONLY',
                  baseline_sha256=BASELINE, candidate_sha256=sha(plugin),
                  migrator_source_before_sha256=MIGRATOR, migrator_source_after_sha256=sha(migrator),
                  migrator_path_preserved=True, migrator_meta_preserved=True,
                  migration_marker_sha256=sha(marker), script_caches_removed=removed,
                  imported_asset_cache_preserved=True, candidate_bytes_modified_during_transition=False)
    receipt.write_text(json.dumps(result, indent=2) + '\n', encoding='utf-8')
    return result


if __name__ == '__main__':
    p = argparse.ArgumentParser()
    p.add_argument('--project', required=True)
    p.add_argument('--candidate', required=True)
    p.add_argument('--evidence', required=True)
    a = p.parse_args()
    print(json.dumps(inject(a.project, a.candidate, a.evidence)))
