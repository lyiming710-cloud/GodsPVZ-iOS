"""Extract bounded original DXBC entries and disassemble with Microsoft's reader.

This reads the authoritative ZIP and writes only the new evidence directory. It
does not use the generic Shader exporter, which discards program code/state.
"""
from pathlib import Path
import ctypes as C
import hashlib
import json
import struct
import sys
import zipfile

W = Path(__file__).resolve().parent
sys.path.insert(0, str(W / 'python-deps'))
import UnityPy
from UnityPy.helpers.CompressionHelper import decompress_lz4

OUT = W / 'custom-shader-evidence' / 'original-dxbc'
OUT.mkdir(exist_ok=True)
compiler = C.WinDLL('d3dcompiler_47.dll')
compiler.D3DDisassemble.argtypes = [C.c_void_p, C.c_size_t, C.c_uint, C.c_char_p, C.POINTER(C.c_void_p)]
compiler.D3DDisassemble.restype = C.c_long

def com(obj, index, result, *args):
    vt = C.cast(obj, C.POINTER(C.POINTER(C.c_void_p))).contents
    fn = C.WINFUNCTYPE(result, C.c_void_p, *[t for t, v in args])(vt[index])
    return fn(obj, *[v for t, v in args])

def disassemble(raw):
    blob = C.c_void_p()
    data = C.create_string_buffer(raw)
    hr = compiler.D3DDisassemble(data, len(raw), 0, None, C.byref(blob))
    if hr < 0:
        raise RuntimeError('D3DDisassemble failed: ' + hex(hr & 0xffffffff))
    try:
        address = com(blob, 3, C.c_void_p)
        size = com(blob, 4, C.c_size_t)
        return C.string_at(address, size).rstrip(b'\0')
    finally:
        com(blob, 2, C.c_ulong)

rows = []
archive = W.parents[1] / 'GodsPVZ_1.0.2.zip'
with zipfile.ZipFile(archive) as z:
    members = [n for n in z.namelist() if n.endswith('/resources.assets')]
    assert len(members) == 1
    env = UnityPy.load(z.read(members[0]))
    for obj in env.objects:
        if obj.type.name != 'Shader' or obj.path_id not in (871, 886):
            continue
        shader = obj.read()
        name = shader.m_ParsedForm.m_Name
        assert name in ('Shader Graphs/Brightness', 'Unlit/GameUnitShader')
        for pi, platform in enumerate(shader.platforms):
            assert platform == 4, (name, platform)
            segments = [decompress_lz4(bytes(shader.compressedBlob)[o:o+c], s)
                        for o, c, s in zip(shader.offsets[pi], shader.compressedLengths[pi], shader.decompressedLengths[pi])]
            count = struct.unpack_from('<I', segments[0])[0]
            for i in range(count):
                offset, size, segment = struct.unpack_from('<III', segments[0], 4 + 12*i)
                raw = segments[segment][offset:offset+size]
                assert len(raw) == size
                starts = [j for j in range(len(raw)-4) if raw[j:j+4] == b'DXBC']
                if not starts:
                    continue  # Indexed parameter entries contain no GPU code.
                assert len(starts) == 1, (name, i, starts)
                start = starts[0]
                length, chunks = struct.unpack_from('<II', raw, start+24)
                assert 32 + 4*chunks <= length <= len(raw)-start
                dxbc = raw[start:start+length]
                for j in range(chunks):
                    chunk = struct.unpack_from('<I', dxbc, 32 + 4*j)[0]
                    assert chunk + 8 <= length
                    n = struct.unpack_from('<I', dxbc, chunk+4)[0]
                    assert chunk + 8 + n <= length
                filename = name.replace('/', '_').replace(' ', '_') + '-program' + str(i)
                asm = disassemble(dxbc)
                (OUT / (filename + '.dxbc')).write_bytes(dxbc)
                (OUT / (filename + '.asm.txt')).write_bytes(asm + b'\n')
                row = dict(shader=name, path_id=obj.path_id, shader_raw_sha256=hashlib.sha256(obj.get_raw_data()).hexdigest(),
                           platform=platform, indexed_entry=i, entry_offset=offset, entry_bytes=size,
                           dxbc_offset_in_entry=start, dxbc_bytes=length, dxbc_sha256=hashlib.sha256(dxbc).hexdigest(),
                           file=filename+'.dxbc', disassembly_sha256=hashlib.sha256(asm+b'\n').hexdigest())
                rows.append(row)
                print(json.dumps(row), flush=True)
(OUT / 'MANIFEST.json').write_bytes((json.dumps(rows, indent=2)+'\n').encode())
assert len(rows) >= 6
