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
ranges = {100663313: (6445584864, 6445585440), 100663939: (6445772368, 6445772960), 100663324: (6445580816, 6445581456), 100663319: (6445578832, 6445579488), 100663931: (6445866832, 6445867488), 100663943: (6445776688, 6445777344), 100664511: (6446116640, 6446117296), 100664944: (6446275152, 6446275808), 100664983: (6446288176, 6446288832), 100665042: (6446302320, 6446302976), 100663320: (6445579488, 6445580160), 100664991: (6446289568, 6446290240), 100663371: (6445616304, 6445616992), 100663687: (6445672656, 6445673344), 100663959: (6445767328, 6445768016), 100664156: (6445917504, 6445918192), 100663922: (6445867776, 6445868480), 100663950: (6445780000, 6445780704), 100663919: (6445866112, 6445866832), 100664858: (6446238624, 6446239344), 100663683: (6445671904, 6445672640), 100663720: (6445702576, 6445703312), 100663930: (6445865360, 6445866096), 100664102: (6445895440, 6445896176), 100664537: (6446148912, 6446149648), 100664618: (6446073280, 6446074016), 100664162: (6445923296, 6445924048), 100664907: (6446253168, 6446253920), 100665029: (6446266224, 6446266976), 100665104: (6446323280, 6446324032), 100664690: (6446164752, 6446165520), 100663651: (6445747296, 6445748080), 100663727: (6445697440, 6445698224), 100664117: (6445903040, 6445903840), 100664596: (6446125904, 6446126704), 100664876: (6446247440, 6446248240), 100665019: (6446299904, 6446300704), 100664736: (6446194448, 6446195264), 100663453: (6445649408, 6445650240), 100663576: (6445713760, 6445714592), 100663713: (6445705520, 6445706352), 100664053: (6445873312, 6445874144), 100664196: (6445934544, 6445935376), 100664547: (6446146064, 6446146896), 100664842: (6446204256, 6446205088), 100665014: (6446295936, 6446296768), 100663412: (6445631616, 6445632464), 100663507: (6445592896, 6445593744), 100663741: (6445695968, 6445696816), 100663769: (6445730544, 6445731392), 100664210: (6445943712, 6445944560), 100664796: (6446220944, 6446221792), 100663735: (6445693776, 6445694640), 100663846: (6445827424, 6445828288), 100663857: (6445837008, 6445837872), 100664148: (6445973216, 6445974080), 100665411: (6446413040, 6446413904), 100664692: (6446165968, 6446166848), 100664076: (6445908960, 6445909872), 100664164: (6445921216, 6445922128), 100664437: (6446018624, 6446019536), 100664462: (6446052224, 6446053136), 100663432: (6445647904, 6445648832), 100663766: (6445727888, 6445728832), 100663953: (6445780704, 6445781648), 100664155: (6445916560, 6445917504), 100664582: (6446124944, 6446125904), 100664831: (6446233552, 6446234512), 100664979: (6446286720, 6446287680), 100664727: (6446190768, 6446191744), 100663314: (6445582896, 6445583888), 100664209: (6445942720, 6445943712), 100665081: (6446313904, 6446314912), 100663404: (6445627104, 6445628128), 100664492: (6446070704, 6446071728), 100664978: (6446285632, 6446286656), 100665600: (6446404480, 6446405504), 100663446: (6445645344, 6445646384), 100664785: (6446212896, 6446213936), 100665040: (6446301280, 6446302320), 100663435: (6445643920, 6445644976), 100664199: (6445940752, 6445941808), 100664702: (6446175872, 6446176928), 100665073: (6446311024, 6446312080), 100665080: (6446312304, 6446313360), 100663528: (6445601584, 6445602672), 100664197: (6445938256, 6445939344), 100664709: (6446182592, 6446183680), 100664884: (6446245776, 6446246864), 100665176: (6446330112, 6446331200), 100664467: (6446053792, 6446054896), 100664647: (6446153600, 6446154704)}

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
