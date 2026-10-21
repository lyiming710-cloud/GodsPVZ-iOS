"""Read immutable IL/metadata and disassemble original machine code. No target/test execution."""
from pathlib import Path
import hashlib
import json
import struct
import sys

ROOT = Path(__file__).resolve().parents[2]
OUT = Path(__file__).resolve().parent
sys.dont_write_bytecode = True
sys.path.insert(0, str(ROOT / 'audit/stage9-native3/python-deps'))
import dnfile
import pefile
from capstone import Cs, CS_ARCH_X86, CS_MODE_64, CS_OP_MEM
from capstone.x86_const import X86_REG_RIP
from dncil.cil.body import CilMethodBody
from dncil.cil.body.reader import CilMethodBodyReaderBytes

INPUT = ROOT / 'audit/offline-test-handoff-2026-10-14/payload/inputs'
PARENT = ROOT / 'audit/native93-codex-2026-10-17/verification-2026-10-17/candidates/candidate1.dll'
raw = PARENT.read_bytes()
if hashlib.sha256(raw).hexdigest() != 'b074224112d46a959de074d3090ab6d0614b036959c66c63b4f49954472e2162':
    raise ValueError('Unexpected source snapshot')
managed = dnfile.dnPE(data=raw)
tables = managed.net.mdtables
ga = INPUT / 'original-pc/GameAssembly.dll'
native = pefile.PE(str(ga))
base = native.OPTIONAL_HEADER.ImageBase
meta = (INPUT / 'original-pc/global-metadata.dat').read_bytes()
maps = json.loads((INPUT / 'maps/native-method-map.json').read_text('utf-8-sig'))
field_maps = json.loads((INPUT / 'maps/native-fields.json').read_text('utf-8-sig'))
owners = {m.row_index: t for t in tables.TypeDef.rows for m in t.MethodList}
field_owners = {f.row_index: t for t in tables.TypeDef.rows for f in t.FieldList}

def typename(t):
    return (str(t.TypeNamespace) + '.' if str(t.TypeNamespace) else '') + str(t.TypeName)

def identity(token):
    tab, rid = token >> 24, token & 0xFFFFFF
    if tab == 6: return typename(owners[rid]) + '::' + str(tables.MethodDef.rows[rid - 1].Name)
    if tab == 4: return typename(field_owners[rid]) + '::' + str(tables.Field.rows[rid - 1].Name)
    if tab in (1, 2): return typename((tables.TypeRef if tab == 1 else tables.TypeDef).rows[rid - 1])
    if tab == 10:
        m = tables.MemberRef.rows[rid - 1]
        owner = typename(m.Class.row) if m.Class.table.name in ('TypeRef', 'TypeDef') else m.Class.table.name + ':' + str(m.Class.row_index)
        return owner + '::' + str(m.Name) + ' / sig ' + m.Signature.value.hex()
    if tab == 0x70: return managed.net.user_strings.get(rid).value
    return hex(token)

# Extents end at next distinct original method pointer; include all pdata fragments and padding.
ranges = {100663495: (6445652592, 6445653367), 100664070: (6445903840, 6445904660), 100664805: (6446230448, 6446231114), 100663742: (6445696816, 6445697435), 100664444: (6446022464, 6446023283), 100664440: (6446019904, 6446020881), 100664325: (6446114784, 6446115388), 100664319: (6446115824, 6446116495), 100664867: (6446241888, 6446242594), 100664650: (6446152320, 6446153011), 100665004: (6446291968, 6446292581), 100664052: (6445872704, 6445873302), 100664205: (6445945872, 6445946512), 100664488: (6446067216, 6446067840), 100664487: (6446066528, 6446067206), 100665596: (6446403904, 6446404466), 100665595: (6446401680, 6446402668), 100665597: (6446402672, 6446403896), 100665598: (6446399600, 6446400631), 100665599: (6446400640, 6446401671)}

current = []
original = []
calls = set()
cells = {}
constants = {}
decoder = Cs(CS_ARCH_X86, CS_MODE_64)
decoder.detail = True
u32 = lambda b, p=0: struct.unpack_from('<I', b, p)[0]
read_native = lambda va, n: native.get_data(va - base, n)

for token, (start, end) in ranges.items():
    m = tables.MethodDef.rows[(token & 0xFFFFFF) - 1]
    b = CilMethodBody(CilMethodBodyReaderBytes(managed.get_data(m.Rva, 8192)))
    local = b.local_var_sig_tok.value if b.local_var_sig_tok else 0
    rec = dict(token=f'0x{token:08X}', name=identity(token), signature=m.Signature.value.hex(),
               rva=m.Rva, max_stack=b.max_stack, code_size=b.code_size, local_sig=local,
               locals_blob=tables.StandAloneSig.rows[(local & 0xFFFFFF) - 1].Signature.value.hex() if local else '',
               eh_count=len(b.exception_handlers),
               il_sha256=hashlib.sha256(b.raw_bytes[b.header_size:b.header_size + b.code_size]).hexdigest(), instructions=[])
    for i in b.instructions:
        op = i.operand
        value = None if op is None else str(op)
        if hasattr(op, 'value'): value = dict(token=f'0x{op.value:08X}', identity=identity(op.value))
        elif isinstance(op, int) and i.opcode.flow_control.name in ('Branch', 'Cond_Branch'):
            value = dict(target=op - b.header_size)
        elif isinstance(op, list): value = [x - b.header_size for x in op]
        rec['instructions'].append(dict(offset=i.offset - b.header_size, opcode=str(i.opcode), operand=value))
    current.append(rec)
    text = [rec['name'], json.dumps({k: v for k, v in rec.items() if k != 'instructions'})]
    text.extend(f"IL_{i['offset']:04X}: {i['opcode']:<14} {i['operand']}" for i in rec['instructions'])
    (OUT / (rec['token'] + '-current-il.txt')).write_text('\n'.join(text), encoding='utf-8')

    code = read_native(start, end - start)
    lines = []
    instructions = []
    for i in decoder.disasm(code, start):
        notes = []
        for op in i.operands:
            if op.type == CS_OP_MEM and op.mem.base == X86_REG_RIP:
                va = i.address + i.size + op.mem.disp
                notes.append(hex(va))
                if 0x181B00000 <= va < 0x181CE0000:
                    value = struct.unpack('<Q', read_native(va, 8))[0]
                    kind, index = (value >> 29) & 7, (value >> 1) & 0xFFFFFFF
                    c = dict(address=hex(va), bytes=read_native(va, 8).hex(), kind=kind, index=index)
                    if kind == 5:
                        size, offset = struct.unpack_from('<II', meta, u32(meta, 8) + 8 * index)
                        payload = meta[u32(meta, 16) + offset:u32(meta, 16) + offset + size]
                        c.update(text=payload.decode('utf-8'), utf8_hex=payload.hex())
                    cells[hex(va)] = c
                elif 0x181500000 <= va < 0x181B00000:
                    constants[hex(va)] = dict(bytes=read_native(va, 16).hex(), float32=struct.unpack('<f', read_native(va, 4))[0])
        if i.mnemonic in ('call', 'jmp') and i.op_str.startswith('0x'):
            calls.add(int(i.op_str, 16))
        instructions.append(dict(address=hex(i.address), bytes=i.bytes.hex(), mnemonic=i.mnemonic, operands=i.op_str))
        lines.append(f'{i.address:016X} {i.mnemonic:<9} {i.op_str}' + (' # ' + ', '.join(notes) if notes else ''))
    original.append(dict(token=f'0x{token:08X}', start=hex(start), end=hex(end), bytes=code.hex(),
                         sha256=hashlib.sha256(code).hexdigest(), instructions=instructions,
                         pdata=[dict(start=hex(base + e.struct.BeginAddress), end=hex(base + e.struct.EndAddress))
                                for e in native.DIRECTORY_ENTRY_EXCEPTION if start <= base + e.struct.BeginAddress < end]))
    (OUT / (f'0x{token:08X}-original.asm')).write_text('\n'.join(lines), encoding='utf-8')

selected = {str(t.TypeName) for t in tables.TypeDef.rows}

fields = [dict(token=f'0x{0x04000000 | rid:08X}', owner=typename(field_owners[rid]), name=str(f.Name),
               signature=f.Signature.value.hex(), public=bool(f.Flags.fdPublic), static=bool(f.Flags.fdStatic))
          for rid, f in enumerate(tables.Field.rows, 1) if str(field_owners[rid].TypeName) in selected]
methods = [dict(token=f'0x{0x06000000 | rid:08X}', name=identity(0x06000000 | rid), signature=m.Signature.value.hex())
           for rid, m in enumerate(tables.MethodDef.rows, 1)]
refs = [dict(token=f'0x{0x0A000000 | rid:08X}', identity=identity(0x0A000000 | rid), signature=r.Signature.value.hex())
        for rid, r in enumerate(tables.MemberRef.rows, 1)]
data = dict(scope='SOURCE_DATA_READ_ONLY_NO_TARGET_OR_TEST_EXECUTION', source_sha256=hashlib.sha256(raw).hexdigest(),
            methods=current, fields=fields, method_index=methods, member_refs=refs)
(OUT / 'CURRENT-SOURCE.json').write_text(json.dumps(data, ensure_ascii=False, indent=2), encoding='utf-8')
data = dict(scope='ORIGINAL_DATA_DISASSEMBLY_ONLY_NO_TARGET_EXECUTION',
            gameassembly_sha256=hashlib.sha256(ga.read_bytes()).hexdigest(),
            metadata_sha256=hashlib.sha256(meta).hexdigest(), methods=original,
            calls=[m for m in maps if int(m['va'], 16) in calls],
            fields=[f for f in field_maps if f['type'] in selected], cells=list(cells.values()), constants=constants)
(OUT / 'ORIGINAL-SOURCE.json').write_text(json.dumps(data, ensure_ascii=False, indent=2), encoding='utf-8')
print(json.dumps({'scope':data['scope'], 'tokens':list(ranges), 'native_sizes':[len(bytes.fromhex(x['bytes'])) for x in original]}))
