"""Cheap, license-free regression checks for the Bulk401 workflow handoffs.

Fixtures exercise file switching and archive validators. They do not execute
Unity, Apple's ditto, Xcode, or gameplay. Candidate hash is checked separately.
"""
from pathlib import Path
import contextlib
import hashlib
import io
import json
import os
import plistlib
import runpy
import shutil
import struct
import sys
import tempfile
from unittest.mock import patch
import zipfile
import prepare_bulk401_unity as transition
import bulk401_export_binding as binding


def main():
    repo = Path(__file__).resolve().parents[2]
    root = repo / 'Recovery/Bulk401-Generic-Repair-2026-10-24'
    expected = transition.CANDIDATE
    with zipfile.ZipFile(root / 'BULK401-CANDIDATE.zip') as z:
        candidate = z.read('BULK401-CANDIDATE.dll')
    if hashlib.sha256(candidate).hexdigest() != expected:
        raise ValueError('Actual candidate hash mismatch')
    checks = []
    def record(name, function):
        function()
        checks.append(name)

    def must_reject(function, message):
        try:
            function()
        except ValueError as e:
            if message not in str(e):
                raise AssertionError('Unexpected rejection: ' + str(e)) from e
        else:
            raise AssertionError('False acceptance: ' + message)

    with tempfile.TemporaryDirectory(prefix='bulk401-preflight-') as tmp:
        tmp = Path(tmp)
        project = tmp / 'project'
        def workcopy(name):
            p = tmp / name
            for folder in ('ProjectSettings', 'Assets/Plugins', 'Assets/Editor', 'Library/Bee', 'Library/ScriptAssemblies', 'Library/Artifacts'):
                (p / folder).mkdir(parents=True)
            (p / 'ProjectSettings/ProjectVersion.txt').write_text('m_EditorVersion: 2022.3.44f1c1\n')
            (p / 'Assets/Plugins/Assembly-CSharp.dll').write_bytes(b'baseline-fixture')
            shutil.copy2(repo / 'Recovery/Bulk389-Qualification-2026-10-23/ios-support/Assets/Editor/GodsPVZPackageReferenceMigrator.cs', p / 'Assets/Editor')
            (p / 'Assets/Editor/GodsPVZPackageReferenceMigrator.cs.meta').write_text('guid: abcdef\n')
            (p / 'Library/GodsPVZ.package-reference-migration.done').write_text('resolved=67/67\ndll_sha256=' + fake_baseline + '\n')
            (p / 'Library/Bee/stale.rsp').write_text('Assets/Editor/GodsPVZPackageReferenceMigrator.cs\n')
            (p / 'Library/ScriptAssemblies/old.dll').write_bytes(b'old initializer')
            (p / 'Library/Artifacts/keep').write_bytes(b'imported-textures')
            return p
        actual = tmp / 'candidate.dll'
        actual.write_bytes(candidate)
        fake_baseline = hashlib.sha256(b'baseline-fixture').hexdigest()
        # Only the baseline test fixture hash is substituted. The target binary
        # and legacy migrator are genuine, with production SHA checks retained.
        with patch.object(transition, 'BASELINE', fake_baseline):
            p = workcopy('good')
            record('transition_valid', lambda: transition.inject(p, actual, tmp / 'e-good'))
            assert (p / 'Assets/Plugins/Assembly-CSharp.dll').read_bytes() == candidate
            assert (p / 'Assets/Editor/GodsPVZPackageReferenceMigrator.cs').read_text() == transition.STUB
            assert (p / 'Assets/Editor/GodsPVZPackageReferenceMigrator.cs.meta').read_text() == 'guid: abcdef\n'
            assert not (p / 'Library/Bee').exists() and not (p / 'Library/ScriptAssemblies').exists()
            assert (p / 'Library/Artifacts/keep').read_bytes() == b'imported-textures'
            checks.append('source_guid_asset_cache_and_candidate_preserved')
            for name, target, value, error in [
                ('wrong_baseline', 'Assets/Plugins/Assembly-CSharp.dll', b'wrong', 'hash mismatch'),
                ('incomplete_migration', 'Library/GodsPVZ.package-reference-migration.done', b'resolved=66/67\n', 'marker required'),
                ('changed_migrator', 'Assets/Editor/GodsPVZPackageReferenceMigrator.cs', b'other', 'migrator source')]:
                p = workcopy(name)
                (p / target).write_bytes(value)
                record(name + '_rejected', lambda p=p, name=name, error=error: must_reject(lambda: transition.inject(p, actual, tmp / ('e-' + name)), error))
                assert (p / 'Library/Bee/stale.rsp').exists()
                assert (p / 'Library/Artifacts/keep').exists()
            wrong = tmp / 'wrong.dll'
            wrong.write_bytes(b'wrong')
            p = workcopy('wrong_candidate')
            record('wrong_candidate_rejected', lambda: must_reject(lambda: transition.inject(p, wrong, tmp / 'e-wrong-candidate'), 'hash mismatch'))

        xcode = tmp / 'xcode'
        for relative, data in [(binding.METADATA, b'synthetic-export-metadata'),
                               ('Unity-iPhone.xcodeproj/project.pbxproj', b'synthetic-pbx'),
                               ('Il2CppOutputProject/Source/il2cppOutput/GodsPVZRuntime1.cpp', b'synthetic-cpp'),
                               ('Il2CppOutputProject/Source/il2cppOutput/Generics__test.cpp', b'synthetic-generic'),
                               ('Il2CppOutputProject/Source/il2cppOutput/GenericMethods__test.cpp', b'synthetic-generic-method')]:
            file = xcode / relative
            file.parent.mkdir(parents=True, exist_ok=True)
            file.write_bytes(data)
        head = '1' * 40
        record('export_binding_created', lambda: binding.create(xcode, actual, head))
        original_binding = (xcode / binding.RECEIPT).read_bytes()
        record('wrong_workflow_rejected', lambda: must_reject(lambda: binding.validate(xcode, '2' * 40), 'binding mismatch'))
        cpp = xcode / 'Il2CppOutputProject/Source/il2cppOutput/GodsPVZRuntime1.cpp'
        cpp.write_bytes(b'tampered-cpp')
        record('changed_cpp_rejected', lambda: must_reject(lambda: binding.validate(xcode, head), 'Changed export file'))
        cpp.write_bytes(b'synthetic-cpp')
        for name in ['Generics__test.cpp','GenericMethods__test.cpp']:
            generic=xcode / 'Il2CppOutputProject/Source/il2cppOutput' / name
            saved=generic.read_bytes();generic.write_bytes(b'tampered-generic')
            record('changed_'+name+'_rejected',lambda:must_reject(lambda:binding.validate(xcode,head),'Changed export file'))
            generic.write_bytes(saved)
        damaged = json.loads(original_binding)
        damaged['candidate_sha256'] = '0' * 64
        (xcode / binding.RECEIPT).write_text(json.dumps(damaged))
        record('wrong_bound_candidate_rejected', lambda: must_reject(lambda: binding.validate(xcode, head), 'binding mismatch'))
        (xcode / binding.RECEIPT).write_bytes(original_binding)

        def macho(role=2, cpu=0x100000c, platform=2, signed=False):
            cmds = struct.pack('<6I', 0x32, 24, platform, 0xc0000, 0x120500, 0)
            if signed:
                cmds += struct.pack('<4I', 0x1d, 16, 0, 1)
            return struct.pack('<8I', 0xfeedfacf, cpu, 0, role, 2 if signed else 1, len(cmds), 0, 0) + cmds

        def package_case(name, mutate=None, build_head=None, explicit_export_head=None):
            case = tmp / name
            app = case / 'input.app'
            app.mkdir(parents=True)
            (app / 'Info.plist').write_bytes(plistlib.dumps(dict(CFBundleIdentifier='com.tipsGodsStudio.godsPVZ', CFBundleExecutable='GodsPVZ')))
            (app / 'GodsPVZ').write_bytes(macho())
            framework = app / 'Frameworks/UnityFramework.framework/UnityFramework'
            framework.parent.mkdir(parents=True)
            framework.write_bytes(macho(6))
            md = app / binding.METADATA
            md.parent.mkdir(parents=True)
            md.write_bytes((xcode / binding.METADATA).read_bytes())
            if mutate:
                mutate(app)
            def fake_ditto(args, **kwargs):
                if args[0] != 'ditto':
                    raise AssertionError('Unexpected subprocess')
                if len(args) == 3:
                    shutil.copytree(args[1], args[2])
                else:
                    payload = Path(args[-2])
                    with zipfile.ZipFile(args[-1], 'x') as z:
                        for file in payload.rglob('*'):
                            if file.is_file():
                                z.write(file, file.relative_to(payload.parent).as_posix())
            argv = ['package_bulk401_ipa.py', '--app', str(app), '--xcode-dir', str(xcode), '--output-dir', str(case / 'out')]
            if explicit_export_head:
                argv += ['--export-workflow-head', explicit_export_head]
            with patch('sys.argv', argv), patch.dict(os.environ, {'GITHUB_SHA': build_head or head}), patch('subprocess.run', fake_ditto), contextlib.redirect_stdout(io.StringIO()):
                runpy.run_path(str(repo / 'scripts/actions/package_bulk401_ipa.py'), run_name='__main__')
            report = json.loads((case / 'out/IPA-VALIDATION.json').read_text('utf-8'))
            assert report['workflow_head'] == head
            assert report['packaging_workflow_head'] == (build_head or head)
        record('synthetic_unsigned_archive_accepted', lambda: package_case('valid-archive'))
        record('reused_export_keeps_distinct_build_and_export_heads', lambda: package_case('reused-archive', build_head='2'*40, explicit_export_head=head))
        record('reused_export_wrong_head_rejected', lambda: must_reject(lambda: package_case('wrong-reuse-head', build_head='2'*40), 'binding mismatch'))
        for name, mutation, error in [
            ('wrong-cpu', lambda p: (p / 'GodsPVZ').write_bytes(macho(cpu=0x1000007)), 'arm64'),
            ('simulator', lambda p: (p / 'GodsPVZ').write_bytes(macho(platform=7)), 'physical iOS'),
            ('signed', lambda p: (p / 'GodsPVZ').write_bytes(macho(signed=True)), 'code signature'),
            ('wrong-role', lambda p: (p / 'GodsPVZ').write_bytes(macho(6)), 'role mismatch'),
            ('wrong-metadata', lambda p: (p / binding.METADATA).write_bytes(b'wrong'), 'metadata differs'),
            ('nested-signature', lambda p: (p / 'Frameworks/UnityFramework.framework/_CodeSignature').mkdir(), 'signing/provisioning')]:
            if name == 'nested-signature':
                def mutation(p):
                    s = p / 'Frameworks/UnityFramework.framework/_CodeSignature/CodeResources'
                    s.parent.mkdir()
                    s.write_bytes(b'signature')
            record(name + '_rejected', lambda name=name, mutation=mutation, error=error: must_reject(lambda: package_case(name, mutation), error))
    report = dict(scope='PORTABLE_WORKFLOW_HANDOFF_FIXTURES_ONLY', checks=checks, passed=len(checks),
                  real_candidate_sha256=expected, synthetic_baseline_hash=True,
                  unity_editor_started=False, apple_ditto_executed=False, xcode_executed=False, game_semantics='NOT_PROVEN')
    print(json.dumps(report, indent=2))


if __name__ == '__main__':
    main()
