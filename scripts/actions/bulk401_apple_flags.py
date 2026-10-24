"""Rebind captured Apple flags, including an unchanged absolute export path.

The historical Bulk389 auditor stays unchanged. Validate the resulting include
paths against the requested export rather than rejecting its former spelling.
"""
from pathlib import Path, PurePosixPath
import json
import subprocess

OLD_ROOT = '/Users/runner/work/_temp/build/xcode'
INCLUDES = (
    'Il2CppOutputProject/Source/il2cppOutput',
    'Il2CppOutputProject/IL2CPP/libil2cpp/pch',
    'Il2CppOutputProject/IL2CPP/libil2cpp',
    'Il2CppOutputProject/IL2CPP/external/baselib/Include',
    'Il2CppOutputProject/IL2CPP/libil2cpp/os/ClassLibraryPAL/brotli/include',
    'Il2CppOutputProject/IL2CPP/external/baselib/Platforms/IOS/Include',
)


def rebind(command, root, compiler, sdk):
    root = str(root).rstrip('/')
    if not root.startswith('/') or '..' in PurePosixPath(root).parts or not root:
        raise ValueError('Require an absolute normalized export root')
    if '/Xcode_16.4.app/' not in compiler or not sdk.endswith('/iPhoneOS18.5.sdk'):
        raise ValueError('Require exact Apple toolchain/SDK')
    source = OLD_ROOT + '/Il2CppOutputProject/Source/il2cppOutput/Generics__63.cpp'
    if command.count(source) != 1:
        raise ValueError('Captured source input is missing or duplicated')
    flags = [compiler]
    i = 1
    while i < len(command):
        item = command[i]
        if item == '-o':
            i += 2
            continue
        if item in ('-c', '-fcolor-diagnostics', '-fembed-bitcode', '-g', '-O3', '-pipe') or item == source:
            i += 1
            continue
        if item == '-isysroot':
            flags.extend([item, sdk])
            i += 2
            continue
        if item.startswith('-I' + OLD_ROOT + '/'):
            item = '-I' + root + item[len('-I' + OLD_ROOT):]
        elif OLD_ROOT in item:
            raise ValueError('Unexpected captured export path argument')
        flags.append(item)
        i += 1
    flags.extend(['-fsyntax-only', '-ferror-limit=0', '-fno-color-diagnostics'])
    expected = ['-I.'] + ['-I' + root + '/' + name for name in INCLUDES]
    if sorted(x for x in flags if x.startswith('-I')) != sorted(expected):
        raise ValueError('Compiler include paths do not match the requested export')
    if flags.count('-arch') != 1 or flags[flags.index('-arch') + 1] != 'arm64':
        raise ValueError('Require exactly the arm64 architecture')
    if flags.count('-isysroot') != 1 or flags[flags.index('-isysroot') + 1] != sdk:
        raise ValueError('Require exactly the selected iPhoneOS SDK')
    if '-std=c++11' not in flags or '-x' not in flags or flags[flags.index('-x') + 1] != 'c++':
        raise ValueError('Captured C++ language flags missing')
    return flags


def compile_flags(root):
    repo = Path(__file__).resolve().parents[2]
    template = repo / 'Recovery/Bulk389-Qualification-2026-10-23/apple-failure-39988665189/APPLE-COMPILE-COMMAND.json'
    command = json.loads(template.read_text('utf-8'))
    compiler = subprocess.check_output(['xcrun', '--find', 'clang++'], text=True).strip()
    sdk = subprocess.check_output(['xcrun', '--sdk', 'iphoneos', '--show-sdk-path'], text=True).strip()
    return rebind(command, str(root), compiler, sdk)
