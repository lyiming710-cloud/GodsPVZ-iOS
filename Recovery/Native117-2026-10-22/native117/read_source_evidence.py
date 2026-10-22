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
ranges = {100665192: (6446370496, 6446370880), 100665229: (6446387104, 6446387504), 100665235: (6446387568, 6446388080), 100665120: (6446391632, 6446392272), 100664944: (6446275152, 6446275808), 100665215: (6446390944, 6446391632), 100663950: (6445780000, 6445780704), 100663930: (6445865360, 6445866096), 100665307: (6446383280, 6446384064), 100664736: (6446194448, 6446195264), 100663412: (6445631616, 6445632464), 100663769: (6445730544, 6445731392), 100664210: (6445943712, 6445944560), 100664796: (6446220944, 6446221792), 100663735: (6445693776, 6445694640), 100664148: (6445973216, 6445974080), 100665411: (6446413040, 6446413904), 100664692: (6446165968, 6446166848), 100664076: (6445908960, 6445909872), 100664437: (6446018624, 6446019536), 100664462: (6446052224, 6446053136), 100663432: (6445647904, 6445648832), 100665608: (6446436224, 6446437152), 100663766: (6445727888, 6445728832), 100663953: (6445780704, 6445781648), 100664582: (6446124944, 6446125904), 100664831: (6446233552, 6446234512), 100664727: (6446190768, 6446191744), 100663314: (6445582896, 6445583888), 100664209: (6445942720, 6445943712), 100665081: (6446313904, 6446314912), 100663404: (6445627104, 6445628128), 100664492: (6446070704, 6446071728), 100665600: (6446404480, 6446405504), 100663446: (6445645344, 6445646384), 100664785: (6446212896, 6446213936), 100665040: (6446301280, 6446302320), 100663435: (6445643920, 6445644976), 100664199: (6445940752, 6445941808), 100664702: (6446175872, 6446176928), 100665073: (6446311024, 6446312080), 100665080: (6446312304, 6446313360), 100663528: (6445601584, 6445602672), 100664197: (6445938256, 6445939344), 100664709: (6446182592, 6446183680), 100664884: (6446245776, 6446246864), 100665176: (6446330112, 6446331200), 100664467: (6446053792, 6446054896), 100664647: (6446153600, 6446154704), 100664237: (6445956176, 6445957296), 100663771: (6445719184, 6445720320), 100663969: (6445782240, 6445783376), 100664520: (6446149840, 6446150976), 100663424: (6445638016, 6445639168), 100664160: (6445922128, 6445923296), 100663691: (6445676272, 6445677456), 100664495: (6446064256, 6446065440), 100665167: (6446389600, 6446390816), 100664214: (6445947008, 6445948240), 100664669: (6446160688, 6446161920), 100663363: (6445612256, 6445613504), 100663858: (6445839712, 6445840976), 100663859: (6445838448, 6445839712), 100664257: (6446090192, 6446091472), 100663346: (6445606192, 6445607488), 100663719: (6445701280, 6445702576), 100664442: (6446020896, 6446022192), 100664745: (6446198768, 6446200064), 100664163: (6445919904, 6445921216), 100664377: (6445985856, 6445987168), 100664116: (6445901344, 6445902672), 100664698: (6446168192, 6446169520), 100665035: (6446235744, 6446237088), 100663384: (6445619968, 6445621328), 100663850: (6445832848, 6445834208), 100663518: (6445593744, 6445595136), 100665247: (6446384128, 6446385536), 100664095: (6445888736, 6445890160), 100664097: (6445892240, 6445893664), 100665253: (6446385600, 6446387040)}

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
