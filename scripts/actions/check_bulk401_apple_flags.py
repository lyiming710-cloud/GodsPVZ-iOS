"""Regression controls for the real-export same-path false rejection."""
from pathlib import Path
from unittest.mock import patch
import json
from bulk401_apple_flags import OLD_ROOT, rebind
from audit_bulk389_all_generated_cpp import compile_flags as historical_flags


def main():
    repo = Path(__file__).resolve().parents[2]
    template = repo / 'Recovery/Bulk389-Qualification-2026-10-23/apple-failure-39988665189/APPLE-COMPILE-COMMAND.json'
    command = json.loads(template.read_text('utf-8'))
    compiler = command[0]
    sdk = command[command.index('-isysroot') + 1]
    rows = []

    def reject(name, fn):
        try:
            fn()
        except ValueError as error:
            rows.append(dict(name=name, expected_rejection=True, pass_=True, reason=str(error)))
        else:
            raise AssertionError('Expected rejection: ' + name)

    with patch('audit_bulk389_all_generated_cpp.subprocess.check_output', side_effect=[compiler, sdk]):
        reject('reproduce_historical_same_path_false_rejection', lambda: historical_flags(OLD_ROOT))
    for root in (OLD_ROOT, '/Users/runner/work/_temp/reused-export/xcode', OLD_ROOT + '/nested'):
        flags = rebind(command, root, compiler, sdk)
        assert '-I' + root + '/Il2CppOutputProject/Source/il2cppOutput' in flags
        assert flags[flags.index('-arch') + 1] == 'arm64'
        assert '-c' not in flags and '-o' not in flags
        assert flags[-3:] == ['-fsyntax-only', '-ferror-limit=0', '-fno-color-diagnostics']
        if root != OLD_ROOT and not root.startswith(OLD_ROOT + '/'):
            with patch('audit_bulk389_all_generated_cpp.subprocess.check_output', side_effect=[compiler, sdk]):
                assert flags == historical_flags(root), 'Changed-root flags must stay exactly historical'
        rows.append(dict(name='valid_export_root_' + root, expected_rejection=False, pass_=True))

    bad = command.copy(); bad[bad.index('arm64')] = 'armv7'
    reject('wrong_architecture', lambda: rebind(bad, OLD_ROOT, compiler, sdk))
    reject('duplicate_architecture', lambda: rebind(command + ['-arch', 'arm64'], OLD_ROOT, compiler, sdk))
    bad_include = command.copy(); bad_include[bad_include.index('-I' + OLD_ROOT + '/Il2CppOutputProject/IL2CPP/libil2cpp')] += '-foreign'
    reject('foreign_include_even_with_same_root', lambda: rebind(bad_include, OLD_ROOT, compiler, sdk))
    reject('unrebound_external_include', lambda: rebind(command + ['-I/other/export'], OLD_ROOT, compiler, sdk))
    reject('missing_source', lambda: rebind(command[:-1], OLD_ROOT, compiler, sdk))
    reject('relative_export', lambda: rebind(command, 'relative/export', compiler, sdk))
    reject('parent_traversal', lambda: rebind(command, '/tmp/../other', compiler, sdk))
    reject('wrong_sdk', lambda: rebind(command, OLD_ROOT, compiler, sdk.replace('18.5', '18.4')))
    reject('wrong_toolchain', lambda: rebind(command, OLD_ROOT, compiler.replace('16.4', '16.3'), sdk))
    result = dict(scope='CAPTURED_COMMAND_REBASING_CONTROLS_NO_MAC_COMPILER_EXECUTION',
                  checks=len(rows), pass_=True, historical_auditor_unchanged=True, results=rows)
    print(json.dumps(result, indent=2))


if __name__ == '__main__':
    main()
